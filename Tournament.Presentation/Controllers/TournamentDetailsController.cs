using Tournament.Services.Contracts;
using System;
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
using System.Runtime.InteropServices;


namespace Tournament.Presentation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TournamentDetailsController : ControllerBase
    {
        //private readonly TournamentApiContext _context;
        //private readonly IUnitOfWorkInterface _unitOfWork;
        //private readonly IMapper _mapper;
        private readonly IServiceManager _serviceManager;

        public TournamentDetailsController(IServiceManager serviceManager)
        {
           _serviceManager = serviceManager;
        }

        // GET: api/TournamentDetails
        [HttpGet]
        public async Task<ActionResult> GetAllTournaments(int page = 1, int pageSize = 20)
        {
            var tournaments = await _serviceManager.TournamentService.GetAllTournamentAsync(page, pageSize);
            return Ok(tournaments);

            ////return await _context.TournamentDetails.ToListAsync();
            //var tournaments = await _unitOfWork.Tournaments.GetAllAsync();

            //if (tournaments == null)
            //{
            //    return NotFound();
            //}

            //if (includedGames)
            //{
            //    var tournamentWithGames = _mapper.Map<IEnumerable<TournamentDto>>(tournaments);
            //    return Ok(tournamentWithGames);                        
            //}

            //else
            //{
            //    var tournamentWithoutGames = _mapper.Map<IEnumerable<TournamentDto>>(tournaments)
            //    .Select(t => new TournamentDto
            //     {
            //          Id = t.Id,
            //          Title = t.Title,
            //          StartDate = t.StartDate,

            //     });

            //    return Ok(tournamentWithoutGames);

            //var tournamentsDto = _mapper.Map<IEnumerable<TournamentDto>>(tournaments);
            //return Ok(tournamentsDto);
        }




    

        // GET: api/TournamentDetails/5
        [HttpGet("{id}")]
        public async Task<ActionResult> GetTournamentById(int id)
        {
           var tournament = await _serviceManager.TournamentService.GetTournamentByIdAsync(id);

            if (tournament == null)
            {
                return NotFound(new ProblemDetails
                {
                    Title = "Not Found",
                    Detail = $"No tournamnet found with id {id}"

                });

            }

            return Ok(tournament);

           // var tournamentDetails = await _context.TournamentDetails.FindAsync(id);
            //  var tournamentDetails = await _unitOfWork.Tournaments.GetAsync(id);

            //if (tournamentDetails == null)
            //{
            //    return NotFound();
            //}

            //var tournamentsDto = _mapper.Map<TournamentDto>(tournamentDetails);

            //return Ok(tournamentsDto);
        }


        // PUT: api/TournamentDetails/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateTournament(int id, [FromBody]TournamentDto tournamentDto)
        {
            if(tournamentDto == null)
            {
                return BadRequest(new ProblemDetails
                {
                    Title = "Bad Request",
                    Detail = "The tournament daata is null"

                });
            }

            await _serviceManager.TournamentService.UpdateTournamentAsync(id, tournamentDto);
            return NoContent();


            //if (id != tournamentDto.Id)
            //{
            //    return BadRequest();
            //}

            //var tournament = _mapper.Map<TournamentDetails>(tournamentDto);

            ////_context.Entry(tournamentDetails).State = EntityState.Modified;
            //  _unitOfWork.Tournaments.Update(tournament);

            //try
            //{
            //    //await _context.SaveChangesAsync();
            //      await _unitOfWork.CompleteAsync();
            //}
            //catch (DbUpdateConcurrencyException)
            //{
            //    if (!await TournamentDetailsExists(id))
            //    {
            //        return NotFound();
            //    }
            //    else
            //    {
            //        throw;
            //    }
            //}

            //return NoContent();
        }

        // POST: api/TournamentDetails
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult> CreateTournament([FromBody] TournamentDto tournamentDto)
        {
            if(tournamentDto == null)
            {
                return BadRequest(new ProblemDetails
                {
                    Title = "Bad Request",
                    Detail = "The tournament data is null"

                });
            }

            var createTournament = await _serviceManager.TournamentService.CreateTournamentAsync(tournamentDto);
            return CreatedAtAction(nameof(GetTournamentById), new {id = createTournament.Id}, createTournament);


            //_context.TournamentDetails.Add(tournamentDetails);
            //await _context.SaveChangesAsync();

            //var tournament = _mapper.Map<TournamentDetails>(tournamentDto);
            //_unitOfWork.Tournaments.Add(tournament);
            //await _unitOfWork.CompleteAsync();

            //return CreatedAtAction("GetTournamentDetails", new { id = tournament.Id }, tournamentDto);
        }

        // DELETE: api/TournamentDetails/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTournament(int id)
        {
            var tournament = await _serviceManager.TournamentService.GetTournamentByIdAsync(id);
            if(tournament == null)
            {
                return BadRequest(new ProblemDetails
                {
                    Title = "Tournament Not Found",
                    Detail = $"There is no tournament with id {id}"
                });

            }
            await _serviceManager.TournamentService.DeleteTournamentAsync(id);
            return NoContent();

            //var tournamentDetails = await _context.TournamentDetails.FindAsync(id);
            //var tournamentDetails = await _unitOfWork.Tournaments.GetAsync(id);
            //if (tournamentDetails == null)
            //{
            //    return NotFound("Element finns inte i databasen");
            //}

        
            //_context.TournamentDetails.Remove(tournamentDetails);
            //await _context.SaveChangesAsync();

            //_unitOfWork.Tournaments.Remove(tournamentDetails);
            //await _unitOfWork.CompleteAsync();

            //return NoContent();
        }

        [HttpPatch("{id}")]
        public async Task<ActionResult> PatchTournament(int id, [FromBody] JsonPatchDocument<TournamentDto> patchDocument)
        {
            if (patchDocument == null)
                return BadRequest("Patch document is invalid");
             
            //Hämtar aktuell tournering
            var tournament = await _serviceManager.TournamentService.GetTournamentByIdAsync(id);

            if (tournament == null)
                return NotFound();

            var tournamentPatch = new TournamentDto
            {
                Title = tournament.Title,
                StartDate = tournament.StartDate
            };

            //Applicerar änringarna
            patchDocument.ApplyTo(tournamentPatch, ModelState);

            //Kontrollerar ModelState om det finns någon valideringsfel
            if (!ModelState.IsValid)
              { return BadRequest(ModelState); }

            await _serviceManager.TournamentService.UpdateTournamentAsync(id, tournamentPatch);

            return NoContent();


                    //if (patchDocument == null) return BadRequest("No patch document");

                    ////Hämtar orginal tournamnet från databasen
                    //var tournamentToPatch = await _unitOfWork.Tournaments.GetAsync(tournamentId);

                    //if (tournamentToPatch == null)
                    //{
                    //    return NotFound("Tournamnet med Id {tournamnetId} Hittades inte");
                    //}

                    ////Mappar till en dto för att använda i patch-dokument
                    //var tournamentDto = _mapper.Map<TournamentDto>(tournamentToPatch);

                    ////Applicera patch-document på dton
                    //patchDocument.ApplyTo(tournamentDto, ModelState);

                    //if (!ModelState.IsValid)
                    //{
                    //    return BadRequest(ModelState);
                    //}

                    ////Uppdaterar den orginala objektet med nya värden
                    //_mapper.Map(tournamentDto, tournamentToPatch);

                    //try
                    //{
                    //    await _unitOfWork.CompleteAsync();
                    //}
                    //catch (Exception ex)
                    //{
                    //    return BadRequest("Kunde inte utföra operationen");
                    //}

                    //return Ok(tournamentDto);


                }

        //[HttpPost("{id}")]
        //public async Task<bool> TournamentDetailsExists(int id)
        //{
        //    //return _context.TournamentDetails.Any(e => e.Id == id);
        //    return await _unitOfWork.Tournaments.AnyAsync(id);
        //}
    }
}
