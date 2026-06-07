using CampusClash.Application.Interfaces;
using CampusClash.Domain.Entities;
using CampusClash.Domain.Enums;
using CampusClash.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace CampusClash.Infrastructure.Repositories;

public class EnrollmentRepository : IEnrollmentRepository
{
    private readonly AppDbContext _context;

    public EnrollmentRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<bool> IsUserInActiveTournamentAsync(Guid userId)
        => await _context.Enrollments
            .Include(e => e.Team).ThenInclude(t => t.Tournament)
            .AnyAsync(e =>
                e.UserId == userId &&
                (e.Team.Tournament.Status == TournamentStatus.Open ||
                 e.Team.Tournament.Status == TournamentStatus.InProgress));

    public async Task<Enrollment?> GetByUserAndTournamentAsync(Guid userId, Guid tournamentId)
        => await _context.Enrollments
            .Include(e => e.Team)
            .ThenInclude(t => t.Enrollments)
            .FirstOrDefaultAsync(e =>
                e.UserId == userId &&
                e.Team.TournamentId == tournamentId);

    public void Remove(Enrollment enrollment)
        => _context.Enrollments.Remove(enrollment);

    public async Task AddAsync(Enrollment enrollment)
        => await _context.Enrollments.AddAsync(enrollment);

    public async Task SaveChangesAsync()
        => await _context.SaveChangesAsync();
}
