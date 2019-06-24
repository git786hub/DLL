using System.Threading;

namespace GTechnology.Oncor.CustomAPI
{
    class GISAutomationMonitorProgram
    {
        static void Main()
        {
            // Create wait event so monitor doesn't exit unless there is an error.
            ManualResetEvent[] waitOnEvent = new ManualResetEvent[1];
            waitOnEvent[0] = new ManualResetEvent(false);

            // Start GISAutomationMonitor
            GISAutomationMonitor gisAutoMonitor = new GISAutomationMonitor();
            gisAutoMonitor.Run(waitOnEvent[0]);

            // Exit if an error is encountered (waitOnEvent.Set())
            int index = WaitHandle.WaitAny(waitOnEvent);            
        }
    }
}
