using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tournament.Core.Dto;
using Tournament.Core.Entities;
using Tournament.Services.Contracts;
using AutoMapper;

namespace Tournament.Services
{
    public class TournamentService : ITournamentService
    {
        //private readonly IRepositoryManager _repository;
        private readonly IMapper _mapper;
        private readonly IUnitOfWorkInterface _unitOfWork;

        public TournamentService(IUnitOfWorkInterface unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<IEnumerable<TournamentDto>> GetAllTournamentAsync(int page, int pageSize)
        {
            ////Max antal objekt per sida
            //if(pageSize > 100) page = 100;

            ////Om ingen sida anges, sätt till 1
            //if(page < 1) page = 1;

            var tournaments = await _unitOfWork.Tournaments.GetAllAsync();
            return _mapper.Map<IEnumerable<TournamentDto>>(tournaments);

        }

        public async Task<TournamentDto> GetTournamentByIdAsync(int tournamentId)
        {
           var tournament = await _unitOfWork.Tournaments.GetByIdAsync(tournamentId);
            if (tournament == null)
            {
                throw new DirectoryNotFoundException($"Tournament with id{tournamentId} was not found");
            }

            return _mapper.Map<TournamentDto>(tournament);
        }

        public async Task<TournamentDto> CreateTournamentAsync(TournamentDto tournamentCreate)
        {
            var createTournament = _mapper.Map<TournamentDetails>(tournamentCreate);
            await _unitOfWork.Tournaments.AddAsync(createTournament);
            await _unitOfWork.CompleteAsync();

            return _mapper.Map<TournamentDto>(createTournament);

        }

        public async Task DeleteTournamentAsync(int tournamentId)
        {
            var tournament = await _unitOfWork.Tournaments.GetByIdAsync(tournamentId);
            if (tournament == null)
            {
                throw new DirectoryNotFoundException($"Tournament with id{tournamentId} was not found");
            }

            _unitOfWork.Tournaments.Remove(tournament);
            await _unitOfWork.CompleteAsync();

        }


        public async Task<TournamentDto> UpdateTournamentAsync(int tournamentId, TournamentDto tournamentUpdateDto)
        {
           var tournament = await _unitOfWork.Tournaments.GetByIdAsync(tournamentId);
            if(tournament == null)
            {
                throw new DirectoryNotFoundException($"Tournament with id{tournament} was not found");
                    
            }

             _mapper.Map(tournamentUpdateDto, tournament);
            _unitOfWork.Tournaments.UpdateAsync(tournament);
            await _unitOfWork.CompleteAsync();

            return _mapper.Map<TournamentDto>(tournamentUpdateDto);

        }
    }
}
