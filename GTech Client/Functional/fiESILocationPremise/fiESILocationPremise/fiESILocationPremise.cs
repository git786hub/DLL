// ===================================================
//  Copyright 2017 Intergraph Corp.
//  File Name: fiESILocationPremise.cs
// 
//  Description:   	This interface validates the service points of esi location attribute if it is connected to Primary Points of Delivery.
//
//  Remarks:
// 
//  Modification History
//  Date                Author                      Comments
//  --------            --------------              --------
//  09/01/2018         Sithara                  Implemented  Business Rule as per JIRA 1124
// ======================================================
using System;
using System.Collections.Generic;
using Intergraph.GTechnology.API;
using ADODB;
using gtCommandLogger;

namespace GTechnology.Oncor.CustomAPI
{
    public class fiESILocationPremise : ICCFIBase
    {
        public override void Validate(out string[] ErrorPriorityArray, out string[] ErrorMessageArray)
        {
            ErrorPriorityArray = null;
            ErrorMessageArray = null;
            Recordset rsPremise = null;
            bool validateMess = false;
            List<string> errorMsg = new List<string>();
            List<string> errorPriority = new List<string>();
            ValidationRuleManager validateMsg = new ValidationRuleManager();
            bool connectedToPPD = false;

            GTValidationLogger gTValidationLogger = null;
            IGTComponent comp = Components[ComponentName];
            int FID = 0;

            string fieldValue = string.Empty;

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
                    ValidationInterfaceName = "ESI Location Premise",
                    ValidationInterfaceType = "FI",
                };
                gTValidationLogger = new GTValidationLogger(logEntries);

                gTValidationLogger.LogEntry("TIMING", "START", "ESI Location Premise Entry", "N/A", "");
            }


            try
            {
                if (ActiveKeyObject.FNO == 55)
                {
                    GetActiveComponent();

                    if (ActiveComponent.CNO == 5504)
                    {
                        rsPremise = ActiveComponent.Recordset;
                        connectedToPPD = IsServicePointConnectedToPrimaryPointsDelivery();

                        if (rsPremise != null && rsPremise.RecordCount > 0 && connectedToPPD)
                        {
                            if (!rsPremise.EOF && !rsPremise.BOF)
                            {
                                if (string.IsNullOrEmpty(Convert.ToString(rsPremise.Fields["PREMISE_NBR"].Value)))
                                {
                                    validateMess = true;
                                }
                            }
                        }

                    }
                }

                if (validateMess)
                {
                    validateMsg.Rule_Id = "PESI01";
                    validateMsg.BuildRuleMessage(GTClassFactory.Create<IGTApplication>(), null);

                    errorMsg.Add(validateMsg.Rule_MSG);
                    errorPriority.Add(Convert.ToString(Arguments.GetArgument(0)));

                    ErrorPriorityArray = errorPriority.ToArray();
                    ErrorMessageArray = errorMsg.ToArray();
                }

                if(gTValidationLogger != null)
                    gTValidationLogger.LogEntry("TIMING", "END", "ESI Location Premise Exit", "N/A", "");

            }
            catch (Exception ex)
            {
                throw new Exception("Error during execution of ESI Location Premise FI: " + ex.Message);
            }
        }
        /// <summary>
        /// IsServicePointConnectedToPrimaryPointsDelivery returns true if active feature is conncted to the Primary Points of Delivery.
        /// </summary>
        /// <returns></returns>
        private bool IsServicePointConnectedToPrimaryPointsDelivery()
        {
            bool connected = false;
            short cRNO = 14;
            try
            {
                IGTKeyObjects relatedfeatures = GetRelatedFeatures(ActiveKeyObject, cRNO);
                foreach (IGTKeyObject relfeature in relatedfeatures)
                {
                    if (relfeature.FNO == 12)
                    {
                        connected = true;
                        break;
                    }
                }
            }
            catch
            {
                throw;
            }
            return connected;
        }

        /// <summary>
        /// Returns the related feature for a corresponding relationship number
        /// </summary>
        /// <param name="activeFeature"></param>
        /// <param name="cRNO"></param>
        /// <returns></returns>
        private IGTKeyObjects GetRelatedFeatures(IGTKeyObject activeFeature, short cRNO)
        {
            try
            {
                using (IGTRelationshipService relService = GTClassFactory.Create<IGTRelationshipService>())
                {

                    relService.DataContext = DataContext;
                    relService.ActiveFeature = activeFeature;

                    IGTKeyObjects relatedFeatures = relService.GetRelatedFeatures(cRNO);

                    return relatedFeatures;
                }
            }
            catch
            {
                throw;
            }
        }
    }
}
