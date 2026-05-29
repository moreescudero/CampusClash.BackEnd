using CampusClash.Domain.Entities;
using CampusClash.Domain.Enums;

namespace CampusClash.Application.Interfaces;

public interface IValidationRepository
{
    Task<ValidationRequest?> GetByUserIdAsync(Guid userId);
    Task<ValidationRequest?> GetByUserEmailAsync(string email);
    Task AddAsync(ValidationRequest request);
    Task SaveChangesAsync();
}