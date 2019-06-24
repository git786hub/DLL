using System;
using Intergraph.GTechnology.API;
using ADODB;
using System.Xml;

namespace GTechnology.Oncor.CustomAPI
{
    public class GISAuto_DEIS : IGISAutoPlugin
    {
        private IGTTransactionManager m_TransactionManager;
        private IGTApplication m_Application = GTClassFactory.Create<IGTApplication>();
        

        public GISAuto_DEIS(IGTTransactionManager transactionManager)
        {
            m_TransactionManager = transactionManager;
        }

        public string SystemName
        {
            get
            {
                return "DEIS";
            }
        }

        /// <summary>
        /// The main entry point for the class.
        /// </summary>
        /// <param name="autoRequest">Contains the needed information to process the transaction</param>
        /// <returns>Boolean indicating status</returns>
        public void Execute(GISAutoRequest autoRequest)
        {
            TransactionDEIS.Initialize();

            if (ParseXML(autoRequest.requestXML.ToString()))
            {
                // Update the DEIS_TRANSACTION table to indicate that the transaction is being processed.
                TransactionDEIS.TransactionStatus = TransactionDEIS.TRANS_STATUS_PROCESSING;
                if (!UpdateTransactionTable())
                {
                    return;
                }

                ProcessTransaction();

                // Limit transaction message to size of DEIS_TRANSACTION.TRAN_MSG
                if (TransactionDEIS.TransactionMessage.Length > 1000)
                {
                    TransactionDEIS.TransactionMessage = TransactionDEIS.TransactionMessage.Substring(0, 1000);
                }

                // Update the transaction table with the status code and message
                if (!UpdateTransactionTable())
                {
                    return;
                }
            }

            // Send response to EdgeFrontier
            SendResponse(autoRequest.requestXML.ToString());
        }

        /// <summary>
        /// Send the response message with the status of the transaction to EdgeFrontier.
        /// </summary>
        /// <param name="xml">XML string containing DEIS transaction</param>
        /// <returns>Boolean indicating status</returns>
        private bool SendResponse(string xml)
        {
            bool returnValue = false;

            try
            {
                // Get URL from metadata
                string sql = "select param_value from sys_generalparameter where subsystem_name = ? and subsystem_component = ? and param_name = ?";
                string responseURL = string.Empty;

                ADODB.Recordset metadataRS = m_Application.DataContext.OpenRecordset(sql, CursorTypeEnum.adOpenDynamic, LockTypeEnum.adLockReadOnly, -1, "GISAUTO_DEIS", "URL", "DEIS_RESPONSE_WEB_ADDRESS");

                if (metadataRS.RecordCount > 0)
                {
                    metadataRS.MoveFirst();
                    responseURL = metadataRS.Fields["PARAM_VALUE"].Value.ToString();
                }
                else
                {
                    DEISException exception = new DEISException(2, "Unable to retrieve the DEIS_GISTransactionResults url from the SYS_GENERALPARAMETER table.");
                    throw exception;
                }

                string responseStatus = string.Empty;
                string responseMessage = string.Empty;

                if (TransactionDEIS.TransactionMessage.ToLower().StartsWith("failed"))
                {
                    responseStatus = "FAILED";
                    responseMessage = TransactionDEIS.TransactionMessage;
                }
                else if (TransactionDEIS.TransactionMessage.ToLower().StartsWith("warning"))
                {
                    responseStatus = "WARNING";
                    responseMessage = TransactionDEIS.TransactionMessage;
                }
                else
                {
                    responseStatus = "COMPLETED";
                    responseMessage = "";
                }

                // Build message
                string rspxml = @"<?xml version = ""1.0"" encoding = ""UTF-8""?>";
                rspxml += @"<NotifyDEISProcessingStatus  xmlns = ""http://www.oncor.com/DIS_DEISProcessingStatusNotify""";
                rspxml += @" xmlns:xsi = ""http://www.w3.org/2001/XMLSchema-instance""";
                rspxml += @" xsi:schemaLocation = ""http://www.oncor.com/DIS_DEISProcessingStatusNotify ../../Schema/DEISProcessingStatus.xsd"">";
                rspxml += @"  <RequestHeader xmlns = ""http://www.oncor.com/DIS"">";
                rspxml += @"    <SourceSystem>GIS</SourceSystem>";
                rspxml += string.Format(@"    <TransactionId>{0}</TransactionId>", TransactionDEIS.TranID);
                rspxml += @"    <TransactionType>REQREP</TransactionType>";
                rspxml += string.Format(@"    <Requestor>{0}</Requestor>", TransactionDEIS.Requestor);
                rspxml += @"  </RequestHeader>";
                rspxml += string.Format(@"    <TransactionNumber>{0}</TransactionNumber>", TransactionDEIS.TranID);
                rspxml += @"  <ResultStatus xmlns = ""http://www.oncor.com/DIS"">";
                rspxml += string.Format(@"    <Status>{0}</Status>", responseStatus);
                rspxml += string.Format(@"    <ErrorMsg>{0}</ErrorMsg>", responseMessage);
                rspxml += @"  </ResultStatus>";
                rspxml += @"</NotifyDEISProcessingStatus>";

                // Send message
                SendReceiveXMLMessage XMLMessenger = new SendReceiveXMLMessage();
                XMLMessenger.URL = responseURL;
                XMLMessenger.Method = "POST";
                XMLMessenger.ContentType = "text/xml";
                XMLMessenger.RequestXMLBody = rspxml;
                XMLMessenger.SendMsgToEF();

                returnValue = true;
            }
            catch (Exception ex)
            {
                DEISException exception = new DEISException(3, "Error sending response: " + ex.Message);
                throw exception;
            }

            return returnValue;
        }

        /// <summary>
        /// Parses the input XML message and sets the transaction properties.
        /// </summary>, namespaceManager)
        /// <param name="xml">XML string containing DEIS transaction</param>
        /// <returns>Boolean indicating status</returns>
        private bool ParseXML(string xml)
        {
            bool returnValue = false;

            try
            {
                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.LoadXml(xml);

                XmlNamespaceManager namespaceManager = new XmlNamespaceManager(xmlDoc.NameTable);
                namespaceManager.AddNamespace("ns0", "http://www.oncor.com/GIS/DEISTransaction");

                XmlNode transaction = xmlDoc.SelectSingleNode("ns0:" + "RequestDEISTransaction", namespaceManager);
                TransactionDEIS.TransactionDate = transaction.SelectSingleNode("ns0:" + "TransactionDate", namespaceManager).InnerText;
                TransactionDEIS.TransactionCode = transaction.SelectSingleNode("ns0:" + "TransactionCode", namespaceManager).InnerText;
                TransactionDEIS.BankFLN = transaction.SelectSingleNode("ns0:" + "StructureID", namespaceManager).InnerText;
                try
                {
                    TransactionDEIS.WrNumber = transaction.SelectSingleNode("ns0:" + "WRNumber", namespaceManager).InnerText;
                }
                catch
                {
                    // Do nothing. Element is not required to be in transaction.
                }
                try
                {
                    TransactionDEIS.ServiceArea = transaction.SelectSingleNode("ns0:" + "ServiceArea", namespaceManager).InnerText;
                }
                catch
                {
                    // Do nothing. Element is not required to be in transaction.
                }
                try
                {
                    TransactionDEIS.Yard = transaction.SelectSingleNode("ns0:" + "Yard", namespaceManager).InnerText;
                }
                catch
                {
                    // Do nothing. Element is not required to be in transaction.
                }
                try
                {
                    TransactionDEIS.UserID = transaction.SelectSingleNode("ns0:" + "UserID", namespaceManager).InnerText;
                }
                catch
                {
                    // Do nothing. Element is not required to be in transaction.
                }
                try
                {
                    TransactionDEIS.SecondaryConnection = transaction.SelectSingleNode("ns0:" + "ConnectSecondary", namespaceManager).InnerText;
                }
                catch
                {
                    // Do nothing. Element is not required to be in transaction.
                }
                try
                {
                    TransactionDEIS.ExistingBank = transaction.SelectSingleNode("ns0:" + "ExistingBank", namespaceManager).InnerText;
                }
                catch
                {
                    // Do nothing. Element is not required to be in transaction.
                }
                try
                {
                    TransactionDEIS.WiringConfigCode = transaction.SelectSingleNode("ns0:" + "WiringConfiguration", namespaceManager).InnerText;
                }
                catch
                {
                    // Do nothing. Element is not required to be in transaction.
                }
                try
                {
                    TransactionDEIS.Location = transaction.SelectSingleNode("ns0:" + "Location", namespaceManager).InnerText;
                }
                catch
                {
                    // Do nothing. Element is not required to be in transaction.
                }

                XmlNode transformer;

                try
                {
                    transformer = transaction.SelectSingleNode("ns0:" + "InstallTRF1", namespaceManager);
                    TransactionDEIS.InsTrf1CompanyNumber = transformer.SelectSingleNode("ns0:" + "CompanyNumber", namespaceManager).InnerText;
                    TransactionDEIS.InsTrf1TSN = transformer.SelectSingleNode("ns0:" + "TSN", namespaceManager).InnerText;
                    if (transformer.SelectSingleNode("ns0:" + "KVA", namespaceManager).InnerText.Length > 0)
                    {
                        TransactionDEIS.InsTrf1KvaSize = Convert.ToDouble(transformer.SelectSingleNode("ns0:" + "KVA", namespaceManager).InnerText);
                    }
                    TransactionDEIS.InsTrf1PhaseCode = transformer.SelectSingleNode("ns0:" + "Phase", namespaceManager).InnerText;
                    TransactionDEIS.InsTrf1TypeCode = transformer.SelectSingleNode("ns0:" + "TypeCode", namespaceManager).InnerText;
                    TransactionDEIS.InsTrf1MountCode = transformer.SelectSingleNode("ns0:" + "MountCode", namespaceManager).InnerText;
                    TransactionDEIS.InsTrf1KindCode = transformer.SelectSingleNode("ns0:" + "KindCode", namespaceManager).InnerText;
                    TransactionDEIS.InsTrf1PriVoltCode = transformer.SelectSingleNode("ns0:" + "PriVoltCode", namespaceManager).InnerText;
                    TransactionDEIS.InsTrf1SecVoltCode = transformer.SelectSingleNode("ns0:" + "SecVoltCode", namespaceManager).InnerText;
                }
                catch
                {
                    // Do nothing. All elements are not needed for all transactions.
                }

                try
                {
                    transformer = transaction.SelectSingleNode("ns0:" + "InstallTRF2", namespaceManager);
                    TransactionDEIS.InsTrf2CompanyNumber = transformer.SelectSingleNode("ns0:" + "CompanyNumber", namespaceManager).InnerText;
                    TransactionDEIS.InsTrf2TSN = transformer.SelectSingleNode("ns0:" + "TSN", namespaceManager).InnerText;
                    if (transformer.SelectSingleNode("ns0:" + "KVA", namespaceManager).InnerText.Length > 0)
                    {
                        TransactionDEIS.InsTrf2KvaSize = Convert.ToDouble(transformer.SelectSingleNode("ns0:" + "KVA", namespaceManager).InnerText);
                    }
                    TransactionDEIS.InsTrf2PhaseCode = transformer.SelectSingleNode("ns0:" + "Phase", namespaceManager).InnerText;
                    TransactionDEIS.InsTrf2TypeCode = transformer.SelectSingleNode("ns0:" + "TypeCode", namespaceManager).InnerText;
                    TransactionDEIS.InsTrf2MountCode = transformer.SelectSingleNode("ns0:" + "MountCode", namespaceManager).InnerText;
                    TransactionDEIS.InsTrf2KindCode = transformer.SelectSingleNode("ns0:" + "KindCode", namespaceManager).InnerText;
                    TransactionDEIS.InsTrf2PriVoltCode = transformer.SelectSingleNode("ns0:" + "PriVoltCode", namespaceManager).InnerText;
                    TransactionDEIS.InsTrf2SecVoltCode = transformer.SelectSingleNode("ns0:" + "SecVoltCode", namespaceManager).InnerText;
                }
                catch
                {
                    // Do nothing. All elements are not needed for all transactions.
                }

                try
                {
                    transformer = transaction.SelectSingleNode("ns0:" + "InstallTRF3", namespaceManager);
                    TransactionDEIS.InsTrf3CompanyNumber = transformer.SelectSingleNode("ns0:" + "CompanyNumber", namespaceManager).InnerText;
                    TransactionDEIS.InsTrf3TSN = transformer.SelectSingleNode("ns0:" + "TSN", namespaceManager).InnerText;
                    if (transformer.SelectSingleNode("ns0:" + "KVA", namespaceManager).InnerText.Length > 0)
                    {
                        TransactionDEIS.InsTrf3KvaSize = Convert.ToDouble(transformer.SelectSingleNode("ns0:" + "KVA", namespaceManager).InnerText);
                    }
                    TransactionDEIS.InsTrf3PhaseCode = transformer.SelectSingleNode("ns0:" + "Phase", namespaceManager).InnerText;
                    TransactionDEIS.InsTrf3TypeCode = transformer.SelectSingleNode("ns0:" + "TypeCode", namespaceManager).InnerText;
                    TransactionDEIS.InsTrf3MountCode = transformer.SelectSingleNode("ns0:" + "MountCode", namespaceManager).InnerText;
                    TransactionDEIS.InsTrf3KindCode = transformer.SelectSingleNode("ns0:" + "KindCode", namespaceManager).InnerText;
                    TransactionDEIS.InsTrf3PriVoltCode = transformer.SelectSingleNode("ns0:" + "PriVoltCode", namespaceManager).InnerText;
                    TransactionDEIS.InsTrf3SecVoltCode = transformer.SelectSingleNode("ns0:" + "SecVoltCode", namespaceManager).InnerText;
                }
                catch
                {
                    // Do nothing. All elements are not needed for all transactions.
                }

                try
                {
                    transformer = transaction.SelectSingleNode("ns0:" + "RemoveTRF1", namespaceManager);
                    TransactionDEIS.RemTrf1CompanyNumber = transformer.SelectSingleNode("ns0:" + "CompanyNumber", namespaceManager).InnerText;
                    TransactionDEIS.RemTrf1TSN = transformer.SelectSingleNode("ns0:" + "TSN", namespaceManager).InnerText;
                    if (transformer.SelectSingleNode("ns0:" + "KVA", namespaceManager).InnerText.Length > 0)
                    {
                        TransactionDEIS.RemTrf1KvaSize = Convert.ToDouble(transformer.SelectSingleNode("ns0:" + "KVA", namespaceManager).InnerText);
                    }
                    TransactionDEIS.RemTrf1PhaseCode = transformer.SelectSingleNode("ns0:" + "Phase", namespaceManager).InnerText;
                    TransactionDEIS.RemTrf1TypeCode = transformer.SelectSingleNode("ns0:" + "TypeCode", namespaceManager).InnerText;
                    TransactionDEIS.RemTrf1MountCode = transformer.SelectSingleNode("ns0:" + "MountCode", namespaceManager).InnerText;
                    TransactionDEIS.RemTrf1KindCode = transformer.SelectSingleNode("ns0:" + "KindCode", namespaceManager).InnerText;
                    TransactionDEIS.RemTrf1PriVoltCode = transformer.SelectSingleNode("ns0:" + "PriVoltCode", namespaceManager).InnerText;
                    TransactionDEIS.RemTrf1SecVoltCode = transformer.SelectSingleNode("ns0:" + "SecVoltCode", namespaceManager).InnerText;
                }
                catch
                {
                    // Do nothing. All elements are not needed for all transactions.
                }

                try
                {
                    transformer = transaction.SelectSingleNode("ns0:" + "RemoveTRF2", namespaceManager);
                    TransactionDEIS.RemTrf2CompanyNumber = transformer.SelectSingleNode("ns0:" + "CompanyNumber", namespaceManager).InnerText;
                    TransactionDEIS.RemTrf2TSN = transformer.SelectSingleNode("ns0:" + "TSN", namespaceManager).InnerText;
                    if (transformer.SelectSingleNode("ns0:" + "KVA", namespaceManager).InnerText.Length > 0)
                    {
                        TransactionDEIS.RemTrf2KvaSize = Convert.ToDouble(transformer.SelectSingleNode("ns0:" + "KVA", namespaceManager).InnerText);
                    }
                    TransactionDEIS.RemTrf2PhaseCode = transformer.SelectSingleNode("ns0:" + "Phase", namespaceManager).InnerText;
                    TransactionDEIS.RemTrf2TypeCode = transformer.SelectSingleNode("ns0:" + "TypeCode", namespaceManager).InnerText;
                    TransactionDEIS.RemTrf2MountCode = transformer.SelectSingleNode("ns0:" + "MountCode", namespaceManager).InnerText;
                    TransactionDEIS.RemTrf2KindCode = transformer.SelectSingleNode("ns0:" + "KindCode", namespaceManager).InnerText;
                    TransactionDEIS.RemTrf2PriVoltCode = transformer.SelectSingleNode("ns0:" + "PriVoltCode", namespaceManager).InnerText;
                    TransactionDEIS.RemTrf2SecVoltCode = transformer.SelectSingleNode("ns0:" + "SecVoltCode", namespaceManager).InnerText;
                }
                catch
                {
                    // Do nothing. All elements are not needed for all transactions.
                }

                try
                {
                    transformer = transaction.SelectSingleNode("ns0:" + "RemoveTRF3", namespaceManager);
                    TransactionDEIS.RemTrf3CompanyNumber = transformer.SelectSingleNode("ns0:" + "CompanyNumber", namespaceManager).InnerText;
                    TransactionDEIS.RemTrf3TSN = transformer.SelectSingleNode("ns0:" + "TSN", namespaceManager).InnerText;
                    if (transformer.SelectSingleNode("ns0:" + "KVA", namespaceManager).InnerText.Length > 0)
                    {
                        TransactionDEIS.RemTrf3KvaSize = Convert.ToDouble(transformer.SelectSingleNode("ns0:" + "KVA", namespaceManager).InnerText);
                    }
                    TransactionDEIS.RemTrf3PhaseCode = transformer.SelectSingleNode("ns0:" + "Phase", namespaceManager).InnerText;
                    TransactionDEIS.RemTrf3TypeCode = transformer.SelectSingleNode("ns0:" + "TypeCode", namespaceManager).InnerText;
                    TransactionDEIS.RemTrf3MountCode = transformer.SelectSingleNode("ns0:" + "MountCode", namespaceManager).InnerText;
                    TransactionDEIS.RemTrf3KindCode = transformer.SelectSingleNode("ns0:" + "KindCode", namespaceManager).InnerText;
                    TransactionDEIS.RemTrf3PriVoltCode = transformer.SelectSingleNode("ns0:" + "PriVoltCode", namespaceManager).InnerText;
                    TransactionDEIS.RemTrf3SecVoltCode = transformer.SelectSingleNode("ns0:" + "SecVoltCode", namespaceManager).InnerText;
                }
                catch
                {
                    // Do nothing. All elements are not needed for all transactions.
                }

                XmlNamespaceManager namespaceManager1 = new XmlNamespaceManager(xmlDoc.NameTable);
                namespaceManager1.AddNamespace("ns1", "http://www.oncor.com/DIS");

                XmlNode header = transaction.SelectSingleNode("ns1:" + "RequestHeader", namespaceManager1);
                TransactionDEIS.TranID = header.SelectSingleNode("ns1:" + "TransactionId", namespaceManager1).InnerText;
                TransactionDEIS.Requestor = header.SelectSingleNode("ns1:" + "Requestor", namespaceManager1).InnerText;

                // Write the transaction to the DEIS_TRANSACTION table.
                if (!InsertTransaction())
                {
                    return false;
                }

                returnValue = true;
            }
            catch (Exception ex)
            {
                TransactionDEIS.TransactionMessage = "FAILED - Error Parsing XML: " + ex.Message;
                returnValue = false;
            }

            return returnValue;
        }

        /// <summary>
        /// Inserts a record into the DEIS_TRANSACTION table.
        /// </summary>
        /// <returns>Boolean indicating status</returns>
        private bool InsertTransaction()
        {
            bool returnValue = false;

            try
            {
               ADODB.Command cmd = new ADODB.Command();

                cmd.CommandText = "insert into deis_transaction (tran_code,tran_date,bank_fln,wr_no,serv_area,yard,user_id,conn_sec,existing_bank,wiring_config_code," +
                                  "locn,ins_trf1_company_no,ins_trf1_tsn,ins_trf1_kva_size,ins_trf1_phs_code,ins_trf1_type_code,ins_trf1_mount_code,ins_trf1_kind_code," +
                                  "ins_trf1_pri_volt_code,ins_trf1_sec_volt_code,ins_trf2_company_no,ins_trf2_tsn,ins_trf2_kva_size,ins_trf2_phs_code,ins_trf2_type_code," +
                                  "ins_trf2_mount_code,ins_trf2_kind_code,ins_trf2_pri_volt_code,ins_trf2_sec_volt_code,ins_trf3_company_no,ins_trf3_tsn,ins_trf3_kva_size," +
                                  "ins_trf3_phs_code,ins_trf3_type_code,ins_trf3_mount_code,ins_trf3_kind_code,ins_trf3_pri_volt_code,ins_trf3_sec_volt_code,rem_trf1_company_no," +
                                  "rem_trf1_tsn,rem_trf1_kva_size,rem_trf1_phs_code,rem_trf1_type_code,rem_trf1_mount_code,rem_trf1_kind_code,rem_trf1_pri_volt_code," +
                                  "rem_trf1_sec_volt_code,rem_trf2_company_no,rem_trf2_tsn,rem_trf2_kva_size,rem_trf2_phs_code,rem_trf2_type_code,rem_trf2_mount_code," +
                                  "rem_trf2_kind_code,rem_trf2_pri_volt_code,rem_trf2_sec_volt_code,rem_trf3_company_no,rem_trf3_tsn,rem_trf3_kva_size,rem_trf3_phs_code," +
                                  "rem_trf3_type_code,rem_trf3_mount_code,rem_trf3_kind_code,rem_trf3_pri_volt_code,rem_trf3_sec_volt_code) " +
                                  "values (?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?) " +
                                  "returning tran_no into ?";
                cmd.CommandType = CommandTypeEnum.adCmdText;

                // Input parameters
                ADODB.Parameter param = cmd.CreateParameter("TranCode", DataTypeEnum.adVarChar, ParameterDirectionEnum.adParamInput, 1, TransactionDEIS.TransactionCode);
                cmd.Parameters.Append(param);

                param = cmd.CreateParameter("TranDate", DataTypeEnum.adDate, ParameterDirectionEnum.adParamInput, 30, TransactionDEIS.TransactionDate.Replace('T', ' '));
                cmd.Parameters.Append(param);

                param = cmd.CreateParameter("BankFLN", DataTypeEnum.adVarChar, ParameterDirectionEnum.adParamInput, 30, TransactionDEIS.BankFLN);
                cmd.Parameters.Append(param);

                param = cmd.CreateParameter("WrNo", DataTypeEnum.adVarChar, ParameterDirectionEnum.adParamInput, 30, TransactionDEIS.WrNumber);
                cmd.Parameters.Append(param);

                param = cmd.CreateParameter("ServArea", DataTypeEnum.adVarChar, ParameterDirectionEnum.adParamInput, 30, TransactionDEIS.ServiceArea);
                cmd.Parameters.Append(param);

                param = cmd.CreateParameter("Yard", DataTypeEnum.adVarChar, ParameterDirectionEnum.adParamInput, 30, TransactionDEIS.Yard);
                cmd.Parameters.Append(param);

                param = cmd.CreateParameter("UserID", DataTypeEnum.adVarChar, ParameterDirectionEnum.adParamInput, 30, TransactionDEIS.UserID);
                cmd.Parameters.Append(param);

                param = cmd.CreateParameter("SecConn", DataTypeEnum.adVarChar, ParameterDirectionEnum.adParamInput, 30, TransactionDEIS.SecondaryConnection);
                cmd.Parameters.Append(param);

                param = cmd.CreateParameter("ExistingBank", DataTypeEnum.adVarChar, ParameterDirectionEnum.adParamInput, 30, TransactionDEIS.ExistingBank);
                cmd.Parameters.Append(param);

                param = cmd.CreateParameter("WiringConfigCode", DataTypeEnum.adVarChar, ParameterDirectionEnum.adParamInput, 30, TransactionDEIS.WiringConfigCode);
                cmd.Parameters.Append(param);

                param = cmd.CreateParameter("Location", DataTypeEnum.adVarChar, ParameterDirectionEnum.adParamInput, 30, TransactionDEIS.Location);
                cmd.Parameters.Append(param);

                param = cmd.CreateParameter("InsTrf1CompanyNumber", DataTypeEnum.adVarChar, ParameterDirectionEnum.adParamInput, 30, TransactionDEIS.InsTrf1CompanyNumber);
                cmd.Parameters.Append(param);

                param = cmd.CreateParameter("InsTrf1TSN", DataTypeEnum.adVarChar, ParameterDirectionEnum.adParamInput, 30, TransactionDEIS.InsTrf1TSN);
                cmd.Parameters.Append(param);

                param = cmd.CreateParameter("InsTrf1KvaSize", DataTypeEnum.adDouble, ParameterDirectionEnum.adParamInput, 10, TransactionDEIS.InsTrf1KvaSize);
                cmd.Parameters.Append(param);

                param = cmd.CreateParameter("InsTrf1PhaseCode", DataTypeEnum.adVarChar, ParameterDirectionEnum.adParamInput, 30, TransactionDEIS.InsTrf1PhaseCode);
                cmd.Parameters.Append(param);

                param = cmd.CreateParameter("InsTrf1TypeCode", DataTypeEnum.adVarChar, ParameterDirectionEnum.adParamInput, 30, TransactionDEIS.InsTrf1TypeCode);
                cmd.Parameters.Append(param);

                param = cmd.CreateParameter("InsTrf1MountCode", DataTypeEnum.adVarChar, ParameterDirectionEnum.adParamInput, 30, TransactionDEIS.InsTrf1MountCode);
                cmd.Parameters.Append(param);

                param = cmd.CreateParameter("InsTrf1KindCode", DataTypeEnum.adVarChar, ParameterDirectionEnum.adParamInput, 30, TransactionDEIS.InsTrf1KindCode);
                cmd.Parameters.Append(param);

                param = cmd.CreateParameter("InsTrf1PriVoltCode", DataTypeEnum.adVarChar, ParameterDirectionEnum.adParamInput, 30, TransactionDEIS.InsTrf1PriVoltCode);
                cmd.Parameters.Append(param);

                param = cmd.CreateParameter("InsTrf1SecVoltCode", DataTypeEnum.adVarChar, ParameterDirectionEnum.adParamInput, 30, TransactionDEIS.InsTrf1SecVoltCode);
                cmd.Parameters.Append(param);


                param = cmd.CreateParameter("InsTrf2CompanyNumber", DataTypeEnum.adVarChar, ParameterDirectionEnum.adParamInput, 30, TransactionDEIS.InsTrf2CompanyNumber);
                cmd.Parameters.Append(param);

                param = cmd.CreateParameter("InsTrf2TSN", DataTypeEnum.adVarChar, ParameterDirectionEnum.adParamInput, 30, TransactionDEIS.InsTrf2TSN);
                cmd.Parameters.Append(param);

                param = cmd.CreateParameter("InsTrf2KvaSize", DataTypeEnum.adDouble, ParameterDirectionEnum.adParamInput, 10, TransactionDEIS.InsTrf2KvaSize);
                cmd.Parameters.Append(param);

                param = cmd.CreateParameter("InsTrf2PhaseCode", DataTypeEnum.adVarChar, ParameterDirectionEnum.adParamInput, 30, TransactionDEIS.InsTrf2PhaseCode);
                cmd.Parameters.Append(param);

                param = cmd.CreateParameter("InsTrf2TypeCode", DataTypeEnum.adVarChar, ParameterDirectionEnum.adParamInput, 30, TransactionDEIS.InsTrf2TypeCode);
                cmd.Parameters.Append(param);

                param = cmd.CreateParameter("InsTrf2MountCode", DataTypeEnum.adVarChar, ParameterDirectionEnum.adParamInput, 30, TransactionDEIS.InsTrf2MountCode);
                cmd.Parameters.Append(param);

                param = cmd.CreateParameter("InsTrf2KindCode", DataTypeEnum.adVarChar, ParameterDirectionEnum.adParamInput, 30, TransactionDEIS.InsTrf2KindCode);
                cmd.Parameters.Append(param);

                param = cmd.CreateParameter("InsTrf2PriVoltCode", DataTypeEnum.adVarChar, ParameterDirectionEnum.adParamInput, 30, TransactionDEIS.InsTrf2PriVoltCode);
                cmd.Parameters.Append(param);

                param = cmd.CreateParameter("InsTrf2SecVoltCode", DataTypeEnum.adVarChar, ParameterDirectionEnum.adParamInput, 30, TransactionDEIS.InsTrf2SecVoltCode);
                cmd.Parameters.Append(param);


                param = cmd.CreateParameter("InsTrf3CompanyNumber", DataTypeEnum.adVarChar, ParameterDirectionEnum.adParamInput, 30, TransactionDEIS.InsTrf3CompanyNumber);
                cmd.Parameters.Append(param);

                param = cmd.CreateParameter("InsTrf3TSN", DataTypeEnum.adVarChar, ParameterDirectionEnum.adParamInput, 30, TransactionDEIS.InsTrf3TSN);
                cmd.Parameters.Append(param);

                param = cmd.CreateParameter("InsTrf3KvaSize", DataTypeEnum.adDouble, ParameterDirectionEnum.adParamInput, 10, TransactionDEIS.InsTrf3KvaSize);
                cmd.Parameters.Append(param);

                param = cmd.CreateParameter("InsTrf3PhaseCode", DataTypeEnum.adVarChar, ParameterDirectionEnum.adParamInput, 30, TransactionDEIS.InsTrf3PhaseCode);
                cmd.Parameters.Append(param);

                param = cmd.CreateParameter("InsTrf3TypeCode", DataTypeEnum.adVarChar, ParameterDirectionEnum.adParamInput, 30, TransactionDEIS.InsTrf3TypeCode);
                cmd.Parameters.Append(param);

                param = cmd.CreateParameter("InsTrf3MountCode", DataTypeEnum.adVarChar, ParameterDirectionEnum.adParamInput, 30, TransactionDEIS.InsTrf3MountCode);
                cmd.Parameters.Append(param);

                param = cmd.CreateParameter("InsTrf3KindCode", DataTypeEnum.adVarChar, ParameterDirectionEnum.adParamInput, 30, TransactionDEIS.InsTrf3KindCode);
                cmd.Parameters.Append(param);

                param = cmd.CreateParameter("InsTrf3PriVoltCode", DataTypeEnum.adVarChar, ParameterDirectionEnum.adParamInput, 30, TransactionDEIS.InsTrf3PriVoltCode);
                cmd.Parameters.Append(param);

                param = cmd.CreateParameter("InsTrf3SecVoltCode", DataTypeEnum.adVarChar, ParameterDirectionEnum.adParamInput, 30, TransactionDEIS.InsTrf3SecVoltCode);
                cmd.Parameters.Append(param);


                param = cmd.CreateParameter("RemTrf1CompanyNumber", DataTypeEnum.adVarChar, ParameterDirectionEnum.adParamInput, 30, TransactionDEIS.RemTrf1CompanyNumber);
                cmd.Parameters.Append(param);

                param = cmd.CreateParameter("RemTrf1TSN", DataTypeEnum.adVarChar, ParameterDirectionEnum.adParamInput, 30, TransactionDEIS.RemTrf1TSN);
                cmd.Parameters.Append(param);

                param = cmd.CreateParameter("RemTrf1KvaSize", DataTypeEnum.adDouble, ParameterDirectionEnum.adParamInput, 10, TransactionDEIS.RemTrf1KvaSize);
                cmd.Parameters.Append(param);

                param = cmd.CreateParameter("RemTrf1PhaseCode", DataTypeEnum.adVarChar, ParameterDirectionEnum.adParamInput, 30, TransactionDEIS.RemTrf1PhaseCode);
                cmd.Parameters.Append(param);

                param = cmd.CreateParameter("RemTrf1TypeCode", DataTypeEnum.adVarChar, ParameterDirectionEnum.adParamInput, 30, TransactionDEIS.RemTrf1TypeCode);
                cmd.Parameters.Append(param);

                param = cmd.CreateParameter("RemTrf1MountCode", DataTypeEnum.adVarChar, ParameterDirectionEnum.adParamInput, 30, TransactionDEIS.RemTrf1MountCode);
                cmd.Parameters.Append(param);

                param = cmd.CreateParameter("RemTrf1KindCode", DataTypeEnum.adVarChar, ParameterDirectionEnum.adParamInput, 30, TransactionDEIS.RemTrf1KindCode);
                cmd.Parameters.Append(param);

                param = cmd.CreateParameter("RemTrf1PriVoltCode", DataTypeEnum.adVarChar, ParameterDirectionEnum.adParamInput, 30, TransactionDEIS.RemTrf1PriVoltCode);
                cmd.Parameters.Append(param);

                param = cmd.CreateParameter("RemTrf1SecVoltCode", DataTypeEnum.adVarChar, ParameterDirectionEnum.adParamInput, 30, TransactionDEIS.RemTrf1SecVoltCode);
                cmd.Parameters.Append(param);


                param = cmd.CreateParameter("RemTrf2CompanyNumber", DataTypeEnum.adVarChar, ParameterDirectionEnum.adParamInput, 30, TransactionDEIS.RemTrf2CompanyNumber);
                cmd.Parameters.Append(param);

                param = cmd.CreateParameter("RemTrf2TSN", DataTypeEnum.adVarChar, ParameterDirectionEnum.adParamInput, 30, TransactionDEIS.RemTrf2TSN);
                cmd.Parameters.Append(param);

                param = cmd.CreateParameter("RemTrf2KvaSize", DataTypeEnum.adDouble, ParameterDirectionEnum.adParamInput, 10, TransactionDEIS.RemTrf2KvaSize);
                cmd.Parameters.Append(param);

                param = cmd.CreateParameter("RemTrf2PhaseCode", DataTypeEnum.adVarChar, ParameterDirectionEnum.adParamInput, 30, TransactionDEIS.RemTrf2PhaseCode);
                cmd.Parameters.Append(param);

                param = cmd.CreateParameter("RemTrf2TypeCode", DataTypeEnum.adVarChar, ParameterDirectionEnum.adParamInput, 30, TransactionDEIS.RemTrf2TypeCode);
                cmd.Parameters.Append(param);

                param = cmd.CreateParameter("RemTrf2MountCode", DataTypeEnum.adVarChar, ParameterDirectionEnum.adParamInput, 30, TransactionDEIS.RemTrf2MountCode);
                cmd.Parameters.Append(param);

                param = cmd.CreateParameter("RemTrf2KindCode", DataTypeEnum.adVarChar, ParameterDirectionEnum.adParamInput, 30, TransactionDEIS.RemTrf2KindCode);
                cmd.Parameters.Append(param);

                param = cmd.CreateParameter("RemTrf2PriVoltCode", DataTypeEnum.adVarChar, ParameterDirectionEnum.adParamInput, 30, TransactionDEIS.RemTrf2PriVoltCode);
                cmd.Parameters.Append(param);

                param = cmd.CreateParameter("RemTrf2SecVoltCode", DataTypeEnum.adVarChar, ParameterDirectionEnum.adParamInput, 30, TransactionDEIS.RemTrf2SecVoltCode);
                cmd.Parameters.Append(param);


                param = cmd.CreateParameter("RemTrf3CompanyNumber", DataTypeEnum.adVarChar, ParameterDirectionEnum.adParamInput, 30, TransactionDEIS.RemTrf3CompanyNumber);
                cmd.Parameters.Append(param);

                param = cmd.CreateParameter("RemTrf3TSN", DataTypeEnum.adVarChar, ParameterDirectionEnum.adParamInput, 30, TransactionDEIS.RemTrf3TSN);
                cmd.Parameters.Append(param);

                param = cmd.CreateParameter("RemTrf3KvaSize", DataTypeEnum.adDouble, ParameterDirectionEnum.adParamInput, 10, TransactionDEIS.RemTrf3KvaSize);
                cmd.Parameters.Append(param);

                param = cmd.CreateParameter("RemTrf3PhaseCode", DataTypeEnum.adVarChar, ParameterDirectionEnum.adParamInput, 30, TransactionDEIS.RemTrf3PhaseCode);
                cmd.Parameters.Append(param);

                param = cmd.CreateParameter("RemTrf3TypeCode", DataTypeEnum.adVarChar, ParameterDirectionEnum.adParamInput, 30, TransactionDEIS.RemTrf3TypeCode);
                cmd.Parameters.Append(param);

                param = cmd.CreateParameter("RemTrf3MountCode", DataTypeEnum.adVarChar, ParameterDirectionEnum.adParamInput, 30, TransactionDEIS.RemTrf3MountCode);
                cmd.Parameters.Append(param);

                param = cmd.CreateParameter("RemTrf3KindCode", DataTypeEnum.adVarChar, ParameterDirectionEnum.adParamInput, 30, TransactionDEIS.RemTrf3KindCode);
                cmd.Parameters.Append(param);

                param = cmd.CreateParameter("RemTrf3PriVoltCode", DataTypeEnum.adVarChar, ParameterDirectionEnum.adParamInput, 30, TransactionDEIS.RemTrf3PriVoltCode);
                cmd.Parameters.Append(param);

                param = cmd.CreateParameter("RemTrf3SecVoltCode", DataTypeEnum.adVarChar, ParameterDirectionEnum.adParamInput, 30, TransactionDEIS.RemTrf3SecVoltCode);
                cmd.Parameters.Append(param);

                // Output parameter
                param = cmd.CreateParameter("TranNo", DataTypeEnum.adBigInt, ParameterDirectionEnum.adParamOutput, 10);
                cmd.Parameters.Append(param);

                int recordsAffected = 0;

                Recordset spRS = m_Application.DataContext.ExecuteCommand(cmd, out recordsAffected);

                m_Application.DataContext.Execute("commit", out recordsAffected, (int)CommandTypeEnum.adCmdText);

                if (!Convert.IsDBNull(cmd.Parameters["TranNo"].Value))
                {
                    TransactionDEIS.TransactionNumber = Convert.ToInt32(cmd.Parameters["TranNo"].Value);
                }

                returnValue = true;
            }
            catch (Exception ex)
            {
                TransactionDEIS.TransactionMessage = "FAILED - Error inserting record into " + TransactionDEIS.TABLE_DEIS_TRANSACTION + ": " + ex.Message;
                returnValue = false;
            }

            return returnValue;
        }

        /// <summary>
        /// Calls methods to validate and process the transaction.
        /// </summary>
        /// <returns>Boolean indicating status</returns>
        private bool ProcessTransaction()
        {
            bool returnValue = false;

            try
            {
                // Validate the transaction
                ValidateDEIS validateDEIS = new ValidateDEIS(m_Application.DataContext);
                if (!validateDEIS.ValidateTransaction())
                {
                    // Merge status and message, so status can be set to 'COMPLETE'. This is needed for WARNINGs.
                    TransactionDEIS.TransactionMessage = TransactionDEIS.TransactionStatus + " - " + TransactionDEIS.TransactionMessage;
                    TransactionDEIS.TransactionStatus = TransactionDEIS.TRANS_STATUS_COMPLETE;
                    return false;
                }

                // If validation passes then call processing method
                m_TransactionManager.Begin("Process DEIS Transaction");
                ProcessDEIS processDEIS = new ProcessDEIS(m_Application.DataContext);

                if (!processDEIS.Process())
                {
                    // Merge status and message, so status can be set to 'COMPLETE'. This is needed for WARNINGs.
                    TransactionDEIS.TransactionMessage = TransactionDEIS.TransactionStatus + " - " + TransactionDEIS.TransactionMessage;
                    TransactionDEIS.TransactionStatus = TransactionDEIS.TRANS_STATUS_COMPLETE;
                    m_TransactionManager.Rollback();
                    return false;
                }

                m_TransactionManager.Commit();

                // If success or warning then post job edits.
                string message = string.Empty;
                if (!PostJobEdits(out message))
                {
                    // Merge status and message, so status can be set to 'COMPLETE'. This is needed for WARNINGs.
                    TransactionDEIS.TransactionMessage = TransactionDEIS.TransactionStatus + " - " + TransactionDEIS.TransactionMessage;
                    TransactionDEIS.TransactionStatus = TransactionDEIS.TRANS_STATUS_COMPLETE;
                    return false;
                }

                if (TransactionDEIS.TransactionMessage.Length > 0)
                {
                    // Merge status and message, so status can be set to 'COMPLETE'. This is needed for WARNINGs.
                    TransactionDEIS.TransactionMessage = TransactionDEIS.TransactionStatus + " - " + TransactionDEIS.TransactionMessage;
                }
                else
                {
                    TransactionDEIS.TransactionMessage = TransactionDEIS.TRANS_STATUS_SUCCESS;
                }

                TransactionDEIS.TransactionStatus = TransactionDEIS.TRANS_STATUS_COMPLETE;                
            }
            catch(Exception ex)
            {
                if (m_TransactionManager.TransactionInProgress)
                {
                    m_TransactionManager.Rollback();
                }
                TransactionDEIS.TransactionStatus = TransactionDEIS.TRANS_STATUS_FAILED;
                TransactionDEIS.TransactionMessage = "Error processing transaction: " + ex.Message;
                returnValue = false;
            }

            return returnValue;
        }

        /// <summary>
        /// Updates the status and message in the DEIS transaction table.
        /// </summary>
        /// <returns>Boolean indicating status</returns>
        internal bool UpdateTransactionTable()
        {
            bool returnValue = false;

            try
            {
                string sqlStatement = string.Format("Update {0} set {1} = ?, {2} = ? where {3} = ?",
                                                    TransactionDEIS.TABLE_DEIS_TRANSACTION, TransactionDEIS.FIELD_DEIS_TRANSACTION_STATUS,
                                                    TransactionDEIS.FIELD_DEIS_TRANSACTION_MSG, TransactionDEIS.FIELD_DEIS_TRANSACTION_NUMBER);
                int recordsAffected = 0;
                m_Application.DataContext.Execute(sqlStatement, out recordsAffected, (int)CommandTypeEnum.adCmdText + (int)ExecuteOptionEnum.adExecuteNoRecords,
                                      TransactionDEIS.TransactionStatus, TransactionDEIS.TransactionMessage, TransactionDEIS.TransactionNumber);
                m_Application.DataContext.Execute("commit", out recordsAffected, (int)CommandTypeEnum.adCmdText);

                returnValue = true;
            }
            catch (Exception ex)
            {
                TransactionDEIS.TransactionStatus = TransactionDEIS.TRANS_STATUS_FAILED;
                TransactionDEIS.TransactionMessage = string.Format("Error updating {0} table: {1}", TransactionDEIS.TABLE_DEIS_TRANSACTION, ex.Message);
                returnValue = false;
            }

            return returnValue;
        }

        /// <summary>
        /// Posts the job edits.
        /// </summary>
        /// <returns>Boolean indicating status</returns>
        internal bool PostJobEdits(out string message)
        {
            bool returnValue = false;
            message = "";

            try
            {
                IGTJobManagementService jobManagement = GTClassFactory.Create<IGTJobManagementService>();
                jobManagement.DataContext = m_Application.DataContext;
                jobManagement.EditJob(m_Application.DataContext.ActiveJob);

                GTPostErrorConstants postError = jobManagement.PostJob();
                if (postError != GTPostErrorConstants.gtjmsNoError)
                {
                    if (jobManagement.PostErrors != null)
                    {
                        Recordset errorRecords = jobManagement.PostErrors;
                        
                        if (postError == GTPostErrorConstants.gtjmsValidation)
                        {
                            jobManagement.PostErrors.Filter = "ErrorPriority = 'P1' OR ErrorPriority = 'P2'";
                        } 

                        if (errorRecords.RecordCount > 0)
                        {
                            errorRecords.MoveFirst();
                            while (!errorRecords.EOF)
                            {
                                message += errorRecords.Fields["G3E_FID"].Value;
                                if (postError == GTPostErrorConstants.gtjmsValidation)
                                {
                                    message += "-" + errorRecords.Fields["ErrorDescription"].Value + Environment.NewLine;
                                }
                                else
                                {
                                    message += "-" + errorRecords.Fields["table_name"].Value + "-" + errorRecords.Fields["job_name"].Value + Environment.NewLine;
                                }
                                errorRecords.MoveNext();
                            }
                        }
                    }                    

                    switch (postError)
                    {
                        case GTPostErrorConstants.gtjmsValidation:
                            message = "Post error - validation errors detected: " + Environment.NewLine + message;
                            break;
                        case GTPostErrorConstants.gtjmsConflict:
                            message = "Post error - conflicts detected: " + message;
                            break;
                        case GTPostErrorConstants.gtjmsDatabase:
                            message = "Post error - database error detected: " + message;
                            break;
                        case GTPostErrorConstants.gtjmsNoPendingEdits:
                            message = "Post error - no pending edits ";
                            break;
                    }

                    if (message.Length > 0)
                    {
                        TransactionDEIS.TransactionStatus = TransactionDEIS.TRANS_STATUS_FAILED;
                        TransactionDEIS.TransactionMessage = message;
                        jobManagement.DiscardJob();
                        returnValue = false;
                    }
                    else
                    {
                        returnValue = true;
                    }
                }
                else
                {
                    returnValue = true;
                }

                jobManagement.RefreshJobEnvironment();
                
            }
            catch (Exception ex)
            {
                TransactionDEIS.TransactionStatus = TransactionDEIS.TRANS_STATUS_FAILED;
                TransactionDEIS.TransactionMessage = string.Format("Error posting transaction: {0}", ex.Message);
                returnValue = false;
            }

            return returnValue;
        }
    }

    public class DEISException : Exception
    {                
        public DEISException(int Severity, string message) :base(message)
        {
           HResult = Severity;
        }
    }
}
