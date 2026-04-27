using CampTrack.Domain.Entities;

namespace CampTrack.Application.Contracts;

public interface IEquipoRepository : IGenericRepository<Equipo>
{
    Task<IEnumerable<Equipo>> GetDisponiblesAsync();
    Task<IEnumerable<Equipo>> GetByCategoriaAsync(int categoriaId);
    Task<IEnumerable<Equipo>> SearchByNombreAsync(string nombre);
    Task<Equipo?> GetWithDetailsAsync(int id);
}

public interface ICategoriaRepository : IGenericRepository<Categoria>
{
    Task<IEnumerable<Categoria>> GetActivasAsync();
}

public interface IPrestamoRepository : IGenericRepository<Prestamo>
{
    Task<IEnumerable<Prestamo>> GetActivosAsync();
    Task<IEnumerable<Prestamo>> GetByEquipoAsync(int equipoId);
}

public interface IRevisionRepository : IGenericRepository<Revision>
{
    Task<IEnumerable<Revision>> GetByEquipoAsync(int equipoId);
}

public interface IUnitOfWork : IDisposable
{
    IEquipoRepository Equipos { get; }
    ICategoriaRepository Categorias { get; }
    IPrestamoRepository Prestamos { get; }
    IRevisionRepository Revisiones { get; }
    Task<int> SaveChangesAsync();
}
