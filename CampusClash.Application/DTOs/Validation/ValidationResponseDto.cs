using CampusClash.Domain.Enums;

namespace CampusClash.Application.DTOs.Validation;

public class ValidationResponseDto
{
    public Guid Id { get; set; }
    public string UniversityName { get; set; } = string.Empty;
    public ValidationStatus Status { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? ReviewedAt { get; set; }
}