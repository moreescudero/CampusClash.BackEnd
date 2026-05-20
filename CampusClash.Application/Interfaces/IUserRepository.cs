using CampusClash.Domain.Entities;

namespace CampusClash.Application.Interfaces;

public interface IUserRepository
{
    Task<User?> GetByEmailAsync(string email);
    Task<User?> GetByIdAsync(Guid id);
    Task UpdateAsync(User user);
    Task AddAsync(User user);
    Task SaveChangesAsync();
}