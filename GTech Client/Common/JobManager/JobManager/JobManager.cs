using System;
using System.Collections.Generic;
using Intergraph.GTechnology.API;
using ADODB;

namespace GTechnology.Oncor.CustomAPI
{
  /// <summary>
  /// Wrapper around some of the JobManagementService functions
  /// to aid in job-related functions specific to Oncor.
  /// </summary>
  public class JobManager
  {
    private IGTApplication app = null;
    private IGTDataContext dc = null;

    /// <summary>
    /// Constructor for the JobManager class
    /// </summary>
    public JobManager()
    {
      // This is a local App object.
      app = GTClassFactory.Create<IGTApplication>();
      dc = app.DataContext;
    }

    /// <summary>
    /// Flag to indicate a NULL numeric field was retrieved.
    /// </summary>
    public bool NullNumericFieldValue
    {
      get; set;
    }

    /// <summary>
    /// Returns the active job name or null if no active job exists.
    /// </summary>
    public string ActiveJob
    {
      get
      {
        return dc.ActiveJob;
      }
    }

    /// <summary>
    /// Gets the string value for FieldName from the active job.
    /// </summary>
    /// <param name="FieldName">Field in G3E_JOB</param>
    /// <returns>string field value</returns>
    private string strJobFieldValue(string FieldName)
    {
      if(string.IsNullOrEmpty(ActiveJob))
      {
        throw new Exception(string.Format("Job must be active to retrieve the value for {0}.", FieldName));
      }

      string sql = string.Format("select {0} from g3e_job where g3e_identifier=?", FieldName);
      Recordset rs = dc.OpenRecordset(sql, CursorTypeEnum.adOpenStatic, LockTypeEnum.adLockReadOnly, (int)CommandTypeEnum.adCmdText, dc.ActiveJob);

      if(null != rs && rs.RecordCount == 1)
      {
        string tmp = string.Empty;

        if(System.DBNull.Value != rs.Fields[0].Value)
        {
          tmp = rs.Fields[0].Value.ToString();
        }

        rs.Close();
        return tmp;
      }
      else
      {
        if(null != rs)
        {
          rs.Close();
          rs = null;
        }

        throw new Exception(string.Format("Unable to retrieve distinct value for {0} for the active job.", FieldName));
      }
    }

    /// <summary>
    /// Gets the int value for FieldName (a NUMBER datatype column) from the active job.
    /// </summary>
    /// <param name="FieldName">Field in G3E_JOB</param>
    /// <returns>int field value</returns>
    private int intJobFieldValue(string FieldName)
    {
      if(string.IsNullOrEmpty(ActiveJob))
      {
        throw new Exception(string.Format("Job must be active to retrieve the value for {0}.", FieldName));
      }

      this.NullNumericFieldValue = false;

      string sql = string.Format("select {0} from g3e_job where g3e_identifier=?", FieldName);
      Recordset rs = dc.OpenRecordset(sql, CursorTypeEnum.adOpenStatic, LockTypeEnum.adLockReadOnly, (int)CommandTypeEnum.adCmdText, dc.ActiveJob);

      if(null != rs && rs.RecordCount == 1)
      {
        int tmp = 0;

        if(System.DBNull.Value != rs.Fields[0].Value)
        {
          tmp = Convert.ToInt32(rs.Fields[0].Value);
        }
        else
        {
          this.NullNumericFieldValue = true;
        }

        rs.Close();
        return tmp;
      }
      else
      {
        if(null != rs && rs.State == Convert.ToInt32(ObjectStateEnum.adStateOpen))
        {
          rs.Close();
          rs = null;
        }

        throw new Exception(string.Format("Unable to retrieve distinct value for {0} for the active job.", FieldName));
      }
    }

    /// <summary>
    /// Gets or sets the value of G3E_JOB.WMIS_STATUS_C for the active job.
    /// </summary>
    public string WMISStatus
    {
      get
      {
        return strJobFieldValue("wmis_status_c");
      }
      set
      {
        if(string.IsNullOrEmpty(ActiveJob))
        {
          throw new Exception("Job must be active to set the WMIS status.");
        }
        UpdateJobField(ActiveJob, "wmis_status_c", value);
      }
    }

    /// <summary>
    /// Gets the WR Number for the active job.
    /// </summary>
    public int WRNumber
    {
      get { return intJobFieldValue("wr_nbr"); }
    }

    /// <summary>
    /// Gets or sets the value of G3E_JOB.G3E_JOBTYPE for the active job.
    /// </summary>
    public string JobType
    {
      get
      {
        return strJobFieldValue("g3e_jobtype");
      }
      set
      {
        if(string.IsNullOrEmpty(ActiveJob))
        {
          throw new Exception("Job must be active to set its Job Type.");
        }
        UpdateJobField(ActiveJob, "g3e_jobtype", value);
      }
    }

    /// <summary>
    /// Gets or sets the Write Back Needed flag for the active job.  Y = true, N = false.
    /// </summary>
    public bool WriteBackNeeded
    {
      get
      {
        return 0 == string.Compare(strJobFieldValue("writeback_needed_yn").ToUpper(), "Y");
      }
      set
      {
        if(string.IsNullOrEmpty(ActiveJob))
        {
          throw new Exception("Job must be active to set its Write Back Needed flag");
        }
        UpdateJobField(ActiveJob, "writeback_needed_yn", value ? "Y" : "N");
      }
    }

		/// <summary>
		/// Returns the value of G3E_JOB.DESIGNER_UID for the active job.
		/// </summary>
		public string DesignerUID
		{
			get
			{
				return strJobFieldValue("designer_uid");
			}
		}

		/// <summary>
		/// Returns the value of G3E_JOB.DESIGNER_RACFID for the active job.
		/// </summary>
		public string DesignerRACFID
		{
			get
			{
				return strJobFieldValue("designer_racfid");
			}
		}

		/// <summary>
		/// Gets or sets the value of G3E_JOB.G3E_JOBSTATUS for the active job.
		/// </summary>
		public string JobStatus
    {
      get
      {
        return strJobFieldValue("g3e_jobstatus");
      }
      set
      {
        if(string.IsNullOrEmpty(ActiveJob))
        {
          throw new Exception("Job must be active to set its Job status.");
        }
        UpdateJobField(ActiveJob, "g3e_jobstatus", value);
      }
    }

    /// <summary>
    /// Returns true if any alternates to the active job have Job Status = ApprovalPending.
    /// </summary>
    public bool AlternatesExistPendingApproval
    {
      get
      {
        if(string.IsNullOrEmpty(ActiveJob))
        {
          throw new Exception("Job must be active to retrieve alternate job information.");
        }

        // Get the list of alternates and return true if any exist.
        return 0 < AlternatesPendingApproval.Count;
      }
    }

    /// <summary>
    /// Returns a list of alternate jobs that are pending approval.  There should only be one entry in the list.
    /// </summary>
    public List<string> AlternatesPendingApproval
    {
      get
      {
        List<string> alts = new List<string>();

        if(string.IsNullOrEmpty(ActiveJob))
        {
          throw new Exception("Job must be active to retrieve alternate job information.");
        }

        string SQL = string.Format("select g3e_identifier from g3e_job where g3e_identifier like '{0}-%' and upper(g3e_jobstatus)='APPROVALPENDING'", ActiveJob.Split('-')[0]);
        Recordset rs = dc.OpenRecordset(SQL, CursorTypeEnum.adOpenStatic, LockTypeEnum.adLockReadOnly, (int)CommandTypeEnum.adCmdText);

        // There should be only one alternate pending approval
        // but returning a list just in case something has allowed multiples.
        if(null != rs && 0 < rs.RecordCount)
        {
          do
          {
            alts.Add(rs.Fields[0].Value.ToString());
            rs.MoveNext();
          } while(!rs.EOF);

          rs.Close();
          rs = null;
        }

        return alts;
      }
    }

    /// <summary>
    /// Returns true if there are any alternates for the active job
    /// </summary>
    public bool AlternatesExist
    {
      get
      {
        if(string.IsNullOrEmpty(ActiveJob))
        {
          throw new Exception("Job must be active to retrieve alternate job information.");
        }

        bool retVal = false;
        if(null != Alternates && 0 < Alternates.Count)
        {
          retVal = true;
        }

        return retVal;
      }
    }

    /// <summary>
    /// Returns a list of alternate jobs if any exist for the active job.
    /// </summary>
    public List<string> Alternates
    {
      get
      {
        List<string> alts = new List<string>();

        if(string.IsNullOrEmpty(ActiveJob))
        {
          throw new Exception("Job must be active to retrieve alternate job information.");
        }

        // Note: Parameterizing this query (i.e. '?%') was not returning records.  Changed to simple formatted string.
        string SQL = string.Format("select g3e_identifier from g3e_job where g3e_identifier like '{0}%' order by 1", ActiveJob.Split('-')[0]);
        Recordset rs = dc.OpenRecordset(SQL, CursorTypeEnum.adOpenStatic, LockTypeEnum.adLockReadOnly, (int)CommandTypeEnum.adCmdText);

        if(null != rs && 0 < rs.RecordCount)
        {
          do
          {
            alts.Add(rs.Fields[0].Value.ToString());
            rs.MoveNext();
          } while(!rs.EOF);

          rs.Close();
          rs = null;
        }

        return alts;
      }
    }
    /// <summary>
    /// Activates the specified job
    /// </summary>
    /// <param name="JobName">Job to activate.</param>
    public void ActivateJob(string JobName)
    {
      IGTJobManagementService jms = GTClassFactory.Create<IGTJobManagementService>();
      jms.DataContext = app.DataContext;
      jms.EditJob(JobName);
    }

    /// <summary>
    /// Deactivates the active job
    /// </summary>
    /// <param name="ViewJob">If true, keeps the active job in the viewed job list.</param>
    public void DeactivateJob(bool ViewJob)
    {
      IGTJobManagementService jms = GTClassFactory.Create<IGTJobManagementService>();
      jms.DataContext = app.DataContext;
      string activeJob = jms.DataContext.ActiveJob;
      if(!string.IsNullOrEmpty(activeJob))
      {
        jms.EndEditJob();
        // Until the custom interface that's using this object returns control to G/Tech,
        // the previously-active job still apears as active. In the interim, removing it as a viewed job
        // will cause it to be completely deactivated (e.g. GTDataContext.ActiveJob will be NULL at that point).
        jms.RemoveJob(activeJob);

        // If RemoveFromList is false, since we've already removed it (as the workaround),
        // then we need to add it back.
        if(ViewJob)
        {
          jms.ViewJob(activeJob);
        }
      }
    }

    /// <summary>
    /// Updates a given field in the G3E_JOB table.
    /// </summary>
    /// <param name="jobID">Job ID (G3E_IDENTIFIER)</param>
    /// <param name="fieldName">Field to update</param>
    /// <param name="fieldValue">Value to assign to field</param>
    /// <returns>Contains exception message string.</returns>
    public string UpdateJobField(string jobID, string fieldName, string fieldValue)
    {
      string SQL = string.Format("update g3e_job set {0}=? where g3e_identifier=?", fieldName);
      dc.Execute(SQL, out int outRecs, (int)CommandTypeEnum.adCmdText + (int)ExecuteOptionEnum.adExecuteNoRecords, fieldValue, jobID);

      string exception = string.Empty;

      switch(outRecs)
      {
        case 0:
          exception = string.Format("Unable to locate job record for job: {0}.", ActiveJob);
          break;
        case 1:
          break;
        default:
          dc.Execute("rollback", out outRecs, (int)CommandTypeEnum.adCmdText);
          exception = string.Format("More than one job record found for job: {0}.  Update rolled back.", ActiveJob);
          break;
      }

      return exception;
    }

    /// <summary>
    /// Returns a recordset containing the Pending Edits for the active job.
    /// </summary>
    /// <returns>Recordset</returns>
    public Recordset PendingEdits
    {
      get
      {
        Recordset pe = null;

        // Refresh the pending edits for the active job.
        string sql = "begin ltt_user.findpendingedits;end;";
        dc.Execute(sql, out int recs, (int)CommandTypeEnum.adCmdText);

        // Get a recordset of the pending edits.
        //sql = "select * from pendingedits p join ltt_identifiers l on p.ltt_id=l.ltt_id where l.ltt_name=?";
        // The above statement produces an XML error.  Changed to below to avoid that (but don't know the error is produced...)
        sql = "select * from pendingedits where ltt_id=(select ltt_id from ltt_identifiers where ltt_name=?)";
        pe = dc.OpenRecordset(sql, CursorTypeEnum.adOpenStatic, LockTypeEnum.adLockReadOnly, (int)CommandTypeEnum.adCmdText, this.ActiveJob);

        return pe;
      }
    }

    /// <summary>
    /// Checks G3E_JOB for existence of a single occurrence of the active job.
    /// </summary>
    /// <returns>Error string if an error is encountered; otherwise string.Empty</returns>
    private string JobRecordExists()
    {
      string SQL = "select count(1) from g3e_job where g3e_identifier=?";
      Recordset rs = dc.OpenRecordset(SQL, CursorTypeEnum.adOpenStatic, LockTypeEnum.adLockReadOnly, (int)CommandTypeEnum.adCmdText, ActiveJob);

      string exception = string.Empty;

      if(null != rs)
      {
        switch(rs.RecordCount)
        {
          case 0:
            exception = string.Format("Unable to locate job record for job: {0}.", ActiveJob);
            break;
          case 1:
            // This is the expected case so nothing to do here.
            break;
          default:
            exception = string.Format("Multiple job records found for job: {0}.", ActiveJob);
            break;
        }

        rs.Close();
        rs = null;
      }
      return exception;
    }

  }
}
