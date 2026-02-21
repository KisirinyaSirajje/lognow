using LogNow.API.Data;
using LogNow.API.Models;
using Microsoft.EntityFrameworkCore;

namespace LogNow.API.Repositories;

public class IncidentRepository : IIncidentRepository
{
    private readonly ApplicationDbContext _context;

    public IncidentRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Incident?> GetByIdAsync(Guid id)
    {
        return await _context.Incidents
            .Include(i => i.Service)
            .Include(i => i.CreatedByUser)
            .Include(i => i.AssignedToUser)
            .Include(i => i.AssignedByUser)
            .Include(i => i.Comments).ThenInclude(c => c.User)
            .Include(i => i.Timeline).ThenInclude(t => t.User)
            .FirstOrDefaultAsync(i => i.Id == id);
    }

    public async Task<Incident?> GetByIncidentNumberAsync(string incidentNumber)
    {
        return await _context.Incidents
            .Include(i => i.Service)
            .Include(i => i.CreatedByUser)
            .Include(i => i.AssignedToUser)
            .Include(i => i.AssignedByUser)
            .FirstOrDefaultAsync(i => i.IncidentNumber == incidentNumber);
    }

    public async Task<IEnumerable<Incident>> GetAllAsync()
    {
        return await _context.Incidents
            .Include(i => i.Service)
            .Include(i => i.CreatedByUser)
            .Include(i => i.AssignedToUser)
            .Include(i => i.AssignedByUser)
            .OrderByDescending(i => i.CreatedAt)
            .ToListAsync();
    }

    public async Task<IEnumerable<Incident>> GetByServiceIdAsync(Guid serviceId)
    {
        return await _context.Incidents
            .Include(i => i.Service)
            .Include(i => i.CreatedByUser)
            .Include(i => i.AssignedToUser)
            .Include(i => i.AssignedByUser)
            .Where(i => i.ServiceId == serviceId)
            .OrderByDescending(i => i.CreatedAt)
            .ToListAsync();
    }

    public async Task<IEnumerable<Incident>> GetByAssignedUserIdAsync(Guid userId)
    {
        return await _context.Incidents
            .Include(i => i.Service)
            .Include(i => i.CreatedByUser)
            .Include(i => i.AssignedToUser)
            .Include(i => i.AssignedByUser)
            .Where(i => i.AssignedToUserId == userId)
            .OrderByDescending(i => i.CreatedAt)
            .ToListAsync();
    }

    public async Task<IEnumerable<Incident>> GetByCreatedUserIdAsync(Guid userId)
    {
        return await _context.Incidents
            .Include(i => i.Service)
            .Include(i => i.CreatedByUser)
            .Include(i => i.AssignedToUser)
            .Include(i => i.AssignedByUser)
            .Where(i => i.CreatedByUserId == userId)
            .OrderByDescending(i => i.CreatedAt)
            .ToListAsync();
    }

    public async Task<Incident> CreateAsync(Incident incident)
    {
        _context.Incidents.Add(incident);
        await _context.SaveChangesAsync();
        
        // Load navigation properties
        await _context.Entry(incident).Reference(i => i.Service).LoadAsync();
        await _context.Entry(incident).Reference(i => i.CreatedByUser).LoadAsync();
        if (incident.AssignedToUserId.HasValue)
        {
            await _context.Entry(incident).Reference(i => i.AssignedToUser).LoadAsync();
        }
        if (incident.AssignedByUserId.HasValue)
        {
            await _context.Entry(incident).Reference(i => i.AssignedByUser).LoadAsync();
        }
        
        return incident;
    }

    public async Task<Incident> UpdateAsync(Incident incident)
    {
        incident.UpdatedAt = DateTime.UtcNow;
        _context.Incidents.Update(incident);
        await _context.SaveChangesAsync();
        return incident;
    }

    public async Task DeleteAsync(Guid id)
    {
        var incident = await _context.Incidents.FindAsync(id);
        if (incident != null)
        {
            _context.Incidents.Remove(incident);
            await _context.SaveChangesAsync();
        }
    }

    public async Task<string> GenerateIncidentNumberAsync()
    {
        var today = DateTime.UtcNow;
        var prefix = $"INC-{today:yyyyMMdd}";
        
        var lastIncident = await _context.Incidents
            .Where(i => i.IncidentNumber.StartsWith(prefix))
            .OrderByDescending(i => i.IncidentNumber)
            .FirstOrDefaultAsync();

        if (lastIncident == null)
        {
            return $"{prefix}-0001";
        }

        var lastNumber = int.Parse(lastIncident.IncidentNumber.Split('-').Last());
        return $"{prefix}-{(lastNumber + 1):D4}";
    }
}
