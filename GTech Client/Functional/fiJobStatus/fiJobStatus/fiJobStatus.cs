//----------------------------------------------------------------------------+
//        Class: fiJobStatus
//  Description: This interface sets the job status to DESIGN when jobs status of active job is PENDING APPROVAL.
//----------------------------------------------------------------------------+
//     $Author:: hkonda                                                       $
//       $Date:: 04/08/17                                                     $
//   $Revision:: 1                                                            $
//----------------------------------------------------------------------------+
//    $History:: fiJobStatus.cs                                           $
// 
// *****************  Version 1  *****************
// User: hkonda     Date: 21/09/17    Time: 18:00  Desc : Created
// User: hkonda     Date: 29/11/17    Time: 18:00  Desc : Changed column name from g3e_status to g3e_jobstatus

//----------------------------------------------------------------------------+


using System;
using System.Windows.Forms;
using ADODB;
using Intergraph.GTechnology.API;
using Intergraph.GTechnology.Interfaces;
using System.Linq;

namespace GTechnology.Oncor.CustomAPI
{
	public class fiJobStatus : IGTFunctional
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
			try
			{
				string jobId = DataContext.ActiveJob;
				string jobStatus = string.Empty;
				Recordset jobInfoRs = ExecuteCommand(string.Format("select G3E_JOBSTATUS from g3e_job where g3e_identifier  = '{0}'", jobId));
				if (jobInfoRs.RecordCount > 0)
				{
					jobInfoRs.MoveFirst();
					jobStatus = Convert.ToString(jobInfoRs.Fields["G3E_JOBSTATUS"].Value);

					if (jobStatus.ToUpper() == "APPROVALPENDING")
					{
						ExecuteCommand(string.Format("update g3e_job set G3E_JOBSTATUS = 'Design' where g3e_identifier = '{0}'", jobId));
					}
				}
			}
			catch (Exception ex)
			{
				MessageBox.Show("Error during Job Status FI execution. " + ex.Message, "G/Technology");
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
		/// Method to execute sql query and return the result record set
		/// </summary>
		/// <param name="sqlString"></param>
		/// <returns></returns>
		private Recordset ExecuteCommand(string sqlString)
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
	}
}


