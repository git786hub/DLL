using ADODB;
using Intergraph.GTechnology.API;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace GTechnology.Oncor.CustomAPI
{
	/// <summary>
	/// Updates the system connectivity attributes. Call the execute method to run the trace and update.
	/// </summary>
	public class UpdateTrace
	{
        // public properties
        private string m_errorMessage = string.Empty;                      // Set to error message so caller can check this when Execute method returns false.
        public string ErrorMessage { get { return m_errorMessage; } }

        // private classes
        private class TraceStep
        {
            public bool IsValid { get; set; }

            // Fields from TRACERESULTS

            public int FID { get; set; }
            public short FNO { get; set; }
            public int TraceOrder { get; set; }
            public int SourceFID { get; set; }
            public int SourceNode { get; set; }
            public int Node_1 { get; set; }
            public int Node_2 { get; set; }

            public string FeatureState { get; set; }
            public string Phase { get; set; }
            public string StatusNormal { get; set; }
            public string FeederType { get; set; }
            
            public string FeederID_1_Actual { get; set; }
            public string SubstationCode_1_Actual { get; set; }
            public string FeederNumber_1_Actual { get; set; }
            public string Voltage_1_Actual { get; set; }

            public string FeederID_2_Actual { get; set; }
            public string SubstationCode_2_Actual { get; set; }
            public string FeederNumber_2_Actual { get; set; }
            public string Voltage_2_Actual { get; set; }

            public string FeederID_1_PP { get; set; }
            public string SubstationCode_1_PP { get; set; }
            public string FeederNumber_1_PP { get; set; }
            public string Voltage_1_PP { get; set; }

            public string FeederID_2_PP { get; set; }
            public string SubstationCode_2_PP { get; set; }
            public string FeederNumber_2_PP { get; set; }
            public string Voltage_2_PP { get; set; }

            public string NetworkID_Actual { get; set; }
            public string NetworkID_PP { get; set; }
            public int ProtectiveDeviceFID_Actual { get; set; }
            public int ProtectiveDeviceFID_PP { get; set; }
            public int UpstreamProtectionCount_Actual { get; set; }
            public int UpstreamProtectionCount_PP { get; set; }
            public int UpstreamNode_Actual { get; set; }
            public int UpstreamNode_PP { get; set; }

            // Derived logic

            //public int UpstreamNode { get { return (this.SourceNode == 0 || (this.SourceNode == this.Node_1 && this.SourceNode != 0)) ? 1 : 2; } }
            public int UpstreamNode { get { return (this.SourceNode == this.Node_2) ? 2 : 1; } }

            public string FeederID_1 { get { return Coalesce(FeederID_1_PP, FeederID_1_Actual); } }
            public string SubstationCode_1 { get { return Coalesce(SubstationCode_1_PP, SubstationCode_1_Actual); } }
            public string FeederNumber_1 { get { return Coalesce(FeederNumber_1_PP, FeederNumber_1_Actual); } }
            public string Voltage_1 { get { return Coalesce(Voltage_1_PP, Voltage_1_Actual); } }

            public string FeederID_2 { get { return Coalesce(FeederID_2_PP, FeederID_2_Actual); } }
            public string SubstationCode_2 { get { return Coalesce(SubstationCode_2_PP, SubstationCode_2_Actual); } }
            public string FeederNumber_2 { get { return Coalesce(FeederNumber_2_PP, FeederNumber_2_Actual); } }
            public string Voltage_2 { get { return Coalesce(Voltage_2_PP, Voltage_2_Actual); } }

            public string NetworkID { get { return Coalesce(NetworkID_PP, NetworkID_Actual); } }
            public int ProtectiveDeviceFID { get { return Coalesce(ProtectiveDeviceFID_PP, ProtectiveDeviceFID_Actual); } }
            public int UpstreamProtectionCount { get { return Coalesce(UpstreamProtectionCount_PP, UpstreamProtectionCount_Actual); } }

            public string DownstreamFeederID { get {
                    if (this.FNO == 16 || this.FNO == 91) return this.FeederID_1;
                    return (this.UpstreamNode == 1) ? this.FeederID_2 : this.FeederID_1;
                } }
            public string DownstreamSubstationCode { get {
                    if (this.FNO == 16 || this.FNO == 91) return this.SubstationCode_1;
                    return (this.UpstreamNode == 1) ? this.SubstationCode_2 : this.SubstationCode_1;
                } }
            public string DownstreamFeederNumber { get {
                    if (this.FNO == 16 || this.FNO == 91) return this.FeederNumber_1;
                    return (this.UpstreamNode == 1) ? this.FeederNumber_2 : this.FeederNumber_1;
                } }
            public string DownstreamVoltage { get {
                    if (this.FNO == 16 || this.FNO == 91) return this.Voltage_1;
                    return (this.UpstreamNode == 1) ? this.Voltage_2 : this.Voltage_1;
                } }

            public bool IsNetworkFeature { get { return (this.FeederType.ToUpper().Equals("NETWORK")); } }

            public TraceStep(ADODB.Recordset p_rs, Int32 p_FID)
            {
                this.IsValid = false;
                if (p_rs == null || (p_rs.BOF && p_rs.EOF)) return;

                p_rs.Filter = "g3e_fid = " + p_FID;
                p_rs.MoveFirst();

                this.FID = p_FID;
                this.FNO = Convert.ToInt16(p_rs.Fields["G3E_FNO"].Value);
                this.TraceOrder = Convert.ToInt32(p_rs.Fields["G3E_TRACEORDER"].Value);
                this.SourceNode = Convert.ToInt32(p_rs.Fields["G3E_SOURCENODE"].Value);
                this.Node_1 = Convert.ToInt32(p_rs.Fields["G3E_NODE1"].Value);
                this.Node_2 = Convert.ToInt32(p_rs.Fields["G3E_NODE2"].Value);

                this.FeatureState = p_rs.Fields["FEATURE_STATE_C"].Value.ToString();
                this.Phase = p_rs.Fields["PHASE_ALPHA"].Value.ToString();
                this.StatusNormal = p_rs.Fields["STATUS_NORMAL_C"].Value.ToString();
                this.FeederType = p_rs.Fields["FEEDER_TYPE_C"].Value.ToString();

                this.FeederID_1_Actual = p_rs.Fields["FEEDER_1_ID"].Value.ToString();
                this.SubstationCode_1_Actual = p_rs.Fields["SSTA_C"].Value.ToString();
                this.FeederNumber_1_Actual = p_rs.Fields["FEEDER_NBR"].Value.ToString();
                this.Voltage_1_Actual = p_rs.Fields["VOLT_1_Q"].Value.ToString();

                this.FeederID_2_Actual = p_rs.Fields["FEEDER_2_ID"].Value.ToString();
                this.SubstationCode_2_Actual = p_rs.Fields["TIE_SSTA_C"].Value.ToString();
                this.FeederNumber_2_Actual = p_rs.Fields["TIE_FEEDER_NBR"].Value.ToString();
                this.Voltage_2_Actual = p_rs.Fields["VOLT_2_Q"].Value.ToString();

                this.FeederID_1_PP = p_rs.Fields["PP_FEEDER_1_ID"].Value.ToString();
                this.SubstationCode_1_PP = p_rs.Fields["PP_SSTA_C"].Value.ToString();
                this.FeederNumber_1_PP = p_rs.Fields["PP_FEEDER_NBR"].Value.ToString();
                this.Voltage_1_PP = p_rs.Fields["PP_VOLT_1_Q"].Value.ToString();

                this.FeederID_2_PP = p_rs.Fields["PP_FEEDER_2_ID"].Value.ToString();
                this.SubstationCode_2_PP = p_rs.Fields["PP_TIE_SSTA_C"].Value.ToString();
                this.FeederNumber_2_PP = p_rs.Fields["PP_TIE_FEEDER_NBR"].Value.ToString();
                this.Voltage_2_PP = p_rs.Fields["PP_VOLT_2_Q"].Value.ToString();

                this.NetworkID_Actual = p_rs.Fields["NETWORK_ID"].Value.ToString();
                this.NetworkID_PP = p_rs.Fields["PP_NETWORK_ID"].Value.ToString();

                this.ProtectiveDeviceFID_Actual = 0;
                if (!Convert.IsDBNull(p_rs.Fields["PROTECTIVE_DEVICE_FID"].Value))
                {
                    this.ProtectiveDeviceFID_Actual = Convert.ToInt32(p_rs.Fields["PROTECTIVE_DEVICE_FID"].Value);
                }
                this.ProtectiveDeviceFID_PP = 0;
                if (!Convert.IsDBNull(p_rs.Fields["PP_PROTECTIVE_DEVICE_FID"].Value))
                {
                    this.ProtectiveDeviceFID_PP = Convert.ToInt32(p_rs.Fields["PP_PROTECTIVE_DEVICE_FID"].Value);
                }

                this.UpstreamProtectionCount_Actual = 0;
                if (!Convert.IsDBNull(p_rs.Fields["UPSTREAM_PROTDEV_Q"].Value))
                {
                    this.UpstreamProtectionCount_Actual = Convert.ToInt16(p_rs.Fields["UPSTREAM_PROTDEV_Q"].Value);
                }
                this.UpstreamProtectionCount_PP = 0;
                if (!Convert.IsDBNull(p_rs.Fields["PP_UPSTREAM_PROTDEV_Q"].Value))
                {
                    this.UpstreamProtectionCount_PP = Convert.ToInt16(p_rs.Fields["PP_UPSTREAM_PROTDEV_Q"].Value);
                }

                this.UpstreamNode_Actual = 0;
                if (!Convert.IsDBNull(p_rs.Fields["UPSTREAM_NODE"].Value))
                {
                    this.UpstreamNode_Actual = Convert.ToInt16(p_rs.Fields["UPSTREAM_NODE"].Value);
                }
                this.UpstreamNode_PP = 0;
                if (!Convert.IsDBNull(p_rs.Fields["PP_UPSTREAM_NODE"].Value))
                {
                    this.UpstreamNode_PP = Convert.ToInt16(p_rs.Fields["PP_UPSTREAM_NODE"].Value);
                }

                // TODO: where/why should this be set to 1?
                //if (this.UpstreamProtectionCount == 0)
                //{
                //    this.UpstreamProtectionCount = 1;
                //}

                p_rs.Filter = ADODB.FilterGroupEnum.adFilterNone;
                this.IsValid = true;
            }

            private string Coalesce(string option1, string option2)
            {
                string retVal = option1;
                if (string.IsNullOrEmpty(retVal)) { retVal = option2; }
                return retVal;
            }

            private int Coalesce(int option1, int option2)
            {
                int retVal = option1;
                if (retVal == 0) { retVal = option2; }
                return retVal;
            }
            private short Coalesce(short option1, short option2)
            {
                short retVal = option1;
                if (retVal == 0) { retVal = option2; }
                return retVal;
            }
        }

        // private properties
        private int SeedFID { get; set; }
        private short SeedFNO { get; set; }

        // module-level variables
        private IGTApplication m_Application = GTClassFactory.Create<IGTApplication>();
		private IGTDataContext m_DataContext;

		private TraceHelper m_TraceHelper;                                 // Class to setup, run, and display trace
		private ADODB.Recordset m_TraceResultsRS;                          // Recordset for trace results and connectivity attributes
		private ADODB.Recordset m_VisitedRS;                               // Recordset to track visited FIDs. Used to display trace.

		private const short FNO_SUB_BREAKER = 16;
		private const short FNO_SUB_BREAKER_NETWORK = 91;
		private const short FNO_AUTO_TRANSFORMER = 34;
		private const short FNO_PRIMARY_CONDUCTOR_OH = 8;
		private const short FNO_PRIMARY_CONDUCTOR_UG = 9;
		private const short FNO_PRIMARY_CONDUCTOR_OH_NETWORK = 84;
		private const short FNO_PRIMARY_CONDUCTOR_UG_NETWORK = 85;
		private const short FNO_TRANSFORMER_OH = 59;
		private const short FNO_TRANSFORMER_UG = 60;
		private const short FNO_TRANSFORMER_OH_NETWORK = 98;
		private const short FNO_TRANSFORMER_UG_NETWORK = 99;

		private bool m_InteractiveMode;                                    // Flag to indicate batch versus interactive.
		private bool m_PrimaryTrace = true;                                // Flag to indicate if primary or secondary was traced. TRUE if primary was traced.
		private bool m_ActualTrace = true;                                 // Flag to indicate if actual or proposed trace. TRUE if actual trace.

		private int m_CommandNumber;                                       // Custom command number used for logging.
		private string m_CommandName;                                      // Custom command name used for logging.

		private string m_TraceType = string.Empty;                         // Type of trace to run; Trace Primary/Secondary or Trace Primary/Secondary Proposed.
		private string m_JobType = string.Empty;                           // G3E_JOB.G3E_JOBTYPE
		private string m_JobStatus = string.Empty;                         // G3E_JOB.G3E_JOBSTATUS for active job.

		private List<int> m_VisitedFIDs;                                   // List of FIDs that have been processed.
		private List<short> m_ProtectiveDeviceFNOs = new List<short>(new short[] { 16, 91, 11, 87, 38, 88, 14, 15, 59, 98, 60, 99 });  // List of Protective Device G3E_FNOs.
		private List<short> m_SourceFNOs = new List<short>(new short[] { 16, 91, 59, 98, 60, 99 });                    // List of feeding features.
		private List<short> m_SecondaryFNOs = new List<short>(new short[] { 23, 52, 53, 54, 55, 59, 60, 63, 86, 94, 95, 96, 97, 98, 99, 154, 155, 161, 162 });     // List of Secondary features.

		private List<string> m_LegendItems;                                // List of items that this module has added to the legend. Used for cleanup.

        private struct structProtectiveDevice
        {
            internal int fid;
            internal string phase;
            internal string state;
            internal int upstreamProtectionCount;
        }

        private structProtectiveDevice m_ProtectiveDevice;
        private List<structProtectiveDevice> m_ProtectiveDevices;              // Keeps track of upstream protective devices.

        private Dictionary<int, int> m_TieTransformers = new Dictionary<int, int>(); // Contains Transformers that have associated Tie Transformers.

		private Dictionary<int, Dictionary<string, string>> m_AttributeUpdates = new Dictionary<int, Dictionary<string, string>>(); // Keeps track of feature and attributes to update.

		private struct structCommandMetadata
		{
			internal object TraceHighlightColor;
			internal object TraceHighlightFillColor;
			internal object TraceHighlightWidth;

			internal object UpdateHighlightColor;
			internal object UpdateHighlightFillColor;
			internal object UpdateHighlightWidth;

			internal object NonJobHighlightColor;
			internal object NonJobHighlightFillColor;
			internal object NonJobHighlightWidth;
		}

		private structCommandMetadata m_CommandMetadata;                       // Used for style properties of the trace legend items.

        private enum AttributesToUpdate
        {
            Neither = 0,
            Both = 1,
            Proposed = 2,
            Actual = 3,
            ActualNullProposed = 4,
            NullProposed = 5,
            Error = -1
        }

		/// <summary>
		/// Constructor.
		/// </summary>
		/// <param name="commandNumber">G3E_CUSTOMCOMMAND.G3E_CCNO. Used for logging.</param>
		/// <param name="commandName">G3E_CUSTOMCOMMAND.G3E_USERNAME. Used for logging.</param>
		/// <returns>Boolean indicating status</returns>
		public UpdateTrace(int commandNumber, string commandName)
		{
			m_CommandNumber = commandNumber;
			m_CommandName = commandName;
		}

		/// <summary>
		/// The entry point for this class.
		/// </summary>
		/// <param name="p_seedFNO">G3E_FNO of the seed feature</param>
		/// <param name="seedFID">G3E_FID of the seed feature</param>
		/// <returns>Boolean indicating status</returns>
		public bool Execute(short p_seedFNO, int p_seedFID)
		{
			bool returnValue = false;
			int traceOrder = 2;

			try
			{
				// If G/Tech is not running in interactive mode, then skip message boxes.
				GUIMode guiMode = new GUIMode();
				m_InteractiveMode = guiMode.InteractiveMode;

				m_DataContext = m_Application.DataContext;
                SeedFID = p_seedFID;
                SeedFNO = p_seedFNO;

				// Get the metadata for Update Trace
				if(!GetMetadataParameters())
				{
					CleanUp();
					return false;
				}

				// Determine which trace to run
				if(!GetTraceType())
				{
					CleanUp();
					return false;
				}

				// Initialize trace object. 
				m_TraceHelper = new TraceHelper(m_CommandNumber, m_CommandName);
				m_TraceHelper.ApplicationName = "Update Trace";
				m_TraceHelper.TraceMetadataUserName = m_TraceType;

				// Validate select set. Check if valid seed feature
				if(!m_TraceHelper.ValidateSeedFNO(SeedFNO))
				{
					if(m_InteractiveMode)
					{
						MessageBox.Show(m_Application.ApplicationWindow, "The selected feature does not participate in electrical connectivity, and therefore cannot be the seed for tracing.",
																								"G/Technology", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
					}
					else
					{
						WriteToCommandLog("INFO", "The selected feature does not participate in electrical connectivity, and therefore cannot be the seed for tracing.", "commonUpdateTrace.Execute");
					}
					CleanUp();
					return false;
				}

				int hintFID = -1;
				// If the seed feature is a Substation Breaker or Transformer, 
				// assume this feature is the source and only trace in the direction of node 2.
				if(m_SourceFNOs.Contains(SeedFNO))
				{
					// Call method to determine the direction to trace by getting the G3E_FID at node 2 of the source.
					if(!GetTraceHint(SeedFNO, SeedFID, ref hintFID))
					{
						CleanUp();
						return false;
					}
				}

				m_TraceHelper.SeedFID = SeedFID;
				m_TraceHelper.HintFID = hintFID;
				m_TraceHelper.HintIdentifiesFeature = "N";

				m_Application.SetStatusBarText(GTStatusPanelConstants.gtaspcMessage, "Executing trace...");
				if(!m_TraceHelper.ExecuteTrace())
				{
					CleanUp();
					return false;
				}
				m_Application.SetStatusBarText(GTStatusPanelConstants.gtaspcMessage, "");

				// Query the trace results and join with the connectivity record
				m_Application.SetStatusBarText(GTStatusPanelConstants.gtaspcMessage, "Processing trace results...");
				if(!GetTraceResults(m_TraceHelper.TraceName))
				{
					CleanUp();
					return false;
				}

				// Determine if primary or secondary trace
				if(m_TraceType.ToUpper().Contains("FEEDER"))
				{
					m_PrimaryTrace = true;
					// Process the primary trace
					if(!ProcessPrimaryTrace(ref traceOrder))
					{
						CleanUp();
						return false;
					}
				}
				else
				{
					m_PrimaryTrace = false;
					// Process the secondary trace
					if(!ProcessSecondaryTrace(ref traceOrder))
					{
						CleanUp();
						return false;
					}
				}

                // Set the tracking variables
                TraceStep stepSeed = new TraceStep(m_TraceResultsRS, SeedFID);
                if (stepSeed == null)
                {
                    CleanUp();
                    return false;
                }
                InitializeProtectiveDeviceList(stepSeed);
                InitializeVisitedList(stepSeed);

                if (stepSeed.FNO == FNO_SUB_BREAKER || stepSeed.FNO == FNO_SUB_BREAKER_NETWORK)
                {
                    SetUpstreamForSubstationBreaker();
                }

				// Walk the trace results tree
				if(!ProcessTraceResults(traceOrder, stepSeed))
				{
					CleanUp();
					return false;
				}

				if(m_AttributeUpdates.Count == 0)
				{
					if(m_InteractiveMode)
					{
						MessageBox.Show(m_Application.ApplicationWindow, "All traced features are in agreement; no updates necessary.",
														"G/Technology", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
					}

					CleanUp();
					return true;
				}

				// Save the current map window location for reset at end.
				IGTWorldRange gtWorldRange = null;
				// Range used to fit updates in map window.
				IGTWorldRange gtUpdateWorldRange = null;

				if(m_InteractiveMode)
				{
					gtWorldRange = m_Application.ActiveMapWindow.GetRange();
					gtUpdateWorldRange = m_Application.ActiveMapWindow.GetRange();

					m_Application.SetStatusBarText(GTStatusPanelConstants.gtaspcMessage, "Displaying results...");
					if(!DisplayResults(ref gtUpdateWorldRange))
					{
						CleanUp();
						return false;
					}
				}

				// Update the connectivity record
				if(m_AttributeUpdates.Count > 0)
				{
					DialogResult dialogResult = DialogResult.Yes;

					if(m_InteractiveMode)
					{
						m_Application.ActiveMapWindow.ZoomArea(gtUpdateWorldRange);
						m_Application.RefreshWindows();

						dialogResult = MessageBox.Show(m_Application.ApplicationWindow, "Update highlighted features? [Y/N]",
												"G/Technology", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation);
					}

					if(dialogResult == DialogResult.Yes)
					{
						m_Application.SetStatusBarText(GTStatusPanelConstants.gtaspcMessage, "Updating connectivity attributes...");
						if(!UpdateConnectivityAttributes())
						{
							CleanUp();
							return false;
						}
					}

					if(m_InteractiveMode)
					{
						// Reset map window to initial location
						m_Application.ActiveMapWindow.ZoomArea(gtWorldRange);
						m_Application.RefreshWindows();
					}
				}
				else
				{
					if(m_InteractiveMode)
					{
						MessageBox.Show(m_Application.ApplicationWindow, "All traced features are in agreement; no updates necessary.",
														"G/Technology", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
					}
				}

				// Remove trace from the database trace tables - traceid, tracelog, traceresult
//				m_TraceHelper.RemoveTrace(m_TraceHelper.TraceName, "", "");

				// Remove the trace and update entries from the legend
				// These should only be executed when not in batch mode
				if(m_InteractiveMode)
				{
					foreach(string legendItem in m_LegendItems)
					{
						m_TraceHelper.RemoveTraceLegendItem("Traces", legendItem);
					}
				}

				m_Application.SetStatusBarText(GTStatusPanelConstants.gtaspcMessage, "Update trace complete.");

				returnValue = true;
			}
			catch(Exception ex)
			{
				if(m_InteractiveMode)
				{
					MessageBox.Show(m_Application.ApplicationWindow, "Error in Execute: " + ex.Message,
													"G/Technology", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
				}
				WriteToCommandLog("ERROR", ex.Message, "commonUpdateTrace.Execute");
				returnValue = false;
			}

			CleanUp();
			return returnValue;
		}

		/// <summary>
		/// Free the resources.
		/// </summary>
		private void CleanUp()
		{
			try
			{
				if(m_AttributeUpdates != null)
				{
					m_AttributeUpdates.Clear();
					m_AttributeUpdates = null;
				}
				if(m_TieTransformers != null)
				{
					m_TieTransformers.Clear();
					m_TieTransformers = null;
				}
				if(m_ProtectiveDevices != null)
				{
					m_ProtectiveDevices.Clear();
					m_ProtectiveDevices = null;
				}
				if(m_LegendItems != null)
				{
					m_LegendItems.Clear();
					m_LegendItems = null;
				}
				if(m_SourceFNOs != null)
				{
					m_SourceFNOs.Clear();
					m_SourceFNOs = null;
				}
				if(m_SecondaryFNOs != null)
				{
					m_SecondaryFNOs.Clear();
					m_SecondaryFNOs = null;
				}
				if(m_ProtectiveDeviceFNOs != null)
				{
					m_ProtectiveDeviceFNOs.Clear();
					m_ProtectiveDeviceFNOs = null;
				}
				if(m_VisitedFIDs != null)
				{
					m_VisitedFIDs.Clear();
					m_VisitedFIDs = null;
				}
				if(m_VisitedRS != null)
				{
					m_VisitedRS.Close();
					m_VisitedRS = null;
				}
				if(m_TraceResultsRS != null)
				{
					m_TraceResultsRS.Close();
					m_TraceResultsRS = null;
				}

				m_TraceHelper = null;
			}
			catch(Exception ex)
			{
				WriteToCommandLog("ERROR", ex.Message, "commonUpdateTrace.CleanUp");
			}

		}

		/// <summary>
		/// Processes the results for a primary trace.
		/// </summary>
		/// <param name="traceOrder">Depth to start the trace processing</param>
		/// <returns>Boolean indicating status</returns>
		private bool ProcessPrimaryTrace(ref int traceOrder)
		{
			bool returnValue = false;

			try
			{
				// Validate that one and only one Substation Breaker exists in the trace results.
				m_TraceResultsRS.Filter = "g3e_fno = " + FNO_SUB_BREAKER + " or g3e_fno = " + FNO_SUB_BREAKER_NETWORK;
				if(m_TraceResultsRS.RecordCount > 1)
				{
					// More than one Substation Breaker was encountered, notify user and exit.
					this.m_errorMessage = "The trace found more than one Substation Breaker.";

					if(m_InteractiveMode)
					{
						MessageBox.Show(m_Application.ApplicationWindow, string.Format("{0}  The command will exit.", this.m_errorMessage),
														"G/Technology", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
					}
					else
					{
						WriteToCommandLog("INFO", this.m_errorMessage, "commonUpdateTrace.ProcessPrimaryTrace");
					}

					return false;
				}
				else if(m_TraceResultsRS.RecordCount == 0)
				{
					// No Substation Breaker was encountered, notify user and exit.
					if(m_InteractiveMode)
					{
						MessageBox.Show(m_Application.ApplicationWindow, "The trace did not find a Substation Breaker. The command will exit.",
														"G/Technology", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
					}
					else
					{
						WriteToCommandLog("INFO", "The trace did not find a Substation Breaker", "commonUpdateTrace.ProcessPrimaryTrace");
					}
					return false;
				}
				m_TraceResultsRS.Filter = "";

				if(!m_SourceFNOs.Contains(SeedFNO))
				{
					// If selected feature isn't a Substation Breaker, then rerun the trace from the Substation Breaker to determine downstream direction.
					m_TraceResultsRS.Filter = "g3e_fno = " + FNO_SUB_BREAKER + " or g3e_fno = " + FNO_SUB_BREAKER_NETWORK;

					// Rerun trace from Substation Breaker
					short tempSeedFNO = Convert.ToInt16(m_TraceResultsRS.Fields["G3E_FNO"].Value);
					int tempSeedFID = Convert.ToInt32(m_TraceResultsRS.Fields["G3E_FID"].Value);

					// Call method to determine the direction to trace
					int hintFID = -1;
					if(!GetTraceHint(tempSeedFNO, tempSeedFID, ref hintFID))
					{
						return false;
					}

					TraceHelper m_TraceHelperSub = new TraceHelper(m_CommandNumber, m_CommandName);

					m_TraceHelperSub.ApplicationName = "Update Trace";
					m_TraceHelperSub.TraceMetadataUserName = m_TraceType;

					m_TraceHelperSub.SeedFID = tempSeedFID;
					m_TraceHelperSub.HintFID = hintFID;
					m_Application.SetStatusBarText(GTStatusPanelConstants.gtaspcMessage, "Executing source trace...");
					if(!m_TraceHelperSub.ExecuteTrace())
					{
						return false;
					}

					// Query the trace results and join with the connectivity record
					m_Application.SetStatusBarText(GTStatusPanelConstants.gtaspcMessage, "Processing trace results...");
					if(!GetTraceResults(m_TraceHelperSub.TraceName))
					{
						return false;
					}

					m_TraceHelperSub.RemoveTrace(m_TraceHelperSub.TraceName, "", "");

					// Prompt user if updates should be done from selected feature or located Substation Breaker.                            
					if(m_InteractiveMode)
					{
						DialogResult dialogResult = MessageBox.Show(m_Application.ApplicationWindow, "Update feeder from the Substation Breaker instead of selected feature? [Y/N]",
														"G/Technology", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation);

						// If Yes, set the discovered Substation Breaker as the new seed feature.
						if(dialogResult == DialogResult.Yes)
						{
							SeedFNO = tempSeedFNO;
							SeedFID = tempSeedFID;
							traceOrder = 2;
						}
						else
						{
							// Only process the results downstream from the selected feature.
							m_TraceResultsRS.Filter = "g3e_fid = " + SeedFID;
							if(m_TraceResultsRS.RecordCount > 0)
							{
								traceOrder = Convert.ToInt32(m_TraceResultsRS.Fields["G3E_TRACEORDER"].Value) + 1;
								m_TraceResultsRS.Filter = "";
							}
							else
							{
								if(m_InteractiveMode)
								{
									MessageBox.Show(m_Application.ApplicationWindow, "Features connected to the downstream side of the source feature are not valid for this type of trace",
																	"G/Technology", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
								}
								WriteToCommandLog("ERROR", "Features connected to the downstream side of the source feature are not valid for this type of trace", "commonUpdateTrace.ProcessPrimaryTrace");
								return false;
							}
						}
					}
					else
					{
						SeedFNO = tempSeedFNO;
						SeedFID = tempSeedFID;
						traceOrder = 2;
					}
				}

				returnValue = true;
			}
			catch(Exception ex)
			{
				if(m_InteractiveMode)
				{
					MessageBox.Show(m_Application.ApplicationWindow, "Error in ProcessPrimaryTrace: " + ex.Message,
													"G/Technology", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
				}
				WriteToCommandLog("ERROR", ex.Message, "commonUpdateTrace.ProcessPrimaryTrace");
				returnValue = false;
			}
			return returnValue;
		}

		/// <summary>
		/// Processes the results for a secondary trace.
		/// </summary>
		/// <param name="traceOrder">Depth to start the trace processing</param>
		/// <returns>Boolean indicating status</returns>
		private bool ProcessSecondaryTrace(ref int traceOrder)
		{
			bool returnValue = false;

			try
			{
				// Add Transformer OH and UG to list of protective devices
				m_ProtectiveDeviceFNOs.Add(FNO_TRANSFORMER_OH);
				m_ProtectiveDeviceFNOs.Add(FNO_TRANSFORMER_UG);
				m_ProtectiveDeviceFNOs.Add(FNO_TRANSFORMER_OH_NETWORK);
				m_ProtectiveDeviceFNOs.Add(FNO_TRANSFORMER_UG_NETWORK);

				m_TraceResultsRS.Filter = "g3e_fno = " + FNO_TRANSFORMER_OH + " or g3e_fno = " + FNO_TRANSFORMER_OH_NETWORK +
																	" or g3e_fno = " + FNO_TRANSFORMER_UG + " or g3e_fno = " + FNO_TRANSFORMER_UG_NETWORK;

				// If no Transformer was encountered, notify user and exit.
				if(m_TraceResultsRS.RecordCount == 0)
				{
					this.m_errorMessage = "No Transformer found in Secondary Trace results.";

					if(m_InteractiveMode)
					{
						MessageBox.Show(m_Application.ApplicationWindow, this.m_errorMessage,
														"G/Technology", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
					}
					else
					{
						WriteToCommandLog("INFO", this.m_errorMessage, "commonUpdateTrace.ProcessSecondaryTrace");
					}

					returnValue = false;
				}
				else
				{
					m_TraceResultsRS.MoveFirst();
					returnValue = true;
					short fno;
					int fid;

					if(m_TraceResultsRS.RecordCount > 1)
					{
						// If more than one Transformer was encountered, check whether all Transformers share the same Tie Transformer ID.  
						// If so, then treat the identified Transformer as the starting point; if not, notify user and exit.
						int xfmrTieID = 0;
						int startingXfmrTieID = -1;
						short cno = 0;

						IGTKeyObject xfmrKO = null;
						ADODB.Recordset xfmrRS = null;

						while(!m_TraceResultsRS.EOF)
						{
							fno = Convert.ToInt16(m_TraceResultsRS.Fields["G3E_FNO"].Value);
							fid = Convert.ToInt32(m_TraceResultsRS.Fields["G3E_FID"].Value);

							if(fno == FNO_TRANSFORMER_OH || fno == FNO_TRANSFORMER_OH_NETWORK)
							{
								cno = 5901;
							}
							else
							{
								cno = 6002;
							}

							xfmrKO = m_DataContext.OpenFeature(fno, fid);
							xfmrRS = xfmrKO.Components.GetComponent(cno).Recordset;

							if(!Convert.IsDBNull(xfmrRS.Fields["TIE_XFMR_ID"].Value))
							{
								xfmrTieID = Convert.ToInt32(xfmrRS.Fields["TIE_XFMR_ID"].Value);
								if(startingXfmrTieID == -1)
								{
									startingXfmrTieID = xfmrTieID;
								}
							}

							if(xfmrTieID != startingXfmrTieID)
							{
								if(m_InteractiveMode)
								{
									MessageBox.Show(m_Application.ApplicationWindow, "Multiple Transformers found in Secondary Trace with different Tie Transformer IDs",
																	"G/Technology", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
								}
								else
								{
									WriteToCommandLog("INFO", "Multiple Transformers found in Secondary Trace with different Tie Transformer IDs", "commonUpdateTrace.ProcessSecondaryTrace");
								}
								returnValue = false;
								break;
							}

							xfmrTieID = 0;

							m_TraceResultsRS.MoveNext();
						}
					}

					if(returnValue && !(SeedFNO == FNO_TRANSFORMER_OH || SeedFNO == FNO_TRANSFORMER_OH_NETWORK 
                                    || SeedFNO == FNO_TRANSFORMER_UG || SeedFNO == FNO_TRANSFORMER_UG_NETWORK))
					{
						// If seed fid isn't a transformer, then rerun trace at transformer
						m_TraceResultsRS.MoveFirst();
						fno = Convert.ToInt16(m_TraceResultsRS.Fields["G3E_FNO"].Value);
						fid = Convert.ToInt32(m_TraceResultsRS.Fields["G3E_FID"].Value);

						// Call method to determine the direction to trace
						int hintFID = -1;
						if(!GetTraceHint(fno, fid, ref hintFID))
						{
							return false;
						}

						TraceHelper m_TraceHelperXfmr = new TraceHelper(m_CommandNumber, m_CommandName);

						m_TraceHelperXfmr.ApplicationName = "Update Trace";
						m_TraceHelperXfmr.TraceMetadataUserName = m_TraceType;

						m_TraceHelperXfmr.SeedFID = fid;
						m_TraceHelperXfmr.HintFID = hintFID;
						m_Application.SetStatusBarText(GTStatusPanelConstants.gtaspcMessage, "Executing source trace...");
						if(!m_TraceHelperXfmr.ExecuteTrace())
						{
							return false;
						}

						// Query the trace results and join with the connectivity record
						m_Application.SetStatusBarText(GTStatusPanelConstants.gtaspcMessage, "Processing trace results...");
						if(!GetTraceResults(m_TraceHelperXfmr.TraceName))
						{
							return false;
						}

						m_TraceHelperXfmr.RemoveTrace(m_TraceHelperXfmr.TraceName, "", "");

                        SeedFNO = fno;
                        SeedFID = fid;
						traceOrder = 2;

						returnValue = true;
					}
				}
			}
			catch(Exception ex)
			{
				if(m_InteractiveMode)
				{
					MessageBox.Show(m_Application.ApplicationWindow, "Error in ProcessSecondaryTrace: " + ex.Message,
													"G/Technology", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
				}
				WriteToCommandLog("ERROR", ex.Message, "commonUpdateTrace.ProcessSecondaryTrace");
				returnValue = false;
			}

			m_TraceResultsRS.Filter = "";
			return returnValue;
		}

		/// <summary>
		/// Determines the type of trace to run.
		/// </summary>
		/// <returns>Boolean indicating status</returns>
		private bool GetTraceType()
		{
			bool returnValue = false;

			if(null == m_DataContext || string.IsNullOrEmpty(m_DataContext.ActiveJob))
			{
				if(m_InteractiveMode)
				{
					MessageBox.Show(m_Application.ApplicationWindow, "Error in GetTraceType: Unable to determine active job.", "G/Technology", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
				}

				WriteToCommandLog("ERROR", "Unable to determine active job.  Trace not run.", "commonUpdateTrace.GetTraceType");
				return returnValue;
			}

			try
			{
				// If the active job is a WR with Job Status = ConstructionComplete, or if it is a GIS Maintenance job, 
				// then use one of the two Actual traces. Otherwise, use one of the two Proposed traces.
				string sql = "select g3e_jobstatus, g3e_jobtype from g3e_job where g3e_identifier = ? and g3e_jobstatus is not null and g3e_jobtype is not null";

				ADODB.Recordset jobRS = m_DataContext.OpenRecordset(sql, CursorTypeEnum.adOpenDynamic, LockTypeEnum.adLockOptimistic, (int)CommandTypeEnum.adCmdText, m_DataContext.ActiveJob);

				if(null != jobRS && jobRS.RecordCount > 0)
				{
					m_JobStatus = jobRS.Fields["G3E_JOBSTATUS"].Value.ToString();
					m_JobType = jobRS.Fields["G3E_JOBTYPE"].Value.ToString();

					if(m_JobStatus == "ConstructionComplete" || m_JobType == "NON-WR" || m_JobType == "WR-MAPCOR")
					{
						// If the seed feature is a Secondary feature then use the Trace Secondary Actual trace
						if(m_SecondaryFNOs.Contains(SeedFNO))
						{
							m_TraceType = "Trace Secondary Actual";
						}
						else
						// Otherwise, use the Trace Feeder Actual trace
						{
							m_TraceType = "Trace Feeder Actual";
						}
					}
					else
					{
						m_ActualTrace = false;

						// If the seed feature is a Secondary feature then use the Trace Secondary Proposed trace
						if(m_SecondaryFNOs.Contains(SeedFNO))
						{
							m_TraceType = "Trace Secondary Proposed";
						}
						else
						// Otherwise, use the Trace Feeder Proposed trace
						{
							m_TraceType = "Trace Feeder Proposed";
						}
					}
					returnValue = true;
				}

			}
			catch(Exception ex)
			{
				if(m_InteractiveMode)
				{
					MessageBox.Show(m_Application.ApplicationWindow, "Error in GetTraceType: " + ex.Message, "G/Technology", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
				}
				WriteToCommandLog("ERROR", ex.Message, "commonUpdateTrace.GetTraceType");
				returnValue = false;
			}
			return returnValue;
		}

		/// <summary>
		/// Get the feature connected to Node 2 of the seed feature.
		/// </summary>
		/// <param name="seedFNO">G3E_FNO of the seed feature</param>
		/// <param name="seedFID">G3E_FID of the seed feature</param>
		/// <param name="traceHint">G3E_FID connected to Node 2. -1 if no Node 2 connection</param>
		/// <returns>Boolean indicating status</returns>
		private bool GetTraceHint(short seedFNO, int seedFID, ref int traceHint)
		{
			bool returnValue = false;

			try
			{
				traceHint = -1;
				IGTKeyObject seedKO = m_DataContext.OpenFeature(seedFNO, seedFID);

				short rno = m_TraceHelper.RNO;

				// Get the FID connected to Node 2 of the seed feature
				IGTRelationshipService relationshipSrvc = GTClassFactory.Create<IGTRelationshipService>();
				relationshipSrvc.DataContext = m_DataContext;
				relationshipSrvc.ActiveFeature = seedKO;
				IGTKeyObjects relatedKO = relationshipSrvc.GetRelatedFeatures(rno, GTRelationshipOrdinalConstants.gtrelRelationshipOrdinal2);

				if(relatedKO.Count > 0)
				{
					traceHint = relatedKO[0].FID;
				}

				relationshipSrvc.Dispose();
				seedKO = null;

				returnValue = true;
			}
			catch(Exception ex)
			{
				if(m_InteractiveMode)
				{
					MessageBox.Show(m_Application.ApplicationWindow, "Error in GetTraceHint: " + ex.Message,
													"G/Technology", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
				}
				WriteToCommandLog("ERROR", ex.Message, "commonUpdateTrace.GetTraceHint");
				returnValue = false;
			}
			return returnValue;
		}

		/// <summary>
		/// Determines if the actual and/or proposed attribute value needs to be updated.
		/// </summary>
		/// <param name="fid">The g3e_fid of the feature to potentially update</param>
		/// <param name="fno">The g3e_fno of the feature to potentially update</param>
		/// <param name="actualAttribute">The field name of the actual connectivity attribute</param>
		/// <param name="actualValue">The current value of the actual connectivity attribute</param>
		/// <param name="proposedAttribute">The field name of the proposed connectivity attribute</param>
		/// <param name="proposedValue">The current value of the proposed connectivity attribute</param>
		/// <param name="newValue">The value to use for the update</param>
		/// <param name="IsNetworkFeature">Indicates if the feeder type is a NETWORK</param>
		/// <returns>Boolean indicating status</returns>
		private AttributesToUpdate DetermineAttributeToUpdate(int fid, string newValue, string actualAttribute, string actualValue, string proposedAttribute, string proposedValue)
		{
            AttributesToUpdate retVal = AttributesToUpdate.Neither;
            bool updateActual = false;
            bool updateProposed = false;
            bool nullProposed = false;

			try
			{
                bool actualDiffers = (actualValue != newValue);
                bool proposedDiffers = (proposedValue != newValue);
                bool existingValuesMatch = (proposedValue == actualValue);

                if (m_ActualTrace)
				{
                    // For an actual trace, update the actual connectivity attributes.                        
                    if (actualDiffers)
                    {
                        AddToUpdateList(fid, actualAttribute, newValue);
                        updateActual = true;
                    }

                    // Also, for each actual attribute that changes, compare the new value to the corresponding proposed attribute; 
                    // if the values are the same then set the proposed attribute to null, since there is no longer a proposed change in value.
                    if (   (actualDiffers && !proposedDiffers) 
                        || (!actualDiffers && existingValuesMatch))
                    {
						AddToUpdateList(fid, proposedAttribute, "");
                        nullProposed = true;
                    }
                }
				else
				{
					// For a proposed trace, update the Proposed connectivity attributes(i.e.-columns beginning with “PP_”).  
					// Only set proposed attributes when the new proposed value differs from the corresponding actual attribute; 
					// otherwise leave the proposed attribute as null.
					if (proposedDiffers)
					{
                        if (actualDiffers)
                        {
                            AddToUpdateList(fid, proposedAttribute, newValue); 
                            updateProposed = true;
                        }
                        else // new proposed value would match actual value after update, so set to null instead
                        {
                            AddToUpdateList(fid, proposedAttribute, "");
                            nullProposed = true;
                        }
                    }
				}

                if (updateActual)
                {
                    if (updateProposed)    { retVal = AttributesToUpdate.Both; }
                    else if (nullProposed) { retVal = AttributesToUpdate.ActualNullProposed; }
                    else                   { retVal = AttributesToUpdate.Actual; }
                }
                else if (updateProposed)   { retVal = AttributesToUpdate.Proposed; }
                else if (nullProposed)     { retVal = AttributesToUpdate.NullProposed; }
			}
			catch(Exception ex)
			{
				if(m_InteractiveMode)
				{
					MessageBox.Show(m_Application.ApplicationWindow, "Error in DetermineAttributeToUpdate: " + ex.Message,
													"G/Technology", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
				}
				WriteToCommandLog("ERROR", ex.Message, "commonUpdateTrace.DetermineAttributeToUpdate");
                retVal = AttributesToUpdate.Error;
			}

			return retVal;
		}

		/// <summary>
		/// Get the trace results joined with the connectivity attributes.
		/// </summary>
		/// <param name="traceName">Name of the trace to use to query the results and connectivity attributes</param>
		/// <returns>Boolean indicating status</returns>
		private bool GetTraceResults(string traceName)
		{
			bool returnValue = false;

			try
			{
				if(!UpdateTransformerFeedDirection(traceName))
				{
					return false;
				}
				string sql = "select tr.g3e_fid, tr.g3e_fno, tr.g3e_traceorder, tr.g3e_sourcefid, tr.g3e_sourcenode, tr.g3e_node1, tr.g3e_node2, " +
										 "conn.ssta_c, conn.tie_ssta_c, conn.network_id, conn.feeder_1_id, conn.feeder_2_id, conn.feeder_nbr, conn.tie_feeder_nbr, conn.protective_device_fid, " +
										 "conn.volt_1_q, conn.volt_2_q, conn.upstream_node, conn.upstream_protdev_q, " +
										 "conn.pp_ssta_c, conn.pp_tie_ssta_c, conn.pp_network_id, conn.pp_feeder_1_id, conn.pp_feeder_2_id, conn.pp_feeder_nbr, conn.pp_tie_feeder_nbr, conn.pp_protective_device_fid, " +
										 "conn.pp_volt_1_q, conn.pp_volt_2_q, conn.pp_upstream_node, conn.pp_upstream_protdev_q, " +
										 "conn.STATUS_NORMAL_C, conn.feature_state_c, conn.phase_alpha, conn.feeder_type_c " +
										 "from traceresult tr, traceid ti, connectivity_n conn " +
										 "where ti.g3e_name = ? " +
										 "and ti.g3e_id = tr.g3e_tno " +
										 "and tr.g3e_fid = conn.g3e_fid";

				m_TraceResultsRS = m_DataContext.OpenRecordset(sql, CursorTypeEnum.adOpenDynamic, LockTypeEnum.adLockOptimistic, (int)CommandTypeEnum.adCmdText, traceName);

				if(!GetTieTransformers(traceName))
				{
					return false;
				}

				returnValue = true;
			}
			catch(Exception ex)
			{
				if(m_InteractiveMode)
				{
					MessageBox.Show(m_Application.ApplicationWindow, "Error in GetTraceResults: " + ex.Message,
													"G/Technology", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
				}
				WriteToCommandLog("ERROR", ex.Message, "commonUpdateTrace.GetTraceResults");
				returnValue = false;
			}

			return returnValue;
		}

		/// <summary>
		/// If multiple Transformers are connected on a secondary line then some Transformers will
		/// be fed from the secondary. Update the Transformer trace record to be traced from the primary node instead of secondary. 
		/// </summary>
		/// <param name="traceName">Name of the trace to use to query the results</param>
		/// <returns>Boolean indicating status</returns>
		private bool UpdateTransformerFeedDirection(string traceName)
		{
			bool returnValue = false;

			try
			{
				string sql = "select tr.g3e_fid, tr.g3e_node1 " +
											"from traceresult tr, traceid ti " +
											"where ti.g3e_name = ? " +
											"and ti.g3e_id = tr.g3e_tno " +
											"and tr.g3e_fno in (59,60,98,99) " +
											"and tr.g3e_sourcenode = tr.g3e_node2";

				Recordset xfmrRS = m_DataContext.OpenRecordset(sql, CursorTypeEnum.adOpenDynamic, LockTypeEnum.adLockOptimistic, (int)CommandTypeEnum.adCmdText, traceName);

				if(xfmrRS.RecordCount > 0)
				{
					int xfmrFID = 0;
					int xfmrNode1 = 0;
					int newSourceFID = 0;
					int newTraceOrder = 0;
					Recordset newSourceRS = null;
					int recordsAffected = 0;

					xfmrRS.MoveFirst();
					while(!xfmrRS.EOF)
					{
						xfmrFID = Convert.ToInt32(xfmrRS.Fields["G3E_FID"].Value);
						xfmrNode1 = Convert.ToInt32(xfmrRS.Fields["G3E_NODE1"].Value);

						sql = "select tr.g3e_fid, tr.g3e_traceorder " +
										"from traceresult tr, traceid ti " +
										"where ti.g3e_name = ? " +
										"and ti.g3e_id = tr.g3e_tno " +
										"and (tr.g3e_node1 = ? or tr.g3e_node2 = ?) " +
										"and tr.g3e_fid <> ?";

						newSourceRS = m_DataContext.OpenRecordset(sql, CursorTypeEnum.adOpenDynamic, LockTypeEnum.adLockOptimistic, (int)CommandTypeEnum.adCmdText,
														traceName, xfmrNode1, xfmrNode1, xfmrFID);

						newSourceFID = 0;
						newTraceOrder = 0;

						if(newSourceRS.RecordCount > 0)
						{
							newSourceRS.MoveFirst();
							newSourceFID = Convert.ToInt32(newSourceRS.Fields["G3E_FID"].Value);
							newTraceOrder = Convert.ToInt32(newSourceRS.Fields["G3E_TRACEORDER"].Value) + 1;
						}

						sql = "update traceresult set g3e_sourcefid = ?, g3e_traceorder = ?, g3e_sourcenode = ? where g3e_fid = ?";
						m_DataContext.Execute(sql, out recordsAffected, (int)CommandTypeEnum.adCmdText, newSourceFID, newTraceOrder, xfmrNode1, xfmrFID);

						xfmrRS.MoveNext();
					}
				}

				returnValue = true;
			}
			catch(Exception ex)
			{
				if(m_InteractiveMode)
				{
					MessageBox.Show(m_Application.ApplicationWindow, "Error in UpdateTransformerFeedDirection: " + ex.Message,
													"G/Technology", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
				}
				WriteToCommandLog("ERROR", ex.Message, "commonUpdateTrace.UpdateTransformerFeedDirection");
				returnValue = false;
			}

			return returnValue;
		}

		/// <summary>
		/// Gets the Tie Transformer ID if exists for the Transformers in the trace. 
		/// </summary>
		/// <param name="traceName">Name of the trace to use to query the results</param>
		/// <returns>Boolean indicating status</returns>
		private bool GetTieTransformers(string traceName)
		{
			bool returnValue = false;

			try
			{
				string sql = "select distinct n.g3e_fid, n.tie_xfmr_id " +
											"from traceresult tr, traceid ti, xfmr_oh_bank_n n " +
											"where ti.g3e_name = ? " +
											"and ti.g3e_id = tr.g3e_tno " +
											"and tr.g3e_fno in (59,98) " +
											"and tr.g3e_fid = n.g3e_fid " +
											"and n.tie_xfmr_id <> n.g3e_fid " +
											"and n.tie_xfmr_id is not null " +
											"union " +
											"select distinct n.g3e_fid, n.tie_xfmr_id " +
											"from traceresult tr, traceid ti, xfmr_ug_unit_n n " +
											"where ti.g3e_name = ? " +
											"and ti.g3e_id = tr.g3e_tno " +
											"and tr.g3e_fno in (60,99) " +
											"and tr.g3e_fid = n.g3e_fid " +
											"and n.tie_xfmr_id <> n.g3e_fid " +
											"and n.tie_xfmr_id is not null";

				Recordset xfmrTieRS = m_DataContext.OpenRecordset(sql, CursorTypeEnum.adOpenDynamic, LockTypeEnum.adLockOptimistic, (int)CommandTypeEnum.adCmdText, traceName, traceName);

				int xfmrFID = 0;
				int tieXfmrFID = 0;

				if(xfmrTieRS.RecordCount > 0)
				{
					xfmrTieRS.MoveFirst();
					while(!xfmrTieRS.EOF)
					{
						xfmrFID = Convert.ToInt32(xfmrTieRS.Fields["G3E_FID"].Value);
						tieXfmrFID = Convert.ToInt32(xfmrTieRS.Fields["TIE_XFMR_ID"].Value);
						if(!m_TieTransformers.ContainsKey(xfmrFID))
						{
							m_TieTransformers.Add(xfmrFID, tieXfmrFID);
						}

						xfmrTieRS.MoveNext();
					}
				}

				returnValue = true;
			}
			catch(Exception ex)
			{
				if(m_InteractiveMode)
				{
					MessageBox.Show(m_Application.ApplicationWindow, "Error in GetTieTransformers: " + ex.Message,
													"G/Technology", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
				}
				WriteToCommandLog("ERROR", ex.Message, "commonUpdateTrace.GetTieTransformers");
				returnValue = false;
			}

			return returnValue;
		}

        private void InitializeProtectiveDeviceList(TraceStep p_seed)
        {
            if (m_ProtectiveDeviceFNOs.Contains(p_seed.FNO))
            {
                int protectiveDeviceFID = p_seed.FID;
                int tieTransformerFID = 0;

                if (m_TieTransformers.TryGetValue(p_seed.FID, out tieTransformerFID))
                {
                    protectiveDeviceFID = tieTransformerFID;
                }

                m_ProtectiveDevice.fid = protectiveDeviceFID;

            }
            else
            {
                m_ProtectiveDevice.fid = p_seed.ProtectiveDeviceFID;
            }

            m_ProtectiveDevice.phase = p_seed.Phase;
            m_ProtectiveDevice.state = p_seed.FeatureState;
            m_ProtectiveDevice.upstreamProtectionCount = p_seed.UpstreamProtectionCount;

            m_ProtectiveDevices = new List<structProtectiveDevice>();
            m_ProtectiveDevices.Add(m_ProtectiveDevice);
        }

        private void InitializeVisitedList(TraceStep p_seed)
        {
            m_VisitedFIDs = new List<int>();
            m_VisitedFIDs.Add(p_seed.FID);

            m_VisitedRS = new ADODB.Recordset();
            m_VisitedRS.Fields.Append("G3E_FNO", DataTypeEnum.adInteger, 5);
            m_VisitedRS.Fields.Append("G3E_FID", DataTypeEnum.adInteger, 10);
            m_VisitedRS.Open();
            m_VisitedRS.AddNew();
            m_VisitedRS.Fields["G3E_FID"].Value = p_seed.FID;
            m_VisitedRS.Fields["G3E_FNO"].Value = p_seed.FNO;
        }

        private void SetUpstreamForSubstationBreaker()
        {
            ADODB.Recordset rsBreakers = m_TraceResultsRS.Clone();

            if (rsBreakers != null)
            {
                rsBreakers.Filter = "g3e_fno = " + FNO_SUB_BREAKER;
                if (rsBreakers.RecordCount > 0)
                {
                    rsBreakers.MoveFirst();
                    while (rsBreakers.EOF == false)
                    {
                        int breakerFID = Convert.ToInt32(rsBreakers.Fields["G3E_FID"].Value);
                        TraceStep breaker = new TraceStep(rsBreakers, breakerFID);

                        if (breaker.UpstreamNode_Actual != 1)
                        {
                            AddToUpdateList(breakerFID, "UPSTREAM_NODE", "1");
                        }
                        if (breaker.UpstreamNode_PP != 1)
                        {
                            AddToUpdateList(breakerFID, "PP_UPSTREAM_NODE", "1");
                        }

                        rsBreakers.MoveNext();
                        break;
                    }
                }
            }

            rsBreakers.Close();
            rsBreakers = null;
        }

        /// <summary>
        /// Walks the trace results tree and compares the current feature connectivity attributes with the tracking variables. Adds to list if different.
        /// </summary>
        /// <param name="traceOrder">Trace depth to look for next downstream feature</param>
        /// <param name="callingStep">Structure containing tracked values from caller</param>
        /// <returns>Boolean indicating status</returns>
        private bool ProcessTraceResults(int traceOrder, TraceStep callingStep)
		{
			bool returnValue = false;

			try
			{
                AttributesToUpdate attributesToUpdate = AttributesToUpdate.Neither;
				bool protectiveDeviceAdded = false;
				structProtectiveDevice protectiveDevice;

                // VERIFY: Should no longer be necessary since we pass the child step recursively instead of calling step
                //      string originalSourceVoltage = callingStep.voltage;

                // There could be multiple features connected to Source FID. Process each branch.
                List<int> downstreamFIDs = GetNextSteps(callingStep.FID, traceOrder);
                foreach(int childFID in downstreamFIDs)
                { 
                    TraceStep activeStep = new TraceStep(m_TraceResultsRS, childFID);

                    // Reset the source voltage as it may have changed when traversing a previous branch.
                    // For example Elbow FID 1 voltage is 12 and is connected to Elbow FID 2 and Transformer FID 3, 
                    // process goes down connected Transformer branch and source voltage becomes 120/240
                    // then process returns and goes down branch of connected Elbow FID 2. If we don't reset the original source voltage then
                    // Elbow FID 2 voltage will be set to 120/240 along with all features downstream of Elbow FID 2.

                    // VERIFY: Should no longer be necessary since we pass the child step recursively instead of calling step
                    //      callingStep.voltage = originalSourceVoltage;

                    // If FID has not been visited and FNO is not a source feature in secondary trace then process. Else, skip to next record.
                    // Multiple source type features can be in the trace in the case of a secondary trace with tie transformers.
                    // No need to process tie transformers.
                    if (!m_VisitedFIDs.Contains(activeStep.FID) && !(m_SourceFNOs.Contains(activeStep.FNO) && !m_PrimaryTrace))
					{
						m_VisitedFIDs.Add(activeStep.FID);

						m_VisitedRS.AddNew();
						m_VisitedRS.Fields["G3E_FID"].Value = activeStep.FID;
						m_VisitedRS.Fields["G3E_FNO"].Value = activeStep.FNO;

                        // VERIFY: I believe this step is unnecessary since it predates proposed connectivity attributes
                        //
                        //// When using the results of a proposed trace, if the previous feature was PPI or ABI and the new encountered feature is not, 
                        //// then reset tracking variables based on attributes from the new encountered feature.  
                        //// This will ensure that proposed features separated by in-service features will agree with the features they are directly connected to, and therefore validate successfully.
                        //bool IsProposedToActual =
                        //                     (!m_ActualTrace && (callingStep.FeatureState == "PPI" || callingStep.FeatureState == "ABI")
                        //                  && (activeStep.FeatureState != "PPI" && activeStep.FeatureState != "ABI"));

                        bool isSecondaryNetwork = (activeStep.IsNetworkFeature 
                            && m_SecondaryFNOs.Contains(activeStep.FNO) && !m_SourceFNOs.Contains(activeStep.FNO));

                        if (activeStep.StatusNormal == "CLOSED" || activeStep.UpstreamNode == 1)
                        {
                            if (!isSecondaryNetwork)
                            {
                                // Feeder ID
                                attributesToUpdate = DetermineAttributeToUpdate(activeStep.FID, callingStep.DownstreamFeederID,
                                        "FEEDER_1_ID", activeStep.FeederID_1_Actual, "PP_FEEDER_1_ID", activeStep.FeederID_1_PP);
                                if (attributesToUpdate == AttributesToUpdate.Error)                 { return false; }
                                if (attributesToUpdate == AttributesToUpdate.Actual
                                    || attributesToUpdate == AttributesToUpdate.ActualNullProposed
                                    || attributesToUpdate == AttributesToUpdate.Both)               { activeStep.FeederID_1_Actual = callingStep.DownstreamFeederID; }
                                if (attributesToUpdate == AttributesToUpdate.Proposed
                                    || attributesToUpdate == AttributesToUpdate.Both)               { activeStep.FeederID_1_PP = callingStep.DownstreamFeederID; }
                                if (attributesToUpdate == AttributesToUpdate.NullProposed
                                    || attributesToUpdate == AttributesToUpdate.ActualNullProposed) { activeStep.FeederID_1_PP = ""; }

                                // Feeder Number
                                attributesToUpdate = DetermineAttributeToUpdate(activeStep.FID, callingStep.DownstreamFeederNumber,
                                        "FEEDER_NBR", activeStep.FeederNumber_1_Actual, "PP_FEEDER_NBR", activeStep.FeederNumber_1_PP);
                                if (attributesToUpdate == AttributesToUpdate.Error)                 { return false; }
                                if (attributesToUpdate == AttributesToUpdate.Actual
                                    || attributesToUpdate == AttributesToUpdate.ActualNullProposed
                                    || attributesToUpdate == AttributesToUpdate.Both)               { activeStep.FeederNumber_1_Actual = callingStep.FeederNumber_1_PP; }
                                if (attributesToUpdate == AttributesToUpdate.Proposed
                                    || attributesToUpdate == AttributesToUpdate.Both)               { activeStep.FeederNumber_1_PP = callingStep.FeederNumber_1_PP; }
                                if (attributesToUpdate == AttributesToUpdate.NullProposed
                                    || attributesToUpdate == AttributesToUpdate.ActualNullProposed) { activeStep.FeederNumber_1_PP = ""; }
                            }

                            // Substation Code
                            attributesToUpdate = DetermineAttributeToUpdate(activeStep.FID, callingStep.DownstreamSubstationCode,
                                    "SSTA_C", activeStep.SubstationCode_1_Actual, "PP_SSTA_C", activeStep.SubstationCode_1_PP);
                            if (attributesToUpdate == AttributesToUpdate.Error)                 { return false; }
                            if (attributesToUpdate == AttributesToUpdate.Actual
                                || attributesToUpdate == AttributesToUpdate.ActualNullProposed
                                || attributesToUpdate == AttributesToUpdate.Both)               { activeStep.SubstationCode_1_Actual = callingStep.DownstreamSubstationCode; }
                            if (attributesToUpdate == AttributesToUpdate.Proposed
                                || attributesToUpdate == AttributesToUpdate.Both)               { activeStep.SubstationCode_1_PP = callingStep.DownstreamSubstationCode; }
                            if (attributesToUpdate == AttributesToUpdate.NullProposed
                                || attributesToUpdate == AttributesToUpdate.ActualNullProposed) { activeStep.SubstationCode_1_PP = ""; }
                        }

                        if (activeStep.StatusNormal == "CLOSED" || activeStep.UpstreamNode == 2)
						{
                            if (!isSecondaryNetwork)
                            {
                                // Tie Feeder ID
                                attributesToUpdate = DetermineAttributeToUpdate(activeStep.FID, callingStep.DownstreamFeederID,
                                        "FEEDER_2_ID", activeStep.FeederID_2_Actual, "PP_FEEDER_2_ID", activeStep.FeederID_2_PP);
                                if (attributesToUpdate == AttributesToUpdate.Error)                 { return false; }
                                if (attributesToUpdate == AttributesToUpdate.Actual
                                    || attributesToUpdate == AttributesToUpdate.ActualNullProposed
                                    || attributesToUpdate == AttributesToUpdate.Both)               { activeStep.FeederID_2_Actual = callingStep.DownstreamFeederID; }
                                if (attributesToUpdate == AttributesToUpdate.Proposed
                                    || attributesToUpdate == AttributesToUpdate.Both)               { activeStep.FeederID_2_PP = callingStep.DownstreamFeederID; }
                                if (attributesToUpdate == AttributesToUpdate.NullProposed
                                    || attributesToUpdate == AttributesToUpdate.ActualNullProposed) { activeStep.FeederID_2_PP = ""; }

                                // Tie Feeder Number
                                attributesToUpdate = DetermineAttributeToUpdate(activeStep.FID, callingStep.DownstreamFeederNumber,
                                        "TIE_FEEDER_NBR", activeStep.FeederNumber_2_Actual, "PP_TIE_FEEDER_NBR", activeStep.FeederNumber_2_PP);
                                if (attributesToUpdate == AttributesToUpdate.Error)                 { return false; }
                                if (attributesToUpdate == AttributesToUpdate.Actual
                                    || attributesToUpdate == AttributesToUpdate.ActualNullProposed
                                    || attributesToUpdate == AttributesToUpdate.Both)               { activeStep.FeederNumber_2_Actual = callingStep.DownstreamFeederNumber; }
                                if (attributesToUpdate == AttributesToUpdate.Proposed
                                    || attributesToUpdate == AttributesToUpdate.Both)               { activeStep.FeederNumber_2_PP = callingStep.DownstreamFeederNumber; }
                                if (attributesToUpdate == AttributesToUpdate.NullProposed
                                    || attributesToUpdate == AttributesToUpdate.ActualNullProposed) { activeStep.FeederNumber_2_PP = ""; }
                            }

                            // Tie Substation Code
                            attributesToUpdate = DetermineAttributeToUpdate(activeStep.FID, callingStep.DownstreamSubstationCode,
                                    "TIE_SSTA_C", activeStep.SubstationCode_2_Actual, "PP_TIE_SSTA_C", activeStep.SubstationCode_2_PP);
                            if (attributesToUpdate == AttributesToUpdate.Error)                 { return false; }
                            if (attributesToUpdate == AttributesToUpdate.Actual
                                || attributesToUpdate == AttributesToUpdate.ActualNullProposed
                                || attributesToUpdate == AttributesToUpdate.Both)               { activeStep.SubstationCode_2_Actual = callingStep.DownstreamSubstationCode; }
                            if (attributesToUpdate == AttributesToUpdate.Proposed
                                || attributesToUpdate == AttributesToUpdate.Both)               { activeStep.SubstationCode_2_PP = callingStep.DownstreamSubstationCode; }
                            if (attributesToUpdate == AttributesToUpdate.NullProposed
                                || attributesToUpdate == AttributesToUpdate.ActualNullProposed) { activeStep.SubstationCode_2_PP = ""; }
                        }

                        // Network ID
                        if (activeStep.IsNetworkFeature)
                        {
                            attributesToUpdate = DetermineAttributeToUpdate(activeStep.FID, callingStep.NetworkID,
                                    "NETWORK_ID", activeStep.NetworkID_Actual, "PP_NETWORK_ID", activeStep.NetworkID_PP);
                            if (attributesToUpdate == AttributesToUpdate.Error)                 { return false; }
                            if (attributesToUpdate == AttributesToUpdate.Actual
                                || attributesToUpdate == AttributesToUpdate.ActualNullProposed
                                || attributesToUpdate == AttributesToUpdate.Both)               { activeStep.NetworkID_Actual = callingStep.NetworkID; }
                            if (attributesToUpdate == AttributesToUpdate.Proposed
                                || attributesToUpdate == AttributesToUpdate.Both)               { activeStep.NetworkID_PP = callingStep.NetworkID; }
                            if (attributesToUpdate == AttributesToUpdate.NullProposed
                                || attributesToUpdate == AttributesToUpdate.ActualNullProposed) { activeStep.NetworkID_PP = ""; }
                        }
                            
                        // Voltage
                        if (activeStep.FNO != FNO_AUTO_TRANSFORMER 
                                || (activeStep.FNO == FNO_AUTO_TRANSFORMER && activeStep.UpstreamNode == 1))
                        {
                            attributesToUpdate = DetermineAttributeToUpdate(activeStep.FID, callingStep.DownstreamVoltage,
                                    "VOLT_1_Q", activeStep.Voltage_1_Actual, "PP_VOLT_1_Q", activeStep.Voltage_1_PP);
                            if (attributesToUpdate == AttributesToUpdate.Error)                 { return false; }
                            if (attributesToUpdate == AttributesToUpdate.Actual
                                || attributesToUpdate == AttributesToUpdate.ActualNullProposed
                                || attributesToUpdate == AttributesToUpdate.Both)               { activeStep.Voltage_1_Actual = callingStep.DownstreamVoltage; }
                            if (attributesToUpdate == AttributesToUpdate.Proposed
                                || attributesToUpdate == AttributesToUpdate.Both)               { activeStep.Voltage_1_PP = callingStep.DownstreamVoltage; }
                            if (attributesToUpdate == AttributesToUpdate.NullProposed
                                || attributesToUpdate == AttributesToUpdate.ActualNullProposed) { activeStep.Voltage_1_PP = ""; }
                        }

                        if ((activeStep.FNO != FNO_AUTO_TRANSFORMER && !m_SourceFNOs.Contains(activeStep.FNO)) 
                                || (activeStep.FNO == FNO_AUTO_TRANSFORMER && activeStep.UpstreamNode == 2))
                        {
                            attributesToUpdate = DetermineAttributeToUpdate(activeStep.FID, callingStep.DownstreamVoltage,
                                    "VOLT_2_Q", activeStep.Voltage_2_Actual, "PP_VOLT_2_Q", activeStep.Voltage_2_PP);
                            if (attributesToUpdate == AttributesToUpdate.Error)                 { return false; }
                            if (attributesToUpdate == AttributesToUpdate.Actual
                                || attributesToUpdate == AttributesToUpdate.ActualNullProposed
                                || attributesToUpdate == AttributesToUpdate.Both)               { activeStep.Voltage_2_Actual = callingStep.DownstreamVoltage; }
                            if (attributesToUpdate == AttributesToUpdate.Proposed
                                || attributesToUpdate == AttributesToUpdate.Both)               { activeStep.Voltage_2_PP = callingStep.DownstreamVoltage; }
                            if (attributesToUpdate == AttributesToUpdate.NullProposed
                                || attributesToUpdate == AttributesToUpdate.ActualNullProposed) { activeStep.Voltage_2_PP = ""; }
                        }

                        if (!isSecondaryNetwork)
                        {
                            // Upstream Node
                            attributesToUpdate = DetermineAttributeToUpdate(activeStep.FID, activeStep.UpstreamNode.ToString(),
                                    "UPSTREAM_NODE", activeStep.UpstreamNode_Actual.ToString(),
                                    "PP_UPSTREAM_NODE", activeStep.UpstreamNode_PP.ToString());
                            if (attributesToUpdate == AttributesToUpdate.Error)                 { return false; }
                            // no need to set actual or proposed values in activeStep since upstream node is always derived from trace results

                            // Protective Device
                            protectiveDevice = m_ProtectiveDevices.Last();
                            if (activeStep.Phase != protectiveDevice.phase)
                            {
                                // Determine the most recent Protective Device in the stack that shares at least one phase letter 
                                // in common with the encountered feature.
                                m_ProtectiveDevices.Reverse();

                                bool phaseFound = false;
                                foreach (structProtectiveDevice device in m_ProtectiveDevices)
                                {
                                    foreach (char phase in activeStep.Phase.ToArray())
                                    {
                                        if (device.phase.Contains(phase))
                                        {
                                            protectiveDevice = device;
                                            phaseFound = true;
                                            break;
                                        }
                                    }

                                    if (phaseFound)
                                    {
                                        break;
                                    }
                                }

                                m_ProtectiveDevices.Reverse();
                            }

                            if (protectiveDevice.fid > 0)
                            {
                                attributesToUpdate = DetermineAttributeToUpdate(activeStep.FID,
                                        protectiveDevice.fid.ToString(),
                                        "PROTECTIVE_DEVICE_FID", activeStep.ProtectiveDeviceFID_Actual.ToString(),
                                        "PP_PROTECTIVE_DEVICE_FID", activeStep.ProtectiveDeviceFID_PP.ToString());
                                if (attributesToUpdate == AttributesToUpdate.Error)                 { return false; }
                                if (attributesToUpdate == AttributesToUpdate.Actual
                                    || attributesToUpdate == AttributesToUpdate.ActualNullProposed
                                    || attributesToUpdate == AttributesToUpdate.Both)               { activeStep.ProtectiveDeviceFID_Actual = protectiveDevice.fid; }
                                if (attributesToUpdate == AttributesToUpdate.Proposed
                                    || attributesToUpdate == AttributesToUpdate.Both)               { activeStep.ProtectiveDeviceFID_PP = protectiveDevice.fid; }
                                if (attributesToUpdate == AttributesToUpdate.NullProposed
                                    || attributesToUpdate == AttributesToUpdate.ActualNullProposed) { activeStep.ProtectiveDeviceFID_PP = 0; }
                            }

                            // Upstream Protection Count
                            attributesToUpdate = DetermineAttributeToUpdate(activeStep.FID,
                                    protectiveDevice.upstreamProtectionCount.ToString(),
                                    "UPSTREAM_PROTDEV_Q", activeStep.UpstreamProtectionCount_Actual.ToString(),
                                    "PP_UPSTREAM_PROTDEV_Q", activeStep.UpstreamProtectionCount_PP.ToString());
                            if (attributesToUpdate == AttributesToUpdate.Error)                 { return false; }
                            if (attributesToUpdate == AttributesToUpdate.Actual
                                || attributesToUpdate == AttributesToUpdate.ActualNullProposed
                                || attributesToUpdate == AttributesToUpdate.Both)               { activeStep.UpstreamProtectionCount_Actual = protectiveDevice.upstreamProtectionCount; }
                            if (attributesToUpdate == AttributesToUpdate.Proposed
                                || attributesToUpdate == AttributesToUpdate.Both)               { activeStep.UpstreamProtectionCount_PP = protectiveDevice.upstreamProtectionCount; }
                            if (attributesToUpdate == AttributesToUpdate.NullProposed
                                || attributesToUpdate == AttributesToUpdate.ActualNullProposed) { activeStep.UpstreamProtectionCount_PP = 0; }

							// Check if current feature is a protective device. If so, add to Protective Device list.                            
							if(m_ProtectiveDeviceFNOs.Contains(activeStep.FNO))
							{
                                int protectiveDeviceFID = activeStep.FID;
                                int tieTransformerFID = 0;

                                if (m_TieTransformers.TryGetValue(activeStep.FID, out tieTransformerFID))
								{
									protectiveDeviceFID = tieTransformerFID;
								}

								m_ProtectiveDevice = new structProtectiveDevice();
								m_ProtectiveDevice.fid = protectiveDeviceFID;
								m_ProtectiveDevice.phase = activeStep.Phase;
								m_ProtectiveDevice.state = activeStep.FeatureState;
								m_ProtectiveDevice.upstreamProtectionCount = protectiveDevice.upstreamProtectionCount + 1;

								m_ProtectiveDevices.Add(m_ProtectiveDevice);

								protectiveDeviceAdded = true;
							}
                        }

                        // Continue walking the trace results tree using recursive call
                        if (!ProcessTraceResults(traceOrder + 1, activeStep))
                        {
                            return false;
                        }

                        // If the current feature is a Protection Device then remove from the Protective Device stack
                        // since it has already been used downstream by the preceding recursive call and we need to rollback to the
                        // previous Protective Device when exiting this recursive call.
                        if (protectiveDeviceAdded)
						{
							m_ProtectiveDevices.RemoveAt(m_ProtectiveDevices.Count() - 1);
							protectiveDeviceAdded = false;
						}
					}
				}

				returnValue = true;
			}
			catch(Exception ex)
			{
				if(m_InteractiveMode)
				{
					MessageBox.Show(m_Application.ApplicationWindow, "Error in ProcessTraceResults: " + ex.Message,
													"G/Technology", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
				}
				WriteToCommandLog("ERROR", ex.Message, "commonUpdateTrace.ProcessTraceResults");

				returnValue = false; 
            }

			return returnValue;
		}

        /// <summary>
        /// Filter recordset to get downstream features directly connected to calling FID.
        /// </summary>
        /// <param name="p_callingFID">FID whose downstream node to check</param>
        /// <param name="p_traceOrder">Trace order for filter</param>
        /// <returns>List of FIDs</returns>
        private List<int> GetNextSteps(int p_callingFID, int p_traceOrder)
        {
            List<int> retVal = new List<int>();
            int FID = 0;

            Recordset traceRS = m_TraceResultsRS.Clone();

            traceRS.Filter = "g3e_traceorder = " + p_traceOrder + " and g3e_sourcefid = " + p_callingFID;
            if (traceRS.RecordCount > 0)
            {
                traceRS.MoveFirst();
                while (!traceRS.EOF)
                {
                    try
                    {
                        FID = Convert.ToInt32(traceRS.Fields["G3E_FID"].Value);
                        retVal.Add(FID);
                    }
                    catch { }
                    traceRS.MoveNext();
                }
            }

            traceRS.Close();
            traceRS = null;

            return retVal;
        }

        /// <summary>
        /// Adds 2 entries to the legend. 1) Trace results 2) Features that will be updated
        /// </summary>
        /// <param name="gtUpdateWorldRange">Range of the map window to fit the updates</param>
        /// <returns>Boolean indicating status</returns>
        private bool DisplayResults(ref IGTWorldRange gtUpdateWorldRange)
		{
			bool returnValue = false;

			try
			{
				// Create symbology to use to display trace results
				IGTSymbology gtSymbology = GTClassFactory.Create<IGTSymbology>();
				gtSymbology.Color = m_CommandMetadata.TraceHighlightColor;
				gtSymbology.FillColor = m_CommandMetadata.TraceHighlightFillColor;
				gtSymbology.Width = m_CommandMetadata.TraceHighlightWidth;

				// Add trace results to legend
				m_TraceHelper.DisplayResults(m_VisitedRS, "Traces", "Update Trace - " + m_TraceHelper.SeedFID.ToString(), gtSymbology, true, true, true, true);
				m_LegendItems = new List<string>();
				m_LegendItems.Add("Update Trace - " + m_TraceHelper.SeedFID.ToString());

				short fno;
				int fid;

				// Create recordset to display all features to update that are not part of the WR.
				ADODB.Recordset updatesRS = new ADODB.Recordset();
				updatesRS.Fields.Append("G3E_FNO", DataTypeEnum.adInteger, 5);
				updatesRS.Fields.Append("G3E_FID", DataTypeEnum.adInteger, 10);
				updatesRS.Open();

                #region Commented logic to limit edits to features affected by the WR
                /*  Commented logic to limit edits to features affected by the WR
                 *  
                // 5-23-2018: Change to DDD. Comment out logic to display and limit the edits to features affected by the WR
                //if (m_JobType != "NON-WR")
                //{
                //    // Get the data that is part of the WR
                //    ADODB.Recordset wrRS = null;
                //    GetWrData(ref wrRS);

                //    // Create recordset to display all WR features that need to be updated
                //    ADODB.Recordset wrUpdatesRS = new ADODB.Recordset();
                //    wrUpdatesRS.Fields.Append("G3E_FNO", DataTypeEnum.adInteger, 5);
                //    wrUpdatesRS.Fields.Append("G3E_FID", DataTypeEnum.adInteger, 10);
                //    wrUpdatesRS.Open();

                //    // Build recordsets for display.
                //    // wrUpdatesRS will include features to update if in WR and feature is part of WR
                //    // updatesRS will include features to update if in WR and feature is not part of WR
                //    foreach (KeyValuePair<int, Dictionary<string, string>> kvpFID in m_AttributeUpdates)
                //    {
                //        fid = kvpFID.Key;
                //        m_TraceResultsRS.Filter = "g3e_fid = " + fid;
                //        fno = Convert.ToInt16(m_TraceResultsRS.Fields["G3E_FNO"].Value);

                //        wrRS.Filter = "g3e_fid = " + fid;
                //        if (wrRS.RecordCount > 0)
                //        {
                //            wrUpdatesRS.AddNew();
                //            wrUpdatesRS.Fields["G3E_FID"].Value = fid;
                //            wrUpdatesRS.Fields["G3E_FNO"].Value = fno;
                //        }
                //        else
                //        {
                //            updatesRS.AddNew();
                //            updatesRS.Fields["G3E_FID"].Value = fid;
                //            updatesRS.Fields["G3E_FNO"].Value = fno;
                //        }
                //        wrRS.Filter = "";
                //        m_TraceResultsRS.Filter = "";
                //    }

                //    if (wrUpdatesRS.RecordCount > 0)
                //    {
                //        // Create symbology to use to display features to be updated that are part of WR
                //        gtSymbology.Color = m_CommandMetadata.UpdateHighlightColor;
                //        gtSymbology.FillColor = m_CommandMetadata.UpdateHighlightFillColor;
                //        gtSymbology.Width = m_CommandMetadata.UpdateHighlightWidth;

                //        // Add features to legend that that will be updated and are part of WR
                //        m_TraceHelper.DisplayResults(wrUpdatesRS, "Traces", "Features to Update in WR", gtSymbology, true, true, true, true);
                //        m_LegendItems.Add("Features to Update in WR");
                //    }

                //    if (updatesRS.RecordCount > 0)
                //    {
                //        // Create symbology to use to display features to be updated that are not part of WR 
                //        gtSymbology.Color = m_CommandMetadata.NonJobHighlightColor;
                //        gtSymbology.FillColor = m_CommandMetadata.NonJobHighlightFillColor;
                //        gtSymbology.Width = m_CommandMetadata.NonJobHighlightWidth;

                //        // Add features to legend that that will be updated and are not part of WR
                //        m_TraceHelper.DisplayResults(updatesRS, "Traces", "Features to Update not in WR", gtSymbology, true, true, true, true);
                //        m_LegendItems.Add("Features to Update not in WR");
                //    }                   
                //}
                //else
                //{
                // Build recordset for display.
                // updatesRS will include all features to update //if in Non-WR
                */
                #endregion

                foreach (KeyValuePair<int, Dictionary<string, string>> kvpFID in m_AttributeUpdates)
				{
					fid = kvpFID.Key;
					m_TraceResultsRS.Filter = "g3e_fid = " + fid;
					if(m_TraceResultsRS.RecordCount > 0)
					{
						fno = Convert.ToInt16(m_TraceResultsRS.Fields["G3E_FNO"].Value);

						updatesRS.AddNew();
						updatesRS.Fields["G3E_FID"].Value = fid;
						updatesRS.Fields["G3E_FNO"].Value = fno;
					}
					m_TraceResultsRS.Filter = "";
				}

				if(updatesRS.RecordCount > 0)
				{
					// Create symbology to use to display features to be updated
					gtSymbology.Color = m_CommandMetadata.UpdateHighlightColor;
					gtSymbology.FillColor = m_CommandMetadata.UpdateHighlightFillColor;
					gtSymbology.Width = m_CommandMetadata.UpdateHighlightWidth;

					// Add features to legend that that will be updated
					m_TraceHelper.DisplayResults(updatesRS, "Traces", "Features to Update", gtSymbology, true, true, true, true);
					m_LegendItems.Add("Features to Update");
				}
				//}

				IGTDisplayService displayService = m_Application.ActiveMapWindow.DisplayService;

                #region Commented logic to display and limit the edits to features affected by the WR
                /*
                // 5-23-2018: Change to DDD. Comment out logic to display and limit the edits to features affected by the WR
                //if (m_JobType != "NON-WR" && updatesRS.RecordCount > 0)
                //{
                //    // Fit features to update that are not part of WR
                //    gtUpdateWorldRange = displayService.GetRange("Traces", "Features to Update not in WR");
                //    m_Application.ActiveMapWindow.ZoomArea(gtUpdateWorldRange);
                //    m_Application.RefreshWindows();

                //    DialogResult dialogResult = MessageBox.Show(m_Application.ApplicationWindow, "Trace encountered features requiring updates that are not part of the active WR.  Update these features also? [Y/N]",
                //                    "G/Technology", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation);
                //    if (dialogResult == DialogResult.Yes)
                //    {
                //        if (m_LegendItems.Contains("Features to Update in WR"))
                //        {
                //            // Build map window range covering results for both features to update in WR and not in WR.
                //            // Used to fit in map window the features to update.
                //            IGTWorldRange gtTempWorldRange = displayService.GetRange("Traces", "Features to Update in WR");
                //            IGTPoint gtPointBottomLeft = GTClassFactory.Create<IGTPoint>();
                //            IGTPoint gtPointTopRight = GTClassFactory.Create<IGTPoint>();
                //            if (gtUpdateWorldRange.BottomLeft.X < gtTempWorldRange.BottomLeft.X)
                //            {
                //                gtPointBottomLeft.X = gtUpdateWorldRange.BottomLeft.X;
                //            }
                //            else
                //            {
                //                gtPointBottomLeft.X = gtTempWorldRange.BottomLeft.X;
                //            }

                //            if (gtUpdateWorldRange.BottomLeft.Y < gtTempWorldRange.BottomLeft.Y)
                //            {
                //                gtPointBottomLeft.Y = gtUpdateWorldRange.BottomLeft.Y;
                //            }
                //            else
                //            {
                //                gtPointBottomLeft.Y = gtTempWorldRange.BottomLeft.Y;
                //            }

                //            if (gtUpdateWorldRange.TopRight.X > gtTempWorldRange.TopRight.X)
                //            {
                //                gtPointTopRight.X = gtUpdateWorldRange.TopRight.X;
                //            }
                //            else
                //            {
                //                gtPointTopRight.X = gtTempWorldRange.TopRight.X;
                //            }

                //            if (gtUpdateWorldRange.TopRight.Y > gtTempWorldRange.TopRight.Y)
                //            {
                //                gtPointTopRight.Y = gtUpdateWorldRange.TopRight.Y;
                //            }
                //            else
                //            {
                //                gtPointTopRight.Y = gtTempWorldRange.TopRight.Y;
                //            }

                //            gtUpdateWorldRange.BottomLeft = gtPointBottomLeft;
                //            gtUpdateWorldRange.TopRight = gtPointTopRight;
                //        }

                //        m_TraceHelper.RemoveTraceLegendItem("Traces", "Features to Update not in WR");

                //        // Update symbology for features that will be updated and are not part of WR
                //        // to be the same as the features that will be updated and are part of WR
                //        // so the user can see all features that will be updated.
                //        gtSymbology = GTClassFactory.Create<IGTSymbology>();
                //        gtSymbology.Color = m_CommandMetadata.UpdateHighlightColor;
                //        gtSymbology.FillColor = m_CommandMetadata.UpdateHighlightFillColor;
                //        gtSymbology.Width = m_CommandMetadata.UpdateHighlightWidth;

                //        // Add features to legend that that will be updated and are not part of WR
                //        m_TraceHelper.DisplayResults(updatesRS, "Traces", "Features to Update not in WR", gtSymbology, true, true, true, true);
                //    }
                //    else
                //    {
                //        // Remove features not in WR from the update list and legend
                //        updatesRS.MoveFirst();

                //        while (!updatesRS.EOF)
                //        {
                //            fid = Convert.ToInt32(updatesRS.Fields["G3E_FID"].Value);
                //            m_AttributeUpdates.Remove(fid);

                //            updatesRS.MoveNext();
                //        }
                //        m_TraceHelper.RemoveTraceLegendItem("Traces", "Features to Update not in WR");
                //        m_LegendItems.Remove("Features to Update not in WR");
                //    }
                //}
                //else
                //{
                //    if (m_JobType == "NON-WR")
                //    {
                gtUpdateWorldRange = displayService.GetRange("Traces", "Features to Update");
				//    }
				//    else
				//    {
				//        gtUpdateWorldRange = displayService.GetRange("Traces", "Features to Update in WR");
				//    }
				//}
                */
                #endregion

                // Add buffer to range
                int paperScale = displayService.PaperScale;
				int bufferOffset = paperScale / 100;
				IGTPoint gtPointBufferBottomLeft = GTClassFactory.Create<IGTPoint>();
				IGTPoint gtPointBufferTopRight = GTClassFactory.Create<IGTPoint>();
				gtPointBufferBottomLeft.X = gtUpdateWorldRange.BottomLeft.X - bufferOffset;
				gtPointBufferBottomLeft.Y = gtUpdateWorldRange.BottomLeft.Y - bufferOffset;
				gtPointBufferTopRight.X = gtUpdateWorldRange.TopRight.X + bufferOffset;
				gtPointBufferTopRight.Y = gtUpdateWorldRange.TopRight.Y + bufferOffset;

				gtUpdateWorldRange.BottomLeft = gtPointBufferBottomLeft;
				gtUpdateWorldRange.TopRight = gtPointBufferTopRight;

				returnValue = true;
			}
			catch(Exception ex)
			{
				if(m_InteractiveMode)
				{
					MessageBox.Show(m_Application.ApplicationWindow, "Error in DisplayResults: " + ex.Message,
													"G/Technology", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
				}
				WriteToCommandLog("ERROR", ex.Message, "commonUpdateTrace.DisplayResults");
				returnValue = false;
			}

			return returnValue;
		}

		/// <summary>
		/// Returns a recordset containing the posted edits from the asset history for the WR and also the pending edits in the WR.
		/// </summary>
		/// <param name="wrRS">Recordset containing the edits for the WR. The edits include the posted edits from the Asset History table and the pending edits</param>
		/// <returns>Boolean indicating status</returns>
		private bool GetWrData(ref ADODB.Recordset wrRS)
		{
			bool returnValue = false;

			try
			{
				// Get posted edits from the Asset History table.
				string sql = "select g3e_fno, g3e_fid from asset_history where g3e_identifier = ?";

				wrRS = m_DataContext.OpenRecordset(sql, CursorTypeEnum.adOpenDynamic, LockTypeEnum.adLockOptimistic, (int)CommandTypeEnum.adCmdText, m_DataContext.ActiveJob);

				IGTJobHelper gtJobHelper = GTClassFactory.Create<IGTJobHelper>();
				gtJobHelper.DataContext = m_DataContext;

				// Get pending edits.
				ADODB.Recordset pendingEditsRS = gtJobHelper.FindPendingEdits();

				// Merge edits into one recordset.
				if(wrRS.RecordCount > 0)
				{
					if(pendingEditsRS.RecordCount > 0)
					{
						// Add pending edit records
						while(!pendingEditsRS.EOF)
						{
							wrRS.AddNew();
							wrRS.Fields["G3E_FNO"].Value = pendingEditsRS.Fields["G3E_FNO"].Value;
							wrRS.Fields["G3E_FID"].Value = pendingEditsRS.Fields["G3E_FID"].Value;

							pendingEditsRS.MoveNext();
						}
					}
				}
				else
				{
					// No edits in asset history. Return pending edits.
					if(pendingEditsRS != null)
					{
						wrRS = pendingEditsRS;
					}
				}

				gtJobHelper = null;
				returnValue = true;
			}
			catch(Exception ex)
			{
				if(m_InteractiveMode)
				{
					MessageBox.Show(m_Application.ApplicationWindow, "Error in GetWrData: " + ex.Message,
													"G/Technology", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
				}
				WriteToCommandLog("ERROR", ex.Message, "commonUpdateTrace.GetWrData");
				returnValue = false;
			}

			return returnValue;
		}

		/// <summary>
		/// Gets the metadata parameters defined in SYS_GENERALPARAMETER for UpdateTrace.
		/// </summary>
		/// <returns>Boolean indicating status</returns>
		private bool GetMetadataParameters()
		{
			bool returnValue = false;

			try
			{
				string sql = "select subsystem_component, param_name, param_value from sys_generalparameter where subsystem_name = ?";

				ADODB.Recordset commandMetadataRS = m_DataContext.OpenRecordset(sql, CursorTypeEnum.adOpenDynamic, LockTypeEnum.adLockOptimistic, (int)CommandTypeEnum.adCmdText, "UpdateTrace");

				if(commandMetadataRS.RecordCount > 0)
				{
					m_CommandMetadata = new structCommandMetadata();
					string subsystemComponent = string.Empty;
					string paramName = string.Empty;
					string paramValue = string.Empty;

					commandMetadataRS.MoveFirst();

					while(!commandMetadataRS.EOF)
					{
						subsystemComponent = commandMetadataRS.Fields["SUBSYSTEM_COMPONENT"].Value.ToString();
						paramName = commandMetadataRS.Fields["PARAM_NAME"].Value.ToString();
						paramValue = commandMetadataRS.Fields["PARAM_VALUE"].Value.ToString();

						if(subsystemComponent == "UpdateHighlightProperty")
						{
							if(paramName == "Color")
							{
								m_CommandMetadata.UpdateHighlightColor = paramValue;
							}
							else if(paramName == "FillColor")
							{
								m_CommandMetadata.UpdateHighlightFillColor = paramValue;
							}
							else if(paramName == "Width")
							{
								m_CommandMetadata.UpdateHighlightWidth = paramValue;
							}
						}
						else if(subsystemComponent == "NonJobHighlightProperty")
						{
							if(paramName == "Color")
							{
								m_CommandMetadata.NonJobHighlightColor = paramValue;
							}
							else if(paramName == "FillColor")
							{
								m_CommandMetadata.NonJobHighlightFillColor = paramValue;
							}
							else if(paramName == "Width")
							{
								m_CommandMetadata.NonJobHighlightWidth = paramValue;
							}
						}
						else if(subsystemComponent == "TraceHighlightProperty")
						{
							if(paramName == "Color")
							{
								m_CommandMetadata.TraceHighlightColor = paramValue;
							}
							else if(paramName == "FillColor")
							{
								m_CommandMetadata.TraceHighlightFillColor = paramValue;
							}
							else if(paramName == "Width")
							{
								m_CommandMetadata.TraceHighlightWidth = paramValue;
							}
						}
						commandMetadataRS.MoveNext();
					}
				}

				returnValue = true;
			}
			catch(Exception ex)
			{
				if(m_InteractiveMode)
				{
					MessageBox.Show(m_Application.ApplicationWindow, "Error in GetMetadataParameters: " + ex.Message,
													"G/Technology", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
				}
				WriteToCommandLog("ERROR", ex.Message, "commonUpdateTrace.GetMetadataParameters");
				returnValue = false;
			}

			return returnValue;
		}

		/// <summary>
		/// Adds the G3E_FID, connectivity attribute field name and value to m_AttributeUpdates for later updating.
		/// </summary>
		/// <param name="fid">G3E_FID to add or update if exists in m_AttributeUpdates</param>
		/// <param name="attributeName">CONNECTIVITY_N.column name to add to m_AttributeUpdates</param>
		/// <param name="attributeValue">Attribute value for CONNECTIVITY_N.column name to add to m_AttributeUpdates</param>
		/// <returns>Boolean indicating status</returns>
		private bool AddToUpdateList(Int32 fid, string attributeName, string attributeValue)
		{
			bool returnValue = false;

			try
			{
				Dictionary<string, string> columnValueDictionary = new Dictionary<string, string>();
				if(m_AttributeUpdates.TryGetValue(fid, out columnValueDictionary))
				{
					// The FID is already in m_AttributeUpdates. Add the field and value.
					// Verify that the attribute does not exist already then add
					string sAttributeValue = string.Empty;

					if(!columnValueDictionary.TryGetValue(attributeName, out sAttributeValue))
					{
						columnValueDictionary.Add(attributeName, attributeValue);
					}
				}
				else
				{
					// Add the fid, field, and value to m_AttributeUpdates.
					columnValueDictionary = new Dictionary<string, string>();
					columnValueDictionary.Add(attributeName, attributeValue);
					m_AttributeUpdates.Add(fid, columnValueDictionary);
				}

				returnValue = true;
			}
			catch(Exception ex)
			{
				if(m_InteractiveMode)
				{
					MessageBox.Show(m_Application.ApplicationWindow, "Error in AddToUpdateList: " + ex.Message,
													"G/Technology", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
				}
				WriteToCommandLog("ERROR", ex.Message, "commonUpdateTrace.AddToUpdateList");
				returnValue = false;
			}

			return returnValue;
		}

		/// <summary>
		/// Updates the connectivity attributes for the G3E_FID and attributes in m_AttributeUpdates.
		/// </summary>
		/// <returns>Boolean indicating status</returns>
		private bool UpdateConnectivityAttributes()
		{
			bool returnValue = false;
			string sql = string.Empty;
			int recordsAffected = 0;
			List<object> parameters = new List<object>();

			try
			{
				// Build the UPDATE statement. Loop through the G3E_FIDs in m_AttributeUpdates
				foreach(KeyValuePair<int, Dictionary<string, string>> kvpFID in m_AttributeUpdates)
				{
					parameters.Clear();
					sql = "update connectivity_n set ";

					// Build the UPDATE statement. Loop through each attribute for the active G3E_FID in m_AttributeUpdates
					foreach(KeyValuePair<string, string> kvpAttribute in kvpFID.Value)
					{
						sql += kvpAttribute.Key + "=?,";
						parameters.Add(kvpAttribute.Value);
					}

					sql = sql.Remove(sql.Length - 1);
					sql += " where g3e_fid = ?";
					parameters.Add(kvpFID.Key);

					// Run the update statement
					m_DataContext.Execute(sql, out recordsAffected, (int)CommandTypeEnum.adCmdText, parameters.ToArray());
				}

				returnValue = true;
			}
			catch(Exception ex)
			{
				if(m_InteractiveMode)
				{
					MessageBox.Show(m_Application.ApplicationWindow, "Error in UpdateConnectivityAttributes: " + ex.Message,
													"G/Technology", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
				}

				WriteToCommandLog("ERROR", ex.Message, "commonUpdateTrace.UpdateConnectivityAttributes");

				returnValue = false;
			}

			parameters.Clear();
			parameters = null;

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
