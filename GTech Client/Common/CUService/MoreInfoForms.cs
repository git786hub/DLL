using ADODB;
using Intergraph.GTechnology.API;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Linq;

namespace GTechnology.Oncor.CustomAPI
{
    public enum RemovalActivity
    {
        Remove, Salvage,NoValue
    }
    public enum InstallActivity
    {
        Select, Replace, DoNotInstall,Novalue
    }
    public class CuCommonForms
    {
        public RemovalActivity RemovalActivitySelected { get; set; }
        public InstallActivity InstallActivitySelected { get; set; }
        private IGTApplication m_oApp;
        private bool bReplacewithSameCU = true;
        public bool CancelClicked { get; set; }
        public CuCommonForms(IGTApplication oApp ,bool bReplaceCU=true)
        {
            m_oApp = oApp;
            //ALM-1592-- Added a parameter to disable the "Replace with same CU" option if the CU is not found
            bReplacewithSameCU = bReplaceCU;
        }
        public void ShowChangeOutForm()
        {
            //ALM-1592-- Added a parameter to disable the "Replace with same CU" option if the CU is not found
            dlgChangeOut obj = new dlgChangeOut(bReplacewithSameCU);
            obj.ShowDialog();            
            RemovalActivitySelected = obj.RemovalActivitySet;
            InstallActivitySelected = obj.InstallActivitySet;
            CancelClicked = obj.CancelClicked;            
        }

        public void ShowMoreInfoForm(string CU, bool IsPrimaryCU, bool IsCU)
        {                   
            PopulateMoreCuInformation(CU, IsPrimaryCU, IsCU);
        }

        #region private methods

        private void GetDataTableWithUsernames(ref DataTable p_Data, bool p_IsPrimary)
        {
            Dictionary<string, string> dicColumnMapping = new Dictionary<string, string>();
            try
            {
                dicColumnMapping = GetMappedColumns(p_IsPrimary);
                string sUsername = string.Empty;

                for (int i = 0; i < p_Data.Columns.Count; i++)
                {
                    if (dicColumnMapping.ContainsKey(p_Data.Columns[i].ColumnName))
                    {
                        dicColumnMapping.TryGetValue(p_Data.Columns[i].ColumnName, out sUsername);
                        p_Data.Columns[i].ColumnName = sUsername;
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }           
        }
        
        private Dictionary<string, string> GetMappedColumns(bool p_IsPrimary)
        {
            Dictionary<string, string> dicColumnToUserNameMapping = new Dictionary<string, string>();
            if (p_IsPrimary)
            {
                dicColumnToUserNameMapping.Add("MATERIAL_ID", "Material Code");
                dicColumnToUserNameMapping.Add("MINIMUM_ISSUE_QTY", "TSN Quantity");
                dicColumnToUserNameMapping.Add("MMS_UNIT_COST", "Unit Amount");
                dicColumnToUserNameMapping.Add("UNIT_OF_MEASURE_C", "Unit of Measure");
                dicColumnToUserNameMapping.Add("MATERIAL_DESC", "Material Description");
            }
            else
            {                
                dicColumnToUserNameMapping.Add("CU_ID", "CU ID");
                dicColumnToUserNameMapping.Add("CATEGORY_C", "Category");
                dicColumnToUserNameMapping.Add("CU_DESC", "Description");
                dicColumnToUserNameMapping.Add("CU_QTY", "Qty");
                dicColumnToUserNameMapping.Add("TYPE_C", "Type");
                dicColumnToUserNameMapping.Add("EFFECTIVE_D", "Effective Date");
                dicColumnToUserNameMapping.Add("EXPIRATION_D", "Expiration Date");
            }
            return dicColumnToUserNameMapping;
        }
        private void PopulateMoreCuInformation(string CU, bool IsPrimaryCU, bool IsCU)
        {
            Dictionary<string, string> dicColumnToUserNameMapping = new Dictionary<string, string>();
            ADODB.Recordset rs = null;
            ADODB.Recordset rsMaterial = null;

            try
            {
                if (IsCU)
                {
                    Dictionary<string, string> dColumnToUserNameMapping = new Dictionary<string, string>();

                    dlgMoreInfoForm oFormMoreInfo = new dlgMoreInfoForm();
                    rs = m_oApp.DataContext.OpenRecordset(@"select a.ID,a.CU_ID,a.CU_DESC,a.CATEGORY_C,a.PRIME_ACCT_NBR,a.RETIREMENT_NBR,a.PROPERTY_UNIT_C,to_char(a.EFFECTIVE_D,'DD-MON-YYYY') EFFECTIVE_D,to_char(a.EXPIRATION_D,'DD-MON-YYYY') EXPIRATION_D,a.MATERIAL_AMT,a.UNIT_OF_MEASURE_C, a.SHORT_WORK_INSTRUCTION, b.CATEGORY_DESC from CULIB_UNIT a, CULIB_CATEGORY b where a.CATEGORY_C = b.CATEGORY_C and a.CU_ID = ?", ADODB.CursorTypeEnum.adOpenDynamic, ADODB.LockTypeEnum.adLockOptimistic, (int)ADODB.CommandTypeEnum.adCmdText, CU);
                    if (rs != null)
                    {
                        if (rs.RecordCount > 0)
                        {
                            oFormMoreInfo.CUID = Convert.ToString(rs.Fields["CU_ID"].Value);
                            oFormMoreInfo.CUDescription = Convert.ToString(rs.Fields["CU_DESC"].Value);
                            oFormMoreInfo.EffectiveDate = Convert.ToString(rs.Fields["EFFECTIVE_D"].Value);
                            oFormMoreInfo.ExpirationDate = Convert.ToString(rs.Fields["EXPIRATION_D"].Value); oFormMoreInfo.PropertyUnitCode = Convert.ToString(rs.Fields["PROPERTY_UNIT_C"].Value);
                            oFormMoreInfo.RetirementType = Convert.ToString(rs.Fields["RETIREMENT_NBR"].Value);
                            oFormMoreInfo.CapitalPrimeAccount = Convert.ToString(rs.Fields["PRIME_ACCT_NBR"].Value);
                            oFormMoreInfo.CUCategory = Convert.ToString(rs.Fields["CATEGORY_C"].Value);
                            oFormMoreInfo.CategoryDescription = Convert.ToString(rs.Fields["CATEGORY_DESC"].Value);
                            rsMaterial = m_oApp.DataContext.OpenRecordset("select A.MATERIAL_ID TSN,A.MATERIAL_SOURCE_C, A.MINIMUM_ISSUE_QTY, A.MMS_UNIT_COST,A.UNIT_OF_MEASURE_C , A.MATERIAL_DESC from CULIB_MATERIAL a, CULIB_UNITMATERIAL b where a.MATERIAL_ID = b.MATERIAL_ID and b.CU_ID = ? ", ADODB.CursorTypeEnum.adOpenStatic, ADODB.LockTypeEnum.adLockOptimistic, (int)ADODB.CommandTypeEnum.adCmdText, CU);

                            DataTable dtData = null;
                            if (rsMaterial != null)
                            {
                                if (rsMaterial.RecordCount > 0)
                                {
                                    dtData = GetDataTable(rsMaterial);
                                    GetDataTableWithUsernames(ref dtData, IsCU);
                                    oFormMoreInfo.TSNData = dtData;
                                }
                            }
                        }
                        oFormMoreInfo.ShowDialog(m_oApp.ApplicationWindow);
                    }
                }
                else
                {
                    dlgMCUMoreInfo oFormMoreInfoMCU = new dlgMCUMoreInfo();
                    rs = m_oApp.DataContext.OpenRecordset("select * from CULIB_MACRO where MU_ID = ?", ADODB.CursorTypeEnum.adOpenDynamic, ADODB.LockTypeEnum.adLockOptimistic, (int)ADODB.CommandTypeEnum.adCmdText, CU);
                    if (rs.RecordCount > 0)
                    {
                        oFormMoreInfoMCU.MUID = Convert.ToString(rs.Fields["MU_ID"].Value);
                        oFormMoreInfoMCU.MUDescription = Convert.ToString(rs.Fields["MU_DESC"].Value);
                        rsMaterial = m_oApp.DataContext.OpenRecordset(@"select distinct a.CU_ID,a.CU_DESC,a.CATEGORY_C,to_char(a.EFFECTIVE_D,'DD-MON-YYYY') EFFECTIVE_D,to_char(a.EXPIRATION_D,'DD-MON-YYYY') EXPIRATION_D, b.CU_QTY, d.TYPE_C from CULIB_UNIT a,CULIB_MACROUNIT b, CULIB_MACRO c, CULIB_CATEGORY d  where 
                            a.CU_ID IN(SELECT CU_ID FROM CULIB_MACROUNIT WHERE MU_ID = ?) and a.CATEGORY_C = d.CATEGORY_C  and b.mu_id = c.mu_id
                            and a.CU_ID = b.cu_id and c.category_c = ?", ADODB.CursorTypeEnum.adOpenDynamic, ADODB.LockTypeEnum.adLockOptimistic, (int)ADODB.CommandTypeEnum.adCmdText, CU, Convert.ToString(rs.Fields["CATEGORY_C"].Value));

                        if (rsMaterial != null)
                        {
                            if (rsMaterial.RecordCount > 0)
                            {
                                DataTable dtData = null;
                                dtData = GetDataTable(rsMaterial);
                                GetDataTableWithUsernames(ref dtData, IsCU);
                                oFormMoreInfoMCU.GridData = dtData;
                            }
                        }
                    }
                    oFormMoreInfoMCU.ShowDialog(m_oApp.ApplicationWindow);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }           
        }
        public string GetPreferredCuFilterString(ADODB.Recordset p_PreferredCuRecords)
        {
            List<string> sPreferredCuList = new List<string>();
            string sReturnFilter = string.Empty;

            if (p_PreferredCuRecords != null)
            {
                if (p_PreferredCuRecords.RecordCount > 0)
                {
                    p_PreferredCuRecords.MoveFirst();
                    while (p_PreferredCuRecords.EOF == false)
                    {
                        sPreferredCuList.Add("'" + Convert.ToString(p_PreferredCuRecords.Fields["CU_CODE"].Value) + "'");
                        p_PreferredCuRecords.MoveNext();

                    }
                    sReturnFilter = string.Join(",", sPreferredCuList.ToArray());
                }
            }

            return sReturnFilter;
        }

        public void GetPreferredCUData(String p_PRefUID)
        {
            string sql = "insert into CUSELECT_USERPREF values (?,?,?)";
            List<string> queryParams = new List<string>();
            ADODB.Recordset rs = null;

            queryParams.Add(p_PRefUID);

            rs= m_oApp.DataContext.OpenRecordset(sql, CursorTypeEnum.adOpenDynamic, LockTypeEnum.adLockOptimistic, (int)ADODB.CommandTypeEnum.adCmdText, queryParams.ToArray<object>());            
        }

        public ADODB.Recordset GetPreferredCU(string p_CategoryCode)
        {
            ADODB.Recordset rs = null;
            string sql = string.Empty;
            sql = "select CU_CODE from CUSELECT_USERPREF where PREF_UID = ? and CU_CATEGORY_CODE =?";
            rs = m_oApp.DataContext.OpenRecordset(sql, CursorTypeEnum.adOpenDynamic, LockTypeEnum.adLockOptimistic, (int)ADODB.CommandTypeEnum.adCmdText, m_oApp.DataContext.DatabaseUserName, p_CategoryCode);
            return rs;
        }
        public void MakePreferredCU(string p_CU, string p_CategoryCode, string PrefUID)
        {            
            string sql = "insert into CUSELECT_USERPREF values (?,?,?)";          
            List<string>queryParams = new List<string>();
            queryParams.AddRange(new string[] { PrefUID, p_CU, p_CategoryCode });

            m_oApp.DataContext.OpenRecordset(sql, CursorTypeEnum.adOpenDynamic, LockTypeEnum.adLockOptimistic, (int)ADODB.CommandTypeEnum.adCmdText, queryParams.ToArray<object>());

            sql = "commit";
            m_oApp.DataContext.OpenRecordset(sql, CursorTypeEnum.adOpenDynamic, LockTypeEnum.adLockOptimistic, (int)ADODB.CommandTypeEnum.adCmdText, queryParams);
        }
        public static DataTable GetDataTable(Recordset oRS)
        {
            try
            {
                OleDbDataAdapter oDA = new System.Data.OleDb.OleDbDataAdapter();
                DataTable oDT = new DataTable();
                oDA.Fill(oDT, oRS);
                oDA.Dispose();
                oRS.Close();
                return oDT;
            }
            catch (Exception oEx)
            {
                throw oEx;
            }
        }
        #endregion
    }

}
