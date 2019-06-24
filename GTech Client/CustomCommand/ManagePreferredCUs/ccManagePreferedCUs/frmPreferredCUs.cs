using Intergraph.GTechnology.API;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace GTechnology.Oncor.CustomAPI
{
    public partial class frmPreferredCUs : Form
    {
        IGTDataContext m_DataContext;
        ManagePreferredCUsHelper m_oCUsHelper;
        string m_selectedCategory;
        Dictionary<string, string> m_lstAddedToPreferred = new Dictionary<string, string>();
        Dictionary<string, string> m_lstAddedToAvailable = new Dictionary<string, string>();

        public frmPreferredCUs(IGTDataContext dataContext)
        {
            m_DataContext = dataContext;
            InitializeComponent();
            m_oCUsHelper = new ManagePreferredCUsHelper();
            GetDataForForm();
            UpdateButtonStatus();
        }
        /// <summary>
        /// Method to update the controls with data
        /// </summary>
        private void GetDataForForm()
        {
            try
            {
                m_oCUsHelper.DataContext = m_DataContext;
                cmbCategory.DataSource = m_oCUsHelper.GetAllCategories();
                cmbCategory.SelectedItem = cmbCategory.Items[0];
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Method to fill data in  list boxes
        /// </summary>
        /// <param name="category"></param>
        private void AddItemsToListBox(string category)
        {
            try
            {
                if (!string.IsNullOrEmpty(category))
                {
                    category = category.Split('-')[0].Trim();
                }
                m_oCUsHelper.SelectedCategory = category;
                List<string> preferredCUs = m_oCUsHelper.GetUserPreferredCUsForCategory(category);
                foreach (string item in preferredCUs)
                {
                    if (!m_lstAddedToAvailable.Keys.Contains(item))
                    {
                        listboxPreferred.Items.Add(item);
                    }
                }

                string[] arrayPreferred = m_lstAddedToPreferred.Where(a => a.Value == category).Select(b => b.Key).ToArray();
                foreach (string item in arrayPreferred)
                {
                    if (!listboxPreferred.Items.Contains(item))
                    {
                        listboxPreferred.Items.Add(item);
                    }

                }
                List<string> availableCUs = m_oCUsHelper.GetDeltaCUsForCategory(category);
                foreach (string item in availableCUs)
                {
                    if (!m_lstAddedToPreferred.Keys.Contains(item))
                    {
                        listboxAvailable.Items.Add(item);
                    }
                }

                string[] arrayAvailable = m_lstAddedToAvailable.Where(a => a.Value == category).Select(b => b.Key).ToArray();
                foreach (string item in arrayAvailable)
                {
                    if (!listboxAvailable.Items.Contains(item))
                    {
                        listboxAvailable.Items.Add(item);
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Event handler for "<"  button click event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnToPreferred_Click(object sender, EventArgs e)
        {
            try
            {
                while (listboxAvailable.SelectedItems.Count > 0)
                {
                    if (m_lstAddedToAvailable.ContainsKey(listboxAvailable.SelectedItem.ToString()))
                    {
                        m_lstAddedToAvailable.Remove(listboxAvailable.SelectedItem.ToString());
                    }
                    m_lstAddedToPreferred.Add(listboxAvailable.SelectedItem.ToString(), m_selectedCategory);
                    listboxPreferred.Items.Add(listboxAvailable.SelectedItem.ToString());
                    listboxAvailable.Items.Remove(listboxAvailable.SelectedItem.ToString());
                }
                UpdateButtonStatus();
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Event handler for ">"  button click event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnToAvailable_Click(object sender, EventArgs e)
        {
            try
            {
                while (listboxPreferred.SelectedItems.Count > 0)
                {
                    if (m_lstAddedToPreferred.ContainsKey(listboxPreferred.SelectedItem.ToString()))
                    {
                        m_lstAddedToPreferred.Remove(listboxPreferred.SelectedItem.ToString());
                    }
                    m_lstAddedToAvailable.Add(listboxPreferred.SelectedItem.ToString(), m_selectedCategory);
                    listboxAvailable.Items.Add(listboxPreferred.SelectedItem.ToString());
                    listboxPreferred.Items.Remove(listboxPreferred.SelectedItem.ToString());
                }
                UpdateButtonStatus();
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Event handler for "<<"  button click event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnToPreferredMany_Click(object sender, EventArgs e)
        {
            try
            {
                foreach (string item in listboxAvailable.Items)
                {
                    if (!m_lstAddedToPreferred.ContainsKey(item))
                    {
                        m_lstAddedToPreferred.Add(item, m_selectedCategory);
                        m_lstAddedToAvailable.Remove(item);
                    }

                }
                listboxPreferred.Items.AddRange(listboxAvailable.Items);
                listboxAvailable.Items.Clear();
                UpdateButtonStatus();
            }
            catch (Exception)
            {
                throw;
            }
        }
        /// <summary>
        /// Event handler for ">>"  button click event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnToAvailableMany_Click(object sender, EventArgs e)
        {
            try
            {
                foreach (string item in listboxPreferred.Items)
                {
                    if (!m_lstAddedToAvailable.ContainsKey(item))
                    {
                        m_lstAddedToAvailable.Add(item, m_selectedCategory);
                        m_lstAddedToPreferred.Remove(item);

                    }
                }
                listboxAvailable.Items.AddRange(listboxPreferred.Items);
                listboxPreferred.Items.Clear();
                UpdateButtonStatus();
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Event handler for Category filter combo box
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cmbCategory_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                listboxPreferred.Items.Clear();
                listboxAvailable.Items.Clear();
                string selectedItem = Convert.ToString(((ComboBox)sender).SelectedItem);
                m_selectedCategory = selectedItem.Split('-')[0].Trim();
                AddItemsToListBox(selectedItem);
                UpdateButtonStatus();
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Method to update the button status based on selection in list boxes
        /// </summary>
        private void UpdateButtonStatus()
        {
            try
            {
                btnToAvailable.Enabled = listboxPreferred.Items.Count > 0 && listboxPreferred.SelectedItems.Count > 0;
                btnToPreferred.Enabled = listboxAvailable.Items.Count > 0 && listboxAvailable.SelectedItems.Count > 0;
                btnToPreferredMany.Enabled = listboxAvailable.Items.Count > 0;
                btnToAvailableMany.Enabled = listboxPreferred.Items.Count > 0;
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Event handler for Cancel button click event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnCancel_Click(object sender, EventArgs e)
        {
            frmPreferredCUs_FormClosing(sender, null);
        }

        /// <summary>
        /// Event handler for Save button click event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                m_oCUsHelper.SaveToPreferredCUs(m_lstAddedToPreferred);
                m_oCUsHelper.DeleteFromPreferredCUs(m_lstAddedToAvailable);
                this.FormClosing -= frmPreferredCUs_FormClosing;
                this.Close();
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Event handler for listboxPreferred selection change event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void listboxPreferred_SelectedIndexChanged(object sender, EventArgs e)
        {
            UpdateButtonStatus();
        }

        /// <summary>
        /// Event handler for listboxAvailable selection change event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void listboxAvailable_SelectedIndexChanged(object sender, EventArgs e)
        {
            UpdateButtonStatus();
        }

        /// <summary>
        /// Event handler for form closing event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void frmPreferredCUs_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                if (MessageBox.Show("Are you sure you want to cancel and lose any pending changes ?", "G/Technology", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.OK)
                {
                    this.DialogResult = DialogResult.OK;
                    this.FormClosing -= frmPreferredCUs_FormClosing;
                }
                else
                {
                    this.DialogResult = DialogResult.None;
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        private void frmPreferredCUs_Resize(object sender, EventArgs e)
        {
            try
            {
                panel1.Height = this.Height - 45;
                //leftListBox.Top = 0;
                pnlCategory.Width = panel1.ClientSize.Width - 30;
                listboxPreferred.Width = (panel1.ClientSize.Width - 90) / 2;
                listboxPreferred.Height = panel1.Height - 130;
                btnToAvailable.Left = listboxPreferred.Right + 6;
                btnToPreferred.Left = listboxPreferred.Right + 6;
                btnToAvailableMany.Left = listboxPreferred.Right + 6;
                btnToPreferredMany.Left = listboxPreferred.Right + 6;
                pnlPreferred.Width = listboxPreferred.Width;
                pnlAvailable.Width = listboxPreferred.Width;
                listboxPreferred.Left = pnlPreferred.Left;
                listboxAvailable.Left = listboxPreferred.Right + 60;
                pnlAvailable.Left = listboxAvailable.Left;
                listboxAvailable.Width = (panel1.ClientSize.Width - 90) / 2;
                listboxAvailable.Height = panel1.Height - 130;
                btnCancel.Top = listboxAvailable.Bottom + 15;
                btnSave.Top = listboxAvailable.Bottom + 15;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
