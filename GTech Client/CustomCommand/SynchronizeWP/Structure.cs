// =====================================================================================================================================================================
//  File Name: Structure.cs
// 
// Description:  Command to synchronize work points
//  Remarks:
// 
//  Modification History
//  Date                Author                      Comments
//  --------            --------------              --------
//  03/25/2019          Shubham                       Initial implementation
//=====================================================================================================================================================================

using System;
using System.Collections.Generic;
using System.Linq;
using Intergraph.GTechnology.API;
namespace GTechnology.Oncor.CustomAPI
{
   public class Structure
    {
        private IGTKeyObject m_StructureKeyObject;
        private IGTApplication m_oApp;
        public Structure(IGTKeyObject p_keyObject, IGTApplication p_oApp)
        {
            m_StructureKeyObject = p_keyObject;
            m_oApp = p_oApp;
        }
        public Dictionary<IGTKeyObject,IGTComponents> GetOwnedFeatureCollection()
        {
            Dictionary<IGTKeyObject, IGTComponents> oReturnCollection = new Dictionary<IGTKeyObject, IGTComponents>();
            IGTJobManagementService oJobManagement = GTClassFactory.Create<IGTJobManagementService>();

            try
            {
                oJobManagement.DataContext = m_oApp.DataContext;

                IGTKeyObjects oRelatedFeatures = GetRelatedFeaturesAtStructure(m_StructureKeyObject);
                ADODB.Recordset rs = oJobManagement.FindPendingEdits();

                if (rs != null && rs.RecordCount > 0)
                {
                    Features oFeatureCollection = new Features(oRelatedFeatures);
                    oReturnCollection = oFeatureCollection.GetFeatureComponentCollection();
                }
            }
            catch (Exception ex)
            {
                string exMsg = string.Format("Error occurred in {0} of {1}.{2}{3}", System.Reflection.MethodBase.GetCurrentMethod().Name, this.ToString(), Environment.NewLine, ex.Message);
                throw new Exception(exMsg);
            }
                 

            return oReturnCollection;
        }
        private IGTKeyObjects GetRelatedFeaturesAtStructure(IGTKeyObject p_Structure)
        {
            IGTKeyObjects oKeyObjectsCollection = null;
            using (IGTRelationshipService oRelSvc = GTClassFactory.Create<IGTRelationshipService>())
            {
                oRelSvc.DataContext = m_oApp.DataContext;
                oRelSvc.ActiveFeature = p_Structure;

                try
                {
                    oKeyObjectsCollection = oRelSvc.GetRelatedFeatures(2);
                }
                catch (Exception)
                {


                }
            }
            return oKeyObjectsCollection;
        }
    }

    public class Features
    {
        private IGTKeyObjects m_featureKeyObject;
        private IGTApplication m_oApp = null;
        public Features(IGTKeyObjects p_keyObjects)
        {
            m_featureKeyObject = p_keyObjects;
            m_oApp = GTClassFactory.Create<IGTApplication>();
        }

        public Dictionary<IGTKeyObject, IGTComponents> GetFeatureComponentCollection()
        {
            Dictionary<IGTKeyObject, IGTComponents> oReturnCollection = new Dictionary<IGTKeyObject, IGTComponents>();
            IGTJobManagementService oJobManagement = GTClassFactory.Create<IGTJobManagementService>();

            try
            {
                oJobManagement.DataContext = m_oApp.DataContext;
                ADODB.Recordset rs = oJobManagement.FindPendingEdits();

                if (rs != null && rs.RecordCount > 0)
                {
                    foreach (IGTKeyObject item in m_featureKeyObject)
                    {
                        Feature obj = new Feature(item, m_oApp);
                        rs.Filter = "g3e_fid = " + item.FID;

                        if (rs != null && rs.RecordCount > 0)
                        {
                            IGTComponents oFeatureEditedComp = obj.GetEditedCUComponents(item, rs);

                            if (oFeatureEditedComp.Count > 0 && (!oReturnCollection.Contains(new KeyValuePair<IGTKeyObject, IGTComponents>(item, oFeatureEditedComp))))
                            {
                                oReturnCollection.Add(item, oFeatureEditedComp);
                            }
                        }
                        rs.Filter = ADODB.FilterGroupEnum.adFilterNone;
                    }
                }

            }
            catch (Exception ex)
            {
                string exMsg = string.Format("Error occurred in {0} of {1}.{2}{3}", System.Reflection.MethodBase.GetCurrentMethod().Name, this.ToString(), Environment.NewLine, ex.Message);
                throw new Exception(exMsg);
            }
        
            return oReturnCollection;
        }
    }

    public class Feature
    {
        private IGTKeyObject m_featureKeyObject;
        private IGTApplication m_oApp;

        public IGTComponents GetEditedCUCollection()
        {
            IGTComponents oEditedCUCollection = null;
            return oEditedCUCollection;

        }
        private Int16 IsPrimaryGraphic(int p_FNO, IGTApplication p_oApp)
        {
            Int16 iCNO = 0;
            ADODB.Recordset rs = m_oApp.DataContext.MetadataRecordset("G3E_FEATURES_OPTABLE", "g3e_fno = " + p_FNO);
            iCNO = Convert.ToInt16(rs.Fields["G3E_PRIMARYGEOGRAPHICCNO"].Value);

            return iCNO;
        }
        public Feature(IGTKeyObject p_keyObject, IGTApplication p_oApp)
        {
            m_oApp = p_oApp;
            m_featureKeyObject = p_keyObject;
        }
        private Int16 IsPrimaryGraphic(int p_FNO)
        {
            Int16 iCNO = 0;
            ADODB.Recordset rs = m_oApp.DataContext.MetadataRecordset("G3E_FEATURES_OPTABLE", "g3e_fno = " + p_FNO);
            iCNO = Convert.ToInt16(rs.Fields["G3E_PRIMARYGEOGRAPHICCNO"].Value);
            return iCNO;
        }

        private bool IsWPExist(string p_StrucureID)
        {
            bool bReturn = false;
            ADODB.Recordset rs = m_oApp.DataContext.OpenRecordset("select count(*) from workpoint_n where structure_id = ?", ADODB.CursorTypeEnum.adOpenDynamic, ADODB.LockTypeEnum.adLockOptimistic, (int)ADODB.CommandTypeEnum.adCmdText, p_StrucureID);

            if (rs!=null)
            {
                if (rs.RecordCount>0)
                {
                    rs.MoveFirst();
                    bReturn = Convert.ToInt32(rs.Fields[0].Value) == 1;
                }
            }
            return bReturn;
        }
        public IGTComponents GetEditedCUComponents(IGTKeyObject p_KeyObject, ADODB.Recordset p_RecordSet)
        {
            IGTComponents oEditedComponentCollection = GTClassFactory.Create<IGTComponents>();
            Dictionary<Int32, Int16> oDic = new Dictionary<Int32, Int16>();
            bool bPrimaryGeoemtryEdited = false;
            bool bCommonComponentEdited = false;
            string sStrcurure_ID = string.Empty;
            bool bAncillaryEdited = false;
            bool bPrimaryCUEdited = false;

            try
            {
                if (p_RecordSet != null)
                {
                    if (p_RecordSet.RecordCount > 0)
                    {
                        p_RecordSet.MoveFirst();
                        while (p_RecordSet.EOF == false)
                        {
                            int FID = Convert.ToInt32(p_RecordSet.Fields["g3e_fid"].Value);
                            Int16 FNO = Convert.ToInt16(p_RecordSet.Fields["g3e_fno"].Value);
                            int CNO = Convert.ToInt32(p_RecordSet.Fields["g3e_cno"].Value);

                            if (CNO == 22)
                            {
                                oEditedComponentCollection.Add(p_KeyObject.Components.GetComponent(22));
                                bAncillaryEdited = true;
                            }
                            if (CNO == 21)
                            {
                                oEditedComponentCollection.Add(p_KeyObject.Components.GetComponent(21));
                                bPrimaryCUEdited = true;
                            }
                            if (CNO == 1)
                            {
                                bCommonComponentEdited = true;
                                p_KeyObject.Components.GetComponent(1).Recordset.MoveFirst();
                                sStrcurure_ID = Convert.ToString(p_KeyObject.Components.GetComponent(1).Recordset.Fields["STRUCTURE_ID"].Value);
                            }
                            if (CNO.Equals(IsPrimaryGraphic(FNO)))
                            {
                                bPrimaryGeoemtryEdited = true;
                            }
                            p_RecordSet.MoveNext();
                        }
                        if (bCommonComponentEdited && bPrimaryGeoemtryEdited && IsWPExist(sStrcurure_ID))
                        {
                            if (p_KeyObject.Components.GetComponent(22) != null)
                            {
                                if (!bAncillaryEdited)
                                {
                                    oEditedComponentCollection.Add(p_KeyObject.Components.GetComponent(22));
                                }
                            }
                            if (p_KeyObject.Components.GetComponent(21)!=null)
                            {
                                if (!bPrimaryCUEdited)
                                {
                                    oEditedComponentCollection.Add(p_KeyObject.Components.GetComponent(21));
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                string exMsg = string.Format("Error occurred in {0} of {1}.{2}{3}", System.Reflection.MethodBase.GetCurrentMethod().Name, this.ToString(), Environment.NewLine, ex.Message);
                throw new Exception(exMsg);
            }
           
            return oEditedComponentCollection;
        }       
    }
}
