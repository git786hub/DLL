using ADODB;
using Intergraph.CoordSystems;
using Intergraph.GTechnology.API;
using System;
using System.Windows.Forms;

namespace GTechnology.Oncor.CustomAPI
{
    public class clsCPTProcessSelectSet
    {
        private IGTApplication m_Application = GTClassFactory.Create<IGTApplication>();
        private IGTRelationshipService relationshipService = GTClassFactory.Create<IGTRelationshipService>();
        private int m_SelectedFID = 0;
        private short m_SelectedFNO = 0;
        private string m_node1Stucture = string.Empty;
        private string m_node2Stucture = string.Empty;
        private string m_featureState = string.Empty;
        private string m_CU = string.Empty;
        private bool m_AllowCUEdit = false;

        private struct CablePullPoint
        {
            public int iFNO;
            public int lFID;
            public IGTPoint oPnt;
            public Boolean IsEndPoint;
            public Double dBendAngle;
        }
        public struct CablePullSegment
        {
            public int iFNO;
            public int lFID;
            public int iLength;
            public IGTPolylineGeometry oSeg;
        }

        private double[] m_aBends; //The array of bend values
        private float m_fBendSearchDistance; //The distance along a series of bends to detemine when a true bend occurs.

        private CablePullPoint[] m_aSortedPoints; //The array of sorted points that define all the segments
        private CablePullSegment[] m_aSegments; //The array of segments created from the array of sorted points

        public CablePullSegment[] Segments
        {
            get
            {
                return m_aSegments;
            }
            set
            {
                m_aSegments = value;
            }
        }

        public double[] Bends
        {
            get
            {
                return m_aBends;
            }
            set
            {
                m_aBends = value;
            }
        }

        public int SelectedFID
        {
            get
            {
                return m_SelectedFID;
            }
            set
            {
                m_SelectedFID = value;
            }
        }

        public short SelectedFNO
        {
            get
            {
                return m_SelectedFNO;
            }
            set
            {
                m_SelectedFNO = value;
            }
        }
        public string node1Structure 
        {
            get
            {
                return m_node1Stucture;
            }
            set
            {
                m_node1Stucture = value;
            }
        }
        public string node2Structure 
        {
            get
            {
                return m_node2Stucture;
            }
            set
            {
                m_node2Stucture = value;
            }
        }

        public string featureState 
        {
            get
            {
                return m_featureState;
            }
            set
            {
                m_featureState = value;
            }
        }

        public string CU
        {
            get
            {
                return m_CU;
            }
            set
            {
                m_CU = value;
            }
        }

        public bool AllowCUEdit
        {
            get
            {
                return m_AllowCUEdit;
            }
            set
            {
                m_AllowCUEdit = value;
            }
        }
        //public string node1Structure

        public bool ProcessSelectSet()
        {
            bool bReturnValue = false;

            try
            {

                m_aSortedPoints = new CablePullPoint[1];
                m_aSegments = new CablePullSegment[1];

                IGTDDCKeyObjects oGTDCKeys = GTClassFactory.Create<IGTDDCKeyObjects>();
                oGTDCKeys = m_Application.SelectedObjects.GetObjects();
                relationshipService.DataContext = m_Application.DataContext;
                int index = 0;
                m_SelectedFID = oGTDCKeys[index].FID;
                m_SelectedFNO = oGTDCKeys[index].FNO;
                relationshipService.ActiveFeature = m_Application.DataContext.OpenFeature(m_SelectedFNO, m_SelectedFID);

                if (relationshipService.ActiveFeature.Components.GetComponent(1).Recordset.Fields["FEATURE_STATE_C"].Value != null)
                {
                    m_featureState = relationshipService.ActiveFeature.Components.GetComponent(1).Recordset.Fields["FEATURE_STATE_C"].Value.ToString();
                }

                if (relationshipService.ActiveFeature.Components.GetComponent(21).Recordset.RecordCount > 0)
                {
                    relationshipService.ActiveFeature.Components.GetComponent(21).Recordset.MoveFirst();
                    m_CU = relationshipService.ActiveFeature.Components.GetComponent(21).Recordset.Fields["CU_C"].Value.ToString();
                }

                if (CommonDT.AllowCUEdit(relationshipService.ActiveFeature))
                {
                    m_AllowCUEdit = true;
                }
                else
                {
                    m_AllowCUEdit = false;
                }

                int ductBankFID = 0;

                try
                {

                    IGTKeyObjects relatedDuctKOs = relationshipService.GetRelatedFeatures(6);

                    if (relatedDuctKOs.Count > 0)
                    {
                        relationshipService.ActiveFeature = relatedDuctKOs[0];
                        IGTKeyObjects relatedFormationKOs = relationshipService.GetRelatedFeatures(6);
                        if (relatedFormationKOs.Count > 0)
                        {
                            relationshipService.ActiveFeature = relatedFormationKOs[0];
                            IGTKeyObjects relatedDuctBankKOs = relationshipService.GetRelatedFeatures(3);
                            if (relatedDuctBankKOs.Count > 0)
                            {
                                ductBankFID = relatedDuctBankKOs[0].FID;
                                relationshipService.ActiveFeature = relatedDuctBankKOs[0];
                                IGTKeyObjects structure1 = relationshipService.GetRelatedFeatures(122, GTRelationshipOrdinalConstants.gtrelRelationshipOrdinal1);
                                IGTKeyObjects structure2 = relationshipService.GetRelatedFeatures(122, GTRelationshipOrdinalConstants.gtrelRelationshipOrdinal2);
                                if (structure1.Count > 0)
                                {
                                    IGTKeyObject thing = structure1[0];
                                    thing.Components.GetComponent(1).Recordset.MoveFirst();
                                    if (!thing.Components.GetComponent(1).Recordset.BOF && !thing.Components.GetComponent(1).Recordset.EOF)
                                    {
                                        m_node1Stucture = thing.Components.GetComponent(1).Recordset.Fields["STRUCTURE_ID"].Value.ToString();
                                    }
                                }
                                if (structure2.Count > 0)
                                {
                                    IGTKeyObject thing = structure2[0];
                                    thing.Components.GetComponent(1).Recordset.MoveFirst();
                                    if (!thing.Components.GetComponent(1).Recordset.BOF && !thing.Components.GetComponent(1).Recordset.EOF)
                                    {
                                        m_node2Stucture = thing.Components.GetComponent(1).Recordset.Fields["STRUCTURE_ID"].Value.ToString();
                                    }
                                }
                            }
                        }
                    }
                    else
                    {
                        MessageBox.Show(m_Application.ApplicationWindow, ConstantsDT.ERROR_CPT_NO_RELATED_DUCTBANK, ConstantsDT.COMMAND_NAME_CABLE_PULL_TENSION, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        return false;
                    }
                }
                catch
                {

                }


                // Get the primary geographic and primary detail componentviews from metadata
                // Get the selected object matching the primary geographic or primary detail componentviews in that order
                Recordset metadataRS = m_Application.DataContext.MetadataRecordset("G3E_FEATURES_OPTABLE", "g3e_fno = " + ConstantsDT.FNO_DUCTBANK);
                short primaryGeographicCNO = 0;
                short primaryDetailCNO = 0;
                if (metadataRS.RecordCount > 0)
                {
                    if (!Convert.IsDBNull(metadataRS.Fields["G3E_PRIMARYGEOGRAPHICCNO"].Value))
                    {
                        primaryGeographicCNO = Convert.ToInt16(metadataRS.Fields["G3E_PRIMARYGEOGRAPHICCNO"].Value);
                    }

                    if (!Convert.IsDBNull(metadataRS.Fields["G3E_PRIMARYDETAILCNO"].Value))
                    {
                        primaryDetailCNO = Convert.ToInt16(metadataRS.Fields["G3E_PRIMARYDETAILCNO"].Value);
                    }
                }

                IGTDDCKeyObjects oDuctBankGTDCKeys = GTClassFactory.Create<IGTDDCKeyObjects>();
                oDuctBankGTDCKeys = m_Application.DataContext.GetDDCKeyObjects(ConstantsDT.FNO_DUCTBANK, ductBankFID, GTComponentGeometryConstants.gtddcgAllPrimary);

                metadataRS = m_Application.DataContext.MetadataRecordset("G3E_COMPONENTVIEWS_OPTABLE");
                string filter = "(g3e_fno = " + ConstantsDT.FNO_DUCTBANK + " and g3e_cno = " + primaryGeographicCNO + ") or (g3e_fno = " + ConstantsDT.FNO_DUCTBANK + " and g3e_cno = " + primaryDetailCNO + ")";
                metadataRS.Filter = filter;
                if (metadataRS.RecordCount > 0)
                {
                    metadataRS.Sort = "g3e_detail";

                    while (!metadataRS.EOF)
                    {
                        if (metadataRS.Fields["G3E_VIEW"].Value.ToString() == oDuctBankGTDCKeys[index].ComponentViewName)
                        {
                            //m_SelectedFID = oGTDCKeys[index].FID;
                            //m_SelectedFNO = oGTDCKeys[index].FNO;
                            break;
                        }
                        metadataRS.MoveNext();
                        index++;
                    }


                    IGTGeometry oGeometry = oDuctBankGTDCKeys[index].Geometry;
                    IGTPolylineGeometry oPolyLineGeom = GTClassFactory.Create<IGTPolylineGeometry>();

                    if (oGeometry.Type == GTGeometryTypeConstants.gtgtPolylineGeometry || oGeometry.Type == GTGeometryTypeConstants.gtgtLineGeometry)
                    {
                        // Geometry is okay to use as it is.
                        oPolyLineGeom = (IGTPolylineGeometry)oGeometry;
                        bReturnValue = true;
                    }
                    else if (oGeometry.Type == GTGeometryTypeConstants.gtgtCompositePolylineGeometry)
                    {
                        // Any arcs must be stroked to line segments
                        oPolyLineGeom = (IGTPolylineGeometry)oGeometry.Stroke();
                        bReturnValue = true;
                    }
                    else
                    {
                        // Invalid geometry type.
                        MessageBox.Show(m_Application.ApplicationWindow, ConstantsDT.ERROR_CPT_INVALID_GEOMETRY, ConstantsDT.COMMAND_NAME_CABLE_PULL_TENSION, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        bReturnValue = false;
                    }

                    if (bReturnValue)
                    {
                        // Store each point of the line in the feature's geometry
                        StoreLinePoints(oPolyLineGeom, m_SelectedFNO, m_SelectedFID);

                        //Compute the included angles for each set of three points in the sorted array
                        ComputeIncludedAngles();

                        //Build the arrays for the segments and bends that will comprise the pull
                        if (!BuildSegmentsAndBends())
                        {
                            bReturnValue = false;
                        }
                    }
                }
                else
                {
                    MessageBox.Show(m_Application.ApplicationWindow, ConstantsDT.ERROR_CPT_INVALID_COMPONENT_SELECTED, ConstantsDT.COMMAND_NAME_CABLE_PULL_TENSION, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    bReturnValue = false;
                }
            }
            catch(Exception e)
            {
                m_aSegments = new CablePullSegment[1];
                m_aBends = new double[1];
                m_aSortedPoints = new CablePullPoint[1];
            }

            return bReturnValue;
        }

        private bool StoreLinePoints(IGTPolylineGeometry oPolyLineGeom, int iFNO, int lFID)
        {
            bool bReturnValue = false;
            
            try
            {
                IGTPoints oPnts = oPolyLineGeom.Points;

                //Get the size of the current array of unsorted points
                int j = m_aSortedPoints.Length - 1;

                //Set the new upper boundary for the array
                int k = j + oPnts.Count;

                //Reallocate memory for the new points
                Array.Resize(ref m_aSortedPoints, k);

                //Store the values in the array
                for (int i = j; i < k; i++)
                {
                    m_aSortedPoints[i].oPnt = oPnts[i - j];
                    m_aSortedPoints[i].iFNO = iFNO;
                    m_aSortedPoints[i].lFID = lFID;

                    //Set the endpoint indicator after loop
                    m_aSortedPoints[i].IsEndPoint = false;
                }
                m_aSortedPoints[j].IsEndPoint = true;
                m_aSortedPoints[k - 1].IsEndPoint = true;
                oPnts = null;

                bReturnValue = true;
                
            }
            catch (Exception ex)
            {
                MessageBox.Show(m_Application.ApplicationWindow, ConstantsDT.ERROR_CPT_PROCESSING_GEOMETRY + ": " + ex.Message, ConstantsDT.COMMAND_NAME_CABLE_PULL_TENSION, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                bReturnValue = false;
            }

            return bReturnValue;
        }

        private void ComputeIncludedAngles()
        {
            int i = 0;
            double dAngle1;
            double dAngle2;
            double dIncludedAngle;

            IGTPoint oP1 = GTClassFactory.Create<IGTPoint>();
            IGTPoint oP2 = GTClassFactory.Create<IGTPoint>();

            // There isn't an angle, as such, at the end points.
            // Set this value to zero to support the algorithm
            // that computes the bends and straight-line segments.
            m_aSortedPoints[1].dBendAngle = 0;
            m_aSortedPoints[m_aSortedPoints.Length - 1].dBendAngle = 0;

            //Calculate the angle of each segment
            //for (i = 2; i <= (m_aSortedPoints.Length - 1) - 1; i++)
            //{
            //    oP1 = m_aSortedPoints[i].oPnt;
            //    oP2 = m_aSortedPoints[i - 1].oPnt;
            //    dAngle1 = GetAngleByTwoPoints(oP1, oP2);
            //    oP2 = m_aSortedPoints[i + 1].oPnt;
            //    dAngle2 = GetAngleByTwoPoints(oP1, oP2);
            //    dIncludedAngle = Math.Abs(dAngle1 - dAngle2);
            //    m_aSortedPoints[i].dBendAngle = Math.Abs(180 - dIncludedAngle);
            //}
            for (i = 1; i <= (m_aSortedPoints.Length - 1) - 1; i++)
            {
                oP1 = m_aSortedPoints[i].oPnt;
                oP2 = m_aSortedPoints[i - 1].oPnt;
                dAngle1 = GetAngleByTwoPoints(oP1, oP2);
                oP2 = m_aSortedPoints[i + 1].oPnt;
                dAngle2 = GetAngleByTwoPoints(oP1, oP2);
                dIncludedAngle = Math.Abs(dAngle1 - dAngle2);
                m_aSortedPoints[i].dBendAngle = Math.Round((Math.Abs(180 - dIncludedAngle)), 0);
            }
            oP1 = null;
            oP2 = null;
        }

        private double GetAngleByTwoPoints(IGTPoint oP1, IGTPoint oP2)
        {
            double returnValue = 0;
            
            double dRetVal = 0;
            double dXVal = oP2.X - oP1.X;
            double dYVal = oP2.Y - oP1.Y;

            if ((int)dXVal == 0)
            {
                if (dYVal > 0)
                {
                    dRetVal = 90;
                }
                else if (dYVal < 0)
                {
                    dRetVal = 270;
                }
                else if ((int)dYVal == 0)
                {
                    //Should never happen - points would be coincident.
                    //Default to zero degrees.
                    dRetVal = 0;
                }
            }
            else if (dXVal > 0)
            {
                if (dYVal > 0)
                {
                    //First quadrant
                    dRetVal = System.Math.Atan(System.Convert.ToDouble(dYVal / dXVal)) * (180.0 / Math.PI);// RadiansToDegrees;
                }
                else if (dYVal < 0)
                {
                    //Fourth quadrant
                    dRetVal = System.Math.Atan(System.Convert.ToDouble(dYVal / dXVal)) * (180.0 / Math.PI);// RadiansToDegrees;
                }
                else if ((int)dYVal == 0)
                {
                    dRetVal = 0;
                }
            }
            else if (dXVal < 0)
            {
                if ((int)dYVal == 0)
                {
                    dRetVal = 180;
                }
                else
                {
                    //Second  or third quadrant
                    dRetVal = System.Math.Atan(System.Convert.ToDouble(dYVal / dXVal)) * (180.0 / Math.PI);// RadiansToDegrees;
                    dRetVal = dRetVal + 180;
                }
            }
            returnValue = System.Convert.ToDouble(dRetVal);
            return returnValue;
        }

        private bool BuildSegmentsAndBends()
        {
            bool returnValue = false;
            //Based on the segment and bends from the select set geometry,
            //define the pieces of the geometry will be considered
            //straight-line segments and where the bends will occur.
            //The algorithm is:
            //Start with the second point in the array of sorted points.
            //Accumulate the distance from the previous point.
            //Accumulate the angle at this bend.
            //If the distance is greater than the search distance, set it to zero.
            //If the distance is less than or equal to the search distance
            //  and the angle exceeds the minimum bend angle,
            //  define a bend.
            //Zero the length and bend angle.
            //Continue until the end of the array of sorted points is reached.
            int iArrSize = 0;
            bool bRetVal = false;
            int i = 0;
            double dTmp = 0.0;
            bRetVal = false;
            m_aSegments = new CablePullSegment[1];
            m_aBends = new double[1];
            iArrSize = (m_aSortedPoints.Length - 1);
            int j = 0;
            float fDistanceAccumulator = 0;
            float fAngleAccumulator = 0;
            
            for (i = 1; i <= iArrSize; i++)
            {
                //Sum the cumulative distance and angle
                double temp_dDistance = dTmp;
                if (!DistanceBetweenPoints(m_aSortedPoints[i].oPnt, m_aSortedPoints[i - 1].oPnt, ref temp_dDistance))
                {
                    return bRetVal;
                }
                //fDistanceAccumulator = fDistanceAccumulator + Convert.ToSingle(dTmp);
                ////If the distance exceeds the search distance, the length must be reset;
                ////otherwise, long straight segments with a bend won't be recognized.
                //if (fDistanceAccumulator > m_fBendSearchDistance)
                //{
                //    fDistanceAccumulator = 0;
                //}
                //The angles can continue to accumulate until a bend is found.
                fAngleAccumulator = fAngleAccumulator + Convert.ToSingle(m_aSortedPoints[i].dBendAngle);
                //if (fDistanceAccumulator <= m_fBendSearchDistance && fAngleAccumulator > Convert.ToSingle(m_MinimumBendRadius))
                if (fDistanceAccumulator <= m_fBendSearchDistance && fAngleAccumulator > Convert.ToSingle(0))
                {
                    if (!AddSegment(j, i))
                    {
                        return bRetVal;
                    }
                    if (!AddBend(System.Convert.ToDouble(fAngleAccumulator)))
                    {
                        return bRetVal;
                    }
                    //When a bend is realized, reset the search parameters to zero
                    fDistanceAccumulator = 0;
                    fAngleAccumulator = 0;
                    j = i;
                }
            }
            if (!AddSegment(j, i - 1))
            {
                //goto cleanup;
                returnValue = bRetVal;
                return returnValue;
            }
            bRetVal = true;
            return bRetVal;
        }

        private bool AddSegment(int iLowerIdx, int iUpperIdx)
        {
            bool returnValue = false;
            double length = 0;
            try
            {
                int iArrSize = (int)(m_aSegments.Length - 1);
                iArrSize++;
                Array.Resize(ref m_aSegments, iArrSize + 1);
                m_aSegments[iArrSize].iFNO = m_aSortedPoints[iLowerIdx].iFNO;
                m_aSegments[iArrSize].lFID = m_aSortedPoints[iLowerIdx].lFID;
                m_aSegments[iArrSize].oSeg = GTClassFactory.Create<IGTPolylineGeometry>(); //m_oApplication.CreateService("GFramme.PolylineGeometry")
                for (int i = iLowerIdx; i <= iUpperIdx; i++)
                {
                    m_aSegments[iArrSize].oSeg.Points.Add(m_aSortedPoints[i].oPnt);
                }

                if (!MeasurePolyline(m_aSegments[iArrSize].oSeg, ref length))
                {
                    return returnValue;
                }
                m_aSegments[iArrSize].iLength = Convert.ToInt16(length);

                returnValue = true;
            }
            catch (Exception ex)
            {
                //MessageBox.Show(Application.ApplicationWindow, ex.Message, System.Convert.ToString(modCPTRes.G3E_I_APP_NAME), MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                returnValue = false;
            }

            return returnValue;
        }

        private bool AddBend(double dBendAngle)
        {
            int iArrSize = 0;
            bool returnValue = false;
            try
            {
                iArrSize = m_aBends.Length - 1;
                iArrSize++;
                Array.Resize(ref m_aBends, iArrSize + 1);
                m_aBends[iArrSize] = dBendAngle;
                returnValue = true;
            }
            catch (Exception ex)
            {
                //MessageBox.Show(Application.ApplicationWindow, ex.Message, System.Convert.ToString(modCPTRes.G3E_I_APP_NAME), MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                returnValue = false;
            }

            return returnValue;
        }

        //Returns the XY distance in meters between two points.
        private bool DistanceBetweenPoints(IGTPoint oP1, IGTPoint oP2, ref double dDistance)
        {            
            bool returnValue = false;

            try
            {
                //Create a line to measure
                IGTPolylineGeometry oPolyLineGeometry = GTClassFactory.Create<IGTPolylineGeometry>();
                oPolyLineGeometry.Points.Add(oP1);
                oPolyLineGeometry.Points.Add(oP2);
                //Get the length
                if (!MeasurePolyline(oPolyLineGeometry, ref dDistance))
                {
                    return returnValue;
                }
                oPolyLineGeometry = null;
                returnValue = true;
            }
            catch (Exception ex)
            {
                //MessageBox.Show(Application.ApplicationWindow, ex.Message, Convert.ToString(modCPTRes.G3E_I_APP_NAME), MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                returnValue = false;
            }

            return returnValue;
        }

        //Returns the length(in meters) of a polyline geometry.
        private bool MeasurePolyline(IGTPolylineGeometry oPolyLineGeometry, ref double dLength)
        {
            bool returnValue = false;

            try
            {
                IGTMeasureService oMeasurementSvc = GTClassFactory.Create<IGTMeasureService>();
                int intMeasRefSpace = Convert.ToInt16(m_Application.GetPreference(GTPreferenceConstants.gtpcMeasurementReferenceSpace));
                oMeasurementSvc.ReferenceSpace = intMeasRefSpace;
                oMeasurementSvc.CoordSystem = ((ICoordSystemsMgr)m_Application.DataContext.CoordSystemsMgr).BaseCoordSystem as CoordSystem;
                //Give the measurement service the geometry
                oMeasurementSvc.Geometry = oPolyLineGeometry;
                //Get the length
                dLength = System.Convert.ToInt16(oMeasurementSvc.Length);
                // Measurement service returns length in meters. Convert to feet.
                dLength = Math.Round((dLength / .3048), 0);

                oMeasurementSvc = null;
                returnValue = true;
            }
            catch (Exception ex)
            {
                //MessageBox.Show(Application.ApplicationWindow, ex.Message, System.Convert.ToString(modCPTRes.G3E_I_APP_NAME), MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                returnValue = false;
            }

            return returnValue;
        }
    }
}
