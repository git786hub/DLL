//----------------------------------------------------------------------------+
//        Class: fiSetJobAttribute
//  Description: This functional interface sets the value of the attribute with 
//               which the interface is associated to the Job Status of the 
//               active job.
//----------------------------------------------------------------------------+
//     $Author:: pnlella                                                      $
//       $Date:: 3/07/2017                                                     $
//   $Revision:: 1                                                            $
//----------------------------------------------------------------------------+
//    $History:: fiSetJobAttribute.cs                                           $
//----------------------------------------------------------------------------+

using System;
using System.Windows.Forms;
using Intergraph.GTechnology.Interfaces;
using Intergraph.GTechnology.API;

namespace GTechnology.Oncor.CustomAPI
{
    public class fiSetJobAttribute : IGTFunctional
    {
        #region Private Members
        private GTArguments m_GTArguments = null;
        private IGTDataContext m_GTDataContext = null;
        private string m_gCompName = string.Empty;
        private IGTComponents m_gComps = null;
        private IGTFieldValue m_gFieldVal = null;
        private int m_sjobANO = 0;
        private string m_gFieldName = string.Empty;
        private string m_jobFieldName = string.Empty;
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

        }

        public void Execute()
        {
            int cnt = 0;
            ADODB.Recordset rs;
            try
            {
                m_sjobANO = Convert.ToInt32(m_GTArguments.GetArgument(0));
                rs = DataContext.MetadataRecordset("G3E_ATTRIBUTEINFO_OPTABLE", "G3E_ANO = " + m_sjobANO + " AND G3E_NAME = 'G3E_JOB'");
               // rs.Filter = "G3E_ANO = " + m_sjobANO + " AND G3E_NAME = 'G3E_JOB'";
                if (rs != null)
                {
                    if (!(rs.EOF && rs.BOF))
                    {
                        rs.MoveFirst();
                        m_jobFieldName = Convert.ToString(rs.Fields["G3E_FIELD"].Value);
                    }
                }
                string sql = "SELECT " + m_jobFieldName + " FROM G3E_JOB WHERE G3E_IDENTIFIER=:1";
                rs = DataContext.Execute(sql, out cnt, (int)ADODB.CommandTypeEnum.adCmdText, m_GTDataContext.ActiveJob.ToString());
                if (rs != null)
                {
                    if (!(rs.EOF && rs.BOF))
                    {
                        rs.MoveFirst();
                        if (m_gComps[m_gCompName].Recordset != null)
                        {
                            if (m_gComps[m_gCompName].Recordset.RecordCount > 0 || !(m_gComps[m_gCompName].Recordset.EOF && m_gComps[m_gCompName].Recordset.BOF))
                            {
                                m_gComps[m_gCompName].Recordset.MoveFirst();
                                m_gComps[m_gCompName].Recordset.Fields[m_gFieldName].Value = rs.Fields[0].Value;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("There is an error in \"Set Job Attribute on Create\" Funtional Interface \n" + ex.Message, "G/Technology");
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

    }
}
