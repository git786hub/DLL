using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using Intergraph.GTechnology.API;
using OncorTicketCreation;

namespace GTechnology.Oncor.CustomAPI
{
    public partial class frmEditNjunsTicket : Form
    {
        internal IGTDataContext DataContext;
        internal int PoleFid;
        internal Ticket objTicket = null;
        internal List<Tuple<int, int, string>> ticketsForPole = null;

        bool changesSaved = true;

        DataLayer dataLayer = null;
        GridSelectionHelper m_GridSelectionHelper = null;

        public frmEditNjunsTicket()
        {
            InitializeComponent();

        }

        /// <summary>
        /// Method to check for existence of local ticket 
        /// </summary>
        /// <returns>True if a local ticket for the pole exists</returns>
        internal short GetTicketCount()
        {
            try
            {
                ticketsForPole = objTicket.GetTicketIdsAndNumber(PoleFid);

                if (ticketsForPole == null)
                    return 0;

                return (short)ticketsForPole.Count;
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Method to return all the tickets available for a pole
        /// </summary>
        /// <returns>Tickets (GIS Njusn Tickets) as a datatable</returns>
        internal DataTable GetLocalTickets()
        {

            DataTable table = new DataTable();
            table.Columns.Add("GIS Tickets", typeof(int));
            foreach (Tuple<int, int, string> ticket in ticketsForPole)
            {
                table.Rows.Add(ticket.Item1);

            }
            return table;
        }



        #region Form events
        /// <summary>
        /// Form load event handler
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void frmEditNjunsTicket_Load(object sender, EventArgs e)
        {
            try
            {
                LoadTheForm();
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Method to load the form and set the controls to read-only /  read-write based on whether the ticket is submitted to NJUNS or not respectively.
        /// </summary>
        public void LoadTheForm()
        {
            try
            {
                if (objTicket.TICKET_NUMBER <= 0 && objTicket.NJUNS_TICKET_ID == "-1")
                {
                    // Ticket is already created in GIS. 
                    objTicket.GetTicketAttributes(objTicket.GIS_NJUNS_TICKET_ID);
                    LoadUI();
                    UnLockControlValues(new List<Control> { grpBxTicket, grpBxSteps, grpBxSubmission });
                    UpdateUIControlState(0);
                }
                else
                {
                    objTicket.GetTicketAttributes(objTicket.GIS_NJUNS_TICKET_ID, objTicket.TICKET_NUMBER, objTicket.NJUNS_TICKET_ID);
                    LoadUI();
                    dgvSteps.CurrentCell.Selected = false;
                    LockControlValues(new List<Control> { grpBxTicket, grpBxSteps, grpBxSubmission });
                }
                BoldOutRequiredAttributes();
                SetFieldsReadOnly();
            }
            catch (Exception)
            {

                throw;
            }
        }

        /// <summary>
        /// Bold out the certain labels on UI indicating that these fields are mandatory
        /// </summary>
        private void BoldOutRequiredAttributes()
        {
            try
            {
                lblStructureId.Font = new Font("Microsoft Sans Serif", 7.8F, FontStyle.Bold);
                lblNjunsMemCode.Font = new Font("Microsoft Sans Serif", 7.8F, FontStyle.Bold);
                lblPoleOwner.Font = new Font("Microsoft Sans Serif", 7.8F, FontStyle.Bold);
                lblState.Font = new Font("Microsoft Sans Serif", 7.8F, FontStyle.Bold);
                lblCounty.Font = new Font("Microsoft Sans Serif", 7.8F, FontStyle.Bold);
                lblPriority.Font = new Font("Microsoft Sans Serif", 7.8F, FontStyle.Bold);
                dgvSteps.Columns[1].HeaderCell.Style.Font = new Font("Microsoft Sans Serif", 7.8F, FontStyle.Bold);
                dgvSteps.Columns[3].HeaderCell.Style.Font = new Font("Microsoft Sans Serif", 7.8F, FontStyle.Bold);
                dgvSteps.Columns[5].HeaderCell.Style.Font = new Font("Microsoft Sans Serif", 7.8F, FontStyle.Bold);
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Set certain fields on the UI to read-only 
        /// </summary>
        private void SetFieldsReadOnly()
        {
            try
            {
                txtStructureId.ReadOnly = true;
                txtWr.ReadOnly = true;
                txtStatus.ReadOnly = true;
                txtPlot.ReadOnly = true;
                txtInvoice.ReadOnly = true;
            }
            catch (Exception)
            {

                throw;
            }
        }

        /// <summary>
        /// Form closing event handler
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void frmEditNjunsTicket_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                if (ticketBindingSource != null)
                {
                    ticketBindingSource.Dispose();
                }
                if (nJUNSMemberCodeListBindingSource != null)
                {
                    nJUNSMemberCodeListBindingSource.Dispose();
                }
                if (poleOwnerListBindingSource != null)
                {
                    poleOwnerListBindingSource.Dispose();
                }
                if (priorityListBindingSource != null)
                {
                    priorityListBindingSource.Dispose();
                }
                if (ticketStepTypeListBindingSource != null)
                {
                    ticketStepTypeListBindingSource.Dispose();
                }
            }
            catch (Exception)
            {
                throw;
            }

        }

        /// <summary>
        /// Member code combo box selection changed event handler
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cmbNjunsMemCode_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                objTicket.NJUNS_MEMBER_CODE = Convert.ToString(((ComboBox)sender).SelectedItem);
                changesSaved = false;
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Pole owner combo box selection changed event handler
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cmbPoleOwner_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                objTicket.POLE_OWNER = Convert.ToString(((ComboBox)sender).SelectedItem);
                changesSaved = false;
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Priority combo box selection changed event handler
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cmbPriority_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                objTicket.PRIORITY_CODE = Convert.ToString(((ComboBox)sender).SelectedItem);
                changesSaved = false;
            }
            catch (Exception)
            {
                throw;
            }

        }
        #endregion

        #region Button events

        /// <summary>
        /// Save button click event handler. This method saved the ticket details to the DB.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                customNjunsSharedLibrary njunsSharedLibrary = new customNjunsSharedLibrary();
                ResetStepIds();
                objTicket.TicketStepTypeList = GetStepsByOrder();
                dataLayer = new DataLayer();
                dataLayer.DataContext = DataContext;
                dataLayer.CopyPropertiesFrom(objTicket);
                dataLayer.SaveTicketAndStepDetails();
                ticketBindingSource.ResetBindings(false);
                changesSaved = true;
                this.Close();
            }
            catch (Exception)
            {
                throw;
            }
        }
        private void btnPlot_Click(object sender, EventArgs e)
        { }
        private void btnInvoice_Click(object sender, EventArgs e)
        { }

        /// <summary>
        /// Expedite submission button click event handler.This method submits the ticket to NJUNS submit ticket service
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnExpSubmission_Click(object sender, EventArgs e)
        {

            if (!ValidateRequiredFieldsForNull())
            {
                // Show error and  return
                MessageBox.Show("Failed to submit the ticket.Required fields are empty.", "G/Technology");
                return;
            }
            if (!changesSaved)
            {
                MessageBox.Show("Please save the changes before submitting the ticket. ", "G/Technology");
                return;
            }
            // Submit the ticket 
            customNjunsSharedLibrary njunsSharedLibrary = new customNjunsSharedLibrary();
            if (GetStepsByOrder() == null)
            {
                MessageBox.Show("A ticket should have atleast one step for submission.", "G/Technology");
                return;
            }
            if (njunsSharedLibrary.SubmitTicket(objTicket.GIS_NJUNS_TICKET_ID, 'I'))
            {
                // Make the form read only. Grey out Submission and Save buttons.
                LockControlValues(new List<Control> { grpBxTicket, grpBxSteps, grpBxSubmission });
            }

        }




        /// <summary>
        /// Up button click event handler
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnUp_Click(object sender, EventArgs e)
        {
            try
            {
                RemoveRow(m_GridSelectionHelper.SelectedRowIndex);
                RemoveRow(m_GridSelectionHelper.AboveRowIndex);
                InsertRow(m_GridSelectionHelper.AboveRowIndex, m_GridSelectionHelper.SelectedRow);
                InsertRow(m_GridSelectionHelper.SelectedRowIndex, m_GridSelectionHelper.RowAbove);
                ResetStepIds();
                dgvSteps.ClearSelection();
                dgvSteps.Rows[m_GridSelectionHelper.AboveRowIndex].Selected = true;
                m_GridSelectionHelper.SelectedRow = (TicketStepType)dgvSteps.Rows[m_GridSelectionHelper.AboveRowIndex].DataBoundItem;
                m_GridSelectionHelper.SelectedRowIndex = dgvSteps.Rows[m_GridSelectionHelper.AboveRowIndex].Index;
                GetAdjacentRowsOfGrid(m_GridSelectionHelper.SelectedRowIndex);
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Down button click event handler
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnDown_Click(object sender, EventArgs e)
        {
            try
            {
                RemoveRow(m_GridSelectionHelper.BelowRowIndex);
                RemoveRow(m_GridSelectionHelper.SelectedRowIndex);
                InsertRow(m_GridSelectionHelper.SelectedRowIndex, m_GridSelectionHelper.RowBelow);
                InsertRow(m_GridSelectionHelper.BelowRowIndex, m_GridSelectionHelper.SelectedRow);
                ResetStepIds();
                dgvSteps.ClearSelection();
                dgvSteps.Rows[m_GridSelectionHelper.BelowRowIndex].Selected = true;
                m_GridSelectionHelper.SelectedRow = (TicketStepType)dgvSteps.Rows[m_GridSelectionHelper.BelowRowIndex].DataBoundItem;
                m_GridSelectionHelper.SelectedRowIndex = dgvSteps.Rows[m_GridSelectionHelper.BelowRowIndex].Index;
                GetAdjacentRowsOfGrid(m_GridSelectionHelper.SelectedRowIndex);
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Cancel/ Close button click event handler
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        #endregion

        #region Data Grid View events

        /// <summary>
        /// Data Grid View (DGV) Row header click event handler
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dgvSteps_RowHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            try
            {
                m_GridSelectionHelper = new GridSelectionHelper();

                int rowIndex = Convert.ToInt32(((DataGridView)sender).SelectedRows[0].Index);
                if ((rowIndex == dgvSteps.RowCount - 2) || (rowIndex == dgvSteps.RowCount - 1))
                {
                    UpdateUIControlState(rowIndex);
                }

                DataGridViewRow selectedRow = dgvSteps.SelectedRows[0];
                m_GridSelectionHelper.SelectedRow = (TicketStepType)selectedRow.DataBoundItem;
                m_GridSelectionHelper.SelectedRowIndex = selectedRow.Index;

                GetAdjacentRowsOfGrid(rowIndex);
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Data Grid View (DGV) user deleted row event handler
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dgvSteps_UserDeletedRow(object sender, DataGridViewRowEventArgs e)
        {
            try
            {
                ResetStepIds();
                dgvSteps.ClearSelection();
                UpdateUIControlState(dgvSteps.Rows.Count - 1);
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Data Grid View (DGV) cell click event handler
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dgvSteps_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            UpdateUIControlState(dgvSteps.Rows.Count - 1);
        }

        /// <summary>
        /// Method to set a flag stating that user has edited ticket details and those changes are not saved
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ticketBindingSource_ListChanged(object sender, System.ComponentModel.ListChangedEventArgs e)
        {
            if (e != null && e.ListChangedType == ListChangedType.ItemChanged)
            {
                changesSaved = false;
            }

        }

        /// <summary>
        /// Method to set a flag stating that user has edited ticket details and those changes are not saved
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ticketStepTypeListBindingSource_ListChanged(object sender, System.ComponentModel.ListChangedEventArgs e)
        {
            if (e != null && e.ListChangedType == ListChangedType.ItemChanged)
            {
                changesSaved = false;
            }
        }

        #endregion

        #region Private methods

        /// <summary>
        /// Check whether the required fields are null.
        /// </summary>
        /// <returns>false, if any of the required is null. Else returns true</returns>
        private bool ValidateRequiredFieldsForNull()
        {
            try
            {
                if (string.IsNullOrEmpty(objTicket.TICKET_TYPE) ||
                    string.IsNullOrEmpty(objTicket.POLE_NUMBER) ||
                    string.IsNullOrEmpty(objTicket.NJUNS_MEMBER_CODE) ||
                    string.IsNullOrEmpty(objTicket.POLE_OWNER) ||
                    string.IsNullOrEmpty(objTicket.STATE) ||
                    string.IsNullOrEmpty(objTicket.COUNTY) ||
                    string.IsNullOrEmpty(objTicket.PRIORITY_CODE))
                {
                    return false;
                }

                foreach (TicketStepType step in objTicket.TicketStepTypeList)
                {
                    if (step.Equals(null) ||
                        step.NjunsMemberCode.Equals(null) ||
                        string.IsNullOrEmpty(step.NjunsMemberCode.Value) ||
                        string.IsNullOrEmpty(step.JobType) ||
                        string.IsNullOrEmpty(step.DaysInterval))
                    {
                        return false;
                    }
                }
                return true;
            }
            catch (Exception)
            {

                throw;
            }
        }

        /// <summary>
        /// Method to load UI
        /// </summary>
        private void LoadUI()
        {
            try
            {
                ticketBindingSource.Clear(); //Clear any existing data bindings before adding latest one.
                ticketBindingSource.Add(objTicket);

                cmbNjunsMemCode.SelectedIndexChanged -= cmbNjunsMemCode_SelectedIndexChanged;
                cmbPriority.SelectedIndexChanged -= cmbPriority_SelectedIndexChanged;
                cmbPoleOwner.SelectedIndexChanged -= cmbPoleOwner_SelectedIndexChanged;
                cmbNjunsMemCode.SelectedItem = objTicket.NJUNS_MEMBER_CODE;
                cmbPriority.SelectedItem = objTicket.PRIORITY_CODE;
                cmbPoleOwner.SelectedItem = objTicket.POLE_OWNER;
                cmbNjunsMemCode.SelectedIndexChanged += cmbNjunsMemCode_SelectedIndexChanged;
                cmbPriority.SelectedIndexChanged += cmbPriority_SelectedIndexChanged;
                cmbPoleOwner.SelectedIndexChanged += cmbPoleOwner_SelectedIndexChanged;
                UpdateUIControlState(-1);
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Method to get the adjacent rows (row above and row below) for a selected row
        /// </summary>
        /// <param name="rowIndex">Row index of the current row for which adjacent rows are to be captured</param>
        private void GetAdjacentRowsOfGrid(int rowIndex)
        {
            int aboveRowIndex = rowIndex - 1;
            int belowRowIndex = rowIndex + 1;

            DataGridViewRow rowAboveTheSelected = aboveRowIndex < 0 ? null : dgvSteps.Rows[aboveRowIndex];
            DataGridViewRow rowBelowTheSelected = belowRowIndex > dgvSteps.Rows.Count - 1 ? null : dgvSteps.Rows[belowRowIndex];

            if (rowBelowTheSelected == null && rowAboveTheSelected == null)
            {
                UpdateUIControlState(rowIndex);
            }
            else if (rowAboveTheSelected == null)
            {
                m_GridSelectionHelper.RowBelow = (TicketStepType)rowBelowTheSelected.DataBoundItem;
                m_GridSelectionHelper.BelowRowIndex = rowBelowTheSelected.Index;
                UpdateUIControlState(0);
            }
            else if (rowBelowTheSelected == null)
            {
                m_GridSelectionHelper.RowAbove = (TicketStepType)rowAboveTheSelected.DataBoundItem;
                m_GridSelectionHelper.AboveRowIndex = rowAboveTheSelected.Index;
                UpdateUIControlState(dgvSteps.Rows.Count - 1);
            }
            else
            {
                m_GridSelectionHelper.RowBelow = (TicketStepType)rowBelowTheSelected.DataBoundItem;
                m_GridSelectionHelper.BelowRowIndex = rowBelowTheSelected.Index;
                m_GridSelectionHelper.RowAbove = (TicketStepType)rowAboveTheSelected.DataBoundItem;
                m_GridSelectionHelper.AboveRowIndex = rowAboveTheSelected.Index;
                UpdateUIControlState(rowIndex);
            }
        }

        /// <summary>
        /// Insert a row of type 'Ticket' at specified row index
        /// </summary>
        /// <param name="rowIindex"> row index</param>
        /// <param name="t"> ticket step object</param>
        private void InsertRow(int rowIindex, TicketStepType t)
        {
            try
            {
                BindingSource bs = (BindingSource)this.ticketStepTypeListBindingSource;
                bs.Insert(rowIindex, t);
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Remove row at specified row index
        /// </summary>
        /// <param name="rowIindex"> row index</param>
        private void RemoveRow(int rowIndex)
        {
            try
            {
                BindingSource bs = (BindingSource)this.ticketStepTypeListBindingSource;
                bs.RemoveAt(rowIndex);
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Method to reset the step ids for the steps in DGV
        /// </summary>
        private void ResetStepIds()
        {
            try
            {
                changesSaved = false;
                for (int i = 0; i < dgvSteps.Rows.Count; i++)
                {
                    dgvSteps.Rows[i].Cells[0].Value = i + 1;
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Update state of button controls based on row index.
        /// </summary>
        /// <param name="rowIndex">Row index for which buttons states need to be adjusted</param>
        private void UpdateUIControlState(int rowIndex)
        {
            try
            {
                if (rowIndex == dgvSteps.RowCount - 2)
                {
                    btnUp.Enabled = true;
                    btnDown.Enabled = false;
                }
                else if (rowIndex == dgvSteps.RowCount - 1)
                {
                    btnUp.Enabled = false;
                    btnDown.Enabled = false;
                }
                else
                {
                    btnUp.Enabled = (dgvSteps.SelectedRows.Count == 1 && rowIndex != 0);
                    btnDown.Enabled = dgvSteps.SelectedRows.Count == 1 && rowIndex != dgvSteps.Rows.Count - 1;
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Read the all steps in DGV
        /// </summary>
        /// <returns></returns>
        private List<TicketStepType> GetStepsByOrder()
        {
            try
            {
                if (dgvSteps.Rows.Count > 0)
                {
                    List<TicketStepType> steps = new List<TicketStepType>();
                    foreach (DataGridViewRow row in dgvSteps.Rows)
                    {
                        steps.Add(HandleSpecialCharacters((TicketStepType)row.DataBoundItem));
                    }
                    return steps;
                }
                return null;
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Handle single quote in ticket step details
        /// </summary>
        /// <param name="step"></param>
        /// <returns></returns>
        private TicketStepType HandleSpecialCharacters(TicketStepType step)
        {
            if (step != null)
            {
                if (!string.IsNullOrEmpty(step.JobType))
                    step.JobType = step.JobType.Replace("'", "''").Replace("&", "'&");
                if (!string.IsNullOrEmpty(step.DaysInterval))
                    step.DaysInterval = step.DaysInterval.Replace("'", "''").Replace("&", "'&");
                if (!string.IsNullOrEmpty(step.NumberOfPoles))
                    step.NumberOfPoles = step.NumberOfPoles.Replace("'", "''").Replace("&", "'&");
                if (step.NjunsMemberCode != null && !string.IsNullOrEmpty(step.NjunsMemberCode.Value))
                    step.NjunsMemberCode.Value = step.NjunsMemberCode.Value.Replace("'", "''").Replace("&", "'&");
                if (!string.IsNullOrEmpty(step.Remarks))
                    step.Remarks = step.Remarks.Replace("'", "''").Replace("&", "'&");
            }
            return step;
        }

        /// <summary>
        /// Lock (set read only) all the controls in the DGV
        /// </summary>
        /// <param name="Containers"></param>
        private void LockControlValues(List<Control> Containers)
        {
            try
            {
                foreach (Control container in Containers)
                {
                    foreach (Control ctrl in container.Controls)
                    {
                        if (ctrl.GetType() == typeof(TextBox))
                            ((TextBox)ctrl).ReadOnly = true;
                        if (ctrl.GetType() == typeof(ComboBox))
                            ((ComboBox)ctrl).Enabled = false;
                        if (ctrl.GetType() == typeof(Button))
                            ((Button)ctrl).Enabled = false;
                        if (ctrl.GetType() == typeof(DateTimePicker))
                            ((DateTimePicker)ctrl).Enabled = false;
                        if (ctrl.GetType() == typeof(DataGridView))
                        {
                            ((DataGridView)ctrl).ReadOnly = true;
                            dgvSteps.DefaultCellStyle.BackColor = SystemColors.Control;
                            dgvSteps.DefaultCellStyle.ForeColor = SystemColors.GrayText;
                            dgvSteps.ColumnHeadersDefaultCellStyle.BackColor = SystemColors.Control;
                            dgvSteps.ColumnHeadersDefaultCellStyle.ForeColor = SystemColors.GrayText;
                            dgvSteps.BackgroundColor = SystemColors.Control;
                        }
                    }
                }
                btnCancel.Text = "Close";
                btnDown.Enabled = false;
                btnUp.Enabled = false;
                btnCancel.Enabled = true;
                this.dgvSteps.RowHeaderMouseClick -= new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.dgvSteps_RowHeaderMouseClick);
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Unlock (set read write) all the controls in the DGV
        /// </summary>
        /// <param name="Containers"></param>
        private void UnLockControlValues(List<Control> Containers)
        {
            try
            {
                foreach (Control container in Containers)
                {
                    foreach (Control ctrl in container.Controls)
                    {
                        if (ctrl.GetType() == typeof(TextBox))
                            ((TextBox)ctrl).ReadOnly = false;
                        if (ctrl.GetType() == typeof(ComboBox))
                            ((ComboBox)ctrl).Enabled = true;
                        if (ctrl.GetType() == typeof(Button))
                            ((Button)ctrl).Enabled = false;
                        if (ctrl.GetType() == typeof(DateTimePicker))
                            ((DateTimePicker)ctrl).Enabled = true;
                        if (ctrl.GetType() == typeof(DataGridView))
                        {
                            ((DataGridView)ctrl).ReadOnly = false;
                            dgvSteps.DefaultCellStyle.BackColor = SystemColors.Window;
                            dgvSteps.DefaultCellStyle.ForeColor = SystemColors.ControlText;
                            dgvSteps.ColumnHeadersDefaultCellStyle.BackColor = SystemColors.Control;
                            dgvSteps.ColumnHeadersDefaultCellStyle.ForeColor = SystemColors.WindowText;
                            dgvSteps.BackgroundColor = SystemColors.AppWorkspace;
                        }
                    }
                }
                btnCancel.Text = "Cancel";
                btnDown.Enabled = true;
                btnUp.Enabled = true;
                btnCancel.Enabled = true;
                btnSave.Enabled = true;
                btnExpSubmission.Enabled = true;
            }
            catch (Exception)
            {
                throw;
            }
        }


        #endregion

    }
}
