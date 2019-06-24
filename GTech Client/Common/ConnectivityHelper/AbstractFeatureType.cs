// ===================================================
//  Copyright 2018 Hexagon
//  File Name: InsideFeatures.cs
// 
// Description:   
//  This class is responsible to get all the Inside Features of an Afftected feature

//
//  Remarks:
// 
//  Modification History
//  Date                Author                      Comments
//  --------            --------------              --------
//  28/06/2018          Shubham                     Initial Creation
// ======================================================
using System.Collections.Generic;
using Intergraph.GTechnology.API;

namespace GTechnology.Oncor.CustomAPI
{
    public abstract class RelatedFeatureTypes
    {
        public enum NodeType
        {
            Node1, Node2, Both
        }

        public const int ConnectivityCNO = 11;
        public const int OwnershipCNO = 3;

        public const string ATTRIB_NODE1 = "NODE_1_ID";
        public const string ATTRIB_NODE2 = "NODE_2_ID";
        public const string ATTRIB_PHASE = "PHASE_ALPHA";
        public const int RNOConnectivity =14;

        public abstract List<IGTKeyObject>  RelatedFeaturesList { get; }
        public abstract IGTKeyObject AffectedFeature { get; set; }

        public abstract List<IGTKeyObject> GetRelatedFeaturesForFeatureTypes();
        public abstract List<char> GetRelatedPhases();

        public abstract bool IsAffectedDownstreamOfRelated(IGTKeyObject p_relatedObject);

        internal FeatureConnectivityAttributes GetConnectivityAttributes(IGTKeyObject p_KeyObjectFeature)
        {
            FeatureConnectivityAttributes oConnectivityAttr = new FeatureConnectivityAttributes(p_KeyObjectFeature);
            return oConnectivityAttr;
        }       
        internal List<IGTKeyObject> GetAllRelatedFeatures(NodeType p_NodeType)
        {
            IGTKeyObjects oRelatedObjects = GTClassFactory.Create<IGTKeyObjects>();
            IGTApplication oApp = GTClassFactory.Create<IGTApplication>();

            using (IGTRelationshipService oRelationshipObj = GTClassFactory.Create<IGTRelationshipService>())
            {
                oRelationshipObj.DataContext = oApp.DataContext;
                oRelationshipObj.ActiveFeature = AffectedFeature;

                List<IGTKeyObject> RelatedFeaturesList = new List<IGTKeyObject>();
                switch (p_NodeType)
                {
                    case NodeType.Node1:
                        try
                        {
                            oRelatedObjects = oRelationshipObj.GetRelatedFeatures(RNOConnectivity, GTRelationshipOrdinalConstants.gtrelRelationshipOrdinal1);
                        }
                        catch (System.Exception)
                        {

                        }

                        break;
                    case NodeType.Node2:
                        try
                        {
                            oRelatedObjects = oRelationshipObj.GetRelatedFeatures(RNOConnectivity, GTRelationshipOrdinalConstants.gtrelRelationshipOrdinal2);
                        }
                        catch (System.Exception)
                        {

                        }

                        break;
                    case NodeType.Both:
                        oRelatedObjects = oRelationshipObj.GetRelatedFeatures(RNOConnectivity, GTRelationshipOrdinalConstants.gtrelRelationshipOrdinalAll);
                        break;
                    default:
                        break;
                }

                foreach (IGTKeyObject item in oRelatedObjects)
                {
                    RelatedFeaturesList.Add(item);
                }

                return RelatedFeaturesList;
            }
        }
    }
}
