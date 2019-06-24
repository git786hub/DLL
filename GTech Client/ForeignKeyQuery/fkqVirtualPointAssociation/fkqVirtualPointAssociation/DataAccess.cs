// ===================================================
//  Copyright 2017 Intergraph Corp.
//  File Name: DataAccess.cs
//
//  Description:    Class to build the recordset.
//  Remarks: 
//  Modification History
//  Date                Author                      Comments
//  --------            --------------              --------
//  15/04/2019           Prathyusha                  Created 
// ======================================================
using System;
using System.Collections.Generic;
using ADODB;
using Intergraph.GTechnology.API;

namespace GTechnology.Oncor.CustomAPI
{
    class DataAccess
    {
        #region Variables

        private IGTApplication m_oGTApp;

        #endregion

        #region Constructor
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="p_application">The current G/Technology application object.</param>
        public DataAccess(IGTApplication p_oGTApp)
        {
           m_oGTApp = p_oGTApp;
        }
        #endregion

        #region Methods

        /// <summary>
        /// Returns the Primary geometry of the feature
        /// </summary>
        /// <param name="feature"></param>
        /// <returns></returns>
        public void GetPrimaryGeometry(IGTKeyObject feature, ref IGTGeometry primaryGeometry)
        {
            IGTComponent component = null;
            try
            {
                Recordset oRSFeature = m_oGTApp.DataContext.MetadataRecordset("G3E_FEATURES_OPTABLE", "G3E_FNO=" + feature.FNO);


                if (!(oRSFeature.Fields["G3E_PRIMARYGEOGRAPHICCNO"].Value is DBNull))
                {
                    component = feature.Components.GetComponent(Convert.ToInt16(oRSFeature.Fields["G3E_PRIMARYGEOGRAPHICCNO"].Value));

                    if (component != null && component.Recordset != null && component.Recordset.RecordCount == 1)
                    {
                        primaryGeometry = component.Geometry;
                    }

                    else if (component != null && component.Recordset != null && component.Recordset.RecordCount > 1)
                    {
                        throw new DuplicateIdentifierException(string.Format("More than one feature exists with the same feature identifier {0}. Please contact system administrator.", feature.FID));
                    }
                    else
                    {
                        if (!(oRSFeature.Fields["G3E_PRIMARYDETAILCNO"].Value is DBNull))
                        {
                            component = feature.Components.GetComponent(Convert.ToInt16(oRSFeature.Fields["G3E_PRIMARYDETAILCNO"].Value));

                            if (component != null && component.Recordset != null && component.Recordset.RecordCount > 0)
                            {
                                primaryGeometry = component.Geometry;
                            }
                        }
                    }
                }

                else if (!(oRSFeature.Fields["G3E_PRIMARYDETAILCNO"].Value is DBNull))
                {
                    component = feature.Components.GetComponent(Convert.ToInt16(oRSFeature.Fields["G3E_PRIMARYDETAILCNO"].Value));

                    if (component != null && component.Recordset != null && component.Recordset.RecordCount > 0)
                    {
                        primaryGeometry = component.Geometry;
                    }
                    else
                    {
                        if (!(oRSFeature.Fields["G3E_PRIMARYGEOGRAPHICCNO"].Value is DBNull))
                        {
                            component = feature.Components.GetComponent(Convert.ToInt16(oRSFeature.Fields["G3E_PRIMARYGEOGRAPHICCNO"].Value));

                            if (component != null && component.Recordset != null && component.Recordset.RecordCount > 0)
                            {
                                primaryGeometry = component.Geometry;
                            }
                        }
                    }
                }
            }

            catch (DuplicateIdentifierException ex)
            {
                throw ex;
            }
            catch
            {
                throw;
            }
        }
        public void GetRangeValueToLocateAssociateFeatures(ref double p_locateRange)
        {
            Recordset rs = null;
            try
            {
                string sql = "select PARAM_VALUE from SYS_GENERALPARAMETER where PARAM_NAME='VirtualPoint_LocateRange'"; 
                 rs = m_oGTApp.DataContext.OpenRecordset(sql, CursorTypeEnum.adOpenStatic,
                               LockTypeEnum.adLockReadOnly, (int)CommandTypeEnum.adCmdText);

                if (rs != null && rs.RecordCount > 0)
                {
                    rs.MoveFirst();

                    p_locateRange = Convert.ToDouble(rs.Fields[0].Value);
                }

            }
            catch
            {
                throw;
            }
            finally
            {
                if(rs != null)
                rs = null;
            }
        }

        public void GetListOfRelatedFeaturesOfVP(short p_virtualPtFno, ref Dictionary<short, string> p_relatedFeatures)
        {
            try
            {
                Recordset rsConnectivity = m_oGTApp.DataContext.OpenRecordset(String.Format("SELECT DISTINCT G3E_CONNECTINGFNO,G3E_USERNAME FROM G3E_NODEEDGECONN_ELEC_OPTABLE NC,G3E_FEATURES_OPTABLE F WHERE G3E_SOURCEFNO = {0} AND NC.G3E_CONNECTINGFNO=F.G3E_FNO", p_virtualPtFno), CursorTypeEnum.adOpenStatic,
                         LockTypeEnum.adLockReadOnly, (int)CommandTypeEnum.adCmdText);

                if (rsConnectivity != null && rsConnectivity.RecordCount > 0)
                {
                    rsConnectivity.MoveFirst();

                    while (!rsConnectivity.EOF)
                    {
                        p_relatedFeatures.Add(Convert.ToInt16(rsConnectivity.Fields["G3E_CONNECTINGFNO"].Value), Convert.ToString(rsConnectivity.Fields["G3E_USERNAME"].Value));
                        rsConnectivity.MoveNext();
                    }
                }
            }
            catch
            {
                throw;
            }
        }

        #endregion
    }
}
