// ===================================================
//  Copyright 2018 Intergraph Corp.
//  File Name: Common.cs 
//  Modification History
//  Date                Author                      Comments
//  --------            --------------              --------
// User: pnlella     Date: 03/05/19   Time: 11:00  Desc : Created
// ======================================================
using Intergraph.GTechnology.API;
using Intergraph.GTechnology.Interfaces;
using ADODB;
using System;

namespace GTechnology.Oncor.CustomAPI
{
    public class Common
    {
        RGIBaseClass m_rgiBase = null;
        short m_activeFNO = 0;
        bool m_validate = false;

        public Common(RGIBaseClass rGIBaseClass)
        {
           m_rgiBase = rGIBaseClass;
        }

        public short ActiveFNO
        {
            get
            {
                return m_activeFNO;
            }
            set
            {
                m_activeFNO = value;
            }
        }
        public bool Validation
        {
            get
            {
                return m_validate;
            }
            set
            {
                m_validate = value;
            }
        }

        public class FeatureInfo
        {
            public bool IsValid { get; set; }

            public int FID { get; set; }
            public short FNO { get; set; }
            public string FeatureState { get; set; }

            public FeatureInfo(ADODB.Recordset p_rs)
            {
                this.IsValid = false;
                if (p_rs == null || (p_rs.BOF && p_rs.EOF)) return;

                p_rs.MoveFirst();

                this.FID = Convert.ToInt32(p_rs.Fields["G3E_FID"].Value); ;
                this.FNO = Convert.ToInt16(p_rs.Fields["G3E_FNO"].Value);
                this.FeatureState = p_rs.Fields["FEATURE_STATE_C"].Value.ToString();

                this.IsValid = true;
            }

        }

        /// <summary>
        /// Method to get related features
        /// <summary>
        public IGTKeyObjects GetRelatedFeatures()
        {
            try
            {
                IGTKeyObjects configuredKeyObjects = GTClassFactory.Create<IGTKeyObjects>();

                using (IGTRelationshipService relService = GTClassFactory.Create<IGTRelationshipService>())
                {
                    relService.DataContext = m_rgiBase.DataContext;
                    relService.ActiveFeature = m_rgiBase.ActiveFeature;
                    IGTKeyObjects relatedFeatures = relService.GetRelatedFeatures(m_rgiBase.RNO);

                    if (Validation)
                    {
                        foreach (IGTKeyObject item in relatedFeatures)
                        {
                            if (((ActiveFNO == 16 || ActiveFNO == 91) && (item.FNO == 16 || item.FNO == 17 || item.FNO == 18 || item.FNO == 91)) 
                                || ((ActiveFNO == 17 || ActiveFNO == 18) && (item.FNO == 16 || item.FNO == 91)))
                            {
                                continue;
                            }
                            else
                            {
                                configuredKeyObjects.Add(item);
                            }
                        }
                    }
                    else
                    {
                        configuredKeyObjects = relatedFeatures;
                    }
                    return configuredKeyObjects;
                }
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Method to get active related features nodes
        /// <summary>
        public void GetActiveRelatedNodes(Recordset p_activeFeatureRS, Recordset p_relatedFeatureRS, ref short activeNode,ref short relatedNode)
        {
            try
            {

                if (p_activeFeatureRS != null && p_relatedFeatureRS != null)
                {
                    if (!(p_activeFeatureRS.EOF && p_activeFeatureRS.BOF) && !(p_relatedFeatureRS.EOF && p_relatedFeatureRS.BOF))
                    {
                        p_activeFeatureRS.MoveFirst();
                        p_relatedFeatureRS.MoveFirst();

                        if (CheckAffectedRelatedNodeVoltages("NODE_1_ID", "NODE_1_ID", p_activeFeatureRS,p_relatedFeatureRS))
                        {
                            activeNode = 1;
                            relatedNode = 1;
                        }
                        else if (CheckAffectedRelatedNodeVoltages("NODE_1_ID", "NODE_2_ID", p_activeFeatureRS,p_relatedFeatureRS))
                        {
                            activeNode = 1;
                            relatedNode = 2;
                        }
                        else if (CheckAffectedRelatedNodeVoltages("NODE_2_ID", "NODE_1_ID", p_activeFeatureRS,p_relatedFeatureRS))
                        {
                            activeNode = 2;
                            relatedNode = 1;
                        }
                        else if (CheckAffectedRelatedNodeVoltages("NODE_2_ID", "NODE_2_ID", p_activeFeatureRS,p_relatedFeatureRS))
                        {
                            activeNode = 2;
                            relatedNode = 2;
                        }
                    }
                }
            }
            catch
            {
                throw;
            }
        }
        
        /// <summary>
        /// Method to set the values from Base class
        /// <summary>
        public void SetValues()
        {
            m_activeFNO = m_rgiBase.ActiveFeature.FNO;

            if(m_rgiBase.ProcessingMode.Equals(GTRelationshipGeometryProcessingModeConstants.gtrgiValidation))
            {
                m_validate = true;
            }
        }

        /// <summary>
        /// Method to check which node of active and related features are connected.
        /// <summary>
        private bool CheckAffectedRelatedNodeVoltages(string activeNode, string relatedNode,Recordset p_activeFeatureRS, Recordset p_relatedFeatureRS)
        {
            bool connected = false;
            try
            {
                if ((Convert.ToInt32(p_activeFeatureRS.Fields[activeNode].Value) != 0 || Convert.ToInt32(p_relatedFeatureRS.Fields[relatedNode].Value) != 0) && p_activeFeatureRS.Fields[activeNode].Value != DBNull.Value && (p_activeFeatureRS.Fields[activeNode].Value.Equals(p_relatedFeatureRS.Fields[relatedNode].Value)))
                {
                    connected = true;
                }
            }
            catch
            {
                throw;
            }
            return connected;
        }

        /// <summary>
        /// Method to get the feature state of the feature.
        /// <summary>
        public void GetFeatureState(Recordset p_relatedRecordset,ref string featureState, bool p_active)
        {
            try
            {
                if (p_active)
                {
                    Recordset commonRS = m_rgiBase.ActiveFeature.Components.GetComponent(1).Recordset;

                    if (commonRS != null && commonRS.RecordCount > 0)
                    {
                        commonRS.MoveFirst();

                        featureState = Convert.ToString(commonRS.Fields["FEATURE_STATE_C"].Value);
                    }
                }
                else
                {
                    p_relatedRecordset.MoveFirst();
                    featureState = Convert.ToString(p_relatedRecordset.Fields["FEATURE_STATE_C"].Value);
                }
            }
            catch
            {
                throw;
            }
        }
    }
}
