//----------------------------------------------------------------------------+
//        Class: fiProtectiveDeviceValidation
//  Description: 
//		Validates that a single phase or two phase feature is not directly protected by a Substation Breaker.
//      Validates that a Recloser is directly protected by a Substation Breaker.
//----------------------------------------------------------------------------+
//     $Author:: kappana                                                      $
//       $Date:: 07/07/17                                                     $
//   $Revision:: 1                                                            $
//----------------------------------------------------------------------------+
//    $History:: fiProtectiveDeviceValidation.cs                                           $

//User   Date         Comments
//Hari   30/05/2018   Code changes done per JIRA ONCORDEV-1719
//Prathyusha  15/11/2018   Code changes done per JIRA ONCORDEV-2169(Exclude Transformer – OH and Transformer – OH Network features from receiving a validation error when they are protected by a Substation Breaker).
//Prathyusha  14/02/2018   Code changes done per JIRA ONCORDEV-2563(Exclude Primary Fuses and Reclosers features(both network and non-network) from receiving a validation error when they are protected by a Substation Breaker).
//----------------------------------------------------------------------------+
using System;
using System.Collections.Generic;
using Intergraph.GTechnology.API;
using Intergraph.GTechnology.Interfaces;
using ADODB;
using gtCommandLogger;

namespace GTechnology.Oncor.CustomAPI
{
    public class fiProtectiveDeviceValidation : IGTFunctional
    {
        #region Private Variables
        private GTArguments m_arguments;
        private string m_componentName;
        private IGTComponents m_components;
        private IGTDataContext m_dataContext;
        private string m_fieldName;
        private IGTFieldValue m_fieldValueBeforeChange;
        private GTFunctionalTypeConstants m_type;
        #endregion

        #region Properities
        public GTArguments Arguments
        {
            get
            {
                return m_arguments;
            }

            set
            {
                m_arguments = value;
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

        #region IGTFunctional Methods
        public void Delete()
        {

        }

        public void Execute()
        {

        }

        /// <summary>
        /// Protective Device validation.
        /// </summary>
        /// <param name="ErrorPriorityArray">Error messages that need to be displayed after validation</param>
        /// <param name="ErrorMessageArray">Error message Priority that need to be displayed after validation</param>
        public void Validate(out string[] ErrorPriorityArray, out string[] ErrorMessageArray)
        {
            ErrorMessageArray = null;
            ErrorPriorityArray = null;
            List<string> lstErrorMsg = new List<string>();
            List<string> lstErrorPriority = new List<string>();
            ValidationRuleManager validateMsg = new ValidationRuleManager();
            IGTApplication gtApp = (IGTApplication)GTClassFactory.Create<IGTApplication>();
            List<int> lstFeaturesExcludedFromVal = new List<int> { 11, 14, 15, 38, 59, 87, 88, 98 };

            IGTComponent comp = Components[ComponentName];
            GTValidationLogger gTValidationLogger = null;
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
                    ValidationInterfaceName = "Protective Device Validation",
                    ValidationInterfaceType = "FI",
                };
                gTValidationLogger = new GTValidationLogger(logEntries);

                gTValidationLogger.LogEntry("TIMING", "START", "Protective Device Validation Entry", "N/A", "");
            }

            try
            {
                gtApp.SetStatusBarText(GTStatusPanelConstants.gtaspcMessage, "Protective Device Validation Started.");
                if (comp != null)
                {
                    String phase = comp.Recordset.Fields["PHASE_ALPHA"].Value.ToString();
                    int protectiveDeviceFID = 0;
                    int proposedProtectiveDeviceFID = 0;
                    if (!String.IsNullOrEmpty(Convert.ToString(comp.Recordset.Fields["PROTECTIVE_DEVICE_FID"].Value)))
                    {
                        protectiveDeviceFID = Convert.ToInt32(comp.Recordset.Fields["PROTECTIVE_DEVICE_FID"].Value);
                    }
                    if (!String.IsNullOrEmpty(Convert.ToString(comp.Recordset.Fields["PP_PROTECTIVE_DEVICE_FID"].Value)))
                    {
                        proposedProtectiveDeviceFID = Convert.ToInt32(comp.Recordset.Fields["PP_PROTECTIVE_DEVICE_FID"].Value);
                    }

                    int phaseCount = 0;
                    bool isSubstationBreaker = false;

                    //Feature Phase count is 1 or 2 and Protective Device ID indicates a Substation Breaker

                    if (phase.Length > 0 && phase != "*")//(phase != "*" || phase != "N"))
                    {
                        phaseCount = phase.Length;
                    }

                    if (CheckIsFIDSubstation(protectiveDeviceFID, gtApp) || CheckIsFIDSubstation(proposedProtectiveDeviceFID, gtApp))
                    {
                        isSubstationBreaker = true;
                    }

                    if ((phaseCount == 1 || phaseCount == 2) && isSubstationBreaker && !lstFeaturesExcludedFromVal.Contains(Convert.ToInt32(comp.Recordset.Fields["G3E_FNO"].Value)))                       
                    {
                        validateMsg.Rule_Id = "PDEV01";
                        validateMsg.BuildRuleMessage(GTClassFactory.Create<IGTApplication>(), null);

                        lstErrorMsg.Add(validateMsg.Rule_MSG);
                        lstErrorPriority.Add(Arguments.GetArgument(0).ToString());
                    }

                    // Protective Device ID or Proposed Protective Device ID indicates a feature other than Substation Breaker or Recloser

                    if (Convert.ToInt32(comp.Recordset.Fields["G3E_FNO"].Value) == 14 || Convert.ToInt32(comp.Recordset.Fields["G3E_FNO"].Value) == 15)
                    {
                        bool isRecloser = false;
                        if (IsProtectiveDeviceRecloser(protectiveDeviceFID, Convert.ToInt32(comp.Recordset.Fields["G3E_FNO"].Value), gtApp) ||
                             IsProtectiveDeviceRecloser(proposedProtectiveDeviceFID, Convert.ToInt32(comp.Recordset.Fields["G3E_FNO"].Value), gtApp))
                        {
                            isRecloser = true;
                        }

                        if (!isRecloser && !isSubstationBreaker)
                        {
                            validateMsg.Rule_Id = "PDEV02";
                            validateMsg.BuildRuleMessage(GTClassFactory.Create<IGTApplication>(), null);

                            lstErrorMsg.Add(validateMsg.Rule_MSG);
                            lstErrorPriority.Add(Arguments.GetArgument(0).ToString());
                        }
                    }

                    ErrorMessageArray = lstErrorMsg.ToArray();
                    ErrorPriorityArray = lstErrorPriority.ToArray();
                }
                gtApp.SetStatusBarText(GTStatusPanelConstants.gtaspcMessage, "Protective Device Validation Completed.");

                if (gTValidationLogger != null)
                    gTValidationLogger.LogEntry("TIMING", "END", "Protective Device Validation Exit", "N/A", "");
            }
            catch (Exception ex)
            {
                throw new Exception("Error during Protective Device Validation" + ex.Message);
            }
            finally
            {
                comp = null;
                gtApp = null;
            }

        }

        /// <summary>
        /// Method to check give fid belongs to substation breaker or not
        /// </summary>
        /// <param name="protectiveDeviceFID">FID</param>
        /// <param name="gtApp">Application object</param>
        /// <returns></returns>
        private bool CheckIsFIDSubstation(int protectiveDeviceFID, IGTApplication gtApp)
        {
            bool isSubstationBreaker = false;
            int recordsEffected = 1;
            Recordset rsResult;

            try
            {
                if (protectiveDeviceFID == 0)
                {
                    return isSubstationBreaker;
                }
                String strSQL = string.Empty;
                strSQL += "SELECT G3E_FNO from COMMON_N where G3E_FID=" + protectiveDeviceFID + " AND G3E_FNO=16";

                rsResult = gtApp.DataContext.Execute(strSQL, out recordsEffected, Convert.ToInt32(CommandTypeEnum.adCmdText));

                if (rsResult.RecordCount != 0)
                {
                    isSubstationBreaker = true;
                }
                rsResult.Close();
            }
            catch (Exception ex)
            {
                throw new Exception("Error during Protective Device Validation method checkIsFIDSubstation()" + ex.Message);
            }
            finally
            {
                rsResult = null;
            }

            return isSubstationBreaker;
        }

        private bool IsProtectiveDeviceRecloser(int protectiveDeviceFID, int protectiveDeviceFNO, IGTApplication gtApp)
        {
            bool isRecloser = false;
            int recordsEffected = 1;
            Recordset rsResult;

            try
            {
                if (protectiveDeviceFID == 0)
                {
                    return isRecloser;
                }

                String strSQL = string.Empty;
                strSQL += "SELECT G3E_FNO from COMMON_N where G3E_FID=" + protectiveDeviceFID + " AND G3E_FNO in (14,15)";

                rsResult = gtApp.DataContext.Execute(strSQL, out recordsEffected, Convert.ToInt32(CommandTypeEnum.adCmdText));

                if (rsResult.RecordCount != 0)
                {
                    isRecloser = true;
                }
                rsResult.Close();
            }
            catch (Exception ex)
            {
                throw new Exception("Error during Protective Device Validation method checkIsFIDSubstation()" + ex.Message);
            }
            finally
            {
                rsResult = null;
            }

            return isRecloser;
        }
        #endregion

    }
}
