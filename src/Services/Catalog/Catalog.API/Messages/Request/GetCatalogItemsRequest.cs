namespace Catalog.API.Messages.Request
{
    public record GetPlateItemsRequest
    {
        public int? PageNumber { get; set; }
    }
}