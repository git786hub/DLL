using System;
using System.Data.OleDb;
using System.IO;

namespace EdgeFrontierConnect
{
    public class Logger
    {
        /// <summary>
        /// Add entry to INTERFACE_LOG, and xml_data exists, then add an entry
        /// with a foreign key to INTERFACE_XML_DATA 
        /// </summary>
        /// <param name="connectionString"></param>
        /// <param name="interface_name"></param>
        /// <param name="subinterface_name"></param>
        /// <param name="component_name"></param>
        /// <param name="result_code"></param>
        /// <param name="result_status"></param>
        /// <param name="log_detail"></param>
        /// <param name="correlation_id"></param>
        /// <param name="process_run_id"></param>
        /// <param name="data_table"></param>
        /// <param name="data_row_id"></param>
        /// <param name="xml_data"></param>
        /// <returns></returns>
        public bool Log_To_INTERFACE_LOG_AND_INTERFACE_XML_DATA(
            string connectionString,
            string interface_name, //1
            string subinterface_name, //2
            string component_name, //3
            string result_code, //4
            string result_status, //5
            object log_detail, //6
            string correlation_id, //7
            string process_run_id, //8
            string data_table, //9
            decimal data_row_id, //10
            string xml_data) //11
        {
            bool status = true;
            try
            {
                status &= GetNextVal_SEQ_INTERFACE_LOG(connectionString, out decimal interface_log_id);

                status &= Log_To_INTERFACE_LOG(
                    connectionString, 
                    interface_log_id, 
                    interface_name, 
                    subinterface_name, 
                    component_name,
                    result_code, 
                    result_status, 
                    log_detail, 
                    correlation_id, 
                    process_run_id, 
                    data_table, 
                    data_row_id);

                if (String.IsNullOrEmpty(xml_data) == false)
                {
                    status &= Log_To_INTERFACE_XML_DATA(connectionString, interface_log_id, xml_data);
                }
            }
            catch (Exception ex)
            {
                status = false;
                File.AppendAllText("C:/EF_ManagedCode/efgt_log.txt",
                    //Path.Combine("C:/EF_Logger", interface_name, "_", subinterface_name, "log.txt"), 
                    DateTime.Now.ToString() + Environment.NewLine + ex.ToString());
            }

            return status;
        }

        /// <summary>
        /// Add an entry to the INTERFACE_LOG table
        /// </summary>
        /// <param name="connectionString"></param>
        /// <param name="interface_log_id"></param>
        /// <param name="interface_name"></param>
        /// <param name="subinterface_name"></param>
        /// <param name="component_name"></param>
        /// <param name="result_code"></param>
        /// <param name="result_status"></param>
        /// <param name="log_detail"></param>
        /// <param name="correlation_id"></param>
        /// <param name="process_run_id"></param>
        /// <param name="data_table"></param>
        /// <param name="data_row_id"></param>
        /// <returns></returns>
        private bool Log_To_INTERFACE_LOG(
            string connectionString, 
            decimal interface_log_id, //1
            string interface_name, //2
            string subinterface_name, //3
            string component_name, //4
            string result_code, //5
            string result_status, //6
            object log_detail, //7
            string correlation_id, //8
            string process_run_id, //9
            string data_table, //10
            decimal data_row_id) //11
        {
            bool status = false;

            // Create and open the connection in a using block. This
            // ensures that all resources will be closed and disposed
            // when the code exits.
            string insertTemplate =
                    "INSERT INTO GIS_STG.INTERFACE_LOG (" +
                        "interface_log_id," + //1
                        "interface_name," + //2
                        "sub_interface_name," + //3
                        "component_name," + //4
                        "result_code," + //5
                        "result_status," + //6
                        "log_detail," + //7
                        "correlation_id," + //8
                        "process_run_id," + //9
                        "data_table," + //10
                        "data_row_id) " + //11
                    "VALUES (?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?)"; //11-count

            using (OleDbConnection connection = new OleDbConnection(connectionString))
            {
                // Create the Command and Parameter objects.
                OleDbCommand insertCommand = new OleDbCommand(insertTemplate, connection);
                insertCommand.Parameters.Add("@interface_log_id", OleDbType.Decimal).Value = interface_log_id; //1
                insertCommand.Parameters.Add("@interface_name", OleDbType.VarChar, 60).Value = interface_name; //2
                insertCommand.Parameters.Add("@sub_interface_name", OleDbType.VarChar, 250).Value = subinterface_name; //3
                insertCommand.Parameters.Add("@component_name", OleDbType.VarChar, 60).Value = component_name; //4
                insertCommand.Parameters.Add("@result_code", OleDbType.VarChar, 10).Value = result_code; //5
                insertCommand.Parameters.Add("@result_status", OleDbType.VarChar, 30).Value = result_status; //6
                insertCommand.Parameters.Add("@log_detail", OleDbType.VarChar).Value = log_detail; //7
                insertCommand.Parameters.Add("@correlation_id", OleDbType.VarChar, 100).Value = correlation_id; //8
                insertCommand.Parameters.Add("@process_run_id", OleDbType.VarChar, 100).Value = process_run_id; //9
                insertCommand.Parameters.Add("@data_table", OleDbType.VarChar, 40).Value = data_table; //10
                insertCommand.Parameters.Add("@data_row_id", OleDbType.Decimal).Value = data_row_id; //11

                // Open the connection in a try/catch block. 
                // Create and execute the DataReader, writing the result
                // set to the console window.
                connection.Open();
                int rowsAffected = insertCommand.ExecuteNonQuery();
                status = true;
            }

            return status;
        }

        /// <summary>
        /// Gets the next sequence value for INTERFACE_LOG_SEQ
        /// </summary>
        /// <param name="connectionString"></param>
        /// <param name="interface_log_id"></param>
        /// <returns></returns>
        private bool GetNextVal_SEQ_INTERFACE_LOG(string connectionString, out decimal interface_log_id)
        {
            bool status = false;

            string queryString = "SELECT GIS_STG.INTERFACE_LOG_SEQ.NEXTVAL from DUAL";

            // Create and open the connection in a using block. This
            // ensures that all resources will be closed and disposed
            // when the code exits.
            using (OleDbConnection connection = new OleDbConnection(connectionString))
            {
                // Create the Command and Parameter objects.
                OleDbCommand command = new OleDbCommand(queryString, connection);

                // Open the connection in a try/catch block. 
                // Create and execute the DataReader, writing the result
                // set to the console window.
                connection.Open();
                object temp = command.ExecuteScalar();
                interface_log_id = Decimal.Parse(temp.ToString());
                status = true;
            }

            return status;
        }

        /// <summary>
        /// Insert an entry with a foreign key to INTERFACE_XML_DATA
        /// </summary>
        /// <param name="connectionString"></param>
        /// <param name="interface_log_id"></param>
        /// <param name="xml_data"></param>
        /// <returns></returns>
        private bool Log_To_INTERFACE_XML_DATA(
           string connectionString,
           decimal interface_log_id, //1
           string xml_data) //2
        {
            bool status = false;

            string queryString =
                "INSERT INTO GIS_STG.INTERFACE_XML_DATA (" +
                    "interface_log_id," + //1
                    "xml_data)" + //2
                "VALUES (?, ?)"; //2-count

            // Create and open the connection in a using block. This
            // ensures that all resources will be closed and disposed
            // when the code exits.
            using (OleDbConnection connection = new OleDbConnection(connectionString))
            {
                // Create the Command and Parameter objects.
                OleDbCommand command = new OleDbCommand(queryString, connection);
                command.Parameters.Add("@interface_log_id", OleDbType.Decimal).Value = interface_log_id; //1
                command.Parameters.Add("@xml_data", OleDbType.VarChar).Value = xml_data; //2

                // Open the connection in a try/catch block. 
                // Create and execute the DataReader, writing the result
                // set to the console window.
                connection.Open();
                int rowsAffected = command.ExecuteNonQuery();
                status = true;
            }

            return status;
        }
    }
}
