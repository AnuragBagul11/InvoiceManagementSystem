using System.ComponentModel.DataAnnotations;
namespace InvoiceManagementSystem.Models
{
    public class Invoice
    {
        public int Id { get; set; }

        [Required]
        public string InvoiceNumber { get; set; }=string.Empty;
        [Required]
        public DateTime InvoiceDate {  get; set; }
        [Required]
        public string CustomerName { get; set; } = string.Empty;

        [Required]
        [Range(0.01,double.MaxValue)]
        public decimal TotalAmount { get; set; }

        [Required]
        public decimal TaxAmount { get; set; }

        [Required]
        public string Status { get; set; } =string.Empty;
        public DateTime CreatedDate { get; set; }= DateTime.Now;
        public bool IsDeleted { get; set; }=false;
    }
}
