//----------------------------------------------------------------------------+
//        Class: fiLocationAttributes
//  Description: This interface sets location attributes when structure symbols are placed or moved.
//----------------------------------------------------------------------------+
//     $Author:: hkonda                                                       $
//       $Date:: 04/08/17                                                     $
//   $Revision:: 1                                                            $
//----------------------------------------------------------------------------+
//    $History:: fiLocationAttributes.cs                                           $
// 
// *****************  Version 1  *****************
// User: hkonda     Date: 04/08/17    Time: 18:00  Desc : Created
// User: hkonda     Date: 25/09/17    Time: 18:00  Desc : When Y is negative then structure id will contain two hyphens, which is not correct and will exceed 16 chars. Fixed code to ignore '- ' sign of Y.
// User: hkonda     Date: 03/11/17    Time: 18:00  Desc : Ref.jira-957. Code changes done to update structure id only if the Job status DESIGN and in this case update the structure ids of the associated work points.
// User: hkonda     Date: 06/02/18    Time: 18:00  Desc : Code changes done to update structure id during feature placement or when job status is DESIGN.
// User: hkonda     Date: 18/06/18    Time: 18:00  Desc : Fix for JIRA ONCORDEV-1775. Corrected Structure ID and fixed improper truncating.
// User: hkonda     Date: 05/11/18    Time: 18:00  Desc : Fix for ONCORDEV-2136 - Structure ID update on Owned features
// User: hkonda     Date: 21/01/19    Time: 18:00  Desc : Fix for ALM-1702-JIRA-2455 - Error message displays during the placement of a Secondary Enclosure feature
//----------------------------------------------------------------------------+


using System;
using System.Windows.Forms;
using Intergraph.CoordSystems.Interop;
using Intergraph.GTechnology.API;
using Intergraph.GTechnology.Interfaces;
using System.Linq;
using System.Collections.Generic;
using ADODB;

namespace GTechnology.Oncor.CustomAPI
{
    public class fiLocationAttributes : IGTFunctional
    {
        private GTArguments m_Arguments = null;
        private IGTDataContext m_DataContext = null;
        private IGTComponents m_components;
        private string m_ComponentName;
        private string m_FieldName;
        private AttributeHolder m_objAttributeHolder = null;

        public GTArguments Arguments
        {
            get { return m_Arguments; }
            set { m_Arguments = value; }
        }

        public string ComponentName
        {
            get { return m_ComponentName; }
            set { m_ComponentName = value; }
        }

        public IGTComponents Components
        {
            get { return m_components; }
            set { m_components = value; }
        }

        public IGTDataContext DataContext
        {
            get { return m_DataContext; }
            set { m_DataContext = value; }
        }

        public void Delete()
        {
        }

        public void Execute()
        {
            double dXoord = 0.0;
            double dYoord = 0.0;
            double dZoord = 0.0;
            try
            {

                IGTGeometry geometry = Components[ComponentName].Geometry;
                m_objAttributeHolder = new AttributeHolder();

                dXoord = ((IGTPointGeometry)geometry).FirstPoint.X;
                dYoord = ((IGTPointGeometry)geometry).FirstPoint.Y;

                ConvertToLatitudeAndLongitude(ref dXoord, ref dYoord, ref dZoord);

                m_objAttributeHolder.PointX = ((IGTPointGeometry)geometry).FirstPoint.X;
                m_objAttributeHolder.PointY = ((IGTPointGeometry)geometry).FirstPoint.Y;
                m_objAttributeHolder.XCoordinate = dXoord;
                m_objAttributeHolder.YCoordinate = dYoord;
                m_objAttributeHolder.ZCoordinate = dZoord;
                ProcessLocationFlags();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error during Location Attributes FI execution. " + ex.Message, "G/Technology");
            }
        }

        /// <summary>
        /// Method to read the inteface arguments and call the corresponding method to update location attributes
        /// </summary>
        private void ProcessLocationFlags()
        {
            try
            {
                object[] argumentArray = m_Arguments.GTechArgumentArray.ToArray();
                if (argumentArray != null && argumentArray.Length > 0)
                {
                    string argumentString = Convert.ToString(argumentArray[0]);
                    switch (argumentString)
                    {
                        case "OLS": // For structures
                            UpdateLocationAttributes(true, true, true);
                            break;
                        case "OL": // for substation symbol
                            UpdateLocationAttributes(false, true, true);
                            break;
                        case "O": // For non- structures
                            UpdateLocationAttributes(false, true, false);
                            break;
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Method to update the location attributes based.
        /// </summary>
        /// <param name="updateStructureId"></param>
        /// <param name="updateOGGXY"></param>
        /// <param name="updateLatLong"></param>
        private void UpdateLocationAttributes(bool updateStructureId, bool updateOGGXY, bool updateLatLong)
        {
            try
            {
                IGTComponent commonComponent = Components.GetComponent(1);
                if (updateStructureId)
                {
                    commonComponent.Recordset.MoveFirst();

                    int fid = Convert.ToInt32(commonComponent.Recordset.Fields["G3E_FID"].Value);
                    short fno = Convert.ToInt16(commonComponent.Recordset.Fields["G3E_FNO"].Value);

                    /* Structure Id to be updated for the following cases 
                     * During placement of the feature
                     * If feature is NOT posted (Job Status is DESIGN)
                    */
                    if (!CheckIfJobIsPosted() || string.IsNullOrEmpty(Convert.ToString(commonComponent.Recordset.Fields["STRUCTURE_ID"].Value)))
                    {
                        string structureId = GetFormatedStructureId(Convert.ToDecimal(m_objAttributeHolder.YCoordinate), Convert.ToDecimal(m_objAttributeHolder.XCoordinate));
                        UpdateStructureIdForOwnedFeatures(commonComponent.Recordset, structureId);
                        commonComponent.Recordset.Fields["STRUCTURE_ID"].Value = structureId;
                        UpdateAssociatedWorkPointsStructureId(fno, fid, structureId);
                    }
                }
                if (commonComponent.Recordset.RecordCount > 0)
                {
                    commonComponent.Recordset.MoveFirst();
                    if (updateOGGXY)
                    {
                        commonComponent.Recordset.Fields["OGGX_H"].Value = m_objAttributeHolder.PointX;
                        commonComponent.Recordset.Fields["OGGY_H"].Value = m_objAttributeHolder.PointY;
                    }
                    if (updateLatLong)
                    {
                        commonComponent.Recordset.Fields["LATITUDE"].Value = Math.Abs(Truncate(m_objAttributeHolder.YCoordinate));
                        commonComponent.Recordset.Fields["LONGITUDE"].Value = Math.Abs(Truncate(m_objAttributeHolder.XCoordinate));
                    }
                }
                commonComponent.Recordset.Update(System.Type.Missing, System.Type.Missing);
            }
            catch (Exception)
            {
                throw;
            }

        }


        /// <summary>
        /// Method to convert storage coordinates to Latitude and Longitude
        /// </summary>
        /// <param name="dXoord">X location</param>
        /// <param name="dYoord">Y location</param>
        /// <param name="dZoord">Z location (0)</param>
        private void ConvertToLatitudeAndLongitude(ref double dXoord, ref double dYoord, ref double dZoord)
        {
            try
            {
                Intergraph.CoordSystems.Interop.CoordSystemClass coords = new CoordSystemClass();
                int outrec = 0;
                ADODB.Recordset rs = DataContext.Execute("select c.*  from g3e_dataconnection_optable d , gcoordsystemtable  c where " +
                    " d.g3e_username ='" + DataContext.ConfigurationName + "' and d.g3e_csname=c.name", out outrec, (int)ADODB.CommandTypeEnum.adCmdText, new object[0]);

                rs.MoveFirst();
                object[] rowformat = new object[rs.Fields.Count];
                for (int ifld = 0; ifld < rs.Fields.Count; ifld++)
                {
                    rowformat[ifld] = rs.Fields[ifld].Value;
                }
                coords.LoadFromGCoordSystemTableRowFormat(rowformat);

                coords.TransformPoint(Intergraph.CoordSystems.Interop.CSPointConstants.cspUOR, (int)Intergraph.CoordSystems.CSTransformLinkConstants.cstlDatumTransformation,
                                    Intergraph.CoordSystems.Interop.CSPointConstants.cspLLO, (int)Intergraph.CoordSystems.CSTransformLinkConstants.cstlDatumTransformation,
                                    ref dXoord, ref dYoord, ref dZoord);

                dXoord = dXoord * 180 / (4 * Math.Atan(1));
                dYoord = dYoord * 180 / (4 * Math.Atan(1));
            }
            catch (Exception)
            {
                throw;
            }
        }

        public string FieldName
        {
            get { return m_FieldName; }
            set { m_FieldName = value; }
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
            ErrorMessageArray = null;
            ErrorPriorityArray = null;
        }

        /// <summary>
        /// Method to format the structure id based on Lat-Long values
        /// </summary>
        /// <param name="x">x value</param>
        /// <param name="y">y value</param>
        /// <returns></returns>
        private string GetFormatedStructureId(decimal y, decimal x)
        {
            try
            {
                return String.Format("{0:0000000}-{1:00000000}", Math.Abs(y) * 100000, Math.Abs(x) * 100000);
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Method to check whether the Job is posted or not. If the status is DESIGN then it means the job is NOT posted.
        /// </summary>
        /// <returns>false, if job is not posted and vice versa</returns>
        private bool CheckIfJobIsPosted()
        {
            try
            {
                string jobId = DataContext.ActiveJob;
                string jobStatus = string.Empty;
                Recordset jobInfoRs = GetRecordSet(string.Format("select G3E_JOBSTATUS from g3e_job where g3e_identifier  = '{0}'", jobId));
                if (jobInfoRs != null && jobInfoRs.RecordCount > 0)
                {
                    jobInfoRs.MoveFirst();
                    jobStatus = Convert.ToString(jobInfoRs.Fields["G3E_JOBSTATUS"].Value);
                }
                return !string.IsNullOrEmpty(jobStatus) && jobStatus.ToUpper() == "DESIGN" ? false : true;
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Method to update the Structure Id of associated work points for a structure
        /// </summary>
        /// <param name="fno"></param>
        /// <param name="fid"></param>
        /// <param name="newStructureId"></param>
        private void UpdateAssociatedWorkPointsStructureId(short fno, int fid, string newStructureId)
        {
            List<int> workPointFidList = null;
            try
            {
                int recordsAffected = 0;
                ADODB.Recordset rs = DataContext.Execute("select g3e_fid WPFID from workpoint_cu_n where assoc_fid = " + fid + " and assoc_fno =" + fno, out recordsAffected, (int)ADODB.CommandTypeEnum.adCmdText, new int[0]);
                if (rs != null && rs.RecordCount > 0)
                {
                    rs.MoveFirst();
                    workPointFidList = new List<int>();
                    while (!rs.EOF)
                    {
                        workPointFidList.Add(Convert.ToInt32(rs.Fields["WPFID"].Value));
                        rs.MoveNext();
                    }
                }

                if (workPointFidList != null)
                {
                    foreach (int wpFid in workPointFidList)
                    {
                        IGTKeyObject workPointFeature = m_DataContext.OpenFeature(191, wpFid);
                        if (workPointFeature != null)
                        {
                            Recordset workPointFeatureRs = workPointFeature.Components.GetComponent(19101).Recordset;
                            if (workPointFeatureRs != null && workPointFeatureRs.RecordCount > 0)
                            {
                                workPointFeatureRs.MoveFirst();
                                workPointFeatureRs.Fields["STRUCTURE_ID"].Value = newStructureId;
                            }
                        }
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Method to update the new Structure Id for owned features only if the structure id is existing previously and it is same the owner's structure Id
        /// </summary>
        /// <param name="commonCompRs">Common component recordset of active feature</param>
        /// <param name="newStructureId">Structure Id to update on the owned features</param>
        private void UpdateStructureIdForOwnedFeatures(Recordset commonCompRs, string newStructureId)
        {
            List<Tuple<short, int>> ownedFeatures = null; // FNO,FID
            IGTComponent commonComponent = null;
            try
            {
                int fid = Convert.ToInt32(commonCompRs.Fields["G3E_FID"].Value);
                short fno = Convert.ToInt16(commonCompRs.Fields["G3E_FNO"].Value);
                // string oldStructureId = Convert.ToString(commonCompRs.Fields["STRUCTURE_ID"].Value);
                IGTKeyObject activeFeature = DataContext.OpenFeature(fno, fid);

                ownedFeatures = GetOwnedFeatures(activeFeature);
                if (ownedFeatures == null)
                    return;

                foreach (Tuple<short, int> feature in ownedFeatures)
                {
                    commonComponent = DataContext.OpenFeature(feature.Item1, feature.Item2).Components.GetComponent(1);
                    commonComponent.Recordset.MoveFirst();
                    commonComponent.Recordset.Fields["STRUCTURE_ID"].Value = newStructureId;
                }
            }

            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Gets the owned feature for the input active feature
        /// </summary>
        /// <param name="activeFeature"></param>
        /// <returns>Collection of owned feature if any or null</returns>
        private List<Tuple<short, int>> GetOwnedFeatures(IGTKeyObject activeFeature)
        {
            IGTRelationshipService gTRelationshipService = null;
            List<Tuple<short, int>> ownedFeatures = null;
            IGTKeyObjects gTKeyObjects = null;
            try
            {
                gTRelationshipService = GTClassFactory.Create<IGTRelationshipService>();
                gTRelationshipService.DataContext = DataContext;
                gTRelationshipService.ActiveFeature = activeFeature;

                try
                {
                    gTKeyObjects = gTRelationshipService.GetRelatedFeatures(2);
                }
                catch
                {
                    return null;
                }

                if (gTKeyObjects != null && gTKeyObjects.Count > 0)
                {
                    ownedFeatures = new List<Tuple<short, int>>();

                    foreach (IGTKeyObject feature in gTKeyObjects)
                    {
                        if (!IsActiveFeatureIsLinear(feature.FNO))
                        {
                            ownedFeatures.Add(new Tuple<short, int>(feature.FNO, feature.FID));
                        }
                    }
                }
                return ownedFeatures;
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                if (gTRelationshipService!=null)
                {
                    gTRelationshipService.Dispose();
                    gTRelationshipService = null;
                }
            }
        }

        /// <summary>
        /// Returns true if activefeature is linear feature.
        /// </summary>
        /// <returns></returns>
        private bool IsActiveFeatureIsLinear(short Fno)
        {
            string sql = "";
            Recordset rsLinear = null;
            try
            {
                sql = "SELECT * FROM G3E_COMPONENTINFO_OPTABLE WHERE G3E_CNO IN(SELECT G3E_PRIMARYGEOGRAPHICCNO FROM G3E_FEATURES_OPTABLE WHERE G3E_FNO=?) AND UPPER(G3E_GEOMETRYTYPE) LIKE '%POINT%'";
                rsLinear = DataContext.OpenRecordset(sql, ADODB.CursorTypeEnum.adOpenDynamic, ADODB.LockTypeEnum.adLockReadOnly,
            (int)ADODB.CommandTypeEnum.adCmdText, Fno);

                if (rsLinear.RecordCount <= 0)
                {
                    return true;
                }
                return false;
            }
            catch
            {
                throw;
            }

        }
        /// <summary>
		/// Method to execute sql query and return the result record set
		/// </summary>
		/// <param name="sqlString"></param>
		/// <returns></returns>
		private Recordset GetRecordSet(string sqlString)
        {
            try
            {
                int outRecords = 0;
                ADODB.Command command = new ADODB.Command();
                command.CommandText = sqlString;
                ADODB.Recordset results = DataContext.ExecuteCommand(command, out outRecords);
                return results;
            }
            catch (Exception)
            {
                throw;
            }
        }


        /// <summary>
        /// Method to preserve 5 digits after decimal and truncate the rest of the value
        /// </summary>
        /// <param name="value"> x or y coordinate</param>
        /// <returns> properly truncated decimal value</returns>
        private decimal Truncate(double value)
        {
            try
            {
                return Convert.ToDecimal((Math.Truncate(100000 * value) / 100000).ToString("F5"));
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}


