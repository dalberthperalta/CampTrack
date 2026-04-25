using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TiendaDeportes.Domain.Entities;

namespace TiendaDeportes.Infrastructure.Context
{

    public class TiendaContext : DbContext
    {
        public DbSet<Producto> Productos { get; set; }
        public DbSet<Marca> Marcas { get; set; }

        public TiendaContext(DbContextOptions<TiendaContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Marca>()
                .HasMany(m => m.Productos)
                .WithOne(p => p.Marca)
                .HasForeignKey(p => p.MarcaId);
        }
    }
}
