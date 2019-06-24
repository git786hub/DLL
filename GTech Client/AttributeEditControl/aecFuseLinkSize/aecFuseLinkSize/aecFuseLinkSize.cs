//----------------------------------------------------------------------------+
//        Class: aecFuseLinkSize
//  Description: This Interface will control read write and read only link size attribute depending upon link type and job type values.
//----------------------------------------------------------------------------+
//     $Author:: hkonda                                   $
//       $Date:: 06/12/17                                 $
//   $Revision:: 1                                        $
//----------------------------------------------------------------------------+
//    $History:: aecFuseLinkSize.cs                     $
// 
// *****************  Version 1  *****************
// User: hkonda     Date: 06/12/17   Time: 18:00  Desc : Created
//----------------------------------------------------------------------------+
using System;
using Intergraph.GTechnology.API;
using Intergraph.GTechnology.Interfaces;
using ADODB;

namespace GTechnology.Oncor.CustomAPI
{
    public class aecFuseLinkSize : IGTAttributeEditControl
    {
        private IGTDataContext m_oDataContext = null;
        private string m_sComponentName = string.Empty;
        private string m_sFieldName = string.Empty;
        private IGTFieldValue m_fieldValue = null;
        private IGTComponents m_componentCollection = null;
        private GTArguments m_GTArguments = null;

        public GTArguments Arguments
        {
            get
            {
                return m_GTArguments;
            }

            set
            {
                m_GTArguments = value;
            }
        }

        public string ComponentName
        {
            get
            {
                return m_sComponentName;
            }

            set
            {
                m_sComponentName = value;
            }
        }

        public IGTComponents Components
        {
            get
            {
                return m_componentCollection;
            }

            set
            {
                m_componentCollection = value;
            }
        }

        public IGTDataContext DataContext
        {
            get
            {
                return m_oDataContext;
            }

            set
            {
                m_oDataContext = value;
            }
        }

        public string FieldName
        {
            get
            {
                return m_sFieldName;
            }

            set
            {
                m_sFieldName = value;
            }
        }

        public IGTFieldValue FieldValue
        {
            get
            {
                return m_fieldValue;
            }

            set
            {
                m_fieldValue = value;
            }
        }

        public bool IsAttributeEditable(int ANO)
        {
            try
            {
                if (CheckIfNonWrJob())
                {
                    return true;
                }
                if (CheckIfLinkTypeIsVista())
                {
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                throw new Exception("There is an error in \"Fuse Link Size\" Attribute Edit Control Interface \n" + ex.Message);
            }
        }

        /// <summary>
        /// Method to check whether the Job is of type NON-WR.
        /// </summary>
        /// <returns>true, if job is of type NON-WR</returns>
        private bool CheckIfNonWrJob()
        {
            Recordset jobInfoRs = null;
            try
            {
                string jobType = string.Empty;
                jobInfoRs = GetRecordSet(string.Format("select G3E_JOBTYPE from g3e_job where g3e_identifier  = '{0}'", m_oDataContext.ActiveJob));
                if (jobInfoRs != null && jobInfoRs.RecordCount > 0)
                {
                    jobInfoRs.MoveFirst();
                    jobType = Convert.ToString(jobInfoRs.Fields["G3E_JOBTYPE"].Value);
                }
                return !string.IsNullOrEmpty(jobType) && jobType.ToUpper() == "NON-WR" ? true : false;
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                if (jobInfoRs != null)
                {
                    jobInfoRs.Close();
                }
                jobInfoRs = null;
            }
        }

        /// <summary>
        /// Method to check whether the Link Type is VISTA
        /// </summary>
        /// <returns>true, if link type is VISTA</returns>
        private bool CheckIfLinkTypeIsVista()
        {
            try
            {
                string linkType = string.Empty;
                Recordset primaryFuseUnitRs = Components[m_sComponentName].Recordset;
                if (primaryFuseUnitRs != null && primaryFuseUnitRs.RecordCount > 0)
                {
                    primaryFuseUnitRs.MoveFirst();
                    linkType = Convert.ToString(primaryFuseUnitRs.Fields["LINK_TYPE_C"].Value);
                }
                return !string.IsNullOrEmpty(linkType) && linkType.ToUpper() == "VISTA" ? true : false;
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Method to execute sql query and return the result record set
        /// </summary>
        /// <param name="sqlString"></param>
        /// <returns> result recordset</returns>
        private Recordset GetRecordSet(string sqlString)
        {
            try
            {
                int outRecords = 0;
                Command command = new Command();
                command.CommandText = sqlString;
                Recordset results = m_oDataContext.ExecuteCommand(command, out outRecords);
                return results;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
