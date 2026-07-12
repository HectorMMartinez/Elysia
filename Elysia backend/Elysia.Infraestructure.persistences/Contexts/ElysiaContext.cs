using Elysia.Core.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System.Reflection;


namespace Elysia.Infraestructure.persistences.Contexts
{
    public class ElysiaContext : DbContext
    {

        public ElysiaContext(DbContextOptions<ElysiaContext> opt) : base(opt) { }


        public DbSet<Membresia> Membresias { get; set; }
        public DbSet<Tarjeta> Tarjetas { get; set; }
        public DbSet<Plan> Plans { get; set; }
        public DbSet<Plato> Platos { get; set; }
        public DbSet<PlatoMenu> PlatoMenus { get; set; }
        public DbSet<PlatoProducto> PlatoProductos { get; set; }
        public DbSet<Reserva> Reservas { get; set; }
        public DbSet<Pedido> Pedidos { get; set; }
        public DbSet<MovimientoInventario> MovimientoInventarios { get; set; }
        public DbSet<Mesa> Mesas { get; set; }
        public DbSet<Menu> Menus { get; set; }
        public DbSet<DetallesPedido> DetallesPedidos { get; set; }
        public DbSet<CategoriaPlato> CategoriaPlatos { get; set; }
        public DbSet<Producto> Productos { get; set; }



        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

        }



    }
}
