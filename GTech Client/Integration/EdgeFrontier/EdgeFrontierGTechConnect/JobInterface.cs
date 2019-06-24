using System;
using System.Data;
using System.Data.OleDb;
using System.IO;

namespace EdgeFrontierConnect
{
    public class JobInterface
    {
        public int CreateUpdateJob(
            string connectionString,
            decimal p_WR_Number,
            DateTime p_WR_Creation_Date,
            string p_WR_Name,
            string p_WR_Type,
            string p_Designer_Assignment,
            string p_WR_Status,
            DateTime p_Customer_Required_Date,
            DateTime p_Construction_Ready_Date,
            string p_Street_Number,
            string p_Street_Name,
            decimal p_Town,
            decimal p_County,
            string p_Crew_Headquarters,
            string p_Management_Activity_Code)
        {
            int status = -1;

            // Create and open the connection in a using block. This
            // ensures that all resources will be closed and disposed
            // when the code exits.
            try
            {
                using (OleDbConnection connection = new OleDbConnection(connectionString))
                {
                    OleDbCommand command = new OleDbCommand
                       ("GISPKG_WMIS_WR.CreateUpdateJob", connection);

                    command.CommandType = CommandType.StoredProcedure;

                    var result = command.Parameters.Add(new OleDbParameter("RetVal", OleDbType.Integer) { Direction = ParameterDirection.ReturnValue });
                    command.Parameters.Add(new OleDbParameter("@p_WR_Number", OleDbType.Decimal) { Value = p_WR_Number, Direction = ParameterDirection.Input });
                    command.Parameters.Add(new OleDbParameter("@p_WR_Creation_Date", OleDbType.DBDate) { Value = p_WR_Creation_Date, Direction = ParameterDirection.Input });
                    command.Parameters.Add(new OleDbParameter("@p_WR_Name", OleDbType.VarChar) { Value = p_WR_Name, Direction = ParameterDirection.Input });
                    command.Parameters.Add(new OleDbParameter("@p_WR_Type", OleDbType.VarChar) { Value = p_WR_Type, Direction = ParameterDirection.Input });
                    command.Parameters.Add(new OleDbParameter("@p_Designer_Assignment", OleDbType.VarChar) { Value = p_Designer_Assignment, Direction = ParameterDirection.Input });
                    command.Parameters.Add(new OleDbParameter("@p_WR_Status", OleDbType.VarChar) { Value = p_WR_Status, Direction = ParameterDirection.Input });
                    command.Parameters.Add(new OleDbParameter("@p_Customer_Required_Date", OleDbType.DBDate) { Value = p_Customer_Required_Date, Direction = ParameterDirection.Input });
                    command.Parameters.Add(new OleDbParameter("@p_Construction_Ready_Date", OleDbType.DBDate) { Value = p_Construction_Ready_Date, Direction = ParameterDirection.Input });
                    command.Parameters.Add(new OleDbParameter("@p_Street_Number", OleDbType.VarChar) { Value = p_Street_Number, Direction = ParameterDirection.Input });
                    command.Parameters.Add(new OleDbParameter("@p_Street_Name", OleDbType.VarChar) { Value = p_Street_Name, Direction = ParameterDirection.Input });
                    command.Parameters.Add(new OleDbParameter("@p_Town", OleDbType.Decimal) { Value = p_Town, Direction = ParameterDirection.Input });
                    command.Parameters.Add(new OleDbParameter("@p_County", OleDbType.Decimal) { Value = p_County, Direction = ParameterDirection.Input });
                    command.Parameters.Add(new OleDbParameter("@p_Crew_Headquarters", OleDbType.VarChar) { Value = p_Crew_Headquarters, Direction = ParameterDirection.Input });
                    command.Parameters.Add(new OleDbParameter("@p_Management_Activity_Code", OleDbType.VarChar) { Value = p_Management_Activity_Code, Direction = ParameterDirection.Input });

                    connection.Open();
                    int rowsAffected = command.ExecuteNonQuery();
                    status = (int)result.Value;
                }
            }
            catch (Exception ex)
            {
                File.AppendAllText("C:/EF_Logger/log.txt",
                    //Path.Combine("C:/EF_Logger", interface_name, "_", subinterface_name, "log.txt"), 
                    DateTime.Now.ToString() + Environment.NewLine + ex.ToString());
            }

            return status;
        }

        public int UpdateWritebackStatus(
            string connectionString,
            decimal p_WR_Number,
            string p_WritebackStatus)
        {
            int status = -1;

            // Create and open the connection in a using block. This
            // ensures that all resources will be closed and disposed
            // when the code exits.
            try
            {
                using (OleDbConnection connection = new OleDbConnection(connectionString))
                {
                    OleDbCommand command = new OleDbCommand
                       ("GISPKG_WMIS_WR.CreateUpdateJob", connection);

                    command.CommandType = CommandType.StoredProcedure;

                    var result = command.Parameters.Add(new OleDbParameter("RetVal", OleDbType.Integer) { Direction = ParameterDirection.ReturnValue });
                    command.Parameters.Add(new OleDbParameter("@p_WR_Number", OleDbType.Decimal) { Value = p_WR_Number, Direction = ParameterDirection.Input });
                    command.Parameters.Add(new OleDbParameter("@p_WritebackStatus", OleDbType.DBDate) { Value = p_WritebackStatus, Direction = ParameterDirection.Input });

                    connection.Open();
                    int rowsAffected = command.ExecuteNonQuery();
                    status = (int)result.Value;
                }
            }
            catch (Exception ex)
            {
                File.AppendAllText("C:/EF_Logger/log.txt",
                    //Path.Combine("C:/EF_Logger", interface_name, "_", subinterface_name, "log.txt"), 
                    DateTime.Now.ToString() + Environment.NewLine + ex.ToString());
            }

            return status;
        }
    }
}
