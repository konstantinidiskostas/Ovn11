namespace MovieApi.DTOs;

public class AverageRatingReportDto
{
    public string Title { get; set; } = string.Empty;
    public double AverageRating { get; set; }
    public int ReviewCount { get; set; }
}
