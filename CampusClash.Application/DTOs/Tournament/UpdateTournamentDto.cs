using CampusClash.Domain.Enums;

namespace CampusClash.Application.DTOs.Tournament;

public class UpdateTournamentDto
{
    public string Name { get; set; } = string.Empty;
    public DateTime StartDate { get; set; }
    public Game Game { get; set; }
    public string Description { get; set; } = string.Empty;
    public bool IsInterUniversity { get; set; }
    public Guid? UniversityId { get; set; }
    public int MaxTeams { get; set; }
}
