using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Intergraph.GTechnology.API;
using Intergraph.GTechnology.Interfaces;
using ADODB;

namespace GTechnology.Oncor.CustomAPI
{
    public class ccDisplayConstrRedLines : IGTCustomCommandModal
    {
        public IGTTransactionManager TransactionManager
        {
            set
            {
                csGlobals.gTransManager = value;
            }
        }

        public void Activate()
        {

            csGlobals.gApp = GTClassFactory.Create<IGTApplication>();
            csGlobals.gCurrActMapWind = csGlobals.gApp.ActiveMapWindow;
            csGlobals.gGetRedLineFilesfromSp();

        }
    }
}
