using CampusClash.Application.Interfaces;
using CampusClash.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace CampusClash.Infrastructure.Services;

public class LobbyAutoCreateService : BackgroundService
{
    private readonly IServiceScopeFactory _scopeFactory;
    private readonly ILogger<LobbyAutoCreateService> _logger;

    public LobbyAutoCreateService(IServiceScopeFactory scopeFactory, ILogger<LobbyAutoCreateService> logger)
    {
        _scopeFactory = scopeFactory;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                await CheckAndCreateLobbiesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error en LobbyAutoCreateService");
            }

            await Task.Delay(TimeSpan.FromMinutes(1), stoppingToken);
        }
    }

    private async Task CheckAndCreateLobbiesAsync()
    {
        using var scope = _scopeFactory.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        var lcuService = scope.ServiceProvider.GetRequiredService<ILcuService>();

        var now = DateTime.UtcNow;
        var windowStart = now.AddMinutes(475);
        var windowEnd = now.AddMinutes(485);

        // Partidos que arrancan en ~1 hora y tienen sesión LCU registrada pero lobby aún no creado
        var pending = await db.LcuSessions
            .Include(s => s.Match)
            .Where(s =>
                !s.LobbyCreated &&
                s.Match.ScheduledAt.HasValue &&
                s.Match.ScheduledAt.Value >= windowStart &&
                s.Match.ScheduledAt.Value <= windowEnd)
            .ToListAsync();

        foreach (var session in pending)
        {
            try
            {
                _logger.LogInformation(
                    "Creando lobby automáticamente para partido {MatchId} (programado para {ScheduledAt})",
                    session.MatchId, session.Match.ScheduledAt);

                await lcuService.CreateLobbyAsync(session.MatchId);

                session.LobbyCreated = true;
                await db.SaveChangesAsync();

                _logger.LogInformation("Lobby creado exitosamente para partido {MatchId}", session.MatchId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "No se pudo crear el lobby para partido {MatchId}", session.MatchId);
            }
        }
    }
}
