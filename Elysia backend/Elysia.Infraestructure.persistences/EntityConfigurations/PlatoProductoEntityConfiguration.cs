using Elysia.Core.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Elysia.Infraestructure.persistences.EntityConfigurations
{
    public class PlatoProductoEntityConfiguration : IEntityTypeConfiguration<PlatoProducto>
    {
        public void Configure(EntityTypeBuilder<PlatoProducto> builder)
        {
            #region basic configuration
            builder.ToTable("PlatoProductos");
            builder.HasKey(x => x.Id);
            #endregion


            #region property configuration
            builder.Property(x => x.Cantidad).IsRequired();
            #endregion


            #region relationship configuration
            builder.HasOne(x => x.Producto)
                  .WithMany(c => c.PlatoProductos)
                  .HasForeignKey(x => x.PlatoId)
                  .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(x => x.Plato)
                   .WithMany(c => c.PlatoProductos)
                   .HasForeignKey(x => x.PlatoId)
                   .OnDelete(DeleteBehavior.NoAction);
            #endregion
        }
    }
}
