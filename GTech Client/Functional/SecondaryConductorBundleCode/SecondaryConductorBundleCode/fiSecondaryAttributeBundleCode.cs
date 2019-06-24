//----------------------------------------------------------------------------+
//        Class: fiSecondaryAttributeBundleCode
//  Description: This interface sets a hidden Bundle attribute on the Secondary Conductor component whenever a Secondary Wire component is added, modified, or deleted, 
//				 using the full set of wire-level Bundle values to determine a value for the component-level attribute.
//----------------------------------------------------------------------------+
//     $Author:: hkonda                                                       $
//       $Date:: 04/08/17                                                     $
//   $Revision:: 1                                                            $
//----------------------------------------------------------------------------+
//    $History:: fiSecondaryAttributeBundleCode.cs                                           $
// 
// *****************  Version 1  *****************
// User: hkonda     Date: 25/09/17    Time: 18:00  Desc : Created
// User: hkonda     Date: 17/10/17    Time: 18:00  Desc : Added delete logic
//----------------------------------------------------------------------------+
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using ADODB;
using Intergraph.GTechnology.API;
using Intergraph.GTechnology.Interfaces;

namespace GTechnology.Oncor.CustomAPI
{
	public class fiSecondaryAttributeBundleCode : IGTFunctional
	{
		private GTArguments m_Arguments = null;
		private IGTDataContext m_DataContext = null;
		private IGTComponents m_components;
		private string m_ComponentName;
		private string m_FieldName;
		private bool m_isDelete = false;

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
            try
            {
                m_isDelete = true;
                ProcessBundleCodes();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error during Secondary Conductor Bundle Code FI execution. " + ex.Message, "G/Technology");
            }
        }

		public void Execute()
		{
			try
			{
				m_isDelete = false;
				ProcessBundleCodes();
			}
			catch (Exception ex)
			{
				MessageBox.Show("Error during Secondary Conductor Bundle Code FI execution. " + ex.Message, "G/Technology");
			}
		}

		private void ProcessBundleCodes()
		{
			try
			{
				Recordset secondaryWireComponentRs = Components[ComponentName].Recordset;
				Recordset secondaryConductorComponentRs = Components.GetComponent(5301).Recordset;

				List<string> bundleCodes = new List<string>();

				if (secondaryWireComponentRs != null && secondaryWireComponentRs.RecordCount > 0)
				{
					string tempBundleCode = Convert.ToString(secondaryWireComponentRs.Fields["BUNDLE_C"].Value);
					secondaryWireComponentRs.MoveFirst();
					while (!secondaryWireComponentRs.EOF)
					{
						bundleCodes.Add(Convert.ToString(secondaryWireComponentRs.Fields["BUNDLE_C"].Value));
						secondaryWireComponentRs.MoveNext();
					}
					if (m_isDelete)
					{
						bundleCodes.Remove(tempBundleCode);
					}
				}

				if (bundleCodes.Count > 0)
				{
					secondaryConductorComponentRs.MoveFirst();
					if ((bundleCodes.Contains("D") && bundleCodes.Contains("T")) || (bundleCodes.Contains("D") && bundleCodes.Contains("Q")) || (bundleCodes.Contains("T") && bundleCodes.Contains("Q"))
						|| (bundleCodes.Contains("D") && bundleCodes.Contains("T") && bundleCodes.Contains("Q"))
						|| (bundleCodes.Contains("D") && bundleCodes.Contains("T") && bundleCodes.Contains("Q") && bundleCodes.Contains("N")))
					{
						secondaryConductorComponentRs.Fields["BUNDLE_C"].Value = "D";  // If there are multiple values from the set [D, T Q] then set a value from the Set arbitrarily. Setting "D" as arbitrary value.
					}

					else
					{
						SetBundleCode(secondaryConductorComponentRs, bundleCodes);
					}
				}
			}
			catch (Exception)
			{
				throw;
			}
		}

		/// <summary>
		/// Method to set the bundle code when the combination of bundle codes for wires does not include all the three value from the set [D, T, Q]
		/// </summary>
		/// <param name="secondaryConductorComponentRs"></param>
		/// <param name="bundleCodes"></param>

		private void SetBundleCode(Recordset secondaryConductorComponentRs, List<string> bundleCodes)
		{
			int neutralCount = bundleCodes.Where(c => c == "N").Count();
			int dTypeCount = bundleCodes.Where(c => c == "D").Count();
			int tTypeCount = bundleCodes.Where(c => c == "T").Count();
			int qTypeCount = bundleCodes.Where(c => c == "Q").Count();

			if (neutralCount == bundleCodes.Count)  // All wires are Neutral
			{
				secondaryConductorComponentRs.Fields["BUNDLE_C"].Value = "N";
			}

			else if (dTypeCount == bundleCodes.Count || (dTypeCount != 0 && (qTypeCount == 0 && tTypeCount == 0)) || (bundleCodes.Contains("D") && bundleCodes.Contains("N")))
			{
				secondaryConductorComponentRs.Fields["BUNDLE_C"].Value = "D";
			}

			else if (tTypeCount == bundleCodes.Count || (tTypeCount != 0 && (qTypeCount == 0 && dTypeCount == 0)) || (bundleCodes.Contains("T") && bundleCodes.Contains("N")))
			{
				secondaryConductorComponentRs.Fields["BUNDLE_C"].Value = "T";
			}

			else if (qTypeCount == bundleCodes.Count || (qTypeCount != 0 && (dTypeCount == 0 && tTypeCount == 0)) || (bundleCodes.Contains("Q") && bundleCodes.Contains("N")))
			{
				secondaryConductorComponentRs.Fields["BUNDLE_C"].Value = "Q";
			}

			else // If its neutral wires and empty codes OR all empty codes
			{
				secondaryConductorComponentRs.Fields["BUNDLE_C"].Value = "O";
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
