//----------------------------------------------------------------------------+
//        Class: fiSetComponentCount
//  Description: This functional interface sets the attribute identified by the 
//               count of instances of the component for which this interface is 
//               configured.
//----------------------------------------------------------------------------+
//     $Author:: pnlella                                                      $
//       $Date:: 22/08/2017                                                     $
//   $Revision:: 1                                                            $
//----------------------------------------------------------------------------+
//    $History:: fiSetComponentCount.cs                                           $
//----------------------------------------------------------------------------+

using System;
using System.Windows.Forms;
using Intergraph.GTechnology.Interfaces;
using Intergraph.GTechnology.API;

namespace GTechnology.Oncor.CustomAPI
{
    public class fiSetComponentCount : IGTFunctional
    {
        #region Private Members
        private GTArguments m_GTArguments = null;
        private IGTDataContext m_GTDataContext = null;
        private string m_gCompName = string.Empty;
        private IGTComponents m_gComps = null;
        private IGTFieldValue m_gFieldVal = null;
        private int countANO = 0;
        private string m_gFieldName = string.Empty;
        private string m_countFieldName = string.Empty;
        private GTFunctionalTypeConstants m_gFIType;
        #endregion

        #region IGTFunctional Members

        public Intergraph.GTechnology.API.GTArguments Arguments
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
        public Intergraph.GTechnology.API.IGTComponents Components
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
        public Intergraph.GTechnology.API.IGTDataContext DataContext
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
            SetComponentSum(true);
        }

        public void Execute()
        {
            SetComponentSum(false);
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
        /// SetComponentSum
        /// </summary>
        private void SetComponentSum(bool iDelete)
        {
            int cno = 0;
            string componentName = null;
            ADODB.Recordset rs;
            try
            {
                countANO = Convert.ToInt32(m_GTArguments.GetArgument(0));
                rs = DataContext.MetadataRecordset("G3E_ATTRIBUTEINFO_OPTABLE", "G3E_ANO = " + countANO);
               // rs.Filter = "G3E_ANO = " + countANO;
                if (rs != null)
                {
                    if (!(rs.EOF && rs.BOF))
                    {
                        rs.MoveFirst();
                        m_countFieldName = Convert.ToString(rs.Fields["G3E_FIELD"].Value);
                        cno = Convert.ToInt32(rs.Fields["g3e_cno"].Value);
                    }
                }
                rs = DataContext.MetadataRecordset("G3E_COMPONENTINFO_OPTABLE", "G3E_CNO = " + cno);
               // rs.Filter = "G3E_CNO = " + cno;
                if (rs != null)
                {
                    if (!(rs.EOF && rs.BOF))
                    {
                        rs.MoveFirst();
                        componentName = Convert.ToString(rs.Fields["G3E_TABLE"].Value);
                    }
                }
                if (!String.IsNullOrEmpty(componentName))
                {
                    if (m_gComps[componentName].Recordset != null)
                    {
                        if (m_gComps[componentName].Recordset.RecordCount > 0 || !(m_gComps[componentName].Recordset.EOF && m_gComps[componentName].Recordset.BOF))
                        {
                            m_gComps[componentName].Recordset.MoveFirst();
                            if (m_gComps[m_gCompName].Recordset != null)
                            {
                                if (m_gComps[m_gCompName].Recordset.RecordCount > 0 || !(m_gComps[m_gCompName].Recordset.EOF && m_gComps[m_gCompName].Recordset.BOF))
                                {
                                    m_gComps[componentName].Recordset.Fields[m_countFieldName].Value = m_gComps[m_gCompName].Recordset.RecordCount;
                                    if (iDelete)
                                    {
                                        m_gComps[componentName].Recordset.Fields[m_countFieldName].Value = Convert.ToInt16(m_gComps[componentName].Recordset.Fields[m_countFieldName].Value) - 1;
                                    }
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("There is an error in \"Set Component Count\" Funtional Interface \n" + ex.Message, "G/Technology");
            }

        }
        #endregion
    }
}
