using System;
using Intergraph.GTechnology.API;
using Intergraph.GTechnology.Interfaces;
using System.Windows.Forms;

namespace GTechnology.Oncor.CustomAPI
{
    public class ccStreetLightVoltageDrop : IGTCustomCommandModeless
    {
        private IGTTransactionManager m_TransactionManager;
        private IGTApplication m_Application = GTClassFactory.Create<IGTApplication>();
        private IGTCustomCommandHelper m_CustomCommandHelper;
        frmStreetLightVoltageDrop frmStreetLightVoltageDrop = new frmStreetLightVoltageDrop();

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
                    frmStreetLightVoltageDrop.m_TransactionManager = m_TransactionManager;
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

                IGTDDCKeyObjects oGTDCKeys = GTClassFactory.Create<IGTDDCKeyObjects>();

                // Check if selected feature is a Transformer.
                oGTDCKeys = m_Application.SelectedObjects.GetObjects();
                if (!(oGTDCKeys[0].FNO == ConstantsDT.FNO_OH_XFMR || oGTDCKeys[0].FNO == ConstantsDT.FNO_UG_XFMR))
                {
                    MessageBox.Show(m_Application.ApplicationWindow, ConstantsDT.ERROR_SC_INVALID_FEATURE_SELECTED, ConstantsDT.APPLICATION_NAME, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    m_CustomCommandHelper.Complete();
                    return;
                }

                frmStreetLightVoltageDrop.m_CustomCommandHelper = CustomCommandHelper;
                frmStreetLightVoltageDrop.m_TransactionManager = m_TransactionManager;
                frmStreetLightVoltageDrop.StartPosition = FormStartPosition.CenterScreen;
                frmStreetLightVoltageDrop.Show(m_Application.ApplicationWindow);
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
                frmStreetLightVoltageDrop.m_CustomCommandHelper = null;
                frmStreetLightVoltageDrop.m_TransactionManager = null;
                frmStreetLightVoltageDrop = null;
            }
            catch (Exception ex)
            {
                MessageBox.Show(m_Application.ApplicationWindow, ex.Message, ConstantsDT.APPLICATION_NAME, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }
    }
}
