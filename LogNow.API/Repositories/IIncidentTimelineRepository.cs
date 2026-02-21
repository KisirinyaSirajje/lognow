using LogNow.API.Models;

namespace LogNow.API.Repositories;

public interface IIncidentTimelineRepository
{
    Task<IEnumerable<IncidentTimeline>> GetByIncidentIdAsync(Guid incidentId);
    Task<IncidentTimeline> CreateAsync(IncidentTimeline timeline);
}
