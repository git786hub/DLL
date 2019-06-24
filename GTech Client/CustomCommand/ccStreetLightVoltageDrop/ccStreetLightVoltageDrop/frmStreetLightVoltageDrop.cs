using System;
using System.IO;
using System.Drawing;
using System.Collections.Generic;
using System.Windows.Forms;
using Intergraph.GTechnology.API;
using ADODB;
using System.Reflection;
//using OncDocManage;

namespace GTechnology.Oncor.CustomAPI
{
    /// <summary>
    /// Calculates the voltage drop for a Street Light network.
    /// </summary>
    public partial class frmStreetLightVoltageDrop : Form
    {
        public IGTCustomCommandHelper m_CustomCommandHelper;
        public IGTTransactionManager m_TransactionManager;
        private IGTApplication m_Application = GTClassFactory.Create<IGTApplication>();
        private TraceHelper m_clsTraceHelper = new TraceHelper(-1, "Street Light Voltage Drop");        // Class to setup, run, and display trace
        private CommonDT m_CommonDT = new CommonDT();                                                   // Class of common functions for design tools
        private ReportDT m_ReportDT;                                                                    // Class of common report functions

        private int m_XfmrFID = 0;                                                                      // G3E_FID of the selected Transformer
        private short m_XfmrFNO = 0;                                                                    // G3E_FNO of the selected Transformer
        private short m_XfmrUnitCNO = 0;                                                                // G3E_CNO for the bank component of the selected Transformer
        private string m_XfmrCU;                                                                        // CU of the selected Transformer
        private string m_XfmrType;                                                                      // Type of the selected Transformer
        private CommonDT.XfmrProperties xfmrProperties;                                                 // Characteristics of the selected Transformer based on the CU
        private double m_NominalVoltage;                                                                // Selected nominal voltage

        private int m_FormBeginWidth;                                                                   // Starting width of the dialog
        private int m_FormBeginHeight;                                                                  // Starting height of the dialog
        private bool m_FirstCalculation = true;                                                         // Indicates if user has performed calculation

        private string m_ReportFileName;                                                                // Full path of the template used for the report. Retrieved from metadata.
        private string m_ReportName;                                                                    // Report template workbook spreadsheet tab name for report. Retrieved from metadata.
        private string m_ReportPDF;                                                                     // Full path to the generated pdf report.

        private string m_traceUserName;                                                                 // Name of trace to use. Retrieved from metadata.

        private const string M_DISPLAY_PATHNAME = "Traces - Street Light";                              // Entry to add to the legend for the trace results.

        private bool m_PassedValidation = false;                                                        // Indicates status of validation

        // Street Light Datagridview columns
        private const int M_SLGRID_FROM = 0;
        private const int M_SLGRID_TO = 1;
        private const int M_SLGRID_CABLE = 2;
        private const int M_SLGRID_BALLAST = 3;
        private const int M_SLGRID_LENGTH = 4;
        private const int M_SLGRID_COUNT = 5;
        private const int M_SLGRID_AMPS = 6;
        private const int M_SLGRID_LOAD = 7;
        private const int M_SLGRID_VLMAG = 8;
        private const int M_SLGRID_ALLOWED_MIN = 9;
        private const int M_SLGRID_VDROP = 10;
        private const int M_SLGRID_TOTAL_VDROP = 11;
        private const int M_SLGRID_SECONDARY_FID = 12;
        private const int M_SLGRID_SECONDARY_FNO = 13;
        private const int M_SLGRID_LIGHT_FID = 14;
        private const int M_SLGRID_AMPACITY = 15;
        private const int M_SLGRID_CABLECU = 16;
        private const int M_SLGRID_LIGHTCU = 17;
        private const int M_SLGRID_CABLE_EDIT = 18;
        private const int M_SLGRID_LIGHT_EDIT = 19;
        private const int M_SLGRID_LENGTH_EDIT = 20;

        private Color m_DefaultTextBoxColor = Color.White;                                              // Default text box color.
        private Color m_DefaultReadOnlyCellColor = Color.Gainsboro;                                     // Default cell color for read-only fields
        private Color m_ErrorColor = Color.Red;                                                         // Color to use for cells with errors
        private Color m_WarningColor = Color.Yellow;                                                    // Color to use for cells with warnings
        private Color m_PendingChangesColor = Color.Yellow;                                             // Color to use for cells with pending edits

        private GTSelectBehaviorConstants m_StartingSelectSetBehavior;                                  // Capture select set behavior at beginning of command to reset at end.    

        /// <summary>
        /// Constructor.
        /// </summary>
        public frmStreetLightVoltageDrop()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Load the Street Light Voltage Drop dialog.
        /// </summary>
        private void frmStreetLightVoltageDrop_Load(object sender, EventArgs e)
        {
            if (!InitializeForm())
            {
                Close();
                return;
            }
        }

        /// <summary>
        /// Initialize the Street Light Voltage Drop dialog
        /// </summary>
        /// <returns>Boolean indicating status</returns>
        private bool InitializeForm()
        {
            bool returnValue = false;

            try
            {
                // Process the select set
                IGTDDCKeyObjects gtDDCKeys = m_Application.SelectedObjects.GetObjects();
                m_XfmrFID = gtDDCKeys[0].FID;
                m_XfmrFNO = gtDDCKeys[0].FNO;

                if (m_XfmrFNO == ConstantsDT.FNO_OH_XFMR)
                {
                    m_XfmrUnitCNO = ConstantsDT.CNO_OH_XFMR_UNIT;
                }
                else
                {
                    m_XfmrUnitCNO = ConstantsDT.CNO_UG_XFMR_UNIT;
                }

                IGTKeyObject gtKey = m_Application.DataContext.OpenFeature(m_XfmrFNO, m_XfmrFID);
                IGTComponents gtComponents = gtKey.Components;

                // Get the Transformer CU. To be used to lookup the Transformer properties.
                Recordset componentRS = gtComponents.GetComponent(ConstantsDT.CNO_COMPUNIT).Recordset;

                if (componentRS.RecordCount > 0)
                {
                    m_XfmrCU = componentRS.Fields[ConstantsDT.FIELD_COMPUNIT_CU].Value.ToString();
                }

                // Get the metadata for the calculations
                if (!GetMetadata())
                {
                    return false;
                }

                // Initialize the controls
                cmdCalculate.Enabled = true;
                cmdApply.Enabled = false;
                cmdPrintReport.Enabled = false;
                cmdSaveReport.Enabled = false;

                txtAllowedMinimumVoltage.Text = "95";

                // Capture the select set behavior so it can be reset when the command exits.
                m_StartingSelectSetBehavior = m_Application.ActiveMapWindow.SelectBehavior;                

                // Resize the form to hide the grids
                m_FormBeginHeight = this.Size.Height;
                m_FormBeginWidth = this.Size.Width;
                this.Size = new Size(this.Width, dgvStreetLights.Location.Y + 35);
                this.FormBorderStyle = FormBorderStyle.FixedSingle;

                dgvStreetLights.Visible = false;
                dgvStreetLights.RowHeadersWidth = 10;
                dgvStreetLights.ClearSelection();
                
                returnValue = true;
            }
            catch (Exception ex)
            {
                returnValue = false;
                MessageBox.Show(m_Application.ApplicationWindow, ConstantsDT.ERROR_SC_FORM_INITIALIZATION + ": " + ex.Message, ConstantsDT.APPLICATION_NAME, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }

            return returnValue;
        }

        /// <summary>
        /// Get the metadata for the command.
        /// </summary>
        /// <returns>Boolean indicating status</returns>
        private bool GetMetadata()
        {
            bool returnValue = false;

            // Get the cable characteristics metadata
            if (!m_CommonDT.GetCableMetadata())
            {
                return false;
            }

            // Get the transformer characteristics metadata
            if (!m_CommonDT.GetTransformerMetadata())
            {
                return false;
            }                       

            // Get the properties for the selected transformer
            if (!m_CommonDT.GetTransformerProperties(m_XfmrCU, ref m_XfmrType, ref xfmrProperties))
            {
                return false;
            }

            // Get the street light characteristics metadata
            if (!m_CommonDT.GetStreetLightMetadata())
            {
                return false;
            }

            try
            {
                Recordset metadataRS = null;
                CommonDT.GetCommandMetadata("StreetLightVoltageDropCC", ref metadataRS);

                if (metadataRS.RecordCount > 0)
                {
                    string paramName = string.Empty;
                    string paramValue = string.Empty;
                    metadataRS.MoveFirst();

                    while(!metadataRS.EOF)
                    {
                        paramName = metadataRS.Fields["PARAM_NAME"].Value.ToString();
                        paramValue = metadataRS.Fields["PARAM_VALUE"].Value.ToString();

                        if (paramName == "TraceName")
                        {
                            m_traceUserName = paramValue;
                        }
                        else if (paramName == "ReportFileName")
                        {
                            m_ReportFileName = paramValue;
                        }
                        else if (paramName == "ReportName")
                        {
                            m_ReportName = paramValue;
                        }
                        else if (paramName == "NominalVoltages")
                        {
                            string [] nominalVoltages = paramValue.Split(',');
                            foreach(string nominalVoltage in nominalVoltages)
                            {
                                cboNominalVoltage.Items.Add(nominalVoltage);
                            }
                        }
                        else if (paramName == "NominalVoltageDefault")
                        {
                            m_NominalVoltage = Convert.ToDouble(paramValue); 
                            txtActualSourceVoltage.Text = Math.Round((m_NominalVoltage * 1.05)).ToString();
                            clsCalculate.ActualSourceVoltage = Convert.ToDouble(txtActualSourceVoltage.Text);
                            clsCalculate.AllowedMinimumVoltage = Convert.ToDouble(txtAllowedMinimumVoltage.Text);
                        }

                        metadataRS.MoveNext();
                    }

                    cboNominalVoltage.SelectedItem = m_NominalVoltage.ToString();

                    returnValue = true;
                }
                else
                {
                    MessageBox.Show(m_Application.ApplicationWindow, ConstantsDT.ERROR_RETRIEVING_COMMAND_METADATA, ConstantsDT.APPLICATION_NAME, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    returnValue = false;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(m_Application.ApplicationWindow, ConstantsDT.ERROR_RETRIEVING_COMMAND_METADATA + ": " + ex.Message, ConstantsDT.APPLICATION_NAME, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return false;
            }

            return returnValue;
        }
        /// <summary>
        /// Dialog closing event.
        /// </summary>
        private void frmStreetLightVoltageDrop_FormClosing(object sender, FormClosingEventArgs e)
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

            m_CommonDT.RemoveLegendItem("Calculation Problems", "Street Light Voltage Drop");

            if (m_clsTraceHelper.TraceName.Length > 0)
            {
                m_clsTraceHelper.RemoveTrace(m_clsTraceHelper.TraceName, M_DISPLAY_PATHNAME, m_clsTraceHelper.TraceName);
            }

            if (!(m_CommonDT.ConductorsDictionary == null))
            {
                m_CommonDT.ConductorsDictionary.Clear();
                m_CommonDT.ConductorsDictionary = null;
            }

            if (!(m_CommonDT.StreetLightsDictionary == null))
            {
                m_CommonDT.StreetLightsDictionary.Clear();
                m_CommonDT.StreetLightsDictionary = null;
            }

            m_clsTraceHelper = null;
            m_CommonDT = null;
            m_Application.EndWaitCursor();

            // Reset select set behavior.
            m_Application.ActiveMapWindow.SelectBehavior = m_StartingSelectSetBehavior;

            m_CustomCommandHelper.Complete();
        }

        /// <summary>
        /// Dialog closing event.
        /// </summary>
        private void cmdClose_Click(object sender, EventArgs e)
        {
            Close();
        }

        /// <summary>
        /// Options button event. Open options dialog.
        /// </summary>
        private void cmdOptions_Click(object sender, EventArgs e)
        {
            OptionsDT frmDesignToolsOptions = new OptionsDT();
            
            frmDesignToolsOptions.StartPosition = FormStartPosition.CenterParent;
            frmDesignToolsOptions.ShowDialog(this);
            frmDesignToolsOptions = null;
        }

        /// <summary>
        /// Print the calculation results to a pdf.
        /// </summary>
        private void cmdPrintReport_Click(object sender, EventArgs e)
        {
            try
            {
                m_Application.SetStatusBarText(GTStatusPanelConstants.gtaspcMessage, ConstantsDT.MESSAGE_REPORT_CREATING);
                m_Application.BeginWaitCursor();
                
                string reportVersionDate = Assembly.GetExecutingAssembly().GetName().Version.ToString();
                DateTime buildDate = new FileInfo(Assembly.GetExecutingAssembly().Location).LastWriteTime;
                reportVersionDate += "  " + buildDate.ToShortDateString();

                // Add report values
                List<KeyValuePair<string, string>> reportValues = new List<KeyValuePair<string, string>>();
                reportValues.Add(new KeyValuePair<string, string>(ConstantsDT.REPORT_PARAMETER_VERSIONDATE, reportVersionDate));
                reportValues.Add(new KeyValuePair<string, string>(ConstantsDT.REPORT_PARAMETER_WR, m_Application.DataContext.ActiveJob));
                reportValues.Add(new KeyValuePair<string, string>(ConstantsDT.REPORT_PARAMETER_DATE, DateTime.Now.ToShortDateString()));
                reportValues.Add(new KeyValuePair<string, string>(ConstantsDT.REPORT_PARAMETER_WR_DESCRIPTION, CommonDT.WrDescription));
                reportValues.Add(new KeyValuePair<string, string>(ConstantsDT.REPORT_PARAMETER_SL_NOMINAL_VOLTAGE, cboNominalVoltage.SelectedItem.ToString()));
                reportValues.Add(new KeyValuePair<string, string>(ConstantsDT.REPORT_PARAMETER_SL_SOURCE_VOLTAGE, txtActualSourceVoltage.Text));
                reportValues.Add(new KeyValuePair<string, string>(ConstantsDT.REPORT_PARAMETER_SL_ALLOWED_VOLTAGE, txtAllowedMinimumVoltage.Text + "%"));

                m_ReportDT = new ReportDT();

                m_ReportDT.ReportFile = m_ReportFileName;
                m_ReportDT.ReportName = m_ReportName;

                m_ReportDT.CreateReport(ConstantsDT.COMMAND_NAME_STREET_LIGHT_VOLTAGE_DROP, m_Application.DataContext.ActiveJob, m_XfmrFID, reportValues, dgvStreetLights, null);
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

        /// <summary>
        /// Save the report to a shared location and hyperlink the report to transformer
        /// </summary>
        private void cmdSaveReport_Click(object sender, EventArgs e)
        {
            string url = string.Empty;
            string docMgmtFileName = string.Empty;

            try
            {
                // Add report to document management
                if (m_ReportDT.UploadReport(m_ReportPDF, "Street Light Voltage Drop", ref url, ref docMgmtFileName))
                {
                    string tmpQry = "select g3e_fid from DESIGNAREA_P where JOB_ID = ?";
                    Recordset tmpRs = m_Application.DataContext.OpenRecordset(tmpQry, CursorTypeEnum.adOpenStatic, LockTypeEnum.adLockReadOnly, (int)CommandTypeEnum.adCmdText, m_Application.DataContext.ActiveJob);
                    if (!(tmpRs.BOF && tmpRs.EOF))
                    {
                        tmpRs.MoveFirst();
                        int designAreaFid = Convert.ToInt32(tmpRs.Fields["g3e_fid"].Value);

                        m_TransactionManager.Begin("New Hyperlink");
                        // Create a hyperlink component for the active feature
                        if (m_ReportDT.AddHyperlinkComponent(designAreaFid, ConstantsDT.FNO_DESIGN_AREA, url, "Street Light Voltage Drop", null))
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

        /// <summary>
        /// Call function to save the pending CU changes to the features
        /// </summary>
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

        /// <summary>
        /// Get the pending CU changes and call function to apply the changes
        /// </summary>
        /// <returns>Boolean indicating status</returns>
        private bool SavePendingChanges()
        {
            bool returnValue = false;

            try
            {
                m_TransactionManager.Begin("Edit Feature");
                m_Application.SetStatusBarText(GTStatusPanelConstants.gtaspcMessage, ConstantsDT.MESSAGE_APPLY_PENDING_CHANGES);
                m_Application.BeginWaitCursor();

                // Check each Secondary Conductor and Street Light for CU change
                foreach (DataGridViewRow r in dgvStreetLights.Rows)
                {
                    // Check for Secondary Conductor pending CU edit
                    if (Convert.ToInt16(r.Cells[M_SLGRID_CABLE_EDIT].Value) == 1)
                    {
                        if (!UpdateCU(Convert.ToInt16(r.Cells[M_SLGRID_SECONDARY_FNO].Value), Convert.ToInt32(r.Cells[M_SLGRID_SECONDARY_FID].Value), 
                                        r.Cells[M_SLGRID_CABLECU].Value.ToString()))
                        {
                            m_TransactionManager.Rollback();
                            return false;
                        }
                        r.Cells[M_SLGRID_CABLE_EDIT].Value = 0;
                        r.Cells[M_SLGRID_CABLE].Style.BackColor = Color.White;
                    }

                    // Check for Street Light pending CU edit
                    if (Convert.ToInt16(r.Cells[M_SLGRID_LIGHT_EDIT].Value) == 1)
                    {
                        if (!UpdateCU(ConstantsDT.FNO_STREETLIGHT, Convert.ToInt32(r.Cells[M_SLGRID_LIGHT_FID].Value), r.Cells[M_SLGRID_LIGHTCU].Value.ToString()))
                        {
                            m_TransactionManager.Rollback();
                            return false;
                        }
                        r.Cells[M_SLGRID_LIGHT_EDIT].Value = 0;
                        r.Cells[M_SLGRID_BALLAST].Style.BackColor = Color.White;
                    }

                    // Check for Secondary Conductor pending length edit
                    if (Convert.ToInt16(r.Cells[M_SLGRID_LENGTH_EDIT].Value) == 1)
                    {
                        if (!UpdateLength(Convert.ToInt16(r.Cells[M_SLGRID_SECONDARY_FNO].Value), Convert.ToInt32(r.Cells[M_SLGRID_SECONDARY_FID].Value), Convert.ToDouble(r.Cells[M_SLGRID_LENGTH].Value)))
                        {
                            m_TransactionManager.Rollback();
                            return false;
                        }
                        r.Cells[M_SLGRID_LENGTH_EDIT].Value = 0;
                        r.Cells[M_SLGRID_LENGTH].Style.BackColor = Color.White;
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

        /// <summary>
        /// Save the pending CU change to the feature
        /// </summary>
        /// <param name="fno">G3E_FNO of the feature to update</param>
        /// <param name="fid">G3E_FID of the feature to update</param>
        /// <param name="cu">CU value to use for update</param>
        /// <returns>Boolean indicating status</returns>
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

        /// <summary>
        /// Save the pending length change to the feature
        /// </summary>
        /// <param name="fno">G3E_FNO of the feature to update</param>
        /// <param name="fid">G3E_FID of the feature to update</param>
        /// <param name="cu">Length value to use for update</param>
        /// <returns>Boolean indicating status</returns>
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

        /// <summary>
        /// Trace the secondary network. Perform calculations and display the results.
        /// </summary>
        private void cmdCalculate_Click(object sender, EventArgs e)
        {
            try
            {
                m_Application.BeginWaitCursor();

                // Reset the results for a new calculation.
                ResetResultsForCalculation();

                if (m_FirstCalculation)
                {
                    // Setup the trace
                    m_clsTraceHelper.SeedFID = m_XfmrFID;
                    m_clsTraceHelper.TraceMetadataUserName = m_traceUserName;
                    m_clsTraceHelper.ApplicationName = ConstantsDT.COMMAND_NAME_STREET_LIGHT_VOLTAGE_DROP;

                    m_Application.SetStatusBarText(GTStatusPanelConstants.gtaspcMessage, ConstantsDT.MESSAGE_TRACE_EXECUTING);

                    // Trace the network
                    if (!m_clsTraceHelper.ExecuteTrace())
                    {
                        m_Application.SetStatusBarText(GTStatusPanelConstants.gtaspcMessage, "");
                        m_Application.EndWaitCursor();
                        return;
                    }

                    // Process the trace results
                    m_Application.SetStatusBarText(GTStatusPanelConstants.gtaspcMessage, ConstantsDT.MESSAGE_TRACE_PROCESSING_RESULTS);
                    if (!m_CommonDT.ProcessTraceResults(m_clsTraceHelper.TraceName))
                    {
                        m_Application.SetStatusBarText(GTStatusPanelConstants.gtaspcMessage, "");
                        m_Application.EndWaitCursor();
                        return;
                    }

                    // Resize the form to show the grid
                    this.FormBorderStyle = FormBorderStyle.Sizable;
                    this.Size = new Size(m_FormBeginWidth, m_FormBeginHeight);                    
                    m_FirstCalculation = false;
                }

                // Call function to add trace results to legend if output to map window option has been selected.
                m_CommonDT.DisplayResults(m_CommonDT.TraceResultsRS, M_DISPLAY_PATHNAME, m_clsTraceHelper.TraceName, CommonDT.DisplayResultsType.Trace);

                // Run the calculations
                m_Application.SetStatusBarText(GTStatusPanelConstants.gtaspcMessage, ConstantsDT.MESSAGE_CALCULATING);

                if (!Calculate())
                {
                    m_Application.SetStatusBarText(GTStatusPanelConstants.gtaspcMessage, "");
                    m_Application.EndWaitCursor();
                    return;
                }

                //dgvStreetLights.Rows.Clear();
                if (dgvStreetLights.RowCount == 0)
                {
                    // Add the results to the grid
                    if (!PopulateGrid())
                    {
                        m_Application.SetStatusBarText(GTStatusPanelConstants.gtaspcMessage, "");
                        m_Application.EndWaitCursor();
                        return;
                    }
                }
                else
                {
                    // Update the results in the grid
                    if (!UpdateGrid())
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

                dgvStreetLights.Visible = true;

                m_Application.SetStatusBarText(GTStatusPanelConstants.gtaspcMessage, "");

                dgvStreetLights.ClearSelection();

                cmdPrintReport.Enabled = true;

                m_Application.EndWaitCursor();
            }
            catch (Exception ex)
            {
                m_Application.EndWaitCursor();
                MessageBox.Show(m_Application.ApplicationWindow, ConstantsDT.ERROR_SC_CALCULATE + ": " + ex.Message, ConstantsDT.APPLICATION_NAME, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        /// <summary>
        /// Run the calculation
        /// </summary>
        /// <returns>Boolean indicating status</returns>
        private bool Calculate()
        {
            bool returnValue = false;

            try
            {
                if (m_CommonDT.ConductorsDictionary != null)
                {
                    double upstreamVlmag = clsCalculate.ActualSourceVoltage;
                    double vlmag = 0;
                    double loadAmps = 0;
                    int lightCount = 0;
                    string errorMessage = string.Empty;
                    double totalUpstreamVoltageDrop = 0;
                    Conductor upstreamConductor = null;

                    foreach (Conductor conductor in m_CommonDT.ConductorsDictionary.Values)
                    {
                        loadAmps = 0;
                        lightCount = 0;

                        if (m_CommonDT.ConductorsDictionary.TryGetValue(conductor.SourceFID, out upstreamConductor))
                        {
                            upstreamVlmag = upstreamConductor.VoltageMag;
                            totalUpstreamVoltageDrop = upstreamConductor.AccumVoltageDropPercent;
                        }

                        if (!GetLoadAmps(conductor, upstreamVlmag, ref vlmag, ref lightCount, ref loadAmps, ref errorMessage))
                        {
                            MessageBox.Show(m_Application.ApplicationWindow, ConstantsDT.ERROR_SL_CALCULATE_LOADAMPS +": " + errorMessage, ConstantsDT.APPLICATION_NAME, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                            return false;
                        }

                        if (conductor.StreetLight != null)
                        {
                            if (!clsCalculate.CalculateStreetLightVoltageDrop(conductor, upstreamVlmag, loadAmps, ref vlmag, ref errorMessage))
                            {
                                MessageBox.Show(m_Application.ApplicationWindow, ConstantsDT.ERROR_SL_CALCULATE_VDROP + ": " + errorMessage, ConstantsDT.APPLICATION_NAME, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                                return false;
                            }
                        }                        

                        conductor.LoadAmps = loadAmps;
                        conductor.NumberOfLights = lightCount;
                        conductor.VoltageDrop = vlmag;
                        if (conductor.StreetLight != null)
                        {
                            conductor.AllowedMinVoltage = conductor.StreetLight.StreetLightProperties.voltage * clsCalculate.AllowedMinimumVoltage / 100;
                        }
                            
                        conductor.VoltageMag = vlmag;
                        conductor.VoltageDropPercent = (upstreamVlmag - vlmag) / vlmag;
                        totalUpstreamVoltageDrop += conductor.VoltageDropPercent;
                        conductor.AccumVoltageDropPercent = totalUpstreamVoltageDrop;
                    }
                }

                returnValue = true;
            }
            catch (Exception ex)
            {
                returnValue = false;
                MessageBox.Show(m_Application.ApplicationWindow, ConstantsDT.ERROR_SL_CALCULATE + ": " + ex.Message, ConstantsDT.APPLICATION_NAME, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }

            return returnValue;
        }

        /// <summary>
        /// Populate the dialog grid with the calulation results
        /// </summary>
        /// <returns>Boolean indicating status</returns>
        private bool PopulateGrid()
        {
            bool returnValue = false;

            try
            {
                if (m_CommonDT.ConductorsDictionary != null)
                {
                    foreach (Conductor conductor in m_CommonDT.ConductorsDictionary.Values)
                    {
                        if (!AddStreetLightToGrid(conductor))
                        {
                            return false;
                        }
                    }
                    SetCellAccess();
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

        /// <summary>
        /// Sets the CU and length cells to read-only if feature isn't part of WR
        /// </summary>
        /// <returns>Boolean indicating status</returns>
        private bool SetCellAccess()
        {
            bool returnValue = false;

            try
            {
                if (m_CommonDT.ConductorsDictionary != null)
                {
                    ADODB.Recordset wrRS = null;
                    m_CommonDT.GetWrData(ref wrRS);

                    int fid;
                    // Set appropriate cells to read-only if feature isn't part of WR.
                    foreach (DataGridViewRow r in dgvStreetLights.Rows)
                    {
                        // Check if current Secondary Conductor FID is part of WR
                        fid = Convert.ToInt32(r.Cells[M_SLGRID_SECONDARY_FID].Value);

                        if (wrRS.RecordCount > 0)
                        {
                            wrRS.Filter = "g3e_fid = " + fid;
                        }

                        if (wrRS.RecordCount == 0)
                        {
                            r.Cells[M_SLGRID_CABLE].Style.BackColor = m_DefaultReadOnlyCellColor;
                            r.Cells[M_SLGRID_CABLE].ReadOnly = true;

                            r.Cells[M_SLGRID_LENGTH].Style.BackColor = m_DefaultReadOnlyCellColor;
                            r.Cells[M_SLGRID_LENGTH].ReadOnly = true;
                        }

                        // Check if current Street Light FID is part of WR
                        fid = Convert.ToInt32(r.Cells[M_SLGRID_LIGHT_FID].Value);

                        if (wrRS.RecordCount > 0)
                        {
                            wrRS.Filter = "g3e_fid = " + fid;
                        }

                        if (wrRS.RecordCount == 0)
                        {
                            r.Cells[M_SLGRID_BALLAST].Style.BackColor = m_DefaultReadOnlyCellColor;
                            r.Cells[M_SLGRID_BALLAST].ReadOnly = true;
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

        /// <summary>
        /// Update the results in the dialog grid with the values in the dictionaries
        /// </summary>
        /// <returns>Boolean indicating status</returns>
        private bool UpdateGrid()
        {
            bool returnValue = false;

            try
            {
                Int32 fid = 0;
                Conductor conductor = null;
                foreach (DataGridViewRow r in dgvStreetLights.Rows)
                {
                    fid = Convert.ToInt32(r.Cells[M_SLGRID_SECONDARY_FID].Value);

                    if (m_CommonDT.ConductorsDictionary.TryGetValue(fid, out conductor))
                    {
                        r.Cells[M_SLGRID_LOAD].Value = conductor.LoadAmps;
                        r.Cells[M_SLGRID_VLMAG].Value = Math.Round(conductor.VoltageMag, 2);
                        r.Cells[M_SLGRID_ALLOWED_MIN].Value = conductor.AllowedMinVoltage;
                        r.Cells[M_SLGRID_VDROP].Value = conductor.VoltageDropPercent;
                        r.Cells[M_SLGRID_TOTAL_VDROP].Value = conductor.AccumVoltageDropPercent;
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

        /// <summary>
        /// Add a Cable and Street Light to the grid
        /// </summary>
        /// <param name="conductor">The conductor object to add to the grid</param>
        /// <returns>Boolean indicating status</returns>
        private bool AddStreetLightToGrid(Conductor conductor)
        {
            bool returnValue = false;

            try
            {
                DataGridViewRow r;

                r = dgvStreetLights.Rows[dgvStreetLights.Rows.Add()];                
                r.Cells[M_SLGRID_SECONDARY_FID].Value = conductor.ConductorFID;
                r.Cells[M_SLGRID_SECONDARY_FNO].Value = conductor.FNO;                
                r.Cells[M_SLGRID_CABLE].Value = conductor.CableType;                
                r.Cells[M_SLGRID_LENGTH].Value = conductor.Length;
                r.Cells[M_SLGRID_COUNT].Value = conductor.NumberOfLights;                
                r.Cells[M_SLGRID_LOAD].Value = conductor.LoadAmps;
                r.Cells[M_SLGRID_VLMAG].Value = Math.Round(conductor.VoltageMag, 2);
                r.Cells[M_SLGRID_ALLOWED_MIN].Value = conductor.AllowedMinVoltage;
                r.Cells[M_SLGRID_VDROP].Value = conductor.VoltageDropPercent;
                r.Cells[M_SLGRID_TOTAL_VDROP].Value = conductor.AccumVoltageDropPercent;
                r.Cells[M_SLGRID_AMPACITY].Value = conductor.CableProperties.Ampacity;
                r.Cells[M_SLGRID_CABLECU].Value = conductor.CU;

                if (conductor.StreetLight != null)
                {
                    r.Cells[M_SLGRID_LIGHT_FID].Value = conductor.StreetLight.FID;
                    r.Cells[M_SLGRID_FROM].Value = conductor.StreetLight.SpanIndexFrom;
                    r.Cells[M_SLGRID_TO].Value = conductor.StreetLight.SpanIndexTo;
                    r.Cells[M_SLGRID_BALLAST].Value = conductor.StreetLight.LightType;
                    r.Cells[M_SLGRID_AMPS].Value = conductor.StreetLight.StreetLightProperties.maxAmps;
                    r.Cells[M_SLGRID_LIGHTCU].Value = conductor.StreetLight.CU;

                    if (conductor.StreetLight.StreetLightProperties.voltage != m_NominalVoltage)
                    {
                        cboNominalVoltage.BackColor = m_ErrorColor;
                        r.Cells[M_SLGRID_BALLAST].Style.BackColor = m_ErrorColor;
                    }

                    if (!conductor.StreetLight.AllowCUEdit)
                    {
                        r.Cells[M_SLGRID_BALLAST].ReadOnly = true;
                        r.Cells[M_SLGRID_BALLAST].Style.BackColor = m_DefaultReadOnlyCellColor;
                    }
                }

                r.Cells[M_SLGRID_CABLE_EDIT].Value = 0;                
                r.Cells[M_SLGRID_LIGHT_EDIT].Value = 0;
                r.Cells[M_SLGRID_LENGTH_EDIT].Value = 0;

                if (!conductor.AllowCUEdit)
                {
                    r.Cells[M_SLGRID_CABLE].ReadOnly = true;
                    r.Cells[M_SLGRID_CABLE].Style.BackColor = m_DefaultReadOnlyCellColor;
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

        /// <summary>
        /// Calculate the downstream load
        /// </summary>
        /// <param name="conductor">Conductor to calculate</param>
        /// <param name="upstreamVlmag">The upstream voltage drop</param>
        /// <param name="vlMag">The calculated voltage drop for the conductor</param>
        /// <param name="lightCount">The downstream light count including light on current conductor</param>
        /// <param name="loadAmps">The calculated load amps for the conductor</param>
        /// <param name="errorMessage">The error message if the calculation fails</param>
        /// <returns>Boolean indicating status</returns>
        public bool GetLoadAmps(Conductor conductor, double upstreamVlmag, ref double vlmag, ref int lightCount, ref double loadAmps, ref string errorMessage)
        {
            bool returnValue = false;

            try
            {
                double lowestVlmag = 0;
                double loadAmpsToUse = 0;
                int totalLightCount = 0;
                bool foundDownStreamConductor = false;

                foreach (Conductor downstreamConductor in m_CommonDT.ConductorsDictionary.Values)
                {
                    if (downstreamConductor.SourceFID == conductor.ConductorFID)
                    {
                        if (!GetLoadAmps(downstreamConductor, upstreamVlmag, ref vlmag, ref lightCount, ref loadAmps, ref errorMessage))
                        {
                            MessageBox.Show(m_Application.ApplicationWindow, ConstantsDT.ERROR_SL_CALCULATE_LOADAMPS + ": " + errorMessage, ConstantsDT.APPLICATION_NAME, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                            return false;
                        }

                        // Use the branch with the highest voltage drop
                        if (vlmag < lowestVlmag || lowestVlmag == 0)
                        {
                            lowestVlmag = vlmag;
                            loadAmpsToUse = loadAmps;
                            totalLightCount += lightCount;
                        }

                        foundDownStreamConductor = true;
                    }
                }

                if (foundDownStreamConductor)
                {
                    loadAmps = loadAmpsToUse + conductor.StreetLight.StreetLightProperties.maxAmps;
                    lightCount = totalLightCount + 1;
                }
                else
                {
                    if (conductor.StreetLight != null)
                    {
                        loadAmps = conductor.StreetLight.StreetLightProperties.maxAmps;
                        lightCount = 1;
                    }
                }

                if (conductor.StreetLight != null)
                {
                    if (!clsCalculate.CalculateStreetLightVoltageDrop(conductor, upstreamVlmag, loadAmps, ref vlmag, ref errorMessage))
                    {
                        MessageBox.Show(m_Application.ApplicationWindow, ConstantsDT.ERROR_SL_CALCULATE_VDROP + ": " + errorMessage, ConstantsDT.APPLICATION_NAME, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        return false;
                    }
                }                    

                upstreamVlmag = vlmag;

                returnValue = true;
            }
            catch (Exception ex)
            {
                returnValue = false;
                errorMessage = ex.Message;
            }

            return returnValue;
        }

        /// <summary>
        /// Validate the calculation results.
        /// </summary>
        /// <returns>Boolean indicating status</returns>
        private bool ValidateResults()
        {
            string validationStatus = string.Empty;
            string validationPriority = string.Empty;
            string comments = string.Empty;
            string sql = string.Empty;
            int secondaryFid = 0;
            int streetlightFid = 0;
            int recordsAffected = 0;
            Recordset validationRS;
            Recordset errorsRS = null;
            string voltageDrop = string.Empty;
            string loadAmps = string.Empty;
            bool returnValue = false;

            try
            {
                foreach (DataGridViewRow r in dgvStreetLights.Rows)
                {
                    validationStatus = ConstantsDT.VALIDATION_PASS;
                    validationPriority = "";
                    comments = "";
                    secondaryFid = Convert.ToInt32(r.Cells[M_SLGRID_SECONDARY_FID].Value);
                    streetlightFid = Convert.ToInt32(r.Cells[M_SLGRID_LIGHT_FID].Value);

                    // Color Nominal Voltage textbox and the grids Load Amps and Vlmag cells where thresholds are exceeded.
                    voltageDrop = r.Cells[M_SLGRID_VLMAG].Value.ToString();
                    if (Convert.ToDouble(r.Cells[M_SLGRID_VLMAG].Value) < Convert.ToDouble(r.Cells[M_SLGRID_ALLOWED_MIN].Value))
                    {
                        r.Cells[M_SLGRID_VLMAG].Style.BackColor = m_ErrorColor;
                        voltageDrop += " - EXCEEDED";
                        validationStatus = ConstantsDT.VALIDATION_FAIL;
                        validationPriority = ConstantsDT.VALIDATION_PRIORITY;
                    }

                    loadAmps = r.Cells[M_SLGRID_LOAD].Value.ToString();
                    if (Convert.ToDouble(r.Cells[M_SLGRID_LOAD].Value) > Convert.ToDouble(r.Cells[M_SLGRID_AMPACITY].Value))
                    {
                        r.Cells[M_SLGRID_LOAD].Style.BackColor = m_ErrorColor;
                        loadAmps += " - EXCEEDED";
                        validationStatus = ConstantsDT.VALIDATION_FAIL;
                        validationPriority = ConstantsDT.VALIDATION_PRIORITY;
                    }

                    comments = "Voltage Drop: " + voltageDrop + "; Load Amps: " + loadAmps + ";";

                    // Check if record exists for WR Number, Secondary Conductor FID and command
                    validationRS = m_Application.DataContext.OpenRecordset(ConstantsDT.QUERY_VALIDATION, CursorTypeEnum.adOpenDynamic,
                                   LockTypeEnum.adLockOptimistic, (int)CommandTypeEnum.adCmdText, m_Application.DataContext.ActiveJob, secondaryFid, ConstantsDT.COMMAND_NAME_STREET_LIGHT_VOLTAGE_DROP);

                    if (validationRS.RecordCount > 0)
                    {
                        // Update record
                        m_Application.DataContext.Execute(ConstantsDT.SQL_UPDATE_VALIDATION, out recordsAffected, (int)CommandTypeEnum.adCmdText,
                                                          validationStatus, validationPriority, comments, m_Application.DataContext.ActiveJob, secondaryFid, ConstantsDT.COMMAND_NAME_STREET_LIGHT_VOLTAGE_DROP);
                    }
                    else
                    {
                        // Add record
                        m_Application.DataContext.Execute(ConstantsDT.SQL_INSERT_VALIDATION, out recordsAffected, (int)CommandTypeEnum.adCmdText,
                                                          m_Application.DataContext.ActiveJob, secondaryFid, ConstantsDT.COMMAND_NAME_STREET_LIGHT_VOLTAGE_DROP, validationStatus,
                                                          validationPriority, comments);
                    }

                    // Check if record exists for WR Number, Street Light FID and command
                    validationRS = m_Application.DataContext.OpenRecordset(ConstantsDT.QUERY_VALIDATION, CursorTypeEnum.adOpenDynamic,
                                   LockTypeEnum.adLockOptimistic, (int)CommandTypeEnum.adCmdText, m_Application.DataContext.ActiveJob, streetlightFid, ConstantsDT.COMMAND_NAME_STREET_LIGHT_VOLTAGE_DROP);

                    if (validationRS.RecordCount > 0)
                    {
                        // Update record
                        m_Application.DataContext.Execute(ConstantsDT.SQL_UPDATE_VALIDATION, out recordsAffected, (int)CommandTypeEnum.adCmdText,
                                                          validationStatus, validationPriority, comments, m_Application.DataContext.ActiveJob, streetlightFid, ConstantsDT.COMMAND_NAME_STREET_LIGHT_VOLTAGE_DROP);
                    }
                    else
                    {
                        // Add record
                        m_Application.DataContext.Execute(ConstantsDT.SQL_INSERT_VALIDATION, out recordsAffected, (int)CommandTypeEnum.adCmdText,
                                                          m_Application.DataContext.ActiveJob, streetlightFid, ConstantsDT.COMMAND_NAME_STREET_LIGHT_VOLTAGE_DROP, validationStatus,
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
                        errorsRS.Fields["G3E_FNO"].Value = ConstantsDT.FNO_OH_SECCOND;
                        errorsRS.Fields["G3E_FID"].Value = secondaryFid;

                        errorsRS.AddNew();
                        errorsRS.Fields["G3E_FNO"].Value = ConstantsDT.FNO_STREETLIGHT;
                        errorsRS.Fields["G3E_FID"].Value = streetlightFid;
                    }
                }

                // Commit validation records to database.
                m_Application.DataContext.Execute("commit", out recordsAffected, (int)CommandTypeEnum.adCmdText);

                if (errorsRS != null)
                {
                    m_CommonDT.DisplayResults(errorsRS, "Calculation Problems", "Street Light Voltage Drop", CommonDT.DisplayResultsType.Problem);
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
        
        /// <summary>
        /// Reset the results for a new calculation.
        /// </summary>
        /// <returns>Boolean indicating status</returns>
        public void ResetResultsForCalculation()
        {
            cmdPrintReport.Enabled = false;
            cmdSaveReport.Enabled = false;
            m_PassedValidation = false;

            foreach (DataGridViewRow r in dgvStreetLights.Rows)
            {
                //  Set the grid cells back to the initial color.
                r.Cells[M_SLGRID_ALLOWED_MIN].Style.BackColor = m_DefaultReadOnlyCellColor;
                r.Cells[M_SLGRID_VLMAG].Style.BackColor = m_DefaultReadOnlyCellColor;
                r.Cells[M_SLGRID_LOAD].Value = null;
                r.Cells[M_SLGRID_VLMAG].Value = null;
                r.Cells[M_SLGRID_VDROP].Value = null;
                r.Cells[M_SLGRID_TOTAL_VDROP].Value = null;
            }
        }

        /// <summary>
        /// Restrict Actual Source Voltage values to a number
        /// </summary>
        private void txtActualSourceVoltage_KeyPress(object sender, KeyPressEventArgs e)
        {
            // Allow user to only enter numbers
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        /// <summary>
        /// Actual Source Voltage change event
        /// </summary>
        private void txtActualSourceVoltage_TextChanged(object sender, EventArgs e)
        {
            ResetResultsForCalculation();

            if (txtActualSourceVoltage.Text.Length > 0)
            {
                clsCalculate.ActualSourceVoltage = Convert.ToDouble(txtActualSourceVoltage.Text);
            }
            else
            {
                clsCalculate.ActualSourceVoltage = 0;
            }
        }

        /// <summary>
        /// Restrict Allowed Minimum Voltage values to a number
        /// </summary>
        private void txtAllowedMinimumVoltage_KeyPress(object sender, KeyPressEventArgs e)
        {
            // Allow user to only enter numbers
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        /// <summary>
        /// Actual Minimum Voltage change event
        /// </summary>
        private void txtAllowedMinimumVoltage_TextChanged(object sender, EventArgs e)
        {
            ResetResultsForCalculation();

            foreach (DataGridViewRow r in dgvStreetLights.Rows)
            {
                r.Cells[M_SLGRID_ALLOWED_MIN].Value = null;
            }

            if (txtAllowedMinimumVoltage.Text.Length > 0)
            {
                clsCalculate.AllowedMinimumVoltage = Convert.ToDouble(txtAllowedMinimumVoltage.Text);
            }
            else
            {
                clsCalculate.AllowedMinimumVoltage = 0;
            }            
        }

        /// <summary>
        /// Grid selection change event. Highlight features in map window
        /// </summary>
        private void dgvStreetLights_SelectionChanged(object sender, EventArgs e)
        {
            m_Application.SelectedObjects.Clear();

            if (dgvStreetLights.SelectedRows.Count > 0)
            {
                foreach (DataGridViewRow r in dgvStreetLights.SelectedRows)
                {
                    m_CommonDT.UpdateMapWindow(Convert.ToInt16(r.Cells[M_SLGRID_SECONDARY_FNO].Value), Convert.ToInt32(r.Cells[M_SLGRID_SECONDARY_FID].Value));
                    m_CommonDT.UpdateMapWindow(ConstantsDT.FNO_STREETLIGHT, Convert.ToInt32(r.Cells[M_SLGRID_LIGHT_FID].Value));
                }
            }
        }

        /// <summary>
        /// CU change event. Launch CU dialog and capture selection.
        /// </summary>
        private void dgvStreetLights_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            bool cuChange = false;
            try
            {
                if (e.ColumnIndex == M_SLGRID_CABLE && e.RowIndex != -1)
                {
                    // Don't allow edit if cell is read-only which indicates feature isn't part of WR.
                    if (dgvStreetLights.Rows[e.RowIndex].Cells[M_SLGRID_CABLE].ReadOnly)
                    {
                        return;
                    }

                    int fid = 0;
                    string cu = string.Empty;

                    // TODO: JIRA 828 - Launch CU Selection
                    // temporary solution
                    frmSelectCU selectCU = new frmSelectCU(CommonDT.CableRS);
                    selectCU.StartPosition = FormStartPosition.CenterParent;
                    selectCU.ShowDialog(m_Application.ApplicationWindow);

                    if (selectCU.CU.Length > 0)
                    {
                        cu = selectCU.CU;
                    }
                    else
                    {
                        cu = dgvStreetLights.Rows[e.RowIndex].Cells[M_SLGRID_CABLECU].Value.ToString();
                    }

                    fid = Convert.ToInt32(dgvStreetLights.Rows[e.RowIndex].Cells[M_SLGRID_SECONDARY_FID].Value);

                    Conductor conductor;

                    if (m_CommonDT.ConductorsDictionary.TryGetValue(fid, out conductor))
                    {
                        // If CU is different than current CU, then disable the Print Report command. A new calculation will need to be executed. 
                        if (cu != conductor.CU)
                        {
                            cuChange = true;
                            dgvStreetLights.Rows[e.RowIndex].Cells[M_SLGRID_CABLECU].Value = cu;
                            dgvStreetLights.Rows[e.RowIndex].Cells[e.ColumnIndex].Style.BackColor = m_PendingChangesColor;
                            dgvStreetLights.Rows[e.RowIndex].Cells[M_SLGRID_CABLE_EDIT].Value = 1;
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

                            dgvStreetLights.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = description;
                        }
                    }
                }
                else if (e.ColumnIndex == M_SLGRID_BALLAST && e.RowIndex != -1)
                {
                    // Don't allow edit if cell is read-only which indicates feature isn't part of WR.
                    if (dgvStreetLights.Rows[e.RowIndex].Cells[M_SLGRID_BALLAST].ReadOnly)
                    {
                        return;
                    }

                    int fid = 0;
                    string cu = string.Empty;

                    // TODO: JIRA 828 - Launch CU Selection
                    // temporary solution
                    frmSelectCU selectCU = new frmSelectCU(CommonDT.StreetLightRS);
                    selectCU.StartPosition = FormStartPosition.CenterParent;
                    selectCU.ShowDialog(m_Application.ApplicationWindow);

                    if (selectCU.CU.Length > 0)
                    {
                        cu = selectCU.CU;
                    }
                    else
                    {
                        cu = dgvStreetLights.Rows[e.RowIndex].Cells[M_SLGRID_LIGHTCU].Value.ToString();
                    }

                    fid = Convert.ToInt32(dgvStreetLights.Rows[e.RowIndex].Cells[M_SLGRID_LIGHT_FID].Value);

                    StreetLight streetLight;

                    if (m_CommonDT.StreetLightsDictionary.TryGetValue(fid, out streetLight))
                    {
                        // If CU is different than current CU, then disable the Print Report command. A new calculation will need to be executed. 
                        if (cu != streetLight.CU)
                        {
                            cuChange = true;
                            dgvStreetLights.Rows[e.RowIndex].Cells[M_SLGRID_LIGHTCU].Value = cu;
                            dgvStreetLights.Rows[e.RowIndex].Cells[e.ColumnIndex].Style.BackColor = m_PendingChangesColor;
                            dgvStreetLights.Rows[e.RowIndex].Cells[M_SLGRID_LIGHT_EDIT].Value = 1;
                            cmdApply.Enabled = true;

                            CommonDT.StreetLightProperties streetLightProperties = new CommonDT.StreetLightProperties();
                            string description = string.Empty;

                            if (!m_CommonDT.GetStreetLightProperties(cu, ref description, ref streetLightProperties))
                            {
                                return;
                            }

                            streetLight.StreetLightProperties = streetLightProperties;
                            streetLight.CU = cu;
                            streetLight.LightType = description;

                            // If the ballast voltage does not match the Nominal Voltage field then 
                            //  the cell and the Nominal Voltage field will be highlighted in red.
                            if (streetLightProperties.voltage != m_NominalVoltage)
                            {
                                cboNominalVoltage.BackColor = m_ErrorColor;
                                dgvStreetLights.Rows[e.RowIndex].Cells[e.ColumnIndex].Style.BackColor = m_ErrorColor;
                            }
                            else
                            {
                                // Check if any of the Street Light voltages differ from the nominal voltage.
                                // If the voltages are all the same then set the Nominal textbox color to the default.
                                bool invalidVoltage = false;
                                
                                foreach (StreetLight sLight in m_CommonDT.StreetLightsDictionary.Values)
                                {
                                    if (sLight.StreetLightProperties.voltage != m_NominalVoltage)
                                    {
                                        invalidVoltage = true;
                                    }
                                }
                                if (!invalidVoltage)
                                {
                                    cboNominalVoltage.BackColor = m_DefaultTextBoxColor;
                                }
                            }

                            dgvStreetLights.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = description;
                            dgvStreetLights.Rows[e.RowIndex].Cells[M_SLGRID_AMPS].Value = streetLight.StreetLightProperties.maxAmps;
                        }
                    }
                }

                if (cuChange)
                {
                    // Clear the calculation results
                    ResetResultsForCalculation();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(m_Application.ApplicationWindow, ConstantsDT.ERROR_SC_GRID_CELL_CLICK + ": " + ex.Message, ConstantsDT.APPLICATION_NAME, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        /// <summary>
        /// Length change event. Highlight change in grid and flag edit.
        /// </summary>
        private void dgvStreetLights_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (e.ColumnIndex == M_SLGRID_LENGTH && e.RowIndex != -1)
                {
                    int fid = 0;
                    double length = 0;

                    if (dgvStreetLights.Rows[e.RowIndex].Cells[M_SLGRID_LENGTH].Value.ToString().Length > 0)
                    {
                        length = Convert.ToDouble(dgvStreetLights.Rows[e.RowIndex].Cells[M_SLGRID_LENGTH].Value);
                    }
                    
                    fid = Convert.ToInt32(dgvStreetLights.Rows[e.RowIndex].Cells[M_SLGRID_SECONDARY_FID].Value);

                    Conductor conductor;

                    if (m_CommonDT.ConductorsDictionary.TryGetValue(fid, out conductor))
                    {
                        // If length is different than current length, then disable the Print Report command. A new calculation will need to be executed. 
                        if (length != conductor.Length)
                        {
                            dgvStreetLights.Rows[e.RowIndex].Cells[e.ColumnIndex].Style.BackColor = m_PendingChangesColor;
                            dgvStreetLights.Rows[e.RowIndex].Cells[M_SLGRID_LENGTH_EDIT].Value = 1;
                            cmdApply.Enabled = true;
                            cmdPrintReport.Enabled = false;

                            conductor.Length = length;

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

        /// <summary>
        /// Nominal Voltage change event
        /// </summary>
        private void cboNominalVoltage_SelectedValueChanged(object sender, EventArgs e)
        {
            m_NominalVoltage = Convert.ToDouble(cboNominalVoltage.SelectedItem);
            txtActualSourceVoltage.Text = Math.Round((m_NominalVoltage * 1.05)).ToString();
            clsCalculate.ActualSourceVoltage = Convert.ToDouble(txtActualSourceVoltage.Text);
            clsCalculate.AllowedMinimumVoltage = Convert.ToDouble(txtAllowedMinimumVoltage.Text);
        }
    }
}
