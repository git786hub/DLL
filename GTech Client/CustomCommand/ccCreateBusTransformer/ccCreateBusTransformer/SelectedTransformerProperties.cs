// ===================================================
//  Copyright 2018 Hexagon
//  File Name: SelectedTransformerProperties.cs
// 
//  Description: This command allows users to identify multiple transformers as being bussed together.
//  Remarks: 
//  Modification History
//  Date                Author                      Comments
//  --------            --------------              --------
//  24/05/2018          Shubham                     Created 
//  09/05/2019          Akhilesh                    Updated Tie Transformer ID Value to DBNull.Value value when user tries to UN Bus Tranformers   -   ALM 2390
// ======================================================
using Intergraph.GTechnology.API;
using System;
using System.Collections.Generic;
using System.Linq;

namespace GTechnology.Oncor.CustomAPI
{
    class SelectedTransformerProperties
    {
        private IGTApplication m_App = null;

        private Dictionary<int, short> m_distinctFNOFID = new Dictionary<int, short>();
        public IGTDDCKeyObjects m_keyobjects = null;
        public SelectedTransformerProperties(IGTDDCKeyObjects p_SelectedObjects)
        {
            try
            {
                m_keyobjects = p_SelectedObjects;
                m_App = GTClassFactory.Create<IGTApplication>();
                GetDistinctSelectedTransformerFNO();
            }
            catch (Exception)
            {
                throw;
            }
        }
        public Dictionary<int, short> GetDistinctSelectedTransformer { get { return m_distinctFNOFID; } }
        public bool IsTransformerBussed()
        {
            bool isBussed = false;
            string sUnitComponent = string.Empty;
            try
            {
                foreach (KeyValuePair<int, short> item in m_distinctFNOFID)
                {
                    IGTKeyObject o = m_App.DataContext.OpenFeature(item.Value, item.Key);
                    sUnitComponent = GetSelectedTransformerUnitComponentName();

                    if (o.Components[sUnitComponent].Recordset.Fields["TIE_XFMR_ID"].Value.GetType() != typeof(DBNull) && Convert.ToInt32(o.Components[sUnitComponent].Recordset.Fields["TIE_XFMR_ID"].Value) != 0)
                    {
                        isBussed = true;
                        break;
                    }
                }
                return isBussed;
            }
            catch (Exception)
            {
                throw;
            }
        }

        private Dictionary<int, short> GetDistinctSelectedTransformerFNO()
        {
            try
            {

                for (int i = 0; i < m_keyobjects.Count; i++)
                {
                    if (!m_distinctFNOFID.ContainsKey(m_keyobjects[i].FID))
                    {
                        m_distinctFNOFID.Add(m_keyobjects[i].FID, m_keyobjects[i].FNO);
                    }
                }
                return m_distinctFNOFID;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public bool ValidateSelecSetForTransformer()
        {
            try
            {
                bool bNonTransformerFeature = true;

                if (m_distinctFNOFID.GroupBy(p => p.Value).Where(p => p.Count() > 0).Count() > 1) //Selected set of features do not belong to same feature class, invalidate the select set;
                {
                    bNonTransformerFeature = false;
                }
                else if (Convert.ToInt32(m_distinctFNOFID.Values.First()) != 59 && Convert.ToInt32(m_distinctFNOFID.Values.First()) != 98 && Convert.ToInt32(m_distinctFNOFID.Values.First()) != 60 && Convert.ToInt32(m_distinctFNOFID.Values.First()) != 99)
                {
                    bNonTransformerFeature = false;
                }
                return bNonTransformerFeature;
            }
            catch (Exception)
            {
                throw;
            }
        }
        public void UnBussTransformer()
        {
            try
            {
                string sUnitComponentName = GetSelectedTransformerUnitComponentName();

                foreach (KeyValuePair<int, short> item in m_distinctFNOFID)
                {
                    IGTKeyObject o = m_App.DataContext.OpenFeature(item.Value, item.Key);
                    o.Components[sUnitComponentName].Recordset.Fields["TIE_XFMR_ID"].Value = DBNull.Value;
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        private string GetSelectedTransformerUnitComponentName()
        {
            try
            {
                string sUnitComponentName = string.Empty;
                foreach (KeyValuePair<int, short> item in m_distinctFNOFID)
                {
                    IGTKeyObject o = m_App.DataContext.OpenFeature(item.Value, item.Key);

                    switch (item.Value)
                    {
                        case 59:
                            sUnitComponentName = "XFMR_OH_BANK_N";
                            //return;
                            break;
                        case 98:
                            sUnitComponentName = "XFMR_OH_BANK_N";
                            // return;
                            break;
                        case 60:
                            sUnitComponentName = "XFMR_UG_UNIT_N";
                            // return;
                            break;
                        case 99:
                            sUnitComponentName = "XFMR_UG_UNIT_N";
                            //  return;
                            break;
                        default:
                            break;
                    }
                }
                return sUnitComponentName;
            }
            catch (Exception)
            {
                throw;
            }
        }
        public void BussAllTransformer(int p_TieTransformerID)
        {
            try
            {
                string sUnitComponentName = GetSelectedTransformerUnitComponentName();

                foreach (KeyValuePair<int, short> item in m_distinctFNOFID)
                {
                    IGTKeyObject o = m_App.DataContext.OpenFeature(item.Value, item.Key);
                    o.Components[sUnitComponentName].Recordset.Fields["TIE_XFMR_ID"].Value = p_TieTransformerID;
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
