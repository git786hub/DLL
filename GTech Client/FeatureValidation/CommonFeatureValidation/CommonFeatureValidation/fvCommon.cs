// =====================================================================================================================================================================
//  File Name: fvCommon.cs
// 
// Description:   

 // This class contains validation logic for rule parameters (i.e., RESTRICTION, ACCOUNTING).The rules parameters for Common and CU component are configured as Interface Arguments 
 // and either Restricted Area validation is fired or Accounting Impact validation is fired or both of them are fired depending on the configuration.
 
 //  Remarks:
// 
//  Modification History
//  Date                Author                      Comments
//  --------            --------------              --------
//  04/09/2018          Hari                       Initial sources
// =====================================================================================================================================================================

using System;
using System.Collections.Generic;
using System.Windows.Forms;
using ADODB;
using Intergraph.GTechnology.API;
using Intergraph.GTechnology.Interfaces;
using System.Linq;

namespace GTechnology.Oncor.CustomAPI
{    public class fvCommon : IGTFeatureValidation
    {
        private GTArguments m_vArgumentArray;
        private IGTDataContext m_oDataContext;
        private string m_sProcessingMode;
        private string m_fvOption = string.Empty;

        public GTArguments Arguments { get => m_vArgumentArray; set => m_vArgumentArray = value; }
        public IGTDataContext DataContext { get => m_oDataContext; set => m_oDataContext = value; }
        public string ProcessingMode { get => m_sProcessingMode; set => m_sProcessingMode = value; }

        public bool Validate(IGTKeyObjects Features, Recordset ValidationErrors)
        {
            FVObjectFactory objectFactory = null;
            bool result = false;

            bool accountingImpactValidated = false;
            bool restrcitedAreaValidated = false;
            try
            {
                if (m_vArgumentArray != null && m_vArgumentArray.Count == 0)
                    return false;

                //Read the fvOption from arguments
                string value = Convert.ToString(m_vArgumentArray.GetArgument(0));
                List<string> fvOptions = value.Split(',').ToList();

                foreach (string option in fvOptions)
                {
                    if (option.Equals("ACCOUNTING"))
                    {
                        if (ValidatePlaceOperation(Features))
                            {
                            if (ValidateDeleteOperation(Features)) //ALM- 1041 
                            {
                                objectFactory = new FVObjectFactory(DataContext, Features, ValidationErrors);
                                IProcessFV accountingImpact = objectFactory.GetFVObject(ProcessObjectType.AccountImpact);
                                m_fvOption = "Accounting Impact";
                                accountingImpactValidated = accountingImpact.Process();
                            }
                            else
                            {
                                MessageBox.Show("Cannot delete property related components installed by a different job." + Environment.NewLine,
                                "G/Techonology", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                return false;
                            }
                        }
                        else
                        {
                            MessageBox.Show("Cannot add property related components in Maintenance Job" + Environment.NewLine,
                               "G/Techonology", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return false;
                        }
                       
                    }

                    else if (option.Equals("RESTRICTION"))
                    {
                        objectFactory = new FVObjectFactory(DataContext, Features, ValidationErrors);
                        IProcessFV restrictedArea = objectFactory.GetFVObject(ProcessObjectType.RestrictedArea);
                        m_fvOption = "Restricted Area";
                        restrcitedAreaValidated = restrictedArea.Process();
                    }
                }

                if (fvOptions.Count == 1)
                {
                    return accountingImpactValidated || restrcitedAreaValidated;
                }

                if (fvOptions.Count == 2)
                {
                    return accountingImpactValidated && restrcitedAreaValidated;
                }

                return false;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error during execution of " + m_fvOption + Environment.NewLine + ex.Message,
                                "G/Techonology", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }
        private bool IsCorrectionJob()
        {
            bool bReturn = false;

            try
            {
                ADODB.Recordset rs = m_oDataContext.OpenRecordset("select G3E_JOBTYPE from g3e_job where g3e_identifier = ?", ADODB.CursorTypeEnum.adOpenDynamic, ADODB.LockTypeEnum.adLockOptimistic, (int)ADODB.CommandTypeEnum.adCmdText, m_oDataContext.ActiveJob);

                rs.MoveFirst();
                if (Convert.ToString(rs.Fields["G3E_JOBTYPE"].Value).Equals("NON-WR"))
                {
                    bReturn = true;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return bReturn;
        }
        private bool ValidatePlaceOperation(IGTKeyObjects p_features)
        {
            bool bReturn = true;

            Recordset rsCUComponent = null;
            Recordset rsAncuComponent = null;
            Recordset rsCU = null;
            string sJobType = string.Empty;

            if (IsCorrectionJob())
            {
                string sql = "select count(*) from COMMON_N where g3e_fid=?";
                Dictionary<int, string> missedCidList = new Dictionary<int, string>();

                try
                {
                    foreach (IGTKeyObject gTKeyObject in p_features)
                    {
                        if (gTKeyObject.Components["COMP_UNIT_N"] != null)
                        {
                            rsCUComponent = gTKeyObject.Components["COMP_UNIT_N"].Recordset;
                        }
                        if (gTKeyObject.Components["COMP_UNIT_ANCIL_N"] != null)
                        {
                            rsAncuComponent = gTKeyObject.Components["COMP_UNIT_ANCIL_N"].Recordset;
                        }

                        if (rsCUComponent != null)
                        {
                            #region CU Attributes processing

                            rsCU = DataContext.OpenRecordset(sql, CursorTypeEnum.adOpenStatic, LockTypeEnum.adLockReadOnly,
                                (int)CommandTypeEnum.adCmdText,                                 
                                        gTKeyObject.FID);

                            if (rsCUComponent != null && rsCUComponent.RecordCount>0 && Convert.ToInt32(rsCU.Fields[0].Value).Equals(0))
                            {
                                //This feature is a new placement
                                bReturn = false;
                            }

                            #endregion CU Attributes processing
                        }

                        if (!bReturn) return bReturn;

                        missedCidList = new Dictionary<int, string>();
                        rsCU = null;

                        if (rsAncuComponent != null)
                        {
                            #region Ancillary CU Attributes processing

                            rsCU = DataContext.OpenRecordset(sql, CursorTypeEnum.adOpenStatic, LockTypeEnum.adLockReadOnly,
                                (int)CommandTypeEnum.adCmdText,                                        
                                        gTKeyObject.FID);


                            if (rsAncuComponent != null && rsAncuComponent.RecordCount>0 && Convert.ToInt32(rsCU.Fields[0].Value).Equals(0))
                            {
                                //This feature is a new placement
                                bReturn = false;
                            }

                            #endregion Ancillary CU Attributes processing              
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            return bReturn;
        }

        private bool ValidateDeleteOperation(IGTKeyObjects features)
        {
            Recordset rsCUComponent = null;
            Recordset rsAncuComponent = null;
            Recordset rsCU = null;

            string sql = "select WR_ID,g3e_cid,CU_C,G3E_CNO from COMP_UNIT_N where g3e_cno=? and g3e_fid=?";

           
            Dictionary<int, string> missedCidList = new Dictionary<int, string>();

            try
            {
                foreach (IGTKeyObject gTKeyObject in features)
                {
                    if (gTKeyObject.Components["COMP_UNIT_N"] != null)
                    {
                        rsCUComponent = gTKeyObject.Components["COMP_UNIT_N"].Recordset;
                    }
                    if (gTKeyObject.Components["COMP_UNIT_ANCIL_N"] != null)
                    {
                        rsAncuComponent = gTKeyObject.Components["COMP_UNIT_ANCIL_N"].Recordset;
                    }

                    if (rsCUComponent != null)
                    {
                        #region CU Attributes processing

                        rsCU = DataContext.OpenRecordset(sql, CursorTypeEnum.adOpenStatic, LockTypeEnum.adLockReadOnly,
                            (int)CommandTypeEnum.adCmdText,
                                    21,
                                    gTKeyObject.FID);


                        missedCidList = FindDeltaRecords(rsCUComponent, missedCidList, rsCU);
                        missedCidList = FindDeltaRecords(rsCU, missedCidList, rsCUComponent);

                        foreach (KeyValuePair<int, string> key in missedCidList)
                        {
                            if (key.Value != DataContext.ActiveJob)
                            {
                                return false;
                            }
                        }

                        #endregion CU Attributes processing
                    }

                    missedCidList = new Dictionary<int, string>();
                    rsCU = null;

                    if (rsAncuComponent != null)
                    {
                        #region Ancillary CU Attributes processing

                        rsCU = DataContext.OpenRecordset(sql, CursorTypeEnum.adOpenStatic, LockTypeEnum.adLockReadOnly,
                        (int)CommandTypeEnum.adCmdText,
                                22,
                                gTKeyObject.FID);


                        missedCidList = FindDeltaRecords(rsAncuComponent, missedCidList, rsCU);
                        missedCidList = FindDeltaRecords(rsCU, missedCidList, rsAncuComponent);


                        foreach (KeyValuePair<int, string> key in missedCidList)
                        {
                            if (key.Value != DataContext.ActiveJob)
                            {
                                return false;
                            }
                        }

                        #endregion Ancillary CU Attributes processing              
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return true;
        }

        private Dictionary<int, string> FindDeltaRecords(Recordset rsComponent,Dictionary<int, string> missedCidList, Recordset rsCU)
        {

            bool found = false;
            if (rsCU.RecordCount > 0)
            {
                rsCU.MoveFirst();
                while (!rsCU.EOF)
                {
                    if (rsComponent.RecordCount > 0)
                    {
                        rsComponent.MoveFirst();
                        while (!rsComponent.EOF)
                        {
                            if (Convert.ToInt32(rsComponent.Fields["g3e_cid"].Value) == Convert.ToInt32(rsCU.Fields["g3e_cid"].Value))
                            {
                                if(Convert.ToInt32(rsCU.Fields["g3e_cno"].Value) == 21)
                                {
                                    if(!string.IsNullOrEmpty(Convert.ToString(rsComponent.Fields["CU_C"].Value)) && 
                                        !string.IsNullOrEmpty(Convert.ToString(rsCU.Fields["CU_C"].Value)))
                                    {
                                        found = true;
                                        break;
                                    }                                   
                                }
                                else
                                {
                                    found = true;
                                    break;
                                }
                                                               
                            }
                            found = false;
                            rsComponent.MoveNext();
                        }
                    }

                    if (!found)
                    {
                        try
                        {
                            missedCidList.Add(Convert.ToInt32(rsCU.Fields["g3e_cid"].Value), Convert.ToString(rsCU.Fields["WR_ID"].Value));
                        }
                        catch
                        {

                        }
                    }
                    rsCU.MoveNext();
                }

            }


            return missedCidList;
        }
    }
}
