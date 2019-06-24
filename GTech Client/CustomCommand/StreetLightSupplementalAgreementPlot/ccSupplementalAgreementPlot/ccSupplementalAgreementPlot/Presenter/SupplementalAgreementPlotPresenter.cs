using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GTechnology.Oncor.CustomAPI.Model;
using GTechnology.Oncor.CustomAPI.View;
using Intergraph.GTechnology.API;
using System.Windows.Forms;

namespace GTechnology.Oncor.CustomAPI.Presenter
{
    public class SupplementalAgreementPlotPresenter
    {
        private SupplementalAgreementPlotModel m_model;
        public string m_UserMessage = "";
        public string m_PlotName = "";
        public SupplementalAgreementPlotPresenter(IGTDataContext gTDataContext,IGTDDCKeyObjects gTDDCKeyObjects,
            IGTApplication gTApplication,IGTCustomCommandHelper gTCustomCommandHelper)
        {          
            m_model = new SupplementalAgreementPlotModel(gTDataContext, gTDDCKeyObjects, gTApplication, gTCustomCommandHelper);            
        }
        public bool IsValidCommand()
        {
            try
            {
                m_model.strParamName = "StltSplmnt_Plot";
                if (m_model.IsValid())
                {
                    m_PlotName = m_model.strPlotName;
                    return true;
                }
                else
                {
                    m_UserMessage = m_model.strErrorMessage;
                }

            }
            catch
            {
                throw;
            }

            return false;
        }

        public bool GeneratePlot(string strName, string strCity, string strDivision)
        {
            try
            {
                return m_model.GeneratePlot(strName, strCity, strDivision);
            }
            catch
            {
                throw;
            }
        }

        public void CompleteCC()
        {
            try
            {
                m_model.ClearCC();
            }
            catch
            {
                throw;
            }
        }

    }
}
