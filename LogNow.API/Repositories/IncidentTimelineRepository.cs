using LogNow.API.Data;
using LogNow.API.Models;
using Microsoft.EntityFrameworkCore;

namespace LogNow.API.Repositories;

public class IncidentTimelineRepository : IIncidentTimelineRepository
{
    private readonly ApplicationDbContext _context;

    public IncidentTimelineRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<IncidentTimeline>> GetByIncidentIdAsync(Guid incidentId)
    {
        return await _context.IncidentTimelines
            .Include(t => t.User)
            .Where(t => t.IncidentId == incidentId)
            .OrderBy(t => t.CreatedAt)
            .ToListAsync();
    }

    public async Task<IncidentTimeline> CreateAsync(IncidentTimeline timeline)
    {
        _context.IncidentTimelines.Add(timeline);
        await _context.SaveChangesAsync();
        
        // Load navigation properties
        if (timeline.UserId.HasValue)
        {
            await _context.Entry(timeline).Reference(t => t.User).LoadAsync();
        }
        
        return timeline;
    }
}
