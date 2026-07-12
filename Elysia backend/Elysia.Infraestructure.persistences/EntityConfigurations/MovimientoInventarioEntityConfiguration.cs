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
    public class MovimientoInventarioEntityConfiguration : IEntityTypeConfiguration<MovimientoInventario>
    {
        public void Configure(EntityTypeBuilder<MovimientoInventario> builder)
        {
            #region basic configuration
            builder.ToTable("MovimientoInventarios");
            builder.HasKey(x => x.Id);
            #endregion

            #region property configuration
            builder.Property(x => x.Cantidad).IsRequired().HasPrecision(20,2);
            builder.Property(x => x.TipoMovimiento).IsRequired();

            #endregion


            #region relationship configuration
            builder.HasOne(x => x.Producto)
                  .WithMany(c => c.Movimientos)
                  .HasForeignKey(x => x.ProductoId)
                  .OnDelete(DeleteBehavior.Cascade);
            #endregion
        }
    }
}
