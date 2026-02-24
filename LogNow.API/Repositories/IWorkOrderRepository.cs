using LogNow.API.Models;

namespace LogNow.API.Repositories;

public interface IWorkOrderRepository
{
    Task<WorkOrder?> GetByIdAsync(Guid id);
    Task<IEnumerable<WorkOrder>> GetAllAsync();
    Task<IEnumerable<WorkOrder>> GetByStatusAsync(WorkOrderStatus status);
    Task<IEnumerable<WorkOrder>> GetByAssignedUserIdAsync(Guid userId);
    Task<IEnumerable<WorkOrder>> GetByAssignedGroupAsync(string group);
    Task<IEnumerable<WorkOrder>> GetByServiceIdAsync(Guid serviceId);
    Task<IEnumerable<WorkOrder>> GetByIncidentIdAsync(Guid incidentId);
    Task<WorkOrder> CreateAsync(WorkOrder workOrder);
    Task UpdateAsync(WorkOrder workOrder);
    Task DeleteAsync(Guid id);
}
