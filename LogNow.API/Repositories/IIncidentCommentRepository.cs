using LogNow.API.Models;

namespace LogNow.API.Repositories;

public interface IIncidentCommentRepository
{
    Task<IncidentComment?> GetByIdAsync(Guid id);
    Task<IEnumerable<IncidentComment>> GetByIncidentIdAsync(Guid incidentId);
    Task<IncidentComment> CreateAsync(IncidentComment comment);
    Task DeleteAsync(Guid id);
}
