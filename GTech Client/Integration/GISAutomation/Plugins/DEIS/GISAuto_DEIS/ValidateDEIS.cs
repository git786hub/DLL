using ADODB;
using Intergraph.GTechnology.API;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Linq;

namespace GTechnology.Oncor.CustomAPI
{
    internal class ValidateDEIS
    {
        private IGTDataContext m_DataContext = null;

        public ValidateDEIS(IGTDataContext dataContext)
        {
            m_DataContext = dataContext;
        }

        /// <summary>
        /// Entry point into class to validate the transaction.
        /// </summary>
        /// <returns>Boolean indicating status</returns>
        internal bool ValidateTransaction()
        {
            bool returnValue = false;

            try
            {
                // Validate transaction code
                if (!TransactionDEIS.VALID_TRANSACTION_CODES.Contains(TransactionDEIS.TransactionCode))
                {
                    TransactionDEIS.TransactionStatus = TransactionDEIS.TRANS_STATUS_FAILED;
                    TransactionDEIS.TransactionMessage = "Invalid Transaction Code";
                    return false;
                }

                if (!FLNtoStructureID())
                {                    
                    return false;
                }

                // Ensure any non-empty Company Numbers are seven-digits.
                if (TransactionDEIS.InsTrf1CompanyNumber.Length > 0) TransactionDEIS.InsTrf1CompanyNumber = TransactionDEIS.InsTrf1CompanyNumber.PadLeft(7, '0');
                if (TransactionDEIS.InsTrf2CompanyNumber.Length > 0) TransactionDEIS.InsTrf2CompanyNumber = TransactionDEIS.InsTrf2CompanyNumber.PadLeft(7, '0');
                if (TransactionDEIS.InsTrf3CompanyNumber.Length > 0) TransactionDEIS.InsTrf3CompanyNumber = TransactionDEIS.InsTrf3CompanyNumber.PadLeft(7, '0');
                if(TransactionDEIS.RemTrf1CompanyNumber.Length > 0) TransactionDEIS.RemTrf1CompanyNumber = TransactionDEIS.RemTrf1CompanyNumber.PadLeft(7, '0');
                if (TransactionDEIS.RemTrf2CompanyNumber.Length > 0) TransactionDEIS.RemTrf2CompanyNumber = TransactionDEIS.RemTrf2CompanyNumber.PadLeft(7, '0');
                if (TransactionDEIS.RemTrf3CompanyNumber.Length > 0) TransactionDEIS.RemTrf3CompanyNumber = TransactionDEIS.RemTrf3CompanyNumber.PadLeft(7, '0');

                // Call transaction specific validation
                switch (TransactionDEIS.TransactionCode)
                {
                    case "I":
                        if (!InstallValidationTransformer())
                        {
                            return false;
                        }
                        break;
                    case "R":
                        if (!RemovalValidationTransformer())
                        {
                            return false;
                        }
                        break;
                    case "C":
                        if (!ChangeoutValidationTransformer(true))
                        {
                            return false;
                        }
                        break;
                    case "D":
                        // If not Voltage Regulator then validate Transformer
                        if (TransactionDEIS.InsTrf1TypeCode != "04" && TransactionDEIS.InsTrf1KindCode != "4")
                        {
                            if (!UpdateValidationTransformer())
                            {
                                return false;
                            }
                        }
                        else
                        {
                            // Else, validate Voltage Regulator
                            if (!UpdateValidationVoltageRegulator())
                            {
                                return false;
                            }                            
                        }
                        
                        break;
                }

                returnValue = true;
            }
            catch (Exception ex)
            {
                TransactionDEIS.TransactionStatus = TransactionDEIS.TRANS_STATUS_FAILED;
                TransactionDEIS.TransactionMessage = "Error validating transaction: " + ex.Message;
                returnValue = false;
            }

            return returnValue;
        }

        /// <summary>
        /// Validates and formats the FLN.
        /// </summary>
        /// <returns>Boolean indicating status</returns>
        private bool FLNtoStructureID()
        {
            bool returnValue = false;

            try
            {                 
                if (TransactionDEIS.BankFLN.Length < 10 || TransactionDEIS.BankFLN.Length > 16 || TransactionDEIS.BankFLN.Length == 13)
                {
                    TransactionDEIS.TransactionStatus = TransactionDEIS.TRANS_STATUS_FAILED;
                    TransactionDEIS.TransactionMessage = "Bank FLN is invalid";
                    return false;
                }
                
                if (!Regex.IsMatch(TransactionDEIS.BankFLN.Substring(0, 3), @"^[0-9]+$") && TransactionDEIS.BankFLN.Substring(6, 1) != "O")
                {
                    // Unknown length. 3rd character is not a number (0 – 9). 7th character is not an uppercase “O”. Split between 6th and 7th characters
                    TransactionDEIS.StructureID = TransactionDEIS.BankFLN.Substring(0, 6) + "-" + TransactionDEIS.BankFLN.Substring(6);
                }
                else if (!Regex.IsMatch(TransactionDEIS.BankFLN.Substring(0, 3), @"^[0-9]+$") && TransactionDEIS.BankFLN.Substring(6, 1) == "O")
                {
                    // Unknown length. 3rd character is not a number (0 – 9), 7th character is an uppercase “O”. 
                    // Replace 7th character with a zero(0).
                    TransactionDEIS.StructureID = TransactionDEIS.BankFLN.Remove(6, 1).Insert(6, "0");
                    // Split between 6th and 7th characters
                    TransactionDEIS.StructureID = TransactionDEIS.StructureID.Substring(0, 6) + "-" + TransactionDEIS.StructureID.Substring(6);
                }
                else if (TransactionDEIS.BankFLN.Length == 16)
                {
                    // 16 characters. If 8th character is a hyphen then no conversion is needed. Else log an error.
                    if (TransactionDEIS.BankFLN.Substring(7, 1) == "-")
                    {
                        // No conversion is needed
                        TransactionDEIS.StructureID = TransactionDEIS.BankFLN;
                    }
                    else
                    {
                        TransactionDEIS.TransactionMessage = "Bank FLN is invalid";
                        return false;
                    }                    
                }
                else if (TransactionDEIS.BankFLN.Length == 15)
                {
                    // 15 characters. Split between 7th and 8th characters
                    TransactionDEIS.StructureID = TransactionDEIS.BankFLN.Substring(0, 7) + "-" + TransactionDEIS.BankFLN.Substring(7);
                }
                else if (TransactionDEIS.BankFLN.Length == 14 && TransactionDEIS.BankFLN.Substring(7, 3) == "000")
                {
                    // 14 characters. Three zeros occupy positions 8 through 10. 
                    // Remove the three zeros. 
                    TransactionDEIS.StructureID = TransactionDEIS.BankFLN.Remove(7, 3);
                    // Split between 7th and 8th remaining characters
                    TransactionDEIS.StructureID = TransactionDEIS.StructureID.Substring(0, 7) + "-" + TransactionDEIS.StructureID.Substring(7);
                }
                else if (TransactionDEIS.BankFLN.Length == 14)
                {
                    // 14 characters. Three zeros do not occupy positions 8 through 10. Split between 7th and 8th characters
                    TransactionDEIS.StructureID = TransactionDEIS.BankFLN.Substring(0, 7) + "-" + TransactionDEIS.BankFLN.Substring(7);
                }
                else if (TransactionDEIS.BankFLN.Length == 12)
                {
                    // 12 characters. Split between 6th and 7th characters
                    TransactionDEIS.StructureID = TransactionDEIS.BankFLN.Substring(0, 6) + "-" + TransactionDEIS.BankFLN.Substring(6);
                }
                else if (TransactionDEIS.BankFLN.Length == 10)
                {
                    // 10 characters. Split between 5th and 6th characters
                    TransactionDEIS.StructureID = TransactionDEIS.BankFLN.Substring(0, 5) + "-" + TransactionDEIS.BankFLN.Substring(5);
                }
                else if (TransactionDEIS.BankFLN.Length == 11)
                {
                    // 11 characters. Split between 7th and 8th characters
                    TransactionDEIS.StructureID = TransactionDEIS.BankFLN.Substring(0, 7) + "-" + TransactionDEIS.BankFLN.Substring(7);
                }

                string sqlStatement = string.Format("Update {0} set STRUCTURE_ID = ? where {1} = ?",
                                                    TransactionDEIS.TABLE_DEIS_TRANSACTION, TransactionDEIS.FIELD_DEIS_TRANSACTION_NUMBER);
                int recordsAffected = 0;
                m_DataContext.Execute(sqlStatement, out recordsAffected, (int)CommandTypeEnum.adCmdText + (int)ExecuteOptionEnum.adExecuteNoRecords,
                                      TransactionDEIS.StructureID, TransactionDEIS.TransactionNumber);
                m_DataContext.Execute("commit", out recordsAffected, (int)CommandTypeEnum.adCmdText);

                returnValue = true;
            }
            catch (Exception ex)
            {
                TransactionDEIS.TransactionStatus = TransactionDEIS.TRANS_STATUS_FAILED;
                TransactionDEIS.TransactionMessage = "Error processing Bank FLN: " + ex.Message;
                returnValue = false;
            }

            return returnValue;
        }

        /// <summary>
        /// Performs validation for Transformer installation.
        /// </summary>
        /// <returns>Boolean indicating status</returns>
        private bool InstallValidationTransformer()
        {
            bool returnValue = false;

            try
            {
                // Validate if single Structure ID exists
                if (!ValidateStructureID(false))
                {
                    return false;
                }

                // Validate the transaction codes - Phase, Type, Mount, Kind, Voltages
                if (!ValidateCodes())
                {
                    return false;
                }

                // Validate CU
                if (!ValidateCU())
                {
                    return false;
                }

                // Determine the type of Transformer - OH, UG, Auto, Network
                if (TransactionDEIS.InsTrf1TypeCode == "07" || TransactionDEIS.InsTrf2TypeCode == "07" || TransactionDEIS.InsTrf3TypeCode == "07")
                {
                    // Auto Transformer
                    TransactionDEIS.TransformerProperties.FNO = TransactionDEIS.FNO_AUTOTRANSFORMER;
                }
                else if (TransactionDEIS.InsTrf1MountOrientation == "OH" || TransactionDEIS.InsTrf2MountOrientation == "OH" || TransactionDEIS.InsTrf3MountOrientation == "OH")
                {
                    if (TransactionDEIS.InsTrf1TypeCode == "05" || TransactionDEIS.InsTrf2TypeCode == "05" || TransactionDEIS.InsTrf3TypeCode == "05")
                    {
                        // OH Network Transformer
                        TransactionDEIS.TransformerProperties.FNO = TransactionDEIS.FNO_TRANSFORMER_OH_NETWORK;
                    }
                    else
                    {
                        // OH Transformer
                        TransactionDEIS.TransformerProperties.FNO = TransactionDEIS.FNO_TRANSFORMER_OH;
                    }
                }
                else
                {
                    if (TransactionDEIS.InsTrf1TypeCode == "05" || TransactionDEIS.InsTrf2TypeCode == "05" || TransactionDEIS.InsTrf3TypeCode == "05")
                    {
                        // UG Network Transformer
                        TransactionDEIS.TransformerProperties.FNO = TransactionDEIS.FNO_TRANSFORMER_UG_NETWORK;
                    }
                    else
                    {
                        // UG Transformer
                        TransactionDEIS.TransformerProperties.FNO = TransactionDEIS.FNO_TRANSFORMER_UG;
                    }
                }

                // Set the transformer feature properties (G3E_CNO, bank/unit tables, ...) based on the TransactionDEIS.TransformerProperties.FNO
                if (!SetTransformerFeatureProperties())
                {
                    return false;
                }

                // Validate that all transformers in a single installation have the same orientation
                string tempString = TransactionDEIS.InsTrf1MountOrientation + TransactionDEIS.InsTrf2MountOrientation + TransactionDEIS.InsTrf3MountOrientation;
                if (tempString.Contains("UG") && tempString.Contains("OH"))
                {
                    TransactionDEIS.TransactionStatus = TransactionDEIS.TRANS_STATUS_FAILED;
                    TransactionDEIS.TransactionMessage = "Install transaction includes multiple transformers with different orientations (e.g. OH, PM, Auto).";
                    return false;
                }

                // Get existing Transformer and set properties. FID will already be set for a PPX or ABX change-out so existing transformer has already been determined.
                if (TransactionDEIS.TransformerProperties.FID == 0)
                {
                    if (!ValidateExistingTransformerInstall())
                    {
                        return false;
                    }
                }                

                // Count the number of Transformer phases and determine phasing rule
                if (!ValidateTransformerPhases())
                {
                    return false;
                }

                // The mount orientation for the transformer must match the identified Structure orientation.
                if (TransactionDEIS.InsTrf1MountOrientation != TransactionDEIS.StructureProperties.Orientation)
                {
                    TransactionDEIS.TransactionStatus = TransactionDEIS.TRANS_STATUS_WARNING;
                    TransactionDEIS.TransactionMessage = string.Format("The install transaction indicates an {0} Transformer, but the located Structure is not an {0} Structure.", TransactionDEIS.InsTrf1MountOrientation);
                }

                if (TransactionDEIS.TransformerProperties.FID == 0)
                {
                    // Get the Primary Conductors at the Structure ID
                    if (!ValidatePrimary())
                    {
                        return false;
                    }

                    // Validate that Secondary exists if CONN_SEC is set to Y
                    if (TransactionDEIS.SecondaryConnection == "Y")
                    {
                        if (!ValidateSecondary())
                        {
                            return false;
                        }
                    }
                }
                
                // Validate the wiring configuration
                if (!ValidateWiringConfiguration())
                {
                    return false;
                }

                returnValue = true;
            }
            catch (Exception ex)
            {
                TransactionDEIS.TransactionStatus = TransactionDEIS.TRANS_STATUS_FAILED;
                TransactionDEIS.TransactionMessage = "Error validating Transformer to install: " + ex.Message;
                returnValue = false;
            }

            return returnValue;
        }

        /// <summary>
        /// Performs validation for Transformer removal.
        /// </summary>
        /// <returns>Boolean indicating status</returns>
        private bool RemovalValidationTransformer()
        {
            bool returnValue = false;

            try
            {
                // Validate if single Structure ID exists
                if (!ValidateStructureID(true))
                {
                    return false;
                }

                // Validate existing Transformer
                if (!ValidateExistingTransformer(TransactionDEIS.RemTrf1CompanyNumber, TransactionDEIS.RemTrf2CompanyNumber, TransactionDEIS.RemTrf3CompanyNumber))
                {
                    return false;
                }

                if (!TransactionDEIS.TransformerProperties.RemoveBank && TransactionDEIS.TransactionCode == "R")
                {
                    TransactionDEIS.TransformerProperties.PhaseCount += - TransactionDEIS.RemTrf1PhaseCode.Length - TransactionDEIS.RemTrf2PhaseCode.Length - TransactionDEIS.RemTrf3PhaseCode.Length;

                    // Validate the wiring configuration
                    if (!ValidateWiringConfiguration())
                    {
                        return false;
                    }
                }

                returnValue = true;
            }
            catch (Exception ex)
            {
                TransactionDEIS.TransactionStatus = TransactionDEIS.TRANS_STATUS_FAILED;
                TransactionDEIS.TransactionMessage = "Error validating Transformer to remove: " + ex.Message;
                returnValue = false;
            }

            return returnValue;
        }

        /// <summary>
        /// Performs validation for Transformer change-out.
        /// </summary>
        /// <param name="removal">Boolean indicating which validation to execute. TRUE = general and removal validation; FALSE = installation validation</param>
        /// <returns>Boolean indicating status</returns>
        public bool ChangeoutValidationTransformer(Boolean removal)
        {
            bool returnValue = false;

            try
            {
                if (removal)
                {
                    if (!RemovalValidationTransformer())
                    {
                        return false;
                    }
                }
                else
                {
                    if (!InstallValidationTransformer())
                    {
                        return false;
                    }
                }

                returnValue = true;
            }
            catch (Exception ex)
            {
                TransactionDEIS.TransactionStatus = TransactionDEIS.TRANS_STATUS_FAILED;
                TransactionDEIS.TransactionMessage = "Error validating Transformer to changeout: " + ex.Message;
                returnValue = false;
            }

            return returnValue;
        }

        /// <summary>
        /// Performs validation for Transformer update.
        /// </summary>
        /// <returns>Boolean indicating status</returns>
        private bool UpdateValidationTransformer()
        {
            bool returnValue = false;

            try
            {
                // Validate if single Structure ID exists
                if (!ValidateStructureID(false))
                {
                    return false;
                }

                // Validate the transaction codes - Phase, Type, Mount, Kind, Voltages
                if (!ValidateCodes())
                {
                    return false;
                }

                // Validate CU
                if (!ValidateCU())
                {
                    return false;
                }

                // Validate existing Transformer
                if (!ValidateExistingTransformer(TransactionDEIS.InsTrf1CompanyNumber, TransactionDEIS.InsTrf2CompanyNumber, TransactionDEIS.InsTrf3CompanyNumber))
                {
                    return false;
                }

                returnValue = true;
            }
            catch (Exception ex)
            {
                TransactionDEIS.TransactionStatus = TransactionDEIS.TRANS_STATUS_FAILED;
                TransactionDEIS.TransactionMessage = "Error validating Transformer to update: " + ex.Message;
                returnValue = false;
            }

            return returnValue;
        }

        /// <summary>
        /// Performs validation for Voltage Regulator update.
        /// </summary>
        /// <returns>Boolean indicating status</returns>
        private bool UpdateValidationVoltageRegulator()
        {
            bool returnValue = false;

            try
            {
                // Validate if single Structure ID exists
                if (!ValidateStructureID(false))
                {
                    return false;
                }

                // Validate the transaction codes - Phase, Type, Mount, Kind, Voltages
                if (!ValidateCodes())
                {
                    return false;
                }

                // Validate existing Voltage Regulator
                if (!ValidateExistingVoltageRegulator())
                {
                    return false;
                }

                returnValue = true;
            }
            catch (Exception ex)
            {
                TransactionDEIS.TransactionStatus = TransactionDEIS.TRANS_STATUS_FAILED;
                TransactionDEIS.TransactionMessage = "Error validating Transformer to update: " + ex.Message;
                returnValue = false;
            }

            return returnValue;
        }

        /// <summary>
        /// Validates a CU.
        /// </summary>
        /// <returns>Boolean indicating status</returns>
        private bool ValidateCU()
        {
            bool returnValue = false;

            try
            {
                // Validate that TSN exists in CU library. 
                string sqlStatement = "select cu_id, material_id from culib_unitmaterial where material_id in (?,?,?)";
                int recordsAffected = 0;
                ADODB.Recordset codeRS = m_DataContext.Execute(sqlStatement, out recordsAffected, (int)CommandTypeEnum.adCmdText, TransactionDEIS.InsTrf1TSN, TransactionDEIS.InsTrf2TSN, TransactionDEIS.InsTrf3TSN);

                if (TransactionDEIS.InsTrf1TSN.Length > 0)
                {
                    codeRS.Filter = "material_id = '" + TransactionDEIS.InsTrf1TSN + "'";
                    if (codeRS.RecordCount == 0)
                    {
                        TransactionDEIS.TransactionStatus = TransactionDEIS.TRANS_STATUS_FAILED;
                        TransactionDEIS.TransactionMessage = string.Format("CU is not found for the supplied TSN: {0}.", TransactionDEIS.InsTrf1TSN);
                        return false;
                    }
                    else if (codeRS.RecordCount > 1)
                    {
                        TransactionDEIS.TransactionStatus = TransactionDEIS.TRANS_STATUS_FAILED;
                        TransactionDEIS.TransactionMessage = string.Format("Multiple CUs found for the supplied TSN: {0}.", TransactionDEIS.InsTrf1TSN);
                        return false;
                    }
                    else
                    {
                        codeRS.MoveFirst();
                        TransactionDEIS.InsTrf1CU = codeRS.Fields["cu_id"].Value.ToString();
                    }
                }

                if (TransactionDEIS.InsTrf2TSN.Length > 0)
                {
                    codeRS.Filter = "material_id = '" + TransactionDEIS.InsTrf2TSN + "'";
                    if (codeRS.RecordCount == 0)
                    {
                        TransactionDEIS.TransactionStatus = TransactionDEIS.TRANS_STATUS_FAILED;
                        TransactionDEIS.TransactionMessage = string.Format("CU is not found for the supplied TSN {0}.", TransactionDEIS.InsTrf2TSN);
                        return false;
                    }
                    else if (codeRS.RecordCount > 1)
                    {
                        TransactionDEIS.TransactionStatus = TransactionDEIS.TRANS_STATUS_FAILED;
                        TransactionDEIS.TransactionMessage = string.Format("Multiple CUs found for the supplied TSN {0}.", TransactionDEIS.InsTrf2TSN);
                        return false;
                    }
                    else
                    {
                        codeRS.MoveFirst();
                        TransactionDEIS.InsTrf2CU = codeRS.Fields["cu_id"].Value.ToString();
                    }
                }

                if (TransactionDEIS.InsTrf3TSN.Length > 0)
                {
                    codeRS.Filter = "material_id = '" + TransactionDEIS.InsTrf3TSN + "'";
                    if (codeRS.RecordCount == 0)
                    {
                        TransactionDEIS.TransactionStatus = TransactionDEIS.TRANS_STATUS_FAILED;
                        TransactionDEIS.TransactionMessage = string.Format("CU is not found for the supplied TSN {0}.", TransactionDEIS.InsTrf3TSN);
                        return false;
                    }
                    else if (codeRS.RecordCount > 1)
                    {
                        TransactionDEIS.TransactionStatus = TransactionDEIS.TRANS_STATUS_FAILED;
                        TransactionDEIS.TransactionMessage = string.Format("Multiple CUs found for the supplied TSN {0}.", TransactionDEIS.InsTrf3TSN);
                        return false;
                    }
                    else
                    {
                        codeRS.MoveFirst();
                        TransactionDEIS.InsTrf3CU = codeRS.Fields["cu_id"].Value.ToString();
                    }
                }

                returnValue = true;
            }
            catch (Exception ex)
            {
                TransactionDEIS.TransactionStatus = TransactionDEIS.TRANS_STATUS_FAILED;
                TransactionDEIS.TransactionMessage = "Error validating CU: " + ex.Message;
                returnValue = false;
            }

            return returnValue;
        }

        /// <summary>
        /// Check if one Structure is a replacement for the other.
        /// </summary>
        /// <param name="featureRS">The recordset containing two fids having the same unique identifier</param>
        /// <param name="installFNO">The G3E_FNO of the feature in the install state</param>
        /// <param name="installFID">The G3E_FID of the feature in the install state</param>
        /// <param name="removeFNO">The G3E_FNO of the feature in the remove state</param>
        /// <param name="removeFID">The G3E_FID of the feature in the remove state</param>
        /// <param name="replacement">Indicates if the one of the features is the replacement for the other</param>
        /// <returns>Boolean indicating status</returns>
        private bool CheckIfReplacement(ADODB.Recordset featureRS, out short installFNO, out Int32 installFID, out short removeFNO, out Int32 removeFID, out bool replacement)
        {
            bool returnValue = false;

            installFID = 0;
            installFNO = 0;
            removeFID = 0;
            removeFNO = 0;
            replacement = false;

            try
            {
                // Check if one structure is a replacement for the other 
                featureRS.MoveFirst();
                int fid1 = Convert.ToInt32(featureRS.Fields["G3E_FID"].Value);
                short fno1 = Convert.ToInt16(featureRS.Fields["G3E_FNO"].Value);
                string featureState1 = featureRS.Fields["FEATURE_STATE_C"].Value.ToString();
                int replacedFID1 = 0;
                if (!Convert.IsDBNull(featureRS.Fields["REPLACED_FID"].Value))
                {
                    replacedFID1 = Convert.ToInt32(featureRS.Fields["REPLACED_FID"].Value);
                }

                featureRS.MoveNext();
                int fid2 = Convert.ToInt32(featureRS.Fields["G3E_FID"].Value);
                short fno2 = Convert.ToInt16(featureRS.Fields["G3E_FNO"].Value);
                string featureState2 = featureRS.Fields["FEATURE_STATE_C"].Value.ToString();
                int replacedFID2 = -1;
                if (!Convert.IsDBNull(featureRS.Fields["REPLACED_FID"].Value))
                {
                    replacedFID2 = Convert.ToInt32(featureRS.Fields["REPLACED_FID"].Value);
                }

                // If one structure is in the PPI, ABI, or INI state and the other structure is in the PPR, ABR, or OSR state
                // and the replaced FID on one structure references the g3e_fid of the other,
                // then select the appropriate structure for the transaction.
                if ((TransactionDEIS.STATES_INSTALLED.Contains(featureState1) && TransactionDEIS.STATES_REMOVED.Contains(featureState2)
                    || TransactionDEIS.STATES_REMOVED.Contains(featureState1) && TransactionDEIS.STATES_INSTALLED.Contains(featureState2))
                    && (fid1 == replacedFID2 || fid2 == replacedFID1))
                {
                    replacement = true;

                    if (TransactionDEIS.STATES_INSTALLED.Contains(featureState1))
                    {
                        installFID = fid1;
                        installFNO = fno1;
                        removeFID = fid2;
                        removeFNO = fno2;
                    }
                    else
                    {
                        installFID = fid2;
                        installFNO = fno2;
                        removeFID = fid1;
                        removeFNO = fno1;
                    }
                }
                else
                {
                    replacement = false;
                }

                returnValue = true;
            }
            catch (Exception ex)
            {
                TransactionDEIS.TransactionStatus = TransactionDEIS.TRANS_STATUS_FAILED;
                TransactionDEIS.TransactionMessage = "Error checking if feature is a replacement: " + ex.Message;
                returnValue = false;
            }

            return returnValue;
        }

        /// <summary>
        /// Validates that the Structure ID exists. Gets Structures properties.
        /// </summary>
        /// <param name="removal">Boolean indicating which structure to return if two structures are found. TRUE = PPR, ABR, OSR; FALSE = PPI, ABI, INI</param>
        /// <returns>Boolean indicating status</returns>
        private bool ValidateStructureID(bool removal)
        {
            bool returnValue = false;

            try
            {
                // Query for Structure ID
                string sqlStatement = "select distinct comm.g3e_fno, comm.g3e_fid, comm.feature_state_c, comm.replaced_fid, own.g3e_ownertriggeringcno " +
                                      "from common_n comm, g3e_ownership_optable own " +
                                      "where comm.structure_id = ? and own.g3e_sourcefno in (?,?,?) and comm.g3e_fno = own.g3e_ownerfno";

                int recordsAffected = 0;
                
                ADODB.Recordset ownerRS = m_DataContext.Execute(sqlStatement, out recordsAffected, (int)CommandTypeEnum.adCmdText, TransactionDEIS.StructureID, TransactionDEIS.FNO_TRANSFORMER_OH, 
                                                                TransactionDEIS.FNO_TRANSFORMER_UG, TransactionDEIS.FNO_VOLTAGE_REGULATOR);

                if (ownerRS.RecordCount == 1)
                {
                    ownerRS.MoveFirst();
                    string featureState = ownerRS.Fields["FEATURE_STATE_C"].Value.ToString();
                    if (TransactionDEIS.TransactionCode == "I" && (featureState == "PPR" || featureState == "ABR" || featureState == "OSR"
                        || featureState == "PPA" || featureState == "ABA" || featureState == "OSA"))
                    {
                        TransactionDEIS.TransactionStatus = TransactionDEIS.TRANS_STATUS_FAILED;
                        TransactionDEIS.TransactionMessage = string.Format("Install transaction identifies a Structure in {0} state.", featureState);
                        return false;
                    }
                    else
                    {
                        // Set Structure properties                    
                        TransactionDEIS.StructureProperties.FNO = Convert.ToInt16(ownerRS.Fields["G3E_FNO"].Value);
                        TransactionDEIS.StructureProperties.FID = Convert.ToInt32(ownerRS.Fields["G3E_FID"].Value);
                        TransactionDEIS.StructureProperties.FeatureState = ownerRS.Fields["FEATURE_STATE_C"].Value.ToString();
                        TransactionDEIS.StructureProperties.PrimaryGraphicCNO = Convert.ToInt16(ownerRS.Fields["G3E_OWNERTRIGGERINGCNO"].Value);
                        if (TransactionDEIS.FNOS_UNDERGROUND_STRUCTURES.Contains(TransactionDEIS.StructureProperties.FNO))
                        {
                            TransactionDEIS.StructureProperties.Orientation = "UG";
                        }
                        else
                        {
                            TransactionDEIS.StructureProperties.Orientation = "OH";
                        }
                    }                    
                }
                else if (ownerRS.RecordCount == 0)
                {
                    TransactionDEIS.TransactionMessage = string.Format("A structure with Structure ID {0} not found.", TransactionDEIS.StructureID);

                    if (removal)
                    {
                        TransactionDEIS.TransactionStatus = TransactionDEIS.TRANS_STATUS_WARNING;
                        return true;
                    }
                    else
                    {
                        TransactionDEIS.TransactionStatus = TransactionDEIS.TRANS_STATUS_FAILED;
                        return false;
                    }
                }
                else if(ownerRS.RecordCount == 2 && !removal)
                {
                    int installFID = 0;
                    short installFNO = 0;
                    int removedFID = 0;
                    short removedFNO = 0;
                    bool replacement = false;

                    if (!CheckIfReplacement(ownerRS, out installFNO, out installFID, out removedFNO, out removedFID, out replacement))
                    {
                        return false;
                    }

                    if (replacement)
                    {                        
                        TransactionDEIS.StructureProperties.FNO = installFNO;
                        TransactionDEIS.StructureProperties.FID = installFID;
                        ownerRS.Filter = "g3e_fid = " + installFID;
                        TransactionDEIS.StructureProperties.FeatureState = ownerRS.Fields["FEATURE_STATE_C"].Value.ToString();
                        TransactionDEIS.StructureProperties.PrimaryGraphicCNO = Convert.ToInt16(ownerRS.Fields["G3E_OWNERTRIGGERINGCNO"].Value);
                    }
                    else
                    {
                        // else log an error
                        TransactionDEIS.TransactionStatus = TransactionDEIS.TRANS_STATUS_FAILED;
                        TransactionDEIS.TransactionMessage = string.Format("Multiple structures with Structure ID {0} found.", TransactionDEIS.StructureID);
                        return false;
                    }
                }
                else if(!removal)
                {
                    TransactionDEIS.TransactionStatus = TransactionDEIS.TRANS_STATUS_FAILED;
                    TransactionDEIS.TransactionMessage = string.Format("Multiple structures with Structure ID {0} found.", TransactionDEIS.StructureID);
                    return false;
                }

                returnValue = true;
            }
            catch (Exception ex)
            {
                TransactionDEIS.TransactionStatus = TransactionDEIS.TRANS_STATUS_FAILED;
                TransactionDEIS.TransactionMessage = "Error validating Structure ID: " + ex.Message;
                returnValue = false;
            }

            return returnValue;
        }

        /// <summary>
        /// Validates the transaction codes (Phase, Type, Mount, Kind, Primary Voltage, Secondary Voltage).
        /// </summary>
        /// <returns>Boolean indicating status</returns>
        private bool ValidateCodes()
        {
            bool returnValue = false;

            try
            {
                // Validate Phase Codes
                string sqlStatement = "select phase_code, phase_value from deis_phase_code";
                int recordsAffected = 0;
                ADODB.Recordset codeRS = m_DataContext.Execute(sqlStatement, out recordsAffected, (int)CommandTypeEnum.adCmdText);

                if (TransactionDEIS.InsTrf1PhaseCode.Length > 0)
                {
                    codeRS.Filter = "phase_code = '" + TransactionDEIS.InsTrf1PhaseCode + "'";
                    if (codeRS.RecordCount > 0)
                    {
                        SetPhasingRule(TransactionDEIS.InsTrf1PhaseCode);
                    }
                    else
                    {
                        TransactionDEIS.TransactionStatus = TransactionDEIS.TRANS_STATUS_FAILED;
                        TransactionDEIS.TransactionMessage = string.Format("Phase code {0} does not exist in DEIS_PHASE_CODE table", TransactionDEIS.InsTrf1PhaseCode);
                        return false;
                    }
                }

                if (TransactionDEIS.InsTrf2PhaseCode.Length > 0)
                {
                    codeRS.Filter = "phase_code = '" + TransactionDEIS.InsTrf2PhaseCode + "'";
                    if (codeRS.RecordCount > 0)
                    {
                        SetPhasingRule(TransactionDEIS.InsTrf2PhaseCode);
                    }
                    else
                    {
                        TransactionDEIS.TransactionStatus = TransactionDEIS.TRANS_STATUS_FAILED;
                        TransactionDEIS.TransactionMessage = string.Format("Phase code {0} does not exist in DEIS_PHASE_CODE table", TransactionDEIS.InsTrf2PhaseCode);
                        return false;
                    }
                }

                if (TransactionDEIS.InsTrf3PhaseCode.Length > 0)
                {
                    codeRS.Filter = "phase_code = '" + TransactionDEIS.InsTrf3PhaseCode + "'";
                    if (codeRS.RecordCount > 0)
                    {
                        SetPhasingRule(TransactionDEIS.InsTrf3PhaseCode);
                    }
                    else
                    {
                        TransactionDEIS.TransactionStatus = TransactionDEIS.TRANS_STATUS_FAILED;
                        TransactionDEIS.TransactionMessage = string.Format("Phase code {0} does not exist in DEIS_PHASE_CODE table", TransactionDEIS.InsTrf3PhaseCode);
                        return false;
                    }
                }

                // Validate Type Codes
                sqlStatement = "select type_code, type_value from deis_type_code";
                codeRS = m_DataContext.Execute(sqlStatement, out recordsAffected, (int)CommandTypeEnum.adCmdText);

                if (TransactionDEIS.InsTrf1TypeCode.Length > 0)
                {
                    codeRS.Filter = "type_code = '" + TransactionDEIS.InsTrf1TypeCode + "'";
                    if (codeRS.RecordCount > 0)
                    {
                        TransactionDEIS.InsTrf1TypeValue = codeRS.Fields["type_value"].Value.ToString();
                    }
                    else
                    {
                        TransactionDEIS.TransactionStatus = TransactionDEIS.TRANS_STATUS_FAILED;
                        TransactionDEIS.TransactionMessage = string.Format("Type code {0} does not exist in DEIS_TYPE_CODE table", TransactionDEIS.InsTrf1TypeCode);
                        return false;
                    }
                }

                if (TransactionDEIS.InsTrf2TypeCode.Length > 0)
                {
                    codeRS.Filter = "type_code = '" + TransactionDEIS.InsTrf2TypeCode + "'";
                    if (codeRS.RecordCount > 0)
                    {
                        TransactionDEIS.InsTrf2TypeValue = codeRS.Fields["type_value"].Value.ToString();
                    }
                    else
                    {
                        TransactionDEIS.TransactionStatus = TransactionDEIS.TRANS_STATUS_FAILED;
                        TransactionDEIS.TransactionMessage = string.Format("Type code {0} does not exist in DEIS_TYPE_CODE table", TransactionDEIS.InsTrf2TypeCode);
                        return false;
                    }
                }

                if (TransactionDEIS.InsTrf3TypeCode.Length > 0)
                {
                    codeRS.Filter = "type_code = '" + TransactionDEIS.InsTrf3TypeCode + "'";
                    if (codeRS.RecordCount > 0)
                    {
                        TransactionDEIS.InsTrf3TypeValue = codeRS.Fields["type_value"].Value.ToString();
                    }
                    else
                    {
                        TransactionDEIS.TransactionStatus = TransactionDEIS.TRANS_STATUS_FAILED;
                        TransactionDEIS.TransactionMessage = string.Format("Type code {0} does not exist in DEIS_TYPE_CODE table", TransactionDEIS.InsTrf3TypeCode);
                        return false;
                    }
                }

                // Validate Mount Codes
                sqlStatement = "select mount_code, mount_value, mount_orientation from deis_mount_code";
                codeRS = m_DataContext.Execute(sqlStatement, out recordsAffected, (int)CommandTypeEnum.adCmdText);

                if (TransactionDEIS.InsTrf1MountCode.Length > 0)
                {
                    codeRS.Filter = "mount_code = '" + TransactionDEIS.InsTrf1MountCode + "'";
                    if (codeRS.RecordCount > 0)
                    {
                        TransactionDEIS.InsTrf1MountValue = codeRS.Fields["mount_value"].Value.ToString();
                        TransactionDEIS.InsTrf1MountOrientation = codeRS.Fields["mount_orientation"].Value.ToString();
                    }
                    else
                    {
                        TransactionDEIS.TransactionStatus = TransactionDEIS.TRANS_STATUS_FAILED;
                        TransactionDEIS.TransactionMessage = string.Format("Mount code {0} does not exist in DEIS_MOUNT_CODE table", TransactionDEIS.InsTrf1MountCode);
                        return false;
                    }
                }

                if (TransactionDEIS.InsTrf2MountCode.Length > 0)
                {
                    codeRS.Filter = "mount_code = '" + TransactionDEIS.InsTrf2MountCode + "'";
                    if (codeRS.RecordCount > 0)
                    {
                        TransactionDEIS.InsTrf2MountValue = codeRS.Fields["mount_value"].Value.ToString();
                        TransactionDEIS.InsTrf2MountOrientation = codeRS.Fields["mount_orientation"].Value.ToString();
                    }
                    else
                    {
                        TransactionDEIS.TransactionStatus = TransactionDEIS.TRANS_STATUS_FAILED;
                        TransactionDEIS.TransactionMessage = string.Format("Mount code {0} does not exist in DEIS_MOUNT_CODE table", TransactionDEIS.InsTrf2MountCode);
                        return false;
                    }
                }

                if (TransactionDEIS.InsTrf3MountCode.Length > 0)
                {
                    codeRS.Filter = "mount_code = '" + TransactionDEIS.InsTrf3MountCode + "'";
                    if (codeRS.RecordCount > 0)
                    {
                        TransactionDEIS.InsTrf3MountValue = codeRS.Fields["mount_value"].Value.ToString();
                        TransactionDEIS.InsTrf3MountOrientation = codeRS.Fields["mount_orientation"].Value.ToString();
                    }
                    else
                    {
                        TransactionDEIS.TransactionStatus = TransactionDEIS.TRANS_STATUS_FAILED;
                        TransactionDEIS.TransactionMessage = string.Format("Mount code {0} does not exist in DEIS_MOUNT_CODE table", TransactionDEIS.InsTrf3MountCode);
                        return false;
                    }
                }

                // Validate Kind Codes
                sqlStatement = "select kind_code, kind_value from deis_kind_code";
                codeRS = m_DataContext.Execute(sqlStatement, out recordsAffected, (int)CommandTypeEnum.adCmdText);

                if (TransactionDEIS.InsTrf1KindCode.Length > 0)
                {
                    codeRS.Filter = "kind_code = '" + TransactionDEIS.InsTrf1KindCode + "'";
                    if (codeRS.RecordCount > 0)
                    {
                        TransactionDEIS.InsTrf1KindValue = codeRS.Fields["kind_value"].Value.ToString();
                    }
                    else
                    {
                        TransactionDEIS.TransactionStatus = TransactionDEIS.TRANS_STATUS_FAILED;
                        TransactionDEIS.TransactionMessage = string.Format("Kind code {0} does not exist in DEIS_KIND_CODE table", TransactionDEIS.InsTrf1KindCode);
                        return false;
                    }
                }

                if (TransactionDEIS.InsTrf2KindCode.Length > 0)
                {
                    codeRS.Filter = "kind_code = '" + TransactionDEIS.InsTrf2KindCode + "'";
                    if (codeRS.RecordCount > 0)
                    {
                        TransactionDEIS.InsTrf2KindValue = codeRS.Fields["kind_value"].Value.ToString();
                    }
                    else
                    {
                        TransactionDEIS.TransactionStatus = TransactionDEIS.TRANS_STATUS_FAILED;
                        TransactionDEIS.TransactionMessage = string.Format("Kind code {0} does not exist in DEIS_KIND_CODE table", TransactionDEIS.InsTrf2KindCode);
                        return false;
                    }
                }

                if (TransactionDEIS.InsTrf3KindCode.Length > 0)
                {
                    codeRS.Filter = "kind_code = '" + TransactionDEIS.InsTrf3KindCode + "'";
                    if (codeRS.RecordCount > 0)
                    {
                        TransactionDEIS.InsTrf3KindValue = codeRS.Fields["kind_value"].Value.ToString();
                    }
                    else
                    {
                        TransactionDEIS.TransactionStatus = TransactionDEIS.TRANS_STATUS_FAILED;
                        TransactionDEIS.TransactionMessage = string.Format("Kind code {0} does not exist in DEIS_KIND_CODE table", TransactionDEIS.InsTrf3KindCode);
                        return false;
                    }
                }

                // Validate Voltage Codes
                sqlStatement = "select voltage_code, voltage_value from deis_voltage_code";
                codeRS = m_DataContext.Execute(sqlStatement, out recordsAffected, (int)CommandTypeEnum.adCmdText);

                //      Validate Primary Voltage Codes
                if (TransactionDEIS.InsTrf1PriVoltCode.Length > 0)
                {
                    codeRS.Filter = "voltage_code = '" + TransactionDEIS.InsTrf1PriVoltCode + "'";
                    if (codeRS.RecordCount > 0)
                    {
                        TransactionDEIS.InsTrf1PriVoltValue = codeRS.Fields["voltage_value"].Value.ToString();
                    }
                    else
                    {
                        TransactionDEIS.TransactionStatus = TransactionDEIS.TRANS_STATUS_FAILED;
                        TransactionDEIS.TransactionMessage = string.Format("Primary voltage code {0} does not exist in DEIS_VOLTAGE_CODE", TransactionDEIS.InsTrf1PriVoltCode);
                        return false;
                    }
                }

                if (TransactionDEIS.InsTrf2PriVoltCode.Length > 0)
                {
                    codeRS.Filter = "voltage_code = '" + TransactionDEIS.InsTrf2PriVoltCode + "'";
                    if (codeRS.RecordCount > 0)
                    {
                        TransactionDEIS.InsTrf2PriVoltValue = codeRS.Fields["voltage_value"].Value.ToString();
                    }
                    else
                    {
                        TransactionDEIS.TransactionStatus = TransactionDEIS.TRANS_STATUS_FAILED;
                        TransactionDEIS.TransactionMessage = string.Format("Primary voltage code {0} does not exist in DEIS_VOLTAGE_CODE", TransactionDEIS.InsTrf2PriVoltCode);
                        return false;
                    }
                }

                if (TransactionDEIS.InsTrf3PriVoltCode.Length > 0)
                {
                    codeRS.Filter = "voltage_code = '" + TransactionDEIS.InsTrf3PriVoltCode + "'";
                    if (codeRS.RecordCount > 0)
                    {
                        TransactionDEIS.InsTrf3PriVoltValue = codeRS.Fields["voltage_value"].Value.ToString();
                    }
                    else
                    {
                        TransactionDEIS.TransactionStatus = TransactionDEIS.TRANS_STATUS_FAILED;
                        TransactionDEIS.TransactionMessage = string.Format("Primary voltage code {0} does not exist in DEIS_VOLTAGE_CODE", TransactionDEIS.InsTrf3PriVoltCode);
                        return false;
                    }
                }

                //      Validate Secondary Voltage Codes
                if (TransactionDEIS.InsTrf1SecVoltCode.Length > 0)
                {
                    codeRS.Filter = "voltage_code = '" + TransactionDEIS.InsTrf1SecVoltCode + "'";
                    if (codeRS.RecordCount > 0)
                    {
                        TransactionDEIS.InsTrf1SecVoltValue = codeRS.Fields["voltage_value"].Value.ToString();
                    }
                    else
                    {
                        TransactionDEIS.TransactionStatus = TransactionDEIS.TRANS_STATUS_FAILED;
                        TransactionDEIS.TransactionMessage = string.Format("Secondary voltage code {0} does not exist in DEIS_VOLTAGE_CODE", TransactionDEIS.InsTrf1SecVoltCode);
                        return false;
                    }
                }

                if (TransactionDEIS.InsTrf2SecVoltCode.Length > 0)
                {
                    codeRS.Filter = "voltage_code = '" + TransactionDEIS.InsTrf2SecVoltCode + "'";
                    if (codeRS.RecordCount > 0)
                    {
                        TransactionDEIS.InsTrf2SecVoltValue = codeRS.Fields["voltage_value"].Value.ToString();
                    }
                    else
                    {
                        TransactionDEIS.TransactionStatus = TransactionDEIS.TRANS_STATUS_FAILED;
                        TransactionDEIS.TransactionMessage = string.Format("Secondary voltage code {0} does not exist in DEIS_VOLTAGE_CODE", TransactionDEIS.InsTrf2SecVoltCode);
                        return false;
                    }
                }

                if (TransactionDEIS.InsTrf3SecVoltCode.Length > 0)
                {
                    codeRS.Filter = "voltage_code = '" + TransactionDEIS.InsTrf3SecVoltCode + "'";
                    if (codeRS.RecordCount > 0)
                    {
                        TransactionDEIS.InsTrf3SecVoltValue = codeRS.Fields["voltage_value"].Value.ToString();
                    }
                    else
                    {
                        TransactionDEIS.TransactionStatus = TransactionDEIS.TRANS_STATUS_FAILED;
                        TransactionDEIS.TransactionMessage = string.Format("Secondary voltage code {0} does not exist in DEIS_VOLTAGE_CODE", TransactionDEIS.InsTrf3SecVoltCode);
                        return false;
                    }
                }

                returnValue = true;
            }
            catch (Exception ex)
            {
                TransactionDEIS.TransactionStatus = TransactionDEIS.TRANS_STATUS_FAILED;
                TransactionDEIS.TransactionMessage = "Error validating transaction codes: " + ex.Message;
                returnValue = false;
            }

            return returnValue;
        }

        /// <summary>
        /// Validate Transformers if exist at structure for installation.
        /// </summary>
        /// <returns>Boolean indicating status</returns>
        private bool ValidateExistingTransformerInstall()
        {
            bool returnValue = false;

            try
            {
                // Query for Transformers at Structure ID
                string sqlStatement = "select comm2.g3e_fid, comm2.g3e_fno, comm2.feature_state_c, conn.phase_alpha " +
                                      "from common_n comm1, common_n comm2, connectivity_n conn, auto_xfmr_unit_n unit " +
                                      "where comm1.g3e_fid = ? and comm2.g3e_fid = conn.g3e_fid and comm2.owner1_id = comm1.g3e_id and comm2.g3e_fno in (?) and comm2.feature_state_c in ('PPI','ABI','INI') " +
                                      "and comm2.g3e_fid = unit.g3e_fid and unit.company_id is null " + 
                                      "union " +
                                      "select comm2.g3e_fid, comm2.g3e_fno, comm2.feature_state_c, conn.phase_alpha " +
                                      "from common_n comm1, common_n comm2, connectivity_n conn, xfmr_oh_unit_n unit " +
                                      "where comm1.g3e_fid = ? and comm2.g3e_fid = conn.g3e_fid and comm2.owner1_id = comm1.g3e_id and comm2.g3e_fno in (?) and comm2.feature_state_c in ('PPI','ABI','INI') " +
                                      "and comm2.g3e_fid = unit.g3e_fid and unit.company_id is null " +
                                      "union " +
                                      "select comm2.g3e_fid, comm2.g3e_fno, comm2.feature_state_c, conn.phase_alpha " +
                                      "from common_n comm1, common_n comm2, connectivity_n conn, xfmr_ug_unit_n unit " +
                                      "where comm1.g3e_fid = ? and comm2.g3e_fid = conn.g3e_fid and comm2.owner1_id = comm1.g3e_id and comm2.g3e_fno in (?) and comm2.feature_state_c in ('PPI','ABI','INI') " +
                                      "and comm2.g3e_fid = unit.g3e_fid and unit.company_id is null";

                int recordsAffected = 0;
                ADODB.Recordset transformerRS = m_DataContext.Execute(sqlStatement, out recordsAffected, (int)CommandTypeEnum.adCmdText, TransactionDEIS.StructureProperties.FID, TransactionDEIS.TransformerProperties.FNO, 
                                                TransactionDEIS.StructureProperties.FID, TransactionDEIS.TransformerProperties.FNO, TransactionDEIS.StructureProperties.FID, TransactionDEIS.TransformerProperties.FNO);
                                
                if (TransactionDEIS.InsTrf1TypeCode == "07" || TransactionDEIS.InsTrf2TypeCode == "07" || TransactionDEIS.InsTrf3TypeCode == "07")
                {
                    // Only one Autotransformer must exist in the PPI state.
                    transformerRS.Filter = "g3e_fno = " + TransactionDEIS.FNO_AUTOTRANSFORMER;
                    if (transformerRS.RecordCount != 1)
                    {
                        TransactionDEIS.TransactionStatus = TransactionDEIS.TRANS_STATUS_FAILED;
                        TransactionDEIS.TransactionMessage = "Install transaction identifies an Autotransformer and an Autotransformer in PPI, ABI, or INI state was not found.";
                        returnValue = false;
                    }
                    else
                    {
                        // Found single Proposed Install Autotransformer. Use this Transformer for installation.
                        TransactionDEIS.TransformerProperties.FID = Convert.ToInt32(transformerRS.Fields["G3E_FID"].Value);
                        TransactionDEIS.TransformerProperties.FeatureState = transformerRS.Fields["FEATURE_STATE_C"].Value.ToString();
                        TransactionDEIS.TransformerProperties.ExistingPhases = transformerRS.Fields["PHASE_ALPHA"].Value.ToString();
                        TransactionDEIS.TransformerProperties.PhaseCount = TransactionDEIS.TransformerProperties.ExistingPhases.Length;
                        returnValue = true;
                    }
                }                
                else if (TransactionDEIS.InsTrf1MountOrientation == "UG")
                {
                    // UG Transformer must exist in the PPI, ABI, or INI state at the identified Structure.
                    transformerRS.Filter = "g3e_fno = " + TransactionDEIS.FNO_TRANSFORMER_UG + " or g3e_fno = " + TransactionDEIS.FNO_TRANSFORMER_UG_NETWORK;
                    if (transformerRS.RecordCount == 0)
                    {
                        TransactionDEIS.TransactionStatus = TransactionDEIS.TRANS_STATUS_FAILED;
                        TransactionDEIS.TransactionMessage = "Install transaction identifies an UG Transformer and an UG Transformer in PPI, ABI, or INI state was not found.";
                        returnValue = false;
                    }
                    else
                    {
                        // Could be multiple so pick the first one. Can't match on phase like OH Transformer to determine the correct one.
                        transformerRS.MoveFirst();
                        TransactionDEIS.TransformerProperties.FID = Convert.ToInt32(transformerRS.Fields["G3E_FID"].Value);
                        TransactionDEIS.TransformerProperties.FeatureState = transformerRS.Fields["FEATURE_STATE_C"].Value.ToString();
                        TransactionDEIS.TransformerProperties.ExistingPhases = transformerRS.Fields["PHASE_ALPHA"].Value.ToString();
                        TransactionDEIS.TransformerProperties.PhaseCount = TransactionDEIS.TransformerProperties.ExistingPhases.Length;
                        returnValue = true;
                    }
                }
                else
                {
                    // OH Transformer
                    if (transformerRS.RecordCount > 0)
                    {                        
                        if (transformerRS.RecordCount == 1)
                        {
                            // Found single Proposed Install Transformer. Use this Transformer for installation.
                            TransactionDEIS.TransformerProperties.FID = Convert.ToInt32(transformerRS.Fields["G3E_FID"].Value);
                            TransactionDEIS.TransformerProperties.FeatureState = transformerRS.Fields["FEATURE_STATE_C"].Value.ToString();
                            TransactionDEIS.TransformerProperties.ExistingPhases = transformerRS.Fields["PHASE_ALPHA"].Value.ToString();
                            TransactionDEIS.TransformerProperties.PhaseCount = TransactionDEIS.TransformerProperties.ExistingPhases.Length;
                            returnValue = true;
                        }
                        else if (transformerRS.RecordCount > 1)
                        {
                            // Determine Transformer to use by matching phases
                            if (!DetermineTransformerInstallByPhase(transformerRS))
                            {
                                return false;
                            }

                            returnValue = true;
                        }
                    }
                    else
                    {
                        // Check for CLS OH Transformer
                        sqlStatement = "select comm2.g3e_fid, comm2.g3e_fno, comm2.feature_state_c, conn.phase_alpha, unit.company_id " +
                                       "from common_n comm1, common_n comm2, connectivity_n conn, xfmr_oh_unit_n unit " +
                                       "where comm1.g3e_fid = ? and comm2.g3e_fid = conn.g3e_fid and comm2.owner1_id = comm1.g3e_id and comm2.g3e_fno in (?) and comm2.feature_state_c = 'CLS' " +
                                       "and comm2.g3e_fid = unit.g3e_fid";

                        transformerRS = m_DataContext.Execute(sqlStatement, out recordsAffected, (int)CommandTypeEnum.adCmdText, TransactionDEIS.StructureProperties.FID, TransactionDEIS.TransformerProperties.FNO);

                        if (transformerRS.RecordCount > 0)
                        {
                            transformerRS.MoveFirst();
                            // If Company ID matches a record then use that FID for update
                            transformerRS.Filter = "company_id = '" + TransactionDEIS.InsTrf1CompanyNumber + "'";
                            if (transformerRS.RecordCount == 1)
                            {
                                // Found single CLS Transformer. Use this Transformer for installation.
                                TransactionDEIS.TransformerProperties.FID = Convert.ToInt32(transformerRS.Fields["G3E_FID"].Value);
                                TransactionDEIS.TransformerProperties.FeatureState = transformerRS.Fields["FEATURE_STATE_C"].Value.ToString();
                                TransactionDEIS.TransformerProperties.ExistingPhases = transformerRS.Fields["PHASE_ALPHA"].Value.ToString();
                                TransactionDEIS.TransformerProperties.PhaseCount = TransactionDEIS.TransformerProperties.ExistingPhases.Length;
                                returnValue = true;
                            }
                            else
                            {
                                transformerRS.Filter = "";
                                transformerRS.MoveFirst();
                                int recordCount = transformerRS.RecordCount;
                                int fid = Convert.ToInt32(transformerRS.Fields["G3E_FID"].Value);
                                transformerRS.Filter = "g3e_fid = " + fid;
                                if (transformerRS.RecordCount == recordCount)
                                {
                                    // If only one FID then use that FID to add unit record
                                    TransactionDEIS.TransformerProperties.FID = Convert.ToInt32(transformerRS.Fields["G3E_FID"].Value);
                                    TransactionDEIS.TransformerProperties.FeatureState = transformerRS.Fields["FEATURE_STATE_C"].Value.ToString();
                                    TransactionDEIS.TransformerProperties.ExistingPhases = transformerRS.Fields["PHASE_ALPHA"].Value.ToString();
                                    TransactionDEIS.TransformerProperties.PhaseCount = TransactionDEIS.TransformerProperties.ExistingPhases.Length;
                                    returnValue = true;
                                }
                                else
                                {
                                    // Only one CLS transformer may exist on the identified Structure
                                    TransactionDEIS.TransactionStatus = TransactionDEIS.TRANS_STATUS_FAILED;
                                    TransactionDEIS.TransactionMessage = "Install identifies an existing bank and the identified structure has more than one CLS transformer bank.";
                                    returnValue = false;
                                }
                            }

                            transformerRS.Filter = "";
                        }
                        else
                        {
                            // New install
                            returnValue = true;
                        }
                    }

                    if (TransactionDEIS.TransformerProperties.FID != 0 && TransactionDEIS.TransactionCode != "C" && TransactionDEIS.ExistingBank != "Y")
                    {
                        // A transformer exists at the indicated structure and EXISTING_BANK is not Y                            
                        TransactionDEIS.TransactionStatus = TransactionDEIS.TRANS_STATUS_WARNING;
                        TransactionDEIS.TransactionMessage = "An existing transformer was found at the structure but the EXISTING_BANK flag was not set to Y.  Updated transformer bank";
                        returnValue = true;
                    }
                    else if (TransactionDEIS.TransformerProperties.FID == 0 && TransactionDEIS.ExistingBank == "Y")
                    {
                        // No transformer exists on the identified Structure
                        TransactionDEIS.TransactionStatus = TransactionDEIS.TRANS_STATUS_FAILED;
                        TransactionDEIS.TransactionMessage = "No transformer exists on the identified Structure.";
                        returnValue = false;
                    }
                }
            }
            catch (Exception ex)
            {
                TransactionDEIS.TransactionStatus = TransactionDEIS.TRANS_STATUS_FAILED;
                TransactionDEIS.TransactionMessage = "Error validating existing Transformer for installation: " + ex.Message;
                returnValue = false;
            }

            return returnValue;
        }

        /// <summary>
        /// Determine Transformer to use for installation when there are multiple. Match transaction phase with Transformer phase.
        /// </summary>
        /// <param name="transformerRS">Recordset containing multiple Transformer canidates</param>
        /// <returns>Boolean indicating status</returns>
        private bool DetermineTransformerInstallByPhase(ADODB.Recordset transformerRS)
        {
            bool returnValue = false;

            try
            {
                IGTKeyObject transformerKO = null;
                ADODB.Recordset compRS = null;

                transformerRS.MoveFirst();
                int fid = 0;
                short fno = 0;
                bool foundTransformer = false;

                while (!transformerRS.EOF)
                {
                    foundTransformer = false;

                    fid = Convert.ToInt32(transformerRS.Fields["G3E_FID"].Value);
                    fno = Convert.ToInt16(transformerRS.Fields["G3E_FNO"].Value);

                    transformerKO = m_DataContext.OpenFeature(fno, fid);
                    compRS = transformerKO.Components.GetComponent(TransactionDEIS.CNO_TRANSFORMER_OH_UNIT).Recordset;
                    if (compRS.RecordCount > 0)
                    {
                        // If each transaction phase matches a transformer unit record then use the transformer
                        if (TransactionDEIS.InsTrf1PhaseCode.Length > 0)
                        {
                            compRS.Filter = "phase_c = '" + TransactionDEIS.InsTrf1PhaseCode + "'";
                            if (compRS.RecordCount == 1)
                            {
                                foundTransformer = true;
                            }
                            else
                            {
                                foundTransformer = false;
                            }
                            compRS.Filter = "";
                        }

                        if (TransactionDEIS.InsTrf2PhaseCode.Length > 0 && foundTransformer)
                        {
                            compRS.Filter = "phase_c = '" + TransactionDEIS.InsTrf2PhaseCode + "'";
                            if (compRS.RecordCount == 1)
                            {
                                foundTransformer = true;
                            }
                            else
                            {
                                foundTransformer = false;
                            }
                            compRS.Filter = "";
                        }

                        if (TransactionDEIS.InsTrf3PhaseCode.Length > 0 && foundTransformer)
                        {
                            compRS.Filter = "phase_c = '" + TransactionDEIS.InsTrf3PhaseCode + "'";
                            if (compRS.RecordCount == 1)
                            {
                                foundTransformer = true;
                            }
                            else
                            {
                                foundTransformer = false;
                            }
                            compRS.Filter = "";
                        }

                        if (foundTransformer)
                        {
                            TransactionDEIS.TransformerProperties.FID = fid;
                            TransactionDEIS.TransformerProperties.FeatureState = transformerRS.Fields["FEATURE_STATE_C"].Value.ToString();
                            TransactionDEIS.TransformerProperties.ExistingPhases = transformerRS.Fields["PHASE_ALPHA"].Value.ToString();
                            TransactionDEIS.TransformerProperties.PhaseCount = TransactionDEIS.TransformerProperties.ExistingPhases.Length;
                            returnValue = true;
                            break;
                        }
                    }

                    transformerRS.MoveNext();
                }

                returnValue = true;
            }
            catch (Exception ex)
            {
                TransactionDEIS.TransactionStatus = TransactionDEIS.TRANS_STATUS_FAILED;
                TransactionDEIS.TransactionMessage = "Error validating existing Transformer for installation: " + ex.Message;
                returnValue = false;
            }

            return returnValue;
        }

        /// <summary>
        /// Validate existing Transformer.
        /// </summary>
        /// <param name="xfmr1CompanyNumber">The Company Number for the first Transformer</param>
        /// <param name="xfmr2CompanyNumber">The Company Number for the second Transformer</param>
        /// <param name="xfmr3CompanyNumber">The Company Number for the third Transformer</param>
        /// <returns>Boolean indicating status</returns>
        private bool ValidateExistingTransformer(string xfmr1CompanyNumber, string xfmr2CompanyNumber, string xfmr3CompanyNumber)
        {
            bool returnValue = false;

            try
            {                
                string companyCondition = string.Empty;
                List<object> queryParams1 = new List<object>();

                // Build Company ID query condition and parameters
                if (xfmr1CompanyNumber.Length > 0)
                {
                    queryParams1.Add(xfmr1CompanyNumber);
                    companyCondition = "(?";
                }
                if (xfmr2CompanyNumber.Length > 0)
                {
                    queryParams1.Add(xfmr2CompanyNumber);
                    companyCondition += ",?";
                }
                if (xfmr3CompanyNumber.Length > 0)
                {
                    queryParams1.Add(xfmr3CompanyNumber);
                    companyCondition += ",?";
                }

                // Make a copy of the parameters for each sql select statement
                object[] queryParams2 = new object[queryParams1.Count * 3];
                Array.Copy(queryParams1.ToArray(), queryParams2, queryParams1.Count);
                Array.Copy(queryParams1.ToArray(), 0, queryParams2, queryParams1.Count, queryParams1.Count);
                Array.Copy(queryParams1.ToArray(), 0, queryParams2, queryParams1.Count * 2, queryParams1.Count);

                companyCondition += ")";

                // Query for Transformers and Structure ID
                string sqlStatement = string.Format("select comm2.g3e_fid, comm2.g3e_fno, comm2.feature_state_c, comm1.structure_id, comm2.replaced_fid, unit.company_id, unit.g3e_cid, conn.phase_alpha, conn.node_1_id, conn.node_2_id " +
                                      "from common_n comm1, common_n comm2, auto_xfmr_unit_n unit, connectivity_n conn " +
                                      "where unit.company_id in {0} and comm2.g3e_fid = unit.g3e_fid and comm2.g3e_fid = conn.g3e_fid and comm2.owner1_id = comm1.g3e_id " +
                                      "union " +
                                      "select comm2.g3e_fid, comm2.g3e_fno, comm2.feature_state_c, comm1.structure_id, comm2.replaced_fid, unit.company_id, unit.g3e_cid, conn.phase_alpha, conn.node_1_id, conn.node_2_id  " +
                                      "from common_n comm1, common_n comm2, xfmr_oh_unit_n unit, connectivity_n conn " +
                                      "where unit.company_id in {1} and comm2.g3e_fid = unit.g3e_fid and comm2.g3e_fid = conn.g3e_fid and comm2.owner1_id = comm1.g3e_id " +
                                      "union " +
                                      "select comm2.g3e_fid, comm2.g3e_fno, comm2.feature_state_c, comm1.structure_id, comm2.replaced_fid, unit.company_id, unit.g3e_cid, conn.phase_alpha, conn.node_1_id, conn.node_2_id " +
                                      "from common_n comm1, common_n comm2, xfmr_ug_unit_n unit, connectivity_n conn " +
                                      "where unit.company_id in {2} and comm2.g3e_fid = unit.g3e_fid and comm2.g3e_fid = conn.g3e_fid and comm2.owner1_id = comm1.g3e_id", companyCondition, companyCondition, companyCondition);

                int recordsAffected = 0;
                ADODB.Recordset transformerRS = m_DataContext.Execute(sqlStatement, out recordsAffected, (int)CommandTypeEnum.adCmdText, queryParams2);

                // All transformers must exist on the Structure identified in the transaction
                // Filter where records not equal to input Structure ID. If recordcount > 0 then error.
                transformerRS.Filter = "structure_id <> '" + TransactionDEIS.StructureID + "'";
                if (transformerRS.RecordCount > 0)
                {
                    transformerRS.MoveFirst();
                    while (!transformerRS.EOF)
                    {
                        TransactionDEIS.TransactionMessage += " Company Number " + transformerRS.Fields["COMPANY_ID"].Value.ToString() + " on Structure " + transformerRS.Fields["STRUCTURE_ID"].Value.ToString();
                        transformerRS.MoveNext();
                    }
                    TransactionDEIS.TransactionMessage = "The following company numbers were found at their respective structure IDs which are not the structure ID given in the transaction: " + TransactionDEIS.TransactionMessage;

                    if (TransactionDEIS.TransactionCode == "R")
                    {
                        TransactionDEIS.TransactionStatus = TransactionDEIS.TRANS_STATUS_WARNING;
                    }   
                    else
                    {
                        TransactionDEIS.TransactionStatus = TransactionDEIS.TRANS_STATUS_FAILED;
                        return false;                        
                    }
                }

                // A company number must identify only one transformer
                // Filter by each input Company Number. If recordcount > 0 then error.
                // Transformer with indicated company number must exist. If recordcount = 0 then error.
                string duplicateCompanyNumbers = string.Empty;
                string missingCompanyNumbers = string.Empty;
                bool duplicates = false;
                bool missing = false;

                if (xfmr1CompanyNumber.Length > 0)
                {
                    transformerRS.Filter = "company_id = '" + xfmr1CompanyNumber + "'";
                    if (transformerRS.RecordCount > 1)
                    {
                        transformerRS.MoveFirst();
                        duplicateCompanyNumbers = xfmr1CompanyNumber + ",";
                        duplicates = true;
                    }
                    else if (transformerRS.RecordCount == 0)
                    {
                        missingCompanyNumbers = xfmr1CompanyNumber + ",";
                        missing = true;
                    }
                }

                if (xfmr2CompanyNumber.Length > 0)
                {
                    transformerRS.Filter = "company_id = '" + xfmr2CompanyNumber + "'";
                    if (transformerRS.RecordCount > 1)
                    {
                        transformerRS.MoveFirst();
                        duplicateCompanyNumbers += xfmr2CompanyNumber + ",";
                        duplicates = true;
                    }
                    else if (transformerRS.RecordCount == 0)
                    {
                        missingCompanyNumbers += xfmr2CompanyNumber + ",";
                        missing = true;
                    }
                }

                if (xfmr3CompanyNumber.Length > 0)
                {
                    transformerRS.Filter = "company_id = '" + xfmr3CompanyNumber + "'";
                    if (transformerRS.RecordCount > 1)
                    {
                        transformerRS.MoveFirst();
                        duplicateCompanyNumbers += xfmr3CompanyNumber + ",";
                        duplicates = true;
                    }
                    else if (transformerRS.RecordCount == 0)
                    {
                        missingCompanyNumbers += xfmr3CompanyNumber + ",";
                        missing = true;
                    }
                }

                if (duplicates)
                {
                    TransactionDEIS.TransactionStatus = TransactionDEIS.TRANS_STATUS_FAILED;
                    TransactionDEIS.TransactionMessage = "More than one transformer was located for the following company number(s): " + duplicateCompanyNumbers.Substring(0, duplicateCompanyNumbers.Length - 1);
                    return false;
                }
                else if (missing)
                {
                    TransactionDEIS.TransactionStatus = TransactionDEIS.TRANS_STATUS_FAILED;
                    TransactionDEIS.TransactionMessage = "The following company number(s) was/were not found in the system: " + missingCompanyNumbers.Substring(0, missingCompanyNumbers.Length - 1);
                    return false;
                }

                // Validate that the company numbers are on the same transformer
                transformerRS.Filter = "";
                transformerRS.MoveFirst();
                int recordCount = transformerRS.RecordCount;
                Int32 transformerFID = Convert.ToInt32(transformerRS.Fields["G3E_FID"].Value);

                transformerRS.Filter = "g3e_fid = " + transformerFID;
                if (transformerRS.RecordCount != recordCount)
                {
                    TransactionDEIS.TransactionStatus = TransactionDEIS.TRANS_STATUS_FAILED;
                    TransactionDEIS.TransactionMessage = "The company numbers identify more than one transformer.";
                    return false;
                }

                transformerRS.Filter = "";
                transformerRS.MoveFirst();

                // If a change-out
                if (TransactionDEIS.TransactionCode == "C")
                {
                    // Capture the Node1 and Node2 values to be used when creating a new Transformer for the change-out
                    if (!Convert.IsDBNull(transformerRS.Fields["NODE_1_ID"].Value))
                    {
                        TransactionDEIS.TransformerProperties.Node1 = Convert.ToInt32(transformerRS.Fields["NODE_1_ID"].Value);
                    }
                    if (!Convert.IsDBNull(transformerRS.Fields["NODE_2_ID"].Value))
                    {
                        TransactionDEIS.TransformerProperties.Node2 = Convert.ToInt32(transformerRS.Fields["NODE_2_ID"].Value);
                    }

                    // If PPR, ABR, or OSR transformer then log a warning if PPI, ABI, or INI transformer doesn't exist.
                    transformerRS.MoveFirst();
                    string featureState = transformerRS.Fields["FEATURE_STATE_C"].Value.ToString();
                    if (featureState == "PPR" || featureState == "ABR" || featureState == "OSR")
                    {
                        // Query for associated 'PPI', 'ABI', or 'INI' transformer
                        sqlStatement = "select g3e_fid from common_n where replaced_fid = ? and feature_state_c in ('PPI', 'ABI', 'INI')";
                        ADODB.Recordset replacedRS = m_DataContext.Execute(sqlStatement, out recordsAffected, (int)CommandTypeEnum.adCmdText, transformerFID);

                        if (replacedRS.RecordCount != 1)
                        {
                            TransactionDEIS.TransactionStatus = TransactionDEIS.TRANS_STATUS_WARNING;
                            TransactionDEIS.TransactionMessage = string.Format("A {0} transformer existed at the indicated structure but no {1} transformer was found at that structure.  {2} transformer was removed and new transformer was created",
                                featureState, featureState == "PPR" ? "PPI" : featureState == "ABR" ? "ABI" : "INI", featureState);
                        }                        
                    }
                }

                TransactionDEIS.TransformerProperties.ExistingPhases = transformerRS.Fields["phase_alpha"].Value.ToString();
                TransactionDEIS.TransformerProperties.PhaseCount = TransactionDEIS.TransformerProperties.ExistingPhases.Length;
                if (TransactionDEIS.TransformerProperties.FID == 0)
                {
                    transformerRS.MoveFirst();
                    string featureState = transformerRS.Fields["FEATURE_STATE_C"].Value.ToString();
                    TransactionDEIS.TransformerProperties.FNO = Convert.ToInt16(transformerRS.Fields["G3E_FNO"].Value);
                    TransactionDEIS.TransformerProperties.FID = Convert.ToInt32(transformerRS.Fields["G3E_FID"].Value);
                    TransactionDEIS.TransformerProperties.FeatureState = featureState;
                }

                // Set the transformer feature properties (G3E_CNO, bank/unit tables, ...) based on the TransactionDEIS.TransformerProperties.FNO
                if (!SetTransformerFeatureProperties())
                {
                    return false;
                }

                if (TransactionDEIS.TransactionCode == "R")
                {
                    // Check if all units are being removed.
                    transformerRS.MoveFirst();

                    object[] queryParams3 = new object[queryParams1.Count + 1];
                    queryParams3[0] = TransactionDEIS.TransformerProperties.FID;
                    Array.Copy(queryParams1.ToArray(), 0, queryParams3, 1, queryParams1.Count);

                    sqlStatement = string.Format("select company_id from {0} where g3e_fid = ? and company_id not in {1}", TransactionDEIS.TransformerProperties.UnitTable, companyCondition);
                    ADODB.Recordset unitRS = m_DataContext.Execute(sqlStatement, out recordsAffected, (int)CommandTypeEnum.adCmdText, queryParams3);

                    if (unitRS.RecordCount > 0)
                    {
                        TransactionDEIS.TransformerProperties.RemoveBank = false;
                    }
                    else
                    {
                        TransactionDEIS.TransformerProperties.RemoveBank = true;
                    }

                    // If a PPR, ABR, or OSR transformer bank is located, then all transformers in it must be removed with the transaction.
                    string featureState = transformerRS.Fields["FEATURE_STATE_C"].Value.ToString();
                    if (featureState == "PPR" || featureState == "ABR" || featureState == "OSR")
                    {
                        if (!TransactionDEIS.TransformerProperties.RemoveBank)
                        {
                            TransactionDEIS.TransactionStatus = TransactionDEIS.TRANS_STATUS_FAILED;
                            TransactionDEIS.TransactionMessage = string.Format("A {0} transformer bank was located but not all transformers in it are identified as removals in this transaction", featureState);
                            return false;
                        }
                    }

                    // If Transformer feature is being removed then query to see if any Service Points with Premises are assigned to the Transformer.
                    if (TransactionDEIS.TransformerProperties.RemoveBank)
                    {
                        sqlStatement = "select premise.g3e_fid from premise_n premise, connectivity_n conn where conn.protective_device_fid = ? and conn.g3e_fid = premise.g3e_fid";
                        ADODB.Recordset premiseRS = m_DataContext.Execute(sqlStatement, out recordsAffected, (int)CommandTypeEnum.adCmdText, TransactionDEIS.TransformerProperties.FID);

                        if (premiseRS.RecordCount > 0)
                        {
                            TransactionDEIS.TransactionStatus = TransactionDEIS.TRANS_STATUS_FAILED;
                            TransactionDEIS.TransactionMessage = "A premise is being served by the transformer being removed";
                            return false;
                        }
                    }
                }                

                returnValue = true; 
            }
            catch (Exception ex)
            {
                TransactionDEIS.TransactionStatus = TransactionDEIS.TRANS_STATUS_FAILED;
                TransactionDEIS.TransactionMessage = "Error validating existing Transformer: " + ex.Message;
                returnValue = false;
            }

            return returnValue;
        }

        /// <summary>
        /// Validate existing Voltage Regulator.
        /// </summary>
        /// <returns>Boolean indicating status</returns>
        private bool ValidateExistingVoltageRegulator()
        {
            bool returnValue = false;

            try
            {
                // Query for Voltage Regulators without a Company Number and owned to the Structure ID
                string sqlStatement = "select comm2.g3e_fid " +
                                      "from common_n comm1, common_n comm2, regulator_unit_n reg " +
                                      "where comm1.g3e_fid = ? and comm2.owner1_id = comm1.g3e_id and comm2.g3e_fno in (?) and comm2.g3e_fid = reg.g3e_fid and reg.company_id is null";

                int recordsAffected = 0;
                ADODB.Recordset voltageRegulatorRS = m_DataContext.Execute(sqlStatement, out recordsAffected, (int)CommandTypeEnum.adCmdText, TransactionDEIS.StructureProperties.FID, TransactionDEIS.FNO_VOLTAGE_REGULATOR);

                if (voltageRegulatorRS.RecordCount == 0)
                {
                    TransactionDEIS.TransactionStatus = TransactionDEIS.TRANS_STATUS_FAILED;
                    TransactionDEIS.TransactionMessage = "Voltage Regulator not found at identified Structure.";
                    returnValue = false;
                }
                else if (voltageRegulatorRS.RecordCount > 1)
                {
                    TransactionDEIS.TransactionStatus = TransactionDEIS.TRANS_STATUS_FAILED;
                    TransactionDEIS.TransactionMessage = "Multiple Voltage Regulators found at identified Structure.";
                    returnValue = false;
                }
                else
                {
                    voltageRegulatorRS.MoveFirst();
                    TransactionDEIS.VoltageRegulatorFID = Convert.ToInt32(voltageRegulatorRS.Fields["g3e_fid"].Value);
                    returnValue = true;
                }
            }
            catch (Exception ex)
            {
                TransactionDEIS.TransactionStatus = TransactionDEIS.TRANS_STATUS_FAILED;
                TransactionDEIS.TransactionMessage = "Error validating existing Voltage Regulator: " + ex.Message;
                returnValue = false;
            }

            return returnValue;
        }

        /// <summary>
        /// Validate that Primary Conductors exist at the Structure. Get phases and connecting node.
        /// </summary>
        /// <returns>Boolean indicating status</returns>
        private bool ValidatePrimary()
        {
            bool returnValue = false;

            IGTRelationshipService relationshipService = GTClassFactory.Create<IGTRelationshipService>();
            IGTKeyObject conductorKO = null;

            try
            {
                relationshipService.DataContext = m_DataContext;

                // If Transformer exists then get Primary Conductor connected to the Transformer.
                if (TransactionDEIS.TransformerProperties.FID != 0)
                {
                    IGTKeyObject transformerKO = m_DataContext.OpenFeature(TransactionDEIS.TransformerProperties.FNO, TransactionDEIS.TransformerProperties.FID);
                    relationshipService.ActiveFeature = transformerKO;

                    IGTKeyObjects connectedKOs = relationshipService.GetRelatedFeatures(TransactionDEIS.RNO_CONNECTIVITY);
                    bool foundPrimary = false;
                    foreach (IGTKeyObject connectedKO in connectedKOs)
                    {
                        if (connectedKO.FNO == 8 || connectedKO.FNO == 9 || connectedKO.FNO == 84 || connectedKO.FNO == 85)
                        {
                            TransactionDEIS.PrimaryProperties.FNO = connectedKO.FNO;
                            TransactionDEIS.PrimaryProperties.FID = connectedKO.FID;
                            foundPrimary = true;
                            break;
                        }
                    }

                    if (!foundPrimary)
                    {
                        TransactionDEIS.TransactionStatus = TransactionDEIS.TRANS_STATUS_FAILED;
                        TransactionDEIS.TransactionMessage = "No Primary Conductor connected to existing Transformer";
                        returnValue = false;
                    }
                }
                else
                {
                    IGTKeyObject structureKO = m_DataContext.OpenFeature(TransactionDEIS.StructureProperties.FNO, TransactionDEIS.StructureProperties.FID);

                    relationshipService.ActiveFeature = structureKO;

                    IGTKeyObjects ownedKOs = relationshipService.GetRelatedFeatures(TransactionDEIS.RNO_OWNERSHIP_PARENT);
                    Dictionary<int, short> ownedConductorsDict = new Dictionary<int, short>();

                    foreach (IGTKeyObject ownedKO in ownedKOs)
                    {
                        if (ownedKO.FNO == 8 || ownedKO.FNO == 9 || ownedKO.FNO == 84 || ownedKO.FNO == 85)
                        {
                            ownedConductorsDict.Add(ownedKO.FID, ownedKO.FNO);
                        }
                    }

                    if (ownedConductorsDict.Count > 0)
                    {
                        // Use one Primary Conductor that will be connected to the Transformer
                        TransactionDEIS.PrimaryProperties.FNO = ownedConductorsDict.First().Value;
                        TransactionDEIS.PrimaryProperties.FID = ownedConductorsDict.First().Key;

                        conductorKO = m_DataContext.OpenFeature(TransactionDEIS.PrimaryProperties.FNO, TransactionDEIS.PrimaryProperties.FID);

                        if (ownedConductorsDict.Count > 1)
                        {
                            // Verify that all the primary conductors are connected to a shared node
                            // Use relationship service to confirm that all primary conductors are connected                            
                            relationshipService.ActiveFeature = conductorKO;

                            List<int> connectedConductors = new List<int>();
                            IGTKeyObjects connectedKOs = relationshipService.GetRelatedFeatures(TransactionDEIS.RNO_CONNECTIVITY);
                            foreach(IGTKeyObject connectedKO in connectedKOs)
                            {
                                if (connectedKO.FNO == 8 || connectedKO.FNO == 9 || connectedKO.FNO == 84 || connectedKO.FNO == 85)
                                {
                                    connectedConductors.Add(connectedKO.FID);
                                }
                            }

                            foreach(KeyValuePair<int, short> conductor in ownedConductorsDict)
                            {
                                if (!connectedConductors.Contains(conductor.Key) && conductor.Key != conductorKO.FID)
                                {
                                    TransactionDEIS.PrimaryProperties.FNO = 0;
                                    TransactionDEIS.PrimaryProperties.FID = 0;
                                    TransactionDEIS.TransactionStatus = TransactionDEIS.TRANS_STATUS_FAILED;
                                    TransactionDEIS.TransactionMessage = "Install identifies a structure that owns multiple primary conductors without a shared node";
                                    returnValue = false;
                                }
                            }
                            
                            if (TransactionDEIS.TransactionStatus != TransactionDEIS.TRANS_STATUS_FAILED)
                            {
                                // Determine Primary Conductor node at which to connect the Transformer. Set Primary Conductor property.
                                ADODB.Recordset connRS = conductorKO.Components.GetComponent(TransactionDEIS.CNO_CONNECTIVITY_ATTRIBUTES).Recordset;
                                if (connRS.RecordCount > 0)
                                {
                                    connRS.MoveFirst();
                                    int pricondNode1 = Convert.ToInt32(connRS.Fields["node_1_id"].Value);

                                    IGTKeyObject connectingKO = m_DataContext.OpenFeature(ownedConductorsDict.Last().Value, ownedConductorsDict.Last().Key);

                                    connRS = connectingKO.Components.GetComponent(TransactionDEIS.CNO_CONNECTIVITY_ATTRIBUTES).Recordset;
                                    connRS.MoveFirst();
                                    int connPricondNode1 = Convert.ToInt32(connRS.Fields["node_1_id"].Value);
                                    int connPricondNode2 = Convert.ToInt32(connRS.Fields["node_2_id"].Value);

                                    if (pricondNode1 == connPricondNode1 || pricondNode1 == connPricondNode2)
                                    {
                                        TransactionDEIS.PrimaryProperties.ConnectNode = 1;
                                    }
                                    else
                                    {
                                        TransactionDEIS.PrimaryProperties.ConnectNode = 2;
                                    }
                                }                                
                            }
                        }
                    }
                    else
                    {
                        TransactionDEIS.TransactionStatus = TransactionDEIS.TRANS_STATUS_FAILED;
                        TransactionDEIS.TransactionMessage = "No primary conductors at identified structure";
                        returnValue = false;
                    }
                }

                if (TransactionDEIS.PrimaryProperties.FID != 0)
                {
                    if (conductorKO == null)
                    {
                        conductorKO = m_DataContext.OpenFeature(TransactionDEIS.PrimaryProperties.FNO, TransactionDEIS.PrimaryProperties.FID);
                    }
                    
                    ADODB.Recordset primaryRS = conductorKO.Components.GetComponent(TransactionDEIS.CNO_CONNECTIVITY_ATTRIBUTES).Recordset;
                    if (primaryRS.RecordCount > 0)
                    {
                        // Primary conductor should have a valid protective device ID.
                        primaryRS.MoveFirst();

                        if (Convert.IsDBNull(primaryRS.Fields["PROTECTIVE_DEVICE_FID"].Value))
                        {
                            TransactionDEIS.PrimaryProperties.ProtectiveDeviceID = 0;
                            TransactionDEIS.TransactionStatus = TransactionDEIS.TRANS_STATUS_WARNING;
                            TransactionDEIS.TransactionMessage = "New transformer was placed but protective device did not exist on the primary conductor.  Primary trace may need to be run.";
                        }
                        else
                        {
                            TransactionDEIS.PrimaryProperties.ProtectiveDeviceID = Convert.ToInt32(primaryRS.Fields["PROTECTIVE_DEVICE_FID"].Value);
                        }

                        if (TransactionDEIS.PrimaryProperties.FNO == 8 || TransactionDEIS.PrimaryProperties.FNO == 84)
                        {
                            TransactionDEIS.PrimaryProperties.BankCNO = TransactionDEIS.CNO_PRICOND_OH_BANK;
                            TransactionDEIS.PrimaryProperties.WireCNO = TransactionDEIS.CNO_PRICOND_OH_WIRE;
                        }
                        else
                        {
                            TransactionDEIS.PrimaryProperties.BankCNO = TransactionDEIS.CNO_PRICOND_UG_BANK;
                            TransactionDEIS.PrimaryProperties.WireCNO = TransactionDEIS.CNO_PRICOND_UG_WIRE;
                        }
                    }

                    // Get Primary Conductor phases to compare to Transformer phases
                    string condWirePhase = string.Empty;
                    ADODB.Recordset condWireRS = conductorKO.Components.GetComponent(TransactionDEIS.PrimaryProperties.WireCNO).Recordset;
                    if (condWireRS.RecordCount > 0)
                    {
                        condWireRS.MoveFirst();                        

                        while (!condWireRS.EOF)
                        {
                            TransactionDEIS.PrimaryProperties.Phases += condWireRS.Fields["PHASE_C"].Value.ToString();
                            condWireRS.MoveNext();
                        }

                        condWireRS.MoveFirst();
                        
                        // If Transformer phase code does not contain only A, B, C or any combination thereof, 
                        // then use the phase code to match the phase position(s) (PHASE_POS_C) of the wire record(s) in the OH Primary Conductor.
                        // Use those wire record’s phase(s) to determine the phase value to set on the transformer unit(s).
                        if ((TransactionDEIS.TransformerProperties.FNO == TransactionDEIS.FNO_TRANSFORMER_OH || TransactionDEIS.TransformerProperties.FNO == TransactionDEIS.FNO_TRANSFORMER_OH_NETWORK)
                            && !TransactionDEIS.TransformerProperties.UsePhaseCode)
                        {
                            if (TransactionDEIS.InsTrf1PhaseCode.Length > 0)
                            {
                                condWireRS.Filter = "phase_pos_c = '" + TransactionDEIS.InsTrf1PhaseCode + "'";
                                if (condWireRS.RecordCount > 0)
                                {
                                    TransactionDEIS.InsTrf1PhaseCode = condWireRS.Fields["PHASE_C"].Value.ToString();
                                }
                            }
                            if (TransactionDEIS.InsTrf2PhaseCode.Length > 0)
                            {
                                condWireRS.Filter = "phase_pos_c = '" + TransactionDEIS.InsTrf2PhaseCode + "'";
                                if (condWireRS.RecordCount > 0)
                                {
                                    TransactionDEIS.InsTrf2PhaseCode = condWireRS.Fields["PHASE_C"].Value.ToString();
                                }
                            }
                            if (TransactionDEIS.InsTrf3PhaseCode.Length > 0)
                            {
                                condWireRS.Filter = "phase_pos_c = '" + TransactionDEIS.InsTrf3PhaseCode + "'";
                                if (condWireRS.RecordCount > 0)
                                {
                                    TransactionDEIS.InsTrf3PhaseCode = condWireRS.Fields["PHASE_C"].Value.ToString();
                                }
                            }
                        }
                        else
                        {
                            condWirePhase = condWireRS.Fields["PHASE_C"].Value.ToString();

                            // Validate that 1 phase transformer does not use 2 or 3 phase wire
                            if (TransactionDEIS.InsTrf1PhaseCode.Length == 1 && condWirePhase.Contains(TransactionDEIS.InsTrf1PhaseCode) && condWirePhase.Length > 1)
                            {
                                TransactionDEIS.TransactionStatus = TransactionDEIS.TRANS_STATUS_FAILED;
                                TransactionDEIS.TransactionMessage = string.Format("Install transaction indicates a one-phase code for a {0}-phase conductor.", condWirePhase.Length == 2 ? "two" : "three");
                                returnValue = false;
                            }
                            // Validate that 1 phase transformer does not use 2 or 3 phase wire
                            if (TransactionDEIS.InsTrf2PhaseCode.Length == 1 && condWirePhase.Contains(TransactionDEIS.InsTrf2PhaseCode) && condWirePhase.Length > 1)
                            {
                                TransactionDEIS.TransactionStatus = TransactionDEIS.TRANS_STATUS_FAILED;
                                TransactionDEIS.TransactionMessage = string.Format("Install transaction indicates a one-phase code for a {0}-phase conductor.", condWirePhase.Length == 2 ? "two" : "three");
                                returnValue = false;
                            }
                            // Validate that 1 phase transformer does not use 2 or 3 phase wire
                            if (TransactionDEIS.InsTrf3PhaseCode.Length == 1 && condWirePhase.Contains(TransactionDEIS.InsTrf3PhaseCode) && condWirePhase.Length > 1)
                            {
                                TransactionDEIS.TransactionStatus = TransactionDEIS.TRANS_STATUS_FAILED;
                                TransactionDEIS.TransactionMessage = string.Format("Install transaction indicates a one-phase code for a {0}-phase conductor.", condWirePhase.Length == 2 ? "two" : "three");
                                returnValue = false;
                            }
                        }

                        // Validate that Transformer phase exists on Primary Conductor 
                        TransactionDEIS.PrimaryProperties.Phases = string.Concat(TransactionDEIS.PrimaryProperties.Phases.OrderBy(c => c));
                        if (TransactionDEIS.InsTrf1PhaseCode.Length > 0 && !TransactionDEIS.PrimaryProperties.Phases.Contains(TransactionDEIS.InsTrf1PhaseCode))
                        {
                            TransactionDEIS.TransactionStatus = TransactionDEIS.TRANS_STATUS_FAILED;
                            TransactionDEIS.TransactionMessage = string.Format("No conductor found for phase: {0}", TransactionDEIS.InsTrf1PhaseCode);
                            returnValue = false;
                        }
                        if (TransactionDEIS.InsTrf2PhaseCode.Length > 0 && !TransactionDEIS.PrimaryProperties.Phases.Contains(TransactionDEIS.InsTrf2PhaseCode))
                        {
                            TransactionDEIS.TransactionStatus = TransactionDEIS.TRANS_STATUS_FAILED;
                            TransactionDEIS.TransactionMessage = string.Format("No conductor found for phase: {0}", TransactionDEIS.InsTrf2PhaseCode);
                            returnValue = false;
                        }
                        if (TransactionDEIS.InsTrf3PhaseCode.Length > 0 && !TransactionDEIS.PrimaryProperties.Phases.Contains(TransactionDEIS.InsTrf3PhaseCode))
                        {
                            TransactionDEIS.TransactionStatus = TransactionDEIS.TRANS_STATUS_FAILED;
                            TransactionDEIS.TransactionMessage = string.Format("No conductor found for phase: {0}", TransactionDEIS.InsTrf3PhaseCode);
                            returnValue = false;
                        }

                        TransactionDEIS.PrimaryProperties.PhaseCount = TransactionDEIS.PrimaryProperties.Phases.Length;

                        // Transformer count must be less than or equal to the number of available phases
                        if (TransactionDEIS.TransformerProperties.PhaseCount > TransactionDEIS.PrimaryProperties.PhaseCount)
                        {
                            TransactionDEIS.TransactionStatus = TransactionDEIS.TRANS_STATUS_FAILED;
                            TransactionDEIS.TransactionMessage = string.Format("Install would result in a greater number of transformers ({0}) than the number of phases ({1})", TransactionDEIS.TransformerProperties.PhaseCount, TransactionDEIS.PrimaryProperties.PhaseCount);
                            returnValue = false;
                        }
                    }

                    if (TransactionDEIS.TransactionStatus != TransactionDEIS.TRANS_STATUS_FAILED)
                    {
                        returnValue = true;
                    }                    
                }                
            }
            catch (Exception ex)
            {
                TransactionDEIS.TransactionStatus = TransactionDEIS.TRANS_STATUS_FAILED;
                TransactionDEIS.TransactionMessage = "Error validating Primary Conductor: " + ex.Message;
                returnValue = false;
            }

            relationshipService.Dispose();

            return returnValue;
        }

        /// <summary>
        /// Validate that a Secondary Conductor or Service Line exists at the Structure.
        /// </summary>
        /// <returns>Boolean indicating status</returns>
        private bool ValidateSecondary()
        {
            bool returnValue = false;

            try
            {
                // Query for Structure ID
                string sqlStatement = string.Format("select comm2.g3e_fno, comm2.g3e_fid, comm2.feature_state_c " +
                                      "from common_n comm1, common_n comm2 " +
                                      "where comm1.g3e_fid = ? and (comm2.owner1_id = comm1.g3e_id or comm2.owner2_id = comm1.g3e_id) and comm2.g3e_fno in ({0}, {1}, {2})",
                                      TransactionDEIS.FNO_SECONDARY_CONDUCTOR_OH, TransactionDEIS.FNO_SECONDARY_CONDUCTOR_UG, TransactionDEIS.FNO_SERVICE_LINE);

                int recordsAffected = 0;
                ADODB.Recordset secondaryRS = m_DataContext.Execute(sqlStatement, out recordsAffected, (int)CommandTypeEnum.adCmdText, TransactionDEIS.StructureProperties.FID);

                if (secondaryRS.RecordCount == 1)
                {
                    // Set Secondary properties
                    secondaryRS.MoveFirst();
                    TransactionDEIS.SecondaryProperties.FNO = Convert.ToInt16(secondaryRS.Fields["G3E_FNO"].Value);
                    TransactionDEIS.SecondaryProperties.FID = Convert.ToInt32(secondaryRS.Fields["G3E_FID"].Value);
                    TransactionDEIS.SecondaryProperties.FeatureState = secondaryRS.Fields["FEATURE_STATE_C"].Value.ToString();
                }
                else if (secondaryRS.RecordCount == 0)
                {
                    TransactionDEIS.TransactionStatus = TransactionDEIS.TransactionStatus = TransactionDEIS.TRANS_STATUS_WARNING;;
                    TransactionDEIS.TransactionMessage = "Secondary connection was indicated but no Secondary Conductor or Service Line found at indicated structure";
                }

                returnValue = true;
            }
            catch (Exception ex)
            {
                TransactionDEIS.TransactionStatus = TransactionDEIS.TRANS_STATUS_FAILED;
                TransactionDEIS.TransactionMessage = "Error validating Secondary connection: " + ex.Message;
                returnValue = false;
            }

            return returnValue;
        }

        /// <summary>
        /// Count the number of phases for the Transformer.
        /// </summary>
        /// <returns>Boolean indicating status</returns>
        private bool ValidateTransformerPhases()
        {
            bool returnValue = false;

            try
            {
                // Count the total number of characters in the phase codes for all the INS transactions.
                // Add the number of phases to be installed to the number of existing phases (zero if new)
                TransactionDEIS.TransformerProperties.PhaseCount += TransactionDEIS.InsTrf1PhaseCode.Length + TransactionDEIS.InsTrf2PhaseCode.Length + TransactionDEIS.InsTrf3PhaseCode.Length;
                
                returnValue = true;
            }
            catch (Exception ex)
            {
                TransactionDEIS.TransactionStatus = TransactionDEIS.TRANS_STATUS_FAILED;
                TransactionDEIS.TransactionMessage = "Error determining number of phases: " + ex.Message;
                returnValue = false;
            }

            return returnValue;
        }

        /// <summary>
        /// Determine the phasing rule to use.
        /// </summary>
        /// <param name="phaseValue">Phase value to check. If phase value does not contain A, B, or C or combination of ABC then use phase position</param>
        /// <returns>Boolean indicating status</returns>
        private bool SetPhasingRule(string phaseValue)
        {
            bool returnValue = false;

            try
            {
                if (Regex.IsMatch(phaseValue, @"^[AaBbCc]+$"))
                {
                    TransactionDEIS.TransformerProperties.UsePhaseCode = true;
                }
                else
                {
                    TransactionDEIS.TransformerProperties.UsePhaseCode = false;
                }

                returnValue = true;
            }
            catch (Exception ex)
            {
                TransactionDEIS.TransactionStatus = TransactionDEIS.TRANS_STATUS_FAILED;
                TransactionDEIS.TransactionMessage = "Error determining phasing rule: " + ex.Message;
                returnValue = false;
            }

            return returnValue;
        }

        /// <summary>
        /// Validate the wire configuration using the wiring configuration code and the number of phases for the Transformer.
        /// </summary>
        /// <returns>Boolean indicating status</returns>
        private bool ValidateWiringConfiguration()
        {
            bool returnValue = false;

            try
            {
                if (TransactionDEIS.WiringConfigCode.Length > 0)
                {
                    TransactionDEIS.TransformerProperties.PrimaryWiringConfiguration = TransactionDEIS.WiringConfigCode.Substring(0, TransactionDEIS.WiringConfigCode.IndexOf(' ')).ToUpper().Trim();
                    TransactionDEIS.TransformerProperties.SecondaryWiringConfiguration = TransactionDEIS.WiringConfigCode.Substring(TransactionDEIS.WiringConfigCode.IndexOf(' ')).ToUpper().Trim();

                    if (TransactionDEIS.TransformerProperties.PrimaryWiringConfiguration == "SINGLE")
                    {
                        if (TransactionDEIS.TransformerProperties.PhaseCount != 1)
                        {
                            // SINGLE is the only valid primary configuration for a one phase Transformer
                            TransactionDEIS.TransactionStatus = TransactionDEIS.TRANS_STATUS_FAILED;
                            TransactionDEIS.TransactionMessage = string.Format("Primary Configuration specified as SINGLE where {0} transformers will exist.", TransactionDEIS.TransformerProperties.PhaseCount);
                            return false;
                        }
                    }
                    else if (TransactionDEIS.TransformerProperties.PrimaryWiringConfiguration == "OPENWYE" || TransactionDEIS.TransformerProperties.PrimaryWiringConfiguration == "OPENDELTA")
                    {
                        if (TransactionDEIS.TransformerProperties.PhaseCount != 2)
                        {
                            // OPENWYE and OPENDELTA are the only valid primary configurations for a two phase Transformer
                            TransactionDEIS.TransactionStatus = TransactionDEIS.TRANS_STATUS_FAILED;
                            TransactionDEIS.TransactionMessage = string.Format("{0} primary configuration is not valid for {1} transformer(s)", TransactionDEIS.TransformerProperties.PrimaryWiringConfiguration, TransactionDEIS.TransformerProperties.PhaseCount);
                            return false;
                        }
                    }
                    else if (TransactionDEIS.TransformerProperties.PrimaryWiringConfiguration == "WYE")
                    {
                        if (TransactionDEIS.TransformerProperties.PhaseCount != 3)
                        {
                            // WYE is only valid for a three phase Transformer
                            TransactionDEIS.TransactionStatus = TransactionDEIS.TRANS_STATUS_FAILED;
                            TransactionDEIS.TransactionMessage = string.Format("Primary Configuration specified as WYE where only {0} transformer will exist", TransactionDEIS.TransformerProperties.PhaseCount);
                            return false;
                        }
                    }
                    else if (TransactionDEIS.TransformerProperties.PrimaryWiringConfiguration == "DELTA")
                    {
                        if (TransactionDEIS.TransformerProperties.PhaseCount == 1)
                        {
                            // DELTA isn't valid for a single phase Transformer
                            TransactionDEIS.TransactionStatus = TransactionDEIS.TRANS_STATUS_FAILED;
                            TransactionDEIS.TransactionMessage = "Transaction identifies number of phases = 1 and the primary configuration is DELTA.";
                            return false;
                        }
                    }
                    else
                    {
                        TransactionDEIS.TransactionStatus = TransactionDEIS.TRANS_STATUS_FAILED;
                        if (TransactionDEIS.TransactionCode != "R")
                        {
                            TransactionDEIS.TransactionMessage = "Transaction identifies a Primary configuration that does not agree with the given number of phases.";
                        }
                        else
                        {
                            TransactionDEIS.TransactionMessage = "Removal will leave the transformer with an invalid primary wiring configuration";
                        }
                        return false;
                    }

                    returnValue = true;
                }
                else
                {
                    TransactionDEIS.TransactionStatus = TransactionDEIS.TRANS_STATUS_FAILED;
                    TransactionDEIS.TransactionMessage = "Primary configuration is null";
                    return false;
                }                
            }
            catch (Exception ex)
            {
                TransactionDEIS.TransactionStatus = TransactionDEIS.TRANS_STATUS_FAILED;
                TransactionDEIS.TransactionMessage = "Error validating the wire configuration: " + ex.Message;
                returnValue = false;
            }

            return returnValue;
        }

        /// <summary>
        /// Determines the Transformer feature type and sets the feature properties (G3E_FNO, G3E_CNO).
        /// </summary>
        /// <returns>Boolean indicating status</returns>
        private bool SetTransformerFeatureProperties()
        {
            bool returnValue = false;

            try
            {
                if (TransactionDEIS.TransformerProperties.FNO == TransactionDEIS.FNO_AUTOTRANSFORMER)
                {
                    // Auto Transformer
                    TransactionDEIS.TransformerProperties.PrimaryGraphicCNO = TransactionDEIS.CNO_AUTOTRANSFORMER_PRIMARYGRAPHIC;
                    TransactionDEIS.TransformerProperties.LabelCNO = TransactionDEIS.CNO_AUTOTRANSFORMER_LABEL;
                    TransactionDEIS.TransformerProperties.BankCNO = TransactionDEIS.CNO_AUTOTRANSFORMER_BANK;
                    TransactionDEIS.TransformerProperties.UnitCNO = TransactionDEIS.CNO_AUTOTRANSFORMER_UNIT;
                    TransactionDEIS.TransformerProperties.BankTable = TransactionDEIS.TABLE_AUTOTRANSFORMER_BANK;
                    TransactionDEIS.TransformerProperties.UnitTable = TransactionDEIS.TABLE_AUTOTRANSFORMER_UNIT;
                }
                else if (TransactionDEIS.TransformerProperties.FNO == TransactionDEIS.FNO_TRANSFORMER_OH_NETWORK || TransactionDEIS.TransformerProperties.FNO == TransactionDEIS.FNO_TRANSFORMER_OH)
                {
                    TransactionDEIS.TransformerProperties.PrimaryGraphicCNO = TransactionDEIS.CNO_TRANSFORMER_OH_PRIMARYGRAPHIC;
                    TransactionDEIS.TransformerProperties.LabelCNO = TransactionDEIS.CNO_TRANSFORMER_OH_LABEL;
                    TransactionDEIS.TransformerProperties.BankCNO = TransactionDEIS.CNO_TRANSFORMER_OH_BANK;
                    TransactionDEIS.TransformerProperties.UnitCNO = TransactionDEIS.CNO_TRANSFORMER_OH_UNIT;
                    TransactionDEIS.TransformerProperties.BankTable = TransactionDEIS.TABLE_TRANSFORMER_OH_BANK;
                    TransactionDEIS.TransformerProperties.UnitTable = TransactionDEIS.TABLE_TRANSFORMER_OH_UNIT;
                }
                else
                {
                    TransactionDEIS.TransformerProperties.PrimaryGraphicCNO = TransactionDEIS.CNO_TRANSFORMER_UG_PRIMARYGRAPHIC;
                    TransactionDEIS.TransformerProperties.LabelCNO = TransactionDEIS.CNO_TRANSFORMER_UG_LABEL;
                    TransactionDEIS.TransformerProperties.BankCNO = 0;
                    TransactionDEIS.TransformerProperties.UnitCNO = TransactionDEIS.CNO_TRANSFORMER_UG_UNIT;
                    TransactionDEIS.TransformerProperties.BankTable = "";
                    TransactionDEIS.TransformerProperties.UnitTable = TransactionDEIS.TABLE_TRANSFORMER_UG_UNIT;
                }

                returnValue = true;
            }
            catch (Exception ex)
            {
                TransactionDEIS.TransactionStatus = TransactionDEIS.TRANS_STATUS_FAILED;
                TransactionDEIS.TransactionMessage = "Error setting Transformer feature properties: " + ex.Message;
                returnValue = false;
            }

            return returnValue;
        }
    }
}
