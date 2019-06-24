// ===================================================
//  Copyright 2018 Hexagon
//  File Name: LoadSideOrDSFeatures.cs
// 
// Description:   
//  This class is responsible to get all the Load Side or Upstream Features of an Afftected feature

//
//  Remarks:
// 
//  Modification History
//  Date                Author                      Comments
//  --------            --------------              --------
//  28/06/2018          Shubham                     Initial Creation
// ======================================================
using System;
using System.Collections.Generic;
using System.Linq;
using Intergraph.GTechnology.API;

namespace GTechnology.Oncor.CustomAPI
{
   public class LoadSideOrDSFeatures : RelatedFeatureTypes
    {
        private IGTKeyObject m_oAffectedObject = null;
        public override IGTKeyObject AffectedFeature { get { return m_oAffectedObject; } set { m_oAffectedObject = value; } }
    
        public override List<IGTKeyObject> RelatedFeaturesList { get => throw new NotImplementedException();}

        /// <summary>
        /// Method to get all the Load Side or Upstream Features w.r.t.affected feature
        /// </summary>
        /// <returns></returns>
        public override List<IGTKeyObject> GetRelatedFeaturesForFeatureTypes()
        {
            try
            {
                FeatureConnectivityAttributes o = GetConnectivityAttributes(AffectedFeature);
                NodeType Node = o.UpstreamNode == 1 ? NodeType.Node2 : NodeType.Node1;

                List<IGTKeyObject> oAllRelatedDownstream = null;

                oAllRelatedDownstream = GetAllRelatedFeatures(Node);

                RelatedFeatureTypes oParallelFeaturesObject = new ParallelFeatures();
                oParallelFeaturesObject.AffectedFeature = AffectedFeature;
                List<IGTKeyObject> oParallelRelatedFeatures = oParallelFeaturesObject.GetRelatedFeaturesForFeatureTypes();


                oAllRelatedDownstream = oAllRelatedDownstream.Except(oParallelRelatedFeatures, new KeyObjectComparer()).ToList<IGTKeyObject>().ToList<IGTKeyObject>();

                RelatedFeatureTypes oSharedLoadsObject = new SharedLoads();
                oSharedLoadsObject.AffectedFeature = AffectedFeature;
                List<IGTKeyObject> oSharedLoadRelatedFeatures = oSharedLoadsObject.GetRelatedFeaturesForFeatureTypes();

                oAllRelatedDownstream = oAllRelatedDownstream.Except(oSharedLoadRelatedFeatures, new KeyObjectComparer()).ToList<IGTKeyObject>();

                return oAllRelatedDownstream;
            }
            catch (Exception)
            {

                throw;
            }
           
        }

        public override List<char> GetRelatedPhases()
        {
            throw new NotImplementedException();
        }

        public override bool IsAffectedDownstreamOfRelated(IGTKeyObject p_relatedObject)
        {
            throw new NotImplementedException();
        }
    }
}
