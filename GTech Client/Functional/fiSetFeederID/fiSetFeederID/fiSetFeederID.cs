//----------------------------------------------------------------------------+
//        Class: fiSetFeederID
//  Description: This interface sets the Feeder ID for a Substation Breaker by concatenating the Substation Code and Feeder Number attributes.
//----------------------------------------------------------------------------+
//     $Author:: hkonda                                                       $
//       $Date:: 18/08/17                                                     $
//   $Revision:: 1                                                            $
//----------------------------------------------------------------------------+
//    $History:: fiSetFeederID.cs                                       $
// 
// *****************  Version 1  *****************
// User: hkonda     Date: 18/08/17    Time: 18:00
// User: hkonda     Date: 09/09/17    Time: 18:00  Desc : Fixed recordset issue during sanity testing
// User: hkonda     Date: 11/09/17    Time: 18:00  Desc : Reading substation code from same feature instead of owning feature
// User: hkonda     Date: 22/05/18    Time: 18:00  Desc : Seperated code logic for Substation breaker and Substation breaker network fetaures per business rules.
//----------------------------------------------------------------------------+


using System;
using System.Windows.Forms;
using ADODB;
using Intergraph.GTechnology.API;
using Intergraph.GTechnology.Interfaces;

namespace GTechnology.Oncor.CustomAPI
{
    public class fiSetFeederID : IGTFunctional
    {
        private GTArguments m_Arguments = null;
        private IGTDataContext m_DataContext = null;
        private IGTComponents m_components;
        private string m_ComponentName;
        private string m_FieldName;

        public GTArguments Arguments
        {
            get { return m_Arguments; }
            set { m_Arguments = value; }
        }

        public string ComponentName
        {
            get { return m_ComponentName; }
            set { m_ComponentName = value; }
        }

        public IGTComponents Components
        {
            get { return m_components; }
            set { m_components = value; }
        }

        public IGTDataContext DataContext
        {
            get { return m_DataContext; }
            set { m_DataContext = value; }
        }

        public void Delete()
        {
        }

        public void Execute()
        {
            Recordset subStationBreakerAttributeRs = null;
            Recordset subStationBreakerConnectivityAttributeRs = null;
            string subStationCode = string.Empty;
            string feederNumber = string.Empty;
            string networkId = string.Empty;
            short fno = 0;
            try
            {
                subStationBreakerAttributeRs = Components[ComponentName].Recordset;
                if (subStationBreakerAttributeRs.RecordCount > 0)
                {
                    subStationBreakerAttributeRs.MoveFirst();
                    fno = Convert.ToInt16(subStationBreakerAttributeRs.Fields["G3E_FNO"].Value);
                    feederNumber = Convert.ToString(subStationBreakerAttributeRs.Fields["FEEDER_NBR"].Value);
                }

                subStationBreakerConnectivityAttributeRs = Components.GetComponent(11).Recordset;

                if (subStationBreakerConnectivityAttributeRs.RecordCount > 0)
                {
                    subStationBreakerConnectivityAttributeRs.MoveFirst();
                    subStationCode = Convert.ToString(subStationBreakerConnectivityAttributeRs.Fields["SSTA_C"].Value);
                    networkId = Convert.ToString(subStationBreakerConnectivityAttributeRs.Fields["NETWORK_ID"].Value);

                    if (fno == 16)
                    {
                        subStationBreakerConnectivityAttributeRs.Fields["FEEDER_1_ID"].Value = subStationCode + feederNumber;
                        subStationBreakerConnectivityAttributeRs.Fields["FEEDER_2_ID"].Value = subStationCode + feederNumber;

                        subStationBreakerConnectivityAttributeRs.Fields["FEEDER_NBR"].Value = feederNumber;
                        subStationBreakerConnectivityAttributeRs.Fields["TIE_FEEDER_NBR"].Value = feederNumber;

                    }
                    else
                    {
                        if (!string.IsNullOrEmpty(networkId))
                        {
                            if (networkId.Length < 3)
                            {
                                subStationBreakerConnectivityAttributeRs.Fields["FEEDER_1_ID"].Value = string.Empty;
                                subStationBreakerConnectivityAttributeRs.Fields["FEEDER_2_ID"].Value = string.Empty;
                                subStationBreakerConnectivityAttributeRs.Fields["FEEDER_NBR"].Value = string.Empty;
                                subStationBreakerConnectivityAttributeRs.Fields["TIE_FEEDER_NBR"].Value = string.Empty;
                                return;
                            }
                            networkId = networkId.Remove(networkId.Length - 2, 2);
                            subStationBreakerConnectivityAttributeRs.Fields["FEEDER_1_ID"].Value = networkId + feederNumber;
                            subStationBreakerConnectivityAttributeRs.Fields["FEEDER_2_ID"].Value = networkId + feederNumber;

                            subStationBreakerConnectivityAttributeRs.Fields["FEEDER_NBR"].Value = feederNumber;
                            subStationBreakerConnectivityAttributeRs.Fields["TIE_FEEDER_NBR"].Value = feederNumber;
                        }
                        else
                        {
                            subStationBreakerConnectivityAttributeRs.Fields["FEEDER_1_ID"].Value = string.Empty;
                            subStationBreakerConnectivityAttributeRs.Fields["FEEDER_2_ID"].Value = string.Empty;
                            subStationBreakerConnectivityAttributeRs.Fields["FEEDER_NBR"].Value = string.Empty;
                            subStationBreakerConnectivityAttributeRs.Fields["TIE_FEEDER_NBR"].Value = string.Empty;
                        }
                    }
                    subStationBreakerConnectivityAttributeRs.Update(System.Type.Missing, System.Type.Missing);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error during Set Feeder ID execution. " + ex.Message, "G/Technology");
            }
        }

        public string FieldName
        {
            get { return m_FieldName; }
            set { m_FieldName = value; }
        }

        public IGTFieldValue FieldValueBeforeChange
        {
            get;
            set;
        }

        public GTFunctionalTypeConstants Type
        {
            get;
            set;
        }

        public void Validate(out string[] ErrorPriorityArray, out string[] ErrorMessageArray)
        {
            ErrorMessageArray = null;
            ErrorPriorityArray = null;
        }
    }
}
