namespace MovieApi.DTOs;
using System.ComponentModel.DataAnnotations;

public class MovieActorDto
{
    [Required]
    public string Role { get; set; } = string.Empty;
}