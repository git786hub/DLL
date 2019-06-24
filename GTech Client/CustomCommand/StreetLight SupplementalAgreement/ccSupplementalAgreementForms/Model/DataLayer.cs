using System;
using System.Collections.Generic;
using System.Windows.Forms;
using Intergraph.GTechnology.API;
using ADODB;
using System.Data;

namespace GTechnology.Oncor.CustomAPI.Model
{
    public class DataLayer
    {
        IGTDataContext m_gTDataContext = null;
        IGTDDCKeyObjects m_gTDDCKeyObjects = null;
        public IGTKeyObjects m_SelectedKeyObjects = GTClassFactory.Create<IGTKeyObjects>();
        public string[] m_SelectedEsiLocations = null;
      
        public int designAreaFid = 0;
        public int m_Additions = 0;

        string m_ActiveWorkRequest;
        public string ActiveWorkRequest { get { return m_ActiveWorkRequest; } set { m_ActiveWorkRequest = value; } }
        public int Additions
        {
            get { return m_Additions; }
            set { m_Additions = value; }
        }

        int m_Removal = 0;
        public int Removal
        {
            get { return m_Removal; }
            set { m_Removal = value; }
        }
        public DataLayer(IGTDataContext gTDataContext, IGTDDCKeyObjects gTDDCKeyObjects)
        {
            m_gTDataContext = gTDataContext;
            m_gTDDCKeyObjects = gTDDCKeyObjects;
        }

        /// <summary>
        /// Common Grid structure of two forms.
        /// </summary>
        /// <returns></returns>
        public DataTable GridStructure()
        {
            DataTable gridDataTable = null;
            try
            {
                gridDataTable = new DataTable();
                gridDataTable.Columns.Add("ESI Location", typeof(string));
                gridDataTable.Columns.Add("Action", typeof(string));
                gridDataTable.Columns.Add("Energize", typeof(string));
                gridDataTable.Columns.Add("Quantity", typeof(int));
                gridDataTable.Columns.Add("Wattage", typeof(string));
                gridDataTable.Columns.Add("Lamp Type", typeof(string));
                gridDataTable.Columns.Add("Rate Schedule", typeof(string));
                gridDataTable.Columns.Add("Luminaire Style", typeof(string));
                gridDataTable.Columns.Add("Identifying Type", typeof(string));
                gridDataTable.Columns.Add("Address", typeof(string));
            }
            catch
            {
                throw;
            }

            return gridDataTable;
        }

        /// <summary>
        /// Data Source of the datagridview.
        /// </summary>
        /// <returns></returns>
        public DataTable GetData()
        {
            DataTable gridDataTable = GridStructure();
            IGTRelationshipService relationShipService = GTClassFactory.Create<IGTRelationshipService>();
            try
            {
                int count = 0;
                string strAction = "";
                string strEnergize = "";
                int Quantity = 1;
                string strWattage = "";
                string strLampType = "";
                string strRateSchedule = "";
                string strLuminaireStyle = "";
                string strIdentifyingType = "";
                string strAddress = "";
                string strESILocation = "";
                string strOwnerType = "";
                string strOwnerCU = "";
                string strLocation = "";

                int Latitude = 0;
                int Longitude = 0;


                IGTComponent gTComponent;

                foreach (IGTKeyObject keyObject in m_SelectedKeyObjects)
                {
                    strESILocation = m_SelectedEsiLocations[count];

                    gTComponent = keyObject.Components["COMMON_N"];

                    GetCommonAttributes(ref strAction, ref strOwnerType, ref strLocation, ref Latitude, ref Longitude, gTComponent);
                    
                    if (strAction == "PPI" || strAction == "ABI" || strAction == "INI")
                    {
                        strAction = "A";
                    }
                    else if (strAction == "PPR" || strAction == "ABO" || strAction == "OSR")
                    {
                        strAction = "R";
                    }
                    else
                    {
                        strAction = "";
                    }

                    if (strAction == "A")
                    {
                        Additions = Additions + 1;
                    }
                    else if (strAction == "R")
                    {
                        Removal = Removal + 1;
                    }

                    strEnergize = "Y";

                    gTComponent = keyObject.Components["STREETLIGHT_N"];

                    GetStreetLightAttributes(ref strWattage, ref strLampType, ref strLuminaireStyle, gTComponent);

                    strRateSchedule = GetRateSchedule(strRateSchedule, strESILocation);

                    gTComponent = keyObject.Components["COMP_UNIT_N"];

                    if (gTComponent != null && gTComponent.Recordset != null && gTComponent.Recordset.RecordCount > 0)
                    {
                        gTComponent.Recordset.MoveFirst();
                        if (!string.IsNullOrEmpty(Convert.ToString(gTComponent.Recordset.Fields["CU_C"].Value)))
                        {
                            strIdentifyingType = Convert.ToString(gTComponent.Recordset.Fields["CU_C"].Value);
                        }
                    }

                    relationShipService.ActiveFeature = keyObject;
                    relationShipService.DataContext = m_gTDataContext;
                    IGTKeyObjects gTOwnerKeyObjects = relationShipService.GetRelatedFeatures(3);

                    if (gTOwnerKeyObjects.Count > 0)
                    {
                        foreach (IGTKeyObject keyOwner in gTOwnerKeyObjects)
                        {
                            gTComponent = keyOwner.Components["COMP_UNIT_N"];
                            if (gTComponent != null && gTComponent.Recordset != null && gTComponent.Recordset.RecordCount > 0)
                            {
                                gTComponent.Recordset.MoveFirst();
                                if (!string.IsNullOrEmpty(Convert.ToString(gTComponent.Recordset.Fields["CU_C"].Value)))
                                {
                                    strOwnerCU = Convert.ToString(gTComponent.Recordset.Fields["CU_C"].Value);
                                }
                            }
                        }
                    }


                    strIdentifyingType = strIdentifyingType + "\n" + strOwnerCU + "\n" + strOwnerType;

                    if (count <= 9)
                    {
                        strAddress = "0" + (count + 1);
                    }
                    else
                    {
                        strAddress = Convert.ToString(count);
                    }

                    strAddress = strAddress + "-" + Latitude + "/" + Longitude + "\n";
                    strAddress = strAddress + strLocation;


                    gridDataTable.Rows.Add(strESILocation, strAction, strEnergize, Quantity, strWattage, strLampType, strRateSchedule, strLuminaireStyle, strIdentifyingType, strAddress);

                    count++;
                }
            }
            catch
            {
                throw;
            }
            finally
            {
                relationShipService.Dispose();
                relationShipService = null;
            }

            return gridDataTable;
        }

        /// <summary>
        /// Get RateSchedule value.
        /// </summary>
        /// <param name="strRateSchedule"></param>
        /// <param name="strESILocation"></param>
        /// <returns></returns>
        private string GetRateSchedule(string strRateSchedule, string strESILocation)
        {
            string sql = null;
            Recordset rsValidate = null;
            try
            {
                sql = "select RATE_SCHEDULE from STLT_ACCOUNT where ESI_LOCATION in ?";
                rsValidate = m_gTDataContext.OpenRecordset(sql, ADODB.CursorTypeEnum.adOpenDynamic, ADODB.LockTypeEnum.adLockReadOnly,
                    (int)ADODB.CommandTypeEnum.adCmdText, strESILocation);

                if (rsValidate.RecordCount > 0)
                {
                    rsValidate.MoveFirst();
                    if (!rsValidate.EOF && !rsValidate.BOF)
                    {
                        strRateSchedule = Convert.ToString(rsValidate.Fields[0].Value);
                    }
                }
            }
            catch
            {
                throw;
            }
            finally
            {
                rsValidate.Close();
                rsValidate = null;
            }
            return strRateSchedule;
        }
        /// <summary>
        /// Get Street light attributes : WATT_Q,LAMP_TYPE_C,LUMIN_STYL_C
        /// </summary>
        /// <param name="strWattage"></param>
        /// <param name="strLampType"></param>
        /// <param name="strLuminaireStyle"></param>
        /// <param name="gTComponent"></param>
        private static void GetStreetLightAttributes(ref string strWattage, ref string strLampType, ref string strLuminaireStyle, IGTComponent gTComponent)
        {
            if (gTComponent != null && gTComponent.Recordset != null && gTComponent.Recordset.RecordCount > 0)
            {
                gTComponent.Recordset.MoveFirst();
                if (!string.IsNullOrEmpty(Convert.ToString(gTComponent.Recordset.Fields["WATT_Q"].Value)))
                {
                    strWattage = Convert.ToString(gTComponent.Recordset.Fields["WATT_Q"].Value);
                }

                if (!string.IsNullOrEmpty(Convert.ToString(gTComponent.Recordset.Fields["LAMP_TYPE_C"].Value)))
                {
                    strLampType = Convert.ToString(gTComponent.Recordset.Fields["LAMP_TYPE_C"].Value);
                }

                if (!string.IsNullOrEmpty(Convert.ToString(gTComponent.Recordset.Fields["LUMIN_STYL_C"].Value)))
                {
                    strLuminaireStyle = Convert.ToString(gTComponent.Recordset.Fields["LUMIN_STYL_C"].Value);
                }
            }
        }
        /// <summary>
        /// Get common attributes.
        /// </summary>
        /// <param name="strFeatureState"></param>
        /// <param name="strOwnerType"></param>
        /// <param name="strLocation"></param>
        /// <param name="Latitude"></param>
        /// <param name="Longitude"></param>
        /// <param name="gTComponent"></param>
        private static void GetCommonAttributes(ref string strFeatureState, ref string strOwnerType, ref string strLocation, ref int Latitude, ref int Longitude, IGTComponent gTComponent)
        {
            if (gTComponent != null && gTComponent.Recordset != null && gTComponent.Recordset.RecordCount > 0)
            {
                gTComponent.Recordset.MoveFirst();
                if (!string.IsNullOrEmpty(Convert.ToString(gTComponent.Recordset.Fields["FEATURE_STATE_C"].Value)))
                {
                    strFeatureState = Convert.ToString(gTComponent.Recordset.Fields["FEATURE_STATE_C"].Value);
                }

                if (!string.IsNullOrEmpty(Convert.ToString(gTComponent.Recordset.Fields["OWNED_TYPE_C"].Value)))
                {
                    strOwnerType = Convert.ToString(gTComponent.Recordset.Fields["OWNED_TYPE_C"].Value);
                }

                if (!string.IsNullOrEmpty(Convert.ToString(gTComponent.Recordset.Fields["LOCATION"].Value)))
                {
                    strLocation = Convert.ToString(gTComponent.Recordset.Fields["LOCATION"].Value);
                }

                if (!string.IsNullOrEmpty(Convert.ToString(gTComponent.Recordset.Fields["LATITUDE"].Value)))
                {
                    Latitude = Convert.ToInt32(gTComponent.Recordset.Fields["LATITUDE"].Value);
                }

                if (!string.IsNullOrEmpty(Convert.ToString(gTComponent.Recordset.Fields["LONGITUDE"].Value)))
                {
                    Longitude = Convert.ToInt32(gTComponent.Recordset.Fields["LONGITUDE"].Value);
                }
            }
        }
        /// <summary>
        /// Get template location of the form.
        /// </summary>
        /// <param name="strParameterName"> param name in the SYS_GENERALPARAMETER table.</param>
        /// <returns></returns>
        public string GetFormTemplateLocation(string strParameterName)
        {
            string strFilePath = "";
            Recordset rs = null;
            try
            {
                rs = m_gTDataContext.OpenRecordset("select param_value from SYS_GENERALPARAMETER where param_name=:1", ADODB.CursorTypeEnum.adOpenDynamic, ADODB.LockTypeEnum.adLockReadOnly,
                    (int)ADODB.CommandTypeEnum.adCmdText, strParameterName);


                if (rs != null && rs.RecordCount > 0)
                {
                    rs.MoveFirst();
                    strFilePath = Convert.ToString(rs.Fields["param_value"].Value);
                    return strFilePath;
                }
            }
            catch
            {
                throw;
            }
            finally
            {
                rs.Close();
                rs = null;
            }
            return null;
        }

        /// <summary>
        /// Get DesignAreaKeyObject of current Job.
        /// </summary>
        /// <returns></returns>
        public IGTKeyObject GetDesignAreaKeyObject()
        {
            string sql = "SELECT G3E_FID FROM DESIGNAREA_P WHERE JOB_ID=:1";
            int outRecords = 0;
            Recordset rsDesignArea = null;
            
            try
            {

                rsDesignArea = m_gTDataContext.Execute(sql, out outRecords, (int)CommandTypeEnum.adCmdText, ActiveWorkRequest);

                if (rsDesignArea.RecordCount > 0)
                {
                    rsDesignArea.MoveFirst();
                    return (m_gTDataContext.OpenFeature(8100, Convert.ToInt32(rsDesignArea.Fields[0].Value)));
                }
            }
            catch
            {
                throw;
            }
            finally
            {
                rsDesignArea.Close();
                rsDesignArea = null;
            }

            return null;
        }

        /// <summary>
        /// Check the file name is already attached to the active job.
        /// </summary>
        /// <param name="strFilename">Html file name</param>
        /// <returns></returns>
        public bool IsExistingAttachment(string strFilename)
        {
            string sql = "";
            Recordset rs = null;
            try
            {
                sql = "select count(*) from JOB_HYPERLINK_N where g3e_fid=:1 and HYPERLINK_T=:2";
                rs = m_gTDataContext.OpenRecordset(sql, ADODB.CursorTypeEnum.adOpenDynamic, ADODB.LockTypeEnum.adLockReadOnly,
                   (int)ADODB.CommandTypeEnum.adCmdText, designAreaFid, strFilename);

                if (rs != null && rs.RecordCount > 0)
                {
                    rs.MoveFirst();
                    if (Convert.ToInt32(rs.Fields[0].Value) > 0)
                    {
                        return true;
                    }
                }
            }
            catch
            {
                throw;
            }
            finally
            {
                rs.Close();
                rs = null;
            }

            return false;
        }
        /// <summary>
        /// Delete Existing Attachment.
        /// </summary>
        /// <param name="strFilename">Html file name</param>
        /// <returns></returns>
        public void DeleteExistingAttachment(string strFilename)
        {
            string sql = "";
            int iRecordsAffected = 0;
            try
            {
                sql = "delete from JOB_HYPERLINK_N where g3e_fid=:1 and HYPERLINK_T=:2";
                m_gTDataContext.Execute(sql, out iRecordsAffected, (int)CommandTypeEnum.adCmdText, designAreaFid, strFilename);
            }
            catch
            {
                throw;
            }
        }
    }
}
