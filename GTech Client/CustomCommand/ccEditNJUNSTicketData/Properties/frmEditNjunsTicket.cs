using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GTechnology.Oncor.CustomAPI
{
    public partial class frmEditNjunsTicket : Form
    {
        
        public frmEditNjunsTicket()
        {
            InitializeComponent();
        }

        private void frmEditNjunsTicket_Load(object sender, EventArgs e)
        {
            try
            {
                LoadUI();
            }
            catch (Exception)
            {
                throw;
            }
        }

        private void LoadUI()
        {
            try
            {
                LoadBindingSources();
                LoadDataGridView();

            }
            catch (Exception)
            {
                throw;
            }
        }

        private void LoadBindingSources()
        {
            try
            {
                BindingSource ticketBS = new BindingSource();
                ticketBS.DataSource = typeof(Ticket);
                Ticket t = new Ticket { StructureId = "001" };
                ticketBS.Add(new Ticket { StructureId = "001" });
                txtStructureId.DataBindings.Add("Text", ticketBS, "StructureId");
                //txtStructureId.DataBindings.Add()
            }
            catch (Exception)
            {
                throw;
            }
        }

        private void LoadDataGridView()
        {
            try
            {
                DataGridViewTextBoxColumn  seqColumn = new DataGridViewTextBoxColumn
                {
                    Name = "colSeq",
                    HeaderText = "Seq",
                    Width = 30
                };
                dgvSteps.Columns.Add(seqColumn);
                DataGridViewComboBoxColumn njunsMemberColumn = new DataGridViewComboBoxColumn
                {
                    Name = "colNjunsMember",
                    HeaderText = "NJUNS\r\nMember",
                    Width = 110
                    //DataSource = nJunsMemberDS
                };
                dgvSteps.Columns.Add(njunsMemberColumn);

                DataGridViewTextBoxColumn typeColumn = new DataGridViewTextBoxColumn
                {
                    Name = "colType",
                    HeaderText = "Type",
                    Width = 60
                };
                dgvSteps.Columns.Add(typeColumn);
                DataGridViewTextBoxColumn daysIntervalColumn = new DataGridViewTextBoxColumn
                {
                    Name = "colDaysInt",
                    HeaderText = "Days\r\nInterval",
                    Width = 60
                };
                dgvSteps.Columns.Add(daysIntervalColumn);
                DataGridViewTextBoxColumn remarksColumn = new DataGridViewTextBoxColumn
                {
                    Name = "colRemarks",
                    HeaderText = "Remarks",
                    Width = 200
                };
                dgvSteps.Columns.Add(remarksColumn);
                DataGridViewButtonColumn plotColumn = new DataGridViewButtonColumn
                {
                    Name = "colPlot",
                    HeaderText = "Plot",
                    Width = 30
                };
                dgvSteps.Columns.Add(plotColumn);
                DataGridViewButtonColumn invoiceColumn = new DataGridViewButtonColumn
                {
                    Name = "colInv",
                    HeaderText = "Inv.",
                    Width = 30
                };
                dgvSteps.Columns.Add(invoiceColumn);
            }
            catch (Exception)
            {
                throw;
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {

            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
