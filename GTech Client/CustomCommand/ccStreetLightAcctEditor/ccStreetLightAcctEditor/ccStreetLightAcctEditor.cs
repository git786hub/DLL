
//  Remarks:
// 
//  Modification History
//  Date                Author                      Comments
//  --------            --------------              --------
//  22/02/2018          Pramod                     Implemented Street Light Account Editor and Value Lists 
//  05/03/2018          Pramod                     Implemented Street Light Boundary 
//  22/03/2018          Pramod                     Implemented Managing Non-Located Street Lights
// ======================================================

using GTechnology.Oncor.CustomAPI.DataAccess;
using GTechnology.Oncor.CustomAPI.GUI;
using Intergraph.GTechnology.API;
using Intergraph.GTechnology.Interfaces;
using System;
using System.Windows.Forms;

namespace GTechnology.Oncor.CustomAPI
{
    public class ccStreetLightAcctEditor : IGTCustomCommandModeless
    {
        IGTApplication _gtApp = null;
        IGTTransactionManager _gtTransactionManager;
        public bool CanTerminate => true;
        StreetLighAccountEditor stltAcctEditor = null;

        public IGTTransactionManager TransactionManager { set => _gtTransactionManager=value; }

        public void Activate(IGTCustomCommandHelper CustomCommandHelper)
        {
            _gtApp = GTClassFactory.Create<IGTApplication>();
            try
            {
                stltAcctEditor = new StreetLighAccountEditor(CustomCommandHelper,_gtTransactionManager);
                stltAcctEditor.Show(_gtApp.ApplicationWindow);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error " + ex.Message, "Street Light Account Editor", MessageBoxButtons.OK, MessageBoxIcon.Error);
                if (_gtTransactionManager != null)
                {
                    if (_gtTransactionManager.TransactionInProgress)
                        _gtTransactionManager.Rollback();
                }
                _gtTransactionManager = null;
                if (CustomCommandHelper != null)
                {
                    CustomCommandHelper.Complete();
                }
                CustomCommandHelper = null;
                stltAcctEditor = null;
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
            _gtTransactionManager = null;
        }

        
    }
    }
