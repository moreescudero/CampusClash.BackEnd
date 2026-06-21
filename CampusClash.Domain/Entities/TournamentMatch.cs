namespace CampusClash.Domain.Entities;

public class TournamentMatch
{
    public Guid Id { get; set; }
    public Guid TournamentId { get; set; }
    public int Round { get; set; }
    public string RoundName { get; set; } = string.Empty;
    public int MatchNumber { get; set; }
    public Guid? TeamAId { get; set; }
    public Guid? TeamBId { get; set; }
    public Guid? WinnerId { get; set; }
    public DateTime? ScheduledAt { get; set; }
    public string? RiotLobbyCode { get; set; }

    public Tournament Tournament { get; set; } = null!;
    public Team? TeamA { get; set; }
    public Team? TeamB { get; set; }
    public Team? Winner { get; set; }
}
