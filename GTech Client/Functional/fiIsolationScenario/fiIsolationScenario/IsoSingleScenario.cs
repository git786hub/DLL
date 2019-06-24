// ===================================================
//  Copyright 2017 Intergraph Corp.
//  File Name: IsoSingleScenario.cs
// 
//  Description:   Validates for active feature not connected to exactly one Isolation Point.
//  Remarks: 
//  Modification History
//  Date                Author                      Comments
//  --------            --------------              --------
//  23/03/2018        Prathyusha                  Created 
// User: pnlella      Date: 08/03/19   Time: 15:15 Adjusted code for handling issues related to Isolation scenario.
// User: Akhilesh     Date: 19/03/19   Time: 15:35 Adjusted code for handling issues related to Detail Isolation Point Creation.
// ======================================================
using System.Collections.Generic;
using System.Linq;
using Intergraph.GTechnology.API;
using System;
using ADODB;
using System.Windows.Forms;

namespace GTechnology.Oncor.CustomAPI
{
    class IsoSingleScenario : IProcessIsolationScenario
    {
        #region Variables

        ValidationRuleManager validateMsg = null;
        List<IGTKeyObject> m_oNode1IsolationFeatures = null;
        List<IGTKeyObject> m_oNode2IsolationFeatures = null;
        IsoScenarioFeature m_IsolationScenarioFeature = null;
        IsoCommon m_IsoCommon = null;
        IGTDataContext m_dataContext = null;
        string m_errorPriority = null;

        #endregion

        #region Constructor
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="p_node1IsolationFeatures">Node1 Isolation features connected to active feature</param>
        /// <param name="p_node2IsolationFeatures">Node2 Isolation features connected to active feature</param>
        /// <param name="p_errorPriority">Error Priority</param>
        public IsoSingleScenario(List<IGTKeyObject> p_node1IsolationFeatures, List<IGTKeyObject> p_node2IsolationFeatures, string p_errorPriority)
        {
            validateMsg = new ValidationRuleManager();
            m_oNode1IsolationFeatures = p_node1IsolationFeatures;
            m_oNode2IsolationFeatures = p_node2IsolationFeatures;
            m_errorPriority = p_errorPriority;
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="dataContext">IGTDataContext object</param>
        /// <param name="isolationScenarioFeature">Isolation Scenario feature</param>
        /// <param name="isoCommon">IsoCommon object</param>
        public IsoSingleScenario(IGTDataContext dataContext, IsoScenarioFeature isolationScenarioFeature, IsoCommon isoCommon)
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
                if (!CheckIsoSingle(m_oNode1IsolationFeatures, m_oNode2IsolationFeatures))
                {
                    validateMsg.Rule_Id = "ISO02";
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
                m_oNode1IsolationFeatures = null;
                m_oNode2IsolationFeatures = null;
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
                bool isoPtDelete = false;
                // Check if Isolation Scenario is valid.
                if (ValidateIsolationPoint(ref isoPtExists, ref isoPtDelete))
                {
                    if (!isoPtExists)
                    {
                        // Create Isolation Point since one doesn't exist
                        CreateIsolationPoint();
                        m_IsoCommon.SetNormalAndAsOperatedStatus(m_IsolationScenarioFeature.IsolationPoint1, 1);
                    }

                    GTRelationshipOrdinalConstants relatedNode = GTRelationshipOrdinalConstants.gtrelRelationshipOrdinal1;

                    // If features related to Isolation Scenario feature are connected directly to Isolation Scenario feature, then
                    // reconnect features to Isolation Point feature.
                    if (!isoPtDelete)
                    {
                        foreach (IGTKeyObject feature in m_IsolationScenarioFeature.RelatedFeaturesNode1)
                        {
                            if (feature.FID != m_IsolationScenarioFeature.IsolationPoint1.FID)
                            {
                                m_IsoCommon.DetermineConnectivityNode(m_IsolationScenarioFeature.GtKeyObject, feature, ref relatedNode);
                                dicRelatedNodes.Add(feature.FID, relatedNode);
                            }
                        }
                    }
                    using (IGTRelationshipService oRel = GTClassFactory.Create<IGTRelationshipService>())
                    {
                        oRel.DataContext = m_dataContext;
                        oRel.ActiveFeature = m_IsolationScenarioFeature.GtKeyObject;                       
                        oRel.SilentDelete(14, GTRelationshipOrdinalConstants.gtrelRelationshipOrdinal1);
                    }
                    if (!isoPtDelete)
                    {
                        foreach (IGTKeyObject feature in m_IsolationScenarioFeature.RelatedFeaturesNode1)
                        {
                            // Skip the processing for the Isolation Point feature since the Isolation Point was already validated/updated.
                            if (feature.FID != m_IsolationScenarioFeature.IsolationPoint1.FID && dicRelatedNodes.TryGetValue(feature.FID,out relatedNode))
                            {
                                //m_IsoCommon.DetermineConnectivityNode(m_IsolationScenarioFeature.GtKeyObject, feature, ref dicRelatedNodes);
                                m_IsoCommon.EstablishConnectivity(m_IsolationScenarioFeature.IsolationPoint1, feature, GTRelationshipOrdinalConstants.gtrelRelationshipOrdinal1, relatedNode);
                            }
                        }

                        m_IsoCommon.EstablishConnectivity(m_IsolationScenarioFeature.IsolationPoint1, m_IsolationScenarioFeature.GtKeyObject, GTRelationshipOrdinalConstants.gtrelRelationshipOrdinal2, GTRelationshipOrdinalConstants.gtrelRelationshipOrdinal1);

                        m_IsoCommon.EstablishOwnerShip(m_IsolationScenarioFeature.GtKeyObject, m_IsolationScenarioFeature.IsolationPoint1);

                        if (m_IsolationScenarioFeature.IsolationPoint1 != null)
                        {
                            PopulateProtectionDeviceIDForIsoPt();
                        }
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

        private void PopulateProtectionDeviceIDForIsoPt()
        {
            try
            {
                Recordset connecRS = m_IsolationScenarioFeature.GtKeyObject.Components.GetComponent(11).Recordset;

                if (connecRS != null && connecRS.RecordCount > 0)
                {
                    connecRS.MoveFirst();

                    Recordset isoPtConnecRS = m_IsolationScenarioFeature.IsolationPoint1.Components.GetComponent(11).Recordset;

                    if (isoPtConnecRS != null && isoPtConnecRS.RecordCount > 0)
                    {
                        isoPtConnecRS.MoveFirst();

                        isoPtConnecRS.Fields["PROTECTIVE_DEVICE_FID"].Value = connecRS.Fields["PROTECTIVE_DEVICE_FID"].Value;
                        isoPtConnecRS.Fields["PP_PROTECTIVE_DEVICE_FID"].Value = connecRS.Fields["PP_PROTECTIVE_DEVICE_FID"].Value;
                    }
                }

            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Method to check whether the active feature has Isolation Point on one node
        /// </summary>
        /// <param name="p_node1isolationFeatures">Node1 Isolation features connected to active feature</param>
        /// <param name="p_node2isolationFeatures">Node2 Isolation features connected to active feature</param>
        /// <returns></returns>
        private bool CheckIsoSingle(List<IGTKeyObject> p_node1isolationFeatures, List<IGTKeyObject> p_node2isolationFeatures)
        {
            bool isIsoSingle = false;
            try
            {
                if ((p_node1isolationFeatures.Count() == 1 && p_node2isolationFeatures.Count() == 0) || (p_node2isolationFeatures.Count() == 1 && p_node1isolationFeatures.Count() == 0))
                {
                    isIsoSingle = true;
                }
            }
            catch
            {
                throw;
            }
            return isIsoSingle;
        }

        /// <summary>
        /// Method to check whether the active feature has Isolation Point on Node1.
        /// If Isolation Point exists then validate association and update if necessary.
        /// </summary>
        /// <param name="isoPtExists">True, if virtual point was found</param>
        /// <returns>Boolean indicating method execution status</returns>
        private bool ValidateIsolationPoint(ref bool isoPtExists, ref bool isoPtDelete)
        {
            bool returnValue = false;
            try
            {
                isoPtExists = false;
                IGTKeyObject feature=null;

                String sql = "select G3E_FID,G3E_FNO from VIRTUALPT_N where ASSOCIATED_FID = ?";

                Recordset virtualRS = m_dataContext.OpenRecordset(sql, CursorTypeEnum.adOpenStatic, LockTypeEnum.adLockReadOnly, (int)CommandTypeEnum.adCmdText,
                                                               m_IsolationScenarioFeature.GtKeyObject.FID);

                if (virtualRS != null && virtualRS.RecordCount > 0)
                {
                    virtualRS.MoveFirst();

                    feature = m_dataContext.OpenFeature(Convert.ToInt16(virtualRS.Fields["G3E_FNO"].Value), Convert.ToInt32(virtualRS.Fields["G3E_FID"].Value));

                    isoPtExists = true;

                    CheckForVirtualGeometry(feature, ref isoPtDelete);

                    if (!isoPtDelete)
                    {
                        m_IsolationScenarioFeature.IsolationPoint1 = feature;

                        // Validate and set, if necessary, specific Isolation Point attributes to associated feature
                        string errMessage;
                        m_IsoCommon.SetVirtualPointAttributes(m_IsolationScenarioFeature, feature, out errMessage);
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

        private void CheckForVirtualGeometry(IGTKeyObject feature,ref bool isoPtDelete)
        {
            short virtualPointCNO = 0;
            try
            {
                if(m_IsoCommon.IsDetailMapWindow())
                {
                    virtualPointCNO = 79;
                }
                else
                {
                    virtualPointCNO = 78;
                }
                IGTComponent virtualPointPrimaryComp = feature.Components.GetComponent(virtualPointCNO);

                if (virtualPointPrimaryComp != null)
                {
                    if(virtualPointPrimaryComp.Geometry == null)
                    {
                        isoPtDelete = true;
                    }                    
                }
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Method to create an Isolation Point.
        /// </summary>
        /// <returns>Boolean indicating method execution status</returns>
        private bool CreateIsolationPoint()
        {
            bool returnValue = false;
            short primaryCNO = 0;
            short isoPtCNO = 0;
            try
            {
                short isoPtFNO;
                if (m_IsolationScenarioFeature.GtKeyObject.FNO == 98 || m_IsolationScenarioFeature.GtKeyObject.FNO == 99)
                {
                    isoPtFNO = 82;
                }
                else
                {
                    isoPtFNO = 6;
                }

                IGTKeyObject isoPtKO = m_dataContext.NewFeature(isoPtFNO);

                string errMessage;
                m_IsoCommon.SetAttributeDefaults(isoPtKO, out errMessage);
                m_IsoCommon.SetVirtualPointAttributes(m_IsolationScenarioFeature, isoPtKO, out errMessage);

                Recordset oRSFeature = m_dataContext.MetadataRecordset("G3E_FEATURES_OPTABLE", "g3e_fno = " + m_IsolationScenarioFeature.GtKeyObject.FNO);
                m_IsoCommon.IsDetailMapWindow();
                if(m_IsoCommon.detailID==0)
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
                    if(m_IsoCommon.detailID != 0)
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

                switch (m_IsolationScenarioFeature.GtKeyObject.FNO)
                {
                    case 4:
                        xOffset = IsoCommon.CAPACITOR_ISO_PT_OFFSET_X;
                        yOffset = IsoCommon.CAPACITOR_ISO_PT_OFFSET_Y;
                        break;
                    case 12:
                        xOffset = IsoCommon.PPOD_ISO_PT_OFFSET_X;
                        yOffset = IsoCommon.PPOD_ISO_PT_OFFSET_Y;
                        break;
                    case 59:
                    case 60:
                    case 98:
                    case 99:
                        xOffset = IsoCommon.XFMR_ISO_PT_OFFSET_X;
                        yOffset = IsoCommon.XFMR_ISO_PT_OFFSET_Y;
                        break;
                }

                gtPt = m_IsoCommon.GetOffsetPoint(orientedPtGeom.Origin, xOffset * .3048, yOffset * .3048, orientedPtGeom.Orientation);

                orientedPtGeom.Origin = gtPt;

                isoPtKO.Components.GetComponent(isoPtCNO).Geometry = orientedPtGeom;

                //m_IsoCommon.EstablishConnectivity(isoPtKO, m_IsolationScenarioFeature.GtKeyObject, GTRelationshipOrdinalConstants.gtrelRelationshipOrdinal2, GTRelationshipOrdinalConstants.gtrelRelationshipOrdinal1);
                m_IsolationScenarioFeature.RelatedFeaturesNode1.Add(isoPtKO);
                m_IsolationScenarioFeature.IsolationPoint1 = isoPtKO;

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
