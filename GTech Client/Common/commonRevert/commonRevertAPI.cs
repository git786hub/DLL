//----------------------------------------------------------------------------+
//        Class: commonRevert
//  Description: commonRevert is a common class for revert feature command and revert job command.
//----------------------------------------------------------------------------+
//     $$Author::         HCCI                                                $
//       $$Date::         14/11/2017 3.30 PM                                  $
//   $$Revision::         1                                                   $
//----------------------------------------------------------------------------+
//    $$History::         commonRevert.cs                                   $
//
//************************Version 1**************************
//User: Sithara    Date: 14/11/2017   Time : 3.30PM
//Created commonRevert.cs class
//----------------------------------------------------------------------------+
using System;
using System.Collections.Generic;
using Intergraph.GTechnology.API;
using ADODB;


namespace GTechnology.Oncor.CustomAPI
{
  public class commonRevertAPI
  {
    private IGTApplication m_gtApplication = null;
    public bool m_uProcessedCUs = false;
    public string m_WRID = null;
    public string m_FromJob = "EMPTY";
    /// <summary>
    /// RevertfeatureComm constructor
    /// </summary>
    /// <param name="gt_Application"></param>
    public commonRevertAPI(IGTApplication gt_Application)
    {
      try
      {
        m_gtApplication = gt_Application;
      }
      catch(Exception ex)
      {
        throw ex;
      }
    }

    /// <summary>
    /// ValidateActiveFetature : Checks the active feature is valid to run the command.
    /// </summary>
    /// <param name="actKeyObj"></param>
    /// <returns></returns>
    public bool ValidateActiveFetature(IGTKeyObject actKeyObj)
    {
      string sql = "";
      Recordset rsValidate = null;

      try
      {
        sql = sql = "SELECT * FROM ASSET_HISTORY WHERE G3E_FID=:1 and rownum = 1 order by change_date desc";
        rsValidate = m_gtApplication.DataContext.OpenRecordset(sql, ADODB.CursorTypeEnum.adOpenDynamic, ADODB.LockTypeEnum.adLockReadOnly,
            (int)ADODB.CommandTypeEnum.adCmdText, new object[] { actKeyObj.FID });
        if(rsValidate != null && rsValidate.RecordCount > 0)
        {
          rsValidate.MoveFirst();
          if(Convert.ToString(rsValidate.Fields["G3E_IDENTIFIER"].Value) == m_gtApplication.DataContext.ActiveJob)
          {
            return true;
          }
          else
          {
            m_WRID = Convert.ToString(rsValidate.Fields["G3E_IDENTIFIER"].Value);
            return false;
          }
        }
        else
        {
          return true;
        }
      }
      catch(Exception ex)
      {
        throw ex;
      }
      finally
      {
        if(rsValidate != null)
        {
          if(rsValidate.State == 1)
          {
            rsValidate.Close();
            rsValidate.ActiveConnection = null;
          }
          rsValidate = null;
        }
      }
    }
        /// <summary>
        /// RevertFeture : Removes CUs for a feature that has been posted as part of a WR.
        /// </summary>
        /// <param name="fno"></param>
        /// <param name="fid"></param>
        /// <param name="strFState"></param>
        /// <param name="isfromWP"></param>
        /// <param name="wpFno"></param>
        /// <param name="wpFid"></param>
        public void RevertFeture(short fno, int fid, string strFState, short isfromWP, short wpFno, int wpFid)
        {
            IGTComponent cuComponent = null;
            IGTComponent acuComponent = null;
            IGTComponents cComponents = GTClassFactory.Create<IGTComponents>();
            IGTKeyObject actKeyObj = null;
            IGTKeyObject actWPKeyObj = null;
            //bool cWRid = false;
            bool deleteFt = false;
            bool unCu = false;
            try
            {

                actKeyObj = m_gtApplication.DataContext.OpenFeature(fno, fid);

                cuComponent = actKeyObj.Components.GetComponent(21);
                acuComponent = actKeyObj.Components.GetComponent(22);
                cComponents.Add(cuComponent);
                cComponents.Add(acuComponent);

                if (isfromWP == 1)
                {
                    if (actKeyObj.Components["COMMON_N"].Recordset != null && actKeyObj.Components["COMMON_N"].Recordset.RecordCount > 0)
                    {
                        actKeyObj.Components["COMMON_N"].Recordset.MoveFirst();
                        strFState = actKeyObj.Components["COMMON_N"].Recordset.Fields["FEATURE_STATE_C"].Value.ToString();
                    }

                    actWPKeyObj = m_gtApplication.DataContext.OpenFeature(wpFno, wpFid);
                }


                bool bActivity = true;
                bool bWRMatches = true;

                #region Case when feature state = INI and all CU and Ancillary CU components have activity I or IC
                foreach (IGTComponent comp in cComponents)
                {
                    if (strFState == "INI")
                    {
                        if (comp != null && comp.Recordset != null && comp.Recordset.RecordCount > 0)
                        {
                            comp.Recordset.MoveFirst();

                            while (!comp.Recordset.EOF)
                            {
                                if (comp.Recordset.Fields["WR_ID"].Value.ToString() != m_gtApplication.DataContext.ActiveJob)
                                {
                                    bWRMatches = false;
                                    break;
                                }
                                if (comp.Recordset.Fields["ACTIVITY_C"].Value.ToString() != "I" && comp.Recordset.Fields["ACTIVITY_C"].Value.ToString() != "IC")
                                {
                                    bActivity = false;
                                    break;
                                }

                                comp.Recordset.MoveNext();
                            }                            
                        }
                    }                   
                }
                if (strFState == "INI" && bActivity && bWRMatches)
                {
                    DeleteFeature(actKeyObj);
                    return;
                }
                #endregion


                foreach (IGTComponent comp in cComponents)
                {
                    //cWRid = false;
                    if (strFState == "PPI" || strFState == "ABI")
                    {
                        #region

                        if (comp != null && comp.Recordset != null && comp.Recordset.RecordCount > 0)
                        {
                            comp.Recordset.MoveFirst();
                            while (!comp.Recordset.EOF)
                            {
                                if (comp.Recordset.Fields["WR_ID"].Value.ToString() == m_gtApplication.DataContext.ActiveJob)
                                {
                                    //cWRid = true;
                                    deleteFt = true;
                                    if (isfromWP == 1)
                                    {
                                        DeleteWorkPointCU(actKeyObj, actWPKeyObj, comp);
                                    }
                                }
                                else
                                { unCu = true; }
                                comp.Recordset.MoveNext();
                            }
                        }

                        #endregion
                    }
                    else if (strFState == "PPR" || strFState == "ABR" || strFState == "PPA" || strFState == "ABA")
                    {
                        #region

                        if (comp != null && comp.Recordset != null && comp.Recordset.RecordCount > 0)
                        {
                            comp.Recordset.MoveFirst();
                            while (!comp.Recordset.EOF)
                            {
                                if (comp.Recordset.Fields["WR_ID"].Value.ToString() == m_gtApplication.DataContext.ActiveJob ||
                                    Convert.ToString(comp.Recordset.Fields["WR_EDITED"].Value) == m_gtApplication.DataContext.ActiveJob)
                                {

                                    if (isfromWP == 1)
                                    {
                                        DeleteWorkPointCU(actKeyObj, actWPKeyObj, comp);
                                    }

                                    comp.Recordset.Fields["ACTIVITY_C"].Value = "";
                                    comp.Recordset.Update();
                                    //cWRid = true;

                                    if (actKeyObj.Components["COMMON_N"] != null && actKeyObj.Components["COMMON_N"].Recordset != null && actKeyObj.Components["COMMON_N"].Recordset.RecordCount > 0)
                                    {
                                        actKeyObj.Components["COMMON_N"].Recordset.Fields["FEATURE_STATE_C"].Value = "CLS";
                                        actKeyObj.Components["COMMON_N"].Recordset.Update();
                                    }


                                }
                                else
                                { unCu = true; }
                                comp.Recordset.MoveNext();
                            }
                        }


                        #endregion
                    }
                    else if (strFState == "PPX" || strFState == "ABX" || strFState == "INI")
                    {
                        #region

                        if (comp != null && comp.Recordset != null && comp.Recordset.RecordCount > 0)
                        {
                            comp.Recordset.MoveFirst();
                            while (!comp.Recordset.EOF)
                            {
                                if (comp.Recordset.Fields["WR_ID"].Value.ToString() == m_gtApplication.DataContext.ActiveJob)
                                {
                                    //cWRid = true;


                                    if (actKeyObj.Components["COMMON_N"] != null && actKeyObj.Components["COMMON_N"].Recordset != null && actKeyObj.Components["COMMON_N"].Recordset.RecordCount > 0)
                                    {
                                        actKeyObj.Components["COMMON_N"].Recordset.Fields["FEATURE_STATE_C"].Value = "CLS";
                                        actKeyObj.Components["COMMON_N"].Recordset.Update();
                                    }

                                    if (isfromWP == 1)
                                    {
                                        DeleteWorkPointCU(actKeyObj, actWPKeyObj, comp);
                                    }

                                    if (Convert.ToString(comp.Recordset.Fields["ACTIVITY_C"].Value) != "")
                                    {
                                        if (comp.Recordset.Fields["ACTIVITY_C"].Value.ToString() == "I" || comp.Recordset.Fields["ACTIVITY_C"].Value.ToString() == "IC")
                                        {
                                            comp.Recordset.Delete();
                                        }
                                        else if (comp.Recordset.Fields["ACTIVITY_C"].Value.ToString() == "R" || comp.Recordset.Fields["ACTIVITY_C"].Value.ToString() == "RC")
                                        {
                                            comp.Recordset.Fields["ACTIVITY_C"].Value = "";
                                            comp.Recordset.Update();
                                        }
                                    }
                                }
                                else
                                { unCu = true; }
                                comp.Recordset.MoveNext();
                            }
                        }

                        #endregion
                    }
                    else
                    {
                        m_uProcessedCUs = true;
                    }
                }

                if (deleteFt && (strFState == "PPI" || strFState == "ABI"))
                {
                    DeleteFeature(actKeyObj);
                }

                if (actWPKeyObj != null)
                {
                    DeleteFeature(actWPKeyObj);
                }

                if (unCu && !deleteFt)
                {
                    m_uProcessedCUs = true;
                }

            }
            catch (Exception)
            {
                throw;
            }
        }

    /// <summary>
    /// Revert Workpoint feature.
    /// </summary>
    /// <param name="actWPKeyObj"></param>
    public void RevertWPFeature(IGTKeyObject actWPKeyObj)
    {
      IGTComponent cuComponent = null;
      short aFno = 0;
      int aFid = 0;
      List<rFeatureCollection> fListcollection = new List<rFeatureCollection>();
      try
      {
        cuComponent = actWPKeyObj.Components.GetComponent(19104);
        if(cuComponent != null && cuComponent.Recordset != null && cuComponent.Recordset.RecordCount > 0)
        {


          cuComponent.Recordset.MoveFirst();
          while(!cuComponent.Recordset.EOF)
          {
            rFeatureCollection fcollection = new rFeatureCollection();

            aFno = Convert.ToInt16(cuComponent.Recordset.Fields["ASSOC_FNO"].Value);
            aFid = Convert.ToInt32(cuComponent.Recordset.Fields["ASSOC_FID"].Value);

            fcollection.RFno = aFno;
            fcollection.RFid = aFid;
            fcollection.RFstate = null;

            fcollection.RWPFno = actWPKeyObj.FNO;
            fcollection.RWPFid = actWPKeyObj.FID;

            if(fListcollection.Count > 0)
            {
              //RFeatureCollection fObject = fListcollection.Where(p => p.RFid.Any(q => q.RFid == aFid));

              if(!fListcollection.Exists(x => x.RFid == aFid))
              {
                fListcollection.Add(fcollection);
              }
            }
            else
            {
              fListcollection.Add(fcollection);
            }

            cuComponent.Recordset.MoveNext();
          }
        }

        foreach(rFeatureCollection fc in fListcollection)
        {
          if(ValidateActiveFetature(m_gtApplication.DataContext.OpenFeature(fc.RFno, fc.RFid)))
          {
            RevertFeture(fc.RFno, fc.RFid, null, 1, fc.RWPFno, fc.RWPFid);
          }
        }


      }
      catch(Exception)
      {
        throw;
      }

    }
    /// <summary>
    /// Delete entire feature.
    /// </summary>
    /// <param name="actKey"></param>
    /// <returns></returns>
    public bool DeleteFeature(IGTKeyObject actKey)
    {
      bool deleted = false;
      bool canDelete = true;
      Recordset rsTemp = null;
      string sql = "";
      IGTComponent cuComponent = null;
      try
      {
        if(actKey.FNO == 191)
        {
          cuComponent = actKey.Components.GetComponent(19104);
          if(cuComponent != null && cuComponent.Recordset != null && cuComponent.Recordset.RecordCount > 0)
          {
            canDelete = false;
          }
          else
          {
            if(m_gtApplication.SelectedObjects.GetObjects().Count > 0 && m_FromJob != "JOB")
            {
              cuComponent = actKey.Components.GetComponent(19001);
              if(cuComponent != null && cuComponent.Recordset != null && cuComponent.Recordset.RecordCount > 0)
              {
                canDelete = false;
              }
            }
          }
        }

        if(canDelete)
        {
          sql = "SELECT G3E_NAME FROM G3E_FEATURECOMPS_OPTABLE WHERE G3E_FNO  = ? ORDER BY G3E_DELETEORDINAL";
          rsTemp = m_gtApplication.DataContext.OpenRecordset(sql, ADODB.CursorTypeEnum.adOpenDynamic, ADODB.LockTypeEnum.adLockReadOnly,
              (int)ADODB.CommandTypeEnum.adCmdText, actKey.FNO);

          for(int i = 0;i < rsTemp.RecordCount;i++)
          {
            foreach(IGTComponent comp in actKey.Components)
            {
              if(comp.Recordset != null && comp.Recordset.RecordCount > 0 && comp.Name == Convert.ToString(rsTemp.Fields["G3E_NAME"].Value))
              {
                comp.Recordset.MoveFirst();
                while(!comp.Recordset.EOF)
                {
                  comp.Recordset.Delete();
                  comp.Recordset.MoveNext();
                }
                break;
              }
            }
            rsTemp.MoveNext();
          }
        }

      }
      catch(Exception)
      {
        throw;
      }
      finally
      {
        if(rsTemp != null)
        {
          if(rsTemp.State == 1)
          {
            rsTemp.Close();
            rsTemp.ActiveConnection = null;
          }
          rsTemp = null;
        }
      }
      return deleted;
    }
    /// <summary>
    /// Delete Workpoint Cu instances.
    /// </summary>
    /// <param name="wpCU"></param>
    /// <param name="WPKeyObj"></param>
    /// <param name="comp"></param>
    /// <returns></returns>
    public bool DeleteWorkPointCU(IGTKeyObject wpCU, IGTKeyObject WPKeyObj, IGTComponent comp)
    {
      bool deleted = false;
      short aFno = 0;
      int aFid = 0;
      int aCid = 0;
      short aCno = 0;

      IGTComponent cuComponent = null;
      try
      {
        cuComponent = WPKeyObj.Components.GetComponent(19104);
        if(cuComponent != null && cuComponent.Recordset != null && cuComponent.Recordset.RecordCount > 0)
        {
          cuComponent.Recordset.MoveFirst();
          while(!cuComponent.Recordset.EOF)
          {
            aFno = Convert.ToInt16(cuComponent.Recordset.Fields["ASSOC_FNO"].Value);
            aFid = Convert.ToInt32(cuComponent.Recordset.Fields["ASSOC_FID"].Value);

            if(Convert.ToString(cuComponent.Recordset.Fields["UNIT_CNO"].Value) != "")
            {
              aCno = Convert.ToInt16(cuComponent.Recordset.Fields["UNIT_CNO"].Value);
            }

            if(Convert.ToString(cuComponent.Recordset.Fields["UNIT_CID"].Value) != "")
            {
              aCid = Convert.ToInt32(cuComponent.Recordset.Fields["UNIT_CID"].Value);
            }


            if(wpCU.FID == aFid && wpCU.FNO == aFno && comp.CNO == aCno && Convert.ToInt32(comp.Recordset.Fields["G3E_CID"].Value) == aCid)
            {
              cuComponent.Recordset.Delete();
            }


            cuComponent.Recordset.MoveNext();
          }
        }
      }
      catch(Exception)
      {
        throw;
      }

      return deleted;
    }
  }
}
