
//----------------------------------------------------------------------------+
//        Class: fiFuseCoordination.cs
//  Description: Validates that a given fuse is properly coordinated with other fuses upstream and downstream.
//----------------------------------------------------------------------------+
//     $$Author::    HKONDA                                                 $
//       $$Date::    5th June 2018                                          $
//   $$Revision::    1                                                      $
//----------------------------------------------------------------------------+
//
//************************Version 1**************************
//User: HKONDA    Date: 5th June 2018     Time : 3.30PM
//User: Pnlella   Date: 25th March 2019   Fixed ALM-2026/JIRA 2592. Fixed EOF/BOF validation error when installing a Primary Fuse - OH.
//----------------------------------------------------------------------------+

using ADODB;
using Intergraph.GTechnology.API;
using Intergraph.GTechnology.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using gtCommandLogger;

namespace GTechnology.Oncor.CustomAPI
{
    public class fiFuseCoordination : IGTFunctional
    {
        #region Private variables

        private GTArguments m_GTArguments = null;
        private string m_ComponentName = string.Empty;
        private IGTComponents m_Components = null;
        private IGTDataContext m_DataContext = null;
        private string m_FieldName = string.Empty;
        private IGTFieldValue m_fieldValueBeforeChange = null;
        private GTFunctionalTypeConstants m_Type;
        private Dictionary<int, short> m_upStreamSet = new Dictionary<int, short>();
        private Dictionary<int, short> m_protectedFuseSet = new Dictionary<int, short>();
        private List<FeatureHelper> m_featureUnitInstanceList = new List<FeatureHelper>();
        List<string> m_lstErrorMsg = new List<string>();
        List<string> m_lstErrorPriority = new List<string>();
        private int m_selectedFeaturefid = 0;
        private string m_selectedFeatureState = string.Empty;
        private const int primaryFuseUnitCNO = 1102;

        #endregion

        #region Public properties
        public GTArguments Arguments { get => m_GTArguments; set => m_GTArguments = value; }
        public string ComponentName { get => m_ComponentName; set => m_ComponentName = value; }
        public IGTComponents Components { get => m_Components; set => m_Components = value; }
        public IGTDataContext DataContext { get => m_DataContext; set => m_DataContext = value; }
        public string FieldName { get => m_FieldName; set => m_FieldName = value; }
        public IGTFieldValue FieldValueBeforeChange { get => m_fieldValueBeforeChange; set => m_fieldValueBeforeChange = value; }
        public GTFunctionalTypeConstants Type { get => m_Type; set => m_Type = value; }

        #endregion

        #region Interface Methods

        public void Delete()
        {
        }

        public void Execute()
        {
        }

        public void Validate(out string[] ErrorPriorityArray, out string[] ErrorMessageArray)
        {
            ErrorPriorityArray = new string[2];
            ErrorMessageArray = new string[2];
            List<string> m_ValidFeatureStates = new List<string> { "PPI", "ABI", "PPX", "ABX", "INI", "CLS" };

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
                    ValidationInterfaceName = "Fuse Coordination",
                    ValidationInterfaceType = "FI",
                };
                gTValidationLogger = new GTValidationLogger(logEntries);

                gTValidationLogger.LogEntry("TIMING", "START", "Fuse Coordination Entry", "N/A", "");
            }
            
            try
            {
                GetFeatureInformation();

                if (!m_ValidFeatureStates.Contains(m_selectedFeatureState))
                {
                    if(gTValidationLogger != null)
                    gTValidationLogger.LogEntry("TIMING", "END", "Fuse Coordination Exit", "N/A", "");
                    return;
                }

                BuildUpStreamFuses(m_selectedFeaturefid);
                BuildProtectedDownStreamFuses(m_selectedFeaturefid);
                ProcessFuseCoordination();

                if (m_lstErrorMsg.Count > 0)
                {
                    ErrorMessageArray = m_lstErrorMsg.ToArray();
                    ErrorPriorityArray = m_lstErrorPriority.ToArray();
                }

                if(gTValidationLogger != null)
                    gTValidationLogger.LogEntry("TIMING", "END", "Fuse Coordination Exit", "N/A", "");
            }
            catch (Exception ex)
            {
                throw new Exception("Error from Fuse Coordination FI: " + ex.Message);
            }
        }

        #endregion

        #region Private Methods
        /// <summary>
        /// Gets the feature information of the selected feature
        /// </summary>
        private void GetFeatureInformation()
        {
            short fno = 0;

            try
            {
                Recordset primaryFuseBankRs = m_Components[m_ComponentName].Recordset;
                primaryFuseBankRs.MoveFirst();
                m_selectedFeaturefid = Convert.ToInt32(primaryFuseBankRs.Fields["G3E_FID"].Value);
                fno = Convert.ToInt16(primaryFuseBankRs.Fields["G3E_FNO"].Value);

                IGTKeyObject selectedFeature = m_DataContext.OpenFeature(fno, m_selectedFeaturefid);
                Recordset primaryFuseUnitRs = selectedFeature.Components.GetComponent(primaryFuseUnitCNO).Recordset;

                primaryFuseUnitRs.MoveFirst();
                while (!primaryFuseUnitRs.EOF)
                {
                    m_featureUnitInstanceList.Add
                    (
                       new FeatureHelper
                       {
                           LinkSize = Convert.ToString(primaryFuseUnitRs.Fields["LINK_SIZE_C"].Value),
                           Phase = Convert.ToString(primaryFuseUnitRs.Fields["PHASE_C"].Value)
                       });

                    primaryFuseUnitRs.MoveNext();
                }

                Recordset commonRs = selectedFeature.Components.GetComponent(1).Recordset;
                commonRs.MoveFirst();

                m_selectedFeatureState = Convert.ToString(commonRs.Fields["FEATURE_STATE_C"].Value);
            }
            catch (Exception)
            {
                throw;
            }
        }


        /// <summary>
        /// Method to build the connected fuses (upstream fuses)
        /// </summary>
        /// <param name="p_fid">fid for which protective device needs to be found out</param>
        private void BuildUpStreamFuses(int p_fid)
        {
            short protectiveDeviceFno = 0;
            int protectiveDeviceFid = 0;
            string featureState = string.Empty;

            try
            {
                bool isProposedDevice = false;
                StringBuilder stringBuilder = new StringBuilder();
                stringBuilder.Append(@"SELECT");

                if (m_selectedFeatureState.Equals("PPX") || m_selectedFeatureState.Equals("ABX") || m_selectedFeatureState.Equals("INI") || m_selectedFeatureState.Equals("CLS"))
                {
                    isProposedDevice = false;
                    //stringBuilder.Append("  NVL(PROTECTIVE_DEVICE_FID,0) PD, ");
                    stringBuilder.Append(" DECODE(G3E_FNO, 16, 0, 91, 0, NVL(PROTECTIVE_DEVICE_FID,0)) PD, ");
                    stringBuilder.Append("  NVL((select g3e_fno from common_n where g3e_fid = PROTECTIVE_DEVICE_FID ),0) PD_FNO ");
                    stringBuilder.Append(" FROM CONNECTIVITY_N  START WITH G3E_FID = {0} ");
                    stringBuilder.Append(" CONNECT BY NOCYCLE  PRIOR PROTECTIVE_DEVICE_FID = G3E_FID");
                }
                else if (m_selectedFeatureState.Equals("PPI") || m_selectedFeatureState.Equals("ABI"))
                {
                    isProposedDevice = true;
                    // stringBuilder.Append(" NVL(PP_PROTECTIVE_DEVICE_FID,0) PRPD, ");
                    stringBuilder.Append(" DECODE(G3E_FNO, 16, 0, 91, 0, NVL(PROTECTIVE_DEVICE_FID,0)) PRPD, ");
                    stringBuilder.Append(" NVL((select g3e_fno from common_n where g3e_fid = PP_PROTECTIVE_DEVICE_FID ),0) PRPD_FNO ");
                    stringBuilder.Append(" FROM CONNECTIVITY_N  START WITH G3E_FID = {0}");
                    stringBuilder.Append(" CONNECT BY NOCYCLE  PRIOR PP_PROTECTIVE_DEVICE_FID = G3E_FID");
                }

                if (stringBuilder.Length == 6)
                {
                    return;
                }


                string query = stringBuilder.ToString();
                Recordset upStreamRs = GetRecordSet(string.Format(query, p_fid));

                if (upStreamRs != null && upStreamRs.RecordCount > 0)
                {
                    upStreamRs.MoveFirst();

                    while (!upStreamRs.EOF)
                    {
                        protectiveDeviceFno = isProposedDevice ? Convert.ToInt16(upStreamRs.Fields["PRPD_FNO"].Value) : Convert.ToInt16(upStreamRs.Fields["PD_FNO"].Value);
                        protectiveDeviceFid = isProposedDevice ? Convert.ToInt32(upStreamRs.Fields["PRPD"].Value) : Convert.ToInt32(upStreamRs.Fields["PD"].Value);
                        if (protectiveDeviceFno == 11 || protectiveDeviceFno == 38 || protectiveDeviceFno == 87 || protectiveDeviceFno == 88)
                        {
                            m_upStreamSet.Add(protectiveDeviceFid, protectiveDeviceFno);
                        }
                        upStreamRs.MoveNext();
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Method to collect protected fuses (downsteam fuses) for a given Protective device fid
        /// </summary>
        /// <param name="p_protectiveDeviceFid"> Protective device fid for which protected fuses are to be collected</param>
        private void BuildProtectedDownStreamFuses(int p_protectiveDeviceFid)
        {
            try
            {
                Recordset protectedFusesRs = GetRecordSet(string.Format("SELECT G3E_FNO, G3E_FID FROM CONNECTIVITY_N WHERE {0} IN (PROTECTIVE_DEVICE_FID ,PP_PROTECTIVE_DEVICE_FID) AND G3E_FNO IN (11,38,87,88)", p_protectiveDeviceFid));
                if (protectedFusesRs != null && protectedFusesRs.RecordCount > 0)
                {
                    protectedFusesRs.MoveFirst();
                    while (!protectedFusesRs.EOF)
                    {
                        m_protectedFuseSet.Add(Convert.ToInt32(protectedFusesRs.Fields["g3e_fid"].Value), Convert.ToInt16(protectedFusesRs.Fields["G3E_FNO"].Value));
                        protectedFusesRs.MoveNext();
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Method to get the feature number and feature state for a given feature identifier
        /// </summary>
        /// <param name="p_fid">feature identifier</param>
        /// <param name="p_g3e_fno">feature number</param>
        /// <param name="p_featureState">feature state</param>
        private void GetFNoAndFeatureStateForFid(int p_fid, out short p_g3e_fno, out string p_featureState)
        {
            try
            {
                p_g3e_fno = 0;
                p_featureState = string.Empty;
                Recordset resultRs = GetRecordSet(string.Format("SELECT G3E_FNO, FEATURE_STATE_C FROM COMMON_N WHERE G3E_FID = {0}", p_fid));

                if (resultRs != null && resultRs.RecordCount > 0)
                {
                    resultRs.MoveFirst();
                    p_g3e_fno = Convert.ToInt16(resultRs.Fields["G3E_FNO"].Value);
                    p_featureState = Convert.ToString(resultRs.Fields["FEATURE_STATE_C"].Value);
                }
            }
            catch (Exception)
            {
                throw;
            }
        }


        /// <summary>
        /// Method to Process Fuse Coordination
        /// </summary>
        private void ProcessFuseCoordination()
        {
            Recordset tempRs = null;
            ValidationRuleManager validateMsg = new ValidationRuleManager();

            try
            {
                string errorPriority = Convert.ToString(m_GTArguments.GetArgument(0));

                foreach (KeyValuePair<int, short> keyValue in m_upStreamSet)
                {
                    tempRs = m_DataContext.OpenFeature(keyValue.Value, keyValue.Key).Components.GetComponent(primaryFuseUnitCNO).Recordset;

                    if (tempRs.BOF)
                    {
                        continue;
                    }
                    else
                    {
                        tempRs.MoveFirst();
                    }

                    while (!tempRs.EOF)
                    {
                        string linkSize = Convert.ToString(tempRs.Fields["LINK_SIZE_C"].Value);
                        string phase = Convert.ToString(tempRs.Fields["PHASE_C"].Value);

                        foreach (FeatureHelper activeFeature in m_featureUnitInstanceList)  // loop through each of the units in selected feature
                        {
                            int actFeatureSizeOrdinal = GetSizeOrdinalForLinkSize(activeFeature.LinkSize);
                            int upstreamFeatureSizeOrdinal = GetSizeOrdinalForLinkSize(linkSize);

                            if (actFeatureSizeOrdinal == 0 || upstreamFeatureSizeOrdinal == 0) // Skipping validation check when either of the size ordinals are 0.
                            {
                                continue;
                            }

                            if (activeFeature.Phase == phase)  // Selected feature's phase from unit instance is equal to phase from a unit instance of feature in Upstreamset
                            {
                                if (activeFeature.Phase.Equals("N")) // Skip neutral phase
                                {
                                    continue;
                                }
                                // size ordinal of active feature's unit instance is minimum 2 units less than the size ordinal upsteam feature's unit instance
                                if ((upstreamFeatureSizeOrdinal - actFeatureSizeOrdinal) < 2)
                                {
                                    validateMsg.Rule_Id = "FUSE01";
                                    validateMsg.BuildRuleMessage(GTClassFactory.Create<IGTApplication>(), null);
                                    m_lstErrorMsg.Add(validateMsg.Rule_MSG);
                                    m_lstErrorPriority.Add(errorPriority);
                                }
                            }
                        }

                        tempRs.MoveNext();
                    }

                }
                foreach (KeyValuePair<int, short> keyValue in m_protectedFuseSet)
                {
                    tempRs = m_DataContext.OpenFeature(keyValue.Value, keyValue.Key).Components.GetComponent(primaryFuseUnitCNO).Recordset;

                    if (tempRs.BOF)
                    {
                        continue;
                    }
                    else
                    {
                        tempRs.MoveFirst();
                    }

                    while (!tempRs.EOF)
                    {
                        string linkSize = Convert.ToString(tempRs.Fields["LINK_SIZE_C"].Value);
                        string phase = Convert.ToString(tempRs.Fields["PHASE_C"].Value);

                        foreach (FeatureHelper activeFeature in m_featureUnitInstanceList)  // loop through each of the units in selected feature
                        {
                            int actFeatureSizeOrdinal = GetSizeOrdinalForLinkSize(activeFeature.LinkSize);
                            int downStreamFeatureSizeOrdinal = GetSizeOrdinalForLinkSize(linkSize);

                            if (actFeatureSizeOrdinal == 0 || downStreamFeatureSizeOrdinal == 0) // Skipping validation check when either of the size ordinals are 0.
                            {
                                continue;
                            }

                            if (activeFeature.Phase == phase)  // Selected feature's phase from unit instance is equal to phase from a unit instance of feature in Upstreamset
                            {
                                // size ordinal of active feature's unit instance is minimum 2 units greater than the size ordinal upsteam feature's unit instance
                                if ((actFeatureSizeOrdinal - downStreamFeatureSizeOrdinal) < 2)
                                {
                                    if (activeFeature.Phase.Equals("N")) // Skip neutral phase
                                    {
                                        continue;
                                    }
                                    validateMsg.Rule_Id = "FUSE02";
                                    validateMsg.BuildRuleMessage(GTClassFactory.Create<IGTApplication>(), null);
                                    m_lstErrorMsg.Add(validateMsg.Rule_MSG);
                                    m_lstErrorPriority.Add(errorPriority);
                                }
                            }
                        }
                        tempRs.MoveNext();
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Method to get the size ordinal for input link size
        /// </summary>
        /// <param name="p_linkSize">Link size for which size ordinal to be fetched</param>
        /// <returns>Size ordinal value or 0</returns>
        private int GetSizeOrdinalForLinkSize(string p_linkSize)
        {
            try
            {
                Recordset sizeOrdinalRs = GetRecordSet(String.Format("SELECT SIZE_ORDINAL FROM FUSE_COORDINATION WHERE LINK_SIZE_C = '{0}'", p_linkSize));
                if (sizeOrdinalRs != null && sizeOrdinalRs.RecordCount > 0)
                {
                    sizeOrdinalRs.MoveFirst();
                    return Convert.ToInt16(sizeOrdinalRs.Fields["SIZE_ORDINAL"].Value);
                }
                return 0;
            }
            catch (Exception)
            {
                throw;
            }
        }


        /// <summary>
        /// Method to execute sql query and return the result record set
        /// </summary>
        /// <param name="sqlString"></param>
        /// <returns>Result recordset</returns>
        private Recordset GetRecordSet(string sqlString)
        {
            try
            {
                int outRecords = 0;
                Command command = new Command();
                command.CommandText = sqlString;
                Recordset results = m_DataContext.ExecuteCommand(command, out outRecords);
                return results;
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion

    }
}
