//----------------------------------------------------------------------------+
//        Class: fiVirtualPointAssociation
//  Description: This functional interface updates the status attributes for a temporary 
//                virtual point when its association is established.
//----------------------------------------------------------------------------+
//     $Author:: pnlella                                                     $
//       $Date:: 15/04/2019                                                    $
//   $Revision:: 1                                                            $
//----------------------------------------------------------------------------+
//    $History:: fiVirtualPointAssociation.cs                                 $
//----------------------------------------------------------------------------+

using System;
using ADODB;
using System.Windows.Forms;
using Intergraph.GTechnology.Interfaces;
using Intergraph.GTechnology.API;
using System.Collections.Generic;

namespace GTechnology.Oncor.CustomAPI
{
    public class fiVirtualPointAssociation : IGTFunctional
    {
        #region Private Members
        private GTArguments m_GTArguments = null;
        private IGTDataContext m_GTDataContext = null;
        private string m_gCompName = string.Empty;
        private IGTComponents m_gComps = null;
        private IGTFieldValue m_gFieldVal = null;
        private string m_gFieldName = string.Empty;
        private GTFunctionalTypeConstants m_gFIType;

        private short m_gtAssociatedFNO = 0;
        private int m_gtAssociatedFID = 0;
        private string m_gtFeatureState = null;

        #endregion

        #region IGTFunctional Members

        public GTArguments Arguments
        {
            get
            {
                return m_GTArguments;
            }
            set
            {
                m_GTArguments = value;
            }
        }
        public string ComponentName
        {
            get
            {
                return m_gCompName;
            }
            set
            {
                m_gCompName = value;
            }
        }
        public IGTComponents Components
        {
            get
            {
                return m_gComps;
            }
            set
            {
                m_gComps = value;
            }
        }
        public IGTDataContext DataContext
        {
            get
            {
                return m_GTDataContext;
            }
            set
            {
                m_GTDataContext = value;
            }
        }

        public void Delete()
        {

        }

        public void Execute()
        {
            try
            {
                Recordset rsVPAttributes = m_gComps[m_gCompName].Recordset;

                if (ValidateVirtualPoint(rsVPAttributes))
                {
                    IGTKeyObject associatedFeature = DataContext.OpenFeature(m_gtAssociatedFNO, m_gtAssociatedFID);

                    IGTComponent associatedCommonComp = associatedFeature.Components.GetComponent(1);

                    if (associatedCommonComp != null)
                    {
                        if (associatedCommonComp.Recordset != null && associatedCommonComp.Recordset.RecordCount > 0)
                        {
                            associatedCommonComp.Recordset.MoveFirst();
                            SetFieldsOfVirtualPoints(Convert.ToString(associatedCommonComp.Recordset.Fields["FEATURE_STATE_C"].Value),
                                associatedFeature.Components.GetComponent(11));
                        }
                    }

                    //SetNormalAsOperatedStatusOfVirtualPoints();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("There is an error in \"Virtual Point Association\" Funtional Interface \n" + ex.Message, "G/Technology");
            }
        }

        public string FieldName
        {
            get
            {
                return m_gFieldName;
            }
            set
            {
                m_gFieldName = value.ToString();
            }
        }

        public IGTFieldValue FieldValueBeforeChange
        {
            get
            {
                return m_gFieldVal;
            }
            set
            {
                m_gFieldVal = value;
            }
        }

        public GTFunctionalTypeConstants Type
        {
            get
            {
                return m_gFIType;
            }
            set
            {
                m_gFIType = value;
            }
        }

        public void Validate(out string[] ErrorPriorityArray, out string[] ErrorMessageArray)
        {
            ErrorPriorityArray = null;
            ErrorMessageArray = null;
        }
        #endregion

        #region Private Methods

        /// <summary>
        /// Method to Validate for the valid Virtual point to update the attributes from associated feature.
        /// </summary>
        private bool ValidateVirtualPoint(Recordset p_rsVPAttributes)
        {
            try
            {
                if (p_rsVPAttributes != null)
                {
                    if (p_rsVPAttributes.RecordCount > 0)
                    {
                        p_rsVPAttributes.MoveFirst();

                        if (Convert.ToString(p_rsVPAttributes.Fields["PERMANENT_YN"].Value) == "Y")
                        {
                            return false;
                        }

                        m_gtAssociatedFID = Convert.ToInt32(p_rsVPAttributes.Fields["ASSOCIATED_FID"].Value == DBNull.Value ? 0 : p_rsVPAttributes.Fields["ASSOCIATED_FID"].Value);

                        if (m_gtAssociatedFID != 0)
                        {
                            if (!CheckForAssociatedFIDFeature())
                            {
                                return false;
                            }
                        }
                        else
                        {
                            return false;
                        }
                    }
                }
                return true;
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Method to check the feature existence identified by the Associated FID attribute.
        /// </summary>
        private bool CheckForAssociatedFIDFeature()
        {
            bool checkAssociatedFID = false;
            try
            {
                Recordset commonRS = DataContext.OpenRecordset(String.Format("SELECT G3E_FNO FROM COMMON_N WHERE G3E_FID = '{0}'", m_gtAssociatedFID), CursorTypeEnum.adOpenStatic,
                                       LockTypeEnum.adLockReadOnly, (int)CommandTypeEnum.adCmdText);

                if (commonRS != null && commonRS.RecordCount > 0)
                {
                    commonRS.MoveFirst();
                    checkAssociatedFID = true;
                    m_gtAssociatedFNO = Convert.ToInt16(commonRS.Fields["G3E_FNO"].Value);
                }

                commonRS = null;
            }
            catch
            {
                throw;
            }
            return checkAssociatedFID;
        }

        /// <summary>
        /// Method to set feature state of the virtual point to match the associated feature.
        /// </summary>
        private void SetFieldsOfVirtualPoints(String p_associatedFeatureState,IGTComponent p_gTAssociateComponent)
        {
            string strActiveFeatureState = "";
            string strActiveFeatureNormalState = "";
            string strActiveFeatureOperaState = "";

            List<string> installedFs = new List<string> { "PPI", "ABI" };
            List<string> removedFs = new List<string> { "PPR", "ABR", "PPA", "ABA" };

            try
            {                
                Recordset vpCommonRs = m_gComps["COMMON_N"].Recordset;

                if (Convert.ToInt16(vpCommonRs.Fields["G3E_FNO"].Value) == 40 || Convert.ToInt16(vpCommonRs.Fields["G3E_FNO"].Value) == 80)
                {
                    if (p_associatedFeatureState == "PPI")
                    {
                        strActiveFeatureState = "PPR";
                        strActiveFeatureNormalState = "OPEN";
                        strActiveFeatureOperaState = "CLOSED";
                    }
                    else if (p_associatedFeatureState == "ABI")
                    {
                        strActiveFeatureState = "ABR";
                        strActiveFeatureNormalState = "OPEN";
                        strActiveFeatureOperaState = "CLOSED";
                    }
                    else if (p_associatedFeatureState == "PPR")
                    {
                        strActiveFeatureState = "PPI";
                        strActiveFeatureNormalState = "CLOSED";
                        strActiveFeatureOperaState = "OPEN";
                    }
                    else if (p_associatedFeatureState == "ABR")
                    {
                        strActiveFeatureState = "ABI";
                        strActiveFeatureNormalState = "CLOSED";
                        strActiveFeatureOperaState = "OPEN";
                    }
                }
                else if (Convert.ToInt16(vpCommonRs.Fields["G3E_FNO"].Value) == 6 || Convert.ToInt16(vpCommonRs.Fields["G3E_FNO"].Value) == 82)
                {
                    if (p_associatedFeatureState == "PPI" || p_associatedFeatureState == "ABI")
                    {
                        strActiveFeatureState = p_associatedFeatureState;
                        strActiveFeatureNormalState = "CLOSED";
                        strActiveFeatureOperaState = "OPEN";
                    }                    
                }


                if (vpCommonRs != null && vpCommonRs.RecordCount > 0)
                {
                    if (!(vpCommonRs.EOF && vpCommonRs.BOF))
                    {
                        vpCommonRs.MoveFirst();

                        vpCommonRs.Fields["FEATURE_STATE_C"].Value = strActiveFeatureState;
                        vpCommonRs.Update();

                        m_gtFeatureState = Convert.ToString(vpCommonRs.Fields["FEATURE_STATE_C"].Value);
                    }
                }


                Recordset vpConnecRs = m_gComps["CONNECTIVITY_N"].Recordset;
                Recordset rsAssociatedConn = p_gTAssociateComponent.Recordset;

                if (vpConnecRs != null && vpConnecRs.RecordCount > 0)
                {
                    if (!(vpConnecRs.EOF && vpConnecRs.BOF))
                    {
                        vpConnecRs.MoveFirst();

                        if(rsAssociatedConn != null && rsAssociatedConn.RecordCount>0)
                        {
                            if (!(rsAssociatedConn.EOF && rsAssociatedConn.BOF))
                            {
                                rsAssociatedConn.MoveFirst();

                                vpConnecRs.Fields["STATUS_NORMAL_C"].Value = strActiveFeatureNormalState;
                                vpConnecRs.Fields["STATUS_OPERATED_C"].Value = strActiveFeatureOperaState;


                                vpConnecRs.Fields["FEEDER_1_ID"].Value = rsAssociatedConn.Fields["FEEDER_1_ID"].Value;
                                vpConnecRs.Fields["FEEDER_NBR"].Value = rsAssociatedConn.Fields["FEEDER_NBR"].Value;
                                vpConnecRs.Fields["FEEDER_TYPE_C"].Value = rsAssociatedConn.Fields["FEEDER_TYPE_C"].Value;
                                vpConnecRs.Fields["SSTA_C"].Value = rsAssociatedConn.Fields["SSTA_C"].Value;
                                vpConnecRs.Fields["VOLT_1_Q"].Value = rsAssociatedConn.Fields["VOLT_1_Q"].Value;
                                vpConnecRs.Fields["VOLT_2_Q"].Value = rsAssociatedConn.Fields["VOLT_2_Q"].Value;
                                vpConnecRs.Fields["PROTECTIVE_DEVICE_FID"].Value = rsAssociatedConn.Fields["PROTECTIVE_DEVICE_FID"].Value;

                                vpConnecRs.Fields["UPSTREAM_NODE"].Value = rsAssociatedConn.Fields["UPSTREAM_NODE"].Value;
                                vpConnecRs.Fields["UPSTREAM_PROTDEV_Q"].Value = rsAssociatedConn.Fields["UPSTREAM_PROTDEV_Q"].Value;
                            }
                        }                        
                    }
                }
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Method to set Normal Status and As-Operated Status attributes of the virtual point as per feature state.
        /// </summary>
        private void SetNormalAsOperatedStatusOfVirtualPoints()
        {
            List<string> installedFs = new List<string> { "PPI", "ABI" };
            List<string> removedFs = new List<string> { "PPR", "ABR", "PPA", "ABA" };
            try
            {
                Recordset vpConnecRs = m_gComps["CONNECTIVITY_N"].Recordset;

                if (vpConnecRs != null && vpConnecRs.RecordCount > 0)
                {
                    if (!(vpConnecRs.EOF && vpConnecRs.BOF))
                    {
                        vpConnecRs.MoveFirst();

                        if (Convert.ToInt16(vpConnecRs.Fields["G3E_FNO"].Value) == 40 || Convert.ToInt16(vpConnecRs.Fields["G3E_FNO"].Value) == 80)
                        {
                            if (installedFs.Contains(m_gtFeatureState))
                            {
                                vpConnecRs.Fields["STATUS_NORMAL_C"].Value = "OPEN";
                                vpConnecRs.Fields["STATUS_OPERATED_C"].Value = "CLOSED";
                            }
                            else if (removedFs.Contains(m_gtFeatureState))
                            {
                                vpConnecRs.Fields["STATUS_NORMAL_C"].Value = "CLOSED";
                                vpConnecRs.Fields["STATUS_OPERATED_C"].Value = "OPEN";
                            }
                        }
                        else if (Convert.ToInt16(vpConnecRs.Fields["G3E_FNO"].Value) == 6 || Convert.ToInt16(vpConnecRs.Fields["G3E_FNO"].Value) == 82)
                        {
                            if (installedFs.Contains(m_gtFeatureState))
                            {
                                vpConnecRs.Fields["STATUS_NORMAL_C"].Value = "CLOSED";
                                vpConnecRs.Fields["STATUS_OPERATED_C"].Value = "OPEN";
                            }
                            else if (removedFs.Contains(m_gtFeatureState))
                            {
                                vpConnecRs.Fields["STATUS_NORMAL_C"].Value = "OPEN";
                                vpConnecRs.Fields["STATUS_OPERATED_C"].Value = "CLOSED";
                            }
                        }

                        
                    }
                }
            }
            catch
            {
                throw;
            }
            finally
            {
                installedFs.Clear();
                installedFs = null;
                removedFs.Clear();
                removedFs = null;
            }
        }

        #endregion
    }
}
