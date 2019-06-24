using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Runtime.Serialization;

namespace EdgeFrontierDummy.Models
{
    [DataContract(Namespace = "")]
    public class RequestData
    {
        //GIS_WMIS_CreateWR
        [DataMember]
        public string G3E_IDENTIFIER { get; set; }
        [DataMember]
        public string WR_Name { get; set; }
        [DataMember]
        public string WR_Type { get; set; }
        [DataMember]
        public string Designer_Assignment { get; set; }
        [DataMember]
        public string Job_Status { get; set; }
        [DataMember]
        public string WR_Scope { get; set; }
        [DataMember]
        public string WR_Story { get; set; }
        [DataMember]
        public string Supervisor_Selection { get; set; }
        [DataMember]
        public string Customer_Required_Date { get; set; }
        [DataMember]
        public string Construction_Ready_Date { get; set; }
        [DataMember]
        public string Street_Number { get; set; }
        [DataMember]
        public string Street_Name { get; set; }
        [DataMember]
        public string Town { get; set; }
        [DataMember]
        public string County { get; set; }
        [DataMember]
        public string Crew_Headquarters { get; set; }
        [DataMember]
        public string Electric_Location_Served { get; set; }
        [DataMember]
        public string Management_Activity_Code { get; set; }
        [DataMember]
        public string Office { get; set; }

        //GIS_WMIS_GetCUCatalog
        [DataMember]
        public string CUCode { get; set; }
        

        //GIS_WMIS_UpdateJobStatus
        [DataMember]
        public string WR_Number { get; set; }
        
        //GIS_WMIS_UpdateWR
        [DataMember]
        public UpdatedAttributes UpdatedAttributes { get; set; }
    }
}