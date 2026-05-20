using CampusClash.Domain.Entities;
using CampusClash.Domain.Enums;

namespace CampusClash.Application.Interfaces;

public interface IValidationRepository
{
    Task<ValidationRequest?> GetByUserIdAsync(Guid userId);
    Task AddAsync(ValidationRequest request);
    Task SaveChangesAsync();
}