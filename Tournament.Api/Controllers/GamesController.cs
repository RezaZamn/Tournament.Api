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
using Azure;
using Microsoft.AspNetCore.JsonPatch;

namespace Tournament.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GamesController : ControllerBase
    {
        private readonly TournamentApiContext _context;
        private readonly IUnitOfWorkInterface _unitOfWork;
        private readonly IMapper _mapper;

        public GamesController(TournamentApiContext context, IUnitOfWorkInterface unitOfWork, IMapper mapper)
        {
            _context = context;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        

        // GET: api/Games
        [HttpGet]
        public async Task<ActionResult<IEnumerable<GameDto>>> GetGame()
        {

            //return await _context.Games.ToListAsync();

            var games = await _unitOfWork.Games.GetAllAsync();

            if (games == null)
            {
                return NotFound();
            }

            var gameDto = _mapper.Map<IEnumerable<GameDto>>(games);
            return Ok(gameDto);
   
        }

        // GET: api/Games/5
        [HttpGet("{id}")]
        public async Task<ActionResult<GameDto>> GetGame(int id)
        {
            //var game = await _context.Games.FindAsync(id);

             var game = await _unitOfWork.Games.GetAsync(id); //Anropar GetAsync via UnitOfWork

            if (game == null)
            {
                return NotFound($"No game founded with the title {game}");
            }

            var gameDto = _mapper.Map<GameDto>(game);

            return Ok(gameDto);

        }


        //GET: api/Games/some string
        [HttpGet("search/{title}")]
        public async Task<ActionResult<IEnumerable<GameDto>>> GetGamesByTitle(string title)
        {
            if(string.IsNullOrEmpty(title))
            {
                return BadRequest("The title can not be empty");
            }

            var allGames = await _unitOfWork.Games.GetAllAsync();
            var games = allGames.Where(g => g.Title == title).ToList();


            if (games.Count == 0)
            {
                return NotFound($"No games founded with the title {title}");
            }

            var gameDto = _mapper.Map<IEnumerable<GameDto>>(games);

            return Ok(gameDto);

        }


        // PUT: api/Games/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutGame(int id, GameDto gameDto)
        {
            
            if (id != gameDto.Id)
            {
                return BadRequest();
            }

            var game = _mapper.Map<Game>(gameDto);

            // _context.Entry(game).State = EntityState.Modified;
            _unitOfWork.Games.Update(game); //Uppdaterar spelet via repository

            try
            {
                //await _context.SaveChangesAsync();
                await _unitOfWork.CompleteAsync(); //Sparar ändringarna
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await GameExists(id))
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

        // POST: api/Games
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<GameDto>> PostGame(GameDto gameDto)
        {
            // _context.Games.Add(game);              
            // await _context.SaveChangesAsync();

            var game = _mapper.Map<Game>(gameDto);
            _unitOfWork.Games.Add(game); //Lägger till spelet via repository
            await _unitOfWork.CompleteAsync(); //Sparar ändringarna

            return CreatedAtAction("GetGame", new { id = game.Id }, game);

        }

        // DELETE: api/Games/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteGame(int id)
        {
            //var game = await _context.Games.FindAsync(id);
            
            var game = await _unitOfWork.Games.GetAsync(id);
            

            if (game == null)
            {
                return NotFound();
            }

            //_context.Games.Remove(game);
            //await _context.SaveChangesAsync();

            var gameDto = _mapper.Map<Game>(game);
            _unitOfWork.Games.Remove(gameDto); //Tar bort spelet via repository
            await _unitOfWork.CompleteAsync(); //Sparar ändringarna

            return NoContent();
        }

        [HttpPatch("{id}")]
        public async Task<ActionResult<GameDto>> PatchGame(int id, JsonPatchDocument<GameDto> patchDocument)
        {
            if (patchDocument == null) return NotFound("No patch document");

            //Hämtar Orginal spelet från databasen
            var gameToPatch = await _unitOfWork.Games.GetAsync(id);

            if (gameToPatch == null)
            {
                return NotFound("Game med Id {gameId} hittades inte");
            }

            //Mappar till Dton
            var gameDto = _mapper.Map<GameDto>(gameToPatch);

            //Applicerar patchDocument på Dton
            patchDocument.ApplyTo(gameDto, ModelState);

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            //Updaterar Dton men nya värden
            _mapper.Map(gameDto, gameToPatch);

            try
            {
                await _unitOfWork.CompleteAsync();
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Kunde inte utföra operationen");
            }

            return Ok(gameDto);

        }


        [HttpGet("{id}/exists")]
        public Task<bool> GameExists(int id)
        {
            return _unitOfWork.Games.AnyAsync(id);
        }


    }
}
