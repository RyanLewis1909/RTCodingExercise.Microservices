using Catalog.API.Data;
using Catalog.API.Messages.Request;
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
        public async Task<IActionResult> Index(int? pageNumber)
        {
            var response = await _catalogService.GetPlateItems(new GetPlateItemsRequest
            {
                PageNumber = pageNumber
            });
            var model = new PaginatedList<Models.PlateModel>();
            response.Plates.ForEach(x =>
            {
                model.Add(new Models.PlateModel
                {
                    Id = x.Id,
                    Registration = x.Registration,
                    PurchasePrice = x.PurchasePrice,
                    Letters = x.Letters,
                    Numbers = x.Numbers
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
                SalePrice = plate.SalePrice
            });
            return RedirectToAction("Index");
        }
    }
}
