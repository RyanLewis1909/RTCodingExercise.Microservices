using AutoMapper;
using Catalog.API.Data;
using Catalog.API.Messages.Request;
using IntegrationEvents;
using MassTransit;
using MassTransit.Transports;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using WebMVC.Models;
using WebMVC.Services;

namespace WebMVC.Controllers
{
    public class PlateController : Controller
    {
        private readonly ICatalogService _catalogService;
        private readonly IMapper _mapper;
        private readonly IPublishEndpoint _publishEndpoint;

        public PlateController(ICatalogService catalogService, IMapper mapper, IPublishEndpoint publishEndpoint)
        {
            _catalogService = catalogService;   
            _mapper = mapper;
            _publishEndpoint = publishEndpoint;
        }

        [HttpGet]
        public async Task<IActionResult> Index(int? pageNumber, string? sortOrder, string? searchString, string? currentFilter, string? promoCode)
        {
            ViewData["PromoCode"] = promoCode;
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
            var model = _mapper.Map<PaginatedList<PlateModel>>(response.Plates);
            model.PageIndex = response.PageIndex;
            model.TotalPages = response.TotalPages;
            model.TotalPages = response.TotalPages;
            model.TotalSold = response.TotalSold;
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
            await AuditPlateChange(plate, "Create");
            var request = _mapper.Map<CreatePlateRequest>(plate);
            await _catalogService.CreatePlate(request);
            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> Edit(Guid id)
        {
            var plate = await _catalogService.GetPlate(id);
            var model = _mapper.Map<PlateModel>(plate);
            model.OriginalIsReserved = model.IsReserved;
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(PlateModel plate)
        {
            await AuditPlateChange(plate, "Edit");
            var request = _mapper.Map<UpdatePlateRequest>(plate);
            await _catalogService.UpdatePlate(request);
            return RedirectToAction("Index");
        }

        private async Task AuditPlateChange(PlateModel plate, string from)
        {
            var oldValue = string.Empty;
            switch (from)
            {
                case "Create":
                    oldValue = "Added";
                    break;
                default:
                    oldValue = plate.OriginalIsReserved.ToString();
                    break;
            }

            if (from == "Create" || plate.OriginalIsReserved != plate.IsReserved)
            {
                await _publishEndpoint.Publish<CreatePlateAudit>(new
                {
                    Table = "Plate",
                    TableId = plate.Id,
                    Field = "IsReserved",
                    OldValue = oldValue,
                    NewValue = plate.IsReserved,
                });
            }
        }
    }
}
