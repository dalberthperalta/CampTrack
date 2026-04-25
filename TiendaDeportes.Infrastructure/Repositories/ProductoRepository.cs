using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TiendaDeportes.Infrastructure.Context;

namespace TiendaDeportes.Infrastructure.Repositories
{
    public class ProductoRepository : BaseRepository<Producto>, IProductoRepository
    {
        public ProductoRepository(TiendaContext context) : base(context) { }

        // Puedes agregar métodos específicos, si son necesarios
    }
}
