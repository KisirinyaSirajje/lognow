namespace LogNow.API.Models;

public class User
{
    public Guid Id { get; set; }
    public string Username { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string PasswordHash { get; set; } = string.Empty;
    public string FullName { get; set; } = string.Empty;
    public UserRole Role { get; set; }
    public string? Team { get; set; }
    public bool IsActive { get; set; } = true;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }

    // Navigation properties
    public ICollection<Incident> CreatedIncidents { get; set; } = new List<Incident>();
    public ICollection<Incident> AssignedIncidents { get; set; } = new List<Incident>();
    public ICollection<IncidentComment> Comments { get; set; } = new List<IncidentComment>();
    public ICollection<IncidentTimeline> TimelineActions { get; set; } = new List<IncidentTimeline>();
}

public enum UserRole
{
    Admin,
    Engineer,
    TeamLead,
    Viewer
}
