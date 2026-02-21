using LogNow.API.DTOs;

namespace LogNow.API.Services;

public interface IDashboardService
{
    Task<DashboardDto> GetDashboardDataAsync();
}
