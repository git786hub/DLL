
// Revision History
//  22/06/2018          Hari                       Adjusted code to use custom work point creator
//  31/07/18            Hari                       Fixed bugs per ONCORDEV-1951. (1. RNO Changes  2. Work point number creation )
// User: pnlella    Date: 08/03/19   Time: 15:15 Adjusted code for handling Isolation scenario.

using ADODB;
using Intergraph.GTechnology.API;
using System;
using System.Collections.Generic;


namespace GTechnology.Oncor.CustomAPI
{
    public class NewFeatureCreator
    {
      //  IGTKeyObject m_newFeature;
        internal IGTDataContext m_dataContext { get; set; }
        internal IGTDDCKeyObject m_oldFeature { get; set; }
        internal IGTKeyObject m_newFeature { get; set; }
        internal string m_jobType;
        internal string m_jobStatus;
        internal int m_newG3eId;
        internal string m_modifiedActivityOldFeature;
        private string m_modifiedActivityNewFeature;

        Helper oHelper;
        IsolationScenario oIsolationScenario;

        /// <summary>
        /// Method to start processing feature creation 
        /// </summary>
        public void ProcessFeatureCreation()
        {
            try
            {
                oHelper = new Helper();
                oHelper.m_dataContext = m_dataContext;
                if (CreateNewFeature())
                {
                    
                    oIsolationScenario = new IsolationScenario(m_dataContext);
                    if (oIsolationScenario.CheckIsoScenarioFeature(m_newFeature.FNO,m_newFeature.FID))
                    {
                        oIsolationScenario.DeleteOldIsolationPoint(m_newFeature);
                    }

                    SetActivity(21);
                    SetActivity(22);
                    SetFeatureState();
                    SetStructureID();
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
        
        /// <summary>
        /// Method to create a copy of selected feature
        /// </summary>
        /// <returns></returns>
        private bool CreateNewFeature()
        {
            try
            {               
                IGTComponents oldFeatureComponents = m_dataContext.OpenFeature(m_oldFeature.FNO, m_oldFeature.FID).Components;                           
                m_newFeature = m_dataContext.NewFeature(m_oldFeature.FNO);

                foreach (IGTComponent component in oldFeatureComponents)
                {
                    Recordset rsOldFeature = component.Recordset;
                    IGTComponent componentNewFeature = m_newFeature.Components[component.Name]; // Get old feature equivalent component in new feature
                    if (componentNewFeature != null)
                    {
                        Recordset rsNewFeature = componentNewFeature.Recordset;
                        if (component.CNO == 1)
                        {
                            m_newG3eId = Convert.ToInt32(rsNewFeature.Fields["G3E_ID"].Value);
                        }
                        if (rsNewFeature != null)
                        {
                            int count = 0;
                            if (rsOldFeature.RecordCount>0)
                            {
                                rsOldFeature.MoveFirst();
                            }

                            if (rsNewFeature!=null)
                            {
                                if (rsNewFeature.RecordCount>0)
                                {
                                    rsNewFeature.MoveFirst();
                                }
                            }
                            while (!rsOldFeature.EOF)
                            {

                                //if (rsNewFeature.RecordCount == 0 || rsNewFeature.EOF || count > 0)
                                if (rsNewFeature.RecordCount == 0 || (rsNewFeature.EOF && count > 0))
                                {
                                    rsNewFeature.AddNew("G3E_FID", m_newFeature.FID);
                                    rsNewFeature.Fields["G3E_FNO"].Value = m_newFeature.FNO;
                                    rsNewFeature.Fields["G3E_CNO"].Value = component.CNO;
                                  //  rsNewFeature.Fields["G3E_CID"].Value = component.Recordset.Fields["G3E_CID"].Value;
                                }
                                for (int i = 0; i < rsOldFeature.Fields.Count; i++)
                                {

                                    string fieldName = Convert.ToString(rsOldFeature.Fields[i].Name);
                                    Recordset rs = m_dataContext.MetadataRecordset("G3E_ATTRIBUTEINFO_OPTABLE", "G3E_CNO = " + component.CNO + " AND  G3E_FIELD = '" + fieldName + "'");
                                    if (rs != null && rs.RecordCount > 0)
                                    {
                                        rs.MoveFirst();
                                        if (fieldName == "G3E_GEOMETRY" || (component.Geometry != null && componentNewFeature.Geometry == null))
                                        {
                                            componentNewFeature.Geometry = component.Geometry;
                                        }
                                        if (((Convert.ToInt16(rs.Fields["G3E_EXCLUDEFROMREPLACE"].Value) == 0) || fieldName == "NODE_1_ID" || fieldName == "NODE_2_ID") &&
                                            (((fieldName != "G3E_FID" && fieldName != "G3E_ID" &&
                                            (fieldName.Substring(0, 3) != "G3E") && (fieldName.Substring(0, 3) != "LTT")) || fieldName == "G3E_ALIGNMENT") && fieldName != "ACTIVITY_C" && fieldName != "WR_ID" && Convert.ToInt32(rs.Fields["G3E_ANO"].Value) != 110102))
                                        {
                                            rsNewFeature.Fields[fieldName].Value = rsOldFeature.Fields[fieldName].Value;                                         
                                        }
                                    }
                                }
                                count = count + 1;
                                rsNewFeature.MoveNext();
                                rsOldFeature.MoveNext();
                            }
                        }
                    }
                }
                return true;
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Method to set activity to newly installed feature
        /// </summary>
        /// <param name="cno">component number</param>
        private void SetActivity(short cno)
        {
            string activityValue = "I";
            try
            {
                Recordset ComponentRs = null;
                IGTKeyObject feature = m_dataContext.OpenFeature(m_newFeature.FNO, m_newFeature.FID);
                // Set Activity in CU attributes
                ComponentRs = feature.Components.GetComponent(cno).Recordset;
                if (ComponentRs != null && ComponentRs.RecordCount > 0)
                {
                    ComponentRs.MoveFirst();
                    if (oHelper.CheckForCorrectionModeProperty() || m_jobType == "WR-MAPCOR")
                    {
                        activityValue = "IC";
                    }
                    while (!ComponentRs.EOF)
                    {
                        ComponentRs.Fields["ACTIVITY_C"].Value = activityValue;
                        ComponentRs.MoveNext();
                    }
                    m_modifiedActivityNewFeature = activityValue;
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        private void SetStructureID()
        {
            string sOldFeatureStructureID = string.Empty;
            IGTComponents oldFeatureComponents = m_dataContext.OpenFeature(m_oldFeature.FNO, m_oldFeature.FID).Components;

            if (oldFeatureComponents.GetComponent(1).Recordset != null)
            {
                if (oldFeatureComponents.GetComponent(1).Recordset.RecordCount > 0)
                {
                    oldFeatureComponents.GetComponent(1).Recordset.MoveFirst();
                    sOldFeatureStructureID = Convert.ToString(oldFeatureComponents.GetComponent(1).Recordset.Fields["STRUCTURE_ID"].Value);
                    m_newFeature.Components.GetComponent(1).Recordset.Fields["STRUCTURE_ID"].Value = sOldFeatureStructureID;
                }
            }
        }
        /// <summary>
        /// Method to set the feature state of the installed feature
        /// </summary>
        private void SetFeatureState()
        {
            try
            {
                oHelper.m_fno = m_newFeature.FNO;
                oHelper.m_fid = m_newFeature.FID;
                oHelper.m_featureState = IdentifyFeatureState();
                oHelper.m_replacedFid = m_oldFeature.FID;
                oHelper.SetFeatureState();
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Method to indentify feature state based on jobstatus
        /// </summary>
        /// <returns></returns>
        private string IdentifyFeatureState()
        {
            try
            {
                if (m_jobStatus.ToUpper() == "ASBUILT")
                {
                    return "ABI";
                }
                else if (m_jobStatus.ToUpper() == "CONSTRUCTIONCOMPLETE")
                {
                    return "INI";
                }
                else
                {
                    return "PPI";
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void SynchronizeWP(IGTKeyObject p_keyObject)
        {
            IGTComponents CUComponentsNewFeature = GTClassFactory.Create<IGTComponents>();

            foreach (IGTComponent item in p_keyObject.Components)
            {
                if (item.CNO == 21 || item.CNO == 22)
                {
                    CUComponentsNewFeature.Add(item);
                }
            }
            WorkPointOperations obj = new WorkPointOperations(CUComponentsNewFeature, p_keyObject, m_dataContext);
            obj.DoWorkpointOperations();
        }
    }
}
