namespace Films.Domain.Entities;

public class RefAF
{
    public int Id { get; set; }
    public int? ActorId { get; set; }
    public int? FilmId { get; set; }
    public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
    public DateTime? ModifiedDate { get; set; }
    public bool IsActive { get; set; } = true;
    public string CreatedBy { get; set; } = string.Empty;
    public string? ModifiedBy { get; set; }

    // Navigation properties
    public virtual Actor? Actor { get; set; }
    public virtual Film? Film { get; set; }
}