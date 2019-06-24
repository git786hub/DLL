// =====================================================================================================================================================================
//  File Name: WorkPoints.cs
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
using Intergraph.GTechnology.API;

namespace GTechnology.Oncor.CustomAPI
{
   public class WorkPoints
    {
        private Dictionary<int, Int16> m_WorkPointsKeyObjects = null;
        private IGTApplication m_oApp = null;
        private IGTJobManagementService m_jobManagement = null;


        public WorkPoints(Dictionary<int,Int16> p_WorkPoints)
        {
            m_WorkPointsKeyObjects = p_WorkPoints;
            m_oApp = GTClassFactory.Create<IGTApplication>();
            m_jobManagement = GTClassFactory.Create<IGTJobManagementService>();
            m_jobManagement.DataContext = m_oApp.DataContext;

        }

        //public void SynchronizeWorkPointForObsoleteCUs()
        //{
        //    IGTKeyObject oKeyObject = GTClassFactory.Create<IGTKeyObject>();

        //    try
        //    {
        //        foreach (KeyValuePair<int, Int16> item in m_WorkPointsKeyObjects)
        //        {
        //            oKeyObject = m_oApp.DataContext.OpenFeature(item.Value, item.Key);
        //            DeleteObsoleteCURecords(oKeyObject);
        //        }             
        //    }
        //    catch (Exception ex)
        //    {
        //        string exMsg = string.Format("Error occurred in {0} of {1}.{2}{3}", System.Reflection.MethodBase.GetCurrentMethod().Name, this.ToString(), Environment.NewLine, ex.Message);
        //        throw new Exception(exMsg);
        //    }
        //}

        public void SynchronizeWorkPointsForDiscardedFeatures()
        {
            IGTKeyObjects oKeyObjects = GTClassFactory.Create<IGTKeyObjects>();
            IGTKeyObject oKeyObject = GTClassFactory.Create<IGTKeyObject>();

            try
            {              
                foreach (KeyValuePair<int, Int16> item in m_WorkPointsKeyObjects)
                {
                    oKeyObject = m_oApp.DataContext.OpenFeature(item.Value, item.Key);

                    if (DiscardWorkPoint(oKeyObject))
                    {
                        oKeyObjects.Add(oKeyObject);
                    }
                   // DeleteObsoleteCURecords(oKeyObject);
                }
               
                if (oKeyObjects.Count > 0)
                {
                    ADODB.Recordset rsDiscardRecordSet = CreateInMemoryADORS(oKeyObjects);
                    m_oApp.SetStatusBarText(GTStatusPanelConstants.gtaspcMessage, "Synchronizing Workpoints...");
                    m_jobManagement.DiscardFeatureEdits(rsDiscardRecordSet);
                }
            }
            catch (Exception ex)
            {
                string exMsg = string.Format("Error occurred in {0} of {1}.{2}{3}", System.Reflection.MethodBase.GetCurrentMethod().Name, this.ToString(), Environment.NewLine, ex.Message);
                throw new Exception(exMsg);
            }            
        }

        private ADODB.Recordset CreateInMemoryADORS(IGTKeyObjects DDCKeyObjects)
        {
            ADODB.Recordset shapeADORecordset = new ADODB.Recordset();

            try
            {              
                shapeADORecordset.Fields.Append("G3E_FNO", ADODB.DataTypeEnum.adInteger, 0, ADODB.FieldAttributeEnum.adFldFixed, null);
                shapeADORecordset.Fields.Append("G3E_FID", ADODB.DataTypeEnum.adInteger, 0, ADODB.FieldAttributeEnum.adFldFixed, null);
                shapeADORecordset.Open(System.Type.Missing, System.Type.Missing, ADODB.CursorTypeEnum.adOpenUnspecified, ADODB.LockTypeEnum.adLockUnspecified, 0);

                foreach (IGTKeyObject keyObject in DDCKeyObjects)
                {
                    shapeADORecordset.AddNew(System.Type.Missing, System.Type.Missing);
                    shapeADORecordset.Fields["G3E_FNO"].Value = keyObject.FNO;
                    shapeADORecordset.Fields["G3E_FID"].Value = keyObject.FID;
                    shapeADORecordset.Update(System.Type.Missing, System.Type.Missing);
                }
            }
            catch (Exception ex)
            {
                string exMsg = string.Format("Error occurred in {0} of {1}.{2}{3}", System.Reflection.MethodBase.GetCurrentMethod().Name, this.ToString(), Environment.NewLine, ex.Message);
                throw new Exception(exMsg);
            }           

            return shapeADORecordset;
        }

        //public void DeleteObsoleteCURecords(IGTKeyObject p_keyObject)
        //{
        //    IGTComponent oComponent = p_keyObject.Components.GetComponent(19104);
        //    ADODB.Recordset rs = null;
        //    string sStrcutureIDWP = string.Empty;
        //    ADODB.Recordset rsCUComponent = null;
            
        //    try
        //    {
        //        rsCUComponent = p_keyObject.Components.GetComponent(19101).Recordset;
        //        sStrcutureIDWP = Convert.ToString(rsCUComponent.Fields["STRUCTURE_ID"].Value);

        //        if (oComponent != null)
        //        {
        //            if (oComponent.Recordset != null)
        //            {
        //                if (oComponent.Recordset.RecordCount > 0)
        //                {
        //                    oComponent.Recordset.MoveFirst();
        //                    while (oComponent.Recordset.EOF == false)
        //                    {
        //                        int iFID = Convert.ToInt32(oComponent.Recordset.Fields["ASSOC_FID"].Value);
        //                        int iCID = Convert.ToInt32(oComponent.Recordset.Fields["UNIT_CID"].Value);
        //                        string sCU = Convert.ToString(oComponent.Recordset.Fields["CU_C"].Value);

        //                        rs = m_oApp.DataContext.OpenRecordset("select count(*) from COMMON_N where g3e_fid =?", ADODB.CursorTypeEnum.adOpenDynamic, ADODB.LockTypeEnum.adLockOptimistic, (int)ADODB.CommandTypeEnum.adCmdText, iFID);

        //                        if (Convert.ToInt32(rs.Fields[0].Value).Equals(1)) //Feature exists but Strucure ID is different now
        //                        {
        //                            rs = m_oApp.DataContext.OpenRecordset("select count(*) from COMMON_N where g3e_fid =? and STRUCTURE_ID<> ?", ADODB.CursorTypeEnum.adOpenDynamic, ADODB.LockTypeEnum.adLockOptimistic, (int)ADODB.CommandTypeEnum.adCmdText, iFID, sStrcutureIDWP);

        //                            if (Convert.ToInt32(rs.Fields[0].Value).Equals(1))
        //                            {
        //                                oComponent.Recordset.Delete();
        //                            }
        //                            else  //Check if the relevent record that we are seeing exists on the feature or not. This will take care of discard for a feature on CID level
        //                            {
        //                                rs = m_oApp.DataContext.OpenRecordset("select count(*) from COMP_UNIT_N where g3e_fid =? and g3e_cid = ? and cu_c = ?", ADODB.CursorTypeEnum.adOpenDynamic, ADODB.LockTypeEnum.adLockOptimistic, (int)ADODB.CommandTypeEnum.adCmdText, iFID, iCID,sCU);

        //                                if (Convert.ToInt32(rs.Fields[0].Value).Equals(0))
        //                                {
        //                                    oComponent.Recordset.Delete();
        //                                }
        //                            }
        //                        }
        //                        else //Feature does not exist so delete the record corresponding to this associated FID
        //                        {
        //                            oComponent.Recordset.Delete();
        //                        }
                                
        //                        if (oComponent.Recordset.EOF == false)
        //                        {
        //                            oComponent.Recordset.MoveNext();
        //                        }
        //                    }
        //                }
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        string exMsg = string.Format("Error occurred in {0} of {1}.{2}{3}", System.Reflection.MethodBase.GetCurrentMethod().Name, this.ToString(), Environment.NewLine, ex.Message);
        //        throw new Exception(exMsg);
        //    }          
        //}
        //Will discard the WP only if the Strucure it points does not exist
        private bool DiscardWorkPoint(IGTKeyObject p_keyObject)
        {
            bool bReturn = true;
            IGTComponent oComponent = p_keyObject.Components.GetComponent(19104);
            ADODB.Recordset rs = null;

            try
            {
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
                                rs = m_oApp.DataContext.OpenRecordset("select count(*) from COMMON_N where g3e_fid =?", ADODB.CursorTypeEnum.adOpenDynamic, ADODB.LockTypeEnum.adLockOptimistic, (int)ADODB.CommandTypeEnum.adCmdText, iFID);

                                if (Convert.ToInt32(rs.Fields[0].Value).Equals(1))
                                {
                                    bReturn = false;
                                    break;
                                }
                                oComponent.Recordset.MoveNext();
                            }
                        }                      
                    }
                }
                if (bReturn) //Even if recordcount is 0 for WP, check the Strucutre ID does not exist before marking it for the discard
                {
                    oComponent = p_keyObject.Components.GetComponent(19101);
                    int iWPNumber = 0;

                    if (oComponent != null)
                    {
                        if (oComponent.Recordset != null)
                        {
                            oComponent.Recordset.MoveFirst();
                            iWPNumber = Convert.ToInt32(oComponent.Recordset.Fields["WP_NBR"].Value);

                            string sStrucureID = Convert.ToString(oComponent.Recordset.Fields["STRUCTURE_ID"].Value);
                            rs = m_oApp.DataContext.OpenRecordset("select count(*) from WORKPOINT_CU_N where WP_RELATED =? and g3e_fid in (select g3e_fid from workpoint_n where wr_nbr =?)", ADODB.CursorTypeEnum.adOpenDynamic, ADODB.LockTypeEnum.adLockOptimistic, (int)ADODB.CommandTypeEnum.adCmdText, iWPNumber, m_oApp.DataContext.ActiveJob);

                            if (!Convert.ToInt32(rs.Fields[0].Value).Equals(0))
                            {
                                bReturn = false;                               
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
           
            return bReturn;
        }
    }
}
