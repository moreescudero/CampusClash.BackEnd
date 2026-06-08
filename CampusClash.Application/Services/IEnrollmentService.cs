using CampusClash.Application.DTOs.Tournament;

namespace CampusClash.Application.Interfaces;

public interface IEnrollmentService
{
    Task<EnrollResponseDto> EnrollAsync(Guid userId, Guid tournamentId);
    Task LeaveAsync(Guid userId, Guid tournamentId);
}
