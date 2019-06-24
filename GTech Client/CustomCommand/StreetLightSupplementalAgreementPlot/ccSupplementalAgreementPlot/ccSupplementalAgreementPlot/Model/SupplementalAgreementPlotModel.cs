using System;
using System.Collections.Generic;
using System.Windows.Forms;
using Intergraph.GTechnology.API;
using ADODB;

namespace GTechnology.Oncor.CustomAPI.Model
{
    public class SupplementalAgreementPlotModel : ISupplementalAgreementPlotModel
    {
        IGTDataContext m_gTDataContext;
        IGTDDCKeyObjects m_gTDDCKeyObjects;
        IGTKeyObjects m_gTKeyObjects;
        IGTApplication m_gTApplication;
        IGTCustomCommandHelper m_gTCustomCommandHelper;
        string[] m_SelectedEsiLocations;
        string m_strMessage = "";
        string m_strPlotName = "";
        string m_strParamName = "";        
        int m_descriptionId = 0;
        ProcessPlot processPlot = null;

        IGTNamedPlot m_gTNamedPlot = null;
        public IGTDataContext gTDataContext
        {
            get { return m_gTDataContext; }
            set { m_gTDataContext = value; }
        }
        public IGTDDCKeyObjects gTDDCKeyObjects
        {
            get { return m_gTDDCKeyObjects; }
            set { m_gTDDCKeyObjects = value; }
        }
        public IGTKeyObjects gTKeyObjects
        {
            get { return m_gTKeyObjects; }
            set { m_gTKeyObjects = value; }
        }
        public IGTApplication gTApplication
        {
            get { return m_gTApplication; }
            set { m_gTApplication = value; }
        }
        public IGTCustomCommandHelper gTCustomCommandHelper
        {
            get { return m_gTCustomCommandHelper; }
            set { m_gTCustomCommandHelper = value; }
        }

        string m_ActiveWorkRequest;
        public string ActiveWorkRequest { get { return m_ActiveWorkRequest; } set { m_ActiveWorkRequest = value; } }
        public SupplementalAgreementPlotModel(IGTDataContext gTDC ,IGTDDCKeyObjects gTDDCKey,
            IGTApplication gTApp,IGTCustomCommandHelper gTCCHeler)
        {
            gTDataContext = gTDC;
            gTDDCKeyObjects = gTDDCKey;
            gTApplication = gTApp;
            gTCustomCommandHelper = gTCCHeler;            
        }
        public string strErrorMessage { get { return m_strMessage; } set { m_strMessage = value; } }
        public string strPlotName { get { return processPlot.GetAvailablePlotName(); } set { m_strPlotName = value; } }
        public string strParamName { get { return m_strParamName; } set { m_strParamName = value; } }            
        public IGTNamedPlot gTNamedPlot { get { return m_gTNamedPlot; } set { m_gTNamedPlot = value; } }

        public bool IsValid()
        {
            bool validate = true;
            try
            {
                CheckValidation checkValidation = new CheckValidation(gTDataContext, gTDDCKeyObjects, gTApplication, gTCustomCommandHelper);
                checkValidation.strParamName = strParamName;
                validate = checkValidation.IsValid();
                strErrorMessage = checkValidation.strErrorMessage;
                ActiveWorkRequest = checkValidation.ActiveWorkRequest;
                m_SelectedEsiLocations = checkValidation.m_SelectedEsiLocations;
                m_descriptionId = checkValidation.m_descriptionId;
                gTKeyObjects = checkValidation.gTKeyObjects;
                gTNamedPlot = checkValidation.gTNamedPlot;
                processPlot = new ProcessPlot(gTApplication, gTNamedPlot, m_SelectedEsiLocations);
            }
            catch
            {
                throw;
            }
            return validate;
        }

        public bool GeneratePlot(string strPlot, string strCity, string strDivision)
        {            
            return processPlot.GeneratePlot(strPlot, strCity, strDivision);
        }
        public void ClearCC()
        {
            if (m_gTCustomCommandHelper != null)
            {
                m_gTCustomCommandHelper.Complete();
                m_gTCustomCommandHelper = null;
            }
        }
       
    }
}
