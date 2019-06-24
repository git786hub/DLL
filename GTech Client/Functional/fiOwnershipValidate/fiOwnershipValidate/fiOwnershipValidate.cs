// ===================================================
//  Copyright 2017 Intergraph Corp.
//  File Name: fiPhasePositionValidate.cs
// 
//  Description:   	This interface validates that for span features defined as having two owners, two different owning features are specified.
//
//  Remarks:
// 
//  Modification History
//  Date                Author                      Comments
//  --------            --------------              --------
//  29/06/2017          Pramod                  Implemented  Business Rule as per JIRA 542
//  12/09/2017          Shubham                 Modified per the design document and configured error priority in metadata. 
//                                              Also changed the Owner ID comparision logic
// ======================================================
using Intergraph.GTechnology.API;
using Intergraph.GTechnology.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using gtCommandLogger;

namespace GTechnology.Oncor.CustomAPI
{
    public class fiOwnershipValidate : IGTFunctional
    {

        private GTArguments m_arguments = null;
        private IGTDataContext m_dataContext = null;
        private IGTComponents m_components;
        private string m_componentName;
        private string m_fieldName;

        public GTArguments Arguments
        {
            get { return m_arguments; }
            set { m_arguments = value; }
        }

        public string ComponentName
        {
            get { return m_componentName; }
            set { m_componentName = value; }
        }

        public IGTComponents Components
        {
            get { return m_components; }
            set { m_components = value; }
        }

        public IGTDataContext DataContext
        {
            get { return m_dataContext; }
            set { m_dataContext = value; }
        }


        public void Delete()
        {
        }

        public void Execute()
        {
        }

        public string FieldName
        {
            get { return m_fieldName; }
            set { m_fieldName = value; }
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

        public IGTComponents Components1
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

        public string ComponentName1
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

        public void Validate(out string[] ErrorPriorityArray, out string[] ErrorMessageArray)
        {
           

            ADODB.Recordset rs = null;
            int[] spanFeatures = {8,84, 9,85,53,96,63,97};
            
            Int16 fnoActiveFeature = 0;           
            int owner1ID = 0;
            int owner2ID = 0;

            List<string> errorMsg = new List<string>();
            List<string> errorPriority = new List<string>();
            object[] messArguments = new object[2];

            bool passedValidation = true;
            ErrorPriorityArray = null;
            ErrorMessageArray = null;
            string strActiveFeatureState = null;
            string strOwner1FeatureState = null;
            string strOwner2FeatureState = null;

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
                    ValidationInterfaceName = "Ownership Validation",
                    ValidationInterfaceType = "FI",
                };
                gTValidationLogger = new GTValidationLogger(logEntries);

                gTValidationLogger.LogEntry("TIMING", "START", "Ownership Validation Entry", "N/A", "");
            }


            IGTApplication gTApplication = GTClassFactory.Create<IGTApplication>();
            ValidationRuleManager validateMsg = new ValidationRuleManager();
            try
            {
                IGTComponent gtComponent = m_components[m_componentName];
                if (gtComponent != null)
                {
                    rs = gtComponent.Recordset;
                    if (rs != null && rs.RecordCount > 0)

                    rs.MoveFirst();
                    fnoActiveFeature = Convert.ToInt16(rs.Fields["G3E_FNO"].Value);


                    if ((!(rs.Fields["OWNER1_ID"].Value is DBNull) && Convert.ToInt32(rs.Fields["OWNER1_ID"].Value) > 0))
                    {
                        owner1ID = Convert.ToInt32(rs.Fields["OWNER1_ID"].Value);                      
                    }

                    if ((!(rs.Fields["OWNER2_ID"].Value is DBNull) && Convert.ToInt32(rs.Fields["OWNER2_ID"].Value) > 0))
                    {
                        owner2ID = Convert.ToInt32(rs.Fields["OWNER2_ID"].Value);
                    }

                    if (spanFeatures.Contains<int>(fnoActiveFeature))
                    {
                        if (owner1ID == owner2ID)
                        {
                            passedValidation = false;
                        }
                    }
                    if (!passedValidation)
                    {
                       
                        validateMsg.Rule_Id = "OWN01";
                        validateMsg.BuildRuleMessage(gTApplication, null);

                        errorMsg.Add(validateMsg.Rule_MSG);
                        errorPriority.Add(Convert.ToString(m_arguments.GetArgument(0)));                        
                    }

                    if(!(rs.Fields["FEATURE_STATE_C"].Value is DBNull))
                    {
                        strActiveFeatureState = (String)rs.Fields["FEATURE_STATE_C"].Value;
                        if (strActiveFeatureState == "INI" || strActiveFeatureState == "CLS" || strActiveFeatureState == "PPI" || strActiveFeatureState == "PPX"
                            || strActiveFeatureState == "ABI" || strActiveFeatureState == "ABX")
                        {
                            CheckOwnerShipStatus(owner1ID, owner2ID, errorMsg, errorPriority, ref strOwner1FeatureState, ref strOwner2FeatureState, gTApplication, validateMsg, strActiveFeatureState);
                        }
                    }

                    ErrorPriorityArray = errorPriority.ToArray();
                    ErrorMessageArray = errorMsg.ToArray();
                }

                if(gTValidationLogger != null)
                      gTValidationLogger.LogEntry("TIMING", "END", "Ownership Validation Exit", "N/A", "");

            }
            catch (Exception ex)
            {
                throw new Exception("Error in Ownership Validaiton FI" + ex.Message);
            }            
        }

        /// <summary>
        /// Method is used to add the error in message array id rules are satisfied.
        /// </summary>
        /// <param name="owner1ID"></param>
        /// <param name="owner2ID"></param>
        /// <param name="errorMsg"></param>
        /// <param name="errorPriority"></param>
        /// <param name="strOwner1FeatureState"></param>
        /// <param name="strOwner2FeatureState"></param>
        /// <param name="gTApplication"></param>
        /// <param name="validateMsg"></param>
        /// <param name="strFeatureState"></param>

        private void CheckOwnerShipStatus(int owner1ID, int owner2ID, List<string> errorMsg, List<string> errorPriority, ref string strOwner1FeatureState, ref string strOwner2FeatureState, 
            IGTApplication gTApplication, ValidationRuleManager validateMsg , string strFeatureState)
        {
            try
            {
                object[] messArguments = new object[2];

                if (owner1ID > 0)
                {
                    strOwner1FeatureState = GetOwnerStatus(owner1ID);
                    if (strOwner1FeatureState == "PPR" || strOwner1FeatureState == "PPA" || strOwner1FeatureState == "ABR" || strOwner1FeatureState == "ABA" ||
                        strOwner1FeatureState == "OSR" || strOwner1FeatureState == "OSA" || strOwner1FeatureState == "LIP")
                    {
                        messArguments[0] = GetStatusAbbreviation(strFeatureState);
                        messArguments[1] = GetStatusAbbreviation(strOwner1FeatureState);

                        validateMsg.Rule_Id = "OWN02";
                        validateMsg.BuildRuleMessage(gTApplication, messArguments);

                        errorMsg.Add(validateMsg.Rule_MSG);
                        errorPriority.Add(Convert.ToString(m_arguments.GetArgument(0)));
                    }
                    else if (owner2ID > 0)
                    {
                        strOwner2FeatureState = GetOwnerStatus(owner2ID);
                        if (strOwner2FeatureState == "PPR" || strOwner2FeatureState == "PPA" || strOwner2FeatureState == "ABR" || strOwner2FeatureState == "ABA" ||
                        strOwner2FeatureState == "OSR" || strOwner2FeatureState == "OSA" || strOwner2FeatureState == "LIP")
                        {
                            messArguments[0] = GetStatusAbbreviation(strFeatureState);
                            messArguments[1] = GetStatusAbbreviation(strOwner2FeatureState);

                            validateMsg.Rule_Id = "OWN02";
                            validateMsg.BuildRuleMessage(gTApplication, messArguments);

                            errorMsg.Add(validateMsg.Rule_MSG);
                            errorPriority.Add(Convert.ToString(m_arguments.GetArgument(0)));
                        }
                    }
                }
                else if (owner2ID > 0)
                {
                    strOwner2FeatureState = GetOwnerStatus(owner2ID);
                    if (strOwner2FeatureState == "PPR" || strOwner2FeatureState == "PPA" || strOwner2FeatureState == "ABR" || strOwner2FeatureState == "ABA" ||
                    strOwner2FeatureState == "OSR" || strOwner2FeatureState == "OSA" || strOwner2FeatureState == "LIP")
                    {
                        messArguments[0] = GetStatusAbbreviation(strFeatureState);
                        messArguments[1] = GetStatusAbbreviation(strOwner2FeatureState);

                        validateMsg.Rule_Id = "OWN02";
                        validateMsg.BuildRuleMessage(gTApplication, messArguments);

                        errorMsg.Add(validateMsg.Rule_MSG);
                        errorPriority.Add(Convert.ToString(m_arguments.GetArgument(0)));
                    }
                }
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Method is used to get the owner status
        /// </summary>
        /// <param name="OwnerID">Owner Id</param>
        /// <returns></returns>
        private string GetOwnerStatus(int OwnerID)
        {
            string sql = "";
            ADODB.Recordset rsTemp = null;
            int count = 0;
            try
            {
                sql = "select FEATURE_STATE_C from COMMON_N where g3e_id={0}";
                rsTemp = DataContext.Execute(string.Format(sql, OwnerID), out count, (int)ADODB.CommandTypeEnum.adCmdText, null);
                if (rsTemp != null && rsTemp.RecordCount > 0)
                {
                    rsTemp.MoveFirst();
                    return (String)rsTemp.Fields["FEATURE_STATE_C"].Value;
                }
            }
            catch
            {
                throw;
            }
            finally
            {
                rsTemp = null;
            }
            return null;
        }

        /// <summary>
        /// Method is used to get the status Abbreviation
        /// </summary>
        /// <param name="strCode"> Status Code </param>
        /// <returns></returns>
        private string GetStatusAbbreviation(string strCode)
        {
            string featureState = "";
            switch (strCode)
            {
                case "INI":
                    featureState = "In Service - Installed";
                    break;
                case "CLS":
                    featureState = "Closed";
                    break;
                case "PPI":
                    featureState = "Proposed Install";
                    break;
                case "PPX":
                    featureState = "Proposed Changeout";
                    break;
                case "ABI":
                    featureState = "As-Built Install";
                    break;
                case "ABX":
                    featureState = "As Built Changeout";
                    break;
                case "PPR":
                    featureState = "Proposed Remove";
                    break;
                case "PPA":
                    featureState = "Proposed Abandon";
                    break;
                case "ABR":
                    featureState = "As-Built Remove";
                    break;
                case "ABA":
                    featureState = "As-Built Abandon";
                    break;
                case "OSR":
                    featureState = "Out of Service - Removed";
                    break;
                case "OSA":
                    featureState = "Out of Service - Abandoned";
                    break;
                case "LIP":
                    featureState = "Left In Place";
                    break;
                default:
                    featureState = null;
                    break;
            }
            return featureState;
        }
    }
}
