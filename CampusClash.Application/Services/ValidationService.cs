using CampusClash.Application.DTOs.Validation;
using CampusClash.Application.Interfaces;
using CampusClash.Domain.Entities;
using CampusClash.Domain.Enums;

namespace CampusClash.Application.Services;

public class ValidationService : IValidationService
{
    private readonly IValidationRepository _validationRepository;
    private readonly IUserRepository _userRepository;
    private readonly IEmailService _emailService;

    public ValidationService(
        IValidationRepository validationRepository,
        IUserRepository userRepository,
        IEmailService emailService)
    {
        _validationRepository = validationRepository;
        _userRepository = userRepository;
        _emailService = emailService;
    }

    public async Task<ValidationResponseDto> RequestValidationAsync(
        Guid userId, ValidationRequestDto request)
    {
        var user = await _userRepository.GetByIdAsync(userId);
        if (user is null)
            throw new Exception("Usuario no encontrado.");

        var existing = await _validationRepository.GetByUserIdAsync(userId);
        if (existing is not null)
            throw new Exception("Ya existe una solicitud de validación para este usuario.");

        var validation = new ValidationRequest
        {
            Id = Guid.NewGuid(),
            UserId = userId,
            UniversityId = request.University,
            Legajo = request.Legajo,
            Faculty = request.Faculty,
            Career = request.Career,
            Year = request.Year,
            UserEmail = user.Email,
            CertificateUrl = request.CertificateUrl,
            Status = ValidationStatus.Pending,
            CreatedAt = DateTime.UtcNow
        };

        await _validationRepository.AddAsync(validation);
        await _validationRepository.SaveChangesAsync();

        await _emailService.SendValidationRequestNotificationAsync(
            user.Email, user.Username,
            request.Legajo, request.Faculty, request.Career, request.Year,
            request.CertificateUrl);

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
        if (validation is null)
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

    public async Task<ValidationResponseDto> ApproveValidationAsync(string email)
    {
        var validation = await _validationRepository.GetByUserEmailAsync(email);
        if (validation is null)
            throw new Exception("No se encontró solicitud de validación para ese email.");

        if (validation.Status != ValidationStatus.Pending)
            throw new Exception("La solicitud no está en estado pendiente.");

        validation.Status = ValidationStatus.Approved;
        validation.ReviewedAt = DateTime.UtcNow;
        await _validationRepository.SaveChangesAsync();

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
