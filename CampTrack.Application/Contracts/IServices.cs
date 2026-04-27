using CampTrack.Application.DTOs;

namespace CampTrack.Application.Contracts;

public interface IEquipoService
{
    Task<IEnumerable<EquipoDto>> GetAllAsync();
    Task<IEnumerable<EquipoDto>> GetDisponiblesAsync();
    Task<IEnumerable<EquipoDto>> SearchAsync(string nombre);
    Task<EquipoDto?> GetByIdAsync(int id);
    Task<EquipoDto> CreateAsync(CreateEquipoDto dto);
    Task<EquipoDto?> UpdateAsync(int id, UpdateEquipoDto dto);
    Task<bool> DeleteAsync(int id);
}

public interface ICategoriaService
{
    Task<IEnumerable<CategoriaDto>> GetAllAsync();
    Task<CategoriaDto?> GetByIdAsync(int id);
    Task<CategoriaDto> CreateAsync(CreateCategoriaDto dto);
    Task<CategoriaDto?> UpdateAsync(int id, CreateCategoriaDto dto);
    Task<bool> DeleteAsync(int id);
}

public interface IPrestamoService
{
    Task<IEnumerable<PrestamoDto>> GetAllAsync();
    Task<IEnumerable<PrestamoDto>> GetActivosAsync();
    Task<PrestamoDto?> GetByIdAsync(int id);
    Task<PrestamoDto> CreateAsync(CreatePrestamoDto dto);
    Task<PrestamoDto?> DevolverAsync(int id);
    Task<bool> DeleteAsync(int id);
}

public interface IRevisionService
{
    Task<IEnumerable<RevisionDto>> GetAllAsync();
    Task<IEnumerable<RevisionDto>> GetByEquipoAsync(int equipoId);
    Task<RevisionDto?> GetByIdAsync(int id);
    Task<RevisionDto> CreateAsync(CreateRevisionDto dto);
    Task<bool> DeleteAsync(int id);
}
