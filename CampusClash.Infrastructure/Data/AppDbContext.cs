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
    public DbSet<TournamentMatch> TournamentMatches { get; set; }

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

            entity.HasData(
                new University { Id = 1,  Name = "Universidad de Buenos Aires",        ShortName = "UBA"     },
                new University { Id = 2,  Name = "Universidad Nacional de Córdoba",    ShortName = "UNC"     },
                new University { Id = 3,  Name = "Universidad Nacional de La Plata",   ShortName = "UNLP"    },
                new University { Id = 4,  Name = "Universidad Tecnológica Nacional",   ShortName = "UTN"     },
                new University { Id = 5,  Name = "Universidad Nacional de Rosario",    ShortName = "UNR"     },
                new University { Id = 6,  Name = "Universidad Nacional de Mar del Plata", ShortName = "UNMDP" },
                new University { Id = 7,  Name = "Universidad Nacional de Tucumán",    ShortName = "UNT"     },
                new University { Id = 8,  Name = "Universidad Nacional de La Matanza", ShortName = "UNLaM"   },
                new University { Id = 9,  Name = "Universidad Nacional de Quilmes",    ShortName = "UNQ"     },
                new University { Id = 10, Name = "Universidad Argentina de la Empresa",ShortName = "UADE"    },
                new University { Id = 11, Name = "Universidad Abierta Interamericana", ShortName = "UAI"     },
                new University { Id = 12, Name = "Universidad Austral",                ShortName = "AUSTRAL" },
                new University { Id = 13, Name = "Universidad del Salvador",           ShortName = "USAL"    },
                new University { Id = 14, Name = "Universidad de Palermo",             ShortName = "UP"      },
                new University { Id = 15, Name = "Universidad Siglo 21",               ShortName = "SIGLO21" }
            );
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

        modelBuilder.Entity<TournamentMatch>(entity =>
        {
            entity.HasKey(m => m.Id);
            entity.Property(m => m.RoundName).IsRequired().HasMaxLength(50);
            entity.Property(m => m.RiotLobbyCode).HasMaxLength(100);
            entity.HasOne(m => m.Tournament)
                  .WithMany()
                  .HasForeignKey(m => m.TournamentId)
                  .OnDelete(DeleteBehavior.Cascade);
            entity.HasOne(m => m.TeamA)
                  .WithMany()
                  .HasForeignKey(m => m.TeamAId)
                  .IsRequired(false)
                  .OnDelete(DeleteBehavior.Restrict);
            entity.HasOne(m => m.TeamB)
                  .WithMany()
                  .HasForeignKey(m => m.TeamBId)
                  .IsRequired(false)
                  .OnDelete(DeleteBehavior.Restrict);
            entity.HasOne(m => m.Winner)
                  .WithMany()
                  .HasForeignKey(m => m.WinnerId)
                  .IsRequired(false)
                  .OnDelete(DeleteBehavior.Restrict);
        });
    }
}
