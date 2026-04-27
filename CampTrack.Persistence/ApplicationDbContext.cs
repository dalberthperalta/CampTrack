using CampTrack.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace CampTrack.Persistence;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

    public DbSet<Categoria> Categorias => Set<Categoria>();
    public DbSet<Equipo> Equipos => Set<Equipo>();
    public DbSet<Prestamo> Prestamos => Set<Prestamo>();
    public DbSet<Revision> Revisiones => Set<Revision>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Categoria>(e =>
        {
            e.HasKey(x => x.Id);
            e.Property(x => x.Nombre).IsRequired().HasMaxLength(100);
            e.Property(x => x.Descripcion).HasMaxLength(500);
        });

        modelBuilder.Entity<Equipo>(e =>
        {
            e.HasKey(x => x.Id);
            e.Property(x => x.Nombre).IsRequired().HasMaxLength(150);
            e.Property(x => x.CodigoSerial).HasMaxLength(50);
            e.Property(x => x.Estado).HasMaxLength(30);
            e.Property(x => x.Costo).HasColumnType("decimal(18,2)");
            e.HasOne(x => x.Categoria)
             .WithMany(x => x.Equipos)
             .HasForeignKey(x => x.CategoriaId)
             .OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<Prestamo>(e =>
        {
            e.HasKey(x => x.Id);
            e.Property(x => x.UsuarioNombre).IsRequired().HasMaxLength(150);
            e.Property(x => x.Estado).HasMaxLength(20);
            e.HasOne(x => x.Equipo)
             .WithMany(x => x.Prestamos)
             .HasForeignKey(x => x.EquipoId)
             .OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<Revision>(e =>
        {
            e.HasKey(x => x.Id);
            e.Property(x => x.Tecnico).IsRequired().HasMaxLength(150);
            e.Property(x => x.NivelDano).HasMaxLength(20);
            e.Property(x => x.Resultado).HasMaxLength(30);
            e.Property(x => x.CostoReparacion).HasColumnType("decimal(18,2)");
            e.HasOne(x => x.Equipo)
             .WithMany(x => x.Revisiones)
             .HasForeignKey(x => x.EquipoId)
             .OnDelete(DeleteBehavior.Restrict);
        });
    }
}
