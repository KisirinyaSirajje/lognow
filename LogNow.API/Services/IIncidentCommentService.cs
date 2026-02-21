using LogNow.API.DTOs;

namespace LogNow.API.Services;

public interface IIncidentCommentService
{
    Task<IEnumerable<IncidentCommentDto>> GetCommentsByIncidentIdAsync(Guid incidentId);
    Task<IncidentCommentDto> CreateCommentAsync(Guid incidentId, CreateIncidentCommentDto createCommentDto, Guid userId);
    Task DeleteCommentAsync(Guid id);
}
