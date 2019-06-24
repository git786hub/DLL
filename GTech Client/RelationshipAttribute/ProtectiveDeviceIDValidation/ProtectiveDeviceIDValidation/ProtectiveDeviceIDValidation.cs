//----------------------------------------------------------------------------+
//  Class: raiProtectiveDeviceID
//  Description: 
//		This interface validates the PROTECTIVE_DEVICE_FID between active and related feature.
//      At the time of relationship establishment, if PROTECTIVE_DEVICE_FID is null, it is set
//      for active feature from the PROTECTIVE_DEVICE_FID of the related feature. 
//      At the time of validation,if the PROTECTIVE_DEVICE_FID does not match, error is thrown.
//      If the conneted device is a protective device, error is shown to indicate that
//      PROTECTIVE_DEVICE_FID does not match between active feature and the related protective device
//----------------------------------------------------------------------------+
//  $Author::   Shubham Agarwal                                                       $
//  $Date::     30/07/17                                                                $
//  $Revision:: 1                                                                   $
//----------------------------------------------------------------------------+
//  $History:: raiProtectiveDeviceID.cs                                             $
// 
// *****************  Version 1  *****************
//  User: sagarwal     Date: 30/07/17     Initial Creation  
//  User: sagarwal     Date: 30/04/18     Modified per the changes in design to accomodate Upstream and Downstream nodes
//----------------------------------------------------------------------------+

using System;
using Intergraph.GTechnology.API;
using Intergraph.GTechnology.Interfaces;
using System.Windows.Forms;
using System.Collections.Generic;
using GTechnology.Oncor.CustomAPI;
using gtCommandLogger;

namespace ONCOR.GTechnology.Oncor.CustomAPI
{
    public class raiProtectiveDeviceID : IGTRelationshipAttribute
    {
        private int m_iActiveANO = 0;
        private string m_sActiveComponentName = string.Empty;
        private IGTComponents m_oActiveComponents = null;
        private string m_sActiveFieldName = string.Empty;
        private IGTFieldValue m_oActiveFieldValue = null;
        private GTArguments m_oGTArguments = null;
        private IGTDataContext m_oDataContext = null;
        private string m_sPriority = string.Empty;
        private int m_iRelatedANO = 0;
        private string m_sRelatedComponentName = string.Empty;
        private IGTComponents m_oRelatedComponents = null;
        private string m_sRelatedFieldName = string.Empty;
        private IGTFieldValue m_oRelatedFieldValue = null;
        private int[] arrProtectiveDevicesFNOs = { 11, 87, 38, 88, 14, 15, 16, 91, 59, 98, 60, 99 };
       // private int[] arrSecondaryNetworkFeatures = { 23, 52, 53, 54, 55, 63, 86, 94, 95, 96, 97, 154, 155, 161, 162 };
        private List<short> m_SecondaryFNOs = new List<short>(new short[] { 23, 52, 53, 54, 55, 63, 86, 94, 95, 96, 97, 154, 155, 161, 162 });     // List of Secondary features.
        public int ActiveANO
        {
            get
            {
                return m_iActiveANO;
            }

            set
            {
                m_iActiveANO = value;
            }
        }

        public string ActiveComponentName
        {
            get
            {
                return m_sActiveComponentName;
            }

            set
            {
                m_sActiveComponentName = value;
            }
        }

        public IGTComponents ActiveComponents
        {
            get
            {
                return m_oActiveComponents;
            }

            set
            {
                m_oActiveComponents = value;
            }
        }

        public string ActiveFieldName
        {
            get
            {
                return m_sActiveFieldName;
            }

            set
            {
                m_sActiveFieldName = value;
            }
        }

        public IGTFieldValue ActiveFieldValue
        {
            get
            {
                return m_oActiveFieldValue;
            }

            set
            {
                m_oActiveFieldValue = value;
            }
        }

        public GTArguments Arguments
        {
            get
            {
                return m_oGTArguments;
            }

            set
            {
                m_oGTArguments = value;
            }
        }

        public IGTDataContext DataContext
        {
            get
            {
                return m_oDataContext;
            }

            set
            {
                m_oDataContext = value;
            }
        }

        public string Priority
        {
            get
            {
                return m_sPriority;
            }

            set
            {
                m_sPriority = value;
            }
        }

        public int RelatedANO
        {
            get
            {
                return m_iRelatedANO;
            }

            set
            {
                m_iRelatedANO = value;
            }
        }

        public string RelatedComponentName
        {
            get
            {
                return m_sRelatedComponentName;
            }

            set
            {
                m_sRelatedComponentName = value;
            }
        }

        public IGTComponents RelatedComponents
        {
            get
            {
                return m_oRelatedComponents;
            }

            set
            {
                m_oRelatedComponents = value;
            }
        }

        public string RelatedFieldName
        {
            get
            {
                return m_sRelatedFieldName;
            }

            set
            {
                m_sRelatedFieldName = value;
            }
        }

        public IGTFieldValue RelatedFieldValue
        {
            get
            {
                return m_oRelatedFieldValue;
            }

            set
            {
                m_oRelatedFieldValue = value;
            }
        }

        private void ProcessAfterEstablish(string p_protectivedeviceIDUserName)
        {
            string stRelatedFeatureState = null;
            try
            {
                IGTKeyObject oRelatedKeyObject = null;

                short iFNORelated = Convert.ToInt16(RelatedComponents[RelatedComponentName].Recordset.Fields["G3E_FNO"].Value);
                int iFIDRelated = Convert.ToInt32(RelatedComponents[RelatedComponentName].Recordset.Fields["G3E_FID"].Value);
                oRelatedKeyObject = DataContext.OpenFeature(iFNORelated, iFIDRelated);

                if (p_protectivedeviceIDUserName == "PROTECTIVE_DEVICE_FID")
                {
                    if (oRelatedKeyObject.Components["COMMON_N"] != null && oRelatedKeyObject.Components["COMMON_N"].Recordset != null && oRelatedKeyObject.Components["COMMON_N"].Recordset.RecordCount > 0)
                    {
                        ActiveComponents["COMMON_N"].Recordset.MoveFirst();
                        stRelatedFeatureState = Convert.ToString(ActiveComponents["COMMON_N"].Recordset.Fields["FEATURE_STATE_C"].Value);
                    }

                    if (stRelatedFeatureState == "PPI" && stRelatedFeatureState == "ABI")
                    {
                        return;
                    }
                }

                FeatureProperties oRelated = new FeatureProperties(oRelatedKeyObject, m_sRelatedComponentName, "");
                m_oActiveComponents[m_sActiveComponentName].Recordset.Fields[p_protectivedeviceIDUserName].Value = oRelated.ProtectiveDeviceIDToSet;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Method to find whether a given secondary feature is a network feature based on the FEEDER_TYPE_C value of NETWORK
        /// </summary>
        /// <param name="fid"></param>
        /// <param name="fno"></param>
        /// <returns></returns>

        private bool IsNetworkFeature(int fid, short fno)
        {
            bool bReturn = false;

            IGTKeyObject keyObject = m_oDataContext.OpenFeature(fno, fid);
            if (keyObject.Components.GetComponent(11) != null)
            {
                if (keyObject.Components.GetComponent(11).Recordset != null)
                {
                    if (keyObject.Components.GetComponent(11).Recordset.RecordCount > 0)
                    {
                        keyObject.Components.GetComponent(11).Recordset.MoveFirst();
                        if (keyObject.Components.GetComponent(11).Recordset.Fields["FEEDER_TYPE_C"].Value.Equals("NETWORK"))
                        {
                            bReturn = true;
                        }
                    }
                }
            }
            return bReturn;
        }

        private void ProcessValidate(ref List<string> lstPriority, ref List<string> lstErrors)
        {
            String sRelatedFeatureName = string.Empty;
            string sActiveFeatureName = string.Empty;

            string errorMessage = string.Empty;

            try
            {
                ValidateFeatureConditions oValidation = new ValidateFeatureConditions();

                short iFNORelated = 0;
                short iFNOActive = 0;
                int iFIDRelated = 0;
                int iFIDActive = 0;
                IGTKeyObject oActiveKeyObject = null;
                IGTKeyObject oRelatedKeyObject = null;

                iFNOActive = Convert.ToInt16(ActiveComponents[ActiveComponentName].Recordset.Fields["G3E_FNO"].Value);
                iFNORelated = Convert.ToInt16(RelatedComponents[RelatedComponentName].Recordset.Fields["G3E_FNO"].Value);              

                iFIDActive = Convert.ToInt32(ActiveComponents[ActiveComponentName].Recordset.Fields["G3E_FID"].Value);
                iFIDRelated = Convert.ToInt32(RelatedComponents[RelatedComponentName].Recordset.Fields["G3E_FID"].Value);

                //We do not want to validate against the Network Secondary Features either active nor related
                if ((m_SecondaryFNOs.Contains(iFNOActive) && IsNetworkFeature(iFIDActive, iFNOActive)) ||
                    (m_SecondaryFNOs.Contains(iFNORelated) && IsNetworkFeature(iFIDRelated, iFNORelated))) return;

                oActiveKeyObject = DataContext.OpenFeature(iFNOActive, iFIDActive);
                oRelatedKeyObject = DataContext.OpenFeature(iFNORelated, iFIDRelated);

                FeatureProperties oActiveFeature = new FeatureProperties(oActiveKeyObject, m_sActiveComponentName,"");
                FeatureProperties oRelatedFeature = new FeatureProperties(oRelatedKeyObject, m_sRelatedComponentName,"");

                bool? isRelatedFeatureDownStreamNullable = oValidation.IsRelatedFeatureDownstream(oActiveFeature, oRelatedFeature);
                if (isRelatedFeatureDownStreamNullable == null) return; //Parallel branches, no need to validate

                bool isRelatedFeatureDownStream = isRelatedFeatureDownStreamNullable == true ? true : false;

                int oActiveProtectioDeviceID = oActiveFeature.ProtectioDeviceID.Equals(DBNull.Value) ? 0 : Convert.ToInt32(oActiveFeature.ProtectioDeviceID);
                int oRelatedProtectioDeviceID = oRelatedFeature.ProtectioDeviceID.Equals(DBNull.Value) ? 0 : Convert.ToInt32(oRelatedFeature.ProtectioDeviceID);


                if (oActiveFeature.IsProtectiveDevice && !oActiveFeature.IsBussedTransformer && isRelatedFeatureDownStream)
                {
                    if (oRelatedProtectioDeviceID != oActiveFeature.FID)
                    {
                        lstErrors.Add("Connected Protective Device ID does not match this protective device’s FID.");
                        lstPriority.Add(Priority);
                        return;
                    }
                   else
                    {
                        return;
                    }
                }               

                if (oActiveFeature.IsBussedTransformer && isRelatedFeatureDownStream)
                {
                    if (oRelatedProtectioDeviceID != oActiveFeature.ProtectiveDeviceIDToSet)
                    {
                        lstErrors.Add("Connected Protective Device ID does not match this protective device’s Tie Transformer ID.");
                        lstPriority.Add(Priority);
                        return;
                    }
                    else
                    {
                        return;
                    }
                }
                
                if (oRelatedFeature.IsProtectiveDevice && !oRelatedFeature.IsBussedTransformer && !isRelatedFeatureDownStream)
                {
                    if (oActiveProtectioDeviceID != oRelatedFeature.FID)
                    {
                        lstErrors.Add("Protective Device ID does not match the connected protective device’s FID.");
                        lstPriority.Add(Priority);
                        return;
                    }
                    else
                    {
                        return;
                    }
                }

                if (oRelatedFeature.IsBussedTransformer && !isRelatedFeatureDownStream)
                {
                    if (oActiveProtectioDeviceID != oRelatedFeature.ProtectiveDeviceIDToSet)
                    {
                        lstErrors.Add("Protective Device ID does not match the connected protective device’s Tie Transformer ID.");
                        lstPriority.Add(Priority);
                        return;
                    }
                    else
                    {
                        return;
                    }
                }
                

                if (oActiveProtectioDeviceID != oRelatedProtectioDeviceID)
                {
                    lstErrors.Add("Protective Device ID does not agree with connected feature.");
                    lstPriority.Add(Priority);
                    return;
                }
                else if (oActiveProtectioDeviceID == oRelatedProtectioDeviceID)
                {
                    return;
                }                
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void AfterEstablish()
        {
            try
            {
                string strFeatureState = null;

                IGTKeyObject oActiveKeyObject = null;
                IGTKeyObject oRelatedKeyObject = null;

                short iFNOActive = Convert.ToInt16(ActiveComponents[ActiveComponentName].Recordset.Fields["G3E_FNO"].Value);
                short iFNORelated = Convert.ToInt16(RelatedComponents[RelatedComponentName].Recordset.Fields["G3E_FNO"].Value);

                int iFIDActive = Convert.ToInt32(ActiveComponents[ActiveComponentName].Recordset.Fields["G3E_FID"].Value);
                int iFIDRelated = Convert.ToInt32(RelatedComponents[RelatedComponentName].Recordset.Fields["G3E_FID"].Value);

                oActiveKeyObject = DataContext.OpenFeature(iFNOActive, iFIDActive);
                oRelatedKeyObject = DataContext.OpenFeature(iFNORelated, iFIDRelated);


                FeatureProperties oActiveFeature = new FeatureProperties(oActiveKeyObject, m_sActiveComponentName, "");
                FeatureProperties oRelatedFeature = new FeatureProperties(oRelatedKeyObject, m_sRelatedComponentName, "");

                if (oRelatedFeature.UpstreamNode != 0 && oRelatedFeature.UpstreamNode == oActiveFeature.DownStreamNode) //Affected Feature is U/S of related feature
                {
                    return;
                }                

                if (ActiveComponents["COMMON_N"] != null && ActiveComponents["COMMON_N"].Recordset != null && ActiveComponents["COMMON_N"].Recordset.RecordCount>0)
                {
                    ActiveComponents["COMMON_N"].Recordset.MoveFirst();
                    strFeatureState = Convert.ToString(ActiveComponents["COMMON_N"].Recordset.Fields["FEATURE_STATE_C"].Value);
                }

                if (strFeatureState != null && (strFeatureState == "PPI" || strFeatureState == "ABI"))
                {
                    ProcessAfterEstablish("PP_PROTECTIVE_DEVICE_FID");
                }
                if (strFeatureState != null && strFeatureState != "PPI" && strFeatureState != "ABI" &&
                     ActiveComponents[ActiveComponentName] != null && ActiveComponents[ActiveComponentName].Recordset != null &&
                    ActiveComponents[ActiveComponentName].Recordset.RecordCount > 0 && 
                    ActiveComponents[ActiveComponentName].Recordset.Fields["PROTECTIVE_DEVICE_FID"].Value.GetType() == typeof(DBNull))
                {
                    ProcessAfterEstablish("PROTECTIVE_DEVICE_FID");
                }                
                
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error in raiProtectiveDeviceID relationship attribute interface" + ex.Message);
            }
        }

        public void Validate(out string[] ErrorPriorityArray, out string[] ErrorMessageArray)
        {
            ErrorPriorityArray = new string[1];
            ErrorMessageArray = new string[1];

            List<string> lstErrors = new List<string>();
            List<string> lstPriority = new List<string>();

            GTValidationLogger gTValidationLogger = null;

            int activeFID = 0;
            int relatedFID = 0;

            string activeFieldValue = string.Empty;
            string relatedFieldValue = string.Empty;

            IGTComponent activeComponent = ActiveComponents[ActiveComponentName];
            if (activeComponent != null && activeComponent.Recordset != null && activeComponent.Recordset.RecordCount > 0)
            {
                activeFID = int.Parse(activeComponent.Recordset.Fields["G3E_FID"].Value.ToString());
                activeFieldValue = Convert.ToString(activeComponent.Recordset.Fields[ActiveFieldName].Value);
            }

            IGTComponent relatedComponent = RelatedComponents[RelatedComponentName];
            if (relatedComponent != null && relatedComponent.Recordset != null && relatedComponent.Recordset.RecordCount > 0)
            {
                relatedFID = int.Parse(relatedComponent.Recordset.Fields["G3E_FID"].Value.ToString());
                relatedFieldValue = Convert.ToString(relatedComponent.Recordset.Fields[RelatedFieldName].Value);
            }

            if (new gtLogHelper().CheckIfLoggingIsEnabled())
            {
                LogEntries logEntries = new LogEntries
                {
                    ActiveComponentName = ActiveComponentName,
                    ActiveFID = activeFID,
                    ActiveFieldName = ActiveFieldName,
                    ActiveFieldValue = activeFieldValue,
                    JobID = DataContext.ActiveJob,
                    RelatedComponentName = RelatedComponentName,
                    RelatedFID = relatedFID,
                    RelatedFieldName = RelatedFieldName,
                    RelatedFieldValue = relatedFieldValue,
                    ValidationInterfaceName = "Protective Device Validation",
                    ValidationInterfaceType = "RAI",
                };
                gTValidationLogger = new GTValidationLogger(logEntries);

                gTValidationLogger.LogEntry("TIMING", "START", "Protective Device Validation Entry", "N/A", "");
            }


            try
            {
                ProcessValidate(ref lstPriority, ref lstErrors);
              //  ProcessValidate(ref lstPriority, ref lstErrors, "PP_PROTECTIVE_DEVICE_FID");

                if (lstErrors.Count > 0)
                {
                    ErrorPriorityArray = lstPriority.ToArray();
                    ErrorMessageArray = lstErrors.ToArray();
                }

                if(gTValidationLogger != null)
                    gTValidationLogger.LogEntry("TIMING", "END", "Protective Device Validation Exit", "N/A", "");
            }
            catch (Exception)
            {
                throw; //Just throwing error so that it will be added to the Validation Errors grid automatically               
            }
        }
    }
}
