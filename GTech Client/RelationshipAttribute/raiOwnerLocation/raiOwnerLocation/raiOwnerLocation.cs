//----------------------------------------------------------------------------+
//  Class: raiOwnerLocation
//  Description: This interface validates the Prmary Conductor if it is connected to
//  a single phase or if Primary Conductor with a dead end does not have a Guy
//----------------------------------------------------------------------------+
//  $Author:: Prathyusha                                                      $
//  $Date:: 09/09/17                                                                $
//  $Revision:: 1                                                                   $
//----------------------------------------------------------------------------+
//  $History:: raiOwnerLocation.cs                                             $
// 
// *****************  Version 1  *****************
//  User: pnlella     Date: 09/09/17    Time: 11:00
//----------------------------------------------------------------------------+

using System;
using Intergraph.GTechnology.Interfaces;
using Intergraph.GTechnology.API;
using System.Collections.Generic;
using System.Windows.Forms;

namespace GTechnology.Oncor.CustomAPI
{
    public class raiOwnerLocation : IGTRelationshipAttribute
    {
        private int m_iActiveANO = 0;
        private string m_sActiveComponentName = string.Empty;
        private IGTComponents m_oActiveComponents = null;
        private string m_sActiveFieldName = string.Empty;
        private IGTFieldValue m_oActiveFieldValue = null;
        private GTArguments m_oGTArguments = null;
        private IGTDataContext m_oDataContext = null;
        private string m_sPriority = string.Empty;
        private int m_iRelatedANO = 0;
        private string m_sRelatedComponentName = string.Empty;
        private IGTComponents m_oRelatedComponents = null;
        private string m_sRelatedFieldName = string.Empty;
        private IGTFieldValue m_oRelatedFieldValue = null;
        private const string m_OwnerComponent = "COMMON_N";

        public int ActiveANO
        {
            get
            {
                return m_iActiveANO;
            }

            set
            {
                m_iActiveANO = value;
            }
        }

        public string ActiveComponentName
        {
            get
            {
                return m_sActiveComponentName;
            }

            set
            {
                m_sActiveComponentName = value;
            }
        }

        public IGTComponents ActiveComponents
        {
            get
            {
                return m_oActiveComponents;
            }

            set
            {
                m_oActiveComponents = value;
            }
        }

        public string ActiveFieldName
        {
            get
            {
                return m_sActiveFieldName;
            }

            set
            {
                m_sActiveFieldName = value;
            }
        }

        public IGTFieldValue ActiveFieldValue
        {
            get
            {
                return m_oActiveFieldValue;
            }

            set
            {
                m_oActiveFieldValue = value;
            }
        }

        public GTArguments Arguments
        {
            get
            {
                return m_oGTArguments;
            }

            set
            {
                m_oGTArguments = value;
            }
        }

        public IGTDataContext DataContext
        {
            get
            {
                return m_oDataContext;
            }

            set
            {
                m_oDataContext = value;
            }
        }

        public string Priority
        {
            get
            {
                return m_sPriority;
            }

            set
            {
                m_sPriority = value;
            }
        }

        public int RelatedANO
        {
            get
            {
                return m_iRelatedANO;
            }

            set
            {
                m_iRelatedANO = value;
            }
        }

        public string RelatedComponentName
        {
            get
            {
                return m_sRelatedComponentName;
            }

            set
            {
                m_sRelatedComponentName = value;
            }
        }

        public IGTComponents RelatedComponents
        {
            get
            {
                return m_oRelatedComponents;
            }

            set
            {
                m_oRelatedComponents = value;
            }
        }

        public string RelatedFieldName
        {
            get
            {
                return m_sRelatedFieldName;
            }

            set
            {
                m_sRelatedFieldName = value;
            }
        }

        public IGTFieldValue RelatedFieldValue
        {
            get
            {
                return m_oRelatedFieldValue;
            }

            set
            {
                m_oRelatedFieldValue = value;
            }
        }

        public void AfterEstablish()
        {
			short iFNOActive = 0;
            short iFNORelated = 0;
            List<short> featureList = new List<short>();
            ADODB.Recordset rs;
            int cnt = 0;
            try
            {
                if (m_oActiveComponents[m_sActiveComponentName].Recordset != null)
                {
                    if (!(m_oActiveComponents[m_sActiveComponentName].Recordset.EOF && m_oActiveComponents[m_sActiveComponentName].Recordset.BOF))
                    {
                        m_oActiveComponents[m_sActiveComponentName].Recordset.MoveFirst();
                        iFNOActive = Convert.ToInt16(m_oActiveComponents[m_sActiveComponentName].Recordset.Fields["g3e_fno"].Value);
                    }
                }

                if (m_oRelatedComponents[m_sRelatedComponentName].Recordset != null)
                {
                    if (!(m_oRelatedComponents[m_sRelatedComponentName].Recordset.EOF && m_oRelatedComponents[m_sRelatedComponentName].Recordset.BOF))
                    {
                        m_oRelatedComponents[m_sRelatedComponentName].Recordset.MoveFirst();
                        iFNORelated = Convert.ToInt16(m_oRelatedComponents[m_sRelatedComponentName].Recordset.Fields["g3e_fno"].Value);
                    }
                }

                string sql = @"select g3e_fno from g3e_featurecomps_optable where g3e_fno in 
								(SELECT f.g3e_fno FROM g3e_features_optable f,  g3e_featurecomps_optable C 
								WHERE c.g3e_cno=F.G3e_Primarygeographiccno and c.g3e_type=16)
								and g3e_cno=1";

                rs = m_oDataContext.Execute(sql, out cnt, (int)ADODB.CommandTypeEnum.adCmdText);
                if (rs != null)
                {
                    if (!(rs.EOF && rs.BOF))
                    {
                        rs.MoveFirst();
                        for (int i = 0; i < rs.RecordCount; i++)
                        {
                            featureList.Add(Convert.ToInt16(rs.Fields["g3e_fno"].Value));
                            rs.MoveNext();
                        }
                    }
                }
                if (featureList.Contains(iFNOActive))
                {
                    if (m_oRelatedComponents[m_OwnerComponent].Recordset != null)
                    {
                        if (!(m_oRelatedComponents[m_OwnerComponent].Recordset.EOF && m_oRelatedComponents[m_OwnerComponent].Recordset.BOF))
                        {
                            m_oRelatedComponents[m_OwnerComponent].Recordset.MoveFirst();
                            if (m_oActiveComponents[m_sActiveComponentName].Recordset != null)
                            {
                                if (!(m_oActiveComponents[m_sActiveComponentName].Recordset.EOF && m_oActiveComponents[m_sActiveComponentName].Recordset.BOF))
                                {
                                    m_oActiveComponents[m_OwnerComponent].Recordset.MoveFirst();
                                    m_oActiveComponents[m_OwnerComponent].Recordset.Fields["STRUCTURE_ID"].Value = m_oRelatedComponents[m_OwnerComponent].Recordset.Fields["STRUCTURE_ID"].Value;
                                    m_oActiveComponents[m_OwnerComponent].Recordset.Fields["LATITUDE"].Value = m_oRelatedComponents[m_OwnerComponent].Recordset.Fields["LATITUDE"].Value;
                                    m_oActiveComponents[m_OwnerComponent].Recordset.Fields["LONGITUDE"].Value = m_oRelatedComponents[m_OwnerComponent].Recordset.Fields["LONGITUDE"].Value;
                                }
                            }
                        }
                    }

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error in Owner Location Relationship Attribute Interface \n" + ex.Message, "G/Technology");
            }

        }

        public void Validate(out string[] ErrorPriorityArray, out string[] ErrorMessageArray)
        {
            ErrorPriorityArray = new string[1];
            ErrorMessageArray = new string[1];
        }
    }
}
