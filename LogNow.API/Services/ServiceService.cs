using LogNow.API.DTOs;
using LogNow.API.Models;
using LogNow.API.Repositories;

namespace LogNow.API.Services;

public class ServiceService : IServiceService
{
    private readonly IServiceRepository _serviceRepository;

    public ServiceService(IServiceRepository serviceRepository)
    {
        _serviceRepository = serviceRepository;
    }

    public async Task<IEnumerable<ServiceDto>> GetAllServicesAsync()
    {
        var services = await _serviceRepository.GetAllAsync();
        return services.Select(MapToServiceDto);
    }

    public async Task<ServiceDto?> GetServiceByIdAsync(Guid id)
    {
        var service = await _serviceRepository.GetByIdAsync(id);
        return service == null ? null : MapToServiceDto(service);
    }

    public async Task<ServiceDto> CreateServiceAsync(CreateServiceDto createServiceDto)
    {
        if (!Enum.TryParse<ServiceStatus>(createServiceDto.Status, true, out var status))
        {
            status = ServiceStatus.Active;
        }

        var service = new Service
        {
            Id = Guid.NewGuid(),
            Name = createServiceDto.Name,
            Description = createServiceDto.Description,
            OwnerTeam = createServiceDto.OwnerTeam,
            Status = status,
            CreatedAt = DateTime.UtcNow
        };

        var createdService = await _serviceRepository.CreateAsync(service);
        return MapToServiceDto(createdService);
    }

    public async Task<ServiceDto> UpdateServiceAsync(Guid id, UpdateServiceDto updateServiceDto)
    {
        var service = await _serviceRepository.GetByIdAsync(id);
        if (service == null)
        {
            throw new KeyNotFoundException($"Service with ID {id} not found");
        }

        if (!string.IsNullOrEmpty(updateServiceDto.Name))
            service.Name = updateServiceDto.Name;

        if (!string.IsNullOrEmpty(updateServiceDto.Description))
            service.Description = updateServiceDto.Description;

        if (!string.IsNullOrEmpty(updateServiceDto.OwnerTeam))
            service.OwnerTeam = updateServiceDto.OwnerTeam;

        if (!string.IsNullOrEmpty(updateServiceDto.Status) && 
            Enum.TryParse<ServiceStatus>(updateServiceDto.Status, true, out var status))
        {
            service.Status = status;
        }

        var updatedService = await _serviceRepository.UpdateAsync(service);
        return MapToServiceDto(updatedService);
    }

    public async Task DeleteServiceAsync(Guid id)
    {
        await _serviceRepository.DeleteAsync(id);
    }

    private ServiceDto MapToServiceDto(Service service)
    {
        return new ServiceDto
        {
            Id = service.Id,
            Name = service.Name,
            Description = service.Description,
            OwnerTeam = service.OwnerTeam,
            Status = service.Status.ToString(),
            CreatedAt = service.CreatedAt,
            UpdatedAt = service.UpdatedAt
        };
    }
}
