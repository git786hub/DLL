// ===================================================
//  Copyright 2018 Hexagon
//  File Name: InsideFeatures.cs
// 
// Description:   
//  This class is responsible to get all the Inside Features of an Afftected feature

//
//  Remarks:
// 
//  Modification History
//  Date                Author                      Comments
//  --------            --------------              --------
//  28/06/2018          Shubham                     Initial Creation
// ======================================================
using Intergraph.GTechnology.API;
using System;
using System.Collections.Generic;

namespace GTechnology.Oncor.CustomAPI
{
  internal class FeatureConnectivityAttributes
    {

        private int m_upStreamNode;
        private int m_downStreamNode;

        private int m_upStreamNodeValue;
        private int m_downStreamNodeValue;


        private IGTKeyObject m_FeatureKeyObject;
        private string m_componentName = "CONNECTIVITY_N";
        private int m_Node1;
        private int m_Node2;
        private IGTApplication m_oApp;
        public FeatureConnectivityAttributes(IGTKeyObject p_keyObject)
        {
            m_FeatureKeyObject = p_keyObject;
            m_oApp = GTClassFactory.Create<IGTApplication>();
            SetUpstreamNode();
        }

        public int UpstreamNodeValue { get { return m_upStreamNodeValue; } }
        public int DownStreamNodeValue { get { return m_downStreamNodeValue; } }
        public int UpstreamNode { get { return m_upStreamNode; } }
        public int DownStreamNode { get { return m_downStreamNode; } }

        public int Node1 { get { return m_Node1; } }
        public int Node2 { get { return m_Node2; } }

        private void SetUpstreamNode()
        {
            try
            {
                m_FeatureKeyObject.Components[m_componentName].Recordset.MoveFirst();
                string CorrectUpStreamNode = GetCorrectUpstreamNode();
              
                int iRelatedFeatureUSNodeNumber = m_FeatureKeyObject.Components[m_componentName].Recordset.Fields[CorrectUpStreamNode].Value.GetType() != typeof(DBNull) ? Convert.ToInt32(m_FeatureKeyObject.Components[m_componentName].Recordset.Fields[CorrectUpStreamNode].Value) : 0;


                m_Node1 = m_FeatureKeyObject.Components[m_componentName].Recordset.Fields["NODE_1_ID"].Value.GetType() != typeof(DBNull) ? Convert.ToInt32(m_FeatureKeyObject.Components[m_componentName].Recordset.Fields["NODE_1_ID"].Value) : 0;

                m_Node2 = m_FeatureKeyObject.Components[m_componentName].Recordset.Fields["NODE_2_ID"].Value.GetType() != typeof(DBNull) ? Convert.ToInt32(m_FeatureKeyObject.Components[m_componentName].Recordset.Fields["NODE_2_ID"].Value) : 0;

                m_upStreamNodeValue = iRelatedFeatureUSNodeNumber == 1 ? m_Node1 : m_Node2;
                m_downStreamNodeValue = iRelatedFeatureUSNodeNumber == 1 ? m_Node2 : m_Node1;

                m_upStreamNode = iRelatedFeatureUSNodeNumber == 1 ? 1 : 2;
                m_downStreamNode = iRelatedFeatureUSNodeNumber == 1 ? 2 : 1;
            }
            catch (Exception)
            {
                throw;
            }
        }
        private string GetCorrectUpstreamNode()
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

            if (sJobType == "NON-WR" || sJobStatus == "ConstructionComplete")
            {
                sReturn = "UPSTREAM_NODE";
                return sReturn;
            }

            //When Job Type is not Construction Complete or Job Type is not Maintenance
            //If Proposed Upstream Node is populated, give that preference over Upstream Node.

            sReturn = "PP_UPSTREAM_NODE"; //Set the default value to Proposed Upstream Node

            if (m_FeatureKeyObject.Components[m_componentName].Recordset.Fields["PP_UPSTREAM_NODE"].Value.GetType() != typeof(DBNull))
            {
                sReturn = "PP_UPSTREAM_NODE";
            }
            else if (m_FeatureKeyObject.Components[m_componentName].Recordset.Fields["UPSTREAM_NODE"].Value.GetType() != typeof(DBNull))
            {
                sReturn = "UPSTREAM_NODE";
            }

            return sReturn;
        }
    }

 
    class KeyObjectComparer : IEqualityComparer<IGTKeyObject>
    {
        public bool Equals(IGTKeyObject x, IGTKeyObject y)
        {
            //Check whether the compared objects reference the same data.
            if (Object.ReferenceEquals(x, y)) return true;

            //Check whether any of the compared objects is null.
            if (Object.ReferenceEquals(x, null) || Object.ReferenceEquals(y, null))
                return false;

            //Check whether the IGTKeyObject' properties FID and FNO are equal.
            return x.FID == y.FID && x.FNO == y.FNO;
        }

        public int GetHashCode(IGTKeyObject obj)
        {
            return obj.FID;
        }
    }
    }
