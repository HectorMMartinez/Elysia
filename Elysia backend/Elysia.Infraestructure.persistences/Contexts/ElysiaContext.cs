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


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

        }



    }
}
