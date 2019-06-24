//----------------------------------------------------------------------------+
//        Class: ccRevertFeature
//  Description: ccRevertFeature is a model custom command.It used to remove CUs for a feature that has been posted as part of a WR, and transition that feature back to a closed state.  
//               The intention is to undo individual changes after WR approval..
//----------------------------------------------------------------------------+
//     $$Author::         HCCI                                                $
//       $$Date::         14/11/2017 3.30 PM                                  $
//   $$Revision::         1                                                   $
//----------------------------------------------------------------------------+
//    $$History::         ccRevertFeature.cs                                   $
//
//************************Version 1**************************
//User: Sithara    Date: 14/11/2017   Time : 3.30PM
//Created ccRevertFeature.cs class
// User: hkonda      Date: 08/10/18   Time: Code adjusted as per ONCORDEV-2085
// User: skamaraj Date: 18/03/19 Code adjusted as per ONCORDEV-2655/2650
//----------------------------------------------------------------------------+
using System;
using System.Windows.Forms;
using Intergraph.GTechnology.Interfaces;
using Intergraph.GTechnology.API;
using ADODB;

namespace GTechnology.Oncor.CustomAPI
{
    public class ccRevertFeature : IGTCustomCommandModal
    {
        private IGTTransactionManager m_oGTTransactionManager;
        IGTApplication m_ogtApplication = GTClassFactory.Create<IGTApplication>();
        private IGTKeyObject m_oActiveKeyObject = GTClassFactory.Create<IGTKeyObject>();
        private IGTDDCKeyObject activeFeatureDDCKey = GTClassFactory.Create<IGTDDCKeyObject>();

        private string m_ostrStatus = null;
        private string m_ostrJobtype = null;
        private string m_ostrActFeatureState = null;
        public IGTTransactionManager TransactionManager
        {
            get { return m_oGTTransactionManager; }
            set { m_oGTTransactionManager = value; }
        }

        public void Activate()
        {
           IGTComponent cuComponent = null;
            short aFno = 0;
            int aFid = 0;
            int wpNotAssoFeaturesCount = 0;
            try
            {
                IGTDDCKeyObjects gtDDCKeyObjects = m_ogtApplication.Application.SelectedObjects.GetObjects();
                activeFeatureDDCKey = gtDDCKeyObjects[0];

                IGTJobHelper gTJobHelper = null;
                gTJobHelper = GTClassFactory.Create<IGTJobHelper>();
                ADODB.Recordset myfeatures = gTJobHelper.FindPendingEdits();
                if (myfeatures != null && myfeatures.RecordCount > 0)
                {
                    myfeatures.MoveFirst();
                    myfeatures.Find("g3e_fid=" + activeFeatureDDCKey.FID + "");
                    if (!(myfeatures.BOF || myfeatures.EOF))
                    {
                        MessageBox.Show("This command cannot be used on unposted features.", "G/Technology", MessageBoxButtons.OK, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button1);
                        return;
                    }
                }
                gTJobHelper = null;
                myfeatures = null;

                commonRevertAPI rFeature = new commonRevertAPI(m_ogtApplication);
                if (ValidateCommand())
                {
                    if (!rFeature.ValidateActiveFetature(m_oActiveKeyObject))
                    {
                        //m_ogtApplication.SetStatusBarText(GTStatusPanelConstants.gtaspcMessage, "Feature has been edited by WR " + rFeature.m_WRID + "; cannot revert.");

                        MessageBox.Show("Feature has been edited by WR " + rFeature.m_WRID + "; cannot revert.", "G/Technology", MessageBoxButtons.OK, MessageBoxIcon.Warning,
                        MessageBoxDefaultButton.Button1);
                    }
                    else
                    {
                        if (m_oActiveKeyObject.FNO == 191)
                        {

                            m_oGTTransactionManager.Begin("Revert Feature Fno=" + m_oActiveKeyObject.FNO + " FID=" + m_oActiveKeyObject.FID);

                            cuComponent = m_oActiveKeyObject.Components.GetComponent(19104);
                            if (cuComponent != null && cuComponent.Recordset != null && cuComponent.Recordset.RecordCount > 0)
                            {

                                cuComponent.Recordset.MoveFirst();
                                while (!cuComponent.Recordset.EOF)
                                {
                                    aFno = Convert.ToInt16(cuComponent.Recordset.Fields["ASSOC_FNO"].Value);
                                    aFid = Convert.ToInt32(cuComponent.Recordset.Fields["ASSOC_FID"].Value);

                                    if (!rFeature.ValidateActiveFetature(m_ogtApplication.DataContext.OpenFeature(aFno, aFid)))
                                    {
                                        wpNotAssoFeaturesCount = wpNotAssoFeaturesCount + 1;
                                    }

                                    cuComponent.Recordset.MoveNext();
                                }
                            }

                            if (wpNotAssoFeaturesCount == 0)
                            {
                                rFeature.RevertWPFeature(m_oActiveKeyObject);

                                if (m_oGTTransactionManager.TransactionInProgress)
                                {
                                    m_oGTTransactionManager.Commit();
                                    m_oGTTransactionManager.RefreshDatabaseChanges();
                                }
                            }
                            else if (wpNotAssoFeaturesCount == cuComponent.Recordset.RecordCount)
                            {
                                MessageBox.Show("All features associated with this Work Point have been edited by other WRs and cannot be reverted.", "G/Technology", MessageBoxButtons.OK, MessageBoxIcon.Warning,
                        MessageBoxDefaultButton.Button1);
                            }
                            else if (wpNotAssoFeaturesCount < cuComponent.Recordset.RecordCount)
                            {
                                DialogResult dResult = MessageBox.Show("One or more features associated with this Work Point have been edited by other WRs and cannot be reverted.  Continue reverting other associated features?", "G/Technology", MessageBoxButtons.OKCancel,
                                    MessageBoxIcon.Question);

                                if (dResult == DialogResult.OK)
                                {
                                    rFeature.RevertWPFeature(m_oActiveKeyObject);

                                    if (m_oGTTransactionManager.TransactionInProgress)
                                    {
                                        m_oGTTransactionManager.Commit();
                                        m_oGTTransactionManager.RefreshDatabaseChanges();
                                    }
                                }
                                else
                                {
                                    m_oGTTransactionManager.Rollback();
                                }
                            }

                        }
                        else
                        {
                            if (m_oActiveKeyObject.Components["COMMON_N"].Recordset != null && m_oActiveKeyObject.Components["COMMON_N"].Recordset.RecordCount > 0)
                            {
                                m_oGTTransactionManager.Begin("Revert Feature Fno=" + m_oActiveKeyObject.FNO + " FID=" + m_oActiveKeyObject.FID);

                                rFeature.RevertFeture(m_oActiveKeyObject.FNO, m_oActiveKeyObject.FID, m_ostrActFeatureState, 0, 0, 0);

                                if (m_oGTTransactionManager.TransactionInProgress)
                                {
                                    m_oGTTransactionManager.Commit();
                                    m_oGTTransactionManager.RefreshDatabaseChanges();
                                }
                            }
                        }

                        if (rFeature.m_uProcessedCUs)
                        {
                            MessageBox.Show("Command was unable to revert all activity.", "G/Technology", MessageBoxButtons.OK, MessageBoxIcon.Warning,
                        MessageBoxDefaultButton.Button1);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "G/Technology", MessageBoxButtons.OK, MessageBoxIcon.Error,
                        MessageBoxDefaultButton.Button1);

                m_oGTTransactionManager.Rollback();
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
            short selectedFno = 0;
            int selectedFid = 0;
            try
            {
                selectedFno = activeFeatureDDCKey.FNO;
                selectedFid = activeFeatureDDCKey.FID;
                m_oActiveKeyObject = m_ogtApplication.DataContext.OpenFeature(selectedFno, selectedFid);

                if (m_oActiveKeyObject.Components["COMMON_N"] != null && m_oActiveKeyObject.Components["COMMON_N"].Recordset != null && m_oActiveKeyObject.Components["COMMON_N"].Recordset.RecordCount > 0)
                {
                    m_oActiveKeyObject.Components["COMMON_N"].Recordset.MoveFirst();
                    m_ostrActFeatureState = m_oActiveKeyObject.Components["COMMON_N"].Recordset.Fields["FEATURE_STATE_C"].Value.ToString();
                }

                sql = "select G3E_JOBTYPE,G3E_JOBSTATUS from G3E_JOB where G3E_IDENTIFIER=?";
                //sql = "select JOB_TYPE,G3E_STATUS from G3E_JOB where G3E_IDENTIFIER=?";
                rsValidate = m_ogtApplication.DataContext.OpenRecordset(sql, ADODB.CursorTypeEnum.adOpenDynamic, ADODB.LockTypeEnum.adLockReadOnly,
                        (int)ADODB.CommandTypeEnum.adCmdText, m_ogtApplication.DataContext.ActiveJob);

                if (rsValidate.RecordCount > 0)
                {
                    rsValidate.MoveFirst();
                    if (!rsValidate.EOF && !rsValidate.BOF)
                    {
                        m_ostrJobtype = rsValidate.Fields[0].Value.ToString();
                        m_ostrStatus = rsValidate.Fields[1].Value.ToString();
                    }
                }


                if (m_ostrJobtype == "NON-WR")
                {
                    MessageBox.Show("This command applies only to WR jobs.", "G/Technology", MessageBoxButtons.OK, MessageBoxIcon.Warning,
                        MessageBoxDefaultButton.Button1);
                    validate = false;
                }
                else
                {
                    if (m_ostrStatus.ToUpper() != "ASBUILT" && m_ostrStatus.ToUpper() != "CONSTRUCTIONCOMPLETE" && m_ostrStatus.ToUpper() != "CANCELED")
                    {
                        MessageBox.Show("Features may only be reverted in jobs in AsBuilt, ConstructionComplete or Canceled status.", "G/Technology", MessageBoxButtons.OK, MessageBoxIcon.Warning,
                        MessageBoxDefaultButton.Button1);
                        validate = false;
                    }
                    else if (m_ostrStatus.ToUpper() == "ASBUILT" && m_oActiveKeyObject.FNO != 191)
                    {
                        if (m_ostrActFeatureState != "PPI" && m_ostrActFeatureState != "PPR" && m_ostrActFeatureState != "PPA" && m_ostrActFeatureState != "PPX" &&
                            m_ostrActFeatureState != "ABI" && m_ostrActFeatureState != "ABR" && m_ostrActFeatureState != "ABA" && m_ostrActFeatureState != "ABX")
                        {
                            MessageBox.Show("Only features in proposed states may be reverted in an AsBuilt job.", "G/Technology", MessageBoxButtons.OK, MessageBoxIcon.Warning,
                        MessageBoxDefaultButton.Button1);
                            validate = false;
                        }
                    }
                    else if (m_ostrStatus.ToUpper() == "CONSTRUCTIONCOMPLETE" && m_oActiveKeyObject.FNO != 191)
                    {
                        if (m_ostrActFeatureState != "INI" && m_ostrActFeatureState != "OSR")
                        {
                            MessageBox.Show("Only INI and OSR features may be reverted in a ConstructionComplete job.", "G/Technology", MessageBoxButtons.OK, MessageBoxIcon.Warning,
                        MessageBoxDefaultButton.Button1);
                            validate = false;
                        }
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
