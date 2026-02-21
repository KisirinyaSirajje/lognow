using LogNow.API.DTOs;
using LogNow.API.Models;
using LogNow.API.Repositories;

namespace LogNow.API.Services;

public class IncidentCommentService : IIncidentCommentService
{
    private readonly IIncidentCommentRepository _commentRepository;
    private readonly IIncidentTimelineService _timelineService;

    public IncidentCommentService(
        IIncidentCommentRepository commentRepository,
        IIncidentTimelineService timelineService)
    {
        _commentRepository = commentRepository;
        _timelineService = timelineService;
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
