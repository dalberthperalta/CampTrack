using AutoMapper;
using CampTrack.Application.Contracts;
using CampTrack.Application.DTOs;
using CampTrack.Domain.Entities;

namespace CampTrack.Application.Services;

public class CategoriaService : ICategoriaService
{
    private readonly IUnitOfWork _uow;
    private readonly IMapper _mapper;

    public CategoriaService(IUnitOfWork uow, IMapper mapper)
    {
        _uow = uow;
        _mapper = mapper;
    }

    public async Task<IEnumerable<CategoriaDto>> GetAllAsync()
    {
        var items = await _uow.Categorias.GetAllAsync();
        return _mapper.Map<IEnumerable<CategoriaDto>>(items);
    }

    public async Task<CategoriaDto?> GetByIdAsync(int id)
    {
        var item = await _uow.Categorias.GetByIdAsync(id);
        return item == null ? null : _mapper.Map<CategoriaDto>(item);
    }

    public async Task<CategoriaDto> CreateAsync(CreateCategoriaDto dto)
    {
        var entity = _mapper.Map<Categoria>(dto);
        await _uow.Categorias.AddAsync(entity);
        await _uow.SaveChangesAsync();
        return _mapper.Map<CategoriaDto>(entity);
    }

    public async Task<CategoriaDto?> UpdateAsync(int id, CreateCategoriaDto dto)
    {
        var entity = await _uow.Categorias.GetByIdAsync(id);
        if (entity == null) return null;
        _mapper.Map(dto, entity);
        _uow.Categorias.Update(entity);
        await _uow.SaveChangesAsync();
        return _mapper.Map<CategoriaDto>(entity);
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var entity = await _uow.Categorias.GetByIdAsync(id);
        if (entity == null) return false;
        _uow.Categorias.Delete(entity);
        await _uow.SaveChangesAsync();
        return true;
    }
}

public class EquipoService : IEquipoService
{
    private readonly IUnitOfWork _uow;
    private readonly IMapper _mapper;

    public EquipoService(IUnitOfWork uow, IMapper mapper)
    {
        _uow = uow;
        _mapper = mapper;
    }

    public async Task<IEnumerable<EquipoDto>> GetAllAsync()
    {
        var items = await _uow.Equipos.GetAllAsync();
        return _mapper.Map<IEnumerable<EquipoDto>>(items);
    }

    public async Task<IEnumerable<EquipoDto>> GetDisponiblesAsync()
    {
        var items = await _uow.Equipos.GetDisponiblesAsync();
        return _mapper.Map<IEnumerable<EquipoDto>>(items);
    }

    public async Task<IEnumerable<EquipoDto>> SearchAsync(string nombre)
    {
        var items = await _uow.Equipos.SearchByNombreAsync(nombre);
        return _mapper.Map<IEnumerable<EquipoDto>>(items);
    }

    public async Task<EquipoDto?> GetByIdAsync(int id)
    {
        var item = await _uow.Equipos.GetWithDetailsAsync(id);
        return item == null ? null : _mapper.Map<EquipoDto>(item);
    }

    public async Task<EquipoDto> CreateAsync(CreateEquipoDto dto)
    {
        var entity = _mapper.Map<Equipo>(dto);
        entity.Estado = "disponible";
        entity.Disponible = true;
        await _uow.Equipos.AddAsync(entity);
        await _uow.SaveChangesAsync();
        return _mapper.Map<EquipoDto>(entity);
    }

    public async Task<EquipoDto?> UpdateAsync(int id, UpdateEquipoDto dto)
    {
        var entity = await _uow.Equipos.GetByIdAsync(id);
        if (entity == null) return null;
        _mapper.Map(dto, entity);
        _uow.Equipos.Update(entity);
        await _uow.SaveChangesAsync();
        return _mapper.Map<EquipoDto>(entity);
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var entity = await _uow.Equipos.GetByIdAsync(id);
        if (entity == null) return false;
        _uow.Equipos.Delete(entity);
        await _uow.SaveChangesAsync();
        return true;
    }
}

public class PrestamoService : IPrestamoService
{
    private readonly IUnitOfWork _uow;
    private readonly IMapper _mapper;

    public PrestamoService(IUnitOfWork uow, IMapper mapper)
    {
        _uow = uow;
        _mapper = mapper;
    }

    public async Task<IEnumerable<PrestamoDto>> GetAllAsync()
    {
        var items = await _uow.Prestamos.GetAllAsync();
        return _mapper.Map<IEnumerable<PrestamoDto>>(items);
    }

    public async Task<IEnumerable<PrestamoDto>> GetActivosAsync()
    {
        var items = await _uow.Prestamos.GetActivosAsync();
        return _mapper.Map<IEnumerable<PrestamoDto>>(items);
    }

    public async Task<PrestamoDto?> GetByIdAsync(int id)
    {
        var item = await _uow.Prestamos.GetByIdAsync(id);
        return item == null ? null : _mapper.Map<PrestamoDto>(item);
    }

    public async Task<PrestamoDto> CreateAsync(CreatePrestamoDto dto)
    {

        var equipo = await _uow.Equipos.GetByIdAsync(dto.EquipoId);
        if (equipo == null) throw new Exception("Equipo no encontrado.");
        if (!equipo.Disponible) throw new Exception("El equipo no está disponible.");

        var entity = _mapper.Map<Prestamo>(dto);
        entity.FechaSalida = DateTime.Now;
        entity.Estado = "activo";
        await _uow.Prestamos.AddAsync(entity);

        equipo.Disponible = false;
        equipo.Estado = "prestado";
        _uow.Equipos.Update(equipo);

        await _uow.SaveChangesAsync();
        return _mapper.Map<PrestamoDto>(entity);
    }

    public async Task<PrestamoDto?> DevolverAsync(int id)
    {
        var entity = await _uow.Prestamos.GetByIdAsync(id);
        if (entity == null) return null;

        entity.FechaRetornoReal = DateTime.Now;
        entity.Estado = "devuelto";
        _uow.Prestamos.Update(entity);

        var equipo = await _uow.Equipos.GetByIdAsync(entity.EquipoId);
        if (equipo != null)
        {
            equipo.Disponible = true;
            equipo.Estado = "disponible";
            _uow.Equipos.Update(equipo);
        }

        await _uow.SaveChangesAsync();
        return _mapper.Map<PrestamoDto>(entity);
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var entity = await _uow.Prestamos.GetByIdAsync(id);
        if (entity == null) return false;
        _uow.Prestamos.Delete(entity);
        await _uow.SaveChangesAsync();
        return true;
    }
}

public class RevisionService : IRevisionService
{
    private readonly IUnitOfWork _uow;
    private readonly IMapper _mapper;

    public RevisionService(IUnitOfWork uow, IMapper mapper)
    {
        _uow = uow;
        _mapper = mapper;
    }

    public async Task<IEnumerable<RevisionDto>> GetAllAsync()
    {
        var items = await _uow.Revisiones.GetAllAsync();
        return _mapper.Map<IEnumerable<RevisionDto>>(items);
    }

    public async Task<IEnumerable<RevisionDto>> GetByEquipoAsync(int equipoId)
    {
        var items = await _uow.Revisiones.GetByEquipoAsync(equipoId);
        return _mapper.Map<IEnumerable<RevisionDto>>(items);
    }

    public async Task<RevisionDto?> GetByIdAsync(int id)
    {
        var item = await _uow.Revisiones.GetByIdAsync(id);
        return item == null ? null : _mapper.Map<RevisionDto>(item);
    }

    public async Task<RevisionDto> CreateAsync(CreateRevisionDto dto)
    {
        var entity = _mapper.Map<Revision>(dto);
        entity.Fecha = DateTime.Now;
        await _uow.Revisiones.AddAsync(entity);

        var equipo = await _uow.Equipos.GetByIdAsync(dto.EquipoId);
        if (equipo != null)
        {
            equipo.Estado = dto.Resultado switch
            {
                "en reparacion" => "en mantenimiento",
                "dado de baja"  => "dañado",
                _               => "disponible"
            };
            equipo.Disponible = dto.Resultado == "aprobado";
            _uow.Equipos.Update(equipo);
        }

        await _uow.SaveChangesAsync();
        return _mapper.Map<RevisionDto>(entity);
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var entity = await _uow.Revisiones.GetByIdAsync(id);
        if (entity == null) return false;
        _uow.Revisiones.Delete(entity);
        await _uow.SaveChangesAsync();
        return true;
    }
}
