//----------------------------------------------------------------------------+
//        Class: SharedWriteBackLibrary
//  Description: This class holds methods to process Update Job Status.
//                                                                  
//----------------------------------------------------------------------------+
//          $Author:: Shubham Agarwal                                       $
//          $Date:: 25/03/18                                                $
//          $Revision:: 1                                                   $
//----------------------------------------------------------------------------+
//    $History:: ProcessJobStatusUpdate.cs                     $
// 
// *****************  Version 1  *****************
// User: sagarwal     Date: 25/03/18   Time: 18:00  Desc : Created
//----------------------------------------------------------------------------+

using CustomWriteBackLibrary.CustomWriteBackLibrary;
using GTechnology.Oncor.CustomAPI;
using Intergraph.GTechnology.API;
using System;
using System.IO;
using System.Xml;
using System.Xml.Serialization;

namespace CustomWriteBackLibrary
{
  public class ProcessJobStatusUpdate
  {
    private IGTApplication m_oApp;
    public ProcessJobStatusUpdate(IGTApplication p_oApp)
    {
      m_oApp = p_oApp;
    }

    /// <summary>
    /// Method to call processing of Update Job Status
    /// </summary>
    /// <param name="p_wrNumber">G3E_JOB.WR_NBR</param>
    /// <param name="p_jobStatus">G3E_JOB.JOB_STATUS</param>
    /// <param name="p_errorInfo">Any error infor</param>
    /// <returns>returns the Status as SUCCESS or FAILURE</returns>
    public string ProcessUpdateJobStatus(string p_wrNumber, string p_jobStatus, out string p_errorInfo)
    {
      string status = string.Empty;
      p_errorInfo = string.Empty;

      try
      {
        UpdateJobStatusSerializable oUpdateJob = GetRequestObject(p_wrNumber, p_jobStatus);
        DataAccess oDataAccess = new DataAccess(m_oApp);
        string sRequestURL = oDataAccess.GetEFUrl("WMIS_UpdateStatus", "EdgeFrontier");
        string sRequestXML = GetRequestXML(oUpdateJob);

        
        // --------------------------------------------------- Begin Temporary Logging ------------------------------------------
        //string sql = "insert into command_log " +
        //            "columns(command_name,command_nbr,log_code,log_context,log_type,log_msg) " +
        //            "values(?,?,?,?,?,?)";
        //IGTApplication app = GTClassFactory.Create<IGTApplication>();
        //// This CCNO is 71
        //app.DataContext.Execute(sql, out int recs, (int)ADODB.CommandTypeEnum.adCmdText, "SharedWriteBackLibrary.ProcessJobStatusUpdate", 0, "", "XML for RequestJobStatusUpdate", "INFO", sRequestXML);
        //app.DataContext.Execute(sql, out recs, (int)ADODB.CommandTypeEnum.adCmdText, "SharedWriteBackLibrary.ProcessJobStatusUpdate", 0, "", "URL for RequestJobStatusUpdate", "INFO", sRequestURL);
        //app.DataContext.Execute("commit", out recs, (int)ADODB.CommandTypeEnum.adCmdText);

        // --------------------------------------------------- End Temporary Logging ------------------------------------------


        status = ProcessRequest(sRequestURL, sRequestXML, out p_errorInfo);
        return status;
      }
      catch(Exception ex)
      {
        System.Diagnostics.Debug.Print(ex.Message);
        throw;
      }
    }

    private string ProcessRequest(string p_sRequestURL, string p_sRequestXML, out string p_errorInfo)
    {
      p_errorInfo = string.Empty;
      string status = "SUCCESS";
      SendReceiveXMLMessage oSendRecieve = new SendReceiveXMLMessage(p_sRequestURL, p_sRequestXML, "POST");
      oSendRecieve.SendMsgToEF();
      string sResponse = oSendRecieve.ResponseXML;

      if(!string.IsNullOrEmpty(sResponse))
      {
        bool ResponseIsXML = true;
        XmlDocument documentXML = new XmlDocument();

        try
        {
          documentXML.LoadXml(sResponse);  //loading soap message as string 
        }
        catch(Exception)
        {
          ResponseIsXML = false;
          status = "FAILURE";
          p_errorInfo = string.Format("Received the following response message from the ProcessJobStatus WEB Request: {0}{1}{1}This is not a valid XML message.", sResponse, System.Environment.NewLine);
        }

        if(ResponseIsXML)
        {
          string sStatusResponse = documentXML.ChildNodes[1].ChildNodes[0].ChildNodes[0].InnerText;
          status = sStatusResponse.Trim().Equals("SUCCESS") ? "SUCCESS" : "FAILURE";

          if(status.Equals("FAILURE"))
          {
            p_errorInfo = documentXML.ChildNodes[1].ChildNodes[0].ChildNodes[2].InnerText;
          }
        }

      }
      else
      {
        status = "FAILURE";
        p_errorInfo = string.Format("Received an empty response message from the WriteBack WEB Request.");
      }

      return status;
    }

    private string GetRequestXML(UpdateJobStatusSerializable p_oUpdateJob)
    {
      string sRequestXML = string.Empty;
      var xmlString = new StringWriter();
      XmlSerializer xmlSerializer = new XmlSerializer(typeof(UpdateJobStatusSerializable));

      XmlSerializerNamespaces ns = new XmlSerializerNamespaces();
      //ns.Add("xsi", "http://www.w3.org/2001/XMLSchema-instance");
      ns.Add("xsi", "http://www.w3.org/2001/XMLSchema-instance");

      xmlSerializer.Serialize(xmlString, p_oUpdateJob, ns);

      sRequestXML = xmlString.ToString();
      return sRequestXML;
    }

    private UpdateJobStatusSerializable GetRequestObject(string p_wrNumber, string p_jobStatus)
    {
      UpdateJobStatusSerializable oUpdateJob = new UpdateJobStatusSerializable();
      RequestHeaderType oRequest = new RequestHeaderType();
      string sTransactionID = string.Empty;

      DataAccess oDataAccess = new DataAccess(m_oApp);
      sTransactionID = oDataAccess.GetFirstFieldValueFromRecordset("select CORRELATION_ID_SEQ.nextval from dual");

      oRequest.TransactionType = "RequestReply";
      oRequest.SourceSystem = "GIS";
      oRequest.TransactionId = sTransactionID;
      oRequest.Requestor = m_oApp.DataContext.DatabaseUserName;
      oRequest.Timestamp = DateTime.Now;

      WorkRequestType oWork = new WorkRequestType();
      oWork.WRNumber = p_wrNumber;
      oWork.WRStatus = p_jobStatus;

      oUpdateJob.RequestHeaderType = oRequest;
      oUpdateJob.WorkRequestType = oWork;

      return oUpdateJob;
    }
  }
}
