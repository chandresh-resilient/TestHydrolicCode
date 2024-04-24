using System;
using System.Collections.Generic;
using HydraulicCalAPI.Service;
using iText.Layout.Element;
using iText.Layout.Properties;

namespace HydraulicCalAPI.ViewModel
{
    public class PgPreface
    {
        Dictionary<string, string> pdfAuthor;
        string dtPrepared = string.Empty;
        public Table GetAuthorContent(PdfReportService objInputData)
        {
            Table _tableAuthor;
            return _tableAuthor = getTableContent(objInputData);
        }

        protected Table getTableContent(PdfReportService _objInputData)
        {
            try
            {
                if (_objInputData.PreparedOn == null)
                {
                    dtPrepared = string.Empty;
                }
                else
                {
                    var _date = _objInputData.PreparedOn.ToString();
                    dtPrepared = GetFormattedDate(_date.ToString());
                }

                pdfAuthor = new Dictionary<string, string>();
                pdfAuthor.Add("Customer : ", _objInputData.Customer != null ? _objInputData.Customer.ToString() : "");
                pdfAuthor.Add("Job Number : ", _objInputData.PlJobNumber != null ? _objInputData.PlJobNumber.ToString() : "");
                pdfAuthor.Add("Well Name and Number : ", _objInputData.WellNameNumber != null ? _objInputData.WellNameNumber.ToString() : "");
                pdfAuthor.Add("Prepared By : ", _objInputData.PreparedBy != null ? _objInputData.PreparedBy.ToString() : "");
                pdfAuthor.Add("Prepared On : ", dtPrepared);

                Table _table = new Table(2, true);

                foreach (var item in pdfAuthor)
                {
                    _table.AddCell(new Paragraph(item.Key).SetTextAlignment(TextAlignment.LEFT).SetBold());
                    _table.AddCell(new Paragraph(item.Value).SetTextAlignment(TextAlignment.LEFT).SetWidth(150));
                }
                return _table.SetAutoLayout();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        protected string GetFormattedDate(string objDate)
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
    }
}
