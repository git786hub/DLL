using System;
using System.Windows.Forms;


namespace GTechnology.Oncor.CustomAPI
{
    public partial class ccReviewVouchersForm : Form
    {
        #region Constructors
        public ccReviewVouchersForm()
        {
            InitializeComponent();
            BindData();
        }
        #endregion

        #region Event Handlers
        private void ccReviewVouchersForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            gGlobals.ExitCommand();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        private void gridReviewVouchers_ColumnHeaderMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            try
            {
                if (gGlobals.count == 1)
                {
                    gridReviewVouchers.Sort(gridReviewVouchers.Columns[e.ColumnIndex], System.ComponentModel.ListSortDirection.Ascending);
                    gGlobals.count++;
                }
                else if (gGlobals.count == 2)
                {
                    gridReviewVouchers.Sort(gridReviewVouchers.Columns[e.ColumnIndex], System.ComponentModel.ListSortDirection.Descending);
                    gGlobals.count++;
                }
                else if (gGlobals.count == 3)
                {
                    gridReviewVouchers.DataSource = null;
                    gridReviewVouchers.DataSource = gGlobals.GetData();
                    SetProperties();
                    gGlobals.count = 1;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error in Review Vouchers command: " + ex.Message, "G/Technology", MessageBoxButtons.OK, MessageBoxIcon.Error);
                gGlobals.ExitCommand();
            }
        }

        private void ccReviewVouchersForm_KeyUp(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Escape)
                {
                    this.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error in Review Vouchers command: " + ex.Message, "G/Technology", MessageBoxButtons.OK, MessageBoxIcon.Error);
                gGlobals.ExitCommand();
            }
        }
        #endregion

        #region Private Methods

        /// <summary>
        /// Method to bind the data to grid.
        /// </summary>
        private void BindData()
        {
            try
            {
                gridReviewVouchers.DataSource = gGlobals.GetData();
                SetProperties();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error in Review Vouchers command: " + ex.Message, "G/Technology", MessageBoxButtons.OK, MessageBoxIcon.Error);
                gGlobals.ExitCommand();
            }
        }

        /// <summary>
        /// Method to set properties for data grid view.
        /// </summary>
        private void SetProperties()
        {
            try
            {
                for (int i = 0; i < gridReviewVouchers.ColumnCount; i++)
                {
                    gridReviewVouchers.Columns[i].SortMode = DataGridViewColumnSortMode.NotSortable;
                }
                gridReviewVouchers.ReadOnly = true;
                gridReviewVouchers.Columns[6].AutoSizeMode = DataGridViewAutoSizeColumnMode.ColumnHeader;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error in Review Vouchers command: " + ex.Message, "G/Technology", MessageBoxButtons.OK, MessageBoxIcon.Error);
                gGlobals.ExitCommand();
            }
        }
        #endregion

    }
}
