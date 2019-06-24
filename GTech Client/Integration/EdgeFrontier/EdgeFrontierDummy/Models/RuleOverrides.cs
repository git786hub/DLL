using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Runtime.Serialization;

namespace EdgeFrontierDummy.Models
{
    [DataContract(Namespace = "")]
    public class RuleOverrides
    {
        [DataMember]
        public string FeatureFID { get; set; }
        [DataMember]
        public string FeatureFNO { get; set; }
        [DataMember]
        public string FeatureClass { get; set; }
        [DataMember]
        public string StructureID { get; set; }
        [DataMember]
        public string Error { get; set; }
        [DataMember]
        public string Severity { get; set; }
        [DataMember]
        public string Reason { get; set; }
    }
}