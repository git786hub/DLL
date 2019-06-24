//----------------------------------------------------------------------------+
//        Class: PDFExportHelper
//  Description: This class contains methods to build Workprint and export it to PDF.
//                                                                  
//----------------------------------------------------------------------------+
//          $Author     :: Shubham Agarwal                                       $
//          $Date       :: 10/01/2019                                               $
//          $Revision   :: 1                                                   $
//----------------------------------------------------------------------------+
//    $History:: PDFGenerateHelper.cs                     $
// 
// *****************  Version 1  *****************
// User: sagarwal     Date: 10/01/2019   Time: 18:00  Desc : Created
//----------------------------------------------------------------------------+

using System;
using System.Collections.Generic;
using System.IO;

namespace Intergraph.GTechnology.API
{
    class PDFExportHelper
    {
        internal void BuildWorkPrint(List<PlotBoundaryAttributes> p_PlotBoundaryCollection, IGTApplication p_oApp, string p_destDir, string p_outputFileName, string p_legendName)
        {
            try
            {
                List<string> PDFNames = new List<string>();

                for (int i = 0; i < p_PlotBoundaryCollection.Count; i++)
                {
                    string sErrMsg = string.Empty;
                    IGTNamedPlot oNamedPlot = null;
                    IGTKeyObject oClipGeom = p_oApp.DataContext.OpenFeature(p_PlotBoundaryCollection[i].FNO, p_PlotBoundaryCollection[i].FID);
                    IGTComponent oComp = oClipGeom.Components.GetComponent(18802);

                    oComp.Recordset.MoveFirst();

                    IGTPolygonGeometry oGeom = (IGTPolygonGeometry)oComp.Geometry;

                    foreach (IGTNamedPlot item in p_oApp.NamedPlots)
                    {
                        if (item.Name.Equals(p_PlotBoundaryCollection[i].PlotTemplateName))
                        {
                            oNamedPlot = item;
                            break;
                        }
                    }

                    DeleteNamedPlot("OUTPUT - " + p_oApp.DataContext.ActiveJob + "-" + i, p_oApp);

                    IGTNamedPlot oCopiedPlot = oNamedPlot.CopyPlot("OUTPUT - " + p_oApp.DataContext.ActiveJob + "-" + i);
                    IGTPlotWindow oPlotWindow = p_oApp.NewPlotWindow(oCopiedPlot);
                    IGTPlotFrame oFrame = null;

                    foreach (IGTPlotFrame tmpFrame in oPlotWindow.NamedPlot.Frames)
                    {
                       // IGTPlotFrame oFrame = oPlotWindow.NamedPlot.Frames[0];
                       if (tmpFrame.Type == GTPlotFrameTypeConstants.gtpftMap)
                        {
                            oFrame = tmpFrame;
                            break;
                        }
                    }
                    oFrame.Activate();
                    oFrame.PlotMap.DisplayService.ReplaceLegend(p_legendName);
                    AssignClipGeometryToFrame(p_oApp, oFrame, oGeom);

                    oPlotWindow.Activate();

                    SetAutomaticFields(oPlotWindow.NamedPlot, p_PlotBoundaryCollection.Count, i + 1,p_oApp);
                    p_oApp.SetStatusBarText(GTStatusPanelConstants.gtaspcMessage, string.Format("Generating Construction Print {0} of {1}", i + 1, p_PlotBoundaryCollection));
                    ExportToPDF(p_destDir, p_outputFileName + "-" + i + ".pdf", false, oPlotWindow, out sErrMsg);
                    PDFNames.Add(p_outputFileName + "-" + i + ".pdf");
                    oPlotWindow.Close();
                    DeleteNamedPlot("OUTPUT - " + p_oApp.DataContext.ActiveJob + "-" + i + 1, p_oApp);
                }

                if (PDFNames.Count > 0)
                {
                    MergePDFs(p_destDir, PDFNames, p_outputFileName);
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        private void MergePDFs(string p_OutputLocation, List<string> p_PDFFiles, string p_OriginalFileName)
        {
            try
            {
                // Perform export
                IGTExportService svcExport = GTClassFactory.Create<IGTExportService>();
                svcExport.PDFLayersEnabled = false;

                if (File.Exists(Path.Combine(p_OutputLocation, p_OriginalFileName+".pdf")))
                {
                    File.Delete(Path.Combine(p_OutputLocation, p_OriginalFileName+".pdf"));
                }

                for (int i = 1; i < p_PDFFiles.Count; i++)
                {
                    svcExport.AppendPDF(Path.Combine(p_OutputLocation, p_PDFFiles[i].ToString()), Path.Combine(p_OutputLocation, p_PDFFiles[0].ToString()));
                }

                File.Move(Path.Combine(p_OutputLocation, p_PDFFiles[0].ToString()), Path.Combine(p_OutputLocation, p_OriginalFileName+".pdf"));

                for (int i = 1; i < p_PDFFiles.Count; i++)
                {
                    File.Delete(Path.Combine(p_OutputLocation, p_PDFFiles[i].ToString()));
                }
            }
            catch (Exception)
            {
                throw;
            }

        }
        private void DeleteNamedPlot(string p_NamedPlotName, IGTApplication p_oApp)
        {
            for (int i = 0; i < p_oApp.NamedPlots.Count; i++)
            {
                if (p_oApp.NamedPlots[i].Name.Equals(p_NamedPlotName))
                {
                    try
                    {
                        p_oApp.NamedPlots.Remove(p_oApp.NamedPlots[i].Name);
                    }
                    catch (Exception)
                    {
                        
                    }

                    break;
                }
            }
        }
        private string ExportToPDF(string p_destDir, string p_destName, bool p_append, IGTPlotWindow p_outputWindow, out string p_msg)
        {           
            string retVal = string.Empty;
            p_msg = "";

            try
            {
                string destPathName = Path.Combine(p_destDir, p_destName);
                IGTPDFPrinterProperties pdfProps = GTClassFactory.Create<IGTPDFPrinterProperties>();
                pdfProps.EmbedFont = true; 

                if (p_outputWindow.NamedPlot != null)
                {
                    pdfProps.PageHeight = p_outputWindow.NamedPlot.PaperHeight;
                    pdfProps.PageWidth = p_outputWindow.NamedPlot.PaperWidth;
                }
                pdfProps.Orientation = PageOrientationType.Portrait; 
                pdfProps.PageSize = PageSizeValue.Auto;
                pdfProps.Resolution = ResolutionValue.DPI600;

                // Perform export
                IGTExportService svcExport = GTClassFactory.Create<IGTExportService>();
                svcExport.PDFLayersEnabled = false;

                svcExport.SaveAsPDF(destPathName, pdfProps, p_outputWindow, p_append);
                svcExport = null;

                // Close output window
                p_outputWindow.Close();

                retVal = destPathName;
            }
            catch (Exception ex)
            {
                p_msg = ex.Message;
            }

            return retVal;
        }
      
        private void SetAutomaticFields(IGTNamedPlot p_NamedPlot, int p_TotalCount, int p_current_count, IGTApplication p_oApp)
        {
            foreach (IGTPlotRedline item in p_NamedPlot.GetRedlines(GTPlotRedlineCollectionConstants.gtprcTextOnly))
            {
                if ((item.AutomaticTextField == "SHEET m OF n"))
                {
                    ((IGTTextPointGeometry)(item.Geometry)).Text = "SHEET " + p_current_count + " OF " + p_TotalCount;
                }
                else if ((item.AutomaticTextField == "SCALE"))
                {
                    foreach (IGTPlotFrame tmpFrame in p_NamedPlot.Frames)
                    {
                        if (tmpFrame.Type == GTPlotFrameTypeConstants.gtpftMap)
                        {
                           // ((IGTTextPointGeometry)(item.Geometry)).Text = p_NamedPlot.Frames[0].PlotMap.DisplayScale.ToString();
                            ((IGTTextPointGeometry)(item.Geometry)).Text = tmpFrame.PlotMap.DisplayScale.ToString();
                            break;
                        }
                    }
                }
            }

            p_NamedPlot.FieldsQuery = @"select G3E_JOB.WR_NBR ""WR NO"",G3E_JOB.SERVICE_CENTER_C ""SERVICE CENTER"",
                                        G3E_JOB.OFFICE_C ""OFFICE"",
                                        G3E_JOB.DESIGNER_NM || '[' || G3E_JOB.DESIGNER_UID || ']' || '           ' || '(' || SUBSTR(G3E_JOB.DESIGNER_PHONE, 1, 3) || ')' || SUBSTR(G3E_JOB.DESIGNER_PHONE, 4, 3) || '-' || SUBSTR(G3E_JOB.DESIGNER_PHONE, 7, 4)
                                        ""DESIGNER"",G3E_JOB.PROJMGR_NM || '[' || G3E_JOB.PROJMGR_RACFID || ']' || '           ' || '(' || SUBSTR(G3E_JOB.PROJMGR_PHONE, 1, 3) || ')' || SUBSTR(G3E_JOB.PROJMGR_PHONE, 4, 3) || '-' || SUBSTR(G3E_JOB.PROJMGR_PHONE, 7, 4)
                                        ""PROJECT MANAGER"",G3E_JOB.G3E_DESCRIPTION ""WR NAME"",
                                        G3E_JOB.CUSTOMER_NM ""CUSTOMER"",
                                        G3E_JOB.WR_HOUSE_NBR || ' ' || G3E_JOB.WR_HOUSE_NBR_FRACTION || ' ' || G3E_JOB.WR_STREET_LEADING_DIR || ' ' || G3E_JOB.WR_STREET_NM || ' ' || G3E_JOB.WR_STREET_TYPE || ' '
                                        || G3E_JOB.WR_STREET_TRAILING_DIR || ',' || G3E_JOB.WR_TOWN_C ""ADDRESS"",
                                        G3E_JOB.LOCATION ""LOCATION""
                                        from g3e_job where g3e_IDENTIFIER = '" + p_oApp.DataContext.ActiveJob + "'";
        }
        private bool AssignClipGeometryToFrame(IGTApplication oApp, IGTPlotFrame apf, IGTPolygonGeometry clipPG)
        {
            bool bReturn = false;
            apf.Activate();

            // Need to Zoom out a little more as the WorldToPaper seems to fail on pts that are on the edge.
            IGTPoint originPt = GTClassFactory.Create<IGTPoint>(0, 0, 0);
            IGTGeometry prClipBufferPG = ((IGTGeometry)clipPG).CopyGeometryParallel(originPt, 2);

            apf.PlotMap.ZoomArea(prClipBufferPG.Range);
            apf.ClipGeometry = GetClipBoundary(oApp, clipPG);
            apf.IsClipped = true;
            apf.Deactivate();
            oApp.RefreshWindows();
            bReturn = true;
            return bReturn;
        }

        private IGTGeometry GetClipBoundary(IGTApplication oApp, IGTPolygonGeometry clipIn)
        {
            IGTPolygonGeometry prClipPG = GTClassFactory.Create<IGTPolygonGeometry>();

            IGTPoint ipt;
            foreach (IGTPoint pt in clipIn.Points)
            {
                ipt = oApp.ActivePlotWindow.ActiveFrame.PlotMap.WorldToPaper(pt);
                prClipPG.Points.Add(ipt);
            }

            return prClipPG;
        }       
    }
}
