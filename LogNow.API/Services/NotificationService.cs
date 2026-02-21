using LogNow.API.Models;
using LogNow.API.Repositories;

namespace LogNow.API.Services;

public interface INotificationService
{
    Task<Notification> CreateNotificationAsync(Guid userId, string title, string message, NotificationType type, string? relatedEntityId = null);
    Task<IEnumerable<Notification>> GetUserNotificationsAsync(Guid userId);
    Task<IEnumerable<Notification>> GetUnreadNotificationsAsync(Guid userId);
    Task<int> GetUnreadCountAsync(Guid userId);
    Task MarkAsReadAsync(Guid notificationId);
    Task MarkAllAsReadAsync(Guid userId);
}

public class NotificationService : INotificationService
{
    private readonly INotificationRepository _notificationRepository;

    public NotificationService(INotificationRepository notificationRepository)
    {
        _notificationRepository = notificationRepository;
    }

    public async Task<Notification> CreateNotificationAsync(Guid userId, string title, string message, NotificationType type, string? relatedEntityId = null)
    {
        var notification = new Notification
        {
            Id = Guid.NewGuid(),
            UserId = userId,
            Title = title,
            Message = message,
            Type = type,
            RelatedEntityId = relatedEntityId,
            IsRead = false,
            CreatedAt = DateTime.UtcNow
        };

        return await _notificationRepository.CreateAsync(notification);
    }

    public async Task<IEnumerable<Notification>> GetUserNotificationsAsync(Guid userId)
    {
        return await _notificationRepository.GetByUserIdAsync(userId);
    }

    public async Task<IEnumerable<Notification>> GetUnreadNotificationsAsync(Guid userId)
    {
        return await _notificationRepository.GetUnreadByUserIdAsync(userId);
    }

    public async Task<int> GetUnreadCountAsync(Guid userId)
    {
        return await _notificationRepository.GetUnreadCountAsync(userId);
    }

    public async Task MarkAsReadAsync(Guid notificationId)
    {
        await _notificationRepository.MarkAsReadAsync(notificationId);
    }

    public async Task MarkAllAsReadAsync(Guid userId)
    {
        await _notificationRepository.MarkAllAsReadAsync(userId);
    }
}
