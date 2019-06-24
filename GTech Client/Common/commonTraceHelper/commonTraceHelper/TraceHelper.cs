using System;
using Intergraph.GTechnology.API;
using ADODB;
using System.Windows.Forms;

namespace GTechnology.Oncor.CustomAPI
{
  /// <summary>
  /// Provides properties and methods to define and execute a trace.
  /// </summary>
  public class TraceHelper
  {
    private IGTApplication m_Application = GTClassFactory.Create<IGTApplication>();
    private Command m_ADOCommand = new Command();
    private string m_TraceName = "";
    private string m_TraceMetadataUserName;
    private int m_TraceID;
    private int m_SeedFID;
    private int m_HintFID = -1;
    private string m_HintIdentifiesFeature = "Y";
    private short m_RNO;

    private string m_ApplicationName;
    private int m_CommandNumber;                                       // Custom command number used for logging.
    private string m_CommandName;                                      // Custom command name used for logging.
    private bool m_InteractiveMode;                                    // Flag to indicate batch versus interactive.

    private const string TRACE_ADD_PARAMETER_ERROR = "Error adding parameters to trace command";
    private const string TRACE_DELETE_PARAMETER_ERROR = "Error deleting parameters from trace command";
    private const string TRACE_EXECUTE_ERROR = "Error executing trace";
    private const string TRACE_HINT_ERROR = "Features connected to the downstream side of the source feature are not valid for this type of trace";
    private const string TRACE_DEFINITION_ERROR = "Error creating trace definition";
    private const string TRACE_DELETE_ERROR = "Error deleting trace query";
    private const string TRACE_DELETE_LEGEND_ERROR = "Error deleting trace from legend";
    private const string TRACE_METADATA_ERROR = "No trace records exist in metadata for g3e_traceid = ";

    /// <summary>
    /// Constructor.
    /// </summary>
    /// <param name="commandNumber">G3E_CUSTOMCOMMAND.G3E_CCNO. Used for logging.</param>
    /// <param name="commandName">G3E_CUSTOMCOMMAND.G3E_USERNAME. Used for logging.</param>
    /// <returns>Boolean indicating status</returns>
    public TraceHelper(int commandNumber, string commandName)
    {
      m_CommandNumber = commandNumber;
      m_CommandName = commandName;

      // G/Tech is running not running in interactive mode, then skip message boxes.
      GUIMode guiMode = new GUIMode();
      m_InteractiveMode = guiMode.InteractiveMode;
    }

    /// <summary>
    /// Property to retrieve the name of the trace to use for the trace results.
    /// </summary>
    public string TraceName
    {
      get
      {
        return m_TraceName;
      }
    }

    /// <summary>
    /// Property to set and retrieve the name of the trace defined in metadata.
    /// </summary>
    public string TraceMetadataUserName
    {
      get
      {
        return m_TraceMetadataUserName;
      }
      set
      {
        m_TraceMetadataUserName = value;
      }
    }

    /// <summary>
    /// Property to retrieve the id of the trace defined in metadata.
    /// </summary>
    public int TraceID
    {
      get
      {
        return m_TraceID;
      }
    }

    /// <summary>
    /// Property to set and retrieve the starting G3E_FID for the trace.
    /// </summary>
    public int SeedFID
    {
      get
      {
        return m_SeedFID;
      }
      set
      {
        m_SeedFID = value;
      }
    }

    /// <summary>
    /// Property to set and retrieve the G3E_FID to be used as a hint for the trace.
    /// </summary>
    public int HintFID
    {
      get
      {
        return m_HintFID;
      }
      set
      {
        m_HintFID = value;
      }
    }

    /// <summary>
    /// Property to set if the hint identifies that the hint fid is the only
    /// feature that is connected to the seed feature (Y) or that the hint is interpreted as 
    /// defining which node of the seed feature from which the trace should be performed (N).
    /// </summary>
    public string HintIdentifiesFeature
    {
      set
      {
        m_HintIdentifiesFeature = value;
      }
    }

    /// <summary>
    /// Property to set and retrieve the name of the command that is calling the trace.
    /// </summary>
    public string ApplicationName
    {
      get
      {
        return m_ApplicationName;
      }
      set
      {
        m_ApplicationName = value;
      }
    }

    /// <summary>
    /// Property to set and retrieve the relationship number for the trace.
    /// </summary>
    public short RNO
    {
      get
      {
        return m_RNO;
      }
      set
      {
        m_RNO = value;
      }
    }

    /// <summary>
    /// Removes the trace from the trace tables and the legend.
    /// </summary>
    /// <param name="traceName">Name of the trace</param>
    /// <param name="displayPathName">Node on the legend that contains the trace entry</param>
    /// <param name="displayName">Name of the trace entry on the legend</param>
    /// <returns>Boolean indicating status</returns>
    public bool RemoveTrace(string traceName, string displayPathName, string displayName)
    {
      if(!RemoveTraceQuery(traceName))
      {
        return false;
      }

      if(!RemoveTraceLegendItem(displayPathName, displayName))
      {
        return false;
      }

      return true;
    }

    /// <summary>
    /// Defines and executes the database trace procedures.
    /// </summary>
    /// <returns>Boolean indicating status</returns>
    public bool ExecuteTrace()
    {
      bool returnValue = false;
      int recordsAffected = 0;

      try
      {
        // Create unique trace name
        m_TraceName = ApplicationName + "-" + SeedFID + "-" + DateTime.Now.ToString("yyMMddHHmmss");

        // Ensure the trace does not exist in the TRACEID table.
        RemoveTraceQuery(TraceName);

        // Define trace
        DefineTrace();

        // Set seed              
        string sql = "{call TRACE.SETSEED(?)}";
        m_ADOCommand.CommandText = sql;
        AddParameterToCommand(m_ADOCommand, DataTypeEnum.adInteger, SeedFID, ParameterDirectionEnum.adParamInput, "");
        m_Application.DataContext.ExecuteCommand(m_ADOCommand, out recordsAffected);
        DeleteADOCommandParameters();

        if(HintFID != -1)
        {
          sql = "{call TRACE.SETHINT(?,null,?)}";
          m_ADOCommand.CommandText = sql;
          AddParameterToCommand(m_ADOCommand, DataTypeEnum.adInteger, HintFID, ParameterDirectionEnum.adParamInput, "");
          AddParameterToCommand(m_ADOCommand, DataTypeEnum.adChar, m_HintIdentifiesFeature, ParameterDirectionEnum.adParamInput, "");
          m_Application.DataContext.ExecuteCommand(m_ADOCommand, out recordsAffected);
          DeleteADOCommandParameters();
        }

        sql = "{call TRACE.EXECUTE}";
        m_ADOCommand.CommandText = sql;
        m_Application.DataContext.ExecuteCommand(m_ADOCommand, out recordsAffected);

        returnValue = true;

      }
      catch(Exception ex)
      {
        if(m_InteractiveMode)
        {
          if(ex.Message.Contains("hint"))
          {
              MessageBox.Show(m_Application.ApplicationWindow, TRACE_HINT_ERROR, ApplicationName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
          }
          else
          {
              MessageBox.Show(m_Application.ApplicationWindow, TRACE_EXECUTE_ERROR + ": " + ex.Message, ApplicationName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
          }                    
        }
        WriteToCommandLog("ERROR", TRACE_EXECUTE_ERROR + ": " + ex.Message, "commonTraceHelper.ExecuteTrace");
        return returnValue;
      }
      finally
      {
        m_Application.SetStatusBarText(GTStatusPanelConstants.gtaspcMessage, "");
      }

      return returnValue;
    }

    /// <summary>
    /// Defines the parameters for the trace.
    /// </summary>
    /// <returns>Boolean indicating status</returns>
    private bool DefineTrace()
    {
      int recordsAffected = 0;
      short paramRNO = 0;
      string paramStopCriteria = string.Empty;
      string paramFilterCriteria = string.Empty;
      string paramPathCost = string.Empty;
      bool returnValue = false;

      try
      {
        // Query the trace metadata
        Recordset metadataRS = m_Application.DataContext.MetadataRecordset("G3E_TRACE_OPTABLE");
        string filter = "(g3e_username = '" + TraceMetadataUserName + "')";
        metadataRS.Filter = filter;
        if(metadataRS.RecordCount > 0)
        {
          if(!Convert.IsDBNull(metadataRS.Fields["G3E_TRACEID"].Value))
          {
            m_TraceID = Convert.ToInt16(metadataRS.Fields["G3E_TRACEID"].Value);
          }

          if(!Convert.IsDBNull(metadataRS.Fields["G3E_RNO"].Value))
          {
            paramRNO = Convert.ToInt16(metadataRS.Fields["G3E_RNO"].Value);
          }

          if(metadataRS.Fields["G3E_STOPCRITERIA"].Value.ToString() != "")
          {
            paramStopCriteria = metadataRS.Fields["G3E_STOPCRITERIA"].Value.ToString();
          }

          if(metadataRS.Fields["G3E_FILTERCRITERIA"].Value.ToString() != "")
          {
            paramFilterCriteria = metadataRS.Fields["G3E_FILTERCRITERIA"].Value.ToString();
          }

          if(metadataRS.Fields["G3E_PATHCOST"].Value.ToString() != "")
          {
            paramPathCost = metadataRS.Fields["G3E_PATHCOST"].Value.ToString();
          }

        }
        else
        {
          if(m_InteractiveMode)
          {
            MessageBox.Show(m_Application.ApplicationWindow, TRACE_METADATA_ERROR + ": " + TraceID, ApplicationName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
          }
          WriteToCommandLog("ERROR", TRACE_METADATA_ERROR + ": " + TraceID, "commonTraceHelper.DefineTrace");
          returnValue = false;
          return returnValue;
        }

        string sql = "{call TRACE.DEFINE(?, ?, ?, ?, ?, ?)}";

        // Build parameters
        m_ADOCommand.CommandText = sql;
        m_ADOCommand.CommandType = CommandTypeEnum.adCmdText;
        AddParameterToCommand(m_ADOCommand, DataTypeEnum.adChar, TraceName, ParameterDirectionEnum.adParamInput, "");
        AddParameterToCommand(m_ADOCommand, DataTypeEnum.adSmallInt, paramRNO, ParameterDirectionEnum.adParamInput, "");
        AddParameterToCommand(m_ADOCommand, DataTypeEnum.adChar, paramStopCriteria, ParameterDirectionEnum.adParamInput, "");
        AddParameterToCommand(m_ADOCommand, DataTypeEnum.adChar, paramFilterCriteria, ParameterDirectionEnum.adParamInput, "");
        AddParameterToCommand(m_ADOCommand, DataTypeEnum.adChar, paramPathCost, ParameterDirectionEnum.adParamInput, "");
        AddParameterToCommand(m_ADOCommand, DataTypeEnum.adSmallInt, TraceID, ParameterDirectionEnum.adParamInput, "");

        m_Application.DataContext.ExecuteCommand(m_ADOCommand, out recordsAffected);

        returnValue = true;

      }
      catch(Exception ex)
      {
        if(m_InteractiveMode)
        {
          MessageBox.Show(m_Application.ApplicationWindow, TRACE_DEFINITION_ERROR + ": " + ex.Message, ApplicationName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        }
        WriteToCommandLog("ERROR", TRACE_DEFINITION_ERROR + ": " + ex.Message, "commonTraceHelper.DefineTrace");
        returnValue = false;
      }
      finally
      {
        DeleteADOCommandParameters();
      }

      return returnValue;
    }

    /// <summary>
    /// Adds a parameter to the specified command object using the specified parameters.
    /// </summary>
    /// <param name="oADOCommand">Command to which to add the parameter</param>
    /// <param name="parameterType">Data type for the parameter</param>
    /// <param name="varParameterValue">Value for the parameter</param>
    /// <param name="parameterDirection">Direction of the parameter - In, Out, In/Out</param>
    /// <param name="parameterName">Name of the parameter</param>
    /// <returns>Boolean indicating status</returns>
    private bool AddParameterToCommand(Command oADOCommand, DataTypeEnum parameterType, object varParameterValue, ParameterDirectionEnum parameterDirection, string parameterName)
    {
      bool returnValue = false;

      try
      {
        Parameter oParameter = default(Parameter);
        Command adoCommand = oADOCommand;
        oParameter = adoCommand.CreateParameter();
        oParameter.Direction = parameterDirection;

        if(parameterName.Length > 0)
        {
          oParameter.Name = parameterName;
        }

        oParameter.Type = parameterType;
        if(oParameter.Type == DataTypeEnum.adChar | oParameter.Type == DataTypeEnum.adVarChar)
        {
          if(Convert.IsDBNull(varParameterValue) || (string)varParameterValue == string.Empty)
          {
            oParameter.Size = 1;
          }
          else
          {
            oParameter.Size = Convert.ToInt32(varParameterValue.ToString().Length);
          }
        }

        if(Convert.IsDBNull(varParameterValue) || varParameterValue.ToString() == string.Empty)
        {
          Object adParamNullable = null;
          oParameter.Attributes = Convert.ToInt32(adParamNullable);
          oParameter.Direction = ParameterDirectionEnum.adParamInput;
          oParameter.Value = DBNull.Value;
        }
        else
        {
          oParameter.Value = varParameterValue;
        }
        adoCommand.Parameters.Append(oParameter);
        oParameter = null;
        returnValue = true;
      }
      catch(Exception ex)
      {
        if(m_InteractiveMode)
        {
          MessageBox.Show(m_Application.ApplicationWindow, TRACE_ADD_PARAMETER_ERROR + ": " + ex.Message, ApplicationName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        }
        WriteToCommandLog("ERROR", TRACE_ADD_PARAMETER_ERROR + ": " + ex.Message, "commonTraceHelper.AddParameterToCommand");
        returnValue = false;
      }

      return returnValue;
    }

    /// <summary>
    /// Delete any parameters that may have been defined for the ADO command object.
    /// </summary>
    /// <returns>Boolean indicating status</returns>
    private bool DeleteADOCommandParameters()
    {
      bool returnValue = false;

      try
      {
        for(short i = 0;i < m_ADOCommand.Parameters.Count;)
        {
          m_ADOCommand.Parameters.Delete(i);
        }

        returnValue = true;
      }
      catch(Exception ex)
      {
        if(m_InteractiveMode)
        {
          MessageBox.Show(m_Application.ApplicationWindow, TRACE_DELETE_PARAMETER_ERROR + ": " + ex.Message, ApplicationName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        }
        WriteToCommandLog("ERROR", TRACE_DELETE_PARAMETER_ERROR + ": " + ex.Message, "commonTraceHelper.DeleteADOCommandParameters");
        returnValue = false;
      }

      return returnValue;
    }

    /// <summary>
    /// Delete the trace from the traceid, tracelog and traceresult tables.
    /// </summary>
    /// /// <param name="name">Name of the trace</param>
    /// <returns>Boolean indicating status</returns>
    private bool RemoveTraceQuery(string name)
    {
      bool returnValue = false;

      try
      {
        int recordsAffected = 0;

        // --- Delete trace query from TRACEID table.
        string sql = "{call TRACE.DELETE(?)}";
        m_ADOCommand.CommandType = CommandTypeEnum.adCmdText;
        m_ADOCommand.CommandText = sql;
        AddParameterToCommand(m_ADOCommand, DataTypeEnum.adVarChar, name, ParameterDirectionEnum.adParamInput, "");
        m_Application.DataContext.ExecuteCommand(m_ADOCommand, out recordsAffected);
        m_Application.DataContext.Execute("commit", out recordsAffected, (int)CommandTypeEnum.adCmdText);
        DeleteADOCommandParameters();

        returnValue = true;
      }
      catch(Exception ex)
      {
        if(m_InteractiveMode)
        {
          MessageBox.Show(m_Application.ApplicationWindow, TRACE_DELETE_ERROR + ": " + ex.Message, ApplicationName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        }
        WriteToCommandLog("ERROR", TRACE_DELETE_ERROR + ": " + ex.Message, "commonTraceHelper.RemoveTraceQuery");
        returnValue = false;
      }

      return returnValue;
    }

    /// <summary>
    /// Delete the trace from the legend.
    /// </summary>
    /// <param name="displayPathName">Node on the legend that contains the trace entry</param>
    /// <param name="displayName">Name of the trace entry on the legend</param>
    /// <returns>Boolean indicating status</returns>
    public bool RemoveTraceLegendItem(string displayPathName, string displayName)
    {
      bool returnValue = false;

      try
      {
        // --- Remove from display control
        IGTMapWindows gtMapWindows = m_Application.GetMapWindows(GTMapWindowTypeConstants.gtapmtGeographic);

        foreach(IGTMapWindow gtMapWindow in gtMapWindows)
        {
          try
          {
            gtMapWindow.DisplayService.Remove(displayPathName, displayName);
          }
          catch
          {
            // Ignore error if node is not on display control
          }
        }

        gtMapWindows = null;

        m_Application.RefreshWindows();

        returnValue = true;
      }
      catch(Exception ex)
      {
        if(m_InteractiveMode)
        {
          MessageBox.Show(m_Application.ApplicationWindow, TRACE_DELETE_LEGEND_ERROR + ": " + ex.Message, ApplicationName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        }
        WriteToCommandLog("ERROR", TRACE_DELETE_LEGEND_ERROR + ": " + ex.Message, "commonTraceHelper.RemoveTraceLegendItem");
        returnValue = false;
      }

      return returnValue;
    }

    /// <summary>
    /// Gets the upstream G3E_FID matching the passed in targetFNOs for the passed in fid.
    /// </summary>
    /// <param name="trace">The trace recordset</param>
    /// <param name="fid">The G3E_FID to find the sourceFID</param>
    /// /// <param name="targetFNOs">An array of G3E_FNO values to consider as the source</param>
    /// /// <param name="sourceFID">The located source G3E_FID</param>
    /// <returns>Boolean indicating status</returns>
    public bool GetConnectivitySourceFID(Recordset trace, int fid, short[] targetFNOs, out int sourceFID)
    {
      bool returnValue = false;

      sourceFID = 0;

      // Get the source fid record of the passed in fid.
      // If the source fid record matches the passed in targetFNO then return the sourceFID
      //  else traverse up the trace recordset until the source fno record matches the passed in targetFNO

      string filter = "g3e_fid = " + fid;
      trace.Filter = filter;

      if(trace.RecordCount > 0)
      {
        short sourceFNO = Convert.ToInt16(trace.Fields["G3E_FNO"].Value);
        foreach(short targetFNO in targetFNOs)
        {
          if(sourceFNO == targetFNO)
          {
            sourceFID = Convert.ToInt32(trace.Fields["G3E_FID"].Value);
            break;
          }
        }

        if(sourceFID == 0)
        {
          fid = Convert.ToInt32(trace.Fields["G3E_SOURCEFID"].Value);
          trace.Filter = "";
          GetConnectivitySourceFID(trace, fid, targetFNOs, out sourceFID);
        }
      }
      else
      {
        sourceFID = 0;
      }

      return returnValue;
    }

    /// <summary>
    /// Adds the results of the trace to the legend.
    /// </summary>
    /// <param name="displayRS">The trace recordset to display</param>
    /// <param name="displayPathName">Node on the legend that contains the trace entry</param>
    /// <param name="displayName">Name of the trace entry on the legend</param>
    /// <param name="symbology">Symbology overrides for the trace</param>
    /// <param name="overrideStyle">True if style should be overridden</param>
    /// <param name="primaryGraphicsOnly">True if only the primary graphic should be displayed</param>
    /// <param name="activeWindowOnly">True if the results should only be displayed in the active window</param>
    /// <param name="removeItem">True if the existing item should be removed from the legend</param>
    /// <returns>Boolean indicating status</returns>
    public bool DisplayResults(Recordset displayRS, string displayPathName, string displayName, IGTSymbology symbology,
                                bool overrideStyle, bool primaryGraphicsOnly, bool activeWindowOnly, bool removeItem)
    {
      IGTMapWindow gtMapWindow = default(IGTMapWindow);
      IGTMapWindows gtMapWindows = default(IGTMapWindows);
      IGTDisplayService gtDisplayService = default(IGTDisplayService);

      bool returnValue = false;

      try
      {
        if(removeItem)
        {
          // Remove existing item from the Display Control window
          try
          {
            if(activeWindowOnly)
            {
              gtMapWindow = m_Application.ActiveMapWindow;
              gtMapWindow.DisplayService.Remove(displayPathName, displayName);
            }
            else
            {
              gtMapWindows = m_Application.GetMapWindows(GTMapWindowTypeConstants.gtapmtGeographic);

              foreach(IGTMapWindow mapWindow in gtMapWindows)
              {
                mapWindow.DisplayService.Remove(displayPathName, displayName);
              }
            }
          }
          catch
          {
            // Ignore error if item is not on the legend
          }

        }

        if(displayRS.RecordCount > 0)
        {
          if(activeWindowOnly)
          {
            gtMapWindow = m_Application.ActiveMapWindow;
            gtDisplayService = gtMapWindow.DisplayService;

            if(overrideStyle)
            {
              gtDisplayService.AppendQuery(displayPathName, displayName, displayRS, symbology, primaryGraphicsOnly);
            }
            else
            {
              gtDisplayService.AppendQuery(displayPathName, displayName, displayRS);
            }
          }
          else
          {
            gtMapWindows = m_Application.GetMapWindows(GTMapWindowTypeConstants.gtapmtAll);
            foreach(IGTMapWindow mapWindow in gtMapWindows)
            {
              gtDisplayService = mapWindow.DisplayService;

              if(overrideStyle)
              {
                gtDisplayService.AppendQuery(displayPathName, displayName, displayRS, symbology, primaryGraphicsOnly);
              }
              else
              {
                gtDisplayService.AppendQuery(displayPathName, displayName, displayRS);
              }
            }
          }
        }

        m_Application.RefreshWindows();

        returnValue = true;
      }
      catch
      {
        returnValue = false;
      }
      finally
      {
        gtMapWindow = null;
        gtMapWindows = null;
        gtDisplayService = null;
      }

      return returnValue;
    }

    /// <summary>
    /// Validates that the seedFNO is a valid starting feature for the trace.
    /// </summary>
    /// <param name="seedFNO">G3E_FNO to validate as the seed feature for the trace</param>
    /// <returns>Boolean indicating status</returns>
    public bool ValidateSeedFNO(short seedFNO)
    {
      bool returnValue = false;

      try
      {
        // Query the trace metadata
        Recordset metadataRS = m_Application.DataContext.MetadataRecordset("G3E_TRACE_OPTABLE", "g3e_fno = " + seedFNO + " and g3e_username = '" + m_TraceMetadataUserName + "'");

        if(metadataRS.RecordCount > 0)
        {
          m_RNO = Convert.ToInt16(metadataRS.Fields["G3E_RNO"].Value);
          returnValue = true;
        }

      }
      catch(Exception ex)
      {
        if(m_InteractiveMode)
        {
          MessageBox.Show(m_Application.ApplicationWindow, "ValidateSeedFNO: " + ex.Message, ApplicationName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        }
        WriteToCommandLog("ERROR", ex.Message, "commonTraceHelper.ValidateSeedFNO");
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
      gtCommandLogger.CommandNum = m_CommandNumber;
      gtCommandLogger.CommandName = m_CommandName;
      gtCommandLogger.LogType = logType;
      gtCommandLogger.LogMsg = logMessage;
      gtCommandLogger.LogContext = logContext;
      gtCommandLogger.logEntry();

      gtCommandLogger = null;
    }
  }
}
