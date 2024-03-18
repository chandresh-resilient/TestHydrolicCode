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
using System.Text;

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

    public class PDFReportGen
    {
        Color accuColor;

        Dictionary<string, string> pdfAuthor;
        Dictionary<string, string> pdfLstItemData;
        List<PdfReportService> pdfFooter;

        Dictionary<string, string> pdfPieChart = new Dictionary<string, string>();
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
            Dictionary<string, string> dicLstCltData = new Dictionary<string, string>(); ;
            foreach (var cltItem in objHydCalSrvs.annulusInput)
            {
                increment++;
                //Code to get WellBore weight and WellBore Grade form PdfReportService
                var cltWeight = objInputData.CasingLinerTubeData.Where(x => x.WellBoreSection.Equals(cltItem.WellboreSectionName) && x.WellTop.Equals(cltItem.AnnulusTopInFeet))
                                                    .Select(x => x.WellBoreWeight);

                var cltGrade = objInputData.CasingLinerTubeData.Where(x => x.WellBoreSection.Equals(cltItem.WellboreSectionName) && x.WellTop.Equals(cltItem.AnnulusTopInFeet))
                                                    .Select(x => x.Grade);
                dicLstCltData.Add("CLTID" + increment, increment.ToString());
                dicLstCltData.Add("WellBoreSection" + increment, (cltItem.WellboreSectionName != null ? cltItem.WellboreSectionName.ToString() : ""));
                dicLstCltData.Add("OutDiameter" + increment, (cltItem.AnnulusODInInch > 0 ? cltItem.AnnulusODInInch.ToString() : "0"));
                dicLstCltData.Add("InnDiameter" + increment, (cltItem.AnnulusIDInInch > 0 ? cltItem.AnnulusIDInInch.ToString() : "0"));
                dicLstCltData.Add("WellBoreWeight" + increment, (string.IsNullOrEmpty(cltWeight.ToString()) ? "0" : cltWeight.FirstOrDefault()));
                dicLstCltData.Add("Grade" + increment, (string.IsNullOrEmpty(cltGrade.ToString()) ? "" : cltGrade.FirstOrDefault()));
                dicLstCltData.Add("WellTop" + increment, (cltItem.AnnulusTopInFeet >= 0 ? cltItem.AnnulusTopInFeet.ToString() : "0"));
                dicLstCltData.Add("WellBottom" + increment, (cltItem.AnnulusBottomInFeet > 0 ? cltItem.AnnulusBottomInFeet.ToString() : ""));
            }
            Table tblCasingLinerTube = getCasingLinerTubing(dicLstCltData, _tabheader); ;
            increment = 0;
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
                            var wrkStrWeight = objInputData.WorkStringItems.Where(wks => wks.wrkSectionName.Equals(bhawrkStrlstitem.SectionName.ToString()) && wks.wrkLength.Equals(bhawrkStrlstitem.LengthInFeet.ToString()))
                                                    .Select(wks => wks.wrkWeight);
                            var wrkStrUpConnTyp = objInputData.WorkStringItems.Where(wks => wks.wrkSectionName.Equals(bhawrkStrlstitem.SectionName.ToString()) && wks.wrkLength.Equals(bhawrkStrlstitem.LengthInFeet.ToString()))
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
                            var bhatoolWeight = objInputData.BHAToolItemData.Where(bt => bt.SerialNumber.Equals(bhawrkStrlstitem.PositionNumber.ToString()) && bt.Length.Equals(bhawrkStrlstitem.LengthInFeet.ToString()))
                                                .Select(bt => bt.Weight);
                            var bhatoolUpConnTyp = objInputData.BHAToolItemData.Where(bt => bt.SerialNumber.Equals(bhawrkStrlstitem.PositionNumber.ToString()) && bt.Length.Equals(bhawrkStrlstitem.LengthInFeet.ToString()))
                                                .Select(bt => bt.UpperConnType);
                            var bhatoolLowConntyp = objInputData.BHAToolItemData.Where(bt => bt.SerialNumber.Equals(bhawrkStrlstitem.PositionNumber.ToString()) && bt.Length.Equals(bhawrkStrlstitem.LengthInFeet.ToString()))
                                                                    .Select(bt => bt.LowerConnType);
                            var bhatoolFishNeckOD = objInputData.BHAToolItemData.Where(bt => bt.SerialNumber.Equals(bhawrkStrlstitem.PositionNumber.ToString()) && bt.Length.Equals(bhawrkStrlstitem.LengthInFeet.ToString()))
                                                                    .Select(bt => bt.FishNeckOD);
                            var bhatoolFishNeckLen = objInputData.BHAToolItemData.Where(bt => bt.SerialNumber.Equals(bhawrkStrlstitem.PositionNumber.ToString()) && bt.Length.Equals(bhawrkStrlstitem.LengthInFeet.ToString()))
                                                                     .Select(bt => bt.FishNeckLength);
                            dicLstBhaData.Add("Weight" + increment, bhatoolWeight != null ? bhatoolWeight.FirstOrDefault() : "0");
                            dicLstBhaData.Add("Length" + increment, bhawrkStrlstitem.LengthInFeet > 0 ? bhawrkStrlstitem.LengthInFeet.ToString() : "0");
                            dicLstBhaData.Add("UpperConnType" + increment, bhatoolUpConnTyp != null ? bhatoolUpConnTyp.FirstOrDefault() : "N/A");
                            dicLstBhaData.Add("LowerConnType" + increment, bhatoolLowConntyp != null ? bhatoolLowConntyp.FirstOrDefault() : "N/A");
                            dicLstBhaData.Add("FishNeckOD" + increment, bhatoolFishNeckOD != null ? bhatoolFishNeckOD.FirstOrDefault() : "");
                            dicLstBhaData.Add("FishNeckLength" + increment, bhatoolFishNeckLen != null ? bhatoolFishNeckLen.FirstOrDefault() : "0");
                            dicLstBhaData.Add("HydraulicOD" + increment, bhawrkStrlstitem.OutsideDiameterInInch > 0 ? bhawrkStrlstitem.OutsideDiameterInInch.ToString() : "0");
                            dicLstBhaData.Add("HydraulicID" + increment, bhawrkStrlstitem.OutsideDiameterInInch > 0 ? bhawrkStrlstitem.OutsideDiameterInInch.ToString() : "0");
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

            #region Hydraulic BHA Output
            List<HydraulicAnalysisAnnulusOutputData> objHyAnlyAnnuOutputData = new List<HydraulicAnalysisAnnulusOutputData>();

            foreach (var itemannulsoutputlst in objChartService.HydraulicOutputAnnulusList)
            {
                objHyAnlyAnnuOutputData.Add(new HydraulicAnalysisAnnulusOutputData
                {
                    AnnulusColor = Convert.ToString(itemannulsoutputlst.AnnulusColor),
                    Annulus = itemannulsoutputlst.Annulus,
                    WorkString = itemannulsoutputlst.Workstring,
                    From = itemannulsoutputlst.FromAnnulus,
                    To = itemannulsoutputlst.ToAnnulus,
                    AverageVelocity = itemannulsoutputlst.AverageVelocity,
                    AvgVelocityColor = Convert.ToString(itemannulsoutputlst.AverageVelocityColor),
                    CriticalVelocity = itemannulsoutputlst.CriticalVelocity,
                    Flow = itemannulsoutputlst.FlowType,
                    ChipRate = itemannulsoutputlst.ChipRate,
                    ChipRateColor = Convert.ToString(itemannulsoutputlst.ChipRateColor),
                    PressureDrop = itemannulsoutputlst.AnnulusPressureDrop
                });
            }

            Table tblannulusdata = getAnnulusTableData(objHyAnlyAnnuOutputData);
           

            #endregion

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
                        NewMethod(img, newline, legend, ls, header, tableAuthor, comment, footer, _headerinfo, tblSegment, tblJobInformation, tblWellInformation, tblOriginator, tblCustomerContacts,
                            tblWeatherfordContacts, tblGenInfo, tblApproval, _casingLinerTubingInfo, tblDepthAnalysis, tblCasingLinerTube, tblBhaData, tblSurfaceEquipment, tblFluidEnvelope,
                            tblFluid, _chartheader, imgPie, tblannulusdata, _stdpipeHeader, imgStdPvsFlwRate, xscale,yscale, document);
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
        private static void NewMethod(Image img, Paragraph newline, Paragraph legend, LineSeparator ls, Paragraph header, Table tableAuthor, Paragraph comment, Table footer, Paragraph _headerinfo,
            Table tblSegment, Table tblJobInformation, Table tblWellInformation, Table tblOriginator, Table tblCustomerContacts, Table tblWeatherfordContacts, Table tblGenInfo, Table tblApproval,
            Paragraph _casingLinerTubingInfo, Table tblDepthAnalysis, Table tblCasingLinerTube, Table tblBhaData, Table tblSurfaceEquipment, Table tblFluidEnvelope, Table tblFluid, Paragraph _chartheader,
            Image imgPie, Table tblannulusdata,Paragraph _stdpipeHeader, Image imgStdPvsFlwRate, Paragraph xscale, Paragraph yscale, Document document)
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

            document.Add(imgPie);
            document.Add(newline);
            document.Add(tblannulusdata);
            tblannulusdata.Flush();
            tblannulusdata.Complete();
            document.Add(newline);
            document.Add(ls);
            document.Add(footer);

            document.Add(new AreaBreak(AreaBreakType.NEXT_PAGE));
            document.Add(img);
            document.Add(_casingLinerTubingInfo);
            document.Add(ls);
            document.Add(newline);
            document.Add(_stdpipeHeader);
            document.Add(newline);
            document.Add(xscale);
            document.Add(yscale);
            document.Add(imgStdPvsFlwRate);
            document.Add(newline);
            document.Add(ls);
            document.Add(footer);
        }

       #region Methods

        #region Graph and Chart Generate Method Section
        public byte[] GeneratePieChart(Dictionary<string, string> objPrsDrop, List<PieData> pieData)
        {
            float width = 400;
            float height = 400;
            float margin = 15;
            using (var surfcae = SKSurface.Create(new SKImageInfo((int)width, (int)height)))
            {
                var canvas = surfcae.Canvas;
                canvas.Clear(SKColors.White);
                float centerX = width / 2;
                float centerY = height / 2;
                float radius = Math.Min(width, height) / 2 - margin;

                float totalValue = 0;

                foreach (var itemval in objPrsDrop.Keys)
                {
                    if (itemval.Substring(0, 5) == "Value")
                    {
                        float itmValue = (float)Math.Round(float.Parse(objPrsDrop[itemval].ToString()));
                        totalValue += itmValue;
                    }
                }

                float startAngle = 0;
                float sweepAngle = 0;
                foreach (var lstitem in objPrsDrop.Keys)
                {
                    if (lstitem.Substring(0, 5) == "Value")
                    {
                        float itValue = (float)Math.Round(float.Parse(objPrsDrop[lstitem].ToString()));
                        sweepAngle = (itValue / totalValue) * 360;
                    }
                    if (lstitem.Substring(0, 5) == "Color")
                    {
                        using (var paint = new SKPaint())
                        {
                            string colourName = objPrsDrop[lstitem].ToString();

                            string hexString = ViewModel.ColorConverter.ColorNameToHexString(colourName);

                            paint.Color = SKColor.Parse(hexString);
                            canvas.DrawArc(new SKRect(50, 50, 350, 350), startAngle, sweepAngle, true, paint);
                            startAngle += sweepAngle;
                        }
                    }
                }

                float legendX = width - margin - 50;
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
                    canvas.DrawText($"({labelPercentage:F2} %)", legendX + legendItemHeight + 5, legendY + 15, new SKPaint());
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

        public Table getAnnulusTableData(List<HydraulicAnalysisAnnulusOutputData> objHydrAnnulus)
        {
            Table _tblannulusdata = new Table(10, true).SetFontSize(10);
            Cell tblhead01 = new Cell(1, 1).Add(new Paragraph().SetBackgroundColor(ColorConstants.LIGHT_GRAY));
            Cell tblhead02 = new Cell(1, 1).Add(new Paragraph("Annulus").SetBackgroundColor(ColorConstants.LIGHT_GRAY).SetTextAlignment(TextAlignment.CENTER));
            Cell tblhead03 = new Cell(1, 1).Add(new Paragraph("WorkString").SetBackgroundColor(ColorConstants.LIGHT_GRAY).SetTextAlignment(TextAlignment.CENTER));
            Cell tblhead04 = new Cell(1, 1).Add(new Paragraph("From (ft)").SetBackgroundColor(ColorConstants.LIGHT_GRAY).SetTextAlignment(TextAlignment.CENTER));
            Cell tblhead05 = new Cell(1, 1).Add(new Paragraph("To (ft)").SetBackgroundColor(ColorConstants.LIGHT_GRAY).SetTextAlignment(TextAlignment.CENTER));
            Cell tblhead06 = new Cell(1, 1).Add(new Paragraph("Average Velocity (ft/min)").SetBackgroundColor(ColorConstants.LIGHT_GRAY).SetTextAlignment(TextAlignment.CENTER));
            Cell tblhead07 = new Cell(1, 1).Add(new Paragraph("Critical Velocity (ft/min)").SetBackgroundColor(ColorConstants.LIGHT_GRAY).SetTextAlignment(TextAlignment.CENTER));
            Cell tblhead08 = new Cell(1, 1).Add(new Paragraph("Flow").SetBackgroundColor(ColorConstants.LIGHT_GRAY).SetTextAlignment(TextAlignment.CENTER));
            Cell tblhead09 = new Cell(1, 1).Add(new Paragraph("Chip Rate (ft/min)").SetBackgroundColor(ColorConstants.LIGHT_GRAY).SetTextAlignment(TextAlignment.CENTER));
            Cell tblhead10 = new Cell(1, 1).Add(new Paragraph("Pressure Drop (psi)").SetBackgroundColor(ColorConstants.LIGHT_GRAY).SetTextAlignment(TextAlignment.CENTER));

            _tblannulusdata.AddCell(tblhead01);
            _tblannulusdata.AddCell(tblhead02);
            _tblannulusdata.AddCell(tblhead03);
            _tblannulusdata.AddCell(tblhead04);
            _tblannulusdata.AddCell(tblhead05);
            _tblannulusdata.AddCell(tblhead06);
            _tblannulusdata.AddCell(tblhead07);
            _tblannulusdata.AddCell(tblhead08);
            _tblannulusdata.AddCell(tblhead09);
            _tblannulusdata.AddCell(tblhead10);

            foreach (var itmAnu in objHydrAnnulus)
            {
                if(itmAnu.AnnulusColor.ToUpper() != "GREEN")
                {
                    Cell clanColor = new Cell(1, 1).Add(new Paragraph(char.ConvertFromUtf32(0x2191) + "   " + char.ConvertFromUtf32(0x2191)).SetFontColor(ColorConstants.GREEN));
                    _tblannulusdata.AddCell(clanColor);
                }
                Cell clan02 = new Cell(1, 1).Add(new Paragraph(itmAnu.Annulus).SetTextAlignment(TextAlignment.LEFT));
                Cell clan03 = new Cell(1, 1).Add(new Paragraph(itmAnu.WorkString).SetTextAlignment(TextAlignment.LEFT));
                Cell clan04 = new Cell(1, 1).Add(new Paragraph(itmAnu.From.ToString()).SetTextAlignment(TextAlignment.LEFT));
                Cell clan05 = new Cell(1, 1).Add(new Paragraph(itmAnu.To.ToString()).SetTextAlignment(TextAlignment.LEFT));
                Cell clan06 = new Cell(1, 1).Add(new Paragraph(itmAnu.AverageVelocity.ToString()).SetTextAlignment(TextAlignment.LEFT));
                Cell clan07 = new Cell(1, 1).Add(new Paragraph(itmAnu.CriticalVelocity.ToString()).SetTextAlignment(TextAlignment.LEFT));
                Cell clan08 = new Cell(1, 1).Add(new Paragraph(itmAnu.ChipRate.ToString()).SetTextAlignment(TextAlignment.LEFT));
                Cell clan09 = new Cell(1, 1).Add(new Paragraph(itmAnu.PressureDrop.ToString()).SetTextAlignment(TextAlignment.LEFT));
                _tblannulusdata.AddCell(clan02);
                _tblannulusdata.AddCell(clan03);
                _tblannulusdata.AddCell(clan04);
                _tblannulusdata.AddCell(clan05);
                _tblannulusdata.AddCell(clan06);
                _tblannulusdata.AddCell(clan07);
                _tblannulusdata.AddCell(clan08);
                _tblannulusdata.AddCell(clan09);

            }
            
           


            return _tblannulusdata;
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
                    var xfixPoint = objCags.UpperOperatingPoint;
                    var yfixPoint = objCags.MaxPressure;

                    float xpoint = margin;
                    for (int i=1; i <= xlimit; i++)
                    {
                        int x = i * 100;
                        string xScale = Convert.ToString(x);
                        xpoint += xpoint + 50;
                        canvas.DrawText(xScale, new SKPoint(xpoint, height + 15 - margin), paint);
                    }
                    float ypoint = height + 10 - margin;
                    for(int j = 1; j <= ylimit; j++)
                    {
                        ypoint -= 50;
                        int y = j * 1000;
                        string yScale = Convert.ToString(y);
                        canvas.DrawText(yScale,new SKPoint(0,ypoint),paint);
                    }
                    canvas.DrawText("X",(float)xfixPoint, (float)yfixPoint, paint);
                }

                float minX = (float)objCags.MinimumFlowRate;
                float maxX = (float)objCags.MaxFlowRate;
                float minY = 0;
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
            _tableSurf.SetFontSize(10);
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

        #region GetHdraPro Series graph per tool
        public void  getHydraulicBHAGraphs()
        {
        } 
        #endregion
       #endregion
    }

}

