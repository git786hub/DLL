using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Runtime.Serialization;

namespace EdgeFrontierDummy.Models
{
    [DataContract(Namespace = "")]
    public class WorkPoints
    {
        [DataMember]
        public string Number { get; set; }
        [DataMember]
        public string Type { get; set; }
        [DataMember]
        public string StructureIDFrom { get; set; }
        [DataMember]
        public string StructureIDTo { get; set; }
        [DataMember]
        public List<WP_CUData> WP_CUData { get; set; }
        [DataMember]
        public List<Vouchers> Vouchers { get; set; }
    }
}