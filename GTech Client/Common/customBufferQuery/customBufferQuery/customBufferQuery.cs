//----------------------------------------------------------------------------+
//        Class: customBufferQuery
//  Description: This custom dll exposes method to perform buffer query by accepting feature class ,geographic coordinate and a distance and performs buffer processing to find all instances of a specific feature class.
//----------------------------------------------------------------------------+
//     $Author:: hkonda                                   $
//       $Date:: 08/01/18                                 $
//   $Revision:: 1                                        $
//----------------------------------------------------------------------------+
//    $History:: customBufferQuery.cs                     $
// 
// *****************  Version 1  *****************
// User: hkonda     Date: 08/01/18   Time: 18:00  Desc : Created
//----------------------------------------------------------------------------+

using ADODB;
using Intergraph.GTechnology.API;
using System.Collections.Generic;
using System;

namespace GTechnology.Oncor.CustomAPI
{
    public class customBufferQuery
    {
        private IGTPoint m_referencePoint;
        private short m_targetFno;
        private double m_distance;
        IGTApplication m_iGtApplication;
        IGTDataContext m_iGtDataContext;
        Dictionary<int, double> m_fidDistancePair = null;


        public customBufferQuery(IGTPoint point, double distance, short fno)
        {
            this.m_referencePoint = point;
            this.m_distance = distance * 3.28084; // Input is in meters, convert meters to feet -- 1 m = 3.28084 ft
            this.m_targetFno = fno;
            this.m_iGtApplication = GTClassFactory.Create<IGTApplication>();
            this.m_iGtDataContext = m_iGtApplication.DataContext;
        }

        /// <summary>
        /// Performs buffer query with a given reference point, distance and FNO.
        /// FNO is the target feature FNO for which feature class instances are to be searched inside the buffer zone.
        /// </summary>
        /// <returns>Key Value pair of feature class instance and distance from reference point</returns>
        public Dictionary<int, double> PerformBufferQuery()
        {
            Recordset resultRecordSet = null;
            IGTGeometry gTGeometry = null;
            try
            {

                IGTOrientedPointGeometry gTOrientedPointGeometry = GTClassFactory.Create<IGTOrientedPointGeometry>();
                gTOrientedPointGeometry.Origin = m_referencePoint;

                IGTZoneService gTZoneService = GTClassFactory.Create<IGTZoneService>();
                gTZoneService.ZoneWidth = m_distance;
                gTZoneService.InputGeometries = gTOrientedPointGeometry;
                IGTGeometry outPutGeometry = gTZoneService.OutputGeometries;

                IGTSpatialService spatialService = GTClassFactory.Create<IGTSpatialService>();
                spatialService.DataContext = m_iGtDataContext;
                spatialService.Operator = GTSpatialOperatorConstants.gtsoEntirelyContains;
                spatialService.FilterGeometry = outPutGeometry;

                resultRecordSet = spatialService.GetResultsByFNO(new short[] { this.m_targetFno });
                if (resultRecordSet != null && resultRecordSet.RecordCount > 0)
                {
                    resultRecordSet.MoveFirst();
                    m_fidDistancePair = new Dictionary<int, double>();
                    short primaryGraphicCno = GetPrimaryGraphicCno(m_targetFno);
                    while (!resultRecordSet.EOF)
                    {
                        int fid = Convert.ToInt32(resultRecordSet.Fields["G3E_FID"].Value);
                        gTGeometry = m_iGtDataContext.OpenFeature(m_targetFno, fid).Components.GetComponent(primaryGraphicCno).Geometry;
                        if (gTGeometry != null)
                        {
                            m_fidDistancePair.Add(fid, CalculateDistanceFromReferencePoint(gTGeometry.FirstPoint));
                        }
                        resultRecordSet.MoveNext();
                    }
                }
                return m_fidDistancePair;
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                if (resultRecordSet != null)
                {
                    resultRecordSet.Close();
                    resultRecordSet = null;
                }
            }
        }

        /// <summary>
        /// Method to calcuate the distance of feature class instance from reference point
        /// </summary>
        /// <param name="targetPoint"></param>
        /// <returns></returns>
        private double CalculateDistanceFromReferencePoint(IGTPoint targetPoint)
        {
            try
            {
                double totalDistance = Math.Sqrt(((targetPoint.X - m_referencePoint.X) * (targetPoint.X - m_referencePoint.X)) + ((targetPoint.Y - m_referencePoint.Y) * (targetPoint.Y - m_referencePoint.Y)));
                return totalDistance;
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        ///  Method to get the primary graphic cno for fno
        /// </summary>
        /// <param name="fNo"></param>
        /// <returns>cno or 0</returns>
        private short GetPrimaryGraphicCno(short fNo)
        {
            short primaryGraphicCno = 0;
            try
            {
                Recordset tempRs = m_iGtDataContext.MetadataRecordset("G3E_FEATURES_OPTABLE", "G3E_FNO = " + fNo);
                if (tempRs != null && tempRs.RecordCount > 0)
                {
                    tempRs.MoveFirst();
                    primaryGraphicCno = Convert.ToInt16(tempRs.Fields["G3E_PRIMARYGEOGRAPHICCNO"].Value);
                }
                return primaryGraphicCno;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
