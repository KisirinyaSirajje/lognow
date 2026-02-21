namespace LogNow.API.Models;

public class Incident
{
    public Guid Id { get; set; }
    public string IncidentNumber { get; set; } = string.Empty;
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    
    public Guid ServiceId { get; set; }
    public Service Service { get; set; } = null!;
    
    public Severity Severity { get; set; }
    public IncidentStatus Status { get; set; } = IncidentStatus.Pending;
    
    public Guid? AssignedToUserId { get; set; }
    public User? AssignedToUser { get; set; }
    
    public Guid? AssignedByUserId { get; set; }
    public User? AssignedByUser { get; set; }
    
    public Guid CreatedByUserId { get; set; }
    public User CreatedByUser { get; set; } = null!;
    
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }
    public DateTime? ResolvedAt { get; set; }
    public string? ResolutionNote { get; set; }
    public string? OnHoldReason { get; set; }

    // SLA tracking
    public DateTime? ResponseDueAt { get; set; }
    public DateTime? ResolutionDueAt { get; set; }
    public bool IsResponseBreached { get; set; }
    public bool IsResolutionBreached { get; set; }

    // Navigation properties
    public ICollection<IncidentComment> Comments { get; set; } = new List<IncidentComment>();
    public ICollection<IncidentTimeline> Timeline { get; set; } = new List<IncidentTimeline>();
}

public enum Severity
{
    SEV1,
    SEV2,
    SEV3,
    SEV4
}

public enum IncidentStatus
{
    Pending,
    Assigned,
    InProgress,
    OnHold,
    Resolved,
    Cancelled
}
