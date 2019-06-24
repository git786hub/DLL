using System;
using System.Collections.Generic;
using Intergraph.GTechnology.Interfaces;
using Intergraph.CoordSystems;
using Intergraph.GTechnology.API;
using ADODB;
using System.Data;
using System.Data.OleDb;
using System.Linq;
using System.Windows.Forms;

namespace GTechnology.Oncor.CustomAPI
{
    public class ccFieldActivityErrorResolution : IGTCustomCommandModeless
    {
        public bool CanTerminate
        {
            get
            {
                //temporary implementation.
                return true;
            }
        }

        public IGTTransactionManager TransactionManager
        {
            set
            {
                dataProvider.transactionManager = value;
            }
        }

        public void Activate(IGTCustomCommandHelper CustomCommandHelper)
        {
            FieldActivityErrorResolutionDialog dialogWindow = new FieldActivityErrorResolutionDialog();
            dataProvider.ccHelper = CustomCommandHelper;
            dataProvider.LoadData();
            if (dataProvider.dtList == null)
            {
                MessageBox.Show("Field Activity Error Resolution command - No errors to display ", "G/Technology", MessageBoxButtons.OK, MessageBoxIcon.Information);
                dataProvider.ccHelper.Complete();
                dataProvider.dispose();
                return;
            }
            dialogWindow.ShowDialog();
        }

        public void Pause()
        {
        }

        public void Resume()
        {
        }

        public void Terminate()
        {
        }
    }

    /// <summary>
    /// This class is the work horse for the dialog's data
    /// </summary>
    internal static class dataProvider
    {
        public static IGTApplication gtApp = GTClassFactory.Create<IGTApplication>();
        public static IGTTransactionManager transactionManager;
        public static IGTCustomCommandHelper ccHelper;
        public static IGTDataContext gtDataContext = gtApp.DataContext;
        public static IGTGeometryEditService gtGeoEditService = GTClassFactory.Create<IGTGeometryEditService>();
        public static string ErrorMessage;
        public static Recordset fieldActivityErrorRecords = new Recordset();
        public static Recordset selectedFeatures = new Recordset();
        static Recordset metaDataRecords = new Recordset();
        public static Dictionary<string, List<string>> ColumnRules = new Dictionary<string, List<string>>();
        static string[] requiredColumnsSetup = { "Status", "StructureID" };
        public static readonly List<string> requiredColumns = new List<string>(requiredColumnsSetup);
        static OleDbDataAdapter odaListMaker;
        public static DataTable dtList;
        public static DataTable locateFeatures;

        public enum MetaValue { DATA_TYPE, DATA_LEGNTH, DATA_PRECISION, DATA_SCALE };

        /// <summary>
        /// Loads the data from the db.
        /// </summary>
        public static void LoadData()
        {
            try
            {
                string metaDataQuery = "SELECT COLUMN_NAME, DATA_TYPE, DATA_LENGTH, DATA_PRECISION, DATA_SCALE FROM ALL_TAB_COLUMNS  WHERE TABLE_NAME = 'SERVICE_ACTIVITY'";
                metaDataRecords = gtDataContext.OpenRecordset(metaDataQuery, CursorTypeEnum.adOpenDynamic, LockTypeEnum.adLockOptimistic, -1);
                metaDataRecords.MoveFirst();
                //set up dictionary to programatically restrict how our columns are allowed to handle data. Not as important with so few editables, but i may make a library of some
                //sort with this set up style. its useful for editable grids.
                while (!metaDataRecords.EOF && metaDataRecords.RecordCount > 0)
                {
                    string[] values = {metaDataRecords.Fields["DATA_TYPE"].Value.ToString(),
                    metaDataRecords.Fields["DATA_LENGTH"].Value.ToString(),
                    metaDataRecords.Fields["DATA_PRECISION"].Value.ToString(),
                    metaDataRecords.Fields["DATA_SCALE"].Value.ToString()};
                    if (!ColumnRules.ContainsKey(metaDataRecords.Fields["COLUMN_NAME"].Value.ToString()))
                        ColumnRules.Add(metaDataRecords.Fields["COLUMN_NAME"].Value.ToString(), new List<string>(values));
                    metaDataRecords.MoveNext();
                }
                string fieldActivityErrorQuery = @"SELECT ADDRESS,
                                                          ADDR_LEAD_DIR_IND,
                                                          ADDR_TRAIL_DIR_IND,
                                                          CORRECTION_COMMENTS,
                                                          CU_ID,
                                                          DELETED_BY_ID,
                                                          DELETED_DATE,
                                                          DWELL_TYPE_C,
                                                          EDIT_DATE,
                                                          ESI_LOCATION,
                                                          EXIST_PREM_GANGBASE,
                                                          FEATURE_TYPE_CODE,
                                                          GIS_LOCATE_METHOD,
                                                          HOUSE_NO,
                                                          HOUSE_NO_FRACTION,
                                                          INSTL_VINT_YEAR,
                                                          INVALID_METER_GEOCODE,
                                                          METER_LATITUDE,
                                                          METER_LONGITUDE,
                                                          MGMT_ACTIVITY_CODE,
                                                          MSG_T,
                                                          OVERRIDE_TOLERANCE,
                                                          NVL(O_OR_U_CODE, '*') O_OR_U_CODE,
                                                          REMARKS_MOBILE,
                                                          RMV_VINT_YEAR,
                                                          SERVICE_ACTIVITY_ID,
                                                          SERVICE_CENTER_CODE,
                                                          NVL(SERVICE_INFO_CODE, '*') SERVICE_INFO_CODE,
                                                          SERVICE_ORDER_NO,
                                                          STATUS_C,
                                                          STREET_NAME,
                                                          STREET_TYPE,
                                                          SVC_STRUCTURE_ID,
                                                          TOWN_NM,
                                                          TRANS_DATE,
                                                          TRF_CO_H,
                                                          TRF_STRUCTURE_ID,
                                                          UNIT_H,
                                                          USER_ID,
                                                          WMIS_POSTED_DATE FROM SERVICE_ACTIVITY WHERE STATUS_C IN ('FAILED', 'QUEUED', 'DELETED')";
                fieldActivityErrorRecords = gtDataContext.OpenRecordset(fieldActivityErrorQuery, CursorTypeEnum.adOpenDynamic, LockTypeEnum.adLockOptimistic, -1);
                if (fieldActivityErrorRecords.EOF && fieldActivityErrorRecords.BOF)
                {
                    ErrorMessage = "No Errors found.";
                }
                else
                {
                    //trying to deal with the datamanagement that is "outside" the dialog here for consistancy, if it doesnt work move it to the _load function in the dialog cs
                    fieldActivityErrorRecords.MoveFirst();
                    fieldActivityErrorRecords.MoveLast();
                    odaListMaker = new OleDbDataAdapter();
                    dtList = new DataTable();
                    odaListMaker.Fill(dtList, fieldActivityErrorRecords);
                    DataColumn[] PrimaryKeyColumns = new DataColumn[1];
                    PrimaryKeyColumns[0] = dtList.Columns["SERVICE_ACTIVITY_ID"];
                    dtList.PrimaryKey = PrimaryKeyColumns;
                    dtList.Columns.Add("EDITED_CELLS", typeof(string));
                    dtList.Columns.Add("STRUCT_ID", typeof(string));
                    foreach (DataRow row in dtList.Rows)
                    {
                        //row.SetField<string>("STRUCT_ID", row.Field<string>("FLNX_H") + "-" + row.Field<string>("FLNY_H"));
                        row.SetField<string>("STRUCT_ID", row.Field<string>("SVC_STRUCTURE_ID"));
                        row["TRANS_DATE"] = DateTime.Parse((row["TRANS_DATE"].ToString())).ToString("MM/dd/yyyy");
                    }
                }
            }
            catch (Exception error)
            {
                MessageBox.Show("Error in LoadData (" + error.Message + ")", "G/Technology");
            }
        }

        /// <summary>
        /// Locates the error based on the address, using the Premise_n table.
        /// </summary>
        /// <param name="houseNumber"></param>
        /// <param name="streetName"></param>
        /// <param name="streetType"></param>
        /// <param name="houseNumberFraction"></param>
        /// <param name="direction"></param>
        /// <param name="directionTrailing"></param>
        /// <param name="unit"></param>
        public static void LocateByAddress(int? houseNumber, string streetName, string streetType, string houseNumberFraction, string direction, string directionTrailing, string unit)
        {
            string selectStatement = "SELECT G3E_FID, G3E_FNO FROM PREMISE_N WHERE";
            int nullCheck = selectStatement.Count();
            if (houseNumber != null)
            {
                selectStatement += " HOUSE_NBR = '" + houseNumber + "'";
            }
            if (streetName != "")
            {
                selectStatement += "AND STREET_NM = '" + streetName + "'";
            }
            if (streetType != "")
            {
                selectStatement += "AND STREET_TYPE_C = '" + streetType + "'";
            }
            if (houseNumberFraction != "")
            {
                selectStatement += "AND HOUSE_FRACTION_NBR = " + houseNumberFraction;
            }
            if (direction != "")
            {
                selectStatement += "AND DIR_LEADING_C = '" + direction + "'";
            }
            if (directionTrailing != "")
            {
                selectStatement += "AND DIR_TRAILING_C = '" + directionTrailing + "'";
            }
            if (unit != "")
            {
                selectStatement += "AND UNIT_NBR = " + unit;
            }
            if (selectStatement.Count() == nullCheck)
            {
                MessageBox.Show("No address data found in record.", "G/Technology");
            }
            else
            {
                Recordset featureID = gtDataContext.OpenRecordset(selectStatement, CursorTypeEnum.adOpenDynamic, LockTypeEnum.adLockOptimistic, -1);
                locateFeatures = null;
                if (featureID.EOF && featureID.BOF)
                {
                    MessageBox.Show("No features found with a matching address.", "G/Technology");
                }
                else
                {
                    featureID.MoveFirst();
                    featureID.MoveLast();
                    odaListMaker = new OleDbDataAdapter();
                    locateFeatures = new DataTable();
                    odaListMaker.Fill(locateFeatures, featureID);
                    if (!featureID.EOF && featureID.RecordCount > 0)
                    {
                        short FNO = Int16.Parse(featureID.Fields["G3E_FNO"].Value.ToString());
                        int FID = Int32.Parse(featureID.Fields["G3E_FID"].Value.ToString());
                        locate(FID, FNO);
                    }
                }
            }
        }

        /// <summary>
        /// Goes to the location of the error based on the Premise Number, not super testable atm.
        /// </summary>
        /// <param name="premiseNumber"></param>
        public static void LocateByPremiseNumber(string premiseNumber)
        {
            if (premiseNumber.Length > 0)
            {
                string selectStatement = "SELECT G3E_FID, G3E_FNO FROM PREMISE_N WHERE PREMISE_NBR = ?";
                int nullCheck = selectStatement.Count();
                Recordset featureID = gtDataContext.OpenRecordset(selectStatement, CursorTypeEnum.adOpenDynamic, LockTypeEnum.adLockOptimistic, -1, premiseNumber);
                locateFeatures = null;
                if (featureID.EOF && featureID.BOF)
                {
                    MessageBox.Show("No features found with a matching Premise Number.", "G/Technology");
                }
                else
                {
                    featureID.MoveFirst();
                    featureID.MoveLast();
                    odaListMaker = new OleDbDataAdapter();
                    locateFeatures = new DataTable();
                    odaListMaker.Fill(locateFeatures, featureID);
                    if (!featureID.EOF && featureID.RecordCount > 0)
                    {
                        short FNO = Int16.Parse(featureID.Fields["G3E_FNO"].Value.ToString());
                        int FID = Int32.Parse(featureID.Fields["G3E_FID"].Value.ToString());
                        locate(FID, FNO);
                    }
                }
            }
            else
            {
                MessageBox.Show("Record has no Premise Number", "G/Technology");
            }
        }

        /// <summary>
        /// Locates a feature based on the structure ID in the record. 
        /// </summary>
        /// <param name="structID"></param>
        public static void LocateByStructID(string structID)
        {
            string selectStatement = "SELECT G3E_FID, G3E_FNO FROM COMMON_N WHERE STRUCTURE_ID = ?";
            Recordset featureID = gtDataContext.OpenRecordset(selectStatement, CursorTypeEnum.adOpenDynamic, LockTypeEnum.adLockOptimistic, -1, structID);
            locateFeatures = null;
            if (featureID.EOF && featureID.BOF)
            {
                MessageBox.Show("No features found with a matching Structure ID.", "G/Technology");
            }
            else
            {

                featureID.MoveFirst();
                featureID.MoveLast();
                odaListMaker = new OleDbDataAdapter();
                locateFeatures = new DataTable();
                odaListMaker.Fill(locateFeatures, featureID);
                if (!featureID.EOF && featureID.RecordCount > 0)
                {
                    short FNO = Int16.Parse(featureID.Fields["G3E_FNO"].Value.ToString());
                    int FID = Int32.Parse(featureID.Fields["G3E_FID"].Value.ToString());
                    locate(FID, FNO);
                }
            }
        }

        /// <summary>
        /// Locates feaures based on the Transformer Company Number value of the record.
        /// </summary>
        /// <param name="transID"></param>
        public static void LocateByTransformerID(string transID)
        {
            string selectStatement = "SELECT DISTINCT G3E_FID, G3E_FNO FROM XFMR_UG_UNIT_N WHERE COMPANY_ID = ? " +
                                     "union " +
                                     "SELECT DISTINCT G3E_FID, G3E_FNO FROM XFMR_OH_UNIT_N WHERE COMPANY_ID = ?";
            Recordset featureID = gtDataContext.OpenRecordset(selectStatement, CursorTypeEnum.adOpenDynamic, LockTypeEnum.adLockOptimistic, -1, transID, transID);
            locateFeatures = null;
            if (featureID.EOF && featureID.BOF)
            {
                MessageBox.Show("No Transformer found for Transformer Company ID", "G/Technology");
            }
            else
            {
                featureID.MoveFirst();
                featureID.MoveLast();
                odaListMaker = new OleDbDataAdapter();
                locateFeatures = new DataTable();
                odaListMaker.Fill(locateFeatures, featureID);
                if (!featureID.EOF && featureID.RecordCount > 0)
                {
                    short FNO = Int16.Parse(featureID.Fields["G3E_FNO"].Value.ToString());
                    int FID = Int32.Parse(featureID.Fields["G3E_FID"].Value.ToString());
                    locate(FID, FNO);
                }
            }
        }

        /// <summary>
        /// Support method for Geocode locate. May be removed at a later date.
        /// </summary>
        /// <param name="dX"></param>
        /// <param name="dY"></param>
        /// <param name="dZ"></param>
        /// <returns></returns>
        internal static IGTPoint gCoordinateConvert(double dX, double dY, double dZ)
        {
            //CoordSystem coordsystem;
            //IDictionary<string, IAltCoordSystemPath> myAltPathDict;
            IGTPoint point = GTClassFactory.Create<IGTPoint>();
            //ICoordSystemsMgr coordMgr;
            //IAltCoordSystemPath tmpPath;
            const double degToRad = Math.PI / 180.0;

            try
            {
                //// convert LAT LONG to State Plane
                //coordMgr = gtDataContext.CoordSystemsMgr;
                //System.Diagnostics.Debug.Print("Lon :" + dX.ToString() + " Lat :" + dY.ToString());

                ////Convert to radians
                //dX = dX * degToRad;
                //dY = dY * degToRad;

                //coordsystem = coordMgr.BaseCoordSystem as CoordSystem;
                //myAltPathDict = coordMgr.AltPathDictionary;
                //myAltPathDict.TryGetValue("{720E36F0-5586-4960-84B1-4544BA6123B3}", out tmpPath);
                //// Coodinate systems in database.

                //((ILinkableTransformation)coordsystem).TransformPoint(CSPointConstants.cspLLU, 1,
                //                                                      CSPointConstants.cspENU, 1, //CSPointConstants.cspENO
                //                                                      ref dX, ref dY, ref dZ);

                //point.X = dX;
                //point.Y = dY;
                //point.Z = 0.0;

                //System.Diagnostics.Debug.Print("X :" + dX.ToString() + " Y :" + dY.ToString());
                //coordMgr = null;
                //coordsystem = null;

                IGTHelperService helperSrvc = GTClassFactory.Create<IGTHelperService>();
                helperSrvc.DataContext = gtDataContext;
                point.X = dX * degToRad;
                point.Y = dY * degToRad;
                point = helperSrvc.GeographicPointToStorage(point);
                helperSrvc = null;

                return point;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "gCoordinateConvert - Error", MessageBoxButtons.OK);
                point.X = 0.0;
                point.Y = 0.0;
                return point;
            }
        }

        /// <summary>
        /// Support method for Geocode locate. May be removed at a later date.
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        internal static void geoLocate(double x, double y)
        {
            gtGeoEditService.TargetMapWindow = gtApp.ActiveMapWindow;
            gtGeoEditService.RemoveAllGeometries();
            gtApp.ActiveMapWindow.DisplayScale = 250;
            IGTPoint locationPoint = gCoordinateConvert(x, y, 0);
            IGTWorldRange gtWorldRange = gtApp.ActiveMapWindow.GetRange();
            double xOffset = (gtWorldRange.TopRight.X - gtWorldRange.BottomLeft.X) / 2;
            double yOffset = (gtWorldRange.TopRight.Y - gtWorldRange.BottomLeft.Y) / 2;

            IGTPoint tempPt = GTClassFactory.Create<IGTPoint>();

            tempPt.X = 0;
            tempPt.Y = 0;
            tempPt.X = locationPoint.X + xOffset;
            tempPt.Y = locationPoint.Y + yOffset;
            gtWorldRange.TopRight = tempPt;

            tempPt.X = 0;
            tempPt.Y = 0;
            tempPt.X = locationPoint.X - xOffset;
            tempPt.Y = locationPoint.Y - yOffset;
            gtWorldRange.BottomLeft = tempPt;

            gtGeoEditService.TargetMapWindow = gtApp.ActiveMapWindow;
            IGTPointGeometry newSymbol = GTClassFactory.Create<IGTPointGeometry>();
            newSymbol.Origin = locationPoint;
            gtGeoEditService.AddGeometry(newSymbol, Convert.ToInt16(GTStyleIDConstants.gtstyleHandleSquareX));
            gtApp.ActiveMapWindow.DisplayScale = 250;

            gtApp.ActiveMapWindow.ZoomArea(gtWorldRange);
            gtApp.RefreshWindows();
        }

        /// <summary>
        /// Submits the changes the user has made to the rows to the db.
        /// </summary>
        /// <param name="rowsChanged"></param>
        internal static void submit(DataTable rowsChanged)
        {
            foreach (DataRow row in rowsChanged.Rows)
            {
                string updateStatement = "UPDATE SERVICE_ACTIVITY" +
                    " SET ";
                if (row.Field<string>("EDITED_CELLS") != "" && row.Field<string>("EDITED_CELLS") != null)
                {
                    foreach (DataColumn column in rowsChanged.Columns)
                    {
                        if (row.Field<object>(column.ColumnName) != null && dataProvider.ColumnRules.ContainsKey(column.ColumnName) && column.ColumnName != "EDIT_DATE" && column.ColumnName != "SERVICE_ACTIVITY_ID")
                        {
                            if (column.DataType.Name == "String")
                            {
                                updateStatement += column.ColumnName + " = '" + row.Field<object>(column.ColumnName) + "', ";
                            }
                            else if (column.DataType.Name == "DateTime")
                            {
                                updateStatement += column.ColumnName + " = TO_DATE('" + row.Field<DateTime>(column.ColumnName).ToShortDateString() + "', 'MM/DD/YYYY'), ";
                            }
                            else
                            {
                                updateStatement += column.ColumnName + " = " + row.Field<object>(column.ColumnName) + ", ";
                            }
                        }
                        if (column.ColumnName != null && column.ColumnName == "EDIT_DATE")
                        {
                            updateStatement += column.ColumnName + " = TO_DATE('" + DateTime.Now.ToShortDateString() + "', 'MM/DD/YYYY'), ";
                        }
                    }
                    int x = 0;
                    updateStatement = updateStatement.Remove(updateStatement.Length - 2, 1);
                    updateStatement += "WHERE SERVICE_ACTIVITY_ID = " + row.Field<Decimal>("SERVICE_ACTIVITY_ID").ToString();
                    dataProvider.gtDataContext.Execute(updateStatement, out x, (int)ADODB.CommandTypeEnum.adCmdText);
                    dataProvider.gtDataContext.Execute("COMMIT", out x, (int)ADODB.CommandTypeEnum.adCmdText);
                }
            }
            dtList.AcceptChanges();
            //refresh();
        }

        /// <summary>
        /// Function to refresh the entries from the database. This won't be super testable currently, but the database will eventually have its own functions to run on entries. 
        /// </summary>
        internal static void refresh()
        {
            List<int> SERVICE_ACTIVITY_IDList = new List<int>();
            foreach (DataRow row in dtList.Rows)
            {
                SERVICE_ACTIVITY_IDList.Add(Int32.Parse(row.Field<object>("SERVICE_ACTIVITY_ID").ToString()));
            }
            string fieldErrorActivityRefreshStatement = "";
            if (SERVICE_ACTIVITY_IDList.Count < 1000)
            {
                string SERVICE_ACTIVITY_IDString = String.Join(", ", SERVICE_ACTIVITY_IDList);
                fieldErrorActivityRefreshStatement = "SELECT * FROM SERVICE_ACTIVITY WHERE SERVICE_ACTIVITY_ID IN (" + SERVICE_ACTIVITY_IDString + ")";
            }
            else
            {
                int index = 998;
                string SERVICE_ACTIVITY_IDString = String.Join(", ", SERVICE_ACTIVITY_IDList.GetRange(0, index));
                fieldErrorActivityRefreshStatement = "SELECT * FROM SERVICE_ACTIVITY WHERE SERVICE_ACTIVITY_ID IN (" + SERVICE_ACTIVITY_IDString + ")";
                while (SERVICE_ACTIVITY_IDList.Count > index)
                {
                    if (index + 998 > SERVICE_ACTIVITY_IDList.Count)
                    {
                        SERVICE_ACTIVITY_IDString = String.Join(", ", SERVICE_ACTIVITY_IDList.GetRange(index, SERVICE_ACTIVITY_IDList.Count - index));
                        fieldErrorActivityRefreshStatement += " OR SERVICE_ACTIVITY_ID IN (" + SERVICE_ACTIVITY_IDString + ")";
                        index += 998;
                    }
                    else
                    {
                        SERVICE_ACTIVITY_IDString = String.Join(", ", SERVICE_ACTIVITY_IDList.GetRange(index, 998));
                        fieldErrorActivityRefreshStatement += " OR SERVICE_ACTIVITY_ID IN (" + SERVICE_ACTIVITY_IDString + ")";
                        index += 998;
                    }
                }
            }
            Recordset refreshData = gtDataContext.OpenRecordset(fieldErrorActivityRefreshStatement, CursorTypeEnum.adOpenDynamic, LockTypeEnum.adLockOptimistic, -1);
            if (refreshData.EOF && refreshData.BOF)
            {
                MessageBox.Show("No records found to refresh", "G/TEchnology");
                refreshData.Close();
            }
            else
            {
                refreshData.MoveFirst();
                while (!refreshData.EOF && metaDataRecords.RecordCount > 0)
                {
                    //load the data into the proper rows.
                    int recordSERVICE_ACTIVITY_ID = Convert.ToInt32(refreshData.Fields["SERVICE_ACTIVITY_ID"].Value);
                    DataRow foundROW = dtList.Rows.Find(recordSERVICE_ACTIVITY_ID);
                    foreach (Field field in refreshData.Fields)
                    {
                        foundROW[field.Name] = field.Value;
                    }
                    refreshData.MoveNext();
                }
                refreshData.Close();
            }
        }

        /// <summary>
        /// Function to refresh the entries from the database. This won't be super testable currently, but the database will eventually have its own functions to run on entries. 
        /// </summary>
        internal static void refresh(List<int> conflictedRecords)
        {
            List<int> SERVICE_ACTIVITY_IDList = new List<int>();
            foreach (int record in conflictedRecords)
            {
                SERVICE_ACTIVITY_IDList.Add(record);
            }
            string fieldErrorActivityRefreshStatement = "";
            if (SERVICE_ACTIVITY_IDList.Count < 1000)
            {
                string SERVICE_ACTIVITY_IDString = String.Join(", ", SERVICE_ACTIVITY_IDList);
                fieldErrorActivityRefreshStatement = "SELECT * FROM SERVICE_ACTIVITY WHERE SERVICE_ACTIVITY_ID IN (" + SERVICE_ACTIVITY_IDString + ")";
            }
            else
            {
                int index = 998;
                string SERVICE_ACTIVITY_IDString = String.Join(", ", SERVICE_ACTIVITY_IDList.GetRange(0, index));
                fieldErrorActivityRefreshStatement = "SELECT * FROM SERVICE_ACTIVITY WHERE SERVICE_ACTIVITY_ID IN (" + SERVICE_ACTIVITY_IDString + ")";
                while (SERVICE_ACTIVITY_IDList.Count > index)
                {
                    if (index + 998 > SERVICE_ACTIVITY_IDList.Count)
                    {
                        SERVICE_ACTIVITY_IDString = String.Join(", ", SERVICE_ACTIVITY_IDList.GetRange(index, SERVICE_ACTIVITY_IDList.Count - index));
                        fieldErrorActivityRefreshStatement += " OR SERVICE_ACTIVITY_ID IN (" + SERVICE_ACTIVITY_IDString + ")";
                        index += 998;
                    }
                    else
                    {
                        SERVICE_ACTIVITY_IDString = String.Join(", ", SERVICE_ACTIVITY_IDList.GetRange(index, 998));
                        fieldErrorActivityRefreshStatement += " OR SERVICE_ACTIVITY_ID IN (" + SERVICE_ACTIVITY_IDString + ")";
                        index += 998;
                    }
                }
            }
            Recordset refreshData = gtDataContext.OpenRecordset(fieldErrorActivityRefreshStatement, CursorTypeEnum.adOpenDynamic, LockTypeEnum.adLockOptimistic, -1);
            if (refreshData.EOF && refreshData.BOF)
            {
                MessageBox.Show("No records found to refresh", "G/TEchnology");
                refreshData.Close();
            }
            else
            {
                refreshData.MoveFirst();
                while (!refreshData.EOF && metaDataRecords.RecordCount > 0)
                {
                    //load the data into the proper rows.
                    int recordSERVICE_ACTIVITY_ID = Convert.ToInt32(refreshData.Fields["SERVICE_ACTIVITY_ID"].Value);
                    DataRow foundROW = dtList.Rows.Find(recordSERVICE_ACTIVITY_ID);
                    foreach (Field field in refreshData.Fields)
                    {
                        foundROW[field.Name] = field.Value;
                    }
                    refreshData.MoveNext();
                }
                refreshData.Close();
            }
        }

        /// <summary>
        /// Method to clear out the leftover data in the form closing event. Putting it in the global object should allow it to clear out any data that we may not want.
        /// </summary>
        internal static void dispose()
        {
            try
            {
                if (fieldActivityErrorRecords != null && fieldActivityErrorRecords.State != (int)ADODB.ObjectStateEnum.adStateClosed)
                {
                    fieldActivityErrorRecords.Close();
                }
                if (gtGeoEditService != null && gtGeoEditService.GeometryCount > 0)
                {
                    gtGeoEditService.RemoveAllGeometries();
                    gtGeoEditService = null;
                }
                if (metaDataRecords != null) metaDataRecords.Close();
                if (ColumnRules != null) ColumnRules.Clear();
                ErrorMessage = null;
            }
            catch (Exception e)
            {
                MessageBox.Show("Error in Dispose (" + e.Message + ")", "G/Technology");
            }
        }

        /// <summary>
        /// Checks to see if the edit date matches the current date for the record in the datagrid. 
        /// </summary>
        /// <param name="rowIndex"></param>
        internal static bool dateChecker(int rowIndex)
        {
            int SERVICE_ACTIVITY_ID = Convert.ToInt32(dtList.Rows.Find(rowIndex).Field<decimal>("SERVICE_ACTIVITY_ID"));
            string dateGetterString = "SELECT EDIT_DATE FROM SERVICE_ACTIVITY WHERE SERVICE_ACTIVITY_ID = " + SERVICE_ACTIVITY_ID.ToString();
            Recordset dateForComparison = gtDataContext.OpenRecordset(dateGetterString, CursorTypeEnum.adOpenDynamic, LockTypeEnum.adLockOptimistic, -1);
            dateForComparison.MoveFirst();
            if (!dateForComparison.EOF && !dateForComparison.BOF && dateForComparison.Fields["EDIT_DATE"].Value.ToString() != "")
            {
                //if (dateForComparison.Fields["EDIT_DATE"].Value.ToString() == dtList.Rows[rowIndex].Field<object>("EDIT_DATE").ToString())
                if (dateForComparison.Fields["EDIT_DATE"].Value.ToString() == dtList.Rows.Find(rowIndex).Field<object>("EDIT_DATE").ToString())
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return true;
            }
        }

        /// <summary>
        /// Locates an object and zooms into it int he map window.
        /// </summary>
        /// <param name="FID"></param>
        /// <param name="FNO"></param>
        internal static void locate(int FID, short FNO)
        {
            gtApp.SelectedObjects.Clear();
            double zoomFactorSave = gtApp.ActiveMapWindow.SelectBehaviorZoomFactor;
            var selectBehaviorSave = gtApp.ActiveMapWindow.SelectBehavior;
            bool activeBehaviorSave = gtApp.ActiveMapWindow.ApplySelectBehaviorWhenActive;
            gtApp.ActiveMapWindow.ApplySelectBehaviorWhenActive = true;
            gtApp.ActiveMapWindow.SelectBehavior = GTSelectBehaviorConstants.gtmwsbHighlightAndCenter;
            gtApp.ActiveMapWindow.SelectBehaviorZoomFactor = 1.25;
            gtApp.ActiveMapWindow.DisplayScale = 250;
            gtApp.ActiveMapWindow.Activate();
            gtApp.SelectedObjects.Add(GTSelectModeConstants.gtsosmSelectedComponentsOnly, gtApp.DataContext.GetDDCKeyObjects(FNO, FID, GTComponentGeometryConstants.gtddcgAllGeographic)[0]);
            gtApp.ActiveMapWindow.CenterSelectedObjects();
            gtApp.ActiveMapWindow.SelectBehaviorZoomFactor = zoomFactorSave;
            gtApp.ActiveMapWindow.SelectBehavior = selectBehaviorSave;
            gtApp.ActiveMapWindow.ApplySelectBehaviorWhenActive = activeBehaviorSave;
        }

        /// <summary>
        /// Checks to see if a Structure ID exists in the db.
        /// </summary>
        /// <param name="structID"></param>
        /// <returns></returns>
        internal static Boolean checkStructID(string structID)
        {
            string checkString = "SELECT * FROM COMMON_N WHERE STRUCTURE_ID = '" + structID + "'";
            Recordset StructRecords = gtDataContext.OpenRecordset(checkString, CursorTypeEnum.adOpenDynamic, LockTypeEnum.adLockOptimistic, -1);
            if (!StructRecords.EOF && !StructRecords.BOF)
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// Checks to see if a Transformer Company Number exists in the db.
        /// </summary>
        /// <param name="transID"></param>
        /// <returns></returns>
        internal static Boolean checkTransCompanyID(string transID)
        {
            string checkString = "SELECT * FROM XFMR_OH_UNIT_N WHERE COMPANY_ID = '" + transID + "'";
            Recordset TransCompRecords = gtDataContext.OpenRecordset(checkString, CursorTypeEnum.adOpenDynamic, LockTypeEnum.adLockOptimistic, -1);
            if (!TransCompRecords.EOF && !TransCompRecords.BOF)
            {
                return true;
            }
            checkString = "SELECT * FROM XFMR_UG_UNIT_N WHERE COMPANY_ID = '" + transID + "'";
            TransCompRecords = gtDataContext.OpenRecordset(checkString, CursorTypeEnum.adOpenDynamic, LockTypeEnum.adLockOptimistic, -1);
            if (!TransCompRecords.EOF && !TransCompRecords.BOF)
            {
                return true;
            }
            return false;
        }
    }
}
