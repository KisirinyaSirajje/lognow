using LogNow.API.DTOs;
using LogNow.API.Models;
using LogNow.API.Repositories;

namespace LogNow.API.Services;

public interface IWorkOrderService
{
    Task<IEnumerable<WorkOrderDto>> GetAllWorkOrdersAsync();
    Task<WorkOrderDto?> GetWorkOrderByIdAsync(Guid id);
    Task<IEnumerable<WorkOrderDto>> GetByStatusAsync(WorkOrderStatus status);
    Task<IEnumerable<WorkOrderDto>> GetByAssignedUserIdAsync(Guid userId);
    Task<IEnumerable<WorkOrderDto>> GetByAssignedGroupAsync(string group);
    Task<WorkOrderDto> CreateWorkOrderAsync(CreateWorkOrderDto createWorkOrderDto, Guid createdByUserId);
    Task<WorkOrderDto> UpdateWorkOrderAsync(Guid id, UpdateWorkOrderDto updateWorkOrderDto);
    Task<WorkOrderDto> AssignWorkOrderAsync(Guid id, AssignWorkOrderDto assignDto, Guid assignedByUserId);
    Task DeleteWorkOrderAsync(Guid id);
}

public class WorkOrderService : IWorkOrderService
{
    private readonly IWorkOrderRepository _workOrderRepository;
    private readonly INotificationService _notificationService;

    public WorkOrderService(
        IWorkOrderRepository workOrderRepository,
        INotificationService notificationService)
    {
        _workOrderRepository = workOrderRepository;
        _notificationService = notificationService;
    }

    public async Task<IEnumerable<WorkOrderDto>> GetAllWorkOrdersAsync()
    {
        var workOrders = await _workOrderRepository.GetAllAsync();
        return workOrders.Select(MapToDto);
    }

    public async Task<WorkOrderDto?> GetWorkOrderByIdAsync(Guid id)
    {
        var workOrder = await _workOrderRepository.GetByIdAsync(id);
        return workOrder == null ? null : MapToDto(workOrder);
    }

    public async Task<IEnumerable<WorkOrderDto>> GetByStatusAsync(WorkOrderStatus status)
    {
        var workOrders = await _workOrderRepository.GetByStatusAsync(status);
        return workOrders.Select(MapToDto);
    }

    public async Task<IEnumerable<WorkOrderDto>> GetByAssignedUserIdAsync(Guid userId)
    {
        var workOrders = await _workOrderRepository.GetByAssignedUserIdAsync(userId);
        return workOrders.Select(MapToDto);
    }

    public async Task<IEnumerable<WorkOrderDto>> GetByAssignedGroupAsync(string group)
    {
        var workOrders = await _workOrderRepository.GetByAssignedGroupAsync(group);
        return workOrders.Select(MapToDto);
    }

    public async Task<WorkOrderDto> CreateWorkOrderAsync(CreateWorkOrderDto createWorkOrderDto, Guid createdByUserId)
    {
        if (!Enum.TryParse<WorkOrderType>(createWorkOrderDto.Type, true, out var type))
        {
            throw new ArgumentException($"Invalid work order type: {createWorkOrderDto.Type}");
        }

        if (!Enum.TryParse<WorkOrderPriority>(createWorkOrderDto.Priority, true, out var priority))
        {
            throw new ArgumentException($"Invalid priority: {createWorkOrderDto.Priority}");
        }

        var workOrderNumber = await GenerateWorkOrderNumberAsync();

        var workOrder = new WorkOrder
        {
            Id = Guid.NewGuid(),
            WorkOrderNumber = workOrderNumber,
            Title = createWorkOrderDto.Title,
            Description = createWorkOrderDto.Description,
            Type = type,
            Priority = priority,
            Status = WorkOrderStatus.Draft,
            ServiceId = createWorkOrderDto.ServiceId,
            IncidentId = createWorkOrderDto.IncidentId,
            Location = createWorkOrderDto.Location,
            ScheduledStartDate = createWorkOrderDto.ScheduledStartDate,
            ScheduledEndDate = createWorkOrderDto.ScheduledEndDate,
            EstimatedCost = createWorkOrderDto.EstimatedCost,
            EstimatedHours = createWorkOrderDto.EstimatedHours,
            PartsRequired = createWorkOrderDto.PartsRequired,
            CreatedByUserId = createdByUserId,
            CreatedAt = DateTime.UtcNow
        };

        var createdWorkOrder = await _workOrderRepository.CreateAsync(workOrder);
        return MapToDto(createdWorkOrder);
    }

    public async Task<WorkOrderDto> UpdateWorkOrderAsync(Guid id, UpdateWorkOrderDto updateWorkOrderDto)
    {
        var workOrder = await _workOrderRepository.GetByIdAsync(id);
        if (workOrder == null)
        {
            throw new KeyNotFoundException($"Work order with ID {id} not found");
        }

        if (!string.IsNullOrEmpty(updateWorkOrderDto.Title))
            workOrder.Title = updateWorkOrderDto.Title;

        if (!string.IsNullOrEmpty(updateWorkOrderDto.Description))
            workOrder.Description = updateWorkOrderDto.Description;

        if (!string.IsNullOrEmpty(updateWorkOrderDto.Priority) &&
            Enum.TryParse<WorkOrderPriority>(updateWorkOrderDto.Priority, true, out var priority))
            workOrder.Priority = priority;

        if (!string.IsNullOrEmpty(updateWorkOrderDto.Status) &&
            Enum.TryParse<WorkOrderStatus>(updateWorkOrderDto.Status, true, out var status))
        {
            workOrder.Status = status;
            if (status == WorkOrderStatus.Completed)
            {
                workOrder.CompletedAt = DateTime.UtcNow;
            }
        }

        if (updateWorkOrderDto.ScheduledStartDate.HasValue)
            workOrder.ScheduledStartDate = updateWorkOrderDto.ScheduledStartDate;

        if (updateWorkOrderDto.ScheduledEndDate.HasValue)
            workOrder.ScheduledEndDate = updateWorkOrderDto.ScheduledEndDate;

        if (updateWorkOrderDto.ActualStartDate.HasValue)
            workOrder.ActualStartDate = updateWorkOrderDto.ActualStartDate;

        if (updateWorkOrderDto.ActualEndDate.HasValue)
            workOrder.ActualEndDate = updateWorkOrderDto.ActualEndDate;

        if (updateWorkOrderDto.ActualCost.HasValue)
            workOrder.ActualCost = updateWorkOrderDto.ActualCost;

        if (updateWorkOrderDto.ActualHours.HasValue)
            workOrder.ActualHours = updateWorkOrderDto.ActualHours;

        if (!string.IsNullOrEmpty(updateWorkOrderDto.Location))
            workOrder.Location = updateWorkOrderDto.Location;

        if (!string.IsNullOrEmpty(updateWorkOrderDto.PartsRequired))
            workOrder.PartsRequired = updateWorkOrderDto.PartsRequired;

        if (!string.IsNullOrEmpty(updateWorkOrderDto.CompletionNotes))
            workOrder.CompletionNotes = updateWorkOrderDto.CompletionNotes;

        workOrder.UpdatedAt = DateTime.UtcNow;

        await _workOrderRepository.UpdateAsync(workOrder);
        
        // Reload to get updated navigation properties
        workOrder = await _workOrderRepository.GetByIdAsync(id);
        return MapToDto(workOrder!);
    }

    public async Task<WorkOrderDto> AssignWorkOrderAsync(Guid id, AssignWorkOrderDto assignDto, Guid assignedByUserId)
    {
        var workOrder = await _workOrderRepository.GetByIdAsync(id);
        if (workOrder == null)
        {
            throw new KeyNotFoundException($"Work order with ID {id} not found");
        }

        if (!string.IsNullOrWhiteSpace(assignDto.AssignedGroup))
        {
            workOrder.AssignedGroup = assignDto.AssignedGroup;
        }

        if (assignDto.UserId.HasValue)
        {
            workOrder.AssignedToUserId = assignDto.UserId.Value;
            workOrder.Status = WorkOrderStatus.Assigned;

            // Notify assigned user
            if (assignDto.UserId.Value != assignedByUserId)
            {
                await _notificationService.CreateNotificationAsync(
                    assignDto.UserId.Value,
                    "Work Order Assigned",
                    $"Work order {workOrder.WorkOrderNumber} ({workOrder.Title}) has been assigned to you",
                    NotificationType.IncidentAssigned,
                    workOrder.Id.ToString()
                );
            }
        }

        workOrder.UpdatedAt = DateTime.UtcNow;
        await _workOrderRepository.UpdateAsync(workOrder);

        // Reload to get updated navigation properties
        workOrder = await _workOrderRepository.GetByIdAsync(id);
        return MapToDto(workOrder!);
    }

    public async Task DeleteWorkOrderAsync(Guid id)
    {
        await _workOrderRepository.DeleteAsync(id);
    }

    private async Task<string> GenerateWorkOrderNumberAsync()
    {
        var date = DateTime.UtcNow;
        var prefix = "WO";
        var datePart = date.ToString("yyyyMMdd");
        
        var allWorkOrders = await _workOrderRepository.GetAllAsync();
        var todaysWorkOrders = allWorkOrders
            .Where(w => w.WorkOrderNumber.Contains(datePart))
            .Count();
        
        var sequence = (todaysWorkOrders + 1).ToString("D4");
        return $"{prefix}-{datePart}-{sequence}";
    }

    private WorkOrderDto MapToDto(WorkOrder workOrder)
    {
        return new WorkOrderDto
        {
            Id = workOrder.Id,
            WorkOrderNumber = workOrder.WorkOrderNumber,
            Title = workOrder.Title,
            Description = workOrder.Description,
            Type = workOrder.Type.ToString(),
            Priority = workOrder.Priority.ToString(),
            Status = workOrder.Status.ToString(),
            ServiceId = workOrder.ServiceId,
            ServiceName = workOrder.Service?.Name,
            IncidentId = workOrder.IncidentId,
            IncidentNumber = workOrder.Incident?.IncidentNumber,
            AssignedToUserId = workOrder.AssignedToUserId,
            AssignedToUserName = workOrder.AssignedToUser?.FullName,
            AssignedGroup = workOrder.AssignedGroup,
            CreatedByUserId = workOrder.CreatedByUserId,
            CreatedByUserName = workOrder.CreatedByUser.FullName,
            ScheduledStartDate = workOrder.ScheduledStartDate,
            ScheduledEndDate = workOrder.ScheduledEndDate,
            ActualStartDate = workOrder.ActualStartDate,
            ActualEndDate = workOrder.ActualEndDate,
            EstimatedCost = workOrder.EstimatedCost,
            ActualCost = workOrder.ActualCost,
            EstimatedHours = workOrder.EstimatedHours,
            ActualHours = workOrder.ActualHours,
            Location = workOrder.Location,
            PartsRequired = workOrder.PartsRequired,
            CompletionNotes = workOrder.CompletionNotes,
            CreatedAt = workOrder.CreatedAt,
            UpdatedAt = workOrder.UpdatedAt,
            CompletedAt = workOrder.CompletedAt
        };
    }
}
