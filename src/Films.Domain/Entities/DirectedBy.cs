namespace Films.Domain.Entities;

public class DirectedBy
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;

    // Navigation properties
    public ICollection<RefDAF> FilmReferences { get; set; } = new List<RefDAF>();
}