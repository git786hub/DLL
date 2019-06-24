//----------------------------------------------------------------------------+
//        Class: VirtualPointAssociatedFeaturesForm
//  Description: Form to show the located fetaures that can be associated to active virtual point.
//----------------------------------------------------------------------------+
//     $Author:: pnlella                                                      $
//       $Date:: 15/04/2019                                                   $
//   $Revision:: 1                                                            $
//----------------------------------------------------------------------------+
//    $History:: VirtualPointAssociatedFeaturesForm.cs                        $
//----------------------------------------------------------------------------+
using System;
using ADODB;
using System.Windows.Forms;
using System.Collections.Generic;
using Intergraph.GTechnology.API;
using System.Data;

namespace GTechnology.Oncor.CustomAPI
{
    /// <summary>
    /// VirtualPointAssociatedFeaturesForm
    /// </summary>
    public partial class VirtualPointAssociatedFeaturesForm : Form
    {

        #region Private variables

        DataTable associatedFeaturesRS = null;
        int memberValue = 0;
        bool reviewMode = false;
        IGTApplication gTApplication = null;
        Dictionary<short,string> relatedFeatures = null;
        int virtualFid = 0;

        #endregion

        #region Constructors
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="attributeRS">Recordset of the Voucher attributes which needs to be displayed in form</param>
        /// <param name="readOnly">Attribute Grid mode when the interface is called</param>
        public VirtualPointAssociatedFeaturesForm(DataTable attributeRS, bool readOnly, IGTApplication application, Dictionary<short,string> p_relatedFeatures,int p_virtualFid)
        {
            InitializeComponent();
            associatedFeaturesRS = attributeRS;
            reviewMode = readOnly;
            gTApplication = application;
            relatedFeatures = p_relatedFeatures;
            virtualFid = p_virtualFid;

            gridVPAssocFeatures.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        }
        #endregion

        #region Event Handlers
        private void btnOK_Click(object sender, EventArgs e)
        {
            try
            {
                memberValue = Convert.ToInt32(gridVPAssocFeatures.SelectedRows[0].Cells["Feature Instance"].Value);
                Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error in VirtualPointAssociatedFeaturesForm: " + ex.Message, "G/Technology", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Close();
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void gridVPAssocFeatures_RowHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            try
            {
                VirtualPointAssociationUtility associationUtility = new VirtualPointAssociationUtility(gTApplication);
                associationUtility.HighLightOnMapWindow(Convert.ToInt16(gridVPAssocFeatures.SelectedRows[0].Cells["G3E_FNO"].Value), Convert.ToInt32(gridVPAssocFeatures.SelectedRows[0].Cells["Feature Instance"].Value));

                if (!reviewMode)
                {
                    btnOK.Enabled = true;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error in VirtualPointAssociatedFeaturesForm: " + ex.Message, "G/Technology", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Close();
            }
        }
        private void gridVPAssocFeatures_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            btnOK.Enabled = false;
        }
        private void gridVPAssocFeatures_RowHeaderMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {

            if (!reviewMode)
            {
                memberValue = Convert.ToInt32(gridVPAssocFeatures.SelectedRows[0].Cells["Feature Instance"].Value);
                Close();
            }           
        }
        private void VirtualPointAssociatedFeaturesForm_Load(object sender, EventArgs e)
        {
            try
            {
                gridVPAssocFeatures.DataSource = LoadVirtualPointAssociatedFeaturesData().DefaultView;
                gridVPAssocFeatures.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
               

                gridVPAssocFeatures.Columns[0].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                gridVPAssocFeatures.Columns[1].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                gridVPAssocFeatures.Columns[2].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;

                gridVPAssocFeatures.Columns["G3E_FNO"].Visible = false;
                gridVPAssocFeatures.ReadOnly = true;
                gridVPAssocFeatures.ClearSelection();

                if (reviewMode)
                {
                    btnOK.Hide();
                    btnCancel.Text = "Close";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error in VirtualPointAssociatedFeaturesForm: " + ex.Message, "G/Technology", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        #endregion

        /// <summary>
        /// AccountValue
        /// </summary>
        public int AssociatedFID
        {
            get
            {
                return memberValue;
            }
        }

        #region Methods
        public DataTable LoadVirtualPointAssociatedFeaturesData()
        {
            DataTable results = new DataTable("Results");
          
            try
            {
                results.Columns.Add("Feature");
                results.Columns.Add("Feature Instance");
                results.Columns.Add("G3E_FNO");

                if (associatedFeaturesRS != null && associatedFeaturesRS.Rows.Count > 0)
                {
                   results = associatedFeaturesRS.DefaultView.ToTable(true);                    
                }               
            }
            catch
            {
                throw;
            }
            return results;
        }
        #endregion

    }
}
