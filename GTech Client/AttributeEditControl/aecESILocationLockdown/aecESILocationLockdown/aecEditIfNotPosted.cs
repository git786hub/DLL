using System;
using System.Windows.Forms;
using Intergraph.GTechnology.API;
using ADODB;

// ------------------------------------------------------------------------------------------
// Description:
//     This attribute edit control interface prevents the active attribute from being entered 
//     once the passed in attribute has been set and posted. 
//     This attribute edit control interface will be called 
//     when the record is put into edit mode in Feature Explorer 
//     or when the attribute is attempted to be modified within G/Technology 
//     such as through the Edit Common Attributes command.
// History:
//     12-DEC-2017, v0.1    Hexagon, Initial creation
// ------------------------------------------------------------------------------------------

namespace GTechnology.Oncor.CustomAPI
{
  public class aecEditIfNotPosted : Intergraph.GTechnology.Interfaces.IGTAttributeEditControl
  {
    private IGTDataContext m_DataContext = null;
    private IGTComponents m_GTComponents = null;
    private GTArguments m_GTArguments = null;
    private IGTFieldValue m_FieldValue = null;
    private string m_ComponentName = string.Empty;
    private string m_FieldName = string.Empty;
    private string m_FieldCheck = string.Empty;
    private string m_TableCheck = string.Empty;

    private bool m_InteractiveMode = true;

    const string M_PROP_SKIP_AECEDITIFNOTPOSTED = "Skip_aecEditIfNotPosted";

    public GTArguments Arguments
    {
      get
      {
        return m_GTArguments;
      }

      set
      {
        m_GTArguments = value;

        m_FieldCheck = m_GTArguments.GetArgument(0).ToString();
        m_TableCheck = m_GTArguments.GetArgument(1).ToString();
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

    public IGTFieldValue FieldValue
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

    // Return TRUE if attribute can be edited
    public bool IsAttributeEditable(int ANO)
    {
      bool bEditControl = true;

      try
      {
        // If the Component Name isn't passed in to this interface then skip processing.
        // This occurs when a field with a picklist is selected.
        if(m_ComponentName is null)
        {
          return bEditControl;
        }

        object propertyValue;

        // If G/Tech is not running in interactive mode, skip message boxes.
        GUIMode guiMode = new GUIMode();
        m_InteractiveMode = guiMode.InteractiveMode;

        // Skip processing if M_PROP_SKIP_AECEDITIFNOTPOSTED properties exists.
        // This flag is needed to let this attribute edit control
        // know that the passed in attribute is being set by an interface
        // such as a functional interface and that the processing 
        // in this attribute edit control interface should be skipped.
        if(CheckIfPropertyExists(M_PROP_SKIP_AECEDITIFNOTPOSTED, out propertyValue))
        {
          return bEditControl;
        }

        Recordset componentRS = m_GTComponents[m_ComponentName].Recordset;

        if(componentRS.RecordCount > 0)
        {
          if(!Convert.IsDBNull(componentRS.Fields["g3e_id"].Value))
          {
            int g3eID = Convert.ToInt32(componentRS.Fields["g3e_id"].Value);

            // Only check if m_FieldCheck is populated.
            // If m_FieldCheck is not populated then allow the attribute to be editable.
            if(componentRS.Fields[m_FieldCheck].Value.ToString().Length > 0)
            {
              // Call the CheckIfPopulatedAndPosted database function to determine if a master record exists with the m_FieldCheck populated.
              string sql = string.Format("select GISPKG_CCB_ESILOCATION.CheckIfPopulatedAndPosted('{0}','{1}',{2}) STATUS from dual", m_TableCheck, m_FieldCheck, g3eID);

              int affectedRows = 0;

              Recordset premiseRS = m_DataContext.Execute(sql, out affectedRows, (int)ADODB.CommandTypeEnum.adCmdText);

              if(premiseRS.RecordCount > 0)
              {
                premiseRS.MoveFirst();
                if(!Convert.IsDBNull(premiseRS.Fields["STATUS"].Value))
                {
                  int status = Convert.ToInt32(premiseRS.Fields["STATUS"].Value);
                  // If CheckIfPopulatedAndPosted returns a zero then allow the attribute to be editable.
                  // Otherwise, make the attribute read-only.
                  if(status > 0)
                  {
                    bEditControl = false;
                  }
                }
              }
            }
          }
        }
      }
      catch(Exception ex)
      {
        if(m_InteractiveMode)
        {
          MessageBox.Show("Error in aecEditIfNotPosted:IsAttributeEditable - " + ex.Message, "G/Technology", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        }
        bEditControl = false;
      }

      return bEditControl;
    }

    // Check if the property exists in the Properties collection
    // This is a place to store data that is needed as long as the session is active.
    public bool CheckIfPropertyExists(string propertyName, out object propertyValue)
    {
      bool returnValue = false;

      propertyValue = null;

      IGTApplication m_Application = GTClassFactory.Create<IGTApplication>();

      try
      {
        propertyValue = m_Application.Properties[propertyName];
        returnValue = true;
      }
      catch
      {
        returnValue = false;
      }

      m_Application = null;

      return returnValue;
    }
  }
}
