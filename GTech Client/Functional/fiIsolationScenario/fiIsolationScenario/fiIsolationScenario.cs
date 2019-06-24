// ===================================================
//  Copyright 2017 Intergraph Corp.
//  File Name: fiIsolationScenario.cs
// 
//  Description:   Validates isolation scenarios, which specify how many virtual points of which types should be connected to the affected feature.
//  Remarks: 
//  Modification History
//  Date                Author                      Comments
//  --------            --------------              --------
//  25/09/2017          Prathyusha                  Created 
// ======================================================
using System;
using System.Collections.Generic;
using System.Linq;
using ADODB;
using Intergraph.GTechnology.API;
using Intergraph.GTechnology.Interfaces;
using System.Windows.Forms;
using gtCommandLogger;

namespace GTechnology.Oncor.CustomAPI
{
    public class fiIsolationScenario : IGTFunctional
    {
        #region Private Members
        private GTArguments m_GTArguments = null;
        private IGTDataContext m_GTDataContext = null;
        private string m_gCompName = string.Empty;
        private IGTComponents m_gComps = null;
        private IGTFieldValue m_gFieldVal = null;
        private string m_gFieldName = string.Empty;
        private GTFunctionalTypeConstants m_gFIType;

        private const short m_cCNO = 11;
        private const short m_cRNO = 14;

        private List<short> m_IsoScenarioFNOs = new List<short>(new short[] { 4, 5, 12, 14, 15, 34, 36, 59, 60, 98, 99 });
        private enum IsolationScenarios { ISOSINGLE, ISODUAL, ELBOW, BYPASS, NULL };

        private bool m_InteractiveMode = false;
        private short m_ActiveFNO = 0;
        private Int32 m_ActiveFID = 0;

        int m_aNode1 = 0;
        int m_aNode2 = 0;

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
                m_gFieldName = value;
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
            try
            {
                // If G/Tech is not running in interactive mode, then skip Message Box displays
                GUIMode guiMode = new GUIMode();
                m_InteractiveMode = guiMode.InteractiveMode;

                m_ActiveFNO = Convert.ToInt16(Components[ComponentName].Recordset.Fields["G3E_FNO"].Value);
                m_ActiveFID = Convert.ToInt32(Components[ComponentName].Recordset.Fields["G3E_FID"].Value);

                bool isIsolationScenario = false;
                IsolationScenarios isolationScenarioType = IsolationScenarios.NULL;

                if (m_IsoScenarioFNOs.Contains(m_ActiveFNO))
                {
                    // Check if active feature is an Isolation Scenario feature.
                    if (CheckIsoScenarioFeature(m_gComps, m_ActiveFNO, ref isIsolationScenario, ref isolationScenarioType))
                    {
                        if (isIsolationScenario)
                        {
                            IGTKeyObject isoScenarioFeatureKO = DataContext.OpenFeature(m_ActiveFNO, m_ActiveFID);
                            ProcessIsoScenarioFeature(isoScenarioFeatureKO, isolationScenarioType);
                        }
                    }
                }
                else
                // Check if active feature is connected to feature that has Isolation Scenario
                {
                    Int32 node1 = Convert.ToInt32(Components.GetComponent(11).Recordset.Fields["NODE_1_ID"].Value);
                    Int32 node2 = Convert.ToInt32(Components.GetComponent(11).Recordset.Fields["NODE_2_ID"].Value);

                    string sql = "select conn.g3e_fno, conn.g3e_fid from connectivity_n conn " +
                                 "where (((conn.node_1_id = ? or conn.node_1_id = ?) and conn.node_1_id <> 0) or ((conn.node_2_id = ? or conn.node_2_id = ?) and conn.node_2_id <> 0)) " +
                                 "and conn.g3e_fno in (4,5,12,14,15,34,36,59,60,98,99)";

                    Recordset connRs = DataContext.OpenRecordset(sql, CursorTypeEnum.adOpenStatic, LockTypeEnum.adLockReadOnly, (int)CommandTypeEnum.adCmdText,
                                                                 node1, node2, node1, node2);

                    if (connRs.RecordCount > 0)
                    {
                        connRs.MoveFirst();

                        IGTKeyObject isoScenarioKO = null;
                        short isoScenarioFNO = Convert.ToInt16(connRs.Fields["G3E_FNO"].Value);
                        Int32 isoScenarioFID = Convert.ToInt32(connRs.Fields["G3E_FID"].Value);

                        while (!connRs.EOF)
                        {
                            isoScenarioFNO = Convert.ToInt16(connRs.Fields["G3E_FNO"].Value);
                            isoScenarioFID = Convert.ToInt32(connRs.Fields["G3E_FID"].Value);

                            isoScenarioKO = DataContext.OpenFeature(isoScenarioFNO, isoScenarioFID);
                            // Check if active feature is an Isolation Scenario feature.
                            if (CheckIsoScenarioFeature(isoScenarioKO.Components, isoScenarioFNO, ref isIsolationScenario, ref isolationScenarioType))
                            {
                                if (isIsolationScenario)
                                {
                                    ProcessIsoScenarioFeature(isoScenarioKO, isolationScenarioType);
                                }
                            }
                            connRs.MoveNext();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                GUIMode guiMode = new GUIMode();
                if (guiMode.InteractiveMode)
                {
                    MessageBox.Show("Error in Isolation Scenario FI:Execute - " + ex.Message, "G/Technology", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }
            }
        }

        /// <summary>
        /// Validate
        /// </summary>
        /// <param name="ErrorPriorityArray"></param>
        /// <param name="ErrorMessageArray"></param>
        public void Validate(out string[] ErrorPriorityArray, out string[] ErrorMessageArray)
        {
            ErrorPriorityArray = new string[1];
            ErrorMessageArray = new string[1];

            GTValidationLogger gTValidationLogger = null;
            IGTComponent comp = Components[ComponentName];
            int FID = 0;

            string fieldValue = string.Empty;

            if (comp != null && comp.Recordset != null && comp.Recordset.RecordCount > 0)
            {
                FID = int.Parse(comp.Recordset.Fields["G3E_FID"].Value.ToString());
                fieldValue = Convert.ToString(comp.Recordset.Fields[FieldName].Value);
            }

            if (new gtLogHelper().CheckIfLoggingIsEnabled())
            {
                LogEntries logEntries = new LogEntries
                {
                    ActiveComponentName = ComponentName,
                    ActiveFID = FID,
                    ActiveFieldName = FieldName,
                    ActiveFieldValue = fieldValue,
                    JobID = DataContext.ActiveJob,
                    RelatedComponentName = "N/A",
                    RelatedFID = 0,
                    RelatedFieldName = "N/A",
                    RelatedFieldValue = "N/A",
                    ValidationInterfaceName = "Validate Isolation",
                    ValidationInterfaceType = "FI"
                };
                gTValidationLogger = new GTValidationLogger(logEntries);

                gTValidationLogger.LogEntry("TIMING", "START", "Validate Isolation Entry", "N/A", "");
            }

            try
            {
                short activeFNO = Convert.ToInt16(Components[ComponentName].Recordset.Fields["G3E_FNO"].Value);

                bool isIsolationScenario = false;
                IsolationScenarios isolationScenarioType = IsolationScenarios.NULL;

                if (m_IsoScenarioFNOs.Contains(activeFNO))
                {
                    // Check if active feature is an Isolation Scenario feature.
                    if (CheckIsoScenarioFeature(m_gComps, activeFNO, ref isIsolationScenario, ref isolationScenarioType))
                    {
                        if (isIsolationScenario)
                        {
                            string errorPriority = Convert.ToString(Arguments.GetArgument(0));

                            string activeFeatureState = null;
                            string feedType = null;
                            short numberofNodes = 0;

                            IGTKeyObject m_activeFeature = GTClassFactory.Create<IGTKeyObject>();
                            IGTKeyObject relativeFeature = GTClassFactory.Create<IGTKeyObject>();

                            IGTKeyObjects m_relatedNode1Features = null;
                            IGTKeyObjects m_relatedNode2Features = null;
                            Recordset activeFeatureConnRS = null;

                            List<IGTKeyObject> node1FeaturesLst = new List<IGTKeyObject>();
                            List<IGTKeyObject> node2FeaturesLst = new List<IGTKeyObject>();

                            m_activeFeature = DataContext.OpenFeature(Convert.ToInt16(Components[ComponentName].Recordset.Fields["G3E_FNO"].Value), Convert.ToInt32(Components[ComponentName].Recordset.Fields["G3E_FID"].Value));

                            GetNumberOfNodes(m_activeFeature, ref numberofNodes);

                            m_relatedNode1Features = GetRelatedFeatures(m_activeFeature, m_cRNO, "NODE1");

                            if (numberofNodes == 2)
                            {
                                m_relatedNode2Features = GetRelatedFeatures(m_activeFeature, m_cRNO, "NODE2");
                            }

                            activeFeatureConnRS = m_activeFeature.Components.GetComponent(m_cCNO).Recordset;

                            if (activeFeatureConnRS.RecordCount > 0)
                            {
                                activeFeatureConnRS.MoveFirst();

                                m_aNode1 = Convert.ToInt32(activeFeatureConnRS.Fields["NODE_1_ID"].Value == System.DBNull.Value ? 0 : activeFeatureConnRS.Fields["NODE_1_ID"].Value);
                                m_aNode2 = Convert.ToInt32(activeFeatureConnRS.Fields["NODE_2_ID"].Value == System.DBNull.Value ? 0 : activeFeatureConnRS.Fields["NODE_2_ID"].Value);

                                if (m_activeFeature.Components.GetComponent(1).Recordset != null && m_activeFeature.Components.GetComponent(1).Recordset.RecordCount > 0)
                                {
                                    m_activeFeature.Components.GetComponent(1).Recordset.MoveFirst();
                                    activeFeatureState = Convert.ToString(m_activeFeature.Components.GetComponent(1).Recordset.Fields["FEATURE_STATE_C"].Value);
                                }
                                if (m_activeFeature.FNO == 59 && m_activeFeature.Components.GetComponent(5901).Recordset != null && m_activeFeature.Components.GetComponent(5901).Recordset.RecordCount > 0)
                                {
                                    m_activeFeature.Components.GetComponent(5901).Recordset.MoveFirst();
                                    feedType = Convert.ToString(m_activeFeature.Components.GetComponent(5901).Recordset.Fields["FEED_TYPE"].Value);
                                }
                                else if (m_activeFeature.FNO == 60 && m_activeFeature.Components.GetComponent(6002).Recordset != null && m_activeFeature.Components.GetComponent(6002).Recordset.RecordCount > 0)
                                {
                                    m_activeFeature.Components.GetComponent(6002).Recordset.MoveFirst();
                                    feedType = Convert.ToString(m_activeFeature.Components.GetComponent(6002).Recordset.Fields["FEED_TYPE"].Value);
                                }
                            }

                            if (CheckFeatureState(activeFeatureState))
                            {
                                if (m_relatedNode1Features != null)
                                {
                                    foreach (IGTKeyObject feature in m_relatedNode1Features)
                                    {
                                        node1FeaturesLst.Add(feature);
                                    }
                                }

                                if (m_relatedNode2Features != null)
                                {
                                    foreach (IGTKeyObject feature in m_relatedNode2Features)
                                    {
                                        node2FeaturesLst.Add(feature);
                                    }
                                }

                                //ISODUAL
                                if (m_activeFeature.FNO == 34) //Autotransformer
                                {
                                    IProcessIsolationScenario isoDual = new IsoDualScenario(node1FeaturesLst.Where(a => a.FNO == 6).ToList(), node2FeaturesLst.Where(a => a.FNO == 6).ToList(), errorPriority);
                                    isoDual.ProcessIsolationScenario(out ErrorMessageArray[0], out ErrorPriorityArray[0]);
                                }

                                //ISOSINGLE
                                if (m_activeFeature.FNO == 4 || m_activeFeature.FNO == 12 || m_activeFeature.FNO == 59 || (m_activeFeature.FNO == 60 && feedType.ToUpper() == "RADIAL"))
                                {
                                    IProcessIsolationScenario isoSingle = new IsoSingleScenario(node1FeaturesLst.Where(a => a.FNO == 6).ToList(), node2FeaturesLst.Where(a => a.FNO == 6).ToList(), errorPriority);
                                    isoSingle.ProcessIsolationScenario(out ErrorMessageArray[0], out ErrorPriorityArray[0]);
                                }

                                //ISOSINGLE NETWORK FEATURES
                                if (m_activeFeature.FNO == 99 || m_activeFeature.FNO == 98)
                                {
                                    IProcessIsolationScenario isoSingle = new IsoSingleScenario(node1FeaturesLst.Where(a => a.FNO == 82).ToList(), node2FeaturesLst.Where(a => a.FNO == 82).ToList(), errorPriority);
                                    isoSingle.ProcessIsolationScenario(out ErrorMessageArray[0], out ErrorPriorityArray[0]);
                                }

                                //ELBOW need clarification
                                if (m_activeFeature.FNO == 5 || (m_activeFeature.FNO == 60 && feedType.ToUpper() == "LOOP"))
                                {
                                    IProcessIsolationScenario elbowScenario = new ElbowScenario(node1FeaturesLst.Where(a => a.FNO == 41).ToList(), node2FeaturesLst.Where(a => a.FNO == 41).ToList(), errorPriority);
                                    elbowScenario.ProcessIsolationScenario(out ErrorMessageArray[0], out ErrorPriorityArray[0]);
                                }

                                //BYPASS
                                if (m_activeFeature.FNO == 14 || m_activeFeature.FNO == 15 || m_activeFeature.FNO == 36)
                                {
                                    IProcessIsolationScenario byPassScenario = new ByPassScenario(node1FeaturesLst.Where(a => a.FNO == 40).ToList(), node2FeaturesLst.Where(a => a.FNO == 40).ToList(), errorPriority);
                                    byPassScenario.ProcessIsolationScenario(out ErrorMessageArray[0], out ErrorPriorityArray[0]);
                                }
                            }
                        }
                    }
                }
                if(gTValidationLogger != null )
                    gTValidationLogger.LogEntry("TIMING", "END ", "Validate Isolation Exit", "N/A", "");
            }
            catch (Exception ex)
            {
                throw new Exception("Error from Isolation Scenario FI: " + ex.Message);
            }
        }
        #endregion

        #region Private Methods
        /// <summary>
        ///  Method to check the for valid feature state to throw the validation
        /// </summary>
        /// <param name="p_featureState"></param>
        /// <returns>True, if its a valid feature state to throw the validation</returns>
        private bool CheckFeatureState(string p_featureState)
        {
            switch (p_featureState)
            {
                case "PPI":
                case "ABI":
                case "INI":
                case "CLS":
                    return true;
                default:
                    return false;
            }
        }

        /// <summary>
        /// Returns the number of nodes present for feature for connectivity relationship.
        /// </summary>
        /// <param name="activeFeature"></param>
        /// <returns></returns>
        private void GetNumberOfNodes(IGTKeyObject activeFeature, ref short numberofNodes)
        {
            try
            {
                Recordset oRSFeature = DataContext.MetadataRecordset("G3E_FEATURES_OPTABLE", "G3E_FNO=" + activeFeature.FNO);
               // oRSFeature.Filter = "G3E_FNO=" + activeFeature.FNO;

                numberofNodes = Convert.ToInt16(oRSFeature.Fields["G3E_NUMBEROFNODES"].Value);
            }
            catch
            {
                throw;
            }
        }
        /// <summary>
        /// Method to return the RelatedFeatures of active feature
        /// </summary>
        /// <param name="activeFeature"></param>
        /// <param name="cRNO"></param>
        /// <returns></returns>
        private IGTKeyObjects GetRelatedFeatures(IGTKeyObject p_activeFeature, short p_cRNO, string nodeOrdinal)
        {
            using (IGTRelationshipService m_relService = GTClassFactory.Create<IGTRelationshipService>())
            {
                IGTKeyObjects m_relatedFeatures = null;

                m_relService.DataContext = DataContext;
                m_relService.ActiveFeature = p_activeFeature;

                if (nodeOrdinal == "NODE1")
                {
                    m_relatedFeatures = m_relService.GetRelatedFeatures(p_cRNO, GTRelationshipOrdinalConstants.gtrelRelationshipOrdinal1);
                }
                else if (nodeOrdinal == "NODE2")
                {
                    m_relatedFeatures = m_relService.GetRelatedFeatures(p_cRNO, GTRelationshipOrdinalConstants.gtrelRelationshipOrdinal2);
                }
                return m_relatedFeatures;
            }
        }

        /// <summary>
        ///  Method to check if the feature is an Isolation Scenario feature.
        /// </summary>
        /// <param name="gtComps">IGTComponents used to check for specific attribute values</param>
        /// <param name="fno">Feature number to check</param>
        /// <param name="isIsolationScenario">Indicates if feature takes part in an Isolation Scenario</param>
        /// <param name="isolationScenarioType">Type of Isolation Scenario. Set if isIsolationScenario is true</param>
        /// <returns>Boolean indicating method execution status</returns>
        private bool CheckIsoScenarioFeature(IGTComponents gtComps, short fno, ref bool isIsolationScenario, ref IsolationScenarios isolationScenarioType)
        {
            try
            {
                string feedType = string.Empty;
                if (fno == 59 && gtComps.GetComponent(5901).Recordset != null && gtComps.GetComponent(5901).Recordset.RecordCount > 0)
                {
                    gtComps.GetComponent(5901).Recordset.MoveFirst();
                    feedType = Convert.ToString(gtComps.GetComponent(5901).Recordset.Fields["FEED_TYPE"].Value);
                }
                else if (fno == 60 && gtComps.GetComponent(6002).Recordset != null && gtComps.GetComponent(6002).Recordset.RecordCount > 0)
                {
                    gtComps.GetComponent(6002).Recordset.MoveFirst();
                    feedType = Convert.ToString(gtComps.GetComponent(6002).Recordset.Fields["FEED_TYPE"].Value);
                }

                //ISODUAL
                if (fno == 34) //Autotransformer
                {
                    isIsolationScenario = true;
                    isolationScenarioType = IsolationScenarios.ISODUAL;
                }
                //ISOSINGLE
                else if (fno == 4 || fno == 12 || fno == 59 || (fno == 60 && feedType.ToUpper() == "RADIAL"))
                {
                    isIsolationScenario = true;
                    isolationScenarioType = IsolationScenarios.ISOSINGLE;
                }
                //ISOSINGLE NETWORK FEATURES
                else if (fno == 99 || fno == 98)
                {
                    isIsolationScenario = true;
                    isolationScenarioType = IsolationScenarios.ISOSINGLE;
                }
                //ELBOW
                else if (fno == 5 || (fno == 60 && feedType.ToUpper() == "LOOP"))
                {
                    isIsolationScenario = true;
                    isolationScenarioType = IsolationScenarios.ELBOW;
                }
                //BYPASS
                else if (fno == 14 || fno == 15 || fno == 36)
                {
                    isIsolationScenario = true;
                    isolationScenarioType = IsolationScenarios.BYPASS;
                }
                else
                {
                    isIsolationScenario = false;
                }

                return true;
            }
            catch (Exception ex)
            {
                throw new Exception("Error from Isolation Scenario FI:CheckIsoScenarioFeature: " + ex.Message);
            }
        }

        /// <summary>
        ///  Method to process the Isolation Scenario feature.
        /// </summary>
        /// <param name="gtComps">IGTKeyObject of the Isolation Scenario feature</param>
        /// <param name="isolationScenarioType">Type of Isolation Scenario</param>
        /// <returns>Boolean indicating method execution status</returns>
        private bool ProcessIsoScenarioFeature(IGTKeyObject gtKO, IsolationScenarios isolationScenarioType)
        {
            try
            {
                IsoScenarioFeature activeFeature = new IsoScenarioFeature();
                activeFeature.GtKeyObject = gtKO;

                short numberofNodes = 0;
                GetNumberOfNodes(activeFeature.GtKeyObject, ref numberofNodes);

                activeFeature.RelatedFeaturesNode1 = GetRelatedFeatures(activeFeature.GtKeyObject, m_cRNO, "NODE1");

                if (numberofNodes == 2)
                {
                    activeFeature.RelatedFeaturesNode2 = GetRelatedFeatures(activeFeature.GtKeyObject, m_cRNO, "NODE2");
                }

                Recordset connRS = activeFeature.GtKeyObject.Components.GetComponent(11).Recordset;
                if (connRS.RecordCount > 0)
                {
                    connRS.MoveFirst();
                    activeFeature.Phase = connRS.Fields["PHASE_ALPHA"].Value.ToString();
                }

                Recordset commRS = activeFeature.GtKeyObject.Components.GetComponent(1).Recordset;
                if (commRS.RecordCount > 0)
                {
                    commRS.MoveFirst();
                    activeFeature.FeatureState = commRS.Fields["FEATURE_STATE_C"].Value.ToString();
                }

                IsoCommon isoCommon = new IsoCommon();
                isoCommon.ActiveFNO = m_ActiveFNO;
                isoCommon.ActiveFID = m_ActiveFID;
                isoCommon.InteractiveMode = m_InteractiveMode;
                isoCommon.DataContext = m_GTDataContext;

                // ISOSINGLE
                if (isolationScenarioType == IsolationScenarios.ISOSINGLE)
                {
                    IProcessIsolationScenario isoSingle = new IsoSingleScenario(m_GTDataContext, activeFeature, isoCommon);
                    isoSingle.ValidateIsolationScenario();
                }
                // ISODUAL
                else if (isolationScenarioType == IsolationScenarios.ISODUAL)
                {
                    IProcessIsolationScenario isoDual = new IsoDualScenario(m_GTDataContext, activeFeature, isoCommon);
                    isoDual.ValidateIsolationScenario();
                }
                // ISOELBOW
                else if (isolationScenarioType == IsolationScenarios.ELBOW)
                {
                    IProcessIsolationScenario isoElbow = new ElbowScenario(m_GTDataContext, activeFeature, isoCommon);
                    isoElbow.ValidateIsolationScenario();
                }
                // ISOELBOW
                else if (isolationScenarioType == IsolationScenarios.BYPASS)
                {
                    IProcessIsolationScenario isoBypass = new ByPassScenario(m_GTDataContext, activeFeature, isoCommon);
                    isoBypass.ValidateIsolationScenario();
                }

                return true;
            }
            catch (Exception ex)
            {
                throw new Exception("Error from Isolation Scenario FI:ProcessIsoScenarioFeature: " + ex.Message);
            }
        }

        #endregion

    }
}
