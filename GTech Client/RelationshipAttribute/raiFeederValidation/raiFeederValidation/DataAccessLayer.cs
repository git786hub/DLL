// ===================================================
//  Copyright 2018 Intergraph Corp.
//  File Name: DataAccessLayer.cs
//
//  Description:    Class to build the recordset.
//  Remarks: 
//  Modification History
//  Date                Author                      Comments
//  --------            --------------              --------
//  26/04/2018          Sitara                      Created
//  28/05/2018          Prathyusha                  Modified the code as per the new Requirements mentioned in JIRA-1610.
// ======================================================
using ADODB;
using Intergraph.GTechnology.API;
using System;

namespace GTechnology.Oncor.CustomAPI
{
    class DataAccessLayer
    {
        #region Variables

        private IGTDataContext m_oGTDataContext;

        #endregion

        #region Constructor

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="p_dataContext">The current G/Technology application object.</param>
        public DataAccessLayer(IGTDataContext p_dataContext)
        {
            this.m_oGTDataContext = p_dataContext;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Method to get the tie Feeder Attribute value.
        /// </summary>
        /// <param name="p_component"></param>
        /// <param name="p_relatedFieldName"></param>
        /// <returns></returns>
        public string GetTieFeederAttribute(IGTComponent p_component,string p_relatedFieldName)
        {
            string strFeederAttribute = null;

            try
            {
                if (p_component != null &&
                   p_component.Recordset != null &&
                   p_component.Recordset.RecordCount > 0)
                {
                    p_component.Recordset.MoveFirst();

                    strFeederAttribute = Convert.ToString(p_component.Recordset.Fields[p_relatedFieldName].Value);
                }
            }
            catch
            {
                throw;
            }

            return strFeederAttribute;
        }

        /// <summary>
        /// Method to get the tie Feeder Attribute Field Name.
        /// </summary>
        /// <param name="p_tieANO"></param>
        /// <param name="p_CNO"></param>
        /// <returns></returns>
        public string GetTieFeederAttributeFieldName(long p_tieANO, short p_CNO)
        {
            string strTieFeederFieldName = null;
            Recordset rs = null;
            try
            {
                rs= m_oGTDataContext.MetadataRecordset("G3E_ATTRIBUTEINFO_OPTABLE");
                rs.Filter = "G3E_ANO = " + p_tieANO + " AND G3E_CNO = "+ p_CNO;

                if (rs != null)
                {
                    if (!(rs.EOF && rs.BOF))
                    {
                        rs.MoveFirst();
                        strTieFeederFieldName = Convert.ToString(rs.Fields["G3E_FIELD"].Value);
                    }
                }
            }
            catch
            {
                throw;
            }

            return strTieFeederFieldName;
        }

        /// <summary>
        /// Method to set the tie Feeder Attribute and Feeder Attribute of the active component from the value of related component.
        /// </summary>
        /// <param name="p_ActiveComponent"></param>
        /// <param name="p_FeederAttribute"></param>
        /// <param name="p_activeField"></param>
        /// <param name="p_tieFeederAttribute"></param>
        /// <param name="p_normalStatus"></param>
        public void SetFeederAttribute(IGTComponent p_ActiveComponent, string p_FeederAttribute,string p_activeField,string p_tieFeederAttribute,string p_normalStatus)
        {
            try
            {
                if (p_ActiveComponent != null &&
                   p_ActiveComponent.Recordset != null &&
                   p_ActiveComponent.Recordset.RecordCount > 0)
                {
                    p_ActiveComponent.Recordset.MoveFirst();
                    p_ActiveComponent.Recordset.Fields[p_activeField].Value = p_FeederAttribute;

                    if (p_tieFeederAttribute != null)
                    {
                        if (string.Equals(p_normalStatus.ToUpper(), "CLOSED"))
                        {
                            p_ActiveComponent.Recordset.Fields[p_tieFeederAttribute].Value = p_FeederAttribute;
                        }
                    }
                    p_ActiveComponent.Recordset.Update();
                }
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Method to get Normal Status of active component.
        /// </summary>
        /// <param name="p_activeComponent"></param>
        /// <returns></returns>
        public string GetNormalStatus(IGTComponent p_activeComponent)
        {
            string strStatus = null;
            try
            {
                if (p_activeComponent != null &&
                   p_activeComponent.Recordset != null &&
                   p_activeComponent.Recordset.RecordCount > 0)
                {
                    p_activeComponent.Recordset.MoveFirst();

                    strStatus = Convert.ToString(p_activeComponent.Recordset.Fields["STATUS_NORMAL_C"].Value);
                }
            }
            catch
            {
                throw;
            }

            return strStatus;
        }
        #endregion
    }
}
