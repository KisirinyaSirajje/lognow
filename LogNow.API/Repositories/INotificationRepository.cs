using LogNow.API.Models;

namespace LogNow.API.Repositories;

public interface INotificationRepository
{
    Task<Notification> GetByIdAsync(Guid id);
    Task<IEnumerable<Notification>> GetByUserIdAsync(Guid userId);
    Task<IEnumerable<Notification>> GetUnreadByUserIdAsync(Guid userId);
    Task<Notification> CreateAsync(Notification notification);
    Task UpdateAsync(Notification notification);
    Task DeleteAsync(Guid id);
    Task<int> GetUnreadCountAsync(Guid userId);
    Task MarkAsReadAsync(Guid id);
    Task MarkAllAsReadAsync(Guid userId);
}
