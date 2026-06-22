using CampusClash.Application.DTOs.Lcu;
using CampusClash.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CampusClash.API.Controllers;

[ApiController]
[Route("api")]
public class LcuController : ControllerBase
{
    private readonly ILcuService _lcuService;

    public LcuController(ILcuService lcuService)
    {
        _lcuService = lcuService;
    }

    /// <summary>
    /// Registra la sesión LCU de la PC host (ngrok URL + auth token del proceso LeagueClientUx).
    /// Si ya existe una sesión para ese partido, la sobreescribe.
    /// </summary>
    [HttpPost("lcu/register")]
    [Authorize]
    public async Task<IActionResult> Register([FromBody] RegisterLcuSessionDto dto)
    {
        try
        {
            await _lcuService.RegisterSessionAsync(dto.MatchId, dto.BaseUrl, dto.AuthToken);
            return Ok(new { message = "Sesión LCU registrada correctamente." });
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    /// <summary>
    /// Crea el lobby personalizado en el cliente de League de la PC host.
    /// </summary>
    [HttpPost("matches/{matchId:guid}/create-lobby")]
    [Authorize]
    public async Task<IActionResult> CreateLobby(Guid matchId)
    {
        try
        {
            await _lcuService.CreateLobbyAsync(matchId);
            return Ok(new { message = "Lobby creado exitosamente en el cliente de League." });
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    /// <summary>
    /// Resuelve summonerNames a summonerIds y envía invitaciones desde el cliente host.
    /// </summary>
    [HttpPost("matches/{matchId:guid}/invite")]
    [Authorize]
    public async Task<IActionResult> Invite(Guid matchId, [FromBody] InvitePlayersDto dto)
    {
        if (dto.SummonerNames.Count == 0)
            return BadRequest(new { message = "Debés pasar al menos un nombre de invocador." });

        try
        {
            await _lcuService.InvitePlayersAsync(matchId, dto.SummonerNames);
            return Ok(new { message = $"Invitaciones enviadas a {dto.SummonerNames.Count} jugador(es)." });
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    /// <summary>
    /// Devuelve el estado actual del lobby (miembros conectados) consultando la LCU.
    /// </summary>
    [HttpGet("matches/{matchId:guid}/lobby-status")]
    [AllowAnonymous]
    public async Task<IActionResult> LobbyStatus(Guid matchId)
    {
        try
        {
            var json = await _lcuService.GetLobbyStatusAsync(matchId);
            return Content(json, "application/json");
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }
}
