// ===================================================
//  Copyright 2017 Intergraph Corp.
//  File Name: fiPhaseAgreement.cs
// 
//  Description:   Phase attributes for a given feature and those features it is connected are validated using this interface.
//  Remarks: 
//  Modification History
//  Date                Author                      Comments
//  --------            --------------              --------
//  07/03/2017          Uma Avote                   Created 
//  22/09/2017          Prathyusha                  Modified 
//  12/06/2018          Prathyusha                  Refactored code as per the Connectivity Helper Changes 
// ======================================================
using System;
using Intergraph.GTechnology.Interfaces;
using Intergraph.GTechnology.API;
using System.Collections.Generic;
using System.Linq;
using ADODB;
using gtCommandLogger;

namespace GTechnology.Oncor.CustomAPI
{
    class fiPhaseAgreement : IGTFunctional
    {
        #region Private Members
        private GTArguments m_GTArguments = null;
        private IGTDataContext m_GTDataContext = null;
        private string m_gCompName = string.Empty;
        private IGTComponents m_gComps = null;
        private IGTFieldValue m_gFieldVal = null;
        private string m_gFieldName = string.Empty;
        private GTFunctionalTypeConstants m_gFIType;
        private Recordset m_affectedConnectivityRs = null;

        string m_activeFeaturePhase;
        int m_aNode1 = 0;
        int m_aNode2 = 0;
        short m_numberofNodes = 0;

        //constant  
        private const string ATTRIB_FID = "G3E_FID";
        private const string ATTRIB_FNO = "G3E_FNO";
        private const string ATTRIB_NODE1 = "NODE_1_ID";
        private const string ATTRIB_NODE2 = "NODE_2_ID";
        private const string ATTRIB_PHASE = "PHASE_ALPHA";
        private const short m_cRNO = 14;
        private const short m_cCNO = 11;

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

        }

        /// <summary>
        /// Validate
        /// </summary>
        /// <param name="ErrorPriorityArray"></param>
        /// <param name="ErrorMessageArray"></param>
        public void Validate(out string[] ErrorPriorityArray, out string[] ErrorMessageArray)
        {

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
                    ValidationInterfaceName = "Phase Agreement",
                    ValidationInterfaceType = "FI"
                };

                gTValidationLogger = new GTValidationLogger(logEntries);

                gTValidationLogger.LogEntry("TIMING", "START", "Phase Agreement Entry", "N/A", "");
            }

            ErrorPriorityArray = null;
            ErrorMessageArray = null;

            IGTKeyObject affectedFeatureKeyObject = GTClassFactory.Create<IGTKeyObject>();
            IGTKeyObjects relatedFeaturesKeyObjects = GTClassFactory.Create<IGTKeyObjects>();

            List<char> downstreamPhaseList = new List<char>();
            List<char> upstreamPhaseList = new List<char>();
            List<char> insidePhaseList = new List<char>();
            List<char> parallelPhaseList = new List<char>();
            List<char> outsidePhaseList = new List<char>();

            List<string> errorMessage = new List<string>();
            List<string> errorPriority = new List<string>();

            GTClassFactory.Create<IGTApplication>().SetStatusBarText(GTStatusPanelConstants.gtaspcMessage, "Phase Agreement FI");

            try
            {
                if (!CheckAffectedFeatureIsSecondary()) // Check for affected feature is secondary or not
                {
                    if (m_affectedConnectivityRs.RecordCount > 0)
                    {
                        m_affectedConnectivityRs.MoveFirst();

                        m_aNode1 = Convert.ToInt32(m_affectedConnectivityRs.Fields[ATTRIB_NODE1].Value == DBNull.Value ? 0.0 : m_affectedConnectivityRs.Fields[ATTRIB_NODE1].Value);

                        m_aNode2 = Convert.ToInt32(m_affectedConnectivityRs.Fields[ATTRIB_NODE2].Value == DBNull.Value ? 0.0 : m_affectedConnectivityRs.Fields[ATTRIB_NODE2].Value);

                        if (m_aNode1 != 0 || m_aNode2 != 0)
                        {
                            m_activeFeaturePhase = Convert.ToString(m_affectedConnectivityRs.Fields[ATTRIB_PHASE].Value);

                            affectedFeatureKeyObject = DataContext.OpenFeature(Convert.ToInt16(Components[ComponentName].Recordset.Fields[ATTRIB_FNO].Value), Convert.ToInt32(Components[ComponentName].Recordset.Fields[ATTRIB_FID].Value));

                            // Gets the related features of affected feature.
                            relatedFeaturesKeyObjects = GetRelatedFeatures(affectedFeatureKeyObject, m_cRNO);

                            GetNumberOfNodes(affectedFeatureKeyObject);

                            if (m_numberofNodes == 2)
                            {
                                ConnectivityFactory connecFactory = new ConnectivityHelperFactory();

                                //Collects phase list of parallel features of affected feature.
                                GetParallelFeaturesPhaseList(affectedFeatureKeyObject, connecFactory, ref parallelPhaseList);

                                //Collects phase list of Upstream features of affected feature.
                                GetUpstreamFeaturesPhaseList(affectedFeatureKeyObject, connecFactory, ref upstreamPhaseList);

                                //Collects phase list of DownStream features of affected feature.
                                GetDownStreamFeaturesPhaseList(affectedFeatureKeyObject, connecFactory, ref downstreamPhaseList);

                                //Collects phase list of Inside features of affected feature.
                                GetInsideFeaturesPhaseList(affectedFeatureKeyObject, connecFactory, ref insidePhaseList);

                                //Collects phase list of Outside features of affected feature.
                                GetOutsideFeaturesPhaseList(affectedFeatureKeyObject, connecFactory, ref outsidePhaseList);

                            }
                            //Validates the Phase values based on different conditions
                            ValidatePhase(outsidePhaseList, insidePhaseList, parallelPhaseList, upstreamPhaseList, downstreamPhaseList, relatedFeaturesKeyObjects, affectedFeatureKeyObject.FNO, ref errorPriority, ref errorMessage);

                            if (errorMessage.Count > 0)
                            {
                                ErrorPriorityArray = errorPriority.ToArray();
                                ErrorMessageArray = errorMessage.ToArray();
                            }
                        }
                    }
                }

                if (gTValidationLogger != null)
                    gTValidationLogger.LogEntry("TIMING", "END", "Phase Agreement Exit", "N/A", "");
            }
            catch (Exception ex)
            {
                throw new Exception("Error during Phase agreement Validation" + ex.Message);
            }
            finally
            {
                downstreamPhaseList = null;
                upstreamPhaseList = null;
                insidePhaseList = null;
                outsidePhaseList = null;
            }
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// ValidatePhase : All the validations reagrding the Phase values are validated in this method.
        /// </summary>
        /// <param name="outsidePhaseList">List of Outside Phases</param>
        /// <param name="insidePhaseList">List of Inside Phases</param>
        /// <param name="parallelPhaseList">List of parallel Phases</param>
        /// <param name="upstreamPhaseList">List of upstream Phases</param>
        /// <param name="downstreamPhaseList">List of downstream Phases</param>
        /// <param name="relatedFeatures">List of related features of active feature</param>
        /// <param name="activeFNO">Active feature FNO</param>
        /// <param name="errorPriority">List of error priorities</param>
        /// <param name="errorMessage">List of error Messages</param>
        private void ValidatePhase(List<char> outsidePhaseList, List<char> insidePhaseList, List<char> parallelPhaseList, List<char> upstreamPhaseList, List<char> downstreamPhaseList, IGTKeyObjects relatedFeatures, short activeFNO, ref List<string> errorPriority, ref List<string> errorMessage)
        {
            string relatedPhase = String.Empty;
            List<short> protectionDevicesFNO = new List<short> { 11, 87, 38, 88, 14, 15, 16, 91, 59, 98, 60, 99, 18, 34 }; //Protection Device feature FNO's

            List<short> conductorsFNO = new List<short> { 8, 9, 84, 85 }; //conductor feature FNO's
            List<char> activePhaseList = new List<char>();

            try
            {
                ValidationRuleManager validateMsg = new ValidationRuleManager();

                string errorPriorityMissing = Convert.ToString(Arguments.GetArgument(0));
                string errorPriorityParallelSingle = Convert.ToString(Arguments.GetArgument(1));
                string errorPriorityUnused = Convert.ToString(Arguments.GetArgument(2));
                string errorPriorityPhaseCount = Convert.ToString(Arguments.GetArgument(3));
                string errorPriorityUnfed = Convert.ToString(Arguments.GetArgument(4));

                if (!String.IsNullOrEmpty(m_activeFeaturePhase))
                {
                    foreach (char c in m_activeFeaturePhase)
                    {
                        if (c != 'N')
                        {
                            activePhaseList.Add(c); //Active phase list
                        }
                    }
                }

                if (m_numberofNodes == 2 && activeFNO != 16 && activeFNO != 91)
                {
                    //Checks if Phase of affected feature minus “outside phases” is not an empty set"
                    if (outsidePhaseList != null && activePhaseList.Except(outsidePhaseList).Count() > 0)
                    {
                        if (!CheckPhaseNotEmptySet(activePhaseList, outsidePhaseList, conductorsFNO)) // Checks for Affected feature has three phases,Affected feature is not a conductor, Quantity of “outside phases” = 2

                        {
                            validateMsg.Rule_Id = "PHAG01";
                            validateMsg.BuildRuleMessage(GTClassFactory.Create<IGTApplication>(), null);

                            errorPriority.Add(errorPriorityUnused);
                            errorMessage.Add(validateMsg.Rule_MSG);
                        }
                    }

                    //If Set of ”outside phases” differs from the set of “inside phases”,then raise an error "Not all phases are carried between nodes of the feature."

                    if (outsidePhaseList != null && insidePhaseList != null)
                    {
                        if (outsidePhaseList.Except(insidePhaseList).Count() > 0 || insidePhaseList.Except(outsidePhaseList).Count() > 0)
                        {
                            validateMsg.Rule_Id = "PHAG02";
                            validateMsg.BuildRuleMessage(GTClassFactory.Create<IGTApplication>(), null);

                            errorPriority.Add(errorPriorityMissing);
                            errorMessage.Add(validateMsg.Rule_MSG);
                        }
                    }

                    //Single-phase devices with the same phase connected in parallel.

                    if (activePhaseList.Count == 1 && parallelPhaseList.Count > 0)
                    {
                        if (parallelPhaseList.Contains(activePhaseList[0]))
                        {
                            validateMsg.Rule_Id = "PHAG03";
                            validateMsg.BuildRuleMessage(GTClassFactory.Create<IGTApplication>(), null);

                            errorPriority.Add(errorPriorityParallelSingle);
                            errorMessage.Add(validateMsg.Rule_MSG);
                        }
                    }
                }
                //1 or 2 phase feature is connected to a 3-phase feature other than a protective device.
                if ((activePhaseList.Count == 1 || activePhaseList.Count == 2) && conductorsFNO.Contains(activeFNO))
                {
                    if (relatedFeatures.Count > 0)
                    {
                        for (int i = 0; i < relatedFeatures.Count; i++)
                        {
                            relatedFeatures[i].Components.GetComponent(m_cCNO).Recordset.MoveFirst();

                            relatedPhase = Convert.ToString(relatedFeatures[i].Components.GetComponent(m_cCNO).Recordset.Fields[ATTRIB_PHASE].Value);

                            if (relatedPhase.Length == 3 && !relatedPhase.Contains('N') && !protectionDevicesFNO.Contains(relatedFeatures[i].FNO))
                            {
                                validateMsg.Rule_Id = "PHAG04";
                                validateMsg.BuildRuleMessage(GTClassFactory.Create<IGTApplication>(), null);

                                errorPriority.Add(errorPriorityPhaseCount);
                                errorMessage.Add(validateMsg.Rule_MSG);

                            }
                        }
                    }

                }

                if (m_numberofNodes == 2 && activeFNO != 16 && activeFNO != 91)
                {
                    //One or more phases connected downstream of the feature are not present at the upstream node.

                    if (downstreamPhaseList.Except(upstreamPhaseList).Count() > 0)
                    {
                        validateMsg.Rule_Id = "PHAG05";
                        validateMsg.BuildRuleMessage(GTClassFactory.Create<IGTApplication>(), null);

                        errorPriority.Add(errorPriorityUnfed);
                        errorMessage.Add(validateMsg.Rule_MSG);
                    }
                }
            }
            catch
            {
                throw;
            }
            finally
            {
                protectionDevicesFNO = null;
                conductorsFNO = null;
                activePhaseList = null;
            }
        }

        /// <summary>
        /// CheckAffectedFeatureIsSecondary : Checks if the active feature is a secondary feature.
        /// </summary>
        /// <returns>True if the feature is secondary and false if not.</returns>
        private bool CheckAffectedFeatureIsSecondary()
        {
            List<short> secondaryFeatures = new List<short> { 52, 53, 54, 55, 56, 58, 59, 60, 61, 62, 63, 86, 94, 95, 96, 97, 98, 99, 153, 154, 155, 156, 157, 161, 162 };
            bool isSecondary = false;

            try
            {
                m_affectedConnectivityRs = Components.GetComponent(m_cCNO).Recordset;
                m_affectedConnectivityRs.MoveFirst();

                if (secondaryFeatures.Contains(Convert.ToInt16(m_affectedConnectivityRs.Fields[ATTRIB_FNO].Value)))
                {
                    isSecondary = true;
                }
            }
            catch
            {
                throw;
            }
            finally
            {
                secondaryFeatures = null;
            }
            return isSecondary;
        }

        /// <summary>
        /// Returns the number of nodes present for feature for connectivity relationship.
        /// </summary>
        /// <param name="activeFeature"></param>
        /// <returns></returns>
        private void GetNumberOfNodes(IGTKeyObject activeFeature)
        {
            try
            {
                Recordset oRSFeature = DataContext.MetadataRecordset("G3E_FEATURES_OPTABLE", "G3E_FNO=" + activeFeature.FNO);
                // oRSFeature.Filter = "G3E_FNO=" + activeFeature.FNO;

                if (oRSFeature.Fields["G3E_NUMBEROFNODES"].Value.GetType() == typeof(DBNull))
                {
                    m_numberofNodes = 0;
                }
                else
                {
                    m_numberofNodes = Convert.ToInt16(oRSFeature.Fields["G3E_NUMBEROFNODES"].Value);
                }
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// GetParallelFeaturesPhaseList : Gives the Phase list of features which are connected parallel to active feature.
        /// </summary>
        /// <param name="p_affectedKeyObject">Active feature</param>
        /// <param name="parallelPhaseList">Returns list of parallel features Phase list</param>
        private void GetParallelFeaturesPhaseList(IGTKeyObject p_affectedKeyObject, ConnectivityFactory connFactory, ref List<char> parallelPhaseList)
        {
            List<IGTKeyObject> parallelFeaturesKeyObjects = new List<IGTKeyObject>();
            try
            {
                RelatedFeatureTypes relParallelFeatureType = connFactory.GetHelper(ConnectivityHelper.ParallelFeatures);
                relParallelFeatureType.AffectedFeature = p_affectedKeyObject;

                parallelFeaturesKeyObjects = relParallelFeatureType.GetRelatedFeaturesForFeatureTypes();

                GetPhaseList(parallelFeaturesKeyObjects, ref parallelPhaseList);
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// GetUpstreamFeaturesPhaseList : Gives the Phase list of features which are connected Upstream to active feature.
        /// </summary>
        /// <param name="p_affectedKeyObject">Active feature</param>
        /// <param name="upstreamPhaseList">Returns list of downstream features Phase list</param>
        private void GetUpstreamFeaturesPhaseList(IGTKeyObject p_affectedKeyObject, ConnectivityFactory connFactory, ref List<char> upstreamPhaseList)
        {
            List<IGTKeyObject> upstreamFeaturesKeyObjects = new List<IGTKeyObject>();
            try
            {
                RelatedFeatureTypes relUpstreamFeatureType = connFactory.GetHelper(ConnectivityHelper.SourceSideOrUSFeatures);
                relUpstreamFeatureType.AffectedFeature = p_affectedKeyObject;

                upstreamFeaturesKeyObjects = relUpstreamFeatureType.GetRelatedFeaturesForFeatureTypes();

                GetPhaseList(upstreamFeaturesKeyObjects, ref upstreamPhaseList);

            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// GetDownStreamFeaturesPhaseList : Gives the Phase list of features which are connected downstream to active feature.
        /// </summary>
        /// <param name="p_affectedKeyObject">Active feature</param>
        /// <param name="downstreamPhaseList">Returns list of DownStream features Phase list</param>
        private void GetDownStreamFeaturesPhaseList(IGTKeyObject p_affectedKeyObject, ConnectivityFactory connFactory, ref List<char> downstreamPhaseList)
        {
            List<IGTKeyObject> downstreamNodeFeaturesKeyObjects = new List<IGTKeyObject>();
            try
            {
                RelatedFeatureTypes relDownstreamFeatureType = connFactory.GetHelper(ConnectivityHelper.LoadSideORDSFeatures);
                relDownstreamFeatureType.AffectedFeature = p_affectedKeyObject;

                downstreamNodeFeaturesKeyObjects = relDownstreamFeatureType.GetRelatedFeaturesForFeatureTypes();

                GetPhaseList(downstreamNodeFeaturesKeyObjects, ref downstreamPhaseList);
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// GetInsideFeaturesPhaseList : Gives the Phase list of features which are connected downstream to active feature.
        /// </summary>
        /// <param name="p_affectedKeyObject">Active feature</param>
        /// <param name="insidePhaseList">Returns list of inside features Phase list</param>
        private void GetInsideFeaturesPhaseList(IGTKeyObject p_affectedKeyObject, ConnectivityFactory connFactory, ref List<char> insidePhaseList)
        {
            try
            {
                RelatedFeatureTypes relInsideFeatureType = connFactory.GetHelper(ConnectivityHelper.InsideFeatures);
                relInsideFeatureType.AffectedFeature = p_affectedKeyObject;

                insidePhaseList = relInsideFeatureType.GetRelatedPhases();
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// GetInsideFeaturesPhaseList : Gives the Phase list of features which are connected downstream to active feature.
        /// </summary>
        /// <param name="p_affectedKeyObject">Active feature</param>
        /// <param name="outsidePhaseList">Returns list of Outside features Phase list</param>
        private void GetOutsideFeaturesPhaseList(IGTKeyObject p_affectedKeyObject, ConnectivityFactory connFactory, ref List<char> outsidePhaseList)
        {
            try
            {
                RelatedFeatureTypes relOutsideFeatureType = connFactory.GetHelper(ConnectivityHelper.OutsideFeatures);
                relOutsideFeatureType.AffectedFeature = p_affectedKeyObject;

                outsidePhaseList = relOutsideFeatureType.GetRelatedPhases();
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// GetPhaseList : Gives the Phase list of features.
        /// </summary>
        /// <param name="p_keyObjects">Key objects connected to the active feature</param>
        /// <param name="phaseList">Return list of phase</param>
        private void GetPhaseList(List<IGTKeyObject> p_keyObjects, ref List<char> phaseList)
        {
            string phase = null;
            try
            {
                foreach (IGTKeyObject keyObject in p_keyObjects)
                {
                    keyObject.Components.GetComponent(m_cCNO).Recordset.MoveFirst();

                    phase = Convert.ToString(keyObject.Components.GetComponent(m_cCNO).Recordset.Fields[ATTRIB_PHASE].Value);

                    if (!String.IsNullOrEmpty(phase))
                    {
                        foreach (char c in phase)
                        {
                            if (c != 'N')
                            {
                                phaseList.Add(c);
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
        /// GetRelatedFeatures:Returns the related feature for a corresponding relationship number
        /// </summary>
        /// <param name="activeFeature"></param>
        /// <param name="cRNO"></param>
        /// <returns></returns>
        private IGTKeyObjects GetRelatedFeatures(IGTKeyObject p_activeFeature, short p_cRNO)
        {
            try
            {
                using (IGTRelationshipService m_relService = GTClassFactory.Create<IGTRelationshipService>())
                {
                    m_relService.DataContext = DataContext;
                    m_relService.ActiveFeature = p_activeFeature;
                    IGTKeyObjects m_relatedFeatures = m_relService.GetRelatedFeatures(p_cRNO);
                    return m_relatedFeatures;
                }
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// CheckAffectedFeature : Checks whether the Affected feature has three phases,is not a conductor and Quantity of outside phases is 2
        /// </summary>
        /// <param name="p_activePhase">Active Phase list of the feature</param>
        /// <param name="p_outsidePhase">Outside phase list of the active feature</param>
        /// <param name="p_conductorsFNO">Lsit of the conductor Fno's</param>
        /// <returns>Returns false if conditions doesn't satisfy else true.</returns>
        private bool CheckPhaseNotEmptySet(List<char> p_activePhase, List<char> p_outsidePhase, List<short> p_conductorsFNO)
        {
            bool errorPriorityUnused = false;
            try
            {
                if (p_activePhase.Count == 3 && !p_conductorsFNO.Contains(Convert.ToInt16(Components[ComponentName].Recordset.Fields[ATTRIB_FNO].Value)) && p_outsidePhase.Count == 2)
                {
                    errorPriorityUnused = true;
                }
            }
            catch
            {
                throw;
            }
            return errorPriorityUnused;
        }

        #endregion
    }
}