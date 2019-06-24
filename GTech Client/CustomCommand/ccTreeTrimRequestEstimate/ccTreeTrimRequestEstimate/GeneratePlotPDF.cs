// ===================================================
//  Copyright 2017 Intergraph Corp.
//  File Name: GeneratePlotPDF.cs
//
//  Description:    Class Generate the Plot and PDF.
//  Remarks: 
//  Modification History
//  Date                Author                      Comments
//  --------            --------------              --------
//  15/03/2018          Prathyusha                  Created 
// ======================================================
using Intergraph.GTechnology.API;
using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;
using ADODB;

namespace GTechnology.Oncor.CustomAPI
{
    class GeneratePlotPDF
    {
        #region Variables

        private IGTApplication m_oGTApplication;
        bool m_oUserCancelRequest = false;
        string m_oPlotPDFName = null;

        #endregion

        #region Constructor
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="p_application">The current G/Technology application object.</param>
        public GeneratePlotPDF(IGTApplication p_application)
        {
            this.m_oGTApplication = p_application;
        }
        #endregion

        #region Properties
        public bool CancelRequest
        {
            get
            {
                return m_oUserCancelRequest;
            }
        }
        public string GeneratedPlotPDF
        {
            get
            {
                return m_oPlotPDFName;
            }
        }
        #endregion

        #region Methods
        /// <summary>
        /// Method to check of the Vegetation management PlotTemplate in Workspace.
        /// </summary>
        /// <param name="p_vegMngSheetTemp">Name of the copy of the Vegetation management HTML sheet template</param>
        /// <param name="p_newTreeTrimmingfeature">Current placed feature</param>
        public void CreateVegetationManagementPlotTemplateCopy(string p_vegMngSheetTemp, IGTKeyObject p_newTreeTrimmingfeature)
        {
            string plotTemplateName = null;
            Recordset rs = null;
            bool found = false;
            IGTNamedPlot template = null;
            try
            {
                rs = m_oGTApplication.DataContext.OpenRecordset("select PARAM_VALUE from sys_generalparameter where PARAM_NAME='VegMgmtEstimate_Plot'", CursorTypeEnum.adOpenStatic,
                               LockTypeEnum.adLockReadOnly, (int)CommandTypeEnum.adCmdText);
                if (rs != null && rs.RecordCount > 0)
                {
                    rs.MoveFirst();
                    plotTemplateName = Convert.ToString(rs.Fields["PARAM_VALUE"].Value);
                    IGTNamedPlots npcollection = m_oGTApplication.NamedPlots;
                    if (npcollection != null)
                    {
                        if (npcollection.Count > 0)
                        {
                            foreach (IGTNamedPlot np in npcollection)
                            {
                                if (np.Name == plotTemplateName)
                                {
                                    template = np;
                                    found = true;
                                    break;
                                }
                            }
                        }
                    }
                    if (!found)
                    {
                        DialogResult result = MessageBox.Show(m_oGTApplication.ApplicationWindow, "Unable to generate plot; missing template " + plotTemplateName + ". Send anyway? ", "G /Technology", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
                        switch (result)
                        {
                            case DialogResult.OK:
                                {
                                    break;
                                }
                            case DialogResult.Cancel:
                                {
                                    m_oUserCancelRequest = true;
                                    if (!String.IsNullOrEmpty(p_vegMngSheetTemp))
                                    {
                                        File.Delete(p_vegMngSheetTemp);
                                    }
                                    break;
                                }
                        }
                    }
                    else
                    {
                        CreatePlotWindowAndPlotPDF(template, p_newTreeTrimmingfeature);
                    }
                }
            }

            catch (Exception)
            {
                m_oUserCancelRequest = true;
                throw;
            }
        }

        /// <summary>
        /// Method to create a Plot window with given template and exports the plot window to create a PDF.   
        /// </summary>
        /// <param name="p_template">Specified plot template using which need to create a new named plot </param>
        /// <param name="p_newTreeTrimmingfeature">Current placed feature</param>
        private void CreatePlotWindowAndPlotPDF(IGTNamedPlot p_template, IGTKeyObject p_newTreeTrimmingfeature)
        {
            Recordset oPlotRS = null;
            IGTPlotRedlines oRLs = null;
            IGTPlotFrame gTPlotFrame = GTClassFactory.Create<IGTPlotFrame>();
            IGTPlotMap gtPlotMap = GTClassFactory.Create<IGTPlotMap>();
            IGTNamedPlot namedPlot = GTClassFactory.Create<IGTNamedPlot>();
            IGTPlotWindow gTPlotWindow = GTClassFactory.Create<IGTPlotWindow>();
            string plotName = null;
            object autoVal = null;
            try
            {
                plotName = GetPlotName();
                namedPlot = p_template.CopyPlot(plotName);

                gTPlotWindow = m_oGTApplication.NewPlotWindow(namedPlot);
                gTPlotWindow.BackColor = System.Drawing.Color.White;
                gTPlotWindow.Caption = gTPlotWindow.NamedPlot.Name;

                foreach (IGTPlotFrame apf in namedPlot.Frames)
                {
                    if (apf.Type == GTPlotFrameTypeConstants.gtpftMap)
                    {
                        gTPlotFrame = apf;
                    }
                }
                if (gTPlotFrame != null)
                {
                    gtPlotMap = gTPlotFrame.PlotMap;
                    if (!String.IsNullOrEmpty(p_template.FieldsQuery))
                    {
                        gTPlotWindow.NamedPlot.FieldsQuery = p_template.FieldsQuery;
                    }
                    else
                    {
                        gTPlotWindow.NamedPlot.FieldsQuery = "Select WR_NBR,G3E_IDENTIFIER,DESIGNER_UID,G3E_DATECREATED,WR_CREW_HQ_C from g3e_job where g3e_identifier='" + m_oGTApplication.DataContext.ActiveJob + "'";
                    }

                    oPlotRS = m_oGTApplication.DataContext.OpenRecordset(gTPlotWindow.NamedPlot.FieldsQuery, ADODB.CursorTypeEnum.adOpenStatic, ADODB.LockTypeEnum.adLockReadOnly, (int)ADODB.CommandTypeEnum.adCmdText);

                    oRLs = gTPlotWindow.NamedPlot.GetRedlines(GTPlotRedlineCollectionConstants.gtprcTextOnly);

                    foreach (IGTPlotRedline oRL in oRLs)
                    {
                        IGTTextPointGeometry oTPG = (IGTTextPointGeometry)oRL.Geometry.Copy();

                        if (oRL.AutomaticTextSource == GTPlotAutomaticTextSourceConstants.gtpatPlotByQuery)
                        {
                            if (oRL.AutomaticTextField.ToString() == "WR Number")
                            {
                                autoVal = oPlotRS.Fields["WR_NBR"].Value;
                            }
                            else if (oRL.AutomaticTextField.ToString() == "WR Name")
                            {
                                autoVal = oPlotRS.Fields["G3E_IDENTIFIER"].Value;
                            }
                            else if (oRL.AutomaticTextField.ToString() == "Designer Name")
                            {
                                autoVal = oPlotRS.Fields["DESIGNER_UID"].Value;
                            }
                            else if (oRL.AutomaticTextField.ToString() == "Date")
                            {
                                autoVal = oPlotRS.Fields["G3E_DATECREATED"].Value;
                            }
                            else if (oRL.AutomaticTextField.ToString() == "Crew HQ")
                            {
                                autoVal = oPlotRS.Fields["WR_CREW_HQ_C"].Value;
                            }
                            oTPG.Text = (autoVal == null) ? " " : autoVal.ToString();

                            oRL.Geometry = oTPG;
                        }
                    }
                    gTPlotFrame.Activate();
                    gTPlotFrame.PlotMap.ZoomArea(p_newTreeTrimmingfeature.Components.GetComponent(19002).Geometry.Range);
                    ZoomToExtents(gTPlotFrame, p_newTreeTrimmingfeature.Components.GetComponent(19002).Geometry.Range, 1.2);
                    gTPlotFrame.ClipGeometry = WorldToPaper(gtPlotMap, (IGTPolygonGeometry)p_newTreeTrimmingfeature.Components.GetComponent(19002).Geometry);
                    gTPlotFrame.IsClipped = true;
                    gTPlotFrame.Deactivate();
                    m_oGTApplication.RefreshWindows();

                    m_oPlotPDFName = plotName.Replace('/', '_').Replace('\\', '_').Replace(' ', '_');
                    ExportToPDF(Path.GetTempPath(), m_oPlotPDFName, false, gTPlotWindow);
                }
            }
            catch
            {
                throw;
            }
            finally
            {
                oPlotRS = null;
            }
        }
        /// <summary>
        /// Method to return the Plot Name which needs to be created when 'Generate Plot' is Checked in the form.
        /// </summary>
        /// <returns>plotName</returns>
        private string GetPlotName()
        {
            string plotName = null;
            string activeJob = null;
            List<int> plotNames = new List<int>();
            try
            {
                activeJob = m_oGTApplication.DataContext.ActiveJob;
                foreach (IGTNamedPlot np in m_oGTApplication.NamedPlots)
                {
                    if (np.Name.Contains(activeJob + " VegMgmt"))
                    {
                        plotName = np.Name;
                        plotNames.Add(Convert.ToInt32(plotName.Substring(plotName.Length - 1)));
                    }
                }
                if (plotNames.Count > 0)
                {
                    plotNames.Sort();
                    plotName = activeJob + " VegMgmt " + (plotNames[plotNames.Count - 1] + 1);
                }
                else
                {
                    plotName = activeJob + " VegMgmt1";
                }
            }
            catch
            {
                throw;
            }
            finally
            {
                plotNames = null;
            }
            return plotName;
        }

        /// <summary>
        /// Method to which converts the World coordinates of the placed Tree trimming features to Paper coordinates
        /// </summary>
        /// <param name="p_plotMap">Plot Map</param>
        /// <param name="p_worldPolygon">Geometry of the placed feature which needs to be converted to Paper coordinates</param>
        /// <returns></returns>
        private IGTPolygonGeometry WorldToPaper(IGTPlotMap p_plotMap, IGTPolygonGeometry p_worldPolygon)
        {
            IGTPolygonGeometry paperPolygon = GTClassFactory.Create<IGTPolygonGeometry>();
            IGTPoint pt = null;
            IGTPoint firstPT = null;
            for (int count = 0; count < p_worldPolygon.Points.Count; count++)
            {
                IGTPoint worldPoint = p_worldPolygon.GetKeypointPosition(count);
                pt = WorldToPaper(p_plotMap, worldPoint);
                if (count == 0)
                {
                    firstPT = pt;
                }
                paperPolygon.Points.Add(pt);
            }
            paperPolygon.Points.Add(firstPT);
            return paperPolygon;
        }

        /// <summary>
        /// Method to which convert a point to Paper coordinates
        /// </summary>
        /// <param name="p_plotMap">Plot Map</param>
        /// <param name="p_ptWorld">Point which needs to be convereted to Paper coordinates</param>
        /// <returns></returns>
        private IGTPoint WorldToPaper(IGTPlotMap p_plotMap, IGTPoint p_ptWorld)
        {
            try { return p_plotMap.WorldToPaper(p_ptWorld); }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// Method to zoom the plot window to a given extend value. 
        /// </summary>
        /// <param name="p_frame">Plot Frame</param>
        /// <param name="p_worldRange">World Range of the placed feature</param>
        /// <param name="p_buffer">Extend value to which the map to be zoomed</param>
        private void ZoomToExtents(IGTPlotFrame p_frame, IGTWorldRange p_worldRange, double p_buffer)
        {
            IGTWorldRange rangeBuffered = GTClassFactory.Create<IGTWorldRange>();
            IGTPoint ptTemp;

            ptTemp = GTClassFactory.Create<IGTPoint>();
            ptTemp.X = p_worldRange.TopRight.X + p_buffer;
            ptTemp.Y = p_worldRange.TopRight.Y + p_buffer;
            rangeBuffered.TopRight = ptTemp;

            ptTemp = (IGTPoint)GTClassFactory.Create<IGTPoint>();
            ptTemp.X = p_worldRange.BottomLeft.X - p_buffer;
            ptTemp.Y = p_worldRange.BottomLeft.Y - p_buffer;
            rangeBuffered.BottomLeft = ptTemp;

            p_frame.PlotMap.ZoomArea(rangeBuffered);
        }

        /// <summary>
        /// Method to Export the given plot window to PDF 
        /// </summary>
        /// <param name="p_destDir">Directory path of the target PDF filename.</param>
        /// <param name="p_destName">PDF filename</param>
        /// <param name="p_append">Append the results to destFileName, if destFileName already exists; otherwise create/overwrite FileName</param>
        /// <param name="pw">Plot window which needs to be exported to PDF</param>
        private void ExportToPDF(string p_destDir, string p_destName, bool p_append, IGTPlotWindow pw)
        {
            IGTExportService svcExport = null;
            IGTPDFPrinterProperties printProps = null;

            try
            {
                // Construct full path
                string destPathName = Path.Combine(p_destDir, p_destName);

                // Construct printer properties
                PageOrientationType orientation =
                    (pw.NamedPlot.PaperWidth > pw.NamedPlot.PaperHeight)
                    ? PageOrientationType.Portrait : PageOrientationType.Landscape;

                printProps = GTClassFactory.Create<IGTPDFPrinterProperties>();
                printProps.PageWidth = pw.NamedPlot.PaperWidth;
                printProps.PageHeight = pw.NamedPlot.PaperHeight;
                printProps.Orientation = orientation;
                printProps.PageSize = PageSizeValue.Auto;
                printProps.Resolution = ResolutionValue.DPI600;

                // Perform export
                svcExport = GTClassFactory.Create<IGTExportService>();
                svcExport.PDFLayersEnabled = false;
                svcExport.SaveAsPDF(destPathName, printProps, pw, p_append);
            }
            catch
            {
                throw;
            }
            finally
            {
                svcExport = null;
                printProps = null;
                pw = null;
            }

        }
    }
    #endregion
}
