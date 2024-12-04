using Tournament.Api.Games.Core.Repositories;
using Tournament.Api.Games.Data.Repositories;
using Tournament.Api.Tournament.Core.Repositories;
using Tournament.Data.Data;


namespace Tournament.Api.Tournament.Data.Repositories
{
    public class UnitOfWorkClass : IUnitOfWorkInterface
    {
        private readonly TournamentApiContext _context;
        public ITournamentRepository Tournaments { get; }

        public IGameRepository Games{ get; }

        public UnitOfWorkClass(TournamentApiContext context, ITournamentRepository tournamentRepository, IGameRepository gameRepository)
        { 
        
            _context = context;
            Tournaments = tournamentRepository;
            Games = gameRepository;
        }        

        public async Task CompleteAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
