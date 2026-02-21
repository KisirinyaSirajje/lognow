using LogNow.API.DTOs;

namespace LogNow.API.Services;

public interface IIncidentService
{
    Task<IEnumerable<IncidentDto>> GetAllIncidentsAsync();
    Task<IncidentDto?> GetIncidentByIdAsync(Guid id);
    Task<IncidentDto> CreateIncidentAsync(CreateIncidentDto createIncidentDto, Guid createdByUserId);
    Task<IncidentDto> UpdateIncidentAsync(Guid id, UpdateIncidentDto updateIncidentDto, Guid userId);
    Task<IncidentDto> AssignIncidentAsync(Guid id, AssignIncidentDto assignIncidentDto, Guid userId);
    Task<IncidentDto> UpdateIncidentStatusAsync(Guid id, UpdateIncidentStatusDto updateStatusDto, Guid userId);
    Task DeleteIncidentAsync(Guid id);
}
