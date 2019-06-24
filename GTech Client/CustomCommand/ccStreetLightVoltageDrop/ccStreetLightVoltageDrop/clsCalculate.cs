using System;

namespace GTechnology.Oncor.CustomAPI
{
    static class clsCalculate
    {
        private static double m_NominalVoltage;
        private static double m_ActualSourceVoltage;
        private static double m_AllowedMinimumVoltage;

        /// <summary>
        /// Property to set and return the Nominal Voltage.
        /// </summary>
        public static double NominalVoltage
        {
            get
            {
                return m_NominalVoltage;
            }
            set
            {
                m_NominalVoltage = value;
            }
        }

        /// <summary>
        /// Property to set and return the Actual Source Voltage.
        /// </summary>
        public static double ActualSourceVoltage
        {
            get
            {
                return m_ActualSourceVoltage;
            }
            set
            {
                m_ActualSourceVoltage = value;
            }
        }

        /// <summary>
        /// Property to set and return the Allowed Minimum Voltage.
        /// </summary>
        public static double AllowedMinimumVoltage
        {
            get
            {
                return m_AllowedMinimumVoltage;
            }
            set
            {
                m_AllowedMinimumVoltage = value;
            }
        }

        /// <summary>
        /// Calculates the Street Light Voltage Drop for the passed in Conductor.
        /// </summary>
        /// <param name="conductor">Conductor to calculate</param>
        /// <param name="upstreamVlmag">The upstream voltage drop</param>
        /// <param name="loadAmps">The downstream load</param>
        /// <param name="vlMag">The calculated voltage drop for the conductor</param>
        /// <param name="errorMessage">The error message if the calculation fails</param>
        /// <returns>Boolean indicating status</returns>
        public static bool CalculateStreetLightVoltageDrop(Conductor conductor, double upstreamVlmag, double loadAmps, ref double vlMag, ref string errorMessage)
        {
            bool returnValue = false;

            try
            {
                double tcMax = conductor.CableProperties.Tc;
                double tAmbient = conductor.CableProperties.Tcmin;
                double amps = conductor.CableProperties.Ampacity;
                double pf = conductor.StreetLight.StreetLightProperties.powerFactor;
                double x = conductor.CableProperties.X;
                double rf = Math.Sqrt((1 - Math.Pow(pf, 2)));
                double tc = (tcMax - tAmbient) / Math.Pow(amps, 2) * Math.Pow(loadAmps, 2) + tAmbient;
                double rdc = conductor.CableProperties.Rdc25;
                double rdcnm = rdc * (tc + 228.1) / (25 + 228.1) * 1000;
                double rac = rdcnm * (1 + 11 / Math.Pow((rdcnm + 4 / rdcnm - 2.56 / Math.Pow(rdcnm, 2)), 2)) / 1000;
                double f = 0;

                if (conductor.CableType.ToString().Contains("DPX"))
                {
                    f = 2;
                }
                else
                {
                    f = 1;
                }

                double alpha = Math.Asin((f * conductor.Length / 1000 * loadAmps * (x * pf - rac * rf) / upstreamVlmag));

                vlMag = upstreamVlmag * Math.Cos(alpha) - f * conductor.Length / 1000 * loadAmps * (rac * pf + x * rf);

                returnValue = true;
            }
            catch (Exception ex)
            {
                returnValue = false;
                errorMessage = ex.Message;
            }

            return returnValue;
        }        
    }
}
