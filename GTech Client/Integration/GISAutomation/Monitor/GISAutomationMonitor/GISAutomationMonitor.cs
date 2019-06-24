using Microsoft.Win32;
using System;
using System.Configuration;
using System.Diagnostics;
using System.Threading;
using System.Timers;
using System.Windows.Forms;
using System.Data.OleDb;
using System.Data;

namespace GTechnology.Oncor.CustomAPI
{
  class GISAutomationMonitor
  {
    private System.Timers.Timer m_PollingTimer;                         // Timer to check if GISAutomationBroker command is hung
    private int m_PreviousRequestID = -1;                               // Queue table request id from previous timer event
    private string m_Username = string.Empty;                           // Name of user for logging into G/Tech
    private string m_Password = string.Empty;                           // Password of user for logging into G/Tech
    private string m_ParamConfig = string.Empty;                        // G/Tech configuration (app.config - Config)
    private string m_DataSource = string.Empty;                         // Database SID from tnsnames.ora (app.config - DataSource)
    private string m_ParamAutoCustomCommand = string.Empty;             // GISAutomationBroker custom command number (app.config - AutoCustomCommand)
    private string m_ParamAutoCustomCommandArgument = string.Empty;     // Arguments for custom command (app.config - AutoCustomCommandArgument)
    private string m_ParamActiveJob = string.Empty;                     // G/Tech job to activate (app.config - ActiveJob)
    private int m_ParamHeartbeatInterval = 300;                         // Interval to check if GISAutomationBroker command is hung (app.config - HeartbeatInterval)
    private string m_EmailListTo = string.Empty;                        // To Email list to send email if GISAutomationBroker command is hung (app.config - EmailListTo)
    private string m_EmailListFrom = string.Empty;                      // From Email list to send email if GISAutomationBroker command is hung (app.config - EmailListFrom)
    private string m_QueueTable = string.Empty;                         // Queue table to query to see if GISAutomationBroker command is hung (app.config - AutoCustomCommandArgument)
    private int m_BrokerPollingInterval = 0;                            // Polling interval for GISAutomationBroker command (app.config - AutoCustomCommandArgument)
    private string m_BrokerInstanceID = string.Empty;                   // Broker id for GISAutomationBroker command (app.config - AutoCustomCommandArgument)

    private const int GTECH_START_WAIT_TIME = 300;                      // Wait time in seconds between time G/Tech is called to start and before starting timer.

    /// <summary>
    /// The main driver of the GISAutomationMonitor application.
    /// </summary>
    /// <param name="mre">The wait event. Prevents the application from exiting until the event is fired</param>
    internal void Run(ManualResetEvent mre)
    {
      // Get the application parameters from the app.config file.
      if(!GetApplicationParameters())
      {
        mre.Set();
        return;
      }

      // Call LaunchGIS to start G/Tech with GISAutomationBroker custom command
      if(!StartGTech())
      {
        mre.Set();
        return;
      }

      // Wait for G/Tech to fully launch and GISAutomationBroker command to start then start timer
      Thread.Sleep(GTECH_START_WAIT_TIME * 1000);

      // Create timer to check if the GISAutomationBroker is hung
      m_PollingTimer = new System.Timers.Timer();
      m_PollingTimer.Elapsed += new ElapsedEventHandler(OnTimedEvent);
      m_PollingTimer.Interval = m_ParamHeartbeatInterval * 1000;
      m_PollingTimer.Start();
    }

    /// <summary>
    /// Monitors the GIS automation queue table to see if the GISAutomationBroker is running.
    /// </summary>
    /// <param name="source">The source of the elapsed event.</param>
    /// <param name="e">The ElapsedEventArgs object that contains the event data.</param>
    private void OnTimedEvent(object source, ElapsedEventArgs e)
    {
      OleDbConnection oraConnection = null;
      OleDbDataReader oraReader = null;

      try
      {
        string sql = "select REQUEST_ID FROM " + m_QueueTable + " where REQUEST_STATUS IN ('PROCESSING','NEW') ";

        if(m_BrokerInstanceID.Length > 0)
        {
          sql += " and AUTO_BROKER_ID = " + m_BrokerInstanceID;
        }

        sql += " order by AUD_CREATE_TS";

        String connectionString = string.Format(@"Provider=MSDAORA.1;User ID={0};password={1};Data Source = {2}; Persist Security Info = False", m_Username, m_Password, m_DataSource);

        oraConnection = new OleDbConnection(connectionString);
        OleDbCommand oraCommand = new OleDbCommand(sql, oraConnection);

        oraConnection.Open();
        oraReader = oraCommand.ExecuteReader();
        oraReader.Read();

        if(oraReader.HasRows)
        {
          // Get the oldest unprocessed Request ID from the queue table.
          // If the Request ID is the same as the previous query then the GISAutomationBroker command is hung.
          int requestID = Convert.ToInt32(oraReader.GetValue(0));

          if(requestID == m_PreviousRequestID)
          {
            int recordCount = 1;
            while(oraReader.Read())
            {
              recordCount++;
            }

            oraReader.Close();

            // GISAutomationBroker command appears to be hung. Send email.
            oraCommand.CommandType = CommandType.Text;
            oraCommand.CommandText = string.Format("begin Send_EF_Email_PKG.emToAddress := '{0}'; end;", m_EmailListTo);
            oraCommand.ExecuteNonQuery();
            oraCommand.CommandText = string.Format("begin Send_EF_Email_PKG.emFromAddress := '{0}'; end;", m_EmailListFrom);
            oraCommand.ExecuteNonQuery();
            oraCommand.CommandText = string.Format("begin Send_EF_Email_PKG.emSubject := 'GISAutomationBroker has stopped on {0}'; end;", Environment.MachineName);
            oraCommand.ExecuteNonQuery();
            oraCommand.CommandText = string.Format("begin Send_EF_Email_PKG.emMessage := 'Current Request ID: {0}{1}Number of pending records in the {2} table: {3}'; end;", requestID, Environment.NewLine, m_QueueTable, recordCount);
            oraCommand.ExecuteNonQuery();
            oraCommand.CommandText = "begin Send_EF_Email_PKG.SendEmail(); end;";
            oraCommand.ExecuteNonQuery();
          }

          m_PreviousRequestID = requestID;
        }
        else
        {
          m_PreviousRequestID = -1;
        }
      }
      catch(Exception ex)
      {
        // Log an error to the Applcation Error Event Log.
        if(EventLog.SourceExists("Application Error"))
        {
          EventLog.WriteEntry("Application Error", string.Format("GISAutomationMonitor:  Error in OnTimedEvent determining pending records and sending notification.{0}{1}", Environment.NewLine, ex.Message));
        }
      }
      finally
      {
        oraReader.Close();
        oraConnection.Close();
        oraConnection.Dispose();
      }
    }

    /// <summary>
    /// Gets the parameters for the GISAutomationMonitor application and GISAutomationBroker custom command.
    /// </summary>
    /// <returns>Boolean indicating status</returns>
    private bool GetApplicationParameters()
    {
      bool returnValue = false;

      try
      {
        // TODO: need to get username using authentication. Work-around for now is to add username to App.config.
        m_Username = ConfigurationManager.AppSettings.Get("Username");
        if(m_Username.Length == 0)
        {
          throw new Exception("Unable to retrieve username for logging into G/Technology.");
        }

        // TODO: need to get password using authentication. Work-around for now is to add password to App.config.
        m_Password = ConfigurationManager.AppSettings.Get("Password");
        if(m_Password.Length == 0)
        {
          throw new Exception("Unable to retrieve password for logging into G/Technology.");
        }

        m_ParamConfig = ConfigurationManager.AppSettings.Get("Config");
        if(m_ParamConfig.Length == 0)
        {
          throw new Exception("Config parameter in app.config is null.");
        }

        m_DataSource = ConfigurationManager.AppSettings.Get("DataSource");
        if(m_DataSource.Length == 0)
        {
          throw new Exception("Unable to retrieve data source.");
        }

        m_ParamAutoCustomCommand = ConfigurationManager.AppSettings.Get("AutoCustomCommand");
        if(m_ParamAutoCustomCommand.Length == 0)
        {
          throw new Exception("AutoCustomCommand parameter in app.config is null.");
        }

        m_ParamAutoCustomCommandArgument = ConfigurationManager.AppSettings.Get("AutoCustomCommandArgument");
        if(m_ParamAutoCustomCommandArgument.Length == 0)
        {
          throw new Exception("AutoCustomCommandArgument parameter in app.config is null.");
        }

        m_ParamActiveJob = ConfigurationManager.AppSettings.Get("ActiveJob");
        if(m_ParamActiveJob.Length == 0)
        {
          throw new Exception("ActiveJob parameter in app.config is null.");
        }

        m_ParamHeartbeatInterval = Convert.ToInt32(ConfigurationManager.AppSettings.Get("HeartbeatInterval"));

        m_EmailListTo = ConfigurationManager.AppSettings.Get("EmailListTo");
        if(m_EmailListTo.Length == 0)
        {
          throw new Exception("EmailListTo parameter in app.config is null.");
        }

        m_EmailListFrom = ConfigurationManager.AppSettings.Get("EmailListFrom");
        if(m_EmailListFrom.Length == 0)
        {
          throw new Exception("EmailListFrom parameter in app.config is null.");
        }

        string[] commandArgsArray = m_ParamAutoCustomCommandArgument.Split(',');

        foreach(string arg in commandArgsArray)
        {
          string[] parameterValues = arg.Split('=');

          switch(parameterValues[0].Trim())
          {
            case "QueueTable":
              m_QueueTable = parameterValues[1].Trim();
              break;
            case "PollingInterval":
              if(!int.TryParse(parameterValues[1].Trim(), out m_BrokerPollingInterval))
              {
                throw new Exception("Non-integer value for PollingInterval");
              }
              break;
            case "InstanceID":
              m_BrokerInstanceID = parameterValues[1].Trim();
              break;
            default:
              break;
          }
        }

        if(m_QueueTable.Length == 0)
        {
          throw new Exception("QueueTable value in AutoCustomCommandArgument parameter in app.config is null.");
        }

        if(m_BrokerPollingInterval == 0)
        {
          throw new Exception("PollingInterval value in AutoCustomCommandArgument parameter in app.config must be greater than zero.");
        }

        returnValue = true;
      }
      catch(Exception ex)
      {
        // Log an error to the Applcation Error Event Log.
        if(EventLog.SourceExists("Application Error"))
        {
          EventLog.WriteEntry("Application Error", string.Format("GISAutomationMonitor:  Error reading configuration parameters from app.config.{0}{1}", Environment.NewLine, ex.Message));
        }

        returnValue = false;
      }

      return returnValue;
    }

    /// <summary>
    /// Starts an instance of G/Technology with the GISAutomationBroker custom command by calling LaunchGIS.
    /// </summary>
    /// <returns>Boolean indicating status</returns>
    private bool StartGTech()
    {
      bool returnValue = false;

      try
      {
        // Get G/Tech program path from registry
        string gtechPath = string.Empty;
        string regSubKey = @"SOFTWARE\Intergraph\GFramme\02.00\setup\";
        RegistryKey regKey = Registry.LocalMachine.OpenSubKey(regSubKey);
        if(regKey != null)
        {
          object regValue = regKey.GetValue("svAppPath");
          if(regValue != null)
          {
            gtechPath = regValue.ToString();
          }
          else
          {
            throw new Exception(string.Format("Error getting G/Technology program path from registy using registry key: {0}", regSubKey + "svAppPath"));
          }
        }
        else
        {
          throw new Exception(string.Format("Error getting G/Technology program path from registy using registry key: {0}", regSubKey));
        }

        // Start G/Tech with the GISAutomationBroker custom command by calling LaunchGIS
        ProcessStartInfo procStartInfo = new ProcessStartInfo();
        procStartInfo.WorkingDirectory = gtechPath;
        procStartInfo.FileName = "LaunchGIS.exe";
        procStartInfo.Arguments = "Username=" + m_Username + " Password=" + m_Password + " Config=" + m_ParamConfig + " AutoCustomCommand=" +
                                   m_ParamAutoCustomCommand + " AutoCustomCommandArgument=\"" + m_ParamAutoCustomCommandArgument + "\" ActiveJob=" + m_ParamActiveJob;
        Process.Start(procStartInfo);

        returnValue = true;
      }
      catch(Exception ex)
      {
        // Log an error to the Applcation Error Event Log.
        if(EventLog.SourceExists("Application Error"))
        {
          EventLog.WriteEntry("Application Error", string.Format("GISAutomationMonitor:  Error starting G/Technology.{0}{1}", Environment.NewLine, ex.Message));
        }

        returnValue = false;
      }

      return returnValue;
    }
  }
}
