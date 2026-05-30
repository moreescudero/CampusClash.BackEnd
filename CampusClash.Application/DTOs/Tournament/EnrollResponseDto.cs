namespace CampusClash.Application.DTOs.Tournament;

public class EnrollResponseDto
{
    public Guid TeamId { get; set; }
    public string TeamName { get; set; } = string.Empty;
    public string UniversityName { get; set; } = string.Empty;
    public int CurrentPlayers { get; set; }
    public bool IsFull { get; set; }
}
