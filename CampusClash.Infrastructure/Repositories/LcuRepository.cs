using CampusClash.Application.Interfaces;
using CampusClash.Domain.Entities;
using CampusClash.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace CampusClash.Infrastructure.Repositories;

public class LcuRepository : ILcuRepository
{
    private readonly AppDbContext _context;

    public LcuRepository(AppDbContext context)
    {
        _context = context;
    }

    public Task<LcuSession?> GetByMatchIdAsync(Guid matchId)
        => _context.LcuSessions.FirstOrDefaultAsync(s => s.MatchId == matchId);

    public async Task AddAsync(LcuSession session)
        => await _context.LcuSessions.AddAsync(session);

    public Task SaveChangesAsync()
        => _context.SaveChangesAsync();
}
