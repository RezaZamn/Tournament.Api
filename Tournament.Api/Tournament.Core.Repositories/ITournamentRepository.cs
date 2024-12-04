using Tournament.Core.Entities;

namespace Tournament.Api.Tournament.Core.Repositories
{
    public interface ITournamentRepository
    {
        Task<IEnumerable<TournamentDetails>> GetAllAsync();
        Task<TournamentDetails> GetAsync(int id);
        Task<bool> AnyAsync(int id);
        void Add(TournamentDetails tournament);
        void Update(TournamentDetails tournament);
        void Remove(TournamentDetails tournament);
    }
}
