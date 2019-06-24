//----------------------------------------------------------------------------+
//        Class: InputValidation
//  Description: This class contains methods to validate input parameters and various validation conditions.
//                                                                  
//----------------------------------------------------------------------------+
//          $Author     :: Shubham Agarwal                                       $
//          $Date       :: 10/01/2019                                               $
//          $Revision   :: 1                                                   $
//----------------------------------------------------------------------------+
//    $History:: InputValidation.cs                     $
// 
// *****************  Version 1  *****************
// User: sagarwal     Date: 10/01/2019   Time: 18:00  Desc : Created
//----------------------------------------------------------------------------+

using System;
using System.Collections.Generic;
using System.IO;

namespace Intergraph.GTechnology.API
{
  public class InputValidation
  {
    internal ValidationErrors ValidatePlotTemplate(IGTApplication p_App, string p_PlotTemplateName)
    {
      ValidationErrors b_Return = ValidationErrors.PlotTemplateDoesNotExist;

      try
      {
        foreach(IGTNamedPlot item in p_App.NamedPlots)
        {
          if(item.Name.Equals(p_PlotTemplateName))
          {
            b_Return = ValidationErrors.NoError;
            if(!MapFrameExists(item))
            {
              b_Return = ValidationErrors.MapFrameInPlotTemplateDoesNotExist;
              break;
            }
          }
        }

      }
      catch(Exception)
      {
        throw;
      }

      return b_Return;
    }


    internal ValidationErrors ValidateInitialInput(IGTApplication p_App, string p_OutputLocation, string p_LegendName, out List<PlotBoundaryAttributes> p_outPlotBoundaries)
    {
      ValidationErrors b_Return = ValidationErrors.NoError;
      try
      {
        p_outPlotBoundaries = GetPlotBoundaries(p_App);

        if(string.IsNullOrEmpty(p_App.DataContext.ActiveJob))
        {
          b_Return = ValidationErrors.ActiveJobDoesNotExist;
        }
        else if(!CanCreateFile(p_OutputLocation))
        {
          b_Return = ValidationErrors.OutputFilePathNotAccessible;
        }
        else if(p_outPlotBoundaries.Count == 0)
        {
          b_Return = ValidationErrors.PlotBoundaryDoesNotExist;
        }
        else if(!IsValidLegend(p_App, p_LegendName))
        {
          b_Return = ValidationErrors.NotValidLegend;
        }
      }
      catch(Exception)
      {
        throw;
      }

      return b_Return;
    }

    private List<PlotBoundaryAttributes> GetPlotBoundaries(IGTApplication p_App)
    {
      List<PlotBoundaryAttributes> plotBoundaries = new List<PlotBoundaryAttributes>();

      try
      {
        ADODB.Recordset rs = p_App.DataContext.OpenRecordset("select * from PLOTBND_N where job_id =? and product_type='Construction Print' order by SHEET_NUM ASC", ADODB.CursorTypeEnum.adOpenDynamic, ADODB.LockTypeEnum.adLockOptimistic, (int)ADODB.CommandTypeEnum.adCmdText, p_App.DataContext.ActiveJob);

        if(rs != null)
        {
          if(rs.RecordCount > 0)
          {
            rs.MoveFirst();
            while(rs.EOF == false)
            {
              PlotBoundaryAttributes obj = new PlotBoundaryAttributes()
              {
                FID = Convert.ToInt32(rs.Fields["g3e_fid"].Value),
                FNO = Convert.ToInt16(rs.Fields["g3e_fno"].Value),
                ProductType = Convert.ToString(rs.Fields["PRODUCT_TYPE"].Value),
                PlotTemplateName = Convert.ToString(rs.Fields["PRODUCT_TYPE"].Value),
                SheetNumber = Convert.ToString(rs.Fields["SHEET_NUM"].Value)
              };
              plotBoundaries.Add(obj);
              rs.MoveNext();
            }
          }
        }
      }
      catch(Exception)
      {
        throw;
      }

      return plotBoundaries;
    }

    private bool MapFrameExists(IGTNamedPlot p_NamedPlot)
    {
      bool bReturn = true;
      int iCount = 0;

      try
      {
        for(int i = 0;i < p_NamedPlot.Frames.Count;i++)
        {
          if(p_NamedPlot.Frames[i].Type == GTPlotFrameTypeConstants.gtpftMap)
          {
            iCount = iCount + 1;
          }
        }
        if(iCount == 0 || iCount > 1)
        {
          bReturn = false;
        }
      }
      catch(Exception)
      {
        throw;
      }

      return bReturn;
    }
    private bool CanCreateFile(string p_Path)
    {

      string file = Path.Combine(p_Path, Guid.NewGuid().ToString() + ".tmp");
      bool canCreate;
      try
      {
        using(File.Create(file)) { }
        File.Delete(file);
        canCreate = true;
      }
      catch
      {
        canCreate = false;
      }
      return canCreate;
    }

    private bool IsValidLegend(IGTApplication p_oApp, string p_LegendName)
    {
      bool bReturn = false;

      try
      {
        ADODB.Recordset rs = p_oApp.DataContext.MetadataRecordset("G3E_LEGENDS_OPTABLE");
        rs.Filter = "G3E_USERNAME = '" + p_LegendName + "'";

        if(rs != null)
        {
          if(rs.RecordCount == 1)
          {
            bReturn = true;
          }
        }
      }
      catch(Exception)
      {
        throw;
      }

      return bReturn;
    }
  }


  public enum ValidationErrors
  {
    ActiveJobDoesNotExist, PlotBoundaryDoesNotExist, PlotTemplateDoesNotExist, MapFrameInPlotTemplateDoesNotExist, OutputFilePathNotAccessible, PLotTemplateContainsMoreMapFrame, NoError, NotValidLegend
  }
}
