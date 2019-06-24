// ===================================================
//  Copyright 2018 Intergraph Corp.
//  File Name: ccReviewAssetHistory.cs
// 
//  Description: ReviewAssetHistory is used to display complete asset history for a single feature, or asset history for all features in a specified WR/job.
//
//  Remarks:
// 
//  Modification History
//  Date                Author                      Comments
//  --------            --------------              --------
//  09/01/2018          Sithara                     Implemented  as per JIRA 1188
// ======================================================
using System;
using System.Collections.Generic;
using Intergraph.GTechnology.API;
using Intergraph.GTechnology.Interfaces;
using System.Windows.Forms;
using ADODB;


namespace GTechnology.Oncor.CustomAPI
{
  public class ccReviewAssetHistory : IGTCustomCommandModeless
  {
    #region Global Variables

    IGTCustomCommandHelper m_GTCustomCommandHelper;
    IGTTransactionManager m_oGTTransactionManager;
    ReviewAssetHistoryForm reviewAssetHistoryForm;
    Boolean m_canTerminate = false;
    AssetHistoryModel assetHistoryModel;

    #endregion

    public bool CanTerminate
    {
      get { return m_canTerminate; }
    }

    public IGTTransactionManager TransactionManager
    {
      get { return m_oGTTransactionManager; }
      set { m_oGTTransactionManager = value; }
    }

    public void Activate(IGTCustomCommandHelper CustomCommandHelper)
    {
      IGTDDCKeyObjects selectedDDDCKeyObjects = null;
      IList<int> SelectedFeatureIDList = new List<int>();
      assetHistoryModel = new AssetHistoryModel();
      m_GTCustomCommandHelper = CustomCommandHelper;
      IGTDisplayService gTDisplayService;

            try
            {
                assetHistoryModel.m_Application = (IGTApplication)GTClassFactory.Create<IGTApplication>();
                assetHistoryModel.m_DataContext = assetHistoryModel.m_Application.DataContext;

                if (assetHistoryModel.m_Application.ActiveMapWindow != null) 
                {
                    gTDisplayService = assetHistoryModel.m_Application.ActiveMapWindow.DisplayService;
                    selectedDDDCKeyObjects = assetHistoryModel.m_Application.SelectedObjects.GetObjects();
                }
                //If selected set cntains features.
                if (selectedDDDCKeyObjects != null && selectedDDDCKeyObjects.Count > 0)
                {
                    #region
                    foreach (IGTDDCKeyObject ddcKeyObject in selectedDDDCKeyObjects)
                    {
                        if (!SelectedFeatureIDList.Contains(ddcKeyObject.FID))
                        {
                            SelectedFeatureIDList.Add(ddcKeyObject.FID);
                        }
                    }

                    if (SelectedFeatureIDList.Count == 1)
                    {
                        CreateKeyObjectOfSelectedDDC(selectedDDDCKeyObjects);
                    }
                    else
                    {
                        assetHistoryModel.m_WRID = assetHistoryModel.m_DataContext.ActiveJob;
                        assetHistoryModel.m_isStructure = false;
                    }
                    #endregion
                }
                //If User selected the temporary geometry(Tool tip code)
                //else if (gTDisplayService.SelectedDisplayControlNode != null && gTDisplayService.SelectedDisplayControlNode.DisplayName != string.Empty
                //    && gTDisplayService.SelectedDisplayControlNode.LegendEntry != null && gTDisplayService.SelectedDisplayControlNode.LegendEntry.EntryType == GTech.GTLegendEntryTypeConstants.gtletTemporaryGeometry)
                //{
                //    strTempGeoToolTip = GetToolTipOfTemporaryGeometry(gTDisplayService, out strTempGeoDisplayPath, out strTempGeoDisplayName, strTempGeoToolTip);

                //    if (!string.IsNullOrEmpty(strTempGeoToolTip) && strTempGeoToolTip.ToUpper().StartsWith("FID"))
                //    {
                //        string[] tempArray = strTempGeoToolTip.Split(new char[0], StringSplitOptions.RemoveEmptyEntries);
                //        CreateKeyObjectOfGivenFID(Convert.ToInt32(tempArray[1]));
                //    }
                //}
                //If user did not select feature /Temporary geometry the form defaults to qurying by WR. 
                else
                {
                    assetHistoryModel.m_isStructure = false;
                    assetHistoryModel.m_FID = 0;
                    assetHistoryModel.m_WRID = assetHistoryModel.m_DataContext.ActiveJob;
                }


                //Load the history form.
                GetReplaceIDAno();
                reviewAssetHistoryForm = new ReviewAssetHistoryForm(assetHistoryModel, m_oGTTransactionManager, m_GTCustomCommandHelper);
                reviewAssetHistoryForm.StartPosition = FormStartPosition.CenterParent;
                reviewAssetHistoryForm.Show(assetHistoryModel.m_Application.ApplicationWindow);


            }
            catch (Exception ex)
            {
                if (reviewAssetHistoryForm != null) { reviewAssetHistoryForm.Close(); reviewAssetHistoryForm.Dispose(); reviewAssetHistoryForm = null; }
                MessageBox.Show("Error during execution of Review Asset History custom command." + Environment.NewLine + ex.Message,
                    "G/Techonology", MessageBoxButtons.OK, MessageBoxIcon.Error);

                assetHistoryModel.ExitCommand(m_oGTTransactionManager, m_GTCustomCommandHelper);
            }
            finally
            {
                assetHistoryModel.m_Application.EndWaitCursor();
            }
    }

    /// <summary>
    /// This method is used to get the tool tip of the selected temporary geometry.
    /// </summary>
    /// <param name="gTDisplayService"></param>
    /// <param name="strTempGeoDisplayPath"></param>
    /// <param name="strTempGeoDisplayName"></param>
    /// <param name="strTempGeoToolTip"></param>
    /// <returns></returns>
    private string GetToolTipOfTemporaryGeometry(IGTDisplayService gTDisplayService, out string strTempGeoDisplayPath, out string strTempGeoDisplayName, string strTempGeoToolTip)
    {
      strTempGeoDisplayPath = gTDisplayService.SelectedDisplayControlNode.DisplayPathName;
      strTempGeoDisplayName = gTDisplayService.SelectedDisplayControlNode.DisplayName;

      if(!string.IsNullOrEmpty(strTempGeoDisplayPath) && !string.IsNullOrEmpty(strTempGeoDisplayName))
      {
        IGTPoint ptTempGeo = gTDisplayService.GetRange(strTempGeoDisplayPath, strTempGeoDisplayName).BottomLeft;
        IGTDDCKeyObjects gTDDCTempGeoKeyObjects = assetHistoryModel.m_Application.ActiveMapWindow.LocateService.Locate(ptTempGeo, 5, -1, GTSelectionTypeConstants.gtmwstSelectAll);

        if(gTDDCTempGeoKeyObjects.Count > 0)
        {
          foreach(IGTDDCKeyObject ddcKey in gTDDCTempGeoKeyObjects)
          {
            if(ddcKey.FNO == 400)
            {
              strTempGeoToolTip = Convert.ToString(gTDDCTempGeoKeyObjects[0].Recordset.Fields["G3E_TOOLTIP"].Value);
              break;
            }
          }
        }
      }
      if(string.IsNullOrEmpty(strTempGeoToolTip))
      {
        strTempGeoToolTip = gTDisplayService.SelectedDisplayControlNode.DisplayName;
      }

      return strTempGeoToolTip;
    }

    /// <summary>
    /// This method created the keyobject if the select set contains exactly one feature and if it is a structure, the form defaults to querying by Structure ID.
    /// </summary>
    /// <param name="selectedDDDCKeyObjects"></param>
    /// <returns></returns>
    private void CreateKeyObjectOfSelectedDDC(IGTDDCKeyObjects selectedDDDCKeyObjects)
    {
      try
      {
        ConfirmSelectedFeatureIsStructure(selectedDDDCKeyObjects[0].FID, selectedDDDCKeyObjects[0].FNO);
      }
      catch
      {
        throw;
      }

    }

    /// <summary>
    /// This method created the keyobject using temporary geometry tooltip and if it is a structure, the form defaults to querying by Structure ID.
    /// </summary>
    /// <param name="Fid"></param>
    private void CreateKeyObjectOfGivenFID(int Fid)
    {

      Recordset rs = null;
      try
      {
        string sql = string.Format("select g3e_fno from common_n where g3e_fid={0}", Fid);
        rs = assetHistoryModel.m_DataContext.OpenRecordset(sql, ADODB.CursorTypeEnum.adOpenDynamic, ADODB.LockTypeEnum.adLockReadOnly, (int)ADODB.CommandTypeEnum.adCmdText,
               null);

        if(rs != null && rs.RecordCount > 0)
        {
          rs.MoveFirst();
          ConfirmSelectedFeatureIsStructure(Fid, Convert.ToInt16(rs.Fields[0].Value));
        }
      }
      catch
      {
        throw;
      }
      finally
      {
        if(rs != null)
        {
          rs.Close();
          rs = null;
        }
      }
    }

    /// <summary>
    /// This method validated the selected feature is Structure or not.
    /// </summary>
    /// <param name="Fid"></param>
    /// <param name="Fno"></param>
    private void ConfirmSelectedFeatureIsStructure(int fid, short fno)
    {
      try
      {
        IGTComponent gTCommonComponent = null;
        assetHistoryModel.m_FID = fid;
        assetHistoryModel.m_KeyObject = assetHistoryModel.m_DataContext.OpenFeature(fno, fid);
        assetHistoryModel.m_WRID = null;

        if(assetHistoryModel.m_KeyObject.Components["COMMON_N"] != null)
        {
          gTCommonComponent = assetHistoryModel.m_KeyObject.Components["COMMON_N"];
        }

        if(gTCommonComponent != null &&
            gTCommonComponent.Recordset != null &&
            gTCommonComponent.Recordset.RecordCount > 0)
        {

          gTCommonComponent.Recordset.MoveFirst();
          if(!string.IsNullOrEmpty(Convert.ToString(gTCommonComponent.Recordset.Fields["STRUCTURE_ID"].Value)))
          {
            assetHistoryModel.m_isStructure = true;
            assetHistoryModel.m_StructureID = Convert.ToString(gTCommonComponent.Recordset.Fields["STRUCTURE_ID"].Value);
          }
          else
          {
            assetHistoryModel.m_isStructure = false;
          }
        }
      }
      catch
      {
        throw;
      }
    }

    private void GetReplaceIDAno()
    {
      Recordset rsAno = null;
      try
      {
        string strAno = "REPLACED_FID";
        short sAno = 1;
        int ano = 0;

        rsAno = assetHistoryModel.m_DataContext.MetadataRecordset("G3E_ATTRIBUTEINFO_OPTABLE", "G3E_CNO = " + sAno + " AND  G3E_FIELD = '" + strAno + "'");
        if(rsAno != null && rsAno.RecordCount > 0)
        {
          rsAno.MoveFirst();
          ano = Convert.ToInt32(rsAno.Fields["G3E_ANO"].Value);
          assetHistoryModel.m_ReplaceAno = ano;
        }
      }
      catch
      {
        throw;
      }
      finally
      {
        rsAno = null;
      }
    }
    public void Pause()
    {

    }

    public void Resume()
    {

    }

    public void Terminate()
    {

    }


  }
}
