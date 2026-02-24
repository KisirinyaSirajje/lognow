using LogNow.API.DTOs;
using LogNow.API.Models;
using LogNow.API.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using LogNow.API.Hubs;
using System.Security.Claims;

namespace LogNow.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class WorkOrdersController : ControllerBase
{
    private readonly IWorkOrderService _workOrderService;
    private readonly IHubContext<NotificationHub> _hubContext;
    private readonly ILogger<WorkOrdersController> _logger;

    public WorkOrdersController(
        IWorkOrderService workOrderService,
        IHubContext<NotificationHub> hubContext,
        ILogger<WorkOrdersController> logger)
    {
        _workOrderService = workOrderService;
        _hubContext = hubContext;
        _logger = logger;
    }

    private Guid GetCurrentUserId()
    {
        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (string.IsNullOrEmpty(userIdClaim) || !Guid.TryParse(userIdClaim, out var userId))
        {
            throw new UnauthorizedAccessException("Invalid user ID");
        }
        return userId;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<WorkOrderDto>>> GetAll()
    {
        var workOrders = await _workOrderService.GetAllWorkOrdersAsync();
        return Ok(workOrders);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<WorkOrderDto>> GetById(Guid id)
    {
        var workOrder = await _workOrderService.GetWorkOrderByIdAsync(id);
        if (workOrder == null)
        {
            return NotFound(new { message = "Work order not found" });
        }
        return Ok(workOrder);
    }

    [HttpGet("status/{status}")]
    public async Task<ActionResult<IEnumerable<WorkOrderDto>>> GetByStatus(string status)
    {
        if (!Enum.TryParse<WorkOrderStatus>(status, true, out var workOrderStatus))
        {
            return BadRequest(new { message = "Invalid status" });
        }

        var workOrders = await _workOrderService.GetByStatusAsync(workOrderStatus);
        return Ok(workOrders);
    }

    [HttpGet("assigned-to-me")]
    public async Task<ActionResult<IEnumerable<WorkOrderDto>>> GetAssignedToMe()
    {
        var userId = GetCurrentUserId();
        var workOrders = await _workOrderService.GetByAssignedUserIdAsync(userId);
        return Ok(workOrders);
    }

    [HttpGet("group/{group}")]
    public async Task<ActionResult<IEnumerable<WorkOrderDto>>> GetByGroup(string group)
    {
        var workOrders = await _workOrderService.GetByAssignedGroupAsync(group);
        return Ok(workOrders);
    }

    [HttpPost]
    [Authorize(Roles = "Admin,TeamLead,Engineer")]
    public async Task<ActionResult<WorkOrderDto>> Create([FromBody] CreateWorkOrderDto createWorkOrderDto)
    {
        try
        {
            var userId = GetCurrentUserId();
            var workOrder = await _workOrderService.CreateWorkOrderAsync(createWorkOrderDto, userId);

            // Notify all clients about new work order
            await _hubContext.Clients.All.SendAsync("WorkOrderCreated", workOrder);

            return CreatedAtAction(nameof(GetById), new { id = workOrder.Id }, workOrder);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating work order");
            return StatusCode(500, new { message = "An error occurred while creating the work order" });
        }
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<WorkOrderDto>> Update(Guid id, [FromBody] UpdateWorkOrderDto updateWorkOrderDto)
    {
        try
        {
            var workOrder = await _workOrderService.UpdateWorkOrderAsync(id, updateWorkOrderDto);

            // Notify all clients about work order update
            await _hubContext.Clients.All.SendAsync("WorkOrderUpdated", workOrder);

            return Ok(workOrder);
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
            _logger.LogError(ex, "Error updating work order {WorkOrderId}", id);
            return StatusCode(500, new { message = "An error occurred while updating the work order" });
        }
    }

    [HttpPut("{id}/assign")]
    [Authorize(Roles = "Admin,TeamLead,Engineer")]
    public async Task<ActionResult<WorkOrderDto>> Assign(Guid id, [FromBody] AssignWorkOrderDto assignDto)
    {
        try
        {
            var userId = GetCurrentUserId();
            var workOrder = await _workOrderService.AssignWorkOrderAsync(id, assignDto, userId);

            // Notify all clients about work order assignment
            await _hubContext.Clients.All.SendAsync("WorkOrderUpdated", workOrder);

            return Ok(workOrder);
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error assigning work order {WorkOrderId}", id);
            return StatusCode(500, new { message = "An error occurred while assigning the work order" });
        }
    }

    [HttpDelete("{id}")]
    [Authorize(Roles = "Admin,TeamLead")]
    public async Task<IActionResult> Delete(Guid id)
    {
        try
        {
            await _workOrderService.DeleteWorkOrderAsync(id);
            return NoContent();
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting work order {WorkOrderId}", id);
            return StatusCode(500, new { message = "An error occurred while deleting the work order" });
        }
    }
}
