namespace GTechnology.Oncor.CustomAPI
{
    public class Service : Cable
    {
        private int m_FID;
        private int m_SourceFID;
        private int m_SpanIndex;
        private ServicePoint m_ServicePoint;

        public int ServiceFID
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

        public int SpanIndex
        {
            get
            {
                return m_SpanIndex;
            }
            set
            {
                m_SpanIndex = value;
            }
        }

        public ServicePoint ServicePoint
        {
            get
            {
                return m_ServicePoint;
            }
            set
            {
                m_ServicePoint = value;
            }
        }
    }
}
