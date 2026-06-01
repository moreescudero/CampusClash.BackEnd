using CampusClash.Application.DTOs.Auth;
using CampusClash.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace CampusClash.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;

    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterRequestDto request)
    {
        try
        {
            var response = await _authService.RegisterAsync(request);
            return Ok(response);
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequestDto request)
    {
        try
        {
            var response = await _authService.LoginAsync(request);
            return Ok(response);
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpGet("confirm-email")]
    public async Task<IActionResult> ConfirmEmail([FromQuery] string email, [FromQuery] string token)
    {
        var result = await _authService.ConfirmEmailAsync(email, token);
        if (!result)
            return BadRequest(new { message = "No se pudo confirmar el email." });

        return Ok(new { message = "Email confirmado correctamente." });
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