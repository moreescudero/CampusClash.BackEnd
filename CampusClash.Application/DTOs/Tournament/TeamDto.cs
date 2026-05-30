namespace CampusClash.Application.DTOs.Tournament;

public class TeamDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string UniversityName { get; set; } = string.Empty;
    public List<PlayerDto> Players { get; set; } = [];
    public bool IsFull { get; set; }
}
