//----------------------------------------------------------------------------+
//        Class: fiSetSummedAttributes
//  Description: This interface sums an attribute across unit components and store the result in an attribute of the bank component, whenever a unit component instance is updated.
//----------------------------------------------------------------------------+
//     $Author:: hkonda                                                       $
//       $Date:: 18/08/17                                                     $
//   $Revision:: 1                                                            $
//----------------------------------------------------------------------------+
//    $History:: fiSetSummedAttributes.cs                                       $
// 
// *****************  Version 1  *****************
// User: hkonda     Date: 22/08/17    Time: 02:00
// User: hkonda     Date: 17/10/17    Time: 02:00  Implemented Delete as well
// User: hkonda     Date: 19/02/18    Time: 02:00  Handled DBNull issue for KVA Size attribute.This issue is because of introduction of CU metadata due to which KVA size is not set to 0 by default.
//----------------------------------------------------------------------------+

using System;
using System.Windows.Forms;
using ADODB;
using Intergraph.GTechnology.API;
using Intergraph.GTechnology.Interfaces;

namespace GTechnology.Oncor.CustomAPI
{
    public class fiSetSummedAttributes : IGTFunctional
    {
        private GTArguments m_Arguments = null;
        private IGTDataContext m_DataContext = null;
        private IGTComponents m_components;
        private string m_ComponentName;
        private string m_FieldName;
        private bool m_isDelete = false;

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
                m_isDelete = true;
                ProcessSumAndUpdateRs();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error during Set Summed Attributes execution. " + ex.Message, "G/Technology");
            }
        }

        public void Execute()
        {
            try
            {
                m_isDelete = false;
                ProcessSumAndUpdateRs();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error during Set Summed Attributes execution. " + ex.Message, "G/Technology");
            }
        }

        private void ProcessSumAndUpdateRs()
        {
            short cNo = 0;
            string field = string.Empty;
            int attributeValueSum = 0;
            Recordset bankAttributesRs = null;

            try
            {
                //Calculate attribute value sum
                IGTComponent component = Components[ComponentName];
                ADODB.Recordset kvaResultsRs = component.Recordset;
                if (kvaResultsRs.RecordCount > 0)
                {
                    int deletetedValue = string.IsNullOrEmpty(Convert.ToString(kvaResultsRs.Fields["KVA_Q"].Value)) ? 0 : Convert.ToInt32(kvaResultsRs.Fields["KVA_Q"].Value);
                    kvaResultsRs.MoveFirst();
                    while (!kvaResultsRs.EOF)
                    {
                        attributeValueSum = attributeValueSum + (string.IsNullOrEmpty(Convert.ToString(kvaResultsRs.Fields["KVA_Q"].Value)) ? 0 : Convert.ToInt32(kvaResultsRs.Fields["KVA_Q"].Value));
                        kvaResultsRs.MoveNext();
                    }
                    if (m_isDelete)
                    {
                        attributeValueSum = attributeValueSum - deletetedValue;
                    }
                }

                //Get the attribute to be updated and update the attribute sum to this attribute

                string sqlString = string.Format("select g3e_field FIELD, g3e_cno CNO from G3E_ATTRIBUTEINFO_OPTABLE where g3e_ano = {0} ", Convert.ToInt32(m_Arguments.GetArgument(0)));
                ADODB.Recordset results = GetRecordSet(sqlString);

                if (results.RecordCount > 0)
                {
                    results.MoveFirst();
                    cNo = Convert.ToInt16(results.Fields["CNO"].Value);
                    field = Convert.ToString(results.Fields["FIELD"].Value);
                }

                bankAttributesRs = Components.GetComponent(cNo).Recordset;
                bankAttributesRs.MoveFirst();
                bankAttributesRs.Fields[field].Value = attributeValueSum;
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

        public void Validate(out string[] ErrorPriorityArray, out string[] ErrorMessageArray)
        {
            ErrorMessageArray = null;
            ErrorPriorityArray = null;
        }
    }
}
