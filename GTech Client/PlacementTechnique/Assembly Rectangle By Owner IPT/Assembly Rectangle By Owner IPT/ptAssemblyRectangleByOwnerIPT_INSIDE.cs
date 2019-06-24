// ===================================================
//  Copyright 2017 Intergraph Corp.
//  File Name: fiAttachmentHeightValidation.cs
// 
//  Description:   This class creates placements for primary switch gear based on owner ( Pad / Vault)
//
//  Remarks:
// 
//  Modification History
//  Date                Author                      Comments
//  --------            --------------              --------
//  26/07/2017          Hari                        Created 

// ======================================================

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Intergraph.CoordSystems;
using Intergraph.GTechnology.API;
using Intergraph.GTechnology.Interfaces;
using Intergraph.CoordSystems.Interop;
using ADODB;

namespace GTechnology.Oncor.CustomAPI
{
    public class ptAssemblyRectangleByOwnerIPT : IGTPlacementTechnique
    {
        #region Private Members
        private IGTKeyObject m_KeyObject = null;
        private IGTKeyObjects m_KeyObjectCollection = null;
        private IGTGraphicComponents m_GraphicComponents = null;
        private IGTGraphicComponent m_ActiveGraphicComponent = null;
        private IGTMapWindow m_ActiveMapWindow = null;
        private IGTPlacementTechniqueHelper m_PTHelper = null;
        private GTArguments m_Arguments = null;
        private IGTApplication m_GTApplication = null;
        private frmAssembly frmAssmbly;
        private IGTPoint m_CentriodPoint = null;
        #endregion

        public const string Caption = "G/Technology";

        #region IGTPlacementTechnique Members

        public void AbortPlacement()
        {

            if (m_PTHelper != null)
            {
                m_PTHelper.AbortPlacement();
            }
        }

        public IGTGraphicComponent GraphicComponent
        {
            get
            {
                return m_ActiveGraphicComponent;
            }
            set
            {
                m_ActiveGraphicComponent = value;
            }
        }

        public IGTGraphicComponents GraphicComponents
        {
            get
            {
                return m_GraphicComponents;
            }
            set
            {
                m_GraphicComponents = value;
            }
        }

        public void KeyUp(Intergraph.GTechnology.API.IGTMapWindow MapWindow, int KeyCode, int ShiftState, Intergraph.GTechnology.API.IGTPlacementTechniqueEventMode EventMode)
        {
        }

        /// <summary>
        /// Fires when left mouse button is clicked. If the click is on Pad/Vault only then Primary Switch gear will be placed.
        /// </summary>
        /// <param name="MapWindow"></param>
        /// <param name="UserPoint"></param>
        /// <param name="Button"></param>
        /// <param name="ShiftState"></param>
        /// <param name="LocatedObjects"></param>
        /// <param name="EventMode"></param>
        public void MouseClick(Intergraph.GTechnology.API.IGTMapWindow MapWindow, Intergraph.GTechnology.API.IGTPoint UserPoint, int Button, int ShiftState, Intergraph.GTechnology.API.IGTDDCKeyObjects LocatedObjects, Intergraph.GTechnology.API.IGTPlacementTechniqueEventMode EventMode)
        {
            try
            {
                string ownerFeatureName = m_ActiveMapWindow.DetailID == 0 ? "Pad" : "Vault";
                if (0 == LocatedObjects.Count)
                {
                    // If there are no linear objects in the collection, then there is nothing to use to create
                    // the new geometry.  Beep to notify the user and display a message in the status bar.
                    System.Console.Beep();
                    m_GTApplication.SetStatusBarText(GTStatusPanelConstants.gtaspcMessage, "There is no existing " + ownerFeatureName + " symbol feature to Own the Primary switch gear which is to be placed adjacent to it.");
                }
                else
                {
                    if (1 == LocatedObjects.Count && ((LocatedObjects[0].FNO == 108 && LocatedObjects[0].ComponentViewName == "V_PAD_S" && m_ActiveMapWindow.DetailID == 0) || (LocatedObjects[0].FNO == 117 && LocatedObjects[0].ComponentViewName == "V_VAULT_DP" && m_GTApplication.ActiveMapWindow.DetailID != 0)))
                    {
                        // There is exactly 1 linear object in the collection, so use it to creat the new geometry.
                        this.PlacePrimarySwitchGear(LocatedObjects[0]);
                        MapWindow.HighlightedObjects.Clear();
                    }
                    else
                    {
                        m_GTApplication.SetStatusBarText(GTStatusPanelConstants.gtaspcMessage, "Please identify only " + ownerFeatureName + " symbol feature to Own the Primary switch gear which is to be placed adjacent to it.");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, Caption);
            }

        }

        /// <summary>
        /// Fires when left mouse button is double clicked
        /// </summary>
        /// <param name="MapWindow"></param>
        /// <param name="UserPoint"></param>
        /// <param name="ShiftState"></param>
        /// <param name="LocatedObjects"></param>
        public void MouseDblClick(IGTMapWindow MapWindow, IGTPoint UserPoint, int ShiftState, IGTDDCKeyObjects LocatedObjects)
        {
            m_PTHelper.MouseDblClick(UserPoint, ShiftState);
        }

        public void MouseMove(Intergraph.GTechnology.API.IGTMapWindow MapWindow, Intergraph.GTechnology.API.IGTPoint UserPoint, int ShiftState, Intergraph.GTechnology.API.IGTDDCKeyObjects LocatedObjects, Intergraph.GTechnology.API.IGTPlacementTechniqueEventMode EventMode)
        {

        }

        /// <summary>
        ///  Method is called when pad/vault  mounted switch gear placement is started
        /// </summary>
        /// <param name="PTHelper"></param>
        /// <param name="KeyObject"></param>
        /// <param name="KeyObjectCollection"></param>
        public void StartPlacement(IGTPlacementTechniqueHelper PTHelper, IGTKeyObject KeyObject, IGTKeyObjects KeyObjectCollection)
        {
            try
            {
                m_PTHelper = PTHelper;
                m_KeyObject = KeyObject;
                m_KeyObjectCollection = KeyObjectCollection;


                m_PTHelper.ConstructionAidsEnabled = Intergraph.GTechnology.API.GTConstructionAidsEnabledConstants.gtptConstructionAidsNone;
                m_PTHelper.ConstructionAidDynamicsEnabled = false;
                m_PTHelper.StatusBarPromptsEnabled = false;

                m_PTHelper.StartPlacement(m_KeyObject, m_KeyObjectCollection);

                m_GTApplication = GTClassFactory.Create<IGTApplication>().Application;
                m_Arguments = m_ActiveGraphicComponent.Arguments;
                m_ActiveMapWindow = m_GTApplication.ActiveMapWindow;
                string ownerFeatureName = m_ActiveMapWindow.DetailID == 0 ? "Pad" : "Vault";
                m_GTApplication.SetStatusBarText(GTStatusPanelConstants.gtaspcMessage, "Click to select an existing " + ownerFeatureName + " feature to Own the Primary switch gear which is to be placed adjacent to it.");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, Caption);

            }
        }

        #endregion

        private void PlacePrimarySwitchGear(IGTDDCKeyObject selectedObject)
        {
            try
            {
                //Reading the arguments
                IGTPolygonGeometry oCPLGeom = null;
                double XOffset = short.Parse(m_ActiveGraphicComponent.Arguments.GetArgument(0).ToString());
                double YOffset = short.Parse(m_ActiveGraphicComponent.Arguments.GetArgument(1).ToString());
                if (selectedObject.Geometry.Type == GTGeometryTypeConstants.gtgtOrientedPointGeometry)
                {
                    IGTOrientedPointGeometry iPointGeom = (IGTOrientedPointGeometry)selectedObject.Geometry;
                    //oCPLGeom = GetPolyGeom(iPointGeom.Origin, XOffset, YOffset);
                    oCPLGeom = GetPolyGeom(iPointGeom.Origin, 0, 0);
                    RotateGeom(iPointGeom, ref oCPLGeom);
                }
                else if (selectedObject.Geometry.Type == GTGeometryTypeConstants.gtgtPolygonGeometry)
                {
                    IGTPolygonGeometry iPolygonGeom = (IGTPolygonGeometry)selectedObject.Geometry;
                    List<Point2D> polygonPoints = new List<Point2D>();
                    foreach (IGTPoint point in iPolygonGeom.Points)
                    {
                        polygonPoints.Add(new Point2D { x = point.X, y = point.Y });
                    }

                    //Calulate approx minimum length for sides of polygon
                    double sideLength1 = 0.0;
                    double sideLength2 = 0.0;
                    double sideLength3 = 0.0;
                    double length = 0.0;
                    if (polygonPoints.Count > 3)
                    {
                        sideLength1 = Math.Abs(polygonPoints[0].y - polygonPoints[0].x);
                        sideLength2 = Math.Abs(polygonPoints[1].y - polygonPoints[1].x);
                        sideLength3 = Math.Abs(polygonPoints[2].y - polygonPoints[2].x);
                        length = Math.Min(sideLength1, sideLength2);
                        length = Math.Min(length, sideLength3);
                    }



                    int vertexCount = iPolygonGeom.Points.Count - 1;
                    Point2D centroid = Compute2DPolygonCentroid(polygonPoints, vertexCount);

                    m_CentriodPoint = GTClassFactory.Create<IGTPoint>();
                    m_CentriodPoint.X = centroid.x;
                    m_CentriodPoint.Y = centroid.y;
                    m_CentriodPoint.Z = 0.0;

                    //oCPLGeom = GetPolyGeom(m_CentriodPoint, XOffset, YOffset);
                    oCPLGeom = GetPolyGeom(m_CentriodPoint, 0, 0, length);
                    RotateGeom(iPolygonGeom, ref oCPLGeom);
                }
                if (oCPLGeom != null)
                {
                    m_PTHelper.SetGeometry(oCPLGeom);
                    m_PTHelper.EndPlacement();
                    this.AssemblyCopyToModel(oCPLGeom);
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        private IGTPolygonGeometry GetPolyGeom(IGTPoint iPt, double xOffset, double yOffset, double length = 0)
        {
            try
            {
                if (iPt == null)
                {
                    return null;
                }
                IGTPolylineGeometry oPolyLineGeom = GTClassFactory.Create<IGTPolylineGeometry>();
                IGTPolygonGeometry oGeom = GTClassFactory.Create<IGTPolygonGeometry>();
                IGTPoint oTmpPt = GTClassFactory.Create<IGTPoint>();
                IGTPoint oMidPT = GTClassFactory.Create<IGTPoint>();
                IGTPoint oStartPt = GTClassFactory.Create<IGTPoint>(); ;

                oMidPT.X = iPt.X;
                oMidPT.Y = iPt.Y;

                if (length > 0)
                {
                    //oStartPt.X = oMidPT.X;
                    //oStartPt.Y = oMidPT.Y - (length/2);
                    //oPolyLineGeom.Points.Add(oStartPt);

                    //oTmpPt.X = oStartPt.X + (length/2);
                    //oTmpPt.Y = oStartPt.Y;
                    //oPolyLineGeom.Points.Add(oTmpPt);

                    //oTmpPt.X = oTmpPt.X;
                    //oTmpPt.Y = oTmpPt.Y + length;
                    //oPolyLineGeom.Points.Add(oTmpPt);

                    //oTmpPt.X = oTmpPt.X - length;
                    //oTmpPt.Y = oTmpPt.Y;
                    //oPolyLineGeom.Points.Add(oTmpPt);

                    //oPolyLineGeom.Points.Add(oStartPt);

                    oStartPt.X = oMidPT.X;
                    oStartPt.Y = oMidPT.Y - 5;
                    oPolyLineGeom.Points.Add(oStartPt);

                    oTmpPt.X = oStartPt.X + 5;
                    oTmpPt.Y = oStartPt.Y;
                    oPolyLineGeom.Points.Add(oTmpPt);

                    oTmpPt.X = oTmpPt.X;
                    oTmpPt.Y = oTmpPt.Y + 10;
                    oPolyLineGeom.Points.Add(oTmpPt);

                    oTmpPt.X = oTmpPt.X - 10;
                    oTmpPt.Y = oTmpPt.Y;
                    oPolyLineGeom.Points.Add(oTmpPt);

                    oTmpPt.X = oTmpPt.X;
                    oTmpPt.Y = oTmpPt.Y - 10;
                    oPolyLineGeom.Points.Add(oTmpPt);

                    oPolyLineGeom.Points.Add(oStartPt);
                }

                else
                {
                    oStartPt.X = oMidPT.X + (-14);
                    oStartPt.Y = oMidPT.Y;
                    oPolyLineGeom.Points.Add(oStartPt);

                    oTmpPt.X = oMidPT.X + 14;
                    oTmpPt.Y = oMidPT.Y;
                    oPolyLineGeom.Points.Add(oTmpPt);

                    oTmpPt.X = oTmpPt.X;
                    oTmpPt.Y = oTmpPt.Y + 28;
                    oPolyLineGeom.Points.Add(oTmpPt);

                    oTmpPt.X = oTmpPt.X - 28;
                    oTmpPt.Y = oTmpPt.Y;
                    oPolyLineGeom.Points.Add(oTmpPt);

                    oPolyLineGeom.Points.Add(oStartPt);
                }
                foreach (IGTPoint pt in oPolyLineGeom.Points)
                {
                    oGeom.Points.Add(pt);
                }

                return oGeom;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        /// <summary>
        /// Rotates input geometry relative to the reference geom
        /// </summary>
        /// <param name="ogeom"></param>
        /// <returns></returns>
        private void RotateGeom(IGTGeometry refGeom, ref IGTPolygonGeometry inpGeom)
        {
            double dAngle = 0;
            IGTPolygonGeometry oPolyGeom = null;

            try
            {
                if (refGeom == null || inpGeom == null) return;
                if (refGeom.GetType().ToString() == "Intergraph.GTechnology.Private.GTOrientedPointGeometry")
                {
                    dAngle = AngleBetweenTwoPoints(((IGTOrientedPointGeometry)refGeom).Orientation);
                }
                else if (refGeom.GetType().ToString() == "Intergraph.GTechnology.Private.GTPolygonGeometry")
                {
                    oPolyGeom = (IGTPolygonGeometry)refGeom.Stroke();
                    dAngle = AngleBetweenTwoPoints(oPolyGeom.Points[0], oPolyGeom.Points[1]);
                }

                if (dAngle < -90.0 || dAngle > 90.0)
                {
                    dAngle = dAngle + 180.0;
                }
                IGTVector transVec = GTClassFactory.Create<IGTVector>();

                if (inpGeom.GetType().ToString() == "Intergraph.GTechnology.Private.GTPolygonGeometry")
                {
                    // move and rotate the geometry to the correct position 
                    if (m_ActiveMapWindow.DetailID == 0)
                    {
                        transVec.I = refGeom.FirstPoint.X * -1;
                        transVec.J = refGeom.FirstPoint.Y * -1;
                        transVec.K = refGeom.FirstPoint.Z * -1;
                    }
                    else
                    {
                        if (m_CentriodPoint != null)
                        {
                            transVec.I = m_CentriodPoint.X * -1;
                            transVec.J = m_CentriodPoint.Y * -1;
                            transVec.K = m_CentriodPoint.Z * -1;
                        }
                    }
                }

                IGTMatrix tmpTMatrix = GTClassFactory.Create<IGTMatrix>();
                tmpTMatrix = TranslationTransform(transVec);
                inpGeom = (IGTPolygonGeometry)inpGeom.Multiply(inpGeom, tmpTMatrix);

                // rotate the geometry to the angle of the 
                tmpTMatrix = GTClassFactory.Create<IGTMatrix>();
                tmpTMatrix = RotateZTransform(dAngle, 'D');
                inpGeom = (IGTPolygonGeometry)inpGeom.Multiply(inpGeom, tmpTMatrix);

                transVec = transVec.NegateVector(transVec);
                tmpTMatrix = GTClassFactory.Create<IGTMatrix>();
                tmpTMatrix = TranslationTransform(transVec);
                inpGeom = (IGTPolygonGeometry)inpGeom.Multiply(inpGeom, tmpTMatrix);
            }

            catch (Exception)
            {
                throw;
            }

            finally
            {
            }
        }
        private double AngleBetweenTwoPoints(IGTVector iVector)
        {
            try
            {
                double dAngleBetweenTwoPoints;
                dAngleBetweenTwoPoints = Math.Atan2(iVector.J, iVector.I) * (180 / Math.PI);
                return dAngleBetweenTwoPoints;
            }
            catch (Exception)
            {
                throw;
            }

        }
        private double AngleBetweenTwoPoints(IGTPoint pntA, IGTPoint pntB)
        {
            try
            {
                double DX = 0;
                double DY = 0;
                double dAngleBetweenTwoPoints;

                DX = pntA.X - pntB.X;
                DY = pntA.Y - pntB.Y;

                dAngleBetweenTwoPoints = Math.Atan2(DY, DX) * (180 / Math.PI);
                dAngleBetweenTwoPoints = dAngleBetweenTwoPoints - 90;
                return dAngleBetweenTwoPoints;
            }
            catch (Exception)
            {
                throw;
            }

        }
        internal class Point2D
        {
            internal double x;
            internal double y;
        }
        private Point2D Compute2DPolygonCentroid(List<Point2D> vertices, int vertexCount)
        {
            Point2D centroid = new Point2D();
            double signedArea = 0.0;
            double x0 = 0.0; // Current vertex X
            double y0 = 0.0; // Current vertex Y
            double x1 = 0.0; // Next vertex X
            double y1 = 0.0; // Next vertex Y
            double a = 0.0;  // Partial signed area

            // For all vertices except last
            int i = 0;
            for (i = 0; i < vertexCount - 1; ++i)
            {
                x0 = vertices[i].x;
                y0 = vertices[i].y;
                x1 = vertices[i + 1].x;
                y1 = vertices[i + 1].y;
                a = x0 * y1 - x1 * y0;
                signedArea += a;
                centroid.x += (x0 + x1) * a;
                centroid.y += (y0 + y1) * a;
            }

            // Do last vertex separately to avoid performing an expensive
            // modulus operation in each iteration.
            x0 = vertices[i].x;
            y0 = vertices[i].y;
            x1 = vertices[0].x;
            y1 = vertices[0].y;
            a = x0 * y1 - x1 * y0;
            signedArea += a;
            centroid.x += (x0 + x1) * a;
            centroid.y += (y0 + y1) * a;

            signedArea *= 0.5;
            centroid.x /= (6.0 * signedArea);
            centroid.y /= (6.0 * signedArea);

            return centroid;
        }


        private IGTMatrix TranslationTransform(IGTVector Voffset)
        {
            IGTMatrix m = null;
            try
            {
                m = GTClassFactory.Create<IGTMatrix>();

                m.M_Matrix[0, 0] = 1.0; m.M_Matrix[0, 1] = 0.0; m.M_Matrix[0, 2] = 0.0; m.M_Matrix[0, 3] = 0.0;
                m.M_Matrix[1, 0] = 0.0; m.M_Matrix[1, 1] = 1.0; m.M_Matrix[1, 2] = 0.0; m.M_Matrix[1, 3] = 0.0;
                m.M_Matrix[2, 0] = 0.0; m.M_Matrix[2, 1] = 0.0; m.M_Matrix[2, 2] = 1.0; m.M_Matrix[2, 3] = 0.0;
                m.M_Matrix[3, 0] = Voffset.I; m.M_Matrix[3, 1] = Voffset.J; m.M_Matrix[3, 2] = Voffset.K; m.M_Matrix[3, 3] = 1.0;
                return m;
            }
            catch (Exception)
            {
                throw;
            }
        }
        private IGTMatrix RotateZTransform(double theta, char RorD)
        {
            if (RorD == 'D')
            {
                theta = (theta * Math.PI) / 180;
            }

            double cos = Math.Cos(theta);
            double sin = Math.Sin(theta);

            IGTMatrix m = null;
            try
            {
                m = GTClassFactory.Create<IGTMatrix>();
                m.M_Matrix[0, 0] = cos; m.M_Matrix[0, 1] = sin; m.M_Matrix[0, 2] = 0.0; m.M_Matrix[0, 3] = 0.0;
                m.M_Matrix[1, 0] = -sin; m.M_Matrix[1, 1] = cos; m.M_Matrix[1, 2] = 0.0; m.M_Matrix[1, 3] = 0.0;
                m.M_Matrix[2, 0] = 0.0; m.M_Matrix[2, 1] = 0.0; m.M_Matrix[2, 2] = 1.0; m.M_Matrix[2, 3] = 0.0;
                m.M_Matrix[3, 0] = 0.0; m.M_Matrix[3, 1] = 0.0; m.M_Matrix[3, 2] = 0.0; m.M_Matrix[3, 3] = 1.0;
                return m;
            }
            catch (Exception e)
            {
                throw;
            }
        }

        private void AssemblyCopyToModel(IGTPolygonGeometry oGeometry)
        {
            int ownerID = 0;
            int lRecords;
            double dGeoOrientation = 0.0;
            try
            {
                frmAssmbly = new frmAssembly();
                frmAssmbly.Application = m_GTApplication;
                frmAssmbly.FeatureFNO = m_ActiveGraphicComponent.FNO;
                frmAssmbly.DetailIdentifier = m_ActiveMapWindow.DetailID;
                frmAssmbly.ShowDialog();
                if (frmAssmbly.FeatureIdentifier != 0)
                {
                    ownerID = GetOwnerIdentifier();
                    GetAssemblyPointsAndOrientation(oGeometry, ref dGeoOrientation);
                    ADODB.Recordset oRS = m_GTApplication.DataContext.Execute("{call OC_EXECUTE_DEEPCOPY(" + m_ActiveGraphicComponent.FNO.ToString() + " ," +
                                                                     frmAssmbly.FeatureIdentifier.ToString() + ", " +
                                                                     m_ActiveGraphicComponent.FNO.ToString() + ", " +
                                                                     m_ActiveGraphicComponent.CNO.ToString() + ", " +
                                                                     m_ActiveGraphicComponent.FID.ToString() + ", " +
                                                                     ownerID.ToString() + ", " + dGeoOrientation + ", " + m_ActiveMapWindow.DetailID + ")}", out lRecords, 1, new object[1]);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        private int GetOwnerIdentifier()
        {
            int returnValue = 0;
            Recordset oCompRS;
            try
            {
                oCompRS = m_KeyObject.Components["COMMON_N"].Recordset;

                if (oCompRS.RecordCount > 0)
                {
                    oCompRS.MoveFirst();
                    returnValue = System.Convert.ToInt32(oCompRS.Fields["G3E_ID"].Value);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
            finally
            {
                oCompRS = null;
            }
            return returnValue;
        }

        private void GetAssemblyPointsAndOrientation(IGTPolygonGeometry oGeometry, ref double dOrientation)
        {
            double pi = 0.0;
            double dx = 0.0;
            double dy = 0.0;
            string sCoordinates1 = string.Empty;
            pi = 4 * System.Math.Atan(1);
            dOrientation = 0;
            try
            {
                IGTPoint oPoint0 = oGeometry.Points[0];
                IGTPoint oPoint1 = oGeometry.Points[1];

                IGTPolygonGeometry oTemp = (IGTPolygonGeometry)(oGeometry);
                for (int i = 0; i < oTemp.Points.Count; i++)
                {
                    sCoordinates1 = string.Join(",", new string[] { sCoordinates1, oTemp.Points[i].X.ToString(), oTemp.Points[i].Y.ToString(),
                                                oTemp.Points[i].Z.ToString() });
                }

                sCoordinates1 = sCoordinates1.Remove(0, 1);
                int rAffected = 0;
                // Execute .NET stored procedure
                ADODB.Recordset oRS = m_GTApplication.DataContext.Execute("{call OC_INSERT_TEMP_PLR_COORD('" + sCoordinates1 + "')}",
                                      out rAffected, 1, new object[1]);


                // Calculate orientation
                dx = oPoint1.X - oPoint0.X;
                dy = oPoint1.Y - oPoint0.Y;
                if (dx == 0)
                {
                    if (dy >= 0)
                    {
                        dOrientation = pi / 2;
                    }
                    else
                    {
                        dOrientation = -(pi / 2);
                    }
                }
                else
                {
                    dOrientation = System.Math.Atan(dy / dx);
                    if (dx < 0)
                    {
                        dOrientation = dOrientation + pi;
                    }
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}
