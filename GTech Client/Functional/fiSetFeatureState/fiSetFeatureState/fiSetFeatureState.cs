//----------------------------------------------------------------------------+
//        Class: fiSetFeatureState
//  Description: This interface determines the appropriate feature state based on the active job type and status.
//----------------------------------------------------------------------------+
//     $Author:: hkonda                                   $
//       $Date:: 13/11/17                                 $
//   $Revision:: 1                                        $
//----------------------------------------------------------------------------+
//    $History:: fiSetFeatureState.cs                     $
// 
// *****************  Version 1  *****************
// User: hkonda     Date: 25/10/17   Time: 18:00  Desc : Created
// 
// Modified:
//  09-JAN-2019, Rich Adase - Changed feature state update processing to assume firing on SetValue for COMP_UNIT_N.ACTIVITY_C instead of on Update
//                          - Refactored SetFeatureStateBasedOnActivity as DetermineFeatureStateBasedOnActivity
//  27-JAN-2019, Hari       -  Added new else-if condition to fix ALM-1685
//  13-MAr-2019, Hari       - Code adjusted as per ALM-1657-JIRA-2460    
//  19-MAr-2019, Hari       - Code adjusted as per discussion with Rich for ALM-1657-JIRA-2460
//  22-Mar-2019, Hari       - ALM-2127-JIRA-2680 - Feature state to be set to INI when all the CU activities are blank - For Field Activity service installations
//                          - ALM-2129-JIRA-2679 - Improvement - Feature state is not getting changed from ‘Closed’ state to ‘In Service Install’ state.
//  01-Apr-2019,Hari        - ALM_2033-JIRA-2697 - Feature state is not updating correctly on relocate command
//----------------------------------------------------------------------------+
using ADODB;
using Intergraph.GTechnology.API;
using Intergraph.GTechnology.Interfaces;
using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Linq;

namespace GTechnology.Oncor.CustomAPI
{
	public class fiSetFeatureState : IGTFunctional
	{
		#region Private Variables
		private GTArguments m_arguments;
		private string m_componentName;
		private IGTComponents m_components;
		private IGTDataContext m_dataContext;
		private string m_fieldName;
		private IGTFieldValue m_fieldValueBeforeChange;
		private GTFunctionalTypeConstants m_type;
		private string m_oJobType = string.Empty;
		private short m_recordCount = 0;

		#endregion

		#region Properties
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

		#region IGTFunctional Methods
		public void Delete()
		{
		}

		public void Execute()
		{
			string jobStatus = string.Empty;
			bool isNonWRJob = CheckIfNonWrJob(out jobStatus);
			try
			{
				// For new feature placements, set initial Feature State based on job type and status
				//   Note: assumes this FI is configured to fire as AddNew only on Common Attributes component (CNO 1)
				if(m_type == GTFunctionalTypeConstants.gtftcAddNew)
				{
					try
					{
						Recordset commonComponentRs = Components[ComponentName].Recordset;
						commonComponentRs.MoveFirst();

						if((!string.IsNullOrEmpty(m_oJobType) && m_oJobType.ToUpper().Equals("WR-MAPCOR")))
						{
							commonComponentRs.Fields["FEATURE_STATE_C"].Value = "INI";
						}

						else if(!isNonWRJob) //Any value except NON-WR
						{
							if(!string.IsNullOrEmpty(jobStatus) && jobStatus.ToUpper() == "ASBUILT")
							{
								commonComponentRs.Fields["FEATURE_STATE_C"].Value = "ABI";
							}
							else if(!string.IsNullOrEmpty(jobStatus) && jobStatus.ToUpper() == "CONSTRUCTIONCOMPLETE")
							{
								commonComponentRs.Fields["FEATURE_STATE_C"].Value = "INI";
							}
							else
							{
								commonComponentRs.Fields["FEATURE_STATE_C"].Value = "PPI";
							}
						}
						else // GIS Maintenance job
						{
							commonComponentRs.Fields["FEATURE_STATE_C"].Value = "CLS";
						}
					}
					catch(Exception ex)
					{
						MessageBox.Show("Error during fiSetFeatureState execution." + ex.Message, "G/Technology");
					}
				}
				// For CU edits, update Feature State based on aggregated CU Activity
				//   Note: assumes this FI is configured to fire on change to COMP_UNIT_N.ACTIVITY_C
				else if(m_componentName == "COMP_UNIT_N" && m_fieldName == "ACTIVITY_C" && !string.Equals(GetJobType(), "ClosurePending")) //(m_type == GTFunctionalTypeConstants.gtftcUpdate)
				{
					IGTComponent oCUComponent = Components[ComponentName];
					List<String> CUActivity = new List<string>();
					string currentState = string.Empty;
					string newState = string.Empty;

					if(oCUComponent.Recordset != null)
					{
						if(oCUComponent.Recordset.RecordCount > 0)
						{
							// Build list of Activity codes from all instances
							oCUComponent.Recordset.MoveFirst();

							short fno = Convert.ToInt16(oCUComponent.Recordset.Fields["G3E_FNO"].Value);
							int fid = Convert.ToInt32(oCUComponent.Recordset.Fields["G3E_FID"].Value);

							if(fno == 54 && IsPlacementTypeAssociated()) // Check if Service Line has Placement Type equals to ASSOCIATED
							{
								return;
							}

							while(oCUComponent.Recordset.EOF == false)
							{
								CUActivity.Add(Convert.ToString(oCUComponent.Recordset.Fields["ACTIVITY_C"].Value));
								oCUComponent.Recordset.MoveNext();
							}
							m_recordCount = (short)oCUComponent.Recordset.RecordCount;
							// Update feature state if necessary
							Components.GetComponent(1).Recordset.MoveFirst();
							currentState = Components.GetComponent(1).Recordset.Fields["FEATURE_STATE_C"].Value.ToString();
							newState = DetermineFeatureStateBasedOnActivity(jobStatus, currentState, CUActivity);
							if(!string.IsNullOrEmpty(newState) && !newState.Equals(currentState))
							{
								Components.GetComponent(1).Recordset.Fields["FEATURE_STATE_C"].Value = newState;
							}
						}
					}
				}
			}
			catch
			{
				// avoid displaying message boxes in functional interfaces, to avoid impact on unattended sessions (e.g.- GIS Automator)
			}
		}

		private string DetermineFeatureStateBasedOnActivity(string p_jobStatus, string p_currentState, List<string> p_ActivityCode)
		{
			string sFeatureState = string.Empty;

			try
			{
				List<string> distinctSet = p_ActivityCode.Distinct().ToList<string>();

				// NOTE: This won't work because the problem is occurring after Completion processing has already set the feature state
				//   to INI, and so this method would not be short-circuited.  Might drop p_currentState as an argument.
				//
				//// Only change state for stable-state features; do not overwrite an existing proposed state
				////   Therefore return empty string if the feature is not starting in a stable state
				//if (!(p_currentState == "INI" || p_currentState == "CLS"))
				//{
				//    return sFeatureState;
				//}

				// If set has only blank activities
				if(distinctSet.Count == 1 && distinctSet.Contains(""))
				{
					sFeatureState = "INI";
				}


				// If the set includes combination of install , remove/salvage/abandon and blank activities, treat it as a changeout 

				//Combination of codes- 
				/*
				 * Install and Removal codes   OR
				 * Removal codes and Blanks OR
				*/

				else if(((distinctSet.Contains("I") || distinctSet.Contains("IC") || distinctSet.Contains("TI")) && (distinctSet.Contains("R") || distinctSet.Contains("RC") || distinctSet.Contains("S") || distinctSet.Contains("A") || distinctSet.Contains("T") || distinctSet.Contains("TC")))
						|| ((distinctSet.Contains("R") || distinctSet.Contains("RC") || distinctSet.Contains("S") || distinctSet.Contains("A") || distinctSet.Contains("T") || distinctSet.Contains("TC")) && distinctSet.Contains("")))
				// || ((distinctSet.Contains("I") || distinctSet.Contains("IC") || distinctSet.Contains("TI")) && distinctSet.Contains("")))

				{
					if(!string.IsNullOrEmpty(m_oJobType) && m_oJobType.ToUpper().Equals("WR-MAPCOR"))
					{
						sFeatureState = "INI";
						return sFeatureState;
					}
					if(p_jobStatus.Equals("Design"))
					{
						sFeatureState = "PPX";
					}
					else if(p_jobStatus.Equals("AsBuilt"))
					{
						sFeatureState = "ABX";
					}
					else if(p_jobStatus.Equals("ConstructionComplete"))
					{
						sFeatureState = "INI";
					}
				}
				// Else, if the set contains only install activities, treat it as an install
				else if((distinctSet.Count == 1 && (distinctSet.Contains("I") || distinctSet.Contains("IC") || distinctSet.Contains("TI")))
						|| (distinctSet.Count == 2 && ((!distinctSet.Contains("R") && !distinctSet.Contains("RC") && !distinctSet.Contains("S") && !distinctSet.Contains("A") && !distinctSet.Contains("T") && !distinctSet.Contains("TC"))) && ((distinctSet.Contains("I") || distinctSet.Contains("IC"))))
						|| (distinctSet.Count == 3 && ((!distinctSet.Contains("R") && !distinctSet.Contains("RC") && !distinctSet.Contains("S") && !distinctSet.Contains("A") && !distinctSet.Contains("T") && !distinctSet.Contains("TC"))) && ((distinctSet.Contains("I") || distinctSet.Contains("IC") || distinctSet.Contains("TI"))))
						|| ((distinctSet.Contains("I") || distinctSet.Contains("IC") || distinctSet.Contains("TI")) && distinctSet.Contains("")))
				{
					if(!string.IsNullOrEmpty(m_oJobType) && m_oJobType.ToUpper().Equals("WR-MAPCOR"))
					{
						sFeatureState = "INI";
					}
					else if(p_jobStatus.Equals("Design"))
					{
						sFeatureState = "PPI";
					}
					else if(p_jobStatus.Equals("AsBuilt"))
					{
						sFeatureState = "ABI";
					}
					else if(p_jobStatus.Equals("ConstructionComplete"))
					{
						sFeatureState = "INI";
					}
				}

				// For ALM 1685-ONCORDEV-2433 - 
				// For Abandon feature command all the Activities would be 'A' and would not have any blanks. So the below condition makes sure that this FI got fired as a result of Abandon feature
				// and sets back to the respective feature states
				// had to provide this fix because this FI is configured with type SetValue and this FI is being fired for every instance of CU when Abandon feature sets each Activity.
				else if(distinctSet.Contains("A") && (!distinctSet.Contains("R") && !distinctSet.Contains("RC") && !distinctSet.Contains("S") && !distinctSet.Contains("")))
				{
					if(!string.IsNullOrEmpty(m_oJobType) && m_oJobType.ToUpper().Equals("WR-MAPCOR"))
					{
						sFeatureState = "OSA";
						return sFeatureState;
					}
					if(p_jobStatus.Equals("Design"))
					{
						sFeatureState = "PPA";
					}
					else if(p_jobStatus.Equals("AsBuilt"))
					{
						sFeatureState = "ABA";
					}
					else if(p_jobStatus.Equals("ConstructionComplete"))
					{
						sFeatureState = "OSA";
					}
				}

				// Else, if the set contains only remove/salvage/abandon and blank activities, treat it as a removal 

				//else if ((distinctSet.Contains("R") || distinctSet.Contains("S") || distinctSet.Contains("A")) && distinctSet.Contains(""))
				//else if (!(!(distinctSet.Contains("R")) && !distinctSet.Contains("S") && !distinctSet.Contains("A") && !distinctSet.Contains("")))

				else if((distinctSet.Contains("R") || distinctSet.Contains("RC") || distinctSet.Contains("S") || distinctSet.Contains("A") || distinctSet.Contains("T") || distinctSet.Contains("TC")))
				{
					if(!string.IsNullOrEmpty(m_oJobType) && m_oJobType.ToUpper().Equals("WR-MAPCOR"))
					{
						sFeatureState = "OSR";
					}
					else if(p_jobStatus.Equals("Design"))
					{
						sFeatureState = "PPR";
					}
					else if(p_jobStatus.Equals("AsBuilt"))
					{
						sFeatureState = "ABR";
					}
					else if(p_jobStatus.Equals("ConstructionComplete"))
					{
						sFeatureState = "OSR";
					}
				}

			}
			catch(Exception ex)
			{
				throw new Exception(ex.Message);
			}

			return sFeatureState;
		}


		private bool IsPlacementTypeAssociated()
		{
			try
			{
				IGTComponent slAttributes = Components.GetComponent(5401);

				if(slAttributes != null && slAttributes.Recordset != null && slAttributes.Recordset.RecordCount > 0)
				{
					slAttributes.Recordset.MoveFirst();
					return Convert.ToString(slAttributes.Recordset.Fields["PLACEMENT_TYPE_C"].Value).Equals("ASSOCIATED");
				}

				return false;
			}
			catch(Exception)
			{

				throw;
			}
		}

		private string GetJobType()
		{
			string sReturn = string.Empty;

			ADODB.Recordset rs = m_dataContext.OpenRecordset("select G3E_JOBSTATUS from g3e_job where G3E_IDENTIFIER = ?", CursorTypeEnum.adOpenDynamic, LockTypeEnum.adLockOptimistic, (int)(ADODB.CommandTypeEnum.adCmdText), m_dataContext.ActiveJob);

			if(rs != null)
			{
				if(rs.RecordCount > 0)
				{
					rs.MoveFirst();
					sReturn = Convert.ToString(rs.Fields["G3E_JOBSTATUS"].Value);
				}
			}
			return sReturn;
		}

		public void Validate(out string[] ErrorPriorityArray, out string[] ErrorMessageArray)
		{
			ErrorMessageArray = null;
			ErrorPriorityArray = null;
		}
		#endregion

		#region Private methods

		/// <summary>
		/// Method to check whether the Job is of type NON-WR.
		/// </summary>
		/// <returns>true, if job is of type NON-WR</returns>
		private bool CheckIfNonWrJob(out string jobStatus)
		{
			jobStatus = string.Empty;
			try
			{
				Recordset jobInfoRs = GetRecordSet(string.Format("select G3E_JOBTYPE, G3E_JOBSTATUS from g3e_job where g3e_identifier  = '{0}'", m_dataContext.ActiveJob));
				if(jobInfoRs != null && jobInfoRs.RecordCount > 0)
				{
					jobInfoRs.MoveFirst();
					m_oJobType = Convert.ToString(jobInfoRs.Fields["G3E_JOBTYPE"].Value);
					jobStatus = Convert.ToString(jobInfoRs.Fields["G3E_JOBSTATUS"].Value);
				}
				return !string.IsNullOrEmpty(m_oJobType) && m_oJobType.ToUpper() == "NON-WR" ? true : false;
			}
			catch(Exception)
			{
				throw;
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
				Command command = new ADODB.Command();
				command.CommandText = sqlString;
				Recordset results = m_dataContext.ExecuteCommand(command, out outRecords);
				return results;
			}
			catch(Exception)
			{
				throw;
			}
		}
		#endregion
	}

}


