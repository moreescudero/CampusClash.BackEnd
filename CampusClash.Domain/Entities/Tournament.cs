using CampusClash.Domain.Enums;

namespace CampusClash.Domain.Entities;

public class Tournament
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public Game Game { get; set; }
    public string Description { get; set; } = string.Empty;
    public bool IsInterUniversity { get; set; }
    public int? UniversityId { get; set; }
    public int MaxTeams { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime? EnrollmentDeadline { get; set; }
    public TournamentStatus Status { get; set; }
    public Guid CreatedByUserId { get; set; }
    public DateTime CreatedAt { get; set; }
    public int? RiotTournamentId { get; set; }

    public User CreatedBy { get; set; } = null!;
    public University? University { get; set; }
    public ICollection<Team> Teams { get; set; } = new List<Team>();
}
