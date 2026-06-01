using CampusClash.Domain.Entities;

namespace CampusClash.Application.Interfaces;

public interface IUniversityRepository
{
    Task<List<University>> GetAllAsync();
}
