using System.ComponentModel.DataAnnotations;

namespace MovieApi.DTOs;

public class ActorUpdateDto
{
    [Required]
    [Range(1, int.MaxValue)]
    public int Id { get; set; }

    [Required]
    public string Name { get; set; }

    [Range(1800, 2100)]
    public int BirthYear { get; set; }
}
