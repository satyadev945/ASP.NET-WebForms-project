namespace Films.Domain.Entities;

public class Sex
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;

    // Navigation properties
    public ICollection<Actor> Actors { get; set; } = new List<Actor>();
}