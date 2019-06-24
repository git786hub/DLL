// ===================================================
//  Copyright 2018 Intergraph Corp.
//  File Name: ccSupplementalAgreementForms.cs
// 
//  Description:  A single custom command allows the user to generate either type of agreement.  The command will determine the appropriate dialog to display (and, thus, the appropriate form to generate) based on the existence of an MSLA Date for the selected Street Lights
//  Remarks: 
//  Modification History
//  Date                Author                      Comments
//  --------            --------------              --------
//  29/03/2018          Sithara                     Created 
// ======================================================
using System;
using System.Windows.Forms;
using Intergraph.GTechnology.Interfaces;
using Intergraph.GTechnology.API;
using GTechnology.Oncor.CustomAPI.View;
using GTechnology.Oncor.CustomAPI.Presenter;

namespace GTechnology.Oncor.CustomAPI
{
    public class ccSupplementalAgreementForms : IGTCustomCommandModeless , ISupplementalAgreementView
    {
        private IGTTransactionManager m_oGTTransactionManager;
        IGTApplication m_iGtApplication = null;
        IGTDataContext m_gTDataContext = null;
        IGTDDCKeyObjects m_gTDDCKeyObjects = null;
        bool m_canTerminate = false;
        SupplementalAgreementPresenter presenter;
        IGTCustomCommandHelper m_gTCustomCommandHelper = null;

        public IGTTransactionManager TransactionManager
        {
            get { return m_oGTTransactionManager; }
            set { m_oGTTransactionManager = value; }
        }       
        public IGTDataContext GTDataContext
        {
            get
            {
                m_iGtApplication= GTClassFactory.Create<IGTApplication>();
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

        public bool IsCommandValid
        {
            get;
            set;
        }

        public void Activate(IGTCustomCommandHelper CustomCommandHelper)
        {
            m_gTCustomCommandHelper = CustomCommandHelper;
            if (GTDataContext.IsRoleGranted("PRIV_DESIGN_ALL"))
            {
                try
                {

                    presenter = new SupplementalAgreementPresenter(this, CustomCommandHelper, m_oGTTransactionManager);
                    //CustomCommandHelper.MouseMove += CustomCommandHelper_MouseMove;
                    string[] strAlterDesign;
                    
                    if (presenter.IsCCommandValid())
                    {
                        
                        //If command is valid and MSLA date exist CC loads MSLA form.
                        if (presenter.IsMSLAForm)
                        {
                            #region Form With MSLA

                            FormWithMSLA formWithMSLA = new FormWithMSLA();
                            formWithMSLA.StartPosition = FormStartPosition.CenterParent;


                            if (presenter.ActiveWR.Contains("-"))
                            {
                                strAlterDesign = new string[2];
                                strAlterDesign = presenter.ActiveWR.Split('-');
                                presenter.ActiveWR = strAlterDesign[0];
                            }
                            presenter.m_UserForm = formWithMSLA;
                            formWithMSLA.m_formPresenter = presenter;
                            formWithMSLA.ShowDialog(m_iGtApplication.ApplicationWindow);

                            #endregion
                        }
                        else
                        {
                            //If command is valid and MSLA date is null CC loads with out MSLA form.

                            #region Form Without MSLA

                            FormWithoutMSLA formWithoutMSLA = new FormWithoutMSLA();
                            formWithoutMSLA.StartPosition = FormStartPosition.CenterParent;


                            if (presenter.ActiveWR.Contains("-"))
                            {
                                strAlterDesign = new string[2];
                                strAlterDesign = presenter.ActiveWR.Split('-');
                                presenter.ActiveWR = strAlterDesign[0];
                            }
                            presenter.m_UserForm = formWithoutMSLA;
                            formWithoutMSLA.m_formPresenter = presenter;
                            formWithoutMSLA.ShowDialog(m_iGtApplication.ApplicationWindow);

                            #endregion
                        }
                    }
                    else
                    {
                        if (!string.IsNullOrEmpty(presenter.NotifyPresenterMess))
                        {
                            //GTClassFactory.Create<IGTApplication>().SetStatusBarText(GTStatusPanelConstants.gtaspcMessage, presenter.NotifyPresenterMess);
                            MessageBox.Show(presenter.NotifyPresenterMess, "G/Technology", MessageBoxButtons.OK, MessageBoxIcon.Information,
                            MessageBoxDefaultButton.Button1);


                            presenter = null;
                            //m_gTCustomCommandHelper.MouseMove -= CustomCommandHelper_MouseMove;
                            ExitCommand(m_gTCustomCommandHelper);
                        }
                        else
                        {
                            presenter = null;
                           // m_gTCustomCommandHelper.MouseMove -= CustomCommandHelper_MouseMove;
                            ExitCommand(m_gTCustomCommandHelper);
                        }
                        
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "G/Technology", MessageBoxButtons.OK, MessageBoxIcon.Error,
                            MessageBoxDefaultButton.Button1);

                    //CustomCommandHelper.MouseMove -= CustomCommandHelper_MouseMove;

                    if (m_oGTTransactionManager != null)
                    {
                        if (m_oGTTransactionManager.TransactionInProgress)
                            m_oGTTransactionManager.Rollback();
                    }
                    m_oGTTransactionManager = null;

                    ExitCommand(CustomCommandHelper);
                }
            }
            else
            {
                MessageBox.Show("User does not have PRIV_DESIGN_ALL role.", "G/Technology", MessageBoxButtons.OK, MessageBoxIcon.Information,
                            MessageBoxDefaultButton.Button1);
                //CustomCommandHelper.MouseMove -= CustomCommandHelper_MouseMove;
                ExitCommand(CustomCommandHelper);
            }
        }
       
        private void CustomCommandHelper_MouseMove(object sender, GTMouseEventArgs e)
        {
            if (!string.IsNullOrEmpty(presenter.NotifyPresenterMess))
                GTClassFactory.Create<IGTApplication>().SetStatusBarText(GTStatusPanelConstants.gtaspcMessage, presenter.NotifyPresenterMess);            
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
