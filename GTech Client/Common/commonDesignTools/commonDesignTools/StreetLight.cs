namespace GTechnology.Oncor.CustomAPI
{
    public class StreetLight : Load
    {
        private int m_FID;
        private int m_SourceFID;
        private string m_LightType;
        private string m_CU;
        private bool m_AllowCUEdit;
        private int m_SpanIndexFrom;
        private int m_SpanIndexTo;
        private CommonDT.StreetLightProperties m_StreetLightProperties = new CommonDT.StreetLightProperties();

        public int FID
        {
            get
            {
                return m_FID;
            }
            set
            {
                m_FID = value;
            }
        }

        public int SourceFID
        {
            get
            {
                return m_SourceFID;
            }
            set
            {
                m_SourceFID = value;
            }
        }

        public int SpanIndexFrom
        {
            get
            {
                return m_SpanIndexFrom;
            }
            set
            {
                m_SpanIndexFrom = value;
            }
        }

        public int SpanIndexTo
        {
            get
            {
                return m_SpanIndexTo;
            }
            set
            {
                m_SpanIndexTo = value;
            }
        }

        public string LightType
        {
            get
            {
                return m_LightType;
            }
            set
            {
                m_LightType = value;
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

        public CommonDT.StreetLightProperties StreetLightProperties
        {
            get
            {
                return m_StreetLightProperties;
            }
            set
            {
                m_StreetLightProperties = value;
            }
        }
    }
}
