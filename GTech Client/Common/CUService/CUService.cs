
using System.Collections.Generic;
using System;
using Intergraph.GTechnology.API;
using System.Windows.Forms;
using System.Data;
using ADODB;
using System.Data.OleDb;

namespace GTechnology.Oncor.CustomAPI
{
    public class CUService
    {
        private const string M_SMSGBOXTITLE = "CUService";
        private const string M_SGENERALPARAMATERTABLE = "SYS_GENERALPARAMETER";
        private const string M_SCU = "CU"; //CU
        private const string M_SMCU = "MCU"; //Macro CU
        private const string M_SACU = "ACU"; //Ancillary CU
        private const string M_SAMCU = "AMCU"; //Ancillary Macro CU
        private IGTDataContext m_oDataContext = null;
        private IGTKeyObject m_oKeyObject = null;
        private string m_sCULibraryTable = null;
        private string m_sCUMacroLibraryTable = null;
        private int m_iCompUnitANO = 0;
        private string m_sCompUnitFieldName = null;
        private int m_iCompUnitMacroANO = 0;
        private string m_sCompUnitMacroFieldName = null;
        private int m_iCompUnitQtyANO = 0;
        private string m_sCompUnitQtyFieldName = null;
        private int m_iAncCompUnitANO = 0;
        private string m_sAncCompUnitFieldName = null;
        private int m_iAncCompUnitQtyANO = 0;
        private string m_sAncCompUnitQtyFieldName = null;
        private int m_iAncCompUnitMacroANO = 0;
        private string m_sAncCompUnitMacroFieldName = null;
        private int m_iCompUnitCNO = 0;
        private int m_iCompUnitMacroCNO = 0;
        private int m_iAncCompUnitCNO = 0;
        private int m_iAncCompUnitMacroCNO = 0;
        private string m_sCurrentPriCU = null;
        private string m_sSelectedCUType = string.Empty;
        private string m_sExistingCUType = string.Empty;
        private string m_sCuCategoryTable = string.Empty;
        private string m_ExistingUnitCID = string.Empty;
        private string m_ExistingUnitCNO = string.Empty;
        private string m_UnitCID = string.Empty;
        private string m_UnitCNO = string.Empty;


        //private string m_CuFieldName = string.Empty;
        //private short m_CNO = -1;
        private bool m_bSignificantAncillary = false;

        public CUService(int p_CUANO, bool p_SignificantAncillary)
        {
            m_iCompUnitANO = p_CUANO;
            m_bSignificantAncillary = p_SignificantAncillary;
        }
        public string CUField
        {
            get
            {
                return m_sCompUnitFieldName;
            }
        }

        public int CUComponentCNO
        {
            get
            {
                return m_iCompUnitCNO;
            }
        }
        public int AncillaryCUComponentCNO
        {
            get
            {
                return m_iAncCompUnitCNO;
            }
        }
        public IGTDataContext DataContext
        {
            set
            {
                m_oDataContext = value;
                InitializeGeneralParameters();
            }
        }
        /// <summary>
        /// Sets the GTKeyObject in the CUService.
        /// </summary>
        /// <value>
        /// GTKeyobject
        /// </value>
        /// <returns></returns>
        /// <remarks>
        /// Contains the feature for which the CU information will be manipulated by this service.
        /// </remarks>
        public IGTKeyObject KeyObject
        {
            get
            {
                return m_oKeyObject;
            }
            set
            {
                m_oKeyObject = value;
            }
        }

        public string CurrentPrimaryCU
        {
            set
            {
                m_sCurrentPriCU = value;
            }
        }
        /// <summary>
        /// Returns the G3E_ANO of the Primary CU attribute.
        /// </summary>
        /// <value>
        /// Integer
        /// </value>
        /// <remarks>
        /// None.
        /// </remarks>
        public int CompUnitANO
        {
            get
            {
                return m_iCompUnitANO;
            }
        }
        /// <summary>
        /// Returns the G3E_ANO of the Primary Macro CU attribute.
        /// </summary>
        /// <value>
        /// Integer
        /// </value>
        /// <remarks>
        /// None.
        /// </remarks>
        public int CompUnitMacroANO
        {
            get
            {
                return m_iCompUnitMacroANO;
            }
        }
        /// <summary>
        /// Returns the G3E_ANO of the Ancillary CU attribute.
        /// </summary>
        /// <value>
        /// Integer
        /// </value>
        /// <remarks>
        /// None.
        /// </remarks>
        public int AncCompUnitANO
        {
            get
            {
                return m_iAncCompUnitANO;
            }
        }
        /// <summary>
        /// Returns the G3E_ANO of the Ancillary Macro CU attribute.
        /// </summary>
        /// <value>
        /// Integer
        /// </value>
        /// <remarks>
        /// None.
        /// </remarks>
        public int AncCompUnitMacroANO
        {
            get
            {
                return m_iAncCompUnitMacroANO;
            }
        }
        /// <summary>
        /// Returns the G3E_ANO of the Primary CU Quantity attribute.
        /// </summary>
        /// <value>
        /// Integer
        /// </value>
        /// <remarks>
        /// None.
        /// </remarks>
        public int CompUnitQtyANO
        {
            get
            {
                return m_iCompUnitQtyANO;
            }
        }
        /// <summary>
        /// Returns the G3E_ANO of the Ancillary CU Quantity attribute.
        /// </summary>
        /// <value>
        /// Integer
        /// </value>
        /// <remarks>
        /// None.
        /// </remarks>
        public int AncCompUnitQtyANO
        {
            get
            {
                return m_iAncCompUnitQtyANO;
            }
        }

        public void Add(string CUCode, bool Primary, string CUQty, string CUActivity, string AMCUCode = null)
        {
            CheckDataContext();
            AddCU(CUCode, Primary, CUActivity,m_sExistingCUType, CUQty, AMCUCode);
        }
        private void AddCU(string sCUCode, bool bPrimary, string sCUActivit, string sCUTypeSelected = null,string sCUQty = null, string AMCUCode = null)
        {
            IGTComponent oComponent = null;
            ADODB.Recordset oRS = null;
            Exception oEx = null;
            string sCUType = null;
            object[] sCUArr = null;
            object[] sCUQtyArr = null;
            object[] sCUActivitArr = null;
            object[] sAMCUArr = null;
            string sExMsg = null;
            string sCU = null;
            int iCNO = 0;
            string sCUFieldName = null;
            string sMCUFieldName = null;
            short iCUQty = System.Convert.ToInt16(null);
            short iCurrentCU = System.Convert.ToInt16(null);
            int iCurrentCID = 0;
            bool bHaveQty = System.Convert.ToBoolean(null);
            bool bHaveActivit = System.Convert.ToBoolean(null);
            int iQty = 0;
            string strActivity = null;
            object[] sCUTypeArr = null;
            string sAMCU = null;

            try
            {

                CheckDataContext();

                //CUCode (and CUQty) can be a comma-separated string of CU values.
                //Process each one individually.

                //It is assumed that if sCUQty is given, the position of each quantity
                //corresponds with the same position for each CU. This is particularly useful for the Ancillary CU

                sCUArr = sCUCode.Split(',');
                if (m_sSelectedCUType != null)
                {
                    sCUTypeArr = m_sSelectedCUType.Split(',');
                }

                bHaveQty = !(sCUQty == null);
                if (bHaveQty)
                {
                    sCUQtyArr = sCUQty.Split(',');
                }

                bHaveActivit = !(sCUActivit == null);
                if (bHaveActivit)
                {
                    sCUActivitArr = sCUActivit.Split(',');
                }

                if (!string.IsNullOrEmpty(AMCUCode))
                {
                    sAMCUArr = AMCUCode.Split(',');
                }

                for (int i = 0; i <= sCUArr.GetLength(0) - 1; i++)
                {
                    //Get the indicated CU in the CU Array
                    //and that CU's quantity.
                    sCU = sCUArr[i].ToString().Trim();


                    if (!string.IsNullOrEmpty(AMCUCode))
                    {
                        sAMCU = sAMCUArr[i].ToString().Trim();
                    }

                    if (bHaveQty)
                    {
                        iQty = System.Convert.ToInt32(sCUQtyArr[i].ToString().Trim());
                    }
                    else
                    {
                        iQty = 1;
                    }


                    if (bHaveActivit)
                    {
                        strActivity = sCUActivitArr[i].ToString().Trim();
                    }
                    else
                    {
                        strActivity = "I";
                    }

                    //When a CU is "Add"ed, look in all the GTKeyObjects for any possible
                    //features for which that CU will apply.  If the CU applies, check to
                    //see if the recordset on the CU component has any records.  If it doesn't,
                    //add one and set the CU code on that component.  If the CU component recordset
                    //exists, then simply set the CU code.

                    //If the Primary flag is True, then also perform any static attribute setting
                    //that might apply to the feature/CU combination.

                    //Which CU component
                    sCUType = m_sSelectedCUType;

                    if (sCUTypeArr != null)
                    {
                        sCUType = sCUTypeArr[i].ToString().Trim();
                    }

                    if (!string.IsNullOrEmpty(Convert.ToString(sCUType)))
                    {

                        //Ensure the CU types stored in the CU Tables agree with the Primary flag
                        // and determine the field name to set.
                        if (((string)sCUType == M_SCU))
                        {
                            sCUFieldName = m_sCompUnitFieldName;
                            sMCUFieldName = m_sCompUnitMacroFieldName;
                            if (bPrimary)
                            {
                                iCNO = System.Convert.ToInt32(m_iCompUnitCNO);
                            }
                            else
                            {
                                sExMsg = "CU mapping for CU " + System.Convert.ToString(sCU) + " defines it as a Primary CU for G3E_FNO " + System.Convert.ToString(m_oKeyObject.FNO) + " but it was passed to the CUService.Add() method as a non-Primary CU.";
                                oEx = new Exception(System.Convert.ToString(sExMsg));
                                throw (oEx);
                            }
                        }
                        else if ((string)sCUType == M_SMCU)
                        {
                            sCUFieldName = m_sCompUnitMacroFieldName;
                            sMCUFieldName = m_sCompUnitMacroFieldName;
                            if (bPrimary)
                            {
                                iCNO = System.Convert.ToInt32(m_iCompUnitMacroCNO);
                            }
                            else
                            {
                                sExMsg = "CU mapping for Macro CU " + System.Convert.ToString(sCU) + " defines it as a Primary Macro CU for G3E_FNO " + System.Convert.ToString(m_oKeyObject.FNO) + " but it was passed to the CUService.Add() method as a non-Primary CU.";
                                oEx = new Exception(System.Convert.ToString(sExMsg));
                                throw (oEx);
                            }
                        }
                        else if ((string)sCUType == M_SACU)
                        {
                            sCUFieldName = m_sAncCompUnitFieldName;
                            sMCUFieldName = m_sAncCompUnitMacroFieldName;
                            if (!bPrimary)
                            {
                                iCNO = System.Convert.ToInt32(m_iAncCompUnitCNO);

                            }
                            else
                            {
                                sExMsg = "CU mapping for Ancillary CU " + System.Convert.ToString(sCU) + " defines it as an Ancillary CU for G3E_FNO " + System.Convert.ToString(m_oKeyObject.FNO) + " but it was passed to the CUService.Add() method as a Primary CU.";
                                oEx = new Exception(System.Convert.ToString(sExMsg));
                                throw (oEx);
                            }
                        }
                        else if ((string)sCUType == M_SAMCU)
                        {
                            sCUFieldName = m_sAncCompUnitMacroFieldName;
                            sMCUFieldName = m_sAncCompUnitMacroFieldName;
                            if (!bPrimary)
                            {
                                iCNO = System.Convert.ToInt32(m_iAncCompUnitMacroCNO);

                            }
                            else
                            {
                                sExMsg = "CU mapping for Ancillary Macro CU " + System.Convert.ToString(sCU) + " defines it as an Ancillary Macro CU for G3E_FNO " + System.Convert.ToString(m_oKeyObject.FNO) + " but it was passed to the CUService.Add() method as a Primary CU.";
                                oEx = new Exception(System.Convert.ToString(sExMsg));
                                throw (oEx);
                            }
                        }
                        else
                        {
                            sCUFieldName = null;
                        }

                        if (!string.IsNullOrEmpty(sCUFieldName))
                        {
                            oComponent = m_oKeyObject.Components.GetComponent((short)iCNO);

                            //Does this feature have a CU/ACU component
                            if (!(oComponent == null))
                            {
                                //If this is a macro CU, then we need to generate
                                //the appropriate number of CU records by expanding it.

                                if ((string)sCUType == M_SMCU || (string)sCUType == M_SAMCU)
                                {
                                    if (bPrimary)
                                    {
                                        for (int iCnt = 0; iCnt <= iQty - 1; iCnt++)
                                        {
                                            ExpandMacro(sCU, sCUType, 1, m_oKeyObject, strActivity);
                                        }
                                    }
                                    else
                                    {
                                        ExpandMacro(sCU, sCUType, iQty, m_oKeyObject, strActivity);
                                    }
                                }
                                else
                                {
                                    //Create/update cu records as needed.
                                    oRS = oComponent.Recordset;
                                    oRS.Filter = "";

                                    if (!(bPrimary))
                                    {
                                        if (iQty != 0)
                                        {
                                            GenerateCURecord(oRS, bPrimary, iCNO, null);
                                            iCurrentCID = System.Convert.ToInt32(oRS.Fields["g3e_cid"].Value);
                                            SetDefaultCUFieldInfo(m_oKeyObject, bPrimary, sCUType, iCNO, sCU, sCUFieldName, null, sMCUFieldName, iQty, strActivity, sAMCU);
                                        }
                                    }
                                    else
                                    {
                                        //This loop is not needed for ONCOR as Primary CU quantity will always be 1, will remove it later
                                        for (int iCnt = 0; iCnt <= iQty - 1; iCnt++)
                                        {
                                            GenerateCURecord(oRS, bPrimary, iCNO, null);
                                            iCurrentCID = System.Convert.ToInt32(oRS.Fields["g3e_cid"].Value);
                                            SetDefaultCUFieldInfo(m_oKeyObject, bPrimary, sCUType, iCNO, sCU, sCUFieldName, null, sMCUFieldName, 1, strActivity);

                                        }
                                    }
                                } //If sCUType = M_SMCU Or sCUType = M_SAMCU Then
                            } //If Not IsNothing(oComponent)
                        } //If vbNullString <> sCUFieldName
                    } //If vbNullString <> sCUType Then
                } //For i As Integer = 0 To sCUArr.GetLength(0) - 

            }
            catch (Exception ex)
            {
                oEx = new System.Exception("AddCU - " + ex.Message);
                throw (oEx);
            }

        }
        private void ReplaceCU(string CUActivity,string sExistingCUCode, string sNewCUCode, bool bPrimary, short iOccurrence, bool bUserCancel = false, string iMacroQty = "1", string sAMCU = null)
        {

            string sExistingCUType = null;
            string sNewCUType = null;
            System.Exception oEx = null;
            IGTComponent oComp = null;
            ADODB.Recordset oRS = null;
            string sExistingMacroCUCode = null;
            string sMsg = null;

            string s1CUcode = "";
            string[] sArrMCUqty = null;
            int i = 0;
          
            object[] sAMCUArr = null;
            string sAMCOCode = string.Empty;

            try
            {
                //Get the CU Types.
                if (!(string.IsNullOrEmpty(sExistingCUCode)))
                {
                    sExistingCUType = m_sExistingCUType; //ONCOR - We already stored the existing CU type
                }
                else
                {
                    sExistingCUType = null;
                }

                if (!string.IsNullOrEmpty(sAMCU))
                {
                    sAMCUArr = sAMCU.Split(',');
                }


                sNewCUType = m_sSelectedCUType;

                if ((sNewCUCode.Split(',').Length > 1) && (sExistingCUType == null || (string)sExistingCUType == M_SCU || (string)sExistingCUType == M_SMCU) && ((string)sNewCUType == M_SCU || (string)sNewCUType == M_SMCU))
                {
                    oEx = new System.Exception("Please select only one CU on Replace");
                    throw (oEx);
                }

                if (((string)sExistingCUType == M_SCU) || ((string)sExistingCUType == M_SMCU))
                {
                    if (!bPrimary)
                    {
                        oEx = new System.Exception("CU - " + System.Convert.ToString(sExistingCUCode) + " - is defined as a Primary CU/MCU for this feature but was passed to the Replace() method as an Ancillary CU/MCU.");
                        throw (oEx);
                    }
                }
                else if (((string)sExistingCUType == M_SACU) || ((string)sExistingCUType == M_SAMCU))
                {
                    if (bPrimary)
                    {
                        oEx = new System.Exception("CU - " + System.Convert.ToString(sExistingCUCode) + " - is defined as an Ancillary CU/MCU for this feature but was passed to the Replace() method as a Primary CU/MCU.");
                        throw (oEx);
                    }
                }

                //If the existing CU's type is M_SCU or M_SACU and the occurrence's macro CU field is not empty,
                //then the user must decide whether to drop the existing macro CU and add the CU or cancel the operation.
                if ((string)sExistingCUType == M_SCU || (string)sExistingCUType == M_SACU)
                {
                    oComp = m_oKeyObject.Components.GetComponent(System.Convert.ToInt16(bPrimary ? m_iCompUnitCNO : m_iAncCompUnitCNO));
                    if (!(oComp == null))
                    {
                        oRS = oComp.Recordset;
                        if (!(oRS == null))
                        {
                            //Filter down to just the one G3E_CID value
                            oRS.Filter = "g3e_cid=" + iOccurrence.ToString();
                            if (!(oRS.BOF && oRS.EOF))
                            {
                                if (!m_bSignificantAncillary)
                                {
                                    if (1 == oRS.RecordCount)
                                    {
                                        //We have the one record we're looking for.  Check its macro CU field.
                                        if (!FieldIsEmpty(oRS.Fields[(bPrimary ? m_sCompUnitMacroFieldName : m_sAncCompUnitMacroFieldName)]))
                                        {
                                            //The macro CU has data.  Assume it contains a valid macro CU value.
                                            //Warn the user that if the CU is replaced, macro will be deleted first
                                            //and the CU will be added (not a direct replacement as such).
                                            sExistingMacroCUCode = oRS.Fields[(bPrimary ? m_sCompUnitMacroFieldName : m_sAncCompUnitMacroFieldName)].Value.ToString();

                                            sMsg = "A CU that is part of a Macro CU definition cannot be replaced.";
                                            sMsg = sMsg + "\n" + "\n" + "Replacing CU \"" + System.Convert.ToString(sExistingCUCode) + "\" with \"" + System.Convert.ToString(sNewCUCode) + "";
                                            sMsg = sMsg + "\n" + " will result in the deletion of Macro CU \"" + System.Convert.ToString(sExistingMacroCUCode) + "\"";
                                            sMsg = sMsg + "\n" + "and the CU code \"" + System.Convert.ToString(sNewCUCode) + "\" will be added as a separate, new record.";
                                            sMsg = sMsg + "\n" + "\n" + "Do you wish to continue?";
                                            DialogResult drOutput = MessageBox.Show(sMsg, "Replace Macro CU Warning", MessageBoxButtons.OKCancel);

                                            switch (drOutput)
                                            {
                                                case DialogResult.Cancel:
                                                    bUserCancel = true;
                                                    break;
                                                case DialogResult.OK:
                                                    //If the user wants to replace a Macro with a non-Macro CU,
                                                    //then delete the macro and add the CU.
                                                    DeleteCU(sExistingMacroCUCode, bPrimary, iOccurrence);

                                                    sArrMCUqty = iMacroQty.Split(',');
                                                    if (sAMCUArr != null)
                                                    {
                                                        sAMCOCode = sAMCUArr[i].ToString();
                                                    }

                                                    foreach (string tempLoopVar_s1CUcode in sNewCUCode.Split(','))
                                                    {
                                                        s1CUcode = tempLoopVar_s1CUcode;
                                                        if (sAMCUArr != null)
                                                        {
                                                            sAMCOCode = sAMCUArr[i].ToString();
                                                        }
                                                        AddCU(s1CUcode, bPrimary, CUActivity, sCUQty: sArrMCUqty[i].ToString(), AMCUCode: sAMCOCode);
                                                        i++;
                                                    }
                                                    //'  Replace is complete, set the flag
                                                    bUserCancel = true;
                                                    break;

                                            }

                                        }
                                    }
                                }
                            }
                        }
                    }
                }

                //If we are in a situation where we're replacing a MacroCU with a CU
                //but the user elected to cancel that operation, then we don't want
                //to process the rest of this code.
                if (!bUserCancel)
                {

                    if ((string)sExistingCUType == M_SMCU || (string)sExistingCUType == M_SAMCU || (string)sNewCUType == M_SMCU || (string)sNewCUType == M_SAMCU)
                    {
                        //If either the existing CU or new CU is a macro,
                        //then the process will be to delete the existing CU
                        //and add the new one.

                        //If the existing record contains a CU but no Macro CU (the case when the macro CU field
                        //is selected in Feature Explorer and there's an existing CU), then use the CU
                        //as the existing CU to delete.
                        oComp = m_oKeyObject.Components.GetComponent(System.Convert.ToInt16(bPrimary ? m_iCompUnitCNO : m_iAncCompUnitCNO));
                        if (!(oComp == null))
                        {
                            oRS = oComp.Recordset;
                            if (!(oRS == null))
                            {
                                //Filter down to just the one G3E_CID value
                                oRS.Filter = "g3e_cid=" + iOccurrence.ToString();
                                if (!(oRS.BOF && oRS.EOF))
                                {
                                    if (1 == oRS.RecordCount)
                                    {
                                        //We have the one record we're looking for.  Check its CU field.
                                        //We have the one record we're looking for.  If the macro field is empty and the CU field is not empty, then check its CU field.
                                        if (FieldIsEmpty(oRS.Fields[(bPrimary ? m_sCompUnitMacroFieldName : m_sAncCompUnitMacroFieldName)]) && !FieldIsEmpty(oRS.Fields[(bPrimary ? m_sCompUnitFieldName : m_sAncCompUnitFieldName)]))
                                        {
                                            sExistingCUCode = oRS.Fields[(bPrimary ? m_sCompUnitFieldName : m_sAncCompUnitFieldName)].Value.ToString();
                                        }
                                        //    End If
                                    }
                                }
                            }
                        }
                        DeleteCU(sExistingCUCode, bPrimary, iOccurrence);

                        sArrMCUqty = iMacroQty.Split(',');
                      

                        foreach (string tempLoopVar_s1CUcode in sNewCUCode.Split(','))
                        {
                            s1CUcode = tempLoopVar_s1CUcode;
                            if (sAMCUArr!=null)
                            {
                                sAMCOCode = sAMCUArr[i].ToString();
                            }
                            AddCU(s1CUcode, bPrimary, CUActivity, sCUQty: sArrMCUqty[i].ToString(),AMCUCode: sAMCOCode);
                            i++;
                        }
                    }

                    if ((sNewCUCode.Split(',').Length > 1) && ((string)sExistingCUType == M_SACU || (string)sNewCUType == M_SACU))
                    {
                        DeleteCU(sExistingCUCode, bPrimary, iOccurrence);

                        sArrMCUqty = iMacroQty.Split(',');
                        foreach (string tempLoopVar_s1CUcode in sNewCUCode.Split(','))
                        {
                            s1CUcode = tempLoopVar_s1CUcode;
                            AddCU(s1CUcode, bPrimary, CUActivity, sCUQty: sArrMCUqty[i].ToString(), AMCUCode: sAMCUArr[i].ToString());
                            i++;
                        }

                    }

                    if ((sExistingCUType == null && sNewCUCode.Split(',').Length == 1 && ((string)sNewCUType == M_SCU || (string)sNewCUType == M_SACU)) ||
                        ((string)sExistingCUType == M_SCU && (string)sNewCUType == M_SCU && sNewCUCode.Split(',').Length == 1) ||
                        ((string)sExistingCUType == M_SACU && (string)sNewCUType == M_SACU && sNewCUCode.Split(',').Length == 1))
                    {
                        //For CU and ACUs, we will simply replace the attribute value at the specifiec G3E_CID
                        //and, in the case of primary CUs, update the static attributes for it.
                        oComp = m_oKeyObject.Components.GetComponent(System.Convert.ToInt16(bPrimary ? m_iCompUnitCNO : m_iAncCompUnitCNO));
                        if (!(oComp == null))
                        {
                            oRS = oComp.Recordset;
                            if (!(oRS == null))
                            {
                                //Filter down to just the one G3E_CID value
                                oRS.Filter = "g3e_cid=" + iOccurrence.ToString();
                                if (!(oRS.BOF && oRS.EOF))
                                {
                                    oRS.MoveFirst();
                                    if (1 == oRS.RecordCount)
                                    {
                                        //We have the record we want.  Set the new field value.
                                        if (bPrimary)
                                        {
                                            oRS.Fields[m_sCompUnitFieldName].Value = sNewCUCode;
                                        }
                                        else
                                        {
                                            oRS.Fields[m_sAncCompUnitFieldName].Value = sNewCUCode;
                                        }
                                        // set other attributes on the CU Component, if the Old CU was nothing...
                                        //    ie. if first record created by G/Tech & no other attributes are set. - YN/May-2010
                                        if (sExistingCUType == null)
                                        {
                                            SetDefaultCUFieldInfo(m_oKeyObject, bPrimary, sNewCUType, System.Convert.ToInt32(m_iCompUnitCNO),
                                            sNewCUCode, m_sCompUnitFieldName, null, m_sCompUnitMacroFieldName, System.Convert.ToInt32(("1")), CUActivity);
                                        }
                                        else //Take care of setting attributes corresponding to CU as it is removed from FI now
                                        {
                                            string sComponentName = Convert.ToInt32(oRS.Fields["g3e_cno"].Value) == 21 ? "COMP_UNIT_N" : "COMP_UNIT_ANCIL_N";

                                            SetCUAttributes(m_oKeyObject.Components, sComponentName, iOccurrence);
                                        }
                                        oRS.Filter = string.Empty;
                                    }
                                    else
                                    {
                                        oEx = new System.Exception("Did not get a distinct record for CU component recordset where G3E_CID = " + iOccurrence.ToString() + ".");
                                        throw (oEx);
                                    }
                                }
                                else
                                {
                                    oEx = new System.Exception("Recordset was empty for component " + System.Convert.ToString(bPrimary ? m_iCompUnitCNO.ToString() : m_iAncCompUnitCNO.ToString()));
                                    throw (oEx);
                                }
                            }
                            else
                            {
                                oEx = new System.Exception("Failed to get recordset for component " + System.Convert.ToString(bPrimary ? m_iCompUnitCNO.ToString() : m_iAncCompUnitCNO.ToString()));
                                throw (oEx);
                            }
                        }
                        else
                        {
                            oEx = new System.Exception("Failed to get component " + System.Convert.ToString(bPrimary ? m_iCompUnitCNO.ToString() : m_iAncCompUnitCNO.ToString()));
                            throw (oEx);
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                oEx = new System.Exception("Error in ReplaceCU method: " + ex.Message);
                throw (oEx);
            }
        }

        /// <summary>
        /// The method will return of Picklist table data corresponding to argument iPNO
        /// </summary>
        /// <param name="iPNO"></param>
        /// <returns></returns>
        private List<PickiListData> GetPickList(int iPNO)
        {
            List<PickiListData> lstReturn = new List<PickiListData>();
            string sPickListTableName = string.Empty;
            Dictionary<string, List<PickiListData>> dicPickListData = new Dictionary<string, List<PickiListData>>();

            try
            {
                ADODB.Recordset rsPiclickist = m_oDataContext.MetadataRecordset("G3E_PICKLIST_OPTABLE");
                rsPiclickist.Filter = "g3e_pno = " + iPNO;

                string sKeyField = string.Empty;
                string sValueField = string.Empty;

                if (rsPiclickist != null)
                {
                    if (rsPiclickist.RecordCount > 0)
                    {
                        rsPiclickist.MoveFirst();
                        sPickListTableName = Convert.ToString(rsPiclickist.Fields["G3E_TABLE"].Value);
                        sKeyField = Convert.ToString(rsPiclickist.Fields["G3E_KEYFIELD"].Value);
                        sValueField = Convert.ToString(rsPiclickist.Fields["G3E_VALUEFIELD"].Value);
                    }
                }

                string sSql = "select " + sKeyField + " KeyField," + sValueField + " ValueField from " + sPickListTableName;
                rsPiclickist = m_oDataContext.OpenRecordset(sSql, CursorTypeEnum.adOpenStatic, LockTypeEnum.adLockReadOnly, (int)ADODB.CommandTypeEnum.adCmdText, new object[1]);

                if (rsPiclickist != null)
                {
                    if (rsPiclickist.RecordCount > 0)
                    {
                        rsPiclickist.MoveFirst();
                        while (rsPiclickist.EOF == false)
                        {
                            PickiListData obj = new PickiListData(Convert.ToString(rsPiclickist.Fields["KeyField"].Value), Convert.ToString(rsPiclickist.Fields["ValueField"].Value));
                            lstReturn.Add(obj);
                            rsPiclickist.MoveNext();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return lstReturn;
        }

        private void GetPickListData(Dictionary<string, List<PickiListData>> p_dicPickListMapping, string p_sCategoryFilter)
        {
            string[] arrCategory = p_sCategoryFilter.Split(',');
            List<PickiListData> lstCategoryData = new List<PickiListData>();

            try
            {
                for (int i = 0; i < arrCategory.Length; i++)
                {
                    PickiListData objPickList = new PickiListData(arrCategory[i], arrCategory[i]);
                    lstCategoryData.Add(objPickList);
                }

                p_dicPickListMapping.Add("Category", lstCategoryData);
                List<PickiListData> lstCuTypeData = new List<PickiListData>();

                PickiListData obj2 = new PickiListData("C", "C");
                PickiListData obj1 = new PickiListData("M", "M");
                lstCuTypeData.Add(obj1);
                lstCuTypeData.Add(obj2);
                p_dicPickListMapping.Add("M/C", lstCuTypeData);
            }

            catch (Exception ex)
            {
                throw ex;
            }
        }

        private string[] GetAllAncillaryCategories(string p_filter)
        {
            string[] sreturnCategories = null;
            List<string> CategoriesAncillaries = new List<string>();

            try
            {
                string sql = string.IsNullOrEmpty(p_filter) ? "select distinct CATEGORY_C from V_CUSELECTION_ACU_VL" : "select distinct CATEGORY_C from V_CUSELECTION_ACU_VL where CATEGORY_C = '" + p_filter + "'";

                ADODB.Recordset rs = m_oDataContext.OpenRecordset("select distinct CATEGORY_C from V_CUSELECTION_ACU_VL", CursorTypeEnum.adOpenDynamic, LockTypeEnum.adLockOptimistic, (int)ADODB.CommandTypeEnum.adCmdText, new object[1]);
                if (rs != null)
                {
                    if (rs.RecordCount > 0)
                    {
                        while (rs.EOF == false)
                        {
                            CategoriesAncillaries.Add(Convert.ToString(rs.Fields[0].Value));
                            rs.MoveNext();
                        }
                    }
                }
                sreturnCategories = CategoriesAncillaries.ToArray();
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return sreturnCategories;
        }
        private List<CUDataModel> GetAllCUData(int iFNO, bool bPrimary, string sCategoryFilter, string sFilterField, string sFilterValue, out Dictionary<string, List<PickiListData>> sPickListData)
        {
            List<CUDataModel> returnData = new List<CUDataModel>();
            sPickListData = null;

            try
            {
                string[] sArrCategories = null;

                if (string.IsNullOrEmpty(sCategoryFilter) && !bPrimary)
                {
                    sArrCategories = GetAllAncillaryCategories(sCategoryFilter);
                }
                else
                {
                    sArrCategories = sCategoryFilter.Split(',');
                }

                for (int i = 0; i < sArrCategories.Length; i++)
                {
                    CUDataModel obj = new CUDataModel();
                    obj.CUDataWithStandardAttributes = GetCUList(iFNO, bPrimary, sArrCategories[i], sFilterField, sFilterValue, out sPickListData);
                    obj.Category = sArrCategories[i].ToString();
                    returnData.Add(obj);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return returnData;
        }
        private List<PickiListData> GetAllCUCategories(string sCategoryFilter)
        {
            List<PickiListData> lReturnCategoriesList = new List<PickiListData>();

            try
            {
                string[] sArrCategories = sCategoryFilter.Split(',');

                for (int i = 0; i < sArrCategories.Length; i++)
                {
                    PickiListData obj = new PickiListData(sArrCategories[i], sArrCategories[i]);
                    lReturnCategoriesList.Add(obj);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return lReturnCategoriesList;
        }

        private string ReturnNullStrings(int p_count)
        {
            string sNullString = string.Empty;

            try
            {
                for (int i = 0; i < p_count; i++)
                {
                    sNullString = ",null" + sNullString;

                }
                if (!string.IsNullOrEmpty(sNullString))
                {
                    sNullString = sNullString.Remove(0, 1);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return sNullString;
        }
        private string GetCategoryType(string p_Cateogry)
        {
            string sReturn = string.Empty;
            ADODB.Recordset rs = m_oDataContext.OpenRecordset("select type_C from culib_category where category_c =?", CursorTypeEnum.adOpenDynamic, LockTypeEnum.adLockOptimistic, (int)ADODB.CommandTypeEnum.adCmdText, p_Cateogry);
            if (rs!=null)
            {
                if (rs.RecordCount>0)
                {
                    rs.MoveFirst();
                    sReturn = Convert.ToString(rs.Fields["TYPE_C"].Value);
                }
            }
            return sReturn;

        }
        private DataTable GetCUList(int iFNO, bool bPrimary, string sCategoryFilter, string sFilterField, string sFilterValue, out Dictionary<string, List<PickiListData>> sPickListData)
        {
            DataTable Result = new DataTable();
            Recordset oRS = null;
            String sSQL = string.Empty;
            String sFilter = string.Empty;

            Dictionary<string, string> dColumnNames = new Dictionary<string, string>();
            Dictionary<string, List<PickiListData>> dicPickListMapping = new Dictionary<string, List<PickiListData>>();

            try
            {
                List<string> lStaticAttributesUsernames = new List<string>();
                List<string> lstColumnNames = new List<string>();
                string sColumnNames = string.Empty;

                string sFirstCategorySelected = string.Empty;
                sFirstCategorySelected = sCategoryFilter;
                string sNullValues = string.Empty;

                if (sCategoryFilter.Contains(","))
                {
                    sFirstCategorySelected = sCategoryFilter.Split(',')[0];
                }

                sPickListData = null;

                //Collect the static attributes irrespective of the fact whether the CU is primary or Ancillary as there is a possibility
                //an Ancillary CU can point to static attributes for the unit components
                if (!sCategoryFilter.Contains(",") && bPrimary)
                {
                    Recordset rsStaticAttrbutes = null;
                    string sCategoryType = GetCategoryType(sCategoryFilter);
                    string sSQl = string.Empty;

                    if (sCategoryType.Equals("M"))
                    {
                        sSQl = @"select distinct ga.G3E_USERNAME, ga.G3E_PNO
                                from CULIB_MACRO m
                                join CULIB_MACROUNIT mu on m.MU_ID = mu.MU_ID
                                join CULIB_UNITATTRIBUTE ua on mu.CU_ID = ua.CU_ID
                                join CULIB_ATTRIBUTE a  on (ua.CATEGORY_C = a.CATEGORY_C and ua.ATTRIBUTE_ID = a.ATTRIBUTE_ID)
                                join G3E_ATTRIBUTEINFO_OPTABLE  ga on a.G3E_ANO = ga.G3E_ANO
                                where m.CATEGORY_C = ?
                                and mu.PRIMARY_YN = 'Y'
                                order by 1,2";                       
                    }
                    else
                    {
                        sSQl = @"select distinct ga.G3E_USERNAME, ga.G3E_PNO
                                from CULIB_ATTRIBUTE            a
                                join G3E_ATTRIBUTEINFO_OPTABLE  ga on a.G3E_ANO = ga.G3E_ANO
                                where a.CATEGORY_C = ?
                                order by 1,2";

                    }

                    //rsStaticAttrbutes = m_oDataContext.OpenRecordset("select g3e_username, g3e_pno from G3E_ATTRIBUTEINFO_OPTABLE a, CULIB_ATTRIBUTE b where a.g3e_ano = b.g3e_ano and  g3e_fno = ? and b.category_C = '" + sCategoryFilter + "' order by ordinal", ADODB.CursorTypeEnum.adOpenStatic, ADODB.LockTypeEnum.adLockReadOnly, (System.Int32)ADODB.CommandTypeEnum.adCmdText, iFNO);
                    rsStaticAttrbutes = m_oDataContext.OpenRecordset(sSQl, ADODB.CursorTypeEnum.adOpenStatic, ADODB.LockTypeEnum.adLockReadOnly, (System.Int32)ADODB.CommandTypeEnum.adCmdText, sCategoryFilter);

                    if (rsStaticAttrbutes != null)
                    {
                        if (rsStaticAttrbutes.RecordCount > 0)
                        {
                            rsStaticAttrbutes.MoveFirst();
                            while (!rsStaticAttrbutes.EOF)
                            {
                                lStaticAttributesUsernames.Add("'" + Convert.ToString(rsStaticAttrbutes.Fields["g3e_username"].Value) + "'");
                                lstColumnNames.Add(" '" + Convert.ToString(rsStaticAttrbutes.Fields["g3e_username"].Value) + "' as " + "\"" + Convert.ToString(rsStaticAttrbutes.Fields["g3e_username"].Value).Replace(" ", "") + "\"");

                                if (Convert.ToString(rsStaticAttrbutes.Fields["g3e_username"].Value).Contains(" "))
                                {
                                    dColumnNames.Add(Convert.ToString(rsStaticAttrbutes.Fields["g3e_username"].Value).Replace(" ", ""), Convert.ToString(rsStaticAttrbutes.Fields["g3e_username"].Value));
                                }
                                if (Convert.ToString(rsStaticAttrbutes.Fields["g3e_pno"].Value) != "" && rsStaticAttrbutes.Fields["g3e_pno"].Value != DBNull.Value && rsStaticAttrbutes.Fields["g3e_pno"].Value != null)
                                {
                                    dicPickListMapping.Add(Convert.ToString(rsStaticAttrbutes.Fields["g3e_username"].Value), GetPickList(Convert.ToInt32(rsStaticAttrbutes.Fields["g3e_pno"].Value)));
                                }
                                rsStaticAttrbutes.MoveNext();
                            }
                        }
                    }
                }

                GetPickListData(dicPickListMapping, sCategoryFilter);
                sPickListData = dicPickListMapping;
                string sCommaSeperatedValue = string.Join(",", lStaticAttributesUsernames);
                sColumnNames = string.Join(",", lstColumnNames);
                sNullValues = ReturnNullStrings(lstColumnNames.Count);


                if (bPrimary)
                {
                    //two conditions need to be covered, one for sColumnNames and one for not having sColumnNames
                    if (!string.IsNullOrEmpty(sColumnNames))
                    {
                        sSQL = string.Format(@"select * from (
                                     select * from GIS_ONC.CULIB_SELECTION_VW
                                     where Category in ('{0}'))
                                     PIVOT(max(ATTRVALUE) for ATTRNAME in ({1}))
                                     order by MC desc, CU ", sCategoryFilter, sColumnNames);
                    }
                    else
                    {
                        sSQL = string.Format(@"select 
    DECODE(m.MU_ID, NULL, u.CATEGORY_C, m.CATEGORY_C) as Category,
    DECODE(m.MU_ID, NULL, 'CU', 'MCU') as MC,
    DECODE(m.MU_ID, NULL, u.CU_ID, m.MU_ID) as CU,
    DECODE(m.MU_ID, NULL, u.CU_DESC, m.MU_DESC) as Description
    from      CULIB_UNIT      u
    left join CULIB_MACROUNIT mu on (u.CU_ID = mu.CU_ID and mu.PRIMARY_YN = 'Y')
    left join CULIB_MACRO     m  on mu.MU_ID = m.MU_ID
    where COALESCE(m.CATEGORY_C, u.CATEGORY_C) in ('{0}')
      and (u.EFFECTIVE_D <= SYSDATE or u.EFFECTIVE_D is null)
      and (u.EXPIRATION_D >= SYSDATE or u.EXPIRATION_D is null)
    order by 2 desc, 3", sCategoryFilter);
                    }
                }
                else
                {
                    // Because Ancillaries do not have standard attributes, this query will not use column names and will not need a PIVOT
                    //                sSQL = string.Format(@"select 
                    //DECODE(m.MU_ID, NULL, u.CATEGORY_C, m.CATEGORY_C) as Category,
                    //DECODE(m.MU_ID, NULL, 'ACU', 'AMCU') as MC,
                    //DECODE(m.MU_ID, NULL, u.CU_ID, m.MU_ID) as CU,
                    //DECODE(m.MU_ID, NULL, u.CU_DESC, m.MU_DESC) as Description
                    //from      CULIB_UNIT      u
                    //left join CULIB_MACROUNIT mu on (u.CU_ID = mu.CU_ID and mu.PRIMARY_YN = 'Y')
                    //left join CULIB_MACRO     m  on mu.MU_ID = m.MU_ID
                    //where COALESCE(m.CATEGORY_C, u.CATEGORY_C) in ('{0}')
                    //  and (u.EFFECTIVE_D <= SYSDATE or u.EFFECTIVE_D is null)
                    //  and (u.EXPIRATION_D >= SYSDATE or u.EXPIRATION_D is null)
                    //order by 2 desc, 3", sCategoryFilter);
                    sSQL = string.Format(@"select DISTINCT
                    DECODE(m.MU_ID, NULL, u.CATEGORY_C, m.CATEGORY_C) as Category,
                    DECODE(m.MU_ID, NULL, 'ACU', 'AMCU') as MC,
                    DECODE(m.MU_ID, NULL, u.CU_ID, m.MU_ID) as CU,
                    DECODE(m.MU_ID, NULL, u.CU_DESC, m.MU_DESC) as Description
                    from      CULIB_UNIT      u
                    left join CULIB_MACROUNIT mu on (u.CU_ID = mu.CU_ID)
                    left join CULIB_MACRO     m  on mu.MU_ID = m.MU_ID
                    where COALESCE(m.CATEGORY_C, u.CATEGORY_C) in ('{0}')
                      and (u.EFFECTIVE_D <= SYSDATE or u.EFFECTIVE_D is null)
                      and (u.EXPIRATION_D >= SYSDATE or u.EXPIRATION_D is null)
                    order by 2 desc, 3", sCategoryFilter);
                }

                oRS = m_oDataContext.OpenRecordset(sSQL, ADODB.CursorTypeEnum.adOpenStatic, ADODB.LockTypeEnum.adLockReadOnly, (System.Int32)ADODB.CommandTypeEnum.adCmdText);

                Result = GetDataTable(oRS);

                for (int j = 0; j < Result.Columns.Count; j++)
                {
                    if (dColumnNames.ContainsKey(Result.Columns[j].ColumnName))
                    {
                        string sColumnName = string.Empty;
                        dColumnNames.TryGetValue(Result.Columns[j].ColumnName, out sColumnName);
                        Result.Columns[j].ColumnName = sColumnName;
                    }

                    if (Result.Columns[j].ColumnName.Equals("MC"))
                    {
                        Result.Columns[j].ColumnName = "M/C"; //This is for display purpose
                    }

                    if (Result.Columns[j].ColumnName.Equals("CATEGORY"))
                    {
                        Result.Columns[j].ColumnName = "Category"; //This is for display purpose
                    }

                }
            }
            catch (Exception ex)
            {
                throw (ex);
            }

            if (!bPrimary)
            {
                if (Result.Columns.Contains("Category"))
                {
                    Result.Columns.Remove("Category");
                }
            }
            GetKeyValueDataTable(Result, sPickListData);
            return Result;
        }

        /// <summary>
        /// Method to replace the content of DataTable to Value Field of a picklist
        /// </summary>
        /// <param name="dKeyFieldData"></param>
        /// <param name="dicPickList"></param>
        private void GetKeyValueDataTable(DataTable dKeyFieldData, Dictionary<string, List<PickiListData>> dicPickList)
        {
            try
            {
                if (dicPickList != null)
                {
                    for (int i = 0; i < dKeyFieldData.Columns.Count; i++)
                    {
                        if (dicPickList.ContainsKey(dKeyFieldData.Columns[i].ColumnName))
                        {
                            for (int j = 0; j < dKeyFieldData.Rows.Count; j++)
                            {
                                List<PickiListData> lstKeyField = new List<PickiListData>();
                                dicPickList.TryGetValue(dKeyFieldData.Columns[i].ColumnName, out lstKeyField);
                                PickiListData sValue = lstKeyField.Find(p => p.KeyField == dKeyFieldData.Rows[j][i].ToString());

                                if (sValue != null)
                                {
                                    dKeyFieldData.Rows[j][i] = sValue.ValueField;
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw (ex);
            }
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

        DataRow m_drSelectedCURow = null;
        private void RemoveCUInstance(int p_ReplacedCID)
        {
            try
            {
                int iCurrentRecord = Convert.ToInt32(m_oKeyObject.Components["COMP_UNIT_N"].Recordset.Fields["g3e_cid"].Value);
                int iPreviousCID = 0;
                bool bRecordFound = false;

                if (m_oKeyObject.Components["COMP_UNIT_N"].Recordset != null)
                {                    
                    m_oKeyObject.Components["COMP_UNIT_N"].Recordset.MoveFirst();
                    while (m_oKeyObject.Components["COMP_UNIT_N"].Recordset.EOF == false)
                    {
                        iPreviousCID = Convert.ToInt32(m_oKeyObject.Components["COMP_UNIT_N"].Recordset.Fields["G3E_CID"].Value);
                        if (Convert.ToString(m_oKeyObject.Components["COMP_UNIT_N"].Recordset.Fields["REPLACED_CID"].Value).Equals(p_ReplacedCID.ToString()) && Convert.ToString(m_oKeyObject.Components["COMP_UNIT_N"].Recordset.Fields["ACTIVITY_C"].Value).Equals("I") && (iPreviousCID != iCurrentRecord))
                        {
                            bRecordFound = true;
                            break;
                        }
                        m_oKeyObject.Components["COMP_UNIT_N"].Recordset.MoveNext();
                    }
                    if (bRecordFound)
                    {
                        m_oKeyObject.Components["COMP_UNIT_N"].Recordset.Delete();
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        bool m_bChangeOutOperation = false;
        int m_changeOutInstance = -1;

        private DialogResult CUSelection(bool bPrimary, string sCategoryFilter, int iOccurrence = -1, string sKeyword = null, string sValue = null, string sExistingCUpassed = null, RemovalActivity RemovalActivity = RemovalActivity.NoValue, InstallActivity InstallActivitySet = InstallActivity.Novalue, string sExistngCU = null, string ExistingActivity = null, string UnitCNO = null, string UNITCID = null) //, Optional ByVal bExistingCUType As Boolean = False
        {

            // First check if it was only the removal activity
            // Next check if the same CU needs the replacement
            // IF not then show the dialog for the CU selection

            if (InstallActivitySet != InstallActivity.Novalue)
            {
                m_ExistingUnitCID = UNITCID;
                m_ExistingUnitCNO = UnitCNO;
            }

            if (RemovalActivity  !=RemovalActivity.NoValue)
            {
                m_bChangeOutOperation = true;
                m_changeOutInstance = iOccurrence;
            }


            if (InstallActivitySet == InstallActivity.Replace) //This is a changeout operation
            {
                m_sSelectedCUType = "CU";

                Add(sExistngCU, bPrimary, "1", "I");

                if (ExistingActivity.Equals("R") || ExistingActivity.Equals("S"))
                {
                    RemoveCUInstance(Convert.ToInt32(m_oKeyObject.Components["COMP_UNIT_N"].Recordset.Fields["REPLACED_CID"].Value));
                }
                return DialogResult.OK;
            }

            DialogResult vRetVal = DialogResult.Cancel;
            dlgPriCUSelection oDlgPri = null;
            dlgAncCUSelection oDlgAnc = null;
            System.Windows.Forms.DialogResult vDialogResult = DialogResult.None;
            Exception oEx = null;
            string sExistingCU = null;
            bool bRecordExists = false;
            int i = 0;
            ADODB.Recordset oRS = null;
            string sCUType = null;
            bool bMacroCU = false;
            bool bUserCancel = false;
            List<CUDataModel> dtCUDataSource = null;

            try
            {
                //If a feature has not been defined, throw an exception.
                if (m_oKeyObject == null)
                {
                    oEx = new Exception("The Selection method was called and no feature is currently specified.");
                    throw (oEx);
                }

                Dictionary<string, List<PickiListData>> dicPickListData = new Dictionary<string, List<PickiListData>>();

                //If there are no CUs to show, inform the user and exit.

                dtCUDataSource = this.GetAllCUData(m_oKeyObject.FNO, bPrimary, sCategoryFilter, sKeyword, sValue, out dicPickListData);

                if (dtCUDataSource == null)
                {
                    //Problems getting a valid recordset.
                    oEx = new Exception("Error getting recordset for CU list.");
                    throw (oEx);
                }
                if (dtCUDataSource.Count == 0)
                {
                    //Recordset is empty.
                    MessageBox.Show("There are no CUs that match the current set of filter requirements.", "CUService", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return DialogResult.Cancel;
                }

                //In the CUSelection mode (user is selecting a CU/MCU from the selection dialog,
                //if we  pass a valid occurrence (iOccurrence > 0), then the CU/MCU
                //is essentially replacing whatever is on this record.  Therefore, we must
                //DELETE the existing record and then let the Add operation add the new CU/MCU.

                //Before we do this, though, we need to check to see if the user is trying to "replace"
                //an occurrence of an Ancillary CU record that was generated from a Primary Macro CU.
                //If so, then raise an exception as this is not allowed.

                if (0 < iOccurrence && !bPrimary)
                {
                    sExistingCU = GetExistingCU(false, true, iOccurrence, m_oKeyObject, ref bRecordExists);
                    if (bRecordExists)
                    {
                        if (string.Empty != sExistingCU)
                        {
                            if (sExistingCU.StartsWith("(") && sExistingCU.EndsWith(")"))
                            {
                                oEx = new System.Exception("An occurrence of Ancillary CU that has been" + "\n" + "generated as part of a Primary Macro CU" + "\n" + "cannot be removed by replacing the Ancillary CU.");
                                throw (oEx);
                            }
                        }
                    }
                }

                if (bPrimary)
                {
                    oDlgPri = new dlgPriCUSelection();
                    oDlgPri.Load += oDlgPri.dlgPriCUSelection_Load;
                    oDlgPri.CURecordset = oRS;
                    oDlgPri.CUGridBindingWholeData = dtCUDataSource;
                    oDlgPri.CategoryList = GetAllCUCategories(sCategoryFilter);
                    oDlgPri.CUService = this;
                    oDlgPri.DataContext = this.m_oDataContext;
                    oDlgPri.FNO = m_oKeyObject.FNO;
                    m_sSelectedCUType = sValue; //The value that is getting selected will always refer the selected CU Type. No need to query it again and again

                    oDlgPri.ColumnPickListData = dicPickListData;
                    try
                    {
                        oDlgPri.ShowDialog(GTClassFactory.Create<IGTApplication>().ApplicationWindow);
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }

                    vDialogResult = oDlgPri.DialogResult;
                    if (vDialogResult == DialogResult.OK)
                    {
                        //The Primary CU Selection dialog will return a single CU
                        //so there's no need to parse it for multiple CU values.

                        sCUType = oDlgPri.CUTypeSelected;
                        m_sSelectedCUType = sCUType;
                        m_drSelectedCURow = oDlgPri.m_selectedCUDataRecord;

                        bMacroCU = (string)sCUType == M_SMCU || (string)sCUType == M_SAMCU;

                        if (!(sExistingCUpassed == null) && 0 < iOccurrence)
                        {
                            sExistingCU = sExistingCUpassed;
                            // GetCUMapping(sExistingCU, m_oKeyObject.FNO, ref sCUType, ref sFilter);
                            bMacroCU = (string)sCUType == M_SMCU || (string)sCUType == M_SAMCU;
                            sExistingCU = GetExistingCU(bPrimary, bMacroCU, iOccurrence, m_oKeyObject, ref bRecordExists);
                        }
                        else
                        {
                            sExistingCU = GetExistingCU(bPrimary, bMacroCU, iOccurrence, m_oKeyObject, ref bRecordExists);
                            //if (sExistingCU == "") bRecordExists = false; Sometimes the component exists but the CU Code could be null

                        }

                        if (InstallActivitySet != InstallActivity.Novalue) bRecordExists = false; //This is a changeout request and needs to be created newly so just set bRecordExists to false

                        //If there is an occurrence specified, and an existing CU/MCU, then this is a Replacement operation.
                        if (0 < iOccurrence && bRecordExists)
                        {
                            ReplaceCU("I" ,sExistingCU, oDlgPri.CU, bPrimary, (short)iOccurrence, bUserCancel);
                            //If the user elects not to replace a Macro CU with a CU,
                            //then we don't want to process any of the rest of this code
                            //for this KeyObject.
                            if (bUserCancel)
                            {
                            }
                        }
                        else
                        {
                            //Not a replacement so just process the selection based on the type.
                            if (((string)sCUType == M_SCU))
                            {
                                for (i = 1; i <= oDlgPri.Qty; i++)
                                {
                                    Add(oDlgPri.CU, bPrimary, "1", "I");
                                }
                            }
                            else if ((string)sCUType == M_SMCU)
                            {
                                Add(oDlgPri.CU, bPrimary, "1","I");
                            }
                            else if (((string)sCUType == M_SACU) || ((string)sCUType == M_SAMCU))
                            {
                                //Ancillary CUs or Ancillary Macro CUs should not be returned by the Primary Selection dialog.
                                //Throw an error if this occurs.
                                oEx = new Exception("Selection process stopped.  CU: " + System.Convert.ToString(oDlgPri.CU) + " is neither a Primary CU or Primary Macro CU.");
                                throw (oEx);
                            }
                        }
                        vRetVal = DialogResult.OK;
                    }
                    else if (vDialogResult == DialogResult.Cancel)
                    {
                        vRetVal = DialogResult.Cancel;
                    }
                }
                else
                {
                    oDlgAnc = new dlgAncCUSelection();
                    oDlgAnc.Load += oDlgAnc.dlgAncCUSelection_Load;
                    oDlgAnc.Shown += oDlgAnc.dlgSelection_Shown;
                    oDlgAnc.CURecordset = oRS;
                    oDlgAnc.CUService = this;
                    oDlgAnc.DataContext = this.m_oDataContext;
                    oDlgAnc.FNO = m_oKeyObject.FNO;
                    oDlgAnc.ColumnPickListData = dicPickListData;
                    m_sSelectedCUType = sValue;

                    if (m_sSelectedCUType == "ACU")
                    {
                        oDlgAnc.AncillaryCategories = GetCUCategories(sCategoryFilter);
                    }
                    oDlgAnc.CUGridBindingWholeData = dtCUDataSource;//Shubham NEed to take care of this part later
                    oDlgAnc.ShowDialog(GTClassFactory.Create<IGTApplication>().ApplicationWindow);
                    vDialogResult = oDlgAnc.DialogResult;
                    if (vDialogResult == DialogResult.OK)
                    {
                        m_sSelectedCUType = oDlgAnc.CUTypeSelected;
                        //Get the CU Type.

                        sCUType = m_sSelectedCUType; //This will be overriden further as the selected CU may be ACU or AMCU depending upon each selection

                        bMacroCU = (string)sCUType == M_SMCU || (string)sCUType == M_SAMCU; //This will be overriden further as the selected CU may be ACU or AMCU 

                        if (!(sExistingCUpassed == null))
                        {
                            sExistingCU = sExistingCUpassed;
                            bRecordExists = true;
                        }
                        else
                        {
                            sExistingCU = GetExistingCU(bPrimary, bMacroCU, iOccurrence, m_oKeyObject, ref bRecordExists);
                        }

                        if (InstallActivitySet != InstallActivity.Novalue) bRecordExists = false; //This is a changeout request and needs to be created newly
                                                                                                  //If there is an occurrence specified, and an existing ACU/AMCU, then this is a Replacement operation.
                        if (0 < iOccurrence && bRecordExists)
                        {
                            ReplaceCU(oDlgAnc.Activity, sExistingCU, oDlgAnc.CU, bPrimary, (short)iOccurrence, bUserCancel, System.Convert.ToString(oDlgAnc.Qty), oDlgAnc.AMCU);
                            //If the user elects not to replace a Macro CU with a CU,
                            //then we don't want to process any of the rest of this code
                            //for this KeyObject.
                            if (bUserCancel)
                            {

                            }
                        }
                        else
                        {
                            //Not a replacement so just process the selection based on the type.
                            Add(oDlgAnc.CU, bPrimary, oDlgAnc.Qty, oDlgAnc.Activity, oDlgAnc.AMCU);
                        }

                        vRetVal = DialogResult.OK;
                    }
                    else if (vDialogResult == DialogResult.Cancel)
                    {
                        vRetVal = DialogResult.Cancel;
                    }
                }

                if (InstallActivitySet != InstallActivity.Novalue && vRetVal == DialogResult.OK) //This is a Changeout scenario, so 
                {
                    if (ExistingActivity.Equals("R") || ExistingActivity.Equals("S"))
                    {
                        RemoveCUInstance(Convert.ToInt32(m_oKeyObject.Components["COMP_UNIT_N"].Recordset.Fields["REPLACED_CID"].Value));
                    }
                }
                return vRetVal;

            }
            catch (Exception ex)
            {
                throw (ex);
            }

        }
        //private void RemoveDuplicatedRecordForReplacedCID(InstallActivity p_InstallActivitySet)
        //{
        //    if (p_InstallActivitySet != InstallActivity.Novalue)
        //    {
        //        try
        //        {
        //            if (m_oKeyObject.Components["COMP_UNIT_N"].Recordset != null)
        //            {
        //                m_oKeyObject.Components["COMP_UNIT_N"].Recordset.MoveFirst();
        //                while (m_oKeyObject.Components["COMP_UNIT_N"].Recordset.EOF == false)
        //                {
        //                    if (Convert.ToString(m_oKeyObject.Components["COMP_UNIT_N"].Recordset.Fields["REPLACED_CID"].Value).Equals(""))
        //                    {
        //                        break;
        //                    }
        //                    m_oKeyObject.Components["COMP_UNIT_N"].Recordset.MoveNext();
        //                }
        //                m_oKeyObject.Components["COMP_UNIT_N"].Recordset.Delete();
        //            }
        //        }
        //        catch (Exception ex)
        //        {
        //            throw ex;
        //        }
        //    }
        //}
        private string GetExistingCU(bool bPrimary, bool bMacro, int iOccurrence, IGTKeyObject oKeyObj, ref bool bRecordExists)
        {
            string sCU = null;
            IGTComponent oComponent = null;
            ADODB.Recordset oRS = null;
            int iCNO = -1;
            string sFieldName = null;
            System.Exception oEx = null;

            try
            {
                bRecordExists = false;

                if (0 > iOccurrence)
                {
                    goto endOfTry;
                }

                if (bPrimary)
                {
                    iCNO = System.Convert.ToInt32(m_iCompUnitCNO);
                    if (bMacro)
                    {
                        sFieldName = m_sCompUnitMacroFieldName;
                        m_sExistingCUType = "MCU";

                    }
                    else
                    {
                        sFieldName = m_sCompUnitFieldName;
                        m_sExistingCUType = "CU";
                    }
                }
                else
                {
                    iCNO = System.Convert.ToInt32(m_iAncCompUnitCNO);
                    if (bMacro)
                    {
                        sFieldName = m_sAncCompUnitMacroFieldName;
                        m_sExistingCUType = "AMCU";
                    }
                    else
                    {
                        sFieldName = m_sAncCompUnitFieldName;
                        m_sExistingCUType = "ACU";
                    }
                }

                oComponent = oKeyObj.Components.GetComponent((short)iCNO);
                if (oComponent == null)
                {
                    goto endOfTry;
                }

                //Clone the feature's recordset here so that any changes (filters, etc.)
                //won't affect downstream operations.
                oRS = oComponent.Recordset.Clone();
                if (oRS == null)
                {
                    goto endOfTry;
                }

                if (!oRS.BOF)
                {
                    oRS.MoveFirst();
                }
                if (oRS.BOF && oRS.EOF)
                {
                    goto endOfTry;
                }

                oRS.Filter = "g3e_cid=" + iOccurrence.ToString();
                if (1 == oRS.RecordCount)
                {
                    sCU = oRS.Fields[sFieldName].Value.ToString();
                    bRecordExists = true;
                }
            }
            catch (Exception ex)
            {
                oEx = new System.Exception("Error getting existing CU value." + "\n" + ex.Message);
                throw (oEx);
            }
            endOfTry:

            if (!(oRS == null))
            {
                oRS.Filter = System.String.Empty;
            }

            return sCU;
        }

        private List<string> GetCUCategories(string sFilter)
        {
            List<string> CUCategories = new List<string>();
            ADODB.Recordset rsCUCategories = null;
            string sSql = string.Empty;

            try
            {
                sSql = string.IsNullOrEmpty(sFilter) ? "select DISTINCT CATEGORY_C from " + m_sCuCategoryTable + " where TYPE_C = 'A'" : "select DISTINCT CATEGORY_C from " + m_sCuCategoryTable + " WHERE TYPE_C = 'A' AND category_c in ('" + sFilter + "')";

                rsCUCategories = m_oDataContext.OpenRecordset(sSql, CursorTypeEnum.adOpenStatic, LockTypeEnum.adLockReadOnly, (int)ADODB.CommandTypeEnum.adCmdText, new object[1]);
                if (rsCUCategories != null)
                {
                    if (rsCUCategories.RecordCount > 0)
                    {
                        rsCUCategories.MoveFirst();
                        while (rsCUCategories.EOF == false)
                        {
                            CUCategories.Add(Convert.ToString(rsCUCategories.Fields["CATEGORY_C"].Value));
                            rsCUCategories.MoveNext();
                        }
                    }
                }
            }

            catch (Exception ex)
            {
                throw ex;
            }
            return CUCategories;
        }

        public DialogResult Selection(bool Primary, String Keyword, String Value, string CategoryFilter)
        {
            DialogResult returnValue = default(DialogResult);
            CheckDataContext();
            returnValue = CUSelection(Primary, CategoryFilter, sKeyword: Keyword, sValue: Value);
            return returnValue;
        }
        public DialogResult Selection(bool Primary, short Occurrence, string Keyword, string Value, string CategoryFilter, RemovalActivity RemovalActivity, InstallActivity InstallActivity, string ExistingCU = null, string ExistingActivity = null, string ExistingUnitCID = null, string ExistingUnitCNO = null)
        {
            DialogResult returnValue = default(DialogResult);
            CheckDataContext();
            returnValue = CUSelection(Primary, CategoryFilter, iOccurrence: Occurrence, sKeyword: Keyword, sValue: Value, RemovalActivity: RemovalActivity, InstallActivitySet: InstallActivity, sExistngCU: ExistingCU, ExistingActivity: ExistingActivity, UNITCID: ExistingUnitCID, UnitCNO: ExistingUnitCNO);
            return returnValue;
        }

        private void InitializeGeneralParameters()
        {
            //Initializes private member variables from SYS_GENERALPARAMETER.
            ADODB.Recordset oRS = null;
            Exception oEx = null;

            try
            {
                m_sCULibraryTable = GetParameter("CU Library Table", true).ToString();
                m_sCUMacroLibraryTable = GetParameter("CU Macro Library Table", true).ToString();
                if (m_iCompUnitANO == 0)
                {
                    m_iCompUnitANO = System.Convert.ToInt32(GetParameter("Primary CU Attribute", true));
                }
                m_iCompUnitMacroANO = System.Convert.ToInt32(GetParameter("Primary Macro CU Attribute", true));
                m_iAncCompUnitANO = System.Convert.ToInt32(GetParameter("Ancillary CU Attribute", true));
                m_iAncCompUnitMacroANO = System.Convert.ToInt32(GetParameter("Ancillary Macro CU Attribute", true));
                m_iCompUnitQtyANO = System.Convert.ToInt32(GetParameter("Primary CU Qty Attribute", true));
                m_iAncCompUnitQtyANO = System.Convert.ToInt32(GetParameter("Ancillary CU Qty Attribute", true));
                m_sCuCategoryTable = GetParameter("CU Category Mapping Table", true).ToString();

                oRS = m_oDataContext.MetadataRecordset("G3E_ATTRIBUTEINFO_OPTABLE");

                if (oRS.BOF && oRS.EOF)
                {
                    oEx = new Exception("No data found in G3E_ATTRIBUTEINFO_OPTABLE.");
                    throw (oEx);
                }
                else
                {
                    oRS.Filter = "g3e_ano=" + m_iCompUnitANO.ToString();
                    if (oRS.BOF && oRS.EOF)
                    {
                        oRS.Close();
                        oEx = new Exception("Value not found in G3E_ATTRIBUTEINFO_OPTABLE where G3E_ANO=" + m_iCompUnitANO.ToString());
                        throw (oEx);
                    }
                    //The following  are NotNull fields
                    m_iCompUnitCNO = System.Convert.ToInt32(oRS.Fields["g3e_cno"].Value);
                    m_sCompUnitFieldName = oRS.Fields["g3e_field"].Value.ToString();

                    oRS.Filter = "g3e_ano=" + m_iCompUnitMacroANO.ToString();
                    if (oRS.BOF && oRS.EOF)
                    {
                        oRS.Close();
                        oEx = new Exception("Value not found in G3E_ATTRIBUTEINFO_OPTABLE where G3E_ANO=" + m_iCompUnitMacroANO.ToString());
                        throw (oEx);
                    }
                    //The following  are NotNull fields
                    m_iCompUnitMacroCNO = System.Convert.ToInt32(oRS.Fields["g3e_cno"].Value);
                    m_sCompUnitMacroFieldName = oRS.Fields["g3e_field"].Value.ToString();

                    oRS.Filter = "g3e_ano=" + m_iAncCompUnitANO.ToString();
                    if (oRS.BOF && oRS.EOF)
                    {
                        oRS.Close();
                        oEx = new Exception("Value not found in G3E_ATTRIBUTEINFO_OPTABLE where G3E_ANO=" + m_iAncCompUnitANO.ToString());
                        throw (oEx);
                    }
                    //The following  are NotNull fields
                    m_iAncCompUnitCNO = System.Convert.ToInt32(oRS.Fields["g3e_cno"].Value);
                    m_sAncCompUnitFieldName = oRS.Fields["g3e_field"].Value.ToString();

                    oRS.Filter = "g3e_ano=" + m_iAncCompUnitMacroANO.ToString();
                    if (oRS.BOF && oRS.EOF)
                    {
                        oRS.Close();
                        oEx = new Exception("Value not found in G3E_ATTRIBUTEINFO_OPTABLE where G3E_ANO=" + m_iAncCompUnitMacroANO.ToString());
                        throw (oEx);
                    }
                    //The following  are NotNull fields
                    m_iAncCompUnitMacroCNO = System.Convert.ToInt32(oRS.Fields["g3e_cno"].Value);
                    m_sAncCompUnitMacroFieldName = oRS.Fields["g3e_field"].Value.ToString();

                    oRS.Filter = "g3e_ano=" + m_iCompUnitQtyANO.ToString();
                    if (oRS.BOF && oRS.EOF)
                    {
                        oRS.Close();
                        oEx = new Exception("Value not found in G3E_ATTRIBUTEINFO_OPTABLE where G3E_ANO=" + m_iCompUnitQtyANO.ToString());
                        throw (oEx);
                    }
                    //The following  are NotNull fields
                    m_sCompUnitQtyFieldName = oRS.Fields["g3e_field"].Value.ToString();

                    oRS.Filter = "g3e_ano=" + m_iAncCompUnitQtyANO.ToString();
                    if (oRS.BOF && oRS.EOF)
                    {
                        oRS.Close();
                        oEx = new Exception("Value not found in G3E_ATTRIBUTEINFO_OPTABLE where G3E_ANO=" + m_iAncCompUnitQtyANO.ToString());
                        throw (oEx);
                    }
                    //The following  are NotNull fields
                    m_sAncCompUnitQtyFieldName = oRS.Fields["g3e_field"].Value.ToString();

                }

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
                //Pass the exception back up to the calling process.
                throw (ex);
            }

            oRS.Close();
            return;

        }
        private string GetParameter(string sParameterName, bool bErrorIfNotExist)
        {
            //Retrieves the parameter value from the recordset.
            //Throws exception of parameter not found.
            ADODB.Recordset oRS = null;
            Exception oEx = null;
            System.String sRetVal = string.Empty;

            try
            {
                oRS = m_oDataContext.OpenRecordset("select * from " + M_SGENERALPARAMATERTABLE + " where SUBSYSTEM_COMPONENT = 'CUSelection' and SUBSYSTEM_NAME = 'CUSelection' and  PARAM_NAME=?", ADODB.CursorTypeEnum.adOpenStatic, ADODB.LockTypeEnum.adLockReadOnly, (System.Int32)ADODB.CommandTypeEnum.adCmdText, sParameterName);
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
                if (bErrorIfNotExist)
                {
                    oEx = new Exception("Entry not found in " + M_SGENERALPARAMATERTABLE + " where G3E_NAME = \'" + System.Convert.ToString(sParameterName) + "\'");
                    throw (oEx);
                }
            }
            else
            {
                oRS.MoveFirst();
                sRetVal = oRS.Fields["PARAM_VALUE"].Value.ToString();
                oRS.Close();
            }

            return sRetVal;

        }
        private bool IsComponentRepeating(int iFNO, int iCNO)
        {
            bool returnValue = default(bool);
            ADODB.Recordset oRS = null;
            Exception oEx = null;

            try
            {
                oRS = m_oDataContext.MetadataRecordset("G3E_FEATURECOMPS_OPTABLE");
            }
            catch (Exception)
            {
                if (!(oRS == null))
                {
                    if ((int)ADODB.ObjectStateEnum.adStateOpen == oRS.State)
                    {
                        oRS.Close();
                    }
                }
                oEx = new Exception("Unable to open recordset for table G3E_FEATURECOMPS_OPTABLE");
                throw (oEx);
            }

            if (oRS.BOF && oRS.EOF)
            {
                oRS.Close();
                oEx = new Exception("G3E_FEATURECOMPS_OPTABLE is empty.");
                throw (oEx);
            }

            oRS.Filter = "g3e_fno=" + iFNO.ToString() + " and g3e_cno=" + iCNO.ToString();
            if (oRS.BOF && oRS.EOF)
            {
                oRS.Close();
                oEx = new Exception("No entries in G3E_FEATURECOMPS_OPTABLE where G3E_FNO=" + iFNO.ToString() + " and G3E_CNO=" + iCNO.ToString());
                throw (oEx);
            }

            try
            {
                returnValue = (oRS.Fields["g3e_repeating"].Value.ToString() == "1") ? true : false;
            }
            catch (Exception ex)
            {
                throw (ex);
            }

            oRS.Close();

            return returnValue;
        }
        private bool IsComponentRequired(int iFNO, int iCNO)
        {
            bool returnValue = default(bool);
            ADODB.Recordset oRS = null;
            Exception oEx = null;

            try
            {
                oRS = m_oDataContext.MetadataRecordset("G3E_FEATUREPLACEMENT_OPTABLE");
            }
            catch (Exception)
            {
                if (!(oRS == null))
                {
                    if ((int)ADODB.ObjectStateEnum.adStateOpen == oRS.State)
                    {
                        oRS.Close();
                    }
                }
                oEx = new Exception("Unable to open recordset for table G3E_FEATUREPLACEMENT_OPTABLE");
                throw (oEx);
            }

            if (oRS.BOF && oRS.EOF)
            {
                oRS.Close();
                oEx = new Exception("G3E_FEATUREPLACEMENT_OPTABLE is empty.");
                throw (oEx);
            }

            oRS.Filter = "g3e_fno=" + iFNO.ToString() + " and g3e_cno=" + iCNO.ToString();
            if (oRS.BOF && oRS.EOF)
            {
                oRS.Close();
                oEx = new Exception("No entries in G3E_FEATUREPLACEMENT_OPTABLE where G3E_FNO=" + iFNO.ToString() + " and G3E_CNO=" + iCNO.ToString());
                throw (oEx);
            }

            try
            {
                returnValue = (oRS.Fields["g3e_required"].Value.ToString() == "1") ? true : false;
            }
            catch (Exception ex)
            {
                throw (ex);
            }

            oRS.Close();

            return returnValue;
        }
        private bool IsMacroCU(string sCUCode)
        {
            System.Object[] oParams = new System.Object[1];
            ADODB.Recordset oRS = null;
            Exception oEx = null;
            System.Object sSQL = null;

            try
            {
                sSQL = "select count(distinct(MU_ID)) from " + System.Convert.ToString(m_sCUMacroLibraryTable) + " where MU_ID=?";
                oParams[0] = sCUCode;
                oRS = m_oDataContext.OpenRecordset(System.Convert.ToString(sSQL), ADODB.CursorTypeEnum.adOpenStatic, ADODB.LockTypeEnum.adLockReadOnly, (System.Int32)ADODB.CommandTypeEnum.adCmdText, oParams);
            }
            catch (Exception)
            {
                if (!(oRS == null))
                {
                    if ((int)ADODB.ObjectStateEnum.adStateOpen == oRS.State)
                    {
                        oRS.Close();
                    }
                }
                oEx = new Exception("Unable to open recordset for query: " + System.Convert.ToString(sSQL));
                throw (oEx);
            }

            try
            {
                return (Convert.ToInt32(oRS.Fields[0].Value) == 1) ? true : false;
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }

        private void ExpandMacro(string sMacroCU, string sCUType, int iM_Qty, IGTKeyObject oKeyObj, string strActivity)
        {
            //If the CU is a macro CU (either primary or ancillary), expand it
            //by creating additional ancillary CU records for the number and type
            //of CUs defined for the macro.

            System.Exception oEx = null;
            System.Object sSQL = null;
            System.Object[] oParams = new System.Object[1];
            ADODB.Recordset oMacroRS = null;
            IGTComponent oComp = null;
            ADODB.Recordset oCompRS = null;
            string sCU = null;
            short iQty = System.Convert.ToInt16(null);
            bool bPrimary = System.Convert.ToBoolean(null);
            bool bPrimaryCU = System.Convert.ToBoolean(null);
            int iCNO = 0;
            string sCUFieldName = null;
            string sMCUFieldName = null;
            short iCUCount = System.Convert.ToInt16(null);
            string sLocalCUType = null;
            bool bRepeatingCUComponent = System.Convert.ToBoolean(null);
            int iCID = 0;

            try
            {

                if (!(M_SMCU == (string)sCUType || M_SAMCU == (string)sCUType))
                {
                    //Not a macro CU.  Nothing to do here.
                    goto WrapUp;
                }

                //We need an indicator that tells us whether we're processing
                //a Primary Macro CU or Ancillary Macro CU.
                //(Used for the procedure that generates a CU record).
                bPrimary = (string)sCUType == M_SMCU;

                //Get a recordset of the CUs for this macro
                sSQL = "select * from " + System.Convert.ToString(m_sCUMacroLibraryTable) + " where mu_id=?";
                oParams[0] = (System.Object)sMacroCU;
                oMacroRS = m_oDataContext.OpenRecordset(System.Convert.ToString(sSQL), ADODB.CursorTypeEnum.adOpenStatic, ADODB.LockTypeEnum.adLockReadOnly, (System.Int32)ADODB.CommandTypeEnum.adCmdText, oParams);

                //Generate new CU records for each CU of the macro
                if (!(oMacroRS.BOF && oMacroRS.EOF))
                {
                    oMacroRS.MoveFirst();
                    while (!oMacroRS.EOF)
                    {
                        //These are not-null fields.
                        sCU = oMacroRS.Fields["cu_id"].Value.ToString();
                        iQty = (short)(System.Convert.ToInt32(oMacroRS.Fields["cu_qty"].Value));
                        if (iM_Qty > 0)
                        {
                            iQty = (short)(iQty * iM_Qty);
                        }

                        bPrimaryCU = "Y" == oMacroRS.Fields["primary_YN"].Value.ToString().ToUpper();

                        if (!bPrimary) bPrimaryCU = false; //Assume all Ancillary MAcro contain Ancillary

                        if (!bPrimary && bPrimaryCU) //Case where an Ancillary MU (AMCU) is expanding Primary CU
                        {
                            //Only ACU should be a "local" CU type.
                           // oEx = new System.Exception("A Primary CU Type configuration was found for Macro CU " + System.Convert.ToString(sCU) + ".  This is invalid in a macro CU definition.");
                           // throw (oEx);
                        }
                        if (bPrimary && bPrimaryCU)
                        {
                            sLocalCUType = "CU";
                        }
                        else
                        {
                            sLocalCUType = "ACU";
                        }

                        if (bPrimaryCU)
                        {
                            iCNO = System.Convert.ToInt32(m_iCompUnitCNO);
                            sCUFieldName = m_sCompUnitFieldName;
                            sMCUFieldName = m_sCompUnitMacroFieldName;
                        }
                        else
                        {
                            iCNO = System.Convert.ToInt32(m_iAncCompUnitCNO);
                            sCUFieldName = m_sAncCompUnitFieldName;
                            sMCUFieldName = m_sAncCompUnitMacroFieldName;
                            //Set the private member variable to indicate
                            //that Ancillary CU records have been affected
                            //by the expansion of this macro.                            
                        }

                        //Is the CU component repeating.
                        bRepeatingCUComponent = IsComponentRepeating(oKeyObj.FNO, iCNO);

                        oComp = oKeyObj.Components.GetComponent((short)iCNO);
                        if (!(oComp == null))
                        {
                            oCompRS = oComp.Recordset;

                            if (sCUType == M_SMCU)
                            {
                                //This is a primary Macro CU.
                                //If the CU component is repeating and this is the primary CU
                                //or the CU is an RCU, then generate the number of records
                                //equal to iQty and set QTY on each record to 1;
                                //else, generate a single CU record and set its QTY to iQty.
                                if (((string)sLocalCUType == M_SCU))
                                {
                                    if (bRepeatingCUComponent)
                                    {
                                        for (iCUCount = 1; iCUCount <= iQty; iCUCount++)
                                        {
                                            //If necessary, generate a new CU component record.
                                            GenerateCURecord(oCompRS, bPrimary, iCNO, sMacroCU);

                                            //At this point, the recordset should be pointing to a valid record
                                            //(either a new one or the last empty existing one).
                                            //Get a local copy of the G3E_CID.
                                            iCID = System.Convert.ToInt32(oCompRS.Fields["g3e_cid"].Value);

                                            //Set the various field values.
                                            SetDefaultCUFieldInfo(oKeyObj, true, sLocalCUType, iCNO, sCU, sCUFieldName, sMacroCU, sMCUFieldName, 1, strActivity);

                                        }
                                    }
                                    else
                                    {
                                        //The component is not repeating.
                                        //Generate a single record and set its QTY to iQty.
                                        GenerateCURecord(oCompRS, bPrimary, iCNO, sMacroCU);
                                        //At this point, the recordset should be pointing to a valid record
                                        //(either a new one or the last empty existing one).
                                        //Set the various field values.
                                        SetDefaultCUFieldInfo(oKeyObj, true, sLocalCUType, iCNO, sCU, sCUFieldName, sMacroCU, sMCUFieldName, iQty, strActivity);
                                    }
                                }
                                else if ((string)sLocalCUType == M_SACU)
                                {
                                    //Ancillary Macros only generate a single record and set its QTY to iQty.
                                    GenerateCURecord(oCompRS, bPrimary, iCNO, sMacroCU);

                                    //At this point, the recordset should be pointing to a valid record (either a new one or the last empty existing one).
                                    //Set the various field values.
                                    SetDefaultCUFieldInfo(oKeyObj, true, sLocalCUType, iCNO, sCU, sCUFieldName, sMacroCU, sMCUFieldName, iQty, strActivity);
                                }
                                else
                                {
                                    //Only CU, RCU, or ACU should be a "local" CU type.
                                    oEx = new System.Exception("A CU Type of " + System.Convert.ToString(sLocalCUType) + " was found for Macro CU " + System.Convert.ToString(sCU) + ".  This is invalid in a macro CU definition.");
                                    throw (oEx);
                                }
                            }
                            else if (sCUType == M_SAMCU)
                            {
                                //When Ancillary Macros are expanded, we generate a single record for each CU
                                //and set its QTY to the value of iQty.
                                GenerateCURecord(oCompRS, bPrimary, iCNO, sMacroCU);

                                //At this point, the recordset should be pointing to a valid record (either a new one or the last empty existing one).
                                //Set the various field values.
                                SetDefaultCUFieldInfo(oKeyObj, false, sLocalCUType, iCNO, sCU, sCUFieldName, sMacroCU, sMCUFieldName, iQty, strActivity);
                            }

                        }
                        oMacroRS.MoveNext();
                    }
                }

            }
            catch (Exception ex)
            {
                throw (ex);
            }

            WrapUp:
            if (!(oMacroRS == null))
            {
                if ((int)ADODB.ObjectStateEnum.adStateOpen == oMacroRS.State)
                {
                    oMacroRS.Close();
                }
            }
        }

        /// <summary>
        /// This is the routine that handles the Delete() methods.
        ///
        ///If the CU component from which the CU is to be removed is optional,
        ///any instances of this CU will be deleted.
        ///
        ///If the CU component from which the CU is to be removed is required
        ///and this is not the last instance of this component,
        ///then the instance will be deleted.
        ///
        ///If this is the last instance of a required CU component, the instance will not be deleted;
        ///instead, all non-key fields will be set to null.
        ///
        ///If the CU being deleted is a primary CU,
        ///any static attributes associated with this CU will be left as is.
        ///
        ///If the CU being deleted is a macro CU,
        ///then all CUs (both primary and ancillary) associated with the macro will be deleted.
        ///
        ///If Delete is called and more than one GTKeyObject has been specified,
        ///an exception will be raised.
        ///
        ///If there are no features defined,
        ///then nothing will be done and no exception will be raised.
        ///
        ///If the CU hasn't been defined for the feature defined,
        ///then nothing will be done and no exception will be raised.
        ///
        ///If the CU is successfully deleted, then any auxilliary component records
        ///that have a matching G3E_CID to the deleted CU record will also be deleted.
        /// </summary>
        /// <param name="sCUCode"></param>
        /// <param name="bPrimary"></param>
        /// <param name="iOccurrence"></param>
        private void DeleteCU(string sCUCode, bool bPrimary, int iOccurrence = -1)
        {
            //See note for the AddCU() method for information about the m_oGTKeyObject
            //member variable and the declaration below.
            //Dim oKeyObj As GTKeyObject = Nothing

            IGTComponent oComponent = null;
            ADODB.Recordset oCompRS = null;
            int iCNO = 0;
            int iCID = 0;
            bool bComponentIsRequired = false;
            System.String sCUFieldName = string.Empty;
            System.String sMCUFieldName = string.Empty;
            System.String sCUFieldValue = string.Empty;
            System.String sMCUFieldValue = string.Empty;
            bool bMacroCU = false;
            bool bOccurrenceMatches = false;

            //If no key objects defined, nothing to do.
            if (m_oKeyObject == null)
            {
                goto WrapUp;
            }


            bMacroCU = IsMacroCU(sCUCode);

            //Which component and fieldnames
            if (bPrimary)
            {
                if (bMacroCU)
                {
                    iCNO = System.Convert.ToInt32(m_iCompUnitMacroCNO);
                }
                else
                {
                    iCNO = System.Convert.ToInt32(m_iCompUnitCNO);
                }
                sCUFieldName = System.Convert.ToString(m_sCompUnitFieldName);
                sMCUFieldName = System.Convert.ToString(m_sCompUnitMacroFieldName);
            }
            else
            {
                if (bMacroCU)
                {
                    iCNO = System.Convert.ToInt32(m_iAncCompUnitMacroCNO);
                }
                else
                {
                    iCNO = System.Convert.ToInt32(m_iAncCompUnitCNO);
                }
                sCUFieldName = System.Convert.ToString(m_sAncCompUnitFieldName);
                sMCUFieldName = System.Convert.ToString(m_sAncCompUnitMacroFieldName);
            }

            bComponentIsRequired = IsComponentRequired(m_oKeyObject.FNO, iCNO);

            //Get the appropriate CU Component
            oComponent = m_oKeyObject.Components.GetComponent((short)iCNO);

            if (oComponent == null)
            {
                //No CU component.  Nothing to do.
                goto WrapUp;
            }

            oCompRS = oComponent.Recordset;
            oCompRS.Filter = "";

            if (oCompRS == null)
            {
                //No component recordset.  Nothing to do.
                goto WrapUp;
            }

            if (oCompRS.BOF && oCompRS.EOF)
            {
                goto WrapUp;
            }

            //Iterate the records in the component recordset.
            //Delete records or null attribute value as appropriate.
            oCompRS.MoveLast();
            while (!oCompRS.BOF)
            {
                //Get field values from the recordset into local variables.
                if (bMacroCU)
                {
                    //This is a Macro CU.  Get the Macro CU code.
                    if (!Information.IsDBNull(oCompRS.Fields[sMCUFieldName].Value))
                    {
                        sMCUFieldValue = oCompRS.Fields[sMCUFieldName].Value.ToString();
                    }
                    else
                    {
                        sMCUFieldValue = string.Empty;
                    }
                }
                else
                {
                    //This is a standard CU.  Get the CU code.
                    if (!Information.IsDBNull(oCompRS.Fields[sCUFieldName].Value))
                    {
                        sCUFieldValue = oCompRS.Fields[sCUFieldName].Value.ToString();
                    }
                    else
                    {
                        sCUFieldValue = string.Empty;
                    }
                }

                //G3E_CID cannot be NULL so no need to check for NULL here.
                iCID = System.Convert.ToInt32(oCompRS.Fields["g3e_cid"].Value);

                if (!bMacroCU || bMacroCU) //This is to include deletion of a Macro CU corresponding to occurence, lets keep it like this at the moment until we discuss this further and then may be deleted. 
                {
                    //If the occurrence is < 0, then occurrence matching doesn't matter.
                    //Default it to True in this case so any matching CU record will be deleted/blanked.
                    if (0 > iOccurrence)
                    {
                        bOccurrenceMatches = true;
                    }
                    else
                    {
                        //If we have a valid occurrence, it has to match to remove/null the record.
                        bOccurrenceMatches = iOccurrence == iCID;
                    }
                }
                else
                {
                    //If deleting a MacroCU, then we don't need to honor Occurrence Matching.
                    bOccurrenceMatches = true;
                }

                if (1 < oCompRS.RecordCount)
                {
                    //If there is more than one record at this point,
                    //then we can safely delete a record
                    //regardless of whether the component is required.
                    if (bMacroCU)
                    {
                        //Look for match on macro CU field value.
                        if (sMCUFieldValue == (string)sCUCode && bOccurrenceMatches)
                        {
                            oCompRS.Delete((ADODB.AffectEnum)1);
                            oCompRS.MoveNext();
                            //When deleting a macro CU, remove any ancillary records belonging to it.
                            DeleteAncillaryMacroRecords(sCUCode, bPrimary);
                            //Delete any matching G3E_CID records on any auxilliary (RCU) component recordsets.
                            //  DeleteAuxilliaryComponentRecords(m_oKeyObject, iCID);
                        }
                    }
                    else
                    {
                        //Look for a match on the CU field value.
                        string sCU = null;
                        bool bCUMatching = false;
                        foreach (string tempLoopVar_sCU in sCUCode.Split(','))
                        {
                            sCU = tempLoopVar_sCU;
                            if (sCUFieldValue == sCU)
                            {
                                bCUMatching = true;
                                break;
                            }
                        }
                        if (bCUMatching && bOccurrenceMatches)
                        {
                            oCompRS.Delete((ADODB.AffectEnum)1);
                            oCompRS.MoveNext();
                            //Delete any matching G3E_CID records on any auxilliary (RCU) component recordsets.
                            // DeleteAuxilliaryComponentRecords(m_oKeyObject, iCID);
                        }
                    }
                }
                else
                {
                    //Getting to this point indicates there is exactly one record
                    //in the component's recordset.
                    //If the component is required, then only null the required fields;
                    //otherwise, delete the record.
                    if (bComponentIsRequired)
                    {
                        //If we're not on the last record, then we can delete the record.
                        if (bMacroCU)
                        {
                            //Test for a match on the macro field
                            if (sMCUFieldValue == (string)sCUCode && bOccurrenceMatches)
                            {
                                //This is the last record and it is required; therefore, just Null both fields.
                                oCompRS.Fields[sCUFieldName].Value = System.DBNull.Value;
                                oCompRS.Fields[sMCUFieldName].Value = System.DBNull.Value;
                                //When deleting a macro CU, remove any ancillary records belonging to it.
                                DeleteAncillaryMacroRecords(sCUCode, bPrimary);
                            }
                        }
                        else
                        {
                            //Test for a match on the CU field
                            if (sCUFieldValue == (string)sCUCode && bOccurrenceMatches)
                            {
                                //Null just the CU field.
                                oCompRS.Fields[sCUFieldName].Value = System.DBNull.Value;
                            }
                        } //If bMacroCU Then
                    }
                    else //If bComponentIsRequired Then
                    {
                        //The component is not required.  We can safely delete it.
                        if (bMacroCU)
                        {
                            //Test for a match on the macro field
                            if (sMCUFieldValue == (string)sCUCode && bOccurrenceMatches)
                            {
                                oCompRS.Delete((ADODB.AffectEnum)1);

                                if (!oCompRS.EOF)
                                {
                                    oCompRS.MoveNext();
                                }

                                //When deleting a macro CU, remove any ancillary records belonging to it.
                                DeleteAncillaryMacroRecords(sCUCode, bPrimary);
                            }
                        }
                        else
                        {
                            //Test for a match on the CU field
                            if (sCUFieldValue == (string)sCUCode && bOccurrenceMatches)
                            {
                                oCompRS.Delete((ADODB.AffectEnum)1);
                                oCompRS.MoveNext();
                            }
                        }
                    } //If bComponentIsRequired Then
                } //If 1 < oCompRS.RecordCount Then

                //Check for BOF as we may have removed the last record during the iteration.
                if (!oCompRS.BOF)
                {
                    oCompRS.MovePrevious();
                }

            }

            WrapUp:
            1.GetHashCode(); //VBConversions note: C# requires an executable line here, so a dummy line was added.

        }
        private void DeleteAncillaryMacroRecords(string sCUCode, bool bPrimary)
        {
            //Removes ancillary macro records that "belong" to the sCUCode macro.
            //An exception will be thrown if the oGTKeyObjects collection has more than one record
            //when this procedure is called.

            IGTComponent oComp = null;
            ADODB.Recordset oCompRS = null;
            System.String sMacroName = string.Empty;
            bool bComponentIsRequired = false;

            try
            {
                //Primary macro CUs that are expanded on the ancillary CU component
                //have their names enclosed in parentheses.  Add them here so the macro name will match.
                if (bPrimary)
                {
                    sMacroName = "(" + System.Convert.ToString(sCUCode) + ")";
                }
                else
                {
                    //Ancillary macro CUs do not have the parentheses.
                    sMacroName = System.Convert.ToString(sCUCode);
                }

                //If the component is required, then we have to avoid attempting to delete the last component record.
                bComponentIsRequired = IsComponentRequired(m_oKeyObject.FNO, System.Convert.ToInt32(m_iAncCompUnitMacroCNO));

                //Get a local component and component recordset
                oComp = m_oKeyObject.Components.GetComponent(System.Convert.ToInt16(m_iAncCompUnitMacroCNO));
                oCompRS = oComp.Recordset;

                if (oCompRS == null)
                {
                    goto WrapUp;
                }

                if (oCompRS.BOF && oCompRS.EOF)
                {
                    goto WrapUp;
                }
                if (oCompRS.EOF && oCompRS.RecordCount == 0)
                {
                    goto WrapUp;
                }
                oCompRS.MoveLast();
                while (!oCompRS.BOF)
                {
                    //If the macro name matches, then either delete the component record or null its fields
                    if (!Information.IsDBNull(oCompRS.Fields[m_sAncCompUnitMacroFieldName].Value))
                    {
                        if (sMacroName == oCompRS.Fields[m_sAncCompUnitMacroFieldName].Value.ToString())
                        {
                            if (!bComponentIsRequired)
                            {
                                //Component record can safely deleted.
                                oCompRS.Delete((ADODB.AffectEnum)1);
                                oCompRS.MoveNext();
                            }
                            else
                            {
                                if (1 < oCompRS.RecordCount)
                                {
                                    //Component is required but this is not the last
                                    //component record so okay to delete.
                                    oCompRS.Delete((ADODB.AffectEnum)1);
                                    oCompRS.MoveNext();
                                }
                                else
                                {
                                    //This is the last occurrence of a required component.
                                    //Just null the CU field values.
                                    oCompRS.Fields[m_sAncCompUnitMacroFieldName].Value = System.DBNull.Value;
                                    oCompRS.Fields[m_sAncCompUnitFieldName].Value = System.DBNull.Value;
                                }
                            }
                        }
                    }
                    //Check for BOF as we may have removed the last record during the iteration.
                    if (!oCompRS.BOF)
                    {
                        oCompRS.MovePrevious();
                    }
                }
            }
            catch (Exception ex)
            {
                throw (ex);
            }

            WrapUp:
            1.GetHashCode(); //VBConversions note: C# requires an executable line here, so a dummy line was added.

        }
        private void CheckDataContext()
        {
            Exception oEx = null;
            if (m_oDataContext == null)
            {
                oEx = new Exception("DataContext must be set before calling this method.");
                throw (oEx);
            }
        }

        private void GenerateCURecord(ADODB.Recordset oCompRS,
            bool bPrimary,
            int iCNO, string
            sMacroCU)
        {

            //Routine to initialize a CU Component Recordset
            //adding a new record if needed and leaving
            //the recordset positioned so that the CU information
            //can be set on that record in the calling procedure.

            System.Object sCUFieldName = null;
            System.Object sMCUFieldName = null;
            bool bCreateNew = false;
            System.Object sTmpMCU = null;
            System.Exception oEx = null;

            try
            {

                if (iCNO == m_iCompUnitCNO)
                {
                    sCUFieldName = m_sCompUnitFieldName;
                    sMCUFieldName = m_sCompUnitMacroFieldName;
                }
                else if (iCNO == m_iAncCompUnitCNO)
                {
                    sCUFieldName = m_sAncCompUnitFieldName;
                    sMCUFieldName = m_sAncCompUnitMacroFieldName;
                }
                else
                {
                    //An invalid CNO was received.
                    oEx = new System.Exception("Command asked to create CU record for non-CU component " + iCNO.ToString());
                    throw (oEx);
                }

                //bCreateNew = true;
                //goto endOfTry;
            
            
            if (0 == oCompRS.RecordCount || oCompRS.BOF && oCompRS.EOF)
            {
                bCreateNew = true;
                goto endOfTry;
            }

            //If either field name values are Nothing, then we cannot completely check an existing record.
            //In this case, generate a new record.
            if ((sCUFieldName == null) || (sMCUFieldName == null))
            {
                bCreateNew = true;
                goto endOfTry;
            }

            //If the last record is "empty", use it.
            oCompRS.MoveLast();

            if (m_bSignificantAncillary)
            {
                bCreateNew = true;
                goto endOfTry;
            }
            else
            {
                if (Information.IsDBNull(oCompRS.Fields[sCUFieldName].Value) && Information.IsDBNull(oCompRS.Fields[sMCUFieldName].Value))
                {
                    //Nothing else to do.
                    goto endOfTry;
                }

                //If the last CU field is not NULL, then we cannot re-use this record.
                if (!Information.IsDBNull(oCompRS.Fields[sCUFieldName].Value))
                {
                    bCreateNew = true;
                    goto endOfTry;
                }

                //If neither field is null, then create a new record.
                if (!Information.IsDBNull(oCompRS.Fields[sCUFieldName].Value) && !Information.IsDBNull(oCompRS.Fields[sMCUFieldName].Value))
                {
                    bCreateNew = true;
                    goto endOfTry;
                }

                //If both fields are not null, then we need to check the Macro CU field.
                //If the CU field is empty but the Macro CU field matches the current Macro CU,
                //the we can re-use this record.
                if (Information.IsDBNull(oCompRS.Fields[sCUFieldName].Value) && !Information.IsDBNull(oCompRS.Fields[sMCUFieldName].Value))
                {

                    sTmpMCU = oCompRS.Fields[sMCUFieldName].Value.ToString();

                    if (bPrimary && iCNO == m_iAncCompUnitCNO)
                    {
                        //If we're examining an Primary Macro CU for an Ancillary component,
                        //then we need to enclose the macro CU in parentheses.
                        sTmpMCU = "(" + System.Convert.ToString(sTmpMCU) + ")";
                    }

                    if (oCompRS.Fields[sMCUFieldName].Value.ToString() != (string)sTmpMCU)
                    {
                        //The CU field was empty but the macro CU value does not match the current macro value.
                        //Create a new record.
                        bCreateNew = true;
                        goto endOfTry;
                    }

                }
            }
        }
            catch (Exception ex)
            {
                oEx = new System.Exception("Error generating new CU component record." + "\n" + ex.Message);
                throw (oEx);
            }
            endOfTry:

            //We have a separate "Try" here to make use of the "Exit Try" logic
            //thus avoiding the above routine getting too messy with too many levels of "If-Then-Else" logic.
            try
            {
                if (bCreateNew)
                {
                    oCompRS.AddNew(Type.Missing, Type.Missing);
                    oCompRS.MoveLast();
                    //If (oCompRS.Status = ADODB.RecordStatusEnum.adRecNew) Then

                    //End If
                }
            }
            catch (Exception ex)
            {
                oEx = new System.Exception("Error generating new CU component record." + "\n" + ex.Message);
                throw (oEx);
            }

        }
        private void MoveRecordSetToCurrentCID(ADODB.Recordset oCompRS, int iCurrentCID)
        {
            if (oCompRS.RecordCount > 0)
            {
                oCompRS.MoveFirst();

                while (oCompRS.EOF == false)
                {
                    if (Convert.ToInt32(oCompRS.Fields["G3E_CID"].Value) == iCurrentCID)
                    {
                        break;
                    }
                    oCompRS.MoveNext();
                }
            }
        }
        private void SetDefaultCUFieldInfo(IGTKeyObject oKeyObj,
            bool bPrimary, string
            sCUType,
            int iCNO, string
            sCU, string
            sCUFieldName, string
            sMacroCU, string
            sMCUFieldName,
            int iQty, string strActivity, string sAMCU = null)
        {

            //Sets the default attribute values on a new CU record.
           
            IGTComponent oComp = null;
            ADODB.Recordset oCompRS = null;
            Exception oEx = null;

            try
            {
                oComp = oKeyObj.Components.GetComponent(System.Convert.ToInt16(iCNO));
                if (oComp == null)
                {
                    return;
                }

                oCompRS = oComp.Recordset;
                if (oCompRS == null)
                {
                    return;
                }

                if (oCompRS.BOF || oCompRS.EOF)
                {
                    return;
                }
                int iCurrentCID = Convert.ToInt32(oCompRS.Fields["g3e_cid"].Value);

                if ((oCompRS.Fields["g3e_fno"].Value == null) || Information.IsDBNull(oCompRS.Fields["g3e_fno"].Value))
                {
                    //This field will be null on a new record.
                    oCompRS.Fields["g3e_fno"].Value = oKeyObj.FNO;
                }
                if ((oCompRS.Fields["g3e_fid"].Value == null) || Information.IsDBNull(oCompRS.Fields["g3e_fid"].Value))
                {
                    //This field will be null on a new record.
                    oCompRS.Fields["g3e_fid"].Value = oKeyObj.FID;
                }

                if (m_bSignificantAncillary)
                {
                    oCompRS.Fields[sCUFieldName].Value = sCU;
                    //Temp Change - For some reason the recordset is getting set to first instance after updating the field so lets move it to the correct location again using the method MoveRecordSetToCurrentCID
                    MoveRecordSetToCurrentCID(oCompRS, iCurrentCID);
                    return;
                }

                //First set the UnitCID and UnitCNO 
                string sFieldName = "";
                //sFieldName = GetParameter("CU UnitCID Field Name", true).ToString();

                //if (!string.IsNullOrEmpty(m_ExistingUnitCID))
                //{
                //    oCompRS.Fields[sFieldName].Value = m_ExistingUnitCID; //if already exists as in case of Changeout operation, set UnitCID so that it need not to be handled through Set Default FI operation.
                //                                                          //The Set Standard Attributes FI will not set Unit CNO and Unit CID if it is already populated
                //}

                MoveRecordSetToCurrentCID(oCompRS, iCurrentCID);

                sFieldName = GetParameter("CU Unit CNO Field Name", true).ToString();

                if (!string.IsNullOrEmpty(m_ExistingUnitCNO))
                {
                    oCompRS.Fields[sFieldName].Value = m_ExistingUnitCNO; //if already exists as in case of Changeout operation, set UnitCNO
                }

                MoveRecordSetToCurrentCID(oCompRS, iCurrentCID);

                //We only set the CU value if it is a primary CU, primary macro CU, or ancillary CU.
                //Ancillary macro CUs will always get ancillary CU records generated for them
                //and they will set both the CU and MacroCU field values.
                if (M_SCU == (string)sCUType || M_SMCU == (string)sCUType || M_SACU == (string)sCUType)
                {
                    oCompRS.Fields[sCUFieldName].Value = sCU;
                    //Temp Change - For some reason the recordset is getting set to first instance after updating the field so lets move it to the correct location again using the method MoveRecordSetToCurrentCID
                    MoveRecordSetToCurrentCID(oCompRS, iCurrentCID);

                    if (sAMCU!=null && !sAMCU.Equals("*"))
                    {
                        oCompRS.Fields[sMCUFieldName].Value = sAMCU;
                    }
                    if (m_bChangeOutOperation && iCNO == 21)
                    {
                        oCompRS.Fields["REPLACED_CID"].Value = m_changeOutInstance;
                    }

                    MoveRecordSetToCurrentCID(oCompRS, iCurrentCID);

                    string sComponentName = iCNO == 21 ? "COMP_UNIT_N" : "COMP_UNIT_ANCIL_N";
                    SetCUAttributes(oKeyObj.Components, sComponentName, iCurrentCID);
                }

                if (!(sMacroCU == null))
                {
                    //If this is a primary macro and the current CU is an Ancillary,
                    //then enclose the macro CU name in parentheses to indicate it was
                    //generated as a result of a primary macro CU expansion.
                    if (bPrimary && iCNO == m_iAncCompUnitCNO)
                    {
                        oCompRS.Fields[sMCUFieldName].Value = "(" + System.Convert.ToString(sMacroCU) + ")";
                    }
                    else
                    {
                        oCompRS.Fields[sMCUFieldName].Value = sMacroCU;
                    }
                }
                //Keep this temporarily here but eventually needs to remove

                //Fix for the issue ONCORDEV-2014
                if (Convert.ToInt16(oCompRS.Fields["G3E_CNO"].Value) == 22)
                {
                    oCompRS.Fields[m_sCompUnitQtyFieldName].Value = iQty;

                    string sJobType;
                    bool bCorrectionMode;
                    GetCorrectionModeandJobType(out sJobType, out bCorrectionMode);

                    if (sJobType.Equals("WR-MAPCOR") || bCorrectionMode)
                    {
                        strActivity = strActivity + "C";
                    }                   

                    oCompRS.Fields["ACTIVITY_C"].Value = strActivity;
                }

                MoveRecordSetToCurrentCID(oCompRS, iCurrentCID);
           
            }
            catch (Exception)
            {
                oEx = new System.Exception("Error setting default CU field information. " + "\n" + oEx.Message);
                throw (oEx);
            }
        }

     
        private void SetCUAttributes(IGTComponents p_components, string p_componentName, int p_currentRecord)
        {
            if (m_bSignificantAncillary) return;
            CommonSetCUStandardAttributes oCUDefaultAttributes = new CommonSetCUStandardAttributes(p_components, p_componentName);
            oCUDefaultAttributes.SetCUAttributes();

            string  sFieldName = GetParameter("CU Activity Field Name", true).ToString();
            p_components[p_componentName].Recordset.Fields[sFieldName].Value = SetActivityONCUComponent();
            MoveRecordSetToCurrentCID(p_components[p_componentName].Recordset, p_currentRecord);
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
        private string SetActivityONCUComponent()
        {
            string sActivityCode = string.Empty;

            string sJobType;
            bool bCorrectionMode;
            GetCorrectionModeandJobType(out sJobType, out bCorrectionMode);

            if (sJobType.Equals("WR-MAPCOR") || bCorrectionMode)
            {
                sActivityCode = "IC";
            }
            else
            {
                sActivityCode = "I";
            }

            return sActivityCode;
        }

        private void GetCorrectionModeandJobType(out string sJobType, out bool bCorrectionMode)
        {
            ADODB.Recordset rs = m_oDataContext.OpenRecordset("select g3e_jobtype from g3e_job where G3E_IDENTIFIER =  ?", ADODB.CursorTypeEnum.adOpenDynamic, ADODB.LockTypeEnum.adLockReadOnly, (int)ADODB.CommandTypeEnum.adCmdText, m_oDataContext.ActiveJob);
            sJobType = string.Empty;
            IGTApplication oApp = GTClassFactory.Create<IGTApplication>();
            bCorrectionMode = false;
            if (rs != null)
            {
                if (rs.RecordCount > 0)
                {
                    rs.MoveFirst();
                    sJobType = Convert.ToString(rs.Fields[0].Value);
                }
            }

            for (int i = 0; i < oApp.Properties.Count; i++)
            {
                if (oApp.Properties.Keys[i].Equals("CorrectionsMode"))
                {
                    bCorrectionMode = true;
                    break;
                }
            }
        }

        private bool FieldIsEmpty(ADODB.Field oFld)
        {
            //Returns True if field value is DbNull or is Empty
            try
            {
                if (Information.IsDBNull(oFld.Value))
                {
                    return true;
                }
                else
                {
                    if (oFld.Value.ToString() == string.Empty)
                    {
                        return true;
                    }
                }
            }
            catch (Exception ex)
            {
                throw (ex);
            }
            return default(bool);
        }

    }
}
