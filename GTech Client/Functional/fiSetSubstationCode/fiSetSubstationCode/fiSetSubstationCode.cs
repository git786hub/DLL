//----------------------------------------------------------------------------+
//        Class: fiSetSubstationCode
//  Description: This interface sets the proper Substation Code for a feature when the Feeder ID is set.
//----------------------------------------------------------------------------+
//     $Author:: hkonda                                                       $
//       $Date:: 18/08/17                                                     $
//   $Revision:: 1                                                            $
//----------------------------------------------------------------------------+
//    $History:: fiSetSubstationCode.cs                                       $
// 
// *****************  Version 1  *****************
// User: hkonda     Date: 18/08/17    Time: 18:00
// User: hkonda     Date: 09/09/17    Time: 18:00  Desc : Fixed recordset issue during sanity testing
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
	public class fiSetSubstationCode : IGTFunctional
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
		}

		public void Execute()
		{
			short fNo = 0;
			Recordset connectivityComponentRs = null;
			string feederId = string.Empty;
			string subStationCode = string.Empty;
			try
			{
				IGTComponent component = Components.GetComponent(11);
				connectivityComponentRs = component.Recordset;
				if (connectivityComponentRs.RecordCount > 0)
				{
					connectivityComponentRs.MoveFirst();
					fNo = Convert.ToInt16(connectivityComponentRs.Fields["G3E_FNO"].Value);
				}
				if (fNo == 16)  // If affected feature is substation breaker then do nothing
				{
					return;
				}
				feederId = Convert.ToString(connectivityComponentRs.Fields["FEEDER_1_ID"].Value);
				ADODB.Command command = null;
				int outRecords = 0;
				string sqlString = string.Format("select distinct ssta_c SSCODE from CONNECTIVITY_N where g3e_fno = 16 and feeder_1_id = '{0}' ", feederId);

				command = new ADODB.Command();
				command.CommandText = sqlString;
				ADODB.Recordset results = DataContext.ExecuteCommand(command, out outRecords);
				if (results.RecordCount > 0)
				{
					results.MoveFirst();
					subStationCode = Convert.ToString(results.Fields["SSCODE"].Value);
				}
            
				connectivityComponentRs.Fields["SSTA_C"].Value = subStationCode;
			}
			catch (Exception ex)
			{
				MessageBox.Show("Error during Set Substation Code execution. " + ex.Message, "G/Technology");
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
