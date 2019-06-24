using System;
using System.Windows.Forms;
using Intergraph.GTechnology.API;


namespace GTechnology.Oncor.CustomAPI.View
{
    public interface ISupplementalAgreementView
    {       
        bool IsCommandValid
        {
            get;
            set;
        }
        IGTDataContext GTDataContext { get; set; }
        IGTDDCKeyObjects GTDDCKeyObjects { get; set; }            
    }
}
