using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Elysia.Core.Application.Dtos.CenterIA
{
    public class RespuestaInsightDto
    {
        /// <summary>
        /// Resumen ejecutivo del estado del restaurante.
        /// </summary>
        public string Resumen { get; set; } = string.Empty;

        /// <summary>
        /// Análisis relacionado con las ventas e ingresos.
        /// </summary>
        public string AnalisisVentas { get; set; } = string.Empty;

        /// <summary>
        /// Análisis de los pedidos realizados.
        /// </summary>
        public string AnalisisPedidos { get; set; } = string.Empty;

        /// <summary>
        /// Análisis del estado del inventario.
        /// </summary>
        public string AnalisisInventario { get; set; } = string.Empty;

        /// <summary>
        /// Análisis del rendimiento y comportamiento del menú.
        /// </summary>
        public string AnalisisMenu { get; set; } = string.Empty;

        /// <summary>
        /// Análisis de las reservas del restaurante.
        /// </summary>
        public string AnalisisReservas { get; set; } = string.Empty;

        /// <summary>
        /// Análisis del desempeño de los empleados.
        /// </summary>
        public string AnalisisEmpleados { get; set; } = string.Empty;

        /// <summary>
        /// Recomendaciones estratégicas generadas por la IA.
        /// </summary>
        public List<string> Recomendaciones { get; set; } = [];


    }
}
