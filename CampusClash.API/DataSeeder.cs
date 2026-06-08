using CampusClash.Domain.Entities;
using CampusClash.Domain.Enums;
using CampusClash.Infrastructure.Data;

namespace CampusClash.API;

public static class DataSeeder
{
    public static void Seed(AppDbContext context)
    {
        var organizerId   = Guid.Parse("a1a1a1a1-0000-0000-0000-000000000001");
        if (context.Users.Any(u => u.Id == organizerId)) return;

        var hash = BCrypt.Net.BCrypt.HashPassword("Campus123!");

        // ── IDs fijos ──────────────────────────────────────────────────────────
        var player1Id     = Guid.Parse("b1b1b1b1-0000-0000-0000-000000000001"); // UBA
        var player2Id     = Guid.Parse("b2b2b2b2-0000-0000-0000-000000000002"); // UBA
        var player3Id     = Guid.Parse("c3c3c3c3-0000-0000-0000-000000000003"); // UADE
        var player4Id     = Guid.Parse("c4c4c4c4-0000-0000-0000-000000000004"); // UADE
        var player5Id     = Guid.Parse("d5d5d5d5-0000-0000-0000-000000000005"); // UTN
        var player6Id     = Guid.Parse("d6d6d6d6-0000-0000-0000-000000000006"); // UTN
        var pendingUserId = Guid.Parse("e7e7e7e7-0000-0000-0000-000000000007"); // UNLP

        var lolId     = Guid.Parse("f1f1f1f1-0000-0000-0000-000000000001");
        var valId     = Guid.Parse("f2f2f2f2-0000-0000-0000-000000000002");
        var cs2Id     = Guid.Parse("f3f3f3f3-0000-0000-0000-000000000003");

        var teamUBALol  = Guid.Parse("e1e1e1e1-0000-0000-0000-000000000001");
        var teamUADELol = Guid.Parse("e2e2e2e2-0000-0000-0000-000000000002");
        var teamUADEVal = Guid.Parse("e3e3e3e3-0000-0000-0000-000000000003");
        var teamUTNCs2  = Guid.Parse("e4e4e4e4-0000-0000-0000-000000000004");

        // ── Usuarios ──────────────────────────────────────────────────────────
        context.Users.AddRange(
            new User
            {
                Id = organizerId, Email = "organizador@campusclash.gg", Username = "OrganizadorCC",
                PasswordHash = hash, IsEmailConfirmed = true, IsOrganizer = true,
                IsRiotLinked = true, RiotGameName = "CampusOrg", RiotTagLine = "ARG",
                CreatedAt = DateTime.UtcNow.AddDays(-40)
            },
            new User
            {
                Id = player1Id, Email = "jugador1@uba.ar", Username = "UBA_Player1",
                PasswordHash = hash, IsEmailConfirmed = true,
                IsRiotLinked = true, RiotGameName = "UBAOne", RiotTagLine = "LOL",
                CreatedAt = DateTime.UtcNow.AddDays(-25)
            },
            new User
            {
                Id = player2Id, Email = "jugador2@uba.ar", Username = "UBA_Player2",
                PasswordHash = hash, IsEmailConfirmed = true,
                CreatedAt = DateTime.UtcNow.AddDays(-22)
            },
            new User
            {
                Id = player3Id, Email = "jugador1@uade.edu.ar", Username = "UADE_Player1",
                PasswordHash = hash, IsEmailConfirmed = true,
                IsRiotLinked = true, RiotGameName = "UADEOne", RiotTagLine = "CC",
                CreatedAt = DateTime.UtcNow.AddDays(-30)
            },
            new User
            {
                Id = player4Id, Email = "jugador2@uade.edu.ar", Username = "UADE_Player2",
                PasswordHash = hash, IsEmailConfirmed = true,
                CreatedAt = DateTime.UtcNow.AddDays(-18)
            },
            new User
            {
                Id = player5Id, Email = "jugador1@utn.edu.ar", Username = "UTN_Player1",
                PasswordHash = hash, IsEmailConfirmed = true,
                CreatedAt = DateTime.UtcNow.AddDays(-50)
            },
            new User
            {
                Id = player6Id, Email = "jugador2@utn.edu.ar", Username = "UTN_Player2",
                PasswordHash = hash, IsEmailConfirmed = true,
                CreatedAt = DateTime.UtcNow.AddDays(-48)
            },
            new User
            {
                Id = pendingUserId, Email = "pendiente@unlp.edu.ar", Username = "UNLP_Nuevo",
                PasswordHash = hash, IsEmailConfirmed = true,
                CreatedAt = DateTime.UtcNow.AddDays(-3)
            }
        );

        // ── Validaciones ──────────────────────────────────────────────────────
        context.ValidationRequests.AddRange(
            new ValidationRequest
            {
                Id = Guid.NewGuid(), UserId = organizerId, UserEmail = "organizador@campusclash.gg",
                UniversityId = 10, Legajo = "1091234", Faculty = "Ingeniería",
                Career = "Ingeniería en Sistemas", Year = 3,
                CertificateUrl = "/uploads/seed/org_cert.pdf",
                Status = ValidationStatus.Approved,
                CreatedAt = DateTime.UtcNow.AddDays(-38), ReviewedAt = DateTime.UtcNow.AddDays(-36)
            },
            new ValidationRequest
            {
                Id = Guid.NewGuid(), UserId = player1Id, UserEmail = "jugador1@uba.ar",
                UniversityId = 1, Legajo = "LU001001", Faculty = "Ciencias Exactas",
                Career = "Ciencias de la Computación", Year = 2,
                CertificateUrl = "/uploads/seed/p1_cert.pdf",
                Status = ValidationStatus.Approved,
                CreatedAt = DateTime.UtcNow.AddDays(-24), ReviewedAt = DateTime.UtcNow.AddDays(-22)
            },
            new ValidationRequest
            {
                Id = Guid.NewGuid(), UserId = player2Id, UserEmail = "jugador2@uba.ar",
                UniversityId = 1, Legajo = "LU001002", Faculty = "Ciencias Exactas",
                Career = "Ciencias de la Computación", Year = 1,
                CertificateUrl = "/uploads/seed/p2_cert.pdf",
                Status = ValidationStatus.Approved,
                CreatedAt = DateTime.UtcNow.AddDays(-21), ReviewedAt = DateTime.UtcNow.AddDays(-20)
            },
            new ValidationRequest
            {
                Id = Guid.NewGuid(), UserId = player3Id, UserEmail = "jugador1@uade.edu.ar",
                UniversityId = 10, Legajo = "1090001", Faculty = "Ingeniería",
                Career = "Ingeniería en Sistemas", Year = 4,
                CertificateUrl = "/uploads/seed/p3_cert.pdf",
                Status = ValidationStatus.Approved,
                CreatedAt = DateTime.UtcNow.AddDays(-29), ReviewedAt = DateTime.UtcNow.AddDays(-27)
            },
            new ValidationRequest
            {
                Id = Guid.NewGuid(), UserId = player4Id, UserEmail = "jugador2@uade.edu.ar",
                UniversityId = 10, Legajo = "1090002", Faculty = "Ingeniería",
                Career = "Ingeniería en Informática", Year = 2,
                CertificateUrl = "/uploads/seed/p4_cert.pdf",
                Status = ValidationStatus.Approved,
                CreatedAt = DateTime.UtcNow.AddDays(-17), ReviewedAt = DateTime.UtcNow.AddDays(-16)
            },
            new ValidationRequest
            {
                Id = Guid.NewGuid(), UserId = player5Id, UserEmail = "jugador1@utn.edu.ar",
                UniversityId = 4, Legajo = "UTN00101", Faculty = "Regional Buenos Aires",
                Career = "Ingeniería en Sistemas", Year = 3,
                CertificateUrl = "/uploads/seed/p5_cert.pdf",
                Status = ValidationStatus.Approved,
                CreatedAt = DateTime.UtcNow.AddDays(-49), ReviewedAt = DateTime.UtcNow.AddDays(-47)
            },
            new ValidationRequest
            {
                Id = Guid.NewGuid(), UserId = player6Id, UserEmail = "jugador2@utn.edu.ar",
                UniversityId = 4, Legajo = "UTN00102", Faculty = "Regional Buenos Aires",
                Career = "Ingeniería en Sistemas", Year = 1,
                CertificateUrl = "/uploads/seed/p6_cert.pdf",
                Status = ValidationStatus.Approved,
                CreatedAt = DateTime.UtcNow.AddDays(-47), ReviewedAt = DateTime.UtcNow.AddDays(-45)
            },
            new ValidationRequest
            {
                Id = Guid.NewGuid(), UserId = pendingUserId, UserEmail = "pendiente@unlp.edu.ar",
                UniversityId = 3, Legajo = "UNLP9001", Faculty = "Informática",
                Career = "Licenciatura en Sistemas", Year = 2,
                CertificateUrl = "/uploads/seed/pending_cert.pdf",
                Status = ValidationStatus.Pending,
                CreatedAt = DateTime.UtcNow.AddDays(-2)
            }
        );

        // ── Torneos ───────────────────────────────────────────────────────────
        context.Tournaments.AddRange(
            new Tournament
            {
                Id = lolId,
                Name = "Liga Universitaria LoL 2026",
                Game = Game.LeagueOfLegends,
                Description = "El torneo interuniversitario de League of Legends más grande del país. Clasificatorio por rounds, finales en LAN.",
                IsInterUniversity = true,
                MaxTeams = 8,
                StartDate = new DateTime(2026, 7, 15, 0, 0, 0, DateTimeKind.Utc),
                EnrollmentDeadline = new DateTime(2026, 7, 1, 0, 0, 0, DateTimeKind.Utc),
                Status = TournamentStatus.Open,
                CreatedByUserId = organizerId,
                CreatedAt = DateTime.UtcNow.AddDays(-35)
            },
            new Tournament
            {
                Id = valId,
                Name = "UADE Valorant Cup",
                Game = Game.Valorant,
                Description = "Torneo exclusivo para estudiantes de UADE. Formato eliminación directa, 4 equipos, premios en efectivo.",
                IsInterUniversity = false,
                UniversityId = 10,
                MaxTeams = 4,
                StartDate = new DateTime(2026, 8, 10, 0, 0, 0, DateTimeKind.Utc),
                EnrollmentDeadline = new DateTime(2026, 7, 25, 0, 0, 0, DateTimeKind.Utc),
                Status = TournamentStatus.Open,
                CreatedByUserId = organizerId,
                CreatedAt = DateTime.UtcNow.AddDays(-20)
            },
            new Tournament
            {
                Id = cs2Id,
                Name = "Campus CS2 Championship",
                Game = Game.CS2,
                Description = "El torneo de Counter-Strike 2 interuniversitario. Formato suizo con top 4 en eliminación directa.",
                IsInterUniversity = true,
                MaxTeams = 6,
                StartDate = new DateTime(2026, 6, 1, 0, 0, 0, DateTimeKind.Utc),
                EnrollmentDeadline = new DateTime(2026, 5, 20, 0, 0, 0, DateTimeKind.Utc),
                Status = TournamentStatus.InProgress,
                CreatedByUserId = organizerId,
                CreatedAt = DateTime.UtcNow.AddDays(-60)
            }
        );

        // ── Solicitudes de organizador ────────────────────────────────────────
        context.OrganizerRequests.AddRange(
            new OrganizerRequest
            {
                Id = Guid.NewGuid(),
                UserId = organizerId, UserEmail = "organizador@campusclash.gg",
                TournamentName = "Liga Universitaria LoL 2026",
                StartDate = new DateTime(2026, 7, 15, 0, 0, 0, DateTimeKind.Utc),
                Game = Game.LeagueOfLegends,
                Description = "El torneo interuniversitario de League of Legends más grande del país.",
                IsInterUniversity = true, MaxTeams = 8,
                Status = OrganizerRequestStatus.Approved,
                CreatedAt = DateTime.UtcNow.AddDays(-37), ReviewedAt = DateTime.UtcNow.AddDays(-35)
            },
            // Solicitud pendiente de aprobación (para mostrar en el panel admin)
            new OrganizerRequest
            {
                Id = Guid.NewGuid(),
                UserId = player1Id, UserEmail = "jugador1@uba.ar",
                TournamentName = "UBA Open Tournament",
                StartDate = new DateTime(2026, 9, 5, 0, 0, 0, DateTimeKind.Utc),
                Game = Game.LeagueOfLegends,
                Description = "Torneo exclusivo para estudiantes de UBA. Queremos organizar el primer torneo oficial de la facultad.",
                IsInterUniversity = false, UniversityId = 1, MaxTeams = 4,
                Status = OrganizerRequestStatus.Pending,
                CreatedAt = DateTime.UtcNow.AddDays(-5)
            }
        );

        // ── Equipos ───────────────────────────────────────────────────────────
        context.Teams.AddRange(
            new Team { Id = teamUBALol,  Name = "UBA Nexus",    UniversityId = 1,  TournamentId = lolId, CreatedAt = DateTime.UtcNow.AddDays(-33) },
            new Team { Id = teamUADELol, Name = "UADE Dragons", UniversityId = 10, TournamentId = lolId, CreatedAt = DateTime.UtcNow.AddDays(-31) },
            new Team { Id = teamUADEVal, Name = "UADE Phantom", UniversityId = 10, TournamentId = valId, CreatedAt = DateTime.UtcNow.AddDays(-18) },
            new Team { Id = teamUTNCs2,  Name = "UTN Hackers",  UniversityId = 4,  TournamentId = cs2Id, CreatedAt = DateTime.UtcNow.AddDays(-58) }
        );

        // ── Inscripciones ─────────────────────────────────────────────────────
        context.Enrollments.AddRange(
            // LoL - UBA Nexus: player1 y player2
            new Enrollment { Id = Guid.NewGuid(), UserId = player1Id, TeamId = teamUBALol,  EnrolledAt = DateTime.UtcNow.AddDays(-33) },
            new Enrollment { Id = Guid.NewGuid(), UserId = player2Id, TeamId = teamUBALol,  EnrolledAt = DateTime.UtcNow.AddDays(-32) },
            // LoL - UADE Dragons: player3
            new Enrollment { Id = Guid.NewGuid(), UserId = player3Id, TeamId = teamUADELol, EnrolledAt = DateTime.UtcNow.AddDays(-31) },
            // Valorant - UADE Phantom: player4
            new Enrollment { Id = Guid.NewGuid(), UserId = player4Id, TeamId = teamUADEVal, EnrolledAt = DateTime.UtcNow.AddDays(-18) },
            // CS2 - UTN Hackers: player5 y player6
            new Enrollment { Id = Guid.NewGuid(), UserId = player5Id, TeamId = teamUTNCs2,  EnrolledAt = DateTime.UtcNow.AddDays(-58) },
            new Enrollment { Id = Guid.NewGuid(), UserId = player6Id, TeamId = teamUTNCs2,  EnrolledAt = DateTime.UtcNow.AddDays(-57) }
        );

        context.SaveChanges();
    }
}
