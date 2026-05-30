using CampusClash.Application.Interfaces;
using CampusClash.Domain.Entities;
using CampusClash.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace CampusClash.Infrastructure.Repositories;

public class OrganizerRequestRepository : IOrganizerRequestRepository
{
    private readonly AppDbContext _context;

    public OrganizerRequestRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<OrganizerRequest?> GetByUserIdAsync(Guid userId)
        => await _context.OrganizerRequests
            .Include(o => o.University)
            .FirstOrDefaultAsync(o => o.UserId == userId);

    public async Task<OrganizerRequest?> GetByUserEmailAsync(string email)
        => await _context.OrganizerRequests
            .Include(o => o.University)
            .FirstOrDefaultAsync(o => o.UserEmail == email);

    public async Task AddAsync(OrganizerRequest request)
        => await _context.OrganizerRequests.AddAsync(request);

    public async Task SaveChangesAsync()
        => await _context.SaveChangesAsync();
}
