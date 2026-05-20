namespace CampusClash.Domain.Entities;

public class University
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string ShortName { get; set; } = string.Empty;

    public ICollection<ValidationRequest> ValidationRequests { get; set; } = new List<ValidationRequest>();
    public ICollection<Team> Teams { get; set; } = new List<Team>();
}