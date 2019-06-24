//----------------------------------------------------------------------------+
//        Class: fiSetSystemDate
//  Description: This functional interface sets the value of the attribute with 
//                which the interface is associated to the system date/time.
//----------------------------------------------------------------------------+
//     $Author:: pnlella                                                      $
//       $Date:: 3/07/2017                                                    $
//   $Revision:: 1                                                            $
//----------------------------------------------------------------------------+
//    $History:: fiSetSystemDate.cs                                           $
//----------------------------------------------------------------------------+

using System;
using System.Windows.Forms;
using Intergraph.GTechnology.Interfaces;
using Intergraph.GTechnology.API;

namespace GTechnology.Oncor.CustomAPI
{
    public class fiSetSystemDate : IGTFunctional
    {
        #region Private Members
        private GTArguments m_GTArguments = null;
        private IGTDataContext m_GTDataContext = null;
        private string m_gCompName = string.Empty;
        private IGTComponents m_gComps = null;
        private IGTFieldValue m_gFieldVal = null;
        private string m_gFieldName = string.Empty;
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
            try
            {
                string sSql = "select sysdate from dual";
                ADODB.Recordset rs = m_GTDataContext.OpenRecordset(sSql, ADODB.CursorTypeEnum.adOpenStatic,
                        ADODB.LockTypeEnum.adLockReadOnly, -1);
                if (m_gComps[m_gCompName].Recordset != null)
                {
                    if (!(m_gComps[m_gCompName].Recordset.EOF && m_gComps[m_gCompName].Recordset.BOF))
                    {
                        if (rs != null)
                        {
                            if (!(rs.EOF && rs.BOF))
                            {
                                rs.MoveFirst();
                                m_gComps[m_gCompName].Recordset.Fields[m_gFieldName].Value = rs.Fields[0].Value;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("There is an error in \"Set System Date On Create\" Funtional Interface \n" + ex.Message, "G/Technology");
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
