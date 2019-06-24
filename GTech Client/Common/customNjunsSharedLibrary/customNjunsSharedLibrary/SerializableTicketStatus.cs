using OncorTicketStatus;
using System.Xml;
using System.Xml.Serialization;

namespace GTechnology.Oncor.CustomAPI
{
    [XmlRootAttribute("disTicketStatusRequest", Namespace = "http://www.oncor.com/DIS_TicketCreationService", IsNullable = false)]
    public class SerializableTicketStatus
    {
        [XmlAttribute(Namespace = "http://www.w3.org/2001/XMLSchema-instance")]
        public string schemaLocation = "http://www.oncor.com/DIS_TicketStatusService../../SharedResources/Schemas/NJUNS_TS_Service.xsd";
        [XmlElement(ElementName = "RequestHeader", Namespace = "http://www.oncor.com/DIS")]
        public RequestHeaderType RequestHeaderType;
        [XmlElement(ElementName = "TicketStatus", Namespace = "http://www.oncor.com/NJUNS_Ticket")]
        public TicketStatusType TicketStatusype;

    }
}
