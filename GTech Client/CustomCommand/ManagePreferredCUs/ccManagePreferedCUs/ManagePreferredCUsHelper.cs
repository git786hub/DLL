//----------------------------------------------------------------------------+
//        Class: ManagePreferredCUsHelper
//  Description: This class that contains methods to handle form events
//----------------------------------------------------------------------------+
//     $Author:: hkonda                                   $
//       $Date:: 23/12/17                                 $
//   $Revision:: 1                                        $
//----------------------------------------------------------------------------+
//    $History:: ManagePreferredCUsHelper.cs                     $
// 
// *****************  Version 1  *****************
// User: hkonda     Date: 23/12/17   Time: 18:00  Desc : Created
//----------------------------------------------------------------------------+
using System;
using System.Collections.Generic;
using System.Text;
using ADODB;
using Intergraph.GTechnology.API;

namespace GTechnology.Oncor.CustomAPI
{
    public class ManagePreferredCUsHelper
    {
        internal IGTDataContext DataContext;
        internal string SelectedCategory;

        /// <summary>
        /// Method to get all the categories from CULIB_CATEGORY table. This serves as data source for Catergory combo box.
        /// </summary>
        /// <returns> List of category code concatenated with category description.</returns>
        internal List<string> GetAllCategories()
        {
            Recordset categoriesRs = null;
            try
            {
                string categories = "SELECT CATEGORY_C || ' - ' ||  CATEGORY_DESC category FROM CULIB_CATEGORY";
                categoriesRs = GetRecordSet(categories);
                List<string> categoriesList = new List<string>();
                if (categoriesRs != null && categoriesRs.RecordCount > 0)
                {
                    categoriesRs.MoveFirst();
                    while (!categoriesRs.EOF)
                    {
                        categoriesList.Add(Convert.ToString(categoriesRs.Fields["category"].Value));
                        categoriesRs.MoveNext();
                    }
                    if (categoriesList.Count > 0)
                    {
                        SelectedCategory = categoriesList[0].Split('-')[0].Trim();
                    }
                }
                return categoriesList;
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                if (categoriesRs != null)
                {
                    categoriesRs.Close();
                    categoriesRs = null;
                }
            }
        }

        /// <summary>
        /// Method to get all the preferred CUs for the selected cateogry from cuselect_userpref table. This list contains items for Preferreed CUs list box.
        /// </summary>
        /// <returns> List of cu code concatenated with cu description.</returns>
        internal List<string> GetUserPreferredCUsForCategory(string category)
        {
            Recordset userPreferredCUsRs = null;
            try
            {
                StringBuilder sb = new StringBuilder();
                sb.Append("SELECT csu.cu_code|| ' - ' || clu.cu_desc preferredCU FROM CULIB_UNIT clu , cuselect_userpref csu ");
                sb.Append(" WHERE csu.cu_category_code = clu.category_c AND ");
                sb.Append(" csu.cu_category_code = '{0}' AND ");
                sb.Append(" csu.cu_code = clu.cu_id and ");
                sb.Append(" csu.pref_uid = '{1}' ");
                string userPreferredCUs = sb.ToString();
                string filter = string.IsNullOrEmpty(category) ? SelectedCategory : category;
                userPreferredCUsRs = GetRecordSet(string.Format(userPreferredCUs, filter, DataContext.DatabaseUserName));
                List<string> preferredCUsList = new List<string>();
                if (userPreferredCUsRs != null && userPreferredCUsRs.RecordCount > 0)
                {
                    userPreferredCUsRs.MoveFirst();
                    while (!userPreferredCUsRs.EOF)
                    {
                        preferredCUsList.Add(Convert.ToString(userPreferredCUsRs.Fields["preferredCU"].Value));
                        userPreferredCUsRs.MoveNext();
                    }
                }
                return preferredCUsList;
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                if (userPreferredCUsRs != null)
                {
                    userPreferredCUsRs.Close();
                    userPreferredCUsRs = null;
                }

            }
        }

        /// <summary>
        /// Method to get the delta of available CUs and preferred CUs for the selected cateogry from cuselect_userpref table. This list contains items for Available CUs list box.
        /// </summary>
        /// <returns> List of cu code concatenated with cu description.</returns>
        internal List<string> GetDeltaCUsForCategory(string category)
        {
            Recordset allMinusPreferredCUsRs = null;
            try
            {
                StringBuilder sb = new StringBuilder();
                sb.Append("SELECT clu.cu_id|| ' - ' || clu.cu_desc CU FROM CULIB_UNIT clu  WHERE clu.category_c ='{0}'");
                sb.Append(" AND clu.cu_id || ' - ' || clu.cu_desc not in ");
                sb.Append("(SELECT csu.cu_code|| ' - ' || clu.cu_desc FROM CULIB_UNIT clu , cuselect_userpref csu ");
                sb.Append(" WHERE csu.cu_category_code = clu.category_c AND csu.cu_category_code = '{1}' AND csu.cu_code = clu.cu_id and csu.pref_uid = '{2}' )");

                string deltaCUs = sb.ToString();
                string filter = string.IsNullOrEmpty(category) ? SelectedCategory : category;

                allMinusPreferredCUsRs = GetRecordSet(string.Format(deltaCUs, filter, filter, DataContext.DatabaseUserName));
                List<string> deltaCUsList = new List<string>();
                if (allMinusPreferredCUsRs != null && allMinusPreferredCUsRs.RecordCount > 0)
                {
                    allMinusPreferredCUsRs.MoveFirst();
                    while (!allMinusPreferredCUsRs.EOF)
                    {
                        deltaCUsList.Add(Convert.ToString(allMinusPreferredCUsRs.Fields["CU"].Value));
                        allMinusPreferredCUsRs.MoveNext();
                    }
                }
                return deltaCUsList;
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                if (allMinusPreferredCUsRs != null)
                {
                    allMinusPreferredCUsRs.Close();
                    allMinusPreferredCUsRs = null;
                }
            }
        }

        /// <summary>
        /// Method to save the preferred CUs to cuselect_userpref table.
        /// </summary>
        internal void SaveToPreferredCUs(Dictionary<string,string> deltaPreferred)
        {
            try
            {
                StringBuilder insertQuery = new StringBuilder();
                insertQuery.AppendFormat(" BEGIN ");
                foreach (KeyValuePair<string,string> preferredCU in deltaPreferred)
                {
                    string[] splitCU = SplitSelectedItem(preferredCU.Key);
                    insertQuery.AppendFormat("insert into cuselect_userpref(PREF_UID,CU_CODE, CU_CATEGORY_CODE) values( '{0}','{1}','{2}') ; ", DataContext.DatabaseUserName, splitCU[0].Trim(), preferredCU.Value);
                }
                string sql = insertQuery.ToString() + " commit;";
                sql  = sql + " END ;";
                int iRecordsAffected = 0;
                DataContext.Execute(sql, out iRecordsAffected, (int)CommandTypeEnum.adCmdText);
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Method to delete the preferred CUs from cuselect_userpref table.
        /// </summary>
        internal void DeleteFromPreferredCUs(Dictionary<string, string> deltaPreferred)
        {
            try
            {
                StringBuilder insertQuery = new StringBuilder();
                insertQuery.AppendFormat(" BEGIN ");
                foreach (KeyValuePair<string, string> preferredCU in deltaPreferred)
                {
                    string[] splitCU = SplitSelectedItem(preferredCU.Key);
                    insertQuery.AppendFormat("delete from cuselect_userpref  where PREF_UID = '{0}' and CU_CODE = '{1}' and CU_CATEGORY_CODE = '{2}' ; ", DataContext.DatabaseUserName, splitCU[0].Trim(), preferredCU.Value);
                }
                string sql = insertQuery.ToString() + " commit;";
                sql = sql + " END ;";
                int iRecordsAffected = 0;
                DataContext.Execute(sql, out iRecordsAffected, (int)CommandTypeEnum.adCmdText);
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Helper method to split the string 
        /// </summary>
        /// <param name="selectedCU"></param>
        /// <returns>split string</returns>
        private string[] SplitSelectedItem(string selectedCU)
        {
            try
            {
                //return selectedCU.Split('-');

                return  selectedCU.Split(new string[] { " - " },StringSplitOptions.None);
            }
            catch (Exception)
            {
                throw;
            };
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
                Command command = new ADODB.Command();
                command.CommandText = sqlString;
                Recordset results = DataContext.ExecuteCommand(command, out outRecords);
                return results;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
