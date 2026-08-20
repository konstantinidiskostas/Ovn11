using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MovieApi.Data;
using MovieApi.Models;
using MovieApi.DTOs;
using Asp.Versioning;

namespace MovieApi.Controllers.V1
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    
    public class ActorsController : ControllerBase
    {
        private readonly ILogger<ActorsController> _logger;
        private readonly MovieApiContext _context;

        public ActorsController(MovieApiContext context, ILogger<ActorsController> logger)
        {
            _context = context;
            _logger = logger;
        }
/// <summary>
/// returns the actors
/// </summary>
/// <remarks>a list of actors</remarks>
        // GET: api/Actors
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Actor>>> GetActor()
        {
            _logger.LogInformation("Fetching the Actors of the API version 1.");
            _logger.LogDebug("Trying to fetch the Actors of the API version 1");
            _logger.LogDebug("Actors fetched succesfully");
            return await _context.Actor.ToListAsync();
        }
/// <summary>
/// returns the actor by the id
/// </summary>
/// <param name="id"></param>
/// <returns>an actor</returns>
        // GET: api/Actors/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Actor>> GetActor(int id)
        {
            var actor = await _context.Actor.FindAsync(id);

            if (actor == null)
            {
                return NotFound();
            }

            return actor;
        }
/// <summary>
/// puts an actor
/// </summary>
/// <param name="id"></param>
/// <param name="dto"></param>
/// <returns></returns>
        // PUT: api/Actors/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutActor(int id, ActorUpdateDto dto)
        {
            if (id != dto.Id) return BadRequest();

            var actor = await _context.Actor.FindAsync(id);
            if (actor == null) return NotFound();

            actor.Name = dto.Name;
            actor.BirthYear = dto.BirthYear;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ActorExists(id)) return NotFound();
                throw;
            }

            return NoContent();
        }
/// <summary>
/// posts an actor
/// </summary>

        // POST: api/Actors
        [HttpPost]
        public async Task<ActionResult<Actor>> PostActor(ActorCreateDto dto)
        {
            var actor = new Actor
            {
                Name = dto.Name,
                BirthYear = dto.BirthYear
            };

            _context.Actor.Add(actor);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetActor", new { id = actor.Id }, actor);
        }
/// <summary>
/// deletes an actor
/// </summary>
/// <param name="id"></param>
/// <returns></returns>
        // DELETE: api/Actors/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteActor(int id)
        {
            var actor = await _context.Actor.FindAsync(id);
            if (actor == null)
            {
                return NotFound();
            }

            _context.Actor.Remove(actor);
            await _context.SaveChangesAsync();

            return NoContent();
        }
        /// <summary>
        /// posts an actor for a specific movie
        /// </summary>
        /// <param name="movieId"></param>
        /// <param name="actorId"></param>
        /// <param name="dto"></param>
        /// <returns></returns>
        // POST: /api/movies/{movieId}/actors/{actorId}
        [HttpPost("/api/v{version:ApiVersion}/movies/{movieId}/actors/{actorId}")]
        public async Task<IActionResult> AddActorToMovie(int movieId, int actorId, [FromBody] MovieActorDto dto)
        {
            var movieExists = await _context.Movie.AnyAsync(m => m.Id == movieId);
            if (!movieExists) return NotFound($"Movie {movieId} not found.");

            var actorExists = await _context.Actor.AnyAsync(a => a.Id == actorId);
            if (!actorExists) return NotFound($"Actor {actorId} not found.");

            var alreadyExists = await _context.MovieActor
                .AnyAsync(ma => ma.MovieId == movieId && ma.ActorId == actorId);
            if (alreadyExists) return BadRequest("Actor already assigned to this movie.");

            _context.MovieActor.Add(new MovieActor
            {
                MovieId = movieId,
                ActorId = actorId,
                Role = dto.Role
            });
            await _context.SaveChangesAsync();

            return Ok(new { message = "Actor linked with role." });
        }

        [HttpPut("/api/v{version:ApiVersion}/movies/{movieId}/actors/{actorId}")]
        public async Task<IActionResult> UpdateActorRole(int movieId, int actorId, [FromBody] MovieActorDto dto)
        {
            var movieActor = await _context.MovieActor
                .FirstOrDefaultAsync(ma => ma.MovieId == movieId && ma.ActorId == actorId);

            if (movieActor == null) return NotFound("Actor not assigned to this movie.");

            movieActor.Role = dto.Role;
            await _context.SaveChangesAsync();

            return Ok(new { message = "Role updated." });
        }

        private bool ActorExists(int id)
        {
            return _context.Actor.Any(e => e.Id == id);
        }
    }
}
