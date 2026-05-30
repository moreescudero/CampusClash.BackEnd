using CampusClash.Domain.Entities;

namespace CampusClash.Application.Interfaces;

public interface ITournamentRepository
{
    Task<List<Domain.Entities.Tournament>> GetAllAsync();
    Task<List<Domain.Entities.Tournament>> GetByUserEnrollmentAsync(Guid userId);
    Task<Domain.Entities.Tournament?> GetByIdAsync(Guid id);
    Task<Domain.Entities.Tournament?> GetByOrganizerIdAsync(Guid organizerId);
    Task AddAsync(Domain.Entities.Tournament tournament);
    Task SaveChangesAsync();
}
