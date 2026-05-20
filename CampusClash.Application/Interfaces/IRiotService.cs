using CampusClash.Application.DTOs.Riot;

namespace CampusClash.Application.Interfaces;

public interface IRiotService
{
    Task<RiotAccountDto?> GetAccountByRiotIdAsync(string gameName, string tagLine);
}