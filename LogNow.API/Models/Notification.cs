namespace LogNow.API.Models;

public class Notification
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public User User { get; set; } = null!;
    public string Title { get; set; } = string.Empty;
    public string Message { get; set; } = string.Empty;
    public NotificationType Type { get; set; }
    public string? RelatedEntityId { get; set; } // Incident ID, Comment ID, etc.
    public bool IsRead { get; set; }
    public DateTime CreatedAt { get; set; }
}

public enum NotificationType
{
    IncidentAssigned,
    IncidentUpdated,
    IncidentStatusChanged,
    CommentAdded,
    IncidentEscalated,
    SLABreach
}
