using System;
using HydraulicCalAPI.Service;

using iText.Kernel.Colors;
using iText.Layout.Element;
using iText.Layout.Properties;

namespace HydraulicCalAPI.ViewModel
{
    public class PgFluidSurface
    {
        Color lgtGrey = new DeviceRgb(217, 217, 217);
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
        public Table GetSurfaceEquipDataTable(PdfReportService objInputData, string header)
        {
            Table _tableSurfaceEquipment;
            return _tableSurfaceEquipment = getSurfaceEquipDataTable(objInputData, header);
        }
        private Table getSurfaceEquipDataTable(PdfReportService _objInputData, string tblHeader)
        {
            try
            {
                Table _tableSeg = new Table(5, true);
                _tableSeg.SetFontSize(9);

                string _caseType = _objInputData.HydraCalcService.surfaceEquipmentInput.CaseType.ToString();
                double _totLength = getSurfaceEquipmentTotalLength(_caseType);

                string tabheadertext = tblHeader;
                string charFt = "ft";
                if (_objInputData.UOM.DepthName.ToUpper() != "FT")
                {
                    charFt = _objInputData.UOM.DepthName.ToString();
                }

                Cell sn = new Cell(1, 5).Add(new Paragraph(tabheadertext)).SetTextAlignment(TextAlignment.LEFT).SetBackgroundColor(lgtGrey).SetBold();
                _tableSeg.AddHeaderCell(sn);

                Cell c1 = new Cell(1, 1).Add(new Paragraph("Surface Equipment")).SetWidth(20).SetTextAlignment(TextAlignment.LEFT).SetBold();
                Cell c2 = new Cell(1, 1).Add(new Paragraph(_caseType)).SetWidth(10).SetTextAlignment(TextAlignment.LEFT).SetBold();
                Cell c3 = new Cell(1, 1).Add(new Paragraph("Total Length")).SetWidth(20).SetTextAlignment(TextAlignment.LEFT).SetBold();
                Cell c4 = new Cell(1, 1);
                if (_totLength > 0)
                {
                    _totLength = Math.Round(_totLength * _objInputData.UOM.DensityMultiplier,3);
                    c4 = new Cell(1, 1).Add(new Paragraph(_totLength.ToString("F3"))).SetWidth(30).SetTextAlignment(TextAlignment.LEFT).SetBold();
                }
                Cell surfEquipUom = new Cell(1, 1).Add(new Paragraph(" " + charFt)).SetWidth(15).SetTextAlignment(TextAlignment.LEFT);

                _tableSeg.AddCell(c1);
                _tableSeg.AddCell(c2);
                _tableSeg.AddCell(c3);
                _tableSeg.AddCell(c4);
                _tableSeg.AddCell(surfEquipUom);

                return _tableSeg.SetAutoLayout();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public Table GetFluidEnvelopeInfo(PdfReportService objInputData, string header)
        {
            Table _tableFluidEnvelop;
            return _tableFluidEnvelop = getFluidEnvelopeInfo(objInputData, header);
        }
        private Table getFluidEnvelopeInfo(PdfReportService objUOM, string fluidenvheader)
        {
            try
            {
                string _tblheadText = fluidenvheader;
                string charPsi = "psi";
                string charGal = "gal/min";
                if (objUOM.UOM.PressureName.ToUpper() != "PSI")
                {
                    charPsi = objUOM.UOM.PressureName.ToString();
                }
                else if (objUOM.UOM.FlowRateName.ToUpper() != "GAL/MIN")
                {
                    charGal = objUOM.UOM.FlowRateName.ToString();
                }
                else { }

                Table _tabFluidEnvelope = new Table(3, false);
                _tabFluidEnvelope.SetFontSize(9);

                Cell da = new Cell(1, 3).Add(new Paragraph(_tblheadText)).SetTextAlignment(TextAlignment.LEFT).SetBold().SetBackgroundColor(lgtGrey);
                _tabFluidEnvelope.AddHeaderCell(da);

                double _maxPressure = objUOM.HydraCalcService.maxflowpressure > 0 ? objUOM.HydraCalcService.maxflowpressure : 0;
                if(_maxPressure > 0)
                {
                    _maxPressure =  Math.Round((_maxPressure * objUOM.UOM.PressureMultiplier),3);
                }
                double _maxFlowRate = objUOM.HydraCalcService.maxflowrate > 0 ? objUOM.HydraCalcService.maxflowrate : 0;
                if (_maxFlowRate > 0)
                {
                    _maxFlowRate = Math.Round((_maxFlowRate * objUOM.UOM.FlowRateMultiplier),3);
                }
                string _comments = objUOM.Comments != null ? objUOM.Comments : "";

                // Building Table
                Cell c1 = new Cell(1, 1).Add(new Paragraph("Maximum Allowable Pressure")).SetTextAlignment(TextAlignment.LEFT).SetBold().SetWidth(150);
                Cell c2 = new Cell(1, 1).Add(new Paragraph(_maxPressure > 0 ? _maxPressure.ToString("F3") : "")).SetTextAlignment(TextAlignment.LEFT).SetWidth(100);
                Cell c3 = new Cell(1, 1).Add(new Paragraph(" " + charPsi)).SetTextAlignment(TextAlignment.LEFT).SetWidth(50);
                _tabFluidEnvelope.AddCell(c1);
                _tabFluidEnvelope.AddCell(c2);
                _tabFluidEnvelope.AddCell(c3);

                c1 = new Cell(1, 1).Add(new Paragraph("Maximum Allowable Flowrate")).SetTextAlignment(TextAlignment.LEFT).SetBold().SetWidth(150);
                c2 = new Cell(1, 1).Add(new Paragraph(_maxFlowRate > 0 ?_maxFlowRate.ToString("F3") : "")).SetTextAlignment(TextAlignment.LEFT).SetWidth(100);
                c3 = new Cell(1, 1).Add(new Paragraph(" " + charGal)).SetTextAlignment(TextAlignment.LEFT).SetWidth(50);
                _tabFluidEnvelope.AddCell(c1);
                _tabFluidEnvelope.AddCell(c2);
                _tabFluidEnvelope.AddCell(c3);

                Cell fecomment = new Cell(1, 1).Add(new Paragraph("Comments")).SetTextAlignment(TextAlignment.LEFT).SetBold().SetWidth(100);
                Cell fecomm = new Cell(1, 2).Add(new Paragraph(_comments)).SetTextAlignment(TextAlignment.LEFT).SetWidth(100);
                _tabFluidEnvelope.AddCell(fecomment);
                _tabFluidEnvelope.AddCell(fecomm);

                return _tabFluidEnvelope.SetAutoLayout();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public Table GetFluidInfo(PdfReportService objInputData, string header)
        {
            Table _tableFluidInfo;
            return _tableFluidInfo = getFluidInfo(objInputData, header);
        }
        private Table getFluidInfo(PdfReportService objUOM, string tabHeader)
        {
            try
            {
                string tabheadertext = tabHeader;

                string charIn = "in";
                string charDens = "lb/gal";
                string charCenti = "centipoise";
                string charYld = "lbf/100ft²";

                if (objUOM.UOM.SizeName.ToUpper() != "IN")
                {
                    charIn = objUOM.UOM.SizeName.ToString();
                }
                else if (objUOM.UOM.YieldPointName.ToUpper() != "LBF/100FT^2")
                {
                    charYld = objUOM.UOM.YieldPointName.ToString();
                }
                if (objUOM.UOM.DensityName.ToUpper() != "LB/GAL")
                {
                    charDens = objUOM.UOM.DensityName.ToString();
                }
                else if (objUOM.UOM.PlasticViscosityName.ToUpper() != "CENTIPOISE")
                {
                    charCenti = objUOM.UOM.PlasticViscosityName.ToString();
                }
                else { }

                string strSolids = objUOM.FluidItemData[0].Solids.ToString();
                string strFluidType = objUOM.FluidItemData[0].DrillingFluidType.ToString();
                double dblFluidWeight = objUOM.HydraCalcService.fluidInput.DensityInPoundPerGallon;
                double dblBuoyancy = objUOM.FluidItemData[0].BuoyancyFactor;
                double dblPlasticViscocity = objUOM.HydraCalcService.fluidInput.PlasticViscosityInCentiPoise;
                double dblYieldPoints = objUOM.HydraCalcService.fluidInput.YieldPointInPoundPerFeetSquare;
                double dblAvgCuttingSize = objUOM.HydraCalcService.cuttingsInput.AverageCuttingSizeInInch;
                string strCuttingType = objUOM.HydraCalcService.cuttingsInput.CuttingsType.ToString();

                Table _tableFluidInfo = new Table(6, true);
                _tableFluidInfo.SetFontSize(9);
                Cell sn = new Cell(1, 6).Add(new Paragraph(tabheadertext)).SetTextAlignment(TextAlignment.LEFT).SetBold().SetBackgroundColor(lgtGrey);
                _tableFluidInfo.AddHeaderCell(sn);

                Cell cSolid = new Cell(1, 1).Add(new Paragraph("Solids")).SetTextAlignment(TextAlignment.LEFT).SetBold();
                Cell cSolidValue = new Cell(1, 1).Add(new Paragraph(strSolids)).SetTextAlignment(TextAlignment.LEFT);
                Cell celSolidUom = new Cell(1, 1).Add(new Paragraph(" % "));
                Cell cFluidType = new Cell(1, 1).Add(new Paragraph("Drilling Fluid Type")).SetTextAlignment(TextAlignment.LEFT).SetBold();
                Cell cFludTypValue = new Cell(1, 2).Add(new Paragraph(strFluidType!= null ? strFluidType.ToString() : "")).SetTextAlignment(TextAlignment.LEFT);
                Cell cFluidWgt = new Cell(1, 1).Add(new Paragraph("Drilling Fluid Weight")).SetTextAlignment(TextAlignment.LEFT).SetBold();
                if (dblFluidWeight > 0)
                {
                    dblFluidWeight = dblFluidWeight * objUOM.UOM.DensityMultiplier;
                }
                Cell cFludWgtValue = new Cell(1, 1).Add(new Paragraph(dblFluidWeight > 0 ? dblFluidWeight.ToString("F3") : "")).SetTextAlignment(TextAlignment.LEFT);
                Cell celFludWgtUom = new Cell(1, 1).Add(new Paragraph(" " + charDens));
                Cell cBuoyancy = new Cell(1, 1).Add(new Paragraph("Buoyancy Factor")).SetTextAlignment(TextAlignment.LEFT).SetBold();
                if (dblBuoyancy > 0)
                {
                    dblBuoyancy = dblBuoyancy * objUOM.UOM.DensityMultiplier;
                }
                Cell cBuoyValue = new Cell(1, 1).Add(new Paragraph(dblBuoyancy > 0 ? dblBuoyancy.ToString("F3") : "")).SetTextAlignment(TextAlignment.LEFT);
                Cell celBuoyUom = new Cell(1, 1).Add(new Paragraph(" " + charDens));
                Cell cPlastic = new Cell(1, 1).Add(new Paragraph("Plastic Viscosity")).SetTextAlignment(TextAlignment.LEFT).SetBold();
                if (dblPlasticViscocity > 0)
                {
                    dblPlasticViscocity = dblPlasticViscocity * objUOM.UOM.PlasticViscosityMultiplier;
                }
                Cell cPlasticValue = new Cell(1, 1).Add(new Paragraph(dblPlasticViscocity > 0 ? dblPlasticViscocity.ToString("F3") : "")).SetTextAlignment(TextAlignment.LEFT);
                Cell celPlasticUom = new Cell(1, 1).Add(new Paragraph(" " + charCenti));
                Cell cYieldPoints = new Cell(1, 1).Add(new Paragraph("Yield Point")).SetTextAlignment(TextAlignment.LEFT).SetBold();
                if (dblYieldPoints > 0)
                {
                    dblYieldPoints = dblYieldPoints * objUOM.UOM.YieldPointMultiplier;
                }
                Cell cYldPtValue = new Cell(1, 1).Add(new Paragraph(dblYieldPoints > 0 ? dblYieldPoints.ToString("F3") : "")).SetTextAlignment(TextAlignment.LEFT);
                Cell celYldPtUom = new Cell(1, 1).Add(new Paragraph(" " + charYld));
                Cell cCutAvgSize = new Cell(1, 1).Add(new Paragraph("Cutting Average Size")).SetTextAlignment(TextAlignment.LEFT).SetBold();
                if (dblAvgCuttingSize > 0)
                {
                    dblAvgCuttingSize = dblAvgCuttingSize * objUOM.UOM.SizeMultiplier;
                }
                Cell cCutAvgSizeValue = new Cell(1, 1).Add(new Paragraph(dblAvgCuttingSize > 0 ? dblAvgCuttingSize.ToString("F3") : "")).SetTextAlignment(TextAlignment.LEFT);
                Cell celCutAvgSizeUom = new Cell(1, 1).Add(new Paragraph(" " + charIn));
                Cell cCutType = new Cell(1, 1).Add(new Paragraph("Cutting Type")).SetTextAlignment(TextAlignment.LEFT).SetBold();
                Cell cCutTypValue = new Cell(1, 2).Add(new Paragraph(strCuttingType !=null? strCuttingType.ToString() : "")).SetTextAlignment(TextAlignment.LEFT);

                //Add data to table
                _tableFluidInfo.AddCell(cSolid);
                _tableFluidInfo.AddCell(cSolidValue);
                _tableFluidInfo.AddCell(celSolidUom);
                _tableFluidInfo.AddCell(cFluidType);
                _tableFluidInfo.AddCell(cFludTypValue);
                _tableFluidInfo.AddCell(cFluidWgt);
                _tableFluidInfo.AddCell(cFludWgtValue);
                _tableFluidInfo.AddCell(celFludWgtUom);
                _tableFluidInfo.AddCell(cBuoyancy);
                _tableFluidInfo.AddCell(cBuoyValue);
                _tableFluidInfo.AddCell(celBuoyUom);
                _tableFluidInfo.AddCell(cPlastic);
                _tableFluidInfo.AddCell(cPlasticValue); 
                _tableFluidInfo.AddCell(celPlasticUom);
                _tableFluidInfo.AddCell(cYieldPoints);
                _tableFluidInfo.AddCell(cYldPtValue);
                _tableFluidInfo.AddCell(celYldPtUom);
                _tableFluidInfo.AddCell(cCutAvgSize);
                _tableFluidInfo.AddCell(cCutAvgSizeValue);
                _tableFluidInfo.AddCell(celCutAvgSizeUom);
                _tableFluidInfo.AddCell(cCutType);
                _tableFluidInfo.AddCell(cCutTypValue);

                return _tableFluidInfo.SetAutoLayout();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
