namespace GTechnology.Oncor.CustomAPI
{
    /// <summary>
    /// The IGISAutoPlugin interface processes automation requests 
    /// received by the GISAutomationBroker.
    /// </summary>
    public interface IGISAutoPlugin
    {
        /// <summary>
        /// Property to get the name of the system for which this plug-in can process requests; 
        /// must correspond to an expected value of GISAUTO_QUEUE.REQUEST_SYSTEM_NM.
        /// </summary>
        string SystemName { get; }

        /// <summary>
        /// Method that applies module-specific processing.
        /// </summary>
        /// <param name="autoRequest">Object representing the GIS Automation request</param>
        void Execute(GISAutoRequest autoRequest);
    }
}
