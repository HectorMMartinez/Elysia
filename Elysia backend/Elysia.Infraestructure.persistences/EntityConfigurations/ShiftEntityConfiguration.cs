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
    public class ShiftEntityConfiguration : IEntityTypeConfiguration<Shift>
    {
        public void Configure(EntityTypeBuilder<Shift> builder)
        {

            #region basic configuration
            builder.ToTable("Shifts");
            builder.HasKey(x => x.Id);
            #endregion



            #region property configuration
            builder.Property(x => x.Name).IsRequired().HasMaxLength(250);
            builder.Property(x => x.StartTime).IsRequired();
            builder.Property(x => x.EndTime).IsRequired();
            #endregion


            #region relationship configuration
            #endregion
        }
    }
}
