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
[Route("api/incidents/{incidentId}/comments")]
public class IncidentCommentsController : ControllerBase
{
    private readonly IIncidentCommentService _commentService;
    private readonly IHubContext<NotificationHub> _hubContext;
    private readonly ILogger<IncidentCommentsController> _logger;

    public IncidentCommentsController(
        IIncidentCommentService commentService,
        IHubContext<NotificationHub> hubContext,
        ILogger<IncidentCommentsController> logger)
    {
        _commentService = commentService;
        _hubContext = hubContext;
        _logger = logger;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<IncidentCommentDto>>> GetComments(Guid incidentId)
    {
        try
        {
            var comments = await _commentService.GetCommentsByIncidentIdAsync(incidentId);
            return Ok(comments);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting comments for incident {IncidentId}", incidentId);
            return StatusCode(500, new { message = "An error occurred while retrieving comments" });
        }
    }

    [HttpPost]
    public async Task<ActionResult<IncidentCommentDto>> CreateComment(
        Guid incidentId,
        [FromBody] CreateIncidentCommentDto createCommentDto)
    {
        try
        {
            var userId = GetCurrentUserId();
            var comment = await _commentService.CreateCommentAsync(incidentId, createCommentDto, userId);

            // Notify all clients about new comment
            await _hubContext.Clients.All.SendAsync("CommentAdded", comment);

            return CreatedAtAction(nameof(GetComments), new { incidentId }, comment);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating comment for incident {IncidentId}", incidentId);
            return StatusCode(500, new { message = "An error occurred while creating the comment" });
        }
    }

    [HttpDelete("{commentId}")]
    public async Task<ActionResult> DeleteComment(Guid incidentId, Guid commentId)
    {
        try
        {
            await _commentService.DeleteCommentAsync(commentId);

            // Notify all clients about comment deletion
            await _hubContext.Clients.All.SendAsync("CommentDeleted", commentId);

            return NoContent();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting comment {CommentId}", commentId);
            return StatusCode(500, new { message = "An error occurred while deleting the comment" });
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
