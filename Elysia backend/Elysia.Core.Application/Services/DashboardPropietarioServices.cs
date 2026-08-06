using Elysia.Core.Application.Dtos.dashboard;
using Elysia.Core.Application.Interfaces;
using Elysia.Core.Domain.interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Elysia.Core.Application.Services
{
    public class DashboardPropietarioServices : IDashboardPropietarioServices
    {

        private readonly IReservasRepository reservasRepository;
        private readonly IPedidoRepository pedidoRepository;
        private readonly IEmpleadoRepository empleadoRepository;
        private readonly IShiftRepository shiftRepository;
        private readonly IShiftEmpleadoRepository shiftEmpleadoRepository;
        private readonly IPuestoRepository puestRepository;
        private readonly IMenuRepository menuRepository;
        private readonly IPlatoMenuRepository platoMenuRepository;
        private readonly IMesaRepository mesaRepository;
        private readonly IProductoRepository productoRepository;
        private readonly IPlatoRepository platoRepository;
        private readonly IPlanRepository planRepository;
        private readonly IMembresiaRepository membresiaRepository;

        public DashboardPropietarioServices(IMembresiaRepository membresiaRepository, IPlanRepository lanRepository, IReservasRepository reservasRepository, IPedidoRepository pedidoRepository, IEmpleadoRepository empleadoRepository, IShiftRepository shiftRepository, IShiftEmpleadoRepository shiftEmpleadoRepository, IPuestoRepository puestRepository, IMenuRepository menuRepository, IPlatoMenuRepository platoMenuRepository, IMesaRepository mesaRepository, IProductoRepository productoRepository, IPlatoRepository platoRepository)
        {
            this.reservasRepository = reservasRepository;
            this.pedidoRepository = pedidoRepository;
            this.empleadoRepository = empleadoRepository;
            this.shiftRepository = shiftRepository;
            this.shiftEmpleadoRepository = shiftEmpleadoRepository;
            this.menuRepository = menuRepository;
            this.puestRepository = puestRepository;
            this.platoMenuRepository = platoMenuRepository;
            this.mesaRepository = mesaRepository;
            this.productoRepository = productoRepository;
            this.platoMenuRepository = platoMenuRepository;
            this.platoRepository = platoRepository;
            planRepository = lanRepository;
            this.membresiaRepository = membresiaRepository;

        }


        public async Task<(MostrarIndicadoresPanelPropietarioPremiumDto?, MostrarIndicadoresPanelPropietarioSimpleDto?)> GetIndicadoresPanelPropietario(string PropietarioId)
        {

            try
            {
                var menus = await menuRepository.GetListMenuByPropietarioId(PropietarioId);
                var mesas = await mesaRepository.GetAllByPropietarioId(PropietarioId);
                var platos = await platoRepository.GetAllByPropietarioId(PropietarioId);
                var productos = await productoRepository.GetListProductosByPropietarioid(PropietarioId);
                var membresia_exist = await membresiaRepository.GetMembresiaByPropietarioId(PropietarioId);
                var pedidosCancelados = await pedidoRepository.GetAllPedidosCancelados(PropietarioId);
                var pedidosFinalizados = await pedidoRepository.GetAllPedidosFinalizado(PropietarioId);
                var pedidosListo = await pedidoRepository.GetAllPedidosListo(PropietarioId);
                var pedidosEntregados = await pedidoRepository.GetAllPedidosEntregado(PropietarioId);
                var pedidoEnProceso = await pedidoRepository.GetAllPedidosEnProceso(PropietarioId);
                var pedidosPendientes = await pedidoRepository.GetAllPedidosPendiente(PropietarioId);
                var platoAsociadosMenu = await platoMenuRepository.GetlAllAsync();
                var reservasActivas = await reservasRepository.GetReservasActivasByPropietario(PropietarioId);
                var reservasFinalizada = await reservasRepository.GetReservasFinalizadasByPropietario(PropietarioId);
                var reservasCanceladas = await reservasRepository.GetReservasCanceladaByPropietario(PropietarioId);
                var reservasEnproceso = await reservasRepository.GetReservasEnProcesoByPropietario(PropietarioId);
                var reservasNoAsistio = await reservasRepository.GetReservasNoAsistioByPropietario(PropietarioId);
                var puesto = await puestRepository.GetlAllAsync();
                var empleadosActivos = await empleadoRepository.GetAllEmpleadosActivos(PropietarioId);
                var empleadosInactivos = await empleadoRepository.GetAllEmpleadosInactivo(PropietarioId);
                var turnos = await shiftRepository.GetAllTurnoByPropietarioId(PropietarioId);
                int empleadoAsociados = 0;
                int platosAsociados = 0;
                if (platos.Count() > 0)
                {
                    foreach (var item in platos)
                    {
                        var is_asociado = await platoMenuRepository.GetListByPlatoId(item.Id);
                        if (is_asociado != null)
                        {
                            platosAsociados += 1;
                        }


                    }
                }

                if (empleadosInactivos.Count() > 0 && empleadosActivos.Count() >= 0)
                {

                    empleadosActivos.AddRange(empleadosInactivos);
                    if (empleadosActivos.Count() > 0)
                    {
                        foreach (var empleado in empleadosInactivos)
                        {
                            var is_asociado = await shiftEmpleadoRepository.GetOneEmployeeShiftsByEmpleadoId(empleado.Id);
                            if (is_asociado != null)
                            {
                                empleadoAsociados += 1;
                            }
                        }
                    }

                }


                if (membresia_exist != null && membresia_exist.PlanId == 1)
                {
                    var panel_simple = new MostrarIndicadoresPanelPropietarioSimpleDto()
                    {
                        PlanId = membresia_exist.PlanId,
                        PropietarioId = PropietarioId,
                        CantidadMenu = menus.Count,
                        CantidadMesa = mesas.Count,
                        CantidadPlato = platos.Count,
                        CantidadProducto = productos.Count,
                        PedidosCancelados = pedidosCancelados.Count,
                        PedidosEnProceso = pedidoEnProceso.Count,
                        PedidosEntregado = pedidosEntregados.Count,
                        PedidosFinalizado = pedidosFinalizados.Count,
                        PedidosListo = pedidosListo.Count,
                        PedidoPendiente = pedidosPendientes.Count,
                        TotalPedidos = pedidosListo.Count + pedidoEnProceso.Count + pedidosCancelados.Count + pedidosFinalizados.Count + pedidosPendientes.Count,
                        ReservaEnProceso = reservasEnproceso.Count,
                        ReservasActivas = reservasActivas.Count,
                        ReservasCanceladas = reservasCanceladas.Count,
                        ReservasFinalizadas = reservasFinalizada.Count,
                        ReservasNoAsistio = reservasNoAsistio.Count,
                        TotalReservas = reservasNoAsistio.Count + reservasCanceladas.Count + reservasEnproceso.Count + reservasFinalizada.Count + reservasActivas.Count,
                        PlatoAsociadoAUnMenu = platosAsociados
                    };

                    return (null,panel_simple);
                }


                if (membresia_exist != null && membresia_exist.PlanId == 2)
                {
                    var panel_premium = new MostrarIndicadoresPanelPropietarioPremiumDto()
                    {

                        PlanId = membresia_exist.PlanId,
                        PropietarioId = PropietarioId,
                        CantidadMenu = menus.Count,
                        CantidadMesa = mesas.Count,
                        CantidadPlato = platos.Count,
                        CantidadProducto = productos.Count,
                        PedidosCancelados = pedidosCancelados.Count,
                        PedidosEnProceso = pedidoEnProceso.Count,
                        PedidosEntregado = pedidosEntregados.Count,
                        PedidosFinalizado = pedidosFinalizados.Count,
                        PedidosListo = pedidosListo.Count,
                        PedidoPendiente = pedidosPendientes.Count,
                        TotalPedidos = pedidosListo.Count + pedidoEnProceso.Count + pedidosCancelados.Count + pedidosFinalizados.Count + pedidosPendientes.Count,
                        ReservaEnProceso = reservasEnproceso.Count,
                        ReservasActivas = reservasActivas.Count,
                        ReservasCanceladas = reservasCanceladas.Count,
                        ReservasFinalizadas = reservasFinalizada.Count,
                        ReservasNoAsistio = reservasNoAsistio.Count,
                        TotalReservas = reservasNoAsistio.Count + reservasCanceladas.Count + reservasEnproceso.Count + reservasFinalizada.Count + reservasActivas.Count,
                        PlatoAsociadoAUnMenu = platosAsociados,
                        EmpleadoActivos = empleadosActivos!.Count,
                        EmpleadoNoActivos = empleadosInactivos.Count,
                        TotalEmpleados = empleadosActivos.Count + empleadosInactivos.Count,
                        CantidadPuesto = puesto.Count,
                        TotalTurno = turnos.Count,
                        TotalEmpleadoAsociadosTurno = empleadoAsociados
                    };

                      return (panel_premium,null);
                }

                return (null, null);
            }
            catch (Exception ex)
            {

                throw new Exception("Ocurrio un error al intentar obtener el panel del propietario" + ex.Message);

            }


        }

    }
}
