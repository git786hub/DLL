using System;
using Intergraph.GTechnology.API;
using Intergraph.GTechnology.Interfaces;
using System.Windows.Forms;

//----------------------------------------------------------------------------+
//  Class: fiServicLineAssociatedCostInstall
//  Description: This FI is responsible for the population of Acitivity code for CU component
//               based on the placement type of Service Line Associated Cost.      
//----------------------------------------------------------------------------+
//  $Author:: Shubham Agarwal                                                       $
//  $Date:: 20/12/17                                                                $
//  $Revision:: 1                                                                   $
//----------------------------------------------------------------------------+

namespace GTechnology.Oncor.CustomAPI
{
    public class fiServicLineAssociatedCostInstall : IGTFunctional
    {
        private GTArguments m_arguments = null;
        private IGTDataContext m_dataContext = null;
        private IGTComponents m_components;
        private string m_componentName;
        private string m_fieldName;

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
        }

        public void Execute()
        {
            string placementType = string.Empty;
            string cuCode = string.Empty;
            Int16 fnoServiceLine = 0;

            try
            {
                if (m_componentName.Equals("COMP_UNIT_N"))
                {
                    if (m_components["COMP_UNIT_N"].Recordset != null)
                    {
                        if (m_components["COMP_UNIT_N"].Recordset.RecordCount > 0)
                        {
                            m_components["COMP_UNIT_N"].Recordset.MoveFirst();
                            fnoServiceLine = Convert.ToInt16(m_components["COMP_UNIT_N"].Recordset.Fields["g3e_fno"].Value);
                            if (fnoServiceLine != 54)
                                return;
                        }
                    }
                }
                if (m_components["SERVICE_LINE_N"].Recordset != null)
                {
                    if (m_components["SERVICE_LINE_N"].Recordset.RecordCount > 0)
                    {
                        m_components["SERVICE_LINE_N"].Recordset.MoveFirst();
                        placementType = Convert.ToString(m_components["SERVICE_LINE_N"].Recordset?.Fields["PLACEMENT_TYPE_C"].Value);

                        if (placementType.Equals("ASSOCIATED"))
                        {
                            if (m_components["COMP_UNIT_N"].Recordset != null)
                            {
                                if (m_components["COMP_UNIT_N"].Recordset.RecordCount > 0)
                                {
                                    m_components["COMP_UNIT_N"].Recordset.MoveFirst();
                                    cuCode = Convert.ToString(m_components["COMP_UNIT_N"].Recordset.Fields["ACTIVITY_C"].Value);
                                    if (cuCode.Equals("I"))
                                    {
                                        m_components["COMP_UNIT_N"].Recordset.Fields["ACTIVITY_C"].Value = "IA";
                                    }
                                }
                            }
                        }
                        else
                        {
                            if (m_components["COMP_UNIT_N"].Recordset != null)
                            {
                                if (m_components["COMP_UNIT_N"].Recordset.RecordCount > 0)
                                {
                                    m_components["COMP_UNIT_N"].Recordset.MoveFirst();
                                    cuCode = Convert.ToString(m_components["COMP_UNIT_N"].Recordset.Fields["ACTIVITY_C"].Value);
                                    if (cuCode.Equals("IA"))
                                    {
                                        m_components["COMP_UNIT_N"].Recordset.Fields["ACTIVITY_C"].Value = "I";
                                    }
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error in Service Line Associated Cost Install FI: " + ex.Message, "G/Technology");
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
