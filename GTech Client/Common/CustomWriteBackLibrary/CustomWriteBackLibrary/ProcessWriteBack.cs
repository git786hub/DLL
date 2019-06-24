//----------------------------------------------------------------------------+
//        Class: ProcessWriteBack
//  Description: This class holds necessary methods to make request to process Update Writeback.
//                                                                  
//----------------------------------------------------------------------------+
//          $Author:: Shubham Agarwal                                       $
//          $Date:: 25/03/18                                                $
//          $Revision:: 1                                                   $
//----------------------------------------------------------------------------+
//    $History:: ProcessWriteBack.cs                     $
// 
// *****************  Version 1  *****************
// User: sagarwal     Date: 25/03/18   Time: 18:00  Desc : Created
//----------------------------------------------------------------------------+

using GTechnology.Oncor.CustomAPI;
using Intergraph.GTechnology.API;
using Oracle.DataAccess.Client;
using System;
using System.Xml.Linq;


namespace CustomWriteBackLibrary
{
  public enum WebServiceResponse { Error, Success, Timeout };
  class ProcessWriteBack
  {
    private IGTApplication m_oApp;
    public ProcessWriteBack(IGTApplication oApp)
    {
      m_oApp = oApp;
    }

    /// <summary>
    /// Method to carry out processing of Writeback
    /// </summary>
    /// <param name="p_sJobIdentifier"> G3E_JOB.G3E_IDENTIFIER</param>
    /// <param name="p_passWord">Password string in the form Password='TEST';</param>
    /// <param name="p_sStatus">G3E_JOB.JOB_STATUS to be set for the job</param>
    /// <param name="p_errorInfo">Out parameter for the error info</param>

    public void ProcessWriteBackStatusUpdate(string p_sJobIdentifier, string p_passWord, bool p_AlternateJob, out WebServiceResponse p_sStatus, out string p_errorInfo)
    {
      string sRequestURL = string.Empty;
      string sRequestXML = string.Empty;
      string sResponse = string.Empty;
      string sWRNumber = string.Empty;
      int iPollingInterval = 0;
      string sDatabaseName = string.Empty;

      try
      {
        p_sStatus = WebServiceResponse.Success;
        p_errorInfo = string.Empty;

        DataAccess oDataAccess = new DataAccess(m_oApp);

        sRequestURL = oDataAccess.GetEFUrl("WMIS_WriteBack", "EdgeFrontier");
        iPollingInterval = Convert.ToInt32(oDataAccess.GetEFUrl("WMIS_WritebackPollingInterval", "WMIS"));
        sDatabaseName = Convert.ToString(oDataAccess.GetFirstFieldValueFromRecordset("select sys_context('userenv','instance_name') from dual"));

        sWRNumber = oDataAccess.GetFirstFieldValueFromRecordset(string.Format("select WR_NBR from g3e_job where g3e_identifier ='{0}'", p_sJobIdentifier));

        string sTransactionID = string.Empty;
        sTransactionID = oDataAccess.GetFirstFieldValueFromRecordset("select CORRELATION_ID_SEQ.nextval from dual");

        sRequestXML = GetRequestXML(sWRNumber, sTransactionID, p_sJobIdentifier, p_AlternateJob);

        sResponse = ProcessRequest(sRequestURL, sRequestXML, p_errorInfo);

        if(sResponse.ToLower().Equals("success")) //Move forward
        {
          OracleConnection oConnection = new Oracle.DataAccess.Client.OracleConnection();

          string constr = string.Empty;

          // Need to add a proper call that uses parameters to get a SYS_GENERALPARAMETER value to the DataAccess object...
          string sql = string.Format("select param_value from sys_generalparameter where subsystem_name='{0}' and subsystem_component='{1}' and param_name='{2}'", "WMIS", "WMIS_WRITEBACK", "ConnectionString");
          ADODB.Recordset rs = oDataAccess.GetRecordSet(sql);
          if(null != rs && 0 < rs.RecordCount)
          {
            if(DBNull.Value != rs.Fields[0].Value)
            {
              constr = rs.Fields[0].Value.ToString();
            }
            rs.Close();
            rs = null;
          }

          if(string.IsNullOrEmpty(constr))
          {
            // Issue an exception here?
            return;
          }

          //oConnection.ConnectionString = "User Id=" + m_oApp.DataContext.DatabaseUserName + ";" + p_passWord + "Data Source=" + sDatabaseName;
          oConnection.ConnectionString = constr;
          oConnection.Open();

          OracleCommand oraCommand = new OracleCommand();
          oraCommand.CommandText = "WMIS_PollJobTable";
          oraCommand.CommandType = System.Data.CommandType.StoredProcedure;
          oraCommand.Connection = oConnection;

          OracleParameter jobIdentifier = oraCommand.CreateParameter();
          jobIdentifier.Direction = System.Data.ParameterDirection.Input;
          jobIdentifier.DbType = System.Data.DbType.String;
          jobIdentifier.OracleDbType = OracleDbType.Varchar2;
          jobIdentifier.Size = 30;
          jobIdentifier.Value = p_sJobIdentifier;
          jobIdentifier.ParameterName = "p_jobIdentifier";

          oraCommand.Parameters.Add(jobIdentifier);

          OracleParameter pollingInterval = oraCommand.CreateParameter();
          pollingInterval.Direction = System.Data.ParameterDirection.Input;
          pollingInterval.DbType = System.Data.DbType.String;
          pollingInterval.OracleDbType = OracleDbType.NVarchar2;
          pollingInterval.Size = 30;
          pollingInterval.Value = iPollingInterval;
          pollingInterval.ParameterName = "p_pollingInterval";

          oraCommand.Parameters.Add(pollingInterval);

          OracleParameter outStatus = oraCommand.CreateParameter();
          outStatus.Direction = System.Data.ParameterDirection.Output;
          outStatus.DbType = System.Data.DbType.String;
          outStatus.OracleDbType = OracleDbType.NVarchar2;
          outStatus.Size = 30;
          outStatus.ParameterName = "p_status";
          oraCommand.Parameters.Add(outStatus);

          OracleParameter outWritebackSet = oraCommand.CreateParameter();
          outWritebackSet.Direction = System.Data.ParameterDirection.Output;
          outWritebackSet.DbType = System.Data.DbType.String;
          outWritebackSet.OracleDbType = OracleDbType.NVarchar2;
          outWritebackSet.Size = 30;
          outWritebackSet.ParameterName = "p_writeBackSet";
          oraCommand.Parameters.Add(outWritebackSet);

          oraCommand.ExecuteNonQuery();

          string statusMessage = Convert.ToString(oraCommand.Parameters["p_status"].Value);
          string writeBackSet = Convert.ToString(oraCommand.Parameters["p_writeBackSet"].Value);

          if("1" == writeBackSet)
          {
            // WMIS_STATUS_C was changed to WRITEBACK during the loop.
            // Check the status and report accordingly

            switch(statusMessage)
            {
              case "SUCCESS":
                p_errorInfo = "";
                p_sStatus = WebServiceResponse.Success;
                break;

              case "WRITEBACK":
                p_errorInfo = String.Format("Write back operation for WR {0} timed out after {1}.  Please check back later to see if it completed successfully.", p_sJobIdentifier, timeString(iPollingInterval));
                p_sStatus = WebServiceResponse.Timeout;
                break;

              case "FAILURE":
                p_errorInfo = String.Format("A Failure has occurred while setting the WMIS Status.  Examine the job properties for more information.");
                p_sStatus = WebServiceResponse.Error;
                break;

              default:
                p_errorInfo = String.Format("An unexpected status value of '{0}' was returned while waiting on the WMIS Status to be set.  Examine the job properties for more information.", statusMessage);
                p_sStatus = WebServiceResponse.Error;
                break;
            }

          }
          else
          {
            // WMIS_STATUS_C was never set to WRITEBACK during the loop.
            // Report this a a failure
            p_errorInfo = String.Format("WMIS Status was never set to WRITEBACK for this WR: {0}.  Examine the job properties for more information.", p_sJobIdentifier);
            p_sStatus = WebServiceResponse.Error;
          }

        }
        else //Error has occured while submitting the request, return to caller
        {
          // If the request returns additional information in p_errorInfo, then use that; else, something else happened...
          if(string.IsNullOrEmpty(p_errorInfo))
          {
            p_errorInfo = "Writeback did not return a SUCCESS status for this job.  Examine the job properties for more information.";
          }

          p_sStatus = WebServiceResponse.Error;
        }
      }
      catch(Exception EX)
      {
        p_errorInfo = EX.Message;
        p_sStatus = WebServiceResponse.Error;
      }
    }

    /// <summary>
    /// Processes the XML Request
    /// </summary>
    /// <param name="p_sRequestURL">URL for Request XML</param>
    /// <param name="p_sRequestXML">Request XML</param>
    /// <param name="p_ErrorInfo">Contains additional error information.</param>
    /// <returns></returns>
    private string ProcessRequest(string p_sRequestURL, string p_sRequestXML, string p_ErrorInfo)
    {
      SendReceiveXMLMessage oSendRecieve = new SendReceiveXMLMessage();
      oSendRecieve.RequestXMLBody = p_sRequestXML;
      oSendRecieve.URL = p_sRequestURL;
      oSendRecieve.Method = "POST";
      oSendRecieve.ContentType = "application/xml";
      oSendRecieve.SendMsgToEF();
      string sResponse = oSendRecieve.ResponseXML;
      string sStatusResponse = string.Empty;
      string status = string.Empty;

      try
      {
        if(!string.IsNullOrEmpty(sResponse))
        {
          XDocument document = null;
          bool ResponseIsXML = true;

          try
          {
            document = XDocument.Parse(sResponse);
          }
          catch(Exception)
          {
            ResponseIsXML = false;
            status = "FAILURE";
            p_ErrorInfo = string.Format("Received the following response message from the WriteBack WEB Request: {0}{1}{1}This is not a valid XML message.", sResponse, System.Environment.NewLine);
          }

          if(ResponseIsXML)
          {
            XNamespace s1 = "http://schemas.xmlsoap.org/soap/envelope/";
            XNamespace x = "http://www.informatica.com/wsdl/";

            status = document.Element(s1 + "Envelope")
                             .Element(s1 + "Body")
                             .Element(x + "WMIS_WSH_GIS_WRITEBACKResponse")
                             .Element(x + "WMIS_WSH_GIS_WRITEBACKResponseElement")
                             .Element(x + "Status").Value;
          }
        }
      }

      catch(Exception ex)
      {
        throw ex;
      }

      return status;
    }

    private string GetRequestXML(string p_sWRNumber, string p_sTransactionID, string p_sJobIdentifier, bool p_bAlternateJob)
    {
      string sRequestXML = string.Empty; //Normally wouldn't declare this quite this way but it makes the assignments below formatted better...
      sRequestXML += @"<soap:Envelope xmlns:soap=""http://schemas.xmlsoap.org/soap/envelope/"" xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" xmlns:xsd=""http://www.w3.org/2001/XMLSchema"">" + Environment.NewLine;
      sRequestXML += @"  <soap:Header></soap:Header>" + Environment.NewLine;
      sRequestXML += @"  <soap:Body xmlns:ns0=""http://www.informatica.com/wsdl/"">" + Environment.NewLine;
      sRequestXML += @"    <ns0:WMIS_WSH_GIS_WRITEBACKRequest>" + Environment.NewLine;
      sRequestXML += @"      <ns0:WMIS_WSH_GIS_WRITEBACKRequestElement>" + Environment.NewLine;
      sRequestXML += @"        <ns0:TransactionID>" + p_sTransactionID + @"</ns0:TransactionID>" + Environment.NewLine;
      sRequestXML += @"        <ns0:WorkRequest>" + p_sWRNumber + @"</ns0:WorkRequest>" + Environment.NewLine;

      if(p_bAlternateJob)
      {
        sRequestXML += @"        <ns0:G3E_IDENTIFIER> " + p_sJobIdentifier + @" </ns0:G3E_IDENTIFIER>" + Environment.NewLine;
      }

      sRequestXML += @"      </ns0:WMIS_WSH_GIS_WRITEBACKRequestElement>" + Environment.NewLine;
      sRequestXML += @"    </ns0:WMIS_WSH_GIS_WRITEBACKRequest>" + Environment.NewLine;
      sRequestXML += @"  </soap:Body>" + Environment.NewLine;
      sRequestXML += @"</soap:Envelope>" + Environment.NewLine;

      return sRequestXML;
    }

    /// <summary>
    /// Forms a cleanly-formatted string of minutes and seconds based on the interval parameter
    /// </summary>
    /// <param name="interval">Number of seconds in the interval</param>
    /// <returns>String containing minutes and seconds</returns>
    private string timeString(int interval)
    {
      int quotient = Math.DivRem(interval, 60, out int remainder);
      string minutes = string.Empty;
      string seconds = string.Empty;
      string timestring = string.Empty;

      if(0 < quotient)
      {
        minutes = string.Format("{0} minute{1}", quotient.ToString(), 1 != quotient ? "s" : "");
      }

      if(0 < remainder)
      {
        seconds = string.Format("{0} second{1}", remainder.ToString(), 1 != remainder ? "s" : "");
      }

      if(0 < quotient && 0 < remainder)
      {
        timestring = string.Format("{0} and {1}", minutes, seconds);
      }

      if(0 < quotient && 0 == remainder)
      {
        timestring = minutes;
      }

      if(0 == quotient && 0 < remainder)
      {
        timestring = seconds;
      }

      return timestring;
    }

  }
}
