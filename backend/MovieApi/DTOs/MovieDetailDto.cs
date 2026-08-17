using MovieApi.Models;

namespace MovieApi.DTOs;

public class MovieDetailDto
{
    public int  Id { get; set; }
    public string Title { get; set; }
    public int Year { get; set; }
    public string Genre { get; set; }
    public int Duration { get; set; }
    
    // info
    public string Synopsis { get; set; }
    public string Language { get; set; }
    
    // reviews and actors
    public List<ReviewDto> Reviews { get; set; }
    public List<ActorDto> Actors { get; set; }
}