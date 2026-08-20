using MovieApi.DTOs;

namespace MovieApi.Services;

public interface IMovieService
{
    Task<IEnumerable<MovieDto>> GetAllAsync(string? genre = null, string? search = null);
    Task<MovieDetailDto?> GetByIdAsync(int id);
    Task<MovieDetailDto?> GetDetailsAsync(int id);
    Task<MovieDto> CreateAsync(MovieCreateDto dto);
    Task<bool> UpdateAsync(int id, MovieUpdateDto dto);
    Task<bool> DeleteAsync(int id);
}
