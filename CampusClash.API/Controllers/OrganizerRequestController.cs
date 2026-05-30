using CampusClash.Application.DTOs.OrganizerRequest;
using CampusClash.Application.Interfaces;
using CampusClash.API.Filters;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace CampusClash.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class OrganizerRequestController : ControllerBase
{
    private readonly IOrganizerRequestService _service;

    public OrganizerRequestController(IOrganizerRequestService service)
    {
        _service = service;
    }

    [HttpPost]
    [Authorize]
    public async Task<IActionResult> Submit([FromBody] OrganizerRequestDto dto)
    {
        try
        {
            var userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
            var response = await _service.SubmitAsync(userId, dto);
            return Ok(response);
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpPost("approve/{email}")]
    [AdminApiKey]
    [AllowAnonymous]
    public async Task<IActionResult> Approve(string email)
    {
        try
        {
            var response = await _service.ApproveAsync(email);
            return Ok(response);
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpPost("reject/{email}")]
    [AdminApiKey]
    [AllowAnonymous]
    public async Task<IActionResult> Reject(string email)
    {
        try
        {
            var response = await _service.RejectAsync(email);
            return Ok(response);
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }
}
