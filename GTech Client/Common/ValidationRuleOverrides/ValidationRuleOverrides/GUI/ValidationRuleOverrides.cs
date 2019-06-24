using GTechnology.Oncor.CustomAPI.DataAccess;
using GTechnology.Oncor.CustomAPI.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows.Forms;

namespace GTechnology.Oncor.CustomAPI.GUI
{
  public partial class ValidationRuleOverrides : Form
  {
    private BindingList<WRValidationOverride> wrValidationComments = null;
    private WRValidationOverride selectedValidation = null;

    private bool inInit = false;

    public List<WRValidationOverride> WRValidationComments { get => this.wrValidationComments.ToList(); }

    public ValidationRuleOverrides()
    {
      InitializeComponent();
      btnContinue.Enabled = false;
    }
    public ValidationRuleOverrides(List<WRValidationOverride> wrValidationComments) : this()
    {
      this.wrValidationComments = new BindingList<WRValidationOverride>(wrValidationComments);

      dtGridViewValidationErr.Columns["Error_MSG"].DataPropertyName = "Error_MSG";
      dtGridViewValidationErr.Columns["FeatureClass"].DataPropertyName = "FeatureClass";
      dtGridViewValidationErr.Columns["G3E_Fid"].DataPropertyName = "G3E_Fid";
      dtGridViewValidationErr.Columns["Override_Comments"].DataPropertyName = "Override_Comments";

    }

    #region Events
    private void ValidationRuleOverrides_Shown(object sender, EventArgs e)
    {
      this.inInit = true;
      this.dtGridViewValidationErr.AutoGenerateColumns = false;
      dtGridViewValidationErr.SelectionMode = DataGridViewSelectionMode.RowHeaderSelect;
      this.dtGridViewValidationErr.DataSource = wrValidationComments;
      this.inInit = false;
    }

    private void dtGridViewValidationErr_SelectionChanged(object sender, EventArgs e)
    {
      // During the _Shown method, we don't want this highlighting to occur.
      if(!this.inInit)
      {
        if(dtGridViewValidationErr.SelectedRows.Count > 0)
        {
          selectedValidation = (WRValidationOverride)dtGridViewValidationErr.SelectedRows[0].DataBoundItem;
          if(selectedValidation != null)
          {
            txtError.Text = selectedValidation.Error_Msg;
            txtJustification.Text = selectedValidation.Override_Comments;
            CommonUtil.HighlightFeature(Convert.ToInt16(selectedValidation.G3e_Fno), Convert.ToInt32(selectedValidation.G3e_Fid));
          }
        }
      }
    }

    private void txtJustification_TextChanged(object sender, EventArgs e)
    {
      if(dtGridViewValidationErr.SelectedRows.Count > 0)
      {
        ((WRValidationOverride)dtGridViewValidationErr.SelectedRows[0].DataBoundItem).Override_Comments = txtJustification.Text;
      }
      if(this.wrValidationComments.Count(a => string.IsNullOrEmpty(a.Override_Comments) == true) > 0)
      {
        btnContinue.Enabled = false;
      }
      else
      {
        btnContinue.Enabled = true;
      }

    }
    #endregion
  }
}
