// ===================================================
//  Copyright 2017 Intergraph Corp.
//  File Name: ByPassScenario.cs
// 
//  Description:   Validates for active feature either node not connected to exactly one ByPass Point
//                 or connected ByPass Points are not both connected to the same Bypass Point.
//  Remarks: 
//  Modification History
//  Date                Author                      Comments
//  --------            --------------              --------
//  23/03/2018        Prathyusha                  Created 
// User: pnlella      Date: 08/03/19   Time: 15:15 Adjusted code for handling issues related to Isolation scenario.
// User: Akhilesh     Date: 19/03/19   Time: 15:35 Adjusted code for handling issues related to Detail Isolation Point Creation.
// User: Prathyusha   Date: 13/05/19   Time: 15:35 Adjusted code for handling issues reestablishment of Bypasspoints disconnected and reconnected.
// ======================================================
using System;
using ADODB;
using System.Collections.Generic;
using System.Linq;
using Intergraph.GTechnology.API;
using System.Windows.Forms;

namespace GTechnology.Oncor.CustomAPI
{
    class ByPassScenario : IProcessIsolationScenario
    {
        #region Variables

        ValidationRuleManager validateMsg = null;
        List<IGTKeyObject> m_oNode1ByPassFeatures = null;
        List<IGTKeyObject> m_oNode2ByPassFeatures = null;
        IsoScenarioFeature m_IsolationScenarioFeature = null;
        IsoCommon m_IsoCommon = null;
        IGTDataContext m_dataContext = null;
        string m_errorPriority = null;

        #endregion

        #region Constructor

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="p_ByPassFeatures">Bypass Points features connected to active feature</param>
        /// <param name="p_ErrorPriority">Error Priority</param>
        public ByPassScenario(List<IGTKeyObject> p_node1ByPassFeatures, List<IGTKeyObject> p_node2ByPassFeatures, string p_ErrorPriority)
        {
            validateMsg = new ValidationRuleManager();
            m_oNode1ByPassFeatures = p_node1ByPassFeatures;
            m_oNode2ByPassFeatures = p_node2ByPassFeatures;
            m_errorPriority = p_ErrorPriority;
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="dataContext">IGTDataContext object</param>
        /// <param name="isolationScenarioFeature">Isolation Scenario feature</param>
        /// <param name="isoCommon">IsoCommon object</param>
        public ByPassScenario(IGTDataContext dataContext, IsoScenarioFeature isolationScenarioFeature, IsoCommon isoCommon)
        {
            m_dataContext = dataContext;
            m_IsolationScenarioFeature = isolationScenarioFeature;
            m_IsoCommon = isoCommon;
        }
        #endregion

        #region Methods
        /// <summary>
        /// ProcessByPassScenario
        /// </summary>
        /// <param name="ErrorMessage">Error Message to be displayed</param>
        /// <param name="ErrorPriority">Error Priority</param>
        public void ProcessIsolationScenario(out string ErrorMessage, out string ErrorPriority)
        {
            try
            {
                if (!CheckByPass(m_oNode1ByPassFeatures, m_oNode2ByPassFeatures))
                {
                    validateMsg.Rule_Id = "ISO04";
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
                m_oNode1ByPassFeatures = null;
                m_oNode2ByPassFeatures = null;
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
                bool bypassPtCreated = false;
                // Check if Isolation Scenario is valid on Node1.
                if (ValidateIsolationPoint(m_IsolationScenarioFeature.RelatedFeaturesNode1, ref isoPtExists, 1))
                {
                    if (!isoPtExists)
                    {
                        // Create Isolation Point since one doesn't exist
                        CreateIsolationPoint(1);
                        bypassPtCreated = true;
						m_IsoCommon.SetNormalAndAsOperatedStatus(m_IsolationScenarioFeature.IsolationPoint1, 1);
                      
                    }

                    // Check if Isolation Scenario is valid on Node2.
                    if (ValidateIsolationPoint(m_IsolationScenarioFeature.RelatedFeaturesNode2, ref isoPtExists, 2))
                    {
                        if (!isoPtExists)
                        {
                            // Create Isolation Point since one doesn't exist
                            CreateIsolationPoint(2);
                            bypassPtCreated = true;
							m_IsoCommon.SetNormalAndAsOperatedStatus(m_IsolationScenarioFeature.IsolationPoint2, 2);
                        }

                        // Check if for bypass point
                        short fno = 0;
                        Int32 fid = 0;
                        if (ValidateBypassPoint(ref isoPtExists, ref fno, ref fid))
                        {
                            if (!isoPtExists)
                            {
                                // Create Bypass Point since one doesn't exist
                                CreateIsolationPoint(3);
                                bypassPtCreated = true;
								m_IsoCommon.SetNormalAndAsOperatedStatus(m_IsolationScenarioFeature.IsolationPoint3);
                            }
                            else
                            {
                                m_IsolationScenarioFeature.IsolationPoint3 = m_dataContext.OpenFeature(fno, fid);
                            }
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

                            // If features related to Isolation Scenario feature are connected directly to Node 1 of Isolation Scenario feature, then
                            // reconnect features to Isolation Point feature.
                            foreach (IGTKeyObject feature in m_IsolationScenarioFeature.RelatedFeaturesNode1)
                            {
                                // Skip the processing for the Isolation Point feature since the Isolation Point was already validated/updated.
                                if (feature.FID != m_IsolationScenarioFeature.IsolationPoint1.FID && dicRelatedNodes.TryGetValue(feature.FID, out relatedNode) && feature.FNO!=40)
                                {
                                    //m_IsoCommon.DetermineConnectivityNode(m_IsolationScenarioFeature.GtKeyObject, feature, ref relatedNode);
                                    m_IsoCommon.EstablishConnectivity( m_IsolationScenarioFeature.IsolationPoint1, feature, GTRelationshipOrdinalConstants.gtrelRelationshipOrdinal1, relatedNode);
                                }
                            }

                            dicRelatedNodes.Clear();

                            foreach (IGTKeyObject feature in m_IsolationScenarioFeature.RelatedFeaturesNode2)
                            {
                                if (feature.FID != m_IsolationScenarioFeature.IsolationPoint2.FID)
                                {
                                    m_IsoCommon.DetermineConnectivityNode(m_IsolationScenarioFeature.GtKeyObject, feature, ref relatedNode);
                                    dicRelatedNodes.Add(feature.FID, relatedNode);
                                }
                            }

                            oRel.SilentDelete(14, GTRelationshipOrdinalConstants.gtrelRelationshipOrdinal2);
                            // If features related to Isolation Scenario feature are connected directly to Node 2 of Isolation Scenario feature, then
                            // reconnect features to Isolation Point feature.
                            foreach (IGTKeyObject feature in m_IsolationScenarioFeature.RelatedFeaturesNode2)
                            {
                                // Skip the processing for the Isolation Point feature since the Isolation Point was already validated/updated.
                                if (feature.FID != m_IsolationScenarioFeature.IsolationPoint2.FID && dicRelatedNodes.TryGetValue(feature.FID, out relatedNode) && feature.FNO != 40)
                                {
                                   // m_IsoCommon.DetermineConnectivityNode(m_IsolationScenarioFeature.GtKeyObject, feature, ref relatedNode);
                                    m_IsoCommon.EstablishConnectivity( m_IsolationScenarioFeature.IsolationPoint2, feature, GTRelationshipOrdinalConstants.gtrelRelationshipOrdinal2, relatedNode);
                                }
                            }
                        }
                      
                        m_IsoCommon.EstablishConnectivity(m_IsolationScenarioFeature.IsolationPoint1, m_IsolationScenarioFeature.GtKeyObject, GTRelationshipOrdinalConstants.gtrelRelationshipOrdinal2, GTRelationshipOrdinalConstants.gtrelRelationshipOrdinal1);
                        m_IsoCommon.EstablishConnectivity(m_IsolationScenarioFeature.IsolationPoint2, m_IsolationScenarioFeature.GtKeyObject, GTRelationshipOrdinalConstants.gtrelRelationshipOrdinal1, GTRelationshipOrdinalConstants.gtrelRelationshipOrdinal2);


                        m_IsoCommon.EstablishConnectivity(m_IsolationScenarioFeature.IsolationPoint3, m_IsolationScenarioFeature.IsolationPoint1, GTRelationshipOrdinalConstants.gtrelRelationshipOrdinal1, GTRelationshipOrdinalConstants.gtrelRelationshipOrdinal1);
                        m_IsoCommon.EstablishConnectivity(m_IsolationScenarioFeature.IsolationPoint3, m_IsolationScenarioFeature.IsolationPoint2, GTRelationshipOrdinalConstants.gtrelRelationshipOrdinal2, GTRelationshipOrdinalConstants.gtrelRelationshipOrdinal2);


                        m_IsoCommon.EstablishOwnerShip(m_IsolationScenarioFeature.GtKeyObject, m_IsolationScenarioFeature.IsolationPoint1);
                        m_IsoCommon.EstablishOwnerShip(m_IsolationScenarioFeature.GtKeyObject, m_IsolationScenarioFeature.IsolationPoint2);
                        m_IsoCommon.EstablishOwnerShip(m_IsolationScenarioFeature.GtKeyObject, m_IsolationScenarioFeature.IsolationPoint3);

                        m_IsoCommon.PopulateProtectionDeviceIDForIsoPt(m_IsolationScenarioFeature.GtKeyObject, m_IsolationScenarioFeature.IsolationPoint1);
                        m_IsoCommon.PopulateProtectionDeviceIDForIsoPt(m_IsolationScenarioFeature.GtKeyObject, m_IsolationScenarioFeature.IsolationPoint2);
                        m_IsoCommon.PopulateProtectionDeviceIDForIsoPt(m_IsolationScenarioFeature.GtKeyObject, m_IsolationScenarioFeature.IsolationPoint3);

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
        /// Method to check whether the active feature has Bypass Points on both nodes, plus a third Bypass Point encompassing feature and the other two Bypass Points.
        /// </summary>
        /// <param name="p_byPassFeatures">Bypass Points features connected to active feature</param>
        /// <returns></returns>
        private bool CheckByPass(List<IGTKeyObject> p_node1ByPassFeatures, List<IGTKeyObject> p_node2ByPassFeatures)
        {
            bool byPass = false;
            Recordset connRs = null;
            int node1 = 0;
            int node2 = 0;
            try
            {
                if (p_node1ByPassFeatures.Count() == 1 && p_node2ByPassFeatures.Count() == 1)
                {
                    p_node1ByPassFeatures[0].Components.GetComponent(11).Recordset.MoveFirst();
                    node1 = Convert.ToInt32(p_node1ByPassFeatures[0].Components.GetComponent(11).Recordset.Fields["NODE_1_ID"].Value);

                    p_node2ByPassFeatures[0].Components.GetComponent(11).Recordset.MoveFirst();
                    node2 = Convert.ToInt32(p_node2ByPassFeatures[0].Components.GetComponent(11).Recordset.Fields["NODE_2_ID"].Value);

                    connRs = GTClassFactory.Create<IGTApplication>().DataContext.OpenRecordset(String.Format("Select * from connectivity_n where g3e_fno=40 and NODE_1_ID={0} and NODE_2_ID={1}", node1, node2), CursorTypeEnum.adOpenStatic,
                          LockTypeEnum.adLockReadOnly, (int)CommandTypeEnum.adCmdText);

                    if (connRs != null && connRs.RecordCount >= 1)
                    {
                        byPass = true;
                    }
                }

            }
            catch
            {
                throw;
            }
            return byPass;
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
                    if ((feature.FNO == 40 || feature.FNO == 80) && m_IsoCommon.CheckAssociatedVirtualPoint(feature, m_IsolationScenarioFeature.GtKeyObject.FID))
                    {
                        isoPtExists = true;

                        if (isoPtNumber == 1)
                        {
                            m_IsolationScenarioFeature.IsolationPoint1 = feature;
                        }
                        else
                        {
                            m_IsolationScenarioFeature.IsolationPoint2 = feature;
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
                            foreach (IGTKeyObject relFeature in m_IsolationScenarioFeature.RelatedFeaturesNode2)
                            {
                                if (feature.FID != relFeature.FID && (feature.FNO == 40 || feature.FNO == 80))
                                {
                                    exist = true;
                                    break;
                                }
                            }
                        }
                        else
                        {
                            foreach (IGTKeyObject relFeature in m_IsolationScenarioFeature.RelatedFeaturesNode1)
                            {
                                if (feature.FID != relFeature.FID && (feature.FNO == 40 || feature.FNO == 80))
                                {
                                    exist = true;
                                    break;
                                }
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
        /// Method to check whether the bypass point exists for the isolation scenario feature.
        /// </summary>
        /// <param name="isoPtExists">True, if isolation point exists</param>
        /// <param name="fno">G3E_FNO of the isolation point</param>
        /// <param name="fid">G3E_FID of the isolation point</param>
        /// <returns>Boolean indicating method execution status</returns>
        private bool ValidateBypassPoint(ref bool isoPtExists, ref short fno, ref Int32 fid)
        {
            bool returnValue = false;

            try
            {
                isoPtExists = false;

                string sql = "select g3e_fid, g3e_fno from VIRTUALPT_N where ASSOCIATED_FID = ? and g3e_fid not in (?, ?)";

                Recordset bypassRS = m_dataContext.OpenRecordset(sql, CursorTypeEnum.adOpenStatic, LockTypeEnum.adLockReadOnly, (int)CommandTypeEnum.adCmdText,
                                                               m_IsolationScenarioFeature.GtKeyObject.FID, m_IsolationScenarioFeature.IsolationPoint1.FID, m_IsolationScenarioFeature.IsolationPoint2.FID);

                if (bypassRS.RecordCount > 0)
                {
                    isoPtExists = true;
                    bypassRS.MoveFirst();
                    fno = Convert.ToInt16(bypassRS.Fields["g3e_fno"].Value);
                    fid = Convert.ToInt32(bypassRS.Fields["g3e_fid"].Value);

                    IGTKeyObject bypassKO = m_dataContext.OpenFeature(fno, fid);

                    string errMessage;
                    m_IsoCommon.SetVirtualPointAttributes(m_IsolationScenarioFeature, bypassKO, out errMessage);
                }

                returnValue = true;
            }
            catch (Exception ex)
            {
                returnValue = false;
                if (m_IsoCommon.InteractiveMode)
                {
                    MessageBox.Show("Error in Isolation Scenario FI:ValidateBypassPoint - " + ex.Message, "G/Technology", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }
            }
            return returnValue;
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
                IGTKeyObject isoPtKO = m_dataContext.NewFeature(40);

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

                if (m_IsolationScenarioFeature.GtKeyObject.FNO == 36)
                {
                    if (isoPtNumber == 1)
                    {
                        xOffset = IsoCommon.VOLT_REG_ISO_PT1_OFFSET_X;
                        yOffset = IsoCommon.VOLT_REG_ISO_PT1_OFFSET_Y;
                    }
                    else if (isoPtNumber == 2)
                    {
                        xOffset = IsoCommon.VOLT_REG_ISO_PT2_OFFSET_X;
                        yOffset = IsoCommon.VOLT_REG_ISO_PT2_OFFSET_Y;
                    }
                    else
                    {
                        xOffset = IsoCommon.VOLT_REG_ISO_PT3_OFFSET_X;
                        yOffset = IsoCommon.VOLT_REG_ISO_PT3_OFFSET_Y;
                    }
                }
                else
                {
                    if (isoPtNumber == 1)
                    {
                        xOffset = IsoCommon.RECLOSER_ISO_PT1_OFFSET_X;
                        yOffset = IsoCommon.RECLOSER_ISO_PT1_OFFSET_Y;
                    }
                    else if (isoPtNumber == 2)
                    {
                        xOffset = IsoCommon.RECLOSER_ISO_PT2_OFFSET_X;
                        yOffset = IsoCommon.RECLOSER_ISO_PT2_OFFSET_Y;
                    }
                    else
                    {
                        xOffset = IsoCommon.RECLOSER_ISO_PT3_OFFSET_X;
                        yOffset = IsoCommon.RECLOSER_ISO_PT3_OFFSET_Y;
                    }
                }

                gtPt = m_IsoCommon.GetOffsetPoint(orientedPtGeom.Origin, xOffset * .3048, yOffset * .3048, orientedPtGeom.Orientation);

                orientedPtGeom.Origin = gtPt;

                isoPtKO.Components.GetComponent(isoPtCNO).Geometry = orientedPtGeom;

                if (isoPtNumber == 1)
                {
                    //m_IsoCommon.EstablishConnectivity(isoPtKO, m_IsolationScenarioFeature.GtKeyObject, GTRelationshipOrdinalConstants.gtrelRelationshipOrdinal2, GTRelationshipOrdinalConstants.gtrelRelationshipOrdinal1);
                    m_IsolationScenarioFeature.RelatedFeaturesNode1.Add(isoPtKO);
                    m_IsolationScenarioFeature.IsolationPoint1 = isoPtKO;
                }
                else if (isoPtNumber == 2)
                {
                    //m_IsoCommon.EstablishConnectivity(isoPtKO, m_IsolationScenarioFeature.GtKeyObject, GTRelationshipOrdinalConstants.gtrelRelationshipOrdinal1, GTRelationshipOrdinalConstants.gtrelRelationshipOrdinal2);
                    m_IsolationScenarioFeature.RelatedFeaturesNode2.Add(isoPtKO);
                    m_IsolationScenarioFeature.IsolationPoint2 = isoPtKO;
                }
                else
                {
                    m_IsolationScenarioFeature.IsolationPoint3 = isoPtKO;
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
