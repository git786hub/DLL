//----------------------------------------------------------------------------+
//        Class: fiPhasePosition
//  Description: This interface sets the Configured Phase attribute to the set of all present phases sorted in the order defined by their positions
//----------------------------------------------------------------------------+
//     $Author:: Pramod                                                      $
//       $Date:: 05/05/2017                                                     $
//   $Revision:: 1                                                            $
//----------------------------------------------------------------------------+
//    $History:: fiPhasePosition.cs                                           $
// 
// *****************  Version 1  *****************
// User: Pramod     Date: 05/05/2017     Time: 18:00	Desc: Implemented  Business Rule as per JIRA 476 
// *****************  Version 1.0.0.1  *****************
// User: kappana     Date: 07/21/2017    Time: 18:00	Desc: Implemented  Business Rule as per JIRA 686
// *****************  Version 1.0.0.2  *****************
// User: hkonda     Date: 08/29/2017    Time: 18:00	    Desc: Added new validations and execute method logic as per latest DDD
//----------------------------------------------------------------------------+

using ADODB;
using Intergraph.GTechnology.API;
using Intergraph.GTechnology.Interfaces;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Collections;

namespace GTechnology.Oncor.CustomAPI
{
	public class fiPhasePosition : IGTFunctional
	{
		public const string Caption = "G/Technology";
		public const string set1 = "Left,Center,Right";
		public const string set2 = "T,M,B";
		public const string set3 = "N,E,S,W";

		private GTArguments _arguments = null;
		private IGTDataContext _dataContext = null;
		private IGTComponents _components;
		private string _componentName;
		private string _fieldName;
		private List<string> m_PhaseList = null;

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


			try
			{
				if (_fieldName == "PHASE_C")
				{
					IGTComponent comp = Components[ComponentName];
					Recordset compRecords = comp.Recordset;
					string strPhase;
					if (compRecords != null)
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
				// Set Configured Phase

				Recordset primarConductorOhRs = Components.GetComponent(801).Recordset;
				if (primarConductorOhRs != null)
				{
					primarConductorOhRs.MoveFirst();
					if (!ValidatePhasePositionWithSets())
					{
						//primarConductorOhRs.Fields["PHASE_CONFIG"].Value = 
						PrepareConfiguredPhaseAttribute();
					}
				}
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message, Caption);
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
			ADODB.Recordset rs = null;
			ADODB.Recordset rsTemp = null;
			int record = 0;
			bool isPhaseExists = false;
			ErrorPriorityArray = null;
			ErrorMessageArray = null;

			try
			{
				List<string> errorMsg = new List<string>();
				List<string> errorPriority = new List<string>();
				string priority = Convert.ToString(_arguments.GetArgument(0));

				Recordset commonComponentRs = Components.GetComponent(1).Recordset;
				commonComponentRs.MoveFirst();
				string orientation = Convert.ToString(commonComponentRs.Fields["ORIENTATION_C"].Value);
				int fNo = Convert.ToInt32(commonComponentRs.Fields["G3E_FNO"].Value);
				string featureName = Convert.ToString(GetRecordSet(string.Format("select g3e_username NAME from g3e_feature where g3e_fno = {0}", fNo)).Fields["NAME"].Value);
				int fId = Convert.ToInt32(commonComponentRs.Fields["G3E_FID"].Value);
				bool isFeatureConnected = CheckFeatureConnectivity(fId);

				if (orientation == "OH" && featureName.ToUpper().Contains("PRIMARY") && isFeatureConnected && !CheckPhaseAndPhasePosition(out isPhaseExists))
				{
					errorPriority.Add(priority);
					errorMsg.Add("Overhead feature is missing phase position values.");
				}

				if (CheckPhaseAndPhasePosition(out isPhaseExists))
				{
					if (!isPhaseExists)
					{
						errorPriority.Add(priority);
						errorMsg.Add("Phase Position attributes do not match specified phase.");
					}
				}
				if (ValidatePhasePositionWithSets())
				{
					errorPriority.Add(priority);
					errorMsg.Add("Phase Position attributes are not consistent with respect to one another.");
				}

				if (_fieldName == "PHASE_POS_C")
				{


					string sqlStmt = "Select {0},count({0}) from {1} where g3e_fid={2} and g3e_fno={3} GROUP BY {0} having count({0})>1";
					//string sqlQuery = "Select listagg(a.{0},',') within group (order by a.PHASE_POS_C) as PhasePosition from {1} a where g3e_fid={2} and not exists (Select  vl_key from VL_PHASE_POSITION p where p.vl_key=a.{0})";


					IGTComponent gtComponent = _components[_componentName];
					if (gtComponent != null)
					{
						rs = gtComponent.Recordset;
						if (rs != null && rs.RecordCount > 0)
						{
							rs.MoveFirst();
							if (Convert.ToInt32(rs.Fields["G3e_CID"].Value) == 1)
							{
								//JIRA 195- check Duplicate Phase Position 
								rsTemp = _dataContext.Execute(string.Format(sqlStmt, _fieldName, _componentName, rs.Fields["G3e_FID"].Value, rs.Fields["G3e_FNO"].Value), out record, (int)ADODB.CommandTypeEnum.adCmdText, null);
								if (rsTemp != null && rsTemp.RecordCount > 0)
								{
									errorMsg.Add("Feature having duplicate Phase Position. Phase Position should be unique across all the wires within same conductor.");
									errorPriority.Add(priority);
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

			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message, Caption);
			}
			finally
			{
				rsTemp = null;
			}

		}

		private bool ValidatePhasePositionWithSets()
		{

			string phasePosition = string.Empty;
			int count = 0;
			bool set1Complete = false;
			bool set2Complete = false;
			bool set3Complete = false;
			List<string> setsList = new List<string> { set1, set2, set3 };

			Recordset phasePositionRs = Components[ComponentName].Recordset;
			if (phasePositionRs != null && phasePositionRs.RecordCount > 0)
			{
				phasePositionRs.MoveFirst();
				while (!phasePositionRs.EOF)
				{
					phasePosition = Convert.ToString(phasePositionRs.Fields["PHASE_POS_C"].Value);
					if (set1.Contains(phasePosition))
					{
						set1Complete = true;
					}
					if (set2.Contains(phasePosition))
					{
						set2Complete = true;
					}
					if (set3.Contains(phasePosition))
					{
						set3Complete = true;
					}

					phasePositionRs.MoveNext();
				}
				//phasePosition = phasePosition.TrimStart(',').TrimEnd(',');
				if ((set1Complete && set2Complete) || (set2Complete && set3Complete) || (set3Complete && set1Complete))
				{
					return true;
				}
			}
			return false;
		}

		/// <summary>
		/// Method to set the 'Configured Phase' attribute of Primary Conductor - OH (Network) Attributes
		/// </summary>
		/// <returns></returns>
		private string PrepareConfiguredPhaseAttribute()
		{
			List<KeyValuePair<string, string>> PositionsAndPhasePair = null;
			string configuredPhase = string.Empty;
			bool isSet1 = false;
			bool isSet2 = false;
			bool isSet3 = false;
			bool set1Complete = false;
			bool set2Complete = false;
			bool set3Complete = false;

			try
			{
				Recordset ohAttributesRs = Components[ComponentName].Recordset;

				if (ohAttributesRs != null)
				{
					PositionsAndPhasePair = new List<KeyValuePair<string, string>>();
					string phasePosition = string.Empty;
					string phase = string.Empty;
					ohAttributesRs.MoveFirst();
					while (!ohAttributesRs.EOF)
					{
						phase = Convert.ToString(ohAttributesRs.Fields["PHASE_C"].Value);
						phasePosition = Convert.ToString(ohAttributesRs.Fields["PHASE_POS_C"].Value);
						if (string.IsNullOrEmpty(phase) || string.IsNullOrEmpty(phasePosition))
						{
							return string.Empty;
						}
						PositionsAndPhasePair.Add(new KeyValuePair<string, string>(phasePosition, phase));  // Key - PhasePosition , Value - Phase
						ohAttributesRs.MoveNext();
					}

					foreach (KeyValuePair<string, string> KeyValue in PositionsAndPhasePair)
					{
						if (set1.Contains(KeyValue.Key))
						{
							if (isSet2 || isSet3)
							{
								configuredPhase = string.Empty;
								break;
							}
							if (set1Complete)
							{
								continue;
							}
							isSet1 = true;
							string[] set1Split = set1.Split(',');
							string tempString = string.Empty;
							string tempKey = string.Empty;
							foreach (string position in set1Split)
							{
								var keyValues = (PositionsAndPhasePair.Where(x => x.Key == position));
								foreach (KeyValuePair<string, string> keyVal in keyValues)
								{
									if (tempKey == keyVal.Key)
									{
										return string.Empty;
									}
									configuredPhase = configuredPhase + keyVal.Value;
									tempKey = keyVal.Key;
								}
							}
							set1Complete = true;
						}
						else if (set2.Contains(KeyValue.Key))
						{
							if (isSet1 || isSet3)
							{
								configuredPhase = string.Empty;
								break;
							}
							if (set2Complete)
							{
								continue;
							}
							isSet2 = true;
							string[] set2Split = set2.Split(',');
							string tempString = string.Empty;
							string tempKey = string.Empty;
							foreach (string position in set2Split)
							{
								var keyValues = (PositionsAndPhasePair.Where(x => x.Key == position));
								foreach (KeyValuePair<string, string> keyVal in keyValues)
								{
									if (tempKey == keyVal.Key)
									{
										return string.Empty;
									}
									configuredPhase = configuredPhase + keyVal.Value;
									tempKey = keyVal.Key;
								}
							}
							set2Complete = true;
						}

						else if (set3.Contains(KeyValue.Key))
						{
							if (isSet2 || isSet1)
							{
								configuredPhase = string.Empty;
								break;
							}
							if (set3Complete)
							{
								continue;
							}
							isSet3 = true;
							string[] set3Split = set3.Split(',');
							string tempString = string.Empty;
							string tempKey = string.Empty;
							foreach (string position in set3Split)
							{
								var keyValues = (PositionsAndPhasePair.Where(x => x.Key == position));
								foreach (KeyValuePair<string, string> keyVal in keyValues)
								{
									if (tempKey == keyVal.Key)
									{
										return string.Empty;
									}
									configuredPhase = configuredPhase + keyVal.Value;
									tempKey = keyVal.Key;
								}
							}
							set3Complete = true;
						}
					}
				}
				return configuredPhase;
			}
			catch (Exception)
			{
				throw;
			}
		}

		private bool CheckFeatureConnectivity(int fid)
		{
			int node1 = 0;
			int node2 = 0;
			Recordset tempRs = GetRecordSet(string.Format("select NODE_1_ID NODE1 , NODE_2_ID NODE2 from connectivity_n where g3e_fid = {0}", fid));
			if (tempRs != null && tempRs.RecordCount > 0)
			{
				tempRs.MoveFirst();
				while (!tempRs.EOF)
				{
					node1 = Convert.ToInt32(tempRs.Fields["NODE1"].Value);
					node2 = Convert.ToInt32(tempRs.Fields["NODE2"].Value);
					tempRs.MoveNext();
				}
			}
			if (node1 > 0 && node2 > 0)
			{
				return true;
			}
			return false;
		}


		private bool CheckPhaseAndPhasePosition(out bool isPhaseExists)
		{
			isPhaseExists = false;
			bool isPhaseValid = false;
			bool isPhasePositionvalid = false;


			if (m_PhaseList == null)
			{
				GetPickListPhase();
			}

			List<string> phaseList = new List<string>();
			List<string> phasePositionList = new List<string>();
			Recordset PhaseAndPositionRs = Components[ComponentName].Recordset;
			if (PhaseAndPositionRs != null && PhaseAndPositionRs.RecordCount > 0)
			{
				PhaseAndPositionRs.MoveFirst();
				while (!PhaseAndPositionRs.EOF)
				{
					phaseList.Add(Convert.ToString(PhaseAndPositionRs.Fields["PHASE_C"].Value));
					phasePositionList.Add(Convert.ToString(PhaseAndPositionRs.Fields["PHASE_POS_C"].Value));
					PhaseAndPositionRs.MoveNext();
				}
			}

			bool isPhasePositionExists = phasePositionList.Contains("unknown");   //m_PhasePositionList.All(s => phasePositionList.Contains(s));
			isPhaseExists = m_PhaseList.All(s => phaseList.Contains(s));
			return isPhasePositionExists;
		}


		private void GetPickListPhase()
		{
			Recordset phaseRs = GetRecordSet("select vl_value from VL_WIRE_PHASE");
			if (phaseRs != null && phaseRs.RecordCount > 0)
			{
				m_PhaseList = new List<string>();
				phaseRs.MoveFirst();
				while (!phaseRs.EOF)
				{
					m_PhaseList.Add(Convert.ToString(phaseRs.Fields["vl_value"].Value));
					phaseRs.MoveNext();
				}
			}
		}

		/// <summary>
		/// Method to execute sql query and return the result record set
		/// </summary>
		/// <param name="sqlString"></param>
		/// <returns></returns>
		private Recordset GetRecordSet(string sqlString)
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
