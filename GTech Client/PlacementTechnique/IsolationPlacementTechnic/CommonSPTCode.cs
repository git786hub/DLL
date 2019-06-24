//----------------------------------------------------------------------------+
//  Class: CommonSPTCode
//  Description: The class acts as a common code layer for all the silent placement techniques
//----------------------------------------------------------------------------+
//   $Author:: Uma Avote                              $
//   $Date:: 10/04/18                                 $
//   $Revision:: 1                                    $
//----------------------------------------------------------------------------+
//    $History:: CommonSPTCode.cs                     $
// 
// *****************  Version 1  *****************
// User: Uma Avote     Date: 10/04/18   Time:   Desc : Created
//----------------------------------------------------------------------------+
using System;
using Intergraph.GTechnology.Interfaces;
using Intergraph.GTechnology.API;
using System.Windows.Forms;
using System.Collections.Generic;

namespace GTechnology.Oncor.CustomAPI
{
    public class CommonSPTCode : IGTPlacementTechnique
    {
        #region public Members
        public IGTApplication m_gtApplication = GTClassFactory.Create<IGTApplication>();
        public IGTPlacementTechniqueHelper m_PTHelper = null;
        public IGTGraphicComponents m_GComps = null;
        public IGTGraphicComponent m_GComp = null;
        public string statusNormal = null;
        public const short ConnectivityCNO = 11;
        public const int iRNO = 14;

        #endregion

        #region private Members
        bool bSilent = true;
        const short VirtualAttributeCNO = 4;
        const short VirtualPointCNO = 78;
        #endregion

        #region IGTPlacementTechnique Members
        public virtual void AbortPlacement()
        {
            if (m_PTHelper != null)
            {
                m_PTHelper.AbortPlacement();
            }
        }

        public virtual IGTGraphicComponent GraphicComponent
        {
            get
            {
                return m_GComp;
            }
            set
            {
                m_GComp = value;
            }
        }
        public virtual IGTGraphicComponents GraphicComponents
        {
            get
            {
                return m_GComps;
            }
            set
            {
                m_GComps = value;
            }
        }
        public virtual void KeyUp(IGTMapWindow MapWindow, int KeyCode, int ShiftState, Intergraph.GTechnology.API.IGTPlacementTechniqueEventMode EventMode)
        {
            if (!bSilent)
            {

            }
        }
        public virtual void MouseClick(Intergraph.GTechnology.API.IGTMapWindow MapWindow, Intergraph.GTechnology.API.IGTPoint UserPoint, int Button, int ShiftState, Intergraph.GTechnology.API.IGTDDCKeyObjects LocatedObjects, Intergraph.GTechnology.API.IGTPlacementTechniqueEventMode EventMode)
        {
            if (!bSilent)
            {
                m_PTHelper.MouseClick(UserPoint, Button, ShiftState);
            }
        }
        public virtual void MouseDblClick(Intergraph.GTechnology.API.IGTMapWindow MapWindow, Intergraph.GTechnology.API.IGTPoint UserPoint, int ShiftState, Intergraph.GTechnology.API.IGTDDCKeyObjects LocatedObjects)
        {
            if (!bSilent)
            {
                if (m_PTHelper != null)
                {
                    m_PTHelper.MouseDblClick(UserPoint, ShiftState);
                }
                if (m_gtApplication != null)
                {
                    m_gtApplication.SetStatusBarText(GTStatusPanelConstants.gtaspcMessage, "Please click on related Primary Conductor feature.");
                }
            }
        }
        public virtual void MouseMove(Intergraph.GTechnology.API.IGTMapWindow MapWindow, Intergraph.GTechnology.API.IGTPoint UserPoint, int ShiftState, Intergraph.GTechnology.API.IGTDDCKeyObjects LocatedObjects, Intergraph.GTechnology.API.IGTPlacementTechniqueEventMode EventMode)
        {
            if (!bSilent)
            {
                if (m_PTHelper != null)
                {
                    m_PTHelper.MouseMove(UserPoint, ShiftState);
                }
            }
        }
        public virtual void StartPlacement(IGTPlacementTechniqueHelper PTHelper, IGTKeyObject KeyObject, IGTKeyObjects KeyObjectCollection)
        {
            try
            {
                m_PTHelper = PTHelper;
                IGTKeyObject m_KeyObject = KeyObject;
                IGTKeyObjects m_KeyObjectCollection = KeyObjectCollection;

                if (bSilent)
                {
                    // Disable construction aids and status bar prompts for silent placement techinque.
                    m_PTHelper.ConstructionAidsEnabled = GTConstructionAidsEnabledConstants.gtptConstructionAidsNone;
                    m_PTHelper.ConstructionAidDynamicsEnabled = false;
                    m_PTHelper.StatusBarPromptsEnabled = false;
                    m_PTHelper.StartPlacement(m_KeyObject, m_KeyObjectCollection);

                    // End Placement
                    m_PTHelper.EndPlacement();
                }
                else
                {
                    m_PTHelper.StatusBarPromptsEnabled = true;
                    m_PTHelper.StartPlacement(m_KeyObject, m_KeyObjectCollection);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error in Isolation Scenario PlacementTechnic Interface \n" + ex.Message, "G /Technology", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// m_PTHelper_ConstructionAidComplete
        /// </summary>
        /// <param name="sender"></param>       
        /// <returns></returns>
        public virtual void m_PTHelper_ConstructionAidComplete(object sender, GTConstructionAidCompleteEventArgs e)
        {

        }
        public virtual void m_PTHelper_ArcComplete(object sender, EventArgs e)
        {
        }
        public void Exitcommand()
        {
            if (m_gtApplication != null)
            {
                m_gtApplication.EndWaitCursor();
                m_gtApplication = null;
            }
            if (m_PTHelper != null)
            {
                m_PTHelper.StatusBarPromptsEnabled = false;
                m_PTHelper.ConstructionAidsEnabled = GTConstructionAidsEnabledConstants.gtptConstructionAidsNone;
                m_PTHelper.ConstructionAidDynamicsEnabled = false;
                m_PTHelper = null;
            }
        }
        #endregion

        #region Private Methods
        /// <summary>
        /// PointAtDistanceBetweenTwoPoints
        /// </summary>
        /// <param name="p1"></param>
        /// <param name="p2"></param>
        /// <param name="xoffset"></param>
        /// <param name="yOffset"></param>
        /// <returns></returns>
        protected IGTPoint PointAtDistanceBetweenTwoPoints(IGTPoint p1, IGTPoint p2, double xoffset, double yOffset)
        {
            try
            {
                double totalDistance = DistanceOfTwoPoints(p1, p2);
                double ratio = xoffset / totalDistance;
                //TODO: 3D
                IGTPoint point = GTClassFactory.Create<IGTPoint>();
                point.X = p2.X - (ratio * (p2.X - p1.X));
                point.Y = p2.Y - (ratio * (p2.Y - p1.Y));
                point.Z = 0;
                return point;
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// DistanceOfTwoPoints
        /// </summary>
        /// <param name="p1"></param>
        /// <param name="p2"></param>       
        /// <returns></returns>
        protected double DistanceOfTwoPoints(IGTPoint p1, IGTPoint p2)
        {
            return Math.Sqrt(((p2.X - p1.X) * (p2.X - p1.X)) + ((p2.Y - p1.Y) * (p2.Y - p1.Y)) /*+ TODO: Z for 3D*/);
        }

        /// <summary>
        /// GetRelatedFeatures
        /// </summary>
        /// <param name="gtActiveFeature"></param>
        /// <param name="RNO"></param>
        /// <param name="relatedFNO"></param>
        /// <returns></returns>
        protected IGTKeyObjects GetRelatedFeature(IGTKeyObject activeFeature, short RNO, int relatedFNO)
        {
            try
            {
                IGTRelationshipService m_gtRelationshipService = GTClassFactory.Create<IGTRelationshipService>();
                m_gtRelationshipService.DataContext = m_gtApplication.DataContext;
                m_gtRelationshipService.ActiveFeature = activeFeature;
                IGTKeyObjects m_gtRelatedFeatures = m_gtRelationshipService.GetRelatedFeatures(RNO);
                if (relatedFNO > 0)
                {
                    for (int i = m_gtRelatedFeatures.Count; 0 < i; i--)
                    {
                        if ((m_gtRelatedFeatures[i - 1].FNO) != relatedFNO) m_gtRelatedFeatures.RemoveAt(i - 1);
                    }
                }
                m_gtRelationshipService = null;
                return m_gtRelatedFeatures;
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// VectorByAngle
        /// </summary>
        /// <param name="angle"></param>        
        /// <returns></returns>
        protected IGTVector VectorByAngle(double angle)
        {
            IGTVector vector = GTClassFactory.Create<IGTVector>();
            vector.I = Math.Cos(angle);
            vector.J = Math.Sin(angle);
            vector.K = 0;
            return vector;
        }

        /// <summary>
        /// Angle Between Two Points
        /// </summary>
        /// <param name="gtStartPoint"></param>
        /// <param name="gtEndPoint"></param> 
        /// <returns></returns>
        protected double AngleBetweenTwoPoints(IGTPoint gtStartPoint, IGTPoint gtEndPoint)
        {
            double deltaX = gtEndPoint.X - gtStartPoint.X;
            double deltaY = gtEndPoint.Y - gtStartPoint.Y;
            //TODO 3D oOffset.Z = oEndPoint.Z - oStartPoint.Z;
            return Math.Atan2(deltaY, deltaX);
        }

        /// <summary>
        /// GetAngleBetweenTwoPoints in Degree's
        /// </summary>
        /// <param name="gtStartPoint"></param>
        /// <param name="gtEndPoint"></param> 
        /// <returns></returns>
        protected double GetAngleBetweenTwoPoints(IGTPoint gtStartPoint, IGTPoint gtEndPoint)
        {
            try
            {
                double deltaX = gtEndPoint.X - gtStartPoint.X;
                double deltaY = gtEndPoint.Y - gtStartPoint.Y;
                Double Radians = Math.Atan2(deltaY, deltaX);
                //Degrees=radians*180/Math.pi
                //radians=Degrees*math.Pi/180
                Double Angle = Radians * 180 / Math.PI;
                return Angle;
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Set Attribute value
        /// </summary>
        /// <param name="m_gtComponents"></param>
        /// <param name="m_gtKeyObject"></param>
        /// <param name="m_gtRelativeComponent"></param>
        /// <returns></returns>
        protected void SetAttributeValues(IGTComponents m_gtComponents, IGTKeyObject m_gtKeyObject, IGTGraphicComponent m_gtRelativeComponent)
        {
            try
            {
                foreach (IGTComponent m_gtComponent in m_gtComponents)
                {
                    switch (m_gtComponent.CNO)
                    {
                        case VirtualPointCNO:
                            if (m_gtComponent.Recordset.RecordCount == 0)
                            {
                                m_gtComponent.Recordset.AddNew();
                                m_gtComponent.Recordset.Fields["G3E_FID"].Value = m_gtKeyObject.FID;
                                m_gtComponent.Recordset.Fields["G3E_FNO"].Value = m_gtKeyObject.FNO;
                                m_gtComponent.Recordset.Fields["G3E_CNO"].Value = m_gtComponent.CNO;
                                m_gtComponent.Recordset.Fields["G3E_CID"].Value = 1;
                            }
                            else
                            {
                                m_gtComponent.Recordset.MoveFirst();
                            }
                            break;
                        case ConnectivityCNO:
                            if (m_gtComponent.Recordset.RecordCount == 0)
                            {
                                m_gtComponent.Recordset.AddNew();
                                m_gtComponent.Recordset.Fields["G3E_FID"].Value = m_gtKeyObject.FID;
                                m_gtComponent.Recordset.Fields["G3E_FNO"].Value = m_gtKeyObject.FNO;
                                m_gtComponent.Recordset.Fields["G3E_CNO"].Value = m_gtComponent.CNO;
                                m_gtComponent.Recordset.Fields["G3E_CID"].Value = 1;
                                m_gtComponent.Recordset.Fields["STATUS_NORMAL_C"].Value = statusNormal;
                            }
                            else
                            {
                                m_gtComponent.Recordset.MoveFirst();
                                m_gtComponent.Recordset.Fields["STATUS_NORMAL_C"].Value = statusNormal;
                            }
                            break;
                        case VirtualAttributeCNO:
                            if (m_gtComponent.Recordset.RecordCount == 0)
                            {
                                m_gtComponent.Recordset.AddNew();
                                m_gtComponent.Recordset.Fields["G3E_FID"].Value = m_gtKeyObject.FID;
                                m_gtComponent.Recordset.Fields["G3E_FNO"].Value = m_gtKeyObject.FNO;
                                m_gtComponent.Recordset.Fields["G3E_CNO"].Value = m_gtComponent.CNO;
                                m_gtComponent.Recordset.Fields["G3E_CID"].Value = 1;
                                m_gtComponent.Recordset.Fields["ASSOCIATED_FID"].Value = m_gtRelativeComponent.FID;
                                m_gtComponent.Recordset.Fields["PERMANENT_YN"].Value = "Y";
                            }
                            else
                            {
                                m_gtComponent.Recordset.MoveFirst();
                                m_gtComponent.Recordset.Fields["ASSOCIATED_FID"].Value = m_gtRelativeComponent.FID;
                                m_gtComponent.Recordset.Fields["PERMANENT_YN"].Value = "Y";
                            }
                            break;
                        default:
                            break;
                    }
                }
            }
            catch
            {
                throw;
            }
        }
        /// <summary>
        /// Get Segment And Angle on which point is placed
        /// </summary>
        /// <param name="m_gtPolylineGeometry"></param>
        /// <param name="gtPoint"></param> 
        /// <returns></returns>
        protected IGTGeometry GetSegmentAndAngle(IGTGeometry m_gtPolylineGeometry, IGTPoint gtPoint)
        {
            bool flag = false;
            IGTGeometry m_gtPolylineGeometrySegment = GTClassFactory.Create<IGTGeometry>();
            IGTSegment m_gtSegment = GTClassFactory.Create<IGTSegment>();
            IGTPoint m_gtsnapPoint = GetSnapPoint(m_gtPolylineGeometry, gtPoint);
            try
            {
                if (m_gtPolylineGeometry.Type == "CompositePolylineGeometry")
                {
                    //get the segment
                    for (int i = 0; i < m_gtPolylineGeometry.KeypointCount - 1; i++)
                    {
                        m_gtSegment.Begin = m_gtPolylineGeometry.GetKeypointPosition(i);
                        m_gtSegment.End = m_gtPolylineGeometry.GetKeypointPosition(i + 1);
                        if (m_gtsnapPoint.IsOn(m_gtSegment))
                        {
                            flag = true;
                            break;
                        }
                    }
                    if (flag)
                    {
                        m_gtPolylineGeometrySegment = m_gtPolylineGeometry.ExtractGeometry(m_gtSegment.Point1, m_gtSegment.Point2, false);
                    }
                }
                return m_gtPolylineGeometrySegment;
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
                snap.SnapTolerance = 1.7;
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
        protected object GetPropertyValue(string propertyName)
        {
            object propertyValue = null;

            try
            {
                propertyValue = m_gtApplication.Properties[propertyName];
            }
            catch
            {
              //ignore error
            }

            return propertyValue;
        }

        #endregion
    }
}
