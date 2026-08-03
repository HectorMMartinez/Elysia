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
    public class EmployeeShiftEntityConfiguration : IEntityTypeConfiguration<EmployeeShift>
    {
        public void Configure(EntityTypeBuilder<EmployeeShift> builder)
        {
            #region basic region
            builder.ToTable("EmployeeShifts");
            builder.HasKey(x => x.Id);
            #endregion


            #region property configuration

            #endregion


            #region relationship configuration
            builder.HasOne(x => x.Empleado)
                .WithMany(x => x.EmployeeShifts)
                .HasForeignKey(x => x.EmpleadoId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(x => x.Shift)
                   .WithMany(x => x.EmployeeShifts)
                   .HasForeignKey(x => x.ShiftId)
                   .OnDelete(DeleteBehavior.Cascade);
            #endregion
        }
    }
}
