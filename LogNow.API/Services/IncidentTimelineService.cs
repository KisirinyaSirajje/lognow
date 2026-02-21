using LogNow.API.DTOs;
using LogNow.API.Models;
using LogNow.API.Repositories;

namespace LogNow.API.Services;

public class IncidentTimelineService : IIncidentTimelineService
{
    private readonly IIncidentTimelineRepository _timelineRepository;

    public IncidentTimelineService(IIncidentTimelineRepository timelineRepository)
    {
        _timelineRepository = timelineRepository;
    }

    public async Task<IEnumerable<IncidentTimelineDto>> GetTimelineByIncidentIdAsync(Guid incidentId)
    {
        var timeline = await _timelineRepository.GetByIncidentIdAsync(incidentId);
        return timeline.Select(MapToTimelineDto);
    }

    public async Task AddTimelineEntryAsync(Guid incidentId, ActionType actionType, string description, Guid? userId)
    {
        var timeline = new IncidentTimeline
        {
            Id = Guid.NewGuid(),
            IncidentId = incidentId,
            ActionType = actionType,
            Description = description,
            UserId = userId,
            CreatedAt = DateTime.UtcNow
        };

        await _timelineRepository.CreateAsync(timeline);
    }

    private IncidentTimelineDto MapToTimelineDto(IncidentTimeline timeline)
    {
        return new IncidentTimelineDto
        {
            Id = timeline.Id,
            IncidentId = timeline.IncidentId,
            ActionType = timeline.ActionType.ToString(),
            Description = timeline.Description,
            UserId = timeline.UserId,
            Username = timeline.User?.Username,
            CreatedAt = timeline.CreatedAt
        };
    }
}
