namespace LogNow.API.Models;

public class IncidentTimeline
{
    public Guid Id { get; set; }
    public Guid IncidentId { get; set; }
    public Incident Incident { get; set; } = null!;
    
    public ActionType ActionType { get; set; }
    public string Description { get; set; } = string.Empty;
    
    public Guid? UserId { get; set; }
    public User? User { get; set; }
    
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}

public enum ActionType
{
    Created,
    Assigned,
    StatusChanged,
    CommentAdded,
    SeverityChanged,
    Resolved,
    Closed,
    Reopened
}
