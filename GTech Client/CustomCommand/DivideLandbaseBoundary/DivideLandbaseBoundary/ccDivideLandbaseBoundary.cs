// =======================================================================================================
//  File Name: ccDivideLandbaseBoundary.cs
// 
//  Description:   Custom command to Divide selected boundaries on Map window
//
//  Remarks:
// 
//  Modification History
//  Date                Author                      Comments
//  --------            --------------              --------
//  19/02/2018         Uma                      Implemented changes in Execute method.                   
// ========================================================================================================
// 28-Jan-2019         Hari                     Fix for ALM-1091 - ONCORDEV-2486 - Split parcel command option is not available in the tool bar.
//


using System;
using System.Windows.Forms;
using ADODB;
using Intergraph.GTechnology.API;
using Intergraph.GTechnology.Interfaces;
using System.Collections;

namespace GTechnology.Oncor.CustomAPI
{
    public class ccDivideLandbaseBoundary : IGTCustomCommandModeless
    {
        #region IGTCustomCommandModeless Variables
        IGTTransactionManager m_oGTTransactionManager;
        IGTCustomCommandHelper m_oGTCustomCommandHelper;
        IGTGeometry m_gSrcGeometry = null;

        IGTApplication m_oGTApplication = GTClassFactory.Create<IGTApplication>();
        IGTGeometryEditService m_oEditService = GTClassFactory.Create<IGTGeometryEditService>();
        IGTPoint m_firstPoint = GTClassFactory.Create<IGTPoint>();
        IGTPoint m_secondtPoint = GTClassFactory.Create<IGTPoint>();
        IGTPolylineGeometry m_gLineGeom = GTClassFactory.Create<IGTPolylineGeometry>();

        int m_sourceFID;
        short m_sourceFNO, m_targetFNO, m_primaryGeoCNO;
        bool m_blFirstPointSelected, m_blSecondPointSelected, m_blPointSelected = false;

        const short m_landbaseCNO = 3;
        const string sMsgBoxCaption = "Divide Landbase Boundary";
        const string sValidationMsg = "The selected feature cannot be divided because it is not an updatable landbase boundary.";
        const string sInterfaceName = "DivideBoundary";
        const string sStatusBarMsg1 = "Please select a first point to divide the boundary";
        const string sStatusBarMsg2 = "Please select a second point to divide the boundary";
        const string sStatusBarMsg3 = "Please ensure both end points of the dividing line are snapped to either a vertex or segment of the selected boundary.";
        #endregion

        #region IGTCustomCommandModeless Members
        /// <summary>
        /// Intialize variables and check selected feature is valid.
        /// </summary>
        /// <param name="CustomCommandHelper"></param>
        public void Activate(Intergraph.GTechnology.API.IGTCustomCommandHelper CustomCommandHelper)
        {
            try
            {
                m_oGTCustomCommandHelper = CustomCommandHelper;
                m_oGTApplication.BeginWaitCursor();

                if (m_oGTApplication.SelectedObjects.FeatureCount > 0)
                {
                    //Get Source feature FNO,FID
                    m_sourceFNO = m_oGTApplication.SelectedObjects.GetObjects()[0].FNO;
                    m_sourceFID = m_oGTApplication.SelectedObjects.GetObjects()[0].FID;

                    //Get Primary Graphic component
                    m_primaryGeoCNO = GetPrimaryGeographicCNO(m_sourceFNO);
                    int m_componentType = GetComponentType(m_primaryGeoCNO);

                    //if selected feature is not valid then show the error message
                    if ((m_componentType != 8) || (!ValidateSelectedFeature(m_sourceFNO)))
                    {
                        MessageBox.Show(sValidationMsg, sMsgBoxCaption, MessageBoxButtons.OK, MessageBoxIcon.Error);
                        ExitCommand();
                    }
                    else
                    {
                        //subscribe the events
                        SubscribeEvents();

                        //Get target feature FNO and source geometry
                        m_targetFNO = GetTargetFeature(m_sourceFNO);
                        m_gSrcGeometry = m_oGTApplication.SelectedObjects.GetObjects()[0].Geometry;

                        m_oGTApplication.SetStatusBarText(GTStatusPanelConstants.gtaspcMessage, sStatusBarMsg1);
                        m_oGTApplication.ActiveMapWindow.MousePointer = GTMousePointerConstants.gtmwmpCrossHair;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error " + ex.Message, sMsgBoxCaption, MessageBoxButtons.OK, MessageBoxIcon.Error);
                ExitCommand();
            }
        }
        public bool CanTerminate
        {
            get { return true; }
        }

        public void Pause()
        {
        }

        public void Resume()
        {
        }

        public void Terminate()
        {
            try
            {
                UnsubscribeEvents();
                if (m_oEditService != null)
                {
                    if (m_oEditService != null)
                        if (m_oEditService.GeometryCount > 0)
                            m_oEditService.RemoveAllGeometries();
                }
                m_oGTApplication.Application.SetStatusBarText(GTStatusPanelConstants.gtaspcMessage, "");
                if (m_oGTTransactionManager != null && m_oGTTransactionManager.TransactionInProgress)
                {
                    m_oGTTransactionManager.Rollback();
                    m_oGTTransactionManager.RefreshDatabaseChanges();
                }
                m_oGTApplication.EndWaitCursor();
                m_oGTApplication.SelectedObjects.Clear();
                m_oGTApplication.RefreshWindows();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error " + ex.Message, sMsgBoxCaption, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                m_oGTApplication = null;
                m_oGTCustomCommandHelper = null;
                m_oGTTransactionManager = null;
            }
        }
        public IGTTransactionManager TransactionManager
        {
            set { m_oGTTransactionManager = value; }
        }
        #endregion

        #region Events
        /// <summary>
        /// Custom command mouse move event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void m_oGTCustomCommandHelper_MouseMove(object sender, GTMouseEventArgs e)
        {
            m_oGTApplication.ActiveMapWindow.MousePointer = GTMousePointerConstants.gtmwmpCrossHair;
            if ((m_blFirstPointSelected))
            {
                m_oGTApplication.SetStatusBarText(GTStatusPanelConstants.gtaspcMessage, sStatusBarMsg2);
                m_blSecondPointSelected = true;
                m_blPointSelected = false;

                m_oEditService.TargetMapWindow = m_oGTApplication.ActiveMapWindow;
                m_oEditService.RemoveAllGeometries();

                //Snap to the Selected Geomentry
                m_secondtPoint = GetSnapPoint(m_gSrcGeometry, e.WorldPoint);

                //Append points to Line geometry
                m_gLineGeom = GTClassFactory.Create<IGTPolylineGeometry>();
                m_gLineGeom.Points.Add(m_firstPoint);
                m_gLineGeom.Points.Add(m_secondtPoint);

                //Add Geometry to edit service
                m_oEditService.AddGeometry(m_gLineGeom, Convert.ToInt32(310000042));

                //Begin move from snap point
                m_oEditService.BeginMove(e.WorldPoint);
            }
            else
            {
                m_oGTApplication.SetStatusBarText(GTStatusPanelConstants.gtaspcMessage, sStatusBarMsg1);
                if (!m_blPointSelected)
                {
                    m_blFirstPointSelected = false;
                    m_blPointSelected = true;
                }
            }
        }

        /// <summary>
        /// Command DblClick event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void m_oGTCustomCommandHelper_DblClick(object sender, GTMouseEventArgs e)
        {
            m_oGTTransactionManager.Rollback();
            ExitCommand();
        }

        /// <summary>
        /// Command Click event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void m_oGTCustomCommandHelper_Click(object sender, GTMouseEventArgs e)
        {
            try
            {
                m_oGTApplication.ActiveMapWindow.MousePointer = GTMousePointerConstants.gtmwmpNWArrow;
                if (m_blPointSelected)
                {
                    //Snap to the Selected Geomentry                    
                    m_firstPoint = GetSnapPoint(m_gSrcGeometry, e.WorldPoint);

                    m_blFirstPointSelected = true;
                    m_blSecondPointSelected = false;
                    m_oGTApplication.SetStatusBarText(GTStatusPanelConstants.gtaspcMessage, sStatusBarMsg1);
                }
                if ((m_blFirstPointSelected) && (m_blSecondPointSelected))
                {
                    try
                    {
                        m_oEditService.EndMove(e.WorldPoint);
                        m_blFirstPointSelected = false;
                        m_oGTTransactionManager.Begin("Divide Landbase Boundries");

                        // Get the Located object and snap the destination to nearest vertex.                       
                        IGTDDCKeyObjects m_oLocatedObjects = m_oGTApplication.ActiveMapWindow.LocateService.Locate(e.WorldPoint, 3, 1, GTSelectionTypeConstants.gtmwstSelectSingle);
                        m_secondtPoint = GetSnapPoint(m_oLocatedObjects[0].Geometry, e.WorldPoint);

                        //Perform the break Polygon
                        IGTGeometry[] m_gTargetPolygons = m_gSrcGeometry.BreakPolygon(m_gLineGeom);

                        //Intialise two polygons
                        IGTKeyObject m_oFirstPolygon = m_oGTApplication.DataContext.OpenFeature(m_sourceFNO, m_sourceFID);
                        IGTKeyObject m_oSecondPolygon = m_oGTApplication.DataContext.NewFeature(m_sourceFNO);

                        //Set the geometry with the newly created ones.
                        m_oFirstPolygon.Components.GetComponent(m_primaryGeoCNO).Geometry = CreatePolygon(m_gTargetPolygons[0]);
                        m_oSecondPolygon.Components.GetComponent(m_primaryGeoCNO).Geometry = CreatePolygon(m_gTargetPolygons[1]);

                        //Fetch the required components
                        Recordset rsFirstFeature = m_oFirstPolygon.Components.GetComponent(m_landbaseCNO).Recordset;
                        Recordset rsSecondFeature = m_oSecondPolygon.Components.GetComponent(m_landbaseCNO).Recordset;

                        //Assign required field values
                        rsSecondFeature.Fields["SOURCE"].Value = rsFirstFeature.Fields["SOURCE"].Value;
                        rsSecondFeature.Fields["CREATED_BY"].Value = m_oGTApplication.DataContext.DatabaseUserName;
                        rsSecondFeature.Fields["CREATED_DATE"].Value = System.DateTime.Today;
                        rsSecondFeature.Fields["STAGE"].Value = "Accepted";

                        m_oGTTransactionManager.Commit();
                        m_oGTTransactionManager.RefreshDatabaseChanges();

                        m_oGTApplication.RefreshWindows();
                        m_oGTApplication.SetStatusBarText(GTStatusPanelConstants.gtaspcMessage, "Commit complete");
                        m_oGTApplication.ActiveMapWindow.MousePointer = GTMousePointerConstants.gtmwmpNWArrow;
                        m_oGTCustomCommandHelper.Complete();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(sStatusBarMsg3, sMsgBoxCaption, MessageBoxButtons.OK, MessageBoxIcon.Error);
                        m_oGTApplication.SetStatusBarText(GTStatusPanelConstants.gtaspcMessage, sStatusBarMsg3);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error " + ex.Message, sMsgBoxCaption, MessageBoxButtons.OK, MessageBoxIcon.Error);
                ExitCommand();
            }
            finally
            {
                if (m_blSecondPointSelected)
                {
                    m_gLineGeom = null;
                    m_gSrcGeometry = null;
                    m_oEditService.RemoveAllGeometries();
                    m_oEditService = null;
                    ExitCommand();
                }
            }
        }
        #endregion

        #region Private methods
        /// <summary>
        /// Method create new polygon
        /// </summary>
        /// <param name="m_TargetPolygon"></param>
        /// <returns></returns>
        private IGTCompositePolygonGeometry CreatePolygon(IGTGeometry m_gTargetPolygon)
        {
            IGTPolylineGeometry m_gPolylineGeometry = GTClassFactory.Create<IGTPolylineGeometry>();
            IGTCompositePolygonGeometry m_gCompositePolygonGeometry = GTClassFactory.Create<IGTCompositePolygonGeometry>();
            IGTGeometryCollection m_gGeometryCollection = GTClassFactory.Create<IGTGeometryCollection>();
            try
            {
                //Remove duplicate points from New polygon
                for (int i = 0; i < m_gTargetPolygon.KeypointCount; i++)
                {
                    if (i > 0 && i < m_gTargetPolygon.KeypointCount - 1)
                    {
                        if (!m_gTargetPolygon.GetKeypointPosition(i).PointsAreEqual(m_gTargetPolygon.GetKeypointPosition(i + 1)))
                            m_gPolylineGeometry.Points.Add(m_gTargetPolygon.GetKeypointPosition(i));
                    }
                    else
                        m_gPolylineGeometry.Points.Add(m_gTargetPolygon.GetKeypointPosition(i));
                }

                //Add polylines to the collection
                m_gGeometryCollection.Add(m_gPolylineGeometry);

                //Build composite polygon 
                foreach (IGTPolylineGeometry m_gtPolylineGeometry in m_gGeometryCollection)
                {
                    m_gCompositePolygonGeometry.Add(m_gtPolylineGeometry);
                }
                return m_gCompositePolygonGeometry;
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Get component type
        /// </summary>        
        /// <param name="CNO"></param>   
        /// <returns></returns>
        public int GetComponentType(short CNO)
        {
            try
            {
                int iReturn = 0;
                ADODB.Recordset oRsFeature = m_oGTApplication.Application.DataContext.MetadataRecordset("G3E_COMPONENTINFO_OPTABLE");
                oRsFeature.Filter = "G3E_CNO=" + CNO;
                iReturn = short.Parse(oRsFeature.Fields["G3E_TYPE"].Value.ToString());
                oRsFeature = null;
                return iReturn;
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Get snap point
        /// </summary>        
        /// <param name="sourceGeometry"></param>
        /// <param name="point"></param>   
        /// <returns></returns>
        private IGTPoint GetSnapPoint(IGTGeometry sourceGeometry, IGTPoint point)
        {
            try
            {
                IGTSnapService snap = GTClassFactory.Create<IGTSnapService>();
                snap.SnapTolerance = 1;
                snap.SnapTypesEnabled = GTSnapTypesEnabledConstants.gtssAllSnaps;
                IGTPoint snapPt; IGTGeometry snapGeom; double distance;
                snap.SnapToGeometry(sourceGeometry, point, out snapPt, out snapGeom, out distance);
                if (distance < snap.SnapTolerance)
                {
                    snap.SnapTolerance = distance;
                    point = snapPt;
                }
                snap = null;
                return point;
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Get Primary Graphic Component of Target feature
        /// </summary>        
        /// <param name="trgFno"></param>   
        /// <returns></returns>
        protected short GetPrimaryGeographicCNO(short trgFno)
        {
            ADODB.Recordset oRSFeature = m_oGTApplication.DataContext.MetadataRecordset("G3E_FEATURES_OPTABLE");
            oRSFeature.Filter = "G3E_FNO=" + trgFno;
            return Convert.ToInt16(oRSFeature.Fields["G3E_PRIMARYGEOGRAPHICCNO"].Value);
        }

        ///<summary>
        ///Get manual feature for selected commercial feature
        ///</summary>
        ///<param name = "srcFno" ></ param >
        /// <returns></returns>      
        private short GetTargetFeature(short srcFno)
        {
            try
            {
                short trgFno = 0;
                Recordset rs = null;
                string sqlStmt = "select Mapping_fno from LBM_FEATURE WHERE lbm_interface='{0}' and SOURCE_FNO={1}";
                rs = Execute(string.Format(sqlStmt, sInterfaceName, srcFno));
                if (rs != null)
                {
                    rs.MoveFirst();
                    if (Convert.ToInt16(rs.Fields["Mapping_fno"].Value) > 0)
                        trgFno = Convert.ToInt16(rs.Fields["Mapping_fno"].Value);
                }
                rs = null;
                return trgFno;
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Validate Selected feature is valid
        /// </summary>
        /// <param name="srcfno"></param>
        /// <returns></returns>
        private bool ValidateSelectedFeature(int srcfno)
        {
            try
            {
                bool flag = false;
                Recordset rs = null;
                string sqlStmt = "select count(*) as cnt from LBM_FEATURE WHERE lbm_interface='{0}' and SOURCE_FNO={1}";
                rs = Execute(string.Format(sqlStmt, sInterfaceName, srcfno));
                if (rs != null)
                {
                    rs.MoveFirst();
                    if (Convert.ToInt32(rs.Fields["cnt"].Value) > 0) { flag = true; }
                }
                rs = null;
                return flag;
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Execute SQL statement
        /// </summary>
        /// <param name="sqlStmt"></param>
        /// <returns></returns>
        private Recordset Execute(string sqlStmt)
        {
            int recordsAffected;
            ADODB.Recordset rs = null;
            rs = m_oGTApplication.DataContext.Execute(sqlStmt, out recordsAffected, (int)ADODB.CommandTypeEnum.adCmdText);
            return rs;
        }

        /// <summary>
        /// Registers the events registered for this Custom command
        /// </summary>
        private void SubscribeEvents()
        {
            // Subscribe to m_oIGTCustomCommandHelper events
            m_oGTCustomCommandHelper.MouseMove += new EventHandler<GTMouseEventArgs>(m_oGTCustomCommandHelper_MouseMove);
            m_oGTCustomCommandHelper.Click += new EventHandler<GTMouseEventArgs>(m_oGTCustomCommandHelper_Click);
            m_oGTCustomCommandHelper.DblClick += new EventHandler<GTMouseEventArgs>(m_oGTCustomCommandHelper_DblClick);
        }

        /// <summary>
        /// Unregisters the events registered for this Custom command
        /// </summary>
        private void UnsubscribeEvents()
        {
            // Unsubscribe to m_oIGTCustomCommandHelper events
            m_oGTCustomCommandHelper.Click -= m_oGTCustomCommandHelper_Click;
            m_oGTCustomCommandHelper.MouseMove -= m_oGTCustomCommandHelper_MouseMove;
            m_oGTCustomCommandHelper.DblClick -= m_oGTCustomCommandHelper_DblClick;
        }

        /// <summary>
        /// Unregisters the events registered for this Custom command
        /// </summary>
        private void ExitCommand()
        {
            if (m_oGTCustomCommandHelper != null)
            {
                m_oGTCustomCommandHelper.Complete();
            }
        }
        #endregion
    }
}