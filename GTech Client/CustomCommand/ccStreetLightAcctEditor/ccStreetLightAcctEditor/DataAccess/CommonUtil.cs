using ADODB;
using Intergraph.GTechnology.API;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;

namespace GTechnology.Oncor.CustomAPI.DataAccess
{
	/// <summary>
	/// enum value is used for to indentify mode Entity
	/// </summary>
	public enum EntityMode
	{
		Add = 1,
		Update = 2,
		Delete = 3,
		Review = 4
	}
	public static class CommonUtil
	{

		static IGTApplication _gtApp = GTClassFactory.Create<IGTApplication>();
		static IGTDataContext _gtDataContext = _gtApp.DataContext;
		public static short MiscStructG3eFno = 107;
		public static short MiscStructGeoCno = 10702;
		public static short MiscStructAttributeCno = 10701;

		public static short StreetLightG3eFno = 56;
		public static short StreetLightGeoCno = 5602;
		public static short StreetLightAttributeCno = 5601;

		public static short CommonAttributeCno = 1;
		public static short ConnectivityAttributeCno = 11;
		public static short CUAttributeCno = 21;

		//   public static IGTCustomCommandHelper CustomCommandHelper;
		//    public static IGTTransactionManager TransactionManager;


		/// <summary>
		/// Execute SQL statement
		/// </summary>
		/// <param name="sqlStmt"></param>
		/// <returns></returns>
		public static Recordset Execute(string sqlStmt)
		{
			int recordsAffected;
			ADODB.Recordset rs = null;
			rs = _gtDataContext.Execute(sqlStmt, out recordsAffected, (int)ADODB.CommandTypeEnum.adCmdText);
			return rs;
		}

		/// <summary>
		/// Convert Recordset to Entity
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="rs"></param>
		/// <returns></returns>
		public static BindingList<T> ConvertRSToEntity<T>(Recordset rs)
		{

			BindingList<T> lst = new BindingList<T>();
			while(!rs.EOF)
			{
				T entity = Activator.CreateInstance<T>();
				foreach(var entityColumn in entity.GetType().GetProperties())
				{
					if(entityColumn != null)
					{
						if(entityColumn.Name != "EntityState")
						{
							try
							{
								Type t = Nullable.GetUnderlyingType(entityColumn.PropertyType) ?? entityColumn.PropertyType;
								object objValue = (rs.Fields[entityColumn.Name].Value == null) ? null : Convert.ChangeType(rs.Fields[entityColumn.Name].Value, t);
								entityColumn.SetValue(entity, objValue);
							}
							catch(Exception) { }
						}
					}

				}
				PropertyInfo prop = entity.GetType().GetProperty("EntityState");
				if(prop != null)
				{
					prop.SetValue(entity, EntityMode.Review);
				}
				lst.Add(entity);
				rs.MoveNext();

			}
			return lst;
		}

		/// <summary>
		/// Convert Recordset to List
		/// </summary>
		/// <param name="rs"></param>
		/// <returns></returns>
		public static List<string> ConvertRSToList(Recordset rs)
		{

			List<string> lst = new List<string>();
			while(!rs.EOF)
			{
				lst.Add(Convert.ToString(rs.Fields[0].Value));
				rs.MoveNext();
			}
			return lst;
		}

		public static List<KeyValuePair<string, string>> ConvertRSToKeyValue(Recordset rs)
		{

			List<KeyValuePair<string, string>> keyValues = new List<KeyValuePair<string, string>>();
			while(!rs.EOF)
			{
				keyValues.Add(new KeyValuePair<string, string>(Convert.ToString(rs.Fields["Key"].Value), Convert.ToString(rs.Fields["Value"].Value)));
				rs.MoveNext();
			}
			return keyValues;
		}

		/// <summary>
		/// converts True/False to Yes/No
		/// </summary>
		/// <param name="flag"></param>
		/// <returns></returns>
		public static string ConvertBoolToYN(bool flag)
		{
			return flag == true ? "Y" : "N";
		}

		/// <summary>
		/// check input fno is of type ploygon
		/// </summary>
		/// <param name="g3eFno"></param>
		/// <returns></returns>
		public static bool CheckForBoundaryFno(int g3eFno)
		{
			bool flag = false;
			Recordset rs = _gtDataContext.MetadataRecordset("G3E_FEATURES_OPTABLE", "G3E_FNO=" + g3eFno + " AND G3E_TYPE=8");
			if(rs != null && rs.RecordCount > 0) { flag = true; }
			return flag;
		}

		/// <summary>
		/// return feature username for given fno
		/// </summary>
		/// <param name="g3eFno"></param>
		/// <returns></returns>
		public static string GetUsernameByFno(int g3eFno)
		{
			string g3eUsername = string.Empty;
			Recordset rs = _gtDataContext.MetadataRecordset("G3E_FEATURES_OPTABLE", "G3E_FNO=" + g3eFno);
			if(rs != null && rs.RecordCount > 0) { g3eUsername = Convert.ToString(rs.Fields["G3E_USERNAME"].Value); }
			return g3eUsername;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="g3eFno"></param>
		/// <returns></returns>
		public static string GetComponentName(short g3eFno, int g3eCno)
		{
			string g3eName = string.Empty;
			Recordset rs = _gtDataContext.MetadataRecordset("G3E_FEATURECOMPS_OPTABLE", "G3E_FNO=" + g3eFno + " AND G3E_CNO=" + g3eCno);
			if(rs != null && rs.RecordCount > 0) { g3eName = Convert.ToString(rs.Fields["G3E_NAME"].Value); }
			return g3eName;
		}

		public static short GetPrimaryGeographicCno(short g3eFno)
		{
			short g3eCno = 0;
			Recordset rs = _gtDataContext.MetadataRecordset("G3E_FEATURES_OPTABLE", "G3E_FNO=" + g3eFno);
			if(rs != null && rs.RecordCount > 0) { g3eCno = Convert.ToInt16(rs.Fields["G3E_PRIMARYGEOGRAPHICCNO"].Value); }
			return g3eCno;
		}

		/// <summary>
		/// check input attribute exists in feature component
		/// </summary>
		/// <param name="g3eAno"></param>
		/// <param name="g3efno"></param>
		/// <returns></returns>
		public static bool CheckAtributeExists(int g3eAno, int g3efno)
		{
			string sqlStmt = "select count(attr.g3e_ano) from G3E_ATTRIBUTEINFO_OPTABLE attr,G3E_FEATURECOMPS_OPTABLE fcomp where attr.g3e_cno=fcomp.g3e_cno and fcomp.g3e_fno={0} and attr.g3e_ano={1}";
			bool flag = false;
			Recordset rs = Execute(string.Format(sqlStmt, g3efno, g3eAno));
			if(rs != null && rs.RecordCount > 0)
			{
				if(Convert.ToInt32(rs.Fields[0].Value) > 0) { flag = true; }
			}
			return flag;
		}

		/// <summary>
		/// check input attributes exists in same feature component
		/// </summary>
		/// <param name="g3eAno1"></param>
		/// <param name="g3eAno2"></param>
		/// <param name="g3efno"></param>
		/// <returns></returns>
		public static bool CheckAtributeExists(int g3eAno1, int g3eAno2, int g3efno)
		{
			string sqlStmt = "select count(distinct attr.g3e_cno) from G3E_ATTRIBUTEINFO_OPTABLE attr,G3E_FEATURECOMPS_OPTABLE fcomp where attr.g3e_cno=fcomp.g3e_cno and fcomp.g3e_fno={0} and attr.g3e_ano in ({1},{2})";
			bool flag = false;
			Recordset rs = Execute(string.Format(sqlStmt, g3efno, g3eAno1, g3eAno2));
			if(rs != null && rs.RecordCount > 0)
			{
				if(Convert.ToInt32(rs.Fields[0].Value) == 1) { flag = true; }
			}
			return flag;
		}

		public static void FitSelectedFeature(short g3eFno, int g3eFid, double zoomfactor = default(double))
		{
			IGTApplication _gtApp = GTClassFactory.Create<IGTApplication>();
			var gtDDCKeyObjs = _gtApp.DataContext.GetDDCKeyObjects(g3eFno, g3eFid, GTComponentGeometryConstants.gtddcgAllPrimary);
			if(gtDDCKeyObjs != null && gtDDCKeyObjs.Count > 0)
			{
				_gtApp.SelectedObjects.Clear();
				_gtApp.SelectedObjects.Add(GTSelectModeConstants.gtsosmAllComponentsOfFeature, gtDDCKeyObjs[0]);
				if(zoomfactor == default(double)) { _gtApp.ActiveMapWindow.FitSelectedObjects(); } else { _gtApp.ActiveMapWindow.FitSelectedObjects(zoomfactor); }
				_gtApp.RefreshWindows();
			}
		}

		/// <summary>
		/// Returns the customer-owned StreetLight CU for the given street light.
		/// ALM 2044 - Set the CU for the current street light.
		/// </summary>
		/// <param name="streetLight"></param>
		/// <returns></returns>
		public static string CustomerOwnedSteetLightCU(string lampType, string wattage, string luminaireStyle)
		{
			string retVal = string.Empty;

			try
			{
				string sql = "select u.cu_id from culib_unit u";
				sql += " join culib_unitattribute ua1 on u.cu_id=ua1.cu_id and ua1.category_c='STREETLIGHT' and ua1.attribute_id='LAMP_TYPE_C' and ua1.attr_value=?";
				sql += " join culib_unitattribute ua2 on u.cu_id=ua2.cu_id and ua2.category_c='STREETLIGHT' and ua2.attribute_id='LAMP_WATT_Q' and ua2.attr_value=?";
				sql += " join culib_unitattribute ua3 on u.cu_id=ua3.cu_id and ua3.category_c='STREETLIGHT' and ua3.attribute_id='LUMIN_STYL_C' and ua3.attr_value=?";
				sql += " join culib_unitattribute ua4 on u.cu_id=ua4.cu_id and ua4.category_c='STREETLIGHT' and ua4.attribute_id='OWNER_TYPE_C' and ua4.attr_value='Customer'";

				Recordset rs = _gtApp.DataContext.OpenRecordset(sql, CursorTypeEnum.adOpenStatic, LockTypeEnum.adLockReadOnly, (int)CommandTypeEnum.adCmdText,
													lampType, wattage, luminaireStyle);

				if(null != rs)
				{
					if(1 == rs.RecordCount && DBNull.Value != rs.Fields[0].Value)
					{
						retVal = rs.Fields[0].Value.ToString();
					}

					if((ObjectStateEnum)rs.State == ObjectStateEnum.adStateOpen)
					{
						rs.Close();
					}

					rs = null;
				}
			}
			catch(Exception ex)
			{
				string exMsg = string.Format("Error occurred in {0} of {1}.{2}{3}", System.Reflection.MethodBase.GetCurrentMethod().Name, "CommonUtil", Environment.NewLine, ex.Message);
				throw new Exception(exMsg);
			}

			return retVal;
		}
	}
}
