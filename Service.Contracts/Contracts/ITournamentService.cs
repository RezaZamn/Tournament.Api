using Tournament.Core.Dto;

namespace Tournament.Services.Contracts
{
    public interface ITournamentService
    {
        Task<IEnumerable<TournamentDto>> GetAllTournamentAsync(int page, int pageSize);
        Task<TournamentDto> GetTournamentByIdAsync(int tournamentId);
        Task<TournamentDto> CreateTournamentAsync(TournamentDto tournamentDto);
        Task<TournamentDto> UpdateTournamentAsync(int tournamentDtoId, TournamentDto tournamentDto);
        Task DeleteTournamentAsync(int tournamentId);
    }
}