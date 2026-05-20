using CampusClash.Application.DTOs.Validation;
using CampusClash.Application.Interfaces;
using CampusClash.Domain.Entities;
using CampusClash.Domain.Enums;

namespace CampusClash.Application.Services;

public class ValidationService : IValidationService
{
    private readonly IValidationRepository _validationRepository;
    private readonly IUserRepository _userRepository;

    public ValidationService(
        IValidationRepository validationRepository,
        IUserRepository userRepository)
    {
        _validationRepository = validationRepository;
        _userRepository = userRepository;
    }

    public async Task<ValidationResponseDto> RequestValidationAsync(
        Guid userId, ValidationRequestDto request)
    {
        var user = await _userRepository.GetByIdAsync(userId);
        if (user == null)
            throw new Exception("Usuario no encontrado.");

        var existing = await _validationRepository.GetByUserIdAsync(userId);
        if (existing != null)
            throw new Exception("Ya existe una solicitud de validación para este usuario.");

        var validation = new ValidationRequest
        {
            Id = Guid.NewGuid(),
            UserId = userId,
            UniversityId = request.UniversityId,
            CertificateUrl = request.CertificateUrl,
            Status = ValidationStatus.Pending,
            CreatedAt = DateTime.UtcNow
        };

        await _validationRepository.AddAsync(validation);
        await _validationRepository.SaveChangesAsync();

        return new ValidationResponseDto
        {
            Id = validation.Id,
            UniversityName = string.Empty,
            Status = validation.Status,
            CreatedAt = validation.CreatedAt,
            ReviewedAt = null
        };
    }

    public async Task<ValidationResponseDto> GetValidationStatusAsync(Guid userId)
    {
        var validation = await _validationRepository.GetByUserIdAsync(userId);
        if (validation == null)
            throw new Exception("No se encontró una solicitud de validación.");

        return new ValidationResponseDto
        {
            Id = validation.Id,
            UniversityName = validation.University?.Name ?? string.Empty,
            Status = validation.Status,
            CreatedAt = validation.CreatedAt,
            ReviewedAt = validation.ReviewedAt
        };
    }
}