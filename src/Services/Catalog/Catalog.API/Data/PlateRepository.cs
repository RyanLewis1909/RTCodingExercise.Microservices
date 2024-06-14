using Catalog.API.Messages.Request;
using System;
using System.Text.RegularExpressions;

namespace Catalog.API.Data
{
    public class PlateRepository : IPlateRepository
    {
        private readonly ApplicationDbContext _applicationDbContext;
        private readonly AppSettings _appSettings;

        public PlateRepository(ApplicationDbContext applicationDbContext, AppSettings appSettings)
        {
            _applicationDbContext = applicationDbContext;
            _appSettings = appSettings;
        }

        public async Task CreatePlate(CreatePlateRequest createPlateRequest)
        {
            var letters = new string(createPlateRequest.Registration.Where(char.IsLetter).ToArray());
            var numbers = int.Parse(Regex.Match(createPlateRequest.Registration, @"\d+").Value);
            var newPlate = new Plate
            {
                Id = Guid.NewGuid(),
                Registration = createPlateRequest.Registration,
                PurchasePrice = createPlateRequest.PurchasePrice,
                SalePrice = createPlateRequest.SalePrice,
                Letters = letters,
                Numbers = numbers
            };

            await _applicationDbContext.Plates.AddAsync(newPlate);
            await _applicationDbContext.SaveChangesAsync();
        }

        public async Task<PaginatedList<Plate>> GetPlates(int? pageNumber, string sortOrder)
        {
            var plates = _applicationDbContext.Plates.AsQueryable().OrderBy(x => x.Registration);

            switch (sortOrder)
            {
                case "registration_desc":
                    plates = plates.OrderByDescending(s => s.Registration);
                    break;
                case "PurchasePrice":
                    plates = plates.OrderBy(s => s.PurchasePrice);
                    break;
                case "price_desc":
                    plates = plates.OrderByDescending(s => s.PurchasePrice);
                    break;
                default:
                    plates = plates.OrderBy(s => s.Registration);
                    break;
            }

            // Default to 20 if not set
            var platePageSplit = _appSettings.PlatePageSplit ?? 20;
            return await PaginatedList<Plate>.CreateAsync(plates.AsNoTracking(), pageNumber ?? 1, platePageSplit);
        }
    }
}
