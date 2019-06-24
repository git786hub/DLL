using System;
using System.Windows.Forms;
using Intergraph.GTechnology.API;
using System.Collections.Generic;
using ADODB;

namespace GTechnology.Oncor.CustomAPI
{
    public class CommonDT
    {
        private static Recordset m_CableRS;
        private static Recordset m_XfmrRS;
        private static Recordset m_StreetLightRS;
        private Recordset m_TraceResultsRS;
        private int m_PhaseCount;
        private IGTApplication m_Application = GTClassFactory.Create<IGTApplication>();
        private Dictionary<int, Conductor> m_ConductorsDictionary;
        private Dictionary<int, Service> m_ServicesDictionary;
        private Dictionary<int, StreetLight> m_StreetLightsDictionary;
        private int m_SpanIndex = 1;
        private int m_XfmrCustomerCount;
        private int m_LockedRotorAmps = 0;
        private double m_SummerPowerFactor;

        private static string m_WrNumber = string.Empty;
        private static string m_WrDescription = string.Empty;
        private static string m_JobType = string.Empty;

        private int m_CommandNumber = -1;
        private string m_CommandName = string.Empty;

        public enum DisplayResultsType {Trace, Problem};

        public struct CableProperties
        {
            public double Resistance;
            public double Reactance;
            public double DcInches;
            public double OdInches;
            public double Rdc25;
            public double Gmrf;
            public double Tc;
            public double Ampacity;
            public double Tcmin;
            public double Tzero;
            public double Rdc;
            public double Ract;
            public double X;
        }

        public struct XfmrProperties
        {
            public double Size;
            public string Type;
            public string Orientation;
            public double Resistance;
            public double Reactance;
            public double Impedance;
            public double XfmrXoverR;
            public double SecondaryVoltage;
            public double VoltageFactor;
            public double TypeFactor;
            public double Voltage3Phase;
            public string Voltage;
        }

        public struct StreetLightProperties
        {
            public double voltage;
            public double regulation;
            public double powerFactor;
            public double maxAmps;
        }

        public int CommandNumber
        {
            get
            {
                return m_CommandNumber;
            }
            set
            {
                m_CommandNumber = value;
            }
        }

        public string CommandName
        {
            get
            {
                return m_CommandName;
            }
            set
            {
                m_CommandName = value;
            }
        }

        public Dictionary<int, Conductor> ConductorsDictionary
        {
            get
            {
                return m_ConductorsDictionary;
            }
            set
            {
                m_ConductorsDictionary = value;
            }
        }

        public Dictionary<int, Service> ServicesDictionary
        {
            get
            {
                return m_ServicesDictionary;
            }
            set
            {
                m_ServicesDictionary = value;
            }
        }

        public Dictionary<int, StreetLight> StreetLightsDictionary
        {
            get
            {
                return m_StreetLightsDictionary;
            }
            set
            {
                m_StreetLightsDictionary = value;
            }
        }

        public int PhaseCount
        {
            get
            {
                return m_PhaseCount;
            }
            set
            {
                m_PhaseCount = value;
            }
        }

        public int XfmrCustomerCount
        {
            get
            {
                return m_XfmrCustomerCount;
            }
            set
            {
                m_XfmrCustomerCount = value;
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

        public double SummerPowerFactor
        {
            get
            {
                return m_SummerPowerFactor;
            }
            set
            {
                m_SummerPowerFactor = value;
            }
        }

        public Recordset TraceResultsRS
        {
            get
            {
                return m_TraceResultsRS;
            }
            set
            {
                m_TraceResultsRS = value;
            }
        }

        public static string WrDescription
        {
            get
            {
                return m_WrDescription;
            }
        }

        public static string JobType
        {
            get
            {
                return m_JobType;
            }
        }

        public static Recordset CableRS
        {
            get
            {
                return m_CableRS;
            }
        }

        public static Recordset StreetLightRS
        {
            get
            {
                return m_StreetLightRS;
            }
        }

        public static Recordset XfmrRS
        {
            get
            {
                return m_XfmrRS;
            }
        }

        // Query the g3e_job table for the job description and type for the active job.
        public static bool GetJobInformation()
        {            
            bool bReturnValue = false;
            IGTApplication gApp = GTClassFactory.Create<IGTApplication>();

            try
            {
                Recordset jobRS = gApp.DataContext.OpenRecordset(ConstantsDT.QUERY_JOB, CursorTypeEnum.adOpenDynamic, LockTypeEnum.adLockOptimistic, (int)CommandTypeEnum.adCmdText, gApp.DataContext.ActiveJob);

                if (jobRS.RecordCount > 0)
                {
                    m_WrDescription = jobRS.Fields[ConstantsDT.FIELD_JOB_DESCRIPTION].Value.ToString();
                    m_JobType = jobRS.Fields[ConstantsDT.FIELD_JOB_TYPE].Value.ToString();

                    if(!m_JobType.StartsWith("WR"))
                    {
                        MessageBox.Show(gApp.ApplicationWindow, ConstantsDT.ERROR_JOB_INVALID, ConstantsDT.APPLICATION_NAME, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        bReturnValue = false;
                    }
                    else
                    {
                        bReturnValue = true;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(gApp.ApplicationWindow, ConstantsDT.ERROR_JOB_QUERY + ": " + ex.Message, ConstantsDT.APPLICATION_NAME, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                bReturnValue = false;
            }

            gApp = null;
            return bReturnValue;
        }

        // Query for the command metadata
        public static bool GetCommandMetadata(string subsystemName, ref Recordset metadataRS)
        {
            bool bReturnValue = false;
            IGTApplication gApp = GTClassFactory.Create<IGTApplication>();

            try
            {
                metadataRS = gApp.DataContext.OpenRecordset(ConstantsDT.QUERY_COMMAND_METADATA, CursorTypeEnum.adOpenDynamic, LockTypeEnum.adLockOptimistic, (int)CommandTypeEnum.adCmdText, subsystemName);
                
                bReturnValue = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(gApp.ApplicationWindow, ConstantsDT.ERROR_RETRIEVING_COMMAND_METADATA + ": " + ex.Message, ConstantsDT.APPLICATION_NAME, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                bReturnValue = false;
            }

            gApp = null;
            return bReturnValue;
        }

        public bool GetCableMetadata()
        {            
            bool returnValue = false;

            // Get the cable metadata
            try
            {
                m_CableRS = m_Application.DataContext.OpenRecordset(ConstantsDT.QUERY_CABLE, CursorTypeEnum.adOpenDynamic, LockTypeEnum.adLockOptimistic, (int)CommandTypeEnum.adCmdText);

                if (m_CableRS.RecordCount == 0)
                {
                    MessageBox.Show(m_Application.ApplicationWindow, ConstantsDT.ERROR_SC_NO_CABLE_RECORDS, ConstantsDT.APPLICATION_NAME, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return false;
                }
                returnValue = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(m_Application.ApplicationWindow, ConstantsDT.ERROR_SC_CABLE_METADATA + ": " + ex.Message, ConstantsDT.APPLICATION_NAME, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return false;
            }

            return returnValue;
        }

        // Get the cable properties matching the passed in CU
        public bool GetCableProperties(string cu, ref string description, ref CableProperties cableProperties)
        {
            bool returnValue = false;

            description = "";

            cableProperties.DcInches = 0;
            cableProperties.Gmrf = 0;
            cableProperties.OdInches = 0;
            cableProperties.Ract = 0;
            cableProperties.Rdc = 0;
            cableProperties.Rdc25 = 0;
            cableProperties.Reactance = 0;
            cableProperties.Resistance = 0;
            cableProperties.Tc = 0;
            cableProperties.Ampacity = 0;
            cableProperties.Tcmin = 0;
            cableProperties.Tzero = 0;
            cableProperties.X = 0;

            try
            {
                m_CableRS.Filter = ConstantsDT.FIELD_CABLE_CU + " = '" + cu + "'";

                if (m_CableRS.RecordCount > 0)
                {
                    description = m_CableRS.Fields[ConstantsDT.FIELD_CABLE_DESC].Value.ToString();

                    if (!Convert.IsDBNull(m_CableRS.Fields[ConstantsDT.FIELD_CABLE_DC_INCH].Value))
                    {
                        cableProperties.DcInches = Convert.ToDouble(m_CableRS.Fields[ConstantsDT.FIELD_CABLE_DC_INCH].Value);
                    }

                    if (!Convert.IsDBNull(m_CableRS.Fields[ConstantsDT.FIELD_CABLE_GMRF].Value))
                    {
                        cableProperties.Gmrf = Convert.ToDouble(m_CableRS.Fields[ConstantsDT.FIELD_CABLE_GMRF].Value);
                    }

                    if (!Convert.IsDBNull(m_CableRS.Fields[ConstantsDT.FIELD_CABLE_OD_INCH].Value))
                    {
                        cableProperties.OdInches = Convert.ToDouble(m_CableRS.Fields[ConstantsDT.FIELD_CABLE_OD_INCH].Value);
                    }

                    if (!Convert.IsDBNull(m_CableRS.Fields[ConstantsDT.FIELD_CABLE_RACT].Value))
                    {
                        cableProperties.Ract = Convert.ToDouble(m_CableRS.Fields[ConstantsDT.FIELD_CABLE_RACT].Value);
                    }

                    if (!Convert.IsDBNull(m_CableRS.Fields[ConstantsDT.FIELD_CABLE_RDC].Value))
                    {
                        cableProperties.Rdc = Convert.ToDouble(m_CableRS.Fields[ConstantsDT.FIELD_CABLE_RDC].Value);
                    }

                    if (!Convert.IsDBNull(m_CableRS.Fields[ConstantsDT.FIELD_CABLE_RDC25].Value))
                    {
                        cableProperties.Rdc25 = Convert.ToDouble(m_CableRS.Fields[ConstantsDT.FIELD_CABLE_RDC25].Value);
                    }

                    if (!Convert.IsDBNull(m_CableRS.Fields[ConstantsDT.FIELD_CABLE_R_PER_FT].Value))
                    {
                        cableProperties.Resistance = Convert.ToDouble(m_CableRS.Fields[ConstantsDT.FIELD_CABLE_R_PER_FT].Value);
                    }

                    if (!Convert.IsDBNull(m_CableRS.Fields[ConstantsDT.FIELD_CABLE_X_PER_FT].Value))
                    {
                        cableProperties.Reactance = Convert.ToDouble(m_CableRS.Fields[ConstantsDT.FIELD_CABLE_X_PER_FT].Value);
                    }

                    if (!Convert.IsDBNull(m_CableRS.Fields[ConstantsDT.FIELD_CABLE_TC].Value))
                    {
                        cableProperties.Tc = Convert.ToDouble(m_CableRS.Fields[ConstantsDT.FIELD_CABLE_TC].Value);
                    }

                    if (!Convert.IsDBNull(m_CableRS.Fields[ConstantsDT.FIELD_CABLE_AMPACITY].Value))
                    {
                        cableProperties.Ampacity = Convert.ToDouble(m_CableRS.Fields[ConstantsDT.FIELD_CABLE_AMPACITY].Value);
                    }

                    if (!Convert.IsDBNull(m_CableRS.Fields[ConstantsDT.FIELD_CABLE_TCMIN].Value))
                    {
                        cableProperties.Tcmin = Convert.ToDouble(m_CableRS.Fields[ConstantsDT.FIELD_CABLE_TCMIN].Value);
                    }

                    if (!Convert.IsDBNull(m_CableRS.Fields[ConstantsDT.FIELD_CABLE_TZERO].Value))
                    {
                        cableProperties.Tzero = Convert.ToDouble(m_CableRS.Fields[ConstantsDT.FIELD_CABLE_TZERO].Value);
                    }

                    if (!Convert.IsDBNull(m_CableRS.Fields[ConstantsDT.FIELD_CABLE_X].Value))
                    {
                        cableProperties.X = Convert.ToDouble(m_CableRS.Fields[ConstantsDT.FIELD_CABLE_X].Value);
                    }

                    returnValue = true;
                }
                else
                {
                    returnValue = false;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(m_Application.ApplicationWindow, ConstantsDT.ERROR_SC_CABLE_TYPE + ": " + ex.Message, ConstantsDT.APPLICATION_NAME, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                returnValue = false;
            }

            return returnValue;
        }

        public bool GetTransformerMetadata()
        {
            bool returnValue = false;

            try
            {
                m_XfmrRS = m_Application.DataContext.OpenRecordset(ConstantsDT.QUERY_XFMR, CursorTypeEnum.adOpenDynamic, LockTypeEnum.adLockOptimistic, (int)CommandTypeEnum.adCmdText);

                if (m_XfmrRS.RecordCount == 0)
                {
                    MessageBox.Show(m_Application.ApplicationWindow, ConstantsDT.ERROR_SC_NO_XFMR_RECORDS, ConstantsDT.APPLICATION_NAME, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return false;
                }
                
                returnValue = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(m_Application.ApplicationWindow, ConstantsDT.ERROR_SC_XFMR_METADATA + ": " + ex.Message, ConstantsDT.APPLICATION_NAME, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return false;
            }

            return returnValue;
        }

        public bool GetStreetLightMetadata()
        {
            bool returnValue = false;

            // Get the Street Light metadata
            try
            {
                m_StreetLightRS = m_Application.DataContext.OpenRecordset(ConstantsDT.QUERY_STREET_LIGHT, CursorTypeEnum.adOpenDynamic, LockTypeEnum.adLockOptimistic, (int)CommandTypeEnum.adCmdText);

                if (m_StreetLightRS.RecordCount == 0)
                {
                    MessageBox.Show(m_Application.ApplicationWindow, ConstantsDT.ERROR_SL_NO_STREETLIGHT_RECORDS, ConstantsDT.APPLICATION_NAME, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return false;
                }
                returnValue = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(m_Application.ApplicationWindow, ConstantsDT.ERROR_SL_STREETLIGHT_METADATA + ": " + ex.Message, ConstantsDT.APPLICATION_NAME, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return false;
            }

            return returnValue;
        }

        // Get the street light properties matching the passed in CU
        public bool GetStreetLightProperties(string cu, ref string description, ref StreetLightProperties streetLightProperties)
        {
            bool returnValue = false;

            description = "";

            streetLightProperties.voltage = 0;
            streetLightProperties.regulation = 0;
            streetLightProperties.powerFactor = 0;
            streetLightProperties.maxAmps = 0;

            try
            {
                m_StreetLightRS.Filter = ConstantsDT.FIELD_CABLE_CU + " = '" + cu + "'";

                if (m_StreetLightRS.RecordCount > 0)
                {
                    description = m_StreetLightRS.Fields[ConstantsDT.FIELD_STREETLIGHT_DESC].Value.ToString();

                    if (!Convert.IsDBNull(m_StreetLightRS.Fields[ConstantsDT.FIELD_STREETLIGHT_VOLTAGE].Value))
                    {
                        streetLightProperties.voltage = Convert.ToDouble(m_StreetLightRS.Fields[ConstantsDT.FIELD_STREETLIGHT_VOLTAGE].Value);
                    }

                    if (!Convert.IsDBNull(m_StreetLightRS.Fields[ConstantsDT.FIELD_STREETLIGHT_REGULATION].Value))
                    {
                        streetLightProperties.regulation = Convert.ToDouble(m_StreetLightRS.Fields[ConstantsDT.FIELD_STREETLIGHT_REGULATION].Value);
                    }

                    if (!Convert.IsDBNull(m_StreetLightRS.Fields[ConstantsDT.FIELD_STREETLIGHT_POWER_FACTOR].Value))
                    {
                        streetLightProperties.powerFactor = Convert.ToDouble(m_StreetLightRS.Fields[ConstantsDT.FIELD_STREETLIGHT_POWER_FACTOR].Value);
                    }

                    if (!Convert.IsDBNull(m_StreetLightRS.Fields[ConstantsDT.FIELD_STREETLIGHT_MAX_AMPS].Value))
                    {
                        streetLightProperties.maxAmps = Convert.ToDouble(m_StreetLightRS.Fields[ConstantsDT.FIELD_STREETLIGHT_MAX_AMPS].Value);
                    }

                    returnValue = true;
                }
                else
                {
                    returnValue = false;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(m_Application.ApplicationWindow, ConstantsDT.ERROR_SL_STREETLIGHT_PROPERTIES + ": " + ex.Message, ConstantsDT.APPLICATION_NAME, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                returnValue = false;
            }

            return returnValue;
        }

        // Get the Transformer properties matching the passed in CU
        public bool GetTransformerProperties(string cu, ref string description, ref XfmrProperties xfmrProperties)
        {
            bool returnValue = false;

            try
            {
                string rsFilter = ConstantsDT.FIELD_XFMR_CU + " = '" + cu + "'";
                
                m_XfmrRS.Filter = rsFilter;

                if (m_XfmrRS.RecordCount > 0)
                {
                    description = m_XfmrRS.Fields[ConstantsDT.FIELD_XFMR_DESCRIPTION].Value.ToString();

                    if (!Convert.IsDBNull(m_XfmrRS.Fields[ConstantsDT.FIELD_XFMR_XFMR_SIZE].Value))
                    {
                        xfmrProperties.Size = Convert.ToDouble(m_XfmrRS.Fields[ConstantsDT.FIELD_XFMR_XFMR_SIZE].Value);
                    }

                    xfmrProperties.Type = m_XfmrRS.Fields[ConstantsDT.FIELD_XFMR_TYPE].Value.ToString();
                    xfmrProperties.Orientation = m_XfmrRS.Fields[ConstantsDT.FIELD_XFMR_OH_UG].Value.ToString();

                    if (!Convert.IsDBNull(m_XfmrRS.Fields[ConstantsDT.FIELD_XFMR_RESISTANCE].Value))
                    {
                        xfmrProperties.Resistance = Convert.ToDouble(m_XfmrRS.Fields[ConstantsDT.FIELD_XFMR_RESISTANCE].Value);
                    }

                    if (!Convert.IsDBNull(m_XfmrRS.Fields[ConstantsDT.FIELD_XFMR_REACTANCE].Value))
                    {
                        xfmrProperties.Reactance = Convert.ToDouble(m_XfmrRS.Fields[ConstantsDT.FIELD_XFMR_REACTANCE].Value);
                    }

                    if (xfmrProperties.Resistance != 0)
                    {
                        xfmrProperties.XfmrXoverR = xfmrProperties.Reactance / xfmrProperties.Resistance;
                    }

                    if (!Convert.IsDBNull(m_XfmrRS.Fields[ConstantsDT.FIELD_XFMR_IMPEDANCE].Value))
                    {
                        xfmrProperties.Impedance = Convert.ToDouble(m_XfmrRS.Fields[ConstantsDT.FIELD_XFMR_IMPEDANCE].Value);
                    }

                    xfmrProperties.Voltage = m_XfmrRS.Fields[ConstantsDT.FIELD_XFMR_VOLT_VOLTAGE].Value.ToString();

                    if (!Convert.IsDBNull(m_XfmrRS.Fields[ConstantsDT.FIELD_XFMR_VOLT_VS].Value))
                    {
                        xfmrProperties.SecondaryVoltage = Convert.ToDouble(m_XfmrRS.Fields[ConstantsDT.FIELD_XFMR_VOLT_VS].Value);
                    }

                    if (!Convert.IsDBNull(m_XfmrRS.Fields[ConstantsDT.FIELD_XFMR_VOLT_VF].Value))
                    {
                        xfmrProperties.VoltageFactor = Convert.ToDouble(m_XfmrRS.Fields[ConstantsDT.FIELD_XFMR_VOLT_VF].Value);
                    }

                    if (!Convert.IsDBNull(m_XfmrRS.Fields[ConstantsDT.FIELD_XFMR_TYPE_FACTOR].Value))
                    {
                        xfmrProperties.TypeFactor = Convert.ToDouble(m_XfmrRS.Fields[ConstantsDT.FIELD_XFMR_TYPE_FACTOR].Value);
                    }

                    if (!Convert.IsDBNull(m_XfmrRS.Fields[ConstantsDT.FIELD_XFMR_VOLT_V3PH].Value))
                    {
                        xfmrProperties.Voltage3Phase = Convert.ToDouble(m_XfmrRS.Fields[ConstantsDT.FIELD_XFMR_VOLT_V3PH].Value);
                    }                    
                }
                returnValue = true;
            }
            catch (Exception ex)
            {
                returnValue = false;
                MessageBox.Show(m_Application.ApplicationWindow, ConstantsDT.ERROR_SC_XFMR_PROPERTIES + ": " + ex.Message, ConstantsDT.APPLICATION_NAME, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }

            return returnValue;
        }

        // Validate and process the trace results. 
        public bool ProcessTraceResults(string traceName)
        {
            bool returnValue = false;

            try
            {
                m_TraceResultsRS = m_Application.DataContext.OpenRecordset(ConstantsDT.QUERY_TRACE_RESULTS, CursorTypeEnum.adOpenDynamic, LockTypeEnum.adLockOptimistic, (int)CommandTypeEnum.adCmdText, traceName);

                if (m_TraceResultsRS.RecordCount > 0)
                {
                    // Get the phase count
                    m_TraceResultsRS.MoveFirst();
                    if (m_TraceResultsRS.Fields["phase"].Value.ToString().Length > 0)
                    {
                        m_PhaseCount = Convert.ToInt32(m_TraceResultsRS.Fields["phase"].Value.ToString().Length);
                    }
                    else
                    {
                        m_PhaseCount = 0;
                    }

                    // If the trace results contains multiple Transformers, then the command will exit.
                    m_TraceResultsRS.Filter = "g3e_fno = " + ConstantsDT.FNO_OH_XFMR + " or g3e_fno = " + ConstantsDT.FNO_UG_XFMR;
                    if (m_TraceResultsRS.RecordCount > 1)
                    {
                        MessageBox.Show(m_Application.ApplicationWindow, ConstantsDT.ERROR_SC_MULTIPLE_XFMRS_IN_TRACE, ConstantsDT.APPLICATION_NAME, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        return false;
                    }

                    // If a 3 phase Transformer is feeding a single phase Service Point, then the command will exit. 
                    if (m_PhaseCount == 3)
                    {
                        m_TraceResultsRS.Filter = "g3e_fno = " + ConstantsDT.FNO_SRVCPT;
                        if (m_TraceResultsRS.RecordCount > 1)
                        {
                            while (!m_TraceResultsRS.EOF)
                            {
                                if (m_TraceResultsRS.Fields["phase"].Value.ToString().Length < 2)
                                {
                                    MessageBox.Show(m_Application.ApplicationWindow, ConstantsDT.ERROR_SC_INVALID_PREMISE_PHASE, ConstantsDT.APPLICATION_NAME, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                                    return false;
                                }
                                m_TraceResultsRS.MoveNext();
                            }
                        }
                    }

                    m_TraceResultsRS.Filter = "";

                    short fno = 0;

                    while (!m_TraceResultsRS.EOF)
                    {
                        fno = Convert.ToInt16(m_TraceResultsRS.Fields["G3E_FNO"].Value);

                        switch (fno)
                        {
                            case ConstantsDT.FNO_OH_SECCOND:
                            case ConstantsDT.FNO_UG_SECCOND:
                                if (!ProcessConductor())
                                {
                                    return false;
                                }
                                break;
                            case ConstantsDT.FNO_SRVCCOND:
                                if (!ProcessService())
                                {
                                    return false;
                                }
                                break;
                            case ConstantsDT.FNO_SRVCPT:
                                if (!ProcessServicePoint())
                                {
                                    return false;
                                }
                                break;
                            case ConstantsDT.FNO_STREETLIGHT:
                                if (!ProcessStreetLight())
                                {
                                    return false;
                                }
                                break;
                            default:
                                break;
                        }

                        m_TraceResultsRS.MoveNext();
                    }

                    returnValue = true;
                }
                else
                {
                    returnValue = false;
                    MessageBox.Show(m_Application.ApplicationWindow, ConstantsDT.ERROR_NO_TRACE_RECORDS, ConstantsDT.APPLICATION_NAME, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }
            }
            catch (Exception ex)
            {
                returnValue = false;
                MessageBox.Show(m_Application.ApplicationWindow, ConstantsDT.ERROR_PROCESS_TRACE_RESULTS + ": " + ex.Message, ConstantsDT.APPLICATION_NAME, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }

            return returnValue;
        }

        // Create a Conductor instance for the Secondary Conductor from the trace results.
        private bool ProcessConductor()
        {            
            bool returnValue = false;

            try
            {
                short fno = Convert.ToInt16(m_TraceResultsRS.Fields["G3E_FNO"].Value);
                int fid = Convert.ToInt32(m_TraceResultsRS.Fields["G3E_FID"].Value);
                int sourceFID = Convert.ToInt32(m_TraceResultsRS.Fields["G3E_SOURCEFID"].Value);

                IGTKeyObject gtkey = m_Application.DataContext.OpenFeature(fno, fid);

                // Get the CU value from the CU component recordset
                Recordset componentRS = gtkey.Components.GetComponent(ConstantsDT.CNO_COMPUNIT).Recordset;

                string cableCU = "";
                if (componentRS.RecordCount > 0)
                {
                    cableCU = componentRS.Fields[ConstantsDT.FIELD_COMPUNIT_CU].Value.ToString();
                }

                // Get the cable properties
                CableProperties cableProperties = new CableProperties();
                string cableType = "";
                if (cableCU.Length > 0)
                {
                    GetCableProperties(cableCU, ref cableType, ref cableProperties);
                }

                // Get the user defined length of the conductor
                int cableLength = 0;
                if (!Convert.IsDBNull(m_TraceResultsRS.Fields["length"].Value))
                {
                    cableLength = Convert.ToInt16(m_TraceResultsRS.Fields["length"].Value);
                }

                Conductor conductor = new Conductor();

                conductor.ConductorFID = fid;
                conductor.FNO = fno;
                conductor.SourceFID = sourceFID;
                conductor.SpanIndex = m_SpanIndex++;
                conductor.CableType = cableType;
                conductor.CU = cableCU;
                conductor.Length = cableLength;
                conductor.CableProperties = cableProperties;
                conductor.Resistance = cableProperties.Resistance * cableLength;
                conductor.Reactance = cableProperties.Reactance * cableLength;


                if (AllowCUEdit(gtkey))
                {
                    conductor.AllowCUEdit = true;
                }
                else
                {
                    conductor.AllowCUEdit = false;
                }

                if (m_ConductorsDictionary == null)
                {
                    m_ConductorsDictionary = new Dictionary<int, Conductor>();
                }

                m_ConductorsDictionary.Add(fid, conductor);

                returnValue = true;
            }
            catch (Exception ex)
            {
                returnValue = false;
                MessageBox.Show(m_Application.ApplicationWindow, ConstantsDT.ERROR_SC_PROCESSING_CONDUCTOR + ": " + ex.Message, ConstantsDT.APPLICATION_NAME, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }

            return returnValue;
        }

        // Create a Service instance for the Service Line from the trace results.
        private bool ProcessService()
        {
            bool returnValue = false;

            try
            {
                short fno = Convert.ToInt16(m_TraceResultsRS.Fields["G3E_FNO"].Value);
                int fid = Convert.ToInt32(m_TraceResultsRS.Fields["G3E_FID"].Value);
                int sourceFID = Convert.ToInt32(m_TraceResultsRS.Fields["G3E_SOURCEFID"].Value);

                IGTKeyObject gtkey = m_Application.DataContext.OpenFeature(fno, fid);

                // Get the CU value from the CU component recordset
                Recordset componentRS = gtkey.Components.GetComponent(ConstantsDT.CNO_COMPUNIT).Recordset;

                string cableCU = "";
                if (componentRS.RecordCount > 0)
                {
                    cableCU = componentRS.Fields[ConstantsDT.FIELD_COMPUNIT_CU].Value.ToString();
                }

                // Get the cable properties
                CableProperties cableProperties = new CableProperties();
                string cableType = "";
                if (cableCU.Length > 0)
                {
                    GetCableProperties(cableCU, ref cableType, ref cableProperties);
                }

                // Get the user defined length of the conductor
                int cableLength = 0;
                if (!Convert.IsDBNull(m_TraceResultsRS.Fields["length"].Value))
                {
                    cableLength = Convert.ToInt16(m_TraceResultsRS.Fields["length"].Value);
                }

                Service service = new Service();

                service.ServiceFID = fid;
                service.SourceFID = sourceFID;
                service.CableType = cableType;
                service.CU = cableCU;
                service.Length = cableLength;
                service.CableProperties = cableProperties;
                service.Resistance = cableProperties.Resistance * cableLength;
                service.Reactance = cableProperties.Reactance * cableLength;

                if (AllowCUEdit(gtkey))
                {
                    service.AllowCUEdit = true;
                }
                else
                {
                    service.AllowCUEdit = false;
                }

                Conductor conductor;

                if (m_ConductorsDictionary.TryGetValue(sourceFID, out conductor))
                {
                    service.SpanIndex = conductor.SpanIndex;
                }

                if (m_ServicesDictionary == null)
                {
                    m_ServicesDictionary = new Dictionary<int, Service>();
                }

                m_ServicesDictionary.Add(fid, service);

                // Add the clsService to the feeding clsConductor
                if (m_ConductorsDictionary.Count > 0)
                {
                    if (m_ConductorsDictionary.TryGetValue(sourceFID, out conductor))
                    {
                        conductor.AddService(fid, service);
                    }
                }

                returnValue = true;
            }
            catch (Exception ex)
            {
                returnValue = false;
                MessageBox.Show(m_Application.ApplicationWindow, ConstantsDT.ERROR_SC_PROCESSING_SERVICE + ": " + ex.Message, ConstantsDT.APPLICATION_NAME, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }

            return returnValue;
        }

        // Create a ServicePoint instance for the Service Point from the trace results.
        private bool ProcessServicePoint()
        {
            bool returnValue = false;

            try
            {
                int srvcPtFID = Convert.ToInt32(m_TraceResultsRS.Fields["G3E_FID"].Value);
                int sourceFID = Convert.ToInt32(m_TraceResultsRS.Fields["G3E_SOURCEFID"].Value);

                IGTKeyObject gtkey = m_Application.DataContext.OpenFeature(ConstantsDT.FNO_SRVCPT, srvcPtFID);

                // Get the load values from the Service Point attributes component recordset
                Recordset componentRS = gtkey.Components.GetComponent(ConstantsDT.CNO_SRVCPT_ATTR).Recordset;

                double summerLoadActual = 0;
                double summerLoadEstimated = 0;
                double winterLoadActual = 0;
                double winterLoadEstimated = 0;

                if (componentRS.RecordCount > 0)
                {
                    if (!Convert.IsDBNull(componentRS.Fields[ConstantsDT.FIELD_SRVCPT_LOAD_SUMMER_ACT].Value))
                    {
                        summerLoadActual = Convert.ToDouble(componentRS.Fields[ConstantsDT.FIELD_SRVCPT_LOAD_SUMMER_ACT].Value);
                    }

                    if (!Convert.IsDBNull(componentRS.Fields[ConstantsDT.FIELD_SRVCPT_LOAD_SUMMER_EST].Value))
                    {
                        summerLoadEstimated = Convert.ToDouble(componentRS.Fields[ConstantsDT.FIELD_SRVCPT_LOAD_SUMMER_EST].Value);
                    }

                    if (!Convert.IsDBNull(componentRS.Fields[ConstantsDT.FIELD_SRVCPT_LOAD_WINTER_ACT].Value))
                    {
                        winterLoadActual = Convert.ToDouble(componentRS.Fields[ConstantsDT.FIELD_SRVCPT_LOAD_WINTER_ACT].Value);
                    }

                    if (!Convert.IsDBNull(componentRS.Fields[ConstantsDT.FIELD_SRVCPT_LOAD_WINTER_EST].Value))
                    {
                        winterLoadEstimated = Convert.ToDouble(componentRS.Fields[ConstantsDT.FIELD_SRVCPT_LOAD_WINTER_EST].Value);
                    }
                }

                // Consider each Service Point as 1 customer
                m_XfmrCustomerCount += 1;

                ServicePoint servicePoint = new ServicePoint();

                servicePoint.ServicePointFID = srvcPtFID;
                servicePoint.SourceFID = sourceFID;

                servicePoint.LoadSummerActual = summerLoadActual;
                servicePoint.LoadSummerEstimated = summerLoadEstimated;
                servicePoint.LoadWinterActual = winterLoadActual;
                servicePoint.LoadWinterEstimated = winterLoadEstimated;
                servicePoint.LockedRotorAmps = m_LockedRotorAmps;
                servicePoint.CustomerCount = 1;

                if (!CalculateServicePointImpedance(servicePoint))
                {
                    return false;
                }

                // Add the clsServicePoint to the feeding clsService
                if (m_ServicesDictionary != null)
                {
                    Service service;

                    if (m_ServicesDictionary.TryGetValue(sourceFID, out service))
                    {
                        service.ServicePoint = servicePoint;
                        service.CustomerCount = 1;
                    }
                }

                returnValue = true;
            }
            catch (Exception ex)
            {
                returnValue = false;
                MessageBox.Show(m_Application.ApplicationWindow, ConstantsDT.ERROR_SC_PROCESSING_SERVICE_PT + ": " + ex.Message, ConstantsDT.APPLICATION_NAME, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }

            return returnValue;
        }

        // Create a Street Light instance for the Street Light from the trace results.
        private bool ProcessStreetLight()
        {
            bool returnValue = false;

            try
            {
                short fno = Convert.ToInt16(m_TraceResultsRS.Fields["G3E_FNO"].Value);
                int fid = Convert.ToInt32(m_TraceResultsRS.Fields["G3E_FID"].Value);
                int sourceFID = Convert.ToInt32(m_TraceResultsRS.Fields["G3E_SOURCEFID"].Value);

                IGTKeyObject gtkey = m_Application.DataContext.OpenFeature(fno, fid);

                // Get the CU value from the CU component recordset
                Recordset componentRS = gtkey.Components.GetComponent(ConstantsDT.CNO_COMPUNIT).Recordset;
                string streetLightCU = "";
                if (componentRS.RecordCount > 0)
                {
                    streetLightCU = componentRS.Fields[ConstantsDT.FIELD_COMPUNIT_CU].Value.ToString();
                }

                // Get the cable properties
                StreetLightProperties StreetLightProperties = new StreetLightProperties();
                string streetLightType = "";
                if (streetLightCU.Length > 0)
                {                    
                    GetStreetLightProperties(streetLightCU, ref streetLightType, ref StreetLightProperties);
                }

                StreetLight streetLight = new StreetLight();

                streetLight.FID = fid;
                streetLight.SourceFID = sourceFID;
                streetLight.LightType = streetLightType;
                streetLight.CU = streetLightCU;
                streetLight.StreetLightProperties = StreetLightProperties;

                if (AllowCUEdit(gtkey))
                {
                    streetLight.AllowCUEdit = true;
                }
                else
                {
                    streetLight.AllowCUEdit = false;
                }

                if (m_StreetLightsDictionary == null)
                {
                    m_StreetLightsDictionary = new Dictionary<int, StreetLight>();
                }

                m_StreetLightsDictionary.Add(fid, streetLight);

                // Add the Street Light to the feeding Conductor                
                if (m_ConductorsDictionary != null)
                {
                    Conductor conductor;
                    if (m_ConductorsDictionary.TryGetValue(sourceFID, out conductor))
                    {
                        streetLight.SpanIndexTo = conductor.SpanIndex + 1;
                        conductor.StreetLight = streetLight;
                    }
                    if (conductor != null)
                    {
                        Conductor upstreamConductor;
                        if (m_ConductorsDictionary.TryGetValue(conductor.SourceFID, out upstreamConductor))
                        {
                            streetLight.SpanIndexFrom = upstreamConductor.SpanIndex + 1;
                            conductor.StreetLight = streetLight;
                        }
                        else
                        {
                            streetLight.SpanIndexFrom = 1;
                            conductor.StreetLight = streetLight;
                        }
                    }                    
                }

                returnValue = true;
            }
            catch (Exception ex)
            {
                returnValue = false;
                MessageBox.Show(m_Application.ApplicationWindow, ConstantsDT.ERROR_SL_PROCESSING_STREETLIGHT + ": " + ex.Message, ConstantsDT.APPLICATION_NAME, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }

            return returnValue;
        }

        // Calculate impedance
        private bool CalculateImpedance(double resistance, double reactance, ref double impedance)
        {
            bool returnValue = false;

            try
            {
                impedance = Math.Sqrt(Math.Pow(resistance, 2) + Math.Pow(reactance, 2));
                returnValue = true;
            }
            catch (Exception ex)
            {
                returnValue = false;
                MessageBox.Show(m_Application.ApplicationWindow, ConstantsDT.ERROR_SC_CALCULATE_Z + ": " + ex.Message, ConstantsDT.APPLICATION_NAME, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }

            return returnValue;
        }

        // Calculate the imedance for the passed in Service Point
        public bool CalculateServicePointImpedance(ServicePoint servicePoint)
        {            
            bool returnValue = false;

            try
            {
                if (m_LockedRotorAmps != 0)
                {
                    double resistance = ((ConstantsDT.BASE_VA / 2) / (ConstantsDT.BASE_V * m_LockedRotorAmps)) * m_SummerPowerFactor;
                    double reactance = ((ConstantsDT.BASE_VA / 2) / (ConstantsDT.BASE_V * m_LockedRotorAmps)) * Math.Sin(Math.Acos(m_SummerPowerFactor));
                    servicePoint.Resistance = resistance;
                    servicePoint.Reactance = reactance;

                    double impedance = 0;
                    if (!CalculateImpedance(resistance, reactance, ref impedance))
                    {
                        return false;
                    }
                    servicePoint.Impedance = impedance;
                }                

                returnValue = true;
            }
            catch (Exception ex)
            {
                returnValue = false;
                MessageBox.Show(m_Application.ApplicationWindow, ConstantsDT.ERROR_SC_CALCULATE_Z_SERVICE_PT + ": " + ex.Message, ConstantsDT.APPLICATION_NAME, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }

            return returnValue;
        }

        public bool UpdateMapWindow(short fno, int fid)
        {
            bool returnValue = false;

            try
            {
                OptionsDT frmDesignToolOptions = new OptionsDT();

                if (frmDesignToolOptions.OverrideMapWindowSettings)
                {                    
                    if (frmDesignToolOptions.ZoomOption == 1)
                    {
                        m_Application.ActiveMapWindow.SelectBehavior = GTSelectBehaviorConstants.gtmwsbHighlightOnly; ;
                    }
                    else if (frmDesignToolOptions.ZoomOption == 2)
                    {
                        m_Application.ActiveMapWindow.SelectBehavior = GTSelectBehaviorConstants.gtmwsbHighlightAndCenter;
                    }
                    else if (frmDesignToolOptions.ZoomOption == 3)
                    {
                        m_Application.ActiveMapWindow.SelectBehavior = GTSelectBehaviorConstants.gtmwsbHighlightAndFit;
                        m_Application.ActiveMapWindow.SelectBehaviorZoomFactor = frmDesignToolOptions.ZoomFactor;                    }

                    m_Application.SelectedObjects.Add(GTSelectModeConstants.gtsosmSelectedComponentsOnly, m_Application.DataContext.GetDDCKeyObjects(fno, fid, GTComponentGeometryConstants.gtddcgAllGeographic)[0]);
                }

                frmDesignToolOptions = null;

                returnValue = true;
            }
            catch/* (Exception ex)*/
            {
                returnValue = false;
                //MessageBox.Show(m_Application.ApplicationWindow, ConstantsDT.? + ": " + ex.Message, ConstantsDT.APPLICATION_NAME, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }

            return returnValue;
        }

        public bool DisplayResults(Recordset displayRS, string displayPathName, string displayName, DisplayResultsType displayType)
        {
            IGTMapWindow gtMapWindow = default(IGTMapWindow);
            IGTMapWindows gtMapWindows = default(IGTMapWindows);
            IGTDisplayService gtDisplayService = default(IGTDisplayService);

            bool returnValue = false;

            try
            {
                OptionsDT frmDesignToolOptions = new OptionsDT();
                
                // Remove existing item from the Display Control window
                try
                {
                    if (frmDesignToolOptions.ActiveWindowOnly)
                    {
                        gtMapWindow = m_Application.ActiveMapWindow;
                        gtMapWindow.DisplayService.Remove(displayPathName, displayName);
                    }
                    else
                    {
                        gtMapWindows = m_Application.GetMapWindows(GTMapWindowTypeConstants.gtapmtGeographic);

                        foreach (IGTMapWindow mapWindow in gtMapWindows)
                        {
                            mapWindow.DisplayService.Remove(displayPathName, displayName);
                        }
                    }
                }
                catch
                {
                    // Ignore error if item is not on the legend
                }

                if (frmDesignToolOptions.OutputToMapWindow)
                {

                    if (displayRS.RecordCount > 0)
                    {
                        if (frmDesignToolOptions.ActiveWindowOnly)
                        {
                            gtMapWindow = m_Application.ActiveMapWindow;
                            gtDisplayService = gtMapWindow.DisplayService;

                            if (frmDesignToolOptions.OverrideStyle)
                            {
                                IGTSymbology gtsymbology = GTClassFactory.Create<IGTSymbology>();
                                if (displayType == DisplayResultsType.Trace)
                                {
                                    gtsymbology.Color = frmDesignToolOptions.TraceColor;                                    
                                }
                                else
                                {
                                    gtsymbology.Color = frmDesignToolOptions.ProblemColor;
                                }
                                gtsymbology.FillColor = frmDesignToolOptions.FillColor;
                                gtsymbology.Width = frmDesignToolOptions.LineWeight;

                                gtDisplayService.AppendQuery(displayPathName, displayName, displayRS, gtsymbology, true);
                            }
                            else
                            {
                                gtDisplayService.AppendQuery(displayPathName, displayName, displayRS);
                            }
                        }
                        else
                        {
                            gtMapWindows = m_Application.GetMapWindows(GTMapWindowTypeConstants.gtapmtAll);
                            foreach (IGTMapWindow mapWindow in gtMapWindows)
                            {
                                gtDisplayService = mapWindow.DisplayService;

                                if (frmDesignToolOptions.OverrideStyle)
                                {
                                    IGTSymbology gtsymbology = GTClassFactory.Create<IGTSymbology>();
                                    if (displayType == DisplayResultsType.Trace)
                                    {
                                        gtsymbology.Color = frmDesignToolOptions.TraceColor;
                                    }
                                    else
                                    {
                                        gtsymbology.Color = frmDesignToolOptions.ProblemColor;
                                    }
                                    gtsymbology.FillColor = frmDesignToolOptions.FillColor;
                                    gtsymbology.Width = frmDesignToolOptions.LineWeight;

                                    gtDisplayService.AppendQuery(displayPathName, displayName, displayRS, gtsymbology, true);
                                }
                                else
                                {
                                    gtDisplayService.AppendQuery(displayPathName, displayName, displayRS);
                                }
                            }
                        }
                    }                    
                }

                m_Application.RefreshWindows();

                returnValue = true;
            }
            catch
            {
                returnValue = false;
            }
            finally
            {
                gtMapWindow = null;
                gtMapWindows = null;
                gtDisplayService = null;
            }

            return returnValue;
        }

        public bool RemoveLegendItem(string displayPathName, string displayName)
        {
            IGTMapWindow gtMapWindow = default(IGTMapWindow);
            IGTMapWindows gtMapWindows = default(IGTMapWindows);

            bool returnValue = false;

            OptionsDT frmDesignToolOptions = new OptionsDT();

            // Remove existing item from the Display Control window
            try
            {
                if (frmDesignToolOptions.ActiveWindowOnly)
                {
                    gtMapWindow = m_Application.ActiveMapWindow;
                    gtMapWindow.DisplayService.Remove(displayPathName, displayName);
                }
                else
                {
                    gtMapWindows = m_Application.GetMapWindows(GTMapWindowTypeConstants.gtapmtGeographic);

                    foreach (IGTMapWindow mapWindow in gtMapWindows)
                    {
                        mapWindow.DisplayService.Remove(displayPathName, displayName);
                    }
                }
                returnValue = true;
            }
            catch
            {
                // Ignore error if item is not on the legend
                returnValue = true;
            }
            finally
            {
                gtMapWindow = null;
                gtMapWindows = null;
            }

            return returnValue;
        }

        // Check if the property exists in the Properties collection
        // This is a place to store data that is needed as long as the session is active.
        public bool CheckIfPropertyExists(string propertyName, out object propertyValue)
        {
            bool returnValue = false;

            propertyValue = null;

            try
            {
                propertyValue = m_Application.Properties[propertyName];
                returnValue = true;
            }
            catch
            {
                returnValue = false;
            }

            return returnValue;
        }

        // Add property to the Properties collection
        // This is a place to store data that is needed as long as the session is active.
        public bool AddProperty(string propertyName, object propertyValue)
        {
            bool returnValue = false;

            try
            {
                m_Application.Properties.Remove(propertyName);
            }
            catch
            {
                // ignore error
            }

            try
            {
                m_Application.Properties.Add(propertyName, propertyValue);
                returnValue = true;
            }
            catch
            {
                returnValue = false;
            }

            return returnValue;
        }

        /// <summary>
        /// Returns a recordset containing the posted edits from the asset history for the WR and also the pending edits in the WR.
        /// </summary>
        /// <param name="wrRS">Recordset containing the edits for the WR. The edits include the posted edits from the Asset History table and the pending edits</param>
        /// <returns>Boolean indicating status</returns>
        public bool GetWrData(ref ADODB.Recordset wrRS)
        {
            bool returnValue = false;

            try
            {
                // Get posted edits from the Asset History table.
                string sql = "select g3e_fno, g3e_fid from asset_history where g3e_identifier = ?";

                wrRS = m_Application.DataContext.OpenRecordset(sql, CursorTypeEnum.adOpenDynamic, LockTypeEnum.adLockOptimistic, (int)CommandTypeEnum.adCmdText, m_Application.DataContext.ActiveJob);

                IGTJobHelper gtJobHelper = GTClassFactory.Create<IGTJobHelper>();
                gtJobHelper.DataContext = m_Application.DataContext;

                // Get pending edits.
                ADODB.Recordset pendingEditsRS = gtJobHelper.FindPendingEdits();

                // Merge edits into one recordset.
                if (wrRS.RecordCount > 0)
                {
                    if (pendingEditsRS != null)
                    {
                        if (pendingEditsRS.RecordCount > 0)
                        {
                            // Add pending edit records
                            while (!pendingEditsRS.EOF)
                            {
                                wrRS.AddNew();
                                wrRS.Fields["G3E_FNO"].Value = pendingEditsRS.Fields["G3E_FNO"].Value;
                                wrRS.Fields["G3E_FID"].Value = pendingEditsRS.Fields["G3E_FID"].Value;

                                pendingEditsRS.MoveNext();
                            }
                        }
                    }                    
                }
                else
                {
                    // No edits in asset history. Return pending edits.
                    if (pendingEditsRS != null)
                    {
                        wrRS = pendingEditsRS;
                    }
                }

                gtJobHelper = null;
                returnValue = true;
            }
            catch (Exception ex)
            {               
                MessageBox.Show(m_Application.ApplicationWindow, "Error in GetWrData: " + ex.Message,
                                ConstantsDT.APPLICATION_NAME, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                
                //WriteToCommandLog("ERROR", ex.Message, "commonUpdateTrace.GetWrData");
                returnValue = false;
            }

            return returnValue;
        }

        /// <summary>
        /// Checks if CU can be edited for passed in feature
        /// </summary>
        /// <param name="gtKO">IGTKeyObject of feature to check</param>
        /// <returns>Boolean indicating if CU can be edited</returns>
        public static bool AllowCUEdit(IGTKeyObject gtKO)
        {
            bool returnValue = false;

            try
            {
                IGTApplication gApp = GTClassFactory.Create<IGTApplication>();
                Recordset compUnitRS = gtKO.Components.GetComponent(ConstantsDT.CNO_COMPUNIT).Recordset;
                if (compUnitRS.RecordCount > 0)
                {
                    compUnitRS.MoveFirst();
                    string cu = compUnitRS.Fields[ConstantsDT.FIELD_COMPUNIT_CU].Value.ToString();
                    string activity = compUnitRS.Fields[ConstantsDT.FIELD_COMPUNIT_ACTIVITY].Value.ToString();
                    string installedWR = compUnitRS.Fields[ConstantsDT.FIELD_COMPUNIT_INSTALL_WR].Value.ToString();
                    short fno = Convert.ToInt16(compUnitRS.Fields["G3E_FNO"].Value);

                    if (cu.Length == 0)
                    {
                        returnValue = true;
                    }
                    //Fixed ALM-1853 : To allow Activity "IA" to Service Line Feature
                    else if (activity == "I" || activity == "IC" || (fno == ConstantsDT.FNO_SRVCCOND && activity == "IA"))
                    {
                        if (installedWR == gApp.DataContext.ActiveJob)
                        {
                            returnValue = true;
                        }
                        else
                        {
                            returnValue = false;
                        }
                    }
                    else
                    {
                        returnValue = false;
                    }
                }
            }
            catch
            {
                returnValue = false;
            }

            return returnValue;
        }

        /// <summary>
        /// Calls logger class to log message to COMMAND_LOG table.
        /// </summary>
        /// <param name="logType">The type of message to log - INFO, ERROR, ...</param>
        /// <param name="logMessage">The message to log</param>
        /// <param name="logContext">The context for the message</param>
        private void WriteToCommandLog(string logType, string logMessage, string logContext)
        {            
            //gtCommandLogger.gtCommandLogger gtCommandLogger = new gtCommandLogger.gtCommandLogger();
            //gtCommandLogger.CommandNum = m_CommandNumber;
            //gtCommandLogger.CommandName = m_CommandName;
            //gtCommandLogger.LogType = logType;
            //gtCommandLogger.LogMsg = logMessage;
            //gtCommandLogger.LogContext = logContext;
            //gtCommandLogger.isInteractive = true;
            //gtCommandLogger.logEntry();

            //gtCommandLogger = null;
        }
    }
}
