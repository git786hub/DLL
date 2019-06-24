using ADODB;
using GTechnology.Oncor.CustomAPI.DataAccess;
using GTechnology.Oncor.CustomAPI.Model;
using Intergraph.GTechnology.API;
using System;
using System.Collections.Generic;

namespace GTechnology.Oncor.CustomAPI
{
    public class Highlightpath
    {
        private string displayControlRootNode = "Landbase Feature Data";
        private double averageSymbolSize = 0;
        private double AverageSymbolSize
        {
            get
            {
                Recordset rs = null;
                try
                {
                    if (averageSymbolSize == 0)
                    {
                        rs = application.DataContext.MetadataRecordset("G3E_GENERALPARAMETER_OPTABLE", "g3e_name = 'AverageSymbolSize'");
                        if (rs.RecordCount != 1)
                            averageSymbolSize = 50;
                        else
                        {
                            rs.MoveFirst();
                            averageSymbolSize = double.Parse(rs.Fields["g3e_value"].Value.ToString());
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                finally
                {
                    if (rs != null)
                    {
                        rs.ActiveConnection = null;
                        rs.Close();
                        rs = null;
                    }
                }

                return averageSymbolSize;
            }
        }

        private IGTApplication m_application;
        private IGTApplication application
        {
            get
            {
                if (m_application == null)
                    m_application = GTClassFactory.Create<IGTApplication>();
                return m_application;
            }
        }

        private IGTMapWindow highlightWindowCandidate = null;
        public IGTMapWindow HighlightWindowCandidate
        {
            get { return highlightWindowCandidate; }
            set
            {
                if (HighlightWindowCandidate == null)
                    highlightWindowCandidate = value;

                //when higlihting multiple features we are probably highlighting the entire link, and the user would probably love to land in a geographic window
                else if (highlightWindowCandidate.DetailID != 0 && value.DetailID == 0)
                    highlightWindowCandidate = value;
            }
        }


        /// <summary>
        /// Highlight selected feature on map window
        /// </summary>
        /// <param name="lbmAnalysis"></param>
        public void HighlightSelectedFeatures(LBMAnalysisResults lbmAnalysis)
        {
            try
            {
                RemoveDisplayNode(displayControlRootNode);
                //Add Detected Features to Selected Objects and highlight on map window
                CommonUtil.FitSelectedFeature((short)lbmAnalysis.G3eFno, lbmAnalysis.G3eFid);
              
                IGTSymbology symbologyOverrides = null;
                symbologyOverrides = GTClassFactory.Create<IGTSymbology>();
                symbologyOverrides.Color = 255 * 256 * 256;
                symbologyOverrides.FillColor = 255 * 256;
                symbologyOverrides.Width = 4;

                //Add Detected Polygons to Map Window Display Control and highlight feature with override symbology
                AddFeaturesToGeoMapwindow(lbmAnalysis, symbologyOverrides);
                //Add Detected Polygons to Detail Map Window Display Control and highlight feature with override symbology
                AddFeaturesToDetailMapwindow(lbmAnalysis, symbologyOverrides);
              
                application.RefreshWindows();
            }
            catch (Exception ex)
            {
                if (!ex.Message.Contains("No geometries found")) throw;
            }
        }

        /// <summary>
        /// Add Detected Polygons to Map Window Display Control and highlight feature with override symbology
        /// </summary>
        /// <param name="lbmAnalysis"></param>
        /// <param name="gtSymbologyOverrides"></param>
        private void AddFeaturesToGeoMapwindow(LBMAnalysisResults lbmAnalysis, IGTSymbology gtSymbologyOverrides)
        {
            IGTDisplayService displayService;
            Recordset rs = null;
            string query = null;
            bool validGeometry = false; //check selected polygon has valid geometry
            try
            {
                if (lbmAnalysis.DetectPolygons != null && lbmAnalysis.DetectPolygons.Count > 0)
                {
                    var gtDDCKeyObjs = application.DataContext.GetDDCKeyObjects((short)lbmAnalysis.G3eFno, lbmAnalysis.G3eFid, GTComponentGeometryConstants.gtddcgPrimaryGeographic);
                    if (gtDDCKeyObjs != null && gtDDCKeyObjs.Count > 0)
                    {
                        var gtKeyObj = gtDDCKeyObjs[0];

                        if (gtKeyObj.Geometry != null)
                        {
                            validGeometry = true;
                        }

                    }
                    foreach (KeyValuePair<int, int> feat in lbmAnalysis.DetectPolygons)
                    {
                        gtDDCKeyObjs = application.DataContext.GetDDCKeyObjects((short)feat.Value, feat.Key, GTComponentGeometryConstants.gtddcgPrimaryGeographic);
                        if (gtDDCKeyObjs != null && gtDDCKeyObjs.Count > 0)
                        {
                            var gtKeyObj = gtDDCKeyObjs[0];
                            if (gtKeyObj.Geometry != null)
                            {
                                if (query != null)
                                {
                                    query += " union all ";
                                }
                                query += "select " + gtKeyObj.FNO + " G3E_FNO, " + gtKeyObj.FID + " G3E_FID, " + gtKeyObj.Recordset.Fields["G3E_CNO"].Value.ToString() + " G3E_CNO, " + gtKeyObj.Recordset.Fields["G3E_CID"].Value.ToString() + " G3E_CID  from dual";
                            }
                        }
                    }

                }
                if (!string.IsNullOrEmpty(query))
                {
                    rs = application.DataContext.OpenRecordset(query, CursorTypeEnum.adOpenStatic, LockTypeEnum.adLockReadOnly, 1, new object[] { });
                    if (rs != null && rs.RecordCount > 0 && gtSymbologyOverrides != null)
                    {
                        foreach (IGTMapWindow window in application.GetMapWindows(GTMapWindowTypeConstants.gtapmtGeographic))
                        {
                            // detail windows can have different nominal scale and fitting is done against the active window.Without activating the targetted window we might end up with invalid range
                            if (window.DetailID == 0 && application.ActiveMapWindow.DetailID != 0)
                                window.Activate();

                            displayService = window.DisplayService;
                            if (validGeometry)
                            {
                                displayService.AppendQuery(displayControlRootNode, "Feature_" + lbmAnalysis.G3eFid, rs, gtSymbologyOverrides, true, true, true, true, GTDisplayScaleModeConstants.gtdsdsOff);
                            }
                            if (lbmAnalysis.DetectPolygons != null && lbmAnalysis.DetectPolygons.Count > 0)
                            {
                                foreach (KeyValuePair<int, int> polygon in lbmAnalysis.DetectPolygons)
                                {
                                    if (query.Contains(polygon.Key.ToString()))
                                    {
                                        displayService.AppendQuery(displayControlRootNode, "Feature_" + polygon.Key, rs, gtSymbologyOverrides, true, true, true, true, GTDisplayScaleModeConstants.gtdsdsOff);
                                    }
                                }
                            }
                            if (validGeometry)
                            {
                                IGTWorldRange range = displayService.GetRange(displayControlRootNode, "Feature_" + lbmAnalysis.G3eFid);
                                ExtendFollowingAutoFitRules(window, range);
                                window.ZoomArea(range);
                            }

                        }
                    }
                }
            }
            catch (Exception )
            {
                throw;
            }
            finally
            {
                displayService = null;

                if (rs != null)
                {
                    rs.ActiveConnection = null;
                    rs.Close();
                    rs = null;
                }
            }
        }

        /// <summary>
        /// //Add Detected Polygons to Detail Map Window Display Control and highlight feature with override symbology
        /// </summary>
        /// <param name="lbmAnalysis"></param>
        /// <param name="gtSymbologyOverrides"></param>
        private void AddFeaturesToDetailMapwindow(LBMAnalysisResults lbmAnalysis, IGTSymbology gtSymbologyOverrides)
        {
            IGTDisplayService displayService;
            Recordset rs = null;
            string query = null;
            bool validGeometry = false; //check selected polygon has valid geometry
            try
            {
                if (lbmAnalysis.DetectPolygons != null && lbmAnalysis.DetectPolygons.Count > 0)
                {
                    var gtDDCKeyObjs = application.DataContext.GetDDCKeyObjects((short)lbmAnalysis.G3eFno, lbmAnalysis.G3eFid, GTComponentGeometryConstants.gtddcgPrimaryDetail);
                    if (gtDDCKeyObjs != null && gtDDCKeyObjs.Count > 0)
                    {
                        var gtKeyObj = gtDDCKeyObjs[0];
                        if (gtKeyObj.Geometry != null)
                        {
                            validGeometry = true;
                        }
                    }
                    foreach (KeyValuePair<int, int> feat in lbmAnalysis.DetectPolygons)
                    {
                        gtDDCKeyObjs = application.DataContext.GetDDCKeyObjects((short)feat.Value, feat.Key, GTComponentGeometryConstants.gtddcgPrimaryDetail);
                        if (gtDDCKeyObjs != null && gtDDCKeyObjs.Count > 0)
                        {
                            var gtKeyObj = gtDDCKeyObjs[0];
                            if (gtKeyObj.Geometry != null)
                            {
                                if (query != null)
                                {
                                    query += " union all ";
                                }
                                query += "select " + gtKeyObj.FNO + " G3E_FNO, " + gtKeyObj.FID + " G3E_FID, " + gtKeyObj.Recordset.Fields["G3E_CNO"].Value.ToString() + " G3E_CNO, " + gtKeyObj.Recordset.Fields["G3E_CID"].Value.ToString() + " G3E_CID  from dual";
                            }
                        }
                    }

                }
                if (!string.IsNullOrEmpty(query))
                {
                    rs = application.DataContext.OpenRecordset(query, CursorTypeEnum.adOpenStatic, LockTypeEnum.adLockReadOnly, 1, new object[] { });
                    if (rs != null && rs.RecordCount > 0 && gtSymbologyOverrides != null)
                    {
                        foreach (IGTMapWindow window in application.GetMapWindows(GTMapWindowTypeConstants.gtapmtDetail))
                        {
                            // detail windows can have different nominal scale and fitting is done against the active window.Without activating the targetted window we might end up with invalid range
                            displayService = window.DisplayService;
                            if (validGeometry)
                            {
                                displayService.AppendQuery(displayControlRootNode, "Feature_" + lbmAnalysis.G3eFid, rs, gtSymbologyOverrides, true, true, true, true, GTDisplayScaleModeConstants.gtdsdsOff);
                            }
                            if (lbmAnalysis.DetectPolygons != null && lbmAnalysis.DetectPolygons.Count > 0)
                            {
                                foreach (KeyValuePair<int, int> polygon in lbmAnalysis.DetectPolygons)
                                {
                                    if (query.Contains(polygon.Key.ToString()))
                                    {
                                        displayService.AppendQuery(displayControlRootNode, "Feature_" + polygon.Key, rs, gtSymbologyOverrides, true, true, true, true, GTDisplayScaleModeConstants.gtdsdsOff);
                                    }
                                }
                            }
                            if (validGeometry)
                            {
                                IGTWorldRange range = displayService.GetRange(displayControlRootNode, "Feature_" + lbmAnalysis.G3eFid);
                                ExtendFollowingAutoFitRules(window, range);
                                window.ZoomArea(range);
                            }

                        }
                    }
                }
            }
            catch (Exception )
            {
                throw;
            }
            finally
            {
                displayService = null;

                if (rs != null)
                {
                    rs.ActiveConnection = null;
                    rs.Close();
                    rs = null;
                }
            }
        }

        /// <summary>
        /// Remove node from Diaplay control
        /// </summary>
        /// <param name="displayNodeName"></param>
        public void RemoveDisplayNode(string displayNodeName)
        {
            IGTDisplayControlNodes gtDisplayNodes = null;
            try
            {
                foreach (IGTMapWindow window in application.GetMapWindows(GTMapWindowTypeConstants.gtapmtAll))
                {

                    gtDisplayNodes = window.DisplayService.GetDisplayControlNodes(displayNodeName);
                    if (gtDisplayNodes != null && gtDisplayNodes.Count > 0)
                    {
                        foreach (IGTDisplayControlNode node in gtDisplayNodes)
                        {
                            try
                            {
                                if (node.DisplayName != displayControlRootNode)
                                {
                                    window.DisplayService.Remove(displayControlRootNode, node.DisplayName);
                                }
                            }
                            catch { }
                        }
                        window.HighlightedObjects.Clear();
                        application.RefreshWindows();
                    }
                }
            }
            catch (Exception) { }
        }
        private void ExtendFollowingAutoFitRules(IGTMapWindow window, IGTWorldRange range)
        {
            try
            {
                double extentX = range.TopRight.X - range.BottomLeft.X;
                double extentY = range.TopRight.Y - range.BottomLeft.Y;
                double extendBY;

                if (extentY < 0 || extentX < 0)
                    return;
                if (extentY < 1 && extentX < 1)
                    extendBY = AverageSymbolSize * 4;
                else
                {
                    extendBY = Math.Max(extentX, extentY) * (window.SelectBehaviorZoomFactor - 1);
                }

                IGTPoint tr = GTClassFactory.Create<IGTPoint>(range.TopRight.X + extendBY, range.TopRight.Y + extendBY, 0);
                IGTPoint bl = GTClassFactory.Create<IGTPoint>(range.BottomLeft.X - extendBY, range.BottomLeft.Y - extendBY, 0);

                range.BottomLeft = bl;
                range.TopRight = tr;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

    }
}
