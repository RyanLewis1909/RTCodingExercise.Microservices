﻿using Catalog.API.Messages.Request;
using System;
using System.Linq;
using System.Text.RegularExpressions;
using static Humanizer.In;

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
            var getNumbers = Regex.Match(createPlateRequest.Registration, @"\d+").Value;
            if (string.IsNullOrEmpty(getNumbers))
            {
                throw new Exception("Plate must contain numbers");
            }

            var numbers = int.Parse(getNumbers);
            var newPlate = new Plate
            {
                Id = Guid.NewGuid(),
                Registration = createPlateRequest.Registration,
                PurchasePrice = createPlateRequest.PurchasePrice,
                SalePrice = createPlateRequest.SalePrice,
                Letters = letters,
                Numbers = numbers,
                IsReserved = createPlateRequest.IsReserved
            };

            await _applicationDbContext.Plates.AddAsync(newPlate);
            await _applicationDbContext.SaveChangesAsync();
        }

        public async Task CreatePlateAudit(PlateAudit newPlateAudit)
        {
            await _applicationDbContext.PlateAudits.AddAsync(newPlateAudit);
            await _applicationDbContext.SaveChangesAsync();
        }

        public async Task<Plate> GetPlate(Guid id)
        {
            return await _applicationDbContext.Plates.FirstAsync(x => x.Id == id);
        }

        public async Task<ActionResult<List<PlateAudit>>> GetPlateAudits(Guid id)
        {
            return await _applicationDbContext.PlateAudits.Where(x => x.TableId == id).ToListAsync();
        }

        public async Task<PaginatedList<Plate>> GetPlates(int? pageNumber, string? sortOrder, string? searchString, string? currentFilter)
        {
            var plates = _applicationDbContext.Plates.AsQueryable();

            if (searchString != null)
            {
                pageNumber = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            if (!string.IsNullOrEmpty(searchString))
            {
                // If only numbers assume age search
                if (int.TryParse(searchString, out var age))
                {
                    plates = plates.Where(x => x.Numbers.ToString().Contains(age.ToString()));
                }
                else
                {
                    plates = plates.Where(x => !string.IsNullOrEmpty(x.Letters) && x.Letters.Contains(searchString));
                }

                // Filtered search we only want For Sale regs
                plates = plates.Where(x => !x.IsReserved);
            }

            plates = sortOrder switch
            {
                "registration_desc" => plates.OrderByDescending(x => x.Registration),
                "PurchasePrice" => plates.OrderBy(x => x.PurchasePrice),
                "price_desc" => plates.OrderByDescending(x => x.PurchasePrice),
                _ => plates.OrderBy(x => x.Registration),
            };

            var totalSold = await _applicationDbContext.Plates.Where(x => x.IsSold).SumAsync(x => x.SalePrice);

            // Default to 20 if not set
            var platePageSplit = _appSettings.PlatePageSplit ?? 20;
            return await PaginatedList<Plate>.CreateAsync(plates.AsNoTracking(), pageNumber ?? 1, platePageSplit, totalSold);
        }

        public async Task UpdatePlate(UpdatePlateRequest updatePlateRequest)
        {
            var plate = await GetPlate(updatePlateRequest.Id);
            plate.Registration = updatePlateRequest.Registration;
            plate.PurchasePrice = updatePlateRequest.PurchasePrice;
            plate.SalePrice = updatePlateRequest.SalePrice;
            plate.IsReserved = updatePlateRequest.IsReserved;
            plate.IsSold = updatePlateRequest.IsSold;
            await _applicationDbContext.SaveChangesAsync();
        }
    }
}
