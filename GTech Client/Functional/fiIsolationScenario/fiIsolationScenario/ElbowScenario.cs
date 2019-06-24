// ===================================================
//  Copyright 2017 Intergraph Corp.
//  File Name: ElbowScenario.cs
// 
//  Description:   Validates for active feature not connected to exactly two Elbows on the Node1.
//  Remarks: 
//  Modification History
//  Date                Author                      Comments
//  --------            --------------              --------
//  23/03/2018        Prathyusha                  Created 
// User: pnlella      Date: 08/03/19   Time: 15:15 Adjusted code for handling issues related to Isolation scenario.
// User: Akhilesh     Date: 19/03/19   Time: 15:35 Adjusted code for handling issues related to Detail Isolation Point Creation.
// User: Prathyusha   Date: 13/05/19   Time: 15:35 Adjusted code for handling issues reestablishment of Elbows disconnected and reconnected.
// ======================================================
using System;
using System.Collections.Generic;
using System.Linq;
using Intergraph.GTechnology.API;
using ADODB;
using System.Windows.Forms;

namespace GTechnology.Oncor.CustomAPI
{
    class ElbowScenario : IProcessIsolationScenario
    {
        #region Variables

        ValidationRuleManager validateMsg = null;
        List<IGTKeyObject> m_oNode1ElbowFeatures = null;
        List<IGTKeyObject> m_oNode2ElbowFeatures = null;
        IsoScenarioFeature m_IsolationScenarioFeature = null;
        IsoCommon m_IsoCommon = null;
        IGTDataContext m_dataContext = null;
        string m_errorPriority = null;

        #endregion

        #region Constructor
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="p_Node1ElbowFeatures">Node1 Elbows features connected to active feature</param>
        /// <param name="p_Node2ElbowFeatures">Node2 Elbows features connected to active feature</param>
        /// <param name="p_errorPriority">Error Priority</param>
        public ElbowScenario(List<IGTKeyObject> p_Node1ElbowFeatures, List<IGTKeyObject> p_Node2ElbowFeatures, string p_errorPriority)
        {
            validateMsg = new ValidationRuleManager();
            m_oNode1ElbowFeatures = p_Node1ElbowFeatures;
            m_oNode2ElbowFeatures = p_Node2ElbowFeatures;
            m_errorPriority = p_errorPriority;
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="dataContext">IGTDataContext object</param>
        /// <param name="isolationScenarioFeature">Isolation Scenario feature</param>
        /// <param name="isoCommon">IsoCommon object</param>
        public ElbowScenario(IGTDataContext dataContext, IsoScenarioFeature isolationScenarioFeature, IsoCommon isoCommon)
        {
            m_dataContext = dataContext;
            m_IsolationScenarioFeature = isolationScenarioFeature;
            m_IsoCommon = isoCommon;
        }
        #endregion

        #region Methods
        /// <summary>
        /// ProcessIsolationScenario
        /// </summary>
        /// <param name="ErrorMessage">Error Message to be displayed</param>
        /// <param name="ErrorPriority">Error Priority</param>
        public void ProcessIsolationScenario(out string ErrorMessage, out string ErrorPriority)
        {
            try
            {
                if (!CheckElbow(m_oNode1ElbowFeatures, m_oNode2ElbowFeatures))
                {
                    validateMsg.Rule_Id = "ISO03";
                    validateMsg.BuildRuleMessage(GTClassFactory.Create<IGTApplication>(), null);

                    ErrorMessage = validateMsg.Rule_MSG;
                    ErrorPriority = m_errorPriority;
                }
                else
                {
                    ErrorMessage = null;
                    ErrorPriority = null;
                }
            }
            catch
            {
                throw;
            }
            finally
            {
                validateMsg = null;
                m_oNode1ElbowFeatures = null;
                m_oNode2ElbowFeatures = null;
            }
        }

        /// <summary>
        /// Method to validate that the Isolation Scenario is configured correctly.
        /// If configured correctly then no action.
        /// Else, create virtual points and/or establish connectivity as necessary.
        /// </summary>
        public void ValidateIsolationScenario()
        {
            Dictionary<int, GTRelationshipOrdinalConstants> dicRelatedNodes = new Dictionary<int, GTRelationshipOrdinalConstants>();
            try
            {
                bool isoPtExists = false;
                // Check if Isolation Scenario is valid on Node1.
                if (ValidateIsolationPoint(m_IsolationScenarioFeature.RelatedFeaturesNode1, ref isoPtExists, 1))
                {
                    if (!isoPtExists)
                    {
                        // Create Isolation Point since one doesn't exist
                        CreateIsolationPoint(1);
						 m_IsoCommon.SetNormalAndAsOperatedStatus(m_IsolationScenarioFeature.IsolationPoint1, 1);
                    }

                    // Check if second virtual point exists.
                    if (ValidateIsolationPoint(m_IsolationScenarioFeature.RelatedFeaturesNode1, ref isoPtExists, 2))
                    {
                        if (!isoPtExists)
                        {
                            // Create Isolation Point since one doesn't exist
                            CreateIsolationPoint(2);
							m_IsoCommon.SetNormalAndAsOperatedStatus(m_IsolationScenarioFeature.IsolationPoint2, 2);
                        }

                        GTRelationshipOrdinalConstants relatedNode = GTRelationshipOrdinalConstants.gtrelRelationshipOrdinal1;

                        foreach (IGTKeyObject feature in m_IsolationScenarioFeature.RelatedFeaturesNode1)
                        {
                            if (feature.FID != m_IsolationScenarioFeature.IsolationPoint1.FID)
                            {
                                m_IsoCommon.DetermineConnectivityNode(m_IsolationScenarioFeature.GtKeyObject, feature, ref relatedNode);
                                dicRelatedNodes.Add(feature.FID, relatedNode);
                            }
                        }
                        using (IGTRelationshipService oRel = GTClassFactory.Create<IGTRelationshipService>())
                        {
                            oRel.DataContext = m_dataContext;
                            oRel.ActiveFeature = m_IsolationScenarioFeature.GtKeyObject;
                            oRel.SilentDelete(14, GTRelationshipOrdinalConstants.gtrelRelationshipOrdinal1);
                        }
                        bool Elbow1Connected = false;

                        // If features related to Isolation Scenario feature are connected directly to Node 1 of Isolation Scenario feature, then
                        // reconnect features to Isolation Point features.
                        foreach (IGTKeyObject feature in m_IsolationScenarioFeature.RelatedFeaturesNode1)
                        {
                            // Skip the processing for the Isolation Point feature since the Isolation Point was already validated/updated.
                            if (feature.FID != m_IsolationScenarioFeature.IsolationPoint1.FID && feature.FID != m_IsolationScenarioFeature.IsolationPoint2.FID && dicRelatedNodes.TryGetValue(feature.FID, out relatedNode))
                            {
                                // Since both Elbows are connected to Node 1 of the Isolation Scenario feature,
                                // we need to reconnect the first related feature to Elbow 1 and the rest of the related features to Elbow 2.
                                if (!Elbow1Connected)
                                {
                                    //m_IsoCommon.DetermineConnectivityNode(m_IsolationScenarioFeature.GtKeyObject, feature, ref relatedNode);
                                    m_IsoCommon.EstablishConnectivity(m_IsolationScenarioFeature.IsolationPoint1, feature, GTRelationshipOrdinalConstants.gtrelRelationshipOrdinal1, relatedNode);
                                    Elbow1Connected = true;
                                }
                                else
                                {
                                    //m_IsoCommon.DetermineConnectivityNode(m_IsolationScenarioFeature.GtKeyObject, feature, ref relatedNode);
                                    m_IsoCommon.EstablishConnectivity(m_IsolationScenarioFeature.IsolationPoint2, feature, GTRelationshipOrdinalConstants.gtrelRelationshipOrdinal2, relatedNode);
                                }
                            }
                        }

                        m_IsoCommon.EstablishConnectivity(m_IsolationScenarioFeature.IsolationPoint1, m_IsolationScenarioFeature.GtKeyObject, GTRelationshipOrdinalConstants.gtrelRelationshipOrdinal2, GTRelationshipOrdinalConstants.gtrelRelationshipOrdinal1);
                        m_IsoCommon.EstablishConnectivity(m_IsolationScenarioFeature.IsolationPoint2, m_IsolationScenarioFeature.GtKeyObject, GTRelationshipOrdinalConstants.gtrelRelationshipOrdinal1, GTRelationshipOrdinalConstants.gtrelRelationshipOrdinal1);

                        m_IsoCommon.EstablishOwnerShip(m_IsolationScenarioFeature.GtKeyObject, m_IsolationScenarioFeature.IsolationPoint1);
                        m_IsoCommon.EstablishOwnerShip(m_IsolationScenarioFeature.GtKeyObject, m_IsolationScenarioFeature.IsolationPoint2);

                        m_IsoCommon.PopulateProtectionDeviceIDForIsoPt(m_IsolationScenarioFeature.GtKeyObject, m_IsolationScenarioFeature.IsolationPoint1);
                        m_IsoCommon.PopulateProtectionDeviceIDForIsoPt(m_IsolationScenarioFeature.GtKeyObject, m_IsolationScenarioFeature.IsolationPoint2);

                    }
                }
            }
            catch (Exception ex)
            {
                if (m_IsoCommon.InteractiveMode)
                {
                    MessageBox.Show("Error in Isolation Scenario FI:ValidateIsolationScenario - " + ex.Message, "G/Technology", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }
            }
            finally
            {
                dicRelatedNodes.Clear();
                dicRelatedNodes = null;
            }
        }

        /// <summary>
        /// Method to check whether the active feature has Two Elbows on the Node1
        /// </summary>
        /// <param name="p_Node1ElbowFeatures">Node1 Elbows features connected to active feature</param>
        /// <param name="p_Node2ElbowFeatures">Node2 Elbows features connected to active feature</param>
        /// <returns></returns>
        private bool CheckElbow(List<IGTKeyObject> p_Node1ElbowFeatures, List<IGTKeyObject> p_Node2ElbowFeatures)
        {
            bool isElbow = false;
            try
            {
                if (p_Node1ElbowFeatures.Count() == 2 && p_Node2ElbowFeatures.Count() == 0)
                {
                    isElbow = true;
                }
            }
            catch
            {
                throw;
            }
            return isElbow;
        }

        /// <summary>
        /// Method to check whether the active feature has Isolation Point on specific node.
        /// If Isolation Point exists then validate association and update if necessary.
        /// </summary>
        /// <param name="relatedFeatures">Features to check</param>
        /// <param name="isoPtExists">True, if isolation point exists</param>
        /// <param name="isoPtNumber">Isolation point number</param>
        /// <returns>Boolean indicating method execution status</returns>
        private bool ValidateIsolationPoint(IGTKeyObjects relatedFeatures, ref bool isoPtExists, short isoPtNumber)
        {
            bool returnValue = false;

            try
            {
                isoPtExists = false;
                
                foreach (IGTKeyObject feature in relatedFeatures)
                {
                    // Check if Isolation Point feature exists
                    if ((feature.FNO == 41 || feature.FNO == 81) && m_IsoCommon.CheckAssociatedVirtualPoint(feature, m_IsolationScenarioFeature.GtKeyObject.FID))
                    {
                        if (isoPtNumber == 1)
                        {
                            isoPtExists = true;
                            m_IsolationScenarioFeature.IsolationPoint1 = feature;
                        }
                        else
                        {
                            if (feature.FID != m_IsolationScenarioFeature.IsolationPoint1.FID)
                            {
                                isoPtExists = true;
                                m_IsolationScenarioFeature.IsolationPoint2 = feature;
                            }
                            else
                            {
                                continue;
                            }
                        }

                        // Validate and set, if necessary, specific Isolation Point attributes to associated feature
                        string errMessage;
                        m_IsoCommon.SetVirtualPointAttributes(m_IsolationScenarioFeature, feature, out errMessage);
                        break;
                    }
                }
                if (relatedFeatures.Count == 0)
                {
                    IGTKeyObject gTKeyObject = CheckforVirtualpoint(isoPtNumber);

                    if (gTKeyObject != null)
                    {

                        isoPtExists = true;

                        if (isoPtNumber == 1)
                        {
                            m_IsolationScenarioFeature.IsolationPoint1 = gTKeyObject;
                        }
                        else
                        {
                            m_IsolationScenarioFeature.IsolationPoint2 = gTKeyObject;
                        }

                        // Validate and set, if necessary, specific Isolation Point attributes to associated feature
                        string errMessage;
                        m_IsoCommon.SetVirtualPointAttributes(m_IsolationScenarioFeature, gTKeyObject, out errMessage);
                    }
                }

                returnValue = true;
            }
            catch (Exception ex)
            {
                returnValue = false;
                if (m_IsoCommon.InteractiveMode)
                {
                    MessageBox.Show("Error in Isolation Scenario FI:ValidateIsolationPoint - " + ex.Message, "G/Technology", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }
            }
            return returnValue;
        }      
        private IGTKeyObject CheckforVirtualpoint(short isoPtNumber)
        {
            IGTKeyObject feature = null;
            bool exist = false;
            try
            {
                String sql = "select G3E_FID,G3E_FNO from VIRTUALPT_N where ASSOCIATED_FID = ?";

                Recordset virtualRS = m_dataContext.OpenRecordset(sql, CursorTypeEnum.adOpenStatic, LockTypeEnum.adLockReadOnly, (int)CommandTypeEnum.adCmdText,
                                                               m_IsolationScenarioFeature.GtKeyObject.FID);
                if (virtualRS != null && virtualRS.RecordCount > 0)
                {
                    virtualRS.MoveFirst();

                    while (!virtualRS.EOF)
                    {
                        feature = m_dataContext.OpenFeature(Convert.ToInt16(virtualRS.Fields["G3E_FNO"].Value), Convert.ToInt32(virtualRS.Fields["G3E_FID"].Value));

                        if (isoPtNumber == 1)
                        {
                             if (feature.FNO == 41 || feature.FNO == 81)
                             {
                                    exist = true;
                                    break;
                             }
                        }
                        else
                        {
                            if (feature.FID != m_IsolationScenarioFeature.IsolationPoint1.FID && (feature.FNO == 41 || feature.FNO == 81))
                            {
                                exist = true;
                                break;
                            }
                        }
                        if (exist)
                        {
                            break;
                        }
                        virtualRS.MoveNext();
                    }
                }
            }
            catch
            {
                throw;
            }
            return feature;
        }
        /// <summary>
        /// Method to create an Isolation Point.
        /// </summary>
        /// <param name="isoPtNumber">Isolation point number</param>
        /// <returns>Boolean indicating method execution status</returns>
        private bool CreateIsolationPoint(short isoPtNumber)
        {
            bool returnValue = false;
            short primaryCNO = 0;
            short isoPtCNO = 0;
            try
            {
                IGTKeyObject isoPtKO = m_dataContext.NewFeature(41);

                string errMessage;
                m_IsoCommon.SetAttributeDefaults(isoPtKO, out errMessage);
                m_IsoCommon.SetVirtualPointAttributes(m_IsolationScenarioFeature, isoPtKO, out errMessage);

                Recordset oRSFeature = m_dataContext.MetadataRecordset("G3E_FEATURES_OPTABLE", "g3e_fno = " + m_IsolationScenarioFeature.GtKeyObject.FNO);
                m_IsoCommon.IsDetailMapWindow();
                if (m_IsoCommon.detailID == 0)
                {
                    primaryCNO = Convert.ToInt16(oRSFeature.Fields["G3E_PRIMARYGEOGRAPHICCNO"].Value);
                    isoPtCNO = 78;
                }
                else
                {
                    primaryCNO = Convert.ToInt16(oRSFeature.Fields["G3E_PRIMARYDETAILCNO"].Value);
                    isoPtCNO = 79;
                }

                // Check if symbol component has been created. If metadata is defined with alternate required component then symbol record will need to be added.
                if (isoPtKO.Components.GetComponent(isoPtCNO).Recordset.RecordCount == 0)
                {
                    isoPtKO.Components.GetComponent(isoPtCNO).Recordset.AddNew();
                    isoPtKO.Components.GetComponent(isoPtCNO).Recordset.Fields["G3E_FNO"].Value = isoPtKO.FNO;
                    isoPtKO.Components.GetComponent(isoPtCNO).Recordset.Fields["G3E_FID"].Value = isoPtKO.FID;
                    if (m_IsoCommon.detailID != 0)
                    {
                        isoPtKO.Components.GetComponent(isoPtCNO).Recordset.Fields["G3E_DETAILID"].Value = m_IsoCommon.detailID;
                    }
                    isoPtKO.Components.GetComponent(isoPtCNO).Recordset.MoveFirst();
                }

                // Set the geometry
                IGTPoint gtPt = GTClassFactory.Create<IGTPoint>();
                IGTOrientedPointGeometry orientedPtGeom = GTClassFactory.Create<IGTOrientedPointGeometry>();

                orientedPtGeom = (IGTOrientedPointGeometry)m_IsolationScenarioFeature.GtKeyObject.Components.GetComponent(primaryCNO).Geometry;

                double xOffset = 0;
                double yOffset = 0;

                if (m_IsolationScenarioFeature.GtKeyObject.FNO == 5)
                {
                    if (isoPtNumber == 1)
                    {
                        xOffset = IsoCommon.ENCLOSURE_ISO_PT1_OFFSET_X;
                        yOffset = IsoCommon.ENCLOSURE_ISO_PT1_OFFSET_Y;
                    }
                    else
                    {
                        xOffset = IsoCommon.ENCLOSURE_ISO_PT2_OFFSET_X;
                        yOffset = IsoCommon.ENCLOSURE_ISO_PT2_OFFSET_Y;
                    }
                }
                else
                {
                    if (isoPtNumber == 1)
                    {
                        xOffset = IsoCommon.XFMR_LOOP_ISO_PT1_OFFSET_X;
                        yOffset = IsoCommon.XFMR_LOOP_ISO_PT1_OFFSET_Y;
                    }
                    else
                    {
                        xOffset = IsoCommon.XFMR_LOOP_ISO_PT2_OFFSET_X;
                        yOffset = IsoCommon.XFMR_LOOP_ISO_PT2_OFFSET_Y;
                    }
                }

                gtPt = m_IsoCommon.GetOffsetPoint(orientedPtGeom.Origin, xOffset * .3048, yOffset * .3048, orientedPtGeom.Orientation);

                orientedPtGeom.Origin = gtPt;

                isoPtKO.Components.GetComponent(isoPtCNO).Geometry = orientedPtGeom;

                if (isoPtNumber == 1)
                {
                   // m_IsoCommon.EstablishConnectivity(isoPtKO, m_IsolationScenarioFeature.GtKeyObject, GTRelationshipOrdinalConstants.gtrelRelationshipOrdinal2, GTRelationshipOrdinalConstants.gtrelRelationshipOrdinal1);
                    m_IsolationScenarioFeature.RelatedFeaturesNode1.Add(isoPtKO);
                    m_IsolationScenarioFeature.IsolationPoint1 = isoPtKO;
                }
                else
                {
                   // m_IsoCommon.EstablishConnectivity(isoPtKO, m_IsolationScenarioFeature.GtKeyObject, GTRelationshipOrdinalConstants.gtrelRelationshipOrdinal1, GTRelationshipOrdinalConstants.gtrelRelationshipOrdinal1);
                    m_IsolationScenarioFeature.RelatedFeaturesNode1.Add(isoPtKO);
                    m_IsolationScenarioFeature.IsolationPoint2 = isoPtKO;
                }

                returnValue = true;
            }
            catch (Exception ex)
            {
                returnValue = false;
                if (m_IsoCommon.InteractiveMode)
                {
                    MessageBox.Show("Error in Isolation Scenario FI:CreateIsolationPoint - " + ex.Message, "G/Technology", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }
            }
            return returnValue;
        }
        #endregion
    }
}
