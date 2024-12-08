﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Tournament.Data.Data;
using Tournament.Core.Entities;
using Tournament.Api.Tournament.Data.Repositories;
using Tournament.Api.Tournament.Core.Repositories;
using AutoMapper;
using Tournament.Core.Dto;
using Microsoft.AspNetCore.JsonPatch;

namespace Tournament.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TournamentDetailsController : ControllerBase
    {
        private readonly TournamentApiContext _context;
        private readonly IUnitOfWorkInterface _unitOfWork;
        private readonly IMapper _mapper;

        public TournamentDetailsController(TournamentApiContext context, IUnitOfWorkInterface unitOfWork, IMapper mapper)
        {
            _context = context;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        // GET: api/TournamentDetails
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TournamentDto>>> GetTournamentDetails(bool includedGames = false)
        {
            //return await _context.TournamentDetails.ToListAsync();
            var tournaments = await _unitOfWork.Tournaments.GetAllAsync();

            if (tournaments == null)
            {
                return NotFound();
            }

            if (includedGames)
            {
                var tournamentWithGames = _mapper.Map<IEnumerable<TournamentDto>>(tournaments);
                return Ok(tournamentWithGames);                        
            }

            else
            {
                var tournamentWithoutGames = _mapper.Map<IEnumerable<TournamentDto>>(tournaments)
                .Select(t => new TournamentDto
                 {
                      Id = t.Id,
                      Title = t.Title,
                      StartDate = t.StartDate,

                 });

                return Ok(tournamentWithoutGames);
            }






            var tournamentsDto = _mapper.Map<IEnumerable<TournamentDto>>(tournaments);
            return Ok(tournamentsDto);
        }

        // GET: api/TournamentDetails/5
        [HttpGet("{id}")]
        public async Task<ActionResult<TournamentDto>> GetTournamentDetails(int id)
        {
           // var tournamentDetails = await _context.TournamentDetails.FindAsync(id);
              var tournamentDetails = await _unitOfWork.Tournaments.GetAsync(id);

            if (tournamentDetails == null)
            {
                return NotFound();
            }

            var tournamentsDto = _mapper.Map<TournamentDto>(tournamentDetails);

            return Ok(tournamentsDto);
        }

        // PUT: api/TournamentDetails/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTournamentDetails(int id, TournamentDto tournamentDto)
        {
            if (id != tournamentDto.Id)
            {
                return BadRequest();
            }

            var tournament = _mapper.Map<TournamentDetails>(tournamentDto);

            //_context.Entry(tournamentDetails).State = EntityState.Modified;
              _unitOfWork.Tournaments.Update(tournament);

            try
            {
                //await _context.SaveChangesAsync();
                  await _unitOfWork.CompleteAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await TournamentDetailsExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/TournamentDetails
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<TournamentDto>> PostTournamentDetails(TournamentDto tournamentDto)
        {
            //_context.TournamentDetails.Add(tournamentDetails);
            //await _context.SaveChangesAsync();

            var tournament = _mapper.Map<TournamentDetails>(tournamentDto);
            _unitOfWork.Tournaments.Add(tournament);
            await _unitOfWork.CompleteAsync();

            return CreatedAtAction("GetTournamentDetails", new { id = tournament.Id }, tournamentDto);
        }

        // DELETE: api/TournamentDetails/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTournamentDetails(int id)
        {
            //var tournamentDetails = await _context.TournamentDetails.FindAsync(id);
            var tournamentDetails = await _unitOfWork.Tournaments.GetAsync(id);
            if (tournamentDetails == null)
            {
                return NotFound("Element finns inte i databasen");
            }

        
            //_context.TournamentDetails.Remove(tournamentDetails);
            //await _context.SaveChangesAsync();

            _unitOfWork.Tournaments.Remove(tournamentDetails);
            await _unitOfWork.CompleteAsync();

            return NoContent();
        }

        [HttpPatch("{tournamentId}")]
        public async Task<ActionResult<TournamentDto>> PatchTournament(int tournamentId, [FromBody]JsonPatchDocument<TournamentDto> patchDocument)
        {

            if (patchDocument == null) return BadRequest("No patch document");

            //Hämtar orginal tournamnet från databasen
            var tournamentToPatch = await _unitOfWork.Tournaments.GetAsync(tournamentId);

            if (tournamentToPatch == null)
            {
                return NotFound("Tournamnet med Id {tournamnetId} Hittades inte");
            }

            //Mappar till en dto för att använda i patch-dokument
            var tournamentDto = _mapper.Map<TournamentDto>(tournamentToPatch);

            //Applicera patch-document på dton
            patchDocument.ApplyTo(tournamentDto, ModelState);

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            //Uppdaterar den orginala objektet med nya värden
            _mapper.Map(tournamentDto, tournamentToPatch);

            try
            {
                await _unitOfWork.CompleteAsync();
            }
            catch (Exception ex)
            {
                return BadRequest("Kunde inte utföra operationen");
            }

            return Ok(tournamentDto);


        }

        [HttpPost("{id}")]
        public async Task<bool> TournamentDetailsExists(int id)
        {
            //return _context.TournamentDetails.Any(e => e.Id == id);
            return await _unitOfWork.Tournaments.AnyAsync(id);
        }
    }
}
