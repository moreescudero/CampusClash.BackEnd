using CampusClash.Domain.Enums;

namespace CampusClash.Domain.Entities;

public class OrganizerRequest
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public string UserEmail { get; set; } = string.Empty;
    public string TournamentName { get; set; } = string.Empty;
    public DateTime StartDate { get; set; }
    public Game Game { get; set; }
    public string Description { get; set; } = string.Empty;
    public bool IsInterUniversity { get; set; }
    public int? UniversityId { get; set; }
    public int MaxTeams { get; set; }
    public OrganizerRequestStatus Status { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? ReviewedAt { get; set; }

    public User User { get; set; } = null!;
    public University? University { get; set; }
}
