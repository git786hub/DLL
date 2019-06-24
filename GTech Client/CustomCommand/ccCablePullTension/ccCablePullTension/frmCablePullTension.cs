using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using Intergraph.GTechnology.API;
using ADODB;
using System.Collections.Generic;
using System.IO;
using System.Reflection;

namespace GTechnology.Oncor.CustomAPI
{
    public partial class frmCablePullTension : Form
    {
        public IGTCustomCommandHelper m_CustomCommandHelper;
        public IGTTransactionManager m_TransactionManager;
        private IGTApplication m_Application = GTClassFactory.Create<IGTApplication>();
        //private CUService CU_Service = GTClassFactory.Create<CUService>();
        private clsCPTCalculate m_CPTCalculate = new clsCPTCalculate();
        private IGTRelationshipService relationshipService = GTClassFactory.Create<IGTRelationshipService>();
        clsCPTProcessSelectSet clsCPTProcessSelectSet;
        ReportDT m_ReportDT;
        private string m_ReportPDF;
        private bool m_bDesignMode = false;
        private int m_SelectedFID;
        private short m_SelectedFNO;
        private string m_SelectedVoltage = string.Empty;
        private string m_SelectedConductor = string.Empty;
        private string m_SelectedCableType = string.Empty;
        private string m_CableCU = string.Empty;
        private bool m_DuctPendingEdit = false;
        private double m_JamRatioLowerBound = 0;
        private double m_JamRatioUpperBound = 0;
        private string m_ReportFileName = string.Empty;
        private string m_ReportName = string.Empty;
        private int m_RowIndex = 0;        
        private bool m_SkipStartEndBendAction = false;
        private bool m_SkipSectionDefaults = false;
        private int indexFactor = 0;
        private IGTGeometryEditService m_oSegmentGES;

        private Color m_defaultTextBoxColor;
        private Color m_defaultReadOnlyCellColor;
        private Color m_ErrorColor = Color.Red;
        private Color m_WarningColor = Color.Yellow;
        private Color m_PendingChangesColor = Color.Yellow;

        private bool m_PassedValidation = false;

        private Recordset m_oCableRS;
        private Recordset m_oDuctSizeRS;
        private Recordset m_oValidationRS;        

        public frmCablePullTension()
        {
            InitializeComponent();
        }

        private void frmCablePullTension_Load(object sender, EventArgs e)
        {
            if (!InitializeForm())
            {
                Hide();
                Close();
                return;
            }
        }

        // Perform the calculations in the reverse order by reversing the grid and rerunning the calculations
        private void cmdReversePullDirection_Click(object sender, EventArgs e)
        {
            try
            {
                // Reverse the grid records
                if (dgvSections.SortOrder == SortOrder.Ascending || dgvSections.SortOrder == SortOrder.None)
                {
                    dgvSections.Sort(dgvSections.Columns[clsCPTCalculate.M_GR_ROW_INDEX], ListSortDirection.Descending);
                }
                else
                {
                    dgvSections.Sort(dgvSections.Columns[clsCPTCalculate.M_GR_ROW_INDEX], ListSortDirection.Ascending);
                }

                // The next lines of code will potentially be checking and unchecking the Starting
                // and Ending bend checkboxes. Raise m_SkipStartEndBendAction flag so grid records
                // are not added or removed due to checkboxes changing.
                m_SkipStartEndBendAction = true;

                // Reverse the Starting and Ending Bend checkboxes if only one is set.
                if (chkStarting.Checked == true && chkEnding.Checked == false)
                {
                    chkStarting.Checked = false;
                    chkEnding.Checked = true;
                }
                else if (chkStarting.Checked == false && chkEnding.Checked == true)
                {
                    chkStarting.Checked = true;
                    chkEnding.Checked = false;
                }

                m_SkipStartEndBendAction = false;

                // Renumber the indexes in the grid so the first record has the lowest index number
                RenumberGridIndex();

                // Calculate the tension and swbp
                Calculate();
            }
            catch (Exception ex)
            {
                MessageBox.Show(m_Application.ApplicationWindow, ConstantsDT.ERROR_CPT_REVERSE_CALCULATIONS + ": " + ex.Message, ConstantsDT.COMMAND_NAME_CABLE_PULL_TENSION, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        private void frmCablePullTension_Activated(object sender, EventArgs e)
        {
            dgvSections.ClearSelection();
            if (m_oSegmentGES != null)
            {
                m_oSegmentGES.RemoveAllGeometries();
            }            
        }

        // Initialize the form. Populate the comboboxes. Enable/Disable the controls based on the mode.
        private bool InitializeForm ()
        {
            bool bReturnValue = false;            

            try
            {
                // Populate the Cable combo box
                if (!PopulateCableList())
                {
                    return false;
                }

                // Populate the Duct combo box
                if (!PopulateDuctSizeList())
                {
                    return false;
                }

                // Get the maximum pulling distance for each cable and duct combination in the M_S_VALIDATION_TABLE table.
                if (!GetValidationRecords())
                {
                    return false;
                }

                // Get the command metadata parameters
                if (!GetMetadata())
                {
                    return false;
                }
                
                ((DataGridViewComboBoxColumn)dgvSections.Columns[clsCPTCalculate.M_GR_SECTIONTYPE]).Items.Add(clsCPTCalculate.M_SECTIONTYPE_STRAIGHT);
                ((DataGridViewComboBoxColumn)dgvSections.Columns[clsCPTCalculate.M_GR_SECTIONTYPE]).Items.Add(clsCPTCalculate.M_SECTIONTYPE_VERTICAL);
                ((DataGridViewComboBoxColumn)dgvSections.Columns[clsCPTCalculate.M_GR_SECTIONTYPE]).Items.Add(clsCPTCalculate.M_SECTIONTYPE_HORIZONTAL);
                ((DataGridViewComboBoxColumn)dgvSections.Columns[clsCPTCalculate.M_GR_SECTIONTYPE]).Items.Add(clsCPTCalculate.M_SECTIONTYPE_RISER);
                ((DataGridViewComboBoxColumn)dgvSections.Columns[clsCPTCalculate.M_GR_SECTIONTYPE]).Items.Add(clsCPTCalculate.M_SECTIONTYPE_PULLEY);
                ((DataGridViewComboBoxColumn)dgvSections.Columns[clsCPTCalculate.M_GR_SECTIONTYPE]).Items.Add(clsCPTCalculate.M_SECTIONTYPE_DIP);

                cmdCalculate.Enabled = false;
                cmdPrintReport.Enabled = false;
                cmdReversePullDirection.Enabled = false;
                cmdSaveReport.Enabled = false;
                cmdApply.Enabled = false;
                lblCheckClearance.Visible = false;
                lblCheckJamRatio.Visible = false;

                dgvSections.RowHeadersWidth = 10;

                // Get the default backcolor for the results and grid cells.
                // These values will be used to reset the results and grid backcolors for the next calculation 
                // since the program will set the backcolors to red when the thresholds are exceeded.
                m_defaultTextBoxColor = txtForwardResultsTension.BackColor;
                m_defaultReadOnlyCellColor = dgvSections.Rows[0].Cells[clsCPTCalculate.M_GR_FWD_TENSION].Style.BackColor;

                // Get the feature in the select set to determine which mode to run the command.
                //  Duct Bank in select set means "design mode" and grid will be populated 
                //  with Duct Bank geometry. Rows can't be added to grid.
                //  No feature in select set means "pre-design mode". User will be able to add rows to grid.
                if (m_Application.SelectedObjects.FeatureCount == 0)
                {
                    m_bDesignMode = false;
                    this.Text = this.Text + " (Pre-Design Mode)";
                    // The index factor is used to determine the last row in the grid.
                    // Grid is zero based index. Last row index is rowcount - 1.
                    // In non-design mode where rows can be added there will always be an 
                    // extra row, so last row index is rowcount - 2.
                    indexFactor = 2;
                }
                else
                {
                    m_bDesignMode = true;
                    this.Text = this.Text + " (Design Mode)";
                    indexFactor = 1;
                }

                if (m_bDesignMode)
                {
                    dgvSections.AllowUserToAddRows = false;
                    dgvSections.Columns[clsCPTCalculate.M_GR_SECTIONTYPE].ReadOnly = true;

                    // Initialize m_oSegmentGES. m_oSegmentGES will be used to highlight
                    // the line segments in the map window when a grid row is selected.
                    m_oSegmentGES = GTClassFactory.Create<IGTGeometryEditService>();
                    m_oSegmentGES.TargetMapWindow = m_Application.ActiveMapWindow;

                    // Process the feature in the select set.
                    // Get the FID and break the geometry into segments and bends that will be
                    // added as rows in the grid.
                    clsCPTProcessSelectSet = new clsCPTProcessSelectSet();
                    if (!clsCPTProcessSelectSet.ProcessSelectSet())
                    {
                        bReturnValue = false;
                    }

                    if (!clsCPTProcessSelectSet.AllowCUEdit)
                    {
                        cmdApply.Enabled = false;
                        cmdCableCU.Enabled = false;
                    }
                    m_SelectedFID = clsCPTProcessSelectSet.SelectedFID;
                    m_SelectedFNO = clsCPTProcessSelectSet.SelectedFNO;
                    m_CableCU = clsCPTProcessSelectSet.CU;

                    if (m_CableCU.Length > 0)
                    {
                        m_oCableRS.Filter = ConstantsDT.FIELD_CPT_CABLE_CU + " = '" + m_CableCU + "'";
                        if (m_oCableRS.RecordCount > 0)
                        {
                            m_oCableRS.MoveFirst();
                            cboCable.Text = m_oCableRS.Fields[ConstantsDT.FIELD_CPT_CABLE_DESCRIPTION].Value.ToString();
                            cmdCableCU.Text = m_CableCU;
                        }
                        else
                        {
                            MessageBox.Show(m_Application.ApplicationWindow, ConstantsDT.ERROR_CPT_SELECTED_CU, ConstantsDT.COMMAND_NAME_CABLE_PULL_TENSION, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        }
                    }

                    // Populate the grid with the segments and bends that comprise the pull
                    if (!PopulateGrid())
                    {
                        bReturnValue = false;
                    }

                    chkStarting.Checked = true;
                    chkEnding.Checked = true;
                    chkNotify.Checked = true;
                    cboCable.Enabled = false;

                    bReturnValue = true;
                }
                else
                {
                    m_SelectedFID = 0;
                    m_SelectedFNO = 0;
                    dgvSections.AllowUserToAddRows = true;
                    dgvSections.Columns[0].ReadOnly = false;
                    chkNotify.Enabled = false;
                    cmdCableCU.Enabled = false;

                    bReturnValue = true;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(m_Application.ApplicationWindow, ConstantsDT.ERROR_CPT_FORM_INITIALIZATION + ": " + ex.Message, ConstantsDT.COMMAND_NAME_CABLE_PULL_TENSION, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                bReturnValue = false;
            }            

            return bReturnValue;
        }

        // Populate the datagrid using the segments and bends that were determined from the selected geometry
        private bool PopulateGrid ()
        {
            bool bReturnValue = false;

            try
            {
                int iArraySize = (clsCPTProcessSelectSet.Segments.Length - 1);
                int i = 1;
                int index = 0;

                while (i <= iArraySize)
                {
                    // Add the line record
                    DataGridViewRow r = new DataGridViewRow();

                    index = dgvSections.RowCount;
                    dgvSections.Rows.Insert(index, r);

                    dgvSections.Rows[index].Cells[clsCPTCalculate.M_GR_SECTIONTYPE].Value = clsCPTCalculate.M_SECTIONTYPE_STRAIGHT;                    
                    dgvSections.Rows[index].Cells[clsCPTCalculate.M_GR_BENDANGLE].Value = 0;
                    dgvSections.Rows[index].Cells[clsCPTCalculate.M_GR_BENDRADIUS].Value = 0;
                    dgvSections.Rows[index].Cells[clsCPTCalculate.M_GR_DEPTH].Value = 0;
                    dgvSections.Rows[index].Cells[clsCPTCalculate.M_GR_SEGMENT_INDEX].Value = i;
                    dgvSections.Rows[index].Cells[clsCPTCalculate.M_GR_LENGTH].Value = clsCPTProcessSelectSet.Segments[i].iLength;

                    if (i == 1)
                    {
                        dgvSections.Rows[index].Cells[clsCPTCalculate.M_GR_STRUCTURE_FROM].Value = clsCPTProcessSelectSet.node1Structure;
                        dgvSections.Rows[index].Cells[clsCPTCalculate.M_GR_STRUCTURE_TO].Value = clsCPTProcessSelectSet.node2Structure;
                    }

                    //if (i == iArraySize - 1)
                    //{
                        //dgvSections.Rows[index].Cells[clsCPTCalculate.M_GR_STRUCTURE_TO].Value = clsCPTProcessSelectSet.node2Structure;
                    //}
                    // Add the bend record
                    if (i < iArraySize)
                    {
                        r = new DataGridViewRow();
                        index = dgvSections.RowCount;
                        dgvSections.Rows.Insert(index, r);

                        dgvSections.Rows[index].Cells[clsCPTCalculate.M_GR_SECTIONTYPE].Value = clsCPTCalculate.M_SECTIONTYPE_HORIZONTAL;
                        dgvSections.Rows[index].Cells[clsCPTCalculate.M_GR_LENGTH].Value = 0;
                        dgvSections.Rows[index].Cells[clsCPTCalculate.M_GR_BENDANGLE].Value = clsCPTProcessSelectSet.Bends[i];
                        dgvSections.Rows[index].Cells[clsCPTCalculate.M_GR_BENDRADIUS].Value = "";
                        dgvSections.Rows[index].Cells[clsCPTCalculate.M_GR_DEPTH].Value = 0;
                    }

                    i++;
                }

                bReturnValue = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(m_Application.ApplicationWindow, ConstantsDT.ERROR_CPT_GRID_POPULATION + ": " + ex.Message, ConstantsDT.COMMAND_NAME_CABLE_PULL_TENSION, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                bReturnValue = false;                
            }

            return bReturnValue;
        }

        // Populate the Cable combobox
        private bool PopulateCableList ()
        {
            bool bReturnValue = false;

            try
            {
                m_oCableRS = m_Application.DataContext.OpenRecordset(ConstantsDT.QUERY_CPT_CABLE, CursorTypeEnum.adOpenDynamic, LockTypeEnum.adLockOptimistic, (int)CommandTypeEnum.adCmdText);

                if (m_oCableRS.RecordCount > 0)
                {
                    while (!m_oCableRS.EOF)
                    {
                        cboCable.Items.Add(m_oCableRS.Fields[ConstantsDT.FIELD_CPT_CABLE_DESCRIPTION].Value);
                        m_oCableRS.MoveNext();
                    }
                    bReturnValue = true;
                }
                else
                {
                    MessageBox.Show(m_Application.ApplicationWindow, ConstantsDT.ERROR_CPT_NO_CABLE_RECORDS, ConstantsDT.COMMAND_NAME_CABLE_PULL_TENSION, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    bReturnValue = false;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(m_Application.ApplicationWindow, ConstantsDT.ERROR_CPT_CABLE_LIST + ": " + ex.Message, ConstantsDT.COMMAND_NAME_CABLE_PULL_TENSION, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                bReturnValue = false;
            }

            return bReturnValue;
        }

        // Populate the Duct Size combobox
        private bool PopulateDuctSizeList()
        {
            bool bReturnValue = false;

            try
            {
                m_oDuctSizeRS = m_Application.DataContext.OpenRecordset(ConstantsDT.QUERY_DUCT, CursorTypeEnum.adOpenDynamic, LockTypeEnum.adLockOptimistic, (int)CommandTypeEnum.adCmdText);

                if (m_oDuctSizeRS.RecordCount > 0)
                {
                    while (!m_oDuctSizeRS.EOF)
                    {
                        cboDuctSize.Items.Add(m_oDuctSizeRS.Fields[ConstantsDT.FIELD_DUCT_DESCRIPTION].Value);
                        m_oDuctSizeRS.MoveNext();
                    }
                    bReturnValue = true;
                }
                else
                {
                    MessageBox.Show(m_Application.ApplicationWindow, ConstantsDT.ERROR_CPT_NO_DUCT_RECORDS, ConstantsDT.COMMAND_NAME_CABLE_PULL_TENSION, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    bReturnValue = false;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(m_Application.ApplicationWindow, ConstantsDT.ERROR_CPT_DUCT_LIST + ": " + ex.Message, ConstantsDT.COMMAND_NAME_CABLE_PULL_TENSION, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                bReturnValue = false;
            }

            return bReturnValue;
        }

        // Query the validation table for the max distance for each cable and duct combination.
        private bool GetValidationRecords()
        {
            bool bReturnValue = false;

            try
            {
                m_oValidationRS = m_Application.DataContext.OpenRecordset(ConstantsDT.QUERY_CPT_MAX_DISTANCE, CursorTypeEnum.adOpenDynamic, LockTypeEnum.adLockOptimistic, (int)CommandTypeEnum.adCmdText);
                bReturnValue = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(m_Application.ApplicationWindow, ConstantsDT.ERROR_CPT_VALIDATION_LIST + ": " + ex.Message, ConstantsDT.COMMAND_NAME_CABLE_PULL_TENSION, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                bReturnValue = false;
            }

            return bReturnValue;
        }

        // Query for the command metadata
        private bool GetMetadata()
        {
            bool returnValue = false;

            try
            {
                Recordset metadataRS = null;
                CommonDT.GetCommandMetadata("CablePullTensionCC", ref metadataRS);

                if (metadataRS.RecordCount > 0)
                {
                    string paramName = string.Empty;
                    string paramValue = string.Empty;
                    metadataRS.MoveFirst();

                    while (!metadataRS.EOF)
                    {
                        paramName = metadataRS.Fields["PARAM_NAME"].Value.ToString();
                        paramValue = metadataRS.Fields["PARAM_VALUE"].Value.ToString();

                        if (paramName == "ReportFileName")
                        {
                            m_ReportFileName = paramValue;
                        }
                        else if (paramName == "ReportName")
                        {
                            m_ReportName = paramValue;
                        }
                        else if (paramName == "JamRatioLowerBound")
                        {
                            m_JamRatioLowerBound = Convert.ToDouble(paramValue);
                        }
                        else if (paramName == "JamRatioUpperBound")
                        {
                            m_JamRatioUpperBound = Convert.ToDouble(paramValue);
                        }
                        else if (paramName == "LowCOF")
                        {
                            m_CPTCalculate.LowCOF = Convert.ToDouble(paramValue);
                        }
                        else if (paramName == "HighCOF")
                        {
                            m_CPTCalculate.HighCOF = Convert.ToDouble(paramValue);
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

        // Set the cell default values when a new section type is added.
        private void dgvSections_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (!m_bDesignMode && dgvSections.Rows.Count > 1)
                {
                    // Skip setting defaults when m_SkipSectionDefaults is true.
                    // This flag is needed since the section type can be set programmatically and 
                    // we don't want to override the values. For example add Depth to Straight section
                    // changes the section type to Dip.
                    if (e.ColumnIndex == clsCPTCalculate.M_GR_SECTIONTYPE && !m_SkipSectionDefaults)
                    {
                        string newValue = dgvSections.Rows[e.RowIndex].Cells[clsCPTCalculate.M_GR_SECTIONTYPE].Value.ToString();
                        double length = 0;
                        double bendAngle = 0;
                        double bendRadius = 0;
                        double depth = 0;

                        switch (newValue)
                        {
                            case clsCPTCalculate.M_SECTIONTYPE_STRAIGHT:
                                length = 100;
                                bendAngle = 0;
                                bendRadius = 0;
                                depth = 0;
                                break;
                            case clsCPTCalculate.M_SECTIONTYPE_VERTICAL:
                            case clsCPTCalculate.M_SECTIONTYPE_HORIZONTAL:
                            case clsCPTCalculate.M_SECTIONTYPE_PULLEY:
                                length = 0;
                                bendAngle = 90;
                                if (txtStdBendRadius.Text.Length > 0)
                                {
                                    bendRadius = Convert.ToDouble(txtStdBendRadius.Text.ToString());
                                }
                                else
                                {
                                    bendRadius = 0;
                                }
                                depth = 0;
                                break;
                            case clsCPTCalculate.M_SECTIONTYPE_RISER:
                                length = 30;
                                bendAngle = 90;
                                if (txtStdBendRadius.Text.Length > 0)
                                {
                                    bendRadius = Convert.ToDouble(txtStdBendRadius.Text.ToString());
                                }
                                else
                                {
                                    bendRadius = 0;
                                }
                                depth = 0;
                                break;
                            case clsCPTCalculate.M_SECTIONTYPE_DIP:
                                length = 100;
                                bendAngle = 0;
                                bendRadius = 0;
                                depth = 10;
                                break;
                            default:
                                return;
                        }

                        dgvSections.Rows[e.RowIndex].Cells[clsCPTCalculate.M_GR_LENGTH].Value = length;
                        dgvSections.Rows[e.RowIndex].Cells[clsCPTCalculate.M_GR_BENDANGLE].Value = bendAngle;
                        dgvSections.Rows[e.RowIndex].Cells[clsCPTCalculate.M_GR_BENDRADIUS].Value = bendRadius;
                        dgvSections.Rows[e.RowIndex].Cells[clsCPTCalculate.M_GR_DEPTH].Value = depth;

                        // Update Starting Bend checkbox if first record is a vertical bend.
                        if (e.RowIndex == 0)
                        {
                            m_SkipStartEndBendAction = true;

                            if (dgvSections.Rows[e.RowIndex].Cells[clsCPTCalculate.M_GR_SECTIONTYPE].Value.ToString() == clsCPTCalculate.M_SECTIONTYPE_VERTICAL)
                            {
                                chkStarting.Checked = true;
                            }
                            else
                            {
                                chkStarting.Checked = false;
                            }

                            m_SkipStartEndBendAction = false;
                        }
                        // Update Ending Bend checkbox if last record is a vertical bend.
                        else if (e.RowIndex == dgvSections.Rows.Count - indexFactor)
                        {
                            m_SkipStartEndBendAction = true;

                            if (dgvSections.Rows[e.RowIndex].Cells[clsCPTCalculate.M_GR_SECTIONTYPE].Value.ToString() == clsCPTCalculate.M_SECTIONTYPE_VERTICAL)
                            {
                                chkEnding.Checked = true;
                            }
                            else
                            {
                                chkEnding.Checked = false;
                            }

                            m_SkipStartEndBendAction = false;
                        }
                    }
                }                
            }
            catch (Exception ex)
            {
                MessageBox.Show(m_Application.ApplicationWindow, ConstantsDT.ERROR_CPT_SET_DEFAULT_CELL_VALUES + ": " + ex.Message, ConstantsDT.COMMAND_NAME_CABLE_PULL_TENSION, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        private void dgvSections_SelectionChanged(object sender, EventArgs e)
        {
            try
            {
                // Allow a pulley to be added if a row is selected.
                if (dgvSections.SelectedRows.Count > 0)
                {
                    cmdAddPulley.Enabled = true;
                }
                else
                {
                    cmdAddPulley.Enabled = false;
                }

                // Highlight segment in map window in design mode.
                if (m_bDesignMode)
                {
                    m_oSegmentGES.RemoveAllGeometries();

                    try
                    {
                        if (dgvSections.Rows[dgvSections.CurrentRow.Index].Cells[clsCPTCalculate.M_GR_SECTIONTYPE].Value.ToString() == clsCPTCalculate.M_SECTIONTYPE_STRAIGHT ||
                            dgvSections.Rows[dgvSections.CurrentRow.Index].Cells[clsCPTCalculate.M_GR_SECTIONTYPE].Value.ToString() == clsCPTCalculate.M_SECTIONTYPE_DIP)
                        {
                            m_oSegmentGES.AddGeometry(clsCPTProcessSelectSet.Segments[Convert.ToInt16(dgvSections.Rows[dgvSections.CurrentRow.Index].Cells[clsCPTCalculate.M_GR_SEGMENT_INDEX].Value)].oSeg, Convert.ToInt16(GTStyleIDConstants.gtstyleLineSelectSolid2));
                        }
                        // Skip if first or last non Straight or Dip section record since there is nothing to highlight
                        else if (dgvSections.CurrentRow.Index != 0 && dgvSections.CurrentRow.Index != dgvSections.RowCount - 1)
                        {                            
                            // This is either a bend or pulley, so highlight the previous and next straight or dip sections.
                            // The straight or dip sections will not always be -1 and +1 away from the current index as in the case
                            // of a pulley next to a horizontal bend.
                            int previousSegmentIndex = -1;
                            int nextSegmentIndex = -1;

                            for (int i = dgvSections.CurrentRow.Index - 1; i >= 0; i--)
                            {
                                if (dgvSections.Rows[i].Cells[clsCPTCalculate.M_GR_SEGMENT_INDEX].Value != null)
                                {
                                    previousSegmentIndex = i;
                                    break;
                                }
                            }

                            for (int i = dgvSections.CurrentRow.Index + 1; i <= dgvSections.RowCount; i++)
                            {
                                if (dgvSections.Rows[i].Cells[clsCPTCalculate.M_GR_SEGMENT_INDEX].Value != null)
                                {
                                    nextSegmentIndex = i;
                                    break;
                                }
                            }

                            if (previousSegmentIndex != -1 && nextSegmentIndex != -1)
                            {
                                m_oSegmentGES.AddGeometry(clsCPTProcessSelectSet.Segments[Convert.ToInt16(dgvSections.Rows[previousSegmentIndex].Cells[clsCPTCalculate.M_GR_SEGMENT_INDEX].Value)].oSeg, Convert.ToInt16(GTStyleIDConstants.gtstyleLineSelectSolid2));
                                m_oSegmentGES.AddGeometry(clsCPTProcessSelectSet.Segments[Convert.ToInt16(dgvSections.Rows[nextSegmentIndex].Cells[clsCPTCalculate.M_GR_SEGMENT_INDEX].Value)].oSeg, Convert.ToInt16(GTStyleIDConstants.gtstyleLineSelectSolid2));
                            }                            
                        }
                    }
                    catch
                    {
                       // Don't raise error for highlight issue. 
                    }                    
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(m_Application.ApplicationWindow, ConstantsDT.ERROR_CPT_GRID_SELECTION_CHANGE + ": " + ex.Message, ConstantsDT.COMMAND_NAME_CABLE_PULL_TENSION, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        private void cmdCalculate_Click(object sender, EventArgs e)
        {
            m_Application.BeginWaitCursor();

            // Run the calculations
            m_Application.SetStatusBarText(GTStatusPanelConstants.gtaspcMessage, ConstantsDT.MESSAGE_CALCULATING);

            if (!Calculate())
            {
                m_Application.SetStatusBarText(GTStatusPanelConstants.gtaspcMessage, "");
                m_Application.EndWaitCursor();
                return;
            }

            m_Application.SetStatusBarText(GTStatusPanelConstants.gtaspcMessage, "");
            m_Application.EndWaitCursor();
        }

        // Call the clsCPTCalculate.Calculate method to do the calculations. Display the results
        private bool Calculate ()
        {
            bool bReturnValue = false;

            try
            {
                // Reset the results for a new calculation.
                ResetGridForCalculation();

                if (m_CPTCalculate.Calculate(ref dgvSections))
                {
                    // Update the results from the calculations
                    txtForwardResultsTension.Text = Math.Round(m_CPTCalculate.TotalTension, 0).ToString();
                    int maxForwardTensionPercent = Convert.ToInt32(Math.Round(Convert.ToDouble(txtForwardResultsTension.Text) / Convert.ToDouble(txtMaxTension.Text) * 100));
                    txtForwardTensionMaxPcnt.Text = maxForwardTensionPercent.ToString();

                    if (maxForwardTensionPercent > 100)
                    {
                        txtForwardResultsTension.BackColor = m_ErrorColor;
                    }

                    txtReverseResultsTension.Text = Math.Round(m_CPTCalculate.TotalReverseTension, 0).ToString();
                    int maxReverseTensionPercent = Convert.ToInt32(Math.Round(Convert.ToDouble(txtReverseResultsTension.Text) / Convert.ToDouble(txtMaxTension.Text) * 100));
                    txtReverseTensionMaxPcnt.Text = maxReverseTensionPercent.ToString();

                    if (maxReverseTensionPercent > 100)
                    {
                        txtReverseResultsTension.BackColor = m_ErrorColor;
                    }

                    int totalLength = Convert.ToInt32(Math.Round(m_CPTCalculate.TotalLength, 0).ToString());
                    txtResultsTotalLength.Text = totalLength.ToString();

                    if (totalLength > m_CPTCalculate.MaxLength && m_CPTCalculate.MaxLength != 0)
                    {
                        txtResultsTotalLength.BackColor = m_ErrorColor;
                    }                    

                    // Check if Jam Ratio is exceeded. If so, display warning label.
                    double result = 0;
                    if (double.TryParse(m_CPTCalculate.JamRatio, out result))
                    {
                        if (txtNonStd.Text != "S" &&
                        ((Convert.ToDouble(m_CPTCalculate.JamRatio) < (m_JamRatioLowerBound / 1.05) || (Convert.ToDouble(m_CPTCalculate.JamRatio) * 1.03) > m_JamRatioUpperBound)))
                        {
                            lblCheckJamRatio.Text = "CHECK JAM RATIO: " + m_CPTCalculate.JamRatio;
                            lblCheckJamRatio.Visible = true;
                        }
                    }

                    // Check if Clearance is exceeded. If so, display warning label.
                    if (m_CPTCalculate.CableClearance <= m_CPTCalculate.MinimumClearance)
                    {
                        lblCheckClearance.Text = "CHECK CLEARANCE: " + m_CPTCalculate.CableClearance;
                        lblCheckClearance.Visible = true;
                    }

                    cmdReversePullDirection.Enabled = true;
                    cmdPrintReport.Enabled = true;
                }        
                else
                {
                    return false;
                }

                // Clear selected rows so row select color does not hide cells that have been highlighted in red.
                dgvSections.ClearSelection();

                if (m_bDesignMode)
                {
                    m_oSegmentGES.RemoveAllGeometries();
                }
                
                dgvSections.Refresh();

                if (m_bDesignMode && m_Application.DataContext.ActiveJob.Length > 0)
                {
                    // Add record to validation table showing the results of the calculation
                    WriteValidationResults();
                }

                bReturnValue = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(m_Application.ApplicationWindow, ConstantsDT.ERROR_CPT_DISPLAY_CALCULATIONS + ": " + ex.Message, ConstantsDT.COMMAND_NAME_CABLE_PULL_TENSION, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                bReturnValue = false;
            }

            return bReturnValue;
        }

        // Reset the results for a new calculation.
        private bool ResetGridForCalculation ()
        {
            bool bReturnValue = false;

            try
            {
                m_CPTCalculate.TotalLength = 0;
                m_CPTCalculate.TotalReverseTension = 0;
                m_CPTCalculate.TotalTension = 0;
                m_CPTCalculate.TotalSWBP = 0;
                //m_CPTCalculate.MaxLength = 0;

                txtForwardResultsTension.BackColor = m_defaultTextBoxColor;
                txtReverseResultsTension.BackColor = m_defaultTextBoxColor;
                txtResultsTotalLength.BackColor = m_defaultTextBoxColor;

                txtForwardResultsTension.Text = "";
                txtReverseResultsTension.Text = "";
                txtResultsTotalLength.Text = "";
                txtForwardTensionMaxPcnt.Text = "";
                txtReverseTensionMaxPcnt.Text = "";


                lblCheckJamRatio.Visible = false;
                lblCheckClearance.Visible = false;
                lblCheckClearance.Text = "";

                // Set the cell color back to its initial state and clear results
                foreach (DataGridViewRow row in dgvSections.Rows)
                {
                    row.Cells[clsCPTCalculate.M_GR_FWD_TENSION].Style.BackColor = m_defaultReadOnlyCellColor;
                    row.Cells[clsCPTCalculate.M_GR_FWD_SWBP].Style.BackColor = m_defaultReadOnlyCellColor;
                    row.Cells[clsCPTCalculate.M_GR_REV_TENSION].Style.BackColor = m_defaultReadOnlyCellColor;
                    row.Cells[clsCPTCalculate.M_GR_REV_SWBP].Style.BackColor = m_defaultReadOnlyCellColor;

                    row.Cells[clsCPTCalculate.M_GR_FWD_TENSION].Value = "";
                    row.Cells[clsCPTCalculate.M_GR_FWD_SWBP].Value = "";
                    row.Cells[clsCPTCalculate.M_GR_REV_TENSION].Value = "";
                    row.Cells[clsCPTCalculate.M_GR_REV_SWBP].Value = "";
                }

                cmdPrintReport.Enabled = false;
                cmdSaveReport.Enabled = false;
                m_PassedValidation = false;

                bReturnValue = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(m_Application.ApplicationWindow, ConstantsDT.ERROR_CPT_RESET_FORM + ": " + ex.Message, ConstantsDT.COMMAND_NAME_CABLE_PULL_TENSION, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                bReturnValue = false;
            }

            return bReturnValue;
        }

        private void cboCable_SelectedValueChanged(object sender, EventArgs e)
        {
            try
            {
                if (cboCable.SelectedItem != null)
                {
                    string rsFilter = ConstantsDT.FIELD_CPT_CABLE_DESCRIPTION + " = '" + cboCable.SelectedItem.ToString() + "'";

                    m_oCableRS.Filter = rsFilter;

                    if (m_oCableRS.RecordCount > 0)
                    {
                        cboCable.Tag = Convert.ToInt32(m_oCableRS.Fields[ConstantsDT.FIELD_CPT_CABLE_CINO].Value);

                        // Populate cable characteristics text boxes
                        txtNonStd.Text = m_oCableRS.Fields[ConstantsDT.FIELD_CPT_CABLE_NON_STD].Value.ToString();
                        txtCableWeight.Text = m_oCableRS.Fields[ConstantsDT.FIELD_CPT_CABLE_WEIGHT_PER_FOOT].Value.ToString();
                        txtMaxTension.Text = m_oCableRS.Fields[ConstantsDT.FIELD_CPT_CABLE_MAX_TENSION].Value.ToString();
                        txtMaxSWBP.Text = m_oCableRS.Fields[ConstantsDT.FIELD_CPT_CABLE_MAX_SWBP].Value.ToString();
                        txtCableOD.Text = m_oCableRS.Fields[ConstantsDT.FIELD_CPT_CABLE_OUTSIDE_DIAMETER].Value.ToString();
                        txtCableConfig.Text = m_oCableRS.Fields[ConstantsDT.FIELD_CABLECONFIG_CONFIGURATION].Value.ToString();

                        // Set properties used for calculations
                        m_CPTCalculate.CableConfiguration = m_oCableRS.Fields[ConstantsDT.FIELD_CABLECONFIG_CONFIGURATION].Value.ToString();
                        m_CPTCalculate.CableDiameter = Convert.ToDouble(m_oCableRS.Fields[ConstantsDT.FIELD_CPT_CABLE_OUTSIDE_DIAMETER].Value);
                        m_CPTCalculate.Phases = m_oCableRS.Fields[ConstantsDT.FIELD_CPT_CABLE_NUMBER_OF_PHASES].Value.ToString();
                        m_CPTCalculate.WeightPerFoot = Convert.ToDouble(m_oCableRS.Fields[ConstantsDT.FIELD_CPT_CABLE_WEIGHT_PER_FOOT].Value);
                        m_CPTCalculate.MaxTension = Convert.ToInt32(m_oCableRS.Fields[ConstantsDT.FIELD_CPT_CABLE_MAX_TENSION].Value);
                        m_CPTCalculate.MaxSWBP = Convert.ToInt32(m_oCableRS.Fields[ConstantsDT.FIELD_CPT_CABLE_MAX_SWBP].Value);
                        m_CPTCalculate.SwbpChangePoint = Convert.ToInt32(m_oCableRS.Fields[ConstantsDT.FIELD_CPT_CABLE_SWBP_CHANGE_POINT].Value);
                        if (!Convert.IsDBNull(m_oCableRS.Fields[ConstantsDT.FIELD_CPT_CABLE_MAX_LENGTH].Value))
                        {
                            m_CPTCalculate.MaxLength = Convert.ToInt32(m_oCableRS.Fields[ConstantsDT.FIELD_CPT_CABLE_MAX_LENGTH].Value);
                        }

                        // Set properties for reports
                        m_SelectedVoltage = m_oCableRS.Fields[ConstantsDT.FIELD_CPT_CABLE_VOLTAGE].Value.ToString();
                        m_SelectedCableType = m_oCableRS.Fields[ConstantsDT.FIELD_CPT_CABLE_TYPE].Value.ToString();
                        m_SelectedConductor = m_oCableRS.Fields[ConstantsDT.FIELD_CPT_CABLE_SIZE_MATERIAL].Value.ToString();

                        GetMaxPullingDistance();
                    }
                }
                else
                {
                    // Clear cable characteristics text boxes
                    txtNonStd.Text = "";
                    txtCableWeight.Text = "";
                    txtMaxTension.Text = "";
                    txtMaxSWBP.Text = "";
                    txtCableOD.Text = "";
                    txtCableConfig.Text = "";

                    // Clear report variables
                    m_SelectedVoltage = "";
                }

                ResetGridForCalculation();

            }
            catch (Exception ex)
            {
                MessageBox.Show(m_Application.ApplicationWindow, ConstantsDT.ERROR_CPT_CABLE_CHANGE + ": " + ex.Message, ConstantsDT.COMMAND_NAME_CABLE_PULL_TENSION, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        private void cboDuctSize_SelectedValueChanged(object sender, EventArgs e)
        {
            try
            {
                if (cboDuctSize.SelectedItem != null)
                {
                    string rsFilter = ConstantsDT.FIELD_DUCT_DESCRIPTION + " = '" + cboDuctSize.SelectedItem.ToString() + "'";

                    m_oDuctSizeRS.Filter = rsFilter;

                    if (m_oDuctSizeRS.RecordCount > 0)
                    {
                        cboDuctSize.Tag = Convert.ToInt32(m_oDuctSizeRS.Fields[ConstantsDT.FIELD_DUCT_DTNO].Value);

                        // Populate duct characteristics text boxes
                        txtStdBendRadius.Text = m_oDuctSizeRS.Fields[ConstantsDT.FIELD_DUCT_STD_BEND_RADIUS].Value.ToString();

                        // Set properties used for calculations
                        m_CPTCalculate.DuctDiameter = Convert.ToDouble(m_oDuctSizeRS.Fields[ConstantsDT.FIELD_DUCT_INSIDE_DIAMETER].Value);
                        m_CPTCalculate.DuctDiameterNominal = Convert.ToDouble(m_oDuctSizeRS.Fields[ConstantsDT.FIELD_DUCT_NOM_INSIDE_DIAMETER].Value);
                        m_CPTCalculate.StandardBendRadius = Convert.ToDouble(m_oDuctSizeRS.Fields[ConstantsDT.FIELD_DUCT_STD_BEND_RADIUS].Value);
                        m_CPTCalculate.MinimumClearance = Convert.ToDouble(m_oDuctSizeRS.Fields[ConstantsDT.FIELD_DUCT_MIN_CLEARANCE].Value);
                    }

                    // Default the radius to the standard bend radius if the radius is null                    
                    foreach (DataGridViewRow r in dgvSections.Rows)
                    {
                        if (r.Cells[clsCPTCalculate.M_GR_SECTIONTYPE].Value != null)
                        {
                            if (r.Cells[clsCPTCalculate.M_GR_SECTIONTYPE].Value.ToString() != clsCPTCalculate.M_SECTIONTYPE_STRAIGHT
                                                        && r.Cells[clsCPTCalculate.M_GR_SECTIONTYPE].Value.ToString() != clsCPTCalculate.M_SECTIONTYPE_DIP
                                                        && (r.Cells[clsCPTCalculate.M_GR_BENDRADIUS].Value.ToString() == ""
                                                        || r.Cells[clsCPTCalculate.M_GR_BENDRADIUS].Value.ToString() == "0"))
                            {
                                r.Cells[clsCPTCalculate.M_GR_BENDRADIUS].Value = m_CPTCalculate.StandardBendRadius;
                            }
                        }                        
                    }

                    GetMaxPullingDistance();
                }
                else
                {
                    // Clear duct characteristics text boxes
                    txtStdBendRadius.Text = "";
                }

                ResetGridForCalculation();
            }
            catch (Exception ex)
            {
                MessageBox.Show(m_Application.ApplicationWindow, ConstantsDT.ERROR_CPT_DUCT_SIZE_CHANGE + ": " + ex.Message, ConstantsDT.COMMAND_NAME_CABLE_PULL_TENSION, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        private void dgvSections_RowsAdded(object sender, DataGridViewRowsAddedEventArgs e)
        {
            try
            {
                dgvSections.Rows[e.RowIndex].Cells[clsCPTCalculate.M_GR_ROW_INDEX].Value = m_RowIndex;
                m_RowIndex++;

                // If first record is being added and type is Vertical Bend then check starting bend.
                // If last record is being added and type is Vertical Bend then check ending bend.
                if (dgvSections.SelectedRows.Count == 0)
                {
                    // Starting or Ending Vertical Bend check box is being checked or Pulley is being added. Skip processing.
                    return;
                }

                if (dgvSections.SelectedRows[0].Index == 0)
                {
                    if (dgvSections.Rows[0].Cells[clsCPTCalculate.M_GR_SECTIONTYPE].EditedFormattedValue.ToString() == clsCPTCalculate.M_SECTIONTYPE_VERTICAL)
                    {
                        m_SkipStartEndBendAction = true;
                        chkStarting.Checked = true;
                        m_SkipStartEndBendAction = false;
                    }
                    else
                    {
                        m_SkipStartEndBendAction = true;
                        chkStarting.Checked = false;
                        m_SkipStartEndBendAction = false;
                    }
                }

                else if (dgvSections.SelectedRows[0].Index == dgvSections.Rows.Count - indexFactor)
                {
                    if (dgvSections.Rows[dgvSections.SelectedRows[0].Index].Cells[clsCPTCalculate.M_GR_SECTIONTYPE].EditedFormattedValue.ToString() == clsCPTCalculate.M_SECTIONTYPE_VERTICAL)
                    {
                        m_SkipStartEndBendAction = true;
                        chkEnding.Checked = true;
                        m_SkipStartEndBendAction = false;
                    }
                    else
                    {
                        m_SkipStartEndBendAction = true;
                        chkEnding.Checked = false;
                        m_SkipStartEndBendAction = false;
                    }
                }

                RenumberGridIndex();
                ResetGridForCalculation();
            }
            catch (Exception ex)
            {
                MessageBox.Show(m_Application.ApplicationWindow, ConstantsDT.ERROR_CPT_ADD_ROW + ": " + ex.Message, ConstantsDT.COMMAND_NAME_CABLE_PULL_TENSION, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        private void dgvSections_CurrentCellDirtyStateChanged(object sender, EventArgs e)
        {
            // Commit the edit so the cell change event is raised for the datagridview combobox
            if (dgvSections.IsCurrentCellDirty)
            {
                dgvSections.CommitEdit(DataGridViewDataErrorContexts.Commit);
            }
        }

        // Add or remove first vertical bend record based on check value
        private void chkStarting_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                if (!m_SkipStartEndBendAction)
                {
                    if (chkStarting.Checked == true)
                    {
                        // Add Vertical Bend to first row in grid
                        DataGridViewRow r = new DataGridViewRow();

                        dgvSections.Rows.Insert(0, r);

                        dgvSections.Rows[0].Cells[clsCPTCalculate.M_GR_SECTIONTYPE].Value = clsCPTCalculate.M_SECTIONTYPE_VERTICAL;
                        dgvSections.Rows[0].Cells[clsCPTCalculate.M_GR_LENGTH].Value = 0;
                        dgvSections.Rows[0].Cells[clsCPTCalculate.M_GR_BENDANGLE].Value = 90;
                        dgvSections.Rows[0].Cells[clsCPTCalculate.M_GR_BENDRADIUS].Value = txtStdBendRadius.Text;
                        dgvSections.Rows[0].Cells[clsCPTCalculate.M_GR_DEPTH].Value = 0;

                        RenumberGridIndex();
                    }
                    else
                    {
                        // Remove Vertical Bend from first row in grid
                        dgvSections.Rows.Remove(dgvSections.Rows[0]);
                    }
                }
                ResetGridForCalculation();
            }
            catch (Exception ex)
            {
                MessageBox.Show(m_Application.ApplicationWindow, ConstantsDT.ERROR_CPT_STARTING_BEND + ": " + ex.Message, ConstantsDT.COMMAND_NAME_CABLE_PULL_TENSION, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        // Add or remove last vertical bend record based on check value
        private void chkEnding_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                if (!m_SkipStartEndBendAction)
                {
                    if (chkEnding.Checked == true)
                    {
                        // Add Vertical Bend to last row in grid
                        DataGridViewRow r = new DataGridViewRow();

                        dgvSections.Rows.Insert(dgvSections.Rows.Count - indexFactor + 1, r);

                        dgvSections.Rows[dgvSections.Rows.Count - indexFactor].Cells[clsCPTCalculate.M_GR_SECTIONTYPE].Value = clsCPTCalculate.M_SECTIONTYPE_VERTICAL;
                        dgvSections.Rows[dgvSections.Rows.Count - indexFactor].Cells[clsCPTCalculate.M_GR_LENGTH].Value = 0;
                        dgvSections.Rows[dgvSections.Rows.Count - indexFactor].Cells[clsCPTCalculate.M_GR_BENDANGLE].Value = 90;
                        dgvSections.Rows[dgvSections.Rows.Count - indexFactor].Cells[clsCPTCalculate.M_GR_BENDRADIUS].Value = txtStdBendRadius.Text;
                        dgvSections.Rows[dgvSections.Rows.Count - indexFactor].Cells[clsCPTCalculate.M_GR_DEPTH].Value = 0;

                        RenumberGridIndex();
                    }
                    else
                    {
                        // Remove Vertical Bend from last row in grid
                        dgvSections.Rows.Remove(dgvSections.Rows[dgvSections.Rows.Count - indexFactor]);
                    }
                }
                ResetGridForCalculation();
            }
            catch (Exception ex)
            {
                MessageBox.Show(m_Application.ApplicationWindow, ConstantsDT.ERROR_CPT_ENDING_BEND + ": " + ex.Message, ConstantsDT.COMMAND_NAME_CABLE_PULL_TENSION, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        // Renumber the index values in the grid. The index is used for sorting.
        private bool RenumberGridIndex ()
        {
            bool bReturnValue = false;

            m_RowIndex = 1;

            try
            {
                foreach (DataGridViewRow row in dgvSections.Rows)
                {
                    row.Cells[clsCPTCalculate.M_GR_ROW_INDEX].Value = m_RowIndex;
                    m_RowIndex ++;
                }

                bReturnValue = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(m_Application.ApplicationWindow, ConstantsDT.ERROR_CPT_RENUMBER_INDEX + ": " + ex.Message, ConstantsDT.COMMAND_NAME_CABLE_PULL_TENSION, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }

            return bReturnValue;
        }

        // Add a new row in the datagridview. The section type will be a Pulley.
        private void cmdAddPulley_Click(object sender, EventArgs e)
        {
            try
            {
                int selectedRowIndex = dgvSections.SelectedRows[0].Index;
                DataGridViewRow r = new DataGridViewRow();
                dgvSections.Rows.Insert(selectedRowIndex, r);

                dgvSections.Rows[selectedRowIndex].Cells[clsCPTCalculate.M_GR_SECTIONTYPE].Value = clsCPTCalculate.M_SECTIONTYPE_PULLEY;
                dgvSections.Rows[selectedRowIndex].Cells[clsCPTCalculate.M_GR_LENGTH].Value = 0;
                dgvSections.Rows[selectedRowIndex].Cells[clsCPTCalculate.M_GR_BENDANGLE].Value = 90;
                dgvSections.Rows[selectedRowIndex].Cells[clsCPTCalculate.M_GR_BENDRADIUS].Value = txtStdBendRadius.Text;
                dgvSections.Rows[selectedRowIndex].Cells[clsCPTCalculate.M_GR_DEPTH].Value = 0;

                RenumberGridIndex();
            }
            catch (Exception ex)
            {
                MessageBox.Show(m_Application.ApplicationWindow, ConstantsDT.ERROR_CPT_ADD_PULLEY + ": " + ex.Message, ConstantsDT.COMMAND_NAME_CABLE_PULL_TENSION, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        private void dgvSections_RowsRemoved(object sender, DataGridViewRowsRemovedEventArgs e)
        {
            try
            {
                // Update the Starting and Ending bend checkboxes when rows are removed.
                // If the first record is a Vertical Bend then the Starting Bend checkbox will be checked
                // If the last record is a Vertical Bend then the Ending Bend checkbox will be checked.
                if (dgvSections.RowCount > 1)
                {
                    m_SkipStartEndBendAction = true;

                    if (dgvSections.Rows[0].Cells[clsCPTCalculate.M_GR_SECTIONTYPE].Value.ToString() == clsCPTCalculate.M_SECTIONTYPE_VERTICAL)
                    {
                        chkStarting.Checked = true;
                    }
                    else
                    {
                        chkStarting.Checked = false;
                    }

                    if (dgvSections.RowCount != indexFactor)
                    {
                        // Don't check the Ending Bend checkbox if there is only 1 row. The second row is empty.
                        if (dgvSections.Rows[dgvSections.Rows.Count - indexFactor].Cells[clsCPTCalculate.M_GR_SECTIONTYPE].Value.ToString() == clsCPTCalculate.M_SECTIONTYPE_VERTICAL)
                        {
                            chkEnding.Checked = true;
                        }
                        else
                        {
                            chkEnding.Checked = false;
                        }
                    }
                    else
                    {
                        chkEnding.Checked = false;
                    }

                    m_SkipStartEndBendAction = false;
                }

                else
                {
                    m_SkipStartEndBendAction = true;
                    chkStarting.Checked = false;
                    chkEnding.Checked = false;
                    m_SkipStartEndBendAction = false;
                }
                ResetGridForCalculation();
            }
            catch (Exception ex)
            {
                MessageBox.Show(m_Application.ApplicationWindow, ConstantsDT.ERROR_CPT_REMOVE_ROW + ": " + ex.Message, ConstantsDT.COMMAND_NAME_CABLE_PULL_TENSION, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        // Clean up when dialog is closed.
        private void cmdClose_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void frmCablePullTension_FormClosing(object sender, FormClosingEventArgs e)
        {
            // If the Apply command is enabled then there are pending CUs.
            if (cmdApply.Enabled)
            {
                // If the Notify checkbox is checked, then notify user if there are any pending CUs.
                if (chkNotify.Checked)
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

            if (!(m_oSegmentGES == null))
            {
                if (0 < m_oSegmentGES.GeometryCount)
                {
                    m_oSegmentGES.RemoveAllGeometries();
                }
            }

            m_oCableRS = null;
            m_oDuctSizeRS = null;
            m_oValidationRS = null;
            m_oSegmentGES = null;
            m_CPTCalculate = null;
            m_Application.EndWaitCursor();
            m_CustomCommandHelper.Complete();
        }

        // Restrict values in the grid cells
        private void dgvSections_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
        {
            if (dgvSections.CurrentCell.ReadOnly == false)
            {
                e.Control.KeyPress += new KeyPressEventHandler(CheckValue);
            }
        }

        // Restrict values in the grid cells
        private void CheckValue(object sender, KeyPressEventArgs e)
        {
            // Allow user to only enter numbers and decimal for length and depth
            if (dgvSections.CurrentCell.ColumnIndex == clsCPTCalculate.M_GR_LENGTH ||
                dgvSections.CurrentCell.ColumnIndex == clsCPTCalculate.M_GR_DEPTH)
            {
                if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && e.KeyChar != '.')
                {
                    e.Handled = true;
                }
            }
            // Allow user to only enter numbers, decimal and negative sign for angle and radius
            else
            {
                if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && e.KeyChar != '.' && e.KeyChar != '-')
                {
                    e.Handled = true;
                }
            }

            // Allow user to only enter one decimal
            if (e.KeyChar == '.' && (sender as TextBox).Text.IndexOf('.') > -1)
            {
                e.Handled = true;
            }

            // Allow user to only enter one negative sign
            if (e.KeyChar == '-' && (sender as TextBox).Text.IndexOf('-') > -1)
            {
                e.Handled = true;
            }

            // Allow user to only enter negative sign at beginning of cell
            if (e.KeyChar == '-' && (sender as TextBox).Text.Length > 0)
            {
                e.Handled = true;
            }
        }

        // Update the section type value when certain properties are changed.
        private void dgvSections_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                ResetGridForCalculation();

                // Change the Section Type to Riser if length is added to a Vertical section.
                if (e.ColumnIndex == clsCPTCalculate.M_GR_LENGTH
                    && dgvSections.Rows[e.RowIndex].Cells[clsCPTCalculate.M_GR_SECTIONTYPE].Value.ToString() == clsCPTCalculate.M_SECTIONTYPE_VERTICAL)
                {
                    if (dgvSections.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString().Length > 0)
                    {
                        if (Convert.ToInt32(dgvSections.Rows[e.RowIndex].Cells[e.ColumnIndex].Value) != 0)
                        {
                            m_SkipSectionDefaults = true;
                            dgvSections.Rows[e.RowIndex].Cells[clsCPTCalculate.M_GR_SECTIONTYPE].Value = clsCPTCalculate.M_SECTIONTYPE_RISER;
                            m_SkipSectionDefaults = false;
                        }
                    }
                }

                // Change the Section Type to Dip and the Angle and Radius to 0 if depth is added to a Straight section.
                if (e.ColumnIndex == clsCPTCalculate.M_GR_DEPTH
                    && dgvSections.Rows[e.RowIndex].Cells[clsCPTCalculate.M_GR_SECTIONTYPE].Value.ToString() == clsCPTCalculate.M_SECTIONTYPE_STRAIGHT)
                {
                    if (dgvSections.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString().Length > 0)
                    {
                        if (Convert.ToInt32(dgvSections.Rows[e.RowIndex].Cells[e.ColumnIndex].Value) != 0)
                        {
                            m_SkipSectionDefaults = true;
                            dgvSections.Rows[e.RowIndex].Cells[clsCPTCalculate.M_GR_SECTIONTYPE].Value = clsCPTCalculate.M_SECTIONTYPE_DIP;
                            dgvSections.Rows[e.RowIndex].Cells[clsCPTCalculate.M_GR_BENDANGLE].Value = 0;
                            dgvSections.Rows[e.RowIndex].Cells[clsCPTCalculate.M_GR_BENDRADIUS].Value = 0;
                            m_SkipSectionDefaults = false;
                        }
                    }
                }
                
            }
            catch (Exception ex)
            {
                MessageBox.Show(m_Application.ApplicationWindow, ConstantsDT.ERROR_CPT_CELL_EDIT + ": " + ex.Message, ConstantsDT.COMMAND_NAME_CABLE_PULL_TENSION, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        // Get the maximum distance the selected cable can be pulled through the selected duct.
        private bool GetMaxPullingDistance ()
        {
            bool bReturnValue = false;

            try
            {
                if (cboCable.SelectedItem != null && cboDuctSize.SelectedItem != null)
                {
                    string rsFilter = ConstantsDT.FIELD_CPT_VALIDATION_CABLE_ID + " = " + cboCable.Tag + " AND " +
                               ConstantsDT.FIELD_CPT_VALIDATION_DUCT_ID + " = " + cboDuctSize.Tag;

                    m_oValidationRS.Filter = rsFilter;

                    if (m_oValidationRS.RecordCount > 0)
                    {
                        if (!Convert.IsDBNull(m_oValidationRS.Fields[ConstantsDT.FIELD_CPT_VALIDATION_MAX_DISTANCE].Value))
                        {
                            m_CPTCalculate.MaxLength = Convert.ToInt32(m_oValidationRS.Fields[ConstantsDT.FIELD_CPT_VALIDATION_MAX_DISTANCE].Value);
                            txtResultsMaxLength.Text = m_CPTCalculate.MaxLength.ToString();
                        }
                        else
                        {
                            m_CPTCalculate.MaxLength = 0;
                            txtResultsMaxLength.Text = "";
                        }
                    }
                    else
                    {
                        txtResultsMaxLength.Text = "";
                    }

                    cmdCalculate.Enabled = true;
                    bReturnValue = true;
                }
                else
                {
                    cmdCalculate.Enabled = false;
                    bReturnValue = true;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(m_Application.ApplicationWindow, ConstantsDT.ERROR_CPT_GET_MAX_PULLING_DISTANCE + ": " + ex.Message, ConstantsDT.COMMAND_NAME_CABLE_PULL_TENSION, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                bReturnValue = false;
            }

            return bReturnValue;
        }

        // Write the validation results to the M_S_VALIDATIONLOG_TABLE table.
        private bool WriteValidationResults ()
        {
            bool bReturnValue = false;

            try
            {                
                string forwardTension = Math.Round(m_CPTCalculate.TotalTension, 0).ToString();
                string forwardSWBP = Math.Round(m_CPTCalculate.TotalSWBP, 0).ToString();
                string reverseTension = Math.Round(m_CPTCalculate.TotalReverseTension, 0).ToString();
                string reverseSWBP = Math.Round(m_CPTCalculate.TotalReverseSWBP, 0).ToString();
                string pullingDistance = Math.Round(m_CPTCalculate.TotalLength, 0).ToString();

                string validationStatus = ConstantsDT.VALIDATION_PASS;
                string validationPriority = "";

                if (m_CPTCalculate.TotalTension > m_CPTCalculate.MaxTension)
                {
                    forwardTension += " - EXCEEDED";
                    validationStatus = ConstantsDT.VALIDATION_FAIL;
                    validationPriority = ConstantsDT.VALIDATION_PRIORITY;
                }

                if (m_CPTCalculate.TotalSWBP > m_CPTCalculate.MaxSWBP)
                {
                    forwardSWBP += " - EXCEEDED";
                    validationStatus = ConstantsDT.VALIDATION_FAIL;
                    validationPriority = ConstantsDT.VALIDATION_PRIORITY;
                }

                if (m_CPTCalculate.TotalLength > m_CPTCalculate.MaxLength && m_CPTCalculate.MaxLength > 0)
                {
                    pullingDistance += " - EXCEEDED";
                    validationStatus = ConstantsDT.VALIDATION_FAIL;
                    validationPriority = ConstantsDT.VALIDATION_PRIORITY;
                }

                string comments = "Forward Tension: " + forwardTension + "; Forward SWBP: " + forwardSWBP + "; Reverse Tension: " + reverseTension +
                           "; Reverse SWBP: " + reverseSWBP + "; Pulling Distance: " + pullingDistance + ";";


                // Check if record exists for WR Number, G3E_FID and command
                Recordset validationRS = m_Application.DataContext.OpenRecordset(ConstantsDT.QUERY_VALIDATION, CursorTypeEnum.adOpenDynamic, 
                               LockTypeEnum.adLockOptimistic, (int)CommandTypeEnum.adCmdText, m_Application.DataContext.ActiveJob, m_SelectedFID, ConstantsDT.COMMAND_NAME_CABLE_PULL_TENSION);
                int recordsAffected = 0;

                if (validationRS.RecordCount > 0)
                {
                    // Update record
                    m_Application.DataContext.Execute(ConstantsDT.SQL_UPDATE_VALIDATION, out recordsAffected, (int)CommandTypeEnum.adCmdText,
                                                      validationStatus, validationPriority, comments, m_Application.DataContext.ActiveJob,
                                                      m_SelectedFID, ConstantsDT.COMMAND_NAME_CABLE_PULL_TENSION);
                    
                }
                else
                {
                    // Add record
                    m_Application.DataContext.Execute(ConstantsDT.SQL_INSERT_VALIDATION, out recordsAffected, (int)CommandTypeEnum.adCmdText,
                                                      m_Application.DataContext.ActiveJob, m_SelectedFID, ConstantsDT.COMMAND_NAME_CABLE_PULL_TENSION, validationStatus,
                                                      validationPriority, comments);
                }

                // Commit validation record to database.
                m_Application.DataContext.Execute("commit", out recordsAffected, (int)CommandTypeEnum.adCmdText);


                if (validationStatus == ConstantsDT.VALIDATION_PASS && lblCheckClearance.Text.Length == 0 && lblCheckJamRatio.Text.Length == 0)
                {
                    m_PassedValidation = true;
                }

                bReturnValue = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(m_Application.ApplicationWindow, ConstantsDT.ERROR_CPT_WRITING_VALIDATION_RESULTS + ": " + ex.Message, ConstantsDT.COMMAND_NAME_CABLE_PULL_TENSION, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                bReturnValue = false;
            }

            return bReturnValue;
        }

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
                reportValues.Add(new KeyValuePair<string, string> (ConstantsDT.REPORT_PARAMETER_VERSIONDATE, reportVersionDate));
                reportValues.Add(new KeyValuePair<string, string>(ConstantsDT.REPORT_PARAMETER_WR, m_Application.DataContext.ActiveJob));
                reportValues.Add(new KeyValuePair<string, string>(ConstantsDT.REPORT_PARAMETER_DATE, DateTime.Now.ToShortDateString()));
                reportValues.Add(new KeyValuePair<string, string>(ConstantsDT.REPORT_PARAMETER_WR_DESCRIPTION, CommonDT.WrDescription));
                reportValues.Add(new KeyValuePair<string, string>(ConstantsDT.REPORT_PARAMETER_CPT_VOLTAGE, m_SelectedVoltage));
                reportValues.Add(new KeyValuePair<string, string>(ConstantsDT.REPORT_PARAMETER_CPT_CABLE_TYPE, m_SelectedCableType));
                reportValues.Add(new KeyValuePair<string, string>(ConstantsDT.REPORT_PARAMETER_CPT_CONDUCTOR, m_SelectedConductor));
                reportValues.Add(new KeyValuePair<string, string>(ConstantsDT.REPORT_PARAMETER_CPT_PHASES, m_CPTCalculate.Phases));
                reportValues.Add(new KeyValuePair<string, string>(ConstantsDT.REPORT_PARAMETER_CPT_DUCT_SIZE, m_CPTCalculate.DuctDiameterNominal.ToString() + "\""));
                reportValues.Add(new KeyValuePair<string, string>(ConstantsDT.REPORT_PARAMETER_CPT_CONDUIT_ID, m_CPTCalculate.DuctDiameter.ToString() + "\""));
                reportValues.Add(new KeyValuePair<string, string>(ConstantsDT.REPORT_PARAMETER_CPT_MIN_BEND_RADIUS, txtStdBendRadius.Text));
                reportValues.Add(new KeyValuePair<string, string>(ConstantsDT.REPORT_PARAMETER_CPT_JAM_RATIO, m_CPTCalculate.JamRatio.ToString()));
                reportValues.Add(new KeyValuePair<string, string>(ConstantsDT.REPORT_PARAMETER_CPT_NON_STD, txtNonStd.Text));
                reportValues.Add(new KeyValuePair<string, string>(ConstantsDT.REPORT_PARAMETER_CPT_CABLE_WEIGHT, txtCableWeight.Text + " lb/ft"));
                reportValues.Add(new KeyValuePair<string, string>(ConstantsDT.REPORT_PARAMETER_CPT_MAX_TENSION, txtMaxTension.Text + " lbs"));
                reportValues.Add(new KeyValuePair<string, string>(ConstantsDT.REPORT_PARAMETER_CPT_MAX_SWBP, txtMaxSWBP.Text + " lb/ft"));
                reportValues.Add(new KeyValuePair<string, string>(ConstantsDT.REPORT_PARAMETER_CPT_CABLE_OD, txtCableOD.Text + "\""));
                reportValues.Add(new KeyValuePair<string, string>(ConstantsDT.REPORT_PARAMETER_CPT_CABLE_CONFIG, txtCableConfig.Text));
                reportValues.Add(new KeyValuePair<string, string>(ConstantsDT.REPORT_PARAMETER_CPT_WT_CORR_FACTOR, m_CPTCalculate.WeightCorrectionFactor.ToString()));
                reportValues.Add(new KeyValuePair<string, string>(ConstantsDT.REPORT_PARAMETER_CPT_CLEARANCE, m_CPTCalculate.CableClearance.ToString() + "\""));
                reportValues.Add(new KeyValuePair<string, string>(ConstantsDT.REPORT_PARAMETER_CPT_TOTAL_LENGTH, m_CPTCalculate.TotalLength.ToString()));
                reportValues.Add(new KeyValuePair<string, string>(ConstantsDT.REPORT_PARAMETER_CPT_CABLE_DESCRIPTION, cboCable.SelectedItem.ToString()));

                m_ReportDT = new ReportDT();

                m_ReportDT.ReportFile = m_ReportFileName;
                m_ReportDT.ReportName = m_ReportName;

                m_ReportDT.CreateReport(ConstantsDT.COMMAND_NAME_CABLE_PULL_TENSION, m_Application.DataContext.ActiveJob, m_SelectedFID, reportValues, dgvSections, null);
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

        private void dgvSections_UserDeletingRow(object sender, DataGridViewRowCancelEventArgs e)
        {
            // If in design mode then only allow pulleys to be deleted.
            if (m_bDesignMode && dgvSections.Rows[e.Row.Index].Cells[clsCPTCalculate.M_GR_SECTIONTYPE].Value.ToString() != clsCPTCalculate.M_SECTIONTYPE_PULLEY)
            {
                e.Cancel = true;
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

                // Check for Duct CU change
                if (m_DuctPendingEdit)
                {
                    if (!UpdateCU(clsCPTProcessSelectSet.SelectedFNO, m_SelectedFID, m_CableCU))
                    {
                        m_TransactionManager.Rollback();
                        return false;
                    }
                    m_DuctPendingEdit = false;
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

        // Save the report to a shared location and hyperlink the report to transformer
        private void cmdSaveReport_Click(object sender, EventArgs e)
        {
            string url = string.Empty;
            string docMgmtFileName = string.Empty;

            // Add report to document management
            if (m_ReportDT.UploadReport(m_ReportPDF, "Cable Pull Tension", ref url, ref docMgmtFileName))
            {
                m_TransactionManager.Begin("New Hyperlink");
                // Create a hyperlink component for the active feature
                if (m_ReportDT.AddHyperlinkComponent(m_SelectedFID, m_SelectedFNO, url, "Cable Pull Tension", null))
                {
                    m_TransactionManager.Commit();
                }
                else
                {
                    m_TransactionManager.Rollback();
                }
            }
        }

        // Launch CU Selection dialog and set Duct Size
        private void cmdDuctCU_Click(object sender, EventArgs e)
        {
            try
            {
                // TODO: JIRA 828 - Launch CU Selection
                // temporary solution
                frmSelectCU selectCU = new frmSelectCU(m_oCableRS);
                selectCU.StartPosition = FormStartPosition.CenterParent;
                selectCU.ShowDialog(m_Application.ApplicationWindow);

                string cu = string.Empty;

                if (selectCU.CU.Length > 0)
                {
                    cu = selectCU.CU;
                }
                else
                {
                    cu = cmdCableCU.Text.ToString();
                }

                // If CU is different than current CU, then disable the Print Report command. A new calculation will need to be executed. 
                if (cu != m_CableCU)
                {
                    m_CableCU = cu;                    
                    cmdCableCU.Text = cu;
                    m_oCableRS.Filter = ConstantsDT.FIELD_CPT_CABLE_CU + " = '" + m_CableCU + "'";
                    if (m_oCableRS.RecordCount > 0)
                    {
                        m_oCableRS.MoveFirst();
                        cboCable.Text = m_oCableRS.Fields[ConstantsDT.FIELD_CPT_CABLE_DESCRIPTION].Value.ToString();
                    }
                    ResetGridForCalculation();

                    if (m_bDesignMode)
                    {
                        m_DuctPendingEdit = true;
                        cmdCableCU.BackColor = m_PendingChangesColor;
                        cmdApply.Enabled = true;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(m_Application.ApplicationWindow, ConstantsDT.ERROR_CPT_DUCT_CHANGE + ": " + ex.Message, ConstantsDT.APPLICATION_NAME, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        private void lblCable_Click(object sender, EventArgs e)
        {

        }
    }
}
