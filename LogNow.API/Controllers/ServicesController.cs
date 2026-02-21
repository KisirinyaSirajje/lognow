using LogNow.API.DTOs;
using LogNow.API.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LogNow.API.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class ServicesController : ControllerBase
{
    private readonly IServiceService _serviceService;
    private readonly ILogger<ServicesController> _logger;

    public ServicesController(IServiceService serviceService, ILogger<ServicesController> logger)
    {
        _serviceService = serviceService;
        _logger = logger;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<ServiceDto>>> GetAll()
    {
        try
        {
            var services = await _serviceService.GetAllServicesAsync();
            return Ok(services);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting services");
            return StatusCode(500, new { message = "An error occurred while retrieving services" });
        }
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<ServiceDto>> GetById(Guid id)
    {
        try
        {
            var service = await _serviceService.GetServiceByIdAsync(id);
            if (service == null)
            {
                return NotFound(new { message = "Service not found" });
            }
            return Ok(service);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting service {ServiceId}", id);
            return StatusCode(500, new { message = "An error occurred while retrieving the service" });
        }
    }

    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<ServiceDto>> Create([FromBody] CreateServiceDto createServiceDto)
    {
        try
        {
            var service = await _serviceService.CreateServiceAsync(createServiceDto);
            return CreatedAtAction(nameof(GetById), new { id = service.Id }, service);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating service");
            return StatusCode(500, new { message = "An error occurred while creating the service" });
        }
    }

    [HttpPut("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<ServiceDto>> Update(Guid id, [FromBody] UpdateServiceDto updateServiceDto)
    {
        try
        {
            var service = await _serviceService.UpdateServiceAsync(id, updateServiceDto);
            return Ok(service);
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating service {ServiceId}", id);
            return StatusCode(500, new { message = "An error occurred while updating the service" });
        }
    }

    [HttpDelete("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult> Delete(Guid id)
    {
        try
        {
            await _serviceService.DeleteServiceAsync(id);
            return NoContent();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting service {ServiceId}", id);
            return StatusCode(500, new { message = "An error occurred while deleting the service" });
        }
    }
}
