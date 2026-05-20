using CampusClash.Application.DTOs.Riot;
using CampusClash.Application.Interfaces;

namespace CampusClash.Application.Services;

public class RiotLinkService
{
    private readonly IRiotService _riotService;
    private readonly IUserRepository _userRepository;

    public RiotLinkService(IRiotService riotService, IUserRepository userRepository)
    {
        _riotService = riotService;
        _userRepository = userRepository;
    }

    public async Task<bool> LinkRiotAccountAsync(Guid userId, LinkRiotRequestDto request)
    {
        var user = await _userRepository.GetByIdAsync(userId);
        if (user == null)
            throw new Exception("Usuario no encontrado.");

        var riotAccount = await _riotService.GetAccountByRiotIdAsync(
            request.GameName, request.TagLine);

        if (riotAccount == null)
            throw new Exception("No se encontró la cuenta de Riot. Verificá el nombre y tag.");

        user.RiotGameName = riotAccount.GameName;
        user.RiotTagLine = riotAccount.TagLine;
        user.IsRiotLinked = true;

        await _userRepository.UpdateAsync(user);
        await _userRepository.SaveChangesAsync();

        return true;
    }
}