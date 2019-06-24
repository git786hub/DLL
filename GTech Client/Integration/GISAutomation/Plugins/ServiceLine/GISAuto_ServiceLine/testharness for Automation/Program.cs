using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using Intergraph.GTechnology.API;
using Intergraph.GTechnology.Interfaces;

namespace GTechnology.Oncor.CustomAPI
{
   internal class ccTransactionTestHarness : IGTCustomCommandModeless
    {
        private IGTTransactionManager gTransactionManager;
        public IGTTransactionManager TransactionManager
        {
            set
            {
                gTransactionManager = value;
            }
        }

        public bool CanTerminate
        {
            get
            {
                return true;
            }
        }

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
        }

        public void Activate(IGTCustomCommandHelper CustomCommandHelper)
        {
            Form1 window = new Form1(gTransactionManager);
            window.Show();
        }

        public void Pause()
        {
            throw new NotImplementedException();
        }

        public void Resume()
        {
            throw new NotImplementedException();
        }

        public void Terminate()
        {
        }
    }
}
