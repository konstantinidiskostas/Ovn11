using System.ComponentModel.DataAnnotations;

namespace MovieApi.DTOs;

public class MovieUpdateDto
{
    [Required]
    [Range(1,int.MaxValue)]
    public int Id { get; set; }
    [Required]
    public string Title { get; set; }
    [Range(1888,2100)]
    public int Year { get; set; }
    [Required]
    public string Genre { get; set; }
    [Range(1,600)]
    public int Duration { get; set; }
}