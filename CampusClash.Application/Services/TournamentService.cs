using CampusClash.Application.DTOs.Tournament;
using CampusClash.Application.Interfaces;

namespace CampusClash.Application.Services;

public class TournamentService : ITournamentService
{
    private readonly ITournamentRepository _tournamentRepository;

    public TournamentService(ITournamentRepository tournamentRepository)
    {
        _tournamentRepository = tournamentRepository;
    }

    public async Task<List<TournamentResponseDto>> GetAllAsync()
    {
        var tournaments = await _tournamentRepository.GetAllAsync();
        return tournaments.Select(ToResponseDto).ToList();
    }

    public async Task<List<TournamentResponseDto>> GetMyTournamentsAsync(Guid userId)
    {
        var tournaments = await _tournamentRepository.GetByUserEnrollmentAsync(userId);
        return tournaments.Select(ToResponseDto).ToList();
    }

    public async Task<TournamentResponseDto> GetByIdAsync(Guid id)
    {
        var tournament = await _tournamentRepository.GetByIdAsync(id);
        if (tournament is null)
            throw new Exception("Torneo no encontrado.");
        return ToResponseDto(tournament);
    }

    public async Task<TournamentResponseDto> UpdateAsync(Guid tournamentId, Guid organizerId, UpdateTournamentDto dto)
    {
        var tournament = await _tournamentRepository.GetByIdAsync(tournamentId);
        if (tournament is null)
            throw new Exception("Torneo no encontrado.");

        if (tournament.CreatedByUserId != organizerId)
            throw new Exception("No tenés permiso para editar este torneo.");

        if (dto.MaxTeams < 2 || dto.MaxTeams > 16)
            throw new Exception("La cantidad de equipos debe ser entre 2 y 16.");

        if (!dto.IsInterUniversity && dto.UniversityId is null)
            throw new Exception("Debés indicar la universidad para un torneo exclusivo.");

        tournament.Name = dto.Name;
        tournament.StartDate = dto.StartDate;
        tournament.Game = dto.Game;
        tournament.Description = dto.Description;
        tournament.IsInterUniversity = dto.IsInterUniversity;
        tournament.UniversityId = dto.IsInterUniversity ? null : dto.UniversityId;
        tournament.MaxTeams = dto.MaxTeams;

        await _tournamentRepository.SaveChangesAsync();
        return ToResponseDto(tournament);
    }

    private static TournamentResponseDto ToResponseDto(Domain.Entities.Tournament t) => new()
    {
        Id = t.Id,
        Name = t.Name,
        Game = t.Game,
        Description = t.Description,
        IsInterUniversity = t.IsInterUniversity,
        UniversityName = t.University?.Name,
        MaxTeams = t.MaxTeams,
        StartDate = t.StartDate,
        Status = t.Status,
        OrganizerUsername = t.CreatedBy?.Username ?? string.Empty,
        CreatedAt = t.CreatedAt,
        Teams = t.Teams.Select(team => new TeamDto
        {
            Id = team.Id,
            Name = team.Name,
            UniversityName = team.University?.Name ?? string.Empty,
            IsFull = team.Enrollments.Count >= 5,
            Players = team.Enrollments.Select(e => new PlayerDto
            {
                UserId = e.UserId,
                Username = e.User?.Username ?? string.Empty
            }).ToList()
        }).ToList()
    };
}
