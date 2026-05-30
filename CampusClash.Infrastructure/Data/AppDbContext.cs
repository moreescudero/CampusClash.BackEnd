using CampusClash.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace CampusClash.Infrastructure.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<User> Users { get; set; }
    public DbSet<University> Universities { get; set; }
    public DbSet<ValidationRequest> ValidationRequests { get; set; }
    public DbSet<OrganizerRequest> OrganizerRequests { get; set; }
    public DbSet<Tournament> Tournaments { get; set; }
    public DbSet<Team> Teams { get; set; }
    public DbSet<Enrollment> Enrollments { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(u => u.Id);
            entity.Property(u => u.Email).IsRequired().HasMaxLength(255);
            entity.HasIndex(u => u.Email).IsUnique();
            entity.Property(u => u.Username).IsRequired().HasMaxLength(100);
            entity.Property(u => u.PasswordHash).IsRequired();
        });

        modelBuilder.Entity<University>(entity =>
        {
            entity.HasKey(u => u.Id);
            entity.Property(u => u.Name).IsRequired().HasMaxLength(255);
            entity.Property(u => u.ShortName).IsRequired().HasMaxLength(20);
        });

        modelBuilder.Entity<ValidationRequest>(entity =>
        {
            entity.HasKey(v => v.Id);
            entity.HasOne(v => v.User)
                  .WithOne(u => u.ValidationRequest)
                  .HasForeignKey<ValidationRequest>(v => v.UserId);
            entity.HasOne(v => v.University)
                  .WithMany(u => u.ValidationRequests)
                  .HasForeignKey(v => v.UniversityId);
        });

        modelBuilder.Entity<OrganizerRequest>(entity =>
        {
            entity.HasKey(o => o.Id);
            entity.Property(o => o.Game).IsRequired().HasConversion<string>();
            entity.HasOne(o => o.User)
                  .WithMany()
                  .HasForeignKey(o => o.UserId)
                  .OnDelete(DeleteBehavior.Restrict);
            entity.HasOne(o => o.University)
                  .WithMany()
                  .HasForeignKey(o => o.UniversityId)
                  .IsRequired(false)
                  .OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<Tournament>(entity =>
        {
            entity.HasKey(t => t.Id);
            entity.Property(t => t.Name).IsRequired().HasMaxLength(255);
            entity.Property(t => t.Game).IsRequired().HasConversion<string>();
            entity.HasOne(t => t.CreatedBy)
                  .WithMany()
                  .HasForeignKey(t => t.CreatedByUserId)
                  .OnDelete(DeleteBehavior.Restrict);
            entity.HasOne(t => t.University)
                  .WithMany()
                  .HasForeignKey(t => t.UniversityId)
                  .IsRequired(false)
                  .OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<Team>(entity =>
        {
            entity.HasKey(t => t.Id);
            entity.Property(t => t.Name).IsRequired().HasMaxLength(100);
            entity.HasOne(t => t.University)
                  .WithMany(u => u.Teams)
                  .HasForeignKey(t => t.UniversityId);
            entity.HasOne(t => t.Tournament)
                  .WithMany(t => t.Teams)
                  .HasForeignKey(t => t.TournamentId);
        });

        modelBuilder.Entity<Enrollment>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.HasOne(e => e.User)
                  .WithMany(u => u.Enrollments)
                  .HasForeignKey(e => e.UserId);
            entity.HasOne(e => e.Team)
                  .WithMany(t => t.Enrollments)
                  .HasForeignKey(e => e.TeamId);
            entity.HasIndex(e => new { e.UserId, e.TeamId }).IsUnique();
        });
    }
}
