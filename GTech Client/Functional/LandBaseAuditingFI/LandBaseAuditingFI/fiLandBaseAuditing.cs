//----------------------------------------------------------------------------+
//        Class: fiLandBaseAuditing
//  Description: Provides a functional interface for the landbase auditing attributes
//----------------------------------------------------------------------------+
//     $Author:: Shubham Agarwal (sagarwal)                                                      $
//       $Date:: 14/07/17                                                     $
//   $Revision:: 1                                                            $
//----------------------------------------------------------------------------+
//    $History::                                         $
// 
// *****************  Version 1  *****************
// User: sagarwal     Date: 14/07/17    Time: 18:00
//----------------------------------------------------------------------------+

using System;
using Intergraph.GTechnology.API;
using Intergraph.GTechnology.Interfaces;
using System.Windows.Forms;
using System.Collections.Generic;
using ADODB;

namespace GTechnology.Oncor.CustomAPI
{ 
    public class fiLandBaseAuditing : IGTFunctional
    {
        #region Private Members
        GTArguments m_gtArguments = null;
        string m_gtComponent = null;
        IGTComponents m_gtComponentCollection = null;
        IGTDataContext m_gtDataContext = null;
        string m_sFieldName = string.Empty;
        IGTFieldValue m_gtFieldValue = null;
        GTFunctionalTypeConstants m_gtFunctionalTypeConstant;

        int m_iuserANONew = 0;
        int m_idateANONew = 0;
        int m_iuserANOUpdate = 0;
        int m_idateANOUpdate = 0;
        private List<short> m_LandbaseManualFNOs = new List<short>(new short[] { 217,224,228,232 });

        #endregion

        #region IGTFunctional Members
        public GTArguments Arguments
        {
            get
            {
                return m_gtArguments;
            }

            set
            {
                m_gtArguments = value;
                m_iuserANONew = Convert.ToInt32(m_gtArguments.GetArgument(0));
                m_idateANONew = Convert.ToInt32(m_gtArguments.GetArgument(1));
                m_iuserANOUpdate = Convert.ToInt32(m_gtArguments.GetArgument(2));
                m_idateANOUpdate = Convert.ToInt32(m_gtArguments.GetArgument(3));
            }
        }

        public string ComponentName
        {
            get
            {
                return m_gtComponent;
            }

            set
            {
                m_gtComponent = value;
            }
        }

        public IGTComponents Components
        {
            get
            {
                return m_gtComponentCollection;
            }

            set
            {
                m_gtComponentCollection = value;
            }
        }

        public IGTDataContext DataContext
        {
            get
            {
                return m_gtDataContext;
            }

            set
            {
                m_gtDataContext = value;
            }
        }

        public string FieldName
        {
            get
            {
                return m_sFieldName;
            }

            set
            {
                m_sFieldName = value;
            }
        }

        public IGTFieldValue FieldValueBeforeChange
        {
            get
            {
                return m_gtFieldValue;
            }

            set
            {
                m_gtFieldValue = value;
            }
        }

        public GTFunctionalTypeConstants Type
        {
            get
            {
                return m_gtFunctionalTypeConstant;
            }

            set
            {
                m_gtFunctionalTypeConstant = value;
            }
        }

        public void Delete()
        {
            try
            {
                SetLandbaseAttributes();
            }
            catch (Exception ex)
            {
                MessageBox.Show("The error occured at fiLandBaseAuditing " + ex.Message, "G/Technology");
            }
        }

        public void Execute()
        {
            try
            {
                SetLandbaseAttributes();
            }
            catch (Exception ex)
            {
                MessageBox.Show("The error occured at fiLandBaseAuditing " + ex.Message,"G/Technology");               
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
        /// SetLandbaseAttributes
        /// </summary>
        private void SetLandbaseAttributes()
        {
            Recordset attributeRS = null;
            string userANONewFieldName = string.Empty;
            string dateANONewFieldName = string.Empty;
            string userANOUpdateFieldName = string.Empty;
            string dateANOUpdateFieldName = string.Empty;
            IGTComponent landbaseAuditingComp = null;
            short landbaseAuditingCNO = 3;
           
            try
            {
                landbaseAuditingComp = Components.GetComponent(landbaseAuditingCNO);

                if (CheckValidManualLandbaseFeature(landbaseAuditingComp))
                {
                    attributeRS = m_gtDataContext.MetadataRecordset("G3E_ATTRIBUTEINFO_OPTABLE");

                    if (attributeRS != null)
                    {
                        GetAttributeField(attributeRS, m_iuserANONew, ref userANONewFieldName);

                        GetAttributeField(attributeRS, m_idateANONew, ref dateANONewFieldName);

                        GetAttributeField(attributeRS, m_iuserANOUpdate, ref userANOUpdateFieldName);

                        GetAttributeField(attributeRS, m_idateANOUpdate, ref dateANOUpdateFieldName);

                        attributeRS.Filter = FilterGroupEnum.adFilterNone;
                    }

                    string sql = "select user, SYSDATE from dual";

                    attributeRS = m_gtDataContext.OpenRecordset(sql, CursorTypeEnum.adOpenDynamic, LockTypeEnum.adLockOptimistic, (int)CommandTypeEnum.adCmdText, new object[1]);
                    attributeRS.MoveFirst();

                    if (Type == GTFunctionalTypeConstants.gtftcAddNew)
                    {
                        if (landbaseAuditingComp != null && landbaseAuditingComp.Recordset.RecordCount > 0)
                        {
                            landbaseAuditingComp.Recordset.MoveFirst();

                            landbaseAuditingComp.Recordset.Fields[userANONewFieldName].Value = attributeRS.Fields["user"].Value;
                            landbaseAuditingComp.Recordset.Fields[dateANONewFieldName].Value = attributeRS.Fields["sysdate"].Value;
                        }
                    }
                    else if (Type == GTFunctionalTypeConstants.gtftcUpdate)
                    {
                        if (landbaseAuditingComp != null && landbaseAuditingComp.Recordset.RecordCount > 0)
                        {
                            landbaseAuditingComp.Recordset.MoveFirst();

                            landbaseAuditingComp.Recordset.Fields[userANOUpdateFieldName].Value = attributeRS.Fields["user"].Value;
                            landbaseAuditingComp.Recordset.Fields[dateANOUpdateFieldName].Value = attributeRS.Fields["sysdate"].Value;
                        }
                    }
                }
            }
            catch
            {
                throw;
            }
        }

        private bool CheckValidManualLandbaseFeature(IGTComponent landbaseAuditingComp)
        {
            short m_ActiveFNO = 0;
            bool valid = false;
            try
            {
                if (landbaseAuditingComp != null && landbaseAuditingComp.Recordset.RecordCount > 0)
                {
                    landbaseAuditingComp.Recordset.MoveFirst();

                    m_ActiveFNO = Convert.ToInt16(landbaseAuditingComp.Recordset.Fields["G3E_FNO"].Value);

                    if (m_LandbaseManualFNOs.Contains(m_ActiveFNO))
                    {
                        valid = true;
                    }
                }
            }
            catch
            {
                throw;
            }
            return valid;
        }

        private void GetAttributeField(Recordset p_attributeRS, int p_ANO, ref string p_ANOFieldName)
        {
            try
            {
                p_attributeRS.Filter = FilterGroupEnum.adFilterNone;
                p_attributeRS.Filter = "G3E_ANO = " + p_ANO;
                p_attributeRS.MoveFirst();

                p_ANOFieldName = Convert.ToString(p_attributeRS.Fields["G3E_FIELD"].Value);
            }
            catch
            {
                throw;
            }
        }
        #endregion
    }
}
