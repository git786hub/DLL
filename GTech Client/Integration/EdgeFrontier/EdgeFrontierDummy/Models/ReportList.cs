using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Runtime.Serialization;

namespace EdgeFrontierDummy.Models
{
    [DataContract(Namespace = "")]
    public class ReportList
    {
        [DataMember]
        public string Name { get; set; }
    }
}