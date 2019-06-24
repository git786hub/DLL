//----------------------------------------------------------------------------+
//        Class: fiJobStatus
//  Description: This interface sets the Work Request Number .
//----------------------------------------------------------------------------+
//     $Author:: skamaraj                                                       $
//       $Date:: 06/02/19                                                     $
//   $Revision:: 1                                                            $
//----------------------------------------------------------------------------+
//    $History:: fiSetWorkRequestNumber.cs                                           $
// 
// *****************  Version 1  *****************
// User: skamaraj     Date: 06/02/19     Time: 18:00  Desc : Created

//----------------------------------------------------------------------------+

using System;
using System.Windows.Forms;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ADODB;
using Intergraph.GTechnology.API;
using Intergraph.GTechnology.Interfaces;

namespace GTechnology.Oncor.CustomAPI
{
    public class fiSetWorkRequestNumber : IGTFunctional
    {
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
        }

        public void Execute()
        {
            try
            {
                int wrNbr = 0;
                Recordset jobInfoRs = ExecuteCommand(string.Format("select WR_NBR from g3e_job where g3e_identifier  = '{0}'", DataContext.ActiveJob));
                if (jobInfoRs.RecordCount > 0)
                {
                    jobInfoRs.MoveFirst();

                    if (jobInfoRs.Fields["WR_NBR"].Value.GetType() != typeof(DBNull))
                    {
                        wrNbr = Convert.ToInt32(jobInfoRs.Fields["WR_NBR"].Value);
                    }


                    IGTComponent oActiveComponent = Components[ComponentName];
                    if (oActiveComponent.Recordset != null)
                    {
                        if (oActiveComponent.Recordset.RecordCount > 0)
                        {
                            oActiveComponent.Recordset.MoveFirst();
                            while (oActiveComponent.Recordset.EOF == false)
                            {
                                oActiveComponent.Recordset.Fields[FieldName].Value = wrNbr > 0 ? Convert.ToString(wrNbr) : Convert.ToString(DataContext.ActiveJob);                              
                                oActiveComponent.Recordset.MoveNext();
                            }
                        }
                    }

                }

            }
            catch (Exception ex)
            {
                MessageBox.Show("Error during fiSetWorkRequestNumber execution." + ex.Message, "G/Technology");
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


        /// <summary>
        /// Method to execute sql query and return the result record set
        /// </summary>
        /// <param name="sqlString"></param>
        /// <returns></returns>
        private Recordset ExecuteCommand(string sqlString)
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
    }
}


