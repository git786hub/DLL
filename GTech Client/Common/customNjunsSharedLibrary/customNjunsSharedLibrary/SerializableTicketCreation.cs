using OncorTicketCreation;
using System.Xml;
using System.Xml.Serialization;

namespace GTechnology.Oncor.CustomAPI
{
    [XmlRootAttribute("disTicketCreationRequest", Namespace = "http://www.oncor.com/DIS_TicketCreationService", IsNullable = false)]
    public class SerializableTicketCreation
    {
        [XmlAttribute(Namespace = "http://www.w3.org/2001/XMLSchema-instance")]
        //[XmlAttribute(Namespace = "http://www.oncor.com/DIS_TicketCreationService")]
        public string schemaLocation = "http://www.oncor.com/DIS_TicketCreationService../../SharedResources/Schemas/NJUNS_TC_Service.xsd";
        [XmlElement(ElementName = "RequestHeader", Namespace = "http://www.oncor.com/DIS")]
        public RequestHeaderType RequestHeaderType;
        [XmlElement(ElementName = "TicketCreation", Namespace = "http://www.oncor.com/NJUNS_Ticket")]
        public TicketCreationType TicketCreationType;
    }
}
