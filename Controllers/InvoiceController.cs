using InvoiceManagementSystem.Data;
using InvoiceManagementSystem.Models;
using InvoiceManagementSystem.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace InvoiceManagementSystem.Controllers
{
    public class InvoiceController : Controller
    {
        private readonly ApplicationDbContext _context;

        public InvoiceController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index(
            string status,
            string sortBy,
            DateTime? fromDate,
            DateTime? toDate)
        {
            var invoices = _context.Invoices
                .Where(x => !x.IsDeleted)
                .AsQueryable();

            
            if (!string.IsNullOrEmpty(status))
            {
                invoices = invoices.Where(x => x.Status == status);
            }

            if (fromDate.HasValue)
            {
                invoices = invoices.Where(x => x.InvoiceDate >= fromDate.Value);
            }

            if (toDate.HasValue)
            {
                invoices = invoices.Where(x => x.InvoiceDate <= toDate.Value);
            }

            switch (sortBy)
            {
                case "date":
                    invoices = invoices.OrderByDescending(x => x.InvoiceDate);
                    break;

                case "amount":
                    invoices = invoices.OrderByDescending(x => x.TotalAmount);
                    break;

                default:
                    invoices = invoices.OrderByDescending(x => x.Id);
                    break;
            }

            return View(await invoices.ToListAsync());
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Invoice invoice)
        {
            if (!ModelState.IsValid)
            {
                return View(invoice);
            }

            bool isDuplicate = await _context.Invoices
                .AnyAsync(x => x.InvoiceNumber == invoice.InvoiceNumber);

            if (isDuplicate)
            {
                ModelState.AddModelError("InvoiceNumber", "Invoice number already exists");
                return View(invoice);
            }

            _context.Invoices.Add(invoice);
            await _context.SaveChangesAsync();

            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Edit(int id)
        {
            var invoice = await _context.Invoices.FindAsync(id);
            return View(invoice);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(Invoice invoice)
        {
            if (!ModelState.IsValid)
            {
                return View(invoice);
            }

            var existingInvoice = await _context.Invoices
                .FirstOrDefaultAsync(x => x.Id == invoice.Id);

            if (existingInvoice == null)
            {
                return NotFound();
            }

            bool isDuplicate = await _context.Invoices
                .AnyAsync(x => x.InvoiceNumber == invoice.InvoiceNumber && x.Id != invoice.Id);

            if (isDuplicate)
            {
                ModelState.AddModelError("InvoiceNumber", "Invoice number already exists");
                return View(invoice);
            }

            existingInvoice.InvoiceNumber = invoice.InvoiceNumber;
            existingInvoice.InvoiceDate = invoice.InvoiceDate;
            existingInvoice.CustomerName = invoice.CustomerName;
            existingInvoice.TotalAmount = invoice.TotalAmount;
            existingInvoice.TaxAmount = invoice.TaxAmount;
            existingInvoice.Status = invoice.Status;

            await _context.SaveChangesAsync();

            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Delete(int id)
        {
            var invoice = await _context.Invoices.FindAsync(id);

            if (invoice != null)
            {
                invoice.IsDeleted = true;
                await _context.SaveChangesAsync();
            }

            return RedirectToAction("Index");
        }

        public IActionResult Export()
        {
            var invoices = _context.Invoices
                .Where(x => !x.IsDeleted)
                .ToList();
                
            var exporService = new ExportService();
            var fileContent = exporService.ExExportInvoices(invoices);

            return File(
                fileContent,
                "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                "Invoices.xlsx");
        }
    }
}