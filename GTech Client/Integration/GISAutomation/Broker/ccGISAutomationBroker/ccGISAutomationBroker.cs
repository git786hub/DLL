using ADODB;
using Intergraph.GTechnology.API;
using Intergraph.GTechnology.Interfaces;
using System;
using System.Threading;
using System.Windows.Forms;

namespace GTechnology.Oncor.CustomAPI
{
	public class ccGISAutomationBroker : IGTCustomCommandModeless
	{
		private IGTTransactionManager m_TransactionManager;                                                     // Passed to plug-ins. Used to manage database transactions
		private IGTApplication m_Application = GTClassFactory.Create<IGTApplication>();                         // G/Tech instance
		private IGTJobManagementService m_JobManagement = GTClassFactory.Create<IGTJobManagementService>();     // Used to activate job
		private string m_QueueTable = "GISAUTO_QUEUE";                                                             // Queue table to query for new requests to pass to plug-ins
		private int m_PollingInterval = 60;                                                                      // Polling interval to query the queue table
		private string m_InstanceID = string.Empty;                                                             // Broker id to use to filter the records in the queue table
		private string m_StartingJob = string.Empty;                                                            // The active job when this command was started

		public bool CanTerminate
		{
			get
			{
				return true;
			}
		}

		public IGTTransactionManager TransactionManager
		{
			set
			{
				m_TransactionManager = value;
			}
		}

		/// <summary>
		/// The entry point for the custom command.
		/// </summary>
		/// <param name="CustomCommandHelper">Provides notification to the system that the command has finished</param>
		public void Activate(IGTCustomCommandHelper CustomCommandHelper)
		{
			// Get the active job. This will be used later to reset the job if it is changed by the plug-in module.
			m_StartingJob = m_Application.DataContext.ActiveJob;

			// Parse the comma-delimited argument string passed in by the monitor.
			if(!GetCommandArguments())
			{
				CustomCommandHelper.Complete();
			}

			// Reset queue table REQUEST_STATUS = 'NEW' if any records for the instance ID are set to = 'PROCESSING'.
			if(!ResetRequestStatus())
			{
				CustomCommandHelper.Complete();
			}

			// Set an application property named “UnattendedMode” as an indicator to other custom code that processing should not display messages and wait for user input.  
			// Note that the value of this property is not important; the presence of the named property indicates unattended mode, and its absence indicates interactive mode.
			try
			{
				m_Application.Properties.Add("UnattendedMode", true);
			}
			catch
			{
				// ignore error if property is already set
			}

			// Process any pending requests
			if(!Process())
			{
				CustomCommandHelper.Complete();
			}
		}

		public void Pause()
		{
			return;
		}

		public void Resume()
		{
			return;
		}

		public void Terminate()
		{
			m_JobManagement = null;
			m_Application = null;
			m_TransactionManager = null;
		}

		/// <summary>
		/// Gets the parameters for the custom command.
		/// </summary>
		/// <returns>Boolean indicating status</returns>
		private bool GetCommandArguments()
		{
			bool returnValue = false;
			string commandArgs = string.Empty;

			try
			{
				// Parse the comma-delimited argument string passed to the command.
				// Expected arguments:
				//   1.	QueueTable – Name of the automation queue table to monitor, optionally qualified with the schema name        
				//   2.	PollingInterval – Time in seconds to wait between querying the automation queue for new requests
				//   3.	InstanceID – Value that uniquely identifies the instance of the GIS Automation Broker
				commandArgs = m_Application.Properties["AutoCustomCommandArgument"].ToString();
				string[] commandArgsArray = commandArgs.Split(',');

				foreach(string arg in commandArgsArray)
				{
					string[] parameterValues = arg.Split('=');

					switch(parameterValues[0].Trim())
					{
						case "QueueTable":
							m_QueueTable = parameterValues[1].Trim();
							break;
						case "PollingInterval":
							if(!int.TryParse(parameterValues[1].Trim(), out m_PollingInterval))
							{
								throw new Exception("Non-integer value for PollingInterval");
							}
							break;
						case "InstanceID":
							m_InstanceID = parameterValues[1].Trim();
							break;
						default:
							break;
					}
				}

				returnValue = true;
			}
			catch(Exception ex)
			{
				// While this error should display almost immediately on startup and a message box might be appropriate,
				// writing this notification to the command log instead to make the broker as resilient as possible in unattended mode.
				WriteToCommandLog("ERROR", "Error retrieving command arguments.", "GISAutomationBroker.GetCommandArguments");
				WriteToCommandLog("ERROR", "Expected arguments: string QueueTable, integer PollingInterval, [Optional] string InstanceID.", "GISAutomationBroker.GetCommandArguments");
				WriteToCommandLog("ERROR", string.Format("Actual arguments: {0}.", commandArgs), "GISAutomationBroker.GetCommandArguments");
				WriteToCommandLog("ERROR", string.Format("Error Message: {0}", ex.Message), "GISAutomationBroker.GetCommandArguments");
				returnValue = false;
			}

			return returnValue;
		}

		/// <summary>
		/// Continuously calls ProcessRequest to process new transactions.
		/// </summary>
		/// <returns>Boolean indicating status</returns>
		private bool Process()
		{
			bool returnValue = false;

			try
			{
				do
				{
					if(!ProcessRequest())
					{
						return false;
					}

				} while(true);

			}
			catch
			{
				returnValue = false;
			}
			return returnValue;
		}

		/// <summary>
		/// Resets the request status for any requests that were interrupted.
		/// </summary>
		/// <returns>Boolean indicating status</returns>
		private bool ResetRequestStatus()
		{
			bool returnValue = false;
			string sql = string.Empty;

			try
			{
				// Query the queue table for any requests with the specified instance ID (AUTO_BROKER_ID) where REQUEST_STATUS = PROCESSING.
				// If any are found, reset REQUEST_STATUS = NEW so that they will be picked up for reprocessing in the polling loop.  
				// The assumption is that the request was interrupted during an attempt prior to restarting the automation process.
				sql = "update " + m_QueueTable + " set REQUEST_STATUS = 'NEW' where REQUEST_STATUS = 'PROCESSING'";

				if(m_InstanceID.Length > 0)
				{
					sql += " and AUTO_BROKER_ID = '" + m_InstanceID + "'";
				}

				int recordsAffected = 0;
				m_Application.DataContext.Execute(sql, out recordsAffected, (int)CommandTypeEnum.adCmdText + (int)ExecuteOptionEnum.adExecuteNoRecords);

				if(recordsAffected > 0)
				{
					m_Application.DataContext.Execute("commit", out recordsAffected, (int)CommandTypeEnum.adCmdText);
				}

				returnValue = true;
			}
			catch(Exception ex)
			{
				WriteToCommandLog("ERROR", string.Format("Error resetting {0}.REQUEST_STATUS where REQUEST_STATUS = 'PROCESSING'.", m_QueueTable), "GISAutomationBroker.ResetRequestStatus");
				WriteToCommandLog("ERROR", string.Format("SQL: {0}", sql), "GISAutomationBroker.ResetRequestStatus");
				WriteToCommandLog("ERROR", string.Format("Error message: {0}", ex.Message), "GISAutomationBroker.ResetRequestStatus");

				returnValue = false;
			}
			return returnValue;
		}

		/// <summary>
		/// Queries for new requests and passes the request to the appropriate plug-in.
		/// </summary>
		/// <returns>Boolean indicating status</returns>
		private bool ProcessRequest()
		{
			bool returnValue = false;

			int requestID = 0;
			int recordsAffected = 0;
			string logContext = string.Empty;

			try
			{
				object[] queryParams = null;

				// Query for new requests
				string sql = "select REQUEST_ID, REQUEST_SYSTEM_NM, REQUEST_SERVICE_NM, REQUEST_TRANSACT_ID, REQUEST_XML, REQUEST_WR_NBR, AUD_CREATE_UID, AUD_CREATE_TS FROM " +
									 m_QueueTable + " where REQUEST_STATUS = 'NEW'";

				if(m_InstanceID.Length > 0)
				{
					sql += " and AUTO_BROKER_ID = ?";
					queryParams = new object[1];
					queryParams[0] = m_InstanceID;
				}

				sql += " order by AUD_CREATE_TS";

				m_Application.SetStatusBarText(GTStatusPanelConstants.gtaspcMessage, "Querying the " + m_QueueTable + " table for new requests...");

				Recordset requestRS = m_Application.DataContext.Execute(sql, out recordsAffected, (int)CommandTypeEnum.adCmdText, queryParams);

				if(requestRS.RecordCount > 0)
				{
					m_Application.SetStatusBarText(GTStatusPanelConstants.gtaspcMessage, "Processing Request...");

					while(!requestRS.EOF)
					{
						// Instantiate a request object and populate it with data provided in the queue row
						GISAutoRequest autoRequest = new GISAutoRequest();

						requestID = Convert.ToInt32(requestRS.Fields["REQUEST_ID"].Value);
						autoRequest.requestID = requestID;
						autoRequest.requestSystemName = requestRS.Fields["REQUEST_SYSTEM_NM"].Value.ToString();
						autoRequest.requestServiceName = requestRS.Fields["REQUEST_SERVICE_NM"].Value.ToString();
						if(!Convert.IsDBNull(requestRS.Fields["REQUEST_TRANSACT_ID"].Value))
						{
							autoRequest.requestTransactionID = Convert.ToInt32(requestRS.Fields["REQUEST_TRANSACT_ID"].Value);
						}
						autoRequest.requestXML = requestRS.Fields["REQUEST_XML"].Value.ToString();
						autoRequest.requestWRNumber = requestRS.Fields["REQUEST_WR_NBR"].Value.ToString();
						autoRequest.auditUserID = requestRS.Fields["AUD_CREATE_UID"].Value.ToString();
						autoRequest.auditTimeStamp = requestRS.Fields["AUD_CREATE_TS"].Value.ToString();

						// Write a row to the Command Log (GIS.COMMAND_LOG) table to indicate that the request has been claimed.
						logContext = string.Format("ID {0}: {1}|{2}|{3}|{4}", m_InstanceID, autoRequest.requestSystemName, autoRequest.requestServiceName,
																			 autoRequest.requestID, autoRequest.requestWRNumber);

						WriteToCommandLog("INFO", "Request claimed", logContext);

						IGISAutoPlugin plugin = null;

						try
						{
							// Update REQUEST_STATUS = PROCESSING for the current request
							sql = "update " + m_QueueTable + " set REQUEST_STATUS = 'PROCESSING' where REQUEST_ID = ?";
							m_Application.DataContext.Execute(sql, out recordsAffected, (int)CommandTypeEnum.adCmdText, requestID);
							m_Application.DataContext.Execute("commit", out recordsAffected, (int)CommandTypeEnum.adCmdText);

							// Create appropriate plug-in
							switch(autoRequest.requestServiceName)
							{
								case "GIS_DEISTransaction":// "DEIS":
									plugin = new GISAuto_DEIS(m_TransactionManager);
									break;
								case "GIS_CreateFieldTransaction":// "ServiceLine":
									plugin = new GISAuto_ServiceLine(m_TransactionManager);
									break;
								case "GIS_RequestBatch":// "WMIS":
									plugin = new BatchProcessingModule(m_TransactionManager);
									break;
								default:
									WriteToCommandLog("ERROR", "Service Name not found in switch statement.", "GISAutomationBroker.ProcessRequest");
									break;
							}

							// Call plug-in method to process the request
							plugin.Execute(autoRequest);

							// The request has been successfully processed. Update request status and status date.
							sql = "update " + m_QueueTable + " set REQUEST_STATUS = 'COMPLETE', STATUS_DT = sysdate where REQUEST_ID = ?";
							m_Application.DataContext.Execute(sql, out recordsAffected, (int)CommandTypeEnum.adCmdText, requestID);
							m_Application.DataContext.Execute("commit", out recordsAffected, (int)CommandTypeEnum.adCmdText);
						}
						catch(Exception ex)
						{
							string logmsg = string.Format("Exception caught in Broker: {0}", ex.Message);

							if(logmsg.Length > 100)
							{
								logmsg = logmsg.Substring(0, 99);
							}

							WriteToCommandLog("INFO", "Status", logmsg);

							//using(System.IO.StreamWriter file = new System.IO.StreamWriter(@"C:\temp\gisautomator.txt", true))
							//{
							//	file.WriteLine(ex.HResult.ToString().Substring(1, 10));
							//	file.WriteLine(ex.Message);
							//	file.WriteLine(requestID.ToString());
							//}

							// Add error to request entry
							sql = "update " + m_QueueTable + " set REQUEST_STATUS = ?, RESPONSE_C = ?, RESPONSE_MSG = ?, STATUS_DT = sysdate where REQUEST_ID = ?";
							m_Application.DataContext.Execute(sql, out recordsAffected, (int)CommandTypeEnum.adCmdText, "ERROR", ex.HResult.ToString().Substring(1, 10), ex.Message, requestID);
							m_Application.DataContext.Execute("commit", out recordsAffected, (int)CommandTypeEnum.adCmdText);
							// Add error to command_log using logger class
							WriteToCommandLog("ERROR", ex.HResult + ":" + ex.Message, logContext);
						}

						plugin = null;
						autoRequest = null;

						// Compare the active job to the job initially active when the command was invoked; 
						// if the active job was changed by the plug-in module, 
						// then reactivate the original job before the next request is processed.
						string currentJob = m_Application.DataContext.ActiveJob;

						if(m_StartingJob != currentJob)
						{
							m_JobManagement.DataContext = m_Application.DataContext;
							m_JobManagement.EditJob(m_StartingJob);
						}

						requestRS.MoveNext();
					}
				}
				else
				{
					Thread.Sleep(m_PollingInterval * 1000);
				}

				returnValue = true;
			}
			catch(Exception ex)
			{
				WriteToCommandLog("ERROR", "Error processing request.", "GISAutomationBroker.ProcessRequest");
				WriteToCommandLog("ERROR", string.Format("Error message: {0}", ex.Message), "GISAutomationBroker.ProcessRequest");

				returnValue = false;
			}

			return returnValue;
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
			gtCommandLogger.LogMsg = logMessage;
			gtCommandLogger.LogContext = logContext;
			gtCommandLogger.logEntry();

			gtCommandLogger = null;
		}
	}
}
