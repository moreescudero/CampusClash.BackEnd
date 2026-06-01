using CampusClash.Application.DTOs.Riot;
using CampusClash.Application.Interfaces;
using Microsoft.Extensions.Configuration;
using System.Text.Json;

namespace CampusClash.Infrastructure.Services;

public class RiotService : IRiotService
{
    private readonly HttpClient _httpClient;
    private readonly string _apiKey;

    public RiotService(HttpClient httpClient, IConfiguration configuration)
    {
        _httpClient = httpClient;
        _apiKey = configuration["RiotGames:ApiKey"]!;
    }

    public async Task<RiotAccountDto?> GetAccountByRiotIdAsync(string gameName, string tagLine)
    {
        // El Account v1 requiere producto RSO habilitado; usamos el Summoner v4 de LoL
        // que funciona con dev keys estándar
        var encodedName = Uri.EscapeDataString(gameName);
        var baseUrl = _httpClient.BaseAddress?.ToString().TrimEnd('/')
                      ?? "https://la1.api.riotgames.com";
        var url = $"{baseUrl}/lol/summoner/v4/summoners/by-name/{encodedName}?api_key={_apiKey}";

        var response = await _httpClient.GetAsync(url);

        if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
            return null;

        if (!response.IsSuccessStatusCode)
        {
            var body = await response.Content.ReadAsStringAsync();
            throw new Exception($"Riot API respondió {(int)response.StatusCode}: {body}");
        }

        var content = await response.Content.ReadAsStringAsync();
        var summoner = JsonSerializer.Deserialize<SummonerDto>(content, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        });

        if (summoner is null) return null;

        return new RiotAccountDto
        {
            Puuid    = summoner.Puuid,
            GameName = gameName,
            TagLine  = tagLine
        };
    }

    private sealed class SummonerDto
    {
        public string Puuid { get; set; } = string.Empty;
        public string Name  { get; set; } = string.Empty;
    }
}