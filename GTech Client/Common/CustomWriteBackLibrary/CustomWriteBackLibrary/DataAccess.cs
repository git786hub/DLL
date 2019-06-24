//----------------------------------------------------------------------------+
//        Class: DataAccess
//  Description: This class contains methods to access data using a data access layer.
//                                                                  
//----------------------------------------------------------------------------+
//          $Author:: Shubham Agarwal                                       $
//          $Date:: 25/03/18                                                $
//          $Revision:: 1                                                   $
//----------------------------------------------------------------------------+
//    $History:: DataAccess.cs                                              $
// 
// *****************  Version 1  *****************
// User: sagarwal     Date: 25/03/18   Time: 18:00  Desc : Created
//----------------------------------------------------------------------------+

using ADODB;
using Intergraph.GTechnology.API;
using System;

namespace CustomWriteBackLibrary
{
   public class DataAccess
    {
        private IGTApplication m_oApp;
        public DataAccess(IGTApplication p_oApp)
        {
            m_oApp = p_oApp;
        }

        /// <summary>
        /// Method to get first field of the first recordset in string format
        /// </summary>
        /// <param name="p_sql"> SQL string to process </param>
        /// <returns></returns>
        public string GetFirstFieldValueFromRecordset(string p_sql)
        {
            ADODB.Recordset rs =  GetRecordSet(p_sql);
            string sReturnFirstField = string.Empty;

            if (rs !=null)
            {
                if (rs.RecordCount>0)
                {
                    rs.MoveFirst();
                    sReturnFirstField = Convert.ToString(rs.Fields[0].Value);
                }
            }
            return sReturnFirstField;
        }

        /// <summary>
        /// Method to get recordset
        /// </summary>
        /// <param name="p_sqlString"> SQL string</param>
        /// <returns> Recordset object after executing SQL </returns>
        public Recordset GetRecordSet(string p_sqlString)
        {
            try
            {
                int outRecords = 0;
                Command command = new Command();
                command.CommandText = p_sqlString;
                Recordset results = m_oApp.DataContext.ExecuteCommand(command, out outRecords);
                if (results!=null)
                {
                    if (results.RecordCount>0)
                    {
                        results.MoveFirst();
                    }
                }
                return results;
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Method to get URL
        /// </summary>
        /// <param name="p_paramName">SYS_GENERALPARAMETER.SUBSYSTEM_COMPONENT</param>
        /// <param name="p_subSystemName">SYS_GENERALPARAMETER.SUBSYSTEM_NAME</param>
        /// <returns></returns>
        public string GetEFUrl(string p_paramName, string p_subSystemName)
        {
            Recordset resultRs = null;
            try
            {
                resultRs = GetRecordSet(string.Format("SELECT PARAM_VALUE FROM SYS_GENERALPARAMETER WHERE upper(SUBSYSTEM_COMPONENT) = upper('{0}') AND upper(SUBSYSTEM_NAME) = upper('{1}')", p_paramName, p_subSystemName));

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
            finally
            {
                if (resultRs != null)
                {
                    resultRs.Close();
                    resultRs = null;
                }
            }
        }
    }
}
