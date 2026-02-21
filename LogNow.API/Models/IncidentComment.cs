namespace LogNow.API.Models;

public class IncidentComment
{
    public Guid Id { get; set; }
    public Guid IncidentId { get; set; }
    public Incident Incident { get; set; } = null!;
    
    public Guid UserId { get; set; }
    public User User { get; set; } = null!;
    
    public string CommentText { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
