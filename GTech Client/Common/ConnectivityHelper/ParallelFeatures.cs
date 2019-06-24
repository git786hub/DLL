// ===================================================
//  Copyright 2018 Hexagon
//  File Name: ParallelFeatures.cs
// 
// Description:   
//  This class is responsible to get all the PArallel Features of an Afftected feature

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
    public class ParallelFeatures : RelatedFeatureTypes
    {
        private IGTKeyObject m_oAffectedObject = null;
        public override IGTKeyObject AffectedFeature { get { return m_oAffectedObject; } set { m_oAffectedObject = value; } }
    
        public override List<IGTKeyObject> RelatedFeaturesList { get => throw new NotImplementedException(); }

        /// <summary>
        /// Method to get all the Parallel Features w.r.t.affected feature
        /// </summary>
        /// <returns></returns>
        public override List<IGTKeyObject> GetRelatedFeaturesForFeatureTypes()
         {
            try
            {
                List<IGTKeyObject> oRelated = GetAllRelatedFeatures(NodeType.Both);
                //The parallel set of features would be those which are connected to each other at both the nodes U/S to U/S and D/S to D/S
                FeatureConnectivityAttributes oAffectedConnectivity = GetConnectivityAttributes(AffectedFeature);
                FeatureConnectivityAttributes oRelatedConnectivity = null;

                List<IGTKeyObject> ParallelObjects = new List<IGTKeyObject>();

                foreach (IGTKeyObject item in oRelated)
                {
                    oRelatedConnectivity = GetConnectivityAttributes(item);
                    if ((oRelatedConnectivity.UpstreamNodeValue == oAffectedConnectivity.UpstreamNodeValue && oRelatedConnectivity.UpstreamNodeValue != 0 && oAffectedConnectivity.UpstreamNodeValue != 0) &&
                        (oRelatedConnectivity.DownStreamNodeValue == oAffectedConnectivity.DownStreamNodeValue && oRelatedConnectivity.DownStreamNodeValue != 0 && oAffectedConnectivity.DownStreamNodeValue != 0))
                    {
                        ParallelObjects.Add(item);
                    }
                }

                return ParallelObjects;
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
