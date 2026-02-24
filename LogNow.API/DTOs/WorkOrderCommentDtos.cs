namespace LogNow.API.DTOs;

public class WorkOrderCommentDto
{
    public Guid Id { get; set; }
    public Guid WorkOrderId { get; set; }
    public Guid UserId { get; set; }
    public string UserName { get; set; } = string.Empty;
    public string CommentText { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
}

public class CreateWorkOrderCommentDto
{
    public string CommentText { get; set; } = string.Empty;
}
