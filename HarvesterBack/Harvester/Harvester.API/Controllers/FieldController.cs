using Harvester.API.Application.ErrorResponse;
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
            if (field == null)
            {
                var errorResponse = new ErrorResponse()
                {
                    Status = 404,
                    Message = "Field not found",
                    Details = $"No area found for identifier {id}"
                };
                return NotFound(errorResponse);
            }
            return Ok(field);
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateFieldDto dto)
        {
            if (!ModelState.IsValid)
            {
                var errorResponse = new ErrorResponse()
                {
                    Status = 400,
                    Message = "Validation Errors",
                    Details = string.Join("; ",
                        ModelState
                            .Where(ms => ms.Value.Errors.Any())
                            .SelectMany(ms => ms.Value.Errors.Select(e =>
                                $"{ms.Key}: {e.ErrorMessage}"))
                    )
                };
                return BadRequest(errorResponse);
            }
            try
            {
                await fieldService.CreateAsync(dto);
                return StatusCode(201);
            }
            catch
            {
                var errorResponse = new ErrorResponse()
                {
                    Status = 500,
                    Message = "Server error",
                    Details = "Error occured"
                };
                return StatusCode(StatusCodes.Status500InternalServerError, errorResponse);
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, CreateFieldDto dto)
        {
            if (!ModelState.IsValid)
            {
                var errorResponse = new ErrorResponse()
                {
                    Status = 400,
                    Message = "Validation Errors",
                    Details = string.Join("; ",
                        ModelState
                            .Where(ms => ms.Value.Errors.Any())
                            .SelectMany(ms => ms.Value.Errors.Select(e =>
                                $"{ms.Key}: {e.ErrorMessage}"))
                    )
                };
                return BadRequest(errorResponse);
            }
            try
            {
                await fieldService.UpdateAsync(id,dto);
                return StatusCode(204);
            }
            catch (NotFoundException ex)
            {
                var errorResponse = new ErrorResponse()
                {
                    Status = 404,
                    Message = "Field not found",
                    Details = $"No area found for identifier {id}"
                };
                return NotFound(errorResponse);
            }
            catch
            {
                var errorResponse = new ErrorResponse()
                {
                    Status = 500,
                    Message = "Server error",
                    Details = "Error occured"
                };
                return StatusCode(StatusCodes.Status500InternalServerError, errorResponse);
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            if (!ModelState.IsValid)
            {
                var errorResponse = new ErrorResponse()
                {
                    Status = 400,
                    Message = "Validation Errors",
                    Details = string.Join("; ",
                      ModelState
                          .Where(ms => ms.Value.Errors.Any())
                          .SelectMany(ms => ms.Value.Errors.Select(e =>
                              $"{ms.Key}: {e.ErrorMessage}"))
                  )
                };
                return BadRequest(errorResponse);
            }
            try
            {
                await fieldService.DeleteAsync(id);
                return StatusCode(204);
            }
            catch(NotFoundException ex)
            {
                var errorResponse = new ErrorResponse()
                {
                    Status = 404,
                    Message = "Field not found",
                    Details = $"No area found for identifier {id}"
                };
                return NotFound(errorResponse);
            }
            catch
            {
                var errorResponse = new ErrorResponse()
                {
                    Status = 500,
                    Message = "Server error",
                    Details = "Error occured"
                };
                return StatusCode(StatusCodes.Status500InternalServerError, errorResponse);
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
                var errorResponse = new ErrorResponse()
                {
                    Status = 404,
                    Message = "Plot was not found",
                    Details = $"No area found for identifier {nameIdentifier}"
                };
                return NotFound(errorResponse);
            }
            catch
            {
                var errorResponse = new ErrorResponse()
                {
                    Status = 500,
                    Message = "Server error",
                    Details = "Error occured"
                };
                return StatusCode(StatusCodes.Status500InternalServerError, errorResponse);
            }

        }
    }
}
