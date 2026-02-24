using LogNow.API.Data;
using LogNow.API.Models;
using Microsoft.EntityFrameworkCore;

namespace LogNow.API.Repositories;

public class WorkOrderRepository : IWorkOrderRepository
{
    private readonly ApplicationDbContext _context;

    public WorkOrderRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<WorkOrder?> GetByIdAsync(Guid id)
    {
        return await _context.WorkOrders
            .Include(w => w.Service)
            .Include(w => w.Incident)
            .Include(w => w.AssignedToUser)
            .Include(w => w.CreatedByUser)
            .FirstOrDefaultAsync(w => w.Id == id);
    }

    public async Task<IEnumerable<WorkOrder>> GetAllAsync()
    {
        return await _context.WorkOrders
            .Include(w => w.Service)
            .Include(w => w.Incident)
            .Include(w => w.AssignedToUser)
            .Include(w => w.CreatedByUser)
            .OrderByDescending(w => w.CreatedAt)
            .ToListAsync();
    }

    public async Task<IEnumerable<WorkOrder>> GetByStatusAsync(WorkOrderStatus status)
    {
        return await _context.WorkOrders
            .Include(w => w.Service)
            .Include(w => w.Incident)
            .Include(w => w.AssignedToUser)
            .Include(w => w.CreatedByUser)
            .Where(w => w.Status == status)
            .OrderByDescending(w => w.CreatedAt)
            .ToListAsync();
    }

    public async Task<IEnumerable<WorkOrder>> GetByAssignedUserIdAsync(Guid userId)
    {
        return await _context.WorkOrders
            .Include(w => w.Service)
            .Include(w => w.Incident)
            .Include(w => w.AssignedToUser)
            .Include(w => w.CreatedByUser)
            .Where(w => w.AssignedToUserId == userId)
            .OrderByDescending(w => w.CreatedAt)
            .ToListAsync();
    }

    public async Task<IEnumerable<WorkOrder>> GetByAssignedGroupAsync(string group)
    {
        return await _context.WorkOrders
            .Include(w => w.Service)
            .Include(w => w.Incident)
            .Include(w => w.AssignedToUser)
            .Include(w => w.CreatedByUser)
            .Where(w => w.AssignedGroup == group)
            .OrderByDescending(w => w.CreatedAt)
            .ToListAsync();
    }

    public async Task<IEnumerable<WorkOrder>> GetByServiceIdAsync(Guid serviceId)
    {
        return await _context.WorkOrders
            .Include(w => w.Service)
            .Include(w => w.Incident)
            .Include(w => w.AssignedToUser)
            .Include(w => w.CreatedByUser)
            .Where(w => w.ServiceId == serviceId)
            .OrderByDescending(w => w.CreatedAt)
            .ToListAsync();
    }

    public async Task<IEnumerable<WorkOrder>> GetByIncidentIdAsync(Guid incidentId)
    {
        return await _context.WorkOrders
            .Include(w => w.Service)
            .Include(w => w.Incident)
            .Include(w => w.AssignedToUser)
            .Include(w => w.CreatedByUser)
            .Where(w => w.IncidentId == incidentId)
            .OrderByDescending(w => w.CreatedAt)
            .ToListAsync();
    }

    public async Task<WorkOrder> CreateAsync(WorkOrder workOrder)
    {
        _context.WorkOrders.Add(workOrder);
        await _context.SaveChangesAsync();
        
        // Reload to get navigation properties
        return (await GetByIdAsync(workOrder.Id))!;
    }

    public async Task UpdateAsync(WorkOrder workOrder)
    {
        _context.WorkOrders.Update(workOrder);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(Guid id)
    {
        var workOrder = await _context.WorkOrders.FindAsync(id);
        if (workOrder != null)
        {
            _context.WorkOrders.Remove(workOrder);
            await _context.SaveChangesAsync();
        }
    }
}
