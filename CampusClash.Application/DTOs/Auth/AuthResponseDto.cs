namespace CampusClash.Application.DTOs.Auth;

public class AuthResponseDto
{
    public string Token { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Username { get; set; } = string.Empty;
    public bool IsEmailConfirmed { get; set; }
    public bool IsRiotLinked { get; set; }
    public bool IsValidated { get; set; }
}