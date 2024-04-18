using System;
using System.Linq;
using System.Collections.Generic;
using HydraulicCalAPI.Service;

using iText.Kernel.Colors;
using iText.Layout.Element;
using iText.Layout.Properties;

namespace HydraulicCalAPI.ViewModel
{
    public class CasingLinerTubingData
    {
        public string WellBoreSectionName { get; set; }
        public double AnnulusODInInch { get; set; }
        public double AnnulusIDInInch { get; set; }
        public double WellBoreWeight { get; set; }
        public string Grade { get; set; }
        public double AnnulusTopInFeet { get; set; }
        public double AnnulusBottomInFeet { get; set; }
        public double AnnlusLengthInFeet { get; set; }
    }
    public class PgCasingLinerTubing
    {
        List<string> pdfCasingData = new List<string>();
        Color lgtGrey = new DeviceRgb(217, 217, 217);
        public Table GetDepthAnalysis(PdfReportService objInputData, ChartAndGraphService objChartService, string header)
        {
            Table _tableDpthAnalysis;
            return _tableDpthAnalysis = getDepthAnalysis(objInputData, objChartService, header);
        }
        private Table getDepthAnalysis(PdfReportService objUOM, ChartAndGraphService _objChartService, string tablehead)
        {
            try
            {
                //Code to get Annulus Length and BhaTool Length
                double annulusLength = 0.00;
                double bhatoolLength = 0.00;
                double toolLength = 0.00;
                int count = 3;
                string _tblheadText = tablehead;
                string charFt = "ft";
                double _length;

                Table _tabDepth = new Table(3, false);
                _tabDepth.SetFontSize(10);

                annulusLength = objUOM.HydraCalcService.annulusInput.Select(x => x.AnnulusBottomInFeet).LastOrDefault();
                
                foreach (var bhaTitem in objUOM.HydraCalcService.bhaInput)
                {
                    if (bhaTitem.bhatooltype.ToString().ToUpper() != "WRKSTR")
                        bhatoolLength += bhaTitem.LengthInFeet;
                }

                toolLength = _objChartService.HydraulicOutputAnnulusList.Select(y => y.ToAnnulus).LastOrDefault();
                
                pdfCasingData.Add(annulusLength > 0 ? (Math.Round(annulusLength, 3).ToString()) : "");
                pdfCasingData.Add(bhatoolLength > 0 ? (Math.Round(bhatoolLength, 3).ToString()) : "");
                pdfCasingData.Add(toolLength > 0 ? (Math.Round(toolLength, 3).ToString()) : "");

                if (objUOM.UOM.DepthName.ToUpper() != "FT")
                {
                    charFt = objUOM.UOM.DepthName.ToString();
                }

                // Adding Data to cell
                Cell da = new Cell(1, 3).Add(new Paragraph(_tblheadText)).SetTextAlignment(TextAlignment.LEFT).SetBold().SetBackgroundColor(lgtGrey).SetWidth(200);
                _tabDepth.AddHeaderCell(da);

                for (int i = 1; i <= count; i++)
                {
                    switch (i)
                    {
                        case 1:
                            {
                                _length = 0.00;
                                Cell daAnnulusLength = new Cell(1, 1).Add(new Paragraph("Annulus Length")).SetTextAlignment(TextAlignment.LEFT).SetBold().SetWidth(100);
                                if (pdfCasingData[0] != "")
                                {
                                    _length = double.Parse(pdfCasingData[0]) * objUOM.UOM.DepthMultiplier;
                                }
                                Cell daAnLen = new Cell(1, 1).Add(new Paragraph(Math.Round(_length, 3).ToString())).SetTextAlignment(TextAlignment.LEFT).SetWidth(100);
                                _tabDepth.AddCell(daAnnulusLength);
                                _tabDepth.AddCell(daAnLen);
                                break;
                            }
                        case 2:
                            {
                                _length = 0.00;
                                Cell daBHALength = new Cell(1, 1).Add(new Paragraph("BHA Length")).SetTextAlignment(TextAlignment.LEFT).SetBold().SetWidth(100);
                                if (pdfCasingData[1] != "")
                                {
                                    _length = double.Parse(pdfCasingData[1]) * objUOM.UOM.DepthMultiplier;
                                }
                                Cell daBhaLen = new Cell(1, 1).Add(new Paragraph(Math.Round(_length, 3).ToString())).SetTextAlignment(TextAlignment.LEFT).SetWidth(100);
                                _tabDepth.AddCell(daBHALength);
                                _tabDepth.AddCell(daBhaLen);
                                break;
                            }
                        case 3:
                            {
                                _length = 0.00;
                                Cell daToolLength = new Cell(1, 1).Add(new Paragraph("Tool Depth")).SetTextAlignment(TextAlignment.LEFT).SetBold().SetWidth(100);
                                if (pdfCasingData[2] != "")
                                {
                                    _length = double.Parse(pdfCasingData[2]) * objUOM.UOM.DepthMultiplier;
                                }
                                Cell daTulDpth = new Cell(1, 1).Add(new Paragraph(Math.Round(_length, 3).ToString())).SetTextAlignment(TextAlignment.LEFT).SetWidth(100);
                                _tabDepth.AddCell(daToolLength);
                                _tabDepth.AddCell(daTulDpth);
                                break;
                            }
                        default:
                            break;
                    }
                    Cell celdaUom = new Cell(1, 1).Add(new Paragraph(" " + charFt)).SetTextAlignment(TextAlignment.LEFT).SetBold().SetWidth(100);
                    _tabDepth.AddCell(celdaUom);
                }

                return _tabDepth.SetAutoLayout();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public Table GetCasingLinerTubing(PdfReportService objInputData, HydraulicCalculationService objHydraulicCalculationService, string header)
        {
            Table _tblCasing;
            return _tblCasing = getCasingLinerTubing(objInputData, objHydraulicCalculationService, header);
        }
        private Table getCasingLinerTubing(PdfReportService objUOM, HydraulicCalculationService _objHydCalSrvs, string tablehead)
        {
            int counter = 0;
            string _tblcltheader = tablehead;
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

            try
            {
                Table _tabclt = new Table(8, true);
                _tabclt.SetFontSize(10);

                List<CasingLinerTubingData> dataCLT = new List<CasingLinerTubingData>();
                foreach (var itemCLT in _objHydCalSrvs.annulusInput)
                {
                    dataCLT.Add(new CasingLinerTubingData
                    {
                        WellBoreSectionName = itemCLT.WellboreSectionName,
                        WellBoreWeight = GetWellBoreWeight(objUOM.CasingLinerTubeData[counter].WellBoreWeight),
                        AnnulusODInInch = itemCLT.AnnulusODInInch,
                        AnnulusIDInInch = itemCLT.AnnulusIDInInch,
                        Grade = objUOM.CasingLinerTubeData[counter].Grade,
                        AnnulusTopInFeet = itemCLT.AnnulusTopInFeet,
                        AnnulusBottomInFeet = itemCLT.AnnulusBottomInFeet,
                        AnnlusLengthInFeet = itemCLT.AnnulusLengthInFeet
                    });
                    counter++;
                }

                // Creating Table to dispaly data in Report

                counter = 1;
                Cell cltheadcell = new Cell(1, 8).Add(new Paragraph(_tblcltheader)).SetTextAlignment(TextAlignment.LEFT).SetBold().SetBackgroundColor(lgtGrey);
                _tabclt.AddHeaderCell(cltheadcell);

                Cell cltid = new Cell(1, 1).SetTextAlignment(TextAlignment.CENTER).Add(new Paragraph("#")).SetBold().SetBackgroundColor(lgtGrey);
                Cell cltwellsection = new Cell(1, 1).SetTextAlignment(TextAlignment.CENTER).Add(new Paragraph("Wellbore Section")).SetBold().SetBackgroundColor(lgtGrey);
                Cell cltoutdia = new Cell(1, 1).SetTextAlignment(TextAlignment.CENTER).Add(new Paragraph("OD (" + charIn + ")")).SetBold().SetBackgroundColor(lgtGrey);
                Cell cltindia = new Cell(1, 1).SetTextAlignment(TextAlignment.CENTER).Add(new Paragraph("ID (" + charIn + ")")).SetBold().SetBackgroundColor(lgtGrey);
                Cell cltweight = new Cell(1, 1).SetTextAlignment(TextAlignment.CENTER).Add(new Paragraph("Weight (" + charLbs + "/" + charFt + ")")).SetBold().SetBackgroundColor(lgtGrey);
                Cell cltgrade = new Cell(1, 1).SetTextAlignment(TextAlignment.CENTER).Add(new Paragraph("Grade")).SetBold().SetBackgroundColor(lgtGrey);
                Cell clttop = new Cell(1, 1).SetTextAlignment(TextAlignment.CENTER).Add(new Paragraph("Top Depth (" + charFt + ")")).SetBold().SetBackgroundColor(lgtGrey);
                Cell cltbottom = new Cell(1, 1).SetTextAlignment(TextAlignment.CENTER).Add(new Paragraph("Bottom Depth (" + charFt + ")")).SetBold().SetBackgroundColor(lgtGrey);

                _tabclt.AddCell(cltid);
                _tabclt.AddCell(cltwellsection);
                _tabclt.AddCell(cltoutdia);
                _tabclt.AddCell(cltindia);
                _tabclt.AddCell(cltweight);
                _tabclt.AddCell(cltgrade);
                _tabclt.AddCell(clttop);
                _tabclt.AddCell(cltbottom);

                foreach (var itmClltData in dataCLT)
                {
                    double newUom = 0.00;
                    cltid = new Cell(1, 1).Add(new Paragraph(Convert.ToString(counter++))).SetTextAlignment(TextAlignment.CENTER);

                    if (itmClltData.WellBoreSectionName != null)
                        cltwellsection = new Cell(1, 1).Add(new Paragraph(itmClltData.WellBoreSectionName)).SetTextAlignment(TextAlignment.CENTER);
                    if (itmClltData.AnnulusODInInch > 0)
                    {
                        newUom = Math.Round(itmClltData.AnnulusODInInch * objUOM.UOM.SizeMultiplier, 3);
                        cltoutdia = new Cell(1, 1).Add(new Paragraph(newUom.ToString())).SetTextAlignment(TextAlignment.LEFT);
                    }
                    else
                        cltoutdia = new Cell(1, 1).Add(new Paragraph("")).SetTextAlignment(TextAlignment.LEFT);
                    if (itmClltData.AnnulusIDInInch > 0)
                    {
                        newUom = Math.Round(itmClltData.AnnulusIDInInch * objUOM.UOM.SizeMultiplier, 3);
                        cltindia = new Cell(1, 1).Add(new Paragraph(newUom.ToString())).SetTextAlignment(TextAlignment.LEFT);
                    }
                    else
                        cltindia = new Cell(1, 1).Add(new Paragraph("")).SetTextAlignment(TextAlignment.LEFT);
                    if (itmClltData.WellBoreWeight > 0)
                    {
                        newUom = Math.Round(itmClltData.WellBoreWeight * objUOM.UOM.WeightMultiplier, 3);
                        cltweight = new Cell(1, 1).Add(new Paragraph(newUom.ToString())).SetTextAlignment(TextAlignment.LEFT);
                    }
                    else
                        cltweight = new Cell(1, 1).Add(new Paragraph("")).SetTextAlignment(TextAlignment.LEFT);
                    if (itmClltData.Grade != null)
                        cltgrade = new Cell(1, 1).Add(new Paragraph(itmClltData.Grade)).SetTextAlignment(TextAlignment.CENTER);
                    else
                        cltgrade = new Cell(1, 1).Add(new Paragraph("")).SetTextAlignment(TextAlignment.LEFT);
                    if (itmClltData.AnnulusTopInFeet >= 0)
                    {
                        newUom = Math.Round(itmClltData.AnnulusTopInFeet * objUOM.UOM.SizeMultiplier, 3);
                        clttop = new Cell(1, 1).Add(new Paragraph(newUom.ToString())).SetTextAlignment(TextAlignment.LEFT);
                    }
                    else
                        clttop = new Cell(1, 1).Add(new Paragraph("")).SetTextAlignment(TextAlignment.LEFT);
                    if (itmClltData.AnnulusBottomInFeet > 0)
                    {
                        newUom = Math.Round(itmClltData.AnnulusBottomInFeet * objUOM.UOM.SizeMultiplier, 3);
                        cltbottom = new Cell(1, 1).Add(new Paragraph(newUom.ToString())).SetTextAlignment(TextAlignment.LEFT);
                    }
                    else
                        cltbottom = new Cell(1, 1).Add(new Paragraph("")).SetTextAlignment(TextAlignment.LEFT);

                    //Add data to Table
                    _tabclt.AddCell(cltid);
                    _tabclt.AddCell(cltwellsection);
                    _tabclt.AddCell(cltoutdia);
                    _tabclt.AddCell(cltindia);
                    _tabclt.AddCell(cltweight);
                    _tabclt.AddCell(cltgrade);
                    _tabclt.AddCell(clttop);
                    _tabclt.AddCell(cltbottom);
                }
                return _tabclt.SetAutoLayout();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        private double GetWellBoreWeight(string objWellBoreWeight)
        {
            try
            {
                if (!string.IsNullOrEmpty(objWellBoreWeight) || objWellBoreWeight.ToUpper() != "NA" || objWellBoreWeight.ToUpper() != "NULL")
                    return double.Parse(objWellBoreWeight);
                else
                    return 0;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
    }
}