//----------------------------------------------------------------------------+
//        Class: fiWorkPointNumbering
//  Description: Provides a functional interface to Validate Work Point numbering with respect to the rest of the WR.
//----------------------------------------------------------------------------+
//     $Author:: Prathyusha Lella (pnlella)                                                      $
//       $Date:: 4/12/17                                                     $
//   $Revision:: 1                                                            $
//----------------------------------------------------------------------------+
//    $History::                                         $
// 
// *****************  Version 1  *****************
// User: pnlella     Date: 4/12/17    Time: 15:00
//----------------------------------------------------------------------------+

using System;
using ADODB;
using Intergraph.GTechnology.API;
using Intergraph.GTechnology.Interfaces;
using gtCommandLogger;


namespace GTechnology.Oncor.CustomAPI
{
    public class fiWorkPointNumbering : IGTFunctional
    {
        #region Private Members
        GTArguments m_gtArguments = null;
        string m_gtComponent = null;
        IGTComponents m_gtComponentCollection = null;
        IGTDataContext m_gtDataContext = null;
        string m_sFieldName = string.Empty;
        IGTFieldValue m_gtFieldValue = null;
        GTFunctionalTypeConstants m_gtFunctionalTypeConstant;

        #endregion

        #region IGTFunctional Members
        public GTArguments Arguments
        {
            get
            {
                return m_gtArguments;
            }

            set
            {
                m_gtArguments = value;
            }
        }

        public string ComponentName
        {
            get
            {
                return m_gtComponent;
            }

            set
            {
                m_gtComponent = value;
            }
        }

        public IGTComponents Components
        {
            get
            {
                return m_gtComponentCollection;
            }

            set
            {
                m_gtComponentCollection = value;
            }
        }

        public IGTDataContext DataContext
        {
            get
            {
                return m_gtDataContext;
            }

            set
            {
                m_gtDataContext = value;
            }
        }

        public string FieldName
        {
            get
            {
                return m_sFieldName;
            }

            set
            {
                m_sFieldName = value;
            }
        }

        public IGTFieldValue FieldValueBeforeChange
        {
            get
            {
                return m_gtFieldValue;
            }

            set
            {
                m_gtFieldValue = value;
            }
        }

        public GTFunctionalTypeConstants Type
        {
            get
            {
                return m_gtFunctionalTypeConstant;
            }

            set
            {
                m_gtFunctionalTypeConstant = value;
            }
        }

        public void Delete()
        {

        }

        public void Execute()
        {

        }

        /// <summary>
        /// Validate
        /// </summary>
        /// <param name="ErrorPriorityArray"></param>
        /// <param name="ErrorMessageArray"></param>
        public void Validate(out string[] ErrorPriorityArray, out string[] ErrorMessageArray)
        {
            Recordset workPointRS = null;

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
                    ValidationInterfaceName = "Work Point Numbering",
                    ValidationInterfaceType = "FI",
                };
                gTValidationLogger = new GTValidationLogger(logEntries);

                gTValidationLogger.LogEntry("TIMING", "START", "Work Point Numbering Entry", "N/A", "");
            }

            try
            {
                string errorPriority = Convert.ToString(Arguments.GetArgument(0));
                object workPointNbr = m_gtComponentCollection[m_gtComponent].Recordset.Fields[m_sFieldName].Value == DBNull.Value ? null : m_gtComponentCollection[m_gtComponent].Recordset.Fields[m_sFieldName].Value;
                object workRequestNbr= m_gtComponentCollection[m_gtComponent].Recordset.Fields["WR_NBR"].Value == DBNull.Value ? null : m_gtComponentCollection[m_gtComponent].Recordset.Fields["WR_NBR"].Value;
                ErrorPriorityArray = new string[2];
                ErrorMessageArray = new string[2];

                ValidationRuleManager validateMsg = new ValidationRuleManager();
                if (workPointNbr != null)
                {
                    if (Convert.ToInt32(workPointNbr) <= 0)
                    {
                        ErrorPriorityArray[0] = errorPriority;
                        validateMsg.Rule_Id = "JM3";
                        validateMsg.BuildRuleMessage(GTClassFactory.Create<IGTApplication>(), null);
                        ErrorMessageArray[0] = validateMsg.Rule_MSG;
                    }
                    else
                    {
                        workPointRS = ExecuteCommand(string.Format("Select WP_NBR,WR_NBR from WORKPOINT_N"));
                        workPointRS.Filter = "WP_NBR = " + workPointNbr + " AND WR_NBR='"+ workRequestNbr +"'";
                        if (workPointRS != null && workPointRS.RecordCount > 1)
                        {
                            ErrorPriorityArray[1] = errorPriority;
                            validateMsg.Rule_Id = "JM4";
                            validateMsg.BuildRuleMessage(GTClassFactory.Create<IGTApplication>(), null);
                            ErrorMessageArray[1] = validateMsg.Rule_MSG;
                        }
                    }
                }

                if(gTValidationLogger != null)
                  gTValidationLogger.LogEntry("TIMING", "END", "Work Point Numbering Exit", "N/A", "");
            }
            catch (Exception ex)
            {
                throw new Exception("Error during Work Point Numbering Validation" + ex.Message);
            }
            finally
            {
                workPointRS = null;
            }
        }
        #endregion

        #region Private Methods
        /// <summary>
        /// Method to execute sql query and return the result record set
        /// </summary>
        /// <param name="sqlString"></param>
        /// <returns></returns>
        private Recordset ExecuteCommand(string sqlString)
        {
            Recordset results = null;
            try
            {
                int outRecords = 0;
                Command command = new Command();
                command.CommandText = sqlString;
                results = DataContext.ExecuteCommand(command, out outRecords);
            }
            catch (Exception)
            {
                throw;
            }
            return results;
        }
        #endregion
    }
}
