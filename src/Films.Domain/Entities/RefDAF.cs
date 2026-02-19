namespace Films.Domain.Entities;

/// <summary>
/// Represents a director-film relationship entity in the system
/// </summary>
public class RefDAF
{
    public int Id { get; set; }
    public int DirectorId { get; set; }
    public int FilmId { get; set; }

    // Navigation properties
    public Director Director { get; set; } = null!;
    public Film Film { get; set; } = null!;
}
