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
    public class PlatoEntityConfiguration : IEntityTypeConfiguration<Plato>
    {
        public void Configure(EntityTypeBuilder<Plato> builder)
        {
            #region basic configuration
            builder.ToTable("Platos");
            builder.HasKey(x => x.Id);

            #endregion


            #region property configuration
            builder.Property(x => x.Estado).IsRequired();   
            builder.Property(x => x.Nombre).IsRequired().HasMaxLength(250);   
            builder.Property(x => x.Descripcion).IsRequired().HasMaxLength(250);
            builder.Property(x => x.Codigo).IsRequired().HasMaxLength(250);
            builder.Property(x => x.Imagen).HasMaxLength(int.MaxValue);
            builder.Property(x => x.Precio).HasPrecision(20, 2);
            #endregion


            #region relationship configuration
            builder.HasOne(x => x.Categoria)
                   .WithMany(c => c.Platos)
                   .HasForeignKey(x => x.CategoriaId)
                   .OnDelete(DeleteBehavior.Cascade);

            #endregion
        }
    }
}
