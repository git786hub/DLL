//----------------------------------------------------------------------------+
//  Class: IsolationSingleSPT
//  Description: Silent Placement technic for Isolation Point as per Isolation scenario
//----------------------------------------------------------------------------+
//   $Author:: Uma Avote                              $
//   $Date:: 20/04/18                                 $
//   $Revision:: 1                                    $
//----------------------------------------------------------------------------+
//    $History:: IsolationSingleSPT.cs                     $
// 
// *****************  Version 1  *****************
// User: Uma Avote     Date: 20/04/18   Time: 10:00  Desc : Created
//----------------------------------------------------------------------------+
using Intergraph.GTechnology.API;
using System.Collections.Generic;
using System;
using ADODB;
using System.Windows.Forms;

namespace GTechnology.Oncor.CustomAPI
{
    public class IsolationSingleSPT : CommonSPTCode
    {
        #region Private Members
        bool bSilent = true;
        string pointLocation = null;
        double xOffset, yOffset;
        short primaryCNO;
        int KeyobjectFNO;
        int linearFNO;
        long m_rlnode1 = 0, m_rlnode2 = 0;
        long m_rpnode1 = 0, m_rpnode2 = 0;

        const int transformerUGFNO = 60;
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
                IGTKeyObjects m_KeyObjectCollection = KeyObjectCollection;
                IGTPoint m_gtOrigin;
                IGTGraphicComponent relativeComponent = GTClassFactory.Create<IGTGraphicComponent>();
                if (bSilent)
                {
                    m_gtApplication.BeginWaitCursor();

                    // Disable construction aids and status bar prompts for silent placement techinque.
                    m_PTHelper.ConstructionAidsEnabled = GTConstructionAidsEnabledConstants.gtptConstructionAidsNone;
                    m_PTHelper.ConstructionAidDynamicsEnabled = false;
                    m_gtApplication.SetStatusBarText(GTStatusPanelConstants.gtaspcMessage, "Isolation Point placement is in Progress......");

                    IGTComponents gtComponents = KeyObject.Components;
                    m_PTHelper.StartPlacement(KeyObject, KeyObjectCollection);

                    //read arguments
                    ReadArgument();
                    KeyobjectFNO = m_KeyObject.FNO;

                    //Find the relative component defined in the placement technique arguments.
                    if ((Convert.ToString(m_GComp.Arguments.GetArgument(0)) != "") && (m_GComps.Count > 0) && (m_GComp != null))
                    {
                        relativeComponent = AccessRelativeComponent.RetrieveRelativeComponent(Convert.ToString(m_GComp.Arguments.GetArgument(0)), m_GComps, m_GComp);
                        //get the origin
                        m_gtOrigin = GetOrigin(KeyObject, gtComponents, relativeComponent, xOffset, pointLocation);

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
                            MessageBox.Show("Error: Unable to place Isolation Points", "G/Technology", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                        AbortPlacement();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: Unable to place Isolation Points \n" + ex.Message, "G/Technology", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
        public void EstablishRelationship(IGTKeyObject m_gtRelativePoint, IGTKeyObjects m_gtRelativeLines)
        {
            try
            {
                //Get the related line node value
                //Fetch the required components
                Recordset m_rsRelativeFirstLine = m_gtRelativeLines[0].Components.GetComponent(ConnectivityCNO).Recordset;
                if (m_rsRelativeFirstLine.RecordCount > 0)
                {
                    if (!(m_rsRelativeFirstLine.EOF && m_rsRelativeFirstLine.BOF) && !(m_rsRelativeFirstLine.EOF && m_rsRelativeFirstLine.BOF))
                    {
                        m_rsRelativeFirstLine.MoveFirst();

                        Recordset m_rsRelativePoint = m_gtRelativePoint.Components.GetComponent(ConnectivityCNO).Recordset;
                        if (m_rsRelativePoint != null)
                        {
                            m_rsRelativePoint.MoveFirst();
                            // check if opposite nodes are connected otherwise .. change the node of the related feature
                            if (m_gtRelativeLines.Count == 2)
                            {
                                Recordset m_rsRelativeSecondLine = m_gtRelativeLines[1].Components.GetComponent(ConnectivityCNO).Recordset;
                                if (m_rsRelativeSecondLine.RecordCount > 0)
                                {
                                    if (!(m_rsRelativeSecondLine.EOF && m_rsRelativeSecondLine.BOF) && !(m_rsRelativeSecondLine.EOF && m_rsRelativeSecondLine.BOF))
                                    {
                                        m_rsRelativeSecondLine.MoveFirst();
                                        if ((Convert.ToInt64(m_rsRelativeFirstLine.Fields["NODE_2_ID"].Value)) == (Convert.ToInt64(m_rsRelativeSecondLine.Fields["NODE_1_ID"].Value)))
                                        {
                                            m_rpnode1 = Convert.ToInt64(m_rsRelativeFirstLine.Fields["NODE_2_ID"].Value);
                                        }
                                        else
                                        {
                                            m_rsRelativeSecondLine.Fields["NODE_1_ID"].Value = m_rsRelativeFirstLine.Fields["NODE_2_ID"].Value;
                                            m_rsRelativeSecondLine.Update();
                                        }
                                    }
                                }
                            }
                            //check if active feature is connected at end of Realted Linear
                            if (m_gtRelativeLines.Count == 1)
                            {
                                if ((Convert.ToInt64(m_rsRelativeFirstLine.Fields["NODE_2_ID"].Value) != Convert.ToInt64(m_rsRelativeFirstLine.Fields["NODE_1_ID"].Value)) && (Convert.ToInt64(m_rsRelativePoint.Fields["NODE_2_ID"].Value) != 0))
                                {
                                    m_rpnode1 = Convert.ToInt64(m_rsRelativePoint.Fields["NODE_2_ID"].Value);
                                    m_rpnode2 = 0;
                                }
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
        /// Get Origin
        /// </summary>
        /// <param name="m_gtComponents"></param>
        /// <param name="m_gtRelativeComponent"></param>
        /// <param name="xOffset"></param>
        /// <param name="pointLocation"></param>
        /// <returns></returns>
        private IGTPoint GetOrigin(IGTKeyObject KeyObject, IGTComponents m_gtComponents, IGTGraphicComponent m_gtRelativeComponent, double xOffset, string pointLocation)
        {
            try
            {
                IGTPoint m_gtOriginPoint = null;
                m_gtApplication.SetStatusBarText(GTStatusPanelConstants.gtaspcMessage, "Isolation Point placement is in Progress......");

                //Get the related Point feature
                IGTKeyObject m_gtRelativePoint = m_gtApplication.DataContext.OpenFeature(m_gtRelativeComponent.FNO, m_gtRelativeComponent.FID);

                //get the related lines
                IGTKeyObjects m_gtRelativeLines = GetRelatedFeature(m_gtRelativePoint, iRNO, linearFNO);

                //get the related Linear features in case they are not connected to relativepoint
                if (m_gtRelativeLines.Count == 0)
                {
                    IGTKeyObjects m_gtBypasspt = GetRelatedFeature(m_gtRelativePoint, iRNO, KeyobjectFNO);
                    if (m_gtBypasspt.Count > 0)
                    {
                        m_gtRelativeLines = GetRelatedFeature(m_gtBypasspt[0], iRNO, linearFNO);
                    }
                }

                //Check if related line geometry is not null
                if (m_gtRelativeLines != null && m_gtRelativeLines.Count > 0)
                {
                    EstablishRelationship(m_gtRelativePoint, m_gtRelativeLines);

                    IGTGeometry m_gtLineGeometry = m_gtRelativeLines[0].Components.GetComponent(primaryCNO).Geometry;

                    if (m_gtLineGeometry != null)
                    {
                        //Get node values
                        GetNodeValues(m_gtRelativePoint, m_gtRelativeLines);

                        //create offset geoemtry
                        if ((m_rlnode2 != 0) && (m_rlnode2 == m_rpnode1))
                        {
                            IGTGeometry[] geoms = m_gtLineGeometry.CreateOffsetGeometries(xOffset);
                            m_gtOriginPoint = geoms[0].LastPoint;

                            //set the node value
                            m_gtComponents.GetComponent(ConnectivityCNO).Recordset.Fields["NODE_1_ID"].Value = m_rlnode2;
                            m_gtRelativePoint.Components.GetComponent(ConnectivityCNO).Recordset.Fields["NODE_1_ID"].Value = 0;
                            m_gtRelativePoint.Components.GetComponent(ConnectivityCNO).Recordset.Fields["NODE_2_ID"].Value = 0;
                            m_gtRelativePoint.Components.GetComponent(ConnectivityCNO).Recordset.Update();
                            m_gtComponents.GetComponent(ConnectivityCNO).Recordset.Update();

                            //set the node value
                            if (m_gtRelativePoint.FNO == transformerUGFNO)
                            {
                                if (m_gtRelativeLines.Count > 1)
                                {
                                    m_gtRelativeLines[1].Components.GetComponent(ConnectivityCNO).Recordset.Fields["NODE_1_ID"].Value = m_rlnode2;
                                    m_gtRelativeLines[1].Components.GetComponent(ConnectivityCNO).Recordset.Update();
                                }
                            }
                        }
                        else
                        {
                            IGTGeometry[] geoms = m_gtLineGeometry.CreateOffsetGeometries(xOffset);
                            m_gtOriginPoint = geoms[0].FirstPoint;

                            //set the node value
                            m_gtComponents.GetComponent(ConnectivityCNO).Recordset.Fields["NODE_1_ID"].Value = m_rlnode1;
                            m_gtRelativePoint.Components.GetComponent(ConnectivityCNO).Recordset.Fields["NODE_1_ID"].Value = 0;
                            m_gtRelativePoint.Components.GetComponent(ConnectivityCNO).Recordset.Fields["NODE_2_ID"].Value = 0;
                            m_gtRelativePoint.Components.GetComponent(ConnectivityCNO).Recordset.Update();
                            m_gtComponents.GetComponent(ConnectivityCNO).Recordset.Update();

                            //set the node value
                            if (m_gtRelativePoint.FNO == transformerUGFNO)
                            {
                                if (m_gtRelativeLines.Count > 1)
                                {
                                    m_gtRelativeLines[1].Components.GetComponent(ConnectivityCNO).Recordset.Fields["NODE_1_ID"].Value = m_rlnode1;
                                    m_gtRelativeLines[1].Components.GetComponent(ConnectivityCNO).Recordset.Update();
                                }
                            }
                        }
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
                        MessageBox.Show("Error: Unable to place Isolation Points as the linear feature is not selected properly", "G/Technology", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                return m_gtOriginPoint;
            }
            catch (Exception)
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
        public void GetNodeValues(IGTKeyObject m_gtRelativePoint, IGTKeyObjects m_gtRelativeLines)
        {
            try
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
            }
            catch
            {
                throw;
            }
        }
        #endregion
    }
}