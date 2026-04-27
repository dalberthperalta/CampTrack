using CampTrack.Application.Contracts;
using CampTrack.Domain.Entities;
using CampTrack.Persistence;
using Microsoft.EntityFrameworkCore;

namespace CampTrack.Infrastructure.Repositories;

public class GenericRepository<T> : IGenericRepository<T> where T : class
{
    protected readonly ApplicationDbContext _context;
    protected readonly DbSet<T> _dbSet;

    public GenericRepository(ApplicationDbContext context)
    {
        _context = context;
        _dbSet = context.Set<T>();
    }

    public virtual async Task<IEnumerable<T>> GetAllAsync() => await _dbSet.ToListAsync();
    public async Task<T?> GetByIdAsync(int id) => await _dbSet.FindAsync(id);
    public async Task AddAsync(T entity) => await _dbSet.AddAsync(entity);
    public void Update(T entity) => _dbSet.Update(entity);
    public void Delete(T entity) => _dbSet.Remove(entity);
}

public class EquipoRepository : GenericRepository<Equipo>, IEquipoRepository
{
    public EquipoRepository(ApplicationDbContext context) : base(context) { }

    public async Task<IEnumerable<Equipo>> GetDisponiblesAsync()
        => await _context.Equipos.Include(e => e.Categoria)
                                  .Where(e => e.Disponible && e.IsActive)
                                  .ToListAsync();

    public async Task<IEnumerable<Equipo>> GetByCategoriaAsync(int categoriaId)
        => await _context.Equipos.Include(e => e.Categoria)
                                  .Where(e => e.CategoriaId == categoriaId && e.IsActive)
                                  .ToListAsync();

    public async Task<IEnumerable<Equipo>> SearchByNombreAsync(string nombre)
        => await _context.Equipos.Include(e => e.Categoria)
                                  .Where(e => e.Nombre.Contains(nombre) && e.IsActive)
                                  .ToListAsync();

    public async Task<Equipo?> GetWithDetailsAsync(int id)
        => await _context.Equipos.Include(e => e.Categoria)
                                  .Include(e => e.Prestamos)
                                  .Include(e => e.Revisiones)
                                  .FirstOrDefaultAsync(e => e.Id == id);

    public override async Task<IEnumerable<Equipo>> GetAllAsync()
        => await _context.Equipos.Include(e => e.Categoria)
                                  .Where(e => e.IsActive)
                                  .ToListAsync();
}

public class CategoriaRepository : GenericRepository<Categoria>, ICategoriaRepository
{
    public CategoriaRepository(ApplicationDbContext context) : base(context) { }

    public async Task<IEnumerable<Categoria>> GetActivasAsync()
        => await _context.Categorias.Where(c => c.IsActive).ToListAsync();

    public override async Task<IEnumerable<Categoria>> GetAllAsync()
        => await _context.Categorias.Where(c => c.IsActive).ToListAsync();
}

public class PrestamoRepository : GenericRepository<Prestamo>, IPrestamoRepository
{
    public PrestamoRepository(ApplicationDbContext context) : base(context) { }

    public async Task<IEnumerable<Prestamo>> GetActivosAsync()
        => await _context.Prestamos.Include(p => p.Equipo)
                                    .Where(p => p.Estado == "activo" && p.IsActive)
                                    .ToListAsync();

    public async Task<IEnumerable<Prestamo>> GetByEquipoAsync(int equipoId)
        => await _context.Prestamos.Include(p => p.Equipo)
                                    .Where(p => p.EquipoId == equipoId && p.IsActive)
                                    .ToListAsync();

    public override async Task<IEnumerable<Prestamo>> GetAllAsync()
        => await _context.Prestamos.Include(p => p.Equipo)
                                    .Where(p => p.IsActive)
                                    .ToListAsync();
}

public class RevisionRepository : GenericRepository<Revision>, IRevisionRepository
{
    public RevisionRepository(ApplicationDbContext context) : base(context) { }

    public async Task<IEnumerable<Revision>> GetByEquipoAsync(int equipoId)
        => await _context.Revisiones.Include(r => r.Equipo)
                                     .Where(r => r.EquipoId == equipoId && r.IsActive)
                                     .ToListAsync();

    public override async Task<IEnumerable<Revision>> GetAllAsync()
        => await _context.Revisiones.Include(r => r.Equipo)
                                     .Where(r => r.IsActive)
                                     .ToListAsync();
}

public class UnitOfWork : IUnitOfWork
{
    private readonly ApplicationDbContext _context;

    public IEquipoRepository    Equipos    { get; }
    public ICategoriaRepository Categorias { get; }
    public IPrestamoRepository  Prestamos  { get; }
    public IRevisionRepository  Revisiones { get; }

    public UnitOfWork(ApplicationDbContext context)
    {
        _context   = context;
        Equipos    = new EquipoRepository(context);
        Categorias = new CategoriaRepository(context);
        Prestamos  = new PrestamoRepository(context);
        Revisiones = new RevisionRepository(context);
    }

    public async Task<int> SaveChangesAsync() => await _context.SaveChangesAsync();
    public void Dispose() => _context.Dispose();
}
