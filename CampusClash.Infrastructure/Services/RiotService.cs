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
        var encodedName = Uri.EscapeDataString(gameName);
        var encodedTag  = Uri.EscapeDataString(tagLine);
        var url = $"https://americas.api.riotgames.com/riot/account/v1/accounts/by-riot-id/{encodedName}/{encodedTag}?api_key={_apiKey}";

        var response = await _httpClient.GetAsync(url);

        if (!response.IsSuccessStatusCode)
        {
            var body = await response.Content.ReadAsStringAsync();
            throw new Exception($"Riot API respondió {(int)response.StatusCode}: {body}");
        }

        var content = await response.Content.ReadAsStringAsync();

        return JsonSerializer.Deserialize<RiotAccountDto>(content, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        });
    }
}