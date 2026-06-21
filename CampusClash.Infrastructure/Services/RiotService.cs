using CampusClash.Application.DTOs.Riot;
using CampusClash.Application.Interfaces;
using Microsoft.Extensions.Configuration;
using System.Net.Http.Json;
using System.Text.Json;

namespace CampusClash.Infrastructure.Services;

public class RiotService : IRiotService
{
    private readonly HttpClient _httpClient;
    private readonly string _apiKey;
    private readonly string _tournamentBaseUrl;
    private readonly int _providerId;

    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        PropertyNameCaseInsensitive = true
    };

    public RiotService(HttpClient httpClient, IConfiguration configuration)
    {
        _httpClient = httpClient;
        _apiKey = configuration["RiotGames:ApiKey"]!;
        _providerId = configuration.GetValue<int>("RiotGames:ProviderId");

        // Usar stub API con dev key; cambiar a false cuando se tenga key de producción
        var useStub = configuration.GetValue<bool>("RiotGames:UseStubApi", defaultValue: true);
        _tournamentBaseUrl = useStub
            ? "https://americas.api.riotgames.com/lol/tournament-stub/v5"
            : "https://americas.api.riotgames.com/lol/tournament/v5";
    }

    public async Task<RiotAccountDto?> GetAccountByRiotIdAsync(string gameName, string tagLine)
    {
        var url = $"https://americas.api.riotgames.com/riot/account/v1/accounts/by-riot-id/{Uri.EscapeDataString(gameName)}/{Uri.EscapeDataString(tagLine)}";
        using var request = new HttpRequestMessage(HttpMethod.Get, url);
        request.Headers.Add("X-Riot-Token", _apiKey);

        var response = await _httpClient.SendAsync(request);

        if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
            return null;

        if (!response.IsSuccessStatusCode)
            throw new Exception($"Riot API respondió {(int)response.StatusCode}: {await response.Content.ReadAsStringAsync()}");

        return JsonSerializer.Deserialize<RiotAccountDto>(await response.Content.ReadAsStringAsync(), JsonOptions);
    }

    public async Task<int> CreateRiotTournamentAsync(string tournamentName)
    {
        using var request = new HttpRequestMessage(HttpMethod.Post, $"{_tournamentBaseUrl}/tournaments");
        request.Headers.Add("X-Riot-Token", _apiKey);
        request.Content = JsonContent.Create(new { name = tournamentName, providerId = _providerId });

        var response = await _httpClient.SendAsync(request);
        if (!response.IsSuccessStatusCode)
            throw new Exception($"Error creando torneo en Riot: {await response.Content.ReadAsStringAsync()}");

        return JsonSerializer.Deserialize<int>(await response.Content.ReadAsStringAsync());
    }

    public async Task<string> CreateTournamentCodeAsync(int riotTournamentId, string metadata)
    {
        var url = $"{_tournamentBaseUrl}/codes?count=1&tournamentId={riotTournamentId}";
        using var request = new HttpRequestMessage(HttpMethod.Post, url);
        request.Headers.Add("X-Riot-Token", _apiKey);
        request.Content = JsonContent.Create(new
        {
            enoughPlayers  = false,
            mapType        = "SUMMONERS_RIFT",
            metadata,
            pickType       = "TOURNAMENT_DRAFT",
            spectatorType  = "ALL",
            teamSize       = 5
        });

        var response = await _httpClient.SendAsync(request);
        if (!response.IsSuccessStatusCode)
            throw new Exception($"Error generando código de sala Riot: {await response.Content.ReadAsStringAsync()}");

        var codes = JsonSerializer.Deserialize<string[]>(await response.Content.ReadAsStringAsync(), JsonOptions);
        return codes?[0] ?? throw new Exception("Riot no devolvió ningún código de sala.");
    }
}
