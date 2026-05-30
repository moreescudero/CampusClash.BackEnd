using CampusClash.Domain.Enums;

namespace CampusClash.Application.DTOs.OrganizerRequest;

public class OrganizerRequestResponseDto
{
    public Guid Id { get; set; }
    public string TournamentName { get; set; } = string.Empty;
    public DateTime StartDate { get; set; }
    public Game Game { get; set; }
    public string Description { get; set; } = string.Empty;
    public bool IsInterUniversity { get; set; }
    public string? UniversityName { get; set; }
    public int MaxTeams { get; set; }
    public OrganizerRequestStatus Status { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? ReviewedAt { get; set; }
}
