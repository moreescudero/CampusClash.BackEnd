using CampusClash.Application.DTOs.Validation;
using CampusClash.Application.Interfaces;
using CampusClash.API.Filters;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace CampusClash.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class ValidationController : ControllerBase
{
    private readonly IValidationService _validationService;
    private readonly IWebHostEnvironment _env;

    public ValidationController(IValidationService validationService, IWebHostEnvironment env)
    {
        _validationService = validationService;
        _env = env;
    }

    [HttpPost("request")]
    public async Task<IActionResult> RequestValidation(
        [FromForm] string legajo,
        [FromForm] Guid university,
        [FromForm] string faculty,
        [FromForm] string career,
        [FromForm] int year,
        IFormFile file)
    {
        try
        {
            if (file is null || file.Length == 0)
                return BadRequest(new { message = "El archivo es requerido." });

            var uploadsDir = Path.Combine(_env.ContentRootPath, "uploads", "validations");
            Directory.CreateDirectory(uploadsDir);

            var savedName = $"{Guid.NewGuid()}{Path.GetExtension(file.FileName)}";
            var filePath = Path.Combine(uploadsDir, savedName);

            await using (var stream = System.IO.File.Create(filePath))
                await file.CopyToAsync(stream);

            var dto = new ValidationRequestDto
            {
                Legajo = legajo,
                University = university,
                Faculty = faculty,
                Career = career,
                Year = year,
                CertificateUrl = filePath
            };

            var userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
            var response = await _validationService.RequestValidationAsync(userId, dto);
            return Ok(response);
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpGet("status")]
    public async Task<IActionResult> GetStatus()
    {
        try
        {
            var userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
            var response = await _validationService.GetValidationStatusAsync(userId);
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
    public async Task<IActionResult> ApproveValidation(string email)
    {
        try
        {
            var response = await _validationService.ApproveValidationAsync(email);
            return Ok(response);
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }
}
