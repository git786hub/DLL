using Intergraph.EdgeFrontier.ManagedCall.Utilities;
using System;
using System.Data;
using System.Data.OleDb;
using System.IO;
using System.Timers;

namespace EdgeFrontierConnect
{
    public class EventRaised
    {
        Timer myTimer;
        int integer = 1;

        public string StartTimer(int interval)
        {
            myTimer = new Timer(interval);
            myTimer.Elapsed += MyTimer_Elapsed;
            myTimer.Start();
            return "Starting Timer";
        }

        private void MyTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            return;
            //Event.Raise((integer++).ToString(), "HELLO WORLD", e.SignalTime);
        }
    }
}
