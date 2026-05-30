using CampusClash.Domain.Entities;

namespace CampusClash.Application.Interfaces;

public interface IOrganizerRequestRepository
{
    Task<OrganizerRequest?> GetByUserIdAsync(Guid userId);
    Task<OrganizerRequest?> GetByUserEmailAsync(string email);
    Task AddAsync(OrganizerRequest request);
    Task SaveChangesAsync();
}
