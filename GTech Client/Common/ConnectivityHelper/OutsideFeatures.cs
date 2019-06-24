// ===================================================
//  Copyright 2018 Hexagon
//  File Name: OutsideFeatures.cs
// 
// Description:   
//  This class is responsible to get all the Outside Features of an Afftected feature

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
    public class OutsideFeatures : RelatedFeatureTypes
    {
        public OutsideFeatures()
        {
        }

        private IGTKeyObject m_oAffectedObject = null;
        public override IGTKeyObject AffectedFeature { get { return m_oAffectedObject; } set { m_oAffectedObject = value; } }
       
        private List<IGTKeyObject> m_RelatedFeaturesList = null;
        private List<IGTKeyObject> m_DSRelatedFeaturesList = null;
        private List<IGTKeyObject> m_USRelatedFeaturesList = null;
        public override List<IGTKeyObject> RelatedFeaturesList { get {return  m_RelatedFeaturesList; } }

        /// <summary>
        /// Method to get all the Outside Features w.r.t.affected feature
        /// </summary>
        /// <returns></returns>
        public override List<IGTKeyObject> GetRelatedFeaturesForFeatureTypes()
        {
            try
            {
                m_RelatedFeaturesList = new List<IGTKeyObject>();
                RelatedFeatureTypes oSourceSideFeaturesObject = new SourceSideOrUSFeatures();
                oSourceSideFeaturesObject.AffectedFeature = AffectedFeature;
                List<IGTKeyObject> oSourceSideRelatedFeatures = oSourceSideFeaturesObject.GetRelatedFeaturesForFeatureTypes();
                m_USRelatedFeaturesList = oSourceSideRelatedFeatures;

                RelatedFeatureTypes oLoadSideFeaturesObject = new LoadSideOrDSFeatures();
                oLoadSideFeaturesObject.AffectedFeature = AffectedFeature;
                List<IGTKeyObject> oLoadSideRelatedFeatures = oLoadSideFeaturesObject.GetRelatedFeaturesForFeatureTypes();
                m_DSRelatedFeaturesList = oLoadSideRelatedFeatures;

                m_RelatedFeaturesList = oSourceSideRelatedFeatures.Union(oLoadSideRelatedFeatures, new KeyObjectComparer()).ToList<IGTKeyObject>();

                return m_RelatedFeaturesList;
            }
            catch (Exception)
            {

                throw;
            }         
        }

        /// <summary>
        /// Method to get all Outside Phases w.r.t. Affected Feature
        /// </summary>
        /// <returns></returns>
        public override List<char> GetRelatedPhases()
        {
            try
            {
                if (m_RelatedFeaturesList == null) GetRelatedFeaturesForFeatureTypes();
                List<char> USPhases = new List<char>();
                List<char> DSPhases = new List<char>();
                List<char> OutsidePhases = new List<char>();
                string sPhaseAlpha = null;
                int iCount = 0;

                foreach (IGTKeyObject item in m_USRelatedFeaturesList)
                {
                    if (item.Components.GetComponent(ConnectivityCNO).Recordset != null)
                    {
                        iCount = iCount + 1;
                        item.Components.GetComponent(ConnectivityCNO).Recordset.MoveFirst();
                        sPhaseAlpha = Convert.ToString(item.Components.GetComponent(ConnectivityCNO).Recordset.Fields[ATTRIB_PHASE].Value);

                        if (iCount == 1)
                        {
                            USPhases.AddRange(sPhaseAlpha.ToCharArray());
                            USPhases.RemoveAll(P => P == 'N');
                        }
                        else
                        {
                            List<Char> lInterMediate = sPhaseAlpha.ToCharArray().ToList<char>();
                            lInterMediate.RemoveAll(p => p == 'N');
                            USPhases = USPhases.Union(lInterMediate).ToList<char>();
                        }
                    }
                }

                iCount = 0;

                foreach (IGTKeyObject item in m_DSRelatedFeaturesList)
                {
                    if (item.Components.GetComponent(ConnectivityCNO).Recordset != null)
                    {
                        iCount = iCount + 1;
                        item.Components.GetComponent(ConnectivityCNO).Recordset.MoveFirst();
                        sPhaseAlpha = Convert.ToString(item.Components.GetComponent(ConnectivityCNO).Recordset.Fields[ATTRIB_PHASE].Value);

                        if (iCount == 1)
                        {
                            DSPhases.AddRange(sPhaseAlpha.ToCharArray());
                            DSPhases.RemoveAll(P => P == 'N');
                        }
                        else
                        {
                            List<Char> lInterMediate = sPhaseAlpha.ToCharArray().ToList<char>();
                            lInterMediate.RemoveAll(p => p == 'N');
                            DSPhases = DSPhases.Union(lInterMediate).ToList<char>();
                        }
                    }
                }

                OutsidePhases = USPhases.Intersect(DSPhases).ToList<char>();

                return OutsidePhases;
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
