using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using TiendaDeportes.Domain;
using TiendaDeportes.Infrastructure.Context;

public abstract class BaseRepository<TEntity> where TEntity : BaseEntity
{
    protected readonly TiendaContext _context;
    protected readonly DbSet<TEntity> _entities;

    public BaseRepository(TiendaContext context)
    {
        _context = context;
        _entities = context.Set<TEntity>();
    }

    public virtual async Task<List<TEntity>> GetAllAsync() =>
        await _entities.ToListAsync();

    public virtual async Task<TEntity> GetByIdAsync(int id) =>
        await _entities.FindAsync(id);

    public virtual async Task AddAsync(TEntity entity)
    {
        await _entities.AddAsync(entity);
        await _context.SaveChangesAsync();
    }

    public virtual async Task UpdateAsync(TEntity entity)
    {
        _entities.Update(entity);
        await _context.SaveChangesAsync();
    }

    public virtual async Task DeleteAsync(int id)
    {
        var entity = await _entities.FindAsync(id);
        if (entity != null)
        {
            _entities.Remove(entity);
            await _context.SaveChangesAsync();
        }
    }
}