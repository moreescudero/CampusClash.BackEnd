using CampusClash.Domain.Enums;

namespace CampusClash.Domain.Entities;

public class Tournament
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Game { get; set; } = string.Empty;
    public int MaxTeams { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EnrollmentDeadline { get; set; }
    public TournamentStatus Status { get; set; }
    public Guid CreatedByUserId { get; set; }
    public DateTime CreatedAt { get; set; }

    public User CreatedBy { get; set; } = null!;
    public ICollection<Team> Teams { get; set; } = new List<Team>();
}