// ===================================================
//  Copyright 2017 Intergraph Corp.
//  File Name: ImportSpreadsheetRecordValidation.cs
//
//  Description: Validate the each record in spreadsheet before any record processing is done.
//  Remarks: 
//  Modification History
//  Date                Author                      Comments
//  --------            --------------              --------
//  03/04/2018          Prathyusha                  Created 
// ==========================================================
using System;
using ADODB;
using Intergraph.GTechnology.API;
using System.Data;

namespace GTechnology.Oncor.CustomAPI
{
	class ImportSpreadsheetRecordValidation
	{
		#region Variables

		private IGTApplication m_oGTApplication;
		private IGTTransactionManager m_oGTTransactionManager;
		private int m_nbrRemoveSuccess = 0;
		private int m_nbrRemoveWarning = 0;
		private int m_nbrRemoveError = 0;
		private int m_nbrAddSuccess = 0;
		private int m_nbrAddWarning = 0;
		private int m_nbrAddError = 0;
		public bool m_nbrDataBaseChanges = false;
		#endregion

		#region Properties
		public int SuccessfulRemovalTransactions
		{
			get { return m_nbrRemoveSuccess; }
			set { m_nbrRemoveSuccess = value; }
		}
		public int WarningRemovalTransactions
		{
			get { return m_nbrRemoveWarning; }
			set { m_nbrRemoveWarning = value; }
		}
		public int ErrorRemovalTransactions
		{
			get { return m_nbrRemoveError; }
			set { m_nbrRemoveError = value; }
		}
		public int SuccessfulAddTransactions
		{
			get { return m_nbrAddSuccess; }
			set { m_nbrAddSuccess = value; }
		}
		public int WarningAddTransactions
		{
			get { return m_nbrAddWarning; }
			set { m_nbrAddWarning = value; }
		}
		public int ErrorAddTransactions
		{
			get { return m_nbrAddError; }
			set { m_nbrAddError = value; }
		}
		#endregion

		#region Constructor
		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="p_dataContext">The current G/Technology application object.</param>
		/// <param name="p_transactionManager">The transaction of G/Technology application.</param>
		public ImportSpreadsheetRecordValidation(IGTApplication p_application, IGTTransactionManager p_transactionManager)
		{
			this.m_oGTApplication = p_application;
			m_oGTTransactionManager = p_transactionManager;
		}
		#endregion

		#region Methods

		/// <summary>
		/// Method to validate each record in spreadsheet before the information in that record is applied to the GIS.
		/// </summary>
		/// <param name="p_dataTable">Worksheet data Datatable</param>
		/// <returns></returns>
		public DataTable ValidateSpreadsheetRecord(DataTable p_dataTable)
		{
			DataAccess dataAccess = new DataAccess(m_oGTApplication.DataContext);
			DataRow dataRow = null;
			try
			{
				for(int i = 0;i < p_dataTable.Rows.Count;i++)
				{

					// Each instance of an error needs to call this before executing the continue
					// as well as at the end of each iteration (for successful transactions).
					void EvaluateErrors()
					{
						dataRow = ProcessingImport(p_dataTable.Rows[i]);
						UpdateDataTable(ref p_dataTable, i, dataRow);
						m_nbrDataBaseChanges = true;
					}


					m_oGTApplication.SetStatusBarText(GTStatusPanelConstants.gtaspcMessage, "Street Light Import Tool : Validating Spreadsheet Record");
					p_dataTable.Rows[i]["TRANSACTION DATE"] = DateTime.Now;
					if(Convert.ToString(p_dataTable.Rows[i]["TRANSACTION STATUS"]).ToUpper() != "SUCCESS" && Convert.ToString(p_dataTable.Rows[i]["TRANSACTION STATUS"]).ToUpper() != "WARNING")
					{
						if(String.IsNullOrEmpty(Convert.ToString(p_dataTable.Rows[i]["TRANSACTION TYPE"])) || String.IsNullOrEmpty(Convert.ToString(p_dataTable.Rows[i]["ESI LOCATION"])) || String.IsNullOrEmpty(Convert.ToString(p_dataTable.Rows[i]["LAMP TYPE"])) || String.IsNullOrEmpty(Convert.ToString(p_dataTable.Rows[i]["WATTAGE"]))
								|| String.IsNullOrEmpty(Convert.ToString(p_dataTable.Rows[i]["LUMINAIRE STYLE"])) || String.IsNullOrEmpty(Convert.ToString(p_dataTable.Rows[i]["ONCOR STRUCTURE"])) || String.IsNullOrEmpty(Convert.ToString(p_dataTable.Rows[i]["LOCATION DESCRIPTION"])))
						{
							p_dataTable.Rows[i]["TRANSACTION STATUS"] = "ERROR";
							p_dataTable.Rows[i]["TRANSACTION COMMENT"] = "One or more required fields do not contain data.";
							EvaluateErrors();
							continue;
						}

						if(!(Convert.ToString(p_dataTable.Rows[i]["TRANSACTION TYPE"]).ToUpper() == "ADD" || Convert.ToString(p_dataTable.Rows[i]["TRANSACTION TYPE"]).ToUpper() == "REMOVE"))
						{
							p_dataTable.Rows[i]["TRANSACTION STATUS"] = "ERROR";
							p_dataTable.Rows[i]["TRANSACTION COMMENT"] = "Invalid Transaction Type";
							EvaluateErrors();
							continue;
						}

						string[] args = new string[] { Convert.ToString(p_dataTable.Rows[i]["ESI LOCATION"]) };
						Recordset accountRS = dataAccess.GetRecordset("select * from STLT_ACCOUNT where ESI_LOCATION=?", args);

						if(accountRS != null && accountRS.RecordCount == 0)
						{
							p_dataTable.Rows[i]["TRANSACTION STATUS"] = "ERROR";
							p_dataTable.Rows[i]["TRANSACTION COMMENT"] = "ESI Location does not exist in the STLT_ACCOUNT table.";
							EvaluateErrors();
							continue;
						}

						accountRS.MoveFirst();

						if(Convert.ToString(p_dataTable.Rows[i]["ESI LOCATION"]) != Convert.ToString(accountRS.Fields["ESI_LOCATION"].Value) || Convert.ToString(p_dataTable.Rows[i]["LAMP TYPE"]) != Convert.ToString(accountRS.Fields["LAMP_TYPE"].Value)
								|| (Convert.ToString(p_dataTable.Rows[i]["WATTAGE"]) != Convert.ToString(accountRS.Fields["WATTAGE"].Value)) || Convert.ToString(p_dataTable.Rows[i]["LUMINAIRE STYLE"]) != Convert.ToString(accountRS.Fields["LUMINARE_STYLE"].Value))
						{
							p_dataTable.Rows[i]["TRANSACTION STATUS"] = "ERROR";
							p_dataTable.Rows[i]["TRANSACTION COMMENT"] = "Account not found that matches ESI LOCATION, LAMP TYPE, WATTAGE, and LUMINAIRE STYLE.";
							EvaluateErrors();
							continue;
						}

						string strCUCode = dataAccess.GetCustomerOwnedCUCode(Convert.ToString(p_dataTable.Rows[i]["LAMP TYPE"]),
																						 Convert.ToString(p_dataTable.Rows[i]["LUMINAIRE STYLE"]),
																						 Convert.ToString(p_dataTable.Rows[i]["WATTAGE"]));

						if(string.IsNullOrEmpty(strCUCode))
						{
							p_dataTable.Rows[i]["TRANSACTION STATUS"] = "ERROR";
							p_dataTable.Rows[i]["TRANSACTION COMMENT"] = "Distinct customer-owned CU not found that matches LAMP TYPE, WATTAGE, and LUMINAIRE STYLE";
							EvaluateErrors();
							continue;
						}

						if(!String.IsNullOrEmpty(Convert.ToString(p_dataTable.Rows[i]["STREETLIGHT ID"])))
						{
							Recordset streetLightAttributeRS = dataAccess.GetRecordset(String.Format("select * from STREETLIGHT_N WHERE ACCOUNT_ID='{0}' and CO_IDENTIFIER='{1}'", Convert.ToString(p_dataTable.Rows[i]["ESI LOCATION"]), Convert.ToString(p_dataTable.Rows[i]["STREETLIGHT ID"])));

							if(Convert.ToString(p_dataTable.Rows[i]["TRANSACTION TYPE"]).ToUpper() == "ADD")
							{
								if(streetLightAttributeRS != null && streetLightAttributeRS.RecordCount > 0)
								{
									p_dataTable.Rows[i]["TRANSACTION STATUS"] = "ERROR";
									p_dataTable.Rows[i]["TRANSACTION COMMENT"] = "Duplicate Street Light exists with this ESI Location and Street Light ID.";
									EvaluateErrors();
									continue;
								}
							}

							if(Convert.ToString(p_dataTable.Rows[i]["TRANSACTION TYPE"]).ToUpper() == "REMOVE")
							{
								if(streetLightAttributeRS != null && streetLightAttributeRS.RecordCount == 0)
								{
									p_dataTable.Rows[i]["TRANSACTION STATUS"] = "ERROR";
									p_dataTable.Rows[i]["TRANSACTION COMMENT"] = "No Street Light found for this ESI Location and Street Light ID.";
									EvaluateErrors();
									continue;
								}
								else if(streetLightAttributeRS.RecordCount > 1)
								{
									p_dataTable.Rows[i]["TRANSACTION STATUS"] = "ERROR";
									p_dataTable.Rows[i]["TRANSACTION COMMENT"] = "Multiple Street Lights were found for this ESI Location and Street Light ID.";
									EvaluateErrors();
									continue;
								}
							}
						}

						if(!(Convert.ToString(p_dataTable.Rows[i]["ONCOR STRUCTURE"]) == 'Y'.ToString() || Convert.ToString(p_dataTable.Rows[i]["ONCOR STRUCTURE"]) == 'N'.ToString()))
						{
							p_dataTable.Rows[i]["TRANSACTION STATUS"] = "ERROR";
							p_dataTable.Rows[i]["TRANSACTION COMMENT"] = "ONCOR STRUCTURE must be Y or N.";
							EvaluateErrors();
							continue;
						}

						if(String.IsNullOrEmpty(Convert.ToString(p_dataTable.Rows[i]["ONCOR STRUCTURE ID"])) && String.IsNullOrEmpty(Convert.ToString(p_dataTable.Rows[i]["GPS X"])) && String.IsNullOrEmpty(Convert.ToString(p_dataTable.Rows[i]["GPS Y"])))
						{
							if(dataAccess.GetBoundaryFID(Convert.ToString(p_dataTable.Rows[i]["ESI LOCATION"])) == 0)
							{
								p_dataTable.Rows[i]["TRANSACTION STATUS"] = "ERROR";
								p_dataTable.Rows[i]["TRANSACTION COMMENT"] = "No STRUCTURE ID, GPS given. Account does not have a Boundary or Structure defined.";
							}
							EvaluateErrors();
							continue;
						}

						Recordset commonRS = dataAccess.GetRecordset(String.Format("select * from COMMON_N WHERE STRUCTURE_ID='{0}'", Convert.ToString(p_dataTable.Rows[i]["ONCOR STRUCTURE ID"])));

						if(Convert.ToString(p_dataTable.Rows[i]["ONCOR STRUCTURE"]) == 'Y'.ToString())
						{
							if(commonRS != null && commonRS.RecordCount == 0)
							{
								p_dataTable.Rows[i]["TRANSACTION STATUS"] = "ERROR";
								p_dataTable.Rows[i]["TRANSACTION COMMENT"] = "ONCOR STRUCTURE is Y but no Structure found for STRUCTURE ID.";
								EvaluateErrors();
								continue;
							}
							else if(commonRS.RecordCount > 1)
							{
								p_dataTable.Rows[i]["TRANSACTION STATUS"] = "ERROR";
								p_dataTable.Rows[i]["TRANSACTION COMMENT"] = "ONCOR STRUCTURE is Y and multiple Structures found for STRUCTURE ID.";
								EvaluateErrors();
								continue;
							}
						}

						if(Convert.ToString(p_dataTable.Rows[i]["ONCOR STRUCTURE"]) == 'N'.ToString())
						{
							if(commonRS != null && commonRS.RecordCount > 0)
							{
								p_dataTable.Rows[i]["TRANSACTION STATUS"] = "ERROR";
								p_dataTable.Rows[i]["TRANSACTION COMMENT"] = "STRUCTURE ID has a value but ONCOR STRUCTURE flag is set to N.";
								EvaluateErrors();
								continue;
							}
						}

						if(!String.IsNullOrEmpty(Convert.ToString(p_dataTable.Rows[i]["GPS X"])) && !String.IsNullOrEmpty(Convert.ToString(p_dataTable.Rows[i]["GPS Y"])) && !CheckGPSCoordinates(Convert.ToString(p_dataTable.Rows[i]["GPS X"]), Convert.ToString(p_dataTable.Rows[i]["GPS Y"])))
						{
							p_dataTable.Rows[i]["TRANSACTION STATUS"] = "ERROR";
							p_dataTable.Rows[i]["TRANSACTION COMMENT"] = "GPS Coordinates are either malformed or outside of the expected range of 93.0W – 107.0W and 25.0N – 37.0N.";
							EvaluateErrors();
							continue;
						}
					}

					// For the cases where no errors were encountered,
					// the SUCCESSFUL transactions also need to be counted.
					EvaluateErrors();
				}
			}
			catch
			{
				throw;
			}
			finally
			{
				dataAccess = null;
			}

			return p_dataTable;
		}


		/// <summary>
		/// Method to validate GPS Coordinates.
		/// </summary>
		/// <param name="xGPS">GPS X Value</param>
		/// <param name="yGPS">GPS Y Value</param>
		/// <returns></returns>
		private bool CheckGPSCoordinates(string xGPS, string yGPS)
		{
			bool checkGPSCoordinates = false;
			try
			{
				if((xGPS.EndsWith("W") && xGPS.Contains(".")) && (yGPS.EndsWith("N") && yGPS.Contains(".")))
				{
					if(!(xGPS.EndsWith(" W") && xGPS.EndsWith(" N")))
					{
						if((Convert.ToDouble(xGPS.Substring(0, xGPS.Length - 1)) >= 93.0 && Convert.ToDouble(xGPS.Substring(0, xGPS.Length - 1)) <= 107.0) && (Convert.ToDouble(yGPS.Substring(0, xGPS.Length - 1)) >= 25.0 && Convert.ToDouble(yGPS.Substring(0, xGPS.Length - 1)) <= 37.0))
						{
							checkGPSCoordinates = true;
						}
					}
				}
			}
			catch
			{
				throw;
			}
			return checkGPSCoordinates;
		}


		private DataRow ProcessingImport(DataRow dataRow)
		{
			try
			{
				if(Convert.ToString(dataRow["TRANSACTION TYPE"]).ToUpper() == "REMOVE")
				{
					m_oGTApplication.SetStatusBarText(GTStatusPanelConstants.gtaspcMessage, "Street Light Import Tool : Remove processing");
					StreetLightRemoveProcessing removeProcessing = new StreetLightRemoveProcessing(m_oGTApplication.DataContext, m_oGTTransactionManager);
					dataRow = removeProcessing.DeleteStreetLightForTranstionTypeRemove(dataRow);
					if(Convert.ToString(dataRow["TRANSACTION STATUS"]) == "SUCCESS")
					{
						m_nbrRemoveSuccess++;
					}
					else if(Convert.ToString(dataRow["TRANSACTION STATUS"]) == "ERROR")
					{
						m_nbrRemoveError++;
					}
					else if(Convert.ToString(dataRow["TRANSACTION STATUS"]) == "WARNING")
					{
						m_nbrRemoveWarning++;
					}
				}
				else if(Convert.ToString(dataRow["TRANSACTION TYPE"]).ToUpper() == "ADD")
				{
					m_oGTApplication.SetStatusBarText(GTStatusPanelConstants.gtaspcMessage, "Street Light Import Tool : Add processing");
					StreetLightAddProcessing streetLightAddProcessing = new StreetLightAddProcessing(m_oGTApplication.DataContext,
					m_oGTTransactionManager, m_oGTApplication);
					dataRow = streetLightAddProcessing.AddStreetLightForTranstionTypeAdd(dataRow);
					if(Convert.ToString(dataRow["TRANSACTION STATUS"]) == "SUCCESS")
					{
						m_nbrAddSuccess++;
					}
					else if(Convert.ToString(dataRow["TRANSACTION STATUS"]) == "ERROR")
					{
						m_nbrAddError++;
					}
					else if(Convert.ToString(dataRow["TRANSACTION STATUS"]) == "WARNING")
					{
						m_nbrAddWarning++;
					}
				}
			}
			catch
			{
				throw;
			}
			return dataRow;
		}

		private void UpdateDataTable(ref DataTable p_dataTable, int p_rowNumber, DataRow dataRow)
		{
			try
			{
				foreach(DataColumn dc in p_dataTable.Columns)
				{
					p_dataTable.Rows[p_rowNumber][dc] = dataRow[dc];
				}
			}
			catch
			{
				throw;
			}
		}

		#endregion
	}
}
