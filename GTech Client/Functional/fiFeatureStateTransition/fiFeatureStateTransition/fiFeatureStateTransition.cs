//----------------------------------------------------------------------------+
//        Class: fiFeatureStateTransition
//  Description: This functional interface sets the attribute identified by the 
//               count of instances of the component for which this interface is 
//               configured.
//----------------------------------------------------------------------------+
//     $Author:: pnlella                                                      $
//       $Date:: 22/08/2017                                                     $
//   $Revision:: 1                                                            $
//----------------------------------------------------------------------------+
//    $History:: fiFeatureStateTransition.cs                                           $
//----------------------------------------------------------------------------+
//  09-JAN-2019, Hari -Fix for ALMS 1557 and 1817
//  08-March-2019, Akhliesh -Fix for ALMS 2003
//  18-March-2019, Hari -Fix for ALM 1817- Set status attributes for active feature and associated virtual points if they exist
//  21-April-2019, Prathyusha -Fix for ALM 2173- Updated code to delete virtual points as per the fetaure state
//  7-May-2019, Hari - Fix for ALM-2356-JIRA-2798 - As-Operated and Normal Status of feature transitioning INI > CLS should not transition to match
//  7-May-2019, Hari - Fix for ALM-2360-JIRA-2799 - when feature state transitions to INI or CLS, copy the affected proposed voltages to the actual voltage attributes (Voltage 2 for Transformers, and both Voltage 1 & 2 for Autotransformers).

using System;
using ADODB;
using System.Windows.Forms;
using Intergraph.GTechnology.Interfaces;
using Intergraph.GTechnology.API;
using System.Collections.Generic;

namespace GTechnology.Oncor.CustomAPI
{
    public class fiFeatureStateTransition : IGTFunctional
    {
        #region Private Members

        private GTArguments m_GTArguments = null;
        private IGTDataContext m_GTDataContext = null;
        private string m_gCompName = string.Empty;
        private IGTComponents m_gComps = null;
        private IGTFieldValue m_gFieldVal = null;
        private string m_gFieldName = string.Empty;
        private GTFunctionalTypeConstants m_gFIType;

        private const short m_cRNO = 14;
        private const short m_virtualAttributesCNO = 4;
        private string m_connectivityTable = null;
        string m_featureStateBeforeChange = string.Empty;
        string m_featureState = string.Empty;

        private List<short> m_IsoScenarioFNOs = new List<short>(new short[] { 4, 5, 12, 14, 15, 34, 36, 59, 60, 98, 99 });
        private List<short> m_virtualPointFNOs = new List<short>(new short[] { 40, 80, 41, 81, 6, 82 });
        private List<short> m_transformerFNOs = new List<short>(new short[] { 34,59,98,60,99 });

        #endregion

        #region IGTFunctional Members

        public GTArguments Arguments
        {
            get
            {
                return m_GTArguments;
            }
            set
            {
                m_GTArguments = value;
            }
        }
        public string ComponentName
        {
            get
            {
                return m_gCompName;
            }
            set
            {
                m_gCompName = value;
            }
        }
        public IGTComponents Components
        {
            get
            {
                return m_gComps;
            }
            set
            {
                m_gComps = value;
            }
        }
        public IGTDataContext DataContext
        {
            get
            {
                return m_GTDataContext;
            }
            set
            {
                m_GTDataContext = value;
            }
        }
        public string FieldName
        {
            get
            {
                return m_gFieldName;
            }
            set
            {
                m_gFieldName = value.ToString();
            }
        }

        public IGTFieldValue FieldValueBeforeChange
        {
            get
            {
                return m_gFieldVal;
            }
            set
            {
                m_gFieldVal = value;
            }
        }

        public GTFunctionalTypeConstants Type
        {
            get
            {
                return m_gFIType;
            }
            set
            {
                m_gFIType = value;
            }
        }

        public void Delete()
        {

        }

        public void Execute()
        {
            IGTKeyObject activeFeature = null;
            IGTKeyObjects relatedFeatures = null;
            try
            {
                Recordset commonRS = m_gComps[m_gCompName].Recordset;

                if (commonRS != null)
                {
                    if (commonRS.RecordCount > 0 || !(commonRS.EOF && commonRS.BOF))
                    {
                        commonRS.MoveFirst();

                        CopyFeatureStateToConnectivity(commonRS);

                        if (!String.IsNullOrEmpty(m_connectivityTable) && m_gComps[m_connectivityTable] != null)
                        {
                            activeFeature = DataContext.OpenFeature(Convert.ToInt16(commonRS.Fields["G3E_FNO"].Value), Convert.ToInt32(commonRS.Fields["G3E_FID"].Value));

                            relatedFeatures = GetRelatedFeatures(activeFeature);

                            CheckForAnyRelatedVirtualPoint(relatedFeatures, activeFeature.FID, Convert.ToString(commonRS.Fields[m_gFieldName].Value));

                            m_featureStateBeforeChange = Convert.ToString(commonRS.Fields["FEATURE_STATE_C"].OriginalValue);
                            m_featureState = Convert.ToString(commonRS.Fields[m_gFieldName].Value);

                            // Fix for ALM-1586,1644,1557 - May need to revisit this code after getting the complete clarification for the comment in ONCORDEV-2284
                            // ALM  1817 - Set Normal and As-operated status when feature state is Closed as well
                            if (m_connectivityTable.Equals("CONNECTIVITY_N") && (m_featureState == "INI" || m_featureState == "CLS"))
                            {
                                SetNormalAndAsOperatedAttributes(activeFeature, relatedFeatures);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("There is an error in \"Feature State Transition\" Funtional Interface \n" + ex.Message, "G/Technology");
            }
        }
        public void Validate(out string[] ErrorPriorityArray, out string[] ErrorMessageArray)
        {
            ErrorPriorityArray = null;
            ErrorMessageArray = null;
        }
        #endregion

        #region Private Methods

        private void CopyFeatureStateToConnectivity(Recordset p_commonRS)
        {
            try
            {
                if (m_gComps["DUCTIVITY_N"] != null)
                {
                    if (m_gComps["DUCTIVITY_N"].Recordset != null)
                    {
                        if (m_gComps["DUCTIVITY_N"].Recordset.RecordCount > 0 || !(m_gComps["DUCTIVITY_N"].Recordset.EOF && m_gComps["DUCTIVITY_N"].Recordset.BOF))
                        {
                            m_connectivityTable = "DUCTIVITY_N";
                            m_gComps["DUCTIVITY_N"].Recordset.MoveFirst();
                            m_gComps["DUCTIVITY_N"].Recordset.Fields["FEATURE_STATE_C"].Value = Convert.ToString(p_commonRS.Fields[m_gFieldName].Value);
                        }
                    }
                }
                if (m_gComps["CONNECTIVITY_N"] != null)
                {
                    if (m_gComps["CONNECTIVITY_N"].Recordset != null)
                    {
                        if (m_gComps["CONNECTIVITY_N"].Recordset.RecordCount > 0 || !(m_gComps["CONNECTIVITY_N"].Recordset.EOF && m_gComps["CONNECTIVITY_N"].Recordset.BOF))
                        {
                            m_connectivityTable = "CONNECTIVITY_N";
                            m_gComps["CONNECTIVITY_N"].Recordset.MoveFirst();
                            m_gComps["CONNECTIVITY_N"].Recordset.Fields["FEATURE_STATE_C"].Value = Convert.ToString(p_commonRS.Fields[m_gFieldName].Value);
                        }
                    }
                }
            }
            catch
            {
                throw;
            }
        }
        private void CheckForAnyRelatedVirtualPoint(IGTKeyObjects p_relatedFeatures, int p_fID, string p_featureState)
        {
            int node1 = 0;
            int node2 = 0;
            int rnode1 = 0;
            int rnode2 = 0;
            try
            {
                foreach (IGTKeyObject relatedFeature in p_relatedFeatures)
                {
                    if (CheckAssociatedFeature(relatedFeature, p_fID))
                    {
                        if (((relatedFeature.FNO == 6 || relatedFeature.FNO == 82) && p_featureState == "INI") ||
                            ((relatedFeature.FNO == 40 || relatedFeature.FNO == 80) && (p_featureState == "OSR" || p_featureState == "OSA")))
                        {
                            node1 = Convert.ToInt32(m_gComps[m_connectivityTable].Recordset.Fields["NODE_1_ID"].Value);
                            node2 = Convert.ToInt32(m_gComps[m_connectivityTable].Recordset.Fields["NODE_2_ID"].Value);

                            IGTComponent relConnComp = relatedFeature.Components.GetComponent(m_gComps[m_connectivityTable].CNO);

                            if (relConnComp.Recordset != null)
                            {
                                GetRelatedNodes(relConnComp.Recordset, ref rnode1, ref rnode2);

                                StitchConnectivity(node1, node2, rnode1, rnode2, relatedFeature);
                            }

                            DeleteVirtualPoint(relatedFeature);
                        }                       
                    }
                }
            }
            catch
            {
                throw;
            }
        }

        private void StitchConnectivity(int node1, int node2, int rnode1, int rnode2, IGTKeyObject relatedFeature)
        {
            try
            {
                if ((node1 != 0 && (node1 == rnode1) && rnode2 != 0))
                {
                    ReconnectRelatedFeature(node1, rnode2, relatedFeature);
                }
                else if ((node2 != 0 && (node2 == rnode1) && rnode2 != 0))
                {
                    ReconnectRelatedFeature(node2, rnode2, relatedFeature);
                }
                else if ((node1 != 0 && (node1 == rnode2) && rnode1 != 0))
                {
                    ReconnectRelatedFeature(node1, rnode1, relatedFeature);
                }
                else if ((node2 != 0 && (node2 == rnode2) && rnode1 != 0))
                {
                    ReconnectRelatedFeature(node2, rnode1, relatedFeature);
                }
            }
            catch
            {
                throw;
            }
        }

        private void GetRelatedNodes(Recordset p_relConnRS,ref int rnode1,ref int rnode2)
        {
            try
            {
                p_relConnRS.MoveFirst();

                if (!String.IsNullOrEmpty(Convert.ToString(p_relConnRS.Fields["NODE_1_ID"].Value)))
                {
                    rnode1 = Convert.ToInt32(p_relConnRS.Fields["NODE_1_ID"].Value);
                }
                else
                {
                    rnode1 = 0;
                }
                if (!String.IsNullOrEmpty(Convert.ToString(p_relConnRS.Fields["NODE_2_ID"].Value)))
                {
                    rnode2 = Convert.ToInt32(p_relConnRS.Fields["NODE_2_ID"].Value);
                }
                else
                {
                    rnode2 = 0;
                }
            }
            catch
            {
                throw;
            }
        }
        private void DeleteVirtualPoint(IGTKeyObject relatedFeature)
        {
            Recordset compRS = null;
            try
            {
                foreach (IGTComponent comp in relatedFeature.Components)
                {
                    compRS = comp.Recordset;

                    if (compRS != null && compRS.RecordCount > 0)
                    {
                        compRS.MoveFirst();

                        while (!compRS.EOF)
                        {
                            compRS.Delete();
                            compRS.Update();
                            compRS.MoveNext();
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
        /// GetRelatedFeatures
        /// </summary>
        /// <param name="activeFeature"></param>
        /// <param name="cRNO"></param>
        /// <returns></returns>
        private IGTKeyObjects GetRelatedFeatures(IGTKeyObject activeFeature)
        {
            using (IGTRelationshipService relService = GTClassFactory.Create<IGTRelationshipService>())
            {
                relService.DataContext = DataContext;
                relService.ActiveFeature = activeFeature;
                IGTKeyObjects relatedFeatures = GTClassFactory.Create<IGTKeyObjects>();

                try
                {
                    relatedFeatures = relService.GetRelatedFeatures(m_cRNO);
                }
                catch (Exception)
                {

                }

                return relatedFeatures;
            }
        }

        /// <summary>
        /// ReconnectRelatedFeature: Reconnects the node of the active feature to the other node feature of Virtual point.
        /// </summary>
        /// <param name="a_node">Active node ID common for active feature and Virtual point</param>
        /// <param name="r_node">The other node ID of the Virtual point which is not common with active feature</param>
        /// <param name="relatedFeature">Related Virtual point Feature</param>
        private void ReconnectRelatedFeature(int a_nodeID, int r_nodeID, IGTKeyObject relatedFeature)
        {
            Recordset connectRs = null;
            IGTKeyObjects relatedFeatures = null;
            try
            {
                relatedFeatures = GetRelatedFeatures(relatedFeature);

                for (int i = 0; i < relatedFeatures.Count; i++)
                {
                    if (relatedFeatures[i].FID != Convert.ToInt32(m_gComps[m_connectivityTable].Recordset.Fields["g3e_fid"].Value))
                    {
                        connectRs = relatedFeatures[i].Components.GetComponent(m_gComps[m_connectivityTable].CNO).Recordset;

                        if (connectRs != null)
                        {
                            if (connectRs.RecordCount > 0)
                            {
                                connectRs.MoveFirst();
                                if (Convert.ToInt32(connectRs.Fields["NODE_1_ID"].Value) == r_nodeID)
                                {
                                   connectRs.Fields["NODE_1_ID"].Value = a_nodeID;
                                }
                                else if (Convert.ToInt32(connectRs.Fields["NODE_2_ID"].Value) == r_nodeID)
                                {
                                    connectRs.Fields["NODE_2_ID"].Value = a_nodeID;
                                }
                                connectRs.Update();
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

        /// <summary>
        /// CheckAssociatedFeature: Check whether the active feature is present as ASSOCIATED_FID for virtual point.
        /// </summary>
        /// <param name="relatedFeature">Virtaul point Features related to active feature</param>
        /// <param name="activeFID">FID of the active feature</param>
        /// <returns></returns>
        private bool CheckAssociatedFeature(IGTKeyObject relatedFeature, Int32 activeFID)
        {
            IGTComponent virtualComponent = null;
            bool associated = false;
            try
            {
                virtualComponent = relatedFeature.Components.GetComponent(m_virtualAttributesCNO);

                if (virtualComponent != null)
                {
                    if (virtualComponent.Recordset != null && virtualComponent.Recordset.RecordCount > 0)
                    {
                        virtualComponent.Recordset.MoveFirst();
                        if (Convert.ToInt32(virtualComponent.Recordset.Fields["ASSOCIATED_FID"].Value == DBNull.Value ? 0.0 : virtualComponent.Recordset.Fields["ASSOCIATED_FID"].Value) == activeFID && Convert.ToChar(virtualComponent.Recordset.Fields["PERMANENT_YN"].Value) == 'N')
                        {
                            associated = true;
                        }
                    }
                }
            }
            catch
            {
                throw;
            }
            return associated;
        }

        /// <summary>
        ///  Method to get the primary graphic cno for fno
        /// </summary>
        /// <param name="fNo"></param>
        /// <returns>cno or 0</returns>
        private short GetPrimaryGraphicCno(short fNo)
        {
            short primaryGraphicCno = 0;
            try
            {
                Recordset tempRs = m_GTDataContext.MetadataRecordset("G3E_FEATURES_OPTABLE", "G3E_FNO = " + fNo);
                if (tempRs != null && tempRs.RecordCount > 0)
                {
                    tempRs.MoveFirst();
                    primaryGraphicCno = Convert.ToInt16(tempRs.Fields["G3E_PRIMARYGEOGRAPHICCNO"].Value);
                }
                return primaryGraphicCno;
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Method to set the Normal and As-Operated status for features in INI and CLS states
        /// </summary>
        /// <param name="activeFeature"></param>
        /// <param name="relatedFeatures"></param>
        private void SetNormalAndAsOperatedAttributes(IGTKeyObject activeFeature, IGTKeyObjects relatedFeatures)
        {
            Dictionary<int, short> relatedIsolationPoints = new Dictionary<int, short>();
            string normalStatus = string.Empty;
            string asOperatedStatus = string.Empty;

            try
            {
                // Set status attributes for active feature irrespective of feature class
                // ALM-2356-JIRA-2798
                
                // ALM-2356 change complete
                IGTComponent connectivityComponent = activeFeature.Components.GetComponent(11);
                if (connectivityComponent == null)
                    return;
                Recordset connectivityRs = connectivityComponent.Recordset;
                normalStatus = Convert.ToString(connectivityRs.Fields["STATUS_NORMAL_C"].Value);
                asOperatedStatus = Convert.ToString(connectivityRs.Fields["STATUS_OPERATED_C"].Value);

                // If feature state is changed from INI to CLS, do nothing
                if (m_featureStateBeforeChange == "INI" && m_featureState == "CLS")
                { }
                else
                {
                    if (asOperatedStatus != normalStatus)
                    {
                        connectivityRs.Fields["STATUS_OPERATED_C"].Value = normalStatus;
                    }
                }
                // ALM-2360-JIRA-2799 - when feature state transitions to INI or CLS, 
                // copy the affected proposed voltages to the actual voltage attributes (Voltage 2 for Transformers, and both Voltage 1 & 2 for Autotransformers).
                // ALM affects Auto Transformer, Transformer OH and OH Network, Transformer UG and UG Network features

                if (m_transformerFNOs.Contains(activeFeature.FNO))
                {
                    if (activeFeature.FNO == 34) // Autotransformer
                    {
                        connectivityRs.Fields["VOLT_1_Q"].Value = connectivityRs.Fields["PP_VOLT_1_Q"].Value;
                        connectivityRs.Fields["VOLT_2_Q"].Value = connectivityRs.Fields["PP_VOLT_2_Q"].Value;
                    }
                    else // Other transformers
                    {
                        connectivityRs.Fields["VOLT_2_Q"].Value = connectivityRs.Fields["PP_VOLT_2_Q"].Value;
                    }
                }

                // Set status attributes for associated isolation points incase of Isolation scenario features
                if (m_IsoScenarioFNOs.Contains(activeFeature.FNO))
                {
                    // update statuses for inline isolation points. Inlines isolation points are related/connected to active feature.
                    foreach (IGTKeyObject relatedFeature in relatedFeatures)
                    {
                        if (m_virtualPointFNOs.Contains(relatedFeature.FNO))
                        {
                            relatedIsolationPoints.Add(relatedFeature.FID, relatedFeature.FNO);
                            Recordset relatedFeatureRs = relatedFeature.Components.GetComponent(11).Recordset;
                            if (relatedFeatureRs.RecordCount > 0)
                            {
                                relatedFeatureRs.MoveFirst();
                                normalStatus = Convert.ToString(relatedFeatureRs.Fields["STATUS_NORMAL_C"].Value);
                                asOperatedStatus = Convert.ToString(relatedFeatureRs.Fields["STATUS_OPERATED_C"].Value);
                                if (!asOperatedStatus.Equals(normalStatus))
                                {
                                    relatedFeatureRs.Fields["STATUS_OPERATED_C"].Value = normalStatus;
                                }
                            }
                        }
                    }


                    // Find out whether there is any third isolation point. The Recloser would have a third isolation point (true Bypass point) that is not connected to the Recloser.
                    // We will find out the true Bypass point using Zone and Spatial service and if it exists we will update the statuses.
                    // For some active features ,say Transformer UG, there are NO isolation points but they do have other connected features which are not isolated points.
                    // We want to consider only related isolation points leaving out the rest of the connected features

                    if (relatedIsolationPoints.Count != 0)
                    {
                        foreach (KeyValuePair<int, short> feature in relatedIsolationPoints)
                        {
                            IGTOrientedPointGeometry gTOrientedPointGeometry = GTClassFactory.Create<IGTOrientedPointGeometry>();
                            gTOrientedPointGeometry.Origin = ((IGTOrientedPointGeometry)activeFeature.Components.GetComponent(GetPrimaryGraphicCno(activeFeature.FNO)).Geometry).Origin;

                            IGTZoneService gTZoneService = GTClassFactory.Create<IGTZoneService>();
                            gTZoneService.ZoneWidth = 4;
                            gTZoneService.InputGeometries = gTOrientedPointGeometry;
                            IGTGeometry outPutGeometry = gTZoneService.OutputGeometries;

                            IGTSpatialService gTSpatialService = GTClassFactory.Create<IGTSpatialService>();
                            gTSpatialService.DataContext = m_GTDataContext;
                            gTSpatialService.Operator = GTSpatialOperatorConstants.gtsoEntirelyContains;
                            gTSpatialService.FilterGeometry = outPutGeometry;

                            Recordset resultRecordSet = gTSpatialService.GetResultsByFNO(new short[] { feature.Value });
                            if (resultRecordSet != null && resultRecordSet.RecordCount > 0)
                            {
                                // the recordset contains the isolated points that are within the zone and hence contains already connected isolation points

                                resultRecordSet.MoveFirst();
                                while (!resultRecordSet.EOF)
                                {
                                    int fid = Convert.ToInt32(resultRecordSet.Fields["G3E_FID"].Value);
                                    short fno = Convert.ToInt16(resultRecordSet.Fields["G3E_FNO"].Value);

                                    // we would like to consider the isolation point that is not connected to the active feature, but it is part of the active feature.
                                    if (!relatedIsolationPoints.ContainsKey(fid))
                                    {
                                        Recordset thirdIsolationPoint = m_GTDataContext.OpenFeature(fno, fid).Components.GetComponent(11).Recordset;
                                        if (thirdIsolationPoint != null && thirdIsolationPoint.RecordCount > 0)
                                        {
                                            thirdIsolationPoint.MoveFirst();
                                            normalStatus = Convert.ToString(thirdIsolationPoint.Fields["STATUS_NORMAL_C"].Value);
                                            asOperatedStatus = Convert.ToString(thirdIsolationPoint.Fields["STATUS_OPERATED_C"].Value);
                                            if (!asOperatedStatus.Equals(normalStatus))
                                            {
                                                thirdIsolationPoint.Fields["STATUS_OPERATED_C"].Value = normalStatus;
                                            }
                                        }
                                    }
                                    resultRecordSet.MoveNext();
                                }

                            }
                        }
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion
    }
}
