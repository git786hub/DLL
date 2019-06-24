using System;
using System.Collections.Generic;
using System.Text;
using Intergraph.GTechnology.Interfaces;
using Intergraph.GTechnology.API;
using ADODB;
using System.Windows.Forms;

namespace GTechnology.Oncor.CustomAPI
{
    class ByPassRGI : CommonRGI, IGTRelationshipGeometry
    {
        #region ByPassRGI Members
        IGTApplication gtApplication = GTClassFactory.Create<IGTApplication>();

        const string sMsgBoxCaption = "ByPassRGI";

        /// <summary>
        /// Get component type
        /// </summary>
        /// <param name="CNO"></param>   
        /// <returns></returns>
        public int GetComponentType(short CNO)
        {
            try
            {
                int iReturn = 0;
                ADODB.Recordset oRsFeature = gtApplication.DataContext.MetadataRecordset("G3E_COMPONENTINFO_OPTABLE");
                oRsFeature.Filter = "G3E_CNO=" + CNO;
                iReturn = short.Parse(oRsFeature.Fields["G3E_TYPE"].Value.ToString());
                oRsFeature = null;
                return iReturn;
            }
            catch
            {
                throw;
            }
        }
        /// <summary>
        /// override After establish
        /// </summary>        
        public override void AfterEstablish()
        {
            IGTRelationshipService gtRelationshipService = GTClassFactory.Create<IGTRelationshipService>() as IGTRelationshipService;

            try
            {
                if ((ActiveFeature.FNO == RecloserOH_FNO) || (ActiveFeature.FNO == RecloserUG_FNO) || (ActiveFeature.FNO == VoltageRegulator_FNO) || (ActiveFeature.FNO == Autotransformer_FNO) || (ActiveFeature.FNO == TransformerUG_FNO))
                {                  
                    gtRelationshipService.DataContext = gtApplication.DataContext;
                    gtRelationshipService.ActiveFeature = this.ActiveFeature;

                    //Get related features
                    IGTKeyObjects relationshipcandidates = gtRelationshipService.GetRelatedFeatures(m_RNO);
                    if (relationshipcandidates != null)
                    {
                        for (int i = 0; i < relationshipcandidates.Count; i++)
                        {
                            // check if opposite nodes are connected otherwise .. change the node of the related feature
                            if ((relationshipcandidates.Count == 2) || (GetComponentType(relationshipcandidates[i].CNO).ToString() == "PolylineGeometry") && (GetComponentType(ActiveFeature.CNO) != GetComponentType(relationshipcandidates[i].CNO)))
                            {
                                if (Convert.ToString(relationshipcandidates[i].Components.GetComponent(ConnectivityG3eCno).Recordset.Fields["NODE_2_ID"].Value) != Convert.ToString(relationshipcandidates[i + 1].Components.GetComponent(ConnectivityG3eCno).Recordset.Fields["NODE_1_ID"].Value))
                                {
                                    ActiveFeature.Components.GetComponent(ConnectivityG3eCno).Recordset.Fields["NODE_1_ID"].Value = relationshipcandidates[i].Components.GetComponent(ConnectivityG3eCno).Recordset.Fields["NODE_2_ID"].Value;
                                    ActiveFeature.Components.GetComponent(ConnectivityG3eCno).Recordset.Fields["NODE_2_ID"].Value = relationshipcandidates[i + 1].Components.GetComponent(ConnectivityG3eCno).Recordset.Fields["NODE_1_ID"].Value;
                                    ActiveFeature.Components.GetComponent(ConnectivityG3eCno).Recordset.Update();
                                    break;
                                }
                                else
                                {
                                    ActiveFeature.Components.GetComponent(ConnectivityG3eCno).Recordset.Fields["NODE_1_ID"].Value = relationshipcandidates[i].Components.GetComponent(ConnectivityG3eCno).Recordset.Fields["NODE_2_ID"].Value;
                                    relationshipcandidates[i + 1].Components.GetComponent(ConnectivityG3eCno).Recordset.Fields["NODE_1_ID"].Value = 0;
                                    relationshipcandidates[i + 1].Components.GetComponent(ConnectivityG3eCno).Recordset.Update();
                                    gtRelationshipService.SilentEstablish(m_RNO, relationshipcandidates[i + 1], GTRelationshipOrdinalConstants.gtrelRelationshipOrdinal2, GTRelationshipOrdinalConstants.gtrelRelationshipOrdinal1);
                                    break;
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error " + ex.Message, sMsgBoxCaption, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                if (gtRelationshipService!=null)
                {
                    gtRelationshipService.Dispose();
                    gtRelationshipService = null;
                }
            }
        }
        #endregion
    }
}
