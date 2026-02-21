using System.Security.Claims;
using LogNow.API.DTOs;
using LogNow.API.Hubs;
using LogNow.API.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;

namespace LogNow.API.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class IncidentsController : ControllerBase
{
    private readonly IIncidentService _incidentService;
    private readonly IHubContext<NotificationHub> _hubContext;
    private readonly ILogger<IncidentsController> _logger;

    public IncidentsController(
        IIncidentService incidentService,
        IHubContext<NotificationHub> hubContext,
        ILogger<IncidentsController> logger)
    {
        _incidentService = incidentService;
        _hubContext = hubContext;
        _logger = logger;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<IncidentDto>>> GetAll()
    {
        try
        {
            var incidents = await _incidentService.GetAllIncidentsAsync();
            return Ok(incidents);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting incidents");
            return StatusCode(500, new { message = "An error occurred while retrieving incidents" });
        }
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<IncidentDto>> GetById(Guid id)
    {
        try
        {
            var incident = await _incidentService.GetIncidentByIdAsync(id);
            if (incident == null)
            {
                return NotFound(new { message = "Incident not found" });
            }
            return Ok(incident);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting incident {IncidentId}", id);
            return StatusCode(500, new { message = "An error occurred while retrieving the incident" });
        }
    }

    [HttpPost]
    public async Task<ActionResult<IncidentDto>> Create([FromBody] CreateIncidentDto createIncidentDto)
    {
        try
        {
            var userId = GetCurrentUserId();
            var incident = await _incidentService.CreateIncidentAsync(createIncidentDto, userId);

            // Notify all clients about new incident
            await _hubContext.Clients.All.SendAsync("IncidentCreated", incident);

            return CreatedAtAction(nameof(GetById), new { id = incident.Id }, incident);
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating incident");
            return StatusCode(500, new { message = "An error occurred while creating the incident" });
        }
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<IncidentDto>> Update(Guid id, [FromBody] UpdateIncidentDto updateIncidentDto)
    {
        try
        {
            var userId = GetCurrentUserId();
            var incident = await _incidentService.UpdateIncidentAsync(id, updateIncidentDto, userId);

            // Notify all clients about incident update
            await _hubContext.Clients.All.SendAsync("IncidentUpdated", incident);

            return Ok(incident);
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating incident {IncidentId}", id);
            return StatusCode(500, new { message = "An error occurred while updating the incident" });
        }
    }

    [HttpPut("{id}/assign")]
    [Authorize(Roles = "Admin,TeamLead")]
    public async Task<ActionResult<IncidentDto>> Assign(Guid id, [FromBody] AssignIncidentDto assignIncidentDto)
    {
        try
        {
            var userId = GetCurrentUserId();
            var incident = await _incidentService.AssignIncidentAsync(id, assignIncidentDto, userId);

            // Notify all clients about incident assignment
            await _hubContext.Clients.All.SendAsync("IncidentUpdated", incident);

            return Ok(incident);
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error assigning incident {IncidentId}", id);
            return StatusCode(500, new { message = "An error occurred while assigning the incident" });
        }
    }

    [HttpPut("{id}/status")]
    public async Task<ActionResult<IncidentDto>> UpdateStatus(Guid id, [FromBody] UpdateIncidentStatusDto updateStatusDto)
    {
        try
        {
            var userId = GetCurrentUserId();
            var incident = await _incidentService.UpdateIncidentStatusAsync(id, updateStatusDto, userId);

            // Notify all clients about status update
            await _hubContext.Clients.All.SendAsync("IncidentUpdated", incident);

            return Ok(incident);
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new { message = ex.Message });
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating incident status {IncidentId}", id);
            return StatusCode(500, new { message = "An error occurred while updating the incident status" });
        }
    }

    [HttpDelete("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult> Delete(Guid id)
    {
        try
        {
            await _incidentService.DeleteIncidentAsync(id);

            // Notify all clients about incident deletion
            await _hubContext.Clients.All.SendAsync("IncidentDeleted", id);

            return NoContent();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting incident {IncidentId}", id);
            return StatusCode(500, new { message = "An error occurred while deleting the incident" });
        }
    }

    private Guid GetCurrentUserId()
    {
        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value 
            ?? User.FindFirst("sub")?.Value;
        
        if (string.IsNullOrEmpty(userIdClaim) || !Guid.TryParse(userIdClaim, out var userId))
        {
            throw new UnauthorizedAccessException("User ID not found in token");
        }

        return userId;
    }
}
