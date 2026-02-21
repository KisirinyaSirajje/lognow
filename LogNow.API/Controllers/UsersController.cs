using LogNow.API.DTOs;
using LogNow.API.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace LogNow.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class UsersController : ControllerBase
{
    private readonly IUserRepository _userRepository;

    public UsersController(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<UserDto>>> GetAllUsers()
    {
        var users = await _userRepository.GetAllAsync();
        return Ok(users.Select(u => new UserDto
        {
            Id = u.Id,
            Username = u.Username,
            Email = u.Email,
            FullName = u.FullName,
            Role = u.Role.ToString(),
            Team = u.Team,
            IsActive = u.IsActive,
            CreatedAt = u.CreatedAt
        }));
    }

    [HttpGet("team/{team}")]
    public async Task<ActionResult<IEnumerable<UserDto>>> GetUsersByTeam(string team)
    {
        var users = await _userRepository.GetByTeamAsync(team);
        return Ok(users.Select(u => new UserDto
        {
            Id = u.Id,
            Username = u.Username,
            Email = u.Email,
            FullName = u.FullName,
            Role = u.Role.ToString(),
            Team = u.Team,
            IsActive = u.IsActive,
            CreatedAt = u.CreatedAt
        }));
    }

    [HttpGet("teams")]
    public async Task<ActionResult<IEnumerable<string>>> GetAllTeams()
    {
        var users = await _userRepository.GetAllAsync();
        var teams = users
            .Where(u => !string.IsNullOrEmpty(u.Team))
            .Select(u => u.Team!)
            .Distinct()
            .OrderBy(t => t)
            .ToList();
        return Ok(teams);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<UserDto>> GetUserById(Guid id)
    {
        var user = await _userRepository.GetByIdAsync(id);
        if (user == null)
        {
            return NotFound();
        }

        return Ok(new UserDto
        {
            Id = user.Id,
            Username = user.Username,
            Email = user.Email,
            FullName = user.FullName,
            Role = user.Role.ToString(),
            Team = user.Team,
            IsActive = user.IsActive,
            CreatedAt = user.CreatedAt
        });
    }
}

public class UserDto
{
    public Guid Id { get; set; }
    public string Username { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string FullName { get; set; } = string.Empty;
    public string Role { get; set; } = string.Empty;
    public string? Team { get; set; }
    public bool IsActive { get; set; }
    public DateTime CreatedAt { get; set; }
}
