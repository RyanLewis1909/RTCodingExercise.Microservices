namespace Catalog.API.Messages.Response
{
    public record GetPlateItemsResult
    {
        public PaginatedList<Plate> Plates { get; set; }
        public int PageIndex { get; set; }
        public int TotalPages { get; set; }
        public decimal TotalSold { get; set; }
    }
}