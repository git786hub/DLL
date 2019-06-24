using System.Collections.Generic;

namespace GTechnology.Oncor.CustomAPI
{
    public class Conductor : Cable
    {
        private int m_FID;
        private short m_FNO;
        private int m_SourceFID;
        private int m_SpanIndex;
        private Dictionary<int, Service> m_Services = new Dictionary<int, Service>();
        private StreetLight m_StreetLight;

        public int ConductorFID
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

        public short FNO
        {
            get
            {
                return m_FNO;
            }
            set
            {
                m_FNO = value;
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

        public Dictionary<int, Service> Services
        {
            get
            {
                return m_Services;
            }
            set
            {
                m_Services = value;
            }
        }

        public void AddService(int fid, Service service)
        {
            m_Services.Add(fid, service);
        }

        public StreetLight StreetLight
        {
            get
            {
                return m_StreetLight;
            }
            set
            {
                m_StreetLight = value;
            }
        }
    }
}
