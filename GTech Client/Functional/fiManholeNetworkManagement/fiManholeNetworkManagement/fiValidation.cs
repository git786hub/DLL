// ===================================================
//  Copyright 2018 Intergraph Corp.
//  File Name: fiValidation.cs
//
//  Description:    Class to validate the FI.
//  Remarks: 
//  Modification History
//  Date                Author                      Comments
//  --------            --------------              --------
//  26/04/2018          Sitara                      Created
// ======================================================
using System;
using ADODB;
using Intergraph.GTechnology.API;

namespace GTechnology.Oncor.CustomAPI
{
    public class fiValidation
    {
        private IGTDataContext m_oGTDataContext;

        public fiValidation(IGTDataContext p_gTDataContext)
        {
            m_oGTDataContext = p_gTDataContext;
        }
        public bool ValidateFI()
        {
            bool validate = false;
            Recordset jobRs = null;
            string strWRType = null;
            try
            {
                if (m_oGTDataContext.IsRoleGranted("PRIV_DESIGN_NET"))
                {
                    DataAccessLayer dataAccessLayer = new DataAccessLayer(m_oGTDataContext);
                    jobRs = dataAccessLayer.GetRecordset("SELECT WR_TYPE_C FROM G3E_JOB WHERE G3E_IDENTIFIER=:1", m_oGTDataContext.ActiveJob);
                    if (jobRs != null && jobRs.RecordCount > 0)
                    {
                        jobRs.MoveFirst();
                        strWRType = Convert.ToString(jobRs.Fields["WR_TYPE_C"].Value);
                        if (!string.IsNullOrEmpty(strWRType) && strWRType.ToUpper() == "NET")
                        {
                            validate = true;
                        }
                    }
                }
            }
            catch
            {
                throw;
            }
            finally
            {
                if(jobRs != null)
                {
                    jobRs.Close();
                    jobRs = null;
                }
            }
            return validate;
        }
    }
}
