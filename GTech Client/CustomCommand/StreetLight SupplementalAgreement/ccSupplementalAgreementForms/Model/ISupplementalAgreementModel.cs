using System;
using System.Data;
using Intergraph.GTechnology.API;

namespace GTechnology.Oncor.CustomAPI.Model
{
    public interface ISupplementalAgreementModel
    {
        bool IsWRJob();
        bool IsSelectSetStreetLight();
        bool IsSelectSetHasESILocation();
        bool IsSelectSetHaSameCustomer();
        bool IsMSLAExist();
        bool IsCommandValid();
        string NotifyModelMess { get; set; }
        string ActiveWorkRequest { get; set; }
        string Customer { get; set; }
        int Additions { get; set; }
        int Removal { get; set; }
        DateTime? AgreementDate { get; set; }
        DataTable GridDataTable { get; set; }
        string GetFormTemplateLocation(string strPName);        
        IGTKeyObject DesignAreaKeyObject { get; set; }
        bool IsExistingAttachment(string strFilename);

        void DeleteExistingAttachment(string strFilename);

    }
}
