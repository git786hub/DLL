using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;
using Intergraph.GTechnology.API;
using Intergraph.GTechnology.Interfaces;
using Intergraph.CoordSystems;
using ADODB;
using System.Data.OleDb;
using System.Data;

namespace GTechnology.Oncor.CustomAPI
{
  public class GISAuto_ServiceLine : IGISAutoPlugin
  {

    private IGTApplication gtApp = GTClassFactory.Create<IGTApplication>();
    private IGTDataContext gtDataContext;
    private IGTRelationshipService gtRelationshipService = GTClassFactory.Create<IGTRelationshipService>();
    private IGTTransactionManager gtTransactionManager;
    private IGTJobManagementService jobManagement = GTClassFactory.Create<IGTJobManagementService>();
    private IGTPoint m_StructureGeometry = null;

    private int g_Service_Point_FID;
    private int g_Service_Point_CID;
    private string g_Service_Point_State;
    private int connectingFacilityFID;
    private short facilityFNO = 59;
    private string g_Service_Info_Code;
    private string[] featureStates = { "PPI", "ABI", "INI", "CLS" };
    private short elecOwner = 2;
    private short elecOwnedBy = 3;
    private string status;
    private string g_Locate_Method;
    private int g_Service_Line_FID;
    private string g_Service_Line_State;
    private string message;
    private int g_Structure_FID;
    private int g_OwningStructureFID;
    private string g_Transaction_Number;
    private string g_ESI_Location;
    private string g_House_Number;
    private string g_Street_Name;
    private string g_Trans_Type;
    private string g_Structure_ID;
    private string g_Transformer_Number;
    private string g_CU;
    private string g_Unit;
    private string g_Exist_Prem_Gangbase;
    private string g_Meter_Latitude;
    private string g_Meter_Longitude;
    private int g_Meter_Tolerance_Override;
    private int g_Connecting_FID;
    private short g_Connecting_FNO;
    private string locateType;
    private short g_Structure_FNO;
    private short g_OwningStructureFNO;
    private short g_Common_CNO = 1;
    private short g_Connectivity_CNO = 11;
    private short g_CU_CNO = 21;
    private short g_Service_Line_FNO = 54;
    private short g_Service_Line_Attributes_CNO = 5401;
    private short g_ServiceLine_Linear_CNO = 5402;
    private const short g_Service_Point_FNO = 55;
    private short g_Service_Point_Attribute_CNO = 5501;
    private short g_Service_Point_Symbol_CNO = 5502;
    private short g_Service_Point_Label_CNO = 5503;
    private short g_Premise_Attributes_CNO = 5504;
    private short g_Transformer_OH_FNO = 59;
    private short g_Transformer_UG_FNO = 60;
    private int g_New_Line_FID;
    private short[] g_Struct_List = new short[] { 107, 108, 110, 113 };
    private short[] g_Facility_List = new short[] { 59, 60 };

    private OleDbDataAdapter odaListMaker = new OleDbDataAdapter();

    private const short ELECTRIC_CONNECTIVITY_RNO = 14;

    public GISAuto_ServiceLine(IGTTransactionManager transactionManager)
    {
      gtTransactionManager = transactionManager;
      gtDataContext = gtApp.DataContext;
    }
    public string SystemName
    {
      get
      {
        return "ServiceLine";
      }
    }

    public void Execute(GISAutoRequest autoRequest)
    {
      ProcessTransaction(autoRequest.requestXML);
    }

    // If error updating status in SERVICE_ACTIVITY table then throw error.
    /// <summary>
    /// Main function for proecessing transactions from the service Activity Table.
    /// </summary>
    /// <param name="xmlMessage">xml formatted message from the SERVICE_ACTIVITY table</param>        
    public void ProcessTransaction(string xmlMessage)
    {
      try
      {
        GetTransaction(xmlMessage);
        jobManagement.DataContext = gtDataContext;
        jobManagement.EditJob(gtDataContext.ActiveJob);

        gtTransactionManager.Begin("Automated Changes");
        string updateStatement = "UPDATE SERVICE_ACTIVITY SET STATUS_C = 'RUNNING' WHERE SERVICE_ACTIVITY_ID = ?";
        int recordsAffected;
        gtDataContext.Execute(updateStatement, out recordsAffected, (int)CommandTypeEnum.adCmdUnspecified, g_Transaction_Number);
        GetStructureFID();
        if(status == "FAILED")
        {
          UpdateTransaction(message, status);
          gtTransactionManager.Rollback();
        }
        else
        {
          LocateServicePoint();
          if(status == "FAILED")
          {
            UpdateTransaction(message, status);
            gtTransactionManager.Rollback();
          }
          else
          {
            switch(g_Service_Info_Code)
            {
              case "ES":
                StructureIdCorrection();
                break;
              case "GB":
                ServiceGangBaseInstall();
                break;
              case "MO":
                PremiseOnlyGangBaseInstall();
                break;
              case "NS":
                ServiceInstall();
                break;
              case "RM":
                ServiceRemoval();
                break;
              case "RP":
                ServiceLineChangeout();
                break;
            }
            if(status == "FAILED")
            {
              UpdateTransaction(message, status);
              gtTransactionManager.Rollback();
            }
            else
            {
              gtTransactionManager.Commit();
              string postErrorMessage = "";
              GTPostErrorConstants postError = jobManagement.PostJob();
              if(jobManagement.PostErrors != null)
              {
                jobManagement.PostErrors.Filter = "ErrorPriority = 'P1' OR ErrorPriority = 'P2'";
                Recordset errorRecords = jobManagement.PostErrors;
                if(!(errorRecords.EOF && errorRecords.BOF))
                {
                  errorRecords.MoveFirst();
                }
                message = string.Empty;
                while(!errorRecords.EOF)
                {
                  message = message + errorRecords.Fields["G3E_FID"].Value + "-" + errorRecords.Fields["ErrorDescription"].Value + ":\n";
                  errorRecords.MoveNext();
                }
                if(message.Length > 300)
                {
                  message = message.Substring(0, 299);
                }
                else
                {
                  message = message.Substring(0, message.Length - 1);
                }

                switch(postError)
                {
                  case GTPostErrorConstants.gtjmsValidation:
                    postErrorMessage = "Post error - validation errors detected:\n";
                    postErrorMessage = postErrorMessage + " " + message;
                    //message = errorRecords.GetString(StringFormatEnum.adClipString, 1000, ",").Substring(0, 300);
                    break;
                  case GTPostErrorConstants.gtjmsConflict:
                    postErrorMessage = "Post error - conflicts detected:\n";
                    postErrorMessage = postErrorMessage + " " + message;
                    //message = errorRecords.GetString(StringFormatEnum.adClipString, 1000, ",").Substring(0, 300);
                    break;
                  case GTPostErrorConstants.gtjmsDatabase:
                    postErrorMessage = "Post error - database error detected:\n";
                    postErrorMessage = postErrorMessage + " " + message;
                    //message = errorRecords.GetString(StringFormatEnum.adClipString, 1000, ",").Substring(0, 300);
                    break;
                  case GTPostErrorConstants.gtjmsNoPendingEdits:
                    postErrorMessage = "Post error - no pending edits ";
                    break;
                }
                if(postErrorMessage.Length > 0)
                {
                  jobManagement.DiscardJob();
                  UpdateTransaction(postErrorMessage, "FAILED");
                }
                else
                {
                  UpdateTransaction("", "SUCCESS");
                }
              }              
            }
          }
        }
      }
      catch(Exception e)
      {
        UpdateTransaction(e.Message, "FAILED");
        gtTransactionManager.Rollback();
      }
    }

    /// <summary>
    /// Service Line Changeout method, unifies all steps to perform action within gtech.
    /// </summary>
    private void ServiceLineChangeout()
    {
      GetServiceLine();
      if(status != "SUCCESS")
      {
        UpdateTransaction(message, status);
      }
      else
      {
        ReplaceServiceLine();
        if(status == "FAILED")
        {
          UpdateTransaction(message, status);
        }
        else
        {
          UpdateTransaction("Service replaced", "COMPLETED");
          status = "SUCCESS";
        }
      }
    }

    /// <summary>
    /// Service Removal method, unifies all steps to perform action within gtech.
    /// </summary>
    private void ServiceRemoval()
    {
      GetServiceLine();
      if(status == "FAILED")
      {
        UpdateTransaction(message, status);
      }
      else
      {
        if(g_Service_Line_State != "PPR" && g_Service_Line_State != "ABR" && g_Service_Line_State != "OSR" &&
            g_Service_Point_State != "PPR" && g_Service_Point_State != "ABR" && g_Service_Point_State != "OSR")
        {
          RemoveServicePoint();
          if(status == "FAILED")
          {
            UpdateTransaction(message, status);
          }
          else
          {
            if(g_Service_Line_FID != 0)
            {
              RemoveServiceLine();
              if (status == "FAILED")
              {
                UpdateTransaction(message, status);
              }
              else
              {
                UpdateTransaction("Service removed", "COMPLETED");
                status = "SUCCESS";
              }
            }
            else
            {
              UpdateTransaction("Service Point removed - no Service Line", "WARNING");
              status = "SUCCESS";
            }
          }
        }
        else
        {
          UpdateTransaction("WR Estimated", "WARNING");
          status = "SUCCESS";
        }
      }
    }

    /// <summary>
    /// Service Install method, unifies all steps to perform action within gtech.
    /// </summary>
    private void ServiceInstall()
    {
      if(g_Service_Point_FID == 0)
      {
        GetConnectingFacility();
        if(status == "FAILED")
        {
          UpdateTransaction(message, status);
        }
        else
        {
          if(g_Meter_Tolerance_Override == 0)
          {
            CheckMeterGeocodeLocation();
            if(status == "FAILED")
            {
              UpdateTransaction(message, status);
            }
          }

          if(status != "FAILED")
          {
            AddServicePoint();
            if(status == "FAILED")
            {
              UpdateTransaction(message, status);
            }
            else
            {
              AddPremise();
              if(status == "FAILED")
              {
                UpdateTransaction(message, status);
              }
              else if(!g_CU.Contains("CUSTOMER"))
              {
                AddServiceLine();
                if(status == "FAILED")
                {
                  UpdateTransaction(message, status);
                }
                else
                {
                  EstablishConnectivity();
                  if(status == "FAILED")
                  {
                    UpdateTransaction(message, status);
                  }
                  //TODO: add code for structure ownership once service point can be owned
                  UpdateTransaction("SERVICE PLACED", "COMPLETE");

                  IGTKeyObject structure = gtDataContext.OpenFeature(g_Structure_FNO, Convert.ToInt32(g_Structure_FID));
                  gtRelationshipService.ActiveFeature = gtDataContext.OpenFeature(g_Service_Line_FNO, g_Service_Line_FID);
                  gtRelationshipService.SilentEstablish(elecOwnedBy, structure);
                }
              }
              else
              {
                EstablishConnectivity();
                if(status == "FAILED")
                {
                  UpdateTransaction(message, status);
                }
                //TODO: add code for structure ownership once service point can be owned
                UpdateTransaction("SERVICE PLACED", "COMPLETE");
              }//need to add else statement
            }
          }
        }
      }
      else
      {
        if(!g_CU.Contains("CUSTOMER"))
        {
          GetServiceLine();
          if(status == "FAILED")
          {
            UpdateTransaction(message, status);
          }
        }
        if((g_Locate_Method == "ADDRESS" || g_Locate_Method == "ESI LOCATION" || g_Locate_Method == "GANG ESI LOCATION") && g_Service_Point_CID != 0)
        {
          UpdatePremise();
          if(status == "FAILED")
          {
            UpdateTransaction(message, status);
          }
        }
        else if(g_Locate_Method == "METER GEOCODE" || g_Locate_Method == "GANG ESI LOCATION" && g_Service_Point_CID == 0)
        {
          AddPremise();
          if(status == "FAILED")
          {
            UpdateTransaction(message, status);
          }
        }

        if(status != "FAILED")
        {
          if(!g_CU.Contains("CUSTOMER"))
          {
            string serviceLineStatement = "SELECT SL.PLACEMENT_TYPE_C, COMM.FEATURE_STATE_C FROM SERVICE_LINE_N SL, COMMON_N COMM WHERE SL.G3E_FID = ? AND SL.G3E_FID = COMM.G3E_FID";
            Recordset serviceLine = gtDataContext.OpenRecordset(serviceLineStatement, CursorTypeEnum.adOpenStatic, LockTypeEnum.adLockReadOnly, (int)CommandTypeEnum.adCmdText, g_Service_Line_FID);

            if(null != serviceLine && 0 < serviceLine.RecordCount)
            {
              serviceLine.MoveFirst();

              string[] states = { "PPI", "ABI", "INI" };
              if(serviceLine.Fields["PLACEMENT_TYPE_C"].Value.ToString() == "ASSOCIATED" && states.Contains(serviceLine.Fields["FEATURE_STATE_C"].Value.ToString()))
              {
                UpdateServiceLine();

                if(status == "FAILED")
                {
                  UpdateTransaction(message, status);
                }
                else
                {
                  UpdateTransaction("SERVICE PLACED", "COMPLETED");
                }
              }
              else
              {
                UpdateTransaction("Service already estimated", "WARNING");
              }
            }
          }
          else
          {
            UpdateTransaction("SERVICE PLACED", "COMPLETED");
          }
        }
      }
    }

    /// <summary>
    /// Premise Only Gangebase Install method, unifies all steps to perform action within gtech.
    /// </summary>
    private void PremiseOnlyGangBaseInstall()
    {
      if (g_Service_Point_FID != 0)
      {      
        IGTKeyObject servicePtKO = gtDataContext.OpenFeature(g_Service_Point_FNO, g_Service_Point_FID);
        Recordset servicePtRS = servicePtKO.Components.GetComponent(g_Service_Point_Attribute_CNO).Recordset;
        string gangMeter = "N";

        if(servicePtRS.RecordCount > 0)
        {
          servicePtRS.MoveFirst();
          gangMeter = servicePtRS.Fields["GANG_METER_YN"].Value.ToString();
        }

        if(gangMeter == "Y")
        {
          if(g_Locate_Method == "ESI LOCATION")
          {
            UpdateTransaction("Premise set", "COMPLETED");
          }
          else if((g_Locate_Method == "ADDRESS" || g_Locate_Method == "METER GEOCODE" || g_Locate_Method == "GANG ESI LOCATION") && g_Service_Point_CID != 0)
          {
            UpdatePremise();
            if(status == "FAILED")
            {
              UpdateTransaction(message, status);
            }
            else
            {
              UpdateTransaction("Premise set", "COMPLETED");
              status = "SUCCESS";
            }
          }
          else if((g_Locate_Method == "ADDRESS" || g_Locate_Method == "METER GEOCODE" || g_Locate_Method == "GANG ESI LOCATION") && g_Service_Point_CID == 0)
          {
            AddPremise();
            if(status == "FAILED")
            {
              UpdateTransaction(message, status);
            }
            else
            {
              UpdateTransaction("Premise set", "COMPLETED");
              status = "SUCCESS";
            }
          }
        }
        else
        {
          message = "INVALID GANG METER";
          status = "FAILED";
        }
      }
      else
      {
        message = "INVALID GANG METER";
        status = "FAILED";
      }
    }

    /// <summary>
    /// Service Gangebase Install method, unifies all steps to perform action within gtech.
    /// </summary>
    private void ServiceGangBaseInstall()
    {
      if(g_Service_Point_FID == 0)
      {
        GetConnectingFacility();
        if(status == "FAILED")
        {
          UpdateTransaction(message, status);
        }
        else
        {
          if(g_Meter_Tolerance_Override == 0)
          {
            CheckMeterGeocodeLocation();
          }
          if(status != "FAILED")
          {
            AddServicePoint();
            if(status == "FAILED")
            {
              UpdateTransaction(message, status);
            }
            else
            {
              AddPremise();
              if(status == "FAILED")
              {
                UpdateTransaction(message, status);
              }
              else
              {
                AddServiceLine();
                if(status == "FAILED")
                {
                  UpdateTransaction(message, status);
                }
                else
                {
                  EstablishConnectivity();
                  if(status == "FAILED")
                  {
                    UpdateTransaction(message, status);
                  }
                  else
                  {
                    UpdateTransaction("Service placed", "COMPLETED");

                    IGTKeyObject structure = gtDataContext.OpenFeature(g_Structure_FNO, Convert.ToInt32(g_Structure_FID));
                    gtRelationshipService.ActiveFeature = gtDataContext.OpenFeature(g_Service_Line_FNO, g_Service_Line_FID);
                    gtRelationshipService.SilentEstablish(elecOwnedBy, structure);
                  }
                }
              }
            }
          }
        }
      }
      else
      {
        UpdateServicePoint();
        if(status == "FAILED")
        {
          UpdateTransaction(message, status);
        }
        else
        {
          GetServiceLine();
          if(status == "FAILED")
          {
            UpdateTransaction(message, status);
          }
          else
          {
            if((g_Locate_Method == "ESI LOCATION" || g_Locate_Method == "ADDRESS" || g_Locate_Method == "GANG ESI LOCATION") && g_Service_Point_CID != 0)
            {
              UpdatePremise();
            }
            else if((g_Locate_Method == "METER GEOCODE" || g_Locate_Method == "GANG ESI LOCATION") && g_Service_Point_CID == 0)
            {
              AddPremise();
            }

            if(status == "FAILED")
            {
              UpdateTransaction(message, status);
            }
            else
            {
              string serviceLineStatement = "SELECT SL.PLACEMENT_TYPE_C, COMM.FEATURE_STATE_C FROM SERVICE_LINE_N SL, COMMON_N COMM WHERE SL.G3E_FID = ? AND SL.G3E_FID = COMM.G3E_FID";
              Recordset serviceLine = gtDataContext.OpenRecordset(serviceLineStatement, CursorTypeEnum.adOpenStatic, LockTypeEnum.adLockReadOnly, (int)CommandTypeEnum.adCmdText, g_Service_Line_FID);

              if(null != serviceLine && 0 < serviceLine.RecordCount)
              {
                serviceLine.MoveFirst();
                string[] states = { "PPI", "ABI", "INI" };

                if(serviceLine.Fields["PLACEMENT_TYPE_C"].Value.ToString() == "ASSOCIATED" && states.Contains(serviceLine.Fields["FEATURE_STATE_C"].Value.ToString()))
                {
                  UpdateServiceLine();

                  if(status == "FAILED")
                  {
                    UpdateTransaction(message, status);
                  }
                  else
                  {
                    UpdateTransaction("SERVICE PLACED", "COMPLETED");
                  }
                }
                else
                {
                  UpdateTransaction("Service already estimated", "WARNING");
                }
              }
            }
          }
        }
      }
    }

    /// <summary>
    /// Method for retrieving applicable data from the xml message.
    /// </summary>
    /// <param name="xmlRecord">Xml formatted message from the Service Activity Table</param>
    /// <returns>true or false based on whether or not data retrieval was successful</returns>
    private bool GetTransaction(string xmlRecord)
    {
      try
      {
        if(xmlRecord != null)
        {
          XmlDocument xmlDoc = new XmlDocument();
          xmlDoc.LoadXml(xmlRecord);
          XmlNode transaction = xmlDoc.SelectSingleNode("Transaction");
          g_Transaction_Number = transaction.SelectSingleNode("SERVICE_ACTIVITY_ID").InnerText;
          g_ESI_Location = transaction.SelectSingleNode("ESI_LOCATION").InnerText;
          g_House_Number = transaction.SelectSingleNode("HOUSE_NO").InnerText;
          g_Street_Name = transaction.SelectSingleNode("STREET_NAME").InnerText;
          g_Service_Info_Code = transaction.SelectSingleNode("SERVICE_INFO_CODE").InnerText;
          g_Trans_Type = transaction.SelectSingleNode("O_OR_U_CODE").InnerText;
          g_Structure_ID = transaction.SelectSingleNode("SVC_STRUCTURE_ID").InnerText;
          g_Transformer_Number = transaction.SelectSingleNode("TRF_CO_H").InnerText + "";
          g_CU = transaction.SelectSingleNode("CU_ID").InnerText;
          g_Unit = transaction.SelectSingleNode("UNIT_H").InnerText;
          g_Exist_Prem_Gangbase = transaction.SelectSingleNode("EXIST_PREM_GANGBASE").InnerText;
          g_Meter_Latitude = transaction.SelectSingleNode("METER_LATITUDE").InnerText;
          g_Meter_Longitude = transaction.SelectSingleNode("METER_LONGITUDE").InnerText;
          g_Meter_Tolerance_Override = Convert.ToInt16(transaction.SelectSingleNode("OVERRIDE_TOLERANCE").InnerText);
          status = "SUCCESS";
          return true;
        }
        else
        {
          status = "FAILED";
          message = "NO XML DATA FOUND";
          return false;
        }
      }
      catch(Exception e)
      {
        status = "FAILED";
        message = e.Message;
        return false;
      }
    }

    /// <summary>
    /// Updates the record in the SERVICE_ACTIVITY table for the transaction currently underway.
    /// </summary>
    /// <param name="messageInput">Message to be recorded to the table</param>
    /// <param name="statusInput">Status to be recorded to the table</param>
    /// <returns>true or false based on success of the method</returns>
    private bool UpdateTransaction(string messageInput, string statusInput)
    {
      try
      {
        string updateStatement = "UPDATE SERVICE_ACTIVITY SET ";
        updateStatement += "MSG_T = '" + messageInput + "', ";
        updateStatement += "STATUS_C = '" + statusInput + "' ";
        updateStatement += "WHERE SERVICE_ACTIVITY_ID = " + g_Transaction_Number;
        int x = 0;
        gtDataContext.Execute(updateStatement, out x, (int)ADODB.CommandTypeEnum.adCmdText);
        gtDataContext.Execute("COMMIT", out x, (int)ADODB.CommandTypeEnum.adCmdText);
        message = messageInput;
        status = statusInput;
        return true;
      }
      catch(Exception)
      {
        return false;
      }
    }

    /// <summary>
    /// Adds a premise to the service point associated with the current transaction.
    /// </summary>
    /// <returns>returns a boolean based on success of the method</returns>
    private bool AddPremise()
    {
      try
      {
        bool retVal = false;

        IGTKeyObject editedFeature = gtDataContext.OpenFeature(g_Service_Point_FNO, g_Service_Point_FID);
        if(null != editedFeature)
        {
          IGTComponent premiseAttributes = editedFeature.Components.GetComponent(g_Premise_Attributes_CNO);
          if(null != premiseAttributes && null != premiseAttributes.Recordset)
          {
            Recordset componentAdd = premiseAttributes.Recordset;

            componentAdd.AddNew();
            componentAdd.Fields["G3E_FNO"].Value = g_Service_Point_FNO;
            componentAdd.Fields["G3E_FID"].Value = g_Service_Point_FID;
            componentAdd.Fields["PREMISE_NBR"].Value = g_ESI_Location.ToString();
            componentAdd.Update();

            status = "SUCCESS";
            retVal = true;
          }
        }

        return retVal;
      }
      catch(Exception e)
      {
        status = "FAILED";
        message = e.Message;
        return false;
      }
    }

    /// <summary>
    /// Removes a premise from the service point associated with the transaction. Based on the ESI location in the transaction.
    /// </summary>
    internal void RemovePremise()
    {
      try
      {
        Recordset componentRS = ComponentRecordset(g_Service_Point_FNO, g_Service_Point_FID, g_Premise_Attributes_CNO);

        if(null != componentRS)
        {
          componentRS.MoveFirst();

          do
          {
            if(DBNull.Value != componentRS.Fields["PREMISE_NBR"].Value && componentRS.Fields["PREMISE_NBR"].Value.ToString() == g_ESI_Location)
            {
              break;
            }
            componentRS.MoveNext();
          } while(!componentRS.EOF);

          if(componentRS.EOF)
          {
            status = "FAILED";
            message = "No matching ESI Location found";
          }
          else
          {
            componentRS.Delete();
            status = "SUCCESS";
          }
        }
      }
      catch(Exception e)
      {
        status = "FAILED";
        message = e.Message;
      }
    }

    /// <summary>
    /// Updates the premise associated with the Transaction. 
    /// </summary>
    internal void UpdatePremise()
    {
      try
      {
        Recordset componentAdd = ComponentRecordset(g_Service_Point_FNO, g_Service_Point_FID, g_Premise_Attributes_CNO);

        if(null != componentAdd)
        {
          componentAdd.MoveFirst();

          do
          {
            if(DBNull.Value != componentAdd.Fields["G3E_CID"].Value && Convert.ToInt32(componentAdd.Fields["G3E_CID"].Value) == g_Service_Point_CID)
            {
              break;
            }
            componentAdd.MoveNext();
          } while(!componentAdd.EOF);

          if(componentAdd.EOF)
          {
            status = "FAILED";
            message = "No matching ESI Location found";
          }
          else
          {
            try
            {
              componentAdd.Fields["PREMISE_NBR"].Value = g_ESI_Location;
              status = "SUCCESS";
            }
            catch(Exception ex)
            {
              if (ex.Message == "Operation was canceled.")
              {
                status = "SUCCESS";
              }
              else
              {
                status = "FAILED";
                message = ex.Message;
              }
            }
          }
        }
      }
      catch(Exception e)
      {
        status = "FAILED";
        message = e.Message;
      }
    }

    /// <summary>
    /// Method to add a service point, based on the Meter Geocod Location.
    /// </summary>
    internal void AddServicePoint()
    {
      try
      {
        IGTKeyObject addedObject = gtDataContext.NewFeature(g_Service_Point_FNO);
        SetAttributeDefaults(addedObject);
        IGTComponent serviceSymbol = addedObject.Components.GetComponent(g_Service_Point_Symbol_CNO);
        IGTComponent serviceLabel = addedObject.Components.GetComponent(g_Service_Point_Label_CNO);

        if(g_Service_Info_Code == "GB" || (g_Service_Info_Code == "NS" && !g_CU.Contains("CUSTOMER")))
        {
          if(g_Meter_Latitude != null && g_Meter_Longitude != null)
          {
            IGTOrientedPointGeometry point = GTClassFactory.Create<IGTOrientedPointGeometry>();
            point.Origin = gCoordinateConvert(Double.Parse(g_Meter_Longitude), Double.Parse(g_Meter_Latitude), 0);
            IGTTextPointGeometry labelPoint = GTClassFactory.Create<IGTTextPointGeometry>();
            labelPoint.Origin = point.Origin;
            serviceSymbol.Geometry = point;
            labelPoint.Alignment = GTAlignmentConstants.gtalTopLeft;
            serviceLabel.Geometry = labelPoint;
            IGTComponent commonAttributes = addedObject.Components.GetComponent(1);
            commonAttributes.Recordset.MoveFirst();
            g_Service_Point_FID = addedObject.FID;
            commonAttributes.Recordset.Fields["FEATURE_STATE_C"].Value = "CLS";

            if(g_Service_Info_Code == "GB")
            {
              Recordset rsNewServicePointAttr = ComponentRecordset(addedObject.FNO, addedObject.FID, g_Service_Point_Attribute_CNO);

              if(null != rsNewServicePointAttr)
              {
                rsNewServicePointAttr.MoveFirst();
                rsNewServicePointAttr.Fields["GANG_METER_YN"].Value = "Y";
              }
            }
          }
          else
          {
            status = "FAILED";
            message = "No meter geolocation found";
          }
        }
        else
        {
          string structFNOQuery = "SELECT G3E_FNO FROM COMMON_N WHERE G3E_FID = ?";
          Recordset structureRecords = gtDataContext.OpenRecordset(structFNOQuery, CursorTypeEnum.adOpenStatic, LockTypeEnum.adLockReadOnly, (int)CommandTypeEnum.adCmdText, g_Structure_FID);

          if(null != structureRecords && 0 < structureRecords.RecordCount)
          {
            structureRecords.MoveFirst();
            IGTOrientedPointGeometry point = GTClassFactory.Create<IGTOrientedPointGeometry>();
            IGTKeyObject structure = gtDataContext.OpenFeature(Convert.ToInt16(structureRecords.Fields["G3E_FNO"].Value), g_Structure_FID);
            Recordset structureMetaData = gtDataContext.MetadataRecordset("G3E_FEATURES_OPTABLE", "G3E_FNO = " + structureRecords.Fields["G3E_FNO"].Value);

            if(null != structureMetaData && 0 < structureMetaData.RecordCount)
            {
              structureMetaData.MoveFirst();
              IGTComponent geoComponent = structure.Components.GetComponent(Convert.ToInt16(structureMetaData.Fields["G3E_PRIMARYGEOGRAPHICCNO"].Value));

              if(null != geoComponent && null != geoComponent.Geometry)
              {
                point.Origin = geoComponent.Geometry.FirstPoint;
                serviceSymbol.Geometry = point;

                IGTTextPointGeometry labelPoint = GTClassFactory.Create<IGTTextPointGeometry>();
                labelPoint.Origin = point.Origin;
                labelPoint.Alignment = GTAlignmentConstants.gtalTopLeft;
                serviceLabel.Geometry = labelPoint;

                Recordset rsCommon = ComponentRecordset(addedObject.FNO, addedObject.FID, 1);

                if(null != rsCommon)
                {
                  rsCommon.MoveFirst();
                  rsCommon.Fields["FEATURE_STATE_C"].Value = "CLS";
                }

                g_Service_Point_FID = addedObject.FID;
              }
            }
          }
        }
      }
      catch(Exception e)
      {
        status = "FAILED";
        message = e.Message;
      }
    }

    /// <summary>
    /// Method to convert from geo coordinates to x y. Must have the proper coord system from teh data base. 
    /// </summary>
    /// <param name="dX"></param>
    /// <param name="dY"></param>
    /// <param name="dZ"></param>
    /// <returns></returns>
    internal IGTPoint gCoordinateConvert(double dX, double dY, double dZ)
    {
      //CoordSystem coordsystem;
      //IDictionary<string, IAltCoordSystemPath> myAltPathDict;
      IGTPoint point = GTClassFactory.Create<IGTPoint>();
      //ICoordSystemsMgr coordMgr;
      //IAltCoordSystemPath tmpPath;
      const double degToRad = Math.PI / 180.0;

      try
      {
        //// convert LAT LONG to State Plane
        //coordMgr = gtDataContext.CoordSystemsMgr;
        //System.Diagnostics.Debug.Print("Lon :" + dX.ToString() + " Lat :" + dY.ToString());

        ////Convert to radians
        //dX = dX * degToRad;
        //dY = dY * degToRad;

        //coordsystem = coordMgr.BaseCoordSystem as CoordSystem;
        //myAltPathDict = coordMgr.AltPathDictionary;
        //myAltPathDict.TryGetValue("{720E36F0-5586-4960-84B1-4544BA6123B3}", out tmpPath);
        //// Coodinate systems in database.

        //((ILinkableTransformation)coordsystem).TransformPoint(CSPointConstants.cspLLU, 1,
        //                                                      CSPointConstants.cspENU, 1, //CSPointConstants.cspENO
        //                                                      ref dX, ref dY, ref dZ);

        //// convert meters to feet
        //point.X = dX * 3.28084;
        //point.Y = dY * 3.28084;
        //point.Z = 0.0;

        //System.Diagnostics.Debug.Print("X :" + dX.ToString() + " Y :" + dY.ToString());
        //coordMgr = null;
        //coordsystem = null;

        IGTHelperService helperSrvc = GTClassFactory.Create<IGTHelperService>();
        helperSrvc.DataContext = gtDataContext;
        point.X = dX * degToRad;
        point.Y = dY * degToRad;
        point = helperSrvc.GeographicPointToStorage(point);
        helperSrvc = null;

        return point;
      }
      catch(Exception ex)
      {
        status = "FAILED";
        message = string.Format("Error in gCoordinateConvert: {0}", ex.Message);
        point.X = 0.0;
        point.Y = 0.0;
        return point;
      }

    }

    /// <summary>
    /// Method to remove service point. Based on values retrieved through locate service point.
    /// </summary>
    internal void RemoveServicePoint()
    {
      try
      {
        IGTKeyObject removeObject = gtDataContext.OpenFeature(g_Service_Point_FNO, g_Service_Point_FID);
        if(DeleteFeature(g_Service_Point_FID, g_Service_Point_FNO))
        {
          status = "SUCCESS";
        }
        else
        {
          status = "FAILED";
        }
      }
      catch(Exception e)
      {
        status = "FAILED";
        message = e.Message;
      }
    }

    /// <summary>
    /// Method to get the FID of the structure feature. Retrieves it based on the provided STRUCT_ID.
    /// </summary>
    public void GetStructureFID()
    {
      try
      {
        if(g_Structure_ID != null)
        {
          string queryStatement = "SELECT G3E_FID, G3E_FNO FROM COMMON_N WHERE STRUCTURE_ID = ? AND UPPER(FEATURE_STATE_C) IN ('PPI', 'ABI', 'INI', 'CLS') AND G3E_FNO IN (?,?,?,?)";
          Recordset feature = gtDataContext.OpenRecordset(queryStatement, CursorTypeEnum.adOpenStatic, LockTypeEnum.adLockReadOnly, (int)CommandTypeEnum.adCmdText, g_Structure_ID, g_Struct_List[0], g_Struct_List[1], g_Struct_List[2], g_Struct_List[3]);

          if(null != feature && 0 == feature.RecordCount)
          {
            message = "NO MATCHING STRUCTURE";
            status = "FAILED";
          }
          else
          {
            feature.MoveFirst();

            if(feature.RecordCount == 1)
            {
              g_Structure_FID = Convert.ToInt32(feature.Fields["G3E_FID"].Value);
              g_Structure_FNO = Convert.ToInt16(feature.Fields["G3E_FNO"].Value);
              status = "SUCCESS";
            }
            else
            {
              message = "DUPLICATE STRUCTURES FOUND";
              status = "FAILED";
            }
          }
        }
        else
        {
          message = "NO STRUCTURE ID";
          status = "FAILED";
        }
      }
      catch(Exception e)
      {
        status = "FAILED";
        message = e.Message;
      }
    }

    /// <summary>
    /// Establishes connectivity to a feature based on the info codes or cu codes.
    /// </summary>
    internal void EstablishConnectivity()
    {
      try
      {
        if(g_Service_Info_Code == "GB" || (g_Service_Info_Code == "NS" && !g_CU.Contains("CUSTOMER")))
        {
          gtRelationshipService.DataContext = gtDataContext;
          gtRelationshipService.ActiveFeature = gtDataContext.OpenFeature(g_Service_Line_FNO, g_Service_Line_FID);
          Recordset RNORecords = gtDataContext.MetadataRecordset("G3E_RELATIONSHIPS_OPTABLE", "G3E_TYPE = 2 AND G3E_TABLE = 'G3E_NODEEDGECONN_ELEC'");
          IGTKeyObject relatedFacility = gtDataContext.OpenFeature(g_Connecting_FNO, g_Connecting_FID);
          IGTKeyObject relatedServicePoint = gtDataContext.OpenFeature(g_Service_Point_FNO, g_Service_Point_FID);
          short RNO;

          if(null != RNORecords && 0 < RNORecords.RecordCount)
          {
            RNORecords.MoveFirst();

            RNO = Convert.ToInt16(RNORecords.Fields["G3E_RNO"].Value);
            if(g_Connecting_FNO == g_Transformer_OH_FNO || g_Connecting_FNO == g_Transformer_UG_FNO)
            {
              gtRelationshipService.SilentEstablish(RNO, relatedFacility, GTRelationshipOrdinalConstants.gtrelRelationshipOrdinal1, GTRelationshipOrdinalConstants.gtrelRelationshipOrdinal2);
            }
            else
            {
              Recordset connectingConnRS = relatedFacility.Components.GetComponent(g_Connectivity_CNO).Recordset;
              GTRelationshipOrdinalConstants connectingRelationshipOrdinal = GTRelationshipOrdinalConstants.gtrelRelationshipOrdinal1;
              if(connectingConnRS.RecordCount > 0)
              {
                connectingConnRS.MoveFirst();
                int node1 = Convert.ToInt32(connectingConnRS.Fields["NODE_1_ID"].Value);
                connectingRelationshipOrdinal = DetermineConnectingNode(node1, g_Connecting_FID, g_Structure_FID);
              }
              gtRelationshipService.SilentEstablish(RNO, relatedFacility, GTRelationshipOrdinalConstants.gtrelRelationshipOrdinal1, connectingRelationshipOrdinal);
            }

            gtRelationshipService.ActiveFeature = relatedServicePoint;
            gtRelationshipService.SilentEstablish(RNO, gtDataContext.OpenFeature(g_Service_Line_FNO, g_Service_Line_FID), GTRelationshipOrdinalConstants.gtrelRelationshipOrdinal1, GTRelationshipOrdinalConstants.gtrelRelationshipOrdinal2);
            status = "SUCCESS";
          }
          else//this should never be triggered but just to cover all our bases 
          {
            status = "FAILED";
            message = "NO ELECTICAL CONNECTIVITY RELATIONSHIP FOUND";
          }
        }
        else
        {
          gtRelationshipService.DataContext = gtDataContext;
          gtRelationshipService.ActiveFeature = gtDataContext.OpenFeature(g_Service_Point_FNO, g_Service_Point_FID);
          Recordset RNORecords = gtDataContext.MetadataRecordset("G3E_RELATIONSHIPS_OPTABLE", "G3E_TYPE = 2 AND G3E_TABLE = 'G3E_NODEEDGECONN_ELEC'");
          IGTKeyObject relatedFacility = gtDataContext.OpenFeature(g_Connecting_FNO, g_Connecting_FID);
          short RNO;

          if(null != RNORecords && 0 < RNORecords.RecordCount)
          {
            RNORecords.MoveFirst();
            RNO = Convert.ToInt16(RNORecords.Fields["G3E_RNO"].Value);
            gtRelationshipService.SilentEstablish(RNO, relatedFacility, GTRelationshipOrdinalConstants.gtrelRelationshipOrdinal1, GTRelationshipOrdinalConstants.gtrelRelationshipOrdinal2);
            status = "SUCCESS";
          }
          else//this should never be triggered but just to cover all our bases 
          {
            status = "FAILED";
            message = "NO ELECTICAL CONNECTIVITY RELATIONSHIP FOUND";
          }
        }
      }
      catch(Exception e)
      {
        status = "FAILED";
        message = e.Message;
      }
    }

    /// <summary>
    /// Checks to see if the feature is within a tolerable distance of the meter.
    /// </summary>
    internal void CheckMeterGeocodeLocation()
    {
      try
      {
        int? geocodeToleranceOH = null;
        string tmp = GeneralParameter("GISAUTO_SERVICELINE", "TOLERANCE", "GEOCODE_TOLERANCE_OH");

        if(!string.IsNullOrEmpty(tmp))
        {
          geocodeToleranceOH = Convert.ToInt32(tmp);
        }
        else
        {
          status = "FAILED";
          message = "GEOCODE_TOLERANCE_OH missing from SYS_GENERALPARAMETER table.";
        }

        int? geocodeToleranceUG = null;
        tmp = GeneralParameter("GISAUTO_SERVICELINE", "TOLERANCE", "GEOCODE_TOLERANCE_UG");

        if(!string.IsNullOrEmpty(tmp))
        {
          geocodeToleranceUG = Convert.ToInt32(tmp);
        }
        else
        {
          status = "FAILED";
          message = "GEOCODE_TOLERANCE_UG missing from SYS_GENERALPARAMETER table.";
        }

        if(geocodeToleranceOH != null && geocodeToleranceUG != null)
        {
          Recordset metadata = gtDataContext.MetadataRecordset("G3E_FEATURES_OPTABLE", "G3E_FNO = " + g_Structure_FNO);
          metadata.MoveFirst();

          IGTKeyObject structure = gtDataContext.OpenFeature(g_Structure_FNO, g_Structure_FID);
          IGTComponent structureGeometry = structure.Components.GetComponent(Convert.ToInt16(metadata.Fields["G3E_PRIMARYGEOGRAPHICCNO"].Value));

          IGTPoint structPoint = GTClassFactory.Create<IGTPoint>();
          structPoint.X = Convert.ToDouble(structureGeometry.Geometry.FirstPoint.X);
          structPoint.Y = Convert.ToDouble(structureGeometry.Geometry.FirstPoint.Y);

          IGTPoint geocodeXAndY = gCoordinateConvert(Double.Parse(g_Meter_Longitude), Double.Parse(g_Meter_Latitude), 0);
          double distanceCalculationX = structPoint.X - geocodeXAndY.X;
          double distanceCalculationY = structPoint.Y - geocodeXAndY.Y;
          double distanceCalculation = Math.Sqrt((distanceCalculationX * distanceCalculationX) + (distanceCalculationY * distanceCalculationY)) * 3.2808399;

          if(g_Trans_Type == "O" && distanceCalculation <= geocodeToleranceOH)
          {
            status = "SUCCESS";
          }
          else if(g_Trans_Type == "U" && distanceCalculation <= geocodeToleranceUG)
          {
            status = "SUCCESS";
          }
          else
          {
            status = "FAILED";
            message = "INVALID METER GEOCODE";
          }
        }
      }
      catch(Exception e)
      {
        message = e.Message;
        status = "FAILED";
      }
    }

    /// <summary>
    /// Updates the service point with data from the xml message.
    /// </summary>
    internal void UpdateServicePoint()
    {
      try
      {
        if(g_Service_Info_Code == "GB" || g_Service_Info_Code == "MO")
        {
          Recordset componentEdit = ComponentRecordset(g_Service_Point_FNO, g_Service_Point_FID, g_Service_Point_Attribute_CNO);

          if(null != componentEdit)
          {
            componentEdit.MoveFirst();
            componentEdit.Fields["GANG_METER_YN"].Value = "Y";
            status = "SUCCESS";
          }
        }
      }
      catch(Exception e)
      {
        message = e.Message;
        status = "FAILED";
      }
    }

    /// <summary>
    /// Adds a service line to connect a transformer to a service point. Either does a brand new placement or replaces an already existant line.
    /// </summary>
    internal void AddServiceLine()
    {
      try
      {
        Recordset componentMetaData = gtDataContext.MetadataRecordset("G3E_COMPONENTINFO_OPTABLE", "G3E_CNO = 5402");
        if(!componentMetaData.BOF && !componentMetaData.EOF)
        {
          componentMetaData.MoveFirst();
          string geometryType = componentMetaData.Fields["G3E_GEOMETRYTYPE"].Value.ToString();
          if(geometryType.ToUpper() == "POLYLINEGEOMETRY")
          {
            if(g_Service_Info_Code == "GB" || g_Service_Info_Code == "NS")
            {
              // If connecting feature is a Transformer and Transformer is connected to a Dead End type Secondary Conductor Node, then
              // delete Secondary Conductor Node. The Dead End Secondary Conductor Node is only there for the Transformer to pass
              // connectivity validation and is no longer needed since a Service Line is now being connected.
              if(g_Connecting_FNO == g_Transformer_OH_FNO || g_Connecting_FNO == g_Transformer_UG_FNO)
              {
                string sql = "select conn1.g3e_fno, conn1.g3e_fid from connectivity_n conn1, connectivity_n conn2, sec_cond_node_n secCondNode " +
                             "where conn2.g3e_fid = ? and conn2.node_2_id <> 0 and conn2.node_2_id = conn1.node_1_id " +
                             "and conn1.g3e_fid = secCondNode.g3e_fid and secCondnode.type_c = 'DEADEND'";

                Recordset connRS = gtDataContext.OpenRecordset(sql, CursorTypeEnum.adOpenDynamic, LockTypeEnum.adLockOptimistic, (int)CommandTypeEnum.adCmdText, g_Connecting_FID);
                if(connRS.RecordCount > 0)
                {
                  Int16 secCondNodeFNO;
                  Int32 secCondNodeFID;

                  connRS.MoveFirst();
                  while(!connRS.EOF)
                  {
                    secCondNodeFNO = Convert.ToInt16(connRS.Fields["G3E_FNO"].Value);
                    secCondNodeFID = Convert.ToInt32(connRS.Fields["G3E_FID"].Value);
                    DeleteFeature(secCondNodeFID, secCondNodeFNO);
                    connRS.MoveNext();
                  }
                }

              }

              IGTKeyObject addedObject = gtDataContext.NewFeature(54);
              SetAttributeDefaults(addedObject);
              IGTPolylineGeometry polylineGeometry = GTClassFactory.Create<IGTPolylineGeometry>();
              Recordset ConnectingFeatureGeometryInformation = gtDataContext.MetadataRecordset("G3E_FEATURES_OPTABLE", "G3E_FNO = " + g_Service_Point_FNO);
              short geometryConnectingFeature = Convert.ToInt16(ConnectingFeatureGeometryInformation.Fields["G3E_PRIMARYGEOGRAPHICCNO"].Value.ToString());//get the cno for the geometry of the feature
              Recordset FacilityGeometryInformation = gtDataContext.MetadataRecordset("G3E_FEATURES_OPTABLE", "G3E_FNO = " + g_Connecting_FNO);
              short FacilityConnectingFeature = Convert.ToInt16(FacilityGeometryInformation.Fields["G3E_PRIMARYGEOGRAPHICCNO"].Value.ToString());//get the cno for the geometry of the feature
              IGTKeyObject relatedFacility = gtDataContext.OpenFeature(g_Connecting_FNO, g_Connecting_FID);
              IGTKeyObject relatedServicePoint = gtDataContext.OpenFeature(g_Service_Point_FNO, g_Service_Point_FID);
              IGTComponent lineSymbol = addedObject.Components.GetComponent(g_ServiceLine_Linear_CNO);
              IGTComponent lineAttributes = addedObject.Components.GetComponent(g_Service_Line_Attributes_CNO);
              IGTComponent lineCommonAttributes = addedObject.Components.GetComponent(g_Common_CNO);
              //polylineGeometry.Points.Add(relatedFacility.Components.GetComponent(FacilityConnectingFeature).Geometry.FirstPoint);
              polylineGeometry.Points.Add(DetermineClosestPoint(relatedFacility.Components.GetComponent(FacilityConnectingFeature).Geometry, GetStructureGeometry(), true));
              polylineGeometry.Points.Add(relatedServicePoint.Components.GetComponent(geometryConnectingFeature).Geometry.FirstPoint);
              lineSymbol.Geometry = polylineGeometry;

              if(null != lineAttributes && null != lineAttributes.Recordset & 0 < lineAttributes.Recordset.RecordCount)
              {
                lineAttributes.Recordset.MoveFirst();
                lineAttributes.Recordset.Fields["PLACEMENT_TYPE_C"].Value = "AUTOMATED";
              }
              
              IGTComponent lineCUAttributes = addedObject.Components.GetComponent(g_CU_CNO);
              IGTComponent lineElectricalAttributes = addedObject.Components.GetComponent(g_Connectivity_CNO);

              if(null != lineCUAttributes && null != lineCUAttributes.Recordset && 0 < lineCUAttributes.Recordset.RecordCount)
              {
                lineCUAttributes.Recordset.MoveFirst();
                lineCUAttributes.Recordset.Fields["QTY_LENGTH_Q"].Value = lineElectricalAttributes.Recordset.Fields["LENGTH_GRAPHIC_Q"].Value;

                try
                {
                  lineCUAttributes.Recordset.Fields["CU_C"].Value = g_CU;
                }
                catch(Exception)
                {

                }


                lineCUAttributes.Recordset.Fields["ACTIVITY_C"].Value = "I";
                lineCUAttributes.Recordset.Fields["VINTAGE_YR"].Value = DateTime.Today.Year;
                lineCUAttributes.Recordset.Fields["UNIT_CNO"].Value = g_Service_Line_Attributes_CNO;
                lineCUAttributes.Recordset.Fields["UNIT_CID"].Value = 1;
              }

              if (null != lineCommonAttributes && null != lineCommonAttributes.Recordset && 0 < lineCommonAttributes.Recordset.RecordCount)
              {
                lineCommonAttributes.Recordset.MoveFirst();
                lineCommonAttributes.Recordset.Fields["FEATURE_STATE_C"].Value = "CLS";
              }

              if (g_CU.ToUpper().Contains("CUSTOMER"))
              {
                lineCommonAttributes.Recordset.Fields["OWNED_TYPE_C"].Value = "CUSTOMER";
              }
              else
              {
                lineCommonAttributes.Recordset.Fields["OWNED_TYPE_C"].Value = "COMPANY";
              }

              g_Service_Line_FID = addedObject.FID;
            }
            else
            {
              GetServiceLine();
              IGTKeyObject oldLine = gtDataContext.OpenFeature(g_Service_Line_FNO, g_Service_Line_FID);
              IGTComponent oldLineSymbol = oldLine.Components.GetComponent(g_ServiceLine_Linear_CNO);
              IGTComponent oldLineElectrical = oldLine.Components.GetComponent(g_Connectivity_CNO);
              IGTKeyObject addedObject = gtDataContext.NewFeature(g_Service_Line_FNO);
              SetAttributeDefaults(addedObject);
              IGTComponent lineAttributes = addedObject.Components.GetComponent(g_Service_Line_Attributes_CNO);
              IGTComponent lineCommonAttributes = addedObject.Components.GetComponent(g_Common_CNO);
              addedObject.Components.GetComponent(g_ServiceLine_Linear_CNO).Geometry = oldLineSymbol.Geometry;


              if(null != lineAttributes && null != lineAttributes.Recordset && 0 < lineAttributes.Recordset.RecordCount)
              {
                lineAttributes.Recordset.MoveFirst();
                lineAttributes.Recordset.Fields["PLACEMENT_TYPE_C"].Value = "AUTOMATED";
              }

              IGTComponent lineCUAttributes = addedObject.Components.GetComponent(g_CU_CNO);

              if(null != lineCUAttributes && null != lineCUAttributes.Recordset && 0 < lineCUAttributes.Recordset.RecordCount)
              {
                lineCUAttributes.Recordset.MoveFirst();
                try
                {
                  lineCUAttributes.Recordset.Fields["CU_C"].Value = g_CU;
                }
                catch(Exception)
                {

                }

                lineCUAttributes.Recordset.Fields["QTY_LENGTH_Q"].Value = oldLineElectrical.Recordset.Fields["LENGTH_GRAPHIC_Q"].Value;
                lineCUAttributes.Recordset.Fields["VINTAGE_YR"].Value = DateTime.Today.Year;
                lineCUAttributes.Recordset.Fields["UNIT_CNO"].Value = g_Service_Line_Attributes_CNO;
                lineCUAttributes.Recordset.Fields["UNIT_CID"].Value = 1;
                lineCUAttributes.Recordset.Fields["ACTIVITY_C"].Value = "X";
              }

              if (null != lineCommonAttributes && null != lineCommonAttributes.Recordset && 0 < lineCommonAttributes.Recordset.RecordCount)
              {
                lineCommonAttributes.Recordset.MoveFirst();
                lineCommonAttributes.Recordset.Fields["FEATURE_STATE_C"].Value = "CLS";


                if (g_CU.ToUpper() == "CUSTOMER")
                {
                  lineCommonAttributes.Recordset.Fields["OWNED_TYPE_C"].Value = "CUSTOMER";
                }
                else
                {
                  lineCommonAttributes.Recordset.Fields["OWNED_TYPE_C"].Value = "COMPANY";
                }
              }

              g_New_Line_FID = addedObject.FID;
            }
          }
          else if(geometryType.ToUpper() == "COMPOSITEPOLYLINEGEOMETRY")
          {
            if(g_Service_Info_Code == "GB" || g_Service_Info_Code == "NS")
            {
              IGTCompositePolylineGeometry polylineGeometry = GTClassFactory.Create<IGTCompositePolylineGeometry>();
            }
            else
            {

            }
          }
          else
          {
          }
        }

        status = "SUCCESS";
      }
      catch(Exception e)
      {
        message = e.Message;
        status = "FAILED";
      }
    }

    /// <summary>
    /// Determine which point of the passed in geometry is closest to the target point. 
    /// </summary>
    /// <param name="sourceGeometry">The IGTGeometry of the connecting facility</param>
    /// <param name="targetPt">The IGTPoint to use for comparison</param>
    /// <param name="returnClosestPt">Boolean indicating which point to return</param>
    /// <returns>returnClosestPt = true then return the IGTPoint closest to the target point, otherwise return the IGTPoint furthest away from the target point</returns>
    internal IGTPoint DetermineClosestPoint(IGTGeometry sourceGeometry, IGTPoint targetPt, bool returnClosestPt)
    {
      IGTPoint gtPt = sourceGeometry.FirstPoint;

      if (sourceGeometry.FirstPoint.X != sourceGeometry.LastPoint.X || sourceGeometry.FirstPoint.Y != sourceGeometry.LastPoint.Y)
      {
        double distanceX = targetPt.X - sourceGeometry.FirstPoint.X;
        double distanceY = targetPt.Y - sourceGeometry.FirstPoint.Y;
        double pt1Distance = Math.Sqrt((distanceX * distanceX) + (distanceY * distanceY));

        distanceX = targetPt.X - sourceGeometry.LastPoint.X;
        distanceY = targetPt.Y - sourceGeometry.LastPoint.Y;
        double pt2Distance = Math.Sqrt((distanceX * distanceX) + (distanceY * distanceY));

        if(pt2Distance < pt1Distance)
        {
          if(returnClosestPt)
          {
            gtPt = sourceGeometry.LastPoint;
          }
        }
        else
        {
          if (!returnClosestPt)
          {
            gtPt = sourceGeometry.LastPoint;
          }
        }
      }
      else
      {
        if(!returnClosestPt)
        {
          gtPt = sourceGeometry.LastPoint;
        }
      }

      return gtPt;
    }

    /// <summary>
    /// Determine which node of the connecting feature to use for establishing connectivity. 
    /// </summary>
    /// <param name="node1">Node_1_ID of the connecting facility</param>
    /// <param name="connectingFID">Connecting feature</param>
    /// <param name="structureFID">Owning structure</param>
    /// <returns>Relationship ordinal 1 if connected at node 1, otherwise relationship ordinal 2</returns>
    internal GTRelationshipOrdinalConstants DetermineConnectingNode(int node1, int connectingFID, int structureFID)
    {
      GTRelationshipOrdinalConstants relationshipOrdinal = GTRelationshipOrdinalConstants.gtrelRelationshipOrdinal1;

      try
      {
        // Check if any connected features owned to the located structure share the node 1 of the connecting facility
        string connectionQuery = "SELECT CONN.G3E_FID FROM CONNECTIVITY_N CONN, COMMON_N COMM, COMMON_N COWNER WHERE (CONN.NODE_1_ID = ? OR CONN.NODE_2_ID = ?) AND CONN.G3E_FID = COMM.G3E_FID " +
                                 "AND CONN.G3E_FID != ? AND (COMM.OWNER1_ID = COWNER.G3E_ID OR COMM.OWNER2_ID = COWNER.G3E_ID) AND COWNER.G3E_FID = ?";

        Recordset connectionRecords = gtDataContext.OpenRecordset(connectionQuery, CursorTypeEnum.adOpenDynamic, LockTypeEnum.adLockOptimistic, -1, node1, node1, connectingFID, structureFID);

        // If no records are found then the connecting facility shared node at the located structure is node 2.
        if (connectionRecords.RecordCount == 0)
        {
          relationshipOrdinal = GTRelationshipOrdinalConstants.gtrelRelationshipOrdinal2;
        }
      }
      catch
      {
        // use relationship ordinal 1
      }      

      return relationshipOrdinal;
    }

    /// <summary>
    /// Return the geometry for the located structure. 
    /// </summary>
    /// <param name="connectingFacilityGeometry">The IGTGeometry of the connecting facility</param>
    /// <returns>The IGTPoint closest to the located Structure</returns>
    internal IGTPoint GetStructureGeometry()
    {
      if(m_StructureGeometry == null)
      {
        Recordset metadata = gtDataContext.MetadataRecordset("G3E_FEATURES_OPTABLE", "G3E_FNO = " + g_Structure_FNO);
        metadata.MoveFirst();

        IGTKeyObject structure = gtDataContext.OpenFeature(g_Structure_FNO, g_Structure_FID);
        IGTComponent structureGeometry = structure.Components.GetComponent(Convert.ToInt16(metadata.Fields["G3E_PRIMARYGEOGRAPHICCNO"].Value));

        m_StructureGeometry = GTClassFactory.Create<IGTPoint>();
        m_StructureGeometry.X = Convert.ToDouble(structureGeometry.Geometry.FirstPoint.X);
        m_StructureGeometry.Y = Convert.ToDouble(structureGeometry.Geometry.FirstPoint.Y);
      }

      return m_StructureGeometry;
    }

    /// <summary>
    /// Updates the existing service line with data from the xml message.
    /// </summary>
    internal void UpdateServiceLine()
    {
      try
      {
        Recordset rsCU = ComponentRecordset(g_Service_Line_FNO, g_Service_Line_FID, g_CU_CNO);

        if(null != rsCU)
        {
          rsCU.MoveFirst();
          rsCU.Fields["CU_C"].Value = g_CU;
          rsCU.Fields["ACTIVITY_C"].Value = "";
          rsCU.Fields["VINTAGE_YR"].Value = DateTime.Today.Year;
        }

        Recordset rsCommon = ComponentRecordset(g_Service_Line_FNO, g_Service_Line_FID, g_Common_CNO);

        if (null != rsCommon)
        {
          rsCommon.MoveFirst();
          rsCommon.Fields["FEATURE_STATE_C"].Value = "CLS";
        }

        status = "SUCCESS";
      }
      catch(Exception e)
      {
        status = "FAILED";
        message = e.Message;
      }
    }

    /// <summary>
    /// Removes service line for a service.
    /// </summary>
    internal void RemoveServiceLine()
    {
      try
      {
        string yearStatement = "UPDATE SERVICE_ACTIVITY SET RMV_VINT_YEAR = ? WHERE SERVICE_ACTIVITY_ID = ?";

        gtDataContext.Execute(yearStatement, out int x, (int)CommandTypeEnum.adCmdText, DateTime.Today.Year, g_Transaction_Number);

        if(DeleteFeature(g_Service_Line_FID, g_Service_Line_FNO))
        {
          status = "SUCCESS";
        }
        else
        {
          status = "FAILED";
        }

      }
      catch(Exception e)
      {
        status = "FAILED";
        message = e.Message;
      }
    }

    /// <summary>
    /// Method to delete a feature. 
    /// </summary>
    /// <param name="FID"></param>
    /// <param name="FNO"></param>
    /// <returns></returns>
    internal bool DeleteFeature(int FID, short FNO)
    {
      bool retVal = false;

      try
      {

        IGTKeyObject feature = gtDataContext.OpenFeature(FNO, FID);

        if(null != feature)
        {

          // Determine the components to delete and the order in which to delete them
          Recordset comps = gtDataContext.MetadataRecordset("G3E_FEATURECOMPS_OPTABLE", "G3E_FNO = " + FNO);

          if(null != comps && 0 < comps.RecordCount)
          {
            comps.Sort = "g3e_deleteordinal asc";
            comps.MoveFirst();

            do
            {
              short CNO = Convert.ToInt16(comps.Fields["g3e_cno"].Value);
              Recordset rsDelete = ComponentRecordset(FNO, FID, CNO);

              if(null != rsDelete)
              {
                rsDelete.MoveFirst();

                do
                {
                  rsDelete.Delete(AffectEnum.adAffectCurrent);

                  if(!rsDelete.EOF)
                  {
                    rsDelete.MoveNext();
                  }
                } while(!rsDelete.EOF && 0 < rsDelete.RecordCount);
              }

              comps.MoveNext();
            } while(!comps.EOF);
          }

          retVal = true;
        }

        return retVal;
      }
      catch(Exception e)
      {
        status = "FAILED";
        message = e.Message;
        return false;
      }

    }

    /// <summary>
    /// Method to Replace a service line. 
    /// </summary>
    internal void ReplaceServiceLine()
    {
      try
      {
        //data processing for the original service line
        gtRelationshipService.DataContext = gtDataContext;
        gtRelationshipService.ActiveFeature = gtDataContext.OpenFeature(g_Service_Line_FNO, g_Service_Line_FID);
        IGTKeyObject oldLine = gtDataContext.OpenFeature(g_Service_Line_FNO, g_Service_Line_FID);

        Recordset RNORecords = gtDataContext.MetadataRecordset("G3E_RELATIONSHIPS_OPTABLE", "G3E_TYPE = 2 AND G3E_TABLE = 'G3E_NODEEDGECONN_ELEC'");
        RNORecords.MoveFirst();
        short RNO = Convert.ToInt16(RNORecords.Fields["G3E_RNO"].Value);

        IGTKeyObjects connectedFeaturesNode1 = gtRelationshipService.GetRelatedFeatures(RNO, GTRelationshipOrdinalConstants.gtrelRelationshipOrdinal1);
        IGTKeyObjects connectedFeaturesNode2 = gtRelationshipService.GetRelatedFeatures(RNO, GTRelationshipOrdinalConstants.gtrelRelationshipOrdinal2);

        Recordset connectionsToServiceLine = gtDataContext.MetadataRecordset("G3E_NODEEDGECONN_ELEC_OPTABLE", "G3E_SOURCEFNO = " + g_Service_Line_FNO);
        List<IGTKeyObject> goodConnectedFeaturesNode1 = new List<IGTKeyObject>();
        List<IGTKeyObject> goodConnectedFeaturesNode2 = new List<IGTKeyObject>();

        if(connectedFeaturesNode1.Count > 0)
        {
          if(null != connectionsToServiceLine && 0 < connectionsToServiceLine.RecordCount)
          {
            connectionsToServiceLine.MoveFirst();

            foreach(IGTKeyObject feature in connectedFeaturesNode1)
            {
              connectionsToServiceLine.MoveFirst();

              do
              {
                if(feature.FNO == Convert.ToInt16(connectionsToServiceLine.Fields["G3E_CONNECTINGFNO"].Value))
                {
                  goodConnectedFeaturesNode1.Add(feature);
                }

                connectionsToServiceLine.MoveNext();
              } while(!connectionsToServiceLine.EOF);

            }
          }
        }
        if(connectedFeaturesNode2.Count > 0)
        {
          connectingFacilityFID = connectedFeaturesNode2[0].FID;
          facilityFNO = connectedFeaturesNode2[0].FNO;

          if(connectionsToServiceLine.RecordCount > 0)
          {
            connectionsToServiceLine.MoveFirst();
          }

          if(!connectionsToServiceLine.BOF && !connectionsToServiceLine.EOF)
          {
            connectionsToServiceLine.MoveFirst();

            foreach(IGTKeyObject feature in connectedFeaturesNode2)
            {
              connectionsToServiceLine.MoveFirst();

              do
              {
                if(feature.FNO == Convert.ToInt16(connectionsToServiceLine.Fields["G3E_CONNECTINGFNO"].Value))
                {
                  goodConnectedFeaturesNode2.Add(feature);
                }

                connectionsToServiceLine.MoveNext();
              } while(!connectionsToServiceLine.EOF);
            }
          }
        }
        AddServiceLine();
        IGTKeyObject srvcLineKO = gtDataContext.OpenFeature(g_Service_Line_FNO, g_New_Line_FID);
        gtRelationshipService.ActiveFeature = srvcLineKO;

        IGTKeyObject structure = gtDataContext.OpenFeature(g_Structure_FNO, Convert.ToInt32(g_Structure_FID));

        gtRelationshipService.SilentEstablish(elecOwnedBy, structure);

        foreach(IGTKeyObject feature in goodConnectedFeaturesNode1)
        {
          if (feature.FNO == g_Transformer_OH_FNO || feature.FNO == g_Transformer_UG_FNO)
          {
            gtRelationshipService.SilentEstablish(RNO, feature, GTRelationshipOrdinalConstants.gtrelRelationshipOrdinal1, GTRelationshipOrdinalConstants.gtrelRelationshipOrdinal2);
          }
          else
          {
            GTRelationshipOrdinalConstants connectingRelationshipOrdinal = GTRelationshipOrdinalConstants.gtrelRelationshipOrdinal1;
            Recordset connectingConnRS = feature.Components.GetComponent(g_Connectivity_CNO).Recordset;
            if (connectingConnRS.RecordCount > 0)
            {
              connectingConnRS.MoveFirst();
              int node1 = Convert.ToInt32(connectingConnRS.Fields["NODE_1_ID"].Value);
              connectingRelationshipOrdinal = DetermineConnectingNode(node1, feature.FID, g_Structure_FID);
            }
            gtRelationshipService.SilentEstablish(RNO, feature, GTRelationshipOrdinalConstants.gtrelRelationshipOrdinal1, connectingRelationshipOrdinal);
          }
          break;
        }

        foreach(IGTKeyObject feature in goodConnectedFeaturesNode2)
        {
          gtRelationshipService.ActiveFeature = feature;
          gtRelationshipService.SilentEstablish(RNO, srvcLineKO, GTRelationshipOrdinalConstants.gtrelRelationshipOrdinal1, GTRelationshipOrdinalConstants.gtrelRelationshipOrdinal2);
          break;
        }

        if(status == "SUCCESS")
        {
          RemoveServiceLine();
        }

      }
      catch(Exception e)
      {
        status = "FAILED";
        message = e.Message;
      }
    }

    /// <summary>
    /// Gets the owning structure of a given feature based on the FID
    /// </summary>
    /// <param name="G3E_FID"></param>
    internal void GetOwningStructureFID(int G3E_FID)
    {
      string getOwningFID = "SELECT COMM2.G3E_FID, COMM2.G3E_ID, COMM2.G3E_FNO FROM COMMON_N COMM1, COMMON_N COMM2 WHERE COMM1.G3E_FID = ? AND COMM1.OWNER1_ID = COMM2.G3E_ID";
      Recordset owningFIDRecords = gtDataContext.OpenRecordset(getOwningFID, CursorTypeEnum.adOpenStatic, LockTypeEnum.adLockReadOnly, (int)CommandTypeEnum.adCmdText, G3E_FID);

      if(null != owningFIDRecords && 0 < owningFIDRecords.RecordCount)
      {
        owningFIDRecords.MoveFirst();

        if(owningFIDRecords.RecordCount > 1)
        {
          status = "FAILED";
          message = "DUPLICATE OWNING STRUCTURE";
        }
        else
        {
          if(owningFIDRecords.EOF && owningFIDRecords.BOF)
          {
            status = "FAILED";
            message = "NO MATCHING OWNING STRUCTURE";
          }
          else
          {
            owningFIDRecords.MoveFirst();
            if(owningFIDRecords.RecordCount > 1)
            {
              status = "FAILED";
              message = "DUPLICATE OWNING STRUCTURE";
            }
            else
            {
              status = "SUCCESS";
              g_OwningStructureFID = Convert.ToInt32(owningFIDRecords.Fields["G3E_FID"].Value);
              g_OwningStructureFNO = Convert.ToInt16(owningFIDRecords.Fields["G3E_FNO"].Value);
            }
          }
        }
      }
      else
      {
        status = "FAILED";
        message = "NO MATCHING OWNING STRUCTURE";
      }
    }

    /// <summary>
    /// Updates the structure ID based on the xml message.
    /// </summary>
    internal void UpdateStructureID()
    {
      try
      {
        Recordset rsCommon = ComponentRecordset(g_Structure_FNO, g_Structure_FID, g_Common_CNO);

        if(null != rsCommon)
        {
          rsCommon.Fields["STRUCTURE_ID"].Value = g_Structure_ID;
        }

        status = "SUCCESS";
      }
      catch(Exception e)
      {
        status = "FAILED";
        message = e.Message;
      }
    }

    /// <summary>
    /// Method to find the connecting facility.
    /// </summary>
    internal void GetConnectingFacility()
    {
      // Get the transformers and secondary conductors that are owned to the located structure
      string structureFacilitiesQuery = "SELECT COMM.G3E_FID, COMM.G3E_FNO FROM COMMON_N COMM, COMMON_N COWNER WHERE COMM.FEATURE_STATE_C IN ('PPI', 'ABI', 'INI', 'CLS') AND (COMM.OWNER1_ID = COWNER.G3E_ID OR COMM.OWNER2_ID = COWNER.G3E_ID)" +
          " AND COWNER.G3E_FID = ? AND (COMM.G3E_FNO = 59 OR COMM.G3E_FNO = 60 OR COMM.G3E_FNO = 53 OR COMM.G3E_FNO = 63)";
      Recordset structureFacilitesRS = gtDataContext.OpenRecordset(structureFacilitiesQuery, CursorTypeEnum.adOpenDynamic, LockTypeEnum.adLockOptimistic, -1, g_Structure_FID);
      
      if(!structureFacilitesRS.EOF && !structureFacilitesRS.BOF)
      {
        structureFacilitesRS.MoveFirst();
        if(structureFacilitesRS.RecordCount == 1)
        {
          g_Connecting_FNO = Convert.ToInt16(structureFacilitesRS.Fields["G3E_FNO"].Value);
          g_Connecting_FID = Convert.ToInt32(structureFacilitesRS.Fields["G3E_FID"].Value);
          status = "SUCCESS";
        }
        else
        {
          structureFacilitesRS.Filter = "g3e_fno = 59 or g3e_fno = 60";
          if (g_Transformer_Number.Length > 0 && structureFacilitesRS.RecordCount > 0)
          {
            // Get the transformers matching the transaction company number and owned to the located structure
            string xfmrQuery = "SELECT COMM.G3E_FID, COMM.G3E_FNO FROM COMMON_N COWNER, COMMON_N COMM LEFT JOIN XFMR_OH_UNIT_N XOH ON COMM.G3E_FID = XOH.G3E_FID LEFT JOIN XFMR_UG_UNIT_N XUG ON COMM.G3E_FID = XUG.G3E_FID " +
            "WHERE COMM.FEATURE_STATE_C IN ('PPI', 'ABI', 'INI', 'CLS') AND COMM.OWNER1_ID = COWNER.G3E_ID AND COWNER.G3E_FID = ? AND " +
            "(XOH.COMPANY_ID = ? OR XUG.COMPANY_ID = ? )";
            Recordset xfmrRecords = gtDataContext.OpenRecordset(xfmrQuery, CursorTypeEnum.adOpenDynamic, LockTypeEnum.adLockOptimistic, -1, g_Structure_FID, g_Transformer_Number, g_Transformer_Number);

            if (xfmrRecords.BOF && xfmrRecords.EOF)
            {
              status = "FAILED";
              message = "UNABLE TO DETERMINE CONNECTING FACILITY";
            }
            else
            {
              xfmrRecords.MoveFirst();
              if (xfmrRecords.RecordCount == 1)
              {
                g_Connecting_FID = Convert.ToInt32(xfmrRecords.Fields["G3E_FID"].Value);
                g_Connecting_FNO = Convert.ToInt16(xfmrRecords.Fields["G3E_FNO"].Value);
                status = "SUCCESS";
              }
              else
              {
                status = "FAILED";
                message = "MULTIPLE MATCHING TRANSFORMERS AT LOCATION";
              }
            }
          }
          else
          {
            structureFacilitesRS.Filter = "";
            structureFacilitesRS.MoveFirst();
            Int16 tempConnFNO = 0;
            Int32 tempConnFID = 0;
            List<string> FIDS = new List<string>();
            for (int i = 0; i < structureFacilitesRS.RecordCount; i++)
            {
              FIDS.Add(Convert.ToString(structureFacilitesRS.Fields["G3E_FID"].Value));
              if (Convert.ToInt16(structureFacilitesRS.Fields["G3E_FNO"].Value) == g_Transformer_OH_FNO || Convert.ToInt16(structureFacilitesRS.Fields["G3E_FNO"].Value) == g_Transformer_UG_FNO)
              {
                tempConnFNO = Convert.ToInt16(structureFacilitesRS.Fields["G3E_FNO"].Value);
                tempConnFID = Convert.ToInt32(structureFacilitesRS.Fields["G3E_FID"].Value);
              }
              structureFacilitesRS.MoveNext();
            }

            // Need to check if all FIDs returned in the query are connected at a common node.
            // If a transformer was found then use that as the FID to compare since we will want to use it as the g_connecting_facility. Otherwise, use the first FID.
            if (tempConnFID == 0)
            {
              structureFacilitesRS.MoveFirst();
              tempConnFNO = Convert.ToInt16(structureFacilitesRS.Fields["G3E_FNO"].Value);
              tempConnFID = Convert.ToInt32(structureFacilitesRS.Fields["G3E_FID"].Value);
            }

            FIDS.Remove(tempConnFID.ToString());

            string connectionQuery = "SELECT CONN1.G3E_FID FROM CONNECTIVITY_N CONN1, CONNECTIVITY_N CONN2 WHERE CONN1.G3E_FID IN (" + String.Join(" ,", FIDS.ToArray()) + ") " +
                "AND (CONN1.NODE_1_ID = CONN2.NODE_1_ID OR CONN1.NODE_1_ID = CONN2.NODE_2_ID OR CONN1.NODE_2_ID = CONN2.NODE_1_ID OR CONN1.NODE_2_ID = CONN2.NODE_2_ID) AND CONN2.G3E_FID = ?";
            Recordset connectionRecords = gtDataContext.OpenRecordset(connectionQuery, CursorTypeEnum.adOpenDynamic, LockTypeEnum.adLockOptimistic, -1, tempConnFID);
            if (connectionRecords.RecordCount == FIDS.Count)
            {
              g_Connecting_FNO = tempConnFNO;
              g_Connecting_FID = tempConnFID;
            }
            else
            {
              message = "UNABLE TO DETERMINE CONNECTING FACILITY";
              status = "FAILED";
            }
          }          
        }
      }
      else
      {
        message = "UNABLE TO DETERMINE CONNECTING FACILITY";
        status = "FAILED";
      }
    }

    /// <summary>
    /// Method to get the service line, based on xml message.
    /// </summary>
    internal void GetServiceLine()
    {
      string[] codes = { "GB", "NS" };
      string[] states = { "PPI", "ABI", "INI" };
      List<string> serviceCodes = new List<string>(codes);
      List<string> stateCodes = new List<string>(states);
      if(serviceCodes.Contains(g_Service_Info_Code))
      { 
        string lineFIDQuery = "SELECT SRVCONN.G3E_FID, OWNCOMM.G3E_FID OWNER_FID" +
                               " FROM CONNECTIVITY_N SRVCONN, CONNECTIVITY_N SRVCPTCONN," +
                               " SERVICE_LINE_N SRVC, COMMON_N COMM, COMMON_N OWNCOMM" +
                               " WHERE SRVCPTCONN.G3E_FID = ?" +
                                    " AND ((SRVCONN.NODE_1_ID = SRVCPTCONN.NODE_1_ID OR SRVCONN.NODE_2_ID = SRVCPTCONN.NODE_1_ID)" +
                                    " AND SRVCPTCONN.NODE_1_ID <> 0)" +
                                    " AND COMM.FEATURE_STATE_C IN ('PPI', 'ABI', 'INI','CLS')" +
                                    " AND SRVCONN.G3E_FNO = 54" +
                                    " AND SRVCONN.G3E_FID = SRVC.G3E_FID" +
                                    " AND SRVCONN.G3E_FID = COMM.G3E_FID" +
                                    " AND COMM.OWNER1_ID = OWNCOMM.G3E_ID";
        Recordset serviceLines = gtDataContext.OpenRecordset(lineFIDQuery, CursorTypeEnum.adOpenDynamic, LockTypeEnum.adLockOptimistic, -1, g_Service_Point_FID);
        if(!serviceLines.EOF && !serviceLines.BOF)
        {
          serviceLines.MoveFirst();
          if(serviceLines.RecordCount > 1)
          {
            status = "FAILED";
            message = "SERVICE LINE CANNOT BE DETERMINED";
          }
          else//ADD CUSTOMER OWNED QUERY
          {
            if(Convert.ToInt32(serviceLines.Fields["OWNER_FID"].Value) != g_Structure_FID && g_Service_Info_Code != "ES")
            {
              status = "FAILED";
              message = "INVALID STRUCTURE";
            }
            else
            {
              status = "SUCCESS";
              g_Service_Line_FID = Convert.ToInt32(serviceLines.Fields["G3E_FID"].Value);
            }
          }

        }
        else
        {
          status = "FAILED";
          message = "SERVICE LINE CANNOT BE DETERMINED";
        }
      }
      else if(g_Service_Info_Code == "RP")
      {
        string lineFIDQuery = "SELECT SRVCONN.G3E_FID, OWNCOMM.G3E_FID AS OWNER_FID, COMM.FEATURE_STATE_C FROM CONNECTIVITY_N SRVCONN, CONNECTIVITY_N SRVCPTCONN, SERVICE_LINE_N SRVC, COMMON_N COMM, COMMON_N OWNCOMM WHERE SRVCPTCONN.G3E_FID = ? " +
            " AND (SRVCONN.NODE_1_ID = SRVCPTCONN.NODE_1_ID OR SRVCONN.NODE_2_ID = SRVCPTCONN.NODE_1_ID) AND COMM.FEATURE_STATE_C IN ('INI', 'CLS', 'PPI', 'ABI', 'PPR', 'ABR', 'OSR') AND SRVCONN.G3E_FNO = 54 AND SRVCONN.G3E_FID = SRVC.G3E_FID " +
            "AND SRVCONN.G3E_FID = COMM.G3E_FID AND COMM.OWNER1_ID = OWNCOMM.G3E_ID";
        Recordset serviceLines = gtDataContext.OpenRecordset(lineFIDQuery, CursorTypeEnum.adOpenDynamic, LockTypeEnum.adLockOptimistic, -1, g_Service_Point_FID);
        if(!serviceLines.EOF && !serviceLines.BOF)
        {
          serviceLines.MoveFirst();
          string featureState1 = serviceLines.Fields["FEATURE_STATE_C"].Value.ToString();
          if (serviceLines.RecordCount == 1)
          {
            if(Convert.ToInt32(serviceLines.Fields["OWNER_FID"].Value) != g_Structure_FID)
            {
              status = "FAILED";
              message = "INVALID STRUCTURE";
            }
            else
            {
              if(featureState1 == "INI" || featureState1 == "CLS")
              {
                status = "SUCCESS";
                g_Service_Line_FID = Convert.ToInt32(serviceLines.Fields["G3E_FID"].Value);
              }
              else
              {
                status = "FAILED";
                message = "SERVICE LINE CANNOT BE DETERMINED";
              }
            }
          }
          else if (serviceLines.RecordCount == 2)
          {
            serviceLines.MoveNext();
            string featureState2 = serviceLines.Fields["FEATURE_STATE_C"].Value.ToString();
            if (((featureState1 == "PPI" || featureState1 == "ABI" || featureState1 == "INI") && (featureState2 == "PPR" || featureState2 == "ABR" || featureState2 == "OSR")) ||
                ((featureState2 == "PPI" || featureState2 == "ABI" || featureState2 == "INI") && (featureState1 == "PPR" || featureState1 == "ABR" || featureState1 == "OSR")))
            {
              status = "WARNING";
              message = "WR Estimated";
            }
            else
            {
              status = "FAILED";
              message = "SERVICE LINE CANNOT BE DETERMINED";
            }
          }
          else
          {
            status = "FAILED";
            message = "SERVICE LINE CANNOT BE DETERMINED";
          }
        }
        else
        {
          status = "FAILED";
          message = "SERVICE LINE CANNOT BE DETERMINED";
        }
      }
      else if(g_Service_Info_Code == "RM")
      {
        string lineFIDQuery = "SELECT SRVCONN.G3E_FID, OWNCOMM.G3E_FID AS OWNER_FID, COMM.FEATURE_STATE_C FROM CONNECTIVITY_N SRVCONN, CONNECTIVITY_N SRVCPTCONN, SERVICE_LINE_N SRVC, COMMON_N COMM, COMMON_N OWNCOMM WHERE SRVCPTCONN.G3E_FID = ?" +
            " AND (SRVCONN.NODE_1_ID = SRVCPTCONN.NODE_1_ID OR SRVCONN.NODE_2_ID = SRVCPTCONN.NODE_1_ID) AND SRVCPTCONN.NODE_1_ID <> 0 AND COMM.FEATURE_STATE_C IN ('INI', 'CLS', 'PPR', 'ABR', 'OSR') AND SRVCONN.G3E_FNO = 54 AND SRVCONN.G3E_FID = SRVC.G3E_FID " +
            "AND SRVCONN.G3E_FID = COMM.G3E_FID AND COMM.OWNER1_ID = OWNCOMM.G3E_ID";
        Recordset serviceLines = gtDataContext.OpenRecordset(lineFIDQuery, CursorTypeEnum.adOpenDynamic, LockTypeEnum.adLockOptimistic, -1, g_Service_Point_FID);
        if(!serviceLines.EOF && !serviceLines.BOF)
        {
          serviceLines.MoveFirst();
          if(serviceLines.RecordCount == 1)
          {
            if(Convert.ToInt32(serviceLines.Fields["OWNER_FID"].Value) != g_Structure_FID)
            {
              status = "FAILED";
              message = "INVALID STRUCTURE";
            }
            else
            {
              status = "SUCCESS";
              g_Service_Line_FID = Convert.ToInt32(serviceLines.Fields["G3E_FID"].Value);
              g_Service_Line_State = serviceLines.Fields["FEATURE_STATE_C"].Value.ToString();
            }
          }
          else if(serviceLines.RecordCount == 0)
          {
            status = "SUCCESS";
          }
          else
          {
            status = "FAILED";
            message = "SERVICE LINE CANNOT BE DETERMINED";
          }
        }
        else
        {
          status = "SUCCESS";
          //message = "SERVICE LINE CANNOT BE DETERMINED";
        }
      }
      else
      {
        status = "FAILED";
        message = "SERVICE LINE CANNOT BE DETERMINED";
      }
    }

    /// <summary>
    /// Locates a service point by the ESI location of a premise.
    /// </summary>
    /// <param name="ESILocation"></param>
    public void LocateSrvcPtByESILocation(string ESILocation)
    {
      try
      {
        string servicePointQuery = "SELECT COMM.G3E_FID, PREM.G3E_CID, COMM.FEATURE_STATE_C" +
                                     " FROM COMMON_N COMM, PREMISE_N PREM" +
                                     " WHERE COMM.FEATURE_STATE_C IN ('PPI', 'ABI', 'INI', 'CLS')" +
                                       " AND PREM.PREMISE_NBR = ? AND COMM.G3E_FID = PREM.G3E_FID";

        if (g_Service_Info_Code == "RM")
        {
          servicePointQuery = "SELECT COMM.G3E_FID, PREM.G3E_CID, COMM.FEATURE_STATE_C" +
                                       " FROM COMMON_N COMM, PREMISE_N PREM" +
                                       " WHERE COMM.FEATURE_STATE_C IN ('PPI', 'ABI', 'INI', 'CLS', 'PPR', 'ABR', 'OSR')" +
                                         " AND PREM.PREMISE_NBR = ? AND COMM.G3E_FID = PREM.G3E_FID";
        }

        Recordset servicePoints = gtDataContext.OpenRecordset(servicePointQuery, CursorTypeEnum.adOpenDynamic, LockTypeEnum.adLockOptimistic, -1, ESILocation);
        if(servicePoints.EOF && servicePoints.BOF)
        {
          g_Service_Point_FID = 0;
          status = "SUCCESS";
        }
        else
        {
          servicePoints.MoveFirst();
          if(servicePoints.RecordCount > 1)
          {
            message = "DUPLICATE ESI LOCATION";
            status = "FAILED";
          }
          else
          {
            if(locateType == "ESI")
            {
              g_Service_Point_FID = Convert.ToInt32(servicePoints.Fields["G3E_FID"].Value);
              g_Service_Point_CID = Convert.ToInt32(servicePoints.Fields["G3E_CID"].Value);
              g_Service_Point_State = servicePoints.Fields["FEATURE_STATE_C"].Value.ToString();
              status = "SUCCESS";
            }
            else
            {
              string gangBaseQuery = "SELECT PREMISE_NBR, G3E_CID, HOUSE_NBR, STREET_NM, UNIT_NBR FROM PREMISE_N WHERE  G3E_FID = ? AND PREMISE_NBR IS NULL";
              Recordset gangBasePremises = gtDataContext.OpenRecordset(gangBaseQuery, CursorTypeEnum.adOpenDynamic, LockTypeEnum.adLockOptimistic, -1, Convert.ToInt32(servicePoints.Fields["G3E_FID"].Value));

              if(gangBasePremises.BOF && gangBasePremises.EOF)
              {
                g_Service_Point_FID = Convert.ToInt32(servicePoints.Fields["G3E_FID"].Value);
                status = "SUCCESS";
              }
              else
              {
                gangBasePremises.MoveFirst();
                if(gangBasePremises.RecordCount == 1)
                {
                  g_Service_Point_FID = Convert.ToInt32(servicePoints.Fields["G3E_FID"].Value);
                  g_Service_Point_CID = Convert.ToInt32(gangBasePremises.Fields["G3E_CID"].Value);
                  status = "SUCCESS";
                }
                else
                {
                  int masterMatchedParameters = -1;
                  while(!gangBasePremises.EOF)
                  {
                    int matchedParameters = 0;
                    if(g_House_Number == gangBasePremises.Fields["HOUSE_NBR"].Value.ToString())
                    {
                      matchedParameters++;
                    }
                    if(g_Street_Name == gangBasePremises.Fields["STREET_NM"].Value.ToString())
                    {
                      matchedParameters++;
                    }
                    if(g_Unit == gangBasePremises.Fields["UNIT_NBR"].Value.ToString())
                    {
                      matchedParameters++;
                    }

                    if(matchedParameters >= masterMatchedParameters)
                    {
                      masterMatchedParameters = matchedParameters;
                      g_Service_Point_FID = Convert.ToInt32(gangBasePremises.Fields["G3E_FID"].Value);
                      g_Service_Point_CID = Convert.ToInt32(gangBasePremises.Fields["G3E_CID"].Value);
                      status = "SUCCESS";
                    }
                    gangBasePremises.MoveNext();
                  }
                }
              }
            }
          }
        }

      }
      catch(Exception e)
      {
        message = e.Message;
        status = "FAILED";
      }
    }
    /// <summary>
    /// Locates a service point by Address, based on the xml message. 
    /// </summary>
    public void LocateSrvcPtByAddress()
    {
      string servicePointQuery = "SELECT SP.G3E_FID, SP.GANG_METER_YN, PM.G3E_CID, PM.PREMISE_NBR FROM SERVICE_POINT_N SP, COMMON_N COMMSP, COMMON_N COMMST, COMMON_N COMMOF, CONNECTIVITY_N CONNSP, CONNECTIVITY_N CONNOF, PREMISE_N PM" +
          " WHERE SP.G3E_FID = PM.G3E_FID AND SP.G3E_FID = COMMSP.G3E_FID AND COMMSP.FEATURE_STATE_C IN('PPI', 'ABI', 'INI', 'CLS') AND COMMST.G3E_FID = ? AND(COMMOF.OWNER1_ID = COMMST.G3E_ID OR COMMOF.OWNER2_ID = COMMST.G3E_ID)" +
          " AND COMMOF.G3E_FID = CONNOF.G3E_FID AND COMMSP.G3E_FID = CONNSP.G3E_FID AND(CONNOF.NODE_1_ID = CONNSP.NODE_1_ID OR CONNOF.NODE_2_ID = CONNSP.NODE_1_ID) AND CONNSP.NODE_1_ID <> 0 AND PM.HOUSE_NBR = ? AND PM.STREET_NM = ?" +
          " AND PM.UNIT_NBR = ?";
      Recordset servicePoints = gtDataContext.OpenRecordset(servicePointQuery, CursorTypeEnum.adOpenDynamic, LockTypeEnum.adLockOptimistic, -1, g_Structure_FID, g_House_Number, g_Street_Name, g_Unit);
      if(servicePoints.EOF && servicePoints.BOF)
      {
        servicePointQuery = "SELECT SP.G3E_FID, SP.GANG_METER_YN, PM.G3E_CID, PM.PREMISE_NBR FROM SERVICE_POINT_N SP, COMMON_N COMMSP, COMMON_N COMMST, COMMON_N COMMOF, CONNECTIVITY_N CONNSP, CONNECTIVITY_N CONNOF, PREMISE_N PM" +
            " WHERE SP.G3E_FID = PM.G3E_FID AND SP.G3E_FID = COMMSP.G3E_FID AND COMMSP.FEATURE_STATE_C IN('PPI', 'ABI', 'INI', 'CLS') AND COMMST.G3E_FID = ? AND(COMMOF.OWNER1_ID = COMMST.G3E_ID OR COMMOF.OWNER2_ID = COMMST.G3E_ID)" +
            " AND COMMOF.G3E_FID = CONNOF.G3E_FID AND COMMSP.G3E_FID = CONNSP.G3E_FID AND(CONNOF.NODE_1_ID = CONNSP.NODE_1_ID OR CONNOF.NODE_2_ID = CONNSP.NODE_1_ID) AND CONNSP.NODE_1_ID <> 0 AND PM.HOUSE_NBR = ? AND PM.STREET_NM = ?";
        servicePoints = gtDataContext.OpenRecordset(servicePointQuery, CursorTypeEnum.adOpenDynamic, LockTypeEnum.adLockOptimistic, -1, g_Structure_FID, g_House_Number, g_Street_Name);
      }

      if(servicePoints.EOF && servicePoints.BOF)
      {
        servicePointQuery = "SELECT SP.G3E_FID, SP.GANG_METER_YN, PM.G3E_CID, PM.PREMISE_NBR FROM SERVICE_POINT_N SP, COMMON_N COMMSP, COMMON_N COMMST, COMMON_N COMMOF, CONNECTIVITY_N CONNSP, CONNECTIVITY_N CONNOF, PREMISE_N PM" +
            " WHERE SP.G3E_FID = PM.G3E_FID AND SP.G3E_FID = COMMSP.G3E_FID AND COMMSP.FEATURE_STATE_C IN('PPI', 'ABI', 'INI', 'CLS') AND COMMST.G3E_FID = ? AND(COMMOF.OWNER1_ID = COMMST.G3E_ID OR COMMOF.OWNER2_ID = COMMST.G3E_ID)" +
            " AND COMMOF.G3E_FID = CONNOF.G3E_FID AND COMMSP.G3E_FID = CONNSP.G3E_FID AND(CONNOF.NODE_1_ID = CONNSP.NODE_1_ID OR CONNOF.NODE_2_ID = CONNSP.NODE_1_ID) AND CONNSP.NODE_1_ID <> 0 AND PM.HOUSE_NBR = ?";
        servicePoints = gtDataContext.OpenRecordset(servicePointQuery, CursorTypeEnum.adOpenDynamic, LockTypeEnum.adLockOptimistic, -1, g_Structure_FID, g_House_Number);
      }

      if(servicePoints.EOF && servicePoints.BOF)
      {
        g_Service_Point_FID = 0;
        status = "SUCCESS";
      }
      else
      {
        servicePoints.MoveFirst();
        if(servicePoints.RecordCount == 1)
        {
          if(servicePoints.Fields["PREMISE_NBR"].Value.ToString().Length > 0)
          {
            if((g_Service_Info_Code == "NS" || g_Service_Info_Code == "GB") || (g_Service_Info_Code == "MO" && servicePoints.Fields["GANG_METER_YN"].Value.ToString() != "Y"))
            {
              message = "ADDRESS LOCATE - ESI LOCATION EXISTS ON SINGLE PREMISE NON-GANG METER";
              status = "FAILED";
            }
            else
            {
              g_Service_Point_FID = Convert.ToInt32(servicePoints.Fields["G3E_FID"].Value);
              status = "SUCCESS";
            }
          }
          else if ((g_Service_Info_Code == "NS" || g_Service_Info_Code == "GB") || (g_Service_Info_Code == "MO" && servicePoints.Fields["GANG_METER_YN"].Value.ToString() == "Y"))
          {
            g_Service_Point_FID = Convert.ToInt32(servicePoints.Fields["G3E_FID"].Value);
            g_Service_Point_CID = Convert.ToInt32(servicePoints.Fields["G3E_CID"].Value);
            status = "SUCCESS";
          }
          else
          {
            message = "ADDRESS LOCATE - ESI LOCATION NOT FOUND ON SINGLE PREMISE NON-GANG METER";
            status = "FAILED";
          }
        }
        else
        {
          DataTable dtList = new DataTable();
          odaListMaker.Fill(dtList, servicePoints);
          List<int> FidList = new List<int>();
          List<string> ESIList = new List<string>();
          DataRow noESI = null;

          foreach(DataRow row in dtList.Rows)
          {
            if(!FidList.Contains(Convert.ToInt32(row.Field<decimal>("G3E_FID"))))
            {
              FidList.Add(Convert.ToInt32(row.Field<decimal>("G3E_FID")));              
            }
            if(row.Field<string>("PREMISE_NBR") != null)
            {
              ESIList.Add(row.Field<string>("PREMISE_NBR"));
            }
            else
            {
              noESI = row;
            }
          }
          if(FidList.Count == 1)
          {
            if(g_Service_Info_Code == "NS" || g_Service_Info_Code == "GB")
            {
              if(ESIList.Count == 0)
              {
                g_Service_Point_FID = Convert.ToInt32(noESI.Field<decimal>("G3E_FID"));
                g_Service_Point_CID = Convert.ToInt32(noESI.Field<decimal>("G3E_CID"));
                status = "SUCCESS";
              }
              else
              {
                message = "ADDRESS LOCATE - ESI LOCATION EXISTS ON MULTIPLE PREMISE NON-GANG METER";
                //message = "ADDRESS LOCATE - ESI LOCATION NOT FOUND ON MULTIPLE PREMISE NON-GANG METER";
                status = "FAILED";
              }
            }
            else
            {
              if (ESIList.Count == servicePoints.RecordCount)
              {
                if (servicePoints.Fields["GANG_METER_YN"].Value.ToString() == "Y")
                {
                  g_Service_Point_FID = Convert.ToInt32(servicePoints.Fields["G3E_FID"].Value);
                  status = "SUCCESS";
                }
                else
                {
                  message = "ADDRESS LOCATE - ESI LOCATION EXISTS ON MULTIPLE PREMISE NON-GANG METER";
                  status = "FAILED";
                }
              }
              else
              {
                if (servicePoints.Fields["GANG_METER_YN"].Value.ToString() == "Y")
                {
                  g_Service_Point_FID = Convert.ToInt32(noESI.Field<decimal>("G3E_FID"));
                  g_Service_Point_CID = Convert.ToInt32(noESI.Field<decimal>("G3E_CID"));
                  status = "SUCCESS";
                }
                else
                {
                  message = "ADDRESS LOCATE - ESI LOCATION EXISTS ON MULTIPLE PREMISE NON-GANG METER";
                  status = "FAILED";
                }
              }
            }
          }
          else
          {
            DataRow[] gangRecords = dtList.Select("GANG_METER_YN = 'Y'");            
            if(gangRecords.Length == 0 || ((g_Service_Info_Code == "NS" || g_Service_Info_Code == "GB") && ESIList.Count == servicePoints.RecordCount))
            {
              message = "ADDRESS LOCATE - MULTIPLE NON-GANG METER SERVICE POINTS FOUND";
              status = "FAILED";
            }
            else
            {
              int numWithESI = 0;
              int LeastNoESI = 1000;
              int index = 0;
              int CIDstore = 0;
              int CID = 0;

              DataTable gangRecordsTable = gangRecords.CopyToDataTable();
              DataView uniqueMaker = new DataView(gangRecordsTable);
              DataTable FIDS = uniqueMaker.ToTable(true, "G3E_FID");
              foreach(DataRow row in FIDS.Rows)
              {
                DataRow[] servicePremise = dtList.Select("G3E_FID = " + row.Field<decimal>("G3E_FID"));
                foreach(DataRow premise in servicePremise)
                {
                  if(premise.Field<string>("PREMISE_NBR") != null)
                  {                    
                    numWithESI++;
                  }
                  else
                  {
                    CIDstore = Convert.ToInt32(premise.Field<decimal>("G3E_CID"));
                  }
                }
                if(LeastNoESI > numWithESI && CIDstore != 0)
                {
                  LeastNoESI = numWithESI;
                  index = Convert.ToInt32(row.Field<decimal>("G3E_FID"));
                  CID = CIDstore;
                }
                CIDstore = 0;
                numWithESI = 0;

              }
              if(index != 0 && CID != 0)
              {
                g_Service_Point_FID = index;
                g_Service_Point_CID = CID;
                status = "SUCCESS";
              }
              else
              {
                g_Service_Point_FID = Convert.ToInt32(gangRecords[0].Field<decimal>("G3E_FID"));
              }
            }
          }
        }
      }
    }

    public void LocateSrvcPtByMeterGeocode()
    {
      string servicePointQuery = "SELECT COMMSP.G3E_FID FROM COMMON_N COMMSP, COMMON_N COMMOTHER, COMMON_N COMMSTRUCT, CONNECTIVITY_N CONNSP, CONNECTIVITY_N CONNOTHER WHERE COMMSP.FEATURE_STATE_C IN('PPI', 'ABI', 'INI', 'CLS') AND " +
           "COMMSP.G3E_FID = CONNSP.G3E_FID AND CONNOTHER.G3E_FID = COMMOTHER.G3E_FID AND(CONNOTHER.NODE_1_ID = CONNSP.NODE_1_ID OR CONNOTHER.NODE_2_ID = CONNSP.NODE_1_ID) " +
           "AND COMMSP.G3E_FNO = ? AND NOT EXISTS(SELECT * FROM PREMISE_N WHERE G3E_FID = COMMSP.G3E_FID) AND COMMOTHER.OWNER1_ID = COMMSTRUCT.G3E_ID AND COMMSTRUCT.G3E_FID = ?";
      Recordset servicePoints = gtDataContext.OpenRecordset(servicePointQuery, CursorTypeEnum.adOpenStatic, LockTypeEnum.adLockReadOnly, (int)CommandTypeEnum.adCmdText, g_Service_Point_FNO, g_Structure_FID);

      if(null != servicePoints && 0 < servicePoints.RecordCount)
      {
        servicePoints.MoveFirst();
        CheckMeterGeocodeLocation();

        int? geocodeToleranceOH = null;
        int? geocodeToleranceUG = null;

        string tmp = GeneralParameter("GISAUTO_SERVICELINE", "TOLERANCE", "GEOCODE_TOLERANCE_OH");

        if(!string.IsNullOrEmpty(tmp))
        {
          geocodeToleranceOH = Convert.ToInt32(tmp);
        }
        else
        {
          status = "FAILED";
          message = "GEOCODE_TOLERANCE_OH missing from SYS_GENERALPARAMETER table.";
        }

        tmp = GeneralParameter("GISAUTO_SERVICELINE", "TOLERANCE", "GEOCODE_TOLERANCE_UG");

        if(!string.IsNullOrEmpty(tmp))
        {
          geocodeToleranceUG = Convert.ToInt32(tmp);
        }
        else
        {
          status = "FAILED";
          message = "GEOCODE_TOLERANCE_UG missing from SYS_GENERALPARAMETER table.";
        }

        IGTPoint geocodeXAndY = gCoordinateConvert(Double.Parse(g_Meter_Longitude), Double.Parse(g_Meter_Latitude), 0);

        if(servicePoints.RecordCount == 1)
        {
          if(geocodeToleranceOH != null && geocodeToleranceUG != null)
          {
            Recordset metadata = gtDataContext.MetadataRecordset("G3E_FEATURES_OPTABLE", "G3E_FNO = " + g_Service_Point_FNO);
            IGTKeyObject servicePointKO = gtDataContext.OpenFeature(g_Service_Point_FNO, Convert.ToInt32(servicePoints.Fields["G3E_FID"].Value));
            IGTComponent servicePointGeometry = servicePointKO.Components.GetComponent(Convert.ToInt16(metadata.Fields["G3E_PRIMARYGEOGRAPHICCNO"].Value));

            IGTPoint servicePointPt = GTClassFactory.Create<IGTPoint>();
            servicePointPt.X = Convert.ToDouble(servicePointGeometry.Geometry.FirstPoint.X);
            servicePointPt.Y = Convert.ToDouble(servicePointGeometry.Geometry.FirstPoint.Y);

            // Calculate the distance between the located service point and the located structure
            double distanceCalculationX = servicePointPt.X - GetStructureGeometry().X;
            double distanceCalculationY = servicePointPt.Y - GetStructureGeometry().Y;
            double distanceCalculation = Math.Sqrt((distanceCalculationX * distanceCalculationX) + (distanceCalculationY * distanceCalculationY)) * 3.2808399;

            if(g_Trans_Type == "O" && distanceCalculation <= geocodeToleranceOH)
            {
              g_Service_Point_FID = Convert.ToInt32(servicePoints.Fields["G3E_FID"].Value);
              status = "SUCCESS";
            }
            else if(g_Trans_Type == "U" && distanceCalculation <= geocodeToleranceUG)
            {
              g_Service_Point_FID = Convert.ToInt32(servicePoints.Fields["G3E_FID"].Value);
              status = "SUCCESS";
            }
            else
            {
              status = "SUCCESS";
              g_Service_Point_FID = 0;
            }
          }
        }
        else // here we want to go through the function and find the service point that is closest to the data point and within the geocode tolerance.
        {
          if (geocodeToleranceOH != null && geocodeToleranceUG != null)
          {
            double geocodeTolerance;
            if (g_Trans_Type == "O")
            {
              geocodeTolerance = Convert.ToDouble(geocodeToleranceOH);
            }
            else
            {
              geocodeTolerance = Convert.ToDouble(geocodeToleranceUG);
            }

            double smallestDistance = geocodeTolerance;

            do
            {
              Recordset metadata = gtDataContext.MetadataRecordset("G3E_FEATURES_OPTABLE", "G3E_FNO = " + g_Service_Point_FNO);
              IGTKeyObject servicePointKO = gtDataContext.OpenFeature(g_Service_Point_FNO, Convert.ToInt32(servicePoints.Fields["G3E_FID"].Value));
              IGTComponent servicePointGeometry = servicePointKO.Components.GetComponent(Convert.ToInt16(metadata.Fields["G3E_PRIMARYGEOGRAPHICCNO"].Value));

              IGTPoint servicePointPt = GTClassFactory.Create<IGTPoint>();
              servicePointPt.X = Convert.ToDouble(servicePointGeometry.Geometry.FirstPoint.X);
              servicePointPt.Y = Convert.ToDouble(servicePointGeometry.Geometry.FirstPoint.Y);

              // Calculate the distance between the located service point and the transactions converted lat/lon values.
              double distanceCalculationX = servicePointPt.X - geocodeXAndY.X;
              double distanceCalculationY = servicePointPt.Y - geocodeXAndY.Y;
              double testDistance = Math.Sqrt((distanceCalculationX * distanceCalculationX) + (distanceCalculationY * distanceCalculationY)) * 3.2808399;

              // Calculate the distance between the located service point and the located structure
              double structDistanceX = servicePointPt.X - GetStructureGeometry().X;
              double structDistanceY = servicePointPt.Y - GetStructureGeometry().Y;
              double structDistance = Math.Sqrt((structDistanceX * structDistanceX) + (structDistanceY * structDistanceY)) * 3.2808399;

              if (testDistance <= smallestDistance && structDistance <= geocodeTolerance)
              {
                smallestDistance = testDistance;
                g_Service_Point_FID = Convert.ToInt32(servicePoints.Fields["G3E_FID"].Value);
              }
              servicePoints.MoveNext();
            } while (!servicePoints.EOF);

            status = "SUCCESS";
          }          
        }
      }
    }

    public void LocateServicePoint()
    {
      locateType = "ESI";
      LocateSrvcPtByESILocation(g_ESI_Location);
      if(g_Service_Point_FID != 0)
      {
        // add gis locate method variable to globals
        g_Locate_Method = "ESI LOCATION";
      }
      else if(g_Service_Info_Code != "GB" && g_Service_Info_Code != "NS" && g_Service_Info_Code != "MO")
      {
        message = "UNABLE TO LOCATE SERVICE POINT";
        status = "FAILED";
      }
      else
      {
        if(status != "FAILED")
        {
          if(g_Exist_Prem_Gangbase != null)
          {
            locateType = "GANG BASE";
            LocateSrvcPtByESILocation(g_Exist_Prem_Gangbase);
          }
          if(g_Service_Point_FID != 0)
          {
            g_Locate_Method = "GANG ESI LOCATION";
          }
          else
          {
            if(status != "FAILED")
            {
              LocateSrvcPtByAddress();
              if(status != "FAILED")
              {
                if(g_Service_Point_FID != 0)
                {
                  g_Locate_Method = "ADDRESS";
                }
                else
                {
                  LocateSrvcPtByMeterGeocode();
                  if(g_Service_Point_FID != 0)
                  {
                    g_Locate_Method = "METER GEOCODE";
                  }
                }
              }
            }
          }
        }        
      }
      if(g_Locate_Method != null)
      {
        string updateStatement = "UPDATE SERVICE_ACTIVITY SET ";
        updateStatement += "GIS_LOCATE_METHOD = ? ";
        updateStatement += "WHERE SERVICE_ACTIVITY_ID = ?";
        int recordsAffected = 0;
        gtDataContext.Execute(updateStatement, out recordsAffected, (int)ADODB.CommandTypeEnum.adCmdText, g_Locate_Method, g_Transaction_Number);
        gtDataContext.Execute("COMMIT", out recordsAffected, (int)ADODB.CommandTypeEnum.adCmdText);
      }
    }


    public void StructureIdCorrection()
    {
      // Get the features connected to the Service Point
      string connectedFIDSQuery = "SELECT CONN2.G3E_FID, CONN2.G3E_FNO FROM CONNECTIVITY_N CONN1, CONNECTIVITY_N CONN2, COMMON_N COMM WHERE CONN1.G3E_FID = ? " +
                                  "AND (CONN2.NODE_1_ID = CONN1.NODE_1_ID OR CONN2.NODE_2_ID = CONN1.NODE_1_ID) AND COMM.G3E_FID = CONN2.G3E_FID AND COMM.FEATURE_STATE_C IN ('PPI', 'ABI', 'INI', 'CLS') AND COMM.G3E_FID != ?";
      Recordset connectedFIDS = gtDataContext.OpenRecordset(connectedFIDSQuery, CursorTypeEnum.adOpenDynamic, LockTypeEnum.adLockOptimistic, -1, g_Service_Point_FID, g_Service_Point_FID);

      if(null != connectedFIDS && 0 < connectedFIDS.RecordCount)
      {
        connectedFIDS.MoveFirst();
        GetOwningStructureFID(Convert.ToInt32(connectedFIDS.Fields["G3E_FID"].Value));

        if(status == "FAILED")
        {
          UpdateTransaction(message, status);
        }
        else
        {
          if(g_Structure_FID == g_OwningStructureFID)
          {
            UpdateTransaction("Structure ID Corrected", "COMPLETED");
          }
          else
          {
            if(status == "FAILED")
            {
              UpdateTransaction(message, status);
            }
            else
            {
              Int16 sourceFNO = 0;

              // Get the features from the metadata connectivity that can be connected to the feature (Service Line or Service Point) that will be moved
              string metadataConnFilter;
              if (Convert.ToInt32(connectedFIDS.Fields["G3E_FNO"].Value) == g_Service_Line_FNO)
              {
                metadataConnFilter = "G3E_SOURCEFNO = " + g_Service_Line_FNO + " and G3E_CONNECTINGFNO <> " + g_Service_Point_FNO;
                sourceFNO = g_Service_Line_FNO;
              }
              else
              {
                metadataConnFilter = "G3E_SOURCEFNO = " + g_Service_Point_FNO;
                sourceFNO = g_Service_Point_FNO;
              }

              Recordset connectionCandidates = gtDataContext.MetadataRecordset("G3E_NODEEDGECONN_ELEC_OPTABLE", metadataConnFilter);
              List<Int16> connCandidates = new List<short>();

              if (connectionCandidates.RecordCount > 0)
              {
                connectionCandidates.MoveFirst();
                while (!connectionCandidates.EOF)
                {
                  connCandidates.Add(Convert.ToInt16(connectionCandidates.Fields["G3E_CONNECTINGFNO"].Value));
                  connectionCandidates.MoveNext();
                }
              }

              // Get the features on the new owning structure filtered by only the ones that can be connected to the feature (Service Line or Service Point) that will be moved
              IGTKeyObject structure = gtDataContext.OpenFeature(g_Structure_FNO, g_Structure_FID);

              gtRelationshipService.DataContext = gtDataContext;
              gtRelationshipService.ActiveFeature = structure;
              IGTKeyObjects structureOwned = gtRelationshipService.GetRelatedFeatures(elecOwner);
              IGTKeyObjects structureOwnedFacilities = GTClassFactory.Create<IGTKeyObjects>();

              foreach (IGTKeyObject feature in structureOwned)
              {
                if (connCandidates.Contains<short>(feature.FNO))
                {
                  structureOwnedFacilities.Add(feature);
                }
              }

              GTRelationshipOrdinalConstants connectingRelationshipOrdinal = GTRelationshipOrdinalConstants.gtrelRelationshipOrdinal1;

              if (structureOwnedFacilities.Count > 0)
              {
                // Determine the node on the connecting feature to connect to
                Int16 connectionNode = 0;                

                connectionCandidates.Filter = "G3E_SOURCEFNO = " + sourceFNO + " and G3E_CONNECTINGFNO = " + structureOwnedFacilities[0].FNO;
                connectionCandidates.MoveFirst();
                if (!Convert.IsDBNull(connectionCandidates.Fields["G3E_RELATEDCONNECTINGNODE"].Value))
                {
                  connectionNode = Convert.ToInt16(connectionCandidates.Fields["G3E_RELATEDCONNECTINGNODE"].Value);
                  if (connectionNode == 2)
                  {
                    connectingRelationshipOrdinal = GTRelationshipOrdinalConstants.gtrelRelationshipOrdinal2;
                  }
                }
                else
                {
                  // No metadata restriction on which node to connect to.
                  // Use the located connecting facilities shared node on the located structure
                  Recordset connectingConnRS = structureOwnedFacilities[0].Components.GetComponent(g_Connectivity_CNO).Recordset;
                  if (connectingConnRS.RecordCount > 0)
                  {
                    connectingConnRS.MoveFirst();
                    int node1 = Convert.ToInt32(connectingConnRS.Fields["NODE_1_ID"].Value);
                    connectingRelationshipOrdinal = DetermineConnectingNode(node1, structureOwnedFacilities[0].FID, g_Structure_FID);
                  }
                }
              }

              if (Convert.ToInt32(connectedFIDS.Fields["G3E_FNO"].Value) == g_Service_Line_FNO)
              {
                IGTKeyObject serviceLine = gtDataContext.OpenFeature(Convert.ToInt16(connectedFIDS.Fields["G3E_FNO"].Value), Convert.ToInt32(connectedFIDS.Fields["G3E_FID"].Value));
                IGTComponent serviceGeoComponent = serviceLine.Components.GetComponent(g_ServiceLine_Linear_CNO);

                IGTKeyObject servicePoint = gtDataContext.OpenFeature(g_Service_Point_FNO, g_Service_Point_FID);
                IGTComponent servicepointGeoComponent = servicePoint.Components.GetComponent(g_Service_Point_Symbol_CNO);

                // Move the Structure end of the Service Line to a connecting facility on the located Structure
                // Get the service line point furthest from the service point
                IGTPolylineGeometry newServiceLineGeometry = GTClassFactory.Create<IGTPolylineGeometry>();                
                IGTPoint newServiceLinePt = DetermineClosestPoint(serviceGeoComponent.Geometry, servicepointGeoComponent.Geometry.FirstPoint, false);
                if(serviceGeoComponent.Geometry.FirstPoint.X == newServiceLinePt.X && serviceGeoComponent.Geometry.FirstPoint.Y == newServiceLinePt.Y)
                {
                  Recordset FacilityGeometryInformation = gtDataContext.MetadataRecordset("G3E_FEATURES_OPTABLE", "G3E_FNO = " + structureOwnedFacilities[0].FNO);
                  short connectingPrimaryGraphicCNO = Convert.ToInt16(FacilityGeometryInformation.Fields["G3E_PRIMARYGEOGRAPHICCNO"].Value.ToString());
                  newServiceLineGeometry.Points.Add(DetermineClosestPoint(structureOwnedFacilities[0].Components.GetComponent(connectingPrimaryGraphicCNO).Geometry, GetStructureGeometry(), true));
                  newServiceLineGeometry.Points.Add(servicepointGeoComponent.Geometry.FirstPoint);
                  serviceGeoComponent.Geometry = newServiceLineGeometry;
                }
                else
                {
                  newServiceLineGeometry.Points.Add(servicepointGeoComponent.Geometry.FirstPoint);
                  newServiceLineGeometry.Points.Add(GetStructureGeometry());                  
                  serviceGeoComponent.Geometry = newServiceLineGeometry;
                }

                // Connect the Structure end of the Service Line to a facility on the located Structure
                gtRelationshipService.ActiveFeature = serviceLine;
                IGTKeyObjects connectedFeatures = gtRelationshipService.GetRelatedFeatures(ELECTRIC_CONNECTIVITY_RNO, GTRelationshipOrdinalConstants.gtrelRelationshipOrdinal1);
                bool servicePointOnNode1 = false;
                foreach (IGTKeyObject feature in connectedFeatures)
                {
                  if (feature.FNO == g_Service_Point_FNO)
                  {
                    servicePointOnNode1 = true;
                  }
                }

                if (servicePointOnNode1)
                {
                  gtRelationshipService.SilentDelete(ELECTRIC_CONNECTIVITY_RNO, GTRelationshipOrdinalConstants.gtrelRelationshipOrdinal2);
                  gtRelationshipService.SilentEstablish(ELECTRIC_CONNECTIVITY_RNO, structureOwnedFacilities[0], GTRelationshipOrdinalConstants.gtrelRelationshipOrdinal2, connectingRelationshipOrdinal);

                  // Reestablish connectivity with Service Point so new connectivity attributes are copied from Service Line to Service Point.
                  gtRelationshipService.ActiveFeature = servicePoint;
                  gtRelationshipService.SilentDelete(ELECTRIC_CONNECTIVITY_RNO, GTRelationshipOrdinalConstants.gtrelRelationshipOrdinal1);
                  gtRelationshipService.SilentEstablish(ELECTRIC_CONNECTIVITY_RNO, serviceLine, GTRelationshipOrdinalConstants.gtrelRelationshipOrdinal1, GTRelationshipOrdinalConstants.gtrelRelationshipOrdinal1);
                }
                else
                {
                  gtRelationshipService.SilentDelete(ELECTRIC_CONNECTIVITY_RNO, GTRelationshipOrdinalConstants.gtrelRelationshipOrdinal1);
                  gtRelationshipService.SilentEstablish(ELECTRIC_CONNECTIVITY_RNO, structureOwnedFacilities[0], GTRelationshipOrdinalConstants.gtrelRelationshipOrdinal1, connectingRelationshipOrdinal);

                  // Reestablish connectivity with Service Point so new connectivity attributes are copied from Service Line to Service Point.
                  gtRelationshipService.ActiveFeature = servicePoint;
                  gtRelationshipService.SilentDelete(ELECTRIC_CONNECTIVITY_RNO, GTRelationshipOrdinalConstants.gtrelRelationshipOrdinal1);
                  gtRelationshipService.SilentEstablish(ELECTRIC_CONNECTIVITY_RNO, serviceLine, GTRelationshipOrdinalConstants.gtrelRelationshipOrdinal1, GTRelationshipOrdinalConstants.gtrelRelationshipOrdinal2);
                }

                // Own the Service Line to the located Structure
                gtRelationshipService.ActiveFeature = serviceLine;
                gtRelationshipService.SilentEstablish(elecOwnedBy, structure);

                UpdateTransaction("Structure ID Corrected", "COMPLETED");
              }
              else
              {
                IGTKeyObject servicePoint = gtDataContext.OpenFeature(g_Service_Point_FNO, g_Service_Point_FID);
                Recordset metadata = gtDataContext.MetadataRecordset("G3E_FEATURES_OPTABLE", "G3E_FNO = " + g_Structure_FNO);
                metadata.MoveFirst();

                IGTComponent structureGeometry = structure.Components.GetComponent(Convert.ToInt16(metadata.Fields["G3E_PRIMARYGEOGRAPHICCNO"].Value));

                // Move the Service Point to the location of the Structure
                IGTComponent serviceGeoComponent = servicePoint.Components.GetComponent(g_Service_Point_Symbol_CNO);
                if (serviceGeoComponent.Geometry != null)
                {
                  serviceGeoComponent.Geometry = structureGeometry.Geometry;
                }                

                IGTComponent serviceGeoLabel = servicePoint.Components.GetComponent(g_Service_Point_Label_CNO);
                if(serviceGeoLabel.Geometry != null)
                {
                  serviceGeoLabel.Geometry = structureGeometry.Geometry;
                }                

                // Connect the Service Point to a facility on the located Structure
                gtRelationshipService.ActiveFeature = servicePoint;
                gtRelationshipService.SilentDelete(ELECTRIC_CONNECTIVITY_RNO, GTRelationshipOrdinalConstants.gtrelRelationshipOrdinal1);
                gtRelationshipService.SilentEstablish(ELECTRIC_CONNECTIVITY_RNO, structureOwnedFacilities[0], GTRelationshipOrdinalConstants.gtrelRelationshipOrdinal1, connectingRelationshipOrdinal);

                UpdateTransaction("Structure ID Corrected", "COMPLETED");
              }              
            }
          }
        }
      }
      else
      {
        UpdateTransaction("SERVICE POINT IS NOT CONNECTED", "FAILED");
        status = "FAILED";
      }
    }

    /// <summary>
    /// Gets and sets the attribute defaults for the passed in feature
    /// for the passed in fid.
    /// </summary>
    /// <param name="gtKeyObject">Feature key object to apply defaults.</param>
    /// <returns>Boolean indicating status</returns>
    private bool SetAttributeDefaults(IGTKeyObject gtKeyObject)
    {
      bool returnValue = false;

      try
      {
        string sql = "select tabattr.g3e_cno, tabattr.g3e_field, tabattr.g3e_default " +
                     "from g3e_dialogattributes_optable tabattr, g3e_dialogs_optable dialog " +
                     "where dialog.g3e_fno = ? and tabattr.g3e_dtno = dialog.g3e_dtno " +
                     "and dialog.g3e_type = 'Placement' and tabattr.g3e_default is not null " +
                     "order by tabattr.g3e_cno";

        Recordset attrDefaultsRS = gtDataContext.OpenRecordset(sql, CursorTypeEnum.adOpenStatic, LockTypeEnum.adLockReadOnly, (int)CommandTypeEnum.adCmdText, gtKeyObject.FNO);

        if(null != attrDefaultsRS && attrDefaultsRS.RecordCount > 0)
        {
          attrDefaultsRS.MoveFirst();

          do
          {
            short cno = Convert.ToInt16(attrDefaultsRS.Fields["G3E_CNO"].Value);
            string field = attrDefaultsRS.Fields["G3E_FIELD"].Value.ToString();
            string defaultValue = attrDefaultsRS.Fields["G3E_DEFAULT"].Value.ToString();

            Recordset componentRS = gtKeyObject.Components.GetComponent(cno).Recordset;
            componentRS.Fields[field].Value = defaultValue;

            attrDefaultsRS.MoveNext();
          } while(!attrDefaultsRS.EOF);
        }

        returnValue = true;
      }
      catch(Exception ex)
      {
        status = "FAILED";
        message = ex.Message;
      }

      return returnValue;
    }

    /// <summary>
    /// Gets a parameter value from SYS_GENERALPARAMETER given SUBSYSTEM_NAME, SUBSYSTEM_COMPONENT, and PARAM_NAME.
    /// </summary>
    /// <param name="subSystemName"></param>
    /// <param name="subSystemComponent"></param>
    /// <param name="paramName"></param>
    /// <returns>PARAM_VALUE if record exists and value is not null; else string.Empty</returns>
    private string GeneralParameter(string subSystemName, string subSystemComponent, string paramName)
    {
      string retVal = string.Empty;

      try
      {
        string sql = "select param_value from sys_generalparameter where subsystem_name=? and subsystem_component=? and param_name=? and param_value is not null";
        Recordset rs = gtDataContext.OpenRecordset(sql, CursorTypeEnum.adOpenStatic, LockTypeEnum.adLockReadOnly, (int)CommandTypeEnum.adCmdText, subSystemName, subSystemComponent, paramName);

        if(null != rs && 0 < rs.RecordCount)
        {
          rs.MoveFirst();
          retVal = rs.Fields[0].Value.ToString();
          rs.Close();
          rs = null;
        }
      }
      catch(Exception ex)
      {
        string exMsg = string.Format("Error occurred in {0} of {1}.{2}{3}", System.Reflection.MethodBase.GetCurrentMethod().Name, this.ToString(), Environment.NewLine, ex.Message);
        throw new Exception(exMsg);
      }

      return retVal;
    }

    /// <summary>
    /// Returns a component recordset (or NULL) based on the arguments.
    /// </summary>
    /// <param name="FNO"></param>
    /// <param name="FID"></param>
    /// <param name="CNO"></param>
    /// <returns>Recordset if exists and contains a record; else, NULL</returns>
    private Recordset ComponentRecordset(short FNO, int FID, short CNO)
    {
      Recordset retVal = null;

      try
      {
        IGTKeyObject feature = gtDataContext.OpenFeature(FNO, FID);

        if(null != feature && null != feature.Components)
        {
          IGTComponent component = feature.Components.GetComponent(CNO);

          if(null != component && null != component.Recordset && 0 < component.Recordset.RecordCount)
          {
            retVal = component.Recordset;
          }
        }
      }
      catch(Exception ex)
      {
        string exMsg = string.Format("Error occurred in {0} of {1}.{2}{3}", System.Reflection.MethodBase.GetCurrentMethod().Name, this.ToString(), Environment.NewLine, ex.Message);
        throw new Exception(exMsg);
      }

      return retVal;
    }

    /// <summary>
    /// Clears all the data 
    /// </summary>
    public void Dispose()
    {
      gtApp = null;
      gtDataContext = null;
      gtRelationshipService = null;
      gtTransactionManager = null;
      jobManagement = null;
    }
  }
}

