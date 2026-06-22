namespace CampusClash.Domain.Entities;

public class LcuSession
{
    public Guid Id { get; set; }
    public Guid MatchId { get; set; }
    public string BaseUrl { get; set; } = string.Empty;
    public string AuthToken { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public bool LobbyCreated { get; set; }

    public TournamentMatch Match { get; set; } = null!;
}
