using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Runtime.Serialization;

namespace EdgeFrontierDummy.Models
{
    [DataContract(Namespace = "")]
    public class WP_CUData
    {
        [DataMember]
        public string FID { get; set; }
        [DataMember]
        public string FNO { get; set; }
        [DataMember]
        public string ACCOUNT { get; set; }
        [DataMember]
        public string ACTIVITY { get; set; }
        [DataMember]
        public string CU { get; set; }
        [DataMember]
        public string CUC_DESCRIPTION { get; set; }
        [DataMember]
        public string DIFFICULTY_FACTOR { get; set; }
        [DataMember]
        public string MACRO_CU { get; set; }
        [DataMember]
        public string QUANTITY { get; set; }
        [DataMember]
        public string UNIT_CID { get; set; }
        [DataMember]
        public string USAGE { get; set; }
        [DataMember]
        public string TASK_CODE { get; set; }
        [DataMember]
        public string TASK_DESCRIPTION { get; set; }
        [DataMember]
        public string MATERIAL_CODE { get; set; }
    }
}