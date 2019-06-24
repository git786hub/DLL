// ===================================================
//  Copyright 2017 Intergraph Corp.
//  File Name: StreetLightRemoveProcessing.cs
//
//  Description: Class which processes each record of spreadsheet having TranstionType 'Remove'.
//  Remarks: 
//  Modification History
//  Date                Author                      Comments
//  --------            --------------              --------
//  03/04/2018          Prathyusha                  Created 
// ==========================================================
using System;
using System.Data;
using ADODB;
using Intergraph.GTechnology.API;

namespace GTechnology.Oncor.CustomAPI
{
    class StreetLightRemoveProcessing
    {
        #region Variables

        private IGTDataContext m_oGTDataContext;
        private IGTTransactionManager m_oGTTransactionManager;
        private int m_nbrSuccess = 0;
        private int m_nbrWarning = 0;
        private int m_nbrError = 0;

        #endregion

        #region Constructor
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="p_dataContext">The current G/Technology application object.</param>
        /// <param name="p_transactionManager">The transaction of G/Technology application.</param>
        public StreetLightRemoveProcessing(IGTDataContext p_dataContext, IGTTransactionManager p_transactionManager)
        {
            m_oGTDataContext = p_dataContext;
            m_oGTTransactionManager = p_transactionManager;
        }
        #endregion

        #region Properties
        public int SuccessfulRemovalTransactions
        {
            get { return m_nbrSuccess; }
            set { m_nbrSuccess = value; }
        }
        public int WarningRemovalTransactions
        {
            get { return m_nbrWarning; }
            set { m_nbrWarning = value; }
        }
        public int ErrorRemovalTransactions
        {
            get { return m_nbrError; }
            set { m_nbrError = value; }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Method to delete each record of import worksheet StreetLight of TranstionType 'Remove' .
        /// </summary>
        /// <param name="p_dataRow">Worksheet data Datatable</param>
        /// <returns></returns>
        public DataRow DeleteStreetLightForTranstionTypeRemove(DataRow p_dataRow)
        {
            DataAccess dataAccess = new DataAccess(m_oGTDataContext);
            Recordset streetLightAttributeRS = null;
            try
            {
                streetLightAttributeRS = dataAccess.GetRecordset(String.Format("select * from STREETLIGHT_N WHERE ACCOUNT_ID='{0}' and CO_IDENTIFIER='{1}'", Convert.ToString(p_dataRow["ESI LOCATION"]), Convert.ToString(p_dataRow["STREETLIGHT ID"])));

                if (streetLightAttributeRS != null && streetLightAttributeRS.RecordCount == 1)
                {
                    DeleteStreetLight(streetLightAttributeRS, true);

                    p_dataRow["TRANSACTION STATUS"] = "SUCCESS";
                    p_dataRow["TRANSACTION COMMENT"] = null;

                    m_nbrSuccess++;
                }
                else
                {
                    streetLightAttributeRS = dataAccess.GetRecordset(String.Format("select * from STREETLIGHT_N WHERE ACCOUNT_ID='{0}' AND CO_IDENTIFIER is null AND LOCATABLE_YN='N'", Convert.ToString(p_dataRow["ESI LOCATION"])));

                    if (streetLightAttributeRS != null && streetLightAttributeRS.RecordCount == 1)
                    {
                        DeleteStreetLight(streetLightAttributeRS, false);

                        p_dataRow["TRANSACTION STATUS"] = "WARNING";
                        p_dataRow["TRANSACTION COMMENT"] = "Removed a non-located Street Light that have that ESI Location but the Street Light ID is missing.";

                        m_nbrWarning++;
                    }
                    else
                    {
                        p_dataRow["TRANSACTION STATUS"] = "ERROR";
                        p_dataRow["TRANSACTION COMMENT"] = "Street Light could not be located for a removal action";

                        m_nbrError++;
                    }
                }
            }
            catch
            {
                throw;
            }
            finally
            {
                dataAccess = null;
            }

            return p_dataRow;
        }

        /// <summary>
        /// Method to delete StreetLight.
        /// </summary>
        /// <param name="streetLightAttributeRS">Recordset of StreetLight Attribute</param>
        /// <param name="locatable">Bool value whether StreetLight is Located to delete</param>
        /// <returns></returns>
        private void DeleteStreetLight(Recordset streetLightAttributeRS, bool locatable)
        {
            IGTKeyObjects relatedFeatures = GTClassFactory.Create<IGTKeyObjects>();
            IGTKeyObjects deleteFeatures = GTClassFactory.Create<IGTKeyObjects>();
            IGTKeyObject feature = GTClassFactory.Create<IGTKeyObject>();
            try
            {
                StreetLightImportUtility importUtility = new StreetLightImportUtility(m_oGTDataContext);

                if (!m_oGTTransactionManager.TransactionInProgress)
                {
                    m_oGTTransactionManager.Begin("Importing Street Light(s) using Import Tool");
                }

                if (locatable)
                {
                    streetLightAttributeRS.MoveFirst();

                    feature = m_oGTDataContext.OpenFeature(Convert.ToInt16(streetLightAttributeRS.Fields["G3E_FNO"].Value), Convert.ToInt32(streetLightAttributeRS.Fields["G3E_FID"].Value));
                    deleteFeatures.Add(feature);

                    relatedFeatures = importUtility.GetRelatedFeatures(feature, 3);
                    if (relatedFeatures.Count > 0)
                    {
                        feature = CheckForMiscellaneousStructure(relatedFeatures, feature.FID);

                        if (feature != null)
                        {
                            deleteFeatures.Add(feature);
                        }
                    }
                }
                else
                {
                    streetLightAttributeRS.MoveFirst();
                    while (!streetLightAttributeRS.EOF)
                    {
                        feature = m_oGTDataContext.OpenFeature(Convert.ToInt16(streetLightAttributeRS.Fields["G3E_FNO"].Value), Convert.ToInt32(streetLightAttributeRS.Fields["G3E_FID"].Value));
                        deleteFeatures.Add(feature);

                        streetLightAttributeRS.MoveNext();
                    }
                }
                importUtility.DeleteFeatures(deleteFeatures);

                if (m_oGTTransactionManager.TransactionInProgress)
                {
                    m_oGTTransactionManager.Commit(true);
                    m_oGTTransactionManager.RefreshDatabaseChanges();
                }
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Method to return Miscellaneous Structure of type Street Light Point which is owner of Located Street Light which is getting removed.
        /// </summary>
        /// <param name="relatedFeatures">RelatedFeatures list of Located StreetLight</param>
        /// <param name="streetLightFID">Located Street Light FID</param>
        /// <returns></returns>
        private IGTKeyObject CheckForMiscellaneousStructure(IGTKeyObjects relatedFeatures, int streetLightFID)
        {
            IGTKeyObject structureFeature = null;
            try
            {
                foreach (IGTKeyObject relatedFeature in relatedFeatures)
                {
                    if (relatedFeature.FNO == 107)
                    {
                        relatedFeature.Components.GetComponent(10701).Recordset.MoveFirst();

                        if (Convert.ToString(relatedFeature.Components.GetComponent(10701).Recordset.Fields["TYPE_C"].Value) == "SP")
                        {
                            StreetLightImportUtility importUtility = new StreetLightImportUtility(m_oGTDataContext);
                            relatedFeatures = importUtility.GetRelatedFeatures(relatedFeature, 2);

                            if (relatedFeatures.Count == 1 && relatedFeatures[0].FID == streetLightFID)
                            {
                                structureFeature = relatedFeatures[0];
                            }
                        }
                    }
                }
            }
            catch
            {
                throw;
            }
            return structureFeature;
        }
        #endregion
    }
}
