using Intergraph.GTechnology.API;
using Intergraph.GTechnology.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GTechnology.Oncor.CustomAPI
{
    internal class ccManualLandbaseReview : IGTCustomCommandModeless
    {
        ManualLandbaseReviewFrm ManualLandbaseReviewFrm = null;
        IGTTransactionManager m_oGTTransactionManager = null;
        IGTCustomCommandHelper m_oGTCustomCommandHelper = null;
        IGTApplication m_application = null;
        public static ccManualLandbaseReview ManualLandbaseReviewCtl;

        public bool CanTerminate
        {
            get { return true; }
        }

        public IGTTransactionManager TransactionManager

        {
            get { return m_oGTTransactionManager; }
            set { m_oGTTransactionManager = value; }
        }

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        public void Activate(IGTCustomCommandHelper CustomCommandHelper)
        {
            m_oGTCustomCommandHelper = CustomCommandHelper;
            try
            {
                if (ManualLandbaseReviewCtl == null)
                {
                    m_application = (IGTApplication)GTClassFactory.Create<IGTApplication>();
                    ManualLandbaseReviewCtl = this;
                    Application.EnableVisualStyles();
                    ManualLandbaseReviewFrm = new ManualLandbaseReviewFrm(m_oGTCustomCommandHelper, m_oGTTransactionManager);
                    ManualLandbaseReviewFrm.Show(m_application.ApplicationWindow);
                }
                else
                {
                    MessageBox.Show("Command already running");
                }

            }
            catch (Exception ex)
            {
                 ManualLandbaseReviewCtl = null;


                if (ManualLandbaseReviewFrm != null)
                {
                    ManualLandbaseReviewFrm.CloseForm();
                    ManualLandbaseReviewFrm.Close();
                }
                if (m_oGTCustomCommandHelper != null)
                    m_oGTCustomCommandHelper.Complete();
            }
        }

        void IGTCustomCommandModeless.Terminate()
        {

        }

        void IGTCustomCommandModeless.Pause()
        {

        }

        void IGTCustomCommandModeless.Resume()
        {

        }
    }
}
