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

        [HttpGet("GetPlateItems")]
        public async Task<ActionResult<GetPlateItemsResult>> GetPlateItems([FromQuery] GetPlateItemsRequest catalogItemsRequest)
        {
            var plates = await _plateRepository.GetPlates(catalogItemsRequest.PageNumber, catalogItemsRequest.SortOrder, catalogItemsRequest.SearchString, catalogItemsRequest.CurrentFilter);
            if (plates == null)
            {
                return NotFound();
            }

            return new GetPlateItemsResult
            {
                Plates = plates,
                PageIndex = plates.PageIndex,
                TotalPages = plates.TotalPages,
                TotalSold = plates.TotalSold
            };
        }

        [HttpGet("GetPlate")]
        public async Task<ActionResult<Plate>> GetPlateItems([FromQuery] Guid id)
        {
            var plate = await _plateRepository.GetPlate(id);
            if (plate == null)
            {
                return NotFound();
            }

            return plate;
        }

        [HttpPost("Create")]
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

        [HttpPut("Update")]
        public async Task<ActionResult> UpdatePlate([FromQuery] UpdatePlateRequest updatePlateRequest)
        {
            try
            {
                await _plateRepository.UpdatePlate(updatePlateRequest);
            }
            catch (Exception)
            {
                return BadRequest("Failed to create new plate");
            }

            return NoContent();
        }
    }
}
