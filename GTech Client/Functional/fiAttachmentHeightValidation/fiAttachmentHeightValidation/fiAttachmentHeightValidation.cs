//----------------------------------------------------------------------------+
//        Class: fiAttachmentHeightValidation
//  Description: Validates a feature to check that the entered value for Attachment Height is formatted correctly in feet and inches.
//----------------------------------------------------------------------------+
//     $Author:: hkonda                                                       $
//       $Date:: 27/07/17                                                     $
//   $Revision:: 1                                                            $
//----------------------------------------------------------------------------+
//    $History:: fiAttachmentHeightValidation.cs                              $
// 
// *****************  Version 1  *****************
// User: hkonda     Date: 27/07/17    Time: 18:00
// User: hkonda     Date: 21/09/17    Time: 18:00  Desc: Removed validation for feets, only inches are validated 
// User: pnlella    Date: 13/10/17    Time: 15:00  Desc: Modified the P1 validation message as per the requirement of JIRA 906 
//----------------------------------------------------------------------------+

using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using ADODB;
using Intergraph.GTechnology.API;
using Intergraph.GTechnology.Interfaces;
using gtCommandLogger;

namespace GTechnology.Oncor.CustomAPI
{
    public class fiAttachmentHeightValidation : IGTFunctional
	{
		private GTArguments m_Arguments = null;
		private IGTDataContext m_DataContext = null;
		private IGTComponents m_components;
		private string m_ComponentName;
		private string m_FieldName;
		private string m_AttachmentHeight = string.Empty;

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
			Regex regex = new Regex("^\\d+'\\d+\"$"); // Regular expression for height format - ex 5'6" 

			ErrorPriorityArray = null;
			ErrorMessageArray = null;
			List<string> errorMsg = new List<string>();
			List<string> errorPriority = new List<string>();
			IGTComponent component = Components[ComponentName];
			Recordset componentRecordSet = component.Recordset;
			ValidationRuleManager validateMsg = new ValidationRuleManager();

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
                    ValidationInterfaceName = "Height Validation",
                    ValidationInterfaceType = "FI",
                };
                gTValidationLogger = new GTValidationLogger(logEntries);

                gTValidationLogger.LogEntry("TIMING", "START", "Height Validation Entry", "N/A", "");
            }



            try
			{
				if (componentRecordSet != null)
				{
					if (componentRecordSet.RecordCount > 0)
					{
						m_AttachmentHeight = Convert.ToString(componentRecordSet.Fields[m_FieldName].Value);
						if (!string.IsNullOrEmpty(m_AttachmentHeight))
						{
							Match match = regex.Match(m_AttachmentHeight);
							if (match.Success)
							{
								if (!CheckHeightRange(m_AttachmentHeight))
								{
									validateMsg.Rule_Id = "HGT01";
									validateMsg.BuildRuleMessage(GTClassFactory.Create<IGTApplication>(), null);
									errorMsg.Add(validateMsg.Rule_MSG);
									errorPriority.Add(Convert.ToString(m_Arguments.GetArgument(0)));
								}
							}
							else
							{
								validateMsg.Rule_Id = "HGT01";
								validateMsg.BuildRuleMessage(GTClassFactory.Create<IGTApplication>(), null);
								errorMsg.Add(validateMsg.Rule_MSG);
								errorPriority.Add(Convert.ToString(m_Arguments.GetArgument(0)));
							}
						}
					}

					if (errorMsg.Count > 0)
					{
						ErrorPriorityArray = errorPriority.ToArray();
						ErrorMessageArray = errorMsg.ToArray();
					}
				}

                if (gTValidationLogger != null)
                    gTValidationLogger.LogEntry("TIMING", "END", "Height Validation Exit", "N/A", "");
            }
			catch (Exception ex)
			{
				throw new Exception("Error during Attachment Height Validation" + ex.Message);
			}
		}

		/// <summary>
		/// Method to check the input height range.
		/// </summary>
		/// <param name="height">Height of the attachment, ex: 5'6"</param>
		/// <returns>True if height is in proper format and within valid range, i.e., 0 and 11</returns>

		private bool CheckHeightRange(string height)
		{
			try
			{
				string[] heights = height.Split('\'');

				if (Convert.ToInt16(heights[1].Split('\"')[0]) < 0 || Convert.ToInt16(heights[1].Split('\"')[0]) > 11)
				{
					return false;
				}

				return true;
			}
			catch (Exception)
			{
				throw;
			}
		}
	}
}
