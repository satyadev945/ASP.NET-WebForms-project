namespace Films.Domain.Entities;

public class Actor
{
    public int Id { get; set; }
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public DateTime? DateOfBirth { get; set; }
    public int? SexId { get; set; }

    // Navigation properties
    public Sex? Sex { get; set; }
    public ICollection<RefAF> FilmReferences { get; set; } = new List<RefAF>();
}