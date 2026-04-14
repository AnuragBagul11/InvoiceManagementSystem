using InvoiceManagementSystem.Models;
using Microsoft.EntityFrameworkCore;

namespace InvoiceManagementSystem.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Invoice> Invoices { get; set; }
    }
}