using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;

using SkiaSharp;
using iText.Layout;
using iText.IO.Image;
using iText.Kernel.Pdf;
using iText.Kernel.Geom;
using iText.Kernel.Colors;
using iText.Kernel.Events;
using iText.Layout.Borders;
using iText.Layout.Element;
using iText.Layout.Properties;
using iText.Kernel.Pdf.Canvas;
using iText.Kernel.Pdf.Canvas.Draw;

using HydraulicCalAPI.Controllers;
using HydraulicCalAPI.Service;
using Microsoft.AspNetCore.Mvc;


namespace HydraulicCalAPI.Service
{
    public class DataPoints
    {
        public float X { get; set; }
        public float Y { get; set; }
        public string LineClr { get; set; }
    }
    public class PieData
    {
        public string Label { get; set; }
        public float Value { get; set; }
        public string Color { get; set; }
    }
    public class HydraulicBHAToolOutPutData
    {
        public int PositionNum { get; set; }
        public string WorkString { get; set; }
        public double Length { get; set; }
        public double InputFlowRate { get; set; }
        public double AverageVelocity { get; set; }
        public string AvgVelocityColor { get; set; }
        public double CriticalVelocity { get; set; }
        public string FlowType { get; set; }
        public double PressureDrop { get; set; }
        public double HydraulicOD { get; set; }
        public double HydraulicID { get; set; }
    }
    public class HydraulicAnalysisAnnulusOutputData
    {
        public string Annulus { get; set; }
        public string WorkString { get; set; }
        public double From { get; set; }
        public double To { get; set; }
        public double AverageVelocity { get; set; }
        public double CriticalVelocity { get; set; }
        public string Flow { get; set; }
        public double ChipRate { get; set; }
        public double PressureDrop { get; set; }
        public string AvgVelocityColor { get; set; }
        public string AnnulusColor { get; set; }
        public string ChipRateColor { get; set; }
    }
    class PageEventHandler : IEventHandler
    {
        public void HandleEvent(Event @event)
        {
            PdfDocumentEvent docEvent = (PdfDocumentEvent)@event;
            PdfDocument pdfDoc = docEvent.GetDocument();
            PdfPage page = docEvent.GetPage();

            int pageNumber = pdfDoc.GetPageNumber(page);
            int numofpages = pdfDoc.GetNumberOfPages();
            
            PdfCanvas canvas = new PdfCanvas(page.NewContentStreamBefore(),page.GetResources(),pdfDoc);
            Rectangle pageSize = page.GetPageSize();

            float x = pageSize.GetWidth() / 2 +140;
            float yStart = 166;
            float yEnd = yStart - 20;
            canvas.SaveState()
                .SetLineWidth(1)
                .MoveTo(x, yStart)
                .LineTo(x, yEnd)
                .Stroke()
                .RestoreState();

            canvas.BeginText()
                .SetFontAndSize(iText.Kernel.Font.PdfFontFactory.CreateFont(iText.IO.Font.Constants.StandardFonts.HELVETICA), 12)
                .MoveText(pageSize.GetWidth() / 2 + 150, 150)
                .ShowText(pageNumber.ToString() + " out of " + numofpages)
                .EndText();
            canvas.Release();
        }
    }

    public class PDFReportGen
    {
        Color accuColor;

        Dictionary<string, string> pdfAuthor;
        Dictionary<string, string> pdfLstItemData;

        Dictionary<string, string> pdfPieChart = new Dictionary<string, string>();

        HydraulicCalculationService objHydCalSrvs = new HydraulicCalculationService();
        string strCasingLinerTubing = "New Run";
          

        public byte[] generatePDF(HydraulicCalAPI.Service.PdfReportService objInputData, ChartAndGraphService objChartService, HydraulicCalculationService inputHydra)
        {
            objHydCalSrvs = inputHydra;
            string _tabheader = string.Empty;
            accuColor = new DeviceRgb(165, 42, 42);
            string baseDir = AppDomain.CurrentDomain.BaseDirectory;

            // Construct the path to the image relative to the base directory
            string imagePath = System.IO.Path.Combine(baseDir, "Images", "wft.jpg");
            // Add image
            Image img = new Image(ImageDataFactory
               .Create(imagePath))
               .SetTextAlignment(TextAlignment.LEFT).SetWidth(80).SetHeight(30).SetMarginBottom(3);

            // New line
            Paragraph newline = new Paragraph(new Text("\n"));
            Paragraph legend = new Paragraph();
            // Line separator
            SolidLine line = new SolidLine(3f);
            line.SetColor(accuColor);
            LineSeparator ls = new LineSeparator(line);

            #region First Page
            Paragraph header = getHeader(objInputData.ReportHeader.ToString());

            Table _pdfHead = new Table(2, false);
            Cell _pdfHeadcell = new Cell();
            _pdfHeadcell.Add(img);
            _pdfHead.AddCell(_pdfHeadcell)
                .SetMarginBottom(2);

            pdfAuthor = new Dictionary<string, string>();
            pdfAuthor.Add("Customer : ", objInputData.Customer != null ? objInputData.Customer.ToString() : "");
            pdfAuthor.Add("Job Number : ", objInputData.JobNumber != null ? objInputData.JobNumber.ToString() : "");
            pdfAuthor.Add("Well Name and Number : ", objInputData.WellNameNumber != null ? objInputData.WellNameNumber.ToString() : "");
            pdfAuthor.Add("Prepared By : ", objInputData.PreparedBy != null ? objInputData.JobNumber.ToString() : "");
            pdfAuthor.Add("Prepared On : ", objInputData.PreparedOn != null ? objInputData.PreparedOn.ToString() : "");
            Table tableAuthor = getTableContent(pdfAuthor);

            Paragraph comment = new Paragraph("Comment:").SetTextAlignment(TextAlignment.LEFT).SetFontSize(10);
            #endregion
            
            #region HeaderInformation
            Dictionary<string, string> pdfHederInfoData = new Dictionary<string, string>();
            Paragraph _headerinfo = new Paragraph("Header Information")
             .SetTextAlignment(TextAlignment.CENTER)
             .SetFontSize(20).SetBold();

            _tabheader = "Segment and Product / Service";
            pdfHederInfoData.Add("Segment", objInputData.ProductLine != null ? objInputData.ProductLine.ToString() : "");
            pdfHederInfoData.Add("Product / Service", objInputData.SubProductLine != null ? objInputData.SubProductLine.ToString() : "");
            Table tblSegment = getHeaderInfoTable(pdfHederInfoData, _tabheader);

            pdfHederInfoData.Clear();

            _tabheader = "Job Information";
            pdfHederInfoData.Add("Job Start Date", objInputData.JobStartDate != null ? objInputData.JobStartDate.ToString() : "");
            pdfHederInfoData.Add("Well Location", objInputData.WellLocation != null ? objInputData.WellLocation.ToString() : "");
            pdfHederInfoData.Add("Customer", objInputData.Customer != null ? objInputData.Customer.ToString() : "");
            pdfHederInfoData.Add("Well Depth (MD)", objInputData.WellDepth > 0.00 ? (objInputData.WellDepth.ToString() + " | ft") : "0.00");
            pdfHederInfoData.Add("Job End Date", objInputData.JobEndDate != null ? objInputData.JobEndDate.ToString() : "");
            pdfHederInfoData.Add("JDE Delivery Ticket No", objInputData.JDEDeliveryTicketNo != null ? objInputData.JDEDeliveryTicketNo.ToString() : "");
            Table tblJobInformation = getHeaderInfoTable(pdfHederInfoData, _tabheader);

            pdfHederInfoData.Clear();

            _tabheader = "Well Information";
            pdfHederInfoData.Add("Well Name and Number", objInputData.WellNameNumber != null ? objInputData.WellNameNumber.ToString() : "");
            pdfHederInfoData.Add("Well Location", objInputData.WellLocation != null ? objInputData.WellLocation.ToString() : "");
            pdfHederInfoData.Add("Field", objInputData.Field != null ? objInputData.Field.ToString() : "");
            pdfHederInfoData.Add("Lease", objInputData.Lease != null ? objInputData.Lease.ToString() : "");
            pdfHederInfoData.Add("Rig", objInputData.Rig != null ? objInputData.Rig.ToString() : "");
            pdfHederInfoData.Add("legal.API/OCS-G", objInputData.legalAPIOCSG != null ? objInputData.legalAPIOCSG.ToString() : "");
            pdfHederInfoData.Add("Latitude", objInputData.Latitude != null ? objInputData.Latitude.ToString() : "");
            pdfHederInfoData.Add("Longitude", objInputData.Longitude != null ? objInputData.Longitude.ToString() : "");
            pdfHederInfoData.Add("Well Country", objInputData.WellCountry != null ? objInputData.WellCountry.ToString() : "");
            pdfHederInfoData.Add("County/Parish", objInputData.CountyParish != null ? objInputData.CountyParish.ToString() : "");
            Table tblWellInformation = getHeaderInfoTable(pdfHederInfoData, _tabheader);

            pdfHederInfoData.Clear();

            _tabheader = "Originator / Servicing Location Organization Data";
            pdfHederInfoData.Add("WFRD Location", objInputData.WFRDLocation != null ? objInputData.WFRDLocation.ToString() : "");
            pdfHederInfoData.Add("Hemisphere", objInputData.Hemisphere != null ? objInputData.Hemisphere.ToString() : "");
            pdfHederInfoData.Add("Geozone", objInputData.Geozone != null ? objInputData.Geozone.ToString() : "");
            pdfHederInfoData.Add("Region", objInputData.Region != null ? objInputData.Region.ToString() : "");
            pdfHederInfoData.Add("SubRegion", objInputData.SubRegion != null ? objInputData.SubRegion.ToString() : "");
            Table tblOriginator = getHeaderInfoTable(pdfHederInfoData, _tabheader);

            pdfHederInfoData.Clear();

            _tabheader = "Customer Contacts";
            pdfHederInfoData.Add("Customer Contact (Office)", objInputData.CustomerContactOffice != null ? objInputData.CustomerContactOffice.ToString() : "");
            pdfHederInfoData.Add("Customer Phone No. (Office)", objInputData.CustomerPhoneNoOffice != null ? objInputData.CustomerPhoneNoOffice.ToString() : "");
            pdfHederInfoData.Add("Customer Contact (Field)", objInputData.CustomerContactField != null ? objInputData.CustomerContactField.ToString() : "");
            pdfHederInfoData.Add("Customer Phone No. (Field)", objInputData.CustomerPhoneNoField != null ? objInputData.CustomerPhoneNoField.ToString() : "");
            pdfHederInfoData.Add("Drilling Engineer", objInputData.DrillingEngineer != null ? objInputData.DrillingEngineer.ToString() : "");
            pdfHederInfoData.Add("Drilling Contractor", objInputData.DrillingContractor != null ? objInputData.DrillingContractor.ToString() : "");
            Table tblCustomerContacts = getHeaderInfoTable(pdfHederInfoData, _tabheader);

            pdfHederInfoData.Clear();

            _tabheader = "Weatherford Contacts";
            pdfHederInfoData.Add("WFRD Salesman", objInputData.WFRDSalesman != null ? objInputData.WFRDSalesman.ToString() : "");
            pdfHederInfoData.Add("WFRD Field Engineer", objInputData.WFRDFieldEngineer != null ? objInputData.WFRDFieldEngineer.ToString() : "");
            Table tblWeatherfordContacts = getHeaderInfoTable(pdfHederInfoData, _tabheader);

            pdfHederInfoData.Clear();

            Table tblGenInfo;
            getGeneralInfo(objInputData, out _tabheader, pdfHederInfoData, out tblGenInfo);

            pdfHederInfoData.Clear();

            _tabheader = "Approval and General Input Data";
            pdfHederInfoData.Add("Status", objInputData.Status != null ? objInputData.Status.ToString() : "");
            pdfHederInfoData.Add("Input By", objInputData.InputBy != null ? objInputData.InputBy.ToString() : "");
            pdfHederInfoData.Add("Prepared By", objInputData.StatusPreparedBy != null ? objInputData.StatusPreparedBy.ToString() : "");
            pdfHederInfoData.Add("Accuview Input Date", objInputData.WellDepth > 0.00 ? (objInputData.WellDepth.ToString() + " | ft") : "0.00");
            pdfHederInfoData.Add("Submitted Date", objInputData.JobEndDate != null ? objInputData.JobEndDate.ToString() : "");
            pdfHederInfoData.Add("Approved By", objInputData.JDEDeliveryTicketNo != null ? objInputData.JDEDeliveryTicketNo.ToString() : "");
            pdfHederInfoData.Add("Approved Date", objInputData.JDEDeliveryTicketNo != null ? objInputData.JDEDeliveryTicketNo.ToString() : "");
            Table tblApproval = getHeaderInfoTable(pdfHederInfoData, _tabheader);

            pdfHederInfoData.Clear();

            #endregion

            #region Casing Liner Tubing
            
            List<string> pdfCasingData = new List<string>();
            if(objInputData.SubProductLine.ToString().ToUpper() == "FISHING")
            {
                strCasingLinerTubing = "CH Fishing Run";
            }
            else if(objInputData.SubProductLine.ToString().ToUpper() == "CASING")
            {
                strCasingLinerTubing = "FRE Casing Run";
            }
            else
            {
                strCasingLinerTubing = "New Run";
            }
            Paragraph _casingLinerTubingInfo = new Paragraph(strCasingLinerTubing)
            .SetTextAlignment(TextAlignment.CENTER)
            .SetFontSize(20).SetBold();
            //Code to get Annulus Length and BhaTool Length
            double annulusLength = 0.00;
            double bhatoolLength = 0.00;
            foreach (var anulsitem in objChartService.HydraulicOutputAnnulusList)
            {
                annulusLength += anulsitem.Length;
            }
            foreach (var bhaTitem in objChartService.HydraulicOutputBHAList)
            {
                bhatoolLength += bhaTitem.LengthBHA;
            }

            pdfCasingData.Add(annulusLength > 0 ? (annulusLength.ToString() + " | ft") : "0.00");
            pdfCasingData.Add(bhatoolLength > 0 ? (bhatoolLength.ToString() + " | ft") : "0.00");
            pdfCasingData.Add(objChartService.ToolDepth > 0 ? (objChartService.ToolDepth.ToString() + " | ft") : "0.00");

            _tabheader = "Depth Analysis";
            Table tblDepthAnalysis = getDepthAnalysis(pdfCasingData, _tabheader);

            pdfCasingData.Clear();

            _tabheader = "Casing/ Liner/ Tubing Data";
            // preparing Casing, Liner and Tubing Object List
            Dictionary<string, string> dicLstCltData = new Dictionary<string, string>(); ;
            int increment = 0;
            foreach (var cltItem in objHydCalSrvs.annulusInput)
            {

                //Code to get WellBore weight and WellBore Grade form PdfReportService
                string cltWeight = objInputData.CasingLinerTubeData[increment].WellBoreWeight;

                string cltGrade = objInputData.CasingLinerTubeData[increment].Grade;
                increment++;
                dicLstCltData.Add("CLTID" + increment, increment.ToString());
                dicLstCltData.Add("WellBoreSection" + increment, (cltItem.WellboreSectionName != null ? cltItem.WellboreSectionName.ToString() : ""));
                dicLstCltData.Add("OutDiameter" + increment, (cltItem.AnnulusODInInch > 0 ? cltItem.AnnulusODInInch.ToString() : "0"));
                dicLstCltData.Add("InnDiameter" + increment, (cltItem.AnnulusIDInInch > 0 ? cltItem.AnnulusIDInInch.ToString() : "0"));
                dicLstCltData.Add("WellBoreWeight" + increment, (string.IsNullOrEmpty(cltWeight) ? "0" : cltWeight));
                dicLstCltData.Add("Grade" + increment, (string.IsNullOrEmpty(cltGrade.ToString()) ? "" : cltGrade));
                dicLstCltData.Add("WellTop" + increment, (cltItem.AnnulusTopInFeet >= 0 ? cltItem.AnnulusTopInFeet.ToString() : "0"));
                dicLstCltData.Add("WellBottom" + increment, (cltItem.AnnulusBottomInFeet > 0 ? cltItem.AnnulusBottomInFeet.ToString() : ""));
            }
            Table tblCasingLinerTube = getCasingLinerTubing(dicLstCltData, _tabheader); ;
            increment = 0;
            #endregion

            #region Table Of Content
            Table tblToc = new Table(2, false).SetBorder(Border.NO_BORDER);
            tblToc = getTableOfcontent(strCasingLinerTubing);
            #endregion

            #region BHA Input Data
            _tabheader = "Bottom Hole Assembly (Top Down)";
            Dictionary<string, string> dicLstBhaData = new Dictionary<string, string>();

            foreach (var bhawrkStrlstitem in objHydCalSrvs.bhaInput)
            {
                string toolType = bhawrkStrlstitem.bhatooltype.ToString().ToUpper();
                increment++;
                switch (toolType)
                {
                    case "WRKSTR":
                        {
                            dicLstBhaData.Add("BhaLstID" + increment, increment.ToString());
                            dicLstBhaData.Add("ToolDescription" + increment, bhawrkStrlstitem.SectionName != null ? bhawrkStrlstitem.SectionName.ToString() : "");
                            dicLstBhaData.Add("SerialNumber" + increment, "");
                            dicLstBhaData.Add("MeasuredOD" + increment, bhawrkStrlstitem.OutsideDiameterInInch > 0 ? bhawrkStrlstitem.OutsideDiameterInInch.ToString() : "0");
                            dicLstBhaData.Add("InnerDiameter" + increment, bhawrkStrlstitem.InsideDiameterInInch > 0 ? bhawrkStrlstitem.InsideDiameterInInch.ToString() : "0");
                            // code to get Workstring Weight and Upper Connection type
                            var wrkStrWeight = objInputData.WorkStringItems.Where(wks => increment.Equals(bhawrkStrlstitem.PositionNumber))
                                                  .Select(wks => wks.wrkWeight);
                            var wrkStrUpConnTyp = objInputData.WorkStringItems.Where(wks => increment.Equals(bhawrkStrlstitem.PositionNumber))
                                                    .Select(wks => wks.wrkUpperConnType);
                            dicLstBhaData.Add("Weight" + increment, wrkStrWeight != null ? wrkStrWeight.FirstOrDefault() : "0");
                            dicLstBhaData.Add("Length" + increment, bhawrkStrlstitem.LengthInFeet > 0 ? bhawrkStrlstitem.LengthInFeet.ToString() : "0");
                            dicLstBhaData.Add("UpperConnType" + increment, wrkStrUpConnTyp != null ? wrkStrUpConnTyp.FirstOrDefault() : "N/A");
                            dicLstBhaData.Add("LowerConnType" + increment, "N/A");
                            dicLstBhaData.Add("FishNeckOD" + increment, "0.00");
                            dicLstBhaData.Add("FishNeckLength" + increment, "0");
                            dicLstBhaData.Add("HydraulicOD" + increment, "0");
                            dicLstBhaData.Add("HydraulicID" + increment, "0");
                            break;
                        }
                    default:
                        {
                            dicLstBhaData.Add("BhaLstID" + increment, increment.ToString());
                            dicLstBhaData.Add("ToolDescription" + increment, bhawrkStrlstitem.toolDescription != null ? bhawrkStrlstitem.toolDescription.ToString() : "");
                            dicLstBhaData.Add("SerialNumber" + increment, bhawrkStrlstitem.PositionNumber > 0 ? bhawrkStrlstitem.PositionNumber.ToString() : "");
                            dicLstBhaData.Add("MeasuredOD" + increment, bhawrkStrlstitem.OutsideDiameterInInch > 0 ? bhawrkStrlstitem.OutsideDiameterInInch.ToString() : "0");
                            dicLstBhaData.Add("InnerDiameter" + increment, bhawrkStrlstitem.InsideDiameterInInch > 0 ? bhawrkStrlstitem.InsideDiameterInInch.ToString() : "0");
                            // code to get BHA Weight and Upper Connection type
                            var bhatoolWeight = objInputData.BHAToolItemData.Where(bt => bt.SerialNumber == bhawrkStrlstitem.PositionNumber)
                                                .Select(bt => bt.Weight);
                            var bhatoolUpConnTyp = objInputData.BHAToolItemData.Where(bt => bt.SerialNumber == bhawrkStrlstitem.PositionNumber)
                                                .Select(bt => bt.UpperConnType);
                            var bhatoolLowConntyp = objInputData.BHAToolItemData.Where(bt => bt.SerialNumber == bhawrkStrlstitem.PositionNumber)
                                                                    .Select(bt => bt.LowerConnType);
                            var bhatoolFishNeckOD = objInputData.BHAToolItemData.Where(bt => bt.SerialNumber == bhawrkStrlstitem.PositionNumber)
                                                                    .Select(bt => bt.FishNeckOD);
                            var bhatoolFishNeckLen = objInputData.BHAToolItemData.Where(bt => bt.SerialNumber == bhawrkStrlstitem.PositionNumber)
                                                                     .Select(bt => bt.FishNeckLength);
                            dicLstBhaData.Add("Weight" + increment, bhatoolWeight != null ? bhatoolWeight.FirstOrDefault() : "0");
                            dicLstBhaData.Add("Length" + increment, bhawrkStrlstitem.LengthInFeet > 0 ? bhawrkStrlstitem.LengthInFeet.ToString() : "0");
                            dicLstBhaData.Add("UpperConnType" + increment, bhatoolUpConnTyp != null ? bhatoolUpConnTyp.FirstOrDefault() : "N/A");
                            dicLstBhaData.Add("LowerConnType" + increment, bhatoolLowConntyp != null ? bhatoolLowConntyp.FirstOrDefault() : "N/A");
                            dicLstBhaData.Add("FishNeckOD" + increment, bhatoolFishNeckOD != null ? bhatoolFishNeckOD.FirstOrDefault() : "");
                            dicLstBhaData.Add("FishNeckLength" + increment, bhatoolFishNeckLen != null ? bhatoolFishNeckLen.FirstOrDefault() : "0");
                            dicLstBhaData.Add("HydraulicOD" + increment, bhawrkStrlstitem.OutsideDiameterInInch > 0 ? bhawrkStrlstitem.OutsideDiameterInInch.ToString() : "0");
                            dicLstBhaData.Add("HydraulicID" + increment, bhawrkStrlstitem.InsideDiameterInInch > 0 ? bhawrkStrlstitem.InsideDiameterInInch.ToString() : "0");
                            break;
                        }
                }
            }
            Table tblBhaData = getBha(dicLstBhaData, _tabheader);
            increment = 0;

            #endregion

            #region Surface Equipment / Fluid Envelop / Fluid

            _tabheader = "Surface Equipment";
            string _surfaceEquipmentData = objHydCalSrvs.surfaceEquipmentInput.CaseType.ToString();
            double _totLength = getSurfaceEquipmentTotalLength(_surfaceEquipmentData.ToString());
            pdfHederInfoData.Add("Surface Equipment", _surfaceEquipmentData != null ? _surfaceEquipmentData.ToString() : "");
            pdfHederInfoData.Add("Total Length", _totLength > 0.00 ? (_totLength.ToString() + " | ft") : "0.00");
            Table tblSurfaceEquipment = getHeaderInfoTable(pdfHederInfoData, _tabheader);
            pdfHederInfoData.Clear();

            _tabheader = "Fluid Envelope";
            pdfLstItemData = new Dictionary<string, string>();
            pdfLstItemData.Add("MaximumAllowablePressure", objHydCalSrvs.maxflowpressure > 0 ? objHydCalSrvs.maxflowpressure.ToString() : "0.00");
            pdfLstItemData.Add("MaximumAllowableFlowrate", objHydCalSrvs.maxflowrate > 0 ? objHydCalSrvs.maxflowrate.ToString() : "0.00");
            pdfLstItemData.Add("Comments", objInputData.Comments != null ? objInputData.Comments.ToString() : "");
            Table tblFluidEnvelope = getFluidEnvelopeInfo(pdfLstItemData, _tabheader);

            pdfLstItemData.Clear();

            _tabheader = "Fluid";
            foreach (var fluidlstitem in objInputData.FluidItemData)
            {
                pdfHederInfoData.Add("Solids", fluidlstitem.Solids > 0.00 ? (fluidlstitem.Solids.ToString() + " | % ") : "0.00");
                pdfHederInfoData.Add("Drilling Fluid Type", fluidlstitem.DrillingFluidType != null ? fluidlstitem.DrillingFluidType.ToString() : "");
                pdfHederInfoData.Add("Drilling Fluid Weight", objHydCalSrvs.fluidInput.DensityInPoundPerGallon > 0.00 ? (objHydCalSrvs.fluidInput.DensityInPoundPerGallon.ToString() + " | lb/gal") : "0.00");
                pdfHederInfoData.Add("Buoyancy Factor", fluidlstitem.BuoyancyFactor > 0.00 ? (fluidlstitem.BuoyancyFactor.ToString() + " | lb/gal") : "0.00");
                pdfHederInfoData.Add("Plastic Viscosity", objHydCalSrvs.fluidInput.PlasticViscosityInCentiPoise > 0.00 ? (objHydCalSrvs.fluidInput.PlasticViscosityInCentiPoise.ToString() + " | centipoise") : "0.00");
                pdfHederInfoData.Add("Yield Point", objHydCalSrvs.fluidInput.YieldPointInPoundPerFeetSquare > 0.00 ? (objHydCalSrvs.fluidInput.YieldPointInPoundPerFeetSquare.ToString() + " | lbf/100ft²") : "0.00");
                pdfHederInfoData.Add("Cutting Average Size", objHydCalSrvs.cuttingsInput.AverageCuttingSizeInInch > 0 ? (objHydCalSrvs.cuttingsInput.AverageCuttingSizeInInch.ToString() + " | in") : "0.00");
                pdfHederInfoData.Add("Cutting Type", fluidlstitem.CuttingType != null ? fluidlstitem.CuttingType.ToString() : "");
            }
            Table tblFluid = getSurfacePageInfo(pdfHederInfoData, _tabheader);

            pdfHederInfoData.Clear();

            #endregion

            #region Chart and Graph Section
            #region Pressure Distribution Chart

            Table PieChartTable = getPieHeaderTable(objChartService);

            Paragraph _chartheader = new Paragraph("Pressure Distribution Chart")
            .SetTextAlignment(TextAlignment.CENTER)
            .SetFontSize(12).SetBold();
            List<PieData> pieDataPoints = new List<PieData>();

            foreach (var pieitem in objChartService.PressureDistributionChartCollection)
            {
                pieDataPoints.Add(new PieData
                {
                    Label = pieitem.Name,
                    Value = (float)pieitem.Value,
                    Color = pieitem.Color
                });
            }

            foreach (var itempiedata in objChartService.PressureDistributionChartCollection)
            {
                increment++;
                pdfPieChart.Add("Name" + increment, itempiedata.Name != null ? itempiedata.Name.ToString() : "");
                pdfPieChart.Add("Value" + increment, itempiedata.Value > 0 ? itempiedata.Value.ToString() : "");
                pdfPieChart.Add("Color" + increment, itempiedata.Color != null ? itempiedata.Color.ToString() : "");
            }
            increment = 0;
            byte[] chartBytes = GeneratePieChart(pdfPieChart, pieDataPoints);
            Image imgPie = new Image(ImageDataFactory.Create(chartBytes));
            #endregion

            // Code for Drwaing Line Graph
            #region Standpipe Vs Flowrate Graph
            Paragraph _stdpipeHeader = new Paragraph("StandPipe Vs Flow Rate")
                .SetTextAlignment(TextAlignment.CENTER)
                .SetFontSize(12).SetBold();

            List<DataPoints> dataPoints = new List<DataPoints>();
            if (objChartService.standpipePressureListRL.Count > 0)
            {
                foreach (var item in objChartService.standpipePressureListRL)
                {
                    dataPoints.Add(new DataPoints
                    {
                        X = (float)item.PrimaryAxisValue,
                        Y = (float)item.SecondaryAxisValue,
                        LineClr = "Red"
                    });
                }
            }
            if (objChartService.standpipePressureListYL.Count > 0)
            {
                foreach (var item in objChartService.standpipePressureListYL)
                {
                    dataPoints.Add(new DataPoints
                    {
                        X = (float)item.PrimaryAxisValue,
                        Y = (float)item.SecondaryAxisValue,
                        LineClr = "Yellow"
                    });
                }
            }
            if (objChartService.standpipePressureListG.Count > 0)
            {
                foreach (var item in objChartService.standpipePressureListG)
                {
                    dataPoints.Add(new DataPoints
                    {
                        X = (float)item.PrimaryAxisValue,
                        Y = (float)item.SecondaryAxisValue,
                        LineClr = "Green"
                    });
                }
            }
            if (objChartService.standpipePressureListYH.Count > 0)
            {
                foreach (var item in objChartService.standpipePressureListYH)
                {
                    dataPoints.Add(new DataPoints
                    {
                        X = (float)item.PrimaryAxisValue,
                        Y = (float)item.SecondaryAxisValue,
                        LineClr = "Yellow"
                    });
                }
            }
            if (objChartService.standpipePressureListRH.Count > 0)
            {
                foreach (var item in objChartService.standpipePressureListRH)
                {
                    dataPoints.Add(new DataPoints
                    {
                        X = (float)item.PrimaryAxisValue,
                        Y = (float)item.SecondaryAxisValue,
                        LineClr = "Red"
                    });
                }
            }

            // Generate line graph image
            byte[] graphBytes = DrawLineGraph(objChartService, dataPoints);
            Image imgStdPvsFlwRate = new Image(ImageDataFactory.Create(graphBytes));

            // Add labels and values to PDF document
            Paragraph xscale = new Paragraph($"X-axis: Flow Rate (gal/min)");
            Paragraph yscale = new Paragraph($"Y-axis: Standpipe Pressure (psi)");
            #endregion

            #region Hydraulic Annulus Output
            List<HydraulicAnalysisAnnulusOutputData> objHyAnlyAnnuOutputData = new List<HydraulicAnalysisAnnulusOutputData>();
            List<Table> dicLstAnnulusOutputData = new List<Table>();
            foreach (var itemannulsoutputlst in objChartService.HydraulicOutputAnnulusList)
            {
                objHyAnlyAnnuOutputData.Add(new HydraulicAnalysisAnnulusOutputData
                {
                    AnnulusColor = itemannulsoutputlst.AnnulusColor.ToString(),
                    Annulus = itemannulsoutputlst.Annulus != null ? itemannulsoutputlst.Annulus.ToString() : "",
                    WorkString = itemannulsoutputlst.Workstring != null ? itemannulsoutputlst.Workstring.ToString() : "",
                    From = itemannulsoutputlst.FromAnnulus > 0 ? Math.Round(itemannulsoutputlst.FromAnnulus, 3) : 0,
                    To = itemannulsoutputlst.ToAnnulus > 0 ? Math.Round(itemannulsoutputlst.ToAnnulus, 3) : 0,
                    AverageVelocity = itemannulsoutputlst.AverageVelocity > 0 ? Math.Round(itemannulsoutputlst.AverageVelocity, 3) : 0,
                    AvgVelocityColor = itemannulsoutputlst.AverageVelocityColor.ToString(),
                    CriticalVelocity = itemannulsoutputlst.CriticalVelocity > 0.00 ? Math.Round(itemannulsoutputlst.CriticalVelocity, 3) : 0,
                    Flow = itemannulsoutputlst.FlowType != null ? itemannulsoutputlst.FlowType : "",
                    ChipRate = itemannulsoutputlst.ChipRate > 0.00 ? Math.Round(itemannulsoutputlst.ChipRate, 3) : 0,
                    ChipRateColor = itemannulsoutputlst.ChipRateColor.ToString(),
                    PressureDrop = itemannulsoutputlst.AnnulusPressureDrop > 0.00 ? Math.Round(itemannulsoutputlst.AnnulusPressureDrop, 3) : 0
                });
            }

            Table tblHeaderAnnulusOutput = getAnnulusOutputTblHead();
            for (int i = 0; i < objHyAnlyAnnuOutputData.Count; i++)
            {
                dicLstAnnulusOutputData.Add(getAnnulusTableData(objHyAnlyAnnuOutputData[i]));
            }

            #endregion

            #region Hydraulic BHA Tool OutPut Tables
            List<Table> lstTblBHAheader = new List<Table>();
            List<Table> lstTblBhaSide = new List<Table>();
            List<HydraulicBHAToolOutPutData> objBhaToolOutput = new List<HydraulicBHAToolOutPutData>();
            List<HydraulicBHAToolOutPutData> objBha1 = new List<HydraulicBHAToolOutPutData>();
            foreach (var item in objChartService.HydraulicOutputBHAList)
            {
                objBhaToolOutput.Add(new HydraulicBHAToolOutPutData
                {
                    PositionNum = item.PositionNo > 0 ? item.PositionNo : 0,
                    WorkString = string.IsNullOrEmpty(item.Workstring) ? "" : item.Workstring.ToString(),
                    Length = item.LengthBHA > 0 ? Math.Round(item.LengthBHA, 2) : 0,
                    InputFlowRate = item.InputFlowRate > 0 ? Math.Round(item.InputFlowRate, 3) : 0,
                    AverageVelocity = item.AverageVelocity > 0 ? Math.Round(item.AverageVelocity, 3) : 0,
                    AvgVelocityColor = item.AverageVelocityColor.ToString() != null ? item.AverageVelocityColor.ToString() : "",
                    CriticalVelocity = item.CriticalVelocity > 0 ? Math.Round(item.CriticalVelocity, 3) : 0,
                    FlowType = string.IsNullOrEmpty(item.FlowType) ? "" : item.FlowType.ToString(),
                    PressureDrop = item.BHAPressureDrop > 0 ? Math.Round(item.BHAPressureDrop, 3) : 0
                });
            }
            foreach (var itemInput in objHydCalSrvs.bhaInput)
            {
                objBha1.Add(new HydraulicBHAToolOutPutData
                {
                    PositionNum = itemInput.PositionNumber > 0 ? itemInput.PositionNumber : 0,
                    HydraulicOD = itemInput.OutsideDiameterInInch > 0 ? itemInput.OutsideDiameterInInch : 0,
                    HydraulicID = itemInput.InsideDiameterInInch > 0 ? itemInput.InsideDiameterInInch : 0
                });
            }

            foreach (var item in objBhaToolOutput)
            {
                var hyod = objBha1.Where(bt => bt.PositionNum == item.PositionNum)
                     .Select(bt => bt.HydraulicOD);
                var hyid = objBha1.Where(bt => bt.PositionNum == item.PositionNum)
                   .Select(bt => bt.HydraulicID);
                item.HydraulicOD = hyod.FirstOrDefault();
                item.HydraulicID = hyid.FirstOrDefault();
            }

            for (int i = 0; i < objBhaToolOutput.Count; i++)
            {
                lstTblBHAheader.Add(getBHAToolLine(objBhaToolOutput[i]));
                lstTblBhaSide.Add(getBhaToolSide(objBhaToolOutput[i]));
            }
            #endregion

            #region Hydraulic BHA Tools Graph section
            Dictionary<string, Array> dicBhaChart = new Dictionary<string, Array>();
            List<DataPoints> hyprodatapoints;
            byte[] hydraprograph;
            List<Image> graph = new List<Image>();

            for (int i = 0; i < objChartService.HydraulicOutputBHAList.Count; i++)
            {
                dicBhaChart.Add("HydraproLineSeries" + i, objChartService.HydraulicOutputBHAList[i].BHAchart["HydraproLineSeries"].ToArray());
            }

            for (int i = 0; i < dicBhaChart.Count; i++)
            {
                hyprodatapoints = new List<DataPoints>();
                foreach (WFT.UI.Common.Charts.XYValueModelForLineData<double> hyproitem in dicBhaChart["HydraproLineSeries" + i])
                {
                    hyprodatapoints.Add(new DataPoints
                    {
                        X = (float)hyproitem.PrimaryAxisValue,
                        Y = (float)hyproitem.SecondaryAxisValue
                    });
                }
                hydraprograph = DrawHydraulicToolsGraph(hyprodatapoints, objChartService.HydraulicOutputBHAList[i]);
                graph.Add(new Image(ImageDataFactory.Create(hydraprograph)));
                hyprodatapoints.Clear();
            }
            #endregion

            #endregion

            #region Report Section
            using (MemoryStream memoryStream = new MemoryStream())
            {
                using (PdfWriter writer = new PdfWriter(memoryStream))
                {
                    using (PdfDocument pdf = new PdfDocument(writer))
                    {
                        Document document = new Document(pdf, PageSize.A4, immediateFlush: false);
                        NewMethod(img, ls, newline,
                            header, tableAuthor, comment, tblToc, _headerinfo,
                            tblSegment, tblJobInformation, tblWellInformation, tblOriginator, tblCustomerContacts, tblWeatherfordContacts, tblGenInfo, tblApproval,
                            _casingLinerTubingInfo, tblDepthAnalysis, tblCasingLinerTube,
                            tblBhaData, tblSurfaceEquipment, tblFluidEnvelope, tblFluid,
                            _stdpipeHeader, imgStdPvsFlwRate, xscale, yscale,
                            PieChartTable, _chartheader, imgPie, legend, tblHeaderAnnulusOutput, dicLstAnnulusOutputData,
                            lstTblBHAheader, lstTblBhaSide, graph, document, pdf);

                        AddFooter(pdf, document, objInputData);
                        pdf.AddEventHandler(PdfDocumentEvent.END_PAGE, new PageEventHandler());
                        document.Close();
                    }

                    // After closing the document, the MemoryStream contains the PDF
                    return memoryStream.ToArray();
                }
            }


            #endregion
        }

        private void getGeneralInfo(PdfReportService objInputData, out string _tabheader, Dictionary<string, string> pdfHederInfoData, out Table tblGenInfo)
        {
            _tabheader = "General Information";
            pdfHederInfoData.Add("PL Job Number", objInputData.JobNumber != null ? objInputData.JobNumber.ToString() : "");
            pdfHederInfoData.Add("Project Name", objInputData.ProjectName != null ? objInputData.ProjectName.ToString() : "");
            pdfHederInfoData.Add("Customer Order Number", objInputData.CustomerOrderNumber != null ? objInputData.CustomerOrderNumber.ToString() : "");
            pdfHederInfoData.Add("Quote Number", objInputData.QuoteNumber != null ? objInputData.QuoteNumber.ToString() : "");
            pdfHederInfoData.Add("Rig Elevation", objInputData.RigElevation != null ? (objInputData.RigElevation.ToString() + " | ft") : "");
            pdfHederInfoData.Add("Reservoir Type", objInputData.ReservoirType != null ? objInputData.ReservoirType.ToString() : "");
            pdfHederInfoData.Add("Water Depth", objInputData.WaterDepth != null ? (objInputData.WaterDepth.ToString() + " | ft") : "");
            pdfHederInfoData.Add("Well Depth (TVD)", objInputData.WellDepth > 0.00 ? (objInputData.WellDepth.ToString() + " | ft") : "");
            pdfHederInfoData.Add("Rig Type", objInputData.RigType != null ? objInputData.RigType.ToString() : "");
            pdfHederInfoData.Add("Well Classification", objInputData.WellClassification != null ? objInputData.WellClassification.ToString() : "");
            pdfHederInfoData.Add("Work String", objInputData.WorkString != null ? objInputData.WorkString.ToString() : "");
            pdfHederInfoData.Add("Inclination", objInputData.Inclination != null ? objInputData.Inclination.ToString() : "");
            pdfHederInfoData.Add("Customer Type", objInputData.CustomerType != null ? objInputData.CustomerType.ToString() : "");
            pdfHederInfoData.Add("H2S Present", objInputData.H2SPresent != null ? objInputData.H2SPresent.ToString() : "");
            pdfHederInfoData.Add("CO2 Present", objInputData.CO2Present != null ? objInputData.CO2Present.ToString() : "0.00");
            pdfHederInfoData.Add("Total mileage to/from location", objInputData.TotalMileageTFLocation != null ? (objInputData.TotalMileageTFLocation.ToString() + " | miles") : "");
            pdfHederInfoData.Add("Total travel time to/from location", objInputData.TotalTravelTimeTFLlocation != null ? (objInputData.TotalTravelTimeTFLlocation.ToString() + " | hours") : "");
            pdfHederInfoData.Add("Total off-duty hours at location", objInputData.TotalOffDutyHrsAtLocation != null ? (objInputData.TotalOffDutyHrsAtLocation.ToString() + " | hours") : "");
            tblGenInfo = getHeaderInfoTable(pdfHederInfoData, _tabheader);
        }

        public byte[] GetPdfBytesFromFile(string filePath)
        {
            // Ensure the file exists to prevent FileNotFoundException
            if (!System.IO.File.Exists(filePath))
            {
                throw new FileNotFoundException("The specified file was not found.", filePath);
            }

            // Read the file into a byte array
            byte[] fileBytes = System.IO.File.ReadAllBytes(filePath);
            return fileBytes;
        }
        private static void NewMethod(Image img, LineSeparator ls, Paragraph newline,
            Paragraph header, Table tableAuthor, Paragraph comment, Table tblToc, Paragraph _headerinfo,
            Table tblSegment, Table tblJobInformation, Table tblWellInformation, Table tblOriginator, Table tblCustomerContacts, Table tblWeatherfordContacts, Table tblGenInfo, Table tblApproval,
            Paragraph _casingLinerTubingInfo, Table tblDepthAnalysis, Table tblCasingLinerTube,
            Table tblBhaData, Table tblSurfaceEquipment, Table tblFluidEnvelope, Table tblFluid,
            Paragraph _stdpipeHeader, Image imgStdPvsFlwRate, Paragraph xscale, Paragraph yscale,
            Table PieChartTable, Paragraph _chartheader, Image imgPie, Paragraph legend, Table tblHeaderAnnulusOutput, List<Table> dicLstAnnulusOutputData,
            List<Table> lstTblBHAheader, List<Table> lstTblBhaSide, List<Image> bhaToolGraphsLst, Document document, PdfDocument pdf)
        {
            #region Content First Page
            document.Add(img);
            document.Add(ls);
            document.Add(newline);
            document.Add(newline);
            document.Add(header);
            document.Add(newline);
            document.Add(newline);
            document.Add(tableAuthor);
            tableAuthor.Flush();
            tableAuthor.Complete();
            document.Add(newline);
            document.Add(newline);
            document.Add(comment);
            document.Add(new AreaBreak(AreaBreakType.NEXT_PAGE));
            #endregion

            #region Header for all Pages
            Table head1 = new Table(2, false)
                .SetBorder(Border.NO_BORDER);
            Cell headcell1 = new Cell(1, 1).SetBorder(Border.NO_BORDER);
            headcell1.Add(img).SetWidth(40);
            Cell headcell2 = new Cell(1, 1).SetBorder(Border.NO_BORDER).SetTextAlignment(TextAlignment.CENTER);
            headcell2.Add(_headerinfo).SetWidth(500); ;
            Cell headcell3 = new Cell(1, 2).SetBorder(Border.NO_BORDER);
            headcell3.Add(ls).SetWidth(500);
            head1.AddCell(headcell1);
            head1.AddCell(headcell2);
            head1.AddCell(headcell3);

            Table head2 = new Table(2, false)
                .SetBorder(Border.NO_BORDER);
            Cell head2cell1 = new Cell(1, 1).SetBorder(Border.NO_BORDER);
            head2cell1.Add(img).SetWidth(40);
            Cell head2cell2 = new Cell(1, 1).SetBorder(Border.NO_BORDER).SetTextAlignment(TextAlignment.CENTER);
            head2cell2.Add(_casingLinerTubingInfo).SetWidth(500);
            Cell head2cell3 = new Cell(1, 2).SetBorder(Border.NO_BORDER);
            head2cell3.Add(ls).SetWidth(500); ;

            head2.AddCell(head2cell1);
            head2.AddCell(head2cell2);
            head2.AddCell(head2cell3);
            #endregion

            #region Content of TOC
            document.Add(tblToc);
            tblToc.Flush();
            tblToc.Complete();
            document.Add(new AreaBreak(AreaBreakType.NEXT_PAGE));
            #endregion

            #region Content Header Information
            document.Add(head1);

            document.Add(tblSegment);
            tblSegment.Flush();
            tblSegment.Complete();

            document.Add(tblJobInformation);
            tblJobInformation.Flush();
            tblJobInformation.Complete();

            document.Add(tblWellInformation);
            tblWellInformation.Flush();
            tblWellInformation.Complete();

            document.Add(tblOriginator);
            tblOriginator.Flush();
            tblOriginator.Complete();

            document.Add(tblCustomerContacts);
            tblCustomerContacts.Flush();
            tblCustomerContacts.Complete();

            document.Add(tblWeatherfordContacts);
            tblWeatherfordContacts.Flush();
            tblWeatherfordContacts.Complete();

            document.Add(tblGenInfo);
            tblGenInfo.Flush();
            tblGenInfo.Complete();

            document.Add(tblApproval);
            tblApproval.Flush();
            tblApproval.Complete();
            document.Add(new AreaBreak(AreaBreakType.NEXT_PAGE));
            #endregion

            #region Content Casing / Liner / Tubing
            document.Add(head2);
            document.Add(newline);

            document.Add(tblDepthAnalysis);
            document.Add(newline);

            document.Add(tblCasingLinerTube);
            tblCasingLinerTube.Flush();
            tblCasingLinerTube.Complete();
            document.Add(new AreaBreak(AreaBreakType.NEXT_PAGE));
            #endregion

            #region Content Bottom Hole Assembly
            document.Add(head2);

            document.Add(newline);
            document.Add(tblBhaData);
            tblBhaData.Flush();
            tblBhaData.Complete();
            document.Add(new AreaBreak(AreaBreakType.NEXT_PAGE));
            #endregion

            #region Content Surface Equipment / Fluid Envelop / Fluid
            document.Add(head2);
            document.Add(newline);

            document.Add(tblSurfaceEquipment);
            tblSurfaceEquipment.Flush();
            tblSurfaceEquipment.Complete();
            document.Add(newline);

            document.Add(tblFluidEnvelope);
            document.Add(newline);

            document.Add(tblFluid);
            tblFluid.Flush();
            tblFluid.Complete();
            document.Add(new AreaBreak(AreaBreakType.NEXT_PAGE));
            #endregion

            #region Content Standpipe vs Flowrate
            document.Add(head2);

            document.Add(newline);
            document.Add(_stdpipeHeader);
            document.Add(newline);
            document.Add(xscale);
            document.Add(yscale);
            document.Add(imgStdPvsFlwRate);
            document.Add(new AreaBreak(AreaBreakType.NEXT_PAGE));
            #endregion

            #region Content Pressure Drop
            document.Add(head2);

            document.Add(PieChartTable);
            PieChartTable.Flush();
            PieChartTable.Complete();
            document.Add(newline);

            document.Add(_chartheader);
            document.Add(imgPie);
            document.Add(tblHeaderAnnulusOutput);
            for (int i = 0; i < dicLstAnnulusOutputData.Count; i++)
            {
                document.Add(dicLstAnnulusOutputData[i]);
            }

            document.Add(new AreaBreak(AreaBreakType.NEXT_PAGE));
            #endregion

            #region Content Line chart for every Tool
            Table bhatooldetail = new Table(2, true).SetBorder(Border.NO_BORDER);
            Cell celltd1;
            Cell celltd2;
            Cell celltd3;

            int graphCount = bhaToolGraphsLst.Count;
            int pageBreak = 0;
            document.Add(head2);
            for (int gp = 0; gp < graphCount; gp++)
            {
                document.Add(newline);
                celltd1 = new Cell(1, 2).Add(lstTblBHAheader[gp]);
                celltd2 = new Cell(1, 1).Add(lstTblBhaSide[gp]).SetPadding(10);
                celltd3 = new Cell(1, 1).Add(bhaToolGraphsLst[gp]);
                bhatooldetail.AddCell(celltd1);
                bhatooldetail.AddCell(celltd2);
                bhatooldetail.AddCell(celltd3);
                document.Add(bhatooldetail);
                pageBreak++;
                if (pageBreak == 2)
                {
                    document.Add(new AreaBreak(AreaBreakType.NEXT_PAGE));
                    pageBreak = 0;
                }
            }

            bhatooldetail.Flush();
            bhatooldetail.Complete();
            #endregion
        }

        #region Methods
        public Table getTableOfcontent(string strSubProductLine)
        {
            Table _tbltoc = new Table(2, true);

            Cell tocHeader = new Cell(1, 2).Add(new Paragraph("Table Of Contents")).SetFontSize(18).SetBold().SetBorder(Border.NO_BORDER);
            
            Cell tocLine1col1 = new Cell(1, 1).Add(new Paragraph("1.    Header Information")).SetFontSize(12).SetBorder(Border.NO_BORDER).SetTextAlignment(TextAlignment.LEFT);
            Cell tocLine1col2 = new Cell(1, 1).Add(new Paragraph("......3")).SetFontSize(12).SetBorder(Border.NO_BORDER).SetTextAlignment(TextAlignment.RIGHT);
            Cell tocLine2col1 = new Cell(1, 1).Add(new Paragraph("2.    " + strSubProductLine + " - Casing Liner Tubing")).SetFontSize(12).SetBorder(Border.NO_BORDER).SetTextAlignment(TextAlignment.LEFT);
            Cell tocLine2col2 = new Cell(1, 1).Add(new Paragraph("......4")).SetFontSize(12).SetBorder(Border.NO_BORDER).SetTextAlignment(TextAlignment.RIGHT);
            Cell tocLine3col1 = new Cell(1, 1).Add(new Paragraph("3.    "+ strSubProductLine + " - BHA")).SetFontSize(12).SetBorder(Border.NO_BORDER).SetTextAlignment(TextAlignment.LEFT);
            Cell tocLine3col2 = new Cell(1, 1).Add(new Paragraph("......5")).SetFontSize(12).SetBorder(Border.NO_BORDER).SetTextAlignment(TextAlignment.RIGHT);
            Cell tocLine4col1 = new Cell(1, 1).Add(new Paragraph("4.    "+ strSubProductLine + " - Surface Equipment & Fluid Information")).SetFontSize(12).SetBorder(Border.NO_BORDER).SetTextAlignment(TextAlignment.LEFT);
            Cell tocLine4col2 = new Cell(1, 1).Add(new Paragraph("......6")).SetFontSize(12).SetBorder(Border.NO_BORDER).SetTextAlignment(TextAlignment.RIGHT);
            Cell tocLine5col1 = new Cell(1, 1).Add(new Paragraph("5.    "+ strSubProductLine + " - Standpipe vs Flowrate Graph")).SetFontSize(12).SetBorder(Border.NO_BORDER).SetTextAlignment(TextAlignment.LEFT);
            Cell tocLine5col2 = new Cell(1, 1).Add(new Paragraph("......7")).SetFontSize(12).SetBorder(Border.NO_BORDER).SetTextAlignment(TextAlignment.RIGHT);
            Cell tocLine6col1 = new Cell(1, 1).Add(new Paragraph("6.    "+ strSubProductLine + " - Hydraulic Output")).SetFontSize(12).SetBorder(Border.NO_BORDER).SetTextAlignment(TextAlignment.LEFT);
            Cell tocLine6col2 = new Cell(1, 1).Add(new Paragraph("......8")).SetFontSize(12).SetBorder(Border.NO_BORDER).SetTextAlignment(TextAlignment.RIGHT);
            
            _tbltoc.AddCell(tocHeader);
            _tbltoc.AddCell(tocLine1col1);
            _tbltoc.AddCell(tocLine1col2);
            _tbltoc.AddCell(tocLine2col1);
            _tbltoc.AddCell(tocLine2col2);
            _tbltoc.AddCell(tocLine3col1);
            _tbltoc.AddCell(tocLine3col2);
            _tbltoc.AddCell(tocLine4col1);
            _tbltoc.AddCell(tocLine4col2);
            _tbltoc.AddCell(tocLine5col1);
            _tbltoc.AddCell(tocLine5col2);
            _tbltoc.AddCell(tocLine6col1);
            _tbltoc.AddCell(tocLine6col2);
            return _tbltoc;
        }
        private static void AddFooter(PdfDocument pdfDocument, Document document, PdfReportService objFooterData)
        {
            Paragraph footer = new Paragraph();
            List<PdfReportService> pdfFooter = new List<PdfReportService>();
            pdfFooter.Add(new PdfReportService
            {
                JobID = (objFooterData.JobID != null ? objFooterData.JobID.ToString() : ""),
                WPTSReportID = (objFooterData.WPTSReportID != null ? objFooterData.WPTSReportID.ToString() : ""),
                AccuViewVersion = (objFooterData.AccuViewVersion != null ? objFooterData.AccuViewVersion.ToString() : "")
            });

            Color footlineColor = new DeviceRgb(165, 42, 42);
            SolidLine line = new SolidLine(3f);
            line.SetColor(footlineColor);
            LineSeparator footerSeperator = new LineSeparator(line);

            // Creating Footer
            Table footerTable = new Table(3, false).SetFontSize(7);
            footerTable.AddCell("AccuView Job ID");
            footerTable.AddCell("WPTS Report ID");
            footerTable.AddCell("AccuView Version Number");

            foreach (PdfReportService item in pdfFooter)
            {
                footerTable.AddCell(item.JobID.ToString()).SetTextAlignment(TextAlignment.LEFT);
                footerTable.AddCell(item.WPTSReportID.ToString()).SetTextAlignment(TextAlignment.LEFT);
                footerTable.AddCell(item.AccuViewVersion.ToString()).SetTextAlignment(TextAlignment.LEFT);
            }

            Paragraph footerTradeMark = new Paragraph("AccuView" + "\u2122" + " is a Weatherford trademark").SetFontSize(7);

            Paragraph footerDisclaimer = new Paragraph("© 2015 WEATHERFORD - All Rights Reserved -Proprietary and Confidential: This document is copyrighted and contains valuable proprietary and confidential" +
                                       "information, whether patentable or unpatentable, of Weatherford. Recipients agree the document is loaned with confidential restrictions, and with the understanding that" +
                                        "neither it nor the information contained therein will be reproduced, used or disclosed in whole or in part for any purpose except as may be specifically authorized in" +
                                          "writing by Weatherford.This document shall be returned to Weatherford upon demand.").SetFontSize(7);

            Table finalfooter = new Table(1, false).SetBorder(Border.NO_BORDER);
            Cell _footcell = new Cell(1, 1).SetBorder(Border.NO_BORDER);
            _footcell.Add(footerSeperator).SetWidth(500);
            Cell _footcell1 = new Cell(2, 1).SetBorder(Border.NO_BORDER);
            _footcell1.Add(footerTable);
            Cell _footcell2 = new Cell(3, 1).SetBorder(Border.NO_BORDER);
            _footcell2.Add(footerTradeMark);
            Cell _footcell3 = new Cell(4, 1).SetBorder(Border.NO_BORDER);
            _footcell3.Add(footerDisclaimer).SetWidth(500);

            finalfooter.AddCell(_footcell);
            finalfooter.AddCell(_footcell1);
            finalfooter.AddCell(_footcell2);
            finalfooter.AddCell(_footcell3);

            footer.Add(finalfooter.SetAutoLayout());
            var numPages = pdfDocument.GetNumberOfPages();
            for (int pageId = 1; pageId <= numPages; pageId++)
            {
                var page = pdfDocument.GetPage(pageId);
                var leftMarginPosition = document.GetLeftMargin();
                document.ShowTextAligned(footer, leftMarginPosition, UnitConverter.mm2uu(50), pageId,
                    TextAlignment.LEFT, VerticalAlignment.MIDDLE, 0);
            }
        }
        public static class UnitConverter
        {
            public static float uu2inch(float uu) => uu / 72f;
            public static float inch2uu(float inch) => inch * 72f;
            public static float inch2mm(float inch) => inch * 25.4f;
            public static float mm2inch(float mm) => mm / 25.4f;
            public static float uu2mm(float uu) => inch2mm(uu2inch(uu));
            public static float mm2uu(float mm) => inch2uu(mm2inch(mm));
        }

        #region Graph and Chart Generate Method Section
        public Table getPieHeaderTable(ChartAndGraphService objPieTableData)
        {

            double toolflowrate = 0.00;
            double tooldepth = 0.00;
            double toolPressureDrop = 0.00;
            Table pHeader = new Table(3, true)
                .SetTextAlignment(TextAlignment.LEFT).SetBackgroundColor(ColorConstants.LIGHT_GRAY);
            foreach (var item in objPieTableData.HydraulicOutputBHAList)
            {
                if (item.InputFlowRate >= 0)
                    toolflowrate += item.InputFlowRate;
            }
            tooldepth = objPieTableData.ToolDepth;
            toolPressureDrop = objPieTableData.TotalPressureDrop;

            pHeader.AddCell(new Paragraph("Flow Rate : " + toolflowrate + " (gal/min)"));
            pHeader.AddCell(new Paragraph("Tool Depth : " + tooldepth + " (ft)"));
            pHeader.AddCell(new Paragraph("Flow Rate : " + toolPressureDrop + " (psi)"));

            return pHeader.SetAutoLayout();
        }
        public Table getBHAToolLine(HydraulicBHAToolOutPutData objData)
        {
            Table tblbhtoolhead = new Table(8, false)
                .SetBold().SetFontSize(7).SetWidth(UnitValue.CreatePercentValue(100));
            Cell _blankcell;

            Cell tblhead01 = new Cell(1, 1).Add(new Paragraph("").SetBackgroundColor(ColorConstants.LIGHT_GRAY));
            Cell tblhead02 = new Cell(1, 1).Add(new Paragraph("Work String").SetBackgroundColor(ColorConstants.LIGHT_GRAY).SetTextAlignment(TextAlignment.CENTER));
            Cell tblhead03 = new Cell(1, 1).Add(new Paragraph("Length (ft)").SetBackgroundColor(ColorConstants.LIGHT_GRAY).SetTextAlignment(TextAlignment.CENTER));
            Cell tblhead04 = new Cell(1, 1).Add(new Paragraph("Input Flow Rate (gal/min)").SetBackgroundColor(ColorConstants.LIGHT_GRAY).SetTextAlignment(TextAlignment.CENTER));
            Cell tblhead05 = new Cell(1, 1).Add(new Paragraph("Average Velocity (ft/sec)").SetBackgroundColor(ColorConstants.LIGHT_GRAY).SetTextAlignment(TextAlignment.CENTER));
            Cell tblhead06 = new Cell(1, 1).Add(new Paragraph("Critical Velocity (ft/sec)").SetBackgroundColor(ColorConstants.LIGHT_GRAY).SetTextAlignment(TextAlignment.CENTER));
            Cell tblhead07 = new Cell(1, 1).Add(new Paragraph("Flow").SetBackgroundColor(ColorConstants.LIGHT_GRAY).SetTextAlignment(TextAlignment.CENTER));
            Cell tblhead08 = new Cell(1, 1).Add(new Paragraph("Pressure Drop (psi)").SetBackgroundColor(ColorConstants.LIGHT_GRAY).SetTextAlignment(TextAlignment.CENTER));

            tblbhtoolhead.AddCell(tblhead01);
            tblbhtoolhead.AddCell(tblhead02);
            tblbhtoolhead.AddCell(tblhead03);
            tblbhtoolhead.AddCell(tblhead04);
            tblbhtoolhead.AddCell(tblhead05);
            tblbhtoolhead.AddCell(tblhead06);
            tblbhtoolhead.AddCell(tblhead07);
            tblbhtoolhead.AddCell(tblhead08);

            if (objData.AvgVelocityColor.ToString().ToUpper() == "RED")
            {
                _blankcell = new Cell(1, 1).SetBackgroundColor(ColorConstants.RED).Add(new Paragraph(" "));
            }
            else if (objData.AvgVelocityColor.ToString().ToUpper() == "YELLOW")
            {
                _blankcell = new Cell(1, 1).SetBackgroundColor(ColorConstants.YELLOW).Add(new Paragraph(" "));
            }
            else
            {
                _blankcell = new Cell(1, 1).SetBackgroundColor(ColorConstants.GREEN).Add(new Paragraph(" "));
            }

            Cell celWks = new Cell(1, 1).Add(new Paragraph(objData.WorkString)).SetTextAlignment(TextAlignment.CENTER);
            Cell celLen = new Cell(1, 1).Add(new Paragraph(objData.Length.ToString())).SetTextAlignment(TextAlignment.LEFT);
            Cell celInFlow = new Cell(1, 1).Add(new Paragraph(objData.InputFlowRate.ToString())).SetTextAlignment(TextAlignment.LEFT);
            Cell celAvgV;
            if (objData.AvgVelocityColor.ToString().ToUpper() == "RED")
            {
                celAvgV = new Cell(1, 1).Add(new Paragraph(objData.AverageVelocity.ToString())).SetTextAlignment(TextAlignment.LEFT).SetBackgroundColor(ColorConstants.RED);
            }
            else if (objData.AvgVelocityColor.ToString().ToUpper() == "YELLOW")
            {
                celAvgV = new Cell(1, 1).Add(new Paragraph(objData.AverageVelocity.ToString())).SetTextAlignment(TextAlignment.LEFT).SetBackgroundColor(ColorConstants.YELLOW);
            }
            else
            {
                celAvgV = new Cell(1, 1).Add(new Paragraph(objData.AverageVelocity.ToString())).SetTextAlignment(TextAlignment.LEFT).SetBackgroundColor(ColorConstants.GREEN);
            }
            Cell celCricVel = new Cell(1, 1).Add(new Paragraph(objData.CriticalVelocity.ToString())).SetTextAlignment(TextAlignment.LEFT);
            Cell celFlowtyp = new Cell(1, 1).Add(new Paragraph(objData.FlowType.ToString())).SetTextAlignment(TextAlignment.CENTER);
            Cell celPressureDrp = new Cell(1, 1).Add(new Paragraph(objData.PressureDrop.ToString())).SetTextAlignment(TextAlignment.LEFT);

            tblbhtoolhead.AddCell(_blankcell);
            tblbhtoolhead.AddCell(celWks);
            tblbhtoolhead.AddCell(celLen);
            tblbhtoolhead.AddCell(celInFlow);
            tblbhtoolhead.AddCell(celAvgV);
            tblbhtoolhead.AddCell(celCricVel);
            tblbhtoolhead.AddCell(celFlowtyp);
            tblbhtoolhead.AddCell(celPressureDrp);

            return tblbhtoolhead.SetAutoLayout();
        }
        public Table getBhaToolSide(HydraulicBHAToolOutPutData objData)
        {
            Table tblbhtoolSide = new Table(2, false)
                .SetTextAlignment(TextAlignment.LEFT).SetFontSize(7);

            Cell cell00 = new Cell(1, 1).Add(new Paragraph("Flow Type"));
            Cell cell01 = new Cell(1, 1).Add(new Paragraph(objData.FlowType));
            Cell cell10 = new Cell(1, 1).Add(new Paragraph("Average Velocity"));
            Cell cell11 = new Cell(1, 1).Add(new Paragraph(objData.AverageVelocity.ToString() + " | ft/sec"));
            Cell cell20 = new Cell(1, 1).Add(new Paragraph("Critical Velocity"));
            Cell cell21 = new Cell(1, 1).Add(new Paragraph(objData.CriticalVelocity.ToString() + " | ft/sec"));
            Cell cell30 = new Cell(1, 1).Add(new Paragraph("Pressure Drop"));
            Cell cell31 = new Cell(1, 1).Add(new Paragraph(objData.PressureDrop.ToString() + " | psi"));
            Cell cell40 = new Cell(1, 1).Add(new Paragraph("Hydraulic OD"));
            Cell cell41 = new Cell(1, 1).Add(new Paragraph(objData.HydraulicOD.ToString()));
            Cell cell50 = new Cell(1, 1).Add(new Paragraph("Hydraulic ID"));
            Cell cell51 = new Cell(1, 1).Add(new Paragraph(objData.HydraulicID.ToString()));
            Cell cell60 = new Cell(1, 1).Add(new Paragraph("Length"));
            Cell cell61 = new Cell(1, 1).Add(new Paragraph(objData.Length.ToString() + " | ft"));

            tblbhtoolSide.AddCell(cell00);
            tblbhtoolSide.AddCell(cell01);
            tblbhtoolSide.AddCell(cell10);
            tblbhtoolSide.AddCell(cell11);
            tblbhtoolSide.AddCell(cell20);
            tblbhtoolSide.AddCell(cell21);
            tblbhtoolSide.AddCell(cell30);
            tblbhtoolSide.AddCell(cell31);
            tblbhtoolSide.AddCell(cell40);
            tblbhtoolSide.AddCell(cell41);
            tblbhtoolSide.AddCell(cell50);
            tblbhtoolSide.AddCell(cell51);
            tblbhtoolSide.AddCell(cell60);
            tblbhtoolSide.AddCell(cell61);

            return tblbhtoolSide.SetAutoLayout();
        }
        public byte[] GeneratePieChart(Dictionary<string, string> objPrsDrop, List<PieData> pieData)
        {
            float width = 250;
            float height = 250;
            float margin = 15;
            using (var surfcae = SKSurface.Create(new SKImageInfo(400, 300)))
            {
                var canvas = surfcae.Canvas;
                canvas.Clear(SKColors.White);
                float centerX = width / 2f;
                float centerY = height / 2f;
                float radius = Math.Min(width, height) * 0.8f;

                float totalValue = 0;

                foreach (var itemval in objPrsDrop.Keys)
                {
                    if (itemval.Substring(0, 5) == "Value")
                    {
                        string fltVale = !string.IsNullOrEmpty(objPrsDrop[itemval]) ? objPrsDrop[itemval].ToString() : "0";
                        float itmValue = (float)Math.Round(float.Parse(fltVale));
                        totalValue += itmValue;
                    }
                }

                float startAngle = 0;
                float sweepAngle = 0;
                foreach (var lstitem in objPrsDrop.Keys)
                {
                    if (lstitem.Substring(0, 5) == "Value")
                    {
                        string fltVal = !string.IsNullOrEmpty(objPrsDrop[lstitem]) ? objPrsDrop[lstitem].ToString() : "0";
                        float itValue = (float)Math.Round(float.Parse(fltVal));
                        sweepAngle = (itValue / totalValue) * 360f;
                    }
                    if (lstitem.Substring(0, 5) == "Color")
                    {
                        using (var paint = new SKPaint())
                        {
                            string colourName = objPrsDrop[lstitem].ToString();

                            string hexString = ViewModel.ColorConverter.ColorNameToHexString(colourName);

                            paint.Color = SKColor.Parse(hexString);
                            canvas.DrawArc(new SKRect(50, 50, 200, 200), startAngle, sweepAngle, true, paint);
                            startAngle += sweepAngle;
                        }
                    }
                }

                float legendX = 400 - margin - 150;
                float legendY = margin;
                float legendItemHeight = 8;
                foreach (var data in pieData)
                {
                    using (var paint = new SKPaint())
                    {
                        string colourName = data.Color;
                        string hexString = ViewModel.ColorConverter.ColorNameToHexString(colourName);

                        paint.Color = SKColor.Parse(hexString);
                        canvas.DrawRect(legendX, legendY, legendItemHeight, legendItemHeight, paint);
                    }
                    float labelPercentage = (float)Math.Round((data.Value / totalValue) * 100);
                    canvas.DrawText($"({labelPercentage:F2} %) => {data.Label}", legendX + legendItemHeight + 5, legendY + 8, new SKPaint());
                    legendY += legendItemHeight + 5;
                }

                using (var image = surfcae.Snapshot())
                using (var data = image.Encode(SKEncodedImageFormat.Png, 100))
                {
                    using (MemoryStream stream = new MemoryStream())
                    {
                        data.SaveTo(stream);
                        return stream.ToArray();
                    }
                }
            }
        }
        public Table getAnnulusOutputTblHead()
        {
            Table tblhead = new Table(10, false).SetFontSize(7).SetWidth(UnitValue.CreatePercentValue(100));
            Cell tblhead01 = new Cell(1, 1).Add(new Paragraph(" ").SetBackgroundColor(ColorConstants.LIGHT_GRAY)).SetWidth(10);
            Cell tblhead02 = new Cell(1, 1).Add(new Paragraph("Annulus").SetBackgroundColor(ColorConstants.LIGHT_GRAY).SetTextAlignment(TextAlignment.CENTER)).SetWidth(40);
            Cell tblhead03 = new Cell(1, 1).Add(new Paragraph("WorkString").SetBackgroundColor(ColorConstants.LIGHT_GRAY).SetTextAlignment(TextAlignment.CENTER)).SetWidth(100);
            Cell tblhead04 = new Cell(1, 1).Add(new Paragraph("From (ft)").SetBackgroundColor(ColorConstants.LIGHT_GRAY).SetTextAlignment(TextAlignment.CENTER)).SetWidth(30);
            Cell tblhead05 = new Cell(1, 1).Add(new Paragraph("To (ft)").SetBackgroundColor(ColorConstants.LIGHT_GRAY).SetTextAlignment(TextAlignment.CENTER)).SetWidth(30);
            Cell tblhead06 = new Cell(1, 1).Add(new Paragraph("Average Velocity (ft/min)").SetBackgroundColor(ColorConstants.LIGHT_GRAY).SetTextAlignment(TextAlignment.CENTER)).SetWidth(50);
            Cell tblhead07 = new Cell(1, 1).Add(new Paragraph("Critical Velocity (ft/min)").SetBackgroundColor(ColorConstants.LIGHT_GRAY).SetTextAlignment(TextAlignment.CENTER)).SetWidth(50);
            Cell tblhead08 = new Cell(1, 1).Add(new Paragraph("Flow").SetBackgroundColor(ColorConstants.LIGHT_GRAY).SetTextAlignment(TextAlignment.CENTER)).SetWidth(70);
            Cell tblhead09 = new Cell(1, 1).Add(new Paragraph("Chip Rate (ft/min)").SetBackgroundColor(ColorConstants.LIGHT_GRAY).SetTextAlignment(TextAlignment.CENTER)).SetWidth(50);
            Cell tblhead10 = new Cell(1, 1).Add(new Paragraph("Pressure Drop (psi)").SetBackgroundColor(ColorConstants.LIGHT_GRAY).SetTextAlignment(TextAlignment.CENTER)).SetWidth(50);

            tblhead.AddCell(tblhead01);
            tblhead.AddCell(tblhead02);
            tblhead.AddCell(tblhead03);
            tblhead.AddCell(tblhead04);
            tblhead.AddCell(tblhead05);
            tblhead.AddCell(tblhead06);
            tblhead.AddCell(tblhead07);
            tblhead.AddCell(tblhead08);
            tblhead.AddCell(tblhead09);
            tblhead.AddCell(tblhead10);
            return tblhead.SetAutoLayout();
        }
        public Table getAnnulusTableData(HydraulicAnalysisAnnulusOutputData objHydrAnnulus)
        {
            Table _tblannulusdata = new Table(10, false)
                .SetFontSize(7).SetWidth(UnitValue.CreatePercentValue(100));
            Cell _blankcell;

            if (objHydrAnnulus.AnnulusColor.ToString().ToUpper() == "RED")
            {
                _blankcell = new Cell(1, 1).SetBackgroundColor(ColorConstants.RED).Add(new Paragraph(" ")).SetWidth(10);
            }
            else if (objHydrAnnulus.AnnulusColor.ToString().ToUpper() == "YELLOW")
            {
                _blankcell = new Cell(1, 1).SetBackgroundColor(ColorConstants.YELLOW).Add(new Paragraph(" ")).SetWidth(10);
            }
            else
            {
                _blankcell = new Cell(1, 1).SetBackgroundColor(ColorConstants.GREEN).Add(new Paragraph(" ")).SetWidth(10);
            }
            Cell cellAnnulus = new Cell(1, 1).Add(new Paragraph(objHydrAnnulus.Annulus.ToString()).SetTextAlignment(TextAlignment.CENTER)).SetWidth(40);
            Cell celWks = new Cell(1, 1).Add(new Paragraph(objHydrAnnulus.WorkString.ToString()).SetTextAlignment(TextAlignment.CENTER)).SetWidth(100);
            Cell celFrom = new Cell(1, 1).Add(new Paragraph(objHydrAnnulus.From.ToString())).SetTextAlignment(TextAlignment.LEFT).SetWidth(30);
            Cell celTo = new Cell(1, 1).Add(new Paragraph(objHydrAnnulus.To.ToString())).SetTextAlignment(TextAlignment.LEFT).SetWidth(30);
            Cell celAvgV;
            if (objHydrAnnulus.AvgVelocityColor.ToString().ToUpper() == "RED")
            {
                celAvgV = new Cell(1, 1).Add(new Paragraph(objHydrAnnulus.AverageVelocity.ToString())).SetTextAlignment(TextAlignment.LEFT).SetBackgroundColor(ColorConstants.RED).SetWidth(50);
            }
            else if (objHydrAnnulus.AvgVelocityColor.ToString().ToUpper() == "YELLOW")
            {
                celAvgV = new Cell(1, 1).Add(new Paragraph(objHydrAnnulus.AverageVelocity.ToString())).SetTextAlignment(TextAlignment.LEFT).SetBackgroundColor(ColorConstants.YELLOW).SetWidth(50);
            }
            else
            {
                celAvgV = new Cell(1, 1).Add(new Paragraph(objHydrAnnulus.AverageVelocity.ToString())).SetTextAlignment(TextAlignment.LEFT).SetBackgroundColor(ColorConstants.GREEN).SetWidth(50);
            }
            Cell celCricVel = new Cell(1, 1).Add(new Paragraph(objHydrAnnulus.CriticalVelocity.ToString())).SetTextAlignment(TextAlignment.LEFT).SetWidth(50);
            Cell celFlowtyp = new Cell(1, 1).Add(new Paragraph(objHydrAnnulus.Flow.ToString())).SetTextAlignment(TextAlignment.CENTER).SetWidth(70);
            Cell celChipcolor;
            if (objHydrAnnulus.ChipRateColor.ToString().ToUpper() == "RED")
            {
                celChipcolor = new Cell(1, 1).Add(new Paragraph(objHydrAnnulus.ChipRate.ToString())).SetTextAlignment(TextAlignment.LEFT).SetBackgroundColor(ColorConstants.RED).SetWidth(50);
            }
            else if (objHydrAnnulus.ChipRateColor.ToString().ToUpper() == "YELLOW")
            {
                celChipcolor = new Cell(1, 1).Add(new Paragraph(objHydrAnnulus.ChipRate.ToString())).SetTextAlignment(TextAlignment.LEFT).SetBackgroundColor(ColorConstants.YELLOW).SetWidth(50);
            }
            else
            {
                celChipcolor = new Cell(1, 1).Add(new Paragraph(objHydrAnnulus.ChipRate.ToString())).SetTextAlignment(TextAlignment.LEFT).SetBackgroundColor(ColorConstants.GREEN).SetWidth(50);
            }

            Cell celPressureDrp = new Cell(1, 1).Add(new Paragraph(objHydrAnnulus.PressureDrop.ToString())).SetTextAlignment(TextAlignment.LEFT).SetWidth(50);

            _tblannulusdata.AddCell(_blankcell);
            _tblannulusdata.AddCell(cellAnnulus);
            _tblannulusdata.AddCell(celWks);
            _tblannulusdata.AddCell(celFrom);
            _tblannulusdata.AddCell(celTo);
            _tblannulusdata.AddCell(celAvgV);
            _tblannulusdata.AddCell(celCricVel);
            _tblannulusdata.AddCell(celFlowtyp);
            _tblannulusdata.AddCell(celChipcolor);
            _tblannulusdata.AddCell(celPressureDrp);

            return _tblannulusdata.SetAutoLayout();
        }
        public byte[] DrawLineGraph(ChartAndGraphService objCags, List<DataPoints> dataPoints)
        {
            using (var surfcae = SKSurface.Create(new SKImageInfo(400, 300)))
            {
                var canvas = surfcae.Canvas;
                canvas.Clear(SKColors.White);
                float width = 400;
                float height = 300;
                float margin = 30;

                float graphWidth = width - 2 * margin;
                float graphHeight = height - 2 * margin;
                using (var paint = new SKPaint { Color = SKColors.Black, StrokeWidth = 1 })
                {
                    canvas.DrawLine(margin, margin, margin, height - margin, paint); // Y-axis
                    canvas.DrawLine(margin, height - margin, width - margin, height - margin, paint); // X-axis

                    // Default Point for x-axis and y-axis
                    canvas.DrawText("0", new SKPoint(margin, height + 15 - margin), paint);

                    var xlimit = objCags.MaxFlowRate;
                    var ylimit = objCags.MaxPressure;
                    var xfixPoint = objCags.ObservedPressure;
                    var yfixPoint = objCags.MaxPressure;

                    float xpoint = margin;
                    for (int i = 1; i <= xlimit; i++)
                    {
                        
                    }
                    float ypoint = height + 10 - margin;
                    for (int j = 1; j <= ylimit; j++)
                    {
                        ypoint -= 50;
                        int y = j * 1000;
                        int x = (int)(margin);
                        string yScale = Convert.ToString(y);
                        canvas.DrawLine(x-5,ypoint,x+5,ypoint,paint);
                        canvas.DrawText(yScale, new SKPoint(0, ypoint), paint);
                    }
                    
                }

                float minX = (float)objCags.MinimumFlowRate;
                float maxX = (float)objCags.MaxFlowRate;
                float minY = (float)objCags.ObservedPressure;
                float maxY = (float)objCags.MaxPressure;

                foreach (var point in dataPoints)
                {
                    minX = Math.Min(minX, point.X);
                    maxX = Math.Max(maxX, point.X);
                    minY = Math.Min(minY, point.Y);
                    maxY = Math.Max(maxY, point.Y);
                }
                
                using (var paint = new SKPaint { Color = SKColors.Red, StrokeWidth = 2, IsAntialias = true })
                {
                    float scaleX = graphWidth / (maxX - minX);
                    float scaleY = graphHeight / (maxY - minY);
                    canvas.DrawText("X", minX, minY, paint);

                    for (int i = 0; i < dataPoints.Count - 1; i++)
                    {
                        float x1 = margin + (dataPoints[i].X - minX) * scaleX;
                        float y1 = height - margin - (dataPoints[i].Y - minY) * scaleY;
                        float x2 = margin + (dataPoints[i + 1].X - minX) * scaleX;
                        float y2 = height - margin - (dataPoints[i + 1].Y - minY) * scaleY;

                        if (dataPoints[i].LineClr == "Yellow")
                        {
                            string colourName = dataPoints[i].LineClr;
                            string hexString = ViewModel.ColorConverter.ColorNameToHexString(colourName);
                            paint.Color = SKColor.Parse(hexString);
                            canvas.DrawLine(x1, y1, x2, y2, paint);
                        }
                        else if (dataPoints[i].LineClr == "Green")
                        {
                            string colourName = dataPoints[i].LineClr;
                            string hexString = ViewModel.ColorConverter.ColorNameToHexString(colourName);
                            paint.Color = SKColor.Parse(hexString);
                            canvas.DrawLine(x1, y1, x2, y2, paint);
                        }
                        else
                        {
                            canvas.DrawLine(x1, y1, x2, y2, paint);
                        }

                    }
                }

                // Convert bitmap to byte array
                using (var image = surfcae.Snapshot())
                using (var data = image.Encode(SKEncodedImageFormat.Png, 100))
                {
                    using (MemoryStream stream = new MemoryStream())
                    {
                        data.SaveTo(stream);
                        return stream.ToArray();
                    }
                }
            }
        }
        public byte[] DrawHydraulicToolsGraph(List<DataPoints> hyprobhadataPoints, ViewModel.HydraulicOutputBHAViewModel service)
        {
            using (var surfcae = SKSurface.Create(new SKImageInfo(200, 200)))
            {
                var canvas = surfcae.Canvas;
                canvas.Clear(SKColors.White);
                float width = 200;
                float height = 200;
                float margin = 30;

                float graphWidth = width - 2 * margin;
                float graphHeight = height - 2 * margin;
                using (var paint = new SKPaint { Color = SKColors.Black, StrokeWidth = 1 })
                {
                    canvas.DrawLine(margin, margin, margin, height - margin, paint); // Y-axis
                    canvas.DrawLine(margin, height - margin, width - margin, height - margin, paint); // X-axis

                    // Default Point for x-axis and y-axis
                    canvas.DrawText("0", new SKPoint(margin, height + 15 - margin), paint);

                    var xlimit = service.InputFlowRate;
                    var ylimit = service.BHAPressureDrop;
                    var xfixPoint = service.InputFlowRate;
                    var yfixPoint = service.BHAPressureDrop;

                    float xpoint = margin;
                    for (int i = 1; i <= xlimit; i++)
                    {
                        int x = i * 100;
                        int y = (int)(height - margin);
                        string xScale = Convert.ToString(x);
                        xpoint += 50;
                        canvas.DrawLine(xpoint, y - 5, xpoint, y + 5, paint);
                        canvas.DrawText(xScale, xpoint - 5, (height + 15 - margin), paint);
                    }
                    float ypoint = height + 10 - margin;
                    for (int j = 1; j <= ylimit; j++)
                    {
                        int y = 0;
                        ypoint -= 50;
                        if(yfixPoint < 100)
                        {
                            y = j * 10; 
                        }
                        else
                        {
                            y = j * 1000;
                        }
                        
                        int x = (int)(margin);
                        string yScale = Convert.ToString(y);
                        canvas.DrawLine(x - 5, ypoint, x + 5, ypoint, paint);
                        canvas.DrawText(yScale, new SKPoint(0, ypoint), paint);
                    }
                    canvas.DrawText("X", (float)xfixPoint, (float)yfixPoint, paint);
                }

                float minX = (float)service.InputFlowRate;
                float maxX = (float)service._maxFlowRate;
                float minY = (float)service.BHAPressureDrop;
                float maxY = (float)service._maxPressure;

                foreach (var point in hyprobhadataPoints)
                {
                    minX = Math.Min(minX, point.X);
                    maxX = Math.Max(maxX, point.X);
                    minY = Math.Min(minY, point.Y);
                    maxY = Math.Max(maxY, point.Y);
                }

                using (var paint = new SKPaint { Color = SKColors.Blue, StrokeWidth = 2, IsAntialias = true })
                {
                    float scaleX = graphWidth / (maxX - minX);
                    float scaleY = graphHeight / (maxY - minY);
                    for (int i = 0; i < hyprobhadataPoints.Count - 1; i++)
                    {
                        float x1 = margin + (hyprobhadataPoints[i].X - minX) * scaleX;
                        float y1 = height - margin - (hyprobhadataPoints[i].Y - minY) * scaleY;
                        float x2 = margin + (hyprobhadataPoints[i + 1].X - minX) * scaleX;
                        float y2 = height - margin - (hyprobhadataPoints[i + 1].Y - minY) * scaleY;
                        canvas.DrawLine(x1, y1, x2, y2, paint);
                    }
                }

                // Convert bitmap to byte array
                using (var image = surfcae.Snapshot())
                using (var data = image.Encode(SKEncodedImageFormat.Png, 100))
                {
                    using (MemoryStream stream = new MemoryStream())
                    {
                        data.SaveTo(stream);
                        return stream.ToArray();
                    }
                }
            }
        }
        #endregion

        #region Header Section
        public Paragraph getHeader(string rptHeader)
        {
            Paragraph _headertext = new Paragraph(rptHeader)
            .SetTextAlignment(TextAlignment.CENTER)
              .SetFontSize(20).SetBold().SetFontColor(accuColor);
            return _headertext;
        }
        public Table getTableContent(Dictionary<string, string> objDic)
        {
            Table _table = new Table(2, true);

            foreach (var item in objDic)
            {
                _table.AddCell(new Paragraph(item.Key).SetTextAlignment(TextAlignment.LEFT));
                _table.AddCell(new Paragraph(item.Value).SetTextAlignment(TextAlignment.LEFT).SetWidth(150));
            }
            //doc.Add(table);
            return _table.SetAutoLayout();
        }
        public Table getHeaderInfoTable(Dictionary<string, string> objSegment, string tabHeader)
        {
            string tabheadertext = tabHeader;
            string headertype = tabheadertext.Substring(0, 8).ToUpper();
            Cell cellChoiceKey = new Cell();
            Cell cellChoiceVal = new Cell();

            Table _tableSeg = new Table(4, true);
            _tableSeg.SetFontSize(7);
            Cell sn = new Cell(1, 4).SetTextAlignment(TextAlignment.LEFT).Add(new Paragraph(tabheadertext).SetBackgroundColor(ColorConstants.LIGHT_GRAY));
            _tableSeg.AddHeaderCell(sn);

            switch (headertype)
            {
                case "APPROVAL":
                    {
                        cellChoiceKey = new Cell(2, 1).SetTextAlignment(TextAlignment.LEFT).Add(new Paragraph("Status"));
                        cellChoiceVal = new Cell(2, 4).SetTextAlignment(TextAlignment.LEFT).Add(new Paragraph(objSegment["Status"].ToString()));
                        _tableSeg.AddCell(cellChoiceKey);
                        _tableSeg.AddCell(cellChoiceVal);
                        break;
                    }
                case "ORIGINAT":
                    {
                        cellChoiceKey = new Cell(2, 1).SetTextAlignment(TextAlignment.LEFT).Add(new Paragraph("WFRD Location"));
                        cellChoiceVal = new Cell(2, 4).SetTextAlignment(TextAlignment.LEFT).Add(new Paragraph(objSegment["WFRD Location"].ToString()));
                        _tableSeg.AddCell(cellChoiceKey);
                        _tableSeg.AddCell(cellChoiceVal);
                        break;
                    }
                default:
                    {
                        break;
                    }
            }

            foreach (var item in objSegment)
            {
                if (item.Key.ToUpper() == "STATUS") { }
                else if (item.Key.ToUpper() == "WFRD LOCATION") { }
                else
                {
                    _tableSeg.AddCell(new Paragraph(item.Key).SetTextAlignment(TextAlignment.LEFT));
                    _tableSeg.AddCell(new Paragraph(item.Value).SetTextAlignment(TextAlignment.LEFT));
                }
            }
            return _tableSeg.SetAutoLayout();
        }
        #endregion

        #region Casing / Liner / Tubing
        public Table getDepthAnalysis(List<string> objDpthAnalysisValue, string tablehead)
        {
            int count = 3;
            string _tblheadText = tablehead;

            Table _tabDepth = new Table(2, false);
            _tabDepth.SetFontSize(8);

            Cell da = new Cell(1, 2).SetTextAlignment(TextAlignment.LEFT).Add(new Paragraph(_tblheadText).SetBackgroundColor(ColorConstants.LIGHT_GRAY).SetWidth(200));
            _tabDepth.AddHeaderCell(da);

            for (int i = 1; i <= count; i++)
            {
                switch (i)
                {
                    case 1:
                        {
                            Cell daAnnulusLength = new Cell(1, 1).SetTextAlignment(TextAlignment.LEFT).Add(new Paragraph("Annulus Length").SetWidth(100));
                            Cell daAnLen = new Cell(1, 1).SetTextAlignment(TextAlignment.LEFT).Add(new Paragraph(objDpthAnalysisValue[0]).SetWidth(100));
                            _tabDepth.AddCell(daAnnulusLength);
                            _tabDepth.AddCell(daAnLen);
                            break;
                        }
                    case 2:
                        {
                            Cell daBHALength = new Cell(1, 1).SetTextAlignment(TextAlignment.LEFT).Add(new Paragraph("BHA Length").SetWidth(100));
                            Cell daBhaLen = new Cell(1, 1).SetTextAlignment(TextAlignment.LEFT).Add(new Paragraph(objDpthAnalysisValue[1]).SetWidth(100));
                            _tabDepth.AddCell(daBHALength);
                            _tabDepth.AddCell(daBhaLen);
                            break;
                        }
                    case 3:
                        {
                            Cell daToolLength = new Cell(1, 1).SetTextAlignment(TextAlignment.LEFT).Add(new Paragraph("Tool Depth").SetWidth(100));
                            Cell daTulDpth = new Cell(1, 1).SetTextAlignment(TextAlignment.LEFT).Add(new Paragraph(objDpthAnalysisValue[2]).SetWidth(100));
                            _tabDepth.AddCell(daToolLength);
                            _tabDepth.AddCell(daTulDpth);
                            break;
                        }
                    default:
                        break;
                }
            }

            return _tabDepth.SetAutoLayout();
        }

        public Table getCasingLinerTubing(Dictionary<string, string> objclt, string tablehead)
        {
            string _tblcltheader = tablehead;

            Table _tabclt = new Table(8, true);
            _tabclt.SetFontSize(8);
            Cell cltheadcell = new Cell(1, 8).SetTextAlignment(TextAlignment.LEFT).Add(new Paragraph(_tblcltheader).SetBackgroundColor(ColorConstants.LIGHT_GRAY));
            _tabclt.AddHeaderCell(cltheadcell);

            Cell cltid = new Cell(1, 1).SetTextAlignment(TextAlignment.LEFT).Add(new Paragraph("#").SetBackgroundColor(ColorConstants.LIGHT_GRAY));
            Cell cltwellsection = new Cell(1, 1).SetTextAlignment(TextAlignment.LEFT).Add(new Paragraph("Wellbore Section").SetBackgroundColor(ColorConstants.LIGHT_GRAY));
            Cell cltoutdia = new Cell(1, 1).SetTextAlignment(TextAlignment.LEFT).Add(new Paragraph("OD (in)").SetBackgroundColor(ColorConstants.LIGHT_GRAY));
            Cell cltindia = new Cell(1, 1).SetTextAlignment(TextAlignment.LEFT).Add(new Paragraph("ID (in)").SetBackgroundColor(ColorConstants.LIGHT_GRAY));
            Cell cltweight = new Cell(1, 1).SetTextAlignment(TextAlignment.LEFT).Add(new Paragraph("Weight (lbs/ft)").SetBackgroundColor(ColorConstants.LIGHT_GRAY));
            Cell cltgrade = new Cell(1, 1).SetTextAlignment(TextAlignment.LEFT).Add(new Paragraph("Grade").SetBackgroundColor(ColorConstants.LIGHT_GRAY));
            Cell clttop = new Cell(1, 1).SetTextAlignment(TextAlignment.LEFT).Add(new Paragraph("Top Depth (ft)").SetBackgroundColor(ColorConstants.LIGHT_GRAY));
            Cell cltbottom = new Cell(1, 1).SetTextAlignment(TextAlignment.LEFT).Add(new Paragraph("Bottom Depth (ft)").SetBackgroundColor(ColorConstants.LIGHT_GRAY));

            _tabclt.AddCell(cltid);
            _tabclt.AddCell(cltwellsection);
            _tabclt.AddCell(cltoutdia);
            _tabclt.AddCell(cltindia);
            _tabclt.AddCell(cltweight);
            _tabclt.AddCell(cltgrade);
            _tabclt.AddCell(clttop);
            _tabclt.AddCell(cltbottom);

            foreach (var item in objclt.Keys)
            {
                string addtocell = string.IsNullOrEmpty(objclt[item]) ? "" : objclt[item].ToString();
                Cell cltidv = new Cell(1, 1).SetTextAlignment(TextAlignment.LEFT).Add(new Paragraph(addtocell));
                _tabclt.AddCell(cltidv);
            }
            return _tabclt.SetAutoLayout();
        }

        #endregion

        #region Bottom Hole Assembly
        public Table getBha(Dictionary<string, string> objbhaitem, string bhaheadtext)
        {
            string _tblbhaheader = bhaheadtext;

            Table _tblBHA = new Table(13, true);
            _tblBHA.SetFontSize(8);
            Cell _bhaheadcell = new Cell(1, 13).SetTextAlignment(TextAlignment.LEFT).Add(new Paragraph(_tblbhaheader).SetBackgroundColor(ColorConstants.LIGHT_GRAY).SetBold());
            _tblBHA.AddHeaderCell(_bhaheadcell);

            Cell bhaid = new Cell(1, 1).SetTextAlignment(TextAlignment.LEFT).Add(new Paragraph("#").SetBackgroundColor(ColorConstants.LIGHT_GRAY).SetBold());
            Cell bhatooldesc = new Cell(1, 1).SetTextAlignment(TextAlignment.LEFT).Add(new Paragraph("Tool Description").SetBackgroundColor(ColorConstants.LIGHT_GRAY).SetBold());
            Cell bhaserialno = new Cell(1, 1).SetTextAlignment(TextAlignment.LEFT).Add(new Paragraph("Serial Number").SetBackgroundColor(ColorConstants.LIGHT_GRAY).SetBold());
            Cell bhamod = new Cell(1, 1).SetTextAlignment(TextAlignment.LEFT).Add(new Paragraph("Measured OD (in)").SetBackgroundColor(ColorConstants.LIGHT_GRAY).SetBold());
            Cell bhainndia = new Cell(1, 1).SetTextAlignment(TextAlignment.LEFT).Add(new Paragraph("ID (in)").SetBackgroundColor(ColorConstants.LIGHT_GRAY).SetBold());
            Cell bhaweight = new Cell(1, 1).SetTextAlignment(TextAlignment.LEFT).Add(new Paragraph("Weight (lbs)").SetBackgroundColor(ColorConstants.LIGHT_GRAY).SetBold());
            Cell bhatoollength = new Cell(1, 1).SetTextAlignment(TextAlignment.LEFT).Add(new Paragraph("Length (ft)").SetBackgroundColor(ColorConstants.LIGHT_GRAY).SetBold());
            Cell bhaupcontyp = new Cell(1, 1).SetTextAlignment(TextAlignment.LEFT).Add(new Paragraph("Upper Conn. Type").SetBackgroundColor(ColorConstants.LIGHT_GRAY).SetBold());
            Cell bhalowcontyp = new Cell(1, 1).SetTextAlignment(TextAlignment.LEFT).Add(new Paragraph("Lower Conn. Type").SetBackgroundColor(ColorConstants.LIGHT_GRAY).SetBold());
            Cell bhafishneckod = new Cell(1, 1).SetTextAlignment(TextAlignment.LEFT).Add(new Paragraph("Fish Neck OD(in)").SetBackgroundColor(ColorConstants.LIGHT_GRAY).SetBold());
            Cell bhafishnecklen = new Cell(1, 1).SetTextAlignment(TextAlignment.LEFT).Add(new Paragraph("Fish Neck Length (ft)").SetBackgroundColor(ColorConstants.LIGHT_GRAY).SetBold());
            Cell bhahydod = new Cell(1, 1).SetTextAlignment(TextAlignment.LEFT).Add(new Paragraph("Hydraulic OD(in)").SetBackgroundColor(ColorConstants.LIGHT_GRAY).SetBold());
            Cell bhahydind = new Cell(1, 1).SetTextAlignment(TextAlignment.LEFT).Add(new Paragraph("Hydraulic ID(in)").SetBackgroundColor(ColorConstants.LIGHT_GRAY).SetBold());

            _tblBHA.AddCell(bhaid);
            _tblBHA.AddCell(bhatooldesc);
            _tblBHA.AddCell(bhaserialno);
            _tblBHA.AddCell(bhamod);
            _tblBHA.AddCell(bhainndia);
            _tblBHA.AddCell(bhaweight);
            _tblBHA.AddCell(bhatoollength);
            _tblBHA.AddCell(bhaupcontyp);
            _tblBHA.AddCell(bhalowcontyp);
            _tblBHA.AddCell(bhafishneckod);
            _tblBHA.AddCell(bhafishnecklen);
            _tblBHA.AddCell(bhahydod);
            _tblBHA.AddCell(bhahydind);

            foreach (var item in objbhaitem.Keys)
            {
                string addtocell = string.IsNullOrEmpty(objbhaitem[item]) ? "" : objbhaitem[item].ToString();
                Cell bhaidv = new Cell(1, 1).SetTextAlignment(TextAlignment.LEFT).Add(new Paragraph(addtocell));
                _tblBHA.AddCell(bhaidv);
            }

            return _tblBHA.SetAutoLayout();
        }
        #endregion

        #region Fluid Envelope
        private double getSurfaceEquipmentTotalLength(string equipmenttyp)
        {
            double tlength = 0.00;
            switch (equipmenttyp)
            {
                case "Case1":
                    {
                        tlength = 124.00;
                        break;
                    }
                case "Case2":
                    {
                        tlength = 140.00;
                        break;
                    }
                case "Case3":
                    {
                        tlength = 145.00;
                        break;
                    }
                case "Case4":
                    {
                        tlength = 146.00;
                        break;
                    }
                case "TopDrive":
                    {
                        tlength = 106.00;
                        break;
                    }
                default:
                    break;
            }
            return tlength;
        }
        public Table getSurfacePageInfo(Dictionary<string, string> objSegment, string tabHeader)
        {
            string tabheadertext = tabHeader;

            Table _tableSurf = new Table(4, true);
            _tableSurf.SetFontSize(8);
            Cell sn = new Cell(1, 4).SetTextAlignment(TextAlignment.LEFT).Add(new Paragraph(tabheadertext).SetBackgroundColor(ColorConstants.LIGHT_GRAY));
            _tableSurf.AddHeaderCell(sn);

            foreach (var item in objSegment)
            {
                string itmvalue = string.IsNullOrEmpty(item.Value) ? "" : item.Value.ToString();
                _tableSurf.AddCell(new Paragraph(item.Key).SetTextAlignment(TextAlignment.LEFT));
                _tableSurf.AddCell(new Paragraph(itmvalue).SetTextAlignment(TextAlignment.LEFT));
            }
            return _tableSurf.SetAutoLayout();
        }

        public Table getFluidEnvelopeInfo(Dictionary<string, string> objfluidvalues, string fluidenvheader)
        {
            int count = 3;
            string _tblheadText = fluidenvheader;

            Table _tabFluidEnvelope = new Table(2, false);
            _tabFluidEnvelope.SetFontSize(8);

            Cell da = new Cell(1, 2).SetTextAlignment(TextAlignment.LEFT).Add(new Paragraph(_tblheadText).SetBackgroundColor(ColorConstants.LIGHT_GRAY).SetWidth(200));
            _tabFluidEnvelope.AddHeaderCell(da);

            for (int i = 1; i <= count; i++)
            {
                switch (i)
                {
                    case 1:
                        {
                            Cell maxflowpressure = new Cell(1, 1).SetTextAlignment(TextAlignment.LEFT).Add(new Paragraph("Maximum Allowable Pressure").SetWidth(100));
                            Cell maxflowpr = new Cell(1, 1).SetTextAlignment(TextAlignment.LEFT).Add(new Paragraph(objfluidvalues["MaximumAllowablePressure"]).SetWidth(100));
                            _tabFluidEnvelope.AddCell(maxflowpressure);
                            _tabFluidEnvelope.AddCell(maxflowpr);
                            break;
                        }
                    case 2:
                        {
                            Cell maxflowrate = new Cell(1, 1).SetTextAlignment(TextAlignment.LEFT).Add(new Paragraph("Maximum Allowable Flowrate").SetWidth(100));
                            Cell maxflrate = new Cell(1, 1).SetTextAlignment(TextAlignment.LEFT).Add(new Paragraph(objfluidvalues["MaximumAllowableFlowrate"]).SetWidth(100));
                            _tabFluidEnvelope.AddCell(maxflowrate);
                            _tabFluidEnvelope.AddCell(maxflrate);
                            break;
                        }
                    case 3:
                        {
                            Cell fecomment = new Cell(1, 1).SetTextAlignment(TextAlignment.LEFT).Add(new Paragraph("Comments").SetWidth(100));
                            Cell fecomm = new Cell(1, 1).SetTextAlignment(TextAlignment.LEFT).Add(new Paragraph(objfluidvalues["Comments"]).SetWidth(100));
                            _tabFluidEnvelope.AddCell(fecomment);
                            _tabFluidEnvelope.AddCell(fecomm);
                            break;
                        }
                    default:
                        break;
                }
            }

            return _tabFluidEnvelope.SetAutoLayout();
        }

        #endregion

        #endregion
    }

}

