//----------------------------------------------------------------------------+
//        Class: fiCopyLengthAttributes
//  Description: This command provides the copy of the Graphic and Actual Length population from Common Component to Connectivity Component.
//----------------------------------------------------------------------------+
//     $Author:: pnlella                                   $
//       $Date:: 30/08/18                                 $
//   $Revision:: 1                                        $
//----------------------------------------------------------------------------+
//    $History:: fiCopyLengthAttributes.cs                     $
// 
// *****************  Version 1  *****************
// User: pnlella     Date: 30/08/18   Time: 18:00  Desc : Created as per the JIRA Requirement ONCORDEV-2041.
// *****************  Version 2  *****************
// User: pnlella     Date: 20/02/19   Time: 11:00  Desc : Removed Ceiling function in Conversion from Meters to Feet.
// *****************  Version 3  *****************
// User: Akhilesh    Date: 21/03/19   Time: 15:00  Desc : Included Rounding Function to 1 Decimal Point in Conversion from Meters to Feet as per the Requirement.
//----------------------------------------------------------------------------+

using System;
using Intergraph.GTechnology.API;
using Intergraph.GTechnology.Interfaces;
using System.Windows.Forms;
using ADODB;

namespace GTechnology.Oncor.CustomAPI
{
    public class fiCopyLengthAttributes : IGTFunctional
    {
        #region Private Variables
        private GTArguments m_arguments;
        private string m_componentName;
        private IGTComponents m_components;
        private IGTDataContext m_dataContext;
        private string m_fieldName;
        private IGTFieldValue m_fieldValueBeforeChange;
        private GTFunctionalTypeConstants m_type;
        private double m_ftGraphicLength = 0.0;
        private double m_ftGraphicLengthValueBeforeChange = 0.0;
        private bool m_InteractiveMode = false;
        #endregion

        #region Properities
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

        #region Functional Interface Members
        public void Delete()
        {

        }  
        
        public void Execute()
        {
            short commonCompCNO = 1;
            short CUCompCNO = 21;
            try
            {
                // If G/Tech is not running in interactive mode, then skip message boxes.
                GUIMode guiMode = new GUIMode();
                m_InteractiveMode = guiMode.InteractiveMode;

                IGTComponent commonComponent = m_components.GetComponent(commonCompCNO);

                if ((commonComponent != null) && (commonComponent.Recordset.RecordCount > 0))
                {
                    commonComponent.Recordset.MoveFirst();

                    if (commonComponent.Recordset.Fields["LENGTH_GRAPHIC_Q"].Value.GetType() != typeof(DBNull))
                    {
                        double graphicLengthMtr = Convert.ToDouble(commonComponent.Recordset.Fields["LENGTH_GRAPHIC_Q"].Value);

                        SetLengthAttributes(graphicLengthMtr, commonComponent);
                    }

                    if (commonComponent.Recordset.Fields["LENGTH_ACTUAL_Q"].Value.GetType() != typeof(DBNull))
                    {
                        CUAttributesLengthUpdate(commonComponent,CUCompCNO);
                    }
                }
            }
            catch (Exception exception)
            {
                if (m_InteractiveMode)
                {
                    MessageBox.Show("Error occured in CopyLengthAttributes Functional interface : " + exception.Message, "G/Technology");
                }                    
            }
        }
        public void Validate(out string[] ErrorPriorityArray, out string[] ErrorMessageArray)
        {
            ErrorPriorityArray = null;
            ErrorMessageArray = null;
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Method to set the Actual and Graphic length attribute of both Common and Connectivity/Ductivity Attributes Components.
        /// </summary>
        /// <param name="p_commonGraphicLengthMtr">Graphic Length in meters</param>
        /// <param name="p_commonComp">Common component</param>
        private void SetLengthAttributes(double p_commonGraphicLengthMtr, IGTComponent p_commonComp)
        {
            IGTComponent component = null;
            try
            {
                GetGraphicLengthConvertedMtrToFt(p_commonGraphicLengthMtr);

                GetConnectivityComp(ref component);

                if (m_type == GTFunctionalTypeConstants.gtftcSetValue)
                {
                    p_commonComp.Recordset.MoveFirst();

                    if (p_commonComp.Recordset.Fields["LENGTH_GRAPHIC_FT"].Value.GetType() != typeof(DBNull))
                    {
                        m_ftGraphicLengthValueBeforeChange = Convert.ToDouble(p_commonComp.Recordset.Fields["LENGTH_GRAPHIC_FT"].Value);
                    }

                    p_commonComp.Recordset.Fields["LENGTH_GRAPHIC_FT"].Value = m_ftGraphicLength;
                    p_commonComp.Recordset.Update();
                }

                SetActualLength(p_commonComp.Recordset,null);

                SetConnectivityLengthAttributes(component,p_commonComp.Recordset);
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        ///  Method to convert the Graphic length from meters to feet.
        /// </summary>
        /// <param name="p_graphicLengthMtr">Graphic Length in meters</param>
        private void GetGraphicLengthConvertedMtrToFt(double p_graphicLengthMtr)
        {
            try
            {
                m_ftGraphicLength = p_graphicLengthMtr * 3.280839895;
                m_ftGraphicLength = Math.Round(m_ftGraphicLength, 1);
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Method to get the Connectivity/Ductivity Attributes Components if present for feature.
        /// </summary>
        /// <param name="p_cmpConnectvity"></param>
        private void GetConnectivityComp(ref IGTComponent p_cmpConnectvity)
        {
            short connectivityCompCNO = 11;
            short ductivityCompCNO = 13;
            try
            {
                if (((m_components.GetComponent(connectivityCompCNO) != null) && (m_components.GetComponent(connectivityCompCNO).Recordset != null)) && (m_components.GetComponent(connectivityCompCNO).Recordset.RecordCount > 0))
                {
                    p_cmpConnectvity = m_components.GetComponent(connectivityCompCNO);
                }
                else if (((m_components.GetComponent(ductivityCompCNO) != null) && (m_components.GetComponent(ductivityCompCNO).Recordset != null)) && (m_components.GetComponent(ductivityCompCNO).Recordset.RecordCount > 0))
                {
                    p_cmpConnectvity = m_components.GetComponent(ductivityCompCNO);
                }
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Method to set the Actual length based on the FunctionalType Constant.
        /// </summary>
        /// <param name="p_compRecordset">Recordset</param>
        private void SetActualLength(Recordset p_compRecordset,Recordset p_commCmpRs)
        {
            try
            {
                p_compRecordset.MoveFirst();

                if (m_type == GTFunctionalTypeConstants.gtftcSetValue)
                {
                    if ((p_compRecordset.Fields["LENGTH_ACTUAL_Q"].Value.GetType() == typeof(DBNull)) || Convert.ToDouble(p_compRecordset.Fields["LENGTH_ACTUAL_Q"].Value) == m_ftGraphicLengthValueBeforeChange)
                    {
                        p_compRecordset.Fields["LENGTH_ACTUAL_Q"].Value = m_ftGraphicLength;
                        p_compRecordset.Update();
                    }
                }
                else if (m_type == GTFunctionalTypeConstants.gtftcUpdate)
                {
                   if (p_compRecordset.Fields["LENGTH_ACTUAL_Q"].Value.GetType() == typeof(DBNull) || (Convert.ToInt16(p_compRecordset.Fields["LENGTH_ACTUAL_Q"].Value) == 0))
                    {
                        p_compRecordset.Fields["LENGTH_ACTUAL_Q"].Value = m_ftGraphicLength;
                        p_compRecordset.Update();
                    }

                    if(m_componentName == "COMMON_N" && (Convert.ToInt32(p_compRecordset.Fields["G3E_CNO"].Value) == 11 || Convert.ToInt32(p_compRecordset.Fields["G3E_CNO"].Value) == 13))
                    {
                        p_commCmpRs.MoveFirst();
                        p_compRecordset.MoveFirst();

                        if (!p_commCmpRs.Fields["LENGTH_ACTUAL_Q"].Value.Equals(p_compRecordset.Fields["LENGTH_ACTUAL_Q"].Value))
                        {
                            p_compRecordset.Fields["LENGTH_ACTUAL_Q"].Value = p_commCmpRs.Fields["LENGTH_ACTUAL_Q"].Value;
                        }
                    }
                }
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Method to set the Actual and Graphic length attribute of Connectivity/Ductivity Attributes Components.
        /// </summary>
        /// <param name="p_cmpConnectvity"></param>
        private void SetConnectivityLengthAttributes(IGTComponent p_cmpConnectvity,Recordset p_commCmpRs)
        {
            try
            {
                if (p_cmpConnectvity != null)
                {
                    if (m_type == GTFunctionalTypeConstants.gtftcSetValue)
                    {
                        p_cmpConnectvity.Recordset.MoveFirst();
                        p_cmpConnectvity.Recordset.Fields["LENGTH_GRAPHIC_Q"].Value = m_ftGraphicLength;
                        p_cmpConnectvity.Recordset.Update();
                    }
                    SetActualLength(p_cmpConnectvity.Recordset, p_commCmpRs);
                }
            }
            catch
            {
                throw;
            }
        }

        private void CUAttributesLengthUpdate(IGTComponent p_commonComponent,short compCNO)
        {
            try
            {
                if(m_components.GetComponent(compCNO)!=null)
                {
                    if (m_components.GetComponent(compCNO).Recordset != null && m_components.GetComponent(compCNO).Recordset.RecordCount>0)
                    {
                        Recordset rsCU = m_components.GetComponent(compCNO).Recordset;

                        p_commonComponent.Recordset.MoveFirst();

                        rsCU.MoveFirst();

                        while(!rsCU.EOF)
                        {
                            rsCU.Fields["QTY_LENGTH_Q"].Value = p_commonComponent.Recordset.Fields["LENGTH_ACTUAL_Q"].Value;
                            rsCU.MoveNext();
                        }
                    }
                }
            }
            catch
            {
                throw;
            }
        }

        #endregion
    }
}
