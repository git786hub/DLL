using System.Xml;
using System.Xml.Serialization;

namespace CustomWriteBackLibrary
{
    namespace CustomWriteBackLibrary
    {
        //[System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.6.1055.0")]
        //[System.SerializableAttribute()]
        //[System.Diagnostics.DebuggerStepThroughAttribute()]
        //[System.ComponentModel.DesignerCategoryAttribute("code")]
        //[System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.oncor.com/DIS")]      
        [System.Xml.Serialization.XmlRootAttribute("disWorkRequestRequest", Namespace = "http://www.oncor.com/DIS_WorkRequestService", IsNullable = false)]
        public class UpdateJobStatusSerializable
        {
            [XmlAttribute(Namespace = "http://www.w3.org/2001/XMLSchema-instance")]
            public string schemaLocation = "http://www.oncor.com/DIS_WorkRequestService ../../Schema/WMIS_WR_Service.xsd";            
            [XmlElement(ElementName = "RequestHeader", Namespace = "http://www.oncor.com/DIS")]
            public RequestHeaderType RequestHeaderType;
            [XmlElement(ElementName = "WorkRequest", Namespace = "http://www.oncor.com/Work_Request")]
            public WorkRequestType WorkRequestType;

        }
    }
}




