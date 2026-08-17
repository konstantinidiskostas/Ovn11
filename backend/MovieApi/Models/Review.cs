namespace MovieApi.Models;

public class Review
{
    public int Id { get; set; }
    public string ReviewerName { get; set; }
    public string Comment { get; set; }
    public int Rating { get; set; }
    
    //M:1
    public int MovieID { get; set; }
    public Movie  Movie { get; set; }
    
}