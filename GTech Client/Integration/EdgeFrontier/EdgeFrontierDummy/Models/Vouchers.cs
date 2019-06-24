using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Runtime.Serialization;

namespace EdgeFrontierDummy.Models
{
    [DataContract(Namespace = "")]
    public class Vouchers
    {
        [DataMember]
        public string VOUCHER_NO { get; set; }
        [DataMember]
        public string REQUESTOR { get; set; }
        [DataMember]
        public string DATE_REQUESTED { get; set; }
        [DataMember]
        public string VOUCHER_CODE { get; set; }
        [DataMember]
        public string FERC_PRIMARY_ACCT { get; set; }
        [DataMember]
        public string FERC_SUB_ACCT { get; set; }
        [DataMember]
        public string COST_CMPT { get; set; }
        [DataMember]
        public string ESTIMATED_AMT { get; set; }
        [DataMember]
        public string COMMENT { get; set; }
    }
}