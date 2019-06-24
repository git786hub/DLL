using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Intergraph.GTechnology.Interfaces;
using Intergraph.GTechnology.API;
using System.Windows.Forms;
using Microsoft.CSharp;
using ADODB;


namespace GTechnology.Oncor.CustomAPI
{
    public class ccFieldActivity : IGTCustomCommandModeless
    {
        private IGTTransactionManager transactionManager = null;
        private static IGTApplication gtApp = GTClassFactory.Create<IGTApplication>();
        private IGTDataContext dataContext = gtApp.DataContext;
        private IGTCustomCommandHelper customCommandHelper = null;
        private IGTDDCKeyObjects selectedFeatures;
        const short g_Service_Line_FNO = 54;
        const short g_Secondary_Box_FNO = 113;
        //same fno as area light, but will need to look at type
        const short g_Gaurd_Light_FNO = 61;
        const short g_Gaurd_Light_CNO = 6101;

        public bool CanTerminate
        {
            get { return true; }
        }

        public IGTTransactionManager TransactionManager
        {
            set { transactionManager = value; }
        } 

        public void Activate(IGTCustomCommandHelper CustomCommandHelper)
        {
            try
            {
                customCommandHelper = CustomCommandHelper;
                if (transactionManager != null)
                {
                    transactionManager.Begin("Blanket Unitization");
                    selectedFeatures = gtApp.SelectedObjects.GetObjects();
                    bool oneFeatureType = true;
                    short fnoChecker;
                    string errorMessage = "";
                    if (selectedFeatures.Count > 1)
                    {
                        fnoChecker = selectedFeatures[0].FNO;
                        foreach (IGTDDCKeyObject feature in selectedFeatures)
                        {
                            if (fnoChecker != feature.FNO)
                            {
                                oneFeatureType = false;
                                errorMessage = "Multiple feature types selected, please retry command after selecting features of the same type.";
                            }
                        }
                    }

                    if (oneFeatureType)
                    {
                        switch (selectedFeatures[0].FNO)
                        {
                            case g_Service_Line_FNO:
                            case g_Secondary_Box_FNO:
                                featureHandler();
                                break;
                            case g_Gaurd_Light_FNO:
                                guardLightHandler();
                                break;
                            default:
                                MessageBox.Show("Invalid feature selected, please select a service line, secondary box, or guard light.");
                                break;
                        }
                        customCommandHelper.Complete();
                    }else
                    {
                        MessageBox.Show(errorMessage, "GTechnology");
                        customCommandHelper.Complete();
                    }
                        
                }
            }catch(Exception e)
            {
                MessageBox.Show("Error in ccFieldActivity.Activate " + e.Message + ".");
                transactionManager.Rollback();
                customCommandHelper.Complete();
            }
        }

        public void Pause()
        {
        }

        public void Resume()
        {
        }

        public void Terminate()
        {
            transactionManager.Commit();
            transactionManager = null;
            dataContext = null;
            customCommandHelper = null;
        }

        public void featureHandler()
        {
            if (selectedFeatures[0].FNO == g_Secondary_Box_FNO)
            {
                foreach (IGTDDCKeyObject feature in selectedFeatures)
                {
                    IGTKeyObject keyObject = dataContext.OpenFeature(feature.FNO, feature.FID);
                    IGTComponent cuAttributes = keyObject.Components.GetComponent(21);
                    cuAttributes.Recordset.Fields["ACTIVITY_C"].Value = "UR";
                }
            }
            else
            {
                foreach (IGTDDCKeyObject feature in selectedFeatures)
                {
                    replaceOrRemoveDialog dialog = new replaceOrRemoveDialog(feature.FNO);
                    dialog.ShowDialog();
                    IGTKeyObject keyObject = dataContext.OpenFeature(feature.FNO, feature.FID);
                    IGTComponent cuAttributes = keyObject.Components.GetComponent(21);
                    cuAttributes.Recordset.Fields["ACTIVITY_C"].Value = dialog.activityCode;
                    dialog.Close();
                }
            }
        }

        public void guardLightHandler()
        {
            int FID = selectedFeatures[0].FID;
            short FNO = selectedFeatures[0].FNO;
            IGTKeyObject guardLight = dataContext.OpenFeature(FNO, FID);
            IGTComponent guardLightComponent = guardLight.Components.GetComponent(g_Gaurd_Light_CNO);
            if (guardLightComponent.Recordset.Fields["LAMP_USE_C"].Value.ToString() == "G")
            {
                replaceOrRemoveDialog dialog = new replaceOrRemoveDialog(FNO);
                dialog.ShowDialog();
                IGTComponent cuAttributes = guardLight.Components.GetComponent(21);
                cuAttributes.Recordset.Fields["ACTIVITY_C"].Value = dialog.activityCode;
                dialog.Close();
            }
            else
            {
                MessageBox.Show("Invalid feature selected, please select a service line, secondary box, or guard light.");
                customCommandHelper.Complete();
            }
        }
    }
}
