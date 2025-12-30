namespace Films.Domain.Entities;

public class Film
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
    public DateTime? ModifiedDate { get; set; }
    public bool IsActive { get; set; } = true;
    public string CreatedBy { get; set; } = string.Empty;
    public string? ModifiedBy { get; set; }
    public string? Genre { get; set; }
    public int? Year { get; set; }
    public string? Director { get; set; }
    public string? Country { get; set; }
    public int? Duration { get; set; }
    public decimal? Rating { get; set; }

    // Navigation properties
    public virtual ICollection<RefAF> FilmActors { get; set; } = new List<RefAF>();
    public virtual ICollection<RefDAF> FilmDirectors { get; set; } = new List<RefDAF>();
}