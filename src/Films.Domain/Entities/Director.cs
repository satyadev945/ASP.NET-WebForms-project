namespace Films.Domain.Entities;

/// <summary>
/// Represents a director entity
/// </summary>
public class Director
{
    public int Id { get; set; }

    public string Name { get; set; } = string.Empty;

    public string? Description { get; set; }

    public DateTime? BirthDate { get; set; }

    public int? SexId { get; set; }

    public string? ImageUrl { get; set; }

    public DateTime CreatedDate { get; set; } = DateTime.UtcNow;

    public DateTime? ModifiedDate { get; set; }

    public bool IsActive { get; set; } = true;

    public string CreatedBy { get; set; } = string.Empty;

    public string? ModifiedBy { get; set; }

    // Navigation properties
    public virtual Sex? Sex { get; set; }
    public virtual ICollection<RefDAF> RefDAFs { get; set; } = new List<RefDAF>();
}