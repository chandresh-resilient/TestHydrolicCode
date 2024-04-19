using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using HydraulicCalAPI.Service;

using iText.Kernel.Colors;
using iText.Layout.Element;
using iText.Layout.Properties;

namespace HydraulicCalAPI.ViewModel
{
    public class BhaToolData
    {
        public int PositionNo { get; set; }
        public string ToolDescription { get; set; }
        public string SerialNo { get; set; }
        public double ToolOuterDia { get; set; }
        public double ToolInnerDia { get; set; }
        public double ToolWeight { get; set; }
        public double ToolLength { get; set; }
        public string  ToolUpperConnType { get; set; }
        public string ToolLowerConnType { get; set; }
        public double ToolFishNeckOD { get; set; }
        public double ToolFishNeckLength { get; set; }
        public double ToolHydraulicOD { get; set; }
        public double ToolHydraulicID { get; set; }
    }
    public class PgBottomHoleAssembly
    {
        Color lgtGrey = new DeviceRgb(217, 217, 217);
        List<BhaToolData> objBhaToolData = new List<BhaToolData>();
        int wksCounter = 0;
        int bhaCounter = 0;
        public Table GetBha(PdfReportService objInputData,string header)
        {
            Table _tablebha;
            return _tablebha = getBha(objInputData, header);
        }
        private Table getBha(PdfReportService objInputData, string bhaheadtext)
        {
            try
            {
                string charFt = "ft";
                string charIn = "in";
                string charLbs = "lbs";
                if (objInputData.UOM.SizeName.ToUpper() != "IN")
                {
                    charIn = objInputData.UOM.SizeName.ToString();
                }
                else if (objInputData.UOM.WeightName.ToUpper() != "LBS")
                {
                    charLbs = objInputData.UOM.WeightName.ToString();
                }
                else if (objInputData.UOM.DepthName.ToUpper() != "FT")
                {
                    charFt = objInputData.UOM.DepthName.ToString();
                }
                else { }
                
                foreach (var itmWrkString in objInputData.HydraCalcService.bhaInput)
                {
                    string tooltype = itmWrkString.bhatooltype.ToString().ToUpper();
                    switch (tooltype)
                    {
                        case "WRKSTR":
                            {
                                objBhaToolData.Add(new BhaToolData
                                {
                                    PositionNo = itmWrkString.PositionNumber,
                                    ToolDescription = itmWrkString.SectionName,
                                    SerialNo = "",
                                    ToolOuterDia = itmWrkString.OutsideDiameterInInch,
                                    ToolInnerDia = itmWrkString.InsideDiameterInInch,
                                    ToolWeight = GetWorkStringWeight(objInputData, wksCounter),
                                    ToolLength = itmWrkString.LengthInFeet,
                                    ToolUpperConnType = GetWorkStringUpperConn(objInputData, wksCounter),
                                    ToolLowerConnType = "",
                                    ToolFishNeckOD = 0,
                                    ToolFishNeckLength = 0,
                                    ToolHydraulicOD = 0,
                                    ToolHydraulicID = 0
                                });
                                wksCounter++;
                                break;
                            }
                        default:
                            {
                               objBhaToolData.Add(new BhaToolData
                                {
                                    PositionNo = itmWrkString.PositionNumber,
                                    ToolDescription = itmWrkString.SectionName,
                                    SerialNo = GetSerialNo(objInputData, bhaCounter),
                                    ToolOuterDia = itmWrkString.OutsideDiameterInInch,
                                    ToolInnerDia = itmWrkString.InsideDiameterInInch,
                                    ToolWeight = GetToolWeight(objInputData, bhaCounter),
                                    ToolLength = itmWrkString.LengthInFeet,
                                    ToolUpperConnType = GetToolUpperConn(objInputData, bhaCounter),
                                    ToolLowerConnType = GetToolLowerConn(objInputData, bhaCounter),
                                    ToolFishNeckOD = GetToolFishNeckOD(objInputData, bhaCounter),
                                    ToolFishNeckLength = GetToolFishNeckLength(objInputData, bhaCounter),
                                    ToolHydraulicOD = GetToolHydraulicOD(objInputData, bhaCounter),
                                    ToolHydraulicID = GetToolHydraulicID(objInputData, bhaCounter),
                                });
                                bhaCounter++;
                                break;
                            }
                    }
                }
              
                string _tblbhaheader = bhaheadtext;

                Table _tblBHA = new Table(13, true);
                _tblBHA.SetFontSize(9);
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

                foreach (var item in objBhaToolData)
                {
                    double outdia = 0.00;
                    double inndia = 0.00;
                    double toolwt = 0.00;
                    double toollen = 0.00;
                    double fishod = 0.00;
                    double fishlen = 0.00;
                    double hydod = 0.00;
                    double hydid = 0.00;
                    
                    bhaid = new Cell(1, 1).Add(new Paragraph(item.PositionNo > 0 ? item.PositionNo.ToString() : "")).SetTextAlignment(TextAlignment.CENTER);
                    _tblBHA.AddCell(bhaid);
                    bhatooldesc = new Cell(1, 1).Add(new Paragraph(item.ToolDescription != null ? item.ToolDescription.ToString() : "")).SetTextAlignment(TextAlignment.LEFT);
                    _tblBHA.AddCell(bhatooldesc);
                    bhaserialno = new Cell(1,1).Add(new Paragraph(item.SerialNo != null ? item.SerialNo.ToString() : "")).SetTextAlignment(TextAlignment.LEFT);
                    _tblBHA.AddCell(bhaserialno);
                    if (item.ToolOuterDia > 0)
                    {
                        outdia = Math.Round((item.ToolOuterDia * objInputData.UOM.SizeMultiplier), 3);
                    }
                    bhamod = new Cell(1, 1).Add(new Paragraph(outdia > 0 ? outdia.ToString() : "")).SetTextAlignment(TextAlignment.LEFT);
                    _tblBHA.AddCell(bhamod);
                    if (item.ToolInnerDia > 0)
                    {
                        inndia = Math.Round((item.ToolInnerDia * objInputData.UOM.SizeMultiplier), 3);
                    }
                    bhainndia = new Cell(1, 1).Add(new Paragraph(inndia > 0 ? inndia.ToString() : "")).SetTextAlignment(TextAlignment.LEFT);
                    _tblBHA.AddCell(bhainndia);
                    if (item.ToolWeight > 0)
                    {
                        toolwt = Math.Round((item.ToolWeight * objInputData.UOM.WeightMultiplier), 3);
                    }
                    bhaweight = new Cell(1, 1).Add(new Paragraph(toolwt > 0 ? toolwt.ToString() : "")).SetTextAlignment(TextAlignment.LEFT);
                    _tblBHA.AddCell(bhaweight);
                    if (item.ToolLength > 0)
                    {
                        toollen = Math.Round((item.ToolLength * objInputData.UOM.DepthMultiplier), 3);
                    }
                    bhatoollength = new Cell(1,1).Add(new Paragraph(toollen > 0 ? toollen.ToString() : "")).SetTextAlignment(TextAlignment.LEFT);
                    _tblBHA.AddCell(bhatoollength);
                    bhaupcontyp = new Cell(1,1).Add(new Paragraph(item.ToolUpperConnType != null ? item.ToolUpperConnType.ToString() :"")).SetTextAlignment(TextAlignment.LEFT);
                    _tblBHA.AddCell(bhaupcontyp);
                    bhalowcontyp = new Cell(1,1).Add(new Paragraph(item.ToolLowerConnType != null ? item.ToolLowerConnType.ToString() : "")).SetTextAlignment(TextAlignment.LEFT);
                    _tblBHA.AddCell(bhalowcontyp);
                    if (item.ToolFishNeckOD > 0)
                    {
                        fishod = Math.Round((item.ToolFishNeckOD * objInputData.UOM.SizeMultiplier), 3);
                    }
                    bhafishneckod = new Cell(1,1).Add(new Paragraph(fishod > 0 ? fishod.ToString() : "")).SetTextAlignment(TextAlignment.LEFT);
                    _tblBHA.AddCell(bhafishneckod);
                    if (item.ToolFishNeckLength > 0)
                    {
                        fishlen = Math.Round((item.ToolFishNeckLength * objInputData.UOM.DepthMultiplier), 3);
                    }
                    bhafishnecklen = new Cell(1,1).Add(new Paragraph(fishlen > 0 ? fishlen.ToString() : "")).SetTextAlignment(TextAlignment.LEFT);
                    _tblBHA.AddCell(bhafishnecklen);
                    if (item.ToolHydraulicOD > 0)
                    {
                        hydod = Math.Round((item.ToolHydraulicOD * objInputData.UOM.SizeMultiplier), 3);
                    }
                    bhahydod = new Cell(1, 1).Add(new Paragraph(hydod > 0 ? hydod.ToString() : "")).SetTextAlignment(TextAlignment.LEFT);
                    _tblBHA.AddCell(bhahydod);
                    if (item.ToolHydraulicID > 0)
                    {
                        hydid = Math.Round((item.ToolHydraulicID * objInputData.UOM.SizeMultiplier), 3);
                    }
                    bhahydind = new Cell(1, 1).Add(new Paragraph(hydid > 0 ? hydid.ToString() : "")).SetTextAlignment(TextAlignment.LEFT);
                    _tblBHA.AddCell(bhahydind);
                }

                return _tblBHA.SetAutoLayout();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
       
        private double GetWorkStringWeight(PdfReportService objWrkString, int increment)
        {
            double _toolWeight = 0;
            string wrkStrWeight = objWrkString.WorkStringItems[increment].wrkWeight;

            if(wrkStrWeight != null)
            {
                _toolWeight = double.Parse(wrkStrWeight);
            }
            return _toolWeight;
        }
        private string GetWorkStringUpperConn(PdfReportService objUpConnType, int increment)
        {
            string _toolUpperConnType = string.Empty;
            string wrkStrUpConnTyp = objUpConnType.WorkStringItems[increment].wrkUpperConnType;
            if(wrkStrUpConnTyp != null)
            {
                _toolUpperConnType = wrkStrUpConnTyp.ToString();
            }
            return _toolUpperConnType;
        }
        private string GetSerialNo(PdfReportService objBhaSerialNo, int increment)
        {
            string _toolSerialNo = string.Empty;
            string bhaSerialNo = objBhaSerialNo.BHAToolItemData[increment].SerialNumber;
            if (bhaSerialNo != null)
            {
                _toolSerialNo = bhaSerialNo.ToString();
            }
            return _toolSerialNo;
        }
        private double GetToolWeight(PdfReportService objBhaToolWeight, int increment)
        {
            double _toolWeight = 0;
            string bhaStrWeight = objBhaToolWeight.BHAToolItemData[increment].Weight;
            if (bhaStrWeight != null)
            {
                _toolWeight = double.Parse(bhaStrWeight);
            }
            return _toolWeight;
        }
        private string GetToolUpperConn(PdfReportService objUpConnType, int increment)
        {
            string _toolUpperConnType = string.Empty;
            string bhaStrUpConnTyp = objUpConnType.BHAToolItemData[increment].UpperConnType;
            if (bhaStrUpConnTyp != null)
            {
                _toolUpperConnType = bhaStrUpConnTyp.ToString();
            }
            return _toolUpperConnType;
        }
        private string GetToolLowerConn(PdfReportService objLowConnType, int increment)
        {
            string _toolLowConnType = string.Empty;
            var bhaLowConnType = objLowConnType.BHAToolItemData[increment].LowerConnType;
            if (bhaLowConnType != null)
            {
                _toolLowConnType = bhaLowConnType.ToString();
            }
            return _toolLowConnType;
        }
        private double GetToolFishNeckOD(PdfReportService objFishNeckOd, int increment)
        {
            double _toolFishNeckOD = 0;
            double bhaFishNeckOd = objFishNeckOd.BHAToolItemData[increment].FishNeckOD;
            if (bhaFishNeckOd > 0)
            {
                _toolFishNeckOD = bhaFishNeckOd;
            }
            return _toolFishNeckOD;
        }
        private double GetToolFishNeckLength(PdfReportService objFishNeckLen, int increment)
        {
            double _toolWeight = 0;
            double bhaFishNeckLen = objFishNeckLen.BHAToolItemData[increment].FishNeckLength;
            if (bhaFishNeckLen > 0)
            {
                _toolWeight = bhaFishNeckLen;
            }
            return _toolWeight;
        }
        private double GetToolHydraulicOD(PdfReportService objHydraulicOD, int increment)
        {
            double _toolWeight = 0;
            double bhaHydraOD = objHydraulicOD.BHAToolItemData[increment].HydraulicOD;
            if (bhaHydraOD > 0)
            {
                _toolWeight = bhaHydraOD;
            }
            return _toolWeight;
        }
        private double GetToolHydraulicID(PdfReportService objHydraulicID, int increment)
        {
            double _toolHydrID = 0;
            double bhaHydraID = objHydraulicID.BHAToolItemData[increment].HydraulicID;
            if (bhaHydraID > 0)
            {
                _toolHydrID = bhaHydraID;
            }
            return _toolHydrID;
        }
    }
}
