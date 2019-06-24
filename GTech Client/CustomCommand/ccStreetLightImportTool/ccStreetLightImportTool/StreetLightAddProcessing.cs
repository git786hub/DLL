// ===================================================
//  Copyright 2017 Intergraph Corp.
//  File Name: StreetLightRemoveProcessing.cs
//
//  Description: Create StreetLight For TranstionType Add for each record of import worksheet.
//  Remarks: 
//  Modification History
//  Date                Author                      Comments
//  --------            --------------              --------
//  12/04/2018          Sitara                  Created 
// ==========================================================
using System;
using Intergraph.GTechnology.API;
using System.Data;
using System.Text.RegularExpressions;
using Intergraph.CoordSystems;

namespace GTechnology.Oncor.CustomAPI
{
    class StreetLightAddProcessing
    {
        #region Variables

        private IGTDataContext m_gTDataContext;
        private IGTTransactionManager m_gTTransactionManager;
        public int m_nbrSuccess = 0;
        public int m_nbrWarning = 0;
        public int m_nbrError = 0;
        private IGTApplication m_gTApplication = null;
        #endregion

        public StreetLightAddProcessing(IGTDataContext p_dataContext, IGTTransactionManager p_gTTransactionManager ,IGTApplication p_gTApplication)
        {
            m_gTDataContext = p_dataContext;
            m_gTTransactionManager = p_gTTransactionManager;
            m_gTApplication = p_gTApplication;
        }

        /// <summary>
        /// Create streetlights based on table data.
        /// </summary>
        /// <param name="p_xlTable">Excel sheet as Datatable</param>
        /// <returns></returns>
        public DataRow AddStreetLightForTranstionTypeAdd(DataRow p_xlRow)
        {
            StreetLightImportUtility streetLightImportUtility = null;
           
            try
            {

                if (Convert.ToString(p_xlRow["TRANSACTION TYPE"]).ToUpper() == "ADD")
                {
                    if (p_xlRow["TRANSACTION STATUS"] == null ||
                        (Convert.ToString(p_xlRow["TRANSACTION STATUS"]).ToUpper() != "SUCCESS" &&
                        Convert.ToString(p_xlRow["TRANSACTION STATUS"]).ToUpper() != "WARNING"))
                    {
                        if (Convert.ToString(p_xlRow["ONCOR STRUCTURE"]).ToUpper() == "Y")
                        {
                            if (!string.IsNullOrEmpty(Convert.ToString(p_xlRow["ONCOR STRUCTURE ID"])))
                            {
                                p_xlRow = StreetLightStructureIDSY(p_xlRow);
                            }
                            else if (p_xlRow["GPS X"] != null && p_xlRow["GPS Y"] != null
                                && (!string.IsNullOrEmpty(Convert.ToString(p_xlRow["GPS X"]))) &&
                                (!string.IsNullOrEmpty(Convert.ToString(p_xlRow["GPS Y"]))))
                            {
                                p_xlRow = StreetLightGPSSY(p_xlRow);
                            }
                            else if (!string.IsNullOrEmpty(Convert.ToString(p_xlRow["ESI LOCATION"])))
                            {
                                p_xlRow = StreetLightESILocationSY(p_xlRow);
                            }

                        }
                        else if (Convert.ToString(p_xlRow["ONCOR STRUCTURE"]).ToUpper() == "N")
                        {
                            if (p_xlRow["GPS X"] != null && p_xlRow["GPS Y"] != null
                                && (!string.IsNullOrEmpty(Convert.ToString(p_xlRow["GPS X"]))) &&
                                (!string.IsNullOrEmpty(Convert.ToString(p_xlRow["GPS Y"]))))
                            {
                                p_xlRow = StreetLightGPSSN(p_xlRow);
                            }
                            else if (!string.IsNullOrEmpty(Convert.ToString(p_xlRow["ESI LOCATION"])))
                            {
                                streetLightImportUtility = new StreetLightImportUtility(m_gTDataContext, false, m_gTTransactionManager, m_gTApplication);
                                p_xlRow = StreetLightESILocationSN(p_xlRow, streetLightImportUtility);
                            }
                        }

                        if (Convert.ToString(p_xlRow["TRANSACTION STATUS"]) == "SUCCESS")
                        {
                            m_nbrSuccess++;
                        }
                        else if (Convert.ToString(p_xlRow["TRANSACTION STATUS"]) == "ERROR")
                        {
                            m_nbrError++;
                        }
                        else if (Convert.ToString(p_xlRow["TRANSACTION STATUS"]) == "WARNING")
                        {
                            m_nbrWarning++;
                        }

                        p_xlRow["TRANSACTION DATE"] = DateTime.Today;
                    }
                }

            }
            catch
            {
                throw;
            }

            return p_xlRow;
        }

        /// <summary>
        /// Process street light using ESI Location(Structure is No).
        /// </summary>
        /// <param name="p_xlTable">Excel data as datatbale</param>
        /// <param name="streetLightImportUtility">ImportUtility class object</param>
        /// <param name="i"></param>
        private DataRow StreetLightESILocationSN(DataRow p_xlRow, StreetLightImportUtility streetLightImportUtility)
        {
            try
            {
                AddCuAttributes(p_xlRow, streetLightImportUtility);
                int MiscellaneousStructureFID = streetLightImportUtility.GetMiscStructureFID(Convert.ToString(p_xlRow["ESI LOCATION"]));

                if (MiscellaneousStructureFID != 0)
                {
                    streetLightImportUtility.CreateStreetLightWithAccountStructure(MiscellaneousStructureFID);
                    if (string.IsNullOrEmpty(streetLightImportUtility.m_strStatus))
                    {
                        p_xlRow["TRANSACTION STATUS"] = "WARNING";
                        p_xlRow["TRANSACTION COMMENT"] = "No Structure or GPS given.  Placed non-located Street Light at location of account’s structure.";
                    }
                    else
                    {
                        p_xlRow["TRANSACTION STATUS"] = streetLightImportUtility.m_strStatus;
                        p_xlRow["TRANSACTION COMMENT"] = streetLightImportUtility.m_strComment;
                    }
                }
                else
                {
                    streetLightImportUtility.CreateStreetLightWithAccountBoundary(Convert.ToString(p_xlRow["ESI LOCATION"]));
                    if (string.IsNullOrEmpty(streetLightImportUtility.m_strStatus))
                    {
                        p_xlRow["TRANSACTION STATUS"] = "WARNING";
                        p_xlRow["TRANSACTION COMMENT"] = "No Structure or GPS given.  Placed non-located Street Light at centroid of account boundary.";
                    }
                    else
                    {
                        p_xlRow["TRANSACTION STATUS"] = streetLightImportUtility.m_strStatus;
                        p_xlRow["TRANSACTION COMMENT"] = streetLightImportUtility.m_strComment;
                    }
                }
            }
            catch
            {
                throw;
            }

            return p_xlRow;
        }

        /// <summary>
        /// Process street light using GPS Coordinates(Structure is No).
        /// </summary>
        /// <param name="p_xlTable">Excel data as datatbale</param>
        /// <param name="i"></param>
        private DataRow StreetLightGPSSN(DataRow p_xlRow)
        {
            StreetLightImportUtility streetLightImportUtility = new StreetLightImportUtility(m_gTDataContext, true, m_gTTransactionManager, m_gTApplication);
            try
            {
                AddCuAttributes(p_xlRow, streetLightImportUtility);

                string gpsx = Regex.Replace(Convert.ToString(p_xlRow["GPS X"]), "[A-Za-z ]", "");
                double gpsdx = double.Parse(gpsx.Trim());

                string gpsy = Regex.Replace(Convert.ToString(p_xlRow["GPS Y"]), "[A-Za-z ]", "");
                double gpsdy = double.Parse(gpsy.Trim());

                double dZoord = 0.0;

                ConvertToLatitudeAndLongitude(ref gpsdx, ref gpsdy, ref dZoord);

                streetLightImportUtility.CreateStreetLight(gpsdx, gpsdy);


                if (string.IsNullOrEmpty(streetLightImportUtility.m_strStatus))
                {
                    p_xlRow["TRANSACTION STATUS"] = "SUCCESS";
                    p_xlRow["TRANSACTION COMMENT"] = "";
                }
                else
                {
                    p_xlRow["TRANSACTION STATUS"] = streetLightImportUtility.m_strStatus;
                    p_xlRow["TRANSACTION COMMENT"] = streetLightImportUtility.m_strComment;
                }
            }
            catch
            {
                throw;
            }

            return p_xlRow;
        }

        /// <summary>
        /// Process street light using GPS Coordinates(Structure is Yes).
        /// </summary>
        /// <param name="p_xlTable">Excel data as datatbale</param>
        /// <param name="i"></param>
        private DataRow StreetLightESILocationSY(DataRow p_xlRow)
        {
            StreetLightImportUtility streetLightImportUtility = new StreetLightImportUtility(m_gTDataContext, false, m_gTTransactionManager, m_gTApplication);
            try
            {
                if (!string.IsNullOrEmpty(Convert.ToString(p_xlRow["LAMP TYPE"])) &&
    !string.IsNullOrEmpty(Convert.ToString(p_xlRow["WATTAGE"])) &&
    !string.IsNullOrEmpty(Convert.ToString(p_xlRow["LUMINAIRE STYLE"])))
                {
                    streetLightImportUtility.m_strLampType = Convert.ToString(p_xlRow["LAMP TYPE"]);
                    streetLightImportUtility.m_strLumiStyle = Convert.ToString(p_xlRow["LUMINAIRE STYLE"]);
                    streetLightImportUtility.m_strWattage = Convert.ToString(p_xlRow["WATTAGE"]);
                }
                int MiscellaneousStructureFID = streetLightImportUtility.GetMiscStructureFID(Convert.ToString(p_xlRow["ESI LOCATION"]));

                if (MiscellaneousStructureFID != 0)
                {
                    streetLightImportUtility.CreateStreetLightWithAccountStructure(MiscellaneousStructureFID);
                    if (string.IsNullOrEmpty(streetLightImportUtility.m_strStatus))
                    {
                        p_xlRow["TRANSACTION STATUS"] = "WARNING";
                        p_xlRow["TRANSACTION COMMENT"] = "Structure ID not found and no GPS given.  Placed non-located Street Light at location of account’s structure.";
                    }
                    else
                    {
                        p_xlRow["TRANSACTION STATUS"] = streetLightImportUtility.m_strStatus;
                        p_xlRow["TRANSACTION COMMENT"] = streetLightImportUtility.m_strComment;
                    }
                }
                else
                {
                    streetLightImportUtility.CreateStreetLightWithAccountBoundary(Convert.ToString(p_xlRow["ESI LOCATION"]));

                    if (string.IsNullOrEmpty(streetLightImportUtility.m_strStatus))
                    {
                        p_xlRow["TRANSACTION STATUS"] = "WARNING";
                        p_xlRow["TRANSACTION COMMENT"] = "Structure ID not found and no GPS.  Placed non-located Street Light at centroid of account boundary.";
                    }
                    else
                    {
                        p_xlRow["TRANSACTION STATUS"] = streetLightImportUtility.m_strStatus;
                        p_xlRow["TRANSACTION COMMENT"] = streetLightImportUtility.m_strComment;
                    }
                }
            }
            catch
            {
                throw;
            }
            return p_xlRow;
        }

        /// <summary>
        /// Process street light using GPS Coordinates(Structure is Yes).
        /// </summary>
        /// <param name="p_xlTable">Excel data as datatbale</param>
        /// <param name="i"></param>
        private DataRow StreetLightGPSSY(DataRow p_xlRow)
        {
            StreetLightImportUtility streetLightImportUtility = new StreetLightImportUtility(m_gTDataContext, true, m_gTTransactionManager, m_gTApplication);
            try
            {
                AddCuAttributes(p_xlRow, streetLightImportUtility);


                string gpsx = Regex.Replace(Convert.ToString(p_xlRow["GPS X"]), "[A-Za-z ]", "");
                double gpsdx = double.Parse(gpsx.Trim());

                string gpsy = Regex.Replace(Convert.ToString(p_xlRow["GPS Y"]), "[A-Za-z ]", "");
                double gpsdy = double.Parse(gpsy.Trim());
                double dZoord = 0.0;

                ConvertToLatitudeAndLongitude(ref gpsdx, ref gpsdy, ref dZoord);

                streetLightImportUtility.CreateStreetLight(gpsdx, gpsdy);

                if (string.IsNullOrEmpty(streetLightImportUtility.m_strStatus))
                {
                    p_xlRow["TRANSACTION STATUS"] = "WARNING";
                    p_xlRow["TRANSACTION COMMENT"] = "Structure ID not found.Placed Street Light and structure at GPS location.";
                }
                else
                {
                    p_xlRow["TRANSACTION STATUS"] = streetLightImportUtility.m_strStatus;
                    p_xlRow["TRANSACTION COMMENT"] = streetLightImportUtility.m_strComment;
                }
            }
            catch
            {
                throw;
            }

            return p_xlRow;

        }

        /// <summary>
        /// Process street light using Structure Id.(Structure is Yes).
        /// </summary>
        /// <param name="p_xlTable">Excel data as datatbale</param>
        /// <param name="i"></param>
        private DataRow StreetLightStructureIDSY(DataRow p_xlRow)
        {
            StreetLightImportUtility streetLightImportUtility = new StreetLightImportUtility(m_gTDataContext, true, m_gTTransactionManager, m_gTApplication);
            try
            {
                AddCuAttributes(p_xlRow, streetLightImportUtility);
                streetLightImportUtility.CreateStreetLight(Convert.ToString(p_xlRow["ONCOR STRUCTURE ID"]));

                if (string.IsNullOrEmpty(streetLightImportUtility.m_strStatus))
                {
                    p_xlRow["TRANSACTION STATUS"] = "SUCCESS";
                    p_xlRow["TRANSACTION COMMENT"] = "";
                }
                else
                {
                    p_xlRow["TRANSACTION STATUS"] = streetLightImportUtility.m_strStatus;
                    p_xlRow["TRANSACTION COMMENT"] = streetLightImportUtility.m_strComment;
                }
            }
            catch
            {
                throw;
            }

            return p_xlRow;
        }

        /// <summary>
        /// Adding Cu Attributes to ImportUtility class object.
        /// </summary>
        /// <param name="p_xlTable">Excel data as datatbale</param>
        /// <param name="streetLightImportUtility">ImportUtility class object</param>
        /// <param name="i"></param>
        private static void AddCuAttributes(DataRow p_xlRow, StreetLightImportUtility streetLightImportUtility)
        {
            if (!string.IsNullOrEmpty(Convert.ToString(p_xlRow["LAMP TYPE"])) &&
                                                   !string.IsNullOrEmpty(Convert.ToString(p_xlRow["WATTAGE"])) &&
                                                   !string.IsNullOrEmpty(Convert.ToString(p_xlRow["LUMINAIRE STYLE"])))
            {
                streetLightImportUtility.m_strLampType = Convert.ToString(p_xlRow["LAMP TYPE"]);
                streetLightImportUtility.m_strLumiStyle = Convert.ToString(p_xlRow["LUMINAIRE STYLE"]);
                streetLightImportUtility.m_strWattage = Convert.ToString(p_xlRow["WATTAGE"]);
            }
        }

        /// <summary>
        /// Converting lat ,long to system coordinates.
        /// </summary>
        /// <param name="dXoord">X Coordinate</param>
        /// <param name="dYoord">Y Coordinate</param>
        /// <param name="dZoord">Z Coordinate</param>
        private void ConvertToLatitudeAndLongitude(ref double dXoord, ref double dYoord, ref double dZoord)
        {
            try
            {
                ICoordSystemsMgr objCoordSystemsMgr = (ICoordSystemsMgr)m_gTDataContext.CoordSystemsMgr;
                ILinkableTransformation LinkTransform = objCoordSystemsMgr.AltPathDictionary[objCoordSystemsMgr.BaseCoordSystem.GUID].LinkableTransformationDictionary["Server CS"];
                LinkTransform.TransformPoint(Intergraph.CoordSystems.CSPointConstants.cspUOR, 2, Intergraph.CoordSystems.CSPointConstants.cspUOR, 1, ref dXoord, ref dYoord, ref dZoord);

                dXoord = dXoord * 180 / Math.PI;
                dYoord = dYoord * 180 / Math.PI;

            }
            catch (Exception)
            {
                throw;
            }
        }

    }
}
