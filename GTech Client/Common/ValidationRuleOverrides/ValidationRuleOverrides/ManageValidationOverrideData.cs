using System;
using ADODB;
using Intergraph.GTechnology.API;

// WR_VALIDATION_OVERRIDE table
//ID                  NUMBER(10) (Always Generated Identity)
//RULE_ID             VARCHAR2(10 Byte)
//RULE_NM             VARCHAR2(50 Byte)
//G3E_IDENTIFIER      VARCHAR2(30 Byte)
//G3E_FNO             NUMBER(5)
//G3E_FID             NUMBER(10)
//STRUCTURE_ID        VARCHAR2(16 Byte)
//ERROR_MSG           VARCHAR2(400 Byte)
//OVERRIDE_UID        VARCHAR2(30 Byte)
//OVERRIDE_COMMENTS   VARCHAR2(1000 Byte)
//OVERRIDE_D          DATE (Default: SYSDATE)

// Sample Recordset
//Name: ErrorPriority      Value: P1
//Name: ErrorDescription   Value: The Common Use attribute of component Pole Attributes is required
//Name: ErrorLocation      Value: 
//Name: Connection         Value: GIS
//Name: G3E_FNO            Value: 110
//Name: G3E_FID            Value: 931000433
//Name: G3E_CNO            Value: 11001
//Name: G3E_CID            Value: 1
//Name: OVERRIDE_COMMENTS  Value: 45

namespace GTechnology.Oncor.CustomAPI
{
  public class ManageValidationOverrideData
  {

    public ManageValidationOverrideData()
    {
    }

    /// <summary>
    /// Reconciles override data to the WR_VALIDATION_OVERRIDE table
    /// </summary>
    /// <param name="rs">Recordset containing override data</param>
    /// <returns>true if update is successful; else, false</returns>
    public bool UpdateValidationOverrides(Recordset rs)
    {
      try
      {
        bool retVal = true;

        // This would be very abnormal, but if we get an empty/null recordset,
        // then raise it as an exception.
        if(null == rs || 0 == rs.RecordCount)
        {
          throw new Exception("Empty recordset was sent to this method.");
        }

        // For each record, add it to WR_VALIDATION_OVERRIDE if it doesn't already exist
        // or update the existing record.
        rs.MoveFirst();
        do
        {
          //for(int i = 0;i < rs.Fields.Count;i++)
          //{
          //  System.Diagnostics.Debug.Print(string.Format("Name: {0},           Value: {1}", rs.Fields[i].Name, rs.Fields[i].Value.ToString()));
          //}
          //System.Diagnostics.Debug.Print("----------------------------------------------------------------------------------------------------------");


          // ErrorDescription contains the string we need to use to determine RULE_ID and RULE_NM (from WR_VALIDATION_RULE)
          string errorDescription = rs.Fields["errordescription"].Value.ToString();
                    string ruleID = string.Empty;
                    // Get the Rule Name (or empty string if Rule ID is blank).
                    string ruleName = string.Empty;

                    GetRuleIDRuleMsg(errorDescription, out ruleID, out ruleName);

          // If the override already exists, just update its override comments; else, insert a new record for the override.
          int FID = Convert.ToInt32(rs.Fields["g3e_fid"].Value);
          short FNO = Convert.ToInt16(rs.Fields["g3e_fno"].Value);
          string structureID = StructureIDbyFID(FID);
          string overrideComments = rs.Fields["override_comments"].Value.ToString();

          // Override ID is the key into the WR_VALIDATION_OVERRIDE table.
          int overrideID = GetOverrideID(FID, FNO, structureID, errorDescription, ruleID);

          if(-1 < overrideID)
          {
            // overrideID will be -1 if the record doesn't exist so, in this case, perform an update.
            UpdateOverrideComments(overrideID, overrideComments);
          }
          else
          {
            // This is a new override.  Insert it.
            NewOverride(ruleID, ruleName, FNO, FID, structureID, errorDescription, overrideComments);
          }

          rs.MoveNext();
        } while(!rs.EOF);


        return retVal;
      }
      catch(Exception ex)
      {
        string exMsg = string.Format("Error occurred in {0} of {1}.{2}{3}", System.Reflection.MethodBase.GetCurrentMethod().Name, this.ToString(), Environment.NewLine, ex.Message);
        throw new Exception(exMsg);
      }
    }
        
        private void GetRuleIDRuleMsg(string p_errDescription, out string p_RuleID, out string p_RuleName)
        {
            p_RuleID = string.Empty;
            p_RuleName = string.Empty;

            try
            {
                IGTApplication app = GTClassFactory.Create<IGTApplication>();
                string sql = "select rule_nm, rule_id from wr_validation_rule where rule_msg=?";
                Recordset rs = app.DataContext.OpenRecordset(sql, CursorTypeEnum.adOpenStatic, LockTypeEnum.adLockReadOnly, (int)CommandTypeEnum.adCmdText, p_errDescription);

                if (rs != null)
                {
                    if (rs.RecordCount > 0)
                    {
                        rs.MoveFirst();
                        p_RuleName = Convert.ToString(rs.Fields["rule_nm"].Value);
                        p_RuleID = Convert.ToString(rs.Fields["rule_id"].Value);
                    }
                }
            }
            catch (Exception ex)
            {
                string exMsg = string.Format("Error occurred in {0} of {1}.{2}{3}", System.Reflection.MethodBase.GetCurrentMethod().Name, this.ToString(), Environment.NewLine, ex.Message);
                throw new Exception(exMsg);
            }            
        }
    /// <summary>
    /// Returns the Structure ID of a feature based on its FID value
    /// </summary>
    /// <param name="FID">G3E_FID value</param>
    /// <returns>Structure ID</returns>
    private string StructureIDbyFID(int FID)
    {
      try
      {
        string retVal = string.Empty;

        IGTApplication app = GTClassFactory.Create<IGTApplication>();
        string sql = "select structure_id from common_n where g3e_fid=?";
        Recordset rs = app.DataContext.OpenRecordset(sql, CursorTypeEnum.adOpenStatic, LockTypeEnum.adLockReadOnly, (int)CommandTypeEnum.adCmdText, FID);
        if(null != rs && 1 == rs.RecordCount)
        {
          rs.MoveFirst();
          if(DBNull.Value != rs.Fields[0].Value)
          {
            retVal = rs.Fields[0].Value.ToString();
          }
        }

        if(null != rs)
        {
          if(rs.State != Convert.ToInt32(ObjectStateEnum.adStateClosed))
          {
            rs.Close();
          }
          rs = null;
        }

        return retVal;
      }
      catch(Exception ex)
      {
        string exMsg = string.Format("Error occurred in {0} of {1}.{2}{3}", System.Reflection.MethodBase.GetCurrentMethod().Name, this.ToString(), Environment.NewLine, ex.Message);
        throw new Exception(exMsg);
      }
    }

    /// <summary>
    /// Gets the value for WR_VALIDATION_OVERRIDE.ID based on given parameter values.
    /// </summary>
    /// <returns>WR_VALIDATION_OVERRIDE.ID if record is found; else, -1</returns>
    private int GetOverrideID(int FID, short FNO, string StructureID, string ErrorDescription, string RuleID)
    {
      try
      {
        int id = -1;

        IGTApplication app = GTClassFactory.Create<IGTApplication>();
        bool haveRuleID = !string.IsNullOrEmpty(RuleID);

        string sql = "select id from wr_validation_override where";
        sql += " g3e_fid=?";
        sql += " and g3e_fno=?";
        sql += " and g3e_identifier=?";
        sql += " and structure_id=?";
        sql += " and error_msg=?";
        sql += " and override_uid=user";

        object[] dbParams;

        if(haveRuleID)
        {
          sql += " and rule_id=?";
          dbParams = new object[] { FID, FNO, app.DataContext.ActiveJob, StructureID, ErrorDescription, RuleID };
        }
        else
        {
          dbParams = new object[] { FID, FNO, app.DataContext.ActiveJob, StructureID, ErrorDescription };
        }

        Recordset rs = app.DataContext.OpenRecordset(sql, CursorTypeEnum.adOpenStatic, LockTypeEnum.adLockReadOnly, (int)CommandTypeEnum.adCmdText, dbParams);

        if(null != rs && 1 == rs.RecordCount)
        {
          id = Convert.ToInt32(rs.Fields[0].Value);
        }

        if(null != rs)
        {
          if(rs.State != Convert.ToInt32(ObjectStateEnum.adStateClosed))
          {
            rs.Close();
          }
          rs = null;
        }

        return id;
      }
      catch(Exception ex)
      {
        string exMsg = string.Format("Error occurred in {0} of {1}.{2}{3}", System.Reflection.MethodBase.GetCurrentMethod().Name, this.ToString(), Environment.NewLine, ex.Message);
        throw new Exception(exMsg);
      }
    }

    /// <summary>
    /// Updates existing override comments and date
    /// </summary>
    /// <param name="ID">WR_VALIDATION_OVERRIDE.ID</param>
    /// <param name="OverrideComments">Validation Override Comments string</param>
    private void UpdateOverrideComments(int ID, string OverrideComments)
    {
      try
      {
        // Update the record based on its ID and whether the comments no longer match.
        string sql = "update wr_validation_override set override_comments=?, override_d=sysdate where id=? and override_comments!=?";
        IGTApplication app = GTClassFactory.Create<IGTApplication>();
        app.DataContext.Execute(sql, out int recs, (int)CommandTypeEnum.adCmdText, OverrideComments, ID, OverrideComments);

        // The RecordsAffected parameter does not return the number of records edited and, in this case,
        // is always zero when successful; however, to ensure the record was updated, query for it...

        sql = "select count(1) from wr_validation_override where id=? and override_comments=?";
        Recordset rs = app.DataContext.OpenRecordset(sql, CursorTypeEnum.adOpenStatic, LockTypeEnum.adLockReadOnly, (int)CommandTypeEnum.adCmdText, ID, OverrideComments);

        if(null != rs && 1 == rs.RecordCount)
        {
          app.DataContext.Execute("commit", out recs, (int)CommandTypeEnum.adCmdText);
        }
        else
        {
          // Given the ID was evaluated just prior to executing this code, this should never occur but...
          app.DataContext.Execute("rollback", out recs, (int)CommandTypeEnum.adCmdText);
          throw new Exception("Did not update validation override comments successfully.");
        }

        if(null != rs)
        {
          if(rs.State != Convert.ToInt32(ObjectStateEnum.adStateClosed))
          {
            rs.Close();
          }
          rs = null;
        }

      }
      catch(Exception ex)
      {
        string exMsg = string.Format("Error occurred in {0} of {1}.{2}{3}", System.Reflection.MethodBase.GetCurrentMethod().Name, this.ToString(), Environment.NewLine, ex.Message);
        throw new Exception(exMsg);
      }
    }

    /// <summary>
    /// Inserts a new override into WR_VALIDATION_OVERRIDE
    /// </summary>
    private void NewOverride(string RuleID, string RuleName, short FNO, int FID, string StructureID, string ErrorDescription, string OverrideComments)
    {
      try
      {
        string sql = "insert into wr_validation_override";
        sql += " columns(rule_id,rule_nm,g3e_identifier,g3e_fno,g3e_fid,structure_id,error_msg,override_comments,override_uid)";
        sql += "values(?,?,?,?,?,?,?,?,user)";
        IGTApplication app = GTClassFactory.Create<IGTApplication>();
        object[] dbParams = new object[] { RuleID, RuleName, app.DataContext.ActiveJob, FNO, FID, StructureID, ErrorDescription, OverrideComments };
        for(int i = 0;i < dbParams.Length;i++)
        {
          System.Diagnostics.Debug.Print(dbParams[i].ToString());
        }
        app.DataContext.Execute(sql, out int recs, (int)CommandTypeEnum.adCmdText, dbParams);
        app.DataContext.Execute("commit", out recs, (int)CommandTypeEnum.adCmdText);
      }
      catch(Exception ex)
      {
        string exMsg = string.Format("Error occurred in {0} of {1}.{2}{3}", System.Reflection.MethodBase.GetCurrentMethod().Name, this.ToString(), Environment.NewLine, ex.Message);
        throw new Exception(exMsg);
      }
    }
  }
}

