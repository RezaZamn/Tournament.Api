using Tournament.Core.Dto;

namespace Tournament.Services.Contracts
{
    public interface IGameService
    {
        Task<IEnumerable<GameDto>> GetAllGamesAsync();
        Task<GameDto> GetGameByIdAsync(int gameId); 
        Task<GameDto> CreateGameAsync(GameDto gameDto);
        Task<GameDto> UpdateGameAsync(int gameId, GameDto gameDto);
        Task<GameDto> DeleteGameAsync(int gameId);
    }
}
