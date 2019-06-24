using System;
using Intergraph.GTechnology.API;
using Intergraph.GTechnology.Interfaces;
using System.Windows.Forms;
using System.Collections.Generic;
using ADODB;

namespace GTechnology.Oncor.CustomAPI
{
    public class ccGuying : IGTCustomCommandModeless
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

                IGTDDCKeyObjects gtDDCKeys = GTClassFactory.Create<IGTDDCKeyObjects>();
                gtDDCKeys = m_Application.SelectedObjects.GetObjects();
                List<int> FIDs = new List<int>();

                if (gtDDCKeys.Count > 0)
                {
                    foreach (IGTDDCKeyObject gtDDCKey in gtDDCKeys)
                    {
                        // Check if selected feature is a Pole.
                        if (gtDDCKey.FNO == ConstantsDT.FNO_POLE)
                        {
                            if (!FIDs.Contains(gtDDCKey.FID))
                            {
                                m_EmbeddedDT.SelectedFID = gtDDCKey.FID;
                                FIDs.Add(gtDDCKey.FID);
                            }                            
                        }
                        else
                        {
                            MessageBox.Show(m_Application.ApplicationWindow, ConstantsDT.ERROR_GUY_INVALID_FEATURE_SELECTED, ConstantsDT.APPLICATION_NAME, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                            CustomCommandHelper.Complete();
                            return;
                        }
                    }                    
                }

                FIDs.Clear();
                FIDs = null;

                m_EmbeddedDT.Application = m_Application;
                m_EmbeddedDT.Text = ConstantsDT.COMMAND_NAME_GUYING;
                m_EmbeddedDT.CommandName = ConstantsDT.COMMAND_NAME_GUYING;
                m_EmbeddedDT.SelectedFNO = ConstantsDT.FNO_POLE;

                // Get the Guying Scenario
                string guyingScenarioNumber = string.Empty;
                bool newGuyingScenario = false;
                string hyperlinkFilePath = string.Empty;
                if (!GetGuyingScenario(ref guyingScenarioNumber, ref newGuyingScenario, ref hyperlinkFilePath))
                {
                    CustomCommandHelper.Complete();
                    return;
                }

                if (newGuyingScenario)
                {
                    // Pass notification to Save command to increment G3E_JOB.GUY_SCENARIO_COUNT on successful save.
                    m_EmbeddedDT.NewGuyScenario = true;
                    m_EmbeddedDT.GuyScenarioCount = Convert.ToInt16(guyingScenarioNumber);
                }
                else
                {
                    // Create Hyperlink component
                    m_EmbeddedDT.WrNumber = m_Application.DataContext.ActiveJob;
                    m_EmbeddedDT.GuyScenarioCount = Convert.ToInt16(guyingScenarioNumber);

                    m_TransactionManager.Begin("New Hyperlink");
                    if (m_EmbeddedDT.AddHyperlinkComponent(hyperlinkFilePath))
                    {
                        m_TransactionManager.Commit();
                    }
                    else
                    {
                        m_TransactionManager.Rollback();
                    }

                    CustomCommandHelper.Complete();
                    return;
                }

                // Get the report data
                if (!GetReportData(guyingScenarioNumber))
                {
                    CustomCommandHelper.Complete();
                    return;
                }

                // Get the form Close events so we can call the Complete method for the command
                // when the form closes.
                m_EmbeddedDT.cmdClose.Click += cmdClose_Click;
                m_EmbeddedDT.FormClosing += cmdClose_Click;               

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

        public void Terminate()
        {
            try
            {
                m_Application = null;
                m_TransactionManager = null;
                m_CustomCommandHelper = null;
                m_EmbeddedDT.Application = null;
                m_EmbeddedDT.TransactionManager = null;
                if (m_EmbeddedDT != null)
                {
                    m_EmbeddedDT.Close();
                    m_EmbeddedDT = null;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(m_Application.ApplicationWindow, ex.Message, ConstantsDT.APPLICATION_NAME, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        // The form is closing. Release the objects and call the Complete method to end the command.
        private void cmdClose_Click(object sender, EventArgs e)
        {
            m_EmbeddedDT.Cleanup();
            m_EmbeddedDT.FormClosing -= cmdClose_Click;
            m_EmbeddedDT.cmdClose.Click -= cmdClose_Click;

            if (m_CustomCommandHelper != null)
            {
                m_EmbeddedDT.Close();
                m_CustomCommandHelper.Complete();
            }     
        }

        /// <summary>
        /// Gets the values to pre-populate on the report.
        /// </summary>
        /// <param name="guyingScenarioNumber">The Guying Scenario Number to use for the report</param>
        /// <returns>Boolean indicating status</returns>
        private bool GetReportData(string guyingScenarioNumber)
        {
            bool returnValue = false;

            try
            {
                string wrNumber = m_Application.DataContext.ActiveJob;
                CommonDT commonDT = new CommonDT();

                m_EmbeddedDT.WrNumber = wrNumber;
                m_EmbeddedDT.ReportName = ConstantsDT.COMMAND_NAME_GUYING + " Scenario " + wrNumber + "-" + guyingScenarioNumber + ".pdf";

                commonDT = null;

                List<KeyValuePair<string, string>> reportValues = new List<KeyValuePair<string, string>>();

                // Add report values
                reportValues.Add(new KeyValuePair<string, string>(ConstantsDT.REPORT_PARAMETER_GUY_WR, wrNumber));
                reportValues.Add(new KeyValuePair<string, string>(ConstantsDT.REPORT_PARAMETER_GUY_SCENARIO, guyingScenarioNumber));
                reportValues.Add(new KeyValuePair<string, string>(ConstantsDT.REPORT_PARAMETER_GUY_DATE, DateTime.Now.ToShortDateString()));

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

        /// <summary>
        /// Get the Guying Scenario number.
        /// </summary>
        /// <param name="guyingScenario">The Guying Scenario Number</param>
        /// <param name="newGuyingScenario">Indicates if the guying scenario is new</param>
        /// <param name="filePath">Path to an existing hyperlink</param>
        /// <returns>Boolean indicating status</returns>
        private bool GetGuyingScenario (ref string guyingScenario, ref bool newGuyingScenario, ref string filePath)
        {
            bool returnValue = false;

            try
            {
                short guyingScenarioCount = 0;
                string sql = "select GUY_SCENARIO_COUNT from G3E_JOB where g3e_identifier = ?";
                // Query the G3E_JOB.GUY_SCENARIO_COUNT for the active job and set guyingScenarioCount.
                Recordset guyScenarioRS = m_Application.DataContext.OpenRecordset(sql, CursorTypeEnum.adOpenDynamic, LockTypeEnum.adLockOptimistic, (int)CommandTypeEnum.adCmdText, m_Application.DataContext.ActiveJob);
                if (guyScenarioRS.RecordCount > 0 && guyScenarioRS.Fields["GUY_SCENARIO_COUNT"].Value !=  DBNull.Value)
                {
                    guyingScenarioCount = Convert.ToInt16(guyScenarioRS.Fields["GUY_SCENARIO_COUNT"].Value);
                }

                // If G3E_JOB.GUY_SCENARIO_COUNT = 0 then set Guying Scenario number = 1
                // Else check for existing Guying Scenarios by querying the HYPERLINK_N table for the active WR
                //  Allow user to select an existing Guying Scenario or create a new one.
                if (guyingScenarioCount == 0)
                {
                    guyingScenario = "01";
                    newGuyingScenario = true;
                }
                else
                {
                    // Query the HYPERLINK_N table to get the guying scenarios for the active WR
                    sql = "select distinct substr(description_t, Instr(description_t, '-') + 1) as GUYING_SCENARIO, hyperlink_t from hyperlink_n "+
                                 "where type_c = 'Guying Scenario' " +
                                 "and substr(description_t, 1, (instr(description_t, '-') - 1)) = ? order by GUYING_SCENARIO";

                    guyScenarioRS = m_Application.DataContext.OpenRecordset(sql, CursorTypeEnum.adOpenDynamic, LockTypeEnum.adLockOptimistic, (int)CommandTypeEnum.adCmdText, m_Application.DataContext.ActiveJob);

                    if (guyScenarioRS.RecordCount > 0)
                    {
                        // Display dialog to allow user to select an existing Guying Scenario or create a new one.
                        frmGuyingScenario frmGuyingScenario = new frmGuyingScenario();

                        while (!guyScenarioRS.EOF)
                        {
                            frmGuyingScenario.cboGuyingScenarios.Items.Add(guyScenarioRS.Fields["GUYING_SCENARIO"].Value.ToString());
                            guyScenarioRS.MoveNext();
                        }

                        frmGuyingScenario.cboGuyingScenarios.SelectedIndex = 0;

                        frmGuyingScenario.ShowDialog(m_Application.ApplicationWindow); 
                        
                        // Set Guying Scenario number based on user selection.
                        if (frmGuyingScenario.CreateNewGuyingScenario)
                        {
                            // New guying scenario
                            guyingScenario = (guyingScenarioCount + 1).ToString().PadLeft(2, '0');
                            newGuyingScenario = true;
                        }
                        else
                        {
                            // Get existing guying scenario
                            guyingScenario = frmGuyingScenario.GuyingScenario;
                            guyScenarioRS.MoveFirst();
                            guyScenarioRS.Filter = "GUYING_SCENARIO = " + frmGuyingScenario.GuyingScenario;
                            filePath = guyScenarioRS.Fields["hyperlink_t"].Value.ToString();
                            newGuyingScenario = false;
                        }

                        frmGuyingScenario = null;
                    }
                    else
                    {
                        guyingScenario = (guyingScenarioCount + 1).ToString().PadLeft(2, '0');
                        newGuyingScenario = true;
                    }                   
                }

                returnValue = true;
            }
            catch (Exception ex)
            {
                //MessageBox.Show(m_Application.ApplicationWindow, ConstantsDT.? + ": " + ex.Message, ConstantsDT.APPLICATION_NAME, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return false;
            }

            return returnValue;
        }
    }
}