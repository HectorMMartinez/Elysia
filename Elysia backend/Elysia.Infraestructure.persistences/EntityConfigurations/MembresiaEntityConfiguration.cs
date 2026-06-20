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
    public class MembresiaEntityConfiguration : IEntityTypeConfiguration<Membresia>
    {
        public void Configure(EntityTypeBuilder<Membresia> builder)
        {
            #region Basic configuration
            builder.ToTable("Membresias");
            builder.HasKey(x => x.Id);
            #endregion



            #region property configuration
            builder.Property(x => x.UsuarioId).IsRequired().HasMaxLength(int.MaxValue);
            builder.Property(x => x.PlanId).IsRequired();
            builder.Property(x => x.Estado).IsRequired();
            builder.Property(x => x.FechaFin).IsRequired();
            #endregion



            #region relationship configuration
               builder.HasOne(m => m.Plan)
               .WithMany(p => p.Membresias)
               .HasForeignKey(m => m.PlanId)
               .OnDelete(DeleteBehavior.Restrict);
            #endregion
        }
    }
}
