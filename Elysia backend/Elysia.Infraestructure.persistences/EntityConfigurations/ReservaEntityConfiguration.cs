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
    public class ReservaEntityConfiguration : IEntityTypeConfiguration<Reserva>
    {
        public void Configure(EntityTypeBuilder<Reserva> builder)
        {
            #region basic configuration
            builder.ToTable("Reservas");
            builder.HasKey(x => x.Id);
            #endregion


            #region property configuration
            builder.Property(x => x.NombreCliente).IsRequired().HasMaxLength(250);
            builder.Property(x => x.DNICliente).IsRequired().HasMaxLength(250);
            builder.Property(x => x.Estado).IsRequired();
            builder.Property(x => x.CantidadPersona).IsRequired();
            builder.Property(x => x.FechaReserva).IsRequired();
            #endregion


            #region relationship configuration
            builder.HasOne(x => x.Mesa)
                   .WithMany(x => x.Reservas)
                   .HasForeignKey(x => x.MesaId)
                   .OnDelete(DeleteBehavior.Restrict);

            #endregion
        }
    }
}
