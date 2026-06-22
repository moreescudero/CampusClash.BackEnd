using CampusClash.Domain.Entities;

namespace CampusClash.Application.Interfaces;

public interface ILcuRepository
{
    Task<LcuSession?> GetByMatchIdAsync(Guid matchId);
    Task AddAsync(LcuSession session);
    Task SaveChangesAsync();
}
