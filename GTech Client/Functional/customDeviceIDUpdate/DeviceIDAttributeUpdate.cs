//----------------------------------------------------------------------------+
//        Class: DeviceIDAttributeUpdate
//  Description: 
//           Create a trigger that will populate the last 4 digits of the G3E_FID as the DEVICE ID.
//                  Update the tool tip to contain the following attributes each on a separate line.  We will apply this to all features that have DEVICE ID attribute.  
//                  [PS: SEE IF FUNCTIONAL INTERFACE MAKES MORE SENSE HERE BECAUSE THE UPDATES ARE INSTANT]
//----------------------------------------------------------------------------+
//     $Author:: Uma Avote                                                      $
//       $Date:: 05/05/2017                                                     $
//   $Revision:: 1                                                            $
//----------------------------------------------------------------------------+
//    $History:: DeviceIDAttributeUpdate.cs                                           $
// 
// *****************  Version 1  *****************
// User: Uma     Date: 05/05/2017     Time: 18:00
// *****************  Version 1.0.0.2  *****************
// User: kappana     Date: 07/21/2017    Time: 18:00	Desc: Updated the component "Conectivity" to "scada attributes" as part of JIRA 712
//----------------------------------------------------------------------------+
using System;
using Intergraph.GTechnology.Interfaces;
using Intergraph.GTechnology.API;
using ADODB;
using System.Windows.Forms;

namespace GTechnology.Oncor.CustomAPI
{ 
    public class fiSetAttributeBySequence : IGTFunctional
    {
        #region IGTFunctional Members
        
        //constant
        public const string Caption = "G/Technology";

		#region Private Variables
		private GTArguments m_arguments;
		private string m_componentName;
		private IGTComponents m_components;
		private IGTDataContext m_dataContext;
		private string m_fieldName;
		private IGTFieldValue m_fieldValueBeforeChange;
		private GTFunctionalTypeConstants m_type;
        private string m_seqName = string.Empty;

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
                m_seqName = Convert.ToString(m_arguments.GetArgument(0));

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

		/// <summary>
		/// When a feature is created with a SCADA Attributes component, this interface sets the Device ID attribute to the next available unique value
		/// </summary>
		public void Execute()
        {
            IGTApplication _gtApp = (IGTApplication)GTClassFactory.Create<IGTApplication>();

            _gtApp.SetStatusBarText(GTStatusPanelConstants.gtaspcMessage, "Device ID Attribute Update FI");

            IGTComponent comp = Components[ComponentName];
			try
			{
				if (comp != null)
				{
					Recordset rs = null;
					int i = 0;
					comp.Recordset.MoveFirst();
					rs = _gtApp.DataContext.Execute("SELECT " + m_seqName + ".NEXTVAL FROM DUAL", out i, (int)CommandTypeEnum.adCmdText);

					if (rs != null && rs.RecordCount > 0)
					{
						comp.Recordset.Fields[m_fieldName].Value = Convert.ToString(rs.Fields[0].Value);
						comp.Recordset.Update();
					}

				}
			}
			catch (Exception ex)
			{
				MessageBox.Show("Error occured in fiSetAttributeBySequence FI " + ex.Message, Caption);

			}
			finally
			{
				// comp = null;
			}           
        } 

		/// <summary>
		/// 
		/// </summary>
		/// <param name="ErrorPriorityArray"></param>
		/// <param name="ErrorMessageArray"></param>
        public void Validate(out string[] ErrorPriorityArray, out string[] ErrorMessageArray)
        {
            ErrorPriorityArray = null;
            ErrorMessageArray = null;
        }

        #endregion
    }
}
