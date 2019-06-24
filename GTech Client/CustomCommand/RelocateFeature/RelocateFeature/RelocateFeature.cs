//----------------------------------------------------------------------------+
//        Class: RelocateFeature
//  Description: RelocateFeature class is used to relocate the feature once all validtaions are passed.
//----------------------------------------------------------------------------+
//     $$Author::         HCCI                                                $
//       $$Date::         06/11/2017 3.30 PM                                  $
//   $$Revision::         1                                                   $
//----------------------------------------------------------------------------+
//    $$History::         RelocateFeature.cs                                   $
//
//************************Version 1**************************
//User: Sithara    Date: 6/11/2017   Time : 3.30PM
//Created Trasferfeature.cs class
//----------------------------------------------------------------------------+
using System;
using Intergraph.GTechnology.API;
using ADODB;
using System.Windows.Forms;
using System.Collections.Generic;
using System.Collections;

namespace GTechnology.Oncor.CustomAPI
{
    public class RelocateFeature
    {
        public IGTCustomCommandHelper m_CustomCommandHelper = null;
        private IGTApplication m_gtApplication = null;
        private IGTKeyObject m_actKeyObj = null;
        private IGTKeyObjects m_oldOwnerKeyObjects = null;
        private IGTTransactionManager m_GTTransactionManager;
        private string m_strJStatus = null;
        private string m_strJtype = null;
        private bool m_moStatus = true;
        /// <summary>
        /// Trasferfeature class constructor
        /// </summary>
        /// <param name="CustomCommandHelper"></param>
        /// <param name="gt_Application"></param>
        /// <param name="AObject"></param>
        /// <param name="oldOwners"></param>
        /// <param name="TransactionManager"></param>
        /// <param name="strStatus"></param>
        /// <param name="strJobtype"></param>
        public RelocateFeature(IGTCustomCommandHelper CustomCommandHelper, IGTApplication gt_Application, IGTKeyObject AObject, IGTKeyObjects oldOwners,
            IGTTransactionManager TransactionManager, string strStatus, string strJobtype)
        {
            try
            {
                m_CustomCommandHelper = CustomCommandHelper;
                m_gtApplication = gt_Application;
                m_actKeyObj = AObject;
                m_oldOwnerKeyObjects = oldOwners;
                m_GTTransactionManager = TransactionManager;
                m_strJStatus = strStatus;
                m_strJtype = strJobtype;
                RegisterEvents();
                GTClassFactory.Create<IGTApplication>().SetStatusBarText(GTStatusPanelConstants.gtaspcMessage, "Identify new owner or press Esc key to exit the command.");
            }
            catch (Exception ex)
            {
                UnRegisterEvents();
                throw ex;
            }
        }


        /// <summary>
        /// Register click event
        /// </summary>
        private void RegisterEvents()
        {
            try
            {
                m_CustomCommandHelper.Click += new EventHandler<GTMouseEventArgs>(m_CustomCommandHelper_Click);
                m_CustomCommandHelper.KeyUp += M_CustomCommandHelper_KeyUp;
                m_CustomCommandHelper.MouseMove += M_CustomCommandHelper_MouseMove;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void M_CustomCommandHelper_MouseMove(object sender, GTMouseEventArgs e)
        {
            if (m_moStatus)
            {
                GTClassFactory.Create<IGTApplication>().SetStatusBarText(GTStatusPanelConstants.gtaspcMessage, "Identify new owner or press Esc key to exit the command.");
            }
        }

        private void M_CustomCommandHelper_KeyUp(object sender, GTKeyEventArgs e)
        {
            if (e.KeyCode == 27)
            {
                ExitCmd();
            }
        }


        /// <summary>
        /// On mouse click : Creates new feature on mouse click.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void m_CustomCommandHelper_Click(object sender, GTMouseEventArgs e)
        {
            int nFid = 0;
            short nFno = 0;
            String sql, oFeatureSt = string.Empty;

            IGTDDCKeyObject newOwnerDDC = null;
            IGTKeyObject ownerKey = GTClassFactory.Create<IGTKeyObject>();
            Recordset rsValidate = null;
            IGTComponent ownerCompo = null;
            string strSelectedFeature = "";
            string strSelectedOwner = "";

            try
            {
                GTClassFactory.Create<IGTApplication>().SetStatusBarText(GTStatusPanelConstants.gtaspcMessage, "Identify new owner or Esc to exit the command.");

                if (e.MapWindow.MousePointer.ToString().Contains("CrossHair") || e.MapWindow.MousePointer.ToString().Contains("NW"))
                {
                    LocateNodeFeature(e.WorldPoint, out nFid, out nFno, out newOwnerDDC);
                    if (nFid != 0 && nFno != 0)
                    {
                        sql = "SELECT G3E_USERNAME FROM G3E_FEATURES_OPTABLE WHERE G3E_FNO = ?";
                        rsValidate = m_gtApplication.DataContext.OpenRecordset(sql, ADODB.CursorTypeEnum.adOpenDynamic, ADODB.LockTypeEnum.adLockReadOnly,
                        (int)ADODB.CommandTypeEnum.adCmdText, m_actKeyObj.FNO);
                        if (rsValidate.RecordCount > 0)
                        {
                            rsValidate.MoveFirst();
                            strSelectedFeature = Convert.ToString(rsValidate.Fields[0].Value);
                        }

                        sql = "SELECT G3E_USERNAME FROM G3E_FEATURES_OPTABLE WHERE G3E_FNO =?";
                        rsValidate = m_gtApplication.DataContext.OpenRecordset(sql, ADODB.CursorTypeEnum.adOpenDynamic, ADODB.LockTypeEnum.adLockReadOnly,
                        (int)ADODB.CommandTypeEnum.adCmdText, nFno);
                        if (rsValidate.RecordCount > 0)
                        {
                            rsValidate.MoveFirst();
                            strSelectedOwner = Convert.ToString(rsValidate.Fields[0].Value);
                        }

                        rsValidate = null;

                        ownerKey = m_gtApplication.DataContext.OpenFeature(nFno, nFid);
                        sql = "SELECT * FROM G3E_OWNERSHIP_ELEC_OPTABLE WHERE G3E_SOURCEFNO=" + m_actKeyObj.FNO + " AND G3E_OWNERFNO=" + ownerKey.FNO + "";
                        rsValidate = m_gtApplication.DataContext.OpenRecordset(sql, ADODB.CursorTypeEnum.adOpenDynamic, ADODB.LockTypeEnum.adLockReadOnly,
                        (int)ADODB.CommandTypeEnum.adCmdText, null);

                        if (rsValidate.RecordCount > 0)
                        {
                            ownerCompo = ownerKey.Components.GetComponent(1);

                            if (ownerCompo.Recordset != null && ownerCompo.Recordset.RecordCount > 0)
                            {
                                ownerCompo.Recordset.MoveFirst();
                                if (ownerCompo.Recordset.Fields["FEATURE_STATE_C"] != null &&
                                    !string.IsNullOrEmpty(Convert.ToString(ownerCompo.Recordset.Fields["FEATURE_STATE_C"].Value)))
                                {
                                    oFeatureSt = ownerCompo.Recordset.Fields["FEATURE_STATE_C"].Value.ToString();
                                }

                                if (oFeatureSt != string.Empty)
                                {
                                    if (oFeatureSt == "INI" || oFeatureSt == "CLS" || oFeatureSt == "PPI" || oFeatureSt == "ABI")
                                    {
                                        m_moStatus = true;
                                        m_gtApplication.Application.SelectedObjects.Add(GTSelectModeConstants.gtsosmAllComponentsOfFeature, newOwnerDDC);
                                        Relocatefeature(ownerKey);
                                        ExitCmd();
                                    }
                                    else
                                    {
                                        GTClassFactory.Create<IGTApplication>().SetStatusBarText(GTStatusPanelConstants.gtaspcMessage,
                                            "" + strSelectedOwner + " is not a valid owner for " + strSelectedFeature + "; identify new owner.");

                                        m_moStatus = false;

                                    }
                                }
                            }
                        }
                        else
                        {
                            GTClassFactory.Create<IGTApplication>().SetStatusBarText(GTStatusPanelConstants.gtaspcMessage,
                                            "" + strSelectedOwner + " is not a valid owner for " + strSelectedFeature + "; identify new owner.");
                            m_moStatus = false;
                        }
                    }
                    else
                    {
                        GTClassFactory.Create<IGTApplication>().SetStatusBarText(GTStatusPanelConstants.gtaspcMessage,
                                            "" + strSelectedOwner + " is not a valid owner for " + strSelectedFeature + "; identify new owner.");
                        m_moStatus = false;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Relocate Feature", MessageBoxButtons.OK, MessageBoxIcon.Error,
                        MessageBoxDefaultButton.Button1);
                UnRegisterEvents();
                if (m_gtApplication.Application.Properties.Count > 0)
                    m_gtApplication.Application.Properties.Remove("RelocateFeatureCC");
                if (m_CustomCommandHelper != null)
                {
                    m_CustomCommandHelper.Complete();
                    m_CustomCommandHelper = null;
                }

                m_CustomCommandHelper = null;
                m_gtApplication.Application.EndWaitCursor();
                m_gtApplication.Application.RefreshWindows();
            }
            finally
            {
                if (rsValidate != null)
                {
                    if (rsValidate.State == 1)
                    {
                        rsValidate.Close();
                        rsValidate.ActiveConnection = null;
                    }
                    rsValidate = null;
                }
            }
        }

        /// <summary>
        /// Unregister mouse click event.
        /// </summary>
        private void UnRegisterEvents()
        {
            try
            {
                m_CustomCommandHelper.Click -= new EventHandler<GTMouseEventArgs>(m_CustomCommandHelper_Click);
                m_CustomCommandHelper.KeyUp -= M_CustomCommandHelper_KeyUp;
                m_CustomCommandHelper.MouseMove -= M_CustomCommandHelper_MouseMove;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// LocateNodeFeature method Identifies the feature fno and fid on mouse click.
        /// </summary>
        /// <param name="igtPoint"></param>
        /// <param name="NodeFid"></param>
        /// <param name="NodeFno"></param>
        /// <param name="selectedOwnerDDC"></param>
        private void LocateNodeFeature(IGTPoint igtPoint, out int NodeFid, out short NodeFno, out IGTDDCKeyObject selectedOwnerDDC)
        {
            NodeFid = 0;
            NodeFno = 0;
            selectedOwnerDDC = null;
            try
            {
                IGTDDCKeyObjects KeyObjects, LocatedObjects, NodeObjects;

                LocatedObjects = GTClassFactory.Create<IGTDDCKeyObjects>();

                KeyObjects = m_gtApplication.Application.ActiveMapWindow.LocateService.Locate(igtPoint, 10, -1, GTSelectionTypeConstants.gtmwstSelectAll);

                if (KeyObjects.Count > 0)
                {

                    foreach (IGTDDCKeyObject item in KeyObjects)
                    {
                        LocatedObjects.Add(item);
                    }


                    if (LocatedObjects != null && (LocatedObjects.Count == 1))
                    {
                        NodeFid = LocatedObjects[0].FID;
                        NodeFno = LocatedObjects[0].FNO;
                        selectedOwnerDDC = LocatedObjects[0];
                    }
                    else if (LocatedObjects.Count > 1)
                    {
                        NodeObjects = m_gtApplication.Application.ActiveMapWindow.LocateService.PickQuick(LocatedObjects, GTPickQuickTypeConstants.gtlspqtSelectSingle);
                        if (NodeObjects.Count == 1)
                        {
                            NodeFid = NodeObjects[0].FID;
                            NodeFno = NodeObjects[0].FNO;
                            selectedOwnerDDC = NodeObjects[0];
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// On selecting new owner of the feature RelocateFeature method creates new feature and establish new geometry with respect to the new owner, 
        /// preserving the direction and distance from the original owner.
        /// </summary>
        /// <param name="newOwnerKey"></param>
        private void Relocatefeature(IGTKeyObject newOwnerKey)
        {
            IGTKeyObject newFeature = null;
            Recordset rsValidate = null;
            Recordset rsattributesExclude = null;
            string sql = string.Empty;
            string fName = string.Empty;
            string cGeoType = string.Empty;
            short primarygSelectedCno = 0, primarygCnoNewOwner = 0, primarygCnoOldOwner = 0;
            IGTComponent primSeleOldCom = null;
            IGTComponent primSeleNCom = null;
            IGTComponent seleNewCom = null;
            IGTComponent primOldOwnerCom = null;
            IGTComponent primNewOwnerCom = null;
            IGTComponent seleCommonCom = null;

            int rCount = 0;
            IGTTextPointGeometry newFtTextGeo = null;
            IGTOrientedPointGeometry newFtOriGeo = null;
            IGTPolylineGeometry newFtPlineGeo = null;
            IGTCompositePolylineGeometry newFtCpline = null;
            double x = 0.0;
            double y = 0.0;
            double dist = 0.0;
            double angle = 0.0;

            bool bCorrctM = false;
            int ownerId = 0;

            try
            {
                sql = "select G3E_CNO,G3E_FIELD from G3E_ATTRIBUTEINFO_OPTABLE where G3E_EXCLUDEFROMREPLACE=1";
                rsattributesExclude = m_gtApplication.DataContext.OpenRecordset(sql, ADODB.CursorTypeEnum.adOpenDynamic, ADODB.LockTypeEnum.adLockReadOnly,
                       (int)ADODB.CommandTypeEnum.adCmdText, null);

                newOwnerKey.Components["COMMON_N"].Recordset.MoveFirst();
                ownerId = Convert.ToInt32(newOwnerKey.Components["COMMON_N"].Recordset.Fields["G3E_ID"].Value);
                String sStructureID = Convert.ToString(newOwnerKey.Components["COMMON_N"].Recordset.Fields["STRUCTURE_ID"].Value);

                #region Get primary graphic cno of selected feature

                sql = "select G3E_PRIMARYGEOGRAPHICCNO from G3E_FEATURES_OPTABLE where g3e_fno=?";
                rsValidate = m_gtApplication.DataContext.OpenRecordset(sql, ADODB.CursorTypeEnum.adOpenDynamic, ADODB.LockTypeEnum.adLockReadOnly,
                       (int)ADODB.CommandTypeEnum.adCmdText, m_actKeyObj.FNO);
                if (rsValidate.RecordCount > 0)
                {
                    rsValidate.MoveFirst();
                    primarygSelectedCno = Convert.ToInt16(rsValidate.Fields[0].Value);
                }

                #endregion

                #region  Get primary graphic cno of old owner feature .. oldOwnerKeyObjects


                foreach (IGTKeyObject oldOwKey in m_oldOwnerKeyObjects)
                {
                    sql = "select G3E_PRIMARYGEOGRAPHICCNO from G3E_FEATURES_OPTABLE where g3e_fno=?";
                    rsValidate = m_gtApplication.DataContext.OpenRecordset(sql, ADODB.CursorTypeEnum.adOpenDynamic, ADODB.LockTypeEnum.adLockReadOnly,
                           (int)ADODB.CommandTypeEnum.adCmdText, oldOwKey.FNO);
                    if (rsValidate.RecordCount > 0)
                    {
                        rsValidate.MoveFirst();
                        primarygCnoOldOwner = Convert.ToInt16(rsValidate.Fields[0].Value);
                    }

                    primOldOwnerCom = oldOwKey.Components.GetComponent(primarygCnoOldOwner);
                    break;
                }


                #endregion

                #region  Get primary graphic cno of new owner feature

                sql = "select G3E_PRIMARYGEOGRAPHICCNO from G3E_FEATURES_OPTABLE where g3e_fno=?";
                rsValidate = m_gtApplication.DataContext.OpenRecordset(sql, ADODB.CursorTypeEnum.adOpenDynamic, ADODB.LockTypeEnum.adLockReadOnly,
                       (int)ADODB.CommandTypeEnum.adCmdText, newOwnerKey.FNO);
                if (rsValidate.RecordCount > 0)
                {
                    rsValidate.MoveFirst();
                    primarygCnoNewOwner = Convert.ToInt16(rsValidate.Fields[0].Value);
                }

                primNewOwnerCom = newOwnerKey.Components.GetComponent(primarygCnoNewOwner);
                #endregion

                // Create new feature from selected feature

                // Start Transaction
                m_GTTransactionManager.Begin("Relocate Feature FNO = " + m_actKeyObj.FNO.ToString() + " FID = " + m_actKeyObj.FID.ToString());

                newFeature = m_gtApplication.DataContext.NewFeature(m_actKeyObj.FNO);


                #region Copy Primary graphic component wrt new owner

                primSeleOldCom = m_actKeyObj.Components.GetComponent(primarygSelectedCno);
                primSeleNCom = newFeature.Components.GetComponent(primarygSelectedCno);

                if (primSeleOldCom != null && primSeleOldCom.Recordset != null && primSeleOldCom.Recordset.RecordCount > 0)
                {
                    primSeleOldCom.Recordset.MoveFirst();
                    rCount = 0;
                    cGeoType = primSeleOldCom.Geometry.Type;
                    while (!primSeleOldCom.Recordset.EOF)
                    {
                        if (rCount > 0 || primSeleNCom.Recordset.RecordCount <= 0)
                        {
                            primSeleNCom.Recordset.AddNew();
                            primSeleNCom.Recordset.Fields["G3E_FNO"].Value = newFeature.FNO;
                            primSeleNCom.Recordset.Fields["G3E_FID"].Value = newFeature.FID;
                            primSeleNCom.Recordset.Fields["G3E_CNO"].Value = primSeleOldCom.CNO;
                            // primSeleNCom.Recordset.Fields["G3E_CID"].Value = primSeleOldCom.Recordset.Fields["G3E_CID"].Value;
                        }
                        else
                        {
                            primSeleNCom.Recordset.MoveFirst();
                        }

                        for (int i = 0; i < primSeleOldCom.Recordset.Fields.Count; i++)
                        {
                            fName = primSeleOldCom.Recordset.Fields[i].Name;
                            rsattributesExclude.Filter = "G3E_CNO = " + primarygSelectedCno + " AND G3E_FIELD = '" + fName + "'";
                            if (rsattributesExclude.RecordCount <= 0 && ((fName != "G3E_FNO" && fName != "G3E_FID" && fName != "G3E_ID" && (fName.Substring(0, 3) != "G3E")
                                && (fName.Substring(0, 3) != "LTT")) || fName == "G3E_ALIGNMENT"))
                            {
                                if (fName == "ACTIVITY_C")
                                {
                                    primSeleNCom.Recordset.Fields[i].Value = "TI";
                                }
                                else
                                {
                                    primSeleNCom.Recordset.Fields[i].Value = primSeleOldCom.Recordset.Fields[i].Value;
                                }
                                primSeleNCom.Recordset.Update();

                            }

                        }


                        newFtTextGeo = null;
                        newFtOriGeo = null;
                        newFtPlineGeo = null;
                        newFtCpline = null;
                        x = 0;
                        y = 0;
                        dist = 0;
                        angle = 0;

                        UpdateComponentGeo(cGeoType, primSeleNCom, primOldOwnerCom, primNewOwnerCom, ref newFtTextGeo, ref newFtOriGeo, ref newFtPlineGeo, ref newFtCpline,
                                   ref x, ref y, ref dist, ref angle, primSeleOldCom, primSeleOldCom, primSeleNCom);

                        //primSeleNCom.Geometry = primSeleOldCom.Geometry;

                        primSeleNCom.Recordset.Update();
                        rCount = rCount + 1;
                        primSeleOldCom.Recordset.MoveNext();
                    }

                }

                #endregion

                #region Copy remaining components

                foreach (IGTComponent oldCompo in m_actKeyObj.Components)
                {
                    if (oldCompo.Name.ToUpper() != "CONTAIN_N" &&
                        oldCompo.Name.ToUpper() != "STREETCONN_CL_N" &&
                        oldCompo.Name.ToUpper() != "GC_NR_CONNECT" &&
                        oldCompo.Name.ToUpper() != "GC_NE_CONNECT" &&
                        oldCompo.Name.ToUpper() != "GC_CONTAIN" &&
                        oldCompo.Name.ToUpper() != "GC_SPLICE_CONNECT" &&
                        oldCompo.Name.ToUpper() != "VW_COUNT_CONNECT_ONE" &&
                        oldCompo.Name.ToUpper() != "VW_COUNT_CONNECT")
                    {
                        rCount = 0;

                        newFtTextGeo = null;
                        newFtOriGeo = null;
                        newFtPlineGeo = null;
                        newFtCpline = null;
                        x = 0;
                        y = 0;
                        dist = 0;
                        angle = 0;

                        if (oldCompo != null && oldCompo.Recordset != null && oldCompo.Recordset.RecordCount > 0 && oldCompo.CNO != primarygSelectedCno)
                        {
                            seleNewCom = newFeature.Components.GetComponent(oldCompo.CNO);

                            if (IsGraphicGeoComp(oldCompo.CNO))
                            {
                                if (oldCompo.Geometry != null)
                                {
                                    cGeoType = oldCompo.Geometry.Type;
                                }
                                else
                                {
                                    cGeoType = null;
                                }

                                #region                     
                                oldCompo.Recordset.MoveFirst();
                                rCount = 0;
                                while (!oldCompo.Recordset.EOF)
                                {
                                    if (rCount > 0 || ((seleNewCom.Recordset.BOF == true) || (seleNewCom.Recordset.EOF == true)))
                                    {
                                        seleNewCom.Recordset.AddNew();
                                        seleNewCom.Recordset.Fields["G3E_FID"].Value = newFeature.FID;
                                        seleNewCom.Recordset.Fields["G3E_FNO"].Value = newFeature.FNO;
                                        seleNewCom.Recordset.Fields["G3E_CNO"].Value = oldCompo.CNO;
                                        //seleNewCom.Recordset.Fields["G3E_CID"].Value = oldCompo.Recordset.Fields["G3E_CID"].Value;
                                    }



                                    for (int i = 0; i < oldCompo.Recordset.Fields.Count; i++)
                                    {
                                        fName = oldCompo.Recordset.Fields[i].Name;
                                        rsattributesExclude.Filter = "G3E_CNO = " + oldCompo.CNO + " AND G3E_FIELD = '" + fName + "'";
                                        if (rsattributesExclude.RecordCount <= 0 && ((fName != "G3E_FNO" && fName != "G3E_FID" && fName != "G3E_ID" && (fName.Substring(0, 3) != "G3E")
                                    && (fName.Substring(0, 3) != "LTT")) || fName == "G3E_ALIGNMENT"))
                                        {
                                            if (fName == "ACTIVITY_C")
                                            {
                                                seleNewCom.Recordset.Fields[i].Value = "TI";
                                            }
                                            else
                                            {
                                                seleNewCom.Recordset.Fields[i].Value = oldCompo.Recordset.Fields[i].Value;
                                            }
                                        }
                                    }

                                    if (cGeoType != null)
                                    {
                                        UpdateComponentGeo(cGeoType, seleNewCom, primOldOwnerCom, primNewOwnerCom, ref newFtTextGeo, ref newFtOriGeo, ref newFtPlineGeo, ref newFtCpline,
                                            ref x, ref y, ref dist, ref angle, oldCompo, primSeleOldCom, primSeleNCom);
                                    }

                                    seleNewCom.Recordset.Update();
                                    rCount = rCount + 1;
                                    oldCompo.Recordset.MoveNext();
                                }
                                #endregion

                            }
                            else if (IsNonGraphicGeoComp(oldCompo.CNO))
                            {
                                #region 
                                Recordset rtemp = oldCompo.Recordset;
                                rtemp.MoveFirst();
                                rCount = 1;
                                /*if (rtemp.RecordCount == 1)
								{
                                    /*
									if (((seleNewCom.Recordset.BOF == true) || (seleNewCom.Recordset.EOF == true)))
									{
                                        seleNewCom.Recordset.AddNew();
                                        seleNewCom.Recordset.Fields["G3E_FID"].Value = newFeature.FID;
                                        seleNewCom.Recordset.Fields["G3E_FNO"].Value = newFeature.FNO;
										seleNewCom.Recordset.Fields["G3E_CNO"].Value = oldCompo.CNO;
										//seleNewCom.Recordset.Fields["G3E_CID"].Value = rtemp.Fields["G3E_CID"].Value;
									}

									for (int i = 0; i < rtemp.Fields.Count; i++)
									{
										fName = rtemp.Fields[i].Name;
										rsattributesExclude.Filter = "G3E_CNO = " + oldCompo.CNO + " AND G3E_FIELD = '" + fName + "'";

										if (rsattributesExclude.RecordCount <= 0 && ((fName != "G3E_FNO" && fName != "G3E_FID" && fName != "G3E_ID" && (fName.Substring(0, 3) != "G3E")
									&& (fName.Substring(0, 3) != "LTT")) || fName == "G3E_ALIGNMENT") && fName != "OWNER2_ID")
										{
											if (fName != "FEATURE_STATE_C")
											{
												if (fName == "OWNER1_ID")
												{
													seleNewCom.Recordset.Fields[i].Value = ownerId;
												}
												else
												{
                                                    if (fName == "ACTIVITY_C")
                                                    {
                                                        seleNewCom.Recordset.Fields[i].Value = "TI";
                                                    }
                                                    else
                                                    {
                                                        seleNewCom.Recordset.Fields[i].Value = rtemp.Fields[i].Value;
                                                    }
												}
											}
											else
											{
												if (m_strJStatus.ToUpper() == "ASBUILT")
												{
													seleNewCom.Recordset.Fields[i].Value = "ABI";
												}
												else if (m_strJStatus.ToUpper() == "CONSTRUCTIONCOMPLETE")
												{
													seleNewCom.Recordset.Fields[i].Value = "INI";
												}
												else
												{
													seleNewCom.Recordset.Fields[i].Value = "PPI";
												}
											}
										}
									}

									seleNewCom.Recordset.Update();

									rCount = rCount + 1;
								}
								else
								{ */
                                if (seleNewCom.Recordset != null && seleNewCom.Recordset.RecordCount > 0)
                                {
                                    seleNewCom.Recordset.MoveFirst();

                                    while (!rtemp.EOF)
                                    {

                                        if ((seleNewCom.Recordset.RecordCount == 0) || (seleNewCom.Recordset.EOF && rCount > 1))
                                        {
                                            seleNewCom.Recordset.AddNew();
                                            seleNewCom.Recordset.Fields["G3E_FID"].Value = newFeature.FID;
                                            seleNewCom.Recordset.Fields["G3E_FNO"].Value = newFeature.FNO;
                                            seleNewCom.Recordset.Fields["G3E_CNO"].Value = oldCompo.CNO;
                                            //seleNewCom.Recordset.Fields["G3E_CID"].Value = rtemp.Fields["G3E_CID"].Value;
                                        }


                                        for (int i = 0; i < rtemp.Fields.Count; i++)
                                        {
                                            fName = rtemp.Fields[i].Name;
                                            rsattributesExclude.Filter = "G3E_CNO = " + oldCompo.CNO + " AND G3E_FIELD = '" + fName + "'";

                                            if (rsattributesExclude.RecordCount <= 0 && ((fName != "G3E_FNO" && fName != "G3E_FID" && fName != "G3E_ID" && (fName.Substring(0, 3) != "G3E")
                                        && (fName.Substring(0, 3) != "LTT")) || fName == "G3E_ALIGNMENT") && fName != "OWNER2_ID")
                                            {
                                                if (fName != "FEATURE_STATE_C")
                                                {
                                                    if (fName == "OWNER1_ID")
                                                    {
                                                        seleNewCom.Recordset.Fields[i].Value = ownerId;
                                                    }
                                                    else
                                                    {
                                                        if (fName == "ACTIVITY_C")
                                                        {
                                                            seleNewCom.Recordset.Fields[i].Value = "TI";
                                                        }
                                                        else
                                                        {
                                                            seleNewCom.Recordset.Fields[i].Value = rtemp.Fields[i].Value;
                                                        }
                                                    }
                                                }
                                                else
                                                {
                                                    if (m_strJStatus.ToUpper() == "ASBUILT")
                                                    {
                                                        seleNewCom.Recordset.Fields[i].Value = "ABI";
                                                    }
                                                    else if (m_strJStatus.ToUpper() == "CONSTRUCTIONCOMPLETE")
                                                    {
                                                        seleNewCom.Recordset.Fields[i].Value = "INI";
                                                    }
                                                    else
                                                    {
                                                        seleNewCom.Recordset.Fields[i].Value = "PPI";
                                                    }
                                                }
                                            }
                                        }

                                        seleNewCom.Recordset.Update();
                                        seleNewCom.Recordset.MoveNext();
                                        rCount = rCount + 1;
                                        rtemp.MoveNext();
                                    }


                                }
                                #endregion
                            }
                        }
                    }
                }

                #endregion

                m_actKeyObj = m_gtApplication.DataContext.OpenFeature(m_actKeyObj.FNO, m_actKeyObj.FID);


                #region Modify the existing feature


                seleCommonCom = m_actKeyObj.Components["COMMON_N"];
                seleCommonCom.Recordset.MoveFirst();

                if (m_strJStatus.ToUpper() == "ASBUILT")
                {
                    seleCommonCom.Recordset.Fields["FEATURE_STATE_C"].Value = "ABR";
                }
                else if (m_strJStatus.ToUpper() == "CONSTRUCTIONCOMPLETE")
                {
                    seleCommonCom.Recordset.Fields["FEATURE_STATE_C"].Value = "OSR";
                }
                else
                {
                    seleCommonCom.Recordset.Fields["FEATURE_STATE_C"].Value = "PPR";
                }

                seleCommonCom.Recordset.Update();


                for (int i = 0; i < m_gtApplication.Application.Properties.Keys.Count; i++)
                {
                    if (string.Equals("CorrectionsMode", Convert.ToString(m_gtApplication.Application.Properties.Keys.Get(i))))
                    {
                        bCorrctM = true;
                    }
                }

                seleCommonCom = null;
                seleCommonCom = m_actKeyObj.Components["COMP_UNIT_N"];

                seleCommonCom.Recordset.MoveFirst();

                while (!seleCommonCom.Recordset.EOF && !seleCommonCom.Recordset.BOF)
                {
                    if (m_strJtype.ToUpper() == "WR-MAPCOR" || bCorrctM == true)
                    {
                        seleCommonCom.Recordset.Fields["ACTIVITY_C"].Value = "TC";
                    }
                    else
                    {
                        seleCommonCom.Recordset.Fields["ACTIVITY_C"].Value = "T";
                    }

                    seleCommonCom.Recordset.Update(Type.Missing, Type.Missing);
                    seleCommonCom.Recordset.MoveNext();
                }

                seleCommonCom = null;
                seleCommonCom = m_actKeyObj.Components["COMP_UNIT_ANCIL_N"];

                if (seleCommonCom != null && seleCommonCom.Recordset != null && seleCommonCom.Recordset.RecordCount > 0)
                {
                    seleCommonCom.Recordset.MoveFirst();

                    while (!seleCommonCom.Recordset.EOF && !seleCommonCom.Recordset.BOF)
                    {
                        if (m_strJtype.ToUpper() == "WR-MAPCOR" || bCorrctM == true)
                        {
                            if (Convert.ToString(seleCommonCom.Recordset.Fields["RETIREMENT_C"].Value).Equals("1") || Convert.ToString(seleCommonCom.Recordset.Fields["RETIREMENT_C"].Value).Equals("2"))
                            {
                                seleCommonCom.Recordset.Fields["ACTIVITY_C"].Value = "TC";
                            }
                        }
                        else
                        {
                            if (Convert.ToString(seleCommonCom.Recordset.Fields["RETIREMENT_C"].Value).Equals("1") || Convert.ToString(seleCommonCom.Recordset.Fields["RETIREMENT_C"].Value).Equals("2"))
                            {
                                seleCommonCom.Recordset.Fields["ACTIVITY_C"].Value = "T";
                            }
                        }

                        seleCommonCom.Recordset.Update(Type.Missing, Type.Missing);
                        seleCommonCom.Recordset.MoveNext();
                    }
                }
                if (bCorrctM)
                {
                    m_gtApplication.Application.Properties.Remove("CorrectionsMode");

                    string mapCaption = m_gtApplication.Application.ActiveMapWindow.Caption.Replace("CORRECTIONS MODE - ", "");
                    m_gtApplication.Application.ActiveMapWindow.Caption = mapCaption;
                }

                #endregion

                #region copy new owner connectivity features to new relocated feature
                //ALM-2050 Changes
                EstablishConnectivitywithNewOwner(newFeature, newOwnerKey);
                newFeature.Components.GetComponent(1).Recordset.MoveFirst();
                newFeature.Components.GetComponent(1).Recordset.Fields["STRUCTURE_ID"].Value = sStructureID;
                #endregion
                m_GTTransactionManager.Commit();
                m_GTTransactionManager.RefreshDatabaseChanges();

                //Shubham - This is the place where we need to put the procesisg for the WorkPoints
                SynchronizeWP(m_actKeyObj);
                SynchronizeWP(newFeature);
            }

            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (rsValidate != null)
                {
                    if (rsValidate.State == 1)
                    {
                        rsValidate.Close();
                        rsValidate.ActiveConnection = null;
                    }
                    rsValidate = null;
                }
            }
        }

        private void EstablishConnectivitywithNewOwner(IGTKeyObject newgTKeyObj, IGTKeyObject relatedKO)
        {
            IGTRelationshipService relationshipService = null;
            IGTKeyObjects m_relatedFeatures = null;

            try
            {
                relationshipService = GTClassFactory.Create<IGTRelationshipService>();
                relationshipService.DataContext = m_gtApplication.DataContext;
                relationshipService.ActiveFeature = relatedKO;
                try
                {
                    m_relatedFeatures = relationshipService.GetRelatedFeatures(2);
                }
                catch
                {

                }

                if (m_relatedFeatures != null)
                {
                    foreach (IGTKeyObject feature in m_relatedFeatures)
                    {
                        if (feature.FID != newgTKeyObj.FID)
                        {
                            relationshipService.ActiveFeature = newgTKeyObj;
                            if (relationshipService.AllowSilentEstablish(feature))
                            {
                                try
                                {
                                    relationshipService.SilentEstablish(14, feature);
                                }
                                catch
                                {

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

            relationshipService.Dispose();

        }
        private void SynchronizeWP(IGTKeyObject p_KeyObjectFeature)
        {
            try
            {
                m_GTTransactionManager.Begin("Process Synchronize WorkPoints for Relocate Feature =" + p_KeyObjectFeature.FNO.ToString() + " FID = " + p_KeyObjectFeature.FID.ToString());

                IGTKeyObject refreshedFeature = m_gtApplication.DataContext.OpenFeature(p_KeyObjectFeature.FNO, p_KeyObjectFeature.FID);

                IGTComponents CUComponentsOldFeature = GTClassFactory.Create<IGTComponents>();

                foreach (IGTComponent item in refreshedFeature.Components)
                {
                    if (item.CNO == 21 || item.CNO == 22)
                    {
                        CUComponentsOldFeature.Add(item);
                    }
                }
                WorkPointOperations obj = new WorkPointOperations(CUComponentsOldFeature, refreshedFeature, m_gtApplication.DataContext);
                obj.DoWorkpointOperations();

                m_GTTransactionManager.Commit();
                m_GTTransactionManager.RefreshDatabaseChanges();
            }
            catch (Exception ex)
            {
                if (m_GTTransactionManager.TransactionInProgress)
                {
                    m_GTTransactionManager.Rollback();
                }
                throw ex;
            }
        }

        /// <summary>
        /// UpdateComponentGeo method updates the Geometry field of the new feature component.
        /// </summary>
        /// <param name="cGeoType"></param>
        /// <param name="seleNewCom"></param>
        /// <param name="primOldOwnerCom"></param>
        /// <param name="primNewOwnerCom"></param>
        /// <param name="newFtTextGeo"></param>
        /// <param name="newFtOriGeo"></param>
        /// <param name="newFtPlineGeo"></param>
        /// <param name="newFtCpline"></param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="dist"></param>
        /// <param name="angle"></param>
        /// <param name="oldCompo"></param>
        private void UpdateComponentGeo(string cGeoType, IGTComponent seleNewCom, IGTComponent primOldOwnerCom, IGTComponent primNewOwnerCom, ref IGTTextPointGeometry newFtTextGeo,
            ref IGTOrientedPointGeometry newFtOriGeo, ref IGTPolylineGeometry newFtPlineGeo, ref IGTCompositePolylineGeometry newFtCpline, ref double x, ref double y, ref double dist,
            ref double angle, IGTComponent oldCompo, IGTComponent primarySelectedComponent, IGTComponent primaryNewComponent)
        {
            IGTPoint newPt = GTClassFactory.Create<IGTPoint>();
            IGTVector tmpVector = GTClassFactory.Create<IGTVector>();
            IGTMatrix tmpMatx = GTClassFactory.Create<IGTMatrix>();
            bool lineOwners = false;

            if (primNewOwnerCom.Geometry.Type != GTGeometryTypeConstants.gtgtOrientedPointGeometry &&
               primNewOwnerCom.Geometry.Type != GTGeometryTypeConstants.gtgtPointGeometry &&
               primNewOwnerCom.Geometry.Type != GTGeometryTypeConstants.gtgtTextPointGeometry &&
               primOldOwnerCom.Geometry.Type != GTGeometryTypeConstants.gtgtOrientedPointGeometry &&
               primOldOwnerCom.Geometry.Type != GTGeometryTypeConstants.gtgtPointGeometry &&
               primOldOwnerCom.Geometry.Type != GTGeometryTypeConstants.gtgtTextPointGeometry)
            {
                lineOwners = true;
            }

            if (cGeoType == "TextPointGeometry")
            {
                newFtTextGeo = GTClassFactory.Create<IGTTextPointGeometry>();


                dist = findistance(primOldOwnerCom.Geometry.FirstPoint, oldCompo.Geometry.FirstPoint);
                angle = findangle(primOldOwnerCom.Geometry.FirstPoint, oldCompo.Geometry.FirstPoint);


                newPt.X = primNewOwnerCom.Geometry.FirstPoint.X + dist * Math.Cos(angle);
                newPt.Y = primNewOwnerCom.Geometry.FirstPoint.Y + dist * Math.Sin(angle);

                newFtTextGeo.Origin = newPt;
                newFtTextGeo.Normal = ((IGTOrientedPointGeometry)oldCompo.Geometry).Orientation;


                if (lineOwners)
                {
                    if (primNewOwnerCom.Geometry.Type == GTGeometryTypeConstants.gtgtCompositePolylineGeometry)
                    {
                        newFtTextGeo.Origin = GetPointAtLength((IGTCompositePolylineGeometry)primNewOwnerCom.Geometry, dist);
                    }
                    else if (primNewOwnerCom.Geometry.Type == GTGeometryTypeConstants.gtgtPolylineGeometry)
                    {
                        newFtTextGeo.Origin = GetPointAtLength((IGTPolylineGeometry)primNewOwnerCom.Geometry, dist);
                    }
                }

                seleNewCom.Geometry = newFtTextGeo;



            }
            else if (cGeoType == "OrientedPointGeometry")
            {
                newFtOriGeo = GTClassFactory.Create<IGTOrientedPointGeometry>();

                dist = findistance(primOldOwnerCom.Geometry.FirstPoint, oldCompo.Geometry.FirstPoint);
                angle = findangle(primOldOwnerCom.Geometry.FirstPoint, oldCompo.Geometry.FirstPoint);


                newPt.X = primNewOwnerCom.Geometry.FirstPoint.X + dist * Math.Cos(angle);
                newPt.Y = primNewOwnerCom.Geometry.FirstPoint.Y + dist * Math.Sin(angle);


                newFtOriGeo.Origin = newPt;
                newFtOriGeo.Orientation = ((IGTOrientedPointGeometry)oldCompo.Geometry).Orientation;


                if (lineOwners)
                {
                    if (primNewOwnerCom.Geometry.Type == GTGeometryTypeConstants.gtgtCompositePolylineGeometry)
                    {
                        newFtOriGeo.Origin = GetPointAtLength((IGTCompositePolylineGeometry)primNewOwnerCom.Geometry, dist);
                    }
                    else if (primNewOwnerCom.Geometry.Type == GTGeometryTypeConstants.gtgtPolylineGeometry)
                    {
                        newFtOriGeo.Origin = GetPointAtLength((IGTPolylineGeometry)primNewOwnerCom.Geometry, dist);
                    }
                }
                seleNewCom.Geometry = newFtOriGeo;


            }
            else if (cGeoType == "PolylineGeometry")
            {
                newFtPlineGeo = GTClassFactory.Create<IGTPolylineGeometry>();


                for (int k = 0; k <= oldCompo.Geometry.KeypointCount - 1; k++)
                {
                    dist = findistance(primarySelectedComponent.Geometry.FirstPoint, oldCompo.Geometry.GetKeypointPosition(k));
                    angle = findangle(primarySelectedComponent.Geometry.FirstPoint, oldCompo.Geometry.GetKeypointPosition(k));

                    newPt.X = primaryNewComponent.Geometry.FirstPoint.X + dist * Math.Cos(angle);
                    newPt.Y = primaryNewComponent.Geometry.FirstPoint.Y + dist * Math.Sin(angle);

                    newFtPlineGeo.Points.Add(newPt);
                }


                seleNewCom.Geometry = newFtPlineGeo;


            }
            else if (cGeoType == "CompositePolylineGeometry")
            {
                newFtCpline = GTClassFactory.Create<IGTCompositePolylineGeometry>();
                seleNewCom.Geometry = newFtCpline;
                IGTPolylineGeometry polylineGeometrytemp = GTClassFactory.Create<IGTPolylineGeometry>();


                foreach (IGTGeometry subgeom in (IGTCompositePolylineGeometry)oldCompo.Geometry)
                {

                    if (subgeom.Type == GTGeometryTypeConstants.gtgtPointGeometry || subgeom.Type == GTGeometryTypeConstants.gtgtOrientedPointGeometry)
                    {

                        dist = findistance(primarySelectedComponent.Geometry.FirstPoint, subgeom.FirstPoint);
                        angle = findangle(primarySelectedComponent.Geometry.FirstPoint, subgeom.FirstPoint);

                        newPt.X = primaryNewComponent.Geometry.FirstPoint.X + dist * Math.Cos(angle);
                        newPt.Y = primaryNewComponent.Geometry.FirstPoint.Y + dist * Math.Sin(angle);



                        polylineGeometrytemp.Points.Add(newPt);


                    }
                    else if (subgeom.Type == GTGeometryTypeConstants.gtgtLineGeometry)
                    {
                        dist = findistance(primarySelectedComponent.Geometry.FirstPoint, subgeom.FirstPoint);
                        angle = findangle(primarySelectedComponent.Geometry.FirstPoint, subgeom.FirstPoint);

                        newPt.X = primaryNewComponent.Geometry.FirstPoint.X + dist * Math.Cos(angle);
                        newPt.Y = primaryNewComponent.Geometry.FirstPoint.Y + dist * Math.Sin(angle);

                        polylineGeometrytemp.Points.Add(newPt);

                        dist = findistance(primarySelectedComponent.Geometry.LastPoint, subgeom.LastPoint);
                        angle = findangle(primarySelectedComponent.Geometry.LastPoint, subgeom.LastPoint);

                        newPt.X = primaryNewComponent.Geometry.LastPoint.X + dist * Math.Cos(angle);
                        newPt.Y = primaryNewComponent.Geometry.LastPoint.Y + dist * Math.Sin(angle);


                        polylineGeometrytemp.Points.Add(newPt);


                    }
                    else if (subgeom.Type == GTGeometryTypeConstants.gtgtPolylineGeometry)
                    {

                        for (int k = 0; k < subgeom.KeypointCount; k++)
                        {
                            dist = findistance(primarySelectedComponent.Geometry.FirstPoint, oldCompo.Geometry.GetKeypointPosition(k));
                            angle = findangle(primarySelectedComponent.Geometry.FirstPoint, oldCompo.Geometry.GetKeypointPosition(k));

                            newPt.X = primaryNewComponent.Geometry.FirstPoint.X + dist * Math.Cos(angle);
                            newPt.Y = primaryNewComponent.Geometry.FirstPoint.Y + dist * Math.Sin(angle);



                            polylineGeometrytemp.Points.Add(newPt);

                        }
                    }

                }
                newFtCpline.Add((IGTGeometry)polylineGeometrytemp);

                seleNewCom.Geometry = newFtCpline;


            }
        }

        /// <summary>
        /// isGraphicGeoComp method returns input cno is graphic component or not.
        /// </summary>
        /// <param name="cno"></param>
        /// <returns></returns>
        private bool IsGraphicGeoComp(short cno)
        {
            ADODB.Recordset tmprs = null;
            bool isGraphicGeoComp = false;
            try
            {
                tmprs = m_gtApplication.DataContext.MetadataRecordset("G3E_COMPONENTINFO_OPTABLE");
                tmprs.Filter = "G3E_CNO = " + cno;

                if ((Convert.ToInt16(tmprs.Fields["g3e_type"].Value) != 1) && Convert.ToInt16(tmprs.Fields["g3e_detail"].Value) == 0)
                {
                    isGraphicGeoComp = true;
                }
                tmprs.Close();
                tmprs = null;

            }
            catch (Exception ex)
            {
                throw ex;
            }
            return isGraphicGeoComp;
        }

        /// <summary>
        /// isGraphicGeoComp method returns input cno is non graphic component or not.
        /// </summary>
        /// <param name="cno"></param>
        /// <returns></returns>
        private bool IsNonGraphicGeoComp(short cno)
        {
            ADODB.Recordset tmprs = null;
            bool isNonGraphicGeoComp = false;
            try
            {
                tmprs = m_gtApplication.DataContext.MetadataRecordset("G3E_COMPONENTINFO_OPTABLE");
                tmprs.Filter = "G3E_CNO = " + cno;

                if ((Convert.ToInt16(tmprs.Fields["g3e_type"].Value) == 1) && Convert.ToInt16(tmprs.Fields["g3e_detail"].Value) == 0)
                {
                    isNonGraphicGeoComp = true;
                }
                tmprs.Close();
                tmprs = null;

            }
            catch (Exception ex)
            {
                throw ex;
            }
            return isNonGraphicGeoComp;
        }

        /// <summary>
        /// Returns distance between two geometries
        /// </summary>
        /// <param name="oldOwnerGeo"></param>
        /// <param name="oldselectedGeo"></param>
        /// <returns></returns>
        private double findistance(IGTPoint oldOwnerGeo, IGTPoint oldselectedGeo)
        {
            double dis = 0.0;
            double x1 = 0.0, x2 = 0.0, y1 = 0.0, y2 = 0.0;
            try
            {
                x1 = oldOwnerGeo.X;
                y1 = oldOwnerGeo.Y;

                x2 = oldselectedGeo.X;
                y2 = oldselectedGeo.Y;

                dis = Math.Sqrt(((x2 - x1) * (x2 - x1)) + ((y2 - y1) * (y2 - y1)));

            }
            catch (Exception ex)
            {
                throw ex;
            }

            return dis;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="p1"></param>
        /// <param name="p2"></param>
        /// <returns></returns>
        private double findangle(IGTPoint p1, IGTPoint p2)
        {
            double nSlope = 0.0;
            double nOrientedAngle = 0.0;
            double cPI = 3.14159;
            try
            {
                if (p2.Y - p1.Y == 0 || p2.X - p1.X == 0)
                    nSlope = 0;
                else
                    nSlope = (p2.Y - p1.Y) / (p2.X - p1.X);

                nOrientedAngle = Math.Atan(nSlope);

                if (p2.X < p1.X)
                {
                    nOrientedAngle = nOrientedAngle + cPI;
                }

                if (p2.X == p1.X)
                {
                    if (p1.Y > p2.Y)
                    {
                        nOrientedAngle = (3 * cPI) / 2;
                    }
                    if (p1.Y < p2.Y)
                    {
                        nOrientedAngle = cPI / 2;
                    }
                }
                if (nOrientedAngle < 0)
                {
                    nOrientedAngle = nOrientedAngle + 2 * cPI;
                }
                else if (nOrientedAngle > 2 * cPI)
                {
                    nOrientedAngle = nOrientedAngle - 2 * cPI;
                }


            }
            catch (Exception ex)
            {
                throw ex;
            }
            return nOrientedAngle;
        }

        /// <summary>
        /// ExitCmd is used to clear all objects of custom command.
        /// </summary>
        private void ExitCmd()
        {
            try
            {
                GTClassFactory.Create<IGTApplication>().SetStatusBarText(GTStatusPanelConstants.gtaspcMessage, "Trasferfeature : Exit Relocate Feature.");

                if (m_gtApplication.Application.Properties.Count > 0)
                    m_gtApplication.Application.Properties.Remove("RelocateFeatureCC");

                m_gtApplication.Application.SelectedObjects.Clear();
                if (m_CustomCommandHelper != null)
                {
                    m_CustomCommandHelper.Complete();
                    m_CustomCommandHelper = null;
                }

                m_gtApplication.Application.EndWaitCursor();
                m_gtApplication.Application.RefreshWindows();
            }
            catch (Exception ex)
            {
                if (m_gtApplication.Application.Properties.Count > 0)
                    m_gtApplication.Application.Properties.Remove("RelocateFeatureCC");
                if (m_CustomCommandHelper != null)
                {
                    m_CustomCommandHelper.Complete();
                    m_CustomCommandHelper = null;
                }

                m_CustomCommandHelper = null;
                m_gtApplication.Application.EndWaitCursor();
                m_gtApplication.Application.RefreshWindows();

                MessageBox.Show(ex.Message, "Relocate Feature", MessageBoxButtons.OK, MessageBoxIcon.Error,
                        MessageBoxDefaultButton.Button1);
            }
        }


        /// <summary>
        /// gets the geometric point at specified length for the passed in polyline geometry
        /// </summary>
        /// <param name="polygeom"></param>
        /// <param name="length"></param>
        /// <returns></returns>
        public IGTPoint GetPointAtLength(IGTPolylineGeometry segment, double length)
        {
            double currlength = 0;
            // get all the segments of the  line geometry and compute the lengths
            if (segment.KeypointCount > 0)
            {
                for (int j = 0; j < segment.KeypointCount - 1; j++)
                {
                    double ln = findistance(segment.GetKeypointPosition(j), segment.GetKeypointPosition(j + 1));
                    if (currlength + ln > length)
                    {
                        // point is in between these two segments
                        IGTPoint fstpt = segment.GetKeypointPosition(j), lstpt = segment.GetKeypointPosition(j + 1);

                        return (GTClassFactory.Create<IGTPoint>(fstpt.X + (lstpt.X - fstpt.X) / ln * (length - currlength),
                                           fstpt.Y + (lstpt.Y - fstpt.Y) / ln * (length - currlength), 0));
                    }
                    else
                        currlength += ln;

                }
            }
            return null;
        }
        /// <summary>
        /// gets the geometric point at specified length for the passed in composite polyline geometry
        /// </summary>
        /// <param name="polygeom"></param>
        /// <param name="length"></param>
        /// <returns></returns>
        public IGTPoint GetPointAtLength(IGTCompositePolylineGeometry polygeom, double length)
        {
            double currlength = 0; int i = 0;
            //functionName = "getGeomPointAtLength";
            IList<IGTGeometry> geom = (IList<IGTGeometry>)polygeom;
            try
            {
                for (i = 0; i < geom.Count; i++)
                {
                    //if (geom[i].GetType() == typeof(IGTPolylineGeometry))
                    {
                        #region LineSegCalc

                        IGTPolylineGeometry segment = (IGTPolylineGeometry)geom[i];
                        // get all the segments of the  line geometry and compute the lengths
                        if (currlength + findistance(segment.FirstPoint, segment.LastPoint) > length)
                            return GetPointAtLength(segment, length - currlength);
                        else
                            currlength += findistance(segment.FirstPoint, segment.LastPoint);
                        #endregion
                    }
                }
                return null;
            }
            catch (Exception ex)
            {
                //LogHelper.log.Error(functionName + "() : " + ex.Message, true);
                throw ex;
            }

            finally
            {
                currlength = 0;
                geom = null;

            }


        }
    }
}
