using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Intergraph.GTechnology.API;
using System.Windows.Forms;
using ADODB;
using OncDocManage;
using System.IO;

namespace GTechnology.Oncor.CustomAPI
{
    internal class csGlobals
    {
        internal static IGTApplication gApp = null;
        internal static IGTTransactionManager gTransManager = null;
        internal static IGTMapWindow gCurrActMapWind = null;
        internal static string gActJobTyp = string.Empty;
        internal static Form gConstRedLineFrm = null;


        internal static Boolean gGetRedLineFilesfromSp()
        {
            Boolean tmpRetVal = false;
            OncDocManage.OncDocManage spDocMan = new OncDocManage.OncDocManage();
            Recordset rsConfigs = null;
            string sWrName = string.Empty;
            string sEnvTempPath = string.Empty;


            try
            {

                if (gIsWrJobType(ref sWrName))
                {
                    sEnvTempPath = Path.GetTempPath();
                    rsConfigs = rsDocManageConfigs();
                    setDocManVals(rsConfigs, ref spDocMan);
                    rsConfigs = null;
                    spDocMan.WrkOrd_Job = sWrName;
                    spDocMan.SPFileType = "Construction Redlines";
                    spDocMan.SPRelPath = spDocMan.SPRelPath + "/" + sWrName;
                    if (spDocMan.GetFileFromList(sEnvTempPath) == true)
                    {
                        if (bDisplayConstRedLineFile(sEnvTempPath + "\\" + spDocMan.RetFileName) == true)
                        {
                            tmpRetVal = true;
                        }
                    }
                    else
                    {
                        MessageBox.Show("Error: " + spDocMan.RetErrMessage, "Error: gGetRedLineFilesfromSp",
                                 MessageBoxButtons.OK, MessageBoxIcon.Error);
                        tmpRetVal = false;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message, "Error: gGetRedLineFilesfromSp",
                                 MessageBoxButtons.OK, MessageBoxIcon.Error);
                tmpRetVal = false;
            }
            return tmpRetVal;
        }


        internal static Boolean gIsWrJobType(ref string p_WR )
        {
            Boolean tmpRetValue = false;
            string tmpWrTyp = string.Empty;
            Recordset tmpRs = null;
            string tmpQry = string.Empty;
            try
            {
                tmpQry = "Select G3E_JOBTYPE, WR_NBR from g3e_job where g3e_identifier = '" +
                          gApp.DataContext.ActiveJob + "'";
                tmpRs = gApp.DataContext.OpenRecordset(tmpQry, CursorTypeEnum.adOpenStatic,
                                                       LockTypeEnum.adLockReadOnly,
                                                       (int)CommandTypeEnum.adCmdText);
                if (!(tmpRs.BOF && tmpRs.EOF))
                {
                    tmpRs.MoveFirst();
                    if ( tmpRs.Fields[0].Value.ToString() == "NON-WR" || tmpRs.Fields[0].Value.ToString() == "" )
                    {
                        tmpRetValue = false;
                    }
                    else
                    {

                        tmpRetValue = true;
                    }
                }
                else
                {
                    MessageBox.Show("Job "+ gApp.DataContext.ActiveJob + " was not found.", "Error: Job not found.",
                                 MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                p_WR = tmpRs.Fields[1].Value.ToString();

            }
            catch( Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message, "Error: gIsWrJobType", 
                                 MessageBoxButtons.OK,MessageBoxIcon.Error);
            }
            return tmpRetValue;
        }



        private static void setDocManVals(Recordset p_ConfigRs, ref OncDocManage.OncDocManage p_docManage)
        {
            try
            {
                p_ConfigRs.MoveFirst();

                for (int i = 0; i < p_ConfigRs.RecordCount; ++i)
                {
                    switch(p_ConfigRs.Fields[0].Value )
                    {
                        case "ROOT_PATH":
                            p_docManage.SPRootPath = p_ConfigRs.Fields[1].Value.ToString();
                            break;
                        case "SP_URL":
                            p_docManage.SPSiteURL = p_ConfigRs.Fields[1].Value.ToString();
                            break;
                        case "JOBWO_REL_PATH":
                            p_docManage.SPRelPath = p_ConfigRs.Fields[1].Value.ToString();
                            break;
                    }

                    p_ConfigRs.MoveNext();
                }

                p_ConfigRs = null;
            }
            catch(Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message, "Error: setDocManVals",
                                MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private static Recordset rsDocManageConfigs()
        {
            Recordset tmpRs = null;
            string tmpQry = string.Empty;
            //IGTDataContext tmpDataCont = null;
            try
            {
                
                tmpQry = "select param_name,param_value from SYS_GENERALPARAMETER " +
                            "where subsystem_name = 'Doc_Management' and Subsystem_component = 'GT_SharePoint'";
                tmpRs = gApp.DataContext.OpenRecordset(tmpQry, CursorTypeEnum.adOpenStatic,
                                                        LockTypeEnum.adLockReadOnly, (int)CommandTypeEnum.adCmdText);
                if ((tmpRs.BOF && tmpRs.EOF))
                {
                    tmpRs = null;
                }
                        
            }
            catch(Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message, "Error: gGetRedLineFilesfromSp",
                                 MessageBoxButtons.OK, MessageBoxIcon.Error);
                tmpRs = null;
            }
            return tmpRs;
        }

        private static Boolean bDisplayConstRedLineFile(string pPathAndFile)
        {
            Boolean tmpRetVal = false;
            IGTDisplayService tmpDispSvr = null;

            try
            {
                tmpDispSvr = gCurrActMapWind.DisplayService;
                tmpDispSvr.AppendRedlines("Constructions Redlines", pPathAndFile);

                gApp.RefreshWindows();

                tmpRetVal = true;
            }
            catch(Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message, "Error: gGetRedLineFilesfromSp",
                                 MessageBoxButtons.OK, MessageBoxIcon.Error);
                tmpRetVal = false;
            }
            return tmpRetVal;
        }
    }
}
