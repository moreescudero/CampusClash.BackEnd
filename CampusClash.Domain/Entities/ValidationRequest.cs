using CampusClash.Domain.Enums;

namespace CampusClash.Domain.Entities;

public class ValidationRequest
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public Guid UniversityId { get; set; }
    public string CertificateUrl { get; set; } = string.Empty;
    public ValidationStatus Status { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? ReviewedAt { get; set; }

    public User User { get; set; } = null!;
    public University University { get; set; } = null!;
}