namespace GTechnology.Oncor.CustomAPI
{
    public class ConstantsDT
    {
        public const int BASE_VA = 20000000;
        public const int BASE_V = 230;

        public const string APPLICATION_NAME = "G/Technology";
        public const string COMMAND_NAME_SECONDARY_CALCULATOR = "Secondary Calculator";
        public const string COMMAND_NAME_CABLE_PULL_TENSION = "Cable Pull Tension";
        public const string COMMAND_NAME_GUYING = "Guying";
        public const string COMMAND_NAME_STREET_LIGHT_VOLTAGE_DROP = "Street Light Voltage Drop";
        public const string COMMAND_NAME_SAG_CLEARANCE = "Sag Clearance";
        public const string COMMAND_NAME_HANDHOLE_CIAC_CALCULATOR = "Handhole CIAC Calculator";
        public const string COMMAND_NAME_SRVC_ENCLOSURE_COST_CALCULATOR = "Service Enclosure Cost Calculator";


        public const string PROP_DT_EMBEDDED_FORM_HEIGHT = "_DesignToolEmbeddedForm_Height";
        public const string PROP_DT_EMBEDDED_FORM_WIDTH = "_DesignToolEmbeddedForm_Width";

        // Feature Numbers
        public const short FNO_OH_COND = 8;
        public const short FNO_UG_COND = 9;
        public const short FNO_OH_XFMR = 59;
        public const short FNO_UG_XFMR = 60;
        public const short FNO_OH_SECCOND = 53;
        public const short FNO_UG_SECCOND = 63;
        public const short FNO_SRVCCOND = 54;
        public const short FNO_SRVCPT = 55;
        public const short FNO_STREETLIGHT = 56;
        public const short FNO_SECONDARY_BOX = 113;
        public const short FNO_SECONDARY_ENCLOSURE = 120;
        public const short FNO_DUCTBANK = 2200;
        public const short FNO_POLE = 110;
        public const short FNO_DESIGN_AREA = 8100;

        // Component Numbers
        public const short CNO_COMMON = 1;
        public const short CNO_CONNECTIVITY = 11;
        public const short CNO_COMPUNIT = 21;
        public const short CNO_OH_XFMR_UNIT = 5902;
        public const short CNO_OH_XFMR_BANK = 5901;
        public const short CNO_UG_XFMR_UNIT = 6002;
        public const short CNO_SRVCPT_ATTR = 5501;
        public const short CNO_PREMISE_ATTR = 5504;
        public const short CNO_OH_SECCOND_PRIMARYGRAPHIC = 5303;
        public const short CNO_OH_SRVCCOND_PRIMARYGRAPHIC = 5402;
        public const short CNO_OH_STREETLIGHT_PRIMARYGRAPHIC = 5602;

        // Validation results
        public const string VALIDATION_FAIL = "FAIL";
        public const string VALIDATION_PASS = "PASS";
        public const string VALIDATION_PRIORITY = "P2";

        // Database objects
        // Tables
        public const string TABLE_CPT_CABLE = "TOOLS_CPT_CABLE";
        public const string TABLE_CABLECONFIG = "TOOLS_CPT_CABLECONFIGURATION";
        public const string TABLE_DUCTTRENCH = "TOOLS_CPT_DUCTTRENCH";
        public const string TABLE_CPT_VALIDATION = "TOOLS_CPT_VALIDATION";
        public const string TABLE_DERATING = "TOOLS_SC_DERATING";
        public const string TABLE_METER = "TOOLS_SC_METER";
        public const string TABLE_CABLE = "TOOLS_SC_CABLE";
        public const string TABLE_XFMR = "TOOLS_SC_TRANSFORMER";
        public const string TABLE_STREET_LIGHT = "TOOLS_SL_BALLAST";
        public const string TABLE_XFMR_VOLTAGE = "TOOLS_SC_TRANSFORMER_VOLTAGE";
        public const string TABLE_VALIDATIONLOG = "TOOLS_VALIDATION";
        public const string TABLE_JOB = "G3E_JOB";
        public const string TABLE_XFMR_OH_BANK = "XFMR_OH_BANK_N";
        public const string TABLE_XFMR_UG_UNIT = "XFMR_UG_UNIT_N";

        // Columns
        public const string FIELD_CPT_CABLE_CINO = "CINO";
        public const string FIELD_CPT_CABLE_CONFIGURATION = "CONFIGURATION";
        public const string FIELD_CPT_CABLE_CU = "CU";
        public const string FIELD_CPT_CABLE_DESCRIPTION = "DESCRIPTION";
        public const string FIELD_CPT_CABLE_VOLTAGE = "VOLTAGE";
        public const string FIELD_CPT_CABLE_TYPE = "TYPE";
        public const string FIELD_CPT_CABLE_SIZE_MATERIAL = "SIZE_MATERIAL";
        public const string FIELD_CPT_CABLE_NUMBER_OF_PHASES = "NUMBER_OF_PHASES";
        public const string FIELD_CPT_CABLE_NUMBER_OF_CABLES = "NUMBER_OF_CABLES";
        public const string FIELD_CPT_CABLE_NON_STD = "NON_STD";
        public const string FIELD_CPT_CABLE_OUTSIDE_DIAMETER = "OUTSIDE_DIAMETER";
        public const string FIELD_CPT_CABLE_WEIGHT_PER_FOOT = "WEIGHT_PER_FOOT";
        public const string FIELD_CPT_CABLE_MAX_SWBP = "MAX_SWBP";
        public const string FIELD_CPT_CABLE_MAX_TENSION = "MAX_TENSION";
        public const string FIELD_CPT_CABLE_SWBP_CHANGE_POINT = "SWBP_CHANGE_POINT";
        public const string FIELD_CPT_CABLE_MAX_LENGTH = "MAX_LENGTH";

        public const string FIELD_CABLECONFIG_CONFIGURATION = "CONFIGURATION";
        public const string FIELD_CABLECONFIG_CCNO = "CCNO";

        public const string FIELD_DUCT_DTNO = "DTNO";
        public const string FIELD_DUCT_DESCRIPTION = "DESCRIPTION";
        public const string FIELD_DUCT_INSIDE_DIAMETER = "INSIDE_DIAMETER";
        public const string FIELD_DUCT_NOM_INSIDE_DIAMETER = "NOMINAL_INSIDE_DIAMETER";
        public const string FIELD_DUCT_STD_BEND_RADIUS = "STD_BEND_RADIUS";
        public const string FIELD_DUCT_MIN_CLEARANCE = "MIN_CLEARANCE";

        public const string FIELD_CPT_VALIDATION_CABLE_ID = "CABLE_ID";
        public const string FIELD_CPT_VALIDATION_DUCT_ID = "DUCTTRENCH_ID";
        public const string FIELD_CPT_VALIDATION_MAX_DISTANCE = "MAX_DISTANCE";

        public const string FIELD_JOB_IDENTIFIER = "G3E_IDENTIFIER";
        public const string FIELD_JOB_DESCRIPTION = "G3E_DESCRIPTION";
        public const string FIELD_JOB_TYPE = "G3E_JOBTYPE";

        public const string FIELD_CONNECTIVITY_PHASE = "PHASE_ALPHA";
        public const string FIELD_CONNECTIVITY_LENGTH_ACTUAL = "LENGTH_ACTUAL_Q";

        public const string FIELD_COMMON_LOCATION = "LOCATION";
        public const string FIELD_COMPUNIT_CU = "CU_C";
        public const string FIELD_COMPUNIT_ACTIVITY = "ACTIVITY_C";
        public const string FIELD_COMPUNIT_INSTALL_WR = "WR_ID";
        public const string FIELD_SRVCPT_LOAD_SUMMER_ACT = "LOAD_SUMMER_ACTUAL";
        public const string FIELD_SRVCPT_LOAD_SUMMER_EST = "LOAD_SUMMER_ESTIMATED";
        public const string FIELD_SRVCPT_LOAD_WINTER_ACT = "LOAD_WINTER_ACTUAL";
        public const string FIELD_SRVCPT_LOAD_WINTER_EST = "LOAD_WINTER_ESTIMATED";

        public const string FIELD_XFMRUNIT_SIZE = "KVA_Q";
        public const string FIELD_XFMRUNIT_VOLTAGE = "VOLT_PRI_Q";
        public const string FIELD_XFMRUNIT_VOLTAGE_SEC = "VOLT_SEC_Q";
        public const string FIELD_XFMRUNIT_PHASE = "PHASE_C";
        public const string FIELD_XFMRUNIT_PHASE_QUANTITY = "PHASE_Q";

        public const string FIELD_XFMR_TIE_XFMR_ID = "TIE_XFMR_ID";

        public const string FIELD_XFMR_TINO = "TINO";
        public const string FIELD_XFMR_CU = "CU";
        public const string FIELD_XFMR_DESCRIPTION = "DESCRIPTION";
        public const string FIELD_XFMR_XFMR_SIZE = "XFMR_SIZE";
        public const string FIELD_XFMR_TYPE = "TYPE";
        public const string FIELD_XFMR_VOLTAGE = "VOLTAGE";
        public const string FIELD_XFMR_OH_UG = "OH_UG";
        public const string FIELD_XFMR_TYPE_FACTOR = "TYPE_FACTOR";
        public const string FIELD_XFMR_IMPEDANCE = "IMPEDANCE";
        public const string FIELD_XFMR_RESISTANCE = "RESISTANCE";
        public const string FIELD_XFMR_REACTANCE = "REACTANCE";

        public const string FIELD_XFMR_VOLT_TINO = "TINO";
        public const string FIELD_XFMR_VOLT_VOLTAGE = "VOLTAGE";
        public const string FIELD_XFMR_VOLT_VS = "VOLTAGE_SECONDARY";
        public const string FIELD_XFMR_VOLT_VF = "VOLTAGE_FACTOR";
        public const string FIELD_XFMR_VOLT_V3PH = "VOLTAGE_3PH";

        public const string FIELD_CABLE_CINO = "CINO";
        public const string FIELD_CABLE_CU = "CU";
        public const string FIELD_CABLE_DESC = "DESCRIPTION";
        public const string FIELD_CABLE_DC_INCH = "DC_INCHES";
        public const string FIELD_CABLE_OD_INCH = "OD_INCHES";
        public const string FIELD_CABLE_RDC25 = "RDC25";
        public const string FIELD_CABLE_GMRF = "GMRF";
        public const string FIELD_CABLE_TC = "TC";
        public const string FIELD_CABLE_AMPACITY = "AMPACITY";
        public const string FIELD_CABLE_TCMIN = "TCMIN";
        public const string FIELD_CABLE_TZERO = "TZERO";
        public const string FIELD_CABLE_RDC = "RDC";
        public const string FIELD_CABLE_RACT = "RACT";
        public const string FIELD_CABLE_X = "X";
        public const string FIELD_CABLE_R_PER_FT = "RESISTANCE_PER_FT";
        public const string FIELD_CABLE_X_PER_FT = "REACTANCE_PER_FT";

        public const string FIELD_DERATING_VALUE = "DERATING_VALUE";
        public const string FIELD_NUMBER_OF_CABLES = "NUMBER_OF_CABLES";
        public const string FIELD_DERATING_XFMR_TYPE = "XFMR_TYPE";

        public const string FIELD_STREETLIGHT_BINO = "BINO";
        public const string FIELD_STREETLIGHT_CU = "CU";
        public const string FIELD_STREETLIGHT_DESC = "DESCRIPTION";
        public const string FIELD_STREETLIGHT_VOLTAGE = "VOLTAGE";
        public const string FIELD_STREETLIGHT_REGULATION = "REGULATION";
        public const string FIELD_STREETLIGHT_POWER_FACTOR = "POWER_FACTOR";
        public const string FIELD_STREETLIGHT_MAX_AMPS = "MAX_AMPS";

        public const string FIELD_TONNAGE_TONNAGE = "TONNAGE";
        public const string FIELD_TONNAGE_LRA = "LRA";

        public const string FIELD_VALIDATIONLOG_WR_NUMBER = "WR_NBR";
        public const string FIELD_VALIDATIONLOG_G3E_FID = "G3E_FID";
        public const string FIELD_VALIDATIONLOG_TOOL_NAME = "TOOL_NM";
        public const string FIELD_VALIDATIONLOG_STATUS = "VALIDATION_STATUS_C";
        public const string FIELD_VALIDATIONLOG_PRIORITY = "VALIDATION_PRIORITY_C";
        public const string FIELD_VALIDATIONLOG_COMMENTS = "COMMENTS";

        // Queries
        public const string QUERY_COMMAND_METADATA = "select subsystem_name, subsystem_component, param_name, param_value, param_desc " +
                                                     "from sys_generalparameter where subsystem_name = ?";

        public const string QUERY_CPT_CABLE = "select cable." + FIELD_CPT_CABLE_CINO + ", cable." + FIELD_CPT_CABLE_DESCRIPTION + ", cable." + FIELD_CPT_CABLE_VOLTAGE +
                                              ", cable." + FIELD_CPT_CABLE_TYPE + ", cable." + FIELD_CPT_CABLE_SIZE_MATERIAL + ", cable." + FIELD_CPT_CABLE_NUMBER_OF_PHASES +
                                              ", cable." + FIELD_CPT_CABLE_NUMBER_OF_CABLES + ", config." + FIELD_CABLECONFIG_CONFIGURATION +
                                              ", cable." + FIELD_CPT_CABLE_NON_STD + ", cable." + FIELD_CPT_CABLE_OUTSIDE_DIAMETER + ", cable." + FIELD_CPT_CABLE_WEIGHT_PER_FOOT +
                                              ", cable." + FIELD_CPT_CABLE_MAX_SWBP + ", cable." + FIELD_CPT_CABLE_MAX_TENSION + ", cable." + FIELD_CPT_CABLE_CU +
                                              ", cable." + FIELD_CPT_CABLE_SWBP_CHANGE_POINT + ", cable." + FIELD_CPT_CABLE_MAX_LENGTH +
                                              " from " + TABLE_CPT_CABLE + " cable, " + TABLE_CABLECONFIG + " config " +
                                              " where cable." + FIELD_CPT_CABLE_CONFIGURATION + " = config." + FIELD_CABLECONFIG_CCNO +
                                              " order by cable." + FIELD_CPT_CABLE_DESCRIPTION;

        public const string QUERY_DUCT = "select duct." + FIELD_DUCT_DTNO + ", duct." + FIELD_DUCT_DESCRIPTION + ", duct." + FIELD_DUCT_INSIDE_DIAMETER +
                                             ", duct." + FIELD_DUCT_NOM_INSIDE_DIAMETER + ", duct." + FIELD_DUCT_STD_BEND_RADIUS + ", duct." + FIELD_DUCT_MIN_CLEARANCE +
                                             " from " + TABLE_DUCTTRENCH + " duct";

        public const string QUERY_CPT_MAX_DISTANCE = "select val." + FIELD_CPT_VALIDATION_CABLE_ID + ", val." + FIELD_CPT_VALIDATION_DUCT_ID +
                                                   ", val." + FIELD_CPT_VALIDATION_MAX_DISTANCE +
                                                   " from " + TABLE_CPT_VALIDATION + " val";

        public const string QUERY_JOB = "select job." + FIELD_JOB_DESCRIPTION + ", job." + FIELD_JOB_TYPE +
                                                   " from " + TABLE_JOB + " job" +
                                                   " where " + FIELD_JOB_IDENTIFIER + " = ?";

        public const string QUERY_TONNAGE = "select tonnage." + FIELD_TONNAGE_TONNAGE + ", tonnage." + FIELD_TONNAGE_LRA +
                                                " from " + TABLE_METER + " tonnage";

        public const string QUERY_DERATING = "select derating." + FIELD_DERATING_VALUE + ", derating." + FIELD_NUMBER_OF_CABLES +
                                                   " from " + TABLE_DERATING + " derating" +
                                                   " where " + FIELD_DERATING_XFMR_TYPE + " = ?";

        public const string QUERY_XFMR = "select xfmr." + FIELD_XFMR_CU + ", xfmr." + FIELD_XFMR_DESCRIPTION +
                                             ", xfmr." + FIELD_XFMR_XFMR_SIZE + ", xfmr." + FIELD_XFMR_TYPE +
                                             ", xfmr." + FIELD_XFMR_VOLTAGE + ", xfmr." + FIELD_XFMR_OH_UG +
                                             ", xfmr." + FIELD_XFMR_TYPE_FACTOR + ", xfmr." + FIELD_XFMR_IMPEDANCE +
                                             ", xfmr." + FIELD_XFMR_RESISTANCE + ", xfmr." + FIELD_XFMR_REACTANCE +
                                             ", volt." + FIELD_XFMR_VOLT_VS + ", volt." + FIELD_XFMR_VOLT_VF +
                                             ", volt." + FIELD_XFMR_VOLT_V3PH +
                                             " from " + TABLE_XFMR + " xfmr, " + TABLE_XFMR_VOLTAGE + " volt" +
                                             " where xfmr." + FIELD_XFMR_VOLTAGE + " = volt." + FIELD_XFMR_VOLT_VOLTAGE;

        public const string QUERY_CABLE = "select cable." + FIELD_CABLE_CINO + ", cable." + FIELD_CABLE_CU +
                                                 ", cable." + FIELD_CABLE_DESC + ", cable." + FIELD_CABLE_DC_INCH +
                                                 ", cable." + FIELD_CABLE_OD_INCH + ", cable." + FIELD_CABLE_RDC25 +
                                                 ", cable." + FIELD_CABLE_GMRF + ", cable." + FIELD_CABLE_TC +
                                                 ", cable." + FIELD_CABLE_AMPACITY + ", cable." + FIELD_CABLE_TCMIN +
                                                 ", cable." + FIELD_CABLE_TZERO + ", cable." + FIELD_CABLE_RDC +
                                                 ", cable." + FIELD_CABLE_RACT + ", cable." + FIELD_CABLE_X +
                                                 ", cable." + FIELD_CABLE_R_PER_FT + ", cable." + FIELD_CABLE_X_PER_FT +
                                                 " from " + TABLE_CABLE + " cable";

        public const string QUERY_STREET_LIGHT = "select sl." + FIELD_STREETLIGHT_BINO + ", sl." + FIELD_STREETLIGHT_CU +
                                                 ", sl." + FIELD_STREETLIGHT_DESC + ", sl." + FIELD_STREETLIGHT_VOLTAGE +
                                                 ", sl." + FIELD_STREETLIGHT_REGULATION + ", sl." + FIELD_STREETLIGHT_POWER_FACTOR +
                                                 ", sl." + FIELD_STREETLIGHT_MAX_AMPS +
                                                 " from " + TABLE_STREET_LIGHT + " sl";

        public const string QUERY_TRACE_RESULTS = "select tr.g3e_fid, tr.g3e_fno, tr.g3e_traceorder, tr.g3e_sourcefid, conn.length_actual_q length, conn.phase_alpha phase " +
                                                      "from traceresult tr, traceid ti, connectivity_n conn " +
                                                      "where ti.g3e_name = ? and ti.g3e_id = tr.g3e_tno and tr.g3e_fid = conn.g3e_fid " +
                                                      " order by tr.g3e_traceorder";

        public const string QUERY_VALIDATION = "select " + FIELD_VALIDATIONLOG_G3E_FID + " from " + TABLE_VALIDATIONLOG +
                                                   " where " + FIELD_VALIDATIONLOG_WR_NUMBER + " = ? and " + FIELD_VALIDATIONLOG_G3E_FID +
                                                   " = ? and " + FIELD_VALIDATIONLOG_TOOL_NAME + " = ?";

        public const string SQL_UPDATE_VALIDATION = "update " + TABLE_VALIDATIONLOG + " set " + FIELD_VALIDATIONLOG_STATUS + " = ?, " +
                                                        FIELD_VALIDATIONLOG_PRIORITY + " = ?, " + FIELD_VALIDATIONLOG_COMMENTS + " = ? where " +
                                                        FIELD_VALIDATIONLOG_WR_NUMBER + " = ? and " + FIELD_VALIDATIONLOG_G3E_FID +
                                                        " = ? and " + FIELD_VALIDATIONLOG_TOOL_NAME + " = ?";

        public const string SQL_INSERT_VALIDATION = "insert into " + TABLE_VALIDATIONLOG + " (" + FIELD_VALIDATIONLOG_WR_NUMBER + "," +
                                                        FIELD_VALIDATIONLOG_G3E_FID + "," + FIELD_VALIDATIONLOG_TOOL_NAME + "," +
                                                        FIELD_VALIDATIONLOG_STATUS + "," + FIELD_VALIDATIONLOG_PRIORITY + "," +
                                                        FIELD_VALIDATIONLOG_COMMENTS + ") values (?,?,?,?,?,?)";


        // Report Parameter Names
        public const string REPORT_PARAMETER_VERSIONDATE = "VersionAndDate";
        public const string REPORT_PARAMETER_WR = "WR";
        public const string REPORT_PARAMETER_DATE = "Date";
        public const string REPORT_PARAMETER_WR_DESCRIPTION = "WRDescription";

        // Cable Pull Tension
        public const string REPORT_PARAMETER_CPT_VOLTAGE = "Voltage";
        public const string REPORT_PARAMETER_CPT_CABLE_TYPE = "CableType";
        public const string REPORT_PARAMETER_CPT_CONDUCTOR = "Conductor";
        public const string REPORT_PARAMETER_CPT_CABLE_DESCRIPTION = "CableDescription";
        public const string REPORT_PARAMETER_CPT_PHASES = "Phases";
        public const string REPORT_PARAMETER_CPT_DUCT_SIZE = "DuctSize";
        public const string REPORT_PARAMETER_CPT_CONDUIT_ID = "ConduitID";
        public const string REPORT_PARAMETER_CPT_MIN_BEND_RADIUS = "MinBendRadius";
        public const string REPORT_PARAMETER_CPT_JAM_RATIO = "JamRatio";
        public const string REPORT_PARAMETER_CPT_NON_STD = "NonStd";
        public const string REPORT_PARAMETER_CPT_CABLE_WEIGHT = "CableWeight";
        public const string REPORT_PARAMETER_CPT_MAX_TENSION = "MaxTension";
        public const string REPORT_PARAMETER_CPT_MAX_SWBP = "MaxSWBP";
        public const string REPORT_PARAMETER_CPT_CABLE_OD = "CableOD";
        public const string REPORT_PARAMETER_CPT_CABLE_CONFIG = "CableConfig";
        public const string REPORT_PARAMETER_CPT_WT_CORR_FACTOR = "WtCorrFactor";
        public const string REPORT_PARAMETER_CPT_CLEARANCE = "Clearance";
        public const string REPORT_PARAMETER_CPT_TOTAL_LENGTH = "TotalLength";

        // Guying
        public const string REPORT_PARAMETER_GUY_WR = "WR";
        public const string REPORT_PARAMETER_GUY_SCENARIO = "Scenario";
        public const string REPORT_PARAMETER_GUY_DATE = "Date";

        // Secondary Calculator
        public const string REPORT_PARAMETER_SC_PF_SUMMER = "PowerFactorSummer";
        public const string REPORT_PARAMETER_SC_PF_WINTER = "PowerFactorWinter";
        public const string REPORT_PARAMETER_SC_TONNAGE = "Tonnage";
        public const string REPORT_PARAMETER_SC_XFMR_TYPE = "XfmrType";
        public const string REPORT_PARAMETER_SC_XFMR_VOLTAGE = "XfmrVoltage";
        public const string REPORT_PARAMETER_SC_XFMR_VDROP = "XfmrVoltageDrop";
        public const string REPORT_PARAMETER_SC_XFMR_VDROP_HIGH = "XmfrVDropPctThreshold";
        public const string REPORT_PARAMETER_SC_SEC_VDROP_HIGH = "SecVDropPctThreshold";
        public const string REPORT_PARAMETER_SC_SRVC_VDROP_HIGH = "SrvcVDropPctThreshold";
        public const string REPORT_PARAMETER_SC_SEC_FLICKER_LOW = "SecFlickerPctLowThreshold";
        public const string REPORT_PARAMETER_SC_SEC_FLICKER_HIGH = "SecFlickerPctHighThreshold";
        public const string REPORT_PARAMETER_SC_SRVC_FLICKER_LOW = "SrvcFlickerPctLowThreshold";
        public const string REPORT_PARAMETER_SC_SRVC_FLICKER_HIGH = "SrvcFlickerPctHighThreshold";

        // Street Light Voltage Drop
        public const string REPORT_PARAMETER_SL_NOMINAL_VOLTAGE = "NominalVoltage";
        public const string REPORT_PARAMETER_SL_SOURCE_VOLTAGE = "ActualSourceVoltage";
        public const string REPORT_PARAMETER_SL_ALLOWED_VOLTAGE = "AllowedMinVoltage";

        // Sag Clearance
        public const string REPORT_PARAMETER_SAG_PROJECT = "Project";
        public const string REPORT_PARAMETER_SAG_LOCATION = "Location";
        public const string REPORT_PARAMETER_SAG_DESIGNER = "Designer";
        public const string REPORT_PARAMETER_SAG_DATE = "Date";
        public const string REPORT_PARAMETER_SAG_CONDUCTOR = "Conductor";

        // Error messages
        public const string ERROR_ADDING_HYPERLINK = "Error adding hyperlink component";
        public const string ERROR_NO_TRACE_RECORDS = "The trace did not return any records";
        public const string ERROR_PROCESS_TRACE_RESULTS = "Error processing the trace results";
        public const string ERROR_REPORT_CREATION = "Error creating report";
        public const string ERROR_REPORT_PRINTING = "Error printing report";
        public const string ERROR_REPORT_SAVING = "Error saving report";
        public const string ERROR_CONVERTING_PDF = "Error converting to pdf";
        public const string ERROR_OPENING_PDF = "Error opening the pdf";
        public const string ERROR_DELETING_FILE = "Error deleting file";
        public const string ERROR_LOADING_FILE = "Error loading file";
        public const string ERROR_DEFAULT_VALUES = "Error setting default values";
        public const string ERROR_JOB_QUERY = "Error querying for job description";
        public const string ERROR_JOB_INVALID = "Invalid Job Type selected. This command is only available for WR type jobs";
        public const string ERROR_APPLY_PENDING_CHANGES = "Error applying pending CU changes";
        public const string ERROR_RETRIEVING_COMMAND_METADATA = "Error retrieving metadata for command";
        public const string ERROR_RETRIEVING_REPORT_DATA = "Error retrieving report data";
        public const string ERROR_WRITING_VALIDATION_RESULTS = "Error writing the validation results to " + TABLE_VALIDATIONLOG;

        // Cable Pull Tension
        public const string ERROR_CPT_CABLE_LIST = "Error populating list of Cable Descriptions";
        public const string ERROR_CPT_DUCT_LIST = "Error populating list of Duct Sizes";
        public const string ERROR_CPT_VALIDATION_LIST = "Error querying for validation records in " + TABLE_CPT_VALIDATION;
        public const string ERROR_CPT_CALCULATE_CABLE_CLEARANCE = "Error calculating the cable clearance";
        public const string ERROR_CPT_CALCULATE = "Error performing calculations";
        public const string ERROR_CPT_CALCULATE_JAM_RATIO = "Error calculating the jam ratio";
        public const string ERROR_CPT_CALCULATE_TENSION = "Error calculating the tension";
        public const string ERROR_CPT_CALCULATE_STRAIGHT_TENSION = "Error calculating the tension for a straight section";
        public const string ERROR_CPT_CALCULATE_HBEND_TENSION = "Error calculating the tension for a horizontal bend section";
        public const string ERROR_CPT_CALCULATE_VBEND_TENSION = "Error calculating the tension for a vertical bend section";
        public const string ERROR_CPT_CALCULATE_RISER_TENSION = "Error calculating the tension for a riser section";
        public const string ERROR_CPT_CALCULATE_DIP_TENSION = "Error calculating the tension for a dip section";
        public const string ERROR_CPT_CALCULATE_SWBP = "Error calculating the side wall bearing pressure (SWBP)";
        public const string ERROR_CPT_CALCULATE_WCF = "Error calculating the weight correction factor";
        public const string ERROR_CPT_DISPLAY_CALCULATIONS = "Error displaying calculation results";
        public const string ERROR_CPT_REVERSE_CALCULATIONS = "Error performing the reverse calculations";
        public const string ERROR_CPT_NO_CABLE_RECORDS = "There are no cable records defined in " + TABLE_CPT_CABLE;
        public const string ERROR_CPT_NO_DUCT_RECORDS = "There are no duct records defined in " + TABLE_DUCTTRENCH;
        public const string ERROR_CPT_FORM_INITIALIZATION = "Error initializing form";
        public const string ERROR_CPT_INVALID_FEATURE_SELECTED = "Invalid feature selected. Select a single UG Conductor feature.";
        public const string ERROR_CPT_INVALID_COMPONENT_SELECTED = "Invalid component selected. Select the UG Conductor's primary graphic or detail component.";
        public const string ERROR_CPT_INVALID_GEOMETRY = "Geometry type is invalid for this command.";
        public const string ERROR_CPT_NO_RELATED_DUCTBANK = "Related Duct Bank does not exist";
        public const string ERROR_CPT_PROCESSING_GEOMETRY = "Error processing the geometry record";
        public const string ERROR_CPT_MULTIPLE_FEATURES_SELECTED = "More than one feature selected. Select a single UG Conductor feature.";
        public const string ERROR_CPT_WRITING_VALIDATION_RESULTS = "Error writing the validation results to " +TABLE_VALIDATIONLOG;
        public const string ERROR_CPT_GET_MAX_PULLING_DISTANCE = "Error getting the maximum pulling distance from " + TABLE_CPT_VALIDATION;
        public const string ERROR_CPT_CELL_EDIT = "Error in the edited cell event";
        public const string ERROR_CPT_ADD_ROW = "Error in the add row event";
        public const string ERROR_CPT_REMOVE_ROW = "Error in the remove row event";
        public const string ERROR_CPT_ADD_PULLEY = "Error adding a Pulley";
        public const string ERROR_CPT_RENUMBER_INDEX = "Error renumbering the grid indexes";
        public const string ERROR_CPT_ENDING_BEND = "Error processing Ending Vertical Bend";
        public const string ERROR_CPT_STARTING_BEND = "Error processing Starting Vertical Bend";
        public const string ERROR_CPT_DUCT_SIZE_CHANGE = "Error in the change duct size event";
        public const string ERROR_CPT_CABLE_CHANGE = "Error in the change cable event";
        public const string ERROR_CPT_RESET_FORM = "Error resetting the form for a new calculation";
        public const string ERROR_CPT_GRID_SELECTION_CHANGE = "Error in the grid selection change event";
        public const string ERROR_CPT_SET_DEFAULT_CELL_VALUES = "Error setting the default cell values for a new row";
        public const string ERROR_CPT_GRID_POPULATION = "Error populating the grid with the selected geomtry";
        public const string ERROR_CPT_DUCT_CHANGE = "Error processing the Duct change";
        public const string ERROR_CPT_SELECTED_CU = "The selected CU has not been configured for this command";

        // Guying
        public const string ERROR_GUY_INVALID_FEATURE_SELECTED = "Invalid feature selected. Select set includes features other than Poles.";
        public const string ERROR_GUY_VALIDATING_RESULTS = "Error validating results";
        public const string ERROR_GUY_WRITING_VALIDATION_RESULTS = "Error writing validation results";

        // Sag Clearance
        public const string ERROR_SAG_INVALID_FEATURE_SELECTED = "Invalid feature selected. Select a single Conductor feature.";

        // Handhole CIAC
        public const string ERROR_HH_INVALID_FEATURE_SELECTED = "Invalid feature selected. Select a single Secondary Box feature.";

        // Service Enclosure
        public const string ERROR_SE_INVALID_FEATURE_SELECTED = "Invalid feature selected. Select a single Secondary Enclosure feature.";

        // Secondary Calculator
        public const string ERROR_SC_MULTIPLE_FEATURES_SELECTED = "More than one feature selected. Select a single Transformer feature.";
        public const string ERROR_SC_TIE_XFMR_SELECTED = "A Tie Transformer was found in the trace results. The calculation cannot be performed.";
        public const string ERROR_SC_MULTIPLE_XFMRS_IN_TRACE = "A second Transformer was found in the trace results. The calculation cannot be performed.";
        public const string ERROR_SC_INVALID_FEATURE_SELECTED = "Invalid feature selected. Select a single Transformer feature.";
        public const string ERROR_SC_INVALID_3PH_VOLTAGE = "Invalid voltage for the selected 3-Phase Transformer. The calculation cannot be performed.";
        public const string ERROR_SC_INVALID_PREMISE_PHASE = "A calculation cannot be performed on a 3-phase Transformer feeding a single phase Service Point.";
        public const string ERROR_SC_INVALID_CONFIGURATION = "Invalid configuration. This command can’t be used for an Open Wye or Open Delta configuration.";
        public const string ERROR_SC_FORM_INITIALIZATION = "Error initializing form";
        public const string ERROR_SC_NO_TONNAGE_RECORDS = "There are no AC Tonnage records defined in " + TABLE_METER;
        public const string ERROR_SC_CABLES_PER_PHASE_RECORDS = "There are no records defined in " + TABLE_DERATING + " for the Transformer type.";
        public const string ERROR_SC_NO_CABLE_RECORDS = "There are no Cable records defined in " + TABLE_CABLE;
        public const string ERROR_SC_NO_XFMR_RECORDS = "There are no Transformer records defined in " + TABLE_XFMR;
        public const string ERROR_SC_TONNAGE_LIST = "Error populating list of AC Tonnage values";
        public const string ERROR_SC_CABLES_PER_PHASE_LIST = "Error populating list of # Cables Per Phase";
        public const string ERROR_SC_TONNAGE_CHANGE = "Error processing change to AC Tonnage";
        public const string ERROR_SC_CABLES_PER_PHASE_CHANGE = "Error processing change to # Cables Per Phase";
        public const string ERROR_SC_CABLE_METADATA = "Error getting list of cables from " + TABLE_CABLE;
        public const string ERROR_SC_XFMR_METADATA = "Error getting list of Transformers from " + TABLE_XFMR;
        public const string ERROR_SC_CABLE_TYPE = "Error getting cable type for CU";
        public const string ERROR_SC_XFMR_SIZING_COMMAND = "Error opening Transformer Sizing spreadsheet";
        public const string ERROR_SC_CALCULATE = "Error performing the calculation";
        public const string ERROR_SC_CALCULATE_LOADKVA = "Error calculating the Load KVA";
        public const string ERROR_SC_CALCULATE_XFMR_VDROP = "Error calculating the Transformer Voltage Drop";
        public const string ERROR_SC_CALCULATE_CONDUCTOR_VDROP = "Error calculating the Conductor Voltage Drop";
        public const string ERROR_SC_CALCULATE_CONDUCTOR_VLMAG = "Error calculating the Conductor Vlmag";
        public const string ERROR_SC_CALCULATE_FLICKER = "Error calculating the flicker";
        public const string ERROR_SC_CALCULATE_Z = "Error calculating the impedance";
        public const string ERROR_SC_CALCULATE_Z_SERVICE_PT = "Error calculating the Service Point impedance";
        public const string ERROR_SC_PROCESSING_CONDUCTOR = "Error processing Secondary Conductor";
        public const string ERROR_SC_PROCESSING_SERVICE = "Error processing Service Line";
        public const string ERROR_SC_PROCESSING_SERVICE_PT = "Error processing Service Point";
        public const string ERROR_SC_DETERMINING_LOAD = "Error determining the correct load to use";
        public const string ERROR_SC_UPDATING_LOAD = "Error updating the load values";
        public const string ERROR_SC_GRID_POPULATION = "Error populating the grids";
        public const string ERROR_SC_GRID_ADD_CONDUCTOR = "Error adding Secondary Conductor to the grid";
        public const string ERROR_SC_GRID_ADD_SERVICE = "Error adding Service Line to the grid";
        public const string ERROR_SC_GRID_CELL_CLICK = "Error processing cell click";
        public const string ERROR_SC_VALIDATION = "Error validating the calculation results";
        public const string ERROR_SC_XFMR_CHANGE = "Error processing the Transformer change";
        public const string ERROR_SC_XFMR_PROPERTIES = "Error setting Transformer properties";
        public const string ERROR_SC_GRID_FUTURE_SRVC_CHANGE = "Error processing the Future Service Count change";

        // Street Light Voltage Drop
        public const string ERROR_SL_NO_STREETLIGHT_RECORDS = "There are no Street Light records defined in " + TABLE_STREET_LIGHT;
        public const string ERROR_SL_STREETLIGHT_METADATA = "Error getting list of Street Lights from " + TABLE_STREET_LIGHT;
        public const string ERROR_SL_STREETLIGHT_PROPERTIES = "Error setting Street Light properties";
        public const string ERROR_SL_PROCESSING_STREETLIGHT = "Error processing Street Light";
        public const string ERROR_SL_CALCULATE = "Error performing the calculation";
        public const string ERROR_SL_CALCULATE_VDROP = "Error calculating the Voltage Drop";
        public const string ERROR_SL_CALCULATE_LOADAMPS = "Error calculating the Load Amps";

        // Status Messages
        public const string MESSAGE_CREATING_HYPERLINK = "Creating Hyperlink component...";
        public const string MESSAGE_CALCULATING = "Calculating...";
        public const string MESSAGE_VALIDATING = "Validating...";
        public const string MESSAGE_REPORT_CREATING = "Creating Report...";
        public const string MESSAGE_REPORT_SAVING = "Saving Report...";
        public const string MESSAGE_APPLY_PENDING_CHANGES = "Applying pending CU changes...";
        public const string MESSAGE_TRACE_EXECUTING = "Executing trace...";
        public const string MESSAGE_TRACE_PROCESSING_RESULTS = "Processing trace results...";
        public const string MESSAGE_TRACE_DISPLAY_RESULTS = "Adding trace to legend...";
        public const string MESSAGE_REPORT_UPLOADING = "Uploading Report...";

        // Secondary Calculator
        public const string MESSAGE_SC_OPEN_XFMR_SIZING = "Opening Single Phase Transformer Sizing spreadsheet...";
    }
}
