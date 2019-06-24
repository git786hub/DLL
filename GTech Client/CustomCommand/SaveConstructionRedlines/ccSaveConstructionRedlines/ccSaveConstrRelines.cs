using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Intergraph.GTechnology.Interfaces;
using Intergraph.GTechnology.API;
using ADODB;
using System.Windows.Forms;

namespace GTechnology.Oncor.CustomAPI
{
    public class ccSaveConstRedlines : IGTCustomCommandModal
    {
        private IGTTransactionManager TransManager = null;
        private IGTApplication gtApp = null;

        IGTTransactionManager IGTCustomCommandModal.TransactionManager
        {
            set
            {
                TransManager = value;
            }
        }

        void IGTCustomCommandModal.Activate()
        {
            int tmpDesAreaFid = 0;
            string WRNumber = string.Empty;
            gtApp = GTClassFactory.Create<IGTApplication>();

            gtApp.SetStatusBarText(GTStatusPanelConstants.gtaspcMessage, "Saving Construction Redline file." );
            if (IsWRJob(ref WRNumber) == false) return;
            if (DoesDesignAreaExist(ref tmpDesAreaFid) == false) return;
            SaveXMLAddHyperlink(WRNumber, tmpDesAreaFid);
            gtApp.SetStatusBarText(GTStatusPanelConstants.gtaspcMessage, "Saving Construction Redline file ended.");
        }

        private Boolean IsWRJob(ref string p_WR_NBR)
        {
            Boolean tmpRetVal = false;
            Recordset tmpRs = null;
            string tmpQry = string.Empty;
            IGTDataContext tmpDC = gtApp.DataContext;

            try
            {

                tmpQry = "select g3e_jobtype, wr_nbr from g3e_job " +
                           "where g3e_identifier = '" + tmpDC.ActiveJob + "'";
                tmpRs = tmpDC.OpenRecordset(tmpQry, CursorTypeEnum.adOpenStatic,
                                            LockTypeEnum.adLockReadOnly, (int)CommandTypeEnum.adCmdText);
                if (! (tmpRs.BOF && tmpRs.EOF))
                {
                    tmpRs.MoveFirst();
                    if ( tmpRs.Fields[0].Value == "NON-WR")
                    {
                        tmpRetVal = false;
                        gtApp.SetStatusBarText(GTStatusPanelConstants.gtaspcMessage, "Saving Construction Redline file ended.");
                        MessageBox.Show("Error: The active Job is a 'NON-WR' job. \n Please use a job associated to a Work Order.",
                                        "IsWRJob - Error", MessageBoxButtons.OK);

                    }
                    else
                    {
                        p_WR_NBR = tmpRs.Fields[1].Value.ToString();
                        tmpRetVal = true;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message, "IsWRJob - Error", MessageBoxButtons.OK);
                tmpRetVal = false;
            }
            return tmpRetVal;
        }


        private Boolean DoesDesignAreaExist(ref Int32 p_DesAreaFid)
        {
            Boolean tmpRetVal = false;
            Recordset tmpRs = null;
            string tmpQry = string.Empty;
            string tmpJob = string.Empty;
            IGTDataContext tmpDC = gtApp.DataContext;

            try
            {
                tmpQry = "select g3e_fid from DESIGNAREA_P " +
                            "where job_id = '" + tmpDC.ActiveJob + "'";
                tmpRs = tmpDC.OpenRecordset(tmpQry, CursorTypeEnum.adOpenStatic,
                                            LockTypeEnum.adLockReadOnly, (int)CommandTypeEnum.adCmdText);
                if (!(tmpRs.BOF && tmpRs.EOF))
                {
                    tmpRs.MoveFirst();
                    p_DesAreaFid = Convert.ToInt32(tmpRs.Fields[0].Value.ToString());
                    tmpRetVal = true;
                }
                else
                {
                    gtApp.SetStatusBarText(GTStatusPanelConstants.gtaspcMessage, "Saving Construction Redline file ended.");
                    MessageBox.Show("Error: The active Job does not have a Design Area associated to it. \n" +
                                    "Please create a Design Area for this Job and try again.",
                                    "DoesDesignAreaExist - Error", MessageBoxButtons.OK);
                    tmpRetVal = false;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message, "DoesDesignAreaExist - Error", MessageBoxButtons.OK);
                tmpRetVal = false;
            }
            return tmpRetVal;
        }

        private void SaveXMLAddHyperlink(string p_WrNbr,int p_DesArFid)
        {
            string LocRedLineFile = string.Empty;
            string tmpWrNum = string.Empty;
            OncDocManage.OncDocManage tmpDocMg = new OncDocManage.OncDocManage();

            try
            {
                tmpWrNum = p_WrNbr;
                LocRedLineFile = SaveRedlineXml(tmpWrNum);
                if (LocRedLineFile != string.Empty)
                {
                    if (SaveXmlToSP(tmpWrNum, LocRedLineFile, ref tmpDocMg)==true ) 
                    {
                        AddHyperlink(p_DesArFid, tmpDocMg);
                    }
                }
                else
                {
                    MessageBox.Show("No Redlines are currently present in the session. \n" + 
                                    "Nothing will be saved in SharePoint and the Hyperlink will not be created." 
                        ,"SaveXMLAddHyperlink - Error", MessageBoxButtons.OK);
                }


            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message, "SaveXMLAddHyperlink - Error", MessageBoxButtons.OK);
            }
        }

        private string SaveRedlineXml(string p_wrNum )
        {
           
            
            string tmpRedLineXML = string.Empty;
            string RLPathFile = string.Empty;
            string dtDateTimeStr = string.Empty;

            try
            {
                // save the xml file to the users %temp% directory.
                dtDateTimeStr = DateTime.Now.ToString("yyyyMMdd-HHmm");
                RLPathFile = Path.GetTempPath();
                RLPathFile = RLPathFile +  p_wrNum + "_Constr_" + dtDateTimeStr + ".xml";
                tmpRedLineXML = gtApp.ActiveMapWindow.RedlineService.GetXML(string.Empty, 0);

                // make sure that the redline file contains redline data.
                if (!tmpRedLineXML.Contains("<Redlines>"))
                {
                    RLPathFile = string.Empty;
                    return RLPathFile;
                }

                File.WriteAllText(RLPathFile, tmpRedLineXML);
                
            }
            catch(Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message, "SaveRedlineXml - Error", MessageBoxButtons.OK);
                tmpRedLineXML = string.Empty;
            }
            return RLPathFile;
        }
        
        private Boolean SaveXmlToSP(string p_WrNum, string p_SrcPathFile, ref OncDocManage.OncDocManage p_DocMan)
        {
            Boolean tmpRetVal = false;
            Recordset tmpRs = null;
            string tmpQry = string.Empty;
            IGTDataContext tmpDC = gtApp.DataContext;
            OncDocManage.OncDocManage tmpDocMg = p_DocMan;

            try
            {
                tmpQry = "select param_name, param_value from gis_onc.sys_generalparameter " +
                            "where subsystem_name = 'Doc_Management' and subsystem_component = 'GT_SharePoint'";
                tmpRs = tmpDC.OpenRecordset(tmpQry, CursorTypeEnum.adOpenStatic,
                                            LockTypeEnum.adLockReadOnly, (int)CommandTypeEnum.adCmdText);
                if(!(tmpRs.BOF && tmpRs.EOF))
                {
                    tmpRs.MoveFirst();
                    for(int i = 0; i < tmpRs.RecordCount; ++i)
                    {
                        switch (tmpRs.Fields[0].Value.ToString())
                        {
                            case "JOBWO_REL_PATH":
                                tmpDocMg.SPRelPath = tmpRs.Fields[1].Value.ToString();
                                break;
                            case "ROOT_PATH":
                                tmpDocMg.SPRootPath = tmpRs.Fields[1].Value.ToString();
                                break;
                            case "SP_URL":
                                tmpDocMg.SPSiteURL = tmpRs.Fields[1].Value.ToString();
                                break;
                        }
                        tmpRs.MoveNext();
                    }

                    tmpDocMg.SrcFilePath = p_SrcPathFile;
                    tmpDocMg.WrkOrd_Job = p_WrNum;
                    tmpDocMg.SPFileType = "Construction Redlines";
                    tmpDocMg.SPFileDescription = "Saved on " + DateTime.Now.ToString("MM/dd/yyyy a\\t HH:mm");

                    if (! tmpDocMg.AddSPFile(true))
                    {
                        tmpRetVal = false;
                        MessageBox.Show("Error:The Construction Redline file was not saved to SharePoint.","SaveXmlToSP - Error", MessageBoxButtons.OK);
                    }

                    tmpRetVal = true;
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message, "SaveXmlToSP - Error", MessageBoxButtons.OK);

            }
            return tmpRetVal;
        }
        private Boolean AddHyperlink(int p_DesignAreaFid, OncDocManage.OncDocManage p_DocMan)
        {
            Boolean tmpRetVal = false;
            IGTKeyObject tmpKeyObj = null;
            string SpDocType = "Construction Redlines";
            try
            {
                TransManager.Begin("Add Hyperlink to Design Area");

                tmpKeyObj = gtApp.DataContext.OpenFeature(8100, p_DesignAreaFid);
                tmpKeyObj.Components["JOB_HYPERLINK_N"].Recordset.AddNew("G3E_FID", p_DesignAreaFid);
                tmpKeyObj.Components["JOB_HYPERLINK_N"].Recordset.Fields["HYPERLINK_T"].Value = p_DocMan.RetFileURL;
                tmpKeyObj.Components["JOB_HYPERLINK_N"].Recordset.Fields["DESCRIPTION_T"].Value = p_DocMan.SPFileDescription;
                tmpKeyObj.Components["JOB_HYPERLINK_N"].Recordset.Fields["FILENAME_T"].Value = p_DocMan.RetFileName;

                if (SpDocType.Length > tmpKeyObj.Components["JOB_HYPERLINK_N"].Recordset.Fields["TYPE_C"].DefinedSize)
                {
                    tmpKeyObj.Components["JOB_HYPERLINK_N"].Recordset.Fields["TYPE_C"].Value =
                        SpDocType.Substring(0, tmpKeyObj.Components["JOB_HYPERLINK_N"].Recordset.Fields["TYPE_C"].DefinedSize);
                }
                else
                {
                    tmpKeyObj.Components["JOB_HYPERLINK_N"].Recordset.Fields["TYPE_C"].Value = SpDocType;
                }

                tmpKeyObj.Components["JOB_HYPERLINK_N"].Recordset.Fields["G3E_FNO"].Value = 8100;
                tmpKeyObj.Components["JOB_HYPERLINK_N"].Recordset.Update();
                if (TransManager.TransactionInProgress) TransManager.Commit();
               
                tmpRetVal = true;
            }
            catch(Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message, "AddHyperlink - Error", MessageBoxButtons.OK);
                tmpRetVal = false;
            }
            return tmpRetVal;
        }
    }
}
