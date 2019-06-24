//----------------------------------------------------------------------------+
//        Class: fiPhasePositionValidate
//  Description: 
//          Validates a feature to check Duplicate Phase Position for all wires with in the same conductor. it found throws validation error message
//----------------------------------------------------------------------------+
//     $Author:: Pramod                                                      $
//       $Date:: 05/05/2017                                                     $
//   $Revision:: 1                                                            $
//----------------------------------------------------------------------------+
//    $History:: fiPhasePositionValidate.cs                                           $
// 
// *****************  Version 1  *****************
// User: Pramod     Date: 05/05/2017     Time: 18:00	Desc: Implemented  Business Rule as per JIRA 476 
// *****************  Version 1.0.0.1  *****************
// User: kappana     Date: 07/21/2017    Time: 18:00	Desc: Implemented  Business Rule as per JIRA 686
//----------------------------------------------------------------------------+

using ADODB;
using Intergraph.GTechnology.API;
using Intergraph.GTechnology.Interfaces;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace GTechnology.Oncor.CustomAPI
{
    public class fiPhasePositionValidate : IGTFunctional
    {
		public const string Caption = "G/Technology";

		private GTArguments _arguments = null;
        private IGTDataContext _dataContext = null;
        private IGTComponents _components;
        private string _componentName;
        private string _fieldName;

        public GTArguments Arguments
        {
            get { return _arguments; }
            set { _arguments = value; }
        }

        public string ComponentName
        {
            get { return _componentName; }
            set { _componentName = value; }
        }

        public IGTComponents Components
        {
            get { return _components; }
            set { _components = value; }
        }

        public IGTDataContext DataContext
        {
            get { return _dataContext; }
            set { _dataContext = value; }
        }


        public void Delete()
        {
        }

        public void Execute()
        {
			//JIRA-686 FI to blank out the Phase Postion when Phase is set to N
			if (_fieldName== "PHASE_C")
			{
				IGTComponent comp = Components[ComponentName];
				Recordset compRecords = comp.Recordset;
				string strPhase;

				try
				{
					if (compRecords !=null)
					{
						compRecords.MoveFirst();
						while (!(compRecords.EOF || compRecords.BOF))
						{
							strPhase = Convert.ToString(compRecords.Fields[_fieldName].Value);
							if (strPhase != null && strPhase == "N")
							{
								compRecords.Fields["PHASE_POS_C"].Value = null;
							}
							compRecords.MoveNext();
						}
					}	
				}
				catch (Exception ex)
				{
					MessageBox.Show(ex.Message, Caption);
				}
				finally
				{
					comp = null;
				}
			}
        }

        public string FieldName
        {
            get { return _fieldName; }
            set { _fieldName = value; }
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
			if (_fieldName == "PHASE_POS_C")
			{
				ADODB.Recordset rs = null;
				ADODB.Recordset rsTemp = null;
				int record = 0;
				//string phasePosition = "";
				ErrorPriorityArray = null;
				ErrorMessageArray = null;
				string sqlStmt = "Select {0},count({0}) from {1} where g3e_fid={2} and g3e_fno={3} GROUP BY {0} having count({0})>1";
				//string sqlQuery = "Select listagg(a.{0},',') within group (order by a.PHASE_POS_C) as PhasePosition from {1} a where g3e_fid={2} and not exists (Select  vl_key from VL_PHASE_POSITION p where p.vl_key=a.{0})";
				List<string> errorMsg = new List<string>();
				List<string> errorPriority = new List<string>();
				try
				{
					IGTComponent gtComponent = _components[_componentName];
					if (gtComponent != null)
					{
						rs = gtComponent.Recordset;
						if (rs != null && rs.RecordCount > 0)
						{
							if (Convert.ToInt32(rs.Fields["G3e_CID"].Value) == 1)
							{
								//JIRA 195- check Duplicate Phase Position 
								rsTemp = _dataContext.Execute(string.Format(sqlStmt, _fieldName, _componentName, rs.Fields["G3e_FID"].Value, rs.Fields["G3e_FNO"].Value), out record, (int)ADODB.CommandTypeEnum.adCmdText, null);
								if (rsTemp != null && rsTemp.RecordCount > 0)
								{
									errorMsg.Add("Feature having duplicate Phase Position. Phase Position should be unique across all the wires within same conductor.");
									errorPriority.Add("P1");
								}
                                /*
                                 * Commneted this code as per Shubham suggestion
                                 
								// As per the JIRA 476 - Business Rule Validate Unknown Values in Phase Position 
								rsTemp = _dataContext.Execute(string.Format(sqlQuery, _fieldName, _componentName, rs.Fields["G3e_FID"].Value), out record, (int)ADODB.CommandTypeEnum.adCmdText, null);
								if (rsTemp != null && rsTemp.RecordCount > 0)
								{
									rsTemp.MoveFirst();
									phasePosition = Convert.ToString(rsTemp.Fields[0].Value);
									if (!string.IsNullOrEmpty(phasePosition))
									{
										errorMsg.Add("Phase Position " + rsTemp.Fields[0].Value + " are unknown Values");
										errorPriority.Add("P2");
									}
								}
                                */
							}
						}
					}
					if (errorMsg.Count > 0)
					{
						ErrorPriorityArray = errorPriority.ToArray();
						ErrorMessageArray = errorMsg.ToArray();
					}
				}
				catch(Exception ex)
				{
					MessageBox.Show(ex.Message, Caption);
				}
				finally
				{
					rsTemp = null;
				}
			}
			else
			{
				ErrorPriorityArray = null;
				ErrorMessageArray = null;
			}
        }
    }
}
