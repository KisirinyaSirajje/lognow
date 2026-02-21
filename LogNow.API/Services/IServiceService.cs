using LogNow.API.DTOs;

namespace LogNow.API.Services;

public interface IServiceService
{
    Task<IEnumerable<ServiceDto>> GetAllServicesAsync();
    Task<ServiceDto?> GetServiceByIdAsync(Guid id);
    Task<ServiceDto> CreateServiceAsync(CreateServiceDto createServiceDto);
    Task<ServiceDto> UpdateServiceAsync(Guid id, UpdateServiceDto updateServiceDto);
    Task DeleteServiceAsync(Guid id);
}
