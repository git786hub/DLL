// ===================================================
//  Copyright 2017 Intergraph Corp.
//  File Name: fiTransformerValidation.cs
// 
//  Description:   	if feature is a secondary Transformer then Secondary operating voltage > Primary operating Voltage           	
//
//  Remarks:BR-0.1.14.F29
// 
//  Modification History
//  Date                Author                      Comments
//  --------            --------------              --------
//  01/09/2017          Prathyusha                   Created 
//  24/01/2018          Prathyusha                   Removed the code under validate method as per the changes given in ONCORDEV-477 JIRA 
// ======================================================
using System;
using System.Windows.Forms;
using Intergraph.GTechnology.Interfaces;
using Intergraph.GTechnology.API;

namespace GTechnology.Oncor.CustomAPI
{
    class fiTransformerVoltage : IGTFunctional
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
        public IGTComponents Components
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
        public string FieldName
        {
            get
            {
                return m_gFieldName;
            }

            set
            {
                m_gFieldName = value;
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

        public void Delete()
        {

        }

        public void Execute()
        {
            try
            {
                if (m_gComps[m_gCompName].Recordset != null)
                {
                    if (m_gComps[m_gCompName].Recordset.RecordCount > 0 || !(m_gComps[m_gCompName].Recordset.EOF && m_gComps[m_gCompName].Recordset.BOF))
                    {
                        m_gComps[m_gCompName].Recordset.MoveFirst();
                        if (Convert.ToInt16(m_gComps[m_gCompName].Recordset.Fields["G3E_FNO"].Value) == 18)
                        {
                            m_gComps[m_gCompName].Recordset.Fields["VOLT_1_Q"].Value = m_gComps[m_gCompName].Recordset.Fields["VOLT_2_Q"].Value;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("There is an error in \"Transformer Voltage\" Funtional Interface \n" + ex.Message, "G/Technology"); 
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