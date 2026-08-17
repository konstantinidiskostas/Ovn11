using Moq;
using MovieApi.DTOs;
using MovieApi.Interfaces;
using MovieApi.Models;
using MovieApi.Services;

namespace MovieApi.Tests.Services;

public class MovieServiceTests
{
    [Fact]
    public async Task CreateAsync_AddsMovieAndReturnsDto()
    {
        var mockDb = new Mock<IMovieApiDbContext>();
        Movie? addedMovie = null;

        mockDb.Setup(d => d.Movie.Add(It.IsAny<Movie>()))
            .Callback<Movie>(m => addedMovie = m);
        mockDb.Setup(d => d.SaveChangesAsync(default))
            .ReturnsAsync(1)
            .Callback(() =>
            {
                if (addedMovie != null) addedMovie.Id = 1;
            });

        var service = new MovieService(mockDb.Object);
        var dto = new MovieCreateDto { Title = "Test", Year = 2020, Genre = "Action", Duration = 120 };

        var result = await service.CreateAsync(dto);

        Assert.Equal("Test", result.Title);
        mockDb.Verify(d => d.Movie.Add(It.Is<Movie>(m => m.Title == "Test")), Times.Once);
        mockDb.Verify(d => d.SaveChangesAsync(default), Times.Once);
    }

    [Fact]
    public async Task DeleteAsync_WithValidId_RemovesAndReturnsTrue()
    {
        var mockDb = new Mock<IMovieApiDbContext>();
        var movie = new Movie { Id = 1, Title = "Inception", Year = 2010, Genre = "Sci-Fi", Duration = 148 };

        mockDb.Setup(d => d.Movie.FindAsync(It.IsAny<object[]>()))
            .ReturnsAsync(movie);
        mockDb.Setup(d => d.SaveChangesAsync(default))
            .ReturnsAsync(1);

        var service = new MovieService(mockDb.Object);
        var result = await service.DeleteAsync(1);

        Assert.True(result);
        mockDb.Verify(d => d.Movie.Remove(movie), Times.Once);
        mockDb.Verify(d => d.SaveChangesAsync(default), Times.Once);
    }

    [Fact]
    public async Task DeleteAsync_WithInvalidId_ReturnsFalse()
    {
        var mockDb = new Mock<IMovieApiDbContext>();

        mockDb.Setup(d => d.Movie.FindAsync(It.IsAny<object[]>()))
            .ReturnsAsync((Movie?)null);

        var service = new MovieService(mockDb.Object);
        var result = await service.DeleteAsync(99);

        Assert.False(result);
        mockDb.Verify(d => d.Movie.Remove(It.IsAny<Movie>()), Times.Never);
    }

    [Fact]
    public async Task UpdateAsync_WithIdMismatch_ReturnsFalse()
    {
        var mockDb = new Mock<IMovieApiDbContext>();
        var service = new MovieService(mockDb.Object);
        var dto = new MovieUpdateDto { Id = 2, Title = "Test", Year = 2020, Genre = "Action", Duration = 120 };

        var result = await service.UpdateAsync(1, dto);

        Assert.False(result);
        mockDb.Verify(d => d.SaveChangesAsync(default), Times.Never);
    }

    [Fact]
    public async Task UpdateAsync_WithValidId_UpdatesAndReturnsTrue()
    {
        var mockDb = new Mock<IMovieApiDbContext>();
        var existingMovie = new Movie { Id = 1, Title = "Old", Year = 2000, Genre = "Comedy", Duration = 100 };

        mockDb.Setup(d => d.Movie.FindAsync(It.IsAny<object[]>()))
            .ReturnsAsync(existingMovie);
        mockDb.Setup(d => d.SaveChangesAsync(default))
            .ReturnsAsync(1);

        var service = new MovieService(mockDb.Object);
        var dto = new MovieUpdateDto { Id = 1, Title = "New", Year = 2020, Genre = "Action", Duration = 120 };

        var result = await service.UpdateAsync(1, dto);

        Assert.True(result);
        Assert.Equal("New", existingMovie.Title);
        Assert.Equal(2020, existingMovie.Year);
        mockDb.Verify(d => d.SaveChangesAsync(default), Times.Once);
    }
}
