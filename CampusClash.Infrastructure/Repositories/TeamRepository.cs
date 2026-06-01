using CampusClash.Application.Interfaces;
using CampusClash.Domain.Entities;
using CampusClash.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace CampusClash.Infrastructure.Repositories;

public class TeamRepository : ITeamRepository
{
    private const int TeamSize = 5;
    private readonly AppDbContext _context;

    public TeamRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<Team?> GetNonFullByTournamentAndUniversityAsync(Guid tournamentId, int universityId)
        => await _context.Teams
            .Include(t => t.Enrollments)
            .Include(t => t.University)
            .FirstOrDefaultAsync(t =>
                t.TournamentId == tournamentId &&
                t.UniversityId == universityId &&
                t.Enrollments.Count < TeamSize);

    public async Task<int> GetCountByTournamentAsync(Guid tournamentId)
        => await _context.Teams.CountAsync(t => t.TournamentId == tournamentId);

    public async Task<int> GetCountByTournamentAndUniversityAsync(Guid tournamentId, int universityId)
        => await _context.Teams.CountAsync(t =>
            t.TournamentId == tournamentId &&
            t.UniversityId == universityId);

    public async Task AddAsync(Team team)
        => await _context.Teams.AddAsync(team);

    public async Task SaveChangesAsync()
        => await _context.SaveChangesAsync();
}
