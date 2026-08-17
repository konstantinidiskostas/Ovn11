namespace MovieApi.DTOs;

public class ReviewDto
{
    public int Id { get; set; }
    public string ReviewerName { get; set; }
    public string Comment { get; set; }
    public int Rating { get; set; }
}