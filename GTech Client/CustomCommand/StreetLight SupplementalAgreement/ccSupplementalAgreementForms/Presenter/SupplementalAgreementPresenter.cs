using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Windows.Forms;
using System.Data;
using GTechnology.Oncor.CustomAPI.View;
using GTechnology.Oncor.CustomAPI.Model;
using Intergraph.GTechnology.API;
using ADODB;
using System.Text.RegularExpressions;
using System.Threading;


namespace GTechnology.Oncor.CustomAPI.Presenter
{
    public class SupplementalAgreementPresenter
    {
        private ISupplementalAgreementView m_view;
        private SupplementalAgreementModel m_model;
        private IGTCustomCommandHelper m_gTCustomCommandHelper;
        private IGTTransactionManager m_oGTTransactionManager;
        DataTable m_DataTable;

        private string m_NotifyPresenterMess;
        private bool m_isMSLAForm;
        private string m_ActiveWR;
        private string m_Customer;
        private DateTime? m_AgreementDate = DateTime.Now;
        public int m_Addition = 0;
        public int m_Removal = 0;

        public Form m_UserForm;
        public string NotifyPresenterMess
        {
            get
            {
                return m_NotifyPresenterMess;
            }
            set
            {
                m_NotifyPresenterMess = value;
            }
        }        
        public bool IsMSLAForm
        {
            get
            {
                return m_isMSLAForm;
            }
            set
            {
                m_isMSLAForm = value;
            }
        }       
        public string ActiveWR
        {
            get
            {
                return m_ActiveWR;
            }
            set
            {
                m_ActiveWR = value;
            }
        }        
        public string Customer { get { return m_Customer; } set
            { m_Customer = value; } }        
        public DataTable GridDataTable
        {
            get
            {
                m_DataTable = m_model.GridDataTable;
                m_Addition = m_model.Additions;
                m_Removal = m_model.Removal;
                return m_DataTable;
            }
            set
            {
                m_DataTable = value;
            }
        }        
        public DateTime? AgreementDate { get { return m_AgreementDate; } set { m_AgreementDate = value; } }

        public SupplementalAgreementPresenter()
        {

        }
        public SupplementalAgreementPresenter(ISupplementalAgreementView supplementalAgreementView, IGTCustomCommandHelper CustomCommandHelper
            ,IGTTransactionManager gTTransactionManager)
        {
            m_view = supplementalAgreementView;
            m_model = new Model.SupplementalAgreementModel(m_view.GTDataContext, m_view.GTDDCKeyObjects);
            
            m_gTCustomCommandHelper = CustomCommandHelper;
            m_oGTTransactionManager = gTTransactionManager;
        }

        /// <summary>
        /// To update the number of Additions and Removals.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void CellEndEvent(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                DataGridView dataGridView = m_UserForm.Controls.Find("dgvMSLA", true).FirstOrDefault() as DataGridView;

                //if(dataGridView.Columns[e.ColumnIndex].Name == "Action")
                //{
                //    string strCellValue = "";
                //    int count = 0;
                //    strCellValue = Convert.ToString(dataGridView.Rows[e.RowIndex].Cells[e.ColumnIndex].Value);
                //    if (!string.IsNullOrEmpty(strCellValue))
                //    {
                //        if (strCellValue == "A")
                //        {
                //            TextBox tbAddition = m_UserForm.Controls.Find("txtAddition", true).FirstOrDefault() as TextBox;
                //            count = Convert.ToInt32(tbAddition.Text);
                //            count = count + 1;
                //            tbAddition.Text = Convert.ToString(count); 
                //        }
                //        else if (strCellValue == "R")
                //        {
                //            TextBox tbRemovalA = m_UserForm.Controls.Find("txtRemoval", true).FirstOrDefault() as TextBox;
                //            count = Convert.ToInt32(tbRemovalA.Text);
                //            count = count + 1;
                //            tbRemovalA.Text = Convert.ToString(count);
                //        }
                //    }
                //}

                m_Addition = 0;
                m_Removal = 0;

                int datarowCount = dataGridView.RowCount;
                DataGridViewRowCollection dataGridViewRowCollection = dataGridView.Rows;
                string strAction = "";

                for (int i = 0; i < datarowCount; i++)
                {
                    strAction = Convert.ToString(dataGridViewRowCollection[i].Cells["Action"].Value);
                    if (!string.IsNullOrEmpty(strAction))
                    {
                        if (strAction == "A")
                        {
                            m_Addition = m_Addition + 1;
                        }
                        else if (strAction == "R")
                        {
                            m_Removal = m_Removal + 1;
                        }
                    }
                }
            }
            catch
            {

            }
        }

        /// <summary>
        /// To delete the row in datagridview.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void dgvKeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if(e.KeyCode == Keys.Delete)
                {
                    DataGridView dataGridView = m_UserForm.Controls.Find("dgvMSLA", true).FirstOrDefault() as DataGridView;
                    if(dataGridView.SelectedRows.Count == 1)
                    {
                        dataGridView.Rows.RemoveAt(dataGridView.SelectedRows[0].Index);
                    }

                    m_Addition = 0;
                    m_Removal = 0;

                    int datarowCount = dataGridView.RowCount;
                    DataGridViewRowCollection dataGridViewRowCollection = dataGridView.Rows;
                    string strAction = "";

                    for (int i = 0; i < datarowCount; i++)
                    {
                        strAction = Convert.ToString(dataGridViewRowCollection[i].Cells["Action"].Value);
                        if (!string.IsNullOrEmpty(strAction))
                        {
                            if (strAction == "A")
                            {
                                m_Addition = m_Addition + 1;
                            }
                            else if (strAction == "R")
                            {
                                m_Removal = m_Removal + 1;
                            }
                        }
                    }
                }
            }
            catch
            {
                throw;
            }
        }
      
        /// <summary>
        /// To generate the html template.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void btnGenerateClik(object sender, EventArgs e)
        {
            String strFilePath = "";
            string strJobDoumentsPath = "";
            IGTKeyObject gTDesignAreaKeyObject = null;
            List<string> lstImages = null;
            List<string> lstDataRows = null;
            string strCommentsRow = null;
            ProcessHtml processHtml = new ProcessHtml();
           
            try
            {              
                strJobDoumentsPath = Path.GetTempPath();
                gTDesignAreaKeyObject = m_model.DesignAreaKeyObject;
                if (gTDesignAreaKeyObject != null)
                {
                   
                    System.IO.StreamReader objReader;
                    if (IsMSLAForm)
                    {
                        strFilePath = m_model.GetFormTemplateLocation("StreetLightWithMSLA_URL");
                    }
                    else
                    {
                        strFilePath = m_model.GetFormTemplateLocation("StreetLightWithWithOutMSLA_URL");
                    }
                    if (!string.IsNullOrEmpty(strFilePath))
                    {

                        objReader = new System.IO.StreamReader(strFilePath, Encoding.Default);
                        string strContent = objReader.ReadToEnd();
                        objReader.Close();

                        lstImages = processHtml.GetImagesInHtmlString(strContent);

                        strContent = processHtml.ReplaceImagesTagsWithLabels(lstImages, strContent);

                        lstDataRows = processHtml.GetRowsOfGridInHtmlString(strContent);

                        string strHtmlFinalRow = "";
                        string strHtmlRow = "";

                        processHtml.ReplaceTableWithGridValues(lstDataRows, ref strHtmlFinalRow, ref strHtmlRow, m_UserForm.Controls.Find("dgvMSLA", true).FirstOrDefault() as DataGridView);

                        if (!string.IsNullOrEmpty(strHtmlFinalRow))
                        {
                            strContent = strContent.Replace(lstDataRows[0], strHtmlFinalRow);
                        }
                        else
                        {
                            strContent = strContent.Replace(lstDataRows[0], "<tr for='DataRows' style='mso-yfti-irow:1;mso-yfti-lastrow:yes'></tr>");
                        }

                        strContent = strContent.Replace("[[Active WR]]", ActiveWR);
                        strContent = strContent.Replace("[[Active Customer]]", Customer);
                        if (!string.IsNullOrEmpty(Convert.ToString(m_model.AgreementDate)))
                        {
                            string format = "MMMM dd, yyyy";
                            DateTime dt = m_model.AgreementDate.Value;
                            strContent = strContent.Replace("[[Active Agreement]]", dt.ToString(format));
                        }
                        strContent = strContent.Replace("[[Active Customer Description]]", Customer);
                        strContent = strContent.Replace("[[Active City]]", Customer);

                        DateTimePicker dateTimePicker = m_UserForm.Controls.Find("dtPicker", true).FirstOrDefault() as DateTimePicker;
                        TextBox textBoxCA = m_UserForm.Controls.Find("txtCustomerAgent", true).FirstOrDefault() as TextBox;
                        TextBox textBoxCost = m_UserForm.Controls.Find("txtCost", true).FirstOrDefault() as TextBox;
                        TextBox textBoxComments = m_UserForm.Controls.Find("txtComments", true).FirstOrDefault() as TextBox;

                        if (dateTimePicker != null)
                        {
                            strContent = strContent.Replace("[[Active Month]]", 
                                Convert.ToString(Thread.CurrentThread.CurrentCulture.DateTimeFormat.MonthNames[dateTimePicker.Value.Month - 1]));
                            

                            string suffix = "";
                            switch (dateTimePicker.Value.Day)
                            {
                                case 1:
                                case 21:
                                case 31:
                                    suffix = "st";
                                    break;
                                case 2:
                                case 22:
                                    suffix = "nd";
                                    break;
                                case 3:
                                case 23:
                                    suffix = "rd";
                                    break;
                                default:
                                    suffix = "th";
                                    break;
                            }
                            strContent = strContent.Replace("[[Active Day]]", Convert.ToString(dateTimePicker.Value.Day) + suffix);

                            strContent = strContent.Replace("[[Active Year]]", Convert.ToString(dateTimePicker.Value.Year));

                            DateTime oDate = dateTimePicker.Value;
                            
                            strContent = strContent.Replace("[[Active Service Date]]", oDate.ToString("MMMM dd, yyyy"));
                        }

                        if (textBoxCA != null)
                        {
                            strContent = strContent.Replace("[[Active Customer Agent]]", textBoxCA.Text);
                        }

                        if (textBoxCost != null)
                        {
                            strContent = strContent.Replace("[[ACost]]", textBoxCost.Text);
                        }

                        if (textBoxComments != null)
                        {
                            string str = textBoxComments.Text;
                            string sCommentR = @"<tr> 
  <td width=69 style='width:52.1pt;border-top:none;border-left:solid black 2.25pt;
  border-bottom:solid black 1.0pt; padding: 0cm 6.0pt 0cm 6.0pt'>
      <p class=MsoNormal align =center style='margin-bottom:2.9pt;text-align:center'><i><span
         lang = EN-US ><b>Comments:</b></span></i></p>
  </td>  
 <td colspan='9' width=69 style='width:52.1pt;border-top:none;
  border-bottom:solid black 1.0pt;padding:0cm 6.0pt 0cm 6.0pt;border-right:solid black 2.25pt;padding:0cm 6.0pt 0cm 6.0pt'>
  <p class=MsoNormal align = center style='margin-bottom:2.9pt;text-align:left'><i><span
    lang = EN-US >[[ActiveComments]]</span></i></p>
  </td> 
 </tr>";
                            if(!string.IsNullOrEmpty(str))
                            {
                                if(str.Contains("\r\n"))
                                {
                                    str = str.Replace("\r\n", "</br>");
                                }
                                else if (str.Contains("\n"))
                                {
                                    str = str.Replace("\r\n", "</br>");
                                }
                            }
                            strCommentsRow = processHtml.GetRowofComments(strContent);
                            strContent = strContent.Replace(strCommentsRow, sCommentR);
                            strContent = strContent.Replace("[[ActiveComments]]", str);
                        }

                        TextBox textBoxAdd = m_UserForm.Controls.Find("txtAddition", true).FirstOrDefault() as TextBox;
                        TextBox textBoxRemo = m_UserForm.Controls.Find("txtRemoval", true).FirstOrDefault() as TextBox;

                        if (textBoxAdd != null)
                        {
                            strContent = strContent.Replace("[[AADD]]", textBoxAdd.Text);
                        }

                        if (textBoxRemo != null)
                        {
                            strContent = strContent.Replace("[[ARemove]]", textBoxRemo.Text);
                        }

                        string strFileName = ActiveWR + "-" + Customer + ".html";
                        strJobDoumentsPath = strJobDoumentsPath + "\\" + ActiveWR;
                        strJobDoumentsPath = AttachFileRecursive(strJobDoumentsPath, gTDesignAreaKeyObject, strContent, ref strFileName);

                        if (!string.IsNullOrEmpty(strJobDoumentsPath + "\\" + strFileName))
                        {
                            System.Diagnostics.Process.Start(strJobDoumentsPath + "\\" + strFileName);                            
                        }
                    }
                }
                else
                {
                    MessageBox.Show("Active Job does not have a Design Area.", "G/Technology", MessageBoxButtons.OK, MessageBoxIcon.Information,
                        MessageBoxDefaultButton.Button1);
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message, "G/Technology", MessageBoxButtons.OK, MessageBoxIcon.Error,
                        MessageBoxDefaultButton.Button1);
            }
        }

       
        /// <summary>
        /// Check the html file name for existence and attach the html file to the current job.
        /// </summary>
        /// <param name="strJobDoumentsPath">Document Path</param>
        /// <param name="gTDesignAreaKeyObject">Document Path</param>
        /// <param name="strContent">Content of the html document</param>
        /// <param name="strFileName">File name of the html document</param>
        /// <returns></returns>
        private string AttachFileRecursive(string strJobDoumentsPath, IGTKeyObject gTDesignAreaKeyObject, string strContent,ref string strFileName)
        {                        
            if (!m_model.IsExistingAttachment(strJobDoumentsPath + "\\" + strFileName))
            {               
                AttachFile(strJobDoumentsPath, gTDesignAreaKeyObject, strContent, strFileName,false);                
            }
            else
            {
                frmOverwriteDocument OverWriteform = new frmOverwriteDocument();
                DialogResult dialogResult = OverWriteform.ShowDialog(this.m_UserForm);
                if (dialogResult == DialogResult.Ignore)
                {
                    AttachFile(strJobDoumentsPath, gTDesignAreaKeyObject, strContent, strFileName ,true);
                }
                else if (dialogResult == DialogResult.OK)
                {
                    strFileName = OverWriteform.strResult + ".html";
                    AttachFileRecursive(strJobDoumentsPath, gTDesignAreaKeyObject, strContent, ref strFileName);
                }
            }

            return strJobDoumentsPath;
        }

        /// <summary>
        /// Attach html file to the active job.
        /// </summary>
        /// <param name="strJobDoumentsPath">Document Path</param>
        /// <param name="gTDesignAreaKeyObject">Document Path</param>
        /// <param name="strContent">Content of the html document</param>
        /// <param name="strFileName">File name of the html document</param>
        private void AttachFile(string strJobDoumentsPath, IGTKeyObject gTDesignAreaKeyObject, string strContent, string strFileName, bool overRide)
        {
            
            try
            {
                if (!Directory.Exists(strJobDoumentsPath))
                    Directory.CreateDirectory(strJobDoumentsPath);
                File.WriteAllText(strJobDoumentsPath + "\\" + strFileName, strContent);
            }
            catch
            {

            }

            string strDescription = ActiveWR + "-" + Customer + "-" + "HTML DOCUMENT";

            if (overRide)
            {
                m_oGTTransactionManager.Begin("Supplemental Agreement Forms");
                m_model.DeleteExistingAttachment(strJobDoumentsPath + "\\" + strFileName);
                if (m_oGTTransactionManager.TransactionInProgress) m_oGTTransactionManager.Commit();
            }

            m_oGTTransactionManager.Begin("Supplemental Agreement Forms");
            IGTKeyObject gTTempKeyObject = m_view.GTDataContext.OpenFeature(gTDesignAreaKeyObject.FNO, gTDesignAreaKeyObject.FID);
            Recordset rs = gTTempKeyObject.Components.GetComponent(8130).Recordset;

            rs.AddNew("G3E_FID", gTTempKeyObject.FID);
            rs.Fields["HYPERLINK_T"].Value = strJobDoumentsPath + "\\" + strFileName;
            rs.Fields["DESCRIPTION_T"].Value = strDescription;
            rs.Fields["TYPE_C"].Value = "SUPPLEMENT";
            rs.Fields["G3E_FNO"].Value = 8100;
            rs.Update();
            if (m_oGTTransactionManager.TransactionInProgress) m_oGTTransactionManager.Commit();

            MessageBox.Show("The Street Light Supplemental Agreement Form was generated and attached to the WR.", "G/Technology",
                MessageBoxButtons.OK, MessageBoxIcon.Information);
                                 
        }

        /// <summary>
        /// Verify  the CC is valid or not.
        /// </summary>
        /// <returns></returns>
        public bool IsCCommandValid()
        {            
            m_view.IsCommandValid = m_model.IsCommandValid();              
            NotifyPresenterMess = m_model.NotifyModelMess;
            if (m_view.IsCommandValid)
            {
                IsMSLAForm = m_model.IsMSLAExist();
            }
            ActiveWR = m_model.ActiveWorkRequest;
            Customer = m_model.Customer;
            AgreementDate = m_model.AgreementDate;
           
            return m_view.IsCommandValid;
        }
        public void CompleteCC()
        {
            if (m_gTCustomCommandHelper != null)
            {
                m_gTCustomCommandHelper.Complete();
                m_gTCustomCommandHelper = null;
            }
        }


    }
}
