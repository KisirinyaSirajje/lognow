using LogNow.API.Data;
using LogNow.API.Models;
using Microsoft.EntityFrameworkCore;

namespace LogNow.API.Repositories;

public class ServiceRepository : IServiceRepository
{
    private readonly ApplicationDbContext _context;

    public ServiceRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Service?> GetByIdAsync(Guid id)
    {
        return await _context.Services.FindAsync(id);
    }

    public async Task<IEnumerable<Service>> GetAllAsync()
    {
        return await _context.Services.OrderBy(s => s.Name).ToListAsync();
    }

    public async Task<Service> CreateAsync(Service service)
    {
        _context.Services.Add(service);
        await _context.SaveChangesAsync();
        return service;
    }

    public async Task<Service> UpdateAsync(Service service)
    {
        service.UpdatedAt = DateTime.UtcNow;
        _context.Services.Update(service);
        await _context.SaveChangesAsync();
        return service;
    }

    public async Task DeleteAsync(Guid id)
    {
        var service = await _context.Services.FindAsync(id);
        if (service != null)
        {
            _context.Services.Remove(service);
            await _context.SaveChangesAsync();
        }
    }
}
