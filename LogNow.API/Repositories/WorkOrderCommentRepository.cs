using LogNow.API.Data;
using LogNow.API.Models;
using Microsoft.EntityFrameworkCore;

namespace LogNow.API.Repositories;

public interface IWorkOrderCommentRepository
{
    Task<IEnumerable<WorkOrderComment>> GetByWorkOrderIdAsync(Guid workOrderId);
    Task<WorkOrderComment> AddAsync(WorkOrderComment comment);
}

public class WorkOrderCommentRepository : IWorkOrderCommentRepository
{
    private readonly ApplicationDbContext _context;

    public WorkOrderCommentRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<WorkOrderComment>> GetByWorkOrderIdAsync(Guid workOrderId)
    {
        return await _context.WorkOrderComments
            .Include(c => c.User)
            .Where(c => c.WorkOrderId == workOrderId)
            .OrderBy(c => c.CreatedAt)
            .ToListAsync();
    }

    public async Task<WorkOrderComment> AddAsync(WorkOrderComment comment)
    {
        _context.WorkOrderComments.Add(comment);
        await _context.SaveChangesAsync();
        
        // Reload with user included
        await _context.Entry(comment).Reference(c => c.User).LoadAsync();
        return comment;
    }
}
