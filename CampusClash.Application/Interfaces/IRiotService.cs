using CampusClash.Application.DTOs.Riot;

namespace CampusClash.Application.Interfaces;

public interface IRiotService
{
    Task<RiotAccountDto?> GetAccountByRiotIdAsync(string gameName, string tagLine);
    Task<int> CreateRiotTournamentAsync(string tournamentName);
    Task<string> CreateTournamentCodeAsync(int riotTournamentId, string metadata);
}