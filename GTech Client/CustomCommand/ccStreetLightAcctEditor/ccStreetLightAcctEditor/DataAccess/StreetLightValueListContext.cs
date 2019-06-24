using Intergraph.GTechnology.API;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ADODB;
using GTechnology.Oncor.CustomAPI.Model;

namespace GTechnology.Oncor.CustomAPI.DataAccess
{
    /// <summary>
    /// Methods in the class to manage Street Light Value Lists
    /// </summary>
    public class StreetLightValueListContext
    {
        public StreetLightValueListContext()
        {
        }

        #region Description Value List

        #region Public Methods

        /// <summary>
        /// Get all Street Light Description Value List
        /// </summary>
        /// <returns></returns>
        public BindingList<StreetLightDescription> GetSteetLightDescription()
        {

            BindingList<StreetLightDescription> stltDescriptions = new BindingList<StreetLightDescription>();
            Recordset rs = CommonUtil.Execute("Select DESCRIPTION_ID as DescriptionID,DESCRIPTION as Description,MSLA_DATE as MSLA_Date  from STLT_DESC_VL");
            if (rs != null && rs.RecordCount > 0)
            {
                stltDescriptions = CommonUtil.ConvertRSToEntity<StreetLightDescription>(rs);
            }
            return stltDescriptions;
        }

        /// <summary>
        /// Persist Street Light Description into Database
        /// </summary>
        /// <param name="stltDesc"></param>
        public void SaveStreetLightDesc(StreetLightDescription stltDesc)
        {
            if (stltDesc.EntityState == EntityMode.Add) { InsStreetLightDescription(stltDesc); }
            if (stltDesc.EntityState == EntityMode.Update) { UpdStreetLightDescription(stltDesc); }
            if (stltDesc.EntityState == EntityMode.Delete) { DelStreetLightDescription(stltDesc); }
        }
        #endregion

        #region Private Methods

        /// <summary>
        /// Insert Street Light Description
        /// </summary>
        /// <param name="stltDesc"></param>
        private void InsStreetLightDescription(StreetLightDescription stltDesc)
        {

            string sqlStmt = "Insert into STLT_DESC_VL(DESCRIPTION_ID,DESCRIPTION,MSLA_DATE) " +
                "values((select nvl(max(DESCRIPTION_ID),0)+1 from STLT_DESC_VL),'{0}',TO_DATE('{1}','MM/DD/YYYY'))";
            sqlStmt = string.Format(sqlStmt, stltDesc.DESCRIPTION, stltDesc.MSLA_Date.ToShortDateString());
            CommonUtil.Execute(sqlStmt);
        }

        /// <summary>
        /// Update Street Light Description 
        /// </summary>
        /// <param name="stltDesc"></param>
        private void UpdStreetLightDescription(StreetLightDescription stltDesc)
        {
            string sqlStmt = "Update STLT_DESC_VL set DESCRIPTION='{0}',MSLA_DATE=TO_DATE('{1}','MM/DD/YYYY') Where DESCRIPTION_ID={2}";
            sqlStmt = string.Format(sqlStmt, stltDesc.DESCRIPTION, stltDesc.MSLA_Date.ToShortDateString(), stltDesc.DescriptionID);
            CommonUtil.Execute(sqlStmt);
        }

        /// <summary>
        /// Delete Street Light Description from Database
        /// </summary>
        /// <param name="stltDesc"></param>
        private void DelStreetLightDescription(StreetLightDescription stltDesc)
        {
            string sqlStmt = "Delete From STLT_DESC_VL Where  DESCRIPTION_ID={0}";
            sqlStmt = string.Format(sqlStmt, stltDesc.DescriptionID);
            CommonUtil.Execute(sqlStmt);
        }
        #endregion

        #endregion

        #region Owner Value List

        #region Public Methods

        /// <summary>
        /// Get all Street Light Owner Value list
        /// </summary>
        /// <returns></returns>
        public BindingList<StreetLightOwner> GetSteetLightOwner()
        {
            BindingList<StreetLightOwner> stltOwnerCodes = new BindingList<StreetLightOwner>();
            Recordset rs = CommonUtil.Execute("Select OWNER_CODE as OwnerCode,OWNER_NAME as OwnerName  from STLT_OWNER_VL");
            if (rs != null && rs.RecordCount > 0)
            {
                stltOwnerCodes = CommonUtil.ConvertRSToEntity<StreetLightOwner>(rs);
            }
            return stltOwnerCodes;
        }

        /// <summary>
        /// Persist Street Light Owner into Database
        /// </summary>
        /// <param name="stltOwner"></param>
        public void SaveStreetLightOwner(StreetLightOwner stltOwner)
        {
            if (stltOwner.EntityState == EntityMode.Add) { InsStreetLightOwner(stltOwner); }
            if (stltOwner.EntityState == EntityMode.Update) { UpdStreetLightOwner(stltOwner); }
            if (stltOwner.EntityState == EntityMode.Delete) { DelStreetLightOwner(stltOwner); }
        }
        #endregion

        #region Private Methods

        /// <summary>
        /// Insert Street Light Owner
        /// </summary>
        /// <param name="stltOwner"></param>
        public void InsStreetLightOwner(StreetLightOwner stltOwner)
        {

            string sqlStmt = "Insert into STLT_OWNER_VL(OWNER_CODE,OWNER_NAME) " +
                "values('{0}','{1}')";
            sqlStmt = string.Format(sqlStmt, stltOwner.OwnerCode, stltOwner.OwnerName);
            CommonUtil.Execute(sqlStmt);
        }

        /// <summary>
        /// Update Street Light Owner
        /// </summary>
        /// <param name="stltOwner"></param>
        public void UpdStreetLightOwner(StreetLightOwner stltOwner)
        {
            string sqlStmt = "Update STLT_OWNER_VL set OWNER_CODE='{0}',OWNER_NAME='{1}' Where OWNER_CODE='{2}'";
            sqlStmt = string.Format(sqlStmt, stltOwner.OwnerCode, stltOwner.OwnerName, stltOwner.PrevOwnerCodeVal);
            CommonUtil.Execute(sqlStmt);
        }

        /// <summary>
        /// Delete Street Light Owner from Database
        /// </summary>
        /// <param name="stltOwner"></param>
        public void DelStreetLightOwner(StreetLightOwner stltOwner)
        {
            string sqlStmt = "Delete From STLT_OWNER_VL Where  OWNER_CODE='{0}'";
            sqlStmt = string.Format(sqlStmt, stltOwner.OwnerCode);
            CommonUtil.Execute(sqlStmt);
        }

        #endregion

        #endregion

        #region Rate Schedule Value List

        #region Public Methods
        /// <summary>
        /// Get all Street Light Rate Schedule Value List
        /// </summary>
        /// <returns></returns>
        public BindingList<StreetLightRateSchedule> GetStreetLightRateSchedule()
        {

            BindingList<StreetLightRateSchedule> stltRateSchedules = new BindingList<StreetLightRateSchedule>();
            //Recordset rs = 
            Recordset rs = CommonUtil.Execute("Select RATE_SCHEDULE as RateSchedule from STLT_SCHEDULE_VL");
            if (rs != null && rs.RecordCount > 0)
            {
                stltRateSchedules = CommonUtil.ConvertRSToEntity<StreetLightRateSchedule>(rs);
            }
            return stltRateSchedules;
        }

        /// <summary>
        /// Persist Street Light Rate Schedule into Database
        /// </summary>
        /// <param name="stltRateSch"></param>
        public void SaveSteetLightRateSchedule(StreetLightRateSchedule stltRateSch)
        {
            if (stltRateSch.EntityState == EntityMode.Add) { InsStreetLightRateSchedule(stltRateSch); }
            if (stltRateSch.EntityState == EntityMode.Update) { UpdStreetLightRateSchedule(stltRateSch); }
            if (stltRateSch.EntityState == EntityMode.Delete) { DelStreetLightRateSchedule(stltRateSch); }
        }
        #endregion

        #region Private Methods

        /// <summary>
        /// Insert Street Light Rate Schedule into Database
        /// </summary>
        /// <param name="stltRateSch"></param>
        private void InsStreetLightRateSchedule(StreetLightRateSchedule stltRateSch)
        {

            string sqlStmt = "Insert into STLT_SCHEDULE_VL(RATE_SCHEDULE) values('{0}')";
            sqlStmt = string.Format(sqlStmt, stltRateSch.RateSchedule);
            CommonUtil.Execute(sqlStmt);
        }

        /// <summary>
        /// Update Street Light Rate Schedule
        /// </summary>
        /// <param name="stltRateSch"></param>
        private void UpdStreetLightRateSchedule(StreetLightRateSchedule stltRateSch)
        {
            string sqlStmt = "Update STLT_SCHEDULE_VL set RATE_SCHEDULE='{0}' Where RATE_SCHEDULE='{1}'";
            sqlStmt = string.Format(sqlStmt, stltRateSch.RateSchedule, stltRateSch.PrevRateScheduleVal);
            CommonUtil.Execute(sqlStmt);
        }

        /// <summary>
        /// Delete Street Light Rate Schedule from Database
        /// </summary>
        /// <param name="stltRateSch"></param>
        private void DelStreetLightRateSchedule(StreetLightRateSchedule stltRateSch)
        {
            string sqlStmt = "Delete From STLT_SCHEDULE_VL Where  RATE_SCHEDULE='{0}'";
            sqlStmt = string.Format(sqlStmt, stltRateSch.RateSchedule);
            CommonUtil.Execute(sqlStmt);
        }
        #endregion

        #endregion

        #region Rate Code Value List

        #region Public Methods
        /// <summary>
        /// Get all Street Light Rate Code Value list
        /// </summary>
        /// <returns></returns>
        public BindingList<StreetLightRateCode> GetSteetLightRateCode()
        {

            BindingList<StreetLightRateCode> stltRateCodes = new BindingList<StreetLightRateCode>();
            Recordset rs = CommonUtil.Execute("Select RATE_CODE as RateCode  from STLT_RATECODE_VL");
            if (rs != null && rs.RecordCount > 0)
            {
                stltRateCodes = CommonUtil.ConvertRSToEntity<StreetLightRateCode>(rs);
            }
            return stltRateCodes;
        }

        /// <summary>
        /// Persist Street Light Rate Code into database
        /// </summary>
        /// <param name="stltRateCode"></param>
        public void SaveStreetLightRateCode(StreetLightRateCode stltRateCode)
        {
            if (stltRateCode.EntityState == EntityMode.Add) { InsStreetLightRateCode(stltRateCode); }
            if (stltRateCode.EntityState == EntityMode.Update) { UpdStreetLightRateCode(stltRateCode); }
            if (stltRateCode.EntityState == EntityMode.Delete) { DelStreetLightRateCode(stltRateCode); }
        }
        #endregion

        #region Private Methods

        /// <summary>
        /// Insert Street Light Rate code
        /// </summary>
        /// <param name="stltRateCode"></param>
        private void InsStreetLightRateCode(StreetLightRateCode stltRateCode)
        {

            string sqlStmt = "Insert into STLT_RATECODE_VL(RATE_CODE) values('{0}')";
            sqlStmt = string.Format(sqlStmt, stltRateCode.RateCode);
            CommonUtil.Execute(sqlStmt);
        }

        /// <summary>
        /// Update Street Light Rate code
        /// </summary>
        /// <param name="stltRateCode"></param>
        private void UpdStreetLightRateCode(StreetLightRateCode stltRateCode)
        {
            string sqlStmt = "Update STLT_RATECODE_VL set RATE_CODE='{0}' Where RATE_CODE='{1}'";
            sqlStmt = string.Format(sqlStmt, stltRateCode.RateCode, stltRateCode.PrevRateCodeVal);
            CommonUtil.Execute(sqlStmt);
        }


        /// <summary>
        /// Delete Street Light Rate code from Database
        /// </summary>
        /// <param name="stltRateCode"></param>
        private void DelStreetLightRateCode(StreetLightRateCode stltRateCode)
        {
            string sqlStmt = "Delete From STLT_RATECODE_VL Where  RATE_CODE='{0}'";
            sqlStmt = string.Format(sqlStmt, stltRateCode.RateCode);
            CommonUtil.Execute(sqlStmt);
        }

        #endregion

        #endregion


    }
}
