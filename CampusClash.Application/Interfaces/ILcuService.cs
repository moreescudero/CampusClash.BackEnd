namespace CampusClash.Application.Interfaces;

public interface ILcuService
{
    Task RegisterSessionAsync(Guid matchId, string baseUrl, string authToken);
    Task CreateLobbyAsync(Guid matchId);
    Task InvitePlayersAsync(Guid matchId, List<string> summonerNames);
    Task<string> GetLobbyStatusAsync(Guid matchId);
    Task MarkLobbyCreatedAsync(Guid matchId);
}
