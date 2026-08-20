using Microsoft.EntityFrameworkCore;
using MovieApi.Data;
using MovieApi.DTOs;
using MovieApi.Interfaces;
using MovieApi.Models;

namespace MovieApi.Services;

public class MovieService : IMovieService
{
    private readonly IMovieApiDbContext _db;

    public MovieService(IMovieApiDbContext db)
    {
        _db = db;
    }

public async Task<IEnumerable<MovieDto>> GetAllAsync(string? genre = null, string? search = null)
{
    var query = _db.Movie.AsQueryable();

    
    if (!string.IsNullOrWhiteSpace(genre))
    {
        query = query.Where(m => m.Genre.ToLower() == genre.ToLower());
    }

    
    if (!string.IsNullOrWhiteSpace(search))
    {
        query = query.Where(m => m.Title.ToLower().Contains(search.ToLower()));
    }

    return await query
        .Select(m => new MovieDto
        {
            Id = m.Id,
            Title = m.Title,
            Year = m.Year,
            Genre = m.Genre,
            Duration = m.Duration
        })
        .ToListAsync();
}

    public async Task<MovieDetailDto?> GetByIdAsync(int id)
    {
        var movie = await _db.Movie
            .Include(m => m.MovieActors)
            .ThenInclude(ma => ma.Actor)
            .Include(m => m.Reviews)
            .Include(m => m.MovieDetails)
            .FirstOrDefaultAsync(m => m.Id == id);

        if (movie == null) return null;

        return new MovieDetailDto
        {
            Id = movie.Id,
            Title = movie.Title,
            Year = movie.Year,
            Genre = movie.Genre,
            Duration = movie.Duration,
            Synopsis = movie.MovieDetails?.Synopsis ?? string.Empty,
            Language = movie.MovieDetails?.Language ?? string.Empty,
            Reviews = movie.Reviews.Select(r => new ReviewDto
            {
                Id = r.Id,
                ReviewerName = r.ReviewerName,
                Comment = r.Comment,
                Rating = r.Rating
            }).ToList(),
            Actors = movie.MovieActors.Select(ma => new ActorDto
            {
                Id = ma.Actor.Id,
                Name = ma.Actor.Name,
                BirthYear = ma.Actor.BirthYear,
                Role = ma.Role
            }).ToList()
        };
    }

    public async Task<MovieDetailDto?> GetDetailsAsync(int id)
    {
        return await GetByIdAsync(id);
    }

    public async Task<MovieDto> CreateAsync(MovieCreateDto dto)
    {
        var movie = new Movie
        {
            Title = dto.Title,
            Year = dto.Year,
            Genre = dto.Genre,
            Duration = dto.Duration
        };

        _db.Movie.Add(movie);
        await _db.SaveChangesAsync(default);

        return new MovieDto
        {
            Id = movie.Id,
            Title = movie.Title,
            Year = movie.Year,
            Genre = movie.Genre,
            Duration = movie.Duration
        };
    }

    public async Task<bool> UpdateAsync(int id, MovieUpdateDto dto)
    {
        if (id != dto.Id) return false;

        var movie = await _db.Movie.FindAsync(id);
        if (movie == null) return false;

        movie.Title = dto.Title;
        movie.Year = dto.Year;
        movie.Genre = dto.Genre;
        movie.Duration = dto.Duration;

        await _db.SaveChangesAsync(default);
        return true;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var movie = await _db.Movie.FindAsync(id);
        if (movie == null) return false;

        _db.Movie.Remove(movie);
        await _db.SaveChangesAsync(default);
        return true;
    }
}
