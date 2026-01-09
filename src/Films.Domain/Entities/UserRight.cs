namespace Films.Domain.Entities;

/// <summary>
/// Represents a many-to-many relationship between Users and Rights
/// </summary>
public class UserRight
{
    public int Id { get; set; }

    public int UserId { get; set; }

    public int RightId { get; set; }

    public DateTime CreatedDate { get; set; }

    public bool IsActive { get; set; }

    public User? User { get; set; }

    public Right? Right { get; set; }
}
