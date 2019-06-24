// ===================================================
//  Copyright 2017 Intergraph Corp.
//  File Name: fiESILocationPremise.cs
// 
// Description:   


// If the component is being deleted, delete the corresponding CU record from the Work Point(if it exists for the active WR).
// If an instance of the Ancillary CU Attributes component is being deleted, and it refers to a significant ancillary attribute on a non-CU component, reset that non-CU attribute to null.

//
//  Remarks:
// 
//  Modification History
//  Date                Author                      Comments
//  --------            --------------              --------
//  01/02/2018          Sithara                    Implemented  Business Rule as per JIRA 1294
// ======================================================
using System;
using System.Windows.Forms;
using Intergraph.GTechnology.API;
using System.Collections.Generic;
using gtCommandLogger;

namespace GTechnology.Oncor.CustomAPI
{
    public class fiAccountingImpact : FIBaseClass
    {
        #region Private Variables

        CommonFunctions m_gtcommonFunctions = new CommonFunctions();
        WorkPointCommonFunctions m_WorkPointCommonFunctions = null;
        IGTComponent m_gTActiveComponent = null;
        IGTApplication m_gtApplication = GTClassFactory.Create<IGTApplication>();

        #endregion

        #region IGTFunctional methods
        public override void Execute()
        {

        }

        public override void Delete()
        {
            try
            {
                m_WorkPointCommonFunctions = new WorkPointCommonFunctions(ActiveKeyObject, DataContext);
                m_gTActiveComponent = GetActiveComponent();
                m_WorkPointCommonFunctions.m_gTActiveComponent = m_gTActiveComponent;
                m_WorkPointCommonFunctions.m_gTComponents = Components;

                if ((m_gTActiveComponent != null) && (m_gTActiveComponent.CNO == 21 || m_gTActiveComponent.CNO == 22))
                {
                    DeleteWorkpointCUInstace();

                    if (m_gTActiveComponent.CNO == 22)
                    {
                        SetAncillaryAttribute();
                    }
                }              
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error during execution of Accounting Impact FI." + Environment.NewLine + ex.Message,
                 "G/Techonology", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Method to check whether feature belong to valid set of feature state to continue the processing
        /// </summary>
        /// <returns></returns>
        private bool IsValidFeatureState()
        {
            bool bReturn = true;

            if (ActiveKeyObject.Components.GetComponent(1).Recordset != null)
            {
                if (ActiveKeyObject.Components.GetComponent(1).Recordset.RecordCount > 0)
                {
                    ActiveKeyObject.Components.GetComponent(1).Recordset.MoveFirst();

                    string sFeatureState = Convert.ToString(ActiveKeyObject.Components.GetComponent(1).Recordset.Fields["FEATURE_STATE_C"].Value);

                    if (sFeatureState.Equals("CLS") || sFeatureState.Equals("LIP"))
                    {
                        bReturn = false;
                    }
                }
            }
            return bReturn;
        }

        private bool IsCorrectionJob()
        {
            bool bReturn = false;
            ADODB.Recordset oRS = null;

            try
            {
                oRS = DataContext.OpenRecordset("select G3E_JOBTYPE from g3e_job where g3e_identifier = ?", ADODB.CursorTypeEnum.adOpenDynamic, ADODB.LockTypeEnum.adLockOptimistic, (int)ADODB.CommandTypeEnum.adCmdText, DataContext.ActiveJob);

                oRS.MoveFirst();
                if (Convert.ToString(oRS.Fields["G3E_JOBTYPE"].Value).Equals("NON-WR"))
                {
                    bReturn = true;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (oRS != null)
                {
                    if (oRS.State == 1)
                    {
                        oRS.Close();
                        oRS.ActiveConnection = null;
                    }
                    oRS = null;
                }
            }
            return bReturn;
        }
        private bool IsFeatureEdited(int p_FID)
        {
            bool bReturn = false;
            IGTJobManagementService oJobSrv = GTClassFactory.Create<IGTJobManagementService>();
            oJobSrv.DataContext = DataContext;
            ADODB.Recordset rs = oJobSrv.FindPendingEdits();

            if (rs != null)
            {
                if (rs.RecordCount > 0)
                {
                    rs.MoveFirst();
                    while (rs.EOF == false)
                    {
                        if (Convert.ToInt32(rs.Fields["g3e_fid"].Value) == p_FID && (Convert.ToInt32(rs.Fields["G3E_CNO"].Value) == 21 || Convert.ToInt32(rs.Fields["G3E_CNO"].Value) == 22))
                        {
                            bReturn = true;
                            break;
                        }
                        rs.MoveNext();
                    }
                }
            }

            oJobSrv = null;
            return bReturn;
        }

        private bool IsFeatureOwnedToPrimarySwitchGear(int p_FID, int p_FNO)
        {
            bool bReturn = false;
            string sSQL = "Select count(*) from common_n where g3e_id = (select owner1_id from common_n where g3e_fno =? and g3e_fid = ?) and g3e_fno = 19";
            ADODB.Recordset rs = m_gtApplication.DataContext.OpenRecordset(sSQL, ADODB.CursorTypeEnum.adOpenDynamic, ADODB.LockTypeEnum.adLockOptimistic, (int)ADODB.CommandTypeEnum.adCmdText, p_FNO, p_FID);

            if (rs!=null)
            {
                if (rs.RecordCount>0)
                {
                    rs.MoveFirst();
                    int iCount = Convert.ToInt32(rs.Fields[0].Value);

                    bReturn = iCount ==1;
                }
            }

            return bReturn;
        }

        public override void Validate(out string[] ErrorPriorityArray, out string[] ErrorMessageArray)
        {
            ErrorPriorityArray = null;
            ErrorMessageArray = null;

            if (string.IsNullOrEmpty(DataContext.ActiveJob)) return;
            if (IsCorrectionJob()) return;
            if (IsFeatureOwnedToPrimarySwitchGear(Convert.ToInt32(Components[ComponentName].Recordset.Fields["g3e_fid"].Value), Convert.ToInt32(Components[ComponentName].Recordset.Fields["g3e_fno"].Value))) return;
            if (!IsFeatureEdited(Convert.ToInt32(Components[ComponentName].Recordset.Fields["g3e_fid"].Value))) return;

            m_WorkPointCommonFunctions = new WorkPointCommonFunctions(ActiveKeyObject, DataContext);

            List<string> m_lstErrorMessage = new List<string>();
            List<string> m_lstErrorPriority = new List<string>();

            int FID = 0;
            IGTComponent comp = Components[ComponentName];


            string fieldValue = string.Empty;
            GTValidationLogger gTValidationLogger = null;

            if (comp != null && comp.Recordset != null && comp.Recordset.RecordCount > 0)
            {
                FID = int.Parse(comp.Recordset.Fields["G3E_FID"].Value.ToString());
                fieldValue = Convert.ToString(comp.Recordset.Fields[FieldName].Value);
            }

            if (new gtLogHelper().CheckIfLoggingIsEnabled())
            {
                LogEntries logEntries = new LogEntries
                {
                    ActiveComponentName = ComponentName,
                    ActiveFID = FID,
                    ActiveFieldName = FieldName,
                    ActiveFieldValue = fieldValue,
                    JobID = DataContext.ActiveJob,
                    RelatedComponentName = "N/A",
                    RelatedFID = 0,
                    RelatedFieldName = "N/A",
                    RelatedFieldValue = "N/A",
                    ValidationInterfaceName = "Accounting Impact",
                    ValidationInterfaceType = "FI",
                };
                gTValidationLogger = new GTValidationLogger(logEntries);

                gTValidationLogger.LogEntry("TIMING", "START", "Accounting Impact Entry", "N/A", "");
            }

            try
            {
                //    //Features in the state CLS, OSR, OSA, or LIP do not require Validation as they interfere with the WMIS closure batch process
                if (!IsValidFeatureState())
                {
                    if (gTValidationLogger != null)
                        gTValidationLogger.LogEntry("TIMING", "END", "Accounting Impact Exit", "N/A", "");
                    return;
                }

                string errorPriorityNoWorkpoint = Convert.ToString(Arguments.GetArgument(0));
                string errorPriorityNoMatchingWorkPoint = Convert.ToString(Arguments.GetArgument(0));

                m_gTActiveComponent = GetActiveComponent();

                m_WorkPointCommonFunctions.m_gTActiveComponent = m_gTActiveComponent;
                m_WorkPointCommonFunctions.m_gTComponents = Components;

                if ((m_gTActiveComponent != null) && (m_gTActiveComponent.CNO == 21 || m_gTActiveComponent.CNO == 22))
                {
                    ValidateFeature(errorPriorityNoWorkpoint, errorPriorityNoMatchingWorkPoint, ref m_lstErrorMessage, ref m_lstErrorPriority);

                    if (m_lstErrorMessage.Count > 0)
                    {
                        ErrorMessageArray = m_lstErrorMessage.ToArray();
                        ErrorPriorityArray = m_lstErrorPriority.ToArray();
                    }
                }
                if (gTValidationLogger != null)
                    gTValidationLogger.LogEntry("TIMING", "END", "Accounting Impact Exit", "N/A", "");
            }
            catch (Exception ex)
            {
                if (null != ex.InnerException && "MULTIPLE WORKPOINTS" == ex.InnerException.Message)
                {
                    ErrorPriorityArray = new string[1];
                    ErrorMessageArray = new string[1];
                    ErrorMessageArray[0] = ex.Message;
                    ErrorPriorityArray[0] = "P1";
                }
                else
                {
                    throw new Exception("Error during execution of Accounting Impact FI." + Environment.NewLine + ex.Message);
                }
            }

        }

        #endregion

        #region Private methods
        private void ValidateFeature(string errorPriorityNoWorkpoint, string errorPriorityNoMatchingWorkPoint, ref List<string> p_lstErrorMessage, ref List<string> p_lstErrorPriority)
        {
            m_WorkPointCommonFunctions.ValidateFeature(errorPriorityNoWorkpoint, errorPriorityNoMatchingWorkPoint, ref p_lstErrorMessage, ref p_lstErrorPriority, m_gtcommonFunctions);
        }


        /// <summary>
        /// If the triggering component = CU / Ancillary CU Attributes:Delete the corresponding instance of Work Point CU Attributes.
        /// </summary>
        private void DeleteWorkpointCUInstace()
        {
            try
            {
                m_WorkPointCommonFunctions.DeleteWorkpointCUInstace(m_gtcommonFunctions);
            }
            catch
            {
                throw;
            }
        }


        /// <summary>
        /// If the triggering component is Ancillary CU Attributes,If the Unit CNO, Unit CID, and Ancillary Attribute attributes are populated.
        /// Find the corresponding attribute on the active feature,If found, set the attribute to null.
        /// </summary>
        private void SetAncillaryAttribute()
        {
            try
            {
                m_WorkPointCommonFunctions.SetAncillaryAttribute();
            }
            catch
            {
                throw;
            }
        }
        #endregion
    }
}
