namespace Catalog.API.Messages.Request
{
    public record GetPlateItemsRequest
    {
        public int? PageNumber { get; set; }
        public string? SortOrder { get; set; }
        public string? SearchString { get; set; }
        public string? CurrentFilter { get; set; }
    }
}