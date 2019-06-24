//----------------------------------------------------------------------------+
//  Class: fiServicePointPremiseAggregation
//  Description: This interface populates the aggregated premise on the Service Point component 
//----------------------------------------------------------------------------+
//  $Author::   Shubham Agarwal                                               $
//  $Date::     23 October 2017                                               $      
//  $Revision::      1                                                        $
//----------------------------------------------------------------------------+
// *****************  Version 1  *****************
// User:  Shubham Agarwal     Date: 10/23/17   Time: 18:00  Desc : Created
// User: hkonda              Date: 11/3/17   Time: 18:00  Desc : Updated code to set count of Service point Attributes / Critical and Major customers
//----------------------------------------------------------------------------+

using System;
using Intergraph.GTechnology.API;
using Intergraph.GTechnology.Interfaces;
using System.Windows.Forms;

namespace GTechnology.Oncor.CustomAPI
{
    public class fiServicePointPremiseAggregation : IGTFunctional
    {
        private GTArguments m_arguments = null;
        private IGTDataContext m_dataContext = null;
        private IGTComponents m_components;
        private string m_componentName;
        private string m_fieldName;
        private bool m_isDelete = false;

        public GTArguments Arguments
        {
            get { return m_arguments; }
            set { m_arguments = value; }
        }

        public string ComponentName
        {
            get { return m_componentName; }
            set { m_componentName = value; }
        }

        public IGTComponents Components
        {
            get { return m_components; }
            set { m_components = value; }
        }

        public IGTDataContext DataContext
        {
            get { return m_dataContext; }
            set { m_dataContext = value; }
        }


        public void Delete()
        {
            m_isDelete = true;
            ProcessPremiseAggregation();
        }

        public void Execute()
        {
            try
            {
                m_isDelete = false;
                ProcessPremiseAggregation();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error from Service Point Premise Aggregation FI: " + ex.Message, "G/Technology");
            }
        }

        /// <summary>
        /// Method to process premise aggregation during update and delete
        /// </summary>
        private void ProcessPremiseAggregation()
        {
            try
            {
                IGTApplication oApp = GTClassFactory.Create<IGTApplication>();
                string houseNumber = string.Empty;
                string deletedHouseNumber = string.Empty;
                int countPremises = 0;
                int majorCustomerCount = 0;
                int criticalCustomerCount = 0;
                if (m_components[m_componentName].Recordset != null)
                {
                    if (m_components[m_componentName].Recordset.RecordCount > 0)
                    {
                        if (m_isDelete)
                        {
                            if (m_components[m_componentName].Recordset.EOF == false)
                            {
                                if (string.Equals(Convert.ToString(m_components[m_componentName].Recordset.Fields["MAJOR_CUSTOMER_C"].Value), "Y"))
                                {
                                    majorCustomerCount = majorCustomerCount - 1;
                                }
                                if (string.Equals(Convert.ToString(m_components[m_componentName].Recordset.Fields["CRITICAL_CUSTOMER_C"].Value), "Y"))
                                {
                                    criticalCustomerCount = criticalCustomerCount - 1;
                                }
                                deletedHouseNumber = Convert.ToString(m_components[m_componentName].Recordset.Fields["HOUSE_NBR"].Value);
                                countPremises = countPremises - 1;
                            }
                        }

                        m_components[m_componentName].Recordset.MoveFirst();

                        while (m_components[m_componentName].Recordset.EOF == false)
                        {
                            if (string.Equals(Convert.ToString(m_components[m_componentName].Recordset.Fields["MAJOR_CUSTOMER_C"].Value), "Y"))
                            {
                                majorCustomerCount = majorCustomerCount + 1;
                            }
                            if (string.Equals(Convert.ToString(m_components[m_componentName].Recordset.Fields["CRITICAL_CUSTOMER_C"].Value), "Y"))
                            {
                                criticalCustomerCount = criticalCustomerCount + 1;
                            }

                            if (Convert.ToString(m_components[m_componentName].Recordset.Fields["HOUSE_NBR"].Value) != deletedHouseNumber)
                            {
                                houseNumber = Convert.ToString(m_components[m_componentName].Recordset.Fields["HOUSE_NBR"].Value);
                            }
                            countPremises = countPremises + 1;
                            m_components[m_componentName].Recordset.MoveNext();
                        }

                        m_components["SERVICE_POINT_N"].Recordset.MoveFirst();
                        m_components["SERVICE_POINT_N"].Recordset.Fields["MAJOR_CUSTOMER_Q"].Value = majorCustomerCount;

                        m_components["SERVICE_POINT_N"].Recordset.MoveFirst();
                        m_components["SERVICE_POINT_N"].Recordset.Fields["CRITICAL_CUSTOMER_Q"].Value = criticalCustomerCount;

                        m_components["SERVICE_POINT_N"].Recordset.MoveFirst();

                        if (string.IsNullOrEmpty(Convert.ToString(m_components["SERVICE_POINT_N"].Recordset.Fields["HOUSE_NBR"].Value)) || m_isDelete)
                        {
                            if (!string.IsNullOrEmpty(houseNumber))
                            {
                                m_components["SERVICE_POINT_N"].Recordset.Fields["HOUSE_NBR"].Value = houseNumber;
                            }
                        }

                        if (!string.IsNullOrEmpty(Convert.ToString(m_components["SERVICE_POINT_N"].Recordset.Fields["HOUSE_NBR"].Value)))
                        {
                            if (countPremises > 1)
                            {
                                string hNumber = Convert.ToString(m_components["SERVICE_POINT_N"].Recordset.Fields["HOUSE_NBR"].Value);
                                m_components["SERVICE_POINT_N"].Recordset.Fields["HOUSE_NBR"].Value = hNumber.Replace("*", string.Empty) + "*";
                            }
                        }
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public string FieldName
        {
            get { return m_fieldName; }
            set { m_fieldName = value; }
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

        public IGTComponents Components1
        {
            get
            {
                return m_components;
            }

            set
            {
                m_components = value;
            }
        }

        public string ComponentName1
        {
            get
            {
                return m_componentName;
            }

            set
            {
                m_componentName = value;
            }
        }

        public void Validate(out string[] ErrorPriorityArray, out string[] ErrorMessageArray)
        {
            ErrorPriorityArray = null;
            ErrorMessageArray = null;
        }
    }
}
