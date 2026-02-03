namespace Films.Domain.Entities;

public class UserRight
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public int RightId { get; set; }

    // Navigation properties
    public User User { get; set; } = null!;
    public Right Right { get; set; } = null!;
}