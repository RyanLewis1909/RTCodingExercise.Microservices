namespace Catalog.API.Messages.Request
{
    public record UpdatePlateRequest
    {
        public Guid Id { get; set; }
        public string Registration { get; set; }
        public decimal PurchasePrice { get; set; }
        public decimal SalePrice { get; set; }
        public bool IsReserved { get; set; }
        public bool IsSold { get; set; }
    }
}