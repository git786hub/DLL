// ===================================================
//  Copyright 2018 Intergraph Corp.
//  File Name: ccCreateBusTransformers.cs
// 
//  Description: This command allows users to identify multiple transformers as being bussed together.
//  Remarks: 
//  Modification History
//  Date                Author                      Comments
//  --------            --------------              --------
//  24/05/2018          Shubham                     Created 
// ======================================================

using System.Collections.Generic;
using System.Windows.Forms;
using Intergraph.GTechnology.API;
using Intergraph.GTechnology.Interfaces;
using System;

namespace GTechnology.Oncor.CustomAPI
{
    public class ccCreateBusTransformers : IGTCustomCommandModal
    {
        private IGTTransactionManager m_TransactionManager;
        public IGTTransactionManager TransactionManager { set => m_TransactionManager = value; }
        private IGTApplication m_oApp;
        private IGTDDCKeyObjects m_keyobjects;

        public void Activate()
        {
            try
            {

                m_oApp = GTClassFactory.Create<IGTApplication>();
                m_keyobjects = m_oApp.SelectedObjects.GetObjects();

                SelectedTransformerProperties oProcess = new SelectedTransformerProperties(m_keyobjects);

                if (oProcess.ValidateSelecSetForTransformer()) //Proceed only if select set is validated
                {
                    Dictionary<int, short> distinctSelectSet = oProcess.GetDistinctSelectedTransformer;

                    if (distinctSelectSet.Count == 1)
                    {
                        if (oProcess.IsTransformerBussed())
                        {
                            UserPreferenceForm oForm = new UserPreferenceForm();
                            oForm.ShowAdditionalButton = false;
                            oForm.PromptText = "Disassociate this transformer from its existing bus?";
                            oForm.OkButtonCaption = "OK";
                            oForm.ShowCancelButton = true;
                            oForm.ShowDialog(m_oApp.ApplicationWindow);

                            if (oForm.ButtonClicked == ButtonClicked.Ok)
                            {
                                m_TransactionManager.Begin("Update Tie Transformer ID...");
                                oProcess.UnBussTransformer();
                                m_TransactionManager.Commit();
                            }
                            else
                            {
                                return;
                            }
                        }
                        else
                        {
                            UserPreferenceForm oForm = new UserPreferenceForm();
                            oForm.ShowAdditionalButton = false;
                            oForm.ShowCancelButton = false;

                            oForm.PromptText = "This command can only operate on two or more transformers.";
                            oForm.OkButtonCaption = "OK";
                            oForm.ShowDialog(m_oApp.ApplicationWindow);
                            return;
                        }
                    }
                    else if (distinctSelectSet.Count > 1)
                    {
                        if (oProcess.IsTransformerBussed())
                        {
                            UserPreferenceForm oForm = new UserPreferenceForm();

                            oForm.ShowAdditionalButton = true;
                            oForm.ShowCancelButton = true;

                            oForm.AdditionalButtonCaption = "Un-bus";
                            oForm.OkButtonCaption = "Create";

                            oForm.PromptText = "One or more selected transformers are already bussed. \nUn-bus the transformers, or create a new bus?";

                            oForm.ShowDialog(m_oApp.ApplicationWindow);

                            switch (oForm.ButtonClicked)
                            {
                                case ButtonClicked.Ok:
                                    m_TransactionManager.Begin("Update Tie Transformer ID...");
                                    oProcess.BussAllTransformer(m_keyobjects[0].FID);
                                    m_TransactionManager.Commit();
                                    break;
                                case ButtonClicked.Cancel:
                                    return;
                                case ButtonClicked.Additional:
                                    m_TransactionManager.Begin("Update Tie Transformer ID...");
                                    oProcess.UnBussTransformer();
                                    m_TransactionManager.Commit();
                                    break;
                                default:
                                    break;
                            }
                        }
                        else
                        {
                            m_TransactionManager.Begin("Update Tie Transformer ID...");
                            oProcess.BussAllTransformer(m_keyobjects[0].FID);
                            m_TransactionManager.Commit();
                        }
                    }
                }
                else
                {
                    MessageBox.Show("This command is applicable only for Transformers of the same type", "G/Technology", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }

            catch (Exception)
            {
                throw;
            }
        }
    }
}
