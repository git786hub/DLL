// ===================================================
//  Copyright 2018 Intergraph Corp.
//  File Name: SaveViewForm.cs
// 
//  Description: SaveViewForm form is used to save the named view.
//
//  Remarks:
// 
//  Modification History
//  Date                Author                      Comments
//  --------            --------------              --------
//  09/01/2018          Sithara                     
// ======================================================
using System;
using System.Windows.Forms;
using Intergraph.GTechnology.API;
using ADODB;

namespace GTechnology.Oncor.CustomAPI
{
    public partial class SaveViewForm : Form
    {
        IGTDataContext m_gTDataContext;
        DataGridViewColumnSelector m_dataGridViewColumnSelector;        
        public SaveViewForm(IGTDataContext gTDataContext, DataGridViewColumnSelector dataGridViewColumnSelector)
        {
            InitializeComponent();
            m_gTDataContext = gTDataContext;
            m_dataGridViewColumnSelector = dataGridViewColumnSelector;
        }

        /// <summary>
        /// Inserts named view data into the tables  ASSET_HISTORY_VIEW,ASSET_HISTORY_VIEWFILTER,ASSET_HISTORY_VIEWSORT
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSave_Click(object sender, EventArgs e)
        {
            int viewId = 0;
            Recordset rs = null;
            try
            {
                string sql = "";
                if(!string.IsNullOrEmpty(txtViewname.Text))
                {
                   
                    sql = "select max(view_id)+1 from ASSET_HISTORY_VIEW";
                    rs = m_gTDataContext.OpenRecordset(sql, ADODB.CursorTypeEnum.adOpenDynamic, ADODB.LockTypeEnum.adLockReadOnly, (int)ADODB.CommandTypeEnum.adCmdText,
                        null);

                    if (rs != null && rs.RecordCount > 0)
                    {
                        rs.MoveFirst();
                        if (string.IsNullOrEmpty(Convert.ToString(rs.Fields[0].Value)))
                        {
                            viewId = 1;
                        }
                        else
                        {
                            viewId = Convert.ToInt32(rs.Fields[0].Value);
                        }
                    }

                    rs = null;
                    sql = "select count(*) from ASSET_HISTORY_VIEW where view_nm = :1";
                    rs = m_gTDataContext.OpenRecordset(sql, ADODB.CursorTypeEnum.adOpenDynamic, ADODB.LockTypeEnum.adLockReadOnly, (int)ADODB.CommandTypeEnum.adCmdText,
                      new object[] { txtViewname.Text });

                    if (rs != null && rs.RecordCount > 0)
                    {
                        rs.MoveFirst();
                        if (Convert.ToInt32(rs.Fields[0].Value) > 0)
                        {
                            if (MessageBox.Show(GTClassFactory.Create<IGTApplication>().ApplicationWindow, "View name already exist in database. Do you want to replace it?", "G/Technology", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                            {
                                sql = null;
                                sql = string.Format("DELETE FROM ASSET_HISTORY_VIEWFILTER where VIEW_ID = (SELECT VIEW_ID FROM ASSET_HISTORY_VIEW WHERE VIEW_NM='{0}')",txtViewname.Text);
                                ExecuteCommand(sql);

                                sql = string.Format("DELETE FROM ASSET_HISTORY_VIEWSORT where VIEW_ID = (SELECT VIEW_ID FROM ASSET_HISTORY_VIEW WHERE VIEW_NM='{0}')", txtViewname.Text);
                                ExecuteCommand(sql);

                                sql = string.Format("DELETE FROM ASSET_HISTORY_VIEW where VIEW_NM = '{0}'", txtViewname.Text);
                                ExecuteCommand(sql);

                                sql = string.Format("commit", txtViewname.Text);
                                ExecuteCommand(sql);

                                sql = SaveViewWithGivenName(viewId);
                                this.DialogResult = DialogResult.OK;
                            }
                            else
                            {
                                this.DialogResult = DialogResult.None;
                            }
                        }
                        else
                        {
                            sql = SaveViewWithGivenName(viewId);
                            this.DialogResult = DialogResult.OK;
                        }
                    }
                   
                }
                else
                {
                    MessageBox.Show("View name should not be empty.","G/Techonology", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    this.DialogResult = DialogResult.None;
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show("Error during execution of Review Asset History custom command." + Environment.NewLine + ex.Message,
                    "G/Techonology", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {                
                if (rs != null)
                {
                    rs.Close();
                    rs = null;
                }
            }
        }        
        private string SaveViewWithGivenName(int viewId)
        {
            string sql = m_dataGridViewColumnSelector.BuildSaveQuery(viewId, txtViewname.Text);
            if (!string.IsNullOrEmpty(sql))
            {
                ExecuteCommand(sql);
                MessageBox.Show("View is Saved.", "G/Techonology", MessageBoxButtons.OK, MessageBoxIcon.Information);                
            }

            return sql;
        }

        private int ExecuteCommand(string sql)
        {
            ADODB.Recordset results = null;
            try
            {
                int outRecords = 0;
                ADODB.Command command = new ADODB.Command();
                command.CommandText = sql;
                results = m_gTDataContext.ExecuteCommand(command, out outRecords);
                return outRecords;
            }
            catch
            {
                throw;
            }         

        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void txtViewname_TextChanged(object sender, EventArgs e)
        {
            try
            {
                if(!string.IsNullOrEmpty(txtViewname.Text))
                {
                    if(IsSpecialCharacter(txtViewname.Text))
                    {
                        txtViewname.Text = null;
                        MessageBox.Show("Please enter numeric value.", "G/Techonology", MessageBoxButtons.OK, MessageBoxIcon.Warning,
MessageBoxDefaultButton.Button1);
                    }
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show("Error during execution of Review Asset History custom command." + Environment.NewLine + ex.Message,
                  "G/Techonology", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private bool IsSpecialCharacter(string viewText)
        {
            bool IsSpecialChar = false;
            System.Text.RegularExpressions.Regex regex = new System.Text.RegularExpressions.Regex("^[a-zA-Z0-9_]+$");
            try
            {
                System.Text.RegularExpressions.MatchCollection matches = regex.Matches(viewText);
                if(matches.Count <= 0)
                {
                    IsSpecialChar = true;
                }
            }
            catch
            {
                throw;
            }

            return IsSpecialChar;
        }
    }
}
