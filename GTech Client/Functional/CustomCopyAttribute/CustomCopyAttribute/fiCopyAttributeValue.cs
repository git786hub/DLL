//----------------------------------------------------------------------------+
//        Class: fiCopyAttributeValue
//  Description: 
//		Primary Conductor – OH Lengths
//----------------------------------------------------------------------------+
//     $Author:: kappana                                                      $
//       $Date:: 11/05/17                                                     $
//   $Revision:: 1                                                            $
//----------------------------------------------------------------------------+
//    $History:: fiCopyAttributeValue.cs                                           $
// 
// *****************  Version 1  *****************
// User: kappana     Date: 11/05/17    Time: 18:00
// User: sagarwal    Date: 29/08/17    Time: 18:00   Description: Modified the code per the Business Rules DDD V7 to include Service Line and Duct Bank Features
// User: kappana     Date: 18/12/17    Time: 18:00   Description: Added arguments as per Business Rules DDD V8
// User: pnlella     Date: 17/05/18    Time: 12:00   Description:  Rounded to the nearest whole foot value of Actual Length.
// User: pnlella     Date: 23/05/18    Time: 12:00   Description:  Updated code to have feet to meters conversion to match the value of Actual Length to the before changed value of Graphic length
//----------------------------------------------------------------------------+

using System;
using Intergraph.GTechnology.API;
using Intergraph.GTechnology.Interfaces;
using System.Windows.Forms;

namespace GTechnology.Oncor.CustomAPI
{
    public class fiCopyActualLength : IGTFunctional
    {
		#region Private Variables
		private GTArguments m_arguments;
		private string m_componentName;
		private IGTComponents m_components;
		private IGTDataContext m_dataContext;
		private string m_fieldName;
		private IGTFieldValue m_fieldValueBeforeChange;
		private GTFunctionalTypeConstants m_type;
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
        /// Updating the ACTUAL length with graphic length for features primary Conductor OH, Primary Conductor UG and secondary conductor, Service Line, and Duct Bank
        /// </summary>
        public void Execute()
        {
            IGTComponent comp = Components[ComponentName];
			String compName = Convert.ToString(Arguments.GetArgument(0));
			String attributeName = Convert.ToString(Arguments.GetArgument(1));

			try
            {
                if (comp != null)
                {
                    int fno = Convert.ToInt32(comp.Recordset.Fields["G3E_FNO"].Value);
                    if (fno == 8 || fno == 84|| fno == 9 || fno == 85 || fno == 53 || fno == 96 || fno == 63 || fno == 97 || fno == 2200 || fno == 54 || fno == 104)
                    {
                        IGTComponent connComp = Components[compName];
                        if (m_type == GTFunctionalTypeConstants.gtftcSetValue)
                        {
                            if (connComp.Recordset.Fields[attributeName].Value.GetType() == typeof(DBNull) || Convert.ToInt32(Math.Round(Convert.ToDouble(connComp.Recordset.Fields[attributeName].Value) * 0.3048)) == Convert.ToInt32(m_fieldValueBeforeChange.FieldValue))
                            {
                                connComp.Recordset.Fields[attributeName].Value = Math.Round(Convert.ToDouble(connComp.Recordset.Fields["LENGTH_GRAPHIC_Q"].Value) * (3.280839895));
                                connComp.Recordset.Update();
                            }
                        }
                        else if(m_type == GTFunctionalTypeConstants.gtftcUpdate)
                        {
                            if(!Convert.IsDBNull(connComp.Recordset.Fields["LENGTH_GRAPHIC_Q"].Value) && (connComp.Recordset.Fields[attributeName].Value.GetType() == typeof(DBNull) || Convert.ToInt16(connComp.Recordset.Fields[attributeName].Value) == 0))
                            {
                                connComp.Recordset.Fields[attributeName].Value= Math.Round(Convert.ToDouble(connComp.Recordset.Fields["LENGTH_GRAPHIC_Q"].Value) * (3.280839895));
                                connComp.Recordset.Update();
                            }
                        }
                    }
                }
            }
			catch (Exception ex)
			{
				MessageBox.Show("Error occured in fiCopyAttributeValue Functional interface : " + ex.Message , "G/Technology");
			}
		}
        
        public void Validate(out string[] ErrorPriorityArray, out string[] ErrorMessageArray)
        {
            ErrorPriorityArray = null;
            ErrorMessageArray = null;
        }
    }
}
