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
using Intergraph.GTechnology.API;
using Intergraph.GTechnology.Common;

namespace GTechnology.Oncor.CustomAPI
{
    public partial class Form1 : Form
    {
        GISAuto_ServiceLine processing;
        public Form1(IGTTransactionManager transactionManager)
        {
            InitializeComponent();
            processing = new GISAuto_ServiceLine(transactionManager);
        }

        private void GetTransaction_btn_Click(object sender, EventArgs e)
        {
            string xmlDoc = "";
            string xmlDocLocation = FileLocation.Text;
            XmlDocument xmlTest = new XmlDocument();
            xmlTest.Load(xmlDocLocation);
            xmlDoc = xmlTest.SelectSingleNode("Transaction").Value;
            processing.ProcessTransaction(xmlTest.OuterXml);
        }

        private void CorrectStructureID_btn_Click(object sender, EventArgs e)
        {
            processing.StructureIdCorrection();
        }

        
    }
}
