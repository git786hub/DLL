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
    public partial class dlgMoreInfoForm : Form
    {
        public dlgMoreInfoForm()
        {
            InitializeComponent();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        public string CUID { get; set; }
        public string CUCategory { get; set; }
        //public string Status { get; set; }
        public string EffectiveDate { get; set; }
        public string ExpirationDate { get; set; }
        public string PropertyUnitCode { get; set; }
        public string RetirementType { get; set; }
        public string CapitalPrimeAccount { get; set; }
        public string CapitalSubAccount { get; set; }
        public string CUDescription { get; set; }
        public string CategoryDescription { get; set; }
        public DataTable TSNData { get; set; }

        public string MyProperty { get; set; }

        private void dlgMoreInfoForm_Load(object sender, EventArgs e)
        {
            PopulateData();
        }

        private void PopulateData()
        {
            lblDynamicCapitalPrimeAccnt.Text = CapitalPrimeAccount;
            //lblDynamicCapitalSubAccnt.Text = CapitalSubAccount;
            lblDynamicCategoryDesc.Text = CategoryDescription;
            lblDynamicCuCategory.Text = CUCategory;
            lblDynamicCUDesc.Text = CUDescription;
            lblDynamicCuID.Text = CUID;
            lblDynamicEffectiveDate.Text = EffectiveDate;
            lblDynamicExpirationDate.Text = ExpirationDate;
            lblDynamicPropertyCUCode.Text = PropertyUnitCode;
            lblDynamicRetirementType.Text = RetirementType;
           // lblDynamicStatus.Text = Status;

            if (TSNData != null)
            {
                dataGridView1.DataSource = ChangeDataColumn(TSNData, new List<string> { "TSN", "Material Code", "TSN Quantity", "Unit Amount", "Unit of Measure", "Material Description" });
                dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            }
        }
        private DataTable ChangeDataColumn( DataTable p_Data, List<string> OrderedColumnNames)
        {
            for (int i = 0; i < OrderedColumnNames.Count; i++)
            {
                p_Data.Columns[i].ColumnName = OrderedColumnNames[i].ToString();
            }
            return p_Data;
        }

      
    }
}
