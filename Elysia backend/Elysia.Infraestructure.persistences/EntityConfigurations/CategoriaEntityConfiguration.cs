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
    public class CategoriaEntityConfiguration : IEntityTypeConfiguration<CategoriaPlato>
    {
        public void Configure(EntityTypeBuilder<CategoriaPlato> builder)
        {
            #region basic configuration
            builder.ToTable("CategoriasPlatos");
            builder.HasKey(c => c.Id);
            #endregion


            #region property configuration
            builder.Property(x => x.Nombre).IsRequired().HasMaxLength(250);
            builder.Property(x => x.Descripcion).IsRequired().HasMaxLength(250);
            #endregion



            #region relationship configuration
            #endregion

        }
    }
}
