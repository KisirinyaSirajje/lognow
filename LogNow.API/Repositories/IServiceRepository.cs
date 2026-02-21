using LogNow.API.Models;

namespace LogNow.API.Repositories;

public interface IServiceRepository
{
    Task<Service?> GetByIdAsync(Guid id);
    Task<IEnumerable<Service>> GetAllAsync();
    Task<Service> CreateAsync(Service service);
    Task<Service> UpdateAsync(Service service);
    Task DeleteAsync(Guid id);
}
