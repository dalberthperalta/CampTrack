using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TiendaDeportes.Domain.core;

namespace TiendaDeportes.Domain.Entities
{
    public class Producto : BaseEntity
    {
        public string Nombre { get; set; }
        public int MarcaId { get; set; }
        public Marca Marca { get; set; }
        public decimal Precio { get; set; }
        public int Stock { get; set; }
    }
}
