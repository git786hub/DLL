//----------------------------------------------------------------------------+
//        Class: fiSetDateOnValueMatch
//  Description: 
//		Custom FI: Set Date on Value Match.
//----------------------------------------------------------------------------+
//     $Author:: kappana                                                      $
//       $Date:: 28/04/17                                                     $
//   $Revision:: 1                                                            $
//----------------------------------------------------------------------------+
//    $History:: fiSetDateOnValueMatch.cs                                           $
// 
//----------------------------------------------------------------------------+
using System;
using Intergraph.GTechnology.API;
using Intergraph.GTechnology.Interfaces;
using ADODB;
using System.Windows.Forms;
namespace GTechnology.Oncor.CustomAPI
{
    class fiSetDateOnValueMatch : IGTFunctional
    {
		#region Private Variables
		private GTArguments m_arguments;
		private string m_componentName;
		private IGTComponents m_components;
		private IGTDataContext m_dataContext;
		private string m_fieldName;
		private IGTFieldValue m_fieldValueBeforeChange;
		private GTFunctionalTypeConstants m_type;
        private string m_valuesToMatch = string.Empty;
        private string m_anosToUpdate = string.Empty;

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
                m_valuesToMatch = Convert.ToString(m_arguments.GetArgument(0));
                m_anosToUpdate = Convert.ToString(m_arguments.GetArgument(1));


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
		/// Validation
		/// </summary>
        public void Execute()
        {
            try
            {
                IGTApplication gtApp = (IGTApplication)GTClassFactory.Create<IGTApplication>();
                IGTComponent comp = Components[ComponentName];

                if (comp != null)
                {
                    string strAttStatus = Convert.ToString(comp.Recordset.Fields[m_fieldName].Value).ToUpper();

                    if (m_valuesToMatch.Contains(strAttStatus)) //If attribute value is found in the configured argument then proceed, otherwise do not do anything
                    {
                        string[] arrValuesToMatch = m_valuesToMatch.Split(',');
                        int index = -1;
                                      
                        //ToDo - Replace it with LINQ query          
                        for (int i = 0; i < arrValuesToMatch.Length; i++)
                        {
                            if (arrValuesToMatch[i].Equals(strAttStatus))
                            {
                                index = i;
                                break;
                            }
                        }

                        if (index!=-1)
                        {
                            string fieldName = string.Empty;

                            int anoToUpdate = Convert.ToInt32(m_anosToUpdate.Split(',')[index]);
                            ADODB.Recordset rs = gtApp.DataContext.MetadataRecordset("G3E_ATTRIBUTEINFO_OPTABLE", "G3E_ANO = " + anoToUpdate);
                           // rs.Filter = "G3E_ANO = " + anoToUpdate;
                            rs.MoveFirst();

                            fieldName = Convert.ToString(rs.Fields["G3E_FIELD"].Value);
                            comp.Recordset.Fields[fieldName].Value = GetCurrentDate(gtApp);
                            comp.Recordset.Update();
                        }
                    }
                }

     //           if (comp.CNO == 35)
     //               {
     //                   string strAttStatus = Convert.ToString(comp.Recordset.Fields["E_ATTACHMENT_STATUS"].Value).ToUpper();
                   
     //                   if (strAttStatus == "REMOVED")
     //                   {
     //                       comp.Recordset.Fields["E_REMOVED_D"].Value = GetCurrentDate(_gtApp);
     //                   }
     //                   else if (strAttStatus == "ACTIVE")
     //                   {
     //                       comp.Recordset.Fields["E_ATTACHED_D"].Value = GetCurrentDate(_gtApp);
     //                   }
     //                   else if (strAttStatus == "ABANDONED")
     //                   {
     //                       comp.Recordset.Fields["E_ABANDON_D"].Value = GetCurrentDate(_gtApp);
     //                   }
					//	comp.Recordset.Update();
					//}
     //               else if (comp.CNO == 34)
     //               {
     //                   string strAttStatus = Convert.ToString(comp.Recordset.Fields["W_ATTACHMENT_STATUS"].Value).ToUpper();
     //                   if (strAttStatus == "REMOVED")
     //                   {
     //                       comp.Recordset.Fields["W_REMOVED_D"].Value = GetCurrentDate(_gtApp);
     //                   }
     //                   else if (strAttStatus == "ACTIVE")
     //                   {
     //                       comp.Recordset.Fields["W_ATTACHED_D"].Value = GetCurrentDate(_gtApp);
     //                   }
     //                   else if (strAttStatus == "ABANDONED")
     //                   {
     //                       comp.Recordset.Fields["W_ABANDON_D"].Value = GetCurrentDate(_gtApp);
     //                   }
					//	comp.Recordset.Update();
					//}
            }
			catch (Exception ex)
			{
                MessageBox.Show("Error occured in fiSetDateOnValueMatch " + ex.Message, "G/Technology");				
			}
		}

		public void Validate(out string[] ErrorPriorityArray, out string[] ErrorMessageArray)
		{
			ErrorMessageArray = null;
			ErrorPriorityArray = null;
		}

		/// <summary>
		/// get current date
		/// </summary>
		/// <param name="_gtApp"></param>
		/// <returns></returns>
		private string GetCurrentDate(IGTApplication _gtApp)
        {
            int recordsEffected=1;
            Recordset rsDate;
            string strCurrentDate=null;
            try
            {
                rsDate = _gtApp.DataContext.Execute("SELECT SYSDATE FROM DUAL", out recordsEffected, Convert.ToInt32(CommandTypeEnum.adCmdText));
                //SELECT TO_CHAR(SYSDATE, 'DD-MM-YY') FROM dual
                if (rsDate != null)
                {
                    rsDate.MoveFirst();
                    strCurrentDate = Convert.ToString(rsDate.Fields[0].Value);
                }
            }
			catch (Exception ex)
			{
				throw new Exception("Error getCurrentDate() " + ex.Message);
			}
			return strCurrentDate;
        }

        

        
    }
}

