using System.ComponentModel.DataAnnotations;

namespace WebMVC.Models
{
    public class PlateModel
    {
        public Guid Id { get; set; }

        [Required] 
        public string? Registration { get; set; }

        [Required]
        public decimal PurchasePrice { get; set; }

        [Required]
        public decimal SalePrice => Math.Round(PurchasePrice * 1.2m, 2);

        [Required]
        public bool IsReserved { get; set; }

        public string Status => IsReserved ? "Reserved" : "For Sale";
    }
}
