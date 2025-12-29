namespace Films.Domain.Entities;

public class RefDAF
{
    public int Id { get; set; }
    public int DirectorId { get; set; }
    public int FilmId { get; set; }
    public DateTime CreatedDate { get; set; }
    public bool IsActive { get; set; }
    public string CreatedBy { get; set; } = string.Empty;

    public virtual DirectedBy Director { get; set; } = null!;
    public virtual Film Film { get; set; } = null!;
}
