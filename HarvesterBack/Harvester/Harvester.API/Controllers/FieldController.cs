using Harvester.Application.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;

namespace Harvester.API.Controllers
{
    [ApiController]
    [Route("api/[controller]s")]
    public class FieldController(IFieldService fieldService, IOnGeoService onGeoService) :ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var fields = await fieldService.GetAll();
            return Ok(fields);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var field = await fieldService.GetById(id);
            if (field == null) return NotFound();
            return Ok(field);
        }

        [HttpGet("{nameIdentifier}/area")]
        public async Task<IActionResult> GetArea(string nameIdentifier)
        {
            var area = await onGeoService.GetDataAsync(nameIdentifier);
            return Ok(area);
        }
    }
}
