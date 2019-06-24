//----------------------------------------------------------------------------+
//  Class: ByPassSPT
//  Description: Silent Placement technic for Bypass scenario
//----------------------------------------------------------------------------+
//   $Author:: Uma Avote                              $
//   $Date:: 15/04/18                                 $
//   $Revision:: 1                                    $
//----------------------------------------------------------------------------+
//    $History:: ByPassSPT.cs                     $
// 
// *****************  Version 1  *****************
// User: Uma Avote     Date: 15/04/18   Time: 10:00  Desc : Created
//----------------------------------------------------------------------------+
using Intergraph.GTechnology.API;
using System;
using ADODB;
using System.Windows.Forms;

namespace GTechnology.Oncor.CustomAPI
{
    public class ByPassSPT : CommonSPTCode
    {
        #region Private Members
        bool bSilent = true;
        string pointLocation = null;
        double xOffset, yOffset;
        short primaryCNO;
        short linearFNO;
        long m_rlnode1 = 0, m_rlnode2 = 0;
        long m_rpnode1 = 0, m_rpnode2 = 0;
        IGTKeyObjects m_KeyObjectCollection = null;
        string currentKeyObject = "";
        const int transformerUGFNO = 60;
        const int AutotransfomerFNO = 34;
        const int ElbowFNO = 41;
        const int IsolationFNO = 6;
        #endregion

        #region IGTPlacementTechnique Members
        /// <summary>
        /// StartPlacement
        /// </summary>
        /// <param name="PTHelper"></param> 
        /// <param name="KeyObject"></param>
        /// <param name="KeyObjectCollection"></param>
        /// <returns></returns>
        public override void StartPlacement(IGTPlacementTechniqueHelper PTHelper, IGTKeyObject KeyObject, IGTKeyObjects KeyObjectCollection)
        {
            try
            {
                m_PTHelper = PTHelper;
                IGTKeyObject m_KeyObject = KeyObject;
                IGTPoint m_gtOrigin;
                IGTGraphicComponent relativeComponent = GTClassFactory.Create<IGTGraphicComponent>();
                m_KeyObjectCollection = KeyObjectCollection;

                //get current Key object
                currentKeyObject = GetcurrentKeyObject(KeyObject.FNO);

                if (bSilent)
                {
                    m_gtApplication.BeginWaitCursor();

                    // Disable construction aids and status bar prompts for silent placement techinque.
                    m_PTHelper.ConstructionAidsEnabled = GTConstructionAidsEnabledConstants.gtptConstructionAidsNone;
                    m_PTHelper.ConstructionAidDynamicsEnabled = false;
                    m_PTHelper.StatusBarPromptsEnabled = true;
                    m_gtApplication.SetStatusBarText(GTStatusPanelConstants.gtaspcMessage, currentKeyObject + " Point placement is in Progress......");

                    IGTComponents gtComponents = KeyObject.Components;
                    m_PTHelper.StartPlacement(KeyObject, KeyObjectCollection);

                    //Read arguments
                    ReadArgument();

                    //Find the relative component defined in the placement technique arguments.
                    if ((Convert.ToString(m_GComp.Arguments.GetArgument(0)) != "") && (m_GComps.Count > 0) && (m_GComp != null))
                    {
                        relativeComponent = AccessRelativeComponent.RetrieveRelativeComponent(Convert.ToString(m_GComp.Arguments.GetArgument(0)), m_GComps, m_GComp);
                        //get the origin
                        m_gtOrigin = GetOrigin(m_KeyObject, gtComponents, relativeComponent, xOffset, pointLocation);

                        if ((m_gtOrigin != null) && (relativeComponent != null))
                        {
                            //Set Attribute values
                            SetAttributeValues(gtComponents, m_KeyObject, relativeComponent);

                            //Create new point geometry aligned to the linear feature
                            IGTOrientedPointGeometry newPointGeometry = (IGTOrientedPointGeometry)PTHelper.CreateGeometry();
                            //newPointGeometry.Orientation = VectorByAngle(ang);
                            newPointGeometry.Origin = m_gtOrigin;
                            m_PTHelper.SetGeometry(newPointGeometry);
                            m_PTHelper.EndPlacement();
                        }
                        else
                        {
                            AbortPlacement();
                        }
                    }
                    else
                    {
                        if (pointLocation == "R")
                        {
                            MessageBox.Show("Error: Unable to place " + currentKeyObject + " Points", "G/Technology", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                        AbortPlacement();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: Unable to place " + currentKeyObject + " Points \n" + ex.Message, "G/Technology", MessageBoxButtons.OK, MessageBoxIcon.Error);
                AbortPlacement();
            }
            finally
            {
                m_KeyObjectCollection = null;
                m_GComps = null;
                m_GComp = null;
                if (pointLocation == "R")
                {
                    m_gtApplication.EndWaitCursor();
                    m_gtApplication.SetStatusBarText(GTStatusPanelConstants.gtaspcMessage, "Feature Placement Completed");
                    m_gtApplication.ActiveMapWindow.MousePointer = GTMousePointerConstants.gtmwmpNWArrow;
                    m_PTHelper.ConstructionAidsEnabled = GTConstructionAidsEnabledConstants.gtptConstructionAidsNone;
                    m_PTHelper.ConstructionAidDynamicsEnabled = false;
                }
                Exitcommand();
            }
        }
        #endregion

        #region Private methods
        /// <summary>
        /// Getcurrent Key Object
        /// </summary>   
        /// <returns></returns>
        private string GetcurrentKeyObject(int KeyObjectFNO)
        {
            //get current Key object
            switch (KeyObjectFNO)
            {
                case ElbowFNO:
                    currentKeyObject = "Elbow";
                    break;
                case IsolationFNO:
                    currentKeyObject = "Isolation";
                    break;
                default:
                    currentKeyObject = "ByPass";
                    break;
            }
            return currentKeyObject;
        }

        /// <summary>
        /// Read Argument
        /// </summary>   
        /// <returns></returns>
        private void ReadArgument()
        {
            xOffset = Convert.ToDouble(m_GComp.Arguments.GetArgument(1));
            yOffset = Convert.ToDouble(m_GComp.Arguments.GetArgument(2));
            statusNormal = m_GComp.Arguments.GetArgument(3).ToString();
            pointLocation = m_GComp.Arguments.GetArgument(4).ToString();
            //Get Located Primary Conductor OH/UG G3Fno from session if exists otherwise get the value from interface arguments
            var fno = GetPropertyValue("linearFNO");
            if (fno != null)
            {
                linearFNO = Convert.ToInt16(fno);
                primaryCNO = GetPrimaryComponent(linearFNO);
            }
            else
            {
                linearFNO = Convert.ToInt16(m_GComp.Arguments.GetArgument(5));
                primaryCNO = Convert.ToInt16(m_GComp.Arguments.GetArgument(6));
            }
        }

        /// <summary>
        /// Get Origin point of point feature
        /// </summary>
        /// <param name="m_gtComponents"></param>
        /// <param name="gtRelativeComponent"></param>
        /// <param name="xOffset"></param>
        /// <param name="pointLocation"></param>
        /// <returns></returns>
        private IGTPoint GetOrigin(IGTKeyObject m_KeyObject, IGTComponents m_gtComponents, IGTGraphicComponent m_gtRelativeComponent, double xOffset, string pointLocation)
        {
            try
            {
                IGTPoint m_gtOriginPoint = null;
                IGTGeometry m_gtLineGeometry = GTClassFactory.Create<IGTGeometry>();
                m_gtApplication.SetStatusBarText(GTStatusPanelConstants.gtaspcMessage, currentKeyObject + " Point placement is in Progress......");

                //Get Relative Component
                IGTKeyObject m_gtRelativePoint = m_gtApplication.DataContext.OpenFeature(m_gtRelativeComponent.FNO, m_gtRelativeComponent.FID);

                //get the related Linear features
                IGTKeyObjects m_gtRelativeLines = GetRelatedFeature(m_gtRelativePoint, iRNO, linearFNO);

                //get the related Linear features in case they are not connected to relativepoint
                if (m_gtRelativeLines.Count == 0)
                {
                    IGTKeyObjects m_gtBypasspt = GetRelatedFeature(m_gtRelativePoint, iRNO, m_KeyObject.FNO);
                    if (m_gtBypasspt.Count > 0)
                    {
                        m_gtRelativeLines = GetRelatedFeature(m_gtBypasspt[0], iRNO, linearFNO);
                    }
                }

                //Check if related line geometry is not null
                if (m_gtRelativeLines != null && m_gtRelativeLines.Count > 0)
                {
                    //Get node values
                    m_gtRelativeLines = GetNodeValues(m_gtRelativePoint, m_gtRelativeLines);
                    m_gtLineGeometry = m_gtRelativeLines[0].Components.GetComponent(primaryCNO).Geometry;

                    //Get Line segment and its angle
                    m_gtLineGeometry = GetSegmentAndAngle(m_gtLineGeometry, m_gtRelativeComponent.Geometry.FirstPoint);

                    if (m_gtLineGeometry != null)
                    {
                        switch (pointLocation)
                        {
                            case "L":
                                //Get Left Origin Point
                                m_gtOriginPoint = GetLeftOriginPoint(m_gtComponents, m_gtRelativePoint, m_gtLineGeometry, m_gtRelativeLines);

                                break;
                            case "R":
                                //Get Right Origin Point
                                m_gtOriginPoint = GetRightOriginPoint(m_gtComponents, m_gtRelativePoint, m_gtLineGeometry);

                                break;
                            case "T":
                                //Get Top Origin Point
                                m_gtOriginPoint = GetTopOriginPoint(m_gtLineGeometry);

                                //establish the connectivity for Relative point with Linear feature                                
                                EstablishRelationship(m_gtRelativePoint, m_gtRelativeLines);
                                break;
                            default:
                                break;
                        }
                        if (m_gtOriginPoint == null)
                        {
                            m_gtOriginPoint = m_gtRelativeComponent.Geometry.FirstPoint;
                        }
                    }
                    else
                    {
                        if (pointLocation == "R")
                        {
                            MessageBox.Show("Error: Unable to place " + currentKeyObject + " Points as the linear feature is not selected properly", "G/Technology", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
                return m_gtOriginPoint;
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Get Top Origin Point
        /// </summary>       
        /// <param name="m_gtLineGeometry"></param> 
        /// <returns></returns>
        private IGTPoint GetTopOriginPoint(IGTGeometry m_gtLineGeometry)
        {
            try
            {
                IGTPoint m_gtTopOriginPoint = null;
                //create offset geoemtry                                    
                IGTGeometry[] geoms = m_gtLineGeometry.CreateOffsetGeometries(xOffset);
                if ((m_rlnode1 != 0) && (m_rlnode1 == m_rpnode1))
                {
                    m_gtTopOriginPoint = geoms[0].FirstPoint;
                }
                if ((m_rlnode2 != 0) && (m_rlnode2 == m_rpnode1))
                {
                    m_gtTopOriginPoint = geoms[0].LastPoint;
                }
                return m_gtTopOriginPoint;
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Get Left Origin Point
        /// </summary>
        /// <param name="m_gtComponents"></param>
        /// <param name="m_gtRelativePoint"></param> 
        /// <param name="m_gtLineGeometry"></param> 
        /// <returns></returns>
        private IGTPoint GetLeftOriginPoint(IGTComponents m_gtComponents, IGTKeyObject m_gtRelativePoint, IGTGeometry m_gtLineGeometry, IGTKeyObjects m_gtRelativeLines)
        {
            try
            {
                //Get Left Origin Point
                IGTPoint m_gtLeftOriginPoint = null;
                IGTRelationshipService m_gtRelationshipService = GTClassFactory.Create<IGTRelationshipService>();

                if ((m_gtRelativePoint.FNO == transformerUGFNO) || (m_gtRelativePoint.FNO == AutotransfomerFNO))
                {
                    //establish the connectivity for Relative point with Linear feature                                    
                    EstablishRelationship(m_gtRelativePoint, m_gtRelativeLines);
                }
                else
                {
                    //set the node value for Bypass point placed at top   
                    if (m_KeyObjectCollection[1] != null)
                    {
                        m_gtRelationshipService.DataContext = m_gtApplication.DataContext;
                        m_gtRelationshipService.ActiveFeature = m_KeyObjectCollection[1];
                        m_gtRelationshipService.SilentDelete(iRNO);
                        IGTComponent m_gtBypassComp = m_KeyObjectCollection[1].Components.GetComponent(ConnectivityCNO);
                        m_gtBypassComp.Recordset.Fields["NODE_1_ID"].Value = 0;
                        m_gtBypassComp.Recordset.Fields["NODE_2_ID"].Value = 0;
                        m_gtBypassComp.Recordset.Update();
                    }
                }

                //set the node value  for Bypass point placed at Left 
                if ((m_rlnode2 != 0) && (m_rlnode2 == m_rpnode1))
                {
                    m_gtLeftOriginPoint = PointAtDistanceBetweenTwoPoints(m_gtLineGeometry.FirstPoint, m_gtLineGeometry.LastPoint, xOffset, 0);
                    m_gtComponents.GetComponent(ConnectivityCNO).Recordset.Fields["NODE_1_ID"].Value = m_rlnode2;
                    m_gtRelativePoint.Components.GetComponent(ConnectivityCNO).Recordset.Fields["NODE_1_ID"].Value = 0;
                    m_gtComponents.GetComponent(ConnectivityCNO).Recordset.Update();
                    m_gtRelativePoint.Components.GetComponent(ConnectivityCNO).Recordset.Update();
                }
                else
                {
                    m_gtLeftOriginPoint = PointAtDistanceBetweenTwoPoints(m_gtLineGeometry.LastPoint, m_gtLineGeometry.FirstPoint, -xOffset, 0);
                    m_gtComponents.GetComponent(ConnectivityCNO).Recordset.Fields["NODE_1_ID"].Value = 0;
                    m_gtRelativePoint.Components.GetComponent(ConnectivityCNO).Recordset.Fields["NODE_2_ID"].Value = m_rlnode1;
                    m_gtRelativePoint.Components.GetComponent(ConnectivityCNO).Recordset.Fields["NODE_1_ID"].Value = 0;
                    m_gtComponents.GetComponent(ConnectivityCNO).Recordset.Update();
                    m_gtRelativePoint.Components.GetComponent(ConnectivityCNO).Recordset.Update();
                }
                m_gtRelationshipService = null;
                return m_gtLeftOriginPoint;
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Set Right Node Values
        /// </summary>
        /// <param name="m_gtComponents"></param>
        /// <param name="m_gtRelativePoint"></param> 
        /// <param name="m_gtLineGeometry"></param> 
        /// <returns></returns>
        private IGTPoint GetRightOriginPoint(IGTComponents m_gtComponents, IGTKeyObject m_gtRelativePoint, IGTGeometry m_gtLineGeometry)
        {
            try
            {
                IGTPoint m_gtRightOriginPoint = null;
                //set the node value  for Bypass point placed at Right   
                if ((m_rlnode1 != 0) && (m_rlnode1 == m_rpnode2))
                {
                    m_gtRightOriginPoint = PointAtDistanceBetweenTwoPoints(m_gtLineGeometry.LastPoint, m_gtLineGeometry.FirstPoint, xOffset, 0);
                    m_gtComponents.GetComponent(ConnectivityCNO).Recordset.Fields["NODE_2_ID"].Value = m_rlnode1;
                    m_gtRelativePoint.Components.GetComponent(ConnectivityCNO).Recordset.Fields["NODE_2_ID"].Value = 0;
                    m_gtComponents.GetComponent(ConnectivityCNO).Recordset.Update();
                    m_gtRelativePoint.Components.GetComponent(ConnectivityCNO).Recordset.Update();
                }
                else
                {
                    m_gtRightOriginPoint = PointAtDistanceBetweenTwoPoints(m_gtLineGeometry.FirstPoint, m_gtLineGeometry.LastPoint, -xOffset, 0);
                    m_gtComponents.GetComponent(ConnectivityCNO).Recordset.Fields["NODE_2_ID"].Value = 0;
                    m_gtRelativePoint.Components.GetComponent(ConnectivityCNO).Recordset.Fields["NODE_2_ID"].Value = 0;
                    m_gtComponents.GetComponent(ConnectivityCNO).Recordset.Update();
                    m_gtRelativePoint.Components.GetComponent(ConnectivityCNO).Recordset.Update();
                }

                return m_gtRightOriginPoint;
            }
            catch
            {
                throw;
            }
        }
        /// <summary>
        /// Establish Relationship
        /// </summary>
        /// <param name="m_gtRelativePoint"></param>
        /// <param name="m_gtRelativeLines"></param>       
        /// <returns></returns>
        public void EstablishRelationship(IGTKeyObject m_gtRelativePoint, IGTKeyObjects m_gtRelativeLines)
        {
            try
            {
                //Fetch the required components
                Recordset m_rsRelativeFirstLine = m_gtRelativeLines[0].Components.GetComponent(ConnectivityCNO).Recordset;
                if (m_rsRelativeFirstLine != null)
                {
                    if (!(m_rsRelativeFirstLine.EOF && m_rsRelativeFirstLine.BOF) && !(m_rsRelativeFirstLine.EOF && m_rsRelativeFirstLine.BOF))
                    {
                        m_rsRelativeFirstLine.MoveFirst();

                        // check if opposite nodes are connected otherwise .. change the node of the related feature
                        if (m_gtRelativeLines.Count == 2)
                        {
                            Recordset m_rsRelativeSecondLine = m_gtRelativeLines[1].Components.GetComponent(ConnectivityCNO).Recordset;
                            if (m_rsRelativeSecondLine != null)
                            {
                                if (!(m_rsRelativeSecondLine.EOF && m_rsRelativeSecondLine.BOF) && !(m_rsRelativeSecondLine.EOF && m_rsRelativeSecondLine.BOF))
                                {
                                    m_rsRelativeSecondLine.MoveFirst();

                                    if ((Convert.ToInt64(m_rsRelativeFirstLine.Fields["NODE_2_ID"].Value)) != (Convert.ToInt64(m_rsRelativeSecondLine.Fields["NODE_1_ID"].Value)))
                                    {
                                        m_rpnode1 = Convert.ToInt64(m_rsRelativeFirstLine.Fields["NODE_2_ID"].Value);
                                        m_rpnode2 = Convert.ToInt64(m_rsRelativeSecondLine.Fields["NODE_1_ID"].Value);
                                    }
                                    else
                                    {
                                        m_rpnode1 = Convert.ToInt64(m_rsRelativeFirstLine.Fields["NODE_2_ID"].Value);
                                        //Get the node value from node edge connectivity sequence
                                        Int64 iNodeID = GetNextNodeID();
                                        m_rpnode2 = iNodeID;
                                        m_rsRelativeSecondLine.Fields["NODE_1_ID"].Value = iNodeID;
                                        m_gtRelativePoint.Components.GetComponent(ConnectivityCNO).Recordset.Fields["NODE_2_ID"].Value = iNodeID;
                                        m_gtRelativePoint.Components.GetComponent(ConnectivityCNO).Recordset.Update();
                                        m_rsRelativeSecondLine.Update();
                                    }
                                }
                            }
                        }
                        //check if active feature is connected at end
                        if (m_gtRelativeLines.Count == 1)
                        {
                            if ((Convert.ToInt64(m_rsRelativeFirstLine.Fields["NODE_2_ID"].Value) != Convert.ToInt64(m_rsRelativeFirstLine.Fields["NODE_1_ID"].Value)) && (Convert.ToInt64(m_gtRelativePoint.Components.GetComponent(ConnectivityCNO).Recordset.Fields["NODE_1_ID"].Value) != 0))
                            {
                                m_rpnode1 = Convert.ToInt64(m_gtRelativePoint.Components.GetComponent(ConnectivityCNO).Recordset.Fields["NODE_1_ID"].Value);
                                m_rpnode2 = 0;
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
        /// Get Node Values
        /// </summary>
        /// <param name="m_gtRelativePoint"></param>
        /// <param name="m_gtRelativeLines"></param>       
        /// <returns></returns>
        public IGTKeyObjects GetNodeValues(IGTKeyObject m_gtRelativePoint, IGTKeyObjects m_gtRelativeLines)
        {
            Recordset m_rsRelativePoint = m_gtRelativePoint.Components.GetComponent(ConnectivityCNO).Recordset;
            if (m_rsRelativePoint != null)
            {
                if (!(m_rsRelativePoint.EOF && m_rsRelativePoint.BOF) && !(m_rsRelativePoint.EOF && m_rsRelativePoint.BOF))
                {
                    m_rsRelativePoint.MoveFirst();

                    //Get the related line node value                
                    if (!Convert.IsDBNull(m_rsRelativePoint.Fields["NODE_1_ID"].Value))
                    {
                        m_rpnode1 = Convert.ToInt64(m_rsRelativePoint.Fields["NODE_1_ID"].Value);
                    }
                    if (!Convert.IsDBNull(m_rsRelativePoint.Fields["NODE_2_ID"].Value))
                    {
                        m_rpnode2 = Convert.ToInt64(m_rsRelativePoint.Fields["NODE_2_ID"].Value);
                    }
                }
            }

            Recordset m_rsRelativeFirstLine = m_gtRelativeLines[0].Components.GetComponent(ConnectivityCNO).Recordset;
            if (m_rsRelativeFirstLine != null)
            {
                if (!(m_rsRelativeFirstLine.EOF && m_rsRelativeFirstLine.BOF) && !(m_rsRelativeFirstLine.EOF && m_rsRelativeFirstLine.BOF))
                {
                    m_rsRelativeFirstLine.MoveFirst();
                    //Get the related line node value
                    if (!Convert.IsDBNull(m_rsRelativeFirstLine.Fields["NODE_1_ID"].Value))
                    {
                        m_rlnode1 = Convert.ToInt64(m_rsRelativeFirstLine.Fields["NODE_1_ID"].Value);
                    }
                    if (!Convert.IsDBNull(m_rsRelativeFirstLine.Fields["NODE_2_ID"].Value))
                    {
                        m_rlnode2 = Convert.ToInt64(m_rsRelativeFirstLine.Fields["NODE_2_ID"].Value);
                    }
                }
            }
            return m_gtRelativeLines;
        }

        /// <summary>
        /// Get Next NodeID
        /// </summary>
        /// <returns></returns>
        private Int64 GetNextNodeID()
        {
            return Convert.ToInt64(GetConnSeqNextVal(iRNO));
        }

        /// <summary>
        /// Get Conn Seq NextVal
        /// </summary>
        /// <param name="RNO"></param>
        /// <returns></returns>
        internal int GetConnSeqNextVal(short RNO)
        {
            int NodeId = 0;
            ADODB.Recordset rs = m_gtApplication.DataContext.MetadataRecordset("G3E_ADD_REL_PARAMS_OPTABLE");
            rs.Filter = "G3E_PARAMNAME = " + RNO + "-NodeEdgeConnectivitySequence";
            //Get the nodeid sequence number
            if (!(rs.Fields["G3E_PARAMVALUE"].Value is DBNull))
            {
                ADODB.Recordset oRSNodeID = m_gtApplication.DataContext.OpenRecordset("select " + rs.Fields["G3E_PARAMVALUE"].Value + ".nextval  from dual", ADODB.CursorTypeEnum.adOpenForwardOnly, ADODB.LockTypeEnum.adLockReadOnly, (int)ADODB.CommandTypeEnum.adCmdText, new Object[1]);
                NodeId = Convert.ToInt32(oRSNodeID.Fields[0].Value);
                oRSNodeID.Close();
            }
            rs.Close();
            return NodeId;
        }

        private short GetPrimaryComponent(short g3eFno)
        {
            short g3eCno = 0;
            ADODB.Recordset rs = m_gtApplication.DataContext.MetadataRecordset("g3e_features_optable", "G3E_FNO=" + g3eFno);
            if (rs != null & rs.RecordCount > 0)
            {
                if (!Convert.IsDBNull(rs.Fields["G3E_PRIMARYGEOGRAPHICCNO"].Value))
                {
                    g3eCno = Convert.ToInt16(rs.Fields["G3E_PRIMARYGEOGRAPHICCNO"].Value);
                }
                else
                {
                    g3eCno = Convert.ToInt16(rs.Fields["G3E_PRIMARYDETAILCNO"].Value);
                }
            }
            return g3eCno;

        }
        #endregion
    }
}