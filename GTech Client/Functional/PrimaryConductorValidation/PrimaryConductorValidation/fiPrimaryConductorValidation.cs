//----------------------------------------------------------------------------+
//        Class: fiPrimaryConductorValidation
//  Description: 
//		This interface validates placement conditions for Primary Conductor features:
//			Ensures that the length of the affected conductor does not exceed the 
//			maximum length based on its type
//----------------------------------------------------------------------------+
//     $Author:: kappana                                                      $
//       $Date:: 30/06/17                                                     $
//   $Revision:: 1                                                            $
//----------------------------------------------------------------------------+
//    $History:: fiPrimaryConductorValidation.cs                                           $
// 
// *****************  Version 1  *****************
// User: kappana     Date: 30/06/17    Time: 18:00
// User: hkonda     Date:  30/08/17    Time: 18:00  Desc: Modified code to read maxUGLength from interface argument and validating if conductor has distinct phases
// User: hkonda     Date:  13/09/17    Time: 18:00  Desc: Modified code to trim size and type of wire as it was including empty spaces as well
// User: hkonda     Date:  17/10/17    Time: 18:00  Desc: Removed orientation check for UG conductors.
// User: hkonda     Date:  19/10/17    Time: 18:00  Desc: Included recommended length instead of actual length in the error message for OH conductors. Also changed wordings as mentioned in JIRA 916.
// User: hkonda     Date:  08/11/17    Time: 18:00  Desc: Updated validation condition (strSize == "2" && strType == "ACSR" && span_Length > 400) 
// User: pnlella    Date:  15/12/17    Time: 10:30  Desc: Updated GetAggregatePhaseValue method to remove Recordset Clone which is removing filter for the current record and added bookmark code. 
// User: pnlella    Date:  06/02/18    Time: 10:30  Desc: Updated code as per the requirement given in ONCORDEV-1292
// User: hkonda     Date:  29/01/19    Time: 10:30  Desc: Fix for ALM-1793-ONCORDEV-2443
// User: skamaraj   Date:  01/02/19    Time: 10:30  Desc: Fix for ALM-1678-ONCORDEV-2457
// User: hkonda     Date:  28/02/19    Time: 15:00  Desc: Fix for ALM-1793-ONCORDEV-2443 - Refactored GetAggregatePhaseValue method
//----------------------------------------------------------------------------+

using System;
using Intergraph.GTechnology.API;
using Intergraph.GTechnology.Interfaces;
using System.Collections.Generic;
using ADODB;
using gtCommandLogger;

namespace GTechnology.Oncor.CustomAPI
{
    public class fiPrimaryConductorValidation : IGTFunctional
    {
        #region Private Variables
        private GTArguments m_arguments;
        private string m_componentName;
        private IGTComponents m_components;
        private IGTDataContext m_dataContext;
        private string m_fieldName;
        private IGTFieldValue m_fieldValueBeforeChange;
        private GTFunctionalTypeConstants m_type;
        IGTComponent m_compWire = null;
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
        /// Primary Conductor OH and UG validation.
        /// </summary>
        /// <param name="ErrorPriorityArray">Error messages that need to be displayed after validation</param>
        /// <param name="ErrorMessageArray">Error message Priority that need to be displayed after validation</param>
        public void Validate(out string[] ErrorPriorityArray, out string[] ErrorMessageArray)
        {
            List<String> strErrorMsg = new List<String>();
            List<String> strErrorPriority = new List<String>();
            ErrorMessageArray = null;
            ErrorPriorityArray = null;
            ValidationRuleManager validateMsg = new ValidationRuleManager();
            object[] messArguments = new object[2];
            IGTApplication gtApp = (IGTApplication)GTClassFactory.Create<IGTApplication>();

            IGTComponent comp = Components[ComponentName];
            IGTComponent compConn = Components["CONNECTIVITY_N"];
            IGTComponent compComm = Components["COMMON_N"];

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
                    ValidationInterfaceName = "Primary Conductor Validation",
                    ValidationInterfaceType = "FI",
                };
                gTValidationLogger = new GTValidationLogger(logEntries);

                gTValidationLogger.LogEntry("TIMING", "START", "Primary Conductor Validation Entry", "N/A", "");
            }


            gtApp.SetStatusBarText(GTStatusPanelConstants.gtaspcMessage, "Primary Conductor Validation Started");

            try
            {
                string errorPriorityPhasing = Convert.ToString(Arguments.GetArgument(0));
                string errorPriorityLength = Convert.ToString(Arguments.GetArgument(1));
                int maxUGLength = Convert.ToInt32(Arguments.GetArgument(2));
                if (comp != null)
                {
                    comp.Recordset.MoveFirst();
                    compConn.Recordset.MoveFirst();
                    compComm.Recordset.MoveFirst();

                    short fno = Convert.ToInt16(comp.Recordset.Fields["G3E_FNO"].Value);
                    Int32 fid = Convert.ToInt32(comp.Recordset.Fields["G3E_FID"].Value);

                    if (fno == 8 || fno == 84)
                    {
                        m_compWire = Components["PRI_WIRE_OH_N"];
                    }
                    else if (fno == 9 || fno == 85)
                    {
                        m_compWire = Components["PRI_WIRE_UG_N"];
                    }

                    int span_Length = compConn.Recordset.Fields["LENGTH_ACTUAL_Q"].Value == DBNull.Value ? 0 : Convert.ToInt32(compConn.Recordset.Fields["LENGTH_ACTUAL_Q"].Value);
                    string orientation = Convert.ToString(compComm.Recordset.Fields["ORIENTATION_C"].Value);
                    //strErrorPriority[0] = Convert.ToString(Arguments.GetArgument(0));

                   // IGTKeyObject igtActiveFeature = gtApp.DataContext.OpenFeature(fno, Convert.ToInt32(comp.Recordset.Fields["G3E_FID"].Value));

                    if (fno == 8 || fno == 84)
                    {
                        //primary conductor OH
                        if (m_compWire.Recordset != null && m_compWire.Recordset.RecordCount > 0)
                        {
                            m_compWire.Recordset.MoveFirst();
                            while (!m_compWire.Recordset.EOF && !m_compWire.Recordset.BOF)
                            {
                                string strSize = Convert.ToString(m_compWire.Recordset.Fields["SIZE_C"].Value);
                                if (!string.IsNullOrEmpty(strSize))
                                {
                                    strSize = strSize.Trim();
                                }
                                string strType = Convert.ToString(m_compWire.Recordset.Fields["TYPE_C"].Value);
                                if (!string.IsNullOrEmpty(strType))
                                {
                                    strType = strType.Trim();
                                }

                                if (!string.IsNullOrEmpty(Convert.ToString(m_compWire.Recordset.Fields["MAX_LENGTH_Q"].Value)))
                                {
                                    int maxLength = Convert.ToInt32(m_compWire.Recordset.Fields["MAX_LENGTH_Q"].Value);

                                    if (span_Length > maxLength)
                                    {
                                        validateMsg.Rule_Id = "PCND02";

                                        messArguments[0] = maxLength + " feet";
                                        messArguments[1] = strType;

                                        validateMsg.BuildRuleMessage(GTClassFactory.Create<IGTApplication>(), messArguments);
                                        strErrorMsg.Add(validateMsg.Rule_MSG);
                                        strErrorPriority.Add(errorPriorityLength);
                                    }
                                }
                                //string strMsg = "OH span length exceeds recommended length " + span_Length + " for " + strType + " type.";
                             
                                m_compWire.Recordset.MoveNext();
                            }

                        }
                    }
                    else if (fno == 9 || fno == 85)
                    {
                        if (span_Length > maxUGLength)
                        {
                            validateMsg.Rule_Id = "PCND01";
                            validateMsg.BuildRuleMessage(GTClassFactory.Create<IGTApplication>(), null);

                            strErrorMsg.Add(validateMsg.Rule_MSG);
                            strErrorPriority.Add(errorPriorityLength);
                        }
                    }

                    if (!isPhaseDistinct(GetAggregatePhaseValue(m_compWire)))
                    {
                        validateMsg.Rule_Id = "PCND03";
                        validateMsg.BuildRuleMessage(GTClassFactory.Create<IGTApplication>(), null);

                        strErrorMsg.Add(validateMsg.Rule_MSG);
                        strErrorPriority.Add(errorPriorityPhasing);
                    }

                    gtApp.SetStatusBarText(GTStatusPanelConstants.gtaspcMessage, "Primary Conductor Validation Completed");

                    ErrorMessageArray = strErrorMsg.ToArray();
                    ErrorPriorityArray = strErrorPriority.ToArray();
                }

                if(gTValidationLogger != null)
                    gTValidationLogger.LogEntry("TIMING", "END", "Primary Conductor Validation Exit", "N/A", "");

            }
            catch (Exception ex)
            {
                throw new Exception("Error during Primary Conductor Validation" + ex.Message);
            }
        }
        #endregion
        /// <summary>
        /// Aggregate all Phase Value in unit component & return as string 
        /// </summary>
        /// <returns></returns>
        private string GetAggregatePhaseValue(IGTComponent activeComponent, int g3eCid = 0)
        {
            string phaseValue = string.Empty;
            Recordset rs = null;
            try
            {
                if (activeComponent != null)
                {
                    activeComponent.Recordset.MoveFirst();
                    rs = activeComponent.Recordset;
                    if (rs != null && rs.RecordCount > 0)
                    {
                        List<int> replacedCidList = new List<int>();
                        Recordset cuComponentRS = Components.GetComponent(21).Recordset;
                        cuComponentRS.MoveFirst();

                        while (!cuComponentRS.EOF)
                        {
                            if (!Convert.IsDBNull(cuComponentRS.Fields["REPLACED_CID"].Value))
                            {
                                replacedCidList.Add(Convert.ToInt32(cuComponentRS.Fields["REPLACED_CID"].Value));
                            }
                            cuComponentRS.MoveNext();
                        }

                        rs.MoveFirst();

                        while (!rs.EOF)
                        {
                            if (replacedCidList.Contains(Convert.ToInt32(rs.Fields["G3E_CID"].Value)))
                            {
                                rs.MoveNext();
                                continue;
                            }
                            phaseValue = phaseValue + Convert.ToString(rs.Fields["PHASE_C"].Value);
                            rs.MoveNext();
                        }
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
            return phaseValue;
        }

        /// <summary>
        ///  Method to check whether the change out operation is carrried out on the input wire instance.
        /// if replaced cid exists for the input cid, that means change out is performed.
        /// </summary>
        /// <param name="cid"> input cid</param>
        /// <param name="cuComponentRS"> CU component recordset</param>
        /// <param name="changedOutCid"> cid of the changed-out record</param>
        /// <returns>true, if the wire is changed-out i.e., replaced cid exists, else false</returns>
        private bool CheckIfWireIsChangedout(int cid, Recordset cuComponentRS, out int changedOutCid)
        {
            bool returnValue = false;
            try
            {
                changedOutCid = -1;

                if (cuComponentRS != null && cuComponentRS.RecordCount > 0)
                {
                    cuComponentRS.MoveFirst();
                    cuComponentRS.Filter = " REPLACED_CID = " + cid;

                    if (cuComponentRS.RecordCount > 0 && !cuComponentRS.EOF)
                    {
                        changedOutCid = Convert.ToInt32(cuComponentRS.Fields["G3E_CID"].Value);
                        returnValue = true;
                    }
                }
                cuComponentRS.Filter = FilterGroupEnum.adFilterNone;
                return returnValue;
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Method to get phase for input cid
        /// </summary>
        /// <param name="cid">Cid</param>
        /// <returns>Phase if exists, else returns empty string</returns>
        private string GetPhaseAtCid(int cid)
        {
            string phase = string.Empty;
            try
            {
                Recordset wireRecordSet = m_compWire.Recordset;
                wireRecordSet.Filter = "G3E_CID = " + cid;
                if (wireRecordSet != null && !wireRecordSet.EOF)
                {
                    phase = Convert.ToString(wireRecordSet.Fields["PHASE_C"].Value);
                }
                wireRecordSet.Filter = FilterGroupEnum.adFilterNone;
                return phase;
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Gets the distinct count of phases in the available string
        /// </summary>
        /// <param name="phaseValue"></param>
        /// <returns></returns>
        private bool isPhaseDistinct(string phaseValue)
        {
            char previous = '\0';
            short distinctCount = 0;
            char[] phaseArray = phaseValue.ToCharArray();
            Array.Sort(phaseArray);
            phaseValue = new string(phaseArray);
            if (!string.IsNullOrEmpty(phaseValue))
            {
                foreach (char phase in phaseArray)
                {
                    if (phase == 'N')
                    {
                        continue;
                    }
                    else if (phase == previous)
                    {
                        return false;
                    }
                    previous = phase;
                }
            }
            return true;
        }
    }
}
