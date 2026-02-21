using LogNow.API.DTOs;
using LogNow.API.Models;
using LogNow.API.Repositories;

namespace LogNow.API.Services;

public class IncidentService : IIncidentService
{
    private readonly IIncidentRepository _incidentRepository;
    private readonly IIncidentTimelineService _timelineService;
    private readonly IServiceRepository _serviceRepository;
    private readonly IUserRepository _userRepository;

    public IncidentService(
        IIncidentRepository incidentRepository,
        IIncidentTimelineService timelineService,
        IServiceRepository serviceRepository,
        IUserRepository userRepository)
    {
        _incidentRepository = incidentRepository;
        _timelineService = timelineService;
        _serviceRepository = serviceRepository;
        _userRepository = userRepository;
    }

    public async Task<IEnumerable<IncidentDto>> GetAllIncidentsAsync()
    {
        var incidents = await _incidentRepository.GetAllAsync();
        return incidents.Select(MapToIncidentDto);
    }

    public async Task<IncidentDto?> GetIncidentByIdAsync(Guid id)
    {
        var incident = await _incidentRepository.GetByIdAsync(id);
        return incident == null ? null : MapToIncidentDto(incident);
    }

    public async Task<IncidentDto> CreateIncidentAsync(CreateIncidentDto createIncidentDto, Guid createdByUserId)
    {
        // Validate service exists
        var service = await _serviceRepository.GetByIdAsync(createIncidentDto.ServiceId);
        if (service == null)
        {
            throw new KeyNotFoundException($"Service with ID {createIncidentDto.ServiceId} not found");
        }

        // Parse severity
        if (!Enum.TryParse<Severity>(createIncidentDto.Severity, true, out var severity))
        {
            severity = Severity.SEV3;
        }

        // Generate incident number
        var incidentNumber = await _incidentRepository.GenerateIncidentNumberAsync();

        // Calculate SLA times
        var slaConfig = SLA.GetDefaultSLAs()[severity];
        var responseDueAt = DateTime.UtcNow.AddMinutes(slaConfig.ResponseTimeMinutes);
        var resolutionDueAt = DateTime.UtcNow.AddMinutes(slaConfig.ResolutionTimeMinutes);

        var incident = new Incident
        {
            Id = Guid.NewGuid(),
            IncidentNumber = incidentNumber,
            Title = createIncidentDto.Title,
            Description = createIncidentDto.Description,
            ServiceId = createIncidentDto.ServiceId,
            Severity = severity,
            Status = IncidentStatus.Pending,
            CreatedByUserId = createdByUserId,
            CreatedAt = DateTime.UtcNow,
            ResponseDueAt = responseDueAt,
            ResolutionDueAt = resolutionDueAt
        };

        var createdIncident = await _incidentRepository.CreateAsync(incident);

        // Add timeline entry
        await _timelineService.AddTimelineEntryAsync(
            createdIncident.Id,
            ActionType.Created,
            $"Incident created with severity {severity}",
            createdByUserId
        );

        return MapToIncidentDto(createdIncident);
    }

    public async Task<IncidentDto> UpdateIncidentAsync(Guid id, UpdateIncidentDto updateIncidentDto, Guid userId)
    {
        var incident = await _incidentRepository.GetByIdAsync(id);
        if (incident == null)
        {
            throw new KeyNotFoundException($"Incident with ID {id} not found");
        }

        var hasChanges = false;

        if (!string.IsNullOrEmpty(updateIncidentDto.Title))
        {
            incident.Title = updateIncidentDto.Title;
            hasChanges = true;
        }

        if (!string.IsNullOrEmpty(updateIncidentDto.Description))
        {
            incident.Description = updateIncidentDto.Description;
            hasChanges = true;
        }

        if (!string.IsNullOrEmpty(updateIncidentDto.Severity) &&
            Enum.TryParse<Severity>(updateIncidentDto.Severity, true, out var severity) &&
            incident.Severity != severity)
        {
            var oldSeverity = incident.Severity;
            incident.Severity = severity;
            hasChanges = true;

            // Recalculate SLA times
            var slaConfig = SLA.GetDefaultSLAs()[severity];
            incident.ResponseDueAt = incident.CreatedAt.AddMinutes(slaConfig.ResponseTimeMinutes);
            incident.ResolutionDueAt = incident.CreatedAt.AddMinutes(slaConfig.ResolutionTimeMinutes);

            await _timelineService.AddTimelineEntryAsync(
                incident.Id,
                ActionType.SeverityChanged,
                $"Severity changed from {oldSeverity} to {severity}",
                userId
            );
        }

        if (hasChanges)
        {
            await _incidentRepository.UpdateAsync(incident);
        }

        return MapToIncidentDto(incident);
    }

    public async Task<IncidentDto> AssignIncidentAsync(Guid id, AssignIncidentDto assignIncidentDto, Guid userId)
    {
        var incident = await _incidentRepository.GetByIdAsync(id);
        if (incident == null)
        {
            throw new KeyNotFoundException($"Incident with ID {id} not found");
        }

        var assignedUser = await _userRepository.GetByIdAsync(assignIncidentDto.UserId);
        if (assignedUser == null)
        {
            throw new KeyNotFoundException($"User with ID {assignIncidentDto.UserId} not found");
        }

        incident.AssignedToUserId = assignIncidentDto.UserId;
        incident.AssignedByUserId = userId;
        incident.Status = IncidentStatus.Assigned;

        await _incidentRepository.UpdateAsync(incident);

        await _timelineService.AddTimelineEntryAsync(
            incident.Id,
            ActionType.Assigned,
            $"Incident assigned to {assignedUser.FullName}",
            userId
        );

        // Reload to get updated navigation properties
        incident = await _incidentRepository.GetByIdAsync(id);
        return MapToIncidentDto(incident!);
    }

    public async Task<IncidentDto> UpdateIncidentStatusAsync(Guid id, UpdateIncidentStatusDto updateStatusDto, Guid userId)
    {
        var incident = await _incidentRepository.GetByIdAsync(id);
        if (incident == null)
        {
            throw new KeyNotFoundException($"Incident with ID {id} not found");
        }

        if (!Enum.TryParse<IncidentStatus>(updateStatusDto.Status, true, out var newStatus))
        {
            throw new ArgumentException($"Invalid status: {updateStatusDto.Status}");
        }

        // Validate resolution note for resolved/cancelled incidents
        if ((newStatus == IncidentStatus.Resolved || newStatus == IncidentStatus.Cancelled) 
            && string.IsNullOrWhiteSpace(updateStatusDto.ResolutionNote))
        {
            throw new ArgumentException("Resolution note is required when resolving or cancelling an incident");
        }

        // Validate hold reason for on-hold incidents
        if (newStatus == IncidentStatus.OnHold && string.IsNullOrWhiteSpace(updateStatusDto.OnHoldReason))
        {
            throw new ArgumentException("Hold reason is required when putting an incident on hold");
        }

        var oldStatus = incident.Status;
        incident.Status = newStatus;

        if (newStatus == IncidentStatus.Resolved || newStatus == IncidentStatus.Cancelled)
        {
            incident.ResolutionNote = updateStatusDto.ResolutionNote;
            if (newStatus == IncidentStatus.Resolved)
            {
                incident.ResolvedAt = DateTime.UtcNow;
                
                // Check SLA breaches
                if (incident.ResolutionDueAt.HasValue && DateTime.UtcNow > incident.ResolutionDueAt.Value)
                {
                    incident.IsResolutionBreached = true;
                }
            }
        }

        if (newStatus == IncidentStatus.OnHold)
        {
            incident.OnHoldReason = updateStatusDto.OnHoldReason;
        }

        await _incidentRepository.UpdateAsync(incident);

        var actionType = newStatus == IncidentStatus.Resolved ? ActionType.Resolved :
                        newStatus == IncidentStatus.Cancelled ? ActionType.Closed :
                        ActionType.StatusChanged;

        await _timelineService.AddTimelineEntryAsync(
            incident.Id,
            actionType,
            $"Status changed from {oldStatus} to {newStatus}",
            userId
        );

        return MapToIncidentDto(incident);
    }

    public async Task DeleteIncidentAsync(Guid id)
    {
        await _incidentRepository.DeleteAsync(id);
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
            AssignedByUserId = incident.AssignedByUserId,
            AssignedByUserName = incident.AssignedByUser?.FullName,
            CreatedByUserId = incident.CreatedByUserId,
            CreatedByUserName = incident.CreatedByUser?.FullName ?? string.Empty,
            CreatedAt = incident.CreatedAt,
            UpdatedAt = incident.UpdatedAt,
            ResolvedAt = incident.ResolvedAt,
            ResponseDueAt = incident.ResponseDueAt,
            ResolutionDueAt = incident.ResolutionDueAt,
            IsResponseBreached = incident.IsResponseBreached,
            IsResolutionBreached = incident.IsResolutionBreached,
            ResolutionNote = incident.ResolutionNote,
            OnHoldReason = incident.OnHoldReason
        };
    }
}
