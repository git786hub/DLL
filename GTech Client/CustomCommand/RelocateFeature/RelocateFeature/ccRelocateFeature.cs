//----------------------------------------------------------------------------+
//        Class: ccRelocateFeature
//  Description: ccRelocateFeature is a modeless custom command to relocate the selected feature.
//----------------------------------------------------------------------------+
//     $$Author::         HCCI                                                $
//       $$Date::         06/11/2017 3.30 PM                                  $
//   $$Revision::         1                                                   $
//----------------------------------------------------------------------------+
//    $$History::         ccRelocateFeature.cs                                   $
//
//************************Version 1**************************
//User: Sithara    Date: 6/11/2017   Time : 3.30PM
//Created ccRelocateFeature.cs class

// User: hkonda     Date: 31/01/19   Time: 18:00 Code adjusted as per ALM-1550-JIRA-2462
//----------------------------------------------------------------------------+
using System;
using System.Windows.Forms;
using Intergraph.GTechnology.Interfaces;
using Intergraph.GTechnology.API;
using ADODB;

namespace GTechnology.Oncor.CustomAPI
{
    public class ccRelocateFeature : IGTCustomCommandModeless
    {
        #region Variable declarations

        Boolean m_canTerminate = false;
        private IGTTransactionManager m_GTTransactionManager;
        IGTCustomCommandHelper m_GTCustomCommandHelper;
        IGTApplication m_gtApplication = (IGTApplication)GTClassFactory.Create<IGTApplication>();
        private IGTDDCKeyObjects m_gtDDCKeyObjects = GTClassFactory.Create<IGTDDCKeyObjects>();
        private IGTDDCKeyObject m_activeFeatureDDCKey;
        private short m_selectedFno;
        private Int32 m_selectedFid;
        private IGTKeyObject m_oActiveKeyObject = GTClassFactory.Create<IGTKeyObject>();
        private string m_strStatus = null;
        private string m_strJobtype = null;

        #endregion

        public IGTTransactionManager TransactionManager
        {
            get { return m_GTTransactionManager; }
            set { m_GTTransactionManager = value; }
        }

        public bool CanTerminate
        {
            get { return m_canTerminate; }
        }

        /// <summary>
        /// Custom Command activate method: This mwthos checks all validation and once it is passed RelocateFeature class will be called to relocate the feature.
        /// </summary>
        /// <param name="CustomCommandHelper"></param>
        public void Activate(IGTCustomCommandHelper CustomCommandHelper)
        {
            bool ccValidation = false;
            IGTKeyObjects relatedFeaturesOWB = GTClassFactory.Create<IGTKeyObjects>();

            try
            {
                m_GTCustomCommandHelper = CustomCommandHelper;

                // check if the command is called multiple times
                for (int i = 0; i < m_gtApplication.Application.Properties.Keys.Count; i++)
                {
                    if (string.Equals("RelocateFeatureCC", Convert.ToString(m_gtApplication.Application.Properties.Keys.Get(i))))
                    {
                        return;
                    }
                }

                m_gtApplication.Application.Properties.Add("RelocateFeatureCC", "RelocateFeature");

                GTClassFactory.Create<IGTApplication>().SetStatusBarText(GTStatusPanelConstants.gtaspcMessage, "Relocate Feature custom command....");
                ccValidation = ValidateCC(out relatedFeaturesOWB);
                if (ccValidation)
                {
                    RelocateFeature feature_T = new RelocateFeature(m_GTCustomCommandHelper, m_gtApplication, m_oActiveKeyObject, relatedFeaturesOWB,
                        TransactionManager, m_strStatus, m_strJobtype);
                    GTClassFactory.Create<IGTApplication>().SetStatusBarText(GTStatusPanelConstants.gtaspcMessage,
                        "Relocate Feature Custom Command: Please select new owner feature or press Esc key to exit the command.");
                }
                else
                {
                    ExitCmd();
                }

            }
            catch (Exception ex)
            {
                if (m_gtApplication.Application.Properties.Count > 0)
                    m_gtApplication.Application.Properties.Remove("RelocateFeatureCC");

                MessageBox.Show(ex.Message, "Relocate Feature", MessageBoxButtons.OK, MessageBoxIcon.Error,
                        MessageBoxDefaultButton.Button1);
            }
            finally
            {


            }
        }

        public void Pause()
        {

        }

        public void Resume()
        {

        }

        public void Terminate()
        {
            if (m_GTTransactionManager != null)
            {
                if (m_GTTransactionManager.TransactionInProgress)
                    m_GTTransactionManager.Rollback();
            }
            m_GTTransactionManager = null;
        }


        /// <summary>
        /// ExisCmd is used to exit the Custom command from the Gtech
        /// </summary>
        private void ExitCmd()
        {
            try
            {
                GTClassFactory.Create<IGTApplication>().SetStatusBarText(GTStatusPanelConstants.gtaspcMessage, "Relocate Feature Command : Exiting...");

                if (m_gtApplication.Application.Properties.Count > 0)
                    m_gtApplication.Application.Properties.Remove("RelocateFeatureCC");


                if (m_GTCustomCommandHelper != null)
                {
                    m_GTCustomCommandHelper.Complete();
                    m_GTCustomCommandHelper = null;
                }

                m_gtApplication.Application.EndWaitCursor();
                m_gtApplication.Application.RefreshWindows();
            }
            catch (Exception ex)
            {
                if (m_gtApplication.Application.Properties.Count > 0)
                    m_gtApplication.Application.Properties.Remove("RelocateFeatureCC");
                if (m_GTCustomCommandHelper != null)
                {
                    m_GTCustomCommandHelper.Complete();
                    m_GTCustomCommandHelper = null;
                }

                m_GTCustomCommandHelper = null;
                m_gtApplication.Application.EndWaitCursor();
                m_gtApplication.Application.RefreshWindows();

                MessageBox.Show(ex.Message, "Relocate Feature", MessageBoxButtons.OK, MessageBoxIcon.Error,
                        MessageBoxDefaultButton.Button1);
            }
        }


        /// <summary>
        /// This method is used to validate the initial conditions of the relocate custom command.
        /// </summary>
        /// <param name="relatedFeaturesOWB"></param>
        /// <returns></returns>
        private bool ValidateCC(out IGTKeyObjects relatedFeaturesOWB)
        {
            bool execute = true;
            string sql = "";
            Recordset rsValidate = null;
            int reCount = 0;
            IGTRelationshipService relationShipService = GTClassFactory.Create<IGTRelationshipService>();
            relatedFeaturesOWB = GTClassFactory.Create<IGTKeyObjects>();
            try
            {
                //Validate conditions to run the custom command. 

                // 1.  This command applies only to WR jobs.

                #region First Condition
                sql = "select G3E_JOBTYPE,G3E_JOBSTATUS from G3E_JOB where G3E_IDENTIFIER=?";
                //sql = "select JOB_TYPE,G3E_STATUS from G3E_JOB where G3E_IDENTIFIER=?";
                rsValidate = m_gtApplication.DataContext.OpenRecordset(sql, ADODB.CursorTypeEnum.adOpenDynamic, ADODB.LockTypeEnum.adLockReadOnly,
                        (int)ADODB.CommandTypeEnum.adCmdText, m_gtApplication.DataContext.ActiveJob);

                if (rsValidate.RecordCount > 0)
                {
                    rsValidate.MoveFirst();
                    if (!rsValidate.EOF && !rsValidate.BOF)
                    {
                        m_strJobtype = Convert.ToString(rsValidate.Fields[0].Value);
                        m_strStatus = Convert.ToString(rsValidate.Fields[1].Value);
                    }
                }
                #endregion

                if (m_strJobtype == "NON-WR")
                {
                    MessageBox.Show("This command applies only to WR jobs.", "Relocate Feature", MessageBoxButtons.OK, MessageBoxIcon.Warning,
                        MessageBoxDefaultButton.Button1);
                    execute = false;
                }
                else
                {
                    //2. This command applies only to features with CUs.

                    #region Second Condition

                    m_gtDDCKeyObjects = m_gtApplication.Application.SelectedObjects.GetObjects();
                    m_activeFeatureDDCKey = m_gtDDCKeyObjects[0];
                    m_selectedFno = m_activeFeatureDDCKey.FNO;
                    m_selectedFid = m_activeFeatureDDCKey.FID;

                    sql = "SELECT count(*) FROM G3E_FEATURECOMPS_OPTABLE WHERE G3E_FNO=? AND G3E_CNO=21";
                    rsValidate = m_gtApplication.DataContext.OpenRecordset(sql, ADODB.CursorTypeEnum.adOpenDynamic, ADODB.LockTypeEnum.adLockReadOnly,
                        (int)ADODB.CommandTypeEnum.adCmdText, m_selectedFno);

                    if (rsValidate.RecordCount > 0)
                    {
                        rsValidate.MoveFirst();
                        if (!rsValidate.EOF && !rsValidate.BOF)
                        {
                            reCount = Convert.ToInt32(rsValidate.Fields[0].Value);
                        }
                    }

                    #endregion

                    if (reCount < 1)
                    {
                        MessageBox.Show("This command applies only to features with CUs.", "Relocate Feature", MessageBoxButtons.OK, MessageBoxIcon.Warning,
                        MessageBoxDefaultButton.Button1);

                        execute = false;
                    }
                    else
                    {
                        // 3. Cannot Relocate a feature that is not currently owned.
                        #region Third Condition
                        m_oActiveKeyObject = m_gtApplication.DataContext.OpenFeature(m_selectedFno, m_selectedFid);
                        relationShipService.ActiveFeature = m_oActiveKeyObject;
                        relationShipService.DataContext = m_gtApplication.DataContext;
                        int ownFcount = 0;
                        try
                        {
                            relatedFeaturesOWB = relationShipService.GetRelatedFeatures(3);
                            ownFcount = relatedFeaturesOWB.Count;
                        }
                        catch
                        {

                        }

                        #endregion
                        if (ownFcount <= 0)
                        {
                            MessageBox.Show("Cannot relocate a feature that is not currently owned.", "Relocate Feature", MessageBoxButtons.OK, MessageBoxIcon.Warning,
                        MessageBoxDefaultButton.Button1);

                            execute = false;
                        }
                        else
                        {
                            // 4. This command applies only to point features.
                            #region Fourth Condition
                            sql = "SELECT * FROM G3E_COMPONENTINFO_OPTABLE WHERE G3E_CNO IN(SELECT G3E_PRIMARYGEOGRAPHICCNO FROM G3E_FEATURES_OPTABLE WHERE G3E_FNO=?) AND UPPER(G3E_GEOMETRYTYPE) LIKE '%POINT%'";
                            rsValidate = m_gtApplication.DataContext.OpenRecordset(sql, ADODB.CursorTypeEnum.adOpenDynamic, ADODB.LockTypeEnum.adLockReadOnly,
                        (int)ADODB.CommandTypeEnum.adCmdText, m_selectedFno);

                            #endregion
                            if (rsValidate.RecordCount <= 0)
                            {
                                MessageBox.Show("This command applies only to point features.", "Relocate Feature", MessageBoxButtons.OK, MessageBoxIcon.Warning,
                        MessageBoxDefaultButton.Button1);

                                execute = false;
                            }
                            else
                            {
                                //execute = true;
                                if (!CheckIfInstallAndActiveWrAreDifferent())
                                {
                                    MessageBox.Show("The same feature may not be installed and relocated in the same WR.", "Relocate Feature", MessageBoxButtons.OK, MessageBoxIcon.Warning,
                       MessageBoxDefaultButton.Button1);
                                    execute = false;
                                }
                                else
                                {
                                    execute = true;
                                }
                            }

                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
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

                if (relationShipService != null)
                {
                    relationShipService.Dispose();
                    relationShipService = null;
                }
            }

            return execute;
        }


        private bool CheckIfInstallAndActiveWrAreDifferent()
        {
            try
            {
                IGTKeyObject feature = m_gtApplication.DataContext.OpenFeature(m_selectedFno, m_selectedFid);
                IGTComponent cuComponent = feature.Components.GetComponent(21);
                Recordset cuComponentRs = cuComponent.Recordset;
                cuComponentRs.MoveFirst();
                while (!cuComponentRs.EOF)
                {
                    string installWr = Convert.ToString(cuComponentRs.Fields["WR_ID"].Value);
                    if (!installWr.Equals(m_gtApplication.DataContext.ActiveJob))
                    {
                        return true;
                    }
                    cuComponentRs.MoveNext();
                }
                return false;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
