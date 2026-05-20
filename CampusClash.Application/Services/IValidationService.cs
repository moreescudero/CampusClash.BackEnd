using CampusClash.Application.DTOs.Validation;

namespace CampusClash.Application.Interfaces;

public interface IValidationService
{
    Task<ValidationResponseDto> RequestValidationAsync(Guid userId, ValidationRequestDto request);
    Task<ValidationResponseDto> GetValidationStatusAsync(Guid userId);
}