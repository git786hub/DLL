// ===================================================
//  Copyright 2017 Intergraph Corp.
//  File Name: DataAccess.cs
//
//  Description:    Class to build the recordset.
//  Remarks: 
//  Modification History
//  Date                Author                      Comments
//  --------            --------------              --------
//  03/04/2018          Prathyusha                  Created 
//  12/04/2018          Sitara                      Modified
// ======================================================
using System;
using ADODB;
using Intergraph.GTechnology.API;

namespace GTechnology.Oncor.CustomAPI
{
	class DataAccess
	{
		#region Variables

		private IGTDataContext m_oGTDataContext;
		public string m_strStatus = string.Empty;
		public string m_strComment = string.Empty;
		public short m_boundaryFNO = 0;
		public int m_boundaryFID = 0;
		private IGTApplication m_gTApplication = null;
		#endregion

		#region Constructor
		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="p_dataContext">The current G/Technology application object.</param>
		public DataAccess(IGTDataContext p_dataContext)
		{
			this.m_oGTDataContext = p_dataContext;
		}

		public DataAccess(IGTDataContext p_dataContext, IGTApplication p_gTApplication)
		{
			this.m_oGTDataContext = p_dataContext;
			this.m_gTApplication = p_gTApplication;
		}
		#endregion

		#region Methods
		/// <summary>
		/// Method to build the recordset.
		/// </summary>
		/// <param name="p_strSql">SQL Query</param>
		/// <returns></returns>
		public Recordset GetRecordset(string p_strSql)
		{
			Recordset recordset = null;
			try
			{
				recordset = m_oGTDataContext.OpenRecordset(p_strSql, CursorTypeEnum.adOpenStatic,
											 LockTypeEnum.adLockReadOnly, (int)CommandTypeEnum.adCmdText);
			}
			catch
			{
				throw;
			}
			return recordset;
		}

		public Recordset GetRecordset(string p_strSql, object p_params)
		{
			Recordset recordset = null;
			try
			{
				recordset = m_oGTDataContext.OpenRecordset(p_strSql, CursorTypeEnum.adOpenStatic,
											 LockTypeEnum.adLockReadOnly, (int)CommandTypeEnum.adCmdText, p_params);
			}
			catch
			{
				throw;
			}
			return recordset;
		}

		/// <summary>
		/// Method to build the MetadataRecordset.
		/// </summary>
		/// <param name="p_tableName">SQL Query</param>
		/// <returns></returns>
		public Recordset GetMetadataRecordset(string p_tableName)
		{
			Recordset recordset = null;
			try
			{
				recordset = m_oGTDataContext.MetadataRecordset(p_tableName);
			}
			catch
			{
				throw;
			}
			return recordset;
		}

		public IGTKeyObject GetOwner(string p_structureId)
		{
			Recordset rsOwner = null;
			try
			{
				rsOwner = GetOwnerRecordSet(p_structureId);
				if(rsOwner != null)
				{
					rsOwner.MoveFirst();
					if(rsOwner.RecordCount > 1)
					{
						while(!rsOwner.EOF)
						{
							if(rsOwner.Fields[0] != null && !string.IsNullOrEmpty(Convert.ToString(rsOwner.Fields[0].Value)))
							{
								if(Convert.ToString(rsOwner.Fields[0].Value) == "PPI" ||
										Convert.ToString(rsOwner.Fields[0].Value) == "ABI" ||
										Convert.ToString(rsOwner.Fields[0].Value) == "INI")
								{
									return m_oGTDataContext.OpenFeature(Convert.ToInt16(rsOwner.Fields[1].Value), Convert.ToInt32(rsOwner.Fields[2].Value));
								}
							}
							rsOwner.MoveNext();
						}
					}
					else if(rsOwner.RecordCount == 1)
					{
						return m_oGTDataContext.OpenFeature(Convert.ToInt16(rsOwner.Fields[1].Value), Convert.ToInt32(rsOwner.Fields[2].Value));
					}
				}
				else
				{
					m_strStatus = "ERROR";
					m_strComment = "Invalid Structure located.  Valid features are Miscellaneous Structure, Pole, and Street Light Standard.";
				}
			}
			catch
			{
				throw;
			}
			finally
			{
				if(rsOwner != null)
				{
					rsOwner.Close();
					rsOwner = null;
				}
			}

			return null;
		}
		private Recordset GetOwnerRecordSet(string p_structureId)
		{
			try
			{
				string sql = string.Format("select FEATURE_STATE_C,G3E_FNO,G3E_FID from COMMON_N where STRUCTURE_ID = '{0}' and " +
						"G3E_FNO IN (107,110,114)", p_structureId);
				return GetRecordset(sql);
			}
			catch
			{
				throw;
			}

		}
		public int GetBoundaryFID(string p_strESILocation)
		{
			Recordset rs = null;
			string sql = string.Empty;
			string strBoundaryClass = string.Empty;
			int boundaryFID = 0;
			string strBoundaryField = string.Empty;
			string strBoundaryANOValue = string.Empty;
			string strBoundaryTableName = string.Empty;
			try
			{
				rs = GetRecordset("select a.boundary_id,bnd.bnd_fno,ai.g3e_field,ai.g3e_cno,ai.g3e_componenttable from stlt_account a join " +
				"stlt_boundary bnd on a.boundary_class = bnd.bnd_class join g3e_attributeinfo_optable ai on " +
				"bnd.bnd_id_ano = ai.g3e_ano where a.esi_location = :1",
						p_strESILocation);

				if(rs != null && rs.RecordCount > 0)
				{
					rs.MoveFirst();
					strBoundaryField = Convert.ToString(rs.Fields["g3e_field"].Value);
					strBoundaryANOValue = Convert.ToString(rs.Fields["boundary_id"].Value);
					m_boundaryFNO = Convert.ToInt16(rs.Fields["bnd_fno"].Value);
					strBoundaryTableName = Convert.ToString(rs.Fields["g3e_componenttable"].Value);

					sql = "select g3e_fid from " + strBoundaryTableName + " WHERE " +
									"" + strBoundaryField + " = :1";

					Recordset rsFid = GetRecordset(sql, strBoundaryANOValue);
					if(rsFid != null && rsFid.RecordCount > 0)
					{
						rsFid.MoveFirst();
						boundaryFID = Convert.ToInt32(rsFid.Fields[0].Value);
					}
					if(rsFid != null)
					{
						rsFid.Close();
						rsFid = null;
					}
				}

			}
			catch
			{
				throw;
			}
			finally
			{
				if(rs != null)
				{
					rs.Close();
					rs = null;
				}
			}
			m_boundaryFID = boundaryFID;
			return boundaryFID;
		}
		public int GetMiscellaneousStructure(string p_ESILocation)
		{
			Recordset accountRS = null;
			int fid = 0;
			try
			{
				accountRS = GetRecordset(String.Format("select MISC_STRUCT_FID from STLT_ACCOUNT WHERE " +
					 "ESI_LOCATION='{0}'  AND MISC_STRUCT_FID IS NOT NULL", p_ESILocation));

				if(accountRS != null && accountRS.RecordCount > 0)
				{
					accountRS.MoveFirst();
					if(accountRS.Fields[0].Value != null)
					{
						fid = Convert.ToInt32(accountRS.Fields[0].Value);
					}
				}
			}
			catch
			{
				throw;
			}
			finally
			{
				if(accountRS != null)
				{
					accountRS.Close();
					accountRS = null;
				}
			}
			return fid;
		}
		public string GetCustomerOwnedCUCode(string p_LampType, string p_LuminaireStyle, string p_Wattage)
		{
			Recordset cuRecordset = null;

			try
			{
				string sql = "select u.cu_id from culib_unit u";
				sql += " join culib_unitattribute ua1 on u.cu_id=ua1.cu_id and ua1.category_c='STREETLIGHT' and ua1.attribute_id='LAMP_TYPE_C' and ua1.attr_value=?";
				sql += " join culib_unitattribute ua2 on u.cu_id=ua2.cu_id and ua2.category_c='STREETLIGHT' and ua2.attribute_id='LAMP_WATT_Q' and ua2.attr_value=?";
				sql += " join culib_unitattribute ua3 on u.cu_id=ua3.cu_id and ua3.category_c='STREETLIGHT' and ua3.attribute_id='LUMIN_STYL_C' and ua3.attr_value=?";
				sql += " join culib_unitattribute ua4 on u.cu_id=ua4.cu_id and ua4.category_c='STREETLIGHT' and ua4.attribute_id='OWNER_TYPE_C' and ua4.attr_value='Customer'";

				string[] args = new string[3] { p_LampType, p_Wattage, p_LuminaireStyle };
				cuRecordset = GetRecordset(sql, args);

				if(cuRecordset != null)
				{
					if(1 != cuRecordset.RecordCount)
					{
						m_strStatus = "ERROR";
						m_strComment = "Distinct customer-owned CU cannot be found for the given Lamp Type, Wattage, and Luminaire Style.";
					}
					else
					{
						cuRecordset.MoveFirst();
						return Convert.ToString(cuRecordset.Fields[0].Value);
					}
				}
				else
				{
					m_strStatus = "ERROR";
					m_strComment = "Customer-owned CU cannot be found for the given Lamp Type, Wattage, and Luminaire Style.";
				}
			}
			catch
			{
				throw;
			}
			finally
			{
				if(cuRecordset != null)
				{
					cuRecordset.Close();
					cuRecordset = null;
				}
			}

			return null;
		}

		#endregion
	}
}
