using CampusClash.Application.Interfaces;
using CampusClash.Domain.Entities;
using CampusClash.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace CampusClash.Infrastructure.Repositories;

public class ValidationRepository : IValidationRepository
{
    private readonly AppDbContext _context;

    public ValidationRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<ValidationRequest?> GetByUserIdAsync(Guid userId)
        => await _context.ValidationRequests
            .Include(v => v.University)
            .FirstOrDefaultAsync(v => v.UserId == userId);

    public async Task<ValidationRequest?> GetByUserEmailAsync(string email)
        => await _context.ValidationRequests
            .Include(v => v.University)
            .FirstOrDefaultAsync(v => v.UserEmail == email);

    public async Task AddAsync(ValidationRequest request)
        => await _context.ValidationRequests.AddAsync(request);

    public async Task SaveChangesAsync()
        => await _context.SaveChangesAsync();
}