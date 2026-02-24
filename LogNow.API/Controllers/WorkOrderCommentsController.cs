using LogNow.API.DTOs;
using LogNow.API.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using LogNow.API.Hubs;
using System.Security.Claims;

namespace LogNow.API.Controllers;

[ApiController]
[Route("api/workorders/{workOrderId}/comments")]
[Authorize]
public class WorkOrderCommentsController : ControllerBase
{
    private readonly IWorkOrderCommentService _commentService;
    private readonly IHubContext<NotificationHub> _hubContext;
    private readonly ILogger<WorkOrderCommentsController> _logger;

    public WorkOrderCommentsController(
        IWorkOrderCommentService commentService,
        IHubContext<NotificationHub> hubContext,
        ILogger<WorkOrderCommentsController> logger)
    {
        _commentService = commentService;
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
    public async Task<ActionResult<IEnumerable<WorkOrderCommentDto>>> GetComments(Guid workOrderId)
    {
        try
        {
            var comments = await _commentService.GetCommentsByWorkOrderIdAsync(workOrderId);
            return Ok(comments);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting comments for work order {WorkOrderId}", workOrderId);
            return StatusCode(500, new { message = "An error occurred while fetching comments" });
        }
    }

    [HttpPost]
    public async Task<ActionResult<WorkOrderCommentDto>> AddComment(
        Guid workOrderId,
        [FromBody] CreateWorkOrderCommentDto createDto)
    {
        try
        {
            var userId = GetCurrentUserId();
            var comment = await _commentService.AddCommentAsync(workOrderId, createDto, userId);

            // Broadcast comment to all connected clients
            await _hubContext.Clients.All.SendAsync("WorkOrderCommentAdded", comment);

            return CreatedAtAction(nameof(GetComments), new { workOrderId }, comment);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error adding comment to work order {WorkOrderId}", workOrderId);
            return StatusCode(500, new { message = "An error occurred while adding the comment" });
        }
    }
}
