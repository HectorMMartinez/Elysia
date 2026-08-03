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
    public class EmpleadoEntityConfiguration : IEntityTypeConfiguration<Empleado>
    {
        public void Configure(EntityTypeBuilder<Empleado> builder)
        {

            #region basic configuration
            builder.ToTable("Empleados");
            builder.HasKey(x => x.Id);
            #endregion

            #region property configuration
            builder.Property(x => x.Salary).IsRequired().HasPrecision(24, 2);
            builder.Property(x => x.Email).IsRequired();
            builder.Property(x => x.FirstName).IsRequired().HasMaxLength(250);
            builder.Property(x => x.LastName).IsRequired().HasMaxLength(250);
            builder.Property(x => x.RestaurantId).IsRequired().HasMaxLength(int.MaxValue);
            builder.Property(x => x.Phone).IsRequired().HasMaxLength(11);
            #endregion


            #region relationship configuration
            builder.HasOne(x => x.Puesto)
                .WithMany(x => x.Empleados)
                .HasForeignKey(x => x.PuestoId)
                .OnDelete(DeleteBehavior.Restrict);

                   
            #endregion



        }
    }
}
