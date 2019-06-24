using GTechnology.Oncor.CustomAPI.DataAccess;
using GTechnology.Oncor.CustomAPI.Model;
using Intergraph.GTechnology.API;
using System;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Windows.Forms;

namespace GTechnology.Oncor.CustomAPI.GUI
{
	public partial class ManageNonLocatedSTLT : Form
	{
		BindingList<StreetLightNonLocated> streetLights = null;
		ManageNonLocatedSTLTContext stltNonLocatedCtx = null;
		StreetLightAccount streetLightAcct = null;
		string ManageNonLocatedTxt = "Manage Non-Located Street Light";
		string PendingChangesMsg = "There are pending edits to save.  Do you want to save your changes?";
		bool isJobActive;
		bool isPlacementEnabled = false;


		IGTApplication _gtApp = null;
		IGTKeyObject _gtKeyObj = null;
		IGTKeyObject _gtKeyEditObj = null;

		IGTTransactionManager _gtTransactionManager = null;
		IGTCustomCommandHelper _gtCustomCommandHelper;

		IGTFeatureExplorerService _gtFeatureExplorerSrvc = null;
		IGTFeaturePlacementService _gtPlacementSrvc = null;

		/// <summary>
		/// CU to be applied to all non-located lights for the active account.
		/// Since the CU has already been found before this form is generated,
		/// then allow the owner of this form to set the CU.
		/// </summary>
		internal string CU { set; get; }

		public ManageNonLocatedSTLT()
		{
			InitializeComponent();
			dtGridViewNonLocated.AutoGenerateColumns = false;
			_gtApp = GTClassFactory.Create<IGTApplication>();

			// Bind Data Property to Street Light Non Located data grid
			dtGridViewNonLocated.Columns["StltIdentifier"].DataPropertyName = "StltIdentifier";
			dtGridViewNonLocated.Columns["ConnectionStatus"].DataPropertyName = "ConnectionStatus";
			dtGridViewNonLocated.Columns["DisconnectDate"].DataPropertyName = "DisconnectDate";
			dtGridViewNonLocated.Columns["ConnectDate"].DataPropertyName = "ConnectDate";
			dtGridViewNonLocated.Columns["Location"].DataPropertyName = "Location";
			dtGridViewNonLocated.Columns["AdditionalLocation"].DataPropertyName = "AdditionalLocation";

            ((DataGridViewComboBoxColumn)dtGridViewNonLocated.Columns["ConnectionStatus"]).DisplayMember = "Value";
            ((DataGridViewComboBoxColumn)dtGridViewNonLocated.Columns["ConnectionStatus"]).ValueMember = "Key";

            stltNonLocatedCtx = new ManageNonLocatedSTLTContext();
            ((DataGridViewComboBoxColumn)dtGridViewNonLocated.Columns["ConnectionStatus"]).DataSource = stltNonLocatedCtx.GetConnectionStatusVL();
            isJobActive = !string.IsNullOrEmpty(_gtApp.DataContext.ActiveJob);
			//check Job is active
			if(isJobActive)
			{
				dtGridViewNonLocated.ReadOnly = false;
				dtGridViewNonLocated.AllowUserToAddRows = true;
				dtGridViewNonLocated.AllowUserToDeleteRows = true;
			}
			else
			{
				//Disable all button except Exit and set all rows in grid set to readonly
				dtGridViewNonLocated.ReadOnly = true;
				EnableButtonCtrls(false);
			}

			dtGridViewNonLocated.SelectionMode = DataGridViewSelectionMode.RowHeaderSelect;
			this.FormClosing += ManageNonLocatedSTLT_FormClosing;
		}

        
		public ManageNonLocatedSTLT(IGTCustomCommandHelper customCommandHelper, IGTTransactionManager gtTransaction) : this()
		{

			this._gtCustomCommandHelper = customCommandHelper;
			this._gtTransactionManager = gtTransaction;
			if(isJobActive)
			{
				_gtFeatureExplorerSrvc = GTClassFactory.Create<IGTFeatureExplorerService>(_gtCustomCommandHelper);
				_gtFeatureExplorerSrvc.Locked = false;
			}
			//Subscrive eventds
			SubscribEvents(true);
		}

		/// <summary>
		/// Load Street Lights assigned to given street light account
		/// </summary>
		/// <param name="stltAccount"></param>
		public void LoadStreetLight(StreetLightAccount stltAccount)
		{
			if(stltAccount.MiscStructFid == default(int) && string.IsNullOrEmpty(stltAccount.BOUNDARY_CLASS)) { dtGridViewNonLocated.AllowUserToAddRows = false; }
			this.streetLightAcct = stltAccount;
			this.streetLights = stltNonLocatedCtx.GetStreetLightByAccountID(stltAccount.ESI_LOCATION);
			dtGridViewNonLocated.DataSource = this.streetLights;
			//Change btnstructure text
			btnStructure.Text = (stltAccount.MiscStructFid == default(int)) ? "Place Structure" : "Locate Structue";
			txtStreetLightAcct.Text = "ESI Location:" + stltAccount.ESI_LOCATION + "," + stltAccount.Wattage + "," + stltAccount.LAMP_TYPE + "," + stltAccount.LUMINARE_STYLE;
		}

		#region DataGridView Events


		/// <summary>
		/// 
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void dtGridViewNonLocated_RowsAdded(object sender, DataGridViewRowsAddedEventArgs e)
		{
			if(!dtGridViewNonLocated.Rows[e.RowIndex].IsNewRow)
			{
				dtGridViewNonLocated.Rows[e.RowIndex].ReadOnly = true;
				dtGridViewNonLocated.Rows[e.RowIndex].DefaultCellStyle.BackColor = System.Drawing.Color.DarkGray;
                ((DataGridViewComboBoxCell)dtGridViewNonLocated.Rows[e.RowIndex].Cells["ConnectionStatus"]).DisplayStyle = DataGridViewComboBoxDisplayStyle.Nothing;
            }
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void dtGridViewNonLocated_UserDeletingRow(object sender, DataGridViewRowCancelEventArgs e)
		{
			var obj = (StreetLightNonLocated)e.Row.DataBoundItem;
			if(obj.EntityState != EntityMode.Add)
			{
				obj.EntityState = EntityMode.Delete;
				dtGridViewNonLocated.CurrentCell = null;
				e.Row.Visible = false;
				e.Cancel = true;
			}
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void dtGridViewNonLocated_CellValueChanged(object sender, DataGridViewCellEventArgs e)
		{

			if(e.ColumnIndex == 0)
			{
				var obj = (StreetLightNonLocated)dtGridViewNonLocated.Rows[e.RowIndex].DataBoundItem;
				if(obj != null && this.streetLightAcct.MiscStructFid == default(int) && !string.IsNullOrEmpty(this.streetLightAcct.BOUNDARY_CLASS))
				{
					PlaceMiscStructure(true);
					btnStructure.Text = "Locate Structue";
				}
			}
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void dtGridViewNonLocated_SelectionChanged(object sender, EventArgs e)
		{

			btnEditStreetLight.Enabled = false;
			if(dtGridViewNonLocated.SelectedRows.Count > 0)
			{
				var obj = (StreetLightNonLocated)dtGridViewNonLocated.SelectedRows[0].DataBoundItem;
				if(obj != null)
				{

					if(obj.EntityState != EntityMode.Add)
					{
						btnEditStreetLight.Enabled = true;
					}
					else
					{
						btnEditStreetLight.Enabled = false;
					}
				}
			}
		}

		#endregion

		#region Form Events

		/// <summary>
		/// 
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void ManageNonLocatedSTLT_Shown(object sender, EventArgs e)
		{
			if(!isJobActive)
			{
				MessageBox.Show("A GIS Maintenance job must be active before changing any non-located Street Lights.", ManageNonLocatedTxt, MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
			else
			{
				if(this.streetLightAcct.BndryFno == default(short) && this.streetLightAcct.MiscStructFid == default(int))
				{
					MessageBox.Show("A Miscellaneous Structure is not defined for this Street Light Account and the system cannot automatically generate one properly \n because the selected Street Light Account does not have an Account Boundary defined for it.", ManageNonLocatedTxt, MessageBoxButtons.OK, MessageBoxIcon.Error);
				}
			}
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void ManageNonLocatedSTLT_FormClosing(object sender, FormClosingEventArgs e)
		{
			if(this.streetLights.Where(a => a.EntityState != EntityMode.Review).Count() > 0)
			{
				if(MessageBox.Show(PendingChangesMsg, ManageNonLocatedTxt, MessageBoxButtons.YesNo) == DialogResult.Yes)
				{
					e.Cancel = true;
					return;
				}
				else
				{
					this.streetLightAcct.MiscStructFid = default(int);
					_gtTransactionManager.Rollback();
				}
			}
			CleanUp();
		}

		#endregion


		#region Button Events

		/// <summary>
		/// 
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void btnEditStreetLight_Click(object sender, EventArgs e)
		{
			_gtKeyEditObj = null;
			if(dtGridViewNonLocated.SelectedRows.Count > 0)
			{
				try
				{
					var obj = (StreetLightNonLocated)dtGridViewNonLocated.SelectedRows[0].DataBoundItem;
					if(obj != null)
					{
						btnEditStreetLight.Enabled = false;
						btnStructure.Enabled = false;
						btnSave.Enabled = false;
						if(obj.EntityState != EntityMode.Add)
						{
							_gtTransactionManager.Begin("Edit Street Light " + obj.G3eFid);
							_gtKeyEditObj = _gtApp.DataContext.OpenFeature(obj.G3efno, obj.G3eFid);
							_gtFeatureExplorerSrvc.ExploreFeature(_gtKeyEditObj, "Edit");
							_gtFeatureExplorerSrvc.Visible = true;
						}
					}
				}
				catch(Exception ex)
				{

					MessageBox.Show("Error while editing feature " + ex.Message);
					_gtTransactionManager.Rollback();
				}
			}
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void btnStructure_Click(object sender, EventArgs e)
		{
			if(streetLightAcct.MiscStructFid == default(int))
			{
				if(string.IsNullOrEmpty(streetLightAcct.BOUNDARY_CLASS))
				{
					_gtPlacementSrvc = GTClassFactory.Create<IGTFeaturePlacementService>(_gtCustomCommandHelper);
					_gtPlacementSrvc.Finished += gtPlacementSrvc_Finished;
					PlaceMiscStructure(false);
				}
				else
				{
					PlaceMiscStructure(true);
					btnStructure.Text = "Locate Structue";
				}
			}
			else
			{
				//Locate Miscellaneous Struture
				CommonUtil.FitSelectedFeature(CommonUtil.MiscStructG3eFno, streetLightAcct.MiscStructFid);
			}
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void btnSave_Click(object sender, EventArgs e)
		{
			stltNonLocatedCtx.MiscStructG3eFid = streetLightAcct.MiscStructFid;
			if(this.streetLights.Where<StreetLightNonLocated>(s => s.EntityState != EntityMode.Review).Count() > 0)
			{
				_gtTransactionManager.Begin("Saving Street Light(s)");
				try
				{
					// ALM 2044 - Set the CU for the current street light.
					// The same customer-owned CU will apply to all non-located lights for the active account.
					// If there's not a customer-owned CU for this account, then allow the user to exit the save operation.
					string CU = CommonUtil.CustomerOwnedSteetLightCU(this.streetLightAcct.LAMP_TYPE, this.streetLightAcct.Wattage, this.streetLightAcct.LUMINARE_STYLE);

					if(string.IsNullOrEmpty(CU))
					{
						string msg = string.Format("A unique customer-owned CU was not found where Lamp Type = {0} and Wattage = {1} and Luminaire Style = {2}.{3}{3}Continue?", this.streetLightAcct.LAMP_TYPE, this.streetLightAcct.Wattage, this.streetLightAcct.LUMINARE_STYLE, Environment.NewLine);

						if(DialogResult.No == MessageBox.Show(msg, "G/Technology", MessageBoxButtons.YesNo, MessageBoxIcon.Warning))
						{
							return;
						}
					}
					//Save the Street Light Boundary
					_gtApp.BeginWaitCursor();
					foreach(StreetLightNonLocated streetLight in this.streetLights.Where<StreetLightNonLocated>(s => s.EntityState != EntityMode.Review))
					{
						if(streetLight.EntityState == EntityMode.Add)
						{
							streetLight.LAMP_TYPE = this.streetLightAcct.LAMP_TYPE;
							streetLight.LUMINARE_STYLE = this.streetLightAcct.LUMINARE_STYLE;
							streetLight.Wattage = this.streetLightAcct.Wattage;
							streetLight.ESI_LOCATION = this.streetLightAcct.ESI_LOCATION;
							streetLight.RATE_CODE = this.streetLightAcct.RATE_CODE;
							streetLight.RATE_SCHEDULE = this.streetLightAcct.RATE_SCHEDULE;
							streetLight.CU = this.CU;
						}
						stltNonLocatedCtx.SaveStreetLight(streetLight);
					}
					_gtTransactionManager.Commit(true);
					this.streetLights = stltNonLocatedCtx.GetStreetLightByAccountID(this.streetLightAcct.ESI_LOCATION);
					dtGridViewNonLocated.DataSource = this.streetLights;
                    _gtApp.RefreshWindows();
                    _gtApp.EndWaitCursor();
					MessageBox.Show("Pending changes committed successfully.", ManageNonLocatedTxt, MessageBoxButtons.OK, MessageBoxIcon.Information);
				}
				catch(Exception ex)
				{
					if(_gtTransactionManager.TransactionInProgress)
					{
						_gtTransactionManager.Rollback();
					}
					_gtApp.EndWaitCursor();
					MessageBox.Show("Error while Saving Street Light(s): " + ex.Message, ManageNonLocatedTxt, MessageBoxButtons.OK, MessageBoxIcon.Error);
				}
			}
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void btnExit_Click(object sender, EventArgs e)
		{
			this.Close();
		}

		#endregion
		#region GTService Events

		/// <summary>
		/// 
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void gtFeatureExplorerSrvc_CancelClick(object sender, EventArgs e)
		{
			_gtFeatureExplorerSrvc.Clear();
			_gtFeatureExplorerSrvc.Visible = false;
			EnableButtonCtrls(true);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void gtFeatureExplorerSrvc_SaveAndContinueClick(object sender, EventArgs e)
		{
			//  _gtFeatureExplorerSrvc.Clear();
			_gtTransactionManager.Commit(true);
			_gtFeatureExplorerSrvc.Visible = false;
			EnableButtonCtrls(true);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void gtFeatureExplorerSrvc_SaveClick(object sender, EventArgs e)
		{
			_gtTransactionManager.Commit(true);
			_gtFeatureExplorerSrvc.Visible = false;
			EnableButtonCtrls(true);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void CustomCommandHelper_Click(object sender, GTMouseEventArgs e)
		{
			if(isPlacementEnabled)
			{
				var btnclk = e.Button;
				IGTPointGeometry gtPointGeom = GTClassFactory.Create<IGTPointGeometry>();
				gtPointGeom.Origin = e.WorldPoint;
				_gtKeyObj.Components.GetComponent(CommonUtil.MiscStructGeoCno).Geometry = gtPointGeom;
			}
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void gtPlacementSrvc_Finished(object sender, GTFinishedEventArgs e)
		{
			isPlacementEnabled = false;
			try
			{
				stltNonLocatedCtx.UpdateStreetLightAcct(e.ComponentsPlaced[0].FID, streetLightAcct.ESI_LOCATION);
				_gtTransactionManager.Commit();
				this.streetLightAcct.MiscStructFid = e.ComponentsPlaced[0].FID;
				btnStructure.Text = "Locate Structue";
				dtGridViewNonLocated.AllowUserToAddRows = true;
			}
			catch(Exception ex)
			{
				MessageBox.Show("Error while placing Misc Structure  " + ex.Message, ManageNonLocatedTxt, MessageBoxButtons.OK, MessageBoxIcon.Error);
				if(_gtTransactionManager.TransactionInProgress)
				{
					_gtTransactionManager.Rollback();
				}
				_gtPlacementSrvc.CancelPlacement();
			}
			_gtPlacementSrvc.Finished -= gtPlacementSrvc_Finished;
			_gtPlacementSrvc.Dispose();
		}
		#endregion

		#region Private Methods

		/// <summary>
		/// 
		/// </summary>
		/// <param name="isSilent"></param>
		private void PlaceMiscStructure(bool isSilent)
		{
			try
			{
				_gtTransactionManager.Begin("Placing Miscellanous Structure ");
				_gtKeyObj = stltNonLocatedCtx.GetNewMiscStructure();
				if(!isSilent)
				{
					_gtFeatureExplorerSrvc.Clear();
					_gtApp.EndWaitCursor();
					isPlacementEnabled = true;
					_gtPlacementSrvc.StartComponent(_gtKeyObj, CommonUtil.MiscStructGeoCno);
				}
				else
				{
					_gtApp.BeginWaitCursor();
					stltNonLocatedCtx.PlaceMiscStrucAtBndryCenter(_gtKeyObj, streetLightAcct.BndryFid, streetLightAcct.BndryFno);
					this.streetLightAcct.MiscStructFid = _gtKeyObj.FID;
					stltNonLocatedCtx.UpdateStreetLightAcct(_gtKeyObj.FID, streetLightAcct.ESI_LOCATION);
					_gtTransactionManager.Commit();
					_gtApp.EndWaitCursor();
				}

			}
			catch(Exception ex)
			{
				MessageBox.Show("Error while placing Misc Structure  " + ex.Message, ManageNonLocatedTxt, MessageBoxButtons.OK, MessageBoxIcon.Error);
				if(_gtTransactionManager.TransactionInProgress)
				{
					_gtTransactionManager.Rollback();
				}
				_gtPlacementSrvc.CancelPlacement();
				_gtApp.EndWaitCursor();
			}

		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="flag"></param>
		private void SubscribEvents(bool flag)
		{
			if(flag)
			{
				if(isJobActive)
				{
					_gtCustomCommandHelper.Click += CustomCommandHelper_Click;
					_gtFeatureExplorerSrvc.SaveClick += gtFeatureExplorerSrvc_SaveClick;
					_gtFeatureExplorerSrvc.SaveAndContinueClick += gtFeatureExplorerSrvc_SaveAndContinueClick;
					_gtFeatureExplorerSrvc.CancelClick += gtFeatureExplorerSrvc_CancelClick;
					dtGridViewNonLocated.SelectionChanged += dtGridViewNonLocated_SelectionChanged;
					dtGridViewNonLocated.CellValueChanged += dtGridViewNonLocated_CellValueChanged;
					dtGridViewNonLocated.UserDeletingRow += dtGridViewNonLocated_UserDeletingRow;
				}
				dtGridViewNonLocated.RowsAdded += dtGridViewNonLocated_RowsAdded;

			}
			else
			{
				if(isJobActive)
				{

					_gtCustomCommandHelper.Click -= CustomCommandHelper_Click;
					_gtFeatureExplorerSrvc.SaveClick -= gtFeatureExplorerSrvc_SaveClick;
					_gtFeatureExplorerSrvc.SaveAndContinueClick -= gtFeatureExplorerSrvc_SaveAndContinueClick;
					_gtFeatureExplorerSrvc.CancelClick -= gtFeatureExplorerSrvc_CancelClick;
					dtGridViewNonLocated.CellValueChanged -= dtGridViewNonLocated_CellValueChanged;
					dtGridViewNonLocated.UserDeletingRow -= dtGridViewNonLocated_UserDeletingRow;

				}
				dtGridViewNonLocated.RowsAdded -= dtGridViewNonLocated_RowsAdded;
			}
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="flag"></param>
		private void EnableButtonCtrls(bool flag)
		{

			btnEditStreetLight.Enabled = flag;
			btnStructure.Enabled = flag;
			btnSave.Enabled = flag;
		}

      
        /// <summary>
        /// 
        /// </summary>
        private void CleanUp()
		{
			SubscribEvents(false);
			if(_gtFeatureExplorerSrvc != null)
			{
				_gtFeatureExplorerSrvc.Clear();
				_gtFeatureExplorerSrvc.Dispose();
				_gtFeatureExplorerSrvc = null;
			}
			if(_gtPlacementSrvc != null)
			{
				_gtPlacementSrvc.Dispose();
				_gtPlacementSrvc = null;
			}
			streetLights = null;
			if(stltNonLocatedCtx != null)
			{
				stltNonLocatedCtx.Dispose();
			}
			stltNonLocatedCtx = null;
			streetLightAcct = null;
			_gtKeyObj = null;
			_gtKeyEditObj = null;
		}

		#endregion


	}
}
