//----------------------------------------------------------------------------+
//  Class: PrimaryEnclosureSPT
//  Description: Silent Placement technic for Elbow Point as per Isolation scenario
//----------------------------------------------------------------------------+
//   $Author:: Uma Avote                              $
//   $Date:: 25/04/18                                 $
//   $Revision:: 1                                    $
//----------------------------------------------------------------------------+
//    $History:: PrimaryEnclosureSPT.cs                     $
// 
// *****************  Version 1  *****************
// User: Uma Avote     Date: 25/04/18   Time: 11:00  Desc : Created
//----------------------------------------------------------------------------+
using Intergraph.GTechnology.API;
using System;
using ADODB;
using System.Windows.Forms;

namespace GTechnology.Oncor.CustomAPI
{
    public class PrimaryEnclosureSPT : CommonSPTCode
    {
        #region Private Members
        bool bSilent = true;
        string pointLocation = null;
        double xOffset, yOffset;
        short primaryCNO;
        int linearFNO;
        long m_rlnode1 = 0, m_rlnode2 = 0;
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
                IGTKeyObjects m_KeyObjectCollection = KeyObjectCollection;
                IGTGraphicComponent relativeComponent = GTClassFactory.Create<IGTGraphicComponent>();
                if (bSilent)
                {
                    m_gtApplication.BeginWaitCursor();

                    // Disable construction aids and status bar prompts for silent placement techinque.
                    m_PTHelper.ConstructionAidsEnabled = GTConstructionAidsEnabledConstants.gtptConstructionAidsNone;
                    m_PTHelper.ConstructionAidDynamicsEnabled = false;
                    m_gtApplication.SetStatusBarText(GTStatusPanelConstants.gtaspcMessage, "Elbow Point placement is in Progress......");

                    IGTComponents gtComponents = KeyObject.Components;
                    m_PTHelper.StartPlacement(KeyObject, KeyObjectCollection);

                    //read arguments
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
                            MessageBox.Show("Error: Unable to place Elbow Points", "G/Technology", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                        AbortPlacement();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: Unable to place Elbow Points \n" + ex.Message, "G/Technology", MessageBoxButtons.OK, MessageBoxIcon.Error);
                AbortPlacement();
            }
            finally
            {
                KeyObjectCollection = null;
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
        /// Read Argument
        /// </summary>   
        /// <returns></returns>
        private void ReadArgument()
        {
            xOffset = Convert.ToDouble(m_GComp.Arguments.GetArgument(1));
            yOffset = Convert.ToDouble(m_GComp.Arguments.GetArgument(2));
            statusNormal = m_GComp.Arguments.GetArgument(3).ToString();
            pointLocation = m_GComp.Arguments.GetArgument(4).ToString();
            linearFNO = Convert.ToInt16(m_GComp.Arguments.GetArgument(5));
            primaryCNO = Convert.ToInt16(m_GComp.Arguments.GetArgument(6));
        }

        /// <summary>
        /// Get Origin of point feature
        /// </summary>
        /// <param name="m_gtComponents"></param>
        /// <param name="m_gtRelativeComponent"></param>
        /// <param name="xOffset"></param>
        /// <param name="pointLocation"></param>
        /// <returns></returns>
        private IGTPoint GetOrigin(IGTKeyObject m_KeyObject, IGTComponents m_gtComponents, IGTGraphicComponent m_gtRelativeComponent, double xOffset, string pointLocation)
        {
            try
            {
                IGTPoint m_gtOriginPoint = null;
                IGTGeometry m_gtLineGeometry = GTClassFactory.Create<IGTGeometry>();
                IGTKeyObject m_gtRelativePoint = m_gtApplication.DataContext.OpenFeature(m_gtRelativeComponent.FNO, m_gtRelativeComponent.FID);
                m_gtApplication.SetStatusBarText(GTStatusPanelConstants.gtaspcMessage, "Elbow Point placement is in Progress......");

                //get the related lines
                IGTKeyObjects m_gtRelativeLines = GetRelatedFeature(m_gtRelativePoint, iRNO, linearFNO);

                //get the related features in case if gtRelativeLines is null
                if (m_gtRelativeLines.Count == 0)
                {
                    IGTKeyObjects m_gtElbowpt = GetRelatedFeature(m_gtRelativePoint, iRNO, m_KeyObject.FNO);
                    if (m_gtElbowpt.Count > 0)
                    {
                        m_gtRelativeLines = GetRelatedFeature(m_gtElbowpt[0], iRNO, linearFNO);
                    }
                }

                //get the related line geometry
                if (m_gtRelativeLines != null && m_gtRelativeLines.Count > 0)
                {
                    //Get node values
                    GetNodeValues(m_gtRelativeLines);
                    m_gtLineGeometry = m_gtRelativeLines[0].Components.GetComponent(primaryCNO).Geometry;

                    //Get Line segment and its angle
                    m_gtLineGeometry = GetSegmentAndAngle(m_gtLineGeometry, m_gtRelativeComponent.Geometry.FirstPoint);

                    if (m_gtLineGeometry != null)
                    {
                        switch (pointLocation)
                        {
                            case "L":
                                //Get Left Origin Point
                                m_gtOriginPoint = GetLeftOriginPoint(m_KeyObject, m_gtComponents, m_gtRelativePoint, m_gtLineGeometry, m_gtRelativeLines);
                                break;
                            case "R":
                                //Get Right Origin Point
                                m_gtOriginPoint = GetRightOriginPoint(m_KeyObject, m_gtRelativeLines[0], m_gtRelativePoint, m_gtLineGeometry);
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
                            MessageBox.Show("Error: Unable to place Elbow Points as the linear feature is not selected properly", "G/Technology", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
        /// Get Left Origin Point
        /// </summary>
        /// <param name="m_gtComponents"></param>
        /// <param name="m_gtRelativePoint"></param> 
        /// <param name="m_gtLineGeometry"></param> 
        /// <returns></returns>
        private IGTPoint GetLeftOriginPoint(IGTKeyObject m_KeyObject, IGTComponents m_gtComponents, IGTKeyObject m_gtRelativePoint, IGTGeometry m_gtLineGeometry, IGTKeyObjects m_gtRelativeLines)
        {
            try
            {
                //Get Left Origin Point
                IGTPoint m_gtLeftOriginPoint = null;
                IGTRelationshipService m_gtRelationshipService = GTClassFactory.Create<IGTRelationshipService>();
                m_gtRelationshipService.DataContext = m_gtApplication.DataContext;
                if (m_rlnode2 != 0)
                {
                    m_gtLeftOriginPoint = PointAtDistanceBetweenTwoPoints(m_gtLineGeometry.FirstPoint, m_gtLineGeometry.LastPoint, xOffset, 0);
                    m_gtRelativeLines[0].Components.GetComponent(ConnectivityCNO).Recordset.Fields["NODE_2_ID"].Value = 0;
                    m_gtRelativeLines[0].Components.GetComponent(ConnectivityCNO).Recordset.Update();

                    //establish the relationship
                    m_gtRelationshipService.ActiveFeature = m_KeyObject;
                    m_gtRelationshipService.SilentEstablish(iRNO, m_gtRelativeLines[0], GTRelationshipOrdinalConstants.gtrelRelationshipOrdinal1, GTRelationshipOrdinalConstants.gtrelRelationshipOrdinal2);
                    m_gtRelativeLines[0].Components.GetComponent(ConnectivityCNO).Recordset.Update();
                    m_gtComponents.GetComponent(ConnectivityCNO).Recordset.Update();
                }
                else
                {
                    m_gtLeftOriginPoint = PointAtDistanceBetweenTwoPoints(m_gtLineGeometry.LastPoint, m_gtLineGeometry.FirstPoint, -xOffset, 0);
                    m_gtRelativeLines[0].Components.GetComponent(ConnectivityCNO).Recordset.Fields["NODE_2_ID"].Value = 0;
                    m_gtRelativeLines[0].Components.GetComponent(ConnectivityCNO).Recordset.Update();
                    m_gtComponents.GetComponent(ConnectivityCNO).Recordset.Update();
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
        private IGTPoint GetRightOriginPoint(IGTKeyObject m_KeyObject, IGTKeyObject m_gtRelativeLine, IGTKeyObject m_gtRelativePoint, IGTGeometry m_gtLineGeometry)
        {
            try
            {
                IGTPoint m_gtRightOriginPoint = null;
                IGTRelationshipService m_gtRelationshipService = GTClassFactory.Create<IGTRelationshipService>();
                m_gtRelationshipService.DataContext = m_gtApplication.DataContext;
                //get the related feature
                IGTKeyObjects m_gtElbow = GetRelatedFeature(m_gtRelativePoint, iRNO, m_KeyObject.FNO);

                //delete the relationship to remove primary conductor
                m_gtRelationshipService.ActiveFeature = m_gtRelativePoint;
                m_gtRelationshipService.SilentDelete(iRNO, GTRelationshipOrdinalConstants.gtrelRelationshipOrdinal1);
                m_gtRelativePoint.Components.GetComponent(ConnectivityCNO).Recordset.Update();
                m_gtElbow[0].Components.GetComponent(ConnectivityCNO).Recordset.Update();
                m_gtRelativeLine.Components.GetComponent(ConnectivityCNO).Recordset.Update();

                //establish the relationhip
                m_gtRelationshipService.SilentEstablish(iRNO, m_gtElbow[0], GTRelationshipOrdinalConstants.gtrelRelationshipOrdinal1, GTRelationshipOrdinalConstants.gtrelRelationshipOrdinal2);
                m_gtRelativePoint.Components.GetComponent(ConnectivityCNO).Recordset.Update();
                m_gtElbow[0].Components.GetComponent(ConnectivityCNO).Recordset.Update();

                if (m_rlnode1 != 0)
                {
                    m_gtRightOriginPoint = PointAtDistanceBetweenTwoPoints(m_gtLineGeometry.LastPoint, m_gtLineGeometry.FirstPoint, xOffset, 0);
                    m_gtRelativeLine.Components.GetComponent(ConnectivityCNO).Recordset.Fields["NODE_1_ID"].Value = 0;
                    m_gtRelativeLine.Components.GetComponent(ConnectivityCNO).Recordset.Update();
                    //establish the relationhip
                    m_gtRelationshipService.ActiveFeature = m_KeyObject;
                    m_gtRelationshipService.SilentEstablish(iRNO, m_gtRelativeLine, GTRelationshipOrdinalConstants.gtrelRelationshipOrdinal2, GTRelationshipOrdinalConstants.gtrelRelationshipOrdinal1);
                    m_KeyObject.Components.GetComponent(ConnectivityCNO).Recordset.Update();
                }
                else
                {
                    m_gtRightOriginPoint = PointAtDistanceBetweenTwoPoints(m_gtLineGeometry.FirstPoint, m_gtLineGeometry.LastPoint, -xOffset, 0);
                    m_gtRelativeLine.Components.GetComponent(ConnectivityCNO).Recordset.Fields["NODE_1_ID"].Value = 0;
                }
                m_gtRelativeLine.Components.GetComponent(ConnectivityCNO).Recordset.Update();
                m_gtRelationshipService = null;
                return m_gtRightOriginPoint;
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
        public void GetNodeValues(IGTKeyObjects m_gtRelativeLines)
        {
            try
            {
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
            }
            catch
            {
                throw;
            }
        }
        #endregion
    }
}
