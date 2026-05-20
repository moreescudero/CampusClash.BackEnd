namespace CampusClash.Application.DTOs.Validation;

public class ValidationRequestDto
{
    public Guid UniversityId { get; set; }
    public string CertificateUrl { get; set; } = string.Empty;
}