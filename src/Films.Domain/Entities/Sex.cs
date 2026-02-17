namespace Films.Domain.Entities;

/// <summary>
/// Represents a sex/gender entity in the system
/// </summary>
public class Sex
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;

    // Navigation properties
    public ICollection<Actor> Actors { get; set; } = new List<Actor>();
    public ICollection<Director> Directors { get; set; } = new List<Director>();
    public ICollection<User> Users { get; set; } = new List<User>();
}
