//----------------------------------------------------------------------------+
//  Class: CommonSetCUStandardAttributes
//  Description: This class library is a common library that exposes methods that are responsible for the population
//  of CU default attributes and the standard attributed in the Unit component     
//----------------------------------------------------------------------------+
//  $Author:: Shubham Agarwal                                                       $
//  $Date:: 26/07/18                                                                $
//  $Revision:: 1                                                                   $
//----------------------------------------------------------------------------+
//    $History:: CommonSetCUStandardAttributes.cs                     $
// 
// *****************  Version 1  *****************
// User: asgiribo     Date: 24/04/19   Time: 12:00  Desc : Included Formation.Quantity/Length CU population as per ALM 2299.
//----------------------------------------------------------------------------+

using System;
using ADODB;
using Intergraph.GTechnology.API;

namespace GTechnology.Oncor.CustomAPI
{
    public class CommonSetCUStandardAttributes
    {
        private IGTApplication m_oApp;
        private IGTComponents m_oComponents;
        private string m_componentName;     
        private const string M_SIWGENERALPARAMATERTABLE = "SYS_GENERALPARAMETER";
        private const short m_iAncCompUnitCNO = 22;
        private IGTDataContext m_oDataContext;

        public CommonSetCUStandardAttributes(IGTComponents p_component, string p_componentName)
        {
            m_oApp = GTClassFactory.Create<IGTApplication>();
            m_oDataContext = m_oApp.DataContext;
            m_oComponents = p_component;
            m_componentName = p_componentName;
        }

        /// <summary>
        /// Method to set the standard attributes corresponding to the CU set
        /// </summary>
        public void SetStandardAttributes()
        {
            try
            {             
                // When CU attribute is set - Set the Unit attributes corresponding to the CID
                // If already UnitCId is set that means already CID exists, update that
                // In case of ChangeOut - New component is created with Install activity, It will always create a new CID for the standard attributes or will update the changed out, this conclusion needs to be made and till then this piece of code is not final

                int UnitComponentCNO = GetUnitComponentCNO(Convert.ToString(m_oComponents[m_componentName].Recordset.Fields["CU_C"].Value), Convert.ToInt32(m_oComponents[m_componentName].Recordset.Fields["G3e_CID"].Value), Convert.ToInt32(m_oComponents[m_componentName].Recordset.Fields["G3e_CNO"].Value), m_oDataContext);
               
                int iCurrentCID = Convert.ToInt32(m_oComponents[m_componentName].Recordset.Fields["G3e_CID"].Value);
             
                if (UnitComponentCNO != 0)
                {
                    //First set the UnitCID and UnitCNO

                  string  sFieldName = GetParameter("CU UnitCID Field Name", "CUSelection", "CUSelection", true).ToString();

                    if (string.IsNullOrEmpty(Convert.ToString(m_oComponents[m_componentName].Recordset.Fields[sFieldName].Value)))
                    {
                        m_oComponents[m_componentName].Recordset.Fields[sFieldName].Value = iCurrentCID;
                    }

                    MoveRecordSetToCurrentCID(m_oComponents[m_componentName].Recordset, iCurrentCID);

                    sFieldName = GetParameter("CU Unit CNO Field Name", "CUSelection", "CUSelection", true).ToString();

                    if (string.IsNullOrEmpty(Convert.ToString(m_oComponents[m_componentName].Recordset.Fields[sFieldName].Value)))
                    {
                        m_oComponents[m_componentName].Recordset.Fields[sFieldName].Value = UnitComponentCNO;
                    }

                    MoveRecordSetToCurrentCID(m_oComponents[m_componentName].Recordset, iCurrentCID);
                }

                MoveRecordSetToCurrentCID(m_oComponents[m_componentName].Recordset, Convert.ToInt32(m_oComponents[m_componentName].Recordset.Fields["G3e_CID"].Value));

                int UnitCIDCreated = SetStaticAttrModifiedVersion(Convert.ToString(m_oComponents[m_componentName].Recordset.Fields["CU_C"].Value), (m_oComponents[m_componentName].Recordset.Fields["UNIT_CID"].Value.GetType() == typeof(DBNull) ? 0 : Convert.ToInt32(m_oComponents[m_componentName].Recordset.Fields["UNIT_CID"].Value)), Convert.ToInt32(m_oComponents[m_componentName].Recordset.Fields["G3e_CNO"].Value), m_oDataContext);

                if (UnitCIDCreated != 0)
                {
                    //Update UNITCID
                    m_oComponents[m_componentName].Recordset.Fields["UNIT_CID"].Value = UnitCIDCreated;
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Method to set CU attributes corresponding to the CU Code set
        /// </summary>
        public void SetCUAttributes()
        {
            try
            {
                SetONCORSpecificDefaultAttributes(m_oComponents[m_componentName].Recordset, Convert.ToInt32(m_oComponents[m_componentName].Recordset.Fields["G3e_CID"].Value));
            }
            catch (Exception)
            {
                throw;
            }
        }

        #region Private methods

        private void MoveRecordSetToCurrentCID(ADODB.Recordset p_oCompRS, int p_iCurrentCID)
        {
            int CID = p_iCurrentCID;
            if (CID > 0)
            {
                m_oComponents[m_componentName].Recordset.MoveFirst();
                while (true)
                {
                    if (Convert.ToInt32(m_oComponents[m_componentName].Recordset.Fields["G3E_CID"].Value) == CID)
                    {
                        break;
                    }
                    m_oComponents[m_componentName].Recordset.MoveNext();
                }
            }
        }
        private string GetParameter(string p_sParameterName, string p_sSubSystemName, string p_sSubsystemComponent, bool p_bErrorIfNotExist)
        {
            //Retrieves the parameter value from the recordset.
            //Throws exception if parameter not found.
            ADODB.Recordset oRS = null;
            Exception oEx = null;
            System.String sRetVal = string.Empty;

            try
            {
                oRS = m_oDataContext.OpenRecordset("select * from " + M_SIWGENERALPARAMATERTABLE + " where PARAM_NAME=? and SUBSYSTEM_COMPONENT = ? and SUBSYSTEM_NAME =?", ADODB.CursorTypeEnum.adOpenStatic, ADODB.LockTypeEnum.adLockReadOnly, (System.Int32)ADODB.CommandTypeEnum.adCmdText, p_sParameterName, p_sSubSystemName, p_sSubsystemComponent);
            }
            catch (Exception ex)
            {
                if (!(oRS == null))
                {
                    if ((int)ADODB.ObjectStateEnum.adStateOpen == oRS.State)
                    {
                        oRS.Close();
                    }
                }
                throw (ex);
            }

            if (oRS.BOF && oRS.EOF)
            {
                oRS.Close();
                if (p_bErrorIfNotExist)
                {
                    oEx = new Exception("Entry not found in " + M_SIWGENERALPARAMATERTABLE + " where PARAM_NAME = \'" + System.Convert.ToString(p_sParameterName) + "\'");
                    throw (oEx);
                }
            }
            else
            {
                sRetVal = oRS.Fields["PARAM_VALUE"].Value.ToString();
                oRS.Close();
            }

            return sRetVal;

        }

        private bool IsDBNull(object oValue)
        {
            bool bReturn = false;
            if (oValue.GetType() == typeof(DBNull))
            {
                bReturn = true;
            }
            return bReturn;
        }

        /// <summary>
        /// Method to set the ONCOR specific default attributes
        /// </summary>
        /// <param name="oCompRS"></param>
        /// <param name="iCurrentCID"></param>
        /// <param name="iCNO"></param>
        private void SetONCORSpecificDefaultAttributes(ADODB.Recordset oCompRS, int iCurrentCID)
        {
            try
            {
                string sFieldName = "";               
                string sCU = Convert.ToString(oCompRS.Fields["CU_C"].Value);
                IGTKeyObject oKeyObj = m_oDataContext.OpenFeature(Convert.ToInt16(oCompRS.Fields["G3E_FNO"].Value), Convert.ToInt32(oCompRS.Fields["G3E_FID"].Value));

                ADODB.Recordset rs = m_oDataContext.OpenRecordset("select * from CULIB_UNIT where CU_ID =?", CursorTypeEnum.adOpenDynamic, LockTypeEnum.adLockReadOnly, (int)ADODB.CommandTypeEnum.adCmdText, sCU);
                if (rs != null)
                {
                    if (rs.RecordCount > 0)
                    {
                        sFieldName = "";
                        sFieldName = GetParameter("CU Description Field Name", "CUSelection", "CUSelection", true).ToString();
                        if (Convert.ToString(rs.Fields["CU_DESC"].Value).Length > 59)
                        {
                            oCompRS.Fields[sFieldName].Value = Convert.ToString(rs.Fields["CU_DESC"].Value).Substring(0, 59);
                        }
                        else
                        {
                            oCompRS.Fields[sFieldName].Value = Convert.ToString(rs.Fields["CU_DESC"].Value);

                        }

                        MoveRecordSetToCurrentCID(oCompRS, iCurrentCID);

                        sFieldName = "";
                        sFieldName = GetParameter("CU Prime Account # Field Name", "CUSelection", "CUSelection", true).ToString();
                        if (!IsDBNull(rs.Fields["PRIME_ACCT_NBR"].Value))
                        {
                            oCompRS.Fields[sFieldName].Value = Convert.ToString(rs.Fields["PRIME_ACCT_NBR"].Value);
                            MoveRecordSetToCurrentCID(oCompRS, iCurrentCID);
                        }

                        sFieldName = "";
                        sFieldName = GetParameter("CU Property Unit Field Name", "CUSelection", "CUSelection", true).ToString();
                        if (!IsDBNull(rs.Fields["PROPERTY_UNIT_C"].Value))
                        {
                            oCompRS.Fields[sFieldName].Value = Convert.ToString(rs.Fields["PROPERTY_UNIT_C"].Value);
                            MoveRecordSetToCurrentCID(oCompRS, iCurrentCID);
                        }

                        sFieldName = "";
                        sFieldName = GetParameter("CU Retirement Type Name", "CUSelection", "CUSelection", true).ToString();
                        if (!IsDBNull(rs.Fields["RETIREMENT_NBR"].Value))
                        {
                            oCompRS.Fields[sFieldName].Value = Convert.ToString(rs.Fields["RETIREMENT_NBR"].Value);
                            MoveRecordSetToCurrentCID(oCompRS, iCurrentCID);
                        }

                        sFieldName = "";
                        sFieldName = GetParameter("CU Length Flag Field Name", "CUSelection", "CUSelection", true).ToString();
                        string sFieldName2 = GetParameter("CU Quantity/Length Name", "CUSelection", "CUSelection", true).ToString();

                        if (Convert.ToString(rs.Fields["UNIT_OF_MEASURE_C"].Value).Equals("FT"))
                        {
                            oCompRS.Fields[sFieldName].Value = "L";

                            oCompRS.Fields[sFieldName2].Value = GetQuantityLength(oKeyObj) ;
                        }
                        else
                        {
                            oCompRS.Fields[sFieldName].Value = "Q";

                            //if (Convert.ToInt32(oCompRS.Fields["G3E_CNO"].Value) != m_iAncCompUnitCNO)
                            //{
                                oCompRS.Fields[sFieldName2].Value = 1;
                            //}
                        }

                        MoveRecordSetToCurrentCID(oCompRS, iCurrentCID);

                        sFieldName = "";
                        sFieldName = GetParameter("CU Installed WR", "CUSelection", "CUSelection", true).ToString();
                        string sInstalledWR = Convert.ToString(oCompRS.Fields[sFieldName].Value);

                        oCompRS.Fields[sFieldName].Value = m_oApp.DataContext.ActiveJob;
                        MoveRecordSetToCurrentCID(oCompRS, iCurrentCID);
                       
                        sFieldName = "";
                        sFieldName = GetParameter("CU Edited WR", "CUSelection", "CUSelection", true).ToString();
                        if (!sInstalledWR.Equals(m_oApp.DataContext.ActiveJob))
                        {
                            oCompRS.Fields[sFieldName].Value = m_oApp.DataContext.ActiveJob;
                            MoveRecordSetToCurrentCID(oCompRS, iCurrentCID);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private string GetQuantityLength(IGTKeyObject p_keyObj)
        {
            IGTKeyObjects DuctsBankObjects = null;
            string length = null;
            try
            {
                if (p_keyObj.FNO == 2400)
                {
                    using (IGTRelationshipService o_RelSVC = GTClassFactory.Create<IGTRelationshipService>())
                    {
                        o_RelSVC.DataContext = m_oDataContext;
                        o_RelSVC.ActiveFeature = p_keyObj;
                        DuctsBankObjects = o_RelSVC.GetRelatedFeatures(6);
                    }

                    for (int i = 0; i <= DuctsBankObjects.Count - 1; i++)
                    {
                        if (DuctsBankObjects[i].FNO == 2200)
                        {
                            p_keyObj = DuctsBankObjects[i];
                            break;
                        }
                    }
                }

                if (p_keyObj.Components["COMMON_N"] != null && p_keyObj.Components["COMMON_N"].Recordset.RecordCount > 0)
                {
                    p_keyObj.Components["COMMON_N"].Recordset.MoveFirst();

                    if (!IsDBNull(p_keyObj.Components["COMMON_N"].Recordset.Fields["LENGTH_ACTUAL_Q"].Value))
                    {
                        length = Convert.ToString(p_keyObj.Components["COMMON_N"].Recordset.Fields["LENGTH_ACTUAL_Q"].Value);
                    }
                }
            }
            catch
            {
                throw;
            }
            return length;
        }

        /// <summary>
        /// Method to get the Component Number of the Unit Component whose standard/static attributes will be populated
        /// </summary>
        /// <param name="sCUCode"></param>
        /// <param name="iCUCID"></param>
        /// <param name="m_iCompUnitCNO"></param>
        /// <param name="m_oDataContext"></param>
        /// <returns></returns>
        private int GetUnitComponentCNO(string sCUCode, int iCUCID, int m_iCompUnitCNO, IGTDataContext m_oDataContext)
        {
            int sUnitComponentCNO = 0;
            ADODB.Recordset oRS = null;
            string sSQL = string.Empty;

            IGTKeyObject oKeyObject;
            ///''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
            //Query for the ANOs and their values from the m_sCUStaticAttributeTable table.
            //For each record returned in that query, set the ANO value.
            ///''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

            oKeyObject = m_oApp.DataContext.OpenFeature(Convert.ToInt16(m_oComponents[m_componentName].Recordset.Fields["G3e_FNO"].Value), Convert.ToInt32(m_oComponents[m_componentName].Recordset.Fields["G3e_FID"].Value));

            sSQL = @" select distinct a.g3e_cno from CULIB_ATTRIBUTE s,G3E_ATTRIBUTEINFO_OPTABLE a, CULIB_UNITATTRIBUTE b
                    where s.g3e_fno=? and b.cu_id=? and a.g3e_ano=s.g3e_ano and s.category_C = b.category_c and s.ATTRIBUTE_ID = b.ATTRIBUTE_ID
                        order by a.g3e_cno";

            oRS = m_oDataContext.OpenRecordset(sSQL, ADODB.CursorTypeEnum.adOpenStatic, ADODB.LockTypeEnum.adLockReadOnly, (int)ADODB.CommandTypeEnum.adCmdText, oKeyObject.FNO, sCUCode);

            if (oRS != null && oRS.RecordCount > 0)
            {
                sUnitComponentCNO = Convert.ToInt32(oRS.Fields["g3e_cno"].Value);
            }
            return sUnitComponentCNO;
        }
        private void SetStaticAttr(string sCUCode, int iCUCID, int m_iCompUnitCNO, IGTDataContext m_oDataContext)
        {
            IGTKeyObject oKeyObject;
            ///''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
            //Query for the ANOs and their values from the m_sCUStaticAttributeTable table.
            //For each record returned in that query, set the ANO value.
            ///''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

            oKeyObject = m_oApp.DataContext.OpenFeature(Convert.ToInt16(m_oComponents[m_componentName].Recordset.Fields["G3e_FNO"].Value), Convert.ToInt32(m_oComponents[m_componentName].Recordset.Fields["G3e_FID"].Value));

            string sSQL = string.Empty;
            Recordset oRS = null;
            int iDataType = 0;
            string sValue = null;
            int iValue = 0;
            string sField = null;
            int iCNO = 0;
            int iCurrentCNO = 0;
            string sExceptionMsg = null;
            Exception oEx = null;
            bool bNumericField = false;
            IGTComponent oComp = null;
            Recordset oCompRS = null;

            try
            {
                sSQL = @" select a.g3e_datatype,b.attr_key,a.g3e_field,a.g3e_cno from CULIB_ATTRIBUTE s,g3e_attributeinfo_optlang a, CULIB_UNITATTRIBUTE b
                    where s.g3e_fno=? and b.cu_id=? and a.g3e_ano=s.g3e_ano and s.category_C = b.category_c and s.ATTRIBUTE_ID = b.ATTRIBUTE_ID
                        order by a.g3e_cno";

                oRS = m_oDataContext.OpenRecordset(sSQL, ADODB.CursorTypeEnum.adOpenStatic, ADODB.LockTypeEnum.adLockReadOnly, (int)ADODB.CommandTypeEnum.adCmdText, oKeyObject.FNO, sCUCode);

                if (!(oRS.BOF && oRS.EOF))
                {
                    oRS.MoveFirst();
                    while (!oRS.EOF)
                    {
                        //Set the various attributes in the recordset.
                        iDataType = System.Convert.ToInt32(oRS.Fields["g3e_datatype"].Value);
                        sValue = oRS.Fields["attr_key"].Value.ToString();
                        sField = oRS.Fields["g3e_field"].Value.ToString();
                        iCNO = System.Convert.ToInt32(oRS.Fields["g3e_cno"].Value);

                        //The recordset is ordered by G3E_CNO.
                        //When that value changes, then determine whether
                        //to generate a new component record.
                        if (iCurrentCNO != iCNO)
                        {
                            //Prevent this branch of logic from executing
                            //until the next distinct G3E_CNO is encountered.
                            iCurrentCNO = iCNO;
                            //Do we need to generate a new record for this attribute
                            //If the CNO is the Primary CU Component, then do not create any additional records.
                            if (iCNO != m_iCompUnitCNO)
                            {
                                //If a component record does not exist, create one.
                                oComp = oKeyObject.Components.GetComponent((short)iCNO);
                                if (!(oComp == null))
                                {
                                    oCompRS = oComp.Recordset;
                                    if (!(oCompRS == null))
                                    {
                                        if (0 == oCompRS.RecordCount)
                                        {
                                            //No component record exists.  Create one.
                                            oCompRS.AddNew(System.Type.Missing, System.Type.Missing);
                                            oCompRS.Fields["g3e_fno"].Value = oKeyObject.FNO;
                                            oCompRS.Fields["g3e_fid"].Value = oKeyObject.FID;
                                        }
                                    }
                                }
                            }
                        }

                        oCompRS = null;
                        oComp = null;

                        if (1 <= iDataType & 7 >= iDataType)
                        {
                            bNumericField = true;
                            try
                            {
                                //Ensure the value given is indeed numeric.
                                iValue = System.Convert.ToInt32(sValue);
                            }
                            catch (Exception)
                            {
                                sExceptionMsg = "Attribute " + System.Convert.ToString(sField) + " cannot be set to " + System.Convert.ToString(sValue) + ". The value must be numeric.";
                                oEx = new Exception(System.Convert.ToString(sExceptionMsg));
                                throw (oEx);
                            }
                        }
                        else
                        {
                            bNumericField = false;
                        }

                        //Get the component for the selected attribute
                        oComp = oKeyObject.Components.GetComponent((short)iCNO);
                        if (!(oComp == null))
                        {
                            oCompRS = oComp.Recordset;
                            if (!(oCompRS == null))
                            {
                                if (!(oCompRS.BOF && oCompRS.EOF))
                                {

                                    //These are some hard-coded adjustments to certain attributes.
                                    if ((string)sValue == "NONE")
                                    {
                                        sValue = "";
                                    }

                                    //Set the attribute value on the G3E_CID of the current component
                                    //that matches the G3E_CID of the CU on the CU component.
                                    oCompRS.MoveFirst();
                                    while (!oCompRS.EOF)
                                    {
                                        if (iCUCID == System.Convert.ToInt32(oCompRS.Fields["g3e_cid"].Value))
                                        {
                                            if (bNumericField)
                                            {
                                                oCompRS.Fields[sField].Value = iValue;
                                            }
                                            else
                                            {
                                                oCompRS.Fields[sField].Value = sValue;
                                            }
                                            break;
                                        }
                                        oCompRS.MoveNext();
                                    }
                                    if (iCUCID > oCompRS.RecordCount)
                                    {
                                        //No component record exists.  Create one.
                                        oCompRS.AddNew(System.Type.Missing, System.Type.Missing);
                                        oCompRS.Fields["g3e_fno"].Value = oKeyObject.FNO;
                                        oCompRS.Fields["g3e_fid"].Value = oKeyObject.FID;
                                        oCompRS.MoveLast();
                                        if (bNumericField)
                                        {
                                            oCompRS.Fields[sField].Value = iValue;
                                        }
                                        else
                                        {
                                            oCompRS.Fields[sField].Value = sValue;
                                        }
                                    }
                                }
                            }
                        }
                        oRS.MoveNext();
                    }
                }
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }

        private bool IsRepeatingComponent(int p_CNO)
        {
            bool bReturn = false;
            ADODB.Recordset rs = m_oApp.DataContext.MetadataRecordset("G3E_FEATURECOMPS_OPTABLE", "g3e_cno = " + p_CNO);
           // rs.Filter = "g3e_cno = " + p_CNO;

            if (rs!=null)
            {
                if (rs.RecordCount>0)
                {
                    rs.MoveFirst();
                    bReturn = Convert.ToInt32(rs.Fields["G3E_REPEATING"].Value) ==1;
                }
            }           
            return bReturn;
        }

        /// <summary>
        /// Method to set the standard attributed on the Unit component
        /// </summary>
        /// <param name="sCUCode"></param>
        /// <param name="iCUCID"></param>
        /// <param name="m_iCompUnitCNO"></param>
        /// <param name="m_oDataContext"></param>
        /// <returns></returns>
        private int SetStaticAttrModifiedVersion(string sCUCode, int iCUCID, int m_iCompUnitCNO, IGTDataContext m_oDataContext)
        {
            IGTKeyObject oKeyObject;
            ///''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
            //Query for the ANOs and their values from the m_sCUStaticAttributeTable table.
            //For each record returned in that query, set the ANO value.
            ///''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

            oKeyObject = m_oApp.DataContext.OpenFeature(Convert.ToInt16(m_oComponents[m_componentName].Recordset.Fields["G3e_FNO"].Value), Convert.ToInt32(m_oComponents[m_componentName].Recordset.Fields["G3e_FID"].Value));

            string sSQL = string.Empty;
            Recordset oRS = null;
            int iDataType = 0;
            string sValue = null;
            int iValue = 0;
            string sField = null;
            int iCNO = 0;
            int iCurrentCNO = 0;
            string sExceptionMsg = null;
            Exception oEx = null;
            bool bNumericField = false;
            IGTComponent oComp = null;
            Recordset oCompRS = null;
            bool bFoundRecord = false;
            int iCreatedCID = 0;

            try
            {
                sSQL = @" select a.g3e_datatype,b.attr_key,a.g3e_field,a.g3e_cno from CULIB_ATTRIBUTE s,G3E_ATTRIBUTEINFO_OPTABLE a, CULIB_UNITATTRIBUTE b
                            where s.g3e_fno=? and b.cu_id=? and a.g3e_ano=s.g3e_ano and s.category_C = b.category_c and s.ATTRIBUTE_ID = b.ATTRIBUTE_ID
                            order by a.g3e_cno";

                oRS = m_oDataContext.OpenRecordset(sSQL, ADODB.CursorTypeEnum.adOpenStatic, ADODB.LockTypeEnum.adLockReadOnly, (int)ADODB.CommandTypeEnum.adCmdText, oKeyObject.FNO, sCUCode);

                if (!(oRS.BOF && oRS.EOF))
                {
                    oRS.MoveFirst();
                    while (!oRS.EOF)
                    {
                        //Set the various attributes in the recordset.
                        iDataType = System.Convert.ToInt32(oRS.Fields["g3e_datatype"].Value);
                        sValue = oRS.Fields["attr_key"].Value.ToString();
                        sField = oRS.Fields["g3e_field"].Value.ToString();
                        iCNO = System.Convert.ToInt32(oRS.Fields["g3e_cno"].Value);

                        //The recordset is ordered by G3E_CNO.
                        //When that value changes, then determine whether
                        //to generate a new component record.
                        if (iCurrentCNO != iCNO)
                        {
                            //Prevent this branch of logic from executing
                            //until the next distinct G3E_CNO is encountered.
                            iCurrentCNO = iCNO;
                            //Do we need to generate a new record for this attribute
                            //If the CNO is the Primary CU Component, then do not create any additional records.
                            if (iCNO != m_iCompUnitCNO)
                            {
                                //If a component record does not exist, create one.
                                oComp = oKeyObject.Components.GetComponent((short)iCNO);
                                if (!(oComp == null))
                                {
                                    oCompRS = oComp.Recordset;
                                    if (!(oCompRS == null))
                                    {
                                        if (0 == oCompRS.RecordCount)
                                        {
                                            //No component record exists.  Create one.
                                            oCompRS.AddNew(System.Type.Missing, System.Type.Missing);
                                            oCompRS.Fields["g3e_fno"].Value = oKeyObject.FNO;
                                            oCompRS.Fields["g3e_fid"].Value = oKeyObject.FID;
                                        }
                                    }
                                }
                            }
                        }

                        oCompRS = null;
                        oComp = null;

                        if (1 <= iDataType & 7 >= iDataType)
                        {
                            bNumericField = true;
                            try
                            {
                                //Ensure the value given is indeed numeric.
                                Decimal dTemp = Convert.ToDecimal(sValue);
                                iValue = System.Convert.ToInt32(dTemp);
                            }
                            catch (Exception)
                            {
                                sExceptionMsg = "Attribute " + System.Convert.ToString(sField) + " cannot be set to " + System.Convert.ToString(sValue) + ". The value must be numeric.";
                                oEx = new Exception(System.Convert.ToString(sExceptionMsg));
                                throw (oEx);
                            }
                        }
                        else
                        {
                            bNumericField = false;
                        }

                        //Get the component for the selected attribute
                        oComp = oKeyObject.Components.GetComponent((short)iCNO);
                        if (!(oComp == null))
                        {
                            oCompRS = oComp.Recordset;
                            if (!(oCompRS == null))
                            {
                                if (!(oCompRS.BOF && oCompRS.EOF))
                                {

                                    //These are some hard-coded adjustments to certain attributes.
                                    if ((string)sValue == "NONE")
                                    {
                                        sValue = "";
                                    }

                                    //Set the attribute value on the G3E_CID of the current component
                                    //that matches the G3E_CID of the CU on the CU component.

                                    oCompRS.MoveFirst();
                                    while (!oCompRS.EOF)
                                    {
                                        if (iCUCID == System.Convert.ToInt32(oCompRS.Fields["g3e_cid"].Value))
                                        {
                                            bFoundRecord = true;

                                            try
                                            {
                                                if (bNumericField)
                                                {
                                                    try
                                                    {
                                                        oCompRS.Fields[sField].Value = iValue;
                                                    }
                                                    catch (Exception)
                                                    {
                                                        
                                                    }                                                    
                                                }
                                                else
                                                {
                                                    try
                                                    {
                                                        oCompRS.Fields[sField].Value = sValue;
                                                    }
                                                    catch (Exception ex)
                                                    {
                                                        
                                                    }                                                  
                                                }
                                                break;
                                            }
                                            catch (Exception)
                                            {
                                              //  throw;
                                            }                                                                                  
                                        }
                                        oCompRS.MoveNext();
                                    }
                                    if (!bFoundRecord)
                                    {
                                        if (IsRepeatingComponent(iCNO) || (!IsRepeatingComponent(iCNO) && oCompRS.RecordCount==0))
                                        {
                                            //No component record exists.  Create one.
                                            oCompRS.AddNew(System.Type.Missing, System.Type.Missing);
                                            oCompRS.Fields["g3e_fno"].Value = oKeyObject.FNO;
                                            oCompRS.Fields["g3e_fid"].Value = oKeyObject.FID;
                                            iCreatedCID = Convert.ToInt32(oCompRS.Fields["g3e_cid"].Value);

                                            oCompRS.MoveLast();
                                            if (bNumericField)
                                            {
                                                try
                                                {
                                                    oCompRS.Fields[sField].Value = iValue;
                                                }
                                                catch (Exception)
                                                {

                                                }
                                            }
                                            else
                                            {
                                                try
                                                {
                                                    oCompRS.Fields[sField].Value = sValue;
                                                }
                                                catch (Exception)
                                                {
                                                }

                                            }
                                        }
                                        else if ((!IsRepeatingComponent(iCNO) && oCompRS.RecordCount == 1))
                                        {
                                            oCompRS.MoveFirst();

                                            if (bNumericField)
                                            {
                                                try
                                                {
                                                    oCompRS.Fields[sField].Value = iValue;
                                                }
                                                catch (Exception)
                                                {

                                                }
                                            }
                                            else
                                            {
                                                try
                                                {
                                                    oCompRS.Fields[sField].Value = sValue;
                                                }
                                                catch (Exception)
                                                {
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                        oRS.MoveNext();
                    }
                }
            }
            catch (Exception ex)
            {
                throw (ex);
            }
            return iCreatedCID;
        }
        #endregion
    }
}
