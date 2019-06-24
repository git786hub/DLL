using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ADODB;

namespace GTechnology.Oncor.CustomAPI
{
    public partial class frmSelectCU : Form
    {
        private string m_CU = string.Empty;

        public frmSelectCU(Recordset cuRS)
        {
            InitializeComponent();

            cuRS.Filter = "";

            if (cuRS.RecordCount > 0)
            {                
                cuRS.MoveFirst();
                while (!cuRS.EOF)
                {
                    if (cuRS.Fields["CU"].Value.ToString().Length > 0)
                    {
                        DataGridViewRow r;

                        r = dgvCUs.Rows[dgvCUs.Rows.Add()];
                        r.Cells["colCU"].Value = cuRS.Fields["CU"].Value.ToString();
                        r.Cells["colDescription"].Value = cuRS.Fields["DESCRIPTION"].Value.ToString();                        
                    }
                    cuRS.MoveNext();
                }
                dgvCUs.ClearSelection();
                m_CU = "";
            }
        }

        /// <summary>
        /// Property to return the selected CU
        /// </summary>
        public string CU
        {
            get
            {
                return m_CU;
            }
        }

        private void dgvCUs_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvCUs.SelectedRows.Count > 0)
            {
                m_CU = dgvCUs.SelectedRows[0].Cells[0].Value.ToString();
            }            
        }

        private void cmdCancel_Click(object sender, EventArgs e)
        {
            m_CU = "";
            Close();
        }

        private void cmdOK_Click(object sender, EventArgs e)
        {
            if (dgvCUs.SelectedRows.Count > 0)
            {
                m_CU = dgvCUs.SelectedRows[0].Cells[0].Value.ToString();
            }
            Close();
        }

        private void dgvCUs_CellContentDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dgvCUs.SelectedRows.Count > 0)
            {
                m_CU = dgvCUs.SelectedRows[0].Cells[0].Value.ToString();
            }
            Close();
        }
    }
}
