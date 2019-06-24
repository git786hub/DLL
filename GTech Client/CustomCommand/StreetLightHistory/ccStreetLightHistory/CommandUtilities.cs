using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.OleDb;
using ADODB;
using Intergraph.GTechnology.API;
using Intergraph.GTechnology.Interfaces;
using Intergraph.GTechnology.Diagnostics;

namespace GTechnology.Oncor.CustomAPI
{
  public static class CommandUtilities
  {
    #region Diagnostic Methods

    public static void LogMethodEntry(GTDiagnostics oDiag, string sMethod)
    {
      try
      {
        if (oDiag.IsEnabled(GTDiagCat.EE)) oDiag.LogEnter(sMethod);
      }
      catch (Exception ex)
      {
        throw ex;
      }
    }

    public static void LogMethodExit(GTDiagnostics oDiag, string sMethod)
    {
      try
      {
        if (oDiag.IsEnabled(GTDiagCat.EE)) oDiag.LogExit(sMethod);
      }
      catch (Exception ex)
      {
        throw ex;
      }
    }

    public static void LogMessage(GTDiagnostics oDiag, string sMethod, string sCategory, string sMessage)
    {
      try
      {
        if (oDiag.IsEnabled(GTDiagCat.EE)) oDiag.LogMessage(sMethod, sCategory, sMessage);
      }
      catch (Exception ex)
      {
        throw ex;
      }

    }

    public static void LogException(GTDiagnostics oDiag, string sMethod, Exception logExc)
    {
      try
      {
        if (oDiag.IsEnabled(GTDiagCat.EE)) oDiag.LogException(sMethod, logExc);
      }
      catch (Exception ex)
      {
        throw ex;
      }
    }

    public static void LogTestPoints(GTDiagnostics oDiag, IGTDDCKeyObjects ddclist)
    {
      try
      {
        LogMessage(oDiag, "LogTextPoints", "TestPoints", "All located structures.");

        foreach (IGTDDCKeyObject ddc in ddclist)
        {
          if ((ddc.Geometry.Type == GTGeometryTypeConstants.gtgtOrientedPointGeometry) || (ddc.Geometry.Type == GTGeometryTypeConstants.gtgtPointGeometry))
          {
            LogMessage(oDiag, "LogTextPoints", "TestPoints", "FID =" + ddc.FID.ToString() + " FNO = " + ddc.FNO.ToString() + " Geometry = " + ddc.Geometry.Type.ToString());
            LogMessage(oDiag, "LogTextPoints", "TestPoints", "Location = " + ddc.Geometry.FirstPoint.ToString());
          }
        }
      }
      catch (Exception ex)
      {
        throw ex;
      }
    }

    public static void LogStreetLightPoints(GTDiagnostics oDiag, IGTDDCKeyObjects ddc56List)
    {
      try
      {
        LogMessage(oDiag, "StreetLightLogTestPoints", "TestPoints", "All located streetlights.");

        foreach (IGTDDCKeyObject ddc in ddc56List)
        {
          if ((ddc.Geometry.Type == GTGeometryTypeConstants.gtgtOrientedPointGeometry) || (ddc.Geometry.Type == GTGeometryTypeConstants.gtgtPointGeometry))
          {
            LogMessage(oDiag, "StreetLightLogTestPoints", "TestPoints", "FID =" + ddc.FID.ToString() + " FNO = " + ddc.FNO.ToString() + " Geometry = " + ddc.Geometry.Type.ToString());
            LogMessage(oDiag, "StreetLightLogTestPoints", "TestPoints", "Location = " + ddc.Geometry.FirstPoint.ToString());
          }
        }
      }
      catch (Exception ex)
      {
        throw ex;
      }
    }

    public static void LogDistanceCalculationPoints(int _counter, GTActiveStreetLight _sl, GTDiagnostics _odiag)
    {
      try
      {
        if (_counter == 0)
        {
          LogMessage(_odiag, "LogDistanceCalculationPoints", "TEST", "Starting distance calculation for fid = " + _sl.G3E_FID.ToString());
        }

        LogMessage(_odiag, "LogDistanceCalculationPoints", "TEST", "Counter = " + _counter.ToString() + " OGGX = " + _sl.GeometrySource[_counter].OGG_X1.ToString());
        LogMessage(_odiag, "LogDistanceCalculationPoints", "TEST", "Counter = " + _counter.ToString() + " OGGY = " + _sl.GeometrySource[_counter].OGG_Y1.ToString());
        LogMessage(_odiag, "LogDistanceCalculationPoints", "TEST", "Counter = " + _counter+1.ToString() + " OGGX+1 = " + _sl.GeometrySource[_counter+1].OGG_X1.ToString());
        LogMessage(_odiag, "LogDistanceCalculationPoints", "TEST", "Counter = " + _counter + 1.ToString() + " OGGY+1 = " + _sl.GeometrySource[_counter + 1].OGG_Y1.ToString());
      }
      catch (Exception ex)
      {
        LogException(_odiag, "LogDistanceCalculationPoints", ex);
        throw ex;
      }
    }
   
    public static void LogLocationCriteria(IGTGeometry _locCritera, AssetHistory _obj, GTDiagnostics _odiag)
    {
      try
      {

      }
      catch(Exception ex)
      {
        LogException(_odiag, "LogLocationCrietria", ex);
        throw ex;
      }
    }

    #endregion Diagnostic Methods

    #region Query Methods

    public static Recordset ExecuteQuery(IGTDataContext _dc, String sSql, GTDiagnostics _diag)
    {
      Recordset _rs = null;

      try
      {
        int rEffected = -1;
        _rs = _dc.Execute(sSql, out rEffected, (int)CommandTypeEnum.adCmdText, null);
      }
      catch (Exception ex)
      {
        if (_diag.IsEnabled(GTDiagCat.EE)) _diag.LogException("CommandUtilities.ExecuteQuery", ex);
      }

      return _rs;
    }

    public static String Get_Recordset_Value(Recordset oRS, String sColumn, String sFilterColumn, String sFilter, GTDiagnostics _diag)
    {
      string returnString = string.Empty;

      try
      {
        if ((!oRS.EOF) && (!oRS.BOF)) oRS.MoveFirst();
        while ((!oRS.EOF) || (!oRS.BOF))
        {
          if (!string.IsNullOrEmpty(sFilter))
          {
            if (sFilter == oRS.Fields[sFilterColumn].Value.ToString().Trim())
            {
              returnString = oRS.Fields[sColumn].Value.ToString().Trim();
              break;
            }
          }
          else
          {
            returnString = oRS.Fields[sColumn].Value.ToString().Trim();
          }

          oRS.MoveNext();
        }
      }
      catch (Exception ex)
      {
        if (_diag.IsEnabled(GTDiagCat.EE)) _diag.LogException("CommandUtilities.Get_Recordset_Value", ex);
        throw ex;
      }

      return returnString;
    }
    public static DataTable GetDataTable(IGTDataContext oDC, string sSQL, object[] Parameter, GTDiagnostics _diag)
    {
      DataTable dt = null;

      try
      {
        Recordset oRS = oDC.OpenRecordset(sSQL, CursorTypeEnum.adOpenStatic, LockTypeEnum.adLockReadOnly, (int)CommandTypeEnum.adCmdText, Parameter);
        OleDbDataAdapter oDA = new System.Data.OleDb.OleDbDataAdapter();
        dt = new DataTable();
        oDA.Fill(dt, oRS);
        oDA.Dispose();
        oRS.Close();
      }
      catch (Exception ex)
      {
        if (_diag.IsEnabled(GTDiagCat.EE)) _diag.LogException("CommandUtilities.GetDataTable", ex);
        throw ex;
      }

      return dt;
    }

    public static DataTable GetDataTable(IGTDataContext oDC, string sSQL, GTDiagnostics _diag)
    {
      DataTable dt = null;

      try
      {
        //IGTDataContext oDC = GTClassFactory.Create<IGTApplication>().DataContext;
        Recordset oRS = oDC.OpenRecordset(sSQL, CursorTypeEnum.adOpenStatic, LockTypeEnum.adLockReadOnly, (int)CommandTypeEnum.adCmdText);
        OleDbDataAdapter oDA = new System.Data.OleDb.OleDbDataAdapter();
        dt = new DataTable();
        oDA.Fill(dt, oRS);
        oDA.Dispose();
        oRS.Close();
      }
      catch (Exception ex)
      {
        if (_diag.IsEnabled(GTDiagCat.EE)) _diag.LogException("CommandUtilities.GetDataTable", ex);
        throw ex;
      }

      return dt;
    }

    public static ADODB.Recordset OpenCommonRecordset(IGTDataContext _odc, int _fid, short _fno, GTDiagnostics _odiag)
    {
      ADODB.Recordset oRS = null;

      try
      {
        string sSQL = "SELECT G3E_FID, G3E_FNO, STRUCTURE_ID FROM COMMON_N WHERE G3E_FNO = '" + _fno + "'" + " AND G3E_FID = '" + _fid + "'";
        oRS = ExecuteQuery(_odc, sSQL, _odiag);

        if (oRS.RecordCount == 0) return null;

        if ((oRS.EOF) && (oRS.BOF)) return null;

        oRS.MoveFirst();


        while ((!oRS.EOF) && (!oRS.BOF))
        {
          //obj.G3E_FNO = Convert.ToInt16(oRS.Fields["G3E_FNO"].Value.ToString().Trim());
          //obj.G3E_FID = Convert.ToInt32(oRS.Fields["G3E_FID"].Value.ToString().Trim());
        }
      }
      catch (Exception ex)
      {
        LogException(_odiag, "OpencommonRecordset", ex);
        throw ex;
      }

      return oRS;
    }

    #endregion Query Methods

    #region General Parameter

    public static bool GetSysGeneralParameters(IGTDataContext _odc, GTDiagnostics _odiag, ref GTSysGenParam _gp)
    {
      bool bSuccess = false;

      try
      {
        string sSqlSysGP = "SELECT ID, SUBSYSTEM_NAME, SUBSYSTEM_COMPONENT, PARAM_NAME, PARAM_VALUE, PARAM_DESC FROM SYS_GENERALPARAMETER WHERE SUBSYSTEM_NAME = 'ASSET_HISTORY_TRACKING'";
        Recordset rsGeneralParam = CommandUtilities.ExecuteQuery(_odc, sSqlSysGP, _odiag);

        if (rsGeneralParam == null) { CommandUtilities.LogMessage(_odiag, "GetGeneralParameters", "GeneralParameter", "Nothing returned from SYS_GENERALPARAMETER."); return bSuccess; }

        string sThreshold = CommandUtilities.Get_Recordset_Value(rsGeneralParam, "PARAM_VALUE", "PARAM_NAME", "StreetLightMovementThreshold", _odiag);
        string sHistoricalSymbol = CommandUtilities.Get_Recordset_Value(rsGeneralParam, "PARAM_VALUE", "PARAM_NAME", "StreetLightHistoricalSymbol", _odiag);
        string sHistoricalLine = CommandUtilities.Get_Recordset_Value(rsGeneralParam, "PARAM_VALUE", "PARAM_NAME", "StreetLightHistoricalLine", _odiag);
        string sHistoricalStacking = CommandUtilities.Get_Recordset_Value(rsGeneralParam, "PARAM_VALUE", "PARAM_NAME", "StreetLightHistoricalStacking", _odiag);
        string sANOx = Get_Recordset_Value(rsGeneralParam, "PARAM_VALUE", "PARAM_NAME", "ANO_OGGX", _odiag);
        string sANOy = Get_Recordset_Value(rsGeneralParam, "PARAM_VALUE", "PARAM_NAME", "ANO_OGGY", _odiag);
        string sANOz = Get_Recordset_Value(rsGeneralParam, "PARAM_VALUE", "PARAM_NAME", "ANO_OGGZ", _odiag);

        if (!string.IsNullOrEmpty(sThreshold)) _gp.MovementThreshold = Convert.ToInt32(sThreshold);
        if (!string.IsNullOrEmpty(sHistoricalSymbol)) _gp.HistoricalSymbol = Convert.ToInt32(sHistoricalSymbol);
        if (!string.IsNullOrEmpty(sHistoricalSymbol)) _gp.HistoricalLine = Convert.ToInt32(sHistoricalLine);
        if (!string.IsNullOrEmpty(sHistoricalStacking)) _gp.HistoricalStacking = Convert.ToInt32(sHistoricalStacking);
        if (!string.IsNullOrEmpty(sANOx)) _gp.ANO_X = Convert.ToInt32(sANOx);
        else _gp.ANO_X = 108;

        if (!string.IsNullOrEmpty(sANOy)) _gp.ANO_Y = Convert.ToInt32(sANOy);
        else _gp.ANO_Y = 109;

        if (!string.IsNullOrEmpty(sANOz)) _gp.ANO_Z = Convert.ToInt32(sANOz);
        else _gp.ANO_Z = 110;

        rsGeneralParam.Close();
        rsGeneralParam = null;
        bSuccess = true;
      }
      catch (Exception ex)
      {
        CommandUtilities.LogException(_odiag, "GTAssetTracker.GetSysGeneralParameters", ex);
        throw ex;
      }

      return bSuccess;
    }

    #endregion General Parameter

    #region CommonN

    public static GTActiveStreetLight GetCommonNActiveFeature(IGTKeyObject _koStreetLight, IGTKeyObject _koStructure, GTDiagnostics _odiag)
    {
      GTActiveStreetLight _oSL = null;

      try
      {
        _oSL = GetCommonNStreetLight(_koStreetLight, _odiag);
        if (_oSL == null) return null;

        GetCommonNStructure(_koStructure, ref _oSL, _odiag);
      }
      catch(Exception ex)
      {
        LogException(_odiag, "GetCommonN", ex);
        throw ex;
      }

      return _oSL;
    }

    
    private static GTActiveStreetLight GetCommonNStreetLight(IGTKeyObject _ko, GTDiagnostics _odiag)
    {
      GTActiveStreetLight oCommonN = null;

      try
      {
        IGTComponent commonN = _ko.Components["COMMON_N"];
        if (commonN == null) return null;

        ADODB.Recordset oRS = commonN.Recordset;

        if (oRS == null) return null;

        if (oRS.RecordCount == 0) return null;

        oRS.MoveFirst();

        oCommonN = new GTActiveStreetLight();

        if ((!oRS.EOF) && (!oRS.BOF))
        {
          string sid = oRS.Fields["STRUCTURE_ID"].Value.ToString().Trim();
          if (!string.IsNullOrEmpty(sid))
          {
            //Picking up structure id from structure common_n
          }
          else
          {
            oCommonN = null;
            LogMessage(_odiag, "GetCommonN", "COMMON_N", "FID = " + _ko.FID.ToString() + " has no structure_id.");
            return null;
          }


          string sx = oRS.Fields["OGGX_H"].Value.ToString().Trim();
          if (!string.IsNullOrEmpty(sx))
          {
            oCommonN.OGG_X1 = Convert.ToDouble(sx);
          }

          string sy = oRS.Fields["OGGY_H"].Value.ToString().Trim();
          if (!string.IsNullOrEmpty(sy))
          {
            oCommonN.OGG_Y1 = Convert.ToDouble(sy);
          }

          string sz = oRS.Fields["OGGZ_H"].Value.ToString().Trim();
          if (!string.IsNullOrEmpty(sz))
          {
            oCommonN.OGG_Z1 = Convert.ToDouble(sz);
          }

          string sRepFID = oRS.Fields["REPLACED_FID"].Value.ToString().Trim();
          if (!string.IsNullOrEmpty(sRepFID))
          {
            oCommonN.REPLACED_FID = Convert.ToInt32(oRS.Fields["REPLACED_FID"].Value.ToString().Trim());
          }
        }

      }
      catch (Exception ex)
      {
        LogException(_odiag, "GetCommonN", ex);
        throw ex;
      }

      return oCommonN;
    }

    private static void GetCommonNStructure(IGTKeyObject _ko, ref GTActiveStreetLight _obj, GTDiagnostics _odiag)
    {
      GTActiveStreetLight oCommonN = null;

      try
      {
        IGTComponent commonN = _ko.Components["COMMON_N"];
        if (commonN == null) return;

        ADODB.Recordset oRS = commonN.Recordset;

        if (oRS == null) return;

        if (oRS.RecordCount == 0) return;

        oRS.MoveFirst();

        oCommonN = new GTActiveStreetLight();

        if ((!oRS.EOF) && (!oRS.BOF))
        {
          string sid = oRS.Fields["STRUCTURE_ID"].Value.ToString().Trim();
          if (!string.IsNullOrEmpty(sid))
          {
            oCommonN.StructureID = sid;
            _obj.StructureID = sid;
          }
          else
          {
            oCommonN = null;
            LogMessage(_odiag, "GetCommonN", "COMMON_N", "FID = " + _ko.FID.ToString() + " has no structure_id.");
            return;
          }


          string sx = oRS.Fields["OGGX_H"].Value.ToString().Trim();
          if (!string.IsNullOrEmpty(sx))
          {
            oCommonN.StructureOGG_X1 = Convert.ToDouble(sx);
            _obj.StructureOGG_X1 = Convert.ToDouble(sx);
          }

          string sy = oRS.Fields["OGGY_H"].Value.ToString().Trim();
          if (!string.IsNullOrEmpty(sy))
          {
            _obj.StructureOGG_Y1 = Convert.ToDouble(sy);
          }

          string sz = oRS.Fields["OGGZ_H"].Value.ToString().Trim();
          if (!string.IsNullOrEmpty(sz))
          {
            oCommonN.StructureOGG_Z1 = Convert.ToDouble(sz);
            _obj.StructureOGG_Z1 = Convert.ToDouble(sx);
          }
        }

      }
      catch (Exception ex)
      {
        LogException(_odiag, "GetCommonN", ex);
        throw ex;
      }
    }

    public static string GetCommonNStructureID(IGTKeyObject _ko, GTDiagnostics _odiag)
    {
      string osid = string.Empty;

      try
      {
        IGTComponent commonN = _ko.Components["COMMON_N"];
        ADODB.Recordset oRS = commonN.Recordset;

        if (oRS == null) return osid;

        if (oRS.RecordCount == 0) return osid;

        oRS.MoveFirst();


        if ((!oRS.EOF) && (!oRS.BOF))
        {
          osid = oRS.Fields["STRUCTURE_ID"].Value.ToString().Trim();
          if (!string.IsNullOrEmpty(osid))
          {
            return osid;
          }
          else
          {
            LogMessage(_odiag, "GetCommonN", "COMMON_N", "FID = " + _ko.FID.ToString() + " has no structure_id.");
            return osid;
          }
        }
      }
      catch (Exception ex)
      {
        LogException(_odiag, "GetCommonN", ex);
        throw ex;
      }

      return osid;
    }
    #endregion CommonN

    #region AssetHistory

    public static List<AssetHistory> GetAllAssetHistory(IGTDataContext _odc, GTDiagnostics _odiag)
    {
      List<AssetHistory> assetlist = null;

      try
      {
        string sSQL = string.Empty;


        sSQL = "SELECT G3E_IDENTIFIER, G3E_FNO, G3E_FID, G3E_ANO, G3E_CNO, G3E_CID, CHANGE_OPERATION, STRUCTURE_ID_1, OGG_X_1, OGG_Y_1, OGG_Z_1, CHANGE_DATE, VALUE_OLD, VALUE_NEW FROM ASSET_HISTORY WHERE G3E_FNO = 56 OR G3E_FNO = 107 OR G3E_FNO = 110 OR G3E_FNO = 114 ORDER BY G3E_FID, CHANGE_DATE ASC";


        ADODB.Recordset oRS = null;

        oRS = ExecuteQuery(_odc, sSQL, _odiag);

        if (oRS == null) return null;
        if (oRS.RecordCount == 0) return null;

        if ((oRS.EOF) && (oRS.BOF)) return null;

        oRS.MoveFirst();

        assetlist = ProcessAssetHistoryRS(oRS, _odiag);

        if (assetlist == null) return null;

        if (assetlist.Count == 0) return null;
      }
      catch (Exception ex)
      {
        LogException(_odiag, "GetAllAssetHistory", ex);
        throw ex;
      }

      return assetlist;
    }

    public static List<AssetHistory> GetAssetHistoryBySID(IGTDataContext _odc, string _sid, GTDiagnostics _odiag)
    {
      List<AssetHistory> assetlist = null;

      try
      {
        string sSQL = string.Empty;


        sSQL = "SELECT G3E_IDENTIFIER, G3E_FNO, G3E_FID, G3E_ANO, G3E_CNO, G3E_CID, CHANGE_OPERATION, STRUCTURE_ID_1, OGG_X_1, OGG_Y_1, OGG_Z_1, CHANGE_DATE, VALUE_OLD, VALUE_NEW FROM ASSET_HISTORY WHERE STRUCTURE_ID_1 = '" + _sid + "' ORDER BY G3E_FID, CHANGE_DATE ASC";


        ADODB.Recordset oRS = null;

        oRS = ExecuteQuery(_odc, sSQL, _odiag);

        if (oRS == null) return null;
        if (oRS.RecordCount == 0) return null;

        if ((oRS.EOF) && (oRS.BOF)) return null;

        oRS.MoveFirst();

        assetlist = ProcessAssetHistoryRS(oRS, _odiag);

        if (assetlist == null) return null;

        if (assetlist.Count == 0) return null;
      }
      catch (Exception ex)
      {
        LogException(_odiag, "GetAllAssetHistory", ex);
        throw ex;
      }

      return assetlist;
    }

    public static List<AssetHistory> GetAssetHistoryByFID(IGTDataContext _odc, int _fid, GTDiagnostics _odiag)
    {
      List<AssetHistory> assetlist = null;

      try
      {
        string sSQL = string.Empty;


        sSQL = "SELECT G3E_IDENTIFIER, G3E_FNO, G3E_FID, G3E_ANO, G3E_CNO, G3E_CID, CHANGE_OPERATION, STRUCTURE_ID_1, OGG_X_1, OGG_Y_1, OGG_Z_1, CHANGE_DATE, VALUE_OLD, VALUE_NEW FROM ASSET_HISTORY WHERE STRUCTURE_ID_1 = " + _fid + " ORDER BY G3E_FID, CHANGE_DATE ASC";


        ADODB.Recordset oRS = null;

        oRS = ExecuteQuery(_odc, sSQL, _odiag);

        if (oRS == null) return null;
        if (oRS.RecordCount == 0) return null;

        if ((oRS.EOF) && (oRS.BOF)) return null;

        oRS.MoveFirst();

        assetlist = ProcessAssetHistoryRS(oRS, _odiag);

        if (assetlist == null) return null;

        if (assetlist.Count == 0) return null;
      }
      catch (Exception ex)
      {
        LogException(_odiag, "GetAllAssetHistory", ex);
        throw ex;
      }

      return assetlist;
    }
    private static List<AssetHistory> ProcessAssetHistoryRS(ADODB.Recordset _ors, GTDiagnostics _odiag)
    {
      List<AssetHistory> histList = null;
	  int g3eFid ;

      try
      {
        AssetHistory obj = null;
        histList = new List<AssetHistory>();

        while ((!_ors.EOF) && (!_ors.BOF))
        {
          obj = new AssetHistory();
          obj.G3E_FID = Convert.ToInt32(_ors.Fields["G3E_FID"].Value.ToString().Trim());
		  g3eFid = obj.G3E_FID;
          obj.G3E_FNO = Convert.ToInt16(_ors.Fields["G3E_FNO"].Value.ToString().Trim());

          string sCNO = _ors.Fields["G3E_CNO"].Value.ToString().Trim();
          if (!string.IsNullOrEmpty(sCNO)) obj.G3E_CNO = Convert.ToInt16(sCNO);
          else obj.G3E_CNO = 0;

          string sANO = _ors.Fields["G3E_ANO"].Value.ToString().Trim();
          if (!string.IsNullOrEmpty(sANO)) obj.G3E_ANO = Convert.ToInt32(sANO);
          else obj.G3E_ANO = 0;


          string sCID = _ors.Fields["G3E_CID"].Value.ToString().Trim();
          if (!string.IsNullOrEmpty(sCID)) obj.G3E_CID = Convert.ToInt32(sCID);
          else obj.G3E_CID = 0;

          obj.StructureID_1 = _ors.Fields["STRUCTURE_ID_1"].Value.ToString().Trim();

          string sx = _ors.Fields["OGG_X_1"].Value.ToString().Trim();
          if (string.IsNullOrEmpty(sx))
          {
            LogMessage(_odiag, "ProcessAssetHistoryRS", "LocationXYZ", "No location for x value for fid = " + obj.G3E_FID.ToString());
            obj = null;
           
          }
          string sy = _ors.Fields["OGG_Y_1"].Value.ToString().Trim();
          if (string.IsNullOrEmpty(sy))
          {
            LogMessage(_odiag, "ProcessAssetHistoryRS", "LocationXYZ", "No location for y value for fid = " + g3eFid);
            obj = null;
           
          }
		  if(obj!=null)
		  {
		  	  obj.OGG_X1 = Convert.ToDouble(sx);
              obj.OGG_Y1 = Convert.ToDouble(sy);
          

          string sz = _ors.Fields["OGG_Z_1"].Value.ToString().Trim();
          if (!string.IsNullOrEmpty(sz))
          {
            obj.OGG_Z1 = Convert.ToDouble(sz);
          }
          else
          {
            obj.OGG_Z1 = 0;
          }

          string oldValue = _ors.Fields["VALUE_OLD"].Value.ToString().Trim();
          if (!string.IsNullOrEmpty(oldValue))
          {
            obj.OLD_VALUE = oldValue;
          }

          string newValue = _ors.Fields["VALUE_NEW"].Value.ToString().Trim();
          if (!string.IsNullOrEmpty(newValue))
          {
            obj.NEW_VALUE = newValue;
          }

          obj.ChangeOperation = _ors.Fields["CHANGE_OPERATION"].Value.ToString().Trim();
          string sChangeDate = _ors.Fields["CHANGE_DATE"].Value.ToString().Trim();
          DateTime changedate;
          DateTime.TryParse(sChangeDate, out changedate);
          obj.ChangeDate = changedate;

          histList.Add(obj);
}
          _ors.MoveNext();
        }

        if (histList.Count == 0) return null;
      }
      catch (Exception ex)
      {
        LogException(_odiag, "ProcessAssetHistoryRS", ex);
        throw ex;
      }

      return histList;
    }

    #endregion AssetHistory

    #region Lambda Utilities

    public static bool CompareLists<T>(List<T> aListA, List<T> aListB)
    {
      if (aListA == null || aListB == null || aListA.Count != aListB.Count)
        return false;
      if (aListA.Count == 0)
        return true;
      Dictionary<T, int> lookUp = new Dictionary<T, int>();
      // create index for the first list
      for (int i = 0; i < aListA.Count; i++)
      {
        int count = 0;
        if (!lookUp.TryGetValue(aListA[i], out count))
        {
          lookUp.Add(aListA[i], 1);
          continue;
        }
        lookUp[aListA[i]] = count + 1;
      }
      for (int i = 0; i < aListB.Count; i++)
      {
        int count = 0;
        if (!lookUp.TryGetValue(aListB[i], out count))
        {
          // early exit as the current value in B doesn't exist in the lookUp (and not in ListA)
          return false;
        }
        count--;
        if (count <= 0)
          lookUp.Remove(aListB[i]);
        else
          lookUp[aListB[i]] = count;
      }
      // if there are remaining elements in the lookUp, that means ListA contains elements that do not exist in ListB
      return lookUp.Count == 0;
    }

    public static int GetDistance(double xPt1, double yPt1, double zPt1, double xPt2, double yPt2, double zPt2, GTDiagnostics _diag)
    {
      int iDistance = 0;


      try
      {
        //Distance in meters
        double _dist = Math.Sqrt(Math.Pow((xPt1 - xPt2), 2) + Math.Pow((yPt1 - yPt2), 2)) / 1.6093;
        //Convert to feet

        _dist = _dist / 0.3048;

        iDistance = (int)_dist;
      }
      catch (Exception ex)
      {
        LogException(_diag, "CommandUtilities.GetDistance", ex);
        throw ex;
      }



      return iDistance;
    }
    #endregion Lambda Utilities
  }
}
