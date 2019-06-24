using System;
using System.Collections.Generic;
using Intergraph.GTechnology.Interfaces;
using Intergraph.GTechnology.API;
using System.Windows.Forms;
using ADODB;

namespace GTechnology.Oncor.CustomAPI
{
	public class ccSessionManagement : IGTCustomCommandModal
	{
		public IGTTransactionManager TransactionManager
		{
			set { }
		}

		private bool InteractiveMode = false;

		public void Activate()
		{

			// If a job is active when the workspace is opened, then the ActiveJobChanged event is not fired.
			// To ensure the code in that event is called under these circunstances, detect when a job is
			// active when this command is called and call the same event handler that's called when
			// the active job is changed.
			IGTApplication app = GTClassFactory.Create<IGTApplication>();
			if(!string.IsNullOrEmpty(app.DataContext.ActiveJob))
			{
				// Job is active when command is called.
				GTActiveJobChangedEventArgs eventArgs = new GTActiveJobChangedEventArgs(app.DataContext.ActiveJob, app.DataContext.ConfigurationName);
				App_ActiveJobChanged(app, eventArgs);
			}

			IGTApplication App = GTClassFactory.Create<IGTApplication>();
			App.ActiveJobChanged += App_ActiveJobChanged;   //App_ActiveJobChanged method is called when App.ActiveJobChanged is triggered

			// Cache the GUI Mode as it doesn't have to be evaluated each time for this command.
			GUIMode guiMode = new GUIMode();
			this.InteractiveMode = guiMode.InteractiveMode;
		}

		private void App_ActiveJobChanged(object sender, GTActiveJobChangedEventArgs e)
		{
			string jobStatus = null;
			string wmisStatus = null;
			string Job = e.ActiveJob;

			if(string.IsNullOrEmpty(Job))  //if no job is active, do nothing
			{
				return;
			}

			IGTApplication app = (IGTApplication)sender;
			IGTDataContext dc = app.DataContext;
			string SQL = "select G3E_JOBSTATUS, WMIS_STATUS_C , G3E_ENABLELOGGING FROM G3E_JOB WHERE G3E_IDENTIFIER=?";
			ADODB.Recordset RS = dc.OpenRecordset(SQL, ADODB.CursorTypeEnum.adOpenStatic, ADODB.LockTypeEnum.adLockReadOnly, (int)ADODB.CommandTypeEnum.adCmdText, Job);

			if(RS != null && RS.RecordCount == 1)
			{

				RefreshLoggingProperty(app, Convert.ToString(RS.Fields["G3E_ENABLELOGGING"].Value));

				//this section handles various WMIS statuses, then deactivates the job and skips all remaining steps.
				wmisStatus = Convert.ToString(RS.Fields["WMIS_STATUS_C"].Value).ToUpper().Trim();

				if(wmisStatus != "SUCCESS" && wmisStatus != "FAILURE" && !string.IsNullOrEmpty(wmisStatus))
				{
					if(InteractiveMode)
					{
						switch(wmisStatus)
						{
							case "WRITEBACK":
								MessageBox.Show("Job cannot be activated while a writeback operation is in progress.", "G/Technology", MessageBoxButtons.OK);
								break;

							case "BATCH":
								MessageBox.Show("Job cannot be activated while a batch operation is in progress.", "G/Technology", MessageBoxButtons.OK);
								break;

							case "STATUSCHG":
								MessageBox.Show("Job cannot be activated while a status change is being communicated to WMIS.", "G/Technology", MessageBoxButtons.OK);
								break;

							case "ERROR":
								MessageBox.Show("Job cannot be activated due to an outstanding error; please notify Support.", "G/Technology", MessageBoxButtons.OK);
								break;

							default:
								break;
						}
					}

					DeactivateJob(Job);        //deactivate job
					return;                    //skip remaining actions (job status section)
				}


				//this section handles various job statuses
				jobStatus = Convert.ToString(RS.Fields["G3E_JOBSTATUS"].Value).ToUpper().Trim();

				if(!string.IsNullOrEmpty(jobStatus))
				{
					switch(jobStatus)
					{
						case "APPROVALPENDING":

							if(InteractiveMode)
							{
								DialogResult ApprovalPendingResult = MessageBox.Show("WR is marked for approval. Unmark?", "G/Technology", MessageBoxButtons.YesNo);
								if(ApprovalPendingResult == DialogResult.Yes)
								{
									//change job status to Design and leave job active
									WMISStatus wMISStatus = new WMISStatus();
									wMISStatus.SetJobStatus(Job, "Design");
								}
								if(ApprovalPendingResult == DialogResult.No)
								{
									MessageBox.Show("Job is being deactivated and the WR will remain marked for approval.", "G/Technology", MessageBoxButtons.OK);
									DeactivateJob(Job);        //deactivate job
								}
							}
							else // UnattendedMode
							{
								//change job status to Design and leave job active
								WMISStatus wMISStatus = new WMISStatus();
								wMISStatus.SetJobStatus(Job, "Design");
							}
							break;

						case "DESIGN":
							bool hasAlternateDesign = false; //initialize to false. This will be set in the if statements below if an alternate design exists for the job.

							//this if/else determines if the active job contains a "-" character. Whether this is true or not determines what the SQL statement looks like.
							if(Job.Contains("-"))
							{
								string jobSubstring = Job.Substring(0, Job.IndexOf("-"));
								SQL = "select G3E_IDENTIFIER, G3E_JOBSTATUS from G3E_JOB where G3E_IDENTIFIER like ? and G3E_JOBSTATUS=?";
								RS = dc.OpenRecordset(SQL, ADODB.CursorTypeEnum.adOpenStatic, ADODB.LockTypeEnum.adLockReadOnly, (int)ADODB.CommandTypeEnum.adCmdText, jobSubstring + "-%", "ApprovalPending");
							}

							else
							{
								SQL = "select G3E_IDENTIFIER, G3E_JOBSTATUS from G3E_JOB where G3E_IDENTIFIER like ? and G3E_JOBSTATUS=?";
								RS = dc.OpenRecordset(SQL, ADODB.CursorTypeEnum.adOpenStatic, ADODB.LockTypeEnum.adLockReadOnly, (int)ADODB.CommandTypeEnum.adCmdText, Job + "-%", "ApprovalPending");
							}

							if(RS != null && RS.RecordCount > 0) //check to see if recordset from above if/else has records. If yes, this job has an alternate design.
							{
								hasAlternateDesign = true;
							}

							if(hasAlternateDesign)
							{
								string altJobIdentifier = Convert.ToString(RS.Fields["G3E_IDENTIFIER"].Value);

								if(InteractiveMode)
								{
									DialogResult DesignResult = MessageBox.Show("An alternate design for this WR is marked for approval. Unmark it so that this alternate may be activated?", "G/Technology", MessageBoxButtons.YesNo);
									if(DesignResult == DialogResult.Yes)
									{
										//update job status to Design for the alternate design, and leave the current job active
										WMISStatus wMISStatus = new WMISStatus();
										wMISStatus.SetJobStatus(altJobIdentifier, "Design");
									}
									if(DesignResult == DialogResult.No)
									{
										MessageBox.Show("Job is being deactivated and the alternate design will remain marked for approval.", "G/Technology", MessageBoxButtons.OK);
										DeactivateJob(Job);        //deactivate job
									}
								}
								else // UnattendedMode
								{
									//update job status to Design for the alternate design, and leave the current job active
									WMISStatus wMISStatus = new WMISStatus();
									wMISStatus.SetJobStatus(altJobIdentifier, "Design");
								}
							}
							break;

						case "CLOSUREPENDING":
							//SQL = "update G3E_JOB set G3E_JOBSTATUS='ConstructionComplete' where G3E_IDENTIFIER=?";

							if(InteractiveMode)
							{
								DialogResult ClosurePendingResult = MessageBox.Show("WR is marked for closure. Unmark?", "G/Technology", MessageBoxButtons.YesNo);
								if(ClosurePendingResult == DialogResult.Yes)
								{
									//change job status to ConstructionComplete and leave job active
									WMISStatus wMISStatus = new WMISStatus();
									wMISStatus.SetJobStatus(Job, "ConstructionComplete");
								}
								if(ClosurePendingResult == DialogResult.No)
								{
									MessageBox.Show("Job is being deactivated and the WR will remain marked for closure.", "G/Technology", MessageBoxButtons.OK);
									DeactivateJob(Job);        //deactivate job
								}
							}
							else // UnattendedMode
							{
								//change job status to ConstructionComplete and leave job active
								WMISStatus wMISStatus = new WMISStatus();
								wMISStatus.SetJobStatus(Job, "ConstructionComplete");
							}
							break;

						default:
							break;
					}
				}
				CommonWorkPointDisplayQuery workPointDisplayQuery = new CommonWorkPointDisplayQuery(app);
				workPointDisplayQuery.RedisplayWorkPoints();

			}
			RS.Close();
		}

		/// <summary>
		/// Exceutes an SQL with one string parameter
		/// </summary>
		/// <param name="sql">SQL statement to execute</param>
		/// <param name="p">Parameter for the SQL</param>
		private void ExecuteSQL(string sql, string p)
		{
			IGTApplication app = GTClassFactory.Create<IGTApplication>();
			app.DataContext.Execute(sql, out int recs, (int)ADODB.CommandTypeEnum.adCmdText, p);
			app.DataContext.Execute("commit", out recs, (int)ADODB.CommandTypeEnum.adCmdText);
		}

		private void DeactivateJob(string job) //deactivates active job
		{
			IGTJobHelper jobHelper = GTClassFactory.Create<IGTJobHelper>();
			jobHelper.EndEditJob();
			jobHelper.RemoveJob(job);
		}


		/// <summary>
		///  Refreshes the logging property
		/// </summary>
		/// <param name="app">GT application object</param>
		/// <param name="log">Yes/No</param>
		private void RefreshLoggingProperty(IGTApplication app, string logSetting)
		{
			try
			{
				if(CheckForLoggingProperty(app))
				{
					app.Application.Properties.Remove("EnableLogging");
				}
				app.Application.Properties.Add("EnableLogging", logSetting);
			}
			catch(Exception)
			{
				throw;
			}
		}

		/// <summary>
		/// Checks whether EnableLogging property exists
		/// </summary>
		/// <param name="app">GT application object</param>
		/// <returns> true, if property exists</returns>
		private bool CheckForLoggingProperty(IGTApplication app)
		{
			for(short i = 0;i < app.Application.Properties.Keys.Count;i++)
			{
				if(Convert.ToString(app.Application.Properties.Keys.Get(i)) == "EnableLogging")
				{
					return true;
				}
			}
			return false;
		}

	}
}

