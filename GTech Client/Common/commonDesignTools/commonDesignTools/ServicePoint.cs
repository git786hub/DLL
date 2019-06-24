namespace GTechnology.Oncor.CustomAPI
{
    public class ServicePoint : Load
    {
        private int m_FID;
        private int m_SourceFID;
        private int m_LockedRotorAmps;

        public int ServicePointFID
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

        public int LockedRotorAmps
        {
            get
            {
                return m_LockedRotorAmps;
            }
            set
            {
                m_LockedRotorAmps = value;
            }
        }
    }
}
