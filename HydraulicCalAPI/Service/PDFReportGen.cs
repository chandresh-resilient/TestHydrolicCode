using System;

using System.Linq;
using System.Collections.Generic;

using SkiaSharp;
using iText.Layout;
using iText.IO.Image;
using iText.Kernel.Pdf;
using iText.Kernel.Geom;
using iText.Kernel.Colors;
using iText.Layout.Borders;
using iText.Layout.Element;
using iText.Layout.Properties;
using iText.Kernel.Pdf.Canvas.Draw;

using HydraulicCalAPI.Controllers;
using HydraulicCalAPI.Service;
using Microsoft.AspNetCore.Mvc;
using System.IO;


namespace HydraulicCalAPI.Service
{
    public class PDFReportGen
    {
        Color accuColor;

        Dictionary<string, string> pdfAuthor;
        Dictionary<string, string> pdfLstItemData;
        List<PdfReportService> pdfFooter;

        Dictionary<string, Object> pdfPieChart = new Dictionary<string, object>();
        string[] _fontfamily = { "Arial", "Helvetica", "sans-serif" };
        int increment = 0;

        HydraulicCalculationService objHydCalSrvs = new HydraulicCalculationService();
        public byte[] generatePDF(HydraulicCalAPI.Service.PdfReportService objInputData, ChartAndGraphService objChartService, HydraulicCalculationService inputHydra)
        {
            objHydCalSrvs = inputHydra;
            string ApplicationFolderName = "Accuview";
            string _tabheader = string.Empty;
            var ControlCutApplicationFolder = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\Weatherford\\" + ApplicationFolderName + "\\";
            accuColor = new DeviceRgb(165, 42, 42);

            // Add image
            Image img = new Image(ImageDataFactory
               .Create("wft.jpg"))
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
                .SetBorder(Border.NO_BORDER)
                ;
            footer = FooterSection(pdfFooter);

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
            Paragraph _casingLinerTubingInfo = new Paragraph("CH Fishing Run 1")
            .SetTextAlignment(TextAlignment.CENTER)
            .SetFontSize(20).SetBold();
            List<string> pdfCasingData = new List<string>();

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
            Dictionary<string, string> objcltdata = new Dictionary<string, string>();
            foreach (var cltItem in objChartService.HydraulicOutputAnnulusList)
            {
                pdfLstItemData = new Dictionary<string, string>();
                increment++;
                pdfLstItemData.Add("CLTID", increment.ToString());
                pdfLstItemData.Add("WellBoreSection", cltItem.Annulus != null ? cltItem.Annulus.ToString() : "");
                pdfLstItemData.Add("OutDiameter", cltItem.ToolOuterDiameter > 0 ? cltItem.ToolOuterDiameter.ToString() : "0");
                pdfLstItemData.Add("InnDiameter", cltItem.InnerDiameter > 0 ? cltItem.InnerDiameter.ToString() : "0");

                // Code to get WellBore weight and WellBore Grade form PdfReportService
                //IEnumerable<string> cltWeight = from x in objInputData.CasingLinerTubeData
                //                             where x.WellBoreSection == cltItem.Annulus && x.WellTop == cltItem.FromAnnulus
                //                             select x.WellBoreWeight;
                //IEnumerable<string> cltGrade = from x in objInputData.CasingLinerTubeData
                //                                where x.WellBoreSection == cltItem.Annulus && x.WellTop == cltItem.FromAnnulus
                //                                select x.WellBoreWeight;
                string cltWeight = "23.8";
                string cltGrade = "F1";
                pdfLstItemData.Add("WellBoreWeight", cltWeight != null ? cltWeight.ToString() : "0");
                pdfLstItemData.Add("Grade", cltGrade != null ? cltGrade.ToString() : "");
                pdfLstItemData.Add("WellTop", cltItem.FromAnnulus >= 0 ? cltItem.FromAnnulus.ToString() : "0");
                pdfLstItemData.Add("WellBottom", cltItem.ToAnnulus > 0 ? cltItem.ToAnnulus.ToString() : "");
                objcltdata.Add("clt" + increment, pdfLstItemData.ToString());
            }


            Table tblCasingLinerTube = new Table(1, false);  // = getCasingLinerTubing(objcltdata, _tabheader);

            pdfLstItemData.Clear();
            increment = 0;
            #endregion

            #region BHA Input Data
            _tabheader = "Bottom Hole Assembly (Top Down)";
            Dictionary<string, string> objbdata = new Dictionary<string, string>();
            foreach (var wrkStrlstitem in objHydCalSrvs.bhaInput)
            {
                pdfLstItemData = new Dictionary<string, string>();
                increment++;
                pdfLstItemData.Add("BhaLstID", increment.ToString());
                pdfLstItemData.Add("ToolDescription", wrkStrlstitem.SectionName != null ? wrkStrlstitem.SectionName.ToString() : "");
                pdfLstItemData.Add("SerialNumber", "");
                pdfLstItemData.Add("MeasuredOD", wrkStrlstitem.OutsideDiameterInInch > 0 ? wrkStrlstitem.OutsideDiameterInInch.ToString() : "0");
                pdfLstItemData.Add("InnerDiameter", wrkStrlstitem.InsideDiameterInInch > 0 ? wrkStrlstitem.InsideDiameterInInch.ToString() : "0");

                // code to get Workstring Weight and Upper Connection type
                //IEnumerable<string> wrkStrWeight = from x in objInputData.WorkStringItems
                //                                   where x.wrkToolDescription == wrkStrlstitem.SectionName && x.wrkLength == wrkStrlstitem.LengthInFeet.ToString()
                //                                select x.wrkWeight;
                //IEnumerable<string> wrkStrUpConnTyp = from x in objInputData.WorkStringItems
                //                                      where x.wrkToolDescription == wrkStrlstitem.SectionName && x.wrkLength == wrkStrlstitem.LengthInFeet.ToString()
                //                                      select x.wrkUpperConnType;
                string wrkStrWeight = "23";
                string wrkStrUpConnTyp = "F1";
                pdfLstItemData.Add("Weight", wrkStrWeight != null ? wrkStrWeight.ToString() : "0");
                pdfLstItemData.Add("Length", wrkStrlstitem.LengthInFeet > 0 ? wrkStrlstitem.LengthInFeet.ToString() : "0");
                pdfLstItemData.Add("UpperConnType", wrkStrUpConnTyp != null ? wrkStrUpConnTyp.ToString() : "");
                pdfLstItemData.Add("LowerConnType", "");
                pdfLstItemData.Add("FishNeckOD", "");
                pdfLstItemData.Add("FishNeckLength", "");
                pdfLstItemData.Add("HydraulicOD", "");
                pdfLstItemData.Add("HydraulicID", "");
                objbdata.Add("bd" + increment, pdfLstItemData.ToString());
            }

            // loop to get BHA Tool Data
            foreach (var bhalstitem in objHydCalSrvs.bhaInput)
            {
                pdfLstItemData = new Dictionary<string, string>();
                increment++;
                pdfLstItemData.Add("BhaLstID", increment.ToString());
                pdfLstItemData.Add("ToolDescription", bhalstitem.toolDescription != null ? bhalstitem.toolDescription.ToString() : "");
                pdfLstItemData.Add("SerialNumber", bhalstitem.PositionNumber > 0 ? bhalstitem.PositionNumber.ToString() : "");
                pdfLstItemData.Add("MeasuredOD", bhalstitem.OutsideDiameterInInch > 0 ? bhalstitem.OutsideDiameterInInch.ToString() : "0");
                pdfLstItemData.Add("InnerDiameter", bhalstitem.InsideDiameterInInch > 0 ? bhalstitem.InsideDiameterInInch.ToString() : "0");

                // code to get BHA Weight and Upper Connection type
                //IEnumerable<string> bhatoolWeight = from x in objInputData.BHAToolItemData
                //                                   where x.SerialNumber == bhalstitem.PositionNumber.ToString() && x.Length == bhalstitem.LengthInFeet.ToString()
                //                                   select x.Weight;
                //IEnumerable<string> bhatoolUpConnTyp = from x in objInputData.BHAToolItemData
                //                                       where x.SerialNumber == bhalstitem.PositionNumber.ToString() && x.Length == bhalstitem.LengthInFeet.ToString()
                //                                       select x.UpperConnType;
                //IEnumerable<string> bhatoolLowConntyp = from x in objInputData.BHAToolItemData
                //                                        where x.SerialNumber == bhalstitem.PositionNumber.ToString() && x.Length == bhalstitem.LengthInFeet.ToString()
                //                                        select x.LowerConnType;
                //IEnumerable<string> bhatoolFishNeckOD = from x in objInputData.BHAToolItemData
                //                                        where x.SerialNumber == bhalstitem.PositionNumber.ToString() && x.Length == bhalstitem.LengthInFeet.ToString()
                //                                        select x.FishNeckOD;
                //IEnumerable<string> bhatoolFishNeckLen = from x in objInputData.BHAToolItemData
                //                                         where x.SerialNumber == bhalstitem.PositionNumber.ToString() && x.Length == bhalstitem.LengthInFeet.ToString()
                //                                         select x.FishNeckLength;
                string bhatoolWeight = "23";
                string bhatoolUpConnTyp = "F1";
                string bhatoolLowConntyp = "23";
                string bhatoolFishNeckOD = "F1";
                string bhatoolFishNeckLen = "23";


                pdfLstItemData.Add("Weight", bhatoolWeight != null ? bhatoolWeight.ToString() : "0");
                pdfLstItemData.Add("Length", bhalstitem.LengthInFeet > 0 ? bhalstitem.LengthInFeet.ToString() : "0");
                pdfLstItemData.Add("UpperConnType", bhatoolUpConnTyp != null ? bhatoolUpConnTyp.ToString() : "");
                pdfLstItemData.Add("LowerConnType", bhatoolLowConntyp != null ? bhatoolLowConntyp.ToString() : "");
                pdfLstItemData.Add("FishNeckOD", bhatoolFishNeckOD != null ? bhatoolFishNeckOD.ToString() : "");
                pdfLstItemData.Add("FishNeckLength", bhatoolFishNeckLen != null ? bhatoolFishNeckLen.ToString() : "0");
                pdfLstItemData.Add("HydraulicOD", bhalstitem.OutsideDiameterInInch > 0 ? bhalstitem.OutsideDiameterInInch.ToString() : "0");
                pdfLstItemData.Add("HydraulicID", bhalstitem.OutsideDiameterInInch > 0 ? bhalstitem.OutsideDiameterInInch.ToString() : "0");
                objbdata.Add("bd" + increment, pdfLstItemData.ToString());
            }
            increment = 0;
            Table tblBhaData = new Table(1, false);// = getBha(pdfLstItemData, _tabheader);

            pdfLstItemData.Clear();

            #endregion

            #region Surface Equipment / Fluid Envelop / Fluid

            _tabheader = "Surface Equipment";
            var _surfaceEquipmentData = objHydCalSrvs.surfaceEquipmentInput;
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
            //foreach (var fluidlstitem in objInputData.FluidItemData)
            //{
            //    pdfHederInfoData.Add("Solids", fluidlstitem.Solids > 0.00 ? (fluidlstitem.Solids.ToString() + " | % ") : "0.00");
            //    pdfHederInfoData.Add("Drilling Fluid Type", fluidlstitem.DrillingFluidType != null ? fluidlstitem.DrillingFluidType.ToString() : "");
            //    pdfHederInfoData.Add("Drilling Fluid Weight", objHydCalSrvs.fluidInput.DensityInPoundPerGallon > 0.00 ? (objHydCalSrvs.fluidInput.DensityInPoundPerGallon.ToString() + " | lb/gal") : "0.00");
            //    pdfHederInfoData.Add("Buoyancy Factor", fluidlstitem.BuoyancyFactor > 0.00 ? (fluidlstitem.BuoyancyFactor.ToString() + " | lb/gal") : "0.00");
            //    pdfHederInfoData.Add("Plastic Viscosity", objHydCalSrvs.fluidInput.PlasticViscosityInCentiPoise > 0.00 ? (objHydCalSrvs.fluidInput.PlasticViscosityInCentiPoise.ToString() + " | centipoise") : "0.00");
            //    pdfHederInfoData.Add("Yield Point", objHydCalSrvs.fluidInput.YieldPointInPoundPerFeetSquare > 0.00 ? (objHydCalSrvs.fluidInput.YieldPointInPoundPerFeetSquare.ToString() + " | lbf/100ft²") : "0.00");
            //    pdfHederInfoData.Add("Cutting Average Size", objHydCalSrvs.cuttingsInput.AverageCuttingSizeInInch > 0 ? (objHydCalSrvs.cuttingsInput.AverageCuttingSizeInInch.ToString() + " | in") : "0.00");
            //    pdfHederInfoData.Add("Cutting Type", fluidlstitem.CuttingType != null ? fluidlstitem.CuttingType.ToString() : "");
            //}
            Table tblFluid = new Table(1, false); //= getSurfacePageInfo(pdfHederInfoData, _tabheader);

            pdfHederInfoData.Clear();

            #endregion

            #region Chart and Graph Section

            Paragraph _chartheader = new Paragraph("Pressure Distribution Chart")
             .SetTextAlignment(TextAlignment.CENTER)
             .SetFontSize(12).SetBold();

            foreach (var itempiedata in objChartService.PressureDistributionChartCollection)
            {
                increment++;
                Dictionary<string, string> piechartcollection = new Dictionary<string, string>();
                piechartcollection.Add("Name", itempiedata.Name != null ? itempiedata.Name.ToString() : "");
                piechartcollection.Add("Value", itempiedata.Value > 0 ? itempiedata.Value.ToString() : "");
                piechartcollection.Add("Color", itempiedata.Color != null ? itempiedata.Color.ToString() : "");
                pdfPieChart.Add("object" + increment, piechartcollection);
            }
            increment = 0;
            byte[] chartBytes = GeneratePieChart(pdfPieChart);


            Image imgPie = new Image(ImageDataFactory.Create(chartBytes));




            #endregion

            #region Report Section
            // Define the path for the temp directory
            var tempDir = System.IO.Path.Combine(System.IO.Path.GetTempPath(), "MyTempPdfDir");
            // Ensure the temp directory exists
            if (!Directory.Exists(tempDir))
            {
                Directory.CreateDirectory(tempDir);
            }

            // Generate a random file name for the PDF
            var tempFileName = System.IO.Path.Combine(tempDir, Guid.NewGuid().ToString() + ".pdf");


            try
            {
                using (PdfWriter writer = new PdfWriter(tempFileName))
                {
                    using (PdfDocument pdf = new PdfDocument(writer))
                    {
                        Document document = new Document(pdf, PageSize.A4);
                        NewMethod(img, newline, legend, ls, header, tableAuthor, comment, footer, _headerinfo, tblSegment, tblJobInformation, tblWellInformation, tblOriginator, tblCustomerContacts, tblWeatherfordContacts, tblGenInfo, tblApproval, _casingLinerTubingInfo, tblDepthAnalysis, tblCasingLinerTube, tblBhaData, tblSurfaceEquipment, tblFluidEnvelope, tblFluid, _chartheader, imgPie, document);
                        document.Close();


                    }
                }
                return GetPdfBytesFromFile(tempFileName);
            }
            finally
            {
                if (File.Exists(tempFileName))
                {
                    File.Delete(tempFileName);
                }
            }


            /* Page numbers
            int n = pdf.GetNumberOfPages();
            for (int i = 1; i <= n; i++)
            {
                document.ShowTextAligned(new Paragraph(String
                   .Format(i + " out of " + n)),
                   559, 806, i, TextAlignment.RIGHT, VerticalAlignment.BOTTOM, 0);
            }*/

            // document.Close();

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
        private static void NewMethod(Image img, Paragraph newline, Paragraph legend, LineSeparator ls, Paragraph header, Table tableAuthor, Paragraph comment, Table footer, Paragraph _headerinfo, Table tblSegment, Table tblJobInformation, Table tblWellInformation, Table tblOriginator, Table tblCustomerContacts, Table tblWeatherfordContacts, Table tblGenInfo, Table tblApproval, Paragraph _casingLinerTubingInfo, Table tblDepthAnalysis, Table tblCasingLinerTube, Table tblBhaData, Table tblSurfaceEquipment, Table tblFluidEnvelope, Table tblFluid, Paragraph _chartheader, Image imgPie, Document document)
        {
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
        }

        #region Methods

        #region Graph and Chart Generate Method Section

        public byte[] GeneratePieChart(Dictionary<string, Object> objPrsDrop)
        {
            using (var surfcae = SKSurface.Create(new SKImageInfo(400, 400)))
            {
                var canvas = surfcae.Canvas;
                canvas.Clear(SKColors.White);
                float totalValue = 0;
                foreach (var itemval in objPrsDrop.Keys)
                {
                    //totalValue += Convert.ToInt32(itemval[.);
                    totalValue = 100;
                }

                float startAngle = 0;

                foreach (var lstitem in objPrsDrop)
                {
                    float sweepAngle = (Convert.ToInt32(34) / totalValue) * 360;
                    using (var paint = new SKPaint())
                    {
                        string colourName = "Red";

                        //lstitem.color;
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
        public Table getDepthAnalysis(List<string> objDpthAnalysisValue, string tablehead)
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

            foreach (var item in objclt.Keys)
            {
                Cell cltidv = new Cell(1, 1).SetTextAlignment(TextAlignment.LEFT).Add(new Paragraph(objclt["CLTID"]));
                Cell cltwellsectionv = new Cell(1, 1).SetTextAlignment(TextAlignment.LEFT).Add(new Paragraph(objclt["WellBoreSection"]));
                Cell cltoutdiav = new Cell(1, 1).SetTextAlignment(TextAlignment.LEFT).Add(new Paragraph(objclt["OutDiameter"]));
                Cell cltindiav = new Cell(1, 1).SetTextAlignment(TextAlignment.LEFT).Add(new Paragraph(objclt["InnDiameter"]));
                Cell cltweightv = new Cell(1, 1).SetTextAlignment(TextAlignment.LEFT).Add(new Paragraph(objclt["WellBoreWeight"]));
                Cell cltgradev = new Cell(1, 1).SetTextAlignment(TextAlignment.LEFT).Add(new Paragraph(objclt["Grade"]));
                Cell clttopv = new Cell(1, 1).SetTextAlignment(TextAlignment.LEFT).Add(new Paragraph(objclt["WellTop"]));
                Cell cltbottomv = new Cell(1, 1).SetTextAlignment(TextAlignment.LEFT).Add(new Paragraph(objclt["WellBottom"]));

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
        public Table getBha(Dictionary<string, string> objbhaitem, string bhaheadtext)
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

            foreach (var item in objbhaitem.Keys)
            {
                Cell bhaidv = new Cell(1, 1).SetTextAlignment(TextAlignment.LEFT).Add(new Paragraph(objbhaitem["BhaLstID"].ToString()));
                Cell bhatooldescv = new Cell(1, 1).SetTextAlignment(TextAlignment.LEFT).Add(new Paragraph(objbhaitem["ToolDescription"].ToString()));
                Cell bhaserialnov = new Cell(1, 1).SetTextAlignment(TextAlignment.LEFT).Add(new Paragraph(objbhaitem["SerialNumber"].ToString()));
                Cell bhamodv = new Cell(1, 1).SetTextAlignment(TextAlignment.LEFT).Add(new Paragraph(objbhaitem["MeasuredOD"].ToString()));
                Cell bhainndiav = new Cell(1, 1).SetTextAlignment(TextAlignment.LEFT).Add(new Paragraph(objbhaitem["InnerDiameter"].ToString()));
                Cell bhaweightv = new Cell(1, 1).SetTextAlignment(TextAlignment.LEFT).Add(new Paragraph(objbhaitem["Weight"].ToString()));
                Cell bhatoollengthv = new Cell(1, 1).SetTextAlignment(TextAlignment.LEFT).Add(new Paragraph(objbhaitem["Length"].ToString()));
                Cell bhaupcontypv = new Cell(1, 1).SetTextAlignment(TextAlignment.LEFT).Add(new Paragraph(objbhaitem["UpperConnType"].ToString()));
                Cell bhalowcontypv = new Cell(1, 1).SetTextAlignment(TextAlignment.LEFT).Add(new Paragraph(objbhaitem["LowerConnType"].ToString()));
                Cell bhafishneckodv = new Cell(1, 1).SetTextAlignment(TextAlignment.LEFT).Add(new Paragraph(objbhaitem["FishNeckOD"].ToString()));
                Cell bhafishnecklenv = new Cell(1, 1).SetTextAlignment(TextAlignment.LEFT).Add(new Paragraph(objbhaitem["FishNeckLength"].ToString()));
                Cell bhahydodv = new Cell(1, 1).SetTextAlignment(TextAlignment.LEFT).Add(new Paragraph(objbhaitem["HydraulicOD"].ToString()));
                Cell bhahydindv = new Cell(1, 1).SetTextAlignment(TextAlignment.LEFT).Add(new Paragraph(objbhaitem["HydraulicID"].ToString()));

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

        public Table getFluidEnvelopeInfo(Dictionary<string, string> objfluidvalues, string fluidenvheader)
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

