using CampusClash.Domain.Entities;

namespace CampusClash.Application.Interfaces;

public interface IEnrollmentRepository
{
    Task<bool> IsUserInActiveTournamentAsync(Guid userId);
    Task AddAsync(Enrollment enrollment);
    Task SaveChangesAsync();
}
