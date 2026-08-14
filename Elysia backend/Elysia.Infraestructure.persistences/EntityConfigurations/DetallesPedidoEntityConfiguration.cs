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
    public class DetallesPedidoEntityConfiguration : IEntityTypeConfiguration<DetallesPedido>
    {
        public void Configure(EntityTypeBuilder<DetallesPedido> builder)
        {
            #region basic configuration
            builder.ToTable("DetallesPedidos");
            builder.HasKey("Id");
            #endregion


            #region basic configuration 
            builder.Property(x => x.PrecioUnitario).IsRequired().HasPrecision(20, 2);
            builder.Property(x => x.Cantidad).IsRequired();
            #endregion



            #region relationship configuration
            builder.HasOne(x => x.Pedido)
                  .WithMany(c => c.DetallesPedidos)
                  .HasForeignKey(x => x.PedidoId)
                  .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(x => x.Plato)
                   .WithMany(c => c.DetallesPedidos)
                   .HasForeignKey(x => x.PlatoId)
                   .OnDelete(DeleteBehavior.NoAction);
            #endregion
        }
    }
}
