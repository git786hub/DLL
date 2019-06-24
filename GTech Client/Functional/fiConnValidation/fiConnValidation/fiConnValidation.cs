//----------------------------------------------------------------------------+
//        Class: fiConnValidation
//  Description: 
//		Validates a feature has valid connectivity, or else is allowed to be disconnected.
//                 	Validates that the owner of a point feature is the same as all other point features connected at the same node.
//----------------------------------------------------------------------------+
//     $Author:: Prathyusha                                                      $
//       $Date:: 25/08/2017                                                     $
//   $Revision:: 1                                                            $
//----------------------------------------------------------------------------+
//    $History:: fiConnValidation.cs                                           $
//User   Date         Comments
//Hari   04/06/2018   Code changes done per JIRA ONCORDEV-1717
//Prathyusha 12/06/2018 Code changes done per JIRA ONCORDEV-1718.
//Prathyusha 22/01/2018 Code changes done per JIRA ONCORDEV-2435 to exclude Communication Features.
//Hari       25/02/2019 Fix for ALM-1931-JIRA-2565 - Replace Feature on a Closed Fuse cause P1 error.
//Hari       7/3/2019   Fix for ALM-2066-JIRA-2630 - P1 validation error – Error using functional interface.Error from Connection Validation FI: Either BOF or EOF is True.
//Hari       8/3/2019   Fix for ALM-1931-JIRA-2565 - Replace Feature on a Closed Fuse cause P1 error. Reverted code for CONN07
//Hari       12/3/2019   Fix for ALM-1931-JIRA-2565 - Replace Feature on a Closed Fuse cause P1 error. Added parallel sources (pure parallel) check along with shared loads
//Prathyusha 12/06/2018  ALM-2395: Code changes to add the exception, that if any feature owned by a Primary Switch Gear, allow nodes to be unconnected and do not throw error CONN01
//----------------------------------------------------------------------------+

using System;
using Intergraph.GTechnology.Interfaces;
using Intergraph.GTechnology.API;
using System.Collections.Generic;
using ADODB;
using gtCommandLogger;

namespace GTechnology.Oncor.CustomAPI
{
    class fiConnValidation : IGTFunctional
    {
        #region Private Variables
        private GTArguments m_GTArguments = null;
        private string m_componentName = string.Empty;
        private IGTComponents m_components = null;
        private IGTDataContext m_dataContext = null;
        private string m_fieldName = string.Empty;
        private IGTFieldValue m_fieldValueBeforeChange = null;
        private GTFunctionalTypeConstants m_type;

        //constant

        private const string ATTRIB_FID = "G3E_FID";
        private const string ATTRIB_FNO = "G3E_FNO";
        private const string ATTRIB_FEATURE_STATE = "FEATURE_STATE_C";
        private const string ATTRIB_OWNER1_ID = "OWNER1_ID";
        private const string ATTRIB_NODE1 = "NODE_1_ID";
        private const string ATTRIB_NODE2 = "NODE_2_ID";
        private const short cRNO = 14;
        private const short cCNO = 11;
        private const short oCNO = 1;
        #endregion

        #region Properities
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
                return m_componentName;
            }

            set
            {
                m_componentName = value;
            }
        }

        public IGTComponents Components
        {
            get
            {
                return m_components;
            }

            set
            {
                m_components = value;
            }
        }

        public IGTDataContext DataContext
        {
            get
            {
                return m_dataContext;
            }

            set
            {
                m_dataContext = value;
            }
        }

        public string FieldName
        {
            get
            {
                return m_fieldName;
            }

            set
            {
                m_fieldName = value;
            }
        }

        public IGTFieldValue FieldValueBeforeChange
        {
            get
            {
                return m_fieldValueBeforeChange;
            }

            set
            {
                m_fieldValueBeforeChange = value;
            }
        }

        public GTFunctionalTypeConstants Type
        {
            get
            {
                return m_type;
            }

            set
            {
                m_type = value;
            }
        }

        #endregion

        #region Functional Interface Members
        public void Delete()
        {

        }

        public void Execute()
        {

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ErrorPriorityArray"></param>
        /// <param name="ErrorMessageArray"></param>
        public void Validate(out string[] ErrorPriorityArray, out string[] ErrorMessageArray)
        {
            ErrorPriorityArray = null;
            ErrorMessageArray = null;

            string activeFeatureState = null;
            short numberofNodes = 0;

            int node1 = 0;
            int node2 = 0;

            string errorPriorityUnconnected = Convert.ToString(Arguments.GetArgument(0));
            string errorPrioritySelf = Convert.ToString(Arguments.GetArgument(1));
            string errorPrioritySharedOwner = Convert.ToString(Arguments.GetArgument(2));

            ValidationRuleManager validateMsg = new ValidationRuleManager();
            object[] messArguments = new object[1];

            List<string> errorMessage = new List<string>();
            List<string> errorPriority = new List<string>();

            IGTKeyObject activeFeature = GTClassFactory.Create<IGTKeyObject>();
            IGTKeyObjects relatedFeatures = GTClassFactory.Create<IGTKeyObjects>();

            IGTGeometry primaryGeometry = null;

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
                    ValidationInterfaceName = "Connectivity Validation",
                    ValidationInterfaceType = "FI",
                };
                gTValidationLogger = new GTValidationLogger(logEntries);

                gTValidationLogger.LogEntry("TIMING", "START", "Connectivity Validation Entry", "N/A", "");
            }


            GTClassFactory.Create<IGTApplication>().SetStatusBarText(GTStatusPanelConstants.gtaspcMessage, "ConnectionValidation FI Started");
            try
            {

                activeFeature = DataContext.OpenFeature(Convert.ToInt16(Components[ComponentName].Recordset.Fields[ATTRIB_FNO].Value), Convert.ToInt32(Components[ComponentName].Recordset.Fields[ATTRIB_FID].Value));

                if (CheckValidFeatures(activeFeature.FNO))
                {
                    relatedFeatures = GetRelatedFeatures(activeFeature, cRNO);

                    GetNumberOfNodes(activeFeature, ref numberofNodes);

                    GetPrimaryGeometry(activeFeature, ref primaryGeometry);

                    /*Gets active Feature attributes*/
                    if (activeFeature.Components.GetComponent(cCNO).Recordset.RecordCount > 0)
                    {
                        activeFeature.Components.GetComponent(cCNO).Recordset.MoveFirst();

                        node1 = Convert.ToInt32(activeFeature.Components.GetComponent(cCNO).Recordset.Fields[ATTRIB_NODE1].Value == DBNull.Value ? 0 : activeFeature.Components.GetComponent(cCNO).Recordset.Fields[ATTRIB_NODE1].Value);
                        node2 = Convert.ToInt32(activeFeature.Components.GetComponent(cCNO).Recordset.Fields[ATTRIB_NODE2].Value == DBNull.Value ? 0 : activeFeature.Components.GetComponent(cCNO).Recordset.Fields[ATTRIB_NODE2].Value);

                        activeFeature.Components.GetComponent(oCNO).Recordset.MoveFirst();

                        activeFeatureState = Convert.ToString(activeFeature.Components.GetComponent(oCNO).Recordset.Fields[ATTRIB_FEATURE_STATE].Value);
                    }

                    //Feature in Feature State in (PPI, PPR, PPA, INI, CLS, ABI, ABR) must be connected.

                    if (CheckFeatureState(activeFeatureState) && CheckNotOwnedByPrimarySwitchGear(activeFeature))
                    {
                        if ((numberofNodes == 1 && node1 == 0 && node2 == 0) || (numberofNodes == 2 && (node1 == 0 || node2 == 0)))
                        {

                            messArguments[0] = activeFeatureState;

                            validateMsg.Rule_Id = "CONN01";
                            validateMsg.BuildRuleMessage(GTClassFactory.Create<IGTApplication>(), messArguments);

                            errorMessage.Add(validateMsg.Rule_MSG);
                            errorPriority.Add(errorPriorityUnconnected);
                        }
                    }

                    //Feature may not be connected to itself.

                    if ((numberofNodes == 2) && node1 != 0 && node1 == node2)
                    {
                        validateMsg.Rule_Id = "CONN02";
                        validateMsg.BuildRuleMessage(GTClassFactory.Create<IGTApplication>(), null);

                        errorMessage.Add(validateMsg.Rule_MSG);
                        errorPriority.Add(errorPrioritySelf);
                    }

                    // Feature is a point feature and feature owner not in the set of connected owners

                    if (primaryGeometry != null && primaryGeometry.Type == "OrientedPointGeometry")
                    {
                        int noMatchingOwnerCnt = 0;

                        GetActiveAndRelatedFeaturesOwners(activeFeature, relatedFeatures, ref noMatchingOwnerCnt);

                        for (int i = 1; i <= noMatchingOwnerCnt; i++)
                        {
                            validateMsg.Rule_Id = "CONN03";
                            validateMsg.BuildRuleMessage(GTClassFactory.Create<IGTApplication>(), null);

                            errorMessage.Add(validateMsg.Rule_MSG);
                            errorPriority.Add(errorPrioritySharedOwner);
                        }
                    }


                    //Feature is a Service Line and not connected to exactly one Service Point

                    if (activeFeature.FNO == 54)//FNO == 54, is Service Line
                    {
                        int count = 0;

                        for (int i = 0; i < relatedFeatures.Count; i++)
                        {
                            if (relatedFeatures[i].FNO == 55) //FNO == 55, is Service Point FNO
                            {
                                count++;
                            }
                        }

                        if (count != 1)
                        {
                            validateMsg.Rule_Id = "CONN04";
                            validateMsg.BuildRuleMessage(GTClassFactory.Create<IGTApplication>(), null);

                            errorMessage.Add(validateMsg.Rule_MSG);
                            errorPriority.Add(errorPriorityUnconnected);
                        }
                    }

                    //Feature is a Primary Conductor Node or Secondary Conductor Node and Type = DEAD END and not connected to exactly one other feature

                    if (activeFeature.FNO == 10 || activeFeature.FNO == 162) //FNO == 10, is Primary Conductor Node; FNO == 162, is Secondary Conductor Node
                    {
                        string conductorNodeType = GetConductorNodeType(activeFeature);                       
                        string relatedFeatureState = string.Empty;
                        IGTKeyObjects oKeyObjects = GTClassFactory.Create<IGTKeyObjects>();


                        if (!String.IsNullOrEmpty(conductorNodeType))
                        {
                            if (conductorNodeType.ToUpper() == "DEADEND")
                            {
                                int iCount = 0;

                                foreach (IGTKeyObject item in relatedFeatures)
                                {
                                    //Check if the related feature belong to valid feature state
                                    item.Components.GetComponent(oCNO).Recordset.MoveFirst(); //This is common component recordset so no harm in moving to first
                                    relatedFeatureState = Convert.ToString(item.Components.GetComponent(oCNO).Recordset.Fields["FEATURE_STATE_C"].Value);

                                    if (relatedFeatureState.Equals("PPI") || relatedFeatureState.Equals("ABI") || relatedFeatureState.Equals("PPX") || relatedFeatureState.Equals("ABX") || relatedFeatureState.Equals("INI") || relatedFeatureState.Equals("CLS"))
                                    {                                        
                                        iCount = iCount + 1;
                                    }
                                }

                                if (iCount > 1 && (activeFeatureState.Equals("PPI") || activeFeatureState.Equals("ABI") || activeFeatureState.Equals("INI") || activeFeatureState.Equals("CLS")))
                                {
                                    validateMsg.Rule_Id = "CONN05";
                                    validateMsg.BuildRuleMessage(GTClassFactory.Create<IGTApplication>(), null);

                                    errorMessage.Add(validateMsg.Rule_MSG);
                                    errorPriority.Add(errorPriorityUnconnected);
                                }
                            }
                        }
                    }

                    //Feature is a Street Light and Owner Type = COMPANY and not connected.

                    if (activeFeature.FNO == 56)
                    {
                        activeFeature.Components.GetComponent(oCNO).Recordset.MoveFirst();

                        string ownerType = Convert.ToString(activeFeature.Components.GetComponent(oCNO).Recordset.Fields["OWNED_TYPE_C"].Value);

                        if (!String.IsNullOrEmpty(ownerType))
                        {
                            if (ownerType.ToUpper() == "COMPANY" && node1 == 0 && node2 == 0)
                            {
                                validateMsg.Rule_Id = "CONN06";
                                validateMsg.BuildRuleMessage(GTClassFactory.Create<IGTApplication>(), null);

                                errorMessage.Add(validateMsg.Rule_MSG);
                                errorPriority.Add(errorPriorityUnconnected);
                            }
                        }
                    }

                    //Removal of this feature will cause downstream features to be disconnected.

                    if (numberofNodes == 2 && CheckForRemovalActiveFeatureState(activeFeatureState)) //Active feature Feature State in (PPR, PPA, ABR, ABA)
                    {
                        List<IGTKeyObject> loadFeatures = new List<IGTKeyObject>();
                        List<IGTKeyObject> sharedLoadFeatures = new List<IGTKeyObject>();
                        List<IGTKeyObject> parallelFeatures = new List<IGTKeyObject>();

                        ConnectivityFactory oFactory = new ConnectivityHelperFactory();

                        GetLoadsFeatures(ref loadFeatures, activeFeature, oFactory);

                        GetSharedLoadsFeatures(ref sharedLoadFeatures, activeFeature, oFactory);

                        GetParallelFeatures(ref parallelFeatures, activeFeature, oFactory);

                        if (loadFeatures.Count > 0 && sharedLoadFeatures.Count == 0 && parallelFeatures.Count == 0) //Affected feature has one or more “loads” and Affected features has zero “parallel sources”
                        {
                            validateMsg.Rule_Id = "CONN07";
                            validateMsg.BuildRuleMessage(GTClassFactory.Create<IGTApplication>(), null);

                            errorMessage.Add(validateMsg.Rule_MSG);
                            errorPriority.Add(errorPriorityUnconnected);
                        }
                    }

                    if (errorMessage.Count > 0)
                    {
                        ErrorPriorityArray = errorPriority.ToArray();
                        ErrorMessageArray = errorMessage.ToArray();
                    }

                    GTClassFactory.Create<IGTApplication>().SetStatusBarText(GTStatusPanelConstants.gtaspcMessage, "ConnectionValidation FI Completed");

                    if (gTValidationLogger != null)
                        gTValidationLogger.LogEntry("TIMING", "END", "Connectivity Validation Exit", "N/A", "");
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error from Connection Validation FI: " + ex.Message);
            }
        }
        #endregion

        #region Private Methods

        /// <summary>
        /// Check for the valid feature.
        /// </summary>
        /// <param name="p_fno"></param>
        /// <returns></returns>
        private bool CheckValidFeatures(short p_fno)
        {
            bool validFeature = false;
            try
            {

                Recordset rsConnectivityOptional = DataContext.OpenRecordset(String.Format("SELECT count(*) as cnt FROM G3E_NODEEDGECONN_ELEC_OPTABLE WHERE G3E_ENFORCEMENT IN(0,5) AND G3E_SOURCEFNO  = {0}", p_fno), CursorTypeEnum.adOpenStatic,
                          LockTypeEnum.adLockReadOnly, (int)CommandTypeEnum.adCmdText);

                if (rsConnectivityOptional != null && rsConnectivityOptional.RecordCount > 0)
                {
                    rsConnectivityOptional.MoveFirst();

                    if (Convert.ToInt16(rsConnectivityOptional.Fields["cnt"].Value) == 0)
                    {
                        validFeature = true;
                    }
                }
            }
            catch
            {
                throw;
            }
            return validFeature;
        }

        /// <summary>
        /// Check for the valid feature state
        /// </summary>
        /// <param name="p_featureState"></param>
        /// <returns></returns>
        private bool CheckFeatureState(string p_featureState)
        {
            try
            {
                switch (p_featureState)
                {
                    case "PPI":
                    case "PPR":
                    case "PPA":
                    case "INI":
                    case "CLS":
                    case "ABI":
                    case "ABR":
                        return true;
                }
            }
            catch
            {
                throw;
            }
            return false;
        }

        /// <summary>
        /// Check for the Removal feature state of active feature
        /// </summary>
        /// <param name="p_featureState"></param>
        /// <returns></returns>
        private bool CheckForRemovalActiveFeatureState(string p_featureState)
        {
            try
            {
                switch (p_featureState)
                {
                    case "PPR":
                    case "PPA":
                    case "ABA":
                    case "ABR":
                        return true;
                }
            }
            catch
            {
                throw;
            }
            return false;
        }

        private void GetNonRemovalRelatedFeatures(ref List<IGTKeyObject> featuresList, List<IGTKeyObject> actualFeatures)
        {
            try
            {
                foreach (IGTKeyObject keyObject in actualFeatures)
                {
                    keyObject.Components.GetComponent(oCNO).Recordset.MoveFirst();

                    switch (Convert.ToString(keyObject.Components.GetComponent(oCNO).Recordset.Fields[ATTRIB_FEATURE_STATE].Value))
                    {
                        case "PPR":
                        case "PPA":
                        case "ABA":
                        case "ABR":
                        case "OSR":
                        case "OSA":
                            break;
                        default:
                            {
                                featuresList.Add(keyObject);
                                break;
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
        ///  Gives the Load features or down stream features of the active feature
        /// </summary>
        /// <param name="downStreamNodeFeatures"></param>
        /// <param name="p_activeFeature"></param>
        private void GetLoadsFeatures(ref List<IGTKeyObject> loadFeatures, IGTKeyObject p_activeFeature, ConnectivityFactory connectivityFactory)
        {
            try
            {
                RelatedFeatureTypes relFeatureTypes = connectivityFactory.GetHelper(ConnectivityHelper.LoadSideORDSFeatures);

                relFeatureTypes.AffectedFeature = p_activeFeature;

                List<IGTKeyObject> actualDownStreamNodeFeatures = relFeatureTypes.GetRelatedFeaturesForFeatureTypes();

                GetNonRemovalRelatedFeatures(ref loadFeatures, actualDownStreamNodeFeatures);
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        ///  Gives the shared Load features of the active feature
        /// </summary>
        /// <param name="sharedLoadFeatures"></param>
        /// <param name="p_activeFeature"></param>
        private void GetSharedLoadsFeatures(ref List<IGTKeyObject> sharedLoadFeatures, IGTKeyObject p_activeFeature, ConnectivityFactory connectivityFactory)
        {
            try
            {
                RelatedFeatureTypes relFeatureTypes = connectivityFactory.GetHelper(ConnectivityHelper.SharedLoads);

                relFeatureTypes.AffectedFeature = p_activeFeature;

                List<IGTKeyObject> actualSharedLoadFeatures = relFeatureTypes.GetRelatedFeaturesForFeatureTypes();

                GetNonRemovalRelatedFeatures(ref sharedLoadFeatures, actualSharedLoadFeatures);
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        ///  Gives the parallel features of the active feature
        /// </summary>
        /// <param name="parallelFeatures"></param>
        /// <param name="p_activeFeature"></param>
        /// <param name="connectivityFactory"></param>
        private void GetParallelFeatures(ref List<IGTKeyObject> parallelFeatures, IGTKeyObject p_activeFeature, ConnectivityFactory connectivityFactory)
        {
            try
            {
                RelatedFeatureTypes relFeatureTypes = connectivityFactory.GetHelper(ConnectivityHelper.ParallelFeatures);

                relFeatureTypes.AffectedFeature = p_activeFeature;

                List<IGTKeyObject> actualParallelFeatures = relFeatureTypes.GetRelatedFeaturesForFeatureTypes();

                GetNonRemovalRelatedFeatures(ref parallelFeatures, actualParallelFeatures);
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Returns the Primary geometry of the feature
        /// </summary>
        /// <param name="feature"></param>
        /// <returns></returns>
        private void GetPrimaryGeometry(IGTKeyObject feature, ref IGTGeometry primaryGeometry)
        {
            IGTComponent component = null;
            try
            {
                Recordset oRSFeature = DataContext.MetadataRecordset("G3E_FEATURES_OPTABLE", "G3E_FNO=" + feature.FNO);
                // oRSFeature.Filter = "G3E_FNO=" + feature.FNO;

                if (!(oRSFeature.Fields["G3E_PRIMARYGEOGRAPHICCNO"].Value is DBNull))
                {
                    component = feature.Components.GetComponent(Convert.ToInt16(oRSFeature.Fields["G3E_PRIMARYGEOGRAPHICCNO"].Value));

                    if (component != null && component.Recordset != null && component.Recordset.RecordCount == 1)
                    {
                        primaryGeometry = component.Geometry;
                    }

                    else if (component != null && component.Recordset != null && component.Recordset.RecordCount > 1)
                    {
                        throw new DuplicateIdentifierException(string.Format("More than one feature exists with the same feature identifier {0}. Please contact system administrator.", feature.FID));
                    }
                    else
                    {
                        if (!(oRSFeature.Fields["G3E_PRIMARYDETAILCNO"].Value is DBNull))
                        {
                            component = feature.Components.GetComponent(Convert.ToInt16(oRSFeature.Fields["G3E_PRIMARYDETAILCNO"].Value));

                            if (component != null && component.Recordset != null && component.Recordset.RecordCount > 0)
                            {
                                primaryGeometry = component.Geometry;
                            }
                        }
                    }
                }

                else if (!(oRSFeature.Fields["G3E_PRIMARYDETAILCNO"].Value is DBNull))
                {
                    component = feature.Components.GetComponent(Convert.ToInt16(oRSFeature.Fields["G3E_PRIMARYDETAILCNO"].Value));

                    if (component != null && component.Recordset != null && component.Recordset.RecordCount > 0)
                    {
                        primaryGeometry = component.Geometry;
                    }
                    else
                    {
                        if (!(oRSFeature.Fields["G3E_PRIMARYGEOGRAPHICCNO"].Value is DBNull))
                        {
                            component = feature.Components.GetComponent(Convert.ToInt16(oRSFeature.Fields["G3E_PRIMARYGEOGRAPHICCNO"].Value));

                            if (component != null && component.Recordset != null && component.Recordset.RecordCount > 0)
                            {
                                primaryGeometry = component.Geometry;
                            }
                        }
                    }
                }
            }

            catch (DuplicateIdentifierException ex)
            {
                throw ex;
            }
            catch
            {
                throw;
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
                //  oRSFeature.Filter = "G3E_FNO=" + activeFeature.FNO;

                numberofNodes = Convert.ToInt16(oRSFeature.Fields["G3E_NUMBEROFNODES"].Value);
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Returns the related feature for a corresponding relationship number
        /// </summary>
        /// <param name="activeFeature"></param>
        /// <param name="cRNO"></param>
        /// <returns></returns>
        private IGTKeyObjects GetRelatedFeatures(IGTKeyObject activeFeature, short cRNO)
        {
            try
            {
                using (IGTRelationshipService relService = GTClassFactory.Create<IGTRelationshipService>())
                {
                    relService.DataContext = DataContext;
                    relService.ActiveFeature = activeFeature;

                    IGTKeyObjects relatedFeatures = null;
                    try
                    {
                        relatedFeatures = relService.GetRelatedFeatures(cRNO);
                    }
                    catch
                    {

                    }

                    return relatedFeatures;
                }
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Returns the conductor node type of the Primary/Secondary conductor node.
        /// </summary>
        /// <param name="feature"></param>
        /// <returns></returns>
        private string GetConductorNodeType(IGTKeyObject feature)
        {
            try
            {
                Recordset oRSFeature = DataContext.MetadataRecordset("G3E_FEATURES_OPTABLE", "G3E_FNO=" + feature.FNO);
                //  oRSFeature.Filter = "G3E_FNO=" + feature.FNO;
                feature.Components.GetComponent(Convert.ToInt16(oRSFeature.Fields["G3E_PRIMARYATTRIBUTECNO"].Value)).Recordset.MoveFirst();

                return Convert.ToString(feature.Components.GetComponent(Convert.ToInt16(oRSFeature.Fields["G3E_PRIMARYATTRIBUTECNO"].Value)).Recordset.Fields["TYPE_C"].Value);
            }
            catch
            {
                throw;
            }
        }
        /// <summary>
        /// Checks for the valid owner for the given feature
        /// </summary>
        /// <param name="sourceFNO"></param>
        /// <param name="aOwnerID"></param>
        /// <returns></returns>
        private bool CheckValidOwner(int sourceFNO, int aOwnerID)
        {
            Recordset rs = null;
            int ownerFNO = 0;
            try
            {
                rs = DataContext.OpenRecordset(String.Format("SELECT G3E_FNO FROM COMMON_N WHERE G3E_ID  = {0}", aOwnerID), CursorTypeEnum.adOpenStatic,
                          LockTypeEnum.adLockReadOnly, (int)CommandTypeEnum.adCmdText);

                if (rs != null && rs.RecordCount > 0)
                {
                    rs.MoveFirst();

                    ownerFNO = Convert.ToInt16(rs.Fields[ATTRIB_FNO].Value);
                }

                rs = DataContext.MetadataRecordset("G3E_OWNERSHIP_OPTABLE", "G3E_OWNERFNO=" + ownerFNO + " AND G3E_SOURCEFNO=" + sourceFNO);

                //   rs.Filter = "G3E_OWNERFNO=" + ownerFNO + " AND G3E_SOURCEFNO=" + sourceFNO;

                if (rs != null && rs.RecordCount > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Returns the Owner ID of the given feature
        /// </summary>
        /// <param name="p_commonRS"></param>
        /// <param name="ownerID"></param>
        private void GetOwnerID(Recordset p_commonRS, ref int ownerID)
        {
            try
            {
                if (p_commonRS.RecordCount > 0)
                {
                    p_commonRS.MoveFirst();

                    if (!String.IsNullOrEmpty(Convert.ToString(p_commonRS.Fields[ATTRIB_OWNER1_ID].Value)))
                    {
                        ownerID = Convert.ToInt32(p_commonRS.Fields[ATTRIB_OWNER1_ID].Value);
                    }
                    else
                    {
                        ownerID = 0;
                    }
                }
            }
            catch
            {
                throw;
            }
        }
        /// <summary>
        ///  Gives the list of all directly connected point features to active feature which have different owner other than active feature owner
        /// </summary>
        /// <param name="activeFeature"></param>
        /// <param name="relatedFeatures"></param>
        /// <param name="noMatchingOwnerCnt"></param>
        private void GetActiveAndRelatedFeaturesOwners(IGTKeyObject activeFeature, IGTKeyObjects relatedFeatures, ref int noMatchingOwnerCnt)
        {
            int aOwnerID = 0;
            int rOwnerID = 0;
            IGTGeometry relatedPrimaryGeom = null;
            try
            {
                GetOwnerID(activeFeature.Components.GetComponent(oCNO).Recordset, ref aOwnerID);

                foreach (IGTKeyObject relatedFeature in relatedFeatures)
                {
                    GetPrimaryGeometry(relatedFeature, ref relatedPrimaryGeom);

                    if (relatedPrimaryGeom != null && relatedPrimaryGeom.Type == "OrientedPointGeometry")
                    {
                        GetOwnerID(relatedFeature.Components.GetComponent(oCNO).Recordset, ref rOwnerID);

                        if (aOwnerID != 0 && (rOwnerID != aOwnerID) && CheckValidOwner(relatedFeature.FNO, aOwnerID))
                        {
                            noMatchingOwnerCnt++;
                        }
                    }
                }
            }
            catch
            {
                throw;
            }
        }

        private bool CheckNotOwnedByPrimarySwitchGear(IGTKeyObject activeFeature)
        {
            bool notOwned = true;
            try
            {
                IGTKeyObjects ownerFeatures = GetRelatedFeatures(activeFeature, 3);

                if (ownerFeatures != null)
                {
                    foreach (IGTKeyObject owner in ownerFeatures)
                    {
                        if (owner.FNO == 19)
                        {
                            notOwned = false;
                            break;
                        }
                    }
                }
            }
            catch
            {
                throw;
            }
            return notOwned;
        }

        #endregion
    }
}
