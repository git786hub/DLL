using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;

namespace GTechnology.Oncor.CustomAPI
{
    public partial class TransactionTestHarness : Form
    {
        public TransactionTestHarness()
        {
            InitializeComponent();
        }

        private void GetTransactionTest_btn_Click(object sender, EventArgs e)
        {
            string xmlDoc = "";
            string xmlDocLocation = DocFileLocationBox.Text;
            XmlDocument xmlTest = new XmlDocument();
            xmlTest.Load(xmlDocLocation);
            xmlDoc = xmlTest.SelectSingleNode("Transaction").Value;
            dataProvider.GetTransaction(xmlTest.OuterXml);
        }

        private void AddPremise_btn_Click(object sender, EventArgs e)
        {
            dataProvider.AddPremise();
        }

        private void RemovePremise_btn_Click(object sender, EventArgs e)
        {
            dataProvider.RemovePremise();
        }

        private void UpdatePremise_btn_Click(object sender, EventArgs e)
        {
            dataProvider.UpdatePremise();
        }

        private void AddSB_btn_Click(object sender, EventArgs e)
        {
            dataProvider.AddServicePoint();
        }

        private void RemoveSP_btn_Click(object sender, EventArgs e)
        {
            dataProvider.RemoveServicePoint();
        }

        private void GetFID_btn_Click(object sender, EventArgs e)
        {
            dataProvider.GetStructureFID();
        }

       

        private void GeocodeTolerance_btn_Click(object sender, EventArgs e)
        {
            dataProvider.CheckMeterGeocodeLocation();
        }

        private void EstablishRelationship_btn_Click(object sender, EventArgs e)
        {
            dataProvider.EstablishConnectivity();
        }

        private void UpdateServicePoint_btn_Click(object sender, EventArgs e)
        {
            dataProvider.UpdateServicePoint();
        }

        private void AddServiceLine_btn_Click(object sender, EventArgs e)
        {
            dataProvider.AddServiceLine();
        }

        private void UpdateServiceLine_btn_Click(object sender, EventArgs e)
        {
            dataProvider.UpdateServiceLine();
        }

        private void RemoveServiceLine_btn_Click(object sender, EventArgs e)
        {
            dataProvider.RemoveServiceLine();
        }

        private void GetOwningStructureFID_btn_Click(object sender, EventArgs e)
        {
            dataProvider.GetOwningStructureFID(7011196);
        }

        private void UpdateStructureID_btn_Click(object sender, EventArgs e)
        {
            dataProvider.UpdateStructureID();
        }

        private void GetServiceLine_btn_Click(object sender, EventArgs e)
        {
            dataProvider.GetServiceLine();
        }

        private void ReplaceServiceLine_btn_Click(object sender, EventArgs e)
        {
            dataProvider.ReplaceServiceLine();
        }

        private void LocateSrvcPtByESILocation_btn_Click(object sender, EventArgs e)
        {
            dataProvider.LocateSrvcPtByESILocation("0");
        }

        private void LocateSrvcPtByAddress_btn_Click(object sender, EventArgs e)
        {
            dataProvider.LocateSrvcPtByAddress();
        }

        private void GetConnectingFacility_btn_Click(object sender, EventArgs e)
        {
            dataProvider.GetConnectingFacility();
        }
    }
}
