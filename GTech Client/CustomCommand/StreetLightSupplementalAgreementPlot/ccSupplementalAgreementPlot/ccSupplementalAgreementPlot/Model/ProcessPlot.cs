using System;
using System.Collections.Generic;
using System.Windows.Forms;
using Intergraph.GTechnology.API;
using ADODB;

namespace GTechnology.Oncor.CustomAPI.Model
{
    public class ProcessPlot
    {
        IGTApplication m_gTApplication = null;
        IGTNamedPlot m_gTNamedPlot = null;
        string[] m_SelectedEsiLocations;
        public ProcessPlot(IGTApplication gTApplication, IGTNamedPlot gTNamedPlot, string[] SelectedEsiLocations)
        {
            m_gTApplication = gTApplication;
            m_gTNamedPlot = gTNamedPlot;
            m_SelectedEsiLocations = SelectedEsiLocations;
        }
        private bool IsPlotNameExist(string strUserText)
        {
            IGTNamedPlots gTNamedPlots = null;
            try
            {
                gTNamedPlots = m_gTApplication.NamedPlots;
                if (gTNamedPlots != null && gTNamedPlots.Count > 0)
                {
                    foreach (IGTNamedPlot npt in gTNamedPlots)
                    {
                        if (npt.Name == strUserText)
                        {
                            return true;
                        }
                    }
                }
            }
            catch
            {
                throw;
            }
            return false;
        }
        public string GetAvailablePlotName()
        {
            IGTNamedPlots gTNamedPlots = null;
            string strPlot = "";
            string[] strAvailableName = new string[2];
            int plotNumber = 0;
            try
            {
                gTNamedPlots = m_gTApplication.NamedPlots;
                if (gTNamedPlots != null && gTNamedPlots.Count > 0)
                {
                    foreach (IGTNamedPlot npt in gTNamedPlots)
                    {
                        if (npt.Name.Contains("Supplemental Plot") && npt.Name.Contains("-"))
                        {
                            strAvailableName = npt.Name.Split('-');

                            if (strAvailableName[1] != null && Convert.ToInt32(strAvailableName[1]) > plotNumber)
                            {
                                plotNumber = Convert.ToInt32(strAvailableName[1]);
                            }
                        }                       
                    }


                    if (plotNumber == 0)
                    {
                        strPlot = "Supplemental Plot - 1";
                    }
                    else if (plotNumber > 0)
                    {
                        plotNumber = plotNumber + 1;
                        strPlot = "Supplemental Plot" + " - " + plotNumber;
                    }
                }
            }
            catch
            {
                throw;
            }
            return strPlot;
        }
        public bool GeneratePlot(string strPlot, string strCity, string strDivision)
        {
            IGTNamedPlot gTNewNamedPlot = GTClassFactory.Create<IGTNamedPlot>();
            IGTPlotFrame gTNewPlotFrame = GTClassFactory.Create<IGTPlotFrame>();
            IGTPlotWindow gTPlotWindow = GTClassFactory.Create<IGTPlotWindow>();
            IGTPlotMap gtNewPlotMap = GTClassFactory.Create<IGTPlotMap>();
            IGTPlotRedlines gTPlotRedlines = null;
            //Recordset rs = null;
            object objectValue = null;
            string strNewPlotName = "";
            try
            {
                if (!IsPlotNameExist(strPlot))
                {
                    strNewPlotName = strPlot;
                    if (m_gTNamedPlot != null && !string.IsNullOrEmpty(strNewPlotName))
                    {
                        gTNewNamedPlot = m_gTNamedPlot.CopyPlot(strNewPlotName);
                        gTPlotWindow = m_gTApplication.NewPlotWindow(gTNewNamedPlot);
                        gTPlotWindow.BackColor = System.Drawing.Color.White;
                        gTPlotWindow.Caption = gTPlotWindow.NamedPlot.Name;

                        if (!String.IsNullOrEmpty(m_gTNamedPlot.FieldsQuery))
                        {
                            gTPlotWindow.NamedPlot.FieldsQuery = m_gTNamedPlot.FieldsQuery;
                        }
                        


                        //rs = gTDataContext.OpenRecordset(gTPlotWindow.NamedPlot.FieldsQuery, ADODB.CursorTypeEnum.adOpenStatic, ADODB.LockTypeEnum.adLockReadOnly,
                        // (int)ADODB.CommandTypeEnum.adCmdText, gTDataContext.ActiveJob);

                        gTPlotRedlines = gTPlotWindow.NamedPlot.GetRedlines(GTPlotRedlineCollectionConstants.gtprcTextOnly);

                        foreach (IGTPlotRedline prl in gTPlotRedlines)
                        {
                            IGTTextPointGeometry gTTextPointGeometry = (IGTTextPointGeometry)prl.Geometry.Copy();

                            if (prl.AutomaticTextSource == GTPlotAutomaticTextSourceConstants.gtpatPlotByQuery)
                            {
                                string currentVal = String.IsNullOrEmpty(gTTextPointGeometry.Text) ? " " : gTTextPointGeometry.Text;
                                

                                if (prl.AutomaticTextField.ToString() == "WR")
                                {
                                    objectValue = m_gTApplication.DataContext.ActiveJob;
                                }
                                else if (prl.AutomaticTextField.ToString() == "StreetLightESILocation")
                                {                                   
                                    string esiLocations = "";                                    
                                    foreach (string esiL in m_SelectedEsiLocations)
                                    {
                                        if (!string.IsNullOrEmpty(esiLocations))
                                        {
                                            esiLocations = esiLocations + ",'" + esiL + "'";
                                        }
                                        else
                                        {
                                            esiLocations = "'" + esiL + "'";
                                        }
                                    }

                                    objectValue = esiLocations;
                                }
                                else if (prl.AutomaticTextField.ToString() == "CountofStreetLightsinselectset")
                                {
                                    objectValue = m_SelectedEsiLocations.Length + " Street Lights";
                                }
                                else if (prl.AutomaticTextField.ToString() == "Subdivision")
                                {
                                    objectValue = strDivision;
                                }
                                else if (prl.AutomaticTextField.ToString() == "City")
                                {
                                    objectValue = strCity;
                                }
                                gTTextPointGeometry.Text = (objectValue == null) ? " " : objectValue.ToString();

                                prl.Geometry = gTTextPointGeometry;
                            }
                        }

                        foreach (IGTPlotFrame plf in gTNewNamedPlot.Frames)
                        {
                            if (plf.Type == GTPlotFrameTypeConstants.gtpftMap)
                            {
                                gTNewPlotFrame = plf;
                            }
                        }

                        if (gTNewPlotFrame != null)
                        {
                            gtNewPlotMap = gTNewPlotFrame.PlotMap;
                            gTNewPlotFrame.Activate();
                            gTNewPlotFrame.PlotMap.FitSelectedObjects(1.2);
                            gTNewPlotFrame.Deactivate();
                            m_gTApplication.RefreshWindows();
                            return true;
                        }
                    }
                }
                else
                {
                    MessageBox.Show("Plot name already exists in workspace.  Please enter a different plot name", "G/Technology", MessageBoxButtons.OK, MessageBoxIcon.Information,
                        MessageBoxDefaultButton.Button1);

                    return false;
                }
            }
            catch
            {
                throw;
            }
            return false;
        }
    }
}
