using Harvester.Application.Interfaces.Services;
using Harvester.Domain.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections;

namespace Harvester.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CombineController(ICombineService combineService) : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var combines = await combineService.GetAll();
            return Ok(combines);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var combine = await combineService.GetById(id);
            if(combine == null) return NotFound();
            return Ok(combine);
        }
    }
}
