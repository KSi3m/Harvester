using Harvester.Application.Interfaces.Services;
using Harvester.Application.Services;
using Harvester.Domain.Models;
using Microsoft.AspNetCore.Mvc;

namespace Harvester.API.Controllers
{
    [ApiController]
    [Route("api/[controller]s")]
    public class OrderController(IOrderService orderService): ControllerBase
    {

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var orders = await orderService.GetAll();
            return Ok(orders);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var order = await orderService.GetById(id);
            if (order == null) return NotFound();
            return Ok(order);
        }
    }
}
