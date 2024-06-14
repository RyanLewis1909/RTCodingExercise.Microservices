using Catalog.API.Messages.Request;
using Catalog.API.Messages.Response;

namespace Catalog.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PlateController : Controller
    {
        private readonly IPlateRepository _plateRepository;

        public PlateController(IPlateRepository plateRepository)
        {
            _plateRepository = plateRepository;
        }

        [HttpPost]
        public async Task<ActionResult> CreatePlate([FromQuery] CreatePlateRequest createPlateRequest)
        {
            try
            {
                await _plateRepository.CreatePlate(createPlateRequest);
            }
            catch (Exception)
            {
                return BadRequest("Failed to create new plate");
            }

            return NoContent();
        }

        [HttpGet("GetPlateItems")]
        public async Task<ActionResult<GetPlateItemsResult>> GetPlateItems([FromQuery] GetPlateItemsRequest catalogItemsRequest)
        {
            var plates = await _plateRepository.GetPlates(catalogItemsRequest.PageNumber, catalogItemsRequest.SortOrder, catalogItemsRequest.SearchString, catalogItemsRequest.CurrentFilter);
            if (plates == null)
            {
                return NotFound();
            }

            return new GetPlateItemsResult { 
                Plates = plates, 
                PageIndex = plates.PageIndex,
                TotalPages = plates.TotalPages
            };
        }
    }
}
