using CampusClash.Domain.Entities;

namespace CampusClash.Application.Interfaces;

public interface ITeamRepository
{
    Task<Team?> GetNonFullByTournamentAndUniversityAsync(Guid tournamentId, int universityId);
    Task<int> GetCountByTournamentAsync(Guid tournamentId);
    Task<int> GetCountByTournamentAndUniversityAsync(Guid tournamentId, int universityId);
    Task AddAsync(Team team);
    Task SaveChangesAsync();
}
