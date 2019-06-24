using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Runtime.Serialization;

namespace EdgeFrontierDummy.Models
{
    [DataContract(Namespace = "")]
    public class Input
    {
        [DataMember]
        public string RequestType { get; set; }
        [DataMember]
        public RequestData RequestData { get; set; }
        [DataMember]
        public string WRNumber { get; set; }
        [DataMember]
        public string WR_Number { get; set; }
        [DataMember]
        public string AdditionalCost { get; set; }
        [DataMember]
        public List<ReportList> ReportList { get; set; }
        [DataMember]
        public List<WorkPoints> WorkPoints { get; set; }
        [DataMember]
        public List<RuleOverrides> RuleOverrides { get; set; }
    }
}