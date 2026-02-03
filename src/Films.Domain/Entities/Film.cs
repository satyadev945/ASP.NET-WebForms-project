namespace Films.Domain.Entities;

public class Film
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public int? Year { get; set; }
    public string? Description { get; set; }

    // Navigation properties
    public ICollection<RefAF> ActorReferences { get; set; } = new List<RefAF>();
    public ICollection<RefDAF> DirectorReferences { get; set; } = new List<RefDAF>();
}