using CampusClash.Application.Interfaces;
using CampusClash.Domain.Entities;
using CampusClash.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace CampusClash.Infrastructure.Repositories;

public class BracketRepository : IBracketRepository
{
    private readonly AppDbContext _context;

    public BracketRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<bool> ExistsForTournamentAsync(Guid tournamentId)
        => await _context.TournamentMatches.AnyAsync(m => m.TournamentId == tournamentId);

    public async Task AddMatchesAsync(IEnumerable<TournamentMatch> matches)
        => await _context.TournamentMatches.AddRangeAsync(matches);

    public async Task<List<TournamentMatch>> GetByTournamentAsync(Guid tournamentId)
        => await _context.TournamentMatches
            .Where(m => m.TournamentId == tournamentId)
            .OrderBy(m => m.Round)
            .ThenBy(m => m.MatchNumber)
            .ToListAsync();

    public async Task SaveChangesAsync()
        => await _context.SaveChangesAsync();
}
