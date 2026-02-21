namespace LogNow.API.DTOs;

public class IncidentTimelineDto
{
    public Guid Id { get; set; }
    public Guid IncidentId { get; set; }
    public string ActionType { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public Guid? UserId { get; set; }
    public string? Username { get; set; }
    public DateTime CreatedAt { get; set; }
}
