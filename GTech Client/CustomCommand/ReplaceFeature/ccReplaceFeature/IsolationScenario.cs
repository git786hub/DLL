// ===================================================
//  Copyright 2017 Intergraph Corp.
//  File Name: IsolationScenario.cs
// 
//  Description:   Class which handles all the Isolation scenario methods required for Replace command.
//  Remarks: 
//  Modification History
//  Date                Author                      Comments
//  --------            --------------              --------
//  08/03/2019          Prathyusha                  Created 
// ======================================================
using System;
using System.Collections.Generic;
using ADODB;
using Intergraph.GTechnology.API;

namespace GTechnology.Oncor.CustomAPI
{
    public class IsolationScenario
    {
        IGTDataContext m_dataContext;
        List<short> m_IsoScenarioselectedFeatureFNOs = null;
        private enum IsolationScenarios { ISOSINGLE, ISODUAL, ELBOW, BYPASS, NULL };
        IsolationScenarios m_isolationScenarioType = IsolationScenarios.NULL;
        bool m_elbowConnected = false;

        public IsolationScenario(IGTDataContext p_DataContext)
        {
            m_dataContext = p_DataContext;
            m_IsoScenarioselectedFeatureFNOs = new List<short>(new short[] { 6, 40, 41, 80, 81, 82 });
        }

        public bool CheckIsoScenarioFeature(short selectedFeatureFNO,int selectedFeatureFID)
        {
            bool isolationScenario = false;
            try
            {
                IGTKeyObject selectedFeature= m_dataContext.OpenFeature(selectedFeatureFNO, selectedFeatureFID);

                string feedType = string.Empty;
                if (selectedFeatureFNO == 59 && selectedFeature.Components.GetComponent(5901).Recordset != null && selectedFeature.Components.GetComponent(5901).Recordset.RecordCount > 0)
                {
                    selectedFeature.Components.GetComponent(5901).Recordset.MoveFirst();
                    feedType = Convert.ToString(selectedFeature.Components.GetComponent(5901).Recordset.Fields["FEED_TYPE"].Value);
                }
                else if (selectedFeatureFNO == 60 && selectedFeature.Components.GetComponent(6002).Recordset != null && selectedFeature.Components.GetComponent(6002).Recordset.RecordCount > 0)
                {
                    selectedFeature.Components.GetComponent(6002).Recordset.MoveFirst();
                    feedType = Convert.ToString(selectedFeature.Components.GetComponent(6002).Recordset.Fields["FEED_TYPE"].Value);
                }

                //ISODUAL
                if (selectedFeatureFNO == 34) //Autotransformer
                {
                    isolationScenario = true;
                    m_isolationScenarioType = IsolationScenarios.ISODUAL;
                }
                //ISOSINGLE
                else if (selectedFeatureFNO == 4 || selectedFeatureFNO == 12 || selectedFeatureFNO == 59 || (selectedFeatureFNO == 60 && feedType.ToUpper() == "RADIAL") || selectedFeatureFNO == 99 || selectedFeatureFNO == 98)
                {
                    isolationScenario = true;
                    m_isolationScenarioType = IsolationScenarios.ISOSINGLE;
                }                
                //ELBOW
                else if (selectedFeatureFNO == 5 || (selectedFeatureFNO == 60 && feedType.ToUpper() == "LOOP"))
                {
                    isolationScenario = true;
                    m_isolationScenarioType = IsolationScenarios.ELBOW;
                }
                //BYPASS
                else if (selectedFeatureFNO == 14 || selectedFeatureFNO == 15 || selectedFeatureFNO == 36)
                {
                    isolationScenario = true;
                    m_isolationScenarioType = IsolationScenarios.BYPASS;
                }
                else
                {
                    isolationScenario = false;
                }
                
            }
            catch
            {
                throw;
            }
            return isolationScenario;
        }
        public void DeleteOldIsolationPoint(IGTKeyObject feature)
        {
            try
            {
                if(m_isolationScenarioType==IsolationScenarios.ISOSINGLE || m_isolationScenarioType==IsolationScenarios.ELBOW)
                {
                    DeleteNodeRelatedfeatures(GTRelationshipOrdinalConstants.gtrelRelationshipOrdinal1,feature);
                }
                else if (m_isolationScenarioType == IsolationScenarios.ISODUAL || m_isolationScenarioType==IsolationScenarios.BYPASS)
                {
                    DeleteNodeRelatedfeatures(GTRelationshipOrdinalConstants.gtrelRelationshipOrdinal1,feature);
                    DeleteNodeRelatedfeatures(GTRelationshipOrdinalConstants.gtrelRelationshipOrdinal2,feature);
                }
            }
            catch
            {
                throw;
            }
        }
        private void DeleteNodeRelatedfeatures(GTRelationshipOrdinalConstants ordinal,IGTKeyObject m_feature)
        {
            try
            {
                using (IGTRelationshipService oRel = GTClassFactory.Create<IGTRelationshipService>())
                {
                    oRel.DataContext = m_dataContext;
                    oRel.ActiveFeature = m_feature;

                    IGTKeyObjects relatedFeatures = GTClassFactory.Create<IGTKeyObjects>();
                    try
                    {
                        relatedFeatures = oRel.GetRelatedFeatures(14, ordinal);
                    }
                    catch
                    {

                    }

                    if (relatedFeatures != null)
                    {
                        foreach (IGTKeyObject feature in relatedFeatures)
                        {
                            if (m_IsoScenarioselectedFeatureFNOs.Contains(feature.FNO))
                            {
                                oRel.SilentDelete(14, ordinal);
                            }
                        }
                    }
                }
            }
            catch
            {
                throw;
            }
        }

        public void GetAssociatedVirtualPoints(int selectedFeatureFID, ref Dictionary<int, short> dicAssociatedVirtual)
        {
            try
            {
                String sql = "select G3E_FID,G3E_FNO from VIRTUALPT_N where ASSOCIATED_FID = ?";

                Recordset virtualRS = m_dataContext.OpenRecordset(sql, CursorTypeEnum.adOpenStatic, LockTypeEnum.adLockReadOnly, (int)CommandTypeEnum.adCmdText,
                                                              selectedFeatureFID);
                if (virtualRS != null && virtualRS.RecordCount > 0)
                {
                    virtualRS.MoveFirst();

                    while (!virtualRS.EOF)
                    {
                        if (m_IsoScenarioselectedFeatureFNOs.Contains(Convert.ToInt16(virtualRS.Fields["G3E_FNO"].Value)))
                        {
                            dicAssociatedVirtual.Add(Convert.ToInt32(virtualRS.Fields["G3E_FID"].Value), Convert.ToInt16(virtualRS.Fields["G3E_FNO"].Value));
                        }
                        virtualRS.MoveNext();
                    }
                }
                virtualRS.Close();
                virtualRS = null;
            }
            catch
            {
                throw;
            }
        }
        public void UpdateIsolationPointConnectivity(short selectedObjectFNO,int selectedObjectFID,IGTKeyObject newFeature)
        {
            try
            {
                IGTKeyObject oldfeature = m_dataContext.OpenFeature(selectedObjectFNO, selectedObjectFID);
                            
                IGTKeyObject oIsolationPointParent = null;
                IGTKeyObject oIsolationPointNew = null;


                if (m_isolationScenarioType == IsolationScenarios.ISOSINGLE)
                {
                    GetIsolationPoint(oldfeature, ref oIsolationPointParent,GTRelationshipOrdinalConstants.gtrelRelationshipOrdinal1);
                    GetIsolationPoint(newFeature, ref oIsolationPointNew, GTRelationshipOrdinalConstants.gtrelRelationshipOrdinal1);
                    EstablishIsolationPointConnectivity(oIsolationPointParent, oIsolationPointNew, GTRelationshipOrdinalConstants.gtrelRelationshipOrdinal1);
                }
                else if(m_isolationScenarioType ==IsolationScenarios.ISODUAL || m_isolationScenarioType == IsolationScenarios.BYPASS)
                {
                    GetIsolationPoint(oldfeature, ref oIsolationPointParent, GTRelationshipOrdinalConstants.gtrelRelationshipOrdinal1);
                    GetIsolationPoint(newFeature, ref oIsolationPointNew, GTRelationshipOrdinalConstants.gtrelRelationshipOrdinal1);
                    EstablishIsolationPointConnectivity(oIsolationPointParent, oIsolationPointNew, GTRelationshipOrdinalConstants.gtrelRelationshipOrdinal1);
                    GetIsolationPoint(oldfeature, ref oIsolationPointParent, GTRelationshipOrdinalConstants.gtrelRelationshipOrdinal2);
                    GetIsolationPoint(newFeature, ref oIsolationPointNew, GTRelationshipOrdinalConstants.gtrelRelationshipOrdinal2);
                    EstablishIsolationPointConnectivity(oIsolationPointParent, oIsolationPointNew, GTRelationshipOrdinalConstants.gtrelRelationshipOrdinal2);
                }
                else if(m_isolationScenarioType == IsolationScenarios.ELBOW)
                {
                    GetIsolationPoint(oldfeature, ref oIsolationPointParent, GTRelationshipOrdinalConstants.gtrelRelationshipOrdinal1);
                    GetIsolationPoint(newFeature, ref oIsolationPointNew, GTRelationshipOrdinalConstants.gtrelRelationshipOrdinal1);
                    EstablishIsolationPointConnectivity(oIsolationPointParent, oIsolationPointNew, GTRelationshipOrdinalConstants.gtrelRelationshipOrdinal1);
                    m_elbowConnected = true;
                    GetIsolationPoint(oldfeature, ref oIsolationPointParent, GTRelationshipOrdinalConstants.gtrelRelationshipOrdinal1);
                    m_elbowConnected = true;
                    GetIsolationPoint(newFeature, ref oIsolationPointNew, GTRelationshipOrdinalConstants.gtrelRelationshipOrdinal1);
                    EstablishIsolationPointConnectivity(oIsolationPointParent, oIsolationPointNew, GTRelationshipOrdinalConstants.gtrelRelationshipOrdinal2);
                }
            }            
            catch (Exception)
            {
                throw;
            }
        }

        private void EstablishIsolationPointConnectivity(IGTKeyObject oIsolationPointParent, IGTKeyObject oIsolationPointNew, GTRelationshipOrdinalConstants node)
        { 

            try
            {
                IGTKeyObjects oldIsoRelatedFeatures = GTClassFactory.Create<IGTKeyObjects>();

                using (IGTRelationshipService oRel = GTClassFactory.Create<IGTRelationshipService>())
                {
                    oRel.ActiveFeature = oIsolationPointParent;
                    oRel.DataContext = m_dataContext;
                    try
                    {
                        oldIsoRelatedFeatures = oRel.GetRelatedFeatures(14, node);
                    }
                    catch
                    {

                    }

                    oRel.ActiveFeature = oIsolationPointNew;
                    
                    oRel.SilentDelete(14, node);
                    GTRelationshipOrdinalConstants relatedNode = GTRelationshipOrdinalConstants.gtrelRelationshipOrdinal1;

                    foreach (IGTKeyObject feature in oldIsoRelatedFeatures)
                    {
                        if (m_isolationScenarioType == IsolationScenarios.BYPASS && feature.FNO == 40)
                        {
                            continue;
                        }
                        else
                        {
                            DetermineConnectivityNode(oIsolationPointParent, feature, ref relatedNode);
                            EstablishConnectivity(oIsolationPointNew, feature, node, relatedNode);
                        }
                    }

                }
            }
            catch
            {
                throw;
            }
        }
        public bool EstablishConnectivity(IGTKeyObject activeKO, IGTKeyObject relatedKO, GTRelationshipOrdinalConstants activeRelationshipOrdinal, GTRelationshipOrdinalConstants relatedRelationshipOrdinal)
        {
            bool returnValue = false;
            IGTRelationshipService relationshipService = null;
            Int32 node1 = 0;
            Int32 node2 = 0;
            Recordset connRS = null;

            try
            {
                connRS = relatedKO.Components.GetComponent(11).Recordset;
                if (connRS.RecordCount > 0)
                {
                    connRS.MoveFirst();
                    node1 = Convert.ToInt32(connRS.Fields["NODE_1_ID"].Value);
                    node2 = Convert.ToInt32(connRS.Fields["NODE_2_ID"].Value);
                }

                relationshipService = GTClassFactory.Create<IGTRelationshipService>();
                relationshipService.DataContext = m_dataContext;
                relationshipService.ActiveFeature = activeKO;

                if (relationshipService.AllowSilentEstablish(relatedKO))
                {
                    relationshipService.SilentEstablish(14, relatedKO, activeRelationshipOrdinal, relatedRelationshipOrdinal);
                }

                returnValue = true;
            }
            catch
            {
                //returnValue = false;
                returnValue = true;

                // Reset connectivity
                if (connRS != null)
                {
                    if (connRS.RecordCount > 0)
                    {
                        connRS.MoveFirst();
                        connRS.Fields["NODE_1_ID"].Value = node1;
                        connRS.Fields["NODE_2_ID"].Value = node2;
                    }
                }
              
            }

            relationshipService.Dispose();
            return returnValue;
        }
        private void GetIsolationPoint(IGTKeyObject feature, ref IGTKeyObject oIsolationPointParent, GTRelationshipOrdinalConstants node)
        {
            try
            {
                IGTKeyObjects relatedFeatures = GTClassFactory.Create<IGTKeyObjects>();

                using (IGTRelationshipService oRel = GTClassFactory.Create<IGTRelationshipService>())
                {
                    oRel.ActiveFeature = feature;
                    oRel.DataContext = m_dataContext;
                    try
                    {
                        relatedFeatures = oRel.GetRelatedFeatures(14, node);
                    }
                    catch
                    {

                    }

                    if (relatedFeatures != null && relatedFeatures.Count > 0)
                    {
                        if (feature != null)
                        {
                            foreach (IGTKeyObject item in relatedFeatures)
                            {
                                if (m_IsoScenarioselectedFeatureFNOs.Contains(item.FNO) && CheckAssociatedVirtualPoint(item.FID, feature.FID))
                                {
                                    if (m_elbowConnected)
                                    {
                                        m_elbowConnected = false;
                                        continue;
                                    }
                                    else
                                    {
                                        oIsolationPointParent = item;
                                        break;
                                    }
                                }
                            }
                        }
                    }
                }
            }
            catch
            {
                throw;
            }
        }

        private bool CheckAssociatedVirtualPoint(int virtualFID, int selectedFID)
        {
            bool associtedVirtualPt = false;
            try
            {
                String sql = "select G3E_FID,G3E_FNO from VIRTUALPT_N where ASSOCIATED_FID = ?";

                Recordset virtualRS = m_dataContext.OpenRecordset(sql, CursorTypeEnum.adOpenStatic, LockTypeEnum.adLockReadOnly, (int)CommandTypeEnum.adCmdText,
                                                              selectedFID);
                if (virtualRS != null && virtualRS.RecordCount > 0)
                {
                    virtualRS.MoveFirst();

                    while (!virtualRS.EOF)
                    {
                        if (Convert.ToInt32(virtualRS.Fields["g3e_fid"].Value) == virtualFID)
                        {
                            associtedVirtualPt = true;
                            break;
                        }
                        virtualRS.MoveNext();
                    }
                }
                virtualRS.Close();
                virtualRS = null;
            }
            catch
            {
                throw;
            }
            return associtedVirtualPt;
        }

        public void DetermineConnectivityNode(IGTKeyObject activeKO, IGTKeyObject relatedKO, ref GTRelationshipOrdinalConstants relatedRelationshipOrdinal)
        {
            IGTKeyObjects m_relatedFeatures = null;
            bool foundNode = false;
            try
            {
                using (IGTRelationshipService oRel = GTClassFactory.Create<IGTRelationshipService>())
                {
                    oRel.DataContext = m_dataContext;
                    oRel.ActiveFeature = relatedKO;

                    m_relatedFeatures = oRel.GetRelatedFeatures(14, GTRelationshipOrdinalConstants.gtrelRelationshipOrdinal1);
                }
                    foreach (IGTKeyObject feature in m_relatedFeatures)
                    {
                        if (feature.FID == activeKO.FID)
                        {
                            relatedRelationshipOrdinal = GTRelationshipOrdinalConstants.gtrelRelationshipOrdinal1;
                            foundNode = true;
                            break;
                        }
                    }

                    if (!foundNode)
                    {
                        relatedRelationshipOrdinal = GTRelationshipOrdinalConstants.gtrelRelationshipOrdinal2;
                    }
                
            }
            catch
            {
                throw;
            }       
        }
    }
}
