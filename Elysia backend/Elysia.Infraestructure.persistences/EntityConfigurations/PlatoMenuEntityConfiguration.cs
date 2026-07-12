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
    public class PlatoMenuEntityConfiguration : IEntityTypeConfiguration<PlatoMenu>
    {
        public void Configure(EntityTypeBuilder<PlatoMenu> builder)
        {

            #region basic configuration 
            builder.ToTable("PlatoMenus");
            builder.HasKey(x => x.Id);
            #endregion


            #region property configuration

            #endregion




            #region relationship configuration
            builder.HasOne(x => x.Menu)
                  .WithMany(c => c.PlatoMenus)
                  .HasForeignKey(x => x.IdMenu)
                  .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(x => x.Plato)
                   .WithMany(c => c.PlatoMenus)
                   .HasForeignKey(x =>x.IdPlato)
                   .OnDelete(DeleteBehavior.Cascade);
            #endregion

        }
    }
}
