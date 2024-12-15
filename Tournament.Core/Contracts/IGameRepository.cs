using Tournament.Core.Entities;

namespace Tournament.Api.Games.Core.Repositories
{
    public interface IGameRepository
    {
             
            Task<IEnumerable<Game>> GetAllAsync();
            Task<Game> GetAsync(int id);
            Task<bool> AnyAsync(int id);
            Task AddAsync(Game game);
            void Update(Game game);
            void Remove(Game game);
        

    }
}
