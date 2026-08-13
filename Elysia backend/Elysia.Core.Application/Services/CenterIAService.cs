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
                 - Basado en datos reales de ventas, precios y estructura de menús
                 - Honesto y constructivo
                 - Enfocado en rentabilidad, rotación de platos y experiencia del cliente

                 Tu tarea es analizar el desempeño del menú (o menús) de un restaurante y generar recomendaciones concretas.

                 CONTEXTO MULTI-MENÚ (importante):
                 - Un restaurante puede tener UNO o VARIOS menús (principal, del día, infantil, etc.).
                 - Debes tener en cuenta esa estructura al analizar.
                 - No trates todos los platos como si pertenecieran a una sola carta indistinta.
                 - Cuando generes una recomendación sobre un plato, indica el menú al que pertenece si esa información está en el contexto (ejemplo: "Hamburguesa Clásica (Menú principal): ...").
                 - Si un plato está en más de un menú, indícalo.
                 - Los platos huérfanos (sin menú asignado) deben mencionarse de forma explícita, sobre todo en retirar o en sugerencias de asignación.
                 - Si solo hay un menú, analiza con normalidad sin forzar referencias innecesarias.

                  REGLAS IMPORTANTES:
                   1. Responde ÚNICAMENTE con un JSON válido, sin texto adicional antes o después.
                   2. No inventes datos que no estén en el contexto.
                   3. Cada recomendación debe incluir una justificación clara de al menos 2-3 oraciones.
                   4. Todo el contenido debe estar en español.
                   5. Si un plato no tiene ventas, menciónalo como candidato a revisar o retirar.
                   6. Prioriza acciones de alto impacto: platos top, platos sin ventas, platos huérfanos y riesgos de stock si aparecen en el contexto.
                   7. Diferencia, cuando aplique, entre menú principal y menús secundarios o temporales.

                 Estructura exacta del JSON que debes devolver:
                 {
                   "Resumen": "Resumen ejecutivo del estado del menú o menús (3-5 oraciones). Si hay varios menús, menciónalo.",
                   "PlatosPromocionar": [
                      "Nombre del plato (Menú X): justificación completa de por qué promocionarlo (mínimo 2-3 oraciones)"
                     ],
                   "PlatosRevisarPrecio": [
                      "Nombre del plato (Menú X): justificación completa de por qué revisar su precio (mínimo 2-3 oraciones)"
                    ],
                   "PlatosRetirar": [
                        "Nombre del plato (Menú X): justificación completa de por qué considerarlo para retirar (mínimo 2-3 oraciones)"
                     ],
                   "NuevasSugerencias": [
                      "Sugerencia concreta (puede ser por menú o transversal): justificación completa (mínimo 2-3 oraciones)"
                    ] 
                    }
                 """;

            // 3. Prompt
            var prompt = $"""
                 A continuación tienes los datos reales del menú (o menús) y su desempeño en los últimos 30 días.
                 El restaurante puede tener varios menús: analízalos con esa estructura y no los mezcles como una sola carta.
                 Genera las recomendaciones solicitadas.
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


            // ========== MOVIMIENTOS DE INVENTARIO (últimos 30 días) ==========
            var productosIds = productosActivos.Select(p => p.Id).ToList();

            sb.AppendLine("=== MOVIMIENTOS DE INVENTARIO (últimos 30 días) ===");

            if (productosIds.Count == 0)
            {
                sb.AppendLine("No hay productos activos para analizar movimientos.");
            }
            else
            {
                // IQueryable: filtra en BD si el provider es EF
                var movimientos = movimientoRepository.GetAllQuariableAsync()
                    .Where(m => productosIds.Contains(m.ProductoId) && m.FechaMovimiento >= fechaDesde)
                    .ToList(); // si es async real: usa ToListAsync() con using Microsoft.EntityFrameworkCore

                if (movimientos.Count == 0)
                {
                    sb.AppendLine("No hay movimientos registrados en los últimos 30 días.");
                }
                else
                {
                    var entradas = movimientos.Where(m => m.TipoMovimiento == TipoMovimientoInventario.Entrada).ToList();
                    var salidas = movimientos.Where(m => m.TipoMovimiento == TipoMovimientoInventario.Salida).ToList();
                    var ajustes = movimientos.Where(m => m.TipoMovimiento == TipoMovimientoInventario.Ajuste).ToList();

                    sb.AppendLine($"Total movimientos: {movimientos.Count}");
                    sb.AppendLine($"- Entradas: {entradas.Count} (cantidad: {entradas.Sum(x => x.Cantidad):N2})");
                    sb.AppendLine($"- Salidas: {salidas.Count} (cantidad: {salidas.Sum(x => x.Cantidad):N2})");
                    sb.AppendLine($"- Ajustes: {ajustes.Count} (cantidad: {ajustes.Sum(x => x.Cantidad):N2})");

                    // Nombre desde productos ya cargados (no dependemos de m.Producto)
                    var productosPorId = productosActivos.ToDictionary(p => p.Id, p => p.Nombre);

                    var topSalidas = salidas
                        .GroupBy(m => m.ProductoId)
                        .Select(g => new
                        {
                            Nombre = productosPorId.TryGetValue(g.Key, out var n) ? n : $"Producto {g.Key}",
                            Cantidad = g.Sum(x => x.Cantidad)
                        })
                        .OrderByDescending(x => x.Cantidad)
                        .Take(8)
                        .ToList();

                    sb.AppendLine("Productos con mayor salida (consumo/rotación):");
                    foreach (var item in topSalidas)
                        sb.AppendLine($"- {item.Nombre}: {item.Cantidad:N2}");

                    // Señal simple de posible sobre-stock: entradas sin salidas
                    var idsConSalida = salidas.Select(s => s.ProductoId).ToHashSet();
                    var soloEntradas = entradas
                        .Select(e => e.ProductoId)
                        .Distinct()
                        .Where(id => !idsConSalida.Contains(id))
                        .Take(5)
                        .ToList();

                    if (soloEntradas.Count > 0)
                    {
                        sb.AppendLine("Productos con entradas pero sin salidas en el período:");
                        foreach (var id in soloEntradas)
                        {
                            var nombre = productosPorId.TryGetValue(id, out var n) ? n : $"Producto {id}";
                            sb.AppendLine($"- {nombre}");
                        }
                    }
                }
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

            // --- Base ---
            var platos = (await platoRepository.GetAllByPropietarioId(restauranteId))?.ToList()
                         ?? new List<Plato>();
            var pedidos = await pedidoRepository.GetPedidosByPropietarioAndFecha(restauranteId, fechaDesde);
            var productos = await productoRepository.GetListProductosByPropietarioid(restauranteId);
            var productosDict = (productos ?? new List<Producto>()).ToDictionary(p => p.Id);

            // --- Menús ---
            var menus = (await menuRepository.GetListMenuByPropietarioId(restauranteId))
                ?.Where(m => m != null)
                .Cast<Menu>()
                .ToList() ?? new List<Menu>();

            // PlatoMenus de todos los menús del propietario
            var platoMenus = new List<PlatoMenu>();
            foreach (var menu in menus)
            {
                var lista = await platoMenuRepository.GetListByMenuId(menu.Id);
                if (lista != null)
                    platoMenus.AddRange(lista.Where(x => x != null)!);
            }

            var platosEnMenu = platoMenus.Select(pm => pm.IdPlato).ToHashSet();
            var platosHuerfanos = platos.Where(p => !platosEnMenu.Contains(p.Id)).ToList();

            // --- Recetas (PlatoProducto) en una sola query ---
            var platoIds = platos.Select(p => p.Id).ToList();
            var platoProductos = platoIds.Count == 0
                ? new List<PlatoProducto>()
                : platoProductoRepository.GetAllQuariableAsync()
                    .Where(pp => platoIds.Contains(pp.PlatoId))
                    .ToList();

            // --- Ventas ---
            var ventasPorPlato = pedidos
                .SelectMany(p => p.DetallesPedidos ?? Enumerable.Empty<DetallesPedido>())
                .GroupBy(d => d.PlatoId)
                .Select(g => new
                {
                    PlatoId = g.Key,
                    CantidadVendida = g.Sum(x => x.Cantidad),
                    Nombre = g.FirstOrDefault()?.Plato?.Nombre ?? "Plato desconocido"
                })
                .ToDictionary(x => x.PlatoId, x => x);

            // ========== ESTRUCTURA ==========
            sb.AppendLine("=== ESTRUCTURA DE MENÚS ===");
            sb.AppendLine($"Total menús: {menus.Count}");
            sb.AppendLine($"Total platos registrados: {platos.Count}");
            sb.AppendLine($"Platos en al menos un menú: {platosEnMenu.Count}");
            sb.AppendLine($"Platos huérfanos (sin menú): {platosHuerfanos.Count}");

            if (platosHuerfanos.Count > 0)
            {
                sb.AppendLine("Platos sin menú asignado:");
                foreach (var p in platosHuerfanos.Take(10))
                    sb.AppendLine($"- {p.Nombre} | Precio: ${p.Precio:N2}");
            }
            sb.AppendLine();

            if (menus.Count > 0)
            {
                sb.AppendLine("Resumen por menú:");
                foreach (var menu in menus.Take(10))
                {
                    var cant = platoMenus.Count(pm => pm.IdMenu == menu.Id);
                    var etiqueta = menu.IsPrincipal ? " (principal)" : string.Empty;
                    sb.AppendLine($"- {menu.Nombre}{etiqueta}: {cant} platos");
                }
                sb.AppendLine();
            }

            // ========== DESEMPEÑO + RIESGO STOCK ==========
            sb.AppendLine("=== DESEMPEÑO DEL MENÚ (últimos 30 días) ===");

            string InfoRiesgo(int platoId)
            {
                var ings = platoProductos.Where(pp => pp.PlatoId == platoId).ToList();
                if (ings.Count == 0) return " | Sin receta cargada";

                var enRiesgo = new List<string>();
                foreach (var ing in ings)
                {
                    if (!productosDict.TryGetValue(ing.ProductoId, out var prod)) continue;
                    if (prod.Activo && prod.StockActual <= prod.StockMinimo)
                        enRiesgo.Add(prod.Nombre);
                }
                return enRiesgo.Count > 0
                    ? $" | Riesgo stock: {string.Join(", ", enRiesgo.Take(3))}"
                    : string.Empty;
            }

            var masVendidos = ventasPorPlato.Values
                .OrderByDescending(x => x.CantidadVendida)
                .Take(8)
                .ToList();

            sb.AppendLine("Top platos más vendidos:");
            if (masVendidos.Count > 0)
            {
                foreach (var item in masVendidos)
                {
                    var plato = platos.FirstOrDefault(p => p.Id == item.PlatoId);
                    var precio = plato?.Precio ?? 0;
                    sb.AppendLine($"- {item.Nombre} | Vendidos: {item.CantidadVendida} | Precio: ${precio:N2}{InfoRiesgo(item.PlatoId)}");
                }
            }
            else
            {
                sb.AppendLine("No hay ventas registradas en los últimos 30 días.");
            }
            sb.AppendLine();

            sb.AppendLine("Platos con baja o nula rotación:");
            var bajaRotacion = platos
                .Select(p =>
                {
                    ventasPorPlato.TryGetValue(p.Id, out var v);
                    return new { Plato = p, Cantidad = v?.CantidadVendida ?? 0 };
                })
                .OrderBy(x => x.Cantidad)
                .Take(8);

            foreach (var item in bajaRotacion)
            {
                sb.AppendLine(
                    $"- {item.Plato.Nombre} | Vendidos: {item.Cantidad} | Precio: ${item.Plato.Precio:N2}{InfoRiesgo(item.Plato.Id)}");
            }
            sb.AppendLine();

            sb.AppendLine("Lista completa (Nombre | Precio | Unidades | Riesgo):");
            foreach (var p in platos.OrderByDescending(x =>
            {
                ventasPorPlato.TryGetValue(x.Id, out var v);
                return v?.CantidadVendida ?? 0;
            }))
            {
                ventasPorPlato.TryGetValue(p.Id, out var venta);
                var cant = venta?.CantidadVendida ?? 0;
                sb.AppendLine($"- {p.Nombre} | ${p.Precio:N2} | {cant} uds{InfoRiesgo(p.Id)}");
            }

            return sb.ToString();
        }









    }
}
