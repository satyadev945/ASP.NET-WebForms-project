namespace Films.Domain.Entities;

public class RefAF
{
    public int Id { get; set; }
    public int ActorId { get; set; }
    public int FilmId { get; set; }

    // Navigation properties
    public Actor Actor { get; set; } = null!;
    public Film Film { get; set; } = null!;
}