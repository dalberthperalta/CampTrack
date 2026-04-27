namespace CampTrack.Domain.Entities;

public class Equipo
{
    public int Id { get; set; }
    public string Nombre { get; set; } = string.Empty;
    public string? Descripcion { get; set; }
    public string? CodigoSerial { get; set; }
    public decimal Costo { get; set; }
    public string Estado { get; set; } = "disponible";
    public bool Disponible { get; set; } = true;
    public bool IsActive { get; set; } = true;

    public int CategoriaId { get; set; }
    public Categoria Categoria { get; set; } = null!;

    public ICollection<Prestamo> Prestamos { get; set; } = new List<Prestamo>();
    public ICollection<Revision> Revisiones { get; set; } = new List<Revision>();
}
