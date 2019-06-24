using System;
using Intergraph.GTechnology.API;
using Intergraph.GTechnology.Interfaces;
using System.Windows.Forms;
using System.Collections.Generic;
using ADODB;

namespace GTechnology.Oncor.CustomAPI
{
    public class ccSagClearance : IGTCustomCommandModeless
    {
        private IGTTransactionManager m_TransactionManager;
        private IGTApplication m_Application = GTClassFactory.Create<IGTApplication>();
        private IGTCustomCommandHelper m_CustomCommandHelper;
        private EmbeddedDT m_EmbeddedDT = new EmbeddedDT();

        public bool CanTerminate
        {
            get
            {
                return true;
            }
        }

        public IGTTransactionManager TransactionManager
        {
            set
            {
                try
                {
                    m_TransactionManager = value;
                    m_EmbeddedDT.TransactionManager = m_TransactionManager;
                }
                catch (Exception ex)
                {
                    MessageBox.Show(m_Application.ApplicationWindow, ex.Message, ConstantsDT.APPLICATION_NAME, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }
            }
        }

        /// <summary>
        /// The entry point for the custom command.
        /// </summary>
        /// <param name="CustomCommandHelper">Provides notification to the system that the command has finished</param>
        public void Activate(IGTCustomCommandHelper CustomCommandHelper)
        {
            try
            {
                m_CustomCommandHelper = CustomCommandHelper;

                // Get the job information. Used for validating command enabling.
                if (!CommonDT.GetJobInformation())
                {
                    m_CustomCommandHelper.Complete();
                    return;
                }

                short fno = 0;
                int fid = 0;

                m_EmbeddedDT.cmdSaveReport.Enabled = false;
                
                IGTDDCKeyObjects oGTDCKeys = GTClassFactory.Create<IGTDDCKeyObjects>();
                oGTDCKeys = m_Application.SelectedObjects.GetObjects();
                if (oGTDCKeys.Count > 0)
                {
                    // Check if selected feature is a Conductor.
                    if (oGTDCKeys[0].FNO == ConstantsDT.FNO_OH_COND || oGTDCKeys[0].FNO == ConstantsDT.FNO_UG_COND ||
                        oGTDCKeys[0].FNO == ConstantsDT.FNO_OH_SECCOND || oGTDCKeys[0].FNO == ConstantsDT.FNO_UG_SECCOND)
                    {
                        fid = oGTDCKeys[0].FID;
                        fno = oGTDCKeys[0].FNO;
                        m_EmbeddedDT.cmdSaveReport.Enabled = true;
                    }
                    else
                    {
                        MessageBox.Show(m_Application.ApplicationWindow, ConstantsDT.ERROR_SAG_INVALID_FEATURE_SELECTED, ConstantsDT.APPLICATION_NAME, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        m_CustomCommandHelper.Complete();
                        return;
                    }
                }

                // Get the report data
                if (!GetReportData(fno, fid))
                {
                    CustomCommandHelper.Complete();
                    return;
                }
                
                // Get the form Close events so we can call the Complete method for the command
                // when the form closes.
                m_EmbeddedDT.cmdClose.Click += cmdClose_Click;
                m_EmbeddedDT.FormClosing += cmdClose_Click;

                m_EmbeddedDT.Application = m_Application;
                m_EmbeddedDT.Text = ConstantsDT.COMMAND_NAME_SAG_CLEARANCE;
                m_EmbeddedDT.CommandName = ConstantsDT.COMMAND_NAME_SAG_CLEARANCE;
                m_EmbeddedDT.SelectedFID = fid;
                m_EmbeddedDT.SelectedFNO = fno;

                m_EmbeddedDT.InitializeFormSize();

                m_EmbeddedDT.StartPosition = FormStartPosition.CenterScreen;
                m_EmbeddedDT.Show(m_Application.ApplicationWindow);
            }
            catch (Exception ex)
            {
                MessageBox.Show(m_Application.ApplicationWindow, ex.Message, ConstantsDT.APPLICATION_NAME, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                CustomCommandHelper.Complete();
            }
        }

        public void Pause()
        {
            return;
        }

        public void Resume()
        {
            return;
        }

        /// <summary>
        /// Command has terminated. Release the objects.
        /// </summary>
        public void Terminate()
        {
            try
            {
                m_Application = null;
                m_TransactionManager = null;
                m_CustomCommandHelper = null;
                m_EmbeddedDT.Application = null;
                m_EmbeddedDT.TransactionManager = null;
                m_EmbeddedDT = null;
            }
            catch (Exception ex)
            {
                MessageBox.Show(m_Application.ApplicationWindow, ex.Message, ConstantsDT.APPLICATION_NAME, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        /// <summary>
        /// Form closing event. Call the Complete method to end the command.
        /// </summary>
        private void cmdClose_Click(object sender, EventArgs e)
        {                    
            m_EmbeddedDT.Cleanup();
            m_EmbeddedDT.FormClosing -= cmdClose_Click;
            m_EmbeddedDT.cmdClose.Click -= cmdClose_Click;
            m_EmbeddedDT.Close();
            m_CustomCommandHelper.Complete();
        }

        /// <summary>
        /// Gets the values to pre-populate on the report.
        /// </summary>
        /// <param name="fno">G3E_FNO of the selected conductor</param>
        /// <param name="fid">G3E_FID of the selected conductor</param>
        /// <returns>Boolean indicating status</returns>
        private bool GetReportData(short fno, int fid)
        {
            bool returnValue = false;

            try
            {
                string cu = string.Empty;
                string location = string.Empty;

                if (fid != 0)
                {
                    IGTKeyObject oGTKey = m_Application.DataContext.OpenFeature(fno, fid);
                    IGTComponents oGTComponents = oGTKey.Components;
                    Recordset componentRS = oGTComponents.GetComponent(ConstantsDT.CNO_COMPUNIT).Recordset;

                    if (componentRS.RecordCount > 0)
                    {
                        cu = componentRS.Fields[ConstantsDT.FIELD_COMPUNIT_CU].Value.ToString();
                    }

                    componentRS = oGTComponents.GetComponent(ConstantsDT.CNO_COMMON).Recordset;

                    if (componentRS.RecordCount > 0)
                    {
                        location = componentRS.Fields[ConstantsDT.FIELD_COMMON_LOCATION].Value.ToString();
                    }
                }                

                m_EmbeddedDT.ReportName = ConstantsDT.COMMAND_NAME_SAG_CLEARANCE + " " + m_Application.DataContext.ActiveJob + "-" + fid + ".pdf";

                List<KeyValuePair<string, string>> reportValues = new List<KeyValuePair<string, string>>();

                // Add report values
                reportValues.Add(new KeyValuePair<string, string>(ConstantsDT.REPORT_PARAMETER_SAG_PROJECT, CommonDT.WrDescription));
                reportValues.Add(new KeyValuePair<string, string>(ConstantsDT.REPORT_PARAMETER_SAG_LOCATION, location));
                reportValues.Add(new KeyValuePair<string, string>(ConstantsDT.REPORT_PARAMETER_SAG_DESIGNER, Environment.UserName));
                reportValues.Add(new KeyValuePair<string, string>(ConstantsDT.REPORT_PARAMETER_SAG_DATE, DateTime.Now.ToShortDateString()));
                reportValues.Add(new KeyValuePair<string, string>(ConstantsDT.REPORT_PARAMETER_SAG_CONDUCTOR, cu));

                m_EmbeddedDT.ReportValues = reportValues;

                returnValue = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(m_Application.ApplicationWindow, ConstantsDT.ERROR_RETRIEVING_REPORT_DATA + ": " + ex.Message, ConstantsDT.APPLICATION_NAME, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return false;
            }

            return returnValue;
        }
    }
}
