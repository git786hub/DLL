using ADODB;
using Intergraph.GTechnology.API;
using System.Linq;
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
        Dictionary<short, short> FnoGraphicCnoPair;
        Dictionary<short, short> FnoAttributeCnoPair;
        //public IGTPoint Point
        //{
        //    get
        //    {
        //        return m_iGtpoint;
        //    }
        //    set
        //    {
        //        m_iGtpoint = value;
        //    }
        //}

        //public int Fid
        //{
        //    get
        //    {
        //        return m_fid;
        //    }
        //    set
        //    {
        //        m_fid = value;
        //    }
        //}
        //public short Fno
        //{
        //    get
        //    {
        //        return m_fno;
        //    }
        //    set
        //    {
        //        m_fno = value;
        //    }
        //}

        public customBoundaryQuery(IGTPoint Point, short BoundaryFno)
        {
            this.m_iGtpoint = Point;
            this.m_boundaryFno = BoundaryFno;
            this.m_iGtApplication = GTClassFactory.Create<IGTApplication>();
            this.m_iGtDataContext = m_iGtApplication.DataContext;
            this.CreateFnoCnoPair();
        }

        public customBoundaryQuery(IGTPoint Point, short PointFno, short BoundaryFno, int BoundryFid)
        {
            this.m_iGtpoint = Point;
            this.m_pointFno = PointFno;
            this.m_boundaryFno = BoundaryFno;
            this.m_boundaryFid = BoundryFid;
            this.m_iGtApplication = GTClassFactory.Create<IGTApplication>();
            this.m_iGtDataContext = m_iGtApplication.DataContext;
            this.CreateFnoCnoPair();
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

                if (this.m_boundaryFid == 0)
                {
                    spatialService.Operator = GTSpatialOperatorConstants.gtsoEntirelyContainedBy;
                    IGTOrientedPointGeometry pointGeometry = GTClassFactory.Create<IGTOrientedPointGeometry>();
                    IGTPoint gtPoint = GTClassFactory.Create<IGTPoint>();
                    gtPoint.X = m_iGtpoint.X;
                    gtPoint.Y = m_iGtpoint.Y;
                    gtPoint.Z = 0;
                    pointGeometry.Origin = gtPoint;
                    spatialService.FilterGeometry = pointGeometry;
                    resultRecordSet = spatialService.GetResultsByFNO(new short[] { this.m_boundaryFno });
                    return resultRecordSet;
                }
                // FID is provided. Retrieve the geometry of the boundary feature and check whether the boundary contains the point
                IGTKeyObject boundaryFeature = m_iGtDataContext.OpenFeature(this.m_boundaryFno, this.m_boundaryFid);
                IGTGeometry boundaryGeometry = (IGTGeometry)boundaryFeature.Components.GetComponent(FnoGraphicCnoPair[this.m_boundaryFno]).Geometry;
                spatialService.Operator = GTSpatialOperatorConstants.gtsoEntirelyContains;
                spatialService.FilterGeometry = boundaryGeometry;
                resultRecordSet = spatialService.GetResultsByFNO(new short[] { this.m_pointFno });
                if (resultRecordSet != null && resultRecordSet.RecordCount > 0)
                {
                    resultRecordSet = boundaryFeature.Components.GetComponent(FnoGraphicCnoPair[this.m_boundaryFno]).Recordset;
                }
                return resultRecordSet;
            }
            catch (Exception)
            {
                throw;
            }
        }

        private void CreateFnoCnoPair()
        {
            if (FnoGraphicCnoPair == null)
            {
                FnoGraphicCnoPair = new Dictionary<short, short>();
                FnoGraphicCnoPair.Add(234, 23402); // City
                FnoGraphicCnoPair.Add(235, 23502);// County
                FnoGraphicCnoPair.Add(214, 21402);// Zip code
            }
            if (FnoAttributeCnoPair == null)
            {
                FnoAttributeCnoPair = new Dictionary<short, short>();
                FnoAttributeCnoPair.Add(234, 23401); // City
                FnoAttributeCnoPair.Add(235, 23501);// County
                FnoAttributeCnoPair.Add(214, 21401);// Zip code
            }
        }
    }
}
