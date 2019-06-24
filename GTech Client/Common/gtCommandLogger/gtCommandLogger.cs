using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Intergraph.GTechnology.API;
using ADODB;

namespace gtCommandLogger
{
	public class gtCommandLogger
	{

		// Using a property to ensure the length of the incoming string
		// does not exceed the column size where it will be stored.
		public string LogMsg
		{
			set
			{
				if(1000 < value.Length)
				{
					// Unsure why a string length of 1000 still sometimes yields
					// an Oracle size error so providing a bit more buffer at 950.
					logMsg = value.Substring(0, 950);
				}
				else
				{
					logMsg = value;
				}
			}
			get
			{
				return logMsg;
			}
		}

		public int CommandNum = 0;                  // required property if not set the default value is 0 and an error will be generated.
		public string CommandName = string.Empty;   // optional property
		public string LogType = "ERROR";            // required property if not set the default value is ERROR
		public string LogCode = string.Empty;       // optional property
		public string LogContext = string.Empty;    // optional property

		private bool isInteractive = true;          // required property if not set the default is true
		public string logMsg = string.Empty;        // string used for the message being logged

		/// <summary>
		/// buildValuesList builds the 'values' portion of an Oracle insert statement that will 
		///   insert rows into the Command_log table.
		/// The function also insures that all the values are valid.
		/// </summary>
		/// <returns>
		/// The function returns a string that represents the values portion of the Oracle 
		///   insert statement in the insert script.</returns>
		private string buildValuesList()
		{
			string tmpValuesStr = string.Empty;
			string tmpException = string.Empty;
			string tmpStr = string.Empty;

			try
			{
				tmpValuesStr = "(";
				// Build the 'values' portion of the insert string,
				//    and check for valid or existing values.
				if(CommandNum == 0)
				{
					tmpStr = "CommandNum value is not set.\n";
				}
				else
				{
					tmpValuesStr = tmpValuesStr + CommandNum.ToString() + ",";
				}

				if(CommandName == string.Empty)
				{
					tmpValuesStr = tmpValuesStr + "NULL,";
				}
				else
				{
					tmpValuesStr = tmpValuesStr + "'" + CommandName + "',";
				}

				switch(LogType)
				{
					case "ERROR":
					case "INFO":
					case "WARNING":
						tmpValuesStr = tmpValuesStr + "'" + LogType + "',";
						break;
					default:
						tmpStr = tmpStr + "LogType is not set to 'ERROR', 'INFO', or 'WARNING.\n";
						break;
				}

				if(LogCode == string.Empty)
				{
					tmpValuesStr = tmpValuesStr + "NULL,";
				}
				else
				{
					tmpValuesStr = tmpValuesStr + "'" + LogCode.Replace("'", "''") + "',";
				}

				if(logMsg == string.Empty)
				{
					tmpStr = tmpStr + "LogMsg is not set.\n ";
				}
				else
				{
					tmpValuesStr = tmpValuesStr + "'" + logMsg.Replace("'", "''") + "',";
				}

				LogMsg.Replace("'", "''");

				if(1000 < LogMsg.Length)
				{
					// Unsure why a string length of 1000 still sometimes yields
					// an Oracle size error so providing a bit more buffer at 950.
					if(!LogMsg.Substring(950).Contains("'"))
					{
						// If the part of the string that's > 1000 characters
						// does not contain a quote, then it's safe to truncate it.
						LogMsg = LogMsg.Substring(0, 950);
					}
					else
					{
						// Truncating the string would invalidate the matching quotes.
						// Write the message to a text file and log the text file info.

						string logFileName = @"C:\temp\gisautomator.txt";
						using(System.IO.StreamWriter file = new System.IO.StreamWriter(logFileName, true))
						{
							file.WriteLine(LogMsg);
						}

						LogMsg = string.Format("Log message was not formatted such that it could be logged in COMMAND_LOG.  See {0} for the contents of the current log message.", logFileName);
					}
				}

				if(LogContext == string.Empty)
				{
					tmpValuesStr = tmpValuesStr + "NULL)";
				}
				else
				{
					tmpValuesStr = tmpValuesStr + "'" + LogContext.Replace("'", "''") + "') ";
				}

				// Throw an error if any of the values are bad.
				if(tmpStr != string.Empty)
				{
					tmpException = tmpException + "Values Set: " + tmpStr + "\n A log entry was not added to the COMMAND_LOG table.";
					throw new Exception(tmpException);
				}
			}
			catch(Exception e)
			{
				if(isInteractive)
				{
					// if in an interactive GTech Session, Show errors in a message box.
					MessageBox.Show("Error: " + e.Message, "Error: gtCommandLogger.buildValuesList", MessageBoxButtons.OK);
				}
				else
				{
					// Log an error to the Applcation Error Event Log.
					if(EventLog.SourceExists("Application Error"))
					{
						EventLog.WriteEntry("Application Error", "Error in G/Technology Custom Command Logger - gtCommandLogger.buildValuesList: " + e.Message);
					}
				}
				//return a empty string if an error occurred. 
				tmpValuesStr = string.Empty;
			}
			// return the insert values string.
			return tmpValuesStr;
		}

		public gtCommandLogger()
		{
			GTechnology.Oncor.CustomAPI.GUIMode guiMode = new GTechnology.Oncor.CustomAPI.GUIMode();
			this.isInteractive = guiMode.InteractiveMode;
		}

		/// <summary>
		/// logEntry function logs an entry in the Command_Log table. It uses the 
		///  properties in the class to generate the log entry.
		/// </summary>
		/// <returns>true if it succeeds and false if it fails.</returns>
		public bool logEntry()
		{
			bool tmpReturn = true;
			string tmpInsertStr = string.Empty;

			try
			{
				// Get the GTech datacontext.
				IGTApplication tmpApp = GTClassFactory.Create<IGTApplication>();
				IGTDataContext tmpDatCont = tmpApp.DataContext;

				// Build the data insert script.
				tmpInsertStr = "begin insert into COMMAND_LOG columns(COMMAND_NBR,COMMAND_NAME,LOG_TYPE,LOG_CODE,LOG_MSG,LOG_CONTEXT) values";

				// get the values list.
				string tmpValues = buildValuesList();

				if(string.IsNullOrEmpty(tmpValues))
				{
					//The buildValuesList function generated an error and a empty string was returned. 
					tmpReturn = false;
				}
				else
				{
					// The buildValuesList function succeeded.
					// Complete the insert script.
					tmpInsertStr = string.Format("{0} {1};commit;end;", tmpInsertStr, tmpValues);
					// Execute the insert script
					tmpDatCont.Execute(tmpInsertStr, out int tmpRecUpdated, (int)CommandTypeEnum.adCmdText);
				}
			}
			catch(Exception e)
			{
				if(isInteractive)
				{
					// if in an interactive GTech Session, Show errors in a message box.
					MessageBox.Show("Error: " + e.Message, "Error: gtCommandLogger.logEntry", MessageBoxButtons.OK);
					tmpReturn = false;
				}
				else
				{
					// If the error is due to privileges (resulting in a "table or view does not exist"), then only write
					// the exception message to the event log.  Otherwise, write the error to the command log.
					if(e.Message.Contains("ORA-00942"))
					{
						// Log an error to the Applcation Error Event Log.
						if(EventLog.SourceExists("Application Error"))
						{
							EventLog.WriteEntry("Application Error", "Error in G/Technology Custom Command Logger - gtCommandLogger.logEntry: " + e.Message);
						}
					}
					else
					{
						// This may risk infinite recursion, but since the error is not due to table access,
						// then merely logging the error message should not produce an exception and when this call returns,
						// then this "catch" should fall through and exit.
						this.logMsg = e.Message;
						this.logEntry();
					}
				}
			}

			return tmpReturn;
		}
	}
}
