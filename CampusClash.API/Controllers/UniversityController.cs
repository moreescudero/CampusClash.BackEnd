using CampusClash.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace CampusClash.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UniversityController : ControllerBase
{
    private readonly IUniversityRepository _repository;

    public UniversityController(IUniversityRepository repository)
    {
        _repository = repository;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var universities = await _repository.GetAllAsync();
        return Ok(universities.Select(u => new { u.Id, u.Name, u.ShortName }));
    }
}
