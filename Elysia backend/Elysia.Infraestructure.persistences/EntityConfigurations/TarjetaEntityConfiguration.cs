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
    public class TarjetaEntityConfiguration : IEntityTypeConfiguration<Tarjeta>
    {
        public void Configure(EntityTypeBuilder<Tarjeta> builder)
        {
            #region basic configuration
            builder.ToTable("Tarjetas");
            builder.HasKey(x => x.Id);
            #endregion



            #region property configuration
            builder.Property(x => x.NombreTitular).IsRequired().HasMaxLength(256);
            builder.Property(x => x.NumeroTarjeta).IsRequired().HasMaxLength(256);
            builder.Property(x => x.Tipo).IsRequired().HasMaxLength(256);
            builder.Property(x => x.CVV).IsRequired();
            builder.Property(x => x.MesVencimiento).IsRequired();
            builder.Property(x => x.AnioVencimiento).IsRequired();
            builder.Property(x => x.UsuarioId).IsRequired();
            #endregion



            #region relationship configuration

            #endregion
        }
    }
}
