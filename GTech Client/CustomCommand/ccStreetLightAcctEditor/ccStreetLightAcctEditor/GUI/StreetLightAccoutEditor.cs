
//  Remarks:
// 
//  Modification History
//  Date                Author                      Comments
//  --------            --------------              --------
//  22/02/2018          Pramod                     Implemented Street Light Account Editor and Value Lists 
//  05/03/2018          Pramod                     Implemented Street Light Boundary 
//  22/03/2018          Pramod                     Implemented Managing Non-Located Street Lights
//  27/04/2018          Pramod                     Fixed Description Data Grid unhandled excpetion and 
//                                                 Sync Street Light Data Grid columns and filter data grid colums
//  21/06/2018          Pramod                     Fixed Boundary tab unhandle exceptions
//  04/07/2018          Pramod                     Fixed Sorting columns on StreetlightAccountGrid and Scroll bar sync
//  09/01/2019          Pramod                     Fixed ManageNonLocatedSTLT and Street Light Boudary tab issues(ALM1630,1637)
//  19/04/2019          Pramod                     Fixed ALM - 1035 and 2040 - Added Drop down box to Connection column on MangeNonLocated Grid
// ===============================================================================================================================================

using GTechnology.Oncor.CustomAPI.DataAccess;
using GTechnology.Oncor.CustomAPI.Model;
using Intergraph.GTechnology.API;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using ADODB;

namespace GTechnology.Oncor.CustomAPI.GUI
{
	public partial class StreetLighAccountEditor : Form
	{
		StreetLightActEditorContext stltAccountCtx = null;
		StreetLightValueListContext stltValueLstCtx = null;
		StreetLightBoundaryContext stltBounndryCtx = null;

		BindingList<StreetLightAccount> streetLightAccts = null;
		List<StreetLightAccount> stltFilterAccts = null;

		// cache Street Light Accounts assigned to Street light Feature
		IList<string> assignedSTLTAccounts = null;
		// cache CSIESI location
		IList<string> csiESILocations = null;
		// cache boundary fid assigned to street Light account. Useful when click on show Boundary
		Dictionary<string, int> assignedBoundarys = null;
		List<ErrorLog> errLogs = null;

		string DeleteValidationMsg = "{0} assigned to Street Light Account.Hence cannot be deleted";
		string NotNullValidationMSg = "{0} Null value not allowed";
		string UniqueConstValidationMsg = "{0} unique constraint violated.Hence Duplicate {1} is not allowed";
		string CCBValidationMsg = "CC&B validation failed for the account {0}";
		string UnSetBoundaryMsg = "Removing Boundary definition from selected row.\nPlease confirm?";
		string StreetLightAcctEditorTxt = "Street Light Account Editor";
		string StreetLightAcctTxt = "Street Light Account";
		string PendingChangesMsg = "There are pending edits to save.Do you want to save the changes?";
		string DescriptionValueLstTxt = "Description Value List";
		string RateScheduleValueLstTxt = "Rate Schedule Value List";
		string RateCodeValueLstTxt = "Rate Code Value List";
		string OwnerValueLstTxt = "Owner Value List";
		string StreetLightBndryTxt = "Street Light Boundary";


		DateTimePicker oDateTimePicker = null;

		BindingList<LampType> vlLampTypes = null;
		BindingList<Wattage> vlWattages = null;
		BindingList<LuminaireStyle> vlLuminareStyles = null;
		BindingList<StreetLightRateCode> vlStreetLightRateCodes = null;
		BindingList<StreetLightRateSchedule> vlStreetLightRateSchedules = null;
		BindingList<StreetLightDescription> vlStreetLightDesc = null;
		BindingList<StreetLightOwner> vlStreetLightOwner = null;
		BindingList<StreetLightBoundary> streetLightBndrys = null;



		IGTTransactionManager _gtTransactionManager = null;
		IGTCustomCommandHelper _gtCustomCommandHelper;

		IGTApplication _gtApp = null;

		bool isJobActive;

		/// <summary>
		/// initialize Street Light Account Editor 
		/// </summary>
		public StreetLighAccountEditor()
		{
			InitializeComponent();
			_gtApp = GTClassFactory.Create<IGTApplication>();

			dtGridViewStreetAcct.AutoGenerateColumns = false;
			dtGridViewDescription.AutoGenerateColumns = false;
			dtGridViewOwner.AutoGenerateColumns = false;
			dtGridviewRateCode.AutoGenerateColumns = false;
			dtGridViewRateSch.AutoGenerateColumns = false;
			dtGridViewFilter.AutoGenerateColumns = false;
			dtGridViewBndry.AutoGenerateColumns = false;

			stltAccountCtx = new StreetLightActEditorContext();
			stltValueLstCtx = new StreetLightValueListContext();
			stltBounndryCtx = new StreetLightBoundaryContext();
			errLogs = new List<ErrorLog>();

			this.dtGridViewStreetAcct.DataError += this.dtGridViewStreetAcct_DataError;

			dtGridViewDescription.CellFormatting += DtGridViewDescription_CellFormatting;
			dtGridviewRateCode.CellFormatting += DtGridviewRateCode_CellFormatting;
			dtGridViewRateSch.CellFormatting += DtGridViewRateSch_CellFormatting;
			dtGridViewOwner.CellFormatting += DtGridViewOwner_CellFormatting;
			dtGridViewBndry.CellFormatting += DtGridViewBndry_CellFormatting;
			dtGridViewStreetAcct.CellFormatting += DtGridViewStreetAcct_CellFormatting;

			//bind Property Name to DataGric columns

			BindDataProperties();
			BindDataSourceToGrid();


			dtGridViewStreetAcct.ReadOnly = false;
			dtGridViewStreetAcct.AllowUserToDeleteRows = true;
			dtGridViewStreetAcct.AllowUserToResizeColumns = true;
			dtGridViewStreetAcct.AllowUserToAddRows = true;
			dtGridViewStreetAcct.SelectionMode = DataGridViewSelectionMode.RowHeaderSelect;

			dtGridViewDescription.ReadOnly = false;
			dtGridViewDescription.AllowUserToDeleteRows = true;
			dtGridViewDescription.AllowUserToAddRows = true;
			dtGridViewDescription.SelectionMode = DataGridViewSelectionMode.RowHeaderSelect;
			dtGridViewDescription.CellClick += DtGridViewDescription_CellClick;

			dtGridViewOwner.ReadOnly = false;
			dtGridViewOwner.AllowUserToDeleteRows = true;
			dtGridViewOwner.AllowUserToAddRows = true;
			dtGridViewOwner.SelectionMode = DataGridViewSelectionMode.RowHeaderSelect;

			dtGridviewRateCode.ReadOnly = false;
			dtGridviewRateCode.AllowUserToDeleteRows = true;
			dtGridviewRateCode.AllowUserToAddRows = true;
			dtGridviewRateCode.SelectionMode = DataGridViewSelectionMode.RowHeaderSelect;

			dtGridViewRateSch.ReadOnly = false;
			dtGridViewRateSch.AllowUserToDeleteRows = true;
			dtGridViewRateSch.AllowUserToAddRows = true;
			dtGridViewRateSch.SelectionMode = DataGridViewSelectionMode.RowHeaderSelect;

			dtGridViewBndry.ReadOnly = false;
			dtGridViewBndry.AllowUserToDeleteRows = true;
			dtGridViewBndry.AllowUserToAddRows = true;
			dtGridViewBndry.SelectionMode = DataGridViewSelectionMode.RowHeaderSelect;
			dtGridViewBndry.DataError += DtGridViewBndry_DataError;

			dtGridViewFilter.ReadOnly = false;
			dtGridViewFilter.AllowUserToDeleteRows = false;
			dtGridViewFilter.AllowUserToAddRows = false;
			dtGridViewFilter.CellValueChanged += DtGridViewFilter_CellValueChanged;

			streetLightTabCtrl.Selected += StreetLightTabCtrl_Selected;

			dtGridViewStreetAcct.Scroll += DtGridViewStreetAcct_Scroll;
			dtGridViewStreetAcct.CellValueChanged += DtGridViewStreetAcct_CellValueChanged;
			dtGridViewStreetAcct.SelectionChanged += DtGridViewStreetAcct_SelectionChanged;
			dtGridViewStreetAcct.ColumnHeaderMouseClick += DtGridViewStreetAcct_ColumnHeaderMouseClick;


			dtGridViewStreetAcct.UserDeletingRow += DtGridViewStreetAcct_UserDeletingRow;
			dtGridViewDescription.UserDeletingRow += DtGridViewDescription_UserDeletingRow;
			dtGridviewRateCode.UserDeletingRow += DtGridviewRateCode_UserDeletingRow;
			dtGridViewRateSch.UserDeletingRow += DtGridViewRateSch_UserDeletingRow;
			dtGridViewOwner.UserDeletingRow += DtGridViewOwner_UserDeletingRow;
			dtGridViewBndry.UserDeletingRow += DtGridViewBndry_UserDeletingRow;


			isJobActive = !string.IsNullOrEmpty(_gtApp.DataContext.ActiveJob);
			assignedBoundarys = new Dictionary<string, int>();


			//Sync Street Light Data Grid columns and filter data grid colums
			SyncStreetLightFilterGridColWidth();
			dtGridViewStreetAcct.ColumnWidthChanged += dtGridViewStreetAcct_ColumnWidthChanged;

			oDateTimePicker = new DateTimePicker();

			//Adding DateTimePicker control into DataGridView   
			dtGridViewDescription.Controls.Add(oDateTimePicker);
			// An event attached to dateTimePicker Control which is fired when DateTimeControl is closed  
			oDateTimePicker.CloseUp += new EventHandler(oDateTimePicker_CloseUp);

			// An event attached to dateTimePicker Control which is fired when any date is selected  
			oDateTimePicker.TextChanged += new EventHandler(dateTimePicker_OnTextChange);
			oDateTimePicker.Format = DateTimePickerFormat.Short;
			oDateTimePicker.Visible = false;
			SetColumnsReadOnly();
		}

		public StreetLighAccountEditor(IGTCustomCommandHelper customCommandHelper, IGTTransactionManager gtTransaction) : this()
		{
			this._gtCustomCommandHelper = customCommandHelper;
			this._gtTransactionManager = gtTransaction;

		}

		#region Events

		#region Datagrid  User Deleting events

		/// <summary>
		/// Validate Street Light Account before deleting if account assigned to street light then log error and cancel delete operation
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void DtGridViewStreetAcct_UserDeletingRow(object sender, DataGridViewRowCancelEventArgs e)
		{
			if(this.assignedSTLTAccounts.Count > 0)
			{
				var obj = (StreetLightAccount)e.Row.DataBoundItem;
				if(this.assignedSTLTAccounts.Contains(obj.ESI_LOCATION))
				{
					//Add to Error Dialog
					errLogs.Add(new ErrorLog { ErrorIn = StreetLightAcctTxt, ErrorMessage = string.Format(DeleteValidationMsg, "ESI Location " + obj.ESI_LOCATION) });
					e.Cancel = true;
				}
				else
				{
					if(obj.EntityState == EntityMode.Add)
					{
						e.Cancel = false;
					}
					else
					{
						obj.EntityState = EntityMode.Delete;
						dtGridViewStreetAcct.CurrentCell = null;
						e.Row.Visible = false;
						e.Cancel = true;
					}

				}
			}
		}

		/// <summary>
		/// Validate Street Light Owner before deleting if it is assigned to street light account then log error and cancel delete operation
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void DtGridViewOwner_UserDeletingRow(object sender, DataGridViewRowCancelEventArgs e)
		{
			var obj = (StreetLightOwner)e.Row.DataBoundItem;
			var cnt = this.streetLightAccts.Where(s => s.OWNER_CODE == obj.OwnerCode).Count();
			if(cnt > 0)
			{
				//Add to Error Dialog
				errLogs.Add(new ErrorLog { ErrorIn = OwnerValueLstTxt, ErrorMessage = string.Format(DeleteValidationMsg, "Owner Code " + obj.OwnerCode) });
				e.Cancel = true;
			}
			else
			{
				if(obj.EntityState == EntityMode.Add)
				{
					e.Cancel = false;
				}
				else
				{
					obj.EntityState = EntityMode.Delete;
					dtGridViewOwner.CurrentCell = null;
					e.Row.Visible = false;
					e.Cancel = true;
				}
			}
		}

		/// <summary>
		/// Validate Street Light Rate Schedule before deleting if it is assigned to street light account then log error and cancel delete operation
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void DtGridViewRateSch_UserDeletingRow(object sender, DataGridViewRowCancelEventArgs e)
		{
			var obj = (StreetLightRateSchedule)e.Row.DataBoundItem;
			var cnt = this.streetLightAccts.Where(s => s.RATE_SCHEDULE == obj.RateSchedule).Count();
			if(cnt > 0)
			{
				//Add to Error Dialog
				errLogs.Add(new ErrorLog { ErrorIn = RateScheduleValueLstTxt, ErrorMessage = string.Format(DeleteValidationMsg, "Rate Schedule " + obj.RateSchedule) });
				e.Cancel = true;
			}
			else
			{
				if(obj.EntityState == EntityMode.Add)
				{
					e.Cancel = false;
				}
				else
				{
					obj.EntityState = EntityMode.Delete;
					dtGridViewRateSch.CurrentCell = null;
					e.Row.Visible = false;
					e.Cancel = true;
				}
			}
		}

		/// <summary>
		/// Validate Street Light Rate code before deleting if it is assigned to street light account then log error and cancel delete operation
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void DtGridviewRateCode_UserDeletingRow(object sender, DataGridViewRowCancelEventArgs e)
		{
			var obj = (StreetLightRateCode)e.Row.DataBoundItem;
			var cnt = this.streetLightAccts.Where(s => s.RATE_CODE == obj.RateCode).Count();
			if(cnt > 0)
			{
				//Add to Error Dialog
				errLogs.Add(new ErrorLog { ErrorIn = RateCodeValueLstTxt, ErrorMessage = string.Format(DeleteValidationMsg, "Rate Code " + obj.RateCode) });
				e.Cancel = true;
			}
			else
			{
				if(obj.EntityState == EntityMode.Add)
				{
					e.Cancel = false;
				}
				else
				{
					obj.EntityState = EntityMode.Delete;
					dtGridviewRateCode.CurrentCell = null;
					e.Row.Visible = false;
					e.Cancel = true;
				}
			}
		}

		/// <summary>
		/// Validate Street Light Description before deleting if it is assigned to street light account then log error and cancel delete operation
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void DtGridViewDescription_UserDeletingRow(object sender, DataGridViewRowCancelEventArgs e)
		{
			var obj = (StreetLightDescription)e.Row.DataBoundItem;
			var cnt = this.streetLightAccts.Where(s => s.Description == obj.DESCRIPTION).Count();
			if(cnt > 0)
			{
				//Add to Error Dialog
				errLogs.Add(new ErrorLog { ErrorIn = DescriptionValueLstTxt, ErrorMessage = string.Format(DeleteValidationMsg, "Description " + obj.DESCRIPTION) });
				e.Cancel = true;
			}
			else
			{
				if(obj.EntityState == EntityMode.Add)
				{
					e.Cancel = false;
				}
				else
				{
					obj.EntityState = EntityMode.Delete;
					dtGridViewDescription.CurrentCell = null;
					e.Row.Visible = false;
					e.Cancel = true;
				}
			}
		}


		private void DtGridViewBndry_UserDeletingRow(object sender, DataGridViewRowCancelEventArgs e)
		{
			var obj = (StreetLightBoundary)e.Row.DataBoundItem;
			var cnt = this.streetLightAccts.Where(s => s.BOUNDARY_CLASS == Convert.ToString(obj.Bnd_Class)).Count();
			if(cnt > 0)
			{
				//Add to Error Dialog
				errLogs.Add(new ErrorLog { ErrorIn = StreetLightBndryTxt, ErrorMessage = string.Format(DeleteValidationMsg, "Boundary Class" + obj.Bnd_Class) });
				e.Cancel = true;
			}
			else
			{
				if(obj.EntityState == EntityMode.Add)
				{
					e.Cancel = false;
				}
				else
				{
					obj.EntityState = EntityMode.Delete;
					dtGridViewBndry.CurrentCell = null;
					e.Row.Visible = false;
					e.Cancel = true;
				}
			}
		}
		#endregion

		#region DataGrid Cell formatting events

		private void DtGridViewStreetAcct_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
		{
			if(e.RowIndex >= 0)
			{

				var obj = (StreetLightAccount)dtGridViewStreetAcct.Rows[e.RowIndex].DataBoundItem;
				if(obj != null && obj.EntityState != EntityMode.Add)
				{
					((DataGridViewComboBoxCell)dtGridViewStreetAcct.Rows[e.RowIndex].Cells["Wattage"]).DisplayStyle = DataGridViewComboBoxDisplayStyle.Nothing;
					((DataGridViewComboBoxCell)dtGridViewStreetAcct.Rows[e.RowIndex].Cells["Lamp_Type"]).DisplayStyle = DataGridViewComboBoxDisplayStyle.Nothing;
					((DataGridViewComboBoxCell)dtGridViewStreetAcct.Rows[e.RowIndex].Cells["Luminare_Style"]).DisplayStyle = DataGridViewComboBoxDisplayStyle.Nothing;
					((DataGridViewComboBoxCell)dtGridViewStreetAcct.Rows[e.RowIndex].Cells["Rate_Schedule"]).DisplayStyle = DataGridViewComboBoxDisplayStyle.Nothing;
					((DataGridViewComboBoxCell)dtGridViewStreetAcct.Rows[e.RowIndex].Cells["Rate_Code"]).DisplayStyle = DataGridViewComboBoxDisplayStyle.Nothing;


					dtGridViewStreetAcct.Rows[e.RowIndex].Cells["ESI_Location"].ReadOnly = true;
					dtGridViewStreetAcct.Rows[e.RowIndex].Cells["ESI_Location"].Style.BackColor = Color.DarkGray;

					dtGridViewStreetAcct.Rows[e.RowIndex].Cells["Wattage"].ReadOnly = true;
					dtGridViewStreetAcct.Rows[e.RowIndex].Cells["Wattage"].Style.BackColor = Color.DarkGray;

					dtGridViewStreetAcct.Rows[e.RowIndex].Cells["Lamp_Type"].ReadOnly = true;
					dtGridViewStreetAcct.Rows[e.RowIndex].Cells["Lamp_Type"].Style.BackColor = Color.DarkGray;

					dtGridViewStreetAcct.Rows[e.RowIndex].Cells["Luminare_Style"].ReadOnly = true;
					dtGridViewStreetAcct.Rows[e.RowIndex].Cells["Luminare_Style"].Style.BackColor = Color.DarkGray;

					dtGridViewStreetAcct.Rows[e.RowIndex].Cells["Rate_Schedule"].ReadOnly = true;
					dtGridViewStreetAcct.Rows[e.RowIndex].Cells["Rate_Schedule"].Style.BackColor = Color.DarkGray;

					dtGridViewStreetAcct.Rows[e.RowIndex].Cells["Rate_Code"].ReadOnly = true;
					dtGridViewStreetAcct.Rows[e.RowIndex].Cells["Rate_Code"].Style.BackColor = Color.DarkGray;
				}
			}
		}


		private void DtGridViewBndry_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
		{
			if(e.ColumnIndex == 0 || e.ColumnIndex == 3)
			{
				var obj = (StreetLightBoundary)dtGridViewBndry.Rows[e.RowIndex].DataBoundItem;
				if(obj != null)
				{
					//check Owner code in Street Light Account if so then set Owner code to Readonly 
					var cnt = this.streetLightAccts.Where(s => s.BOUNDARY_CLASS == obj.Bnd_Class.ToString() && s.EntityState != EntityMode.Delete).Count();
					if(cnt > 0)
					{
						dtGridViewBndry.Rows[e.RowIndex].Cells[e.ColumnIndex].ReadOnly = true;
						dtGridViewBndry.Rows[e.RowIndex].Cells[e.ColumnIndex].Style.BackColor = Color.DarkGray;
					}
					else
					{
						dtGridViewBndry.Rows[e.RowIndex].Cells[e.ColumnIndex].ReadOnly = false;
						dtGridViewBndry.Rows[e.RowIndex].Cells[e.ColumnIndex].Style.BackColor = Color.White;
					}
				}
			}
		}


		/// <summary>
		/// set Owner code to Readlonly in DataGrdi if it is used in Street Light Account 
		/// this is to avoid updating the Value
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void DtGridViewOwner_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
		{
			if(e.ColumnIndex == 0)
			{
				var obj = (StreetLightOwner)dtGridViewOwner.Rows[e.RowIndex].DataBoundItem;
				if(obj != null && !String.IsNullOrEmpty(obj.OwnerCode))
				{
					//check Owner code in Street Light Account if so then set Owner code to Readonly 
					var cnt = this.streetLightAccts.Where(s => s.OWNER_CODE == obj.OwnerCode && s.EntityState != EntityMode.Delete).Count();
					if(cnt > 0)
					{
						dtGridViewOwner.Rows[e.RowIndex].Cells["vlOwnerCode"].ReadOnly = true;
						dtGridViewOwner.Rows[e.RowIndex].Cells["vlOwnerCode"].Style.BackColor = Color.DarkGray;
					}
					else
					{
						dtGridViewOwner.Rows[e.RowIndex].Cells[e.ColumnIndex].ReadOnly = false;
						dtGridViewOwner.Rows[e.RowIndex].Cells[e.ColumnIndex].Style.BackColor = Color.White;
					}
				}
			}
		}

		/// <summary>
		/// set Rate Schedule to Readlonly in DataGrid if it is used in Street Light Account 
		/// this is to avoid updating the Value
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void DtGridViewRateSch_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
		{
			if(e.ColumnIndex == 0)
			{
				var obj = (StreetLightRateSchedule)dtGridViewRateSch.Rows[e.RowIndex].DataBoundItem;
				if(obj != null && !String.IsNullOrEmpty(obj.RateSchedule))
				{
					//check Rate Schedule in Street Light Account if so then set Rate Schedule to Readonly 
					var cnt = this.streetLightAccts.Where(s => s.RATE_SCHEDULE == obj.RateSchedule && s.EntityState != EntityMode.Delete).Count();
					if(cnt > 0)
					{
						dtGridViewRateSch.Rows[e.RowIndex].Cells["vlRateSchedule"].ReadOnly = true;
						dtGridViewRateSch.Rows[e.RowIndex].Cells["vlRateSchedule"].Style.BackColor = Color.DarkGray;
					}
					else
					{
						dtGridViewRateSch.Rows[e.RowIndex].Cells[e.ColumnIndex].ReadOnly = false;
						dtGridViewRateSch.Rows[e.RowIndex].Cells[e.ColumnIndex].Style.BackColor = Color.White;
					}
				}

			}
		}

		/// <summary>
		/// set Description to Readlonly in DataGrid if it is used in Street Light Account 
		/// this is to avoid updating the Value
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void DtGridViewDescription_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
		{
			if(dtGridViewDescription.Columns[e.ColumnIndex].Name == "vlDescription")
			{
				var obj = (StreetLightDescription)dtGridViewDescription.Rows[e.RowIndex].DataBoundItem;
				if(obj != null && !String.IsNullOrEmpty(obj.DESCRIPTION))
				{
					//check Description in Street Light Account if so then set Description to Readonly 
					var cnt = this.streetLightAccts.Where(s => s.Description == obj.DESCRIPTION && s.EntityState != EntityMode.Delete).Count();
					if(cnt > 0)
					{
						dtGridViewDescription.Rows[e.RowIndex].Cells["vlDescription"].ReadOnly = true;
						dtGridViewDescription.Rows[e.RowIndex].Cells["vlDescription"].Style.BackColor = Color.DarkGray;
					}
					else
					{
						dtGridViewDescription.Rows[e.RowIndex].Cells[e.ColumnIndex].ReadOnly = false;
						dtGridViewDescription.Rows[e.RowIndex].Cells[e.ColumnIndex].Style.BackColor = Color.White;
					}
				}
			}

		}

		/// <summary>
		/// set Rate code to Readlonly in DataGrid if it is used in Street Light Account 
		/// this is to avoid updating the Value
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void DtGridviewRateCode_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
		{
			if(e.ColumnIndex == 0)
			{
				var obj = (StreetLightRateCode)dtGridviewRateCode.Rows[e.RowIndex].DataBoundItem;
				if(obj != null && !String.IsNullOrEmpty(obj.RateCode))
				{
					//check Rate code in Street Light Account if so then set Rate code to Readonly 
					var cnt = this.streetLightAccts.Where(s => s.RATE_CODE == obj.RateCode && s.EntityState != EntityMode.Delete).Count();
					if(cnt > 0)
					{
						dtGridviewRateCode.Rows[e.RowIndex].Cells["vlRateCode"].ReadOnly = true;
						dtGridviewRateCode.Rows[e.RowIndex].Cells["vlRateCode"].Style.BackColor = Color.DarkGray;
					}
					else
					{
						dtGridviewRateCode.Rows[e.RowIndex].Cells[e.ColumnIndex].ReadOnly = false;
						dtGridviewRateCode.Rows[e.RowIndex].Cells[e.ColumnIndex].Style.BackColor = Color.White;
					}

				}
			}
		}

		#endregion

		#region Street Light Account Data Grid Events

		/// <summary>
		/// Enable/Disbable button controls on main from when selected row on Street Light Account data grid 
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void DtGridViewStreetAcct_SelectionChanged(object sender, EventArgs e)
		{
			btnShowBndry.Enabled = false;
			btnUnsetBndry.Enabled = false;
			btnSetBoundary.Enabled = false;
			btnNonLocated.Enabled = false;

			if(dtGridViewStreetAcct.SelectedRows.Count > 0)
			{
				//enable Set boundary button when select row.

				var obj = (StreetLightAccount)dtGridViewStreetAcct.SelectedRows[0].DataBoundItem;
				if(obj != null)
				{
					btnSetBoundary.Enabled = true;
					btnNonLocated.Enabled = true;
					/*
					 * Check bounary assigned to Street Light Account 
					 * if yes then enable show and Unset button main form else disable
					 */
					if(!string.IsNullOrEmpty(obj.BOUNDARY_CLASS))
					{
						btnShowBndry.Enabled = true;
						btnUnsetBndry.Enabled = true;
					}
				}
			}
		}


		/// <summary>
		/// Set ESI_Location PadLeft with 0 and checks user enter ESI_Location is unique otherwise log message in errorLog
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void DtGridViewStreetAcct_CellValueChanged(object sender, DataGridViewCellEventArgs e)
		{
			if(e.ColumnIndex == 0)
			{
				var obj = (StreetLightAccount)dtGridViewStreetAcct.Rows[e.RowIndex].DataBoundItem;
				if(!string.IsNullOrEmpty(obj.ESI_LOCATION) && obj.ESI_LOCATION.Length < 10)
				{
					obj.ESI_LOCATION = obj.ESI_LOCATION.PadLeft(10, '0');
					if(this.streetLightAccts.Where<StreetLightAccount>(a => a.ESI_LOCATION == obj.ESI_LOCATION).Count() > 1) { errLogs.Add(new ErrorLog { ErrorIn = "Street Light Account", ErrorMessage = string.Format(UniqueConstValidationMsg, "ESI Location " + obj.ESI_LOCATION, "ESI Location") }); }
				}
			}
		}

		/// <summary>
		/// Scroll filter Criteria Grid when user scroll Street Light Account grid horizontally
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void DtGridViewStreetAcct_Scroll(object sender, ScrollEventArgs e)
		{
			if(e.ScrollOrientation == ScrollOrientation.HorizontalScroll)
			{
				this.dtGridViewFilter.FirstDisplayedScrollingColumnIndex = this.dtGridViewStreetAcct.FirstDisplayedScrollingColumnIndex;
				this.dtGridViewFilter.HorizontalScrollingOffset = e.NewValue;
			}
		}

		/// <summary>
		/// To maitain column width of Filter Criteria  same as Street Light Account grid
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void dtGridViewStreetAcct_ColumnWidthChanged(object sender, DataGridViewColumnEventArgs e)
		{
			dtGridViewFilter.Columns[e.Column.Index].Width = e.Column.Width;
			this.dtGridViewFilter.HorizontalScrollingOffset = this.dtGridViewStreetAcct.HorizontalScrollingOffset;
			dtGridViewFilter.Refresh();
		}

		/// <summary>
		/// Suppress Error while loading data into Street Light Accounts
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="anError"></param>
		private void dtGridViewStreetAcct_DataError(object sender, DataGridViewDataErrorEventArgs anError)
		{
			anError.ThrowException = false;
		}

		/// <summary>
		/// Sort columns when user click on column header
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void DtGridViewStreetAcct_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
		{
			if(dtGridViewStreetAcct.Rows.Count > 0)
			{

				var column = dtGridViewStreetAcct.Columns[e.ColumnIndex];
				if(column.SortMode != DataGridViewColumnSortMode.Programmatic)
					return;

				var sortGlyph = column.HeaderCell.SortGlyphDirection;
				switch(sortGlyph)
				{
					case SortOrder.None:
					case SortOrder.Ascending:
						dtGridViewStreetAcct.DataSource = new BindingList<StreetLightAccount>(streetLightAccts.OrderByDescending(s => s.GetType().GetProperty(column.DataPropertyName).GetValue(s, null)).ToList());
						column.HeaderCell.SortGlyphDirection = SortOrder.Descending;
						break;
					case SortOrder.Descending:
						dtGridViewStreetAcct.DataSource = new BindingList<StreetLightAccount>(streetLightAccts.OrderBy(s => s.GetType().GetProperty(column.DataPropertyName).GetValue(s, null)).ToList());
						column.HeaderCell.SortGlyphDirection = SortOrder.Ascending;
						break;
				}
			}
		}

		/// <summary>
		/// Fires when user enter criteria to filter Street Light Accounts
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void DtGridViewFilter_CellValueChanged(object sender, DataGridViewCellEventArgs e)
		{
			if(dtGridViewFilter.Rows.Count > 0)
			{
				List<StreetLightAccount> filterAcct = ((List<StreetLightAccount>)dtGridViewFilter.DataSource);
				//checks filter criteria
				if(filterAcct[0].ESI_LOCATION == "" && filterAcct[0].Description == "" && filterAcct[0].Wattage == "" && filterAcct[0].LAMP_TYPE == "" && filterAcct[0].LUMINARE_STYLE == "" && filterAcct[0].RATE_SCHEDULE == "")
				{
					dtGridViewStreetAcct.DataSource = this.streetLightAccts;
				}
				else
				{
					//Filter Street Light Accounts as per user inpput criteria
					FilterStlAccountByCriteria(filterAcct[0].ESI_LOCATION, Convert.ToString(filterAcct[0].Description), filterAcct[0].Wattage, filterAcct[0].LAMP_TYPE
							, filterAcct[0].LUMINARE_STYLE, filterAcct[0].RATE_SCHEDULE);

				}

			}
		}


		#endregion

		#region DataGrid Description events

		/// <summary>
		/// Popup Calendar Control when user click on MSLA_DATE column in dtGridViewDescription
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void DtGridViewDescription_CellClick(object sender, DataGridViewCellEventArgs e)
		{
			// If any cell is clicked on the Column "MSLA_Date" in Data Grid
			if(e.RowIndex >= 0 && e.ColumnIndex >= 0 && dtGridViewDescription.Columns[e.ColumnIndex].Name == "vlMSLADate")
			{
				// It returns the retangular area that represents the Display area for a cell  
				Rectangle oRectangle = dtGridViewDescription.GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, false);

				oDateTimePicker.Size = new Size(oRectangle.Width, oRectangle.Height);
				oDateTimePicker.Location = new Point(oRectangle.X, oRectangle.Y);

				oDateTimePicker.Visible = true;

				dtGridViewDescription.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = oDateTimePicker.Text.ToString();
				dtGridViewDescription.CommitEdit(DataGridViewDataErrorContexts.Commit);
				dtGridViewDescription.Refresh();
			}
		}

		/// <summary>
		/// Assign Selected Data to DataGrdi Cell
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void dateTimePicker_OnTextChange(object sender, EventArgs e)
		{
			// Saving the 'Selected Date on Calendar' into DataGridView current cell  
			dtGridViewDescription.CurrentCell.Value = oDateTimePicker.Text.ToString();
		}

		/// <summary>
		/// close Data time picker controlr
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		void oDateTimePicker_CloseUp(object sender, EventArgs e)
		{
			// Hiding the control after use   
			oDateTimePicker.Visible = false;
		}
		#endregion

		#region  Boundary Events

		private void DtGridViewBndry_DataError(object sender, DataGridViewDataErrorEventArgs anError)
		{
			anError.ThrowException = false;
		}

		#endregion

		#region Button click events

		private void btnNonLocated_Click(object sender, EventArgs e)
		{
			ManageNonLocatedSTLT stltNonLocated = null;
			StreetLightBoundary bndry = null;
			if(dtGridViewStreetAcct.SelectedRows.Count > 0)
			{
				StreetLightAccount stltAcct = (StreetLightAccount)dtGridViewStreetAcct.SelectedRows[0].DataBoundItem;
				if(stltAcct != null)
				{

					// If the structure FID for this account is invalid (or is not yet posted to master data), then exit.
					if(!ValidateNonlocatedStructure(stltAcct.MiscStructFid))
					{
						return;
					}

					if(!string.IsNullOrEmpty(stltAcct.BOUNDARY_CLASS))
					{
						bndry = this.streetLightBndrys.Where(b => b.Bnd_Class == Convert.ToInt32(stltAcct.BOUNDARY_CLASS)).FirstOrDefault();
						stltAcct.BndryFno = bndry.Bnd_Fno;

						if(assignedBoundarys.ContainsKey(stltAcct.ESI_LOCATION))
						{
							stltAcct.BndryFid = assignedBoundarys[stltAcct.ESI_LOCATION];

							if(!ValidateAccountBoundaryGeometry(stltAcct.BndryFno, stltAcct.BndryFid))
							{
								return;
							}

						}
						else
						{
							stltAcct.BndryFid = this.stltBounndryCtx.GetBoundaryByIDValue(bndry, stltAcct.BOUNDARY_ID);

							//  check if unique boundary exists otherwise throw the error message 
							if(0 == stltAcct.BndryFid)
							{
								string msg = string.Format("Non-located lights cannot be created because a unique Boundary was not found where  Boundary Fno ={0} and Boundary Identifier attribute '{1}'='{2}'", bndry.Bnd_Fno, bndry.Bnd_ID_G3efield, stltAcct.BOUNDARY_ID);
								if(!string.IsNullOrEmpty(bndry.Bnd_Type))
								{
									msg += string.Format(" and Boundary Type atribute '{0}'='{1}'", bndry.Bnd_Type_G3eField, bndry.Bnd_Type);
								}
								msg += string.Format("{0}{0}Please check with an administrator to resolve this issue.", Environment.NewLine);
								MessageBox.Show(msg, "G/Technology", MessageBoxButtons.OK, MessageBoxIcon.Error);
								return;
							}

							if(!ValidateAccountBoundaryGeometry(stltAcct.BndryFno,stltAcct.BndryFid))
							{
								return;
							}

							assignedBoundarys.Add(stltAcct.ESI_LOCATION, stltAcct.BndryFid);
						}
					}

					// ALM 2044 - Set the CU for the current street light.
					// The same customer-owned CU will apply to all non-located lights for the active account.
					// If there's not a customer-owned CU for this account, then stop the user from accessing the non-located lights dialog.
					string CU = CommonUtil.CustomerOwnedSteetLightCU(stltAcct.LAMP_TYPE, stltAcct.Wattage, stltAcct.LUMINARE_STYLE);

					if(string.IsNullOrEmpty(CU))
					{
						string msg = string.Format("Non-located lights cannot be created because a unique customer-owned CU was not found where Lamp Type = {0} and Wattage = {1} and Luminaire Style = {2}.{3}{3}Please check with an administrator to resolve this issue.", stltAcct.LAMP_TYPE, stltAcct.Wattage, stltAcct.LUMINARE_STYLE, Environment.NewLine);
						MessageBox.Show(msg, "G/Technology", MessageBoxButtons.OK, MessageBoxIcon.Error);
						return;
					}

					stltNonLocated = new ManageNonLocatedSTLT(_gtCustomCommandHelper, _gtTransactionManager);
					stltNonLocated.FormClosed += StltNonLocated_FormClosed;

					stltNonLocated.LoadStreetLight(stltAcct);
					stltNonLocated.CU = CU;
					this.Visible = false;
					stltNonLocated.Show(this);
				}
			}
		}

		private bool ValidateAccountBoundaryGeometry(short FNO, int FID)
		{
			// Check to see if we can retrieve the geometry for the boundary.  If not, inform the user and exit.
			IGTDDCKeyObjects bndKOs = _gtApp.DataContext.GetDDCKeyObjects(FNO, FID, GTComponentGeometryConstants.gtddcgPrimaryGeographic);

			if(null == bndKOs || 0 == bndKOs.Count || null != bndKOs[0].Geometry)
			{
				string errMsg = "Unable to retrieve geometry for the boundary associated with this Street Light Account.  Please check with an administrator to resolve this issue.";
				MessageBox.Show(errMsg, "G/Technology", MessageBoxButtons.OK, MessageBoxIcon.Error);
				return false;
			}

			return true;
		}

		/// <summary>
		/// Assigne Selected boundary from Street Light boundary to Selected street Light Account
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void btnSetBoundary_Click(object sender, EventArgs e)
		{
			StreetLightBndry stltBndry = new StreetLightBndry(this.streetLightBndrys.Where(b => b.EntityState != EntityMode.Delete).ToList());
			if(stltBndry.ShowDialog() == DialogResult.OK)
			{
				if(dtGridViewStreetAcct.SelectedRows.Count > 0)
				{
					var obj = (StreetLightAccount)dtGridViewStreetAcct.SelectedRows[0].DataBoundItem;
					if(obj != null)
					{
						obj.BOUNDARY_CLASS = stltBndry.SelectedBndClass;
						obj.BOUNDARY_ID = stltBndry.SelectedIDValue;
						obj.BndryFid = stltBndry.SelectedBndryG3eFid;
						//Add selected boundary and street light account to cache
						if(!assignedBoundarys.ContainsKey(obj.ESI_LOCATION))
						{
							assignedBoundarys.Add(obj.ESI_LOCATION, stltBndry.SelectedBndryG3eFid);
						}
						else
						{
							assignedBoundarys[obj.ESI_LOCATION] = stltBndry.SelectedBndryG3eFid;
						}

						//enable show and Unset Boundary buttons
						btnShowBndry.Enabled = true;
						btnUnsetBndry.Enabled = true;
						dtGridViewBndry.Refresh();

					}
				}
			}
		}

		/// <summary>
		/// Remove assigned Boundary from Selected Street Light Account
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void btnUnsetBndry_Click(object sender, EventArgs e)
		{
			if(MessageBox.Show(UnSetBoundaryMsg, StreetLightAcctEditorTxt, MessageBoxButtons.YesNo) == DialogResult.Yes)
			{
				if(dtGridViewStreetAcct.SelectedRows.Count > 0)
				{
					var obj = (StreetLightAccount)dtGridViewStreetAcct.SelectedRows[0].DataBoundItem;
					if(obj != null)
					{
						obj.BOUNDARY_CLASS = null;
						obj.BOUNDARY_ID = null;
						//remove street light account from cache
						if(assignedBoundarys.ContainsKey(obj.ESI_LOCATION)) { assignedBoundarys.Remove(obj.ESI_LOCATION); }

						//Disable Show Boundary and unset boundary buttons
						btnShowBndry.Enabled = false;
						btnUnsetBndry.Enabled = false;
						dtGridViewBndry.Refresh();
					}
				}
			}
		}

		/// <summary>
		/// Fit boudanry assigned to selected row(Street light Account) on Map window
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void btnShowBndry_Click(object sender, EventArgs e)
		{
			if(dtGridViewStreetAcct.SelectedRows.Count > 0)
			{
				var obj = (StreetLightAccount)dtGridViewStreetAcct.SelectedRows[0].DataBoundItem;
				if(obj != null)
				{
					StreetLightBoundary bndry = this.streetLightBndrys.Where<StreetLightBoundary>(b => b.Bnd_Class.ToString() == obj.BOUNDARY_CLASS).First();
					/*
					 * check cache assigned boundary list for selected Street Light Account
					 * if exist get Boundary G3e_fid from list 
					 * else query component table get the boundary g3e_fid
					 */
					if(!assignedBoundarys.ContainsKey(obj.ESI_LOCATION))
					{
						int g3eFid = stltBounndryCtx.GetBoundaryByIDValue(bndry, obj.BOUNDARY_ID);
						//  check if unique boundary exists otherwise throw the error message 
						if(0 == g3eFid)
						{
							string msg = string.Format("Unique Boundary was not found  where  Boundary Fno ={0} and Boundary Identifier attribute '{1}'='{2}'", bndry.Bnd_Fno, bndry.Bnd_ID_G3efield, obj.BOUNDARY_ID);
							if(!string.IsNullOrEmpty(bndry.Bnd_Type))
							{
								msg += string.Format(" and Boundary Type atribute '{0}'='{1}'", bndry.Bnd_Type_G3eField, bndry.Bnd_Type);
							}
							msg += string.Format("{0}{0}Please check with an administrator to resolve this issue.", Environment.NewLine);
							MessageBox.Show(msg, "G/Technology", MessageBoxButtons.OK, MessageBoxIcon.Error);
							return;
						}
						else
						{
							//Add Street Light Account and Boundary G3efid to cache
							assignedBoundarys.Add(obj.ESI_LOCATION, g3eFid);
						}
					}
					//Fit Boundary Assigned to Street ligght account on map window.
					CommonUtil.FitSelectedFeature(bndry.Bnd_Fno, assignedBoundarys[obj.ESI_LOCATION]);
				}
			}
		}



		/// <summary>
		/// save Street Light Account and Value List edits to the Database
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void btnSave_Click(object sender, EventArgs e)
		{
			try
			{
				//validate Street Light Account and Value Lists
				if(!ValidatePendingChanges())
				{

					//Save the Street Light Boundary
					foreach(StreetLightBoundary stltBoundary in this.streetLightBndrys.Where<StreetLightBoundary>(s => s.EntityState != EntityMode.Review))
					{
						try
						{
							stltBounndryCtx.SaveStreetLightBoundary(stltBoundary);
						}
						catch(Exception ex)
						{
							LogErrors(StreetLightBndryTxt, ex.Message);
						}
					}


					//Save the Street Light Rate code edits
					foreach(StreetLightRateCode stltRateCode in this.vlStreetLightRateCodes.Where<StreetLightRateCode>(s => s.EntityState != EntityMode.Review))
					{
						try
						{
							stltValueLstCtx.SaveStreetLightRateCode(stltRateCode);
						}
						catch(Exception ex)
						{
							LogErrors(RateCodeValueLstTxt, ex.Message);
						}
					}

					//Save the Street Light Rate Schedule edits
					foreach(StreetLightRateSchedule stltRateSchedule in this.vlStreetLightRateSchedules.Where<StreetLightRateSchedule>(s => s.EntityState != EntityMode.Review))
					{
						try
						{
							stltValueLstCtx.SaveSteetLightRateSchedule(stltRateSchedule);
						}
						catch(Exception ex)
						{
							LogErrors(RateScheduleValueLstTxt, ex.Message);
						}
					}

					////Save the Street Light Owner edits
					foreach(StreetLightOwner stltOwner in this.vlStreetLightOwner.Where<StreetLightOwner>(s => s.EntityState != EntityMode.Review))
					{
						try
						{
							stltValueLstCtx.SaveStreetLightOwner(stltOwner);
						}
						catch(Exception ex)
						{
							LogErrors(OwnerValueLstTxt, ex.Message);
						}
					}

					//Save the Street Light Description edits
					foreach(StreetLightDescription stltDesc in this.vlStreetLightDesc.Where<StreetLightDescription>(s => s.EntityState != EntityMode.Review))
					{
						try
						{
							stltValueLstCtx.SaveStreetLightDesc(stltDesc);
						}
						catch(Exception ex)
						{
							LogErrors(DescriptionValueLstTxt, ex.Message);
						}
					}
					//Save the Street Light Accounts
					foreach(StreetLightAccount stltAccount in this.streetLightAccts.Where<StreetLightAccount>(s => s.EntityState != EntityMode.Review))
					{
						try
						{
							//Save Street Light Account Changes
							stltAccountCtx.SaveStreetLightAcct(stltAccount);
						}
						catch(Exception ex)
						{
							LogErrors(StreetLightAcctTxt, ex.Message);
						}
					}
					if(errLogs.Count == 0)
					{
						//commit the transaction
						CommitTransaction(true);
						MessageBox.Show("Pending changes commited successfully.", StreetLightAcctEditorTxt, MessageBoxButtons.OK, MessageBoxIcon.Information);
						RefreshDataGrids();
					}
					else
					{
						CommitTransaction(false);
					}
				}

			}
			catch(Exception ex)
			{
				MessageBox.Show("Error " + ex.Message, StreetLightAcctEditorTxt, MessageBoxButtons.OK, MessageBoxIcon.Error);
				//Rollback the changes
				CommitTransaction(false);
				errLogs.Clear();
			}
			if(errLogs.Count > 0)
			{
				ErrorMsgLog err = new ErrorMsgLog();
				err.ErrorLog = this.errLogs;
				err.ShowDialog();
				errLogs.Clear();
			}
		}

		/// <summary>
		/// close the Form
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void btnExit_Click(object sender, EventArgs e)
		{
			this.Close();
		}
		#endregion

		#region Form events
		/// <summary>
		/// Prompt the user beforing existing if there are any pending Edits to commit
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void StreetLighAccountEditor_FormClosing(object sender, FormClosingEventArgs e)
		{
			if(vlStreetLightRateCodes.Where(a => a.EntityState != EntityMode.Review).Count() > 0
					|| this.vlStreetLightRateSchedules.Where(a => a.EntityState != EntityMode.Review).Count() > 0
					|| this.vlStreetLightDesc.Where(a => a.EntityState != EntityMode.Review).Count() > 0
					|| this.vlStreetLightOwner.Where(a => a.EntityState != EntityMode.Review).Count() > 0
					|| this.streetLightAccts.Where(a => a.EntityState != EntityMode.Review).Count() > 0)
			{
				if(MessageBox.Show(PendingChangesMsg, StreetLightAcctEditorTxt, MessageBoxButtons.YesNo) == DialogResult.Yes)
				{
					e.Cancel = true;
					return;
				}
			}
			CleanUp();
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void StltNonLocated_FormClosed(object sender, FormClosedEventArgs e)
		{

			this.assignedSTLTAccounts = stltAccountCtx.GetAccountsAssignedToSTLT();
			this.Visible = true;
		}

		/// <summary>
		/// enable Non located and bounary control when user select Account tab
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void StreetLightTabCtrl_Selected(object sender, TabControlEventArgs e)
		{
			if(e.TabPageIndex == 0)
			{
				btnNonLocated.Visible = true;
				btnSetBoundary.Visible = true;
				btnShowBndry.Visible = true;
				btnUnsetBndry.Visible = true;
			}
			else
			{
				btnNonLocated.Visible = false;
				btnSetBoundary.Visible = false;
				btnShowBndry.Visible = false;
				btnUnsetBndry.Visible = false;
			}

			btnShowBndry.Enabled = false;
			btnUnsetBndry.Enabled = false;
		}
		#endregion

		#endregion

		#region Private Methods

		/// <summary>
		/// Set Columns in Street Light Account Datagrid to readonly
		/// </summary>
		private void SetColumnsReadOnly()
		{

			dtGridViewStreetAcct.Columns["Previous_Count"].ReadOnly = true;
			dtGridViewStreetAcct.Columns["Current_Count"].ReadOnly = true;
			dtGridViewStreetAcct.Columns["Threshold_State"].ReadOnly = true;
			dtGridViewStreetAcct.Columns["LastRunDate"].ReadOnly = true;
			dtGridViewStreetAcct.Columns["LastModifiedBy"].ReadOnly = true;
			dtGridViewStreetAcct.Columns["CreationDate"].ReadOnly = true;
			dtGridViewStreetAcct.Columns["Boundary"].ReadOnly = true;

			dtGridViewStreetAcct.Columns["Previous_Count"].DefaultCellStyle.BackColor = Color.DarkGray;
			dtGridViewStreetAcct.Columns["Current_Count"].DefaultCellStyle.BackColor = Color.DarkGray;
			dtGridViewStreetAcct.Columns["Threshold_State"].DefaultCellStyle.BackColor = Color.DarkGray;
			dtGridViewStreetAcct.Columns["LastRunDate"].DefaultCellStyle.BackColor = Color.DarkGray;
			dtGridViewStreetAcct.Columns["LastModifiedBy"].DefaultCellStyle.BackColor = Color.DarkGray;
			dtGridViewStreetAcct.Columns["CreationDate"].DefaultCellStyle.BackColor = Color.DarkGray;
			dtGridViewStreetAcct.Columns["Boundary"].DefaultCellStyle.BackColor = Color.DarkGray;

			dtGridViewFilter.Columns["txt_Billable"].ReadOnly = true;
			dtGridViewFilter.Columns["txt_OwnerCode"].ReadOnly = true;
			dtGridViewFilter.Columns["txt_RateCode"].ReadOnly = true;
			dtGridViewFilter.Columns["txt_PreviousCount"].ReadOnly = true;
			dtGridViewFilter.Columns["txt_CurrentCount"].ReadOnly = true;
			dtGridViewFilter.Columns["txt_ThresholdPercent"].ReadOnly = true;
			dtGridViewFilter.Columns["txt_ThresholdOverride"].ReadOnly = true;
			dtGridViewFilter.Columns["txt_LastRunDate"].ReadOnly = true;
			dtGridViewFilter.Columns["txt_LastModifiedBy"].ReadOnly = true;
			dtGridViewFilter.Columns["txt_CreationDate"].ReadOnly = true;
			dtGridViewFilter.Columns["txt_Boundary"].ReadOnly = true;
			dtGridViewFilter.Columns["txt_Restricted"].ReadOnly = true;
			dtGridViewFilter.Columns["txt_ThresholdState"].ReadOnly = true;

			dtGridViewFilter.Columns["txt_Billable"].DefaultCellStyle.BackColor = Color.DarkGray;
			dtGridViewFilter.Columns["txt_OwnerCode"].DefaultCellStyle.BackColor = Color.DarkGray;
			dtGridViewFilter.Columns["txt_RateCode"].DefaultCellStyle.BackColor = Color.DarkGray;
			dtGridViewFilter.Columns["txt_PreviousCount"].DefaultCellStyle.BackColor = Color.DarkGray;
			dtGridViewFilter.Columns["txt_CurrentCount"].DefaultCellStyle.BackColor = Color.DarkGray;
			dtGridViewFilter.Columns["txt_ThresholdPercent"].DefaultCellStyle.BackColor = Color.DarkGray;
			dtGridViewFilter.Columns["txt_ThresholdOverride"].DefaultCellStyle.BackColor = Color.DarkGray;
			dtGridViewFilter.Columns["txt_ThresholdState"].DefaultCellStyle.BackColor = Color.DarkGray;
			dtGridViewFilter.Columns["txt_LastRunDate"].DefaultCellStyle.BackColor = Color.DarkGray;
			dtGridViewFilter.Columns["txt_LastModifiedBy"].DefaultCellStyle.BackColor = Color.DarkGray;
			dtGridViewFilter.Columns["txt_CreationDate"].DefaultCellStyle.BackColor = Color.DarkGray;
			dtGridViewFilter.Columns["txt_Boundary"].DefaultCellStyle.BackColor = Color.DarkGray;
			dtGridViewFilter.Columns["txt_Restricted"].DefaultCellStyle.BackColor = Color.DarkGray;


			dtGridViewFilter.Columns["txt_Billable"].DefaultCellStyle.SelectionBackColor = Color.DarkGray;
			dtGridViewFilter.Columns["txt_OwnerCode"].DefaultCellStyle.SelectionBackColor = Color.DarkGray;
			dtGridViewFilter.Columns["txt_RateCode"].DefaultCellStyle.SelectionBackColor = Color.DarkGray;
			dtGridViewFilter.Columns["txt_PreviousCount"].DefaultCellStyle.SelectionBackColor = Color.DarkGray;
			dtGridViewFilter.Columns["txt_CurrentCount"].DefaultCellStyle.SelectionBackColor = Color.DarkGray;
			dtGridViewFilter.Columns["txt_ThresholdPercent"].DefaultCellStyle.SelectionBackColor = Color.DarkGray;
			dtGridViewFilter.Columns["txt_ThresholdOverride"].DefaultCellStyle.SelectionBackColor = Color.DarkGray;
			dtGridViewFilter.Columns["txt_ThresholdState"].DefaultCellStyle.SelectionBackColor = Color.DarkGray;
			dtGridViewFilter.Columns["txt_LastRunDate"].DefaultCellStyle.SelectionBackColor = Color.DarkGray;
			dtGridViewFilter.Columns["txt_LastModifiedBy"].DefaultCellStyle.SelectionBackColor = Color.DarkGray;
			dtGridViewFilter.Columns["txt_CreationDate"].DefaultCellStyle.SelectionBackColor = Color.DarkGray;
			dtGridViewFilter.Columns["txt_Boundary"].DefaultCellStyle.SelectionBackColor = Color.DarkGray;
			dtGridViewFilter.Columns["txt_Restricted"].DefaultCellStyle.SelectionBackColor = Color.DarkGray;

			dtGridViewDescription.Columns["vlMSLADate"].ReadOnly = false;
			foreach(DataGridViewColumn column in dtGridViewStreetAcct.Columns)
			{
				column.SortMode = DataGridViewColumnSortMode.Programmatic;
			}

		}

		/// <summary>
		/// Bind Data Property Name to Data Grid columns
		/// </summary>
		private void BindDataProperties()
		{

			// Bind Data Property to Street Account DataGrid
			dtGridViewStreetAcct.Columns["ESI_Location"].DataPropertyName = "ESI_LOCATION";
			dtGridViewStreetAcct.Columns["Billable"].DataPropertyName = "Billable";
			dtGridViewStreetAcct.Columns["stDescription"].DataPropertyName = "Description";
			dtGridViewStreetAcct.Columns["OwnerCode"].DataPropertyName = "OWNER_CODE";
			dtGridViewStreetAcct.Columns["Wattage"].DataPropertyName = "Wattage";
			dtGridViewStreetAcct.Columns["Lamp_Type"].DataPropertyName = "LAMP_TYPE";
			dtGridViewStreetAcct.Columns["Luminare_Style"].DataPropertyName = "LUMINARE_STYLE";
			dtGridViewStreetAcct.Columns["Rate_Schedule"].DataPropertyName = "RATE_SCHEDULE";
			dtGridViewStreetAcct.Columns["Rate_Code"].DataPropertyName = "RATE_CODE";
			dtGridViewStreetAcct.Columns["Previous_Count"].DataPropertyName = "PREVIOUS_COUNT";
			dtGridViewStreetAcct.Columns["Current_Count"].DataPropertyName = "CURRENT_COUNT";
			dtGridViewStreetAcct.Columns["Threshold_Percent"].DataPropertyName = "THRESHOLD_PERCENT";
			dtGridViewStreetAcct.Columns["Threshold_State"].DataPropertyName = "THRESHOLD_STATE";
			dtGridViewStreetAcct.Columns["Threshold_Override"].DataPropertyName = "THRESHOLD_OVERRIDE";
			dtGridViewStreetAcct.Columns["LastRunDate"].DataPropertyName = "RUN_DATE";
			dtGridViewStreetAcct.Columns["LastModifiedBy"].DataPropertyName = "MODIFIED_BY";
			dtGridViewStreetAcct.Columns["CreationDate"].DataPropertyName = "CREATION_DATE";
			dtGridViewStreetAcct.Columns["Boundary"].DataPropertyName = "Boundary";
			dtGridViewStreetAcct.Columns["Restricted"].DataPropertyName = "Restricted";
			dtGridViewStreetAcct.Columns["EntityState"].DataPropertyName = "EntityState";


			// Bind Data Property to Street Ligth Value List DataGrid
			dtGridViewDescription.Columns["vlDescription"].DataPropertyName = "DESCRIPTION";
			dtGridViewDescription.Columns["vlMSLADate"].DataPropertyName = "MSLA_Date";


			dtGridViewOwner.Columns["vlOwnerCode"].DataPropertyName = "OwnerCode";
			dtGridViewOwner.Columns["vlOwnerName"].DataPropertyName = "OwnerName";

			dtGridViewRateSch.Columns["vlRateSchedule"].DataPropertyName = "RateSchedule";
			dtGridviewRateCode.Columns["vlRateCode"].DataPropertyName = "RateCode";


			//Bind Data propery to Street Light Boundry Datagrid
			dtGridViewBndry.Columns["bndryG3eFno"].DataPropertyName = "Bnd_Fno";
			dtGridViewBndry.Columns["bndryTypeG3eAno"].DataPropertyName = "Bnd_Type_Ano";
			dtGridViewBndry.Columns["bndryType"].DataPropertyName = "Bnd_Type";
			dtGridViewBndry.Columns["bndryIDG3eAno"].DataPropertyName = "Bnd_ID_Ano";



			this.vlWattages = stltAccountCtx.GetWattageValueList();
			this.vlLampTypes = stltAccountCtx.GetLampTypeValueList();
			this.vlLuminareStyles = stltAccountCtx.GetLuminaireStyleValueList();

			//Bind Data Property to Filter Datagrid

			dtGridViewFilter.Columns["txt_ESILocation"].DataPropertyName = "ESI_Location";
			dtGridViewFilter.Columns["txt_Description"].DataPropertyName = "DESCRIPTION";
			dtGridViewFilter.Columns["txt_Wattage"].DataPropertyName = "Wattage";
			dtGridViewFilter.Columns["txt_LampType"].DataPropertyName = "LAMP_TYPE";
			dtGridViewFilter.Columns["txt_LuminareStyle"].DataPropertyName = "LUMINARE_STYLE";
			dtGridViewFilter.Columns["txt_RateSchedule"].DataPropertyName = "RATE_SCHEDULE";

			dtGridViewFilter.Columns["txt_ESILocation"].DefaultCellStyle.SelectionBackColor = Color.White;
			dtGridViewFilter.Columns["txt_ESILocation"].DefaultCellStyle.SelectionForeColor = Color.Black;

			dtGridViewFilter.Columns["txt_Description"].DefaultCellStyle.SelectionBackColor = Color.White;
			dtGridViewFilter.Columns["txt_Description"].DefaultCellStyle.SelectionForeColor = Color.Black;


			dtGridViewFilter.Columns["txt_Wattage"].DefaultCellStyle.SelectionBackColor = Color.White;
			dtGridViewFilter.Columns["txt_Wattage"].DefaultCellStyle.SelectionForeColor = Color.Black;

			dtGridViewFilter.Columns["txt_LampType"].DefaultCellStyle.SelectionBackColor = Color.White;
			dtGridViewFilter.Columns["txt_LampType"].DefaultCellStyle.SelectionForeColor = Color.Black;

			dtGridViewFilter.Columns["txt_LuminareStyle"].DefaultCellStyle.SelectionBackColor = Color.White;
			dtGridViewFilter.Columns["txt_LuminareStyle"].DefaultCellStyle.SelectionForeColor = Color.Black;

			dtGridViewFilter.Columns["txt_RateSchedule"].DefaultCellStyle.SelectionBackColor = Color.White;
			dtGridViewFilter.Columns["txt_RateSchedule"].DefaultCellStyle.SelectionForeColor = Color.Black;

			stltFilterAccts = new List<StreetLightAccount>();
			stltFilterAccts.Add(new StreetLightAccount());
			dtGridViewFilter.DataSource = stltFilterAccts;

		}

		/// <summary>
		/// Get all Street Light Accounts and value lists
		/// </summary>
		private void RefreshDataGrids()
		{
			_gtApp.BeginWaitCursor();
			if(vlStreetLightRateCodes == null || vlStreetLightRateCodes.Count(r => r.EntityState != EntityMode.Review) > 0)
			{
				vlStreetLightRateCodes = stltValueLstCtx.GetSteetLightRateCode();
			}
			if(vlStreetLightRateSchedules == null || vlStreetLightRateSchedules.Count(r => r.EntityState != EntityMode.Review) > 0)
			{
				vlStreetLightRateSchedules = stltValueLstCtx.GetStreetLightRateSchedule();
			}

			if(vlStreetLightDesc == null || vlStreetLightDesc.Count(r => r.EntityState != EntityMode.Review) > 0)
			{
				vlStreetLightDesc = stltValueLstCtx.GetSteetLightDescription();
			}
			if(vlStreetLightOwner == null || vlStreetLightOwner.Count(r => r.EntityState != EntityMode.Review) > 0)
			{
				vlStreetLightOwner = stltValueLstCtx.GetSteetLightOwner();
			}
			if(streetLightAccts == null || streetLightAccts.Count(r => r.EntityState != EntityMode.Review) > 0)
			{
				streetLightAccts = stltAccountCtx.GetStreetLightAccounts();
			}
			assignedSTLTAccounts = stltAccountCtx.GetAccountsAssignedToSTLT();
			csiESILocations = stltAccountCtx.GetCisEsiLocations();

			//Get all Stret Light Boundarys
			if(streetLightBndrys == null || streetLightBndrys.Count(r => r.EntityState != EntityMode.Review) > 0)
			{
				streetLightBndrys = stltBounndryCtx.GetSteetLightBoundarys();
			}
			_gtApp.EndWaitCursor();
			//bind Data to DataGrid
			dtGridViewDescription.DataSource = this.vlStreetLightDesc;
			dtGridViewOwner.DataSource = this.vlStreetLightOwner;
			dtGridviewRateCode.DataSource = this.vlStreetLightRateCodes;
			dtGridViewRateSch.DataSource = this.vlStreetLightRateSchedules;
			dtGridViewBndry.DataSource = this.streetLightBndrys;
			dtGridViewStreetAcct.DataSource = this.streetLightAccts;
		}
		/// <summary>
		/// Bind Data to DataGrid 
		/// </summary>
		private void BindDataSourceToGrid()
		{
			//Get all Street Light Value Lists and Accounts
			_gtApp.BeginWaitCursor();
			vlStreetLightRateCodes = stltValueLstCtx.GetSteetLightRateCode();
			vlStreetLightRateSchedules = stltValueLstCtx.GetStreetLightRateSchedule();
			vlStreetLightDesc = stltValueLstCtx.GetSteetLightDescription();
			vlStreetLightOwner = stltValueLstCtx.GetSteetLightOwner();
			streetLightAccts = stltAccountCtx.GetStreetLightAccounts();
			assignedSTLTAccounts = stltAccountCtx.GetAccountsAssignedToSTLT();
			csiESILocations = stltAccountCtx.GetCisEsiLocations();

			//Get all Stret Light Boundarys
			streetLightBndrys = stltBounndryCtx.GetSteetLightBoundarys();
			_gtApp.EndWaitCursor();

			//Bind Data Property to combobox cell
			((DataGridViewComboBoxColumn)dtGridViewStreetAcct.Columns["stDescription"]).DisplayMember = "Description";
			((DataGridViewComboBoxColumn)dtGridViewStreetAcct.Columns["stDescription"]).ValueMember = "Description";

			((DataGridViewComboBoxColumn)dtGridViewStreetAcct.Columns["OwnerCode"]).DisplayMember = "OwnerName";
			((DataGridViewComboBoxColumn)dtGridViewStreetAcct.Columns["OwnerCode"]).ValueMember = "OwnerCode";


			((DataGridViewComboBoxColumn)dtGridViewStreetAcct.Columns["Rate_Schedule"]).DisplayMember = "RateSchedule";
			((DataGridViewComboBoxColumn)dtGridViewStreetAcct.Columns["Rate_Schedule"]).ValueMember = "RateSchedule";

			((DataGridViewComboBoxColumn)dtGridViewStreetAcct.Columns["Rate_Code"]).DisplayMember = "RateCode";
			((DataGridViewComboBoxColumn)dtGridViewStreetAcct.Columns["Rate_Code"]).ValueMember = "RateCode";

			((DataGridViewComboBoxColumn)dtGridViewStreetAcct.Columns["Lamp_Type"]).DisplayMember = "KeyValue";
			((DataGridViewComboBoxColumn)dtGridViewStreetAcct.Columns["Lamp_Type"]).ValueMember = "Key";

			((DataGridViewComboBoxColumn)dtGridViewStreetAcct.Columns["Wattage"]).DisplayMember = "KeyValue";
			((DataGridViewComboBoxColumn)dtGridViewStreetAcct.Columns["Wattage"]).ValueMember = "Key";

			((DataGridViewComboBoxColumn)dtGridViewStreetAcct.Columns["Luminare_Style"]).DisplayMember = "KeyValue";
			((DataGridViewComboBoxColumn)dtGridViewStreetAcct.Columns["Luminare_Style"]).ValueMember = "Key";


			((DataGridViewComboBoxColumn)dtGridViewStreetAcct.Columns["stDescription"]).DataSource = this.vlStreetLightDesc;
			((DataGridViewComboBoxColumn)dtGridViewStreetAcct.Columns["OwnerCode"]).DataSource = this.vlStreetLightOwner;
			((DataGridViewComboBoxColumn)dtGridViewStreetAcct.Columns["Rate_Schedule"]).DataSource = this.vlStreetLightRateSchedules;
			((DataGridViewComboBoxColumn)dtGridViewStreetAcct.Columns["Rate_Code"]).DataSource = this.vlStreetLightRateCodes;

			((DataGridViewComboBoxColumn)dtGridViewStreetAcct.Columns["Lamp_Type"]).DataSource = this.vlLampTypes;
			((DataGridViewComboBoxColumn)dtGridViewStreetAcct.Columns["Wattage"]).DataSource = this.vlWattages;
			((DataGridViewComboBoxColumn)dtGridViewStreetAcct.Columns["Luminare_Style"]).DataSource = this.vlLuminareStyles;

			//bind Data to DataGrid
			dtGridViewDescription.DataSource = this.vlStreetLightDesc;
			dtGridViewOwner.DataSource = this.vlStreetLightOwner;
			dtGridviewRateCode.DataSource = this.vlStreetLightRateCodes;
			dtGridViewRateSch.DataSource = this.vlStreetLightRateSchedules;
			dtGridViewBndry.DataSource = this.streetLightBndrys;
			dtGridViewStreetAcct.DataSource = this.streetLightAccts;
		}

		/// <summary>
		/// filter Street Light Account as per input criteria
		/// </summary>
		/// <param name="esiLocation"></param>
		/// <param name="desc"></param>
		/// <param name="rateSchedule"></param>
		private void FilterStlAccountByCriteria(string esiLocation, string desc, string wattage, string lampType, string lmStyle, string rateSchedule)
		{

			var filterAccts = this.streetLightAccts.ToList<StreetLightAccount>();
			if(!string.IsNullOrEmpty(esiLocation))
			{
				filterAccts = filterAccts.Where<StreetLightAccount>(a => a.ESI_LOCATION.ToUpper().Contains(esiLocation.ToUpper())).ToList<StreetLightAccount>();
			}
			if(!string.IsNullOrEmpty(desc))
			{
				filterAccts = filterAccts.Where<StreetLightAccount>(a => a.Description.ToUpper().Contains(desc.ToUpper())).ToList<StreetLightAccount>();
			}
			if(!string.IsNullOrEmpty(wattage))
			{
				filterAccts = filterAccts.Where<StreetLightAccount>(a => a.Wattage.Contains(wattage)).ToList<StreetLightAccount>();
			}
			if(!string.IsNullOrEmpty(lampType))
			{
				filterAccts = filterAccts.Where<StreetLightAccount>(a => a.LAMP_TYPE.ToUpper().Contains(lampType.ToUpper())).ToList<StreetLightAccount>();
			}
			if(!string.IsNullOrEmpty(lmStyle))
			{
				var lmStyl = this.vlLuminareStyles.Where<LuminaireStyle>(a => a.KeyValue.ToUpper().Contains(lmStyle.ToUpper()));
				if(lmStyl != null && lmStyl.Count() > 0)
				{
					filterAccts = filterAccts.Where<StreetLightAccount>(a => a.LUMINARE_STYLE.Contains(lmStyl.FirstOrDefault().Key)).ToList<StreetLightAccount>();
				}
			}
			if(!string.IsNullOrEmpty(rateSchedule))
			{
				filterAccts = filterAccts.Where<StreetLightAccount>(a => a.RATE_SCHEDULE.ToUpper().Contains(rateSchedule.ToUpper())).ToList<StreetLightAccount>();
			}
			dtGridViewStreetAcct.DataSource = new BindingList<StreetLightAccount>(filterAccts);
		}

		/// <summary>
		/// Validate Pending Changes and log validation errors
		/// </summary>
		/// <returns></returns>
		private bool ValidatePendingChanges()
		{

			//Validate Street Light Account;
			ValidateStreetLightAccount();
			// Validate Value Lists
			ValidateValueList();
			//Validate  Street Light Boundary
			ValidateBoundary();
			return errLogs.Count > 0 ? true : false;
		}

		/// <summary>
		/// Validate Street Light Account and log Validation error in the error List
		/// </summary>
		private void ValidateStreetLightAccount()
		{

			//Validate Street Light Account
			foreach(StreetLightAccount act in this.streetLightAccts.Where(a => a.EntityState == EntityMode.Add))
			{
				if(string.IsNullOrEmpty(act.ESI_LOCATION)) { errLogs.Add(new ErrorLog { ErrorIn = StreetLightAcctTxt, ErrorMessage = string.Format(NotNullValidationMSg, "ESI Location ") }); }
				if(string.IsNullOrEmpty(act.LAMP_TYPE)) { errLogs.Add(new ErrorLog { ErrorIn = StreetLightAcctTxt, ErrorMessage = string.Format(NotNullValidationMSg, "For ESI Location " + act.ESI_LOCATION + " - Lamp Type ") }); }
				if(string.IsNullOrEmpty(act.LUMINARE_STYLE)) { errLogs.Add(new ErrorLog { ErrorIn = StreetLightAcctTxt, ErrorMessage = string.Format(NotNullValidationMSg, "For ESI Location " + act.ESI_LOCATION + " - Luminare Style ") }); }
				if(string.IsNullOrEmpty(act.OWNER_CODE)) { errLogs.Add(new ErrorLog { ErrorIn = StreetLightAcctTxt, ErrorMessage = string.Format(NotNullValidationMSg, "For ESI Location " + act.ESI_LOCATION + " - Owner Code ") }); }
				if(string.IsNullOrEmpty(act.Description)) { errLogs.Add(new ErrorLog { ErrorIn = StreetLightAcctTxt, ErrorMessage = string.Format(NotNullValidationMSg, "For ESI Location " + act.ESI_LOCATION + " - Description ") }); }
				if(string.IsNullOrEmpty(act.RATE_CODE)) { errLogs.Add(new ErrorLog { ErrorIn = StreetLightAcctTxt, ErrorMessage = string.Format(NotNullValidationMSg, "For ESI Location " + act.ESI_LOCATION + " - Rate Code ") }); }
				if(string.IsNullOrEmpty(act.RATE_SCHEDULE)) { errLogs.Add(new ErrorLog { ErrorIn = StreetLightAcctTxt, ErrorMessage = string.Format(NotNullValidationMSg, "For ESI Location " + act.ESI_LOCATION + " - Rate Schedule ") }); }
				if(string.IsNullOrEmpty(act.Wattage)) { errLogs.Add(new ErrorLog { ErrorIn = StreetLightAcctTxt, ErrorMessage = string.Format(NotNullValidationMSg, "For ESI Location " + act.ESI_LOCATION + " - Wattage ") }); }

				//CC&B Validation - check ESI Location exists in CSI_ESI_Location
				if(!this.csiESILocations.Contains(act.ESI_LOCATION)) { errLogs.Add(new ErrorLog { ErrorIn = StreetLightAcctTxt, ErrorMessage = string.Format(CCBValidationMsg, act.ESI_LOCATION) }); }
			}
		}

		/// <summary>
		/// Validate Value Lists and log Validation error in the error List
		/// </summary>
		private void ValidateValueList()
		{

			//Validate Street Light Description Value List
			foreach(StreetLightDescription desc in this.vlStreetLightDesc.Where(a => a.EntityState == EntityMode.Add || a.EntityState == EntityMode.Update))
			{
				//Check Null Value
				if(string.IsNullOrEmpty(desc.DESCRIPTION) && desc.MSLA_Date != default(DateTime))
				{
					errLogs.Add(new ErrorLog { ErrorIn = DescriptionValueLstTxt, ErrorMessage = string.Format(NotNullValidationMSg, "Description") });
				}
				else
				{
					//check for duplicate Description exists
					if(this.vlStreetLightDesc.Where<StreetLightDescription>(a => a.DESCRIPTION == desc.DESCRIPTION).Count() > 1)
					{
						errLogs.Add(new ErrorLog { ErrorIn = DescriptionValueLstTxt, ErrorMessage = string.Format(UniqueConstValidationMsg, "Description  " + desc.DESCRIPTION, "Description") });
					}
				}
			}

			//Validate Street Light Owner Value List
			foreach(StreetLightOwner own in this.vlStreetLightOwner.Where(a => a.EntityState == EntityMode.Add || a.EntityState == EntityMode.Update))
			{
				//check for null
				if(string.IsNullOrEmpty(own.OwnerCode))
				{
					errLogs.Add(new ErrorLog { ErrorIn = OwnerValueLstTxt, ErrorMessage = string.Format(NotNullValidationMSg, "Owner Code") });
				}
				else
				{
					//check for Duplicate Owner code exists
					if(this.vlStreetLightOwner.Where<StreetLightOwner>(a => a.OwnerCode == own.OwnerCode).Count() > 1)
					{
						errLogs.Add(new ErrorLog { ErrorIn = OwnerValueLstTxt, ErrorMessage = string.Format(UniqueConstValidationMsg, "Owner Code  " + own.OwnerCode, "Owner Code") });
					}
				}
				//check for null
				if(string.IsNullOrEmpty(own.OwnerName)) { errLogs.Add(new ErrorLog { ErrorIn = OwnerValueLstTxt, ErrorMessage = string.Format(NotNullValidationMSg, "Owner Name") }); }
			}

			//Validate Street Light Rate code
			foreach(StreetLightRateCode rc in this.vlStreetLightRateCodes.Where(a => a.EntityState == EntityMode.Add || a.EntityState == EntityMode.Update))
			{
				if(string.IsNullOrEmpty(rc.RateCode))
				{
					errLogs.Add(new ErrorLog { ErrorIn = RateCodeValueLstTxt, ErrorMessage = string.Format(NotNullValidationMSg, "Rate Code") });
				}
				else
				{
					//check duplicate Rate code exists
					if(this.vlStreetLightRateCodes.Where<StreetLightRateCode>(a => a.RateCode == rc.RateCode).Count() > 1)
					{
						errLogs.Add(new ErrorLog { ErrorIn = RateCodeValueLstTxt, ErrorMessage = string.Format(UniqueConstValidationMsg, "Rate Code  " + rc.RateCode, "Rate Code") });
					}
				}
			}

			//Validate Street Light Rate Schedule
			foreach(StreetLightRateSchedule rs in this.vlStreetLightRateSchedules.Where(a => a.EntityState == EntityMode.Add || a.EntityState == EntityMode.Update))
			{

				if(string.IsNullOrEmpty(rs.RateSchedule))
				{
					errLogs.Add(new ErrorLog { ErrorIn = RateScheduleValueLstTxt, ErrorMessage = string.Format(NotNullValidationMSg, "Rate Schedule") });
				}
				else
				{
					//check duplicate Rate Schedule exists
					if(this.vlStreetLightRateSchedules.Where<StreetLightRateSchedule>(a => a.RateSchedule == rs.RateSchedule).Count() > 1)
					{
						errLogs.Add(new ErrorLog { ErrorIn = RateScheduleValueLstTxt, ErrorMessage = string.Format(UniqueConstValidationMsg, "Rate Schedule  " + rs.RateSchedule, "Rate Schedule") });
					}
				}
			}

		}

		/// <summary>
		/// Validate Street Light Boundary and log Validation error in the error List
		/// </summary>
		private void ValidateBoundary()
		{

			foreach(StreetLightBoundary bndry in this.streetLightBndrys.Where(a => a.EntityState == EntityMode.Add || a.EntityState == EntityMode.Update))
			{

				if(!CommonUtil.CheckForBoundaryFno(bndry.Bnd_Fno))
				{
					errLogs.Add(new ErrorLog { ErrorIn = StreetLightBndryTxt, ErrorMessage = string.Format("Feature Number " + bndry.Bnd_Fno + " is not Boundary Feature") });
				}
				else
				{

					//check attribute exists in Boundary feature component
					if(!CommonUtil.CheckAtributeExists(bndry.Bnd_ID_Ano, bndry.Bnd_Fno))
					{
						errLogs.Add(new ErrorLog { ErrorIn = StreetLightBndryTxt, ErrorMessage = string.Format("Attribute Identifier number " + bndry.Bnd_ID_Ano + " doesn't exists") });
					}
					else
					{
						if(bndry.Bnd_Type_Ano != default(int) && String.IsNullOrEmpty(bndry.Bnd_Type))
						{
							errLogs.Add(new ErrorLog { ErrorIn = StreetLightBndryTxt, ErrorMessage = "Boundary Type should not be null for the Boundary Type Attribute " + bndry.Bnd_Type_Ano });
						}
						else
						{
							//check attribute Type and Attribute ID attribute  exists in same Boundary feature component
							if(!CommonUtil.CheckAtributeExists(bndry.Bnd_ID_Ano, bndry.Bnd_Type_Ano, bndry.Bnd_Fno))
							{
								errLogs.Add(new ErrorLog { ErrorIn = StreetLightBndryTxt, ErrorMessage = String.Format("Boundary Type Attribute {0} and Boundary ID Attribute {1} should be from same component", bndry.Bnd_Type_Ano, bndry.Bnd_ID_Ano) });
							}
						}


						if(bndry.Bnd_Type_Ano == default(int) && !String.IsNullOrEmpty(bndry.Bnd_Type))
						{
							errLogs.Add(new ErrorLog { ErrorIn = StreetLightBndryTxt, ErrorMessage = "Boundary Type Attribute should not be null for the Boundary Type " + bndry.Bnd_Type });
						}
					}
				}
			}

		}

		/// <summary>
		/// Validates whether the Structure ID exists and, if so, whether it is in master data or only in a job edit.
		/// </summary>
		/// <param name="FID">G3E_FID value of a Structure</param>
		/// <returns>true if exists in master data; else, false.</returns>
		private bool ValidateNonlocatedStructure(int FID)
		{
			bool retVal = false;

			// If STLT_ACCOUNT.MISC_STRUCTURE_FID is null, the StreetLightAccount object
			// will end up with its structure FID set to zero.  Since MISC_STRUCTURE_FID
			// should never be zero, it is assumed that if zero is passed here, the column was NULL
			// and, in that case, it's okay to return true since that will
			// allow the non-located lights dialog to provide the method
			// for the user to set that structure.

			if(0 == FID)
			{
				retVal = true;
			}
			else
			{
				// Determine whether the feature exists in master data or only in a job and respond accordingly.
				try
				{
					IGTApplication app = GTClassFactory.Create<IGTApplication>();
					string sql = "select * from v_stlt_structure where g3e_fid=? order by ltt_id asc";
					Recordset rs = app.DataContext.OpenRecordset(sql, CursorTypeEnum.adOpenStatic, LockTypeEnum.adLockReadOnly, (int)CommandTypeEnum.adCmdText, FID);

					if(null != rs && 0 < rs.RecordCount)
					{
						rs.MoveFirst();

						// If first record's LTT_ID = 0, then the feature is in master data
						// so, regardless of any pending edits (LTT_ID != 0), return true.
						if(0 == Convert.ToInt32(rs.Fields["ltt_id"].Value))
						{
							retVal = true;
						}
						else
						{
							// If there are only job records (no master records), then there should only be
							// one job affecting this feature.  If that job is not the active job, then
							// return false, but notify the user which job has not yet posted the feature.
							string wrName = rs.Fields["ltt_name"].Value.ToString();

							if(wrName.Equals(app.DataContext.ActiveJob))
							{
								// If the active job is the same as the job in the structure's LTT values,
								// then this is an okay condition as the system will be able to locate that feature.
								retVal = true;
							}
							else
							{
								// Structure exists only in the context of a job other than the active job.
								MessageBox.Show(string.Format("Structure FID: {0} exists only as an unposted addition in WR: {1}.  Structure must be posted before proceeding to Nonlocated Light interface.", FID.ToString(), wrName), "G/Technology", MessageBoxButtons.OK, MessageBoxIcon.Error);
							}
						}
					}
					else
					{
						MessageBox.Show(string.Format("Structure FID: {0} does not exist.  Correct before editing non-located lights.", FID.ToString()), "G/Technology", MessageBoxButtons.OK, MessageBoxIcon.Error);
					}

					if(null != rs && (ConnectionState)rs.State == ConnectionState.Open)
					{
						rs.Close();
						rs = null;
					}

				}
				catch(Exception ex)
				{
					errLogs.Add(new ErrorLog { ErrorIn = "ValidateNonlocatedStructure", ErrorMessage = ex.Message });
				}
			}
			return retVal;
		}

		/// <summary>
		/// Log Errors in Error Log if error Message is Oracle Error otherwise throw Error Message
		/// </summary>
		/// <param name="errIn"></param>
		/// <param name="errMsg"></param>
		private void LogErrors(string errIn, string errMsg)
		{
			//Log Oracle error to show on Error dialog 
			if(errMsg.Contains("ORA"))
			{
				errLogs.Add(new ErrorLog { ErrorIn = errIn, ErrorMessage = errMsg });
				var msg = errMsg;
			}
			else
			{
				throw new Exception("Error in " + errIn + " :- " + errMsg);
			}
		}

		private void CommitTransaction(bool flag)
		{
			if(flag)
			{
				if(isJobActive) { _gtTransactionManager.Commit(); } else { CommonUtil.Execute("commit"); }
			}
			else
			{
				if(isJobActive) { _gtTransactionManager.Rollback(); } else { CommonUtil.Execute("Rollback"); }
			}
		}

		private void SyncStreetLightFilterGridColWidth()
		{
			for(int indx = 0;indx < dtGridViewStreetAcct.ColumnCount;indx++)
			{
				dtGridViewFilter.Columns[indx].Width = dtGridViewStreetAcct.Columns[indx].Width;
			}

		}
		#endregion

		#region Clean up

		/// <summary>
		/// Clean up
		/// </summary>
		public void CleanUp()
		{

			vlLampTypes = null;
			vlWattages = null;
			vlLuminareStyles = null;
			vlStreetLightRateCodes = null;
			vlStreetLightRateSchedules = null;
			vlStreetLightDesc = null;
			vlStreetLightOwner = null;

			streetLightAccts = null;
			stltFilterAccts = null;
			assignedSTLTAccounts = null;
			errLogs = null;

			stltAccountCtx = null;
			stltValueLstCtx = null;
			if(_gtTransactionManager != null)
			{
				if(_gtTransactionManager.TransactionInProgress)
					_gtTransactionManager.Rollback();
			}
			_gtTransactionManager = null;
			if(_gtCustomCommandHelper != null)
			{
				_gtCustomCommandHelper.Complete();
			}
			_gtCustomCommandHelper = null;

			this.Dispose();
		}
		#endregion
	}
}
