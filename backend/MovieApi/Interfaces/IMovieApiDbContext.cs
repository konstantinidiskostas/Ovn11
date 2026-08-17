namespace MovieApi.Interfaces;

using Microsoft.EntityFrameworkCore;
using MovieApi.Models;

public interface IMovieApiDbContext
{
    DbSet<Actor> Actor { get; set; }
    DbSet<Movie> Movie { get; set; }
    DbSet<MovieActor> MovieActor { get; set; }
    DbSet<Review> Review { get; set; }
    DbSet<MovieDetails> MovieDetails { get; set; }
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    Microsoft.EntityFrameworkCore.ChangeTracking.EntityEntry<TEntity> Entry<TEntity>(TEntity entity) where TEntity : class;
}
