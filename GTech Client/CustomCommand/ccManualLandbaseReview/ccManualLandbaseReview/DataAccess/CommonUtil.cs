using ADODB;
using Intergraph.GTechnology.API;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;

namespace GTechnology.Oncor.CustomAPI.DataAccess
{
    public static class CommonUtil
    {

        static IGTApplication _gtApp = GTClassFactory.Create<IGTApplication>();
        static IGTDataContext _gtDataContext = _gtApp.DataContext;


        /// <summary>
        /// Execute SQL Statement
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sqlStmt"></param>
        /// <returns></returns>
        public static List<T> Execute<T>(string sqlStmt)
        {
            int recordsAffected;
            ADODB.Recordset rs = null;
            rs = _gtDataContext.Execute(sqlStmt, out recordsAffected, (int)ADODB.CommandTypeEnum.adCmdText);
            return ConvertRSToEntity<T>(rs);
        }


        public static List<KeyValuePair<TKey, TValue>> Execute<TKey, TValue>(string sqlStmt)
        {
            int recordsAffected;
            ADODB.Recordset rs = null;
            rs = _gtDataContext.Execute(sqlStmt, out recordsAffected, (int)ADODB.CommandTypeEnum.adCmdText);
            return ConvertRSToKeyValue<TKey, TValue>(rs);
        }

        /// <summary>
        /// Convert Recordset to Entity
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="rs"></param>
        /// <returns></returns>
        public static List<T> ConvertRSToEntity<T>(Recordset rs)
        {

            List<T> lst = new List<T>();
            while (!rs.EOF)
            {
                T entity = Activator.CreateInstance<T>();
                foreach (var entityColumn in entity.GetType().GetProperties())
                {
                    if (entityColumn != null)
                    {

                        try
                        {
                            Type t = Nullable.GetUnderlyingType(entityColumn.PropertyType) ?? entityColumn.PropertyType;
                            object objValue = (rs.Fields[entityColumn.Name].Value == null) ? null : Convert.ChangeType(rs.Fields[entityColumn.Name].Value, t);
                            entityColumn.SetValue(entity, objValue);
                        }
                        catch (Exception) { }
                    }

                }
                lst.Add(entity);
                rs.MoveNext();
            }
            return lst;
        }


        /// <summary>
        /// Return list with key and value
        /// </summary>
        /// <typeparam name="TKey"></typeparam>
        /// <typeparam name="TValue"></typeparam>
        /// <param name="rs"></param>
        /// <returns></returns>
        public static List<KeyValuePair<TKey, TValue>> ConvertRSToKeyValue<TKey, TValue>(Recordset rs)
        {

            List<KeyValuePair<TKey, TValue>> keyValues = new List<KeyValuePair<TKey, TValue>>();
            while (!rs.EOF)
            {
                keyValues.Add(new KeyValuePair<TKey, TValue>((TKey)Convert.ChangeType(rs.Fields["Key"].Value, typeof(TKey)), (TValue)Convert.ChangeType(rs.Fields["Value"].Value, typeof(TValue))));
                rs.MoveNext();
            }
            return keyValues;
        }

        /// <summary>
        /// Fit Selected Feature Instance on Map window
        /// </summary>
        /// <param name="g3eFno"></param>
        /// <param name="g3eFid"></param>
        /// <param name="zoomfactor"></param>
        public static void FitSelectedFeature(short g3eFno, int g3eFid, double zoomfactor = default(double))
        {
            IGTApplication _gtApp = GTClassFactory.Create<IGTApplication>();
            var gtDDCKeyObjs = _gtApp.DataContext.GetDDCKeyObjects(g3eFno, g3eFid, GTComponentGeometryConstants.gtddcgAllPrimary);
            if (gtDDCKeyObjs != null && gtDDCKeyObjs.Count > 0)
            {
                _gtApp.SelectedObjects.Clear();
                _gtApp.SelectedObjects.Add(GTSelectModeConstants.gtsosmAllComponentsOfFeature, gtDDCKeyObjs[0]);
                if (zoomfactor == default(double)) { _gtApp.ActiveMapWindow.FitSelectedObjects(); } else { _gtApp.ActiveMapWindow.FitSelectedObjects(zoomfactor); }
                _gtApp.RefreshWindows();
            }
        }
    }
}
