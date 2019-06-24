using System;
using System.Collections.Generic;
using System.Windows.Forms;
using Intergraph.GTechnology.API;
using ADODB;
using System.Data;


namespace GTechnology.Oncor.CustomAPI.Model
{
    public class SupplementalAgreementModel : ISupplementalAgreementModel
    {
        
        IGTDataContext m_gTDataContext = null;
        IGTDDCKeyObjects m_gTDDCKeyObjects = null;
        IGTKeyObjects m_SelectedKeyObjects = GTClassFactory.Create<IGTKeyObjects>();
        string[] m_SelectedEsiLocations;
        int m_descriptionId = 0;
        int designAreaFid = 0;
        DataLayer dataLayer;

        public SupplementalAgreementModel(IGTDataContext gTDataContext, IGTDDCKeyObjects gTDDCKeyObjects)
        {
            m_gTDataContext = gTDataContext;
            m_gTDDCKeyObjects = gTDDCKeyObjects;
            dataLayer = new DataLayer(gTDataContext, gTDDCKeyObjects);
        }

        string m_NotifyModelMess;
        public string NotifyModelMess
        {
            get
            {
                return m_NotifyModelMess;
            }
            set
            {
                m_NotifyModelMess = value;
            }
        }

        string m_ActiveWorkRequest;
        public string ActiveWorkRequest { get { return m_ActiveWorkRequest; } set { m_ActiveWorkRequest = value; } }

        string m_Customer;
        public string Customer { get { return m_Customer; } set { m_Customer = value; } }

        DateTime? m_AgreementDate = DateTime.Today;

        DataTable m_DataTable;       
        public DateTime? AgreementDate { get { return m_AgreementDate; } set { m_AgreementDate = value; } }
        

        int m_Additions = 0;
        public int Additions
        {
            get { return m_Additions; }
            set { m_Additions = value; }
        }

        int m_Removal = 0;
        public int Removal
        {
            get { return m_Removal; }
            set { m_Removal = value; }
        }

        public DataTable GridDataTable
        {
            get
            {
                m_DataTable = dataLayer.GetData();
                Additions = dataLayer.Additions;
                Removal = dataLayer.Removal;
                return m_DataTable;
            }
            set { m_DataTable = value; }
        }

        IGTKeyObject m_DesignAreaKeyObject = null;
        public IGTKeyObject DesignAreaKeyObject {
            get
            {
                m_DesignAreaKeyObject = dataLayer.GetDesignAreaKeyObject();
                if (m_DesignAreaKeyObject != null)
                {
                    designAreaFid = m_DesignAreaKeyObject.FID;
                    dataLayer.designAreaFid = designAreaFid;
                }
                return m_DesignAreaKeyObject;
            } set { m_DesignAreaKeyObject = value; } }

        /// <summary>
        /// Verify the CC is valid or not.
        /// </summary>
        /// <returns></returns>
        public bool IsCommandValid()
        {
            try
            {
                if (IsWRJob())
                {
                    if(IsSelectSetStreetLight())
                    {
                        if(IsSelectSetHasESILocation())
                        {
                            if(IsSelectSetHaSameCustomer())
                            {
                                return true;
                            }
                            else
                            {
                                NotifyModelMess = "All selected Street Lights must be associated with the same customer .";
                                return false;
                            }
                        }
                        else
                        {                           
                            NotifyModelMess = "All selected Street Lights must have an ESI Location assigned to it .";
                            return false;
                        }
                    }
                    else
                    {                        
                        NotifyModelMess = "All selected features must be Street Lights.";
                        return false;
                    }
                }
                else
                {
                    MessageBox.Show("Active job must be a WR job.", "G/Technology", MessageBoxButtons.OK, MessageBoxIcon.Information,
                           MessageBoxDefaultButton.Button1);                    

                    return false;
                }
            }
            catch
            {
                throw;
            }

        }
        /// <summary>
        /// Verify MASTER AGREEMENT is there or not.
        /// </summary>
        /// <returns></returns>
        public bool IsMSLAExist()
        {
            string sql = "";
            Recordset rsValidate = null;

            try
            {
                sql = "select DESCRIPTION,MSLA_DATE from STLT_DESC_VL where DESCRIPTION_ID=?";
                rsValidate = m_gTDataContext.OpenRecordset(sql, ADODB.CursorTypeEnum.adOpenDynamic, ADODB.LockTypeEnum.adLockReadOnly,
                    (int)ADODB.CommandTypeEnum.adCmdText, m_descriptionId);

                if (rsValidate.RecordCount > 0)
                {
                    rsValidate.MoveFirst();
                    if (!rsValidate.EOF && !rsValidate.BOF)
                    {
                        if (!string.IsNullOrEmpty(Convert.ToString(rsValidate.Fields["MSLA_DATE"].Value)))
                        {
                            m_AgreementDate = Convert.ToDateTime(rsValidate.Fields["MSLA_DATE"].Value);
                            Customer = Convert.ToString(rsValidate.Fields["DESCRIPTION"].Value);
                        }
                        else
                        {
                            m_AgreementDate = null;
                            Customer = Convert.ToString(rsValidate.Fields["DESCRIPTION"].Value);
                            return false;
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

        /// <summary>
        /// Verify all selected streetlights have same customers.
        /// </summary>
        /// <returns></returns>
        public bool IsSelectSetHaSameCustomer()
        {
            string sql = "";
            Recordset rsValidate = null;
            string esiLocations = null;
           
            try
            {
                foreach(string esiL in m_SelectedEsiLocations)
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

                if(!string.IsNullOrEmpty(esiLocations))
                {
                    esiLocations = "(" + esiLocations + ")" ;
                    sql = "select distinct DESCRIPTION_ID from STLT_ACCOUNT where ESI_LOCATION in ?";

                    sql = string.Format("select distinct DESCRIPTION_ID from STLT_ACCOUNT where ESI_LOCATION in {0}", esiLocations);

                    rsValidate = m_gTDataContext.OpenRecordset(sql, ADODB.CursorTypeEnum.adOpenDynamic, ADODB.LockTypeEnum.adLockReadOnly,
                        (int)ADODB.CommandTypeEnum.adCmdText, null);

                    if (rsValidate.RecordCount > 0)
                    {
                        rsValidate.MoveFirst();
                        if (!rsValidate.EOF && !rsValidate.BOF)
                        {
                            m_descriptionId = Convert.ToInt32(rsValidate.Fields[0].Value);
                            if (rsValidate.RecordCount > 1)
                            {
                                return false;
                            }
                            else if(rsValidate.RecordCount == 1)
                            {
                                return true;
                            }                                                       
                        }
                    }
                    else if(rsValidate== null ||rsValidate.RecordCount == 0)
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

        /// <summary>
        /// Verify all selected street lights have ESI Location.
        /// </summary>
        /// <returns></returns>
        public bool IsSelectSetHasESILocation()
        {
            try
            {
                m_SelectedEsiLocations = new string[m_SelectedKeyObjects.Count];
                int count = 0;

                foreach (IGTKeyObject gtKey in m_SelectedKeyObjects)
                {
                    if(gtKey.Components["STREETLIGHT_N"] != null && gtKey.Components["STREETLIGHT_N"].Recordset != null && 
                        gtKey.Components["STREETLIGHT_N"].Recordset.RecordCount > 0)
                    {
                        m_SelectedEsiLocations[count] = Convert.ToString(gtKey.Components["STREETLIGHT_N"].Recordset.Fields["ACCOUNT_ID"].Value);
                        dataLayer.m_SelectedEsiLocations = m_SelectedEsiLocations;
                        if (string.IsNullOrEmpty(Convert.ToString(gtKey.Components["STREETLIGHT_N"].Recordset.Fields["ACCOUNT_ID"].Value)))
                        {
                            return false;
                        }

                        count++;
                    }
                    else
                    {
                        return false;
                    }
                }
            }
            catch
            {
                throw;
            }

            return true;
        }

        /// <summary>
        /// Verify Selected objects are streetlights or not.
        /// </summary>
        /// <returns></returns>
        public bool IsSelectSetStreetLight()
        {            
            IList<int> SelectedFeatureIDList = new List<int>();
            bool returnValue = true;
            try
            {
                if (m_gTDDCKeyObjects.Count > 0)
                {
                    foreach (IGTDDCKeyObject ddcKey in m_gTDDCKeyObjects)
                    {
                        if (!SelectedFeatureIDList.Contains(ddcKey.FID))
                        {
                            SelectedFeatureIDList.Add(ddcKey.FID);   
                            
                            if (ddcKey.FNO != 56)
                            {
                                returnValue = false;
                            }

                            m_SelectedKeyObjects.Add(m_gTDataContext.OpenFeature(56, ddcKey.FID));
                        }
                    }
                }
            }
            catch
            {
                throw;
            }
            dataLayer.m_SelectedKeyObjects = m_SelectedKeyObjects;
            return returnValue;
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
                        ActiveWorkRequest= Convert.ToString(rsValidate.Fields[1].Value);
                        dataLayer.ActiveWorkRequest = ActiveWorkRequest;
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
        /// <summary>
        /// Delete Existing Attachment.
        /// </summary>
        /// <param name="strFilename">Html file name</param>
        /// <returns></returns>
        public void DeleteExistingAttachment(string strFilename)
        {           
            try
            {
                dataLayer.DeleteExistingAttachment(strFilename);
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Get template location of the form.
        /// </summary>
        /// <param name="strParameterName"> param name in the SYS_GENERALPARAMETER table.</param>
        /// <returns></returns>
        public string GetFormTemplateLocation(string strParameterName)
        {
            
            try
            {
                return dataLayer.GetFormTemplateLocation(strParameterName);
            }
            catch
            {
                throw;
            }
         
        }

        /// <summary>
        /// Check the file name is already attached to the active job.
        /// </summary>
        /// <param name="strFilename">Html file name</param>
        /// <returns></returns>
        public bool IsExistingAttachment(string strFilename)
        {
           
            try
            {
                return dataLayer.IsExistingAttachment(strFilename);
            }
            catch
            {
                throw;
            }
           
        }
    }
}
