using ADODB;
using GTechnology.Oncor.CustomAPI.Model;
using System;
using System.ComponentModel;

namespace GTechnology.Oncor.CustomAPI.DataAccess
{
    /// <summary>
    /// Methods in the class to manage Street Light Boundary dialog
    /// </summary>
    public class StreetLightBoundaryContext
    {

        #region Public Methods

        /// <summary>
        /// Get all Street Light Boundarys
        /// </summary>
        /// <returns></returns>
        public BindingList<StreetLightBoundary> GetSteetLightBoundarys()
        {

            BindingList<StreetLightBoundary> stltBoundary = new BindingList<StreetLightBoundary>();
            Recordset rs = CommonUtil.Execute("select BND_CLASS,BND_FNO,BND_TYPE_ANO,BND_TYPE,BND_ID_ANO" +
                ",A.G3E_FIELD AS Bnd_ID_G3efield,A.G3E_USERNAME AS Bnd_ID_G3eUsername,A.G3E_COMPONENTTABLE AS Bnd_ID_G3eCName" +
                ",B.G3E_FIELD AS Bnd_Type_G3eField,B.G3E_USERNAME AS Bnd_Type_G3eUsername,B.G3E_COMPONENTTABLE AS Bnd_Type_G3eCNname " +
                " FROM STLT_BOUNDARY S,G3E_ATTRIBUTEINFO_OPTABLE A,G3E_ATTRIBUTEINFO_OPTABLE B" +
                " WHERE s.BND_ID_ANO=a.g3e_ano and s.BND_TYPE_ANO=B.g3e_ano(+)");

            if (rs != null && rs.RecordCount > 0)
            {
                stltBoundary = CommonUtil.ConvertRSToEntity<StreetLightBoundary>(rs);
            }
            return stltBoundary;
        }


        /// <summary>
        /// Get Boundary G3efid for given input Boundary Identifier Attribute Value
        /// </summary>
        /// <param name="bndry"></param>
        /// <param name="idValue"></param>
        /// <returns>return 0 if there is no records or more than one record exists for given input</returns>
        public int GetBoundaryByIDValue(StreetLightBoundary bndry, string idValue)
        {
            Recordset rs = null;
            int g3eFid = 0;
            if (string.IsNullOrEmpty(bndry.Bnd_Type))
            {
                rs = CommonUtil.Execute(string.Format("Select G3E_FID from {0} where {1}='{2}'", bndry.Bnd_ID_G3eCName, bndry.Bnd_ID_G3efield, idValue));
            }
            else
            {
                rs= CommonUtil.Execute(string.Format("Select G3E_FID from {0} where {1}='{2}' and {3}='{4}'", bndry.Bnd_ID_G3eCName, bndry.Bnd_ID_G3efield, idValue,bndry.Bnd_Type_G3eField,bndry.Bnd_Type));
            }

            if (rs != null && rs.RecordCount==1)
            {
                g3eFid = Convert.ToInt32(rs.Fields["G3E_FID"].Value);
            }
            return g3eFid;
        }


        /// <summary>
        /// Save Street Light Boundary changes
        /// </summary>
        /// <param name="stltBoundry"></param>
        public void SaveStreetLightBoundary(StreetLightBoundary stltBoundry)
        {
            if (stltBoundry.EntityState == EntityMode.Add) { InsStreetLightBndry(stltBoundry); }
            if (stltBoundry.EntityState == EntityMode.Update) { UpdStreetLightBndry(stltBoundry); }
            if (stltBoundry.EntityState == EntityMode.Delete) { DelStreetLightBndry(stltBoundry); }
        }
        #endregion

        #region Private Methods

        /// <summary>
        /// Insert Street Light Boundary
        /// </summary>
        /// <param name="stltBoundry"></param>
        private void InsStreetLightBndry(StreetLightBoundary stltBoundry)
        {

            string sqlStmt = "Insert into STLT_BOUNDARY (BND_FNO,BND_TYPE_ANO,BND_TYPE,BND_ID_ANO) values ({0},{1},'{2}',{3}) ";
            sqlStmt = string.Format(sqlStmt, stltBoundry.Bnd_Fno, (stltBoundry.Bnd_Type_Ano == 0) ? "NULL" : stltBoundry.Bnd_Type_Ano.ToString(), stltBoundry.Bnd_Type, stltBoundry.Bnd_ID_Ano);
            CommonUtil.Execute(sqlStmt);

        }

        /// <summary>
        /// update Street Light Boundary
        /// </summary>
        /// <param name="stltBoundry"></param>
        private void UpdStreetLightBndry(StreetLightBoundary stltBoundry)
        {

            string sqlStmt = "Update STLT_BOUNDARY set BND_FNO={0},BND_TYPE_ANO={1},BND_TYPE='{2}',BND_ID_ANO={3} Where BND_CLASS={4}";
            sqlStmt = string.Format(sqlStmt, stltBoundry.Bnd_Fno, (stltBoundry.Bnd_Type_Ano == 0) ? "NULL" : stltBoundry.Bnd_Type_Ano.ToString(), stltBoundry.Bnd_Type, stltBoundry.Bnd_ID_Ano, stltBoundry.Bnd_Class);
            CommonUtil.Execute(sqlStmt);

        }

        /// <summary>
        /// Delete Street Light boundary 
        /// </summary>
        /// <param name="stltBoundry"></param>
        private void DelStreetLightBndry(StreetLightBoundary stltBoundry)
        {

            string sqlStmt = "Delete From STLT_BOUNDARY Where BND_CLASS={0}";
            sqlStmt = string.Format(sqlStmt, stltBoundry.Bnd_Class);
            CommonUtil.Execute(sqlStmt);

        }
        #endregion


    }
}
