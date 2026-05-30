using CampusClash.Application.DTOs.Tournament;
using CampusClash.Application.Interfaces;
using CampusClash.Domain.Entities;
using CampusClash.Domain.Enums;

namespace CampusClash.Application.Services;

public class EnrollmentService : IEnrollmentService
{
    private const int TeamSize = 5;

    private readonly IUserRepository _userRepository;
    private readonly IValidationRepository _validationRepository;
    private readonly ITournamentRepository _tournamentRepository;
    private readonly ITeamRepository _teamRepository;
    private readonly IEnrollmentRepository _enrollmentRepository;

    public EnrollmentService(
        IUserRepository userRepository,
        IValidationRepository validationRepository,
        ITournamentRepository tournamentRepository,
        ITeamRepository teamRepository,
        IEnrollmentRepository enrollmentRepository)
    {
        _userRepository = userRepository;
        _validationRepository = validationRepository;
        _tournamentRepository = tournamentRepository;
        _teamRepository = teamRepository;
        _enrollmentRepository = enrollmentRepository;
    }

    public async Task<EnrollResponseDto> EnrollAsync(Guid userId, Guid tournamentId)
    {
        var user = await _userRepository.GetByIdAsync(userId);
        if (user is null)
            throw new Exception("Usuario no encontrado.");

        var validation = await _validationRepository.GetByUserIdAsync(userId);
        if (validation is null || validation.Status != ValidationStatus.Approved)
            throw new Exception("Debés tener tu constancia de alumno validada para inscribirte en un torneo.");

        if (await _enrollmentRepository.IsUserInActiveTournamentAsync(userId))
            throw new Exception("Ya estás inscripto en un torneo activo.");

        var tournament = await _tournamentRepository.GetByIdAsync(tournamentId);
        if (tournament is null)
            throw new Exception("Torneo no encontrado.");

        if (tournament.Status != TournamentStatus.Open)
            throw new Exception("El torneo no está abierto para inscripciones.");

        var universityId = validation.UniversityId;

        var team = await _teamRepository.GetNonFullByTournamentAndUniversityAsync(tournamentId, universityId);

        if (team is null)
        {
            var totalTeams = await _teamRepository.GetCountByTournamentAsync(tournamentId);
            if (totalTeams >= tournament.MaxTeams)
                throw new Exception("El torneo ya alcanzó el máximo de equipos permitidos.");

            var teamNumber = await _teamRepository.GetCountByTournamentAndUniversityAsync(tournamentId, universityId) + 1;

            team = new Team
            {
                Id = Guid.NewGuid(),
                Name = $"{validation.University!.ShortName} #{teamNumber}",
                UniversityId = universityId,
                TournamentId = tournamentId,
                CreatedAt = DateTime.UtcNow
            };
            await _teamRepository.AddAsync(team);
        }

        var enrollment = new Enrollment
        {
            Id = Guid.NewGuid(),
            UserId = userId,
            TeamId = team.Id,
            EnrolledAt = DateTime.UtcNow
        };
        await _enrollmentRepository.AddAsync(enrollment);
        await _enrollmentRepository.SaveChangesAsync();

        var currentPlayers = team.Enrollments.Count + 1;

        return new EnrollResponseDto
        {
            TeamId = team.Id,
            TeamName = team.Name,
            UniversityName = validation.University!.Name,
            CurrentPlayers = currentPlayers,
            IsFull = currentPlayers >= TeamSize
        };
    }
}
