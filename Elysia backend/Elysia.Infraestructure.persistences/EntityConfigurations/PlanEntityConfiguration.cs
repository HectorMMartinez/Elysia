
using Elysia.Core.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Elysia.Infraestructure.persistences.EntityConfigurations
{
    public class PlanEntityConfiguration : IEntityTypeConfiguration<Plan>
    {
        public void Configure(EntityTypeBuilder<Plan> builder)
        {

            #region basic configuration
            builder.HasKey(x => x.Id);  
            builder.ToTable("Planes");
            #endregion


            #region property configuration
            builder.Property(x => x.Nombre).IsRequired().HasMaxLength(256);
            builder.Property(x => x.Descripcion).IsRequired().HasMaxLength(int.MaxValue);
            #endregion


            #region relationship configuration

            #endregion


        }
    }
}
