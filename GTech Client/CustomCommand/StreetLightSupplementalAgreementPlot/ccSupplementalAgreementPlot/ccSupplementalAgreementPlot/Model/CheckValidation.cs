using System;
using System.Collections.Generic;
using Intergraph.GTechnology.API;
using ADODB;

namespace GTechnology.Oncor.CustomAPI.Model
{
    public class CheckValidation
    {
        IGTDataContext m_gTDataContext;
        IGTDDCKeyObjects m_gTDDCKeyObjects;
        IGTKeyObjects m_gTKeyObjects;
        IGTApplication m_gTApplication;
        IGTCustomCommandHelper m_gTCustomCommandHelper;
        public string[] m_SelectedEsiLocations;
        string m_strMessage = "";
        string m_strParamName = "";
        public int m_descriptionId = 0;

        IGTNamedPlot m_gTNamedPlot = null;
        public IGTDataContext gTDataContext
        {
            get { return m_gTDataContext; }
            set { m_gTDataContext = value; }
        }
        public IGTDDCKeyObjects gTDDCKeyObjects
        {
            get { return m_gTDDCKeyObjects; }
            set { m_gTDDCKeyObjects = value; }
        }
        public IGTKeyObjects gTKeyObjects
        {
            get { return m_gTKeyObjects; }
            set { m_gTKeyObjects = value; }
        }
        public IGTApplication gTApplication
        {
            get { return m_gTApplication; }
            set { m_gTApplication = value; }
        }
        public IGTCustomCommandHelper gTCustomCommandHelper
        {
            get { return m_gTCustomCommandHelper; }
            set { m_gTCustomCommandHelper = value; }
        }

        string m_ActiveWorkRequest;
        public string ActiveWorkRequest { get { return m_ActiveWorkRequest; } set { m_ActiveWorkRequest = value; } }

        public string strErrorMessage { get { return m_strMessage; } set { m_strMessage = value; } }        
        public string strParamName { get { return m_strParamName; } set { m_strParamName = value; } }
        public IGTNamedPlot gTNamedPlot { get { return m_gTNamedPlot; } set { m_gTNamedPlot = value; } }

        public CheckValidation(IGTDataContext gTDC, IGTDDCKeyObjects gTDDCKey, IGTApplication gTApp, IGTCustomCommandHelper gTCCHeler)
        {
            gTDataContext = gTDC;
            gTDDCKeyObjects = gTDDCKey;
            gTApplication = gTApp;
            gTCustomCommandHelper = gTCCHeler;
        }
        public bool IsValid()
        {
            try
            {
                if (IsWRJob())
                {
                    if (IsSelectedSethaveStreetLight())
                    {
                        if (IsSelectSetHasESILocation())
                        {
                            if (IsSelectSetHaSameCustomer())
                            {
                                if (IsTemplateExist())
                                {
                                    return true;
                                }
                                else
                                {
                                    strErrorMessage = "Unable to generate plot; missing template Supplemental Plot.";
                                }
                            }
                            else
                            {
                                strErrorMessage = "All selected Street Lights must be associated with the same customer.";
                            }
                        }
                        else
                        {
                            strErrorMessage = "All selected Street Lights must have an ESI Location assigned to them.";
                        }
                    }
                    else
                    {
                        strErrorMessage = "Select set must contain at least one Street Light feature.";
                    }
                }
                else
                {
                    strErrorMessage = "Active job must be a WR job.";
                }
            }
            catch
            {
                throw;
            }
            return false;
        }

        private bool IsTemplateExist()
        {
            Recordset rs = null;
            string strPlotTemplateName = "";
            IGTNamedPlots gTNamedPlots = null;
            try
            {
                rs = m_gTDataContext.OpenRecordset("select PARAM_VALUE from SYS_GENERALPARAMETER where PARAM_NAME=:1", CursorTypeEnum.adOpenStatic,
                               LockTypeEnum.adLockReadOnly, (int)CommandTypeEnum.adCmdText, strParamName);

                if (rs != null && rs.RecordCount > 0)
                {
                    rs.MoveFirst();
                    strPlotTemplateName = Convert.ToString(rs.Fields[0].Value);
                    if (!string.IsNullOrEmpty(strPlotTemplateName))
                    {
                        gTNamedPlots = gTApplication.NamedPlots;
                        if (gTNamedPlots != null && gTNamedPlots.Count > 0)
                        {
                            foreach (IGTNamedPlot npt in gTNamedPlots)
                            {
                                if (npt.Name == strPlotTemplateName)
                                {
                                    gTNamedPlot = npt;
                                    return true;
                                }
                            }
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
                rs.Close();
                rs = null;
            }

            return false;
        }
        private bool IsSelectedSethaveStreetLight()
        {
            bool found = false;
            try
            {
                gTKeyObjects = GTClassFactory.Create<IGTKeyObjects>();
                if (gTDDCKeyObjects != null && gTDDCKeyObjects.Count > 0)
                {
                    IList<int> SelectedFeatureIDList = new List<int>();
                    foreach (IGTDDCKeyObject ddcKey in gTDDCKeyObjects)
                    {
                        if (ddcKey.FNO == 56)
                        {
                            found = true;
                        }
                        if (!SelectedFeatureIDList.Contains(ddcKey.FID))
                        {
                            SelectedFeatureIDList.Add(ddcKey.FID);
                            gTKeyObjects.Add(gTDataContext.OpenFeature(ddcKey.FNO, ddcKey.FID));
                        }
                    }
                }
            }
            catch
            {
                throw;
            }
            return found;
        }
        private bool IsSelectSetHaSameCustomer()
        {
            string sql = "";
            Recordset rsValidate = null;
            string esiLocations = null;

            try
            {
                foreach (string esiL in m_SelectedEsiLocations)
                {
                    if (!string.IsNullOrEmpty(esiLocations))
                    {
                        esiLocations = esiLocations + ",'" + esiL + "'";
                    }
                    else
                    {
                        esiLocations = "'" + esiL + "'";
                    }
                }

                if (!string.IsNullOrEmpty(esiLocations))
                {
                    esiLocations = "(" + esiLocations + ")";
                    sql = "select distinct DESCRIPTION_ID from STLT_ACCOUNT where ESI_LOCATION in ?";

                    sql = string.Format("select distinct DESCRIPTION_ID from STLT_ACCOUNT where ESI_LOCATION in {0}", esiLocations);

                    rsValidate = m_gTDataContext.OpenRecordset(sql, ADODB.CursorTypeEnum.adOpenDynamic, ADODB.LockTypeEnum.adLockReadOnly,
                        (int)ADODB.CommandTypeEnum.adCmdText, null);

                    if (rsValidate.RecordCount > 0)
                    {
                        rsValidate.MoveFirst();
                        if (!rsValidate.EOF && !rsValidate.BOF)
                        {
                            if (rsValidate.RecordCount > 1)
                            {
                                return false;
                            }

                            m_descriptionId = Convert.ToInt32(rsValidate.Fields[0].Value);
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
                rsValidate.Close();
                rsValidate = null;
            }

            return true;
        }
        private bool IsSelectSetHasESILocation()
        {
            bool esiLocation = true;

            try
            {
                m_SelectedEsiLocations = new string[gTKeyObjects.Count];
                int count = 0;

                foreach (IGTKeyObject gtKey in gTKeyObjects)
                {
                    if (gtKey.FNO == 56)
                    {
                        if (gtKey.Components["STREETLIGHT_N"] != null && gtKey.Components["STREETLIGHT_N"].Recordset != null &&
                            gtKey.Components["STREETLIGHT_N"].Recordset.RecordCount > 0)
                        {

                            if (string.IsNullOrEmpty(Convert.ToString(gtKey.Components["STREETLIGHT_N"].Recordset.Fields["ACCOUNT_ID"].Value)))
                            {
                                esiLocation = false;
                            }
                            m_SelectedEsiLocations[count] = Convert.ToString(gtKey.Components["STREETLIGHT_N"].Recordset.Fields["ACCOUNT_ID"].Value);
                            count++;
                        }
                        else
                        {
                            esiLocation = false;
                        }
                    }
                }

                HashSet<string> set = new HashSet<string>(m_SelectedEsiLocations);
                m_SelectedEsiLocations = new string[set.Count];
                set.CopyTo(m_SelectedEsiLocations);
            }
            catch
            {
                throw;
            }

            return esiLocation;
        }
        private bool IsWRJob()
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
                        ActiveWorkRequest = Convert.ToString(rsValidate.Fields[1].Value);
                    }
                }

                if (!string.IsNullOrEmpty(m_strJobtype))
                {
                    if (m_strJobtype == "NON-WR" || !m_strJobtype.Contains("WR"))
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
