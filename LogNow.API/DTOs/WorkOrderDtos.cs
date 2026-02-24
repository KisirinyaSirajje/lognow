namespace LogNow.API.DTOs;

public class WorkOrderDto
{
    public Guid Id { get; set; }
    public string WorkOrderNumber { get; set; } = string.Empty;
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string Type { get; set; } = string.Empty;
    public string Priority { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
    
    public Guid? ServiceId { get; set; }
    public string? ServiceName { get; set; }
    
    public Guid? IncidentId { get; set; }
    public string? IncidentNumber { get; set; }
    
    public Guid? AssignedToUserId { get; set; }
    public string? AssignedToUserName { get; set; }
    
    public string? AssignedGroup { get; set; }
    
    public Guid CreatedByUserId { get; set; }
    public string CreatedByUserName { get; set; } = string.Empty;
    
    public DateTime? ScheduledStartDate { get; set; }
    public DateTime? ScheduledEndDate { get; set; }
    public DateTime? ActualStartDate { get; set; }
    public DateTime? ActualEndDate { get; set; }
    
    public decimal? EstimatedCost { get; set; }
    public decimal? ActualCost { get; set; }
    public decimal? EstimatedHours { get; set; }
    public decimal? ActualHours { get; set; }
    
    public string? Location { get; set; }
    public string? PartsRequired { get; set; }
    public string? CompletionNotes { get; set; }
    
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public DateTime? CompletedAt { get; set; }
}

public class CreateWorkOrderDto
{
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string Type { get; set; } = string.Empty;
    public string Priority { get; set; } = string.Empty;
    public Guid? ServiceId { get; set; }
    public Guid? IncidentId { get; set; }
    public string? Location { get; set; }
    public DateTime? ScheduledStartDate { get; set; }
    public DateTime? ScheduledEndDate { get; set; }
    public decimal? EstimatedCost { get; set; }
    public decimal? EstimatedHours { get; set; }
    public string? PartsRequired { get; set; }
}

public class UpdateWorkOrderDto
{
    public string? Title { get; set; }
    public string? Description { get; set; }
    public string? Priority { get; set; }
    public string? Status { get; set; }
    public Guid? AssignedToUserId { get; set; }
    public string? AssignedGroup { get; set; }
    public DateTime? ScheduledStartDate { get; set; }
    public DateTime? ScheduledEndDate { get; set; }
    public DateTime? ActualStartDate { get; set; }
    public DateTime? ActualEndDate { get; set; }
    public decimal? ActualCost { get; set; }
    public decimal? ActualHours { get; set; }
    public string? Location { get; set; }
    public string? PartsRequired { get; set; }
    public string? CompletionNotes { get; set; }
}

public class AssignWorkOrderDto
{
    public string? AssignedGroup { get; set; }
    public Guid? UserId { get; set; }
}
