namespace LogNow.API.DTOs;

public class IncidentDto
{
    public Guid Id { get; set; }
    public string IncidentNumber { get; set; } = string.Empty;
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public Guid ServiceId { get; set; }
    public string ServiceName { get; set; } = string.Empty;
    public string Severity { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
    public Guid? AssignedToUserId { get; set; }
    public string? AssignedToUserName { get; set; }
    public Guid? AssignedByUserId { get; set; }
    public string? AssignedByUserName { get; set; }
    public Guid CreatedByUserId { get; set; }
    public string CreatedByUserName { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public DateTime? ResolvedAt { get; set; }
    public DateTime? ResponseDueAt { get; set; }
    public DateTime? ResolutionDueAt { get; set; }
    public bool IsResponseBreached { get; set; }
    public bool IsResolutionBreached { get; set; }
    public string? ResolutionNote { get; set; }
    public string? OnHoldReason { get; set; }
}

public class CreateIncidentDto
{
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public Guid ServiceId { get; set; }
    public string Severity { get; set; } = "SEV3";
}

public class UpdateIncidentDto
{
    public string? Title { get; set; }
    public string? Description { get; set; }
    public string? Severity { get; set; }
    public string? Status { get; set; }
    public Guid? AssignedToUserId { get; set; }
}

public class AssignIncidentDto
{
    public Guid UserId { get; set; }
}

public class UpdateIncidentStatusDto
{
    public string Status { get; set; } = string.Empty;
    public string? ResolutionNote { get; set; }
    public string? OnHoldReason { get; set; }
}
