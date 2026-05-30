using CampusClash.Domain.Enums;

namespace CampusClash.Application.DTOs.OrganizerRequest;

public class OrganizerRequestDto
{
    public string TournamentName { get; set; } = string.Empty;
    public DateTime StartDate { get; set; }
    public Game Game { get; set; }
    public string Description { get; set; } = string.Empty;
    public bool IsInterUniversity { get; set; }
    public Guid? UniversityId { get; set; }
    public int MaxTeams { get; set; }
}
