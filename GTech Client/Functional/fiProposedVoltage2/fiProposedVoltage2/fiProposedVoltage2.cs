//----------------------------------------------------------------------------+
//        Class: fiProposedVoltage2
//  Description: This interface determines the appropriate volatage value based on the feature state.
//----------------------------------------------------------------------------+
//     $Author:: hkonda                                   $
//       $Date:: 13/11/17                                 $
//   $Revision:: 1                                        $
//----------------------------------------------------------------------------+
//    $History:: fiSetFeatureState.cs                     $
// 
// *****************  Version 1  *****************
// User: skamaraj     Date: 16/05/19   Time: 18:00  Desc : Created
//----------------------------------------------------------------------------+
using System;
using System.Windows.Forms;
using Intergraph.GTechnology.API;
using Intergraph.GTechnology.Interfaces;

namespace GTechnology.Oncor.CustomAPI
{
    public class fiProposedVoltage2 : IGTFunctional
    {
        #region Private Variables

        private GTArguments m_arguments;
        private string m_componentName;
        private IGTComponents m_components;
        private IGTDataContext m_dataContext;
        private string m_fieldName;
        private IGTFieldValue m_fieldValueBeforeChange;
        private GTFunctionalTypeConstants m_type;        

        #endregion

        #region Properties
        public GTArguments Arguments
        {
            get
            {
                return m_arguments;
            }

            set
            {
                m_arguments = value;
            }
        }

        public string ComponentName
        {
            get
            {
                return m_componentName;
            }

            set
            {
                m_componentName = value;
            }
        }

        public IGTComponents Components
        {
            get
            {
                return m_components;
            }

            set
            {
                m_components = value;
            }
        }

        public IGTDataContext DataContext
        {
            get
            {
                return m_dataContext;
            }

            set
            {
                m_dataContext = value;
            }
        }

        public string FieldName
        {
            get
            {
                return m_fieldName;
            }

            set
            {
                m_fieldName = value;
            }
        }

        public IGTFieldValue FieldValueBeforeChange
        {
            get
            {
                return m_fieldValueBeforeChange;
            }

            set
            {
                m_fieldValueBeforeChange = value;
            }
        }

        public GTFunctionalTypeConstants Type
        {
            get
            {
                return m_type;
            }

            set
            {
                m_type = value;
            }
        }

        #endregion
        public void Delete()
        {
           
        }

        public void Execute()
        {
            string strSecVoltage = "";
            string strFeatureState = "";
            IGTComponent gTCommComponent = null;
            IGTComponent gTConnComponent = null;
            try
            {
                if (Components[ComponentName] != null && Components[ComponentName].Recordset != null &&
                    Components[ComponentName].Recordset.RecordCount > 0)
                {
                    strSecVoltage = Convert.ToString(Components[ComponentName].Recordset.Fields[FieldName].Value);
                    gTCommComponent = Components["COMMON_N"];
                    if (!string.IsNullOrEmpty(strSecVoltage) && Convert.ToString(Components[ComponentName].Recordset.Fields[FieldName].OriginalValue) != strSecVoltage)
                    {                        
                        if (gTCommComponent != null && gTCommComponent.Recordset != null && gTCommComponent.Recordset.RecordCount > 0)
                        {
                            gTCommComponent.Recordset.MoveFirst();
                            strFeatureState = Convert.ToString(gTCommComponent.Recordset.Fields["FEATURE_STATE_C"].Value);
                            if (!string.IsNullOrEmpty(strFeatureState))
                            {
                                gTConnComponent = Components["CONNECTIVITY_N"];

                                if (gTConnComponent != null && gTConnComponent.Recordset != null && gTConnComponent.Recordset.RecordCount > 0)
                                {
                                    gTConnComponent.Recordset.MoveFirst();

                                    if (strFeatureState == "PPI" || strFeatureState == "ABI")
                                    {
                                        gTConnComponent.Recordset.Fields["PP_VOLT_2_Q"].Value = strSecVoltage;
                                        gTConnComponent.Recordset.Update();
                                    }
                                    else
                                    {
                                        gTConnComponent.Recordset.Fields["VOLT_2_Q"].Value = strSecVoltage;
                                        gTConnComponent.Recordset.Update();
                                    }
                                }
                            }
                        }
                    }
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show("Error during ProposedVoltage2 FI execution. " + ex.Message, "G/Technology");
            }
        }

        public void Validate(out string[] ErrorPriorityArray, out string[] ErrorMessageArray)
        {
            ErrorMessageArray = null;
            ErrorPriorityArray = null;
        }
    }
}
