namespace Films.Domain.Entities;

public class Right
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;

    // Navigation properties
    public ICollection<UserRight> UserRights { get; set; } = new List<UserRight>();
}