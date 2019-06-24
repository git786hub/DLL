//----------------------------------------------------------------------------+
//        Class: ccManagePreferredCUs
//  Description: This command allows user to add or remove preferred CUs using list of available CUs
//----------------------------------------------------------------------------+
//     $Author:: hkonda                                   $
//       $Date:: 23/12/17                                 $
//   $Revision:: 1                                        $
//----------------------------------------------------------------------------+
//    $History:: ccManagePreferredCUs.cs                     $
// 
// *****************  Version 1  *****************
// User: hkonda     Date: 23/12/17   Time: 18:00  Desc : Created
//----------------------------------------------------------------------------+
using Intergraph.GTechnology.Interfaces;
using System;
using Intergraph.GTechnology.API;
using System.Windows.Forms;

namespace GTechnology.Oncor.CustomAPI
{
    public class ccManagePreferredCUs : IGTCustomCommandModal
    {
        private IGTTransactionManager m_TransactionManager;
        IGTApplication m_iGtApplication;
        private IGTDataContext m_DataContext;
        public ccManagePreferredCUs()
        {
            m_iGtApplication = GTClassFactory.Create<IGTApplication>();
            m_DataContext = m_iGtApplication.DataContext;
        }
        public IGTTransactionManager TransactionManager
        {
            set
            {
                m_TransactionManager = value;
            }
        }

        public void Activate()
        {
            try
            {
                frmPreferredCUs oFrmPreferredCUs = new frmPreferredCUs(m_DataContext);
                if (oFrmPreferredCUs.ShowDialog(m_iGtApplication.ApplicationWindow) == DialogResult.OK)
                {
                    oFrmPreferredCUs.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error during execution of Managed Preferred CUs custom command." + Environment.NewLine + ex.Message, "G/Techonology", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
