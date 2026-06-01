using CampusClash.Application.DTOs.Riot;
using CampusClash.Application.Interfaces;
using Microsoft.Extensions.Configuration;
using System.Text.Json;

namespace CampusClash.Infrastructure.Services;

public class RiotService : IRiotService
{
    private readonly HttpClient _httpClient;
    private readonly string _apiKey;

    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        PropertyNameCaseInsensitive = true
    };

    public RiotService(HttpClient httpClient, IConfiguration configuration)
    {
        _httpClient = httpClient;
        _apiKey = configuration["RiotGames:ApiKey"]!;
        Console.WriteLine($"RiotService - API Key cargada: {_apiKey?.Substring(0, 10)}...");
    }

    public async Task<RiotAccountDto?> GetAccountByRiotIdAsync(string gameName, string tagLine)
    {
        var encodedName = Uri.EscapeDataString(gameName);
        var encodedTag  = Uri.EscapeDataString(tagLine);
        var url = $"https://americas.api.riotgames.com/riot/account/v1/accounts/by-riot-id/{encodedName}/{encodedTag}";

        using var request = new HttpRequestMessage(HttpMethod.Get, url);
        request.Headers.Add("X-Riot-Token", _apiKey);

        var response = await _httpClient.SendAsync(request);

        if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
            return null;

        if (!response.IsSuccessStatusCode)
        {
            var body = await response.Content.ReadAsStringAsync();
            throw new Exception($"Riot API respondió {(int)response.StatusCode}: {body}");
        }

        var content = await response.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<RiotAccountDto>(content, JsonOptions);
    }
}
