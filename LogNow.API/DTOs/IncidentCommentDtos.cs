namespace LogNow.API.DTOs;

public class IncidentCommentDto
{
    public Guid Id { get; set; }
    public Guid IncidentId { get; set; }
    public Guid UserId { get; set; }
    public string Username { get; set; } = string.Empty;
    public string UserFullName { get; set; } = string.Empty;
    public string CommentText { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
}

public class CreateIncidentCommentDto
{
    public string CommentText { get; set; } = string.Empty;
}
