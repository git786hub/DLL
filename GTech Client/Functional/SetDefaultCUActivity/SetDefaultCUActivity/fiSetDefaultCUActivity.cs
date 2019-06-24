using System;
using Intergraph.GTechnology.API;
using Intergraph.GTechnology.Interfaces;
//----------------------------------------------------------------------------+
//  Class: fiSetDefaultCUActivity
//  Description: This FI is responsible for the population of default activity based on the conditions      
//----------------------------------------------------------------------------+
//  $Author::   Shubham Agarwal                                                       $
//  $Date::     20/12/17                                                                $
//  $Revision:: 1                                                                   $
//----------------------------------------------------------------------------+

namespace GTechnology.Oncor.CustomAPI
{
  public class fiSetDefaultCUActivity : IGTFunctional
  {
    private IGTDataContext m_oDataContext = null;
    private string m_sComponentName = null;
    private IGTComponents m_oComponents = null;
    private GTArguments m_oArguments = null;

    private string m_sFieldName = null;
    private IGTFieldValue m_oFieldValue = null;
    private GTFunctionalTypeConstants m_oGTFunctionalType;


    public GTArguments Arguments
    {
      get
      {
        return m_oArguments;
      }

      set
      {
        m_oArguments = value;
      }
    }

    public string ComponentName
    {
      get
      {
        return m_sComponentName;
      }

      set
      {
        m_sComponentName = value;
      }
    }

    public IGTComponents Components
    {
      get
      {
        return m_oComponents;
      }

      set
      {
        m_oComponents = value;
      }
    }


    public IGTDataContext DataContext
    {
      get
      {
        return m_oDataContext;
      }

      set
      {
        m_oDataContext = value;
      }
    }

    public string FieldName
    {
      get
      {
        return m_sFieldName;
      }

      set
      {
        m_sFieldName = value;
      }
    }

    public IGTFieldValue FieldValueBeforeChange
    {
      get
      {
        return m_oFieldValue;
      }

      set
      {
        m_oFieldValue = value;
      }
    }

    public GTFunctionalTypeConstants Type
    {
      get
      {
        return m_oGTFunctionalType;
      }

      set
      {
        m_oGTFunctionalType = value;
      }
    }

    public void Delete()
    {
    }

    /// <summary>
    /// This Property checks for the CorrectionsMode property and returns true if it exists;
    /// else it checks further for the Job Type and if it is WR-MAPCOR then it will return true, else return false
    /// </summary>
    private bool CorrectionsMode
    {
      get
      {
        bool bReturn = false;

        if(m_oApp.Properties.Count > 0)
        {
          for(int i = 0;i < m_oApp.Properties.Count;i++)
          {
            try
            {
              if(m_oApp.Properties.Keys[i].ToString().Equals("CorrectionsMode"))
              {
                bReturn = true;
              }
            }
            catch(Exception)
            {

            }
          }
        }
        if(!bReturn) //Check for the Job Status if it is Map correction type
        {
          ADODB.Recordset rs = m_oApp.DataContext.OpenRecordset("select g3e_jobtype from g3e_job where g3e_identifier = ?", ADODB.CursorTypeEnum.adOpenDynamic, ADODB.LockTypeEnum.adLockOptimistic, (int)ADODB.CommandTypeEnum.adCmdText, m_oApp.DataContext.ActiveJob);
          rs.MoveFirst();

          if(Convert.ToString(rs.Fields[0].Value).Equals("WR-MAPCOR"))
          {
            bReturn = true;
          }

        }

        return bReturn;
      }
    }

    /// <summary>
    /// This method removes the CorrectionsMode property if it exists
    /// </summary>   

    IGTApplication m_oApp;
    public void Execute()
    {
      m_oApp = GTClassFactory.Create<IGTApplication>();

      Components[ComponentName].Recordset.Fields["ACTIVITY_C"].Value = CorrectionsMode ? "IC" : "I";

     // RemoveCorrectionModeProperty();
    }

    public void Validate(out string[] ErrorPriorityArray, out string[] ErrorMessageArray)
    {
      ErrorPriorityArray = null;
      ErrorMessageArray = null;
    }
  }
}
