namespace Films.Domain.Entities;

/// <summary>
/// Represents a many-to-many relationship between Directors and Films
/// </summary>
public class RefDAF
{
    public int Id { get; set; }

    public int DirectorId { get; set; }

    public int FilmId { get; set; }

    public DateTime CreatedDate { get; set; }

    public bool IsActive { get; set; }

    public DirectedBy? Director { get; set; }

    public Film? Film { get; set; }
}
