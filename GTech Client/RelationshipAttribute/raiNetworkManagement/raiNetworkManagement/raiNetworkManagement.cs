//----------------------------------------------------------------------------+
//        Class: raiNetworkManagement
//  Description: Validates whether the current user has network design privileges and generates a validation error of the appropriate priority.
//----------------------------------------------------------------------------+
//     $Author:: pnlella                                   $
//       $Date:: 26/04/18                                 $
//   $Revision:: 1                                        $
//----------------------------------------------------------------------------+
//    $History:: raiNetworkManagement.cs                     $
// 
// *****************  Version 1  *****************
// User: pnlella     Date: 26/04/18   Time: 15:00  Desc : Created
//----------------------------------------------------------------------------+
using System;
using Intergraph.GTechnology.API;
using Intergraph.GTechnology.Interfaces;
using gtCommandLogger;

namespace GTechnology.Oncor.CustomAPI
{
    public class raiNetworkManagement : IGTRelationshipAttribute
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
            string networkManaged = null;
            string networkRestricted = null;
            string errorPriorityManaged = Convert.ToString(Arguments.GetArgument(0));
            string errorPriorityUnmanaged = Convert.ToString(Arguments.GetArgument(1));
            short m_ManholeCNO = 10601;

            GTValidationLogger gTValidationLogger = null;

            int activeFID = 0;
            int relatedFID = 0;

            string activeFieldValue = string.Empty;
            string relatedFieldValue = string.Empty;

            IGTComponent activeComponent = ActiveComponents[ActiveComponentName];
            if (activeComponent != null && activeComponent.Recordset != null && activeComponent.Recordset.RecordCount > 0)
            {
                activeFID = int.Parse(activeComponent.Recordset.Fields["G3E_FID"].Value.ToString());
                activeFieldValue = Convert.ToString(activeComponent.Recordset.Fields[ActiveFieldName].Value);
            }

            IGTComponent relatedComponent = RelatedComponents[RelatedComponentName];
            if (relatedComponent != null && relatedComponent.Recordset != null && relatedComponent.Recordset.RecordCount > 0)
            {
                relatedFID = int.Parse(relatedComponent.Recordset.Fields["G3E_FID"].Value.ToString());
                relatedFieldValue = Convert.ToString(relatedComponent.Recordset.Fields[RelatedFieldName].Value);
            }

            if (new gtLogHelper().CheckIfLoggingIsEnabled())
            {
                LogEntries logEntries = new LogEntries
                {
                    ActiveComponentName = ActiveComponentName,
                    ActiveFID = activeFID,
                    ActiveFieldName = ActiveFieldName,
                    ActiveFieldValue = activeFieldValue,
                    JobID = DataContext.ActiveJob,
                    RelatedComponentName = RelatedComponentName,
                    RelatedFID = relatedFID,
                    RelatedFieldName = RelatedFieldName,
                    RelatedFieldValue = relatedFieldValue,
                    ValidationInterfaceName = "Network Management",
                    ValidationInterfaceType = "RAI",
                };
                gTValidationLogger = new GTValidationLogger(logEntries);

                gTValidationLogger.LogEntry("TIMING", "START", "Network Management Entry", "N/A", "");
            }

            try
            {
                ValidationRuleManager validateMsg = new ValidationRuleManager();
				
				 IGTApplication gtApplication = GTClassFactory.Create<IGTApplication>();
                 gtApplication.SetStatusBarText(GTStatusPanelConstants.gtaspcMessage, "Network Management Validation Started");

                if (m_oRelatedComponents.GetComponent(m_ManholeCNO).Recordset != null)
                {
                    if (!(m_oRelatedComponents.GetComponent(m_ManholeCNO).Recordset.EOF && m_oRelatedComponents.GetComponent(m_ManholeCNO).Recordset.BOF))
                    {
                        m_oRelatedComponents.GetComponent(m_ManholeCNO).Recordset.MoveFirst();

                        networkManaged = Convert.ToString(m_oRelatedComponents.GetComponent(m_ManholeCNO).Recordset.Fields["NETWORK_MANAGED_YN"].Value);
                        networkRestricted = Convert.ToString(m_oRelatedComponents.GetComponent(m_ManholeCNO).Recordset.Fields["NETWORK_RESTRICTED_YN"].Value);
                    }
                }

                if (!m_oDataContext.IsRoleGranted("PRIV_DESIGN_NET") && networkManaged == "Y" && networkRestricted == "Y")
                {
                    validateMsg.Rule_Id = "NETMGMT01";
                    validateMsg.BuildRuleMessage(GTClassFactory.Create<IGTApplication>(), null);

                    ErrorMessageArray[0] = validateMsg.Rule_MSG;
                    ErrorPriorityArray[0] = errorPriorityManaged;
                }

                if(!m_oDataContext.IsRoleGranted("PRIV_DESIGN_NET") && networkManaged == "N" && networkRestricted == "Y")
                {
                    validateMsg.Rule_Id = "NETMGMT02";
                    validateMsg.BuildRuleMessage(GTClassFactory.Create<IGTApplication>(), null);

                    ErrorMessageArray[0] = validateMsg.Rule_MSG;
                    ErrorPriorityArray[0] = errorPriorityUnmanaged;

                }
				
				gtApplication.SetStatusBarText(GTStatusPanelConstants.gtaspcMessage, "Network Management Validation Completed");

                if(gTValidationLogger != null)
                    gTValidationLogger.LogEntry("TIMING", "END", "Network Management Exit", "N/A", "");
            }
            catch (Exception ex)
            {
                throw new Exception("Error in Network Management Relationship Attribute Interface:" + ex.Message);
            }
        }
    }
}
