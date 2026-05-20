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
        var url = $"https://americas.api.riotgames.com/riot/account/v1/accounts/by-riot-id/{gameName}/{tagLine}?api_key={_apiKey}";

        var response = await _httpClient.GetAsync(url);

        if (!response.IsSuccessStatusCode)
            return null;

        var content = await response.Content.ReadAsStringAsync();

        var account = JsonSerializer.Deserialize<RiotAccountDto>(content, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        });

        return account;
    }
}