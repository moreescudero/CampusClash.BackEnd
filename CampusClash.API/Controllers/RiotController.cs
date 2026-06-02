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
    public async Task<IActionResult> LinkRiotAccount([FromBody] JsonElement body)
    {
        try
        {
            var userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
            
            var summonerName = body.GetProperty("summonerName").GetString()!;
            var parts = summonerName.Split('#');
            
            if (parts.Length != 2)
                return BadRequest(new { message = "Formato inválido. Usá NombreDeJuego#Tag" });

            var request = new LinkRiotRequestDto
            {
                GameName = parts[0],
                TagLine = parts[1]
            };

            var result = await _riotLinkService.LinkRiotAccountAsync(userId, request);
            return Ok(new { message = "Cuenta de Riot vinculada correctamente." });
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }
}