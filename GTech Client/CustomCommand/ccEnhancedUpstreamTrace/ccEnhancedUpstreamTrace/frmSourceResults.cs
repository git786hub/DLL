using Intergraph.GTechnology.API;
using System;
using System.Windows.Forms;

namespace GTechnology.Oncor.CustomAPI
{
    public partial class frmSourceResults : Form
    {
        private IGTCustomCommandHelper m_CustomCommandHelper;
        private IGTApplication m_Application = GTClassFactory.Create<IGTApplication>();
        private ADODB.Recordset m_TraceResultsRS = null;
        private bool m_GridLoaded = false;

        public frmSourceResults()
        {
            InitializeComponent();
        }

        internal IGTCustomCommandHelper CustomCommandHelper
        {
            set
            {
                m_CustomCommandHelper = value;
            }
        }

        internal ADODB.Recordset TraceResultsRS
        {
            set
            {
                m_TraceResultsRS = value;
            }
        }

        private void frmSourceResults_Load(object sender, EventArgs e)
        {
            try
            {
                dgvSourceResults.RowHeadersWidth = 40;

                if (m_TraceResultsRS.RecordCount > 0)
                {
                    m_TraceResultsRS.MoveFirst();

                    DataGridViewRow r;
                    dgvSourceResults.Rows.Clear();

                    // Add each record in trace results to the grid. Trace results only contains source features.
                    while (!m_TraceResultsRS.EOF)
                    {
                        r = dgvSourceResults.Rows[dgvSourceResults.Rows.Add()];
                        r.Cells["FeatureID"].Value = m_TraceResultsRS.Fields["G3E_FID"].Value;
                        r.Cells["FeatureClass"].Value = m_TraceResultsRS.Fields["G3E_USERNAME"].Value;
                        r.Cells["FeederID"].Value = m_TraceResultsRS.Fields["FEEDER_1_ID"].Value;
                        r.Cells["NetworkID"].Value = m_TraceResultsRS.Fields["NETWORK_ID"].Value;
                        r.Cells["Fno"].Value = m_TraceResultsRS.Fields["G3E_FNO"].Value;

                        m_TraceResultsRS.MoveNext();
                    }

                    dgvSourceResults.ClearSelection();
                    m_GridLoaded = true;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(m_Application.ApplicationWindow, "Error in frmSourceResults_Load: " + ex.Message,
                                    "G/Technology", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        private void frmSourceResults_FormClosing(object sender, FormClosingEventArgs e)
        {
            m_CustomCommandHelper.Complete();
        }

        private void dgvSourceResults_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            AddSelectedFidToSelectedSet();
        }

        private void dgvSourceResults_SelectionChanged(object sender, EventArgs e)
        {
            AddSelectedFidToSelectedSet();
        }

        /// <summary>
        /// Add the FID corresponding to the selected row to the select set.
        /// </summary>
        private void AddSelectedFidToSelectedSet()
        {
            if (m_GridLoaded)
            {
                if (dgvSourceResults.SelectedRows.Count > 0)
                {
                    short fno;
                    int fid;
                    IGTDDCKeyObjects ddcKO = null;
                    m_Application.SelectedObjects.Clear();

                    foreach (DataGridViewRow r in dgvSourceResults.SelectedRows)
                    {
                        fno = Convert.ToInt16(r.Cells["Fno"].Value);
                        fid = Convert.ToInt32(r.Cells["FeatureID"].Value);
                        ddcKO = m_Application.DataContext.GetDDCKeyObjects(fno, fid, GTComponentGeometryConstants.gtddcgAllGeographic);
                        if (ddcKO.Count > 0)
                        {
                            m_Application.SelectedObjects.Add(GTSelectModeConstants.gtsosmAllComponentsOfFeature, ddcKO[0]);
                        }
                    }
                }
            }
        }
    }
}
