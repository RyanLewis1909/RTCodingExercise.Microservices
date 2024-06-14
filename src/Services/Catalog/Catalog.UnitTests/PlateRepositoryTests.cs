using Catalog.API;
using Catalog.API.Data;
using Catalog.API.Messages.Request;
using Catalog.Domain;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Catalog.UnitTests
{
    public class PlateRepositoryTests
    {
        private PlateRepository _plateRepository;
        private ApplicationDbContext _applicationDbContext;
        private AppSettings _appSettings;
        private CreatePlateRequest _createPlateRequest = new CreatePlateRequest { Registration = "T123T", IsReserved = false, IsSold = false, PurchasePrice = 10m, SalePrice = 20m };
        private readonly Guid _id = Guid.NewGuid();

        public PlateRepositoryTests()
        {
            SetupMocks();
        }

        [Fact]
        public async Task CreatePlate_WhenCalled_SavesNewPlate()
        {
            await _plateRepository.CreatePlate(_createPlateRequest);

            Assert.NotNull(_applicationDbContext.Plates.First(x => x.Registration == "T123T"));
        }

        [Fact]
        public async Task CreatePlate_WhenCalled_SavesCorrectLetters()
        {
            await _plateRepository.CreatePlate(_createPlateRequest);

            var plate = _applicationDbContext.Plates.First(x => x.Registration == "T123T");
            Assert.Equal("TT", plate.Letters);
        }

        [Fact]
        public async Task CreatePlate_WhenCalled_SavesCorrectNumbers()
        {
            await _plateRepository.CreatePlate(_createPlateRequest);

            var plate = _applicationDbContext.Plates.First(x => x.Registration == "T123T");
            Assert.Equal(123, plate.Numbers);
        }

        [Fact]
        public async Task CreatePlate_WhenCalledWithNoLetters_SavesNewPlate()
        {
            _createPlateRequest.Registration = "1234";

            await _plateRepository.CreatePlate(_createPlateRequest);

            Assert.NotNull(_applicationDbContext.Plates.First(x => x.Registration == "1234"));
        }

        [Fact]
        public async Task CreatePlate_WhenCalledWithNoNumbers_ThrowsException()
        {
            _createPlateRequest.Registration = "TTT";

            Func<Task> act = () => _plateRepository.CreatePlate(_createPlateRequest);

            await Assert.ThrowsAsync<Exception>(act);
        }

        [Fact]
        public async Task GetPlate_WhenCalled_ReturnsPlate()
        {
            var plate = await _plateRepository.GetPlate(_id);

            Assert.NotNull(plate);
        }

        [Fact]
        public async Task GetPlate_WhenCalledWithNonExistantId_ThrowsException()
        {
            Func<Task> act = () => _plateRepository.GetPlate(new Guid());

            await Assert.ThrowsAsync<InvalidOperationException>(act);
        }

        private void SetupMocks()
        {
            // Db Context
            var options = new DbContextOptionsBuilder<ApplicationDbContext>().UseInMemoryDatabase(databaseName: "MovieListDatabase").Options;
            _applicationDbContext = new ApplicationDbContext(options);
            _applicationDbContext.Plates.AddRange(MockPlates());
            _applicationDbContext.SaveChanges();

            // App settings
            _appSettings = new AppSettings { PlatePageSplit = 2 };

            _plateRepository = new PlateRepository(_applicationDbContext, _appSettings);
        }

        private List<Plate> MockPlates()
        {
            return new List<Plate>
            {
                new Plate
                {
                    Id = _id,
                    Registration = "TREG1",
                    PurchasePrice = 123.12m
                },
                new Plate
                {
                    Id = Guid.NewGuid(),
                    Registration = "TREG2",
                    PurchasePrice = 321.12m
                },
                new Plate
                {
                    Id = Guid.NewGuid(),
                    Registration = "TREG3",
                    PurchasePrice = 456.32m
                }
            };
        }
    }
}
