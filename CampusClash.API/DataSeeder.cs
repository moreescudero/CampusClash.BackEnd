using CampusClash.Domain.Entities;
using CampusClash.Domain.Enums;
using CampusClash.Infrastructure.Data;

namespace CampusClash.API;

public static class DataSeeder
{
    public static void Seed(AppDbContext context)
    {
        var hash = BCrypt.Net.BCrypt.HashPassword("Campus123!");

        // ── IDs fijos ──────────────────────────────────────────────────────────
        var organizerId   = Guid.Parse("a1a1a1a1-0000-0000-0000-000000000001");
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
        var invId     = Guid.Parse("f4f4f4f4-0000-0000-0000-000000000004"); // torneo lleno listo para bracket

        var teamUBALol  = Guid.Parse("e1e1e1e1-0000-0000-0000-000000000001");
        var teamUADELol = Guid.Parse("e2e2e2e2-0000-0000-0000-000000000002");
        var teamUADEVal = Guid.Parse("e3e3e3e3-0000-0000-0000-000000000003");
        var teamUTNCs2  = Guid.Parse("e4e4e4e4-0000-0000-0000-000000000004");

        // equipos del torneo invitacional (4 equipos × 5 jugadores)
        var invTeam1 = Guid.Parse("1a1a1a1a-0000-0000-0000-000000000001"); // UBA Alpha
        var invTeam2 = Guid.Parse("2b2b2b2b-0000-0000-0000-000000000002"); // UADE Phoenix
        var invTeam3 = Guid.Parse("3c3c3c3c-0000-0000-0000-000000000003"); // UTN Storm
        var invTeam4 = Guid.Parse("4d4d4d4d-0000-0000-0000-000000000004"); // UNC Force

        // jugadores del invitacional (ip = inv player)
        var ip01 = Guid.Parse("00000000-1111-0000-0000-000000000001");
        var ip02 = Guid.Parse("00000000-1111-0000-0000-000000000002");
        var ip03 = Guid.Parse("00000000-1111-0000-0000-000000000003");
        var ip04 = Guid.Parse("00000000-1111-0000-0000-000000000004");
        var ip05 = Guid.Parse("00000000-1111-0000-0000-000000000005");
        var ip06 = Guid.Parse("00000000-1111-0000-0000-000000000006");
        var ip07 = Guid.Parse("00000000-1111-0000-0000-000000000007");
        var ip08 = Guid.Parse("00000000-1111-0000-0000-000000000008");
        var ip09 = Guid.Parse("00000000-1111-0000-0000-000000000009");
        var ip10 = Guid.Parse("00000000-1111-0000-0000-000000000010");
        var ip11 = Guid.Parse("00000000-1111-0000-0000-000000000011");
        var ip12 = Guid.Parse("00000000-1111-0000-0000-000000000012");
        var ip13 = Guid.Parse("00000000-1111-0000-0000-000000000013");
        var ip14 = Guid.Parse("00000000-1111-0000-0000-000000000014");
        var ip15 = Guid.Parse("00000000-1111-0000-0000-000000000015");
        var ip16 = Guid.Parse("00000000-1111-0000-0000-000000000016");
        var ip17 = Guid.Parse("00000000-1111-0000-0000-000000000017");
        var ip18 = Guid.Parse("00000000-1111-0000-0000-000000000018");
        var ip19 = Guid.Parse("00000000-1111-0000-0000-000000000019");
        var ip20 = Guid.Parse("00000000-1111-0000-0000-000000000020");

        // ── Usuarios (inserta solo los que no existen) ────────────────────────
        var allSeedUserIds = new[]
        {
            organizerId, player1Id, player2Id, player3Id, player4Id, player5Id, player6Id, pendingUserId,
            ip01, ip02, ip03, ip04, ip05, ip06, ip07, ip08, ip09, ip10,
            ip11, ip12, ip13, ip14, ip15, ip16, ip17, ip18, ip19, ip20,
        };
        var existingUserIds = context.Users
            .Where(u => allSeedUserIds.Contains(u.Id))
            .Select(u => u.Id)
            .ToHashSet();

        var allUsers = new List<User>
        {
            new() { Id = organizerId,   Email = "organizador@campusclash.gg", Username = "OrganizadorCC",  PasswordHash = hash, IsEmailConfirmed = true, IsOrganizer = true,  IsRiotLinked = true, RiotGameName = "CampusOrg", RiotTagLine = "ARG", CreatedAt = DateTime.UtcNow.AddDays(-40) },
            new() { Id = player1Id,     Email = "jugador1@uba.ar",            Username = "UBA_Player1",    PasswordHash = hash, IsEmailConfirmed = true, IsRiotLinked = true,  RiotGameName = "UBAOne",    RiotTagLine = "LOL", CreatedAt = DateTime.UtcNow.AddDays(-25) },
            new() { Id = player2Id,     Email = "jugador2@uba.ar",            Username = "UBA_Player2",    PasswordHash = hash, IsEmailConfirmed = true, CreatedAt = DateTime.UtcNow.AddDays(-22) },
            new() { Id = player3Id,     Email = "jugador1@uade.edu.ar",       Username = "UADE_Player1",   PasswordHash = hash, IsEmailConfirmed = true, IsRiotLinked = true,  RiotGameName = "UADEOne",   RiotTagLine = "CC",  CreatedAt = DateTime.UtcNow.AddDays(-30) },
            new() { Id = player4Id,     Email = "jugador2@uade.edu.ar",       Username = "UADE_Player2",   PasswordHash = hash, IsEmailConfirmed = true, CreatedAt = DateTime.UtcNow.AddDays(-18) },
            new() { Id = player5Id,     Email = "jugador1@utn.edu.ar",        Username = "UTN_Player1",    PasswordHash = hash, IsEmailConfirmed = true, CreatedAt = DateTime.UtcNow.AddDays(-50) },
            new() { Id = player6Id,     Email = "jugador2@utn.edu.ar",        Username = "UTN_Player2",    PasswordHash = hash, IsEmailConfirmed = true, CreatedAt = DateTime.UtcNow.AddDays(-48) },
            new() { Id = pendingUserId, Email = "pendiente@unlp.edu.ar",      Username = "UNLP_Nuevo",     PasswordHash = hash, IsEmailConfirmed = true, CreatedAt = DateTime.UtcNow.AddDays(-3)  },
            // jugadores del invitacional - UBA Alpha
            new() { Id = ip01, Email = "uba.alpha1@uba.ar",   Username = "UBA_Alpha1",   PasswordHash = hash, IsEmailConfirmed = true, CreatedAt = DateTime.UtcNow.AddDays(-15) },
            new() { Id = ip02, Email = "uba.alpha2@uba.ar",   Username = "UBA_Alpha2",   PasswordHash = hash, IsEmailConfirmed = true, CreatedAt = DateTime.UtcNow.AddDays(-15) },
            new() { Id = ip03, Email = "uba.alpha3@uba.ar",   Username = "UBA_Alpha3",   PasswordHash = hash, IsEmailConfirmed = true, CreatedAt = DateTime.UtcNow.AddDays(-15) },
            new() { Id = ip04, Email = "uba.alpha4@uba.ar",   Username = "UBA_Alpha4",   PasswordHash = hash, IsEmailConfirmed = true, CreatedAt = DateTime.UtcNow.AddDays(-15) },
            new() { Id = ip05, Email = "uba.alpha5@uba.ar",   Username = "UBA_Alpha5",   PasswordHash = hash, IsEmailConfirmed = true, CreatedAt = DateTime.UtcNow.AddDays(-15) },
            // UADE Phoenix
            new() { Id = ip06, Email = "uade.phx1@uade.edu.ar", Username = "UADE_Phoenix1", PasswordHash = hash, IsEmailConfirmed = true, CreatedAt = DateTime.UtcNow.AddDays(-14) },
            new() { Id = ip07, Email = "uade.phx2@uade.edu.ar", Username = "UADE_Phoenix2", PasswordHash = hash, IsEmailConfirmed = true, CreatedAt = DateTime.UtcNow.AddDays(-14) },
            new() { Id = ip08, Email = "uade.phx3@uade.edu.ar", Username = "UADE_Phoenix3", PasswordHash = hash, IsEmailConfirmed = true, CreatedAt = DateTime.UtcNow.AddDays(-14) },
            new() { Id = ip09, Email = "uade.phx4@uade.edu.ar", Username = "UADE_Phoenix4", PasswordHash = hash, IsEmailConfirmed = true, CreatedAt = DateTime.UtcNow.AddDays(-14) },
            new() { Id = ip10, Email = "uade.phx5@uade.edu.ar", Username = "UADE_Phoenix5", PasswordHash = hash, IsEmailConfirmed = true, CreatedAt = DateTime.UtcNow.AddDays(-14) },
            // UTN Storm
            new() { Id = ip11, Email = "utn.storm1@utn.edu.ar", Username = "UTN_Storm1", PasswordHash = hash, IsEmailConfirmed = true, CreatedAt = DateTime.UtcNow.AddDays(-13) },
            new() { Id = ip12, Email = "utn.storm2@utn.edu.ar", Username = "UTN_Storm2", PasswordHash = hash, IsEmailConfirmed = true, CreatedAt = DateTime.UtcNow.AddDays(-13) },
            new() { Id = ip13, Email = "utn.storm3@utn.edu.ar", Username = "UTN_Storm3", PasswordHash = hash, IsEmailConfirmed = true, CreatedAt = DateTime.UtcNow.AddDays(-13) },
            new() { Id = ip14, Email = "utn.storm4@utn.edu.ar", Username = "UTN_Storm4", PasswordHash = hash, IsEmailConfirmed = true, CreatedAt = DateTime.UtcNow.AddDays(-13) },
            new() { Id = ip15, Email = "utn.storm5@utn.edu.ar", Username = "UTN_Storm5", PasswordHash = hash, IsEmailConfirmed = true, CreatedAt = DateTime.UtcNow.AddDays(-13) },
            // UNC Force
            new() { Id = ip16, Email = "unc.force1@unc.edu.ar", Username = "UNC_Force1", PasswordHash = hash, IsEmailConfirmed = true, CreatedAt = DateTime.UtcNow.AddDays(-12) },
            new() { Id = ip17, Email = "unc.force2@unc.edu.ar", Username = "UNC_Force2", PasswordHash = hash, IsEmailConfirmed = true, CreatedAt = DateTime.UtcNow.AddDays(-12) },
            new() { Id = ip18, Email = "unc.force3@unc.edu.ar", Username = "UNC_Force3", PasswordHash = hash, IsEmailConfirmed = true, CreatedAt = DateTime.UtcNow.AddDays(-12) },
            new() { Id = ip19, Email = "unc.force4@unc.edu.ar", Username = "UNC_Force4", PasswordHash = hash, IsEmailConfirmed = true, CreatedAt = DateTime.UtcNow.AddDays(-12) },
            new() { Id = ip20, Email = "unc.force5@unc.edu.ar", Username = "UNC_Force5", PasswordHash = hash, IsEmailConfirmed = true, CreatedAt = DateTime.UtcNow.AddDays(-12) },
        };

        var usersToAdd = allUsers.Where(u => !existingUserIds.Contains(u.Id)).ToList();
        if (usersToAdd.Count > 0)
        {
            context.Users.AddRange(usersToAdd);
            context.SaveChanges();
        }

        // ── Validaciones (inserta solo las que no existen) ────────────────────
        var existingValidationUserIds = context.ValidationRequests
            .Select(v => v.UserId)
            .ToHashSet();

        var allValidations = new List<ValidationRequest>
        {
            new() { Id = Guid.NewGuid(), UserId = organizerId,   UserEmail = "organizador@campusclash.gg", UniversityId = 10, Legajo = "1091234",  Faculty = "Ingeniería",           Career = "Ingeniería en Sistemas",      Year = 3, CertificateUrl = "/uploads/seed/org_cert.pdf",     Status = ValidationStatus.Approved, CreatedAt = DateTime.UtcNow.AddDays(-38), ReviewedAt = DateTime.UtcNow.AddDays(-36) },
            new() { Id = Guid.NewGuid(), UserId = player1Id,     UserEmail = "jugador1@uba.ar",            UniversityId = 1,  Legajo = "LU001001", Faculty = "Ciencias Exactas",     Career = "Ciencias de la Computación",  Year = 2, CertificateUrl = "/uploads/seed/p1_cert.pdf",      Status = ValidationStatus.Approved, CreatedAt = DateTime.UtcNow.AddDays(-24), ReviewedAt = DateTime.UtcNow.AddDays(-22) },
            new() { Id = Guid.NewGuid(), UserId = player2Id,     UserEmail = "jugador2@uba.ar",            UniversityId = 1,  Legajo = "LU001002", Faculty = "Ciencias Exactas",     Career = "Ciencias de la Computación",  Year = 1, CertificateUrl = "/uploads/seed/p2_cert.pdf",      Status = ValidationStatus.Approved, CreatedAt = DateTime.UtcNow.AddDays(-21), ReviewedAt = DateTime.UtcNow.AddDays(-20) },
            new() { Id = Guid.NewGuid(), UserId = player3Id,     UserEmail = "jugador1@uade.edu.ar",       UniversityId = 10, Legajo = "1090001",  Faculty = "Ingeniería",           Career = "Ingeniería en Sistemas",      Year = 4, CertificateUrl = "/uploads/seed/p3_cert.pdf",      Status = ValidationStatus.Approved, CreatedAt = DateTime.UtcNow.AddDays(-29), ReviewedAt = DateTime.UtcNow.AddDays(-27) },
            new() { Id = Guid.NewGuid(), UserId = player4Id,     UserEmail = "jugador2@uade.edu.ar",       UniversityId = 10, Legajo = "1090002",  Faculty = "Ingeniería",           Career = "Ingeniería en Informática",   Year = 2, CertificateUrl = "/uploads/seed/p4_cert.pdf",      Status = ValidationStatus.Approved, CreatedAt = DateTime.UtcNow.AddDays(-17), ReviewedAt = DateTime.UtcNow.AddDays(-16) },
            new() { Id = Guid.NewGuid(), UserId = player5Id,     UserEmail = "jugador1@utn.edu.ar",        UniversityId = 4,  Legajo = "UTN00101", Faculty = "Regional Buenos Aires", Career = "Ingeniería en Sistemas",     Year = 3, CertificateUrl = "/uploads/seed/p5_cert.pdf",      Status = ValidationStatus.Approved, CreatedAt = DateTime.UtcNow.AddDays(-49), ReviewedAt = DateTime.UtcNow.AddDays(-47) },
            new() { Id = Guid.NewGuid(), UserId = player6Id,     UserEmail = "jugador2@utn.edu.ar",        UniversityId = 4,  Legajo = "UTN00102", Faculty = "Regional Buenos Aires", Career = "Ingeniería en Sistemas",     Year = 1, CertificateUrl = "/uploads/seed/p6_cert.pdf",      Status = ValidationStatus.Approved, CreatedAt = DateTime.UtcNow.AddDays(-47), ReviewedAt = DateTime.UtcNow.AddDays(-45) },
            new() { Id = Guid.NewGuid(), UserId = pendingUserId, UserEmail = "pendiente@unlp.edu.ar",      UniversityId = 3,  Legajo = "UNLP9001", Faculty = "Informática",           Career = "Licenciatura en Sistemas",    Year = 2, CertificateUrl = "/uploads/seed/pending_cert.pdf", Status = ValidationStatus.Pending,  CreatedAt = DateTime.UtcNow.AddDays(-2) },
            // invitacional - UBA Alpha
            new() { Id = Guid.NewGuid(), UserId = ip01, UserEmail = "uba.alpha1@uba.ar", UniversityId = 1, Legajo = "LU010001", Faculty = "Ciencias Exactas", Career = "Computación", Year = 2, CertificateUrl = "/uploads/seed/ip01.pdf", Status = ValidationStatus.Approved, CreatedAt = DateTime.UtcNow.AddDays(-15), ReviewedAt = DateTime.UtcNow.AddDays(-14) },
            new() { Id = Guid.NewGuid(), UserId = ip02, UserEmail = "uba.alpha2@uba.ar", UniversityId = 1, Legajo = "LU010002", Faculty = "Ciencias Exactas", Career = "Computación", Year = 3, CertificateUrl = "/uploads/seed/ip02.pdf", Status = ValidationStatus.Approved, CreatedAt = DateTime.UtcNow.AddDays(-15), ReviewedAt = DateTime.UtcNow.AddDays(-14) },
            new() { Id = Guid.NewGuid(), UserId = ip03, UserEmail = "uba.alpha3@uba.ar", UniversityId = 1, Legajo = "LU010003", Faculty = "Ciencias Exactas", Career = "Computación", Year = 1, CertificateUrl = "/uploads/seed/ip03.pdf", Status = ValidationStatus.Approved, CreatedAt = DateTime.UtcNow.AddDays(-15), ReviewedAt = DateTime.UtcNow.AddDays(-14) },
            new() { Id = Guid.NewGuid(), UserId = ip04, UserEmail = "uba.alpha4@uba.ar", UniversityId = 1, Legajo = "LU010004", Faculty = "Ingeniería",       Career = "Informática", Year = 4, CertificateUrl = "/uploads/seed/ip04.pdf", Status = ValidationStatus.Approved, CreatedAt = DateTime.UtcNow.AddDays(-15), ReviewedAt = DateTime.UtcNow.AddDays(-14) },
            new() { Id = Guid.NewGuid(), UserId = ip05, UserEmail = "uba.alpha5@uba.ar", UniversityId = 1, Legajo = "LU010005", Faculty = "Ingeniería",       Career = "Informática", Year = 2, CertificateUrl = "/uploads/seed/ip05.pdf", Status = ValidationStatus.Approved, CreatedAt = DateTime.UtcNow.AddDays(-15), ReviewedAt = DateTime.UtcNow.AddDays(-14) },
            // invitacional - UADE Phoenix
            new() { Id = Guid.NewGuid(), UserId = ip06, UserEmail = "uade.phx1@uade.edu.ar", UniversityId = 10, Legajo = "1090101", Faculty = "Ingeniería", Career = "Sistemas",    Year = 3, CertificateUrl = "/uploads/seed/ip06.pdf", Status = ValidationStatus.Approved, CreatedAt = DateTime.UtcNow.AddDays(-14), ReviewedAt = DateTime.UtcNow.AddDays(-13) },
            new() { Id = Guid.NewGuid(), UserId = ip07, UserEmail = "uade.phx2@uade.edu.ar", UniversityId = 10, Legajo = "1090102", Faculty = "Ingeniería", Career = "Sistemas",    Year = 2, CertificateUrl = "/uploads/seed/ip07.pdf", Status = ValidationStatus.Approved, CreatedAt = DateTime.UtcNow.AddDays(-14), ReviewedAt = DateTime.UtcNow.AddDays(-13) },
            new() { Id = Guid.NewGuid(), UserId = ip08, UserEmail = "uade.phx3@uade.edu.ar", UniversityId = 10, Legajo = "1090103", Faculty = "Ingeniería", Career = "Informática", Year = 1, CertificateUrl = "/uploads/seed/ip08.pdf", Status = ValidationStatus.Approved, CreatedAt = DateTime.UtcNow.AddDays(-14), ReviewedAt = DateTime.UtcNow.AddDays(-13) },
            new() { Id = Guid.NewGuid(), UserId = ip09, UserEmail = "uade.phx4@uade.edu.ar", UniversityId = 10, Legajo = "1090104", Faculty = "Ingeniería", Career = "Informática", Year = 4, CertificateUrl = "/uploads/seed/ip09.pdf", Status = ValidationStatus.Approved, CreatedAt = DateTime.UtcNow.AddDays(-14), ReviewedAt = DateTime.UtcNow.AddDays(-13) },
            new() { Id = Guid.NewGuid(), UserId = ip10, UserEmail = "uade.phx5@uade.edu.ar", UniversityId = 10, Legajo = "1090105", Faculty = "Ingeniería", Career = "Sistemas",    Year = 3, CertificateUrl = "/uploads/seed/ip10.pdf", Status = ValidationStatus.Approved, CreatedAt = DateTime.UtcNow.AddDays(-14), ReviewedAt = DateTime.UtcNow.AddDays(-13) },
            // invitacional - UTN Storm
            new() { Id = Guid.NewGuid(), UserId = ip11, UserEmail = "utn.storm1@utn.edu.ar", UniversityId = 4, Legajo = "UTN10101", Faculty = "Regional BA", Career = "Sistemas", Year = 2, CertificateUrl = "/uploads/seed/ip11.pdf", Status = ValidationStatus.Approved, CreatedAt = DateTime.UtcNow.AddDays(-13), ReviewedAt = DateTime.UtcNow.AddDays(-12) },
            new() { Id = Guid.NewGuid(), UserId = ip12, UserEmail = "utn.storm2@utn.edu.ar", UniversityId = 4, Legajo = "UTN10102", Faculty = "Regional BA", Career = "Sistemas", Year = 3, CertificateUrl = "/uploads/seed/ip12.pdf", Status = ValidationStatus.Approved, CreatedAt = DateTime.UtcNow.AddDays(-13), ReviewedAt = DateTime.UtcNow.AddDays(-12) },
            new() { Id = Guid.NewGuid(), UserId = ip13, UserEmail = "utn.storm3@utn.edu.ar", UniversityId = 4, Legajo = "UTN10103", Faculty = "Regional BA", Career = "Sistemas", Year = 1, CertificateUrl = "/uploads/seed/ip13.pdf", Status = ValidationStatus.Approved, CreatedAt = DateTime.UtcNow.AddDays(-13), ReviewedAt = DateTime.UtcNow.AddDays(-12) },
            new() { Id = Guid.NewGuid(), UserId = ip14, UserEmail = "utn.storm4@utn.edu.ar", UniversityId = 4, Legajo = "UTN10104", Faculty = "Regional BA", Career = "Sistemas", Year = 4, CertificateUrl = "/uploads/seed/ip14.pdf", Status = ValidationStatus.Approved, CreatedAt = DateTime.UtcNow.AddDays(-13), ReviewedAt = DateTime.UtcNow.AddDays(-12) },
            new() { Id = Guid.NewGuid(), UserId = ip15, UserEmail = "utn.storm5@utn.edu.ar", UniversityId = 4, Legajo = "UTN10105", Faculty = "Regional BA", Career = "Sistemas", Year = 2, CertificateUrl = "/uploads/seed/ip15.pdf", Status = ValidationStatus.Approved, CreatedAt = DateTime.UtcNow.AddDays(-13), ReviewedAt = DateTime.UtcNow.AddDays(-12) },
            // invitacional - UNC Force
            new() { Id = Guid.NewGuid(), UserId = ip16, UserEmail = "unc.force1@unc.edu.ar", UniversityId = 2, Legajo = "UNC10101", Faculty = "FaMAF", Career = "Computación", Year = 3, CertificateUrl = "/uploads/seed/ip16.pdf", Status = ValidationStatus.Approved, CreatedAt = DateTime.UtcNow.AddDays(-12), ReviewedAt = DateTime.UtcNow.AddDays(-11) },
            new() { Id = Guid.NewGuid(), UserId = ip17, UserEmail = "unc.force2@unc.edu.ar", UniversityId = 2, Legajo = "UNC10102", Faculty = "FaMAF", Career = "Computación", Year = 2, CertificateUrl = "/uploads/seed/ip17.pdf", Status = ValidationStatus.Approved, CreatedAt = DateTime.UtcNow.AddDays(-12), ReviewedAt = DateTime.UtcNow.AddDays(-11) },
            new() { Id = Guid.NewGuid(), UserId = ip18, UserEmail = "unc.force3@unc.edu.ar", UniversityId = 2, Legajo = "UNC10103", Faculty = "FaMAF", Career = "Computación", Year = 4, CertificateUrl = "/uploads/seed/ip18.pdf", Status = ValidationStatus.Approved, CreatedAt = DateTime.UtcNow.AddDays(-12), ReviewedAt = DateTime.UtcNow.AddDays(-11) },
            new() { Id = Guid.NewGuid(), UserId = ip19, UserEmail = "unc.force4@unc.edu.ar", UniversityId = 2, Legajo = "UNC10104", Faculty = "FaMAF", Career = "Computación", Year = 1, CertificateUrl = "/uploads/seed/ip19.pdf", Status = ValidationStatus.Approved, CreatedAt = DateTime.UtcNow.AddDays(-12), ReviewedAt = DateTime.UtcNow.AddDays(-11) },
            new() { Id = Guid.NewGuid(), UserId = ip20, UserEmail = "unc.force5@unc.edu.ar", UniversityId = 2, Legajo = "UNC10105", Faculty = "FaMAF", Career = "Computación", Year = 3, CertificateUrl = "/uploads/seed/ip20.pdf", Status = ValidationStatus.Approved, CreatedAt = DateTime.UtcNow.AddDays(-12), ReviewedAt = DateTime.UtcNow.AddDays(-11) },
        };

        var validationsToAdd = allValidations.Where(v => !existingValidationUserIds.Contains(v.UserId)).ToList();
        if (validationsToAdd.Count > 0)
        {
            context.ValidationRequests.AddRange(validationsToAdd);
            context.SaveChanges();
        }

        // ── Torneos (inserta solo los que no existen) ─────────────────────────
        var existingTournamentIds = context.Tournaments
            .Where(t => new[] { lolId, valId, cs2Id, invId }.Contains(t.Id))
            .Select(t => t.Id)
            .ToHashSet();

        var allTournaments = new List<Tournament>
        {
            new()
            {
                Id = lolId, Name = "Liga Universitaria LoL 2026", Game = Game.LeagueOfLegends,
                Description = "El torneo interuniversitario de League of Legends más grande del país. Clasificatorio por rounds, finales en LAN.",
                IsInterUniversity = true, MaxTeams = 8,
                StartDate = new DateTime(2026, 7, 15, 0, 0, 0, DateTimeKind.Utc),
                EnrollmentDeadline = new DateTime(2026, 7, 1, 0, 0, 0, DateTimeKind.Utc),
                Status = TournamentStatus.Open, CreatedByUserId = organizerId, CreatedAt = DateTime.UtcNow.AddDays(-35)
            },
            new()
            {
                Id = valId, Name = "UADE Valorant Cup", Game = Game.Valorant,
                Description = "Torneo exclusivo para estudiantes de UADE. Formato eliminación directa, 4 equipos, premios en efectivo.",
                IsInterUniversity = false, UniversityId = 10, MaxTeams = 4,
                StartDate = new DateTime(2026, 8, 10, 0, 0, 0, DateTimeKind.Utc),
                EnrollmentDeadline = new DateTime(2026, 7, 25, 0, 0, 0, DateTimeKind.Utc),
                Status = TournamentStatus.Open, CreatedByUserId = organizerId, CreatedAt = DateTime.UtcNow.AddDays(-20)
            },
            new()
            {
                Id = cs2Id, Name = "Campus CS2 Championship", Game = Game.CS2,
                Description = "El torneo de Counter-Strike 2 interuniversitario. Formato suizo con top 4 en eliminación directa.",
                IsInterUniversity = true, MaxTeams = 6,
                StartDate = new DateTime(2026, 6, 1, 0, 0, 0, DateTimeKind.Utc),
                EnrollmentDeadline = new DateTime(2026, 5, 20, 0, 0, 0, DateTimeKind.Utc),
                Status = TournamentStatus.InProgress, CreatedByUserId = organizerId, CreatedAt = DateTime.UtcNow.AddDays(-60)
            },
            new()
            {
                Id = invId, Name = "Campus LoL Invitational 2026", Game = Game.LeagueOfLegends,
                Description = "Torneo invitacional interuniversitario con los 4 mejores equipos del país. 4 equipos completos, bracket listo para generar.",
                IsInterUniversity = true, MaxTeams = 4,
                StartDate = new DateTime(2026, 7, 20, 0, 0, 0, DateTimeKind.Utc),
                EnrollmentDeadline = new DateTime(2026, 7, 10, 0, 0, 0, DateTimeKind.Utc),
                Status = TournamentStatus.Open, CreatedByUserId = organizerId, CreatedAt = DateTime.UtcNow.AddDays(-10)
            },
        };

        var tournamentsToAdd = allTournaments.Where(t => !existingTournamentIds.Contains(t.Id)).ToList();
        if (tournamentsToAdd.Count > 0)
        {
            context.Tournaments.AddRange(tournamentsToAdd);
            context.SaveChanges();
        }

        // ── Solicitudes de organizador (inserta solo si no hay ninguna del organizer) ──
        if (!context.OrganizerRequests.Any(o => o.UserId == organizerId))
        {
            context.OrganizerRequests.AddRange(
                new OrganizerRequest
                {
                    Id = Guid.NewGuid(), UserId = organizerId, UserEmail = "organizador@campusclash.gg",
                    TournamentName = "Liga Universitaria LoL 2026",
                    StartDate = new DateTime(2026, 7, 15, 0, 0, 0, DateTimeKind.Utc),
                    Game = Game.LeagueOfLegends,
                    Description = "El torneo interuniversitario de League of Legends más grande del país.",
                    IsInterUniversity = true, MaxTeams = 8,
                    Status = OrganizerRequestStatus.Approved,
                    CreatedAt = DateTime.UtcNow.AddDays(-37), ReviewedAt = DateTime.UtcNow.AddDays(-35)
                },
                new OrganizerRequest
                {
                    Id = Guid.NewGuid(), UserId = player1Id, UserEmail = "jugador1@uba.ar",
                    TournamentName = "UBA Open Tournament",
                    StartDate = new DateTime(2026, 9, 5, 0, 0, 0, DateTimeKind.Utc),
                    Game = Game.LeagueOfLegends,
                    Description = "Torneo exclusivo para estudiantes de UBA. Queremos organizar el primer torneo oficial de la facultad.",
                    IsInterUniversity = false, UniversityId = 1, MaxTeams = 4,
                    Status = OrganizerRequestStatus.Pending,
                    CreatedAt = DateTime.UtcNow.AddDays(-5)
                }
            );
            context.SaveChanges();
        }

        // ── Equipos (inserta solo los que no existen) ─────────────────────────
        var existingTeamIds = context.Teams
            .Where(t => new[] { teamUBALol, teamUADELol, teamUADEVal, teamUTNCs2, invTeam1, invTeam2, invTeam3, invTeam4 }.Contains(t.Id))
            .Select(t => t.Id)
            .ToHashSet();

        var allTeams = new List<Team>
        {
            new() { Id = teamUBALol,  Name = "UBA Nexus",    UniversityId = 1,  TournamentId = lolId, CreatedAt = DateTime.UtcNow.AddDays(-33) },
            new() { Id = teamUADELol, Name = "UADE Dragons", UniversityId = 10, TournamentId = lolId, CreatedAt = DateTime.UtcNow.AddDays(-31) },
            new() { Id = teamUADEVal, Name = "UADE Phantom", UniversityId = 10, TournamentId = valId, CreatedAt = DateTime.UtcNow.AddDays(-18) },
            new() { Id = teamUTNCs2,  Name = "UTN Hackers",  UniversityId = 4,  TournamentId = cs2Id, CreatedAt = DateTime.UtcNow.AddDays(-58) },
            // invitacional (todos llenos con 5 jugadores)
            new() { Id = invTeam1, Name = "UBA Alpha",    UniversityId = 1,  TournamentId = invId, CreatedAt = DateTime.UtcNow.AddDays(-9) },
            new() { Id = invTeam2, Name = "UADE Phoenix", UniversityId = 10, TournamentId = invId, CreatedAt = DateTime.UtcNow.AddDays(-9) },
            new() { Id = invTeam3, Name = "UTN Storm",    UniversityId = 4,  TournamentId = invId, CreatedAt = DateTime.UtcNow.AddDays(-8) },
            new() { Id = invTeam4, Name = "UNC Force",    UniversityId = 2,  TournamentId = invId, CreatedAt = DateTime.UtcNow.AddDays(-8) },
        };

        var teamsToAdd = allTeams.Where(t => !existingTeamIds.Contains(t.Id)).ToList();
        if (teamsToAdd.Count > 0)
        {
            context.Teams.AddRange(teamsToAdd);
            context.SaveChanges();
        }

        // ── Inscripciones (inserta solo las que no existen) ───────────────────
        var existingEnrollmentKeys = context.Enrollments
            .Select(e => new { e.UserId, e.TeamId })
            .ToHashSet();

        var allEnrollments = new List<(Guid userId, Guid teamId, int daysAgo)>
        {
            (player1Id, teamUBALol,  33),
            (player2Id, teamUBALol,  32),
            (player3Id, teamUADELol, 31),
            (player4Id, teamUADEVal, 18),
            (player5Id, teamUTNCs2,  58),
            (player6Id, teamUTNCs2,  57),
            // invitacional - UBA Alpha
            (ip01, invTeam1, 9), (ip02, invTeam1, 9), (ip03, invTeam1, 8), (ip04, invTeam1, 8), (ip05, invTeam1, 7),
            // invitacional - UADE Phoenix
            (ip06, invTeam2, 9), (ip07, invTeam2, 9), (ip08, invTeam2, 8), (ip09, invTeam2, 8), (ip10, invTeam2, 7),
            // invitacional - UTN Storm
            (ip11, invTeam3, 8), (ip12, invTeam3, 8), (ip13, invTeam3, 7), (ip14, invTeam3, 7), (ip15, invTeam3, 6),
            // invitacional - UNC Force
            (ip16, invTeam4, 8), (ip17, invTeam4, 8), (ip18, invTeam4, 7), (ip19, invTeam4, 7), (ip20, invTeam4, 6),
        };

        var enrollmentsToAdd = allEnrollments
            .Where(e => !existingEnrollmentKeys.Any(k => k.UserId == e.userId && k.TeamId == e.teamId))
            .Select(e => new Enrollment { Id = Guid.NewGuid(), UserId = e.userId, TeamId = e.teamId, EnrolledAt = DateTime.UtcNow.AddDays(-e.daysAgo) })
            .ToList();

        if (enrollmentsToAdd.Count > 0)
        {
            context.Enrollments.AddRange(enrollmentsToAdd);
            context.SaveChanges();
        }
    }
}
