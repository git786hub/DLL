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

using System;
using System.Collections.Generic;
using System.Linq;
using Intergraph.GTechnology.API;

namespace GTechnology.Oncor.CustomAPI
{
    public class InsideFeatures : RelatedFeatureTypes
    {
        private IGTKeyObject m_oAffectedObject = null;
        public override IGTKeyObject AffectedFeature { get { return m_oAffectedObject; } set { m_oAffectedObject = value; } }
      
        private List<IGTKeyObject> m_RelatedFeaturesList = null;
        public override List<IGTKeyObject> RelatedFeaturesList { get { return m_RelatedFeaturesList; } }
      
        /// <summary>
        /// Method to get all the Inside Features w.r.t.affected feature
        /// </summary>
        /// <returns></returns>
        public override List<IGTKeyObject> GetRelatedFeaturesForFeatureTypes()
        {
            m_RelatedFeaturesList = new List<IGTKeyObject>();
            FeatureConnectivityAttributes o = GetConnectivityAttributes(AffectedFeature);
            NodeType Node = o.UpstreamNode == 1 ? NodeType.Node1 : NodeType.Node2;

            m_RelatedFeaturesList.Add(AffectedFeature);         

            RelatedFeatureTypes oParallelFeatures = new ParallelFeatures();
            oParallelFeatures.AffectedFeature = AffectedFeature;
            List<IGTKeyObject> oParallelRelatedFeatures = oParallelFeatures.GetRelatedFeaturesForFeatureTypes();

            m_RelatedFeaturesList = m_RelatedFeaturesList.Union(oParallelRelatedFeatures,new KeyObjectComparer()).ToList<IGTKeyObject>();

            return m_RelatedFeaturesList;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>

        public override List<char> GetRelatedPhases()
        {
            try
            {
                if (m_RelatedFeaturesList == null) GetRelatedFeaturesForFeatureTypes();
                List<char> InsidePhases = new List<char>();
                string sPhaseAlpha = null;

                foreach (IGTKeyObject item in m_RelatedFeaturesList)
                {
                    if (item.Components.GetComponent(ConnectivityCNO).Recordset != null)
                    {
                        item.Components.GetComponent(ConnectivityCNO).Recordset.MoveFirst();
                        sPhaseAlpha = Convert.ToString(item.Components.GetComponent(ConnectivityCNO).Recordset.Fields[ATTRIB_PHASE].Value);

                        InsidePhases = InsidePhases.Union(sPhaseAlpha.ToCharArray().ToList<char>()).ToList<char>();
                    }
                }
                InsidePhases.RemoveAll(p=>p=='N');
                return InsidePhases;
            }
            catch (Exception)
            {
                throw;
            }           
        }

        public override bool IsAffectedDownstreamOfRelated(IGTKeyObject p_relatedObject)
        {
            throw new NotImplementedException();
        }
    }
}
