using Intergraph.GTechnology.API;

namespace GTechnology.Oncor.CustomAPI.Model
{
    public interface ISupplementalAgreementPlotModel
    {
        IGTDataContext gTDataContext { get; set; }
        IGTDDCKeyObjects gTDDCKeyObjects { get; set; }
        IGTKeyObjects gTKeyObjects { get; set; }
        IGTApplication gTApplication { get; set; }
        IGTCustomCommandHelper gTCustomCommandHelper { get; set; }

        IGTNamedPlot gTNamedPlot { get; set; }

        string strErrorMessage { get; set; }
        string strPlotName { get; set; }
        string strParamName { get; set; }           
        bool IsValid();       
        bool GeneratePlot(string strPlot, string strCity, string strDivision);
        void ClearCC();

    }
}
