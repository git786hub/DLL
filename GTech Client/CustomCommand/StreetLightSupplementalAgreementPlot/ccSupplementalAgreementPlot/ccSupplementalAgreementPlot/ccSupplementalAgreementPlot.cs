// ===================================================
//  Copyright 2018 Intergraph Corp.
//  File Name: ccSupplementalAgreementPlot.cs
// 
//  Description:  The Generate Street Light Supplemental Agreement Plot command will provide a Designer or SLA the ability to generate a Street Light Supplemental Agreement Plot.  This command generates and displays a G/Technology plot window based on a named plot stored in the workspace.
//  Remarks: 
//  Modification History
//  Date                Author                      Comments
//  --------            --------------              --------
//  29/03/2018          Sithara                     Created 
// ======================================================
using System;
using Intergraph.GTechnology.API;
using Intergraph.GTechnology.Interfaces;
using System.Windows.Forms;
using GTechnology.Oncor.CustomAPI.View;
using GTechnology.Oncor.CustomAPI.Presenter;

namespace GTechnology.Oncor.CustomAPI
{
    public class ccSupplementalAgreementPlot : IGTCustomCommandModeless
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
                    SupplementalAgreementPlotPresenter presenter = new SupplementalAgreementPlotPresenter(GTDataContext
                        , GTDDCKeyObjects, m_iGtApplication, CustomCommandHelper);

                    if (presenter.IsValidCommand())
                    {
                        AgreementPlotForm agreementPlotForm = new AgreementPlotForm(presenter);
                        agreementPlotForm.StartPosition = FormStartPosition.CenterParent;
                        agreementPlotForm.ShowDialog(m_iGtApplication.ApplicationWindow);
                    }
                    else
                    {
                        MessageBox.Show(presenter.m_UserMessage, "G/Technology", MessageBoxButtons.OK, MessageBoxIcon.Information,
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
                MessageBox.Show("User does not have PRIV_DESIGN_ALL role.", "G/Technology", MessageBoxButtons.OK, MessageBoxIcon.Error,
                            MessageBoxDefaultButton.Button1);
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
