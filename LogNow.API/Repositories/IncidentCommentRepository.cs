using LogNow.API.Data;
using LogNow.API.Models;
using Microsoft.EntityFrameworkCore;

namespace LogNow.API.Repositories;

public class IncidentCommentRepository : IIncidentCommentRepository
{
    private readonly ApplicationDbContext _context;

    public IncidentCommentRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IncidentComment?> GetByIdAsync(Guid id)
    {
        return await _context.IncidentComments
            .Include(c => c.User)
            .Include(c => c.Incident)
            .FirstOrDefaultAsync(c => c.Id == id);
    }

    public async Task<IEnumerable<IncidentComment>> GetByIncidentIdAsync(Guid incidentId)
    {
        return await _context.IncidentComments
            .Include(c => c.User)
            .Where(c => c.IncidentId == incidentId)
            .OrderBy(c => c.CreatedAt)
            .ToListAsync();
    }

    public async Task<IncidentComment> CreateAsync(IncidentComment comment)
    {
        _context.IncidentComments.Add(comment);
        await _context.SaveChangesAsync();
        
        // Load navigation properties
        await _context.Entry(comment).Reference(c => c.User).LoadAsync();
        
        return comment;
    }

    public async Task DeleteAsync(Guid id)
    {
        var comment = await _context.IncidentComments.FindAsync(id);
        if (comment != null)
        {
            _context.IncidentComments.Remove(comment);
            await _context.SaveChangesAsync();
        }
    }
}
