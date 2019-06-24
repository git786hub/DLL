using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using Intergraph.GTechnology.API;
using ADODB;
using SendEmail;
using System.Collections;
using System.Web.Mvc;

namespace GTechnology.Oncor.CustomAPI
{
	public class BatchProcessingModule : IGISAutoPlugin
	{

		// Flag to control extra debug logging
		private bool logdebugmessages = true;

		private string message = string.Empty;
		private string status = "SUCCESS";
		private short commonCNO = 1;
		private short serviceLineCNO = 5401;
		private string resultCode;
		private string batchtype = "";
		NJUNSAutomationHelper helper;
		SendReceiveXMLMessage XMLMessenger = new SendReceiveXMLMessage();
		IGTApplication gtApp = GTClassFactory.Create<IGTApplication>();
		ArrayList messages = new ArrayList();
		IGTDataContext dataContext;
		JobManager JobManager = new JobManager();
		IGTJobManagementService jobManagement = GTClassFactory.Create<IGTJobManagementService>();
		IGTTransactionManager transactionManager;
		string g_Request_Type;
		string g_WorkRequest_Number;
		string g_Trans_Date;
		string g_TransactionID;
		private string originatingJobStatus = string.Empty;
		private string newJobStatus = string.Empty;

		public string SystemName => throw new NotImplementedException();

		struct FNOFID { public short FNO; public int FID; }

		enum DeletionMode { deleteOwned, deleteOwning };

		public BatchProcessingModule(IGTTransactionManager gTTransactionManager)
		{
			transactionManager = gTTransactionManager;
		}

		private void debugWriteToCommandLog(string logMessage)
		{
			if(logdebugmessages)
			{
				// COMMAND_LOG.LOG_MESSAGE is only 1000 characters wide. 
				if(logMessage.Length > 1000)
				{
					logMessage = logMessage.Substring(0, 999);
				}

				WriteToCommandLog("INFO", logMessage, g_WorkRequest_Number);
			}
		}

		/// <summary>
		/// Calls logger class to log message to COMMAND_LOG table.
		/// </summary>
		/// <param name="logType">The type of message to log - INFO, ERROR, ...</param>
		/// <param name="logMessage">The message to log</param>
		/// <param name="logContext">The context for the message</param>
		private void WriteToCommandLog(string logType, string logMessage, string logContext)
		{
			gtCommandLogger.gtCommandLogger gtCommandLogger = new gtCommandLogger.gtCommandLogger();
			gtCommandLogger.CommandNum = -1;
			gtCommandLogger.CommandName = "GISAutomationBroker";
			gtCommandLogger.LogType = logType;

			if(string.IsNullOrEmpty(logMessage))
			{
				logMessage = "";
			}

			if(logMessage.Length > 1000)
			{
				logMessage = logMessage.Substring(0, 999);
			}

			gtCommandLogger.LogMsg = logMessage;

			if(string.IsNullOrEmpty(logContext))
			{
				logContext = "";
			}

			gtCommandLogger.LogContext = logContext;

			gtCommandLogger.logEntry();

			gtCommandLogger = null;

			Commit();
		}

		public void Execute(GISAutoRequest autoRequest)
		{
			dataContext = gtApp.DataContext;
			jobManagement.DataContext = dataContext;

			ProcessTransaction(autoRequest.requestXML);
		}

		public void ProcessTransaction(string xmlMessage)
		{
			try
			{
				debugWriteToCommandLog("Starting Transaction");
				debugWriteToCommandLog(xmlMessage);

				GetTransactionMemberValues(xmlMessage);

				// Session Management prevents a job from being activated
				// when its WMIS_STATUS_C is BATCH so temporarily set it to SUCCESS
				// until after the job is activated and then set it back to BATCH.
				if("BATCH" == JobManager.WMISStatus)
				{
					// Prefer NULL but JobManager doesn't have a way to do that (yet).
					JobManager.WMISStatus = "SUCCESS";
					Commit();
				}

				jobManagement.EditJob(g_WorkRequest_Number);

				// Cache the originating job state (in case we need to set it back after a failure)
				this.originatingJobStatus = JobManager.JobStatus;

				// Log the initial value of G3E_JOBSTATUS.
				debugWriteToCommandLog(string.Format("Initial Job Status: {0}", this.originatingJobStatus));

				JobManager.WMISStatus = "BATCH";
				Commit();

				StartTransaction();

				if(!string.IsNullOrEmpty(g_Request_Type))
				{
					switch(g_Request_Type)
					{
						case "APPRV":
							batchtype = "Approval";
							Approval();
							break;
						case "INSRV":
							batchtype = "Completion";
							Completion();
							break;
						case "CLOSE":
							batchtype = "Closure";
							Closure();
							break;
						case "CANC":
						case "CANCEL":
							batchtype = "Cancel";
							Cancel();
							break;
						default:
							transactionManager.Rollback();
							//error for incorrect transaction type
							status = "FAILURE";
							message = string.Format("Invalid Request Type: {0}", g_Request_Type);
							break;
					}
				}
				else
				{
					Rollback();
					UpdateJobColumn("wmis_status_c", "FAILURE");
					throw new Exception("Received an empty request type.");
					JobManager.DeactivateJob(true);
				}

				if(status.ToUpper() == "SUCCESS")
				{
					// Delete the Request and set the record to SUCCESS along with associated messages
					// The job manager cannot be used to set the WMIS Status after the Closure method
					// as the job is no longer active.

					// This may not really be necessary but just in case something is still outstanding
					// at the end of a transaction, then ensure it is preserved.
					CommitTransaction();


					// If job is still in edit status, it can't be reactivated.
					jobManagement.EndEditJob();

					UpdateJobColumn("wmis_status_c", "SUCCESS");

					DeleteRequest();
					MessageProcessing();
					Commit();
				}
				else
				{
					Rollback();
					Commit();

					if(batchtype == "Completion")
					{
						// For Completion transactions, the job is expected to still be active
						// so we can discard it without needing to reactivate it.
						jobManagement.DiscardJob();
						Commit();
					}

					// We can't rely on the job still being active for any/all transaction types so,
					// instead of using the JobManager, perform these updates using direct database methods in this class.

					UpdateJobColumn("wmis_status_c", "FAILURE");

					// If the job status has been set, then set it back to what it was when the transaction started.
					if(!string.IsNullOrEmpty(this.newJobStatus))
					{
						UpdateJobColumn("g3e_jobstatus", originatingJobStatus);
					}

					// If job is still in edit status, it can't be reactivated.
					jobManagement.EndEditJob();

					// Error for failed transaction, update the record
					// Email user to notify them of the failure
					SendErrorEmail();
				}

			}
			catch(Exception ex)
			{
				// If something catastrophic occurs in any of the batch modes, this is the last place where
				// an exception will be caught before throwing the exception back to the broker.
				// Discard edits and reset job properties as needed and then throw the exception again
				// so that the broker can log it, etc.

				Rollback();
				Commit();

				UpdateJobColumn("wmis_status_c", "FAILURE");

				// If the job status has been set, then set it back to what it was when the transaction started.
				if(!string.IsNullOrEmpty(this.newJobStatus))
				{
					UpdateJobColumn("g3e_jobstatus", originatingJobStatus);
				}

				// If job is still in edit status, it can't be reactivated.
				jobManagement.EndEditJob();

				string exMsg = string.Format("Error occurred in {0} of {1}.{2}{3}", System.Reflection.MethodBase.GetCurrentMethod().Name, this.ToString(), Environment.NewLine, ex.Message);
				throw new Exception(exMsg);
			}
		}

		private void sendToWMIS()
		{
			try
			{
				XmlDocument wmisResponse = new XmlDocument();

				if(status == "FAILURE")
				{
					XmlElement failureInfo = (XmlElement)wmisResponse.AppendChild(wmisResponse.CreateElement("ResultType"));
					failureInfo.SetAttribute("Status", status);
					failureInfo.SetAttribute("ResultCode", resultCode);
					failureInfo.SetAttribute("Message", message);
					failureInfo.SetAttribute("TimeStamp", DateTime.Now.ToString("o"));
				}
				else
				{
					XmlElement failureInfo = (XmlElement)wmisResponse.AppendChild(wmisResponse.CreateElement("ResultType"));
					failureInfo.SetAttribute("Status", status);
				}
			}
			catch(Exception ex)
			{
				string exMsg = string.Format("Error occurred in {0} of {1}.{2}{3}", System.Reflection.MethodBase.GetCurrentMethod().Name, this.ToString(), Environment.NewLine, ex.Message);
				throw new Exception(exMsg);
			}
		}

		internal void GetTransactionMemberValues(string xmlMessage)
		{
			try
			{
				if(!string.IsNullOrEmpty(xmlMessage))
				{
					XDocument doc = XDocument.Parse(xmlMessage);
					XNamespace ns0 = "http://www.oncor.com/GIS/BatchMgmt";
					XNamespace ns1 = "http://www.oncor.com/DIS";

					XElement e0 = doc.Element(ns0 + "RequestBatchRequest");
					XElement e1 = e0.Element(ns1 + "RequestHeader");

					g_Trans_Date = e1.Element(ns1 + "Timestamp").Value;
					g_TransactionID = e1.Element(ns1 + "TransactionId").Value;

					g_WorkRequest_Number = e0.Element(ns0 + "WRNumber").Value;
					g_Request_Type = e0.Element(ns0 + "BatchType").Value;
				}
			}
			catch(Exception ex)
			{
				string exMsg = string.Format("Error occurred in {0} of {1}.{2}{3}", System.Reflection.MethodBase.GetCurrentMethod().Name, this.ToString(), Environment.NewLine, ex.Message);
				throw new Exception(exMsg);
			}
		}

		internal void Approval()
		{
			try
			{
				debugWriteToCommandLog("Starting Approval");

				ValidateAndPost();

				if(status == "FAILURE")
				{
					return;
				}

				debugWriteToCommandLog("Setting Job Type");

				if(string.IsNullOrEmpty(message))
				{
					if(JobManager.JobType == "WR-MAPCOR")
					{
						JobManager.JobStatus = "ConstructionComplete";
						this.newJobStatus = "ConstructionComplete";
					}
					else
					{
						JobManager.JobStatus = "AsBuilt";
						this.newJobStatus = "AsBuilt";
					}

					Commit();
				}
			}
			catch(Exception ex)
			{
				string exMsg = string.Format("Error occurred in {0} of {1}.{2}{3}", System.Reflection.MethodBase.GetCurrentMethod().Name, this.ToString(), Environment.NewLine, ex.Message);
				throw new Exception(exMsg);
			}
		}

		internal void Completion()
		{
			try
			{
				debugWriteToCommandLog("Starting Completion");

				Recordset pe = jobManagement.FindPendingEdits();

				if(null != pe && 0 < pe.RecordCount)
				{
					message = "Could not complete due to unposted edits";
					status = "FAILURE";
					pe.Close();
					pe = null;
				}
				else
				{
					string queryEdits = "SELECT DISTINCT G3E_FID, G3E_FNO FROM ASSET_HISTORY WHERE G3E_IDENTIFIER = ?";
					Recordset jobedits = dataContext.OpenRecordset(queryEdits, CursorTypeEnum.adOpenDynamic, LockTypeEnum.adLockOptimistic, (int)CommandTypeEnum.adCmdText, dataContext.ActiveJob);

					if(null != jobedits && 0 < jobedits.RecordCount)
					{
						jobedits.MoveFirst();

						do
						{
							short FNO = Convert.ToInt16(jobedits.Fields["G3E_FNO"].Value);
							int FID = Convert.ToInt32(jobedits.Fields["G3E_FID"].Value);

							if(!ServiceLineIsAssociated(FNO, FID))
							{
								Recordset rsCommon = ComponentRecordset(FNO, FID, 1);

								if(null != rsCommon)
								{
									rsCommon.MoveFirst();
									string newState = string.Empty;

									switch(rsCommon.Fields["FEATURE_STATE_C"].Value.ToString())
									{
										case "PPI":
										case "PPX":
										case "ABI":
										case "ABX":
											newState = "INI";
											break;
										case "PPR":
										case "ABR":
											newState = "OSR";
											break;
										case "PPA":
										case "ABA":
											newState = "OSA";
											break;
									}

									if(!string.IsNullOrEmpty(newState))
									{
										rsCommon.Fields["FEATURE_STATE_C"].Value = newState;
									}
								}
							}

							jobedits.MoveNext();
						} while(!jobedits.EOF);
					}

					UpdateAsOperatedStatus();

					debugWriteToCommandLog("Running UpdateTrace for AsBuilt.");

					CommitTransaction();
					StartTransaction();

					if(!UpdateTrace())
					{
						return;
					}

					if(status != "FAILURE")
					{
						debugWriteToCommandLog("Setting Job Status");
						JobManager.JobStatus = "ConstructionComplete";
						this.newJobStatus = "ConstructionComplete";

						debugWriteToCommandLog("Running UpdateTrace for ConstructionComplete.");

						CommitTransaction();
						StartTransaction();

						if(!UpdateTrace())
						{
							return;
						}

						ValidateAndPost();
					}
				}
			}
			catch(Exception ex)
			{
				string exMsg = string.Format("Error occurred in {0} of {1}.{2}{3}", System.Reflection.MethodBase.GetCurrentMethod().Name, this.ToString(), Environment.NewLine, ex.Message);
				throw new Exception(exMsg);
			}
		}


		/// <summary>
		/// Update As-Operated Status for any features where the current job has changed the Normal Status without having caused a feature state transition:
		/// 1. Query asset history to find any entries showing that the Normal Status has changed and been posted.
		/// 2. Check pending edits for any features where As-Operated Status does not equal Normal Status.
		/// 3. For all features found in the previous two steps, provided Normal Status is not NULL, set As-Operated Status equal to Normal Status.
		/// </summary>
		private void UpdateAsOperatedStatus()
		{
			try
			{
				// List to contain features in both query results

				List<FNOFID> features = new List<FNOFID>();

				// Get the pertinent features from ASSET_HISTORY
				string sql = "select distinct g3e_fid,g3e_fno from asset_history ah" +
										 " join g3e_attributeinfo_optable ai on ah.g3e_ano=ai.g3e_ano" +
										 " where ah.g3e_identifier=?" +
										 " and ai.g3e_field='STATUS_OPERATED_C'";

				Recordset rs = dataContext.OpenRecordset(sql, CursorTypeEnum.adOpenStatic, LockTypeEnum.adLockReadOnly, (int)CommandTypeEnum.adCmdText, dataContext.ActiveJob);

				if(null != rs && 0 < rs.RecordCount)
				{
					rs.MoveFirst();

					do
					{
						FNOFID feature = new FNOFID();
						feature.FNO = Convert.ToInt16(rs.Fields["g3e_fno"].Value);
						feature.FID = Convert.ToInt32(rs.Fields["g3e_fid"].Value);
						features.Add(feature);
						rs.MoveNext();
					} while(!rs.EOF);
				}

				// Get the pertinent features from PENDING_EDITS
				rs = jobManagement.FindPendingEdits();

				if(null != rs && 0 < rs.RecordCount)
				{
					rs.MoveFirst();

					do
					{
						FNOFID feature = new FNOFID();
						feature.FNO = Convert.ToInt16(rs.Fields["g3e_fno"].Value);
						feature.FID = Convert.ToInt32(rs.Fields["g3e_fid"].Value);

						// Get the connectivity recordset
						Recordset crs = ComponentRecordset(feature.FNO, feature.FID, 11);

						if(null != crs)
						{
							string normalStatus = fieldValue(crs.Fields["status_normal_c"], string.Empty);
							string asOpStatus = fieldValue(crs.Fields["status_operated_c"], string.Empty);

							// If Normal Status is not NULL and Normal Status != As-Operated Status, then add the feature.
							if(!string.IsNullOrEmpty(normalStatus) && !string.Equals(normalStatus, asOpStatus))
							{
								features.Add(feature);
							}
						}

						rs.MoveNext();
					} while(!rs.EOF);
				}

				if(null != rs)
				{
					if(rs.State == (int)ObjectStateEnum.adStateOpen)
					{
						rs.Close();
					}

					rs = null;
				}

				// Set As-Operated Status to Normal Status
				foreach(FNOFID feature in features)
				{
					Recordset crs = ComponentRecordset(feature.FNO, feature.FID, 11);

					if(null != crs)
					{
						crs.MoveFirst();
						string normalStatus = fieldValue(crs.Fields["status_normal_c"], string.Empty);

						if(!string.IsNullOrEmpty(normalStatus))
						{
							crs.Fields["status_operated_c"].Value = normalStatus;
						}
					}
				}
			}
			catch(Exception ex)
			{
				string exMsg = string.Format("Error occurred in {0} of {1}.{2}{3}", System.Reflection.MethodBase.GetCurrentMethod().Name, this.ToString(), Environment.NewLine, ex.Message);
				throw new Exception(exMsg);
			}
		}

		/// <summary>
		/// Performs an Update Trace using the UpdateTrace class.
		/// </summary>
		/// <returns>true if no exceptions are thrown; else, false</returns>
		internal bool UpdateTrace()
		{
			bool retVal = true;

			try
			{
				// UpdateTrace requires the CCNO for the current command.
				Recordset rs = gtApp.DataContext.MetadataRecordset("G3E_CUSTOMCOMMAND_OPTABLE");
				rs.Filter = "G3E_USERNAME='GIS Automation Broker'";

				// This should NEVER happen because we would not get here in the first place but to be safe (perhaps someone changes the name of the command)...
				if(null == rs || 0 == rs.RecordCount)
				{
					message = "Could not completely process the Completion request.  Custom Command: 'GIS Automation Broker' not found.";
					status = "FAILURE";
					MessageProcessing();
					retVal = false;
				}
				else
				{
					int ccno = Convert.ToInt32(rs.Fields["g3e_ccno"].Value);

					string sql = "select distinct coalesce(c.pp_feeder_1_id,c.feeder_1_id)" +
											" from asset_history ah" +
											" join connectivity_n c on ah.g3e_fid=c.g3e_fid" +
											" where ah.g3e_identifier=?" +
											" and 1 is not null" +
											" order by 1";
					rs = dataContext.OpenRecordset(sql, CursorTypeEnum.adOpenStatic, LockTypeEnum.adLockReadOnly, (int)CommandTypeEnum.adCmdText, dataContext.ActiveJob);

					if(null != rs && 0 < rs.RecordCount)
					{
						rs.MoveFirst();

						do
						{
							// Use each FEEDER_1_ID to find the breaker
							string feeder = rs.Fields[0].Value.ToString();
							sql = "select g3e_fno,g3e_fid from connectivity_n where g3e_fno in(16,91) and feeder_1_id=? order by 1";
							Recordset rsb = dataContext.OpenRecordset(sql, CursorTypeEnum.adOpenStatic, LockTypeEnum.adLockReadOnly, (int)CommandTypeEnum.adCmdText, feeder);

							if(null != rsb && 0 < rsb.RecordCount)
							{
								rsb.MoveFirst();

								// Assume there should only be one breaker found but iterate just in case.
								do
								{
									short FNO = Convert.ToInt16(rsb.Fields["g3e_fno"].Value);
									int FID = Convert.ToInt32(rsb.Fields["g3e_fid"].Value);

									// Need new UpdateTrace each iteration because
									// the cleanup method causes null reference errors
									// if the object is reused.
									UpdateTrace ut = new UpdateTrace(ccno, "GIS Automation Broker");
									retVal = ut.Execute(FNO, FID);

									if(!retVal)
									{
										status = "FAILURE";

										if(string.IsNullOrEmpty(message))
										{
											message = string.Format("Failure occurred during call to Update Trace.  {0}", ut.ErrorMessage);
										}
										else
										{
											message = string.Format("{0}  Failure occurred during call to Update Trace.  {1}", message, ut.ErrorMessage);
										}
									}

									ut = null;

									rsb.MoveNext();
								} while(!rsb.EOF && retVal);
							}

							if(null != rsb)
							{
								if(rsb.State == (int)ObjectStateEnum.adStateOpen)
								{
									rsb.Close();
								}
								rsb = null;
							}

							// Iterate all feeder IDs in the job
							rs.MoveNext();
						} while(!rs.EOF && retVal);
					}
				}

				if(null != rs)
				{
					if(rs.State == (int)ObjectStateEnum.adStateOpen)
					{
						rs.Close();
					}
					rs = null;
				}

			}
			catch(Exception ex)
			{
				string exMsg = string.Format("Error occurred in {0} of {1}.{2}{3}", System.Reflection.MethodBase.GetCurrentMethod().Name, this.ToString(), Environment.NewLine, ex.Message);
				throw new Exception(exMsg);
			}

			return retVal;
		}

		/// <summary>
		/// Returns a true if Service Line can be deleted during
		/// batch operations (i.e. Retirement Code not 1 or 2), else false
		/// </summary>
		/// <param name="FNO"></param>
		/// <param name="FID"></param>
		internal bool ServiceLineIsAssociated(short FNO, int FID)
		{
			bool retVal = false;

			try
			{
				if(54 == FNO)
				{
					Recordset rsSvcLine = ComponentRecordset(FNO, FID, serviceLineCNO);

					if(null != rsSvcLine)
					{
						rsSvcLine.MoveFirst();

						if(DBNull.Value != rsSvcLine.Fields["PLACEMENT_TYPE_C"].Value)
						{
							if(rsSvcLine.Fields["PLACEMENT_TYPE_C"].Value.ToString() == "ASSOCIATED")
							{
								retVal = true;
							}
						}
					}
				}
			}
			catch(Exception ex)
			{
				string exMsg = string.Format("Error occurred in {0} of {1}.{2}{3}", System.Reflection.MethodBase.GetCurrentMethod().Name, this.ToString(), Environment.NewLine, ex.Message);
				throw new Exception(exMsg);
			}

			return retVal;
		}

		/// <summary>
		/// Determines whether a feature has valid owners.
		/// </summary>
		/// <param name="FNO"></param>
		/// <param name="FID"></param>
		/// <returns>true if either owner is not null and != 0; else, false.</returns>
		internal bool FeatureIsOwned(short FNO, int FID)
		{
			bool retVal = false;

			try
			{
				Recordset rsCommon = ComponentRecordset(FNO, FID, 1);

				if(null != rsCommon && 0 < rsCommon.RecordCount)
				{
					rsCommon.MoveFirst();

					if(DBNull.Value != rsCommon.Fields["OWNER1_ID"].Value && 0 != Convert.ToInt32(rsCommon.Fields["OWNER1_ID"].Value))
					{
						retVal = true;
					}

					if(DBNull.Value != rsCommon.Fields["OWNER2_ID"].Value && 0 != Convert.ToInt32(rsCommon.Fields["OWNER2_ID"].Value))
					{
						retVal = true;
					}

				}
			}
			catch(Exception ex)
			{
				string exMsg = string.Format("Error occurred in {0} of {1}.{2}{3}", System.Reflection.MethodBase.GetCurrentMethod().Name, this.ToString(), Environment.NewLine, ex.Message);
				throw new Exception(exMsg);
			}

			return retVal;
		}

		internal void Closure()
		{
			try
			{
				debugWriteToCommandLog("Starting Closure ");

				Recordset pe = jobManagement.FindPendingEdits();

				if(null != pe && 0 < pe.RecordCount)
				{
					//Failure
					pe.Close();
					pe = null;

					message = "Could not complete due to unposted edits";
					debugWriteToCommandLog(message);

					status = "FAILURE";
					MessageProcessing();
				}
				else
				{
					// Ensure that ClosurePending is the current job status (other interfaces depend on this for Closure processing).
					string curStatus = JobManager.JobStatus;

					if(string.IsNullOrEmpty(curStatus) || !string.Equals(curStatus, "ClosurePending"))
					{
						JobManager.JobStatus = "ClosurePending";
						Commit();
						newJobStatus = "ClosurePending";
					}

					string queryEdits = "SELECT DISTINCT G3E_FID, G3E_FNO FROM ASSET_HISTORY WHERE G3E_IDENTIFIER = ?";
					Recordset jobedits = dataContext.OpenRecordset(queryEdits, CursorTypeEnum.adOpenDynamic, LockTypeEnum.adLockOptimistic, (int)CommandTypeEnum.adCmdText, dataContext.ActiveJob);

					if(null != jobedits && 0 < jobedits.RecordCount)
					{


						// If any features in the jobedits loop should have a company number and
						// the company number is missing, add it to this list
						// and use the list to send email to the job owner.
						List<FNOFID> MissingCompanyNumbers = new List<FNOFID>();

						// As we process and delete features, it's necessary to delete any owned features
						// before deleting the owning features.  The DeletionMode enumeration will set the
						// mode variable first to deleteOwned then to deleteOwner which will allow the check
						// for an owner before deleting.  Some features (such as Workpoints, etc.)
						// are deleted outside of this loop but since they do not participate in
						// an ownership relationship, they can be deleted regardless.
						// Some features (e.g. Switch Gears) may require additional processing because
						// they are both owned and owning features but, for now, the following
						// will work for the "standard" feature types (e.g. not hierarchical ownership involved).
						foreach(DeletionMode mode in Enum.GetValues(typeof(DeletionMode)))
						{
							// After deleting any owned features, reset the recordset pointer
							// and iterate the remaining records to delete the owners.
							jobedits.MoveFirst();

							do
							{
								short FNO = Convert.ToInt16(jobedits.Fields["G3E_FNO"].Value);
								int FID = Convert.ToInt32(jobedits.Fields["G3E_FID"].Value);

								// If mode is deleteOwned, then check ownership.  If the feature
								// is not owned, then skip it.  It will be deleted when the mode
								// is deleteOwning.  While it might be safe to delete an owning feature,
								// it would be necessary to check to see if that feature owns any
								// other features but the design does not state to do it that way.
								if(mode == DeletionMode.deleteOwned && !FeatureIsOwned(FNO, FID))
								{
									jobedits.MoveNext();

									if(!jobedits.EOF)
									{
										continue;
									}
									else
									{
										break;
									}
								}

								Recordset rsCommon = ComponentRecordset(FNO, FID, 1);

								if(null != rsCommon)
								{
									rsCommon.MoveFirst();

									// This should never be the case but for completeness, check anyway.
									if(System.DBNull.Value != rsCommon.Fields["FEATURE_STATE_C"].Value)
									{
										switch(rsCommon.Fields["FEATURE_STATE_C"].Value)
										{
											case "INI":
												switch(FNO)
												{
													case 59: // OH Transformer
													case 60: // UG Transformer
														break;
													default:
														rsCommon.Fields["FEATURE_STATE_C"].Value = "CLS";
														break;
												}
												break;
											case "OSR":
												bool deleteThisFeature = true;

												switch(FNO)
												{
													case 110: // Pole

														// Until the NJUNS interface is ready, delete the Pole

														// Check for outstanding NJUNS tickets.
														// If any exist, transition to LIP; otherwise, delete.
														//if(perform the check for NJUNS tickets - interface is currently incomplete)
														//{
														//	rsCommon.Fields["FEATURE_STATE_C"].Value = "LIP";
														//  deleteThisFeature = false;
														//}

														break;

													case 59: // OH Transformer
													case 60: // UG Transformer
														deleteThisFeature = false;
														break;

													case 54: // Service Line
														if(ServiceLineIsAssociated(FNO, FID))
														{
															deleteThisFeature = false;
														}
														break;
												}

												if(deleteThisFeature)
												{
													DeleteFeature(FNO, FID);
												}
												break;
										}
									}

									// Determine whether the company number is missing.
									// If so, add it to the list of features with missing company numbers.
									EvaluateCompanyNumbers(MissingCompanyNumbers, FNO, FID);
								}

								jobedits.MoveNext();
							} while(!jobedits.EOF);

						} // End of the DeletionMode enumeration

						CommitTransaction();
						StartTransaction();

						if(0 < MissingCompanyNumbers.Count)
						{
							// Send email for the list of FIDS and their relative information
							// to the Designer who owns the job.
							emailMissingCompanyNumberInfo(MissingCompanyNumbers);
						}

						// It's necessary to post the changes to the features first,
						// then delete the Work Points (and other job artifacts) and validate and post.
						// If everything is done in one transaction, validation fails because the Work Points are already deleted.

						// Clean up the CU records
						CommitTransaction();
						StartTransaction();

						CleanUpCUs();

						ValidateAndPost();

						// ValidateAndPost commits the currently-active transaction.
						StartTransaction();

						CleanUpJobArtifacts();

						ValidateAndPost();

						if(status != "FAILURE")
						{
							// Not possible to use the JobManager after the job is closed as that deactivates it.
							// Cache the job identifier to use to set the job status after the job is closed.
							string jobid = dataContext.ActiveJob;

							debugWriteToCommandLog("Calling CloseJob method.");
							jobManagement.CloseJob();

							UpdateJobColumn("g3e_jobstatus", "Closed");
						}

					}
				}

			}
			catch(Exception ex)
			{
				string exMsg = string.Format("Error occurred in {0} of {1}.{2}{3}", System.Reflection.MethodBase.GetCurrentMethod().Name, this.ToString(), Environment.NewLine, ex.Message);
				throw new Exception(exMsg);
			}
		}

		private void CleanUpJobArtifacts()
		{
			// Delete the work points
			DeleteFeaturesForActiveJob("workpoint_n", "wr_nbr");

			// Delete the plotting boundaries
			DeleteFeaturesForActiveJob("plotbnd_n", "job_id");

			// Delete the design area
			DeleteFeaturesForActiveJob("designarea_p", "job_id");
		}

		/// <summary>
		/// Deletes features for the active job from the specified table based on the job ID column.
		/// </summary>
		/// <param name="sql">Query to select feature data</param>
		/// <returns></returns>
		private bool DeleteFeaturesForActiveJob(string featureTable, string jobColumn)
		{
			bool retVal = true;

			try
			{
				string sql = string.Format("select g3e_fno,g3e_fid from {0} where {1}=?", featureTable, jobColumn);

				Recordset rs = dataContext.OpenRecordset(sql, CursorTypeEnum.adOpenDynamic, LockTypeEnum.adLockBatchOptimistic, (int)CommandTypeEnum.adCmdText, dataContext.ActiveJob);

				if(null != rs && 0 < rs.RecordCount)
				{
					rs.MoveFirst();

					do
					{
						DeleteFeature(Convert.ToInt16(rs.Fields["g3e_fno"].Value), Convert.ToInt32(rs.Fields["g3e_fid"].Value));
						rs.MoveNext();
					} while(!rs.EOF);

					rs.Close();
					rs = null;
				}
			}
			catch(Exception ex)
			{
				string exMsg = string.Format("Error occurred in {0} of {1}.{2}{3}", System.Reflection.MethodBase.GetCurrentMethod().Name, this.ToString(), Environment.NewLine, ex.Message);
				throw new Exception(exMsg);
			}

			return retVal;
		}

		/// <summary>
		/// Evaluates the feature and adds it to the features list if it is missing company numbers.
		/// </summary>
		/// <param name="features"></param>
		/// <param name="FNO"></param>
		/// <param name="FID"></param>
		private void EvaluateCompanyNumbers(List<FNOFID> features, short FNO, int FID)
		{
			try
			{
				// Determine whether the company number is missing.
				// If so, add it to the list of features.
				short companyNumberCNO = -1;

				switch(FNO)
				{
					case 34:
						companyNumberCNO = 3402;
						break;
					case 36:
						companyNumberCNO = 3602;
						break;
					case 59:
						companyNumberCNO = 5902;
						break;
					case 60:
						companyNumberCNO = 6002;
						break;
				}

				// If the CNO is set, then get the component recordset and evaluate the Company Number.
				if(-1 != companyNumberCNO)
				{
					Recordset companyNumberRS = ComponentRecordset(FNO, FID, companyNumberCNO);

					if(null != companyNumberRS)
					{
						companyNumberRS.MoveFirst();

						// Some of these components may be repeating so need to iterate all records
						do
						{
							// The Company Number field has the same field name for all four components.
							if(System.DBNull.Value == companyNumberRS.Fields["company_id"].Value || string.IsNullOrWhiteSpace(companyNumberRS.Fields["company_id"].Value.ToString()))
							{
								// Company number is either NULL or blank/empty.
								// If this feature hasn't been added to the list, then add it
								if(!features.Any<FNOFID>(p => p.FID == FID))
								{
									FNOFID missing = new FNOFID();
									missing.FNO = FNO;
									missing.FID = FID;
									features.Add(missing);
								}
							}

							companyNumberRS.MoveNext();
						} while(!companyNumberRS.EOF);
					}
				}

			}
			catch(Exception ex)
			{
				string exMsg = string.Format("Error occurred in {0} of {1}.{2}{3}", System.Reflection.MethodBase.GetCurrentMethod().Name, this.ToString(), Environment.NewLine, ex.Message);
				throw new Exception(exMsg);
			}
		}

		/// <summary>
		/// Iterate the ASSET_HISTORY records for the active job.
		/// For the CU component, set the ACTIVITY_C and WR_EDITED properties to NULL.
		/// For the ACU component, remove any non-Retirement Type 1 or 2 CU and set
		/// ACTIVITY_C and WR_EDITED to NULL for any Retirement Type 1 or 2 CU.
		/// </summary>
		private void CleanUpCUs()
		{

			try
			{
				string queryEdits = "SELECT DISTINCT G3E_FID, G3E_FNO FROM ASSET_HISTORY WHERE G3E_IDENTIFIER = ?";
				Recordset jobedits = dataContext.OpenRecordset(queryEdits, CursorTypeEnum.adOpenDynamic, LockTypeEnum.adLockOptimistic, (int)CommandTypeEnum.adCmdText, dataContext.ActiveJob);

				if(null != jobedits && 0 < jobedits.RecordCount)
				{
					jobedits.MoveFirst();

					do
					{
						short FNO = Convert.ToInt16(jobedits.Fields["G3E_FNO"].Value);
						int FID = Convert.ToInt32(jobedits.Fields["G3E_FID"].Value);

						// Remove ACUs where Retirement Code is null or 1 or 2
						Recordset rsCU = ComponentRecordset(FNO, FID, 22);

						if(null != rsCU)
						{
							rsCU.MoveFirst();

							do
							{
								bool deleteThisRecord = true;
								string retString = fieldValue(rsCU.Fields["retirement_c"], string.Empty);

								if(string.IsNullOrEmpty(retString))
								{
									// Treat a null retirement code as a 1 or 2
									deleteThisRecord = false;
								}
								else
								{
									short retCode = Convert.ToInt16(retString);

									if(1 == retCode || 2 == retCode)
									{
										deleteThisRecord = false;
									}
								}

								if(deleteThisRecord)
								{
									rsCU.Delete(AffectEnum.adAffectCurrent);
								}

								rsCU.MoveNext();
							} while(!rsCU.EOF);
						}

						// This must be done before the Activity Codes are set to NULL.
						ProcessAncillaryCURemoval(FNO, FID);

						// Update the Activity and Work Order on the CU component
						rsCU = ComponentRecordset(FNO, FID, 21);

						if(null != rsCU)
						{
							rsCU.MoveFirst();

							do
							{
								rsCU.Fields["activity_c"].Value = System.DBNull.Value;
								rsCU.Fields["wr_edited"].Value = System.DBNull.Value;
								rsCU.MoveNext();
							} while(!rsCU.EOF);
						}

						// Update the Activity and Work Order on the ACU component
						rsCU = ComponentRecordset(FNO, FID, 22);

						if(null != rsCU)
						{
							rsCU.MoveFirst();

							do
							{
								rsCU.Fields["ACTIVITY_C"].Value = System.DBNull.Value;
								rsCU.Fields["WR_EDITED"].Value = System.DBNull.Value;

								rsCU.MoveNext();
							} while(!rsCU.EOF);
						}

						jobedits.MoveNext();
					} while(!jobedits.EOF);
				}
			}
			catch(Exception ex)
			{
				string exMsg = string.Format("Error occurred in {0} of {1}.{2}{3}", System.Reflection.MethodBase.GetCurrentMethod().Name, this.ToString(), Environment.NewLine, ex.Message);
				throw new Exception(exMsg);
			}

		}


		/// <summary>
		/// Estimate the removal of Ancillary CUs for the indicated feature
		/// </summary>
		/// <param name="FNO">Current FNO</param>
		/// <param name="FID">Current FID</param>
		private void ProcessAncillaryCURemoval(short FNO, int FID)
		{
			IGTComponent ancillaryComponent = GetComponent(FNO, FID, 22);

			if(null == ancillaryComponent)
			{
				return;
			}

			int[] cidsToRemove = null;
			int[] cidsToAdjust = null;

			List<int> FinalcidsToRemove = new List<int>();
			List<int> FinalcidsToAdjust = new List<int>();

			Recordset rsAncillaryDummy = ancillaryComponent.Recordset.Clone();
			Recordset rsAncillary = ancillaryComponent.Recordset.Clone();

			rsAncillary.Filter = "RETIREMENT_C=1 OR RETIREMENT_C=2";
			if(rsAncillary.RecordCount > 0)
			{
				rsAncillary.Filter = "ACTIVITY_C='R' OR ACTIVITY_C='RC'";
				if(rsAncillary.RecordCount > 0)
				{

					rsAncillary.MoveFirst();

					while(!rsAncillary.EOF)
					{
						cidsToRemove = null;
						cidsToAdjust = null;

						QuantityCheckProcessing(Convert.ToString(rsAncillary.Fields["CU_C"].Value),
								Convert.ToInt32(rsAncillary.Fields["QTY_LENGTH_Q"].Value),
								rsAncillaryDummy.Clone(), Convert.ToInt32(rsAncillary.Fields["g3e_cid"].Value), out cidsToRemove, out cidsToAdjust);

						if(cidsToRemove != null)
						{
							foreach(int i in cidsToRemove)
							{
								if(i != 0 && (FinalcidsToRemove.Count == 0 || !FinalcidsToRemove.Contains(i)))
								{
									FinalcidsToRemove.Add(i);
								}
							}
						}
						if(cidsToAdjust != null)
						{
							foreach(int i in cidsToAdjust)
							{
								if(i != 0 && (FinalcidsToAdjust.Count == 0 || !FinalcidsToAdjust.Contains(i)))
								{
									FinalcidsToAdjust.Add(i);
								}
							}
						}
						if(FinalcidsToRemove.Count == 0 ||
								!FinalcidsToRemove.Contains(Convert.ToInt32(rsAncillary.Fields["G3E_CID"].Value)))
						{
							FinalcidsToRemove.Add(Convert.ToInt32(rsAncillary.Fields["G3E_CID"].Value));
						}
						rsAncillary.MoveNext();
					}


					Recordset rsAncillaryOriginal = ancillaryComponent.Recordset;
					rsAncillaryOriginal.MoveFirst();
					while(!rsAncillaryOriginal.EOF)
					{
						foreach(int i in FinalcidsToAdjust)
						{
							if(Convert.ToInt32(rsAncillaryOriginal.Fields["G3E_CID"].Value) == i)
							{
								rsAncillaryOriginal.Fields["QTY_LENGTH_Q"].Value = 0;
								break;
							}
						}

						rsAncillaryOriginal.MoveNext();
					}
					string strcids = "";
					string sql = "delete from COMP_UNIT_N where g3e_cno=22 and g3e_fid=" + FID + " and g3e_cid in (";

					foreach(int i in FinalcidsToRemove)
					{
						if(string.IsNullOrEmpty(strcids))
						{
							strcids = Convert.ToString(i);
						}
						else
						{
							strcids = strcids + "," + Convert.ToString(i);
						}
					}

					if(!string.IsNullOrEmpty(strcids))
					{
						sql = sql + strcids + ")";

						dataContext.Execute(sql, out int recs, (int)CommandTypeEnum.adCmdText);
						dataContext.Execute("commit", out int reecs, (int)CommandTypeEnum.adCmdText);
					}
				}
			}
		}

		private void QuantityCheckProcessing(string strCucode, int Quantity, Recordset rs, int currentCid, out int[] cidsToRemove, out int[] cidsToAdjust)
		{
			int sumQuantity = 0;
			Recordset rsMatched = rs;
			rsMatched.Filter = string.Format("CU_C='{0}' AND ACTIVITY_C=null", strCucode);
			rsMatched.Sort = "VINTAGE_YR";

			if(rsMatched.RecordCount > 0)
			{
				cidsToRemove = new int[rsMatched.RecordCount];
				cidsToAdjust = new int[rsMatched.RecordCount];

				if(rsMatched != null && rsMatched.RecordCount > 0)
				{
					sumQuantity = GetQuantity(rsMatched);
				}

				if(sumQuantity > 0 && Quantity >= sumQuantity)
				{
					rsMatched.MoveFirst();
					int i = 0;

					do
					{
						cidsToRemove[i] = Convert.ToInt32(rsMatched.Fields["G3E_CID"].Value);
						rsMatched.MoveNext();
					} while(!rsMatched.EOF);
				}
				else
				{
					if(sumQuantity > 0 && Quantity < sumQuantity)
					{
						int i = 0;
						int QtnToadjust = 0;
						rsMatched.MoveFirst();

						do
						{
							if(DBNull.Value != rsMatched.Fields["QTY_LENGTH_Q"].Value)
							{
								QtnToadjust = Convert.ToInt32(rsMatched.Fields["QTY_LENGTH_Q"].Value);
							}
							else
							{
								QtnToadjust = 0;
							}


							if(Quantity >= sumQuantity - QtnToadjust)
							{
								cidsToAdjust[i] = Convert.ToInt32(rsMatched.Fields["G3E_CID"].Value);
								break;
							}
							else
							{
								sumQuantity = sumQuantity - QtnToadjust;
								cidsToAdjust[i] = Convert.ToInt32(rsMatched.Fields["G3E_CID"].Value);
							}

							rsMatched.MoveNext();
						} while(!rsMatched.EOF);
					}
				}
			}
			else
			{
				cidsToRemove = null;
				cidsToAdjust = null;
			}
		}

		/// <summary>
		/// Returns the QTY_LENGTH_Q field value (or zero if null) from first record of rsMatched.
		/// </summary>
		/// <param name="rsMatched"></param>
		/// <returns></returns>
		private int GetQuantity(Recordset rsMatched)
		{
			int sumQuantity = 0;
			rsMatched.MoveFirst();

			do
			{
				if(DBNull.Value != rsMatched.Fields["QTY_LENGTH_Q"].Value)
				{
					sumQuantity = sumQuantity + Convert.ToInt32(rsMatched.Fields["QTY_LENGTH_Q"].Value);
				}

				rsMatched.MoveNext();
			} while(!rsMatched.EOF);

			return sumQuantity;
		}

		internal void Cancel()
		{
			try
			{
				Rollback();

				jobManagement.DiscardJob();

				// Code to delete the Work Points was here.  Removed that per ALM 1321

				string designAreaSelection = "SELECT G3E_FNO, G3E_FID FROM DESIGNAREA_P WHERE JOB_ID = ?";
				Recordset designAreas = dataContext.OpenRecordset(designAreaSelection, CursorTypeEnum.adOpenDynamic, LockTypeEnum.adLockBatchOptimistic, (int)CommandTypeEnum.adCmdText, dataContext.ActiveJob);

				if(null != designAreas && 0 < designAreas.RecordCount)
				{
					designAreas.MoveFirst();

					do
					{
						DeleteFeature(Convert.ToInt16(designAreas.Fields["G3E_FNO"].Value), Convert.ToInt32(designAreas.Fields["G3E_FID"].Value));
						designAreas.MoveNext();
					} while(!designAreas.EOF);

					designAreas.Close();
					designAreas = null;
				}

				string plottingBoundarySelection = "SELECT G3E_FNO, G3E_FID FROM PLOTBND_N WHERE JOB_ID = ?";
				Recordset plottingBoundaries = dataContext.OpenRecordset(plottingBoundarySelection, CursorTypeEnum.adOpenDynamic, LockTypeEnum.adLockBatchOptimistic, (int)CommandTypeEnum.adCmdText, dataContext.ActiveJob);

				if(null != plottingBoundaries && 0 < plottingBoundaries.RecordCount)
				{
					plottingBoundaries.MoveFirst();

					do
					{
						DeleteFeature(Convert.ToInt16(plottingBoundaries.Fields["G3E_FNO"].Value), Convert.ToInt32(plottingBoundaries.Fields["G3E_FID"].Value));
						plottingBoundaries.MoveNext();
					} while(!plottingBoundaries.EOF);

					plottingBoundaries.Close();
					plottingBoundaries = null;
				}

				string sql = "select count(1) from asset_history where g3e_identifier=?";
				Recordset ah = dataContext.OpenRecordset(sql, CursorTypeEnum.adOpenDynamic, LockTypeEnum.adLockOptimistic, (int)CommandTypeEnum.adCmdText, dataContext.ActiveJob);

				bool editsExist = false;

				if(null != ah && 0 < ah.RecordCount)
				{
					ah.MoveFirst();

					if(0 < Convert.ToInt32(ah.Fields[0].Value))
					{
						editsExist = true;
					}

					ah.Close();
					ah = null;
				}

				string currentStatus = JobManager.JobStatus;

				if(!editsExist && (currentStatus == "Design" || currentStatus == "ApprovalPending"))
				{
					jobManagement.CloseJob();
				}
				else
				{
					JobManager.JobStatus = "Cancelled";
					this.newJobStatus = "Cancelled";
				}

				Commit();
			}
			catch(Exception ex)
			{
				string exMsg = string.Format("Error occurred in {0} of {1}.{2}{3}", System.Reflection.MethodBase.GetCurrentMethod().Name, this.ToString(), Environment.NewLine, ex.Message);
				throw new Exception(exMsg);
			}
		}

		private bool DeleteFeature(short FNO, int FID)
		{
			bool retVal = false;

			try
			{
				IGTKeyObject feature = dataContext.OpenFeature(FNO, FID);

				if(null != feature)
				{

					// Determine the components to delete and the order in which to delete them
					Recordset comps = dataContext.MetadataRecordset("G3E_FEATURECOMPS_OPTABLE", "G3E_FNO = " + FNO);

					if(null != comps && 0 < comps.RecordCount)
					{
						comps.Sort = "g3e_deleteordinal asc";
						comps.MoveFirst();

						do
						{
							short CNO = Convert.ToInt16(comps.Fields["g3e_cno"].Value);
							Recordset rsDelete = ComponentRecordset(FNO, FID, CNO);

							if(null != rsDelete)
							{
								rsDelete.MoveFirst();

								do
								{
									rsDelete.Delete(AffectEnum.adAffectCurrent);

									if(!rsDelete.EOF)
									{
										rsDelete.MoveNext();
									}
								} while(!rsDelete.EOF && 0 < rsDelete.RecordCount);
							}

							comps.MoveNext();
						} while(!comps.EOF);
					}

					retVal = true;
				}

				return retVal;
			}
			catch(Exception e)
			{
				status = "FAILURE";
				message = string.Format("Error in BatchProcessingModule.DeleteFeature: {0}", e.Message);
				return false;
			}
		}

		internal void SendErrorEmail()
		{
			try
			{
				sendStatus();
				MessageProcessing();
			}
			catch(Exception ex)
			{
				string exMsg = string.Format("Error occurred in {0} of {1}.{2}{3}", System.Reflection.MethodBase.GetCurrentMethod().Name, this.ToString(), Environment.NewLine, ex.Message);
				throw new Exception(exMsg);
			}
		}

		#region MethodNoLongerUsed
		/// <summary>
		/// Determines whether all P2 Errors for the recordset parameter are overridden.
		/// This is no longer used but leaving it in for now in case
		/// a decision to perform this is deemed necessary.
		/// </summary>
		/// <param name="errors">Validation Errors recordset</param>
		/// <returns>true if all P2s are overridden, else false</returns>
		internal bool AllOverridesExist(Recordset errors)
		{
			bool retVal = true;

			try
			{

				// If there are no errors, then nothing else to do.
				if(null != errors && 0 < errors.RecordCount)
				{
					errors.Filter = "ErrorPriority='P2'";

					// If there are no P2 errors, then nothing else to do.
					if(0 < errors.RecordCount)
					{
						////////////////////////////////////////////////////////////////////////////////////////////////////////////////
						// A shared method to properly find the RULE_ID and RULE_NM should be used here; however,
						// until that is made available, use this method which looks for a WR_VALIDATION_OVERRIDE record
						// based on: G3E_IDENTIFIER, G3E_FNO, G3E_FID, and ERROR_MSG.
						////////////////////////////////////////////////////////////////////////////////////////////////////////////////

						errors.MoveFirst();

						do
						{
							string sql = "select count(*) from wr_validation_override where g3e_identifier=? and g3e_fno=? and g3e_fid=? and error_msg=?";
							Recordset vo = this.dataContext.OpenRecordset(sql, CursorTypeEnum.adOpenStatic, LockTypeEnum.adLockReadOnly, (int)CommandTypeEnum.adCmdText,
								this.dataContext.ActiveJob,
								Convert.ToInt16(errors.Fields["g3e_fno"].Value),
								Convert.ToInt32(errors.Fields["g3e_fid"].Value),
								errors.Fields["errordescription"].Value.ToString());

							// The recordset should never be null for this type of query but just in case...
							if(null != vo && 0 < vo.RecordCount)
							{
								vo.MoveFirst();

								// If any error records are not found in the overrides table, then exit and return false.
								short matchCount = Convert.ToInt16(vo.Fields[0].Value);

								vo.Close();
								vo = null;

								// No validation record found so exit the loop and return false.
								if(0 == matchCount)
								{
									retVal = false;
									break;
								}

							}
							errors.MoveNext();
						} while(!errors.EOF);

						if(null != errors)
						{
							if(errors.State == (int)ObjectStateEnum.adStateOpen)
							{
								errors.Close();
							}
							errors = null;
						}
					}

				}
			}
			catch(Exception ex)
			{
				string exMsg = string.Format("Error occurred in {0} of {1}.{2}{3}", System.Reflection.MethodBase.GetCurrentMethod().Name, this.ToString(), Environment.NewLine, ex.Message);
				throw new Exception(exMsg);
			}

			return retVal;
		}
		#endregion

		/// <summary>
		/// Validates all pending edits for the active job.
		/// </summary>
		/// <returns>true if no P1 errors; else, false</returns>
		private bool PendingEditsAreValid
		{
			get
			{
				bool retVal = true;

				try
				{
					Recordset val = jobManagement.ValidatePendingEdits();

					// If there are records, examine the recordset looking for P1s and/or P2s and evaluate those as appropriate.
					if(null != val && 0 < val.RecordCount)
					{
						val.Filter = "ErrorPriority='P1'";

						if(null != val && 0 < val.RecordCount)
						{
							// P1 Errors -> fail validation.
							message = "Encountered P1 validation error." + Environment.NewLine;
							val.MoveFirst();

							// Add the validation error details to the message string
							// so those will be included in the error email.

							do
							{
								// Test for DBNull.Value is failing to detect a null field value.  Resort to a method to return the field value or NULL.
								string msg = string.Format("Priority: {0} | Description: {1} | Location: {2} | Connection: {3} | FNO: {4} | FID: {5} | CNO: {6} | CID: {7}",
																						fieldValue(val.Fields["ErrorPriority"], "NULL"),
																						fieldValue(val.Fields["ErrorDescription"], "NULL"),
																						fieldValue(val.Fields["ErrorLocation"], "NULL"),
																						fieldValue(val.Fields["Connection"], "NULL"),
																						fieldValue(val.Fields["G3E_FNO"], "NULL"),
																						fieldValue(val.Fields["G3E_FID"], "NULL"),
																						fieldValue(val.Fields["G3E_CNO"], "NULL"),
																						fieldValue(val.Fields["G3E_CID"], "NULL"));
								debugWriteToCommandLog(msg);
								message = string.Format("{0}{1}{2}", message, Environment.NewLine, msg);
								val.MoveNext();
							} while(!val.EOF);

							switch(g_Request_Type)
							{
								case "APPRV":
									resultCode = "WMISAPP001";
									break;
								case "INSRV":
									resultCode = "WMISCMP001";
									break;
								case "CLOSE":
									resultCode = "WMISCLS001";
									break;
							}

							retVal = false;
						}

						val.Close();
						val = null;
					}
				}
				catch(Exception ex)
				{
					//string exMsg = string.Format("Error occurred in {0} of {1}.{2}{3}", System.Reflection.MethodBase.GetCurrentMethod().Name, this.ToString(), Environment.NewLine, ex.Message);


					// Get stack trace for the exception with source file information
					System.Diagnostics.StackTrace st = new System.Diagnostics.StackTrace(ex, true);
					// Get the top stack frame
					System.Diagnostics.StackFrame frame = st.GetFrame(st.FrameCount - 1);
					// Get the line number from the stack frame
					int line = frame.GetFileLineNumber();

					string exMsg = string.Format("Error occurred at line {4} in {0} of {1}.{2}{3}", System.Reflection.MethodBase.GetCurrentMethod().Name, this.ToString(), Environment.NewLine, ex.Message, line.ToString());
					throw new Exception(exMsg);
				}

				return retVal;
			}
		}


		/// <summary>
		/// Gets a list of FIDs in the active job that have conflicts.
		/// Returns an empty string if no conflicts exist.
		/// </summary>
		private bool ConflictsExist
		{
			get
			{
				bool retVal = false;
				string FIDs = string.Empty;

				try
				{
					Recordset con = jobManagement.FindConflicts();

					if(null != con && 0 < con.RecordCount)
					{
						retVal = true;
						con.MoveFirst();

						do
						{
							if(string.IsNullOrEmpty(FIDs))
							{
								FIDs = con.Fields["g3e_fid"].Value.ToString();
							}
							else
							{
								FIDs = string.Format("{0}, {1}", FIDs, con.Fields["g3e_fid"].Value.ToString());
							}

							con.MoveNext();
						} while(!con.EOF);

						con.Close();
						con = null;

						message = string.Format("Encountered conflicts for the following FID(s): {0}", FIDs);

						switch(g_Request_Type)
						{
							case "APPRV":
								resultCode = "WMISAPP003";
								break;
							case "INSRV":
								resultCode = "WMISCMP003";
								break;
							case "CLOSE":
								resultCode = "WMISCLS003";
								break;
							default:
								break;
						}

					}

				}
				catch(Exception ex)
				{
					string exMsg = string.Format("Error occurred in {0} of {1}.{2}{3}", System.Reflection.MethodBase.GetCurrentMethod().Name, this.ToString(), Environment.NewLine, ex.Message);
					throw new Exception(exMsg);
				}

				return retVal;
			}
		}

		internal void ValidateAndPost()
		{
			try
			{
				if(status != "FAILURE")
				{
					debugWriteToCommandLog("In ValidateAndPost method.");

					if(g_Request_Type == "APPRV")
					{
						//NJUNS TICKET STUFF FOR POLES, may be able to cause errors.
					}

					CommitTransaction();

					Recordset pe = jobManagement.FindPendingEdits();

					if(null != pe)
					{
						debugWriteToCommandLog(string.Format("Before posting - Pending Edit Count is: {0}", pe.RecordCount));
					}
					else
					{
						debugWriteToCommandLog("Before posting - Pending Edits Recordset is NULL");
					}

					// If there are no pending edits, then nothing else to do here.
					if(null == pe || 0 == pe.RecordCount)
					{
						return;
					}

					// Find/evaluate pending edits and exit if P1s
					if(!PendingEditsAreValid)
					{
						// MESSAGE and RESULTCODE are both set in PendingEditsAreValid
						status = "FAILURE";
						return;
					}

					// Find/evaluate conflicts and exit if any are found.
					if(ConflictsExist)
					{
						// MESSAGE and RESULTCODE are both set in ConflictsExist
						status = "FAILURE";
						return;
					}

					GTPostErrorConstants postResult = jobManagement.PostJob();

					pe = jobManagement.FindPendingEdits();

					if(null != pe)
					{
						debugWriteToCommandLog(string.Format("After posting - Pending Edit Count is: {0}", pe.RecordCount));
						pe.Close();
						pe = null;
					}
					else
					{
						debugWriteToCommandLog("After posting - Pending Edits Recordset is NULL");
					}

					// Examine the results of posting
					switch(postResult)
					{
						case GTPostErrorConstants.gtjmsNoError:
							debugWriteToCommandLog("Posting Status is: NoError");
							break;
						case GTPostErrorConstants.gtjmsValidation:
							debugWriteToCommandLog("Posting Status is: Validation");
							message = "ERROR:  Post returned Validation.";
							status = "FAILURE";
							break;
						case GTPostErrorConstants.gtjmsConflict:
							debugWriteToCommandLog("Posting Status is: Conflict");
							message = "ERROR:  Post returned Conflict.";
							status = "FAILURE";
							break;
						case GTPostErrorConstants.gtjmsDatabase:
							debugWriteToCommandLog("Posting Status is: Database");
							message = "ERROR:  Post returned Database.";
							status = "FAILURE";
							break;
						case GTPostErrorConstants.gtjmsPostNotConfigured:
							debugWriteToCommandLog("Posting Status is: PostNotConfigured");
							message = "ERROR:  Post returned PostNotConfigured.";
							status = "FAILURE";
							break;
						case GTPostErrorConstants.gtjmsNoPendingEdits:
							debugWriteToCommandLog("Posting Status is: NoPendingEdits");
							message = "ERROR:  Post returned NoPendingEdits.";
							status = "FAILURE";
							break;
						default:
							debugWriteToCommandLog("In the 'default' case for the postResult switch statement.");
							break;
					}
				}
			}
			catch(Exception ex)
			{
				string exMsg = string.Format("Error occurred in {0} of {1}.{2}{3}", System.Reflection.MethodBase.GetCurrentMethod().Name, this.ToString(), Environment.NewLine, ex.Message);
				throw new Exception(exMsg);
			}

			debugWriteToCommandLog("End of ValidateAndPost method.");
		}

		private void sendStatus()
		{
			try
			{
				string subject = string.Format("BATCH FAILED: {0} for WR {1}", batchtype, g_WorkRequest_Number);
				string body = string.Format("GIS batch processing failed:{0}Work Request # = {1}{0}Batch type = {2}{0}Submitted = {3}{0}Processed = {4}{0}Error = {5}",
																	Environment.NewLine, g_WorkRequest_Number, batchtype, g_Trans_Date, DateTime.Now.ToString("dd-MMM-yyyy hh:mm:ss TT"), message);
				string fromAddr = sysGeneralParameterValue("GISAUTO_WMIS", "ERRORREPORTING", "EmailFromAddress");
				string[] toAddrs = new string[] { string.Format("{0}{1}", JobManager.DesignerRACFID, sysGeneralParameterValue("EMAIL", "RACFID_EMAIL", "RACFID Email Suffix")) };
				SendEmail(toAddrs, fromAddr, subject, body, string.Empty);
			}
			catch(Exception ex)
			{
				string exMsg = string.Format("Error occurred in {0} of {1}.{2}{3}", System.Reflection.MethodBase.GetCurrentMethod().Name, this.ToString(), Environment.NewLine, ex.Message);
				throw new Exception(exMsg);
			}
		}

		/// <summary>
		/// Sends an email to the designer containing Feature Class Name, FID,
		/// and Feature ID and Structure ID of the owner of each FID in the FIDs list.
		/// </summary>
		/// <param name="FIDs">Integer List with features with missing Company Numbers.</param>
		private void emailMissingCompanyNumberInfo(List<FNOFID> features)
		{
			// This should not occur (and the foreach below should be sufficient?) but check anyway.
			if(0 == features.Count)
			{
				return;
			}

			string emailBody = string.Format("At WMIS closure for WR: {0}, Company Numbers were missing from the following features:{1}{1}", g_WorkRequest_Number, Environment.NewLine);

			foreach(FNOFID missing in features)
			{
				string featureName = featureNameByFNO(missing.FNO);
				int ownerFID = -1;
				string structureID = string.Empty;

				// The Owner ID is in the common component's recordset (CNO=1);
				Recordset commonRS = ComponentRecordset(missing.FNO, missing.FID, 1);
				if(null != commonRS)
				{
					commonRS.MoveFirst();

					if(System.DBNull.Value != commonRS.Fields["owner1_id"].Value)
					{
						int ownerID = Convert.ToInt32(commonRS.Fields["owner1_id"].Value);
						string sql = "select g3e_fid,structure_id from common_n where g3e_id=?";
						Recordset ownerInfo = dataContext.OpenRecordset(sql, CursorTypeEnum.adOpenStatic, LockTypeEnum.adLockReadOnly, (int)CommandTypeEnum.adCmdText, ownerID);

						if(null != ownerInfo)
						{
							ownerInfo.MoveFirst();
							ownerFID = Convert.ToInt32(ownerInfo.Fields["g3e_fid"].Value);

							if(System.DBNull.Value != ownerInfo.Fields["structure_id"].Value)
							{
								structureID = ownerInfo.Fields["structure_id"].Value.ToString();
							}
						}
						ownerInfo.Close();
						ownerInfo = null;
					}
				}

				// With the pertinent variables set, inside the loop of features, append those to the body of the message
				string tmp = string.Format("Feature Name: {0}  Feature ID: {1}  Feature ID of Owner: {2}  Structure ID of the Owner: {3}",
																		featureName, missing.FID.ToString(), -1 != ownerFID ? ownerFID.ToString() : "Owner not defined", structureID.ToString());
				emailBody = string.Format("{0}{1}{2}", emailBody, Environment.NewLine, tmp);
			}

			// Send the email
			string fromAddr = sysGeneralParameterValue("GISAUTO_WMIS", "ERRORREPORTING", "EmailFromAddress");
			string[] toAddrs = new string[] { string.Format("{0}{1}", JobManager.DesignerRACFID, sysGeneralParameterValue("EMAIL", "RACFID_EMAIL", "RACFID Email Suffix")) };

			SendEmail(toAddrs, fromAddr, "WMIS Closure: Missing Company Numbers", emailBody, string.Empty);
		}

		/// <summary>
		/// Sends an email using the given parameters
		/// </summary>
		/// <param name="toAddresses"></param>
		/// <param name="fromAddress"></param>
		/// <param name="subject"></param>
		/// <param name="body"></param>
		/// <param name="attachments"></param>
		private void SendEmail(string[] toAddresses, string fromAddress, string subject, string body, string attachments)
		{
			try
			{
				SendEmail.SendEmail sendMail = new SendEmail.SendEmail();
				sendMail.GTInteractive = false;
				sendMail.EmailRequest.ToAddrs = toAddresses;
				sendMail.EmailRequest.FromAddr = fromAddress;
				sendMail.EmailRequest.Subject = subject;
				sendMail.EmailRequest.Message = body;
				sendMail.EmailRequest.Attachments = attachments;
				sendMail.sendEmail();
			}
			catch(Exception ex)
			{
				string exMsg = string.Format("Error occurred in {0} of {1}.{2}{3}", System.Reflection.MethodBase.GetCurrentMethod().Name, this.ToString(), Environment.NewLine, ex.Message);
				throw new Exception(exMsg);
			}

		}


		/// <summary>
		/// Returns the G3E_USERNAME for the FNO
		/// </summary>
		/// <param name="FNO">FNO value</param>
		/// <returns></returns>
		string featureNameByFNO(short FNO)
		{
			try
			{
				string featureName = string.Empty;
				string sql = "select g3e_username from g3e_features_optlang where g3e_fno=?";
				Recordset rs = dataContext.OpenRecordset(sql, CursorTypeEnum.adOpenStatic, LockTypeEnum.adLockReadOnly, (int)CommandTypeEnum.adCmdText, FNO);
				if(null != rs && 0 < rs.RecordCount)
				{
					rs.MoveFirst();
					if(System.DBNull.Value != rs.Fields["g3e_username"].Value)
					{
						featureName = rs.Fields["g3e_username"].Value.ToString();
					}
					rs.Close();
					rs = null;
				}
				return featureName;
			}
			catch(Exception ex)
			{
				string exMsg = string.Format("Error occurred in {0} of {1}.{2}{3}", System.Reflection.MethodBase.GetCurrentMethod().Name, this.ToString(), Environment.NewLine, ex.Message);
				throw new Exception(exMsg);
			}
		}
		private XmlDocument MessageProcessing()
		{
			// None of the calls to this method use the returned value so if the Transactin ID is not valid,
			// then sending a message, etc., is not needed and returning a NULL is fine (for now).
			if("-1" == g_TransactionID)
			{
				return null;
			}

			try
			{
				XmlDocument errorMessage = new XmlDocument();
				XmlDeclaration xmlDeclaration = errorMessage.CreateXmlDeclaration("1.0", "UTF-8", null);
				errorMessage.AppendChild(xmlDeclaration);

				XmlNode disWorkRequestResponse = errorMessage.CreateElement("disWorkRequestResponse");
				XmlAttribute xmlns = errorMessage.CreateAttribute("xmlns");
				xmlns.Value = "http://www.oncor.com/DIS_Work_Request";
				disWorkRequestResponse.Attributes.Append(xmlns);
				errorMessage.AppendChild(disWorkRequestResponse);

				XmlNode resultStatus = errorMessage.CreateElement("ResultStatus");
				XmlAttribute xmlns2 = errorMessage.CreateAttribute("xmlns");
				xmlns2.Value = "http://www.oncor.com/DIS";
				resultStatus.Attributes.Append(xmlns2);
				disWorkRequestResponse.AppendChild(resultStatus);

				XmlNode Status = errorMessage.CreateElement("Status");
				Status.AppendChild(errorMessage.CreateTextNode(status));
				resultStatus.AppendChild(Status);

				XmlNode workRequest = errorMessage.CreateElement("WorkRequest");
				XmlAttribute xmlns3 = errorMessage.CreateAttribute("xmlns");
				xmlns3.Value = "http://www.oncor.com/Work_Request";
				workRequest.Attributes.Append(xmlns3);
				disWorkRequestResponse.AppendChild(workRequest);

				XmlNode WRNumber = errorMessage.CreateElement("WRNumber");
				WRNumber.AppendChild(errorMessage.CreateTextNode(g_WorkRequest_Number));
				workRequest.AppendChild(WRNumber);

				//----------------------------RESULT STATUS DOCUMENT-------------------------------------------------------------------------------

				XmlDocument responseDocument = new XmlDocument();
				XmlDeclaration responseDeclaration = responseDocument.CreateXmlDeclaration("1.0", "UTF-8", null);

				XmlNode documentResultStatus = responseDocument.CreateElement("ResultStatus");
				XmlAttribute docXmlns = responseDocument.CreateAttribute("xmlns");
				docXmlns.Value = "http://www.oncor.com/DIS";
				documentResultStatus.Attributes.Append(docXmlns);
				responseDocument.AppendChild(documentResultStatus);

				XmlNode responseStatus = responseDocument.CreateElement("Status");
				responseStatus.AppendChild(responseDocument.CreateTextNode(status));
				documentResultStatus.AppendChild(responseStatus);

				XmlNode resultCodeNode = responseDocument.CreateElement("ResultCode");
				resultCodeNode.AppendChild(responseDocument.CreateTextNode(resultCode));
				documentResultStatus.AppendChild(resultCodeNode);

				//TODO: add the messages to this somehow, it kinda irks me that we expect multiple messages
				if(message != "")
				{
					XmlNode errorMessageNode = responseDocument.CreateElement("ErrorMsg");
					errorMessageNode.AppendChild(responseDocument.CreateTextNode(message));
					documentResultStatus.AppendChild(errorMessageNode);
				}

				XmlNode Timestamp = responseDocument.CreateElement("Timestamp");
				Timestamp.AppendChild(responseDocument.CreateTextNode(DateTime.Now.ToString()));
				documentResultStatus.AppendChild(resultCodeNode);

				XMLMessenger.RequestXMLBody = responseDocument.ToString();

				XMLMessenger.URL = sysGeneralParameterValue("EdgeFrontier", "WMIS_SendBatchResults", "EF_URL");

				string datetime = "2000-01-01T12:00:00"; // Default to something in case the following fails to retrieve a timestamp.
				string sql = "select to_char(sysdate,'YYYY-MM-DD\"T\"HH24:MI:SS') from dual";
				IGTApplication app = GTClassFactory.Create<IGTApplication>();
				IGTDataContext dc = app.DataContext;
				Recordset rs = dc.OpenRecordset(sql, CursorTypeEnum.adOpenStatic, LockTypeEnum.adLockReadOnly, (int)CommandTypeEnum.adCmdText);

				if(null != rs && 0 < rs.RecordCount)
				{
					rs.MoveFirst();

					if(System.DBNull.Value != rs.Fields[0].Value)
					{
						datetime = rs.Fields[0].Value.ToString();
					}

					rs.Close();
					rs = null;
				}

				string rspxml = @"<?xml version = ""1.0"" encoding = ""UTF-8""?>";
				rspxml += @"<disProcessingStatusNotify xmlns = ""http://www.oncor.com/DIS_ProcessingStatusNotify"">";
				rspxml += @"  <RequestHeader xmlns = ""http://www.oncor.com/DIS"">";
				rspxml += @"    <SourceSystem>GIS</SourceSystem>";
				rspxml += string.Format(@"    <TransactionId>{0}</TransactionId>", g_TransactionID);
				rspxml += string.Format(@"    <TransactionType>{0}</TransactionType>", g_Request_Type);
				rspxml += @"    <Requestor/>";
				rspxml += string.Format(@"    <Timestamp>{0}</Timestamp>", datetime);
				rspxml += @"  </RequestHeader>";
				rspxml += @"  <WorkRequest xmlns = ""http://www.oncor.com/Work_Request"">";
				rspxml += string.Format(@"    <WRNumber>{0}</WRNumber>", g_WorkRequest_Number);
				rspxml += string.Format(@"    <WRStatus>{0}</WRStatus>", g_Request_Type);
				rspxml += @"  </WorkRequest>";
				rspxml += @"  <ResultStatus xmlns = ""http://www.oncor.com/DIS"">";
				rspxml += string.Format(@"    <Status>{0}</Status>", status);
				rspxml += string.Format(@"    <ResultCode>{0}</ResultCode>", resultCode);
				rspxml += string.Format(@"    <ErrorMsg>{0}</ErrorMsg>", message);
				rspxml += string.Format(@"    <Timestamp>{0}</Timestamp>", datetime);
				rspxml += @"  </ResultStatus>";
				rspxml += @"</disProcessingStatusNotify>";


				XMLMessenger.RequestXMLBody = rspxml;

				debugWriteToCommandLog(XMLMessenger.URL);
				debugWriteToCommandLog(XMLMessenger.RequestXMLBody);

				XMLMessenger.Method = HttpVerbs.Post;

				XMLMessenger.SendMsgToEF();

				return errorMessage;

			}
			catch(Exception ex)
			{
				string exMsg = string.Format("Error occurred in {0} of {1}.{2}{3}", System.Reflection.MethodBase.GetCurrentMethod().Name, this.ToString(), Environment.NewLine, ex.Message);
				throw new Exception(exMsg);
			}
		}


		/// <summary>
		/// Returns a component recordset (or NULL) based on the arguments.
		/// </summary>
		/// <param name="FNO"></param>
		/// <param name="FID"></param>
		/// <param name="CNO"></param>
		/// <returns>Recordset if exists and contains a record; else, NULL</returns>
		private Recordset ComponentRecordset(short FNO, int FID, short CNO)
		{
			Recordset retVal = null;

			try
			{
				IGTKeyObject feature = dataContext.OpenFeature(FNO, FID);

				if(null != feature && null != feature.Components)
				{
					IGTComponent component = feature.Components.GetComponent(CNO);

					if(null != component && null != component.Recordset && 0 < component.Recordset.RecordCount)
					{
						retVal = component.Recordset;
					}
				}
			}
			catch(Exception ex)
			{
				string exMsg = string.Format("Error occurred in {0} of {1}.{2}{3}", System.Reflection.MethodBase.GetCurrentMethod().Name, this.ToString(), Environment.NewLine, ex.Message);
				throw new Exception(exMsg);
			}

			return retVal;
		}

		/// <summary>
		/// Returns a component based on the FNO, FID, and CNO.
		/// </summary>
		/// <param name="FNO"></param>
		/// <param name="FID"></param>
		/// <param name="CNO"></param>
		/// <returns></returns>
		private IGTComponent GetComponent(short FNO, int FID, short CNO)
		{
			IGTComponent component = null;
			try
			{
				IGTKeyObject feature = dataContext.OpenFeature(FNO, FID);

				if(null != feature && null != feature.Components)
				{
					component = feature.Components.GetComponent(CNO);

					if(null != component && null != component.Recordset && 0 < component.Recordset.RecordCount)
					{
						return component;
					}
				}
			}
			catch(Exception ex)
			{
				string exMsg = string.Format("Error occurred in {0} of {1}.{2}{3}", System.Reflection.MethodBase.GetCurrentMethod().Name, this.ToString(), Environment.NewLine, ex.Message);
				throw new Exception(exMsg);
			}
			return component;
		}

		private void UpdateJobColumn(string column, string value)
		{
			try
			{
				// Job Status is transitioning to Open when Completion Batch fails
				// but it is not clear where/why that's occurring.  Log changes to
				// G3E_JOBSTATUS until this issue is resolved.
				if(column.ToLower() == "g3e_jobstatus")
				{
					debugWriteToCommandLog(string.Format("Setting Job Status to: {0}", value));
				}

				string sql = string.Format("update g3e_job set {0}=? where g3e_identifier=?", column);
				dataContext.Execute(sql, out int recs, (int)CommandTypeEnum.adCmdText, value, g_WorkRequest_Number);
				Commit();
			}
			catch(Exception ex)
			{
				string exMsg = string.Format("Error occurred in {0} of {1}.{2}{3}", System.Reflection.MethodBase.GetCurrentMethod().Name, this.ToString(), Environment.NewLine, ex.Message);
				throw new Exception(exMsg);
			}
		}

		/// <summary>
		/// Checks WMIS_STATUS_C for WR.
		/// </summary>
		/// <param name="WR"></param>
		/// <returns>true if WMIS_STATUS_C != BATCH; else false</returns>
		private bool WMISStatusIsValid(string WR)
		{
			try
			{
				bool retVal = false;

				string sql = "select wmis_status_c from g3e_job where g3e_identifier=?";
				Recordset rs = dataContext.OpenRecordset(sql, CursorTypeEnum.adOpenStatic, LockTypeEnum.adLockReadOnly, (int)CommandTypeEnum.adCmdText, WR);

				if(null != rs && 1 == rs.RecordCount)
				{
					rs.MoveFirst();

					if(DBNull.Value == rs.Fields[0].Value)
					{
						// NULL value is okay.
						retVal = true;
					}
					else
					{
						retVal = "BATCH" != rs.Fields[0].Value.ToString() ? true : false;
					}

					rs.Close();
					rs = null;
				}
				else
				{
					throw new Exception(string.Format("Job record not found for WR: {0}", WR));
				}

				return retVal;
			}
			catch(Exception ex)
			{
				string exMsg = string.Format("Error occurred in {0} of {1}.{2}{3}", System.Reflection.MethodBase.GetCurrentMethod().Name, this.ToString(), Environment.NewLine, ex.Message);
				throw new Exception(exMsg);
			}
		}

		private string sysGeneralParameterValue(string subsystemName, string subsystemComponent, string paramName)
		{
			string retVal = string.Empty;

			try
			{
				IGTApplication app = GTClassFactory.Create<IGTApplication>();
				IGTDataContext dc = app.DataContext;

				string sql = "SELECT PARAM_VALUE FROM SYS_GENERALPARAMETER WHERE SUBSYSTEM_NAME = ? AND SUBSYSTEM_COMPONENT = ? AND PARAM_NAME = ?";
				Recordset rs = dc.OpenRecordset(sql, CursorTypeEnum.adOpenStatic, LockTypeEnum.adLockReadOnly, (int)CommandTypeEnum.adCmdText, subsystemName, subsystemComponent, paramName);

				if(null != rs && 0 < rs.RecordCount && System.DBNull.Value != rs.Fields["PARAM_VALUE"].Value)
				{
					rs.MoveFirst();
					retVal = rs.Fields["PARAM_VALUE"].Value.ToString();
					rs.Close();
					rs = null;
				}
				else
				{
					throw new Exception(string.Format("No record found for query: {0} with arguments {1}, {2}, and {3}", sql, subsystemName, subsystemComponent, paramName));
				}
			}
			catch(Exception ex)
			{
				string exMsg = string.Format("Error occurred in {0} of {1}.{2}{3}", System.Reflection.MethodBase.GetCurrentMethod().Name, this.ToString(), Environment.NewLine, ex.Message);
				throw new Exception(exMsg);
			}

			return retVal;
		}

		/// <summary>
		///  Returns a string representation of the field value.
		///  System.DBNull.Value is failing to detect when a field is null
		///  so resorted to this method to evaluate a field value.
		/// </summary>
		/// <param name="field">ADO Field</param>
		/// <param name="nullValue">Value to return if field is null</param>
		/// <returns></returns>
		private string fieldValue(ADODB.Field field, string nullValue)
		{
			string retVal = string.Empty;
			try
			{
				retVal = field.Value.ToString();
			}
			catch
			{
				retVal = nullValue;
			}
			return retVal;
		}

		private void DeleteRequest()
		{
			try
			{
				string sql = "delete from gisauto_queue where request_transact_id=?";
				dataContext.Execute(sql, out int recs, (int)CommandTypeEnum.adCmdText, g_TransactionID);
				Commit();
			}
			catch(Exception ex)
			{
				string exMsg = string.Format("Error occurred in {0} of {1}.{2}{3}", System.Reflection.MethodBase.GetCurrentMethod().Name, this.ToString(), Environment.NewLine, ex.Message);
				throw new Exception(exMsg);
			}
		}

		/// <summary>
		/// Starts a new transaction
		/// </summary>
		private void StartTransaction()
		{
			try
			{

				if(transactionManager.TransactionInProgress)
				{
					throw new Exception("Attempting to start a new transaction when one is already ini progress.");
				}

				transactionManager.Begin("Automation");

				if(!transactionManager.TransactionInProgress)
				{
					throw new Exception("Unable to start a new GTTransaction.");
				}

			}
			catch(Exception ex)
			{
				string exMsg = string.Format("Error occurred in {0} of {1}.{2}{3}", System.Reflection.MethodBase.GetCurrentMethod().Name, this.ToString(), Environment.NewLine, ex.Message);
				throw new Exception(exMsg);
			}
		}

		/// <summary>
		/// Commits an active transaction.
		/// </summary>
		private void CommitTransaction()
		{
			try
			{

				if(transactionManager.TransactionInProgress)
				{
					transactionManager.Commit();
				}

			}
			catch(Exception ex)
			{
				string exMsg = string.Format("Error occurred in {0} of {1}.{2}{3}", System.Reflection.MethodBase.GetCurrentMethod().Name, this.ToString(), Environment.NewLine, ex.Message);
				throw new Exception(exMsg);
			}
		}

		/// <summary>
		/// Rolls back a transaction (if it is in progress).
		/// </summary>
		private void Rollback()
		{
			try
			{
				if(transactionManager.TransactionInProgress)
				{
					transactionManager.Rollback();
				}
			}
			catch(Exception ex)
			{
				string exMsg = string.Format("Error occurred in {0} of {1}.{2}{3}", System.Reflection.MethodBase.GetCurrentMethod().Name, this.ToString(), Environment.NewLine, ex.Message);
				throw new Exception(exMsg);
			}
		}

		private void Commit()
		{
			try
			{
				gtApp.DataContext.Execute("commit", out int recs, (int)CommandTypeEnum.adCmdText);
			}
			catch(Exception ex)
			{
				string exMsg = string.Format("Error occurred in {0} of {1}.{2}{3}", System.Reflection.MethodBase.GetCurrentMethod().Name, this.ToString(), Environment.NewLine, ex.Message);
				throw new Exception(exMsg);
			}
		}

	}

}
