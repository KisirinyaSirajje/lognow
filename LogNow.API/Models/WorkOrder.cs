namespace LogNow.API.Models;

public class WorkOrder
{
    public Guid Id { get; set; }
    public string WorkOrderNumber { get; set; } = string.Empty;
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public WorkOrderType Type { get; set; }
    public WorkOrderPriority Priority { get; set; }
    public WorkOrderStatus Status { get; set; }
    
    // Relations
    public Guid? ServiceId { get; set; }
    public Service? Service { get; set; }
    
    public Guid? IncidentId { get; set; }
    public Incident? Incident { get; set; }
    
    public Guid? AssignedToUserId { get; set; }
    public User? AssignedToUser { get; set; }
    
    public string? AssignedGroup { get; set; }
    
    public Guid CreatedByUserId { get; set; }
    public User CreatedByUser { get; set; } = null!;
    
    // Scheduling
    public DateTime? ScheduledStartDate { get; set; }
    public DateTime? ScheduledEndDate { get; set; }
    public DateTime? ActualStartDate { get; set; }
    public DateTime? ActualEndDate { get; set; }
    
    // Cost tracking
    public decimal? EstimatedCost { get; set; }
    public decimal? ActualCost { get; set; }
    public decimal? EstimatedHours { get; set; }
    public decimal? ActualHours { get; set; }
    
    // Additional details
    public string? Location { get; set; }
    public string? PartsRequired { get; set; }
    public string? CompletionNotes { get; set; }
    
    // Timestamps
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public DateTime? CompletedAt { get; set; }
    
    // Navigation properties
    public ICollection<WorkOrderComment> Comments { get; set; } = new List<WorkOrderComment>();
}

public enum WorkOrderType
{
    Corrective,      // Fix something broken
    Preventive,      // Scheduled maintenance
    Inspection,      // Check/inspect equipment
    Installation,    // Install new equipment
    Upgrade,         // Upgrade existing equipment
    Emergency        // Urgent repair
}

public enum WorkOrderPriority
{
    Low,
    Medium,
    High,
    Critical
}

public enum WorkOrderStatus
{
    Draft,           // Being created
    Scheduled,       // Planned for future
    Assigned,        // Assigned to someone
    InProgress,      // Work started
    OnHold,          // Paused
    Completed,       // Work done
    Verified,        // Verified by supervisor
    Cancelled        // Cancelled
}
