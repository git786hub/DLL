using ADODB;
using GTechnology.Oncor.CustomAPI.DataAccess;
using GTechnology.Oncor.CustomAPI.Model;
using Intergraph.GTechnology.API;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace GTechnology.Oncor.CustomAPI
{
    public partial class ManualLandbaseReviewFrm : Form
    {
        #region Variables

        private IGTApplication gtApp;
        IGTTransactionManager gtTransactionManager;
        IGTCustomCommandHelper gtCustomCommandHelper;
        DataGridViewColumnSelector dgvColumnSelector = null;
        Highlightpath highLightPath;

        List<KeyValuePair<int, string>> FeatureAttributeComps;
        List<LBMAnalysisResults> LBMAnalysisFids;
        List<BuildingManual> Buildings;
        List<ParcelManual> Parcels;
        List<StreetCenterLine> StreetCenterLines;
        List<PipeLine> PipeLines;
        public enum FeatureClass {None, BuildingManual, ParcelManual, PipelineManual, StreetCenterlineManual }
        FeatureClass SelectedFeatureClass= FeatureClass.None;


        #endregion Variables

        /// <summary>
        /// Form Constructor
        /// </summary>
        public ManualLandbaseReviewFrm()
        {
            string sqlStmt = "SELECT distinct f.g3e_fno as Key,f.g3e_attributetable as Value FROM LBM_feature l,G3E_FEATURES_OPTABLE f where l.source_fno=f.g3e_fno " +
                                " union " +
                               "select distinct f.g3e_fno as Key,f.g3e_attributetable as Value FROM LBM_feature l, G3E_FEATURES_OPTABLE f where l.mapping_fno = f.g3e_fno";
            gtApp = GTClassFactory.Create<IGTApplication>();
            highLightPath = new Highlightpath();
            InitializeComponent();
            FeatureAttributeComps = CommonUtil.Execute<int, string>(sqlStmt);

        }


        /// <summary>
        /// Form Constructor with arguments
        /// </summary>
        /// <param name="GTCustomCommandHelper"></param>
        /// <param name="GTTransactionManager"></param>
        public ManualLandbaseReviewFrm(IGTCustomCommandHelper GTCustomCommandHelper, IGTTransactionManager GTTransactionManager) : this()
        {
            gtTransactionManager = GTTransactionManager;
            gtCustomCommandHelper = GTCustomCommandHelper;
            dgvColumnSelector = new DataGridViewColumnSelector();
            dgvManualLandbaseReview.CellFormatting += DgvManualLandbaseReview_CellFormatting;

            GetLBMAnalysisResults();
            StreetCenterLines = GetStreetCenterLines();
            PipeLines = GetPipeLines();

            dgvManualLandbaseReview.AutoGenerateColumns = false;

            dgvColumnSelector.mCheckedListBoxPrimFID.SelectedIndexChanged += MCheckedListBoxPrimFID_SelectedIndexChanged;
            dgvColumnSelector.mCheckedListBoxStage.SelectedIndexChanged += MCheckedListBoxStage_SelectedIndexChanged;
            dgvColumnSelector.mCheckedListBoxName.SelectedIndexChanged += MCheckedListBoxName_SelectedIndexChanged;
            dgvColumnSelector.mCheckedListBoxLocation.SelectedIndexChanged += MCheckedListBoxLocation_SelectedIndexChanged;
            dgvColumnSelector.mCheckedListBoxAddress.SelectedIndexChanged += MCheckedListBoxAddress_SelectedIndexChanged;
            dgvColumnSelector.mCheckedListBoxAbstract.SelectedIndexChanged += MCheckedListBoxAbstract_SelectedIndexChanged;
            dgvColumnSelector.mCheckedListBoxSectionDescription.SelectedIndexChanged += MCheckedListBoxSectionDescription_SelectedIndexChanged; ;

            dgvColumnSelector.mCheckedListBoxAdditionName.SelectedIndexChanged += MCheckedListBoxAdditionName_SelectedIndexChanged;
            dgvColumnSelector.mCheckedListBoxBlockNumber.SelectedIndexChanged += MCheckedListBoxBlockNumber_SelectedIndexChanged;

            dgvColumnSelector.mCheckedListBoxLotNumber.SelectedIndexChanged += MCheckedListBoxLotNumber_SelectedIndexChanged;
            dgvColumnSelector.mCheckedListBoxPrefix.SelectedIndexChanged += MCheckedListBoxPrefix_SelectedIndexChanged;
            dgvColumnSelector.mCheckedListBoxSuffix.SelectedIndexChanged += MCheckedListBoxSuffix_SelectedIndexChanged;
            dgvColumnSelector.mCheckedListBoxPretype.SelectedIndexChanged += MCheckedListBoxPretype_SelectedIndexChanged;

            dgvColumnSelector.mCheckedListBoxType.SelectedIndexChanged += MCheckedListBoxType_SelectedIndexChanged;
            dgvColumnSelector.mCheckedListBoxStrtCtrName.SelectedIndexChanged += MCheckedListBoxStrtCtrName_SelectedIndexChanged;
            dgvColumnSelector.mCheckedListBoxOwner.SelectedIndexChanged += MCheckedListBoxOwner_SelectedIndexChanged;

            dgvColumnSelector.mCheckedListBoxStrtCtrType.SelectedIndexChanged += MCheckedListBoxStrtCtrType_SelectedIndexChanged;
            dgvColumnSelector.mCheckedListBoxWidth.SelectedIndexChanged += MCheckedListBoxWidth_SelectedIndexChanged;

            dgvColumnSelector.mCheckedListBoxPipelnMlName.SelectedIndexChanged += MCheckedListBoxPipelnMlName_SelectedIndexChanged;
            dgvColumnSelector.mCheckedListBoxAddressRange.SelectedIndexChanged += MCheckedListBoxAddressRange_SelectedIndexChanged;

        }

        #region Private Methods

        /// <summary>
        /// Get allDetected Building and polygons from LBM_analysisresults table
        /// </summary>
        private void GetLBMAnalysisResults()
        {
            string sqlStmt = "Select distinct g3e_fid2 as Key,g3e_fno2 as value from LBM_ANALYSISRESULT where g3e_fid={0} order by g3e_fid2";
            Buildings = GetBldgMLAnalysisResults();
            Parcels = GetParcelAnalysisResults();
            LBMAnalysisFids = new List<LBMAnalysisResults>();
            //Cache Detecting analysis Fids - to highlight Fids on map window when user selected row in datagrid
            if (Buildings != null)
            {
                foreach (BuildingManual bldg in Buildings)
                {
                    LBMAnalysisFids.Add(new LBMAnalysisResults
                    {
                        G3eFid = bldg.G3eFid,
                        G3eFno = bldg.G3eFno,
                        DetectPolygons = CommonUtil.Execute<int, int>(string.Format(sqlStmt, bldg.G3eFid))
                    });
                }
            }
            if (Parcels != null)
            {
                foreach (ParcelManual prcl in Parcels)
                {
                    LBMAnalysisFids.Add(new LBMAnalysisResults
                    {
                        G3eFid = prcl.G3eFid,
                        G3eFno = prcl.G3eFno,
                        DetectPolygons = CommonUtil.Execute<int, int>(string.Format(sqlStmt, prcl.G3eFid))
                    });
                }
            }
        }

        /// <summary>
        /// Get all detected Building polygons 
        /// </summary>
        /// <returns></returns>
        private List<BuildingManual> GetBldgMLAnalysisResults()
        {
            List<BuildingManual> bldgs = null;
            string sqlStmt = " select DISTINCT ml.g3e_fno as G3eFno,ml.g3e_fid as G3efid,l.Stage as Stage,ml.NAME as Name,ml.LOCATION as Location " +
                "From LBM_ANALYSISRESULT lb,LAND_AUDIT_N l ,{0} ml " +
                " where lb.g3e_fid = ml.g3e_fid and lb.g3e_fid = l.g3e_fid order by ml.g3e_fid ";
            if (FeatureAttributeComps.Count(a => a.Key == 217) > 0)
            {
                sqlStmt = string.Format(sqlStmt, FeatureAttributeComps.First(a => a.Key == 217).Value);
                bldgs = CommonUtil.Execute<BuildingManual>(sqlStmt);
            }

            return bldgs;
        }

        /// <summary>
        /// Get All detected parcels
        /// </summary>
        /// <returns></returns>
        private List<ParcelManual> GetParcelAnalysisResults()
        {
            List<ParcelManual> parcels = null;
            string sqlStmt = "  select DISTINCT ml.g3e_fno as G3efno,ml.g3e_fid G3eFid,l.Stage as Stage,ml.ADDRESS as Address" +
                ",ml.ABSTRACT as ParcelAbstract,ml.SECTION_DESCRIPTION as SectionDescription,ml.ADDITION_NAME as AdditionName" +
                ",ml.BLOCK_NUMBER as BlockNumber,ml.LOT_NUMBER as LotNumber " +
                "from LBM_ANALYSISRESULT lb,LAND_AUDIT_N l, {0} ml " +
                "where lb.g3e_fid = ml.g3e_fid and lb.g3e_fid = l.g3e_fid order by ml.g3e_fid";
            sqlStmt = string.Format(sqlStmt, FeatureAttributeComps.First(a => a.Key == 224).Value);
            if (FeatureAttributeComps.Count(a => a.Key == 224) > 0)
            {
                parcels = CommonUtil.Execute<ParcelManual>(sqlStmt);
            }
            return parcels;
        }

        /// <summary>
        /// Get all Street Center Lines
        /// </summary>
        /// <returns></returns>
        private List<StreetCenterLine> GetStreetCenterLines()
        {
            List<StreetCenterLine> strtCntLines = null;
            string sqlStmt = "select A.G3efno,A.G3eFID,A.STAGE,a.min_val || ',' || a.max_val Address_Range ,A.PREFIX,A.PRETYPE,A.NAME,A.TYPE,A.SUFFIX " +
                "FROM (select n.G3E_Fno as G3efno,N.G3E_FID G3eFID" +
                ", Case when ML.L_F_ADD < ML.L_T_ADD and ML.L_F_ADD < ML.R_F_ADD and ML.L_F_ADD < ML.R_T_ADD then L_F_ADD " +
                "when ML.L_T_ADD < ML.L_F_ADD and ML.L_T_ADD < ML.R_F_ADD and ML.L_T_ADD < ML.R_T_ADD then L_T_ADD " +
                "when ML.R_F_ADD < ML.L_F_ADD and ML.R_F_ADD < ML.L_T_ADD and ML.R_F_ADD < ML.R_T_ADD then R_F_ADD " +
                "else R_T_ADD end min_val" +
                ",Case when ML.L_F_ADD > ML.L_T_ADD and ML.L_F_ADD > ML.R_F_ADD and ML.L_F_ADD > ML.R_T_ADD then L_F_ADD " +
                "when ML.L_T_ADD > ML.L_F_ADD and ML.L_T_ADD > ML.R_F_ADD and ML.L_T_ADD > ML.R_T_ADD then L_T_ADD " +
                "when ML.R_F_ADD > ML.L_F_ADD and ML.R_F_ADD > ML.L_T_ADD and ML.R_F_ADD > ML.R_T_ADD then R_F_ADD " +
                "else R_T_ADD end max_val" +
                ", N.STAGE Stage, ML.L_F_ADD,ML.L_T_ADD,ML.R_F_ADD,ML.R_T_ADD,ML.PREFIX Prefix, ML.PRETYPE Pretype, ML.NAME Name, ML.TYPE Type, ML.SUFFIX Suffix " +
                "from LAND_AUDIT_N N ,STREETCTR_ML_N ML where ml.g3e_fid = n.g3e_fid and n.STAGE='Accepted') a";

            strtCntLines = CommonUtil.Execute<StreetCenterLine>(sqlStmt);
            return strtCntLines;
        }

        /// <summary>
        /// Get all Pipelines
        /// </summary>
        /// <returns></returns>
        private List<PipeLine> GetPipeLines()
        {
            List<PipeLine> pipeLines = null;
            string sqlStmt = "select n.g3e_fno as G3efno,n.g3e_fid G3eFid,n.STAGE stage,ml.NAME Name,ml.OWNER Owner,ml.TYPE Type,ml.MATERIAL Material,ml.WIDTH Width from LAND_AUDIT_N n,PIPELINE_ML_N ml where ml.g3e_fid=n.g3e_fid and n.STAGE='Accepted'";

            pipeLines = CommonUtil.Execute<PipeLine>(sqlStmt);
            return pipeLines;
        }

        /// <summary>
        /// Load Data to grid
        /// </summary>
        private void LoadDataGrid()
        {
            chckPendingFilter.Checked = true;
            chckPendingFilter.Enabled = true;
            if (Convert.ToString(cmbFeatureList.SelectedItem) == "Building - Manual")
            {
                var bldgs = chckPendingFilter.Checked == true ? Buildings.Where(bldg => bldg.Stage == "Pending").ToList() : Buildings;
                BindBuildingToGrid(bldgs);
                SelectedFeatureClass = FeatureClass.BuildingManual;
                dgvColumnSelector.LoadBldgFilterList(bldgs);
            }
            if (Convert.ToString(cmbFeatureList.SelectedItem) == "Parcel - Manual")
            {
                var prcls = chckPendingFilter.Checked == true ? Parcels.Where(prcl => prcl.Stage == "Pending").ToList() : Parcels;
                SelectedFeatureClass = FeatureClass.ParcelManual;
                BindParceltoGrid(prcls);
                dgvColumnSelector.LoadParcelFilterList(prcls);
            }
            if (Convert.ToString(cmbFeatureList.SelectedItem) == "Street Centerline - Manual")
            {
                SelectedFeatureClass = FeatureClass.StreetCenterlineManual;
                BindStreetCenterLineToGrid(StreetCenterLines);
                dgvColumnSelector.LoadStreetCntFilterLst(StreetCenterLines);

                chckPendingFilter.Checked = false;
                chckPendingFilter.Enabled = false;
            }
            if (Convert.ToString(cmbFeatureList.SelectedItem) == "Pipeline Manual")
            {
                SelectedFeatureClass = FeatureClass.PipelineManual;
                BindPipeLineToGrid(PipeLines);
                dgvColumnSelector.LoadPipeLineFilterLst(PipeLines);
                chckPendingFilter.Checked = false;
                chckPendingFilter.Enabled = false;
            }
        }


        /// <summary>
        /// Bind Parcel data to Grid
        /// </summary>
        /// <param name="prcls"></param>
        private void BindParceltoGrid(List<ParcelManual> prcls)
        {
            dgvManualLandbaseReview.DataSource = null;
            dgvManualLandbaseReview.Columns.Clear();
            dgvManualLandbaseReview.Columns.Add("G3eFid", "FID");
            dgvManualLandbaseReview.Columns.Add("Stage", "Stage");
            dgvManualLandbaseReview.Columns.Add("Address", "Address");
            dgvManualLandbaseReview.Columns.Add("parcelMLAbstract", "Abstract");
            dgvManualLandbaseReview.Columns.Add("SectionDescription", "Section Description");
            dgvManualLandbaseReview.Columns.Add("AdditionName", "Addition Name");
            dgvManualLandbaseReview.Columns.Add("BlockNumber", "Block Number");
            dgvManualLandbaseReview.Columns.Add("LotNumber", "Lot Number");

            dgvManualLandbaseReview.Columns["G3eFid"].DataPropertyName = "G3eFid";
            dgvManualLandbaseReview.Columns["Stage"].DataPropertyName = "Stage";
            dgvManualLandbaseReview.Columns["Address"].DataPropertyName = "Address";
            dgvManualLandbaseReview.Columns["ParcelMLAbstract"].DataPropertyName = "ParcelMLAbstract";
            dgvManualLandbaseReview.Columns["SectionDescription"].DataPropertyName = "SectionDescription";
            dgvManualLandbaseReview.Columns["AdditionName"].DataPropertyName = "AdditionName";
            dgvManualLandbaseReview.Columns["BlockNumber"].DataPropertyName = "BlockNumber";
            dgvManualLandbaseReview.Columns["LotNumber"].DataPropertyName = "LotNumber";


            dgvManualLandbaseReview.DataSource = prcls;
        }

        /// <summary>
        /// Bind Building data to Grid
        /// </summary>
        /// <param name="bldgs"></param>
        private void BindBuildingToGrid(List<BuildingManual> bldgs)
        {
            dgvManualLandbaseReview.DataSource = null;
            dgvManualLandbaseReview.Columns.Clear();
            dgvManualLandbaseReview.Columns.Add("G3eFid", "FID");
            dgvManualLandbaseReview.Columns.Add("Stage", "Stage");
            dgvManualLandbaseReview.Columns.Add("Name", "Name");
            dgvManualLandbaseReview.Columns.Add("Location", "Location");

            dgvManualLandbaseReview.Columns["G3eFid"].DataPropertyName = "G3eFid";
            dgvManualLandbaseReview.Columns["Stage"].DataPropertyName = "Stage";
            dgvManualLandbaseReview.Columns["Name"].DataPropertyName = "Name";
            dgvManualLandbaseReview.Columns["Location"].DataPropertyName = "Location";

            dgvManualLandbaseReview.DataSource = bldgs;

        }

        /// <summary>
        /// Bind Street Center Line to Grid
        /// </summary>
        /// <param name="stltCntrsLn"></param>
        private void BindStreetCenterLineToGrid(List<StreetCenterLine> stltCntrsLn)
        {
            dgvManualLandbaseReview.DataSource = null;
            dgvManualLandbaseReview.Columns.Clear();
            dgvManualLandbaseReview.Columns.Add("G3eFid", "FID");
            dgvManualLandbaseReview.Columns.Add("Stage", "Stage");
            dgvManualLandbaseReview.Columns.Add("Name", "Name");
            dgvManualLandbaseReview.Columns.Add("Owner", "Owner");
            dgvManualLandbaseReview.Columns.Add("Type", "Type");
            dgvManualLandbaseReview.Columns.Add("Width", "Width");

            dgvManualLandbaseReview.Columns["G3eFid"].DataPropertyName = "G3eFid";
            dgvManualLandbaseReview.Columns["Stage"].DataPropertyName = "Stage";
            dgvManualLandbaseReview.Columns["Name"].DataPropertyName = "Name";
            dgvManualLandbaseReview.Columns["Owner"].DataPropertyName = "Owner";
            dgvManualLandbaseReview.Columns["Type"].DataPropertyName = "Type";
            dgvManualLandbaseReview.Columns["Width"].DataPropertyName = "Width";

            dgvManualLandbaseReview.DataSource = stltCntrsLn;
        }

        /// <summary>
        /// Bind Pipline data to Grid
        /// </summary>
        /// <param name="pipeLines"></param>
        private void BindPipeLineToGrid(List<PipeLine> pipeLines)
        {
            dgvManualLandbaseReview.DataSource = null;
            dgvManualLandbaseReview.Columns.Clear();
            dgvManualLandbaseReview.Columns.Add("G3eFid", "FID");
            dgvManualLandbaseReview.Columns.Add("Stage", "Stage");
            dgvManualLandbaseReview.Columns.Add("Name", "Name");
            dgvManualLandbaseReview.Columns.Add("AddressRange", "AddressRange");
            dgvManualLandbaseReview.Columns.Add("Type", "Type");
            dgvManualLandbaseReview.Columns.Add("Prefix", "Prefix");
            dgvManualLandbaseReview.Columns.Add("PreType", "PreType");
            dgvManualLandbaseReview.Columns.Add("Suffix", "Suffix");

            dgvManualLandbaseReview.Columns["G3eFid"].DataPropertyName = "G3eFid";
            dgvManualLandbaseReview.Columns["Stage"].DataPropertyName = "Stage";
            dgvManualLandbaseReview.Columns["Name"].DataPropertyName = "Name";
            dgvManualLandbaseReview.Columns["AddressRange"].DataPropertyName = "AddressRange";
            dgvManualLandbaseReview.Columns["Type"].DataPropertyName = "Type";
            dgvManualLandbaseReview.Columns["Prefix"].DataPropertyName = "Prefix";
            dgvManualLandbaseReview.Columns["PreType"].DataPropertyName = "PreType";
            dgvManualLandbaseReview.Columns["Suffix"].DataPropertyName = "Suffix";

            dgvManualLandbaseReview.DataSource = pipeLines;
        }


        private void StageValueUpdate(string stageType, int vSourceFid)
        {
            string mySql = string.Empty;
           // Recordset rsStage = null;
            int records = 0;


            mySql = "UPDATE LAND_AUDIT_N SET STAGE= '" + stageType + "' WHERE G3E_FID=" + vSourceFid;
            gtApp.DataContext.Execute(mySql, out records, Convert.ToInt32(CommandTypeEnum.adCmdText), null);
        }

        #endregion

        #region public Methods

        /// <summary>
        /// Release all objects / all instances and dispose datagridview and the datatable.
        /// </summary>
        public void CloseForm()
        {
            try
            {
                dgvManualLandbaseReview.Dispose();
                dgvColumnSelector = null;
                highLightPath.RemoveDisplayNode("Landbase Feature Data");
                highLightPath = null;
                ccManualLandbaseReview.ManualLandbaseReviewCtl = null;
                if (gtCustomCommandHelper != null)
                {
                    gtCustomCommandHelper.Complete();
                    gtCustomCommandHelper = null;
                }
                gtCustomCommandHelper = null;
                gtTransactionManager = null;
                gtApp.Application.EndWaitCursor();
                gtApp.Application.RefreshWindows();
                Application.Exit();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error during execution of LandBase Analysis custom command." + Environment.NewLine + ex.Message,
                 "Manual Landbase Review", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        #endregion

        #region Events
        private void DgvManualLandbaseReview_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (Convert.ToString(dgvManualLandbaseReview.Rows[e.RowIndex].Cells["Stage"].Value) == "Accepted")
            {
                dgvManualLandbaseReview.Rows[e.RowIndex].DefaultCellStyle.BackColor = Color.LightGray;
            }
            if (Convert.ToString(dgvManualLandbaseReview.Rows[e.RowIndex].Cells["Stage"].Value) == "Archived")
            {
                dgvManualLandbaseReview.Rows[e.RowIndex].DefaultCellStyle.BackColor = Color.DarkGray;
            }
        }

        /// <summary>
        /// To show filter in the grid by right-clicking on a column header.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dgvManualLandbaseReview_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right && e.RowIndex == -1)
            {

                if (dgvManualLandbaseReview.Columns[e.ColumnIndex].Name == "G3eFid")
                {
                    if (dgvColumnSelector.mCheckedListBoxPrimFID.Items.Count > 0)
                    {
                        dgvColumnSelector.mCheckedListBoxPrimFID.ClientSize = new Size(dgvColumnSelector.mCheckedListBoxPrimFID.ClientSize.Width, dgvColumnSelector.mCheckedListBoxPrimFID.GetItemRectangle(0).Height * dgvColumnSelector.mCheckedListBoxPrimFID.Items.Count);
                        dgvColumnSelector.mPopupPrimFID.Height = dgvColumnSelector.mCheckedListBoxPrimFID.ClientSize.Height;
                        dgvColumnSelector.mPopupPrimFID.Width = dgvColumnSelector.mCheckedListBoxPrimFID.ClientSize.Width;
                        dgvColumnSelector.mPopupPrimFID.Show(Control.MousePosition);
                    }
                }

                if (dgvManualLandbaseReview.Columns[e.ColumnIndex].Name.ToUpper() == "STAGE")
                {
                    if (dgvColumnSelector.mCheckedListBoxStage.Items.Count > 0)
                    {
                        dgvColumnSelector.mCheckedListBoxStage.ClientSize = new Size(dgvColumnSelector.mCheckedListBoxStage.ClientSize.Width, dgvColumnSelector.mCheckedListBoxStage.GetItemRectangle(0).Height * dgvColumnSelector.mCheckedListBoxStage.Items.Count);
                        dgvColumnSelector.mPopupStage.Height = dgvColumnSelector.mCheckedListBoxStage.ClientSize.Height;
                        dgvColumnSelector.mPopupStage.Width = dgvColumnSelector.mCheckedListBoxStage.ClientSize.Width;
                        dgvColumnSelector.mPopupStage.Show(Control.MousePosition);
                    }
                }



                if (SelectedFeatureClass == FeatureClass.BuildingManual)
                {
                    if (dgvManualLandbaseReview.Columns[e.ColumnIndex].Name.ToUpper() == "NAME")
                    {

                        if (dgvColumnSelector.mCheckedListBoxName.Items.Count > 0)
                        {
                            dgvColumnSelector.mCheckedListBoxName.ClientSize = new Size(dgvColumnSelector.mCheckedListBoxName.ClientSize.Width, dgvColumnSelector.mCheckedListBoxName.GetItemRectangle(0).Height * dgvColumnSelector.mCheckedListBoxName.Items.Count);
                            dgvColumnSelector.mPopupName.Height = dgvColumnSelector.mCheckedListBoxName.ClientSize.Height;
                            dgvColumnSelector.mPopupName.Width = dgvColumnSelector.mCheckedListBoxName.ClientSize.Width;
                            dgvColumnSelector.mPopupName.Show(Control.MousePosition);
                        }

                    }
                    if (dgvManualLandbaseReview.Columns[e.ColumnIndex].Name == "Location")
                    {
                        if (dgvColumnSelector.mCheckedListBoxLocation.Items.Count > 0)
                        {
                            dgvColumnSelector.mCheckedListBoxLocation.ClientSize = new Size(dgvColumnSelector.mCheckedListBoxLocation.ClientSize.Width, dgvColumnSelector.mCheckedListBoxLocation.GetItemRectangle(0).Height * dgvColumnSelector.mCheckedListBoxLocation.Items.Count);
                            dgvColumnSelector.mPopupLocation.Height = dgvColumnSelector.mCheckedListBoxLocation.ClientSize.Height;
                            dgvColumnSelector.mPopupLocation.Width = dgvColumnSelector.mCheckedListBoxLocation.ClientSize.Width;
                            dgvColumnSelector.mPopupLocation.Show(Control.MousePosition);
                        }
                    }
                }
                if (SelectedFeatureClass == FeatureClass.ParcelManual)
                {

                    if (dgvManualLandbaseReview.Columns[e.ColumnIndex].Name == "Address")
                    {
                        if (dgvColumnSelector.mCheckedListBoxAddress.Items.Count > 0)
                        {
                            dgvColumnSelector.mCheckedListBoxAddress.ClientSize = new Size(dgvColumnSelector.mCheckedListBoxAddress.ClientSize.Width, dgvColumnSelector.mCheckedListBoxAddress.GetItemRectangle(0).Height * dgvColumnSelector.mCheckedListBoxAddress.Items.Count);
                            dgvColumnSelector.mPopupAddress.Height = dgvColumnSelector.mCheckedListBoxAddress.ClientSize.Height;
                            dgvColumnSelector.mPopupAddress.Width = dgvColumnSelector.mCheckedListBoxAddress.ClientSize.Width;
                            dgvColumnSelector.mPopupAddress.Show(Control.MousePosition);
                        }
                    }
                    if (dgvManualLandbaseReview.Columns[e.ColumnIndex].Name == "ParcelMLAbstract")
                    {
                        if (dgvColumnSelector.mCheckedListBoxAbstract.Items.Count > 0)
                        {
                            dgvColumnSelector.mCheckedListBoxAbstract.ClientSize = new Size(dgvColumnSelector.mCheckedListBoxAbstract.ClientSize.Width, dgvColumnSelector.mCheckedListBoxAbstract.GetItemRectangle(0).Height * dgvColumnSelector.mCheckedListBoxAbstract.Items.Count);
                            dgvColumnSelector.mPopupAbstract.Height = dgvColumnSelector.mCheckedListBoxAbstract.ClientSize.Height;
                            dgvColumnSelector.mPopupAbstract.Width = dgvColumnSelector.mCheckedListBoxAbstract.ClientSize.Width;
                            dgvColumnSelector.mPopupAbstract.Show(Control.MousePosition);
                        }
                    }
                    if (dgvManualLandbaseReview.Columns[e.ColumnIndex].Name == "SectionDescription")
                    {
                        if (dgvColumnSelector.mCheckedListBoxSectionDescription.Items.Count > 0)
                        {
                            dgvColumnSelector.mCheckedListBoxSectionDescription.ClientSize = new Size(dgvColumnSelector.mCheckedListBoxSectionDescription.ClientSize.Width, dgvColumnSelector.mCheckedListBoxSectionDescription.GetItemRectangle(0).Height * dgvColumnSelector.mCheckedListBoxSectionDescription.Items.Count);
                            dgvColumnSelector.mPopupSectionDescription.Height = dgvColumnSelector.mCheckedListBoxSectionDescription.ClientSize.Height;
                            dgvColumnSelector.mPopupSectionDescription.Width = dgvColumnSelector.mCheckedListBoxSectionDescription.ClientSize.Width;
                            dgvColumnSelector.mPopupSectionDescription.Show(Control.MousePosition);
                        }
                    }
                    if (dgvManualLandbaseReview.Columns[e.ColumnIndex].Name == "AdditionName")
                    {
                        if (dgvColumnSelector.mCheckedListBoxAdditionName.Items.Count > 0)
                        {
                            dgvColumnSelector.mCheckedListBoxAdditionName.ClientSize = new Size(dgvColumnSelector.mCheckedListBoxAdditionName.ClientSize.Width, dgvColumnSelector.mCheckedListBoxAdditionName.GetItemRectangle(0).Height * dgvColumnSelector.mCheckedListBoxAdditionName.Items.Count);
                            dgvColumnSelector.mPopupAdditionName.Height = dgvColumnSelector.mCheckedListBoxAdditionName.ClientSize.Height;
                            dgvColumnSelector.mPopupAdditionName.Width = dgvColumnSelector.mCheckedListBoxAdditionName.ClientSize.Width;
                            dgvColumnSelector.mPopupAdditionName.Show(Control.MousePosition);
                        }
                    }
                    if (dgvManualLandbaseReview.Columns[e.ColumnIndex].Name == "BlockNumber")
                    {
                        if (dgvColumnSelector.mCheckedListBoxBlockNumber.Items.Count > 0)
                        {
                            dgvColumnSelector.mCheckedListBoxBlockNumber.ClientSize = new Size(dgvColumnSelector.mCheckedListBoxBlockNumber.ClientSize.Width, dgvColumnSelector.mCheckedListBoxBlockNumber.GetItemRectangle(0).Height * dgvColumnSelector.mCheckedListBoxBlockNumber.Items.Count);
                            dgvColumnSelector.mPopupBlockNumber.Height = dgvColumnSelector.mCheckedListBoxBlockNumber.ClientSize.Height;
                            dgvColumnSelector.mPopupBlockNumber.Width = dgvColumnSelector.mCheckedListBoxBlockNumber.ClientSize.Width;
                            dgvColumnSelector.mPopupBlockNumber.Show(Control.MousePosition);
                        }
                    }
                    if (dgvManualLandbaseReview.Columns[e.ColumnIndex].Name == "LotNumber")
                    {
                        if (dgvColumnSelector.mCheckedListBoxLotNumber.Items.Count > 0)
                        {
                            dgvColumnSelector.mCheckedListBoxLotNumber.ClientSize = new Size(dgvColumnSelector.mCheckedListBoxLotNumber.ClientSize.Width, dgvColumnSelector.mCheckedListBoxLotNumber.GetItemRectangle(0).Height * dgvColumnSelector.mCheckedListBoxLotNumber.Items.Count);
                            dgvColumnSelector.mPopupLotNumber.Height = dgvColumnSelector.mCheckedListBoxLotNumber.ClientSize.Height;
                            dgvColumnSelector.mPopupLotNumber.Width = dgvColumnSelector.mCheckedListBoxLotNumber.ClientSize.Width;
                            dgvColumnSelector.mPopupLotNumber.Show(Control.MousePosition);
                        }
                    }

                }

                if (SelectedFeatureClass == FeatureClass.PipelineManual)
                {
                    if (dgvManualLandbaseReview.Columns[e.ColumnIndex].Name.ToUpper() == "NAME")
                    {
                        if (dgvColumnSelector.mCheckedListBoxPipelnMlName.Items.Count > 0)
                        {
                            dgvColumnSelector.mCheckedListBoxPipelnMlName.ClientSize = new Size(dgvColumnSelector.mCheckedListBoxPipelnMlName.ClientSize.Width, dgvColumnSelector.mCheckedListBoxPipelnMlName.GetItemRectangle(0).Height * dgvColumnSelector.mCheckedListBoxPipelnMlName.Items.Count);
                            dgvColumnSelector.mPopupPipelnMlName.Height = dgvColumnSelector.mCheckedListBoxPipelnMlName.ClientSize.Height;
                            dgvColumnSelector.mPopupPipelnMlName.Width = dgvColumnSelector.mCheckedListBoxPipelnMlName.ClientSize.Width;
                            dgvColumnSelector.mPopupPipelnMlName.Show(Control.MousePosition);
                        }
                    }

                    if (dgvManualLandbaseReview.Columns[e.ColumnIndex].Name.ToUpper() == "TYPE")
                    {
                        if (dgvColumnSelector.mCheckedListBoxType.Items.Count > 0)
                        {
                            dgvColumnSelector.mCheckedListBoxType.ClientSize = new Size(dgvColumnSelector.mCheckedListBoxType.ClientSize.Width, dgvColumnSelector.mCheckedListBoxType.GetItemRectangle(0).Height * dgvColumnSelector.mCheckedListBoxType.Items.Count);
                            dgvColumnSelector.mPopupType.Height = dgvColumnSelector.mCheckedListBoxType.ClientSize.Height;
                            dgvColumnSelector.mPopupType.Width = dgvColumnSelector.mCheckedListBoxType.ClientSize.Width;
                            dgvColumnSelector.mPopupType.Show(Control.MousePosition);
                        }
                    }

                    if (dgvManualLandbaseReview.Columns[e.ColumnIndex].Name.ToUpper() == "OWNER")
                    {
                        if (dgvColumnSelector.mCheckedListBoxOwner.Items.Count > 0)
                        {
                            dgvColumnSelector.mCheckedListBoxOwner.ClientSize = new Size(dgvColumnSelector.mCheckedListBoxOwner.ClientSize.Width, dgvColumnSelector.mCheckedListBoxOwner.GetItemRectangle(0).Height * dgvColumnSelector.mCheckedListBoxOwner.Items.Count);
                            dgvColumnSelector.mPopupOwner.Height = dgvColumnSelector.mCheckedListBoxOwner.ClientSize.Height;
                            dgvColumnSelector.mPopupOwner.Width = dgvColumnSelector.mCheckedListBoxOwner.ClientSize.Width;
                            dgvColumnSelector.mPopupOwner.Show(Control.MousePosition);
                        }
                    }
                    if (dgvManualLandbaseReview.Columns[e.ColumnIndex].Name.ToUpper() == "MATERIAL")
                    {
                        if (dgvColumnSelector.mCheckedListBoxMaterial.Items.Count > 0)
                        {
                            dgvColumnSelector.mCheckedListBoxMaterial.ClientSize = new Size(dgvColumnSelector.mCheckedListBoxMaterial.ClientSize.Width, dgvColumnSelector.mCheckedListBoxMaterial.GetItemRectangle(0).Height * dgvColumnSelector.mCheckedListBoxMaterial.Items.Count);
                            dgvColumnSelector.mPopupMaterial.Height = dgvColumnSelector.mCheckedListBoxMaterial.ClientSize.Height;
                            dgvColumnSelector.mPopupMaterial.Width = dgvColumnSelector.mCheckedListBoxMaterial.ClientSize.Width;
                            dgvColumnSelector.mPopupMaterial.Show(Control.MousePosition);
                        }
                    }
                    if (dgvManualLandbaseReview.Columns[e.ColumnIndex].Name.ToUpper() == "WIDTH")
                    {
                        if (dgvColumnSelector.mCheckedListBoxWidth.Items.Count > 0)
                        {
                            dgvColumnSelector.mCheckedListBoxWidth.ClientSize = new Size(dgvColumnSelector.mCheckedListBoxWidth.ClientSize.Width, dgvColumnSelector.mCheckedListBoxWidth.GetItemRectangle(0).Height * dgvColumnSelector.mCheckedListBoxWidth.Items.Count);
                            dgvColumnSelector.mPopupWidth.Height = dgvColumnSelector.mCheckedListBoxWidth.ClientSize.Height;
                            dgvColumnSelector.mPopupWidth.Width = dgvColumnSelector.mCheckedListBoxWidth.ClientSize.Width;
                            dgvColumnSelector.mPopupWidth.Show(Control.MousePosition);
                        }
                    }

                }

                if (SelectedFeatureClass == FeatureClass.StreetCenterlineManual)
                {
                    if (dgvManualLandbaseReview.Columns[e.ColumnIndex].Name.ToUpper() == "NAME")
                    {
                        if (dgvColumnSelector.mCheckedListBoxStrtCtrName.Items.Count > 0)
                        {
                            dgvColumnSelector.mCheckedListBoxStrtCtrName.ClientSize = new Size(dgvColumnSelector.mCheckedListBoxStrtCtrName.ClientSize.Width, dgvColumnSelector.mCheckedListBoxStrtCtrName.GetItemRectangle(0).Height * dgvColumnSelector.mCheckedListBoxStrtCtrName.Items.Count);
                            dgvColumnSelector.mPopupStrtCtrName.Height = dgvColumnSelector.mCheckedListBoxStrtCtrName.ClientSize.Height;
                            dgvColumnSelector.mPopupStrtCtrName.Width = dgvColumnSelector.mCheckedListBoxStrtCtrName.ClientSize.Width;
                            dgvColumnSelector.mPopupStrtCtrName.Show(Control.MousePosition);
                        }
                    }
                    if (dgvManualLandbaseReview.Columns[e.ColumnIndex].Name.ToUpper() == "TYPE")
                    {
                        if (dgvColumnSelector.mCheckedListBoxStrtCtrType.Items.Count > 0)
                        {
                            dgvColumnSelector.mCheckedListBoxStrtCtrType.ClientSize = new Size(dgvColumnSelector.mCheckedListBoxStrtCtrType.ClientSize.Width, dgvColumnSelector.mCheckedListBoxStrtCtrType.GetItemRectangle(0).Height * dgvColumnSelector.mCheckedListBoxStrtCtrType.Items.Count);
                            dgvColumnSelector.mPopupStrtCtrType.Height = dgvColumnSelector.mCheckedListBoxStrtCtrType.ClientSize.Height;
                            dgvColumnSelector.mPopupStrtCtrType.Width = dgvColumnSelector.mCheckedListBoxStrtCtrType.ClientSize.Width;
                            dgvColumnSelector.mPopupStrtCtrType.Show(Control.MousePosition);
                        }
                    }
                }

            }

        }

        /// <summary>
        /// Form Load event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ManualLandbaseReviewFrm_Load(object sender, EventArgs e)
        {
            string mySql = string.Empty;
         //   Recordset rsFeatName = null;
            try
            {
                chckPendingFilter.Checked = true;
                btnAccepted.Enabled = false;
                btnArchive.Enabled = false;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error during execution of Landbase Analysis custom command." + Environment.NewLine + ex.Message,
                   "Manual Landbase Review", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Form closes on Escape button click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ManualLandbaseReviewFrm_KeyDown(object sender, KeyEventArgs e)
        {
            if ((Keys)e.KeyValue == Keys.Escape)
            {
                CloseForm();
            }
        }

        /// <summary>
        /// Feature List Combobox
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cmbFeatureList_SelectedValueChanged(object sender, EventArgs e)
        {
            LoadDataGrid();
        }

    
        private void dgvManualLandbaseReview_SelectionChanged(object sender, EventArgs e)
        {
            string stage = "";
            LBMAnalysisResults lbmResults = null;
            try
            {
                if (dgvManualLandbaseReview.SelectedRows.Count > 0)
                {
                    if (SelectedFeatureClass == FeatureClass.BuildingManual)
                    {
                        var obj = (BuildingManual)dgvManualLandbaseReview.SelectedRows[0].DataBoundItem;
                        if (obj != null)
                        {
                            stage = obj.Stage;
                            lbmResults = LBMAnalysisFids.FirstOrDefault(a => a.G3eFid == obj.G3eFid);
                        }
                        highLightPath.HighlightSelectedFeatures(lbmResults);
                    }

                    if (SelectedFeatureClass == FeatureClass.ParcelManual)
                    {
                        var obj = (ParcelManual)dgvManualLandbaseReview.SelectedRows[0].DataBoundItem;
                        if (obj != null)
                        {
                            stage = obj.Stage;
                            lbmResults = LBMAnalysisFids.FirstOrDefault(a => a.G3eFid == obj.G3eFid);

                        }
                        highLightPath.HighlightSelectedFeatures(lbmResults);
                    }
                    if (SelectedFeatureClass == FeatureClass.StreetCenterlineManual)
                    {
                        var obj = (StreetCenterLine)dgvManualLandbaseReview.SelectedRows[0].DataBoundItem;
                        if (obj != null)
                        {
                            stage = obj.Stage;
                            CommonUtil.FitSelectedFeature((short)obj.G3eFno, obj.G3eFid);
                        }

                    }
                    if (SelectedFeatureClass == FeatureClass.PipelineManual)
                    {
                        var obj = (PipeLine)dgvManualLandbaseReview.SelectedRows[0].DataBoundItem;
                        if (obj != null)
                        {
                            stage = obj.Stage;
                            CommonUtil.FitSelectedFeature((short)obj.G3eFno, obj.G3eFid);
                        }

                    }


                }
                if (!string.IsNullOrEmpty(stage))
                {
                    if (stage == "Pending")
                    {
                        btnAccepted.Enabled = true;
                        btnArchive.Enabled = true;
                    }
                    else if (stage == "Accepted")
                    {
                        btnArchive.Enabled = true;
                        btnAccepted.Enabled = false;
                    }
                    else if (stage == "Archived")
                    {
                        btnArchive.Enabled = false;
                        btnAccepted.Enabled = true;
                    }
                }
                else
                {
                    btnArchive.Enabled = false;
                    btnAccepted.Enabled = false;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error during execution of LandBase Analysis custom command." + Environment.NewLine + ex.Message,
                "Manual Landbase Review", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            try
            {
                CloseForm();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error during execution of LandBase Analysis custom command." + Environment.NewLine + ex.Message,
                 "Manual Landbase Review", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            try
            {
                highLightPath.RemoveDisplayNode("Landbase Feature Data");

                GetLBMAnalysisResults();
                StreetCenterLines = GetStreetCenterLines();
                PipeLines = GetPipeLines();
                LoadDataGrid();

            }
            catch (Exception ex)
            {
                MessageBox.Show("Error while Refreshing." + Environment.NewLine + ex.Message,
                   "Manual Landbase Review", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        private void btnAccepted_Click(object sender, EventArgs e)
        {
            string stage = "Accepted";
            try
            {
                string btnText = (sender as Button).Text;
                if (dgvManualLandbaseReview.SelectedRows.Count > 0)
                {
                    gtTransactionManager.Begin("Stage Update");
                    foreach (DataGridViewRow item in dgvManualLandbaseReview.SelectedRows)
                    {
                        var obj = item.DataBoundItem;
                        if (SelectedFeatureClass == FeatureClass.BuildingManual)
                        {
                            ((BuildingManual)obj).Stage = stage;
                        }

                        if (SelectedFeatureClass == FeatureClass.ParcelManual)
                        {
                            ((ParcelManual)obj).Stage = stage;
                        }
                        if (SelectedFeatureClass == FeatureClass.StreetCenterlineManual)
                        {
                            ((StreetCenterLine)obj).Stage = stage;

                        }
                        if (SelectedFeatureClass == FeatureClass.PipelineManual)
                        {
                            ((PipeLine)obj).Stage = stage;
                        }
                        StageValueUpdate(stage, Convert.ToInt32(item.Cells["G3eFid"].Value));
                    }
                    gtTransactionManager.Commit(true);
                    LoadDataGrid();
                }
                else
                {
                    MessageBox.Show("Please select any Row to enable the Accept/Archive buttons", "Manual Landbase Review", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                btnAccepted.Enabled = false;
            }
            catch (Exception ex)
            {
                if (gtTransactionManager.TransactionInProgress)
                    gtTransactionManager.Rollback();
                MessageBox.Show("Error while Refreshing." + Environment.NewLine + ex.Message, "Manual Landbase Review", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnArchive_Click(object sender, EventArgs e)
        {
            string stage = "Archived";
            try
            {
                string btnText = (sender as Button).Text;
                if (dgvManualLandbaseReview.SelectedRows.Count > 0)
                {
                    gtTransactionManager.Begin("Stage Update");
                    foreach (DataGridViewRow item in dgvManualLandbaseReview.SelectedRows)
                    {

                        var obj = item.DataBoundItem;
                        if (SelectedFeatureClass == FeatureClass.BuildingManual)
                        {
                            ((BuildingManual)obj).Stage = stage;
                        }

                        if (SelectedFeatureClass == FeatureClass.ParcelManual)
                        {
                            ((ParcelManual)obj).Stage = stage;
                        }
                        if (SelectedFeatureClass == FeatureClass.StreetCenterlineManual)
                        {
                            ((StreetCenterLine)obj).Stage = stage;

                        }
                        if (SelectedFeatureClass == FeatureClass.PipelineManual)
                        {
                            ((PipeLine)obj).Stage = stage;
                        }
                        StageValueUpdate(stage, Convert.ToInt32(item.Cells["G3eFid"].Value));
                    }
                    gtTransactionManager.Commit(true);
                    LoadDataGrid();
                }
                else
                {
                    MessageBox.Show("Please select any Row to enable the Accept/Archive buttons", "Manual Landbase Review", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }

                btnArchive.Enabled = false;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error while Refreshing." + Environment.NewLine + ex.Message, "Manual Landbase Review", MessageBoxButtons.OK, MessageBoxIcon.Error);

            }
        }

        private void chckPendingFilter_CheckedChanged(object sender, EventArgs e)
        {
            try
            {

                if (SelectedFeatureClass == FeatureClass.BuildingManual)
                {
                    var bldgs = chckPendingFilter.Checked == true ? Buildings.Where(bldg => bldg.Stage == "Pending").ToList() : Buildings;
                    BindBuildingToGrid(bldgs);
                    dgvColumnSelector.LoadBldgFilterList(bldgs);

                }
                if (SelectedFeatureClass == FeatureClass.ParcelManual)
                {
                    var prcls = chckPendingFilter.Checked == true ? Parcels.Where(prcl => prcl.Stage == "Pending").ToList() : Parcels;
                    BindParceltoGrid(prcls);
                    dgvColumnSelector.LoadParcelFilterList(prcls);
                }

                if (chckPendingFilter.Checked == true)
                {
                    btnArchive.Enabled = true;
                    btnAccepted.Enabled = true;
                }
                else
                {
                    btnArchive.Enabled = false;
                    btnAccepted.Enabled = false;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error during execution of LandBase Analysis custom command." + Environment.NewLine + ex.Message,
                                "Manual Landbase Review", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ManualLandbaseReviewFrm_FormClosed(object sender, FormClosedEventArgs e)
        {
            try
            {
                if (e.CloseReason == CloseReason.UserClosing)
                {
                    CloseForm();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error during execution of LandBase Analysis custom command." + Environment.NewLine + ex.Message,
                 "Manual Landbase Review", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        #endregion

        #region FilterCriteria
        private void MCheckedListBoxPrimFID_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (SelectedFeatureClass == FeatureClass.ParcelManual)
            {
                FilterParcel();
            }
            if (SelectedFeatureClass == FeatureClass.BuildingManual)
            {
                FilterBuilding();
            }
            if (SelectedFeatureClass == FeatureClass.StreetCenterlineManual)
            {
                FilterStreetCntrLines();
            }
            if (SelectedFeatureClass == FeatureClass.PipelineManual)
            {
                FilterPipeLines();
            }
        }

        private void MCheckedListBoxStage_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (SelectedFeatureClass == FeatureClass.ParcelManual)
            {
                FilterParcel();
            }
            if (SelectedFeatureClass == FeatureClass.BuildingManual)
            {
                FilterBuilding();
            }
            if (SelectedFeatureClass == FeatureClass.StreetCenterlineManual)
            {
                FilterStreetCntrLines();
            }
            if (SelectedFeatureClass == FeatureClass.PipelineManual)
            {
                FilterPipeLines();
            }
        }

        private void MCheckedListBoxName_SelectedIndexChanged(object sender, EventArgs e)
        {
            FilterBuilding();
        }

        private void MCheckedListBoxSectionDescription_SelectedIndexChanged(object sender, EventArgs e)
        {
            FilterParcel();

        }

        private void MCheckedListBoxAddress_SelectedIndexChanged(object sender, EventArgs e)
        {
            FilterParcel();
        }

        private void MCheckedListBoxLotNumber_SelectedIndexChanged(object sender, EventArgs e)
        {
            FilterParcel();
        }

        private void MCheckedListBoxBlockNumber_SelectedIndexChanged(object sender, EventArgs e)
        {
            FilterParcel();
        }

        private void MCheckedListBoxAdditionName_SelectedIndexChanged(object sender, EventArgs e)
        {
            FilterParcel();
        }


        private void MCheckedListBoxLocation_SelectedIndexChanged(object sender, EventArgs e)
        {
            FilterBuilding();
        }
        private void MCheckedListBoxAbstract_SelectedIndexChanged(object sender, EventArgs e)
        {
            FilterParcel();
        }

        private void MCheckedListBoxPipelnMlName_SelectedIndexChanged(object sender, EventArgs e)
        {
            FilterStreetCntrLines();
        }

        private void MCheckedListBoxWidth_SelectedIndexChanged(object sender, EventArgs e)
        {
            FilterStreetCntrLines();
        }

        private void MCheckedListBoxMaterial_SelectedIndexChanged(object sender, EventArgs e)
        {
            FilterStreetCntrLines();
        }

        private void MCheckedListBoxStrtCtrType_SelectedIndexChanged(object sender, EventArgs e)
        {
            FilterStreetCntrLines();
        }

        private void MCheckedListBoxOwner_SelectedIndexChanged(object sender, EventArgs e)
        {
            FilterStreetCntrLines();
        }

        private void MCheckedListBoxStrtCtrName_SelectedIndexChanged(object sender, EventArgs e)
        {
            FilterStreetCntrLines();
        }

        private void MCheckedListBoxAddressRange_SelectedIndexChanged(object sender, EventArgs e)
        {
            FilterPipeLines();
        }

        private void MCheckedListBoxType_SelectedIndexChanged(object sender, EventArgs e)
        {
            FilterPipeLines();
        }

        private void MCheckedListBoxPretype_SelectedIndexChanged(object sender, EventArgs e)
        {
            FilterPipeLines();
        }

        private void MCheckedListBoxSuffix_SelectedIndexChanged(object sender, EventArgs e)
        {
            FilterPipeLines();
        }

        private void MCheckedListBoxPrefix_SelectedIndexChanged(object sender, EventArgs e)
        {
            FilterPipeLines();
        }



        private void FilterBuilding()
        {

            List<BuildingManual> bldgs = this.Buildings;
            List<object> filter = new List<object>();
            if (dgvColumnSelector.mCheckedListBoxPrimFID.CheckedItems.Count > 0)
            {
                filter = new List<object>();
                foreach (var item in dgvColumnSelector.mCheckedListBoxPrimFID.CheckedItems)
                {
                    filter.Add(item);
                }

                var bldgFltr = from b in bldgs
                               join f in filter on b.G3eFid equals f
                               select b;
                if (bldgFltr != null)
                {
                    bldgs = bldgFltr.ToList<BuildingManual>();
                }
            }
            if (dgvColumnSelector.mCheckedListBoxStage.CheckedItems.Count > 0)
            {
                filter = new List<object>();
                foreach (var item in dgvColumnSelector.mCheckedListBoxStage.CheckedItems)
                {
                    filter.Add(Convert.ToString(item));
                }

                var bldgFltr = from b in bldgs
                               join f in filter on b.Stage equals f
                               select b;
                if (bldgFltr != null)
                {
                    bldgs = bldgFltr.ToList<BuildingManual>();
                }
            }
            if (dgvColumnSelector.mCheckedListBoxName.CheckedItems.Count > 0)
            {
                filter = new List<object>();
                foreach (var item in dgvColumnSelector.mCheckedListBoxName.CheckedItems)
                {
                    filter.Add(Convert.ToString(item));
                }

                var bldgFltr = from b in bldgs
                               join f in filter on b.Name equals f
                               select b;
                if (bldgFltr != null)
                {
                    bldgs = bldgFltr.ToList<BuildingManual>();
                }
            }
            if (dgvColumnSelector.mCheckedListBoxLocation.CheckedItems.Count > 0)
            {
                filter = new List<object>();
                foreach (var item in dgvColumnSelector.mCheckedListBoxLocation.CheckedItems)
                {
                    filter.Add(Convert.ToString(item));
                }

                var bldgFltr = from b in bldgs
                               join f in filter on b.Location equals f
                               select b;
                if (bldgFltr != null)
                {
                    bldgs = bldgFltr.ToList<BuildingManual>();
                }
            }

            if (bldgs != null)
            {
                this.dgvManualLandbaseReview.DataSource = bldgs;
            }
            else
            {
                this.dgvManualLandbaseReview.DataSource = this.Buildings;
            }
        }

        private void FilterParcel()
        {
            List<ParcelManual> prcls = this.Parcels;
            List<object> filter = new List<object>();
            if (dgvColumnSelector.mCheckedListBoxPrimFID.CheckedItems.Count > 0)
            {
                filter = new List<object>();
                foreach (var item in dgvColumnSelector.mCheckedListBoxPrimFID.CheckedItems)
                {
                    filter.Add(item);
                }

                var prclFltr = from b in prcls
                               join f in filter on b.G3eFid equals f
                               select b;
                if (prclFltr != null)
                {
                    prcls = prclFltr.ToList<ParcelManual>();
                }
            }
            if (dgvColumnSelector.mCheckedListBoxStage.CheckedItems.Count > 0)
            {
                filter = new List<object>();
                foreach (var item in dgvColumnSelector.mCheckedListBoxStage.CheckedItems)
                {
                    filter.Add(Convert.ToString(item));
                }

                var prclFltr = from b in prcls
                               join f in filter on b.Stage equals f
                               select b;
                if (prclFltr != null)
                {
                    prcls = prclFltr.ToList<ParcelManual>();
                }
            }
            if (dgvColumnSelector.mCheckedListBoxLotNumber.CheckedItems.Count > 0)
            {
                filter = new List<object>();
                foreach (var item in dgvColumnSelector.mCheckedListBoxLotNumber.CheckedItems)
                {
                    filter.Add(Convert.ToString(item));
                }

                var prclFltr = from b in prcls
                               join f in filter on b.LotNumber equals f
                               select b;
                if (prclFltr != null)
                {
                    prcls = prclFltr.ToList<ParcelManual>();
                }
            }

            if (dgvColumnSelector.mCheckedListBoxSectionDescription.CheckedItems.Count > 0)
            {
                filter = new List<object>();
                foreach (var item in dgvColumnSelector.mCheckedListBoxSectionDescription.CheckedItems)
                {
                    filter.Add(Convert.ToString(item));
                }

                var prclFltr = from b in prcls
                               join f in filter on b.SectionDescription equals f
                               select b;
                if (prclFltr != null)
                {
                    prcls = prclFltr.ToList<ParcelManual>();
                }
            }

            if (dgvColumnSelector.mCheckedListBoxAbstract.CheckedItems.Count > 0)
            {
                filter = new List<object>();
                foreach (var item in dgvColumnSelector.mCheckedListBoxAbstract.CheckedItems)
                {
                    filter.Add(Convert.ToString(item));
                }

                var prclFltr = from b in prcls
                               join f in filter on b.ParcelMLAbstract equals f
                               select b;
                if (prclFltr != null)
                {
                    prcls = prclFltr.ToList<ParcelManual>();
                }
            }

            if (dgvColumnSelector.mCheckedListBoxAddress.CheckedItems.Count > 0)
            {
                filter = new List<object>();
                foreach (var item in dgvColumnSelector.mCheckedListBoxAddress.CheckedItems)
                {
                    filter.Add(Convert.ToString(item));
                }

                var prclFltr = from b in prcls
                               join f in filter on b.Address equals f
                               select b;
                if (prclFltr != null)
                {
                    prcls = prclFltr.ToList<ParcelManual>();
                }
            }

            if (dgvColumnSelector.mCheckedListBoxAdditionName.CheckedItems.Count > 0)
            {
                filter = new List<object>();
                foreach (var item in dgvColumnSelector.mCheckedListBoxAdditionName.CheckedItems)
                {
                    filter.Add(Convert.ToString(item));
                }

                var prclFltr = from b in prcls
                               join f in filter on b.AdditionName equals f
                               select b;
                if (prclFltr != null)
                {
                    prcls = prclFltr.ToList<ParcelManual>();
                }
            }

            if (dgvColumnSelector.mCheckedListBoxBlockNumber.CheckedItems.Count > 0)
            {
                filter = new List<object>();
                foreach (var item in dgvColumnSelector.mCheckedListBoxBlockNumber.CheckedItems)
                {
                    filter.Add(Convert.ToString(item));
                }

                var prclFltr = from b in prcls
                               join f in filter on b.BlockNumber equals f
                               select b;
                if (prclFltr != null)
                {
                    prcls = prclFltr.ToList<ParcelManual>();
                }
            }

            if (prcls != null)
            {
                this.dgvManualLandbaseReview.DataSource = prcls;
            }
            else
            {
                this.dgvManualLandbaseReview.DataSource = this.Parcels;
            }
        }

        private void FilterStreetCntrLines()
        {
            List<StreetCenterLine> stltCntrLines = this.StreetCenterLines;
            List<object> filter = new List<object>();
            if (dgvColumnSelector.mCheckedListBoxPrimFID.CheckedItems.Count > 0)
            {
                filter = new List<object>();
                foreach (var item in dgvColumnSelector.mCheckedListBoxPrimFID.CheckedItems)
                {
                    filter.Add(item);
                }

                var prclFltr = from b in stltCntrLines
                               join f in filter on b.G3eFid equals f
                               select b;
                if (prclFltr != null)
                {
                    stltCntrLines = prclFltr.ToList<StreetCenterLine>();
                }
            }
            if (dgvColumnSelector.mCheckedListBoxStage.CheckedItems.Count > 0)
            {
                filter = new List<object>();
                foreach (var item in dgvColumnSelector.mCheckedListBoxStage.CheckedItems)
                {
                    filter.Add(Convert.ToString(item));
                }

                var stltCntrFltr = from b in stltCntrLines
                                   join f in filter on b.Stage equals f
                                   select b;
                if (stltCntrFltr != null)
                {
                    stltCntrLines = stltCntrFltr.ToList<StreetCenterLine>();
                }
            }

            if (dgvColumnSelector.mCheckedListBoxStrtCtrName.CheckedItems.Count > 0)
            {
                filter = new List<object>();
                foreach (var item in dgvColumnSelector.mCheckedListBoxStrtCtrName.CheckedItems)
                {
                    filter.Add(Convert.ToString(item));
                }

                var stltCntrFltr = from b in stltCntrLines
                                   join f in filter on b.Name equals f
                                   select b;
                if (stltCntrFltr != null)
                {
                    stltCntrLines = stltCntrFltr.ToList<StreetCenterLine>();
                }
            }
            if (dgvColumnSelector.mCheckedListBoxMaterial.CheckedItems.Count > 0)
            {
                filter = new List<object>();
                foreach (var item in dgvColumnSelector.mCheckedListBoxMaterial.CheckedItems)
                {
                    filter.Add(Convert.ToString(item));
                }

                var stltCntrFltr = from b in stltCntrLines
                                   join f in filter on b.Material equals f
                                   select b;
                if (stltCntrFltr != null)
                {
                    stltCntrLines = stltCntrFltr.ToList<StreetCenterLine>();
                }
            }

            if (dgvColumnSelector.mCheckedListBoxOwner.CheckedItems.Count > 0)
            {
                filter = new List<object>();
                foreach (var item in dgvColumnSelector.mCheckedListBoxOwner.CheckedItems)
                {
                    filter.Add(Convert.ToString(item));
                }

                var stltCntrFltr = from b in stltCntrLines
                                   join f in filter on b.Owner equals f
                                   select b;
                if (stltCntrFltr != null)
                {
                    stltCntrLines = stltCntrFltr.ToList<StreetCenterLine>();
                }
            }

            if (dgvColumnSelector.mCheckedListBoxWidth.CheckedItems.Count > 0)
            {
                filter = new List<object>();
                foreach (var item in dgvColumnSelector.mCheckedListBoxWidth.CheckedItems)
                {
                    filter.Add(Convert.ToString(item));
                }

                var stltCntrFltr = from b in stltCntrLines
                                   join f in filter on b.Width equals f
                                   select b;
                if (stltCntrFltr != null)
                {
                    stltCntrLines = stltCntrFltr.ToList<StreetCenterLine>();
                }
            }

            if (dgvColumnSelector.mCheckedListBoxStrtCtrType.CheckedItems.Count > 0)
            {
                filter = new List<object>();
                foreach (var item in dgvColumnSelector.mCheckedListBoxStrtCtrType.CheckedItems)
                {
                    filter.Add(Convert.ToString(item));
                }

                var stltCntrFltr = from b in stltCntrLines
                                   join f in filter on b.Type equals f
                                   select b;
                if (stltCntrFltr != null)
                {
                    stltCntrLines = stltCntrFltr.ToList<StreetCenterLine>();
                }
            }


            if (stltCntrLines != null)
            {
                this.dgvManualLandbaseReview.DataSource = stltCntrLines;
            }
            else
            {
                this.dgvManualLandbaseReview.DataSource = this.Parcels;
            }
        }

        private void FilterPipeLines()
        {
            List<PipeLine> pipeLines = this.PipeLines;
            List<object> filter = new List<object>();
            if (dgvColumnSelector.mCheckedListBoxPrimFID.CheckedItems.Count > 0)
            {
                filter = new List<object>();
                foreach (var item in dgvColumnSelector.mCheckedListBoxPrimFID.CheckedItems)
                {
                    filter.Add(item);
                }

                var pplnFltr = from b in pipeLines
                               join f in filter on b.G3eFid equals f
                               select b;
                if (pplnFltr != null)
                {
                    pipeLines = pplnFltr.ToList<PipeLine>();
                }
            }
            if (dgvColumnSelector.mCheckedListBoxStage.CheckedItems.Count > 0)
            {
                filter = new List<object>();
                foreach (var item in dgvColumnSelector.mCheckedListBoxStage.CheckedItems)
                {
                    filter.Add(Convert.ToString(item));
                }

                var pplnFltr = from b in pipeLines
                               join f in filter on b.Stage equals f
                               select b;
                if (pplnFltr != null)
                {
                    pipeLines = pplnFltr.ToList<PipeLine>();
                }
            }

            if (dgvColumnSelector.mCheckedListBoxPipelnMlName.CheckedItems.Count > 0)
            {
                filter = new List<object>();
                foreach (var item in dgvColumnSelector.mCheckedListBoxPipelnMlName.CheckedItems)
                {
                    filter.Add(Convert.ToString(item));
                }

                var pplnFltr = from b in pipeLines
                               join f in filter on b.Name equals f
                               select b;
                if (pplnFltr != null)
                {
                    pipeLines = pplnFltr.ToList<PipeLine>();
                }
            }


            if (dgvColumnSelector.mCheckedListBoxAddressRange.CheckedItems.Count > 0)
            {
                filter = new List<object>();
                foreach (var item in dgvColumnSelector.mCheckedListBoxAddressRange.CheckedItems)
                {
                    filter.Add(Convert.ToString(item));
                }

                var pplnFltr = from b in pipeLines
                               join f in filter on b.AddressRange equals f
                               select b;
                if (pplnFltr != null)
                {
                    pipeLines = pplnFltr.ToList<PipeLine>();
                }
            }

            if (dgvColumnSelector.mCheckedListBoxPrefix.CheckedItems.Count > 0)
            {
                filter = new List<object>();
                foreach (var item in dgvColumnSelector.mCheckedListBoxPrefix.CheckedItems)
                {
                    filter.Add(Convert.ToString(item));
                }

                var pplnFltr = from b in pipeLines
                               join f in filter on b.Prefix equals f
                               select b;
                if (pplnFltr != null)
                {
                    pipeLines = pplnFltr.ToList<PipeLine>();
                }
            }

            if (dgvColumnSelector.mCheckedListBoxPretype.CheckedItems.Count > 0)
            {
                filter = new List<object>();
                foreach (var item in dgvColumnSelector.mCheckedListBoxPretype.CheckedItems)
                {
                    filter.Add(Convert.ToString(item));
                }

                var pplnFltr = from b in pipeLines
                               join f in filter on b.PreType equals f
                               select b;
                if (pplnFltr != null)
                {
                    pipeLines = pplnFltr.ToList<PipeLine>();
                }
            }

            if (dgvColumnSelector.mCheckedListBoxSuffix.CheckedItems.Count > 0)
            {
                filter = new List<object>();
                foreach (var item in dgvColumnSelector.mCheckedListBoxSuffix.CheckedItems)
                {
                    filter.Add(Convert.ToString(item));
                }

                var pplnFltr = from b in pipeLines
                               join f in filter on b.Suffix equals f
                               select b;
                if (pplnFltr != null)
                {
                    pipeLines = pplnFltr.ToList<PipeLine>();
                }
            }

            if (dgvColumnSelector.mCheckedListBoxType.CheckedItems.Count > 0)
            {
                filter = new List<object>();
                foreach (var item in dgvColumnSelector.mCheckedListBoxType.CheckedItems)
                {
                    filter.Add(Convert.ToString(item));
                }

                var pplnFltr = from b in pipeLines
                               join f in filter on b.Type equals f
                               select b;
                if (pplnFltr != null)
                {
                    pipeLines = pplnFltr.ToList<PipeLine>();
                }
            }
            if (pipeLines != null)
            {
                this.dgvManualLandbaseReview.DataSource = pipeLines;
            }
            else
            {
                this.dgvManualLandbaseReview.DataSource = this.PipeLines;
            }
        }

        #endregion
              
    }
}


