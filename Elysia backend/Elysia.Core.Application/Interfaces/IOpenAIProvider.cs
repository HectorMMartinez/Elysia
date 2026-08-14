using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Elysia.Core.Application.Interfaces
{
    public interface IOpenAIProvider
    {

        Task<string> GenerarRespuestaAsync(
         string prompt,
         string? instructions = null,
         CancellationToken cancellationToken = default);

    }
}
