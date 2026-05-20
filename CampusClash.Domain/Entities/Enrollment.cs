namespace CampusClash.Domain.Entities;

public class Enrollment
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public Guid TeamId { get; set; }
    public DateTime EnrolledAt { get; set; }

    public User User { get; set; } = null!;
    public Team Team { get; set; } = null!;
}