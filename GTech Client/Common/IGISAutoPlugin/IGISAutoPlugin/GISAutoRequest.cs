namespace GTechnology.Oncor.CustomAPI
{
    /// <summary>
    /// Object representing the GIS Automation request
    /// </summary>
    public class GISAutoRequest
    {
        public int requestID;
        public string requestSystemName;
        public string requestServiceName;
        public int requestTransactionID;
        public string requestXML;
        public string requestWRNumber;
        public string auditUserID;
        public string auditTimeStamp;
    }
}
