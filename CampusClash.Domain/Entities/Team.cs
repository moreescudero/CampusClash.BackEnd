namespace CampusClash.Domain.Entities;

public class Team
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public int UniversityId { get; set; }
    public Guid TournamentId { get; set; }
    public DateTime CreatedAt { get; set; }

    public University University { get; set; } = null!;
    public Tournament Tournament { get; set; } = null!;
    public ICollection<Enrollment> Enrollments { get; set; } = new List<Enrollment>();
}