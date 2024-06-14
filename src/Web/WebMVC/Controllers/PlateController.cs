using Catalog.API.Data;
using Catalog.API.Messages.Request;
using MassTransit;
using WebMVC.Models;
using WebMVC.Services;

namespace WebMVC.Controllers
{
    public class PlateController : Controller
    {
        ICatalogService _catalogService;

        public PlateController(ICatalogService catalogService)
        {
            _catalogService = catalogService;
        }

        [HttpGet]
        public async Task<IActionResult> Index(int? pageNumber, string? sortOrder, string? searchString, string? currentFilter)
        {
            ViewData["CurrentSort"] = sortOrder;
            ViewData["RegSortParm"] = string.IsNullOrEmpty(sortOrder) ? "registration_desc" : "";
            ViewData["PriceSortParm"] = sortOrder == "PurchasePrice" ? "price_desc" : "PurchasePrice";
            var response = await _catalogService.GetPlateItems(new GetPlateItemsRequest
            {
                PageNumber = pageNumber,
                SortOrder = sortOrder,
                SearchString = searchString,
                CurrentFilter = currentFilter
            });
            ViewData["CurrentFilter"] = searchString ?? currentFilter;
            var model = new PaginatedList<PlateModel>();
            response.Plates.ForEach(x =>
            {
                model.Add(new PlateModel
                {
                    Id = x.Id,
                    Registration = x.Registration,
                    PurchasePrice = x.PurchasePrice,
                    IsReserved = x.IsReserved
                });
            });
            model.PageIndex = response.PageIndex;
            model.TotalPages = response.TotalPages;
            return View(model);
        }

        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(PlateModel plate)
        {
            await _catalogService.CreatePlate(new CreatePlateRequest
            {
                Registration = plate.Registration,
                PurchasePrice = plate.PurchasePrice,
                SalePrice = plate.SalePrice,
                IsReserved = plate.IsReserved
            });
            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> Edit(Guid id)
        {
            var response = await _catalogService.GetPlate(id);
            var model = new PlateModel
            {
                Id = response.Id,
                Registration = response.Registration,
                PurchasePrice = response.PurchasePrice,
                IsReserved = response.IsReserved
            };
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(PlateModel plate)
        {
            await _catalogService.UpdatePlate(new UpdatePlateRequest
            {
                Id= plate.Id,
                Registration = plate.Registration,
                PurchasePrice = plate.PurchasePrice,
                SalePrice = plate.SalePrice,
                IsReserved = plate.IsReserved
            });
            return RedirectToAction("Index");
        }
    }
}
