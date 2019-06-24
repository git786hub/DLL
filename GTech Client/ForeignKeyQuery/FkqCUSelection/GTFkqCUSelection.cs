using System;
using System.Windows.Forms;
using Intergraph.GTechnology.Interfaces;
using Intergraph.GTechnology.API;
using static GTechnology.Oncor.CustomAPI.ValidationConditions;
using System.Collections.Generic;
using ADODB;

namespace GTechnology.Oncor.CustomAPI
{
    public class GTFkqCUSelection : IGTForeignKeyQuery
    {
        IGTDataContext m_oDataContext;
        private GTArguments m_GTArguments = null;
        IGTKeyObject m_oFeature;

        CUService m_oCUService;

        private string m_sTableName;
        private string m_sFieldName;
        private Boolean m_bReadOnly;
        private IGTFieldValue m_vOutputValue = null;
        private string m_sCategoryFilter;
        private string m_sCUTypeConfigured;
        private string m_sDialogNo;
        private bool m_sIsAggregateFeature;
        #region IGTForeignKeyQuery Members
        string m_sCUType = string.Empty;
        short m_CUCno = 0;
        bool m_bSignificantAncillary = false;
        int m_iANO = 0;
        string m_componentName = string.Empty;

        public GTArguments Arguments
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                m_GTArguments = value;
                m_sDialogNo = "";

                //First Argument - Indicator whether the CU is PRIMARY/ANCILLARY
                m_sCUTypeConfigured = m_GTArguments.GetArgument(0).ToString();

                //Second Argument - "Y" or "N" to indicate if the selected feature is an aggregate feature
                m_sIsAggregateFeature = m_GTArguments.GetArgument(1).ToString().Equals("Y") ? true : false;

                //Third Argument - Category Filter, mandatory for the PRIMARY, optional for ANCILLARY
                m_sCategoryFilter = m_GTArguments.Count > 2 ? m_GTArguments.GetArgument(2).ToString() : null;
                m_bSignificantAncillary = m_GTArguments.Count == 4 ? true : false;

                if (m_bSignificantAncillary)
                {
                    m_iANO = m_GTArguments.Count == 4 ? Convert.ToInt32(m_GTArguments.GetArgument(3)) : 0;
                    m_componentName = "COMP_UNIT_ANCIL_N";
                }               
            }
        }

        private string GetComponentName(int p_ANO)
        {
            string sComponentName = string.Empty;
            ADODB.Recordset rs = m_oApp.DataContext.MetadataRecordset("G3E_ATTRIBUTEINFO_OPTABLE");
            rs.Filter = "g3e_ano = " + p_ANO;

            if (rs!=null)
            {
                if (rs.RecordCount>0)
                {
                    rs.MoveFirst();
                    sComponentName = Convert.ToString(rs.Fields["G3E_NAME"].Value);
                }
            }
            return sComponentName;
        }

        public IGTDataContext DataContext
        {
            get
            {
                return m_oDataContext;
            }
            set
            {
                m_oDataContext = value;
            }
        }

        string m_sANO = string.Empty;
        string m_sCUano = string.Empty;

        IGTApplication m_oApp = GTClassFactory.Create<IGTApplication>();

        private bool MoveSignificantAncillaryRecordset()
        {
            bool bAncillaryRecordFound = false;


            if (m_oFeature.Components["COMP_UNIT_ANCIL_N"].Recordset != null)
            {
                if (m_oFeature.Components["COMP_UNIT_ANCIL_N"].Recordset.RecordCount > 0)
                {
                    m_oFeature.Components["COMP_UNIT_ANCIL_N"].Recordset.MoveFirst();

                    if (m_oFeature.CID != -1)
                    {
                        while (m_oFeature.Components["COMP_UNIT_ANCIL_N"].Recordset.EOF == false)
                        {
                            int ACU_ANO = m_oFeature.Components["COMP_UNIT_ANCIL_N"].Recordset.Fields["ACU_ANO"].Value.IsDBNull() ? 0 : Convert.ToInt32(m_oFeature.Components["COMP_UNIT_ANCIL_N"].Recordset.Fields["ACU_ANO"].Value);
                            int UNIT_CNO = m_oFeature.Components["COMP_UNIT_ANCIL_N"].Recordset.Fields["UNIT_CNO"].Value.IsDBNull() ? 0 : Convert.ToInt32(m_oFeature.Components["COMP_UNIT_ANCIL_N"].Recordset.Fields["UNIT_CNO"].Value);
                            int UNIT_CID = m_oFeature.Components["COMP_UNIT_ANCIL_N"].Recordset.Fields["UNIT_CID"].Value.IsDBNull() ? 0 : Convert.ToInt32(m_oFeature.Components["COMP_UNIT_ANCIL_N"].Recordset.Fields["UNIT_CID"].Value);

                            if (ACU_ANO == m_iANO && UNIT_CNO == m_oFeature.CNO && UNIT_CID == m_oFeature.CID)
                            {
                                bAncillaryRecordFound = true;
                                break;
                            }
                            else
                            {
                                m_oFeature.Components["COMP_UNIT_ANCIL_N"].Recordset.MoveNext();
                            }
                        }
                    }
                    // m_oFeature.Components["COMP_UNIT_ANCIL_N"].Recordset.
                    
                }
            }
            return bAncillaryRecordFound;
        }
        public bool Execute(object InputValue)
        {
            Int16 nCompNO = 0;
            string sFilterFieldName = "";
            string sCUKeyfield = "";
            string sCUKeyValue = "";
            Boolean flagCUModified = false;
            Boolean flagPrimaryCU = true;
            string sExistingCU = string.Empty;
            string sExistingActivity = string.Empty;
            string sUnitCNO = string.Empty;
            string sUnitCID = string.Empty;
            string sCNoFieldName = string.Empty;
            bool bSignificantRecordFound = false;

            // Implement this method.
            // This method will call CUService to set the CU values 

            try
            {
                m_oCUService = new CUService(m_iANO, m_bSignificantAncillary);
                m_oCUService.DataContext = m_oDataContext;
                m_oCUService.KeyObject = m_oFeature;

                if (m_sCUTypeConfigured == "PRIMARY")
                {
                    flagPrimaryCU = true;
                    m_sCUType = "CU";
                    m_sCUano = Convert.ToString(m_oCUService.CompUnitANO);
                    m_CUCno = (short)m_oCUService.CUComponentCNO;
                    if (!m_bSignificantAncillary)
                    {
                        m_componentName = "COMP_UNIT_N";
                    }
                }

                if (m_sCUTypeConfigured == "ANCILLARY")
                {
                    flagPrimaryCU = false;
                    m_sCUType = "ACU";
                    m_sCUano = Convert.ToString(m_oCUService.AncCompUnitANO);
                    m_CUCno = (short)m_oCUService.AncillaryCUComponentCNO;
                    m_componentName = "COMP_UNIT_ANCIL_N";
                }

                sCNoFieldName = m_oCUService.CUField;
                sCUKeyfield = "CU_TYPE";
                sCUKeyValue = m_sCUType;

                if (m_bReadOnly)
                {
                    if (m_oFeature.CID <= 0)
                    {
                        MessageBox.Show("Nothing to review at this time.", "GTechnology", MessageBoxButtons.OK);
                    }
                    else
                    {
                        //Show More Info Form here
                        CuCommonForms CuComomonForm = new CuCommonForms(m_oApp);
                        CuComomonForm.ShowMoreInfoForm(Convert.ToString(m_oFeature.Components.GetComponent(m_CUCno).Recordset.Fields[sCNoFieldName].Value), true, true);
                    }

                    return ProcessCUUpdate(flagCUModified, ref nCompNO, ref sFilterFieldName); ;
                }

                ValidationConditions oValidateCUConditions = new ValidationConditions(m_oFeature, m_oDataContext, m_sCategoryFilter, m_sCUTypeConfigured, m_sIsAggregateFeature, m_componentName);

                CUAction CuActionSelected = CUAction.Selection;

                if (m_bSignificantAncillary)
                {
                    bSignificantRecordFound =  MoveSignificantAncillaryRecordset();
                }

                CuActionSelected = oValidateCUConditions.GetCUAction(m_oFeature.Components.GetComponent(m_CUCno).Recordset.RecordCount > 0 ? Convert.ToString(m_oFeature.Components.GetComponent(m_CUCno).Recordset.Fields[sCNoFieldName].Value) : "");   //Identify the CU Action based on the conditions

                if (m_oFeature.Components.GetComponent(m_CUCno).Recordset.RecordCount > 0)
                {
                    sExistingCU = Convert.ToString(m_oFeature.Components.GetComponent(m_CUCno).Recordset.Fields[sCNoFieldName].Value);
                }

                RemovalActivity removalActivitySelectedOnChageOutForm = RemovalActivity.NoValue;
                InstallActivity installActivitySelectedOnChageOutForm = InstallActivity.Novalue;

                if (CuActionSelected.Equals(CUAction.NoActionWRMismatches))
                {
                    m_oApp.SetStatusBarText(GTStatusPanelConstants.gtaspcMessage, "Cannot alter a CU installed by a different WR");
                   // MessageBox.Show("Cannot alter a CU installed by a different WR", "GTechnology", MessageBoxButtons.OK);
                    return true;
                }
                else if (CuActionSelected.Equals(CUAction.NoAction))
                {
                    m_oApp.SetStatusBarText(GTStatusPanelConstants.gtaspcMessage, "Cannot edit this CU code.");
                    //MessageBox.Show("Cannot edit this CU code.", "GTechnology", MessageBoxButtons.OK);
                    return true;
                }

                if (!oValidateCUConditions.ValidateCUSelection(CuActionSelected,m_bSignificantAncillary,bSignificantRecordFound)) return true; //Validation conditions do not match and therefore return from here               

                if (CuActionSelected.Equals(CUAction.Changeout)) //This is a changeout operation
                {
                    //ALM-1592-- Added a method to disable the "Replace with same CU" option if the CU is not found
                    bool bReplacewithSameCU = ReplacewithSameCU(sExistingCU, m_sCategoryFilter);

                    CuCommonForms CuComomonForm = new CuCommonForms(m_oApp , bReplacewithSameCU);
                    CuComomonForm.ShowChangeOutForm();
                    removalActivitySelectedOnChageOutForm = CuComomonForm.RemovalActivitySelected;
                    installActivitySelectedOnChageOutForm = CuComomonForm.InstallActivitySelected;

                    if (CuComomonForm.CancelClicked) return true; //Cancel is clicked on Changeout Form so No Action required

                    if (installActivitySelectedOnChageOutForm == InstallActivity.DoNotInstall)
                    {
                        //Handle the setting of Removal Activity and return as there is no new installation needed
                        m_oFeature.Components.GetComponent(m_CUCno).Recordset.Fields["ACTIVITY_C"].Value = (removalActivitySelectedOnChageOutForm == RemovalActivity.Remove ? "R" : "S");
                        m_oFeature.Components.GetComponent(m_CUCno).Recordset.Fields["WR_EDITED"].Value = m_oApp.DataContext.ActiveJob;
                        return ProcessCUUpdate(flagCUModified, ref nCompNO, ref sFilterFieldName);
                    }

                    if (installActivitySelectedOnChageOutForm == InstallActivity.Replace || installActivitySelectedOnChageOutForm == InstallActivity.Select)
                    {
                        sExistingCU = Convert.ToString(m_oFeature.Components.GetComponent(m_CUCno).Recordset.Fields[sCNoFieldName].Value);
                        sUnitCID = Convert.ToString(m_oFeature.Components.GetComponent(m_CUCno).Recordset.Fields["UNIT_CID"].Value);
                        sUnitCNO = Convert.ToString(m_oFeature.Components.GetComponent(m_CUCno).Recordset.Fields["UNIT_CNO"].Value);
                        m_oFeature.Components.GetComponent(m_CUCno).Recordset.Fields["WR_EDITED"].Value = m_oApp.DataContext.ActiveJob;
                        sExistingActivity = Convert.ToString(m_oFeature.Components.GetComponent(m_CUCno).Recordset.Fields["ACTIVITY_C"].Value);
                        m_oFeature.Components.GetComponent(m_CUCno).Recordset.Fields["ACTIVITY_C"].Value = (removalActivitySelectedOnChageOutForm == RemovalActivity.Remove ? "R" : "S");
                    }
                }


                if (m_oFeature.CID <= 0) //This is a clear case of Selection, Proceed as usual
                {
                    if (DialogResult.OK == m_oCUService.Selection(flagPrimaryCU, sCUKeyfield, sCUKeyValue, m_sCategoryFilter)) flagCUModified = true;
                }
                else
                {
                    if (DialogResult.OK == m_oCUService.Selection(flagPrimaryCU, m_oFeature.CID, sCUKeyfield, sCUKeyValue, m_sCategoryFilter, removalActivitySelectedOnChageOutForm, installActivitySelectedOnChageOutForm, sExistingCU, sExistingActivity, sUnitCID, sUnitCNO)) flagCUModified = true;
                }
                return ProcessCUUpdate(flagCUModified, ref nCompNO, ref sFilterFieldName);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error occured in CU Selection Foreign Query " + ex.Message, "G/Technology");
                return false;
            }
        }

        private bool ReplacewithSameCU(string strCucode ,string strCategory)
        {
            ADODB.Recordset rs = null;
            string[] sArrCategories = null;
            string sSql = "";
            try
            {
                if (!string.IsNullOrEmpty(strCategory))
                {
                    sArrCategories = strCategory.Split(',');
                }

                sSql = "select count(*) from CULIB_UNIT  where CU_ID ='"+ strCucode + "' and  category_c in (";

                for (int i = 0; i < sArrCategories.Length; i++)
                {
                    if (i != sArrCategories.Length - 1)
                    {
                        sSql = sSql + "'" + sArrCategories[i] + "'" + ",";
                    }
                    else
                    {
                        sSql = sSql + "'" + sArrCategories[i] + "'" + ")";
                    }
                }

                rs = m_oDataContext.OpenRecordset(sSql, ADODB.CursorTypeEnum.adOpenStatic, ADODB.LockTypeEnum.adLockReadOnly, (int)ADODB.CommandTypeEnum.adCmdText, (int)0);

                rs.MoveFirst();
                if (Convert.ToString(rs.Fields[0].Value).Equals("0"))
                    return false;
                
            }
            catch
            {
                throw;
            }
            finally
            {
                rs.Close();
                rs = null;
            }

            return true;
        }
        private bool ProcessCUUpdate(bool flagCUModified, ref short nCompNO, ref string sFilterFieldName)
        {

            string sCUFieldName = "";
            int changeOutCid = m_oFeature.CID;
            if (flagCUModified)
            {
                IGTFeatureExplorerService mFEservice = GTClassFactory.Create<IGTFeatureExplorerService>();

                if (GetCompFieldNameByANO(m_sCUano, m_oFeature.FNO, ref nCompNO, ref sCUFieldName)) //As the CU field is same for Primary or Ancillary component, there is no difference whether we pass Primary CU ANO or Ancillary CU ANO
                {
                    if (!(m_oFeature.Components.GetComponent(nCompNO).Recordset == null))
                    {
                        // FeatureExplorerService is called to refresh all Dialog tabs of the feature
                        string stablename;
                        string sDiaType;



                        stablename = GetPrimaryComponentTable(m_oFeature.FNO);
                        if (IsPlacementMode(m_oFeature.FNO, m_oFeature.FID, stablename))
                        {
                            if (m_sDialogNo == "")
                            {
                                mFEservice.ExploreFeature(m_oFeature, "Placement");
                            }
                            else
                            {
                                sDiaType = GetDialogType(m_sDialogNo);
                                mFEservice.ExploreFeature(m_oFeature, sDiaType);
                            }
                        }
                        else
                        {
                            mFEservice.ExploreFeature(m_oFeature, "Edit");
                            CopyPhaseAndPositionFromChangeoutInstance(changeOutCid);
                            mFEservice.ExploreFeature(m_oFeature, "Edit");
                        }
                    }
                }

                return true;
            }
            else
                return false;
        }


        private void CopyPhaseAndPositionFromChangeoutInstance(int changeOutInstance)
        {
            try
            {
                string phase = string.Empty;
                string phasePosition = string.Empty;
                string isNeutral = string.Empty;

                short cno = GetUnitOrBankAttributeCNoForFNo(m_oFeature.FNO);

                if (cno == 0)
                    return;
                Recordset attributeRS = m_oDataContext.OpenFeature(m_oFeature.FNO, m_oFeature.FID).Components.GetComponent(cno).Recordset;

                if (attributeRS != null && attributeRS.RecordCount > 0)
                {
                    attributeRS.Filter = "G3E_CID = " + changeOutInstance;

                    if (!(attributeRS.EOF && attributeRS.BOF))
                    {
                        try
                        {
                            phase = Convert.ToString(attributeRS.Fields["PHASE_C"].Value);
                            phasePosition = Convert.ToString(attributeRS.Fields["PHASE_POS_C"].Value);
                        }
                        catch (Exception)
                        { }

                        try
                        {
                            isNeutral = Convert.ToString(attributeRS.Fields["NEUTRAL_YN"].Value);
                        }
                        catch (Exception)
                        { }

                    }
                    attributeRS.Filter = FilterGroupEnum.adFilterNone;

                    attributeRS.MoveFirst();
                    attributeRS.MoveLast();
                    if (!(attributeRS.EOF && attributeRS.BOF))
                    {
                        try
                        {
                            attributeRS.Fields["PHASE_C"].Value = phase;
                            attributeRS.Fields["PHASE_POS_C"].Value = phasePosition;
                        }
                        catch (Exception)
                        { }

                        try
                        {
                            attributeRS.Fields["NEUTRAL_YN"].Value = isNeutral;
                        }
                        catch
                        { }

                    }
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        private short GetUnitOrBankAttributeCNoForFNo(short fno)
        {
            short cno = 0;
            try
            {
                Dictionary<short, short> FnoCnoPair = new Dictionary<short, short>();
                FnoCnoPair.Add(34, 3402);
                FnoCnoPair.Add(4, 401);
                FnoCnoPair.Add(8, 802);
                FnoCnoPair.Add(84, 802);
                FnoCnoPair.Add(9, 902);
                FnoCnoPair.Add(85, 902);
                FnoCnoPair.Add(11, 1102);
                FnoCnoPair.Add(87, 1102);
                FnoCnoPair.Add(38, 1102);
                FnoCnoPair.Add(88, 1102);
                FnoCnoPair.Add(63, 5302);
                FnoCnoPair.Add(97, 5302);
                FnoCnoPair.Add(53, 5302);
                FnoCnoPair.Add(96, 5302);
                FnoCnoPair.Add(161, 16102);
                FnoCnoPair.Add(86, 16102);
                FnoCnoPair.Add(59, 5902);
                FnoCnoPair.Add(98, 5902);
                FnoCnoPair.Add(37, 3701);
                FnoCnoPair.Add(23, 75);
                FnoCnoPair.Add(21, 75);
                FnoCnoPair.Add(22, 75);
                FnoCnoPair.Add(13, 1302);
                FnoCnoPair.Add(89, 1302);
                FnoCnoPair.Add(39, 1302);
                FnoCnoPair.Add(90, 1302);

                if (FnoCnoPair.TryGetValue(fno, out cno))
                {
                    return cno;
                }

                return cno;

            }
            catch (Exception)
            {
                throw;
            }
        }

        public IGTKeyObject Feature
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                m_oFeature = value;
            }
        }

        public string FieldName
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                m_sFieldName = value;
            }
        }

        public IGTFieldValue OutputValue
        {
            get
            {
                return m_vOutputValue;
            }
            set
            {

            }
        }

        public bool ReadOnly
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                m_bReadOnly = value;
            }
        }

        public string TableName
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                m_sTableName = value;
            }
        }

        #endregion

        #region Support Members

        private string GetDialogType(string m_sDialogNo)
        {
            ADODB.Recordset tmpDiaRS;
            string sDialogType;

            sDialogType = "";
            tmpDiaRS = m_oDataContext.MetadataRecordset("G3E_DIALOGS_OPTABLE");
            tmpDiaRS.Filter = "G3E_DNO=" + m_sDialogNo.ToString();

            if (!(tmpDiaRS.EOF && tmpDiaRS.BOF))
                sDialogType = tmpDiaRS.Fields["G3E_TYPE"].Value.ToString();
            return sDialogType;
        }

        private Boolean IsPlacementMode(int nFNO, int nFID, string stablename)
        {

            ADODB.Recordset tmprs;
            string sSql;

            Boolean returnvalue = false;

            sSql = "select count(*) from " + stablename + " where g3e_fno = " + nFNO.ToString() + " and g3e_fid = " + nFID.ToString();
            tmprs = m_oDataContext.OpenRecordset(sSql, ADODB.CursorTypeEnum.adOpenStatic, ADODB.LockTypeEnum.adLockReadOnly, (int)ADODB.CommandTypeEnum.adCmdText, (int)0);

            if (!(tmprs.EOF && tmprs.BOF))
            {
                tmprs.MoveFirst();
                if (Convert.ToInt32(tmprs.Fields[0].Value) > 0)
                    returnvalue = false;
                else
                    returnvalue = true;
            }
            else
                returnvalue = true;

            return returnvalue;
        }

        private string GetPrimaryComponentTable(int nFNO)
        {
            int nCompNO;
            ADODB.Recordset tmpFrs;
            ADODB.Recordset tmpFCrs;

            string stablename;
            stablename = "";

            tmpFrs = m_oDataContext.MetadataRecordset("G3E_FEATURES_OPTABLE");
            tmpFrs.Filter = "G3E_FNO=" + nFNO.ToString();

            if (!(tmpFrs.EOF && tmpFrs.BOF))
            {
                nCompNO = Convert.ToInt32(tmpFrs.Fields["G3E_PRIMARYATTRIBUTECNO"].Value);

                tmpFCrs = m_oDataContext.MetadataRecordset("G3E_FEATURECOMPS_OPTABLE");
                tmpFCrs.Filter = "G3E_CNO=" + nCompNO.ToString() + " AND " + "G3E_FNO=" + nFNO.ToString();
                if (!(tmpFCrs.EOF && tmpFCrs.BOF))
                {
                    stablename = tmpFCrs.Fields["G3E_TABLE"].Value.ToString();
                }
            }
            return stablename;
        }

        private Boolean GetCompFieldNameByANO(string sANO, int nFNO, ref Int16 nCompNO, ref string sFieldName)
        {
            ADODB.Recordset tmpArs;
            ADODB.Recordset tmpFCrs;
            tmpArs = m_oDataContext.MetadataRecordset("G3E_ATTRIBUTEINFO_OPTABLE");

            tmpArs.Filter = "G3E_ANO=" + sANO.ToString();

            if (!(tmpArs.EOF && tmpArs.BOF))
            {
                sFieldName = tmpArs.Fields["G3E_FIELD"].Value.ToString();
                nCompNO = Convert.ToInt16(tmpArs.Fields["G3E_CNO"].Value.ToString());

                tmpFCrs = m_oDataContext.MetadataRecordset("G3E_FEATURECOMPS_OPTABLE");
                tmpFCrs.Filter = "G3E_CNO=" + nCompNO.ToString() + " AND " + "G3E_FNO=" + nFNO.ToString();
                if (!(tmpFCrs.EOF && tmpFCrs.BOF))
                {
                    return true;
                }
            }

            return false;
        }
        #endregion
    }
}
