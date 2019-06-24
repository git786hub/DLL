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
using Intergraph.GTechnology.API;


namespace GTechnology.Oncor.CustomAPI
{
    public partial class frmAddFeatHypLnk : Form
    {
        public frmAddFeatHypLnk()
        {
            InitializeComponent();
        }

        private void btHyperBrowse_Click(object sender, EventArgs e)
        {
          
            string tmpQry = string.Empty;
            Recordset rsFileTypeFilter = null;

            try
            {
                // Create a new instance of the open file dialog 
                ofdGetHypLnkFile = new OpenFileDialog();
                // modify the properties on the new instance of the open file dialog
                ofdGetHypLnkFile.Title = "Select Document";
               
                // add filters for the openJobDocument dialog.
                tmpQry = "select param_name, param_value from SYS_GENERALPARAMETER " +
                                "where subsystem_name = 'AttachFeatureDocumentCC' and Param_name = 'FILE_EXTENSIONS'";
                rsFileTypeFilter = csGlobals.gDatacont.OpenRecordset(tmpQry, CursorTypeEnum.adOpenStatic,
                                                                         LockTypeEnum.adLockReadOnly,
                                                                      (int)CommandTypeEnum.adCmdText);
                if (!(rsFileTypeFilter.BOF && rsFileTypeFilter.EOF))
                {
                    ofdGetHypLnkFile.Filter = rsFileTypeFilter.Fields[1].Value.ToString();
                }

                // show the open file dialog
                if (ofdGetHypLnkFile.ShowDialog() == DialogResult.OK)
                {
                    // set the file Text in the Add Feature Document dialog.
                    txtHyperlinkFile.Text = ofdGetHypLnkFile.FileName;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error in btHyperBrowse_Click: " + ex.Message,
                              "Attach Feature Document - frmAddFeatHypLnk",
                              MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        /// <summary>
        /// Load event for the Add Feature Document form.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void frmAddFeatHypLnk_Load(object sender, EventArgs e)
        {
             string[] SPFileStrArr = null;
             try
             {
                // Add items to the Type combo box on the Add Feature Document form.
                if (csGlobals.gSPFileTypeLst != "")
                {
                    SPFileStrArr = csGlobals.gSPFileTypeLst.Split(',');
                    for(int i = 0; i < SPFileStrArr.Length; ++i)
                    {
                        cbSPDocTyp.Items.Add(SPFileStrArr[i].ToString());
                    }
                }
                cbSPDocTyp.Text = cbSPDocTyp.Items[0].ToString();

                // create a new instance of the open file dialog box.
                ofdGetHypLnkFile = new OpenFileDialog();
                // ... set its properties.
                ofdGetHypLnkFile.Title = "Select Document";
                ofdGetHypLnkFile.Filter = csGlobals.gOpenFileFilterLst;
                ofdGetHypLnkFile.FilterIndex = 0;
                // Show the dialog.
                if (ofdGetHypLnkFile.ShowDialog() == DialogResult.OK )
                {
                    // put the selected file in the File text box on the Add Feature Document form.
                    txtHyperlinkFile.Text = ofdGetHypLnkFile.FileName;
                }
                else
                {
                    // if the cancel button was clicked on the open file dialog, close the Add Feature Document for
                    this.Close();
                }
             }
            catch(Exception ex)
            {
                MessageBox.Show("Error in frmAddFeatHypLnk_Load: " + ex.Message,
                              "Attach Feature Document - frmAddFeatHypLnk",
                              MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btAttach_Click(object sender, EventArgs e)
        {
            
            try
            {
                // show a message box 
                if (MessageBox.Show("Are you sure you want to attach this document?",
                              "Attach Feature Document",
                              MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    // if ok was clicked, do the following.
                    // add the file to SharePoint and add the hyperlink to the feature.
                    if (!csGlobals.gAddSPDocAndHyperLnk(txtDescr.Text, txtHyperlinkFile.Text, cbSPDocTyp.Text)) 
                    {
                        csGlobals.gApp.SetStatusBarText(GTStatusPanelConstants.gtaspcMessage, csGlobals.gMessage); 
                        //csGlobals.WaitFor(1);
                    }
                    else
                    {
                        csGlobals.gApp.SetStatusBarText(GTStatusPanelConstants.gtaspcMessage, csGlobals.gMessage);
                       // csGlobals.WaitFor(1);
                        this.Close();
                    }
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show("Error in btAttach_Click: " + ex.Message,
                              "Attach Feature Document - frmAddFeatHypLnk",
                              MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
