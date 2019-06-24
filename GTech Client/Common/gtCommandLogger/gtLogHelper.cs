using ADODB;
using Intergraph.GTechnology.API;
using System;
using System.Diagnostics;

namespace gtCommandLogger
{
    public class gtLogHelper
    {
        IGTApplication tmpApp;
        public gtLogHelper()
        {
            tmpApp = GTClassFactory.Create<IGTApplication>();
        }
        public bool CheckIfLoggingIsEnabled()
        {
            tmpApp = GTClassFactory.Create<IGTApplication>();
            for (short i = 0; i < tmpApp.Application.Properties.Keys.Count; i++)
            {
                if (Convert.ToString(tmpApp.Application.Properties.Keys.Get(i)).Equals("EnableLogging"))
                {
                    return tmpApp.Properties["EnableLogging"].ToString() == "Y" ? true : false;
                }
            }
            return false;
        }

    }
}
