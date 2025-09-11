using Harvester.API.Filters;
using Harvester.Application.Dtos;
using Harvester.Application.Exceptions;
using Harvester.Application.Interfaces.Services;
using Humanizer;
using Microsoft.AspNetCore.Mvc;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Harvester.API.Controllers
{
    [ApiController]
    [Route("api/[controller]s")]
    public class FieldController(IFieldService fieldService, IOnGeoService onGeoService) : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var fields = await fieldService.GetAllAsync();
            return Ok(fields);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var field = await fieldService.GetByIdAsync(id);
            return Ok(field);
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateFieldDto dto)
        {
            await fieldService.CreateAsync(dto);
            return StatusCode(201);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, CreateFieldDto dto)
        {
            await fieldService.UpdateAsync(id,dto);
            return StatusCode(204);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await fieldService.DeleteAsync(id);
            return StatusCode(204);
        }

        [HttpGet("{nameIdentifier}/geoJsonData")]
        [TypeFilter(typeof(AreaRouteParameterFilter))]
        public async Task<IActionResult> GetGeoJsonData(string nameIdentifier)
        {
            var response = await onGeoService.GetDataAsync(nameIdentifier);
            return Ok(response);
        }
    }
}
