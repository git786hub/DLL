using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ADODB;
using Intergraph.GTechnology.Interfaces;
using Intergraph.GTechnology.API;
using System.Windows.Forms;


namespace GTechnology.Oncor.CustomAPI
{
    public class ccAttachFeatureDoc : IGTCustomCommandModal
    {
        public IGTTransactionManager TransactionManager
        {
            set
            {
                csGlobals.gManageTransactions = value;
            }

        }
        
        public void Activate()
        {
            IGTSelectedObjects tmpSelectedObj = null;
            IGTDDCKeyObjects tmpDDCKeyObjs = null;

            try
            {
                //Set global variables.
                csGlobals.gApp = GTClassFactory.Create<IGTApplication>();
                csGlobals.gDatacont = csGlobals.gApp.DataContext;

                if (!csGlobals.gGetSysGenPrams()) return;

                // check feature for hyperlink component.
                tmpSelectedObj = csGlobals.gApp.SelectedObjects;
                tmpDDCKeyObjs = tmpSelectedObj.GetObjects();
                csGlobals.gCCFno = tmpDDCKeyObjs[0].FNO;
                csGlobals.gCCFid = tmpDDCKeyObjs[0].FID;
                csGlobals.gCCHyperLnkCno = FeatureHasHyperLnkComp(csGlobals.gCCFno);

                
                // if the selected feature has a hyperlink component continue.
                if (csGlobals.gCCHyperLnkCno !=  "")
                {
                    // Create the form.
                    csGlobals.gfrmAddFeatureHyperlink = new frmAddFeatHypLnk();
                    csGlobals.gfrmAddFeatureHyperlink.ShowDialog();
                }
                else
                {
                    tmpDDCKeyObjs = null;
                    tmpSelectedObj = null;
                  
                    return;
                }

            }
            catch(Exception ex)
            {
                MessageBox.Show("Error in Activate: " + ex.Message,
                               "Attach Feature Hyperlink Document",
                               MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
/// <summary>
/// This method checks to see if the seleced feature class contains a hyperlink component.
/// </summary>
/// <param name="p_fno"></param>
/// <returns>the name of the hyperlink component or an empty string.</returns>
        private string FeatureHasHyperLnkComp(int p_fno)
        {
            //int tmpRetVal = 0;
            string tmpRetVal = string.Empty;
            Recordset tmpRS = null;
            string tmpQry = string.Empty;

            try
            {

                tmpQry = "select g3e_cno,g3e_name from G3E_FEATURECOMPS_OPTABLE where g3e_fno = " + p_fno +
                         " and g3e_cno in (2,8130)";
                tmpRS = csGlobals.gDatacont.OpenRecordset(tmpQry, CursorTypeEnum.adOpenStatic,
                                        LockTypeEnum.adLockReadOnly, (int)CommandTypeEnum.adCmdText);
                if (tmpRS.BOF && tmpRS.EOF )
                {
                    csGlobals.gMessage = "You cannot attach a Hyperlink to the selected feature.";
                    MessageBox.Show(csGlobals.gMessage,
                                "Attach Feature Hyperlink Document",
                                MessageBoxButtons.OK, MessageBoxIcon.Error);
                    tmpRetVal = "";
                }
                else
                {
                    tmpRetVal = tmpRS.Fields[1].Value.ToString();
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show("Error in FeatureHasHyperLnkComp: " + ex.Message,
                                "Attach Feature Hyperlink Document",
                                MessageBoxButtons.OK, MessageBoxIcon.Error);
                tmpRetVal = "";
            }
            return tmpRetVal;
        }
    }
}
