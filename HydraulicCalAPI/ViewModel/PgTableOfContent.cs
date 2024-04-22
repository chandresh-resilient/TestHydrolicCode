using System;
using iText.Layout.Element;
using iText.Layout.Borders;
using iText.Kernel.Colors;
using iText.Layout.Properties;


namespace HydraulicCalAPI.ViewModel
{
    public class PgTableOfContent
    {
        public Table GetTableOfcontent(string strSubProductLine)
        {
            try
            {
                Table _tbltoc = new Table(3, true);

                Cell tocHeader = new Cell(1, 3).Add(new Paragraph("Table Of Contents")).SetFontSize(18).SetBold().SetBorder(Border.NO_BORDER);

                Cell tocLine1col1r1 = new Cell(1, 1).Add(new Paragraph("1.")).SetWidth(5).SetFontSize(12).SetBorder(Border.NO_BORDER).SetTextAlignment(TextAlignment.LEFT);
                Cell tocLine1col2r1 = new Cell(1, 1).Add(new Paragraph("Header Information")).SetFontSize(12).SetBorder(Border.NO_BORDER).SetTextAlignment(TextAlignment.LEFT);
                Cell tocLine1col3r1 = new Cell(1, 1).Add(new Paragraph("3")).SetWidth(10).SetFontSize(12).SetBorder(Border.NO_BORDER).SetTextAlignment(TextAlignment.RIGHT).SetFontColor(ColorConstants.BLUE);

                Cell tocLine2col1r2 = new Cell(1, 1).Add(new Paragraph("2.")).SetWidth(5).SetFontSize(12).SetBorder(Border.NO_BORDER).SetTextAlignment(TextAlignment.LEFT);
                Cell tocLine2col2r2 = new Cell(1, 1).Add(new Paragraph(strSubProductLine + " - Casing Liner Tubing")).SetFontSize(12).SetBorder(Border.NO_BORDER).SetTextAlignment(TextAlignment.LEFT);
                Cell tocLine2col3r2 = new Cell(1, 1).Add(new Paragraph("5")).SetFontSize(12).SetBorder(Border.NO_BORDER).SetTextAlignment(TextAlignment.RIGHT).SetFontColor(ColorConstants.BLUE);

                Cell tocLine3col1r3 = new Cell(1, 1).Add(new Paragraph("3.")).SetWidth(5).SetFontSize(12).SetBorder(Border.NO_BORDER).SetTextAlignment(TextAlignment.LEFT);
                Cell tocLine3col2r3 = new Cell(1, 1).Add(new Paragraph(strSubProductLine + " - BHA")).SetFontSize(12).SetBorder(Border.NO_BORDER).SetTextAlignment(TextAlignment.LEFT);
                Cell tocLine3col3r3 = new Cell(1, 1).Add(new Paragraph("6")).SetFontSize(12).SetBorder(Border.NO_BORDER).SetTextAlignment(TextAlignment.RIGHT).SetFontColor(ColorConstants.BLUE);

                Cell tocLine4col1r4 = new Cell(1, 1).Add(new Paragraph("4.")).SetWidth(5).SetFontSize(12).SetBorder(Border.NO_BORDER).SetTextAlignment(TextAlignment.LEFT);
                Cell tocLine4col2r4 = new Cell(1, 1).Add(new Paragraph(strSubProductLine + " - Surface Equipment & Fluid Information")).SetFontSize(12).SetBorder(Border.NO_BORDER).SetTextAlignment(TextAlignment.LEFT);
                Cell tocLine4col3r4 = new Cell(1, 1).Add(new Paragraph("7")).SetFontSize(12).SetBorder(Border.NO_BORDER).SetTextAlignment(TextAlignment.RIGHT).SetFontColor(ColorConstants.BLUE);

                Cell tocLine5col1r5 = new Cell(1, 1).Add(new Paragraph("5.")).SetWidth(5).SetFontSize(12).SetBorder(Border.NO_BORDER).SetTextAlignment(TextAlignment.LEFT);
                Cell tocLine5col2r5 = new Cell(1, 1).Add(new Paragraph(strSubProductLine + " - Standpipe vs Flowrate Graph")).SetFontSize(12).SetBorder(Border.NO_BORDER).SetTextAlignment(TextAlignment.LEFT);
                Cell tocLine5col3r5 = new Cell(1, 1).Add(new Paragraph("8")).SetFontSize(12).SetBorder(Border.NO_BORDER).SetTextAlignment(TextAlignment.RIGHT).SetFontColor(ColorConstants.BLUE);

                Cell tocLine6col1r6 = new Cell(1, 1).Add(new Paragraph("6.")).SetWidth(5).SetFontSize(12).SetBorder(Border.NO_BORDER).SetTextAlignment(TextAlignment.LEFT);
                Cell tocLine6col2r6 = new Cell(1, 1).Add(new Paragraph(strSubProductLine + " - Hydraulic Output")).SetFontSize(12).SetBorder(Border.NO_BORDER).SetTextAlignment(TextAlignment.LEFT);
                Cell tocLine6col3r6 = new Cell(1, 1).Add(new Paragraph("9")).SetWidth(5).SetFontSize(12).SetBorder(Border.NO_BORDER).SetTextAlignment(TextAlignment.RIGHT).SetFontColor(ColorConstants.BLUE);


                _tbltoc.AddCell(tocHeader);
                _tbltoc.AddCell(tocLine1col1r1);
                _tbltoc.AddCell(tocLine1col2r1);
                _tbltoc.AddCell(tocLine1col3r1);

                _tbltoc.AddCell(tocLine2col1r2);
                _tbltoc.AddCell(tocLine2col2r2);
                _tbltoc.AddCell(tocLine2col3r2);

                _tbltoc.AddCell(tocLine3col1r3);
                _tbltoc.AddCell(tocLine3col2r3);
                _tbltoc.AddCell(tocLine3col3r3);

                _tbltoc.AddCell(tocLine4col1r4);
                _tbltoc.AddCell(tocLine4col2r4);
                _tbltoc.AddCell(tocLine4col3r4);

                _tbltoc.AddCell(tocLine5col1r5);
                _tbltoc.AddCell(tocLine5col2r5);
                _tbltoc.AddCell(tocLine5col3r5);

                _tbltoc.AddCell(tocLine6col1r6);
                _tbltoc.AddCell(tocLine6col2r6);
                _tbltoc.AddCell(tocLine6col3r6);
                return _tbltoc.SetAutoLayout();

            }
            catch (Exception ex)
            {
                throw;
            }
            
        }
    }
}
