//----------------------------------------------------------------------------+
//        Class: ccAttachSiteDrawing
//  Description: This command allows a user to generate a PDF from an open plot window and attach it to a selected Permit or Easement feature.
//----------------------------------------------------------------------------+
//     $Author:: kappana                                                       $
//       $Date:: 12/12/17                                                     $
//   $Revision:: 1                                                            $
//----------------------------------------------------------------------------+
//    $History:: ccAttachSiteDrawing.cs                                           $
// 
// *****************  Version 1  *****************
// User: kappana     Date: 12/12/17    Time: 18:00  Desc : Created
// build to D:\Dicks\Oncor\VSTSRoot\oncor-gtech\assemblies\custom
//----------------------------------------------------------------------------+

using System;
using Intergraph.GTechnology.API;
using Intergraph.GTechnology.Interfaces;
using System.Windows.Forms;
using System.IO;
using ADODB;


namespace GTechnology.Oncor.CustomAPI
{
	class ccAttachSiteDrawing : IGTCustomCommandModal
	{
		#region Variables
		private IGTApplication m_igtApplication;
		protected IGTTransactionManager m_igtTransactionManage;
		private int m_selectedFID;
		private short m_selectedFNO;
		private String m_plotwindowPdfFilename;
		private String m_plotwindowPath = Path.GetTempPath();
		#endregion

		#region Properities
		public IGTTransactionManager TransactionManager
		{
			set { m_igtTransactionManage = value; }
		}
		#endregion

		#region IGTCustomCommandModal Methods
		public void Activate()
		{
			IGTPlotWindow igtPlotWindow = null;
			IGTExportService igtExportService=null;
			IGTPDFPrinterProperties igtPDFPrinterProperties = null;
            OncDocManage.OncDocManage tmpOncDocMgr = new OncDocManage.OncDocManage();
            

			try
			{
				m_igtApplication = GTClassFactory.Create<IGTApplication>();
				igtPlotWindow = m_igtApplication.ActivePlotWindow;
				igtExportService = GTClassFactory.Create<IGTExportService>();
				igtPDFPrinterProperties = GTClassFactory.Create<IGTPDFPrinterProperties>();
				m_igtApplication.SetStatusBarText(GTStatusPanelConstants.gtaspcMessage, "Attach Site Drawing to selected feature started");
              
				if (Validate())
				{

					m_plotwindowPdfFilename = m_plotwindowPath + igtPlotWindow.Caption + ".pdf";
					m_igtApplication.BeginWaitCursor();

                    setPDFPrinterProps(ref igtPDFPrinterProperties);

                    igtExportService.SaveAsPDF(m_plotwindowPdfFilename, igtPDFPrinterProperties, igtPlotWindow, false);
                    igtExportService = null;
                    igtPDFPrinterProperties = null;
                    igtPlotWindow = null;
					//m_igtApplication.EndWaitCursor();
					if (File.Exists(m_plotwindowPdfFilename))
					{
                        if (GetGeneralParams(ref tmpOncDocMgr))
                        {
                            m_igtApplication.SetStatusBarText(GTStatusPanelConstants.gtaspcMessage, "Adding the Site Drawing PDF file to SharePoint.");
                            tmpOncDocMgr.SrcFilePath = m_plotwindowPdfFilename;
                            tmpOncDocMgr.SPFileName = m_plotwindowPdfFilename.Substring(m_plotwindowPdfFilename.LastIndexOf("\\")+1);
                            tmpOncDocMgr.SPFileType = "Site Drawings";
                            tmpOncDocMgr.WrkOrd_Job = m_igtApplication.DataContext.ActiveJob;

                            if (tmpOncDocMgr.AddSPFile(true))
                            {
                                //add to hyperlink to selected object
                                //AddHyperLinktoSelectedFeature(m_selectedFNO, m_selectedFID, m_plotwindowPdfFilename, "Plotwindow " + m_igtApplication.ActivePlotWindow.Caption + " pdf", "SITE");
                                AddHyperLinktoSelectedFeature(m_selectedFNO, m_selectedFID,
                                                             tmpOncDocMgr.RetFileURL,tmpOncDocMgr.RetFileName,
                                                             "Plotwindow " + m_igtApplication.ActivePlotWindow.Caption + " pdf", tmpOncDocMgr.SPFileType);

                            }
                            else
                            {
                                MessageBox.Show("Error during while trying to add the file to SharePoint. Error: " + tmpOncDocMgr.RetErrMessage, "G/Technology", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                        }
					}
				}
				else
				{
					MessageBox.Show(m_igtApplication.ApplicationWindow,"Ad hoc plots may only be attached to a Permit or Easement feature.", "G/Technology", MessageBoxButtons.OK, MessageBoxIcon.Warning);
				}
                m_igtApplication.SetStatusBarText(GTStatusPanelConstants.gtaspcMessage, "Attach Site Drawing Command has ended.");
                m_igtApplication.EndWaitCursor();
			}
			catch (Exception ex)
			{
				m_igtApplication.EndWaitCursor();
				MessageBox.Show("Error during execution of Attach Site Drawing custom command" + ex.Message, "G/Technology", MessageBoxButtons.OK, MessageBoxIcon.Error);
                m_igtApplication.SetStatusBarText(GTStatusPanelConstants.gtaspcMessage, "Attach Site Drawing Command has ended with errors.");
            }
		}

		#endregion

		#region Private Methods

		/// <summary>
		/// Validating Custom Command Conditions Selected feature is neither Permit nor Easement
		/// </summary>
		private bool Validate()
		{
			bool isValidation = false;
			IGTDDCKeyObject m_DDCSelectedKeyFeat = null;
			try
			{
				m_DDCSelectedKeyFeat = m_igtApplication.SelectedObjects.GetObjects()[0];
				if (m_DDCSelectedKeyFeat.FNO == 226 || m_DDCSelectedKeyFeat.FNO == 220)
				{
					isValidation = true;
					m_selectedFID = m_DDCSelectedKeyFeat.FID;
					m_selectedFNO = m_DDCSelectedKeyFeat.FNO;
				}
				return isValidation;
			}
			catch (Exception)
			{
				throw;
			}
			finally
			{
				if (m_DDCSelectedKeyFeat != null) m_DDCSelectedKeyFeat.Dispose(); m_DDCSelectedKeyFeat = null;
			}
		}


		/// <summary>
		/// Command to add hyperlink entry to feature
		/// </summary>
		/// <param name="p_selectedFNO"></param>
		/// <param name="p_selectedFID"></param>
		/// <param name="p_plotwindowPdfFilename"></param>
		/// <param name="p_description"></param>
		/// <param name="p_type"></param>
		private void AddHyperLinktoSelectedFeature(short p_selectedFNO, int p_selectedFID, string p_hyperLnk,string p_filename, string p_description, string p_type)
		{
			IGTKeyObject ddcSelectedKeyObj;
            Recordset rsHypLnkComp = null;
            IGTKeyObject tmpSelFeature = null;

            try
			{
                tmpSelFeature = m_igtApplication.DataContext.OpenFeature(p_selectedFNO, p_selectedFID);
                rsHypLnkComp = tmpSelFeature.Components["HYPERLINK_N"].Recordset;
                rsHypLnkComp.Filter = "HYPERLINK_T = '" + p_hyperLnk + "'";

                if (rsHypLnkComp.RecordCount == 0)
                {
                    tmpSelFeature = null;
                    m_igtTransactionManage.Begin("IN PROGRESS");
                    ddcSelectedKeyObj = m_igtApplication.DataContext.OpenFeature(p_selectedFNO, p_selectedFID);
                    ddcSelectedKeyObj.Components["HYPERLINK_N"].Recordset.AddNew("G3E_FID", p_selectedFID);
                    ddcSelectedKeyObj.Components["HYPERLINK_N"].Recordset.Fields["HYPERLINK_T"].Value = p_hyperLnk;
                    ddcSelectedKeyObj.Components["HYPERLINK_N"].Recordset.Fields["DESCRIPTION_T"].Value = p_description;
                    if (p_type.Length > ddcSelectedKeyObj.Components["HYPERLINK_N"].Recordset.Fields["TYPE_C"].DefinedSize)
                    {
                        ddcSelectedKeyObj.Components["HYPERLINK_N"].Recordset.Fields["TYPE_C"].Value = 
                            p_type.Substring(0, ddcSelectedKeyObj.Components["HYPERLINK_N"].Recordset.Fields["TYPE_C"].DefinedSize);
                    }
                    else
                    {
                        ddcSelectedKeyObj.Components["HYPERLINK_N"].Recordset.Fields["TYPE_C"].Value = p_type;
                    }
                    ddcSelectedKeyObj.Components["HYPERLINK_N"].Recordset.Fields["FILENAME_T"].Value = p_filename;
                    ddcSelectedKeyObj.Components["HYPERLINK_N"].Recordset.Update("G3E_FNO", p_selectedFNO);
                    if (m_igtTransactionManage.TransactionInProgress) m_igtTransactionManage.Commit();
                    m_igtApplication.SetStatusBarText(GTStatusPanelConstants.gtaspcMessage, "Site drawing has been attached to the selected " + p_selectedFID + " feature.");
                }
                else
                {
                    tmpSelFeature = null;
                }
            }
			catch (Exception)
			{
				if (m_igtTransactionManage.TransactionInProgress) m_igtTransactionManage.Rollback();
				throw;
			}
			finally
			{
				ddcSelectedKeyObj = null;
			}
		}

        private Boolean GetGeneralParams(ref OncDocManage.OncDocManage objDocMng)
        {
            Boolean bRetVal = false;
            Recordset tmpRs;
            string tmpQry = "select SUBSYSTEM_NAME,SUBSYSTEM_COMPONENT,PARAM_NAME,PARAM_VALUE " +
                                "from GIS_ONC.SYS_GENERALPARAMETER " +
                                "where SUBSYSTEM_NAME = 'Doc_Management'"; 
            try
            {
               tmpRs = m_igtApplication.DataContext.OpenRecordset(tmpQry, CursorTypeEnum.adOpenStatic,
                                                                   LockTypeEnum.adLockReadOnly, 
                                                                   (int)CommandTypeEnum.adCmdText);
                if (! (tmpRs.EOF && tmpRs.EOF))
                {
                    tmpRs.MoveFirst();
                    while (! tmpRs.EOF)
                    {
                        if (tmpRs.Fields["PARAM_NAME"].Value.ToString() == "SP_URL" )
                        {
                            objDocMng.SPSiteURL = tmpRs.Fields["PARAM_VALUE"].Value.ToString();
                        }
                        if (tmpRs.Fields["PARAM_NAME"].Value.ToString() == "JOBWO_REL_PATH")
                        {
                            objDocMng.SPRelPath = tmpRs.Fields["PARAM_VALUE"].Value.ToString();
                        }
                        if (tmpRs.Fields["PARAM_NAME"].Value.ToString() == "ROOT_PATH")
                        {
                            objDocMng.SPRootPath = tmpRs.Fields["PARAM_VALUE"].Value.ToString();
                        }
                        tmpRs.MoveNext();
                    }
                    bRetVal = true;
                }

            }
            catch(Exception)
            {
                bRetVal = false;
                throw;
            }
            return bRetVal;
        }
        private void setPDFPrinterProps(ref IGTPDFPrinterProperties tmpPrintProps)
        {
            IGTNamedPlot tmpNamedPlt = null;
            try
            {
                tmpNamedPlt = m_igtApplication.ActivePlotWindow.NamedPlot;
                tmpPrintProps.PageHeight = tmpNamedPlt.PaperHeight;
                tmpPrintProps.PageWidth = tmpNamedPlt.PaperWidth;
                if (tmpNamedPlt.PaperHeight > tmpNamedPlt.PaperWidth)
                {
                    tmpPrintProps.Orientation = PageOrientationType.Portrait;
                }
                else
                {
                    tmpPrintProps.Orientation = PageOrientationType.Landscape;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error in ccAttachSiteDrawing.setPDFPrinterProps. Error: " + ex.Message,
                                "setPDFPrinterProps Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
		#endregion
	}
}
