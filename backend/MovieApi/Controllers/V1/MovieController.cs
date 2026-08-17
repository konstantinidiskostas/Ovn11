using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using MovieApi.DTOs;
using MovieApi.Services;

namespace MovieApi.Controllers.V1
{
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiVersion("1.0")]
    [ApiController]
    public class MovieController : ControllerBase
    {
        private readonly IMovieService _movieService;

        public MovieController(IMovieService movieService)
        {
            _movieService = movieService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<MovieDto>>> GetMovie()
        {
            var movies = await _movieService.GetAllAsync();
            return Ok(movies);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<MovieDto>> GetMovie(int id)
        {
            var movie = await _movieService.GetByIdAsync(id);
            if (movie == null) return NotFound();
            return Ok(movie);
        }

        [HttpGet("{id}/details")]
        public async Task<ActionResult<MovieDetailDto>> GetMovieDetails(int id)
        {
            var movie = await _movieService.GetDetailsAsync(id);
            if (movie == null) return NotFound();
            return Ok(movie);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutMovie(int id, MovieUpdateDto movieUpdateDto)
        {
            var updated = await _movieService.UpdateAsync(id, movieUpdateDto);
            if (!updated)
            {
                if (id != movieUpdateDto.Id) return BadRequest("Movie ID mismatch");
                return NotFound();
            }
            return NoContent();
        }

        [HttpPost]
        public async Task<ActionResult<MovieDto>> PostMovie(MovieCreateDto movieCreateDto)
        {
            var movieDto = await _movieService.CreateAsync(movieCreateDto);
            return CreatedAtAction(nameof(GetMovie), new { id = movieDto.Id }, movieDto);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMovie(int id)
        {
            var deleted = await _movieService.DeleteAsync(id);
            if (!deleted) return NotFound();
            return NoContent();
        }
    }
}
