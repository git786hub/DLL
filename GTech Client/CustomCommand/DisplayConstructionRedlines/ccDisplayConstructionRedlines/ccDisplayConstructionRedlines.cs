//----------------------------------------------------------------------------+
//        Class: ccDisplayConstructionRedlines
//  Description: The ability to view job-related redline files in the G/Technology map windows will be made possible through the use of the Display Construction Redlines custom command
//----------------------------------------------------------------------------+
//     $Author:: kappana                                                       $
//       $Date:: 03/01/18                                                     $
//   $Revision:: 1                                                            $
//----------------------------------------------------------------------------+
//    $History:: ccDisplayConstructionRedlines.cs                                           $
// 
// *****************  Version 1  *****************
// User: kappana     Date: 03/01/18    Time: 18:00  Desc : Created
//----------------------------------------------------------------------------+

using System;
using Intergraph.GTechnology.API;
using Intergraph.GTechnology.Interfaces;
using System.Windows.Forms;
using ADODB;
using System.Data;
using System.Data.OleDb;

namespace GTechnology.Oncor.CustomAPI
{
	public class ccDisplayConstructionRedlines : IGTCustomCommandModal
	{
		#region Variables
		private IGTApplication m_gtApplication;
		private IGTDataContext m_gtdataContext;
		private string m_strActiveJob;
		private string m_strSQL;
		private string m_strJobType;
		private Recordset m_rsDesignArea = null;
		protected IGTTransactionManager m_igtTransactionManager;
		ConstructionRedlines m_constructionRedlines;
		#endregion

		#region Properities
		public IGTTransactionManager TransactionManager
		{
			set { m_igtTransactionManager = value; }
		}
		#endregion

		#region Interface Methods
		public void Activate()
		{
			m_gtApplication = GTClassFactory.Create<IGTApplication>();
			m_gtdataContext = m_gtApplication.DataContext;
			m_strActiveJob = m_gtApplication.DataContext.ActiveJob;
			m_strSQL = "SELECT G3E_JOBTYPE FROM G3E_JOB WHERE G3E_IDENTIFIER='" + m_strActiveJob + "'";
			m_gtApplication.SetStatusBarText(GTStatusPanelConstants.gtaspcMessage, "Display Construction Redlines Command Started.");
			try
			{
				if (IsJobTypeNONWR(m_strSQL))
				{
					m_constructionRedlines = new ConstructionRedlines(GetWRRedlines());
					if (m_constructionRedlines.ShowDialog(m_gtApplication.ApplicationWindow) == DialogResult.Cancel)
					{
						ExitCommand();
						return;
					}
				}
				else
				{
					MessageBox.Show(m_gtApplication.ApplicationWindow, "This command applies only to WR jobs.", "G/Technology", MessageBoxButtons.OK, MessageBoxIcon.Warning);
				}
			}
			catch (Exception ex)
			{
				MessageBox.Show("Error during execution of Display Construction Redlines custom command" + ex.Message, "G/Technology", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
			finally
			{
				if (m_rsDesignArea != null)
				{
					if (m_rsDesignArea.State == 1)
					{
						m_rsDesignArea.Close();
						m_rsDesignArea.ActiveConnection = null;
					}
					m_rsDesignArea = null;
				}
			}
			m_gtApplication.SetStatusBarText(GTStatusPanelConstants.gtaspcMessage, "Display Construction Redlines completed.");
		}
		#endregion

		#region Methods
		/// <summary>
		/// fucntion get redline information
		/// </summary>
		/// <returns>datatable</returns>
		private DataTable GetWRRedlines()
		{
			DataTable dtRedlines;

			try
			{
				dtRedlines = new DataTable();
				m_strSQL = "SELECT SUBSTR(ng.HYPERLINK_T,INSTR(ng.HYPERLINK_T, '\\',-1)+1) FileName, ng.DESCRIPTION_T,SUBSTR(ng.HYPERLINK_T,0,INSTR(ng.HYPERLINK_T, '\\',-1)) FilePath FROM DESIGNAREA_P p,JOB_HYPERLINK_N ng WHERE p.JOB_ID='" + m_strActiveJob + "' AND p.G3E_FID=ng.G3E_FID AND ng.TYPE_C='CONREDLINE' AND ng.HYPERLINK_T IS NOT NULL";
				ExecuteCommand(m_strSQL);
				if (m_rsDesignArea != null && m_rsDesignArea.RecordCount > 0)
				{
					using (OleDbDataAdapter daComponents = new OleDbDataAdapter())
					{
						daComponents.Fill(dtRedlines, m_rsDesignArea);
						dtRedlines.Columns[0].ColumnName = "Redline File Name";
						dtRedlines.Columns[1].ColumnName = "Description";
						dtRedlines.Columns[2].ColumnName = "Redline File with full Path";
					}
				}
			}
			catch (Exception)
			{
				throw;
			}
			return dtRedlines;
		}


		/// <summary>
		/// Verify current job is NON-WR or not
		/// </summary>
		/// <param name="sSQL"></param>
		/// <returns>bool</returns>
		private bool IsJobTypeNONWR(string sSQL)
		{
			bool isNONWRJob = false;
			try
			{
				ExecuteCommand(sSQL);
				if (m_rsDesignArea.RecordCount > 0)
				{
					m_rsDesignArea.MoveFirst();
					m_strJobType = Convert.ToString(m_rsDesignArea.Fields["G3E_JOBTYPE"].Value);
					if (m_strJobType.Trim() != "NON-WR") { isNONWRJob = true; }
				}
			}
			catch (Exception)
			{
				throw;
			}
			return isNONWRJob;
		}


		/// <summary>
		/// Method to execute sql query and return the result record set
		/// </summary>
		/// <param name="sqlString"></param>
		/// <returns></returns>
		private void ExecuteCommand(string sqlString)
		{
			try
			{
				int outRecords = 0;
				ADODB.Command command = new ADODB.Command();
				command.CommandText = sqlString;
				m_rsDesignArea = m_gtdataContext.ExecuteCommand(command, out outRecords);
			}
			catch (Exception)
			{
				throw;
			}
		}

		private void ExitCommand()
		{
			try
			{
				m_gtApplication.SetStatusBarText(GTStatusPanelConstants.gtaspcMessage, "");
				m_gtApplication.EndWaitCursor();
			}
			catch (Exception)
			{
				throw;
			}
		}
		#endregion
	}
}
