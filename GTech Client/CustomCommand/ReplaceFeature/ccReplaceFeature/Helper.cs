using ADODB;
using System;
using Intergraph.GTechnology.API;

namespace GTechnology.Oncor.CustomAPI
{
    public class Helper
    {
        internal IGTDataContext m_dataContext;
        IGTApplication m_iGtApplication = GTClassFactory.Create<IGTApplication>();
        internal int m_fid;
        internal short m_fno;
        internal string m_featureState;
        internal int m_replacedFid;

        /// <summary>
        /// Method to check for corrections mode property
        /// </summary>
        /// <returns></returns>
        internal bool CheckForCorrectionModeProperty()
        {
            try
            {
                for (int i = 0; i < m_iGtApplication.Properties.Keys.Count; i++)
                {
                    if (string.Equals("CorrectionsMode", Convert.ToString(m_iGtApplication.Properties.Keys.Get(i))))
                    {
                        return true;
                    }
                }
                return false;
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Method to remove corrections mode property
        /// </summary>
        internal void RemoveCorrectionModeProperty()
        {
            try
            {
                m_iGtApplication.Properties.Remove("CorrectionsMode");
                string mapCaption = m_iGtApplication.ActiveMapWindow.Caption.Replace("CORRECTIONS MODE - ", string.Empty);
                m_iGtApplication.ActiveMapWindow.Caption = mapCaption;
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Method to execute sql query and return the result record set
        /// </summary>
        /// <param name="sqlString"></param>
        /// <returns></returns>
        internal Recordset GetRecordSet(string sqlString)
        {
            try
            {
                int outRecords = 0;
                Command command = new ADODB.Command();
                command.CommandText = sqlString;
                Recordset results = m_dataContext.ExecuteCommand(command, out outRecords);
                return results;
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Method to set feature state for the feature in context
        /// </summary>
        internal void SetFeatureState()
        {
            try
            {
                IGTKeyObject feature = m_dataContext.OpenFeature(m_fno, m_fid);
                IGTComponent commonComponent = feature.Components.GetComponent(1);
                if (commonComponent != null)
                {
                    Recordset commonComponentRs = commonComponent.Recordset;
                    if (commonComponentRs != null && commonComponentRs.RecordCount > 0)
                    {
                        commonComponentRs.MoveFirst();
                        commonComponentRs.Fields["FEATURE_STATE_C"].Value = m_featureState;
                        if (m_replacedFid > 0)
                        {
                            commonComponentRs.Fields["REPLACED_FID"].Value = m_replacedFid;
                        }
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        ///  Method to get the primary graphic cno for fno
        /// </summary>
        /// <param name="fNo"></param>
        /// <returns>cno or 0</returns>
        internal short GetPrimaryGraphicCno(short fNo)
        {
            short primaryGraphicCno = 0;
            try
            {
                Recordset tempRs = m_dataContext.MetadataRecordset("G3E_FEATURES_OPTABLE", "G3E_FNO = " + fNo);
                if (tempRs != null && tempRs.RecordCount > 0)
                {
                    tempRs.MoveFirst();
                    primaryGraphicCno = Convert.ToInt16(tempRs.Fields["G3E_PRIMARYGEOGRAPHICCNO"].Value);
                }
                return primaryGraphicCno;
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        ///  Method to check if the feature is an oriented point
        /// </summary>
        /// <param name="fNo"></param>
        /// <returns>cno or 0</returns>
        internal bool CheckIfPrimaryGraphicIsOrientedPoint()
        {
            int g3eType = 0;
            try
            {
                short cNo = GetPrimaryGraphicCno(m_fno);
                Recordset tempRs = m_dataContext.MetadataRecordset("G3E_COMPONENTINFO_OPTABLE", "G3E_CNO = " + cNo);
                if (tempRs != null && tempRs.RecordCount > 0)
                {
                    tempRs.MoveFirst();
                    g3eType = Convert.ToInt16(tempRs.Fields["G3E_TYPE"].Value);
                }
                // Oriented point geometry - g3eType = 16;
                return g3eType == 16;
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Method to get structure id of selected object
        /// </summary>
        /// <returns></returns>
        internal string GetStructureIdOfSelectedObject()
        {
            try
            {
                string structureId = string.Empty;
                IGTKeyObject structureFeature = m_dataContext.OpenFeature(m_fno, m_fid);
                IGTComponent commonComponent = structureFeature.Components.GetComponent(1);

                if (commonComponent != null)
                {
                    Recordset cCRs = commonComponent.Recordset;
                    if (cCRs != null && cCRs.RecordCount > 0)
                    {
                        cCRs.MoveFirst();
                        structureId = Convert.ToString(cCRs.Fields["STRUCTURE_ID"].Value);
                    }
                }
                return structureId;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
