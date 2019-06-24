namespace GTechnology.Oncor.CustomAPI
{
    public class Load
    {
        private double m_Resistance;
        private double m_Reactance;
        private double m_Impedance;
        private double m_VoltageDrop;
        private double m_VoltageDropPercent;
        private double m_VoltageMag;
        private double m_CustomerCount;
        private double m_LoadSummerActual;
        private double m_LoadSummerEstimated;
        private double m_LoadWinterActual;
        private double m_LoadWinterEstimated;
        private double m_LoadSummer;
        private double m_LoadWinter;
        private string m_LoadUsedSummer;
        private string m_LoadUsedWinter;
        private double m_Voltage;
        private double m_Flicker;
        private string m_Season;
        private double m_FutureServiceCount = 0;
        private double m_FutureServiceCountAccum = 0;
        private double m_FutureServiceLoad = 0;
        private double m_FutureServiceLength = 0;

        private double m_NumberOfLights;
        private double m_LoadAmps;
        private double m_AllowedMinVoltage;
        private double m_AccumVoltageDropPercent;

        public double Voltage
        {
            get
            {
                return m_Voltage;
            }
            set
            {
                m_Voltage = value;
            }
        }

        public double Flicker
        {
            get
            {
                return m_Flicker;
            }
            set
            {
                m_Flicker = value;
            }
        }

        public string Season
        {
            get
            {
                return m_Season;
            }
            set
            {
                m_Season = value;
            }
        }

        public double Resistance
        {
            get
            {
                return m_Resistance;
            }
            set
            {
                m_Resistance = value;
            }
        }

        public double Reactance
        {
            get
            {
                return m_Reactance;
            }
            set
            {
                m_Reactance = value;
            }
        }

        public double Impedance
        {
            get
            {
                return m_Impedance;
            }
            set
            {
                m_Impedance = value;
            }
        }

        public double VoltageDrop
        {
            get
            {
                return m_VoltageDrop;
            }
            set
            {
                m_VoltageDrop = value;
            }
        }

        public double VoltageDropPercent
        {
            get
            {
                return m_VoltageDropPercent;
            }
            set
            {
                m_VoltageDropPercent = value;
            }
        }

        public double VoltageMag
        {
            get
            {
                return m_VoltageMag;
            }
            set
            {
                m_VoltageMag = value;
            }
        }

        public double CustomerCount
        {
            get
            {
                return m_CustomerCount;
            }
            set
            {
                m_CustomerCount = value;
            }
        }

        public double LoadSummerActual
        {
            get
            {
                return m_LoadSummerActual;
            }
            set
            {
                m_LoadSummerActual = value;
            }
        }

        public double LoadSummerEstimated
        {
            get
            {
                return m_LoadSummerEstimated;
            }
            set
            {
                m_LoadSummerEstimated = value;
            }
        }

        public double LoadWinterActual
        {
            get
            {
                return m_LoadWinterActual;
            }
            set
            {
                m_LoadWinterActual = value;
            }
        }

        public double LoadWinterEstimated
        {
            get
            {
                return m_LoadWinterEstimated;
            }
            set
            {
                m_LoadWinterEstimated = value;
            }
        }

        public double LoadSummer
        {
            get
            {
                return m_LoadSummer;
            }
            set
            {
                m_LoadSummer = value;
            }
        }

        public double LoadWinter
        {
            get
            {
                return m_LoadWinter;
            }
            set
            {
                m_LoadWinter = value;
            }
        }

        public string LoadUsedSummer
        {
            get
            {
                return m_LoadUsedSummer;
            }
            set
            {
                m_LoadUsedSummer = value;
            }
        }

        public string LoadUsedWinter
        {
            get
            {
                return m_LoadUsedWinter;
            }
            set
            {
                m_LoadUsedWinter = value;
            }
        }

        public double FutureServiceCount
        {
            get
            {
                return m_FutureServiceCount;
            }
            set
            {
                m_FutureServiceCount = value;
            }
        }

        public double FutureServiceCountAccum
        {
            get
            {
                return m_FutureServiceCountAccum;
            }
            set
            {
                m_FutureServiceCountAccum = value;
            }
        }

        public double FutureServiceLength
        {
            get
            {
                return m_FutureServiceLength;
            }
            set
            {
                m_FutureServiceLength = value;
            }
        }

        public double FutureServiceLoad
        {
            get
            {
                return m_FutureServiceLoad;
            }
            set
            {
                m_FutureServiceLoad = value;
            }
        }

        public double NumberOfLights
        {
            get
            {
                return m_NumberOfLights;
            }
            set
            {
                m_NumberOfLights = value;
            }
        }

        public double LoadAmps
        {
            get
            {
                return m_LoadAmps;
            }
            set
            {
                m_LoadAmps = value;
            }
        }

        public double AllowedMinVoltage
        {
            get
            {
                return m_AllowedMinVoltage;
            }
            set
            {
                m_AllowedMinVoltage = value;
            }
        }

        public double AccumVoltageDropPercent
        {
            get
            {
                return m_AccumVoltageDropPercent;
            }
            set
            {
                m_AccumVoltageDropPercent = value;
            }
        }
    }
}
