using LogNow.API.DTOs;
using LogNow.API.Models;
using LogNow.API.Repositories;

namespace LogNow.API.Services;

public class DashboardService : IDashboardService
{
    private readonly IIncidentRepository _incidentRepository;

    public DashboardService(IIncidentRepository incidentRepository)
    {
        _incidentRepository = incidentRepository;
    }

    public async Task<DashboardDto> GetDashboardDataAsync()
    {
        var allIncidents = (await _incidentRepository.GetAllAsync()).ToList();

        var dashboard = new DashboardDto
        {
            TotalIncidents = allIncidents.Count,
            OpenIncidents = allIncidents.Count(i => i.Status == IncidentStatus.Pending),
            InProgressIncidents = allIncidents.Count(i => i.Status == IncidentStatus.InProgress || i.Status == IncidentStatus.Assigned),
            ResolvedIncidents = allIncidents.Count(i => i.Status == IncidentStatus.Resolved || i.Status == IncidentStatus.Cancelled),
            SlaBreaches = allIncidents.Count(i => i.IsResponseBreached || i.IsResolutionBreached)
        };

        // Incidents by severity
        dashboard.IncidentsBySeverity = allIncidents
            .GroupBy(i => i.Severity.ToString())
            .ToDictionary(g => g.Key, g => g.Count());

        // Incidents by service
        dashboard.IncidentsByService = allIncidents
            .GroupBy(i => i.Service?.Name ?? "Unknown")
            .ToDictionary(g => g.Key, g => g.Count());

        // Incidents by status
        dashboard.IncidentsByStatus = allIncidents
            .GroupBy(i => i.Status.ToString())
            .ToDictionary(g => g.Key, g => g.Count());

        // Recent incidents (last 10)
        dashboard.RecentIncidents = allIncidents
            .OrderByDescending(i => i.CreatedAt)
            .Take(10)
            .Select(MapToIncidentDto)
            .ToList();

        return dashboard;
    }

    private IncidentDto MapToIncidentDto(Incident incident)
    {
        return new IncidentDto
        {
            Id = incident.Id,
            IncidentNumber = incident.IncidentNumber,
            Title = incident.Title,
            Description = incident.Description,
            ServiceId = incident.ServiceId,
            ServiceName = incident.Service?.Name ?? string.Empty,
            Severity = incident.Severity.ToString(),
            Status = incident.Status.ToString(),
            AssignedToUserId = incident.AssignedToUserId,
            AssignedToUserName = incident.AssignedToUser?.FullName,
            CreatedByUserId = incident.CreatedByUserId,
            CreatedByUserName = incident.CreatedByUser?.FullName ?? string.Empty,
            CreatedAt = incident.CreatedAt,
            UpdatedAt = incident.UpdatedAt,
            ResolvedAt = incident.ResolvedAt,
            ResponseDueAt = incident.ResponseDueAt,
            ResolutionDueAt = incident.ResolutionDueAt,
            IsResponseBreached = incident.IsResponseBreached,
            IsResolutionBreached = incident.IsResolutionBreached
        };
    }
}
