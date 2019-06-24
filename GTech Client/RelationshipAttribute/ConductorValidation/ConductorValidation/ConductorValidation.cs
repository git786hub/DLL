using System;
using Intergraph.GTechnology.API;
using Intergraph.GTechnology.Interfaces;
using System.Windows.Forms;
using gtCommandLogger;

//----------------------------------------------------------------------------+
//  Class: raiProtectiveDeviceID
//  Description: This interface validates the Prmary Conductor if it is connected to
//  a single phase or if Primary Conductor with a dead end does not have a Guy
//----------------------------------------------------------------------------+
//  $Author:: Shubham Agarwal                                                       $
//  $Date:: 30/07/17                                                                $
//  $Revision:: 1                                                                   $
//----------------------------------------------------------------------------+
//  $History:: raiConductorValidation.cs                                             $
// 
// *****************  Version 1  *****************
//  User: sagarwal     Date: 30/07/17    Time: 18:00
//  User: pnlella      Date: 20/02/18    Time: 11:00 Modified the logic as per ONCORDEV-1278
//----------------------------------------------------------------------------+

namespace GTechnology.Oncor.CustomAPI
{
    public class raiConductorValidation : IGTRelationshipAttribute
    {
        private int m_iActiveANO = 0;
        private string m_sActiveComponentName = string.Empty;
        private IGTComponents m_oActiveComponents = null;
        private string m_sActiveFieldName = string.Empty;
        private IGTFieldValue m_oActiveFieldValue = null;
        private GTArguments m_oGTArguments = null;
        private IGTDataContext m_oDataContext = null;
        private string m_sPriority = string.Empty;
        private int m_iRelatedANO = 0;
        private string m_sRelatedComponentName = string.Empty;
        private IGTComponents m_oRelatedComponents = null;
        private string m_sRelatedFieldName = string.Empty;
        private IGTFieldValue m_oRelatedFieldValue = null;
     

        public int ActiveANO
        {
            get
            {
              return m_iActiveANO;
            }

            set
            {
                m_iActiveANO = value;
            }
        }

        public string ActiveComponentName
        {
            get
            {
                return m_sActiveComponentName;
            }

            set
            {
                m_sActiveComponentName = value;
            }
        }

        public IGTComponents ActiveComponents
        {
            get
            {
               return m_oActiveComponents;
            }

            set
            {
                m_oActiveComponents = value;
            }
        }

        public string ActiveFieldName
        {
            get
            {
                return m_sActiveFieldName;
            }

            set
            {
                m_sActiveFieldName = value;
            }
        }

        public IGTFieldValue ActiveFieldValue
        {
            get
            {
               return m_oActiveFieldValue;
            }

            set
            {
                m_oActiveFieldValue = value;
            }
        }

        public GTArguments Arguments
        {
            get
            {
               return m_oGTArguments;
            }

            set
            {
                m_oGTArguments = value;
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

        public string Priority
        {
            get
            {
                return m_sPriority;
            }

            set
            {
                m_sPriority = value;
            }
        }

        public int RelatedANO
        {
            get
            {
                return m_iRelatedANO;
            }

            set
            {
                m_iRelatedANO = value;
            }
        }

        public string RelatedComponentName
        {
            get
            {
                return m_sRelatedComponentName;
            }

            set
            {
                m_sRelatedComponentName = value;
            }
        }

        public IGTComponents RelatedComponents
        {
            get
            {
               return m_oRelatedComponents;
            }

            set
            {
                m_oRelatedComponents = value;
            }
        }

        public string RelatedFieldName
        {
            get
            {
               return m_sRelatedFieldName;
            }

            set
            {
                m_sRelatedFieldName = value;
            }
        }

        public IGTFieldValue RelatedFieldValue
        {
            get
            {
                return m_oRelatedFieldValue;
            }

            set
            {
                m_oRelatedFieldValue = value;
            }
        }

        public void AfterEstablish()
        {
          
        }
        public void Validate(out string[] ErrorPriorityArray, out string[] ErrorMessageArray)
        {
            ErrorPriorityArray = new string[1];
            ErrorMessageArray = new string[1];

            short iFNOActive = 0;
            int  iFIDActive = 0;

            short iFNORelated = 0;
            int iFIDRelated;

            string sActivePhase = string.Empty;
            IGTRelationshipService relationShipService = GTClassFactory.Create<IGTRelationshipService>();

            IGTKeyObject oRelatedFeature = null;
            IGTKeyObjects oRelatedFeatureOwnedBy = null;
            string sPriCondNodeType = string.Empty;

            IGTKeyObject oActiveFeature = null;
            IGTKeyObjects oActiveFeatureOwnedBy = null;
            int activePhaseCount = 0;

            GTValidationLogger gTValidationLogger = null;
            IGTComponent activeComponent = ActiveComponents[ActiveComponentName];
            int activeFID = 0;

            if (activeComponent != null)
            {
                activeComponent.Recordset.MoveFirst();
                activeFID = int.Parse(activeComponent.Recordset.Fields["G3E_FID"].Value.ToString());
            }

            IGTComponent relatedComponent = RelatedComponents[RelatedComponentName];
            int relatedFID = 0;

            if (relatedComponent != null)
            {
                relatedComponent.Recordset.MoveFirst();
                relatedFID = int.Parse(relatedComponent.Recordset.Fields["G3E_FID"].Value.ToString());
            }

            if (new gtLogHelper().CheckIfLoggingIsEnabled())
            {
                LogEntries logEntries = new LogEntries
                {
                    ActiveComponentName = ActiveComponentName,
                    ActiveFID = activeFID,
                    ActiveFieldName = ActiveFieldName,
                    ActiveFieldValue = Convert.ToString(activeComponent.Recordset.Fields[ActiveFieldName].Value),
                    JobID = DataContext.ActiveJob,
                    RelatedComponentName = RelatedComponentName,
                    RelatedFID = relatedFID,
                    RelatedFieldName = RelatedFieldName,
                    RelatedFieldValue = Convert.ToString(relatedComponent.Recordset.Fields[RelatedFieldName].Value),
                    ValidationInterfaceName = "Conductor Validation",
                    ValidationInterfaceType = "RAI",
                };
                gTValidationLogger = new GTValidationLogger(logEntries);

                gTValidationLogger.LogEntry("TIMING", "START", "Conductor Validation Entry", "N/A", "");
            }



            try
            {
                if (m_oActiveComponents[m_sActiveComponentName].Recordset != null)
                {
                    if (!(m_oActiveComponents[m_sActiveComponentName].Recordset.EOF && m_oActiveComponents[m_sActiveComponentName].Recordset.BOF))
                    {
                        iFNOActive = Convert.ToInt16(m_oActiveComponents[m_sActiveComponentName].Recordset.Fields["g3e_fno"].Value);
                        iFIDActive = Convert.ToInt32(m_oActiveComponents[m_sActiveComponentName].Recordset.Fields["g3e_fid"].Value);

                        sActivePhase = Convert.ToString(m_oActiveComponents[m_sActiveComponentName].Recordset.Fields["PHASE_ALPHA"].Value);
                        activePhaseCount = sActivePhase.Length;
                        oActiveFeature = m_oDataContext.OpenFeature(iFNOActive, iFIDActive);
                    }
                }

                if (m_oRelatedComponents[m_sRelatedComponentName].Recordset != null)
                {
                    if (!(m_oRelatedComponents[m_sRelatedComponentName].Recordset.EOF && m_oRelatedComponents[m_sRelatedComponentName].Recordset.BOF))
                    {
                        iFNORelated = Convert.ToInt16(m_oRelatedComponents[m_sRelatedComponentName].Recordset.Fields["g3e_fno"].Value);
                        iFIDRelated = Convert.ToInt32(m_oRelatedComponents[m_sRelatedComponentName].Recordset.Fields["g3e_fid"].Value);

                        oRelatedFeature = m_oDataContext.OpenFeature(iFNORelated, iFIDRelated);

                        if (iFNORelated == 10) //Primary Conductor Node - Store the Type as well
                        {
                            oRelatedFeature.Components["PRI_COND_NODE_N"].Recordset.MoveFirst();
                            sPriCondNodeType = Convert.ToString(oRelatedFeature.Components["PRI_COND_NODE_N"].Recordset.Fields["TYPE_C"].Value);
                        }
                    }
                }

                if ((iFNOActive == 8 || iFNOActive == 9) && (iFNORelated == 13 || iFNORelated == 39) && activePhaseCount == 1)
                {
                    if (sActivePhase!="*") 
                    {
                        relationShipService.DataContext = m_oDataContext;
                        relationShipService.ActiveFeature = oRelatedFeature;
                        oRelatedFeatureOwnedBy = relationShipService.GetRelatedFeatures(3);

                        if (oRelatedFeatureOwnedBy != null && oRelatedFeatureOwnedBy.Count > 0)
                        {
                            for (int i = 0; i < oRelatedFeatureOwnedBy.Count; i++)
                            {
                                if (oRelatedFeatureOwnedBy[i].FNO == 19)
                                {
                                    ErrorPriorityArray[0] = m_sPriority;
                                    ErrorMessageArray[0] = "Single-phase conductor should not be connected to a switch gear";
                                }
                            }
                        }
                    }
                }

                if ((iFNOActive == 8 || iFNOActive == 9) && iFNORelated == 10 && sPriCondNodeType.Equals("DEADEND"))
                {
                    relationShipService.DataContext = m_oDataContext;
                    relationShipService.ActiveFeature = oActiveFeature;
                    oActiveFeatureOwnedBy = relationShipService.GetRelatedFeatures(3);

                    if (oActiveFeatureOwnedBy != null && oActiveFeatureOwnedBy.Count > 0)
                    {
                        for (int i = 0; i < oActiveFeatureOwnedBy.Count; i++)
                        {
                            if (oActiveFeatureOwnedBy[i].FNO == 110)
                            {
                                IGTKeyObject oPoleOwner = null;
                                bool bGuyFound = false;
                                IGTKeyObjects oFeatureOwnedByPole = null;

                                oPoleOwner = m_oDataContext.OpenFeature(oActiveFeatureOwnedBy[i].FNO, oActiveFeatureOwnedBy[i].FID);
                                
                                relationShipService.DataContext = m_oDataContext;
                                relationShipService.ActiveFeature = oPoleOwner;
                                oFeatureOwnedByPole = relationShipService.GetRelatedFeatures(2);

                                if (oFeatureOwnedByPole != null && oFeatureOwnedByPole.Count > 0)
                                {
                                    for (int j = 0; j < oFeatureOwnedByPole.Count; j++)
                                    {
                                        if (oFeatureOwnedByPole[j].FNO == 105)
                                        {
                                            bGuyFound = true;
                                            break;
                                        }
                                    }                                    
                                }
                                if (!bGuyFound)
                                {
                                    ErrorPriorityArray[0] = m_sPriority;
                                    ErrorMessageArray[0] = "Primary Conductor with a dead end should have a guy present";
                                }                                
                            }
                        }
                    }
                }

                if(gTValidationLogger != null)
                    gTValidationLogger.LogEntry("TIMING", "END", "Conductor Validation Exit", "N/A", "");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error in Conductor Validation Relationship Attribute Interface \n" +  ex.Message,"G/Technology");
            }  
            finally
            {
                if (relationShipService!=null)
                {
                    relationShipService.Dispose();
                    relationShipService = null;
                }
            }
        }
    }
}
