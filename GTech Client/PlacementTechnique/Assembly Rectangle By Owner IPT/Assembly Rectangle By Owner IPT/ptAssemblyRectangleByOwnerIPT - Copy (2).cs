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
        #endregion

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
            //throw new Exception("The method or operation is not implemented.");
        }

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
                MessageBox.Show(ex.Message);
            }

        }

        public void MouseDblClick(IGTMapWindow MapWindow, IGTPoint UserPoint, int ShiftState, IGTDDCKeyObjects LocatedObjects)
        {
            //throw new Exception("The method or operation is not implemented.");
            m_PTHelper.MouseDblClick(UserPoint, ShiftState);
        }

        public void MouseMove(Intergraph.GTechnology.API.IGTMapWindow MapWindow, Intergraph.GTechnology.API.IGTPoint UserPoint, int ShiftState, Intergraph.GTechnology.API.IGTDDCKeyObjects LocatedObjects, Intergraph.GTechnology.API.IGTPlacementTechniqueEventMode EventMode)
        {

        }

        public void StartPlacement(IGTPlacementTechniqueHelper PTHelper, IGTKeyObject KeyObject, IGTKeyObjects KeyObjectCollection)
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

        #endregion

        public void PlacePrimarySwitchGear(IGTDDCKeyObject selectedObject)
        {
            //Reading the arguments
            IGTPolygonGeometry oCPLGeom = null;
            double XOffset = short.Parse(m_ActiveGraphicComponent.Arguments.GetArgument(0).ToString());
            double YOffset = short.Parse(m_ActiveGraphicComponent.Arguments.GetArgument(1).ToString());
            if (selectedObject.Geometry.Type == "OrientedPointGeometry")
            {
                IGTOrientedPointGeometry iPointGeom = (IGTOrientedPointGeometry)selectedObject.Geometry;
                oCPLGeom = GetPolyGeom(iPointGeom.Origin, XOffset, YOffset);
                RotateGeom(iPointGeom, ref oCPLGeom);
            }
            else if (selectedObject.Geometry.Type == "PolygonGeometry") //GTGeometryTypeConstants.gtgtPolygonGeometry
            {
                IGTPolygonGeometry iPolygonGeom = (IGTPolygonGeometry)selectedObject.Geometry;
                // We do not know what is the first placement point. So we will loop through all the points and grab one point from two identical points
                // because First and Last point would be same
                // TODO  Is there any better way ? To confirm..
                        
                var  dupPoint = from p in iPolygonGeom.Points group p by p.X into g where g.Count() > 1 select g.ToString();

                IGTPoint duplicated = iPolygonGeom.Points.First(p => dupPoint.Contains(p.X.ToString()));

                oCPLGeom = GetPolyGeom(duplicated, XOffset, YOffset);
                RotateGeom(iPolygonGeom, ref oCPLGeom);
            }
            if (oCPLGeom != null)
            {
                m_PTHelper.SetGeometry(oCPLGeom);
                m_PTHelper.EndPlacement();
                this.AssemblyCopyToModel(oCPLGeom);
            }
        }

        private IGTPolygonGeometry GetPolyGeom(IGTPoint iPt, double xOffset, double yOffset)
        {
            IGTPolylineGeometry oPolyLineGeom = null;
            IGTPolygonGeometry oGeom = null;
            IGTPoint oTmpPt;
            IGTPoint oStartPt;
            IGTPoint oMidPT;
            try
            {

                if (iPt == null) return null;
                oPolyLineGeom = GTClassFactory.Create<IGTPolylineGeometry>();
                oGeom = GTClassFactory.Create<IGTPolygonGeometry>();
                oTmpPt = GTClassFactory.Create<IGTPoint>();
                oMidPT = GTClassFactory.Create<IGTPoint>();
                oStartPt = GTClassFactory.Create<IGTPoint>(); ;


                oMidPT.X = iPt.X;
                oMidPT.Y = iPt.Y + 10.0;

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

                foreach (IGTPoint pt in oPolyLineGeom.Points)
                {
                    oGeom.Points.Add(pt);
                }

                return oGeom;

            }

            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return oGeom;
            }

            finally
            {
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
            IGTMatrix tmpTMatrix = null;
            IGTVector transVec;

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
                transVec = GTClassFactory.Create<IGTVector>();

                if (inpGeom.GetType().ToString() == "Intergraph.GTechnology.Private.GTPolygonGeometry")
                {
                    oPolyGeom = (IGTPolygonGeometry)inpGeom.Stroke();

                    // move and rotate the geometry to the correct position 
                    transVec.I = oPolyGeom.Points[0].X * -1;
                    transVec.J = oPolyGeom.Points[0].Y * -1;
                    transVec.K = oPolyGeom.Points[0].Z * -1;
                }

                tmpTMatrix = GTClassFactory.Create<IGTMatrix>();
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

            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

            finally
            {
            }
        }
        private double AngleBetweenTwoPoints(IGTVector iVector)
        {
            double dAngleBetweenTwoPoints;
            dAngleBetweenTwoPoints = Math.Atan2(iVector.J, iVector.I) * (180 / Math.PI);
            return dAngleBetweenTwoPoints;

        }
        public double AngleBetweenTwoPoints(IGTPoint pntA, IGTPoint pntB)
        {
            double DX = 0, DY = 0;
            double dAngleBetweenTwoPoints;

            DX = pntA.X - pntB.X;
            DY = pntA.Y - pntB.Y;

            dAngleBetweenTwoPoints = Math.Atan2(DY, DX) * (180 / Math.PI);
            return dAngleBetweenTwoPoints;

        }
        internal IGTMatrix TranslationTransform(IGTVector Voffset)
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
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return null;
            }
        }
        internal static IGTMatrix RotateZTransform(double theta, char RorD)
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
                MessageBox.Show(e.Message, "RotateZTransform", MessageBoxButtons.OK);
                return null;
            }
        }
        private void AssemblyCopyToModel(IGTPolygonGeometry oGeometry)
        {
            int ownerID = 0;
            IGTPoint firstPnt = null;
            IGTPoint secondPnt = null;
            IGTPoint thirtPnt = null;
            int lRecords;
            double dGeoOrientation = 0.0;
            try
            {
                frmAssmbly = new frmAssembly();
                frmAssmbly.Application = m_GTApplication;
                frmAssmbly.FeatureFNO = m_ActiveGraphicComponent.FNO;
                frmAssmbly.ShowDialog();
                if (frmAssmbly.FeatureIdentifier != 0)
                {
                    ownerID = GetOwnerIdentifier();
                    GetAssemblyPointsAndOrientation(oGeometry, ref firstPnt, ref secondPnt, ref thirtPnt, ref dGeoOrientation);
                    //string assemblyQuery = "BEGIN G3E_DeepCopy.CopyChildrenToModel(" + m_ActiveGraphicComponent.FNO.ToString() + " ," +
                    //                                                frmAssmbly.FeatureIdentifier.ToString() + ", " +
                    //                                                m_ActiveGraphicComponent.FNO.ToString() + ", " +
                    //                                                m_ActiveGraphicComponent.CNO.ToString() + ", " +
                    //                                                m_ActiveGraphicComponent.FID.ToString() + ", " +
                    //                                                ownerID.ToString() + ", '" +
                    //                                                firstPnt.X.ToString().Replace(",", ".") + "', '" +
                    //                                                firstPnt.Y.ToString().Replace(",", ".") + "', '" +
                    //                                                secondPnt.X.ToString().Replace(",", ".") + "', '" +
                    //                                                secondPnt.Y.ToString().Replace(",", ".") + "', '" +
                    //                                                thirtPnt.X.ToString().Replace(",", ".") + "', '" +
                    //                                                thirtPnt.Y.ToString().Replace(",", ".") + "', '" +
                    //                                                dGeoOrientation.ToString().Replace(",", ".") + "'," +
                    //                                                "' 0 '," +
                    //                                                "' 0 '," +
                    //                                                "' 0 '," +
                    //                                                "' 0 '," +
                    //                                                "' 0 '," +
                    //                                                "' 0 '," +
                    //                                                "' 0 '," +
                    //                                                "' 0 '" + "); END;";

                    //string assemblyQuery = "BEGIN OC_EXECUTE_DEEPCOPY(" + m_ActiveGraphicComponent.FNO.ToString() + " ," +
                    //                                                frmAssmbly.FeatureIdentifier.ToString() + ", " +
                    //                                                m_ActiveGraphicComponent.FNO.ToString() + ", " +
                    //                                                m_ActiveGraphicComponent.CNO.ToString() + ", " +
                    //                                                m_ActiveGraphicComponent.FID.ToString() + ", " +
                    //                                                ownerID.ToString() + "); END;";
                   //Recordset oRS = m_GTApplication.DataContext.Execute(assemblyQuery, out lRecords, (int)CommandTypeEnum.adCmdText, new object());

                    //ADODB.Recordset oRS = m_GTApplication.DataContext.Execute("{call OC_INSERT_TEMP_PLR_COORD('" + sCoordinates1 + "')}", out rAffected, 1, new object[1]);
                   ADODB.Recordset oRS = m_GTApplication.DataContext.Execute("{call OC_EXECUTE_DEEPCOPY(" + m_ActiveGraphicComponent.FNO.ToString() + " ," +
                                                                    frmAssmbly.FeatureIdentifier.ToString() + ", " +
                                                                    m_ActiveGraphicComponent.FNO.ToString() + ", " +
                                                                    m_ActiveGraphicComponent.CNO.ToString() + ", " +
                                                                    m_ActiveGraphicComponent.FID.ToString() + ", " +
                                                                    ownerID.ToString() + ", " + dGeoOrientation + ")}", out lRecords, 1, new object[1]);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
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
                MessageBox.Show(ex.Message);
            }
            finally
            {
                oCompRS = null;
            }
            return returnValue;
        }
        private void GetAssemblyPointsAndOrientation(IGTPolygonGeometry oGeometry, ref IGTPoint oPoint1, ref IGTPoint oPoint2, ref  IGTPoint oPoint3, ref double dOrientation)
        {
            double pi = 0.0;
            double dx = 0.0;
            double dy = 0.0;
            string sCoordinates1 = string.Empty;
            pi = 4 * System.Math.Atan(1);
            dOrientation = 0;
            try
            {
                oPoint1 = oGeometry.Points[1];
                //oPoint1 = TransformPoint(oGeometry.Points[1]);
                oPoint2 = oGeometry.Points[2];
                // oPoint2 = TransformPoint(oGeometry.Points[2]);
                oPoint3 = oGeometry.Points[3];
                //oPoint3 = TransformPoint(oGeometry.Points[3]);

                //-----------------

                //IGTPolygonGeometry oTemp = (IGTPolygonGeometry)((System.Collections.Generic.IList<IGTGeometry>)(oGeometry));
                IGTPolygonGeometry oTemp = (IGTPolygonGeometry)(oGeometry);
                for (int i = 0; i < oTemp.Points.Count; i++)
                {
                    sCoordinates1 = string.Join(",", new string[] { sCoordinates1, oTemp.Points[i].X.ToString(), oTemp.Points[i].Y.ToString(), oTemp.Points[i].Z.ToString() });
                }

                
                sCoordinates1 = sCoordinates1.Remove(0, 1);
               // sCoordinates1 = "test";
                //Command cmd = new Command();
                //cmd.CommandText = " {call OC_INSERT_TEMP_PLR_COORD(" + sCoordinates1 + ")}";
                //cmd.CommandType = CommandTypeEnum.adCmdText;


               // Parameter Coord1 = cmd.CreateParameter("Coord1", DataTypeEnum.adVarChar, ParameterDirectionEnum.adParamInput, 4000, sCoordinates1);
               // cmd.Parameters.Append(Coord1);

               // cmd.Parameters.Append(cmd.CreateParameter("Coord1", DataTypeEnum.adVarChar, ParameterDirectionEnum.adParamInput, 4000, sCoordinates1));

                int rAffected = 0;
                // Execute .NET stored procedure
                //m_GTApplication.DataContext.ExecuteCommand(cmd, out rAffected,(int) ADODB.CommandTypeEnum.adCmdStoredProc);
                ADODB.Recordset oRS = m_GTApplication.DataContext.Execute("{call OC_INSERT_TEMP_PLR_COORD('" + sCoordinates1 + "')}", out rAffected, 1, new object[1]);
                //int lRecords;

                //string query = "BEGIN OC_INSERT_TEMP_PLR_COORD(" + sCoordinates1 + "); END;";
                //ADODB.Recordset oRS = m_GTApplication.DataContext.Execute(query, out lRecords, (int)ADODB.CommandTypeEnum.adCmdText, new object());

                //--------------------

                //Orientation
                dx = oPoint2.X - oPoint1.X;
                dy = oPoint2.Y - oPoint1.Y;
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
            }
        }

        //private static IGTPoint TransformPoint(IGTPoint point)
        //{

        //    double x, y, z;
        //    x = point.X;
        //    y = point.Y;
        //    z = 0;
        //    IGTDataContext da = null;
        //    IGTPoint transformedPoint = GTClassFactory.Create<IGTPoint>();
        //    Intergraph.CoordSystems.Interop.CoordSystemClass cs = new CoordSystemClass();
        //    cs.TransformPoint(Intergraph.CoordSystems.Interop.CSPointConstants.cspUOR, (int)CSTransformLinkConstants.cstlDatumTransformation,
        //       Intergraph.CoordSystems.Interop.CSPointConstants.cspLLO, (int)CSTransformLinkConstants.cstlDatumTransformation,
        //       ref x, ref y, ref z);

        //    x = x * 180 / (4 * Math.Atan(1));
        //    y = y * 180 / (4 * Math.Atan(1));
        //    transformedPoint.X = x;
        //    transformedPoint.Y = y;
        //    transformedPoint.Z = z;
        //    return transformedPoint;
        //}
    }
}
