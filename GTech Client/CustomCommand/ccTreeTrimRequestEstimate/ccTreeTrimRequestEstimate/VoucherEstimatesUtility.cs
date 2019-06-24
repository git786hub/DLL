// ===================================================
//  Copyright 2017 Intergraph Corp.
//  File Name: ProcessVoucherEstimates.cs
//
//  Description:    Class to provide data required for VoucherEstimates.
//  Remarks: 
//  Modification History
//  Date                Author                      Comments
//  --------            --------------              --------
//  15/03/2018          Prathyusha                  Created 
//  08/04/2019          Prathyusha                  Modfied as part of ALM-2197
// ======================================================
using System;
using ADODB;
using Intergraph.GTechnology.API;

namespace GTechnology.Oncor.CustomAPI
{
    class VoucherEstimatesUtility
    {
        #region Variables

        private IGTApplication m_oGTApp;

        #endregion

        #region Constructor
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="p_application">The current G/Technology application object.</param>
        public VoucherEstimatesUtility(IGTApplication p_oGTApp)
        {
            this.m_oGTApp = p_oGTApp;
        }
        #endregion

        #region Methods
        /// <summary>
        /// Method to build the Work Point features Account Recordset.
        /// </summary>
        /// <param name="p_workPointRS">Workpoint Recordsets Contained by placed TreeTrimming feature</param>
        /// <returns></returns>
        public Recordset GetWorkPtAccountRecordset(Recordset p_workPointRS)
        {
            Recordset accountsRS = null;
            int wpFid = 0;
            try
            {
                p_workPointRS.MoveFirst();

                accountsRS = new Recordset();
                accountsRS.Fields.Append("Prime Account", DataTypeEnum.adInteger, 4, FieldAttributeEnum.adFldIsNullable);
                accountsRS.Fields.Append("Sub Account", DataTypeEnum.adInteger, 4, FieldAttributeEnum.adFldIsNullable);
                accountsRS.Open(System.Reflection.Missing.Value, System.Reflection.Missing.Value, CursorTypeEnum.adOpenDynamic, LockTypeEnum.adLockOptimistic, -1);

                while (!p_workPointRS.EOF)
                {
                    wpFid = Convert.ToInt32(p_workPointRS.Fields["G3E_FID"].Value.ToString());

                    Recordset workpointAttributeRS = m_oGTApp.DataContext.OpenRecordset(String.Format("SELECT DISTINCT PRIME_ACCT_ID,SUB_ACCT FROM WORKPOINT_CU_N CU,WORKPOINT_N WP,REFWMIS_FERC_ACCOUNT REF WHERE CU.G3E_FID=WP.G3E_FID AND WP.WR_NBR='{0}' AND REF.PRIME_ACCT=CU.PRIME_ACCT_ID AND REF.ACTIVITY_C=CU.ACTIVITY_C AND WP.G3E_FID={1}", m_oGTApp.DataContext.ActiveJob, wpFid), CursorTypeEnum.adOpenStatic,
                               LockTypeEnum.adLockReadOnly, (int)CommandTypeEnum.adCmdText);

                    if (workpointAttributeRS != null && workpointAttributeRS.RecordCount > 0)
                    {
                        workpointAttributeRS.MoveFirst();

                        while (!workpointAttributeRS.EOF)
                        {

                            if (workpointAttributeRS.Fields["PRIME_ACCT_ID"].Value != DBNull.Value || workpointAttributeRS.Fields["SUB_ACCT"].Value != DBNull.Value)
                            {
                                accountsRS.AddNew();

                                accountsRS.Fields["Prime Account"].Value = workpointAttributeRS.Fields["PRIME_ACCT_ID"].Value;
                                accountsRS.Fields["Sub Account"].Value = workpointAttributeRS.Fields["SUB_ACCT"].Value;

                                accountsRS.Update(System.Reflection.Missing.Value, System.Reflection.Missing.Value);

                            }

                            workpointAttributeRS.MoveNext();
                        }
                    }
                    p_workPointRS.MoveNext();
                }          
            }
            catch
            {
                throw;
            }
            return accountsRS;
        }

        /// <summary>
        /// Method to Get Workpoint Recordset Contained by placed TreeTrimming feature.
        /// </summary>
        /// <param name="p_oTreeTrimmingfeature">Current placed feature</param>
        /// <returns></returns>
        public Recordset GetWorkpointRsContainedByTreeTrimming(IGTKeyObject p_oTreeTrimmingfeature)
        {
            Recordset wpRs = null;
            try
            {
                IGTSpatialService spatialService = GTClassFactory.Create<IGTSpatialService>();
                spatialService.DataContext = m_oGTApp.DataContext;
                spatialService.Operator = GTSpatialOperatorConstants.gtsoEntirelyContains;
                spatialService.FilterGeometry = p_oTreeTrimmingfeature.Components.GetComponent(19002).Geometry;
                wpRs = spatialService.GetResultsByFNO(new short[] { 191 });
            }
            catch (Exception)
            {
                throw;
            }
            return wpRs;
        }
        #endregion
    }
}
