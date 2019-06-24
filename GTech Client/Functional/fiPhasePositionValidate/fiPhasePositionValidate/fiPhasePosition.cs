//----------------------------------------------------------------------------+
//        Class: fiPhasePosition
//  Description: This interface sets the Configured Phase attribute to the set of all present phases sorted in the order defined by their positions
//----------------------------------------------------------------------------+
//     $Author:: Pramod                                                      $
//       $Date:: 05/05/2017                                                  $
//   $Revision:: 1                                                           $
//----------------------------------------------------------------------------+
//    $History:: fiPhasePosition.cs                                          $
// 
// *****************  Version 1  *****************
// User: Pramod     Date: 05/05/2017     Time: 18:00	Desc: Implemented  Business Rule as per JIRA 476 
// *****************  Version 1.0.0.1  *****************
// User: kappana     Date: 07/21/2017    Time: 18:00	Desc: Implemented  Business Rule as per JIRA 686
// *****************  Version 1.0.0.2  *****************
// User: hkonda     Date: 08/29/2017    Time: 18:00	    Desc: Added new validations and execute method logic as per latest DDD
// User: hkonda     Date: 09/07/2017    Time: 18:00	    Desc: Updated the code to suit new set of Phase positions
// User: hkonda     Date: 09/08/2017    Time: 18:00	    Desc: added logic to validate phase positions WRT NEUTRAL and STATIC
// User: hkonda     Date: 09/11/2017    Time: 18:00	    Desc: Fixed Value cannot be null issue during testing.
// User: Sithara    Date: 11/01/2018    Time: 18:00     Desc: Update this interface to retrieve rule information from WR_VALIDATION_RULE table
// User: hkonda     Date: 28/02/2019    Time: 18:00     Desc: Fix for ALM -2028-JIRA-2593

//----------------------------------------------------------------------------+

using ADODB;
using Intergraph.GTechnology.API;
using Intergraph.GTechnology.Interfaces;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Windows.Forms;
using gtCommandLogger;

namespace GTechnology.Oncor.CustomAPI
{
    public class fiPhasePosition : IGTFunctional
    {
        public const string Caption = "G/Technology";
        public const string set1 = "W,M,E";
        public const string set2 = "T,M,B";
        public const string set3 = "N,M,S";

        private GTArguments _arguments = null;
        private IGTDataContext _dataContext = null;
        private IGTComponents _components;
        private string _componentName;
        private string _fieldName;
        private List<string> m_PhaseList = null;

        public GTArguments Arguments
        {
            get { return _arguments; }
            set { _arguments = value; }
        }

        public string ComponentName
        {
            get { return _componentName; }
            set { _componentName = value; }
        }

        public IGTComponents Components
        {
            get { return _components; }
            set { _components = value; }
        }

        public IGTDataContext DataContext
        {
            get { return _dataContext; }
            set { _dataContext = value; }
        }


        public void Delete()
        {
        }

        public void Execute()
        {
            //JIRA-686 FI to blank out the Phase Postion when Phase is set to N
            try
            {
                if (_fieldName == "PHASE_C")
                {
                    IGTComponent comp = Components[ComponentName];
                    Recordset compRecords = comp.Recordset;
                    string strPhase;
                    if (compRecords != null && compRecords.RecordCount > 0)
                    {
                        compRecords.MoveFirst();
                        while (!(compRecords.EOF || compRecords.BOF))
                        {
                            strPhase = Convert.ToString(compRecords.Fields[_fieldName].Value);
                            if (strPhase != null && strPhase == "N")
                            {
                                compRecords.Fields["PHASE_POS_C"].Value = null;
                            }
                            compRecords.MoveNext();
                        }
                    }
                }
                // Set Configured Phase

                Recordset primarConductorOhRs = Components.GetComponent(801).Recordset;
                if (primarConductorOhRs != null && primarConductorOhRs.RecordCount > 0)
                {
                    primarConductorOhRs.MoveFirst();
                    if (!ValidatePhasePositionWithSets())
                    {
                        primarConductorOhRs.Fields["PHASE_CONFIG"].Value = PrepareConfiguredPhaseAttribute();
                    }
                    else
                    {
                        primarConductorOhRs.Fields["PHASE_CONFIG"].Value = string.Empty;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error during execution of Phase position FI. " + ex.Message, Caption);
            }
        }

        public string FieldName
        {
            get { return _fieldName; }
            set { _fieldName = value; }
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

        public void Validate(out string[] ErrorPriorityArray, out string[] ErrorMessageArray)
        {
            ADODB.Recordset rs = null;
            ADODB.Recordset rsTemp = null;
            int record = 0;
            bool hasPhaseAndPosition = true;
            bool hasUnknownPhasePosition = false;
            ErrorPriorityArray = null;
            ErrorMessageArray = null;

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
                    ValidationInterfaceName = "Phase Position",
                    ValidationInterfaceType = "FI"
                };
                gTValidationLogger = new GTValidationLogger(logEntries);

                gTValidationLogger.LogEntry("TIMING", "START", "Phase Position Entry", "N/A", "");
            }


            try
            {
                List<string> errorMsg = new List<string>();
                List<string> errorPriority = new List<string>();
                string priority = Convert.ToString(_arguments.GetArgument(0));
                ValidationRuleManager validateMsg = new ValidationRuleManager();
                IGTApplication gTApplication = GTClassFactory.Create<IGTApplication>();

                Recordset commonComponentRs = Components.GetComponent(1).Recordset;
                commonComponentRs.MoveFirst();
                string orientation = Convert.ToString(commonComponentRs.Fields["ORIENTATION_C"].Value);
                int fNo = Convert.ToInt32(commonComponentRs.Fields["G3E_FNO"].Value);
                string featureName = Convert.ToString(GetRecordSet(string.Format("select g3e_username NAME from g3e_features_optable where g3e_fno = {0}", fNo)).Fields["NAME"].Value);
                int fId = Convert.ToInt32(commonComponentRs.Fields["G3E_FID"].Value);
                bool isFeatureConnected = CheckFeatureConnectivity(fId);

                ValidatePhaseAndPhasePosition(out hasPhaseAndPosition, out hasUnknownPhasePosition);
                if (orientation == "OH" && featureName.ToUpper().Contains("PRIMARY") && isFeatureConnected && hasUnknownPhasePosition)
                {
                    validateMsg.Rule_Id = "PHPS01";
                    validateMsg.BuildRuleMessage(gTApplication, null);

                    errorPriority.Add(priority);
                    errorMsg.Add(validateMsg.Rule_MSG);

                    //errorMsg.Add("Overhead feature is missing phase position values.");
                }

                if (ValidatePhasePositionWithSets())
                {
                    validateMsg.Rule_Id = "PHPS02";
                    validateMsg.BuildRuleMessage(gTApplication, null);

                    errorPriority.Add(priority);
                    errorMsg.Add(validateMsg.Rule_MSG);

                    //errorMsg.Add("Phase Position attributes are not consistent with respect to one another.");
                }

                if (_fieldName == "PHASE_POS_C")
                {
                    string sqlStmt = "Select {0},count({0}) from {1} where g3e_fid={2} and g3e_fno={3} GROUP BY {0} having count({0})>1";
                    IGTComponent gtComponent = _components[_componentName];
                    if (gtComponent != null)
                    {
                        rs = gtComponent.Recordset;
                        if (rs != null && rs.RecordCount > 0)
                        {
                            rs.MoveFirst();
                            if (Convert.ToInt32(rs.Fields["G3e_CID"].Value) == 1)
                            {
                                //JIRA 195- check Duplicate Phase Position 
                                rsTemp = _dataContext.Execute(string.Format(sqlStmt, _fieldName, _componentName, rs.Fields["G3e_FID"].Value, rs.Fields["G3e_FNO"].Value), out record, (int)ADODB.CommandTypeEnum.adCmdText, null);
                                if (rsTemp != null && rsTemp.RecordCount > 0)
                                {
                                    validateMsg.Rule_Id = "PHPS02";
                                    validateMsg.BuildRuleMessage(gTApplication, null);

                                    errorMsg.Add(validateMsg.Rule_MSG);
                                    //errorMsg.Add("Feature has duplicate Phase Positions. Phase Position should be unique across all the wires within same conductor.");
                                    errorPriority.Add(priority);
                                }
                            }
                        }
                    }
                    if (errorMsg.Count > 0)
                    {
                        ErrorPriorityArray = errorPriority.ToArray();
                        ErrorMessageArray = errorMsg.ToArray();
                    }
                }

                if(gTValidationLogger != null)
                    gTValidationLogger.LogEntry("TIMING", "END", "Phase Position Exit", "N/A", "");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error during Phase position validation FI. " + ex.Message, Caption);
            }
            finally
            {
                rsTemp = null;
            }

        }

        /// <summary>
        /// Method to validate Phases and phase positions i.e., for a Phase there is no Phase position or vice versa.
        /// checks for unknown legacy values in phase position
        /// checkf if neutral has valid phase positions (NT / ST) and A,B, C have valid phase positions (as defined in configured position sets)
        /// </summary>
        /// <param name="hasPhaseAndPosition">returns true if each phase has valid phase position else false</param>
        /// <param name="hasUnknownPhasePosition">returns true if phase position has 'unknown' legacy value</param>
        private void ValidatePhaseAndPhasePosition(out bool hasPhaseAndPosition, out bool hasUnknownPhasePosition)
        {
            hasPhaseAndPosition = true;
            hasUnknownPhasePosition = false;
            bool isPhaseValid = false;
            bool isPhasePositionvalid = false;
            string position = string.Empty;

            try
            {
                List<string> phaseList = new List<string>();
                List<string> phasePositionList = new List<string>();
                Recordset PhaseAndPositionRs = Components[ComponentName].Recordset;
                if (PhaseAndPositionRs != null && PhaseAndPositionRs.RecordCount > 0)
                {
                    PhaseAndPositionRs.MoveFirst();
                    while (!PhaseAndPositionRs.EOF)
                    {
                        phaseList.Add(Convert.ToString(PhaseAndPositionRs.Fields["PHASE_C"].Value));
                        phasePositionList.Add(Convert.ToString(PhaseAndPositionRs.Fields["PHASE_POS_C"].Value));
                        PhaseAndPositionRs.MoveNext();
                    }
                }

                hasUnknownPhasePosition = phasePositionList.Contains("unknown");

                for (int i = 0; i < phasePositionList.Count; i++)
                {
                    if (string.IsNullOrEmpty(phasePositionList[i]) || string.IsNullOrEmpty(phaseList[i]))// Phase / Phase position for each wire is empty
                    {
                        hasPhaseAndPosition = false;
                        break;
                    }
                }

                for (int i = 0; i < phaseList.Count(); i++)
                {
                    if (phaseList[i] == "N")
                    {
                        position = phasePositionList[i];
                        if (position == "NT" || position == "ST")
                        {
                            hasPhaseAndPosition = true;
                        }
                        else
                        {
                            hasPhaseAndPosition = false;
                            break;
                        }
                    }
                    else
                    {
                        position = phasePositionList[i];
                        if (position == "NT" || position == "ST")
                        {
                            hasPhaseAndPosition = false;
                            break;
                        }
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// This methods validates whether phase postions are from same sets or not
        /// checks if phase positions are repeating
        /// checks if the conductor has both neutral and static phase positions
        /// </summary>
        /// <returns>returns true of validation is failed</returns>

        private bool ValidatePhasePositionWithSets()
        {
            string phasePosition = string.Empty;
            bool set1Complete = false;
            bool set2Complete = false;
            bool set3Complete = false;
            bool repeated = false;
            int nonPhaseCount = 0;
            string positionAggregate = string.Empty;

            try
            {
                List<string> setsList = new List<string> { set1, set2, set3 };
                Recordset phasePositionRs = Components[ComponentName].Recordset;
                if (phasePositionRs != null && phasePositionRs.RecordCount > 0)
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

                    phasePositionRs.MoveFirst();
                    while (!phasePositionRs.EOF)
                    {

                        if (replacedCidList.Contains(Convert.ToInt32(phasePositionRs.Fields["G3E_CID"].Value)))
                        {
                            phasePositionRs.MoveNext();
                            continue;
                        }

                        phasePosition = Convert.ToString(phasePositionRs.Fields["PHASE_POS_C"].Value);

                        if (phasePosition == "NT" || phasePosition == "ST")
                        {
                            nonPhaseCount = nonPhaseCount + 1;
                            phasePositionRs.MoveNext();
                            continue;
                        }

                        positionAggregate = positionAggregate + phasePosition;
                        if (phasePosition == "M") // We do not want to check for "M" (Middle) because it is present all the phase postions sets
                        {
                            phasePositionRs.MoveNext();
                            continue;
                        }

                        if (set1.Contains(phasePosition))
                        {
                            set1Complete = true;
                        }
                        if (set2.Contains(phasePosition))
                        {
                            set2Complete = true;
                        }
                        if (set3.Contains(phasePosition))
                        {
                            set3Complete = true;
                        }
                        phasePositionRs.MoveNext();
                    }
                    if (!isPhasePositionDistinct(positionAggregate))
                    {
                        repeated = true;
                    }

                    if ((set1Complete && set2Complete) || (set2Complete && set3Complete) || (set3Complete && set1Complete) || repeated || nonPhaseCount > 1)
                    {
                        return true;
                    }
                }
                return false;
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Method to prepare the 'Configured Phase' attribute of Primary Conductor - OH ( & Network) Attributes
        /// </summary>
        /// <returns>Configured phase value</returns>
        private string PrepareConfiguredPhaseAttribute()
        {
            List<KeyValuePair<string, string>> PositionsAndPhasePair = null;
            string configuredPhase = string.Empty;
            bool set1Complete = false;
            bool set2Complete = false;
            bool set3Complete = false;

            try
            {
                Recordset ohAttributesRs = Components[ComponentName].Recordset;
                if (ohAttributesRs != null && ohAttributesRs.RecordCount > 0)
                {
                    PositionsAndPhasePair = new List<KeyValuePair<string, string>>();
                    string phasePosition = string.Empty;
                    string phase = string.Empty;
                    ohAttributesRs.MoveFirst();
                    while (!ohAttributesRs.EOF)
                    {
                        phase = Convert.ToString(ohAttributesRs.Fields["PHASE_C"].Value);
                        phasePosition = Convert.ToString(ohAttributesRs.Fields["PHASE_POS_C"].Value);
                        if (string.IsNullOrEmpty(phase) || string.IsNullOrEmpty(phasePosition))
                        {
                            return string.Empty;
                        }
                        PositionsAndPhasePair.Add(new KeyValuePair<string, string>(phasePosition, phase));  // Key - PhasePosition , Value - Phase
                        ohAttributesRs.MoveNext();
                    }
                    foreach (KeyValuePair<string, string> KeyValue in PositionsAndPhasePair)
                    {
                        if (set1.Contains(KeyValue.Key))
                        {
                            if (set1Complete)
                            {
                                continue;
                            }
                            string[] set1Split = set1.Split(',');

                            foreach (string position in set1Split)
                            {
                                if (configuredPhase.Length == 3 && (PositionsAndPhasePair.FirstOrDefault(x => x.Key == "M")).Value != null && configuredPhase.Contains((PositionsAndPhasePair.FirstOrDefault(x => x.Key == "M")).Value))
                                {
                                    break;
                                }
                                var tempPhase = (PositionsAndPhasePair.FirstOrDefault(x => x.Key == position)).Value;
                                if (tempPhase != null)
                                {
                                    configuredPhase = configuredPhase + tempPhase;
                                }
                            }
                            if (configuredPhase.Length == 1 && (PositionsAndPhasePair.FirstOrDefault(x => x.Key == "M")).Value != null && configuredPhase.Contains((PositionsAndPhasePair.FirstOrDefault(x => x.Key == "M")).Value))
                            {
                                configuredPhase = string.Empty;
                            }
                            set1Complete = true;
                        }
                        else if (set2.Contains(KeyValue.Key))
                        {
                            if (set2Complete)
                            {
                                continue;
                            }
                            string[] set2Split = set2.Split(',');
                            foreach (string position in set2Split)
                            {
                                if (configuredPhase.Length == 3 && (PositionsAndPhasePair.FirstOrDefault(x => x.Key == "M")).Value != null && configuredPhase.Contains((PositionsAndPhasePair.FirstOrDefault(x => x.Key == "M")).Value))
                                {
                                    break;
                                }
                                var tempPhase = (PositionsAndPhasePair.FirstOrDefault(x => x.Key == position)).Value;
                                if (tempPhase != null)
                                {
                                    configuredPhase = configuredPhase + tempPhase;
                                }
                            }
                            if (configuredPhase.Length == 1 && (PositionsAndPhasePair.FirstOrDefault(x => x.Key == "M")).Value != null && configuredPhase.Contains((PositionsAndPhasePair.FirstOrDefault(x => x.Key == "M")).Value))
                            {
                                configuredPhase = string.Empty;
                            }
                            set2Complete = true;
                        }

                        else if (set3.Contains(KeyValue.Key))
                        {
                            if (set3Complete)
                            {
                                continue;
                            }
                            string[] set3Split = set3.Split(',');
                            foreach (string position in set3Split)
                            {
                                if (configuredPhase.Length == 3 && (PositionsAndPhasePair.FirstOrDefault(x => x.Key == "M")).Value != null && configuredPhase.Contains((PositionsAndPhasePair.FirstOrDefault(x => x.Key == "M")).Value))
                                {
                                    break;
                                }
                                var tempPhase = (PositionsAndPhasePair.FirstOrDefault(x => x.Key == position)).Value;
                                if (tempPhase != null)
                                {
                                    configuredPhase = configuredPhase + tempPhase;
                                }
                            }
                            if (configuredPhase.Length == 1 && (PositionsAndPhasePair.FirstOrDefault(x => x.Key == "M")).Value != null && configuredPhase.Contains((PositionsAndPhasePair.FirstOrDefault(x => x.Key == "M")).Value))
                            {
                                configuredPhase = string.Empty;
                            }
                            set3Complete = true;
                        }
                    }
                }

                return configuredPhase;
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// checks if the feature is connected to any other feature
        /// </summary>
        /// <param name="fid"></param>
        /// <returns></returns>
        private bool CheckFeatureConnectivity(int fid)
        {
            int node1 = 0;
            int node2 = 0;
            try
            {
                Recordset tempRs = GetRecordSet(string.Format("select NODE_1_ID NODE1 , NODE_2_ID NODE2 from connectivity_n where g3e_fid = {0}", fid));
                if (tempRs != null && tempRs.RecordCount > 0)
                {
                    tempRs.MoveFirst();
                    while (!tempRs.EOF)
                    {
                        node1 = Convert.ToInt32(tempRs.Fields["NODE1"].Value);
                        node2 = Convert.ToInt32(tempRs.Fields["NODE2"].Value);
                        tempRs.MoveNext();
                    }
                }
                if (node1 > 0 && node2 > 0)
                {
                    return true;
                }
                return false;
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
        private Recordset GetRecordSet(string sqlString)
        {
            try
            {
                int outRecords = 0;
                ADODB.Command command = new ADODB.Command();
                command.CommandText = sqlString;
                ADODB.Recordset results = DataContext.ExecuteCommand(command, out outRecords);
                return results;
            }
            catch (Exception)
            {
                throw;
            }
        }


        /// <summary>
        /// Gets the distinct count of phase position in the available string
        /// </summary>
        /// <param name="phaseValue"></param>
        /// <returns></returns>
        private bool isPhasePositionDistinct(string phaseValue)
        {
            char previous = '\0';
            short distinctCount = 0;
            try
            {
                if (!string.IsNullOrEmpty(phaseValue))
                {
                    char[] phaseArray = phaseValue.ToCharArray();
                    Array.Sort(phaseArray);
                    phaseValue = new string(phaseArray);

                    foreach (char position in phaseArray)
                    {
                        if (position == previous)
                        {
                            return false;
                        }
                        previous = position;
                    }
                }
                return true;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
