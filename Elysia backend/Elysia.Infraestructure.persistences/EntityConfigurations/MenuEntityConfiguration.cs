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
    public class MenuEntityConfiguration : IEntityTypeConfiguration<Menu>
    {
        public void Configure(EntityTypeBuilder<Menu> builder)
        {
            #region basic configuration
            builder.ToTable("Menus");
            builder.HasKey(x => x.Id);
            #endregion


            #region property configuration
            builder.Property(x => x.IsPrincipal).IsRequired();
            builder.Property(x => x.Estado).IsRequired();
            builder.Property(x => x.Nombre).IsRequired().HasMaxLength(250);
            builder.Property(x => x.Descripcion).IsRequired().HasMaxLength(250);
            builder.Property(x => x.FechaCreacion).IsRequired();
            #endregion


            #region relationship configuration
            #endregion
        }
    }
}
