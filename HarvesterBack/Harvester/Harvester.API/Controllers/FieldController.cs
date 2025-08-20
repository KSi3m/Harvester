using Harvester.Application.Dtos;
using Harvester.Application.Exceptions;
using Harvester.Application.Interfaces.Services;
using Humanizer;
using Microsoft.AspNetCore.Mvc;

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
            if (field == null) return NotFound();
            return Ok(field);
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateFieldDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                await fieldService.CreateAsync(dto);
                return StatusCode(201);
            }
            catch
            {
                return StatusCode(500);
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, CreateFieldDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                await fieldService.UpdateAsync(id,dto);
                return StatusCode(204);
            }
            catch (NotFoundException ex)
            {
                return NotFound();
            }
            catch
            {
                return StatusCode(500);
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                await fieldService.DeleteAsync(id);
                return StatusCode(204);
            }
            catch(NotFoundException ex)
            {
                return NotFound();
            }
            catch
            {
                return StatusCode(500);
            }
        }


        [HttpGet("{nameIdentifier}/area")]
        public async Task<IActionResult> GetArea(string nameIdentifier)
        {
            try
            {
                var area = await onGeoService.GetDataAsync(nameIdentifier);
                return Ok(new { areaInHectares = area});
            }
            catch(PlotNotFoundOnOnGeoUtility ex)
            {
                return NotFound();
            }
            catch
            {
                return StatusCode(500);
            }

        }
    }
}
