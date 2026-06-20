using CampusClash.Application.DTOs.Bracket;
using CampusClash.Application.Interfaces;
using CampusClash.Domain.Entities;
using CampusClash.Domain.Enums;

namespace CampusClash.Application.Services;

public class BracketService : IBracketService
{
    private readonly ITournamentRepository _tournamentRepository;
    private readonly IBracketRepository _bracketRepository;

    // Round names indexed by MaxTeams → ordered from first to last round
    private static readonly Dictionary<int, string[]> RoundNames = new()
    {
        [16] = ["16avos de final", "Cuartos de final", "Semifinal", "Final"],
        [8]  = ["Cuartos de final", "Semifinal", "Final"],
        [4]  = ["Semifinal", "Final"],
        [2]  = ["Final"],
    };

    public BracketService(ITournamentRepository tournamentRepository, IBracketRepository bracketRepository)
    {
        _tournamentRepository = tournamentRepository;
        _bracketRepository = bracketRepository;
    }

    public async Task<BracketResponseDto> GenerateAsync(Guid tournamentId, Guid organizerId)
    {
        var tournament = await _tournamentRepository.GetByIdAsync(tournamentId)
            ?? throw new Exception("Torneo no encontrado.");

        if (tournament.CreatedByUserId != organizerId)
            throw new Exception("No tenés permiso para generar el bracket.");

        if (tournament.Status != TournamentStatus.Open)
            throw new Exception("El torneo debe estar en estado 'Open' para generar el bracket.");

        if (!RoundNames.TryGetValue(tournament.MaxTeams, out var roundNames))
            throw new Exception($"El número de equipos ({tournament.MaxTeams}) no es válido. Debe ser 2, 4, 8 o 16.");

        if (tournament.Teams.Count != tournament.MaxTeams)
            throw new Exception($"El torneo necesita exactamente {tournament.MaxTeams} equipos. Actualmente hay {tournament.Teams.Count}.");

        if (await _bracketRepository.ExistsForTournamentAsync(tournamentId))
            throw new Exception("El bracket ya fue generado para este torneo.");

        var teams = tournament.Teams.ToList().OrderBy(_ => Random.Shared.Next()).ToList();
        var totalRounds = roundNames.Length;
        var matches = new List<TournamentMatch>();

        for (int round = 1; round <= totalRounds; round++)
        {
            // matches per round = MaxTeams / 2^round
            var matchCount = tournament.MaxTeams / (int)Math.Pow(2, round);

            for (int matchNumber = 1; matchNumber <= matchCount; matchNumber++)
            {
                var match = new TournamentMatch
                {
                    Id = Guid.NewGuid(),
                    TournamentId = tournamentId,
                    Round = round,
                    RoundName = roundNames[round - 1],
                    MatchNumber = matchNumber,
                };

                // Only seed teams in the first round
                if (round == 1)
                {
                    match.TeamAId = teams[(matchNumber - 1) * 2].Id;
                    match.TeamBId = teams[(matchNumber - 1) * 2 + 1].Id;
                }

                matches.Add(match);
            }
        }

        tournament.Status = TournamentStatus.InProgress;

        await _bracketRepository.AddMatchesAsync(matches);
        await _bracketRepository.SaveChangesAsync();

        return BuildResponse(tournamentId, matches, tournament.Teams.ToList());
    }

    public async Task<BracketResponseDto> GetAsync(Guid tournamentId)
    {
        var tournament = await _tournamentRepository.GetByIdAsync(tournamentId)
            ?? throw new Exception("Torneo no encontrado.");

        var matches = await _bracketRepository.GetByTournamentAsync(tournamentId);

        if (matches.Count == 0)
            throw new Exception("El bracket aún no fue generado para este torneo.");

        return BuildResponse(tournamentId, matches, tournament.Teams.ToList());
    }

    private static BracketResponseDto BuildResponse(Guid tournamentId, List<TournamentMatch> matches, List<Team> teams)
    {
        var teamMap = teams.ToDictionary(t => t.Id, t => t.Name);

        var rounds = matches
            .GroupBy(m => m.Round)
            .OrderBy(g => g.Key)
            .Select(g => new RoundDto
            {
                Round = g.Key,
                RoundName = g.First().RoundName,
                Matches = g.OrderBy(m => m.MatchNumber).Select(m => new MatchDto
                {
                    Id = m.Id,
                    MatchNumber = m.MatchNumber,
                    TeamAId = m.TeamAId,
                    TeamAName = m.TeamAId.HasValue && teamMap.TryGetValue(m.TeamAId.Value, out var nameA) ? nameA : null,
                    TeamBId = m.TeamBId,
                    TeamBName = m.TeamBId.HasValue && teamMap.TryGetValue(m.TeamBId.Value, out var nameB) ? nameB : null,
                    WinnerId = m.WinnerId,
                    WinnerName = m.WinnerId.HasValue && teamMap.TryGetValue(m.WinnerId.Value, out var nameW) ? nameW : null,
                }).ToList()
            }).ToList();

        return new BracketResponseDto { TournamentId = tournamentId, Rounds = rounds };
    }
}
