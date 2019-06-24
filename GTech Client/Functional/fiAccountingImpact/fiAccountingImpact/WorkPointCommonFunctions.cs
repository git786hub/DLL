using System;
using Intergraph.GTechnology.API;
using ADODB;
using System.Collections.Generic;

namespace GTechnology.Oncor.CustomAPI
{
    public class WorkPointCommonFunctions
    {
        #region Private Variables

        IGTKeyObject m_ActiveKeyObject;
        IGTDataContext m_gTDataContext;
        public IGTComponent m_gTActiveComponent = null;
        public IGTComponents m_gTComponents = null;

        #endregion

        #region Constructor
        public WorkPointCommonFunctions(IGTKeyObject ActiveKeyObject, IGTDataContext gTDataContext)
        {
            m_ActiveKeyObject = ActiveKeyObject;
            m_gTDataContext = gTDataContext;
        }

        #endregion

        #region Private methods
        /// <summary>
        /// If the triggering component = CU / Ancillary CU Attributes:Delete the corresponding instance of Work Point CU Attributes.
        /// </summary>
        public void DeleteWorkpointCUInstace(CommonFunctions m_gtcommonFunctions)
        {
            IGTKeyObjects gTOwnerWPKeyObjects = GTClassFactory.Create<IGTKeyObjects>();
            try
            {
                if (m_gtcommonFunctions.IsActiveFeatureIsLinear(m_ActiveKeyObject.FNO))
                {
                    IGTKeyObjects gTOwnerKeyObjects = LocateOwnersOfActiveFeature();

                    if (gTOwnerKeyObjects.Count > 0)
                    {
                        foreach (IGTKeyObject gtOKeyObject in gTOwnerKeyObjects)
                        {
                            gTOwnerWPKeyObjects = FindWorkpointsOfFeature(gtOKeyObject.Components.GetComponent(1));

                            foreach (IGTKeyObject gtWp in gTOwnerWPKeyObjects)
                            {
                                DeleteCUInstanceMatched(gtWp.Components["WORKPOINT_CU_N"]);

                                DeleteWorkpoint(gtWp);
                            }
                        }
                    }
                }
                else
                {
                    gTOwnerWPKeyObjects = FindWorkpointsOfFeature(m_gTComponents.GetComponent(1));

                    foreach (IGTKeyObject gtWp in gTOwnerWPKeyObjects)
                    {
                        DeleteCUInstanceMatched(gtWp.Components["WORKPOINT_CU_N"]);

                        DeleteWorkpoint(gtWp);
                    }
                }

            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// To find owners of a linear feature.
        /// </summary>
        /// <returns></returns>
        public IGTKeyObjects LocateOwnersOfActiveFeature()
        {
            IGTRelationshipService relationShipService = GTClassFactory.Create<IGTRelationshipService>();
            IGTKeyObjects gTOwnerKeyObjects = GTClassFactory.Create<IGTKeyObjects>();

            try
            {
                relationShipService.ActiveFeature = m_ActiveKeyObject;
                relationShipService.DataContext = m_gTDataContext;
                if (m_ActiveKeyObject.FNO != 2400)
                {
                    gTOwnerKeyObjects = relationShipService.GetRelatedFeatures(3);
                }
                else if (m_ActiveKeyObject.FNO == 2400)
                {
                    IGTKeyObjects gTFormationOwnerKeyObjects = relationShipService.GetRelatedFeatures(6);
                    if (gTFormationOwnerKeyObjects != null && gTFormationOwnerKeyObjects.Count > 0)
                    {
                        foreach (IGTKeyObject gTKeyObject in gTFormationOwnerKeyObjects)
                        {
                            relationShipService.ActiveFeature = gTKeyObject;
                            gTOwnerKeyObjects = relationShipService.GetRelatedFeatures(122);
                            if (gTOwnerKeyObjects != null && gTOwnerKeyObjects.Count > 0)
                                break;
                        }
                    }
                }
            }
            catch
            {

            }

            finally
            {
                relationShipService.Dispose();
                relationShipService = null;
            }
            return gTOwnerKeyObjects;
        }


        /// <summary>
        /// Identifies the Key Object that's the Owner1 and Owner2 of the Child Feature
        /// </summary>
        /// <param name="parentFeatures">IGTKeyObjects</param>
        /// <param name="childFeature">IGTKeyObject</param>
        /// <param name="owner1KeyObject">IGTKeyObject</param>
        /// <param name="owner2KeyObject">IGTKeyObject</param>
        /// <returns></returns>
        public void GetOwner1Owner2(IGTKeyObjects parentFeatures, IGTKeyObject childFeature, ref IGTKeyObject owner1KeyObject, ref IGTKeyObject owner2KeyObject)
        {
            if (DBNull.Value != childFeature.Components["COMMON_N"].Recordset.Fields["OWNER1_ID"].Value)
            {
                int owner1ID = Convert.ToInt32(childFeature.Components["COMMON_N"].Recordset.Fields["OWNER1_ID"].Value);

                foreach (IGTKeyObject ko in parentFeatures)
                {
                    if (null != ko.Components["COMMON_N"].Recordset)
                    {
                        ko.Components["COMMON_N"].Recordset.MoveFirst();

                        if (Convert.ToInt32(ko.Components["COMMON_N"].Recordset.Fields["G3E_ID"].Value) == owner1ID)
                        {
                            owner1KeyObject = ko;
                            break;
                        }
                    }
                }
            }
            if (DBNull.Value != childFeature.Components["COMMON_N"].Recordset.Fields["OWNER2_ID"].Value)
            {
                int owner2ID = Convert.ToInt32(childFeature.Components["COMMON_N"].Recordset.Fields["OWNER2_ID"].Value);

                foreach (IGTKeyObject ko in parentFeatures)
                {
                    if (null != ko.Components["COMMON_N"].Recordset)
                    {
                        ko.Components["COMMON_N"].Recordset.MoveFirst();

                        if (Convert.ToInt32(ko.Components["COMMON_N"].Recordset.Fields["G3E_ID"].Value) == owner2ID)
                        {
                            owner2KeyObject = ko;
                            break;
                        }
                    }
                }
            }
        }


        /// <summary>
        /// Locate the associated Work Point feature (if it exists) for the active WR job.
        /// </summary>
        /// <param name="gTCommonComponent">Common component</param>
        /// <returns></returns>
        public IGTKeyObjects FindWorkpointsOfFeature(IGTComponent gTCommonComponent)
        {
            IGTKeyObjects workPointKeyObjects = GTClassFactory.Create<IGTKeyObjects>();

            try
            {
                if (gTCommonComponent != null && gTCommonComponent.Recordset.RecordCount > 0)
                {
                    gTCommonComponent.Recordset.MoveFirst();

                    if (DBNull.Value != gTCommonComponent.Recordset.Fields["STRUCTURE_ID"].Value)
                    {
                        string strStructureId = Convert.ToString(gTCommonComponent.Recordset.Fields["STRUCTURE_ID"].Value);

                        if (!string.IsNullOrEmpty(strStructureId))
                        {
                            string sql = "select g3e_fid,g3e_fno from workpoint_n where STRUCTURE_ID=? and wr_nbr=?";
                            Recordset rs = m_gTDataContext.OpenRecordset(sql, CursorTypeEnum.adOpenStatic, LockTypeEnum.adLockReadOnly, (int)CommandTypeEnum.adCmdText, strStructureId, m_gTDataContext.ActiveJob);

                            if (rs != null && 0 < rs.RecordCount)
                            {

                                if (1 < rs.RecordCount)
                                {
                                    // If we get multiple workpoints for the same structure that below to the same WR,
                                    // then that's an error and validation should stop.
                                    throw new Exception("Multiple Workpoints found having the same Structure ID and WR values.", new Exception("MULTIPLE WORKPOINTS"));
                                }

                                rs.MoveFirst();

                                do
                                {
                                    workPointKeyObjects.Add(m_gTDataContext.OpenFeature(Convert.ToInt16(rs.Fields["g3e_fno"].Value), Convert.ToInt32(rs.Fields["g3e_fid"].Value)));
                                    rs.MoveNext();
                                } while (!rs.EOF);

                            }
                        }
                    }
                }
            }
            catch
            {
                throw;
            }

            return workPointKeyObjects;
        }

        /// <summary>
        /// If the triggering component = CU / Ancillary CU Attributes:Delete the corresponding instance of Work Point CU Attributes.
        /// </summary>
        /// <param name="gtWorkpointCUComp">Workpoint CU Component</param>
        /// <returns></returns>
        public bool DeleteCUInstanceMatched(IGTComponent gtWorkpointCUComp)
        {
            int aFid = 0;
            short aFno = 0;

            int aCid = 0;
            short aCno = 0;

            int wCUAFid = 0;
            short wCUAFno = 0;

            short wCUACno = 0;
            int wCUACid = 0;

            try
            {
                Recordset rsWorkpoint = null;

                aFid = Convert.ToInt32(m_gTActiveComponent.Recordset.Fields["G3E_FID"].Value);
                aFno = Convert.ToInt16(m_gTActiveComponent.Recordset.Fields["G3E_FNO"].Value);
                aCno = Convert.ToInt16(m_gTActiveComponent.CNO);
                aCid = Convert.ToInt32(m_gTActiveComponent.Recordset.Fields["G3E_CID"].Value);

                if (gtWorkpointCUComp.Recordset != null && gtWorkpointCUComp.Recordset.RecordCount > 0)
                {

                    rsWorkpoint = gtWorkpointCUComp.Recordset;

                    rsWorkpoint.MoveFirst();

                    while (!rsWorkpoint.EOF)
                    {
                        if (!string.IsNullOrEmpty(Convert.ToString(rsWorkpoint.Fields["ASSOC_FID"].Value)))
                        {
                            wCUAFid = Convert.ToInt32(rsWorkpoint.Fields["ASSOC_FID"].Value);
                        }

                        if (!string.IsNullOrEmpty(Convert.ToString(rsWorkpoint.Fields["ASSOC_FNO"].Value)))
                        {
                            wCUAFno = Convert.ToInt16(rsWorkpoint.Fields["ASSOC_FNO"].Value);
                        }

                        if (!string.IsNullOrEmpty(Convert.ToString(rsWorkpoint.Fields["UNIT_CNO"].Value)))
                        {
                            wCUACno = Convert.ToInt16(rsWorkpoint.Fields["UNIT_CNO"].Value);
                        }

                        if (!string.IsNullOrEmpty(Convert.ToString(rsWorkpoint.Fields["UNIT_CID"].Value)))
                        {
                            wCUACid = Convert.ToInt32(rsWorkpoint.Fields["UNIT_CID"].Value);
                        }

                        if (wCUAFid == aFid && wCUAFno == aFno && wCUACno == aCno && wCUACid == aCid)
                        {
                            rsWorkpoint.Delete();
                        }

                        rsWorkpoint.MoveNext();
                    }
                }
            }
            catch
            {
                throw;
            }

            return false;
        }

        /// <summary>
        /// If the triggering component = CU / Ancillary CU Attributes:If no instances of Work Point CU Attributes or Voucher Attributes exist on the Work Point, delete the Work Point.
        /// </summary>
        /// <param name="gtWp"> Worpoint Key Object</param>
        public void DeleteWorkpoint(IGTKeyObject gtWp)
        {
            Recordset rsTemp = null;
            try
            {
                if ((gtWp.Components["WORKPOINT_CU_N"] == null || gtWp.Components["WORKPOINT_CU_N"].Recordset.RecordCount <= 0) &&
                    (gtWp.Components.GetComponent(19001) == null || gtWp.Components.GetComponent(19001).Recordset.RecordCount <= 0))
                {
                    String sql = "SELECT G3E_NAME FROM G3E_FEATURECOMPS_OPTABLE WHERE G3E_FNO  = ? ORDER BY G3E_DELETEORDINAL";
                    rsTemp = m_gTDataContext.OpenRecordset(sql, ADODB.CursorTypeEnum.adOpenDynamic, ADODB.LockTypeEnum.adLockReadOnly,
                             (int)ADODB.CommandTypeEnum.adCmdText, gtWp.FNO);

                    for (int i = 0; i < rsTemp.RecordCount; i++)
                    {
                        foreach (IGTComponent comp in gtWp.Components)
                        {
                            if (comp.Recordset != null && comp.Recordset.RecordCount > 0 && comp.Name == Convert.ToString(rsTemp.Fields["G3E_NAME"].Value))
                            {
                                comp.Recordset.MoveFirst();
                                while (!comp.Recordset.EOF)
                                {
                                    comp.Recordset.Delete();
                                    comp.Recordset.MoveNext();
                                }
                                break;
                            }
                        }
                        rsTemp.MoveNext();
                    }
                }
            }
            catch
            {
                throw;
            }
            finally
            {
                rsTemp = null;
            }
        }

        /// <summary>
        /// If the triggering component is Ancillary CU Attributes,If the Unit CNO, Unit CID, and Ancillary Attribute attributes are populated.
        /// Find the corresponding attribute on the active feature,If found, set the attribute to null.
        /// </summary>
        public void SetAncillaryAttribute()
        {
            int ancillaryAno = 0;
            string sql = "";
            Recordset rsAncillaryFeature = null;
            Recordset rsAncillaryAno = null;
            int count = 0;
            short cno = 0;
            string strField = "";

            try
            {
                Recordset rsAncillary = m_gTActiveComponent.Recordset;
                if (!string.IsNullOrEmpty(Convert.ToString(rsAncillary.Fields["UNIT_CNO"].Value)) && Convert.ToInt16(rsAncillary.Fields["UNIT_CNO"].Value) > 0)
                {
                    if (!string.IsNullOrEmpty(Convert.ToString(rsAncillary.Fields["UNIT_CID"].Value)) && Convert.ToInt16(rsAncillary.Fields["UNIT_CID"].Value) > 0)
                    {
                        if (!string.IsNullOrEmpty(Convert.ToString(rsAncillary.Fields["ACU_ANO"].Value)) && Convert.ToInt32(rsAncillary.Fields["ACU_ANO"].Value) > 0)
                        {
                            ancillaryAno = Convert.ToInt32(rsAncillary.Fields["ACU_ANO"].Value);
                            sql = "select g3e_cno,g3e_field from G3E_ATTRIBUTEINFO_OPTABLE where g3e_ano=:1 and g3e_cno in (select distinct g3e_cno from G3E_FEATURECOMPS_OPTABLE where g3e_fno=:2)";
                            rsAncillaryFeature = m_gTDataContext.Execute(sql, out count, (int)ADODB.CommandTypeEnum.adCmdText, ancillaryAno, m_ActiveKeyObject.FNO);

                            if (rsAncillaryFeature != null && rsAncillaryFeature.RecordCount > 0)
                            {
                                rsAncillaryFeature.MoveFirst();
                                cno = Convert.ToInt16(rsAncillaryFeature.Fields[0].Value);
                                strField = Convert.ToString(rsAncillaryFeature.Fields[1].Value);

                                rsAncillaryAno = m_ActiveKeyObject.Components.GetComponent(cno).Recordset;
                                if (rsAncillaryAno != null)
                                {
                                    rsAncillaryAno.MoveFirst();
                                    while (!rsAncillaryAno.EOF)
                                    {
                                        if (Convert.ToInt16(rsAncillaryAno.Fields["G3E_CNO"].Value) == Convert.ToInt16(rsAncillary.Fields["UNIT_CNO"].Value) &&
                                            Convert.ToInt32(rsAncillaryAno.Fields["G3E_CID"].Value) == Convert.ToInt32(rsAncillary.Fields["UNIT_CID"].Value))
                                        {
                                            rsAncillaryAno.Fields[strField].Value = "";
                                            rsAncillaryAno.Update();
                                        }
                                        rsAncillaryAno.MoveNext();
                                    }
                                }
                            }
                        }
                    }
                }
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Validates the active feature.
        /// </summary>
        /// <param name="errorPriorityNoWorkpoint"></param>
        /// <param name="errorPriorityNoMatchingWorkPoint"></param>
        /// <param name="p_lstErrorMessage"></param>
        /// <param name="p_lstErrorPriority"></param>
        /// <param name="m_gtcommonFunctions"></param>
        public void ValidateFeature(string errorPriorityNoWorkpoint, string errorPriorityNoMatchingWorkPoint, ref List<string> p_lstErrorMessage, ref List<string> p_lstErrorPriority, CommonFunctions m_gtcommonFunctions)
        {
            IGTKeyObjects gTKeyObjects = GTClassFactory.Create<IGTKeyObjects>();
            int relatedWP = 0;
            string strArgument1 = "";
            string strArgument2 = "";
            bool noWorkPoint = false;
            bool noMatchingWorkPoint = false;
            try
            {
                if (m_gtcommonFunctions.IsActiveFeatureIsLinear(m_ActiveKeyObject.FNO))
                {
                    IGTKeyObjects gTOwnerKeyObjects = LocateOwnersOfActiveFeature();
                    if (gTOwnerKeyObjects.Count > 0)
                    {
                        if (gTOwnerKeyObjects.Count == 2)
                        {
                            IGTKeyObject owner1KeyObject = GTClassFactory.Create<IGTKeyObject>();
                            IGTKeyObject owner2KeyObject = GTClassFactory.Create<IGTKeyObject>();

                            GetOwner1Owner2(gTOwnerKeyObjects, m_ActiveKeyObject, ref owner1KeyObject, ref owner2KeyObject);

                            CheckValidationForOwner1Feature(owner1KeyObject, ref relatedWP, ref noWorkPoint, ref noMatchingWorkPoint, ref strArgument1, ref strArgument2);

                            AddValidationsToArray(noWorkPoint, noMatchingWorkPoint, strArgument1, strArgument2, ref p_lstErrorMessage, ref p_lstErrorPriority, errorPriorityNoWorkpoint, errorPriorityNoMatchingWorkPoint);

                            noWorkPoint = false;
                            noMatchingWorkPoint = true;
                            strArgument1 = "";
                            strArgument2 = "";

                            CheckValidationForOwner2Feature(owner2KeyObject, relatedWP, ref noWorkPoint, ref noMatchingWorkPoint, ref strArgument1, ref strArgument2);

                            AddValidationsToArray(noWorkPoint, noMatchingWorkPoint, strArgument1, strArgument2, ref p_lstErrorMessage, ref p_lstErrorPriority, errorPriorityNoWorkpoint, errorPriorityNoMatchingWorkPoint);
                        }
                        else
                        {
                            CheckValidationForOwner1Feature(gTOwnerKeyObjects[0], ref relatedWP, ref noWorkPoint, ref noMatchingWorkPoint, ref strArgument1, ref strArgument2);

                            AddValidationsToArray(noWorkPoint, noMatchingWorkPoint, strArgument1, strArgument2, ref p_lstErrorMessage, ref p_lstErrorPriority, errorPriorityNoWorkpoint, errorPriorityNoMatchingWorkPoint);
                        }
                    }
                }
                else
                {
                    if (CheckForOwnedByRelation())
                    {
                        IGTKeyObjects gTOwnerKeyObjects = LocateOwnersOfActiveFeature();

                        if(gTOwnerKeyObjects.Count>0)
                        {
                            CheckValidationForOwner1Feature(gTOwnerKeyObjects[0], ref relatedWP, ref noWorkPoint, ref noMatchingWorkPoint, ref strArgument1, ref strArgument2);

                            AddValidationsToArray(noWorkPoint, noMatchingWorkPoint, strArgument1, strArgument2, ref p_lstErrorMessage, ref p_lstErrorPriority, errorPriorityNoWorkpoint, errorPriorityNoMatchingWorkPoint);
                        }
                    }
                    else
                    {

                        CheckValidationForOwner1Feature(m_ActiveKeyObject, ref relatedWP, ref noWorkPoint, ref noMatchingWorkPoint, ref strArgument1, ref strArgument2);

                        AddValidationsToArray(noWorkPoint, noMatchingWorkPoint, strArgument1, strArgument2, ref p_lstErrorMessage, ref p_lstErrorPriority, errorPriorityNoWorkpoint, errorPriorityNoMatchingWorkPoint);
                    }
                }
            }
            catch
            {
                throw;
            }

        }

        /// <summary>
        /// Checks whether active feature Owner2 feature has Work point or not and if present WP number is equal to Related WP.
        /// </summary>
        /// <param name="ownerKeyObject"></param>
        /// <param name="relatedWP"></param>
        /// <param name="noWorkPoint"></param>
        /// <param name="noMatchingWorkPoint"></param>
        /// <param name="strArgument1"></param>
        /// <param name="strArgument2"></param>
        private void CheckValidationForOwner2Feature(IGTKeyObject ownerKeyObject, int relatedWP, ref bool noWorkPoint, ref bool noMatchingWorkPoint, ref string strArgument1, ref string strArgument2)
        {
            IGTKeyObjects gTKeyObjects = GTClassFactory.Create<IGTKeyObjects>();
            try
            {
                ownerKeyObject.Components.GetComponent(1).Recordset.MoveFirst();

                strArgument1 = Convert.ToString(ownerKeyObject.Components.GetComponent(1).Recordset.Fields["STRUCTURE_ID"].Value);

                gTKeyObjects = FindWorkpointsOfFeature(ownerKeyObject.Components.GetComponent(1));

                if (gTKeyObjects.Count <= 0)
                {
                    noWorkPoint = true;
                    noMatchingWorkPoint = true;
                }
                else
                {
                    if (relatedWP != 0)
                    {
                        foreach (IGTKeyObject gtwpKey in gTKeyObjects)
                        {
                            if (gtwpKey.Components["WORKPOINT_N"] != null &&
                                 gtwpKey.Components["WORKPOINT_N"].Recordset != null &&
                                 gtwpKey.Components["WORKPOINT_N"].Recordset.RecordCount > 0)
                            {
                                gtwpKey.Components["WORKPOINT_N"].Recordset.MoveFirst();

                                if (!string.IsNullOrEmpty(
                                    Convert.ToString(gtwpKey.Components["WORKPOINT_N"].Recordset.Fields["WP_NBR"].Value)))
                                {
                                    strArgument2 = Convert.ToString(gtwpKey.Components["WORKPOINT_N"].Recordset.Fields["WP_NBR"].Value);

                                    if (relatedWP != Convert.ToUInt32(gtwpKey.Components["WORKPOINT_N"].Recordset.Fields["WP_NBR"].Value))
                                    {
                                        noMatchingWorkPoint = false;
                                    }
                                }
                                else
                                {
                                    noMatchingWorkPoint = false;
                                }
                            }
                        }
                    }
                }
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Checks whether active feature Owner feature has Work point or not and if present matching CU record of active feature is present on Work Point CU's
        /// </summary>
        /// <param name="ownerKeyObject"></param>
        /// <param name="relatedWP"></param>
        /// <param name="noWorkPoint"></param>
        /// <param name="noMatchingWorkPoint"></param>
        /// <param name="strArgument1"></param>
        /// <param name="strArgument2"></param>
        private void CheckValidationForOwner1Feature(IGTKeyObject ownerKeyObject, ref int relatedWP, ref bool noWorkPoint, ref bool noMatchingWorkPoint, ref string strArgument1, ref string strArgument2)
        {
            IGTKeyObjects gTKeyObjects = GTClassFactory.Create<IGTKeyObjects>();
            try
            {
                if (ownerKeyObject != null)
                {
                    if (ownerKeyObject.Components.GetComponent(1) != null)
                    {
                        if (ownerKeyObject.Components.GetComponent(1).Recordset != null && ownerKeyObject.Components.GetComponent(1).Recordset.RecordCount > 0)
                        {
                            ownerKeyObject.Components.GetComponent(1).Recordset.MoveFirst();

                            strArgument1 = Convert.ToString(ownerKeyObject.Components.GetComponent(1).Recordset.Fields["STRUCTURE_ID"].Value);

                            gTKeyObjects = FindWorkpointsOfFeature(ownerKeyObject.Components.GetComponent(1));

                            if (gTKeyObjects.Count <= 0)
                            {
                                noWorkPoint = true;
                                noMatchingWorkPoint = true;
                            }
                            else
                            {
                                foreach (IGTKeyObject gtwpKey in gTKeyObjects)
                                {
                                    noMatchingWorkPoint = IsActiveCUInstanceMatched(gtwpKey.Components["WORKPOINT_CU_N"], ref relatedWP);
                                    strArgument2 = Convert.ToString(gtwpKey.Components["WORKPOINT_N"].Recordset.Fields["WP_NBR"].Value);
                                }
                            }
                        }
                    }
                }
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Add valdiations required to validate the active feature.
        /// </summary>
        /// <param name="noWorkPoint"></param>
        /// <param name="noMatchingWorkPoint"></param>
        /// <param name="strArgument1"></param>
        /// <param name="strArgument2"></param>
        /// <param name="p_lstErrorMessage"></param>
        /// <param name="p_lstErrorPriority"></param>
        /// <param name="errorPriorityNoWorkpoint"></param>
        /// <param name="errorPriorityNoMatchingWorkPoint"></param>
        private void AddValidationsToArray(bool noWorkPoint, bool noMatchingWorkPoint, string strArgument1, string strArgument2, ref List<string> p_lstErrorMessage, ref List<string> p_lstErrorPriority, string errorPriorityNoWorkpoint, string errorPriorityNoMatchingWorkPoint)
        {
            ValidationRuleManager validateMsg = new ValidationRuleManager();
            object[] messArguments;
            try
            {
                if (noWorkPoint)
                {
                    messArguments = new object[1];
                    messArguments[0] = strArgument1;
                    validateMsg.Rule_Id = "JM1";
                    validateMsg.BuildRuleMessage((IGTApplication)GTClassFactory.Create<IGTApplication>(), messArguments);

                    p_lstErrorMessage.Add(validateMsg.Rule_MSG);
                    p_lstErrorPriority.Add(errorPriorityNoWorkpoint);
                }

                if (!noMatchingWorkPoint)
                {
                    messArguments = new object[1];
                    messArguments[0] = strArgument2;
                    validateMsg.Rule_Id = "JM2";
                    validateMsg.BuildRuleMessage((IGTApplication)GTClassFactory.Create<IGTApplication>(), messArguments);

                    p_lstErrorMessage.Add(validateMsg.Rule_MSG);
                    p_lstErrorPriority.Add(errorPriorityNoMatchingWorkPoint);
                }
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// CheckForOwnedByRelation: Check whether the feature is a Owner or a Child feature.
        /// </summary>
        /// <returns></returns>
        private bool CheckForOwnedByRelation()
        {
            Recordset recordset = null;
            short commonComponentCNO = 1;
            bool hasOwner = true;
            try
            {
                recordset = m_ActiveKeyObject.Components.GetComponent(commonComponentCNO).Recordset;
                if (recordset != null)
                {
                    if (!(recordset.EOF && recordset.BOF) && recordset.RecordCount > 0)
                    {
                        recordset.MoveFirst();
                        if (((recordset.Fields["OWNER1_ID"].Value == DBNull.Value && recordset.Fields["OWNER2_ID"].Value == DBNull.Value) || (Convert.ToInt32(recordset.Fields["OWNER1_ID"].Value) == 0 && Convert.ToInt32(recordset.Fields["OWNER2_ID"].Value) == 0)) && !String.IsNullOrEmpty(Convert.ToString(recordset.Fields["STRUCTURE_ID"].Value)))
                        {
                            hasOwner = false;
                        }
                    }
                }
            }
            catch
            {
                throw;
            }
            return hasOwner;
        }

        /// <summary>
        /// Any CUs on the active feature exist in the Work Point CU component’s records (based on FNO, FID, CNO, CID, and CU) return true.  
        /// </summary>
        /// <param name="gtWorkpointCUComp">Workpoint CU Attribute component</param>
        /// <param name="relWorpointNumber">Related WorpointNumber</param>
        /// <returns></returns>
        public bool IsActiveCUInstanceMatched(IGTComponent gtWorkpointCUComp, ref int relWorpointNumber)
        {
            int aFid = 0;
            short aFno = 0;

            int aCid = 0;
            short aCno = 0;

            int wCUAFid = 0;
            short wCUAFno = 0;

            short wCUACno = 0;
            int wCUACid = 0;
         

            try
            {
                string sInstalledWR = Convert.ToString(m_gTActiveComponent.Recordset.Fields["WR_ID"].Value);
                string sEditeddWR = Convert.ToString(m_gTActiveComponent.Recordset.Fields["WR_EDITED"].Value);

                if (!String.IsNullOrEmpty(Convert.ToString(m_gTActiveComponent.Recordset.Fields["CU_C"].Value)) && ((sInstalledWR.Equals(m_gTDataContext.ActiveJob) && string.IsNullOrEmpty(sEditeddWR)) || (sEditeddWR.Equals(m_gTDataContext.ActiveJob))))
                {
                    Recordset rsWorkpoint = null;

                    aFid = Convert.ToInt32(m_gTActiveComponent.Recordset.Fields["G3E_FID"].Value);
                    aFno = Convert.ToInt16(m_gTActiveComponent.Recordset.Fields["G3E_FNO"].Value);
                    aCno = Convert.ToInt16(m_gTActiveComponent.CNO);
                    aCid = Convert.ToInt32(m_gTActiveComponent.Recordset.Fields["G3E_CID"].Value);

                    if (gtWorkpointCUComp.Recordset != null && gtWorkpointCUComp.Recordset.RecordCount > 0)
                    {

                        rsWorkpoint = gtWorkpointCUComp.Recordset;

                        rsWorkpoint.MoveFirst();

                        while (!rsWorkpoint.EOF)
                        {
                            if (!string.IsNullOrEmpty(Convert.ToString(rsWorkpoint.Fields["ASSOC_FID"].Value)))
                            {
                                wCUAFid = Convert.ToInt32(rsWorkpoint.Fields["ASSOC_FID"].Value);
                            }
                            if (!string.IsNullOrEmpty(Convert.ToString(rsWorkpoint.Fields["ASSOC_FNO"].Value)))
                            {
                                wCUAFno = Convert.ToInt16(rsWorkpoint.Fields["ASSOC_FNO"].Value);
                            }

                            if (!string.IsNullOrEmpty(Convert.ToString(rsWorkpoint.Fields["UNIT_CNO"].Value)))
                            {
                                wCUACno = Convert.ToInt16(rsWorkpoint.Fields["UNIT_CNO"].Value);
                            }

                            if (!string.IsNullOrEmpty(Convert.ToString(rsWorkpoint.Fields["UNIT_CID"].Value)))
                            {
                                wCUACid = Convert.ToInt32(rsWorkpoint.Fields["UNIT_CID"].Value);
                            }

                            if (wCUAFid == aFid && wCUAFno == aFno && wCUACno == aCno && wCUACid == aCid)
                            {
                                if (!string.IsNullOrEmpty(Convert.ToString(rsWorkpoint.Fields["WP_RELATED"].Value)))
                                {
                                    relWorpointNumber = Convert.ToInt32(rsWorkpoint.Fields["WP_RELATED"].Value);
                                }
                                return true;
                            }
                            rsWorkpoint.MoveNext();
                        }
                    }
                }
                else
                {
                    return true;
                }

            }
            catch
            {
                throw;
            }

            return false;
        }

        #endregion
    }
}
