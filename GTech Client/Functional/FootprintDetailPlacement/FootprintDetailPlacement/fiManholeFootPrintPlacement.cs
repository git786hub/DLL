// ===================================================
//  Copyright 2017 Intergraph Corp.
//  File Name: fiPhasePositionValidate.cs
// 
//  Description:   FI generate Polygon Geometries and place Detail Footprint in Detail window base on Manhole Type
//
//  Remarks:
// 
//  Modification History
//  Date                Author                      Comments
//  --------            --------------              --------
//  24/07/2017          Pramod                      Implemented changes in Execute method.                   
// ======================================================
using Intergraph.GTechnology.API;
using Intergraph.GTechnology.Interfaces;
using System;

namespace GTechnology.Oncor.CustomAPI
{
    public class fiManholeFootPrintPlacement : IGTFunctional
    {
        private GTArguments _arguments = null;
        private IGTDataContext _dataContext = null;
        private IGTComponents _components;
        private string _componentName;
        private string _fieldName;
        IGTApplication _gtApp;
        public GTArguments Arguments
        {
            get { return _arguments; }
            set { _arguments = value; }
        }

        public string ComponentName
        {
            get { return _componentName; }
            set { _componentName = value; }
        }

        public IGTComponents Components
        {
            get { return _components; }
            set { _components = value; }
        }

        public IGTDataContext DataContext
        {
            get { return _dataContext; }
            set { _dataContext = value; }
        }


        public void Delete()
        {
        }

        public void Execute()
        {
            if (Convert.ToInt32(_components[_componentName].Recordset.Fields["g3E_fno"].Value) != 106) return;

            ADODB.Recordset rs = null;
            IGTComponent gtComponent = _components["MANHOLE_DP"];
            double xCoord = 0;
            double yCoord = 0;
            IGTMapWindow mapWindow = null;
            try
            {
                if (gtComponent.Recordset.RecordCount == 0)
                {
                    if (_components["MANHOLE_N"].Recordset != null)
                    {
                        if (_components["MANHOLE_N"].Recordset.RecordCount > 0)
                        {
                            _components["MANHOLE_N"].Recordset.MoveFirst();
                        }
                    }

                    // Check Mamhole Detail Footprint component count if count==0 then call procedure to generate Footprint Detial components instance otherwise skip 
                    _gtApp = GTClassFactory.Create<IGTApplication>();
                    gtComponent = _components["DETAILIND_T"];
                    //Check Manhole Map window exists otherwise throw message to create Detial Map winod 
                    if (gtComponent.Recordset.RecordCount > 0 && _components["MANHOLE_N"].Recordset.Fields["TYPE_C"].Value.GetType()!=typeof(DBNull))
                    {
                        rs = gtComponent.Recordset;
                        rs.MoveFirst();
                       //Get MBR X and Y . Calculate midpoint 
                        xCoord = (Convert.ToDouble(rs.Fields["MBR_X_HIGH"].Value) + Convert.ToDouble(rs.Fields["MBR_X_LOW"].Value)) / 2;
                        yCoord = (Convert.ToDouble(rs.Fields["MBR_Y_HIGH"].Value) + Convert.ToDouble(rs.Fields["MBR_Y_LOW"].Value)) / 2;                        

                       //Pass ,fid,Detialid,X and Y,Manhole Type to place Detail Footprint in Detail window
                        PlaceManholeFootprint(Convert.ToInt32(rs.Fields["G3e_FID"].Value), Convert.ToInt32(rs.Fields["G3E_DETAILID"].Value), xCoord, yCoord, _components["MANHOLE_N"].Recordset.Fields["TYPE_C"].Value.ToString());

                        if (_components["MANHOLE_DP"].Recordset.RecordCount > 0)
                        {
                            _components["MANHOLE_DP"].Recordset.Update();
                        }

                        IGTMapWindows maps = _gtApp.GetMapWindows(GTMapWindowTypeConstants.gtapmtDetail);
                        foreach (IGTMapWindow detWindow in _gtApp.GetMapWindows(GTMapWindowTypeConstants.gtapmtAll))
                        {
                            if (detWindow.DetailID == Convert.ToInt32(rs.Fields["G3E_DETAILID"].Value))
                            {
                                mapWindow = detWindow;
                                break;
                            }
                        }
                        if (mapWindow != null) {mapWindow.CenterSelectedObjects(); }
                        _gtApp.RefreshWindows();
                    }                   
                }
            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show("Error in fiManholeFootPrintPlacement " + ex.StackTrace,"G/Technology");
                throw ex;
            }
        }

        public string FieldName
        {
            get { return _fieldName; }
            set { _fieldName = value; }
        }

        public IGTFieldValue FieldValueBeforeChange
        {
            get;
            set;
        }

        public GTFunctionalTypeConstants Type
        {
            get;
            set;
        }
        public void Validate(out string[] ErrorPriorityArray, out string[] ErrorMessageArray)
        {
            ErrorPriorityArray = null;
            ErrorMessageArray = null;

        }
        /// <summary>
        /// Call DB Stored Procedure to generate Polygon Geometries for Detail Foorprint component
        /// </summary>
        /// <param name="g3eFid"></param>
        /// <param name="detailID"></param>
        /// <param name="xCoord"></param>
        /// <param name="yCoord"></param>
        /// <param name="mhType"></param>
        private void PlaceManholeFootprint(int g3eFid, int detailID, double xCoord, double yCoord, string mhType)
        {

            ADODB.Command cmd = null;
            int outRecords = 0;
            string sqlString = string.Format("Begin FootPrintDetailPlacement.AddManholeDetailFootprint({0},{1},'{2}',{3},{4}); End;", g3eFid, detailID, mhType, xCoord, yCoord);
            try
            {
                cmd = new ADODB.Command();
                cmd.CommandText = sqlString;
                ADODB.Recordset results = _dataContext.ExecuteCommand(cmd, out outRecords);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
