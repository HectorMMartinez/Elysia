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
    public class PedidoEntityConfiguration : IEntityTypeConfiguration<Pedido>
    {
        public void Configure(EntityTypeBuilder<Pedido> builder)
        {
            #region basic configuration
            builder.ToTable("Pedidos");
            builder.HasKey( x => x.Id);
            #endregion



            #region property configuration
            builder.Property(x => x.IdMesa).IsRequired();
            builder.Property(x => x.Estado).IsRequired();
            builder.Property(x => x.FechaCreacion).IsRequired();    
            builder.Property( x => x.IdPropietario).IsRequired();
            #endregion


            #region relationship configuration
            builder.HasOne(x => x.Mesa)
                 .WithMany(c => c.Pedidos)
                 .HasForeignKey(x => x.IdMesa)
                 .OnDelete(DeleteBehavior.Cascade);
                   
            #endregion



        }




    }
}
