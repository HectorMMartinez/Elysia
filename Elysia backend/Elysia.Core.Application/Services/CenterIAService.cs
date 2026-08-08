using Elysia.Core.Application.Dtos.CenterIA;
using Elysia.Core.Application.Interfaces;
using Elysia.Core.Domain.Common;
using Elysia.Core.Domain.Entities;
using Elysia.Core.Domain.interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Elysia.Core.Application.Services
{
    public class CenterIAService : ICenterIAService
    {
        private readonly IReservasRepository reservasRepository;
        private readonly IPedidoRepository pedidoRepository;
        private readonly IProductoRepository productoRepository; //gestion de producto como tal inventario
        private readonly IPlatoProductoRepository platoProductoRepository; //relacion entre  plato y el consumo de productos
        private readonly IShiftRepository shiftRepository; //turnos
        private readonly IShiftEmpleadoRepository shiftEmpleadoRepository; //turno-empleado
        private readonly IEmpleadoRepository empleadoRepository; //gestion de empleado
        private readonly IMenuRepository menuRepository; //crear un menu
        private readonly IPlatoMenuRepository platoMenuRepository; //agregar plato a un menu
        private readonly IDetallesPedidoRepository detallesPedidoRepository; //contiene el detalle de un pedido como tal
        private readonly IMovimientoRepository movimientoRepository; // registrar los movimiento de un producto entradas/salidas
        private readonly IDashboardPropietarioServices dashboardPropietarioServices; //contiene un metodo que devuelve los indicadores con otros ids
        private readonly IOpenAIProvider openAIProvider;
        private readonly IPlatoRepository platoRepository;


        public CenterIAService(IOpenAIProvider openAIProvider, IReservasRepository reservasRepository, IPlatoRepository platoRepository, IPedidoRepository pedidoRepository, IProductoRepository productoRepository, IPlatoProductoRepository platoProductoRepository, IShiftRepository shiftRepository, IShiftEmpleadoRepository shiftEmpleadoRepository, IEmpleadoRepository empleadoRepository, IMenuRepository menuRepository, IPlatoMenuRepository platoMenuRepository, IDetallesPedidoRepository detallesPedidoRepository, IMovimientoRepository movimientoRepository, IDashboardPropietarioServices dashboardPropietarioServices)
        {
            this.reservasRepository = reservasRepository;
            this.pedidoRepository = pedidoRepository;
            this.productoRepository = productoRepository;
            this.platoProductoRepository = platoProductoRepository;
            this.shiftRepository = shiftRepository;
            this.shiftEmpleadoRepository = shiftEmpleadoRepository;
            this.empleadoRepository = empleadoRepository;
            this.menuRepository = menuRepository;
            this.platoMenuRepository = platoMenuRepository;
            this.detallesPedidoRepository = detallesPedidoRepository;
            this.movimientoRepository = movimientoRepository;
            this.dashboardPropietarioServices = dashboardPropietarioServices;
            this.openAIProvider = openAIProvider;
            this.platoRepository = platoRepository;



        }


        public async Task<RespuestaInsightDto> AnalizarRestauranteAsync(string restauranteId)
        {
            // 1. Construir el contexto con datos reales
            var contexto = await ConstruirContextoInsightsAsync(restauranteId);

            // 2. Instructions (Super Prompt)
            var instructions = """
              Eres un consultor senior especializado en gestión y optimización de restaurantes con más de 15 años de experiencia.
              Has asesorado a cientos de establecimientos de diferentes tamaños y conceptos (casual, fine dining, fast casual, etc.).

             Tu estilo es:
               - Directo, claro y profesional
               - Orientado a la acción (no te quedas solo en el diagnóstico)
               - Basado en datos reales, no en generalidades
               - Honesto: si algo está mal, lo dices con claridad pero de forma constructiva
               - Enfocado en rentabilidad, eficiencia operativa y experiencia del cliente

                Tu tarea es analizar los datos de un restaurante y generar un informe ejecutivo de alta calidad.

             
              REGLAS IMPORTANTES:
              1. Responde ÚNICAMENTE con un JSON válido, sin texto adicional antes o después.
              2. No inventes datos que no estén en el contexto.
              3. Si algún área no tiene suficiente información, indícalo brevemente en ese apartado.
              4. Las recomendaciones deben ser concretas, priorizadas y accionables (máximo 7).
              5. Usa un lenguaje profesional pero fácil de entender para el dueño del restaurante.
              6. Todo el contenido debe estar en español.

              Estructura exacta del JSON que debes devolver:
            {
               "Resumen": "Visión general ejecutiva del estado del restaurante (3-5 oraciones potentes)",
               "AnalisisVentas": "Análisis de ingresos, ticket promedio y tendencias de ventas",
               "AnalisisPedidos": "Análisis del volumen, estados y eficiencia de los pedidos",
               "AnalisisInventario": "Análisis del estado del inventario y riesgos de desabastecimiento",
               "AnalisisMenu": "Análisis del desempeño de los platos (más y menos vendidos)",
               "AnalisisReservas": "Análisis de reservas, tasa de no-show y ocupación",
               "AnalisisEmpleados": "Análisis de dotación de personal y turnos",
               "Recomendaciones": [
                "Recomendación prioritaria 1",
                 "Recomendación prioritaria 2"
                ]
             }
            """;

            // 3. Prompt del usuario
            var prompt = $"""
               A continuación tienes los datos reales del restaurante correspondientes a los últimos 30 días.
                Analízalos con profundidad y genera el informe ejecutivo solicitado.

             {contexto}
           """;

            // 4. Llamar a OpenAI
            var respuestaTexto = await openAIProvider.GenerarRespuestaAsync(prompt, instructions);

            // 5. Deserializar
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };

            var resultado = JsonSerializer.Deserialize<RespuestaInsightDto>(respuestaTexto, options);

            if (resultado is null)
                throw new InvalidOperationException("No se pudo interpretar la respuesta de la IA.");

            return resultado;
        }




        public async Task<RespuestaChatIaDto> ConsultarAsistenteAsync(string restauranteId, SolicitudChatIaDto solicitud)
        {
            if (string.IsNullOrWhiteSpace(solicitud.Pregunta))
                throw new ArgumentException("La pregunta no puede estar vacía.", nameof(solicitud.Pregunta));

            // 1. Construir el mismo contexto de los últimos 30 días
            var contextoRestaurante = await ConstruirContextoInsightsAsync(restauranteId);

            // 2. Instructions (Super Prompt del Asistente)
            var instructions = """
                
                   Eres el Asistente Inteligente de Elysia, un sistema profesional de gestión de restaurantes.
                   Actúas como un consultor interno experto del restaurante del usuario.

                  Tu estilo es:
                   - Claro, directo y profesional
                   - Basado exclusivamente en los datos proporcionados y en el historial de la conversación
                   - Honesto: si no tienes suficiente información, lo dices
                   - Orientado a ayudar al dueño a tomar mejores decisiones
                   - Respuestas concisas pero completas (evita relleno innecesario)

                  REGLAS IMPORTANTES:
                  1. Solo respondes preguntas relacionadas con el restaurante (ventas, menú, inventario, reservas, empleados, operaciones, etc.).
                  2. Si la pregunta no tiene relación con el restaurante, responde amablemente que solo puedes ayudar con temas del negocio.
                  3. Nunca inventes datos. Si algo no está en el contexto ni en el historial, indícalo.
                  4. Usa el historial de la conversación para mantener coherencia y responder preguntas de seguimiento.
                  5. Responde siempre en español.
                  6. Sé concreto y accionable cuando sea posible.
                
                """;

            // 3. Construir el prompt con contexto + historial + pregunta actual
            var sb = new StringBuilder();

            sb.AppendLine("=== CONTEXTO DEL RESTAURANTE (últimos 30 días) ===");
            sb.AppendLine(contextoRestaurante);
            sb.AppendLine();

            // Historial de la conversación (si existe)
            if (solicitud.Historial != null && solicitud.Historial.Any())
            {
                sb.AppendLine("=== HISTORIAL DE LA CONVERSACIÓN ===");
                foreach (var mensaje in solicitud.Historial)
                {
                    var rol = mensaje.Rol.Equals("assistant", StringComparison.OrdinalIgnoreCase)
                        ? "Asistente"
                        : "Usuario";

                    sb.AppendLine($"{rol}: {mensaje.Contenido}");
                }
                sb.AppendLine();
            }

            sb.AppendLine("=== PREGUNTA ACTUAL DEL USUARIO ===");
            sb.AppendLine(solicitud.Pregunta);

            var prompt = sb.ToString();

            // 4. Llamar a OpenAI
            var respuesta = await openAIProvider.GenerarRespuestaAsync(prompt, instructions);

            // 5. Devolver respuesta
            return new RespuestaChatIaDto
            {
                Pregunta = solicitud.Pregunta,
                Respuesta = respuesta
            };
        }




        public async Task<RespuestaOptimizacionMenuDto> OptimizarMenuAsync(string restauranteId)
        {
            // 1. Construir contexto específico de menú
            var contexto = await ConstruirContextoMenuAsync(restauranteId);

            // 2. Instructions
            var instructions = """
                   Eres un consultor gastronómico y de rentabilidad especializado en restaurantes, con amplia experiencia optimizando menús para aumentar ventas y márgenes.

                  Tu estilo es:
                  - Directo, profesional y orientado a resultados
                  - Basado en datos reales de ventas y precios
                  - Honesto y constructivo
                  - Enfocado en rentabilidad, rotación de platos y experiencia del cliente

                   Tu tarea es analizar el desempeño del menú de un restaurante y generar recomendaciones concretas.

                 REGLAS IMPORTANTES:
                 1. Responde ÚNICAMENTE con un JSON válido, sin texto adicional antes o después.
                 2. No inventes datos que no estén en el contexto.
                 3. Cada recomendación debe incluir una justificación clara de al menos 2-3 oraciones.
                 4. Todo el contenido debe estar en español.
                 5. Si un plato no tiene ventas, menciónalo como candidato a revisar o retirar.

                Estructura exacta del JSON que debes devolver:
              {
               "Resumen": "Resumen ejecutivo del estado del menú (3-5 oraciones)",
               "PlatosPromocionar": [
               "Nombre del plato: justificación completa de por qué promocionarlo (mínimo 2-3 oraciones)"
              ],
              "PlatosRevisarPrecio": [
              "Nombre del plato: justificación completa de por qué revisar su precio (mínimo 2-3 oraciones)"
              ],
               "PlatosRetirar": [
               "Nombre del plato: justificación completa de por qué considerarlo para retirar (mínimo 2-3 oraciones)"
               ],
              "NuevasSugerencias": [
                  "Sugerencia concreta: justificación completa (mínimo 2-3 oraciones)"
               ]
            }
        """;

            // 3. Prompt
            var prompt = $"""
              A continuación tienes los datos reales del menú y su desempeño en los últimos 30 días.
              Analízalos y genera las recomendaciones solicitadas.

        {contexto}
        """;

            // 4. Llamar a OpenAI
            var respuestaTexto = await openAIProvider.GenerarRespuestaAsync(prompt, instructions);

            // 5. Deserializar
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };

            var resultado = JsonSerializer.Deserialize<RespuestaOptimizacionMenuDto>(respuestaTexto, options);

            if (resultado is null)
                throw new InvalidOperationException("No se pudo interpretar la respuesta de la IA.");

            return resultado;
        }







        private async Task<string> ConstruirContextoInsightsAsync(string restauranteId)
        {
            var sb = new StringBuilder();
            var fechaDesde = DateTime.UtcNow.Date.AddDays(-30);

            // ========== RESUMEN GENERAL ==========
            var indicadores = await dashboardPropietarioServices.GetIndicadoresPanelPropietario(restauranteId);

            sb.AppendLine("=== RESUMEN GENERAL ===");

            if (indicadores.Item1 is not null) // Premium
            {
                var p = indicadores.Item1;
                sb.AppendLine($"Mesas: {p.CantidadMesa} | Menús: {p.CantidadMenu} | Platos: {p.CantidadPlato} | Productos: {p.CantidadProducto}");
                sb.AppendLine($"Pedidos totales (histórico): {p.TotalPedidos}");
                sb.AppendLine($"Reservas totales (histórico): {p.TotalReservas}");
                sb.AppendLine($"Empleados: {p.TotalEmpleados} (Activos: {p.EmpleadoActivos} | Inactivos: {p.EmpleadoNoActivos})");
                sb.AppendLine($"Turnos definidos: {p.TotalTurno} | Empleados asociados a turnos: {p.TotalEmpleadoAsociadosTurno}");
                sb.AppendLine($"Platos asociados a menú: {p.PlatoAsociadoAUnMenu}");
            }
            else if (indicadores.Item2 is not null) // Simple
            {
                var p = indicadores.Item2;
                sb.AppendLine($"Mesas: {p.CantidadMesa} | Menús: {p.CantidadMenu} | Platos: {p.CantidadPlato} | Productos: {p.CantidadProducto}");
                sb.AppendLine($"Pedidos totales (histórico): {p.TotalPedidos}");
                sb.AppendLine($"Reservas totales (histórico): {p.TotalReservas}");
                sb.AppendLine($"Platos asociados a menú: {p.PlatoAsociadoAUnMenu}");
            }
            else
            {
                sb.AppendLine("No se pudieron obtener los indicadores generales.");
            }

            sb.AppendLine();


            // ========== PEDIDOS ==========
            var pedidos = await pedidoRepository.GetPedidosByPropietarioAndFecha(restauranteId, fechaDesde);

            sb.AppendLine("=== PEDIDOS (últimos 30 días) ===");
            sb.AppendLine($"Total de pedidos: {pedidos.Count}");

            if (pedidos.Any())
            {
                var finalizados = pedidos.Count(p => p.Estado == EstadoPedido.Finalizado || p.Estado == EstadoPedido.Entregado);
                var cancelados = pedidos.Count(p => p.Estado == EstadoPedido.Cancelado);
                var otros = pedidos.Count - finalizados - cancelados;

                var ingresos = pedidos
                    .Where(p => p.Estado == EstadoPedido.Finalizado || p.Estado == EstadoPedido.Entregado)
                    .Sum(p => p.Total);

                var ticketPromedio = finalizados > 0 ? ingresos / finalizados : 0;

                sb.AppendLine($"- Finalizados/Entregados: {finalizados}");
                sb.AppendLine($"- Cancelados: {cancelados}");
                sb.AppendLine($"- Otros (Pendiente/En proceso/Listo): {otros}");
                sb.AppendLine($"Ingresos totales: ${ingresos:N2}");
                sb.AppendLine($"Ticket promedio: ${ticketPromedio:N2}");
                sb.AppendLine();

                // Top platos
                var platosVendidos = pedidos
                    .SelectMany(p => p.DetallesPedidos)
                    .GroupBy(d => d.Plato?.Nombre ?? "Plato desconocido")
                    .Select(g => new { Nombre = g.Key, Cantidad = g.Sum(x => x.Cantidad) })
                    .OrderByDescending(x => x.Cantidad)
                    .ToList();

                sb.AppendLine("Top 5 platos más vendidos:");
                foreach (var plato in platosVendidos.Take(5))
                    sb.AppendLine($"- {plato.Nombre}: {plato.Cantidad} unidades");

                sb.AppendLine();
                sb.AppendLine("Top 5 platos menos vendidos:");
                foreach (var plato in platosVendidos.OrderBy(x => x.Cantidad).Take(5))
                    sb.AppendLine($"- {plato.Nombre}: {plato.Cantidad} unidades");
            }
            else
            {
                sb.AppendLine("No hay pedidos registrados en los últimos 30 días.");
            }
            sb.AppendLine();

            // ========== INVENTARIO ==========
            var productos = await productoRepository.GetListProductosByPropietarioid(restauranteId);
            var productosActivos = productos.Where(p => p.Activo).ToList();
            var stockBajo = productosActivos.Where(p => p.StockActual <= p.StockMinimo).ToList();

            sb.AppendLine("=== INVENTARIO ===");
            sb.AppendLine($"Total productos activos: {productosActivos.Count}");
            sb.AppendLine($"Productos con stock bajo/crítico: {stockBajo.Count}");

            if (stockBajo.Any())
            {
                foreach (var p in stockBajo.Take(10)) // máximo 10 para no saturar
                    sb.AppendLine($"- {p.Nombre ?? "Producto"} → {p.StockActual} {p.UnidadMedida} (mínimo {p.StockMinimo})");
            }
            sb.AppendLine();

            // ========== RESERVAS ==========
            var reservas = await reservasRepository.GetReservasByPropietarioAndFecha(restauranteId, fechaDesde);

            sb.AppendLine("=== RESERVAS (últimos 30 días) ===");
            sb.AppendLine($"Total de reservas: {reservas.Count}");

            if (reservas.Any())
            {
                var finalizadas = reservas.Count(r => r.Estado == EstadoReserva.Finalizada);
                var canceladas = reservas.Count(r => r.Estado == EstadoReserva.Cancelada);
                var noShow = reservas.Count(r => r.Estado == EstadoReserva.NoAsistio);
                var activas = reservas.Count(r => r.Estado == EstadoReserva.Activa || r.Estado == EstadoReserva.EnProceso);

                var tasaNoShow = reservas.Count > 0 ? (noShow * 100.0 / reservas.Count) : 0;

                sb.AppendLine($"- Finalizadas: {finalizadas}");
                sb.AppendLine($"- Canceladas: {canceladas}");
                sb.AppendLine($"- No asistió: {noShow}");
                sb.AppendLine($"- Activas/En proceso: {activas}");
                sb.AppendLine($"Tasa de no-show: {tasaNoShow:N1}%");
            }
            else
            {
                sb.AppendLine("No hay reservas registradas en los últimos 30 días.");
            }
            sb.AppendLine();

            // ========== EMPLEADOS Y TURNOS ==========
            var empleadosActivos = await empleadoRepository.GetAllEmpleadosActivos(restauranteId);
            var empleadosInactivos = await empleadoRepository.GetAllEmpleadosInactivo(restauranteId);
            var turnos = await shiftRepository.GetAllTurnoByPropietarioId(restauranteId);

            int empleadosConTurno = 0;
            foreach (var emp in empleadosActivos)
            {
                var tieneTurno = await shiftEmpleadoRepository.GetOneEmployeeShiftsByEmpleadoId(emp.Id);
                if (tieneTurno != null) empleadosConTurno++;
            }

            sb.AppendLine("=== EMPLEADOS Y TURNOS ===");
            sb.AppendLine($"Total empleados: {empleadosActivos.Count + empleadosInactivos.Count}");
            sb.AppendLine($"- Activos: {empleadosActivos.Count}");
            sb.AppendLine($"- Inactivos: {empleadosInactivos.Count}");
            sb.AppendLine($"Empleados asociados a turnos: {empleadosConTurno}");
            sb.AppendLine($"Total turnos definidos: {turnos.Count}");

            return sb.ToString();
        }





        private async Task<string> ConstruirContextoMenuAsync(string restauranteId)
        {
            var sb = new StringBuilder();
            var fechaDesde = DateTime.UtcNow.Date.AddDays(-30);

            // Platos del restaurante
            var platos = await platoRepository.GetAllByPropietarioId(restauranteId);
            var platosList = platos?.ToList() ?? new List<Plato>();

            // Pedidos de los últimos 30 días (para calcular ventas)
            var pedidos = await pedidoRepository.GetPedidosByPropietarioAndFecha(restauranteId, fechaDesde);

            // Calcular ventas por plato
            var ventasPorPlato = pedidos
                .SelectMany(p => p.DetallesPedidos)
                .GroupBy(d => d.PlatoId)
                .Select(g => new
                {
                    PlatoId = g.Key,
                    CantidadVendida = g.Sum(x => x.Cantidad),
                    Nombre = g.FirstOrDefault()?.Plato?.Nombre ?? "Plato desconocido"
                })
                .ToDictionary(x => x.PlatoId, x => x);

            sb.AppendLine("=== DESEMPEÑO DEL MENÚ (últimos 30 días) ===");
            sb.AppendLine($"Total de platos registrados: {platosList.Count}");
            sb.AppendLine();

            // Top más vendidos
            var masVendidos = ventasPorPlato.Values
                .OrderByDescending(x => x.CantidadVendida)
                .Take(8)
                .ToList();

            sb.AppendLine("Top platos más vendidos:");
            if (masVendidos.Any())
            {
                foreach (var item in masVendidos)
                {
                    var plato = platosList.FirstOrDefault(p => p.Id == item.PlatoId);
                    var precio = plato?.Precio ?? 0;
                    sb.AppendLine($"- {item.Nombre} | Vendidos: {item.CantidadVendida} | Precio: ${precio:N2}");
                }
            }
            else
            {
                sb.AppendLine("No hay ventas registradas en los últimos 30 días.");
            }
            sb.AppendLine();

            // Platos con baja o nula rotación
            sb.AppendLine("Platos con baja o nula rotación:");
            var platosConPocasVentas = platosList
                .Select(p =>
                {
                    ventasPorPlato.TryGetValue(p.Id, out var venta);
                    return new
                    {
                        Plato = p,
                        Cantidad = venta?.CantidadVendida ?? 0
                    };
                })
                .OrderBy(x => x.Cantidad)
                .Take(8)
                .ToList();

            foreach (var item in platosConPocasVentas)
            {
                sb.AppendLine($"- {item.Plato.Nombre} | Vendidos: {item.Cantidad} | Precio: ${item.Plato.Precio:N2}");
            }
            sb.AppendLine();

            // Lista completa resumida (útil para la IA)
            sb.AppendLine("Lista completa de platos (Nombre | Precio | Unidades vendidas):");
            foreach (var p in platosList.OrderByDescending(x =>
            {
                ventasPorPlato.TryGetValue(x.Id, out var v);
                return v?.CantidadVendida ?? 0;
            }))
            {
                ventasPorPlato.TryGetValue(p.Id, out var venta);
                var cantidad = venta?.CantidadVendida ?? 0;
                sb.AppendLine($"- {p.Nombre} | ${p.Precio:N2} | {cantidad} unidades");
            }

            return sb.ToString();
        }












    }
}
