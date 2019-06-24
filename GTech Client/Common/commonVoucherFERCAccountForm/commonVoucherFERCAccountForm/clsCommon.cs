using System;
using ADODB;
using System.Data;
using System.Data.OleDb;
using Intergraph.GTechnology.API;

namespace GTechnology.Oncor.CustomAPI
{
    class clsCommon
    {
        #region Private variables

        Recordset m_oWorkpointAttributeRS = null;

        #endregion

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="p_attributeRS">Recordset of the Voucher attributes which needs to be displayed in form</param>
        public clsCommon(Recordset p_attributeRS)
        {
            m_oWorkpointAttributeRS = p_attributeRS;
        }
        /// <summary>
        /// Method to Load VoucherFERCAccount Data to grid.
        /// </summary>
        /// <param name="p_workPointFeature">Current feature which is placed</param>
        public DataTable LoadVoucherFERCAccountData(IGTKeyObject p_workPointFeature)
        {
            DataTable results = new DataTable("Results");
            bool easementAssociated = false;
            try
            {
                if (p_workPointFeature != null)
                {
                    OleDbDataAdapter oleDbDataAdapter = new OleDbDataAdapter();
                    oleDbDataAdapter.Fill(results, m_oWorkpointAttributeRS);

                    results.Columns.Add("At Work Point?", typeof(bool)).SetOrdinal(0);

                    for (int i = 0; i < results.Rows.Count; i++)
                    {
                        if (Convert.ToInt32(results.Rows[i]["G3E_FID"]) == p_workPointFeature.FID)
                        {
                            results.Rows[i]["At Work Point?"] = true;
                        }
                        if (Convert.ToInt16(results.Rows[i]["ASSOC_FNO"] == DBNull.Value ? 0 : results.Rows[i]["ASSOC_FNO"]) == 220)
                        {
                            easementAssociated = true;
                        }
                    }

                    results.Columns.Remove("G3E_FID");
                    results.Columns.Remove("ASSOC_FNO");
                    results.Columns[1].ColumnName = "Prime";
                    results.Columns[2].ColumnName = "Sub";
                    results.Columns[3].ColumnName = "Description";
                    results = results.DefaultView.ToTable(true);

                    if (easementAssociated)
                    {
                        DataRow dr = results.NewRow();
                        dr["Prime"] = 300;
                        dr["Sub"] = 7000;

                        results.Rows.Add(dr);
                    }
                    results.DefaultView.Sort = "At Work Point? DESC,Prime ASC,Sub ASC";
                }
                else
                {
                    //  08/04/2019          Prathyusha                  Modfied as part of ALM-2197
                    for (int i = 0; i < m_oWorkpointAttributeRS.Fields.Count; i++)
                    {
                        results.Columns.Add(m_oWorkpointAttributeRS.Fields[i].Name);
                    }

                    if (m_oWorkpointAttributeRS != null && m_oWorkpointAttributeRS.RecordCount > 0)
                    {
                        m_oWorkpointAttributeRS.MoveFirst();

                        while (!m_oWorkpointAttributeRS.EOF)
                        {
                            DataRow row = results.NewRow();

                            for (int x = 0; x < results.Columns.Count; x++)
                            {
                                row[x] = m_oWorkpointAttributeRS.Fields[m_oWorkpointAttributeRS.Fields[x].Name].Value;
                            }
                            results.Rows.Add(row);

                            m_oWorkpointAttributeRS.MoveNext();
                        }
                        results = results.DefaultView.ToTable(true);
                    }
                }
            }
            catch
            {
                throw;
            }
            return results;
        }
    }
}
