using Tournament.Core.Entities;

namespace Tournament.Api.Tournament.Core.Repositories
{
    public interface ITournamentRepository
    {
        Task<IEnumerable<TournamentDetails>> GetAllAsync();
        Task<TournamentDetails> GetByIdAsync(int id);
        Task<bool> AnyAsync(int id);
        Task AddAsync(TournamentDetails tournament);
        void UpdateAsync(TournamentDetails tournament);
        void Remove(TournamentDetails tournament);
    }
}
