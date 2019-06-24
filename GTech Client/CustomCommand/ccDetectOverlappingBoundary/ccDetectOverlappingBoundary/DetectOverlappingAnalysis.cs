using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GTechnology.Oncor.CustomAPI
{
    public partial class DetectOverlappingAnalysis : Form
    {

        int selectedFeatureType;
        bool isSelfOverlap;

       public int SelectedFeatureType { get => selectedFeatureType; }
       public bool IsSelfOverlap { get => isSelfOverlap; }

        public DetectOverlappingAnalysis()
        {
            InitializeComponent();
            btnOk.Enabled = false;
        }

        public DetectOverlappingAnalysis(List<KeyValuePair<int, string>> featureTypes) : this()
        {
            cmbBoxFeatureType.DataSource = featureTypes;
            cmbBoxFeatureType.DisplayMember = "Value";
            cmbBoxFeatureType.ValueMember = "Key";

            cmbBoxFeatureType.SelectedValueChanged += cmbBoxFeatureType_SelectedValueChanged;
            rdBtnNo.Checked = true;
        }

        private void cmbBoxFeatureType_SelectedValueChanged(object sender, EventArgs e)
        {
            selectedFeatureType = Convert.ToInt32(cmbBoxFeatureType.SelectedValue);
            btnOk.Enabled = true;
        }

        private void rdBtnYes_CheckedChanged(object sender, EventArgs e)
        {
            if(rdBtnYes.Checked) { isSelfOverlap = true; }
            
        }
        private void rdBtnNo_CheckedChanged(object sender, EventArgs e)
        {
            if (rdBtnNo.Checked) { isSelfOverlap = false; }
        }
    }
}
