//----------------------------------------------------------------------------+
//        Class: ccAttachJobDocument
//  Description: This command allows a user to select a file from the available file system and attach it as hyperlinked attachment to the active job’s Design Area.
//----------------------------------------------------------------------------+
//     $Author:: kappana                                                       $
//       $Date:: 10/12/17                                                     $
//   $Revision:: 1                                                            $
//----------------------------------------------------------------------------+
//    $History:: ccAttachJobDocument.cs                                           $
// 
// *****************  Version 1  *****************
// User: kappana     Date: 10/12/17    Time: 18:00  Desc : Created
//----------------------------------------------------------------------------+


using System;
using Intergraph.GTechnology.API;
using Intergraph.GTechnology.Interfaces;
using ADODB;
using System.Windows.Forms;

namespace GTechnology.Oncor.CustomAPI
{
	public class ccAttachJobDocument : IGTCustomCommandModal
	{
		#region Variables

		private IGTApplication m_gtApplication;
		private AttachJobDocument m_attachJobDocument;
		private IGTDataContext m_dataContext;
		private string m_strActiveJob, m_strSQL, m_strDASQL, m_strJobType;
		private CommonMessages m_commonMessages;
		private Recordset m_rsDesignArea = null;
		#endregion Variables

		#region Properities
		protected IGTTransactionManager m_ManageTransactions;
		public IGTTransactionManager TransactionManager
		{
			set { m_ManageTransactions = value; }
		}

		#endregion Properities

		#region Methods
		/// <summary>
		/// Get active Job Design Area and attached the document
		/// </summary>
		public void Activate()
		{
			
			try
			{
				m_commonMessages = new CommonMessages();
				m_gtApplication = GTClassFactory.Create<IGTApplication>();
				m_dataContext = m_gtApplication.DataContext;
				m_strActiveJob = m_gtApplication.DataContext.ActiveJob;
				m_strSQL = "SELECT G3E_JOBTYPE FROM G3E_JOB WHERE G3E_IDENTIFIER='" + m_strActiveJob + "'";
				m_strDASQL = "SELECT G3E_FID FROM DESIGNAREA_P WHERE JOB_ID='" + m_strActiveJob + "'";
				
				if (IsJobTypeNONWR(m_strSQL))
				{
					//Get the active job design area
					ExecuteCommand(m_strDASQL);
					if (m_rsDesignArea.RecordCount <= 0)
					{
						MessageBox.Show(m_gtApplication.ApplicationWindow,m_commonMessages.NoDesignArea, "G/Technology",MessageBoxButtons.OK,MessageBoxIcon.Warning);
					}
					else if (m_rsDesignArea.RecordCount > 1)
					{
						MessageBox.Show(m_gtApplication.ApplicationWindow,m_commonMessages.MoreDesignAreas, "G/Technology", MessageBoxButtons.OK, MessageBoxIcon.Warning);
					}
					else
					{
						if (m_rsDesignArea.RecordCount > 0)
						{
							m_rsDesignArea.MoveFirst();
							m_attachJobDocument = new AttachJobDocument(m_gtApplication, Convert.ToInt32(m_rsDesignArea.Fields["G3E_FID"].Value), m_ManageTransactions);
							m_attachJobDocument.ShowDialog(m_gtApplication.ApplicationWindow);
						}
					}
				}
				else
				{
					MessageBox.Show(m_gtApplication.ApplicationWindow,m_commonMessages.WRMessage, "G/Technology", MessageBoxButtons.OK, MessageBoxIcon.Warning);
				}

			}
			catch (Exception ex)
			{
				MessageBox.Show(m_gtApplication.ApplicationWindow,"Error " + ex.Message, "G/Technology", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
				if (m_attachJobDocument != null) { m_attachJobDocument.Close(); m_attachJobDocument.Dispose(); m_attachJobDocument = null; }

			}
		}

		/// <summary>
		/// Verify current job is NON-WR or not
		/// </summary>
		/// <param name="sSQL"></param>
		/// <returns></returns>
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
				m_rsDesignArea = m_dataContext.ExecuteCommand(command, out outRecords);
			}
			catch (Exception)
			{
				throw;
			}
		}
	}
	#endregion Methods
}
