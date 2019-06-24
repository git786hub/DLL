using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Diagnostics;
using Microsoft.SharePoint.Client;
using System.Windows.Forms;
using System.Configuration;


namespace OncDocManage
{
  public class OncDocManage
  {
    public String WrkOrd_Job = String.Empty; /// <summary> /// Work order or Job name used to build or find subdirectory in Job Documents folder in SharePoint/// </summary> /// <returns></returns>
    public String SPSiteURL = String.Empty; /// <summary> /// SharePoint Site URL/// </summary> /// <returns></returns>
    public String SrcFilePath = String.Empty; /// <summary> ///Path on the local machine to the file to be added to SharePoint. (File included in the Path)/// </summary> /// <returns></returns>
    private String vSPRelPath = String.Empty; /// <summary> /// Local variable that has been formated correctly using the SPRelPath as the source./// </summary> /// <returns></returns>
    public String SPFileName = String.Empty; /// <summary> /// The file name as it will be added or retrieved to/from SharePoint./// </summary> /// <returns></returns>
    public String SPFileType = String.Empty; /// <summary> /// SharePoint Attribute "Type" that describes file catagory. /// </summary> /// <returns></returns>
    public String SPFileDescription = String.Empty; /// <summary> /// SharePoint Description Attribute value for the file./// </summary> /// <returns></returns>
    public String RetFileName = String.Empty; /// <summary> /// The name of the file that SharePoint has just added./// </summary> /// <returns></returns>
    public String RetFileURL = String.Empty; /// <summary> /// Full URL to the file in SharePoint for the file that was just added or replaced/// </summary> /// <returns></returns>
    public String RetErrMessage = String.Empty; /// <summary> /// Error message that is returned from any function./// </summary> /// <returns></returns>
    public String SPRootPath = String.Empty; /// <summary> /// SharePoint Root folder for the site./// </summary> /// <returns></returns>
    private Boolean FileOverwrite = false; /// <summary> /// Local variable to determine if a files should be replaced or not./// </summary> /// <returns></returns>
    private System.IO.FileStream fs = null;

    private static Configuration oncConfig = ConfigurationManager.OpenExeConfiguration(getThisAssemblyPath());

    private static String typFldName = oncConfig.AppSettings.Settings["SPDocumentTypeFieldName"].Value.ToString();
    private static String DescFldName = oncConfig.AppSettings.Settings["SPDescriptionFieldName"].Value.ToString();

    //string typFldName = Properties.Settings.Default["SPDocumentTypeFieldName"].ToString();
    //string DescFldName = Properties.Settings.Default["SPDescriptionFieldName"].ToString();
    public String SPRelPath /// <summary> /// SharePoint relative path based on the root path/// </summary> /// <returns></returns>
    {
      set
      {   // If the path contains spaces replace the spaces with "%20" so that SharePoint will except the url.
        if(value.Contains(" "))
        {
          vSPRelPath = value.Replace(" ", "%20");
        }
        else
        {
          vSPRelPath = value;
        }
      }
      get  // return the properly formatted path.
      {
        return vSPRelPath;
      }
    }
    /// <summary>
    /// This method adds a file to the SharePoint site. 
    /// 
    /// Please refer to the checkForReqProps metod for required properties.
    /// 
    /// In the documentation that I found the max size of the document can be up to 3.2 meg.
    /// </summary>
    /// <param name="Interactive"></param>
    /// <returns></returns>
    public Boolean AddSPFile(bool Interactive)
    {
      Boolean tmpRetVal = true;
      ClientContext ctx = null;
      Web tmpWeb = null;
      //FileCreationInformation fcInfo = null;
      String tmpRelPath = String.Empty;
      Folder tmpFolder = null;
      String tmpPropList = String.Empty;
      System.Windows.Forms.Form frmReNm = new frmRenameFile();
      DialogResult tmpResult;

      //string typFldName = Properties.Settings.Default["SPDocumentTypeFieldName"].ToString();
      //string DescFldName = Properties.Settings.Default["SPDescriptionFieldName"].ToString();

      try
      {
        // Check to make sure all the required properties are set.
        if(FileOverwrite == false)
        {
          tmpPropList = checkForReqProps("AddFile");
        }
        else
        {  // If FileOverwrite is false then the AddSPFile was called by ReplaceSPFile method.
          tmpPropList = checkForReqProps("ReplaceFile");
        }


        if(tmpPropList != String.Empty)
        {
          tmpRetVal = false;

          RetErrMessage = "The following Properties must be set:  " + tmpPropList;
          return tmpRetVal;
        }
        // See if the desired source file exists.
        if(System.IO.File.Exists(SrcFilePath))
        {
          if(SPFileName == String.Empty) SPFileName = SrcFilePath.Substring(SrcFilePath.LastIndexOf("\\") + 1); // get the SharePoint file name from the SrcFilePath.
          if(AddFolderIfNeeded() == false)
          {
            tmpRetVal = false;
            RetErrMessage = "Could not add the folder: " + SPRelPath;
            return tmpRetVal;
          }
          // Check to see if the file already exists in SharePoint location.
          if(SPFileExists())
          {
            if(Interactive == true)
            {
              // If it exists in SharePoint, give the user a chance to change the name of the file
              //   to be saved in SharePoint.

              // Open dialog here to get new file name
              frmReNm.Controls["lblCurFileName"].Text = frmReNm.Controls["lblCurFileName"].Text + SPFileName;
              frmReNm.Controls["txtNewName"].Text = SPFileName;
              tmpResult = frmReNm.ShowDialog();

              switch(tmpResult)
              {
                case DialogResult.OK:
                  if(frmReNm.Controls["cbReplcRenam"].Text == "Replace") FileOverwrite = true;
                  // Check to see if the user has changed the file name. If changed...
                  if(frmReNm.Controls["txtNewName"].Text !=
                          frmReNm.Controls["lblCurFileName"].Text.Substring(frmReNm.Controls["lblCurFileName"].Text.LastIndexOf(": ") + 1)
                      )
                  {
                    SPFileName = frmReNm.Controls["txtNewName"].Text;
                    if(SPFileExists() && FileOverwrite == false) // check to see if the new file name exists in SharePoint.
                    {
                      tmpRetVal = false;
                      MessageBox.Show("The file " + SPFileName + " already exists in SharePoint. \n Try again.", "File Already Exists", MessageBoxButtons.OK);
                      RetErrMessage = "File already exists in SharePoint. " + SPRelPath + "/" + frmReNm.Controls["txtNewName"].Text;
                    }
                  }
                  else
                  {
                    tmpRetVal = false;
                    RetErrMessage = "File already exists in SharePoint. " + SPRelPath + "/" + frmReNm.Controls["txtNewName"].Text;
                  }
                  break;
                case DialogResult.Cancel:
                  tmpRetVal = false;
                  RetErrMessage = "File already exists in SharePoint. " + SPRelPath + "/" + SPFileName;
                  break;
              }

              frmReNm.Dispose();
            }
            else
            {  // Not in interactive mode and the file exists in SharePoint
              if(FileOverwrite == false)
              {
                tmpRetVal = false;
                RetErrMessage = "File already exists in SharePoint. " + SPRelPath + "/" + SPFileName;
              }
            }
          }
          // If no errors, add the file.
          if(tmpRetVal == true)
          {
            tmpRelPath = SPRelPath + "/" + WrkOrd_Job;
            SPRelPath = tmpRelPath;

            ctx = new ClientContext(SPSiteURL);
            tmpWeb = ctx.Web;
            ctx.Load(tmpWeb);

            tmpFolder = tmpWeb.GetFolderByServerRelativeUrl(vSPRelPath);

            ctx.Load(tmpFolder, f => f.ServerRelativeUrl);
            ctx.ExecuteQuery();

            tmpRelPath = tmpFolder.ServerRelativeUrl;

            fs = new System.IO.FileStream(SrcFilePath, System.IO.FileMode.Open, System.IO.FileAccess.Read);

            ClientContext ctx1 = new ClientContext(SPSiteURL);
            Microsoft.SharePoint.Client.File.SaveBinaryDirect(ctx1, tmpRelPath + "/" + SPFileName, fs, FileOverwrite);
            Microsoft.SharePoint.Client.File tmpSPFile = ctx1.Web.GetFileByServerRelativeUrl(tmpRelPath + "/" + SPFileName);
            tmpSPFile.ListItemAllFields[typFldName] = SPFileType; // Needs to be modified for Oncor
            tmpSPFile.ListItemAllFields[DescFldName] = SPFileDescription; // Needs to be Modified for Oncor
            tmpSPFile.ListItemAllFields.Update();
            ctx1.Load(tmpSPFile);
            ctx1.ExecuteQuery();

            var tmpStr = GetCommonString(SPSiteURL, SPRelPath);
            if(tmpStr != string.Empty)
            {
              RetFileURL = SPSiteURL.Replace(tmpStr, "") + vSPRelPath + "/" + SPFileName;
            }
            else
            {
              RetFileURL = SPSiteURL + vSPRelPath + "/" + SPFileName;
            }
            RetFileName = SPFileName;
          }
        }
        else
        {
          RetErrMessage = "Source File not Found.";
          tmpRetVal = false;

        }
      }
      catch(Exception ex)
      {
        RetErrMessage = ex.Message;
        tmpRetVal = false;
      }

      if(null != fs)
      {
        fs.Dispose();
        fs = null;
      }

      return tmpRetVal;
    }


    /// <summary>
    /// This function replaces and existing file in SharePoint.
    /// 
    ///  Please refer to the checkForReqProps metod for required properties.
    ///  
    /// </summary>
    /// <returns></returns>
    public Boolean ReplaceSPFile()
    {
      Boolean tmpRetVal = true;
      try
      {
        // use the AddSPFile with the overwrite argument turned on to replace the file.
        FileOverwrite = true;
        tmpRetVal = AddSPFile(false);
      }
      catch(Exception ex)
      {
        RetErrMessage = ex.Message;
        tmpRetVal = false;

      }
      FileOverwrite = false;
      return tmpRetVal;
    }

    /// <summary>
    /// This function deletes a file from the designated folder in SharePoint.
    /// 
    ///  Please refer to the checkForReqProps metod for required properties.
    ///  
    /// </summary>
    /// <returns></returns>
    public Boolean DeleteSPFile()
    {
      Boolean tmpRetVal = true;
      ClientContext ctx = null;
      Web tmpWeb = null;
      String tmpRelPath = String.Empty;
      Microsoft.SharePoint.Client.File tmpfile = null;
      String tmpPropList = String.Empty;

      try
      {
        tmpPropList = checkForReqProps("DeleteFile");
        if(tmpPropList == String.Empty)
        {
          RetErrMessage = "The following Properties must be set: " + tmpPropList;
          tmpRetVal = false;
        }
        else
        {
          ctx = new ClientContext(SPSiteURL);
          tmpWeb = ctx.Web;
          tmpRelPath = SPRelPath + "/" + SPFileName;
          tmpRelPath = tmpRelPath.Replace(" ", "%20");
          tmpfile = tmpWeb.GetFileByServerRelativeUrl(tmpRelPath);

          ctx.Load(tmpfile);
          tmpfile.DeleteObject();
          ctx.ExecuteQuery();

          tmpRetVal = true;
        }
      }

      catch(Exception ex)
      {
        RetErrMessage = ex.Message;
        tmpRetVal = false;
      }
      return tmpRetVal;
    }

    /// <summary>
    /// This function adds a subfolder to the desiginated folder in SharePoint.
    /// 
    ///  Please refer to the checkForReqProps metod for required properties.
    ///  
    /// </summary>
    /// <param name="NewFolder"></param>
    /// <param name="ParentPath"></param>
    /// <returns></returns>
    public String AddSPFolder(string NewFolder, string ParentPath)
    {
      String tmpRetVal = String.Empty;
      ClientContext ctx = null;
      Web tmpWeb = null;
      List tmpList = null;
      ListCollection tmpLists;
      FolderCollection tmpFolders = null;
      Microsoft.SharePoint.Client.Folder tmpFolder = null;
      try
      {
        ctx = new ClientContext(SPSiteURL);
        tmpWeb = ctx.Web;
        tmpLists = tmpWeb.Lists;
        tmpList = tmpLists.GetByTitle(SPRootPath);

        if(tmpList != null)
        {
          tmpFolders = tmpWeb.GetFolderByServerRelativeUrl(ParentPath).Folders;

          ctx.Load(tmpFolders);
          ctx.ExecuteQuery();

          tmpFolder = tmpFolders.Add(NewFolder);
          tmpFolder.Update();
          ctx.ExecuteQuery();

          tmpRetVal = ParentPath + "/" + NewFolder;
        }
      }

      catch(Exception ex)
      {
        RetErrMessage = ex.Message;
        tmpRetVal = String.Empty;
      }
      return tmpRetVal;
    }
    internal ListItemCollection getFileListByType()
    {
      ClientContext ctx;
      Web tmpWeb;
      ListCollection tmpLists;
      List tmpONCLst;
      ListItemCollection tmpONCLstItems;
      CamlQuery tmpQry;
      string tmpListStr = string.Empty;
      string tmpStr = string.Empty;
      string tmpFilter = string.Empty;
      try
      {
        ctx = new ClientContext(SPSiteURL);

        tmpWeb = ctx.Web;
        tmpLists = tmpWeb.Lists;

        tmpONCLst = tmpLists.GetByTitle(SPRootPath);

        tmpQry = new CamlQuery();
        tmpQry.FolderServerRelativeUrl = vSPRelPath;
        //tmpQry.ViewXml = @" < View Scope = 'FilesOnly' >" +
        //                    "< Query >" +
        //                     "< Where >" +
        //                         "< FieldRef Name = 'DocType' />" +
        //                           "< Value Type = 'text' >" + SPFileType + "</ Value >" +
        //                        "</ Contains >" +
        //                     "</ Where >" +
        //                    "</ Query >" +
        //                  "</ View > ";

        tmpONCLstItems = tmpONCLst.GetItems(tmpQry);
        ctx.Load(tmpWeb);
        ctx.Load(tmpONCLstItems);
        ctx.ExecuteQuery();

        if(tmpONCLstItems.Count == 0)
        {
          RetErrMessage = "No Files Found.";
          return null;
        }
      }
      catch(Exception ex)
      {
        RetErrMessage = ex.Message;
        return null;
      }
      return tmpONCLstItems;
    }

    /// <summary>
    /// This function gets a list of files from the relative path by the 'Type' if file.
    ///     It then displays the list to the user in a dialog box. The user will then select the 
    ///     file to copy locally. When the clicks the ok button the selected file will be copied
    ///     to destination path (the argument to the function). Sets RetFileName property to the name
    ///     of the file it copied.
    ///  
    ///  Please refer to the checkForReqProps metod for required properties.
    ///  
    /// </summary>
    /// <param name="DestinationPath"></param>
    /// <returns></returns>
    public Boolean GetFileFromList(string DestinationPath)
    {
      Boolean tmpRetVal = true;
      ClientContext ctx = null;
      String tmpPropList = String.Empty;
      ListItemCollection tmpFileCollection = null;
      string tmpPath;
      string tmpPath2;
      String absUrl = string.Empty;
      String tmpStr = string.Empty;
      System.IO.FileStream OutFile;

      try
      {
        tmpPropList = checkForReqProps("GetFileFromList");
        if(tmpPropList != String.Empty)
        {
          RetErrMessage = "The following Properties must be set: " + tmpPropList;
          tmpRetVal = false;
        }
        else
        {
          tmpFileCollection = getFileListByType();
          // load the grid in form
          System.Windows.Forms.Form tmpForm = new frmSPFileToCopy();
          DataGridView tmpListbox = (DataGridView)tmpForm.Controls["dgvFilesToCopy"];
          foreach(ListItem tmpItem in tmpFileCollection)
          {
            //Debug.WriteLine("file: " + tmpItem["FileLeafRef"].ToString() + "DocType: " + tmpItem["DocType"].ToString());
            if(tmpItem[typFldName] != null && tmpItem[typFldName].ToString() == SPFileType)
            {
              if(tmpItem[DescFldName] == null)
              {
                tmpStr = " ";
              }
              else
              {
                tmpStr = tmpItem[DescFldName].ToString();
              }

              tmpListbox.Rows.Add(tmpItem["FileLeafRef"].ToString(), tmpStr);
            }
            if(tmpItem[typFldName] == null || SPFileType == string.Empty)
            {
              if(tmpItem[typFldName] == null)
              {
                tmpListbox.Rows.Add(tmpItem["FileLeafRef"].ToString(), tmpItem[DescFldName]);
              }
              else
              {
                tmpListbox.Rows.Add(tmpItem["FileLeafRef"].ToString(), tmpItem[DescFldName].ToString());
              }
            }
          }

          tmpForm.Text = SPFileType;

          // load the form
          DialogResult tmpDiagRes = tmpForm.ShowDialog();

          ctx = new ClientContext(SPSiteURL);

          tmpPath = vSPRelPath + "/" + tmpListbox.SelectedRows[0].Cells["colFile"].Value.ToString();
          tmpPath2 = tmpPath.Replace(" ", "%20");
          var infile = Microsoft.SharePoint.Client.File.OpenBinaryDirect(ctx, tmpPath2);
          OutFile = System.IO.File.Create(DestinationPath + "\\" + tmpListbox.SelectedRows[0].Cells["colFile"].Value.ToString());

          infile.Stream.CopyTo(OutFile);
          RetFileName = tmpListbox.SelectedRows[0].Cells["colFile"].Value.ToString();
          OutFile.Dispose();
          infile.Dispose();
          tmpForm.Dispose();
        }
      }
      catch(Exception ex)
      {
        tmpRetVal = false;
        RetErrMessage = ex.Message;
      }
      return tmpRetVal;
    }
    /// <summary>
    /// This function tests for the existance of a subfolder in the designated folder in SharePoint.
    /// </summary>
    /// <param name="tmpFolderName"></param>
    /// <param name="tmpRelPath"></param>
    /// <returns></returns>
    public Boolean FolderExists(string tmpFolderName, string tmpRelPath)
    {
      Boolean tmpRetVal = false;
      ClientContext ctx = null;
      Web tmpWeb = null;
      List tmpList = null;
      ListCollection tmpLists;
      FolderCollection tmpFolders = null;

      try
      {
        ctx = new ClientContext(SPSiteURL);
        tmpWeb = ctx.Web;
        tmpLists = tmpWeb.Lists;
        tmpList = tmpLists.GetByTitle(SPRootPath);

        if(tmpList != null)
        {
          tmpFolders = tmpWeb.GetFolderByServerRelativeUrl(tmpRelPath).Folders;
          ctx.Load(tmpFolders, fl => fl.Include(ct => ct.Name).Where(ct => ct.Name == tmpFolderName));
          ctx.ExecuteQuery();

          if(tmpFolders.Count() > 0)
          {
            tmpRetVal = true;
          }
        }
      }

      catch(Exception ex)
      {
        tmpRetVal = false;
        RetErrMessage = ex.Message;
      }
      return tmpRetVal;
    }

    /// <summary>
    /// This function tests for the existance of a file in a designated folder in SharePoint.
    /// </summary>
    /// <returns></returns>
    private Boolean SPFileExists()
    {
      Boolean tmpRetVal = false;
      ClientContext ctx = null;
      Web tmpWeb = null;
      String tmpFileUrl = String.Empty;
      Microsoft.SharePoint.Client.File tmpSPFile = null;

      try
      {

        ctx = new ClientContext(SPSiteURL);
        tmpWeb = ctx.Web;
        tmpFileUrl = SPRelPath + "/" + WrkOrd_Job + "/" + SPFileName;
        tmpSPFile = tmpWeb.GetFileByServerRelativeUrl(tmpFileUrl);
        ctx.Load(tmpSPFile);
        ctx.ExecuteQuery();
        if(tmpSPFile.Exists)
        {
          tmpRetVal = true;
        }
      }
      catch(Exception ex)
      {
        if(ex.GetBaseException().ToString().Contains("File Not Found."))
        {
          //RetErrMessage = "File not found in SharePoint.";
          tmpRetVal = false;
        }
        else
        {
          RetErrMessage = ex.Message;
          tmpRetVal = false;
        }

      }
      return tmpRetVal;
    }
    /// <summary>
    /// This function checks for the existance of a subfolder in a designated folder in SharePoint. 
    ///    If the folder does not exist, then the subfolder is created in the designated folder.
    /// </summary>
    /// <returns></returns>
    private Boolean AddFolderIfNeeded()
    {
      Boolean tmpRetVal = true;
      String lastDirInSPRelP = String.Empty;
      String ParentPath = String.Empty;
      String tmpNewFldPath = String.Empty;

      try
      {
        if(SPRelPath.Substring(vSPRelPath.LastIndexOf("/") + 1).Contains("."))
        {
          lastDirInSPRelP = SPRelPath.Substring(SPRelPath.LastIndexOf("/"));
        }
        //lastDirInSPRelP = SPRelPath;
        lastDirInSPRelP = WrkOrd_Job;
        ParentPath = SPRelPath;
        if(FolderExists(lastDirInSPRelP, ParentPath) == false)
        {
          tmpNewFldPath = AddSPFolder(lastDirInSPRelP, ParentPath);
        }

      }
      catch(Exception ex)
      {
        RetErrMessage = ex.Message;
        tmpRetVal = false;
      }
      return tmpRetVal;
    }

    public Boolean setTypeDescPropsOfFile()
    {
      Boolean tmpRetVal = true;
      Microsoft.SharePoint.Client.File tmpFile = null;
      ClientContext ctx = null;
      String tmpRelPath = String.Empty;
      String tmpFileType = String.Empty;
      String tmpFileDesc = string.Empty;

      try
      {
        if(SPFileType != String.Empty && SPFileDescription != String.Empty)
        {


          tmpFileType = SPFileType;
          tmpFileDesc = SPFileDescription;

          ctx = new ClientContext(SPSiteURL);
          using(ctx)
          {
            tmpRelPath = SPRelPath + "/" + SPFileName;
            tmpRelPath = tmpRelPath.Replace(" ", "%20");
            tmpFile = ctx.Web.GetFileByServerRelativeUrl(tmpRelPath);


            //var item = tmpFile.ListItemAllFields;
            //var fields = item.ParentList.Fields;

            //ctx.Load(tmpFile, f => f.ListItemAllFields);
            ctx.Load(tmpFile);
            //ctx.Load(fields, include => include.Include(f => f.Id, f => f.InternalName, f => f.Title));
            ctx.ExecuteQuery();

            if(tmpFileDesc != String.Empty)
            {
              tmpFile.ListItemAllFields[DescFldName] = tmpFileDesc;
            }
            if(tmpFileType != String.Empty)
            {
              tmpFile.ListItemAllFields[typFldName] = SPFileType;
            }

            //item.Update();
            tmpFile.Update();
            ctx.ExecuteQuery();
            //Debug.WriteLine(item["DocType"]);
          }
        }
      }
      catch(Exception ex)
      {
        tmpRetVal = false;
        RetErrMessage = ex.Message;
      }
      return tmpRetVal;
    }


    /// <summary>
    /// This function find the longest sub-string in FirstStr that is in common with
    ///   the same sub-string in SecondStr.
    ///   example: FirstStr -- https://share.intergraph.com/sgi/infr/UCServices
    ///           SecondStr -- /sgi/infr/UCServices/Oncor%20F2G%20Gap%20Analysis/F2G Project/Development/
    ///        return value -- /sgi/infr/UCServices
    /// </summary>
    /// <param name="FirstStr"></param>
    /// <param name="SecondStr"></param>
    /// <returns></returns>
    private string GetCommonString(String FirstStr, String SecondStr)
    {
      int c1 = 0;
      int c2 = 0;
      int c3 = 0;

      string tmpStr1 = string.Empty;
      string tmpStr2 = string.Empty;
      string tmpSub1 = string.Empty;
      string tmpSub2 = string.Empty;
      string Sub1 = string.Empty;
      string Sub2 = string.Empty;
      string common = string.Empty;
      string tmpComm = string.Empty;

      try
      {
        tmpStr2 = FirstStr;
        tmpStr1 = SecondStr;

        while(c1 < tmpStr2.Length)
        {
          for(int i = c1;i < tmpStr1.Length - 1;i++)
          {
            tmpSub1 = tmpStr1.Substring(i, 2);
            for(int j = c2;j < tmpStr2.Length - 1;j++)
            {
              tmpSub2 = tmpStr2.Substring(j, 2);
              if(tmpSub1 == tmpSub2)
              {
                c3 = 3;
                Sub1 = tmpSub1;
                Sub2 = tmpSub2;

                while(Sub1 == Sub2)
                {
                  if(i + c3 <= tmpStr1.Length && j + c3 <= tmpStr2.Length)
                  {
                    Sub1 = tmpStr1.Substring(i, c3);
                    Sub2 = tmpStr2.Substring(j, c3);
                  }
                  else
                  {
                    c2 = j + c3;
                    break;
                  }
                  if(Sub1 == Sub2)
                  {
                    tmpComm = Sub1;
                  }
                  c3++;
                }
                if(tmpComm.Length > common.Length)
                {
                  common = tmpComm;
                }
              }
            }
          }
          c1++;
        }

        if(common.Length < 3)
        {
          common = string.Empty;
        }
        //Debug.WriteLine(common);
        //tmpStr1 = tmpStr1.Replace(common, "");
        //Debug.WriteLine(tmpStr1);
      }
      catch(Exception ex)
      {
        RetErrMessage = ex.Message;
        common = "Failed: " + ex.Message;
      }
      return common;
    }

    /// <summary>
    /// Check to make all the required properties are set for each function.
    /// </summary>
    /// <param name="CallingFunc"></param>
    /// <returns></returns>
    private String checkForReqProps(String CallingFunc)
    {
      String tmpRetVal = String.Empty;
      String tmpInArg = String.Empty;
      String tmpPropList = String.Empty;

      try
      {
        tmpInArg = CallingFunc;

        if(SPSiteURL == String.Empty) tmpPropList = tmpPropList + "SPSiteURL,";
        //if (SPFileType == String.Empty) tmpPropList = tmpPropList + "SPFileType,";

        switch(tmpInArg)
        {
          case "AddFile":
            // Check to make all the required properties are set.
            if(SrcFilePath == String.Empty) tmpPropList = tmpPropList + " SrcFilePath,"; //Path on local machine to where the file is located including file.
            if(SPRelPath == String.Empty) tmpPropList = tmpPropList + " SPRelPath"; //Relative path to the base Job Documents folder.
            if(WrkOrd_Job == String.Empty) tmpPropList = tmpPropList + " WrkOrd_Job"; //The name of the workorder or job subfolder.
            if(SPRootPath == String.Empty) tmpPropList = tmpPropList + "SPRootPath"; // Name of root Site.
            break;
          case "ReplaceFile":
            if(SrcFilePath == String.Empty) tmpPropList = tmpPropList + " SrcFilePath,";
            if(SPRelPath == String.Empty) tmpPropList = tmpPropList + " SPRelPath,";
            if(SPFileName == String.Empty) tmpPropList = tmpPropList + " SPFileName,";
            if(SPRootPath == String.Empty) tmpPropList = tmpPropList + "SPRootPath";
            break;
          case "DeleteFile":
            if(SrcFilePath == String.Empty) tmpPropList = tmpPropList + " SrcFilePath,";
            if(SPRelPath == String.Empty) tmpPropList = tmpPropList + " SPRelPath,";
            if(SPFileName == String.Empty) tmpPropList = tmpPropList + " SPFileName,";
            if(SPRootPath == String.Empty) tmpPropList = tmpPropList + "SPRootPath";
            break;
          case "GetFileFromList":
            if(SPSiteURL == String.Empty) tmpPropList = tmpPropList + " SPSiteURL, ";
            if(SPRelPath == String.Empty) tmpPropList = tmpPropList + " SPRelPath, ";
            if(SPRootPath == String.Empty) tmpPropList = tmpPropList + "SPRootPath";
            // if (SPFileType == String.Empty) tmpPropList = tmpPropList + "SPFileType,";
            break;
          default:
            break;

        }

        tmpRetVal = tmpPropList;

      }
      catch(Exception ex)
      {
        RetErrMessage = "checkForReqProps failed: " + ex.Message;
        tmpRetVal = RetErrMessage;
      }
      return tmpRetVal;
    }
    private static string getThisAssemblyPath()
    {
      string tmpPath = string.Empty;
      tmpPath = System.Reflection.Assembly.GetAssembly(typeof(OncDocManage)).Location;

      return tmpPath;
    }
  }
}
