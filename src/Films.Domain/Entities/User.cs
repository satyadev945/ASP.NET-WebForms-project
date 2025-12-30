namespace Films.Domain.Entities;

public class User
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
    public DateTime? ModifiedDate { get; set; }
    public bool IsActive { get; set; } = true;
    public string CreatedBy { get; set; } = string.Empty;
    public string? ModifiedBy { get; set; }
    public string Username { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string PasswordHash { get; set; } = string.Empty;
    public int? TypeUserId { get; set; }
    public int? SexId { get; set; }

    // Navigation properties
    public virtual TypeUser? TypeUser { get; set; }
    public virtual Sex? Sex { get; set; }
    public virtual ICollection<UserRight> UserRights { get; set; } = new List<UserRight>();
}