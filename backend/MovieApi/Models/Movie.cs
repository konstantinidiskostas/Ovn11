namespace MovieApi.Models;

public class Movie
{
    public int Id { get; set; }
    public string Title { get; set; }
    public int  Year { get; set; }
    public string Genre { get; set; }
    public int Duration { get; set; }
    
    //1:1
    public MovieDetails MovieDetails { get; set; }
    //1:M
    public List<Review> Reviews { get; set; }
    //N:M
    public List<MovieActor> MovieActors { get; set; }
}