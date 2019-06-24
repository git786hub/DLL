using System;
using System.Windows.Forms;
using Intergraph.GTechnology.API;

namespace GTechnology.Oncor.CustomAPI
{
    public class clsCalculate
    {
        private double m_DiversityFactorSummer;
        private double m_DiversityFactorWinter;
        private double m_DiversityFactorCustomerCountHigh;
        private double m_DiversityFactorCustomerSummerLow;
        private double m_DiversityFactorCustomerSummerHigh;
        private double m_DiversityFactorCustomerWinterLow;
        private double m_DiversityFactorCustomerWinterHigh;
        private double m_PowerFactorSummer;
        private double m_PowerFactorWinter;
        private double m_VoltageSecondary;
        private double m_VoltageFactor;
        private double m_Voltage3Phase;
        private double m_TypeFactor;
        private double m_XfmrImpedance;
        private double m_XfmrXoverR;
        private double m_NumberOfCablesPerPhase;
        private double m_DeratingValue;
        private int m_NeutralVDropFactor;

        private IGTApplication m_Application = GTClassFactory.Create<IGTApplication>();

        public double DiversityFactorSummer
        {
            get
            {
                return m_DiversityFactorSummer;
            }
            set
            {
                m_DiversityFactorSummer = value;
            }
        }

        public double DiversityFactorWinter
        {
            get
            {
                return m_DiversityFactorWinter;
            }
            set
            {
                m_DiversityFactorWinter = value;
            }
        }

        public double DiversityFactorCustomerCountHigh
        {
            get
            {
                return m_DiversityFactorCustomerCountHigh;
            }
            set
            {
                m_DiversityFactorCustomerCountHigh = value;
            }
        }

        public double DiversityFactorCustomerSummerLow
        {
            get
            {
                return m_DiversityFactorCustomerSummerLow;
            }
            set
            {
                m_DiversityFactorCustomerSummerLow = value;
            }
        }

        public double DiversityFactorCustomerSummerHigh
        {
            get
            {
                return m_DiversityFactorCustomerSummerHigh;
            }
            set
            {
                m_DiversityFactorCustomerSummerHigh = value;
            }
        }

        public double DiversityFactorCustomerWinterLow
        {
            get
            {
                return m_DiversityFactorCustomerWinterLow;
            }
            set
            {
                m_DiversityFactorCustomerWinterLow = value;
            }
        }

        public double DiversityFactorCustomerWinterHigh
        {
            get
            {
                return m_DiversityFactorCustomerWinterHigh;
            }
            set
            {
                m_DiversityFactorCustomerWinterHigh = value;
            }
        }

        public double PowerFactorSummer
        {
            get
            {
                return m_PowerFactorSummer;
            }
            set
            {
                m_PowerFactorSummer = value;
            }
        }

        public double PowerFactorWinter
        {
            get
            {
                return m_PowerFactorWinter;
            }
            set
            {
                m_PowerFactorWinter = value;
            }
        }

        public int NeutralVDropFactor
        {
            get
            {
                return m_NeutralVDropFactor;
            }
            set
            {
                m_NeutralVDropFactor = value;
            }
        }

        public double VoltageSecondary
        {
            get
            {
                return m_VoltageSecondary;
            }
            set
            {
                m_VoltageSecondary = value;
            }
        }

        public double VoltageFactor
        {
            get
            {
                return m_VoltageFactor;
            }
            set
            {
                m_VoltageFactor = value;
            }
        }

        public double TypeFactor
        {
            get
            {
                return m_TypeFactor;
            }
            set
            {
                m_TypeFactor = value;
            }
        }

        public double Voltage3Phase
        {
            get
            {
                return m_Voltage3Phase;
            }
            set
            {
                m_Voltage3Phase = value;
            }
        }

        public double XfmrImpedance
        {
            get
            {
                return m_XfmrImpedance;
            }
            set
            {
                m_XfmrImpedance = value;
            }
        }

        public double XfmrXoverR
        {
            get
            {
                return m_XfmrXoverR;
            }
            set
            {
                m_XfmrXoverR = value;
            }
        }

        public double NumberOfCablesPerPhase
        {
            set
            {
                m_NumberOfCablesPerPhase = value;
            }
        }

        public double DeratingValue
        {
            set
            {
                m_DeratingValue = value;
            }
        }

        // Calculate the Summer and Winter load in KVA using the passed in Summer and Winter loads in KW and the customer count.
        public bool LoadKVA(double summerLoad, double winterLoad, double customerCount, ref double summerLoadKVA, ref double winterLoadKVA)
        {
            double summerDiversity;
            double winterDiversity;
            bool returnValue = false;

            try
            {
                if (customerCount == 1)
                {
                    summerLoadKVA = summerLoad / m_PowerFactorSummer;
                    winterLoadKVA = winterLoad / m_PowerFactorWinter;
                }
                else
                {
                    if (customerCount < m_DiversityFactorCustomerCountHigh)
                    {
                        summerDiversity = m_DiversityFactorCustomerSummerLow;
                        winterDiversity = m_DiversityFactorCustomerWinterLow;
                    }
                    else
                    {
                        summerDiversity = m_DiversityFactorCustomerSummerHigh;
                        winterDiversity = m_DiversityFactorCustomerWinterHigh;
                    }

                    summerLoadKVA = summerLoad / m_PowerFactorSummer * summerDiversity * (customerCount + m_DiversityFactorSummer) / (customerCount + 1);
                    winterLoadKVA = winterLoad / m_PowerFactorWinter * winterDiversity * (customerCount + m_DiversityFactorWinter) / (customerCount + 1);
                }

                returnValue = true;
            }
            catch (Exception ex)
            {
                returnValue = false;
                MessageBox.Show(m_Application.ApplicationWindow, ConstantsDT.ERROR_SC_CALCULATE_LOADKVA + ": " + ex.Message, ConstantsDT.APPLICATION_NAME, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }

            return returnValue;
        }

        // Calculate the Transformer Voltage Drop using the passed in Transformer size and the summer and winter load in KVA.
        public bool TransformerVoltageDrop(double xfmrSize, double loadKVASummer, double loadKVAWinter,
                                           ref double voltageDrop, ref double voltageDropVolts, ref double voltageDropPct, ref double vlmag)
        {
            double loadKVA = 0;
            bool returnValue = false;

            try
            {
                double vlmagSummer = m_VoltageSecondary - (loadKVASummer / xfmrSize * m_XfmrImpedance * m_VoltageSecondary);
                double vlmagWinter = m_VoltageSecondary - (loadKVAWinter / xfmrSize * m_XfmrImpedance * m_VoltageSecondary);

                if (vlmagSummer < vlmagWinter)
                {
                    vlmag = vlmagSummer;
                    loadKVA = loadKVASummer;
                }
                else
                {
                    vlmag = vlmagWinter;
                    loadKVA = loadKVAWinter;
                }

                voltageDrop = Math.Round(vlmag / 2, 1);
                voltageDropPct = m_XfmrImpedance * loadKVA / xfmrSize * 100;
                voltageDropVolts = voltageDropPct * m_VoltageSecondary;

                returnValue = true;
            }
            catch (Exception ex)
            {
                returnValue = false;
                MessageBox.Show(m_Application.ApplicationWindow, ConstantsDT.ERROR_SC_CALCULATE_XFMR_VDROP + ": " + ex.Message, ConstantsDT.APPLICATION_NAME, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }

            return returnValue;
        }

        // Calculate the Conductor Voltage Drop using the passed in upstream voltage, cable properties, load and customer count.
        //  Both the summer and winter voltage drop will be calculated. The larger voltage drop will be returned.
        public bool ConductorVoltageDrop(double upstreamVlmag, double upstreamVoltage, double length, CommonDT.CableProperties cableProperties, double loadSummer, double loadWinter,
                                         double customerCount, ref string season, ref double voltageDrop, ref double voltageDropVolts, ref double voltageDropPct, ref double vlmag)
        {
            double vlmagSummer = 0;
            double vlmagWinter = 0;
            double loadKVASummer = 0;
            double loadKVAWinter = 0;
            bool returnValue = false;

            try
            {
                LoadKVA(loadSummer, loadWinter, customerCount, ref loadKVASummer, ref loadKVAWinter);

                ConductorVlmag(upstreamVlmag, upstreamVoltage, length, cableProperties, loadKVASummer, m_PowerFactorSummer, ref vlmagSummer);
                ConductorVlmag(upstreamVlmag, upstreamVoltage, length, cableProperties, loadKVAWinter, m_PowerFactorWinter, ref vlmagWinter);

                if (vlmagSummer <= vlmagWinter)
                {
                    vlmag = vlmagSummer;
                    season = "Summer";
                }
                else
                {
                    vlmag = vlmagWinter;
                    season = "Winter";
                }

                voltageDrop = Math.Round(vlmag / 2, 1);
                voltageDropPct = (upstreamVlmag - vlmag) / vlmag * 100;
                voltageDropVolts = upstreamVoltage - voltageDrop;

                returnValue = true;
            }
            catch (Exception ex)
            {
                returnValue = false;
                MessageBox.Show(m_Application.ApplicationWindow, ConstantsDT.ERROR_SC_CALCULATE_CONDUCTOR_VDROP + ": " + ex.Message, ConstantsDT.APPLICATION_NAME, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }

            return returnValue;
        }

        // Calculate the Conductor Vlmag using the upstream vlmag, upstream voltage, cable properties and load.
        public bool ConductorVlmag(double upstreamVlmag, double upstreamVoltage, double length, CommonDT.CableProperties cableProperties, double load,
                                   double powerFactor, ref double vlmag)
        {            
            bool returnValue = false;

            try
            {
                double Rdc = cableProperties.Rdc25;
                double TcMax = cableProperties.Tc;
                double TAmbient = cableProperties.Tcmin;
                double Amps = cableProperties.Ampacity;
                double TZero = cableProperties.Tzero;
                double ODin = cableProperties.OdInches;
                double DCin = cableProperties.DcInches;
                double LoadAmps = load * 1000 / upstreamVlmag;
                double rf = Math.Sqrt(1 - Math.Pow(powerFactor, 2));
                double GMRin = cableProperties.Gmrf * DCin / 2;
                double X = 0.2794 / 5.28 * Math.Log10(ODin / GMRin);
                double Tc = (TcMax - TAmbient) / Math.Pow(Amps, 2) * Math.Pow(LoadAmps, 2) + TAmbient;
                double Rdcnm = Rdc * (Tc + TZero) / (25 + TZero) * 1000;
                double Rac = Rdcnm * (1 + 11 / Math.Pow((Rdcnm + 4 / Rdcnm - 2.56 / Math.Pow(Rdcnm, 2)), 2)) / 1000;
                double alpha = Math.Asin(m_NeutralVDropFactor * length / 1000 * LoadAmps * (X * powerFactor - Rac * rf) / upstreamVlmag);

                vlmag = upstreamVlmag * Math.Cos(alpha) - m_NeutralVDropFactor * length / 1000 * LoadAmps * (Rac * powerFactor + X * rf);

                if (double.IsNaN(vlmag))
                {
                    vlmag = 0;
                }

                returnValue = true;
            }
            catch (Exception ex)
            {
                returnValue = false;
                MessageBox.Show(m_Application.ApplicationWindow, ConstantsDT.ERROR_SC_CALCULATE_CONDUCTOR_VLMAG + ": " + ex.Message, ConstantsDT.APPLICATION_NAME, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }

            return returnValue;
        }

        // Calculate the Transformer Voltage Drop for a commercial network using the passed in load.
        public bool TransformerVoltageDropCommercial(double loadSummer, double loadWinter, ref double voltageDrop, ref double voltageDropVolts, ref double voltageDropPct, ref double actualVolts)
        {
            bool returnValue = false;

            try
            {
                double RaP = (m_XfmrImpedance / Math.Sqrt(Math.Pow((1 + m_XfmrXoverR), 2)));

                double actualVoltsSummer = m_VoltageSecondary * m_VoltageFactor - (loadSummer / m_PowerFactorSummer) / m_TypeFactor / (loadSummer / m_PowerFactorSummer)
                              * m_Voltage3Phase * (RaP * Math.Cos(Math.Acos(m_PowerFactorSummer)))
                              + m_XfmrImpedance * RaP * Math.Sin(Math.Acos(m_PowerFactorSummer));

                double actualVoltsWinter = m_VoltageSecondary * m_VoltageFactor - (loadWinter / m_PowerFactorWinter) / m_TypeFactor / (loadWinter / m_PowerFactorWinter)
                              * m_Voltage3Phase * (RaP * Math.Cos(Math.Acos(m_PowerFactorWinter)))
                              + m_XfmrImpedance * RaP * Math.Sin(Math.Acos(m_PowerFactorWinter));

                if (actualVoltsSummer <= actualVoltsWinter)
                {
                    actualVolts = actualVoltsSummer;
                }
                else
                {
                    actualVolts = actualVoltsWinter;
                }

                voltageDrop = actualVolts / m_VoltageFactor;
                voltageDropPct = Math.Abs((actualVolts - (m_VoltageSecondary * m_VoltageFactor)) / (m_VoltageSecondary * m_VoltageFactor) * 100);
                voltageDropVolts = voltageDropPct * m_VoltageSecondary;


                returnValue = true;
            }
            catch (Exception ex)
            {
                returnValue = false;
                MessageBox.Show(m_Application.ApplicationWindow, ConstantsDT.ERROR_SC_CALCULATE_XFMR_VDROP + ": " + ex.Message, ConstantsDT.APPLICATION_NAME, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }

            return returnValue;
        }

        // Calculate the Conductor Voltage Drop for a commercial network using the passed in upstream voltage, cable properties, load and customer count.
        //  Both the summer and winter voltage drop will be calculated. The larger voltage drop will be returned.
        public bool ConductorVoltageDropCommercial(double upstreamVolts, double loadSummer, double loadWinter, double length, CommonDT.CableProperties cableProperties, 
                                                   ref string season, ref double voltageDrop, ref double voltageDropVolts, ref double voltageDropPct, ref double actualVolts)
        {
            double actualVoltsSummer = 0;
            double actualVoltsWinter = 0;
            
            bool returnValue = false;

            try
            {
                ConductorActualVoltsCommercial(upstreamVolts, loadSummer, m_PowerFactorSummer, length, cableProperties, ref actualVoltsSummer);
                ConductorActualVoltsCommercial(upstreamVolts, loadWinter, m_PowerFactorWinter, length, cableProperties, ref actualVoltsWinter);

                if (actualVoltsSummer <= actualVoltsWinter)
                {
                    actualVolts = actualVoltsSummer;
                    season = "Summer";
                }
                else
                {
                    actualVolts = actualVoltsWinter;
                    season = "Winter";
                }

                voltageDrop = actualVolts / m_VoltageFactor;
                voltageDropPct =  (upstreamVolts - actualVolts) / actualVolts* 100;
                voltageDropVolts = voltageDropPct * m_VoltageSecondary;

                returnValue = true;
            }
            catch (Exception ex)
            {
                returnValue = false;
                MessageBox.Show(m_Application.ApplicationWindow, ConstantsDT.ERROR_SC_CALCULATE_CONDUCTOR_VDROP + ": " + ex.Message, ConstantsDT.APPLICATION_NAME, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }

            return returnValue;
        }

        // Calculate the Conductor Actual Volts for a commercial network using the passed in upstream volts, load and cable properties.
        public bool ConductorActualVoltsCommercial(double upstreamVolts, double load, double powerFactor, double length, CommonDT.CableProperties cableProperties,
                                                   ref double actualVolts)
        {
            bool returnValue = false;

            try
            {
                double nc = m_NumberOfCablesPerPhase;
                double derating = m_DeratingValue / 100;
                double Rdc = cableProperties.Rdc25;
                double TZero = cableProperties.Tzero;
                double TcMax = cableProperties.Tc;
                double TAmbient = cableProperties.Tcmin;
                double Amps = cableProperties.Ampacity;
                double X = cableProperties.X;
                double lt = load / powerFactor * 1000 / (3 * m_Voltage3Phase);
                double Tc = (TcMax - TAmbient) / Math.Pow((Amps * nc * derating), 2) * Math.Pow(lt, 2) + TAmbient;
                double Rdcnm = Rdc * (Tc + TZero) / (25 + TZero) * 1000;
                double Rac = Rdcnm * (1 + 11 / Math.Pow((Rdcnm + 4 / Rdcnm - 2.56 / Math.Pow(Rdcnm, 2)), 2)) / 1000;

                actualVolts = upstreamVolts - lt * length / 1000 / nc * (Rac * Math.Cos(Math.Acos(powerFactor)) + X * Math.Sin(Math.Acos(powerFactor)));

                returnValue = true;
            }
            catch (Exception ex)
            {
                returnValue = false;
                MessageBox.Show(m_Application.ApplicationWindow, ConstantsDT.ERROR_SC_CALCULATE_CONDUCTOR_VDROP + ": " + ex.Message, ConstantsDT.APPLICATION_NAME, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }

            return returnValue;
        }
    }
}
