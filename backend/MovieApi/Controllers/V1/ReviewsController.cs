using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MovieApi.Data;
using MovieApi.Models;
using MovieApi.DTOs;

namespace MovieApi.Controllers.V1
{
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    [ApiVersion("1.0")]
    public class ReviewsController : ControllerBase
    {
        private readonly MovieApiContext _context;

        public ReviewsController(MovieApiContext context)
        {
            _context = context;
        }

        // GET: api/Reviews
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Review>>> GetReview()
        {
            return await _context.Review.ToListAsync();
        }

        // GET: api/Reviews/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Review>> GetReview(int id)
        {
            var review = await _context.Review.FindAsync(id);

            if (review == null)
            {
                return NotFound();
            }

            return review;
        }

        // GET: api/movies/{movieId}/reviews
        [HttpGet("/api/movies/{movieId}/reviews")]
        public async Task<ActionResult<IEnumerable<ReviewDto>>> GetMovieReviews(int movieId)
        {
            var movieExists = await _context.Movie.AnyAsync(m => m.Id == movieId);
            if (!movieExists) return NotFound($"Movie {movieId} not found.");

            var reviews = await _context.Review
                .Where(r => r.MovieID == movieId)
                .Select(r => new ReviewDto
                {
                    Id = r.Id,
                    ReviewerName = r.ReviewerName,
                    Comment = r.Comment,
                    Rating = r.Rating
                })
                .ToListAsync();

            return Ok(reviews);
        }

        // PUT: api/Reviews/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutReview(int id, Review review)
        {
            if (id != review.Id)
            {
                return BadRequest();
            }

            _context.Entry(review).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ReviewExists(id))
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

        // POST: api/movies/{movieId}/reviews
        [HttpPost("/api/movies/{movieId}/reviews")]
        public async Task<ActionResult<ReviewDto>> PostMovieReview(int movieId, ReviewCreateDto dto)
        {
            var movieExists = await _context.Movie.AnyAsync(m => m.Id == movieId);
            if (!movieExists) return NotFound($"Movie {movieId} not found.");

            var review = new Review
            {
                MovieID = movieId,
                ReviewerName = dto.ReviewerName,
                Comment = dto.Comment,
                Rating = dto.Rating
            };

            _context.Review.Add(review);
            await _context.SaveChangesAsync();

            var reviewDto = new ReviewDto
            {
                Id = review.Id,
                ReviewerName = review.ReviewerName,
                Comment = review.Comment,
                Rating = review.Rating
            };

            return CreatedAtAction(nameof(GetMovieReviews), new { movieId }, reviewDto);
        }

        // DELETE: api/Reviews/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteReview(int id)
        {
            var review = await _context.Review.FindAsync(id);
            if (review == null)
            {
                return NotFound();
            }

            _context.Review.Remove(review);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ReviewExists(int id)
        {
            return _context.Review.Any(e => e.Id == id);
        }
    }
}
