using CampusClash.Domain.Enums;

namespace CampusClash.Application.DTOs.Tournament;

public class TournamentResponseDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public Game Game { get; set; }
    public string Description { get; set; } = string.Empty;
    public bool IsInterUniversity { get; set; }
    public string? UniversityName { get; set; }
    public int MaxTeams { get; set; }
    public DateTime StartDate { get; set; }
    public TournamentStatus Status { get; set; }
    public string OrganizerUsername { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public List<TeamDto> Teams { get; set; } = [];
}
