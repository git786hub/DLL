// ===================================================
//  Copyright 2017 Intergraph Corp.
//  File Name: VirtualPointAssociationUtility.cs
//
//  Description:    Class to provide data required for VirtualPointAssociation.
//  Remarks: 
//  Modification History
//  Date                Author                      Comments
//  --------            --------------              --------
//  15/04/2018          Prathyusha                  Created 
// ======================================================
using ADODB;
using Intergraph.GTechnology.API;
using System;
using System.Collections.Generic;
using System.Data;

namespace GTechnology.Oncor.CustomAPI
{
    class VirtualPointAssociationUtility
    {
        #region Variables

        private IGTApplication m_oGTApplication;

        #endregion

        #region Constructor
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="p_application">The current G/Technology application object.</param>
        public VirtualPointAssociationUtility(IGTApplication p_application)
        {
            m_oGTApplication = p_application;
        }

        #endregion

        #region Methods
        public void GetRecorsetOfAssociateFeaturesForVP(IGTGeometry p_tempGeometry,short[] lstFNO, ref DataTable p_associatedRs, short p_AactiveFno, Dictionary<short, string> p_relatedFeatures)
        {
            DataAccess dataAccess = null;
            Recordset rsTemp = null;
            IGTKeyObject gTKeyObject = null;

            List<string> IsolationFs = new List<string> { "PPI", "ABI" };
            List<string> byPassFs = new List<string> { "PPR", "ABR", "PPI", "ABI" };
            bool emptyRecord = true;
            p_associatedRs = new DataTable("Results");
            p_associatedRs.Columns.Add("Feature");
            p_associatedRs.Columns.Add("Feature Instance");
            p_associatedRs.Columns.Add("G3E_FNO");
            string value = null;

            try
            {
                IGTSpatialService spatialService = GTClassFactory.Create<IGTSpatialService>();
                spatialService.DataContext = m_oGTApplication.DataContext;
                spatialService.Operator = GTSpatialOperatorConstants.gtsoIndexIntersect;
                spatialService.FilterGeometry = p_tempGeometry;

                Recordset rsCommon = null;
                

                rsTemp = spatialService.GetResultsByFNO(lstFNO);

                if (rsTemp != null && rsTemp.RecordCount > 0)
                {
                    rsTemp.MoveFirst();

                    while (!rsTemp.EOF)
                    {

                        gTKeyObject = m_oGTApplication.DataContext.OpenFeature(Convert.ToInt16(rsTemp.Fields["g3e_fno"].Value),
                            Convert.ToInt32(rsTemp.Fields["G3E_FID"].Value));
                        rsCommon = gTKeyObject.Components["COMMON_N"].Recordset;

                        if (rsCommon != null && rsCommon.RecordCount > 0)
                        {
                            rsCommon.MoveFirst();

                            if (p_AactiveFno == 40 || p_AactiveFno == 80)
                            {
                                if (byPassFs.Contains(Convert.ToString(rsCommon.Fields["FEATURE_STATE_C"].Value)))
                                {
                                    DataRow row = p_associatedRs.NewRow();
                                    value = "";

                                    p_relatedFeatures.TryGetValue(Convert.ToInt16(rsTemp.Fields["g3e_fno"].Value), out value);

                                    row["Feature"] = value;
                                    row["Feature Instance"] = Convert.ToInt32(rsTemp.Fields["G3E_FID"].Value);
                                    row["G3E_FNO"] = Convert.ToInt16(rsTemp.Fields["g3e_fno"].Value);

                                    p_associatedRs.Rows.Add(row);

                                    emptyRecord = false;
                                }
                            }
                            else if (p_AactiveFno == 6 || p_AactiveFno == 82)
                            {
                                if (IsolationFs.Contains(Convert.ToString(rsCommon.Fields["FEATURE_STATE_C"].Value)))
                                {
                                    DataRow row = p_associatedRs.NewRow();
                                    value = "";
                                    p_relatedFeatures.TryGetValue(Convert.ToInt16(rsTemp.Fields["g3e_fno"].Value), out value);
                                    row["Feature"] = value;
                                    row["Feature Instance"] = Convert.ToInt32(rsTemp.Fields["G3E_FID"].Value);
                                    row["G3E_FNO"] = Convert.ToInt16(rsTemp.Fields["g3e_fno"].Value);

                                    p_associatedRs.Rows.Add(row);

                                    emptyRecord = false;
                                }
                            }
                        }
                        rsTemp.MoveNext();
                    }
                   
                }              
                
                if(emptyRecord)
                {
                    p_associatedRs = null;
                }

            }
            catch
            {
                throw;
            }
            finally
            {
                if(dataAccess!=null)
                dataAccess = null;
            }
        }
      
        public void HighLightOnMapWindow(short p_fno, int p_fid)
        {
            try
            {
                IGTDDCKeyObject gTDDCKeyObject = m_oGTApplication.DataContext.GetDDCKeyObjects(p_fno, p_fid, GTComponentGeometryConstants.gtddcgAllGeographic)[0];

                m_oGTApplication.ActiveMapWindow.HighlightedObjects.Clear();
                m_oGTApplication.ActiveMapWindow.HighlightedObjects.AddSingle(gTDDCKeyObject);
                m_oGTApplication.ActiveMapWindow.SelectBehavior = GTSelectBehaviorConstants.gtmwsbHighlightAndCenter;

                m_oGTApplication.RefreshWindows();
            }
            catch
            {
                throw;
            }
        }

        public void CreateTemporaryGeometryForVirtualPoint(IGTGeometry virtualPtGeom, double p_range, ref IGTGeometry p_tempGeom)
        {
            try
            {
                IGTZoneService gTZoneService = GTClassFactory.Create<IGTZoneService>();
                gTZoneService.ZoneWidth = p_range;
                gTZoneService.InputGeometries = virtualPtGeom;
                p_tempGeom = gTZoneService.OutputGeometries;
            }
            catch
            {
                throw;
            }
        }
        #endregion
    }
}
