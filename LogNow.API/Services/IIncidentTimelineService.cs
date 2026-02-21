using LogNow.API.DTOs;
using LogNow.API.Models;

namespace LogNow.API.Services;

public interface IIncidentTimelineService
{
    Task<IEnumerable<IncidentTimelineDto>> GetTimelineByIncidentIdAsync(Guid incidentId);
    Task AddTimelineEntryAsync(Guid incidentId, ActionType actionType, string description, Guid? userId);
}
