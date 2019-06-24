using System;
using Intergraph.GTechnology.API;

namespace GTechnology.Oncor.CustomAPI
{
    public class GISAuto_WMIS : IGISAutoPlugin
    {
        private IGTTransactionManager m_TransactionManager;
        private IGTApplication m_Application = GTClassFactory.Create<IGTApplication>();

        public GISAuto_WMIS(IGTTransactionManager transactionManager)
        {
            m_TransactionManager = transactionManager;
        }

        public string SystemName
        {
            get
            {
                return "WMIS";
            }
        }

        public void Execute(GISAutoRequest autoRequest)
        {            
           ProcessTransaction(autoRequest.requestXML.ToString());
        }

        private bool ProcessTransaction(string xml)
        {
            bool returnValue = false;
            //int recordsAffected = 0;

            //string sql = "insert into Gisautomator_Plugin_Test (REQUEST_SYSTEM_NM, request_xml, edit_ts) values (?, ?, sysdate)";

            //m_Application.DataContext.Execute(sql, out recordsAffected, -1, SystemName, xml);

            //m_Application.DataContext.Execute("commit", out recordsAffected, -1);

            return returnValue;
        }
    }

    public class WMISException : Exception
    {                
        public WMISException(int Severity, string message) :base(message)
        {
           HResult = Severity;
        }
    }
}
