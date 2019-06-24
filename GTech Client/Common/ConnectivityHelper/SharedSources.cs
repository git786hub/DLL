// ===================================================
//  Copyright 2018 Hexagon
//  File Name: SharedSources.cs
// 
// Description:   
//  This class is responsible to get all the Shared Sources of an Afftected feature

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
using Intergraph.GTechnology.API;

namespace GTechnology.Oncor.CustomAPI
{
    public class SharedSources : RelatedFeatureTypes
    {
        private IGTKeyObject m_oAffectedObject = null;
        public override IGTKeyObject AffectedFeature { get { return m_oAffectedObject; } set { m_oAffectedObject = value; } }
      
        public override List<IGTKeyObject> RelatedFeaturesList { get => throw new NotImplementedException();  }

        /// <summary>
        /// Method to get all the Shared Sources w.r.t.affected feature
        /// </summary>
        /// <returns></returns>
        public override List<IGTKeyObject> GetRelatedFeaturesForFeatureTypes()
        {
            try
            {
                List<IGTKeyObject> SharedSources = new List<IGTKeyObject>();
                FeatureConnectivityAttributes oAffectedConnectivity = GetConnectivityAttributes(AffectedFeature);
                FeatureConnectivityAttributes oRelatedConnectivity = null;

                NodeType Node = oAffectedConnectivity.UpstreamNode == 1 ? NodeType.Node1 : NodeType.Node2;

                List<IGTKeyObject> oRelated = GetAllRelatedFeatures(Node);

                foreach (IGTKeyObject item in oRelated)
                {
                    oRelatedConnectivity = GetConnectivityAttributes(item);

                    //

                   // oRelatedConnectivity = GetConnectivityAttributes(item);

                    // If upstream node is not set, then it can't be a shared source.
                    if (oRelatedConnectivity.UpstreamNodeValue == 0) { continue; }

                    // If nodes are equal (i.e.- single-node feature) then treat as downstream, not shared load.
                    if (oRelatedConnectivity.DownStreamNodeValue == oRelatedConnectivity.UpstreamNodeValue) { continue; }

                    if (oRelatedConnectivity.UpstreamNodeValue == oAffectedConnectivity.UpstreamNodeValue
                        && oRelatedConnectivity.DownStreamNodeValue != oAffectedConnectivity.DownStreamNodeValue)
                    {
                        SharedSources.Add(item);
                    }
                    //


                    //if ((oRelatedConnectivity.UpstreamNodeValue == oAffectedConnectivity.UpstreamNodeValue && oRelatedConnectivity.UpstreamNodeValue != 0 && oAffectedConnectivity.UpstreamNodeValue != 0) && !((oRelatedConnectivity.UpstreamNodeValue == oAffectedConnectivity.UpstreamNodeValue && oRelatedConnectivity.UpstreamNodeValue != 0 && oAffectedConnectivity.UpstreamNodeValue != 0) &&
                    //    (oRelatedConnectivity.DownStreamNodeValue == oAffectedConnectivity.DownStreamNodeValue && oRelatedConnectivity.DownStreamNodeValue != 0 && oAffectedConnectivity.DownStreamNodeValue != 0)))
                    //{
                    //    SharedSources.Add(item);
                    //}
                }

                return SharedSources;
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
