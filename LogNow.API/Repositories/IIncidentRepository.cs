using LogNow.API.Models;

namespace LogNow.API.Repositories;

public interface IIncidentRepository
{
    Task<Incident?> GetByIdAsync(Guid id);
    Task<Incident?> GetByIncidentNumberAsync(string incidentNumber);
    Task<IEnumerable<Incident>> GetAllAsync();
    Task<IEnumerable<Incident>> GetByServiceIdAsync(Guid serviceId);
    Task<IEnumerable<Incident>> GetByAssignedUserIdAsync(Guid userId);
    Task<IEnumerable<Incident>> GetByCreatedUserIdAsync(Guid userId);
    Task<Incident> CreateAsync(Incident incident);
    Task<Incident> UpdateAsync(Incident incident);
    Task DeleteAsync(Guid id);
    Task<string> GenerateIncidentNumberAsync();
}
