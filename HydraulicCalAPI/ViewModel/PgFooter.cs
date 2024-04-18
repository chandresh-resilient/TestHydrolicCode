using System.Collections.Generic;
using HydraulicCalAPI.Service;
using iText.Layout;
using iText.Kernel.Pdf;
using iText.Kernel.Colors;
using iText.Layout.Borders;
using iText.Layout.Element;
using iText.Layout.Properties;
using iText.Kernel.Pdf.Canvas.Draw;

namespace HydraulicCalAPI.ViewModel
{
    public class PgFooter
    {
        public void AddFooter(PdfDocument pdfDocument, Document document, PdfReportService objFooterData)
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
        private class UnitConverter
        {
            public static float uu2inch(float uu) => uu / 72f;
            public static float inch2uu(float inch) => inch * 72f;
            public static float inch2mm(float inch) => inch * 25.4f;
            public static float mm2inch(float mm) => mm / 25.4f;
            public static float uu2mm(float uu) => inch2mm(uu2inch(uu));
            public static float mm2uu(float mm) => inch2uu(mm2inch(mm));
        }
    }
}
