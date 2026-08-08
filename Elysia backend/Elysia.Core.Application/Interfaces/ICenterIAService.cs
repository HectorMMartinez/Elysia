using Elysia.Core.Application.Dtos.CenterIA;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Elysia.Core.Application.Interfaces
{
    public interface ICenterIAService
    {
        Task<RespuestaInsightDto> AnalizarRestauranteAsync(string restauranteId);

        Task<RespuestaOptimizacionMenuDto> OptimizarMenuAsync(string restauranteId);

        Task<RespuestaChatIaDto> ConsultarAsistenteAsync(string restauranteId, SolicitudChatIaDto solicitud);

    }
}
