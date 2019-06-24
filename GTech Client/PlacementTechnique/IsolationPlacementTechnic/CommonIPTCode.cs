//----------------------------------------------------------------------------+
//  Class: CommonIPTCode
//  Description: The class acts as a common code layer for all the Interactive placement techniques
//----------------------------------------------------------------------------+
//   $Author:: Uma Avote                              $
//   $Date:: 10/04/18                                 $
//   $Revision:: 1                                    $
//----------------------------------------------------------------------------+
//    $History:: CommonIPTCode.cs                     $
// 
// *****************  Version 1  *****************
// User: Uma Avote     Date: 10/04/18   Time:   Desc : Created
//----------------------------------------------------------------------------+
using System;
using Intergraph.GTechnology.Interfaces;
using Intergraph.GTechnology.API;
using System.Windows.Forms;

namespace GTechnology.Oncor.CustomAPI
{
    public class CommonIPTCode : IGTPlacementTechnique
    {
        #region Public Members       
        public IGTApplication m_gtApplication = GTClassFactory.Create<IGTApplication>();
        public IGTPlacementTechniqueHelper m_PTHelper = null;
        public IGTGraphicComponents m_GComps = null;
        public IGTGraphicComponent m_GComp = null;
        public double ang;
        #endregion

        #region private Members
        bool bSilent = false;
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
                if (m_PTHelper != null)
                {
                    m_PTHelper.MouseClick(UserPoint, Button, ShiftState);
                }
            }
        }
        public virtual void MouseDblClick(IGTMapWindow MapWindow, Intergraph.GTechnology.API.IGTPoint UserPoint, int ShiftState, Intergraph.GTechnology.API.IGTDDCKeyObjects LocatedObjects)
        {
            try
            {
                if (!bSilent)
                {
                    if (m_PTHelper != null)
                    {
                        m_PTHelper.MouseDblClick(UserPoint, ShiftState);
                    }
                }
            }
            catch
            {
                throw;
            }
        }
        public virtual void MouseMove(IGTMapWindow MapWindow, Intergraph.GTechnology.API.IGTPoint UserPoint, int ShiftState, Intergraph.GTechnology.API.IGTDDCKeyObjects LocatedObjects, Intergraph.GTechnology.API.IGTPlacementTechniqueEventMode EventMode)
        {
            if (!bSilent)
            {
                if (m_PTHelper != null)
                {
                    m_PTHelper.MouseMove(UserPoint, ShiftState);
                }
                if (m_gtApplication != null)
                {
                    m_gtApplication.SetStatusBarText(GTStatusPanelConstants.gtaspcMessage, "Please click on related Primary Conductor feature.");
                }
            }
        }
        public virtual void StartPlacement(Intergraph.GTechnology.API.IGTPlacementTechniqueHelper PTHelper, Intergraph.GTechnology.API.IGTKeyObject KeyObject, Intergraph.GTechnology.API.IGTKeyObjects KeyObjectCollection)
        {
            try
            {
                m_PTHelper = PTHelper;
                IGTKeyObject m_KeyObject = KeyObject;
                IGTKeyObjects m_KeyObjectCollection = KeyObjectCollection;

                if (bSilent)
                {
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
                m_gtApplication.SetStatusBarText(GTStatusPanelConstants.gtaspcMessage, "");
                m_gtApplication = null;
            }
            if (m_PTHelper != null)
            {
                m_GComps = null;
                m_GComp = null;
                m_PTHelper.StatusBarPromptsEnabled = false;
                m_PTHelper.ConstructionAidsEnabled = GTConstructionAidsEnabledConstants.gtptConstructionAidsNone;
                m_PTHelper.ConstructionAidDynamicsEnabled = false;
                m_PTHelper = null;
            }
        }
        #endregion

        #region protected Members
        /// <summary>
        /// Get Vector By Angle
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
        /// Get Angle Between Two Points
        /// </summary>
        /// <param name="gtStartPoint"></param>
        /// <param name="gtEndPoint"></param>
        /// <returns></returns>
        protected double AngleBetweenTwoPoints(IGTPoint gtStartPoint, IGTPoint gtEndPoint)
        {
            double deltaX = gtEndPoint.X - gtStartPoint.X;
            double deltaY = gtEndPoint.Y - gtStartPoint.Y;
            return Math.Atan2(deltaY, deltaX);
        }

        /// <summary>
        /// Get Point And Angle
        /// </summary>
        /// <param name="m_gtPolylineGeom"></param>
        /// <param name="m_gtPoint">Related Virtual point</param>
        /// <returns></returns>
        protected IGTPoint GetPointAndAngle(IGTGeometry m_gtPolylineGeom, IGTPoint m_gtPoint)
        {
            bool flag = false;
            IGTPoint m_gtProjectedPoint = null;
            //Get snappoint if the point feature is not snapped to the linear
            IGTPoint m_gtsnapPoint = GetSnapPoint(m_gtPolylineGeom, m_gtPoint);
            try
            {
                //Check if the geometry is linear
                if ((m_gtPolylineGeom.Type == "CompositePolylineGeometry") || (m_gtPolylineGeom.Type == "PolylineGeometry"))
                {

                    IGTSegment m_segment = GTClassFactory.Create<IGTSegment>();
                    //get the segment
                    for (int i = 0; i < m_gtPolylineGeom.KeypointCount - 1; i++)
                    {
                        m_segment.Begin = m_gtPolylineGeom.GetKeypointPosition(i);
                        m_segment.End = m_gtPolylineGeom.GetKeypointPosition(i + 1);
                        if (m_gtsnapPoint.IsOn(m_segment))
                        {
                            flag = true;
                            ang = AngleBetweenTwoPoints(m_segment.Begin, m_segment.End);
                            break;
                        }
                        else
                        {
                            // m_gtProjectedPoint = m_gtsnapPoint;
                        }
                    }
                    if (flag)
                    {
                        m_gtProjectedPoint = m_segment.ProjectPointAtAngle(m_segment.Begin, m_gtsnapPoint, ang);
                    }
                }
                return m_gtProjectedPoint;
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
        // Add property to the Properties collection
        // This is a place to store data that is needed as long as the session is active.
        protected  void AddProperty(string propertyName, object propertyValue)
        {
            try
            {
                m_gtApplication.Properties.Remove(propertyName);
            }
            catch
            {
                // ignore error
            }
            try
            {
                m_gtApplication.Properties.Add(propertyName, propertyValue);
            }
            catch
            {
                //ignore error
            }
        }

        #endregion
    }
}
