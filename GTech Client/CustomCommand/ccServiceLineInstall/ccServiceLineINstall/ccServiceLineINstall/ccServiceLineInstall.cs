using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Intergraph.GTechnology.Interfaces;
using Intergraph.GTechnology.API;
using ADODB;

namespace GTechnology.Oncor.CustomAPI
{
    public class ccServiceLineInstall : IGTCustomCommandModeless
    {
        private IGTApplication m_gtApp = GTClassFactory.Create<IGTApplication>();
        private IGTDataContext dataContext = null;
        private IGTFeaturePlacementService featurePlacementService = null;
        private IGTCustomCommandHelper customCommandHelper = null;
        private IGTFeatureExplorerService featureExplorerService = null;
        private IGTKeyObject serviceLine;
        private short g_Service_Line_FNO = 54;
        private bool featureExplorerVisible = false;

        #region IGTCustomCommandModeless Members
        public bool CanTerminate
        {
            get { return true; }
        }

        public IGTTransactionManager TransactionManager {
            set { gtTransactionManager = value; }
        }

        public IGTTransactionManager gtTransactionManager = null;

        /// <summary>
        /// Activates the install service process. Creates the new service line and sets up the listeners.
        /// </summary>
        /// <param name="CustomCommandHelper"></param>
        public void Activate(IGTCustomCommandHelper CustomCommandHelper)
        {
            try
            {
                if (gtTransactionManager != null)
                {
                    serviceLine = null;
                    dataContext = m_gtApp.DataContext;
                    customCommandHelper = CustomCommandHelper;
                    featurePlacementService = GTClassFactory.Create<IGTFeaturePlacementService>(customCommandHelper);
                    featureExplorerService = GTClassFactory.Create<IGTFeatureExplorerService>();
                    featureExplorerVisible = featureExplorerService.Visible;
                    SubscribeEvents();
                    placeServiceLine();
                }
            }catch (Exception e)
            {

            }
        }

        public void placeServiceLine()
        {
            gtTransactionManager.Begin("Service Line Placement");
            serviceLine = dataContext.NewFeature(g_Service_Line_FNO);
            featurePlacementService.StartFeature(serviceLine);
        }

        public void loadServiceLineData()
        {
            IGTComponent lineAttributes = serviceLine.Components.GetComponent(5401);
            IGTComponent lineCommonAttributes = serviceLine.Components.GetComponent(1);
            IGTComponent lineCUAttributes = serviceLine.Components.GetComponent(21);
            IGTComponent lineElectricalAttributes = serviceLine.Components.GetComponent(11);
            string CUQuery = "SELECT PARAM_VALUE FROM SYS_GENERALPARAMETER WHERE SUBSYSTEM_NAME = ? AND PARAM_NAME = ? AND SUBSYSTEM_COMPONENT = ?";
            Recordset defaultValues = dataContext.OpenRecordset(CUQuery, ADODB.CursorTypeEnum.adOpenDynamic, ADODB.LockTypeEnum.adLockBatchOptimistic, -1, "SERVICE_LINE_INSTALL", "CU", "CU_DEFAULT");
            if (defaultValues.BOF && defaultValues.EOF)
            {
                customCommandHelper.Complete();
            }
            else
            {
                if(lineAttributes.Recordset.RecordCount > 0)
                {
                  lineAttributes.Recordset.MoveFirst();
                  lineAttributes.Recordset.Fields["PLACEMENT_TYPE_C"].Value = "ASSOCIATED";
                }
                if(lineCommonAttributes.Recordset.RecordCount > 0)
                {
                  lineCommonAttributes.Recordset.MoveFirst();
                  lineCommonAttributes.Recordset.Fields["FEATURE_STATE_C"].Value = "PPI";
                }
                if(lineCUAttributes.Recordset.RecordCount > 0)
                {
                  lineCUAttributes.Recordset.MoveFirst();
                  lineCUAttributes.Recordset.Fields["CU_C"].Value = defaultValues.Fields["PARAM_VALUE"].Value;
                  lineCUAttributes.Recordset.Fields["WR_ID"].Value = dataContext.ActiveJob;                  
                  lineCUAttributes.Recordset.Fields["ACTIVITY_C"].Value = "I";
                  lineCUAttributes.Recordset.Fields["VINTAGE_YR"].Value = DateTime.Today.Year;
                  lineCUAttributes.Recordset.Fields["UNIT_CNO"].Value = 5401;
                  lineCUAttributes.Recordset.Fields["UNIT_CID"].Value = 1;
                  if(lineElectricalAttributes.Recordset.RecordCount > 0)
                  {
                    lineElectricalAttributes.Recordset.MoveFirst();
                    lineCUAttributes.Recordset.Fields["QTY_LENGTH_Q"].Value = lineElectricalAttributes.Recordset.Fields["LENGTH_GRAPHIC_Q"].Value;
                  }
                }                
            }
        }

        public void Pause()
        {
        }

        public void Resume()
        {
        }

        /// <summary>
        /// Ends the process.
        /// </summary>
        public void Terminate()
        {
            dataContext = null;
            gtTransactionManager = null;
            UnsubscribeEvents();
            customCommandHelper = null;
            if (featurePlacementService != null)
            {
                featurePlacementService.Dispose();
                featurePlacementService = null;
            }
            if (featureExplorerService != null)
            {
                featureExplorerService.Clear();
                featureExplorerService.Dispose();
                featureExplorerService = null;
            }
            m_gtApp.EndWaitCursor();
        }
        #endregion

        #region General Methods

        /// <summary>
        /// Subscribes our actions to related feature stuff.
        /// </summary>
        private void SubscribeEvents()
        {
            featurePlacementService.Finished += new EventHandler<GTFinishedEventArgs>(placementFinished);
            customCommandHelper.KeyUp += new EventHandler<GTKeyEventArgs>(keyUp);
            featureExplorerService.SaveClick += new EventHandler(saveClick);
            featureExplorerService.SaveAndContinueClick += new EventHandler(saveAndContinueClick);
            featureExplorerService.CancelClick += new EventHandler(cancelClick);
        }

        private void keyUp (object sender, GTKeyEventArgs e)
        {
            if (e.KeyCode == (short)ConsoleKey.Escape)
            {
                featurePlacementService.CancelPlacement();
                gtTransactionManager.Rollback();
                customCommandHelper.Complete();
            }
        }

        /// <summary>
        /// Unsbscribes our actions to related feature stuff.
        /// </summary>
        private void UnsubscribeEvents()
        {
            featurePlacementService.Finished -= new EventHandler<GTFinishedEventArgs>(placementFinished);
            customCommandHelper.KeyUp -= new EventHandler<GTKeyEventArgs>(keyUp);
            featureExplorerService.SaveClick -= new EventHandler(saveClick);
            featureExplorerService.SaveAndContinueClick -= new EventHandler(saveAndContinueClick);
            featureExplorerService.CancelClick -= new EventHandler(cancelClick);
        }
        
        /// <summary>
        /// Starts the feature explorer after line placement is finished.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void placementFinished (object sender, EventArgs e)
        {
            loadServiceLineData();
            featureExplorerService.ExploreFeature(serviceLine, "Placement");
            if (featureExplorerVisible)
            {
                featureExplorerService.ExploreFeature(serviceLine, "Placement");
                featureExplorerService.Visible = true;
                featureExplorerService.Slide(true);
            }
            else
            {
                saveAndContinueActions();
            }
        }

        /// <summary>
        /// Kicks off the process to start ending our command process, and commits the transaction.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void saveClick(object sender, EventArgs e)
        {
            m_gtApp.BeginWaitCursor();
            gtTransactionManager.Commit();
            featureExplorerService.Visible = featureExplorerVisible;
            customCommandHelper.Complete();
            m_gtApp.EndWaitCursor();
        }

        /// <summary>
        /// Kicks off the process to repeat our command process, commits the current transaction to keep our new feature safe. Clears out some data to make sure the next
        /// feature isn't tainted.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void saveAndContinueClick(object sender, EventArgs e)
        {
            saveAndContinueActions();
        }
        
        private void saveAndContinueActions()
        {
            m_gtApp.BeginWaitCursor();
            gtTransactionManager.Commit();
            if (featureExplorerService.Visible)
            {
                featureExplorerService.Slide(false);
                featureExplorerService.Clear();
            }
            m_gtApp.EndWaitCursor();
            placeServiceLine();
        }

        /// <summary>
        /// Reverts our changes and ends the process. 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cancelClick(object sender, EventArgs e)
        {
            featureExplorerService.Visible = featureExplorerVisible;
            featurePlacementService.CancelPlacement();
            gtTransactionManager.Rollback();
            customCommandHelper.Complete();
        }
        #endregion
    }
}
