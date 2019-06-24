// User: Akhilesh     Date: 15/04/2019    Time: 17:00  Comments: Included Job Status checking conditions as per ALM 2189

using System;
using ADODB;
using System.Windows.Forms;
using Intergraph.GTechnology.API;
using Intergraph.GTechnology.Interfaces;

namespace GTechnology.Oncor.CustomAPI
{
    public class fkqVoucherFERCAccount: IGTForeignKeyQuery 
    {
        #region Private Members
        
        IGTKeyObject m_gtFeature;
        GTArguments m_gtArguments;
        IGTDataContext m_gtDataContext;
        IGTFieldValue m_gtOutputValue;
        string m_gtFieldName;
        string m_gtTableName;
        bool m_gtReadOnly;

        IGTApplication m_gtApplication = null;

        #endregion

        #region IGTForeignKeyQuery Members
        public GTArguments Arguments
        {
            get
            {
                return m_gtArguments;

            }
            set
            {
                m_gtArguments = value;
            }
        }
        
        public IGTDataContext DataContext
        {
            get
            {
                return m_gtDataContext;
            }
            set
            {
                m_gtDataContext = value;
            }
        }

        public IGTKeyObject Feature
        {
            get
            {
                return m_gtFeature;
            }
            set
            {
                m_gtFeature = value;
            }
        }

        public string FieldName
        {
            get
            {
                return m_gtFieldName;
            }
            set
            {
                m_gtFieldName = value;
            }
        }

        public IGTFieldValue OutputValue
        {
            get
            {
                return m_gtOutputValue;
            }
        }

        public bool ReadOnly
        {
            get
            {
                return m_gtReadOnly;
            }
            set
            {
                m_gtReadOnly = value;
            }
        }

        public string TableName
        {
            get
            {
                return m_gtTableName;
            }
            set
            {
                m_gtTableName = value;
            }
        }
        public bool Execute(object InputValue)
        {
            Boolean selection = false;
            string currentJobStatus = null;
            string instanceJobStatus = null;
            try
            {
                if (m_gtFeature.FNO == 191)
                {
                    m_gtApplication = GTClassFactory.Create<IGTApplication>();

                    if (CheckJobStatus(ref currentJobStatus,ref instanceJobStatus))
                    {
                        m_gtOutputValue = GTClassFactory.Create<IGTFieldValue>();
                        QueryVoucherFERCAccount();
                        selection = true;
                    }
                    else
                    {
                        MessageBox.Show(String.Format("Selected Voucher cannot be queried as the specific voucher was created when the job was in '{0}' Status and the job is now in '{1}' Status",instanceJobStatus,currentJobStatus), "G/Technology");
                        selection = false;
                    }
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show("There is an error in \"Voucher FERC Account\" Foreign Key Query Interface \n" + ex.Message, "G/Technology");
            }
            return selection;
        }

        private bool CheckJobStatus(ref string currentJobStatus,ref string instanceJobStatus)
        {
            bool validJob = true;
            try
            {
                if (m_gtFeature != null)
                {
                    if (m_gtFeature.Components["VOUCHER_N"].Recordset != null && m_gtFeature.Components["VOUCHER_N"].Recordset.RecordCount > 0)
                    {
                        if (m_gtFeature.CID != -1)
                        {
                            string jobId = m_gtApplication.DataContext.ActiveJob;

                            Recordset jobRS = m_gtApplication.DataContext.OpenRecordset(String.Format("SELECT G3E_JOBSTATUS FROM G3E_JOB WHERE G3E_IDENTIFIER = '{0}'", jobId), CursorTypeEnum.adOpenStatic,
                                       LockTypeEnum.adLockReadOnly, (int)CommandTypeEnum.adCmdText);

                            if (jobRS != null && jobRS.RecordCount > 0)
                            {
                                jobRS.MoveFirst();

                                if (jobRS.Fields["G3E_JOBSTATUS"].Value.ToString() != m_gtFeature.Components["VOUCHER_N"].Recordset.Fields["DESIGN_ASBUILT_C"].Value.ToString())
                                {
                                    currentJobStatus = jobRS.Fields["G3E_JOBSTATUS"].Value.ToString();
                                    instanceJobStatus = m_gtFeature.Components["VOUCHER_N"].Recordset.Fields["DESIGN_ASBUILT_C"].Value.ToString();
                                    validJob = false;
                                }
                            }
                        }
                    }
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show("There is an error in \"Voucher FERC Account\" Foreign Key Query Interface\" Check Job Status \n" + ex.Message, "G/Technology");
            }
            return validJob;
        }

        #endregion

        #region Methods
        /// <summary>
        /// Method to Query the Voucher FERCAccount and Sub Account data required to load ForeignKeyQuery form.
        /// </summary>
        private void QueryVoucherFERCAccount()
        {
            Recordset workpointAttributeRS = null;
            string activeWR = null;
            string fkqConfiguredAttribute = null;
            try
            {
                fkqConfiguredAttribute = Convert.ToString(Arguments.GetArgument(0));

                activeWR = m_gtApplication.DataContext.ActiveJob;

                workpointAttributeRS = DataContext.OpenRecordset(String.Format("SELECT DISTINCT PRIME_ACCT_ID,SUB_ACCT,ACCT_DESC,CU.G3E_FID,CU.ASSOC_FNO FROM WORKPOINT_CU_N CU,WORKPOINT_N WP,REFWMIS_FERC_ACCOUNT REF WHERE CU.G3E_FID=WP.G3E_FID AND WP.WR_NBR='{0}' AND REF.PRIME_ACCT=CU.PRIME_ACCT_ID AND REF.ACTIVITY_C=CU.ACTIVITY_C", activeWR), CursorTypeEnum.adOpenStatic,
                               LockTypeEnum.adLockReadOnly, (int)CommandTypeEnum.adCmdText);

                using (VoucherFERCAccountForm voucherFERCAccountForm = new VoucherFERCAccountForm(workpointAttributeRS,m_gtFeature,fkqConfiguredAttribute,m_gtReadOnly))
                {
                    voucherFERCAccountForm.StartPosition = FormStartPosition.CenterScreen;
                    voucherFERCAccountForm.ShowDialog(m_gtApplication.ApplicationWindow);

                    if (voucherFERCAccountForm.AccountValue != null && !m_gtReadOnly)
                    {
                        IGTFeatureExplorerService mFEservice = GTClassFactory.Create<IGTFeatureExplorerService>();
                        mFEservice.ExploreFeature(m_gtFeature, "Edit");
                    }
                    //m_gtOutputValue.FieldValue = voucherFERCAccountForm.AccountValue;
                }
            }
            catch
            {
                throw;
            }
            finally
            {
                if (workpointAttributeRS != null) workpointAttributeRS = null;
            }
        }
        #endregion
    }
}
