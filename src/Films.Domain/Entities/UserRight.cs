namespace Films.Domain.Entities;

/// <summary>
/// Represents a user-right relationship entity in the system
/// </summary>
public class UserRight
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public int RightId { get; set; }
    public DateTime GrantedDate { get; set; }

    // Navigation properties
    public User User { get; set; } = null!;
    public Right Right { get; set; } = null!;
}
