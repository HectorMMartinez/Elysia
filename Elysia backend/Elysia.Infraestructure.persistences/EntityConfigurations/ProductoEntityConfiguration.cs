using Elysia.Core.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Elysia.Infraestructure.persistences.EntityConfigurations
{
    public class ProductoEntityConfiguration : IEntityTypeConfiguration<Producto>
    {
        public void Configure(EntityTypeBuilder<Producto> builder)
        {
            #region basic configuration
            builder.ToTable("Productos");
            builder.HasKey(x => x.Id);
            #endregion


            #region property configuration
            builder.Property(x => x.Nombre).IsRequired().HasMaxLength(250);
            builder.Property(x => x.Descripcion).IsRequired().HasMaxLength(300);
            builder.Property(x => x.Activo).IsRequired();
            builder.Property(x => x.StockMinimo).IsRequired().HasPrecision(18, 2);
            builder.Property(x => x.Imagen).IsRequired().HasMaxLength(int.MaxValue);
            builder.Property(x => x.StockActual).IsRequired().HasPrecision(18, 2);
            #endregion

            #region relationship configuration
            #endregion
        }
    }
}
