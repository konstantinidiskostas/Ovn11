namespace MovieApi.Models;

public class MovieDetails
{
    public int Id { get; set; }
    public string Synopsis { get; set; }
    public string Language { get; set; }
    public int budget { get; set; }
    
    //1:1
    public int MovieID { get; set; }
    public Movie Movie { get; set; }
}