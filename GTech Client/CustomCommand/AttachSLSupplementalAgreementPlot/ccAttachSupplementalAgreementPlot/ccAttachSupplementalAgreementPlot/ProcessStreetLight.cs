using System;
using ADODB;
using Intergraph.GTechnology.API;
using System.IO;

namespace GTechnology.Oncor.CustomAPI
{
    public class ProcessStreetLight
    {
        IGTDataContext m_gTDataContext;
        IGTApplication m_gTApplication;
        public IGTKeyObject m_gTDesignAreaKeyObject = null;
        public string m_strPlotAttachmentName = "";
        string m_strDoumentsPath;
        IGTTransactionManager m_oGTTransactionManager;

        public ProcessStreetLight(IGTApplication gTApplication, IGTTransactionManager gTTransactionManager)
        {
            m_gTApplication = gTApplication;
            m_gTDataContext = gTApplication.DataContext;
            m_oGTTransactionManager = gTTransactionManager;
        }

        /// <summary>
        /// Determine name of the Street Light supplemental plot attachment which is specified as a custom general parameter 
        /// </summary>
        /// <returns></returns>
        private void GetPlotAttachmentName()
        {
            Recordset rs = null;            
            try
            {
                rs = m_gTDataContext.OpenRecordset("select PARAM_VALUE from SYS_GENERALPARAMETER where PARAM_NAME=:1", CursorTypeEnum.adOpenStatic,
                               LockTypeEnum.adLockReadOnly, (int)CommandTypeEnum.adCmdText, "StltSplmnt_AttachName");

                if (rs != null && rs.RecordCount > 0)
                {
                    rs.MoveFirst();

                    m_strPlotAttachmentName = rs.Fields[0].Value.ToString();
                }

            }
            catch
            {
                throw;
            }
            finally
            {
                rs.Close();
                rs = null;
            }
        }

        /// <summary>
        /// Get Design Area KeyObject of current Job.
        /// </summary>
        /// <returns></returns>
        private IGTKeyObject GetDesignAreaKeyObject()
        {
            string sql = "SELECT G3E_FID FROM DESIGNAREA_P WHERE JOB_ID=:1";
            int outRecords = 0;
            Recordset rsDesignArea = null;

            try
            {

                rsDesignArea = m_gTDataContext.Execute(sql, out outRecords, (int)CommandTypeEnum.adCmdText, m_gTDataContext.ActiveJob);

                if (rsDesignArea.RecordCount > 0)
                {
                    rsDesignArea.MoveFirst();
                    return (m_gTDataContext.OpenFeature(8100, Convert.ToInt32(rsDesignArea.Fields[0].Value)));
                }
            }
            catch
            {
                throw;
            }
            finally
            {
                rsDesignArea.Close();
                rsDesignArea = null;
            }

            return null;
        }

        /// <summary>
        /// Check if file name is already attached to the active job.
        /// </summary>
        /// <param name="strFilename">Html file name</param>
        /// <returns></returns>
        public bool IsExistingAttachment()
        {
            string sql = "";
            Recordset rs = null;
            try
            {                
                m_gTDesignAreaKeyObject = GetDesignAreaKeyObject();
                if (m_gTDesignAreaKeyObject != null)
                {
                    GetPlotAttachmentName();
                    m_strDoumentsPath = Path.GetTempPath();
                    m_strDoumentsPath = m_strDoumentsPath + m_strPlotAttachmentName + ".pdf";

                    sql = "select count(*) from JOB_HYPERLINK_N where g3e_fid=:1 and HYPERLINK_T=:2 and TYPE_C='SUPPLEPLOT'";
                    rs = m_gTDataContext.OpenRecordset(sql, ADODB.CursorTypeEnum.adOpenDynamic, ADODB.LockTypeEnum.adLockReadOnly,
                       (int)ADODB.CommandTypeEnum.adCmdText, m_gTDesignAreaKeyObject.FID, m_strDoumentsPath);

                    if (rs != null && rs.RecordCount > 0)
                    {
                        rs.MoveFirst();
                        if (Convert.ToInt32(rs.Fields[0].Value) > 0)
                        {
                            return true;
                        }
                    }
                }
            }
            catch
            {
                throw;
            }
            finally
            {
                if(rs!=null) rs.Close();
                rs = null;
            }

            return false;
        }

        /// <summary>
        /// Attach Plot to Current Active WR.
        /// </summary>
        public void AttachPlot()
        {
            IGTPlotWindow gTPlotWindow = null;
            try
            {
                gTPlotWindow = m_gTApplication.ActivePlotWindow;
                ExportToPDF(gTPlotWindow);
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Delete Existing Attachment.
        /// </summary>
        /// <param name="strFilename">Html file name</param>
        /// <returns></returns>
        public void DeleteExistingAttachment()
        {
            string sql = "";
            int iRecordsAffected = 0;
            try
            {
                sql = "delete from JOB_HYPERLINK_N where g3e_fid=:1 and HYPERLINK_T=:2";
                m_gTDataContext.Execute(sql, out iRecordsAffected, (int)CommandTypeEnum.adCmdText, m_gTDesignAreaKeyObject.FID, m_strDoumentsPath);
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Method to Export the given plot window to PDF 
        /// </summary>
        /// <param name="pw"></param>
        private void ExportToPDF(IGTPlotWindow pw)
        {
            IGTExportService svcExport = null;
            IGTPDFPrinterProperties printProps = null;

            try
            {
              
                // Construct printer properties
                PageOrientationType orientation =
                    (pw.NamedPlot.PaperWidth > pw.NamedPlot.PaperHeight)
                    ? PageOrientationType.Portrait : PageOrientationType.Landscape;

                printProps = GTClassFactory.Create<IGTPDFPrinterProperties>();
                printProps.PageWidth = pw.NamedPlot.PaperWidth;
                printProps.PageHeight = pw.NamedPlot.PaperHeight;
                printProps.Orientation = orientation;
                printProps.PageSize = PageSizeValue.Auto;
                printProps.Resolution = ResolutionValue.DPI600;

                // Perform export
                svcExport = GTClassFactory.Create<IGTExportService>();
                svcExport.PDFLayersEnabled = false;
                svcExport.SaveAsPDF(m_strDoumentsPath, printProps, pw, true);

                m_oGTTransactionManager.Begin("Attach Street Light Supplemental Agreement Plot");

                IGTKeyObject gTTempKeyObject = m_gTDataContext.OpenFeature(m_gTDesignAreaKeyObject.FNO, m_gTDesignAreaKeyObject.FID);
                Recordset rs = gTTempKeyObject.Components.GetComponent(8130).Recordset;

                rs.AddNew("G3E_FID", gTTempKeyObject.FID);
                rs.Fields["HYPERLINK_T"].Value = m_strDoumentsPath;
                rs.Fields["DESCRIPTION_T"].Value = "Street Supplemental Plot";
                rs.Fields["TYPE_C"].Value = "SUPPLEPLOT";
                rs.Fields["G3E_FNO"].Value = 8100;
                rs.Update();
                if (m_oGTTransactionManager.TransactionInProgress) m_oGTTransactionManager.Commit();
            }
            catch
            {
                throw;
            }
            finally
            {
                svcExport = null;
                printProps = null;
                pw = null;
            }

        }
    }
}
