using Microsoft.EntityFrameworkCore;
using Tournament.Api.Tournament.Core.Repositories;
using Tournament.Core.Entities;
using Tournament.Data.Data;

namespace Tournament.Api.Tournament.Data.Repositories
{
    public class TournamentRepository : ITournamentRepository
    {
        private readonly TournamentApiContext _context;

        public TournamentRepository(TournamentApiContext context)
        {
            _context = context;
        }
        public void Add(TournamentDetails tournament)
        {
            _context.TournamentDetails.Add(tournament);
        }

        public async Task<bool> AnyAsync(int id)
        {
            return await _context.TournamentDetails.AnyAsync(t => t.Id == id);
           
        }

        public async Task<IEnumerable<TournamentDetails>> GetAllAsync()
        {
          return await _context.TournamentDetails.ToListAsync();
        }

        public async Task<TournamentDetails> GetAsync(int id)
        {
            return await _context.TournamentDetails.FindAsync(id);
        }

        public void Remove(TournamentDetails tournament)
        {
            _context.TournamentDetails.Remove(tournament);
        }

        public void Update(TournamentDetails tournament)
        {
            _context.TournamentDetails.Update(tournament);
        }
    }
}
