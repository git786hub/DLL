using System;
using ADODB;
using System.Windows.Forms;
using Intergraph.GTechnology.API;

namespace GTechnology.Oncor.CustomAPI
{
    /// <summary>
    /// VoucherFERCAccountForm
    /// </summary>
    public partial class VoucherFERCAccountForm : Form
    {

        #region Private variables

        IGTKeyObject wpFeature = null;
        string memberValue = null;
        Recordset wpAttributeRS = null;
        string fkqConfigAttribute = null;
        bool reviewMode = false;

        #endregion

        #region Constructors
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="attributeRS">Recordset of the Voucher attributes which needs to be displayed in form</param>
        /// <param name="feature">Current feature which is placed</param>
        /// <param name="fkqConfigAttr">Attribute for which FKQ is configured</param>
        /// <param name="readOnly">Attribute Grid mode when the interface is called</param>
        public VoucherFERCAccountForm(Recordset attributeRS, IGTKeyObject feature, string fkqConfigAttr, bool readOnly)
        {
            InitializeComponent();
            wpFeature = feature;
            wpAttributeRS = attributeRS;
            fkqConfigAttribute = fkqConfigAttr;
            reviewMode = readOnly;
        }
        /// <summary>
        /// VoucherFERCAccountForm
        /// </summary>
        /// <param name="attributeRS">Recordset of the Voucher attributes which needs to be displayed in for</param>
        public VoucherFERCAccountForm(Recordset attributeRS)
        {
            InitializeComponent();
            wpAttributeRS = attributeRS;
        }
        #endregion

        #region Event Handlers
        private void btnOK_Click(object sender, EventArgs e)
        {
            try
            {
                SetValuesForPlacedFeature();
                Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error in VoucherFERCAccountForm: " + ex.Message, "G/Technology", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void gridVoucherFERCAcct_RowHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (!reviewMode)
            {
                btnOK.Enabled = true;
            }
            else
            {
                gridVoucherFERCAcct.ClearSelection();
            }
        }
        private void gridVoucherFERCAcct_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            btnOK.Enabled = false;
        }
        private void gridVoucherFERCAcct_RowHeaderMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {

            if (!reviewMode)
            {
                btnOK.Enabled = true;
            }
            else
            {
                gridVoucherFERCAcct.ClearSelection();
            }
        }
        private void VoucherFERCAccountForm_Load(object sender, EventArgs e)
        {
            clsCommon csCommon;
            try
            {
                csCommon = new clsCommon(wpAttributeRS);
                gridVoucherFERCAcct.DataSource = csCommon.LoadVoucherFERCAccountData(wpFeature).DefaultView;
                gridVoucherFERCAcct.ReadOnly = true;
                if (wpFeature != null)
                {
                    gridVoucherFERCAcct.Columns["Description"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                }
                gridVoucherFERCAcct.ClearSelection();
                if (reviewMode)
                {
                    btnOK.Hide();
                    btnCancel.Text = "Close";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error in VoucherFERCAccountForm: " + ex.Message, "G/Technology", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                csCommon = null;
            }
        }
        #endregion

        #region Methods

        /// <summary>
        /// AccountValue
        /// </summary>
        public string AccountValue
        {
            get
            {
                return memberValue;
            }
        }
        /// <summary>
        /// Move the recordset to the current CID value.
        /// </summary>
        /// <param name="oCompRS">Recordset which needs to moved to current CID value</param>
        /// <param name="iCurrentCID">Current CID value</param>
        private void MoveRecordSetToCurrentCID(Recordset oCompRS, int iCurrentCID)
        {
            if (oCompRS.RecordCount > 0)
            {
                oCompRS.MoveFirst();

                while (oCompRS.EOF == false)
                {
                    if (Convert.ToInt32(oCompRS.Fields["G3E_CID"].Value) == iCurrentCID)
                    {
                        break;
                    }
                    oCompRS.MoveNext();
                }
            }
        }
        private void SetValuesForPlacedFeature()
        {
            if (gridVoucherFERCAcct.SelectedRows.Count == 1)
            {
                if (wpFeature != null)
                {
                    if (wpFeature.CID == -1 || wpFeature.CID == 0)
                    {
                        wpFeature.Components["VOUCHER_N"].Recordset.AddNew("G3E_FID", wpFeature.FID);
                        wpFeature.Components["VOUCHER_N"].Recordset.Fields["G3E_FNO"].Value = wpFeature.FNO;
                        wpFeature.Components["VOUCHER_N"].Recordset.Fields["G3E_CNO"].Value = wpFeature.CNO;
                        //wpFeature.Components["VOUCHER_N"].Recordset.Fields["G3E_CID"].Value = wpFeature.Components["VOUCHER_N"].Recordset.RecordCount;
                        wpFeature.Components["VOUCHER_N"].Recordset.Fields["FERC_PRIME_ACCT"].Value = gridVoucherFERCAcct.SelectedRows[0].Cells["Prime"].Value;
                        MoveRecordSetToCurrentCID(wpFeature.Components["VOUCHER_N"].Recordset, wpFeature.Components["VOUCHER_N"].Recordset.RecordCount);//We need to move this recordset to current CID value as it is always pointing to first record in the recordset
                        wpFeature.Components["VOUCHER_N"].Recordset.Fields["FERC_SUB_ACCT"].Value = gridVoucherFERCAcct.SelectedRows[0].Cells["Sub"].Value;
                    }
                    else
                    {
                        wpFeature.Components["VOUCHER_N"].Recordset.Fields["FERC_PRIME_ACCT"].Value = gridVoucherFERCAcct.SelectedRows[0].Cells["Prime"].Value;
                        MoveRecordSetToCurrentCID(wpFeature.Components["VOUCHER_N"].Recordset, wpFeature.CID);//We need to move this recordset to current CID value as it is always pointing to first record in the recordset
                        wpFeature.Components["VOUCHER_N"].Recordset.Fields["FERC_SUB_ACCT"].Value = gridVoucherFERCAcct.SelectedRows[0].Cells["Sub"].Value;
                    }
                    if (fkqConfigAttribute == "FERC_PRIME_ACCT")
                    {
                        memberValue = Convert.ToString(gridVoucherFERCAcct.SelectedRows[0].Cells["Prime"].Value);
                    }
                    else if (fkqConfigAttribute == "FERC_SUB_ACCT")
                    {
                        memberValue = Convert.ToString(gridVoucherFERCAcct.SelectedRows[0].Cells["Sub"].Value);
                    }
                }
                else
                {
                    memberValue = Convert.ToString(gridVoucherFERCAcct.SelectedRows[0].Cells["Prime Account"].Value) + "," + Convert.ToString(gridVoucherFERCAcct.SelectedRows[0].Cells["Sub Account"].Value);
                }
            }
        }
        #endregion
        
    }
}
