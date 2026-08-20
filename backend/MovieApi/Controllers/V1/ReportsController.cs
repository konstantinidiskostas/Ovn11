using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MovieApi.Data;
using MovieApi.DTOs;

namespace MovieApi.Controllers.V1
{
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiVersion("1.0")]
    [ApiController]
    public class ReportsController : ControllerBase
    {
        private readonly MovieApiContext _context;

        public ReportsController(MovieApiContext context)
        {
            _context = context;
        }

        // GET: api/v1/Reports/top-movies-per-genre
        [HttpGet("top-movies-per-genre")]
        public async Task<ActionResult<IEnumerable<TopGenreReportDto>>> GetTopMoviesPerGenre()
        {
            var report = await _context.Movie
                .GroupBy(m => m.Genre)
                .Select(g => new TopGenreReportDto
                {
                    Genre = g.Key,
                    MovieCount = g.Count()
                })
                .OrderByDescending(r => r.MovieCount)
                .ToListAsync();

            return Ok(report);
        }

        // GET: api/v1/Reports/average-rating
        [HttpGet("average-rating")]
        public async Task<ActionResult<IEnumerable<AverageRatingReportDto>>> GetAverageRating()
        {
            var report = await _context.Review
                .GroupBy(r => r.Movie.Title)
                .Select(g => new AverageRatingReportDto
                {
                    Title = g.Key,
                    AverageRating = Math.Round(g.Average(r => r.Rating), 1),
                    ReviewCount = g.Count()
                })
                .OrderByDescending(r => r.AverageRating)
                .ToListAsync();

            return Ok(report);
        }

        // GET: api/v1/Reports/active-actors
        [HttpGet("active-actors")]
        public async Task<ActionResult<IEnumerable<ActiveActorReportDto>>> GetActiveActors()
        {
            var report = await _context.MovieActor
                .GroupBy(ma => ma.Actor.Name)
                .Select(g => new ActiveActorReportDto
                {
                    Name = g.Key,
                    MovieCount = g.Count()
                })
                .OrderByDescending(r => r.MovieCount)
                .ToListAsync();

            return Ok(report);
        }
    }
}
