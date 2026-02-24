namespace LogNow.API.Models;

public class WorkOrderComment
{
    public Guid Id { get; set; }
    public Guid WorkOrderId { get; set; }
    public Guid UserId { get; set; }
    public string CommentText { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }

    // Navigation properties
    public WorkOrder WorkOrder { get; set; } = null!;
    public User User { get; set; } = null!;
}
