//----------------------------------------------------------------------------+
//        Class: fiSetPremiseLocationByBoundary
//  Description: This interface sets default values for City, County, and Zip Code on the Premise Attributes component
//----------------------------------------------------------------------------+
//     $Author:: hkonda                                                       $
//       $Date:: 16/10/17                                                     $
//   $Revision:: 1                                                            $
//----------------------------------------------------------------------------+
//    $History:: fiSetPremiseLocationByBoundary.cs                                           $
// 
// *****************  Version 1  *****************
// User: hkonda     Date: 16/10/17    Time: 18:00  Desc : Created
// User: hkonda     Date: 11/07/18    Time: 18:00  Desc : Fixed issue - Adding all new instances to Premise Attributes would not update City, Zip Code, County Code and City Limits.
// User: hkonda     Date: 06/08/18    Time: 18:00  Desc : Fixed issue - ONCORDEV-1934, Service Point, Premise Attribute Tab Set Premise Location By Boundary FI.
//----------------------------------------------------------------------------+

using System;
using System.Windows.Forms;
using ADODB;
using Intergraph.GTechnology.API;
using Intergraph.GTechnology.Interfaces;

namespace GTechnology.Oncor.CustomAPI
{

  public class fiSetPremiseLocationByBoundary : IGTFunctional
  {
    private GTArguments m_Arguments = null;
    private IGTDataContext m_DataContext = null;
    private IGTComponents m_components;
    private GTFunctionalTypeConstants m_Type;
    private string m_ComponentName;
    private string m_FieldName;
    private string m_City;
    private string m_CountyCode;
    private string m_ZipCode;
    private string m_CityLimits;

    public GTArguments Arguments
    {
      get { return m_Arguments; }
      set { m_Arguments = value; }
    }

    public string ComponentName
    {
      get { return m_ComponentName; }
      set { m_ComponentName = value; }
    }

    public IGTComponents Components
    {
      get { return m_components; }
      set { m_components = value; }
    }

    public IGTDataContext DataContext
    {
      get { return m_DataContext; }
      set { m_DataContext = value; }
    }

    public GTFunctionalTypeConstants Type
    {
      get { return m_Type; }
      set { m_Type = value; }
    }

    public string FieldName
    {
      get { return m_FieldName; }
      set { m_FieldName = value; }
    }

    public IGTFieldValue FieldValueBeforeChange
    {
      get;
      set;
    }
    public void Delete()
    {
    }

    public void Execute()
    {
      string city = string.Empty;
      string county = string.Empty;
      string zipCode = string.Empty;
      string insideCityLimits = string.Empty;


      try
      {
        Recordset premiseAttributesRs = Components[ComponentName].Recordset;
        if(premiseAttributesRs != null && premiseAttributesRs.RecordCount > 0)
          premiseAttributesRs.MoveFirst();

        while(!premiseAttributesRs.EOF)
        {
          city = Convert.ToString(premiseAttributesRs.Fields["CITY_C"].Value);
          county = Convert.ToString(premiseAttributesRs.Fields["COUNTY_C"].Value);
          zipCode = Convert.ToString(premiseAttributesRs.Fields["ZIP_C"].Value);
          insideCityLimits = Convert.ToString(premiseAttributesRs.Fields["INSIDE_CITY_LIMITS_YN"].Value);

          if(Convert.ToInt16(premiseAttributesRs.Fields["G3E_CID"].Value) == 1)
          {
            if(string.IsNullOrEmpty(city) && string.IsNullOrEmpty(county) && string.IsNullOrEmpty(zipCode) && string.IsNullOrEmpty(insideCityLimits))
            {
              CopyValuesFromBoundaries(premiseAttributesRs);
            }
            if(!string.IsNullOrEmpty(city) || !string.IsNullOrEmpty(county) || !string.IsNullOrEmpty(zipCode) || !string.IsNullOrEmpty(insideCityLimits))
            {
              m_City = city;
              m_CountyCode = county;
              m_ZipCode = zipCode;
              m_CityLimits = insideCityLimits;
            }
            premiseAttributesRs.MoveNext();
          }

          else
          {
            if(!string.IsNullOrEmpty(city) || !string.IsNullOrEmpty(county) || !string.IsNullOrEmpty(zipCode) || !string.IsNullOrEmpty(insideCityLimits))
            {
              m_City = city;
              m_CountyCode = county;
              m_ZipCode = zipCode;
              m_CityLimits = insideCityLimits;
            }

            if(string.IsNullOrEmpty(city) && string.IsNullOrEmpty(county) && string.IsNullOrEmpty(zipCode) && string.IsNullOrEmpty(insideCityLimits))
            {
              premiseAttributesRs.Fields["CITY_C"].Value = m_City;
              premiseAttributesRs.Fields["COUNTY_C"].Value = m_CountyCode;
              premiseAttributesRs.Fields["ZIP_C"].Value = m_ZipCode;
              premiseAttributesRs.Fields["INSIDE_CITY_LIMITS_YN"].Value = m_CityLimits;

            }
            premiseAttributesRs.MoveNext();
          }
        }
      }
      catch(Exception ex)
      {
        GUIMode guiMode = new GUIMode();
        if(guiMode.InteractiveMode)
        {
          MessageBox.Show("Error during execution of Set Premise Location By Boundary FI. " + ex.Message, "G/Technology");
        }
        else
        {
          int tmpRecModified = 0;
          string tmpQry = string.Empty;
          tmpQry = "begin insert into gis_stg.interface_log " +
                          "(SUB_INTERFACE_NAME,INTERFACE_NAME,COMPONENT_NAME,CORRELATION_ID,LOG_DETAIL) " +
                          "values ('ccGISAutomationBroker','ccGISAutomationBroker','fiSetPremiseLocationByBoundary',0," +
                                  "'Error during execution of Set Premise Location By Boundary FI. " + ex.Message +
              "'); end;";

          m_DataContext.Execute(tmpQry, out tmpRecModified, (int)CommandTypeEnum.adCmdText);
        }
      }
    }

    private void CopyValuesFromBoundaries(Recordset premiseAttributesRs)
    {
      short fNo = 0;
      int fId = 0;
      string city = string.Empty;
      string county = string.Empty;
      string zipCode = string.Empty;
      string insideCityLimits = string.Empty;
      string tmpQry = string.Empty;

      try
      {

        short cityFNO = Convert.ToInt16(m_Arguments.GetArgument(0));
        int cityANO = Convert.ToInt32(m_Arguments.GetArgument(1));
        short countyFNO = Convert.ToInt16(m_Arguments.GetArgument(2));
        int countyANO = Convert.ToInt32(m_Arguments.GetArgument(3));
        short zipFNO = Convert.ToInt16(m_Arguments.GetArgument(4));
        int zipANO = Convert.ToInt32(m_Arguments.GetArgument(5));

        Recordset commonComponentRs = Components.GetComponent(1).Recordset;
        if(commonComponentRs != null && commonComponentRs.RecordCount > 0)
        {
          commonComponentRs.MoveFirst();
          fNo = Convert.ToInt16(commonComponentRs.Fields["G3E_FNO"].Value);
          fId = Convert.ToInt32(commonComponentRs.Fields["G3E_FID"].Value);
        }

        short primaryGraphicCno = GetPrimaryGraphicCno(fNo);
        if(primaryGraphicCno == 0)
        {
          return;
        }
        IGTKeyObject servicePointFeature = m_DataContext.OpenFeature(fNo, fId);
        IGTOrientedPointGeometry geometry = (IGTOrientedPointGeometry)servicePointFeature.Components.GetComponent(primaryGraphicCno).Geometry;
        if(geometry == null)
        {
          return;
        }
        IGTPoint point = GTClassFactory.Create<IGTPoint>();
        point.X = geometry.FirstPoint.X;
        point.Y = geometry.FirstPoint.Y;
        // City
        customBoundaryQuery objCustomBoundaryQuery = new customBoundaryQuery(point, cityFNO);
        Recordset rs = objCustomBoundaryQuery.PerformPointInPolygon();
        if(rs != null && rs.RecordCount > 0)
        {
          IGTKeyObject cityFeature = DataContext.OpenFeature(Convert.ToInt16(rs.Fields["G3E_FNO"].Value), Convert.ToInt32(rs.Fields["G3E_FID"].Value));

          Recordset tempRs = m_DataContext.MetadataRecordset("G3E_ATTRIBUTEINFO_OPTABLE", "G3E_ANO = " + cityANO);
          if(tempRs != null && tempRs.RecordCount > 0)
          {
            tempRs.MoveFirst();
            string field = Convert.ToString(tempRs.Fields["G3E_FIELD"].Value);
            short cno = 0;
            if(!string.IsNullOrEmpty(Convert.ToString(tempRs.Fields["G3E_CNO"].Value)))
            {
              cno = Convert.ToInt16(tempRs.Fields["G3E_CNO"].Value);
            }
            if(!string.IsNullOrEmpty(field))
            {
              if((string.IsNullOrEmpty(Convert.ToString(premiseAttributesRs.Fields["CITY_C"].Value))) && cno > 0)
              {
                Recordset cityRs = cityFeature.Components.GetComponent(cno).Recordset;
                if(cityRs != null && cityRs.RecordCount > 0)
                {
                  cityRs.MoveFirst();
                  city = Convert.ToString(cityRs.Fields[field].Value);
                }
                if(!string.IsNullOrEmpty(city))
                {
                  //premiseAttributesRs.Fields["CITY_C"].Value = city;
                  m_City = city;
                  //premiseAttributesRs.Fields["INSIDE_CITY_LIMITS_YN"].Value = "Y";
                  m_CityLimits = "Y";
                }
              }
            }
          }
        }

        // County
        objCustomBoundaryQuery = new customBoundaryQuery(point, countyFNO);
        Recordset rs1 = objCustomBoundaryQuery.PerformPointInPolygon();
        if(rs1 != null && rs1.RecordCount > 0)
        {
          IGTKeyObject countyFeature = DataContext.OpenFeature(Convert.ToInt16(rs1.Fields["G3E_FNO"].Value), Convert.ToInt32(rs1.Fields["G3E_FID"].Value));
          Recordset tempRs1 = m_DataContext.MetadataRecordset("G3E_ATTRIBUTEINFO_OPTABLE", "G3E_ANO = " + countyANO);
          if(tempRs1 != null && tempRs1.RecordCount > 0)
          {
            tempRs1.MoveFirst();
            string field = Convert.ToString(tempRs1.Fields["G3E_FIELD"].Value);
            short cno = 0;
            if(!string.IsNullOrEmpty(Convert.ToString(tempRs1.Fields["G3E_CNO"].Value)))
            {
              cno = Convert.ToInt16(tempRs1.Fields["G3E_CNO"].Value);
            }
            if(!string.IsNullOrEmpty(field))
            {
              if((string.IsNullOrEmpty(Convert.ToString(premiseAttributesRs.Fields["COUNTY_C"].Value))) && cno > 0)
              {
                Recordset countyRs = countyFeature.Components.GetComponent(cno).Recordset;
                if(countyRs != null && countyRs.RecordCount > 0)
                {
                  countyRs.MoveFirst();
                  county = Convert.ToString(countyRs.Fields[field].Value);
                }

                if(!string.IsNullOrEmpty(county))
                {
                  //premiseAttributesRs.Fields["COUNTY_C"].Value = county;
                  m_CountyCode = county;
                }
              }
            }
          }
        }

        // Zip Code
        objCustomBoundaryQuery = new customBoundaryQuery(point, zipFNO);
        Recordset rs2 = objCustomBoundaryQuery.PerformPointInPolygon();
        if(rs2 != null && rs2.RecordCount > 0)
        {
          IGTKeyObject zipCodeFeature = DataContext.OpenFeature(Convert.ToInt16(rs2.Fields["G3E_FNO"].Value), Convert.ToInt32(rs2.Fields["G3E_FID"].Value));
          Recordset tempRs2 = m_DataContext.MetadataRecordset("G3E_ATTRIBUTEINFO_OPTABLE", "G3E_ANO = " + zipANO);
          if(tempRs2 != null && tempRs2.RecordCount > 0)
          {
            tempRs2.MoveFirst();
            string field = Convert.ToString(tempRs2.Fields["G3E_FIELD"].Value);
            short cno = 0;
            if(!string.IsNullOrEmpty(Convert.ToString(tempRs2.Fields["G3E_CNO"].Value)))
            {
              cno = Convert.ToInt16(tempRs2.Fields["G3E_CNO"].Value);
            }
            if(!string.IsNullOrEmpty(field))
            {
              if((string.IsNullOrEmpty(Convert.ToString(premiseAttributesRs.Fields["ZIP_C"].Value))) && cno > 0)
              {
                Recordset zipRs = zipCodeFeature.Components.GetComponent(cno).Recordset;
                if(zipRs != null && zipRs.RecordCount > 0)
                {
                  zipRs.MoveFirst();
                  zipCode = Convert.ToString(zipRs.Fields[field].Value);
                }
                if(!string.IsNullOrEmpty(zipCode))
                {
                  //premiseAttributesRs.Fields["ZIP_C"].Value = zipCode;
                  m_ZipCode = zipCode;
                }
              }
            }
          }

          try
          {
            premiseAttributesRs.Fields["CITY_C"].Value = m_City;
            premiseAttributesRs.Fields["INSIDE_CITY_LIMITS_YN"].Value = m_CityLimits;
            premiseAttributesRs.Fields["COUNTY_C"].Value = m_CountyCode;
            premiseAttributesRs.Fields["ZIP_C"].Value = m_ZipCode;
          }
          catch(Exception ex)
          {
            if(ex.Message == "Operation was canceled.")
            {
              int tmpRecModified = 0;
              tmpQry = "begin update premise_n set CITY_C = '" + m_City + "', INSIDE_CITY_LIMITS_YN  = '" + m_CityLimits +
                              "', COUNTY_C = '" + m_CountyCode + "', ZIP_C = '" + m_ZipCode +
                              "' where g3e_fid = " + premiseAttributesRs.Fields["G3E_FID"].Value.ToString() +
                                  " and g3e_cid = " + premiseAttributesRs.Fields["G3E_CID"].Value.ToString() + "; end;";

              m_DataContext.Execute(tmpQry, out tmpRecModified, (int)CommandTypeEnum.adCmdText);
            }
            else
            {
              throw;
            }
          }

        }

      }
      catch(Exception)
      {
        throw;
      }
    }

    public void Validate(out string[] ErrorPriorityArray, out string[] ErrorMessageArray)
    {
      ErrorMessageArray = null;
      ErrorPriorityArray = null;
    }

    /// <summary>
    ///  Method to get the primary graphic cno for fno
    /// </summary>
    /// <param name="fNo"></param>
    /// <returns>cno or 0</returns>
    private short GetPrimaryGraphicCno(short fNo)
    {
      short primaryGraphicCno = 0;
      try
      {
        Recordset tempRs = m_DataContext.MetadataRecordset("G3E_FEATURES_OPTABLE", "G3E_FNO = " + fNo);
        if(tempRs != null && tempRs.RecordCount > 0)
        {
          tempRs.MoveFirst();
          primaryGraphicCno = Convert.ToInt16(tempRs.Fields["G3E_PRIMARYGEOGRAPHICCNO"].Value);
        }
        return primaryGraphicCno;
      }
      catch(Exception)
      {
        throw;
      }
    }
  }
}

