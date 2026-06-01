namespace CampusClash.Application.DTOs.Validation;

public class ValidationRequestDto
{
    public string Legajo { get; set; } = string.Empty;
    public int University { get; set; }
    public string Faculty { get; set; } = string.Empty;
    public string Career { get; set; } = string.Empty;
    public int Year { get; set; }
    public string CertificateUrl { get; set; } = string.Empty;
}