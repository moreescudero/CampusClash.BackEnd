using CampusClash.Application.DTOs.Riot;
using CampusClash.Application.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace CampusClash.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class RiotController : ControllerBase
{
    private readonly RiotLinkService _riotLinkService;

    public RiotController(RiotLinkService riotLinkService)
    {
        _riotLinkService = riotLinkService;
    }

    [HttpPost("link")]
    public async Task<IActionResult> LinkRiotAccount([FromBody] LinkRiotRequestDto request)
    {
        try
        {
            var userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
            var result = await _riotLinkService.LinkRiotAccountAsync(userId, request);
            return Ok(new { message = "Cuenta de Riot vinculada correctamente." });
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpGet("debug-riot-key")]
    public IActionResult DebugRiotKey([FromServices] IConfiguration config)
    {
        var key = config["RiotGames:ApiKey"];
        return Ok(new { 
            keyLength = key?.Length ?? 0,
            keyStart = key?.Substring(0, Math.Min(10, key?.Length ?? 0)) ?? "EMPTY"
        });
    }
}