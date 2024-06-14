using Catalog.API.Messages.Request;

namespace Catalog.API.Data
{
    public interface IPlateRepository
    {
        public Task CreatePlate(CreatePlateRequest createPlateRequest);
        public Task<PaginatedList<Plate>> GetPlates(int? pageNumber);
    }
}
