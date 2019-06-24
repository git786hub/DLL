// ===================================================
//  Copyright 2018 Hexagon
//  File Name: InsideFeatures.cs
// 
// Description:   
//  This class is responsible create instances of the concrete classes related to related feature types

//
//  Remarks:
// 
//  Modification History
//  Date                Author                      Comments
//  --------            --------------              --------
//  28/06/2018          Shubham                     Initial Creation
// ======================================================

namespace GTechnology.Oncor.CustomAPI
{
    public enum ConnectivityHelper
    {
        InsideFeatures,LoadSideORDSFeatures,OutsideFeatures,ParallelFeatures,SharedLoads,SharedSources,SourceSideOrUSFeatures
    }
    public abstract class ConnectivityFactory
    {
        public abstract RelatedFeatureTypes GetHelper(ConnectivityHelper connHelper);

    }

    public class ConnectivityHelperFactory : ConnectivityFactory
    {
        public override RelatedFeatureTypes GetHelper(ConnectivityHelper p_helper)
        {
            RelatedFeatureTypes oConnHelper = null;

            switch (p_helper)
            {
                case ConnectivityHelper.InsideFeatures:
                    oConnHelper = new InsideFeatures();
                    break;
                case ConnectivityHelper.LoadSideORDSFeatures:
                    oConnHelper = new LoadSideOrDSFeatures();
                    break;
                case ConnectivityHelper.OutsideFeatures:
                    oConnHelper = new OutsideFeatures();
                    break;
                case ConnectivityHelper.ParallelFeatures:
                    oConnHelper = new ParallelFeatures();
                    break;
                case ConnectivityHelper.SharedLoads:
                    oConnHelper = new SharedLoads();
                    break;
                case ConnectivityHelper.SharedSources:
                    oConnHelper = new SharedSources();
                    break;
                case ConnectivityHelper.SourceSideOrUSFeatures:
                    oConnHelper = new SourceSideOrUSFeatures();
                    break;               
                default:
                    break;
            }
            return oConnHelper;
        }
    }
}
