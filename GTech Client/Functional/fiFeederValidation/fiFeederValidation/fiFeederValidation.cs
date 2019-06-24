//----------------------------------------------------------------------------+
//        Class: fiFeederValidation
//      Description: This functional interface validates that features with a closed status 
//                   have the same value for both Feeder ID and Tie Feeder ID.
//----------------------------------------------------------------------------+
//     $Author:: pnlella                                                      $
//       $Date:: 08/05/2019                                                     $
//   $Revision:: 1                                                            $
//----------------------------------------------------------------------------+
//    $History:: fiFeederValidation.cs                                           $
//----------------------------------------------------------------------------+
using Intergraph.GTechnology.API;
using Intergraph.GTechnology.Interfaces;
using System;
using gtCommandLogger;

namespace GTechnology.Oncor.CustomAPI
{
    public class fiFeederValidation : IGTFunctional
    {
        #region Private Members
        private GTArguments m_GTArguments = null;
        private IGTDataContext m_GTDataContext = null;
        private string m_gCompName = string.Empty;
        private IGTComponents m_gComps = null;
        private IGTFieldValue m_gFieldVal = null;
        private string m_gFieldName = string.Empty;
        private GTFunctionalTypeConstants m_gFIType;

        #endregion

        #region IGTFunctional Members

        public GTArguments Arguments
        {
            get
            {
                return m_GTArguments;
            }
            set
            {
                m_GTArguments = value;
            }
        }
        public string ComponentName
        {
            get
            {
                return m_gCompName;
            }
            set
            {
                m_gCompName = value;
            }
        }
        public IGTComponents Components
        {
            get
            {
                return m_gComps;
            }
            set
            {
                m_gComps = value;
            }
        }
        public IGTDataContext DataContext
        {
            get
            {
                return m_GTDataContext;
            }
            set
            {
                m_GTDataContext = value;
            }
        }
        public string FieldName
        {
            get
            {
                return m_gFieldName;
            }

            set
            {
                m_gFieldName = value;
            }
        }
        public IGTFieldValue FieldValueBeforeChange
        {
            get
            {
                return m_gFieldVal;
            }

            set
            {
                m_gFieldVal = value;
            }
        }

        public GTFunctionalTypeConstants Type
        {
            get
            {
                return m_gFIType;
            }

            set
            {
                m_gFIType = value;
            }
        }

        public void Delete()
        {

        }

        public void Execute()
        {
            
        }

        public void Validate(out string[] ErrorPriorityArray, out string[] ErrorMessageArray)
        {
            ErrorPriorityArray = null;
            ErrorMessageArray = null;
            GTValidationLogger gTValidationLogger = null;
            try
            {
                string errorPriority = Convert.ToString(Arguments.GetArgument(0));

                ValidationLog(ref gTValidationLogger);

                if(!CheckForEqualFeederTieFeederValues())
                {
                    ValidationRuleManager validateMsg = new ValidationRuleManager();
                    validateMsg.Rule_Id = "FEED02";
                    validateMsg.BuildRuleMessage(GTClassFactory.Create<IGTApplication>(), null);

                    ErrorPriorityArray = new string[1];
                    ErrorMessageArray = new string[1];
                    ErrorPriorityArray[0] = errorPriority;
                    ErrorMessageArray[0] = validateMsg.Rule_MSG;
                }

                if (gTValidationLogger != null)
                    gTValidationLogger.LogEntry("TIMING", "END", "Feeder Validation FI Exit", "N/A", "");
            }
            catch (Exception ex)
            {
                throw new Exception("Error in Feeder Validaiton FI" + ex.Message);
            }
        }
        #endregion

        #region IGTFunctional Members
        /// <summary>
        /// Validate Logger to check the entry and exit of the FI.
        /// </summary>
        /// <param name="gTValidationLogger"></param>        
        private void ValidationLog(ref GTValidationLogger gTValidationLogger)
        {
            int fid = 0;
            string fieldValue = string.Empty;
            try
            {
                IGTComponent comp = Components[ComponentName];
                if (comp != null && comp.Recordset != null && comp.Recordset.RecordCount > 0)
                {
                    comp.Recordset.MoveFirst();

                    fid = Convert.ToInt32(comp.Recordset.Fields["G3E_FID"].Value);
                    fieldValue = Convert.ToString(comp.Recordset.Fields[FieldName].Value);
                }

                if (new gtLogHelper().CheckIfLoggingIsEnabled())
                {
                    LogEntries logEntries = new LogEntries
                    {
                        ActiveComponentName = ComponentName,
                        ActiveFID = fid,
                        ActiveFieldName = FieldName,
                        ActiveFieldValue = fieldValue,
                        JobID = DataContext.ActiveJob,
                        RelatedComponentName = "N/A",
                        RelatedFID = 0,
                        RelatedFieldName = "N/A",
                        RelatedFieldValue = "N/A",
                        ValidationInterfaceName = "Feeder Validaiton",
                        ValidationInterfaceType = "FI",
                    };
                    gTValidationLogger = new GTValidationLogger(logEntries);

                    gTValidationLogger.LogEntry("TIMING", "START", "Feeder Validaiton FI Entry", "N/A", "");
                }
            }
            catch
            {
                throw;
            }
        }
        /// <summary>
        /// Checks that features with a closed status have the same value for both Feeder ID and Tie Feeder ID.
        /// </summary>
        private bool CheckForEqualFeederTieFeederValues()
        {
            bool equal = true;
            try
            {
                IGTComponent connecComp = Components[ComponentName];

                if (connecComp != null && connecComp.Recordset != null && connecComp.Recordset.RecordCount > 0)
                {
                    connecComp.Recordset.MoveFirst();

                    if(Convert.ToString(connecComp.Recordset.Fields["STATUS_OPERATED_C"].Value).ToUpper() == "CLOSED")
                    {
                        if(!connecComp.Recordset.Fields["FEEDER_1_ID"].Value.Equals(connecComp.Recordset.Fields["FEEDER_2_ID"].Value))
                        {
                            equal = false;
                        }
                    }
                }
            }
            catch
            {
                throw;
            }
            return equal;
        }
        #endregion
    }
}
