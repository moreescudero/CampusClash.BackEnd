using CampusClash.Application.DTOs.OrganizerRequest;

namespace CampusClash.Application.Interfaces;

public interface IOrganizerRequestService
{
    Task<OrganizerRequestResponseDto> SubmitAsync(Guid userId, OrganizerRequestDto request);
    Task<OrganizerRequestResponseDto> ApproveAsync(string email);
    Task<OrganizerRequestResponseDto> RejectAsync(string email);
}
