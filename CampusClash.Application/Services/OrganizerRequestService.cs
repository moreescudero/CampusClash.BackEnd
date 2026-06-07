using CampusClash.Application.DTOs.OrganizerRequest;
using CampusClash.Application.Interfaces;
using CampusClash.Domain.Entities;
using CampusClash.Domain.Enums;

namespace CampusClash.Application.Services;

public class OrganizerRequestService : IOrganizerRequestService
{
    private readonly IOrganizerRequestRepository _orgRequestRepository;
    private readonly IValidationRepository _validationRepository;
    private readonly IUserRepository _userRepository;
    private readonly ITournamentRepository _tournamentRepository;

    public OrganizerRequestService(
        IOrganizerRequestRepository orgRequestRepository,
        IValidationRepository validationRepository,
        IUserRepository userRepository,
        ITournamentRepository tournamentRepository)
    {
        _orgRequestRepository = orgRequestRepository;
        _validationRepository = validationRepository;
        _userRepository = userRepository;
        _tournamentRepository = tournamentRepository;
    }

    public async Task<OrganizerRequestResponseDto> SubmitAsync(Guid userId, OrganizerRequestDto dto)
    {
        dto.StartDate = DateTime.SpecifyKind(dto.StartDate, DateTimeKind.Utc);

        var user = await _userRepository.GetByIdAsync(userId);
        if (user is null)
            throw new Exception("Usuario no encontrado.");

        var validation = await _validationRepository.GetByUserIdAsync(userId);
        if (validation is null || validation.Status != ValidationStatus.Approved)
            throw new Exception("Debés tener tu constancia de alumno validada para solicitar ser organizador.");

        var existing = await _orgRequestRepository.GetByUserIdAsync(userId);
        if (existing is not null && existing.Status == OrganizerRequestStatus.Pending)
            throw new Exception("Ya tenés una solicitud de organizador pendiente.");

        if (dto.MaxTeams < 2 || dto.MaxTeams > 16)
            throw new Exception("La cantidad de equipos debe ser entre 2 y 16.");

        if (!dto.IsInterUniversity && dto.UniversityId is null)
            throw new Exception("Debés indicar la universidad para un torneo exclusivo.");

        var request = new OrganizerRequest
        {
            Id = Guid.NewGuid(),
            UserId = userId,
            UserEmail = user.Email,
            TournamentName = dto.TournamentName,
            StartDate = dto.StartDate,
            Game = dto.Game,
            Description = dto.Description,
            IsInterUniversity = dto.IsInterUniversity,
            UniversityId = dto.IsInterUniversity ? null : dto.UniversityId,
            MaxTeams = dto.MaxTeams,
            Status = OrganizerRequestStatus.Pending,
            CreatedAt = DateTime.UtcNow
        };

        await _orgRequestRepository.AddAsync(request);
        await _orgRequestRepository.SaveChangesAsync();

        return ToResponseDto(request);
    }

    public async Task<OrganizerRequestResponseDto> ApproveAsync(string email)
    {
        var request = await _orgRequestRepository.GetByUserEmailAsync(email);
        if (request is null)
            throw new Exception("No se encontró solicitud de organizador para ese email.");

        if (request.Status != OrganizerRequestStatus.Pending)
            throw new Exception("La solicitud no está en estado pendiente.");

        request.Status = OrganizerRequestStatus.Approved;
        request.ReviewedAt = DateTime.UtcNow;

        var user = await _userRepository.GetByIdAsync(request.UserId);
        if (user is not null)
        {
            user.IsOrganizer = true;
            await _userRepository.UpdateAsync(user);
        }

        var tournament = new Domain.Entities.Tournament
        {
            Id = Guid.NewGuid(),
            Name = request.TournamentName,
            Game = request.Game,
            Description = request.Description,
            IsInterUniversity = request.IsInterUniversity,
            UniversityId = request.UniversityId,
            MaxTeams = request.MaxTeams,
            StartDate = request.StartDate,
            Status = TournamentStatus.Open,
            CreatedByUserId = request.UserId,
            CreatedAt = DateTime.UtcNow
        };

        await _tournamentRepository.AddAsync(tournament);
        await _orgRequestRepository.SaveChangesAsync();

        return ToResponseDto(request);
    }

    public async Task<OrganizerRequestResponseDto> RejectAsync(string email)
    {
        var request = await _orgRequestRepository.GetByUserEmailAsync(email);
        if (request is null)
            throw new Exception("No se encontró solicitud de organizador para ese email.");

        if (request.Status != OrganizerRequestStatus.Pending)
            throw new Exception("La solicitud no está en estado pendiente.");

        request.Status = OrganizerRequestStatus.Rejected;
        request.ReviewedAt = DateTime.UtcNow;
        await _orgRequestRepository.SaveChangesAsync();

        return ToResponseDto(request);
    }

    private static OrganizerRequestResponseDto ToResponseDto(OrganizerRequest r) => new()
    {
        Id = r.Id,
        TournamentName = r.TournamentName,
        StartDate = r.StartDate,
        Game = r.Game,
        Description = r.Description,
        IsInterUniversity = r.IsInterUniversity,
        UniversityName = r.University?.Name,
        MaxTeams = r.MaxTeams,
        Status = r.Status,
        CreatedAt = r.CreatedAt,
        ReviewedAt = r.ReviewedAt
    };
}
