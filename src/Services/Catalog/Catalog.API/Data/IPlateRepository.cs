using Catalog.API.Messages.Request;
using Catalog.API.Messages.Response;

namespace Catalog.API.Data
{
    public interface IPlateRepository
    {
        public Task CreatePlate(CreatePlateRequest createPlateRequest);
        public Task<Plate> GetPlate(Guid id);
        public Task<PaginatedList<Plate>> GetPlates(int? pageNumber, string? sortOrder, string? searchString, string? currentFilter);
        public Task UpdatePlate(UpdatePlateRequest updatePlateRequest);
    }
}
