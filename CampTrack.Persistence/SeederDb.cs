using CampTrack.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace CampTrack.Persistence;

public static class SeederDb
{
    public static async Task SeedAsync(ApplicationDbContext context)
    {

        await context.Database.MigrateAsync();

        if (await context.Categorias.AnyAsync()) return;

        var categorias = new List<Categoria>
        {
            new() { Nombre = "Tiendas y Refugios",    Descripcion = "Carpas, tarps y refugios de emergencia" },
            new() { Nombre = "Escalada",               Descripcion = "Cuerdas, arneses, mosquetones y cascos" },
            new() { Nombre = "Supervivencia",          Descripcion = "Navajas, encendedores, brújulas y kits de emergencia" },
            new() { Nombre = "Iluminación",            Descripcion = "Linternas, lámparas y luces de emergencia" },
            new() { Nombre = "Hidratación",            Descripcion = "Filtros de agua, cantimploras y termos" },
        };
        await context.Categorias.AddRangeAsync(categorias);
        await context.SaveChangesAsync();

        var equipos = new List<Equipo>
        {
            new() { Nombre = "Carpa 4 personas Coleman", CodigoSerial = "CT-001", Costo = 120m, CategoriaId = 1, Estado = "disponible", Disponible = true },
            new() { Nombre = "Arnés Black Diamond",      CodigoSerial = "ES-001", Costo = 85m,  CategoriaId = 2, Estado = "disponible", Disponible = true },
            new() { Nombre = "Kit Supervivencia Pro",    CodigoSerial = "SV-001", Costo = 45m,  CategoriaId = 3, Estado = "disponible", Disponible = true },
            new() { Nombre = "Linterna Frontal Petzl",   CodigoSerial = "IL-001", Costo = 35m,  CategoriaId = 4, Estado = "disponible", Disponible = true },
            new() { Nombre = "Filtro LifeStraw",         CodigoSerial = "HI-001", Costo = 30m,  CategoriaId = 5, Estado = "disponible", Disponible = true },
        };
        await context.Equipos.AddRangeAsync(equipos);
        await context.SaveChangesAsync();
    }
}
