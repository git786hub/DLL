using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.IO;
using System.Reflection;
using System.Diagnostics;
using Intergraph.GTechnology.API;
using Excel = Microsoft.Office.Interop.Excel;
using System.Runtime.InteropServices.ComTypes;
using ADODB;

namespace GTechnology.Oncor.CustomAPI
{
  public partial class EmbeddedDT : Form
  {
    private IGTTransactionManager m_TransactionManager;
    private IGTApplication m_Application;
    private Excel.Workbook m_xlWorkbook { get; set; }
    private string m_CommandName = string.Empty;
    private string m_WorkbookPath = string.Empty;
    private string m_WorkbookName = string.Empty;
    private string m_ReportName = string.Empty;
    private string m_TempLocation = string.Empty;
    private short m_SelectedFNO = 0;
    private string m_WrNumber = string.Empty;
    private List<int> m_SelectedFIDs;
    private List<KeyValuePair<string, string>> m_ReportValues;
    private Recordset m_MetadataRS = null;

    private object m_MissingValue = Missing.Value;
    private string m_TempName = string.Empty;

    private string m_GuyingValidation = string.Empty;
    private bool m_NewGuyScenario = false;
    private short m_GuyScenarioCount = 0;

    [DllImport("ole32.dll")]
    static extern int GetRunningObjectTable(uint reserved, out IRunningObjectTable pprot);
    [DllImport("ole32.dll")]
    static extern int CreateBindCtx(uint reserved, out IBindCtx pctx);

    /// <summary>
    /// Constructor
    /// </summary>
    public EmbeddedDT()
    {
      InitializeComponent();
    }

    /// <summary>
    /// Property to set the transaction manager to be used for adding the hyperlink component
    /// </summary>
    public IGTTransactionManager TransactionManager
    {
      set
      {
        m_TransactionManager = value;
      }
    }

    /// <summary>
    /// Property to set the application
    /// </summary>
    public IGTApplication Application
    {
      set
      {
        m_Application = value;
      }
    }

    /// <summary>
    /// Property to set the command name
    /// </summary>
    public string CommandName
    {
      set
      {
        m_CommandName = value;
      }
    }

    /// <summary>
    /// Property to set the full path to the pdf that will get created.
    /// </summary>
    public string ReportName
    {
      set
      {
        m_ReportName = value;
      }
    }

    /// <summary>
    /// Property to set the feature identifier of the selected feature.
    /// </summary>
    public int SelectedFID
    {
      set
      {
        if(m_SelectedFIDs == null)
        {
          m_SelectedFIDs = new List<int>();
        }
        m_SelectedFIDs.Add(value);
      }
    }

    /// <summary>
    /// Property to set the feature number of the selected feature.
    /// </summary>
    public short SelectedFNO
    {
      set
      {
        m_SelectedFNO = value;
      }
    }

    /// <summary>
    /// Property to set the work request number.
    /// </summary>
    public string WrNumber
    {
      set
      {
        m_WrNumber = value;
      }
    }

    /// <summary>
    /// Property to get the metadata for the command.
    /// </summary>
    public Recordset MetadataRS
    {
      get
      {
        return m_MetadataRS;
      }
    }

    /// <summary>
    /// Property to set the default values that will be used in the spreadsheet.
    /// </summary>
    public List<KeyValuePair<string, string>> ReportValues
    {
      set
      {
        m_ReportValues = value;
      }
    }

    /// <summary>
    /// Property to indicate if a new Guying Scenario has been created.
    /// </summary>
    public bool NewGuyScenario
    {
      set
      {
        m_NewGuyScenario = value;
      }
    }

    /// <summary>
    /// Property to set the Guy Scenario count.
    /// </summary>
    public short GuyScenarioCount
    {
      set
      {
        m_GuyScenarioCount = value;
      }
    }

    /// <summary>
    /// Load form event
    /// </summary>
    private void EmbeddedDT_Load(object sender, EventArgs e)
    {
      try
      {
        // Get the command metadata
        if(!GetMetadata())
        {
          return;
        }

        // Workbook will be copied to temp directory.
        // Delete workbook file if it exists in the temp directory.
        if(m_TempLocation.Length > 0)
        {
          m_TempName = m_TempLocation + m_WorkbookName;
        }
        else
        {
          m_TempName = Path.GetTempPath() + m_WorkbookName;
        }

        if(File.Exists(m_TempName))
        {
          try
          {
            File.Delete(m_TempName);
          }
          catch(Exception ex)
          {
            MessageBox.Show(ConstantsDT.ERROR_DELETING_FILE + " " + m_TempName + ": " + Environment.NewLine + ex.Message, ConstantsDT.APPLICATION_NAME, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            return;
          }
        }

        // Copy file to temp directory
        File.Copy(m_WorkbookPath, m_TempName);

        // Load file in webbrowser
        webBrowser1.Navigate(m_TempName, false);
      }
      catch(Exception ex)
      {
        MessageBox.Show(ConstantsDT.ERROR_LOADING_FILE + " " + m_TempName + ": " + Environment.NewLine + ex.Message, ConstantsDT.APPLICATION_NAME, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
      }
    }

    /// <summary>
    /// Initialize the form size. Use the size of the form when last invoked in the current session.
    /// </summary>
    public void InitializeFormSize()
    {
      object propertyValue;
      CommonDT commonDT = new CommonDT();
      int formHeight = this.Height;
      int formWidth = this.Width;

      if(commonDT.CheckIfPropertyExists(m_CommandName + ConstantsDT.PROP_DT_EMBEDDED_FORM_HEIGHT, out propertyValue))
      {
        formHeight = Convert.ToInt32(propertyValue);
        if(commonDT.CheckIfPropertyExists(m_CommandName + ConstantsDT.PROP_DT_EMBEDDED_FORM_WIDTH, out propertyValue))
        {
          formWidth = Convert.ToInt32(propertyValue);
          this.Height = formHeight;
          this.Width = formWidth;
        }
      }
      else
      {
        // Show form at maximum size
        this.WindowState = FormWindowState.Maximized;
      }

      commonDT = null;
    }

    /// <summary>
    /// Get the command metadata.
    /// </summary>
    /// <returns>Boolean indicating status</returns>
    private bool GetMetadata()
    {
      bool returnValue = false;

      try
      {
        CommonDT.GetCommandMetadata(m_CommandName, ref m_MetadataRS);

        if(m_CommandName == ConstantsDT.COMMAND_NAME_SAG_CLEARANCE)
        {
          m_MetadataRS.MoveFirst();
          while(!m_MetadataRS.EOF)
          {
            if(m_MetadataRS.Fields["PARAM_NAME"].Value.ToString() == "WorkbookPath")
            {
              m_WorkbookPath = m_MetadataRS.Fields["PARAM_VALUE"].Value.ToString();
            }
            if(m_MetadataRS.Fields["PARAM_NAME"].Value.ToString() == "WorkbookName")
            {
              m_WorkbookName = m_MetadataRS.Fields["PARAM_VALUE"].Value.ToString();
            }
            m_MetadataRS.MoveNext();
          }
        }
        else if(m_CommandName == ConstantsDT.COMMAND_NAME_HANDHOLE_CIAC_CALCULATOR)
        {
          while(!m_MetadataRS.EOF)
          {
            if(m_MetadataRS.Fields["PARAM_NAME"].Value.ToString() == "WorkbookPath")
            {
              m_WorkbookPath = m_MetadataRS.Fields["PARAM_VALUE"].Value.ToString();
            }
            if(m_MetadataRS.Fields["PARAM_NAME"].Value.ToString() == "WorkbookName")
            {
              m_WorkbookName = m_MetadataRS.Fields["PARAM_VALUE"].Value.ToString();
            }
            m_MetadataRS.MoveNext();
          }
        }
        else if(m_CommandName == ConstantsDT.COMMAND_NAME_SRVC_ENCLOSURE_COST_CALCULATOR)
        {
          while(!m_MetadataRS.EOF)
          {
            if(m_MetadataRS.Fields["PARAM_NAME"].Value.ToString() == "WorkbookPath")
            {
              m_WorkbookPath = m_MetadataRS.Fields["PARAM_VALUE"].Value.ToString();
            }
            if(m_MetadataRS.Fields["PARAM_NAME"].Value.ToString() == "WorkbookName")
            {
              m_WorkbookName = m_MetadataRS.Fields["PARAM_VALUE"].Value.ToString();
            }
            m_MetadataRS.MoveNext();
          }
        }
        else if(m_CommandName == ConstantsDT.COMMAND_NAME_GUYING)
        {
          while(!m_MetadataRS.EOF)
          {
            if(m_MetadataRS.Fields["PARAM_NAME"].Value.ToString() == "WorkbookPath")
            {
              m_WorkbookPath = m_MetadataRS.Fields["PARAM_VALUE"].Value.ToString();
            }
            if(m_MetadataRS.Fields["PARAM_NAME"].Value.ToString() == "WorkbookName")
            {
              m_WorkbookName = m_MetadataRS.Fields["PARAM_VALUE"].Value.ToString();
            }
            if(m_MetadataRS.Fields["PARAM_NAME"].Value.ToString() == "LocalTrustedLocation")
            {
              m_TempLocation = m_MetadataRS.Fields["PARAM_VALUE"].Value.ToString();
            }
            if(m_MetadataRS.Fields["PARAM_NAME"].Value.ToString() == "ValidationChecks")
            {
              m_GuyingValidation = m_MetadataRS.Fields["PARAM_VALUE"].Value.ToString();
            }
            m_MetadataRS.MoveNext();
          }
        }

        returnValue = true;
      }
      catch(Exception ex)
      {
        MessageBox.Show(ConstantsDT.ERROR_RETRIEVING_COMMAND_METADATA + ": " + ex.Message, ConstantsDT.APPLICATION_NAME, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        returnValue = false;
      }

      return returnValue;
    }

    /// <summary>
    /// Webbrowser loaded event. Get the workbook and set the default values.
    /// </summary>
    private void webBrowser1_Navigated(object sender, WebBrowserNavigatedEventArgs e)
    {
      if((m_xlWorkbook = RetrieveWorkbook(m_TempName)) == null) return;

      // Populate the values in the spreadsheet matching the parameter name with the named fields in the spreadsheet template
      if(m_ReportValues != null)
      {
        try
        {
          foreach(KeyValuePair<string, string> kvp in m_ReportValues)
          {
            m_xlWorkbook.Names.Item(kvp.Key).RefersToRange.Value = kvp.Value;
          }
        }
        catch(Exception ex)
        {
          MessageBox.Show(ConstantsDT.ERROR_DEFAULT_VALUES + ": " + ex.Message, ConstantsDT.APPLICATION_NAME, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        }
      }
    }

    // Get the Excel workbook from the running object table
    private Excel.Workbook RetrieveWorkbook(string excelFile)
    {
      IRunningObjectTable pROT = null;
      IEnumMoniker pMonikerEnum = null;
      try
      {
        IntPtr pFetched = IntPtr.Zero;
        if(GetRunningObjectTable(0, out pROT) != 0 || pROT == null) return null;

        pROT.EnumRunning(out pMonikerEnum);
        pMonikerEnum.Reset();
        IMoniker[] monikers = new IMoniker[1];
        while(pMonikerEnum.Next(1, monikers, pFetched) == 0)
        {
          IBindCtx pCTX;
          string filePathName;
          CreateBindCtx(0, out pCTX);
          monikers[0].GetDisplayName(pCTX, null, out filePathName);
          Marshal.ReleaseComObject(pCTX);
          if(filePathName.IndexOf(excelFile) != -1)
          {
            object roVal;
            pROT.GetObject(monikers[0], out roVal);
            if(roVal is Excel.Workbook)
            {
              return roVal as Excel.Workbook;
            }
          }
        }
      }
      catch
      {
        return null;
      }
      finally
      {
        if(pROT != null) Marshal.ReleaseComObject(pROT);
        if(pMonikerEnum != null) Marshal.ReleaseComObject(pMonikerEnum);
      }
      return null;
    }

    // Close workbook and exit Excel process
    // Since the workbook is displayed in a webbrowser control
    // the workbook and excel process don't exit gracefully,
    // so need to kill the process if there are no other
    // workbooks open.
    public void Cleanup()
    {
      try
      {
        // Save form dimensions. To be used when loading the command again in the session.
        CommonDT commonDT = new CommonDT();
        commonDT.AddProperty(m_CommandName + ConstantsDT.PROP_DT_EMBEDDED_FORM_HEIGHT, this.Height);
        commonDT.AddProperty(m_CommandName + ConstantsDT.PROP_DT_EMBEDDED_FORM_WIDTH, this.Width);
        commonDT = null;

        if(m_SelectedFIDs != null)
        {
          m_SelectedFIDs.Clear();
          m_SelectedFIDs = null;
        }

        if(m_ReportValues != null)
        {
          m_ReportValues.Clear();
          m_ReportValues = null;
        }

        if(m_xlWorkbook != null)
        {
          m_xlWorkbook.Close(true, Missing.Value, Missing.Value);
          Marshal.ReleaseComObject(m_xlWorkbook);
          m_xlWorkbook = null;
          GC.Collect();
        }

        Process[] excelProcess = Process.GetProcessesByName("excel");

        foreach(Process p in excelProcess)
        {
          if(p.MainWindowTitle == "")
          {
            p.Kill();
            // The following steps are for a workaround.
            // Without these steps the workbook is ending up in a recovered state
            // the next time the user opens Excel.
            Excel.Application xlApp = new Excel.Application();
            xlApp.Quit();
            Marshal.ReleaseComObject(xlApp);
            xlApp = null;
            GC.Collect();
            break;
          }
        }

        excelProcess = null;

        if(File.Exists(m_TempName))
        {
          try
          {
            File.Delete(m_TempName);
          }
          catch
          {
            // ignore error
          }
        }
      }
      catch
      {
        // ignore error
      }
    }

    // Release the COM object
    private void ReleaseObjects(object obj)
    {
      try
      {
        Marshal.ReleaseComObject(obj);
        obj = null;
      }
      catch
      {
        obj = null;
      }
      finally
      {
        GC.Collect();
      }
    }

    // Call function to export the spreadsheet to a pdf.
    // Display the pdf
    private void cmdPrintReport_Click(object sender, EventArgs e)
    {
      string reportFilename = string.Empty;

      if(ExportReport(ref reportFilename))
      {
        try
        {
          // Display the pdf to the user
          Process.Start(reportFilename);
        }
        catch(Exception ex)
        {
          MessageBox.Show(ConstantsDT.ERROR_OPENING_PDF + " " + reportFilename + ": " + Environment.NewLine + ex.Message, ConstantsDT.APPLICATION_NAME, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        }
      }
    }

    // Save the report as a pdf
    private bool ExportReport(ref string reportFilename)
    {
      m_Application.SetStatusBarText(GTStatusPanelConstants.gtaspcMessage, ConstantsDT.MESSAGE_REPORT_CREATING);
      m_Application.BeginWaitCursor();

      bool returnValue = true;
      reportFilename = Path.GetTempPath() + m_ReportName;

      // Check if the user has the file open from a previous run of the report.
      if(File.Exists(reportFilename))
      {
        try
        {
          File.Delete(reportFilename);
        }
        catch(Exception ex)
        {
          MessageBox.Show(ConstantsDT.ERROR_DELETING_FILE + " " + reportFilename + ": " + Environment.NewLine + ex.Message, ConstantsDT.APPLICATION_NAME, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
          returnValue = false;
        }

        returnValue = true;
      }

      // Export the workbook to a pdf
      if(returnValue)
      {
        try
        {
          if(m_CommandName != ConstantsDT.COMMAND_NAME_GUYING)
          {
            // Only export first tab
            m_xlWorkbook.ExportAsFixedFormat(Excel.XlFixedFormatType.xlTypePDF, reportFilename);
          }
          else
          {
            Excel.Application xlApp = new Excel.Application();
            xlApp.DisplayAlerts = false;

            m_xlWorkbook.Save();

            Excel.Workbook tempWorkbook = xlApp.Workbooks.Add(Excel.XlWBATemplate.xlWBATWorksheet);
            tempWorkbook.SaveAs(Path.GetTempPath() + "Temp_" + m_WorkbookName);
            tempWorkbook = xlApp.Workbooks.Open(Path.GetTempPath() + "Temp_" + m_WorkbookName);
            Excel.Workbook masterWorkbook = xlApp.Workbooks.Open(m_TempName);
            Excel.Range range = masterWorkbook.Sheets["Summary"].Cells[30, 22] as Excel.Range;
            string summaryPage = range.Value2.ToString();
            string rowNumberSelect = "SELECT PARAM_VALUE FROM SYS_GENERALPARAMETER WHERE SUBSYSTEM_NAME = ? AND SUBSYSTEM_COMPONENT = ? AND PARAM_NAME = ?";
            Recordset topNumberRecord = m_Application.DataContext.OpenRecordset(rowNumberSelect, CursorTypeEnum.adOpenDynamic, LockTypeEnum.adLockBatchOptimistic, -1, "GuyingCC", "Summary Bottom Value", "Page " + (Convert.ToInt16(summaryPage) - 1));
            Recordset bottomNumberRecord = m_Application.DataContext.OpenRecordset(rowNumberSelect, CursorTypeEnum.adOpenDynamic, LockTypeEnum.adLockBatchOptimistic, -1, "GuyingCC", "Summary Bottom Value", "Page " + (Convert.ToInt16(summaryPage)));

            if(topNumberRecord.RecordCount > 0 || bottomNumberRecord.RecordCount > 0)
            {
              masterWorkbook.Sheets["AnchorTable"].Copy(tempWorkbook.Worksheets[1]);
              masterWorkbook.Sheets["ReportB"].Copy(tempWorkbook.Worksheets[1]);
              masterWorkbook.Sheets["ReportA"].Copy(tempWorkbook.Worksheets[1]);
              masterWorkbook.Sheets["Summary"].Copy(tempWorkbook.Worksheets[1]);
              tempWorkbook.Sheets["Summary"].Range("O1:Z4536").Delete();
              tempWorkbook.Sheets["Summary"].Visible = Excel.XlSheetVisibility.xlSheetVisible;
              //top range-- it will always have A and N top will always start at A1 no matter what, unless its the top, so we are looking for the bottom row before our diagram
              if(summaryPage != "1")
              {
                topNumberRecord.MoveFirst();
                int topNumber = Convert.ToInt32(topNumberRecord.Fields["PARAM_VALUE"].Value);
                tempWorkbook.Sheets["Summary"].Range("A1:N" + topNumber).Delete();
              }
              //bottom range-- it will always end (if it isnt the lowest possible diagram at Z4536
              if(summaryPage != "72")
              {
                bottomNumberRecord.MoveFirst();
                int bottomNumber = Convert.ToInt32(bottomNumberRecord.Fields["PARAM_VALUE"].Value) + 1;
                tempWorkbook.Sheets["Summary"].Range("A" + bottomNumber + ":N4539").Delete();
              }


              tempWorkbook.Save();

              tempWorkbook.ExportAsFixedFormat(Excel.XlFixedFormatType.xlTypePDF, reportFilename, null, true, false);
            }
            else
            {
              MessageBox.Show("Invalid Guying Arrangement!", "Unable to Print", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
              returnValue = false;
            }

            masterWorkbook.Close(false, m_MissingValue, m_MissingValue);
            tempWorkbook.Close(false, m_MissingValue, m_MissingValue);
            xlApp.Quit();

            ReleaseObjects(masterWorkbook);
            ReleaseObjects(tempWorkbook);
            ReleaseObjects(xlApp);
          }
        }
        catch(Exception ex)
        {
          MessageBox.Show(ConstantsDT.ERROR_CONVERTING_PDF + " " + reportFilename + ": " + Environment.NewLine + ex.Message, ConstantsDT.APPLICATION_NAME, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
          returnValue = false;
        }
      }

      m_Application.EndWaitCursor();
      m_Application.SetStatusBarText(GTStatusPanelConstants.gtaspcMessage, "");

      return returnValue;
    }

    // Save the report to a shared location and hyperlink the report to the selected FID
    // Run validation for Guying
    private void cmdSaveReport_Click(object sender, EventArgs e)
    {
      // Perform validation for Guying
      if(m_CommandName == ConstantsDT.COMMAND_NAME_GUYING)
      {
        if(!ValidateGuyingResults())
        {
          return;
        }
        else
        {
          foreach(int fid in m_SelectedFIDs)
          {
            WriteGuyingResults(fid);
          }
        }
      }

      string reportFilename = string.Empty;

      if(ExportReport(ref reportFilename))
      {
        try
        {
          m_Application.SetStatusBarText(GTStatusPanelConstants.gtaspcMessage, ConstantsDT.MESSAGE_REPORT_SAVING);
          m_Application.BeginWaitCursor();

          Recordset tmpRs = null;

          String tmpQry = String.Empty;
          Boolean bSpFileAdded = false;


          tmpQry = "select param_name, param_value from sys_generalparameter " +
                      "where SUBSYSTEM_NAME = ?";
          tmpRs = m_Application.DataContext.OpenRecordset(tmpQry, CursorTypeEnum.adOpenStatic,
                                                                 LockTypeEnum.adLockReadOnly,
                                                           (int)CommandTypeEnum.adCmdText,
                                                           "Doc_Management");
          if(!(tmpRs.BOF && tmpRs.EOF))
          {
            tmpRs.MoveFirst();
            tmpRs.MoveLast();
            tmpRs.MoveFirst();
            OncDocManage.OncDocManage rptToSave = new OncDocManage.OncDocManage();
            while(!tmpRs.EOF)
            {
              if(tmpRs.Fields["PARAM_NAME"].Value.ToString() == "JOBWO_REL_PATH")
                rptToSave.SPRelPath = tmpRs.Fields["PARAM_VALUE"].Value.ToString();
              if(tmpRs.Fields["PARAM_NAME"].Value.ToString() == "SP_URL")
                rptToSave.SPSiteURL = tmpRs.Fields["PARAM_VALUE"].Value.ToString();
              if(tmpRs.Fields["PARAM_NAME"].Value.ToString() == "ROOT_PATH")
                rptToSave.SPRootPath = tmpRs.Fields["PARAM_VALUE"].Value.ToString();
              tmpRs.MoveNext();
            }
            rptToSave.SrcFilePath = reportFilename;
            rptToSave.WrkOrd_Job = m_Application.DataContext.ActiveJob;
            rptToSave.SPFileName = reportFilename.Substring(reportFilename.LastIndexOf("\\") + 1);
            if(m_CommandName == ConstantsDT.COMMAND_NAME_GUYING)
            {
              rptToSave.SPFileType = "Guying Program";
            }
            else if(m_CommandName == ConstantsDT.COMMAND_NAME_SAG_CLEARANCE)
            {
              rptToSave.SPFileType = "Sag Clearance";
            }
            else
            {
              rptToSave.SPFileType = "";
            }
            bSpFileAdded = rptToSave.AddSPFile(true);
            if(bSpFileAdded == false)
            {
              string msg = string.Format("{0}: Error adding {1} to SharePoint.{2}{2}{3}", ConstantsDT.ERROR_REPORT_SAVING, rptToSave.SPFileName, System.Environment.NewLine, rptToSave.RetErrMessage);
              MessageBox.Show(m_Application.ApplicationWindow, msg, ConstantsDT.APPLICATION_NAME, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
              return;
            }
            tmpRs = null;

            m_TransactionManager.Begin("New Hyperlink");

            if(AddHyperlinkComponent(rptToSave.RetFileURL))
            {
              if(m_CommandName == ConstantsDT.COMMAND_NAME_GUYING && m_NewGuyScenario)
              {
                string sql = "update g3e_job set guy_scenario_count = ? where g3e_identifier = ?";
                int recordsAffected = 0;
                m_Application.DataContext.Execute(sql, out recordsAffected, (int)CommandTypeEnum.adCmdText, m_GuyScenarioCount, m_Application.DataContext.ActiveJob);
                m_Application.DataContext.Execute("commit", out recordsAffected, (int)CommandTypeEnum.adCmdText);
              }

              m_TransactionManager.Commit();
            }
            else
            {
              m_TransactionManager.Rollback();

            }
          }
          else
          {
            if(m_TransactionManager.TransactionInProgress)
            {
              m_TransactionManager.Rollback();
            }
            MessageBox.Show(m_Application.ApplicationWindow, ConstantsDT.ERROR_REPORT_SAVING + ": " +
                            "Error finding General Parameters JOBWO_REL_PATH or 'SP_URL",
                            ConstantsDT.APPLICATION_NAME, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            return;
          }
          m_Application.EndWaitCursor();
          m_Application.SetStatusBarText(GTStatusPanelConstants.gtaspcMessage, "");
        }
        catch(Exception ex)
        {
          m_Application.EndWaitCursor();
          m_Application.SetStatusBarText(GTStatusPanelConstants.gtaspcMessage, "");
          MessageBox.Show(m_Application.ApplicationWindow, ConstantsDT.ERROR_REPORT_SAVING + ": " + ex.Message, ConstantsDT.APPLICATION_NAME, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        }
      }
    }

    /// <summary>
    /// Adds a Hyperlink component to each of the selected fids
    /// </summary>
    /// <param name="filePath">The absolute path to the file</param>
    /// <returns>Boolean indicating status</returns>
    public bool AddHyperlinkComponent(string filePath)
    {
      bool returnValue = false;

      try
      {
        IGTKeyObject tmpKeyObj = GTClassFactory.Create<IGTKeyObject>();

        for(int i = 0;i < m_SelectedFIDs.Count;i++)
        {
          tmpKeyObj = m_Application.DataContext.OpenFeature(m_SelectedFNO, m_SelectedFIDs[i]);
          Recordset tmpHypLnk = tmpKeyObj.Components["HYPERLINK_N"].Recordset;
          tmpHypLnk.AddNew(Type.Missing, Type.Missing);
          tmpHypLnk.Fields["G3E_FNO"].Value = m_SelectedFNO;
          tmpHypLnk.Fields["G3E_FID"].Value = m_SelectedFIDs[i];
          switch(m_CommandName)
          {
            case ConstantsDT.COMMAND_NAME_GUYING:
              tmpHypLnk.Fields["TYPE_C"].Value = "Guying Scenario";
              tmpHypLnk.Fields["DESCRIPTION_T"].Value = m_WrNumber + "-" + m_GuyScenarioCount.ToString().PadLeft(2, '0');
              break;
            case ConstantsDT.COMMAND_NAME_SAG_CLEARANCE:
              tmpHypLnk.Fields["TYPE_C"].Value = "Sag Clearance";
              break;
            default:
              tmpHypLnk.Fields["TYPE_C"].Value = "";
              break;
          }
          tmpHypLnk.Fields["HYPERLINK_T"].Value = filePath;
          tmpHypLnk.Fields["FILENAME_T"].Value = filePath.Substring(filePath.LastIndexOf("/") + 1);
          m_Application.DataContext.UpdateBatch(tmpHypLnk);
          tmpHypLnk.Update();
          tmpKeyObj = null;
        }

        returnValue = true;
      }
      catch(Exception ex)
      {
        MessageBox.Show(m_Application.ApplicationWindow, ConstantsDT.ERROR_ADDING_HYPERLINK + ": " + ex.Message, ConstantsDT.APPLICATION_NAME, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        returnValue = false;
      }

      return returnValue;
    }

    // Validate that metadata defined cells are not red in Guying workbook
    private bool ValidateGuyingResults()
    {
      bool returnValue = false;

      try
      {
        m_Application.SetStatusBarText(GTStatusPanelConstants.gtaspcMessage, ConstantsDT.MESSAGE_VALIDATING);
        m_Application.BeginWaitCursor();

        m_xlWorkbook.Application.ScreenUpdating = false;

        // Save active worksheet to reset after validation.
        Excel.Worksheet activeSheet = (Excel.Worksheet)m_xlWorkbook.Application.ActiveSheet;
        string previousSheetName = activeSheet.Name;

        string validationMessage = "";

        if(m_GuyingValidation.Length > 0)
        {
          string[] validationCheck = m_GuyingValidation.Split(',');
          string[] cellCheck;

          foreach(string tabColumn in validationCheck)
          {
            cellCheck = tabColumn.Split('!');
            Excel.Range validationRange = m_xlWorkbook.Worksheets[cellCheck[0]].Range[cellCheck[1]];

            if(previousSheetName != cellCheck[0].ToString())
            {
              m_xlWorkbook.Worksheets[cellCheck[0]].Activate();
            }

            // Check if conditional formatting is applied
            if(InvalidConfiguration(validationRange))
            {
              validationMessage += Environment.NewLine + tabColumn;
            }

            previousSheetName = cellCheck[0].ToString();
          }

          // Reset active worksheet
          m_xlWorkbook.Worksheets[activeSheet.Name].Activate();
          m_xlWorkbook.Application.ScreenUpdating = true;

          if(validationMessage.Length > 0)
          {
            MessageBox.Show("Validation Error: " + validationMessage, ConstantsDT.APPLICATION_NAME, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            returnValue = false;
          }
          else
          {
            returnValue = true;
          }
        }
      }
      catch(Exception ex)
      {
        MessageBox.Show(ConstantsDT.ERROR_GUY_VALIDATING_RESULTS + ": " + ex.Message, ConstantsDT.APPLICATION_NAME, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        returnValue = false;
      }

      m_Application.EndWaitCursor();
      m_Application.SetStatusBarText(GTStatusPanelConstants.gtaspcMessage, "");

      return returnValue;
    }

    // Write validation results to the database for each Pole FID selected for the Guying tool
    private bool WriteGuyingResults(int fid)
    {
      bool returnValue = false;

      try
      {
        m_Application.SetStatusBarText(GTStatusPanelConstants.gtaspcMessage, ConstantsDT.MESSAGE_VALIDATING);
        m_Application.BeginWaitCursor();

        string validationStatus = ConstantsDT.VALIDATION_PASS;
        string validationPriority = "P3";
        string comments = "Hyperlink record added - " + m_ReportName;
        int recordsAffected = 0;

        // Check if record exists for WR Number, Pole FID and command
        Recordset validationRS = m_Application.DataContext.OpenRecordset(ConstantsDT.QUERY_VALIDATION, CursorTypeEnum.adOpenDynamic,
                       LockTypeEnum.adLockOptimistic, (int)CommandTypeEnum.adCmdText, m_WrNumber, fid, ConstantsDT.COMMAND_NAME_GUYING);

        if(validationRS.RecordCount > 0)
        {
          // Update record
          m_Application.DataContext.Execute(ConstantsDT.SQL_UPDATE_VALIDATION, out recordsAffected, (int)CommandTypeEnum.adCmdText,
                                            validationStatus, validationPriority, comments, m_WrNumber, fid, ConstantsDT.COMMAND_NAME_GUYING);
        }
        else
        {
          // Add record
          m_Application.DataContext.Execute(ConstantsDT.SQL_INSERT_VALIDATION, out recordsAffected, (int)CommandTypeEnum.adCmdText,
                                            m_WrNumber, fid, ConstantsDT.COMMAND_NAME_GUYING, validationStatus,
                                            validationPriority, comments);
        }

        // Commit validation records to database.
        m_Application.DataContext.Execute("commit", out recordsAffected, (int)CommandTypeEnum.adCmdText);

        returnValue = true;
      }
      catch(Exception ex)
      {
        MessageBox.Show(ConstantsDT.ERROR_GUY_WRITING_VALIDATION_RESULTS + ": " + ex.Message, ConstantsDT.APPLICATION_NAME, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        returnValue = false;
      }

      m_Application.EndWaitCursor();
      m_Application.SetStatusBarText(GTStatusPanelConstants.gtaspcMessage, "");

      return returnValue;
    }

    // Check if passed in cell range is red. Unable to check interior color of cell since color for the
    // validation cells are set by conditional formatting. Need to evaluate each formatting condition
    // to see if it is true.
    private bool InvalidConfiguration(Excel.Range range)
    {
      bool formatCondition = false;
      int index = 1;
      Excel.FormatCondition fc;
      bool test = true;
      int check = 0;

      if(range.FormatConditions.Count == 0)
      {
        formatCondition = false;
      }
      else
      {
        for(index = 1;index <= range.FormatConditions.Count;index++)
        {
          test = true;

          try
          {
            fc = range.FormatConditions[index];
            // Only evaluate rule if defined interior color is red
            // since these are the only ones that should raise an error.
            if(fc.Interior.Color == 255)
            {
              if(fc.Type == (int)Excel.XlFormatConditionType.xlCellValue)
              {
                try
                {
                  check = fc.Operator;
                  if(Convert.ToString(range.Value).Length > 0)
                  {
                    switch(fc.Operator)
                    {
                      case (int)Excel.XlFormatConditionOperator.xlEqual:
                        test = range.Value = m_xlWorkbook.Application.Evaluate(fc.Formula1);
                        break;
                      case (int)Excel.XlFormatConditionOperator.xlNotEqual:
                        test = range.Value != m_xlWorkbook.Application.Evaluate(fc.Formula1);
                        break;
                      case (int)Excel.XlFormatConditionOperator.xlGreater:
                        test = range.Value > m_xlWorkbook.Application.Evaluate(fc.Formula1);
                        break;
                      case (int)Excel.XlFormatConditionOperator.xlGreaterEqual:
                        test = range.Value >= m_xlWorkbook.Application.Evaluate(fc.Formula1);
                        break;
                      case (int)Excel.XlFormatConditionOperator.xlLess:
                        test = range.Value < m_xlWorkbook.Application.Evaluate(fc.Formula1);
                        break;
                      case (int)Excel.XlFormatConditionOperator.xlLessEqual:
                        test = range.Value <= m_xlWorkbook.Application.Evaluate(fc.Formula1);
                        break;
                      case (int)Excel.XlFormatConditionOperator.xlBetween:
                        test = range.Value >= m_xlWorkbook.Application.Evaluate(fc.Formula1) && range.Value <= m_xlWorkbook.Application.Evaluate(fc.Formula1);
                        break;
                      case (int)Excel.XlFormatConditionOperator.xlNotBetween:
                        test = range.Value < m_xlWorkbook.Application.Evaluate(fc.Formula1) || range.Value > m_xlWorkbook.Application.Evaluate(fc.Formula1);
                        break;
                      default:
                        break;
                    }
                    if(test)
                    {
                      return true;
                    }
                    else
                    {
                      return false;
                    }
                  }
                }
                catch
                {
                  if(m_xlWorkbook.Application.Evaluate(fc.Formula1))
                  {
                    return true;
                  }
                }
              }
              else
              {
                if(m_xlWorkbook.Application.Evaluate(fc.Formula1))
                {
                  return true;
                }
              }
            }

          }
          catch
          {
            // ignore error
            // certain format conditions like Graded Color Scale throw errors.
          }
        }
      }

      return formatCondition;
    }
  }
}
