using CampusClash.Domain.Entities;

namespace CampusClash.Application.Interfaces;

public interface IBracketRepository
{
    Task<bool> ExistsForTournamentAsync(Guid tournamentId);
    Task AddMatchesAsync(IEnumerable<TournamentMatch> matches);
    Task<List<TournamentMatch>> GetByTournamentAsync(Guid tournamentId);
    Task SaveChangesAsync();
}
