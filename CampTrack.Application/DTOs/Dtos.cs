namespace CampTrack.Application.DTOs;

public class CategoriaDto
{
    public int Id { get; set; }
    public string Nombre { get; set; } = string.Empty;
    public string? Descripcion { get; set; }
    public bool IsActive { get; set; }
}

public class CreateCategoriaDto
{
    public string Nombre { get; set; } = string.Empty;
    public string? Descripcion { get; set; }
}

public class EquipoDto
{
    public int Id { get; set; }
    public string Nombre { get; set; } = string.Empty;
    public string? Descripcion { get; set; }
    public string? CodigoSerial { get; set; }
    public decimal Costo { get; set; }
    public string Estado { get; set; } = string.Empty;
    public bool Disponible { get; set; }
    public int CategoriaId { get; set; }
    public string? CategoriaNombre { get; set; }
}

public class CreateEquipoDto
{
    public string Nombre { get; set; } = string.Empty;
    public string? Descripcion { get; set; }
    public string? CodigoSerial { get; set; }
    public decimal Costo { get; set; }
    public int CategoriaId { get; set; }
}

public class UpdateEquipoDto
{
    public string Nombre { get; set; } = string.Empty;
    public string? Descripcion { get; set; }
    public string? CodigoSerial { get; set; }
    public decimal Costo { get; set; }
    public string Estado { get; set; } = string.Empty;
    public bool Disponible { get; set; }
    public int CategoriaId { get; set; }
}

public class PrestamoDto
{
    public int Id { get; set; }
    public string UsuarioNombre { get; set; } = string.Empty;
    public string? UsuarioContacto { get; set; }
    public DateTime FechaSalida { get; set; }
    public DateTime? FechaRetornoEsperada { get; set; }
    public DateTime? FechaRetornoReal { get; set; }
    public string Estado { get; set; } = string.Empty;
    public string? Observaciones { get; set; }
    public int EquipoId { get; set; }
    public string? EquipoNombre { get; set; }
}

public class CreatePrestamoDto
{
    public string UsuarioNombre { get; set; } = string.Empty;
    public string? UsuarioContacto { get; set; }
    public DateTime? FechaRetornoEsperada { get; set; }
    public string? Observaciones { get; set; }
    public int EquipoId { get; set; }
}

public class RevisionDto
{
    public int Id { get; set; }
    public string Tecnico { get; set; } = string.Empty;
    public DateTime Fecha { get; set; }
    public string? DescripcionDanos { get; set; }
    public string NivelDano { get; set; } = string.Empty;
    public decimal CostoReparacion { get; set; }
    public string Resultado { get; set; } = string.Empty;
    public int EquipoId { get; set; }
    public string? EquipoNombre { get; set; }
}

public class CreateRevisionDto
{
    public string Tecnico { get; set; } = string.Empty;
    public string? DescripcionDanos { get; set; }
    public string NivelDano { get; set; } = "ninguno";
    public decimal CostoReparacion { get; set; }
    public string Resultado { get; set; } = "aprobado";
    public int EquipoId { get; set; }
}
