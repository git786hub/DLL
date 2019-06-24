using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Runtime.Serialization;

namespace EdgeFrontierDummy.Models
{
    [DataContract(Namespace = "")]
    public class Parameters
    {
        [DataMember]
        public headerInfo headerInfo { get; set; }
        [DataMember]
        public string reportType { get; set; }
        [DataMember]
        public string ID { get; set; }
        [DataMember]
        public bool generateLoopSummary { get; set; }
    }
}