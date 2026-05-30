namespace CampusClash.Domain.Entities;

public class User
{
    public Guid Id { get; set; }
    public string Email { get; set; } = string.Empty;
    public string PasswordHash { get; set; } = string.Empty;
    public string Username { get; set; } = string.Empty;
    public bool IsEmailConfirmed { get; set; }
    public string? RiotGameName { get; set; }
    public string? RiotTagLine { get; set; }
    public bool IsRiotLinked { get; set; }
    public bool IsOrganizer { get; set; }
    public DateTime CreatedAt { get; set; }

    public ValidationRequest? ValidationRequest { get; set; }
    public ICollection<Enrollment> Enrollments { get; set; } = new List<Enrollment>();
}