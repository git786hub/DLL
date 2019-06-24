//----------------------------------------------------------------------------+
//  Class: FeatureProperties
//  Description: 
//		Class to hold properties of the Affected and Related Feature 
//----------------------------------------------------------------------------+
//  $Author::   Shubham Agarwal                                                       $
//  $Date::     30/07/17                                                                $
//  $Revision:: 1                                                                   $
//----------------------------------------------------------------------------+
//  $History:: FeatureProperties.cs                                             $
// 
// *****************  Version 1  *****************
//  User: sagarwal     Date: 30/04/18   Initial Creation
//  User: pnlella      Date: 15/04/19    Modified as per the requirment of ALM-2214
//----------------------------------------------------------------------------+

using Intergraph.GTechnology.API;
using System;
using System.Linq;

namespace GTechnology.Oncor.CustomAPI
{
    public class FeatureProperties
    {
        private IGTKeyObject m_FeatureKeyObject;
        private string m_componentName;
        private int m_Node1;
        private int m_Node2;
        private int m_upStreamNode;
        private int m_downStreamNode;
        private bool m_isProtectiveDevice;
        private bool m_isBussedTransformer;
        private int m_tieTransformerID;
        private int m_protectionDeviceID;
        private short m_FNO;
        private int m_FID;
        private int m_protectionDeviceIDToSet;
        private string m_protectiveIDUserName = string.Empty;
        private IGTApplication m_oApp;

        public FeatureProperties(IGTKeyObject p_keyObject, string p_componentName, string p_protectiveIDUsername)
        {          
            m_FeatureKeyObject = p_keyObject;
            m_componentName = p_componentName;

            m_protectiveIDUserName = p_protectiveIDUsername;
            m_oApp = GTClassFactory.Create<IGTApplication>();
            if (string.IsNullOrEmpty(p_protectiveIDUsername))
            {
                m_protectiveIDUserName = GetCorrectAttribute("PROTECTIVE_DEVICE_FID", "PP_PROTECTIVE_DEVICE_FID");
            }
           
            SetFeatureProperties();
        }
        private void SetFeatureProperties()
        {
            try
            {
                m_isProtectiveDevice = IsFeatureProtectiveDevice();
                m_isBussedTransformer = IsFeatureBussed();

                m_Node1 = GetNodeValue("NODE_1_ID");
                m_Node2 = GetNodeValue("NODE_2_ID");

                GetUpstreamNode();

                m_FID = m_FeatureKeyObject.FID;
                m_FNO = m_FeatureKeyObject.FNO;

                m_protectionDeviceID = GetProtectionDeviceID();
                m_protectionDeviceIDToSet = GetProtectiveDeviceToSet();
            }
            catch (Exception)
            {
                throw;
            }          
        }
        public int UpstreamNode { get  { return m_upStreamNode; } }
        public int DownStreamNode { get { return m_downStreamNode; } }
        public bool IsProtectiveDevice { get { return m_isProtectiveDevice; } }
        public bool IsBussedTransformer { get { return m_isBussedTransformer; } }
        public int TieTranformerID { get { return m_tieTransformerID; } }
        public int ProtectioDeviceID { get { return m_protectionDeviceID; } }
        public int ProtectiveDeviceIDToSet { get { return m_protectionDeviceIDToSet; } }
        public short FNO { get { return m_FNO; } }
        public int FID { get { return m_FID; } }

        private int GetNodeValue(string p_NodeName)
        {
            
            if (m_FeatureKeyObject.Components[m_componentName].Recordset.RecordCount > 0)
            {
                m_FeatureKeyObject.Components[m_componentName].Recordset.MoveFirst();
                return Convert.ToInt32(m_FeatureKeyObject.Components[m_componentName].Recordset.Fields[p_NodeName].Value);
            }
            else
            {
                return 0;
            }
        }     

        private void GetUpstreamNode()
        {
            try
            {
                string sUpstreamNodeAttribute = GetCorrectAttribute("UPSTREAM_NODE", "PP_UPSTREAM_NODE");
                int iRelatedFeatureUSNodeNumber = m_FeatureKeyObject.Components[m_componentName].Recordset.Fields[sUpstreamNodeAttribute].Value.GetType() != typeof(DBNull) ? Convert.ToInt32(m_FeatureKeyObject.Components[m_componentName].Recordset.Fields[sUpstreamNodeAttribute].Value) : 0;
                m_upStreamNode = iRelatedFeatureUSNodeNumber == 1 ? m_Node1 : m_Node2;
                m_downStreamNode = iRelatedFeatureUSNodeNumber == 1 ? m_Node2 : m_Node1;
            }
            catch (Exception)
            {
                throw;
            }           
        }

        private int GetDownstreamNode()
        {
            string sUpstreamNodeAttribute = GetCorrectAttribute("UPSTREAM_NODE", "PP_UPSTREAM_NODE");
            return Convert.ToInt32(m_FeatureKeyObject.Components[m_componentName].Recordset.Fields[sUpstreamNodeAttribute].Value);
        }

        private string GetCorrectAttribute(string p_Actual, string p_Proposed)
        {
            string sSql = "Select G3E_JOBSTATUS, G3E_JOBTYPE FROM G3E_JOB where G3E_IDENTIFIER = ? ";

            ADODB.Recordset rs = m_oApp.DataContext.OpenRecordset(sSql, ADODB.CursorTypeEnum.adOpenDynamic, ADODB.LockTypeEnum.adLockOptimistic, (int)ADODB.CommandTypeEnum.adCmdText, m_oApp.DataContext.ActiveJob);
            string sJobType = string.Empty;
            string sJobStatus = string.Empty;
            string sReturn = string.Empty;

            if (rs != null)
            {
                if (rs.RecordCount > 0)
                {
                    rs.MoveFirst();
                    sJobStatus = Convert.ToString(rs.Fields["G3E_JOBSTATUS"].Value);
                    sJobType = Convert.ToString(rs.Fields["G3E_JOBTYPE"].Value);
                }
            }

            //If Proposed Upstream Node is populated, give that preference over Upstream Node.

            sReturn = p_Proposed; //Set the default value to Proposed Upstream Node

            m_FeatureKeyObject.Components[m_componentName].Recordset.MoveFirst();

            if (m_FeatureKeyObject.Components[m_componentName].Recordset.Fields[p_Proposed].Value.GetType() != typeof(DBNull) && Convert.ToInt32(m_FeatureKeyObject.Components[m_componentName].Recordset.Fields[p_Proposed].Value)!=0)
            {
                sReturn = p_Proposed;
            }
            else if (m_FeatureKeyObject.Components[m_componentName].Recordset.Fields[p_Actual].Value.GetType() != typeof(DBNull) && Convert.ToInt32(m_FeatureKeyObject.Components[m_componentName].Recordset.Fields[p_Actual].Value) != 0)
            {
                sReturn = p_Actual;
            }

            return sReturn;
        }
        private int GetProtectiveDeviceToSet()
        {
            int bReturnProtectiveDeviceIDToSet = this.ProtectioDeviceID;

            try
            {
                if (this.IsBussedTransformer)
                {                    
                  bReturnProtectiveDeviceIDToSet = this.TieTranformerID;                    
                }
                else if(this.IsProtectiveDevice)
                {
                    bReturnProtectiveDeviceIDToSet = this.FID;
                }               
            }
            catch (Exception)
            {
                throw;
            }
          
            return bReturnProtectiveDeviceIDToSet;
        }

        private int GetProtectionDeviceID()
        {
            int bReturnProtectionDeviceID = 0;

            try
            {
                if (m_FeatureKeyObject.Components[m_componentName].Recordset != null)
                {
                    if (m_FeatureKeyObject.Components[m_componentName].Recordset.RecordCount > 0)
                    {
                        m_FeatureKeyObject.Components[m_componentName].Recordset.MoveFirst();
                        if (m_FeatureKeyObject.Components[m_componentName].Recordset.Fields[m_protectiveIDUserName].Value.GetType() != typeof(DBNull))
                        {
                            bReturnProtectionDeviceID = Convert.ToInt32((m_FeatureKeyObject.Components[m_componentName].Recordset.Fields[m_protectiveIDUserName].Value));
                        }
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
            
            return bReturnProtectionDeviceID;
        }
        private bool IsFeatureProtectiveDevice()
        {
            bool bReturn = false;
             int[] arrProtectiveDevicesFNOs = { 11, 87, 38, 88, 14, 15, 16, 91, 59, 98, 60, 99,18,34};

            if (arrProtectiveDevicesFNOs.Contains(m_FeatureKeyObject.FNO))
            {
                bReturn = true;
            }
            return bReturn;
        }

        private bool IsFeatureBussed()
        {
            bool bReturn = false;

            try
            {
                if (m_FeatureKeyObject.FNO != 0)
                {
                    if (m_FeatureKeyObject.FNO == 59 || m_FeatureKeyObject.FNO == 60 || m_FeatureKeyObject.FNO == 98 || m_FeatureKeyObject.FNO == 99) //Special case of Transformer where if Transformer Tie ID is populated, it should be set as protective device ID
                    {
                        string unitComponentName = m_FeatureKeyObject.FNO == 59 || m_FeatureKeyObject.FNO == 98 ? "XFMR_OH_BANK_N" : "XFMR_UG_UNIT_N";

                        if (m_FeatureKeyObject.Components[unitComponentName].Recordset != null)
                        {
                            if (m_FeatureKeyObject.Components[unitComponentName].Recordset.RecordCount > 0)
                            {
                                m_FeatureKeyObject.Components[unitComponentName].Recordset.MoveFirst();
                                if (m_FeatureKeyObject.Components[unitComponentName].Recordset.Fields["TIE_XFMR_ID"].Value.GetType() != typeof(DBNull))
                                {
                                    m_tieTransformerID = Convert.ToInt32((m_FeatureKeyObject.Components[unitComponentName].Recordset.Fields["TIE_XFMR_ID"].Value));
                                    bReturn = true;

                                }
                            }
                        }
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
           
            return bReturn;
        }
    }
}
