namespace GTechnology.Oncor.CustomAPI
{
    public class Cable : Load
    {
        private string m_CableType;
        private string m_CU;
        private double m_Length;
        private bool m_AllowCUEdit;
        private CommonDT.CableProperties m_CableProperties = new CommonDT.CableProperties();

        public string CableType
        {
            get
            {
                return m_CableType;
            }
            set
            {
                m_CableType = value;
            }
        }

        public string CU
        {
            get
            {
                return m_CU;
            }
            set
            {
                m_CU = value;
            }
        }

        public double Length
        {
            get
            {
                return m_Length;
            }
            set
            {
                m_Length = value;
            }
        }

        public bool AllowCUEdit
        {
            get
            {
                return m_AllowCUEdit;
            }
            set
            {
                m_AllowCUEdit = value;
            }
        }

        public CommonDT.CableProperties CableProperties
        {
            get
            {
                return m_CableProperties;
            }
            set
            {
                m_CableProperties = value;
            }
        }
    }
}
