// ===================================================
//  Copyright 2018 Hexagon
//  File Name: SourceSideOrUSFeatures.cs
// 
// Description:   
//  This class is responsible to get all the Source Side Features of an Afftected feature

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
    public class SourceSideOrUSFeatures : RelatedFeatureTypes
    {
        private IGTKeyObject m_oAffectedObject = null;
        public override IGTKeyObject AffectedFeature { get { return m_oAffectedObject; } set { m_oAffectedObject = value; } }
     
        public override List<IGTKeyObject> RelatedFeaturesList => throw new NotImplementedException();
        /// <summary>
        /// Method to get all the Source Side or US Features w.r.t.affected feature
        /// </summary>
        /// <returns></returns>
        public override List<IGTKeyObject> GetRelatedFeaturesForFeatureTypes()
        {
            try
            {
                FeatureConnectivityAttributes o = GetConnectivityAttributes(AffectedFeature);
                NodeType Node = o.UpstreamNode == 1 ? NodeType.Node1 : NodeType.Node2;

                List<IGTKeyObject> oAllRelatedUpstream = null;

                oAllRelatedUpstream = GetAllRelatedFeatures(Node);

                RelatedFeatureTypes oParallelFeaturesObject = new ParallelFeatures();
                oParallelFeaturesObject.AffectedFeature = AffectedFeature;

                List<IGTKeyObject> oParallelRelatedFeatures = oParallelFeaturesObject.GetRelatedFeaturesForFeatureTypes();
                oAllRelatedUpstream = oAllRelatedUpstream.Except(oParallelRelatedFeatures, new KeyObjectComparer()).ToList<IGTKeyObject>();

                RelatedFeatureTypes oSharedSourceFeaturesObject = new SharedSources();
                oSharedSourceFeaturesObject.AffectedFeature = AffectedFeature;
                List<IGTKeyObject> oSharedSourceRelatedFeatures = oSharedSourceFeaturesObject.GetRelatedFeaturesForFeatureTypes();

                oAllRelatedUpstream = oAllRelatedUpstream.Except(oSharedSourceRelatedFeatures, new KeyObjectComparer()).ToList<IGTKeyObject>();

                return oAllRelatedUpstream;
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
