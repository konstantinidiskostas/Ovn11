using Microsoft.AspNetCore.Mvc;
using Moq;
using MovieApi.Controllers;
using MovieApi.DTOs;
using MovieApi.Services;

namespace MovieApi.Tests.Controllers;

public class MovieControllerTests
{
    [Fact]
    public async Task GetMovie_ReturnsOkWithList()
    {
        var mockService = new Mock<IMovieService>();
        mockService.Setup(s => s.GetAllAsync())
            .ReturnsAsync(new List<MovieDto>
            {
                new() { Id = 1, Title = "Inception", Year = 2010, Genre = "Sci-Fi", Duration = 148 }
            });

        var controller = new MovieController(mockService.Object);
        var result = await controller.GetMovie();

        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var movies = Assert.IsType<List<MovieDto>>(okResult.Value);
        Assert.Single(movies);
    }

    [Fact]
    public async Task GetMovie_WithValidId_ReturnsOk()
    {
        var mockService = new Mock<IMovieService>();
        mockService.Setup(s => s.GetByIdAsync(1))
            .ReturnsAsync(new MovieDetailDto { Id = 1, Title = "Inception" });

        var controller = new MovieController(mockService.Object);
        var result = await controller.GetMovie(1);

        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var movie = Assert.IsType<MovieDetailDto>(okResult.Value);
        Assert.Equal("Inception", movie.Title);
    }

    [Fact]
    public async Task GetMovie_WithInvalidId_ReturnsNotFound()
    {
        var mockService = new Mock<IMovieService>();
        mockService.Setup(s => s.GetByIdAsync(99)).ReturnsAsync((MovieDetailDto?)null);

        var controller = new MovieController(mockService.Object);
        var result = await controller.GetMovie(99);

        Assert.IsType<NotFoundResult>(result.Result);
    }

    [Fact]
    public async Task PostMovie_ReturnsCreatedAtAction()
    {
        var mockService = new Mock<IMovieService>();
        var dto = new MovieCreateDto { Title = "New", Year = 2020, Genre = "Action", Duration = 120 };
        mockService.Setup(s => s.CreateAsync(dto))
            .ReturnsAsync(new MovieDto { Id = 1, Title = "New", Year = 2020, Genre = "Action", Duration = 120 });

        var controller = new MovieController(mockService.Object);
        var result = await controller.PostMovie(dto);

        var createdResult = Assert.IsType<CreatedAtActionResult>(result.Result);
        Assert.Equal("GetMovie", createdResult.ActionName);
    }

    [Fact]
    public async Task DeleteMovie_WithValidId_ReturnsNoContent()
    {
        var mockService = new Mock<IMovieService>();
        mockService.Setup(s => s.DeleteAsync(1)).ReturnsAsync(true);

        var controller = new MovieController(mockService.Object);
        var result = await controller.DeleteMovie(1);

        Assert.IsType<NoContentResult>(result);
    }

    [Fact]
    public async Task DeleteMovie_WithInvalidId_ReturnsNotFound()
    {
        var mockService = new Mock<IMovieService>();
        mockService.Setup(s => s.DeleteAsync(99)).ReturnsAsync(false);

        var controller = new MovieController(mockService.Object);
        var result = await controller.DeleteMovie(99);

        Assert.IsType<NotFoundResult>(result);
    }
}
