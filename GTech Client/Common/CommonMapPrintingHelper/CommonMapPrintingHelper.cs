//----------------------------------------------------------------------------+
//        Class: CommonMapPrintingHelper
//  Description: This class contains a method GenerateConstructionPlots which is called by external interfaces or commands to generate Construction Prints in PDF format.
//                                                                  
//----------------------------------------------------------------------------+
//          $Author     :: Shubham Agarwal                                       $
//          $Date       :: 10/01/2019                                               $
//          $Revision   :: 1                                                   $
//----------------------------------------------------------------------------+
//    $History:: CommonMapPrintingHelper.cs                     $
// 
// *****************  Version 1  *****************
// User: sagarwal     Date: 10/01/2019   Time: 18:00  Desc : Created
//----------------------------------------------------------------------------+

using System;
using System.Collections.Generic;

namespace Intergraph.GTechnology.API
{
    public class CommonMapPrintingHelper
    {
        IGTApplication m_gtAppliction = null;

        public CommonMapPrintingHelper(IGTApplication p_App)
        {
            m_gtAppliction = p_App;
        }

        /// <summary>
        /// Method to generate Construction Prints at a location supplied by caller
        /// </summary>
        /// <param name="p_LegendName">G3E_LEGEND.G3E_USERNAME</param>
        /// <param name="p_OutputFileName">Output location where file needs to be generated</param>
        /// <param name="p_OutputFilePath">Name of the exported PDF file. The name should NOT contain .PDF in it</param>
        /// <returns></returns>
        public bool GenerateConstructionPlots(string p_LegendName,string p_OutputFileName, string p_OutputFilePath)
        {
            bool b_Return = true;
            try
            {
                if (m_gtAppliction!=null)
                {                    
                    InputValidation oValidationConditions = new InputValidation();
                    ValidationErrors oValidationMessges = ValidationErrors.NoError;
                    List<PlotBoundaryAttributes> plotBoundaryList = null;

                    oValidationMessges = oValidationConditions.ValidateInitialInput(m_gtAppliction, p_OutputFilePath, p_LegendName, out plotBoundaryList);

                    switch (oValidationMessges)
                    {
                        case ValidationErrors.ActiveJobDoesNotExist:
                            b_Return = false;                            
                            throw new Exception("No Active Job Exists");
                        case ValidationErrors.PlotBoundaryDoesNotExist:
                            b_Return = false;                          
                            throw new Exception("No Plot Boundary Exists in active Job");                        
                        case ValidationErrors.OutputFilePathNotAccessible:
                            b_Return = false;
                            throw new Exception("Output Path not accessible");
                        case ValidationErrors.NotValidLegend:
                            b_Return = false;
                            throw new Exception("Legend " + p_LegendName  + " is not a valid legend");
                        case ValidationErrors.NoError:
                            break;
                        default:
                            break;
                    }

                    ValidationErrors oPlotTemplateValidation = ValidationErrors.NoError;

                    foreach (var item in plotBoundaryList) //We need to throw error even if one plot boundary does not qualify the validation condition
                    {
                        oPlotTemplateValidation = oValidationConditions.ValidatePlotTemplate(m_gtAppliction, item.PlotTemplateName);

                        if (oPlotTemplateValidation != ValidationErrors.NoError)
                        {
                            break;
                        }
                    }

                    switch (oPlotTemplateValidation)
                    {
                        case ValidationErrors.PlotTemplateDoesNotExist:
                            b_Return = false;
                            //return b_Return;
                            throw new Exception("One of the Plot Templates corresponding to Plot Boundary/(Boundaries) does not exist in active Job");
                        case ValidationErrors.MapFrameInPlotTemplateDoesNotExist:
                            b_Return = false;
                            //return b_Return;
                            throw new Exception("No Mapframe exists in plot template");
                        default:
                            break;
                    }

                    PDFExportHelper oPDFHelper = new PDFExportHelper();
                    oPDFHelper.BuildWorkPrint(plotBoundaryList, m_gtAppliction, p_OutputFilePath, p_OutputFileName, p_LegendName);
                }
            }
            catch (Exception)
            {
                throw;
            }

            return b_Return;
        }
    }
}
