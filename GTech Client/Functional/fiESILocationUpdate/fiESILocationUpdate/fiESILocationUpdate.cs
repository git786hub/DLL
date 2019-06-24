using System;
using System.Windows.Forms;
using Intergraph.GTechnology.API;
using ADODB;
using Intergraph.GTechnology.Interfaces;

// ------------------------------------------------------------------------------------------
// Description:
//     This functional interface validates the ESI Location entered by the user. 
//     The validation will check that the ESI Location does not exist on another Premise 
//     record in GIS and that the ESI Location exists in the CIS_ESI_LOCATIONS table. 
//     If the validation passes, then this functional interface will copy the Premise data 
//     from the CIS_ESI_LOCATIONS table to the active Premise record in GIS. 
//     This functional interface will be called when a user enters an ESI Location 
//     on a Premise record in G/Technology.
// History:
//     12-DEC-2017, v0.1    Hexagon, Initial creation
// ------------------------------------------------------------------------------------------

namespace GTechnology.Oncor.CustomAPI
{
  public class fiESILocationUpdate : IGTFunctional
  {
    private IGTApplication m_Application = GTClassFactory.Create<IGTApplication>();
    private IGTDataContext m_DataContext = null;
    private IGTComponents m_GTComponents = null;
    private GTArguments m_GTArguments = null;
    private IGTFieldValue m_FieldValue = null;
    private GTFunctionalTypeConstants m_FunctionalType;
    private string m_ComponentName = string.Empty;
    private string m_FieldName = string.Empty;
    private bool m_InteractiveMode = false;
    int m_RecordPosition;

    const string M_PROP_SKIP_AECEDITIFNOTPOSTED = "Skip_aecEditIfNotPosted";

    const string FIELD_PREMISE_ESI_LOCATION = "PREMISE_NBR";
    const string FIELD_PREMISE_ADDRESS = "ADDRESS";
    const string FIELD_PREMISE_HOUSE_NBR = "HOUSE_NBR";
    const string FIELD_PREMISE_HOUSE_FRACTION = "HOUSE_FRACTION_NBR";
    const string FIELD_PREMISE_DIR_LEADING = "DIR_LEADING_C";
    const string FIELD_PREMISE_STREET_NAME = "STREET_NM";
    const string FIELD_PREMISE_STREET_TYPE = "STREET_TYPE_C";
    const string FIELD_PREMISE_DIR_TRAILING = "DIR_TRAILING_C";
    const string FIELD_PREMISE_UNIT_NBR = "UNIT_NBR";
    const string FIELD_PREMISE_ZIP = "ZIP_C";
    const string FIELD_PREMISE_CITY = "CITY_C";
    const string FIELD_PREMISE_TYPE = "TYPE_C";
    const string FIELD_PREMISE_RATE_CODE = "RATE_CODE_C";
    const string FIELD_PREMISE_SIC = "SIC_C";
    const string FIELD_PREMISE_COUNTY = "COUNTY_C";
    const string FIELD_PREMISE_INSIDE_CITY = "INSIDE_CITY_LIMITS_YN";
    const string FIELD_PREMISE_CRITICAL_LOAD = "CRITICAL_LOAD_Q";
    const string FIELD_PREMISE_CRITICAL_CUSTOMER = "CRITICAL_CUSTOMER_C";
    const string FIELD_PREMISE_PREMISE_STATUS = "PREMISE_STATUS";
    const string FIELD_PREMISE_MAJOR_CUSTOMER = "MAJOR_CUSTOMER_C";
    const string FIELD_PREMISE_DWELLING_TYPE = "DWELLING_TYPE_C";

    const string FIELD_CCB_ESI_LOCATION = "ESI_LOCATION";
    const string FIELD_CCB_ADDRESS = "ADDRESS";
    const string FIELD_CCB_HOUSE_NBR = "HOUSE_NUMBER";
    const string FIELD_CCB_HOUSE_FRACTION = "FRACTIONAL_HOUSE_NUMBER";
    const string FIELD_CCB_DIR_LEADING = "DIRECTION_INDICATOR";
    const string FIELD_CCB_STREET_NAME = "STREET_NAME";
    const string FIELD_CCB_STREET_TYPE = "STREET_TYPE";
    const string FIELD_CCB_DIR_TRAILING = "DIRECTIONAL_IND_TRAILING";
    const string FIELD_CCB_UNIT_NBR = "UNIT";
    const string FIELD_CCB_ZIP = "ZIP_CODE";
    const string FIELD_CCB_CITY = "CITY";
    const string FIELD_CCB_TYPE = "PREMISE_TYPE";
    const string FIELD_CCB_RATE_CODE = "RATE_CODE";
    const string FIELD_CCB_SIC = "SIC_CODE";
    const string FIELD_CCB_COUNTY = "COUNTY_CODE";
    const string FIELD_CCB_INSIDE_CITY = "INSIDE_CITY_LIMITS";
    const string FIELD_CCB_CRITICAL_LOAD = "CRITICAL_LOAD";
    const string FIELD_CCB_CRITICAL_CUSTOMER = "CRITICAL_CUSTOMER";
    const string FIELD_CCB_PREMISE_STATUS = "PREMISE_STATUS";
    const string FIELD_CCB_MAJOR_CUSTOMER = "MAJOR_CUSTOMER";
    const string FIELD_CCB_DWELLING_TYPE = "DWELLING_TYPE";

    const string ERROR_DUPLICATE_ESILOCATION_SRVCPT = "ESI Location already exists on this Service Point";
    const string ERROR_DUPLICATE_ESILOCATION = "ESI Location exists on Service Point FID ";
    const string ERROR_INVALID_ESILOCATION = "Invalid ESI Location";

    public GTArguments Arguments
    {
      get
      {
        return m_GTArguments;
      }

      set
      {
        m_GTArguments = value;
      }
    }

    public string ComponentName
    {
      get
      {
        return m_ComponentName;
      }

      set
      {
        m_ComponentName = value;
      }
    }

    public IGTComponents Components
    {
      get
      {
        return m_GTComponents;
      }

      set
      {
        m_GTComponents = value;
      }
    }

    public IGTDataContext DataContext
    {
      get
      {
        return m_DataContext;
      }

      set
      {
        m_DataContext = value;
      }
    }

    public string FieldName
    {
      get
      {
        return m_FieldName;
      }

      set
      {
        m_FieldName = value;
      }
    }

    public IGTFieldValue FieldValueBeforeChange
    {
      get
      {
        return m_FieldValue;
      }

      set
      {
        m_FieldValue = value;
      }
    }

    public GTFunctionalTypeConstants Type
    {
      get
      {
        return m_FunctionalType;
      }

      set
      {
        m_FunctionalType = value;
      }
    }

    public void Delete()
    {
      // Nothing to do when Premise record is deleted.
    }

    public void Execute()
    {
      Recordset componentRS = m_GTComponents[m_ComponentName].Recordset;
      string esiLocation = string.Empty;

      // Need to capture the position of the recordset in order to set
      // the correct fields. The position will change since we need to
      // loop through the recordset to check for a duplicate.
      m_RecordPosition = (int)componentRS.AbsolutePosition;

      // Only validate if new ESI Location value is not null
      if(!Convert.IsDBNull(componentRS.Fields[FIELD_PREMISE_ESI_LOCATION].Value))
      {
        esiLocation = componentRS.Fields[FIELD_PREMISE_ESI_LOCATION].Value.ToString();
      }
      else
      {
        return;
      }

      object propertyValue;

      // If G/Tech is not running in interactive mode, then skip message boxes.
      GUIMode guiMode = new GUIMode();
      m_InteractiveMode = guiMode.InteractiveMode;

      int g3eFID = Convert.ToInt32(componentRS.Fields["G3E_FID"].Value);

      // Validate that the entered ESI Location on the current Premise record 
      // does not exist on another Premise record for the active Service Point.
      if(ValidateDuplicateOnExistingFID(esiLocation, componentRS))
      {
        // Validate that the ESI Location is not being used by a 
        // Premise record on a different Service Point.
        if(ValidateDuplicate(esiLocation, g3eFID))
        {
          // Validate that the ESI Location exists in CC&B.
          if(ExistsInCCB(esiLocation))
          {
            // Populate the Premise record with the values from CC&B.
            if(!PopulatePremiseRecord(esiLocation, ref componentRS))
            {
              // On error, null the ESI Location
              ClearESILocation(ref componentRS);
            }
          }
          else
          {
            // On error, null the ESI Location
            ClearESILocation(ref componentRS);
          }
        }
        else
        {
          // On error, null the ESI Location
          ClearESILocation(ref componentRS);
        }
      }
      else
      {
        // On error, null the ESI Location
        ClearESILocation(ref componentRS);
      }
    }

    public void Validate(out string[] ErrorPriorityArray, out string[] ErrorMessageArray)
    {
      // Nothing to do when Premise record is validated in this interface.
      ErrorPriorityArray = null;
      ErrorMessageArray = null;
    }

    // Loop through the current Premise records on the active Service Point feature 
    // and validate that the entered ESI Location on the current Premise record 
    // does not exist on another Premise record for the active Service Point.
    private bool ValidateDuplicateOnExistingFID(string esiLocation, ADODB.Recordset componentRS)
    {
      bool returnValue = true;

      try
      {
        componentRS.MoveFirst();

        int currentRecordPosition = 0;
        string currentESILocation = "";

        while(!componentRS.EOF)
        {
          currentRecordPosition = (int)componentRS.AbsolutePosition;

          if(currentRecordPosition != m_RecordPosition)
          {
            if(!Convert.IsDBNull(componentRS.Fields[FIELD_PREMISE_ESI_LOCATION].Value))
            {
              currentESILocation = componentRS.Fields[FIELD_PREMISE_ESI_LOCATION].Value.ToString();

              if(esiLocation == currentESILocation)
              {
                if(m_InteractiveMode)
                {
                  MessageBox.Show(ERROR_DUPLICATE_ESILOCATION_SRVCPT, "G/Technology", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }
                returnValue = false;
                break;
              }
            }
          }
          componentRS.MoveNext();
        }
      }
      catch(Exception ex)
      {
        if(m_InteractiveMode)
        {
          MessageBox.Show("Error in fiESILocationUpdate:ValidateDuplicateOnExistingFID - " + ex.Message, "G/Technology", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        }
        returnValue = false;
      }

      return returnValue;
    }

    // Call the GISPKG_CCB_ESILOCATION.ValidateESILocation stored procedure to validate 
    // if the ESI Location is being used by a Premise record on a different Service Point.
    private bool ValidateDuplicate(string esiLocation, int g3eFID)
    {
      bool returnValue = true;

      try
      {
        ADODB.Command cmd = new ADODB.Command();

        cmd.CommandText = "{call GISPKG_CCB_ESILOCATION.ValidateESILocation(?,?,?,?,?)}";
        cmd.CommandType = CommandTypeEnum.adCmdText;

        ADODB.Parameter param = cmd.CreateParameter("ESILocation", DataTypeEnum.adVarChar, ParameterDirectionEnum.adParamInput, 10, esiLocation);
        cmd.Parameters.Append(param);
        param = cmd.CreateParameter("G3EFID", DataTypeEnum.adBigInt, ParameterDirectionEnum.adParamInput, 10, g3eFID);
        cmd.Parameters.Append(param);
        param = cmd.CreateParameter("ExistingFID", DataTypeEnum.adBigInt, ParameterDirectionEnum.adParamOutput, 10);
        cmd.Parameters.Append(param);
        param = cmd.CreateParameter("JobID", DataTypeEnum.adVarChar, ParameterDirectionEnum.adParamOutput, 30);
        cmd.Parameters.Append(param);
        param = cmd.CreateParameter("Status", DataTypeEnum.adSingle, ParameterDirectionEnum.adParamOutput, 1);
        cmd.Parameters.Append(param);

        int recordsAffected = 0;

        Recordset spRS = m_DataContext.ExecuteCommand(cmd, out recordsAffected);

        if(!Convert.ToBoolean(cmd.Parameters["Status"].Value))
        {
          string job = string.Empty;

          if(cmd.Parameters["JobID"].Value.ToString().Length > 0)
          {
            job = " in Job " + cmd.Parameters["JobID"].Value;
          }
          if(m_InteractiveMode)
          {
            MessageBox.Show(ERROR_DUPLICATE_ESILOCATION + cmd.Parameters["ExistingFID"].Value + job, "G/Technology", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
          }
          returnValue = false;
        }
      }
      catch(Exception ex)
      {
        if(m_InteractiveMode)
        {
          MessageBox.Show("Error in fiESILocationUpdate:ValidateDuplicate - " + ex.Message, "G/Technology", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        }
        returnValue = false;
      }

      return returnValue;
    }

    // Call the GISPKG_CCB_ESILOCATION.ValidatePremise database function to validate 
    // that a record exists in CC&B with the ESI_LOCATION value matching the user entered ESI Location.
    private bool ExistsInCCB(string esiLocation)
    {
      bool returnValue = true;

      try
      {
        string sql = "select GISPKG_CCB_ESILOCATION.ValidatePremise(?) as Status from dual";

        int recordsAffected = 0;

        Recordset spRS = m_DataContext.Execute(sql, out recordsAffected, (int)CommandTypeEnum.adCmdText, esiLocation);

        if(!Convert.ToBoolean(spRS.Fields["Status"].Value))
        {
          if(m_InteractiveMode)
          {
            MessageBox.Show(ERROR_INVALID_ESILOCATION, "G/Technology", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
          }
          returnValue = false;
        }
      }
      catch(Exception ex)
      {
        if(m_InteractiveMode)
        {
          MessageBox.Show("Error in fiESILocationUpdate:ExistsInCCB - " + ex.Message, "G/Technology", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        }
        returnValue = false;
      }

      return returnValue;
    }

    // Populate the Premise record with the values from the CIS_ESI_LOCATIONS table.
    // Call the GISPKG_CCB_ESILOCATION.GetESILocationRecord stored procedure to get the values.
    private bool PopulatePremiseRecord(string esiLocation, ref ADODB.Recordset componentRS)
    {
      bool returnValue = true;

      try
      {
        // Need to set a flag to let aecEditNotPosted attribute edit control
        // know that the attributes can be edited.
        AddProperty(M_PROP_SKIP_AECEDITIFNOTPOSTED, "TRUE");

        string sql = "GISPKG_CCB_ESILOCATION.GetESILocationRecord";
        int recordsAffected = 0;

        Recordset spRS = m_DataContext.Execute(sql, out recordsAffected, (int)CommandTypeEnum.adCmdStoredProc, esiLocation);

        // Reset recordset position before updating
        ResetRecordPosition(ref componentRS);

        componentRS.Fields[FIELD_PREMISE_ADDRESS].Value = spRS.Fields[FIELD_CCB_ADDRESS].Value.ToString();
        componentRS.Fields[FIELD_PREMISE_HOUSE_NBR].Value = spRS.Fields[FIELD_CCB_HOUSE_NBR].Value;
        componentRS.Fields[FIELD_PREMISE_HOUSE_FRACTION].Value = spRS.Fields[FIELD_CCB_HOUSE_FRACTION].Value;
        componentRS.Fields[FIELD_PREMISE_DIR_LEADING].Value = spRS.Fields[FIELD_CCB_DIR_LEADING].Value;
        componentRS.Fields[FIELD_PREMISE_STREET_NAME].Value = spRS.Fields[FIELD_CCB_STREET_NAME].Value;
        componentRS.Fields[FIELD_PREMISE_STREET_TYPE].Value = spRS.Fields[FIELD_CCB_STREET_TYPE].Value;
        componentRS.Fields[FIELD_PREMISE_DIR_TRAILING].Value = spRS.Fields[FIELD_CCB_DIR_TRAILING].Value;
        componentRS.Fields[FIELD_PREMISE_UNIT_NBR].Value = spRS.Fields[FIELD_CCB_UNIT_NBR].Value;
        componentRS.Fields[FIELD_PREMISE_ZIP].Value = spRS.Fields[FIELD_CCB_ZIP].Value;
        componentRS.Fields[FIELD_PREMISE_CITY].Value = spRS.Fields[FIELD_CCB_CITY].Value;
        componentRS.Fields[FIELD_PREMISE_TYPE].Value = spRS.Fields[FIELD_CCB_TYPE].Value;
        componentRS.Fields[FIELD_PREMISE_RATE_CODE].Value = spRS.Fields[FIELD_CCB_RATE_CODE].Value;
        componentRS.Fields[FIELD_PREMISE_SIC].Value = spRS.Fields[FIELD_CCB_SIC].Value;
        componentRS.Fields[FIELD_PREMISE_COUNTY].Value = spRS.Fields[FIELD_CCB_COUNTY].Value;
        componentRS.Fields[FIELD_PREMISE_INSIDE_CITY].Value = spRS.Fields[FIELD_CCB_INSIDE_CITY].Value;
        componentRS.Fields[FIELD_PREMISE_CRITICAL_LOAD].Value = spRS.Fields[FIELD_CCB_CRITICAL_LOAD].Value;
        componentRS.Fields[FIELD_PREMISE_CRITICAL_CUSTOMER].Value = spRS.Fields[FIELD_CCB_CRITICAL_CUSTOMER].Value;
        componentRS.Fields[FIELD_PREMISE_PREMISE_STATUS].Value = spRS.Fields[FIELD_CCB_PREMISE_STATUS].Value;
        componentRS.Fields[FIELD_PREMISE_MAJOR_CUSTOMER].Value = spRS.Fields[FIELD_CCB_MAJOR_CUSTOMER].Value;
        componentRS.Fields[FIELD_PREMISE_DWELLING_TYPE].Value = spRS.Fields[FIELD_CCB_DWELLING_TYPE].Value;

        // Remove the property to let aecEditNotPosted attribute edit control
        // know that the attributes cannot be edited.
        m_Application.Properties.Remove(M_PROP_SKIP_AECEDITIFNOTPOSTED);
      }
      catch(Exception ex)
      {
        if(m_InteractiveMode)
        {
          MessageBox.Show("Error in fiESILocationUpdate:PopulatePremiseRecord - " + ex.Message, "G/Technology", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        }
        returnValue = false;
      }

      return returnValue;
    }

    // An error has occured in validation. Null the ESI Location value.
    private void ClearESILocation(ref ADODB.Recordset componentRS)
    {
      try
      {
        // Need to set a flag to let aecEditNotPosted attribute edit control
        // know that ESI Location is being set to null and skip processing.
        // Attribute edit control is firing before the attribute is set to null,
        // so without the flag what happens is the Premise attributes are 
        // set to read-only by the attribute edit control and the ESI Location
        // is then set to null by this FI, thus locking the attributes including
        // the null ESI Location.
        AddProperty(M_PROP_SKIP_AECEDITIFNOTPOSTED, "TRUE");

        // Reset recordset position before updating
        ResetRecordPosition(ref componentRS);
        componentRS.Fields[FIELD_PREMISE_ESI_LOCATION].Value = null;
        m_Application.Properties.Remove(M_PROP_SKIP_AECEDITIFNOTPOSTED);
      }
      catch(Exception ex)
      {
        if(m_InteractiveMode)
        {
          MessageBox.Show("Error in fiESILocationUpdate:ClearESILocation - " + ex.Message, "G/Technology", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        }
      }
    }

    // Need to reset the recordset position before doing any updates.
    // The position changes from the starting postion due to looping
    // through the recordset and possibly through other interfaces
    // that get called.
    private void ResetRecordPosition(ref ADODB.Recordset componentRS)
    {
      // Reset the recordset position
      if(m_RecordPosition != (int)componentRS.AbsolutePosition)
      {
        componentRS.MoveFirst();
        while(!componentRS.EOF)
        {
          if(m_RecordPosition == (int)componentRS.AbsolutePosition)
          {
            break;
          }
          componentRS.MoveNext();
        }
      }
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

    // Add property to the Properties collection that acts like a global variable.
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
  }
}
