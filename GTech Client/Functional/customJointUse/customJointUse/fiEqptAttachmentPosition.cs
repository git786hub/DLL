//----------------------------------------------------------------------------+
//        Class: fiEqptAttachmentPosition
//  Description: 
//		Validates attachment position on Equipment Attachment records.  If determined to be invalid, raise an error with the specified priority.
//----------------------------------------------------------------------------+
//     $Author:: kappana                                                      $
//       $Date:: 28/04/17                                                     $
//   $Revision:: 1                                                            $
//----------------------------------------------------------------------------+
//    $History:: fiEqptAttachmentPosition.cs                                           $
// *****************  Version 1  *****************
// User: kappana     Date: 28/04/17    Time: 18:00
//----------------------------------------------------------------------------+
using System;
using Intergraph.GTechnology.API;
using Intergraph.GTechnology.Interfaces;
using System.Collections.Generic;
using gtCommandLogger;

namespace GTechnology.Oncor.CustomAPI
{
    public class fiEqptAttachmentPosition : IGTFunctional
    {
		#region Private Variables
		private GTArguments m_arguments;
		private string m_componentName;
		private IGTComponents m_components;
		private IGTDataContext m_dataContext;
		private string m_fieldName;
		private IGTFieldValue m_fieldValueBeforeChange;
		private GTFunctionalTypeConstants m_type;
		#endregion

		#region Properities
		public GTArguments Arguments
		{
			get
			{
				return m_arguments;
			}

			set
			{
				m_arguments = value;
			}
		}

		public string ComponentName
		{
			get
			{
				return m_componentName;
			}

			set
			{
				m_componentName = value;
			}
		}

		public IGTComponents Components
		{
			get
			{
				return m_components;
			}

			set
			{
				m_components = value;
			}
		}

		public IGTDataContext DataContext
		{
			get
			{
				return m_dataContext;
			}

			set
			{
				m_dataContext = value;
			}
		}

		public string FieldName
		{
			get
			{
				return m_fieldName;
			}

			set
			{
				m_fieldName = value;
			}
		}

		public IGTFieldValue FieldValueBeforeChange
		{
			get
			{
				return m_fieldValueBeforeChange;
			}

			set
			{
				m_fieldValueBeforeChange = value;
			}
		}

		public GTFunctionalTypeConstants Type
		{
			get
			{
				return m_type;
			}

			set
			{
				m_type = value;
			}
		}

		#endregion


		public void Delete()
        {
            
        }

        public void Execute()
        {
            
        }

        public void Validate(out string[] ErrorPriorityArray, out string[] ErrorMessageArray)
        {
			List<string> lstErrorMsg = new List<string>();
			List<string> lstErrorPriority = new List<string>();
			ValidationRuleManager validateMsg = new ValidationRuleManager();
			
			ErrorMessageArray = null;
            ErrorPriorityArray = null;

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
                    ValidationInterfaceName = "Equipment Attachment Position",
                    ValidationInterfaceType = "FI",
                };
                gTValidationLogger = new GTValidationLogger(logEntries);

                gTValidationLogger.LogEntry("TIMING", "START", "Equipment Attachment Position Entry", "N/A", "");
            }

            try
            {             
                string strAttType, strAttPosition;

                if (comp != null)
                {
                    //comp.Recordset.MoveFirst();
                    strAttType = comp.Recordset.Fields["E_ATTACH_TYPE_C"].Value.ToString();
                    strAttPosition = comp.Recordset.Fields["E_ATTACH_POSITION_C"].Value.ToString();

                    if (strAttPosition == "Top of Pole" && strAttType != "Antenna")
                    {
						validateMsg.Rule_Id = "EQAP01";
						validateMsg.BuildRuleMessage(GTClassFactory.Create<IGTApplication>(), null);

						lstErrorMsg.Add(validateMsg.Rule_MSG);
						//lstErrorPriority.Add("P1");
						lstErrorPriority.Add(Arguments.GetArgument(0).ToString());
					}
                }

				ErrorMessageArray = lstErrorMsg.ToArray();
				ErrorPriorityArray = lstErrorPriority.ToArray();

                if (gTValidationLogger != null)
                    gTValidationLogger.LogEntry("TIMING", "END", "Equipment Attachment Position Exit", "N/A", "");
            }
            catch (Exception ex)
            {
				throw new Exception("Error during Attachment Position Validation" + ex.Message);
			}            
        }
    }
}
