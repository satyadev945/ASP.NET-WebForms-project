namespace Films.Domain.Entities;

public class RefDAF
{
    public int Id { get; set; }
    public int DirectedById { get; set; }
    public int FilmId { get; set; }

    // Navigation properties
    public DirectedBy DirectedBy { get; set; } = null!;
    public Film Film { get; set; } = null!;
}