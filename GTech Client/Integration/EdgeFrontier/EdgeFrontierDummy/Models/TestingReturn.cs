using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Runtime.Serialization;

namespace EdgeFrontierDummy.Models
{
    [DataContract(Namespace = "")]
    public class TestingReturn
    {
        [DataMember]
        public string status { get; set; }
        [DataMember]
        public string id { get; set; }
    }
}