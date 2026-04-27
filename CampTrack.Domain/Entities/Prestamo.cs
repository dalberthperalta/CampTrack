namespace CampTrack.Domain.Entities;

public class Prestamo
{
    public int Id { get; set; }
    public string UsuarioNombre { get; set; } = string.Empty;
    public string? UsuarioContacto { get; set; }
    public DateTime FechaSalida { get; set; } = DateTime.Now;
    public DateTime? FechaRetornoEsperada { get; set; }
    public DateTime? FechaRetornoReal { get; set; }
    public string Estado { get; set; } = "activo";
    public string? Observaciones { get; set; }
    public bool IsActive { get; set; } = true;

    public int EquipoId { get; set; }
    public Equipo Equipo { get; set; } = null!;
}
