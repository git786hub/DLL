using ADODB;
using Intergraph.GTechnology.API;

namespace GTechnology.Oncor.CustomAPI
{
    public enum ProcessObjectType
    {
        AccountImpact, RestrictedArea
    }
    public class FVObjectFactory
    {
        private IGTDataContext m_iGtDataContext;
        private IGTKeyObjects m_features;
        private Recordset m_ValidationErrors;

        public FVObjectFactory(IGTDataContext DataContext, IGTKeyObjects Features, Recordset ValidationErrors)
        {
            m_iGtDataContext = DataContext;
            m_features = Features;
            m_ValidationErrors = ValidationErrors;
        }

        public IProcessFV GetFVObject(ProcessObjectType p_ProcessObjectType)
        {
            IProcessFV returnObject = null;
            switch (p_ProcessObjectType)
            {
                case ProcessObjectType.AccountImpact:
                    returnObject = new AccountImpact();
                    ((AccountImpact)returnObject).DataContext = m_iGtDataContext;
                    ((AccountImpact)returnObject).Features = m_features;
                    ((AccountImpact)returnObject).ValidationErrors = m_ValidationErrors;
                    break;
                case ProcessObjectType.RestrictedArea:
                    returnObject = new RestrictedArea();
                    ((RestrictedArea)returnObject).DataContext = m_iGtDataContext;
                    ((RestrictedArea)returnObject).Features = m_features;
                    ((RestrictedArea)returnObject).ValidationErrors = m_ValidationErrors;
                    break;
                default:
                    returnObject = null;
                    break;
            }
            return returnObject;
        }
    }
}
