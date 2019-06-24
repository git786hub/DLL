// ===================================================
//  Copyright 2017 Intergraph Corp.
//  File Name: StreetLightImportUtility.cs
//
//  Description:    Class to provide data required for StreetLight Import.
//  Remarks: 
//  Modification History
//  Date                Author                      Comments
//  --------            --------------              --------
//  03/04/2018          Prathyusha                  Created 
//  12/04/2018          Sitara                      Modified
// ======================================================
using System;
using ADODB;
using Intergraph.GTechnology.API;
using System.Data;
using Excel = Microsoft.Office.Interop.Excel;

namespace GTechnology.Oncor.CustomAPI
{
    class StreetLightImportUtility
    {
        #region Variables

        private IGTDataContext m_oGTDataContext;
        private DataAccess m_dataAccess;
        private IGTKeyObject m_gTOwnerKeyObject;
        private bool m_Islocatable;
        private IGTTransactionManager m_gTTransactionManager;
        public string m_strLampType = string.Empty;
        public string m_strWattage = string.Empty;
        public string m_strLumiStyle = string.Empty;
        public string m_strStatus = string.Empty;
        public string m_strComment = string.Empty;
        private IGTApplication m_gTApplication = null;
        #endregion

        #region Properties
        private IGTKeyObject gTOwnerKeyObject
        {
            get
            {
                return m_gTOwnerKeyObject;
            }
            set
            {
                m_gTOwnerKeyObject = value;
            }
        }

        #endregion

        #region Constructor

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="p_dataContext">The current G/Technology application object.</param>
        public StreetLightImportUtility(IGTDataContext p_oGTDataContext)
        {
            this.m_oGTDataContext = p_oGTDataContext;
        }

        public StreetLightImportUtility(IGTDataContext p_oGTDataContext, bool p_Islocatable
            , IGTTransactionManager p_gTTransactionManager , IGTApplication p_gTApplication)
        {
            this.m_oGTDataContext = p_oGTDataContext;            
            this.m_Islocatable = p_Islocatable;
            this.m_gTTransactionManager = p_gTTransactionManager;
            this.gTOwnerKeyObject = null;
            this.m_gTApplication = p_gTApplication;
            this.m_dataAccess = new DataAccess(p_oGTDataContext, p_gTApplication);
        }

        #endregion

        #region Private Methods
        /// <summary>
        /// GetRelatedFeatures
        /// </summary>
        /// <param name="activeFeature"></param>
        /// <param name="cRNO"></param>
        /// <returns></returns>
        public IGTKeyObjects GetRelatedFeatures(IGTKeyObject p_activeFeature, short p_RNO)
        {
            try
            {
                IGTRelationshipService relService = GTClassFactory.Create<IGTRelationshipService>();
                relService.DataContext = m_oGTDataContext;
                relService.ActiveFeature = p_activeFeature;
                IGTKeyObjects relatedFeatures = relService.GetRelatedFeatures(p_RNO);
                return relatedFeatures;
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Delete Street light and Miscellaneous Structure Features
        /// </summary>
        /// <param name="p_delFeatures">Collection of delete objects</param>
        /// <returns></returns>
        public void DeleteFeatures(IGTKeyObjects p_delFeatures)
        {
            Recordset compRS = null;
            try
            {
                if (p_delFeatures != null && p_delFeatures.Count > 0)
                {
                    for (int i = 0; i < p_delFeatures.Count; i++)
                    {
                        foreach (IGTComponent component in p_delFeatures[i].Components)
                        {
                            compRS = component.Recordset;
                            if (compRS != null && compRS.RecordCount > 0)
                            {
                                compRS.MoveFirst();
                                while (!compRS.EOF)
                                {
                                    compRS.Delete();
                                    compRS.Update();
                                    compRS.MoveNext();
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
        /// Create street light using structure Id.
        /// </summary>
        /// <param name="p_structureId">Structure Id value.</param>
        public void CreateStreetLight(string p_structureId)
        {
            IGTKeyObject gtStreetLightKObject = null;
            try
            {
                gTOwnerKeyObject = m_dataAccess.GetOwner(p_structureId);
                m_strStatus = m_dataAccess.m_strStatus;
                m_strComment = m_dataAccess.m_strComment;
                if (gTOwnerKeyObject != null)
                {
                    gtStreetLightKObject = NewStreetLight();
                }
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Create street light using GPS coordinates.
        /// </summary>
        /// <param name="p_GpsX">GPS X coordinate</param>
        /// <param name="p_GpsY">GPS Y coordinate</param>
        public void CreateStreetLight(double p_GpsX, double p_GpsY)
        {
            IGTKeyObject gtStreetLightKObject = null;
            try
            {
                gTOwnerKeyObject = GetMiscellaneousStructure(p_GpsX, p_GpsY, false, 0);
                if (gTOwnerKeyObject != null)
                {
                    gtStreetLightKObject = NewStreetLight();
                }
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Create street light using AccountStructure.
        /// </summary>
        /// <param name="p_MiscStructureFID">AccountStructure value</param>
        public void CreateStreetLightWithAccountStructure(int p_MiscStructureFID)
        {
            IGTKeyObject gtStreetLightKObject = null;
            try
            {
                gTOwnerKeyObject = GetMiscellaneousStructure(0, 0, true, p_MiscStructureFID);
                if (gTOwnerKeyObject != null)
                {
                    gtStreetLightKObject = NewStreetLight();
                }
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Create street light using AccountBoundary.
        /// </summary>
        /// <param name="p_strESILocation">ESILocation value </param>
        /// <returns></returns>
        public IGTKeyObject CreateStreetLightWithAccountBoundary(string p_strESILocation)
        {
            IGTKeyObject gtStreetLightKObject = null;
            try
            {
                int boundaryFid = m_dataAccess.GetBoundaryFID(p_strESILocation);

                if (boundaryFid != 0)
                {
                    gTOwnerKeyObject = GetMiscellaneousStructure(0, 0, false, 0);

                    if (gTOwnerKeyObject != null)
                    {
                        gtStreetLightKObject = NewStreetLight();
                    }
                }
                else
                {
                    m_strStatus = "ERROR";
                    m_strComment = "Non-Located Street Light cannot be placed due to the absence of both a Miscellaneous Boundary and an account boundary for the Street Light Account.";
                }

            }
            catch
            {
                throw;
            }

            return gTOwnerKeyObject;
        }

        /// <summary>
        /// Update new street light components.
        /// </summary>
        /// <param name="gTStreetLightKeyObject">Key object of street light</param>
        private void UpdateStreetLightComponents(IGTKeyObject gTStreetLightKeyObject)
        {
            try
            {
                foreach (IGTComponent gtCompo in gTStreetLightKeyObject.Components)
                {
                    if (gtCompo.CNO == 5602)
                    {
                        UpdateStreetLightGeometry(gtCompo);
                    }
                    else if (gtCompo.CNO == 5601)
                    {
                        UpdateStreetLightAttributes(gtCompo);
                    }
                    else if (gtCompo.CNO == 21)
                    {
                        if (!string.IsNullOrEmpty(m_strLampType) &&
                                       !string.IsNullOrEmpty(m_strLumiStyle) &&
                                       !string.IsNullOrEmpty(m_strWattage))
                        {
                            UpdateStreetLightCU(gtCompo);
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
        /// Update CU component of street light.
        /// </summary>
        /// <param name="gtCompo">Street light CU component</param>
        private void UpdateStreetLightCU(IGTComponent gtCompo)
        {
            try
            {
                string strCUCode = m_dataAccess.GetCustomerOwnedCUCode(m_strLampType, m_strLumiStyle, m_strWattage);
                if (!string.IsNullOrEmpty(strCUCode))
                {
                    gtCompo.Recordset.MoveFirst();
                    gtCompo.Recordset.Fields["CU_C"].Value = strCUCode;
                    gtCompo.Recordset.Fields["G3E_CNO"].Value = gtCompo.CNO;
                    gtCompo.Recordset.Update();
                }
            }
            catch
            {

            }
        }

        /// <summary>
        /// Update StreetLightAttributes component.
        /// </summary>
        /// <param name="gtCompo">Street light attribute component.</param>
        private void UpdateStreetLightAttributes(IGTComponent gtCompo)
        {
            try
            {
                Recordset tempRs = null;
                tempRs = gtCompo.Recordset;
                if (tempRs != null)
                {
                    if (tempRs.RecordCount > 0)
                    {
                        gtCompo.Recordset.MoveFirst();
                        if (m_Islocatable)
                        {
                            gtCompo.Recordset.Fields["LOCATABLE_YN"].Value = "Y";
                        }
                        else
                        {
                            gtCompo.Recordset.Fields["LOCATABLE_YN"].Value = "N";
                        }


                        gtCompo.Recordset.Update();
                    }
                }
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Update StreetLight Geometry.
        /// </summary>
        /// <param name="gtCompo">Street light attribute symbol component.</param>
        private void UpdateStreetLightGeometry(IGTComponent gtCompo)
        {
            try
            {
                Recordset tempRs = null;
                tempRs = gtCompo.Recordset;
                if (tempRs != null)
                {
                    if (tempRs.RecordCount > 0)
                    {
                        tempRs.MoveFirst();
                        gtCompo.Recordset.MoveFirst();
                        short gTPrimaryCno = GetPrimaryGraphicCno(gTOwnerKeyObject.FNO);
                        IGTComponent gTPrimaryComponent = m_oGTDataContext.OpenFeature(gTOwnerKeyObject.FNO, gTOwnerKeyObject.FID).Components.GetComponent(gTPrimaryCno);
                        if(gTPrimaryComponent != null && gTPrimaryComponent.Recordset != null)
                        {
                            gTPrimaryComponent.Recordset.MoveFirst();
                            gtCompo.Geometry = gTPrimaryComponent.Geometry;
                            tempRs.Fields["G3E_CNO"].Value = gtCompo.CNO;
                            gtCompo.Recordset.Update();
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
        /// Create new street light feature.
        /// </summary>
        /// <returns></returns>
        private IGTKeyObject NewStreetLight()
        {
            IGTKeyObject gtStreetLightKObject;
            try
            {
                m_gTTransactionManager.Begin("Importing Street Light(s) using Import Tool");
                gtStreetLightKObject = m_oGTDataContext.NewFeature(56);
                UpdateStreetLightComponents(gtStreetLightKObject);
                EstablishOwnership(gtStreetLightKObject);
                if (m_gTTransactionManager.TransactionInProgress)
                {
                    m_gTTransactionManager.Commit(true);
                }
            }
            catch
            {
                throw;
            }

            return gtStreetLightKObject;
        }

        /// <summary>
        /// Establish ownership between street light and Miscellaneous Structure.
        /// </summary>
        /// <param name="gtStreetLightKObject"></param>
        private void EstablishOwnership(IGTKeyObject gtStreetLightKObject)
        {
            IGTRelationshipService gtRelationshipService = GTClassFactory.Create<IGTRelationshipService>();
            try
            {
                Recordset ownerRNORecords = m_oGTDataContext.MetadataRecordset("G3E_RELATIONSHIPS_OPTABLE", "G3E_TYPE = 3 AND G3E_TABLE = 'G3E_OWNERSHIP_ELEC' AND G3E_USERNAME = 'Electrical Owned By'");
                ownerRNORecords.MoveFirst();
                gtRelationshipService.DataContext = m_oGTDataContext;
                gtRelationshipService.ActiveFeature = gtStreetLightKObject;
                if (gtRelationshipService.AllowSilentEstablish(gTOwnerKeyObject))
                {
                    gtRelationshipService.SilentEstablish(Convert.ToInt16(ownerRNORecords.Fields["G3E_RNO"].Value), gTOwnerKeyObject);
                }
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Get MiscellaneousStructure of the input data.
        /// </summary>
        /// <param name="p_GpsX">GPS X</param>
        /// <param name="p_GpsY">GPS Y</param>
        /// <param name="p_HasMSFID">MiscellaneousStructure is there or not</param>
        /// <param name="p_MSFID">MiscellaneousStructure FID</param>
        /// <returns></returns>
        private IGTKeyObject GetMiscellaneousStructure(double p_GpsX, double p_GpsY, bool p_HasMSFID, int p_MSFID)
        {
            IGTKeyObject gtMiscellaneousStructureKO = null;

            try
            {

                if (!p_HasMSFID)
                {

                    if (m_dataAccess.m_boundaryFNO != 0)
                    {
                        int miscFid = MISCStruExistAtCentroid();
                        if (miscFid == 0)
                        {
                            m_gTTransactionManager.Begin("Importing Street Light(s) using Import Tool");
                            gtMiscellaneousStructureKO = m_oGTDataContext.NewFeature(107);
                            UpdateMiscellaneousStructureComponents(0, 0, gtMiscellaneousStructureKO, true);
                            if (m_gTTransactionManager.TransactionInProgress)
                            {
                                m_gTTransactionManager.Commit(true);
                            }

                            try
                            {
                                m_gTApplication.Application.Properties.Add("StreetLightImportToolMSFID", gtMiscellaneousStructureKO.FID);
                            }
                            catch
                            {

                            }
                        }
                        else
                        {
                            gtMiscellaneousStructureKO = m_oGTDataContext.OpenFeature(107, miscFid);
                            //UpdateMiscellaneousStructureComponents(0, 0, gtMiscellaneousStructureKO, true);
                        }
                    }
                    else
                    {
                        m_gTTransactionManager.Begin("Importing Street Light(s) using Import Tool");
                        gtMiscellaneousStructureKO = m_oGTDataContext.NewFeature(107);
                        UpdateMiscellaneousStructureComponents(p_GpsX, p_GpsY, gtMiscellaneousStructureKO, false);
                        if (m_gTTransactionManager.TransactionInProgress)
                        {
                            m_gTTransactionManager.Commit(true);
                        }
                    }
                }
                else
                {
                    gtMiscellaneousStructureKO = m_oGTDataContext.OpenFeature(107, p_MSFID);
                }


            }
            catch
            {
                throw;
            }

            return gtMiscellaneousStructureKO;
        }

        /// <summary>
        /// Update MiscellaneousStructure components.
        /// </summary>
        /// <param name="p_GpsX">GPS X</param>
        /// <param name="p_GpsY">GPS Y</param>
        /// <param name="gtMiscellaneousStructureKO">MiscellaneousStructure Keyobject</param>
        /// <param name="p_ISNonLoc">Is MiscellaneousStructure locatable or not</param>
        private void UpdateMiscellaneousStructureComponents(double p_GpsX, double p_GpsY,
            IGTKeyObject gtMiscellaneousStructureKO, bool p_ISNonLoc)
        {
            Recordset tempRs = null;
            try
            {
                foreach (IGTComponent gtCompo in gtMiscellaneousStructureKO.Components)
                {
                    if (gtCompo.CNO == 10702)
                    {
                        tempRs = gtCompo.Recordset;
                        if (tempRs != null)
                        {
                            if (tempRs.RecordCount > 0)
                            {
                                if (p_ISNonLoc)
                                {
                                    IGTGeometry bndryGeom = m_oGTDataContext.GetDDCKeyObjects(m_dataAccess.m_boundaryFNO,
                                        m_dataAccess.m_boundaryFID, GTComponentGeometryConstants.gtddcgPrimaryGeographic)[0].Geometry;
                                    IGTPoint gtOrigin = CalculateCentroid(bndryGeom);
                                    IGTOrientedPointGeometry gtPointGeom = GTClassFactory.Create<IGTOrientedPointGeometry>();
                                    gtPointGeom.Origin = gtOrigin;

                                    gtCompo.Geometry = gtPointGeom;
                                    gtCompo.Recordset.Update();
                                }
                                else
                                {
                                    IGTPoint gTPoint = GTClassFactory.Create<IGTPoint>();
                                    gTPoint.X = p_GpsX;
                                    gTPoint.Y = p_GpsY;

                                    IGTPointGeometry gtPointGeom = GTClassFactory.Create<IGTOrientedPointGeometry>();
                                    gtPointGeom.Origin = gTPoint;

                                    gtCompo.Geometry = gtPointGeom;
                                    gtCompo.Recordset.Update();
                                }
                            }
                        }
                    }
                    else if (gtCompo.CNO == 10701)
                    {

                        if (p_ISNonLoc)
                        { gtCompo.Recordset.Fields["TYPE_C"].Value = "SPNL"; }
                        else
                        {
                            gtCompo.Recordset.Fields["TYPE_C"].Value = "SP";
                        }
                        gtCompo.Recordset.Fields["g3e_fno"].Value = 107;
                        gtCompo.Recordset.Fields["g3e_fid"].Value = gtMiscellaneousStructureKO.FID;
                        gtCompo.Recordset.Fields["g3e_cno"].Value = gtCompo.CNO;
                        gtCompo.Recordset.Update();

                    }
                    else if (gtCompo.CNO == 1)
                    {
                        tempRs = gtCompo.Recordset;
                        if (tempRs != null)
                        {
                            if (tempRs.RecordCount > 0)
                            {
                                tempRs.MoveFirst();
                                if (!p_ISNonLoc)
                                    tempRs.Fields["FEATURE_STATE_C"].Value = "CLS";
                                tempRs.Fields["OWNED_TYPE_C"].Value = "CUSTOMER";
                                tempRs.Fields["G3E_CNO"].Value = gtCompo.CNO;
                                tempRs.Update();
                                gtCompo.Recordset.Update();
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
        /// Get MiscellaneousStructure Fid from Esi Location
        /// </summary>
        /// <param name="p_strESILocation">Esi Location</param>
        /// <returns></returns>
        public int GetMiscStructureFID(string p_strESILocation)
        {
            int MiscStructureFID = 0;
            try
            {
                MiscStructureFID = m_dataAccess.GetMiscellaneousStructure(p_strESILocation);
            }
            catch
            {
                throw;
            }
            return MiscStructureFID;
        }


        /// <summary>
        ///  Method to get the primary graphic cno for fno
        /// </summary>
        /// <param name="fNo"></param>
        /// <returns>cno or 0</returns>
        internal short GetPrimaryGraphicCno(short fNo)
        {
            short primaryGraphicCno = 0;
            Recordset tempRs = null;
            try
            {
                tempRs = m_oGTDataContext.MetadataRecordset("G3E_FEATURES_OPTABLE", "G3E_FNO = " + fNo);
                if (tempRs != null && tempRs.RecordCount > 0)
                {
                    tempRs.MoveFirst();
                    primaryGraphicCno = Convert.ToInt16(tempRs.Fields["G3E_PRIMARYGEOGRAPHICCNO"].Value);
                }
                return primaryGraphicCno;
            }
            catch
            {
                throw;
            }
            finally
            {
                if (tempRs != null)
                {
                    tempRs.Close();
                    tempRs = null;
                }
            }
        }

        public int MISCStruExistAtCentroid()
        {
            int miscStruFID = 0;            
            try
            {
                object obj = m_gTApplication.Application.Properties["StreetLightImportToolMSFID"];
                miscStruFID = Convert.ToInt32(obj);
            }
            catch
            {

            }
            return miscStruFID;
        }

        /// <summary>
        /// Calculate Centroid for given boundayr Geometry
        /// </summary>
        /// <param name="gtGeometry"></param>
        /// <returns></returns>
        public IGTPoint CalculateCentroid(IGTGeometry gtGeometry)
        {
            IGTPoint gtCenterPt = GTClassFactory.Create<IGTPoint>();
            gtCenterPt.X = 0;
            gtCenterPt.Y = 0;
            gtCenterPt.Z = 0;
            if (gtGeometry.KeypointCount > 1)
            {
                for (int indx = 0; indx < gtGeometry.KeypointCount - 1; indx++)
                {
                    gtCenterPt.X += gtGeometry.GetKeypointPosition(indx).X;
                    gtCenterPt.Y += gtGeometry.GetKeypointPosition(indx).Y;
                    gtCenterPt.Z += gtGeometry.GetKeypointPosition(indx).Z;
                }
                gtCenterPt.X /= gtGeometry.KeypointCount - 1;
                gtCenterPt.Y /= gtGeometry.KeypointCount - 1;
                gtCenterPt.Z /= gtGeometry.KeypointCount - 1;
            }
            else
            {
                gtCenterPt = gtGeometry.FirstPoint;
            }
            return gtCenterPt;
        }


        #endregion
    }
}
