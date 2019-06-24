//----------------------------------------------------------------------------+
//        Class: fiOwningCompanyValidation
//  Description: This interface validates whether or not Owning Company is empty when Owner Type is FOREIGN. If owning company is empty then validation error message will be shown.
//----------------------------------------------------------------------------+
//     $Author:: hkonda                                                       $
//       $Date:: 04/08/17                                                     $
//   $Revision:: 1                                                            $
//----------------------------------------------------------------------------+
//    $History:: fiOwningCompanyValidation.cs                                           $
// 
// *****************  Version 1  *****************
// User: hkonda     Date: 26/09/17    Time: 18:00  Desc : Created
// User: hkonda     Date: 04/05/18    Time: 18:00  Desc : Updated code per latest requirement in JIRA ONCORDEV-1632
//----------------------------------------------------------------------------+
using System;
using ADODB;
using Intergraph.GTechnology.API;
using Intergraph.GTechnology.Interfaces;
using System.Collections.Generic;
using System.Windows.Forms;
using gtCommandLogger;

namespace GTechnology.Oncor.CustomAPI
{
    public class fiOwningCompanyValidation : IGTFunctional
    {
        #region Fields
        private GTArguments m_Arguments = null;
        private IGTDataContext m_DataContext = null;
        private IGTComponents m_components;
        private string m_ComponentName;
        private string m_FieldName;
        private const int poleAttributesCno = 11001;
        #endregion

        #region Public properties

        public GTArguments Arguments
        {
            get { return m_Arguments; }
            set { m_Arguments = value; }
        }

        public string ComponentName
        {
            get { return m_ComponentName; }
            set { m_ComponentName = value; }
        }

        public IGTComponents Components
        {
            get { return m_components; }
            set { m_components = value; }
        }

        public IGTDataContext DataContext
        {
            get { return m_DataContext; }
            set { m_DataContext = value; }
        }

        public string FieldName
        {
            get { return m_FieldName; }
            set { m_FieldName = value; }
        }

        public IGTFieldValue FieldValueBeforeChange
        {
            get;
            set;
        }

        public GTFunctionalTypeConstants Type
        {
            get;
            set;
        }

        #endregion

        #region Interface methods
        public void Delete()
        {
        }

        public void Execute()
        {
            string ownerType = string.Empty;
            string configuredOwningCompany = string.Empty;
            try
            {
                configuredOwningCompany = GetConfiguredOwningCompany();
                Recordset commonComponentRs = Components.GetComponent(1).Recordset;

                if (commonComponentRs!=null)
                {
                    if (commonComponentRs.RecordCount>0)
                    {
                        commonComponentRs.MoveFirst();
                    }
                }
                short fno = Convert.ToInt16(commonComponentRs.Fields["G3E_FNO"].Value);
                if (fno != 110)
                    return;

                if (commonComponentRs != null && commonComponentRs.RecordCount > 0)
                {
                    commonComponentRs.MoveFirst();
                    ownerType = Convert.ToString(commonComponentRs.Fields["OWNED_TYPE_C"].Value);
                }
                if (ownerType == "COMPANY")
                {
                    SetOwningCompany(GetConfiguredOwningCompany());
                }

                if (ownerType == "FOREIGN" && configuredOwningCompany == GetOwningCompany())
                {
                    SetOwningCompany(string.Empty);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error during Owning Company FI execution. " + ex.Message, "G/Technology", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }



        public void Validate(out string[] ErrorPriorityArray, out string[] ErrorMessageArray)
        {
            ErrorMessageArray = null;
            ErrorPriorityArray = null;
            List<String> strErrorMsg = new List<String>();
            List<String> strErrorPriority = new List<String>();
            string ownerType = string.Empty;
            ValidationRuleManager validateMsg = new ValidationRuleManager();

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
                    ValidationInterfaceName = "Owning Company",
                    ValidationInterfaceType = "FI",
                };
                gTValidationLogger = new GTValidationLogger(logEntries);

                gTValidationLogger.LogEntry("TIMING", "START", "Owning Company Entry", "N/A", "");
            }

            try
            {
                IGTApplication iGtApplication = GTClassFactory.Create<IGTApplication>();
                iGtApplication.SetStatusBarText(GTStatusPanelConstants.gtaspcMessage, "Owning Company Validation Started");
                string errorPriority = Convert.ToString(m_Arguments.GetArgument(0));
                Recordset commonComponentRs = Components.GetComponent(1).Recordset;
                commonComponentRs.MoveFirst();
                short fno = Convert.ToInt16(commonComponentRs.Fields["G3E_FNO"].Value);

                if (fno != 110)
                {
                   if(gTValidationLogger != null)
                      gTValidationLogger.LogEntry("TIMING", "END", "Owning Company Exit", "N/A", "");
                   return;
                }

                ownerType = Convert.ToString(commonComponentRs.Fields["OWNED_TYPE_C"].Value);

                if (ownerType == "FOREIGN")
                {
                    if (string.IsNullOrEmpty(GetOwningCompany()))
                    {
                        validateMsg.Rule_Id = "OWNC01";
                        validateMsg.BuildRuleMessage(GTClassFactory.Create<IGTApplication>(), null);

                        strErrorMsg.Add(validateMsg.Rule_MSG);
                        strErrorPriority.Add(errorPriority);
                    }
                }

                if (ownerType == "COMPANY")
                {
                    string configuredCompany = GetConfiguredOwningCompany();
                    if (!Convert.ToString(GetOwningCompany()).ToUpper().Equals(Convert.ToString(configuredCompany).ToUpper()))
                    {
                        object[] messArguments = new object[1];
                        messArguments[0] = configuredCompany;
                        validateMsg.Rule_Id = "OWNC02";
                        validateMsg.BuildRuleMessage(GTClassFactory.Create<IGTApplication>(), messArguments);
                        strErrorMsg.Add(validateMsg.Rule_MSG);
                        strErrorPriority.Add(errorPriority);
                    }
                }

                iGtApplication.SetStatusBarText(GTStatusPanelConstants.gtaspcMessage, "Owning Company Validation Completed");
                ErrorMessageArray = strErrorMsg.ToArray();
                ErrorPriorityArray = strErrorPriority.ToArray();

                if(gTValidationLogger != null)
                    gTValidationLogger.LogEntry("TIMING", "END", "Owning Company Exit", "N/A", "");
            }
            catch (Exception ex)
            {
                throw new Exception("Error during Owning Company FI validation. " + ex.Message);
            }
        }
        #endregion

        #region Private methods
        /// <summary>
        /// Gets the configured owning company from the SYS_GENERALPARAMETER table
        /// </summary>
        /// <returns>Configured company</returns>
        private string GetConfiguredOwningCompany()
        {
            try
            {
                Recordset resultRs = GetRecordSet("SELECT PARAM_VALUE FROM SYS_GENERALPARAMETER WHERE PARAM_NAME = 'OwningCompany_Default' AND SUBSYSTEM_NAME = 'OwningCompany'");
                if (resultRs != null && resultRs.RecordCount > 0)
                {
                    resultRs.MoveFirst();
                    return Convert.ToString(resultRs.Fields["PARAM_VALUE"].Value);
                }
                return string.Empty;
            }
            catch (Exception)
            {
                throw;
            }
        }


        /// <summary>
        /// Gets the owning company from the pole attributes component
        /// </summary>
        /// <returns>Owning company</returns>
        private string GetOwningCompany()
        {
            try
            {
                if (Components.GetComponent(poleAttributesCno) == null)
                    return string.Empty;
                Recordset poleAttributesRs = Components.GetComponent(poleAttributesCno).Recordset;
                if (poleAttributesRs != null && poleAttributesRs.RecordCount > 0)
                {
                    poleAttributesRs.MoveFirst();
                    return Convert.ToString(poleAttributesRs.Fields["OWNING_COMPANY_C"].Value);
                }
                return string.Empty;
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Sets the supplied value as the owning company in the pole attributes component
        /// </summary>
        private void SetOwningCompany(string owningCompany)
        {
            try
            {
                if (Components.GetComponent(poleAttributesCno) == null)
                    return;
                Recordset poleAttributesRs = Components.GetComponent(poleAttributesCno).Recordset;
                if (poleAttributesRs.RecordCount > 0)
                {
                    poleAttributesRs.MoveFirst();
                    poleAttributesRs.Fields["OWNING_COMPANY_C"].Value = owningCompany;
                }
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
        /// <returns></returns>
        public Recordset GetRecordSet(string sqlString)
        {
            try
            {
                int outRecords = 0;
                Command command = new Command();
                command.CommandText = sqlString;
                Recordset results = DataContext.ExecuteCommand(command, out outRecords);
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
