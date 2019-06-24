using Intergraph.GTechnology.API;

namespace GTechnology.Oncor.CustomAPI
{
    class IsoScenarioFeature
    {

        #region Variables
        private IGTKeyObjects m_RelatedFeaturesNode1;
        private IGTKeyObjects m_RelatedFeaturesNode2;
        private IGTKeyObject m_IsolationPoint1;
        private IGTKeyObject m_IsolationPoint2;
        private IGTKeyObject m_IsolationPoint3;
        private IGTKeyObject m_GtKeyObject = GTClassFactory.Create<IGTKeyObject>();
        private string m_Phase;
        private string m_FeatureState;

        #endregion

        #region Properties
        public IGTKeyObjects RelatedFeaturesNode1
        {
            get
            {
                return m_RelatedFeaturesNode1;
            }
            set
            {
                m_RelatedFeaturesNode1 = value;
            }
        }

        public IGTKeyObjects RelatedFeaturesNode2
        {
            get
            {
                return m_RelatedFeaturesNode2;
            }
            set
            {
                m_RelatedFeaturesNode2 = value;
            }
        }

        public IGTKeyObject IsolationPoint1
        {
            get
            {
                return m_IsolationPoint1;
            }
            set
            {
                m_IsolationPoint1 = value;
            }
        }

        public IGTKeyObject IsolationPoint2
        {
            get
            {
                return m_IsolationPoint2;
            }
            set
            {
                m_IsolationPoint2 = value;
            }
        }

        public IGTKeyObject IsolationPoint3
        {
            get
            {
                return m_IsolationPoint3;
            }
            set
            {
                m_IsolationPoint3 = value;
            }
        }

        public IGTKeyObject GtKeyObject
        {
            get
            {
                return m_GtKeyObject;
            }
            set
            {
                m_GtKeyObject = value;
            }
        }

        public string Phase
        {
            get
            {
                return m_Phase;
            }
            set
            {
                m_Phase = value;
            }
        }

        public string FeatureState
        {
            get
            {
                return m_FeatureState;
            }
            set
            {
                m_FeatureState = value;
            }
        }

        #endregion
    }
}
