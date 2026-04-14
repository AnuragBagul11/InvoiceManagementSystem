using ClosedXML.Excel;
using InvoiceManagementSystem.Models;

namespace InvoiceManagementSystem.Services
{
    public class ExportService
    {

        public byte[] ExExportInvoices(List<Invoice> invoices) {
            using var workbook = new XLWorkbook();
            var worksheet = workbook.Worksheets.Add("Invoices");

            worksheet.Cell(1, 1).Value = "Invoice Number";
            worksheet.Cell(1, 2).Value = "Customer";
            worksheet.Cell(1, 3).Value = "Total Amount";
            worksheet.Cell(1, 4).Value = "Tax Amount";
            worksheet.Cell(1, 5).Value = "Status";

            int row = 2;
            foreach (var invoice in invoices)
            {
                worksheet.Cell(row , 1).Value=invoice.InvoiceNumber;
                worksheet.Cell(row, 2).Value = invoice.CustomerName;
                worksheet.Cell(row, 3).Value = invoice.TotalAmount;
                worksheet.Cell(row, 4).Value = invoice.TaxAmount;
                worksheet.Cell(row, 5).Value = invoice.Status;

                row ++; 
            }
            using var strem = new MemoryStream();
            workbook.SaveAs(strem);
            return strem.ToArray();
        }
    }
}
