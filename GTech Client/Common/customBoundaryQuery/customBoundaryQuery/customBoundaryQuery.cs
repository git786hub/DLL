//----------------------------------------------------------------------------+
//        Class: customBoundaryQuery
//  Description: This custom dll exposes method to perform boudary query (point in polygon) by accepting the IGTPoint and boundary fno. This dll is used by Set Premise Location By Boundary FI.
//----------------------------------------------------------------------------+
//     $Author:: hkonda                                   $
//       $Date:: 31/10/17                                 $
//   $Revision:: 1                                        $
//----------------------------------------------------------------------------+
//    $History:: customBoundaryQuery.cs                     $
// 
// *****************  Version 1  *****************
// User: hkonda     Date: 31/10/17   Time: 18:00  Desc : Created
//----------------------------------------------------------------------------+

using ADODB;
using Intergraph.GTechnology.API;
using System.Collections.Generic;
using System;

namespace GTechnology.Oncor.CustomAPI
{
    public class customBoundaryQuery
    {
        private IGTPoint m_iGtpoint;
        private short m_pointFno;
        private short m_boundaryFno;
        private int m_boundaryFid;
        IGTApplication m_iGtApplication;
        IGTDataContext m_iGtDataContext;
    
        public customBoundaryQuery(IGTPoint point, short boundaryFno)
        {
            this.m_iGtpoint = point;
            this.m_boundaryFno = boundaryFno;
            this.m_iGtApplication = GTClassFactory.Create<IGTApplication>();
            this.m_iGtDataContext = m_iGtApplication.DataContext;
        }

        public customBoundaryQuery(IGTPoint Point, short PointFno, short BoundaryFno, int BoundryFid)
        {
            this.m_iGtpoint = Point;
            this.m_pointFno = PointFno;
            this.m_boundaryFno = BoundaryFno;
            this.m_boundaryFid = BoundryFid;
            this.m_iGtApplication = GTClassFactory.Create<IGTApplication>();
            this.m_iGtDataContext = m_iGtApplication.DataContext;
        }

        /// <summary>
        /// This method performs point in polygon query and if the point exists inside polygon, then it returns the boundary feature recordset 
        /// </summary>
        /// <returns></returns>
        public Recordset PerformPointInPolygon()
        {
            Recordset resultRecordSet = null;
            try
            {
                IGTSpatialService spatialService = GTClassFactory.Create<IGTSpatialService>();
                spatialService.DataContext = m_iGtDataContext;

                spatialService.Operator = GTSpatialOperatorConstants.gtsoEntirelyContainedBy;
                IGTOrientedPointGeometry pointGeometry = GTClassFactory.Create<IGTOrientedPointGeometry>();
                IGTPoint gtPoint = GTClassFactory.Create<IGTPoint>();
                gtPoint.X = m_iGtpoint.X;
                gtPoint.Y = m_iGtpoint.Y;
                gtPoint.Z = 0;
                pointGeometry.Origin = gtPoint;
                spatialService.FilterGeometry = pointGeometry;
                if (this.m_boundaryFid == 0)
                {
                    resultRecordSet = spatialService.GetResultsByFNO(new short[] { this.m_boundaryFno });
                    return resultRecordSet;
                }
                // FID is provided. Retrieve the geometry of the boundary feature and check whether the boundary contains the point
                resultRecordSet = spatialService.GetResultsByFNO(new short[] { this.m_boundaryFno });

                if (resultRecordSet != null && resultRecordSet.RecordCount > 0)
                {
                    resultRecordSet.MoveFirst();
                    while (!resultRecordSet.EOF)
                    {
                        if (Convert.ToString(resultRecordSet.Fields["G3E_FID"].Value) == this.m_boundaryFid.ToString())
                        {
                            return resultRecordSet;
                        }
                        resultRecordSet.MoveNext();
                    }
                }
                return resultRecordSet;
            }
            catch (Exception)
            {
                throw;
            }
        }
      
    }
}
