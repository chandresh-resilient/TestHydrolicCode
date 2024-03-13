using System;

using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using HydraulicEngine;
using HydraulicCalAPI.Service;
//using static System.Net.Mime.MediaTypeNames;
using System.IO;
//using System.Reflection.Metadata;
using HydraulicCalAPI.ViewModel;
using iText.IO.Image;
using iText.Kernel.Colors;
using iText.Kernel.Pdf;
using iText.Kernel.Geom;
using iText.Kernel.Pdf.Canvas.Draw;
using iText.Layout;
using iText.Layout.Element;
using iText.Kernel.Events;
using iText.Layout.Properties;
using iText.Layout.Borders;
using SkiaSharp;

namespace HydraulicCalAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HydraulicCalculationsController : ControllerBase
    {
        Color accuColor;
        PdfDocument pdf;
        Document document;
        Dictionary<string, string> pdfAuthor;
        List<PdfReportService> pdfFooter;
        List<PdfReportService> pdfCasingData;
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
        public void getHydraulicReportGenerator([FromBody] HydraulicCalAPI.Service.PdfReportService objRptGeneratorService)
        {
            ChartAndGraphService someData = executeHydraulicCalulations(objRptGeneratorService.HydraulicCalculationService);
           
            new PDFReportGen().generatePDF(objRptGeneratorService);
        }

    }
}
    
