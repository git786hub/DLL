// ===================================================
//  Copyright 2017 Intergraph Corp.
//  File Name: fiIsolationScenario.cs
// 
//  Description:   To update actual length While breaking the conductor.
//  Remarks: 
//  Modification History
//  Date                Author                      Comments
//  --------            --------------              --------
//  18/02/2019          Sithara                      Created 
// ======================================================
using System;
using System.Collections.Generic;
using Intergraph.GTechnology.API;
using Intergraph.GTechnology.Interfaces;
using System.Windows.Forms;


namespace GTechnology.Oncor.CustomAPI
{
    public class pbActuallengthUpdate : IGTPostBreakLinear
    {
        #region Private Members
        private GTArguments m_GTArguments = null;       
        private IGTKeyObject m_GTActiveFeature = null;
        private IGTKeyObject m_GTBreakFeature = null;
        private double m_GTBreakRatio = 0.0;
        private IGTDataContext m_GTDataContext = null;
        private IGTKeyObject m_GTNewFeature = null;
        #endregion

        #region IGTPostBreakLinear Members
        public IGTKeyObject ActiveFeature
        {
            get
            {
                return m_GTActiveFeature;
            }
            set
            {
                m_GTActiveFeature = value;
            }
        }
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
        public IGTKeyObject BreakFeature
        {
            get
            {
                return m_GTBreakFeature;
            }
            set
            {
                m_GTBreakFeature = value;
            }
        }
        public double BreakRatio
        {
            get
            {
                return m_GTBreakRatio;
            }
            set
            {
                m_GTBreakRatio = value;
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
        public IGTKeyObject NewFeature
        {
            get
            {
                return m_GTNewFeature;
            }
            set
            {
                m_GTNewFeature = value;
            }
        }

        private List<short> m_BreakFNOs = new List<short>(new short[] { 8,9,53,54,63,84,85,96,97,104,2200 });
        public void Execute()
        {
            IGTComponent breakNetelemComp = null;
            IGTComponent newFeatureNetelemComp = null;
            
            short cnoNetelem = 1;
            int ActualLength = 0;
            string FeatureState = "";
            
            try
            {
                if (m_BreakFNOs.Contains(BreakFeature.FNO))
                {
                    breakNetelemComp = BreakFeature.Components.GetComponent(cnoNetelem);
                    newFeatureNetelemComp = NewFeature.Components.GetComponent(cnoNetelem);

                    if (breakNetelemComp != null && breakNetelemComp.Recordset != null && breakNetelemComp.Recordset.RecordCount > 0 &&
                       newFeatureNetelemComp != null && newFeatureNetelemComp.Recordset != null && newFeatureNetelemComp.Recordset.RecordCount > 0)
                    {
                        breakNetelemComp.Recordset.MoveFirst();
                        ActualLength = Convert.ToInt32(breakNetelemComp.Recordset.Fields["LENGTH_GRAPHIC_FT"].Value);
                        FeatureState = Convert.ToString(breakNetelemComp.Recordset.Fields["FEATURE_STATE_C"].Value);
                        breakNetelemComp.Recordset.Fields["LENGTH_ACTUAL_Q"].Value = ActualLength;
                        breakNetelemComp.Recordset.Update();

                        ActualLength = 0;
                        newFeatureNetelemComp.Recordset.MoveFirst();
                        ActualLength = Convert.ToInt32(newFeatureNetelemComp.Recordset.Fields["LENGTH_GRAPHIC_FT"].Value);
                        newFeatureNetelemComp.Recordset.Fields["LENGTH_ACTUAL_Q"].Value = ActualLength;
                        newFeatureNetelemComp.Recordset.Fields["FEATURE_STATE_C"].Value = FeatureState;
                        newFeatureNetelemComp.Recordset.Update();
                    }

                    UpdateWRID(21);
                    UpdateWRID(22);
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show("Error in PostBreakLinear:pbActuallengthUpdate - " + ex.Message, "G/Technology", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);               
            }
        }

        private void UpdateWRID(short p_Cno)
        {
            try
            {
                IGTComponent breakCUComp = BreakFeature.Components.GetComponent(p_Cno);
                IGTComponent newFeatureCUComp = NewFeature.Components.GetComponent(p_Cno);
                Dictionary<int, string> dickeyValuePairs = new Dictionary<int, string>();

                if (breakCUComp != null && breakCUComp.Recordset != null && breakCUComp.Recordset.RecordCount > 0 &&
                   newFeatureCUComp != null && newFeatureCUComp.Recordset != null && newFeatureCUComp.Recordset.RecordCount > 0)
                {
                    breakCUComp.Recordset.MoveFirst();

                    while (!breakCUComp.Recordset.EOF)
                    {
                        dickeyValuePairs.Add(Convert.ToInt32(breakCUComp.Recordset.Fields["G3E_CID"].Value),
                            Convert.ToString(breakCUComp.Recordset.Fields["WR_ID"].Value));
                        breakCUComp.Recordset.MoveNext();
                    }

                    newFeatureCUComp.Recordset.MoveFirst();
                    string strWR = "";

                    while (!newFeatureCUComp.Recordset.EOF)
                    {
                        dickeyValuePairs.TryGetValue(Convert.ToInt32(newFeatureCUComp.Recordset.Fields["G3E_CID"].Value), out strWR);

                        if (!string.IsNullOrEmpty(strWR))
                        {
                            newFeatureCUComp.Recordset.Fields["WR_ID"].Value = strWR;
                            newFeatureCUComp.Recordset.Update();
                        }
                        newFeatureCUComp.Recordset.MoveNext();
                    }

                    if(dickeyValuePairs.Count>0)
                    {
                        dickeyValuePairs.Clear();
                    }
                }
            }
            catch
            {
                throw;
            }
        }

        #endregion
    }
}
