namespace Films.Domain.Entities;

public class User
{
    public int Id { get; set; }
    public string Username { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public int? TypeUserId { get; set; }

    // Navigation properties
    public TypeUser? TypeUser { get; set; }
    public ICollection<UserRight> UserRights { get; set; } = new List<UserRight>();
}