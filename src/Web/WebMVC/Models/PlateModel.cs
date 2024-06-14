namespace WebMVC.Models
{
    public class PlateModel
    {
        public Guid Id { get; set; }

        public string? Registration { get; set; }

        public decimal PurchasePrice { get; set; }

        public decimal SalePrice => Math.Round(PurchasePrice * 1.2m, 2);

        public string? Letters { get; set; }

        public int Numbers { get; set; }
    }
}
