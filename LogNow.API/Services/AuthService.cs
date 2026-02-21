using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using LogNow.API.DTOs;
using LogNow.API.Models;
using LogNow.API.Repositories;
using Microsoft.IdentityModel.Tokens;

namespace LogNow.API.Services;

public class AuthService : IAuthService
{
    private readonly IUserRepository _userRepository;
    private readonly IConfiguration _configuration;

    public AuthService(IUserRepository userRepository, IConfiguration configuration)
    {
        _userRepository = userRepository;
        _configuration = configuration;
    }

    public async Task<AuthResponseDto> RegisterAsync(RegisterDto registerDto)
    {
        // Check if user already exists
        var existingUser = await _userRepository.GetByEmailAsync(registerDto.Email);
        if (existingUser != null)
        {
            throw new InvalidOperationException("User with this email already exists");
        }

        var existingUsername = await _userRepository.GetByUsernameAsync(registerDto.Username);
        if (existingUsername != null)
        {
            throw new InvalidOperationException("Username already taken");
        }

        // Parse role
        if (!Enum.TryParse<UserRole>(registerDto.Role, true, out var userRole))
        {
            userRole = UserRole.Engineer;
        }

        // Create new user
        var user = new User
        {
            Id = Guid.NewGuid(),
            Username = registerDto.Username,
            Email = registerDto.Email,
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(registerDto.Password),
            FullName = registerDto.FullName,
            Role = userRole,
            Team = registerDto.Team,
            IsActive = true,
            CreatedAt = DateTime.UtcNow
        };

        await _userRepository.CreateAsync(user);

        var userDto = MapToUserDto(user);
        var token = GenerateJwtToken(userDto);

        return new AuthResponseDto
        {
            Token = token,
            User = userDto
        };
    }

    public async Task<AuthResponseDto> LoginAsync(LoginDto loginDto)
    {
        var user = await _userRepository.GetByEmailAsync(loginDto.Email);
        if (user == null || !BCrypt.Net.BCrypt.Verify(loginDto.Password, user.PasswordHash))
        {
            throw new UnauthorizedAccessException("Invalid email or password");
        }

        if (!user.IsActive)
        {
            throw new UnauthorizedAccessException("User account is inactive");
        }

        var userDto = MapToUserDto(user);
        var token = GenerateJwtToken(userDto);

        return new AuthResponseDto
        {
            Token = token,
            User = userDto
        };
    }

    public string GenerateJwtToken(UserDto user)
    {
        var jwtSettings = _configuration.GetSection("JwtSettings");
        var secretKey = jwtSettings["SecretKey"] ?? throw new InvalidOperationException("JWT SecretKey not configured");
        var issuer = jwtSettings["Issuer"];
        var audience = jwtSettings["Audience"];
        var expirationMinutes = int.Parse(jwtSettings["ExpirationMinutes"] ?? "480");

        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
            new Claim(JwtRegisteredClaimNames.Email, user.Email),
            new Claim(JwtRegisteredClaimNames.Name, user.FullName),
            new Claim("username", user.Username),
            new Claim(ClaimTypes.Role, user.Role),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };

        var token = new JwtSecurityToken(
            issuer: issuer,
            audience: audience,
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(expirationMinutes),
            signingCredentials: credentials
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
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
