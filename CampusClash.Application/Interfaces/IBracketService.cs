using CampusClash.Application.DTOs.Bracket;

namespace CampusClash.Application.Interfaces;

public interface IBracketService
{
    Task<BracketResponseDto> GenerateAsync(Guid tournamentId, Guid organizerId);
    Task<BracketResponseDto> GetAsync(Guid tournamentId);
    Task<MatchDto> ScheduleMatchAsync(Guid tournamentId, Guid matchId, Guid organizerId, DateTime scheduledAt);
}
