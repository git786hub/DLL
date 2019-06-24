using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ADODB;
using Intergraph.GTechnology.API;
using Intergraph.GTechnology.Interfaces;
using System.IO;
using System.Windows.Forms;

namespace ccImportAttachCSV
{
    public class ccImportAttachCSV:IGTCustomCommandModeless
    {
        public void Activate(IGTCustomCommandHelper CustomCommandHelper)
        {
            // Set the global variables
            csGlobals.gApp = GTClassFactory.Create<IGTApplication>();
            csGlobals.gDataCont = csGlobals.gApp.DataContext;
            csGlobals.ofCsv = new OpenFileDialog();
            csGlobals.svCsv = new SaveFileDialog();
            csGlobals.ofCsv.Filter = "CSV Files (*.CSV)|*.CSV|All files (*.*)|*.*)";
            csGlobals.ofCsv.FilterIndex = 1;
            csGlobals.ofCsv.Title = "Open Attachment CSV File";
            csGlobals.frmTmpAttachments = new frmAttachCVS();
            csGlobals.gCcHelper = CustomCommandHelper;


            try
            {
                // load the Company data into a recordset.
                csGlobals.gLoadCompanyPlRs();
                // display the open file dialog
                if (csGlobals.ofCsv.ShowDialog() == DialogResult.OK)
                {

                    csGlobals.csvFile = csGlobals.ofCsv.FileName;
                    csGlobals.CSVStream = new StreamReader(csGlobals.csvFile);

                    //if (MessageBox.Show(csGlobals.ofCsv.FileName) == DialogResult.OK)
                    //{
                    // Load the Attachments CSV dialog.
                        csGlobals.frmTmpAttachments.ShowDialog();
                        //do nothing
                    //}
                }
                else
                {
                    csGlobals.gCcHelper.Complete();
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(csGlobals.gApp.ApplicationWindow, "ccImportAttachCSV.Activate: " + e.Message, "Import Attachments - Error",
                               MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public bool CanTerminate
        {
            get { return csGlobals.gCanTerm; }
        }

        public void Pause()
        {
            throw new NotImplementedException();
        }

        public void Resume()
        {
            throw new NotImplementedException();
        }

        public void Terminate()
        {

            //clean up the objects
            
            if (!(csGlobals.CSVStream == null))
            {
                csGlobals.CSVStream = null;
            }

            if (!(csGlobals.CSVOutStream == null))
            {
                csGlobals.CSVOutStream = null;
            }

            if (!(csGlobals.ofCsv == null))
            {
                csGlobals.ofCsv = null;
            }

            if (!(csGlobals.svCsv == null))
            {
                csGlobals.svCsv = null;
            }

            if (!(csGlobals.frmTmpAttachments == null))
            {
                csGlobals.frmTmpAttachments = null;
            }

            if (!(csGlobals.csvFile == null))
            {
                csGlobals.csvFile = null;
            }

            if (!(csGlobals.gDataCont == null))
            {
                csGlobals.gDataCont = null;
            }

            if (!(csGlobals.gApp == null))
            {
                csGlobals.gApp = null;
            }

            if (!(csGlobals.gTransMgr == null))
            {
                csGlobals.gTransMgr = null;
            }
            if (!(csGlobals.gColDataRs == null))
            {
                csGlobals.gColDataRs = null;
            }

            if (!(csGlobals.gHeaderRowOfCSV == null))
            {
                csGlobals.gHeaderRowOfCSV = null;
            }
            GC.Collect(); // collect the rest of the objects
        }

        public IGTTransactionManager TransactionManager
        {
            set { csGlobals.gTransMgr = value; }
        }
    }
}
