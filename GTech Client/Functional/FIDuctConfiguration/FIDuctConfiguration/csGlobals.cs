using System;
using System.Windows.Forms;
using ADODB;
using Intergraph.GTechnology.API;


namespace FIDuctConfiguration
{
   public class csGlobals
    {
        public csGlobals(IGTDataContext p_oDataContext,IGTKeyObject p_oKeyObject)
        {
            m_oDataContext = p_oDataContext;
            m_FormationFeature = p_oKeyObject;
        }

        private IGTKeyObject m_FormationFeature { get; set; }
        private IGTDataContext m_oDataContext = null;

        public  bool FormationExist(string FID, string FNO)
        {
            string tmpQry = "";
            Recordset tmpRs = null;
            bool bFlag = false;

            try
            {
                tmpQry = "SELECT G3E_FID FROM CONTAIN_N WHERE G3E_OWNERFID=" + FID + " AND G3E_OWNERFNO=" + FNO;

                tmpRs = m_oDataContext.OpenRecordset(tmpQry, CursorTypeEnum.adOpenStatic,
                                                    LockTypeEnum.adLockReadOnly, (
                                                    int)CommandTypeEnum.adCmdText);
                if (tmpRs.RecordCount > 0)
                {
                    bFlag = true;

                }

            }
            catch (Exception e)
            {
                MessageBox.Show("SQL: " + tmpQry + " " + e.Message, "FormationExist - Error", MessageBoxButtons.OK);
            }

            return (bFlag);

        }

        public  void CreateDucConfiguration(int Horz, int Vert)
        {
            try
            {
                int iDuct = 0;
                int i = 0;
                int j = 0;

                object oMissing = System.Reflection.Missing.Value;

                //Get Feature from Relatioship services
               

                //gServices.ActiveFeature = tmpFeature;
                //gServices.SilentEstablish(csConstant.iContains, csGlobals.tmpFeatureFormation);

                iDuct = 1;
                int Val = 0;

                for (i = 1; i < Vert + 1; i++)
                {
                    Val = Horz;
                    for (j = 1; j < Horz + 1; j++)
                    {
                        CreateDuct( iDuct.ToString(), j, i, Val, i, 1, 1);
                        iDuct = iDuct + 1;
                        Val = Val - 1;
                    }
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Create Duct Configuration", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }


        }

        public void CreateDuct(string sASSIGNMENT, int iPOS_HORZ_FROM, int iPOS_VERT_FROM, int iPOS_HORZ_TO, int iPOS_VERT_TO, int iSTALE_FLAG_FROM, int iSTALE_FLAG_TO)
        {
            IGTKeyObject tmpFeatureDuct = null;

            try
            {
                object oMissing = System.Reflection.Missing.Value;

                //Create fORMATION
                tmpFeatureDuct = m_oDataContext.NewFeature(csConstant.SHORT_DUCT);
                //NETELEM
                tmpFeatureDuct.Components["COMMON_N"].Recordset.MoveFirst();
                //csGlobals.tmpFeatureFormation.Components["GC_NETELEM"].Recordset.Fields["FEATURE_STATE"].Value = dataGridViewExpansion.Rows[i].Cells["STATE"].Value.ToString();
                //MAIN COMPONENT
                tmpFeatureDuct.Components["DUCT_N"].Recordset.MoveFirst();
                //ASSIGNMENT,POS_HORZ_FROM,POS_HORZ_TO,POS_VERT_FROM,POS_VERT_TO,STALE_FLAG_FROM,STALE_FLAG_TO
                tmpFeatureDuct.Components["DUCT_N"].Recordset.Fields["ASSIGNMENT_ID"].Value = sASSIGNMENT;
                tmpFeatureDuct.Components["DUCT_N"].Recordset.Fields["FROM_POS_HORZ"].Value = iPOS_HORZ_FROM;
                tmpFeatureDuct.Components["DUCT_N"].Recordset.Fields["TO_POS_HORZ"].Value = iPOS_HORZ_TO;
                tmpFeatureDuct.Components["DUCT_N"].Recordset.Fields["FROM_POS_VERT"].Value = iPOS_VERT_FROM;
                tmpFeatureDuct.Components["DUCT_N"].Recordset.Fields["TO_POS_VERT"].Value = iPOS_VERT_TO;
                tmpFeatureDuct.Components["DUCT_N"].Recordset.Fields["FROM_STALE_FLAG"].Value = iSTALE_FLAG_FROM;
                tmpFeatureDuct.Components["DUCT_N"].Recordset.Fields["TO_STALE_FLAG"].Value = iSTALE_FLAG_TO;

                IGTRelationshipService gServices = GTClassFactory.Create<IGTRelationshipService>();
                gServices.DataContext = m_oDataContext;
                gServices.ActiveFeature = m_FormationFeature;
                gServices.SilentEstablish(csConstant.iContains, tmpFeatureDuct);
                gServices.Dispose();
                gServices = null;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Create Duct", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public void DeleteExistingDucts(short p_FNO, int p_FID)
        {
            try
            {
                IGTKeyObject oKeyObject = m_oDataContext.OpenFeature(p_FNO, p_FID);
                IGTRelationshipService oRelSVC = GTClassFactory.Create<IGTRelationshipService>();
                oRelSVC.DataContext = m_oDataContext;
                oRelSVC.ActiveFeature = oKeyObject;
                IGTKeyObjects oDucks = oRelSVC.GetRelatedFeatures(7);

                if (oDucks != null && oDucks.Count > 0)
                {
                    for (int i = 0; i < oDucks.Count; i++)
                    {
                        foreach (IGTComponent item in oDucks[i].Components)
                        {
                            DeleteComponent(item);
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

        private void DeleteComponent(IGTComponent p_oComponent)
        {
            try
            {
                if (p_oComponent != null)
                {
                    if (p_oComponent.Recordset != null)
                    {
                        if (p_oComponent.Recordset.RecordCount > 0)
                        {
                            p_oComponent.Recordset.MoveFirst();
                            while (p_oComponent.Recordset.EOF == false)
                            {
                                p_oComponent.Recordset.Delete();
                                if (p_oComponent.Recordset.EOF == false)
                                {
                                    p_oComponent.Recordset.MoveNext();
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
    }
}
