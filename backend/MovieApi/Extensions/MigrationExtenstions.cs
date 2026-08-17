using Microsoft.EntityFrameworkCore;
using MovieApi.Data;
using MovieApi.Models;

namespace MovieApi.Extensions;

public static class MigrationExtensions
{
    public static void SeedData(this IApplicationBuilder app)
    {
        using var scope = app.ApplicationServices.CreateScope();
        using var context = scope.ServiceProvider.GetRequiredService<MovieApiContext>();

        context.Database.Migrate();

        if (context.Set<Movie>().Any()) return;

        var leo = new Actor { Name = "Leonardo DiCaprio", BirthYear = 1974 };
        var tom = new Actor { Name = "Tom Hanks", BirthYear = 1956 };
        var margot = new Actor { Name = "Margot Robbie", BirthYear = 1990 };
        var matthew = new Actor { Name = "Matthew McConaughey", BirthYear = 1969 };

        var inception = new Movie
        {
            Title = "Inception",
            Year = 2010,
            Genre = "Sci-Fi",
            Duration = 148,
            MovieDetails = new MovieDetails
            {
                Synopsis = "A thief who steals corporate secrets through dream-sharing technology.",
                Language = "English",
                budget = 160000000
            },
            Reviews = new List<Review>
            {
                new Review { ReviewerName = "Alice", Comment = "Brilliant!", Rating = 5 },
                new Review { ReviewerName = "Bob", Comment = "Mind-bending", Rating = 4 }
            },
            MovieActors = new List<MovieActor>
            {
                new MovieActor { Actor = leo, Role = "Dom Cobb" }
            }
        };

        var forrestGump = new Movie
        {
            Title = "Forrest Gump",
            Year = 1994,
            Genre = "Drama",
            Duration = 142,
            MovieDetails = new MovieDetails
            {
                Synopsis = "The presidencies of Kennedy and Johnson through the eyes of an Alabama man.",
                Language = "English",
                budget = 55000000
            },
            Reviews = new List<Review>
            {
                new Review { ReviewerName = "Charlie", Comment = "A classic!", Rating = 5 }
            },
            MovieActors = new List<MovieActor>
            {
                new MovieActor { Actor = tom, Role = "Forrest Gump" }
            }
        };

        var wolf = new Movie
        {
            Title = "The Wolf of Wall Street",
            Year = 2013,
            Genre = "Comedy",
            Duration = 180,
            MovieDetails = new MovieDetails
            {
                Synopsis = "Based on the true story of Jordan Belfort.",
                Language = "English",
                budget = 100000000
            },
            Reviews = new List<Review>
            {
                new Review { ReviewerName = "Diana", Comment = "Hilarious and shocking", Rating = 4 },
                new Review { ReviewerName = "Eve", Comment = "Too long but worth it", Rating = 3 }
            },
            MovieActors = new List<MovieActor>
            {
                new MovieActor { Actor = leo, Role = "Jordan Belfort" },
                new MovieActor { Actor = margot, Role = "Naomi Lapaglia" }
            }
        };

        var interstellar = new Movie
        {
            Title = "Interstellar",
            Year = 2014,
            Genre = "Sci-Fi",
            Duration = 169,
            MovieDetails = new MovieDetails
            {
                Synopsis = "A team of explorers travel through a wormhole in space.",
                Language = "English",
                budget = 165000000
            },
            Reviews = new List<Review>
            {
                new Review { ReviewerName = "Frank", Comment = "Masterpiece", Rating = 5 }
            },
            MovieActors = new List<MovieActor>
            {
                new MovieActor { Actor = matthew, Role = "Cooper" }
            }
        };

        context.Set<Movie>().AddRange(inception, forrestGump, wolf, interstellar);
        context.SaveChanges();
    }
}