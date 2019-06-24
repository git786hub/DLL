using System;
using System.Linq;
using Intergraph.GTechnology.API;
using Intergraph.GTechnology.Interfaces;
using System.Windows.Forms;
using ADODB;

namespace GTechnology.Oncor.CustomAPI
{
    public class ccSecondaryCalculator : IGTCustomCommandModeless
    {
        private IGTTransactionManager m_TransactionManager;
        private IGTApplication m_Application = GTClassFactory.Create<IGTApplication>();
        private IGTCustomCommandHelper m_CustomCommandHelper;
        private frmSecondaryCalculator m_frmSecondaryCalculator = new frmSecondaryCalculator();

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
                    m_frmSecondaryCalculator.m_TransactionManager = m_TransactionManager;
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

                // Get the metadata for the command
                if (!m_frmSecondaryCalculator.GetMetadata())
                {
                    m_CustomCommandHelper.Complete();
                    return;
                }

                // Check if selected feature is a Transformer.
                IGTDDCKeyObjects oGTDCKeys = GTClassFactory.Create<IGTDDCKeyObjects>();
                oGTDCKeys = m_Application.SelectedObjects.GetObjects();
                if (oGTDCKeys[0].FNO != ConstantsDT.FNO_OH_XFMR && oGTDCKeys[0].FNO != ConstantsDT.FNO_UG_XFMR)
                {
                    MessageBox.Show(m_Application.ApplicationWindow, ConstantsDT.ERROR_SC_INVALID_FEATURE_SELECTED, ConstantsDT.APPLICATION_NAME, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    m_CustomCommandHelper.Complete();
                    return;
                }

                // If the selected Transformer is a 3 Phase then check for valid voltage.
                short xfmrFNO = oGTDCKeys[0].FNO;
                int xfmrFID = oGTDCKeys[0].FID;
                short xfmrUnitCNO = 0;

                if (xfmrFNO == ConstantsDT.FNO_OH_XFMR)
                {
                    xfmrUnitCNO = ConstantsDT.CNO_OH_XFMR_UNIT;
                }
                else
                {
                    xfmrUnitCNO = ConstantsDT.CNO_UG_XFMR_UNIT;
                }

                IGTKeyObject oGTKey = m_Application.DataContext.OpenFeature(xfmrFNO, xfmrFID);
                IGTComponents oGTComponents = oGTKey.Components;
                Recordset componentRS = oGTComponents.GetComponent(xfmrUnitCNO).Recordset;
                Recordset xfmrConfigChecker;
                if (xfmrFNO == ConstantsDT.FNO_OH_XFMR)
                {
                    xfmrConfigChecker = oGTComponents.GetComponent(ConstantsDT.CNO_OH_XFMR_BANK).Recordset;
                }
                else
                {
                    xfmrConfigChecker = oGTComponents.GetComponent(ConstantsDT.CNO_UG_XFMR_UNIT).Recordset;
                }
                short phaseCount = 0;
                string secondaryVoltage = string.Empty;

                if (componentRS.RecordCount > 0)
                {
                    secondaryVoltage = componentRS.Fields[ConstantsDT.FIELD_XFMRUNIT_VOLTAGE_SEC].Value.ToString();
                    if (!Convert.IsDBNull(componentRS.Fields[ConstantsDT.FIELD_XFMRUNIT_PHASE_QUANTITY].Value))
                    {
                        phaseCount = Convert.ToInt16(componentRS.Fields[ConstantsDT.FIELD_XFMRUNIT_PHASE_QUANTITY].Value);
                    }
                    if ((!Convert.IsDBNull(xfmrConfigChecker.Fields["CONFIG_PRI_C"].Value) && xfmrConfigChecker.Fields["CONFIG_PRI_C"].Value.ToString().Contains("OPEN")))
                    {
                        MessageBox.Show(m_Application.ApplicationWindow, ConstantsDT.ERROR_SC_INVALID_CONFIGURATION, ConstantsDT.APPLICATION_NAME, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        m_CustomCommandHelper.Complete();
                        return;
                    }
                }

                if (phaseCount == 3 && !m_frmSecondaryCalculator.m_ValidThreePhaseVoltages.Contains(secondaryVoltage))
                {
                    MessageBox.Show(m_Application.ApplicationWindow, ConstantsDT.ERROR_SC_INVALID_3PH_VOLTAGE, ConstantsDT.APPLICATION_NAME, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    m_CustomCommandHelper.Complete();
                    return;
                }

                // Check if Transformer is a Tie Transformer
                string sql = "select g3e_fid from " + ConstantsDT.TABLE_XFMR_OH_BANK + " where ((" + ConstantsDT.FIELD_XFMR_TIE_XFMR_ID + " = ?) or " +
                                "(g3e_fid = ? and " + ConstantsDT.FIELD_XFMR_TIE_XFMR_ID + " is not null)) and " + ConstantsDT.FIELD_XFMR_TIE_XFMR_ID + " <> g3e_fid " +
                                "union " +
                                "select g3e_fid from " + ConstantsDT.TABLE_XFMR_UG_UNIT + " where ((" + ConstantsDT.FIELD_XFMR_TIE_XFMR_ID + " = ?) or " +
                                "(g3e_fid = ? and " + ConstantsDT.FIELD_XFMR_TIE_XFMR_ID + " is not null)) and " + ConstantsDT.FIELD_XFMR_TIE_XFMR_ID + " <> g3e_fid";

                int recordsAffected = 0;

                ADODB.Recordset rs = m_Application.DataContext.Execute(sql, out recordsAffected, (int)CommandTypeEnum.adCmdText, xfmrFID, xfmrFID, xfmrFID, xfmrFID);

                if (rs.RecordCount > 0)
                {
                    MessageBox.Show(m_Application.ApplicationWindow, ConstantsDT.ERROR_SC_TIE_XFMR_SELECTED, ConstantsDT.APPLICATION_NAME, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    m_CustomCommandHelper.Complete();
                    return;
                }

                m_frmSecondaryCalculator.m_CustomCommandHelper = CustomCommandHelper;
                m_frmSecondaryCalculator.StartPosition = FormStartPosition.CenterScreen;
                m_frmSecondaryCalculator.Show(m_Application.ApplicationWindow);
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
                m_TransactionManager = null;
                m_CustomCommandHelper = null;
                m_frmSecondaryCalculator.m_CustomCommandHelper = null;
                m_frmSecondaryCalculator.m_TransactionManager = null;
                m_frmSecondaryCalculator = null;
            }
            catch (Exception ex)
            {
                MessageBox.Show(m_Application.ApplicationWindow, ex.Message, ConstantsDT.APPLICATION_NAME, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }
    }
}
