using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ADODB;
using Intergraph.GTechnology.API;
using System.Windows.Forms;

namespace SendEmail
{
    class csGlobals
    {
        internal static IGTApplication gApp;
        internal static IGTDataContext gDataCont;
        //internal static IGTCustomCommandHelper gCCHelper;
        //internal static IGTTransactionManager gTransMgr;
        internal static ADODB.Recordset gAppConfigRS = null;
       // internal static Form gFrmSendEmail;
    }
}
