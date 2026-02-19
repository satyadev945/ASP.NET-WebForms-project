namespace Films.Domain.Entities;

/// <summary>
/// Represents a user type entity in the system
/// </summary>
public class TypeUser
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }

    // Navigation properties
    public ICollection<User> Users { get; set; } = new List<User>();
}
