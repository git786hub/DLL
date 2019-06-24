using ADODB;
using Intergraph.GTechnology.API;
using Intergraph.GTechnology.Interfaces;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace GTechnology.Oncor.CustomAPI
{
    public class ccEnhancedUpstreamTrace : IGTCustomCommandModeless
    {
        private IGTApplication m_Application = GTClassFactory.Create<IGTApplication>();
        private IGTCustomCommandHelper m_CustomCommandHelper = null;
        private frmSourceResults m_frmSourceResults = new frmSourceResults();

        private string m_TraceName = string.Empty;

        private const int CUSTOM_COMMAND_NUMBER = 97;
        private const string CUSTOM_COMMAND_NAME = "Enhanced Upstream Trace";
        private const short CNO_CONNECTIVITY_ATTRIBUTES = 11;
        private short FNO_TRANSFORMER_OH = 59;
        private short FNO_TRANSFORMER_OH_NETWORK = 98;
        private short FNO_TRANSFORMER_UG = 60;
        private short FNO_TRANSFORMER_UG_NETWORK = 99;
        private short FNO_SUBSTATION_BREAKER = 16;
        private short FNO_SUBSTATION_BREAKER_NETWORK = 91;

        private List<short> m_SourceFNOs = new List<short>(new short[] { 16, 91, 59, 98, 60, 99 });                                                // List of feeding features.
        private List<short> m_SecondaryFNOs = new List<short>(new short[] { 23, 52, 53, 54, 55, 63, 86, 94, 95, 96, 97, 154, 155, 161, 162 });     // List of Secondary features. Used to determine which trace to execute.

        public bool CanTerminate
        {
            get
            {
                return true;
            }
        }

        public IGTTransactionManager TransactionManager { set => throw new NotImplementedException(); }

        /// <summary>
        /// The entry point for the custom command.
        /// </summary>
        /// <param name="CustomCommandHelper">Provides notification to the system that the command has finished</param>
        public void Activate(IGTCustomCommandHelper CustomCommandHelper)
        {
            try
            {
                m_CustomCommandHelper = CustomCommandHelper;

                // Get feature number and feature identifier for feature in select set
                IGTDDCKeyObjects oGTDCKeys = GTClassFactory.Create<IGTDDCKeyObjects>();
                oGTDCKeys = m_Application.SelectedObjects.GetObjects();
                short fno = oGTDCKeys[0].FNO;
                int fid = oGTDCKeys[0].FID;

                // Check if feature has connectivity component
                if (!ValidateSeedConnectivity(fno))
                {
                    CustomCommandHelper.Complete();
                    return;
                }

                // Call method to trace the network using the selected feature as the seed feature
                if (!TraceNetwork(fno, fid))
                {
                    CustomCommandHelper.Complete();
                    return;
                }

                // Show results of trace if results contain source features
                if(!ShowSourceFeatureResults(fno))
                {
                    CustomCommandHelper.Complete();
                    return;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(m_Application.ApplicationWindow, "ccEnhancedUpstreamTrace.Activate: " + ex.Message,
                                "G/Technology", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
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

        /// <summary>
        /// Command has terminated. Release objects.
        /// </summary>
        public void Terminate()
        {
            if (m_SourceFNOs != null)
            {
                m_SourceFNOs.Clear();
                m_SourceFNOs = null;
            }
            if (m_SecondaryFNOs != null)
            {
                m_SecondaryFNOs.Clear();
                m_SecondaryFNOs = null;
            }

            m_frmSourceResults.CustomCommandHelper = null;
            m_CustomCommandHelper = null;
            m_Application = null;
        }

        /// <summary>
        /// Validates that the feature has the Connectivity Attributes component.
        /// </summary>
        /// <param name="fno">The G3E_FEATURE.G3E_FNO value to validate</param>
        /// <returns>Boolean indicating status</returns>
        private bool ValidateSeedConnectivity(short fno)
        {
            bool returnValue = false;

            try
            {
                Recordset metadataRS = m_Application.DataContext.MetadataRecordset("G3E_FEATURECOMPS_OPTABLE", "g3e_fno = " + fno + " and g3e_cno = " + CNO_CONNECTIVITY_ATTRIBUTES);
                if (metadataRS.RecordCount > 0)
                {
                    returnValue = true;
                }
                else
                {
                    MessageBox.Show(m_Application.ApplicationWindow, "The selected feature does not participate in electrical connectivity, and therefore cannot be the seed for tracing.",
                                    "G/Technology", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    returnValue = false;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(m_Application.ApplicationWindow, "Error in ValidateSeedConnectivity: " + ex.Message,
                                    "G/Technology", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                
                WriteToCommandLog("ERROR", ex.Message, "ccEnhancedUpstreamTrace.ValidateSeedConnectivity");
                returnValue = false;
            }

            return returnValue;
        }

        /// <summary>
        /// Traces the network from the seed feature.
        /// </summary>
        /// <param name="fno">The G3E_FNO value for the seed feature to use in the trace</param>
        /// <param name="fid">The G3E_FID value for the seed feature to use in the trace</param>
        /// <returns>Boolean indicating status</returns>
        private bool TraceNetwork(short fno, int fid)
        {
            bool returnValue = false;

            try
            {
                TraceHelper traceHelper = new TraceHelper(CUSTOM_COMMAND_NUMBER, CUSTOM_COMMAND_NAME);

                // If the seed feature is a Secondary feature then use the Trace Secondary Actual trace to find the source Transformers
                if (m_SecondaryFNOs.Contains(fno))
                {
                    traceHelper.TraceMetadataUserName = "Trace Secondary Actual";
                }
                else
                // Otherwise, use the Trace Feeder Actual trace to find the source Substation Breakers
                {
                    traceHelper.TraceMetadataUserName = "Trace Feeder Actual";
                }

                // Validate seed feature is valid for the trace
                if (!traceHelper.ValidateSeedFNO(fno))
                {
                    MessageBox.Show(m_Application.ApplicationWindow, string.Format("The selected feature is not configured to be a seed feature for the {0} trace", traceHelper.TraceMetadataUserName),
                                    "G/Technology", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return false;
                }

                // Set trace properties
                traceHelper.ApplicationName = CUSTOM_COMMAND_NAME;
                traceHelper.SeedFID = fid;

                m_Application.BeginWaitCursor();
                m_Application.SetStatusBarText(GTStatusPanelConstants.gtaspcMessage, "Executing trace...");

                // Trace the network
                if (!traceHelper.ExecuteTrace())
                {
                    returnValue = false;
                }
                else
                {
                    m_TraceName = traceHelper.TraceName;
                    returnValue = true;
                }
                m_Application.SetStatusBarText(GTStatusPanelConstants.gtaspcMessage, "");
            }
            catch (Exception ex)
            {
                MessageBox.Show(m_Application.ApplicationWindow, "Error in TraceNetwork: " + ex.Message,
                                    "G/Technology", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);

                WriteToCommandLog("ERROR", ex.Message, "ccEnhancedUpstreamTrace.TraceNetwork");
                returnValue = false;
            }

            m_Application.EndWaitCursor();

            return returnValue;
        }

        /// <summary>
        /// Calls method to show source feature in grid. Informs user if no source features.
        /// </summary>
        /// <param name="fno">The G3E_FNO value for the seed feature to use in the trace</param>
        /// <returns>Boolean indicating status</returns>
        private bool ShowSourceFeatureResults(short fno)
        {
            bool returnValue = false;
            bool sourceFound = false;

            try
            {
                string sql = string.Empty;
                ADODB.Recordset traceResultsRS = null;

                m_Application.SetStatusBarText(GTStatusPanelConstants.gtaspcMessage, "Processing trace results...");

                // Get the trace results. Check for source features. If secondary trace then check for Transformers, otherwise check for Substation Breakers.
                if (m_SecondaryFNOs.Contains(fno))
                {
                    sql = "select distinct conn.g3e_fid, conn.g3e_fno, f.g3e_username, conn.feeder_1_id, conn.network_id " +
                             "from traceresult tr, traceid ti, connectivity_n conn, g3e_features_optable f " +
                             "where ti.g3e_name = ? " +
                             "and ti.g3e_id = tr.g3e_tno " +
                             "and tr.g3e_fno in (?,?,?,?) " +
                             "and tr.g3e_fno = f.g3e_fno " +
                             "and tr.g3e_fid = conn.g3e_fid " +
                             "order by conn.g3e_fid";

                    traceResultsRS = m_Application.DataContext.OpenRecordset(sql, CursorTypeEnum.adOpenDynamic, LockTypeEnum.adLockOptimistic, (int)CommandTypeEnum.adCmdText, m_TraceName,
                                     FNO_TRANSFORMER_OH, FNO_TRANSFORMER_OH_NETWORK, FNO_TRANSFORMER_UG, FNO_TRANSFORMER_UG_NETWORK);

                    if (traceResultsRS.RecordCount > 0)
                    {
                        sourceFound = true;
                    }
                }
                else
                {
                    sql = "select distinct conn.g3e_fid, conn.g3e_fno, f.g3e_username, conn.feeder_1_id, conn.network_id " +
                             "from traceresult tr, traceid ti, connectivity_n conn, g3e_features_optable f " +
                             "where ti.g3e_name = ? " +
                             "and ti.g3e_id = tr.g3e_tno " +
                             "and tr.g3e_fno in (?,?) " +
                             "and tr.g3e_fno = f.g3e_fno " +
                             "and tr.g3e_fid = conn.g3e_fid " +
                             "order by conn.g3e_fid";

                    traceResultsRS = m_Application.DataContext.OpenRecordset(sql, CursorTypeEnum.adOpenDynamic, LockTypeEnum.adLockOptimistic, (int)CommandTypeEnum.adCmdText, m_TraceName,
                                     FNO_SUBSTATION_BREAKER, FNO_SUBSTATION_BREAKER_NETWORK);

                    if (traceResultsRS.RecordCount > 0)
                    {
                        sourceFound = true;
                    }
                }

                m_Application.SetStatusBarText(GTStatusPanelConstants.gtaspcMessage, "");

                // If source features exist, then display in grid
                if (sourceFound)
                {
                    m_frmSourceResults.CustomCommandHelper = m_CustomCommandHelper;
                    m_frmSourceResults.TraceResultsRS = traceResultsRS;
                    m_frmSourceResults.StartPosition = FormStartPosition.CenterScreen;
                    m_frmSourceResults.Show(m_Application.ApplicationWindow);
                    returnValue = true;
                }
                else
                {
                    MessageBox.Show(m_Application.ApplicationWindow, "No source features found in trace results.",
                                    "G/Technology", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    returnValue = false;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(m_Application.ApplicationWindow, "Error in ShowSourceFeatureResults: " + ex.Message,
                                    "G/Technology", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);

                WriteToCommandLog("ERROR", ex.Message, "ccEnhancedUpstreamTrace.ShowSourceFeatureResults");
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
            gtCommandLogger.CommandNum = CUSTOM_COMMAND_NUMBER;
            gtCommandLogger.CommandName = CUSTOM_COMMAND_NAME;
            gtCommandLogger.LogType = logType;
            gtCommandLogger.LogMsg = logMessage;
            gtCommandLogger.LogContext = logContext;
            gtCommandLogger.logEntry();

            gtCommandLogger = null;
        }
    }
}
