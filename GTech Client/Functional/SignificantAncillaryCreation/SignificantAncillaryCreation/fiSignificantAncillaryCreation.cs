using System;
using Intergraph.GTechnology.Interfaces;
using Intergraph.GTechnology.API;
using System.Collections.Generic;

namespace GTechnology.Oncor.CustomAPI
{
    public class fiSignificantAncillaryCreation : IGTFunctional
    {
        private IGTDataContext m_oDataContext = null;
        private string m_sComponentName = null;
        private IGTComponents m_oComponents = null;
        private GTArguments m_oArguments = null;
        private string m_sFieldName = null;
        private IGTFieldValue m_oFieldValue = null;
        private GTFunctionalTypeConstants m_oGTFunctionalType;

        private const short m_iAncCompUnitCNO = 22;
        private int m_iTriggeringANO = 0;
        public GTArguments Arguments
        {
            get
            {
                return m_oArguments;
            }

            set
            {
                m_oArguments = value;
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
                return m_oComponents;
            }

            set
            {
                m_oComponents = value;
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
        public IGTFieldValue FieldValueBeforeChange
        {
            get
            {
                return m_oFieldValue;
            }

            set
            {
                m_oFieldValue = value;
            }
        }

        public GTFunctionalTypeConstants Type
        {
            get
            {
                return m_oGTFunctionalType;
            }

            set
            {
                m_oGTFunctionalType = value;
            }
        }

        public void Delete()
        {
            m_oApp = GTClassFactory.Create<IGTApplication>();
            short CNO = 0;
            int CID = 0;
            int FNO = 0;
            int FID = 0;

            m_iTriggeringANO = GetTriggeringANO();

            CID = Convert.ToInt32(Components[ComponentName].Recordset.Fields["G3E_CID"].Value);
            CNO = Convert.ToInt16(Components[ComponentName].Recordset.Fields["G3E_CNO"].Value);

            FNO = Convert.ToInt16(Components[ComponentName].Recordset.Fields["G3E_FNO"].Value);
            FID = Convert.ToInt32(Components[ComponentName].Recordset.Fields["G3E_FID"].Value);

            if (IsMatchingAncillaryExists(CID, CNO, m_iTriggeringANO)) //If matching Ancillary component instance is found, delete it
            {
                Components.GetComponent(22).Recordset.Fields["ACU_ANO"].Value = null;
                Components.GetComponent(22).Recordset.Delete();
               // Components.GetComponent(22).Recordset.Update();
            }
        }

        /// <summary>
        /// This method updates the Unit_CID and Unit_CNO along with CU_C, the rest of the standard and
        /// default attributes will be populated by FI Set CU Standard Attributes 
        /// </summary>
        /// <param name="rsTarget">Target recordset to update</param>
        /// <param name="rsSource">Source recordset used to update </param>
        private void UpdateAncillaryRecord(ADODB.Recordset rsTarget, ADODB.Recordset rsSource)
        {
            rsTarget.Fields["UNIT_CID"].Value = rsSource.Fields["G3E_CID"].Value;
            rsTarget.Fields["UNIT_CNO"].Value = rsSource.Fields["G3E_CNO"].Value;            
            rsTarget.Fields["CU_C"].Value = rsSource.Fields[m_sFieldName].Value;

            CommonSetCUStandardAttributes oCUAttributes = new CommonSetCUStandardAttributes(Components, "COMP_UNIT_ANCIL_N");
            oCUAttributes.SetCUAttributes();
            oCUAttributes.SetStandardAttributes();
        }    
        /// <summary>
        /// Method to get the triggering attribute ANO
        /// </summary>
        /// <returns></returns>
        private int GetTriggeringANO()
        {
            int iReturnANO = 0;
            ADODB.Recordset rs = m_oApp.DataContext.MetadataRecordset("G3E_ATTRIBUTEINFO_OPTABLE", "G3E_NAME = '" + m_sComponentName + "' AND G3E_FIELD = '" + m_sFieldName + "'");
           // rs.Filter = "G3E_NAME = '" + m_sComponentName + "' AND G3E_FIELD = '" + m_sFieldName + "'";
            if (rs != null)
            {
                if (rs.RecordCount > 0)
                {
                    rs.MoveFirst();
                    iReturnANO = Convert.ToInt32(rs.Fields["G3E_ANO"].Value);
                }
            }
            return iReturnANO;
        }
        private void CreateAncillaryComponentInstance(ADODB.Recordset rsSource, int FNO, int FID)
        {
            if (!string.IsNullOrEmpty(Convert.ToString(rsSource.Fields[m_sFieldName].Value)))
            {
                Components.GetComponent(22).Recordset.AddNew(System.Type.Missing, System.Type.Missing);
                Components.GetComponent(22).Recordset.Fields["G3E_CNO"].Value = 22;
                Components.GetComponent(22).Recordset.Fields["G3E_FID"].Value = FID;
                Components.GetComponent(22).Recordset.Fields["G3E_FNO"].Value = FNO;
                Components.GetComponent(22).Recordset.Fields["ACU_ANO"].Value = m_iTriggeringANO;
                Components.GetComponent(22).Recordset.Update(System.Type.Missing, System.Type.Missing);

                UpdateAncillaryRecord(Components.GetComponent(22).Recordset, rsSource);
            }
        }
        /// <summary>
        /// Check to see if Matching Ancillary found, this will set the Ancillary component recordset
        /// to the matching instance and return true
        /// </summary>
        /// <param name="CID"></param>
        /// <param name="CNO"></param>
        /// <returns></returns>
        private bool IsMatchingAncillaryExists(int CID, short CNO, int ANO)
        {
            bool bReturn = false;
            if (Components.GetComponent(22).Recordset != null)
            {
                if (Components.GetComponent(22).Recordset.RecordCount > 0)
                {
                    Components.GetComponent(22).Recordset.MoveFirst();                    

                    while (Components.GetComponent(22).Recordset.EOF == false)
                    {
                        if ((!Convert.IsDBNull(Components.GetComponent(22).Recordset.Fields["UNIT_CNO"].Value)) && (!Convert.IsDBNull(Components.GetComponent(22).Recordset.Fields["UNIT_CID"].Value)) && (!Convert.IsDBNull(Components.GetComponent(22).Recordset.Fields["ACU_ANO"].Value)))
                        {
                            if (Convert.ToInt16(Components.GetComponent(22).Recordset.Fields["UNIT_CNO"].Value) == CNO && Convert.ToInt16(Components.GetComponent(22).Recordset.Fields["UNIT_CID"].Value) == CID && Convert.ToInt32(Components.GetComponent(22).Recordset.Fields["ACU_ANO"].Value) == ANO)
                            {
                                bReturn = true;
                                break;
                            }
                        }
                        Components.GetComponent(22).Recordset.MoveNext();
                    }
                }
            }           
            return bReturn;
        }
        IGTApplication m_oApp;
        public void Execute()
        {
            if (Type == GTFunctionalTypeConstants.gtftcSetValue) // This is configured for the Significant CU
            {
                m_oApp = GTClassFactory.Create<IGTApplication>();
                short CNO = 0;
                int CID = 0;
                int FNO = 0;
                int FID = 0;

                m_iTriggeringANO = GetTriggeringANO();

                CID = Convert.ToInt32(Components[ComponentName].Recordset.Fields["G3E_CID"].Value);
                CNO = Convert.ToInt16(Components[ComponentName].Recordset.Fields["G3E_CNO"].Value);

                FNO = Convert.ToInt16(Components[ComponentName].Recordset.Fields["G3E_FNO"].Value);
                FID = Convert.ToInt32(Components[ComponentName].Recordset.Fields["G3E_FID"].Value);


                if (!IsMatchingAncillaryExists(CID, CNO, m_iTriggeringANO)) //If there is no matching Ancillary component instance is found, create a new one and then update
                {
                    CreateAncillaryComponentInstance(Components[ComponentName].Recordset, FNO, FID);
                }
                else //Found matching Ancillary, just update the instance
                {
                    UpdateAncillaryRecord(Components.GetComponent(22).Recordset, Components[ComponentName].Recordset);
                }
            }
            else
            { 
                List<AncillaryCU> DuplicateCID = new List<AncillaryCU>();
                List<AncillaryCU> ListOFCID = new List<AncillaryCU>();

                int CID = Convert.ToInt32(Components[ComponentName].Recordset.Fields["G3E_CID"].Value);
                int CNO = Convert.ToInt16(Components[ComponentName].Recordset.Fields["G3E_CNO"].Value);

                if (Components.GetComponent(22).Recordset != null)
                {
                    if (Components.GetComponent(22).Recordset.RecordCount > 0)
                    {
                        Components.GetComponent(22).Recordset.MoveFirst();

                        while (Components.GetComponent(22).Recordset.EOF == false)
                        {
                            if ((!Convert.IsDBNull(Components.GetComponent(22).Recordset.Fields["UNIT_CNO"].Value)) && (!Convert.IsDBNull(Components.GetComponent(22).Recordset.Fields["UNIT_CID"].Value)))
                            {
                                AncillaryCU obj = new AncillaryCU();

                                obj.UNIT_CID = Convert.ToInt32(Components.GetComponent(22).Recordset.Fields["UNIT_CID"].Value);
                                obj.UNIT_CNO = Convert.ToInt32(Components.GetComponent(22).Recordset.Fields["UNIT_CNO"].Value);

                                if (Components.GetComponent(22).Recordset.Fields["ACU_ANO"].Value.GetType() != typeof(DBNull))
                                {
                                    obj.ACU_ANO = Convert.ToInt32(Components.GetComponent(22).Recordset.Fields["ACU_ANO"].Value);
                                }
                                //else
                                //{
                                //    obj.CID = Convert.ToInt32(Components.GetComponent(22).Recordset.Fields["G3E_CID"].Value);
                                //    DuplicateCID.Add(obj);
                                //}


                                if (ListOFCID.Contains(obj))
                                {
                                    AncillaryCU obj1 = new AncillaryCU();
                                    obj1.CID = Convert.ToInt32(Components.GetComponent(22).Recordset.Fields["G3E_CID"].Value);
                                    DuplicateCID.Add(obj1);
                                }
                                ListOFCID.Add(obj);
                            }
                            Components.GetComponent(22).Recordset.MoveNext();
                        }
                    }
                }

                if (DuplicateCID.Count > 0)
                {
                    for (int i = 0; i < DuplicateCID.Count; i++)
                    {
                        DeleteDuplicteAncillaryCU(DuplicateCID[i].CID);
                    }

                }              
            }
        }

        /// <summary>
        /// Method to delete duplicated Ancillary Record that are created for the Significant Ancillary
        /// </summary>
        /// <param name="p_CID"></param>

        private void DeleteDuplicteAncillaryCU(int p_CID)
        {
            Components.GetComponent(22).Recordset.MoveFirst();

            while (Components.GetComponent(22).Recordset.EOF == false)
            {
                if (p_CID == Convert.ToInt32(Components.GetComponent(22).Recordset.Fields["G3E_CID"].Value))
                {
                    Components.GetComponent(22).Recordset.Fields["ACU_ANO"].Value = null;
                    Components.GetComponent(22).Recordset.Delete();
                    Components.GetComponent(22).Recordset.Update();
                    break;
                }
                Components.GetComponent(22).Recordset.MoveNext();
            }
        }
        public void Validate(out string[] ErrorPriorityArray, out string[] ErrorMessageArray)
        {
            ErrorPriorityArray = null;
            ErrorMessageArray = null;
            //Nothing to validate
        }
    }

    /// <summary>
    /// Class to hold data related to Ancillary CU that are created as part of Significant Ancillary
    /// </summary>
    public class AncillaryCU
    {
        public int ACU_ANO { get; set; }
        public int UNIT_CID { get; set; }
        public int UNIT_CNO { get; set; }
        public int CID { get; set; }

        public override bool Equals(object obj)
        {
            if (this.ACU_ANO == ((AncillaryCU)obj).ACU_ANO && this.UNIT_CID == ((AncillaryCU)obj).UNIT_CID && this.UNIT_CNO == ((AncillaryCU)obj).UNIT_CNO)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}
