// ===================================================
//  Copyright 2017 Intergraph Corp.
//  File Name: ccTreeTrimRequestForm.cs
//
//  Description:   Custom Form for TreeTrimRequest command.
//  Remarks: 
//  Modification History
//  Date                Author                      Comments
//  --------            --------------              --------
//  15/03/2018          Prathyusha                  Created 
//  11/03/2019          Akhilesh                    Removed Event to ignore Spaces for Special Instructions. - ALM 1903
// ======================================================
using System;
using ADODB;
using Intergraph.GTechnology.API;
using System.IO;
using System.Windows.Forms;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace GTechnology.Oncor.CustomAPI
{
    public partial class ccTreeTrimRequestForm : Form
    {
        #region Variables

        Recordset m_oWorkPointVoucherRS = null;
        IGTApplication m_oGTApp = null;
        string[] m_oOracleUserData = null;
        string m_oVegManagementSheetTemplate = null;
        bool m_oCommandExit = false;
        bool m_oProcessDone = false;
        IGTKeyObject m_oGTTreeTrimmingFeature = null;
        string m_oPlotPDFName = null;
        DataAccess dataAccess = null;

        #endregion

        #region Constructor
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="p_voucherRS">Work Point Vouchers Recordset</param>
        /// <param name="p_application">The current G/Technology application object.</param>
        /// <param name="p_newTreeTrimmingfeature">Current placed feature</param>
        public ccTreeTrimRequestForm(Recordset p_voucherRS, IGTApplication p_application, IGTKeyObject p_newTreeTrimmingfeature)
        {
            InitializeComponent();
            m_oWorkPointVoucherRS = p_voucherRS;
            this.m_oGTApp = p_application;
            this.m_oGTTreeTrimmingFeature = p_newTreeTrimmingfeature;
        }
        #endregion

        #region Properties
        public bool CommandExit
        {
            get
            {
                return m_oCommandExit;
            }
        }
        public bool ProcessDone
        {
            get
            {
                return m_oProcessDone;
            }
        }
        public string RecipientName
        {
            get
            {
                return txtRecipientName.Text;
            }
        }
        public string RecipientEmail
        {
            get
            {
                return txtRecipientEmail.Text;
            }
        }
        public string VoucherAccountVal
        {
            get
            {
                return txtVoucherAccnt.Text;
            }
        }
        public string VegMngSheetTemplate
        {
            get
            {
                return m_oVegManagementSheetTemplate;
            }
        }
        public string PlotPDFName
        {
            get
            {
                return m_oPlotPDFName;
            }
        }
        #endregion

        #region Event Handlers
        private void btnOK_Click(object sender, EventArgs e)
        {
            try
            {
                if (ValidateControls())
                {
                    if (CreateVegetationManagementSheetCopy())
                    {
                        if (!String.IsNullOrEmpty(m_oVegManagementSheetTemplate))
                        {
                            SubstituteFormValuesToVegMgmtEstimateSheet();
                        }
                        if (chkGeneratePlot.Checked)
                        {
                            GeneratePlotPDF generatePlotPDF = new GeneratePlotPDF(m_oGTApp);
                            generatePlotPDF.CreateVegetationManagementPlotTemplateCopy(m_oVegManagementSheetTemplate, m_oGTTreeTrimmingFeature);
                            m_oPlotPDFName = generatePlotPDF.GeneratedPlotPDF;
                            m_oCommandExit = generatePlotPDF.CancelRequest;
                            if (!m_oCommandExit)
                            {
                                m_oProcessDone = true;
                            }
                        }
                        else
                        {
                            m_oProcessDone = true;
                        }
                        Close();
                    }
                    else
                    {
                        Close();
                    }
                }
            }
            catch(Exception ex)
            {
                m_oCommandExit = true;
                MessageBox.Show("Error in TreeTrim Request Estimate command: " + ex.Message, "G/Technology", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Close();
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            dataAccess = null;
            m_oCommandExit = true;
            Close();
        }

        private void ccTreeTrimRequestForm_Load(object sender, EventArgs e)
        {
            m_oOracleUserData = GetRACFID();
            txtFrom.Text = m_oOracleUserData[0];
            dtpEstResponseDate.Value = dtpTodayDate.Value.AddDays(5);
            dtpClearingReqDate.CustomFormat = " ";
            dtpClearingReqDate.Format = DateTimePickerFormat.Custom;
            rbOvertimeChargesNo.Checked = true;
            rbPropertyOwnersNo.Checked = true;
            lblWRNumVal.Text = m_oGTApp.DataContext.ActiveJob;
            m_oGTApp.SetStatusBarText(GTStatusPanelConstants.gtaspcMessage, "Select the Prime and Sub Account for the voucher using Voucher Account selection button");
        }

        private void chkCIACCollecYes_CheckStateChanged(object sender, EventArgs e)
        {
            if (chkCIACCollecYes.Checked && chkCIACCollecNo.Checked)
            {
                chkCIACCollecNo.Checked = false;
            }
        }

        private void chkCIACCollecNo_CheckStateChanged(object sender, EventArgs e)
        {
            if (chkCIACCollecYes.Checked && chkCIACCollecNo.Checked)
            {
                chkCIACCollecYes.Checked = false;
            }
        }

        private void chkClearInspecYes_CheckStateChanged(object sender, EventArgs e)
        {
            if (chkClearInspecYes.Checked && chkClearInspecNo.Checked)
            {
                chkClearInspecNo.Checked = false;
            }
        }

        private void chkClearInspecNo_CheckStateChanged(object sender, EventArgs e)
        {
            if (chkClearInspecYes.Checked && chkClearInspecNo.Checked)
            {
                chkClearInspecYes.Checked = false;
            }
        }

        private void dtpClearingReqDate_ValueChanged(object sender, EventArgs e)
        {
            dtpClearingReqDate.Format = DateTimePickerFormat.Long;
        }

        private void rbOvertimeChargesYes_CheckedChanged(object sender, EventArgs e)
        {
            if (rbOvertimeChargesYes.Checked)
            {
                rbOvertimeChargesNo.Checked = false;
                lblAuthorizingSignVal.Text = m_oOracleUserData[0] + " - " + m_oOracleUserData[1];
            }
        }
        private void btnVouchersAccnt_Click(object sender, EventArgs e)
        {
            try
            {
                VoucherFERCAccountForm voucherFERCAccountForm = new VoucherFERCAccountForm(m_oWorkPointVoucherRS);
                voucherFERCAccountForm.ShowDialog(this);

                if (voucherFERCAccountForm.AccountValue != null)
                {
                    txtVoucherAccnt.Text = voucherFERCAccountForm.AccountValue;
                }

                m_oGTApp.SetStatusBarText(GTStatusPanelConstants.gtaspcMessage, "Enter information for inclusion in the Vegetation Management WR Activity Sheet.Also Specify whether to also generate a plot to accompany the activity sheet");
            }
            catch
            {
                m_oCommandExit = true;
                throw;
            }
        }
        private void ccTreeTrimRequestForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            dataAccess = null;
            m_oCommandExit = true;
        }
        #endregion

        #region Methods
        /// <summary>
        /// Method to Substitute the Form Values To VegetationManagement Estimate Sheet.
        /// </summary>
        private void SubstituteFormValuesToVegMgmtEstimateSheet()
        {
            List<string> checkBoxImages = new List<string>();
            string originalSource = null;
            string replacedSource = null;
            try
            {
                StreamReader sr = new StreamReader(m_oVegManagementSheetTemplate);
                string activitySheetData = sr.ReadToEnd();
                activitySheetData = activitySheetData.Replace("VegetationManagementEstimate_Sheet/", "");
                activitySheetData = activitySheetData.Replace("[[TO]]", txtRecipientName.Text);
                activitySheetData = activitySheetData.Replace("[[FROM]]", txtFrom.Text);
                activitySheetData = activitySheetData.Replace("[[PHONE]]", txtPhoneNbr.Text);
                activitySheetData = activitySheetData.Replace("[[TODAYSDATE]]", Convert.ToString(dtpTodayDate.Value));
                activitySheetData = activitySheetData.Replace("[[CLEARINGREQDATE]]", Convert.ToString(dtpClearingReqDate.Value));
                activitySheetData = activitySheetData.Replace("[[WRPROJECTNUMBER]]", m_oGTApp.DataContext.ActiveJob);
                activitySheetData = activitySheetData.Replace("[[LOCATION/DIRECTIONS]]", txtLocations.Text);
                activitySheetData = activitySheetData.Replace("[[PMDSESTVMCOST]]", txtDesignerEstVMCost.Text);
                activitySheetData = activitySheetData.Replace("[[AuthorizingSignature]]", lblAuthorizingSignVal.Text);
                activitySheetData = activitySheetData.Replace("[[JOBSCOPEDESIGNER]]", txtJobScopeDesigner.Text);
                activitySheetData = activitySheetData.Replace("[[SUPPLEMENTARYROWWIDTHINFO]]", txtSupplyROWWidthInfo.Text);

                checkBoxImages = GetImagesInHTMLString(activitySheetData);

                if (checkBoxImages.Count > 0)
                {
                    if (rbEstimate.Checked)
                    {
                        originalSource = checkBoxImages.Find(a => a.Contains("Check_EstimateYES"));
                        replacedSource = originalSource.Replace("VegetationManagementEstimate_Sheet_Unchecked_Image.png", "VegetationManagementEstimate_Sheet_Checked_Image.png");
                        activitySheetData = activitySheetData.Replace(originalSource, replacedSource);

                        originalSource = checkBoxImages.Find(a => a.Contains("Check_EstimateNO"));
                        replacedSource = originalSource.Replace("VegetationManagementEstimate_Sheet_Checked_Image.png", "VegetationManagementEstimate_Sheet_Unchecked_Image.png");
                        activitySheetData = activitySheetData.Replace(originalSource, replacedSource);
                    }
                    else
                    {
                        originalSource = checkBoxImages.Find(a => a.Contains("Check_EstimateNO"));
                        replacedSource = originalSource.Replace("VegetationManagementEstimate_Sheet_Unchecked_Image.png", "VegetationManagementEstimate_Sheet_Checked_Image.png");
                        activitySheetData = activitySheetData.Replace(originalSource, replacedSource);

                        originalSource = checkBoxImages.Find(a => a.Contains("Check_EstimateYES"));
                        replacedSource = originalSource.Replace("VegetationManagementEstimate_Sheet_Checked_Image.png", "VegetationManagementEstimate_Sheet_Unchecked_Image.png");
                        activitySheetData = activitySheetData.Replace(originalSource, replacedSource);
                    }
                    if (rbFirmPrice.Checked)
                    {
                        originalSource = checkBoxImages.Find(a => a.Contains("Check_FirmPriceYES"));
                        replacedSource = originalSource.Replace("VegetationManagementEstimate_Sheet_Unchecked_Image.png", "VegetationManagementEstimate_Sheet_Checked_Image.png");
                        activitySheetData = activitySheetData.Replace(originalSource, replacedSource);

                        originalSource = checkBoxImages.Find(a => a.Contains("Check_FirmPriceNO"));
                        replacedSource = originalSource.Replace("VegetationManagementEstimate_Sheet_Checked_Image.png", "VegetationManagementEstimate_Sheet_Unchecked_Image.png");
                        activitySheetData = activitySheetData.Replace(originalSource, replacedSource);
                    }
                    else
                    {
                        originalSource = checkBoxImages.Find(a => a.Contains("Check_FirmPriceNO"));
                        replacedSource = originalSource.Replace("VegetationManagementEstimate_Sheet_Unchecked_Image.png", "VegetationManagementEstimate_Sheet_Checked_Image.png");
                        activitySheetData = activitySheetData.Replace(originalSource, replacedSource);

                        originalSource = checkBoxImages.Find(a => a.Contains("Check_EstimateYES"));
                        replacedSource = originalSource.Replace("VegetationManagementEstimate_Sheet_Checked_Image.png", "VegetationManagementEstimate_Sheet_Unchecked_Image.png");
                        activitySheetData = activitySheetData.Replace(originalSource, replacedSource);
                    }
                    if (chkCIACCollecYes.Checked)
                    {
                        originalSource = checkBoxImages.Find(a => a.Contains("Check_CIACCollectibleYES"));
                        replacedSource = originalSource.Replace("VegetationManagementEstimate_Sheet_Unchecked_Image.png", "VegetationManagementEstimate_Sheet_Checked_Image.png");
                        activitySheetData = activitySheetData.Replace(originalSource, replacedSource);

                        originalSource = checkBoxImages.Find(a => a.Contains("Check_CIACCollectibleNO"));
                        replacedSource = originalSource.Replace("VegetationManagementEstimate_Sheet_Checked_Image.png", "VegetationManagementEstimate_Sheet_Unchecked_Image.png");
                        activitySheetData = activitySheetData.Replace(originalSource, replacedSource);
                    }
                    else if (chkCIACCollecNo.Checked)
                    {
                        originalSource = checkBoxImages.Find(a => a.Contains("Check_CIACCollectibleNO"));
                        replacedSource = originalSource.Replace("VegetationManagementEstimate_Sheet_Unchecked_Image.png", "VegetationManagementEstimate_Sheet_Checked_Image.png");
                        activitySheetData = activitySheetData.Replace(originalSource, replacedSource);

                        originalSource = checkBoxImages.Find(a => a.Contains("Check_CIACCollectibleYES"));
                        replacedSource = originalSource.Replace("VegetationManagementEstimate_Sheet_Checked_Image.png", "VegetationManagementEstimate_Sheet_Unchecked_Image.png");
                        activitySheetData = activitySheetData.Replace(originalSource, replacedSource);
                    }
                    if (chkClearInspecYes.Checked)
                    {
                        originalSource = checkBoxImages.Find(a => a.Contains("Check_CUSTClearInspectionYES"));
                        replacedSource = originalSource.Replace("VegetationManagementEstimate_Sheet_Unchecked_Image.png", "VegetationManagementEstimate_Sheet_Checked_Image.png");
                        activitySheetData = activitySheetData.Replace(originalSource, replacedSource);

                        originalSource = checkBoxImages.Find(a => a.Contains("Check_CUSTClearInspectionNO"));
                        replacedSource = originalSource.Replace("VegetationManagementEstimate_Sheet_Checked_Image.png", "VegetationManagementEstimate_Sheet_Unchecked_Image.png");
                        activitySheetData = activitySheetData.Replace(originalSource, replacedSource);
                    }
                    else if (chkClearInspecNo.Checked)
                    {
                        originalSource = checkBoxImages.Find(a => a.Contains("Check_CUSTClearInspectionNO"));
                        replacedSource = originalSource.Replace("VegetationManagementEstimate_Sheet_Unchecked_Image.png", "VegetationManagementEstimate_Sheet_Checked_Image.png");
                        activitySheetData = activitySheetData.Replace(originalSource, replacedSource);

                        originalSource = checkBoxImages.Find(a => a.Contains("Check_CUSTClearInspectionYES"));
                        replacedSource = originalSource.Replace("VegetationManagementEstimate_Sheet_Checked_Image.png", "VegetationManagementEstimate_Sheet_Unchecked_Image.png");
                        activitySheetData = activitySheetData.Replace(originalSource, replacedSource);
                    }
                    if (chkEnvironmentalConcerns.Checked)
                    {
                        originalSource = checkBoxImages.Find(a => a.Contains("Check_EnvironmentalConcerns"));
                        replacedSource = originalSource.Replace("VegetationManagementEstimate_Sheet_Unchecked_Image.png", "VegetationManagementEstimate_Sheet_Checked_Image.png");
                        activitySheetData = activitySheetData.Replace(originalSource, replacedSource);
                    }
                    else
                    {
                        originalSource = checkBoxImages.Find(a => a.Contains("Check_EnvironmentalConcerns"));
                        replacedSource = originalSource.Replace("VegetationManagementEstimate_Sheet_Checked_Image.png", "VegetationManagementEstimate_Sheet_Unchecked_Image.png");
                        activitySheetData = activitySheetData.Replace(originalSource, replacedSource);
                    }
                    if (chkSpecialPrecautions.Checked)
                    {
                        originalSource = checkBoxImages.Find(a => a.Contains("Check_SpecialPrecautions"));
                        replacedSource = originalSource.Replace("VegetationManagementEstimate_Sheet_Unchecked_Image.png", "VegetationManagementEstimate_Sheet_Checked_Image.png");
                        activitySheetData = activitySheetData.Replace(originalSource, replacedSource);
                    }
                    else
                    {
                        originalSource = checkBoxImages.Find(a => a.Contains("Check_SpecialPrecautions"));
                        replacedSource = originalSource.Replace("VegetationManagementEstimate_Sheet_Checked_Image.png", "VegetationManagementEstimate_Sheet_Unchecked_Image.png");
                        activitySheetData = activitySheetData.Replace(originalSource, replacedSource);
                    }
                    if (chkContactEnvironServices.Checked)
                    {
                        originalSource = checkBoxImages.Find(a => a.Contains("Check_ContactEnvironmentalServices"));
                        replacedSource = originalSource.Replace("VegetationManagementEstimate_Sheet_Unchecked_Image.png", "VegetationManagementEstimate_Sheet_Checked_Image.png");
                        activitySheetData = activitySheetData.Replace(originalSource, replacedSource);
                    }
                    else
                    {
                        originalSource = checkBoxImages.Find(a => a.Contains("Check_ContactEnvironmentalServices"));
                        replacedSource = originalSource.Replace("VegetationManagementEstimate_Sheet_Checked_Image.png", "VegetationManagementEstimate_Sheet_Unchecked_Image.png");
                        activitySheetData = activitySheetData.Replace(originalSource, replacedSource);
                    }
                    if (chkPoliticalConcerns.Checked)
                    {
                        originalSource = checkBoxImages.Find(a => a.Contains("Check_PoliticalConcerns"));
                        replacedSource = originalSource.Replace("VegetationManagementEstimate_Sheet_Unchecked_Image.png", "VegetationManagementEstimate_Sheet_Checked_Image.png");
                        activitySheetData = activitySheetData.Replace(originalSource, replacedSource);
                    }
                    else
                    {
                        originalSource = checkBoxImages.Find(a => a.Contains("Check_PoliticalConcerns"));
                        replacedSource = originalSource.Replace("VegetationManagementEstimate_Sheet_Checked_Image.png", "VegetationManagementEstimate_Sheet_Unchecked_Image.png");
                        activitySheetData = activitySheetData.Replace(originalSource, replacedSource);
                    }
                    if (chkApprovedCharges.Checked)
                    {
                        originalSource = checkBoxImages.Find(a => a.Contains("Check_ApprovedForCharges"));
                        replacedSource = originalSource.Replace("VegetationManagementEstimate_Sheet_Unchecked_Image.png", "VegetationManagementEstimate_Sheet_Checked_Image.png");
                        activitySheetData = activitySheetData.Replace(originalSource, replacedSource);
                    }
                    else
                    {
                        originalSource = checkBoxImages.Find(a => a.Contains("Check_ApprovedForCharges"));
                        replacedSource = originalSource.Replace("VegetationManagementEstimate_Sheet_Checked_Image.png", "VegetationManagementEstimate_Sheet_Unchecked_Image.png");
                        activitySheetData = activitySheetData.Replace(originalSource, replacedSource);
                    }
                    if (chkLocalPermits.Checked)
                    {
                        originalSource = checkBoxImages.Find(a => a.Contains("Check_LocalPermitsSecured"));
                        replacedSource = originalSource.Replace("VegetationManagementEstimate_Sheet_Unchecked_Image.png", "VegetationManagementEstimate_Sheet_Checked_Image.png");
                        activitySheetData = activitySheetData.Replace(originalSource, replacedSource);
                    }
                    else
                    {
                        originalSource = checkBoxImages.Find(a => a.Contains("Check_LocalPermitsSecured"));
                        replacedSource = originalSource.Replace("VegetationManagementEstimate_Sheet_Checked_Image.png", "VegetationManagementEstimate_Sheet_Unchecked_Image.png");
                        activitySheetData = activitySheetData.Replace(originalSource, replacedSource);
                    }
                    if (chkEasementsSecured.Checked)
                    {
                        originalSource = checkBoxImages.Find(a => a.Contains("Check_EasementsSecured"));
                        replacedSource = originalSource.Replace("VegetationManagementEstimate_Sheet_Unchecked_Image.png", "VegetationManagementEstimate_Sheet_Checked_Image.png");
                        activitySheetData = activitySheetData.Replace(originalSource, replacedSource);
                    }
                    else
                    {
                        originalSource = checkBoxImages.Find(a => a.Contains("Check_EasementsSecured"));
                        replacedSource = originalSource.Replace("VegetationManagementEstimate_Sheet_Checked_Image.png", "VegetationManagementEstimate_Sheet_Unchecked_Image.png");
                        activitySheetData = activitySheetData.Replace(originalSource, replacedSource);
                    }
                    if (rbWR.Checked)
                    {
                        originalSource = checkBoxImages.Find(a => a.Contains("Check_VMChargesToWR"));
                        replacedSource = originalSource.Replace("VegetationManagementEstimate_Sheet_Unchecked_Image.png", "VegetationManagementEstimate_Sheet_Checked_Image.png");
                        activitySheetData = activitySheetData.Replace(originalSource, replacedSource);
                    }
                    else
                    {
                        originalSource = checkBoxImages.Find(a => a.Contains("Check_VMChargesToWR"));
                        replacedSource = originalSource.Replace("VegetationManagementEstimate_Sheet_Checked_Image.png", "VegetationManagementEstimate_Sheet_Unchecked_Image.png");
                        activitySheetData = activitySheetData.Replace(originalSource, replacedSource);
                    }
                    if (rbOM.Checked)
                    {
                        originalSource = checkBoxImages.Find(a => a.Contains("Check_VMChargesToO&M"));
                        replacedSource = originalSource.Replace("VegetationManagementEstimate_Sheet_Unchecked_Image.png", "VegetationManagementEstimate_Sheet_Checked_Image.png");
                        activitySheetData = activitySheetData.Replace(originalSource, replacedSource);
                    }
                    else
                    {
                        originalSource = checkBoxImages.Find(a => a.Contains("Check_VMChargesToO&M"));
                        replacedSource = originalSource.Replace("VegetationManagementEstimate_Sheet_Checked_Image.png", "VegetationManagementEstimate_Sheet_Unchecked_Image.png");
                        activitySheetData = activitySheetData.Replace(originalSource, replacedSource);
                    }
                    if (rbOvertimeChargesYes.Checked)
                    {
                        originalSource = checkBoxImages.Find(a => a.Contains("Check_OvertimeChargesAllowedYES"));
                        replacedSource = originalSource.Replace("VegetationManagementEstimate_Sheet_Unchecked_Image.png", "VegetationManagementEstimate_Sheet_Checked_Image.png");
                        activitySheetData = activitySheetData.Replace(originalSource, replacedSource);

                        originalSource = checkBoxImages.Find(a => a.Contains("Check_OvertimeChargesAllowedNO"));
                        replacedSource = originalSource.Replace("VegetationManagementEstimate_Sheet_Checked_Image.png", "VegetationManagementEstimate_Sheet_Unchecked_Image.png");
                        activitySheetData = activitySheetData.Replace(originalSource, replacedSource);
                    }
                    else if (rbOvertimeChargesNo.Checked)
                    {
                        originalSource = checkBoxImages.Find(a => a.Contains("Check_OvertimeChargesAllowedNO"));
                        replacedSource = originalSource.Replace("VegetationManagementEstimate_Sheet_Unchecked_Image.png", "VegetationManagementEstimate_Sheet_Checked_Image.png");
                        activitySheetData = activitySheetData.Replace(originalSource, replacedSource);

                        originalSource = checkBoxImages.Find(a => a.Contains("Check_OvertimeChargesAllowedYES"));
                        replacedSource = originalSource.Replace("VegetationManagementEstimate_Sheet_Checked_Image.png", "VegetationManagementEstimate_Sheet_Unchecked_Image.png");
                        activitySheetData = activitySheetData.Replace(originalSource, replacedSource);
                    }
                    if (rbJobstakedYes.Checked)
                    {
                        originalSource = checkBoxImages.Find(a => a.Contains("Check_JobStakedYES"));
                        replacedSource = originalSource.Replace("VegetationManagementEstimate_Sheet_Unchecked_Image.png", "VegetationManagementEstimate_Sheet_Checked_Image.png");
                        activitySheetData = activitySheetData.Replace(originalSource, replacedSource);

                        originalSource = checkBoxImages.Find(a => a.Contains("Check_JobStakedNO"));
                        replacedSource = originalSource.Replace("VegetationManagementEstimate_Sheet_Checked_Image.png", "VegetationManagementEstimate_Sheet_Unchecked_Image.png");
                        activitySheetData = activitySheetData.Replace(originalSource, replacedSource);
                    }
                    else if (rbJobstakedNo.Checked)
                    {
                        originalSource = checkBoxImages.Find(a => a.Contains("Check_JobStakedNO"));
                        replacedSource = originalSource.Replace("VegetationManagementEstimate_Sheet_Unchecked_Image.png", "VegetationManagementEstimate_Sheet_Checked_Image.png");
                        activitySheetData = activitySheetData.Replace(originalSource, replacedSource);

                        originalSource = checkBoxImages.Find(a => a.Contains("Check_JobStakedYES"));
                        replacedSource = originalSource.Replace("VegetationManagementEstimate_Sheet_Checked_Image.png", "VegetationManagementEstimate_Sheet_Unchecked_Image.png");
                        activitySheetData = activitySheetData.Replace(originalSource, replacedSource);
                    }
                    if (rbCutLinesYes.Checked)
                    {
                        originalSource = checkBoxImages.Find(a => a.Contains("Check_CutLinesYES"));
                        replacedSource = originalSource.Replace("VegetationManagementEstimate_Sheet_Unchecked_Image.png", "VegetationManagementEstimate_Sheet_Checked_Image.png");
                        activitySheetData = activitySheetData.Replace(originalSource, replacedSource);

                        originalSource = checkBoxImages.Find(a => a.Contains("Check_CutLinesNO"));
                        replacedSource = originalSource.Replace("VegetationManagementEstimate_Sheet_Checked_Image.png", "VegetationManagementEstimate_Sheet_Unchecked_Image.png");
                        activitySheetData = activitySheetData.Replace(originalSource, replacedSource);
                    }
                    else if (rbCutLinesYes.Checked)
                    {
                        originalSource = checkBoxImages.Find(a => a.Contains("Check_CutLinesNO"));
                        replacedSource = originalSource.Replace("VegetationManagementEstimate_Sheet_Unchecked_Image.png", "VegetationManagementEstimate_Sheet_Checked_Image.png");
                        activitySheetData = activitySheetData.Replace(originalSource, replacedSource);

                        originalSource = checkBoxImages.Find(a => a.Contains("Check_CutLinesYES"));
                        replacedSource = originalSource.Replace("VegetationManagementEstimate_Sheet_Checked_Image.png", "VegetationManagementEstimate_Sheet_Unchecked_Image.png");
                        activitySheetData = activitySheetData.Replace(originalSource, replacedSource);
                    }
                    if (rbPropertyOwnersYes.Checked)
                    {
                        originalSource = checkBoxImages.Find(a => a.Contains("Check_PropertyOwnersNotifiedVmWorkOnWRYES"));
                        replacedSource = originalSource.Replace("VegetationManagementEstimate_Sheet_Unchecked_Image.png", "VegetationManagementEstimate_Sheet_Checked_Image.png");
                        activitySheetData = activitySheetData.Replace(originalSource, replacedSource);

                        originalSource = checkBoxImages.Find(a => a.Contains("Check_PropertyOwnersNotifiedVmWorkOnWRNO"));
                        replacedSource = originalSource.Replace("VegetationManagementEstimate_Sheet_Checked_Image.png", "VegetationManagementEstimate_Sheet_Unchecked_Image.png");
                        activitySheetData = activitySheetData.Replace(originalSource, replacedSource);
                    }
                    else if (rbPropertyOwnersNo.Checked)
                    {
                        originalSource = checkBoxImages.Find(a => a.Contains("Check_PropertyOwnersNotifiedVmWorkOnWRNO"));
                        replacedSource = originalSource.Replace("VegetationManagementEstimate_Sheet_Unchecked_Image.png", "VegetationManagementEstimate_Sheet_Checked_Image.png");
                        activitySheetData = activitySheetData.Replace(originalSource, replacedSource);

                        originalSource = checkBoxImages.Find(a => a.Contains("Check_PropertyOwnersNotifiedVmWorkOnWRYES"));
                        replacedSource = originalSource.Replace("VegetationManagementEstimate_Sheet_Checked_Image.png", "VegetationManagementEstimate_Sheet_Unchecked_Image.png");
                        activitySheetData = activitySheetData.Replace(originalSource, replacedSource);
                    }
                    if (rbOncorDesigner.Checked)
                    {
                        originalSource = checkBoxImages.Find(a => a.Contains("Check_OncorDesignerOrAG"));
                        replacedSource = originalSource.Replace("VegetationManagementEstimate_Sheet_Unchecked_Image.png", "VegetationManagementEstimate_Sheet_Checked_Image.png");
                        activitySheetData = activitySheetData.Replace(originalSource, replacedSource);
                    }
                    else
                    {
                        originalSource = checkBoxImages.Find(a => a.Contains("Check_OncorDesignerOrAG"));
                        replacedSource = originalSource.Replace("VegetationManagementEstimate_Sheet_Checked_Image.png", "VegetationManagementEstimate_Sheet_Unchecked_Image.png");
                        activitySheetData = activitySheetData.Replace(originalSource, replacedSource);
                    }
                    if (rbVMContractor.Checked)
                    {
                        originalSource = checkBoxImages.Find(a => a.Contains("Check_VMContractor"));
                        replacedSource = originalSource.Replace("VegetationManagementEstimate_Sheet_Unchecked_Image.png", "VegetationManagementEstimate_Sheet_Checked_Image.png");
                        activitySheetData = activitySheetData.Replace(originalSource, replacedSource);
                    }
                    else
                    {
                        originalSource = checkBoxImages.Find(a => a.Contains("Check_VMContractor"));
                        replacedSource = originalSource.Replace("VegetationManagementEstimate_Sheet_Checked_Image.png", "VegetationManagementEstimate_Sheet_Unchecked_Image.png");
                        activitySheetData = activitySheetData.Replace(originalSource, replacedSource);
                    }
                    if (chkLeaveOnProperty.Checked)
                    {
                        originalSource = checkBoxImages.Find(a => a.Contains("Check_LeaveOnProperty"));
                        replacedSource = originalSource.Replace("VegetationManagementEstimate_Sheet_Unchecked_Image.png", "VegetationManagementEstimate_Sheet_Checked_Image.png");
                        activitySheetData = activitySheetData.Replace(originalSource, replacedSource);
                    }
                    else
                    {
                        originalSource = checkBoxImages.Find(a => a.Contains("Check_LeaveOnProperty"));
                        replacedSource = originalSource.Replace("VegetationManagementEstimate_Sheet_Checked_Image.png", "VegetationManagementEstimate_Sheet_Unchecked_Image.png");
                        activitySheetData = activitySheetData.Replace(originalSource, replacedSource);
                    }
                    if (chkLeaveLogLength.Checked)
                    {
                        originalSource = checkBoxImages.Find(a => a.Contains("Check_LeaveLogLength"));
                        replacedSource = originalSource.Replace("VegetationManagementEstimate_Sheet_Unchecked_Image.png", "VegetationManagementEstimate_Sheet_Checked_Image.png");
                        activitySheetData = activitySheetData.Replace(originalSource, replacedSource);
                    }
                    else
                    {
                        originalSource = checkBoxImages.Find(a => a.Contains("Check_LeaveLogLength"));
                        replacedSource = originalSource.Replace("VegetationManagementEstimate_Sheet_Checked_Image.png", "VegetationManagementEstimate_Sheet_Unchecked_Image.png");
                        activitySheetData = activitySheetData.Replace(originalSource, replacedSource);
                    }
                    if (chkBlowChipsOnROW.Checked)
                    {
                        originalSource = checkBoxImages.Find(a => a.Contains("Check_BlowChipsOnROW"));
                        replacedSource = originalSource.Replace("VegetationManagementEstimate_Sheet_Unchecked_Image.png", "VegetationManagementEstimate_Sheet_Checked_Image.png");
                        activitySheetData = activitySheetData.Replace(originalSource, replacedSource);
                    }
                    else
                    {
                        originalSource = checkBoxImages.Find(a => a.Contains("Check_BlowChipsOnROW"));
                        replacedSource = originalSource.Replace("VegetationManagementEstimate_Sheet_Checked_Image.png", "VegetationManagementEstimate_Sheet_Unchecked_Image.png");
                        activitySheetData = activitySheetData.Replace(originalSource, replacedSource);
                    }
                    if (chkCompletelyDispose.Checked)
                    {
                        originalSource = checkBoxImages.Find(a => a.Contains("Check_CompletelyDisposeOf"));
                        replacedSource = originalSource.Replace("VegetationManagementEstimate_Sheet_Unchecked_Image.png", "VegetationManagementEstimate_Sheet_Checked_Image.png");
                        activitySheetData = activitySheetData.Replace(originalSource, replacedSource);
                    }
                    else
                    {
                        originalSource = checkBoxImages.Find(a => a.Contains("Check_CompletelyDisposeOf"));
                        replacedSource = originalSource.Replace("VegetationManagementEstimate_Sheet_Checked_Image.png", "VegetationManagementEstimate_Sheet_Unchecked_Image.png");
                        activitySheetData = activitySheetData.Replace(originalSource, replacedSource);
                    }
                    if (chkMowBrush.Checked)
                    {
                        originalSource = checkBoxImages.Find(a => a.Contains("Check_MowBrush"));
                        replacedSource = originalSource.Replace("VegetationManagementEstimate_Sheet_Unchecked_Image.png", "VegetationManagementEstimate_Sheet_Checked_Image.png");
                        activitySheetData = activitySheetData.Replace(originalSource, replacedSource);
                    }
                    else
                    {
                        originalSource = checkBoxImages.Find(a => a.Contains("Check_MowBrush"));
                        replacedSource = originalSource.Replace("VegetationManagementEstimate_Sheet_Checked_Image.png", "VegetationManagementEstimate_Sheet_Unchecked_Image.png");
                        activitySheetData = activitySheetData.Replace(originalSource, replacedSource);
                    }
                }
                sr.Close();

                StreamWriter writer = new StreamWriter(m_oVegManagementSheetTemplate);
                writer.Write(activitySheetData);
                writer.Close();
            }
            catch
            {
                m_oCommandExit = true;
                throw;
            }
            finally
            {
                checkBoxImages = null;
            }
        }

        /// <summary>
        /// Method to return Images present in HTML String.
        /// </summary>
        /// <param name="p_htmlString">HTML String.</param>
        private List<string> GetImagesInHTMLString(string p_htmlString)
        {
            List<string> images = new List<string>();
            List<string> checkImages = new List<string>();
            try
            {
                string pattern = @"<(img)\b[^>]*>";
                Regex rgx = new Regex(pattern, RegexOptions.IgnoreCase);
                MatchCollection matches = rgx.Matches(p_htmlString);

                for (int i = 0, l = matches.Count; i < l; i++)
                {
                    images.Add(matches[i].Value);
                }
                if (images.Count > 0)
                {
                    checkImages = images.FindAll(a => a.Contains("name"));
                }
            }
            catch
            {
                throw;
            }
            finally
            {
                if (images != null) images = null;
            }
            return checkImages;
        }

        /// <summary>
        /// Method to return the RACFID.
        /// </summary>
        /// <returns>userRecords</returns>
        public string[] GetRACFID()
        {
            Recordset rs = null;
            string[] userRecords = new string[2];
            try
            {
                dataAccess = new DataAccess(m_oGTApp);
                rs = dataAccess.GetRecordset("select sys_context('USERENV', 'CURRENT_USERID') as USERID, USER from dual");
                if (rs != null && rs.RecordCount > 0)
                {
                    rs.MoveFirst();
                    userRecords[0] = Convert.ToString(rs.Fields["USERID"].Value);
                    userRecords[1] = Convert.ToString(rs.Fields["USER"].Value);
                }
            }
            catch
            {
                throw;
            }
            finally
            {
                if (rs != null) rs = null;
            }
            return userRecords;
        }
        /// <summary>
        /// Method to Validate the controls of the TreeTrimRequestForm.
        /// </summary>
        private bool ValidateControls()
        {
            bool validate = true;
            string message = null;
            if (!Regex.Match(txtRecipientEmail.Text, @"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$").Success)
            {
                message = "->Please enter Valid Recipient Email.";
                validate = false;
            }
            if (!Regex.Match(txtPhoneNbr.Text, @"^[0-9-()]+$").Success)
            {
                if (!String.IsNullOrEmpty(message))
                {
                    message = message + Environment.NewLine + "->Please enter Valid Phone Number.";
                }
                else
                {
                    message = "->Please enter Valid Phone Number.";
                }
                validate = false;
            }
            if ((chkEnvironmentalConcerns.Checked || chkSpecialPrecautions.Checked || chkContactEnvironServices.Checked || chkPoliticalConcerns.Checked) && String.IsNullOrEmpty(txtSpecialInstructions.Text))
            {
                if (!String.IsNullOrEmpty(message))
                {
                    message = message + Environment.NewLine + "-> If any of the first four boxes are checked under 'Work Area Has', 'Special Instructions' field is required";
                }
                else
                {
                    message = "-> If any of the first four boxes are checked under 'Work Area Has', 'Special Instructions' field is required";
                }
                validate = false;
            }
            if (!chkLeaveLogLength.Checked && !chkLeaveOnProperty.Checked && !chkBlowChipsOnROW.Checked && !chkMowBrush.Checked && !chkCompletelyDispose.Checked)
            {
                if (!String.IsNullOrEmpty(message))
                {
                    message = message + Environment.NewLine + "-> At least one brush disposal instructions needs to selected.";
                }
                else
                {
                    message = "-> At least one brush disposal instructions needs to selected.";
                }
                validate = false;
            }
            if (!String.IsNullOrEmpty(message))
            {
                MessageBox.Show(message, "G/Technology", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            return validate;
        }
        private bool ValidateEmail()
        {
            bool validEmail = false;
            if (!String.IsNullOrEmpty(txtRecipientEmail.Text))
            {
                Regex regex = new Regex(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$");
                Match match = regex.Match(txtRecipientEmail.Text);
                if (match.Success)
                    validEmail = true;
            }
            return validEmail;
        }
        /// <summary>
        /// Method to copy of the Vegetation management sheet in local temp path.
        /// </summary>
        private bool CreateVegetationManagementSheetCopy()
        {
            Recordset rs = null;
            string sourcefile = null;
            string fileName = null;
            string destFile = null;
            bool filefound = false;
            try
            {
                rs = dataAccess.GetRecordset("select PARAM_VALUE from sys_generalparameter where PARAM_NAME='VegMgmtEstimate_Sheet'");
                if (rs != null && rs.RecordCount > 0)
                {
                    rs.MoveFirst();
                    sourcefile = Convert.ToString(rs.Fields["PARAM_VALUE"].Value);
                    if (!String.IsNullOrEmpty(sourcefile))
                    {
                        if (File.Exists(sourcefile))
                        {
                            m_oVegManagementSheetTemplate = Path.Combine(Path.GetTempPath(), Path.GetFileName(sourcefile));
                            File.Copy(sourcefile, m_oVegManagementSheetTemplate, true);

                            string[] files = Directory.GetFiles(sourcefile.Replace(".html", ""));
                            foreach (string s in files)
                            {
                                fileName = Path.GetFileName(s);
                                destFile = Path.Combine(Path.GetTempPath(), fileName);
                                File.Copy(s, destFile, true);
                            }
                            filefound = true;
                        }
                        else
                        {
                            MessageBox.Show("Could not find the file: \"" + sourcefile + "\"", "G/Technology", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                if (rs != null) rs = null;
            }
            return filefound;
        }
        #endregion
    }
}
