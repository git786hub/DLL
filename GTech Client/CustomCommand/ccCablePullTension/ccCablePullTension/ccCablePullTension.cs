using System;
using Intergraph.GTechnology.API;
using Intergraph.GTechnology.Interfaces;
using System.Windows.Forms;

namespace GTechnology.Oncor.CustomAPI
{
    public class ccCablePullTension : IGTCustomCommandModeless
    {
        private IGTTransactionManager m_TransactionManager;
        private IGTApplication m_Application = GTClassFactory.Create<IGTApplication>();
        private IGTCustomCommandHelper m_CustomCommandHelper;
        frmCablePullTension frmCablePullTension = new frmCablePullTension();

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
                    frmCablePullTension.m_TransactionManager = m_TransactionManager;
                }
                catch (Exception ex)
                {
                    MessageBox.Show(m_Application.ApplicationWindow, "IGTCustomCommandModeless_TransactionManager:" + Environment.NewLine + "Error (" + ex.Source + ") - " + ex.Message, ex.Source, System.Windows.Forms.MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
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

                if (m_Application.SelectedObjects.FeatureCount > 1)
                {
                    MessageBox.Show(m_Application.ApplicationWindow, ConstantsDT.ERROR_CPT_MULTIPLE_FEATURES_SELECTED, ConstantsDT.COMMAND_NAME_CABLE_PULL_TENSION, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    m_CustomCommandHelper.Complete();
                    return;
                }
                else if (m_Application.SelectedObjects.FeatureCount == 1)
                {
                    // Only one feature in select set. Check if selected feature is a Duct Bank.
                    IGTDDCKeyObjects oGTDCKeys = m_Application.SelectedObjects.GetObjects();
                    IGTDDCKeyObject objKeyObject = GTClassFactory.Create<IGTDDCKeyObject>();
                    if (oGTDCKeys[0].FNO != ConstantsDT.FNO_UG_SECCOND && oGTDCKeys[0].FNO != ConstantsDT.FNO_UG_COND)
                    {
                        MessageBox.Show(m_Application.ApplicationWindow, ConstantsDT.ERROR_CPT_INVALID_FEATURE_SELECTED, ConstantsDT.COMMAND_NAME_CABLE_PULL_TENSION, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        m_CustomCommandHelper.Complete();
                        return;
                    }
                }

                frmCablePullTension.m_CustomCommandHelper = CustomCommandHelper;
                frmCablePullTension.StartPosition = FormStartPosition.CenterScreen;
                frmCablePullTension.Show(m_Application.ApplicationWindow);
                
                return;
            }
            catch (Exception ex)
            {
                MessageBox.Show(m_Application.ApplicationWindow, "IGTCustomCommandModeless_Activate:" + Environment.NewLine + "Error (" + ex.Source + ") - " + ex.Message, ex.Source, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
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
                frmCablePullTension.m_CustomCommandHelper = null;
                frmCablePullTension.m_TransactionManager = null;
                frmCablePullTension = null;
            }
            catch (Exception ex)
            {
                MessageBox.Show(m_Application.ApplicationWindow, "IGTCustomCommandModeless_Terminate:" + Environment.NewLine + "Error (" + ex.Source + ") - " + ex.Message, ex.Source, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }
    }
}
