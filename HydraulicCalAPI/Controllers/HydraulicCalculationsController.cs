using System;

using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using HydraulicEngine;
using HydraulicCalAPI.Service;
//using static System.Net.Mime.MediaTypeNames;
using System.IO;
//using System.Reflection.Metadata;
using HydraulicCalAPI.ViewModel;
using SkiaSharp;

namespace HydraulicCalAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HydraulicCalculationsController : ControllerBase
    {
       
        [HttpPost("getHydraulicCalculations")]
        public Dictionary<String, Object> getHydraulicCalculations([FromBody] HydraulicCalAPI.Service.HydraulicCalculationService objHcs)
        {
            
            return executeHydraulicCalulations(objHcs).ChartNGraphDataPoints;
        }

        private static ChartAndGraphService executeHydraulicCalulations(HydraulicCalculationService objHcs)
        {
            SurfaceEquipment equipment = new SurfaceEquipment(objHcs.surfaceEquipmentInput.CaseType);
            List<BHATool> bhatools = HydraulicCalculationsControllerHelpers.getBHATools(objHcs);

            if ((double.IsNaN(objHcs.toolDepthInFeet) || objHcs.toolDepthInFeet == 0))
            {
                for (int i = 0; i < objHcs.annulusInput.Count; i++)
                {
                    objHcs.toolDepthInFeet += objHcs.annulusInput[i].AnnulusBottomInFeet;
                }
            }

            ChartAndGraphService objChartnGraph = new ChartAndGraphService();
            objChartnGraph.GetDataPoints(objHcs.fluidInput,
                                                objHcs.flowRateInGPMInput,
                                                objHcs.cuttingsInput,
                                                bhatools,
                                                objHcs.annulusInput,
                                                equipment,
                                                objHcs.maxflowrate,
                                                objHcs.maxflowpressure, objHcs.toolDepthInFeet);
            return objChartnGraph;
        }

        [HttpPost("getHydraulicReportGenerator")]
        public FileContentResult getHydraulicReportGenerator([FromBody] HydraulicCalAPI.Service.PdfReportService objRptGeneratorService)
        {    
            ChartAndGraphService someData = executeHydraulicCalulations(objRptGeneratorService.HydraCalcService);
            byte[] memoryPdf = new PDFReportGen().generatePDF(objRptGeneratorService, someData, objRptGeneratorService.HydraCalcService);
            string fileDownloadName = objRptGeneratorService.JobID + "-HYD report.pdf";

            Response.Headers.Add("Content-Disposition", $"attachment; filename={fileDownloadName}");
            return base.File(memoryPdf, "application/pdf", fileDownloadName);
        }

    }
}
    
