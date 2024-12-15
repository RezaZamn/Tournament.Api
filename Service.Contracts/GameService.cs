using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tournament.Core.Dto;
using Tournament.Core.Entities;
using Tournament.Services.Contracts;

namespace Tournament.Services
{
    public class GameService : IGameService
    {
        private readonly IUnitOfWorkInterface _unitOfWork;
        private readonly IMapper _mapper;

        public GameService(IUnitOfWorkInterface unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<IEnumerable<GameDto>> GetAllGamesAsync(int page, int pageSize)
        {
            var games = await _unitOfWork.Games.GetAllAsync();
            return _mapper.Map<IEnumerable<GameDto>>(games);
        }

        public async Task<GameDto> GetGameByIdAsync(int gameId)
        {
            var game = await _unitOfWork.Games.GetAsync(gameId);

            if(game == null)
            {
                throw new Exception("Game not found");
            }

            return _mapper.Map<GameDto>(game);
        }

        public async Task<GameDto> CreateGameAsync(GameDto gameDto)
        {
            var gameCreate = _mapper.Map<Game>(gameDto);

            var tournament = await _unitOfWork.Tournaments.AddAsync(gameCreate.TournamentId);
            if(tournament == null)
            {
                throw new DirectoryNotFoundException("Tournament not found");
            }

            if (tournament.Games.Count >= 10)
            {
                throw new InvalidOperationException("A tournament can have maximum of 10 games");
            }

            await _unitOfWork.Games.AddAsync(gameCreate);
            await _unitOfWork.CompleteAsync();
            return _mapper.Map<GameDto>(gameCreate);
        }

        public async Task<GameDto> UpdateGameAsync(int gameId, GameDto gameDto)
        {
            var game = await _unitOfWork.Games.GetAsync(gameId);
            if (game == null)
            {
                throw new DirectoryNotFoundException($"Game with {gameId} not found.");
            }

            _mapper.Map(gameDto, game);
            _unitOfWork.Games.Update(game);
            await _unitOfWork.CompleteAsync();

            return _mapper.Map<GameDto>(game);
        }

        public async Task DeleteGameAsync(int gameId)
        {
            var game = await _unitOfWork.Games.GetAsync(gameId);
            if (game == null)
            {
                throw new DirectoryNotFoundException($"{gameId} does not exist");
            }

            _unitOfWork.Games.Remove(game);
            await _unitOfWork.CompleteAsync();


        }
    }
}
