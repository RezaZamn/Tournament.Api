using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tournament.Services.Contracts;
using Tournament.Services;



namespace Tournament.Services
{
    public class ServiceManager : IServiceManager
    {
        private readonly ITournamentService _tournamentService;
        private readonly IGameService _gameService;

        public ServiceManager(ITournamentService tournamentService, IGameService gameService)
        {
            _tournamentService = tournamentService;
            _gameService = gameService;
        }

        public ITournamentService TournamentService => _tournamentService;
        public IGameService GameService => _gameService;
    }
}
