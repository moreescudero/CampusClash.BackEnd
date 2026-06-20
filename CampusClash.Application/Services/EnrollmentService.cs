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
    private readonly IBracketService _bracketService;

    public EnrollmentService(
        IUserRepository userRepository,
        IValidationRepository validationRepository,
        ITournamentRepository tournamentRepository,
        ITeamRepository teamRepository,
        IEnrollmentRepository enrollmentRepository,
        IBracketService bracketService)
    {
        _userRepository = userRepository;
        _validationRepository = validationRepository;
        _tournamentRepository = tournamentRepository;
        _teamRepository = teamRepository;
        _enrollmentRepository = enrollmentRepository;
        _bracketService = bracketService;
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

        // Si este jugador completó el equipo, verificar si el torneo entero está lleno
        if (currentPlayers >= TeamSize)
        {
            var refreshed = await _tournamentRepository.GetByIdAsync(tournamentId);
            if (refreshed is { Status: TournamentStatus.Open } &&
                refreshed.Teams.Count == refreshed.MaxTeams &&
                refreshed.Teams.All(t => t.Enrollments.Count >= TeamSize))
            {
                await _bracketService.GenerateAsync(tournamentId, refreshed.CreatedByUserId);
            }
        }

        return new EnrollResponseDto
        {
            TeamId = team.Id,
            TeamName = team.Name,
            UniversityName = validation.University!.Name,
            CurrentPlayers = currentPlayers,
            IsFull = currentPlayers >= TeamSize
        };
    }

    public async Task LeaveAsync(Guid userId, Guid tournamentId)
    {
        var enrollment = await _enrollmentRepository
            .GetByUserAndTournamentAsync(userId, tournamentId);

        if (enrollment is null)
            throw new Exception("No estás inscripto en este torneo.");

        var team = enrollment.Team;

        if (team is null)
            throw new Exception("Equipo no encontrado.");

        var memberCount = team.Enrollments.Count;

        _enrollmentRepository.Remove(enrollment);

        if (memberCount == 1)
        {
            _teamRepository.Remove(team);
        }

        await _enrollmentRepository.SaveChangesAsync();
    }
}
