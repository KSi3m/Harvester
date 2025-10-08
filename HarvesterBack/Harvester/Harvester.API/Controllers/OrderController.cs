using Harvester.Application.Dtos;
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
            var orders = await orderService.GetAllAsync();
            return Ok(orders);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var order = await orderService.GetByIdAsync(id);
            if (order == null) return NotFound();
            return Ok(order);
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateOrderDto dto)
        {
            var res = await orderService.CreateAsync(dto);
            if(res.Success)
            {
                return StatusCode(201, res);
            }
            return StatusCode(422, res);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, CreateOrderDto dto)
        {
            var res = await orderService.UpdateAsync(id, dto);
            if (res.Success)
            {
                return StatusCode(200, res);
            }
            return StatusCode(422, res);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await orderService.DeleteAsync(id);
            return StatusCode(204);
        }
    }
}
