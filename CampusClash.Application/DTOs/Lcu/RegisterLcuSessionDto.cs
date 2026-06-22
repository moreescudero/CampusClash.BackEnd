namespace CampusClash.Application.DTOs.Lcu;

public class RegisterLcuSessionDto
{
    public Guid MatchId { get; set; }
    public string BaseUrl { get; set; } = string.Empty;
    public string AuthToken { get; set; } = string.Empty;
}
