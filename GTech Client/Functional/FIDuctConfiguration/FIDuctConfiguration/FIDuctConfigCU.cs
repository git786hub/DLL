using System;
using System.Windows.Forms;
using Intergraph.GTechnology.API;
using Intergraph.GTechnology.Interfaces;
using System.Text.RegularExpressions;

namespace FIDuctConfiguration
{
    public class FIDuctConfigCU : IGTFunctional
    {
        #region Fields

        /// <summary>
        /// The components collection of the feature being edited.
        /// </summary>
        private IGTComponents _components;

        /// <summary>
        /// The data context of the application.
        /// </summary>
        private IGTDataContext _context;

        /// <summary>
        /// The component name of the component that caused the functional interface to fire.
        /// </summary>
        private string _compName;

        /// <summary>
        /// The metadata-specified arguments associated with the interface.
        /// </summary>
        private GTArguments _arguments;

        /// <summary>
        /// The field name of the component that caused the functional interface to fire.
        /// </summary>
        private string _fieldName;

        /// <summary>
        /// The type of functional interface being executed.
        /// </summary>
        private GTFunctionalTypeConstants _functionalTypeConst;

        /// <summary>
        /// The original value of the attribute that cased the functional interface to fire.
        /// </summary>
        private IGTFieldValue _original;

        #endregion

        #region IGTFunctional Members

        public GTArguments Arguments
        {
            get
            {
                return this._arguments;

            }
            set
            {
                _arguments = value;
            }
        }

        public string ComponentName
        {
            get
            {
                return _compName;
            }
            set
            {
                _compName = value;
            }
        }

        public IGTComponents Components
        {
            get
            {
                return _components;
            }
            set
            {
                _components = value;
            }
        }

        public IGTDataContext DataContext
        {
            get
            {
                return _context;
            }
            set
            {
                _context = value;
            }
        }

        public void Delete()
        {
            // Do nothing here
        }

        public void Execute()
        {
            GTClassFactory.Create<IGTApplication>().SetStatusBarText(GTStatusPanelConstants.gtaspcMessage, "Creating Duct COnfiguration");
            String valueEntered = "";
            String sFNO = "";
            int FID = 0;
            int iHorz = 0;
            int iVert = 0;
            IGTKeyObject Feature = null;
            short FNO = 0;
            IGTApplication oApp = GTClassFactory.Create<IGTApplication>();

            try
            {
               

                //objGlobal.gServices = GTClassFactory.Create<IGTRelationshipService>();
                //objGlobal.gDataCont = DataContext;

                ADODB.Recordset netElementSet = _components.GetComponent(2401).Recordset;
                netElementSet.MoveFirst();
                valueEntered = netElementSet.Fields["CONFIG_C"].Value.ToString();
                sFNO = netElementSet.Fields["G3E_FNO"].Value.ToString();
                FID = Convert.ToInt32(netElementSet.Fields["G3E_FID"].Value.ToString());
                FNO = Convert.ToInt16(sFNO);
                Feature = DataContext.OpenFeature(FNO, FID);


                ////Validate if the Duct Configuration is defined already
                //csGlobals.gServices.ActiveFeature = Feature;

                //IGTKeyObjects oCollection = GTClassFactory.Create<IGTKeyObjects>();

                //oCollection = csGlobals.gServices.GetRelatedFeatures(csConstant.iContains);

                //if (oCollection != null)
                //{
                //    GTClassFactory.Create<IGTApplication>().SetStatusBarText(GTStatusPanelConstants.gtaspcMessage, "");
                //    MessageBox.Show( " Duct Configuration is alredy defined", "Create Duct Configuration", MessageBoxButtons.OK, MessageBoxIcon.Error);
                //    netElementSet.Fields["CU_C"].Value = _original.FieldValue.ToString();
                //    return;
                //}
                csGlobals objGlobal = new csGlobals(DataContext, Feature);               

                if (sFNO != "2400")
                {
                    GTClassFactory.Create<IGTApplication>().SetStatusBarText(GTStatusPanelConstants.gtaspcMessage, "");
                    return;
                }

                if (valueEntered == "" )
                {
                    GTClassFactory.Create<IGTApplication>().SetStatusBarText(GTStatusPanelConstants.gtaspcMessage, "");
                    return;
                }

                else
                {
                    try
                    {
                        Char Delimiter = 'X';
                        String[] sValues = valueEntered.Split(Delimiter);
                        int iVal = 1;
                        foreach (var sValue in sValues)
                        {
                            if (iVal == 1)
                            {
                                string sCorrectValue = Regex.Match(sValue, @"\d").Value;
                                iHorz = Convert.ToInt32(sCorrectValue);
                            }
                            else
                            {
                                string sCorrectValue = Regex.Match(sValue, @"\d").Value;
                                iVert = Convert.ToInt32(sCorrectValue);
                            }
                            iVal = iVal + 1;
                        }
                    }
                    catch(Exception exValue)
                    {
                        MessageBox.Show(exValue.Message + " Value is incorrect, should be Horizontal Number X Vertical Number. Example: 2X2", "Create Duct Configuration", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }

                    oApp.BeginWaitCursor();
                    objGlobal.DeleteExistingDucts(FNO, FID);
                    objGlobal.CreateDucConfiguration(iHorz, iVert);                 
                 
                    oApp.EndWaitCursor();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error in Duct Creation " + ex.Message);
            }
            finally
            {
                oApp.EndWaitCursor();
            }
            GTClassFactory.Create<IGTApplication>().SetStatusBarText(GTStatusPanelConstants.gtaspcMessage, "");
        }

        public string FieldName
        {
            get
            {
                return _fieldName;
            }
            set
            {
                _fieldName = value;
            }
        }

        public IGTFieldValue FieldValueBeforeChange
        {
            get
            {
                return _original;
            }
            set
            {
                _original = value;
            }
        }

        public GTFunctionalTypeConstants Type
        {
            get
            {
                return _functionalTypeConst;
            }
            set
            {
                _functionalTypeConst = value;
            }
        }

        public void Validate(out string[] ErrorPriorityArray, out string[] ErrorMessageArray)
        {
             ErrorPriorityArray = null;
             ErrorMessageArray = null;
        }

        #endregion
    }
}
