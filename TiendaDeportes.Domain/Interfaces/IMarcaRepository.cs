using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TiendaDeportes.Domain.Entities;

namespace TiendaDeportes.Domain.Interfaces
{
    public interface IMarcaRepository
    {
        Task<List<Marca>> GetAllAsync();
        Task<Marca> GetByIdAsync(int id);
        Task AddAsync(Marca marca);
        Task UpdateAsync(Marca marca);
        Task DeleteAsync(int id);
    }

}
