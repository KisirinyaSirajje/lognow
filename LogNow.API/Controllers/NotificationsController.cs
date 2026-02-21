using LogNow.API.DTOs;
using LogNow.API.Models;
using LogNow.API.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace LogNow.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class NotificationsController : ControllerBase
{
    private readonly INotificationService _notificationService;

    public NotificationsController(INotificationService notificationService)
    {
        _notificationService = notificationService;
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
    public async Task<ActionResult<IEnumerable<NotificationDto>>> GetNotifications()
    {
        var userId = GetCurrentUserId();
        var notifications = await _notificationService.GetUserNotificationsAsync(userId);
        return Ok(notifications.Select(MapToDto));
    }

    [HttpGet("unread")]
    public async Task<ActionResult<IEnumerable<NotificationDto>>> GetUnreadNotifications()
    {
        var userId = GetCurrentUserId();
        var notifications = await _notificationService.GetUnreadNotificationsAsync(userId);
        return Ok(notifications.Select(MapToDto));
    }

    [HttpGet("unread/count")]
    public async Task<ActionResult<int>> GetUnreadCount()
    {
        var userId = GetCurrentUserId();
        var count = await _notificationService.GetUnreadCountAsync(userId);
        return Ok(count);
    }

    [HttpPut("{id}/read")]
    public async Task<IActionResult> MarkAsRead(Guid id)
    {
        await _notificationService.MarkAsReadAsync(id);
        return NoContent();
    }

    [HttpPut("read-all")]
    public async Task<IActionResult> MarkAllAsRead()
    {
        var userId = GetCurrentUserId();
        await _notificationService.MarkAllAsReadAsync(userId);
        return NoContent();
    }

    private NotificationDto MapToDto(Notification notification)
    {
        return new NotificationDto
        {
            Id = notification.Id,
            UserId = notification.UserId,
            Title = notification.Title,
            Message = notification.Message,
            Type = notification.Type.ToString(),
            RelatedEntityId = notification.RelatedEntityId,
            IsRead = notification.IsRead,
            CreatedAt = notification.CreatedAt
        };
    }
}
