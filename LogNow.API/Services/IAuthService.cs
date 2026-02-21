using LogNow.API.DTOs;

namespace LogNow.API.Services;

public interface IAuthService
{
    Task<AuthResponseDto> RegisterAsync(RegisterDto registerDto);
    Task<AuthResponseDto> LoginAsync(LoginDto loginDto);
    string GenerateJwtToken(UserDto user);
}
