using System;
using System.Collections.Generic;
using System.Linq;
using Intergraph.GTechnology.API;
using Intergraph.GTechnology.Interfaces;
using ADODB;
using gtCommandLogger;

namespace GTechnology.Oncor.CustomAPI
{
	public class fiStreetLightAccountValidation : IGTFunctional
	{
		IGTApplication app = GTClassFactory.Create<IGTApplication>();
		JobManager jobManager = new JobManager();
		IGTJobManagementService jobService = GTClassFactory.Create<IGTJobManagementService>();
		private GTArguments m_Arguments = null;
		IGTDataContext m_DataContext = null;
		private IGTComponents m_Components;
		private string m_ComponentName;
		private string m_FieldName;
		internal static IGTTransactionManager gTransMgr;

		public GTArguments Arguments
		{
			get { return m_Arguments; }
			set { m_Arguments = value; }
		}

		public string ComponentName
		{
			get { return m_ComponentName; }
			set { m_ComponentName = value; }
		}

		public IGTComponents Components
		{
			get { return m_Components; }
			set { m_Components = value; }
		}

		public IGTDataContext DataContext
		{
			get { return m_DataContext; }
			set { m_DataContext = value; }
		}

		public string FieldName
		{
			get { return m_FieldName; }
			set { m_FieldName = value; }
		}

		public IGTFieldValue FieldValueBeforeChange
		{
			get;
			set;
		}

		public GTFunctionalTypeConstants Type
		{
			get;
			set;
		}

		public void Delete()
		{
			throw new NotImplementedException();
		}

		public IGTTransactionManager TransactionManager
		{
			set { gTransMgr = value; }
		}

		public void Execute()
		{
			try
			{
				jobService.DataContext = m_DataContext;
				string SQL;
				Recordset RS = new Recordset();
				if(m_FieldName.ToUpper() == "G3E_GEOMETRY") //check for boundary error
				{
					//check for errors releated to street light symbol being moved
					bool errorReturned = CheckForBoundaryErrorExecute();
					if(errorReturned) //if street light symbol move is not valid, meaning errorReturned==true, stop code, because we can only display one message on status bar at a time.
					{
						return;
					}
				}

				//check for account matching error
				bool stopCode = CheckForAccountMatchErrorExecute();
				if(stopCode) //if error conditions were met in method, stop code.
				{
					return;
				}

				//check for SLA errors
				SQL = "select granted_role from user_role_privs where granted_role = ?";
				RS = m_DataContext.OpenRecordset(SQL, CursorTypeEnum.adOpenStatic, LockTypeEnum.adLockReadOnly, (int)ADODB.CommandTypeEnum.adCmdText, "PRIV_MGMT_STLT");
				if(!(RS.RecordCount > 0)) //if user does NOT have SLA role
				{
					//check for a street light that has a restricted accounts, pending edits, and is part of an active job.
					stopCode = CheckForRestrictedAndPendingEditsExecute();
					if(stopCode) //if error conditions were met in method, stop code.
					{
						return;
					}

					//check for street lights that are non-located
					stopCode = CheckForNonLocatedStreetLightExecute();
					if(stopCode) //if error conditions were met in method, stop code.
					{
						return;
					}
				}
			}
			catch(Exception ex)
			{
				string exMsg = string.Format("Error occurred in {0} of {1}.{2}{3}", System.Reflection.MethodBase.GetCurrentMethod().Name, this.ToString(), Environment.NewLine, ex.Message);
				throw new Exception(exMsg);
			}
		}

		//this function is called upon validation. It was made a seperate function from Execute(), because the processing is slightly different.
		public void Validate(out string[] ErrorPriorityArray, out string[] ErrorMessageArray)
		{
			try
			{
				Recordset RS;
				jobService.DataContext = m_DataContext;
				object[] arguments = m_Arguments.GTechArgumentArray;
				string errorP1 = arguments[0].ToString();
				string errorP2 = arguments[1].ToString();
				string error;
				List<string> priorityList = new List<string>();
				List<string> messageList = new List<string>();

				GTValidationLogger gTValidationLogger = null;
				IGTComponent comp = Components[ComponentName];
				int FID = 0;

				string fieldValue = string.Empty;

				if(comp != null && comp.Recordset != null && comp.Recordset.RecordCount > 0)
				{
					FID = int.Parse(comp.Recordset.Fields["G3E_FID"].Value.ToString());
					if(FieldName != "G3E_GEOMETRY")
						fieldValue = Convert.ToString(comp.Recordset.Fields[FieldName].Value);
				}

				if(new gtLogHelper().CheckIfLoggingIsEnabled())
				{
					LogEntries logEntries = new LogEntries
					{
						ActiveComponentName = ComponentName,
						ActiveFID = FID,
						ActiveFieldName = FieldName,
						ActiveFieldValue = FieldName == "G3E_GEOMETRY" ? "N/A" : fieldValue,
						JobID = DataContext.ActiveJob,
						RelatedComponentName = "N/A",
						RelatedFID = 0,
						RelatedFieldName = "N/A",
						RelatedFieldValue = "N/A",
						ValidationInterfaceName = "Street Light Account Agreement",
						ValidationInterfaceType = "FI",
					};
					gTValidationLogger = new GTValidationLogger(logEntries);

					gTValidationLogger.LogEntry("TIMING", "START", "Street Light Account Agreement Entry", "N/A", "");
				}


				if(m_FieldName.ToUpper() == "G3E_GEOMETRY") //check for boundary error
				{
					//check for errors releated to street light symbol being moved
					error = CheckForBoundaryErrorValidate();
					if(!string.IsNullOrEmpty(error)) //if error conditions were met in method
					{
						priorityList.Add(errorP2);
						messageList.Add(error);
					}
				}

				//check for account matching error
				if(m_FieldName.ToUpper() == "ACCOUNT_ID")
				{
					error = CheckForAccountMatchErrorValidate();
					if(!string.IsNullOrEmpty(error)) //if error conditions were met in method
					{
						priorityList.Add(errorP2);
						messageList.Add(error);
					}
				}

				//check for SLA errors
				string SQL = "select granted_role from user_role_privs where granted_role = ?";
				RS = m_DataContext.OpenRecordset(SQL, CursorTypeEnum.adOpenStatic, LockTypeEnum.adLockReadOnly, (int)ADODB.CommandTypeEnum.adCmdText, "PRIV_MGMT_STLT");
				if(!(RS.RecordCount > 0)) //if user does NOT have SLA role
				{
					//check for a street light that has a restricted accounts, pending edits, and is part of an active job.
					error = CheckForRestrictedAndPendingEditsValidate();
					if(!string.IsNullOrEmpty(error)) //if error conditions were met in method, stop code.
					{
						priorityList.Add(errorP1);
						messageList.Add(error);
					}

					//check for street lights that are non-located
					error = CheckForNonLocatedStreetLightValidate();
					if(!string.IsNullOrEmpty(error)) //if error conditions were met in method, stop code.
					{
						priorityList.Add(errorP1);
						messageList.Add(error);
					}
				}

				//make the list into arrays which are passed out as an out parameter
				if(!priorityList.Equals(null) && priorityList.Any() && !messageList.Equals(null) && messageList.Any())
				{
					ErrorPriorityArray = priorityList.ToArray();
					ErrorMessageArray = messageList.ToArray();
				}
				else
				{
					ErrorPriorityArray = null;
					ErrorMessageArray = null;
				}

				if(gTValidationLogger != null)
					gTValidationLogger.LogEntry("TIMING", "END", "Street Light Account Agreement Exit", "N/A", "");

			}
			catch(Exception ex)
			{
				string exMsg = string.Format("Error occurred in {0} of {1}.{2}{3}", System.Reflection.MethodBase.GetCurrentMethod().Name, this.ToString(), Environment.NewLine, ex.Message);
				throw new Exception(exMsg);
			}		}


		public bool CheckForAccountMatchErrorExecute()
		{
			try
			{
				if(m_Components["STREETLIGHT_N"].Recordset.EOF)
				{
					m_Components["STREETLIGHT_N"].Recordset.MoveFirst();
				}
				//get street light fields from the street light account and the street light and compare.
				string lightAccountID = m_Components["STREETLIGHT_N"].Recordset.Fields["ACCOUNT_ID"].Value.ToString();
				string lightLampType = m_Components["STREETLIGHT_N"].Recordset.Fields["LAMP_TYPE_C"].Value.ToString();
				string lightWattage = m_Components["STREETLIGHT_N"].Recordset.Fields["WATT_Q"].Value.ToString();
				string lightRateSchedule = m_Components["STREETLIGHT_N"].Recordset.Fields["RATE_SCHEDULE_C"].Value.ToString();
				string lightLuminareStyle = m_Components["STREETLIGHT_N"].Recordset.Fields["LUMIN_STYL_C"].Value.ToString();
				string SQL = "select WATTAGE, LAMP_TYPE, RATE_SCHEDULE, LUMINARE_STYLE from STLT_ACCOUNT where ESI_LOCATION = ?";
				Recordset RS = m_DataContext.OpenRecordset(SQL, CursorTypeEnum.adOpenStatic, LockTypeEnum.adLockReadOnly, (int)ADODB.CommandTypeEnum.adCmdText, lightAccountID.ToUpper());
				string accountWattage = Convert.ToString(RS.Fields["WATTAGE"].Value);
				string accountLampType = Convert.ToString(RS.Fields["LAMP_TYPE"].Value);
				string accountRateSchedule = Convert.ToString(RS.Fields["RATE_SCHEDULE"].Value);
				string accountLuminareStyle = Convert.ToString(RS.Fields["LUMINARE_STYLE"].Value);

				if(lightLampType.ToUpper().Trim() != accountLampType.ToUpper().Trim() || lightWattage.ToUpper().Trim() != accountWattage.ToUpper().Trim() || lightRateSchedule.ToUpper().Trim() != accountRateSchedule.ToUpper().Trim() || lightLuminareStyle.ToUpper().Trim() != accountLuminareStyle.ToUpper().Trim())
				{
					app.SetStatusBarText(GTStatusPanelConstants.gtaspcMessage, "ACCOUNT # CONFLICT: Street Light attributes do not match the specified Street Light Account.");
					return true; //since the error message was displayed, we return a value of true to let the program know to stop running.
				}
				return false;
			}
			catch(Exception ex)
			{
				string exMsg = string.Format("Error occurred in {0} of {1}.{2}{3}", System.Reflection.MethodBase.GetCurrentMethod().Name, this.ToString(), Environment.NewLine, ex.Message);
				throw new Exception(exMsg);
			}
		}

		public bool CheckForBoundaryErrorExecute()
		{
			try
			{
				if(m_Components["STREETLIGHT_N"].Recordset.EOF)
				{
					m_Components["STREETLIGHT_N"].Recordset.MoveFirst();
				}
				//get the esi_location for the street light
				var lightAccountID = m_Components["STREETLIGHT_N"].Recordset.Fields["ACCOUNT_ID"].Value;
				string SQL = "select ESI_LOCATION from STLT_ACCOUNT where ESI_LOCATION = ?";
				Recordset RS = m_DataContext.OpenRecordset(SQL, CursorTypeEnum.adOpenStatic, LockTypeEnum.adLockReadOnly, (int)ADODB.CommandTypeEnum.adCmdText, lightAccountID);
				string esiLocation;
				if(RS.RecordCount > 0)
				{
					esiLocation = Convert.ToString(RS.Fields["ESI_LOCATION"].Value);
				}
				else
				{
					return false; //do not go any further if an ESI_LOCATION cannot be found. This means the user has not entered one yet, so the symbol was likely just placed.
				}

				//query 1
				SQL = "select a.boundary_id,bnd.bnd_fno,ai.g3e_field,ai.g3e_cno,ai.g3e_componenttable from stlt_account a join stlt_boundary bnd on a.boundary_class = bnd.bnd_class join g3e_attributeinfo_optable ai on bnd.bnd_id_ano = ai.g3e_ano where a.esi_location = ?";
				RS = m_DataContext.OpenRecordset(SQL, CursorTypeEnum.adOpenStatic, LockTypeEnum.adLockReadOnly, (int)ADODB.CommandTypeEnum.adCmdText, esiLocation);
				if(RS.RecordCount > 0)
				{
					string boundaryID = Convert.ToString(RS.Fields["BOUNDARY_ID"].Value);
					string g3eField1 = Convert.ToString(RS.Fields["G3E_FIELD"].Value);
					string componentTable1 = Convert.ToString(RS.Fields["G3E_COMPONENTTABLE"].Value);
					string g3eCno1 = Convert.ToString(RS.Fields["G3E_CNO"].Value);
					//object bndFnoObject = RS.Fields["BND_FNO"].Value; //cast from object to uint to short
					//decimal bndFNOdecimal = RS.Fields["BND_FNO"].Value;
					//short bndFNOshort = (short)bndFNOdecimal;
					//short[] bndFNOarray = new short[] { bndFNOshort };
					Int16[] bndFNOarray = new Int16[] { Convert.ToInt16(RS.Fields["BND_FNO"].Value) };

					IGTOrientedPointGeometry lightGeometry = (IGTOrientedPointGeometry)m_Components[m_ComponentName].Geometry;
					IGTSpatialService spatialService = GTClassFactory.Create<IGTSpatialService>();
					spatialService.DataContext = m_DataContext;
					spatialService.Operator = GTSpatialOperatorConstants.gtsoTouches;
					spatialService.FilterGeometry = lightGeometry;
					Recordset features = spatialService.GetResultsByFNO(bndFNOarray);

					if(features.RecordCount > 0)
					{
						//query 2
						SQL = "select bnd.bnd_type,ai.g3e_field,ai.g3e_cno,ai.g3e_componenttable from stlt_account a join stlt_boundary bnd on a.boundary_class = bnd.bnd_class join g3e_attributeinfo_optable ai on bnd.bnd_type_ano = ai.g3e_ano where a.esi_location = ?";
						RS = m_DataContext.OpenRecordset(SQL, CursorTypeEnum.adOpenStatic, LockTypeEnum.adLockReadOnly, (int)ADODB.CommandTypeEnum.adCmdText, esiLocation);

						string bndType = null;
						string g3eField2 = null;
						string componentTable2 = null;
						string g3eCno2 = null;

						if(RS.RecordCount > 0)
						{
							bndType = Convert.ToString(RS.Fields["BND_TYPE"].Value);
							g3eField2 = Convert.ToString(RS.Fields["G3E_FIELD"].Value);
							componentTable2 = Convert.ToString(RS.Fields["G3E_COMPONENTTABLE"].Value);
							g3eCno2 = Convert.ToString(RS.Fields["G3E_CNO"].Value);
						}

						bool errorDisplayed;
						do
						{
							string fid = Convert.ToString(features.Fields["G3E_FID"].Value); //get the fid value for the current feature in feature list being iterated through.
							if(RS.RecordCount > 0) //if "type" attribute is found
							{
								if(componentTable2.Equals(componentTable1)) //if component tables match
								{
									SQL = "select 1 from " + componentTable1 + " where g3e_cno = ? and g3e_fid = ? and " + g3eField1 + " =? and " + g3eField2 + " =?";
									RS = m_DataContext.OpenRecordset(SQL, CursorTypeEnum.adOpenStatic, LockTypeEnum.adLockReadOnly, (int)ADODB.CommandTypeEnum.adCmdText, g3eCno2, fid, boundaryID, bndType);
									errorDisplayed = DisplayBoundaryErrorExecute(RS);
									if(errorDisplayed)
									{
										return true;
									}
								}
								else //if component tables do not match
								{
									SQL = "select 1 from  " + componentTable1 + " p join " + componentTable2 + " la on p.g3e_fid=la.g3e_fid where g3e_cno = ? and g3e_fid = ? and p." + g3eField1 + " =? and la." + g3eField2 + " =?";
									RS = m_DataContext.OpenRecordset(SQL, CursorTypeEnum.adOpenStatic, LockTypeEnum.adLockReadOnly, (int)ADODB.CommandTypeEnum.adCmdText, g3eCno2, fid, boundaryID, bndType);
									errorDisplayed = DisplayBoundaryErrorExecute(RS);
									if(errorDisplayed)
									{
										return true;
									}
								}
							}

							else //if no "type" attribute is found, determination is done by comparing the value of boundary_id to g3e_field value from g3e_attributeinfo_optable in first query 1
							{
								SQL = "select 1 from " + componentTable1 + " where g3e_fid = ? and g3e_cno = ? and " + g3eField1 + " =?";
								RS = m_DataContext.OpenRecordset(SQL, CursorTypeEnum.adOpenStatic, LockTypeEnum.adLockReadOnly, (int)ADODB.CommandTypeEnum.adCmdText, fid, g3eCno1, boundaryID);
								errorDisplayed = DisplayBoundaryErrorExecute(RS);
								if(errorDisplayed)
								{
									return true;
								}
							}
						} while(!features.EOF);
					}
				}
				return false; //nothing to validate in this case, since the street light account does not have a boundary.

			}
			catch(Exception ex)
			{
				string exMsg = string.Format("Error occurred in {0} of {1}.{2}{3}", System.Reflection.MethodBase.GetCurrentMethod().Name, this.ToString(), Environment.NewLine, ex.Message);
				throw new Exception(exMsg);
			}		}

		public bool CheckForRestrictedAndPendingEditsExecute()
		{
			try
			{
				if(m_Components["STREETLIGHT_N"].Recordset.EOF)
				{
					m_Components["STREETLIGHT_N"].Recordset.MoveFirst();
				}
				//check for street light with pending edits and is a restricted account
				bool hasPendingEdits = false; //default to false
				string restricted = "";
				string streetLightAccountID = m_Components["STREETLIGHT_N"].Recordset.Fields["ACCOUNT_ID"].Value.ToString();
				string SQL = "select RESTRICTED from stlt_account where ESI_LOCATION = ?";
				Recordset RS = m_DataContext.OpenRecordset(SQL, CursorTypeEnum.adOpenStatic, LockTypeEnum.adLockReadOnly, (int)ADODB.CommandTypeEnum.adCmdText, streetLightAccountID);
				if(RS.RecordCount > 0)
				{
					restricted = RS.Fields["RESTRICTED"].Value.ToString();
				}
				string streetLightFNO = m_Components["STREETLIGHT_N"].Recordset.Fields["G3E_FNO"].Value.ToString().ToUpper().Trim();
				string streetLightFID = m_Components["STREETLIGHT_N"].Recordset.Fields["G3E_FID"].Value.ToString().ToUpper().Trim();
				Recordset pendingEdits = jobService.FindPendingEdits();
				if(pendingEdits.RecordCount > 0)
				{
					do
					{
						if(pendingEdits.Fields["G3E_FNO"].Value.ToString().ToUpper().Trim() == streetLightFNO && pendingEdits.Fields["G3E_FID"].Value.ToString().ToUpper().Trim() == streetLightFID)
						{
							hasPendingEdits = true;
							break;
						}
						pendingEdits.MoveNext();
					} while(!pendingEdits.EOF);
				}
				if(jobManager.ActiveJob != null && restricted.ToUpper().Trim() == "Y" && hasPendingEdits)
				{
					app.SetStatusBarText(GTStatusPanelConstants.gtaspcMessage, "ACCOUNT # CONFLICT: Street Lights with Restricted Accounts may only be edited by a Street Light Administrator.");
					return true; //return true to let program know to stop code
				}
				return false; //**********************************
			}
			catch(Exception ex)
			{
				string exMsg = string.Format("Error occurred in {0} of {1}.{2}{3}", System.Reflection.MethodBase.GetCurrentMethod().Name, this.ToString(), Environment.NewLine, ex.Message);
				throw new Exception(exMsg);
			}
		}

		public bool CheckForNonLocatedStreetLightExecute()
		{
			try
			{
				if(m_Components["STREETLIGHT_N"].Recordset.EOF)
				{
					m_Components["STREETLIGHT_N"].Recordset.MoveFirst();
				}
				string streetLightLocated = m_Components["STREETLIGHT_N"].Recordset.Fields["LOCATABLE_YN"].Value.ToString();
				if(streetLightLocated.ToUpper().Trim() == "N")
				{
					app.SetStatusBarText(GTStatusPanelConstants.gtaspcMessage, "ACCOUNT # CONFLICT: Non-Located Street Lights may only be edited by a Street Light Administrator.");
					return true; //return true to let program know to stop code
				}
				return false; //returning false means that the error conditions were not met and code should keep running
			}
			catch(Exception ex)
			{
				string exMsg = string.Format("Error occurred in {0} of {1}.{2}{3}", System.Reflection.MethodBase.GetCurrentMethod().Name, this.ToString(), Environment.NewLine, ex.Message);
				throw new Exception(exMsg);
			}
		}

		public bool DisplayBoundaryErrorExecute(Recordset RS)
		{
			try
			{
				if(m_Components[ComponentName].Recordset.EOF)
				{
					m_Components[ComponentName].Recordset.MoveFirst();
				}
				if(!(RS.RecordCount > 0) && jobManager.JobStatus.ToUpper() == "NON-WR") //if job status is non-wr, then value is reset and a slightly different error message is displayed.
				{
					app.SetStatusBarText(GTStatusPanelConstants.gtaspcMessage, "GRAPHIC EDIT REJECTED: Street Light is located outside the associated Street Light Account boundary.");
					//reset g3e_geometry field to value before change
					string fno = m_Components[m_ComponentName].Recordset.Fields["G3E_FNO"].Value.ToString();
					string fid = m_Components[m_ComponentName].Recordset.Fields["G3E_FID"].Value.ToString();
					gTransMgr.Begin("Resetting Geometry Value");
					string SQL = "update B$STREETLIGHT_T set G3E_GEOMETRY = ? where G3E_FNO = ? and G3E_FID = ?";
					m_DataContext.Execute(SQL, out int RecordsAffected, (int)ADODB.CommandTypeEnum.adCmdText, FieldValueBeforeChange, fno, fid);
					gTransMgr.Commit();
					return true;
				}
				else if(!(RS.RecordCount > 0))
				{
					app.SetStatusBarText(GTStatusPanelConstants.gtaspcMessage, "ACCOUNT # CONFLICT: Street Light is located outside the associated Street Light Account boundary.");
					return true;
				}
				else
				{
					return false;
				}
			}
			catch(Exception ex)
			{
				string exMsg = string.Format("Error occurred in {0} of {1}.{2}{3}", System.Reflection.MethodBase.GetCurrentMethod().Name, this.ToString(), Environment.NewLine, ex.Message);
				throw new Exception(exMsg);
			}
		}

		public string CheckForAccountMatchErrorValidate()
		{
			try
			{
				if(m_Components["STREETLIGHT_N"].Recordset.EOF)
				{
					m_Components["STREETLIGHT_N"].Recordset.MoveFirst();
				}
				string lightAccountID = m_Components["STREETLIGHT_N"].Recordset.Fields["ACCOUNT_ID"].Value.ToString();
				string lightLampType = m_Components["STREETLIGHT_N"].Recordset.Fields["LAMP_TYPE_C"].Value.ToString();
				string lightWattage = m_Components["STREETLIGHT_N"].Recordset.Fields["WATT_Q"].Value.ToString();
				string lightRateSchedule = m_Components["STREETLIGHT_N"].Recordset.Fields["RATE_SCHEDULE_C"].Value.ToString();
				string lightLuminareStyle = m_Components["STREETLIGHT_N"].Recordset.Fields["LUMIN_STYL_C"].Value.ToString();
				string SQL = "select WATTAGE, LAMP_TYPE, RATE_SCHEDULE, LUMINARE_STYLE from STLT_ACCOUNT where ESI_LOCATION = ?";
				Recordset RS = m_DataContext.OpenRecordset(SQL, CursorTypeEnum.adOpenStatic, LockTypeEnum.adLockReadOnly, (int)ADODB.CommandTypeEnum.adCmdText, lightAccountID.ToUpper());
				if(RS.RecordCount > 0)
				{
					string accountWattage = Convert.ToString(RS.Fields["WATTAGE"].Value);
					string accountLampType = Convert.ToString(RS.Fields["LAMP_TYPE"].Value);
					string accountRateSchedule = Convert.ToString(RS.Fields["RATE_SCHEDULE"].Value);
					string accountLuminareStyle = Convert.ToString(RS.Fields["LUMINARE_STYLE"].Value);

					if(lightLampType.ToUpper().Trim() != accountLampType.ToUpper().Trim() || lightWattage.ToUpper().Trim() != accountWattage.ToUpper().Trim() || lightRateSchedule.ToUpper().Trim() != accountRateSchedule.ToUpper().Trim() || lightLuminareStyle.ToUpper().Trim() != accountLuminareStyle.ToUpper().Trim())
					{
						return "ACCOUNT # CONFLICT: Street Light attributes do not match the specified Street Light Account.";
					}
				}
				return string.Empty;
			}
			catch(Exception ex)
			{
				string exMsg = string.Format("Error occurred in {0} of {1}.{2}{3}", System.Reflection.MethodBase.GetCurrentMethod().Name, this.ToString(), Environment.NewLine, ex.Message);
				throw new Exception(exMsg);
			}
		}

		public string CheckForBoundaryErrorValidate()
		{
			try
			{
				//get the esilocation for the street light
				if(m_Components["STREETLIGHT_N"].Recordset.EOF)
				{
					m_Components["STREETLIGHT_N"].Recordset.MoveFirst();
				}
				var lightAccountID = m_Components["STREETLIGHT_N"].Recordset.Fields["ACCOUNT_ID"].Value;
				string SQL = "select ESI_LOCATION from STLT_ACCOUNT where ESI_LOCATION = ?";
				Recordset RS = m_DataContext.OpenRecordset(SQL, CursorTypeEnum.adOpenStatic, LockTypeEnum.adLockReadOnly, (int)ADODB.CommandTypeEnum.adCmdText, lightAccountID);
				string esiLocation;

				if(null != RS && RS.RecordCount > 0)
				{
					esiLocation = Convert.ToString(RS.Fields["ESI_LOCATION"].Value);
				}
				else
				{
					return string.Empty; //do not go any further if an ESI_LOCATION cannot be found. This means the user has not entered one yet, so the symbol was likely just placed.
				}

				//query 1
				SQL = "select a.boundary_id,bnd.bnd_fno,ai.g3e_field,ai.g3e_cno,ai.g3e_componenttable from stlt_account a join stlt_boundary bnd on a.boundary_class = bnd.bnd_class join g3e_attributeinfo_optable ai on bnd.bnd_id_ano = ai.g3e_ano where a.esi_location = ?";
				RS = m_DataContext.OpenRecordset(SQL, CursorTypeEnum.adOpenStatic, LockTypeEnum.adLockReadOnly, (int)ADODB.CommandTypeEnum.adCmdText, esiLocation);
				if(null != RS && RS.RecordCount > 0)
				{
					string boundaryID = Convert.ToString(RS.Fields["BOUNDARY_ID"].Value);
					string g3eField1 = Convert.ToString(RS.Fields["G3E_FIELD"].Value);
					string componentTable1 = Convert.ToString(RS.Fields["G3E_COMPONENTTABLE"].Value);
					string g3eCno1 = Convert.ToString(RS.Fields["G3E_CNO"].Value);
					//decimal bndFNOdecimal = RS.Fields["BND_FNO"].Value;
					//short bndFNOshort = (short)bndFNOdecimal;
					//short[] bndFNOarray = new short[] { bndFNOshort };
					Int16[] bndFNOarray = new Int16[] { Convert.ToInt16(RS.Fields["BND_FNO"].Value) };

					IGTOrientedPointGeometry lightGeometry = (IGTOrientedPointGeometry)m_Components[m_ComponentName].Geometry;
					IGTSpatialService spatialService = GTClassFactory.Create<IGTSpatialService>();
					spatialService.DataContext = m_DataContext;
					spatialService.Operator = GTSpatialOperatorConstants.gtsoTouches;
					spatialService.FilterGeometry = lightGeometry;
					Recordset features = spatialService.GetResultsByFNO(bndFNOarray);

					if(null != features && features.RecordCount > 0)
					{
						//query 2
						SQL = "select bnd.bnd_type,ai.g3e_field,ai.g3e_cno,ai.g3e_componenttable from stlt_account a join stlt_boundary bnd on a.boundary_class = bnd.bnd_class join g3e_attributeinfo_optable ai on bnd.bnd_type_ano = ai.g3e_ano where a.esi_location = ?";
						RS = m_DataContext.OpenRecordset(SQL, CursorTypeEnum.adOpenStatic, LockTypeEnum.adLockReadOnly, (int)ADODB.CommandTypeEnum.adCmdText, esiLocation);

						string bndType = null;
						string g3eField2 = null;
						string componentTable2 = null;
						string g3eCno2 = null;

						if(RS.RecordCount > 0)
						{
							bndType = Convert.ToString(RS.Fields["BND_TYPE"].Value);
							g3eField2 = Convert.ToString(RS.Fields["G3E_FIELD"].Value);
							componentTable2 = Convert.ToString(RS.Fields["G3E_COMPONENTTABLE"].Value);
							g3eCno2 = Convert.ToString(RS.Fields["G3E_CNO"].Value);
						}
						do
						{
							string fid = Convert.ToString(features.Fields["G3E_FID"].Value); //get the fid value for the current feature in feature list being iterated through.
							if(RS.RecordCount > 0) //if "type" attribute is found in query 2
							{
								if(componentTable2.Equals(componentTable1)) //if component tables match
								{
									SQL = "select g3e_id from " + componentTable1 + " where g3e_cno = ? and g3e_fid = ? and " + g3eField1 + " =? and " + g3eField2 + " =?";
									RS = m_DataContext.OpenRecordset(SQL, CursorTypeEnum.adOpenStatic, LockTypeEnum.adLockReadOnly, (int)ADODB.CommandTypeEnum.adCmdText, g3eCno2, fid, boundaryID, bndType);
									if(!(RS.RecordCount > 0) && jobManager.JobStatus.ToUpper() == "NON-WR")
									{
										return "GRAPHIC EDIT REJECTED: Street Light is located outside the associated Street Light Account boundary.";
									}
									else if(!(RS.RecordCount > 0))
									{
										return "ACCOUNT # CONFLICT: Street Light is located outside the associated Street Light Account boundary.";
									}
									else
									{
										return string.Empty;
									}
								}
								else //if component tables do not match
								{
									SQL = "select g3e_id from " + componentTable1 + " p join " + componentTable2 + " la on p.g3e_fid=la.g3e_fid where g3e_cno = ? and g3e_fid = ? and p." + g3eField1 + " =? and la." + g3eField2 + " =?";
									RS = m_DataContext.OpenRecordset(SQL, CursorTypeEnum.adOpenStatic, LockTypeEnum.adLockReadOnly, (int)ADODB.CommandTypeEnum.adCmdText, g3eCno2, fid, boundaryID, bndType);
									if(!(RS.RecordCount > 0) && jobManager.JobStatus.ToUpper() == "NON-WR")
									{
										return "GRAPHIC EDIT REJECTED: Street Light is located outside the associated Street Light Account boundary.";
									}
									else if(!(RS.RecordCount > 0))
									{
										return "ACCOUNT # CONFLICT: Street Light is located outside the associated Street Light Account boundary.";
									}
									else
									{
										return string.Empty;
									}
								}
							}

							else //if no "type" attribute is found, determination is done by comparing the value of boundary_id to g3e_field value from g3e_attributeinfo_optable in first query 1
							{
								SQL = "select g3e_id from " + componentTable1 + " where g3e_fid = ? and g3e_cno = ? and " + g3eField1 + " =?";
								RS = m_DataContext.OpenRecordset(SQL, CursorTypeEnum.adOpenStatic, LockTypeEnum.adLockReadOnly, (int)ADODB.CommandTypeEnum.adCmdText, fid, g3eCno1, boundaryID);
								if(!(RS.RecordCount > 0) && jobManager.JobStatus.ToUpper() == "NON-WR")
								{
									return "GRAPHIC EDIT REJECTED: Street Light is located outside the associated Street Light Account boundary.";
								}
								else if(!(RS.RecordCount > 0))
								{
									return "ACCOUNT # CONFLICT: Street Light is located outside the associated Street Light Account boundary.";
								}
								else
								{
									return string.Empty;
								}
							}
						} while(!features.EOF);
					}
					else
					{
						return string.Empty; //nothing to validate in this case, since spatial query did not return any features
					}
				}
				else
				{
					return string.Empty; //nothing to validate in this case, since the street light account does not have a boundary.
				}

			}
			catch(Exception ex)
			{
				string exMsg = string.Format("Error occurred in {0} of {1}.{2}{3}", System.Reflection.MethodBase.GetCurrentMethod().Name, this.ToString(), Environment.NewLine, ex.Message);
				throw new Exception(exMsg);
			}
		}

		public string CheckForRestrictedAndPendingEditsValidate()
		{
			try
			{
				if(m_Components["STREETLIGHT_N"].Recordset.EOF)
				{
					m_Components["STREETLIGHT_N"].Recordset.MoveFirst();
				}

				//check for street light with pending edits and is a restricted account
				bool hasPendingEdits = false; //default to false
				string restricted = "";
				string streetLightAccountID = m_Components["STREETLIGHT_N"].Recordset.Fields["ACCOUNT_ID"].Value.ToString();
				string SQL = "select RESTRICTED from stlt_account where ESI_LOCATION = ?";
				Recordset RS = m_DataContext.OpenRecordset(SQL, CursorTypeEnum.adOpenStatic, LockTypeEnum.adLockReadOnly, (int)ADODB.CommandTypeEnum.adCmdText, streetLightAccountID);

				if(null != RS && RS.RecordCount > 0)
				{
					restricted = RS.Fields["RESTRICTED"].Value.ToString();
				}
				string streetLightFNO = m_Components["STREETLIGHT_N"].Recordset.Fields["G3E_FNO"].Value.ToString().ToUpper().Trim();
				string streetLightFID = m_Components["STREETLIGHT_N"].Recordset.Fields["G3E_FID"].Value.ToString().ToUpper().Trim();
				Recordset pendingEdits = jobService.FindPendingEdits();

				if(null != pendingEdits && pendingEdits.RecordCount > 0)
				{
					pendingEdits.MoveFirst();

					do
					{
						if(pendingEdits.Fields["G3E_FNO"].Value.ToString().ToUpper().Trim() == streetLightFNO && pendingEdits.Fields["G3E_FID"].Value.ToString().ToUpper().Trim() == streetLightFID)
						{
							hasPendingEdits = true;
							break;
						}
						pendingEdits.MoveNext();
					} while(!pendingEdits.EOF);
				}

				if(jobManager.ActiveJob != null && restricted.ToUpper().Trim() == "Y" && hasPendingEdits)
				{
					return "ACCOUNT # CONFLICT: Street Lights with Restricted Accounts may only be edited by a Street Light Administrator.";
				}
				return string.Empty;
			}
			catch(Exception ex)
			{
				string exMsg = string.Format("Error occurred in {0} of {1}.{2}{3}", System.Reflection.MethodBase.GetCurrentMethod().Name, this.ToString(), Environment.NewLine, ex.Message);
				throw new Exception(exMsg);
			}
		}

		public string CheckForNonLocatedStreetLightValidate()
		{
			try
			{
				if(m_Components["STREETLIGHT_N"].Recordset.EOF)
				{
					m_Components["STREETLIGHT_N"].Recordset.MoveFirst();
				}
				string streetLightLocated = m_Components["STREETLIGHT_N"].Recordset.Fields["LOCATABLE_YN"].Value.ToString();
				if(streetLightLocated.ToUpper().Trim() == "N")
				{
					return "ACCOUNT # CONFLICT: Non-Located Street Lights may only be edited by a Street Light Administrator.";
				}
				return string.Empty;
			}
			catch(Exception ex)
			{
				string exMsg = string.Format("Error occurred in {0} of {1}.{2}{3}", System.Reflection.MethodBase.GetCurrentMethod().Name, this.ToString(), Environment.NewLine, ex.Message);
				throw new Exception(exMsg);
			}
		}
	}
}