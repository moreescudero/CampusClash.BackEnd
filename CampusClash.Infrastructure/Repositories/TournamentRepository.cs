using CampusClash.Application.Interfaces;
using CampusClash.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace CampusClash.Infrastructure.Repositories;

public class TournamentRepository : ITournamentRepository
{
    private readonly AppDbContext _context;

    public TournamentRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<List<Domain.Entities.Tournament>> GetAllAsync()
        => await _context.Tournaments
            .Include(t => t.CreatedBy)
            .Include(t => t.University)
            .Include(t => t.Teams).ThenInclude(team => team.University)
            .Include(t => t.Teams).ThenInclude(team => team.Enrollments).ThenInclude(e => e.User)
            .ToListAsync();

    public async Task<List<Domain.Entities.Tournament>> GetByUserEnrollmentAsync(Guid userId)
        => await _context.Tournaments
            .Include(t => t.CreatedBy)
            .Include(t => t.University)
            .Include(t => t.Teams).ThenInclude(team => team.University)
            .Include(t => t.Teams).ThenInclude(team => team.Enrollments).ThenInclude(e => e.User)
            .Where(t => t.Teams.Any(team => team.Enrollments.Any(e => e.UserId == userId)))
            .ToListAsync();

    public async Task<Domain.Entities.Tournament?> GetByIdAsync(Guid id)
        => await _context.Tournaments
            .Include(t => t.CreatedBy)
            .Include(t => t.University)
            .Include(t => t.Teams).ThenInclude(team => team.University)
            .Include(t => t.Teams).ThenInclude(team => team.Enrollments).ThenInclude(e => e.User)
            .FirstOrDefaultAsync(t => t.Id == id);

    public async Task<Domain.Entities.Tournament?> GetByOrganizerIdAsync(Guid organizerId)
        => await _context.Tournaments
            .FirstOrDefaultAsync(t => t.CreatedByUserId == organizerId);

    public async Task AddAsync(Domain.Entities.Tournament tournament)
        => await _context.Tournaments.AddAsync(tournament);

    public async Task SaveChangesAsync()
        => await _context.SaveChangesAsync();
}
