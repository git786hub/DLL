using System;
using System.IO;
using System.Drawing;
using System.Windows.Forms;
using System.Collections.Generic;
using Intergraph.GTechnology.API;
using ADODB;
using System.Reflection;

namespace GTechnology.Oncor.CustomAPI
{
    public partial class frmSecondaryCalculator : Form
    {
        public IGTCustomCommandHelper m_CustomCommandHelper;
        public IGTTransactionManager m_TransactionManager;
        private IGTApplication m_Application = GTClassFactory.Create<IGTApplication>();
        private clsCalculate m_clsCalculate = new clsCalculate();
        private TraceHelper m_clsTraceHelper = new TraceHelper(-1, "Secondary Calculator");
        private CommonDT m_CommonDT = new CommonDT();
        private ReportDT m_ReportDT;
        private Recordset m_TonnageRS;
        private Recordset m_CablesPerPhaseRS;

        private string m_ReportFileName;
        private string m_ReportName;
        private string m_ReportPDF;

        private string m_XfmrSizingFilePath;

        private double m_OverrideLoad = 0;
        private double m_FutureServiceLoad = 0;
        private double m_FutureServiceLength = 0;

        private int m_XfmrFID = 0;
        private short m_XfmrFNO = 0;
        private short m_XfmrUnitCNO = 0;

        private int m_FormBeginWidth;
        private int m_FormBeginHeight;
        private bool m_FirstCalculation = true;

        private double m_Tonnage = 0;
        private double m_SummerPowerFactor = 0.9;
        private double m_WinterPowerFactor = 0.95;

        private double m_XfmrVoltageDropPctThreshold = 0;
        private double m_SecVoltageDropPctThreshold = 0;
        private double m_SrvcVoltageDropPctThreshold = 0;
        private double m_SecFlickerPctLowThreshold = 0;
        private double m_SecFlickerPctHighThreshold = 0;
        private double m_SrvcFlickerPctLowThreshold = 0;
        private double m_SrvcFlickerPctHighThreshold = 0;

        internal string[] m_ValidThreePhaseVoltages;

        private Color m_DefaultTextBoxColor;
        private Color m_DefaultReadOnlyCellColor = Color.Gainsboro;
        private Color m_ErrorColor = Color.Red;
        private Color m_WarningColor = Color.Yellow;
        private Color m_PendingChangesColor = Color.Yellow;

        private GTSelectBehaviorConstants m_StartingSelectSetBehavior;

        private CommonDT.XfmrProperties m_XfmrProperties;
        private string m_XfmrType;
        private string m_XfmrCU;
        private double m_XfmrSize = 0;
        private int m_XfmrCustomerCount = 0;
        private double m_XfmrLoadSummer = 0;
        private double m_XfmrLoadWinter = 0;
        private double m_XfmrVoltageDrop = 0;
        private double m_XfmrVoltageDropPct = 0;
        private double m_XfmrVoltageDropVolts = 0;
        private double m_XfmrVlmag = 0;
        private double m_XfmrLoadKVASummer = 0;
        private double m_XfmrLoadKVAWinter = 0;
        private double m_XfmrFutureServiceCount = 0;
        private double m_XfmrFutureServiceLoadSummer = 0;
        private double m_XfmrFutureServiceLoadWinter = 0;
        private bool m_XfmrPendingEdit = false;

        private int m_PhaseCount = 0; // Number of phases for the Transformer. 1 - Run residential calculation; 3 - Run commercial calculation.

        private string m_traceUserName;

        private const string M_DISPLAY_PATHNAME = "Traces - Secondary Calculator";

        private bool m_PassedValidation = false;

        // Secondary Datagridview columns
        private const int M_SECGRID_SPAN = 0;
        private const int M_SECGRID_TYPE = 1;
        private const int M_SECGRID_LENGTH = 2;
        private const int M_SECGRID_VOLTAGE = 3;
        private const int M_SECGRID_SEASON = 4;
        private const int M_SECGRID_FLICKER = 5;
        private const int M_SECGRID_FUTURE_SVC = 6;
        private const int M_SECGRID_FID = 7;
        private const int M_SECGRID_SOURCE_FID = 8;
        private const int M_SECGRID_FUTURE_SVC_LOAD = 9;
        private const int M_SECGRID_FUTURE_SVC_LENGTH = 10;
        private const int M_SECGRID_VOLTAGE_DROP_PCT = 11;
        private const int M_SECGRID_FNO = 12;
        private const int M_SECGRID_PENDINGEDIT = 13;
        private const int M_SECGRID_CU = 14;
        private const int M_SECGRID_LENGTHEDIT = 15;

        // Service Datagridview columns
        private const int M_SRVCGRID_SPAN = 0;
        private const int M_SRVCGRID_TYPE = 1;
        private const int M_SRVCGRID_LENGTH = 2;
        private const int M_SRVCGRID_VOLTAGE = 3;
        private const int M_SRVCGRID_SEASON = 4;
        private const int M_SRVCGRID_FLICKER = 5;
        private const int M_SRVCGRID_SUM_LOAD_ACT = 6;
        private const int M_SRVCGRID_SUM_LOAD_EST = 7;
        private const int M_SRVCGRID_WIN_LOAD_ACT = 8;
        private const int M_SRVCGRID_WIN_LOAD_EST = 9;
        private const int M_SRVCGRID_FID = 10;
        private const int M_SRVCGRID_SOURCE_FID = 11;
        private const int M_SRVCGRID_OVERRIDE_LOAD = 12;
        private const int M_SRVCGRID_LOAD_USED = 13;
        private const int M_SRVCGRID_VOLTAGE_DROP_PCT = 14;
        private const int M_SRVCGRID_PENDINGEDIT = 15;
        private const int M_SRVCGRID_CU = 16;
        private const int M_SRVCGRID_SRVCPT_FID = 17;
        private const int M_SRVCGRID_LENGTHEDIT = 18;

        public frmSecondaryCalculator()
        {
            InitializeComponent();
        }

        private void frmSecondaryCalculator_Load(object sender, EventArgs e)
        {
            if (!InitializeForm())
            {
                Close();
                return;
            }
        }

        private void cmdClose_Click(object sender, EventArgs e)
        {
            Close();
        }

        // The form is closing. Perform cleanup.
        private void frmSecondaryCalculator_FormClosing(object sender, FormClosingEventArgs e)
        {
            // If the Apply command is enabled then there are pending CUs.
            if (cmdApply.Enabled)
            {
                // If the Notify checkbox is checked, then notify user if there are any pending CUs.
                OptionsDT frmDesignToolsOptions = new OptionsDT();

                bool notifyChanges = frmDesignToolsOptions.NotifyChanges;
                frmDesignToolsOptions = null;

                if (notifyChanges)
                {
                    if (MessageBox.Show(m_Application.ApplicationWindow, "There are pending CU changes. Do you want to apply the changes?", "G/Technology", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    {
                        if (!SavePendingChanges())
                        {
                            e.Cancel = true;
                            return;
                        }
                    }
                }
            }

            if (m_TransactionManager != null)
            {
                if (m_TransactionManager.TransactionInProgress)
                {
                    m_TransactionManager.Commit();
                }
            }

            m_CommonDT.RemoveLegendItem("Calculation Problems", "Secondary");

            if (m_clsTraceHelper.TraceName.Length > 0)
            {
                m_clsTraceHelper.RemoveTrace(m_clsTraceHelper.TraceName, M_DISPLAY_PATHNAME, m_clsTraceHelper.TraceName);
            }

            if (!(m_CommonDT.ConductorsDictionary == null))
            {
                m_CommonDT.ConductorsDictionary.Clear();
                m_CommonDT.ConductorsDictionary = null;
            }

            if (!(m_CommonDT.ServicesDictionary == null))
            {
                m_CommonDT.ServicesDictionary.Clear();
                m_CommonDT.ServicesDictionary = null;
            }
            
            m_TonnageRS = null;
            m_CablesPerPhaseRS = null;
            m_clsTraceHelper = null;
            m_clsCalculate = null;
            m_CommonDT = null;
            m_Application.EndWaitCursor();

            // Reset select set behavior.
            m_Application.ActiveMapWindow.SelectBehavior = m_StartingSelectSetBehavior;

            m_CustomCommandHelper.Complete();
        }

        // Initialize the form. Populate the comboboxes and textboxes. Enable/Disable the controls.
        private bool InitializeForm()
        {
            bool returnValue = false;            

            try
            {
                // Process the select set
                IGTDDCKeyObjects oGTDDCKeys = GTClassFactory.Create<IGTDDCKeyObjects>();
                oGTDDCKeys = m_Application.SelectedObjects.GetObjects();
                m_XfmrFID = oGTDDCKeys[0].FID;
                m_XfmrFNO = oGTDDCKeys[0].FNO;

                IGTKeyObject Xfmr = m_Application.DataContext.OpenFeature(m_XfmrFNO, m_XfmrFID);
                IGTComponent XfmrCommon = Xfmr.Components.GetComponent(1);

                // Disable the Transformer CU command if feature isn't part of WR
                if (!CommonDT.AllowCUEdit(Xfmr))
                {
                    cmdXfmrCU.Enabled = false;
                }

                string xfmrType = string.Empty;
                if (m_XfmrFNO == ConstantsDT.FNO_OH_XFMR)
                {
                    m_XfmrUnitCNO = ConstantsDT.CNO_OH_XFMR_UNIT;
                    xfmrType = "OH";
                }
                else
                {
                    m_XfmrUnitCNO = ConstantsDT.CNO_UG_XFMR_UNIT;
                    xfmrType = "UG";
                }

                IGTKeyObject oGTKey = m_Application.DataContext.OpenFeature(m_XfmrFNO, m_XfmrFID);
                IGTComponents oGTComponents = oGTKey.Components;

                // Get the Transformer CU. To be used to lookup the Transformer properties.
                Recordset componentRS = oGTComponents.GetComponent(ConstantsDT.CNO_COMPUNIT).Recordset;

                if (componentRS.RecordCount > 0)
                {
                    m_XfmrCU = componentRS.Fields[ConstantsDT.FIELD_COMPUNIT_CU].Value.ToString();
                }

                // Set the Transformer textboxes
                componentRS = oGTComponents.GetComponent(m_XfmrUnitCNO).Recordset;

                if (componentRS.RecordCount > 0)
                {
                    if (!Convert.IsDBNull(componentRS.Fields[ConstantsDT.FIELD_XFMRUNIT_SIZE].Value))
                    {
                        txtXfmrSize.Text = componentRS.Fields[ConstantsDT.FIELD_XFMRUNIT_SIZE].Value.ToString();
                        m_XfmrSize = Convert.ToDouble(txtXfmrSize.Text);
                    }
                    
                    txtXfmrVoltage.Text = componentRS.Fields[ConstantsDT.FIELD_XFMRUNIT_VOLTAGE_SEC].Value.ToString();

                    if (!Convert.IsDBNull(componentRS.Fields[ConstantsDT.FIELD_XFMRUNIT_PHASE_QUANTITY].Value))
                    {
                        m_PhaseCount = Convert.ToInt32(componentRS.Fields[ConstantsDT.FIELD_XFMRUNIT_PHASE_QUANTITY].Value);
                    }
                    
                    txtXfmrType.Text = xfmrType + " " + m_PhaseCount + " Phase";
                }

                if (m_PhaseCount == 3)
                {
                    grpCommercial.Enabled = true;
                    cboCablesPerPhase.Enabled = true;
                    cboLargestACTonnage.Enabled = false;

                    // Populate the # Cables Per Phase combo box
                    if (!PopulateCablesPerPhase())
                    {
                        return false;
                    }
                }

                // Populate the AC Tonnage combo box
                if (!PopulateTonnageList())
                {
                    return false;
                }

                // Get the metadata for the calculations
                if (!GetToolsMetadata())
                {
                    return false;
                }

                // Initialize the controls
                cmdCalculate.Enabled = false;
                cmdApply.Enabled = false;
                cmdPrintReport.Enabled = false;
                cmdSaveReport.Enabled = false;
                optLoadActual.Checked = true;

                txtPFSummer.Text = "90";
                txtPFWinter.Text = "95";
                cboNeutralVDrop.Text = "Yes";
                m_clsCalculate.NeutralVDropFactor = 2;

                m_SummerPowerFactor = Convert.ToDouble(txtPFSummer.Text) / 100;
                m_clsCalculate.PowerFactorSummer = m_SummerPowerFactor;
                m_CommonDT.SummerPowerFactor = m_SummerPowerFactor;

                m_WinterPowerFactor = Convert.ToDouble(txtPFWinter.Text) / 100;
                m_clsCalculate.PowerFactorWinter = m_WinterPowerFactor;

                // Capture the backcolor of the textbox. To be used later when resetting the backcolor.
                m_DefaultTextBoxColor = txtXfmrVoltageDrop.BackColor;

                // Capture the select set behavior so it can be reset when the command exits.
                m_StartingSelectSetBehavior = m_Application.ActiveMapWindow.SelectBehavior;

                // Resize the form to hide the grids
                m_FormBeginHeight = this.Size.Height;
                m_FormBeginWidth = this.Size.Width;
                this.Size = new Size(this.Width, dgvSecondary.Location.Y + 35);
                this.FormBorderStyle = FormBorderStyle.FixedSingle;

                dgvSecondary.Visible = false;
                dgvService.Visible = false;

                dgvSecondary.RowHeadersWidth = 10;
                dgvService.RowHeadersWidth = 10;

                dgvSecondary.ClearSelection();
                dgvService.ClearSelection();

                returnValue = true;
            }
            catch (Exception ex)
            {
                returnValue = false;
                MessageBox.Show(m_Application.ApplicationWindow, ConstantsDT.ERROR_SC_FORM_INITIALIZATION + ": " + ex.Message, ConstantsDT.APPLICATION_NAME, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }

            return returnValue;
        }

        // Trace the secondary network. Perform calculations and display the results.
        private void cmdCalculate_Click(object sender, EventArgs e)
        {
            try
            {
                m_Application.BeginWaitCursor();

                if (m_FirstCalculation)
                {
                    // Setup the trace
                    m_clsTraceHelper.SeedFID = m_XfmrFID;
                    m_clsTraceHelper.TraceMetadataUserName = m_traceUserName;
                    m_clsTraceHelper.ApplicationName = ConstantsDT.COMMAND_NAME_SECONDARY_CALCULATOR;

                    m_Application.SetStatusBarText(GTStatusPanelConstants.gtaspcMessage, ConstantsDT.MESSAGE_TRACE_EXECUTING);                    

                    // Trace the network
                    if (!m_clsTraceHelper.ExecuteTrace())
                    {
                        m_Application.SetStatusBarText(GTStatusPanelConstants.gtaspcMessage, "");
                        m_Application.EndWaitCursor();
                        this.Close();
                        return;
                    }

                    // Process the trace results
                    m_Application.SetStatusBarText(GTStatusPanelConstants.gtaspcMessage, ConstantsDT.MESSAGE_TRACE_PROCESSING_RESULTS);
                    if (!m_CommonDT.ProcessTraceResults(m_clsTraceHelper.TraceName))
                    {
                        m_Application.SetStatusBarText(GTStatusPanelConstants.gtaspcMessage, "");
                        m_Application.EndWaitCursor();
                        this.Close();
                        return;
                    }

                    m_XfmrCustomerCount = m_CommonDT.XfmrCustomerCount;

                    // Resize the form to show the grids
                    this.FormBorderStyle = FormBorderStyle.Sizable;
                    this.Size = new Size(m_FormBeginWidth, m_FormBeginHeight);
                    dgvSecondary.Visible = true;
                    dgvService.Visible = true;
                    m_FirstCalculation = false;
                }

                // Call function to add trace results to legend if output to map window option has been selected.
                m_CommonDT.DisplayResults(m_CommonDT.TraceResultsRS, M_DISPLAY_PATHNAME, m_clsTraceHelper.TraceName, CommonDT.DisplayResultsType.Trace);

                // Run the calculations
                m_Application.SetStatusBarText(GTStatusPanelConstants.gtaspcMessage, ConstantsDT.MESSAGE_CALCULATING);
                
                if (!UpdateLoad())
                {
                    m_Application.SetStatusBarText(GTStatusPanelConstants.gtaspcMessage, "");
                    m_Application.EndWaitCursor();
                    return;
                }

                if (!Calculate())
                {
                    m_Application.SetStatusBarText(GTStatusPanelConstants.gtaspcMessage, "");
                    m_Application.EndWaitCursor();
                    return;
                }

                //dgvSecondary.Rows.Clear();
                //dgvService.Rows.Clear();

                // Add the results to the grids
                if (dgvSecondary.RowCount == 0)
                {
                    // Add the results to the grids
                    if (!PopulateGrids())
                    {
                        m_Application.SetStatusBarText(GTStatusPanelConstants.gtaspcMessage, "");
                        m_Application.EndWaitCursor();
                        return;
                    }
                }
                else
                {
                    // Update the results in the grids
                    if (!UpdateGrids())
                    {
                        m_Application.SetStatusBarText(GTStatusPanelConstants.gtaspcMessage, "");
                        m_Application.EndWaitCursor();
                        return;
                    }
                }

                // Validate the results
                m_Application.SetStatusBarText(GTStatusPanelConstants.gtaspcMessage, ConstantsDT.MESSAGE_VALIDATING);
                if (!ValidateResults())
                {
                    m_Application.SetStatusBarText(GTStatusPanelConstants.gtaspcMessage, "");
                    m_Application.EndWaitCursor();
                    return;
                }
                m_Application.SetStatusBarText(GTStatusPanelConstants.gtaspcMessage, "");

                dgvSecondary.ClearSelection();
                dgvService.ClearSelection();

                cmdPrintReport.Enabled = true;

                m_Application.EndWaitCursor();
            }
            catch (Exception ex)
            {
                m_Application.EndWaitCursor();
                MessageBox.Show(m_Application.ApplicationWindow, ConstantsDT.ERROR_SC_CALCULATE + ": " + ex.Message, ConstantsDT.APPLICATION_NAME, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }
        
        // Update the load values at the Service Point.
        // Rollup the values to the feeding Service Line, upstream Conductors and Transformer.
        private bool UpdateLoad()
        {
            bool returnValue = false;   
            
            try
            {
                if (m_CommonDT.ConductorsDictionary != null)
                {
                    // Clear Conductor load values
                    foreach (Conductor conductor in m_CommonDT.ConductorsDictionary.Values)
                    {
                        conductor.CustomerCount = 0;
                        conductor.LoadSummerActual = 0;
                        conductor.LoadSummerEstimated = 0;
                        conductor.LoadWinterActual = 0;
                        conductor.LoadWinterEstimated = 0;
                        conductor.LoadSummer = 0;
                        conductor.LoadWinter = 0;
                        conductor.FutureServiceCount = 0;
                        conductor.FutureServiceCountAccum = 0;
                        conductor.FutureServiceLoad = 0;
                    }
                }

                // Clear the Transformer load values
                m_XfmrLoadSummer = 0;
                m_XfmrLoadWinter = 0;
                m_XfmrFutureServiceLoadSummer = 0;

                Conductor sourceConductor;
                
                double loadSummer = 0;
                double loadWinter = 0;
                string loadUsedSummer = string.Empty;
                string loadUsedWinter = string.Empty;

                // Update the load for each service to use the correct load based on the load options and service point load values.
                if (m_CommonDT.ServicesDictionary != null)
                {
                    foreach (Service service in m_CommonDT.ServicesDictionary.Values)
                    {
                        if (service.ServicePoint != null)
                        {
                            if (!GetLoadToUse(service.ServicePoint.LoadSummerActual, service.ServicePoint.LoadSummerEstimated, service.ServicePoint.LoadWinterActual,
                                            service.ServicePoint.LoadWinterEstimated, ref loadSummer, ref loadWinter, ref loadUsedSummer, ref loadUsedWinter))
                            {
                                return false;
                            }

                            service.ServicePoint.LoadUsedSummer = loadUsedSummer;
                            service.ServicePoint.LoadUsedWinter = loadUsedWinter;
                            service.ServicePoint.LoadSummer = loadSummer;
                            service.ServicePoint.LoadWinter = loadWinter;
                            service.LoadSummer = loadSummer;
                            service.LoadWinter = loadWinter;

                            // Update the load for each conductor that feeds the service
                            if (m_CommonDT.ConductorsDictionary.TryGetValue(service.SourceFID, out sourceConductor))
                            {
                                sourceConductor.LoadSummer += loadSummer;
                                sourceConductor.LoadWinter += loadWinter;
                                sourceConductor.CustomerCount += 1;
                                // Update the load for the conductor that feeds the service's conductor
                                UpdateLoad(sourceConductor, loadSummer, loadWinter);
                            }
                        }
                    }
                }                

                Conductor grdConductor;
                Conductor grdUpstreamConductor;
                int fid = 0;
                double futureServices = 0;

                // Update the future load values
                for (int i = dgvSecondary.Rows.Count - 1; i >= 0; i--)
                {
                    fid = Convert.ToInt32(dgvSecondary.Rows[i].Cells[M_SECGRID_FID].Value);

                    if (!Convert.IsDBNull(dgvSecondary.Rows[i].Cells[M_SECGRID_FUTURE_SVC].Value))
                    {
                        futureServices = Convert.ToDouble(dgvSecondary.Rows[i].Cells[M_SECGRID_FUTURE_SVC].Value);
                    }

                    if (m_CommonDT.ConductorsDictionary.TryGetValue(fid, out grdConductor))
                    {
                        grdConductor.FutureServiceCount = futureServices;
                        grdConductor.FutureServiceCountAccum += futureServices;
                        grdConductor.FutureServiceLoad += futureServices * m_FutureServiceLoad;
                        grdConductor.FutureServiceLength = m_FutureServiceLength;

                        if (m_CommonDT.ConductorsDictionary.TryGetValue(grdConductor.SourceFID, out grdUpstreamConductor))
                        {
                            grdUpstreamConductor.FutureServiceCountAccum += grdConductor.FutureServiceCountAccum;
                            grdUpstreamConductor.FutureServiceLoad += grdConductor.FutureServiceCountAccum * m_FutureServiceLoad;
                        }
                    }
                }

                // Update the Transformer future load values
                foreach (Conductor conductor in m_CommonDT.ConductorsDictionary.Values)
                {
                    if (conductor.SourceFID == m_XfmrFID)
                    {
                        m_XfmrLoadSummer += conductor.LoadSummer;
                        m_XfmrLoadWinter += conductor.LoadWinter;
                        m_XfmrFutureServiceLoadSummer += conductor.FutureServiceLoad;
                    }
                }                   

                returnValue = true;
            }
            catch (Exception ex)
            {
                returnValue = false;
                MessageBox.Show(m_Application.ApplicationWindow, ConstantsDT.ERROR_SC_UPDATING_LOAD + ": " + ex.Message, ConstantsDT.APPLICATION_NAME, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }

            return returnValue;
        }

        // Recursive function to rollup the load values and customer counts to the source conductor
        private bool UpdateLoad(Conductor conductor, double loadSummer, double loadWinter)
        {            
            bool returnValue = false;

            try
            {
                Conductor sourceConductor;
                if (m_CommonDT.ConductorsDictionary.TryGetValue(conductor.SourceFID, out sourceConductor))
                {
                    sourceConductor.LoadSummer += loadSummer;
                    sourceConductor.LoadWinter += loadWinter;
                    sourceConductor.CustomerCount += 1;
                    UpdateLoad(sourceConductor, loadSummer, loadWinter);
                }

                returnValue = true;
            }
            catch (Exception ex)
            {
                returnValue = false;
                MessageBox.Show(m_Application.ApplicationWindow, ConstantsDT.ERROR_SC_UPDATING_LOAD + ": " + ex.Message, ConstantsDT.APPLICATION_NAME, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }

            return returnValue;
        }

        // Determine the load to use based on the load options selected and if the load values are populated.
        // In the case where the selected load is not available then the load value will be retrieved in the following order: Actual, Estimated. 
        // For example if Estimated is selected but the value is not populated on the Service Point 
        //  then the Actual value from the Service Point will be used if populated. If the Override value is populated then it will be used.
        private bool GetLoadToUse(double summerLoadActual, double summerLoadEstimated, double winterLoadActual, double winterLoadEstimated,
                          ref double summerLoad, ref double winterLoad, ref string loadUsedSummer, ref string loadUsedWinter)
        {
            bool returnValue = false;

            try
            {
                if (optLoadActual.Checked)
                {
                    if (summerLoadActual != 0)
                    {
                        summerLoad = summerLoadActual;
                        loadUsedSummer = "Actual";
                    }
                    else if (summerLoadEstimated != 0)
                    {
                        summerLoad = summerLoadEstimated;
                        loadUsedSummer = "Estimated";
                    }
                    else
                    {
                        summerLoad = 0;
                        loadUsedSummer = "";
                    }

                    if (m_OverrideLoad != 0)
                    {
                        summerLoad = m_OverrideLoad;
                        loadUsedSummer = "Override";
                    }

                    if (winterLoadActual != 0)
                    {
                        winterLoad = winterLoadActual;
                        loadUsedWinter = "Actual";
                    }
                    else if (winterLoadEstimated != 0)
                    {
                        winterLoad = winterLoadEstimated;
                        loadUsedWinter = "Estimated";
                    }
                    else
                    {
                        winterLoad = 0;
                        loadUsedWinter = "";
                    }

                    if (m_OverrideLoad != 0)
                    {
                        winterLoad = m_OverrideLoad;
                        loadUsedWinter = "Override";
                    }
                }
                else
                {
                    if (summerLoadEstimated != 0)
                    {
                        summerLoad = summerLoadEstimated;
                        loadUsedSummer = "Estimated";
                    }
                    else if (summerLoadActual != 0)
                    {
                        summerLoad = summerLoadActual;
                        loadUsedSummer = "Actual";
                    }
                    else
                    {
                        summerLoad = 0;
                        loadUsedSummer = "";
                    }

                    if (m_OverrideLoad != 0)
                    {
                        summerLoad = m_OverrideLoad;
                        loadUsedSummer = "Override";
                    }                    

                    if (winterLoadEstimated != 0)
                    {
                        winterLoad = winterLoadEstimated;
                        loadUsedWinter = "Estimated";
                    }
                    else if (winterLoadActual != 0)
                    {
                        winterLoad = winterLoadActual;
                        loadUsedWinter = "Actual";
                    }
                    else
                    {
                        winterLoad = 0;
                        loadUsedWinter = "";
                    }

                    if (m_OverrideLoad != 0)
                    {
                        winterLoad = m_OverrideLoad;
                        loadUsedWinter = "Override";
                    }
                }

                returnValue = true;
            }
            catch (Exception ex)
            {
                returnValue = false;
                MessageBox.Show(m_Application.ApplicationWindow, ConstantsDT.ERROR_SC_DETERMINING_LOAD + ": " + ex.Message, ConstantsDT.APPLICATION_NAME, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }

            return returnValue;
        }

        // Add the Conductor and Service instances to the grids
        private bool PopulateGrids()
        {
            bool returnValue = false;

            try
            {
                if (m_CommonDT.ConductorsDictionary != null)
                {
                    foreach (Conductor conductor in m_CommonDT.ConductorsDictionary.Values)
                    {
                        if (!AddConductorToGrid(conductor, conductor.SpanIndex))
                        {
                            return false;
                        }
                    }
                }

                if (!(m_CommonDT.ServicesDictionary == null))
                {
                    foreach (Service service in m_CommonDT.ServicesDictionary.Values)
                    {
                        if (!AddServiceToGrid(service, service.SpanIndex))
                        {
                            return false;
                        }
                    }
                }

                SetCellAccess();

                returnValue = true;
            }
            catch (Exception ex)
            {
                returnValue = false;
                MessageBox.Show(m_Application.ApplicationWindow, ConstantsDT.ERROR_SC_GRID_POPULATION + ": " + ex.Message, ConstantsDT.APPLICATION_NAME, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }

            return returnValue;
        }

        /// <summary>
        /// Sets the CU and length cells to read-only if feature isn't part of WR
        /// </summary>
        /// <returns>Boolean indicating status</returns>
        private bool SetCellAccess()
        {
            bool returnValue = false;

            try
            {
                ADODB.Recordset wrRS = null;
                m_CommonDT.GetWrData(ref wrRS);
                int fid;

                if (m_CommonDT.ConductorsDictionary != null)
                {
                    // Set appropriate cells to read-only if feature isn't part of WR.
                    foreach (DataGridViewRow r in dgvSecondary.Rows)
                    {
                        // Check if current Secondary Conductor FID is part of WR
                        fid = Convert.ToInt32(r.Cells[M_SECGRID_FID].Value);

                        if (wrRS.RecordCount > 0)
                        {
                            wrRS.Filter = "g3e_fid = " + fid;
                        }

                        if (wrRS.RecordCount == 0)
                        {
                            r.Cells[M_SECGRID_TYPE].Style.BackColor = m_DefaultReadOnlyCellColor;
                            r.Cells[M_SECGRID_TYPE].ReadOnly = true;

                            r.Cells[M_SECGRID_LENGTH].Style.BackColor = m_DefaultReadOnlyCellColor;
                            r.Cells[M_SECGRID_LENGTH].ReadOnly = true;
                        }
                    }
                }

                if (m_CommonDT.ServicesDictionary != null)
                {
                    // Set appropriate cells to read-only if feature isn't part of WR.
                    foreach (DataGridViewRow r in dgvService.Rows)
                    {
                        // Check if current Service Line FID is part of WR
                        fid = Convert.ToInt32(r.Cells[M_SRVCGRID_FID].Value);

                        if (wrRS.RecordCount > 0)
                        {
                            wrRS.Filter = "g3e_fid = " + fid;
                        }

                        if (wrRS.RecordCount == 0)
                        {
                            r.Cells[M_SRVCGRID_TYPE].Style.BackColor = m_DefaultReadOnlyCellColor;
                            r.Cells[M_SRVCGRID_TYPE].ReadOnly = true;

                            r.Cells[M_SRVCGRID_LENGTH].Style.BackColor = m_DefaultReadOnlyCellColor;
                            r.Cells[M_SRVCGRID_LENGTH].ReadOnly = true;
                        }
                    }
                }

                returnValue = true;
            }
            catch (Exception ex)
            {
                returnValue = false;
                MessageBox.Show(m_Application.ApplicationWindow, ConstantsDT.ERROR_SC_GRID_POPULATION + ": " + ex.Message, ConstantsDT.APPLICATION_NAME, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }

            return returnValue;
        }

        private bool UpdateGrids()
        {
            bool returnValue = false;

            try
            {
                Int32 fid = 0;
                Conductor conductor = null;

                foreach (DataGridViewRow r in dgvSecondary.Rows)
                {
                    fid = Convert.ToInt32(r.Cells[M_SECGRID_FID].Value);

                    if (m_CommonDT.ConductorsDictionary.TryGetValue(fid, out conductor))
                    {
                        r.Cells[M_SECGRID_VOLTAGE].Value = conductor.Voltage;
                        r.Cells[M_SECGRID_SEASON].Value = conductor.Season;
                        r.Cells[M_SECGRID_FLICKER].Value = conductor.Flicker;
                        r.Cells[M_SECGRID_FUTURE_SVC].Value = conductor.FutureServiceCount;
                        r.Cells[M_SECGRID_FUTURE_SVC_LOAD].Value = conductor.FutureServiceLoad;
                        r.Cells[M_SECGRID_FUTURE_SVC_LENGTH].Value = conductor.FutureServiceLength;
                        r.Cells[M_SECGRID_VOLTAGE_DROP_PCT].Value = conductor.VoltageDropPercent;
                    }
                }

                Service service = null;
                foreach (DataGridViewRow r in dgvService.Rows)
                {
                    fid = Convert.ToInt32(r.Cells[M_SRVCGRID_FID].Value);

                    if (m_CommonDT.ServicesDictionary.TryGetValue(fid, out service))
                    {
                        r.Cells[M_SRVCGRID_VOLTAGE].Value = service.Voltage;
                        r.Cells[M_SRVCGRID_SEASON].Value = service.Season;
                        r.Cells[M_SRVCGRID_FLICKER].Value = service.Flicker;

                        if (service.ServicePoint != null)
                        {
                            r.Cells[M_SRVCGRID_SUM_LOAD_ACT].Value = service.ServicePoint.LoadSummerActual;
                            r.Cells[M_SRVCGRID_SUM_LOAD_EST].Value = service.ServicePoint.LoadSummerEstimated;
                            r.Cells[M_SRVCGRID_WIN_LOAD_ACT].Value = service.ServicePoint.LoadWinterActual;
                            r.Cells[M_SRVCGRID_WIN_LOAD_EST].Value = service.ServicePoint.LoadWinterEstimated;
                            r.Cells[M_SRVCGRID_VOLTAGE_DROP_PCT].Value = service.VoltageDropPercent;

                            if (service.Season == "Summer")
                            {
                                r.Cells[M_SRVCGRID_LOAD_USED].Value = service.ServicePoint.LoadUsedSummer;
                            }
                            else
                            {
                                r.Cells[M_SRVCGRID_LOAD_USED].Value = service.ServicePoint.LoadUsedWinter;
                            }

                            if (m_OverrideLoad != 0)
                            {
                                r.Cells[M_SRVCGRID_OVERRIDE_LOAD].Value = m_OverrideLoad;
                            }
                            else
                            {
                                r.Cells[M_SRVCGRID_OVERRIDE_LOAD].Value = "n/a";
                            }
                        }
                    }
                }

                returnValue = true;
            }
            catch (Exception ex)
            {
                returnValue = false;
                MessageBox.Show(m_Application.ApplicationWindow, ConstantsDT.ERROR_SC_GRID_POPULATION + ": " + ex.Message, ConstantsDT.APPLICATION_NAME, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }

            return returnValue;
        }

        // Add a Conductor to the Secondary Conductor grid
        private bool AddConductorToGrid(Conductor conductor, int spanIndex)
        {
            bool returnValue = false;

            try
            {
                DataGridViewRow r = dgvSecondary.Rows[dgvSecondary.Rows.Add()];
                r.Cells[M_SECGRID_SPAN].Value = spanIndex;
                r.Cells[M_SECGRID_TYPE].Value = conductor.CableType;
                r.Cells[M_SECGRID_LENGTH].Value = conductor.Length;
                r.Cells[M_SECGRID_VOLTAGE].Value = conductor.Voltage;
                r.Cells[M_SECGRID_SEASON].Value = conductor.Season;
                r.Cells[M_SECGRID_FLICKER].Value = conductor.Flicker;
                r.Cells[M_SECGRID_FUTURE_SVC].Value = conductor.FutureServiceCount;
                r.Cells[M_SECGRID_FID].Value = conductor.ConductorFID;
                r.Cells[M_SECGRID_FNO].Value = conductor.FNO;
                r.Cells[M_SECGRID_SOURCE_FID].Value = conductor.SourceFID;
                r.Cells[M_SECGRID_FUTURE_SVC_LOAD].Value = conductor.FutureServiceLoad;
                r.Cells[M_SECGRID_FUTURE_SVC_LENGTH].Value = conductor.FutureServiceLength;
                r.Cells[M_SECGRID_VOLTAGE_DROP_PCT].Value = conductor.VoltageDropPercent;
                r.Cells[M_SECGRID_PENDINGEDIT].Value = 0;
                r.Cells[M_SECGRID_CU].Value = conductor.CU;
                r.Cells[M_SECGRID_LENGTHEDIT].Value = 0;

                if (!conductor.AllowCUEdit)
                {
                    r.Cells[M_SECGRID_TYPE].ReadOnly = true;
                    r.Cells[M_SECGRID_TYPE].Style.BackColor = m_DefaultReadOnlyCellColor;
                }

                returnValue = true;
            }
            catch (Exception ex)
            {
                returnValue = false;
                MessageBox.Show(m_Application.ApplicationWindow, ConstantsDT.ERROR_SC_GRID_ADD_CONDUCTOR + ": " + ex.Message, ConstantsDT.APPLICATION_NAME, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }

            return returnValue;
        }

        // Add a Service to the Service grid
        private bool AddServiceToGrid(Service service, int spanIndex)
        {
            bool returnValue = false;

            try
            {
                DataGridViewRow r = dgvService.Rows[dgvService.Rows.Add()];
                r.Cells[M_SRVCGRID_SPAN].Value = spanIndex;
                r.Cells[M_SRVCGRID_TYPE].Value = service.CableType;
                r.Cells[M_SRVCGRID_LENGTH].Value = service.Length;
                r.Cells[M_SRVCGRID_VOLTAGE].Value = service.Voltage;
                r.Cells[M_SRVCGRID_SEASON].Value = service.Season;
                r.Cells[M_SRVCGRID_FLICKER].Value = service.Flicker;
                r.Cells[M_SRVCGRID_FID].Value = service.ServiceFID;
                r.Cells[M_SRVCGRID_SOURCE_FID].Value = service.SourceFID;
                r.Cells[M_SRVCGRID_PENDINGEDIT].Value = 0;
                r.Cells[M_SRVCGRID_CU].Value = service.CU;
                r.Cells[M_SRVCGRID_LENGTHEDIT].Value = 0;

                if (!service.AllowCUEdit)
                {
                    r.Cells[M_SRVCGRID_TYPE].ReadOnly = true;
                    r.Cells[M_SRVCGRID_TYPE].Style.BackColor = m_DefaultReadOnlyCellColor;
                }

                if (service.ServicePoint != null)
                {
                    r.Cells[M_SRVCGRID_SUM_LOAD_ACT].Value = service.ServicePoint.LoadSummerActual;
                    r.Cells[M_SRVCGRID_SUM_LOAD_EST].Value = service.ServicePoint.LoadSummerEstimated;
                    r.Cells[M_SRVCGRID_WIN_LOAD_ACT].Value = service.ServicePoint.LoadWinterActual;
                    r.Cells[M_SRVCGRID_WIN_LOAD_EST].Value = service.ServicePoint.LoadWinterEstimated;
                    r.Cells[M_SRVCGRID_VOLTAGE_DROP_PCT].Value = service.VoltageDropPercent;
                    r.Cells[M_SRVCGRID_SRVCPT_FID].Value = service.ServicePoint.ServicePointFID;

                    if (service.Season == "Summer")
                    {
                        r.Cells[M_SRVCGRID_LOAD_USED].Value = service.ServicePoint.LoadUsedSummer;
                    }
                    else
                    {
                        r.Cells[M_SRVCGRID_LOAD_USED].Value = service.ServicePoint.LoadUsedWinter;
                    }

                    if (m_OverrideLoad != 0)
                    {
                        r.Cells[M_SRVCGRID_OVERRIDE_LOAD].Value = m_OverrideLoad;
                    }
                    else
                    {
                        r.Cells[M_SRVCGRID_OVERRIDE_LOAD].Value = "n/a";
                    }
                }
                
                returnValue = true;
            }
            catch (Exception ex)
            {
                returnValue = false;
                MessageBox.Show(m_Application.ApplicationWindow, ConstantsDT.ERROR_SC_GRID_ADD_SERVICE + ": " + ex.Message, ConstantsDT.APPLICATION_NAME, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }

            return returnValue;
        }

        // Call functions to perform the voltage drop and flicker calculations
        private bool Calculate()
        {            
            bool returnValue = false;

            try
            {
                // Reset the results for a new calculation.
                ResetResultsForCalculation();
                
                double adjustedLoadSummer = m_XfmrLoadSummer + m_XfmrFutureServiceLoadSummer;
                double adjustedLoadWinter = m_XfmrLoadWinter + m_XfmrFutureServiceLoadWinter;
                double adjustedCustomerCount = m_XfmrCustomerCount + m_XfmrFutureServiceCount;

                // Calculate the voltage drop for the transformer.
                if (!m_clsCalculate.LoadKVA(adjustedLoadSummer, adjustedLoadWinter, adjustedCustomerCount, ref m_XfmrLoadKVASummer, ref m_XfmrLoadKVAWinter))
                {
                    returnValue = false;
                }
                
                if (m_PhaseCount != 3)
                {
                    if (!m_clsCalculate.TransformerVoltageDrop(m_XfmrSize, m_XfmrLoadKVASummer, m_XfmrLoadKVAWinter, ref m_XfmrVoltageDrop,
                                                           ref m_XfmrVoltageDropVolts, ref m_XfmrVoltageDropPct, ref m_XfmrVlmag))
                    {
                        returnValue = false;
                    }
                }
                else
                {
                    if (!m_clsCalculate.TransformerVoltageDropCommercial(adjustedLoadSummer, adjustedLoadWinter, ref m_XfmrVoltageDrop,
                                                                         ref m_XfmrVoltageDropVolts, ref m_XfmrVoltageDropPct, ref m_XfmrVlmag))
                    {
                        returnValue = false;
                    }
                }                

                txtXfmrVoltageDrop.Text = Math.Round(m_XfmrVoltageDrop, 1).ToString();
                double accumXfmrCondResistance = m_XfmrProperties.Resistance;
                double accumXfmrCondReactance = m_XfmrProperties.Reactance;

                if (m_CommonDT.ConductorsDictionary.Count > 0)
                {
                    foreach (Conductor conductor in m_CommonDT.ConductorsDictionary.Values)
                    {
                        if (conductor.SourceFID == m_XfmrFID)
                        {
                            accumXfmrCondResistance += conductor.Resistance;
                            accumXfmrCondReactance += conductor.Reactance;

                            if (m_PhaseCount != 3)
                            {
                                if (!CalculateFlicker(conductor, ref accumXfmrCondResistance, ref accumXfmrCondReactance))
                                {
                                    return false;
                                }
                            }
                            
                            if (!CalculateVoltageDrop(conductor, m_XfmrVlmag, m_XfmrVoltageDrop, m_XfmrVoltageDropPct))
                            {
                                return false;
                            }
                        }
                    }
                }

                returnValue = true;
            }
            catch (Exception ex)
            {
                returnValue = false;
                MessageBox.Show(m_Application.ApplicationWindow, ConstantsDT.ERROR_SC_CALCULATE + ": " + ex.Message, ConstantsDT.APPLICATION_NAME, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }

            return returnValue;
        }

        // Color Tranformer Voltage Drop textbox and the grids voltage drop and flicker cells where thresholds are exceeded.
        //  Write the results to the validation table. 
        private bool ValidateResults()
        {            
            bool returnValue = false;

            try
            {
                short fno = 0;
                int fid = 0;
                string flicker = string.Empty;
                string voltageDropPct = string.Empty;
                string validationStatus = ConstantsDT.VALIDATION_PASS;
                string validationPriority = "";
                string comments = "";
                Recordset errorsRS = null;

                voltageDropPct = Math.Round(m_XfmrVoltageDropPct, 2).ToString();

                if (m_XfmrVoltageDropPct > m_XfmrVoltageDropPctThreshold * 100)
                {
                    txtXfmrVoltageDrop.BackColor = m_ErrorColor;
                    voltageDropPct += " - EXCEEDED";
                    validationStatus = ConstantsDT.VALIDATION_FAIL;
                    validationPriority = ConstantsDT.VALIDATION_PRIORITY;
                }

                comments = "Voltage Drop: " + voltageDropPct + ";";
                WriteValidationResults(m_XfmrFNO, m_XfmrFID, validationStatus, validationPriority, comments, ref errorsRS);

                foreach (DataGridViewRow r in dgvSecondary.Rows)
                {
                    validationStatus = ConstantsDT.VALIDATION_PASS;
                    validationPriority = "";
                    comments = "";

                    fno = Convert.ToInt16(r.Cells[M_SECGRID_FNO].Value);
                    fid = Convert.ToInt32(r.Cells[M_SECGRID_FID].Value);

                    if (r.Cells[M_SECGRID_VOLTAGE_DROP_PCT].Value != null)
                    {
                        voltageDropPct = Math.Round(Convert.ToDouble(r.Cells[M_SECGRID_VOLTAGE_DROP_PCT].Value), 2).ToString();

                        if (Convert.ToDouble(r.Cells[M_SECGRID_VOLTAGE_DROP_PCT].Value) > m_SecVoltageDropPctThreshold * 100)
                        {
                            r.Cells[M_SECGRID_VOLTAGE].Style.BackColor = m_ErrorColor;
                            voltageDropPct += " - EXCEEDED";
                            validationStatus = ConstantsDT.VALIDATION_FAIL;
                            validationPriority = ConstantsDT.VALIDATION_PRIORITY;
                        }
                    }
                    else
                    {
                        voltageDropPct = "0";
                    }

                    if (r.Cells[M_SECGRID_FLICKER].Value != null)
                    {
                        flicker = Math.Round(Convert.ToDouble(r.Cells[M_SECGRID_FLICKER].Value) * 100, 2).ToString();
                        if (Convert.ToDouble(r.Cells[M_SECGRID_FLICKER].Value) > m_SecFlickerPctHighThreshold)
                        {
                            r.Cells[M_SECGRID_FLICKER].Style.BackColor = m_ErrorColor;
                            flicker += " - EXCEEDED";
                            validationStatus = ConstantsDT.VALIDATION_FAIL;
                            validationPriority = ConstantsDT.VALIDATION_PRIORITY;
                        }
                        else if (Convert.ToDouble(r.Cells[M_SECGRID_FLICKER].Value) > m_SecFlickerPctLowThreshold)
                        {
                            r.Cells[M_SECGRID_FLICKER].Style.BackColor = m_WarningColor;
                        }
                    }
                    else
                    {
                        flicker = "0";
                    }

                    comments = "Voltage Drop: " + voltageDropPct + "; Flicker: " + flicker + ";";
                    WriteValidationResults(fno, fid, validationStatus, validationPriority, comments, ref errorsRS);
                }

                foreach (DataGridViewRow r in dgvService.Rows)
                {
                    validationStatus = ConstantsDT.VALIDATION_PASS;
                    validationPriority = "";
                    comments = "";

                    fno = ConstantsDT.FNO_SRVCCOND;
                    fid = Convert.ToInt32(r.Cells[M_SRVCGRID_FID].Value);

                    if (r.Cells[M_SRVCGRID_VOLTAGE_DROP_PCT].Value != null)
                    {
                        voltageDropPct = Math.Round(Convert.ToDouble(r.Cells[M_SRVCGRID_VOLTAGE_DROP_PCT].Value), 2).ToString();

                        if (Convert.ToDouble(r.Cells[M_SRVCGRID_VOLTAGE_DROP_PCT].Value) > m_SrvcVoltageDropPctThreshold * 100)
                        {
                            r.Cells[M_SRVCGRID_VOLTAGE].Style.BackColor = m_ErrorColor;
                            voltageDropPct += " - EXCEEDED";
                            validationStatus = ConstantsDT.VALIDATION_FAIL;
                            validationPriority = ConstantsDT.VALIDATION_PRIORITY;
                        }
                    }
                    else
                    {
                        voltageDropPct = "0";
                    }

                    if (r.Cells[M_SRVCGRID_FLICKER].Value != null)
                    {
                        flicker = Math.Round(Convert.ToDouble(r.Cells[M_SRVCGRID_FLICKER].Value) * 100, 2).ToString();

                        if (Convert.ToDouble(r.Cells[M_SRVCGRID_FLICKER].Value) > m_SrvcFlickerPctHighThreshold)
                        {
                            r.Cells[M_SRVCGRID_FLICKER].Style.BackColor = m_ErrorColor;
                            flicker += " - EXCEEDED";
                            validationStatus = ConstantsDT.VALIDATION_FAIL;
                            validationPriority = ConstantsDT.VALIDATION_PRIORITY;
                        }
                        else if (Convert.ToDouble(r.Cells[M_SRVCGRID_FLICKER].Value) > m_SrvcFlickerPctLowThreshold)
                        {
                            r.Cells[M_SRVCGRID_FLICKER].Style.BackColor = m_WarningColor;
                        }
                    }
                    else
                    {
                        flicker = "0";
                    }

                    comments = "Voltage Drop: " + voltageDropPct + "; Flicker: " + flicker + ";";
                    WriteValidationResults(fno, fid, validationStatus, validationPriority, comments, ref errorsRS);
                }

                // Commit validation records to database.
                int recordsAffected = 0;
                m_Application.DataContext.Execute("commit", out recordsAffected, (int)CommandTypeEnum.adCmdText);

                if (errorsRS != null)
                {
                    m_CommonDT.DisplayResults(errorsRS, "Calculation Problems", "Secondary", CommonDT.DisplayResultsType.Problem);
                }
                else
                {
                    m_PassedValidation = true;
                }

                returnValue = true;
            }
            catch (Exception ex)
            {
                returnValue = false;
                MessageBox.Show(m_Application.ApplicationWindow, ConstantsDT.ERROR_SC_VALIDATION + ": " + ex.Message, ConstantsDT.APPLICATION_NAME, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }

            return returnValue;
        }

        // Write the validation results to the validation log table
        private bool WriteValidationResults(short fno, Int32 fid, string validationStatus, string validationPriority, string comments, ref Recordset errorsRS)
        {
            bool returnValue = false;

            try
            {
                // Check if record exists for WR Number, FID and command
                Recordset validationRS = m_Application.DataContext.OpenRecordset(ConstantsDT.QUERY_VALIDATION, CursorTypeEnum.adOpenDynamic,
                               LockTypeEnum.adLockOptimistic, (int)CommandTypeEnum.adCmdText, m_Application.DataContext.ActiveJob, fid, ConstantsDT.COMMAND_NAME_SECONDARY_CALCULATOR);

                int recordsAffected = 0;

                if (validationRS.RecordCount > 0)
                {
                    // Update record
                    m_Application.DataContext.Execute(ConstantsDT.SQL_UPDATE_VALIDATION, out recordsAffected, (int)CommandTypeEnum.adCmdText,
                                                      validationStatus, validationPriority, comments, m_Application.DataContext.ActiveJob, fid, ConstantsDT.COMMAND_NAME_SECONDARY_CALCULATOR);
                }
                else
                {
                    // Add record
                    m_Application.DataContext.Execute(ConstantsDT.SQL_INSERT_VALIDATION, out recordsAffected, (int)CommandTypeEnum.adCmdText,
                                                      m_Application.DataContext.ActiveJob, fid, ConstantsDT.COMMAND_NAME_SECONDARY_CALCULATOR, validationStatus,
                                                      validationPriority, comments);
                }

                if (validationStatus == ConstantsDT.VALIDATION_FAIL)
                {
                    if (errorsRS == null)
                    {
                        errorsRS = new ADODB.Recordset();
                        errorsRS.Fields.Append("G3E_FNO", DataTypeEnum.adInteger, 5, FieldAttributeEnum.adFldKeyColumn);
                        errorsRS.Fields.Append("G3E_FID", DataTypeEnum.adInteger, 10, FieldAttributeEnum.adFldKeyColumn);
                        errorsRS.Open();
                    }

                    errorsRS.AddNew();
                    errorsRS.Fields["G3E_FNO"].Value = fno;
                    errorsRS.Fields["G3E_FID"].Value = fid;
                }

                returnValue = true;
            }
            catch (Exception ex)
            {
                returnValue = false;
                MessageBox.Show(m_Application.ApplicationWindow, ConstantsDT.ERROR_WRITING_VALIDATION_RESULTS + ": " + ex.Message, ConstantsDT.APPLICATION_NAME, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }

            return returnValue;
        }


        // Reset the results for a new calculation.
        //  Set the results textbox background color and grid cells back to the initial color.
        public void ResetResultsForCalculation()
        {
            txtXfmrVoltageDrop.BackColor = m_DefaultTextBoxColor;
            cmdPrintReport.Enabled = false;
            cmdSaveReport.Enabled = false;
            m_PassedValidation = false;

            foreach (DataGridViewRow r in dgvSecondary.Rows)
            {
                r.Cells[M_SECGRID_VOLTAGE].Style.BackColor = m_DefaultReadOnlyCellColor;
                r.Cells[M_SECGRID_FLICKER].Style.BackColor = m_DefaultReadOnlyCellColor;
                r.Cells[M_SECGRID_VOLTAGE].Value = null;
                r.Cells[M_SECGRID_SEASON].Value = null;
                r.Cells[M_SECGRID_FLICKER].Value = null;
                r.Cells[M_SECGRID_VOLTAGE_DROP_PCT].Value = null;
            }

            foreach (DataGridViewRow r in dgvService.Rows)
            {
                r.Cells[M_SRVCGRID_VOLTAGE].Style.BackColor = m_DefaultReadOnlyCellColor;
                r.Cells[M_SRVCGRID_FLICKER].Style.BackColor = m_DefaultReadOnlyCellColor;
                r.Cells[M_SRVCGRID_VOLTAGE].Value = null;
                r.Cells[M_SRVCGRID_SEASON].Value = null;
                r.Cells[M_SRVCGRID_FLICKER].Value = null;
                r.Cells[M_SRVCGRID_VOLTAGE_DROP_PCT].Value = null;
                r.Cells[M_SRVCGRID_LOAD_USED].Value = null;
                r.Cells[M_SRVCGRID_OVERRIDE_LOAD].Value = null;
            }
        }

        // Calculate the flicker
        private bool CalculateFlicker(Conductor conductor, ref double accumXfmrCondResistance, ref double accumXfmrCondReactance)
        {            
            bool returnValue = false;

            try
            {
                double accumResistance = accumXfmrCondResistance;
                double accumReactance = accumXfmrCondReactance;
                double meterResistance = 0;
                double meterReactance = 0;
                double impedance = 0;
                double highImpedance = 0;
                int serviceCount = 0;

                if (!CalculateImpedance(accumResistance, accumReactance, ref impedance))
                {
                    return false;
                }
                conductor.Impedance = impedance;

                foreach (Service service in conductor.Services.Values)
                {
                    accumResistance += service.Resistance;
                    accumReactance += service.Reactance;

                    if (!CalculateImpedance(accumResistance, accumReactance, ref impedance))
                    {
                        return false;
                    }
                    service.Impedance = impedance;
                    
                    if (service.ServicePoint != null)
                    {
                        meterResistance = service.ServicePoint.Resistance;
                        meterReactance = service.ServicePoint.Reactance;

                        if (!CalculateImpedance(meterResistance, meterReactance, ref impedance))
                        {
                            return false;
                        }
                        service.ServicePoint.Impedance = impedance;

                        accumResistance += meterResistance;
                        accumReactance += meterReactance;

                        if (!CalculateImpedance(accumResistance, accumReactance, ref impedance))
                        {
                            return false;
                        }

                        if (impedance > highImpedance)
                        {
                            highImpedance = impedance;
                        }

                        service.Flicker = Math.Round((service.Impedance / impedance), 4);

                        serviceCount++;
                    }

                    accumResistance = accumXfmrCondResistance;
                    accumReactance = accumXfmrCondReactance;
                }

                if (serviceCount > 0)
                {
                    conductor.Flicker = Math.Round((conductor.Impedance / highImpedance), 4);
                }                

                highImpedance = 0;

                foreach (Conductor downstreamConductor in m_CommonDT.ConductorsDictionary.Values)
                {
                    if (downstreamConductor.SourceFID == conductor.ConductorFID)
                    {
                        accumXfmrCondResistance += downstreamConductor.Resistance;
                        accumXfmrCondReactance += downstreamConductor.Reactance;
                        if (!CalculateFlicker(downstreamConductor, ref accumXfmrCondResistance, ref accumXfmrCondReactance))
                        {
                            return false;
                        }
                    }                    
                }

                returnValue = true;
            }
            catch (Exception ex)
            {
                returnValue = false;
                MessageBox.Show(m_Application.ApplicationWindow, ConstantsDT.ERROR_SC_CALCULATE_FLICKER + ": " + ex.Message, ConstantsDT.APPLICATION_NAME, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }

            return returnValue;
        }

        // Recursive function to calculate the Voltage Drop
        private bool CalculateVoltageDrop(Conductor conductor, double upstreamVlmag, double upstreamVoltage, double upstreamVoltageDropPct)
        {            
            bool returnValue = false;

            try
            {
                double adjustedLoad = conductor.FutureServiceLoad + conductor.LoadSummer;
                double adjustedCustomerCount = conductor.FutureServiceCountAccum + conductor.CustomerCount;
                double voltageDrop = 0;
                double voltageDropVolts = 0;
                double voltageDropPct = 0;
                double accumVoltageDropPct = 0;
                double vlmag = 0;
                string season = string.Empty;

                if (m_PhaseCount != 3)
                {
                    if (!m_clsCalculate.ConductorVoltageDrop(upstreamVlmag, upstreamVoltage, conductor.Length, conductor.CableProperties, adjustedLoad,
                                                    conductor.LoadWinter, adjustedCustomerCount, ref season, ref voltageDrop, ref voltageDropVolts, ref voltageDropPct, ref vlmag))
                    {
                        return false;
                    }
                }
                else
                {
                    if (!m_clsCalculate.ConductorVoltageDropCommercial(upstreamVlmag, adjustedLoad, conductor.LoadWinter, conductor.Length, conductor.CableProperties,
                                                                       ref season, ref voltageDrop, ref voltageDropVolts, ref voltageDropPct, ref vlmag))
                    {
                        return false;
                    }
                }

                upstreamVlmag = vlmag;
                upstreamVoltage = voltageDrop;

                accumVoltageDropPct = upstreamVoltageDropPct;
                accumVoltageDropPct += voltageDropPct;

                conductor.Voltage = voltageDrop;
                conductor.VoltageDropPercent = accumVoltageDropPct;
                conductor.Season = season;

                foreach (Service service in conductor.Services.Values)
                {
                    if (m_PhaseCount != 3)
                    {
                        if (!m_clsCalculate.ConductorVoltageDrop(upstreamVlmag, upstreamVoltage, service.Length, service.CableProperties, service.LoadSummer,
                                        service.LoadWinter, service.CustomerCount, ref season, ref voltageDrop, ref voltageDropVolts, ref voltageDropPct, ref vlmag))
                        {
                            return false;
                        }
                    }
                    else
                    {
                        if (!m_clsCalculate.ConductorVoltageDropCommercial(upstreamVlmag, service.LoadSummer, service.LoadWinter, service.Length, service.CableProperties,
                                                                           ref season, ref voltageDrop, ref voltageDropVolts, ref voltageDropPct, ref vlmag))
                        {
                            return false;
                        }
                    }

                    service.Voltage = voltageDrop;
                    accumVoltageDropPct += voltageDropPct;
                    service.VoltageDropPercent = accumVoltageDropPct;
                    service.Season = season;
                }

                foreach (Conductor downstreamConductor in m_CommonDT.ConductorsDictionary.Values)
                {
                    if (downstreamConductor.SourceFID == conductor.ConductorFID)
                    {
                        CalculateVoltageDrop(downstreamConductor, upstreamVlmag, upstreamVoltage, conductor.VoltageDropPercent);
                    }
                }

                returnValue = true;
            }
            catch (Exception ex)
            {
                returnValue = false;
                MessageBox.Show(m_Application.ApplicationWindow, ConstantsDT.ERROR_SC_CALCULATE_CONDUCTOR_VDROP + ": " + ex.Message, ConstantsDT.APPLICATION_NAME, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }

            return returnValue;
        }

        // Get the metadata for the Secondary Calculator command.
        internal bool GetMetadata()
        {
            bool returnValue = false;

            try
            {                
                Recordset metadataRS = null;
                CommonDT.GetCommandMetadata("SecondaryCalculatorCC", ref metadataRS);

                if (metadataRS.RecordCount > 0)
                {
                    string subsystemComponent = string.Empty;
                    string paramName = string.Empty;
                    string paramValue = string.Empty;
                    metadataRS.MoveFirst();

                    while (!metadataRS.EOF)
                    {
                        subsystemComponent = metadataRS.Fields["SUBSYSTEM_COMPONENT"].Value.ToString();
                        paramName = metadataRS.Fields["PARAM_NAME"].Value.ToString();
                        paramValue = metadataRS.Fields["PARAM_VALUE"].Value.ToString();

                        switch(subsystemComponent)
                        {
                            case "DiversityFactor":
                                if(paramName == "Summer")
                                {
                                    m_clsCalculate.DiversityFactorSummer = Convert.ToDouble(paramValue);
                                }
                                else if(paramName == "Winter")
                                {
                                    m_clsCalculate.DiversityFactorWinter = Convert.ToDouble(paramValue);
                                }
                                else if (paramName == "CustomerCountHigh")
                                {
                                    m_clsCalculate.DiversityFactorCustomerCountHigh = Convert.ToDouble(paramValue);
                                }
                                else if (paramName == "CustomerSummerHigh")
                                {
                                    m_clsCalculate.DiversityFactorCustomerSummerHigh = Convert.ToDouble(paramValue);
                                }
                                else if (paramName == "CustomerSummerLow")
                                {
                                    m_clsCalculate.DiversityFactorCustomerSummerLow = Convert.ToDouble(paramValue);
                                }
                                else if (paramName == "CustomerWinterHigh")
                                {
                                    m_clsCalculate.DiversityFactorCustomerWinterHigh = Convert.ToDouble(paramValue);
                                }
                                else if (paramName == "CustomerWinterLow")
                                {
                                    m_clsCalculate.DiversityFactorCustomerWinterLow = Convert.ToDouble(paramValue);
                                }
                                break;
                            case "FlickerThreshold":
                                if (paramName == "SecondaryLow")
                                {
                                    m_SecFlickerPctLowThreshold = Convert.ToDouble(paramValue) * 0.01;
                                }
                                else if (paramName == "SecondaryHigh")
                                {
                                    m_SecFlickerPctHighThreshold = Convert.ToDouble(paramValue) * 0.01;
                                }
                                else if (paramName == "ServiceLow")
                                {
                                    m_SrvcFlickerPctLowThreshold = Convert.ToDouble(paramValue) * 0.01;
                                }
                                else if (paramName == "ServiceHigh")
                                {
                                    m_SrvcFlickerPctHighThreshold = Convert.ToDouble(paramValue) * 0.01;
                                }
                                break;
                            case "VoltageThreshold":
                                if (paramName == "Transformer")
                                {
                                    m_XfmrVoltageDropPctThreshold = Convert.ToDouble(paramValue) * 0.01;
                                }
                                else if (paramName == "Secondary")
                                {
                                    m_SecVoltageDropPctThreshold = Convert.ToDouble(paramValue) * 0.01;
                                }
                                else if (paramName == "Service")
                                {
                                    m_SrvcVoltageDropPctThreshold = Convert.ToDouble(paramValue) * 0.01;
                                }
                                break;
                            case "Report":
                                if (paramName == "ReportFileName")
                                {
                                    m_ReportFileName = paramValue;
                                }
                                else if (paramName == "ReportName")
                                {
                                    m_ReportName = paramValue;
                                }
                                break;
                            case "Trace":
                                if (paramName == "TraceName")
                                {
                                    m_traceUserName = paramValue;
                                }
                                break;
                            case "TransformerSizing":
                                if (paramName == "XfmrSizingFileName")
                                {
                                    m_XfmrSizingFilePath = paramValue;
                                }
                                break;
                            case "EnablingCondition":
                                if (paramName == "3PhaseVoltages")
                                {
                                    m_ValidThreePhaseVoltages = paramValue.Split(',');
                                }
                                break;
                        }

                        metadataRS.MoveNext();
                    }

                    returnValue = true;
                }
                else
                {
                    MessageBox.Show(m_Application.ApplicationWindow, ConstantsDT.ERROR_RETRIEVING_COMMAND_METADATA, ConstantsDT.APPLICATION_NAME, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    returnValue = false;
                }

                returnValue = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(m_Application.ApplicationWindow, ConstantsDT.ERROR_RETRIEVING_COMMAND_METADATA + ": " + ex.Message, ConstantsDT.APPLICATION_NAME, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return false;
            }

            return returnValue;
        }

        private bool GetToolsMetadata()
        {
            bool returnValue = false;

            try
            {
                if (!m_CommonDT.GetCableMetadata())
                {
                    return false;
                }

                if (!m_CommonDT.GetTransformerMetadata())
                {
                    return false;
                }

                if (!m_CommonDT.GetTransformerProperties(m_XfmrCU, ref m_XfmrType, ref m_XfmrProperties))
                {
                    return false;
                }

                cmdXfmrCU.Text = m_XfmrType;
                m_clsCalculate.XfmrXoverR = m_XfmrProperties.XfmrXoverR;
                m_clsCalculate.XfmrImpedance = m_XfmrProperties.Impedance;
                m_clsCalculate.VoltageSecondary = m_XfmrProperties.SecondaryVoltage;
                m_clsCalculate.VoltageFactor = m_XfmrProperties.VoltageFactor;
                m_clsCalculate.TypeFactor = m_XfmrProperties.TypeFactor;
                m_clsCalculate.Voltage3Phase = m_XfmrProperties.Voltage3Phase;

                returnValue = true;
                
            }
            catch (Exception ex)
            {
                MessageBox.Show(m_Application.ApplicationWindow, ConstantsDT.ERROR_RETRIEVING_COMMAND_METADATA + ": " + ex.Message, ConstantsDT.APPLICATION_NAME, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return false;
            }

            return returnValue;
        }

        // Populate the AC Tonnage combobox
        private bool PopulateTonnageList()
        {
            bool returnValue = false;

            try
            {
                // Get the AC Tonnage values from metadata
                m_TonnageRS = m_Application.DataContext.OpenRecordset(ConstantsDT.QUERY_TONNAGE, CursorTypeEnum.adOpenDynamic, LockTypeEnum.adLockOptimistic, (int)CommandTypeEnum.adCmdText);

                if (m_TonnageRS.RecordCount > 0)
                {
                    while (!m_TonnageRS.EOF)
                    {
                        cboLargestACTonnage.Items.Add(m_TonnageRS.Fields[ConstantsDT.FIELD_TONNAGE_TONNAGE].Value);
                        m_TonnageRS.MoveNext();
                    }
                    returnValue = true;
                }
                else
                {
                    MessageBox.Show(m_Application.ApplicationWindow, ConstantsDT.ERROR_SC_NO_TONNAGE_RECORDS, ConstantsDT.APPLICATION_NAME, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    returnValue = false;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(m_Application.ApplicationWindow, ConstantsDT.ERROR_SC_TONNAGE_LIST + ": " + ex.Message, ConstantsDT.APPLICATION_NAME, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                returnValue = false;
            }

            return returnValue;
        }

        // Populate the # Cables Per Phase combobox
        private bool PopulateCablesPerPhase()
        {
            bool returnValue = false;

            try
            {
                string xfmrType = string.Empty;
                if (m_XfmrFNO == ConstantsDT.FNO_OH_XFMR)
                {
                    xfmrType = "OH";
                }
                else
                {
                    xfmrType = "UG";
                }

                // Get the cables per phase values from metadata
                m_CablesPerPhaseRS = m_Application.DataContext.OpenRecordset(ConstantsDT.QUERY_DERATING, CursorTypeEnum.adOpenDynamic, LockTypeEnum.adLockOptimistic, (int)CommandTypeEnum.adCmdText, xfmrType);

                if (m_CablesPerPhaseRS.RecordCount > 0)
                {
                    while (!m_CablesPerPhaseRS.EOF)
                    {
                        cboCablesPerPhase.Items.Add(m_CablesPerPhaseRS.Fields[ConstantsDT.FIELD_NUMBER_OF_CABLES].Value);
                        m_CablesPerPhaseRS.MoveNext();
                    }
                    returnValue = true;
                }
                else
                {
                    MessageBox.Show(m_Application.ApplicationWindow, ConstantsDT.ERROR_SC_CABLES_PER_PHASE_RECORDS, ConstantsDT.APPLICATION_NAME, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    returnValue = false;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(m_Application.ApplicationWindow, ConstantsDT.ERROR_SC_CABLES_PER_PHASE_LIST + ": " + ex.Message, ConstantsDT.APPLICATION_NAME, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                returnValue = false;
            }

            return returnValue;
        }

        // Restrict values for Summer Power Factor
        private void txtPFSummer_KeyPress(object sender, KeyPressEventArgs e)
        {
            // Allow user to only enter numbers
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        // Restrict values for Winter Power Factor
        private void txtPFWinter_KeyPress(object sender, KeyPressEventArgs e)
        {
            // Allow user to only enter numbers
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        // Restrict values for Override Load
        private void txtOverrideLoad_KeyPress(object sender, KeyPressEventArgs e)
        {
            // Allow user to only enter numbers
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        // Restrict values for Future Service Load
        private void txtFutureServiceLoad_KeyPress(object sender, KeyPressEventArgs e)
        {
            // Allow user to only enter numbers
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        // Restrict values for Future Service Length
        private void txtFutureServiceLength_KeyPress(object sender, KeyPressEventArgs e)
        {
            // Allow user to only enter numbers
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        // Display the Options dialog
        private void cmdOptions_Click(object sender, EventArgs e)
        {
            OptionsDT frmDesignToolsOptions = new OptionsDT();

            frmDesignToolsOptions.StartPosition = FormStartPosition.CenterParent;
            frmDesignToolsOptions.ShowDialog(this);
            frmDesignToolsOptions = null;
        }

        // If user holds down left mouse button over grid divider then
        // Call ProcessGridResize to resize the grids based on the user dragging the divider between the grids
        private void imgGridDivider_MouseMove(object sender, MouseEventArgs e)
        {
            Cursor.Current = Cursors.SizeNS;

            if (e.Button == MouseButtons.Left)
            {                
                dgvSecondary.Height = (int)(dgvSecondary.Height + e.Y);
                ProcessGridResize();
            }            
        }

        // Resize the grids based on the user dragging the divider between the grids
        private void ProcessGridResize()
        {
            imgGridDivider.Top = Convert.ToInt32(dgvSecondary.Top + dgvSecondary.Height);
            dgvService.Top = dgvSecondary.Top + dgvSecondary.Height + imgGridDivider.Height;
        }

        // Launch the CU Selection dialog for the Secondary Conductor. Get and set the cable properties if CU has changed.
        private void dgvSecondary_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (e.ColumnIndex == M_SECGRID_TYPE && e.RowIndex != -1)
                {
                    // Don't allow edit if cell is read-only which indicates feature isn't part of WR.
                    if (dgvSecondary.Rows[e.RowIndex].Cells[M_SECGRID_TYPE].ReadOnly)
                    {
                        return;
                    }

                    // TODO: JIRA 828 - Launch CU Selection
                    // temporary solution
                    frmSelectCU selectCU = new frmSelectCU(CommonDT.CableRS);
                    selectCU.StartPosition = FormStartPosition.CenterParent;
                    selectCU.ShowDialog(m_Application.ApplicationWindow);

                    string cu = string.Empty;

                    if (selectCU.CU.Length > 0)
                    {
                        cu = selectCU.CU;
                    }
                    else
                    {
                        cu = dgvSecondary.Rows[e.RowIndex].Cells[M_SECGRID_CU].Value.ToString();
                    }

                    int fid = Convert.ToInt32(dgvSecondary.Rows[e.RowIndex].Cells[M_SECGRID_FID].Value);

                    Conductor conductor;

                    if (m_CommonDT.ConductorsDictionary.TryGetValue(fid, out conductor))
                    {
                        // If CU is different than current CU, then disable the Print Report command. A new calculation will need to be executed. 
                        if (cu != conductor.CU)
                        {
                            dgvSecondary.Rows[e.RowIndex].Cells[M_SECGRID_CU].Value = cu;
                            dgvSecondary.Rows[e.RowIndex].Cells[e.ColumnIndex].Style.BackColor = m_PendingChangesColor;
                            dgvSecondary.Rows[e.RowIndex].Cells[M_SECGRID_PENDINGEDIT].Value = 1;
                            cmdApply.Enabled = true;
                            cmdPrintReport.Enabled = false;

                            CommonDT.CableProperties cableProperties = new CommonDT.CableProperties();
                            string description = string.Empty;

                            if (!m_CommonDT.GetCableProperties(cu, ref description, ref cableProperties))
                            {
                                return;
                            }

                            conductor.CableProperties = cableProperties;
                            conductor.CU = cu;
                            conductor.CableType = description;
                            conductor.Resistance = cableProperties.Resistance * conductor.Length;
                            conductor.Reactance = cableProperties.Reactance * conductor.Length;

                            dgvSecondary.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = description;

                            // Clear the calculation results
                            ResetResultsForCalculation();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(m_Application.ApplicationWindow, ConstantsDT.ERROR_SC_GRID_CELL_CLICK + ": " + ex.Message, ConstantsDT.APPLICATION_NAME, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        // Launch the CU Selection dialog for the Service Line. Get and set the cable properties if CU has changed.
        private void dgvService_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (e.ColumnIndex == M_SRVCGRID_TYPE && e.RowIndex != -1)
                {
                    // Don't allow edit if cell is read-only which indicates feature isn't part of WR.
                    if (dgvService.Rows[e.RowIndex].Cells[M_SRVCGRID_TYPE].ReadOnly)
                    {
                        return;
                    }

                    // TODO: JIRA 828 - Launch CU Selection
                    // temporary solution
                    frmSelectCU selectCU = new frmSelectCU(CommonDT.CableRS);
                    selectCU.StartPosition = FormStartPosition.CenterParent;
                    selectCU.ShowDialog(m_Application.ApplicationWindow);

                    string cu = string.Empty;

                    if (selectCU.CU.Length > 0)
                    {
                        cu = selectCU.CU;
                    }
                    else
                    {
                        cu = dgvService.Rows[e.RowIndex].Cells[M_SRVCGRID_CU].Value.ToString();
                    }

                    int fid = Convert.ToInt32(dgvService.Rows[e.RowIndex].Cells[M_SRVCGRID_FID].Value);

                    Service service;

                    if (m_CommonDT.ServicesDictionary.TryGetValue(fid, out service))
                    {
                        // If CU is different than current CU, then disable the Print Report command. A new calculation will need to be executed. 
                        if (cu != service.CU)
                        {
                            dgvService.Rows[e.RowIndex].Cells[M_SRVCGRID_CU].Value = cu;
                            dgvService.Rows[e.RowIndex].Cells[e.ColumnIndex].Style.BackColor = m_PendingChangesColor;
                            dgvService.Rows[e.RowIndex].Cells[M_SRVCGRID_PENDINGEDIT].Value = 1;
                            cmdApply.Enabled = true;
                            cmdPrintReport.Enabled = false;

                            CommonDT.CableProperties cableProperties = new CommonDT.CableProperties();
                            string description = string.Empty;

                            if (!m_CommonDT.GetCableProperties(cu, ref description, ref cableProperties))
                            {
                                return;
                            }

                            service.CableProperties = cableProperties;
                            service.CU = cu;
                            service.CableType = description;
                            service.Resistance = cableProperties.Resistance * service.Length;
                            service.Reactance = cableProperties.Reactance * service.Length;

                            dgvService.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = description;

                            // Clear the calculation results
                            ResetResultsForCalculation();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(m_Application.ApplicationWindow, ConstantsDT.ERROR_SC_GRID_CELL_CLICK + ": " + ex.Message, ConstantsDT.APPLICATION_NAME, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        // Highlight the corresponding Secondary Conductor in the map window based on the row selected in the secondary grid.
        private void dgvSecondary_SelectionChanged(object sender, EventArgs e)
        {
            m_Application.SelectedObjects.Clear();

            if (dgvSecondary.SelectedRows.Count > 0)
            {
                dgvService.ClearSelection();

                foreach (DataGridViewRow r in dgvSecondary.SelectedRows)
                {
                    m_CommonDT.UpdateMapWindow(Convert.ToInt16(r.Cells[M_SECGRID_FNO].Value), Convert.ToInt32(r.Cells[M_SECGRID_FID].Value));
                }
            }
        }

        // Highlight the corresponding Service Line and Service Point in the map window based on the row selected in the service grid.
        private void dgvService_SelectionChanged(object sender, EventArgs e)
        {
            m_Application.SelectedObjects.Clear();

            if (dgvService.SelectedRows.Count > 0)
            {
                dgvSecondary.ClearSelection();

                foreach (DataGridViewRow r in dgvService.SelectedRows)
                {
                    m_CommonDT.UpdateMapWindow(ConstantsDT.FNO_SRVCCOND, Convert.ToInt32(r.Cells[M_SRVCGRID_FID].Value));
                    if (r.Cells[M_SRVCGRID_SRVCPT_FID] != null)
                    {
                        m_CommonDT.UpdateMapWindow(ConstantsDT.FNO_SRVCPT, Convert.ToInt32(r.Cells[M_SRVCGRID_SRVCPT_FID].Value));
                    }
                }
            }
        }

        // Open the Single Phase Transformer Sizing spreadsheet
        private void cmdTransformerSizing_Click(object sender, EventArgs e)
        {
            try
            {
                m_Application.SetStatusBarText(GTStatusPanelConstants.gtaspcMessage, ConstantsDT.MESSAGE_SC_OPEN_XFMR_SIZING);
                m_Application.BeginWaitCursor();

                string xfmrSizingFileName = m_XfmrSizingFilePath.Substring(m_XfmrSizingFilePath.LastIndexOf("\\") + 1);
                string newFile = Path.GetTempPath() + xfmrSizingFileName;

                File.Copy(m_XfmrSizingFilePath, newFile, true);

                // Open the file for the user
                System.Diagnostics.Process.Start(newFile);

                m_Application.EndWaitCursor();
                m_Application.SetStatusBarText(GTStatusPanelConstants.gtaspcMessage, "");                
            }
            catch (Exception ex)
            {
                m_Application.SetStatusBarText(GTStatusPanelConstants.gtaspcMessage, "");
                m_Application.EndWaitCursor();
                MessageBox.Show(m_Application.ApplicationWindow, ConstantsDT.ERROR_SC_XFMR_SIZING_COMMAND + ": " + ex.Message, ConstantsDT.APPLICATION_NAME, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        // Restrict values in the grid cells
        private void dgvSecondary_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
        {
            if (dgvSecondary.CurrentCell.ReadOnly == false)
            {
                e.Control.KeyPress += new KeyPressEventHandler(CheckValue);
            }
        }

        // Restrict values in the grid cells
        private void CheckValue(object sender, KeyPressEventArgs e)
        {
            // Allow user to only enter numbers for Future Service Count
            if (dgvSecondary.CurrentCell.ColumnIndex == M_SECGRID_FUTURE_SVC)
            {
                if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
                {
                    e.Handled = true;
                }
            }            
        }

        // Recalculate Service Point impedance when Largest AC Tonnage value changes
        private void cboLargestACTonnage_SelectedValueChanged(object sender, EventArgs e)
        {
            try
            {
                if (cboLargestACTonnage.SelectedItem != null)
                {
                    string rsFilter = ConstantsDT.FIELD_TONNAGE_TONNAGE + " = " + Convert.ToDouble(cboLargestACTonnage.SelectedItem);

                    m_TonnageRS.Filter = rsFilter;

                    if (m_TonnageRS.RecordCount > 0)
                    {
                        if (!Convert.IsDBNull(m_TonnageRS.Fields[ConstantsDT.FIELD_TONNAGE_LRA].Value))
                        {
                            m_CommonDT.LockedRotorAmps = Convert.ToInt32(m_TonnageRS.Fields[ConstantsDT.FIELD_TONNAGE_LRA].Value);
                            m_Tonnage = Convert.ToDouble(m_TonnageRS.Fields[ConstantsDT.FIELD_TONNAGE_TONNAGE].Value);

                            if (m_CommonDT.ServicesDictionary != null)
                            {
                                foreach (Service service in m_CommonDT.ServicesDictionary.Values)
                                {
                                    if (service.ServicePoint != null)
                                    {
                                        if (!m_CommonDT.CalculateServicePointImpedance(service.ServicePoint))
                                        {
                                            return;
                                        }
                                    }                                    
                                }
                            }
                        }
                    }

                    // Clear the calculation results
                    ResetResultsForCalculation();

                    cmdCalculate.Enabled = true;
                }
                else
                {
                    cmdCalculate.Enabled = false;
                }
            }
            catch (Exception ex)
            {
                cmdCalculate.Enabled = false;
                MessageBox.Show(m_Application.ApplicationWindow, ConstantsDT.ERROR_SC_TONNAGE_CHANGE + ": " + ex.Message, ConstantsDT.APPLICATION_NAME, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        // Update Summer Power Factor variables when Summer Power Factor changes
        private void txtPFSummer_TextChanged(object sender, EventArgs e)
        {
            if (txtPFSummer.Text.Length > 0)
            {
                m_SummerPowerFactor = Convert.ToDouble(txtPFSummer.Text) / 100;
                m_clsCalculate.PowerFactorSummer = m_SummerPowerFactor;
                m_CommonDT.SummerPowerFactor = m_SummerPowerFactor;
            }
        }

        // Update Winter Power Factor variables when Winter Power Factor changes
        private void txtPFWinter_TextChanged(object sender, EventArgs e)
        {
            if (txtPFWinter.Text.Length > 0)
            {
                m_WinterPowerFactor = Convert.ToDouble(txtPFWinter.Text) / 100;
                m_clsCalculate.PowerFactorWinter = m_WinterPowerFactor;
            }
        }

        // Calculate impedance
        private bool CalculateImpedance(double resistance, double reactance, ref double impedance)
        {
            bool returnValue = false;

            try
            {
                impedance = Math.Sqrt(Math.Pow(resistance, 2) + Math.Pow(reactance, 2));
                returnValue = true;
            }
            catch (Exception ex)
            {
                returnValue = false;
                MessageBox.Show(m_Application.ApplicationWindow, ConstantsDT.ERROR_SC_CALCULATE_Z + ": " + ex.Message, ConstantsDT.APPLICATION_NAME, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }

            return returnValue;
        }

        // Launch CU Selection dialog for Transformer. Get and set Transformer properties.
        private void cmdXfmrCU_Click(object sender, EventArgs e)
        {
            try
            {
                // TODO: JIRA 828 - Launch CU Selection
                // temporary solution
                frmSelectCU selectCU = new frmSelectCU(CommonDT.XfmrRS);
                selectCU.StartPosition = FormStartPosition.CenterParent;
                selectCU.ShowDialog(m_Application.ApplicationWindow);

                string cu = string.Empty;

                if (selectCU.CU.Length > 0)
                {
                    cu = selectCU.CU;
                }
                else
                {
                    cu = cmdXfmrCU.Text.ToString();
                }

                // If CU is different than current CU, then clear the calculation results and disable the Print Report command. A new calculation will need to be executed. 
                if (cu != m_XfmrCU)
                {
                    m_CommonDT.GetTransformerProperties(cu, ref m_XfmrType, ref m_XfmrProperties);

                    m_XfmrCU = cu;
                    m_XfmrPendingEdit = true;
                    cmdXfmrCU.Text = m_XfmrType;
                    txtXfmrSize.Text = m_XfmrProperties.Size.ToString();
                    m_XfmrSize = Convert.ToDouble(txtXfmrSize.Text);
                    txtXfmrVoltage.Text = m_XfmrProperties.Voltage;
                    txtXfmrType.Text = m_XfmrProperties.Orientation + " " + m_XfmrProperties.Type;

                    m_clsCalculate.XfmrXoverR = m_XfmrProperties.XfmrXoverR;
                    m_clsCalculate.XfmrImpedance = m_XfmrProperties.Impedance;
                    m_clsCalculate.VoltageSecondary = m_XfmrProperties.SecondaryVoltage;
                    m_clsCalculate.VoltageFactor = m_XfmrProperties.VoltageFactor;
                    m_clsCalculate.TypeFactor = m_XfmrProperties.TypeFactor;
                    m_clsCalculate.Voltage3Phase = m_XfmrProperties.Voltage3Phase;

                    cmdXfmrCU.BackColor = m_PendingChangesColor;
                    cmdApply.Enabled = true;

                    // Clear the calculation results
                    ResetResultsForCalculation();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(m_Application.ApplicationWindow, ConstantsDT.ERROR_SC_XFMR_CHANGE + ": " + ex.Message, ConstantsDT.APPLICATION_NAME, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        // Update Neutral Voltage Drop variables when Neutral Voltage Drop changes
        private void cboNeutralVDrop_SelectedValueChanged(object sender, EventArgs e)
        {
            if (cboNeutralVDrop.SelectedItem != null)
            {
                if (cboNeutralVDrop.Text.ToString().ToUpper() == "YES")
                {
                    m_clsCalculate.NeutralVDropFactor = 2;
                }
                else
                {
                    m_clsCalculate.NeutralVDropFactor = 1;
                }
            }
        }

        // Update load values when override load changes
        private void txtOverrideLoad_TextChanged(object sender, EventArgs e)
        {
            if (txtOverrideLoad.Text.Length > 0)
            {
                m_OverrideLoad = Convert.ToDouble(txtOverrideLoad.Text);
            }
            else
            {
                m_OverrideLoad = 0;
            }

            // Clear the calculation results
            ResetResultsForCalculation();
        }

        // Update load values when future service length changes
        private void txtFutureServiceLength_TextChanged(object sender, EventArgs e)
        {
            if (txtFutureServiceLength.Text.Length > 0)
            {
                m_FutureServiceLength = Convert.ToDouble(txtFutureServiceLength.Text);
            }
            else
            {
                m_FutureServiceLength = 0;
            }

            // Clear the calculation results
            ResetResultsForCalculation();
        }

        // Update load values when future service load changes
        private void txtFutureServiceLoad_TextChanged(object sender, EventArgs e)
        {            
            if (txtFutureServiceLoad.Text.Length > 0)
            {
                m_FutureServiceLoad = Convert.ToDouble(txtFutureServiceLoad.Text);
            }
            else
            {
                m_FutureServiceLoad = 0;
            }

            // Clear the calculation results
            ResetResultsForCalculation();
        }

        // Print the calculation results to a pdf.
        private void cmdPrintReport_Click(object sender, EventArgs e)
        {
            try
            {
                m_Application.SetStatusBarText(GTStatusPanelConstants.gtaspcMessage, ConstantsDT.MESSAGE_REPORT_CREATING);
                m_Application.BeginWaitCursor();

                List<KeyValuePair<string, string>> reportValues = new List<KeyValuePair<string, string>>();

                string reportVersionDate = Assembly.GetExecutingAssembly().GetName().Version.ToString();
                DateTime buildDate = new FileInfo(Assembly.GetExecutingAssembly().Location).LastWriteTime;
                reportVersionDate += "  " + buildDate.ToShortDateString();

                // Add report values
                reportValues.Add(new KeyValuePair<string, string>(ConstantsDT.REPORT_PARAMETER_VERSIONDATE, reportVersionDate));
                reportValues.Add(new KeyValuePair<string, string>(ConstantsDT.REPORT_PARAMETER_WR, m_Application.DataContext.ActiveJob));
                reportValues.Add(new KeyValuePair<string, string>(ConstantsDT.REPORT_PARAMETER_DATE, DateTime.Now.ToShortDateString()));
                reportValues.Add(new KeyValuePair<string, string>(ConstantsDT.REPORT_PARAMETER_WR_DESCRIPTION, CommonDT.WrDescription));
                reportValues.Add(new KeyValuePair<string, string>(ConstantsDT.REPORT_PARAMETER_SC_PF_SUMMER, (m_clsCalculate.PowerFactorSummer * 100).ToString() + "%"));
                reportValues.Add(new KeyValuePair<string, string>(ConstantsDT.REPORT_PARAMETER_SC_PF_WINTER, (m_clsCalculate.PowerFactorWinter * 100).ToString() + "%"));
                reportValues.Add(new KeyValuePair<string, string>(ConstantsDT.REPORT_PARAMETER_SC_TONNAGE, m_Tonnage.ToString()));
                reportValues.Add(new KeyValuePair<string, string>(ConstantsDT.REPORT_PARAMETER_SC_XFMR_TYPE, m_XfmrType));
                reportValues.Add(new KeyValuePair<string, string>(ConstantsDT.REPORT_PARAMETER_SC_XFMR_VOLTAGE, m_XfmrProperties.Voltage));
                reportValues.Add(new KeyValuePair<string, string>(ConstantsDT.REPORT_PARAMETER_SC_XFMR_VDROP, m_XfmrVoltageDrop.ToString()));
                reportValues.Add(new KeyValuePair<string, string>(ConstantsDT.REPORT_PARAMETER_SC_XFMR_VDROP_HIGH, (m_XfmrProperties.SecondaryVoltage / 2 * (1 - m_XfmrVoltageDropPctThreshold)).ToString()));
                reportValues.Add(new KeyValuePair<string, string>(ConstantsDT.REPORT_PARAMETER_SC_SEC_VDROP_HIGH, (m_XfmrProperties.SecondaryVoltage / 2 * (1 - m_SecVoltageDropPctThreshold)).ToString()));
                reportValues.Add(new KeyValuePair<string, string>(ConstantsDT.REPORT_PARAMETER_SC_SRVC_VDROP_HIGH, (m_XfmrProperties.SecondaryVoltage / 2 * (1 - m_SrvcVoltageDropPctThreshold)).ToString()));
                reportValues.Add(new KeyValuePair<string, string>(ConstantsDT.REPORT_PARAMETER_SC_SEC_FLICKER_LOW, m_SecFlickerPctLowThreshold.ToString()));
                reportValues.Add(new KeyValuePair<string, string>(ConstantsDT.REPORT_PARAMETER_SC_SEC_FLICKER_HIGH, m_SecFlickerPctHighThreshold.ToString()));
                reportValues.Add(new KeyValuePair<string, string>(ConstantsDT.REPORT_PARAMETER_SC_SRVC_FLICKER_LOW, m_SrvcFlickerPctLowThreshold.ToString()));
                reportValues.Add(new KeyValuePair<string, string>(ConstantsDT.REPORT_PARAMETER_SC_SRVC_FLICKER_HIGH, m_SrvcFlickerPctHighThreshold.ToString()));

                m_ReportDT = new ReportDT();

                m_ReportDT.ReportFile = m_ReportFileName;
                m_ReportDT.ReportName = m_ReportName;

                m_ReportDT.CreateReport(ConstantsDT.COMMAND_NAME_SECONDARY_CALCULATOR, m_Application.DataContext.ActiveJob, m_XfmrFID, reportValues, dgvSecondary, dgvService);

                m_ReportPDF = m_ReportDT.ReportPDF;

                m_Application.EndWaitCursor();
                m_Application.SetStatusBarText(GTStatusPanelConstants.gtaspcMessage, "");

                if (m_PassedValidation)
                {
                    cmdSaveReport.Enabled = true;
                }
            }
            catch (Exception ex)
            {
                m_Application.EndWaitCursor();
                m_Application.SetStatusBarText(GTStatusPanelConstants.gtaspcMessage, "");
                MessageBox.Show(m_Application.ApplicationWindow, ConstantsDT.ERROR_REPORT_PRINTING + ": " + ex.Message, ConstantsDT.APPLICATION_NAME, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        // Save the report to a shared location and hyperlink the report to transformer
        private void cmdSaveReport_Click(object sender, EventArgs e)
        {
            string url = string.Empty;
            string docMgmtFileName = string.Empty;

            try
            {
                // Add report to document management
                if (m_ReportDT.UploadReport(m_ReportPDF, "Secondary Calculator", ref url, ref docMgmtFileName))
                {
                    string tmpQry = "select g3e_fid from DESIGNAREA_P where JOB_ID = ?";
                    Recordset tmpRs = m_Application.DataContext.OpenRecordset(tmpQry, CursorTypeEnum.adOpenStatic, LockTypeEnum.adLockReadOnly, (int)CommandTypeEnum.adCmdText, m_Application.DataContext.ActiveJob);
                    if (!(tmpRs.BOF && tmpRs.EOF))
                    {
                        tmpRs.MoveFirst();
                        int designAreaFid = Convert.ToInt32(tmpRs.Fields["g3e_fid"].Value);

                        m_TransactionManager.Begin("New Hyperlink");
                        // Create a hyperlink component for the active feature
                        if (m_ReportDT.AddHyperlinkComponent(designAreaFid, ConstantsDT.FNO_DESIGN_AREA, url, "Secondary Calculator", null))
                        {
                            m_TransactionManager.Commit();
                        }
                        else
                        {
                            m_TransactionManager.Rollback();
                        }
                    }
                    else
                    {
                        MessageBox.Show(m_Application.ApplicationWindow, ConstantsDT.ERROR_REPORT_SAVING + ": " +
                                     "Error: Design Area for Job " + m_Application.DataContext.ActiveJob.ToString() + " not found.",
                                     ConstantsDT.APPLICATION_NAME, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(m_Application.ApplicationWindow, ConstantsDT.ERROR_REPORT_SAVING + ": " + ex.Message,
                                     ConstantsDT.APPLICATION_NAME, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        // Call function to save the pending CU changes to the features
        private void cmdApply_Click(object sender, EventArgs e)
        {
            try
            {
                if (!SavePendingChanges())
                {
                    return;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(m_Application.ApplicationWindow, ConstantsDT.ERROR_APPLY_PENDING_CHANGES + ": " + ex.Message, ConstantsDT.APPLICATION_NAME, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        // Get the pending CU changes and call function to apply the changes
        private bool SavePendingChanges()
        {
            bool returnValue = false;

            try
            {
                m_TransactionManager.Begin("Edit Feature");
                m_Application.SetStatusBarText(GTStatusPanelConstants.gtaspcMessage, ConstantsDT.MESSAGE_APPLY_PENDING_CHANGES);
                m_Application.BeginWaitCursor();

                // Check for Transformer CU change
                if (m_XfmrPendingEdit)
                {
                    if (!UpdateCU(m_XfmrFNO, m_XfmrFID, m_XfmrCU))
                    {
                        m_TransactionManager.Rollback();
                        return false;
                    }
                    cmdXfmrCU.BackColor = m_DefaultTextBoxColor;
                    m_XfmrPendingEdit = false;
                }

                // Check each Secondary Conductor for CU change
                foreach (DataGridViewRow r in dgvSecondary.Rows)
                {
                    if (Convert.ToInt16(r.Cells[M_SECGRID_PENDINGEDIT].Value) == 1)
                    {
                        if (!UpdateCU(Convert.ToInt16(r.Cells[M_SECGRID_FNO].Value), Convert.ToInt32(r.Cells[M_SECGRID_FID].Value), r.Cells[M_SECGRID_CU].Value.ToString()))
                        {
                            m_TransactionManager.Rollback();
                            return false;
                        }
                        r.Cells[M_SECGRID_PENDINGEDIT].Value = 0;
                        r.Cells[M_SECGRID_TYPE].Style.BackColor = Color.White;
                    }

                    // Check for Secondary Conductor pending length edit
                    if (Convert.ToInt16(r.Cells[M_SECGRID_LENGTHEDIT].Value) == 1)
                    {
                        if (!UpdateLength(Convert.ToInt16(r.Cells[M_SECGRID_FNO].Value), Convert.ToInt32(r.Cells[M_SECGRID_FID].Value), Convert.ToDouble(r.Cells[M_SECGRID_LENGTH].Value)))
                        {
                            m_TransactionManager.Rollback();
                            return false;
                        }
                        r.Cells[M_SECGRID_LENGTHEDIT].Value = 0;
                        r.Cells[M_SECGRID_LENGTH].Style.BackColor = Color.White;
                    }
                }

                // Check each Service Line for CU change
                foreach (DataGridViewRow r in dgvService.Rows)
                {
                    if (Convert.ToInt16(r.Cells[M_SRVCGRID_PENDINGEDIT].Value) == 1)
                    {
                        if (!UpdateCU(ConstantsDT.FNO_SRVCCOND, Convert.ToInt32(r.Cells[M_SRVCGRID_FID].Value), r.Cells[M_SRVCGRID_CU].Value.ToString()))
                        {
                            m_TransactionManager.Rollback();
                            return false;
                        }
                        r.Cells[M_SRVCGRID_PENDINGEDIT].Value = 0;
                        r.Cells[M_SRVCGRID_TYPE].Style.BackColor = Color.White;
                    }

                    // Check for Service Line pending length edit
                    if (Convert.ToInt16(r.Cells[M_SRVCGRID_LENGTHEDIT].Value) == 1)
                    {
                        if (!UpdateLength(ConstantsDT.FNO_SRVCCOND, Convert.ToInt32(r.Cells[M_SRVCGRID_FID].Value), Convert.ToDouble(r.Cells[M_SRVCGRID_LENGTH].Value)))
                        {
                            m_TransactionManager.Rollback();
                            return false;
                        }
                        r.Cells[M_SRVCGRID_LENGTHEDIT].Value = 0;
                        r.Cells[M_SRVCGRID_LENGTH].Style.BackColor = Color.White;
                    }
                }

                m_TransactionManager.Commit();
                m_Application.EndWaitCursor();
                m_Application.SetStatusBarText(GTStatusPanelConstants.gtaspcMessage, "");
                cmdApply.Enabled = false;
                returnValue = true;
            }
            catch (Exception ex)
            {
                m_TransactionManager.Rollback();
                returnValue = false;
                m_Application.EndWaitCursor();
                m_Application.SetStatusBarText(GTStatusPanelConstants.gtaspcMessage, "");
                MessageBox.Show(m_Application.ApplicationWindow, ConstantsDT.ERROR_APPLY_PENDING_CHANGES + ": " + ex.Message, ConstantsDT.APPLICATION_NAME, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
            return returnValue;
        }

        // Save the pending CU change to the feature
        private bool UpdateCU(short fno, int fid, string cu)
        {
            bool returnValue = false;

            try
            {
                IGTKeyObject oGTKey = m_Application.DataContext.OpenFeature(fno, fid);
                IGTComponents oGTComponents = oGTKey.Components;
                Recordset componentRS = oGTComponents.GetComponent(ConstantsDT.CNO_COMPUNIT).Recordset;

                if (componentRS.RecordCount > 0)
                {
                    componentRS.Fields[ConstantsDT.FIELD_COMPUNIT_CU].Value = cu;
                }

                returnValue = true;
            }
            catch (Exception ex)
            {
                returnValue = false;
                MessageBox.Show(m_Application.ApplicationWindow, ConstantsDT.ERROR_APPLY_PENDING_CHANGES + ": " + ex.Message, ConstantsDT.APPLICATION_NAME, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
            return returnValue;
        }

        // Save the pending length change to the feature
        private bool UpdateLength(short fno, int fid, double length)
        {
            bool returnValue = false;

            try
            {
                IGTKeyObject oGTKey = m_Application.DataContext.OpenFeature(fno, fid);
                IGTComponents oGTComponents = oGTKey.Components;
                Recordset componentRS = oGTComponents.GetComponent(ConstantsDT.CNO_CONNECTIVITY).Recordset;

                if (componentRS.RecordCount > 0)
                {
                    componentRS.Fields[ConstantsDT.FIELD_CONNECTIVITY_LENGTH_ACTUAL].Value = length;
                }

                returnValue = true;
            }
            catch (Exception ex)
            {
                returnValue = false;
                MessageBox.Show(m_Application.ApplicationWindow, ConstantsDT.ERROR_APPLY_PENDING_CHANGES + ": " + ex.Message, ConstantsDT.APPLICATION_NAME, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
            return returnValue;
        }

        private void optLoadActual_CheckedChanged(object sender, EventArgs e)
        {
            // Clear the calculation results
            ResetResultsForCalculation();
        }

        private void optLoadEstimated_CheckedChanged(object sender, EventArgs e)
        {
            // Clear the calculation results
            ResetResultsForCalculation();
        }

        private void dgvSecondary_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == M_SECGRID_FUTURE_SVC)
            {
                // Clear the calculation results
                ResetResultsForCalculation();
            }
        }

        private void dgvSecondary_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (e.ColumnIndex == M_SECGRID_LENGTH && e.RowIndex != -1)
                {
                    int fid = 0;
                    double length = 0;

                    if (dgvSecondary.Rows[e.RowIndex].Cells[M_SECGRID_LENGTH].Value.ToString().Length > 0)
                    {
                        length = Convert.ToDouble(dgvSecondary.Rows[e.RowIndex].Cells[M_SECGRID_LENGTH].Value);
                    }

                    fid = Convert.ToInt32(dgvSecondary.Rows[e.RowIndex].Cells[M_SECGRID_FID].Value);

                    Conductor conductor;

                    if (m_CommonDT.ConductorsDictionary.TryGetValue(fid, out conductor))
                    {
                        // If length is different than current length, then disable the Print Report command. A new calculation will need to be executed. 
                        if (length != conductor.Length)
                        {
                            conductor.Length = length;
                            if (conductor.CU.Length > 0)
                            {
                                conductor.Resistance = conductor.CableProperties.Resistance * conductor.Length;
                                conductor.Reactance = conductor.CableProperties.Reactance * conductor.Length;
                            }

                            dgvSecondary.Rows[e.RowIndex].Cells[e.ColumnIndex].Style.BackColor = m_PendingChangesColor;
                            dgvSecondary.Rows[e.RowIndex].Cells[M_SECGRID_LENGTHEDIT].Value = 1;
                            cmdApply.Enabled = true;
                            cmdPrintReport.Enabled = false;

                            // Clear the calculation results
                            ResetResultsForCalculation();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(m_Application.ApplicationWindow, ConstantsDT.ERROR_SC_GRID_CELL_CLICK + ": " + ex.Message, ConstantsDT.APPLICATION_NAME, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        private void dgvService_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (e.ColumnIndex == M_SRVCGRID_LENGTH && e.RowIndex != -1)
                {
                    int fid = 0;
                    double length = 0;

                    if (dgvService.Rows[e.RowIndex].Cells[M_SRVCGRID_LENGTH].Value.ToString().Length > 0)
                    {
                        length = Convert.ToDouble(dgvService.Rows[e.RowIndex].Cells[M_SRVCGRID_LENGTH].Value);
                    }

                    fid = Convert.ToInt32(dgvService.Rows[e.RowIndex].Cells[M_SRVCGRID_FID].Value);

                    Service service;

                    if (m_CommonDT.ServicesDictionary.TryGetValue(fid, out service))
                    {
                        // If length is different than current length, then disable the Print Report command. A new calculation will need to be executed. 
                        if (length != service.Length)
                        {
                            service.Length = length;
                            if (service.CU.Length > 0)
                            {
                                service.Resistance = service.CableProperties.Resistance * service.Length;
                                service.Reactance = service.CableProperties.Reactance * service.Length;
                            }

                            dgvService.Rows[e.RowIndex].Cells[e.ColumnIndex].Style.BackColor = m_PendingChangesColor;
                            dgvService.Rows[e.RowIndex].Cells[M_SRVCGRID_LENGTHEDIT].Value = 1;
                            cmdApply.Enabled = true;
                            cmdPrintReport.Enabled = false;

                            // Clear the calculation results
                            ResetResultsForCalculation();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(m_Application.ApplicationWindow, ConstantsDT.ERROR_SC_GRID_CELL_CLICK + ": " + ex.Message, ConstantsDT.APPLICATION_NAME, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        private void cboCablesPerPhase_SelectedValueChanged(object sender, EventArgs e)
        {
            try
            {
                if (cboCablesPerPhase.SelectedItem != null)
                {
                    string rsFilter = ConstantsDT.FIELD_NUMBER_OF_CABLES + " = " + Convert.ToDouble(cboCablesPerPhase.SelectedItem);

                    m_CablesPerPhaseRS.Filter = rsFilter;

                    if (m_CablesPerPhaseRS.RecordCount > 0)
                    {
                        if (!Convert.IsDBNull(m_CablesPerPhaseRS.Fields[ConstantsDT.FIELD_DERATING_VALUE].Value))
                        {
                            m_clsCalculate.NumberOfCablesPerPhase = Convert.ToInt32(m_CablesPerPhaseRS.Fields[ConstantsDT.FIELD_NUMBER_OF_CABLES].Value);
                            m_clsCalculate.DeratingValue = Convert.ToDouble(m_CablesPerPhaseRS.Fields[ConstantsDT.FIELD_DERATING_VALUE].Value);
                        }
                    }

                    // Clear the calculation results
                    ResetResultsForCalculation();

                    cmdCalculate.Enabled = true;
                }
                else
                {
                    cmdCalculate.Enabled = false;
                }
            }
            catch (Exception ex)
            {
                cmdCalculate.Enabled = false;
                MessageBox.Show(m_Application.ApplicationWindow, ConstantsDT.ERROR_SC_CABLES_PER_PHASE_CHANGE + ": " + ex.Message, ConstantsDT.APPLICATION_NAME, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }
    }
}
