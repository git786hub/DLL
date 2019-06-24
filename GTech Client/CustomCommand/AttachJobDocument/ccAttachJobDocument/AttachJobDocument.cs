using System;
using System.Collections.Generic;
using System.Windows.Forms;
using Intergraph.GTechnology.API;
using System.IO;
using ADODB;

namespace GTechnology.Oncor.CustomAPI
{
	public partial class AttachJobDocument : Form
	{
		#region Variables

		private IGTApplication m_igtApplication;
		private IGTTransactionManager m_igtranscation;
		private int m_designAreaFID;
		private CommonMessages m_commonMessages;
        private string SPTypes = string.Empty;
        private string FileExtensions = string.Empty;

		#endregion Variables

		#region Constructors
		public AttachJobDocument()
		{
			InitializeComponent();
		}

		public AttachJobDocument(IGTApplication m_igtApplication, int m_designAreaFID, IGTTransactionManager m_igtranscation)
		{
			InitializeComponent();
			this.m_igtApplication = m_igtApplication;
			this.m_designAreaFID = m_designAreaFID;
			this.m_igtranscation = m_igtranscation;
			m_commonMessages = new CommonMessages();
            getCommandParams();
            //addFileTypes();

        }
		#endregion Constructors


		#region Events
		private void btnSelect_Click(object sender, EventArgs e)
		{
            string tmpQry = string.Empty;
            Recordset rsFileTypeFilter = null;

            try
			{
				openJobDcoument = new OpenFileDialog();
				openJobDcoument.Title = "Select Document";

                // add filters for the openJobDocument dialog.
                tmpQry = "select param_name, param_value from SYS_GENERALPARAMETER " +
                                "where subsystem_name = 'AttachJobDocumentCC' and Param_name = 'FILE_EXTENSIONS'";
                rsFileTypeFilter = m_igtApplication.DataContext.OpenRecordset(tmpQry, CursorTypeEnum.adOpenStatic,
                                                                         LockTypeEnum.adLockReadOnly,
                                                                      (int)CommandTypeEnum.adCmdText);
                if (!(rsFileTypeFilter.BOF && rsFileTypeFilter.EOF))
                {
                    openJobDcoument.Filter = rsFileTypeFilter.Fields[1].Value.ToString();
                }

				if (openJobDcoument.ShowDialog() == DialogResult.OK)
				{
					txtFile.Text = openJobDcoument.FileName;
				}
			}
			catch (Exception)
			{
				throw;
			}

		}

		private void btnCancel_Click(object sender, EventArgs e)
		{
			this.Close();
		}

		private void btnAttach_Click(object sender, EventArgs e)
		{
            OncDocManage.OncDocManage SpDocMan = new OncDocManage.OncDocManage();
            Recordset tmpRs = null;
            string tmpQry = string.Empty;

			try
			{
				//File validation
				if (string.IsNullOrEmpty(txtFile.Text) || !File.Exists(txtFile.Text))
				{
					MessageBox.Show(m_igtApplication.ApplicationWindow,"Valid file is not selected.", "G/Technology", MessageBoxButtons.OK, MessageBoxIcon.Information);
				}
				else
				{
					UpdateStatus("Attach Job Document process started.");
					DialogResult dialogResult = MessageBox.Show(m_igtApplication.ApplicationWindow,m_commonMessages.AttachDialog, "G/Technology", MessageBoxButtons.OKCancel, MessageBoxIcon.Information);
					if (dialogResult == DialogResult.OK)
					{
						IGTKeyObject m_designAreaObj;
						#region Adding Hyperlink
						try
						{
                            try
                            {
                                UpdateStatus("Adding the file to SharePoint");
                                m_igtApplication.BeginWaitCursor();
                                // add file to sharepoint.
                                // get the WR_NBR
                                tmpQry = "Select j.WR_NBR from DESIGNAREA_P da, g3e_job j " +
                                          "where da.JOB_ID = j.G3E_IDENTIFIER and da.g3e_fid = " + m_designAreaFID.ToString();
                                tmpRs = m_igtApplication.DataContext.OpenRecordset(tmpQry, CursorTypeEnum.adOpenStatic, 
                                                                                   LockTypeEnum.adLockReadOnly, 
                                                                                   (int)CommandTypeEnum.adCmdText);
                                if (!(tmpRs.BOF && tmpRs.EOF))
                                {
                                    tmpRs.MoveFirst();
                                    SpDocMan.WrkOrd_Job = tmpRs.Fields[0].Value.ToString();
                                    tmpRs = null;
                                }
                                else
                                {
                                    MessageBox.Show(m_igtApplication.ApplicationWindow, "The WR number was not found in the Job table.", "Attach Job Document - Error ", MessageBoxButtons.OK);
                                    if (tmpRs != null) tmpRs = null;
                                    return;
                                }

                                // Get the Document Management metadata parameters.
                                tmpQry = "select param_name, param_value from SYS_GENERALPARAMETER " +
                                         "where subsystem_name = 'Doc_Management'";
                                tmpRs = m_igtApplication.DataContext.OpenRecordset(tmpQry, CursorTypeEnum.adOpenStatic, 
                                                                                   LockTypeEnum.adLockReadOnly, 
                                                                                 (int)CommandTypeEnum.adCmdText);
                                // Set the OncDocManage class properties 
                                if (!(tmpRs.BOF && tmpRs.EOF))
                                {
                                    tmpRs.MoveFirst();
                                    for (int i = 0; i < tmpRs.RecordCount; ++i)
                                    {
                                        if (tmpRs.Fields["PARAM_NAME"].Value.ToString() == "ROOT_PATH")
                                            SpDocMan.SPRootPath = tmpRs.Fields["PARAM_VALUE"].Value.ToString();
                                        if (tmpRs.Fields["PARAM_NAME"].Value.ToString() == "SP_URL")
                                            SpDocMan.SPSiteURL = tmpRs.Fields["PARAM_VALUE"].Value.ToString();
                                        if (tmpRs.Fields["PARAM_NAME"].Value.ToString() == "JOBWO_REL_PATH")
                                            SpDocMan.SPRelPath = tmpRs.Fields["PARAM_VALUE"].Value.ToString();
                                        tmpRs.MoveNext();
                                    }
                                }
                                SpDocMan.SPFileDescription = txtDescription.Text;
                                SpDocMan.SrcFilePath = txtFile.Text;
                                SpDocMan.SPFileType = cmbType.Text;
                                // Add the file to SharePoint.
                                if (!SpDocMan.AddSPFile(true))
                                {
                                    System.ArgumentException SPExcept = new System.ArgumentException("File was not saved in SharePoint.");
                                }

                            }
                            catch (Exception ex)
                            {
                                m_igtApplication.EndWaitCursor();
                                MessageBox.Show(m_igtApplication.ApplicationWindow, "Unable to copy the file to SharePoint. Error: " + ex.Message, "G/Technology", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                return;
                            }
                            // Add the hyperlink to the Design Area feature.
                            UpdateStatus("Adding the Hyperlink to the Design Area.");
                            // Begin the GTech transaction.
                            m_igtranscation.Begin("IN PROGRESS");
                            // open the Design Area feature.
							m_designAreaObj = m_igtApplication.DataContext.OpenFeature(8100, m_designAreaFID);
							m_designAreaObj.Components["JOB_HYPERLINK_N"].Recordset.AddNew("G3E_FID", m_designAreaFID);
                            m_designAreaObj.Components["JOB_HYPERLINK_N"].Recordset.Fields["HYPERLINK_T"].Value = SpDocMan.RetFileURL;
							m_designAreaObj.Components["JOB_HYPERLINK_N"].Recordset.Fields["DESCRIPTION_T"].Value = txtDescription.Text;
                            // Add a new hyperlink component instance
                            if (cmbType.Text.Length > m_designAreaObj.Components["JOB_HYPERLINK_N"].Recordset.Fields["TYPE_C"].DefinedSize)
                            {
                                m_designAreaObj.Components["JOB_HYPERLINK_N"].Recordset.Fields["TYPE_C"].Value = 
                                    cmbType.Text.Substring(0, m_designAreaObj.Components["JOB_HYPERLINK_N"].Recordset.Fields["TYPE_C"].DefinedSize);
                            }
                            else
                            {
                                m_designAreaObj.Components["JOB_HYPERLINK_N"].Recordset.Fields["TYPE_C"].Value = cmbType.Text;
                            }
           
                            // Add the file name to the hyperlink table.
                            m_designAreaObj.Components["JOB_HYPERLINK_N"].Recordset.Fields["FILENAME_T"].Value = SpDocMan.RetFileName;
         
                            m_designAreaObj.Components["JOB_HYPERLINK_N"].Recordset.Fields["G3E_FNO"].Value = 8100;
							m_designAreaObj.Components["JOB_HYPERLINK_N"].Recordset.Update();
                            // end the GTech transaction.
							if (m_igtranscation.TransactionInProgress) m_igtranscation.Commit();
							MessageBox.Show(m_igtApplication.ApplicationWindow,"Attach Job Document Completed sucessfully.", "G/Technology", MessageBoxButtons.OK, MessageBoxIcon.Information);
							UpdateStatus("Attach Job Document Completed sucessfully.");
							this.Close();

						}
						catch (Exception ex)
						{
                            // if the GTec transaction fails, rollback the edit and exit the command.
                            tmpQry = ex.Message;
                            if (m_igtranscation.TransactionInProgress)
                            {
                                m_igtranscation.Rollback();
                            }
                            m_igtApplication.EndWaitCursor();
                            throw;
						}
						finally
						{
							m_designAreaObj = null;
						}
                        m_igtApplication.EndWaitCursor();
                        #endregion Adding Hyperlink
                    }
				}
			}
			catch (Exception)
			{
                m_igtApplication.EndWaitCursor();
                throw;
			}
			
		}

		
		private void StrandPortFrm_KeyDown(object sender, KeyEventArgs e)
		{
			try
			{
				if ((Keys)e.KeyValue == Keys.Escape)
				{
					this.Close();
				}
			}
			catch (Exception)
			{
				throw;
			}
		}
		
		#endregion Events

		#region Methods



        private void getCommandParams()
        {
            Recordset rsParams = null;
            
            string tmpQry = "select param_name, param_value from gis_onc.SYS_GENERALPARAMETER " +
                                    "where subsystem_name = 'AttachJobDocumentCC'";
            string tmpStr = string.Empty;

            try
            {
                // Add entries in the combo box.
                rsParams = m_igtApplication.DataContext.OpenRecordset(tmpQry, CursorTypeEnum.adOpenStatic,
                                                                         LockTypeEnum.adLockReadOnly,
                                                                         (int)CommandTypeEnum.adCmdText);
                if (!(rsParams.BOF && rsParams.EOF))
                {
                    rsParams.MoveFirst();

                    for (int i = 0; i < rsParams.RecordCount; ++i)
                    {
                        switch (rsParams.Fields[0].Value.ToString())
                        {
                            case "FILE_TYPES":
                                SPTypes = rsParams.Fields[1].Value.ToString();
                                break;
                            case "FILE_EXTENSIONS":
                                FileExtensions = rsParams.Fields[1].Value.ToString();
                                break;
                        }
                        rsParams.MoveNext();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(m_igtApplication.ApplicationWindow, "getCommandParams - Error: " + ex.Message, "Error: Attach Job Document", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


		/// <summary>
		/// add file types to combobox
		/// </summary>
		private void addFileTypes()
		{
			string tmpStr = string.Empty;
            string[] tmpSpFileTypes = null;

            try
            {
                tmpStr = SPTypes;
                tmpSpFileTypes = tmpStr.Split(',');
                cmbType.Items.Clear();

                for (int i = 0; i < tmpSpFileTypes.Length; ++i)
                {
                    cmbType.Items.Add(tmpSpFileTypes[i].ToString());
                }

                cmbType.Text = cmbType.Items[0].ToString();
            }
			catch (Exception)
			{
				throw;
			}			
		}

		/// <summary>
		/// Update the status bar
		/// </summary>
		/// <param name="strMsg"></param>
		private void UpdateStatus(string strMsg)
		{
			m_igtApplication.SetStatusBarText(GTStatusPanelConstants.gtaspcMessage, strMsg);
		}


        #endregion Methods

        private void AttachJobDocument_Load(object sender, EventArgs e)
        {
            try
            {
               // getCommandParams();
                addFileTypes();
                openJobDcoument = new OpenFileDialog();
                openJobDcoument.Title = "Select Document";
                openJobDcoument.Filter = FileExtensions;
                openJobDcoument.FilterIndex = 0;
                if (openJobDcoument.ShowDialog() == DialogResult.OK)
                {
                    // put the selected file in the File text box on the Add Feature Document form.
                    txtFile.Text = openJobDcoument.FileName;
                }
                else
                {
                    // if the cancel button was clicked on the open file dialog, close the Add Feature Document for
                    this.Close();
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show(m_igtApplication.ApplicationWindow, "AttachJobDocument_Load - Error: " + ex.Message, "Error: Attach Job Document", MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.Close();
            }
        }
    }
}
