using CampusClash.Application.DTOs.Tournament;

namespace CampusClash.Application.Interfaces;

public interface ITournamentService
{
    Task<List<TournamentResponseDto>> GetAllAsync();
    Task<List<TournamentResponseDto>> GetMyTournamentsAsync(Guid userId);
    Task<TournamentResponseDto> GetByIdAsync(Guid id);
    Task<TournamentResponseDto> UpdateAsync(Guid tournamentId, Guid organizerId, UpdateTournamentDto dto);
}
