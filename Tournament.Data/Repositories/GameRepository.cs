using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Tournament.Api.Games.Core.Repositories;
using Tournament.Core.Entities;
using Tournament.Core.Contracts;
using Tournament.Data.Data;


namespace Tournament.Api.Games.Data.Repositories

{
    public class GameRepository : IGameRepository
    {
        private readonly TournamentApiContext _context;

        public GameRepository(TournamentApiContext context)
        {
            _context = context;
        }    


        public void Add(Game game)
        {
            _context.Game.Add(game);
        }

        public async Task<bool> AnyAsync(int id)
        {
            return await _context.Game.AnyAsync(g => g.Id == id);
        }

        public async Task CompleteAsync(int id)
        {
           await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<Game>> GetAllAsync()
        {
            return await _context.Game.ToListAsync();
        }

        public async Task<Game> GetAsync(int id)
        {
            return await _context.Game.FindAsync(id);
        }

        public void Remove(Game game)
        {
            _context.Game.Remove(game);
        }

        public void Update(Game game)
        {
            _context.Game.Update(game);
        }

        
    }
}
