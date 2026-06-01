using CampusClash.Application.Interfaces;
using CampusClash.Domain.Entities;
using CampusClash.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace CampusClash.Infrastructure.Repositories;

public class UniversityRepository : IUniversityRepository
{
    private readonly AppDbContext _context;

    public UniversityRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<List<University>> GetAllAsync()
        => await _context.Universities.OrderBy(u => u.Name).ToListAsync();
}
