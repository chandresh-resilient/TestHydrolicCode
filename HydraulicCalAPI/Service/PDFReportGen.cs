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
using HydraulicCalAPI.ViewModel;
using HydraulicCalAPI.Service;
using Microsoft.AspNetCore.Mvc;
using iText.Kernel.Font;

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

            PdfCanvas canvas = new PdfCanvas(page.NewContentStreamBefore(), page.GetResources(), pdfDoc);
            Rectangle pageSize = page.GetPageSize();

            float x = pageSize.GetWidth() / 2 + 170;
            float yStart = 180;
            float yEnd = yStart - 15;
            canvas.SaveState()
                .SetLineWidth(1)
                .MoveTo(x, yStart)
                .LineTo(x, yEnd)
                .Stroke()
                .RestoreState();

            canvas.BeginText()
                .SetFontAndSize(iText.Kernel.Font.PdfFontFactory.CreateFont(iText.IO.Font.Constants.StandardFonts.HELVETICA), 12)
                .MoveText(pageSize.GetWidth() / 2 + 170, 170)
                .ShowText(pageNumber.ToString() + " out of " + numofpages)
                .EndText();
            canvas.Release();
        }
    }
    public class PDFReportGen
    {
        PgPreface objAuthor = new PgPreface();
        PgTableOfContent objTblOfContent = new PgTableOfContent();
        PgCasingLinerTubing objCasingLinerTubing = new PgCasingLinerTubing();
        PgFluidSurface objFluidSurface = new PgFluidSurface();

        Color accuColor, rptgreen, lgtGrey, bhatblgreen;
        DateTime currentDate = DateTime.UtcNow.Date;
       
        Dictionary<string, string> pdfPieChart = new Dictionary<string, string>();
        HydraulicCalculationService objHydCalSrvs = new HydraulicCalculationService();
        string strCasingLinerTubing = "New Run";
        string baseDir = AppDomain.CurrentDomain.BaseDirectory;
        int increment = 0;

        public byte[] generatePDF(HydraulicCalAPI.Service.PdfReportService objInputData, ChartAndGraphService objChartService, HydraulicCalculationService inputHydra)
        {
            objHydCalSrvs = inputHydra;
            string _tabheader = string.Empty;
            accuColor = new DeviceRgb(165, 42, 42);
            rptgreen = new DeviceRgb(0, 128, 0);
            lgtGrey = new DeviceRgb(217, 217, 217);

            // Construct the path to the image relative to the base directoryf
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

            if (string.IsNullOrEmpty(objInputData.RunName.ToString()))
            {
                strCasingLinerTubing = objInputData.RunName.ToString();
            }
            else
            {
                strCasingLinerTubing = "New Run";
            }

            #region First Page
            Paragraph header = getHeader(objInputData.ReportHeader.ToString());

            Table _pdfHead = new Table(2, false);
            Cell _pdfHeadcell = new Cell();
            _pdfHeadcell.Add(img);
            _pdfHead.AddCell(_pdfHeadcell)
                .SetMarginBottom(2);

            Table tableAuthor =  objAuthor.GetAuthorContent(objInputData);
            Paragraph comment = new Paragraph("Comment:").SetTextAlignment(TextAlignment.LEFT).SetPadding(5).SetFontSize(11).SetBold().SetHeight(100).SetBorder(new SolidBorder(1));
            #endregion

            #region Table Of Content
            Table tblToc = new Table(3, false).SetBorder(Border.NO_BORDER);
            tblToc = objTblOfContent.GetTableOfcontent(strCasingLinerTubing);
            #endregion

            #region HeaderInformation
            Dictionary<string, string> pdfHederInfoData = new Dictionary<string, string>();
            Paragraph _headerinfo = new Paragraph("Header Information")
             .SetTextAlignment(TextAlignment.CENTER)
             .SetFontSize(20).SetBold();

            _tabheader = "Segment and Product / Service";
            pdfHederInfoData.Add("Segment", objInputData.ProductLine != null ? objInputData.ProductLine.ToString() : "");
            pdfHederInfoData.Add("Product / Service", objInputData.SubProductLine != null ? objInputData.SubProductLine.ToString() : "");
            Table tblSegment = getHeaderInfoTable(objInputData, pdfHederInfoData, _tabheader);

            pdfHederInfoData.Clear();

            _tabheader = "Job Information";
            pdfHederInfoData.Add("Job Start Date", (objInputData.JobStartDate == null || objInputData.JobStartDate == "NA") ? "" : GetFormattedDate(objInputData.JobStartDate.ToString()));
            pdfHederInfoData.Add("Well Location", objInputData.WellLocation != null ? objInputData.WellLocation.ToString() : "");
            pdfHederInfoData.Add("Customer", objInputData.Customer != null ? objInputData.Customer.ToString() : "");
            pdfHederInfoData.Add("Well Depth (MD)", objInputData.WellDepth > 0.00 ? objInputData.WellDepth.ToString() : "");
            pdfHederInfoData.Add("Job End Date", (objInputData.JobEndDate == null || objInputData.JobEndDate == "NA") ? "" : GetFormattedDate(objInputData.JobEndDate.ToString()));
            pdfHederInfoData.Add("JDE Delivery Ticket No", objInputData.JDEDeliveryTicketNo != null ? objInputData.JDEDeliveryTicketNo.ToString() : "");
            Table tblJobInformation = getHeaderInfoTable(objInputData, pdfHederInfoData, _tabheader);

            pdfHederInfoData.Clear();

            _tabheader = "Well Information";
            pdfHederInfoData.Add("Well Name and Number", objInputData.WellNameNumber != null ? objInputData.WellNameNumber.ToString() : "");
            pdfHederInfoData.Add("Well Location", objInputData.WFRDLocation != null ? objInputData.WFRDLocation.ToString() : "");
            pdfHederInfoData.Add("Field", objInputData.Field != null ? objInputData.Field.ToString() : "");
            pdfHederInfoData.Add("Lease", objInputData.Lease != null ? objInputData.Lease.ToString() : "");
            pdfHederInfoData.Add("Rig", objInputData.Rig != null ? objInputData.Rig.ToString() : "");
            pdfHederInfoData.Add("legal.API/OCS-G", objInputData.legalAPIOCSG != null ? objInputData.legalAPIOCSG.ToString() : "");
            pdfHederInfoData.Add("Latitude", objInputData.Latitude != null ? objInputData.Latitude.ToString() : "");
            pdfHederInfoData.Add("Longitude", objInputData.Longitude != null ? objInputData.Longitude.ToString() : "");
            pdfHederInfoData.Add("Well Country", objInputData.WellCountry != null ? objInputData.WellCountry.ToString() : "");
            pdfHederInfoData.Add("County/Parish", objInputData.CountyParish != null ? objInputData.CountyParish.ToString() : "");
            Table tblWellInformation = getHeaderInfoTable(objInputData, pdfHederInfoData, _tabheader);

            pdfHederInfoData.Clear();

            _tabheader = "Originator / Servicing Location Organization Data";
            pdfHederInfoData.Add("WFRD Location", objInputData.WFRDLocation != null ? objInputData.WFRDLocation.ToString() : "");
            pdfHederInfoData.Add("Hemisphere", objInputData.Hemisphere != null ? objInputData.Hemisphere.ToString() : "");
            pdfHederInfoData.Add("Geozone", objInputData.Geozone != null ? objInputData.Geozone.ToString() : "");
            pdfHederInfoData.Add("Region", objInputData.Region != null ? objInputData.Region.ToString() : "");
            pdfHederInfoData.Add("SubRegion", objInputData.SubRegion != null ? objInputData.SubRegion.ToString() : "");
            Table tblOriginator = getHeaderInfoTable(objInputData, pdfHederInfoData, _tabheader);

            pdfHederInfoData.Clear();

            _tabheader = "Customer Contacts";
            pdfHederInfoData.Add("Customer Contact (Office)", objInputData.CustomerContactOffice != null ? objInputData.CustomerContactOffice.ToString() : "");
            pdfHederInfoData.Add("Customer Phone No. (Office)", objInputData.CustomerPhoneNoOffice != null ? objInputData.CustomerPhoneNoOffice.ToString() : "");
            pdfHederInfoData.Add("Customer Contact (Field)", objInputData.CustomerContactField != null ? objInputData.CustomerContactField.ToString() : "");
            pdfHederInfoData.Add("Customer Phone No. (Field)", objInputData.CustomerPhoneNoField != null ? objInputData.CustomerPhoneNoField.ToString() : "");
            pdfHederInfoData.Add("Drilling Engineer", objInputData.DrillingEngineer != null ? objInputData.DrillingEngineer.ToString() : "");
            pdfHederInfoData.Add("Drilling Contractor", objInputData.DrillingContractor != null ? objInputData.DrillingContractor.ToString() : "");
            Table tblCustomerContacts = getHeaderInfoTable(objInputData, pdfHederInfoData, _tabheader);

            pdfHederInfoData.Clear();

            _tabheader = "Weatherford Contacts";
            pdfHederInfoData.Add("WFRD Salesman", objInputData.WFRDSalesman != null ? objInputData.WFRDSalesman.ToString() : "");
            pdfHederInfoData.Add("WFRD Field Engineer", objInputData.WFRDFieldEngineer != null ? objInputData.WFRDFieldEngineer.ToString() : "");
            Table tblWeatherfordContacts = getHeaderInfoTable(objInputData, pdfHederInfoData, _tabheader);

            pdfHederInfoData.Clear();

            Table tblGenInfo;
            getGeneralInfo(objInputData, out _tabheader, pdfHederInfoData, out tblGenInfo);

            pdfHederInfoData.Clear();

            _tabheader = "Approval and General Input Data";
            pdfHederInfoData.Add("Status", objInputData.Status != null ? objInputData.Status.ToString() : "");
            pdfHederInfoData.Add("Input By", objInputData.InputBy != null ? objInputData.InputBy.ToString() : "");
            pdfHederInfoData.Add("Prepared By", objInputData.StatusPreparedBy != null ? objInputData.StatusPreparedBy.ToString() : "");
            pdfHederInfoData.Add("Accuview Input Date", (objInputData.AccuviewInputDate == null || objInputData.AccuviewInputDate == "NA") ? "" : GetFormattedDate(objInputData.AccuviewInputDate.ToString()));
            pdfHederInfoData.Add("Submitted Date", objInputData.SubmittedDate != null ? GetFormattedDate(objInputData.SubmittedDate.ToString()) : "");
            pdfHederInfoData.Add("Approved By", (objInputData.ApprovedBy == null || objInputData.ApprovedBy == "NA") ? "" : objInputData.ApprovedBy.ToString());
            pdfHederInfoData.Add("Approved Date", (objInputData.ApprovedDate == null || objInputData.ApprovedDate == "NA") ? "" : GetFormattedDate(objInputData.ApprovedDate.ToString()));
            Table tblApproval = getHeaderInfoTable(objInputData, pdfHederInfoData, _tabheader);

            pdfHederInfoData.Clear();

            #endregion

            #region Casing Liner Tubing
            Paragraph _casingLinerTubingInfo = new Paragraph(strCasingLinerTubing)
            .SetTextAlignment(TextAlignment.CENTER)
            .SetFontSize(20).SetBold();

            _tabheader = "Depth Analysis";
            Table tblDepthAnalysis = objCasingLinerTubing.GetDepthAnalysis(objInputData, objChartService, _tabheader);

            _tabheader = "Casing/ Liner/ Tubing Data";
            Table tblCasingLinerTube = objCasingLinerTubing.GetCasingLinerTubing(objInputData, objHydCalSrvs, _tabheader);
            
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
                            dicLstBhaData.Add("MeasuredOD" + increment, bhawrkStrlstitem.OutsideDiameterInInch > 0 ? bhawrkStrlstitem.OutsideDiameterInInch.ToString() : "");
                            dicLstBhaData.Add("InnerDiameter" + increment, bhawrkStrlstitem.InsideDiameterInInch > 0 ? bhawrkStrlstitem.InsideDiameterInInch.ToString() : "");
                            // code to get Workstring Weight and Upper Connection type
                            var wrkStrWeight = objInputData.WorkStringItems.Where(wks => increment.Equals(bhawrkStrlstitem.PositionNumber))
                                                  .Select(wks => wks.wrkWeight);
                            var wrkStrUpConnTyp = objInputData.WorkStringItems.Where(wks => increment.Equals(bhawrkStrlstitem.PositionNumber))
                                                    .Select(wks => wks.wrkUpperConnType);
                            dicLstBhaData.Add("Weight" + increment, wrkStrWeight != null ? wrkStrWeight.FirstOrDefault() : "");
                            dicLstBhaData.Add("Length" + increment, bhawrkStrlstitem.LengthInFeet > 0 ? bhawrkStrlstitem.LengthInFeet.ToString() : "");
                            dicLstBhaData.Add("UpperConnType" + increment, wrkStrUpConnTyp != null ? wrkStrUpConnTyp.FirstOrDefault() : "");
                            dicLstBhaData.Add("LowerConnType" + increment, "");
                            dicLstBhaData.Add("FishNeckOD" + increment, "");
                            dicLstBhaData.Add("LenFshNck" + increment, "");
                            dicLstBhaData.Add("HydraulicOD" + increment, "");
                            dicLstBhaData.Add("HydraulicID" + increment, "");
                            break;
                        }
                    default:
                        {
                            dicLstBhaData.Add("BhaLstID" + increment, increment.ToString());
                            dicLstBhaData.Add("ToolDescription" + increment, bhawrkStrlstitem.toolDescription != null ? bhawrkStrlstitem.toolDescription.ToString() : "");
                            dicLstBhaData.Add("SerialNumber" + increment, bhawrkStrlstitem.PositionNumber > 0 ? bhawrkStrlstitem.PositionNumber.ToString() : "");
                            dicLstBhaData.Add("MeasuredOD" + increment, bhawrkStrlstitem.OutsideDiameterInInch > 0 ? bhawrkStrlstitem.OutsideDiameterInInch.ToString() : "");
                            dicLstBhaData.Add("InnerDiameter" + increment, bhawrkStrlstitem.InsideDiameterInInch > 0 ? bhawrkStrlstitem.InsideDiameterInInch.ToString() : "");
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
                            string _bhatoolweight = bhatoolWeight.FirstOrDefault();
                            if (_bhatoolweight == null || _bhatoolweight.ToUpper() == "NULL" || _bhatoolweight.ToUpper() == "NA")
                            {
                                dicLstBhaData.Add("Weight" + increment, "");
                            }
                            else
                            {
                                dicLstBhaData.Add("Weight" + increment, bhatoolWeight.FirstOrDefault().Substring(0, bhatoolWeight.FirstOrDefault().Length - 5)); 
                            }
                            dicLstBhaData.Add("Length" + increment, bhawrkStrlstitem.LengthInFeet > 0 ? bhawrkStrlstitem.LengthInFeet.ToString() : "");

                            string _bhatoolUpConnTyp = bhatoolUpConnTyp.FirstOrDefault();
                            if (_bhatoolUpConnTyp == null || _bhatoolUpConnTyp.ToUpper() == "NULL" || _bhatoolUpConnTyp.ToUpper() == "NA")
                            {
                                dicLstBhaData.Add("UpperConnType" + increment, ""); 
                            }
                            else
                            {
                                dicLstBhaData.Add("UpperConnType" + increment, bhatoolUpConnTyp.FirstOrDefault());
                            }
                            string _bhatoolLowConntyp = bhatoolLowConntyp.FirstOrDefault();
                            if (_bhatoolLowConntyp == null || _bhatoolLowConntyp.ToUpper() == "NULL" || _bhatoolLowConntyp.ToUpper() == "NA")
                            {
                                dicLstBhaData.Add("LowerConnType" + increment, "");
                            }
                            else
                            {
                                dicLstBhaData.Add("LowerConnType" + increment, bhatoolLowConntyp.FirstOrDefault());
                            }
                            string _bhatoolFishNeckOD = bhatoolFishNeckOD.FirstOrDefault();
                            if (_bhatoolFishNeckOD == null || _bhatoolFishNeckOD.ToUpper() == "NULL" || _bhatoolFishNeckOD.ToUpper() == "NA")
                            {
                                dicLstBhaData.Add("FishNeckOD" + increment, "");
                            }
                            else
                            {
                                dicLstBhaData.Add("FishNeckOD" + increment, bhatoolFishNeckOD.FirstOrDefault());
                            }
                            string _bhatoolFishNeckLen = bhatoolFishNeckLen.FirstOrDefault();
                            if (_bhatoolFishNeckLen == null || _bhatoolFishNeckLen.ToUpper() == "NULL" || _bhatoolFishNeckLen.ToUpper() == "NA")
                            {
                                dicLstBhaData.Add("LenFshNck" + increment, "");
                            }
                            else
                            {
                                dicLstBhaData.Add("LenFshNck" + increment, bhatoolFishNeckLen.FirstOrDefault());
                            }
                            dicLstBhaData.Add("HydraulicOD" + increment, bhawrkStrlstitem.OutsideDiameterInInch > 0 ? bhawrkStrlstitem.OutsideDiameterInInch.ToString() : "");
                            dicLstBhaData.Add("HydraulicID" + increment, bhawrkStrlstitem.InsideDiameterInInch > 0 ? bhawrkStrlstitem.InsideDiameterInInch.ToString() : "");
                            break;
                        }
                }
            }
            Table tblBhaData = getBha(objInputData, dicLstBhaData, _tabheader);
            increment = 0;

            #endregion

            #region Surface Equipment / Fluid Envelop / Fluid
            _tabheader = "Surface Equipment";
            Table tblSurfaceEquipment = objFluidSurface.GetSurfaceEquipDataTable(objInputData, _tabheader);
           
            _tabheader = "Fluid Envelope";
            Table tblFluidEnvelope = objFluidSurface.GetFluidEnvelopeInfo(objInputData, _tabheader);
            
            _tabheader = "Fluid";
            Table tblFluid = objFluidSurface.GetFluidInfo(objInputData, _tabheader);

            pdfHederInfoData.Clear();

            #endregion

            #region Chart and Graph Section
            #region Pressure Distribution Chart

            Table PieChartTable = getPieHeaderTable(objInputData, objChartService);

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
            byte[] graphBytes = DrawLineGraph(objChartService, dataPoints, objInputData);
            Image imgStdPvsFlwRate = new Image(ImageDataFactory.Create(graphBytes));
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

            Table tblHeaderAnnulusOutput = getAnnulusOutputTblHead(objInputData);
            for (int i = 0; i < objHyAnlyAnnuOutputData.Count; i++)
            {
                dicLstAnnulusOutputData.Add(getAnnulusTableData(objHyAnlyAnnuOutputData[i], objInputData));
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
                lstTblBHAheader.Add(getBHAToolLine(objBhaToolOutput[i], objInputData));
                lstTblBhaSide.Add(getBhaToolSide(objBhaToolOutput[i], objInputData));
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
                string dictKey = "HydraproLineSeries" + i.ToString();
                foreach (WFT.UI.Common.Charts.XYValueModelForLineData<double> hyproitem in dicBhaChart[dictKey])
                {
                    hyprodatapoints.Add(new DataPoints
                    {
                        X = (float)hyproitem.PrimaryAxisValue,
                        Y = (float)hyproitem.SecondaryAxisValue,
                        LineClr = "Blue"
                    });
                }
                hydraprograph = DrawHydraulicToolsGraph(hyprodatapoints, objChartService.HydraulicOutputBHAList[i],objInputData);
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
                            _stdpipeHeader, imgStdPvsFlwRate,
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
            pdfHederInfoData.Add("PL Job Number", objInputData.PlJobNumber != null ? objInputData.PlJobNumber.ToString() : "");
            pdfHederInfoData.Add("Project Name", objInputData.ProjectName != null ? objInputData.ProjectName.ToString() : "");
            pdfHederInfoData.Add("Customer Order Number", objInputData.CustomerOrderNumber != null ? objInputData.CustomerOrderNumber.ToString() : "");
            pdfHederInfoData.Add("Quote Number", objInputData.QuoteNumber != null ? objInputData.QuoteNumber.ToString() : "");
            pdfHederInfoData.Add("Rig Elevation", objInputData.RigElevation != null ? objInputData.RigElevation.ToString() : "");
            pdfHederInfoData.Add("Reservoir Type", objInputData.ReservoirType != null ? objInputData.ReservoirType.ToString() : "");
            pdfHederInfoData.Add("Water Depth", objInputData.WaterDepth != null ? objInputData.WaterDepth.ToString() : "");
            pdfHederInfoData.Add("Well Depth (TVD)", objInputData.WellDepthTVD > 0.00 ? objInputData.WellDepthTVD.ToString() : "");
            pdfHederInfoData.Add("Rig Type", objInputData.RigType != null ? objInputData.RigType.ToString() : "");
            pdfHederInfoData.Add("Well Classification", objInputData.WellClassification != null ? objInputData.WellClassification.ToString() : "");
            pdfHederInfoData.Add("Work String", objInputData.WorkString != null ? objInputData.WorkString.ToString() : "");
            pdfHederInfoData.Add("Inclination", objInputData.Inclination != null ? objInputData.Inclination.ToString() : "");
            pdfHederInfoData.Add("Customer Type", objInputData.CustomerType != null ? objInputData.CustomerType.ToString() : "");
            pdfHederInfoData.Add("H2S Present", objInputData.H2SPresent != null ? objInputData.H2SPresent.ToString() : "");
            pdfHederInfoData.Add("CO2 Present", objInputData.CO2Present != null ? objInputData.CO2Present.ToString() : "0.00");
            pdfHederInfoData.Add("Total mileage to/from location", objInputData.TotalMileageTFLocation != null ? objInputData.TotalMileageTFLocation.ToString() : "");
            pdfHederInfoData.Add("Total travel time to/from location", objInputData.TotalTravelTimeTFLlocation != null ? objInputData.TotalTravelTimeTFLlocation.ToString() : "");
            pdfHederInfoData.Add("Total off-duty hours at location", objInputData.TotalOffDutyHrsAtLocation != null ? objInputData.TotalOffDutyHrsAtLocation.ToString() : "");
            tblGenInfo = getHeaderInfoTable(objInputData, pdfHederInfoData, _tabheader);
        }
        private static void NewMethod(Image img, LineSeparator ls, Paragraph newline,
            Paragraph header, Table tableAuthor, Paragraph comment, Table tblToc, Paragraph _headerinfo,
            Table tblSegment, Table tblJobInformation, Table tblWellInformation, Table tblOriginator, Table tblCustomerContacts, Table tblWeatherfordContacts, Table tblGenInfo, Table tblApproval,
            Paragraph _casingLinerTubingInfo, Table tblDepthAnalysis, Table tblCasingLinerTube,
            Table tblBhaData, Table tblSurfaceEquipment, Table tblFluidEnvelope, Table tblFluid,
            Paragraph _stdpipeHeader, Image imgStdPvsFlwRate,
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
            document.Add(imgStdPvsFlwRate);
            document.Add(new AreaBreak(AreaBreakType.NEXT_PAGE));
            #endregion

            #region Content Pressure Drop
            document.Add(head2);

            document.Add(PieChartTable);
            PieChartTable.Flush();
            PieChartTable.Complete();

            document.Add(_chartheader);
            document.Add(imgPie);
            document.Add(tblHeaderAnnulusOutput);
            for (int i = 0; i < dicLstAnnulusOutputData.Count; i++)
            {
                document.Add(dicLstAnnulusOutputData[i]);
            }
            document.Add(newline);
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
                celltd1 = new Cell(1, 2).Add(lstTblBHAheader[gp]);
                celltd2 = new Cell(1, 1).Add(lstTblBhaSide[gp]).SetPadding(10);
                celltd3 = new Cell(1, 1).Add(bhaToolGraphsLst[gp]);
                bhatooldetail.AddCell(celltd1);
                bhatooldetail.AddCell(celltd2);
                bhatooldetail.AddCell(celltd3);
                document.Add(bhatooldetail);
                document.Add(newline);
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
        public string GetFormattedDate(string objDate)
        {
            DateTime dateObject;
            string formattedDate = "";
            if (objDate != null)
            {
                dateObject = DateTime.ParseExact(objDate, "yyyy/MM/dd", null);
                formattedDate = dateObject.ToString("MM/dd/yyyy");
            }
            return formattedDate;
        }
      
        private static void AddFooter(PdfDocument pdfDocument, Document document, PdfReportService objFooterData)
        {
            Paragraph footer = new Paragraph();
            List<PdfReportService> pdfFooter = new List<PdfReportService>();
            pdfFooter.Add(new PdfReportService
            {
                JobID = (objFooterData.JobID != null ? objFooterData.JobID.ToString() : ""),
                WPTSReportID = (objFooterData.WPTSReportID != null ? objFooterData.WPTSReportID.ToString() : ""),
                AccuViewDocNo = (objFooterData.AccuViewDocNo != null ? objFooterData.AccuViewDocNo.ToString() : ""),
                AccuViewVersion = (objFooterData.AccuViewVersion != null ? objFooterData.AccuViewVersion.ToString() : "")
            });

            Color footlineColor = new DeviceRgb(165, 42, 42);
            SolidLine line = new SolidLine(3f);
            line.SetColor(footlineColor);
            LineSeparator footerSeperator = new LineSeparator(line);

            // Creating Footer
            Table footerTable = new Table(4, false).SetFontSize(7);
            footerTable.AddCell("AccuView Job ID");
            footerTable.AddCell("WPTS Report ID");
            footerTable.AddCell("AccuView Document Number");
            footerTable.AddCell("AccuView Version Number");

            foreach (PdfReportService item in pdfFooter)
            {
                footerTable.AddCell(item.JobID.ToString()).SetTextAlignment(TextAlignment.LEFT);
                footerTable.AddCell(item.WPTSReportID.ToString()).SetTextAlignment(TextAlignment.LEFT);
                footerTable.AddCell(item.AccuViewDocNo.ToString()).SetTextAlignment(TextAlignment.LEFT);
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
                document.ShowTextAligned(footer, leftMarginPosition, UnitConverter.mm2uu(55), pageId,
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
        public Table getPieHeaderTable(PdfReportService objUOM, ChartAndGraphService objPieTableData)
        {
            string charPsi = "psi";
            string charFlowRt = "gal/min";
            string charFt = "ft";
            if (objUOM.UOM.PressureName.ToUpper() != "PSI")
            {
                charPsi = objUOM.UOM.PressureName.ToString();
            }
            else if (objUOM.UOM.FlowRateName.ToUpper() != "GAL/MIN")
            {
                charFlowRt = objUOM.UOM.FlowRateName.ToString();
            }
            else if (objUOM.UOM.DepthName.ToUpper() != "FT")
            {
                charFt = objUOM.UOM.DepthName.ToString();
            }
            else { }

            double toolflowrate = 0.00;
            double tooldepth = 0.00;
            double toolPressureDrop = 0.00;
            Table pHeader = new Table(3, true)
                .SetTextAlignment(TextAlignment.LEFT).SetBackgroundColor(lgtGrey);
            foreach (var item in objPieTableData.HydraulicOutputBHAList)
            {
                if (item.InputFlowRate >= 0)
                    toolflowrate += item.InputFlowRate;
            }
            tooldepth = objPieTableData.ToolDepth;
            toolPressureDrop = objPieTableData.TotalPressureDrop;

            pHeader.AddCell(new Paragraph("Flow Rate : " + toolflowrate + " " + charFlowRt));
            pHeader.AddCell(new Paragraph("Tool Depth : " + Math.Round(tooldepth, 2) + " " + charFt));
            pHeader.AddCell(new Paragraph("Standpipe Pressure : " + Math.Round(toolPressureDrop, 2) + " " + charPsi));

            return pHeader.SetAutoLayout();
        }
        public Table getBHAToolLine(HydraulicBHAToolOutPutData objData, PdfReportService objUOM)
        {
            string charPsi = "psi";
            string charFlowRt = "gal/min";
            string charFt = "ft";
            if (objUOM.UOM.PressureName.ToUpper() != "PSI")
            {
                charPsi = objUOM.UOM.PressureName.ToString();
            }
            else if (objUOM.UOM.FlowRateName.ToUpper() != "GAL/MIN")
            {
                charFlowRt = objUOM.UOM.FlowRateName.ToString();
            }
            else if (objUOM.UOM.DepthName.ToUpper() != "FT")
            {
                charFt = objUOM.UOM.DepthName.ToString();
            }
            else { }

            string dnArrowPath = System.IO.Path.Combine(baseDir, "Images", "down-arrow.png");
            bhatblgreen = new DeviceRgb(180, 241, 198);
            // Add image
            Image imgDnArrow = new Image(ImageDataFactory
               .Create(dnArrowPath))
               .SetTextAlignment(TextAlignment.LEFT).SetWidth(10).SetHeight(12).SetMarginBottom(3);

            Table tblbhtoolhead = new Table(8, false)
                .SetBold().SetFontSize(7).SetWidth(UnitValue.CreatePercentValue(100));
            Cell _blankcell;

            Cell tblhead01 = new Cell(1, 1).Add(new Paragraph("")).SetBackgroundColor(lgtGrey);
            Cell tblhead02 = new Cell(1, 1).Add(new Paragraph("Work String")).SetBackgroundColor(lgtGrey).SetTextAlignment(TextAlignment.CENTER);
            Cell tblhead03 = new Cell(1, 1).Add(new Paragraph("Length (" + charFt + ")")).SetBackgroundColor(lgtGrey).SetTextAlignment(TextAlignment.CENTER);
            Cell tblhead04 = new Cell(1, 1).Add(new Paragraph("Input Flow Rate (" + charFlowRt + ")")).SetBackgroundColor(lgtGrey).SetTextAlignment(TextAlignment.CENTER);
            Cell tblhead05 = new Cell(1, 1).Add(new Paragraph("Average Velocity (" + charFt + "/sec)")).SetBackgroundColor(lgtGrey).SetTextAlignment(TextAlignment.CENTER);
            Cell tblhead06 = new Cell(1, 1).Add(new Paragraph("Critical Velocity (" + charFt + "/sec)")).SetBackgroundColor(lgtGrey).SetTextAlignment(TextAlignment.CENTER);
            Cell tblhead07 = new Cell(1, 1).Add(new Paragraph("Flow")).SetBackgroundColor(lgtGrey).SetTextAlignment(TextAlignment.CENTER);
            Cell tblhead08 = new Cell(1, 1).Add(new Paragraph("Pressure Drop (" + charPsi + ")")).SetBackgroundColor(lgtGrey).SetTextAlignment(TextAlignment.CENTER);

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
                _blankcell = new Cell(1, 1).Add(imgDnArrow).SetWidth(10).SetBackgroundColor(ColorConstants.RED);
            }
            else if (objData.AvgVelocityColor.ToString().ToUpper() == "YELLOW")
            {
                _blankcell = new Cell(1, 1).Add(imgDnArrow).SetWidth(10).SetBackgroundColor(ColorConstants.YELLOW);
            }
            else
            {
                _blankcell = new Cell(1, 1).Add(imgDnArrow).SetWidth(10).SetBackgroundColor(bhatblgreen);
            }

            double tlLen = objData.Length * objUOM.UOM.DepthMultiplier;
            double tlInputFlwRt = objData.InputFlowRate * objUOM.UOM.FlowRateMultiplier;
            double tlAvgVel = objData.AverageVelocity * objUOM.UOM.DepthMultiplier;
            double tlCritVel = objData.CriticalVelocity * objUOM.UOM.DepthMultiplier;
            double tlPrDrop = objData.PressureDrop * objUOM.UOM.PressureMultiplier;

            Cell celWks = new Cell(1, 1).Add(new Paragraph(objData.WorkString)).SetTextAlignment(TextAlignment.CENTER);
            Cell celLen = new Cell(1, 1).Add(new Paragraph(tlLen.ToString())).SetTextAlignment(TextAlignment.LEFT);
            Cell celInFlow = new Cell(1, 1).Add(new Paragraph(tlInputFlwRt.ToString())).SetTextAlignment(TextAlignment.LEFT);
            Cell celAvgV;
            if (objData.AvgVelocityColor.ToString().ToUpper() == "RED")
            {
                celAvgV = new Cell(1, 1).Add(new Paragraph(tlAvgVel.ToString())).SetTextAlignment(TextAlignment.LEFT).SetBackgroundColor(ColorConstants.RED);
            }
            else if (objData.AvgVelocityColor.ToString().ToUpper() == "YELLOW")
            {
                celAvgV = new Cell(1, 1).Add(new Paragraph(tlAvgVel.ToString())).SetTextAlignment(TextAlignment.LEFT).SetBackgroundColor(ColorConstants.YELLOW);
            }
            else
            {
                celAvgV = new Cell(1, 1).Add(new Paragraph(tlAvgVel.ToString()).SetFontColor(ColorConstants.WHITE)).SetTextAlignment(TextAlignment.LEFT).SetBackgroundColor(rptgreen);
            }
            Cell celCricVel = new Cell(1, 1).Add(new Paragraph(tlCritVel.ToString())).SetTextAlignment(TextAlignment.LEFT);
            Cell celFlowtyp = new Cell(1, 1).Add(new Paragraph(objData.FlowType.ToString())).SetTextAlignment(TextAlignment.CENTER);
            Cell celPressureDrp = new Cell(1, 1).Add(new Paragraph(tlPrDrop.ToString())).SetTextAlignment(TextAlignment.LEFT);

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
        public Table getBhaToolSide(HydraulicBHAToolOutPutData objData, PdfReportService objUOM)
        {
            string charFt = "ft";
            string charIn = "in";
            string charPsi = "psi";
            if (objUOM.UOM.PressureName.ToUpper() != "PSI")
            {
                charPsi = objUOM.UOM.PressureName.ToString();
            }
            else if (objUOM.UOM.SizeName.ToUpper() != "IN")
            {
                charIn = objUOM.UOM.SizeName.ToString();
            }
            else if (objUOM.UOM.DepthName.ToUpper() != "FT")
            {
                charFt = objUOM.UOM.DepthName.ToString();
            }
            else { }
            Table tblbhtoolSide = new Table(3, false)
                .SetTextAlignment(TextAlignment.LEFT).SetFontSize(7);

            Cell cell00 = new Cell(1, 1).Add(new Paragraph("Flow Type").SetBold()).SetWidth(80);
            Cell cell01 = new Cell(1, 2).Add(new Paragraph(objData.FlowType)).SetWidth(80);

            Cell cell10 = new Cell(1, 1).Add(new Paragraph("Average Velocity").SetBold()).SetWidth(80);
            double tlAvgVel = objData.AverageVelocity * objUOM.UOM.DepthMultiplier;
            Cell cell11 = new Cell(1, 1).Add(new Paragraph(tlAvgVel.ToString())).SetWidth(60);
            Cell cell12 = new Cell(1, 1).Add(new Paragraph(" " + charFt)).SetWidth(20);

            Cell cell20 = new Cell(1, 1).Add(new Paragraph("Critical Velocity").SetBold()).SetWidth(80);
            double tlCritVel = objData.CriticalVelocity * objUOM.UOM.DepthMultiplier;
            Cell cell21 = new Cell(1, 1).Add(new Paragraph(tlCritVel.ToString())).SetWidth(60);
            Cell cell22 = new Cell(1, 1).Add(new Paragraph(" " + charFt)).SetWidth(20);

            Cell cell30 = new Cell(1, 1).Add(new Paragraph("Pressure Drop").SetBold()).SetWidth(80);
            double tlPrDrop = objData.PressureDrop * objUOM.UOM.PressureMultiplier;
            Cell cell31 = new Cell(1, 1).Add(new Paragraph(tlPrDrop.ToString())).SetWidth(60);
            Cell cell32 = new Cell(1, 1).Add(new Paragraph(" " + charPsi)).SetWidth(20);

            Cell cell40 = new Cell(1, 1).Add(new Paragraph("Hydraulic OD").SetBold()).SetWidth(80);
            double tlHydraOD = objData.HydraulicOD * objUOM.UOM.SizeMultiplier;
            Cell cell41 = new Cell(1, 1).Add(new Paragraph(tlHydraOD.ToString())).SetWidth(60);
            Cell cell42 = new Cell(1, 1).Add(new Paragraph(" " + charIn)).SetWidth(20);

            Cell cell50 = new Cell(1, 1).Add(new Paragraph("Hydraulic ID").SetBold()).SetWidth(80);
            double tlHydraID = objData.HydraulicID * objUOM.UOM.SizeMultiplier;
            Cell cell51 = new Cell(1, 1).Add(new Paragraph(tlHydraID.ToString())).SetWidth(60);
            Cell cell52 = new Cell(1, 1).Add(new Paragraph(" " + charIn)).SetWidth(20);

            Cell cell60 = new Cell(1, 1).Add(new Paragraph("Length").SetBold()).SetWidth(80);
            double tlLen = objData.Length * objUOM.UOM.DepthMultiplier;
            Cell cell61 = new Cell(1, 1).Add(new Paragraph(tlLen.ToString())).SetWidth(60);
            Cell cell62 = new Cell(1, 1).Add(new Paragraph(" " + charFt)).SetWidth(20);

            tblbhtoolSide.AddCell(cell00);
            tblbhtoolSide.AddCell(cell01);
            tblbhtoolSide.AddCell(cell10);
            tblbhtoolSide.AddCell(cell11);
            tblbhtoolSide.AddCell(cell12);
            tblbhtoolSide.AddCell(cell20);
            tblbhtoolSide.AddCell(cell21);
            tblbhtoolSide.AddCell(cell22);
            tblbhtoolSide.AddCell(cell30);
            tblbhtoolSide.AddCell(cell31);
            tblbhtoolSide.AddCell(cell32);
            tblbhtoolSide.AddCell(cell40);
            tblbhtoolSide.AddCell(cell41);
            tblbhtoolSide.AddCell(cell42);
            tblbhtoolSide.AddCell(cell50);
            tblbhtoolSide.AddCell(cell51);
            tblbhtoolSide.AddCell(cell52);
            tblbhtoolSide.AddCell(cell60);
            tblbhtoolSide.AddCell(cell61);
            tblbhtoolSide.AddCell(cell62);

            return tblbhtoolSide.SetAutoLayout();
        }
        public byte[] GeneratePieChart(Dictionary<string, string> objPrsDrop, List<PieData> pieData)
        {
            float width = 200;
            float height = 200;
            float margin = 15;
            using (var surfcae = SKSurface.Create(new SKImageInfo(350, 250)))
            {
                var canvas = surfcae.Canvas;
                canvas.Clear(SKColors.White);
                float centerX = width / 2f;
                float centerY = height / 2f;
                float radius = Math.Min(width, height) * 0.8f;
                float labelPercentage = 0;
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
                    float legendX = 400 - margin - 200;
                    float legendY = margin;
                    float legendItemHeight = 8;
                    foreach (var data in pieData)
                    {
                        labelPercentage = (float)Math.Round((data.Value / totalValue) * 100);
                        using (var paint = new SKPaint())
                        {
                            string colourName = data.Color;
                            string hexString = ViewModel.ColorConverter.ColorNameToHexString(colourName);
                            paint.Color = SKColor.Parse(hexString);
                            canvas.DrawRect(legendX, legendY, legendItemHeight, legendItemHeight, paint);
                        }
                        using (var lblpaint = new SKPaint())
                        {
                            lblpaint.TextSize = 8;
                            lblpaint.Color = SKColors.Black;
                            lblpaint.TextAlign = SKTextAlign.Left;
                            canvas.DrawText($"{labelPercentage}%" + "=>" + $"{data.Label}", legendX + legendItemHeight + 5, legendY + 8, lblpaint);
                        }
                           
                        legendY += legendItemHeight + 5;
                    }

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
                            canvas.DrawArc(new SKRect(50, 50, 150, 150), startAngle, sweepAngle, true, paint);
                            startAngle += sweepAngle;
                        }
                    }
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
        public Table getAnnulusOutputTblHead(PdfReportService objUOM)
        {
            string charFt = "ft";
            string charPsi = "psi";
            if (objUOM.UOM.PressureName.ToUpper() != "PSI")
            {
                charPsi = objUOM.UOM.PressureName.ToString();
            }
            else if (objUOM.UOM.DepthName.ToUpper() != "PSI")
            {
                charFt = objUOM.UOM.DepthName.ToString();
            }
            else { }

            Table tblhead = new Table(12, false).SetFontSize(7).SetWidth(UnitValue.CreatePercentValue(100));
            Cell tblhead01 = new Cell(1, 3).Add(new Paragraph(" ")).SetWidth(33).SetBackgroundColor(lgtGrey);
            Cell tblhead02 = new Cell(1, 1).Add(new Paragraph("Annulus").SetTextAlignment(TextAlignment.CENTER)).SetBackgroundColor(lgtGrey).SetWidth(40);
            Cell tblhead03 = new Cell(1, 1).Add(new Paragraph("WorkString").SetTextAlignment(TextAlignment.CENTER)).SetBackgroundColor(lgtGrey).SetWidth(103);
            Cell tblhead04 = new Cell(1, 1).Add(new Paragraph("From (" + charFt + ")").SetTextAlignment(TextAlignment.CENTER)).SetBackgroundColor(lgtGrey).SetWidth(32);
            Cell tblhead05 = new Cell(1, 1).Add(new Paragraph("To (" + charFt + ")").SetTextAlignment(TextAlignment.CENTER)).SetBackgroundColor(lgtGrey).SetWidth(32);
            Cell tblhead06 = new Cell(1, 1).Add(new Paragraph("Average Velocity (" + charFt + "/min)").SetTextAlignment(TextAlignment.CENTER)).SetBackgroundColor(lgtGrey).SetWidth(50);
            Cell tblhead07 = new Cell(1, 1).Add(new Paragraph("Critical Velocity (" + charFt + "/min)").SetTextAlignment(TextAlignment.CENTER)).SetBackgroundColor(lgtGrey).SetWidth(50);
            Cell tblhead08 = new Cell(1, 1).Add(new Paragraph("Flow").SetTextAlignment(TextAlignment.CENTER)).SetBackgroundColor(lgtGrey).SetWidth(68);
            Cell tblhead09 = new Cell(1, 1).Add(new Paragraph("Chip Rate (" + charFt + "/min)").SetTextAlignment(TextAlignment.CENTER)).SetBackgroundColor(lgtGrey).SetWidth(50);
            Cell tblhead10 = new Cell(1, 1).Add(new Paragraph("Pressure Drop (" + charPsi + ")").SetTextAlignment(TextAlignment.CENTER)).SetBackgroundColor(lgtGrey).SetWidth(50);

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
        public Table getAnnulusTableData(HydraulicAnalysisAnnulusOutputData objHydrAnnulus, PdfReportService objUOM)
        {
            string upArrowPath = System.IO.Path.Combine(baseDir, "Images", "up-arrow.png");
            Image imgUpArrow = new Image(ImageDataFactory
               .Create(upArrowPath))
               .SetTextAlignment(TextAlignment.LEFT).SetWidth(10).SetHeight(12).SetMarginBottom(3);

            Table _tblannulusdata = new Table(12, false)
                .SetFontSize(7).SetWidth(UnitValue.CreatePercentValue(100));
            Cell _blankcell, _blankcell1, _blankcell2;

            double annulusFrm = objHydrAnnulus.From * objUOM.UOM.DepthMultiplier;
            double annulusTo = objHydrAnnulus.To * objUOM.UOM.DepthMultiplier;
            double annulusAvgVel = objHydrAnnulus.AverageVelocity * objUOM.UOM.DepthMultiplier;
            double annulusCritVel = objHydrAnnulus.CriticalVelocity * objUOM.UOM.DepthMultiplier;
            double annulusChipRt = objHydrAnnulus.ChipRate * objUOM.UOM.DepthMultiplier;
            double annulusPrDrop = objHydrAnnulus.PressureDrop * objUOM.UOM.PressureMultiplier;

            if (objHydrAnnulus.AnnulusColor.ToString().ToUpper() == "RED")
            {
                _blankcell = new Cell(1, 1).SetBackgroundColor(ColorConstants.RED).Add(imgUpArrow).SetWidth(10).SetBorder(Border.NO_BORDER);
            }
            else if (objHydrAnnulus.AnnulusColor.ToString().ToUpper() == "YELLOW")
            {
                _blankcell = new Cell(1, 1).SetBackgroundColor(ColorConstants.YELLOW).Add(imgUpArrow).SetWidth(10).SetBorder(Border.NO_BORDER);
            }
            else
            {
                _blankcell = new Cell(1, 1).SetBackgroundColor(ColorConstants.GREEN).Add(imgUpArrow).SetWidth(10).SetBorder(Border.NO_BORDER);
            }
            _blankcell1 = new Cell(1, 1).Add(new Paragraph(" ")).SetWidth(10).SetBackgroundColor(lgtGrey).SetBorder(Border.NO_BORDER);
            if (objHydrAnnulus.AnnulusColor.ToString().ToUpper() == "RED")
            {
                _blankcell2 = new Cell(1, 1).SetBackgroundColor(ColorConstants.RED).Add(imgUpArrow).SetWidth(10).SetBorder(Border.NO_BORDER);
            }
            else if (objHydrAnnulus.AnnulusColor.ToString().ToUpper() == "YELLOW")
            {
                _blankcell2 = new Cell(1, 1).SetBackgroundColor(ColorConstants.YELLOW).Add(imgUpArrow).SetWidth(10).SetBorder(Border.NO_BORDER);
            }
            else
            {
                _blankcell2 = new Cell(1, 1).SetBackgroundColor(ColorConstants.GREEN).Add(imgUpArrow).SetWidth(10).SetBorder(Border.NO_BORDER);
            }

            Cell cellAnnulus = new Cell(1, 1).Add(new Paragraph(objHydrAnnulus.Annulus.ToString()).SetTextAlignment(TextAlignment.CENTER).SetWidth(39));
            Cell celWks = new Cell(1, 1).Add(new Paragraph(objHydrAnnulus.WorkString.ToString()).SetTextAlignment(TextAlignment.CENTER).SetWidth(96));
            Cell celFrom = new Cell(1, 1).Add(new Paragraph(annulusFrm.ToString()).SetTextAlignment(TextAlignment.LEFT).SetWidth(30));
            Cell celTo = new Cell(1, 1).Add(new Paragraph(annulusTo.ToString()).SetTextAlignment(TextAlignment.LEFT).SetWidth(30));
            Cell celAvgV;
            if (objHydrAnnulus.AvgVelocityColor.ToString().ToUpper() == "RED")
            {
                celAvgV = new Cell(1, 1).Add(new Paragraph(annulusAvgVel.ToString()).SetTextAlignment(TextAlignment.LEFT).SetWidth(47)).SetBackgroundColor(ColorConstants.RED);
            }
            else if (objHydrAnnulus.AvgVelocityColor.ToString().ToUpper() == "YELLOW")
            {
                celAvgV = new Cell(1, 1).Add(new Paragraph(annulusAvgVel.ToString()).SetTextAlignment(TextAlignment.LEFT).SetWidth(47)).SetBackgroundColor(ColorConstants.YELLOW);
            }
            else
            {
                celAvgV = new Cell(1, 1).Add(new Paragraph(annulusAvgVel.ToString()).SetFontColor(ColorConstants.WHITE).SetTextAlignment(TextAlignment.LEFT).SetWidth(47)).SetBackgroundColor(rptgreen);
            }
            Cell celCricVel = new Cell(1, 1).Add(new Paragraph(annulusCritVel.ToString()).SetTextAlignment(TextAlignment.LEFT).SetWidth(47));
            Cell celFlowtyp = new Cell(1, 1).Add(new Paragraph(objHydrAnnulus.Flow.ToString()).SetTextAlignment(TextAlignment.CENTER).SetWidth(63));
            Cell celChipcolor;
            if (objHydrAnnulus.ChipRateColor.ToString().ToUpper() == "RED")
            {
                celChipcolor = new Cell(1, 1).Add(new Paragraph(annulusChipRt.ToString()).SetTextAlignment(TextAlignment.LEFT).SetWidth(48)).SetBackgroundColor(ColorConstants.RED);
            }
            else if (objHydrAnnulus.ChipRateColor.ToString().ToUpper() == "YELLOW")
            {
                celChipcolor = new Cell(1, 1).Add(new Paragraph(annulusChipRt.ToString()).SetTextAlignment(TextAlignment.LEFT).SetWidth(48)).SetBackgroundColor(ColorConstants.YELLOW);
            }
            else
            {
                celChipcolor = new Cell(1, 1).Add(new Paragraph(annulusChipRt.ToString()).SetFontColor(ColorConstants.WHITE).SetTextAlignment(TextAlignment.LEFT).SetWidth(48)).SetBackgroundColor(rptgreen);
            }

            Cell celPressureDrp = new Cell(1, 1).Add(new Paragraph(annulusPrDrop.ToString()).SetTextAlignment(TextAlignment.LEFT).SetWidth(48));

            _tblannulusdata.AddCell(_blankcell);
            _tblannulusdata.AddCell(_blankcell1);
            _tblannulusdata.AddCell(_blankcell2);
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
        public byte[] DrawLineGraph(ChartAndGraphService objCags, List<DataPoints> dataPoints, PdfReportService objUOM)
        {
            // Add labels and values to PDF document
            string gXValue = "gal/min";
            string gYValue = "psi";
            if (objUOM.UOM.FlowRateName.ToUpper() != "GAL/MIN")
            {
                gXValue = objUOM.UOM.FlowRateName.ToString();
            }
            else if (objUOM.UOM.PressureName.ToUpper() != "PSI")
            {
                gYValue = objUOM.UOM.PressureName.ToString();
            }
            else { }
            // Sample data (replace with your actual data)
            List<float> flowrate = new List<float>();
            List<float> pressure = new List<float>();

            for (int i = 0; i < dataPoints.Count; i++)
            {
                flowrate.Add(dataPoints[i].X);
                pressure.Add(dataPoints[i].Y);
            }

            using (var surfcae = SKSurface.Create(new SKImageInfo(500, 400)))
            {
                var canvas = surfcae.Canvas;
                canvas.Clear(SKColors.White);
                float width = 500;
                float height = 400;
                float margin = 40;

                float graphWidth = width - 2 * margin;
                float graphHeight = height - 2 * margin;

                // Define the scaling factors
                float maxX = flowrate[flowrate.Count - 1];
                float maxY = pressure[pressure.Count - 1];
                float scaleX = graphWidth / maxX;
                float scaleY = graphHeight / maxY;

                float opPointX = (float)objCags.MaxFlowRate;
                float opPointY = (float)objCags.TotalPressureDrop;

                float anx1 = margin + opPointX * scaleX;
                float any1 = height - margin - opPointY * scaleY;
               
                using (var paint = new SKPaint { Color = SKColors.Black, StrokeWidth = 1 })
                {
                    // Draw X and Y axis
                    canvas.DrawLine(margin, margin, margin, height - margin, paint);
                    canvas.DrawLine(margin, height - margin, width - margin, height - margin, paint);
                    canvas.DrawText("X", anx1 - 50 , any1 + 15, paint);

                    float xpoints = (float)Math.Round(maxX / 100);
                    float counterX = 1;
                    float counterY = 1;
                    float xpoint = margin;
                    do
                    {
                        xpoint += 50;
                        float valXAxis = counterX * 100;
                        int cordsY = (int)(height - margin);
                        canvas.DrawLine(xpoint, cordsY-5, xpoint, cordsY+5, paint);
                        canvas.DrawText(valXAxis.ToString(), xpoint-5, cordsY+15, paint);
                        counterX++;
                    } while (counterX <= xpoints);

                    float yMultiplier = 0;
                    if(maxY > 1000)
                    {
                        yMultiplier = 1000;
                    }
                    else if (maxY > 100 && maxY < 1000)
                    {
                        yMultiplier = 100;
                    }
                    else
                    {
                        yMultiplier = 10;
                    }

                    float ypoint = height - margin;
                    do
                    {
                        ypoint -= 50;
                        int cordsX = (int)margin;
                        canvas.DrawLine(cordsX - 2, ypoint, cordsX + 5, ypoint, paint);
                        canvas.DrawText((counterY * yMultiplier).ToString(), new SKPoint(margin-28, ypoint), paint);
                        counterY++;
                    } while (counterY < 10);
                }

                using (var paint = new SKPaint { Color = SKColors.Red, StrokeWidth = 1, IsAntialias = true })
                {
                    // Draw data points and lines
                    for (int i = 0; i < dataPoints.Count - 1; i++)
                    {
                        float x1 = margin + dataPoints[i].X * scaleX;
                        float y1 = height - margin - dataPoints[i].Y * scaleY;
                        float x2 = margin + dataPoints[i + 1].X * scaleX;
                        float y2 = height - margin - dataPoints[i + 1].Y * scaleY;

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

                // Add X-axis label
                using (var xLabelPaint = new SKPaint())
                {
                    xLabelPaint.Color = SKColors.Black;
                    xLabelPaint.TextAlign = SKTextAlign.Center;
                    xLabelPaint.TextSize = 14;
                    canvas.DrawText("Flow Rate (" + gXValue + ")", width / 2, margin / 2 + 370, xLabelPaint);
                }

                // Add Y-axis label
                using (var yLabelPaint = new SKPaint())
                {
                    yLabelPaint.Color = SKColors.Black;
                    yLabelPaint.TextAlign = SKTextAlign.Center;
                    yLabelPaint.TextSize = 14;
                    yLabelPaint.IsAntialias = true;
                    canvas.RotateDegrees(-90);
                    canvas.DrawText("Standpipe Pressure (" + gYValue + ")", -height / 2 - 5, (margin / 2)-10, yLabelPaint);
                    canvas.RotateDegrees(90);
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
        public byte[] DrawHydraulicToolsGraph(List<DataPoints> hyprobhadataPoints, ViewModel.HydraulicOutputBHAViewModel service, PdfReportService objUOM)
        {
            // Add labels and values to PDF document
            string gXValue = "gal/min";
            string gYValue = "psi";
            if (objUOM.UOM.FlowRateName.ToUpper() != "GAL/MIN")
            {
                gXValue = objUOM.UOM.FlowRateName.ToString();
            }
            else if (objUOM.UOM.PressureName.ToUpper() != "PSI")
            {
                gYValue = objUOM.UOM.PressureName.ToString();
            }
            else { }
            // Sample data (replace with your actual data)
            List<float> flowrate = new List<float>();
            List<float> pressure = new List<float>();

            for (int i = 0; i < hyprobhadataPoints.Count; i++)
            {
                flowrate.Add(hyprobhadataPoints[i].X);
                pressure.Add(hyprobhadataPoints[i].Y);
            }

            using (var surfcae = SKSurface.Create(new SKImageInfo(300, 280)))
            {
                var canvas = surfcae.Canvas;
                canvas.Clear(SKColors.White);
                float width = 280;
                float height = 270;
                float margin = 35;

                float graphWidth = width - 2 * margin;
                float graphHeight = height - 2 * margin;

                // Define the scaling factors
                float maxX = flowrate[flowrate.Count - 1];
                float maxY = pressure[pressure.Count - 1];
                float scaleX = graphWidth / maxX;
                float scaleY = graphHeight / maxY;

                float opPointX = (float)service.InputFlowRate;
                float opPointY = (float)service.BHAPressureDrop;

                float anx1 = margin + opPointX * scaleX;
                float any1 = height - margin - opPointY * scaleY;

                using (var paint = new SKPaint { Color = SKColors.Black, StrokeWidth = 1 })
                {
                    // Draw X and Y axis
                    canvas.DrawLine(margin, margin, margin, height - margin, paint);
                    canvas.DrawLine(margin, height - margin, width - margin, height - margin, paint);
                    canvas.DrawText("X", anx1, any1, paint);

                    float xpoints = (float)Math.Round(maxX / 100);
                    float counterX = 1;
                    float counterY = 1;
                    float xpoint = margin;
                    do
                    {
                        xpoint += 50;
                        float valXAxis = counterX * 100;
                        int cordsY = (int)(height - margin);
                        canvas.DrawLine(xpoint, cordsY - 5, xpoint, cordsY + 5, paint);
                        canvas.DrawText(valXAxis.ToString(), xpoint - 5, cordsY + 15, paint);
                        counterX++;
                    } while (counterX < xpoints);

                    float yMultiplier = 0;
                    if (maxY > 1000)
                    {
                        yMultiplier = 1000;
                    }
                    else if (maxY > 100 && maxY < 1000)
                    {
                        yMultiplier = 100;
                    }
                    else
                    {
                        yMultiplier = 20;
                    }

                    float ypoint = height - margin;
                    do
                    {
                        ypoint -= 50;
                        int cordsX = (int)margin;
                        canvas.DrawLine(cordsX - 2, ypoint, cordsX + 5, ypoint, paint);
                        canvas.DrawText((counterY * yMultiplier).ToString(), new SKPoint(margin - 18, ypoint), paint);
                        counterY++;
                    } while (counterY < 5);
                }

                using (var paint = new SKPaint { Color = SKColors.Blue, StrokeWidth = 1, IsAntialias = true })
                {
                    // Draw data points and lines
                    for (int i = 0; i < hyprobhadataPoints.Count - 1; i++)
                    {
                        float x1 = margin + hyprobhadataPoints[i].X * scaleX;
                        float y1 = height - margin - hyprobhadataPoints[i].Y * scaleY;
                        float x2 = margin + hyprobhadataPoints[i + 1].X * scaleX;
                        float y2 = height - margin - hyprobhadataPoints[i + 1].Y * scaleY;
                        canvas.DrawLine(x1, y1, x2, y2, paint);
                    }
                }

                // Add X-axis label
                using (var xLabelPaint = new SKPaint())
                {
                    xLabelPaint.Color = SKColors.Black;
                    xLabelPaint.TextAlign = SKTextAlign.Center;
                    xLabelPaint.TextSize = 10;
                    canvas.DrawText("Flow Rate (" + gXValue + ")", width / 2, margin / 2 + 250, xLabelPaint);
                }

                // Add Y-axis label
                using (var yLabelPaint = new SKPaint())
                {
                    yLabelPaint.Color = SKColors.Black;
                    yLabelPaint.TextAlign = SKTextAlign.Center;
                    yLabelPaint.TextSize = 10;
                    yLabelPaint.IsAntialias = true;
                    canvas.RotateDegrees(-90);
                    canvas.DrawText("Standpipe Pressure (" + gYValue + ")", -height / 2 - 5, (margin / 2) - 10, yLabelPaint);
                    canvas.RotateDegrees(90);
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
        
        public Table getHeaderInfoTable(PdfReportService objUOM, Dictionary<string, string> objSegment, string tabHeader)
        {
            string tabheadertext = tabHeader;

            Table _tableSeg = new Table(6, true);
            _tableSeg.SetFontSize(7);
            Cell sn = new Cell(1, 6).Add(new Paragraph(tabheadertext)).SetTextAlignment(TextAlignment.LEFT).SetBackgroundColor(lgtGrey).SetBold();
            _tableSeg.AddHeaderCell(sn);

            foreach (var item in objSegment)
            {
                Cell celhinfoValue, celhinfoUom;
                string charFt = "ft";
                if (objUOM.UOM.DepthName.ToUpper() != "FT")
                {
                    charFt = objUOM.UOM.DepthName.ToString();
                }

                string itemKey = string.IsNullOrEmpty(item.Key) ? "" : item.Key.ToString();
                string itmvalue = string.IsNullOrEmpty(item.Value) ? "" : item.Value.ToString();

                Cell celHinfoKey = new Cell(1, 1).Add(new Paragraph(itemKey)).SetBold().SetTextAlignment(TextAlignment.LEFT);
                _tableSeg.AddCell(celHinfoKey);
                if (itemKey.ToUpper() == "WELL DEPTH (MD)")
                {
                    if(itmvalue != "")
                    {
                        itmvalue = Math.Round((float.Parse(itmvalue) * objUOM.UOM.DepthMultiplier), 2).ToString();
                    }
                    celhinfoValue = new Cell(1, 1).Add(new Paragraph(itmvalue)).SetTextAlignment(TextAlignment.LEFT);
                    _tableSeg.AddCell(celhinfoValue);

                    celhinfoUom = new Cell(1, 1).Add(new Paragraph("  " + charFt));
                    _tableSeg.AddCell(celhinfoUom);
                }
                else if (itemKey.ToUpper() == "RIG ELEVATION")
                {
                    if (itmvalue != "" && itmvalue.ToUpper() != "NULL")
                    {
                        itmvalue = Math.Round((float.Parse(itmvalue) * objUOM.UOM.DepthMultiplier), 2).ToString();
                    }
                    celhinfoValue = new Cell(1, 1).Add(new Paragraph(itmvalue)).SetTextAlignment(TextAlignment.LEFT);
                    _tableSeg.AddCell(celhinfoValue);
                    celhinfoUom = new Cell(1, 1).Add(new Paragraph("  " + charFt));
                    _tableSeg.AddCell(celhinfoUom);
                }
                else if (itemKey.ToUpper() == "WATER DEPTH")
                {
                    if (itmvalue != "")
                    {
                        itmvalue = Math.Round((float.Parse(itmvalue) * objUOM.UOM.DepthMultiplier), 2).ToString();
                    }
                    
                    celhinfoValue = new Cell(1, 1).Add(new Paragraph(itmvalue)).SetTextAlignment(TextAlignment.LEFT);
                    _tableSeg.AddCell(celhinfoValue);
                    celhinfoUom = new Cell(1, 1).Add(new Paragraph("  " + charFt));
                    _tableSeg.AddCell(celhinfoUom);
                }
                else if (itemKey.ToUpper() == "WELL DEPTH (TVD)")
                {
                    if (itmvalue != "")
                    {
                        itmvalue = Math.Round((float.Parse(itmvalue) * objUOM.UOM.DepthMultiplier), 2).ToString();
                    }

                    celhinfoValue = new Cell(1, 1).Add(new Paragraph(itmvalue)).SetTextAlignment(TextAlignment.LEFT);
                    _tableSeg.AddCell(celhinfoValue);
                    celhinfoUom = new Cell(1, 1).Add(new Paragraph("  " + charFt));
                    _tableSeg.AddCell(celhinfoUom);
                }
                else if (itemKey.ToUpper() == "TOTAL MILEAGE TO/FROM LOCATION")
                {
                    celhinfoValue = new Cell(1, 1).Add(new Paragraph(itmvalue)).SetTextAlignment(TextAlignment.LEFT);
                    _tableSeg.AddCell(celhinfoValue);
                    celhinfoUom = new Cell(1, 1).Add(new Paragraph(" miles "));
                    _tableSeg.AddCell(celhinfoUom);
                }
                else if (itemKey.ToUpper() == "TOTAL TRAVEL TIME TO/FROM LOCATION")
                {
                    celhinfoValue = new Cell(1, 1).Add(new Paragraph(itmvalue)).SetTextAlignment(TextAlignment.LEFT);
                    _tableSeg.AddCell(celhinfoValue);
                    celhinfoUom = new Cell(1, 1).Add(new Paragraph(" hours "));
                    _tableSeg.AddCell(celhinfoUom);
                }
                else if (itemKey.ToUpper() == "TOTAL OFF-DUTY HOURS AT LOCATION")
                {
                    celhinfoValue = new Cell(1, 1).Add(new Paragraph(itmvalue)).SetTextAlignment(TextAlignment.LEFT);
                    _tableSeg.AddCell(celhinfoValue);
                    celhinfoUom = new Cell(1, 1).Add(new Paragraph(" hours "));
                    _tableSeg.AddCell(celhinfoUom);
                }
                else if (itemKey.ToUpper() == "STATUS")
                {
                    celhinfoValue = new Cell(1, 6).Add(new Paragraph(itmvalue)).SetTextAlignment(TextAlignment.LEFT);
                    _tableSeg.AddCell(celhinfoValue);
                }
                else if (itemKey.ToUpper() == "WFRD LOCATION")
                {
                    celhinfoValue = new Cell(1, 6).Add(new Paragraph(itmvalue)).SetTextAlignment(TextAlignment.LEFT);
                    _tableSeg.AddCell(celhinfoValue);
                }
                else
                {
                    celhinfoValue = new Cell(1, 2).Add(new Paragraph(itmvalue)).SetWidth(100).SetTextAlignment(TextAlignment.LEFT);
                    _tableSeg.AddCell(celhinfoValue);
                }
            }
            return _tableSeg.SetAutoLayout();
        }
        #endregion

        #region Bottom Hole Assembly
        public Table getBha(PdfReportService objUOM, Dictionary<string, string> objbhaitem, string bhaheadtext)
        {
            string charFt = "ft";
            string charIn = "in";
            string charLbs = "lbs";
            if (objUOM.UOM.SizeName.ToUpper() != "IN")
            {
                charIn = objUOM.UOM.SizeName.ToString();
            }
            else if (objUOM.UOM.WeightName.ToUpper() != "LBS")
            {
                charLbs = objUOM.UOM.WeightName.ToString();
            }
            else if (objUOM.UOM.DepthName.ToUpper() != "FT")
            {
                charFt = objUOM.UOM.DepthName.ToString();
            }
            else { }

            string _tblbhaheader = bhaheadtext;

            Table _tblBHA = new Table(13, true);
            _tblBHA.SetFontSize(8);
            Cell _bhaheadcell = new Cell(1, 13).Add(new Paragraph(_tblbhaheader)).SetTextAlignment(TextAlignment.LEFT).SetBackgroundColor(lgtGrey).SetBold();
            _tblBHA.AddHeaderCell(_bhaheadcell);

            Cell bhaid = new Cell(1, 1).SetTextAlignment(TextAlignment.LEFT).Add(new Paragraph("#").SetBold()).SetBackgroundColor(lgtGrey);
            Cell bhatooldesc = new Cell(1, 1).SetTextAlignment(TextAlignment.LEFT).Add(new Paragraph("Tool Description").SetBold()).SetBackgroundColor(lgtGrey);
            Cell bhaserialno = new Cell(1, 1).SetTextAlignment(TextAlignment.LEFT).Add(new Paragraph("Serial Number").SetBold()).SetBackgroundColor(lgtGrey);
            Cell bhamod = new Cell(1, 1).SetTextAlignment(TextAlignment.LEFT).Add(new Paragraph("Measured OD (" + charIn + ")").SetBackgroundColor(lgtGrey).SetBold()).SetBackgroundColor(lgtGrey);
            Cell bhainndia = new Cell(1, 1).SetTextAlignment(TextAlignment.LEFT).Add(new Paragraph("ID (" + charIn + ")").SetBackgroundColor(lgtGrey).SetBold()).SetBackgroundColor(lgtGrey);
            Cell bhaweight = new Cell(1, 1).SetTextAlignment(TextAlignment.LEFT).Add(new Paragraph("Weight (" + charLbs + ")").SetBackgroundColor(lgtGrey).SetBold()).SetBackgroundColor(lgtGrey);
            Cell bhatoollength = new Cell(1, 1).SetTextAlignment(TextAlignment.LEFT).Add(new Paragraph("Length (" + charFt + ")").SetBold()).SetBackgroundColor(lgtGrey);
            Cell bhaupcontyp = new Cell(1, 1).SetTextAlignment(TextAlignment.LEFT).Add(new Paragraph("Upper Conn. Type").SetBold()).SetBackgroundColor(lgtGrey);
            Cell bhalowcontyp = new Cell(1, 1).SetTextAlignment(TextAlignment.LEFT).Add(new Paragraph("Lower Conn. Type").SetBold()).SetBackgroundColor(lgtGrey);
            Cell bhafishneckod = new Cell(1, 1).SetTextAlignment(TextAlignment.LEFT).Add(new Paragraph("Fish Neck OD(" + charIn + ")").SetBold()).SetBackgroundColor(lgtGrey);
            Cell bhafishnecklen = new Cell(1, 1).SetTextAlignment(TextAlignment.LEFT).Add(new Paragraph("Fish Neck Length (" + charFt + ")").SetBold()).SetBackgroundColor(lgtGrey);
            Cell bhahydod = new Cell(1, 1).SetTextAlignment(TextAlignment.LEFT).Add(new Paragraph("Hydraulic OD(" + charIn + ")").SetBold()).SetBackgroundColor(lgtGrey);
            Cell bhahydind = new Cell(1, 1).SetTextAlignment(TextAlignment.LEFT).Add(new Paragraph("Hydraulic ID(" + charIn + ")").SetBold()).SetBackgroundColor(lgtGrey);

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
                double newUom = 0.00;
                string addtocell = string.IsNullOrEmpty(objbhaitem[item]) ? "" : objbhaitem[item].ToString();
                if (item.Contains("Measured"))
                {
                    if (addtocell != "")
                    {
                        newUom = Math.Round((float.Parse(objbhaitem[item]) * objUOM.UOM.SizeMultiplier), 2);
                    }
                }
                else if (item.Contains("InnerDiameter"))
                {
                    if (addtocell != "")
                    {
                        newUom = Math.Round((float.Parse(objbhaitem[item]) * objUOM.UOM.SizeMultiplier), 2);
                    }
                }
                else if (item.Contains("Weight"))
                {
                    if (addtocell != "")
                    {
                        newUom = Math.Round((float.Parse(objbhaitem[item]) * objUOM.UOM.WeightMultiplier), 2);
                    }
                }
                else if (item.Contains("Length"))
                {
                    if (addtocell != "")
                    {
                        newUom = Math.Round((float.Parse(objbhaitem[item]) * objUOM.UOM.DepthMultiplier), 2);
                    }
                }
                else if (item.Contains("FishNeckOD"))
                {
                    if (addtocell != "")
                    {
                        newUom = Math.Round((float.Parse(objbhaitem[item]) * objUOM.UOM.SizeMultiplier), 2);
                    }
                }
                else if (item.Contains("LenFshNck"))
                {
                    if (addtocell != "")
                    {
                        newUom = Math.Round((float.Parse(objbhaitem[item]) * objUOM.UOM.DepthMultiplier), 2);
                    }
                }
                else if (item.Contains("HydraulicOD"))
                {
                    if (addtocell != "")
                    {
                        newUom = Math.Round((float.Parse(objbhaitem[item]) * objUOM.UOM.SizeMultiplier), 2);
                    }
                }
                else if (item.Contains("HydraulicID"))
                {
                    if (addtocell != "")
                    {
                        newUom = Math.Round((float.Parse(objbhaitem[item]) * objUOM.UOM.SizeMultiplier), 2);
                    }
                }
                else { }
                if (newUom > 0)
                {
                    addtocell = newUom.ToString();
                }
                else
                {
                    addtocell = addtocell;
                }

                Cell bhaidv = new Cell(1, 1).SetTextAlignment(TextAlignment.LEFT).Add(new Paragraph(addtocell));
                _tblBHA.AddCell(bhaidv);
            }

            return _tblBHA.SetAutoLayout();
        }
        #endregion

        #endregion
    }
}