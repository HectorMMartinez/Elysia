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
    public class MesaEntityConfiguration : IEntityTypeConfiguration<Mesa>
    {
        public void Configure(EntityTypeBuilder<Mesa> builder)
        {
            #region basic configuration
            builder.ToTable("Mesas");
            builder.HasKey(x => x.Id);
            #endregion


            #region property configuration
            builder.Property(x => x.Nombre).IsRequired().HasMaxLength(250);
            builder.Property(X => X.Descripcion).IsRequired().HasMaxLength(300);
            builder.Property(x => x.Estado).IsRequired();
            builder.Property(x => x.Capacidad).IsRequired();
            builder.Property(x => x.Imagen).IsRequired().HasMaxLength(int.MaxValue);
            builder.Property(x => x.Codigo).IsRequired();
            builder.Property(x => x.FechaCreacion).IsRequired();
            #endregion


            #region relationship configuration
            
            #endregion
        }
    }
}
