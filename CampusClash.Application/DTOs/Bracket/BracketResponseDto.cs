namespace CampusClash.Application.DTOs.Bracket;

public class BracketResponseDto
{
    public Guid TournamentId { get; set; }
    public List<RoundDto> Rounds { get; set; } = new();
}

public class RoundDto
{
    public int Round { get; set; }
    public string RoundName { get; set; } = string.Empty;
    public List<MatchDto> Matches { get; set; } = new();
}

public class MatchDto
{
    public Guid Id { get; set; }
    public int MatchNumber { get; set; }
    public Guid? TeamAId { get; set; }
    public string? TeamAName { get; set; }
    public Guid? TeamBId { get; set; }
    public string? TeamBName { get; set; }
    public Guid? WinnerId { get; set; }
    public string? WinnerName { get; set; }
}
