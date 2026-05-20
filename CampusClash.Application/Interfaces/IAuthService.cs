using CampusClash.Application.DTOs.Auth;

namespace CampusClash.Application.Interfaces;

public interface IAuthService
{
    Task<AuthResponseDto> RegisterAsync(RegisterRequestDto request);
    Task<AuthResponseDto> LoginAsync(LoginRequestDto request);
    Task<bool> ConfirmEmailAsync(string email, string token);
}