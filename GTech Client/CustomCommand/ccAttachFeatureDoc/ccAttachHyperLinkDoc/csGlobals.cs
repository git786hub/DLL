using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Intergraph.GTechnology.API;
using ADODB;


namespace GTechnology.Oncor.CustomAPI
{
    class csGlobals
    {
        static internal IGTTransactionManager gManageTransactions;
        static internal Form gfrmAddFeatureHyperlink = null;
        static internal IGTApplication gApp = null;
        static internal IGTDataContext gDatacont = null;
        static internal short gCCFno = 0;
        static internal int gCCFid = 0;
        static internal string gCCHyperLnkCno = string.Empty;
        static internal string gSPFileTypeLst = string.Empty;
        static internal string gOpenFileFilterLst = string.Empty;
        static internal string gSPDocBaseRelPath = string.Empty;
        static internal string gSPDocFeatureDocLoc = string.Empty;
        static internal string gMessage = string.Empty;

/// <summary>
/// Gets the command parameters from the SYS_GENERALPARAMETER table for this command.
/// </summary>
/// <returns></returns>
        internal static Boolean gGetSysGenPrams()
        {
            Recordset tmpRS = null;
            string tmpQry = string.Empty;
            Boolean tmpRetVal = false;
            try
            {
                tmpQry = "select param_name, param_value from gis_onc.SYS_GENERALPARAMETER " +
                                 "where SUBSYSTEM_NAME = 'AttachFeatureDocumentCC'";
                tmpRS = gDatacont.OpenRecordset(tmpQry, CursorTypeEnum.adOpenStatic,
                                 LockTypeEnum.adLockReadOnly, (int)CommandTypeEnum.adCmdText);
                if (!(tmpRS.BOF && tmpRS.EOF))
                {
                    tmpRS.MoveFirst();

                    for (int i = 0; i < tmpRS.RecordCount; ++i)
                    {
                        switch(tmpRS.Fields[0].Value.ToString())
                        {
                            case "SPRelPath":
                                gSPDocBaseRelPath = tmpRS.Fields[1].Value.ToString();
                                break;
                            case "FILE_TYPES":
                                gSPFileTypeLst = tmpRS.Fields[1].Value.ToString();
                                break;
                            case "FILE_EXTENSIONS":
                                gOpenFileFilterLst = tmpRS.Fields[1].Value.ToString();
                                break;
                            case "FEATURE_FILE_LOC":
                                gSPDocFeatureDocLoc = tmpRS.Fields[1].Value.ToString();
                                break;
                        }
                        tmpRS.MoveNext();
                    }
                }

                tmpRetVal = true;
            }
            catch(Exception ex)
            {
                MessageBox.Show("Error in gGetSPFileTypeList: " + ex.Message,
                               "Attach Feature Hyperlink Document - csGlobals",
                               MessageBoxButtons.OK, MessageBoxIcon.Error);
                tmpRetVal = false;

            }
            return tmpRetVal;
        }
        /// <summary>
        /// Adds the file to Sharepoint and adds the hyperlink to the selected feature.
        /// </summary>
        /// <param name="p_Description"></param>
        /// <param name="p_SrcFile"></param>
        /// <param name="p_SPType"></param>
        /// <returns></returns>
        static internal Boolean gAddSPDocAndHyperLnk(string p_Description, string p_SrcFile, string p_SPType)
        {
            Boolean tmpRetVal = false;
            Recordset tmpRs = null;
            string tmpQry = string.Empty;
            string tmpSPFileName = string.Empty;

            OncDocManage.OncDocManage SpDocMan = new OncDocManage.OncDocManage();

            // add document to SharePoint
            try
            {
                gMessage = "Adding the file to SharePoint.";
                gApp.SetStatusBarText(GTStatusPanelConstants.gtaspcMessage, gMessage);

                gApp.BeginWaitCursor();

                tmpQry = "select param_name, param_value from SYS_GENERALPARAMETER " +
                         "where subsystem_name = 'Doc_Management'";
                tmpRs = gDatacont.OpenRecordset(tmpQry, CursorTypeEnum.adOpenStatic,
                                                                   LockTypeEnum.adLockReadOnly,
                                                                   (int)CommandTypeEnum.adCmdText);
                if (!(tmpRs.BOF && tmpRs.EOF))
                {
                    tmpRs.MoveFirst();
                    for (int i = 0; i < tmpRs.RecordCount; ++i)
                    {
                        if (tmpRs.Fields["PARAM_NAME"].Value.ToString() == "ROOT_PATH")
                            SpDocMan.SPRootPath = tmpRs.Fields["PARAM_VALUE"].Value.ToString();
                        if (tmpRs.Fields["PARAM_NAME"].Value.ToString() == "SP_URL")
                            SpDocMan.SPSiteURL = tmpRs.Fields["PARAM_VALUE"].Value.ToString();
                        tmpRs.MoveNext();
                    }
                }
                // set the Properties for the OncDocManage class
                SpDocMan.SPRelPath = gSPDocBaseRelPath;
                SpDocMan.SPFileDescription = p_Description;
                tmpSPFileName = p_SrcFile.Substring(p_SrcFile.LastIndexOf("\\") + 1);
                tmpSPFileName = tmpSPFileName.Insert(tmpSPFileName.LastIndexOf("."), csGlobals.gCCFid.ToString());
                SpDocMan.SPFileName = tmpSPFileName;
                SpDocMan.SrcFilePath = p_SrcFile;
                SpDocMan.SPFileType = p_SPType;
                SpDocMan.WrkOrd_Job = gSPDocFeatureDocLoc; // "Feature Documents";
                // Add the file to SharePoint
                if (!SpDocMan.AddSPFile(true))
                {
                    gMessage = "The file was not saved to SharePoint";
                    gApp.SetStatusBarText(GTStatusPanelConstants.gtaspcMessage, gMessage);
                    //System.ArgumentException SPExcept = new System.ArgumentException("File was not saved in SharePoint.");
                    tmpRetVal = false;
                }

            }
            catch (Exception ex)
            {
                gMessage = "Unable to copy the file to SharePoint. Error: " + ex.Message;
                gApp.SetStatusBarText(GTStatusPanelConstants.gtaspcMessage, gMessage);
                MessageBox.Show(gMessage, 
                                "Attach Feature Hyperlink Document - csGlobals",
                                MessageBoxButtons.OK, MessageBoxIcon.Error);
              
                gApp.EndWaitCursor();
                return tmpRetVal;
            }

            // Add the Hyperlink component to the feature.
            IGTKeyObject tmpFeature = null;
            try
            {
                gMessage = "Adding the Hyperlink record to the feature";
                gApp.SetStatusBarText(GTStatusPanelConstants.gtaspcMessage , gMessage);
                // begin the transaction
                gManageTransactions.Begin("IN PROGRESS");
                // Open the feature
                tmpFeature = gDatacont.OpenFeature(gCCFno, gCCFid);
                // Add the hyperlink component and set its attributes
                tmpFeature.Components[gCCHyperLnkCno].Recordset.AddNew("G3E_FID", gCCFid);
                tmpFeature.Components[gCCHyperLnkCno].Recordset.Fields["G3E_FNO"].Value = gCCFno;
                tmpFeature.Components[gCCHyperLnkCno].Recordset.Fields["HYPERLINK_T"].Value = SpDocMan.RetFileURL.ToString();
                tmpFeature.Components[gCCHyperLnkCno].Recordset.Fields["DESCRIPTION_T"].Value = p_Description.ToString();
                tmpFeature.Components[gCCHyperLnkCno].Recordset.Fields["TYPE_C"].Value = p_SPType.ToString();
                tmpFeature.Components[gCCHyperLnkCno].Recordset.Fields["FILENAME_T"].Value = SpDocMan.RetFileName.ToString();
                // Commit the transaction
                if (gManageTransactions.TransactionInProgress) gManageTransactions.Commit();

                gMessage = "Attach Feature Document Completed sucessfully.";
                
                gApp.SetStatusBarText(GTStatusPanelConstants.gtaspcMessage, gMessage);
             
                tmpRetVal = true;
            }
            catch(Exception ex)
            {
                if (gManageTransactions.TransactionInProgress) gManageTransactions.Rollback();

                gMessage = "Error in gAddSPDocAndHyperLnk: " + ex.Message;
                gApp.SetStatusBarText(GTStatusPanelConstants.gtaspcMessage, gMessage);
                MessageBox.Show(gMessage,
                               "Attach Feature Hyperlink Document - csGlobals",
                               MessageBoxButtons.OK, MessageBoxIcon.Error);
                tmpRetVal = false;
            }
           
            gApp.EndWaitCursor();

            return tmpRetVal;
        }
    }
}
