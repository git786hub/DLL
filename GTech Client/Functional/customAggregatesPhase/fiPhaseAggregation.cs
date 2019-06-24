//----------------------------------------------------------------------------+
//        Class: fiPhaseAggregation
//  Description: This interface updates the feature-level Phase and Phase Quantity attribute
//----------------------------------------------------------------------------+
//     $Author:: uavote                                                       $
//       $Date:: 09/05/2017                                                     $
//   $Revision:: 1                                                            $
//----------------------------------------------------------------------------+
//    $History:: fiPhaseAggregation.cs                                           $
// 
// *****************  Version 1  *****************
// User: uavote     Date: 09/05/2017    Time: 18:00  Comments: Created
// User: pvkurell   Date: 27/06/2017    Time: 18:00  Comments: Added Function GetAggregatePhaseValue to concatenate all phase value. Called this function both in Execute and Delete as well 
// User: hkonda     Date: 23/08/2017    Time: 18:00  Comments:Added two new functions for Alphabetical order of Phases and phase quantity. Removed code from Delete function.
// User: hkonda     Date: 06/09/2017    Time: 18:00  Comments:Adjusted code to get the distinct phase for Connectivity component- Phase attribute
// User: hkonda     Date: 12/09/2017    Time: 18:00  Comments:Adjusted code to ignore '*' in phase value
// User: hkonda     Date: 06/10/2017    Time: 18:00  Comments:Adjusted code for Delete scenario as well
// User: hkonda     Date: 29/03/2018    Time: 18:00  Comments:Code changes done as per JIRA ONCORDEV-1309
// User: pvkurell   Date: 20/09/2018    Time: 18:00  Comments:Code changes done to fix jira 1850
// User: Hari       Date: 25/09/2018    Time: 18:00  Comments:Changes reverted from above commit. Code changes made to update "Phase" and "As operated phase" 
//                                                            even when the activity is null as well as when there are no UNIT CID and UNIT CNO for CU instances.
// User : pramod    Date: 17/10/2018    Time: 18:00  Comments:Excluded Neutral Phase "N" from resulting value of Phase in Connectivity component.  
// User : Hari      Date: 20/01/2019    Time: 18:00  Comments:Excluded Neutral Phase "N" from resulting value of As Operated Phase in Connectivity component. 
//----------------------------------------------------------------------------+

using System;
using Intergraph.GTechnology.Interfaces;
using Intergraph.GTechnology.API;
using ADODB;
using System.Linq;
using System.Windows.Forms;


namespace GTechnology.Oncor.CustomAPI
{
    public class fiPhaseAggregation : IGTFunctional
    {
        #region IGTFunctional Members

        private GTArguments m_Arguments = null;
        private IGTDataContext m_DataContext = null;
        private IGTComponents m_components;
        private string m_ComponentName;
        private string m_FieldName;

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

        public void Delete()
        {
            try
            {
                IGTComponent activeComponent = Components[ComponentName];
                IGTComponent connectivityComponent = Components["CONNECTIVITY_N"];
                //Update Phase_C attribute to remove Deleted Phase 
                Tuple<string, string> phaseValues = GetAggregatePhaseValue(Convert.ToInt32(activeComponent.Recordset.Fields["G3E_CID"].Value));
                if (connectivityComponent!=null && connectivityComponent.Recordset != null && connectivityComponent.Recordset.RecordCount > 0)
                {
                    connectivityComponent.Recordset.MoveFirst();
                    connectivityComponent.Recordset.Fields["PHASE_ALPHA"].Value = GetAlphabetizedPhase(phaseValues.Item1);
                    connectivityComponent.Recordset.Fields["PHASE_OPERATED"].Value = GetAlphabetizedPhase(phaseValues.Item2);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error during Phase Aggregation FI execution. " + ex.Message, "G/Technology");
            }
        }

        public void Execute()
        {
            GTClassFactory.Create<IGTApplication>().SetStatusBarText(GTStatusPanelConstants.gtaspcMessage, "Aggregate Phase FI");
            IGTComponent activeComponent = Components[ComponentName];

            short cNo = 0;
            string field = string.Empty;
            string alphabetizedPhase = string.Empty;
            string alphabetizedOperatedPhase = string.Empty;

            try
            {
                if (activeComponent != null)
                {
                    //Get the active feature current CID
                    int aCID = Convert.ToInt32(activeComponent.Recordset.Fields["G3E_CID"].Value);
                    if ((aCID != 0))
                    {
                        int anoPhase = Convert.ToInt32(m_Arguments.GetArgument(0));
                        int anoPhaseQty = Convert.ToInt32(m_Arguments.GetArgument(1));
                        int anoPhaseOperated = Convert.ToInt32(m_Arguments.GetArgument(2));

                        Tuple<string, string> phaseValues = GetAggregatePhaseValue();
                        Recordset phaseRs = DataContext.MetadataRecordset("G3E_ATTRIBUTEINFO_OPTABLE", "G3E_ANO = " + anoPhase);//    GetRecordSet(sqlString);

                        // Update PHASE
                        if (phaseRs.RecordCount > 0)
                        {
                            phaseRs.MoveFirst();
                            cNo = Convert.ToInt16(phaseRs.Fields["G3E_CNO"].Value);
                            field = Convert.ToString(phaseRs.Fields["G3E_FIELD"].Value);

                            IGTComponent connectivityComponent = Components.GetComponent(cNo);
                            connectivityComponent.Recordset.MoveFirst();

                            alphabetizedPhase = GetAlphabetizedPhase(phaseValues.Item1);
                            connectivityComponent.Recordset.Fields[field].Value = alphabetizedPhase;
                        }

                        Recordset operatedPhaseRs = DataContext.MetadataRecordset("G3E_ATTRIBUTEINFO_OPTABLE", "G3E_ANO = " + anoPhaseOperated);

                        // Update As-Operated Phase
                        if (operatedPhaseRs.RecordCount > 0)
                        {
                            operatedPhaseRs.MoveFirst();
                            cNo = Convert.ToInt16(operatedPhaseRs.Fields["G3E_CNO"].Value);
                            field = Convert.ToString(operatedPhaseRs.Fields["G3E_FIELD"].Value);

                            IGTComponent connectivityComponent = Components.GetComponent(cNo);
                            connectivityComponent.Recordset.MoveFirst();

                            alphabetizedOperatedPhase = GetAlphabetizedPhase(phaseValues.Item2);
                            connectivityComponent.Recordset.Fields[field].Value = alphabetizedOperatedPhase;
                        }

                        // Update PHASE QUANTITY 
                        if (anoPhaseQty != 0)
                        {
                            Recordset phaseQuantityRs = DataContext.MetadataRecordset("G3E_ATTRIBUTEINFO_OPTABLE", "G3E_ANO = " + anoPhaseQty);
                            if (phaseQuantityRs.RecordCount > 0)
                            {
                                phaseQuantityRs.MoveFirst();
                                cNo = Convert.ToInt16(phaseQuantityRs.Fields["G3E_CNO"].Value);
                                field = Convert.ToString(phaseQuantityRs.Fields["G3E_FIELD"].Value);

                                IGTComponent primaryAttributeComponent = Components.GetComponent(cNo);
                                primaryAttributeComponent.Recordset.MoveFirst();
                                primaryAttributeComponent.Recordset.Fields[field].Value = GetDistinctPhaseCount(alphabetizedPhase);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error during Phase Aggregation FI execution. " + ex.Message, "G/Technology");
            }
        }

        public void Validate(out string[] ErrorPriorityArray, out string[] ErrorMessageArray)
        {
            ErrorPriorityArray = null;
            ErrorMessageArray = null;
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

        #region Private Methods


        /// <summary>
        /// Aggregate all Phase Value in unit component based on activity 
        /// </summary>
        /// <returns>Tuple object of phase and operated phase</returns>
        private Tuple<string, string> GetAggregatePhaseValue(int g3eCid = 0)
        {
            string phaseValue = string.Empty;
            string operatedPhaseValue = string.Empty;
            Recordset rs = null;
            Tuple<string, string> phaseValues = null;
            try
            {
                IGTComponent activeComponent = Components[ComponentName];
                if (activeComponent != null)
                {
                    rs = activeComponent.Recordset;
                    if (rs != null && rs.RecordCount > 0)
                    {
                        rs.MoveFirst();
                        rs.Sort = "G3E_CID";
                        while (!rs.EOF)
                        {
                            string activity = GetCorrespondingCUActivityForUnitInstance(activeComponent.CNO, Convert.ToInt32(rs.Fields["G3E_CID"].Value));
                            //Exclude Neutral "N" Phase from resulting Value of Phase and As Operated in the Connectivity component
                            string phase = Convert.ToString(rs.Fields["PHASE_C"].Value);
                            if (g3eCid != Convert.ToInt32(rs.Fields["G3E_CID"].Value) && activity != "R" && activity != "S" && phase!="N" )
                            {
                                phaseValue += phase;
                            }
                            if (g3eCid != Convert.ToInt32(rs.Fields["G3E_CID"].Value) && activity != "I" && phase != "N")
                            {
                                operatedPhaseValue += phase;
                            }
                            rs.MoveNext();
                        }
                    }
                }
                phaseValues = new Tuple<string, string>(phaseValue, operatedPhaseValue);
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                rs = null;
            }
            return phaseValues;
        }

        /// <summary>
        /// Get the CU activity from CU Attributes for the input UNIT CNO and CID
        /// </summary>
        /// <param name="cno">UNIT component number in the CU component instance</param>
        /// <param name="cid">UNIT CID in the CU component instance</param>
        /// <returns>CU activity</returns>
        private string GetCorrespondingCUActivityForUnitInstance(short cno, int cid)
        {
            try
            {
                string sReturn = string.Empty;

                IGTComponent cuComponent = Components.GetComponent(21);
                Recordset tmpRs = cuComponent.Recordset;
                tmpRs.Filter = "UNIT_CNO = " + cno + " AND UNIT_CID = " + cid;
                if (tmpRs.RecordCount > 0)
                {
                    sReturn = Convert.ToString(tmpRs.Fields["ACTIVITY_C"].Value);
                    tmpRs.Filter = FilterGroupEnum.adFilterNone;
                }
                else
                {
                    tmpRs.Filter = FilterGroupEnum.adFilterNone;
                    tmpRs.Filter = "G3E_CID = " + cid;
                    if (tmpRs.RecordCount > 0)
                    {
                        tmpRs.MoveFirst();
                        sReturn = Convert.ToString(tmpRs.Fields["ACTIVITY_C"].Value);
                    }
                    tmpRs.Filter = FilterGroupEnum.adFilterNone;
                }
                
                return sReturn;
            }
            catch (Exception)
            {
                throw;
            }
        }
        /// <summary>
        /// Gets the Alaphabetized phase value for given phase string. ex -  Input - CBAN  Output-  ABCN
        /// </summary>
        /// <param name="phaseValue"></param>
        /// <returns>Alphabetized phase</returns>
        private string GetAlphabetizedPhase(string phaseValue)
        {
            try
            {
                if (!string.IsNullOrEmpty(phaseValue))
                {
                    char[] phaseArray = phaseValue.ToCharArray();
                    Array.Sort(phaseArray);
                    phaseValue = new string(phaseArray.Distinct().ToArray());
                    if (phaseValue.Contains('*'))
                    {
                        phaseValue = phaseValue.Replace("*", string.Empty);
                    }
                }
                return phaseValue;
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
        private int GetDistinctPhaseCount(string phaseValue)
        {
            try
            {
                char previous = '\0';
                short distinctCount = 0;
                if (!string.IsNullOrEmpty(phaseValue))
                {
                    char[] phaseArray = phaseValue.ToCharArray();

                    foreach (char phase in phaseArray)
                    {
                        if (phase == previous || phase == 'N')
                        {
                            continue;
                        }
                        else
                        {
                            previous = phase;
                            distinctCount = (short)(distinctCount + 1);
                        }
                    }
                }
                return distinctCount;
            }
            catch (Exception)
            {

                throw;
            }
        }

        #endregion

    }
}
