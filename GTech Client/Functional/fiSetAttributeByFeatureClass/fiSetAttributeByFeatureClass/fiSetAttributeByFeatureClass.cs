//----------------------------------------------------------------------------+
//        Class: fiSetAttributeByFeatureClass
//  Description: This interface aets the affected attribute to a provided value based on the feature class.
//----------------------------------------------------------------------------+
//     $Author:: hkonda                                                      $
//       $Date:: 23/08/2017                                                  $
//   $Revision:: 1                                                           $
//----------------------------------------------------------------------------+
//    $History:: fiSetAttributeByFeatureClass.cs                             $
// 
// *****************  Version 1.0.0.1  *****************
// User: hkonda     Date: 08/23/2017    Time: 18:00	    Desc: Added execute method logic as per latest DDD
// User: hkonda     Date: 09/11/2017    Time: 18:00	    Desc: Updated code set orientation value when noMatchSetDefault is -1
// User: hkonda     Date: 09/15/2017    Time: 18:00	    Desc: Fix for Runtime binding reference error
//----------------------------------------------------------------------------+


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ADODB;
using Intergraph.GTechnology.API;
using Intergraph.GTechnology.Interfaces;

namespace GTechnology.Oncor.CustomAPI
{
	public class fiSetAttributeByFeatureClass : IGTFunctional
	{
		private GTArguments m_Arguments = null;
		private IGTDataContext m_DataContext = null;
		private IGTComponents m_components;
		private string m_ComponentName;
		private string m_FieldName;

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
			//
		}

		public void Execute()
		{
			short fNo = 0;
			string attributeValue;
			try
			{
				IGTComponent component = Components[ComponentName];
				ADODB.Recordset componentRs = component.Recordset;

				if (componentRs.RecordCount > 0)
				{
					componentRs.MoveFirst();

					if (componentRs.Fields["G3E_FNO"].Value == null)
					{
						return;
					}

                    fNo = Convert.ToInt16(componentRs.Fields["G3E_FNO"].Value);
					CheckForActiveFnoInValueMap(fNo, out attributeValue);
                    if (!String.IsNullOrEmpty(attributeValue))
                    {
                        componentRs.Fields["ORIENTATION_C"].Value = attributeValue;
                    }
				}
			}
			catch (Exception ex)
			{
				MessageBox.Show("Error during Set Attributes By FeatureClass execution. " + ex.Message, "G/Technology");
			}
		}

		/// <summary>
		/// Method to check if the active FNO is configured in the metadata, and get the default value for the attribute
		/// </summary>
		/// <param name="activeFno">FNO</param>
		/// <param name="attributeValue">Default value that is configured in metadata</param>
		/// <returns></returns>
		private void CheckForActiveFnoInValueMap(short activeFno, out string attributeValue)
		{
			bool matchFound = false;
			attributeValue = string.Empty;
			string valueMap = Convert.ToString(m_Arguments.GetArgument(0));
			short noMatchSetDefault = Convert.ToInt16(m_Arguments.GetArgument(1));
			string noMatchDefaultValue = Convert.ToString(m_Arguments.GetArgument(2));

			try
			{
				string[] valueMapPipeArray = valueMap.Split('|');

				if (valueMapPipeArray != null)
				{
					string[] fNoArray = null;

					foreach (string item in valueMapPipeArray)
					{
						//matchFound = true;
						fNoArray = item.Split(':')[1].Split(',');

						matchFound = fNoArray.Contains(activeFno.ToString());
						if (matchFound)
						{
							attributeValue = item.Split(':')[0];
							break;
						}
					}

					if (!matchFound)
					{
						if (noMatchSetDefault == -1)
						{
							attributeValue = noMatchDefaultValue;
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
	}
}
