using LogNow.API.DTOs;
using LogNow.API.Models;
using LogNow.API.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LogNow.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize(Roles = "Admin")]
public class AdminController : ControllerBase
{
    private readonly IUserRepository _userRepository;
    private readonly IIncidentRepository _incidentRepository;
    private readonly IServiceRepository _serviceRepository;

    public AdminController(
        IUserRepository userRepository,
        IIncidentRepository incidentRepository,
        IServiceRepository serviceRepository)
    {
        _userRepository = userRepository;
        _incidentRepository = incidentRepository;
        _serviceRepository = serviceRepository;
    }

    [HttpGet("users")]
    public async Task<ActionResult<IEnumerable<UserDto>>> GetAllUsers()
    {
        var users = await _userRepository.GetAllAsync();
        return Ok(users.Select(MapToUserDto));
    }

    [HttpPut("users/{id}/status")]
    public async Task<IActionResult> UpdateUserStatus(Guid id, [FromBody] UpdateUserStatusDto dto)
    {
        var user = await _userRepository.GetByIdAsync(id);
        if (user == null)
        {
            return NotFound(new { message = "User not found" });
        }

        user.IsActive = dto.IsActive;
        await _userRepository.UpdateAsync(user);

        return NoContent();
    }

    [HttpPut("users/{id}/role")]
    public async Task<IActionResult> UpdateUserRole(Guid id, [FromBody] UpdateUserRoleDto dto)
    {
        var user = await _userRepository.GetByIdAsync(id);
        if (user == null)
        {
            return NotFound(new { message = "User not found" });
        }

        if (!Enum.TryParse<UserRole>(dto.Role, true, out var role))
        {
            return BadRequest(new { message = "Invalid role" });
        }

        user.Role = role;
        await _userRepository.UpdateAsync(user);

        return NoContent();
    }

    [HttpGet("stats")]
    public async Task<ActionResult<AdminStatsDto>> GetAdminStats()
    {
        var users = await _userRepository.GetAllAsync();
        var incidents = await _incidentRepository.GetAllAsync();
        var services = await _serviceRepository.GetAllAsync();

        var stats = new AdminStatsDto
        {
            TotalUsers = users.Count(),
            ActiveUsers = users.Count(u => u.IsActive),
            TotalIncidents = incidents.Count(),
            TotalServices = services.Count(),
            UsersByRole = users.GroupBy(u => u.Role.ToString())
                .ToDictionary(g => g.Key, g => g.Count()),
            IncidentsByStatus = incidents.GroupBy(i => i.Status.ToString())
                .ToDictionary(g => g.Key, g => g.Count())
        };

        return Ok(stats);
    }

    private UserDto MapToUserDto(User user)
    {
        return new UserDto
        {
            Id = user.Id,
            Username = user.Username,
            Email = user.Email,
            FullName = user.FullName,
            Role = user.Role.ToString(),
            Team = user.Team,
            IsActive = user.IsActive,
            CreatedAt = user.CreatedAt
        };
    }
}

public class UpdateUserStatusDto
{
    public bool IsActive { get; set; }
}

public class UpdateUserRoleDto
{
    public string Role { get; set; } = string.Empty;
}

public class AdminStatsDto
{
    public int TotalUsers { get; set; }
    public int ActiveUsers { get; set; }
    public int TotalIncidents { get; set; }
    public int TotalServices { get; set; }
    public Dictionary<string, int> UsersByRole { get; set; } = new();
    public Dictionary<string, int> IncidentsByStatus { get; set; } = new();
}
