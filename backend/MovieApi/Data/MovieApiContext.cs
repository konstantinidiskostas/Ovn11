using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MovieApi.Models;
using MovieApi.Interfaces;

namespace MovieApi.Data
{
    public class MovieApiContext : DbContext, IMovieApiDbContext
    {
        public MovieApiContext (DbContextOptions<MovieApiContext> options)
            : base(options)
        {
        }

        public DbSet<MovieApi.Models.Movie> Movie { get; set; } = default!;
        public DbSet<MovieApi.Models.Actor> Actor { get; set; } = default!;
       
        public DbSet<MovieActor> MovieActor { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<MovieActor>()
                .HasKey(ma => new { ma.MovieId, ma.ActorId });
            
            modelBuilder.Entity<MovieActor>()
                .HasOne(ma => ma.Movie)
                .WithMany(m => m.MovieActors)
                .HasForeignKey(ma => ma.MovieId);
            
            modelBuilder.Entity<MovieActor>()
                .HasOne(ma => ma.Actor)
                .WithMany(m => m.MovieActors)
                .HasForeignKey(ma => ma.ActorId);
        }
        public DbSet<MovieApi.Models.Review> Review { get; set; } = default!;
        public DbSet<MovieDetails> MovieDetails { get; set; } = default!;
    }
}
