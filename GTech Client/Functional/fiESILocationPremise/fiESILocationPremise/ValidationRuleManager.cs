//----------------------------------------------------------------------------+
//        Class: DisplayMessage
//  Description: DisplayMessage is common class to to display the proper messages to the user.
//----------------------------------------------------------------------------+
//     $$Author::         HCCI                                                $
//       $$Date::         11/12/2017 3.30 PM                                  $
//   $$Revision::         1                                                   $
//----------------------------------------------------------------------------+
//    $$History::         DisplayMessage.cs                                   $
//
//************************Version 1**************************
//User: Sithara    Date: 11/12/2017   Time : 3.30PM
//Created DisplayMessage.cs class
//----------------------------------------------------------------------------+
using System;
using System.Windows.Forms;
using Intergraph.GTechnology.API;
using ADODB;

namespace GTechnology.Oncor.CustomAPI
{
	public class ValidationRuleManager
	{
		private IGTApplication m_App = null;
		private IGTDataContext m_DataContext = null;
		private string _ruleid = "";
		private string _rulenm = "";
		private string _rulemsg = "";

		public string Rule_Id
		{
			get { return _ruleid; }
			set { _ruleid = value; }
		}

		public string Rule_NM
		{
			get { return _rulenm; }
			set { _rulenm = value; }
		}

		public string Rule_MSG
		{
			get { return _rulemsg; }
			set { _rulemsg = value; }
		}
        /// <summary>
        ///  To build Rule Message.
        /// </summary>
        /// <param name="oGTApplication">Current Igtapplictaion Object</param>
        /// <param name="messArguments">Substitution parameters</param>
        public void BuildRuleMessage(IGTApplication oGTApplication, object[] messArguments)
		{
			try
			{
				if (oGTApplication != null)
				{
					m_App = oGTApplication;
					m_DataContext = oGTApplication.DataContext;
					GetMessage(messArguments);
				}

			}
			catch (Exception ex)
			{
				Rule_MSG = string.Format("{0}{1}Message: {2}", ex.Message, System.Environment.NewLine, Rule_MSG);
				MessageBox.Show(Rule_MSG, "Gtechnology", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}
		/// <summary>
		/// To build Rule name.
		/// </summary>
		/// <param name="oGTApplication">Current Igtapplictaion Object</param>
		/// <param name="messArguments">Substitution parameters</param>
		public void BuildRuleName(IGTApplication oGTApplication,  object[] messArguments)
		{
			try
			{
				if (oGTApplication != null)
				{
					m_App = oGTApplication;
					m_DataContext = oGTApplication.DataContext;
					GetNMMessage(messArguments);
				}

			}
			catch (Exception ex)
			{
				Rule_MSG = string.Format("{0}{1}Message: {2}", ex.Message, System.Environment.NewLine, Rule_MSG);
				MessageBox.Show(Rule_NM, "Gtechnology", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}

		private void GetMessage(object[] Args)
		{
			string strSqlMessages = "";
			int rAffected = 0;
		
			try
			{
				strSqlMessages = "SELECT RULE_MSG FROM WR_VALIDATION_RULE WHERE RULE_ID=?";
				Recordset recordSetMessages = m_App.DataContext.Execute(strSqlMessages, out rAffected, (int)CommandTypeEnum.adCmdText, Rule_Id);

				if (recordSetMessages.EOF && recordSetMessages.BOF)
				{
					Rule_MSG = "No Messages Found!";					
				}
				else
				{
					recordSetMessages.MoveFirst();
					Rule_MSG = Convert.ToString(recordSetMessages.Fields[0].Value);	
					
					if(Args != null)
					{
						ReplaceMessageArgs(Args);
					}			
				}
			}
			catch
			{
				throw;
			}

		}

		private void GetNMMessage(object[] Args)
		{
			string strSqlMessages = "";
			int rAffected = 0;

			try
			{
				strSqlMessages = "SELECT RULE_NM FROM WR_VALIDATION_RULE WHERE RULE_ID=?";
				Recordset recordSetMessages = m_App.DataContext.Execute(strSqlMessages, out rAffected, (int)CommandTypeEnum.adCmdText, Rule_Id);

				if (recordSetMessages.EOF && recordSetMessages.BOF)
				{
					Rule_MSG = "No Messages Found!";
				}
				else
				{
					recordSetMessages.MoveFirst();
					Rule_MSG = Convert.ToString(recordSetMessages.Fields[0].Value);

					if (Args != null)
					{
						ReplaceNMMessageArgs(Args);
					}
				}
			}
			catch
			{
				throw;
			}

		}

		/// <summary>
		/// Replaces message is used to replace the message with given inputs.
		/// </summary>
		/// <param name="args"></param>
		private void ReplaceMessageArgs(object[] args)
		{
			String replaceMessage = Rule_MSG;
			try
			{
				replaceMessage = string.Format(replaceMessage, args);
			}
			catch (Exception ex)
			{
				replaceMessage = string.Format("{0}{1}Message: {2}", ex.Message, System.Environment.NewLine, replaceMessage);
			}

			Rule_MSG = replaceMessage;
		}

		/// <summary>
		/// Replaces nmmessage is used to replace the NM message with given inputs.
		/// </summary>
		/// <param name="args"></param>
		private void ReplaceNMMessageArgs(object[] args)
		{
			String replaceMessage = Rule_NM;
			try
			{
				replaceMessage = string.Format(replaceMessage, args);
			}
			catch (Exception ex)
			{
				replaceMessage = string.Format("{0}{1}Message: {2}", ex.Message, System.Environment.NewLine, replaceMessage);
			}

			Rule_NM = replaceMessage;
		}
	}
}
