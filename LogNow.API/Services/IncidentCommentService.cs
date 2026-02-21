using LogNow.API.DTOs;
using LogNow.API.Models;
using LogNow.API.Repositories;

namespace LogNow.API.Services;

public class IncidentCommentService : IIncidentCommentService
{
    private readonly IIncidentCommentRepository _commentRepository;
    private readonly IIncidentTimelineService _timelineService;
    private readonly IIncidentRepository _incidentRepository;
    private readonly INotificationService _notificationService;

    public IncidentCommentService(
        IIncidentCommentRepository commentRepository,
        IIncidentTimelineService timelineService,
        IIncidentRepository incidentRepository,
        INotificationService notificationService)
    {
        _commentRepository = commentRepository;
        _timelineService = timelineService;
        _incidentRepository = incidentRepository;
        _notificationService = notificationService;
    }

    public async Task<IEnumerable<IncidentCommentDto>> GetCommentsByIncidentIdAsync(Guid incidentId)
    {
        var comments = await _commentRepository.GetByIncidentIdAsync(incidentId);
        return comments.Select(MapToCommentDto);
    }

    public async Task<IncidentCommentDto> CreateCommentAsync(Guid incidentId, CreateIncidentCommentDto createCommentDto, Guid userId)
    {
        var comment = new IncidentComment
        {
            Id = Guid.NewGuid(),
            IncidentId = incidentId,
            UserId = userId,
            CommentText = createCommentDto.CommentText,
            CreatedAt = DateTime.UtcNow
        };

        var createdComment = await _commentRepository.CreateAsync(comment);

        // Add timeline entry
        await _timelineService.AddTimelineEntryAsync(
            incidentId,
            ActionType.CommentAdded,
            "Comment added",
            userId
        );

        // Get incident to notify relevant users
        var incident = await _incidentRepository.GetByIdAsync(incidentId);
        if (incident != null)
        {
            // Notify assigned user
            if (incident.AssignedToUserId.HasValue && incident.AssignedToUserId.Value != userId)
            {
                await _notificationService.CreateNotificationAsync(
                    incident.AssignedToUserId.Value,
                    "New Comment",
                    $"New comment added to incident {incident.IncidentNumber}",
                    NotificationType.CommentAdded,
                    incident.Id.ToString()
                );
            }

            // Notify creator if different from commenter and assigned user
            if (incident.CreatedByUserId != userId && 
                incident.CreatedByUserId != incident.AssignedToUserId)
            {
                await _notificationService.CreateNotificationAsync(
                    incident.CreatedByUserId,
                    "New Comment",
                    $"New comment added to incident {incident.IncidentNumber}",
                    NotificationType.CommentAdded,
                    incident.Id.ToString()
                );
            }
        }

        return MapToCommentDto(createdComment);
    }

    public async Task DeleteCommentAsync(Guid id)
    {
        await _commentRepository.DeleteAsync(id);
    }

    private IncidentCommentDto MapToCommentDto(IncidentComment comment)
    {
        return new IncidentCommentDto
        {
            Id = comment.Id,
            IncidentId = comment.IncidentId,
            UserId = comment.UserId,
            Username = comment.User?.Username ?? string.Empty,
            UserFullName = comment.User?.FullName ?? string.Empty,
            CommentText = comment.CommentText,
            CreatedAt = comment.CreatedAt
        };
    }
}
