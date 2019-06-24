using Intergraph.GTechnology.API;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

//----------------------------------------------------------------------------+
//  Class: ValidationConditions
//  Description: This class is responsible to carry out ONCOR specific validation conditions check
//               for CU selection      
//----------------------------------------------------------------------------+
//  $Author:: Shubham Agarwal                                                       $
//  $Date:: 20/10/17                                                                $
//  $Revision:: 1                                                                   $
//----------------------------------------------------------------------------+

namespace GTechnology.Oncor.CustomAPI
{
    public class ValidationConditions
    {
        List<int> m_listBlanketUnitization;
        private IGTKeyObject m_oKeyObject;
        private IGTDataContext m_odataContext;
        private string m_category = string.Empty;
        private CUAction m_CuAction;
        private string m_cuType;
        private bool m_isAggregateFeature = false;
        private string m_ComponentName = string.Empty;

        public ValidationConditions(IGTKeyObject p_keyObject, IGTDataContext p_dataContext, string p_category, string p_CUType, bool p_isAggregateFeature, string p_ComponentName)
        {
            m_listBlanketUnitization = new List<int>();
            m_oKeyObject = p_keyObject;
            m_odataContext = p_dataContext;
            m_category = p_category;
            m_cuType = p_CUType;
            m_isAggregateFeature = p_isAggregateFeature;
            m_ComponentName = p_ComponentName;
        }

        private bool IsCorrectionJob()
        {
            bool bReturn = false;

            try
            {
                ADODB.Recordset rs = m_odataContext.OpenRecordset("select G3E_JOBTYPE from g3e_job where g3e_identifier = ?", ADODB.CursorTypeEnum.adOpenDynamic, ADODB.LockTypeEnum.adLockOptimistic, (int)ADODB.CommandTypeEnum.adCmdText, m_odataContext.ActiveJob);

                rs.MoveFirst();
                if (Convert.ToString(rs.Fields["G3E_JOBTYPE"].Value).Equals("NON-WR"))
                {
                    bReturn = true;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return bReturn;
        }

        public enum CUAction
        {
            Selection, Changeout, NoActionWRMismatches, NoAction
        }
        public CUAction GetCUAction(string p_existingCU)
        {
            CUAction bReturn = CUAction.Changeout;

            try
            {
                if (string.IsNullOrEmpty(p_existingCU))
                {
                    bReturn = CUAction.Selection;
                }

                else if (m_oKeyObject.CID <= 0 || (m_oKeyObject.Components[m_ComponentName].Recordset.RecordCount==0))
                {
                    bReturn = CUAction.Selection;
                }

                else if (Convert.ToString(m_oKeyObject.Components[m_ComponentName].Recordset.Fields["ACTIVITY_C"].Value).Equals("I") || Convert.ToString(m_oKeyObject.Components[m_ComponentName].Recordset.Fields["ACTIVITY_C"].Value).Equals("IC"))
                {
                    if ((Convert.ToString(m_oKeyObject.Components[m_ComponentName].Recordset.Fields["WR_ID"].Value).Equals(m_odataContext.ActiveJob) ==true) || (Convert.ToString(m_oKeyObject.Components[m_ComponentName].Recordset.Fields["WR_ID"].Value).Equals("0") ==true) ||(m_oKeyObject.Components[m_ComponentName].Recordset.Fields["WR_ID"].Value.IsDBNull() ==true)) //Need to change this field
                    {
                        bReturn = CUAction.Selection;
                    }
                    else
                    {
                        bReturn = CUAction.NoActionWRMismatches;
                    }
                }

                else if (m_isAggregateFeature && m_ComponentName != "COMP_UNIT_ANCIL_N")
                {
                    bReturn = CUAction.Changeout;
                }

                else
                {
                    bReturn = CUAction.NoAction;
                }

                m_CuAction = bReturn;
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return bReturn;
        }
        public bool ValidateCUSelection(CUAction cuAction, bool p_SignificantAncillary, bool p_SignificantAncillaryFound)
        {
            m_SignificantAncillary = p_SignificantAncillary;
            m_SignificantRecordFound = p_SignificantAncillaryFound;

            return ValidateFeature(cuAction);
        }

        private string GetLatestEditedByWR(bool p_Latest)
        {
            string sReturnEditedByWR = string.Empty;
            ADODB.Recordset rs = null;
            string sSql = string.Empty;

            try
            {
                if (p_Latest == true)
                {
                    sSql = "select G3E_IDENTIFIER from ASSET_HISTORY where G3E_FID =? order by CHANGE_DATE desc";
                    rs = m_odataContext.OpenRecordset(sSql, ADODB.CursorTypeEnum.adOpenDynamic, ADODB.LockTypeEnum.adLockOptimistic, (int)ADODB.CommandTypeEnum.adCmdText, m_oKeyObject.FID);

                    if (rs != null)
                    {
                        if (rs.RecordCount > 0)
                        {
                            rs.MoveFirst();
                            sReturnEditedByWR = Convert.ToString(rs.Fields["G3E_IDENTIFIER"].Value);
                        }
                        else //The feature is not posted so return the active job as the most recently edited by
                        {
                            sReturnEditedByWR = m_odataContext.ActiveJob;
                        }
                    }
                }
                else
                {
                    //string sComponentName = m_cuType == "PRIMARY" ? "COMP_UNIT_N" : "COMP_UNIT_ANCIL_N";
                    if (m_SignificantAncillary)
                    {
                        if (m_SignificantRecordFound)
                        {
                            sReturnEditedByWR = m_oKeyObject.Components[m_ComponentName].Recordset.RecordCount > 0 ? Convert.ToString(m_oKeyObject.Components[m_ComponentName].Recordset.Fields["WR_EDITED"].Value) : "";
                        }
                    }
                    else
                    {
                        sReturnEditedByWR = m_oKeyObject.Components[m_ComponentName].Recordset.RecordCount > 0 ? Convert.ToString(m_oKeyObject.Components[m_ComponentName].Recordset.Fields["WR_EDITED"].Value) : "";
                    }
                    
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return sReturnEditedByWR;
        }

        private bool IsEditedByDifferentWR(bool p_Latest)
        {
            bool bReturn = false;
            string sEditedByDate = string.Empty;
            sEditedByDate = GetLatestEditedByWR(p_Latest);

            if (!string.IsNullOrEmpty(sEditedByDate) && !sEditedByDate.Equals(m_odataContext.ActiveJob) && !(sEditedByDate.Equals("0")))
            {
                bReturn = true;
            }

            return bReturn;
        }

        bool m_SignificantAncillary = false;
        bool m_SignificantRecordFound = false;

        private bool ValidateFeature(CUAction cuAction)
        {
            bool bReturnValue = true;

            try
            {
                if (!IsCorrectionJob() && IsEditedByDifferentWR(false))
                {
                    MessageBox.Show("This CU is being edited by another WR.", "CU Selection", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return false;
                }

                if (!IsCorrectionJob() && (GetFeatureState().Equals("PPI") || GetFeatureState().Equals("ABI")) && IsEditedByDifferentWR(true))
                {
                    MessageBox.Show("Proposed install features must be transitioned to INI before they can be modified; contact Support for assistance.", "CU Selection", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return false;
                }

                if (!IsCorrectionJob() && (!GetFeatureState().Equals("PPI") && !GetFeatureState().Equals("ABI") && !GetFeatureState().Equals("INI") && !GetFeatureState().Equals("CLS") && !GetFeatureState().Equals("PPX") && !GetFeatureState().Equals("ABX")))
                {
                    MessageBox.Show("Cannot edit the CU code for a feature in this state", "CU Selection", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return false;
                }

                if (!IsCorrectionJob() && cuAction == CUAction.Changeout && !m_isAggregateFeature)
                {
                    MessageBox.Show("Changeouts are only valid on aggregate features", "CU Selection", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return false;
                }

                if (IsCorrectionJob())
                {
                    MessageBox.Show("CUs may only be modified in a WR job", "CU Selection", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return false;
                }

                if (string.IsNullOrEmpty(m_category) && m_cuType.Equals("PRIMARY"))
                {
                    MessageBox.Show("Primary CU selection must be configured to be filtered by category; notify system administrator.", "CU Selection", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return false;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return bReturnValue;
        }
        private string GetFeatureState()
        {
            string sFeatureState = string.Empty;
            try
            {
                m_oKeyObject.Components["COMMON_N"].Recordset.MoveFirst();
                sFeatureState = Convert.ToString(m_oKeyObject.Components["COMMON_N"].Recordset.Fields["FEATURE_STATE_C"].Value);
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return sFeatureState;
        }
    }
}
