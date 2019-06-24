using System;
using Intergraph.GTechnology.API;
using Intergraph.GTechnology.Interfaces;
using System.Windows.Forms;

namespace GTechnology.Oncor.CustomAPI
{
    public class ccAttachSupplementalAgreementPlot : IGTCustomCommandModeless
    {
        private IGTTransactionManager m_oGTTransactionManager;
        bool m_canTerminate = false;
        IGTApplication m_iGtApplication = null;
        IGTDataContext m_gTDataContext = null;
        IGTDDCKeyObjects m_gTDDCKeyObjects = null;
        public IGTDataContext GTDataContext
        {
            get
            {
                m_iGtApplication = GTClassFactory.Create<IGTApplication>();
                return m_iGtApplication.DataContext;
            }
            set
            {
                m_gTDataContext = value;
            }
        }

        public IGTDDCKeyObjects GTDDCKeyObjects
        {
            get
            {
                return m_iGtApplication.SelectedObjects.GetObjects();
            }
            set
            {
                m_gTDDCKeyObjects = value;
            }
        }

        public bool CanTerminate
        {
            get { return m_canTerminate; }
        }

        public IGTTransactionManager TransactionManager
        {
            get { return m_oGTTransactionManager; }
            set { m_oGTTransactionManager = value; }
        }

        public void Activate(IGTCustomCommandHelper CustomCommandHelper)
        {
            if (GTDataContext.IsRoleGranted("PRIV_DESIGN_ALL"))
            {
                try
                {
                    CheckValidation checkValidation = new CheckValidation(GTDataContext);

                    if (checkValidation.IsWRJob())
                    {
                        ProcessStreetLight processStreetLight = new ProcessStreetLight(m_iGtApplication, TransactionManager);
                        if (processStreetLight.IsExistingAttachment())
                        {

                            if (MessageBox.Show(GTClassFactory.Create<IGTApplication>().ApplicationWindow,
                                    "The active WR already has a supplemental plot attached to it with the name " + processStreetLight.m_strPlotAttachmentName + " " +
                                    ".Do you wish to overwrite that plot with the active plot window ?", "G/Technology",
                                    MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.OK)
                            {
                                processStreetLight.DeleteExistingAttachment();
                                processStreetLight.AttachPlot();

                                MessageBox.Show("Successfully attached PDF of the plot window to the active WR.", "G/Technology",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);

                                ExitCommand(CustomCommandHelper);
                            }
                            else
                            {
                                ExitCommand(CustomCommandHelper);
                            }

                        }
                        else if(processStreetLight.m_gTDesignAreaKeyObject != null)
                        {
                            if (MessageBox.Show(GTClassFactory.Create<IGTApplication>().ApplicationWindow,
                                    "Attach plot " + processStreetLight.m_strPlotAttachmentName + " to this WR as the Supplemental Agreement Plot? ", "G/Technology",
                                    MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.OK)
                            {

                                processStreetLight.AttachPlot();
                                MessageBox.Show("Successfully attached PDF of the plot window to the active WR.", "G/Technology",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                                ExitCommand(CustomCommandHelper);
                            }
                            else
                            {
                                ExitCommand(CustomCommandHelper);
                            }

                        }
                        else
                        {
                            MessageBox.Show("Active Job does not have a Design Area.", "G/Technology", MessageBoxButtons.OK, MessageBoxIcon.Information,
                        MessageBoxDefaultButton.Button1);
                            ExitCommand(CustomCommandHelper);
                        }
                    }
                    else
                    {
                        MessageBox.Show("Active job must be a WR type.", "G/Technology", MessageBoxButtons.OK, MessageBoxIcon.Information,
                                MessageBoxDefaultButton.Button1);
                        ExitCommand(CustomCommandHelper);
                    }

                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "G/Technology", MessageBoxButtons.OK, MessageBoxIcon.Error,
                            MessageBoxDefaultButton.Button1);

                    ExitCommand(CustomCommandHelper);
                }
            }
            else
            {
                MessageBox.Show("User does not have PRIV_DESIGN_ALL role.", "G/Technology", MessageBoxButtons.OK, MessageBoxIcon.Information,
                           MessageBoxDefaultButton.Button1);
                ExitCommand(CustomCommandHelper);
            }
        }

        public void Pause()
        {

        }

        public void Resume()
        {

        }

        public void Terminate()
        {
            if (m_oGTTransactionManager != null)
            {
                if (m_oGTTransactionManager.TransactionInProgress)
                    m_oGTTransactionManager.Rollback();
            }
            m_oGTTransactionManager = null;
        }

        internal void ExitCommand(IGTCustomCommandHelper m_GTCustomCommandHelper)
        {
            try
            {
                if (m_GTCustomCommandHelper != null)
                {
                    m_GTCustomCommandHelper.Complete();
                    m_GTCustomCommandHelper = null;
                }

                m_iGtApplication.Application.RefreshWindows();
            }
            catch
            {
                if (m_GTCustomCommandHelper != null)
                {
                    m_GTCustomCommandHelper.Complete();
                    m_GTCustomCommandHelper = null;
                }

                m_GTCustomCommandHelper = null;
                m_iGtApplication.Application.RefreshWindows();
            }
        }
    }
}
