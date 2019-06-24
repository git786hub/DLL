using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using EdgeFrontierDummy.Models;

namespace EdgeFrontierDummy.Controllers
{
    [RoutePrefix("EFDummy")]
    public class EdgeFrontierController : ApiController
    {
        [HttpPost]
        [Route("Test")]
        public TestingReturn test([FromBody]Parameters parameters)
        {
            TestingReturn testingReturn = new TestingReturn();

            if (parameters.reportType == null)
            {
                testingReturn.status = "Fail";
            }
            else
            {
                testingReturn.status = "Success";
            }

            testingReturn.id = "10101010";
            //testingReturn.id = parameters.mapID;

            //System.Threading.Thread.Sleep(4000);

            return testingReturn;
        }

        [Route("GIS_WMIS_CreateWR")]
        public WMISReturn GIS_WMIS_CreateWR([FromBody]Input parameters)
        {
            WMISReturn returnData = new WMISReturn();

            if (parameters.RequestType == "")
            {
                //Fail
                returnData.RequestType = "GIS_WMIS_CreateWR";
                returnData.Status = "FAILURE";
                returnData.G3E_IDENTIFIER = parameters.RequestData.G3E_IDENTIFIER;
                returnData.Error = "Error message passed back from WMIS";
            }
            else
            {
                //Success
                returnData.RequestType = "GIS_WMIS_CreateWR";
                returnData.Status = "SUCCESS";
                returnData.WR_Name = parameters.RequestData.WR_Name;
                returnData.WR_Number = "5551515";
            }

            return returnData;
        }

        [Route("GIS_WMIS_GetCUCatalog")]
        public WMISReturn GIS_WMIS_GetCUCatalog([FromBody]Input parameters)
        {
            WMISReturn returnData = new WMISReturn();

            if (parameters.RequestType == "")
            {
                //Fail
                returnData.RequestType = "GIS_WMIS_GetCUCatalog";
                returnData.Status = "FAILURE";
                returnData.CUCode = parameters.RequestData.CUCode;
                returnData.Error = "Specified CU code was not found.";
            }
            else
            {
                //Success
                returnData.RequestType = "GIS_WMIS_GetCUCatalog";
                returnData.Status = "SUCCESS";
                returnData.CatalogData = null;
            }

            return returnData;
        }

        [Route("GIS_WMIS_GetMUCataLog")]
        public WMISReturn GIS_WMIS_GetMUCataLog([FromBody]Input parameters)
        {
            WMISReturn returnData = new WMISReturn();

            if (parameters.RequestType == "")
            {
                //Fail
                returnData.RequestType = "GIS_WMIS_GetMUCataLog";
                returnData.Status = "FAILURE";
                returnData.CUCode = parameters.RequestData.CUCode;
                returnData.Error = "Specified CU code was not found.";
            }
            else
            {
                //Success
                returnData.RequestType = "GIS_WMIS_GetMUCataLog";
                returnData.Status = "SUCCESS";
                returnData.CatalogData = null;
            }

            return returnData;
        }

        [Route("GIS_WMIS_GetReports")]
        public WMISReturn GIS_WMIS_GetReports([FromBody]Input parameters)
        {
            WMISReturn returnData = new WMISReturn();

            if (parameters.RequestType == "")
            {
                //Fail
                returnData.RequestType = "GIS_WMIS_GetMUCataLog";

                for (int i = 0; i < parameters.ReportList.Count; i++)
                {
                    ReportList rl = parameters.ReportList[i];
                    Reports report = new Reports();
                    report.Status = "FAILURE";
                    report.ReportType = rl.Name;
                    report.Error = "Error generating report.";
                    returnData.Reports.Add(report);
                }

            }
            else
            {
                //Success
                returnData.RequestType = "GIS_WMIS_GetMUCataLog";

                for (int i = 0; i < parameters.ReportList.Count; i++)
                {
                    ReportList rl = parameters.ReportList[i];
                    Reports report = new Reports();
                    report.Status = "SUCCESS";
                    report.ReportType = rl.Name;
                    report.ReportLink = @"\\Server\Share\WorkInstructions.pdf";
                    returnData.Reports.Add(report);
                }
                
            }

            return returnData;
        }

        [Route("GIS_WMIS_UpdateJobStatus")]
        public WMISReturn GIS_WMIS_UpdateJobStatus([FromBody]Input parameters)
        {
            WMISReturn returnData = new WMISReturn();

            if (parameters.RequestType == "")
            {
                //Fail
                returnData.RequestType = "GIS_WMIS_UpdateJobStatus";
                returnData.Status = "FAILURE";
                returnData.WR_Number = parameters.RequestData.WR_Number;
                returnData.Error = "Error message returned from WMIS";
            }
            else
            {
                //Success
                returnData.RequestType = "GIS_WMIS_UpdateJobStatus";
                returnData.Status = "SUCCESS";
            }

            return returnData;
        }
    
        [Route("GIS_WMIS_UpdateWR")]
        public WMISReturn GIS_WMIS_UpdateWR([FromBody]Input parameters)
        {
            WMISReturn returnData = new WMISReturn();

            if (parameters.RequestType == "")
            {
                //Fail
                returnData.RequestType = "GIS_WMIS_UpdateWR";
                returnData.Status = "FAILURE";
                returnData.WR_Number = parameters.RequestData.WR_Number;
                returnData.Error = "Error message passed back from WMIS";
            }
            else
            {
                //Success
                returnData.RequestType = "GIS_WMIS_UpdateWR";
                returnData.Status = "SUCCESS";
            }

            return returnData;

        }

        [Route("GIS_WMIS_WriteBack")]
        public WMISReturn GIS_WMIS_WriteBack([FromBody]Input parameters)
        {
            WMISReturn returnData = new WMISReturn();

            if (parameters.RequestType == "")
            {
                //Fail
                returnData.RequestType = "GIS_WMIS_WriteBack";
                returnData.Status = "FAILURE";
                returnData.WR_Number = parameters.WR_Number;
                returnData.Error = "Error message passed back from WMIS";
            }
            else
            {
                //Success
                returnData.RequestType = "GIS_WMIS_WriteBack";
                returnData.Status = "SUCCESS";
            }

            return returnData;
        }
    
    }
}
