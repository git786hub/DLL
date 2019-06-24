using Intergraph.GTechnology.API;
using Intergraph.GTechnology.Interfaces;
using System.Windows.Forms;
using System;

namespace GTechnology.Oncor.CustomAPI
{
	public class ccUpdateTrace : IGTCustomCommandModeless
	{
		private IGTTransactionManager m_TransactionManager;
		private IGTApplication m_Application = GTClassFactory.Create<IGTApplication>();

		private const int CUSTOM_COMMAND_NUMBER = 42;
		private const string CUSTOM_COMMAND_NAME = "Update Trace";

		public bool CanTerminate
		{
			get
			{
				return true;
			}
		}

		public IGTTransactionManager TransactionManager
		{
			set
			{
				m_TransactionManager = value;
			}
		}

		/// <summary>
		/// The entry point for the custom command.
		/// </summary>
		/// <param name="CustomCommandHelper">Provides notification to the system that the command has finished</param>
		public void Activate(IGTCustomCommandHelper CustomCommandHelper)
		{
			try
			{
				m_Application.BeginWaitCursor();
				// Get feature number and feature identifier for feature in select set
				IGTDDCKeyObjects oGTDCKeys = GTClassFactory.Create<IGTDDCKeyObjects>();
				oGTDCKeys = m_Application.SelectedObjects.GetObjects();
				short fno = oGTDCKeys[0].FNO;
				int fid = oGTDCKeys[0].FID;

				// Call execute method on UpdateTrace object to run trace and process results
				UpdateTrace updateTrace = new UpdateTrace(CUSTOM_COMMAND_NUMBER, CUSTOM_COMMAND_NAME);

				m_TransactionManager.Begin("Update Trace");

				if(updateTrace.Execute(fno, fid))
				{
					m_TransactionManager.Commit();
				}
				else
				{
					m_TransactionManager.Rollback();
				}

				updateTrace = null;
			}
			catch(Exception ex)
			{
				MessageBox.Show(m_Application.ApplicationWindow, "ccUpdateTrace.Activate: Error calling UpdateTrace - " + ex.Message,
												"G /Technology", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
			}
			m_Application.EndWaitCursor();

			CustomCommandHelper.Complete();
		}

		public void Pause()
		{
			return;
		}

		public void Resume()
		{
			return;
		}

		public void Terminate()
		{
			m_Application = null;
			m_TransactionManager = null;
		}
	}
}
