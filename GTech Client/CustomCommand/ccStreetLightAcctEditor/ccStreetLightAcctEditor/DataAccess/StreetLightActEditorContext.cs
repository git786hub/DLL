using ADODB;
using GTechnology.Oncor.CustomAPI.Model;
using System.Collections.Generic;
using System.ComponentModel;

namespace GTechnology.Oncor.CustomAPI.DataAccess
{
    /// <summary>
    /// Methods in the class is to manage the Street Light Account Editor
    /// </summary>
    public class StreetLightActEditorContext
    {

        public StreetLightActEditorContext()
        {
        }

        #region ValueLists

        /// <summary>
        /// Get LampType Value List
        /// </summary>
        /// <returns></returns>
        public BindingList<LampType> GetLampTypeValueList()
        {
            BindingList<LampType> vl_lampTypes = new BindingList<LampType>();
            Recordset rs = CommonUtil.Execute("Select vl_key as Key,vl_value as KeyValue  from VL_STLT_LAMP_TYPE");
            if (rs != null && rs.RecordCount > 0)
            {
                vl_lampTypes = CommonUtil.ConvertRSToEntity<LampType>(rs);
            }
            return vl_lampTypes;
        }

        /// <summary>
        /// Get Wattage Value List
        /// </summary>
        /// <returns></returns>
        public BindingList<Wattage> GetWattageValueList()
        {
            BindingList<Wattage> vl_wattage = new BindingList<Wattage>();
            Recordset rs = CommonUtil.Execute("Select vl_key as Key,vl_value as KeyValue  from VL_STLT_WATTAGE");
            if (rs != null && rs.RecordCount > 0)
            {
                vl_wattage = CommonUtil.ConvertRSToEntity<Wattage>(rs);
            }
            return vl_wattage;
        }

        /// <summary>
        /// Get Luminaire Style Value List
        /// </summary>
        /// <returns></returns>
        public BindingList<LuminaireStyle> GetLuminaireStyleValueList()
        {
            BindingList<LuminaireStyle> vl_LuminaireStyle = new BindingList<LuminaireStyle>();
            Recordset rs = CommonUtil.Execute("Select vl_key as Key,vl_value as KeyValue  from VL_STLT_LUM_STYLE");
            if (rs != null && rs.RecordCount > 0)
            {
                vl_LuminaireStyle = CommonUtil.ConvertRSToEntity<LuminaireStyle>(rs);
            }
            return vl_LuminaireStyle;
        }

        #endregion
        #region Public Methods

        /// <summary>
        /// Get all Street Light Accounts
        /// </summary>
        /// <returns></returns>
        public BindingList<StreetLightAccount> GetStreetLightAccounts()
        {
            BindingList<StreetLightAccount> stltAccounts = new BindingList<StreetLightAccount>();
            Recordset rs = CommonUtil.Execute("SELECT A.ESI_LOCATION, DECODE(A.BILLABLE, 'Y', 1, 0) AS BILLABLE, b.Description as Description, A.OWNER_CODE, A.WATTAGE, A.LAMP_TYPE, ls.vl_key AS LUMINARE_STYLE, A.RATE_SCHEDULE" +
                                                     ", A.RATE_CODE, A.PREVIOUS_COUNT, A.CURRENT_COUNT, A.THRESHOLD_PERCENT, A.THRESHOLD_STATE" +
                                                     ", DECODE(A.THRESHOLD_OVERRIDE, 'Y', 1, 0) AS  THRESHOLD_OVERRIDE, A.RUN_DATE, A.MODIFIED_BY" +
                                                     ", A.MODIFIED_DATE, A.CREATION_DATE, A.BOUNDARY_CLASS, A.BOUNDARY_ID" +
                                                     ", DECODE(A.RESTRICTED, 'Y', 1, 0) AS RESTRICTED, A.MISC_STRUCT_FID AS MiscStructFid" +
                                                     "  FROM STLT_ACCOUNT a, STLT_DESC_VL b, VL_STLT_LUM_STYLE ls " +
                                                     "  WHERE a.DESCRIPTION_ID = b.DESCRIPTION_ID and ls.vl_key = a.LUMINARE_STYLE  order by ESI_LOCATION");
            if (rs != null && rs.RecordCount > 0)
            {
                stltAccounts = CommonUtil.ConvertRSToEntity<StreetLightAccount>(rs);
            }
            return stltAccounts;
        }

        /// <summary>
        /// Persist Street Light Account into Datbase
        /// </summary>
        /// <param name="act"></param>
        public void SaveStreetLightAcct(StreetLightAccount act)
        {
            if (act.EntityState == EntityMode.Add) { InsertStreetLightAccount(act); }
            if (act.EntityState == EntityMode.Update) { UpdateStreetLightAccount(act); }
            if (act.EntityState == EntityMode.Delete) { DeleteStreetLightAccount(act); }
        }

        /// <summary>
        /// Get all Street Light Account assignes to Street Light features . 
        /// </summary>
        /// <returns>return list.This list is used for to validate Street Light account before deleting</returns>
        public IList<string> GetAccountsAssignedToSTLT()
        {
            IList<string> lst = new List<string>();
            string sqlStmt = "SELECT DISTINCT ACCOUNT_ID FROM STREETLIGHT_N where ACCOUNT_ID is not null order by ACCOUNT_ID";
            Recordset rs = CommonUtil.Execute(sqlStmt);
            if (rs != null && rs.RecordCount > 0)
            {
                lst = CommonUtil.ConvertRSToList(rs);
            }
            return lst;
        }

        /// <summary>
        /// Get all CC&B Account. 
        /// </summary>
        /// <returns>Return list.this is used for to Validate for new Street Light Acounts</returns>
        public IList<string> GetCisEsiLocations()
        {
            IList<string> lst = new List<string>();
            string sqlStmt = "SELECT DISTINCT ESI_LOCATION FROM CIS_ESI_LOCATIONS order by ESI_LOCATION";
            Recordset rs = CommonUtil.Execute(sqlStmt);
            if (rs != null && rs.RecordCount > 0)
            {
                lst = CommonUtil.ConvertRSToList(rs);
            }
            return lst;
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Insert Street Light Account into Database
        /// </summary>
        /// <param name="streetLightAct"></param>
        private void InsertStreetLightAccount(StreetLightAccount streetLightAct)
        {
            string sqlStmt = "Insert into STLT_ACCOUNT(ESI_LOCATION, BILLABLE, DESCRIPTION_ID, OWNER_CODE, WATTAGE, LAMP_TYPE, LUMINARE_STYLE," +
                " RATE_SCHEDULE, RATE_CODE,THRESHOLD_PERCENT,THRESHOLD_OVERRIDE,BOUNDARY_CLASS, BOUNDARY_ID, RESTRICTED,MISC_STRUCT_FID) " +
                "values('{0}','{1}',(SELECT DESCRIPTION_ID FROM STLT_DESC_VL WHERE DESCRIPTION='{2}'),'{3}','{4}','{5}','{6}','{7}'," +
                " '{8}',{9},'{10}',{11},'{12}','{13}',{14})";
                

            sqlStmt = string.Format(sqlStmt, streetLightAct.ESI_LOCATION, CommonUtil.ConvertBoolToYN(streetLightAct.Billable), streetLightAct.Description, streetLightAct.OWNER_CODE, streetLightAct.Wattage, streetLightAct.LAMP_TYPE, streetLightAct.LUMINARE_STYLE
                , streetLightAct.RATE_SCHEDULE, streetLightAct.RATE_CODE, streetLightAct.THRESHOLD_PERCENT, CommonUtil.ConvertBoolToYN(streetLightAct.THRESHOLD_OVERRIDE)
                , string.IsNullOrEmpty(streetLightAct.BOUNDARY_CLASS)? "NULL": streetLightAct.BOUNDARY_CLASS, streetLightAct.BOUNDARY_ID, CommonUtil.ConvertBoolToYN(streetLightAct.Restricted), streetLightAct.MiscStructFid);
            CommonUtil.Execute(sqlStmt);
        }
        /// <summary>
        /// Update Street light Account
        /// </summary>
        /// <param name="streetLightAct"></param>
        private void UpdateStreetLightAccount(StreetLightAccount streetLightAct)
        {
            string sqlStmt = "Update STLT_ACCOUNT set BILLABLE='{0}',DESCRIPTION_ID=(SELECT DESCRIPTION_ID FROM STLT_DESC_VL WHERE DESCRIPTION='{1}'),OWNER_CODE='{2}',THRESHOLD_PERCENT={3},THRESHOLD_OVERRIDE='{4}',BOUNDARY_CLASS={5}, BOUNDARY_ID='{6}'" +
                ", RESTRICTED='{7}' where  ESI_LOCATION='{8}'";
                
            sqlStmt = string.Format(sqlStmt, CommonUtil.ConvertBoolToYN(streetLightAct.Billable), streetLightAct.Description, streetLightAct.OWNER_CODE, streetLightAct.THRESHOLD_PERCENT,
                 CommonUtil.ConvertBoolToYN(streetLightAct.THRESHOLD_OVERRIDE), string.IsNullOrEmpty(streetLightAct.BOUNDARY_CLASS) ? "NULL" : streetLightAct.BOUNDARY_CLASS, streetLightAct.BOUNDARY_ID, CommonUtil.ConvertBoolToYN(streetLightAct.Restricted), streetLightAct.ESI_LOCATION);
            CommonUtil.Execute(sqlStmt);
        }

        /// <summary>
        /// Delete Street Light Account from Database
        /// </summary>
        /// <param name="streetLightAct"></param>
        private void DeleteStreetLightAccount(StreetLightAccount streetLightAct)
        {
            string sqlStmt = "Delete From STLT_ACCOUNT Where  ESI_LOCATION='{0}'";
            sqlStmt = string.Format(sqlStmt, streetLightAct.ESI_LOCATION);
            CommonUtil.Execute(sqlStmt);
        }
        #endregion


      
    }
}
