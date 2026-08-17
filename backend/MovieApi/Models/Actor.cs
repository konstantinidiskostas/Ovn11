namespace MovieApi.Models;

public class Actor
{
    public int Id { get; set; }
    public string Name { get; set; }
    public int BirthYear { get; set; }

    //N:M
    public List<MovieActor> MovieActors { get; set; }
}