using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ADODB;
using Intergraph.GTechnology.API;

namespace GTechnology.Oncor.CustomAPI
{
    public class WorkPointOperations
    {

        private short m_gtActiveFno = 0;
        private IGTComponent m_gTActiveComponent = null;
        private CommonFunctions m_gtcommonFunctions = new CommonFunctions();
        private Dictionary<int, int> m_NewWorkpointFidList = null;
        private int m_WorkpointSequence = 0;
        private int m_WorkpointNumber = 0;
        private IGTComponents m_gTCUComponents = GTClassFactory.Create<IGTComponents>();
        private IGTKeyObject m_activeKeyObject = null;
        IGTDataContext m_dataContext = null;
        IGTComponents m_gtAllCUComponents = null;
        Dictionary<IGTKeyObject, IGTComponents>  m_gTKeyObjects = null;
        bool m_CUCompModified = false;

        public bool CUCompModified
        {
            get { return m_CUCompModified; }
            set { m_CUCompModified = value; }
        }

        /// <summary>
        /// Object initialization for the WorkPointOperations class
        /// </summary>
        /// <param name="AllCUComponents"> This is collection of Primary and Ancillary CUs component that are chabnged for which Workpoints oprtaiton is needed</param>
        /// <param name="ActiveKeyObject"> Active Object</param>
        /// <param name="DataContext"> Data Context</param>
        public WorkPointOperations(IGTComponents AllCUComponents, IGTKeyObject ActiveKeyObject , IGTDataContext DataContext,Dictionary<IGTKeyObject,IGTComponents> p_FeaturesCollection = null)
        {

            m_gtcommonFunctions = new CommonFunctions();
            m_NewWorkpointFidList = new Dictionary<int, int>();
            m_gtAllCUComponents = AllCUComponents;
            m_activeKeyObject = ActiveKeyObject;
            m_dataContext = DataContext;

            if (ActiveKeyObject != null)
            {
                m_gtActiveFno = ActiveKeyObject.FNO;
            }

            m_gTKeyObjects = p_FeaturesCollection;
           // m_calledFromCC = p_CalledFromCC;
        }

        private void ModifiedCUComponentCollection()
        {
            try
            {
                foreach (KeyValuePair<IGTKeyObject,IGTComponents> gtKeyObject in m_gTKeyObjects)
                {
                    IGTComponents gTCUComponents = GTClassFactory.Create<IGTComponents>();
                    m_gtAllCUComponents = GTClassFactory.Create<IGTComponents>();

                    m_activeKeyObject = gtKeyObject.Key;
                    m_gtActiveFno = m_activeKeyObject.FNO;

                    m_gtAllCUComponents = gtKeyObject.Value;

                    if (m_gtAllCUComponents.Count > 0 &&
                    m_gtcommonFunctions.ISPrimaryGraphicComponentExist(m_activeKeyObject.FNO, m_activeKeyObject.FID))
                    {
                        m_CUCompModified = true;
                        ProcessModifiedCUComponents();
                    }
                }
            }
            catch
            {
                throw;
            }
        }

        public void SetComponentsChanged(IGTComponents gTChangedComponents)
        {
            Recordset rs = null;

            try
            {
                foreach (IGTComponent gtComp in gTChangedComponents)
                {
                    rs = gtComp.Recordset;
                    if (rs != null && rs.RecordCount > 0)
                    {
                        rs.MoveFirst();
                        while (!rs.EOF)
                        {
                            foreach (Field field in rs.Fields)
                            {
                                if (field.OriginalValue.ToString() != field.Value.ToString())
                                {
                                    if (!m_gtAllCUComponents.Contains(gtComp))//Added by Prathyusha
                                    {
                                        m_gtAllCUComponents.Add(gtComp);
                                    }
                                    break;
                                }
                            }
                            rs.MoveNext();
                        }
                    }
                }
            }
            catch
            {
                throw;
            }
        }

        public void DeleteObsoleteCURecords(IGTKeyObject p_keyObject)
        {
            IGTComponent oComponent = p_keyObject.Components.GetComponent(19104);
            ADODB.Recordset rs = null;
            string sStrcutureIDWP = string.Empty;
            ADODB.Recordset rsCUComponent = null;

            try
            {
                rsCUComponent = p_keyObject.Components.GetComponent(19101).Recordset;
                sStrcutureIDWP = Convert.ToString(rsCUComponent.Fields["STRUCTURE_ID"].Value);

                if (oComponent != null)
                {
                    if (oComponent.Recordset != null)
                    {
                        if (oComponent.Recordset.RecordCount > 0)
                        {
                            oComponent.Recordset.MoveFirst();
                            while (oComponent.Recordset.EOF == false)
                            {
                                int iFID = Convert.ToInt32(oComponent.Recordset.Fields["ASSOC_FID"].Value);
                                int iCID = Convert.ToInt32(oComponent.Recordset.Fields["UNIT_CID"].Value);
                                string sCU = Convert.ToString(oComponent.Recordset.Fields["CU_C"].Value);
                                string sActivity = Convert.ToString(oComponent.Recordset.Fields["activity_c"].Value);

                                rs = m_dataContext.OpenRecordset("select count(*) from COMMON_N where g3e_fid =?", ADODB.CursorTypeEnum.adOpenDynamic, ADODB.LockTypeEnum.adLockOptimistic, (int)ADODB.CommandTypeEnum.adCmdText, iFID);

                                if (Convert.ToInt32(rs.Fields[0].Value).Equals(1)) //Feature exists but Strucure ID is different now
                                {
                                    rs = m_dataContext.OpenRecordset("select count(*) from COMMON_N where g3e_fid =? and STRUCTURE_ID<> ?", ADODB.CursorTypeEnum.adOpenDynamic, ADODB.LockTypeEnum.adLockOptimistic, (int)ADODB.CommandTypeEnum.adCmdText, iFID, sStrcutureIDWP);

                                    if (Convert.ToInt32(rs.Fields[0].Value).Equals(1))
                                    {
                                        oComponent.Recordset.Delete();
                                    }
                                    else  //Check if the relevent record that we are seeing exists on the feature or not. This will take care of discard for a feature on CID level
                                    {
                                        rs = m_dataContext.OpenRecordset("select count(*) from COMP_UNIT_N where g3e_fid =? and g3e_cid = ? and cu_c = ? and activity_c = ?", ADODB.CursorTypeEnum.adOpenDynamic, ADODB.LockTypeEnum.adLockOptimistic, (int)ADODB.CommandTypeEnum.adCmdText, iFID, iCID, sCU, sActivity);

                                        if (Convert.ToInt32(rs.Fields[0].Value).Equals(0))
                                        {
                                            oComponent.Recordset.Delete();
                                        }
                                    }
                                }
                                else //Feature does not exist so delete the record corresponding to this associated FID
                                {
                                    oComponent.Recordset.Delete();
                                }

                                if (oComponent.Recordset.EOF == false)
                                {
                                    oComponent.Recordset.MoveNext();
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
        }
        public void SynchronizeWorkPointForObsoleteCUs(Dictionary<int,Int16> p_m_WorkPointsKeyObjects) 
        {
            IGTKeyObject oKeyObject = GTClassFactory.Create<IGTKeyObject>();

            try
            {
                foreach (KeyValuePair<int, Int16> item in p_m_WorkPointsKeyObjects)
                {
                    oKeyObject = m_dataContext.OpenFeature(item.Value, item.Key);
                    DeleteObsoleteCURecords(oKeyObject);
                }
            }
            catch (Exception ex)
            {
                string exMsg = string.Format("Error occurred in {0} of {1}.{2}{3}", System.Reflection.MethodBase.GetCurrentMethod().Name, this.ToString(), Environment.NewLine, ex.Message);
                throw new Exception(exMsg);
            }
        }

        /// <summary>
        /// All workpoint operations like creating new workpoint feature if it is not there / adding or updating workpoint cu attributes.. e.t.c
        /// </summary>
        public void DoWorkpointOperations()
        {
            try
            {
                if (m_gTKeyObjects == null)
                {
                    ProcessModifiedCUComponents();
                }
                else
                {
                    ModifiedCUComponentCollection();
                }
            }
            catch
            {
                throw;
            }
        }

        private void ProcessModifiedCUComponents()
        {
            try
            {
                foreach (IGTComponent item in m_gtAllCUComponents)
                {
                    m_gTActiveComponent = item;
                    if (CheckForOwnedByRelation())
                    {
                        IGTKeyObjects gTOwnerKeyObjects = LocateOwnersOfActiveFeature(3,m_activeKeyObject);
                        if (gTOwnerKeyObjects.Count > 0)
                        {
                            //if (m_gtcommonFunctions.IsActiveFeatureIsLinear(m_gtActiveFno))
                            if (IsActiveFeatureIsLinear(m_gtActiveFno))
                            {
                                int ownerCount = 0;

                                if (gTOwnerKeyObjects.Count == 2)
                                {
                                    IGTKeyObject owner1KeyObject = GTClassFactory.Create<IGTKeyObject>();
                                    IGTKeyObject owner2KeyObject = GTClassFactory.Create<IGTKeyObject>();
                                    GetOwner1Owner2(gTOwnerKeyObjects, m_activeKeyObject, ref owner1KeyObject, ref owner2KeyObject);
                                    CreateOrUpdateWorkpoint(owner1KeyObject.Components.GetComponent(1), true, ownerCount);
                                    CreateOrUpdateWorkpoint(owner2KeyObject.Components.GetComponent(1), true, -1);
                                }
                                else
                                {
                                    CreateOrUpdateWorkpoint(gTOwnerKeyObjects[0].Components.GetComponent(1), true, ownerCount);
                                }

                                if (gTOwnerKeyObjects.Count == 2)
                                    UpdateRelatedWPAttribute(gTOwnerKeyObjects);
                            }
                            else
                            {
                                CreateOrUpdateWorkpoint(gTOwnerKeyObjects[0].Components.GetComponent(1), false, gTOwnerKeyObjects.Count);
                            }
                        }
                    }
                    else
                    {
                        // Accomodate Structures that have no owners

                        // This list of Structure FNOs should be parameterized
                        //106 Manhole
                        //107 Miscellaneous Structure
                        //108 Pad
                        //109 Primary Pull Box
                        //110 Pole
                        //113 Secondary Box
                        //114 Street Light Standard
                        //115 Substation
                        //116 Transmission Tower
                        //117 Vault
                        //120 Secondary Enclosure
                        //2500  Junction Point

                        if (m_activeKeyObject.FNO == 2400)
                        {
                            ProcessingFormation();
                        }
                        else
                        {

                            List<short> structFNO = new List<short> { 106, 107, 108, 109, 110, 113, 114, 115, 116, 117, 120, 2500 };

                            Recordset rs = m_activeKeyObject.Components.GetComponent(1).Recordset;

                            //string structureID = rs.Fields["STRUCTURE_ID"].Value != DBNull.Value ? Convert.ToString(rs.Fields["STRUCTURE_ID"].Value) : string.Empty;
                            string structureID = (!Convert.IsDBNull(rs.Fields["STRUCTURE_ID"].Value)) ? Convert.ToString(rs.Fields["STRUCTURE_ID"].Value) : string.Empty;

                            if (structFNO.Contains(m_activeKeyObject.FNO) && !string.IsNullOrEmpty(structureID))
                            {
                                CreateOrUpdateWorkpoint(m_activeKeyObject.Components.GetComponent(1), false, 0);
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

        private void ProcessingFormation()
        {
            IGTComponent gTContainsComponent = m_activeKeyObject.Components["CONTAIN_N"];


            if (gTContainsComponent != null && gTContainsComponent.Recordset != null &&
                gTContainsComponent.Recordset.RecordCount > 0)
            {
                Recordset rsContains = gTContainsComponent.Recordset;
                Recordset rsDuctivity = null;
               
                rsContains.MoveFirst();
                while (!rsContains.EOF)
                {
                    IGTKeyObject formationOwner = m_dataContext.OpenFeature(Convert.ToInt16(rsContains.Fields["G3E_OWNERFNO"].Value), Convert.ToInt32(rsContains.Fields["G3E_OWNERFID"].Value));
                    if (formationOwner.Components.Count > 0 &&
                          formationOwner.Components["DUCTIVITY_N"] != null &&
                          formationOwner.Components["DUCTIVITY_N"].Recordset != null &&
                          formationOwner.Components["DUCTIVITY_N"].Recordset.RecordCount > 0)
                    {
                        rsDuctivity = formationOwner.Components["DUCTIVITY_N"].Recordset;
                        
                        IGTKeyObjects ownerKeyObjects = LocateOwnersOfActiveFeature(122, formationOwner);

                        foreach(IGTKeyObject gtownerKeyObject in ownerKeyObjects)
                        {
                            if(gtownerKeyObject.FID != formationOwner.FID)
                            {
                                List<short> structFNO = new List<short> { 106, 107, 108, 109, 110, 113, 114, 115, 116, 117, 120, 2500 };
                                if (structFNO.Contains(Convert.ToInt16(gtownerKeyObject.FNO)))
                                {
                                    
                                    IGTKeyObjects ownerWorkpoints = FindWorkpointsOfFeature(gtownerKeyObject.Components.GetComponent(1));

                                    if (ownerWorkpoints != null)
                                    {
                                        if (ownerWorkpoints.Count <= 0)
                                        {
                                            CreateNewWorkpointFeature(
                                                Convert.ToInt16(m_activeKeyObject.Components.GetComponent(1).Recordset.Fields["G3E_FNO"].Value),
                                                Convert.ToInt32(m_activeKeyObject.Components.GetComponent(1).Recordset.Fields["G3E_FID"].Value),
                                                true,
                                               Convert.ToString(gtownerKeyObject.Components.GetComponent(1).Recordset.Fields["STRUCTURE_ID"].Value),
                                               0, gtownerKeyObject.FNO, gtownerKeyObject.FID);
                                        }
                                        else
                                        {
                                            UpdateWorkpointComponents(Convert.ToInt16(gtownerKeyObject.Components.GetComponent(1).Recordset.Fields["G3E_FNO"].Value),
                                                Convert.ToInt32(gtownerKeyObject.Components.GetComponent(1).Recordset.Fields["G3E_FID"].Value),
                                                ownerWorkpoints[0], false,
                                                Convert.ToString(gtownerKeyObject.Components.GetComponent(1).Recordset.Fields["STRUCTURE_ID"].Value), 0,
                                                true);
                                        }
                                    }

                                }
                            }
                        }                       
                         

                    }
                    rsContains.MoveNext();
                }
            }
        }

        private bool CheckForOwnedByRelation()
        {
            short commonComponentCNO = 1;
            bool hasOwner = true;
            try
            {
                Recordset recordset = m_activeKeyObject.Components.GetComponent(commonComponentCNO).Recordset;

                if (null != recordset && 0 < recordset.RecordCount)
                {
                    recordset.MoveFirst();

                    //int owner1 = recordset.Fields["OWNER1_ID"].Value != DBNull.Value ? Convert.ToInt32(recordset.Fields["OWNER1_ID"].Value) : 0;
                    //int owner2 = recordset.Fields["OWNER2_ID"].Value != DBNull.Value ? Convert.ToInt32(recordset.Fields["OWNER2_ID"].Value) : 0;

                    int owner1 = (!Convert.IsDBNull(recordset.Fields["OWNER1_ID"].Value)) ? Convert.ToInt32(recordset.Fields["OWNER1_ID"].Value) : 0;
                    int owner2 = (!Convert.IsDBNull(recordset.Fields["OWNER2_ID"].Value)) ? Convert.ToInt32(recordset.Fields["OWNER2_ID"].Value) : 0;

                    if (0 == owner1 + owner2)
                    {
                        hasOwner = false;
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
        /// To find owners of a linear feature.
        /// </summary>
        /// <returns></returns>
        private IGTKeyObjects LocateOwnersOfActiveFeature(short rno , IGTKeyObject gTKeyObject)
        {
            
            IGTKeyObjects gTOwnerKeyObjects = GTClassFactory.Create<IGTKeyObjects>();
            try
            {
                using (IGTRelationshipService relationShipService = GTClassFactory.Create<IGTRelationshipService>())
                {
                    relationShipService.ActiveFeature = gTKeyObject;
                    relationShipService.DataContext = m_dataContext;
                    gTOwnerKeyObjects = relationShipService.GetRelatedFeatures(rno);
                }
            }
            catch
            {
                
            }

            return gTOwnerKeyObjects;
        }


        /// <summary>
        /// Returns true if activefeature is linear feature.
        /// </summary>
        /// <returns></returns>
        private bool IsActiveFeatureIsLinear(short Fno)
        {
            string sql = "";
            Recordset rsLinear = null;
            try
            {
                sql = "SELECT * FROM G3E_COMPONENTINFO_OPTABLE WHERE G3E_CNO IN(SELECT G3E_PRIMARYGEOGRAPHICCNO FROM G3E_FEATURES_OPTABLE WHERE G3E_FNO=?) AND UPPER(G3E_GEOMETRYTYPE) LIKE '%LINE%'";
                rsLinear = m_dataContext.OpenRecordset(sql, ADODB.CursorTypeEnum.adOpenDynamic, ADODB.LockTypeEnum.adLockReadOnly,
            (int)ADODB.CommandTypeEnum.adCmdText, Fno);

                if (rsLinear.RecordCount > 0)
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

        /// <summary>
        /// Identifies the Key Object that's the Owner1 and Owner2 of the Child Feature
        /// </summary>
        /// <param name="parentFeatures">IGTKeyObjects</param>
        /// <param name="childFeature">IGTKeyObject</param>
        /// <param name="owner1KeyObject">IGTKeyObject</param>
        /// <param name="owner2KeyObject">IGTKeyObject</param>
        /// <returns></returns>
        private void GetOwner1Owner2(IGTKeyObjects parentFeatures, IGTKeyObject childFeature, ref IGTKeyObject owner1KeyObject, ref IGTKeyObject owner2KeyObject)
        {
            //if (DBNull.Value != childFeature.Components["COMMON_N"].Recordset.Fields["OWNER1_ID"].Value)
            if (!Convert.IsDBNull(childFeature.Components["COMMON_N"].Recordset.Fields["OWNER1_ID"].Value))
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
            //if (DBNull.Value != childFeature.Components["COMMON_N"].Recordset.Fields["OWNER2_ID"].Value)
            if (!Convert.IsDBNull(childFeature.Components["COMMON_N"].Recordset.Fields["OWNER2_ID"].Value))
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
        /// Finds and Validates the workpoints associated to the input component parameter.
        /// </summary>
        /// <param name="gTCommonComponent">Common component</param>
        private void CreateOrUpdateWorkpoint(IGTComponent gTCommonComponent, bool isLinear, int ownerCount)
        {
            try
            {
                IGTKeyObjects gTKeyObjects = FindWorkpointsOfFeature(gTCommonComponent);

                if (gTKeyObjects != null)
                {
                    if (gTKeyObjects.Count <= 0)
                    {

                        CreateNewWorkpointFeature(Convert.ToInt16(gTCommonComponent.Recordset.Fields["G3E_FNO"].Value), Convert.ToInt32(gTCommonComponent.Recordset.Fields["G3E_FID"].Value), isLinear,
                           Convert.ToString(gTCommonComponent.Recordset.Fields["STRUCTURE_ID"].Value), ownerCount);

                    }
                    else
                    {
                        UpdateWorkpointComponents(Convert.ToInt16(gTCommonComponent.Recordset.Fields["G3E_FNO"].Value), Convert.ToInt32(gTCommonComponent.Recordset.Fields["G3E_FID"].Value), gTKeyObjects[0], false,
                            Convert.ToString(gTCommonComponent.Recordset.Fields["STRUCTURE_ID"].Value), ownerCount, isLinear); 
                    }
                }
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Locate the associated Work Point feature (if it exists) for the active WR job.
        /// </summary>
        /// <param name="gTCommonComponent">Common component</param>
        /// <returns></returns>
        private IGTKeyObjects FindWorkpointsOfFeature(IGTComponent gTCommonComponent)
        {
            string strStructureId = null;
            string sql = "";
            Recordset rsWorkpoints = null;
            int count = 0;
            IGTKeyObjects workPointKeyObjects = GTClassFactory.Create<IGTKeyObjects>();
            try
            {
                if (gTCommonComponent != null && gTCommonComponent.Recordset.RecordCount > 0)
                {
                    gTCommonComponent.Recordset.MoveFirst();
                    strStructureId = Convert.ToString(gTCommonComponent.Recordset.Fields["STRUCTURE_ID"].Value);
                }

                if (!string.IsNullOrEmpty(strStructureId))
                {
                    sql = string.Format("select G3E_FID,G3E_FNO from WORKPOINT_N where STRUCTURE_ID = '{0}' and WR_NBR = '{1}'", strStructureId, m_dataContext.ActiveJob);
                    rsWorkpoints = m_dataContext.Execute(sql, out count, (int)ADODB.CommandTypeEnum.adCmdText, null);

                    if (rsWorkpoints != null && rsWorkpoints.RecordCount > 0)
                    {
                        workPointKeyObjects = GTClassFactory.Create<IGTKeyObjects>();
                        rsWorkpoints.MoveFirst();
                        while (!rsWorkpoints.EOF)
                        {
                            IGTKeyObject workPoint = m_dataContext.OpenFeature(Convert.ToInt16(rsWorkpoints.Fields["G3E_FNO"].Value), Convert.ToInt32(rsWorkpoints.Fields["G3E_FID"].Value));
                            workPointKeyObjects.Add(workPoint);
                            rsWorkpoints.MoveNext();
                        }
                    }
                    else if (m_NewWorkpointFidList.Count > 0)
                    {
                        foreach (KeyValuePair<int, int> wpListFid in m_NewWorkpointFidList)
                        {
                            if (wpListFid.Key == Convert.ToInt32(gTCommonComponent.Recordset.Fields["G3E_FID"].Value))
                            {
                                IGTKeyObject workPoint = m_dataContext.OpenFeature(191, wpListFid.Value);
                                workPointKeyObjects.Add(workPoint);
                            }
                        }
                    }
                }
                else if (m_NewWorkpointFidList.Count > 0)
                {
                    foreach (KeyValuePair<int, int> wpListFid in m_NewWorkpointFidList)
                    {
                        if (wpListFid.Key == Convert.ToInt32(gTCommonComponent.Recordset.Fields["G3E_FID"].Value))
                        {
                            IGTKeyObject workPoint = m_dataContext.OpenFeature(191, wpListFid.Value);
                            workPointKeyObjects.Add(workPoint);
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
        /// Update the Related Work Point attribute of the Second Work Point.
        /// </summary>
        /// <param name="gTOwnerKeyObjects">Owner Key Object of Active Key Object</param>
        private void UpdateRelatedWPAttribute(IGTKeyObjects gTOwnerKeyObjects)
        {
            IGTComponent gtActiveFeatureCommonCom = null;
            int Owner1Id = 0;
            int Owner2Id = 0;

            int Owner1Fid = 0;
            int Owner2Fid = 0;

            IGTKeyObject gTOwner1KeyObject = null;
            IGTKeyObject gTOwner2KeyObject = null;

            IGTKeyObjects gTWorkPointOwner1KeyObjects = null;
            IGTKeyObjects gTWorkPointOwner2KeyObjects = null;

            IGTComponent gTOwner1Component = null;
            IGTComponent gTOwner2Component = null;

            Recordset tmpOwnerrs = null;
            int WorkPointNumber = 0;

            string sql = "";
            int count = 0;

            try
            {
                gtActiveFeatureCommonCom = m_activeKeyObject.Components.GetComponent(1);
                Owner1Id = Convert.ToInt32(gtActiveFeatureCommonCom.Recordset.Fields["OWNER1_ID"].Value);
                Owner2Id = Convert.ToInt32(gtActiveFeatureCommonCom.Recordset.Fields["OWNER2_ID"].Value);


                sql = "select G3E_FID from COMMON_N where g3e_id = ?";
                tmpOwnerrs = m_dataContext.Execute(sql, out count, (int)ADODB.CommandTypeEnum.adCmdText, Owner1Id);

                if (tmpOwnerrs != null && tmpOwnerrs.RecordCount > 0)
                {
                    tmpOwnerrs.MoveFirst();
                    if (!(tmpOwnerrs.EOF && tmpOwnerrs.BOF))
                        Owner1Fid = Convert.ToInt32(tmpOwnerrs.Fields["G3E_FID"].Value);
                }


                sql = "select G3E_FID from COMMON_N where g3e_id = ?";
                tmpOwnerrs = m_dataContext.Execute(sql, out count, (int)ADODB.CommandTypeEnum.adCmdText, Owner2Id);

                if (tmpOwnerrs != null && tmpOwnerrs.RecordCount > 0)
                {
                    tmpOwnerrs.MoveFirst();
                    if (!(tmpOwnerrs.EOF && tmpOwnerrs.BOF))
                        Owner2Fid = Convert.ToInt32(tmpOwnerrs.Fields["G3E_FID"].Value);
                }



                foreach (IGTKeyObject keyObj in gTOwnerKeyObjects)
                {
                    if (keyObj.FID == Owner1Fid)
                    {
                        gTOwner1KeyObject = keyObj;
                    }
                    else if (keyObj.FID == Owner2Fid)
                    {
                        gTOwner2KeyObject = keyObj;
                    }
                }

                gTWorkPointOwner1KeyObjects = FindWorkpointsOfFeature(gTOwner1KeyObject.Components.GetComponent(1));
                gTWorkPointOwner2KeyObjects = FindWorkpointsOfFeature(gTOwner2KeyObject.Components.GetComponent(1));

                gTOwner1Component = gTWorkPointOwner1KeyObjects[0].Components["WORKPOINT_CU_N"];
                bool wpRUpdate = false;
                if (gTOwner1Component.Recordset != null && gTOwner1Component.Recordset.RecordCount > 0)
                {
                    gTOwner1Component.Recordset.MoveFirst();
                    gTOwner2Component = gTWorkPointOwner2KeyObjects[0].Components["WORKPOINT_N"];

                    if (gTOwner1Component.Recordset != null && gTOwner1Component.Recordset.RecordCount > 0)
                    {
                        gTOwner2Component.Recordset.MoveFirst();

                        WorkPointNumber = Convert.ToInt32(gTOwner2Component.Recordset.Fields["wp_nbr"].Value);

                        while (!gTOwner1Component.Recordset.EOF)
                        {
                            if (!String.IsNullOrEmpty(Convert.ToString(gTOwner1Component.Recordset.Fields["ASSOC_FID"].Value)) &&
                                Convert.ToInt32(gTOwner1Component.Recordset.Fields["ASSOC_FID"].Value) == m_activeKeyObject.FID)
                            {
                                gTOwner1Component.Recordset.Fields["WP_RELATED"].Value = WorkPointNumber;
                                wpRUpdate = true;
                            }

                            gTOwner1Component.Recordset.MoveNext();
                        }
                    }
                }

                if (!wpRUpdate)
                {
                    gTOwner2Component = gTWorkPointOwner2KeyObjects[0].Components["WORKPOINT_CU_N"];

                    if (gTOwner2Component.Recordset != null && gTOwner2Component.Recordset.RecordCount > 0)
                    {
                        gTOwner2Component.Recordset.MoveFirst();
                        gTOwner1Component = gTWorkPointOwner1KeyObjects[0].Components["WORKPOINT_N"];

                        if (gTOwner2Component.Recordset != null && gTOwner2Component.Recordset.RecordCount > 0)
                        {
                            gTOwner1Component.Recordset.MoveFirst();
                            WorkPointNumber = Convert.ToInt32(gTOwner1Component.Recordset.Fields["wp_nbr"].Value);

                            while (!gTOwner2Component.Recordset.EOF)
                            {
                                if (!String.IsNullOrEmpty(Convert.ToString(gTOwner2Component.Recordset.Fields["ASSOC_FID"].Value)) &&
                                Convert.ToInt32(gTOwner2Component.Recordset.Fields["ASSOC_FID"].Value) == m_activeKeyObject.FID)
                                {
                                    gTOwner2Component.Recordset.Fields["WP_RELATED"].Value = WorkPointNumber;
                                }

                                gTOwner2Component.Recordset.MoveNext();
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
        /// Creates new workpoint feature.
        /// </summary>
        private void CreateNewWorkpointFeature(short fno, int fid, bool isLinear, string strStructureID, int ownerCount,
            short formationOwnerFno=0 ,int formationOwnerFid=0)
        {

            try
            {
                IGTGeometry gTGeometry = null;

                if (fno == 2400)
                {
                    gTGeometry = m_dataContext.OpenFeature(formationOwnerFno, formationOwnerFid).Components.GetComponent(m_gtcommonFunctions.GetPrimaryGraphicCno(formationOwnerFno, true)).Geometry;
                }
                else
                {
                    gTGeometry = m_dataContext.OpenFeature(fno, fid).Components.GetComponent(m_gtcommonFunctions.GetPrimaryGraphicCno(fno, true)).Geometry;
                }                

                customWorkPointCreator workPointCreator = new customWorkPointCreator(m_dataContext, gTGeometry.FirstPoint.X, gTGeometry.FirstPoint.Y);
                IGTKeyObject workPointFeature = workPointCreator.CreateWorkPointFeature();

                if (fno == 2400)
                {
                    m_NewWorkpointFidList.Add(formationOwnerFid, workPointFeature.FID);
                    UpdateWorkpointComponents(formationOwnerFno, formationOwnerFid, workPointFeature, true, strStructureID, ownerCount, isLinear);
                }
                else
                {
                    m_NewWorkpointFidList.Add(fid, workPointFeature.FID);
                    UpdateWorkpointComponents(fno, fid, workPointFeature, true, strStructureID, ownerCount, isLinear);
                }
                    

               
                //workPointCreator.SynchronizeWPCuLabel(workPointFeature);
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// UpdateWorkpointComponents : Update Workpoint feature  components.
        /// </summary>
        /// <param name="fno">Workpoint Fno</param>
        /// <param name="fid">Workpoint FID</param>
        /// <param name="workPointFeature">Workpoint Feature</param>
        /// <param name="geoUpdate">Bool value indicates the updation of geometry.</param>
        /// <param name="strStructureID">StructureID of Active Key Object</param>
        private void UpdateWorkpointComponents(short fno, int fid, IGTKeyObject workPointFeature, bool geoUpdate,
            string strStructureID, int ownerCount, bool isLinear)
        {
            IGTComponents workPointComponents = workPointFeature.Components;
            foreach (IGTComponent component in workPointComponents)
            {
                if (component.Name == "WORKPOINT_N" && geoUpdate)
                {
                    UpdateWorkPointAttributes(component, strStructureID);
                }
                else if (component.Name == "WORKPOINT_CU_N" && ownerCount == 0 && isLinear == true)
                {
                    UpdateWorkpointCUAttributes(workPointFeature.FID, component);
                }
                else if (component.Name == "WORKPOINT_CU_N" && isLinear == false)
                {
                    UpdateWorkpointCUAttributes(workPointFeature.FID, component);
                }
            }

            IGTKeyObject oKeyObject = m_dataContext.OpenFeature(fno, fid);
            IGTGeometry gTGeometry = oKeyObject.Components.GetComponent(m_gtcommonFunctions.GetPrimaryGraphicCno(fno, true)).Geometry;
            customWorkPointCreator workPointCreator = new customWorkPointCreator(m_dataContext, gTGeometry.FirstPoint.X, gTGeometry.FirstPoint.Y);
            workPointCreator.SynchronizeWPCuLabel(workPointFeature);
        }


        /// <summary>
        /// Update the Work Point CU Attributes component fiedls.
        /// </summary>
        /// <param name="wPfid">New workpoint fid</param>
        /// <param name="component">Work Point CU Attributes component</param>
        private void UpdateWorkpointCUAttributes(int wPfid, IGTComponent component)
        {       
            try
            {
                Recordset tempRs = null;
                int recordCount = 0;
                tempRs = component.Recordset;

                if (tempRs != null)
                {
                    if (m_gTActiveComponent.Recordset != null && m_gTActiveComponent.Recordset.RecordCount > 0)
                    {
                        m_gTActiveComponent.Recordset.MoveFirst();

                        //if(tempRs.RecordCount > 0 && (m_gTActiveComponent.CNO == 21 || m_gTActiveComponent.CNO == 22))
                        if (tempRs.RecordCount > 0 && (m_gTActiveComponent.CNO == 21 || m_gTActiveComponent.CNO == 22))
                        {
                            tempRs.MoveFirst();
                            while (!tempRs.EOF)
                            {
                                if (tempRs.Fields["ASSOC_FID"].Value != null && !string.IsNullOrEmpty(tempRs.Fields["ASSOC_FID"].Value.ToString()))
                                {
                                    int tmpAssocFID = Convert.ToInt32(tempRs.Fields["ASSOC_FID"].Value);
                                    int tmpActiveFID = Convert.ToInt32(m_gTActiveComponent.Recordset.Fields["G3E_FID"].Value);

                                    short tmpAssocCNO = Convert.ToInt16(tempRs.Fields["UNIT_CNO"].Value);
                                    short tmpActiveCNO = Convert.ToInt16(m_gTActiveComponent.Recordset.Fields["G3E_CNO"].Value);

                                    //string sWPInstalledWR = string.Empty;
                                    //sWPInstalledWR = Convert.ToString(m_gTActiveComponent.Recordset.Fields["WR_ID"].Value);

                                    if (tmpAssocFID == tmpActiveFID && tmpAssocCNO == tmpActiveCNO)
                                    {
                                        tempRs.Delete();
                                    }

                                    //if(Convert.ToInt32(tempRs.Fields["ASSOC_FID"].Value) == Convert.ToInt32(m_gTActiveComponent.Recordset.Fields["G3E_FID"].Value))
                                    //{
                                    //  tempRs.Delete();
                                    //}
                                }
                                tempRs.MoveNext();
                            }
                        }

                        int wrNBR = GetWRNumber();
                        

                        while (!m_gTActiveComponent.Recordset.EOF)
                        {
                            string sInstalledWR = Convert.ToString(m_gTActiveComponent.Recordset.Fields["WR_ID"].Value);
                            string sEditeddWR = Convert.ToString(m_gTActiveComponent.Recordset.Fields["WR_EDITED"].Value);

                            if (!String.IsNullOrEmpty(Convert.ToString(m_gTActiveComponent.Recordset.Fields["CU_C"].Value)) && !String.IsNullOrEmpty(Convert.ToString(m_gTActiveComponent.Recordset.Fields["ACTIVITY_C"].Value)))
                            {
                                if ((sInstalledWR.Equals(m_dataContext.ActiveJob) && (String.IsNullOrEmpty(sEditeddWR))) || (sEditeddWR.Equals(m_dataContext.ActiveJob)))
                                {
                                    recordCount = tempRs.RecordCount + 1;
                                    tempRs.AddNew();
                                    tempRs.Fields["G3E_FID"].Value = wPfid;
                                    tempRs.Fields["G3E_FNO"].Value = 191;
                                    tempRs.Fields["G3E_CNO"].Value = component.CNO;
                                    //       tempRs.Fields["G3E_CID"].Value = recordCount;

                                    tempRs.Fields["ACTIVITY_C"].Value = m_gTActiveComponent.Recordset.Fields["ACTIVITY_C"].Value;
                                    tempRs.Fields["CIAC_C"].Value = m_gTActiveComponent.Recordset.Fields["CIAC_C"].Value;
                                    tempRs.Fields["CU_C"].Value = m_gTActiveComponent.Recordset.Fields["CU_C"].Value;
                                    tempRs.Fields["CU_DESC"].Value = m_gTActiveComponent.Recordset.Fields["CU_DESC"].Value;
                                    tempRs.Fields["ASSOC_FID"].Value = m_gTActiveComponent.Recordset.Fields["G3E_FID"].Value;
                                    tempRs.Fields["ASSOC_FNO"].Value = m_gTActiveComponent.Recordset.Fields["G3E_FNO"].Value;
                                    tempRs.Fields["LENGTH_FLAG"].Value = m_gTActiveComponent.Recordset.Fields["LENGTH_FLAG"].Value;
                                    tempRs.Fields["MACRO_CU_C"].Value = m_gTActiveComponent.Recordset.Fields["MACRO_CU_C"].Value;
                                    tempRs.Fields["PRIME_ACCT_ID"].Value = m_gTActiveComponent.Recordset.Fields["PRIME_ACCT_ID"].Value;
                                    tempRs.Fields["QTY_LENGTH_Q"].Value = m_gTActiveComponent.Recordset.Fields["QTY_LENGTH_Q"].Value;
                                    tempRs.Fields["UNIT_CID"].Value = m_gTActiveComponent.Recordset.Fields["G3E_CID"].Value;
                                    tempRs.Fields["UNIT_CNO"].Value = m_gTActiveComponent.CNO;
                                    tempRs.Fields["VINTAGE_YR"].Value = m_gTActiveComponent.Recordset.Fields["VINTAGE_YR"].Value;

                                    if (m_WorkpointSequence == 0)
                                    {
                                        //m_WorkpointSequence = m_gtcommonFunctions.GetWorkManagementSequence();
                                        m_WorkpointSequence = m_gtcommonFunctions.NextWorkMgmtSeq;
                                    }
                                    else
                                    {
                                        m_WorkpointSequence = m_WorkpointSequence + 1;
                                    }

                                    if (wrNBR == 0)
                                    {
                                        tempRs.Fields["WM_SEQ"].Value = string.Format("{0}{1}", m_dataContext.ActiveJob, m_WorkpointSequence.ToString().PadLeft(4, '0'));
                                    }
                                    else
                                    {
                                        tempRs.Fields["WM_SEQ"].Value = string.Format("{0}{1}", wrNBR, m_WorkpointSequence.ToString().PadLeft(4, '0'));
                                    }

                                    tempRs.Fields["WORK_INSTRUCTION_T"].Value = m_gtcommonFunctions.GetWorkInstruction(Convert.ToString(m_gTActiveComponent.Recordset.Fields["CU_C"].Value));
                                    tempRs.Update();
                                }
                            }
                            m_gTActiveComponent.Recordset.MoveNext();
                        }
                    }
                }
            }
            catch
            {
                throw;
            }
        }

        private int GetWRNumber()
        {
            int wrNbr = 0;
            
            try
            {
                Recordset jobInfoRs = ExecuteCommand(string.Format("select WR_NBR from g3e_job where g3e_identifier  = '{0}'", m_dataContext.ActiveJob));

                if (jobInfoRs.RecordCount > 0)
                {
                    jobInfoRs.MoveFirst();
                    
                    if (jobInfoRs.Fields["WR_NBR"].Value.GetType() != typeof(DBNull))
                    {
                        wrNbr = Convert.ToInt32(jobInfoRs.Fields["WR_NBR"].Value);
                    }
                }
            }
            catch
            {
                throw;
            }
            return wrNbr;
        }

        /// <summary>
		/// Method to execute sql query and return the result record set
		/// </summary>
		/// <param name="sqlString"></param>
		/// <returns></returns>
		private Recordset ExecuteCommand(string sqlString)
        {
            try
            {
                int outRecords = 0;
                ADODB.Command command = new ADODB.Command();
                command.CommandText = sqlString;
                ADODB.Recordset results = m_dataContext.ExecuteCommand(command, out outRecords);
                return results;
            }
            catch (Exception)
            {
                throw;
            }
        }
        /// <summary>
        /// Updates the Workpoint attributes component fiedls.
        /// </summary>
        /// <param name="component">Work Point Attributes component</param>
        private void UpdateWorkPointAttributes(IGTComponent component, string strStructureID)
        {
            Recordset tempRs = null;
            tempRs = component.Recordset;

            if (tempRs != null)
            {
                if (tempRs.RecordCount > 0)
                {
                    component.Recordset.MoveFirst();
                    component.Recordset.Fields["STRUCTURE_ID"].Value = strStructureID;
                    component.Recordset.Fields["WR_NBR"].Value = m_dataContext.ActiveJob;

                    if (m_WorkpointNumber == 0)
                    {
                        m_WorkpointNumber = m_gtcommonFunctions.GetWorkPointNumber();
                    }
                    else
                    {
                        m_WorkpointNumber = m_WorkpointNumber + 1;
                    }
                    component.Recordset.Fields["WP_NBR"].Value = m_WorkpointNumber;
                    component.Recordset.Update();
                }
            }

        }

    }
}
