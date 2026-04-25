using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TiendaDeportes.Infrastructure.Context;

namespace TiendaDeportes.Infrastructure.Repositories
{
    public class MarcaRepository : BaseRepository<Marca>, IMarcaRepository
    {
        public MarcaRepository(TiendaContext context) : base(context) { }

        // Métodos específicos, si son necesarios
    }
}
