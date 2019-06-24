using System.Collections.Generic;

namespace GTechnology.Oncor.CustomAPI
{
    /// <summary>
    /// Used to store the DEIS transaction record and feature information for processing.
    /// </summary>
    internal static class TransactionDEIS
    {
        // Variables for DEIS transaction record
        private static string m_TranID = string.Empty;
        private static string m_Requestor = string.Empty;
        private static int m_TranNumber = 0;
        private static string m_TranDate = string.Empty;
        private static string m_TranCode = string.Empty;
        private static string m_BankFLN = string.Empty;
        private static string m_StructureID = string.Empty;
        private static string m_TranStatus = string.Empty;
        private static string m_TranMessage = string.Empty;
        private static string m_WrNumber = string.Empty;
        private static string m_ServArea = string.Empty;
        private static string m_Yard = string.Empty;
        private static string m_UserID = string.Empty;
        private static string m_ConnSec = string.Empty;
        private static string m_ExistingBank = string.Empty;
        private static string m_WiringConfigCode = string.Empty;
        private static string m_Location = string.Empty;
        private static string m_InsTrf1CompanyNumber = string.Empty;
        private static string m_InsTrf1TSN = string.Empty;
        private static string m_InsTrf1CU = string.Empty;
        private static double m_InsTrf1KvaSize = 0;
        private static string m_InsTrf1PhaseCode = string.Empty;
        private static string m_InsTrf1TypeCode = string.Empty;
        private static string m_InsTrf1TypeValue = string.Empty;
        private static string m_InsTrf1MountCode = string.Empty;
        private static string m_InsTrf1MountValue = string.Empty;
        private static string m_InsTrf1MountOrientation = string.Empty;
        private static string m_InsTrf1KindCode = string.Empty;
        private static string m_InsTrf1KindValue = string.Empty;
        private static string m_InsTrf1PriVoltCode = string.Empty;
        private static string m_InsTrf1PriVoltValue = string.Empty;
        private static string m_InsTrf1SecVoltCode = string.Empty;
        private static string m_InsTrf1SecVoltValue = string.Empty;
        private static string m_InsTrf2CompanyNumber = string.Empty;
        private static string m_InsTrf2TSN = string.Empty;
        private static string m_InsTrf2CU = string.Empty;
        private static double m_InsTrf2KvaSize = 0;
        private static string m_InsTrf2PhaseCode = string.Empty;
        private static string m_InsTrf2TypeCode = string.Empty;
        private static string m_InsTrf2TypeValue = string.Empty;
        private static string m_InsTrf2MountCode = string.Empty;
        private static string m_InsTrf2MountValue = string.Empty;
        private static string m_InsTrf2MountOrientation = string.Empty;
        private static string m_InsTrf2KindCode = string.Empty;
        private static string m_InsTrf2KindValue = string.Empty;
        private static string m_InsTrf2PriVoltCode = string.Empty;
        private static string m_InsTrf2PriVoltValue = string.Empty;
        private static string m_InsTrf2SecVoltCode = string.Empty;
        private static string m_InsTrf2SecVoltValue = string.Empty;
        private static string m_InsTrf3CompanyNumber = string.Empty;
        private static string m_InsTrf3TSN = string.Empty;
        private static string m_InsTrf3CU = string.Empty;
        private static double m_InsTrf3KvaSize = 0;
        private static string m_InsTrf3PhaseCode = string.Empty;
        private static string m_InsTrf3TypeCode = string.Empty;
        private static string m_InsTrf3TypeValue = string.Empty;
        private static string m_InsTrf3MountCode = string.Empty;
        private static string m_InsTrf3MountValue = string.Empty;
        private static string m_InsTrf3MountType = string.Empty;
        private static string m_InsTrf3MountOrientation = string.Empty;
        private static string m_InsTrf3KindCode = string.Empty;
        private static string m_InsTrf3KindValue = string.Empty;
        private static string m_InsTrf3PriVoltCode = string.Empty;
        private static string m_InsTrf3PriVoltValue = string.Empty;
        private static string m_InsTrf3SecVoltCode = string.Empty;
        private static string m_InsTrf3SecVoltValue = string.Empty;
        private static string m_RemTrf1CompanyNumber = string.Empty;
        private static string m_RemTrf1TSN = string.Empty;
        private static double m_RemTrf1KvaSize = 0;
        private static string m_RemTrf1PhaseCode = string.Empty;
        private static string m_RemTrf1TypeCode = string.Empty;
        private static string m_RemTrf1MountCode = string.Empty;
        private static string m_RemTrf1KindCode = string.Empty;
        private static string m_RemTrf1PriVoltCode = string.Empty;
        private static string m_RemTrf1SecVoltCode = string.Empty;
        private static string m_RemTrf2CompanyNumber = string.Empty;
        private static string m_RemTrf2TSN = string.Empty;
        private static double m_RemTrf2KvaSize = 0;
        private static string m_RemTrf2PhaseCode = string.Empty;
        private static string m_RemTrf2TypeCode = string.Empty;
        private static string m_RemTrf2MountCode = string.Empty;
        private static string m_RemTrf2KindCode = string.Empty;
        private static string m_RemTrf2PriVoltCode = string.Empty;
        private static string m_RemTrf2SecVoltCode = string.Empty;
        private static string m_RemTrf3CompanyNumber = string.Empty;
        private static string m_RemTrf3TSN = string.Empty;
        private static double m_RemTrf3KvaSize = 0;
        private static string m_RemTrf3PhaseCode = string.Empty;
        private static string m_RemTrf3TypeCode = string.Empty;
        private static string m_RemTrf3MountCode = string.Empty;
        private static string m_RemTrf3KindCode = string.Empty;
        private static string m_RemTrf3PriVoltCode = string.Empty;
        private static string m_RemTrf3SecVoltCode = string.Empty;

        private static int m_VoltageRegulatorFID = 0;               // Voltage Regulator FID for Voltage Regulator found at the Structure ID

        /// <summary>
        /// Used to store information about the Structure feature found in the GIS matching the input Structure ID.
        /// </summary>
        internal struct StructurePropertiesStruct
        {
            internal short FNO;                                     // The g3e_fno for the existing Structure.
            internal int FID;                                       // The g3e_fid for the existing Structure.
            internal short PrimaryGraphicCNO;                       // The g3e_cno for the existing Structure Symbol component.
            internal string FeatureState;                           // The feature state for the existing Structure.
            internal string Orientation;                            // The orientation (OH, UG) for the existing Structure.
        }
        internal static StructurePropertiesStruct StructureProperties;

        /// <summary>
        /// Used to store information about the Transformer feature either found in the GIS or the new Transformer feature created for an install transaction.
        /// </summary>
        internal struct TransformerPropertiesStruct
        {
            internal short FNO;                                     // The g3e_fno for the existing or newly created Transformer.
            internal int FID;                                       // The g3e_fid for the existing or newly created Transformer.
            internal short PrimaryGraphicCNO;                       // The g3e_cno for the Transformer Symbol component.
            internal short LabelCNO;                                // The g3e_cno for the Transformer Label component.
            internal short BankCNO;                                 // The g3e_cno for the Transformer Bank Attributes component.
            internal short UnitCNO;                                 // The g3e_cno for the Transformer Unit Attributes component.
            internal string BankTable;                              // The name of the Transformer bank table to use.
            internal string UnitTable;                              // The name of the Transformer unit table to use.
            internal bool UsePhaseCode;                             // Flag to indicate if phase should be compared to Primary Conductor Phase (TRUE) or Phase Position (FALSE).
            internal string ExistingPhases;                         // The phases on the existing GIS Transformer.
            internal int PhaseCount;                                // The resultant phase count for the GIS Transformer.
            internal string InsTrf1PhaseValue;                      // The phase value to set on the unit record for the first transformer. Not the same as InsTrf1PhaseCode since it may need to be converted from Primary Conductor phase position.
            internal string InsTrf2PhaseValue;                      // The phase value to set on the unit record for the second transformer. Not the same as InsTrf2PhaseCode since it may need to be converted from Primary Conductor phase position.
            internal string InsTrf3PhaseValue;                      // The phase value to set on the unit record for the third transformer. Not the same as InsTrf3PhaseCode since it may need to be converted from Primary Conductor phase position.
            internal string PrimaryWiringConfiguration;             // The primary wiring configuration.
            internal string SecondaryWiringConfiguration;           // The secondary wiring configuration.
            internal string FeatureState;                           // The feature state for the existing GIS Transformer.
            internal bool RemoveBank;                               // Indicates if Transformer feature is being removed (TRUE) or only input Units (FALSE)
            internal int Node1;                                     // Connectivity Node1 of the existing Transformer. Only used for a change-out.
            internal int Node2;                                     // Connectivity Node2 of the existing Transformer. Only used for a change-out.
        }
        internal static TransformerPropertiesStruct TransformerProperties;

        /// <summary>
        /// Used to store information about the Primary Conductor feature found in the GIS at the Transformer.
        /// </summary>
        internal struct PrimaryPropertiesStruct
        {
            internal short FNO;                                     // The g3e_fno for the existing Primary Conductor.
            internal int FID;                                       // The g3e_fid for the existing Primary Conductor.
            internal short PrimaryGraphicCNO;                       // The g3e_cno for the existing Primary Conductor Line component.
            internal short BankCNO;                                 // The g3e_cno for the existing Primary Conductor Bank Attributes component.
            internal short WireCNO;                                 // The g3e_cno for the existing Primary Conductor Wire Attributes component.
            internal string Phases;                                 // The phases on the existing Primary Conductor.
            internal int PhaseCount;                                // The phase count on the existing Primary Conductor.
            internal int ProtectiveDeviceID;                        // The protective device id for the existing Primary Conductor.
            internal short ConnectNode;                             // Node at which the new Transformer should be connected
        }
        internal static PrimaryPropertiesStruct PrimaryProperties;

        /// <summary>
        /// Used to store information about the secondary features found in the GIS at the Transformer.
        /// </summary>
        internal struct SecondaryPropertiesStruct
        {
            internal short FNO;                                     // The g3e_fno for the existing Secondary Conductor or Service Line.
            internal int FID;                                       // The g3e_fid for the existing Secondary Conductor or Service Line.
            internal short PrimaryGraphicCNO;                       // The g3e_cno for the existing Secondary Conductor Line or Service Line component.
            internal string FeatureState;                           // The feature state for the existing Secondary Conductor or Service Line.
        }
        internal static SecondaryPropertiesStruct SecondaryProperties;

        #region constants
        // Feature Numbers
        internal const short FNO_AUTOTRANSFORMER = 34;
        internal const short FNO_ISOLATION_POINT = 6;
        internal const short FNO_POLE = 110;        
        internal const short FNO_TRANSFORMER_OH = 59;
        internal const short FNO_TRANSFORMER_OH_NETWORK = 98;
        internal const short FNO_TRANSFORMER_UG = 60;
        internal const short FNO_TRANSFORMER_UG_NETWORK = 99;
        internal const short FNO_SECONDARY_CONDNODE = 162;
        internal const short FNO_SECONDARY_CONDUCTOR_OH = 53;
        internal const short FNO_SECONDARY_CONDUCTOR_UG = 63;
        internal const short FNO_SERVICE_LINE = 54;        
        internal const short FNO_VOLTAGE_REGULATOR = 36;

        // Component Numbers
        internal const short CNO_COMMON_ATTRIBUTES = 1;
        internal const short CNO_CONNECTIVITY_ATTRIBUTES = 11;
        internal const short CNO_CU_ATTRIBUTES = 21;

        internal const short CNO_AUTOTRANSFORMER_PRIMARYGRAPHIC = 3403;
        internal const short CNO_AUTOTRANSFORMER_LABEL = 3404;
        internal const short CNO_AUTOTRANSFORMER_BANK = 3401;
        internal const short CNO_AUTOTRANSFORMER_UNIT = 3402;

        internal const short CNO_TRANSFORMER_OH_PRIMARYGRAPHIC = 5903;
        internal const short CNO_TRANSFORMER_OH_LABEL = 5904;
        internal const short CNO_TRANSFORMER_OH_BANK = 5901;
        internal const short CNO_TRANSFORMER_OH_UNIT = 5902;

        internal const short CNO_TRANSFORMER_UG_PRIMARYGRAPHIC = 6003;
        internal const short CNO_TRANSFORMER_UG_LABEL = 6004;
        internal const short CNO_TRANSFORMER_UG_UNIT = 6002;

        internal const short CNO_PRICOND_OH_BANK = 801;
        internal const short CNO_PRICOND_OH_WIRE = 802;

        internal const short CNO_PRICOND_UG_BANK = 901;
        internal const short CNO_PRICOND_UG_WIRE = 902;

        internal const short CNO_SECONDARY_CONDNODE_ATTRIBUTES = 16201;
        internal const short CNO_SECONDARY_CONDNODE_SYMBOL = 16202;

        internal const short CNO_ISOLATION_POINT_ATTRIBUTES = 4;
        internal const short CNO_ISOLATION_POINT_SYMBOL = 78;

        internal const short CNO_VOLTAGE_REGULATOR_ATTRIBUTES = 3602;

        // Tables
        internal const string TABLE_DEIS_TRANSACTION = "DEIS_TRANSACTION";
        internal const string TABLE_AUTOTRANSFORMER_BANK = "AUTO_XFMR_BANK_N";
        internal const string TABLE_AUTOTRANSFORMER_UNIT = "AUTO_XFMR_UNIT_N";
        internal const string TABLE_TRANSFORMER_OH_BANK = "XFMR_OH_BANK_N";
        internal const string TABLE_TRANSFORMER_OH_UNIT = "XFMR_OH_UNIT_N";
        internal const string TABLE_TRANSFORMER_UG_UNIT = "XFMR_UG_UNIT_N";

        // Table Columns
        internal const string FIELD_DEIS_TRANSACTION_NUMBER = "TRAN_NO";
        internal const string FIELD_DEIS_TRANSACTION_STATUS = "TRAN_STAT_CODE";
        internal const string FIELD_DEIS_TRANSACTION_MSG = "TRAN_MSG";

        // Relationships
        internal const short RNO_OWNERSHIP_PARENT = 2;
        internal const short RNO_OWNERSHIP_CHILD = 3;
        internal const short RNO_CONNECTIVITY = 14;

        // Status Codes
        internal const string TRANS_STATUS_PROCESSING = "PROCESSING";
        internal const string TRANS_STATUS_COMPLETE = "COMPLETE";
        internal const string TRANS_STATUS_SUCCESS = "SUCCESS";
        internal const string TRANS_STATUS_WARNING = "WARNING";
        internal const string TRANS_STATUS_FAILED = "FAILED";

        // Miscellaneous
        internal static List<string> VALID_TRANSACTION_CODES = new List<string>(new string[] { "I", "R", "C", "D" });
        internal static List<short> FNOS_UNDERGROUND_STRUCTURES = new List<short>(new short[] { 108, 109, 117 });
        internal static List<string> STATES_INSTALLED = new List<string>(new string[] { "PPI", "INI", "ABI" });
        internal static List<string> STATES_REMOVED = new List<string>(new string[] { "PPR", "OSR", "ABR" });

        #endregion

        #region Properties

        internal static string TranID
        {
            get
            {
                return m_TranID;
            }

            set
            {
                m_TranID = value;
            }
        }

        internal static string Requestor
        {
            get
            {
                return m_Requestor;
            }

            set
            {
                m_Requestor = value;
            }
        }

        internal static int TransactionNumber
        {
            get
            {
                return m_TranNumber;
            }

            set
            {
                m_TranNumber = value;
            }
        }

        internal static string TransactionDate
        {
            get
            {
                return m_TranDate;
            }

            set
            {
                m_TranDate = value;
            }
        }

        internal static string TransactionCode
        {
            get
            {
                return m_TranCode;
            }

            set
            {
                m_TranCode = value;
            }
        }

        internal static string BankFLN
        {
            get
            {
                return m_BankFLN;
            }

            set
            {
                m_BankFLN = value;
            }
        }

        internal static string StructureID
        {
            get
            {
                return m_StructureID;
            }

            set
            {
                m_StructureID = value;
            }
        }

        internal static string TransactionStatus
        {
            get
            {
                return m_TranStatus;
            }

            set
            {
                m_TranStatus = value;
            }
        }

        internal static string TransactionMessage
        {
            get
            {
                return m_TranMessage;
            }

            set
            {
                m_TranMessage = value;
            }
        }

        internal static string WrNumber
        {
            get
            {
                return m_WrNumber;
            }

            set
            {
                m_WrNumber = value;
            }
        }

        internal static string ServiceArea
        {
            get
            {
                return m_ServArea;
            }

            set
            {
                m_ServArea = value;
            }
        }

        internal static string Yard
        {
            get
            {
                return m_Yard;
            }

            set
            {
                m_Yard = value;
            }
        }

        internal static string UserID
        {
            get
            {
                return m_UserID;
            }

            set
            {
                m_UserID = value;
            }
        }

        internal static string SecondaryConnection
        {
            get
            {
                return m_ConnSec;
            }

            set
            {
                m_ConnSec = value;
            }
        }

        internal static string ExistingBank
        {
            get
            {
                return m_ExistingBank;
            }

            set
            {
                m_ExistingBank = value;
            }
        }

        internal static string WiringConfigCode
        {
            get
            {
                return m_WiringConfigCode;
            }

            set
            {
                m_WiringConfigCode = value;
            }
        }

        internal static string Location
        {
            get
            {
                return m_Location;
            }

            set
            {
                m_Location = value;
            }
        }

        internal static string InsTrf1CompanyNumber
        {
            get
            {
                return m_InsTrf1CompanyNumber;
            }

            set
            {
                m_InsTrf1CompanyNumber = value;
            }
        }

        internal static string InsTrf1TSN
        {
            get
            {
                return m_InsTrf1TSN;
            }

            set
            {
                m_InsTrf1TSN = value;
            }
        }

        internal static string InsTrf1CU
        {
            get
            {
                return m_InsTrf1CU;
            }

            set
            {
                m_InsTrf1CU = value;
            }
        }

        internal static double InsTrf1KvaSize
        {
            get
            {
                return m_InsTrf1KvaSize;
            }

            set
            {
                m_InsTrf1KvaSize = value;
            }
        }


        internal static string InsTrf1PhaseCode
        {
            get
            {
                return m_InsTrf1PhaseCode;
            }

            set
            {
                m_InsTrf1PhaseCode = value;
            }
        }

        internal static string InsTrf1TypeCode
        {
            get
            {
                return m_InsTrf1TypeCode;
            }

            set
            {
                m_InsTrf1TypeCode = value;
            }
        }

        internal static string InsTrf1TypeValue
        {
            get
            {
                return m_InsTrf1TypeValue;
            }

            set
            {
                m_InsTrf1TypeValue = value;
            }
        }

        internal static string InsTrf1MountCode
        {
            get
            {
                return m_InsTrf1MountCode;
            }

            set
            {
                m_InsTrf1MountCode = value;
            }
        }

        internal static string InsTrf1MountValue
        {
            get
            {
                return m_InsTrf1MountValue;
            }

            set
            {
                m_InsTrf1MountValue = value;
            }
        }

        internal static string InsTrf1MountOrientation
        {
            get
            {
                return m_InsTrf1MountOrientation;
            }

            set
            {
                m_InsTrf1MountOrientation = value;
            }
        }

        internal static string InsTrf1KindCode
        {
            get
            {
                return m_InsTrf1KindCode;
            }

            set
            {
                m_InsTrf1KindCode = value;
            }
        }

        internal static string InsTrf1KindValue
        {
            get
            {
                return m_InsTrf1KindValue;
            }

            set
            {
                m_InsTrf1KindValue = value;
            }
        }

        internal static string InsTrf1PriVoltCode
        {
            get
            {
                return m_InsTrf1PriVoltCode;
            }

            set
            {
                m_InsTrf1PriVoltCode = value;
            }
        }

        internal static string InsTrf1PriVoltValue
        {
            get
            {
                return m_InsTrf1PriVoltValue;
            }

            set
            {
                m_InsTrf1PriVoltValue = value;
            }
        }

        internal static string InsTrf1SecVoltCode
        {
            get
            {
                return m_InsTrf1SecVoltCode;
            }

            set
            {
                m_InsTrf1SecVoltCode = value;
            }
        }

        internal static string InsTrf1SecVoltValue
        {
            get
            {
                return m_InsTrf1SecVoltValue;
            }

            set
            {
                m_InsTrf1SecVoltValue = value;
            }
        }


        internal static string InsTrf2CompanyNumber
        {
            get
            {
                return m_InsTrf2CompanyNumber;
            }

            set
            {
                m_InsTrf2CompanyNumber = value;
            }
        }

        internal static string InsTrf2TSN
        {
            get
            {
                return m_InsTrf2TSN;
            }

            set
            {
                m_InsTrf2TSN = value;
            }
        }

        internal static string InsTrf2CU
        {
            get
            {
                return m_InsTrf2CU;
            }

            set
            {
                m_InsTrf2CU = value;
            }
        }

        internal static double InsTrf2KvaSize
        {
            get
            {
                return m_InsTrf2KvaSize;
            }

            set
            {
                m_InsTrf2KvaSize = value;
            }
        }


        internal static string InsTrf2PhaseCode
        {
            get
            {
                return m_InsTrf2PhaseCode;
            }

            set
            {
                m_InsTrf2PhaseCode = value;
            }
        }

        internal static string InsTrf2TypeCode
        {
            get
            {
                return m_InsTrf2TypeCode;
            }

            set
            {
                m_InsTrf2TypeCode = value;
            }
        }

        internal static string InsTrf2TypeValue
        {
            get
            {
                return m_InsTrf2TypeValue;
            }

            set
            {
                m_InsTrf2TypeValue = value;
            }
        }

        internal static string InsTrf2MountCode
        {
            get
            {
                return m_InsTrf2MountCode;
            }

            set
            {
                m_InsTrf2MountCode = value;
            }
        }

        internal static string InsTrf2MountValue
        {
            get
            {
                return m_InsTrf2MountValue;
            }

            set
            {
                m_InsTrf2MountValue = value;
            }
        }

        internal static string InsTrf2MountOrientation
        {
            get
            {
                return m_InsTrf2MountOrientation;
            }

            set
            {
                m_InsTrf2MountOrientation = value;
            }
        }

        internal static string InsTrf2KindCode
        {
            get
            {
                return m_InsTrf2KindCode;
            }

            set
            {
                m_InsTrf2KindCode = value;
            }
        }

        internal static string InsTrf2KindValue
        {
            get
            {
                return m_InsTrf2KindValue;
            }

            set
            {
                m_InsTrf2KindValue = value;
            }
        }

        internal static string InsTrf2PriVoltCode
        {
            get
            {
                return m_InsTrf2PriVoltCode;
            }

            set
            {
                m_InsTrf2PriVoltCode = value;
            }
        }

        internal static string InsTrf2PriVoltValue
        {
            get
            {
                return m_InsTrf2PriVoltValue;
            }

            set
            {
                m_InsTrf2PriVoltValue = value;
            }
        }

        internal static string InsTrf2SecVoltCode
        {
            get
            {
                return m_InsTrf2SecVoltCode;
            }

            set
            {
                m_InsTrf2SecVoltCode = value;
            }
        }

        internal static string InsTrf2SecVoltValue
        {
            get
            {
                return m_InsTrf2SecVoltValue;
            }

            set
            {
                m_InsTrf2SecVoltValue = value;
            }
        }

        internal static string InsTrf3CompanyNumber
        {
            get
            {
                return m_InsTrf3CompanyNumber;
            }

            set
            {
                m_InsTrf3CompanyNumber = value;
            }
        }

        internal static string InsTrf3TSN
        {
            get
            {
                return m_InsTrf3TSN;
            }

            set
            {
                m_InsTrf3TSN = value;
            }
        }

        internal static string InsTrf3CU
        {
            get
            {
                return m_InsTrf3CU;
            }

            set
            {
                m_InsTrf3CU = value;
            }
        }

        internal static double InsTrf3KvaSize
        {
            get
            {
                return m_InsTrf3KvaSize;
            }

            set
            {
                m_InsTrf3KvaSize = value;
            }
        }


        internal static string InsTrf3PhaseCode
        {
            get
            {
                return m_InsTrf3PhaseCode;
            }

            set
            {
                m_InsTrf3PhaseCode = value;
            }
        }

        internal static string InsTrf3TypeCode
        {
            get
            {
                return m_InsTrf3TypeCode;
            }

            set
            {
                m_InsTrf3TypeCode = value;
            }
        }

        internal static string InsTrf3TypeValue
        {
            get
            {
                return m_InsTrf3TypeValue;
            }

            set
            {
                m_InsTrf3TypeValue = value;
            }
        }

        internal static string InsTrf3MountCode
        {
            get
            {
                return m_InsTrf3MountCode;
            }

            set
            {
                m_InsTrf3MountCode = value;
            }
        }

        internal static string InsTrf3MountValue
        {
            get
            {
                return m_InsTrf3MountValue;
            }

            set
            {
                m_InsTrf3MountValue = value;
            }
        }

        internal static string InsTrf3MountOrientation
        {
            get
            {
                return m_InsTrf3MountOrientation;
            }

            set
            {
                m_InsTrf3MountOrientation = value;
            }
        }

        internal static string InsTrf3KindCode
        {
            get
            {
                return m_InsTrf3KindCode;
            }

            set
            {
                m_InsTrf3KindCode = value;
            }
        }

        internal static string InsTrf3KindValue
        {
            get
            {
                return m_InsTrf3KindValue;
            }

            set
            {
                m_InsTrf3KindValue = value;
            }
        }

        internal static string InsTrf3PriVoltCode
        {
            get
            {
                return m_InsTrf3PriVoltCode;
            }

            set
            {
                m_InsTrf3PriVoltCode = value;
            }
        }

        internal static string InsTrf3PriVoltValue
        {
            get
            {
                return m_InsTrf3PriVoltValue;
            }

            set
            {
                m_InsTrf3PriVoltValue = value;
            }
        }

        internal static string InsTrf3SecVoltCode
        {
            get
            {
                return m_InsTrf3SecVoltCode;
            }

            set
            {
                m_InsTrf3SecVoltCode = value;
            }
        }

        internal static string InsTrf3SecVoltValue
        {
            get
            {
                return m_InsTrf3SecVoltValue;
            }

            set
            {
                m_InsTrf3SecVoltValue = value;
            }
        }

        internal static string RemTrf1CompanyNumber
        {
            get
            {
                return m_RemTrf1CompanyNumber;
            }

            set
            {
                m_RemTrf1CompanyNumber = value;
            }
        }

        internal static string RemTrf1TSN
        {
            get
            {
                return m_RemTrf1TSN;
            }

            set
            {
                m_RemTrf1TSN = value;
            }
        }

        internal static double RemTrf1KvaSize
        {
            get
            {
                return m_RemTrf1KvaSize;
            }

            set
            {
                m_RemTrf1KvaSize = value;
            }
        }


        internal static string RemTrf1PhaseCode
        {
            get
            {
                return m_RemTrf1PhaseCode;
            }

            set
            {
                m_RemTrf1PhaseCode = value;
            }
        }

        internal static string RemTrf1TypeCode
        {
            get
            {
                return m_RemTrf1TypeCode;
            }

            set
            {
                m_RemTrf1TypeCode = value;
            }
        }

        internal static string RemTrf1MountCode
        {
            get
            {
                return m_RemTrf1MountCode;
            }

            set
            {
                m_RemTrf1MountCode = value;
            }
        }

        internal static string RemTrf1KindCode
        {
            get
            {
                return m_RemTrf1KindCode;
            }

            set
            {
                m_RemTrf1KindCode = value;
            }
        }

        internal static string RemTrf1PriVoltCode
        {
            get
            {
                return m_RemTrf1PriVoltCode;
            }

            set
            {
                m_RemTrf1PriVoltCode = value;
            }
        }

        internal static string RemTrf1SecVoltCode
        {
            get
            {
                return m_RemTrf1SecVoltCode;
            }

            set
            {
                m_RemTrf1SecVoltCode = value;
            }
        }


        internal static string RemTrf2CompanyNumber
        {
            get
            {
                return m_RemTrf2CompanyNumber;
            }

            set
            {
                m_RemTrf2CompanyNumber = value;
            }
        }

        internal static string RemTrf2TSN
        {
            get
            {
                return m_RemTrf2TSN;
            }

            set
            {
                m_RemTrf2TSN = value;
            }
        }

        internal static double RemTrf2KvaSize
        {
            get
            {
                return m_RemTrf2KvaSize;
            }

            set
            {
                m_RemTrf2KvaSize = value;
            }
        }


        internal static string RemTrf2PhaseCode
        {
            get
            {
                return m_RemTrf2PhaseCode;
            }

            set
            {
                m_RemTrf2PhaseCode = value;
            }
        }

        internal static string RemTrf2TypeCode
        {
            get
            {
                return m_RemTrf2TypeCode;
            }

            set
            {
                m_RemTrf2TypeCode = value;
            }
        }

        internal static string RemTrf2MountCode
        {
            get
            {
                return m_RemTrf2MountCode;
            }

            set
            {
                m_RemTrf2MountCode = value;
            }
        }

        internal static string RemTrf2KindCode
        {
            get
            {
                return m_RemTrf2KindCode;
            }

            set
            {
                m_RemTrf2KindCode = value;
            }
        }

        internal static string RemTrf2PriVoltCode
        {
            get
            {
                return m_RemTrf2PriVoltCode;
            }

            set
            {
                m_RemTrf2PriVoltCode = value;
            }
        }

        internal static string RemTrf2SecVoltCode
        {
            get
            {
                return m_RemTrf2SecVoltCode;
            }

            set
            {
                m_RemTrf2SecVoltCode = value;
            }
        }

        internal static string RemTrf3CompanyNumber
        {
            get
            {
                return m_RemTrf3CompanyNumber;
            }

            set
            {
                m_RemTrf3CompanyNumber = value;
            }
        }

        internal static string RemTrf3TSN
        {
            get
            {
                return m_RemTrf3TSN;
            }

            set
            {
                m_RemTrf3TSN = value;
            }
        }

        internal static double RemTrf3KvaSize
        {
            get
            {
                return m_RemTrf3KvaSize;
            }

            set
            {
                m_RemTrf3KvaSize = value;
            }
        }


        internal static string RemTrf3PhaseCode
        {
            get
            {
                return m_RemTrf3PhaseCode;
            }

            set
            {
                m_RemTrf3PhaseCode = value;
            }
        }

        internal static string RemTrf3TypeCode
        {
            get
            {
                return m_RemTrf3TypeCode;
            }

            set
            {
                m_RemTrf3TypeCode = value;
            }
        }

        internal static string RemTrf3MountCode
        {
            get
            {
                return m_RemTrf3MountCode;
            }

            set
            {
                m_RemTrf3MountCode = value;
            }
        }

        internal static string RemTrf3KindCode
        {
            get
            {
                return m_RemTrf3KindCode;
            }

            set
            {
                m_RemTrf3KindCode = value;
            }
        }

        internal static string RemTrf3PriVoltCode
        {
            get
            {
                return m_RemTrf3PriVoltCode;
            }

            set
            {
                m_RemTrf3PriVoltCode = value;
            }
        }

        internal static string RemTrf3SecVoltCode
        {
            get
            {
                return m_RemTrf3SecVoltCode;
            }

            set
            {
                m_RemTrf3SecVoltCode = value;
            }
        }

        internal static int VoltageRegulatorFID
        {
            get
            {
                return m_VoltageRegulatorFID;
            }

            set
            {
                m_VoltageRegulatorFID = value;
            }
        }

        #endregion

        /// <summary>
        /// Initializes the TransactionDEIS variables.
        /// </summary>
        internal static void Initialize()
        {
            m_TranID = string.Empty;
            m_Requestor = string.Empty;
            m_TranNumber = 0;
            m_TranDate = string.Empty;
            m_TranCode = string.Empty;
            m_BankFLN = string.Empty;
            m_StructureID = string.Empty;
            m_TranStatus = string.Empty;
            m_TranMessage = string.Empty;
            m_WrNumber = string.Empty;
            m_ServArea = string.Empty;
            m_Yard = string.Empty;
            m_UserID = string.Empty;
            m_ConnSec = string.Empty;
            m_ExistingBank = string.Empty;
            m_WiringConfigCode = string.Empty;
            m_Location = string.Empty;
            m_InsTrf1CompanyNumber = string.Empty;
            m_InsTrf1TSN = string.Empty;
            m_InsTrf1CU = string.Empty;
            m_InsTrf1KvaSize = 0;
            m_InsTrf1PhaseCode = string.Empty;
            m_InsTrf1TypeCode = string.Empty;
            m_InsTrf1TypeValue = string.Empty;
            m_InsTrf1MountCode = string.Empty;
            m_InsTrf1MountValue = string.Empty;
            m_InsTrf1MountOrientation = string.Empty;
            m_InsTrf1KindCode = string.Empty;
            m_InsTrf1KindValue = string.Empty;
            m_InsTrf1PriVoltCode = string.Empty;
            m_InsTrf1PriVoltValue = string.Empty;
            m_InsTrf1SecVoltCode = string.Empty;
            m_InsTrf1SecVoltValue = string.Empty;
            m_InsTrf2CompanyNumber = string.Empty;
            m_InsTrf2TSN = string.Empty;
            m_InsTrf2CU = string.Empty;
            m_InsTrf2KvaSize = 0;
            m_InsTrf2PhaseCode = string.Empty;
            m_InsTrf2TypeCode = string.Empty;
            m_InsTrf2TypeValue = string.Empty;
            m_InsTrf2MountCode = string.Empty;
            m_InsTrf2MountValue = string.Empty;
            m_InsTrf2MountOrientation = string.Empty;
            m_InsTrf2KindCode = string.Empty;
            m_InsTrf2KindValue = string.Empty;
            m_InsTrf2PriVoltCode = string.Empty;
            m_InsTrf2PriVoltValue = string.Empty;
            m_InsTrf2SecVoltCode = string.Empty;
            m_InsTrf2SecVoltValue = string.Empty;
            m_InsTrf3CompanyNumber = string.Empty;
            m_InsTrf3TSN = string.Empty;
            m_InsTrf3CU = string.Empty;
            m_InsTrf3KvaSize = 0;
            m_InsTrf3PhaseCode = string.Empty;
            m_InsTrf3TypeCode = string.Empty;
            m_InsTrf3TypeValue = string.Empty;
            m_InsTrf3MountCode = string.Empty;
            m_InsTrf3MountValue = string.Empty;
            m_InsTrf3MountType = string.Empty;
            m_InsTrf3MountOrientation = string.Empty;
            m_InsTrf3KindCode = string.Empty;
            m_InsTrf3KindValue = string.Empty;
            m_InsTrf3PriVoltCode = string.Empty;
            m_InsTrf3PriVoltValue = string.Empty;
            m_InsTrf3SecVoltCode = string.Empty;
            m_InsTrf3SecVoltValue = string.Empty;
            m_RemTrf1CompanyNumber = string.Empty;
            m_RemTrf1TSN = string.Empty;
            m_RemTrf1KvaSize = 0;
            m_RemTrf1PhaseCode = string.Empty;
            m_RemTrf1TypeCode = string.Empty;
            m_RemTrf1MountCode = string.Empty;
            m_RemTrf1KindCode = string.Empty;
            m_RemTrf1PriVoltCode = string.Empty;
            m_RemTrf1SecVoltCode = string.Empty;
            m_RemTrf2CompanyNumber = string.Empty;
            m_RemTrf2TSN = string.Empty;
            m_RemTrf2KvaSize = 0;
            m_RemTrf2PhaseCode = string.Empty;
            m_RemTrf2TypeCode = string.Empty;
            m_RemTrf2MountCode = string.Empty;
            m_RemTrf2KindCode = string.Empty;
            m_RemTrf2PriVoltCode = string.Empty;
            m_RemTrf2SecVoltCode = string.Empty;
            m_RemTrf3CompanyNumber = string.Empty;
            m_RemTrf3TSN = string.Empty;
            m_RemTrf3KvaSize = 0;
            m_RemTrf3PhaseCode = string.Empty;
            m_RemTrf3TypeCode = string.Empty;
            m_RemTrf3MountCode = string.Empty;
            m_RemTrf3KindCode = string.Empty;
            m_RemTrf3PriVoltCode = string.Empty;
            m_RemTrf3SecVoltCode = string.Empty;

            m_VoltageRegulatorFID = 0;

            TransformerProperties.Node1 = 0;
            TransformerProperties.Node2 = 0;

            InitializeDataStructures();
        }

        /// <summary>
        /// Initializes the data structures.
        /// </summary>
        internal static void InitializeDataStructures()
        {
            StructureProperties.FNO = 0;
            StructureProperties.FID = 0;
            StructureProperties.PrimaryGraphicCNO = 0;
            StructureProperties.FeatureState = string.Empty;
            StructureProperties.Orientation = string.Empty;

            TransformerProperties.FNO = 0;
            TransformerProperties.FID = 0;
            TransformerProperties.PrimaryGraphicCNO = 0;
            TransformerProperties.LabelCNO = 0;
            TransformerProperties.BankCNO = 0;
            TransformerProperties.UnitCNO = 0;
            TransformerProperties.BankTable = string.Empty;
            TransformerProperties.UnitTable = string.Empty;
            TransformerProperties.UsePhaseCode = false;
            TransformerProperties.ExistingPhases = string.Empty;
            TransformerProperties.PhaseCount = 0;
            TransformerProperties.InsTrf1PhaseValue = string.Empty;
            TransformerProperties.InsTrf2PhaseValue = string.Empty;
            TransformerProperties.InsTrf3PhaseValue = string.Empty;
            TransformerProperties.PrimaryWiringConfiguration = string.Empty;
            TransformerProperties.SecondaryWiringConfiguration = string.Empty;
            TransformerProperties.FeatureState = string.Empty;
            TransformerProperties.RemoveBank = false;

            PrimaryProperties.FNO = 0;
            PrimaryProperties.FID = 0;
            PrimaryProperties.PrimaryGraphicCNO = 0;
            PrimaryProperties.BankCNO = 0;
            PrimaryProperties.WireCNO = 0;
            PrimaryProperties.Phases = string.Empty;
            PrimaryProperties.PhaseCount = 0;
            PrimaryProperties.ProtectiveDeviceID = 0;
            PrimaryProperties.ConnectNode = 0;

            SecondaryProperties.FNO = 0;
            SecondaryProperties.FID = 0;
            SecondaryProperties.PrimaryGraphicCNO = 0;
            SecondaryProperties.FeatureState = string.Empty;
        }
    }
}
