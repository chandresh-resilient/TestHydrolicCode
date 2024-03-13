using iText.IO.Image;
using iText.Kernel.Colors;
using iText.Kernel.Geom;
using iText.Kernel.Pdf.Canvas.Draw;
using iText.Kernel.Pdf;
using iText.Layout.Borders;
using iText.Layout.Element;
using iText.Layout.Properties;
using iText.Layout;
using SkiaSharp;
using System.Collections.Generic;
using System.IO;
using System;

namespace HydraulicCalAPI.Service
{
    public class PDFReportGen
    {
        Color accuColor;
        PdfDocument pdf;
        Document document;
        Dictionary<string, string> pdfAuthor;
        List<PdfReportService> pdfFooter;
        List<PdfReportService> pdfCasingData;
        
        public void generatePDF(HydraulicCalAPI.Service.PdfReportService objInputData)
        {

            string ApplicationFolderName = "Accuview";
            string _tabheader = string.Empty;
            var ControlCutApplicationFolder = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\Weatherford\\" + ApplicationFolderName + "\\";
            accuColor = new DeviceRgb(165, 42, 42);

            PdfWriter writer = new PdfWriter("D:\\AccuView_Docs\\demo.pdf");

            pdf = new PdfDocument(writer);
            document = new Document(pdf, PageSize.A4);
            //PdfWriter writer = new PdfWriter(ControlCutApplicationFolder + "{0}.pdf");

            // Add image
            Image img = new Image(ImageDataFactory
               .Create(@"D:\ReportGeneratorAPI\HydraReportGenerator\wft.jpg"))
               .SetTextAlignment(TextAlignment.LEFT).SetWidth(100).SetHeight(40);

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

            #region Footer Section
            // List for footer Table
            pdfFooter = new List<PdfReportService>();
            pdfFooter.Add(new PdfReportService
            {
                JobID = (objInputData.JobID != null ? objInputData.JobID.ToString() : ""),
                WPTSReportID = (objInputData.WPTSReportID != null ? objInputData.WPTSReportID.ToString() : ""),
                AccuViewVersion = (objInputData.AccuViewVersion != null ? objInputData.AccuViewVersion.ToString() : "")
            });

            Table footer = new Table(1, false)
                .SetBorder(Border.NO_BORDER);
            footer = FooterSection(pdfFooter);

            #endregion

            #region HeaderInformation
            Dictionary<string, string> pdfHederInfoData = new Dictionary<string, string>();
            Paragraph _headerinfo = new Paragraph("Header Information")
             .SetTextAlignment(TextAlignment.CENTER)
             .SetFontSize(20).SetBold();

            _tabheader = "Segment and Product / Service";
            pdfHederInfoData.Add("Segment", objInputData.Segment != null ? objInputData.Segment.ToString() : "");
            pdfHederInfoData.Add("Product / Service", objInputData.ProductService != null ? objInputData.ProductService.ToString() : "");
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
            Table tblGenInfo = getHeaderInfoTable(pdfHederInfoData, _tabheader);
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
            Paragraph _casingLinerTubingInfo = new Paragraph("CH Fishing Run 1")
            .SetTextAlignment(TextAlignment.CENTER)
            .SetFontSize(20).SetBold();

            _tabheader = "Depth Analysis";
            pdfCasingData = new List<PdfReportService>();
            List<CaseLinerTube> pdfCasingItemData = new List<CaseLinerTube>();

            pdfCasingData.Add(new PdfReportService
            {
                AnnulusLength = (objInputData.AnnulusLength != null ? (objInputData.AnnulusLength.ToString() + " | ft") : "0.00"),
                BHALength = (objInputData.BHALength != null ? (objInputData.BHALength.ToString() + " | ft") : "0.00"),
                ToolDepth = (objInputData.ToolDepth != null ? (objInputData.ToolDepth.ToString() + " | ft") : "0.00")
            });
            Table tblDepthAnalysis = getDepthAnalysis(pdfCasingData, _tabheader);
            pdfCasingData.Clear();

            _tabheader = "Casing/ Liner/ Tubing Data";
            // preparing Casing, Liner and Tubing Object List

            foreach (var cltItem in objInputData.CasingLinerTubing)
            {
                pdfCasingItemData.Add(new CaseLinerTube
                {
                    CLTID = cltItem.CLTID != null ? cltItem.CLTID : "",
                    WellBoreSection = cltItem.WellBoreSection != null ? cltItem.WellBoreSection : "",
                    OutDiameter = cltItem.OutDiameter != null ? cltItem.OutDiameter : "",
                    InnDiameter = cltItem.InnDiameter != null ? cltItem.InnDiameter : "",
                    WellBoreWeight = cltItem.WellBoreWeight != null ? cltItem.WellBoreWeight : "",
                    Grade = cltItem.Grade != null ? cltItem.Grade : "",
                    WellTop = cltItem.WellTop != null ? cltItem.WellTop : "",
                    WellBottom = cltItem.WellBottom != null ? cltItem.WellBottom : ""
                });
            }

            Table tblCasingLinerTube = getCasingLinerTubing(pdfCasingItemData, _tabheader);
            pdfCasingItemData.Clear();
            #endregion

            #region BHA Input Data
            _tabheader = "Bottom Hole Assembly (Top Down)";
            List<BhaTopToBottom> pdfBhaItemData = new List<BhaTopToBottom>();

            foreach (var wrkstritem in objInputData.WorkStringItem)
            {
                pdfBhaItemData.Add(new BhaTopToBottom
                {
                    ID = wrkstritem.wrkID != null ? wrkstritem.wrkID : "",
                    ToolDescription = wrkstritem.wrkToolDescription != null ? wrkstritem.wrkToolDescription : "",
                    SerialNumber = "",
                    MeasuredOD = wrkstritem.wrkMeasuredOD != null ? wrkstritem.wrkMeasuredOD : "",
                    InnerDiameter = wrkstritem.wrkInnerDiameter != null ? wrkstritem.wrkInnerDiameter : "",
                    Weight = wrkstritem.wrkWeight != null ? wrkstritem.wrkWeight : "",
                    Length = wrkstritem.wrkLength != null ? wrkstritem.wrkLength : "",
                    UpperConnType = wrkstritem.wrkUpperConnType != null ? wrkstritem.wrkUpperConnType : "",
                    LowerConnType = "",
                    FishNeckOD = "",
                    FishNeckLength = "",
                    HydraulicOD = "",
                    HydraulicID = "",
                });
            };

            foreach (var itembha in objInputData.BhaTopToBottom)
            {
                pdfBhaItemData.Add(new BhaTopToBottom
                {
                    ID = itembha.ID != null ? itembha.ID : "",
                    ToolDescription = itembha.ToolDescription != null ? itembha.ToolDescription : "",
                    SerialNumber = itembha.SerialNumber != null ? itembha.SerialNumber : "",
                    MeasuredOD = itembha.MeasuredOD != null ? itembha.MeasuredOD : "",
                    InnerDiameter = itembha.InnerDiameter != null ? itembha.InnerDiameter : "",
                    Weight = itembha.Weight != null ? itembha.Weight : "",
                    Length = itembha.Length != null ? itembha.Length : "",
                    UpperConnType = itembha.UpperConnType != null ? itembha.UpperConnType : "",
                    LowerConnType = itembha.LowerConnType != null ? itembha.LowerConnType : "",
                    FishNeckOD = itembha.FishNeckOD != null ? itembha.FishNeckOD : "",
                    FishNeckLength = itembha.FishNeckLength != null ? itembha.FishNeckLength : "",
                    HydraulicOD = itembha.HydraulicOD != null ? itembha.HydraulicOD : "",
                    HydraulicID = itembha.HydraulicID != null ? itembha.HydraulicID : ""
                });
            }
            Table tblBhaData = getBha(pdfBhaItemData, _tabheader);
            pdfBhaItemData.Clear();

            #endregion

            #region Surface Equipment / Fluid Envelop / Fluid

            _tabheader = "Surface Equipment";
            pdfHederInfoData.Add("Surface Equipment", objInputData.SurfaceEquipment != null ? objInputData.SurfaceEquipment.ToString() : "");
            pdfHederInfoData.Add("Total Length", objInputData.TotalLength > 0.00 ? (objInputData.TotalLength.ToString() + " | ft") : "0.00");
            Table tblSurfaceEquipment = getHeaderInfoTable(pdfHederInfoData, _tabheader);
            pdfHederInfoData.Clear();

            _tabheader = "Fluid Envelope";
            pdfCasingData = new List<PdfReportService>();
            pdfCasingData.Add(new PdfReportService
            {
                MaximumAllowablePressure = (objInputData.MaximumAllowablePressure != null ? (objInputData.MaximumAllowablePressure.ToString() + " | psi") : "0"),
                MaximumAllowableFlowrate = (objInputData.MaximumAllowableFlowrate != null ? (objInputData.MaximumAllowableFlowrate.ToString() + " | gal/min") : "0"),
                Comments = (objInputData.Comments != null ? objInputData.Comments.ToString() : "0.00")
            });
            Table tblFluidEnvelope = getFluidEnvelopeInfo(pdfCasingData, _tabheader);
            pdfCasingData.Clear();

            _tabheader = "Fluid";
            pdfHederInfoData.Add("Solids", objInputData.Solids > 0.00 ? (objInputData.Solids.ToString() + " | % ") : "0.00");
            pdfHederInfoData.Add("Drilling Fluid Type", objInputData.DrillingFluidType != null ? objInputData.TotalLength.ToString() : "");
            pdfHederInfoData.Add("Drilling Fluid Weight", objInputData.DrillingFluidWeight > 0.00 ? (objInputData.SurfaceEquipment.ToString() + " | lb/gal") : "0.00");
            pdfHederInfoData.Add("Buoyancy Factor", objInputData.BuoyancyFactorl > 0.00 ? (objInputData.BuoyancyFactorl.ToString() + " | lb/gal") : "0.00");
            pdfHederInfoData.Add("Plastic Viscosity", objInputData.PlasticViscosity > 0.00 ? (objInputData.SurfaceEquipment.ToString() + " | centipoise") : "0.00");
            pdfHederInfoData.Add("Yield Point", objInputData.YieldPoint > 0.00 ? (objInputData.YieldPoint.ToString() + " | lbf/100ft²") : "0.00");
            pdfHederInfoData.Add("Cutting Average Size", objInputData.CuttingAverageSize > 0 ? (objInputData.SurfaceEquipment.ToString() + " | in") : "0.00");
            pdfHederInfoData.Add("Cutting Type", objInputData.CuttingType != null ? objInputData.CuttingType : "");
            Table tblFluid = getSurfacePageInfo(pdfHederInfoData, _tabheader);
            pdfHederInfoData.Clear();

            #endregion

            #region Chart and Graph Section

            Paragraph _chartheader = new Paragraph("Pressure Distribution Chart")
             .SetTextAlignment(TextAlignment.CENTER)
             .SetFontSize(12).SetBold();

            List<PressureDistributionChartCollection> pdfPieChart = new List<PressureDistributionChartCollection>();

            foreach (var itempiedata in objInputData.PressureDistributionChartCollection)
            {
                pdfPieChart.Add(new PressureDistributionChartCollection
                {
                    name = itempiedata.name != null ? itempiedata.name.ToString() : "",
                    value = itempiedata.value != null ? itempiedata.value.ToString() : "",
                    color = itempiedata.color != null ? itempiedata.color : ""
                });
            }
            byte[] chartBytes = GeneratePieChart(pdfPieChart);

            for (int i = 0; i < pdfPieChart.Count; i++)
            {
                legend = new Paragraph(pdfPieChart[i].name.ToString());
                legend.SetMarginRight(10);
                legend.SetMarginTop(5);
            }

            Image imgPie = new Image(ImageDataFactory.Create(chartBytes));




            #endregion

            #region Report Section
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

            document.Add(newline);
            document.Add(newline);
            document.Add(newline);
            document.Add(newline);

            document.Add(ls);
            document.Add(footer);
            document.Add(new AreaBreak(AreaBreakType.NEXT_PAGE));

            document.Add(img);
            document.Add(_headerinfo);
            document.Add(ls);

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

            document.Add(ls);
            document.Add(footer);
            document.Add(new AreaBreak(AreaBreakType.NEXT_PAGE));

            document.Add(img);
            document.Add(_casingLinerTubingInfo);
            document.Add(ls);

            document.Add(newline);
            document.Add(tblDepthAnalysis);
            document.Add(newline);
            document.Add(tblCasingLinerTube);
            tblCasingLinerTube.Flush();
            tblCasingLinerTube.Complete();


            document.Add(newline);
            document.Add(newline);
            document.Add(newline);
            document.Add(ls);
            document.Add(footer);

            document.Add(new AreaBreak(AreaBreakType.NEXT_PAGE));


            document.Add(img);
            document.Add(_casingLinerTubingInfo);
            document.Add(ls);
            document.Add(newline);
            document.Add(tblBhaData);
            tblBhaData.Flush();
            tblBhaData.Complete();
            document.Add(newline);
            document.Add(newline);
            document.Add(newline);
            document.Add(ls);
            document.Add(footer);


            document.Add(new AreaBreak(AreaBreakType.NEXT_PAGE));

            document.Add(img);
            document.Add(_casingLinerTubingInfo);
            document.Add(ls);
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

            document.Add(newline);
            document.Add(newline);
            document.Add(newline);
            document.Add(ls);
            document.Add(footer);

            document.Add(new AreaBreak(AreaBreakType.NEXT_PAGE));
            document.Add(img);
            document.Add(_casingLinerTubingInfo);
            document.Add(ls);
            document.Add(newline);
            document.Add(_chartheader);
            document.Add(newline);
            document.Add(legend);
            document.Add(imgPie);
            document.Add(newline);
            document.Add(ls);
            document.Add(footer);

            /* Page numbers
            int n = pdf.GetNumberOfPages();
            for (int i = 1; i <= n; i++)
            {
                document.ShowTextAligned(new Paragraph(String
                   .Format(i + " out of " + n)),
                   559, 806, i, TextAlignment.RIGHT, VerticalAlignment.BOTTOM, 0);
            }*/

            document.Close();
            #endregion
        }

        #region Methods

        #region Graph and Chart Generate Method Section

        public byte[] GeneratePieChart(List<PressureDistributionChartCollection> objPrsDrop)
        {
            using (var surfcae = SKSurface.Create(new SKImageInfo(400, 400)))
            {
                var canvas = surfcae.Canvas;
                canvas.Clear(SKColors.White);
                float totalValue = 0;
                foreach (var itemval in objPrsDrop)
                {
                    totalValue += Convert.ToInt32(itemval.value);
                }

                float startAngle = 0;

                foreach (var lstitem in objPrsDrop)
                {
                    float sweepAngle = (Convert.ToInt32(lstitem.value) / totalValue) * 360;
                    using (var paint = new SKPaint())
                    {
                        string colourName = lstitem.color;
                        string hexString = ViewModel.ColorConverter.ColorNameToHexString(colourName);

                        paint.Color = SKColor.Parse(hexString);
                        canvas.DrawArc(new SKRect(50, 50, 350, 350), startAngle, sweepAngle, true, paint);
                        startAngle += sweepAngle;
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
        #endregion

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

        #region Footer Implementation
        public Table getFooterTable(List<PdfReportService> objValues)
        {
            Table _tableFoot = new Table(3, false);
            _tableFoot.SetFontSize(7);
            Cell headerJobId = new Cell(1, 1).SetTextAlignment(TextAlignment.LEFT).Add(new Paragraph("AccuView Job ID"));
            Cell headerWPTSId = new Cell(1, 1).SetTextAlignment(TextAlignment.LEFT).Add(new Paragraph("WPTS Report ID"));
            Cell headerAccuViewVerion = new Cell(1, 1).SetTextAlignment(TextAlignment.LEFT).Add(new Paragraph("AccuView Version Number"));

            _tableFoot.AddCell(headerJobId);
            _tableFoot.AddCell(headerWPTSId);
            _tableFoot.AddCell(headerAccuViewVerion);

            foreach (PdfReportService item in objValues)
            {
                Cell jobid = new Cell(1, 1).SetTextAlignment(TextAlignment.LEFT).Add(new Paragraph(item.JobID.ToString()));
                Cell wptsid = new Cell(1, 1).SetTextAlignment(TextAlignment.LEFT).Add(new Paragraph(item.WPTSReportID.ToString()));
                Cell accuviewver = new Cell(1, 1).SetTextAlignment(TextAlignment.LEFT).Add(new Paragraph(item.AccuViewVersion.ToString()));

                _tableFoot.AddCell(jobid);
                _tableFoot.AddCell(wptsid);
                _tableFoot.AddCell(accuviewver);
            }
            return _tableFoot;
        }
        public Table FooterSection(List<PdfReportService> objFooter)
        {
            Table tabsign = getFooterTable(objFooter);


            Table _resultantTable = new Table(1, false)
                .SetBorder(Border.NO_BORDER);

            // FooterSection
            Paragraph tmtext = new Paragraph("AccuView" + "\u2122" + "is a Weatherford trademark").SetFontSize(7);

            Paragraph disclaimer = new Paragraph("© 2015 WEATHERFORD - All Rights Reserved -Proprietary and Confidential: This document is copyrighted and contains valuable proprietary and confidential" +
                                       "information, whether patentable or unpatentable, of Weatherford. Recipients agree the document is loaned with confidential restrictions, and with the understanding that" +
                                        "neither it nor the information contained therein will be reproduced, used or disclosed in whole or in part for any purpose except as may be specifically authorized in" +
                                          "writing by Weatherford.This document shall be returned to Weatherford upon demand.").SetFontSize(7);
            Cell _footcell = new Cell(1, 2)
                 .SetBorder(Border.NO_BORDER);
            _footcell.Add(tabsign);
            Cell _footcell1 = new Cell(2, 2)
                .SetBorder(Border.NO_BORDER);
            _footcell1.Add(tmtext);
            Cell _footcell2 = new Cell(3, 2).SetBorder(Border.NO_BORDER);
            _footcell1.Add(disclaimer);

            _resultantTable.AddCell(_footcell);
            _resultantTable.AddCell(_footcell1);
            _resultantTable.AddCell(_footcell2);


            return _resultantTable;
        }
        #endregion

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

        #region Casing / Liner / Tubing
        public Table getDepthAnalysis(List<PdfReportService> objValues, string tablehead)
        {
            int count = 3;
            string _tblheadText = tablehead;

            Table _tabDepth = new Table(2, false);
            _tabDepth.SetFontSize(10);

            Cell da = new Cell(1, 2).SetTextAlignment(TextAlignment.LEFT).Add(new Paragraph(_tblheadText).SetBackgroundColor(ColorConstants.LIGHT_GRAY).SetWidth(200));
            _tabDepth.AddHeaderCell(da);

            for (int i = 1; i <= count; i++)
            {
                switch (i)
                {
                    case 1:
                        {
                            Cell daAnnulusLength = new Cell(1, 1).SetTextAlignment(TextAlignment.LEFT).Add(new Paragraph("Annulus Length").SetWidth(100));
                            Cell daAnLen = new Cell(1, 1).SetTextAlignment(TextAlignment.LEFT).Add(new Paragraph(objValues[0].AnnulusLength).SetWidth(100));
                            _tabDepth.AddCell(daAnnulusLength);
                            _tabDepth.AddCell(daAnLen);
                            break;
                        }
                    case 2:
                        {
                            Cell daBHALength = new Cell(1, 1).SetTextAlignment(TextAlignment.LEFT).Add(new Paragraph("BHA Length").SetWidth(100));
                            Cell daBhaLen = new Cell(1, 1).SetTextAlignment(TextAlignment.LEFT).Add(new Paragraph(objValues[0].BHALength).SetWidth(100));
                            _tabDepth.AddCell(daBHALength);
                            _tabDepth.AddCell(daBhaLen);
                            break;
                        }
                    case 3:
                        {
                            Cell daToolLength = new Cell(1, 1).SetTextAlignment(TextAlignment.LEFT).Add(new Paragraph("Tool Depth").SetWidth(100));
                            Cell daTulDpth = new Cell(1, 1).SetTextAlignment(TextAlignment.LEFT).Add(new Paragraph(objValues[0].ToolDepth).SetWidth(100));
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

        public Table getCasingLinerTubing(List<CaseLinerTube> objclt, string tablehead)
        {
            string _tblcltheader = tablehead;

            Table _tabclt = new Table(8, true);
            _tabclt.SetFontSize(10);
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

            foreach (var item in objclt)
            {
                Cell cltidv = new Cell(1, 1).SetTextAlignment(TextAlignment.LEFT).Add(new Paragraph(item.CLTID.ToString()));
                Cell cltwellsectionv = new Cell(1, 1).SetTextAlignment(TextAlignment.LEFT).Add(new Paragraph(item.WellBoreSection.ToString()));
                Cell cltoutdiav = new Cell(1, 1).SetTextAlignment(TextAlignment.LEFT).Add(new Paragraph(item.OutDiameter.ToString()));
                Cell cltindiav = new Cell(1, 1).SetTextAlignment(TextAlignment.LEFT).Add(new Paragraph(item.InnDiameter.ToString()));
                Cell cltweightv = new Cell(1, 1).SetTextAlignment(TextAlignment.LEFT).Add(new Paragraph(item.WellBoreWeight.ToString()));
                Cell cltgradev = new Cell(1, 1).SetTextAlignment(TextAlignment.LEFT).Add(new Paragraph(item.Grade.ToString()));
                Cell clttopv = new Cell(1, 1).SetTextAlignment(TextAlignment.LEFT).Add(new Paragraph(item.WellTop.ToString()));
                Cell cltbottomv = new Cell(1, 1).SetTextAlignment(TextAlignment.LEFT).Add(new Paragraph(item.WellBottom.ToString()));

                _tabclt.AddCell(cltidv);
                _tabclt.AddCell(cltwellsectionv);
                _tabclt.AddCell(cltoutdiav);
                _tabclt.AddCell(cltindiav);
                _tabclt.AddCell(cltweightv);
                _tabclt.AddCell(cltgradev);
                _tabclt.AddCell(clttopv);
                _tabclt.AddCell(cltbottomv);
            }
            return _tabclt.SetAutoLayout();
        }

        #endregion

        #region Bottom Hole Assembly
        public Table getBha(List<BhaTopToBottom> objbhaitem, string bhaheadtext)
        {
            string _tblbhaheader = bhaheadtext;

            Table _tblBHA = new Table(13, true);
            _tblBHA.SetFontSize(9);
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

            foreach (var item in objbhaitem)
            {
                Cell bhaidv = new Cell(1, 1).SetTextAlignment(TextAlignment.LEFT).Add(new Paragraph(item.ID.ToString()));
                Cell bhatooldescv = new Cell(1, 1).SetTextAlignment(TextAlignment.LEFT).Add(new Paragraph(item.ToolDescription.ToString()));
                Cell bhaserialnov = new Cell(1, 1).SetTextAlignment(TextAlignment.LEFT).Add(new Paragraph(item.SerialNumber.ToString()));
                Cell bhamodv = new Cell(1, 1).SetTextAlignment(TextAlignment.LEFT).Add(new Paragraph(item.MeasuredOD.ToString()));
                Cell bhainndiav = new Cell(1, 1).SetTextAlignment(TextAlignment.LEFT).Add(new Paragraph(item.InnerDiameter.ToString()));
                Cell bhaweightv = new Cell(1, 1).SetTextAlignment(TextAlignment.LEFT).Add(new Paragraph(item.Weight.ToString()));
                Cell bhatoollengthv = new Cell(1, 1).SetTextAlignment(TextAlignment.LEFT).Add(new Paragraph(item.Length.ToString()));
                Cell bhaupcontypv = new Cell(1, 1).SetTextAlignment(TextAlignment.LEFT).Add(new Paragraph(item.UpperConnType.ToString()));
                Cell bhalowcontypv = new Cell(1, 1).SetTextAlignment(TextAlignment.LEFT).Add(new Paragraph(item.LowerConnType.ToString()));
                Cell bhafishneckodv = new Cell(1, 1).SetTextAlignment(TextAlignment.LEFT).Add(new Paragraph(item.FishNeckOD.ToString()));
                Cell bhafishnecklenv = new Cell(1, 1).SetTextAlignment(TextAlignment.LEFT).Add(new Paragraph(item.FishNeckLength.ToString()));
                Cell bhahydodv = new Cell(1, 1).SetTextAlignment(TextAlignment.LEFT).Add(new Paragraph(item.HydraulicOD.ToString()));
                Cell bhahydindv = new Cell(1, 1).SetTextAlignment(TextAlignment.LEFT).Add(new Paragraph(item.HydraulicID.ToString()));

                _tblBHA.AddCell(bhaidv);
                _tblBHA.AddCell(bhatooldescv);
                _tblBHA.AddCell(bhaserialnov);
                _tblBHA.AddCell(bhamodv);
                _tblBHA.AddCell(bhainndiav);
                _tblBHA.AddCell(bhaweightv);
                _tblBHA.AddCell(bhatoollengthv);
                _tblBHA.AddCell(bhaupcontypv);
                _tblBHA.AddCell(bhalowcontypv);
                _tblBHA.AddCell(bhafishneckodv);
                _tblBHA.AddCell(bhafishnecklenv);
                _tblBHA.AddCell(bhahydodv);
                _tblBHA.AddCell(bhahydindv);
            }

            return _tblBHA.SetAutoLayout();
        }
        #endregion

        #region Fluid Envelope

        public Table getSurfacePageInfo(Dictionary<string, string> objSegment, string tabHeader)
        {
            string tabheadertext = tabHeader;

            Table _tableSurf = new Table(4, true);
            _tableSurf.SetFontSize(10);
            Cell sn = new Cell(1, 4).SetTextAlignment(TextAlignment.LEFT).Add(new Paragraph(tabheadertext).SetBackgroundColor(ColorConstants.LIGHT_GRAY));
            _tableSurf.AddHeaderCell(sn);

            foreach (var item in objSegment)
            {
                _tableSurf.AddCell(new Paragraph(item.Key).SetTextAlignment(TextAlignment.LEFT));
                _tableSurf.AddCell(new Paragraph(item.Value).SetTextAlignment(TextAlignment.LEFT));
            }
            return _tableSurf.SetAutoLayout();
        }

        public Table getFluidEnvelopeInfo(List<PdfReportService> objfluidvalues, string fluidenvheader)
        {
            int count = 3;
            string _tblheadText = fluidenvheader;

            Table _tabFluidEnvelope = new Table(2, false);
            _tabFluidEnvelope.SetFontSize(10);

            Cell da = new Cell(1, 2).SetTextAlignment(TextAlignment.LEFT).Add(new Paragraph(_tblheadText).SetBackgroundColor(ColorConstants.LIGHT_GRAY).SetWidth(200));
            _tabFluidEnvelope.AddHeaderCell(da);

            for (int i = 1; i <= count; i++)
            {
                switch (i)
                {
                    case 1:
                        {
                            Cell maxflowpressure = new Cell(1, 1).SetTextAlignment(TextAlignment.LEFT).Add(new Paragraph("Maximum Allowable Pressure").SetWidth(100));
                            Cell maxflowpr = new Cell(1, 1).SetTextAlignment(TextAlignment.LEFT).Add(new Paragraph(objfluidvalues[0].MaximumAllowablePressure).SetWidth(100));
                            _tabFluidEnvelope.AddCell(maxflowpressure);
                            _tabFluidEnvelope.AddCell(maxflowpr);
                            break;
                        }
                    case 2:
                        {
                            Cell maxflowrate = new Cell(1, 1).SetTextAlignment(TextAlignment.LEFT).Add(new Paragraph("Maximum Allowable Flowrate").SetWidth(100));
                            Cell maxflrate = new Cell(1, 1).SetTextAlignment(TextAlignment.LEFT).Add(new Paragraph(objfluidvalues[0].MaximumAllowableFlowrate).SetWidth(100));
                            _tabFluidEnvelope.AddCell(maxflowrate);
                            _tabFluidEnvelope.AddCell(maxflrate);
                            break;
                        }
                    case 3:
                        {
                            Cell fecomment = new Cell(1, 1).SetTextAlignment(TextAlignment.LEFT).Add(new Paragraph("Comments").SetWidth(100));
                            Cell fecomm = new Cell(1, 1).SetTextAlignment(TextAlignment.LEFT).Add(new Paragraph(objfluidvalues[0].Comments).SetWidth(100));
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
        public Paragraph getStandPipeFlowRate()
        {
            Paragraph _headertext = new Paragraph("Hydraulic Analysis Weatherford Fishing")
              .SetTextAlignment(TextAlignment.CENTER)
              .SetFontSize(20).SetBold().SetFontColor(accuColor);
            return _headertext;
        }

        public Paragraph getHydraulicOutput()
        {
            Paragraph _headertext = new Paragraph("Hydraulic Analysis Weatherford Fishing")
              .SetTextAlignment(TextAlignment.CENTER)
              .SetFontSize(20).SetBold().SetFontColor(accuColor);
            return _headertext;
        }
        #endregion
    }

}

