using Elysia.Core.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Elysia.Core.Domain.Entities
{
    public class CategoriaPlato : BaseEntity
    {
       
        //herada las propiedades que va a manejar
        public ICollection<Plato> Platos { get; set; } = new List<Plato>();

    }
}
