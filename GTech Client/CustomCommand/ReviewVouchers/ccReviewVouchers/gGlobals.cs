using System;
using Intergraph.GTechnology.API;
using System.Data;
using System.Windows.Forms;
using System.Data.OleDb;
using ADODB;

namespace GTechnology.Oncor.CustomAPI
{
    class gGlobals
    {
        #region Properties
        static internal IGTTransactionManager m_oGTTransactionManager = null;
        static internal IGTApplication m_oGTApp = null;
        static internal IGTCustomCommandHelper m_oGTCustomCommandHelper;
        static internal int count = 1;
        static internal int workPointFNO = 191;
        #endregion

        #region Methods
        static internal void ExitCommand()
        {
            try
            {
                m_oGTApp.Application.SetStatusBarText(GTStatusPanelConstants.gtaspcMessage, "");
                m_oGTApp.Application.EndWaitCursor();
                if (m_oGTCustomCommandHelper != null)
                {
                    m_oGTCustomCommandHelper.Complete();
                    m_oGTCustomCommandHelper = null;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error in Review Vouchers command: " + ex.Message, "G/Technology", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                m_oGTCustomCommandHelper = null;
                m_oGTTransactionManager = null;
            }
        }
        /// <summary>
        /// Method to get the required data to ReviewVouchers command.
        /// </summary>
        static internal DataTable GetData()
        {
            string selectCommand = null;
            Recordset rs = null;
            DataTable dt = new DataTable();
            try
            {
                selectCommand = "SELECT WP_NBR,VOUCHER_C,FERC_PRIME_ACCT,FERC_SUB_ACCT,COST_COMPONENT_C,AMOUNT_USD,DESIGN_ASBUILT_C,REQUEST_D,REQUEST_UID,COMMENTS FROM VOUCHER_N V, WORKPOINT_N W WHERE V.G3E_FID = W.G3E_FID AND V.G3E_FNO =" + workPointFNO + " AND W.WR_NBR = '" + m_oGTApp.DataContext.ActiveJob + "'";
                rs = gGlobals.m_oGTApp.Application.DataContext.OpenRecordset(selectCommand, ADODB.CursorTypeEnum.adOpenStatic,
                    ADODB.LockTypeEnum.adLockReadOnly, (int)ADODB.CommandTypeEnum.adCmdText);
                OleDbDataAdapter daComponents = new OleDbDataAdapter();
                daComponents.Fill(dt, rs);
                dt.Columns[0].ColumnName = "WP#";
                dt.Columns[1].ColumnName = "Code";
                dt.Columns[2].ColumnName = "Prime Acct";
                dt.Columns[3].ColumnName = "Sub Acct";
                dt.Columns[4].ColumnName = "Cost Comp.";
                dt.Columns[5].ColumnName = "Est. Amount";
                dt.Columns[6].ColumnName = "Design/AsBuilt";
                dt.Columns[7].ColumnName = "Date Requested";
                dt.Columns[8].ColumnName = "Requested by";
                dt.Columns[9].ColumnName = "Comments";
                return dt;
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion
    }
}
