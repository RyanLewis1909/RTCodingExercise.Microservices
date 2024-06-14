using Catalog.API.Messages.Request;
using Catalog.API.Messages.Response;

namespace WebMVC.Services
{
    public interface ICatalogService
    {
        public Task CreatePlate(CreatePlateRequest createPlateRequest);
        public Task<Plate> GetPlate(Guid id);
        public Task<GetPlateItemsResult> GetPlateItems(GetPlateItemsRequest getPlateItemsRequest);
        public Task UpdatePlate(UpdatePlateRequest updatePlateRequest);
    }
}
