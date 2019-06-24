// =======================================================================================================
//  File Name: ccCopyLandbaseBoundary.cs
// 
//  Description:   Custom command copy two selected boundaries on Map window
//
//  Remarks:
// 
//  Modification History
//  Date                Author                      Comments
//  --------            --------------              --------
//  19/02/2017         Uma                      Implemented changes in Execute method.                   
// ========================================================================================================

using System;
using System.Windows.Forms;
using ADODB;
using Intergraph.GTechnology.API;
using Intergraph.GTechnology.Interfaces;
using System.Collections;

namespace GTechnology.Oncor.CustomAPI
{
    public class ccCopyLandbaseBoundary : IGTCustomCommandModeless
    {
        #region IGTCustomCommandModeless Variables
        IGTTransactionManager m_oGTTransactionManager;
        IGTApplication m_oGTApplication = null;
        IGTCustomCommandHelper m_oGTCustomCommandHelper;
        IGTDDCKeyObjects m_locatedObjects = null;
        IGTGeometry m_osrcGeometry = null;
        IGTComponent m_trgComponent = null;
        
        int m_sourceFID, m_componentType;
        short m_sourceFNO, m_targetFNO, m_primaryGeoCNO;
        bool m_blFirstPointSelected, m_blSecondPointSelected, m_blPointSelected = false;
        int m_styleID = Convert.ToInt32(GTStyleIDConstants.gtstyleLineSelectSolid2);
        int m_iEditIndex = 1;

        IGTKeyObject m_otrgFeature = GTClassFactory.Create<IGTKeyObject>();
        IGTPoint m_snapPoint = GTClassFactory.Create<IGTPoint>();
        IGTGeometryEditService m_oEditService = GTClassFactory.Create<IGTGeometryEditService>();
        Hashtable m_EditObjects = new Hashtable();
 
        const string m_sMsgBoxCaption = "Copy Landbase Boundary";
        const string m_sValidationMsg = "The selected feature cannot be copied because it is not an updatable landbase boundary.";
        const string m_sInterfaceName = "CopyBoundary";
        const string m_sStatusBarMsg1 = "Select origin point to copy the boundary";
        const string m_sStatusBarMsg2 = "Select destination point to copy the boundary";

        #endregion

        #region IGTCustomCommandModeless Members
        /// <summary>
        /// Intialize variables and check selected feature is valid.
        /// </summary>
        /// <param name="CustomCommandHelper"></param>
        /// <returns></returns>
        public void Activate(Intergraph.GTechnology.API.IGTCustomCommandHelper CustomCommandHelper)
        {
            try
            {
                m_oGTApplication = GTClassFactory.Create<IGTApplication>();
                m_oGTCustomCommandHelper = CustomCommandHelper;
                m_oGTApplication.BeginWaitCursor();
                if (m_oGTApplication.SelectedObjects.FeatureCount > 0)
                {
                    //Get Source feature FNO,FID
                    m_sourceFNO = m_oGTApplication.SelectedObjects.GetObjects()[0].FNO;
                    m_sourceFID = m_oGTApplication.SelectedObjects.GetObjects()[0].FID;

                    //Get Primary graphic CNO and Type
                    m_primaryGeoCNO = GetPrimaryGeographicCNO(m_sourceFNO);
                    m_componentType = GetComponentType(m_primaryGeoCNO);

                    //if selected feature is not valid then show the error message
                    if ((m_componentType != 8) || (!ValidateSelectedFeature(m_sourceFNO)))
                    {
                        MessageBox.Show(m_sValidationMsg, m_sMsgBoxCaption, MessageBoxButtons.OK, MessageBoxIcon.Error);
                        ExitCommand();
                    }
                    else
                    {
                        SubscribeEvents();
                        //Get target feature FNO
                        m_targetFNO = GetTargetFeature(m_sourceFNO);
                        m_osrcGeometry = m_oGTApplication.SelectedObjects.GetObjects()[0].Geometry;
                        m_oGTApplication.SetStatusBarText(GTStatusPanelConstants.gtaspcMessage, m_sStatusBarMsg1);
                        m_oGTApplication.ActiveMapWindow.MousePointer = GTMousePointerConstants.gtmwmpCrossHair;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error " + ex.Message, m_sMsgBoxCaption, MessageBoxButtons.OK, MessageBoxIcon.Error);
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
                MessageBox.Show("Error " + ex.Message, m_sMsgBoxCaption, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                m_oGTApplication = null;
                m_oGTCustomCommandHelper = null;
                m_osrcGeometry = null;
                m_otrgFeature = null;
                m_trgComponent = null;
                m_oEditService = null;
                m_oGTTransactionManager = null;
                m_EditObjects = null;
                m_locatedObjects = null;
                m_snapPoint = null;                
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
        /// <param name="sender">object sender</param>
        /// <param name="e">GTMouseEventArgs</param>
        /// <returns></returns>
        void m_oGTCustomCommandHelper_MouseMove(object sender, GTMouseEventArgs e)
        {
            m_oGTApplication.ActiveMapWindow.MousePointer = GTMousePointerConstants.gtmwmpCrossHair;
            if ((m_blFirstPointSelected))
            {
                m_oGTApplication.SetStatusBarText(GTStatusPanelConstants.gtaspcMessage, m_sStatusBarMsg2);
                //move the selected feature
                m_oEditService.Move(e.WorldPoint);
                m_blSecondPointSelected = true;
                m_blPointSelected = false;
                m_locatedObjects = m_oGTApplication.ActiveMapWindow.LocateService.Locate(e.WorldPoint, 3, 1, GTSelectionTypeConstants.gtmwstSelectSingle);
            }
            else
            {
                m_oGTApplication.SetStatusBarText(GTStatusPanelConstants.gtaspcMessage, m_sStatusBarMsg1);
                if (!m_blPointSelected)
                {
                    m_blFirstPointSelected = false;
                    m_blPointSelected = true;
                }
            }
        }
		 /// <summary>
        /// Command Double Click
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">GTMouseEventArgs</param>
        /// <returns></returns>
        private void m_oGTCustomCommandHelper_DblClick(object sender, GTMouseEventArgs e)
        {
            m_oGTTransactionManager.Rollback();
            ExitCommand();
        }
        /// <summary>
        /// Command Click event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">GTMouseEventArgs</param>
        /// <returns></returns>
        void m_oGTCustomCommandHelper_Click(object sender, GTMouseEventArgs e)
        {
            try
            {                
               m_oGTApplication.ActiveMapWindow.MousePointer = GTMousePointerConstants.gtmwmpCrossHair;
               if (m_blPointSelected)
                {
                    //Get snap point                    
                    if (m_osrcGeometry != null)
                    {
                        m_snapPoint = GetSnapPoint(m_osrcGeometry, e.WorldPoint);
                    }
                    else
                    {
                        m_snapPoint = e.WorldPoint;
                    }
                    if (m_oGTApplication.SelectedObjects.FeatureCount > 0)
                    {
                        m_oGTTransactionManager.Begin("Copy Landbase Boundries");
                        m_oEditService.TargetMapWindow = m_oGTApplication.ActiveMapWindow;
                        m_oEditService.RemoveAllGeometries();
                        //Create Copy of feature
                        bool bCopyFeature = CopyFeature(m_sourceFNO, m_targetFNO);
                        if (bCopyFeature)
                        {
                            //Lets start moving the feature from snap point
                            m_oEditService.BeginMove(m_snapPoint);
                            m_blFirstPointSelected = true;
                            m_blSecondPointSelected = false;
                            m_oGTApplication.SetStatusBarText(GTStatusPanelConstants.gtaspcMessage, m_sStatusBarMsg1);
                        }
                    }
                }
                if ((m_blFirstPointSelected) && (m_blSecondPointSelected))
                {
                    m_blFirstPointSelected = false;

                    //Get the Located object
                    m_locatedObjects = m_oGTApplication.ActiveMapWindow.LocateService.Locate(e.WorldPoint, 3, 1, GTSelectionTypeConstants.gtmwstSelectSingle);

                    //snap the destination to nearest vertex.
                    if (m_locatedObjects.Count > 0)
                    {
                        m_snapPoint = GetSnapPoint(m_locatedObjects[0].Geometry, e.WorldPoint);
                    }
                    else
                    {
                        m_snapPoint = e.WorldPoint;
                    }
                    
                    //End move to snap point
                    m_oEditService.EndMove(m_snapPoint);

                    //Assign the copied geometry to target feature
                    foreach (int i in m_EditObjects.Keys)
                    {
                        m_otrgFeature.Components.GetComponent(Convert.ToInt16(m_EditObjects[i].ToString())).Geometry = m_oEditService.GetGeometry(i);
                    }
                    m_oGTApplication.ActiveMapWindow.MousePointer = GTMousePointerConstants.gtmwmpNWArrow;
                    m_oEditService.RemoveAllGeometries();

                    m_oGTTransactionManager.Commit();
                    m_oGTTransactionManager.RefreshDatabaseChanges();

                    m_oGTCustomCommandHelper.Complete();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error " + ex.Message, m_sMsgBoxCaption, MessageBoxButtons.OK, MessageBoxIcon.Error);
                m_oGTApplication.ActiveMapWindow.MousePointer = GTMousePointerConstants.gtmwmpNWArrow;
                ExitCommand();
            }
            finally
            {
                if (m_blSecondPointSelected)
                {
                    ExitCommand();
                }
            }
        }
        #endregion

        #region Private methods
        /// <summary>
        /// Method to Get Component Values Of Differnt Feature
        /// </summary>
        /// <param name="m_trgComponents">Target Component Number</param>
        /// <param name="m_srcComponent">Source Component Number</param>
        /// <param name="k"> Edit index</param>
        /// <returns></returns>
        private void GetComponentValuesOfDifferntFeature(IGTComponents m_trgComponents, IGTComponent m_srcComponent, int k)
        {
            try
            {
                foreach (IGTComponent m_trgComponent in m_trgComponents)
                {
                    //check if target component type is same as source component type
                    if ((GetGeometryType(m_trgComponent.CNO) == (GetGeometryType(m_srcComponent.CNO))) && (m_trgComponent.TableName != m_srcComponent.TableName) && (GetDetail(m_trgComponent.CNO) == 0))
                    {
                        //check if source geometry is valid, if so add to edit service
                        if ((m_srcComponent.Geometry != null && m_trgComponent.Geometry == null))
                        {
                            m_oEditService.AddGeometry(m_srcComponent.Geometry, m_styleID);
                            m_EditObjects.Add(k, m_trgComponent.CNO);
                            m_iEditIndex++;
                            break;
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
        /// Method Create new feature
        /// </summary>              
        /// <param name="srcFno">Source Feature Number</param>
        /// <param name="trgFno">Traget Feature Number</param>
        /// <returns></returns>
        private bool CopyFeature(short srcFno, short trgFno)
        {
            try
            {
                IGTComponents m_srcComponents = m_oGTApplication.DataContext.OpenFeature(srcFno, m_sourceFID).Components;                
                m_otrgFeature = m_oGTApplication.DataContext.NewFeature(trgFno);
                IGTComponents m_trgComponents = m_oGTApplication.DataContext.OpenFeature(trgFno, m_otrgFeature.FID).Components;
                foreach (IGTComponent m_srcComponent in m_srcComponents)
                {
                    Recordset rsOldFeature = m_srcComponent.Recordset;
                    //Get target component if target component is same as source component
                    m_trgComponent = m_otrgFeature.Components.GetComponent(m_srcComponent.CNO);

                    //if target component is null for different feature type
                    if (m_trgComponent == null)
                    {
                        if (GetComponentType(m_srcComponent.CNO) != 1)
                        {
                            GetComponentValuesOfDifferntFeature(m_trgComponents, m_srcComponent, m_iEditIndex);
                        }
                        
                    }
                    //if target component is not null                    
                    else
                    {  
                            Recordset rsNewFeature = m_trgComponent.Recordset;
                            //check if target component is 3 if so then set the values
                            if (rsNewFeature != null)
                            {
                                if (m_trgComponent.CNO == 3)
                                {
                                    int m_newG3eId = Convert.ToInt32(rsNewFeature.Fields["G3E_ID"].Value);
                                    rsNewFeature.Fields["CREATED_BY"].Value = m_oGTApplication.DataContext.DatabaseUserName;
                                    rsNewFeature.Fields["CREATED_DATE"].Value = System.DateTime.Today;
                                    rsNewFeature.Fields["SOURCE"].Value = rsOldFeature.Fields["SOURCE"].Value;
                                    rsNewFeature.Fields["STAGE"].Value = "Accepted";

                                }
                                else
                                {
                                    GetComponentValuesOfSameFeature(rsNewFeature, rsOldFeature, m_trgComponent, m_srcComponent, m_iEditIndex);
                                }                               
                            }                        
                    }                   
                }                
                return true;
            }
            catch { throw; }            
        }

        /// <summary>
        /// Method to Get Component Values Of Differnt Feature
        /// </summary>
        /// <param name="rsNewFeature">Target Recordset</param>
        /// <param name="rsOldFeature">Source Recordset</param>
        /// /// <param name="m_trgComponent">Target component number</param>
        /// <param name="m_srcComponent">Source component number></param>
        /// <param name="K">Edit Index</param>
        private void GetComponentValuesOfSameFeature(Recordset rsNewFeature, Recordset rsOldFeature,IGTComponent m_trgComponent, IGTComponent m_srcComponent,int k)
        {
            try
            {
                int count = 0;
                while (!rsOldFeature.EOF)
                {
                    if ((rsNewFeature.RecordCount == 0) || rsNewFeature.EOF || count > 0)
                    {
                        rsNewFeature.AddNew("G3E_FID", m_otrgFeature.FID);
                        rsNewFeature.Fields["G3E_FNO"].Value = m_otrgFeature.FNO;
                        rsNewFeature.Fields["G3E_CNO"].Value = m_trgComponent.CNO;
                        rsNewFeature.Fields["G3E_CID"].Value = m_srcComponent.Recordset.Fields["G3E_CID"].Value;
                    }
                    //check if source geometry is valid, if so add to edit service
                    if ((m_srcComponent.Geometry != null && m_trgComponent.Geometry == null))
                    {
                        m_oEditService.AddGeometry(m_srcComponent.Geometry, m_styleID);
                        m_EditObjects.Add(k, m_trgComponent.CNO);
                        m_iEditIndex++;
                    }
                    //Add attribute values to target component in case fieldname is same as source component
                    for (int i = 0; i < rsOldFeature.Fields.Count; i++)
                    {
                        string fieldName = Convert.ToString(rsOldFeature.Fields[i].Name);
                        if ((m_srcComponent.CNO == Convert.ToInt16(rsNewFeature.Fields["G3E_CNO"].Value)) && (fieldName.Substring(0, 3) != "G3E") && (fieldName.Substring(0, 3) != "LTT"))
                        {
                            rsNewFeature.Fields[fieldName].Value = rsOldFeature.Fields[fieldName].Value;
                        }
                    }
                    count += 1;
                    rsOldFeature.MoveNext();
                }
            }
            catch 
            {
                throw;
            }
        }

        /// <summary>
        /// Get component type
        /// </summary>        
        /// <param name="CNO">component number</param>
        /// <returns></returns>
        public int GetComponentType(short CNO)
        {
            try
            {
                int iReturn = 0;
                ADODB.Recordset oRsFeature = m_oGTApplication.Application.DataContext.MetadataRecordset("G3E_COMPONENTINFO_OPTABLE");
                oRsFeature.Filter = "G3E_CNO=" + CNO;
                iReturn = short.Parse(oRsFeature.Fields["G3E_TYPE"].Value.ToString());
                return iReturn;
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Get component geoemtry type
        /// </summary>        
        /// <param name="CNO">component number</param>   
        /// <returns></returns>
        public string GetGeometryType(short CNO)
        {
            try
            {
                string sReturn = "";
                ADODB.Recordset oRsFeature = m_oGTApplication.Application.DataContext.MetadataRecordset("G3E_COMPONENTINFO_OPTABLE");
                oRsFeature.Filter = "G3E_CNO=" + CNO;
                sReturn = oRsFeature.Fields["G3E_GEOMETRYTYPE"].Value.ToString();
                return sReturn;
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Get detail value
        /// </summary>        
        /// <param name="CNO">component number</param>   
        /// <returns></returns>
        public int GetDetail(short CNO)
        {
            try
            {
                int iReturn = 0;
                ADODB.Recordset oRsFeature = m_oGTApplication.Application.DataContext.MetadataRecordset("G3E_COMPONENTINFO_OPTABLE");
                oRsFeature.Filter = "G3E_CNO=" + CNO;
                iReturn = Convert.ToInt16(oRsFeature.Fields["G3E_DETAIL"].Value);
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
        /// <param name="sourceGeometry">Source geometry</param>
        /// <param name="point">snap point</param>   
        /// <returns></returns>
        private IGTPoint GetSnapPoint(IGTGeometry sourceGeometry, IGTPoint point)
        {
            try
            { 
                IGTSnapService snap = GTClassFactory.Create<IGTSnapService>();
                snap.SnapTolerance = 15;
                snap.SnapTypesEnabled = GTSnapTypesEnabledConstants.gtssSnapToVertex;
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
        /// <param name="trgFno">Target feature number</param> 
        /// /// <param name="cno">component number</param>
        /// <returns></returns>
        private short GetPrimaryGeographicCNO(short trgFno)
        {
            try
            {
                ADODB.Recordset oRSFeature = m_oGTApplication.DataContext.MetadataRecordset("G3E_FEATURES_OPTABLE");
                oRSFeature.Filter = "G3E_FNO=" + trgFno;
                return Convert.ToInt16(oRSFeature.Fields["G3E_PRIMARYGEOGRAPHICCNO"].Value);
            }
            catch
            {
                throw;
            }
        }

        ///<summary>
        ///Get Target feature for supplied source feature
        ///</summary>
        ///<param name = "srcFno" >Source feature number</ param >
        /// <returns></returns>      
        private short GetTargetFeature(short srcFno)
        {
            try
            {
                short trgFno = 0;
                Recordset rs = null;
                string sqlStmt = "select Mapping_fno from LBM_FEATURE WHERE lbm_interface='{0}' and SOURCE_FNO={1}";
                rs = Execute(string.Format(sqlStmt, m_sInterfaceName, srcFno));
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
        /// <param name="srcfno">Source feature number</param>
        /// <returns></returns>
        private bool ValidateSelectedFeature(int srcfno)
        {
            try
            { 
                bool flag = false;
                Recordset rs = null;
                string sqlStmt = "select count(*) as cnt from LBM_FEATURE WHERE lbm_interface='{0}' and SOURCE_FNO={1}";
                rs = Execute(string.Format(sqlStmt, m_sInterfaceName, srcfno));
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
        /// <param name="sqlStmt">sql statement</param>
        /// <returns></returns>
        private Recordset Execute(string sqlStmt)
        {
            try
            {                           
                int recordsAffected;
                ADODB.Recordset rs = null;
                rs = m_oGTApplication.DataContext.Execute(sqlStmt, out recordsAffected, (int)ADODB.CommandTypeEnum.adCmdText);
                return rs;
            }
            catch
            {
                throw;
            }
        }
        /// <summary>
        /// registers the events registered for this Custom command
        /// </summary>
        private void SubscribeEvents()
        {
            m_oGTCustomCommandHelper.MouseMove += new EventHandler<GTMouseEventArgs>(m_oGTCustomCommandHelper_MouseMove);
            m_oGTCustomCommandHelper.Click += new EventHandler<GTMouseEventArgs>(m_oGTCustomCommandHelper_Click);
            m_oGTCustomCommandHelper.DblClick += new EventHandler<GTMouseEventArgs>(m_oGTCustomCommandHelper_DblClick);
        }

        /// <summary>
        /// Unregisters the events registered for this Custom command
        /// </summary>
        private void UnsubscribeEvents()
        {
            m_oGTCustomCommandHelper.Click -= m_oGTCustomCommandHelper_Click;
            m_oGTCustomCommandHelper.MouseMove -= m_oGTCustomCommandHelper_MouseMove;
            m_oGTCustomCommandHelper.DblClick -= m_oGTCustomCommandHelper_DblClick;
        }
        /// <summary>
        /// Exit command
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