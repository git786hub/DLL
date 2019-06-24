// ===================================================
//  Copyright 2017 Intergraph Corp.
//  File Name: ptAssemblyRectangleByOwnerIPT.cs
// 
//  Description:   This class creates placements for primary switch gear based on owner ( Pad / Vault)
//
//  Remarks:
// 
//  Modification History
//  Date                Author                      Comments
//  --------            --------------              --------
//  26-JUL-2017         Hari                        Created 
//  02-JAN-2019         Hari                        Enabled Move and Rotate options for the switch gear
//                                                  Adjusted code to read the width, height and distance from origin from Metadata arguments.
//
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
    public class ptAssemblyRectangleByOwnerIPT : IGTPlacementTechnique2
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
        private IGTPolygonGeometry m_priSwitchGearGeometry = null;
        private IGTGeometryEditService m_GeometryEditService = null;
        private bool m_MoveInProgress = false;
        private bool m_RotateInProgress = false;
        private int tempCount = 0;
        private bool m_rightClickenabled;
        private IGTPolygonGeometry tempGeometry = null;
        private List<IGTPoint> tempSwitchGearPoints = null;
        private bool leftClicked = false;
        double m_dGeoOrientation = 0.0;

        IGTDDCKeyObject selectedDdc = null;

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

        public bool RightMouseClickEnabled
        {
            get
            {
                return true;
            }
            set
            {
                m_rightClickenabled = true;
            }
        }

        public void KeyUp(Intergraph.GTechnology.API.IGTMapWindow MapWindow, int KeyCode, int ShiftState, Intergraph.GTechnology.API.IGTPlacementTechniqueEventMode EventMode)
        {
            if (m_GeometryEditService != null && KeyCode == (int)Keys.KeyCode)
                m_GeometryEditService.RemoveAllGeometries();
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
                IGTPolygonGeometry previous = GTClassFactory.Create<IGTPolygonGeometry>();

                m_GTApplication.SetStatusBarText(GTStatusPanelConstants.gtaspcMessage, "");
                if (Button == 2) // Right click 
                {
                    if (m_GeometryEditService != null)
                    {

                        IGTPolylineGeometry oPolyLineGeom = GTClassFactory.Create<IGTPolylineGeometry>();
                        //m_GeometryEditService.RemoveGeometry(m_priSwitchGearGeometry);
                        for (int i = 0; i < m_priSwitchGearGeometry.KeypointCount; i++)
                        {

                            IGTPoint point = GTClassFactory.Create<IGTPoint>();
                            point.X = tempSwitchGearPoints[i].X;
                            point.Y = tempSwitchGearPoints[i].Y;
                            oPolyLineGeom.Points.Add(point);
                        }
                        foreach (IGTPoint pt in oPolyLineGeom.Points)
                        {
                            previous.Points.Add(pt);
                        }
                    }
                    tempCount = tempCount + 1;
                    if (tempCount == 1) // User right clicked for first time to skip move
                    {
                        if (tempGeometry != null)
                        {
                            m_GeometryEditService.RemoveGeometry(tempGeometry);
                            tempGeometry = null;
                            m_priSwitchGearGeometry = previous;
                            m_GeometryEditService.AddGeometry(previous, 19401, true, true);
                            m_PTHelper.SetGeometry(previous);
                        }
                        m_MoveInProgress = false;
                        m_RotateInProgress = true;

                        m_GTApplication.SetStatusBarText(GTStatusPanelConstants.gtaspcMessage, "Click to place object.Right click to skip rotate.Double click to end placement.");
                    }
                    else if (tempCount == 2)// User right clicked for second time to skip rotate
                    {
                        m_MoveInProgress = false;
                        m_RotateInProgress = false;
                        if (tempGeometry != null)
                        {
                            m_GeometryEditService.RemoveGeometry(tempGeometry);
                            tempGeometry = null;
                            m_priSwitchGearGeometry = previous;
                            m_GeometryEditService.AddGeometry(previous, 19401, true, true);
                            m_PTHelper.SetGeometry(previous);
                        }

                        m_GTApplication.SetStatusBarText(GTStatusPanelConstants.gtaspcMessage, "Double click to end placement.");
                        m_GTApplication.ActiveMapWindow.MousePointer = GTMousePointerConstants.gtmwmpNWArrow;
                    }
                    return;
                }

                if (m_GeometryEditService != null)
                {
                    leftClicked = true;
                    if (m_MoveInProgress)
                    {
                        m_GTApplication.SetStatusBarText(GTStatusPanelConstants.gtaspcMessage, "Click to place object.Right click to skip move.Double click to end placement.");
                        m_GeometryEditService.BeginMove(UserPoint);
                        m_GeometryEditService.Move(UserPoint);
                        m_GeometryEditService.EndMove(m_priSwitchGearGeometry.FirstPoint);
                        tempSwitchGearPoints.Clear();
                        tempSwitchGearPoints.AddRange(m_priSwitchGearGeometry.Points);
                        previous = m_priSwitchGearGeometry;
                        if (m_GeometryEditService != null)
                        {
                            m_PTHelper.SetGeometry(m_priSwitchGearGeometry);
                        }
                        return;
                    }

                    if (m_RotateInProgress)
                    {
                        m_GTApplication.SetStatusBarText(GTStatusPanelConstants.gtaspcMessage, "Click to place object.Right click to skip rotate.Double click to end placement.");
                        m_GeometryEditService.BeginRotate(m_priSwitchGearGeometry.FirstPoint, m_priSwitchGearGeometry.LastPoint);
                        m_GeometryEditService.Rotate(UserPoint);
                        m_GeometryEditService.EndRotate(m_priSwitchGearGeometry.FirstPoint);
                        tempSwitchGearPoints.Clear();
                        tempSwitchGearPoints.AddRange(m_priSwitchGearGeometry.Points);
                        if (m_GeometryEditService != null)
                        {
                            m_PTHelper.SetGeometry(m_priSwitchGearGeometry);
                        }
                        return;
                    }
                }

                if (selectedDdc != null)
                {
                    return;
                }
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
                        selectedDdc = LocatedObjects[0];
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
                if (m_GeometryEditService != null)
                {
                    m_GeometryEditService.RemoveAllGeometries();
                    m_GeometryEditService = null;
                }
                m_PTHelper = null;
                MessageBox.Show("Error in Mouse click during the placement of Assembly equipment." + Environment.NewLine + ex.Message, Caption);

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
            try
            {
                if (m_PTHelper != null)
                {
                    m_RotateInProgress = false;
                    m_MoveInProgress = false;
                    IGTPolygonGeometry geometry = GTClassFactory.Create<IGTPolygonGeometry>();
                    if (m_GeometryEditService != null)
                    {
                        if (tempGeometry != null)
                            m_GeometryEditService.RemoveGeometry(tempGeometry);

                        IGTPolylineGeometry oPolyLineGeom = GTClassFactory.Create<IGTPolylineGeometry>();
                        for (int i = 0; i < m_priSwitchGearGeometry.KeypointCount; i++)
                        {
                            IGTPoint point = GTClassFactory.Create<IGTPoint>();
                            point.X = tempSwitchGearPoints[i].X;
                            point.Y = tempSwitchGearPoints[i].Y;
                            oPolyLineGeom.Points.Add(point);
                        }
                        foreach (IGTPoint pt in oPolyLineGeom.Points)
                        {
                            geometry.Points.Add(pt);
                        }
                        m_GeometryEditService.AddGeometry(geometry, 19401, true, true);
                        m_PTHelper.SetGeometry(geometry);
                    }

                    m_PTHelper.EndPlacement();
                    AssemblyCopyToModel(geometry);

                    CopyComponentInformationForSwitchGear();
                    SetWRForChildren();

                    if (m_GeometryEditService != null)
                    {
                        m_GeometryEditService.RemoveAllGeometries();
                        m_GeometryEditService = null;
                    }

                    int styleId = m_ActiveMapWindow.DetailID == 0 ? 19206 : 19205;
                    short cno = (short)(m_ActiveMapWindow.DetailID == 0 ? 1903 : 1952);
                    if (frmAssmbly.FeatureFNO == 153)
                    {
                        styleId = 153205;
                        cno = 15352;
                    }

                    IGTTextPointGeometry iGtTxtPointGeometry = GTClassFactory.Create<IGTTextPointGeometry>();
                    IGTPoint textPoint = GTClassFactory.Create<IGTPoint>();
                    textPoint.X = geometry.Points[0].X - 4;
                    textPoint.Y = geometry.Points[0].Y - 4;
                    textPoint.Z = 0;

                    iGtTxtPointGeometry.Origin = textPoint;

                    Recordset labelCmpRs = m_GTApplication.DataContext.OpenFeature(m_KeyObject.FNO, m_KeyObject.FID).Components.GetComponent(cno).Recordset;
                    if (labelCmpRs.RecordCount == 0)
                    {
                        labelCmpRs.AddNew("G3E_FID", m_KeyObject.FID);
                        labelCmpRs.Fields["G3E_FNO"].Value = m_KeyObject.FNO;
                        labelCmpRs.Fields["G3E_CNO"].Value = cno;
                    }

                    labelCmpRs.Update(Type.Missing, Type.Missing);

                    m_GTApplication.DataContext.OpenFeature(m_KeyObject.FNO, m_KeyObject.FID).Components.GetComponent(cno).Geometry = iGtTxtPointGeometry;
                    m_GTApplication.ActiveMapWindow.MousePointer = GTMousePointerConstants.gtmwmpNWArrow;
                    m_GTApplication.Application.RefreshWindows();
                    m_PTHelper = null;
                }
            }
            catch (Exception ex)
            {
                if (m_GeometryEditService != null)
                {
                    m_GeometryEditService.RemoveAllGeometries();
                    m_GeometryEditService = null;
                }
                m_PTHelper = null;
                MessageBox.Show("Error in Double click during the placement of Assembly equipment." + Environment.NewLine + ex.Message, Caption);
            }
        }

        private void PlacementService_Finished(object sender, GTFinishedEventArgs e)
        {

        }

        private void GTFeatureExplorerService_SaveClick(object sender, EventArgs e)
        {
            try
            {
                int styleId = m_ActiveMapWindow.DetailID == 0 ? 19206 : 19205;
                if (frmAssmbly.FeatureFNO == 153)
                {
                    styleId = 153205;
                }

                m_GeometryEditService = GTClassFactory.Create<IGTGeometryEditService>();
                m_GeometryEditService.TargetMapWindow = m_GTApplication.ActiveMapWindow;
                //m_GeometryEditService.AddGeometry(m_priSwitchGearGeometry, 19401, true, true);
                IGTOrientedPointGeometry orientedPointGeometry = GTClassFactory.Create<IGTOrientedPointGeometry>();
                orientedPointGeometry.FirstPoint.X = selectedDdc.Geometry.FirstPoint.X - 2;
                orientedPointGeometry.FirstPoint.Y = selectedDdc.Geometry.FirstPoint.Y - 2;

                IGTTextPointGeometry textPointGeometry = GTClassFactory.Create<IGTTextPointGeometry>();
                textPointGeometry.Origin = orientedPointGeometry.Origin;
                m_GeometryEditService.AddGeometry(textPointGeometry, styleId, true, true);
                m_PTHelper.SetGeometry(textPointGeometry);
                m_PTHelper.EndPlacement();
                m_PTHelper = null;
                m_GTApplication.ActiveMapWindow.MousePointer = GTMousePointerConstants.gtmwmpNWArrow;
                m_GTApplication.Application.RefreshWindows();

            }
            catch (Exception)
            {

                throw;
            }
        }

        public void MouseMove(Intergraph.GTechnology.API.IGTMapWindow MapWindow, Intergraph.GTechnology.API.IGTPoint UserPoint, int ShiftState, Intergraph.GTechnology.API.IGTDDCKeyObjects LocatedObjects, Intergraph.GTechnology.API.IGTPlacementTechniqueEventMode EventMode)
        {
            try
            {

                if (m_GeometryEditService != null && m_MoveInProgress)
                {
                    m_GeometryEditService.BeginMove(m_priSwitchGearGeometry.FirstPoint);
                    m_GeometryEditService.Move(UserPoint);
                    m_GeometryEditService.EndMove(UserPoint);
                    tempGeometry = m_priSwitchGearGeometry;
                }
                if (m_GeometryEditService != null && m_RotateInProgress)
                {
                    m_GeometryEditService.BeginRotate(m_priSwitchGearGeometry.FirstPoint, m_priSwitchGearGeometry.Points[2]);
                    //m_GeometryEditService.BeginRotate(m_CentriodPoint, m_priSwitchGearGeometry.FirstPoint);
                    m_GeometryEditService.Rotate(UserPoint);
                    m_GeometryEditService.EndRotate(UserPoint);
                    tempGeometry = m_priSwitchGearGeometry;
                }
            }
            catch (Exception ex)
            {
                if (m_GeometryEditService != null)
                {
                    m_GeometryEditService.RemoveAllGeometries();
                    m_GeometryEditService = null;

                }
                m_PTHelper = null;
                MessageBox.Show("Error in Mouse move during the placement of Assembly equipment." + Environment.NewLine + ex.Message, Caption);
            }
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
                if (m_MoveInProgress || m_RotateInProgress)
                {
                    return;
                }
                m_PTHelper = PTHelper;
                m_KeyObject = KeyObject;
                m_KeyObjectCollection = KeyObjectCollection;

                m_PTHelper.ConstructionAidsEnabled = Intergraph.GTechnology.API.GTConstructionAidsEnabledConstants.gtptConstructionAidsNone;
                m_PTHelper.ConstructionAidDynamicsEnabled = false;
                m_PTHelper.StatusBarPromptsEnabled = true;

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
                frmAssmbly = new frmAssembly();
                frmAssmbly.Application = m_GTApplication;
                frmAssmbly.FeatureFNO = m_ActiveGraphicComponent.FNO;
                frmAssmbly.DetailIdentifier = m_ActiveMapWindow.DetailID;
                DialogResult dr = frmAssmbly.ShowDialog();
                if (dr == DialogResult.Cancel)
                {
                    if (m_PTHelper != null)
                    {
                        m_PTHelper.AbortPlacement();
                        m_PTHelper = null;
                    }
                    return;
                }

                //Reading the arguments
                double width = 0;
                double height = 0;
                GetDimensionsFromMBR(out width, out height);

                string locationArgument = Convert.ToString(m_ActiveGraphicComponent.Arguments.GetArgument(0));
                if (string.IsNullOrEmpty(locationArgument))
                {
                    throw new Exception("Metadata arguments not configured correctly.");
                }
                IGTPolygonGeometry oCPLGeom = null;
                double distance = m_ActiveMapWindow.DetailID == 0 ? Convert.ToDouble(locationArgument.Split(',')[0]) : Convert.ToDouble(locationArgument.Split(',')[1]);

                if (selectedObject.Geometry.Type == GTGeometryTypeConstants.gtgtOrientedPointGeometry)
                {
                    IGTKeyObject gTKeyObject = m_GTApplication.DataContext.OpenFeature(m_ActiveGraphicComponent.FNO, m_ActiveGraphicComponent.FID);
                    IGTOrientedPointGeometry iPointGeom = (IGTOrientedPointGeometry)selectedObject.Geometry;
                    oCPLGeom = GetPolyGeom(iPointGeom.Origin, width, height, distance);
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

                    int vertexCount = iPolygonGeom.Points.Count - 1;
                    Point2D centroid = Compute2DPolygonCentroid(polygonPoints, vertexCount);

                    m_CentriodPoint = GTClassFactory.Create<IGTPoint>();
                    m_CentriodPoint.X = centroid.x;
                    m_CentriodPoint.Y = centroid.y;
                    m_CentriodPoint.Z = 0.0;

                    oCPLGeom = GetPolyGeom(m_CentriodPoint, width, height, distance);
                    RotateGeom(iPolygonGeom, ref oCPLGeom);
                }
                if (oCPLGeom != null)
                {
                    m_priSwitchGearGeometry = oCPLGeom;
                    tempSwitchGearPoints = new List<IGTPoint>();
                    tempSwitchGearPoints.AddRange(m_priSwitchGearGeometry.Points);
                    m_GTApplication.SetStatusBarText(GTStatusPanelConstants.gtaspcMessage, "");
                    m_GeometryEditService = GTClassFactory.Create<IGTGeometryEditService>();
                    m_GeometryEditService.TargetMapWindow = m_GTApplication.ActiveMapWindow;
                    m_GeometryEditService.AddGeometry(m_priSwitchGearGeometry, 19401, true, true);
                    m_PTHelper.SetGeometry(m_priSwitchGearGeometry);
                    m_GTApplication.SetStatusBarText(GTStatusPanelConstants.gtaspcMessage, "Click to place object. Right click to skip move.");
                    m_MoveInProgress = true;
                    m_GTApplication.ActiveMapWindow.MousePointer = GTMousePointerConstants.gtmwmpNWArrow;
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        private void GetDimensionsFromMBR(out double length, out double breadth)
        {

            try
            {
                string assemblyTable = m_ActiveMapWindow.DetailID == 0 ? "E$PRISWGEAR_P" : "E$PRISWGEAR_DP";
                if (frmAssmbly.FeatureFNO == 153)
                {
                    assemblyTable = "E$SECSWGEAR_DP";
                }
                string lengthQuery = @"SELECT
                                ((SELECT SDO_GEOM.SDO_MAX_MBR_ORDINATE(c.G3E_GEOMETRY, m.diminfo, 1)
                                  FROM " + assemblyTable + @" c, all_sdo_geom_metadata m
                                  WHERE m.table_name = " + " '" + assemblyTable + "' " + @" AND m.column_name = 'G3E_GEOMETRY'
                                  AND c.G3E_FID = {0})
                                  -
                                (SELECT SDO_GEOM.SDO_MIN_MBR_ORDINATE(c.G3E_GEOMETRY, m.diminfo, 1)
                                FROM " + assemblyTable + @" c, all_sdo_geom_metadata m
                                WHERE m.table_name = " + " '" + assemblyTable + "' " + @"  AND m.column_name = 'G3E_GEOMETRY'
                                AND c.G3E_FID = {0})) length
                                FROM DUAL";

                string breadthQuery = @"SELECT                
                                ((SELECT SDO_GEOM.SDO_MAX_MBR_ORDINATE(c.G3E_GEOMETRY, m.diminfo, 2)
                                  FROM " + assemblyTable + @" c, all_sdo_geom_metadata m
                                  WHERE m.table_name = " + " '" + assemblyTable + "' " + @" AND m.column_name = 'G3E_GEOMETRY'
                                  AND c.G3E_FID = {0})
                                  -
                                 (SELECT SDO_GEOM.SDO_MIN_MBR_ORDINATE(c.G3E_GEOMETRY, m.diminfo, 2)
                                 FROM " + assemblyTable + @" c, all_sdo_geom_metadata m
                                 WHERE m.table_name =  " + " '" + assemblyTable + "' " + @" AND m.column_name = 'G3E_GEOMETRY'
                                 AND c.G3E_FID = {0})) breadth
                                 FROM DUAL";
                Recordset lengthRs = GetRecordSet(string.Format(lengthQuery, frmAssmbly.FeatureIdentifier));
                length = Convert.ToDouble(lengthRs.Fields["LENGTH"].Value);

                Recordset breadthRs = GetRecordSet(string.Format(breadthQuery, frmAssmbly.FeatureIdentifier));
                breadth = Convert.ToDouble(breadthRs.Fields["BREADTH"].Value);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error reading catalog assembly information. Please contact system administrator.", "G/Technology", MessageBoxButtons.OK, MessageBoxIcon.Error);
                throw;
            }
        }

        private IGTPolygonGeometry GetPolyGeom(IGTPoint iPt, double width, double height, double distanceFromOrigin)
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
                IGTPoint oStartPt = GTClassFactory.Create<IGTPoint>();

                if (m_ActiveMapWindow.DetailID > 0)
                {
                    oMidPT.X = iPt.X;
                    oMidPT.Y = iPt.Y + distanceFromOrigin;

                    oStartPt.X = oMidPT.X - (width / 2);
                    oStartPt.Y = oMidPT.Y - (width / 2);

                    oPolyLineGeom.Points.Add(oStartPt);

                    oTmpPt.X = oStartPt.X + width;
                    oTmpPt.Y = oStartPt.Y;
                    oPolyLineGeom.Points.Add(oTmpPt);

                    oTmpPt.X = oTmpPt.X;
                    oTmpPt.Y = oTmpPt.Y + height;
                    oPolyLineGeom.Points.Add(oTmpPt);

                    oTmpPt.X = oTmpPt.X - width;
                    oTmpPt.Y = oTmpPt.Y;
                    oPolyLineGeom.Points.Add(oTmpPt);

                    oPolyLineGeom.Points.Add(oStartPt);
                }

                else
                {
                    oMidPT.X = iPt.X;
                    oMidPT.Y = iPt.Y + distanceFromOrigin;

                    oStartPt.X = oMidPT.X + (-width / 2);
                    oStartPt.Y = oMidPT.Y;
                    oPolyLineGeom.Points.Add(oStartPt);

                    oTmpPt.X = oMidPT.X + (width / 2);
                    oTmpPt.Y = oMidPT.Y;
                    oPolyLineGeom.Points.Add(oTmpPt);

                    oTmpPt.X = oTmpPt.X;
                    oTmpPt.Y = oTmpPt.Y + (height);
                    oPolyLineGeom.Points.Add(oTmpPt);

                    oTmpPt.X = oTmpPt.X - (width);
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

            try
            {
                if (frmAssmbly.FeatureIdentifier != 0)
                {
                    ownerID = GetOwnerIdentifier();
                    GetAssemblyPointsAndOrientation(oGeometry, ref m_dGeoOrientation);
                    ADODB.Recordset oRS = m_GTApplication.DataContext.Execute("{call OC_EXECUTE_DEEPCOPY(" + m_ActiveGraphicComponent.FNO.ToString() + " ," +
                                                                     frmAssmbly.FeatureIdentifier.ToString() + ", " +
                                                                     m_ActiveGraphicComponent.FNO.ToString() + ", " +
                                                                     m_ActiveGraphicComponent.CNO.ToString() + ", " +
                                                                     m_ActiveGraphicComponent.FID.ToString() + ", " +
                                                                     ownerID.ToString() + ", " + m_dGeoOrientation + ", " + m_ActiveMapWindow.DetailID + ")}", out lRecords, 1, new object[1]);
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
                IGTPoint oPoint0 = oGeometry.Points[3];
                IGTPoint oPoint1 = oGeometry.Points[2];

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
                    //dOrientation = -(pi / 2);
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

        private void SetWRForChildren()
        {
            IGTKeyObject oPSG = m_GTApplication.DataContext.OpenFeature(m_KeyObject.FNO, m_KeyObject.FID);
            IGTKeyObjects oOwnedChildren = null;
            using (IGTRelationshipService oRelSVR = GTClassFactory.Create<IGTRelationshipService>())
            {
                oRelSVR.DataContext = m_GTApplication.DataContext;
                oRelSVR.ActiveFeature = oPSG;
                short iRNO = 2;
                oOwnedChildren = oRelSVR.GetRelatedFeatures(iRNO);
            }
            if (oOwnedChildren != null)
            {
                SetActiveJob(oOwnedChildren, 21);
                SetActiveJob(oOwnedChildren, 22);
            }
        }

        private void SetActiveJob(IGTKeyObjects p_oOwnedChildren, short p_CNO)
        {
            foreach (IGTKeyObject item in p_oOwnedChildren)
            {
                IGTComponent oCUComponent = item.Components.GetComponent(p_CNO);

                if (oCUComponent != null)
                {
                    if (oCUComponent.Recordset != null)
                    {
                        if (oCUComponent.Recordset.RecordCount > 0)
                        {
                            oCUComponent.Recordset.MoveFirst();
                            while (oCUComponent.Recordset.EOF == false)
                            {
                                oCUComponent.Recordset.Fields["WR_ID"].Value = m_GTApplication.DataContext.ActiveJob;
                                oCUComponent.Recordset.Fields["WR_EDITED"].Value = null;
                                oCUComponent.Recordset.MoveNext();
                            }
                        }
                    }
                }
            }
        }
        /// <summary>
        /// Method to copy the switch gear components information from saved assembly equipment to the newly constructed switch gear.
        /// The component information is read from the E$ tables.
        /// </summary>
        private void CopyComponentInformationForSwitchGear()
        {
            bool cuCompleted = false;
            string componentName = string.Empty;
            string selectSql = "SELECT * FROM E${0} WHERE G3E_FNO = {1} AND G3E_FID = {2}";

            try
            {
                if (frmAssmbly.FeatureIdentifier == 0)
                {
                    return;
                }
                List<string> bTables = GetTables();
                IGTKeyObject targetActiveFeature = m_GTApplication.DataContext.OpenFeature(m_ActiveGraphicComponent.FNO, m_ActiveGraphicComponent.FID);
                IGTComponents targetComponents = targetActiveFeature.Components;
                foreach (string table in bTables)
                {
                    try
                    {
                        componentName = table;
                        if (cuCompleted)
                        {
                            componentName = "COMP_UNIT_ANCIL_N";
                            selectSql = selectSql + " AND G3E_CNO = 22 ";
                            cuCompleted = false;
                        }

                        Recordset source = GetRecordSet(string.Format(selectSql, table, frmAssmbly.FeatureFNO, frmAssmbly.FeatureIdentifier));
                        IGTComponent targetComp = targetComponents[componentName];
                        Recordset target = targetComp.Recordset;

                        if (source != null && source.RecordCount > 0 && target != null && target.RecordCount > 0)
                        {
                            target.MoveFirst();
                            source.MoveFirst();

                            for (int i = 0; i < source.Fields.Count; i++)
                            {
                                string fieldName = Convert.ToString(source.Fields[i].Name);
                                if ((fieldName.Substring(0, 3) != "G3E") && (fieldName.Substring(0, 3) != "LTT"))
                                {
                                    try
                                    {
                                        if (fieldName == "WR_ID")
                                        {
                                            target.Fields[fieldName].Value = m_GTApplication.DataContext.ActiveJob;
                                        }
                                        else if (fieldName == "WR_EDITED")
                                        {
                                            target.Fields[fieldName].Value = null;
                                        }
                                        else
                                        {
                                            target.Fields[fieldName].Value = source.Fields[fieldName].Value;
                                        }
                                    }
                                    catch
                                    { }
                                }
                            }
                        }

                        else
                        {
                            if ((target.RecordCount == 0 || (target.EOF)) && (source != null && source.RecordCount > 0))
                            {
                                source.MoveFirst();
                                target.AddNew("G3E_FID", m_ActiveGraphicComponent.FID);
                                target.Fields["G3E_FNO"].Value = m_ActiveGraphicComponent.FNO;
                                target.Fields["G3E_CNO"].Value = targetComponents[componentName].CNO;

                                for (int i = 0; i < source.Fields.Count; i++)
                                {

                                    string fieldName = Convert.ToString(source.Fields[i].Name);
                                    if ((fieldName.Substring(0, 3) != "G3E") && (fieldName.Substring(0, 3) != "LTT"))
                                    {
                                        try
                                        {
                                            target.Fields[fieldName].Value = source.Fields[fieldName].Value;
                                        }
                                        catch
                                        { }
                                    }
                                }
                            }
                        }

                        if (table.Equals("COMP_UNIT_N") && componentName.Equals("COMP_UNIT_N"))
                        {
                            cuCompleted = true;
                        }
                    }
                    catch (Exception EX) // E$ table not found for the corresponding B$ table
                    {
                        continue;
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
        }


        /// <summary>
        /// Gets all the Non graphic tables for the primary switch gear
        /// </summary>
        /// <returns>List of non graphic tables</returns>
        private List<string> GetTables()
        {
            List<string> tables = new List<string>();
            try
            {
                Recordset rs = m_GTApplication.DataContext.MetadataRecordset("G3E_FEATURECOMPS_OPTABLE", " G3E_FNO = " + frmAssmbly.FeatureFNO + " AND G3E_TYPE = 1 ORDER BY G3E_CNO"); // PRIMARY SWITCH GEAR - NON GRAPHIC TABLES
                if (rs != null && rs.RecordCount > 0)
                {
                    rs.MoveFirst();
                    while (!rs.EOF)
                    {
                        tables.Add(Convert.ToString(rs.Fields["G3E_TABLE"].Value));
                        rs.MoveNext();
                    }
                }
                return tables;
            }
            catch (Exception)
            {
                throw;
            }

        }

        /// <summary>
        /// Method to execute sql query and return the result record set
        /// </summary>
        /// <param name="sqlString"></param>
        /// <returns></returns>
        private Recordset GetRecordSet(string sqlString)
        {
            try
            {
                int outRecords = 0;
                Command command = new ADODB.Command();
                command.CommandText = sqlString;
                Recordset results = m_GTApplication.DataContext.ExecuteCommand(command, out outRecords);
                return results;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
