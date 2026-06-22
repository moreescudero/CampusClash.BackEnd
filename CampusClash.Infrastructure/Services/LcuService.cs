using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using CampusClash.Application.Interfaces;
using CampusClash.Domain.Entities;

namespace CampusClash.Infrastructure.Services;

public class LcuService : ILcuService
{
    private readonly ILcuRepository _lcuRepository;
    private readonly IHttpClientFactory _httpClientFactory;

    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        PropertyNameCaseInsensitive = true
    };

    public LcuService(ILcuRepository lcuRepository, IHttpClientFactory httpClientFactory)
    {
        _lcuRepository = lcuRepository;
        _httpClientFactory = httpClientFactory;
    }

    public async Task RegisterSessionAsync(Guid matchId, string baseUrl, string authToken)
    {
        var existing = await _lcuRepository.GetByMatchIdAsync(matchId);
        if (existing is not null)
        {
            existing.BaseUrl = baseUrl.TrimEnd('/');
            existing.AuthToken = authToken;
            existing.CreatedAt = DateTime.UtcNow;
            existing.LobbyCreated = false;
        }
        else
        {
            await _lcuRepository.AddAsync(new LcuSession
            {
                Id = Guid.NewGuid(),
                MatchId = matchId,
                BaseUrl = baseUrl.TrimEnd('/'),
                AuthToken = authToken,
                CreatedAt = DateTime.UtcNow
            });
        }
        await _lcuRepository.SaveChangesAsync();
    }

    public async Task CreateLobbyAsync(Guid matchId)
    {
        var session = await GetSessionOrThrow(matchId);
        var client = BuildClient(session);

        var body = new
        {
            customGameLobby = new
            {
                configuration = new
                {
                    gameMode = "CLASSIC",
                    mapId = 11,
                    teamSize = 5,
                    spectatorPolicy = "AllAllowed",
                    pickType = "",
                    customMutatorName = "SimulPickStrategy"
                },
                lobbyName = $"CampusClash - Match {matchId}",
                lobbyPassword = ""
            },
            isCustom = true,
            queueId = 3100
        };

        var response = await client.PostAsJsonAsync($"{session.BaseUrl}/lol-lobby/v2/lobby", body);
        if (!response.IsSuccessStatusCode)
        {
            var error = await response.Content.ReadAsStringAsync();
            throw new Exception($"Error creando lobby en LCU ({(int)response.StatusCode}): {error}");
        }
    }

    public async Task InvitePlayersAsync(Guid matchId, List<string> summonerNames)
    {
        var session = await GetSessionOrThrow(matchId);
        var client = BuildClient(session);

        var summonerIds = new List<long>();
        var failed = new List<string>();

        foreach (var name in summonerNames)
        {
            var encoded = Uri.EscapeDataString(name);
            var res = await client.GetAsync($"{session.BaseUrl}/lol-summoner/v1/summoners?name={encoded}");
            if (!res.IsSuccessStatusCode)
            {
                failed.Add(name);
                continue;
            }

            var json = await res.Content.ReadAsStringAsync();
            using var doc = JsonDocument.Parse(json);
            if (doc.RootElement.TryGetProperty("summonerId", out var idProp))
                summonerIds.Add(idProp.GetInt64());
            else
                failed.Add(name);
        }

        if (summonerIds.Count == 0)
            throw new Exception("No se pudo resolver ningún nombre de invocador a summonerId.");

        var invitations = summonerIds.Select(id => new { toSummonerId = id }).ToArray();
        var invRes = await client.PostAsJsonAsync($"{session.BaseUrl}/lol-lobby/v2/lobby/invitations", invitations);
        if (!invRes.IsSuccessStatusCode)
        {
            var error = await invRes.Content.ReadAsStringAsync();
            throw new Exception($"Error enviando invitaciones ({(int)invRes.StatusCode}): {error}");
        }
    }

    public async Task<string> GetLobbyStatusAsync(Guid matchId)
    {
        var session = await GetSessionOrThrow(matchId);
        var client = BuildClient(session);

        var res = await client.GetAsync($"{session.BaseUrl}/lol-lobby/v2/lobby/members");
        if (!res.IsSuccessStatusCode)
        {
            var error = await res.Content.ReadAsStringAsync();
            throw new Exception($"Error consultando miembros del lobby ({(int)res.StatusCode}): {error}");
        }

        return await res.Content.ReadAsStringAsync();
    }

    public async Task MarkLobbyCreatedAsync(Guid matchId)
    {
        var session = await GetSessionOrThrow(matchId);
        session.LobbyCreated = true;
        await _lcuRepository.SaveChangesAsync();
    }

    private async Task<LcuSession> GetSessionOrThrow(Guid matchId)
        => await _lcuRepository.GetByMatchIdAsync(matchId)
           ?? throw new Exception("No hay sesión LCU registrada para este partido. Ejecutá POST /api/lcu/register primero.");

    private HttpClient BuildClient(LcuSession session)
    {
        var client = _httpClientFactory.CreateClient("lcu");
        var credentials = Convert.ToBase64String(Encoding.UTF8.GetBytes($"riot:{session.AuthToken}"));
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", credentials);
        return client;
    }
}
