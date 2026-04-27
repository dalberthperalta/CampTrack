namespace CampTrack.Domain.Entities;

public class Revision
{
    public int Id { get; set; }
    public string Tecnico { get; set; } = string.Empty;
    public DateTime Fecha { get; set; } = DateTime.Now;
    public string? DescripcionDanos { get; set; }
    public string NivelDano { get; set; } = "ninguno";
    public decimal CostoReparacion { get; set; }
    public string Resultado { get; set; } = "aprobado";
    public bool IsActive { get; set; } = true;

    public int EquipoId { get; set; }
    public Equipo Equipo { get; set; } = null!;
}
