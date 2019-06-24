using System;
using ADODB;
using Intergraph.GTechnology.API;

namespace GTechnology.Oncor.CustomAPI
{
    public class CheckValidation
    {
        IGTDataContext m_gTDataContext = null;
        public CheckValidation(IGTDataContext gTDataContext)
        {
            m_gTDataContext = gTDataContext;
        }

        /// <summary>
        /// Verify the job is WR.
        /// </summary>
        /// <returns></returns>
        public bool IsWRJob()
        {
            string sql = "";
            Recordset rsValidate = null;
            string m_strJobtype = "";
            try
            {
                sql = "select G3E_JOBTYPE,G3E_IDENTIFIER from G3E_JOB where G3E_IDENTIFIER=?";
                rsValidate = m_gTDataContext.OpenRecordset(sql, ADODB.CursorTypeEnum.adOpenDynamic, ADODB.LockTypeEnum.adLockReadOnly,
                        (int)ADODB.CommandTypeEnum.adCmdText, m_gTDataContext.ActiveJob);

                if (rsValidate.RecordCount > 0)
                {
                    rsValidate.MoveFirst();
                    if (!rsValidate.EOF && !rsValidate.BOF)
                    {
                        m_strJobtype = Convert.ToString(rsValidate.Fields[0].Value);
                    }
                }

                if (!string.IsNullOrEmpty(m_strJobtype))
                {
                    if (m_strJobtype == "NON-WR")
                    {
                        return false;
                    }
                }
            }
            catch
            {
                throw;
            }
            finally
            {
                rsValidate.Close();
                rsValidate = null;
            }

            return true;
        }
    }
}
