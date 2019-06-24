using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Runtime.Serialization;

namespace EdgeFrontierDummy.Models
{
    [DataContract(Namespace = "")]
    public class UpdatedAttributes
    {
        [DataMember]
        public string Customer_Required_Date { get; set; }
        [DataMember]
        public string Construction_Ready_Date { get; set; }
    }
}