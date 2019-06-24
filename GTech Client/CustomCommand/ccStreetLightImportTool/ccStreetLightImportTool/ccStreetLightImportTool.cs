// ===================================================
//  Copyright 2017 Intergraph Corp.
//  File Name: ccStreetLightImportTool.cs
//
//  Description:   The import of customer-owned Street Light data will be accomplished using this custom command.
//  Remarks: 
//  Modification History
//  Date                Author                      Comments
//  --------            --------------              --------
//  03/04/2018          Prathyusha                  Created 
//  12/04/2018          Sitara                      Modified
// ======================================================
using System;
using ADODB;
using System.Windows.Forms;
using Intergraph.GTechnology.API;
using Intergraph.GTechnology.Interfaces;
using System.Data;

namespace GTechnology.Oncor.CustomAPI
{
	public class ccStreetLightImportTool : IGTCustomCommandModeless
	{
		#region IGTCustomCommandModeless variables

		private IGTTransactionManager m_oGTTransactionManager = null;
		private IGTApplication m_oGTApp = null;
		private IGTCustomCommandHelper m_oGTCustomCommandHelper;

		#endregion

		#region IGTCustomCommandModeless Members
		public void Activate(IGTCustomCommandHelper CustomCommandHelper)
		{
			try
			{
				m_oGTApp = GTClassFactory.Create<IGTApplication>();
				m_oGTCustomCommandHelper = CustomCommandHelper;
				if(m_oGTApp.DataContext.IsRoleGranted("PRIV_MGMT_STLT"))
				{
					if(Validate())
					{
						string streetlightImportSheet = OpenFileDialog();
						if(!String.IsNullOrEmpty(streetlightImportSheet))
						{
							m_oGTApp.BeginWaitCursor();
							StreetLightImportWorkSheet importWorkSheet = new StreetLightImportWorkSheet();
							importWorkSheet.InitializeExcel(streetlightImportSheet);
							if(importWorkSheet.ExcelTable != null)
							{
								ImportSpreadsheetFormatValidation formatValidation = new ImportSpreadsheetFormatValidation(m_oGTApp.DataContext);
								m_oGTApp.SetStatusBarText(GTStatusPanelConstants.gtaspcMessage, "Street Light Import Tool : Validating Spreadsheet Format");
								if(formatValidation.ValidateSpreadsheetFormat(importWorkSheet.ExcelTable))
								{
									ImportSpreadsheetRecordValidation recordValidation = new ImportSpreadsheetRecordValidation(m_oGTApp, m_oGTTransactionManager);
									DataTable excelDataTable = recordValidation.ValidateSpreadsheetRecord(importWorkSheet.ExcelTable);
									ValidateProcessing(streetlightImportSheet, importWorkSheet, excelDataTable, recordValidation);

								}
							}
							m_oGTApp.EndWaitCursor();
						}
					}

					ExitCommand();
				}
				else
				{
					MessageBox.Show("User does not have PRIV_MGMT_STLT role.", "G/Technology", MessageBoxButtons.OK, MessageBoxIcon.Error,
													 MessageBoxDefaultButton.Button1);
					ExitCommand();
				}
			}
			catch(Exception ex)
			{
				m_oGTApp.EndWaitCursor();
				if(m_oGTTransactionManager.TransactionInProgress)
				{
					m_oGTTransactionManager.Rollback();
				}
				if(ex.GetType().Name == "DuplicateNameException")
				{
					MessageBox.Show("WorkSheet contains duplicate column names.", "G/Technology", MessageBoxButtons.OK, MessageBoxIcon.Error);
				}
				else
				{
					MessageBox.Show("Error in StreetLight Import Tool command: " + ex.Message, "G/Technology", MessageBoxButtons.OK, MessageBoxIcon.Error);
				}
				ExitCommand();
			}
		}

		private void ValidateProcessing(string streetlightImportSheet, StreetLightImportWorkSheet importWorkSheet, DataTable excelDataTable, ImportSpreadsheetRecordValidation recordValidation)
		{
			try
			{
				importWorkSheet.DataTableTOExcel(streetlightImportSheet, excelDataTable);

				#region Validate Job Edits
				if(recordValidation.m_nbrDataBaseChanges)
				{
					IGTJobManagementService jobService = GTClassFactory.Create<IGTJobManagementService>();
					jobService.DataContext = m_oGTApp.DataContext;
					Recordset rsValidate = jobService.ValidatePendingEdits();
					if(rsValidate != null && rsValidate.RecordCount > 0)
					{
						MessageBox.Show("The job has validation errors that will need to be resolved before it can be posted.",
								 "G/Technology", MessageBoxButtons.OK, MessageBoxIcon.Information);
					}
				}
				#endregion
				MessageBox.Show("Number of successful Add transactions: " + recordValidation.SuccessfulAddTransactions + Environment.NewLine + Environment.NewLine +
														"Number of unsuccessful Add transactions that resulted in a warning: " + recordValidation.WarningAddTransactions + Environment.NewLine + Environment.NewLine +
														"Number of unsuccessful Add transactions that resulted in an error: " + recordValidation.ErrorAddTransactions + Environment.NewLine + Environment.NewLine +
														"Number of successful Removal transactions: " + recordValidation.SuccessfulRemovalTransactions + Environment.NewLine + Environment.NewLine +
														"Number of unsuccessful Removal transactions that resulted in a warning: " + recordValidation.WarningRemovalTransactions + Environment.NewLine + Environment.NewLine +
														"Number of unsuccessful Removal transactions that resulted in an error: " + recordValidation.ErrorRemovalTransactions, "G/Technology", MessageBoxButtons.OK, MessageBoxIcon.Information);

			}
			catch
			{
				throw;
			}
		}

		public bool CanTerminate
		{
			get
			{
				return true;
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
			try
			{
				if(m_oGTTransactionManager != null)
				{
					m_oGTTransactionManager = null;
				}

			}
			catch(Exception e)
			{
				throw e;
			}
		}
		public IGTTransactionManager TransactionManager
		{
			get
			{
				return m_oGTTransactionManager;
			}
			set
			{
				m_oGTTransactionManager = value;
			}
		}
		#endregion

		#region Private methods
		/// <summary>
		/// Method to Validate the required conditions to Execute this custom command
		/// </summary>
		private bool Validate()
		{
			string jobType = null;
			Recordset jobRs = null;
			try
			{
				DataAccess dataAccess = new DataAccess(m_oGTApp.DataContext);
				jobRs = dataAccess.GetRecordset(string.Format("SELECT * FROM G3E_JOB WHERE G3E_IDENTIFIER  = '{0}'", m_oGTApp.DataContext.ActiveJob));
				if(jobRs != null && jobRs.RecordCount > 0)
				{
					jobRs.MoveFirst();
					jobType = Convert.ToString(jobRs.Fields["G3E_JOBTYPE"].Value);
				}
				//Checks for WR Jobs only
				if(jobType.ToUpper() != "NON-WR")
				{
					MessageBox.Show("This command applies only to GIS Maintenance jobs.", "G/Technology", MessageBoxButtons.OK, MessageBoxIcon.Error);
					return false;
				}
				return true;
			}
			catch
			{
				throw;
			}
		}

		/// <summary>
		/// Method to Validate the required conditions to Execute this custom command
		/// </summary>
		private string OpenFileDialog()
		{
			string streetlightSheet = String.Empty;
			try
			{
				OpenFileDialog openStreetlightFileDialog = new OpenFileDialog();

				openStreetlightFileDialog.Title = "Select Streetlight Data";
				openStreetlightFileDialog.InitialDirectory = @"C:\";
				openStreetlightFileDialog.Multiselect = false;
				openStreetlightFileDialog.CheckFileExists = true;
				openStreetlightFileDialog.CheckPathExists = true;
				openStreetlightFileDialog.ReadOnlyChecked = false;
				openStreetlightFileDialog.Filter = "Excel Files (*.xls,*.xlsx)|*.xls;*.xlsx";

				if(openStreetlightFileDialog.ShowDialog() == DialogResult.OK)
				{
					streetlightSheet = openStreetlightFileDialog.FileName;
				}
			}
			catch(Exception)
			{
				throw;
			}
			return streetlightSheet;
		}

		/// <summary>
		/// Method to Exit the command.
		/// </summary>
		private void ExitCommand()
		{
			try
			{
				try
				{
					if(m_oGTApp.Application.Properties.Count > 0)
						m_oGTApp.Application.Properties.Remove("StreetLightImportToolMSFID");
				}
				catch
				{

				}

				if(m_oGTCustomCommandHelper != null)
				{
					m_oGTCustomCommandHelper.Complete();
					m_oGTCustomCommandHelper = null;
				}
			}
			catch
			{
				throw;
			}
			finally
			{
				m_oGTCustomCommandHelper = null;
				m_oGTTransactionManager = null;
			}
		}
		#endregion
	}
}
