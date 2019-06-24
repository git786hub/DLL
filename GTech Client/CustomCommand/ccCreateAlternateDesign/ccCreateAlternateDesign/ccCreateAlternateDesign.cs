using System;
using Intergraph.GTechnology.Interfaces;
using Intergraph.GTechnology.API;
using ADODB;
using System.Windows.Forms;
using System.Collections.Generic;
using System.Text;

namespace GTechnology.Oncor.CustomAPI
{
    public class ccCreateAlternateDesign : IGTCustomCommandModal
    {
        public IGTTransactionManager TransactionManager { set => throw new NotImplementedException(); }

        public void Activate()
        {
            try
            {
                frmCreateAlternateDesign dialogWindow = new frmCreateAlternateDesign();
                IGTApplication oApp = GTClassFactory.Create<IGTApplication>();
                dialogWindow.Show(oApp.ApplicationWindow);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error occured in Create Alternate Design -" + ex.Message, "G/Technology", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }

    internal class DataProvider : IDisposable
    {
        private IGTApplication m_gtApp = null;
        private IGTJobManagementService m_jobManager = null;
        private IGTDataContext m_gtDataContext = null;
        private IGTJobHelper m_jobHelper = null;
        private Recordset m_RS = null;
        private bool m_hasAlternate;
        string m_alternateJobWithHighestSuffix = string.Empty;

        public bool HasAlternate
        {
            get
            {
                return m_hasAlternate;
            }
        }

        public ADODB.Recordset JobRecordData
        {
            get
            {
                return m_RS;
            }
        }

        public DataProvider()
        {
            m_gtApp = GTClassFactory.Create<IGTApplication>();
            m_jobManager = GTClassFactory.Create<IGTJobManagementService>();
            m_gtDataContext = m_gtApp.DataContext;
            m_jobHelper = GTClassFactory.Create<IGTJobHelper>();
        }

        public void InitialLoadData()
        {
            try
            {
                string activeJob = m_jobHelper.ActiveJob;
                string SQL = "select WR_NBR, G3E_DESCRIPTION, G3E_JOBTYPE, G3E_JOBSTATUS, G3E_IDENTIFIER from G3E_JOB where G3E_IDENTIFIER = ?";
                m_RS = m_gtDataContext.OpenRecordset(SQL, CursorTypeEnum.adOpenStatic, LockTypeEnum.adLockReadOnly, (int)ADODB.CommandTypeEnum.adCmdText, activeJob);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void CheckForAlternate(string wrNumber)
        {
            try
            {
                m_hasAlternate = false; //make sure this is defaulted to false.
                string activeJob = m_jobHelper.ActiveJob;

                string SQL = "select length(ltrim(rtrim(WR_ALT_DESIGN))), g3e_identifier, WR_NBR, WR_ALT_DESIGN from g3e_job where WR_NBR = ? group by length(WR_ALT_DESIGN), g3e_identifier, WR_NBR, WR_ALT_DESIGN order by length(ltrim(rtrim(WR_ALT_DESIGN))),wr_alt_design asc";

                m_RS = m_gtDataContext.OpenRecordset(SQL, CursorTypeEnum.adOpenStatic, LockTypeEnum.adLockReadOnly, (int)ADODB.CommandTypeEnum.adCmdText, wrNumber);

                if (m_RS.RecordCount > 0)
                {
                    m_RS.MoveFirst();
                    while (!m_RS.EOF)
                    {
                        if (m_RS.Fields["G3E_IDENTIFIER"].Value.ToString() != activeJob)
                        {
                            m_hasAlternate = true; //alternate(s) do exist for active job
                            //break;              // Need to know the existing alternate job with the highest lettered suffix, so continue till last record.
                            m_alternateJobWithHighestSuffix = m_RS.Fields["G3E_IDENTIFIER"].Value.ToString();
                        }
                        m_RS.MoveNext();
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public string GetAlternateSuffix()
        {
            try
            {
                m_RS.MoveLast();  //recordset is already sorted, we just want the last record, in order to read the suffix used in WR_ALT_DESIGN field

                string wrAltDesignSuffix = Convert.ToString(m_RS.Fields["WR_ALT_DESIGN"].Value);
                wrAltDesignSuffix = wrAltDesignSuffix.ToUpper().Trim();

                //if wr_nbr is only one character (A, B, C, etc.)
                if (wrAltDesignSuffix.Length == 1)
                {
                    char charSuffix = Convert.ToChar(wrAltDesignSuffix);
                    if (wrAltDesignSuffix != "Z")   //example: this converts suffix from "C" to "D"
                    {
                        charSuffix++;
                        wrAltDesignSuffix = Convert.ToString(charSuffix);
                    }
                    else //wrAltDesignSuffix = "Z".
                    {
                        wrAltDesignSuffix = "AA";
                    }
                }
                //if wr nbr is more than one character (AA, AB, AC, etc.)
                else
                {
                    //get first character
                    string firstCharacterString = wrAltDesignSuffix.Substring(0, 1);

                    //get second character
                    string secondCharacterString = wrAltDesignSuffix.Substring(wrAltDesignSuffix.Length - 1);

                    if (secondCharacterString == "Z")
                    {
                        if (firstCharacterString == "Z")
                        {
                            return wrAltDesignSuffix = "ZZ ERROR"; //characters cannot exceed "ZZ"
                        }
                        char firstChar = Convert.ToChar(firstCharacterString);
                        firstChar++;
                        secondCharacterString = "A";
                        firstCharacterString = firstChar.ToString();
                    }
                    else //second character does not equal "Z"
                    {
                        char secondChar = Convert.ToChar(secondCharacterString);
                        secondChar++;
                        secondCharacterString = secondChar.ToString();
                    }
                    //concat the first and second letter together to get the new full suffix for WR_ALT_DESIGN
                    wrAltDesignSuffix = string.Concat(firstCharacterString, secondCharacterString);
                }
                return wrAltDesignSuffix;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void SaveData(string altSuffix, bool copyJobEdits, string newDescription)
        {
            try
            {
                m_jobHelper.DataContext = m_gtDataContext;
                m_jobManager.DataContext = m_gtDataContext;
                string activeJob = m_jobHelper.ActiveJob;
                string SQL = "";
                int RecordsAffected;

                if (!m_hasAlternate)  //active job does not have alternate(s)
                {
                    string altJob = activeJob + "-" + altSuffix;

                    //copy job edits and create new alternate design
                    if (copyJobEdits)
                    {
                        m_jobManager.CopyJob(activeJob, altJob);
                        SQL = "update G3E_JOB set WR_ALT_DESIGN=?, WR_NBR=?, G3E_DESCRIPTION=? where G3E_IDENTIFIER=?";
                        m_gtDataContext.Execute(SQL, out RecordsAffected, (int)ADODB.CommandTypeEnum.adCmdText, altSuffix, activeJob, newDescription, altJob);
                    }
                    //create new alternate design
                    else
                    {
                        m_jobManager.CreateJob(altJob);
                        SQL = "update G3E_JOB set WR_ALT_DESIGN=?, WR_NBR=?, G3E_DESCRIPTION=? where G3E_IDENTIFIER=?";
                        m_gtDataContext.Execute(SQL, out RecordsAffected, (int)ADODB.CommandTypeEnum.adCmdText, altSuffix, activeJob, newDescription, altJob);
                    }

                    PopulateJobPropertiesFromAlternateHighestSuffix(altJob);
                    //update g3e_job table for active job
                    SQL = "update G3E_JOB set G3E_IDENTIFIER=?, WR_ALT_DESIGN=?, WR_NBR=? where G3E_IDENTIFIER=?";
                    m_gtDataContext.Execute(SQL, out RecordsAffected, (int)ADODB.CommandTypeEnum.adCmdText, activeJob + "-A", "A", activeJob, activeJob);

                    //also update ltt_identifiers table for active job
                    SQL = "update LTT_IDENTIFIERS set LTT_NAME=? where LTT_NAME=?";
                    m_gtDataContext.Execute(SQL, out RecordsAffected, (int)ADODB.CommandTypeEnum.adCmdText, activeJob + "-A", activeJob);

                    m_jobManager.EditJob(altJob); //activate newly created alt design
                    SQL = "commit";
                    m_gtDataContext.Execute(SQL, out RecordsAffected, (int)ADODB.CommandTypeEnum.adCmdText); //commit changes to database
                }
                else  //active job does have alternate(s)
                {
                    string activeJobNumberOnly = activeJob.Substring(0, activeJob.IndexOf("-"));
                    string altJob = activeJobNumberOnly + "-" + altSuffix;
                    if (copyJobEdits)
                    {
                        m_jobManager.CopyJob(activeJob, altJob);

                        SQL = "update G3E_JOB set WR_ALT_DESIGN=?, G3E_DESCRIPTION=?, WR_NBR=? where G3E_IDENTIFIER=?";
                        m_gtDataContext.Execute(SQL, out RecordsAffected, (int)ADODB.CommandTypeEnum.adCmdText, altSuffix, newDescription, activeJobNumberOnly, altJob);
                    }

                    else
                    {
                        m_jobManager.CreateJob(altJob);

                        SQL = "update G3E_JOB set WR_ALT_DESIGN=?, G3E_DESCRIPTION=?, WR_NBR=? where G3E_IDENTIFIER=?";
                        m_gtDataContext.Execute(SQL, out RecordsAffected, (int)ADODB.CommandTypeEnum.adCmdText, altSuffix, newDescription, activeJobNumberOnly, altJob);
                    }
                    PopulateJobPropertiesFromAlternateHighestSuffix(altJob);
                    m_jobManager.EditJob(altJob); //activate newly created alt design
                    SQL = "commit";
                    m_gtDataContext.Execute(SQL, out RecordsAffected, (int)ADODB.CommandTypeEnum.adCmdText); //commit changes to database
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void PopulateJobPropertiesFromAlternateHighestSuffix(string currentAltJob)
        {
            int recordAffected = 0;
            int index = 0;
            string SQL = String.Empty;
            string value = string.Empty;
            StringBuilder updateStatement = new StringBuilder();
            updateStatement.Append("UPDATE G3E_JOB SET ");
            List<string> columnList = new List<string>() { "G3E_ID", "G3E_IDENTIFIER", "WR_ALT_DESIGN", "WR_NBR", "G3E_DESCRIPTION", "DESIGNER_RACF", "DESIGNER_RACFID", "DESIGNER_NM", "DESIGNER_UID", "G3E_DATECREATED", "G3E_DATEPOSTED", "G3E_DATECLOSED", "G3E_PROCESSINGSTATUS" };
            try
            {
                string metaDataQuery = "SELECT COLUMN_NAME, DATA_TYPE FROM ALL_TAB_COLUMNS  WHERE TABLE_NAME = 'G3E_JOB'";
                Recordset metaDataRs = m_gtDataContext.OpenRecordset(metaDataQuery, CursorTypeEnum.adOpenStatic, LockTypeEnum.adLockReadOnly, (int)ADODB.CommandTypeEnum.adCmdText);
                metaDataRs.MoveFirst();

                updateStatement.Append(" G3E_DATECREATED  = sysdate , ");
                updateStatement.Append(" G3E_DATEPOSTED = null , ");
                updateStatement.Append(" G3E_DATECLOSED = null , ");
                updateStatement.Append(" G3E_PROCESSINGSTATUS  = 1 , ");

                StringBuilder selectJobPropertiesSql = new StringBuilder();
                selectJobPropertiesSql.Append(" SELECT ");

                while (!metaDataRs.EOF)
                {
                    selectJobPropertiesSql.Append(Convert.ToString(metaDataRs.Fields["COLUMN_NAME"].Value) + " , " );
                    metaDataRs.MoveNext();
                }

                selectJobPropertiesSql.Append("FROM G3E_JOB WHERE G3E_IDENTIFIER = ?");

                string jobName = string.IsNullOrEmpty(m_alternateJobWithHighestSuffix) ? m_gtDataContext.ActiveJob : m_alternateJobWithHighestSuffix;
                SQL = selectJobPropertiesSql.ToString();
                index = SQL.LastIndexOf(',');
                SQL = SQL.Remove(index, 1);
                Recordset rs = m_gtDataContext.Execute(SQL, out recordAffected, (int)CommandTypeEnum.adCmdText, jobName);

                
                if (rs != null && rs.RecordCount > 0)
                {
                    rs.MoveFirst();
                }
                metaDataRs.MoveFirst();
                while (!metaDataRs.EOF && !rs.EOF)
                {
                    if (!columnList.Contains(metaDataRs.Fields["COLUMN_NAME"].Value.ToString()))
                    {
                        string type = Convert.ToString(metaDataRs.Fields["DATA_TYPE"].Value);
                        string field = Convert.ToString(metaDataRs.Fields["COLUMN_NAME"].Value);

                        switch (type)
                        {
                            case "VARCHAR2":
                                value = string.IsNullOrEmpty(Convert.ToString(rs.Fields[field].Value)) ? "null" : "'" + Convert.ToString(rs.Fields[field].Value) + "'";
                                break;
                            case "NUMBER":
                                value = string.IsNullOrEmpty(Convert.ToString(rs.Fields[field].Value)) ? "null" : Convert.ToString(rs.Fields[field].Value);
                                break;
                            case "DATE":
                                value = string.IsNullOrEmpty(Convert.ToString(rs.Fields[field].Value)) ? "null" : " TO_DATE('" + string.Format(Convert.ToDateTime(rs.Fields[field].Value).ToString("dd{0}MMM{0}yy"), "-") + "', 'DD-MON-YY') ";
                                break;
                        }
                        updateStatement.Append(" " + field + " = " + value + ", ");
                    }
                    metaDataRs.MoveNext();
                }

                SQL = updateStatement.ToString();
                index = SQL.LastIndexOf(',');
                SQL = SQL.Remove(index, 1);
                SQL = string.Format (SQL + " WHERE G3E_IDENTIFIER = '{0}'", currentAltJob);

                m_gtDataContext.Execute(SQL, out recordAffected, (int)ADODB.CommandTypeEnum.adCmdText);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void Dispose()
        {
            if (m_RS != null)
            {
                m_RS.Close();
            }
            m_RS = null;
            m_gtApp = null;
            m_jobManager = null;
            m_gtDataContext = null;
            m_jobHelper = null;
        }
    }
}
