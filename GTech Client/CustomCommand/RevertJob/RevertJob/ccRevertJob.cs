//----------------------------------------------------------------------------+
//        Class: ccRevertJob
//  Description: ccRevertJob is a model custom command.It used to remove all CUs that have been posted as part of a WR, 
//               and transition related features back to a closed state.  The intention is to undo the entire design of a WR sometime after WR approval, 
//               either due to premature approval or cancellation of the WR.
//----------------------------------------------------------------------------+
//     $$Author::         HCCI                                                $
//       $$Date::         14/11/2017 3.30 PM                                  $
//   $$Revision::         1                                                   $
//----------------------------------------------------------------------------+
//    $$History::         ccRevertJob.cs                                   $
//
//************************Version 1**************************
//User: Sithara    Date: 14/11/2017   Time : 3.30PM
//Created ccRevertJob.cs class
// User: hkonda     Date: 08/10/18   Time: Code adjusted as per ONCORDEV-2085
//----------------------------------------------------------------------------+
using System;
using System.Windows.Forms;
using Intergraph.GTechnology.Interfaces;
using Intergraph.GTechnology.API;
using ADODB;

namespace GTechnology.Oncor.CustomAPI
{
    public class ccRevertJob : IGTCustomCommandModal
    {
        private IGTTransactionManager m_oGTTransactionManager;
        IGTApplication m_ogtApplication = GTClassFactory.Create<IGTApplication>();


        public IGTTransactionManager TransactionManager
        {
            get { return m_oGTTransactionManager; }
            set { m_oGTTransactionManager = value; }
        }

        public void Activate()
        {
            string sql = "SELECT G3E_FID FROM WORKPOINT_N WHERE WR_NBR=:1 AND G3E_FNO=191";
            Recordset rsValidate = null;
            int jFid = 0;
            short jFno = 0;
            IGTKeyObject jKeyObject = null;
            IGTJobManagementService oJobManagement = GTClassFactory.Create<IGTJobManagementService>();
            oJobManagement.DataContext = m_ogtApplication.DataContext;

            try
            {                
                if (ValidateCommand())
                {
                    ADODB.Recordset rsPendingEdits = oJobManagement.FindPendingEdits();
                    if (rsPendingEdits!=null && rsPendingEdits.RecordCount>0)
                    {

                        oJobManagement.DiscardJob(); //ALM 1321 - Automatically discard job at the start and assume that there are no unposted data
                        m_oGTTransactionManager.RefreshDatabaseChanges();
                        m_ogtApplication.RefreshWindows();
                    }

                    rsValidate = m_ogtApplication.DataContext.OpenRecordset(sql, ADODB.CursorTypeEnum.adOpenDynamic, ADODB.LockTypeEnum.adLockReadOnly,
                        (int)ADODB.CommandTypeEnum.adCmdText, new object[] { m_ogtApplication.DataContext.ActiveJob });

                    if (rsValidate == null || rsValidate.RecordCount == 0) return;

                    m_oGTTransactionManager.Begin("Revert Job");

                    commonRevertAPI rFeature = new commonRevertAPI(m_ogtApplication);
                    rsValidate.MoveFirst();
                    while (!rsValidate.EOF)
                    {
                        jFno = 191;
                        jFid = Convert.ToInt32(rsValidate.Fields["G3E_FID"].Value);

                        jKeyObject = m_ogtApplication.DataContext.OpenFeature(jFno, jFid);

                        rFeature.m_FromJob = "JOB";
                        rFeature.RevertWPFeature(jKeyObject);


                        rsValidate.MoveNext();
                    }

                    if (rFeature.m_uProcessedCUs)
                    {
                        MessageBox.Show("Check remaining Work Points to make manual corrections.", "G/Technology", MessageBoxButtons.OK, MessageBoxIcon.Warning,
                    MessageBoxDefaultButton.Button1);
                    }

                    if (m_oGTTransactionManager.TransactionInProgress)
                    {
                        m_oGTTransactionManager.Commit();
                        m_oGTTransactionManager.RefreshDatabaseChanges();
                    }

                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "G/Technology", MessageBoxButtons.OK, MessageBoxIcon.Error,
                        MessageBoxDefaultButton.Button1);

                m_oGTTransactionManager.Rollback();
            }
            finally
            {
                if (rsValidate != null)
                {
                    if (rsValidate.State == 1)
                    {
                        rsValidate.Close();
                        rsValidate.ActiveConnection = null;
                    }
                    rsValidate = null;
                }
            }
        }

        /// <summary>
        /// validateCommand : This method validates the command execution requirements are satisfied or not.
        /// </summary>
        /// <returns></returns>
        private bool ValidateCommand()
        {
            bool validate = true;
            string sql = "";
            Recordset rsValidate = null;
            string strStatus = null;
            string strJobtype = null;
            try
            {
                sql = "select G3E_JOBTYPE,G3E_JOBSTATUS from G3E_JOB where G3E_IDENTIFIER=?";
                //sql = "select JOB_TYPE,JOB_STATE from G3E_JOB where G3E_IDENTIFIER=?";
                rsValidate = m_ogtApplication.DataContext.OpenRecordset(sql, ADODB.CursorTypeEnum.adOpenDynamic, ADODB.LockTypeEnum.adLockReadOnly,
                        (int)ADODB.CommandTypeEnum.adCmdText, m_ogtApplication.DataContext.ActiveJob);

                if (rsValidate.RecordCount > 0)
                {
                    rsValidate.MoveFirst();
                    if (!rsValidate.EOF && !rsValidate.BOF)
                    {
                        strJobtype = Convert.ToString(rsValidate.Fields[0].Value);
                        strStatus = Convert.ToString(rsValidate.Fields[1].Value);
                    }
                }


                if (strJobtype.ToUpper() == "NON-WR")
                {
                    MessageBox.Show("This command applies only to WR jobs.", "G/Technology", MessageBoxButtons.OK, MessageBoxIcon.Warning,
                        MessageBoxDefaultButton.Button1);
                    validate = false;
                }
                else
                {
                    if (strStatus.ToUpper() != "ASBUILT" && strStatus.ToUpper() != "CONSTRUCTIONCOMPLETE" && strStatus.ToUpper() != "CANCELED")
                    {
                        MessageBox.Show("Only jobs in AsBuilt, ConstructionComplete, or Canceled status may be reverted.", "G/Technology", MessageBoxButtons.OK, MessageBoxIcon.Warning,
                        MessageBoxDefaultButton.Button1);
                        validate = false;
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                if (rsValidate != null)
                {
                    if (rsValidate.State == 1)
                    {
                        rsValidate.Close();
                        rsValidate.ActiveConnection = null;
                    }
                    rsValidate = null;
                }
            }
            return validate;
        }
    }
}
