using Harvester.Application.Dtos;
using Harvester.Application.Interfaces.Services;
using Harvester.Application.Services;
using Microsoft.AspNetCore.Mvc;
using System.Collections;

namespace Harvester.API.Controllers
{
    [ApiController]
    [Route("api/[controller]s")]
    public class CombineController(ICombineService combineService) : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var combines = await combineService.GetAllAsync();
            return Ok(combines);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var combine = await combineService.GetByIdAsync(id);
            return Ok(combine);
        }
        [HttpPost]
        public async Task<IActionResult> Create(CreateCombineDto dto)
        {
            await combineService.CreateAsync(dto);
            return StatusCode(201);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, CreateCombineDto dto)
        {
            await combineService.UpdateAsync(id, dto);
            return StatusCode(204);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await combineService.DeleteAsync(id);
            return StatusCode(204);
        }
    }
}
