//----------------------------------------------------------------------------+
//        Class: fiWorkPointSynchronization
//  Description: Synchronizes all CU and Ancillary CU activity from the associated structure and all features owned by that structure.
//----------------------------------------------------------------------------+
//     $Author:: Prathyusha Lella (pnlella)                                                      $
//       $Date:: 13/12/17                                                     $
//   $Revision:: 1                                                            $
//----------------------------------------------------------------------------+
//    $History::                                         $
// 
// *****************  Version 1  *****************
// User: pnlella     Date: 13/12/17    Time: 13:00
//----------------------------------------------------------------------------+

using System;
using ADODB;
using Intergraph.GTechnology.API;
using Intergraph.GTechnology.Interfaces;
using System.Windows.Forms;

namespace GTechnology.Oncor.CustomAPI
{
    public class fiWorkPointSynchronization : IGTFunctional
    {
        #region Private Members
        GTArguments m_gtArguments = null;
        string m_gtComponent = null;
        IGTComponents m_gtComponentCollection = null;
        IGTDataContext m_gtDataContext = null;
        string m_sFieldName = string.Empty;
        IGTFieldValue m_gtFieldValue = null;
        GTFunctionalTypeConstants m_gtFunctionalTypeConstant;
        IGTComponent m_gtWorkPointCUComp = null;
        int m_gtCIDValue = 0;
        int m_gtCount = 0;

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

        }

        public void Execute()
        {
            string structureID = null;
            string activeWR = null;
            Recordset structureCUAncillaryCURs = null;
            Recordset childCUAncillaryCURs = null;
            try
            {
                structureID = Convert.ToString(Components[ComponentName].Recordset.Fields[m_sFieldName].Value);
                if (!string.IsNullOrEmpty(structureID))
                {
                    activeWR = Convert.ToString(Components[ComponentName].Recordset.Fields["WR_NBR"].Value);
                    m_gtWorkPointCUComp = Components["WORKPOINT_CU_N"];
                    if (m_gtWorkPointCUComp.Recordset != null && m_gtWorkPointCUComp.Recordset.RecordCount > 0)
                    {
                        m_gtWorkPointCUComp.Recordset.MoveFirst();
                        while (!m_gtWorkPointCUComp.Recordset.EOF)
                        {
                            if (Convert.ToInt32(m_gtWorkPointCUComp.Recordset.Fields["ASSOC_FID"].Value) != 0)
                            {
                                m_gtWorkPointCUComp.Recordset.Delete();
                            }
                            m_gtWorkPointCUComp.Recordset.MoveNext();
                        }
                    }
                    structureCUAncillaryCURs = GetCUAncillaryCUDataForStructures(structureID, activeWR);
                    childCUAncillaryCURs = GetCUAncillaryCUDataStructuresOwnedFeatures(structureID, activeWR);
                    m_gtCIDValue = m_gtWorkPointCUComp.Recordset.RecordCount + 1;
                    if (structureCUAncillaryCURs != null || childCUAncillaryCURs != null)
                        SynchronizeCUAncillaryCUToWorkPoint(structureCUAncillaryCURs, childCUAncillaryCURs);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("There is an error in \"Work Point Synchronization\" Funtional Interface \n" + ex.Message, "G/Technology");
            }
            finally
            {
                if (structureCUAncillaryCURs != null) structureCUAncillaryCURs = null;
                if (childCUAncillaryCURs != null) childCUAncillaryCURs = null;
            }
        }

        /// <summary>
        /// Validate
        /// </summary>
        /// <param name="ErrorPriorityArray"></param>
        /// <param name="ErrorMessageArray"></param>
        public void Validate(out string[] ErrorPriorityArray, out string[] ErrorMessageArray)
        {
            ErrorPriorityArray = null;
            ErrorMessageArray = null;
        }
        #endregion

        #region Private Methods
        /// <summary>
        /// Method to return CU Attributes and Ancillary CU Attributes recordset for matched StructureID
        /// </summary>
        /// <param name="structureID"></param>
        /// <returns></returns>
        private Recordset GetCUAncillaryCUDataForStructures(string structureID, string activeWR)
        {
            Recordset attributeRS = null;
            try
            {
                attributeRS = DataContext.OpenRecordset(String.Format("Select CU.* from COMMON_N C,COMP_UNIT_N CU where CU.G3E_FID=C.G3E_FID AND CU.G3E_FNO=C.G3E_FNO AND C.G3E_FNO IN (106,107,108,109,110,113,114,116,117,120,2500) AND C.STRUCTURE_ID='{0}' AND CU.WR_ID='{1}' ORDER BY CU.G3E_FID,CU.G3E_CNO,CU.G3E_CID", structureID, activeWR), CursorTypeEnum.adOpenStatic,
                              LockTypeEnum.adLockReadOnly, (int)CommandTypeEnum.adCmdText);
            }
            catch
            {
                throw;
            }
            return attributeRS;
        }

        /// <summary>
        /// Method to return CU Attributes and Ancillary CU Attributes recordset child features of structures for matched StructureID
        /// </summary>
        /// <param name="structureID"></param>
        /// <returns></returns>
        private Recordset GetCUAncillaryCUDataStructuresOwnedFeatures(string structureID, string activeWR)
        {
            Recordset childAttributeRS = null;
            try
            {
                childAttributeRS = DataContext.OpenRecordset(String.Format("Select CU.* from COMMON_N C,COMP_UNIT_N CU where CU.G3E_FID=C.G3E_FID AND CU.G3E_FNO=C.G3E_FNO and (C.OWNER1_ID IN(SELECT G3E_ID FROM COMMON_N WHERE STRUCTURE_ID='{0}' AND G3E_FNO IN (106,107,108,109,110,113,114,116,117,120,2500)) OR C.OWNER2_ID IN(SELECT G3E_ID FROM COMMON_N WHERE STRUCTURE_ID='{0}' AND G3E_FNO IN (106,107,108,109,110,113,114,116,117,120,2500))) AND CU.WR_ID='{1}' ORDER BY CU.G3E_FID,CU.G3E_CNO,CU.G3E_CID", structureID, activeWR), CursorTypeEnum.adOpenStatic,
                              LockTypeEnum.adLockReadOnly, (int)CommandTypeEnum.adCmdText);
            }
            catch
            {
                throw;
            }
            return childAttributeRS;
        }

        /// <summary>
        /// Method to synchronizes all CU and Ancillary CU activity from the associated structure and all features owned by that structure
        /// </summary>
        /// <param name="structureRs"></param>
        private void SynchronizeCUAncillaryCUToWorkPoint(Recordset structureRs, Recordset childFeaturesOfStructureRs)
        {
            Recordset recordset = null;
            try
            {

                if (m_gtCount == 0)
                    recordset = structureRs;
                else if (m_gtCount == 1)
                    recordset = childFeaturesOfStructureRs;
                else
                    return;
                if (recordset != null && recordset.RecordCount > 0)
                {
                    for (recordset.MoveFirst(); !recordset.EOF; recordset.MoveNext())
                    {
                        m_gtWorkPointCUComp.Recordset.AddNew("G3E_FID", Components[ComponentName].Recordset.Fields["G3E_FID"].Value);
                        m_gtWorkPointCUComp.Recordset.Fields["G3E_FNO"].Value = Components[ComponentName].Recordset.Fields["G3E_FNO"].Value;
                        m_gtWorkPointCUComp.Recordset.Fields["G3E_CNO"].Value = m_gtWorkPointCUComp.CNO;
                        m_gtWorkPointCUComp.Recordset.Fields["G3E_CID"].Value = m_gtCIDValue;
                        m_gtWorkPointCUComp.Recordset.Fields["ACTIVITY_C"].Value = recordset.Fields["ACTIVITY_C"].Value;
                        m_gtWorkPointCUComp.Recordset.Fields["CIAC_C"].Value = recordset.Fields["CIAC_C"].Value;
                        m_gtWorkPointCUComp.Recordset.Fields["CU_C"].Value = recordset.Fields["CU_C"].Value;
                        m_gtWorkPointCUComp.Recordset.Fields["CU_DESC"].Value = recordset.Fields["CU_DESC"].Value;
                        m_gtWorkPointCUComp.Recordset.Fields["LENGTH_FLAG"].Value = recordset.Fields["LENGTH_FLAG"].Value;
                        m_gtWorkPointCUComp.Recordset.Fields["MACRO_CU_C"].Value = recordset.Fields["MACRO_CU_C"].Value;
                        m_gtWorkPointCUComp.Recordset.Fields["PRIME_ACCT_ID"].Value = recordset.Fields["PRIME_ACCT_ID"].Value;
                        m_gtWorkPointCUComp.Recordset.Fields["QTY_LENGTH_Q"].Value = recordset.Fields["QTY_LENGTH_Q"].Value;
                        m_gtWorkPointCUComp.Recordset.Fields["VINTAGE_YR"].Value = recordset.Fields["VINTAGE_YR"].Value;
                        m_gtWorkPointCUComp.Recordset.Fields["WM_SEQ"].Value = recordset.Fields["WM_SEQ"].Value;
                        m_gtWorkPointCUComp.Recordset.Fields["ASSOC_FID"].Value = recordset.Fields["G3E_FID"].Value;
                        m_gtWorkPointCUComp.Recordset.Fields["ASSOC_FNO"].Value = recordset.Fields["G3E_FNO"].Value;
                        m_gtWorkPointCUComp.Recordset.Fields["UNIT_CNO"].Value = recordset.Fields["G3E_CNO"].Value;
                        m_gtWorkPointCUComp.Recordset.Fields["UNIT_CID"].Value = recordset.Fields["G3E_CID"].Value;
                        m_gtCIDValue++;
                    }
                }
                m_gtCount++;
                SynchronizeCUAncillaryCUToWorkPoint(null, childFeaturesOfStructureRs);
            }
            catch
            {
                throw;
            }
            finally
            {
                if (recordset != null) recordset = null;
            }
        }
        #endregion
    }
}
