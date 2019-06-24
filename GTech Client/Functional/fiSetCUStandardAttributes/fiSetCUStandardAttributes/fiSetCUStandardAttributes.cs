using System;
using Intergraph.GTechnology.API;
using Intergraph.GTechnology.Interfaces;
using System.Windows.Forms;
using ADODB;

//----------------------------------------------------------------------------+
//  Class: fiSetCUStandardAttributes
//  Description: This FI is responsible for the population of CU default attributes and the 
//               standard attributed in the Unit component once the CU code is set either through
//               CU selection foreign key query or populated manually by the user      
//----------------------------------------------------------------------------+
//  $Author:: Shubham Agarwal                                                       $
//  $Date:: 20/12/17                                                                $
//  $Revision:: 1                                                                   $
//----------------------------------------------------------------------------+


namespace GTechnology.Oncor.CustomAPI
{
    public class fiSetCUStandardAttributes : IGTFunctional
    {
        private IGTDataContext m_oDataContext = null;
        private string m_sComponentName = null;
        private IGTComponents m_oComponents = null;
        private GTArguments m_oArguments = null;
        private const string M_SIWGENERALPARAMATERTABLE = "SYS_GENERALPARAMETER";
        private string m_sFieldName = null;
        private IGTFieldValue m_oFieldValue = null;
        private GTFunctionalTypeConstants m_oGTFunctionalType;
        private const short m_iAncCompUnitCNO = 22;
        IGTApplication m_oApp = GTClassFactory.Create<IGTApplication>();

        #region FI Properties and Methods
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

        private IGTComponent _activeComponent;
        public virtual IGTComponent ActiveComponent
        {
            get { return _activeComponent; }
            set { _activeComponent = value; }
        }

        public IGTComponent GetActiveComponent()
        {
                foreach (IGTComponent component in m_oComponents)
                {
                    if (component.Name == ComponentName)
                    {
                        ActiveComponent = component;
                        break;
                    }
                }
            
            return ActiveComponent;
        }

        public IGTKeyObject ActiveKeyObject
        {
            get
            {
                IGTApplication _gtApp = (IGTApplication)GTClassFactory.Create<IGTApplication>();

                IGTComponent comp = GetActiveComponent();

                if (comp != null)
                {
                    short FNO = short.Parse(comp.Recordset.Fields["G3E_FNO"].Value.ToString());
                    int FID = int.Parse(comp.Recordset.Fields["G3E_FID"].Value.ToString());
                    return _gtApp.DataContext.OpenFeature(FNO, FID);
                }
                return null;
            }
        }

        private bool IsRepeatingComponent(int p_CNO)
        {
            bool bReturn = false;
            ADODB.Recordset rs = m_oApp.DataContext.MetadataRecordset("G3E_FEATURECOMPS_OPTABLE","g3e_cno = " + p_CNO);
          //  rs.Filter = "g3e_cno = " + p_CNO;

            if (rs != null)
            {
                if (rs.RecordCount > 0)
                {
                    rs.MoveFirst();
                    bReturn = Convert.ToInt32(rs.Fields["G3E_REPEATING"].Value) == 1;
                }
            }
            return bReturn;
        }

        public void Delete()
        {
            IGTComponent gTActiveComponent = GetActiveComponent();
            IGTComponent gTUnitComponent = null;
            Recordset rsActiveComponent = null;
            Recordset rsUnitComponent = null;
            int UnitCid = 0;
            short UnitCno = 0;

            try
            {
                if ((gTActiveComponent != null) && (gTActiveComponent.CNO == 21))
                {
                    rsActiveComponent = gTActiveComponent.Recordset;
                    if (rsActiveComponent != null && rsActiveComponent.RecordCount > 0)
                    {
                        if (rsActiveComponent.Fields["UNIT_CNO"].Value != null &&
                            !string.IsNullOrEmpty(rsActiveComponent.Fields["UNIT_CNO"].Value.ToString()))
                        {
                            UnitCno = Convert.ToInt16(rsActiveComponent.Fields["UNIT_CNO"].Value);
                        }
                        if (rsActiveComponent.Fields["UNIT_CID"].Value != null &&
                            !string.IsNullOrEmpty(rsActiveComponent.Fields["UNIT_CID"].Value.ToString()))
                        {
                            UnitCid = Convert.ToInt32(rsActiveComponent.Fields["UNIT_CID"].Value);
                        }
                    }
                }

                if (UnitCno > 0 && UnitCid > 0 && IsRepeatingComponent(UnitCno))
                {                    
                    gTUnitComponent = ActiveKeyObject.Components.GetComponent(UnitCno);

                    if (gTUnitComponent != null)
                    {
                        rsUnitComponent = gTUnitComponent.Recordset;
                        if (rsUnitComponent != null && rsUnitComponent.RecordCount > 0)
                        {
                            rsUnitComponent.MoveFirst();
                            while (!rsUnitComponent.EOF)
                            {
                                if (UnitCid == Convert.ToInt32(rsUnitComponent.Fields["G3E_CID"].Value))
                                {
                                    rsUnitComponent.Delete();
                                    break;
                                }
                                rsUnitComponent.MoveNext();
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error during execution of Set CUStandard Attributes." + Environment.NewLine + ex.Message,
                "G/Techonology", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }       
 
        public void Execute()
        {
            CommonSetCUStandardAttributes objCuCommonCode = new CommonSetCUStandardAttributes(Components,ComponentName);
            objCuCommonCode.SetCUAttributes();
            objCuCommonCode.SetStandardAttributes();
        }

        public void Validate(out string[] ErrorPriorityArray, out string[] ErrorMessageArray)
        {
            ErrorPriorityArray = null;
            ErrorMessageArray = null;
        }
        #endregion     
    }
}
