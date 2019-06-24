//----------------------------------------------------------------------------+
//  Class: fiCountAncillariesByCategory
//  Description: This interface counts the Ancillary by it's category and populates the count attribute 
//----------------------------------------------------------------------------+
//  $Author::   Shubham Agarwal                                               $
//  $Date::     04 Ocotber 2017                                                     $
//  $Revision::      1                                                        $
//----------------------------------------------------------------------------+
//  $History::    Initial Creation                                                            $
//----------------------------------------------------------------------------
//  Modifications
//  10/05/2019          Akhilesh                    Included condition of checking Activity != 'R' while calculating the Count of Ancillaries by Category   - ALM 2389
//----------------------------------------------------------------------------+

using System;
using Intergraph.GTechnology.API;
using Intergraph.GTechnology.Interfaces;
using System.Windows.Forms;

namespace GTechnology.Oncor.CustomAPI
{
    public class fiCountAncillariesByCategory : IGTFunctional
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
            ProcessCategoryCount(CalledFromDelete.Yes);
        }

       public enum CalledFromDelete
        {
            Yes,No
        }
        public void ProcessCategoryCount(CalledFromDelete p_calledFromDelete)
        {
            IGTApplication oApp = GTClassFactory.Create<IGTApplication>();

            string placementType = string.Empty;
            string cuCode = string.Empty;
            string categroryCodeArgument = Convert.ToString(m_arguments.GetArgument(0));
            int anoCount = Convert.ToInt32(m_arguments.GetArgument(1));
            string currentCU = string.Empty;

            try
            {
                ADODB.Recordset rs = oApp.DataContext.MetadataRecordset("G3E_ATTRIBUTEINFO_OPTABLE", "g3e_Ano = " + anoCount);               
                Int16 cno = 0;
                string fieldName = Convert.ToString(rs.Fields["G3E_FIELD"].Value);

                string categoryCode = string.Empty;

                if (rs != null)
                {
                    if (rs.RecordCount > 0)
                    {
                        rs.MoveFirst();
                        cno = Convert.ToInt16(rs.Fields["g3E_cno"].Value);

                        try
                        {
                            if (m_components.GetComponent(cno) == null)
                            {
                                return;
                            }
                        }
                        catch (Exception)
                        {
                            return;
                        }
                    }
                }

                int iCount = 0;


                if (m_components[m_componentName].Recordset != null)
                {
                    if (m_components[m_componentName].Recordset.RecordCount > 0)
                    {
                        currentCU = Convert.ToString(m_components[m_componentName].Recordset.Fields["CU_C"].Value);
                        m_components[m_componentName].Recordset.MoveFirst();

                        while (!m_components[m_componentName].Recordset.EOF)
                        {
                            categoryCode = GetCUCategoryCode(Convert.ToString(m_components[m_componentName].Recordset.Fields["CU_C"].Value), oApp);

                            if (string.Equals(categoryCode, categroryCodeArgument) && m_components[m_componentName].Recordset.Fields["ACTIVITY_C"].Value.ToString() != "R")
                            {
                                iCount = iCount + 1;
                            }
                            m_components[m_componentName].Recordset.MoveNext();
                        }

                        categoryCode = GetCUCategoryCode(currentCU,oApp);
                        m_components.GetComponent(cno).Recordset.MoveFirst();
                       
                        if (p_calledFromDelete == CalledFromDelete.Yes && string.Equals(categoryCode, categroryCodeArgument))
                        {
                            iCount = iCount - 1;
                        }

                        m_components.GetComponent(cno).Recordset.Fields[fieldName].Value = iCount;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error from Count Ancillaries By Category FI: " + ex.Message, "G/Technology");
            }
        }
        public void Execute()
        {
            ProcessCategoryCount(CalledFromDelete.No);
        }

        public string FieldName
        {
            get { return m_fieldName; }
            set { m_fieldName = value; }
        }

        private string GetCUCategoryCode(string p_cuCode, IGTApplication p_App)
        {
            string cuCategoryCode = string.Empty;
            ADODB.Recordset rs = null;

            try
            {
                string sql = "select CATEGORY_C from CULIB_UNIT where CU_ID = ?";
                rs = p_App.DataContext.OpenRecordset(sql, ADODB.CursorTypeEnum.adOpenDynamic, ADODB.LockTypeEnum.adLockReadOnly, (int)ADODB.CommandTypeEnum.adCmdText, p_cuCode);

                if (rs != null)
                {
                    if (rs.RecordCount > 0)
                    {
                        rs.MoveFirst();
                        cuCategoryCode = Convert.ToString(rs.Fields[0].Value);
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
           
            return cuCategoryCode;
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
